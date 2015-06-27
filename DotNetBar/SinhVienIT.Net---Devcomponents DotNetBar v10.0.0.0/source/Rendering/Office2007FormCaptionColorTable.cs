using System;
using System.Text;
using System.Drawing;

namespace DevComponents.DotNetBar.Rendering
{
    /// <summary>
    /// Defines the color table for all states of Office 2007 style form caption.
    /// </summary>
    public class Office2007FormColorTable
    {
        /// <summary>
        /// Gets or sets the color table for caption in active state.
        /// </summary>
        public Office2007FormStateColorTable Active = new Office2007FormStateColorTable();

        /// <summary>
        /// Gets or sets the color table for caption in inactive state.
        /// </summary>
        public Office2007FormStateColorTable Inactive = new Office2007FormStateColorTable();

        /// <summary>
        /// Gets or sets the background color of the form.
        /// </summary>
        public Color BackColor = Color.Empty;

        /// <summary>
        /// Gets or sets the text color of the form.
        /// </summary>
        public Color TextColor = Color.Empty;

        /// <summary>
        /// Gets or sets the MDI Client Background image.
        /// </summary>
        public Image MdiClientBackgroundImage = null;
    }
}
