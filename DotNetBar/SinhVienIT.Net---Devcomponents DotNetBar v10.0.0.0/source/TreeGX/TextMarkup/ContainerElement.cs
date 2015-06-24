using System;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

#if TREEGX
namespace DevComponents.Tree.TextMarkup
#elif DOTNETBAR
using DevComponents.UI.ContentManager;
namespace DevComponents.DotNetBar.TextMarkup
#elif SUPERGRID
namespace DevComponents.SuperGrid.TextMarkup
#endif
{
    internal class ContainerElement : MarkupElement
    {
        #region Private Variables
        private SerialContentLayoutManager m_Layout = null;
        private MarkupLayoutManager m_MarkupLayout = null;
        #endregion

        #region Internal Implementation
        public override void Measure(Size availableSize, MarkupDrawContext d)
        {
            ArrangeInternal(new Rectangle(Point.Empty, availableSize), d);
        }

        protected virtual SerialContentLayoutManager GetLayoutManager(bool mutliLine)
        {
            if (m_Layout == null)
            {
                m_Layout = new SerialContentLayoutManager();
                m_Layout.MultiLine = mutliLine;
                m_Layout.ContentVerticalAlignment = eContentVerticalAlignment.Top;
                m_Layout.BlockLineAlignment = eContentVerticalAlignment.Top;
                m_Layout.BlockSpacing = 0;
            }

            m_Layout.EvenHeight = true;
            return m_Layout;
        }

        private MarkupLayoutManager GetMarkupLayout()
        {
            if (m_MarkupLayout == null)
            {
                m_MarkupLayout = new MarkupLayoutManager();
            }
            return m_MarkupLayout;
        }

        public override void Render(MarkupDrawContext d)
        {
            Point offset = this.GetContainerOffset();
            d.Offset.Offset(offset.X, offset.Y);

            try
            {
                foreach (MarkupElement e in this.Elements)
                {
                    e.Render(d);
                }
            }
            finally
            {
                d.Offset.Offset(-offset.X, -offset.Y);
            }
        }

        protected virtual Point GetContainerOffset()
        {
            return this.Bounds.Location;
        }

        protected override void ArrangeCore(Rectangle finalRect, MarkupDrawContext d)
        {
            this.Bounds = finalRect;
            ArrangeInternal(finalRect, d);
        }

        protected virtual void ArrangeInternal(Rectangle bounds, MarkupDrawContext d)
        {
            SerialContentLayoutManager layout = GetLayoutManager(d.AllowMultiLine);
            layout.RightToLeft = d.RightToLeft;
            MarkupLayoutManager markupLayout = GetMarkupLayout();
            markupLayout.MarkupDrawContext = d;
            try
            {
                MarkupElement[] blocks = new MarkupElement[this.Elements.Count];
                this.Elements.CopyTo(blocks);

                Rectangle r = layout.Layout(new Rectangle(Point.Empty, bounds.Size), blocks, markupLayout);
                this.Bounds = new Rectangle(bounds.Location, r.Size);
            }
            finally
            {
                markupLayout.MarkupDrawContext = null;
            }
        }

        /// <summary>
        /// Returns whether markup element is an block element that always consumes a whole line in layout.
        /// </summary>
        public override bool IsBlockElement
        {
            get { return true; }
        }
        #endregion
        
    }
}
