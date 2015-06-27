using System;
using System.Text;

#if TREEGX
using DevComponents.Tree;
namespace DevComponents.Tree.TextMarkup
#elif DOTNETBAR
namespace DevComponents.DotNetBar.TextMarkup
#elif SUPERGRID
namespace DevComponents.SuperGrid.TextMarkup
#endif
{
    internal class Span : Div
    {
        /// <summary>
        /// Returns whether markup element is an block element that always consumes a whole line in layout.
        /// </summary>
        public override bool IsBlockElement
        {
            get { return false; }
        }
    }
}
