using System;
using System.Drawing;
using System.Text;
using System.Xml;

#if AdvTree
namespace DevComponents.Tree.TextMarkup
#elif DOTNETBAR
using DevComponents.UI.ContentManager;
namespace DevComponents.DotNetBar.TextMarkup
#elif SUPERGRID
namespace DevComponents.SuperGrid.TextMarkup
#endif
{
    internal abstract class MarkupElement : IBlockExtended
    {
        #region Private Variables
        private MarkupElementCollection m_Elements = null;
        private MarkupElement m_Parent = null;
        private Rectangle m_Bounds = Rectangle.Empty;
        private bool m_Visible = true;
        private bool m_SizeValid = false;
        private Rectangle m_RenderBounds = Rectangle.Empty;
        #endregion

        #region Internal Implementation
        public MarkupElement()
        {
            m_Elements = new MarkupElementCollection(this);
        }

        /// <summary>
        /// Returns whether markup element is an block element that always consumes a whole line in layout.
        /// </summary>
        public virtual bool IsBlockElement
        {
            get { return false; }
        }

        /// <summary>
        /// Returns whether layout manager switches to new line after processing this element.
        /// </summary>
        public virtual bool IsNewLineAfterElement
        {
            get { return false; }
        }

        /// <summary>
        /// Returns whether layout manager can start new line with this element.
        /// </summary>
        public virtual bool CanStartNewLine
        {
            get { return true; }
        }

        /// <summary>
        /// Gets the collection of child elements if any for this markup element.
        /// </summary>
        public virtual MarkupElementCollection Elements
        {
            get
            {
                if (m_Elements == null)
                    m_Elements = new MarkupElementCollection(this);
                return m_Elements; 
            }
        }

        internal void InvalidateElementsSize()
        {
            this.IsSizeValid = false;
            if (m_Elements==null || m_Elements.Count == 0)
                return;
            foreach (MarkupElement e in m_Elements)
            {
                e.InvalidateElementsSize();
                e.IsSizeValid = false;
            }
        }

        /// <summary>
        /// Gets or sets whether element size is valid. When size is not valid element Measure method will be called to validate size.
        /// </summary>
        public virtual bool IsSizeValid
        {
            get { return m_SizeValid; }
            set { m_SizeValid = value; }
        }

        /// <summary>
        /// Gets element parent or null if parent is not set.
        /// </summary>
        public virtual MarkupElement Parent
        {
            get { return m_Parent; }
        }

        internal void SetParent(MarkupElement parent)
        {
            m_Parent = parent;
        }

        /// <summary>
        /// Gets or sets actual rendering bounds.
        /// </summary>
        public Rectangle Bounds
        {
            get { return m_Bounds; }
            set { m_Bounds = value; }
        }

        /// <summary>
        /// Gets or sets whether markup element is visible.
        /// </summary>
        public bool Visible
        {
            get { return m_Visible; }
            set { m_Visible = value; }
        }

        /// <summary>
        /// Measures the element given available size.
        /// </summary>
        /// <param name="availableSize">Size available to element</param>
        /// <param name="g">Reference to graphics object</param>
        public abstract void Measure(Size availableSize, MarkupDrawContext d);

        /// <summary>
        /// Measures the end tag of an element. Most implementations do not need to do anything but implementations like the ones
        /// that change color should return state back at this time.
        /// </summary>
        /// <param name="availableSize"></param>
        /// <param name="d"></param>
        public virtual void MeasureEnd(Size availableSize, MarkupDrawContext d) { }

        /// <summary>
        /// Renders element.
        /// </summary>
        /// <param name="d">Provides markup drawing context information.</param>
        public abstract void Render(MarkupDrawContext d);

        /// <summary>
        /// Renders element tag end. Most implementations do not need to do anything but mplementations like the ones
        /// that change color should return state back at this time.
        /// </summary>
        /// <param name="d">Provides markup drawing context information.</param>
        public virtual void RenderEnd(MarkupDrawContext d) { }

        /// <summary>
        /// Provides final rectangle to element and lets it arrange it's content given new constraint.
        /// </summary>
        /// <param name="finalRect">Final rectangle.</param>
        /// <param name="g"></param>
        protected abstract void ArrangeCore(Rectangle finalRect, MarkupDrawContext d);

        /// <summary>
        /// Arranges the element given the final size. Layout is two step process with Measure followed by Arrange.
        /// </summary>
        /// <param name="finalSize"></param>
        /// <param name="g"></param>
        public void Arrange(Rectangle finalSize, MarkupDrawContext d)
        {
            this.ArrangeCore(finalSize, d);
        }

        public virtual void ReadAttributes(XmlTextReader reader) { }

        /// <summary>
        /// Gets or sets actual rendered bounds for a give markup element if applicable.
        /// </summary>
        public Rectangle RenderBounds
        {
            get { return m_RenderBounds; }
            set { m_RenderBounds = value; }
        }
        #endregion
    }
}
