using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar.Rendering;
using System;


namespace DevComponents.DotNetBar
{
    internal class Office2007ButtonItemPainter : Office2003ButtonItemPainter, IOffice2007Painter
    {
        #region Private Variables
        private int m_CornerSize = 2;
        private Office2007ColorTable m_ColorTable = null; //new Office2007ColorTable();
        private static RoundRectangleShapeDescriptor _DefaultOffice2007Shape = new RoundRectangleShapeDescriptor(2);
        private static RoundRectangleShapeDescriptor _DefaultOffice2007LeftShape = new RoundRectangleShapeDescriptor(2, 0, 2, 0);
        private static RoundRectangleShapeDescriptor _DefaultOffice2007RightShape = new RoundRectangleShapeDescriptor(0, 2, 0, 2);
        private static RoundRectangleShapeDescriptor _DefaultOffice2007TopShape = new RoundRectangleShapeDescriptor(2, 2, 0, 0);
        private static RoundRectangleShapeDescriptor _DefaultOffice2007BottomShape = new RoundRectangleShapeDescriptor(0, 0, 2, 2);
        #endregion

        #region Implementation
        private bool IsOffice2003Rendering(ButtonItem button, ItemPaintArgs pa)
        {
            if (button.ColorTable == eButtonColor.Flat || pa.Owner is DotNetBarManager)
                return true;
            return false;
        }

        /// <summary>
        /// Paints state of the button, either hot, pressed or checked
        /// </summary>
        /// <param name="button"></param>
        /// <param name="pa"></param>
        /// <param name="image"></param>
        public override void PaintButtonMouseOver(ButtonItem button, ItemPaintArgs pa, CompositeImage image, Rectangle r)
        {
            if (IsOffice2003Rendering(button, pa))
            {
                base.PaintButtonMouseOver(button, pa, image, r);
                return;
            }
            //PaintState(button, pa, image, r, button.IsMouseDown);
        }

        public override void PaintButtonCheck(ButtonItem button, ItemPaintArgs pa, CompositeImage image, Rectangle r)
        {
            if (IsOffice2003Rendering(button, pa))
            {
                base.PaintButtonCheck(button, pa, image, r);
                return;
            }
            bool isOnMenu = IsOnMenu(button, pa);
            if (isOnMenu)
                base.PaintButtonCheck(button, pa, image, r);
            else
                PaintState(button, pa, image, r, button.IsMouseDown);
        }

