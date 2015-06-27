using System;
using System.Drawing;
using System.Text;

namespace DevComponents.DotNetBar.Rendering
{
    /// <summary>
    /// Defines color table for the Dialog Launcher button.
    /// </summary>
    public class Office2007DialogLauncherColorTable
    {
        /// <summary>
        /// Gets or sets the colors for the default state.
        /// </summary>
        public Office2007DialogLauncherStateColorTable Default = new Office2007DialogLauncherStateColorTable();

        /// <summary>
        /// Gets or sets the colors for the mouse over state.
        /// </summary>
        public Office2007DialogLauncherStateColorTable MouseOver = new Office2007DialogLauncherStateColorTable();

        /// <summary>
        /// Gets or sets the colors for the pressed state.
        /// </summary>
        public Office2007DialogLauncherStateColorTable Pressed = new Office2007DialogLauncherStateColorTable();
    }
}
