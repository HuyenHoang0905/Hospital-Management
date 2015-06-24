#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing;
using DevComponents.AdvTree;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using DevComponents.DotNetBar.Controls;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Defines Advanced Property Grid Node for Color type.
    /// </summary>
    public class PropertyNodeColor : PropertyNode
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the PropertyNode class.
        /// </summary>
        /// <param name="property"></param>
        public PropertyNodeColor(PropertyDescriptor property) : base(property)
        {
            
        }
        #endregion

        #region Internal Implementation
        private Image _SwatchImage = null;
        protected override void UpdateDisplayedValue(object propertyValue)
        {
            base.UpdateDisplayedValue(propertyValue);
            if (IsDisposing || IsDisposed) return;

            Cell cell = this.EditCell;
            if (propertyValue is Color)
            {
                Color color = (Color)propertyValue;
                if (_SwatchImage == null || !((Color)_SwatchImage.Tag).Equals(color))
                {
                    DisposeSwatchImage();
                    _SwatchImage = CreateSwatchImage(color);
                    cell.Images.Image = _SwatchImage;
                }
            }
            else
            {
                DisposeSwatchImage();
                cell.Images.Image = null;
            }
        }
        protected override bool IsTypeEditorPaintValueSupported
        {
            get
            {
                return false;
            }
        }
        private Image CreateSwatchImage(Color color)
        {
            Rectangle r = new Rectangle(0, 0, 20, 13);
            Bitmap image = new Bitmap(r.Width, r.Height, PixelFormat.Format32bppArgb);
            using (Graphics g = Graphics.FromImage(image))
            {
                DisplayHelp.DrawRectangle(g, SwatchBorderColor, r);
                r.Inflate(-1, -1);

                if (color == Color.Empty)
                {
                    using (HatchBrush brush = new HatchBrush(HatchStyle.BackwardDiagonal, SwatchHatchForeColor, SwatchBackColor))
                        g.FillRectangle(brush, r);
                }
                else if (color == Color.Transparent || color.A < 255)
                {
                    using (HatchBrush brush = new HatchBrush(HatchStyle.LargeCheckerBoard, SwatchHatchForeColor, SwatchBackColor))
                        g.FillRectangle(brush, r);

                    if (color != Color.Transparent)
                    {
                        using (SolidBrush brush = new SolidBrush(color))
                            g.FillRectangle(brush, r);
                    }
                }
                else
                {
                    using (SolidBrush brush = new SolidBrush(color))
                        g.FillRectangle(brush, r);
                }

            }
            image.Tag = color;
            return image;
        }

        private Color _SwatchHatchForeColor = ColorScheme.GetColor(0xC5C5C5);
        private Color SwatchHatchForeColor
        {
            get
            {
                return _SwatchHatchForeColor;
            }
        }

        private Color _SwatchBorderColor = Color.Black;
        private Color SwatchBorderColor
        {
            get
            {
                return _SwatchBorderColor;
            }
        }
        private Color _SwatchBackColor = Color.White;
        private Color SwatchBackColor
        {
            get
            {
                return _SwatchBackColor;
            }
        }

        private void DisposeSwatchImage()
        {
            if (_SwatchImage != null)
            {
                _SwatchImage.Dispose();
                _SwatchImage = null;
                Cell cell = this.EditCell;
                if (cell != null)
                    cell.Images.Image = null;
            }
        }

        protected override void Dispose(bool disposing)
        {
            DisposeSwatchImage();
            base.Dispose(disposing);
        }
        #endregion


    }
}
#endif