        protected virtual void PaintState(ButtonItem button, ItemPaintArgs pa, CompositeImage image, Rectangle r, bool isMouseDown)
        {
            if (r.IsEmpty)
                return;

            bool isOnMenu = pa.IsOnMenu;
            Office2007ColorTable colorTable = this.ColorTable;
            
            Graphics g = pa.Graphics;
            IShapeDescriptor shape = GetButtonShape(button);

            if (pa.ContainerControl is ButtonX)
                shape = ((ButtonX)pa.ContainerControl).GetButtonShape();
            else if (pa.ContainerControl is NavigationBar)
                shape = ((NavigationBar)pa.ContainerControl).ButtonShape;

            eButtonContainer buttonCont = GetButtonContainerQualifier(pa, button);
            Office2007ButtonItemColorTable buttonColorTable = GetColorTable(button, buttonCont);

            Region oldClip = g.Clip;
            try
            {
                g.SetClip(r, CombineMode.Intersect);

                Office2007ButtonItemStateColorTable stateColors = buttonColorTable.Default;

                if (IsOnMenu(button, pa))
                {
                    Rectangle sideRect = new Rectangle(button.DisplayRectangle.Left, button.DisplayRectangle.Top, button.ImageDrawRect.Right, button.DisplayRectangle.Height);
                    if (pa.RightToLeft)
                        sideRect = new Rectangle(button.DisplayRectangle.Right - button.ImageDrawRect.Width, button.DisplayRectangle.Top, button.ImageDrawRect.Width, button.DisplayRectangle.Height);

                    PaintMenuItemSide(button, pa, sideRect);
                }

                if (!IsItemEnabled(button, pa))
                {
                    if (!isOnMenu)
                        PaintBackground(g, buttonColorTable.Disabled, r, shape, pa.IsDefaultButton);
                    g.Clip = oldClip;
                    return;
                }

                bool mouseOver = false;
                bool expanded = false;
                if (!button.DesignMode)
                {
                    if (button.IsMouseOver && button.HotTrackingStyle != eHotTrackingStyle.Image && button.HotTrackingStyle != eHotTrackingStyle.None)
                    {
                        stateColors = buttonColorTable.MouseOver;
                        mouseOver = true;
                    }

                    if (isMouseDown && button.HotTrackingStyle != eHotTrackingStyle.Image && button.HotTrackingStyle != eHotTrackingStyle.None)
                    {
                        if (buttonColorTable.Pressed != null)
                            stateColors = buttonColorTable.Pressed;
                    }
                    else if (button.Checked && !button.IsMouseOver && !IsOnMenu(button, pa))
                    {
                        stateColors = buttonColorTable.Checked;
                    }
                    else if (button.Expanded && button.HotTrackingStyle != eHotTrackingStyle.Image && !(!mouseOver && pa.IsOnMenu))
                    {
                        stateColors = buttonColorTable.Expanded;
                        expanded = true;
                    }
                }
                else
                {
                    if (button.Checked && !IsOnMenu(button, pa))
                    {
                        stateColors = buttonColorTable.Checked;
                    }
                    else if (button.Expanded && button.HotTrackingStyle != eHotTrackingStyle.Image)
                    {
                        stateColors = buttonColorTable.Expanded;
                        expanded = true;
                    }
                }

                Rectangle subRect = GetTotalSubItemsRect(button);
                bool isSplitButton = (mouseOver || expanded) && !button.AutoExpandOnClick && !subRect.IsEmpty && !IsOnMenu(button, pa);
                eDotNetBarStyle effectiveStyle = button.EffectiveStyle;
                subRect.Offset(button.DisplayRectangle.Location);

                if (isSplitButton && !button.Checked &&
                    (effectiveStyle == eDotNetBarStyle.Office2010 || effectiveStyle == eDotNetBarStyle.Windows7))
                {
                    Rectangle[] rects = DisplayHelp.ExcludeRectangle(r, subRect);
                    if (rects.Length > 0 && (button.IsMouseOverExpand || button.Expanded))
                        rects = DisplayHelp.ExcludeRectangle(r, rects[0]);
                    if (rects.Length > 0)
                    {
                        PaintBackground(g, buttonColorTable.MouseOverSplitInactive, r, shape, pa.IsDefaultButton, !pa.IsOnNavigationBar);
                        if (rects[0].X == r.X && rects[0].Right < r.Right) // Left aligned
                            shape = _DefaultOffice2007LeftShape;
                        else if (rects[0].X > r.X && rects[0].Right == r.Right) // Right aligned
                            shape = _DefaultOffice2007RightShape;
                        else if (rects[0].Y == r.Y && rects[0].Bottom < r.Bottom) // Top aligned
                            shape = _DefaultOffice2007TopShape;
                        else if (rects[0].Y > r.Y && rects[0].Bottom == r.Bottom) // Bottom aligned
                            shape = _DefaultOffice2007BottomShape;

                        PaintBackground(g, stateColors, rects[0], shape, pa.IsDefaultButton, !pa.IsOnNavigationBar);
                    }
                    else
                        PaintBackground(g, stateColors, r, shape, pa.IsDefaultButton, !pa.IsOnNavigationBar);
                }
                else
                    PaintBackground(g, stateColors, r, shape, pa.IsDefaultButton, !pa.IsOnNavigationBar);

                if (isSplitButton && !(effectiveStyle == eDotNetBarStyle.Office2010 || effectiveStyle == eDotNetBarStyle.Windows7))
                {
                    using (Brush brush = GetOverlayInactiveBrush())
                    {
                        Rectangle backRect = button.DisplayRectangle;
                        backRect.Inflate(-1, -1);

                        if (!button.IsMouseOverExpand && !expanded)
                        {
                            subRect.Inflate(-1, -1);
                            g.SetClip(subRect, CombineMode.Intersect);
                            GraphicsPath backPath = shape.GetShape(backRect);
                            if (backPath != null)
                            {
                                g.FillPath(brush, backPath);
                                backPath.Dispose();
                            }
                        }
                        else if (!mouseOver || button.IsMouseOverExpand)
                        {
                            g.SetClip(subRect, CombineMode.Exclude);
                            GraphicsPath backPath = shape.GetShape(backRect);
                            if (backPath != null)
                            {
                                g.FillPath(brush, backPath);
                                backPath.Dispose();
                            }
                        }
                    }
                }

                g.Clip = oldClip;
            }
            finally
            {
                if (oldClip != null) oldClip.Dispose();
            }
        }

        public static void PaintBackground(Graphics g, Office2007ButtonItemStateColorTable stateColors, Rectangle r, IShapeDescriptor shape)
        {
            PaintBackground(g, stateColors, r, shape, false);
        }

