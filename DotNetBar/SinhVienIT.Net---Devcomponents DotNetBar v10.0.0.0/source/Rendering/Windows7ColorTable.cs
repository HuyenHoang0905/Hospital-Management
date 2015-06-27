using System;
using System.Text;
using System.Drawing;

namespace DevComponents.DotNetBar.Rendering
{
    /// <summary>
    /// Represents color table for Windows 7 style.
    /// </summary>
    public class Windows7ColorTable : Office2007ColorTable
    {
        /// <summary>
        /// Initializes a new instance of the Office2010ColorTable class.
        /// </summary>
        public Windows7ColorTable()
        {
            Windows7BlueFactory.InitializeBlueColorTable(this, ColorFactory.Empty);
        }

        public Windows7ColorTable(eWindows7ColorScheme color)
        {
            Windows7BlueFactory.InitializeBlueColorTable(this, ColorFactory.Empty);
        }

        public Windows7ColorTable(eWindows7ColorScheme color, Color blendColor)
        {
            Windows7BlueFactory.InitializeBlueColorTable(this, blendColor.IsEmpty ? ColorFactory.Empty : new ColorBlendFactory(blendColor));
        }
    }

        /// <summary>
    /// Defines the color scheme type for the Office2010ColorTable.
    /// </summary>
    public enum eWindows7ColorScheme
    {
        /// <summary>
        /// Blue color scheme.
        /// </summary>
        Blue
    }
}
