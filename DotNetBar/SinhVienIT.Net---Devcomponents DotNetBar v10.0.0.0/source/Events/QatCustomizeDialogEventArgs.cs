using System;
using System.Text;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Provides data for the Quick Access Toolbar Customize dialog events.
    /// </summary>
    public class QatCustomizeDialogEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets whether to cancel the current operation. When showing the dialog this allows to cancel the showing. When dialog is closed
        /// it allows to cancel the changes made on customize dialog.
        /// </summary>
        public bool Cancel = false;

        /// <summary>
        /// Gets or sets the reference to the form that is acting as dialog. You can set this value to your custom form to display it instead of
        /// built-in dialog.
        /// </summary>
        public System.Windows.Forms.Form Dialog = null;

        /// <summary>
        /// Creates new instance of the object and initializes it with default values.
        /// </summary>
        /// <param name="dialog">Reference to the dialog being used for customization.</param>
        public QatCustomizeDialogEventArgs(System.Windows.Forms.Form dialog)
        {
            this.Dialog = dialog;
        }
    }
}
