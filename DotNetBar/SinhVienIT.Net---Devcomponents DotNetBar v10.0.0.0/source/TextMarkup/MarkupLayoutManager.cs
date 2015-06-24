using System.Collections;
using System.Drawing;
using System.Text;

#if AdvTree
namespace DevComponents.Tree.TextMarkup
#elif DOTNETBAR
using DevComponents.UI.ContentManager;
namespace DevComponents.DotNetBar.TextMarkup
#elif SUPERGRID
namespace DevComponents.SuperGrid.TextMarkup
#endif
{
    internal class MarkupLayoutManager : BlockLayoutManager
    {
        private MarkupDrawContext m_MarkupDrawContext = null;

        public MarkupDrawContext MarkupDrawContext
        {
            get { return m_MarkupDrawContext; }
            set { m_MarkupDrawContext = value; }
        }

        public override void Layout(IBlock block, Size availableSize)
        {
            if (block is MarkupElement)
            {
                MarkupElement m = block as MarkupElement;
                if(!m.IsSizeValid)
                    m.Measure(availableSize, m_MarkupDrawContext);
            }
        }

        public override Rectangle FinalizeLayout(Rectangle containerBounds, Rectangle blocksBounds, ArrayList lines)
        {
            return (blocksBounds);
        }
    }
}
