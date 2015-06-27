using System;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using DevComponents.AdvTree;
using System.Drawing.Drawing2D;
using DevComponents.DotNetBar;

namespace DevComponents.WinForms.Drawing
{
    internal class RectangleShape : Shape
    {
        #region Internal Implementation
        /// <summary>
        /// Renders rectangle on canvas.
        /// </summary>
        /// <param name="g">Target graphics to render shape on.</param>
        /// <param name="bounds">Shape bounds.</param>
        public override void Paint(Graphics g, Rectangle bounds)
        {
            if (bounds.Width < 2 || bounds.Height < 2 || g == null || _Fill == null && _Border == null) return;

            GraphicsPath path = null;
            
            if (!_CornerRadius.IsZero)
            {
                path = DisplayHelp.GetRoundedRectanglePath(bounds, _CornerRadius.TopLeft, _CornerRadius.TopRight, 
                    _CornerRadius.BottomRight, _CornerRadius.BottomLeft);
            }

            if (_Fill != null)
            {
                Brush brush = _Fill.CreateBrush(bounds);
                if (brush != null)
                {
                    if (path == null)
                        g.FillRectangle(brush, bounds);
                    else
                        g.FillPath(brush, path);
                    
                    brush.Dispose();
                }
            }

            if (_Border != null)
            {
                Pen pen = _Border.CreatePen();
                if (pen != null)
                {
                    if (path == null)
                        g.DrawRectangle(pen, bounds);
                    else
                        g.DrawPath(pen, path);

                    pen.Dispose();
                }
            }

            Shape content = this.Content;
            if (content != null)
            {
                Rectangle contentBounds = Border.Deflate(bounds, _Border);
                Region oldClip = null;
                if (path != null && ClipToBounds)
                {
                    oldClip = g.Clip;
                    g.SetClip(path, CombineMode.Intersect);
                }
                content.Paint(g, contentBounds);
                if (oldClip != null) g.Clip = oldClip;
            }

            if (path != null) path.Dispose();
        }

        private Border _Border;
        /// <summary>
        /// Gets or sets shape border.
        /// </summary>
        [DefaultValue(null), Description("Indicates shape border.")]
        public Border Border
        {
            get { return _Border; }
            set { _Border = value; }
        }

        private Fill _Fill = null;
        /// <summary>
        /// Gets or sets the shape fill.
        /// </summary>
        [DefaultValue(null), Description("Indicates shape fill")]
        public Fill Fill
        {
            get { return _Fill; }
            set { _Fill = value; }
        }

        private CornerRadius _CornerRadius;
        /// <summary>
        /// Gets or sets the CornerRadius.
        /// </summary>
        public CornerRadius CornerRadius
        {
            get { return _CornerRadius; }
            set { _CornerRadius = value; }
        }
        /// <summary>
        /// Gets whether property should be serialized.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeCornerRadius()
        {
            return !_CornerRadius.IsZero;
        }
        /// <summary>
        /// Resets the property to its default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetCornerRadius()
        {
            CornerRadius = new CornerRadius();
        }
        #endregion
    }
}