        public static void PaintBackground(Graphics g, Office2007ButtonItemStateColorTable stateColors, Rectangle r, IShapeDescriptor shape, bool isDefault)
        {
            PaintBackground(g, stateColors, r, shape, isDefault, true);
        }

        public static void PaintBackground(Graphics g, Office2007ButtonItemStateColorTable stateColors, Rectangle r, IShapeDescriptor shape, bool isDefault, bool paintBorder)
        {
            float topSplit = .35f;
            float bottomSplit = .65f;
            if (!shape.CanDrawShape(r)) return;

            if (stateColors != null)
            {
                Rectangle fillRectangle = r;
                Rectangle backRect = new Rectangle(fillRectangle.X, fillRectangle.Y, fillRectangle.Width, (int)(fillRectangle.Height * topSplit));
                if (!stateColors.OuterBorder.IsEmpty && paintBorder)
                    fillRectangle.Width--;
                GraphicsPath backPath = shape.GetShape(fillRectangle);
                if (stateColors.Background != null)
                {
                    if (backPath != null)
                    {
                        using (LinearGradientBrush lb = new LinearGradientBrush(fillRectangle, stateColors.Background.Start, stateColors.Background.End, stateColors.Background.GradientAngle))
                        {
                            g.FillPath(lb, backPath);
                        }
                        backPath.Dispose();
                    }
                }
                else
                {
                    if (backPath != null)
                    {
                        if (stateColors.TopBackground.End.IsEmpty && stateColors.BottomBackground.End.IsEmpty)
                        {
                            if (stateColors.TopBackground.Start == stateColors.BottomBackground.Start)
                                using (SolidBrush lb = new SolidBrush(stateColors.TopBackground.Start))
                                    g.FillPath(lb, backPath);
                            else
                            {
                                using (LinearGradientBrush lb = new LinearGradientBrush(fillRectangle, stateColors.TopBackground.Start, stateColors.BottomBackground.Start, stateColors.TopBackground.GradientAngle))
                                {
                                    g.FillPath(lb, backPath);
                                }
                            }
                        }
                        else
                        {
                            using (LinearGradientBrush lb = new LinearGradientBrush(fillRectangle, stateColors.TopBackground.Start, stateColors.BottomBackground.End, stateColors.TopBackground.GradientAngle))
                            {
                                ColorBlend cb = new ColorBlend(4);
                                cb.Colors = new Color[] { stateColors.TopBackground.Start, stateColors.TopBackground.End, stateColors.BottomBackground.Start, stateColors.BottomBackground.End };
                                cb.Positions = new float[] { 0, topSplit, topSplit, 1f };
                                lb.InterpolationColors = cb;
                                g.FillPath(lb, backPath);
                            }
                        }
                        backPath.Dispose();
                    }

                    if (!stateColors.TopBackgroundHighlight.IsEmpty)
                    {
                        Rectangle ellipse = new Rectangle(fillRectangle.X, fillRectangle.Y, fillRectangle.Width, fillRectangle.Height);
                        GraphicsPath path = new GraphicsPath();
                        path.AddEllipse(ellipse);
                        PathGradientBrush brush = new PathGradientBrush(path);
                        brush.CenterColor = stateColors.TopBackgroundHighlight.Start;
                        brush.SurroundColors = new Color[] { stateColors.TopBackgroundHighlight.End };
                        brush.CenterPoint = new PointF(ellipse.X + ellipse.Width / 2, fillRectangle.Bottom);
                        Blend blend = new Blend();
                        blend.Factors = new float[] { 0f, .5f, 1f };
                        blend.Positions = new float[] { .0f, .4f, 1f };
                        brush.Blend = blend;

                        g.FillRectangle(brush, backRect);
                        brush.Dispose();
                        path.Dispose();
                    }

                    // Draw Bottom part
                    int bottomHeight = (int)(fillRectangle.Height * bottomSplit);
                    backRect = new Rectangle(fillRectangle.X, fillRectangle.Y + backRect.Height, fillRectangle.Width, fillRectangle.Height - backRect.Height);

                    if (!stateColors.BottomBackgroundHighlight.IsEmpty)
                    {
                        Rectangle ellipse = new Rectangle(fillRectangle.X, fillRectangle.Y + bottomHeight - 2, fillRectangle.Width, fillRectangle.Height + 4);
                        GraphicsPath path = new GraphicsPath();
                        path.AddEllipse(ellipse);
                        PathGradientBrush brush = new PathGradientBrush(path);
                        brush.CenterColor = stateColors.BottomBackgroundHighlight.Start;
                        brush.SurroundColors = new Color[] { stateColors.BottomBackgroundHighlight.End };
                        brush.CenterPoint = new PointF(ellipse.X + ellipse.Width / 2, fillRectangle.Bottom);
                        Blend blend = new Blend();
                        blend.Factors = new float[] { 0f, .5f, .6f };
                        blend.Positions = new float[] { .0f, .4f, 1f };
                        brush.Blend = blend;

                        g.FillRectangle(brush, backRect);
                        brush.Dispose();
                        path.Dispose();
                    }
                }

                if (paintBorder)
                {
                    SmoothingMode sm = g.SmoothingMode;
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    // Draw Border
                    if (!stateColors.OuterBorder.IsEmpty)
                    {
                        Rectangle br = r;
                        br.Width--;
                        br.Height--;
                        GraphicsPath path = shape.GetShape(br);
                        if (path != null)
                        {
                            DisplayHelp.DrawGradientPath(g, path, r, stateColors.OuterBorder, 1);
                            path.Dispose();
                        }
                        //if (cornerSize > 1)
                        //    DisplayHelp.DrawRoundGradientRectangle(g, r, stateColors.OuterBorder, 1, cornerSize);
                        //else
                        //    DisplayHelp.DrawGradientRectangle(g, r, stateColors.OuterBorder, 1);

                        if (isDefault)
                        {
                            Color clr = Color.FromArgb(128, stateColors.OuterBorder.Start);
                            r.Inflate(-1, -1);
                            DisplayHelp.DrawRectangle(g, clr, r);
                            r.Inflate(-1, -1);
                            DisplayHelp.DrawRectangle(g, clr, r);
                        }
                    }
                    if (!isDefault && !stateColors.InnerBorder.IsEmpty)
                    {
                        Rectangle innerRect = r;
                        innerRect.Inflate(-1, -1);
                        innerRect.Width--;
                        innerRect.Height--;
                        using (GraphicsPath path = shape.GetInnerShape(innerRect, 1))
                            DisplayHelp.DrawGradientPath(g, path, innerRect, stateColors.InnerBorder, 1);
                        //cornerSize--;
                        //if (cornerSize > 1)
                        //    DisplayHelp.DrawRoundGradientRectangle(g, innerRect, stateColors.InnerBorder, 1, cornerSize);
                        //else
                        //    DisplayHelp.DrawGradientRectangle(g, innerRect, stateColors.InnerBorder, 1);
                    }

                    g.SmoothingMode = sm;
                }
            }
        }

