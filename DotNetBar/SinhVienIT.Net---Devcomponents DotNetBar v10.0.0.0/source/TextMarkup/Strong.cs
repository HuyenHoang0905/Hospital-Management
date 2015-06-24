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
    internal class Strong : FontChangeElement
    {
        #region Internal Implementation
        protected override void SetFont(MarkupDrawContext d)
        {
            Font font = d.CurrentFont;
            FontStyle style = d.CurrentFont.Style | FontStyle.Bold;

            if (!font.Bold && font.FontFamily.IsStyleAvailable(style))
                d.CurrentFont = new Font(d.CurrentFont, style);
            else
                font = null;

            if (font != null)
                m_OldFont = font;

            base.SetFont(d);
        }
        #endregion
    }
}
