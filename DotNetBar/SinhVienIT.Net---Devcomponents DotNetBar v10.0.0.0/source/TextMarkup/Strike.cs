using System.Drawing;

#if AdvTree
namespace DevComponents.Tree.TextMarkup
#elif DOTNETBAR
namespace DevComponents.DotNetBar.TextMarkup
#elif SUPERGRID
namespace DevComponents.SuperGrid.TextMarkup
#endif
{
    internal class Strike : MarkupElement
    {
        public override void Measure(Size availableSize, MarkupDrawContext d)
        {
            Bounds = Rectangle.Empty;
        }

        public override void Render(MarkupDrawContext d)
        {
            d.StrikeOut = true;
        }

        public override void RenderEnd(MarkupDrawContext d)
        {
            d.StrikeOut = false;

            base.RenderEnd(d);
        }

        protected override void ArrangeCore(Rectangle finalRect, MarkupDrawContext d)
        {
        }
    }
}