        public override void PaintButtonBackground(ButtonItem button, ItemPaintArgs pa, CompositeImage image)
        {
            if (IsOffice2003Rendering(button, pa))
            {
                base.PaintButtonBackground(button, pa, image);
                return;
            }

            if (button is Office2007StartButton && m_ColorTable.RibbonControl.StartButtonDefault!=null)
            {
                Image backImage = m_ColorTable.RibbonControl.StartButtonDefault;
                if(button.IsMouseDown || button.Expanded)
                    backImage = m_ColorTable.RibbonControl.StartButtonPressed;
                else if(button.IsMouseOver)
                    backImage = m_ColorTable.RibbonControl.StartButtonMouseOver;
                if (backImage != null)
                {
                    if (Office2007ColorTable.CloneImagesOnAccess)
                        backImage = backImage.Clone() as Image;
                    pa.Graphics.DrawImageUnscaled(backImage, new Point(button.LeftInternal + (button.WidthInternal - backImage.Width) / 2, button.TopInternal + (button.HeightInternal - backImage.Height) / 2));
                    if (Office2007ColorTable.CloneImagesOnAccess)
                        backImage.Dispose();
                }
            }

            PaintState(button, pa, image, button.DisplayRectangle, button.IsMouseDown);
        }

        private Brush GetOverlayInactiveBrush()
        {
            return new SolidBrush(Color.FromArgb(76, Color.White));
        }

        private static IShapeDescriptor GetButtonShape(ButtonItem button)
        {
            if (button.Shape != null)
                return button.Shape;
            return _DefaultOffice2007Shape;
        }

