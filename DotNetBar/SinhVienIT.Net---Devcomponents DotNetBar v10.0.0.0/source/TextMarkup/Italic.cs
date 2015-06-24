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
    internal class Italic : FontChangeElement
    {
        #region Internal Implementation
        protected override void SetFont(MarkupDrawContext d)
        {
            Font font = d.CurrentFont;
            FontStyle style=font.Style | FontStyle.Italic;

            if (!font.Italic && d.CurrentFont.FontFamily.IsStyleAvailable(style))
                d.CurrentFont = new Font(font, style);
            else
                font = null;

            if (font != null)
                m_OldFont = font;

            base.SetFont(d);
        }
        #endregion
    }
}
