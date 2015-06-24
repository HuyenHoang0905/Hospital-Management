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
#if FRAMEWORK20
    public static class MarkupSettings
#else
    public class MarkupSettings
#endif
    {
        private static HyperlinkStyle _NormalHyperlink = new HyperlinkStyle(Color.Blue, eHyperlinkUnderlineStyle.SolidLine);
        /// <summary>
        /// Gets the style of the hyperlink in its default state.
        /// </summary>
        public static HyperlinkStyle NormalHyperlink
        {
            get { return _NormalHyperlink; }
        }

        private static HyperlinkStyle _MouseOverHyperlink = new HyperlinkStyle();
        /// <summary>
        /// Gets the style of the hyperlink when mouse is over the link.
        /// </summary>
        public static HyperlinkStyle MouseOverHyperlink
        {
            get { return _MouseOverHyperlink; }
        }

        private static HyperlinkStyle _VisitedHyperlink = new HyperlinkStyle();
        /// <summary>
        /// Gets the style of the visited hyperlink.
        /// </summary>
        public static HyperlinkStyle VisitedHyperlink
        {
            get { return _VisitedHyperlink; }
        }

        /// <summary>
        /// Represents the method that will handle the ResolveImage event.
        /// </summary>
        public delegate void ResolveImageEventHandler(object sender, ResolveImageEventArgs e);
        // <summary>
        /// Occurs when DotNetBar is looking for an image for one of the internal img tags that is
        /// used within the minimarkup. You need to set Handled=true if you want your custom image
        /// to be used instead of the built-in resource resolving mechanism.
        /// </summary>
        public static event ResolveImageEventHandler ResolveImage;
        internal static void InvokeResolveImage(ResolveImageEventArgs e)
        {
            if (ResolveImage != null)
                ResolveImage(null, e);
        }
    }

    /// <summary>
    /// Defines the text-markup hyperlink appearance style.
    /// </summary>
    public class HyperlinkStyle
    {
        /// <summary>
        /// Initializes a new instance of the HyperlinkStyle class.
        /// </summary>
        public HyperlinkStyle()
        {
        }

        /// <summary>
        /// Initializes a new instance of the HyperlinkStyle class.
        /// </summary>
        /// <param name="textColor"></param>
        /// <param name="underlineStyle"></param>
        public HyperlinkStyle(Color textColor, eHyperlinkUnderlineStyle underlineStyle)
        {
            _TextColor = textColor;
            _UnderlineStyle = underlineStyle;
        }

        /// <summary>
        /// Initializes a new instance of the HyperlinkStyle class.
        /// </summary>
        /// <param name="textColor"></param>
        /// <param name="backColor"></param>
        /// <param name="underlineStyle"></param>
        public HyperlinkStyle(Color textColor, Color backColor, eHyperlinkUnderlineStyle underlineStyle)
        {
            _TextColor = textColor;
            _BackColor = backColor;
            _UnderlineStyle = underlineStyle;
        }
        private Color _TextColor = Color.Empty;
        /// <summary>
        /// Gets or sets hyperlink text color.
        /// </summary>
        public Color TextColor
        {
            get { return _TextColor; }
            set
            {
                if (_TextColor != value)
                {
                    _TextColor = value;
                }
            }
        }

        private Color _BackColor = Color.Empty;
        /// <summary>
        /// Gets or sets hyperlink back color.
        /// </summary>
        public Color BackColor
        {
            get { return _BackColor; }
            set
            {
                if (_BackColor != value)
                {
                    _BackColor = value;
                }
            }
        }

        private eHyperlinkUnderlineStyle _UnderlineStyle = eHyperlinkUnderlineStyle.None;
        /// <summary>
        /// Gets or sets the underline style for the hyperlink.
        /// </summary>
        public eHyperlinkUnderlineStyle UnderlineStyle
        {
            get { return _UnderlineStyle; }
            set
            {
                if (_UnderlineStyle != value)
                {
                    _UnderlineStyle = value;
                }
            }
        }

        /// <summary>
        /// Gets whether style has been changed from its default state.
        /// </summary>
        public bool IsChanged
        {
            get { return !_TextColor.IsEmpty || !_BackColor.IsEmpty || _UnderlineStyle != eHyperlinkUnderlineStyle.None; }
        }
    }

    public enum eHyperlinkUnderlineStyle
    {
        None,
        SolidLine,
        DashedLine
    }

    /// <summary>
    /// <summary>
    /// Event arguments for ResolveImage event.
    /// </summary>
    public class ResolveImageEventArgs : EventArgs
    {
        /// <summary>
        /// Indicates that event has been handled and that ResolvedImage should be used.
        /// </summary>
        public bool Handled = false;
        /// <summary>
        /// Indicates the string key parameters in url-style for the image that needs to be resolved.
        /// </summary>
        public string Key = "";
        /// <summary>
        /// Indicates the resolved image value.
        /// you need to set this value to the resolved image and you need to set Handled property to true.
        /// </summary>
       public Image ResolvedImage = null;
        /// <summary>
        /// Default constructor.
        /// </summary>
        public ResolveImageEventArgs() { }
    }

}
