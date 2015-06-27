using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar.Rendering;
using DevComponents.DotNetBar.Ribbon;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Summary description for Office2003ButtonItemPainter.
	/// </summary>
	internal class Office2003ButtonItemPainter:ButtonItemPainter
    {
        protected virtual Color GetTextColor(ButtonItem button, ItemPaintArgs pa)
        {
            return ButtonItemPainterHelper.GetTextColor(button, pa);
        }

		public override void PaintButton(ButtonItem button, ItemPaintArgs pa)
        {
			bool isOnMenu=IsOnMenu(button, pa);
            if(isOnMenu && button.Parent is ItemContainer)
                isOnMenu=false;
			bool bIsOnMenuBar=pa.IsOnMenuBar;
			bool bThemed=button.IsThemed;
            Graphics g = pa.Graphics;

            Region oldClip = g.Clip as Region;
            try
            {
                g.SetClip(button.DisplayRectangle, CombineMode.Intersect);

                if (!pa.IsOnMenu && !bIsOnMenuBar && bThemed)
                {
                    if (pa.ContainerControl is ButtonX)
                        ThemedButtonXPainter.PaintButton(button, pa);
                    else
                        ThemedButtonItemPainter.PaintButton(button, pa);
                    return;
                }

                Rectangle itemRect = button.DisplayRectangle;

                CompositeImage image = button.GetImage();
                Rectangle imageRect = GetImageRectangle(button, pa, image);

                PaintButtonBackground(button, pa, image);

                Rectangle customizeCheckRect = GetCustomizeMenuCheckRectangle(button, pa);
                Rectangle checkRect = GetCheckRectangle(button, pa, image);
                Rectangle mouseOverRect = GetMouseOverRectangle(button, pa, image);

                bool mouseOver = button.IsMouseOver;
                if (button.Expanded && !isOnMenu)
                    mouseOver = false;
                if (isOnMenu && button.Expanded && pa.ContainerControl != null && pa.ContainerControl.Parent != null)
                {
                    if (!pa.ContainerControl.Parent.Bounds.Contains(System.Windows.Forms.Control.MousePosition))
                        mouseOver = true;
                }

                if (button.HotTrackingStyle != eHotTrackingStyle.None)
                {
                    if (mouseOver || button.IsMouseDown && !button.DesignMode)
                        PaintButtonMouseOver(button, pa, image, mouseOverRect);
                }

                if (isOnMenu && button.IsOnCustomizeMenu && button.Visible && !button.SystemItem)
                    PaintCustomizeCheck(button, pa, customizeCheckRect);

                if (button.Checked && !button.IsOnCustomizeMenu && (button.GetEnabled(pa.ContainerControl) || isOnMenu))
                    PaintButtonCheck(button, pa, image, checkRect);

                if (image != null && button.ButtonStyle != eButtonStyle.TextOnlyAlways)
                    PaintButtonImage(button, pa, image, imageRect);

                Color textColor = GetTextColor(button, pa);
                if(!(button is Office2007StartButton) || button.EffectiveStyle == eDotNetBarStyle.Office2010)
                    PaintButtonText(button, pa, textColor, image);

                PaintExpandButton(button, pa);

                if (button.Focused && button.DesignMode)
                {
                    Rectangle r = itemRect;
                    r.Inflate(-1, -1);
                    DesignTime.DrawDesignTimeSelection(g, r, pa.Colors.ItemDesignTimeBorder);
                }

                if (image != null)
                    image.Dispose();
            }
            finally
            {
                if (oldClip != null)
                {
                    g.Clip = oldClip;
                    oldClip.Dispose();
                }
                else
                    g.ResetClip();
            }
		}

        protected virtual bool IsOnMenu(ButtonItem button, ItemPaintArgs pa)
        {
            bool isOnMenu = pa.IsOnMenu;
            if (isOnMenu && button.Parent is ItemContainer)
                isOnMenu = false;
            return isOnMenu;
        }


		public override void PaintButtonImage(ButtonItem button, ItemPaintArgs pa, CompositeImage image, Rectangle imagebounds)
		{
            bool isOnMenu = IsOnMenu(button, pa);
            if (imagebounds.Width <= 0 || imagebounds.Height <= 0) return;

			if(isOnMenu)
				image.DrawImage(pa.Graphics,imagebounds);
			else if(!button.IsMouseOver && button.HotTrackingStyle==eHotTrackingStyle.Color)
			{
				// Draw gray-scale image for this hover style...
				float[][] array = new float[5][];
				array[0] = new float[5] {0.2125f, 0.2125f, 0.2125f, 0, 0};
				array[1] = new float[5] {0.5f, 0.5f, 0.5f, 0, 0};
				array[2] = new float[5] {0.0361f, 0.0361f, 0.0361f, 0, 0};
				array[3] = new float[5] {0,       0,       0,       1, 0};
				array[4] = new float[5] {0.2f,    0.2f,    0.2f,    0, 1};
				ColorMatrix grayMatrix = new ColorMatrix(array);
				ImageAttributes att = new ImageAttributes();
				att.SetColorMatrix(grayMatrix);
				image.DrawImage(pa.Graphics,imagebounds,0,0,image.Width,image.Height,GraphicsUnit.Pixel,att);
			}
//			else if(!button.IsMouseOver && !image.IsIcon)
//			{
//				// Draw image little bit lighter, I decied to use gamma it is easy
//				ImageAttributes lightImageAttr = new ImageAttributes();
//				lightImageAttr.SetGamma(.7f,ColorAdjustType.Bitmap);
//				image.DrawImage(pa.Graphics,imagebounds,0,0,image.Width,image.Height,GraphicsUnit.Pixel,lightImageAttr);
//			}
			else
			{
				image.DrawImage(pa.Graphics,imagebounds);
			}
		}

		public override Rectangle GetImageRectangle(ButtonItem button, ItemPaintArgs pa, CompositeImage image)
		{
			Rectangle imageRect=Rectangle.Empty;
            bool isOnMenu = IsOnMenu(button, pa);

			// Calculate image position
			if(image!=null)
			{
				Size imageSize=button.ImageSize;

                if (pa.RightToLeft && isOnMenu)
                {
                    imageRect = new Rectangle(button.DisplayRectangle.Right - (button.ImageDrawRect.X + imageSize.Width+2), button.ImageDrawRect.Y + button.DisplayRectangle.Y, imageSize.Width, imageSize.Height);
                }
                else
                {
                    if (!isOnMenu && (button.ImagePosition == eImagePosition.Top || button.ImagePosition == eImagePosition.Bottom))
                        imageRect = new Rectangle(button.ImageDrawRect.X, button.ImageDrawRect.Y, button.DisplayRectangle.Width, button.ImageDrawRect.Height);
                    else
                        imageRect = new Rectangle(button.ImageDrawRect.X, button.ImageDrawRect.Y, button.ImageDrawRect.Width, button.ImageDrawRect.Height);
                    imageRect.Offset(button.DisplayRectangle.Left, button.DisplayRectangle.Top);
                    if ((button.EffectiveStyle == eDotNetBarStyle.Windows7 || button.EffectiveStyle == eDotNetBarStyle.Office2010) && button is Office2007StartButton)
                        imageRect.Offset(14, (imageRect.Height - imageSize.Height) / 2);
                    else
                        imageRect.Offset((imageRect.Width - imageSize.Width) / 2, (imageRect.Height - imageSize.Height) / 2);

                    imageRect.Width = imageSize.Width;
                    imageRect.Height = imageSize.Height;
                }
			}

			return imageRect;
		}

		public override Rectangle GetCheckRectangle(ButtonItem button, ItemPaintArgs pa, CompositeImage image)
		{
			Rectangle r=Rectangle.Empty;
            bool isOnMenu = IsOnMenu(button, pa);

			if(isOnMenu)
			{
				//r=new Rectangle(button.DisplayRectangle.X+1,button.DisplayRectangle.Y,button.ImageDrawRect.Width-2,button.DisplayRectangle.Height);
				//r.Inflate(-1,-1);
                // This modification fixes the bug of the checkbox appearing on the wrong side of the button item in RightToLeft mode, thanks Brian!
                if (pa.RightToLeft)
                {
                    r = new Rectangle(button.DisplayRectangle.Right - button.ImageDrawRect.Width + 1, button.DisplayRectangle.Y, button.ImageDrawRect.Width - 2, button.DisplayRectangle.Height);
                    r.Inflate(-1, -1);
                }
                else
                {
                    // LTR also adjusted by one pixel from original alignment to give more logical position.
                    // TODO: Find out if MS Office 2007 positions like this or like the original. Original might be right.
                    r = new Rectangle(button.DisplayRectangle.X, button.DisplayRectangle.Y, button.ImageDrawRect.Width - 2, button.DisplayRectangle.Height);
                    r.Inflate(-1, -1);
                }
			}
			else if(button.HotTrackingStyle==eHotTrackingStyle.Image && image!=null)
			{
				r=GetImageRectangle(button,pa,image);
				r.Inflate(2,2);
			}
			else
				r=button.DisplayRectangle;

			return r;
		}

		public override Rectangle GetCustomizeMenuCheckRectangle(ButtonItem button, ItemPaintArgs pa)
		{
            bool isOnMenu = IsOnMenu(button, pa);
			Rectangle r=Rectangle.Empty;
			if(isOnMenu && button.IsOnCustomizeMenu && button.Visible && !button.SystemItem)
			{
				r=new Rectangle(button.DisplayRectangle.Left,button.DisplayRectangle.Top,button.DisplayRectangle.Height,button.DisplayRectangle.Height);
				r.Inflate(-1,-1);
				//System.Diagnostics.Trace.WriteLine("r="+r.ToString()+"  "+button.Text+"  "+button.ImageDrawRect.ToString()+"   "+button.DisplayRectangle.ToString());
			}

			return r;
		}

		public override void PaintCustomizeCheck(ButtonItem button, ItemPaintArgs pa, Rectangle r)
		{
			Color clr=pa.Colors.ItemCheckedBackground;
			Graphics g=pa.Graphics;

			if(button.IsMouseOver && !pa.Colors.ItemHotBackground2.IsEmpty)
			{
				using(LinearGradientBrush gradient=BarFunctions.CreateLinearGradientBrush(r,pa.Colors.ItemHotBackground,pa.Colors.ItemHotBackground2,pa.Colors.ItemHotBackgroundGradientAngle))
				{
					g.FillRectangle(gradient,r);
				}
			}
			else
			{
				if(button.IsMouseOver)
					clr=pa.Colors.ItemHotBackground;

				if(!pa.Colors.ItemCheckedBackground2.IsEmpty && !button.IsMouseOver)
				{
					using(LinearGradientBrush gradient=BarFunctions.CreateLinearGradientBrush(r,pa.Colors.ItemCheckedBackground,pa.Colors.ItemCheckedBackground2,pa.Colors.ItemCheckedBackgroundGradientAngle))
						g.FillRectangle(gradient,r);
				}
				else
				{
					using(SolidBrush brush=new SolidBrush(clr))
						g.FillRectangle(brush,r);
				}
			}

			Pen objPen=new Pen(pa.Colors.ItemCheckedBorder,1);
			DisplayHelp.DrawRectangle(g,objPen,r);

			objPen.Dispose();
			objPen=new Pen(pa.Colors.ItemCheckedText);
			// Draw checker...
			Point[] pt=new Point[3];
			pt[0].X=r.Left+(r.Width-5)/2-1;
			pt[0].Y=r.Top+(r.Height-6)/2+3;
			pt[1].X=pt[0].X+2;
			pt[1].Y=pt[0].Y+2;
			pt[2].X=pt[1].X+4;
			pt[2].Y=pt[1].Y-4;
			g.DrawLines(objPen,pt);
			pt[0].X++;
			pt[1].X++;
			pt[2].X++;
			g.DrawLines(objPen,pt);
			objPen.Dispose();
		}

		/// <summary>
		/// Paints state of the button, either hot, pressed or checked
		/// </summary>
		/// <param name="button"></param>
		/// <param name="pa"></param>
		/// <param name="image"></param>
		public override void PaintButtonMouseOver(ButtonItem button, ItemPaintArgs pa, CompositeImage image, Rectangle r)
		{
			bool isMouseDown=button.IsMouseDown;
            bool isOnMenu = IsOnMenu(button, pa);
			Graphics g=pa.Graphics;

			Brush brush=null;
			Pen pen=null;

			if(isMouseDown && !isOnMenu)
			{
				if(pa.Colors.ItemPressedBackground2.IsEmpty)
					brush=new SolidBrush(pa.Colors.ItemPressedBackground);
				else
					brush=BarFunctions.CreateLinearGradientBrush(r,pa.Colors.ItemPressedBackground,pa.Colors.ItemPressedBackground2,pa.Colors.ItemPressedBackgroundGradientAngle);
				pen=new Pen(pa.Colors.ItemPressedBorder,1);
			}
			else
			{
				if(IsItemEnabled(button, pa))
				{
					if(!pa.Colors.ItemHotBackground2.IsEmpty)
						brush=BarFunctions.CreateLinearGradientBrush(r,pa.Colors.ItemHotBackground,pa.Colors.ItemHotBackground2,pa.Colors.ItemHotBackgroundGradientAngle);
					else
						brush=new SolidBrush(pa.Colors.ItemHotBackground);
                    pen = new Pen(pa.Colors.ItemHotBorder, 1);
				}
                else if(isOnMenu)
				    pen=new Pen(pa.Colors.ItemHotBorder,1);
			}

			if(brush!=null)
			{
                //Rectangle rf=r;
                //rf.Width--;
                //rf.Height--;
				g.FillRectangle(brush,r);
			}
			if(pen!=null)
				DisplayHelp.DrawRectangle(g,pen,r);

			if(brush!=null)
				brush.Dispose();
			if(pen!=null)
				pen.Dispose();
		}

        protected virtual void PaintButtonCheckBackground(ButtonItem button, ItemPaintArgs pa, Rectangle r)
        {
            Graphics g = pa.Graphics;
            bool isOnMenu = IsOnMenu(button, pa);
            if (!button.IsMouseOver || isOnMenu)
            {
                DisplayHelp.FillRectangle(g, r, pa.Colors.ItemCheckedBackground, pa.Colors.ItemCheckedBackground2, pa.Colors.ItemCheckedBackgroundGradientAngle);
                DisplayHelp.DrawRectangle(g, pa.Colors.ItemCheckedBorder, r);
            }
        }

		public override void PaintButtonCheck(ButtonItem button, ItemPaintArgs pa, CompositeImage image, Rectangle r)
		{
			if(r.IsEmpty)
				return;
            bool isOnMenu = IsOnMenu(button, pa);
			Graphics g=pa.Graphics;

            PaintButtonCheckBackground(button, pa, r);

			if((image==null || button.ButtonStyle==eButtonStyle.TextOnlyAlways) && isOnMenu)
			{
				// Draw checker...
                using (Pen pen = new Pen((IsItemEnabled(button, pa) ? pa.Colors.ItemCheckedText : pa.Colors.ItemDisabledText)))
                {
                    Point[] pt = new Point[3];
                    pt[0].X = r.Left + (r.Width - 5) / 2 - 1;
                    pt[0].Y = r.Top + (r.Height - 6) / 2 + 3;
                    pt[1].X = pt[0].X + 2;
                    pt[1].Y = pt[0].Y + 2;
                    pt[2].X = pt[1].X + 4;
                    pt[2].Y = pt[1].Y - 4;
                    g.DrawLines(pen, pt);
                    pt[0].X++;
                    //pt[0].Y
                    pt[1].X++;
                    //pt[1].Y;
                    pt[2].X++;
                    //pt[2].Y;
                    g.DrawLines(pen, pt);
                }
			}
		}

		public override Rectangle GetMouseOverRectangle(ButtonItem button, ItemPaintArgs pa, CompositeImage image)
		{
			Rectangle r=button.DisplayRectangle;

			if(button.HotTrackingStyle==eHotTrackingStyle.None || button.HotTrackingStyle==eHotTrackingStyle.Color)
				return Rectangle.Empty;

			if(button.HotTrackingStyle==eHotTrackingStyle.Image && image!=null)
			{
				r=GetImageRectangle(button,pa,image);
				r.Inflate(2,2);
				return r;
			}

            bool isOnMenu = IsOnMenu(button, pa);
			if(isOnMenu)
			{
				r.X++;
				r.Width-=2;
			}

			return r;
		}

		public override eTextFormat GetStringFormat(ButtonItem button, ItemPaintArgs pa, CompositeImage image)
		{
			eTextFormat stringFormat=pa.ButtonStringFormat;
            bool isOnMenu = IsOnMenu(button, pa);
			if(!isOnMenu)
			{
				if(pa.ContainerControl is RibbonStrip && (image==null || button.ImagePosition==eImagePosition.Top || button.ImagePosition==eImagePosition.Bottom)
                    || button._FixedSizeCenterText)
					stringFormat |= eTextFormat.HorizontalCenter;
                else if (pa.ContainerControl is ButtonX)
                {
                    ButtonX buttonX = pa.ContainerControl as ButtonX;
                    if(buttonX.TextAlignment== eButtonTextAlignment.Center)
                        stringFormat |= eTextFormat.HorizontalCenter;
                    else if(buttonX.TextAlignment== eButtonTextAlignment.Left)
                        stringFormat |= eTextFormat.Left;
                    else if (buttonX.TextAlignment == eButtonTextAlignment.Right && (image==null || button.ImagePosition == eImagePosition.Top || button.ImagePosition == eImagePosition.Bottom))
                        stringFormat |= eTextFormat.Right;
                }
                else if (pa.IsOnMenuBar || (pa.ContainerControl is Bar || pa.ContainerControl is ButtonX || button.Orientation == eOrientation.Vertical) && image == null || button.ImagePosition == eImagePosition.Top || button.ImagePosition == eImagePosition.Bottom)
                    stringFormat |= eTextFormat.HorizontalCenter;
                
                if(pa.ContainerControl is RibbonBar && image!=null && button.ImagePosition==eImagePosition.Top)
                stringFormat|= eTextFormat.WordBreak;
			}
            if (pa.RightToLeft) stringFormat |= eTextFormat.RightToLeft;
            
			return stringFormat;
		}

        protected virtual bool IsTextCentered(ButtonItem button, ItemPaintArgs pa, CompositeImage image)
        {
            bool isOnMenu = IsOnMenu(button, pa);
            if (!isOnMenu)
            {
                System.Windows.Forms.Control cc = pa.ContainerControl;
                if (cc is RibbonStrip && (image == null || button.ImagePosition == eImagePosition.Top || button.ImagePosition == eImagePosition.Bottom) || button.Name.StartsWith("sysgallery") || button._FixedSizeCenterText)
                    return true;
                else if (cc is ButtonX)
                {
                    ButtonX buttonX = cc as ButtonX;
                    if (buttonX.TextAlignment == eButtonTextAlignment.Center)
                        return true;
                }
                else if (pa.IsOnMenuBar || cc is Bar && image == null || button.ImagePosition == eImagePosition.Top || button.ImagePosition == eImagePosition.Bottom)
                    return true;
            }

            return false;
        }

        protected virtual Rectangle GetTextRectangle(ButtonItem button, ItemPaintArgs pa, eTextFormat stringFormat, CompositeImage image)
        {
            Graphics g = pa.Graphics;
            bool isOnMenu = IsOnMenu(button, pa);
            bool isOnMenuBar = pa.IsOnMenuBar;
            Rectangle itemRect = button.DisplayRectangle;
            Rectangle textRect = button.TextDrawRect;
            Rectangle imageRect = button.ImageDrawRect;
            
            bool rightToLeft = pa.RightToLeft;

            // Draw menu item text
            if (isOnMenu || button.ButtonStyle != eButtonStyle.Default || image == null || (!isOnMenu && (button.ImagePosition == eImagePosition.Top || button.ImagePosition == eImagePosition.Bottom)))
            {
                if (isOnMenu)
                {
                    if (rightToLeft)
                    {
                        //rect = new Rectangle(26, button.TextDrawRect.Y, itemRect.Width - button.ImageDrawRect.Width - 28, button.TextDrawRect.Height);
                        // This seems to give a better alignment match with the corresponding LTR alignment
                        if(button.IsOnCustomizeMenu)
                            textRect = new Rectangle(itemRect.Height, textRect.Y, itemRect.Width - imageRect.Width - 11 - itemRect.Height, textRect.Height);
                        else
                            textRect = new Rectangle(17, textRect.Y, itemRect.Width - imageRect.Width - 28, textRect.Height);
                    }
                    else
                        textRect = new Rectangle(textRect.X, textRect.Y, itemRect.Width - imageRect.Right - 26, textRect.Height);
                }
                else
                {
                    //rect = button.TextDrawRect;
                    if (button.ImagePosition == eImagePosition.Top || button.ImagePosition == eImagePosition.Bottom)
                    {
                        if (button.Orientation != eOrientation.Vertical)
                        {
                            textRect = new Rectangle(1, textRect.Y, itemRect.Width - 2, textRect.Height);
                            if (button.SplitButton || pa.ContainerControl is RibbonBar) textRect.Y += 3;
                        }
                    }
                }

                if (image == null && (stringFormat & eTextFormat.HorizontalCenter) != eTextFormat.HorizontalCenter &&
                    !isOnMenu && !isOnMenuBar && textRect.X == 0 && !pa.RightToLeft)
                    textRect.X = 3;

                textRect.Offset(itemRect.Left, itemRect.Top);

                if (button.Orientation == eOrientation.Vertical && !isOnMenu)
                {
                    if (textRect.Bottom > itemRect.Bottom)
                        textRect.Height = itemRect.Bottom - textRect.Y;
                }
                else
                {
                    if (textRect.Right > itemRect.Right)
                        textRect.Width = itemRect.Right - textRect.Left;
                }
            }

            return textRect;
        }

		public override void PaintButtonText(ButtonItem button, ItemPaintArgs pa, Color textColor, CompositeImage image)
		{
            if (!button.RenderText) return;
			Graphics g=pa.Graphics;
			eTextFormat stringFormat=GetStringFormat(button,pa,image);
            bool isOnMenu = IsOnMenu(button, pa);
			bool isOnMenuBar=pa.IsOnMenuBar;
			
            Rectangle itemRect=button.DisplayRectangle;
			Rectangle rect=GetTextRectangle(button, pa, stringFormat, image);
			
			Font font=button.GetFont(pa, false);
            bool rightToLeft = pa.RightToLeft;

            //if (isOnMenu && rightToLeft || (stringFormat & eTextFormat.Left) == eTextFormat.Left)
            //    stringFormat |=eTextFormat.Right;

			// Draw menu item text
			if(isOnMenu || button.ButtonStyle!=eButtonStyle.Default || image==null || (!isOnMenu && (button.ImagePosition==eImagePosition.Top || button.ImagePosition==eImagePosition.Bottom)))
			{
				if(button.Orientation==eOrientation.Vertical && !isOnMenu)
				{
					g.RotateTransform(90);
                    if (button.TextMarkupBody == null)
                    {
                        TextDrawing.DrawStringLegacy(g, GetDrawText(button.Text), font, textColor, new Rectangle(rect.Top, -rect.Right, rect.Height, rect.Width), stringFormat);
                    }
                    else
                    {
                        TextMarkup.MarkupDrawContext d = new TextMarkup.MarkupDrawContext(g, font, textColor, rightToLeft);
                        d.HotKeyPrefixVisible = !((stringFormat & eTextFormat.HidePrefix) == eTextFormat.HidePrefix);
                        button.TextMarkupBody.Bounds = new Rectangle(rect.Top, -rect.Right, button.TextMarkupBody.Bounds.Width, button.TextMarkupBody.Bounds.Height);
                        button.TextMarkupBody.Render(d);
                    }

					g.ResetTransform();
				}
				else
				{
                    if (button.TextMarkupBody == null)
                    {
						#if FRAMEWORK20
                        if (pa.GlassEnabled && (button.Parent is CaptionItemContainer || button.Parent is RibbonTabItemContainer && button.EffectiveStyle == eDotNetBarStyle.Office2010) && !(pa.ContainerControl is QatToolbar))
                        {
                            if (!pa.CachedPaint)
                                Office2007RibbonControlPainter.PaintTextOnGlass(g, button.Text, font, rect, TextDrawing.GetTextFormat(stringFormat), textColor, true, !button.IsMouseOver && !ColorFunctions.IsEqual(textColor, Color.White), 7);
                        }
                        else
						#endif
                            TextDrawing.DrawString(g, GetDrawText(button.Text), font, textColor, rect, stringFormat);
                    }
                    else
                    {
                        TextMarkup.MarkupDrawContext d = new TextMarkup.MarkupDrawContext(g, font, textColor, rightToLeft);
                        d.HotKeyPrefixVisible = !((stringFormat & eTextFormat.HidePrefix) == eTextFormat.HidePrefix);
                        d.ContextObject = button;
                        Rectangle mr = new Rectangle((rightToLeft && isOnMenu?rect.X+rect.Width-button.TextMarkupBody.Bounds.Width:rect.X), rect.Y + (rect.Height - button.TextMarkupBody.Bounds.Height) / 2 /*- (isOnMenu ? -1 : -1)*/+1, button.TextMarkupBody.Bounds.Width, button.TextMarkupBody.Bounds.Height);
                        if (IsTextCentered(button, pa, image))
                            mr.Offset((rect.Width - mr.Width) / 2, 0);
                        if (button._FixedSizeCenterText) mr.Y--;
                        button.TextMarkupBody.Bounds = mr;
                        button.TextMarkupBody.Render(d);
                    }

					if(!button.DesignMode && button.Focused && !isOnMenu && !isOnMenuBar && !(pa.ContainerControl is ButtonX && !((ButtonX)pa.ContainerControl).FocusCuesEnabled))
					{
						Rectangle r=itemRect;
                        r.Inflate(-2, -2);
						ControlPaint.DrawFocusRectangle(g,r);
					}
				}					
			}
			
			// Draw Shortcut text if needed
			if( button.DrawShortcutText!="" && isOnMenu && !button.IsOnCustomizeDialog)
			{
                stringFormat |= eTextFormat.HidePrefix;
                
                //if(rightToLeft)
                //    stringFormat = stringFormat & ~(stringFormat & eTextFormat.Right) | eTextFormat.Left;
                //else
                    stringFormat|=eTextFormat.Right;

				TextDrawing.DrawString(g, button.DrawShortcutText, font, textColor, rect, stringFormat);
			}
		}

        protected virtual Rectangle GetTotalSubItemsRect(ButtonItem button)
        {
            return button.GetTotalSubItemsRect();
        }

		public override void PaintExpandButton(ButtonItem button, ItemPaintArgs pa)
		{
			Graphics g=pa.Graphics;
            bool isOnMenu = IsOnMenu(button, pa);
			Rectangle itemRect=button.DisplayRectangle;
			bool mouseOver=button.IsMouseOver;

            Color textColor = this.GetTextColor(button, pa);
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

                        g.FillPolygon(textBrush, p);
                    }
                    else if (!button.SubItemsRect.IsEmpty)
                    {
                        if (IsItemEnabled(button, pa) && ((mouseOver || button.Checked) && !button.Expanded && button.HotTrackingStyle != eHotTrackingStyle.None && button.HotTrackingStyle != eHotTrackingStyle.Image) && !button.AutoExpandOnClick)
                        {
                            Rectangle r = GetTotalSubItemsRect(button); // button.SubItemsRect;
                            r.Offset(itemRect.Location);
                            using (Pen mypen = new Pen(mouseOver?pa.Colors.ItemHotBorder:pa.Colors.ItemCheckedBorder))
                                DisplayHelp.DrawRectangle(g, mypen, r);
                        }
                        PaintButtonExpandIndicator(button, pa);
                    }
                }
            }
		}

        protected virtual void PaintMenuItemSide(ButtonItem button, ItemPaintArgs pa, Rectangle sideRect)
        {
            Graphics g = pa.Graphics;
            Region oldClip = g.Clip.Clone() as Region;
            g.SetClip(sideRect);

            sideRect.Inflate(0, 1);

            // Draw side bar
            if (button.MenuVisibility == eMenuVisibility.VisibleIfRecentlyUsed && !button.RecentlyUsed)
            {
                DisplayHelp.FillRectangle(g, sideRect, pa.Colors.MenuUnusedSide, pa.Colors.MenuUnusedSide2, pa.Colors.MenuUnusedSideGradientAngle);
            }
            else
            {
                DisplayHelp.FillRectangle(g, sideRect, pa.Colors.MenuSide, pa.Colors.MenuSide2, pa.Colors.MenuSideGradientAngle);
            }

            if (oldClip != null)
                g.Clip = oldClip;
            else
                g.ResetClip();
        }

		public override void PaintButtonBackground(ButtonItem button, ItemPaintArgs pa, CompositeImage image)
		{
			Graphics g=pa.Graphics;
            bool isOnMenu = IsOnMenu(button, pa);

			if(isOnMenu)
			{
                Rectangle sideRect = new Rectangle(button.DisplayRectangle.Left, button.DisplayRectangle.Top, button.ImageDrawRect.Right, button.DisplayRectangle.Height);
                if (pa.RightToLeft)
                {
                    sideRect = new Rectangle(button.DisplayRectangle.Right - button.ImageDrawRect.Right, button.DisplayRectangle.Top, button.ImageDrawRect.Right, button.DisplayRectangle.Height);
                }

                PaintMenuItemSide(button, pa, sideRect);
			}
			else
			{
				// Draw button background
				if(!pa.Colors.ItemBackground.IsEmpty)
				{
					if(pa.Colors.ItemBackground2.IsEmpty)
					{
						using(SolidBrush mybrush=new SolidBrush(pa.Colors.ItemBackground))
							g.FillRectangle(mybrush,button.DisplayRectangle);
					}
					else
					{
						using(LinearGradientBrush gradient=BarFunctions.CreateLinearGradientBrush(button.DisplayRectangle,pa.Colors.ItemBackground,pa.Colors.ItemBackground2,pa.Colors.ItemBackgroundGradientAngle))
							g.FillRectangle(gradient,button.DisplayRectangle);
					}
				}
				else if(!IsItemEnabled(button, pa) && !pa.Colors.ItemDisabledBackground.IsEmpty)
				{
					using(SolidBrush mybrush=new SolidBrush(pa.Colors.ItemDisabledBackground))
						g.FillRectangle(mybrush,button.DisplayRectangle);
				}
			}
			
			Rectangle itemRect=button.DisplayRectangle;
			if(IsItemEnabled(button, pa) || button.DesignMode)
			{
				if(button.Expanded && !isOnMenu)
				{					
					// DotNet Style
					if(pa.Colors.ItemExpandedBackground2.IsEmpty)
					{
						Rectangle rBack=button.DisplayRectangle;
						if(!pa.Colors.ItemExpandedShadow.IsEmpty)
							rBack.Width-=2;
						using(SolidBrush mybrush=new SolidBrush(pa.Colors.ItemExpandedBackground))
							g.FillRectangle(mybrush,rBack);
					}
					else
					{
						LinearGradientBrush gradient=BarFunctions.CreateLinearGradientBrush(new Rectangle(itemRect.Left,itemRect.Top,itemRect.Width-2,itemRect.Height),pa.Colors.ItemExpandedBackground,pa.Colors.ItemExpandedBackground2,pa.Colors.ItemExpandedBackgroundGradientAngle);
						Rectangle rBack=new Rectangle(itemRect.Left,itemRect.Top,itemRect.Width,itemRect.Height);
						if(!pa.Colors.ItemExpandedShadow.IsEmpty)
							rBack.Width-=2;
						g.FillRectangle(gradient,rBack);
						gradient.Dispose();
					}
					Point[] p;
					if(button.Orientation==eOrientation.Horizontal && button.PopupSide==ePopupSide.Default)
						p=new Point[4];
					else
						p=new Point[5];
					p[0].X=itemRect.Left;
					p[0].Y=itemRect.Top+itemRect.Height-1;
					p[1].X=itemRect.Left;
					p[1].Y=itemRect.Top;
                    if (button.Orientation == eOrientation.Horizontal /*&& !pa.Colors.ItemExpandedShadow.IsEmpty*/)
                    {
                        if (!pa.Colors.ItemExpandedShadow.IsEmpty)
                            p[2].X = itemRect.Left + itemRect.Width - 3;
                        else
                            p[2].X = itemRect.Right - 1;
                    }
                    else
                        p[2].X = itemRect.Left + itemRect.Width - 1;
					p[2].Y=itemRect.Top;
                    if (button.Orientation == eOrientation.Horizontal /*&& !pa.Colors.ItemExpandedShadow.IsEmpty*/)
                    {
                        if (!pa.Colors.ItemExpandedShadow.IsEmpty)
                            p[3].X = itemRect.Left + itemRect.Width - 3;
                        else
                            p[3].X = itemRect.Right - 1;
                    }
                    else
                        p[3].X = itemRect.Left + itemRect.Width - 1;

					p[3].Y=itemRect.Top+itemRect.Height-1;

					if(button.Orientation==eOrientation.Vertical || button.PopupSide!=ePopupSide.Default)
					{
						p[4].X=itemRect.Left;
						p[4].Y=itemRect.Top+itemRect.Height-1;
					}
					
					if(!pa.Colors.ItemExpandedBorder.IsEmpty)
					{
						using(Pen mypen=new Pen(pa.Colors.ItemExpandedBorder,1))
							g.DrawLines(mypen,p);
					}
					// Draw the shadow
					if(!pa.Colors.ItemExpandedShadow.IsEmpty && button.Orientation==eOrientation.Horizontal)
					{
						using(SolidBrush shadow=new SolidBrush(pa.Colors.ItemExpandedShadow))
							g.FillRectangle(shadow,itemRect.Left+itemRect.Width-2,itemRect.Top+2,2,itemRect.Height-2); // TODO: ADD GRADIENT SHADOW					
					}
				}
			}
		}
	}
}
