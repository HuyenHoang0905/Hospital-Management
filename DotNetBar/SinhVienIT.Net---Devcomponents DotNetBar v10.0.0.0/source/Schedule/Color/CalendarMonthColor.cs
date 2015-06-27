#if FRAMEWORK20
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar.Schedule
{
    #region enum definitions

    /// <summary>
    /// Month calendar parts enum
    /// </summary>
    public enum eCalendarMonthPart
    {
        DayOfTheWeekHeaderBorder,
        DayOfTheWeekHeaderBackground,
        DayOfTheWeekHeaderForeground,

        SideBarBorder,
        SideBarBackground,
        SideBarForeground,

        DayHeaderBorder,
        DayHeaderBackground,
        DayHeaderForeground,

        DayContentBorder,
        DayContentSelectionBackground,
        DayContentActiveBackground,  
        DayContactInactiveBackground,

        OwnerTabBorder,
        OwnerTabBackground,
        OwnerTabForeground,
        OwnerTabContentBackground,
        OwnerTabSelectedForeground,
        OwnerTabSelectedBackground,

        NowDayHeaderBorder,
        NowDayHeaderForeground,
        NowDayHeaderBackground,
    }

    #endregion

    public class CalendarMonthColor : CalendarColor
    {
        #region Private variables

        private ColorDef[][] _LocalColorTable =
             { _Blue, _Green, _Maroon, _Steel, _Teal, _Purple, _Olive,
               _Red, _DarkPeach, _DarkSteel, _DarkGreen, _Yellow};

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="eColor">Default color</param>
        public CalendarMonthColor(eCalendarColor eColor)
            : base(eColor)
        {
        }

        #region SetColorTable routine

        /// <summary>
        /// Sets our current color table to either
        /// a local or global definition
        /// </summary>
        public override void SetColorTable()
        {
            if (ColorSch != eCalendarColor.Automatic)
            {
                // Use the requested local color table

                ColorTable = _LocalColorTable[(int)ColorSch];
            }
            else
            {
                // Use the globally set color table

                Office2007Renderer r =
                    GlobalManager.Renderer as Office2007Renderer;

                ColorTable = (r != null) 
                    ? r.ColorTable.CalendarView.MonthViewColors
                    : _LocalColorTable[(int)eCalendarColor.Blue];
            }
        }

        #endregion
 
        #region Static Color definitions

        // Array of predefined color

        #region Blue

        static ColorDef[] _Blue = new ColorDef[]
        {
            new ColorDef(0x8DAED9),     // DayOfWeekHeaderBorder

            new ColorDef(new int[] {0xE4ECF6, 0xD6E2F1, 0xC2D4EB, 0xD0DEEF},    // DayOfWeekHeaderBackground
                         new float[] {0f, .6f, .6f, 1f}, 90f),

            new ColorDef(0x000000),     // DayOfWeekHeaderForeground - 0x15428B
            new ColorDef(0x5D8CC9),     // SideBarBorder

            new ColorDef(new int[] {0xE4ECF6, 0xD6E2F1, 0xC2D4EB, 0xD0DEEF},     // SideBarBackground
                         new float[] {0f, .6f, .6f, 1f}, 0f),

            new ColorDef(0x000000),     // SideBarForeground - 0x15428B
            new ColorDef(0x5D8CC9),     // DayHeaderBorder

            new ColorDef(new int[] {0xE4ECF6, 0xD6E2F1, 0xC2D4EB, 0xD0DEEF},    // DayHeaderBackground
                         new float[] {0f, .6f, .6f, 1f}, 90f),

            new ColorDef(0x000000),     // DayHeaderForeground
            new ColorDef(0x8DAED9),     // DayContentBorder
            new ColorDef(0xE6EDF7),     // DayContentSelectionBackground
            new ColorDef(0xFFFFFF),     // DayContentActiveDayBackground
            new ColorDef(0xA5BFE1),     // DayContentInactiveDayBackground

            new ColorDef(0x5D8CC9),     // OwnerTabBorder

            new ColorDef(new int[] {0xBBCFE9, 0x8DAED9},    // OwnerTabBackground
                         new float[] {0f, 1f}, 90f),

            new ColorDef(0x000000),     // OwnerTabForeground
            new ColorDef(0x8DAED9),     // OwnerTabContentBackground
            new ColorDef(0x000000),     // OwnerTabSelectedForeground
            new ColorDef(0xFFFFFF),     // OwnerTabSelectionBackground

            new ColorDef(0xEB8900),     // NowDayViewBorder
            new ColorDef(0x000000),     // NowDayHeaderForeground - 0x15428B

            new ColorDef(new int[] {0xFFED79, 0xFFD86B, 0xFFBB00, 0xFFEA77},      // NowDayHeaderBackground
                         new float[] {0f, .55f, .058f, 1f}, 90f),
        };

        #endregion

        #region Green

        static ColorDef[] _Green = new ColorDef[]
        {
            new ColorDef(0x72A45A),     // DayOfWeekHeaderBorder

            new ColorDef(new int[] {0xE7F0E4, 0xDDE8D7, 0xCCDEC2, 0xD8E5D0},    // DayOfWeekHeaderBackground
                         new float[] {0f, .6f, .6f, 1f}, 90f),

            new ColorDef(0x50734D),     // DayOfWeekHeaderForeground
            new ColorDef(0x72A45A),     // SideBarBorder

            new ColorDef(new int[] {0xE7F0E4, 0xDEE9D8, 0xCBDDC2, 0xD7E5D0},     // SideBarBackground
                         new float[] {0f, .6f, .6f, 1f}, 0f),

            new ColorDef(0x000000),     // SideBarForeground
            new ColorDef(0x50733F),     // DayHeaderBorder

            new ColorDef(new int[] {0xE7F0E4, 0xDDE8D7, 0xCCDEC2, 0xD8E5D0},    // DayHeaderBackground
                         new float[] {0f, .55f, .58f, 1f}, 90f),

            new ColorDef(0x000000),     // DayHeaderForeground
            new ColorDef(0x72A45A),     // DayContentBorder
            new ColorDef(0xE9F1E6),     // DayContentSelectionBackground
            new ColorDef(0xFFFFFF),     // DayContentActiveDayBackground
            new ColorDef(0xB1CDA4),     // DayContentInactiveDayBackground

            new ColorDef(0x72A45A),     // OwnerTabBorder

            new ColorDef(new int[] {0xC3D9B9, 0x9CBF8B},    // OwnerTabBackground
                         new float[] {0f, 1f}, 90f),

            new ColorDef(0x000000),     // OwnerTabForeground
            new ColorDef(0x9CBF8B),     // OwnerTabContentBackground
            new ColorDef(0x000000),     // OwnerTabSelectedForeground
            new ColorDef(0xFFFFFF),     // OwnerTabSelectionBackground

            new ColorDef(0xEB8900),     // NowDayViewBorder
            new ColorDef(0x000000),     // NowDayHeaderForeground - 0x15428B

            new ColorDef(new int[] {0xFFED79, 0xFFD86B, 0xFFBB00, 0xFFEA77},      // NowDayHeaderBackground
                         new float[] {0f, .55f, .058f, 1f}, 90f),
        };

        #endregion

        #region Maroon

        static ColorDef[] _Maroon = new ColorDef[]
        {
            new ColorDef(0x85495E),     // DayOfWeekHeaderBorder

            new ColorDef(new int[] {0xF4E6EB, 0xEFDAE2, 0xE6C6D2, 0xEDD4DD},    // DayOfWeekHeaderBackground
                         new float[] {0f, .6f, .6f, 1f}, 90f),

            new ColorDef(0x85496B),     // DayOfWeekHeaderForeground
            new ColorDef(0xBE6886),     // SideBarBorder
 
            new ColorDef(new int[] {0xF4E6EB, 0xF0DBE3, 0xE7C7D3, 0xECD4DD},     // SideBarBackground
                         new float[] {0f, .6f, .6f, 1f}, 0f),

            new ColorDef(0x000000),     // SideBarForeground
            new ColorDef(0x85495E),     // DayHeaderBorder

            new ColorDef(new int[] {0xF4E6EB, 0xEFDAE2, 0xE6C6D2, 0xEDD4DD},    // DayHeaderBackground
                         new float[] {0f, .55f, .58f, 1f}, 90f),

            new ColorDef(0x000000),     // DayHeaderForeground
            new ColorDef(0xBE6886),     // DayContentBorder
            new ColorDef(0xF5E8EC),     // DayContentSelectionBackground
            new ColorDef(0xFFFFFF),     // DayContentActiveDayBackground
            new ColorDef(0xDBACBC),     // DayContentInactiveDayBackground

            new ColorDef(0xBE6886),     // OwnerTabBorder

            new ColorDef(new int[] {0xE4BFCB, 0xD195AA},    // OwnerTabBackground
                         new float[] {0f, 1f}, 90f),

            new ColorDef(0x000000),     // OwnerTabForeground
            new ColorDef(0xD195AA),     // OwnerTabContentBackground
            new ColorDef(0x000000),     // OwnerTabSelectedForeground
            new ColorDef(0xFFFFFF),     // OwnerTabSelectionBackground

            new ColorDef(0xEB8900),     // NowDayViewBorder
            new ColorDef(0x000000),     // NowDayHeaderForeground - 0x15428B

            new ColorDef(new int[] {0xFFED79, 0xFFD86B, 0xFFBB00, 0xFFEA77},      // NowDayHeaderBackground
                         new float[] {0f, .55f, .058f, 1f}, 90f),
        };

        #endregion

        #region Steel

        static ColorDef[] _Steel = new ColorDef[]
        {
            new ColorDef(0x9199A4),     // DayOfWeekHeaderBorder

            new ColorDef(new int[] {0xDCDFE2, 0xD3D6DA, 0xB4BAC1, 0xCBCED4},    // DayOfWeekHeaderBackground
                         new float[] {0f, .55f, .58f, 1f}, 90f),

            new ColorDef(0x616A76),     // DayOfWeekHeaderForeground
            new ColorDef(0x9199A4),     // SideBarBorder

            new ColorDef(new int[] {0xDCDFE2, 0xD2D5DA, 0xB7BCC3, 0xCACED4},     // SideBarBackground
                         new float[] {0f, .6f, .6f, 1f}, 0f),

            new ColorDef(0x000000),     // SideBarForeground
            new ColorDef(0x9199A4),     // DayHeaderBorder

            new ColorDef(new int[] {0xDCDFE2, 0xD3D6DA, 0xB4BAC1, 0xCBCED4},    // DayHeaderBackground
                         new float[] {0f, .55f, .58f, 1f}, 90f),

            new ColorDef(0x000000),     // DayHeaderForeground
            new ColorDef(0x9199A4),     // DayContentBorder
            new ColorDef(0xE8EAEC),     // DayContentSelectionBackground
            new ColorDef(0xFFFFFF),     // DayContentActiveDayBackground
            new ColorDef(0xC7CBD1),     // DayContentInactiveDayBackground

            new ColorDef(0x9199A4),     // OwnerTabBorder

            new ColorDef(new int[] {0xCFD2D8, 0xB0B6BE},    // OwnerTabBackground
                         new float[] {0f, 1f}, 90f),

            new ColorDef(0x000000),     // OwnerTabForeground
            new ColorDef(0xB0B6BE),     // OwnerTabContentBackground
            new ColorDef(0x000000),     // OwnerTabSelectedForeground
            new ColorDef(0xFFFFFF),     // OwnerTabSelectionBackground

            new ColorDef(0xEB8900),     // NowDayViewBorder
            new ColorDef(0x000000),     // NowDayHeaderForeground - 0x15428B

            new ColorDef(new int[] {0xFFED79, 0xFFD86B, 0xFFBB00, 0xFFEA77},      // NowDayHeaderBackground
                         new float[] {0f, .55f, .058f, 1f}, 90f),
        };

        #endregion

        #region Teal

        static ColorDef[] _Teal = new ColorDef[]
        {
           new ColorDef(0x3F7373),     // DayOfWeekHeaderBorder

            new ColorDef(new int[] {0xDCDFE2, 0xD7E8E8, 0xC0DDDD, 0xD0E5E5},    // DayOfWeekHeaderBackground
                         new float[] {0f, .55f, .58f, 1f}, 90f),

            new ColorDef(0x3F7380),     // DayOfWeekHeaderForeground
            new ColorDef(0x5AA4A4),     // SideBarBorder

            new ColorDef(new int[] {0xE4F0F0, 0xD6E8E8, 0xC2DDDD, 0xD0E5E5},     // SideBarBackground
                         new float[] {0f, .6f, .6f, 1f}, 0f),

            new ColorDef(0x000000),     // SideBarForeground
            new ColorDef(0x3F7373),     // DayHeaderBorder

            new ColorDef(new int[] {0xDCDFE2, 0xD7E8E8, 0xC0DDDD, 0xD0E5E5},    // DayHeaderBackground
                         new float[] {0f, .55f, .58f, 1f}, 90f),

            new ColorDef(0x000000),     // DayHeaderForeground
            new ColorDef(0x5AA4A4),     // DayContentBorder
            new ColorDef(0xE6F1F1),     // DayContentSelectionBackground
            new ColorDef(0xFFFFFF),     // DayContentActiveDayBackground
            new ColorDef(0xA4CDCD),     // DayContentInactiveDayBackground

            new ColorDef(0x5AA4A4),     // OwnerTabBorder

            new ColorDef(new int[] {0xB9D9D9, 0x8CBFC0},    // OwnerTabBackground
                         new float[] {0f, 1f}, 90f),

            new ColorDef(0x000000),     // OwnerTabForeground
            new ColorDef(0x8CBFC0),     // OwnerTabContentBackground
            new ColorDef(0x000000),     // OwnerTabSelectedForeground
            new ColorDef(0xFFFFFF),     // OwnerTabSelectionBackground

            new ColorDef(0xEB8900),     // NowDayViewBorder
            new ColorDef(0x000000),     // NowDayHeaderForeground - 0x15428B

            new ColorDef(new int[] {0xFFED79, 0xFFD86B, 0xFFBB00, 0xFFEA77},      // NowDayHeaderBackground
                         new float[] {0f, .55f, .058f, 1f}, 90f),
        };

        #endregion

        #region Purple

        static ColorDef[] _Purple = new ColorDef[]
        {
            new ColorDef(0x7171CD),     // DayOfWeekHeaderBorder

            new ColorDef(new int[] {0xE6E6F6, 0xDCDCF2, 0xC9C9EC, 0xD7D7F0},    // DayOfWeekHeaderBackground
                         new float[] {0f, .55f, .58f, 1f}, 90f),

            new ColorDef(0x4F4F90),     // DayOfWeekHeaderForeground
            new ColorDef(0x7171CD),     // SideBarBorder

            new ColorDef(new int[] {0xE6E6F6, 0xDCDCF2, 0xC9C9EC, 0xD7D7F0},     // SideBarBackground
                         new float[] {0f, .55f, .58f, 1f}, 0f),

            new ColorDef(0x000000),     // SideBarForeground
            new ColorDef(0x4F4F90),     // DayHeaderBorder

            new ColorDef(new int[] {0xE6E6F6, 0xDCDCF2, 0xC9C9EC, 0xD7D7F0},    // DayHeaderBackground
                         new float[] {0f, .55f, .58f, 1f}, 90f),

            new ColorDef(0x000000),     // DayHeaderForeground
            new ColorDef(0x7171CD),     // DayContentBorder
            new ColorDef(0xE9E9F7),     // DayContentSelectionBackground
            new ColorDef(0xFFFFFF),     // DayContentActiveDayBackground
            new ColorDef(0x9B9BDC),     // DayContentInactiveDayBackground

            new ColorDef(0x7171CD),     // OwnerTabBorder

            new ColorDef(new int[] {0xC1C1EA, 0x8C8CD7},    // OwnerTabBackground
                         new float[] {0f, 1f}, 90f),

            new ColorDef(0x000000),     // OwnerTabForeground
            new ColorDef(0x8C8CD7),     // OwnerTabContentBackground
            new ColorDef(0x000000),     // OwnerTabSelectedForeground
            new ColorDef(0xFFFFFF),     // OwnerTabSelectionBackground

            new ColorDef(0xEB8900),     // NowDayViewBorder
            new ColorDef(0x000000),     // NowDayHeaderForeground - 0x15428B

            new ColorDef(new int[] {0xFFED79, 0xFFD86B, 0xFFBB00, 0xFFEA77},      // NowDayHeaderBackground
                         new float[] {0f, .55f, .058f, 1f}, 90f),
        };

        #endregion

        #region Olive

        static ColorDef[] _Olive = new ColorDef[]
        {
            new ColorDef(0x9D9D57),     // DayOfWeekHeaderBorder

            new ColorDef(new int[] {0xEFEFE3, 0xE8E8D8, 0xDADABF, 0xE3E3CF},    // DayOfWeekHeaderBackground
                         new float[] {0f, .55f, .58f, 1f}, 90f),

            new ColorDef(0x6E6E3D),     // DayOfWeekHeaderForeground
            new ColorDef(0x9D9D57),     // SideBarBorder

            new ColorDef(new int[] {0xEFEFE3, 0xE8E8D8, 0xDADABF, 0xE3E3CF},     // SideBarBackground
                         new float[] {0f, .55f, .58f, 1f}, 0f),

            new ColorDef(0x000000),     // SideBarForeground
            new ColorDef(0x6E6E3D),     // DayHeaderBorder

            new ColorDef(new int[] {0xEFEFE3, 0xE8E8D8, 0xDADABF, 0xE3E3CF},    // DayHeaderBackground
                         new float[] {0f, .55f, .58f, 1f}, 90f),

            new ColorDef(0x000000),     // DayHeaderForeground
            new ColorDef(0x9D9D57),     // DayContentBorder
            new ColorDef(0xF0F0E5),     // DayContentSelectionBackground
            new ColorDef(0xFFFFFF),     // DayContentActiveDayBackground
            new ColorDef(0xC9C9A2),     // DayContentInactiveDayBackground

            new ColorDef(0x9D9D57),     // OwnerTabBorder

            new ColorDef(new int[] {0xD5D5B8, 0xBABA89},    // OwnerTabBackground
                         new float[] {0f, 1f}, 90f),

            new ColorDef(0x000000),     // OwnerTabForeground
            new ColorDef(0xBABA89),     // OwnerTabContentBackground
            new ColorDef(0x000000),     // OwnerTabSelectedForeground
            new ColorDef(0xFFFFFF),     // OwnerTabSelectionBackground

            new ColorDef(0xEB8900),     // NowDayViewBorder
            new ColorDef(0x000000),     // NowDayHeaderForeground - 0x15428B

            new ColorDef(new int[] {0xFFED79, 0xFFD86B, 0xFFBB00, 0xFFEA77},      // NowDayHeaderBackground
                         new float[] {0f, .55f, .058f, 1f}, 90f),
        };

        #endregion

        #region Red

        static ColorDef[] _Red = new ColorDef[]
        {
            new ColorDef(0xC16969),     // DayOfWeekHeaderBorder

            new ColorDef(new int[] {0xF5E6E6, 0xEFDADA, 0xE7C6C6, 0xEDD4D4},    // DayOfWeekHeaderBackground
                         new float[] {0f, .55f, .58f, 1f}, 90f),

            new ColorDef(0x874A4A),     // DayOfWeekHeaderForeground
            new ColorDef(0xC16969),     // SideBarBorder

            new ColorDef(new int[] {0xF5E6E6, 0xEFDADA, 0xE7C6C6, 0xEDD4D4},     // SideBarBackground
                         new float[] {0f, .55f, .58f, 1f}, 0f),

            new ColorDef(0x000000),     // SideBarForeground
            new ColorDef(0x874A4A),     // DayHeaderBorder

            new ColorDef(new int[] {0xF5E6E6, 0xEFDADA, 0xE7C6C6, 0xEDD4D4},    // DayHeaderBackground
                         new float[] {0f, .55f, .58f, 1f}, 90f),

            new ColorDef(0x000000),     // DayHeaderForeground
            new ColorDef(0xC16969),     // DayContentBorder
            new ColorDef(0xF6E8E8),     // DayContentSelectionBackground
            new ColorDef(0xFFFFFF),     // DayContentActiveDayBackground
            new ColorDef(0xDDACAC),     // DayContentInactiveDayBackground

            new ColorDef(0xC16969),     // OwnerTabBorder

            new ColorDef(new int[] {0xE5BFBF, 0xD39696},    // OwnerTabBackground
                         new float[] {0f, 1f}, 90f),

            new ColorDef(0x000000),     // OwnerTabForeground
            new ColorDef(0xD39696),     // OwnerTabContentBackground
            new ColorDef(0x000000),     // OwnerTabSelectedForeground
            new ColorDef(0xFFFFFF),     // OwnerTabSelectionBackground

            new ColorDef(0xEB8900),     // NowDayViewBorder
            new ColorDef(0x000000),     // NowDayHeaderForeground - 0x15428B

            new ColorDef(new int[] {0xFFED79, 0xFFD86B, 0xFFBB00, 0xFFEA77},      // NowDayHeaderBackground
                         new float[] {0f, .55f, .058f, 1f}, 90f),
        };

        #endregion

        #region DarkPeach

        static ColorDef[] _DarkPeach = new ColorDef[]
        {
            new ColorDef(0xA98F5D),     // DayOfWeekHeaderBorder

            new ColorDef(new int[] {0xF1EDE4, 0xE9E4D7, 0xDFD5C1, 0xE6E0D1},    // DayOfWeekHeaderBackground
                         new float[] {0f, .55f, .58f, 1f}, 90f),

            new ColorDef(0x776441),     // DayOfWeekHeaderForeground
            new ColorDef(0xA98F5D),     // SideBarBorder

            new ColorDef(new int[] {0xF1EDE4, 0xDFD5C1, 0xDFD5C1, 0xE6E0D1},     // SideBarBackground
                         new float[] {0f, .55f, .58f, 1f}, 0f),

            new ColorDef(0x000000),     // SideBarForeground
            new ColorDef(0x776441),     // DayHeaderBorder

            new ColorDef(new int[] {0xF1EDE4, 0xE9E4D7, 0xDFD5C1, 0xE6E0D1},    // DayHeaderBackground
                         new float[] {0f, .55f, .58f, 1f}, 90f),

            new ColorDef(0x000000),     // DayHeaderForeground
            new ColorDef(0xA98F5D),     // DayContentBorder
            new ColorDef(0xF2EEE6),     // DayContentSelectionBackground
            new ColorDef(0xFFFFFF),     // DayContentActiveDayBackground
            new ColorDef(0xCFC1A5),     // DayContentInactiveDayBackground

            new ColorDef(0xA98F5D),     // OwnerTabBorder

            new ColorDef(new int[] {0xDBCFBA, 0xC3B08D},    // OwnerTabBackground
                         new float[] {0f, 1f}, 90f),

            new ColorDef(0x000000),     // OwnerTabForeground
            new ColorDef(0xC3B08D),     // OwnerTabContentBackground
            new ColorDef(0x000000),     // OwnerTabSelectedForeground
            new ColorDef(0xFFFFFF),     // OwnerTabSelectionBackground

            new ColorDef(0xEB8900),     // NowDayViewBorder
            new ColorDef(0x000000),     // NowDayHeaderForeground - 0x15428B

            new ColorDef(new int[] {0xFFED79, 0xFFD86B, 0xFFBB00, 0xFFEA77},      // NowDayHeaderBackground
                         new float[] {0f, .55f, .058f, 1f}, 90f),
        };

        #endregion

        #region DarkSteel

        static ColorDef[] _DarkSteel = new ColorDef[]
        {
            new ColorDef(0x6197B1),     // DayOfWeekHeaderBorder

            new ColorDef(new int[] {0xE5EEF2, 0xD8E5EB, 0xC3D7E1, 0xD2E2E9},    // DayOfWeekHeaderBackground
                         new float[] {0f, .55f, .58f, 1f}, 90f),

            new ColorDef(0x446A96),     // DayOfWeekHeaderForeground
            new ColorDef(0x6197B1),     // SideBarBorder

            new ColorDef(new int[] {0xE5EEF2, 0xD8E5EB, 0xC3D7E1, 0xD2E2E9},     // SideBarBackground
                         new float[] {0f, .55f, .58f, 1f}, 0f),

            new ColorDef(0x000000),     // SideBarForeground
            new ColorDef(0x446A7C),     // DayHeaderBorder

            new ColorDef(new int[] {0xE5EEF2, 0xD8E5EB, 0xC3D7E1, 0xD2E2E9},    // DayHeaderBackground
                         new float[] {0f, .55f, .58f, 1f}, 90f),

            new ColorDef(0x000000),     // DayHeaderForeground
            new ColorDef(0x6197B1),     // DayContentBorder
            new ColorDef(0xE7EFF3),     // DayContentSelectionBackground
            new ColorDef(0xFFFFFF),     // DayContentActiveDayBackground
            new ColorDef(0xA8C5D4),     // DayContentInactiveDayBackground

            new ColorDef(0x6197B1),     // OwnerTabBorder

            new ColorDef(new int[] {0xBCD2DE, 0x90B6C8},    // OwnerTabBackground
                         new float[] {0f, 1f}, 90f),

            new ColorDef(0x000000),     // OwnerTabForeground
            new ColorDef(0x90B6C8),     // OwnerTabContentBackground
            new ColorDef(0x000000),     // OwnerTabSelectedForeground
            new ColorDef(0xFFFFFF),     // OwnerTabSelectionBackground

            new ColorDef(0xEB8900),     // NowDayViewBorder
            new ColorDef(0x000000),     // NowDayHeaderForeground - 0x15428B

            new ColorDef(new int[] {0xFFED79, 0xFFD86B, 0xFFBB00, 0xFFEA77},      // NowDayHeaderBackground
                         new float[] {0f, .55f, .058f, 1f}, 90f),
        };

        #endregion

        #region DarkGreen

        static ColorDef[] _DarkGreen = new ColorDef[]
        {
           new ColorDef(0x5AA48C),     // DayOfWeekHeaderBorder

            new ColorDef(new int[] {0xE4F0EC, 0xD7E8E3, 0xC0DDD4, 0xD0E5DF},    // DayOfWeekHeaderBackground
                         new float[] {0f, .55f, .58f, 1f}, 90f),

            new ColorDef(0x3F7362),     // DayOfWeekHeaderForeground
            new ColorDef(0x5AA48C),     // SideBarBorder

            new ColorDef(new int[] {0xE4F0EC, 0xD7E8E3, 0xC0DDD4, 0xD0E5DF},     // SideBarBackground
                         new float[] {0f, .55f, .58f, 1f}, 0f),

            new ColorDef(0x000000),     // SideBarForeground
            new ColorDef(0x3F7362),     // DayHeaderBorder

            new ColorDef(new int[] {0xE4F0EC, 0xD7E8E3, 0xC0DDD4, 0xD0E5DF},    // DayHeaderBackground
                         new float[] {0f, .55f, .58f, 1f}, 90f),

            new ColorDef(0x000000),     // DayHeaderForeground
            new ColorDef(0x5AA48C),     // DayContentBorder
            new ColorDef(0xE6F1ED),     // DayContentSelectionBackground
            new ColorDef(0xFFFFFF),     // DayContentActiveDayBackground
            new ColorDef(0xA4CDBF),     // DayContentInactiveDayBackground

            new ColorDef(0x5AA48C),     // OwnerTabBorder

            new ColorDef(new int[] {0xB9D9CE, 0x8BBFAE},    // OwnerTabBackground
                         new float[] {0f, 1f}, 90f),

            new ColorDef(0x000000),     // OwnerTabForeground
            new ColorDef(0x8BBFAE),     // OwnerTabContentBackground
            new ColorDef(0x000000),     // OwnerTabSelectedForeground
            new ColorDef(0xFFFFFF),     // OwnerTabSelectionBackground

            new ColorDef(0xEB8900),     // NowDayViewBorder
            new ColorDef(0x000000),     // NowDayHeaderForeground - 0x15428B

            new ColorDef(new int[] {0xFFED79, 0xFFD86B, 0xFFBB00, 0xFFEA77},      // NowDayHeaderBackground
                         new float[] {0f, .55f, .058f, 1f}, 90f),
        };

        #endregion

        #region Yellow

        static ColorDef[] _Yellow = new ColorDef[]
        {
            new ColorDef(0xFFD151),     // DayOfWeekHeaderBorder

            new ColorDef(new int[] {0xFFF8E2, 0xFFF5D7, 0xFFEDBD, 0xFFF2CE},    // DayOfWeekHeaderBackground
                         new float[] {0f, .55f, .58f, 1f}, 90f),

            new ColorDef(0x7F6628),     // DayOfWeekHeaderForeground
            new ColorDef(0xFFD151),     // SideBarBorder

            new ColorDef(new int[] {0xFFF8E2, 0xFFF5D7, 0xFFEDBD, 0xFFF2CE},     // SideBarBackground
                         new float[] {0f, .55f, .58f, 1f}, 0f),

            new ColorDef(0x7F6628),     // SideBarForeground
            new ColorDef(0xD3AC40),     // DayHeaderBorder

            new ColorDef(new int[] {0xFFF8E2, 0xFFF5D7, 0xFFEDBD, 0xFFF2CE},    // DayHeaderBackground
                         new float[] {0f, .55f, .58f, 1f}, 90f),

            new ColorDef(0x000000),     // DayHeaderForeground
            new ColorDef(0xFFD151),     // DayContentBorder
            new ColorDef(0xFFF8E4),     // DayContentSelectionBackground
            new ColorDef(0xFFFFFF),     // DayContentActiveDayBackground
            new ColorDef(0xFFE69F),     // DayContentInactiveDayBackground

            new ColorDef(0xFFD151),     // OwnerTabBorder

            new ColorDef(new int[] {0xFFEBB6, 0xFFDF86},    // OwnerTabBackground
                         new float[] {0f, 1f}, 90f),

            new ColorDef(0x000000),     // OwnerTabForeground
            new ColorDef(0xFFDF86),     // OwnerTabContentBackground
            new ColorDef(0x000000),     // OwnerTabSelectedForeground
            new ColorDef(0xFFFFFF),     // OwnerTabSelectionBackground

            new ColorDef(0xEB8900),     // NowDayViewBorder
            new ColorDef(0x000000),     // NowDayHeaderForeground - 0x15428B

            new ColorDef(new int[] {0xFFED79, 0xFFD86B, 0xFFBB00, 0xFFEA77},      // NowDayHeaderBackground
                         new float[] {0f, .55f, .058f, 1f}, 90f),
        };

        #endregion

        #endregion
    }
}
#endif

