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
    internal class NewLine : MarkupElement
    {
        #region Internal Implementation
        public override void Measure(System.Drawing.Size availableSize, MarkupDrawContext d)
        {
            // Causes layout manager to switch to the new line
            this.Bounds = new Rectangle(0, 0, 0, d.CurrentFont.Height);
        }

        /// <summary>
        /// Gets or sets whether element size is valid. When size is not valid element Measure method will be called to validate size.
        /// </summary>
        public override bool IsSizeValid
        {
            get { return false; }
            set {  }
        }

        public override void Render(MarkupDrawContext d) {}

        protected override void ArrangeCore(System.Drawing.Rectangle finalRect, MarkupDrawContext d) { }

        /// <summary>
        /// Returns whether layout manager switches to new line after processing this element.
        /// </summary>
        public override bool IsNewLineAfterElement
        {
            get { return true; }
        }

        #endregion
    }
}
