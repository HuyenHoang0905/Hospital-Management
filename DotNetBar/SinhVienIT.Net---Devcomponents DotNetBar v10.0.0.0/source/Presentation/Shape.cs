using System;
using System.Text;
using System.Drawing;

namespace DevComponents.DotNetBar.Presentation
{
    internal class Shape
    {
        #region Private Variables
        private Location m_Location = new Location();
        private SizeInfo m_Size = new SizeInfo();
        private ShapeCollection m_Children = null;
        private PaddingInfo m_Padding = null;
        private bool m_SetChildClip = false;
        #endregion

        #region Internal Implementation
        /// <summary>
        /// Gets the location of the shape.
        /// </summary>
        public Location Location
        {
            get { return m_Location; }
        }

        /// <summary>
        /// Gets the size of the shape.
        /// </summary>
        public SizeInfo Size
        {
            get { return m_Size; }
        }

        /// <summary>
        /// Gets the shape padding. Padding is the inside spacing between shape and it's child shapes.
        /// </summary>
        public PaddingInfo Padding
        {
            get { return m_Padding; }
            set
            {
                m_Padding = value;
            }
        }

        /// <summary>
        /// Gets the collection of child shapes.
        /// </summary>
        public ShapeCollection Children
        {
            get
            {
                if (m_Children == null)
                    m_Children = new ShapeCollection();
                return m_Children;
            }
        }

        /// <summary>
        /// Gets or sets whether this shape will set the ShapePaintInfo.ChildContentClip property to the region that represents the inside content of the shape.
        /// This is used when there is inside content of the shape which is not part of the shape itself and calling routine needs
        /// access to the region that defines the shape inside bounds.
        /// </summary>
        public bool SetChildClip
        {
            get { return m_SetChildClip; }
            set { m_SetChildClip = value; }
        }

        /// <summary>
        /// Paints the shape on canvas. If overriden base implementation must be called to paint any child shapes.
        /// </summary>
        /// <param name="p">Shape paint information.</param>
        public virtual void Paint(ShapePaintInfo p)
        {
            if (m_Children != null && m_Children.Count > 0)
            {
                System.Drawing.Rectangle originalBounds = p.Bounds;
                System.Drawing.Rectangle bounds = new System.Drawing.Rectangle(this.GetLocation(p.Bounds), this.GetSize(p.Bounds));

                if (m_Padding != null)
                {
                    bounds.Width -= m_Padding.HorizontalPadding;
                    bounds.Height -= m_Padding.VerticalPadding;
                    bounds.X += m_Padding.Left;
                    bounds.Y += m_Padding.Top;
                }

                p.Bounds = bounds;
                Region clip = ClipChildren(p.Graphics, bounds);
                if (m_SetChildClip && p.Graphics.Clip!=null)
                {
                    p.ChildContentClip = p.Graphics.Clip as Region;
                }

                foreach (Shape shape in m_Children)
                {
                    shape.Paint(p);
                }
                if (p.ChildContentClip != null)
                {
                    p.ChildContentClip.Dispose();
                    p.ChildContentClip = null;
                }
                if (clip != null)
                {
                    p.Graphics.Clip = clip;
                    clip.Dispose();
                }
                else
                    p.Graphics.ResetClip();
                p.Bounds = originalBounds;
                if (clip != null) clip.Dispose();
            }
        }

        protected virtual Region ClipChildren(Graphics g, System.Drawing.Rectangle childBounds)
        {
            Region clip = g.Clip;
            g.SetClip(childBounds);
            return clip;
        }

        /// <summary>
        /// Returns absolute location of the shape based on parent bounds.
        /// </summary>
        /// <param name="bounds">Parent absolute bounds.</param>
        /// <returns>Absolute location of the shape</returns>
        protected virtual Point GetLocation(System.Drawing.Rectangle bounds)
        {
            return GetLocation(bounds, m_Location);
        }

        protected virtual Point GetLocation(System.Drawing.Rectangle bounds, Location refLocation)
        {
            Point loc = bounds.Location;

            if (refLocation.RelativeX == eRelativeLocation.Right)
                loc.X = bounds.Right;
            else if (refLocation.RelativeX == eRelativeLocation.Top)
                loc.X = bounds.Y;
            else if (refLocation.RelativeX == eRelativeLocation.Bottom)
                loc.X = bounds.Bottom;

            if (refLocation.RelativeY == eRelativeLocation.Bottom)
                loc.Y = bounds.Bottom;
            else if (refLocation.RelativeY == eRelativeLocation.Right)
                loc.Y = bounds.Right;
            else if (refLocation.RelativeY == eRelativeLocation.Left)
                loc.Y = bounds.X;

            loc.Offset(refLocation.X, refLocation.Y);

            return loc;
        }

        /// <summary>
        /// Returns absolute size of the shape based on the parent bounds.
        /// </summary>
        /// <param name="bounds">Absolute parent bounds.</param>
        /// <returns>Absolute size of the shape.</returns>
        protected virtual Size GetSize(System.Drawing.Rectangle bounds)
        {
            Size size = bounds.Size;

            if (m_Size.RelativeWidth == eRelativeSize.Width)
                size.Width = bounds.Width + m_Size.Width;
            else if (m_Size.RelativeWidth == eRelativeSize.Height)
                size.Width = bounds.Height + m_Size.Height;
            else if (m_Size.Width != 0)
                size.Width = m_Size.Width;

            if (m_Size.RelativeHeight == eRelativeSize.Height)
                size.Height = bounds.Height + m_Size.Height;
            else if (m_Size.RelativeHeight == eRelativeSize.Width)
                size.Height = bounds.Width + m_Size.Height;
            else if (m_Size.Height != 0)
                size.Height = m_Size.Height;

            return size;
        }

        /// <summary>
        /// Gets the absolute bounds of the shape.
        /// </summary>
        /// <param name="parentBounds">Parent bounds.</param>
        /// <returns>Absolute bounds of the shape.</returns>
        protected virtual System.Drawing.Rectangle GetBounds(System.Drawing.Rectangle parentBounds)
        {
            return new System.Drawing.Rectangle(GetLocation(parentBounds), GetSize(parentBounds));
        }
        #endregion
    }
}
