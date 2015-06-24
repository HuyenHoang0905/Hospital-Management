#if FRAMEWORK20
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar.Schedule
{
    #region enum definitions

    /// <summary>
    /// Appointment parts enum
    /// </summary>
    public enum eAppointmentPart
    {
        DefaultBorder,
        DefaultBackground,

        BlueBorder,
        BlueBackground,

        GreenBorder,
        GreenBackground,

        OrangeBorder,
        OrangeBackground,

        PurpleBorder,
        PurpleBackground,

        RedBorder,
        RedBackground,

        YellowBorder,
        YellowBackground,

        BusyTimeMarker,
        FreeTimeMarker,
        OutOfOfficeTimeMarker
    }

    #endregion

    public class AppointmentColor : CalendarColor
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AppointmentColor()
            : base(eCalendarColor.Automatic)
        {
        }

        #region SetColorTable routine

        /// <summary>
        /// Sets our current color table to either
        /// a local or global definition
        /// </summary>
        public override void SetColorTable()
        {
            // Use the globally set color table

            Office2007Renderer r =
                GlobalManager.Renderer as Office2007Renderer;

            ColorTable = (r != null)
                ? r.ColorTable.CalendarView.AppointmentColors
                : _LocalColorTable;
        }

        #endregion

        #region Static Color definitions

        // Array of predefined colors

        static ColorDef[] _LocalColorTable = new ColorDef[]
        {
            new ColorDef(0x4B71A2),                         // DefaultBorder

            new ColorDef(new int[] {0xFDFEFF, 0xC1D3EA},    // DefaultBackground
                         new float[] {0f, 1f}, 90f),

            new ColorDef(0x28518E),                         // BlueBorder

            new ColorDef(new int[] {0xB1C5EC, 0x759DDA},    // BlueBackground
                         new float[] {0f, 1f}, 90f),

            new ColorDef(0x2C6524),                         // GreenBorder

            new ColorDef(new int[] {0xC2E8BC, 0x84D17B},    // GreenBackground
                         new float[] {0f, 1f}, 90f),

            new ColorDef(0x8B3E0A),                         // OrangeBorder

            new ColorDef(new int[] {0xF9C7A0, 0xF49758},    // OrangeBackground
                         new float[] {0f, 1f}, 90f),

            new ColorDef(0x3E2771),                         // PurpleBorder

            new ColorDef(new int[] {0xC5B5E6, 0x957BD2},    // PurpleBackground
                         new float[] {0f, 1f}, 90f),

            new ColorDef(0x86171C),                         // RedBorder

            new ColorDef(new int[] {0xF1AAAC, 0xE5676E},    // RedBackground
                         new float[] {0f, 1f}, 90f),

            new ColorDef(0x7C7814),                         // YellowBorder

            new ColorDef(new int[] {0xFFFCAA, 0xFFF958},    // YellowBackground
                         new float[] {0f, 1f}, 90f),

            new ColorDef(-1),                               // BusyTimeMarker
            new ColorDef(0xFFFFFF),                         // FreeTimeMarker
            new ColorDef(0x800080),                         // OutOfOfficeTimeMarker
        };

        #endregion
    }
}
#endif

