using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace DevComponents.DotNetBar.Rendering
{
    /// <summary>
    /// Defines color table for the SwitchButton control.
    /// </summary>
    public class SwitchButtonColorTable
    {
        /// <summary>
        /// Gets or set the text/caption color.
        /// </summary>
        public Color TextColor = ColorScheme.GetColor(0x1E395B);
        /// <summary>
        /// Gets or sets the OFF text color.
        /// </summary>
        public Color OffTextColor = ColorScheme.GetColor(0x66788E);
        /// <summary>
        /// Gets or sets the ON text color.
        /// </summary>
        public Color OnTextColor = ColorScheme.GetColor(0x66788E);
        /// <summary>
        /// Gets or sets the button border color.
        /// </summary>
        public Color BorderColor = ColorScheme.GetColor(0xB1C0D6);
        /// <summary>
        /// Gets or sets background color of OFF switch part.
        /// </summary>
        public Color OffBackColor = ColorScheme.GetColor(0xEFF6FD);
        /// <summary>
        /// Gets or sets background color of ON switch part.
        /// </summary>
        public Color OnBackColor = ColorScheme.GetColor(0x92D050);
        /// <summary>
        /// Gets or sets the switch border color.
        /// </summary>
        public Color SwitchBorderColor = ColorScheme.GetColor(0x95A8C4);
        /// <summary>
        /// Gets or sets the switch background color.
        /// </summary>
        public Color SwitchBackColor = ColorScheme.GetColor(0xDBE7F4);

        /// <summary>
        /// Gets default disabled color scheme for the switch button.
        /// </summary>
        public static SwitchButtonColorTable Disabled
        {
            get
            {
                SwitchButtonColorTable disabled = new SwitchButtonColorTable();
                disabled.BorderColor = ColorScheme.GetColor(0xD0D0D0);
                disabled.OffBackColor = ColorScheme.GetColor(0xE6E6E6);
                disabled.OffTextColor = ColorScheme.GetColor(0xD0D0D0);
                disabled.OnBackColor = ColorScheme.GetColor(0xE6E6E6);
                disabled.OnTextColor = ColorScheme.GetColor(0xD0D0D0);
                disabled.SwitchBackColor = ColorScheme.GetColor(0xE6E6E6);
                disabled.SwitchBorderColor = ColorScheme.GetColor(0xD0D0D0);
                disabled.TextColor = ColorScheme.GetColor(0x87929F);

                return disabled;
            }
        }
    }

    public class SwitchButtonColors
    {
        /// <summary>
        /// Gets or sets default Switch Button color table.
        /// </summary>
        public SwitchButtonColorTable Default = new SwitchButtonColorTable();
        /// <summary>
        /// Gets or sets the disabled Switch Button color table.
        /// </summary>
        public SwitchButtonColorTable Disabled = SwitchButtonColorTable.Disabled;
    }
}
