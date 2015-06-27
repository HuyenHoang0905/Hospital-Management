using System;
using System.Text;
using System.Drawing;

namespace DevComponents.DotNetBar.Rendering
{
    /// <summary>
    /// Defines the color table for the quick access toolbar in all states.
    /// </summary>
    public class Office2007QuickAccessToolbarColorTable
    {
        /// <summary>
        /// Gets or sets the colors for the quick access toolbar background when hosted in ribbon control caption and form is active
        /// or the background colors when toolbar is hosted below the ribbon control.
        /// </summary>
        public Office2007QuickAccessToolbarStateColorTable Active = new Office2007QuickAccessToolbarStateColorTable();

        /// <summary>
        /// Gets or sets the colors for the quick access toolbar background when hosted in ribbon control caption and form is inactive
        /// </summary>
        public Office2007QuickAccessToolbarStateColorTable Inactive = new Office2007QuickAccessToolbarStateColorTable();

        /// <summary>
        /// Gets or sets the colors for the quick access toolbar background when positioned below the ribbon bar.
        /// </summary>
        public Office2007QuickAccessToolbarStateColorTable Standalone = new Office2007QuickAccessToolbarStateColorTable();

        /// <summary>
        /// Gets or sets the background color of Customize Quick Access Toolbar menu label displayed on customize quick access toolbar menu.
        /// </summary>
        public Color QatCustomizeMenuLabelBackground = Color.Empty;

        /// <summary>
        /// Gets or sets the text color of Customize Quick Access Toolbar menu label displayed on customize quick access toolbar menu.
        /// </summary>
        public Color QatCustomizeMenuLabelText = Color.Empty;
    }
}