        public override void PaintExpandButton(ButtonItem button, ItemPaintArgs pa)
        {
            if (IsOffice2003Rendering(button, pa))
            {
                base.PaintExpandButton(button, pa);
                return;
            }

            Graphics g = pa.Graphics;
            bool isOnMenu = IsOnMenu(button, pa);
            Rectangle itemRect = button.DisplayRectangle;
            bool mouseOver = button.IsMouseOver;
            bool enabled = IsItemEnabled(button, pa);

            Color textColor = Color.Empty;
            eButtonContainer buttonCont = GetButtonContainerQualifier(pa, button);
            Office2007ButtonItemColorTable buttonColorTable = GetColorTable(button, buttonCont);
            if (buttonCont == eButtonContainer.RibbonStrip)
            {
                textColor = GetTextColor(button, pa);
            }
            if(textColor.IsEmpty)
                textColor = GetTextColor(button, pa, buttonColorTable);

            using (SolidBrush textBrush = new SolidBrush(textColor))
            {
                // If it has subitems draw the triangle to indicate that
                if ((button.SubItems.Count > 0 || button.PopupType == ePopupType.Container) && button.ShowSubItems)
                {
                    if (isOnMenu)
                    {
                        Point[] p = new Point[3];
                        if (pa.RightToLeft)
                        {
                            p[0].X = itemRect.Left + 8;
                            p[0].Y = itemRect.Top + (itemRect.Height - 8) / 2;
                            p[1].X = p[0].X;
                            p[1].Y = p[0].Y + 8;
                            p[2].X = p[0].X - 4;
                            p[2].Y = p[0].Y + 4;
                        }
                        else
                        {
                            p[0].X = itemRect.Left + itemRect.Width - 12;
                            p[0].Y = itemRect.Top + (itemRect.Height - 8) / 2;
                            p[1].X = p[0].X;
                            p[1].Y = p[0].Y + 8;
                            p[2].X = p[0].X + 4;
                            p[2].Y = p[0].Y + 4;
                        }
                        SmoothingMode sm = g.SmoothingMode;
                        g.SmoothingMode = SmoothingMode.None;
                        g.FillPolygon(textBrush, p);
                        g.SmoothingMode = sm;
                    }
                    else if (!button.SubItemsRect.IsEmpty)
                    {
                        Point[] p = new Point[3];
                        Rectangle sr = button.SubItemsRect;
                        if (pa.IsOnRibbonBar && button.ImagePosition == eImagePosition.Top && !button.SplitButton && button.Text.Length > 0)
                            sr.Y -= 3;

                        if (button.PopupSide == ePopupSide.Default)
                        {
                            if (pa.IsOnMenu)
                            {
                                if (pa.RightToLeft)
                                {
                                    p[0].X = itemRect.Left + sr.Left + sr.Width / 2 + 3;
                                    p[0].Y = itemRect.Top + sr.Top + sr.Height / 2 - 3;
                                    p[1].X = p[0].X;
                                    p[1].Y = p[0].Y + 6;
                                    p[2].X = p[0].X - 3;
                                    p[2].Y = p[0].Y + 3;
                                }
                                else
                                {
                                    p[0].X = itemRect.Left + sr.Left + sr.Width / 2;
                                    p[0].Y = itemRect.Top + sr.Top + sr.Height / 2 - 3;
                                    p[1].X = p[0].X;
                                    p[1].Y = p[0].Y + 6;
                                    p[2].X = p[0].X + 3;
                                    p[2].Y = p[0].Y + 3;
                                }
                            }
                            else if (button.Orientation == eOrientation.Horizontal)
                            {
                                p[0].X = itemRect.Left + sr.Left + (sr.Width - 5) / 2;
                                p[0].Y = itemRect.Top + sr.Top + (sr.Height - 3) / 2 + 1;
                                p[1].X = p[0].X + 5;
                                p[1].Y = p[0].Y;
                                p[2].X = p[0].X + 2;
                                p[2].Y = p[0].Y + 3;
                            }
                            else
                            {
                                p[0].X = itemRect.Left + sr.Left + (sr.Width - 3) / 2 + 1;
                                p[0].Y = itemRect.Top + sr.Top + (sr.Height - 5) / 2;
                                p[1].X = p[0].X;
                                p[1].Y = p[0].Y + 6;
                                p[2].X = p[0].X - 3;
                                p[2].Y = p[0].Y + 3;
                            }
                        }
                        else
                        {
                            switch (button.PopupSide)
                            {
                                case ePopupSide.Right:
                                    {
                                        p[0].X = itemRect.Left + sr.Left + sr.Width / 2;
                                        p[0].Y = itemRect.Top + sr.Top + sr.Height / 2 - 3;
                                        p[1].X = p[0].X;
                                        p[1].Y = p[0].Y + 6;
                                        p[2].X = p[0].X + 3;
                                        p[2].Y = p[0].Y + 3;
                                        break;
                                    }
                                case ePopupSide.Left:
                                    {
                                        p[0].X = itemRect.Left + sr.Left + sr.Width / 2 + 3;
                                        p[0].Y = itemRect.Top + sr.Top + sr.Height / 2 - 3;
                                        p[1].X = p[0].X;
                                        p[1].Y = p[0].Y + 6;
                                        p[2].X = p[0].X - 3;
                                        p[2].Y = p[0].Y + 3;
                                        break;
                                    }
                                case ePopupSide.Top:
                                    {
                                        p[0].X = itemRect.Left + sr.Left + (sr.Width - 5) / 2;
                                        p[0].Y = itemRect.Top + sr.Top + (sr.Height - 3) / 2 + 4;
                                        p[1].X = p[0].X + 6;
                                        p[1].Y = p[0].Y;
                                        p[2].X = p[0].X + 3;
                                        p[2].Y = p[0].Y - 4;
                                        break;
                                    }
                                case ePopupSide.Bottom:
                                    {
                                        p[0].X = itemRect.Left + sr.Left + (sr.Width - 5) / 2 + 1;
                                        p[0].Y = itemRect.Top + sr.Top + (sr.Height - 3) / 2 + 1;
                                        p[1].X = p[0].X + 5;
                                        p[1].Y = p[0].Y;
                                        p[2].X = p[0].X + 2;
                                        p[2].Y = p[0].Y + 3;
                                        break;
                                    }
                            }
                        }

                        if (button.SplitButton && !button.TextDrawRect.IsEmpty && (button.ImagePosition == eImagePosition.Top))
                        {
                            p[0].Y -= 3;
                            p[1].Y -= 3;
                            p[2].Y -= 3;
                        }

                        if (enabled)
                        {
                            SmoothingMode sm = g.SmoothingMode;
                            g.SmoothingMode = SmoothingMode.None;
                            g.FillPolygon(textBrush, p);
                            g.SmoothingMode = sm;
                            
                        }
                        else
                        {
                            SmoothingMode sm = g.SmoothingMode;
                            g.SmoothingMode = SmoothingMode.None;
                            using (SolidBrush mybrush = new SolidBrush(pa.Colors.ItemDisabledText))
                                g.FillPolygon(mybrush, p);
                            g.SmoothingMode = sm;
                        }
                    }

                    Rectangle r = GetTotalSubItemsRect(button);
                    if (enabled && !r.IsEmpty && (mouseOver || button.Expanded && !pa.IsOnMenu) && !button.AutoExpandOnClick)
                    {
                        r.Offset(itemRect.Location);
                        Point[] pt = new Point[4];
                        if (pa.ContainerControl is RibbonBar && (button.ImagePosition == eImagePosition.Top || button.ImagePosition == eImagePosition.Bottom))
                        {
                            pt[0] = new Point(r.X + 1, r.Y);
                            pt[1] = new Point(r.Right - 2, r.Y);
                            pt[2] = new Point(r.X + 1, r.Y + 1);
                            pt[3] = new Point(r.Right - 2, r.Y + 1);
                            if (button.SplitButton)
                            {
                                pt[0].Y++;
                                pt[1].Y++;
                                pt[2].Y++;
                                pt[3].Y++;
                            }
                        }
                        else
                        {
                            r.Y += 1;
                            r.Height -= 3;
                            if (pa.RightToLeft)
                                r.X = r.Right - 1;
                            pt[0] = new Point(r.X, r.Y);
                            pt[1] = new Point(r.X, r.Bottom);
                            pt[2] = new Point(r.X + 1, r.Y);
                            pt[3] = new Point(r.X + 1, r.Bottom);
                        }
                        eDotNetBarStyle effectiveStyle = button.EffectiveStyle;
                        if (effectiveStyle != eDotNetBarStyle.Office2010 && effectiveStyle != eDotNetBarStyle.Windows7 || button.Checked)
                        {
                            using (Pen pen = new Pen(buttonColorTable.MouseOver.SplitBorder.Start))
                                g.DrawLine(pen, pt[0], pt[1]); //r.X, r.Y, r.X, r.Bottom);
                            using (Pen pen = new Pen(buttonColorTable.MouseOver.SplitBorderLight.Start))
                                g.DrawLine(pen, pt[2], pt[3]); //r.X + 1, r.Y, r.X + 1, r.Bottom);
                        }
                    }


                }
            }
        }

