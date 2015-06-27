using System;
using System.Drawing;
using System.Text;

#if AdvTree
namespace DevComponents.Tree.TextMarkup
#elif DOTNETBAR
namespace DevComponents.DotNetBar.TextMarkup
#elif SUPERGRID
namespace DevComponents.SuperGrid.TextMarkup
#endif
{
    internal class MarkupDrawContext
    {
        public Graphics Graphics = null;
        public Font CurrentFont = null;
        public Color CurrentForeColor = SystemColors.ControlText;
        public bool RightToLeft = false;
        public Point Offset = Point.Empty;
        public bool HyperLink = false;
        public HyperlinkStyle HyperlinkStyle = null;
        public bool Underline = false;
        public Rectangle ClipRectangle = Rectangle.Empty;
        public bool HotKeyPrefixVisible = false;
        public object ContextObject = null;
        public bool AllowMultiLine = true;
        public bool IgnoreFormattingColors = false;
        public bool StrikeOut;

        public MarkupDrawContext(Graphics g, Font currentFont, Color currentForeColor, bool rightToLeft) : this(g, currentFont, currentForeColor, rightToLeft, Rectangle.Empty, false)
        {
        }

        public MarkupDrawContext(Graphics g, Font currentFont, Color currentForeColor, bool rightToLeft, Rectangle clipRectangle, bool hotKeyPrefixVisible)
        {
            this.Graphics = g;
            this.CurrentFont = currentFont;
            this.CurrentForeColor  = currentForeColor;
            this.RightToLeft = rightToLeft;
            this.ClipRectangle = clipRectangle;
            this.HotKeyPrefixVisible = hotKeyPrefixVisible;
        }

        public MarkupDrawContext(Graphics g, Font currentFont, Color currentForeColor, bool rightToLeft, Rectangle clipRectangle, bool hotKeyPrefixVisible, object contextObject)
        {
            this.Graphics = g;
            this.CurrentFont = currentFont;
            this.CurrentForeColor = currentForeColor;
            this.RightToLeft = rightToLeft;
            this.ClipRectangle = clipRectangle;
            this.HotKeyPrefixVisible = hotKeyPrefixVisible;
            this.ContextObject = contextObject;
        }

    }
}
