using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;
using System;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Summary description for ButtonItemLayout.
	/// </summary>
	internal class ButtonItemLayout
	{
        /// <summary>
        /// Arranges the button inner parts when button size has changed externally.
        /// </summary>
        /// <param name="button">Button to arrange inner parts for.</param>
        public static void Arrange(ButtonItem button)
        {
            int minTextSize = 8;
            bool isOnMenu = button.IsOnMenu;
            if (isOnMenu && button.Parent is ItemContainer)
                isOnMenu = false;
            bool hasImage = false;
            using (CompositeImage buttonImage = button.GetImage())
            {
                if (buttonImage != null)
                    hasImage = true;
            }
            bool rightToLeft = button.IsRightToLeft;
            Size imageSize = GetLayoutImageSize(button, hasImage, isOnMenu, false);
            Rectangle bounds = button.DisplayRectangle;

            if (isOnMenu)
            {
                // Add 4 pixel padding to the image size, 2 pixels on each side
                imageSize.Height += 2;
                imageSize.Width += 7;
                // Center image if any...
                //if (rightToLeft)
                //{
                //    if (button.IsOnCustomizeMenu)
                //        button.ImageDrawRect = new Rectangle(bounds.Width - (imageSize.Width + bounds.Height + 2), Math.Max(0, (bounds.Height - imageSize.Height) / 2), imageSize.Width, imageSize.Height);
                //    else
                //        button.ImageDrawRect = new Rectangle(bounds.Width - imageSize.Width - 1, Math.Max(0, (bounds.Height - imageSize.Height) / 2), imageSize.Width, imageSize.Height);
                //}
                //else
                {
                    if (button.IsOnCustomizeMenu && !rightToLeft)
                        button.ImageDrawRect = new Rectangle(bounds.Height + 2, Math.Max(0, (bounds.Height - imageSize.Height) / 2), imageSize.Width, imageSize.Height);
                    else
                        button.ImageDrawRect = new Rectangle(0, Math.Max(0, (bounds.Height - imageSize.Height) / 2), imageSize.Width, imageSize.Height);
                }

                //if (rightToLeft)
                //    button.TextDrawRect = new Rectangle(Math.Max(0, button.ImageDrawRect.X - button.TextDrawRect.Width - 2), Math.Max(0, (bounds.Height - button.TextDrawRect.Height) / 2), button.TextDrawRect.Width, button.TextDrawRect.Height);
                //else
                    button.TextDrawRect = new Rectangle(button.ImageDrawRect.Right + 8, Math.Max(0, (bounds.Height - button.TextDrawRect.Height) / 2), button.TextDrawRect.Width, button.TextDrawRect.Height);

                return;
            }
            int x = 0;
            if (!button.SubItemsRect.IsEmpty)
            {
                Rectangle subItemsRect = button.SubItemsRect;
                if (button.ContainerControl is RibbonBar && (button.ImagePosition == eImagePosition.Top || button.ImagePosition == eImagePosition.Bottom))
                {
                    subItemsRect = new Rectangle(0, bounds.Height - subItemsRect.Height, bounds.Width, subItemsRect.Height);
                    bounds.Height -= subItemsRect.Height;
                }
                else
                {
                    if (button.Orientation == eOrientation.Horizontal)
                    {
                        if (rightToLeft)
                        {
                            subItemsRect = new Rectangle(0, 0, subItemsRect.Width, bounds.Height);
                            //bounds.Width -= subItemsRect.Width;
                            //bounds.X += subItemsRect.Width;
                            x = subItemsRect.Width;
                        }
                        else
                        {
                            subItemsRect = new Rectangle(bounds.Width - subItemsRect.Width, 0, subItemsRect.Width, bounds.Height);
                            bounds.Width -= subItemsRect.Width;
                        }
                    }
                    else
                    {
                        subItemsRect = new Rectangle(0, 0, bounds.Width, subItemsRect.Height);
                        bounds.Height -= subItemsRect.Height;
                    }
                }
                button.SubItemsRect = subItemsRect;
            }

            if (!hasImage || button.ButtonStyle == eButtonStyle.TextOnlyAlways)
            {
                int newHeight = bounds.Height - 2;
                button.TextDrawRect = new Rectangle(2,
                    Math.Max(0, (bounds.Height - newHeight) / 2), bounds.Width - 4, newHeight);
                return;
            }
            else if (button.ButtonStyle == eButtonStyle.Default && !(button.ImagePosition == eImagePosition.Top || button.ImagePosition == eImagePosition.Bottom) || bounds.Width < imageSize.Width + minTextSize)
            {
                // Display image only in center of the button
                button.ImageDrawRect = new Rectangle(Math.Max(0, (bounds.Width - button.ImageDrawRect.Width) / 2),
                    Math.Max(0, (bounds.Height - button.ImageDrawRect.Height) / 2), button.ImageDrawRect.Width, button.ImageDrawRect.Height);
                if (bounds.Width < imageSize.Width + minTextSize)
                    button.TextDrawRect = Rectangle.Empty;
                return;
            }

            // Displays both image and text
            if (button.ImagePosition == eImagePosition.Left && !rightToLeft || button.ImagePosition == eImagePosition.Right && rightToLeft)
            {
                button.ImageDrawRect = new Rectangle(0, Math.Max(0, (bounds.Height - button.ImageDrawRect.Height) / 2),
                    button.ImageDrawRect.Width, button.ImageDrawRect.Height);
                button.TextDrawRect = new Rectangle(button.ImageDrawRect.Right - 2, Math.Max(0, (bounds.Height - button.TextDrawRect.Height) / 2),
                    Math.Min(button.TextDrawRect.Width, bounds.Width - (button.ImageDrawRect.Right - 2)), button.TextDrawRect.Height);
            }
            else if (button.ImagePosition == eImagePosition.Right && !rightToLeft || button.ImagePosition == eImagePosition.Left && rightToLeft)
            {
                button.ImageDrawRect = new Rectangle(bounds.Width  - button.ImageDrawRect.Width, Math.Max(0, (bounds.Height - button.TextDrawRect.Height) / 2),
                    button.ImageDrawRect.Width, button.ImageDrawRect.Height);
                button.TextDrawRect = new Rectangle(Math.Max(x,button.ImageDrawRect.X - button.TextDrawRect.Width + 2), Math.Max(0, (bounds.Height - button.TextDrawRect.Height) / 2),
                    Math.Min(button.TextDrawRect.Width, button.ImageDrawRect.X), button.TextDrawRect.Height);
            }
            else if (button.ImagePosition == eImagePosition.Top)
            {
                int y = Math.Max(2, (bounds.Height - (button.TextDrawRect.Height + button.ImageDrawRect.Height - 2)) / 2);
                if (button.Name != "sysOverflowButton" && button.Parent is ItemContainer && ((ItemContainer)button.Parent).LayoutOrientation == eOrientation.Horizontal && ((ItemContainer)button.Parent).VerticalItemAlignment == eVerticalItemsAlignment.Top)
                    y = 2;
                
                button.ImageDrawRect = new Rectangle(0, y,
                    bounds.Width, button.ImageDrawRect.Height);

                button.TextDrawRect = new Rectangle(Math.Max(0, (bounds.Width - button.TextDrawRect.Width) / 2), button.ImageDrawRect.Bottom,
                    button.TextDrawRect.Width, button.TextDrawRect.Height);
            }
            else if (button.ImagePosition == eImagePosition.Bottom)
            {
                int y = Math.Max(0, (bounds.Height - (button.TextDrawRect.Height + button.ImageDrawRect.Height - 2)) / 2);
                //button.ImageDrawRect = new Rectangle(Math.Max(0, (bounds.Width - button.ImageDrawRect.Width) / 2), bounds.Height - y - button.ImageDrawRect.Height,
                //    button.ImageDrawRect.Width, button.ImageDrawRect.Height);
                button.ImageDrawRect = new Rectangle(0, bounds.Height - y - button.ImageDrawRect.Height,
                    bounds.Width, button.ImageDrawRect.Height);
                button.TextDrawRect = new Rectangle(Math.Max(0, (bounds.Width - button.ImageDrawRect.Width) / 2), Math.Max(0, button.ImageDrawRect.Y + 2 - button.TextDrawRect.Height),
                    button.TextDrawRect.Width, Math.Min(button.TextDrawRect.Height, button.ImageDrawRect.Y));
            }
        }

        public static Size MeasureItemText(BaseItem item, Graphics g, int containerWidth, Font font, eTextFormat stringFormat, bool rightToLeft)
        {
            return MeasureItemText(item, g, containerWidth, font, stringFormat, rightToLeft, false, eImagePosition.Left);
        }

        public static Size MeasureItemText(BaseItem item, Graphics g, int containerWidth, Font font, eTextFormat stringFormat, bool rightToLeft, bool ribbonBarButton, eImagePosition imagePosition)
        {
            if (item.Text == "" && item.TextMarkupBody == null) return Size.Empty;

            Size textSize = Size.Empty;

            if (item.TextMarkupBody == null)
            {
                textSize = TextDrawing.MeasureString(g, ButtonItemPainter.GetDrawText(item.Text), font, containerWidth, stringFormat);
            }
            else
            {
                Size availSize = new Size(containerWidth, 1);
                if (containerWidth == 0)
                    availSize.Width = 1600;
                TextMarkup.MarkupDrawContext d = new TextMarkup.MarkupDrawContext(g, font, Color.Empty, false);
                item.TextMarkupBody.Measure(availSize, d);
                availSize = item.TextMarkupBody.Bounds.Size;
                if (containerWidth != 0 && !(ribbonBarButton && imagePosition== eImagePosition.Top))
                    availSize.Width = containerWidth;
                d.RightToLeft = rightToLeft;
                item.TextMarkupBody.Arrange(new Rectangle(0, 0, availSize.Width, availSize.Height), d);

                textSize = item.TextMarkupBody.Bounds.Size;
            }

            return textSize;
        }

        public static void LayoutButton(ButtonItem button)
        {
            LayoutButton(button, false);
        }

        private static bool UseRibbonWordBreak(ButtonItem button)
        {
            if (button.TextMarkupBody == null)
                return button.Text.IndexOf(' ') > 0;

            return (button.Text.IndexOf(' ') > 0 && !button.TextMarkupBody.HasExpandElement || button.Text.Split(' ').Length > 2 && button.TextMarkupBody.HasExpandElement && button.ButtonStyle != eButtonStyle.Default);
        }

		public static void LayoutButton(ButtonItem button, bool startButtonType)
		{
			Control objCtrl=button.ContainerControl as Control;
			if(objCtrl==null || objCtrl.Disposing || objCtrl.IsDisposed) //if(!BarFunctions.IsHandleValid(objCtrl))
				return;
            if (objCtrl is ButtonX && button._FitContainer)
            {
                LayoutButtonX(button);
                return;
            }
            else if (button.FixedSize.Width > 0 && button.FixedSize.Height > 0)
            {
                button.SetDisplayRectangle(new Rectangle(button.DisplayRectangle.Location, button.FixedSize));
                LayoutButtonX(button);
                return;
            }

            bool isOnMenu = button.IsOnMenu;
            if (isOnMenu && button.Parent is ItemContainer)
                isOnMenu = false;
            bool bHasImage = false;
            using (CompositeImage buttonImage = button.GetImage())
            {
                if (buttonImage != null || startButtonType)
                    bHasImage = true;
            }

            eImagePosition imagePosition = button.ImagePosition;
            bool rightToLeft = (objCtrl.RightToLeft == RightToLeft.Yes);

            Rectangle textDrawRect = Rectangle.Empty;
            Rectangle imageDrawRect = Rectangle.Empty;
            Rectangle subItemsRect = Rectangle.Empty;
            Rectangle bounds = new Rectangle(button.DisplayRectangle.Location, Size.Empty); // Critical to preserve the location for compatibility reasons

            if (rightToLeft && button.Orientation == eOrientation.Horizontal)
            {
                if (imagePosition == eImagePosition.Left)
                    imagePosition = eImagePosition.Right;
                else if (imagePosition == eImagePosition.Right)
                    imagePosition = eImagePosition.Left;
            }

            int measureStringWidth = 0;

            if (button._FitContainer)
                measureStringWidth = button.DisplayRectangle.Width - 4;

            bounds.Width = 0;
            bounds.Height = 0;

            Graphics g = BarFunctions.CreateGraphics(objCtrl);
            try
            {
                eTextFormat stringFormat = GetTextFormat(button);

                // Get the right image size that we will use for calculation
                Size imageSize = GetLayoutImageSize(button, bHasImage, isOnMenu, startButtonType);
                bool ribbonBarButton = false;
                if (button._FitContainer && bHasImage && (imagePosition == eImagePosition.Left || imagePosition == eImagePosition.Right))
                {
                    measureStringWidth -= (imageSize.Width + 10);
                }
                else if (button.RibbonWordWrap && bHasImage && imagePosition == eImagePosition.Top && objCtrl is RibbonBar && UseRibbonWordBreak(button))
                {
                    measureStringWidth = imageSize.Width + 4;
                    stringFormat |= eTextFormat.WordBreak;
                    ribbonBarButton = true;
                }

                // Measure string
                Font font = button.GetFont(null, true);

                SizeF textSize = SizeF.Empty;

                if ((button.Text != "" || button.TextMarkupBody!=null) && (!bHasImage || isOnMenu || button.ButtonStyle != eButtonStyle.Default || button.ImagePosition != eImagePosition.Left && bHasImage))
                {
                    textSize = ButtonItemLayout.MeasureItemText(button, g, measureStringWidth, font, stringFormat, rightToLeft, ribbonBarButton, imagePosition);
                    //if (button.HotFontBold) textSize.Width += textSize.Width * .15f;
                    int maxItt = 0;
                    int increase = Math.Max(14, imageSize.Width / 2);
                    while (ribbonBarButton && textSize.Height > font.Height * 2.2 && maxItt<3)
                    {
                        measureStringWidth += increase;
                        textSize = ButtonItemLayout.MeasureItemText(button, g, measureStringWidth, font, stringFormat, rightToLeft, ribbonBarButton, imagePosition);
                        maxItt++;
                    }
                }

                // See if this button is on menu, and do appropriate calculations
                if (isOnMenu)
                {
                    if (imageSize.IsEmpty)
                        imageSize = new Size(16, 16);

                    // Add 4 pixel padding to the image size, 2 pixels on each side
                    imageSize.Height += 2;
                    imageSize.Width += 7;

                    // Calculate item height
                    if (textSize.Height > imageSize.Height)
                        bounds.Height = (int)textSize.Height + 4;
                    else
                        bounds.Height = imageSize.Height + 4;

                    // Add Vertical Padding to it
                    bounds.Height += button.VerticalPadding;

                    // We know the image position now, we will center it into this area
                    if (button.IsOnCustomizeMenu && !rightToLeft)
                        imageDrawRect = new Rectangle(bounds.Height + 2, (bounds.Height - imageSize.Height) / 2, imageSize.Width, imageSize.Height);
                    else
                        imageDrawRect = new Rectangle(0, (bounds.Height - imageSize.Height) / 2, imageSize.Width, imageSize.Height);

                    bounds.Width = (int)textSize.Width;
                    // Add short-cut size if we have short-cut
                    if (button.DrawShortcutText != "")
                    {
                        Size objSizeShortcut = TextDrawing.MeasureString(g, button.DrawShortcutText, font, 0, stringFormat);
                        bounds.Width += (objSizeShortcut.Width + 14); // 14 distance between text and shortcut
                    }

                    textDrawRect = new Rectangle(imageDrawRect.Right + 8, 2, bounds.Width, bounds.Height - 4);

                    // 8 pixels distance between image and text, 22 pixels if this item has sub items
                    bounds.Width += (imageDrawRect.Right + 8 + 26);
                    bounds.Width += button.HorizontalPadding;
                }
                else
                {
                    bool bThemed = button.IsThemed;
                    if (button.Orientation == eOrientation.Horizontal && (imagePosition == eImagePosition.Left || imagePosition == eImagePosition.Right))
                    {
                        // Recalc size for the Bar button
                        // Add 8 pixel padding to the image size, 4 pixels on each side
                        //objImageSize.Height+=4;
                        imageSize.Width += button.ImagePaddingHorizontal;

                        // Calculate item height
                        if (textSize.Height > imageSize.Height)
                            bounds.Height = (int)textSize.Height + button.ImagePaddingVertical;
                        else
                            bounds.Height = imageSize.Height + button.ImagePaddingVertical;

                        // Add Vertical Padding
                        bounds.Height += button.VerticalPadding;

                        if (bThemed && !button.IsOnMenuBar)
                            bounds.Height += 4;

                        imageDrawRect = Rectangle.Empty;
                        if (button.ButtonStyle != eButtonStyle.TextOnlyAlways && bHasImage)
                        {
                            // We know the image position now, we will center it into this area
                            imageDrawRect = new Rectangle(0, (bounds.Height - imageSize.Height) / 2, imageSize.Width, imageSize.Height);
                        }

                        // Draw Text only if needed
                        textDrawRect = Rectangle.Empty;
                        if (button.ButtonStyle != eButtonStyle.Default || !bHasImage)
                        {
                            if (imageDrawRect.Right > 0)
                            {
                                bounds.Width = (int)textSize.Width + 1;
                                textDrawRect = new Rectangle(imageDrawRect.Right - 2, 2, bounds.Width, bounds.Height - 4);
                            }
                            else
                            {
                                bounds.Width = (int)textSize.Width + 6;
                                if (!bHasImage && button.IsOnMenuBar)
                                {
                                    bounds.Width += 6;
                                    textDrawRect = new Rectangle(2, 2, bounds.Width, bounds.Height - 4);
                                }
                                else
                                    textDrawRect = new Rectangle(2, 2, bounds.Width + button.HorizontalPadding - 4, bounds.Height - 4);
                            }
                        }
                        bounds.Width += imageDrawRect.Right;

                        if (imagePosition == eImagePosition.Right && imageDrawRect.Right > 0 && bHasImage)
                        {
                            textDrawRect.X = 3;
                            imageDrawRect.X = bounds.Width - imageDrawRect.Width;
                        }

                        // Add Horizontal padding
                        bounds.Width += button.HorizontalPadding;
                    }
                    else
                    {
                        // Image is on top or bottom
                        // Calculate width, that is easy
                        if (button.Orientation == eOrientation.Horizontal)
                        {

                            if (textSize.Width > imageSize.Width)
                                bounds.Width = (int)textSize.Width + button.ImagePaddingHorizontal;
                            else
                                bounds.Width = imageSize.Width + button.ImagePaddingHorizontal;

                            // Calculate item height 3 padding on top and bottom and 2 pixels distance between the image and text
                            bounds.Height = (int)(imageSize.Height + textSize.Height + button.ImagePaddingVertical /*10*/);

                            // Add Horizontal/Vertical padding
                            bounds.Width += button.HorizontalPadding;
                            bounds.Height += button.VerticalPadding;

                            if (imagePosition == eImagePosition.Top)
                            {
                                imageDrawRect = new Rectangle(0, button.VerticalPadding / 2 + 2, bounds.Width, imageSize.Height/*+2*/);
                                textDrawRect = new Rectangle((int)(bounds.Width - textSize.Width) / 2, imageDrawRect.Bottom, (int)textSize.Width, (int)textSize.Height + 5);
                            }
                            else
                            {
                                textDrawRect = new Rectangle((int)(bounds.Width - textSize.Width) / 2, button.VerticalPadding / 2, (int)textSize.Width, (int)textSize.Height + 2);
                                imageDrawRect = new Rectangle(0, textDrawRect.Bottom, bounds.Width, imageSize.Height + 5);
                            }
                        }
                        else
                        {
                            if (textSize.Height > imageSize.Width && button.ButtonStyle != eButtonStyle.Default)
                                bounds.Width = (int)textSize.Height + 6;
                            else
                                bounds.Width = imageSize.Width + 10;

                            // Add Horizontal Padding
                            bounds.Width += button.HorizontalPadding;

                            // Calculate item height 3 padding on top and bottom and 2 pixels distance between the image and text
                            if (button.ButtonStyle != eButtonStyle.Default || !bHasImage)
                            {
                                if (bHasImage)
                                    bounds.Height = (int)(imageSize.Height + textSize.Width + 12);
                                else
                                    bounds.Height = (int)(textSize.Width + 6);
                            }
                            else
                                bounds.Height = imageSize.Height + 6;

                            if (imagePosition == eImagePosition.Top || imagePosition == eImagePosition.Left)
                            {
                                if (bHasImage)
                                    imageDrawRect = new Rectangle(0, 0, bounds.Width, imageSize.Height + 6);
                                textDrawRect = new Rectangle((int)(bounds.Width - textSize.Height) / 2, imageDrawRect.Bottom + 2, (int)textSize.Height, (int)textSize.Width + 5);
                            }
                            else
                            {
                                textDrawRect = new Rectangle((int)(bounds.Width - textSize.Height) / 2, 5, (int)textSize.Height, (int)textSize.Width + 5);
                                if (bHasImage)
                                    imageDrawRect = new Rectangle(0, textDrawRect.Bottom -3, bounds.Width, imageSize.Height + 5);
                            }

                            // Add Vertical Padding
                            bounds.Height += button.VerticalPadding;
                        }
                    }

                    if (HasExpandPart(button))
                    {
                        subItemsRect = GetSubItemsButtonBounds(button, bounds, rightToLeft);
                        // Add small button to expand the item
                        Rectangle rTemp = subItemsRect;
                        rTemp.Offset(bounds.Location);

                        if (rightToLeft && !(objCtrl is RibbonBar &&
                            (button.ImagePosition == eImagePosition.Top || button.ImagePosition == eImagePosition.Bottom)))
                        {
                            if (!textDrawRect.IsEmpty)
                                textDrawRect.Offset(subItemsRect.Width + 1, 0);
                            if (!imageDrawRect.IsEmpty && (button.Orientation == eOrientation.Horizontal && (imagePosition == eImagePosition.Left || imagePosition == eImagePosition.Right)))
                                imageDrawRect.Offset(subItemsRect.Width, 0);
                            bounds.X += subItemsRect.Width;
                        }
                        
                        bounds = Rectangle.Union(bounds, rTemp);
                    }
                }
            }
            finally
            {
                g.TextRenderingHint = TextRenderingHint.SystemDefault;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
                g.Dispose();
            }
			objCtrl=null;

            button.SetDisplayRectangle(bounds);
			button.ImageDrawRect=imageDrawRect;
			button.TextDrawRect=textDrawRect;
			button.SubItemsRect=subItemsRect;
		}

        private static bool HasExpandPart(ButtonItem button)
        {
            return (button.SubItems.Count > 0 || button.PopupType == ePopupType.Container) && button.ShowSubItems && !button.IsOnMenuBar &&
                        !(button.TextMarkupBody != null && button.TextMarkupBody.HasExpandElement && button.ButtonStyle != eButtonStyle.Default);
        }

        //private static ButtonTextSize GetTextSize(ButtonItem button, int containerWidth, bool measureShortcut)
        //{
        //    ButtonTextSize textSize = new ButtonTextSize();
        //    Control ctrl = button.ContainerControl as Control;
        //    Graphics g = BarFunctions.CreateGraphics(ctrl);
        //    try
        //    {
        //        eTextFormat stringFormat = GetTextFormat(button);
        //        Font font = button.GetFont(null);

        //        if (button.TextMarkupBody == null)
        //        {
        //            textSize.TextSize = TextDrawing.MeasureString(g, ButtonItemPainter.GetDrawText(button.Text), font, containerWidth, stringFormat);
        //        }
        //        else
        //        {
        //            Size availSize = new Size(containerWidth, 1);
        //            if (containerWidth == 0)
        //                availSize.Width = 1600;
        //            TextMarkup.MarkupDrawContext d = new TextMarkup.MarkupDrawContext(g, font, Color.Empty, false);
        //            button.TextMarkupBody.Measure(availSize, d);
        //            availSize = button.TextMarkupBody.Bounds.Size;
        //            if (containerWidth != 0)
        //                availSize.Width = containerWidth;
        //            d.RightToLeft = button.IsRightToLeft;
        //            button.TextMarkupBody.Arrange(new Rectangle(0, 0, availSize.Width, availSize.Height), d);

        //            textSize.TextSize = button.TextMarkupBody.Bounds.Size;
        //        }

        //        if (measureShortcut && button.DrawShortcutText != "")
        //            textSize.ShortcutTextSize = TextDrawing.MeasureString(g, button.DrawShortcutText, font, 0, stringFormat);
        //    }
        //    finally
        //    {
        //        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
        //        g.TextRenderingHint = TextRenderingHint.SystemDefault;
        //        g.Dispose();
        //    }
        //    return textSize;
        //}

        private static Size GetLayoutImageSize(ButtonItem button, bool hasImage, bool isOnMenu, bool startButtonType)
        {
            // Get the right image size that we will use for calculation
            Size imageSize = Size.Empty;
            if (button.Parent != null && button.ImageFixedSize.IsEmpty)
            {
                ImageItem parentImageItem = button.Parent as ImageItem;
                if (parentImageItem != null && !parentImageItem.SubItemsImageSize.IsEmpty)
                {
                    if (!hasImage || isOnMenu)
                        imageSize = new Size(parentImageItem.SubItemsImageSize.Width, parentImageItem.SubItemsImageSize.Height);
                    else
                    {
                        if (button.Orientation == eOrientation.Horizontal)
                            imageSize = new Size(button.ImageSize.Width, Math.Max(parentImageItem.SubItemsImageSize.Height, button.ImageSize.Height));
                        else
                            imageSize = new Size(Math.Max(parentImageItem.SubItemsImageSize.Width, button.ImageSize.Width), button.ImageSize.Height);
                    }
                }
                else
                    imageSize = button.ImageSize;
            }
            else if (!button.ImageFixedSize.IsEmpty)
                imageSize = button.ImageFixedSize;
            else
                imageSize = button.ImageSize;

            if (startButtonType)
            {
                Office2007Renderer renderer = GlobalManager.Renderer as Office2007Renderer;
                if (renderer != null && renderer.ColorTable is Office2007ColorTable)
                {
                    Office2007ColorTable table = renderer.ColorTable as Office2007ColorTable;
                    if (table.RibbonControl.StartButtonDefault != null)
                    {
                        if (imageSize.Width < table.RibbonControl.StartButtonDefault.Width)
                            imageSize.Width = table.RibbonControl.StartButtonDefault.Width;
                        if (imageSize.Height < table.RibbonControl.StartButtonDefault.Height)
                            imageSize.Height = table.RibbonControl.StartButtonDefault.Height;
                    }
                }
            }

            return imageSize;
        }

		public static Rectangle GetSubItemsButtonBounds(ButtonItem button, Rectangle buttonBounds, bool rightToLeft)
		{
			Rectangle subItemsRect=Rectangle.Empty;

			if(button.ContainerControl is RibbonBar && (button.ImagePosition==eImagePosition.Top || button.ImagePosition==eImagePosition.Bottom))
			{
				subItemsRect=new Rectangle(0,buttonBounds.Height,buttonBounds.Width,button.SubItemsExpandWidth);
			}
			else
			{
				// Add small button to expand the item
				if(button.Orientation==eOrientation.Horizontal)
				{
                    if (button.IsThemed)
                    {
                        if(rightToLeft)
                            subItemsRect = new Rectangle(0, 0, button.SubItemsExpandWidth, buttonBounds.Height);
                        else
                            subItemsRect = new Rectangle(buttonBounds.Width, 0, button.SubItemsExpandWidth, buttonBounds.Height);
                    }
                    else
                    {
                        if (rightToLeft)
                            subItemsRect = new Rectangle(0, 0, button.SubItemsExpandWidth, buttonBounds.Height);
                        else
                            subItemsRect = new Rectangle(buttonBounds.Width + 1, 0, button.SubItemsExpandWidth, buttonBounds.Height);
                    }
				}
				else
				{
					subItemsRect=new Rectangle(0,buttonBounds.Height-2,buttonBounds.Width,button.SubItemsExpandWidth);
				}
			}
			
			return subItemsRect;
		}

		public static eTextFormat GetTextFormat(ButtonItem button)
		{
			eTextFormat format = eTextFormat.Default;
			if (!button._FitContainer)
			{
				format |= eTextFormat.SingleLine;
			}
			else
				format |= eTextFormat.WordBreak;

			format |= eTextFormat.VerticalCenter;
			return format;
		}

        public static void LayoutButtonX(ButtonItem button)
        {
            Control objCtrl = button.ContainerControl as Control;
            ButtonX btnX = button.ContainerControl as ButtonX;
            if (!BarFunctions.IsHandleValid(objCtrl))
                return;
            bool isOnMenu = button.IsOnMenu;
            if (isOnMenu && button.Parent is ItemContainer)
                isOnMenu = false;
            bool bHasImage = false;

            using (CompositeImage buttonImage = button.GetImage())
            {
                if (buttonImage != null)
                    bHasImage = true;
            }

            eImagePosition imagePosition = button.ImagePosition;
            bool rightToLeft = (objCtrl.RightToLeft == RightToLeft.Yes);

            Rectangle textDrawRect = Rectangle.Empty;
            Rectangle imageDrawRect = Rectangle.Empty;
            Rectangle subItemsRect = Rectangle.Empty;
            Rectangle bounds = button.Bounds;

            // Calculate sub-items rectangle
            if (button.SubItems.Count > 0 && button.ShowSubItems &&
                        !(button.TextMarkupBody != null && button.TextMarkupBody.HasExpandElement && button.ButtonStyle != eButtonStyle.Default))
            {
                // Add small button to expand the item
                if (button.Orientation == eOrientation.Horizontal)
                {
                    if (rightToLeft)
                        subItemsRect = new Rectangle(0, 0, button.SubItemsExpandWidth, bounds.Height);
                    else
                        subItemsRect = new Rectangle(bounds.Width - button.SubItemsExpandWidth, 0, button.SubItemsExpandWidth, bounds.Height);
                    if (rightToLeft)
                        bounds.X += button.SubItemsExpandWidth + 1;
                    bounds.Width -= button.SubItemsExpandWidth + 1;
                }
                else
                {
                    subItemsRect = new Rectangle(0, bounds.Height - button.SubItemsExpandWidth, bounds.Width, button.SubItemsExpandWidth);
                    bounds.Height -= button.SubItemsExpandWidth + 1;
                }
            }

            // Adjust image position
            if (rightToLeft && button.Orientation == eOrientation.Horizontal)
            {
                if (imagePosition == eImagePosition.Left)
                    imagePosition = eImagePosition.Right;
                else if (imagePosition == eImagePosition.Right)
                    imagePosition = eImagePosition.Left;
            }

            int measureStringWidth = 0;

            measureStringWidth = bounds.Width;

            Graphics g = BarFunctions.CreateGraphics(objCtrl);
            try
            {

                // Get the right image size that we will use for calculation
                Size imageSize = Size.Empty;
                if (!button.ImageFixedSize.IsEmpty)
                    imageSize = button.ImageFixedSize;
                else
                    imageSize = button.ImageSize;

                if (bHasImage && (imagePosition == eImagePosition.Left || imagePosition == eImagePosition.Right))
                {
                    if (btnX != null)
                        measureStringWidth -= imageSize.Width + btnX.ImageTextSpacing * 2 + 3;
                    else
                        measureStringWidth -= (imageSize.Width + 8);
                }

                if (bHasImage && !imageSize.IsEmpty && btnX != null && btnX.ImageTextSpacing != 0)
                {
                    if (imagePosition == eImagePosition.Left || imagePosition == eImagePosition.Right)
                        imageSize.Width += btnX.ImageTextSpacing * 2;
                    else
                        imageSize.Height += btnX.ImageTextSpacing * 2;
                }

                // Measure string
                Font font = button.GetFont(null, true);

                SizeF textSize = SizeF.Empty;
                eTextFormat stringFormat = eTextFormat.Default | eTextFormat.VerticalCenter;
#if FRAMEWORK20
                if (BarFunctions.IsWindowsXP && BarUtilities.UseTextRenderer) stringFormat |= eTextFormat.LeftAndRightPadding;
#endif
                if (btnX != null || objCtrl is RibbonBar && button.RibbonWordWrap) stringFormat |= eTextFormat.WordBreak;

                if (button.Text != "")
                {
                    if (button.TextMarkupBody == null)
                    {
                        if (button.Orientation == eOrientation.Vertical && !isOnMenu)
                            textSize = TextDrawing.MeasureStringLegacy(g, ButtonItemPainter.GetDrawText(button.Text), font, new Size(measureStringWidth, 0), stringFormat);
                        else
                        {
                            textSize = TextDrawing.MeasureString(g, ButtonItemPainter.GetDrawText(button.Text), font, measureStringWidth, stringFormat);
                            #if FRAMEWORK20
                            if (BarFunctions.IsWindowsXP && BarUtilities.UseTextRenderer) textSize.Width += 2;
                            #endif
                        }
                    }
                    else
                    {
                        Size availSize = new Size(measureStringWidth, 1);
                        if (measureStringWidth == 0)
                            availSize.Width = 1600;
                        TextMarkup.MarkupDrawContext d = new TextMarkup.MarkupDrawContext(g, font, Color.Empty, false);
                        d.RightToLeft = rightToLeft;
                        button.TextMarkupBody.Measure(availSize, d);
                        availSize = button.TextMarkupBody.Bounds.Size;
                        button.TextMarkupBody.Arrange(new Rectangle(0, 0, availSize.Width, availSize.Height), d);

                        textSize = button.TextMarkupBody.Bounds.Size;
                    }
                }

                if (button.Orientation == eOrientation.Horizontal && (imagePosition == eImagePosition.Left || imagePosition == eImagePosition.Right))
                {
                    // Recalc size for the Bar button
                    // Add 8 pixel padding to the image size, 4 pixels on each side
                    if (button.ButtonStyle != eButtonStyle.Default || !bHasImage)
                        imageSize.Width += 4;

                    imageDrawRect = Rectangle.Empty;
                    if (button.ButtonStyle != eButtonStyle.TextOnlyAlways && bHasImage)
                    {
                        // We know the image position now, we will center it into this area
                        if (imagePosition == eImagePosition.Left)
                        {
                            if (btnX!=null && btnX.TextAlignment == eButtonTextAlignment.Left)
                                imageDrawRect = new Rectangle(2, (bounds.Height - imageSize.Height) / 2, imageSize.Width, imageSize.Height);
                            else if (btnX != null && btnX.TextAlignment == eButtonTextAlignment.Right)
                                imageDrawRect = new Rectangle(bounds.Width - (imageSize.Width + (int)Math.Ceiling(textSize.Width) + 4), (bounds.Height - imageSize.Height) / 2, imageSize.Width, imageSize.Height);
                            else if(btnX!=null)
                                imageDrawRect = new Rectangle(/*bounds.X+*/(int)(bounds.Width - (textSize.Width + imageSize.Width)) / 2, (bounds.Height - imageSize.Height) / 2, imageSize.Width, imageSize.Height);
                            else
                                imageDrawRect = new Rectangle(0, (bounds.Height - imageSize.Height) / 2, imageSize.Width, imageSize.Height);
                        }
                        else
                        {
                            if (btnX != null && btnX.TextAlignment == eButtonTextAlignment.Left)
                                imageDrawRect = new Rectangle((int)textSize.Width + 4, (bounds.Height - imageSize.Height) / 2, imageSize.Width, imageSize.Height);
                            else if (btnX != null && btnX.TextAlignment == eButtonTextAlignment.Right)
                                imageDrawRect = new Rectangle(bounds.Width - imageSize.Width, (bounds.Height - imageSize.Height) / 2, imageSize.Width, imageSize.Height);
                            else if(btnX!=null)
                                imageDrawRect = new Rectangle(bounds.Width - (int)(bounds.Width - (textSize.Width + imageSize.Width)) / 2 - (imageSize.Width), (bounds.Height - imageSize.Height) / 2, imageSize.Width, imageSize.Height);
                            else
                                imageDrawRect = new Rectangle(bounds.Width - imageSize.Width, (bounds.Height - imageSize.Height) / 2, imageSize.Width, imageSize.Height);
                        }
                    }

                    // Draw Text only if needed
                    textDrawRect = Rectangle.Empty;
                    if (button.ButtonStyle != eButtonStyle.Default || !bHasImage)
                    {
                        if (bHasImage)
                        {
                            if (imagePosition == eImagePosition.Left)
                            {
                                if (btnX != null && btnX.TextAlignment == eButtonTextAlignment.Center)
                                    textDrawRect = new Rectangle(imageDrawRect.Right + 1, (int)((bounds.Height - textSize.Height) / 2), (int)textSize.Width, (int)textSize.Height);
                                else if (btnX != null && (btnX.TextAlignment == eButtonTextAlignment.Right || btnX.TextAlignment == eButtonTextAlignment.Left))
                                    textDrawRect = new Rectangle(imageDrawRect.Right + 1, (int)((bounds.Height - textSize.Height) / 2), bounds.Width - 3 - imageDrawRect.Width, (int)textSize.Height);
                                else
                                    textDrawRect = new Rectangle(imageDrawRect.Right + 1, (int)((bounds.Height - textSize.Height) / 2), bounds.Width - 3 - imageDrawRect.Width, (int)textSize.Height);
                            }
                            else
                            {
                                if (btnX != null && btnX.TextAlignment == eButtonTextAlignment.Center)
                                    textDrawRect = new Rectangle((int)(imageDrawRect.X - textSize.Width) - 1, (int)((bounds.Height - textSize.Height) / 2), (int)textSize.Width, (int)textSize.Height);
                                else if (btnX != null && btnX.TextAlignment == eButtonTextAlignment.Right)
                                    textDrawRect = new Rectangle(imageDrawRect.X - ((int)textSize.Width + 4), (int)((bounds.Height - textSize.Height) / 2), (int)textSize.Width+2, (int)textSize.Height);
                                else
                                    textDrawRect = new Rectangle(3, (int)((bounds.Height - textSize.Height) / 2), imageDrawRect.X - 2, (int)textSize.Height);
                            }
                        }
                        else
                        {
                            if (btnX != null && btnX.TextAlignment == eButtonTextAlignment.Center || button._FixedSizeCenterText)
                                textDrawRect = new Rectangle(/*bounds.X+*/(int)((bounds.Width-textSize.Width)/2), (int)((bounds.Height - textSize.Height) / 2), (int)textSize.Width, (int)textSize.Height);
                            else
                                textDrawRect = new Rectangle(/*bounds.X +*/ 3, (int)((bounds.Height - textSize.Height) / 2), bounds.Width - 6, (int)textSize.Height);
                        }
                    }
                }
                else
                {
                    // Image is on top or bottom
                    // Calculate width, that is easy
                    if (button.Orientation == eOrientation.Horizontal)
                    {

                        if (imagePosition == eImagePosition.Top)
                        {
                            imageDrawRect = new Rectangle(0, (int)(bounds.Height - (imageSize.Height + textSize.Height)) / 2, bounds.Width, imageSize.Height/*+2*/);
                            textDrawRect = new Rectangle(0, imageDrawRect.Bottom, (int)textSize.Width, (int)textSize.Height + 5);
                        }
                        else
                        {
                            textDrawRect = new Rectangle((int)(bounds.Width - textSize.Width) / 2, (int)(bounds.Height - (imageSize.Height + textSize.Height)) / 2, (int)textSize.Width, (int)textSize.Height);
                            imageDrawRect = new Rectangle(0, textDrawRect.Bottom, bounds.Width, imageSize.Height + 5);
                        }
                    }
                    else
                    {
                        if (imagePosition == eImagePosition.Top || imagePosition == eImagePosition.Left)
                        {
                            if (bHasImage)
                                imageDrawRect = new Rectangle(0, 0, bounds.Width, imageSize.Height + 6);
                            textDrawRect = new Rectangle((int)(bounds.Width - textSize.Height) / 2, imageDrawRect.Bottom + 2, (int)textSize.Height, (int)textSize.Width + 5);
                        }
                        else
                        {
                            textDrawRect = new Rectangle((int)(bounds.Width - textSize.Width) / 2, 0, (int)textSize.Height, (int)textSize.Width + 5);
                            if (bHasImage)
                                imageDrawRect = new Rectangle(0, textDrawRect.Bottom + 2, bounds.Width, imageSize.Height + 5);
                        }
                    }
                }
            }
            finally
            {
                g.TextRenderingHint = TextRenderingHint.SystemDefault;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
                g.Dispose();
            }
            
            button.ImageDrawRect = imageDrawRect;
            button.TextDrawRect = textDrawRect;
            button.SubItemsRect = subItemsRect;
        }

        private struct ButtonTextSize
        {
            public Size TextSize;
            public Size ShortcutTextSize;
        }
	}
}
