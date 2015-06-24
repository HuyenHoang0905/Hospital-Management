using System;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace DevComponents.DotNetBar.Rendering
{
    /// <summary>
    /// Represents painter for Office 2007 style ribbon overflow button.
    /// </summary>
    internal class Office2007RibbonOverflowPainter : Office2007ButtonItemPainter
    {
        public override Rectangle GetImageRectangle(ButtonItem button, ItemPaintArgs pa, CompositeImage image)
        {
            Rectangle imageRect = Rectangle.Empty;
            bool isOnMenu = IsOnMenu(button, pa);

            // Calculate image position
            if (image != null)
            {
                imageRect.Width = image.Width + 16;
                imageRect.Height = image.Height + 16;
                imageRect.X = button.DisplayRectangle.X + (button.DisplayRectangle.Width - imageRect.Width) / 2;
                imageRect.Y = button.DisplayRectangle.Y + 3;
            }

            return imageRect;
        }

        public override void PaintButtonImage(ButtonItem button, ItemPaintArgs pa, CompositeImage image, Rectangle imagebounds)
        {
            if (imagebounds.Width <= 0 || imagebounds.Height <= 0) return;

            // Paint image background
            RibbonOverflowButtonItem overflow = button as RibbonOverflowButtonItem;
            if (overflow == null || overflow.RibbonBar==null)
            {
                base.PaintButtonImage(button, pa, image, imagebounds);
                return;
            }
            ElementStyle backStyle = overflow.RibbonBar.GetPaintBackgroundStyle();
            ElementStyle titleStyle = overflow.RibbonBar.TitleStyle;

            int cornerSize = 3;

            if (backStyle.BackColorBlend.Count > 0)
                DisplayHelp.FillRoundedRectangle(pa.Graphics, imagebounds, cornerSize, backStyle.BackColorBlend[0].Color, backStyle.BackColorBlend[backStyle.BackColorBlend.Count-1].Color, overflow.RibbonBar.BackgroundStyle.BackColorGradientAngle);
            else
                DisplayHelp.FillRoundedRectangle(pa.Graphics, imagebounds, cornerSize, backStyle.BackColor, backStyle.BackColor2, backStyle.BackColorGradientAngle);
            if(!button.Expanded)
                DisplayHelp.FillRectangle(pa.Graphics, new Rectangle(imagebounds.X+1, imagebounds.Bottom - 8, imagebounds.Width-2, 7), titleStyle.BackColor, titleStyle.BackColor2, titleStyle.BackColorGradientAngle);
            
            if (!backStyle.BorderColor.IsEmpty)
            {
                using (GraphicsPath path = new GraphicsPath())
                {
                    path.AddLine(imagebounds.X, imagebounds.Bottom - 8, imagebounds.Right, imagebounds.Bottom - 8);
                    ElementStyleDisplay.AddCornerArc(path, imagebounds, cornerSize, eCornerArc.BottomRight);
                    ElementStyleDisplay.AddCornerArc(path, imagebounds, cornerSize, eCornerArc.BottomLeft);
                    path.CloseAllFigures();
                    using (SolidBrush brush = new SolidBrush(Color.FromArgb(192, backStyle.BorderColor)))
                        pa.Graphics.FillPath(brush, path);
                }
            }

            DisplayHelp.DrawRoundGradientRectangle(pa.Graphics, imagebounds, backStyle.BorderColor, backStyle.BorderColor2, backStyle.BorderGradientAngle, 1, cornerSize);

            imagebounds.X += (imagebounds.Width - image.Width) / 2;
            imagebounds.Y += 4;
            imagebounds.Width = image.Width;
            imagebounds.Height = image.Height;

            image.DrawImage(pa.Graphics, imagebounds);
        }

        protected override Rectangle GetTextRectangle(ButtonItem button, ItemPaintArgs pa, eTextFormat stringFormat, CompositeImage image)
        {
            Rectangle r = base.GetTextRectangle(button, pa, stringFormat, image);
            r.Offset(0, 12);
            //r.Height -= 18;
            return r;
        }

        protected override void PaintState(ButtonItem button, ItemPaintArgs pa, CompositeImage image, Rectangle r, bool isMouseDown)
        {
            
        }
    }
}
