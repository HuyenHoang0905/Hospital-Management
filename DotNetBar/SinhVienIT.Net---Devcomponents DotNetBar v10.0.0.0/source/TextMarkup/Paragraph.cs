using System;
using System.Text;
using System.Drawing;
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
    internal class Paragraph : Div
    {
        #region Internal Implementation
        protected override void ArrangeInternal(Rectangle bounds, MarkupDrawContext d)
        {
            base.ArrangeInternal(bounds, d);
            this.Bounds = new Rectangle(this.Bounds.X, this.Bounds.Y, this.Bounds.Width , this.Bounds.Height + d.CurrentFont.Height);
        }
        #endregion
    }

    
}
