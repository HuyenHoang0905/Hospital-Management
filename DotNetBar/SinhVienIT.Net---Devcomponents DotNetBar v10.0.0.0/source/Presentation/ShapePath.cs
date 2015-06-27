using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;

namespace DevComponents.DotNetBar.Presentation
{
    internal class ShapePath : Shape
    {
        #region Private Variables
        private ShapeBorder m_Border = null;
        private ShapeFill m_Fill = null;
        private GraphicsPath m_Path = null;
        #endregion

        #region Internal Implementation
        public ShapePath() { }
        public ShapePath(ShapeBorder border)
        {
            m_Border = border;
        }
        public ShapePath(ShapeFill fill)
        {
            m_Fill = fill;
        }
        public ShapePath(ShapeBorder border, ShapeFill fill)
        {
            m_Border = border;
            m_Fill = fill;
        }

        public GraphicsPath Path
        {
            get { return m_Path; }
            set { m_Path = value; }
        }

        public override void Paint(ShapePaintInfo p)
        {
            if(m_Path == null)
                return;
            
            System.Drawing.Rectangle bounds = this.GetBounds(p.Bounds);
            Graphics g = p.Graphics;

            if (m_Fill != null)
                PaintFill(g, bounds);

            if (m_Border != null)
                PaintBorder(g, bounds);

            base.Paint(p);
        }

        /// <summary>
        /// Paints the border.
        /// </summary>
        protected virtual void PaintBorder(Graphics g, System.Drawing.Rectangle r)
        {
            if (m_Border.Color1.IsEmpty)
                return;

            ShapeBorder border = m_Border;

            using (GraphicsPath path = m_Path.Clone() as GraphicsPath)
            {
                path.Transform(new Matrix(1, 0, 0, 1, r.X, r.Y));
                DisplayHelp.DrawGradientPathBorder(g, path, border.Color1, border.Color2, border.GradientAngle, border.Width);
            }
        }

        /// <summary>
        /// Paints the border.
        /// </summary>
        protected virtual void PaintFill(Graphics g, System.Drawing.Rectangle r)
        {
            if (r.Width <= 0 || r.Height <= 0 || m_Fill==null)
                return;


            using (GraphicsPath path = m_Path.Clone() as GraphicsPath)
            {
                // Center path inside of the bounds by default
                System.Drawing.Rectangle pathBounds = System.Drawing.Rectangle.Ceiling(path.GetBounds());
                int w = r.X + (int)Math.Ceiling((double)(Math.Max(r.Width, pathBounds.Width) - pathBounds.Width) / 2);
                int h = r.Y + (r.Height - pathBounds.Height) / 2;
                using (Matrix matrix = new Matrix(1, 0, 0, 1, w, h))
                    path.Transform(matrix);
                pathBounds = System.Drawing.Rectangle.Ceiling(path.GetBounds());

                Brush brush = m_Fill.CreateBrush(pathBounds);
                if (brush != null)
                {
                    g.FillPath(brush, path);

                    brush.Dispose();
                }
            }
        }

        //protected override Region ClipChildren(Graphics g, System.Drawing.Rectangle childBounds)
        //{
        //    if (m_CornerSize > 0)
        //    {
        //        Region clip = g.Clip;
        //        g.SetClip(DisplayHelp.GetRoundedRectanglePath(childBounds, m_CornerSize, eStyleBackgroundPathPart.Complete, m_TopLeftCornerType,
        //            m_TopRightCornerType, m_BottomLeftCornerType, m_BottomRightCornerType));
        //        return clip;
        //    }

        //    return base.ClipChildren(g, childBounds);
        //}

        /// <summary>
        /// Gets the shape border.
        /// </summary>
        public ShapeBorder Border
        {
            get { return m_Border; }
            set { m_Border = value; }
        }

        /// <summary>
        /// Gets the shape fill.
        /// </summary>
        public ShapeFill Fill
        {
            get { return m_Fill; }
            set { m_Fill = value; }
        }
        #endregion
        
    }
}