        protected override void PaintMenuItemSide(ButtonItem button, ItemPaintArgs pa, Rectangle sideRect)
        {
            if (IsOffice2003Rendering(button, pa))
            {
                base.PaintMenuItemSide(button, pa, sideRect);
                return;
            }

            Graphics g = pa.Graphics;
            Office2007MenuColorTable ct = m_ColorTable.Menu;
            Region oldClip = g.Clip.Clone() as Region;
            g.SetClip(sideRect);

            sideRect.Inflate(0, 1);

            // Draw side bar
            if (button.MenuVisibility == eMenuVisibility.VisibleIfRecentlyUsed && !button.RecentlyUsed)
                DisplayHelp.FillRectangle(g, sideRect, ct.SideUnused);
            else
                DisplayHelp.FillRectangle(g, sideRect, ct.Side);

            if (pa.RightToLeft)
            {
                Point p = new Point(sideRect.X, sideRect.Y);
                DisplayHelp.DrawGradientLine(g, p, new Point(p.X, p.Y + sideRect.Height), ct.SideBorder, 1);
                
                p.X++;

                DisplayHelp.DrawGradientLine(g, p, new Point(p.X, p.Y + sideRect.Height), ct.SideBorderLight, 1);
            }
            else
            {
                Point p = new Point(sideRect.Right - 2, sideRect.Y);
                DisplayHelp.DrawGradientLine(g, p, new Point(p.X, p.Y + sideRect.Height), ct.SideBorder, 1);

                p.X++;

                DisplayHelp.DrawGradientLine(g, p, new Point(p.X, p.Y + sideRect.Height), ct.SideBorderLight, 1);
            }

            if (oldClip != null)
                g.Clip = oldClip;
            else
                g.ResetClip();
        }

