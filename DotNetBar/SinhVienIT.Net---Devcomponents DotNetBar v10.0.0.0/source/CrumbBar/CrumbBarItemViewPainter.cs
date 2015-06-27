using System;
using System.Text;
using DevComponents.DotNetBar.Rendering;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DevComponents.DotNetBar
{
    internal class CrumbBarItemViewPainter: IOffice2007Painter
    {
        #region IOffice2007Painter
        private Office2007ColorTable _ColorTable = null;

        /// <summary>
        /// Gets or sets color table used by renderer.
        /// </summary>
        public Office2007ColorTable ColorTable
        {
            get { return _ColorTable; }
            set { _ColorTable = value; }
        }
        #endregion

        #region Internal Implementation
        public void Paint(ButtonItem item, ItemPaintArgs pa)
        {
            Paint(item, pa, _ColorTable.CrumbBarItemView);
        }

        public void Paint(ButtonItem item, ItemPaintArgs pa, CrumbBarItemViewColorTable itemColorTable)
        {
            Graphics g =pa.Graphics;

            CrumbBarItemViewStateColorTable stateTable = itemColorTable.Default;
            CrumbBarItemViewStateColorTable stateTable2 = null;
            bool isPressed = false;
            if (item.IsMouseDown || item.Expanded)
            {
                stateTable = itemColorTable.Pressed;
                isPressed = true;
            }
            else if (item.IsMouseOverExpand)
            {
                stateTable = itemColorTable.MouseOverInactive;
                stateTable2 = itemColorTable.MouseOver;
            }
            else if (item.IsMouseOver)
                stateTable = itemColorTable.MouseOver;
            
            Rectangle rect = item.DisplayRectangle;
            rect.Width--;
            rect.Height--;
            Rectangle expandRect = item.GetTotalSubItemsRect();
            if (!expandRect.IsEmpty)
            {
                expandRect.Offset(rect.Location);
                expandRect.Width--;
                expandRect.Height--;
            }

            PaintBackground(item, g, stateTable, stateTable2, isPressed, ref rect, ref expandRect);

            Color textColor = stateTable.Foreground;
            if (!item.ForeColor.IsEmpty)
                textColor = item.ForeColor;
            if (!textColor.IsEmpty)
            {
                // Render text
                Font font = item.GetFont(pa, false);
                bool rightToLeft = pa.RightToLeft;
                rect = GetTextRectangle(item);
                eTextFormat stringFormat = eTextFormat.Left | eTextFormat.VerticalCenter | eTextFormat.HidePrefix;
                if (item.TextMarkupBody == null)
                {
                    TextDrawing.DrawString(g, ButtonItemPainter.GetDrawText(item.Text), font, textColor, rect, stringFormat);
                }
                else
                {
                    TextMarkup.MarkupDrawContext d = new TextMarkup.MarkupDrawContext(g, font, textColor, rightToLeft);
                    d.HotKeyPrefixVisible = !((stringFormat & eTextFormat.HidePrefix) == eTextFormat.HidePrefix);
                    d.ContextObject = item;
                    Rectangle mr = new Rectangle(rect.X, rect.Y + (rect.Height - item.TextMarkupBody.Bounds.Height) / 2 + 1, item.TextMarkupBody.Bounds.Width, item.TextMarkupBody.Bounds.Height);
                    item.TextMarkupBody.Bounds = mr;
                    item.TextMarkupBody.Render(d);
                }

                if ((item.SubItems.Count > 0 || item.PopupType == ePopupType.Container) && item.ShowSubItems)
                {
                    // Render expand sign
                    GraphicsPath path = GetExpandPath(item, expandRect);
                    if (path != null)
                    {
                        SmoothingMode sm = g.SmoothingMode;
                        g.SmoothingMode = SmoothingMode.Default;
                        using(Brush brush=new SolidBrush(stateTable.Foreground))
                            g.FillPath(brush, path);
                        g.SmoothingMode = sm;
                    }
                }
            }
        }

        private static void PaintBackground(ButtonItem item, Graphics g, CrumbBarItemViewStateColorTable stateTable, CrumbBarItemViewStateColorTable stateTable2, bool isPressed, ref Rectangle rect, ref Rectangle expandRect)
        {
            if (stateTable.Background != null && stateTable.Background.Count > 0)
            {
                using (Brush brush = DisplayHelp.CreateBrush(rect, stateTable.Background, 90, eGradientType.Linear))
                    g.FillRectangle(brush, rect);
                if (item.IsMouseOverExpand && stateTable2 != null && stateTable2.Background != null && stateTable2.Background.Count > 0)
                {
                    using (Brush brush = DisplayHelp.CreateBrush(rect, stateTable2.Background, 90, eGradientType.Linear))
                        g.FillRectangle(brush, expandRect);
                }
            }

            if (!stateTable.Border.IsEmpty)
            {
                using (Pen pen = new Pen(stateTable.Border, 1))
                {
                    g.DrawLine(pen, rect.X, rect.Y, rect.X, rect.Bottom);
                    Pen pen2 = stateTable2 != null ? new Pen(stateTable2.Border, 1) : pen;
                    if (!expandRect.IsEmpty)
                    {
                        g.DrawLine(pen2, expandRect.X, expandRect.Y, expandRect.X, expandRect.Bottom);
                    }
                    if (isPressed)
                        g.DrawLine(pen, rect.X, rect.Y, rect.Right, rect.Y);

                    g.DrawLine(pen2, rect.Right, rect.Y, rect.Right, rect.Bottom);
                    if (stateTable2 != null) pen2.Dispose();
                }
            }

            if (!stateTable.BorderLight.IsEmpty)
            {
                Rectangle rectLight = rect;
                if (!expandRect.IsEmpty)
                    rectLight.Width -= expandRect.Width;
                rectLight.Inflate(-1, 0);
                using (Pen pen = new Pen(stateTable.BorderLight, 1))
                {
                    if (isPressed)
                    {
                        g.DrawLine(pen, rectLight.X, rectLight.Y, rectLight.Right, rectLight.Y);
                        g.DrawLine(pen, rectLight.X, rectLight.Y, rectLight.X, rectLight.Bottom);
                        if (!expandRect.IsEmpty)
                        {
                            rectLight = expandRect;
                            rectLight.Inflate(-1, 0);
                            g.DrawLine(pen, rectLight.X, rectLight.Y, rectLight.Right, rectLight.Y);
                            g.DrawLine(pen, rectLight.X, rectLight.Y, rectLight.X, rectLight.Bottom);
                        }
                    }
                    else
                    {
                        g.DrawRectangle(pen, rectLight);
                        if (!expandRect.IsEmpty)
                        {
                            rectLight = expandRect;
                            rectLight.Inflate(-1, 0);
                            g.DrawRectangle(pen, rectLight);
                        }
                    }
                }
            }
        }

        private GraphicsPath GetExpandPath(ButtonItem item, Rectangle expandRect)
        {
            GraphicsPath path = new GraphicsPath();
            if (item.Expanded)
            {
                Point p = new Point(expandRect.X + (expandRect.Width - 6) / 2, expandRect.Y + (expandRect.Height - 1) / 2);
                if (item.IsMouseDown) p.Offset(1, 1);
                path.AddLine(p.X, p.Y, p.X + 8, p.Y);
                path.AddLine(p.X + 8, p.Y, p.X + 4, p.Y + 4);
                path.CloseAllFigures();
            }
            else
            {
                Point p = new Point(expandRect.X + (expandRect.Width - 2) / 2, expandRect.Y + (expandRect.Height - 7) / 2);
                if (item.IsMouseDown) p.Offset(1, 1);
                path.AddLine(p.X, p.Y, p.X, p.Y + 8);
                path.AddLine(p.X, p.Y + 8, p.X + 4, p.Y + 4);
                path.CloseAllFigures();
            }
            return path;
        }

        protected virtual Rectangle GetTextRectangle(ButtonItem button)
        {
            Rectangle itemRect = button.DisplayRectangle;
            Rectangle textRect = button.TextDrawRect;
            //Rectangle imageRect = button.ImageDrawRect;

            textRect.Offset(itemRect.Left, itemRect.Top);

            if (textRect.Right > itemRect.Right)
                textRect.Width = itemRect.Right - textRect.Left;
            textRect.X += 2;
            textRect.Y--;
            if (button.IsMouseDown) textRect.Offset(1, 1);
            return textRect;
        }

        public void PaintOverflowButton(ButtonItem item, ItemPaintArgs pa)
        {
            PaintOverflowButton(item, pa, _ColorTable.CrumbBarItemView);
        }

        public void PaintOverflowButton(ButtonItem item, ItemPaintArgs pa, CrumbBarItemViewColorTable itemColorTable)
        {
            Graphics g =pa.Graphics;

            CrumbBarItemViewStateColorTable stateTable = itemColorTable.Default;
            CrumbBarItemViewStateColorTable stateTable2 = null;
            bool isPressed = false;
            if (item.IsMouseDown || item.Expanded)
            {
                stateTable = itemColorTable.Pressed;
                isPressed = true;
            }
            else if (item.IsMouseOver)
                stateTable = itemColorTable.MouseOver;
            
            Rectangle rect = item.DisplayRectangle;
            rect.Width--;
            rect.Height--;
            Rectangle expandRect = Rectangle.Empty;

            PaintBackground(item, g, stateTable, stateTable2, isPressed, ref rect, ref expandRect);

            Color textColor = stateTable.Foreground;
            if (!textColor.IsEmpty)
            {
                SmoothingMode sm = g.SmoothingMode;
                Point p = new Point(rect.X + (rect.Width - 7) / 2, rect.Y + (rect.Height - 5) / 2);
                if (isPressed)
                    p.Offset(1, 1);
                using (Pen pen = new Pen(textColor, 1))
                {
                    for (int i = 0; i < 2; i++)
                    {
                        g.DrawLine(pen, p.X + 3, p.Y, p.X, p.Y + 2);
                        g.DrawLine(pen, p.X, p.Y + 2, p.X + 3, p.Y + 4);
                        p.X += 4;
                    }
                }
            }
        }
        #endregion
    }
}
