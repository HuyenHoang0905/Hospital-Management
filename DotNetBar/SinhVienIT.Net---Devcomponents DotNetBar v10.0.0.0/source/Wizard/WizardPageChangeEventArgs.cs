using System;
using System.Text;

namespace DevComponents.DotNetBar
{
    #region WizardPageChangeEventArgs
    /// <summary>
    /// Provides data for Wizard Page Change events.
    /// </summary>
    public class WizardPageChangeEventArgs : EventArgs
    {
        /// <summary>
        /// Specifies the new active wizard page. You can change this argument when handling WizardPageChanging event and provide newly selected page of your own.
        /// </summary>
        public WizardPage NewPage = null;

        /// <summary>
        /// Specifies page that was or currently is active.
        /// </summary>
        public WizardPage OldPage = null;

        /// <summary>
        /// Indicates the wizard button that was source of page change.
        /// </summary>
        public eWizardPageChangeSource PageChangeSource = eWizardPageChangeSource.NextButton;

        /// <summary>
        /// Creates new instance of the class with default values.
        /// </summary>
        /// <param name="newPage">New wizard page</param>
        /// <param name="oldPage">Old or current wizard page</param>
        /// <param name="pageChangeSource">Page change source</param>
        public WizardPageChangeEventArgs(WizardPage newPage, WizardPage oldPage, eWizardPageChangeSource pageChangeSource)
        {
            this.NewPage = newPage;
            this.OldPage = oldPage;
            this.PageChangeSource = pageChangeSource;
        }
    }
    #endregion

    #region WizardCancelPageChangeEventArgs
    /// <summary>
    /// Provides data for Wizard Page Change events.
    /// </summary>
    public class WizardCancelPageChangeEventArgs : WizardPageChangeEventArgs
    {
        /// <summary>
        /// Allows you to cancel the page change.
        /// </summary>
        public bool Cancel = false;

        /// <summary>
        /// Creates new instance of the class with default values.
        /// </summary>
        /// <param name="newPage">New wizard page</param>
        /// <param name="oldPage">Old or current wizard page</param>
        /// <param name="pageChangeSource">Page change source</param>
        public WizardCancelPageChangeEventArgs(WizardPage newPage, WizardPage oldPage, eWizardPageChangeSource pageChangeSource) : base(newPage, oldPage, pageChangeSource)
        {
        }
    }
    #endregion

    #region Event Delegates
    /// <summary>
    /// Defines delegate for WizardPageChange events.
    /// </summary>
    public delegate void WizardCancelPageChangeEventHandler(object sender, WizardCancelPageChangeEventArgs e);

    /// <summary>
    /// Defines delegate for WizardPageChange events.
    /// </summary>
    public delegate void WizardPageChangeEventHandler(object sender, WizardPageChangeEventArgs e);
    #endregion
}