        protected override void PaintButtonCheckBackground(ButtonItem button, ItemPaintArgs pa, Rectangle r)
        {
            if (IsOffice2003Rendering(button, pa))
            {
                base.PaintButtonCheckBackground(button, pa, r);
                return;
            }

            Graphics g = pa.Graphics;
            bool isOnMenu = IsOnMenu(button, pa);
            int cornerSize = m_CornerSize;
            if (pa.ContainerControl is ButtonX)
                cornerSize = ((ButtonX)pa.ContainerControl).CornerSize;
            if (!button.IsMouseOver || isOnMenu)
            {
                Rectangle backRect = r;
                backRect.Inflate(-1, -1);
                DisplayHelp.FillRectangle(g, backRect, pa.Colors.ItemCheckedBackground, pa.Colors.ItemCheckedBackground2, pa.Colors.ItemCheckedBackgroundGradientAngle);
                DisplayHelp.DrawRoundedRectangle(g, pa.Colors.ItemCheckedBorder, r, cornerSize);
            }
        }

        protected virtual Office2007ButtonItemColorTable GetColorTable(ButtonItem button, eButtonContainer buttonCont)
        {
            Office2007ColorTable colorTable = this.ColorTable;
            Office2007ButtonItemColorTable buttonColorTable = null;

            if (buttonCont == eButtonContainer.RibbonBar && colorTable.RibbonButtonItemColors.Count>0)
            {
                if (button.CustomColorName != "")
                    buttonColorTable = colorTable.RibbonButtonItemColors[button.CustomColorName];

                if (buttonColorTable == null)
                    buttonColorTable = colorTable.RibbonButtonItemColors[button.GetColorTableName()];

                if (buttonColorTable != null)
                    return buttonColorTable;
            }
            else if ((buttonCont == eButtonContainer.MenuBar || buttonCont == eButtonContainer.StatusBar)&& colorTable.MenuButtonItemColors.Count > 0)
            {
                if (button.CustomColorName != "")
                    buttonColorTable = colorTable.MenuButtonItemColors[button.CustomColorName];

                if (buttonColorTable == null)
                    buttonColorTable = colorTable.MenuButtonItemColors[button.GetColorTableName()];

                if (buttonColorTable != null)
                    return buttonColorTable;
            }
            else if (buttonCont == eButtonContainer.Backstage && colorTable.BackstageButtonItemColors.Count > 0)
            {
                if (button.CustomColorName != "")
                    buttonColorTable = colorTable.BackstageButtonItemColors[button.CustomColorName];

                if (buttonColorTable == null)
                    buttonColorTable = colorTable.BackstageButtonItemColors[button.GetColorTableName()];

                if (buttonColorTable != null)
                    return buttonColorTable;
            }

            if (button.CustomColorName != "")
                buttonColorTable = colorTable.ButtonItemColors[button.CustomColorName];

            if (buttonColorTable == null)
                buttonColorTable = colorTable.ButtonItemColors[button.GetColorTableName()];

            if (buttonColorTable == null)
                return colorTable.ButtonItemColors[0];

            return buttonColorTable;
        }

