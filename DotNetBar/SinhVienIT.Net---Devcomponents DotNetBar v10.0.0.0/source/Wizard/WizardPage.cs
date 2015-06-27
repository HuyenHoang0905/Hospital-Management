using System;
using System.Text;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DevComponents.DotNetBar
{
    [ToolboxItem(false), Designer("DevComponents.DotNetBar.Design.WizardPageDesigner, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
    public class WizardPage : PanelControl
    {
        #region Private Variables
        private eWizardButtonState m_BackButtonEnabled = eWizardButtonState.Auto;
        private eWizardButtonState m_BackButtonVisible = eWizardButtonState.Auto;

        private eWizardButtonState m_NextButtonEnabled = eWizardButtonState.Auto;
        private eWizardButtonState m_NextButtonVisible = eWizardButtonState.Auto;

        private eWizardButtonState m_FinishButtonEnabled = eWizardButtonState.Auto;

        private eWizardButtonState m_CancelButtonEnabled = eWizardButtonState.Auto;
        private eWizardButtonState m_CancelButtonVisible = eWizardButtonState.Auto;

        private eWizardButtonState m_HelpButtonEnabled = eWizardButtonState.Auto;
        private eWizardButtonState m_HelpButtonVisible = eWizardButtonState.Auto;

        private string m_PageTitle = "";
        private string m_PageDescription = "";
        private Image m_PageHeaderImage = null;
        private string m_FormCaption = "";

        private bool m_InteriorPage = true;
        #endregion

        #region Events
        /// <summary>
        /// Occurs before page is displayed. This event can cancel the page change. You can perform any additional setup of the Wizard page in this event.
        /// </summary>
        [Description("Occurs before page is displayed")]
        public event WizardCancelPageChangeEventHandler BeforePageDisplayed;

        /// <summary>
        /// Occurs after page has been displayed.This event can cancel the page change. You can perform any additional setup of the Wizard page in this event.
        /// </summary>
        [Description("Occurs before page is displayed")]
        public event WizardPageChangeEventHandler AfterPageDisplayed;

        /// <summary>
        /// Occurs after page is hidden. You can perform any additional steps that are needed to complete wizard step in this event.
        /// </summary>
        public event WizardPageChangeEventHandler AfterPageHidden;

        /// <summary>
        /// Occurs when Back button is clicked. You can cancel any default processing performed by Wizard control by setting Cancel=true on event arguments.
        /// </summary>
        [Description("Occurs when Back button is clicked.")]
        public event CancelEventHandler BackButtonClick;
        /// <summary>
        /// Occurs when Next button is clicked. You can cancel any default processing performed by Wizard control by setting Cancel=true on event arguments.
        /// </summary>
        [Description("Occurs when Next button is clicked.")]
        public event CancelEventHandler NextButtonClick;
        /// <summary>
        /// Occurs when Finish button is clicked. You can cancel any default processing performed by Wizard control by setting Cancel=true on event arguments.
        /// </summary>
        [Description("Occurs when Finish button is clicked.")]
        public event CancelEventHandler FinishButtonClick;
        /// <summary>
        /// Occurs when Cancel button is clicked. You can cancel any default processing performed by Wizard control by setting Cancel=true on event arguments.
        /// </summary>
        [Description("Occurs when Cancel button is clicked.")]
        public event CancelEventHandler CancelButtonClick;
        /// <summary>
        /// Occurs when Help button is clicked. You can cancel any default processing performed by Wizard control by setting Cancel=true on event arguments.
        /// </summary>
        [Description("Occurs when Help button is clicked.")]
        public event CancelEventHandler HelpButtonClick;
        #endregion

		#region Constructor
		public WizardPage() : base()
		{
			this.BackColor = SystemColors.Control;
		}

        protected override void Dispose(bool disposing)
        {
            if (BarUtilities.DisposeItemImages && !this.DesignMode)
            {
                BarUtilities.DisposeImage(ref m_PageHeaderImage);
            }
            base.Dispose(disposing);
        }
		#endregion

        #region Public Properties
        /// <summary>
        /// Gets whether page is currently selected page in Wizard.
        /// </summary>
        [Browsable(false)]
        public bool IsSelected
        {
            get
            {
                Wizard w = this.Parent as Wizard;
                if (w != null)
                {
                    return w.SelectedPage == this;
                }
                return false;
            }
        }
        /// <summary>
        /// Gets or sets whether back button is enabled when page is active. Default value is eWizardButtonState.Auto which indicates that state is
        /// automatically managed by control.
        /// </summary>
        [Browsable(true), DefaultValue(eWizardButtonState.Auto), Category("Page Options"), Description("Indicates whether back button is enabled when page is active.")]
        public eWizardButtonState BackButtonEnabled
        {
            get { return m_BackButtonEnabled; }
            set
            {
                m_BackButtonEnabled = value;
                UpdatePageState();
            }
        }

        /// <summary>
        /// Gets or sets whether back button is visible when page is active. Default value is eWizardButtonState.Auto.
        /// </summary>
        [Browsable(true), DefaultValue(eWizardButtonState.Auto), Category("Page Options"), Description("Indicates whether back button is visible when page is active.")]
        public eWizardButtonState BackButtonVisible
        {
            get { return m_BackButtonVisible; }
            set
            {
                m_BackButtonVisible = value;
                UpdatePageState();
            }
        }

        /// <summary>
        /// Gets or sets whether next button is enabled when page is active. Default value is eWizardButtonState.Auto which indicates that state is
        /// automatically managed by control.
        /// </summary>
        [Browsable(true), DefaultValue(eWizardButtonState.Auto), Category("Page Options"), Description("Indicates whether next button is enabled when page is active.")]
        public eWizardButtonState NextButtonEnabled
        {
            get { return m_NextButtonEnabled; }
            set
            {
                m_NextButtonEnabled = value;
                UpdatePageState();
            }
        }

        /// <summary>
        /// Gets or sets whether next button is visible when page is active. Default value is eWizardButtonState.Auto.
        /// </summary>
        [Browsable(true), DefaultValue(eWizardButtonState.Auto), Category("Page Options"), Description("Indicates whether next button is visible when page is active.")]
        public eWizardButtonState NextButtonVisible
        {
            get { return m_NextButtonVisible; }
            set
            {
                m_NextButtonVisible = value;
                UpdatePageState();
            }
        }

        /// <summary>
        /// Gets or sets whether finish button is enabled when page is active. Default value is eWizardButtonState.Auto which indicates that state is
        /// automatically managed by control.
        /// </summary>
        [Browsable(true), DefaultValue(eWizardButtonState.Auto), Category("Page Options"), Description("Indicates whether finish button is enabled when page is active.")]
        public eWizardButtonState FinishButtonEnabled
        {
            get { return m_FinishButtonEnabled; }
            set
            {
                m_FinishButtonEnabled = value;
                UpdatePageState();
            }
        }

        /// <summary>
        /// Gets or sets whether cancel button is enabled when page is active. Default value is eWizardButtonState.Auto which indicates that state is
        /// automatically managed by control.
        /// </summary>
        [Browsable(true), DefaultValue(eWizardButtonState.Auto), Category("Page Options"), Description("Indicates whether cancel button is enabled when page is active.")]
        public eWizardButtonState CancelButtonEnabled
        {
            get { return m_CancelButtonEnabled; }
            set
            {
                m_CancelButtonEnabled = value;
                UpdatePageState();
            }
        }

        /// <summary>
        /// Gets or sets whether cancel button is visible when page is active. Default value is eWizardButtonState.Auto.
        /// </summary>
        [Browsable(true), DefaultValue(eWizardButtonState.Auto), Category("Page Options"), Description("Indicates whether cancel button is visible when page is active.")]
        public eWizardButtonState CancelButtonVisible
        {
            get { return m_CancelButtonVisible; }
            set
            {
                m_CancelButtonVisible = value;
                UpdatePageState();
            }
        }

        /// <summary>
        /// Gets or sets whether help button is enabled when page is active. Default value is eWizardButtonState.Auto which indicates that state is
        /// automatically managed by control.
        /// </summary>
        [Browsable(true), DefaultValue(eWizardButtonState.Auto), Category("Page Options"), Description("Indicates whether help button is enabled when page is active.")]
        public eWizardButtonState HelpButtonEnabled
        {
            get { return m_HelpButtonEnabled; }
            set
            {
                m_HelpButtonEnabled = value;
                UpdatePageState();
            }
        }

        /// <summary>
        /// Gets or sets whether help button is visible when page is active. Default value is eWizardButtonState.Auto.
        /// </summary>
        [Browsable(true), DefaultValue(eWizardButtonState.Auto), Category("Page Options"), Description("Indicates whether help button is visible when page is active.")]
        public eWizardButtonState HelpButtonVisible
        {
            get { return m_HelpButtonVisible; }
            set
            {
                m_HelpButtonVisible = value;
                UpdatePageState();
            }
        }

        /// <summary>
        /// Gets or sets the page header image when page is an interior page, InteriorPage=true. Default value is null.
        /// </summary>
        [Browsable(true), DefaultValue(null), Category("Page Options"), Description("Indicates the page header image when page is an interior page.")]
        public Image PageHeaderImage
        {
            get { return m_PageHeaderImage; }
            set
            {
                m_PageHeaderImage = value;
                UpdatePageState();
            }
        }

        /// <summary>
        /// Gets or sets the text that is displayed as title in wizard header when page is active.
        /// </summary>
        [Browsable(true), Localizable(true), DefaultValue(""), Category("Page Options"), Description("Indicates text that is displayed as title in wizard header.")]
        public string PageTitle
        {
            get { return m_PageTitle; }
            set
            {
                m_PageTitle = value;
                UpdatePageState();
            }
        }

        /// <summary>
        /// Gets or sets the text that is displayed as description in wizard header when page is active.
        /// </summary>
        [Browsable(true), Localizable(true), DefaultValue(""), Category("Page Options"), Description("Indicates text that is displayed as description in wizard header when page is active.")]
        public string PageDescription
        {
            get { return m_PageDescription; }
            set
            {
                m_PageDescription = value;
                UpdatePageState();
            }
        }

        /// <summary>
        /// Gets or sets the text that is displayed on form caption when page is active. Default value is empty string which indicates that form caption
        /// is not changed when page becomes active.
        /// </summary>
        [Browsable(true), Localizable(true), DefaultValue(""), Category("Page Options"), Description("Indicates text that is displayed on form caption when page is active.")]
        public string FormCaption
        {
            get { return m_FormCaption; }
            set
            {
                m_FormCaption = value;
                UpdatePageState();
            }
        }

        /// <summary>
        /// Gets or sets whether page is interior page. Interior pages use wizard header area to display page title, description and optional image. They are also padded and do not
        /// fill the client area of the Wizard. Default value is true.
        /// You can set this value to false to hide header area and make page fill the client area of the wizard.
        /// </summary>
        [Browsable(true), DefaultValue(true), Category("Page Options"), Description("Indicates whether page is inner page.")]
        public bool InteriorPage
        {
            get { return m_InteriorPage; }
            set
            {
                if (m_InteriorPage != value)
                {
                    m_InteriorPage = value;
                    OnInnerPageChanged();
                }
            }
        }
        #endregion

        #region Internal Implementation
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public override Color BackColor
        {
            get { return base.BackColor; }
            set { base.BackColor = value; }
        }

        public bool ShouldSerializeBackColor()
        {
            return (BackColor != SystemColors.Control);
        }

		/// <summary>
		/// Gets or sets whether page is visible. Page visibility is managed by Wizard control and it should not be set directly.
		/// </summary>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new bool Visible
		{
			get { return base.Visible; }
			set { base.Visible = value; }
		}

        /// <summary>
        /// Updates page state when one of the page appearance properties has changed.
        /// </summary>
        private void UpdatePageState()
        {
            if (this.Parent is Wizard && ((Wizard)this.Parent).SelectedPage==this)
            {
                ((Wizard)this.Parent).UpdatePageDisplay();
                ((Wizard)this.Parent).SetupButtons(false);
            }
        }

        private void OnInnerPageChanged()
        {
            if (this.Parent is Wizard)
            {
                Wizard w=this.Parent as Wizard;
                w.SetupPage(this);
                if(w.SelectedPage==this)
                    w.UpdatePageDisplay();
            }
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);

            if (this.DesignMode && this.InteriorPage)
            {
                Graphics g = e.Graphics;
                Rectangle r = this.DisplayRectangle;
                using (Pen pen = new Pen(Color.FromArgb(100,SystemColors.ControlDarkDark), 1))
                {
                    pen.DashStyle = DashStyle.Dot;
                    
                    System.Drawing.Drawing2D.SmoothingMode sm = g.SmoothingMode;
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;

                    // Outer guide
                    r.Inflate(-12, 0);
                    g.DrawLine(pen, r.X, r.Y, r.X, r.Bottom);
                    g.DrawLine(pen, r.Right, r.Y, r.Right, r.Bottom);

                    // Inner guide 1
                    r.Inflate(-22, 0);
                    g.DrawLine(pen, r.X, r.Y, r.X, r.Bottom);
                    g.DrawLine(pen, r.Right, r.Y, r.Right, r.Bottom);

                    // Inner guide 2
                    r.Inflate(-22, 0);
                    g.DrawLine(pen, r.X, r.Y, r.X, r.Bottom);
                    g.DrawLine(pen, r.Right, r.Y, r.Right, r.Bottom);
                    
                    g.SmoothingMode = sm;
                }
            }
        }
        #endregion

        #region Event Implementation
        /// <summary>
        /// Fires BeforePageDisplayed event.
        /// </summary>
        /// <param name="e">Event arguments</param>
        protected virtual void OnBeforePageDisplayed(WizardCancelPageChangeEventArgs e)
        {
            if (BeforePageDisplayed != null)
                BeforePageDisplayed(this, e);
        }

        /// <summary>
        /// Invokes the BeforePageDisplayed event.
        /// </summary>
        /// <param name="e">Event arguments</param>
        internal void InvokeBeforePageDisplayed(WizardCancelPageChangeEventArgs e)
        {
            OnBeforePageDisplayed(e);
        }

        /// <summary>
        /// Fires AfterPageDisplayed event.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnAfterPageDisplayed(WizardPageChangeEventArgs e)
        {
            if (AfterPageDisplayed != null)
                AfterPageDisplayed(this, e);
        }

        /// <summary>
        /// Invokes AfterPageDisplayed event.
        /// </summary>
        /// <param name="e">Event arguments</param>
        internal void InvokeAfterPageDisplayed(WizardPageChangeEventArgs e)
        {
            OnAfterPageDisplayed(e);
        }

        /// <summary>
        /// Fires BeforePageDisplayed event.
        /// </summary>
        /// <param name="e">Event arguments</param>
        protected virtual void OnAfterPageHidden(WizardPageChangeEventArgs e)
        {
            if (AfterPageHidden != null)
                AfterPageHidden(this, e);
        }

        /// <summary>
        /// Invokes the BeforePageDisplayed event.
        /// </summary>
        /// <param name="e">Event arguments</param>
        internal void InvokeAfterPageHidden(WizardPageChangeEventArgs e)
        {
            OnAfterPageHidden(e);
        }

        /// <summary>
        /// Raises BackButtonClick event.
        /// </summary>
        /// <param name="e">Event arguments</param>
        protected virtual void OnBackButtonClick(CancelEventArgs e)
        {
            if (BackButtonClick != null)
                BackButtonClick(this, e);
        }

        /// <summary>
        /// Invokes BackButtonClick event.
        /// </summary>
        internal void InvokeBackButtonClick(CancelEventArgs e)
        {
            OnBackButtonClick(e);
        }

        /// <summary>
        /// Raises NextButtonClick event.
        /// </summary>
        /// <param name="e">Event arguments</param>
        protected virtual void OnNextButtonClick(CancelEventArgs e)
        {
            if (NextButtonClick != null)
                NextButtonClick(this, e);
        }

        /// <summary>
        /// Invokes NextButtonClick event.
        /// </summary>
        internal void InvokeNextButtonClick(CancelEventArgs e)
        {
            OnNextButtonClick(e);
        }

        /// <summary>
        /// Raises FinishButtonClick event.
        /// </summary>
        /// <param name="e">Event arguments</param>
        protected virtual void OnFinishButtonClick(CancelEventArgs e)
        {
            if (FinishButtonClick != null)
                FinishButtonClick(this, e);
        }

        /// <summary>
        /// Invokes FinishButtonClick event.
        /// </summary>
        internal void InvokeFinishButtonClick(CancelEventArgs e)
        {
            OnFinishButtonClick(e);
        }

        /// <summary>
        /// Raises CancelButtonClick event.
        /// </summary>
        /// <param name="e">Event arguments</param>
        protected virtual void OnCancelButtonClick(CancelEventArgs e)
        {
            if (CancelButtonClick != null)
                CancelButtonClick(this, e);
        }

        /// <summary>
        /// Invokes CancelButtonClick event.
        /// </summary>
        internal void InvokeCancelButtonClick(CancelEventArgs e)
        {
            OnCancelButtonClick(e);
        }

        /// <summary>
        /// Raises HelpButtonClick event.
        /// </summary>
        /// <param name="e">Event arguments</param>
        protected virtual void OnHelpButtonClick(CancelEventArgs e)
        {
            if (HelpButtonClick != null)
                HelpButtonClick(this, e);
        }

        /// <summary>
        /// Invokes HelpButtonClick event.
        /// </summary>
        internal void InvokeHelpButtonClick(CancelEventArgs e)
        {
            OnHelpButtonClick(e);
        }
        #endregion
    }
}
