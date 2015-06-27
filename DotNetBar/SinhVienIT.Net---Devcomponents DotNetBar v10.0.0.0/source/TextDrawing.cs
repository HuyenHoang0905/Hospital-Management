using System;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Text;

#if DOTNETBAR
namespace DevComponents.DotNetBar
#else
namespace DevComponents.Tree
#endif
{
    internal class TextDrawing
    {
        static TextDrawing()
        {
            string lc = Application.CurrentCulture.TwoLetterISOLanguageName;
            if (lc == "ja" || lc == "ko" || lc == "zh" || lc == "ar")
                UseGenericDefault = true;
        }

        public static bool TextDrawingEnabled = true;
        public static bool UseTextRenderer = false;

        public static void DrawString(Graphics g, string text, Font font, Color color, int x, int y, eTextFormat format)
        {
            DrawString(g, text, font, color, new Rectangle(x, y, 0, 0), format);
        }
        public static void DrawString(Graphics g, string text, Font font, Color color, Rectangle bounds, eTextFormat format)
        {
            #if FRAMEWORK20
            if (UseTextRenderer && (format & eTextFormat.Vertical) == 0)
                TextRenderer.DrawText(g, text, font, bounds, color, GetTextFormatFlags(format));
            else
                DrawStringLegacy(g, text, font, color, bounds, format);
            #else
            DrawStringLegacy(g, text, font, color, bounds, format);
            #endif
        }

        public static void DrawStringLegacy(Graphics g, string text, Font font, Color color, Rectangle bounds, eTextFormat format)
        {
            if (color.IsEmpty || !TextDrawingEnabled)
                return;

            using (SolidBrush brush = new SolidBrush(color))
            {
                using (StringFormat sf = GetStringFormat(format))
                    g.DrawString(text, font, brush, bounds, sf);
            }
        }


        public static Size MeasureString(Graphics g, string text, Font font)
        {
            return MeasureString(g, text, font, Size.Empty, eTextFormat.Default);
        }

        public static Size MeasureString(Graphics g, string text, Font font, int proposedWidth, eTextFormat format)
        {
            return MeasureString(g, text, font, new Size(proposedWidth, 0), format);
        }

        public static Size MeasureString(Graphics g, string text, Font font, int proposedWidth)
        {
            return MeasureString(g, text, font, new Size(proposedWidth, 0), eTextFormat.Default);
        }

        public static Size MeasureString(Graphics g, string text, Font font, Size proposedSize, eTextFormat format)
        {
            #if FRAMEWORK20
            if (UseTextRenderer && (format & eTextFormat.Vertical) == 0)
            {
                format = format & ~(format & eTextFormat.VerticalCenter); // Bug in .NET Framework 2.0
                format = format & ~(format & eTextFormat.Bottom); // Bug in .NET Framework 2.0
                format = format & ~(format & eTextFormat.HorizontalCenter); // Bug in .NET Framework 2.0
                format = format & ~(format & eTextFormat.Right); // Bug in .NET Framework 2.0
                format = format & ~(format & eTextFormat.EndEllipsis); // Bug in .NET Framework 2.0
                return Size.Ceiling(TextRenderer.MeasureText(g, text, font, proposedSize, GetTextFormatFlags(format)));
            }
            using (StringFormat sf = GetStringFormat(format))
                return Size.Ceiling(g.MeasureString(text, font, proposedSize, sf));
            #else
            using (StringFormat sf = GetStringFormat(format))
                return Size.Ceiling(g.MeasureString(text, font, proposedSize, sf));
            #endif
        }

        public static Size MeasureStringLegacy(Graphics g, string text, Font font, Size proposedSize, eTextFormat format)
        {
            using (StringFormat sf = GetStringFormat(format))
                return g.MeasureString(text, font, proposedSize, sf).ToSize();
        }


        public static eTextFormat TranslateHorizontal(StringAlignment align)
        {
            if (align == StringAlignment.Center)
                return eTextFormat.HorizontalCenter;
            else if (align == StringAlignment.Far)
                return eTextFormat.Right;
            return eTextFormat.Default;
        }

        public static eTextFormat TranslateVertical(StringAlignment align)
        {
            if (align == StringAlignment.Center)
                return eTextFormat.VerticalCenter;
            else if (align == StringAlignment.Far)
                return eTextFormat.Bottom;
            return eTextFormat.Default;
        }

#if FRAMEWORK20
        private static TextFormatFlags GetTextFormatFlags(eTextFormat format)
        {
            format |= eTextFormat.PreserveGraphicsTranslateTransform | eTextFormat.PreserveGraphicsClipping;
            if((format & eTextFormat.SingleLine)==eTextFormat.SingleLine && (format & eTextFormat.WordBreak)==eTextFormat.WordBreak)
                format = format & ~(format & eTextFormat.SingleLine);
            return (TextFormatFlags)format;
        }
#endif
        internal static bool UseGenericDefault = false;
        
        public static StringFormat GetStringFormat(eTextFormat format)
        {
            StringFormat sf;
            if (UseGenericDefault)
                sf = (StringFormat)StringFormat.GenericDefault.Clone(); //new StringFormat(StringFormat.GenericDefault);
            else
                sf = (StringFormat)StringFormat.GenericTypographic.Clone(); // new StringFormat(StringFormat.GenericTypographic);
            
            if (format == eTextFormat.Default)
                return sf;
            if ((format & eTextFormat.HorizontalCenter) == eTextFormat.HorizontalCenter)
                sf.Alignment = StringAlignment.Center;
            else if ((format & eTextFormat.Right) == eTextFormat.Right)
                sf.Alignment=StringAlignment.Far;
            if ((format & eTextFormat.VerticalCenter) == eTextFormat.VerticalCenter)
                sf.LineAlignment=StringAlignment.Center;
            else if ((format & eTextFormat.Bottom) == eTextFormat.Bottom)
                sf.LineAlignment=StringAlignment.Far;

            if ((format & eTextFormat.EndEllipsis) == eTextFormat.EndEllipsis)
                sf.Trimming = StringTrimming.EllipsisCharacter;
            else
                sf.Trimming = StringTrimming.Character;

            if ((format & eTextFormat.HidePrefix) == eTextFormat.HidePrefix)
                sf.HotkeyPrefix = HotkeyPrefix.Hide;
            else if ((format & eTextFormat.NoPrefix) == eTextFormat.NoPrefix)
                sf.HotkeyPrefix = HotkeyPrefix.None;
            else
                sf.HotkeyPrefix = HotkeyPrefix.Show;

            if ((format & eTextFormat.WordBreak) == eTextFormat.WordBreak)
                sf.FormatFlags = sf.FormatFlags & ~(sf.FormatFlags & StringFormatFlags.NoWrap);
            else
                sf.FormatFlags |= StringFormatFlags.NoWrap;
				
			if ((format & eTextFormat.LeftAndRightPadding) == eTextFormat.LeftAndRightPadding)
                sf.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;

            if ((format & eTextFormat.RightToLeft) == eTextFormat.RightToLeft)
                sf.FormatFlags |= StringFormatFlags.DirectionRightToLeft;

            if ((format & eTextFormat.Vertical) == eTextFormat.Vertical)
                sf.FormatFlags |= StringFormatFlags.DirectionVertical;

            if ((format & eTextFormat.NoClipping) == eTextFormat.NoClipping)
                sf.FormatFlags |= StringFormatFlags.NoClip;
        	
            return sf;
        }
#if DOTNETBAR
        public static ThemeTextFormat GetTextFormat(eTextFormat format)
        {
            ThemeTextFormat ttf = ThemeTextFormat.Left;
            if (format == eTextFormat.Default)
                return ttf;

            if ((format & eTextFormat.HorizontalCenter) == eTextFormat.HorizontalCenter)
                ttf |= ThemeTextFormat.Center;
            else if ((format & eTextFormat.Right) == eTextFormat.Right)
                ttf|= ThemeTextFormat.Right;
            if ((format & eTextFormat.VerticalCenter) == eTextFormat.VerticalCenter)
                ttf |= ThemeTextFormat.VCenter;
            else if ((format & eTextFormat.Bottom) == eTextFormat.Bottom)
                ttf |= ThemeTextFormat.Bottom;

            if ((format & eTextFormat.EndEllipsis) == eTextFormat.EndEllipsis)
                ttf |= ThemeTextFormat.EndEllipsis;

            if ((format & eTextFormat.HidePrefix) == eTextFormat.HidePrefix)
                ttf |= ThemeTextFormat.HidePrefix;
            else if ((format & eTextFormat.NoPrefix) == eTextFormat.NoPrefix)
                ttf |= ThemeTextFormat.NoPrefix;

            if ((format & eTextFormat.WordBreak) == eTextFormat.WordBreak)
                ttf |= ThemeTextFormat.WordBreak;
            else
                ttf |= ThemeTextFormat.SingleLine;

            if ((format & eTextFormat.RightToLeft) == eTextFormat.RightToLeft)
                ttf |= ThemeTextFormat.RtlReading;

            if ((format & eTextFormat.NoClipping) == eTextFormat.NoClipping)
                ttf |= ThemeTextFormat.NoClip;

            return ttf;
        }
#endif
    }

    [Flags]
    public enum eTextFormat
    {
        Bottom = 8,
        Default = 0,
        EndEllipsis = 0x8000,
        ExpandTabs = 0x40,
        ExternalLeading = 0x200,
        GlyphOverhangPadding = 0,
        HidePrefix = 0x100000,
        HorizontalCenter = 1,
        Internal = 0x1000,
        Left = 0,
        LeftAndRightPadding = 0x20000000,
        ModifyString = 0x10000,
        NoClipping = 0x100,
        NoFullWidthCharacterBreak = 0x80000,
        NoPadding = 0x10000000,
        NoPrefix = 0x800,
        PathEllipsis = 0x4000,
        PrefixOnly = 0x200000,
        PreserveGraphicsClipping = 0x1000000,
        PreserveGraphicsTranslateTransform = 0x2000000,
        Right = 2,
        RightToLeft = 0x20000,
        SingleLine = 0x20,
        TextBoxControl = 0x2000,
        Top = 0,
        VerticalCenter = 4,
        WordBreak = 0x10,
        WordEllipsis = 0x40000,
        Vertical = 0x40000000
    }
}