        private eButtonContainer GetButtonContainerQualifier(ItemPaintArgs pa, ButtonItem button)
        {
            eButtonContainer buttonCont = eButtonContainer.None;
            if (pa.ContainerControl is RibbonBar)
                buttonCont = eButtonContainer.RibbonBar;
            else if (pa.ContainerControl is RibbonStrip)
                buttonCont = eButtonContainer.RibbonStrip;
            else if (pa.ContainerControl is Bar)
            {
                Bar bar = pa.ContainerControl as Bar;
                if (bar.MenuBar)
                    buttonCont = eButtonContainer.MenuBar;
                else if (bar.LayoutType == eLayoutType.Toolbar)
                {
                    if (bar.BarType == eBarType.StatusBar || bar.GrabHandleStyle == eGrabHandleStyle.ResizeHandle || bar.Dock == DockStyle.Bottom || bar.DockSide == eDockSide.Bottom)
                        buttonCont = eButtonContainer.StatusBar;
                    else
                        buttonCont = eButtonContainer.Toolbar;
                }
            }
            else if (pa.IsOnMenu || pa.IsOnPopupBar)
                buttonCont = eButtonContainer.Popup;
            else if (pa.ContainerControl is SuperTabStrip && ((SuperTabStrip)pa.ContainerControl).ApplicationButton != null)
                buttonCont = eButtonContainer.Backstage;

            return buttonCont;
        }

        protected virtual Color GetTextColor(ButtonItem button,  ItemPaintArgs pa, Office2007ButtonItemColorTable buttonColorTable)
        {
            Color textColor = Color.Empty;

            if (button.IsMouseOver)
            {
                if (!button.HotForeColor.IsEmpty)
                    textColor = button.HotForeColor;
            }
            else if (!button.ForeColor.IsEmpty)
                textColor = button.ForeColor;

            if (textColor.IsEmpty)
            {
                if (buttonColorTable != null)
                {
                    if (!button.GetEnabled(pa.ContainerControl))
                    {
                        if (buttonColorTable.Disabled != null)
                            textColor = buttonColorTable.Disabled.Text;
                    }
                    else if (button.IsMouseDown)
                        textColor = buttonColorTable.Pressed.Text;
                    else if (button.IsMouseOver)
                        textColor = buttonColorTable.MouseOver.Text;
                    else if (button.Expanded)
                        textColor = buttonColorTable.Expanded.Text;
                    else if (button.Checked)
                        textColor = buttonColorTable.Checked.Text;
                    else
                        textColor = buttonColorTable.Default.Text;
                }
            }

            if (textColor.IsEmpty)
                return base.GetTextColor(button, pa);

            return textColor;
        }

        protected override Color GetTextColor(ButtonItem button, ItemPaintArgs pa)
        {
            bool enabled = IsItemEnabled(button, pa);

            if (enabled && !button.ForeColor.IsEmpty)
                return button.ForeColor;

            eButtonContainer buttonCont = GetButtonContainerQualifier(pa, button);

            if (!enabled || IsOffice2003Rendering(button, pa))
            {
                if (enabled && buttonCont == eButtonContainer.MenuBar && !button.IsMouseDown && !button.IsMouseOver && !button.Expanded && !button.Checked)
                {
                    Office2007ButtonItemColorTable bt = GetColorTable(button, buttonCont);
                    return bt.Default.Text;
                }
                return base.GetTextColor(button, pa);
            }

            Color textColor = Color.Empty;
            if (buttonCont == eButtonContainer.RibbonStrip && !button.IsMouseOver && !button.IsMouseDown && !button.Expanded)
            {
                Office2007RibbonTabItemColorTable rtc = Office2007RibbonTabItemPainter.GetColorTable(m_ColorTable);
                return rtc.Default.Text;
            }

            Office2007ButtonItemColorTable buttonColorTable = GetColorTable(button, buttonCont);
            textColor = GetTextColor(button, pa, buttonColorTable);
            
            return textColor;
        }

        //public int CornerSize
        //{
        //    get { return m_CornerSize; }
        //    set { m_CornerSize = value; }
        //}
        #endregion

        #region IOffice2007Painter Members

        public Office2007ColorTable ColorTable
        {
            get
            {
                return m_ColorTable;
            }
            set
            {
                m_ColorTable = value;
            }
        }

       
        #endregion
    }

    internal enum eButtonContainer
    {
        None,
        RibbonBar,
        Popup,
        RibbonStrip,
        MenuBar,
        StatusBar,
        Toolbar,
        Backstage
    }
}
