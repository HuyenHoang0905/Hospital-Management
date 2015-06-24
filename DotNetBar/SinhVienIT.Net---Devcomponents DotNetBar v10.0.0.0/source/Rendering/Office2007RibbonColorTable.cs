using System;
using System.Text;
using System.Drawing;

namespace DevComponents.DotNetBar.Rendering
{
    /// <summary>
    /// Defines color table for Ribbon Control.
    /// </summary>
    public class Office2007RibbonColorTable
    {
        /// <summary>
        /// Gets or sets the colors for the outer border.
        /// </summary>
        public LinearGradientColorTable OuterBorder = new LinearGradientColorTable(ColorScheme.GetColor("8DB2E3"), ColorScheme.GetColor("88A1C2"));
        /// <summary>
        /// Gets or sets the colors for the inner border.
        /// </summary>
        public LinearGradientColorTable InnerBorder = new LinearGradientColorTable(ColorScheme.GetColor("E7EFF8"), ColorScheme.GetColor("C0F9FF"));

        /// <summary>
        /// Gets or sets the colors for the tabs background area.
        /// </summary>
        public LinearGradientColorTable TabsBackground = new LinearGradientColorTable(ColorScheme.GetColor("BFDBFF"), Color.Empty);

        /// <summary>
        /// Gets or sets the colors for the tabs background area when Windows Glass is enabled.
        /// </summary>
        public LinearGradientColorTable TabsGlassBackground = new LinearGradientColorTable(ColorScheme.GetColor("BFDBFF"), Color.Empty);

        /// <summary>
        /// Gets or sets the color of border which is drawn above the tab.
        /// </summary>
        public Color TabDividerBorder = ColorScheme.GetColor("AECAF0");
        /// <summary>
        /// Gets or sets the light color of border which is drawn above the tab.
        /// </summary>
        public Color TabDividerBorderLight = ColorScheme.GetColor("D4E3F5");

        /// <summary>
        /// Gets or sets the round corner size for the ribbon control parts.
        /// </summary>
        public int CornerSize = 3;

        /// <summary>
        /// Gets or sets the height in pixels of top background part.
        /// </summary>
        public float PanelTopBackgroundHeight = 15;
        /// <summary>
        /// Gets or sets the top background colors.
        /// </summary>
        public LinearGradientColorTable PanelTopBackground = new LinearGradientColorTable(ColorScheme.GetColor("DBE6F4"), ColorScheme.GetColor("CFDDEF"));
        /// <summary>
        /// Gets or sets the bottom background colors.
        /// </summary>
        public LinearGradientColorTable PanelBottomBackground = new LinearGradientColorTable(ColorScheme.GetColor("C9D9ED"), ColorScheme.GetColor("E7F2FF"));

        /// <summary>
        /// Gets or sets the background image used on Office 2007 style start button displayed in top-left corner of ribbon control.
        /// Note that image assigned to all StartButton properties must be the same size. The size for the button will be determined by image
        /// size set on this property.
        /// </summary>
        public Image StartButtonDefault = null;
        /// <summary>
        /// Gets or sets the background image used on Office 2007 style start button displayed in top-left corner of ribbon control when mouse is over the button.
        /// </summary>
        public Image StartButtonMouseOver = null;
        /// <summary>
        /// Gets or sets the background image used on Office 2007 style start button displayed in top-left corner of ribbon control when button is pressed.
        /// </summary>
        public Image StartButtonPressed = null;
    }
}
