using System;
using System.Text;
using System.Drawing;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Provides data for LayoutWizardButtons event.
    /// </summary>
    public class WizardButtonsLayoutEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets bounds of Back button.
        /// </summary>
        public Rectangle BackButtonBounds = Rectangle.Empty;

        /// <summary>
        /// Gets or sets bounds of Next button.
        /// </summary>
        public Rectangle NextButtonBounds = Rectangle.Empty;

        /// <summary>
        /// Gets or sets bounds of Finish button.
        /// </summary>
        public Rectangle FinishButtonBounds = Rectangle.Empty;

        /// <summary>
        /// Gets or sets bounds of Cancel button.
        /// </summary>
        public Rectangle CancelButtonBounds = Rectangle.Empty;

        /// <summary>
        /// Gets or sets bounds of Help button.
        /// </summary>
        public Rectangle HelpButtonBounds = Rectangle.Empty;

        /// <summary>
        /// Creates new instance of the class.
        /// </summary>
        public WizardButtonsLayoutEventArgs() { }

        /// <summary>
        /// Creates new instance of the class and initializes it with default values.
        /// </summary>
        public WizardButtonsLayoutEventArgs(Rectangle backBounds, Rectangle nextBounds, Rectangle finishBounds, Rectangle cancelBounds, Rectangle helpBounds)
        {
            this.BackButtonBounds = backBounds;
            this.NextButtonBounds = nextBounds;
            this.FinishButtonBounds = finishBounds;
            this.CancelButtonBounds = cancelBounds;
            this.HelpButtonBounds = helpBounds;
        }
    }

    /// <summary>
    /// Defines delegate for WizardPageChange events.
    /// </summary>
    public delegate void WizardButtonsLayoutEventHandler(object sender, WizardButtonsLayoutEventArgs e);
}
