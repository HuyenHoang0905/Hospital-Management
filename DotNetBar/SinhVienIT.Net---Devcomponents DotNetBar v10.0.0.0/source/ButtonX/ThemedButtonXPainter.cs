using System;
using System.Drawing;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Summary description for ThemedButtonItemPainter.
    /// </summary>
    internal class ThemedButtonXPainter
    {
        public static void PaintButton(ButtonItem button, ItemPaintArgs pa)
        {
            System.Drawing.Graphics g = pa.Graphics;
            ThemeButton theme = pa.ThemeButton;
            ThemeButtonParts part = ThemeButtonParts.PushButton;
            ThemeButtonStates state = ThemeButtonStates.PushButtonNormal;
            Color textColor = ButtonItemPainterHelper.GetTextColor(button, pa);

            Rectangle rectImage = Rectangle.Empty;
            Rectangle itemRect = button.DisplayRectangle;

            Font font = null;
            CompositeImage image = button.GetImage();
            
            eTextFormat format = GetStringFormat(button, pa, image);

            font = button.GetFont(pa, false);

            bool bSplitButton = (button.SubItems.Count > 0 || button.PopupType == ePopupType.Container) && button.ShowSubItems && !button.SubItemsRect.IsEmpty;

            // Calculate image position
            if (image != null)
            {
                if (button.ImagePosition == eImagePosition.Top || button.ImagePosition == eImagePosition.Bottom)
                    rectImage = new Rectangle(button.ImageDrawRect.X, button.ImageDrawRect.Y, itemRect.Width, button.ImageDrawRect.Height);
                else
                    rectImage = new Rectangle(button.ImageDrawRect.X, button.ImageDrawRect.Y, button.ImageDrawRect.Width, button.ImageDrawRect.Height);

                rectImage.Offset(itemRect.Left, itemRect.Top);
                rectImage.Offset((rectImage.Width - button.ImageSize.Width) / 2, (rectImage.Height - button.ImageSize.Height) / 2);
                rectImage.Width = button.ImageSize.Width;
                rectImage.Height = button.ImageSize.Height;
            }

            // Set the state and text brush
            if (!ButtonItemPainter.IsItemEnabled(button, pa))
            {
                state = ThemeButtonStates.PushButtonDisabled;
            }
            else if (button.IsMouseDown || button.Expanded)
            {
                state = ThemeButtonStates.PushButtonPressed;
            }
            else if (button.IsMouseOver && button.Checked)
            {
                state = ThemeButtonStates.PushButtonPressed;
            }
            else if (button.IsMouseOver)
            {
                state = ThemeButtonStates.PushButtonHot;
            }
            else if (button.Checked || button.Expanded)
            {
                state = ThemeButtonStates.PushButtonPressed;
            }
            else if (button.Focused || pa.ContainerControl.Focused)
                state = ThemeButtonStates.PushButtonDefaulted;

            Rectangle backRect = button.DisplayRectangle;
            if (button.HotTrackingStyle == eHotTrackingStyle.Image && image != null)
            {
                backRect = rectImage;
                backRect.Inflate(3, 3);
            }
            //else if (bSplitButton)
            //{
            //    backRect.Width = backRect.Width - button.SubItemsRect.Width;
            //}

            // Draw Button Background
            if (button.HotTrackingStyle != eHotTrackingStyle.None)
            {
                theme.DrawBackground(g, part, state, backRect);
            }

            // Draw Image
            if (image != null && button.ButtonStyle != eButtonStyle.TextOnlyAlways)
            {
                if (state == ThemeButtonStates.PushButtonNormal && button.HotTrackingStyle == eHotTrackingStyle.Color)
                {
                    // Draw gray-scale image for this hover style...
                    float[][] array = new float[5][];
                    array[0] = new float[5] { 0.2125f, 0.2125f, 0.2125f, 0, 0 };
                    array[1] = new float[5] { 0.5f, 0.5f, 0.5f, 0, 0 };
                    array[2] = new float[5] { 0.0361f, 0.0361f, 0.0361f, 0, 0 };
                    array[3] = new float[5] { 0, 0, 0, 1, 0 };
                    array[4] = new float[5] { 0.2f, 0.2f, 0.2f, 0, 1 };
                    System.Drawing.Imaging.ColorMatrix grayMatrix = new System.Drawing.Imaging.ColorMatrix(array);
                    System.Drawing.Imaging.ImageAttributes att = new System.Drawing.Imaging.ImageAttributes();
                    att.SetColorMatrix(grayMatrix);
                    //g.DrawImage(image,rectImage,0,0,image.Width,image.Height,GraphicsUnit.Pixel,att);
                    image.DrawImage(g, rectImage, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, att);
                }
                else if (state == ThemeButtonStates.PushButtonNormal && !image.IsIcon)
                {
                    // Draw image little bit lighter, I decied to use gamma it is easy
                    System.Drawing.Imaging.ImageAttributes lightImageAttr = new System.Drawing.Imaging.ImageAttributes();
                    lightImageAttr.SetGamma(.7f, System.Drawing.Imaging.ColorAdjustType.Bitmap);
                    //g.DrawImage(image,rectImage,0,0,image.Width,image.Height,GraphicsUnit.Pixel,lightImageAttr);
                    image.DrawImage(g, rectImage, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, lightImageAttr);
                }
                else
                {
                    image.DrawImage(g, rectImage);
                }
            }

            // Draw Text
            if (button.ButtonStyle == eButtonStyle.ImageAndText || button.ButtonStyle == eButtonStyle.TextOnlyAlways || image == null)
            {
                Rectangle rectText = button.TextDrawRect;
                if (button.ImagePosition == eImagePosition.Top || button.ImagePosition == eImagePosition.Bottom)
                {
                    if (button.Orientation == eOrientation.Vertical)
                    {
                        rectText = new Rectangle(button.TextDrawRect.X, button.TextDrawRect.Y, button.TextDrawRect.Width, button.TextDrawRect.Height);
                    }
                    else
                    {
                        rectText = new Rectangle(button.TextDrawRect.X, button.TextDrawRect.Y, button.TextDrawRect.Width, button.TextDrawRect.Height);
                        //if ((button.SubItems.Count > 0 || button.PopupType == ePopupType.Container) && button.ShowSubItems)
                        //    rectText.Width -= 10;
                    }
                    format |= eTextFormat.HorizontalCenter;
                }

                rectText.Offset(itemRect.Left, itemRect.Top);

                if (button.Orientation == eOrientation.Vertical)
                {
                    g.RotateTransform(90);
                    TextDrawing.DrawStringLegacy(g, ButtonItemPainter.GetDrawText(button.Text), font, textColor, new Rectangle(rectText.Top, -rectText.Right, rectText.Height, rectText.Width), format);
                    g.ResetTransform();
                }
                else
                {
                    if (rectText.Right > button.DisplayRectangle.Right)
                        rectText.Width = button.DisplayRectangle.Right - rectText.Left;
                    TextDrawing.DrawString(g, ButtonItemPainter.GetDrawText(button.Text), font, textColor, rectText, format);
                    if (!button.DesignMode && button.Focused && !pa.IsOnMenu && !pa.IsOnMenuBar)
                    {
                        Rectangle r = button.Bounds;
                        r.Inflate(-3, -3);
                        System.Windows.Forms.ControlPaint.DrawFocusRectangle(g, r);
                    }
                }
            }

            // If it has subitems draw the triangle to indicate that
            if (bSplitButton)
            {
                ButtonItemPainter.PaintButtonExpandIndicator(button, pa);
            }

            if (image != null)
                image.Dispose();
        }

        private static eTextFormat GetStringFormat(ButtonItem button, ItemPaintArgs pa, CompositeImage image)
        {
            eTextFormat stringFormat = pa.ButtonStringFormat;
            bool isOnMenu = IsOnMenu(button, pa);
            if (!isOnMenu)
            {
                if (image == null || button.ImagePosition == eImagePosition.Top || button.ImagePosition == eImagePosition.Bottom)
                    stringFormat |= eTextFormat.HorizontalCenter;
            }
            return stringFormat;
        }

        private static bool IsOnMenu(ButtonItem button, ItemPaintArgs pa)
        {
            bool isOnMenu = pa.IsOnMenu;
            if (isOnMenu && button.Parent is ItemContainer)
                isOnMenu = false;
            return isOnMenu;
        }
    }
}
