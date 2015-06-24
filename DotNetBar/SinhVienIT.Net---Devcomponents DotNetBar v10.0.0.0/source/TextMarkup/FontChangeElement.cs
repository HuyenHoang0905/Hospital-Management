using System;
using System.Text;
using System.Drawing;

#if AdvTree
namespace DevComponents.Tree.TextMarkup
#elif DOTNETBAR
namespace DevComponents.DotNetBar.TextMarkup
#elif SUPERGRID
namespace DevComponents.SuperGrid.TextMarkup
#endif
{
    internal class FontChangeElement : MarkupElement
    {
        #region Private Variables
        protected Font m_OldFont = null;
        #endregion

        #region Internal Implementation

        public override void Measure(Size availableSize, MarkupDrawContext d)
        {
            this.Bounds = Rectangle.Empty;
            SetFont(d);
        }

        public override void Render(MarkupDrawContext d)
        {
            SetFont(d);
        }

        protected virtual void SetFont(MarkupDrawContext d)
        {
            
        }

        public override void RenderEnd(MarkupDrawContext d)
        {
            if(m_OldFont!=null)
                d.CurrentFont = m_OldFont;
            m_OldFont = null;
            base.RenderEnd(d);
        }

        public override void MeasureEnd(Size availableSize, MarkupDrawContext d)
        {
            if (m_OldFont != null)
                d.CurrentFont = m_OldFont;
            m_OldFont = null;
            base.MeasureEnd(availableSize, d);
        }
        protected override void ArrangeCore(Rectangle finalRect, MarkupDrawContext d) { }
        #endregion
    }
}
