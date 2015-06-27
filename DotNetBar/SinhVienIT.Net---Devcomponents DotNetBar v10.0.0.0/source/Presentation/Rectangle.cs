using System;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DevComponents.DotNetBar.Presentation
{
    internal class Rectangle : Shape
    {
        #region Private Variables
        private int m_CornerSize = 0;
        private ShapeBorder m_Border = new ShapeBorder();
        private ShapeFill m_Fill = new ShapeFill();
        private eCornerType m_TopLeftCornerType = eCornerType.Square;
        private eCornerType m_TopRightCornerType = eCornerType.Square;
        private eCornerType m_BottomLeftCornerType = eCornerType.Square;
        private eCornerType m_BottomRightCornerType = eCornerType.Square;
        #endregion

        #region Internal Implementation
        public Rectangle() { }
        public Rectangle(ShapeBorder border)
        {
            m_Border = border;
        }
        public Rectangle(ShapeBorder border, int cornerSize, eCornerType cornerType)
        {
            m_Border = border;
            m_CornerSize = cornerSize;
            this.CornerType = cornerType;
        }
        public Rectangle(ShapeBorder border, ShapeFill fill)
        {
            m_Border = border;
            m_Fill = fill;
        }
        public Rectangle(ShapeBorder border, ShapeFill fill,int cornerSize, eCornerType cornerType)
        {
            m_Border = border;
            m_Fill = fill;
            m_CornerSize = cornerSize;
            this.CornerType = cornerType;
        }
        public Rectangle(ShapeBorder border, ShapeFill fill, int cornerSize, eCornerType cornerType, PaddingInfo padding)
        {
            m_Border = border;
            m_Fill = fill;
            m_CornerSize = cornerSize;
            this.Padding = padding;
            this.CornerType = cornerType;
        }
        public Rectangle(ShapeFill fill)
        {
            m_Fill = fill;
        }
        public override void Paint(ShapePaintInfo p)
        {
            System.Drawing.Rectangle bounds = this.GetBounds(p.Bounds);
            Graphics g = p.Graphics;

            PaintFill(g, bounds);
            PaintBorder(g, bounds);

            base.Paint(p);
        }

        /// <summary>
        /// Paints the border.
        /// </summary>
        protected virtual void PaintBorder(Graphics g, System.Drawing.Rectangle r)
        {
            if (m_Border.Width == 0 || m_Border.Color1.IsEmpty || r.Width <= 0 || r.Height <= 0)
                return;
            
            int roundCornerSize = m_CornerSize;
            ShapeBorder border = m_Border;

            // Workaround for GDI+ bug
            r.Width--;
            r.Height--;

            using (System.Drawing.Drawing2D.GraphicsPath path = DisplayHelp.GetRoundedRectanglePath(r, m_CornerSize, eStyleBackgroundPathPart.Complete,
                m_TopLeftCornerType, m_TopRightCornerType, m_BottomLeftCornerType, m_BottomRightCornerType))
            {

                using (Pen pen = new Pen(border.Color1, border.Width))
                    path.Widen(pen);

                if (border.Color2.IsEmpty)
                {
                    using (SolidBrush brush = new SolidBrush(border.Color1))
                        g.FillPath(brush, path);
                }
                else
                {
                    using (System.Drawing.Drawing2D.LinearGradientBrush brush = DisplayHelp.CreateLinearGradientBrush(r, border.Color1, border.Color2, border.GradientAngle))
                        g.FillPath(brush, path);
                }
            }
        }

        /// <summary>
        /// Paints the border.
        /// </summary>
        protected virtual void PaintFill(Graphics g, System.Drawing.Rectangle r)
        {
            if (r.Width <= 0 || r.Height <= 0)
                return;

            int roundCornerSize = m_CornerSize;

            if (roundCornerSize > 0)
            {
                using (System.Drawing.Drawing2D.GraphicsPath path = DisplayHelp.GetRoundedRectanglePath(r, m_CornerSize, eStyleBackgroundPathPart.Complete,
                m_TopLeftCornerType, m_TopRightCornerType, m_BottomLeftCornerType, m_BottomRightCornerType))
                {
                    Brush brush = m_Fill.CreateBrush(System.Drawing.Rectangle.Ceiling(path.GetBounds()));
                    if (brush != null)
                    {
                        DisplayHelp.FillPath(g, path, m_Fill.Color1, m_Fill.Color2, m_Fill.GradientAngle);
                        brush.Dispose();
                    }
                }
            }
            else
            {
                Brush brush = m_Fill.CreateBrush(r);
                if (brush != null)
                {
                    g.FillRectangle(brush, r);
                    brush.Dispose();
                }
            }
        }

        protected override Region ClipChildren(Graphics g, System.Drawing.Rectangle childBounds)
        {
            if (m_CornerSize > 0)
            {
                Region clip = g.Clip;
                using (GraphicsPath path = DisplayHelp.GetRoundedRectanglePath(childBounds, m_CornerSize, 
                    eStyleBackgroundPathPart.Complete, m_TopLeftCornerType, m_TopRightCornerType, m_BottomLeftCornerType, m_BottomRightCornerType))
                {
                    g.SetClip(path);
                }
                return clip;
            }

            return base.ClipChildren(g, childBounds);
        }

        /// <summary>
        /// Gets or sets the rounded corner size.
        /// </summary>
        public int CornerSize
        {
            get { return m_CornerSize; }
            set {m_CornerSize = value;}
        }

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

        public eCornerType CornerType
        {
            get { return m_TopLeftCornerType; }
            set
            {
                m_TopLeftCornerType = value;
                m_TopRightCornerType = value;
                m_BottomLeftCornerType = value;
                m_BottomRightCornerType = value;
            }
        }

        public eCornerType TopLeftCornerType
        {
            get { return m_TopLeftCornerType; }
            set { m_TopLeftCornerType = value; }
        }

        public eCornerType TopRightCornerType
        {
            get { return m_TopRightCornerType; }
            set { m_TopRightCornerType = value; }
        }

        public eCornerType BottomLeftCornerType
        {
            get { return m_BottomLeftCornerType; }
            set { m_BottomLeftCornerType = value; }
        }

        public eCornerType BottomRightCornerType
        {
            get { return m_BottomRightCornerType; }
            set { m_BottomRightCornerType = value; }
        }
        #endregion
    }
}
