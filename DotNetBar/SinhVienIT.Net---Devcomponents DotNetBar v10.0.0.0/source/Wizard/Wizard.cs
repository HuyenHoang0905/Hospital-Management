using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Represents Wizard control.
	/// </summary>
    [ToolboxBitmap(typeof(Wizard), "Wizard.Wizard.ico"), ToolboxItem(true), Designer("DevComponents.DotNetBar.Design.WizardDesigner, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
    public class Wizard : UserControl
    {
        #region Private Variables
        private PanelControl panelHeader;
        private int m_ButtonSpacingMajor = 8;
        private int m_ButtonSpacingMinor = 1;
        private int m_ButtonHeight = 22;
        private FlatStyle m_ButtonFlatStyle = FlatStyle.System;
        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public PanelControl panelFooter;
        private ButtonX buttonHelp;
        private ButtonX buttonCancel;
        private ButtonX buttonFinish;
        private ButtonX buttonNext;
        private ButtonX buttonBack;

        private bool m_FinishButtonAlwaysVisible = false;
        private Label labelDescription;
        private Label labelCaption;
        
        private WizardPageCollection m_WizardPages = new WizardPageCollection();
        private Stack m_PagesHistory = new Stack();
        private int m_SelectedPageIndex = 0;

        private bool m_PageChangeDisableButtons = true;
        private bool m_PageChangeWaitCursor = true;
        private eWizardFormAcceptButton m_FormAcceptButton = eWizardFormAcceptButton.FinishAndNext;
        private eWizardFormCancelButton m_FormCancelButton = eWizardFormCancelButton.Cancel;
        private PictureBox pictureHeader;
        private Image m_HeaderImage = null;
        private bool m_HeaderImageChanged = false;
		private bool m_HeaderImageVisible = true;
        private bool m_HelpButtonVisible = true;

        /// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
        #endregion

        #region Events
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

        /// <summary>
        /// Occurs before wizard page has changed and gives you opportunity to cancel the change.
        /// </summary>
        [Description("Occurs before wizard page has changed and gives you opportunity to cancel the change.")]
        public event WizardCancelPageChangeEventHandler WizardPageChanging;
        /// <summary>
        /// Occurs after wizard page has changed. This event cannot be cancelled. To cancel the page change please use WizardPageChanging event.
        /// </summary>
        [Description("Occurs after wizard page has changed.")]
        public event WizardPageChangeEventHandler WizardPageChanged;

        /// <summary>
        /// Occurs when wizard buttons (Back, Next, Finish etc) are positioned and resized.
        /// </summary>
        [Description("Occurs when wizard buttons (Back, Next, Finish etc) are positioned and resized.")]
        public event WizardButtonsLayoutEventHandler LayoutWizardButtons;
        #endregion

        #region Constructor Dispose
        public Wizard()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

            m_WizardPages.ParentWizard = this;
            pictureHeader.BackgroundImage = BarFunctions.LoadBitmap("SystemImages.WizardHeaderImage.png");
            panelFooter.Style.StyleChanged += new EventHandler(FooterStyleStyleChanged);
		}

        private void FooterStyleStyleChanged(object sender, EventArgs e)
        {
            if (panelFooter.Style.BackColor == Color.Transparent && panelFooter.Style.BackColor2.IsEmpty && panelFooter.Style.BackColorBlend.Count == 0)
                panelFooter.BackColor = Color.Transparent;
            else if (panelFooter.BackColor != SystemColors.Control)
                panelFooter.BackColor = SystemColors.Control;
        }

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}

            if (BarUtilities.DisposeItemImages && !this.DesignMode)
            {
                BarUtilities.DisposeImage(ref m_HeaderImage);
            }

			base.Dispose( disposing );
        }
        #endregion

        #region Component Designer generated code
        /// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.panelHeader = new DevComponents.DotNetBar.PanelControl();
            this.pictureHeader = new System.Windows.Forms.PictureBox();
            this.labelDescription = new System.Windows.Forms.Label();
            this.labelCaption = new System.Windows.Forms.Label();
            this.panelFooter = new DevComponents.DotNetBar.PanelControl();
            this.buttonHelp = new ButtonX();
            this.buttonCancel = new ButtonX();
            this.buttonFinish = new ButtonX();
            this.buttonNext = new ButtonX();
            this.buttonBack = new ButtonX();
            this.panelHeader.SuspendLayout();
            this.panelFooter.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.AntiAlias=false;
            this.panelHeader.BackColor = Color.Transparent;
			this.panelHeader.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelHeader.Controls.Add(this.pictureHeader);
            this.panelHeader.Controls.Add(this.labelDescription);
            this.panelHeader.Controls.Add(this.labelCaption);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(548, 60);
            // 
            // 
            // 
            this.panelHeader.Style.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.panelHeader.Style.BackColorGradientAngle = 90;
            this.panelHeader.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Etched;
            this.panelHeader.Style.BorderBottomWidth = 1;
            this.panelHeader.Style.BorderColor = System.Drawing.SystemColors.Control;
            this.panelHeader.Style.BorderLeftWidth = 1;
            this.panelHeader.Style.BorderRightWidth = 1;
            this.panelHeader.Style.BorderTopWidth = 1;
            this.panelHeader.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.panelHeader.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelHeader.TabIndex = 5;
            // 
            // pictureHeader
            // 
            this.pictureHeader.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureHeader.Location = new System.Drawing.Point(496, 6);
            this.pictureHeader.Name = "pictureHeader";
            this.pictureHeader.Size = new System.Drawing.Size(48, 48);
            this.pictureHeader.TabIndex = 7;
            this.pictureHeader.TabStop = false;
            // 
            // labelDescription
            // 
            this.labelDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.labelDescription.Location = new System.Drawing.Point(44, 22);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(446, 32);
            this.labelDescription.TabIndex = 1;
            // 
            // labelCaption
            // 
            this.labelCaption.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.labelCaption.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCaption.Location = new System.Drawing.Point(16, 5);
            this.labelCaption.Name = "labelCaption";
            this.labelCaption.Size = new System.Drawing.Size(474, 17);
            this.labelCaption.TabIndex = 0;
            // 
            // panelFooter
            // 
			this.panelFooter.BackColor = System.Drawing.SystemColors.Control;
			this.panelFooter.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelFooter.Controls.Add(this.buttonHelp);
            this.panelFooter.Controls.Add(this.buttonCancel);
            this.panelFooter.Controls.Add(this.buttonFinish);
            this.panelFooter.Controls.Add(this.buttonNext);
            this.panelFooter.Controls.Add(this.buttonBack);
            this.panelFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelFooter.Location = new System.Drawing.Point(0, 329);
            this.panelFooter.Name = "panelFooter";
            this.panelFooter.Size = new System.Drawing.Size(548, 46);
			this.panelFooter.AntiAlias = false;
            // 
            // 
            // 
            this.panelFooter.TabIndex = 6;
            this.panelFooter.Resize += new System.EventHandler(this.panelFooter_Resize);
            // 
            // buttonHelp
            // 
            this.buttonHelp.CausesValidation = false;
            this.buttonHelp.Location = new System.Drawing.Point(462, 13);
            this.buttonHelp.Name = "buttonHelp";
            this.buttonHelp.Size = new System.Drawing.Size(74, 22);
            this.buttonHelp.TabIndex = 5;
            this.buttonHelp.Text = "Help";
            this.buttonHelp.VisibleChanged += new System.EventHandler(this.CommandButtonVisibleChanged);
            this.buttonHelp.Click += new System.EventHandler(this.buttonHelp_Click);
            this.buttonHelp.ThemeAware = true;
            this.buttonHelp.Style = eDotNetBarStyle.Office2000;
            this.buttonHelp.ColorTable = eButtonColor.Office2007WithBackground;
            // 
            // buttonCancel
            // 
            this.buttonCancel.CausesValidation = false;
            this.buttonCancel.Location = new System.Drawing.Point(382, 13);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(74, 22);
            this.buttonCancel.TabIndex = 4;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.VisibleChanged += new System.EventHandler(this.CommandButtonVisibleChanged);
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            this.buttonCancel.ThemeAware = true;
            this.buttonCancel.Style = eDotNetBarStyle.Office2000;
            this.buttonCancel.ColorTable = eButtonColor.Office2007WithBackground;
            // 
            // buttonFinish
            // 
            this.buttonFinish.Location = new System.Drawing.Point(148, 13);
            this.buttonFinish.Name = "buttonFinish";
            this.buttonFinish.Size = new System.Drawing.Size(74, 22);
            this.buttonFinish.TabIndex = 3;
            this.buttonFinish.Text = "Finish";
            this.buttonFinish.VisibleChanged += new System.EventHandler(this.CommandButtonVisibleChanged);
            this.buttonFinish.Click += new System.EventHandler(this.buttonFinish_Click);
            this.buttonFinish.ThemeAware = true;
            this.buttonFinish.Style = eDotNetBarStyle.Office2000;
            this.buttonFinish.ColorTable = eButtonColor.Office2007WithBackground;
            // 
            // buttonNext
            // 
            this.buttonNext.Location = new System.Drawing.Point(302, 13);
            this.buttonNext.Name = "buttonNext";
            this.buttonNext.Size = new System.Drawing.Size(74, 22);
            this.buttonNext.TabIndex = 2;
            this.buttonNext.Text = "Next >";
            this.buttonNext.VisibleChanged += new System.EventHandler(this.CommandButtonVisibleChanged);
            this.buttonNext.Click += new System.EventHandler(this.buttonNext_Click);
            this.buttonNext.ThemeAware = true;
            this.buttonNext.Style = eDotNetBarStyle.Office2000;
            this.buttonNext.ColorTable = eButtonColor.Office2007WithBackground;
            // 
            // buttonBack
            // 
            this.buttonBack.CausesValidation = false;
            this.buttonBack.Location = new System.Drawing.Point(228, 13);
            this.buttonBack.Name = "buttonBack";
            this.buttonBack.Size = new System.Drawing.Size(74, 22);
            this.buttonBack.TabIndex = 1;
            this.buttonBack.Text = "< Back";
            this.buttonBack.VisibleChanged += new System.EventHandler(this.CommandButtonVisibleChanged);
            this.buttonBack.Click += new System.EventHandler(this.buttonBack_Click);
            this.buttonBack.ThemeAware = true;
            this.buttonBack.Style = eDotNetBarStyle.Office2000;
            this.buttonBack.ColorTable = eButtonColor.Office2007WithBackground;
            // 
            // Wizard
            // 
            this.Controls.Add(this.panelHeader);
            this.Controls.Add(this.panelFooter);
            this.Name = "Wizard";
            this.Size = new System.Drawing.Size(548, 375);
            this.panelHeader.ResumeLayout(false);
            this.panelFooter.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

        #region Public Properties
        /// <summary>
        /// Gets or sets the header image. Default value is null which means that internal header image is used. You can hide header image
        /// by setting HeaderImageVisible property.
        /// </summary>
        [Browsable(true), Category("Header and Footer"), DefaultValue(null), Description("Indicates the header image.")]
        public Image HeaderImage
        {
            get { return m_HeaderImage; }
            set
            {
                m_HeaderImage = value;
                m_HeaderImageChanged = true;
                UpdatePageDisplay();
            }
        }

        /// <summary>
        /// Gets or sets whether header image is visible. Default value is true.
        /// </summary>
        [Browsable(true), Category("Header and Footer"), DefaultValue(true), Description("Indicates whether header image is visible.")]
        public bool HeaderImageVisible
        {
            get { return m_HeaderImageVisible; }
            set
            {
				m_HeaderImageVisible=value;
                if (pictureHeader.Visible != m_HeaderImageVisible)
                {
                    pictureHeader.Visible = m_HeaderImageVisible;
                    LayoutHeader();
                }
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new Control.ControlCollection Controls
        {
            get
            {
                return base.Controls;
            }
        }
        /// <summary>
        /// Gets or sets wizard button that is clicked when ENTER key is pressed. Default value is eWizardFormAcceptButton.FinishAndNext which 
        /// indicates that finish button will be clicked if available otherwise next button will be clicked.
        /// </summary>
        [Browsable(true), DefaultValue(eWizardFormAcceptButton.FinishAndNext), Category("Wizard Behavior"), Description("Indicates wizard button that is clicked when ENTER key is pressed.")]
        public eWizardFormAcceptButton FormAcceptButton
        {
            get { return m_FormAcceptButton; }
            set
            {
                m_FormAcceptButton = value;
                SetFormAcceptButton();
            }
        }

        /// <summary>
        /// Gets or sets wizard button that is clicked when ESCAPE key is pressed. Default value is eWizardFormCancelButton.Cancel which 
        /// indicates that Cancel button will be clicked.
        /// </summary>
        [Browsable(true), DefaultValue(eWizardFormCancelButton.Cancel), Category("Wizard Behavior"), Description("Indicates wizard button that is clicked when ESCAPE key is pressed.")]
        public eWizardFormCancelButton FormCancelButton
        {
            get { return m_FormCancelButton; }
            set
            {
                m_FormCancelButton = value;
                SetFormCancelButton();
            }
        }

        /// <summary>
        /// Gets or sets whether all buttons are disabled while wizard page is changed which prevents users from clicking the buttons
        /// if page change is taking longer. Default value is true.
        /// </summary>
        [Browsable(true), DefaultValue(true), Category("Wizard Behavior"), Description("Indicates whether all buttons are disabled while wizard page is changed.")]
        public bool PageChangeDisableButtons
        {
            get { return m_PageChangeDisableButtons; }
            set { m_PageChangeDisableButtons = value; }
        }

        /// <summary>
        /// Gets or sets whether wait cursor is displayed while page is changed. Default value is true.
        /// </summary>
        [Browsable(true), DefaultValue(true), Category("Wizard Behavior"), Description("Indicates whether wait cursor is displayed while page is changed.")]
        public bool PageChangeWaitCursor
        {
            get { return m_PageChangeWaitCursor; }
            set { m_PageChangeWaitCursor = value; }
        }

        /// <summary>
        /// Gets or sets the selected page index. You can set this property to change the currently selected wizard page.
        /// </summary>
        [Browsable(false), Category("Wizard Pages"), Description("Indicates selected page index."), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int SelectedPageIndex
        {
            get { return m_SelectedPageIndex; }
            set
            {
                if (value < 0)
                    value = 0;

                if (value < m_WizardPages.Count)
                    ShowPage(m_WizardPages[value], eWizardPageChangeSource.Code);
                else
                    m_SelectedPageIndex = value;
            }
        }

        /// <summary>
        /// Gets or sets selected wizard page. You can set this property to change the currently selected wizard page.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public WizardPage SelectedPage
        {
            get
            {
                if (m_SelectedPageIndex < m_WizardPages.Count)
                    return m_WizardPages[m_SelectedPageIndex];
                return null;
            }
            set
            {
                if (!m_WizardPages.Contains(value))
                    throw new InvalidOperationException("WizardPage is not member of WizardPages collection. Add page to the WizardPages collection before setting this property");
                ShowPage(value, eWizardPageChangeSource.Code);
            }
        }

        /// <summary>
        /// Gets the collection of Wizard pages. The order of WizardPage objects inside of this collection determines the flow of the wizard.
        /// </summary>
        [Browsable(true), Category("Wizard Pages"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public WizardPageCollection WizardPages
        {
            get
            {
                return m_WizardPages;
            }
        }

        /// <summary>
        /// Returns a Stack of page history. Each time next page is displayed by wizard, previously visited page is added to the history.
        /// When user commands Wizard back, the last page from the history is shown and removed from the stack. You should not modify this collection
        /// directly since it is maintained by Wizard control.
        /// </summary>
        [Browsable(false), Category("Wizard Pages"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Stack PagesHistory
        {
            get
            {
                return m_PagesHistory;
            }
        }

        /// <summary>
        /// Gets or sets the FlatStyle setting for the wizard buttons. Default value is FlatStyle.System
        /// </summary>
        [DefaultValue(FlatStyle.System), Browsable(false), Category("Wizard Buttons"), Description("Indicates flat style for buttons"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Obsolete("Property is obsolete and no longer applies")]
        public FlatStyle ButtonFlatStyle
        {
            get { return m_ButtonFlatStyle; }
            set
            {
                m_ButtonFlatStyle = value;
            }
        }

        /// <summary>
        /// Gets or sets height of wizard command buttons. Default value is 22 pixels.
        /// </summary>
        [Browsable(true), DefaultValue(22), Category("Wizard Buttons"), Description("Indicates the height of the wizard command buttons")]
        public int ButtonHeight
        {
            get { return m_ButtonHeight; }
            set
            {
                if (m_ButtonHeight != value)
                {
                    m_ButtonHeight = value;
                    RepositionButtons();
                }
            }
        }

        /// <summary>
        /// Gets or sets whether back button causes validation to be performed on any controls that require validation when it receives focus. Default value is false.
        /// </summary>
        [Browsable(true), DefaultValue(false), Category("Wizard Buttons"), Description("Indicates whether back button causes validation to be performed on any controls that require validation when it receives focus.")]
        public bool BackButtonCausesValidation
        {
            get { return buttonBack.CausesValidation; }
            set {buttonBack.CausesValidation = value;}
        }

        /// <summary>
        /// Gets or sets tab index of back button. Default value is 1.
        /// </summary>
        [Browsable(true), DefaultValue(1), Category("Wizard Buttons"), Description("Indicates tab index of back button.")]
        public int BackButtonTabIndex
        {
            get { return buttonBack.TabIndex; }
            set { buttonBack.TabIndex = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the user can give the focus to this back button using the TAB key. Default value is true.
        /// </summary>
        [Browsable(true), DefaultValue(true), Category("Wizard Buttons"), Description("Indicates whether the user can give the focus to this back button using the TAB key.")]
        public bool BackButtonTabStop
        {
            get { return buttonBack.TabStop; }
            set { buttonBack.TabStop = value; }
        }

        /// <summary>
        /// Gets or sets caption of the back button.
        /// </summary>
        [Browsable(true), Localizable(true), DefaultValue("< Back"), Category("Wizard Buttons"), Description("Indicates caption of the button")]
        public string BackButtonText
        {
            get { return buttonBack.Text; }
            set { buttonBack.Text = value; }
        }

        /// <summary>
        /// Gets or sets width of the back button. Default value is 74.
        /// </summary>
        [Browsable(true), DefaultValue(74), Category("Wizard Buttons"), Description("Indicates width of button")]
        public int BackButtonWidth
        {
            get { return buttonBack.Width; }
            set
            {
                if (buttonBack.Width != value)
                {
                    buttonBack.Width = value;
                    RepositionButtons();
                }
            }
        }

#if FRAMEWORK20
        /// <summary>
        /// Gets or sets auto size of the button. Default value is false.
        /// </summary>
        [Browsable(true), DefaultValue(false), Category("Wizard Buttons"), Description("Indicates whether button is auto sized")]
        public bool BackButtonAutoSize
        {
            get { return buttonBack.AutoSize; }
            set
            {
                if (buttonBack.AutoSize != value)
                {
                    buttonBack.AutoSize = value;
                    RepositionButtons();
                }
            }
        }

        /// <summary>
        /// Gets or sets auto size mode of the button. Default value is AutoSizeMode.GrowOnly.
        /// </summary>
        [Browsable(true), DefaultValue(AutoSizeMode.GrowOnly), Category("Wizard Buttons"), Description("Indicates button auto-size mode")]
        public AutoSizeMode BackButtonAutoSizeMode
        {
            get { return buttonBack.AutoSizeMode; }
            set
            {
                if (buttonBack.AutoSizeMode != value)
                {
                    buttonBack.AutoSizeMode = value;
                    RepositionButtons();
                }
            }
        }
#endif

        /// <summary>
        /// Gets or sets whether next button causes validation to be performed on any controls that require validation when it receives focus. Default value is true.
        /// </summary>
        [Browsable(true), DefaultValue(true), Category("Wizard Buttons"), Description("Indicates whether button causes validation to be performed on any controls that require validation when it receives focus.")]
        public bool NextButtonCausesValidation
        {
            get { return buttonNext.CausesValidation; }
            set { buttonNext.CausesValidation = value; }
        }

        /// <summary>
        /// Gets or sets tab index of next button. Default value is 2.
        /// </summary>
        [Browsable(true), DefaultValue(2), Category("Wizard Buttons"), Description("Indicates tab index of next button.")]
        public int NextButtonTabIndex
        {
            get { return buttonNext.TabIndex; }
            set { buttonNext.TabIndex = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the user can give the focus to button using the TAB key. Default value is true.
        /// </summary>
        [Browsable(true), DefaultValue(true), Category("Wizard Buttons"), Description("Indicates whether the user can give the focus to button using the TAB key.")]
        public bool NextButtonTabStop
        {
            get { return buttonNext.TabStop; }
            set { buttonNext.TabStop = value; }
        }

        /// <summary>
        /// Gets or sets caption of the next button.
        /// </summary>
        [Browsable(true), Localizable(true), DefaultValue("Next >"), Category("Wizard Buttons"), Description("Indicates caption of the button")]
        public string NextButtonText
        {
            get { return buttonNext.Text; }
            set { buttonNext.Text = value; }
        }

        /// <summary>
        /// Gets or sets width of the next button. Default value is 74.
        /// </summary>
        [Browsable(true), DefaultValue(74), Category("Wizard Buttons"), Description("Indicates width of button")]
        public int NextButtonWidth
        {
            get { return buttonNext.Width; }
            set
            {
                if (buttonNext.Width != value)
                {
                    buttonNext.Width = value;
                    RepositionButtons();
                }
            }
        }

#if FRAMEWORK20
        /// <summary>
        /// Gets or sets auto size of the button. Default value is false.
        /// </summary>
        [Browsable(true), DefaultValue(false), Category("Wizard Buttons"), Description("Indicates whether button is auto sized")]
        public bool NextButtonAutoSize
        {
            get { return buttonNext.AutoSize; }
            set
            {
                if (buttonNext.AutoSize != value)
                {
                    buttonNext.AutoSize = value;
                    RepositionButtons();
                }
            }
        }

        /// <summary>
        /// Gets or sets auto size mode of the button. Default value is AutoSizeMode.GrowOnly.
        /// </summary>
        [Browsable(true), DefaultValue(AutoSizeMode.GrowOnly), Category("Wizard Buttons"), Description("Indicates button auto-size mode")]
        public AutoSizeMode NextButtonAutoSizeMode
        {
            get { return buttonNext.AutoSizeMode; }
            set
            {
                if (buttonNext.AutoSizeMode != value)
                {
                    buttonNext.AutoSizeMode = value;
                    RepositionButtons();
                }
            }
        }
#endif

        /// <summary>
        /// Gets or sets whether button causes validation to be performed on any controls that require validation when it receives focus. Default value is false.
        /// </summary>
        [Browsable(true), DefaultValue(false), Category("Wizard Buttons"), Description("Indicates whether button causes validation to be performed on any controls that require validation when it receives focus.")]
        public bool CancelButtonCausesValidation
        {
            get { return buttonCancel.CausesValidation; }
            set { buttonCancel.CausesValidation = value; }
        }

        /// <summary>
        /// Gets or sets tab index of the button. Default value is 4.
        /// </summary>
        [Browsable(true), DefaultValue(4), Category("Wizard Buttons"), Description("Indicates tab index of the button.")]
        public int CancelButtonTabIndex
        {
            get { return buttonCancel.TabIndex; }
            set { buttonCancel.TabIndex = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the user can give the focus to button using the TAB key. Default value is true.
        /// </summary>
        [Browsable(true), DefaultValue(true), Category("Wizard Buttons"), Description("Indicates whether the user can give the focus to button using the TAB key.")]
        public bool CancelButtonTabStop
        {
            get { return buttonCancel.TabStop; }
            set { buttonCancel.TabStop = value; }
        }

        /// <summary>
        /// Gets or sets caption of the button.
        /// </summary>
        [Browsable(true), Localizable(true), Category("Wizard Buttons"), Description("Indicates caption of the button")] // DefaultValue("Cancel") attribute removed since WinForms designer had trouble localizing with it
        public string CancelButtonText
        {
            get { return buttonCancel.Text; }
            set { buttonCancel.Text = value; }
        }

        /// <summary>
        /// Gets or sets width of the button. Default value is 74.
        /// </summary>
        [Browsable(true), DefaultValue(74), Category("Wizard Buttons"), Description("Indicates width of button")]
        public int CancelButtonWidth
        {
            get { return buttonCancel.Width; }
            set
            {
                if (buttonCancel.Width != value)
                {
                    buttonCancel.Width = value;
                    RepositionButtons();
                }
            }
        }

#if FRAMEWORK20
        /// <summary>
        /// Gets or sets auto size of the button. Default value is false.
        /// </summary>
        [Browsable(true), DefaultValue(false), Category("Wizard Buttons"), Description("Indicates whether button is auto sized")]
        public bool CancelButtonAutoSize
        {
            get { return buttonCancel.AutoSize; }
            set
            {
                if (buttonCancel.AutoSize != value)
                {
                    buttonCancel.AutoSize = value;
                    RepositionButtons();
                }
            }
        }

        /// <summary>
        /// Gets or sets auto size mode of the button. Default value is AutoSizeMode.GrowOnly.
        /// </summary>
        [Browsable(true), DefaultValue(AutoSizeMode.GrowOnly), Category("Wizard Buttons"), Description("Indicates button auto-size mode")]
        public AutoSizeMode CancelButtonAutoSizeMode
        {
            get { return buttonCancel.AutoSizeMode; }
            set
            {
                if (buttonCancel.AutoSizeMode != value)
                {
                    buttonCancel.AutoSizeMode = value;
                    RepositionButtons();
                }
            }
        }
#endif

        /// <summary>
        /// Gets or sets whether button causes validation to be performed on any controls that require validation when it receives focus. Default value is true.
        /// </summary>
        [Browsable(true), DefaultValue(true), Category("Wizard Buttons"), Description("Indicates whether button causes validation to be performed on any controls that require validation when it receives focus.")]
        public bool FinishButtonCausesValidation
        {
            get { return buttonFinish.CausesValidation; }
            set { buttonFinish.CausesValidation = value; }
        }

        /// <summary>
        /// Gets or sets tab index of the button. Default value is 4.
        /// </summary>
        [Browsable(true), DefaultValue(4), Category("Wizard Buttons"), Description("Indicates tab index of the button.")]
        public int FinishButtonTabIndex
        {
            get { return buttonFinish.TabIndex; }
            set { buttonFinish.TabIndex = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the user can give the focus to button using the TAB key. Default value is true.
        /// </summary>
        [Browsable(true), DefaultValue(true), Category("Wizard Buttons"), Description("Indicates whether the user can give the focus to button using the TAB key.")]
        public bool FinishButtonTabStop
        {
            get { return buttonFinish.TabStop; }
            set { buttonFinish.TabStop = value; }
        }

        /// <summary>
        /// Gets or sets caption of the button.
        /// </summary>
        [Browsable(true), Localizable(true), DefaultValue("Finish"), Category("Wizard Buttons"), Description("Indicates caption of the button")]
        public string FinishButtonText
        {
            get { return buttonFinish.Text; }
            set { buttonFinish.Text = value; }
        }

        /// <summary>
        /// Gets or sets width of the button. Default value is 74.
        /// </summary>
        [Browsable(true), DefaultValue(74), Category("Wizard Buttons"), Description("Indicates width of button")]
        public int FinishButtonWidth
        {
            get { return buttonFinish.Width; }
            set
            {
                if (buttonFinish.Width != value)
                {
                    buttonFinish.Width = value;
                    RepositionButtons();
                }
            }
        }

        /// <summary>
        /// Gets or sets whether finish button is always visible next to the Next button. Default value is false which means that Finish
        /// button will be visible only on last Wizard page and it will replace the Next button. When set to true Finish button is always visible next
        /// to the Next button except on first Welcome wizard page.
        /// </summary>
        [Browsable(true), DefaultValue(false), Category("Wizard Buttons"), Description("Indicates whether Finish button is always visible or only when needed.")]
        public bool FinishButtonAlwaysVisible
        {
            get { return m_FinishButtonAlwaysVisible; }
            set
            {
                if (m_FinishButtonAlwaysVisible != value)
                {
                    m_FinishButtonAlwaysVisible = value;
                    OnFinishButtonAlwaysVisibleChanged();
                    RepositionButtons();
                }
            }
        }

#if FRAMEWORK20
        /// <summary>
        /// Gets or sets auto size of the button. Default value is false.
        /// </summary>
        [Browsable(true), DefaultValue(false), Category("Wizard Buttons"), Description("Indicates whether button is auto sized")]
        public bool FinishButtonAutoSize
        {
            get { return buttonFinish.AutoSize; }
            set
            {
                if (buttonFinish.AutoSize != value)
                {
                    buttonFinish.AutoSize = value;
                    RepositionButtons();
                }
            }
        }

        /// <summary>
        /// Gets or sets auto size mode of the button. Default value is AutoSizeMode.GrowOnly.
        /// </summary>
        [Browsable(true), DefaultValue(AutoSizeMode.GrowOnly), Category("Wizard Buttons"), Description("Indicates button auto-size mode")]
        public AutoSizeMode FinishButtonAutoSizeMode
        {
            get { return buttonFinish.AutoSizeMode; }
            set
            {
                if (buttonFinish.AutoSizeMode != value)
                {
                    buttonFinish.AutoSizeMode = value;
                    RepositionButtons();
                }
            }
        }
#endif

        /// <summary>
        /// Gets or sets whether button causes validation to be performed on any controls that require validation when it receives focus. Default value is false.
        /// </summary>
        [Browsable(true), DefaultValue(false), Category("Wizard Buttons"), Description("Indicates whether button causes validation to be performed on any controls that require validation when it receives focus.")]
        public bool HelpButtonCausesValidation
        {
            get { return buttonHelp.CausesValidation; }
            set { buttonHelp.CausesValidation = value; }
        }

        /// <summary>
        /// Gets or sets whether button is visible. Default value is true.
        /// </summary>
        [Browsable(true), DefaultValue(true), Category("Wizard Buttons"), Description("Indicates whether button is visible")]
        public bool HelpButtonVisible
        {
            get { return m_HelpButtonVisible; }
            set
            {
                m_HelpButtonVisible = value;
                buttonHelp.Visible = m_HelpButtonVisible;
                RepositionButtons();
            }
        }

        /// <summary>
        /// Gets or sets tab index of the button. Default value is 5.
        /// </summary>
        [Browsable(true), DefaultValue(5), Category("Wizard Buttons"), Description("Indicates tab index of the button.")]
        public int HelpButtonTabIndex
        {
            get { return buttonHelp.TabIndex; }
            set { buttonHelp.TabIndex = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the user can give the focus to button using the TAB key. Default value is true.
        /// </summary>
        [Browsable(true), DefaultValue(true), Category("Wizard Buttons"), Description("Indicates whether the user can give the focus to button using the TAB key.")]
        public bool HelpButtonTabStop
        {
            get { return buttonHelp.TabStop; }
            set { buttonHelp.TabStop = value; }
        }

        /// <summary>
        /// Gets or sets caption of the button.
        /// </summary>
        [Browsable(true), Localizable(true), DefaultValue("Help"), Category("Wizard Buttons"), Description("Indicates caption of the button")]
        public string HelpButtonText
        {
            get { return buttonHelp.Text; }
            set { buttonHelp.Text = value; }
        }

        /// <summary>
        /// Gets or sets width of the button. Default value is 74.
        /// </summary>
        [Browsable(true), DefaultValue(74), Category("Wizard Buttons"), Description("Indicates width of button")]
        public int HelpButtonWidth
        {
            get { return buttonHelp.Width; }
            set
            {
                if (buttonHelp.Width != value)
                {
                    buttonHelp.Width = value;
                    RepositionButtons();
                }
            }
        }

#if FRAMEWORK20
        /// <summary>
        /// Gets or sets auto size of the button. Default value is false.
        /// </summary>
        [Browsable(true), DefaultValue(false), Category("Wizard Buttons"), Description("Indicates whether button is auto sized")]
        public bool HelpButtonAutoSize
        {
            get { return buttonHelp.AutoSize; }
            set
            {
                if (buttonHelp.AutoSize != value)
                {
                    buttonHelp.AutoSize = value;
                    RepositionButtons();
                }
            }
        }

        /// <summary>
        /// Gets or sets auto size mode of the button. Default value is AutoSizeMode.GrowOnly.
        /// </summary>
        [Browsable(true), DefaultValue(AutoSizeMode.GrowOnly), Category("Wizard Buttons"), Description("Indicates button auto-size mode")]
        public AutoSizeMode HelpButtonAutoSizeMode
        {
            get { return buttonHelp.AutoSizeMode; }
            set
            {
                if (buttonHelp.AutoSizeMode != value)
                {
                    buttonHelp.AutoSizeMode = value;
                    RepositionButtons();
                }
            }
        }
#endif

        /// <summary>
        /// Gets or sets the height of the wizard footer. Default value is 46
        /// </summary>
        [Browsable(true), DefaultValue(46), Category("Header and Footer"), Description("Indicates height of the wizard footer.")]
        public int FooterHeight
        {
            get { return panelFooter.Height; }
            set
            {
                if (panelFooter.Height != value)
                {
                    panelFooter.Height = value;
                    RepositionButtons();
                    SetupPage(this.SelectedPage);
                }
            }
        }

        /// <summary>
        /// Gets or sets the height of the wizard header. Default value is 60
        /// </summary>
        [Browsable(true), DefaultValue(60), Category("Header and Footer"), Description("Indicates height of the wizard header.")]
        public int HeaderHeight
        {
            get { return panelHeader.Height; }
            set
            {
                if (panelHeader.Height != value)
                {
                    panelHeader.Height = value;
                    SetupPage(this.SelectedPage);
                }
            }
        }

        private eWizardTitleImageAlignment _HeaderImageAlignment = eWizardTitleImageAlignment.Right;
        /// <summary>
        /// Gets or sets the header image alignment. Default value is right.
        /// </summary>
        [DefaultValue(eWizardTitleImageAlignment.Right), Category("Header and Footer"), Description("Indicates header image alignment.")]
        public eWizardTitleImageAlignment HeaderImageAlignment
        {
            get { return _HeaderImageAlignment; }
            set { _HeaderImageAlignment = value; LayoutHeader(); }
        }

        /// <summary>
        /// Gets or sets the header image size for interior wizard pages. Default value is 48x48
        /// </summary>
        [Browsable(true), Category("Header and Footer"), Description("Indicates header image size for interior wizard pages.")]
        public Size HeaderImageSize
        {
            get { return pictureHeader.Size; }
            set
            {
                if (pictureHeader.Size != value)
                {
                    pictureHeader.Size = value;
                    LayoutHeader();
                }
            }
        }
        private bool ShouldSerializeHeaderImageSize()
        {
            return (pictureHeader.Size.Width != 48 || pictureHeader.Size.Height != 48);
        }
        private void ResetHeaderImageSize()
        {
            TypeDescriptor.GetProperties(this)["HeaderImageSize"].SetValue(this, new Size(48, 48));
        }

        /// <summary>
        /// Gets or sets indentation of header title label. Default value is 16.
        /// </summary>
        [Browsable(true), DefaultValue(16),Category("Header and Footer"), Description("Indicates indentation of header title label.")]
        public int HeaderTitleIndent
        {
            get { return labelCaption.Left; }
            set
            {
                if (labelCaption.Left != value)
                {
                    labelCaption.Left = value;
                    LayoutHeader();
                }
            }
        }

        /// <summary>
        /// Gets or sets indentation of header description label. Default value is 44.
        /// </summary>
        [Browsable(true), DefaultValue(44), Category("Header and Footer"), Description("Indicates indentation of header description label.")]
        public int HeaderDescriptionIndent
        {
            get { return labelDescription.Left; }
            set
            {
                if (labelDescription.Left != value)
                {
                    labelDescription.Left = value;
                    LayoutHeader();
                }
            }
        }

        // <summary>
        /// Indicates the font used to render header description text.
        /// </summary>
        [Category("Header and Footer"), Description(" Indicates the font used to render header description text.")]
        public Font HeaderDescriptionFont
        {
            get { return labelDescription.Font; }
            set { labelDescription.Font = value; LayoutHeader(); }
        }

        /// <summary>
        /// Indicates the font used to render caption header text.
        /// </summary>
        [Category("Header and Footer"), Description("Indicates the font used to render caption header text.")]
        public Font HeaderCaptionFont
        {
            get { return labelCaption.Font; }
            set { labelCaption.Font = value; LayoutHeader(); }
        }

        private bool _HeaderDescriptionVisible = true;
        /// <summary>
        /// Gets or sets whether description text displayed in wizard header is visible.
        /// </summary>
        [DefaultValue(true), Category("Header and Footer"), Description("Indicates whether description text displayed in wizard header is visible.")]
        public bool HeaderDescriptionVisible
        {
            get { return _HeaderDescriptionVisible; }
            set
            {
                _HeaderDescriptionVisible = value;
                labelDescription.Visible = value;
                LayoutHeader();
            }
        }
        

        /// <summary>
        /// Gets or sets the header background style.
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), NotifyParentPropertyAttribute(true), Category("Style"), Description("Gets or sets header background style."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ElementStyle HeaderStyle
        {
            get
            {
                return panelHeader.Style;
            }
        }
        /// <summary>
        ///     Resets the style to it's default value.
        /// </summary>
        private void ResetHeaderStyle()
        {
            panelHeader.ResetStyle();
        }

        /// <summary>
        /// Gets or sets the footer background style.
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), NotifyParentPropertyAttribute(true), Category("Style"), Description("Gets or sets footer background style."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ElementStyle FooterStyle
        {
            get
            {
                return panelFooter.Style;
            }
        }
        /// <summary>
        ///     Resets the style to it's default value.
        /// </summary>
        private void ResetFooterStyle()
        {
            panelFooter.ResetStyle();
        }

        #endregion

        #region Internal Implementation
        /// <summary>
        /// Simulates Back button click on Wizard control. Note that this method will raise the same events as 
        /// standard Wizard Back button click.
        /// </summary>
        public void NavigateBack()
        {
            CancelEventArgs ce = new CancelEventArgs();
            OnBackButtonClick(ce);
            if (ce.Cancel)
                return;

            WizardPage page = this.SelectedPage;
            if (page != null)
            {
                page.InvokeBackButtonClick(ce);
                if (ce.Cancel)
                    return;
            }

            page = GetBackPage();

            if (page != null)
            {
                ShowPage(page, eWizardPageChangeSource.BackButton);
            }
        }

        /// <summary>
        /// Simulates Next button click on Wizard control. Note that this method will raise the same events as 
        /// standard Wizard Next button click.
        /// </summary>
        public void NavigateNext()
        {
            CancelEventArgs ce = new CancelEventArgs();
            OnNextButtonClick(ce);
            if (ce.Cancel)
                return;

            WizardPage page = this.SelectedPage;
            if (page != null)
            {
                page.InvokeNextButtonClick(ce);
                if (ce.Cancel)
                    return;
            }

            // Go to next page
            page = GetNextPage();
            if (page == null)
                return;

            WizardPage oldPage = this.SelectedPage;
            if (ShowPage(page, eWizardPageChangeSource.NextButton))
            {
                if (!this.DesignMode)
                    m_PagesHistory.Push(oldPage);
            }
        }

        /// <summary>
        /// Simulates Cancel button click on Wizard control. Note that this method will raise the same events as 
        /// standard Wizard Cancel button click.
        /// </summary>
        public void NavigateCancel()
        {
            CancelEventArgs ce = new CancelEventArgs();
            OnCancelButtonClick(ce);
            if (ce.Cancel)
                return;

            WizardPage page = this.SelectedPage;
            if (page != null)
            {
                page.InvokeCancelButtonClick(ce);
                if (ce.Cancel)
                    return;
            }
        }

        /// <summary>
        /// Simulates Finish button click on Wizard control. Note that this method will raise the same events as 
        /// standard Wizard Finish button click.
        /// </summary>
        public void NavigateFinish()
        {
            CancelEventArgs ce = new CancelEventArgs();
            OnFinishButtonClick(ce);
            if (ce.Cancel)
                return;

            WizardPage page = this.SelectedPage;
            if (page != null)
            {
                page.InvokeFinishButtonClick(ce);
                if (ce.Cancel)
                    return;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (this.DesignMode && this.WizardPages.Count == 0)
            {
                Rectangle r = this.ClientRectangle;
                Graphics g = e.Graphics;
                r.Inflate(6, 6);
                TextDrawing.DrawString(g, "Right-click control and use context menu commands to create, delete, navigate and re-order wizard pages. You can use Next and Back button to navigate wizard just like in run-time.",
                    this.Font, SystemColors.ControlDarkDark, r,
                    eTextFormat.HorizontalCenter | eTextFormat.VerticalCenter | eTextFormat.WordBreak);
            }
            base.OnPaint(e);
        }

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			if(this.DesignMode)
				this.Invalidate();
		}

        private void LayoutHeader()
        {
            int margin = 4;
            int titleWidth = this.Width - labelCaption.Left - margin;
            int descWidth = this.Width - labelDescription.Left - margin;

            if (m_HeaderImageVisible || this.DesignMode)
            {
                if (_HeaderImageAlignment == eWizardTitleImageAlignment.Right)
                {
                    pictureHeader.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
                    pictureHeader.Location = new Point(this.Width - pictureHeader.Width - margin,
                        (panelHeader.Height - pictureHeader.Height) / 2);
                }
                else
                {
                    pictureHeader.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
                    pictureHeader.Location = new Point(margin,
                        (panelHeader.Height - pictureHeader.Height) / 2);
                }
                titleWidth -= (pictureHeader.Width + margin);
                descWidth -= (pictureHeader.Width + margin);
            }

            labelCaption.Width = titleWidth;
            labelDescription.Width = descWidth;
            labelCaption.Top = 5;
            labelCaption.Height = labelCaption.Font.Height + 4;

            if (_HeaderImageAlignment == eWizardTitleImageAlignment.Right)
            {
                labelCaption.Left = 16;
            }
            else
            {
                labelCaption.Left = pictureHeader.Bounds.Right + 10;
                if (!_HeaderDescriptionVisible)
                    labelCaption.Top = (panelHeader.Height - labelCaption.Height) / 2;
            }
            labelDescription.Left = labelCaption.Left;

            if (this.IsRightToLeft)
            {
                pictureHeader.Left = this.Width - pictureHeader.Bounds.Right;
                labelCaption.Left = this.Width - labelCaption.Bounds.Right;
                labelDescription.Left = this.Width - labelDescription.Bounds.Right;
            }
        }

        /// <summary>
        /// Returns reference to internal Next button control.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ButtonX NextButtonControl
        {
            get { return buttonNext; }
        }

        /// <summary>
        /// Returns reference to internal Back button control.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ButtonX BackButtonControl
        {
            get { return buttonBack; }
        }

        private bool IsRightToLeft
        {
            get
            {
                return this.RightToLeft == RightToLeft.Yes;
            }
        }

        protected override void OnRightToLeftChanged(EventArgs e)
        {
            this.RepositionButtons();
            this.LayoutHeader();
            base.OnRightToLeftChanged(e);
        }

        private void RepositionButtons()
        {
            int x=panelFooter.Width-m_ButtonSpacingMajor;
            int y=(panelFooter.Height-m_ButtonHeight)/2;

            WizardButtonsLayoutEventArgs e=new WizardButtonsLayoutEventArgs();

            if (buttonHelp.Visible)
            {
                x -= buttonHelp.Width;
                e.HelpButtonBounds = new Rectangle(x, y, buttonHelp.Width, m_ButtonHeight);
                x -= m_ButtonSpacingMajor;
            }

            if (buttonCancel.Visible)
            {
                x -= buttonCancel.Width;
                e.CancelButtonBounds = new Rectangle(x, y, buttonCancel.Width, m_ButtonHeight);
                x -= m_ButtonSpacingMajor;
            }

            if (buttonFinish.Visible)
            {
                x -= buttonFinish.Width;
                e.FinishButtonBounds = new Rectangle(x, y, buttonFinish.Width, m_ButtonHeight);
                if(m_FinishButtonAlwaysVisible)
                    x -= m_ButtonSpacingMajor;
                else
                    x -= m_ButtonSpacingMinor;
            }

            if (buttonNext.Visible)
            {
                x -= buttonNext.Width;
                e.NextButtonBounds = new Rectangle(x, y, buttonNext.Width, m_ButtonHeight);
                x -= m_ButtonSpacingMinor;
            }

            if (buttonBack.Visible)
            {
                x -= buttonBack.Width;
                e.BackButtonBounds = new Rectangle(x, y, buttonBack.Width, m_ButtonHeight);
                x -= m_ButtonSpacingMinor;
            }

            if (IsRightToLeft)
            {
                int xOffset = panelFooter.Width-x;
                if (!e.HelpButtonBounds.IsEmpty)
                {
                    Rectangle r = e.HelpButtonBounds;
                    e.HelpButtonBounds = new Rectangle(panelFooter.Width - r.Right, r.Y, r.Width, r.Height);
                }
                if (!e.CancelButtonBounds.IsEmpty)
                {
                    Rectangle r = e.CancelButtonBounds;
                    e.CancelButtonBounds = new Rectangle(panelFooter.Width - r.Right, r.Y, r.Width, r.Height);
                }
                if (!e.FinishButtonBounds.IsEmpty)
                {
                    Rectangle r = e.FinishButtonBounds;
                    e.FinishButtonBounds = new Rectangle(panelFooter.Width - r.Right, r.Y, r.Width, r.Height);
                }
                if (!e.NextButtonBounds.IsEmpty)
                {
                    Rectangle r = e.NextButtonBounds;
                    e.NextButtonBounds = new Rectangle(panelFooter.Width - r.Right, r.Y, r.Width, r.Height);
                }
                if (!e.BackButtonBounds.IsEmpty)
                {
                    Rectangle r = e.BackButtonBounds;
                    e.BackButtonBounds = new Rectangle(panelFooter.Width - r.Right, r.Y, r.Width, r.Height);
                }
            }

            OnLayoutWizardButtons(e);

            if (buttonHelp.Visible)
                buttonHelp.Bounds = e.HelpButtonBounds;

            if (buttonCancel.Visible)
                buttonCancel.Bounds = e.CancelButtonBounds;

            if (buttonNext.Visible)
                buttonNext.Bounds = e.NextButtonBounds;

            if (buttonFinish.Visible)
                buttonFinish.Bounds = e.FinishButtonBounds;

            if (buttonBack.Visible)
                buttonBack.Bounds = e.BackButtonBounds;
        }

        private void OnFinishButtonAlwaysVisibleChanged()
        {
        }

        private void panelFooter_Resize(object sender, EventArgs e)
        {
            RepositionButtons();
        }

        private void CommandButtonVisibleChanged(object sender, EventArgs e)
        {
            RepositionButtons();
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            NavigateBack();
        }
        [EditorBrowsable(EditorBrowsableState.Never)]
        public WizardPage GetBackPage()
        {
            WizardPage page = null;

            if (m_PagesHistory.Count > 0)
            {
                page = m_PagesHistory.Pop() as WizardPage;
            }
            else
            {
                if (this.SelectedPageIndex > 0)
                {
                    page = m_WizardPages[this.SelectedPageIndex - 1];
                }
            }

            return page;
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            NavigateNext();
        }
        [EditorBrowsable(EditorBrowsableState.Never)]
        public WizardPage GetNextPage()
        {
            if (this.SelectedPageIndex + 1 < m_WizardPages.Count)
                return m_WizardPages[this.SelectedPageIndex + 1];
            return null;
        }

        private void buttonFinish_Click(object sender, EventArgs e)
        {
            NavigateFinish();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            NavigateCancel();
        }

        private void buttonHelp_Click(object sender, EventArgs e)
        {
            CancelEventArgs ce = new CancelEventArgs();
            OnHelpButtonClick(ce);
            if (ce.Cancel)
                return;

            WizardPage page = this.SelectedPage;
            if (page != null)
            {
                page.InvokeHelpButtonClick(ce);
                if (ce.Cancel)
                    return;
            }
        }

        internal void OnWizardPageAdded(WizardPage page)
        {
            SetupPage(page);
            if (m_WizardPages.IndexOf(page) == m_SelectedPageIndex)
                ShowPage(page, eWizardPageChangeSource.Code);
            else
                page.Visible = false;
                
            this.Controls.Add(page);

            SetupButtons(true);
        }

        internal void OnWizardPageRemoved(WizardPage page)
        {
            this.Controls.Remove(page);
            if (page.Visible && this.SelectedPage!=null && !this.SelectedPage.Visible)
            {
                ShowPage(this.SelectedPage, eWizardPageChangeSource.Code);
            }
        }

        private bool ShowPage(WizardPage page, eWizardPageChangeSource changeSource)
        {
            WizardPage oldPage = this.SelectedPage;

            WizardCancelPageChangeEventArgs e = new WizardCancelPageChangeEventArgs(page, oldPage, changeSource);
            OnWizardPageChanging(e);
            if (e.Cancel || e.NewPage==null)
                return false;

            page = e.NewPage;

            if (page != null)
            {
                page.InvokeBeforePageDisplayed(e);
                if (e.Cancel || e.NewPage == null)
                    return false;
                page = e.NewPage;
            }

            if (m_PageChangeDisableButtons)
            {
                buttonBack.Enabled = false;
                buttonNext.Enabled = false;
                buttonFinish.Enabled = false;
                buttonCancel.Enabled = false;
            }

            if(m_PageChangeWaitCursor)
                this.Cursor = Cursors.WaitCursor;

            // Change the page
            if (oldPage != null)
            {
                if (oldPage != page)
                {
                    if (this.DesignMode)
                        TypeDescriptor.GetProperties(oldPage)["Visible"].SetValue(oldPage, false);
                    oldPage.Visible = false;
                    oldPage.InvokeAfterPageHidden(new WizardPageChangeEventArgs(page, oldPage, changeSource));
                }
                else
                {
                    foreach (WizardPage wp in this.WizardPages)
                    {
                        if (wp != page)
                        {
                            if (this.DesignMode)
                                TypeDescriptor.GetProperties(wp)["Visible"].SetValue(wp, false);
                            wp.Visible = false;
                        }
                    }
                }
            }

            if (page != null)
            {
                if (this.DesignMode)
                    TypeDescriptor.GetProperties(page)["Visible"].SetValue(page, true);

                page.Visible = true;
            }

            m_SelectedPageIndex = m_WizardPages.IndexOf(page);

            SetupButtons(true);

            // Raise event
            e.Cancel = false;
            OnWizardPageChanged(e);

            if (page != null)
            {
                page.InvokeAfterPageDisplayed(new WizardPageChangeEventArgs(page, oldPage, changeSource));
            }

            UpdatePageDisplay();

            if (m_PageChangeWaitCursor)
                this.Cursor = Cursors.Default;

            return true;
        }

        internal void UpdatePageDisplay()
        {
            WizardPage page = this.SelectedPage;
            if (page == null)
                return;

            if (page.InteriorPage)
            {
                if (!panelHeader.Visible)
                    panelHeader.Visible = true;
				if(pictureHeader.Visible != m_HeaderImageVisible)
				{
					pictureHeader.Visible = m_HeaderImageVisible;
					LayoutHeader();
				}
            }
            else
            {
                if (panelHeader.Visible)
                    panelHeader.Visible = false;
            }

            labelCaption.Text = page.PageTitle;
            labelDescription.Text = page.PageDescription;

            if (page.PageHeaderImage != null)
            {
                pictureHeader.BackgroundImage = page.PageHeaderImage;
                m_HeaderImageChanged = true;
            }
            else if (m_HeaderImageChanged)
            {
                if (m_HeaderImage != null)
                    pictureHeader.BackgroundImage = m_HeaderImage;
                else
                    pictureHeader.BackgroundImage = BarFunctions.LoadBitmap("SystemImages.WizardHeaderImage.png");

                m_HeaderImageChanged = false;
            }

            if (page.FormCaption != "")
            {
                Form f = this.FindForm();
                if (f != null)
                    f.Text = page.FormCaption;
            }

#if !TRIAL
			if (NativeFunctions.keyValidated2 != 266)
				labelCaption.Text="Invalid DotNetBar License";
#endif
        }

        internal void SetupButtons(bool canSetFocus)
        {
            eWizardButtonState backButtonEnabled = eWizardButtonState.Auto;
            eWizardButtonState backButtonVisible = eWizardButtonState.Auto;
            eWizardButtonState nextButtonEnabled = eWizardButtonState.Auto;
            eWizardButtonState nextButtonVisible = eWizardButtonState.Auto;
            eWizardButtonState finishButtonEnabled = eWizardButtonState.Auto;
            eWizardButtonState cancelButtonEnabled = eWizardButtonState.Auto;
            eWizardButtonState cancelButtonVisible = eWizardButtonState.Auto;
            eWizardButtonState helpButtonEnabled = eWizardButtonState.Auto;
            eWizardButtonState helpButtonVisible = eWizardButtonState.Auto;

            WizardPage page = this.SelectedPage;
            if (page != null)
            {
                backButtonEnabled = page.BackButtonEnabled;
                backButtonVisible = page.BackButtonVisible;
                nextButtonEnabled = page.NextButtonEnabled;
                nextButtonVisible = page.NextButtonVisible;
                finishButtonEnabled = page.FinishButtonEnabled;
                cancelButtonEnabled = page.CancelButtonEnabled;
                cancelButtonVisible = page.CancelButtonVisible;
                helpButtonEnabled = page.HelpButtonEnabled;
                helpButtonVisible = page.HelpButtonVisible;
            }

            if (!m_HelpButtonVisible)
                helpButtonVisible = eWizardButtonState.False;

            if (this.WizardPages.Count == 0 && this.DesignMode)
            {
                backButtonEnabled = eWizardButtonState.False;
                nextButtonEnabled = eWizardButtonState.False;
                finishButtonEnabled = eWizardButtonState.False;
                cancelButtonEnabled = eWizardButtonState.False;
            }

            // Help button
            if (helpButtonVisible == eWizardButtonState.False)
                buttonHelp.Visible = false;
            else if(!buttonHelp.Visible)
                buttonHelp.Visible = true;
            if (helpButtonEnabled == eWizardButtonState.False)
                buttonHelp.Enabled = false;
            else if(!buttonHelp.Enabled)
                buttonHelp.Enabled = true;

            // Cancel button
            if (cancelButtonVisible == eWizardButtonState.False)
                buttonCancel.Visible = false;
            else if (!buttonCancel.Visible)
                buttonCancel.Visible = true;
            if (cancelButtonEnabled == eWizardButtonState.False)
                buttonCancel.Enabled = false;
            else if(!buttonCancel.Enabled)
                buttonCancel.Enabled = true;
            

            // Finish button
            if (m_FinishButtonAlwaysVisible ||
                this.SelectedPageIndex == m_WizardPages.Count - 1 || finishButtonEnabled == eWizardButtonState.True)
            {
                if (!buttonFinish.Visible)
                    buttonFinish.Visible = true;
            }
            else
                buttonFinish.Visible = false;

            if (finishButtonEnabled == eWizardButtonState.False ||
                finishButtonEnabled == eWizardButtonState.Auto && this.SelectedPageIndex < m_WizardPages.Count - 1)
            {
                buttonFinish.Enabled = false;
            }
            else if (!buttonFinish.Enabled)
                buttonFinish.Enabled = true;

            // Next button
            if (nextButtonVisible == eWizardButtonState.True)
            {
                if (!buttonNext.Visible)
                    buttonNext.Visible = true;
            }
            else if (nextButtonVisible == eWizardButtonState.Auto)
            {
                if (buttonFinish.Visible && buttonFinish.Enabled && !m_FinishButtonAlwaysVisible)
                    buttonNext.Visible = false;
                else if (!buttonNext.Visible)
                    buttonNext.Visible = true;
            }
            else if (buttonNext.Visible)
                buttonNext.Visible = false;

            if (nextButtonEnabled == eWizardButtonState.True)
            {
                if (!buttonNext.Enabled)
                    buttonNext.Enabled = true;
            }
            else if (nextButtonEnabled == eWizardButtonState.Auto)
            {
                if (buttonFinish.Visible && buttonFinish.Enabled || this.SelectedPageIndex == m_WizardPages.Count - 1)
                    buttonNext.Enabled = false;
                else if (!buttonNext.Enabled)
                    buttonNext.Enabled = true;
            }
            else if (buttonNext.Enabled)
                buttonNext.Enabled = false;

            if (canSetFocus)
            {
                if (buttonNext.Enabled && buttonNext.Visible)
                    buttonNext.Select();
                else if (buttonFinish.Enabled && buttonFinish.Visible)
                    buttonFinish.Select();
            }

            if (backButtonVisible == eWizardButtonState.True || backButtonVisible == eWizardButtonState.Auto)
            {
                if (!buttonBack.Visible)
                    buttonBack.Visible = true;
            }
            else if (buttonBack.Visible)
                buttonBack.Visible = false;

            if (backButtonEnabled == eWizardButtonState.True)
            {
                if (!buttonBack.Enabled)
                    buttonBack.Enabled = true;
            }
            else if (backButtonEnabled == eWizardButtonState.Auto)
            {
                if (m_PagesHistory.Count > 0 || this.SelectedPageIndex > 0)
                {
                    if (!buttonBack.Enabled)
                        buttonBack.Enabled = true;
                }
                else if (buttonBack.Enabled)
                    buttonBack.Enabled = false;
            }
            else if (backButtonEnabled == eWizardButtonState.False)
            {
                if (buttonBack.Enabled)
                    buttonBack.Enabled = false;
            }

            SetFormAcceptButton();
            SetFormCancelButton();

            RepositionButtons();
        }

        private void SetFormAcceptButton()
        {
            Form form = this.FindForm();
            if (form == null)
                return;

            if (m_FormAcceptButton == eWizardFormAcceptButton.FinishAndNext)
            {
                if (buttonFinish.Visible && buttonFinish.Enabled)
                {
                    if(form.AcceptButton != buttonFinish)
                        form.AcceptButton = buttonFinish;
                }
                else
                {
                    if (form.AcceptButton != buttonNext)
                        form.AcceptButton = buttonNext;
                }
            }
            else if (m_FormAcceptButton == eWizardFormAcceptButton.Next)
            {
                if (form.AcceptButton != buttonNext)
                    form.AcceptButton = buttonNext;
            }
            else if (m_FormAcceptButton == eWizardFormAcceptButton.Finish)
            {
                if (form.AcceptButton != buttonFinish)
                    form.AcceptButton = buttonFinish;
            }
            else if (m_FormAcceptButton == eWizardFormAcceptButton.None)
            {
                if (form.AcceptButton == buttonFinish || form.AcceptButton == buttonNext)
                    form.AcceptButton = null;
            }
            
        }

        private void SetFormCancelButton()
        {
            Form form = this.FindForm();
            if (form == null)
                return;

            if (m_FormCancelButton == eWizardFormCancelButton.Cancel)
            {
                if(form.CancelButton != buttonCancel)
                    form.CancelButton = buttonCancel;
            }
            else if (form.CancelButton == buttonCancel)
                form.CancelButton = null;
        }

        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);

            SetFormCancelButton();
            SetFormAcceptButton();
        }

        internal void SetupPage(WizardPage page)
        {
            if (page == null)
                return;

            Size innerPadding = new Size(7, 12);

            this.SuspendLayout();

            try
            {
                Rectangle bounds = Rectangle.Empty;
                if (page.InteriorPage)
                {
                    bounds = new Rectangle(this.ClientRectangle.X + innerPadding.Width,
                         this.ClientRectangle.Y + panelHeader.Height + innerPadding.Height,
                         this.ClientRectangle.Width - innerPadding.Width * 2,
                         this.ClientRectangle.Height - (panelHeader.Height + panelFooter.Height + innerPadding.Height * 2));
                }
                else
                {
                    bounds = new Rectangle(this.ClientRectangle.X,
                         this.ClientRectangle.Y,
                         this.ClientRectangle.Width,
                         this.ClientRectangle.Height - panelFooter.Height);
                }

                TypeDescriptor.GetProperties(page)["Bounds"].SetValue(page, bounds);

                TypeDescriptor.GetProperties(page)["Anchor"].SetValue(page, AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom);
            }
            finally
            {
                this.ResumeLayout();
            }
        }

        private bool _ButtonFocusCuesEnabled = true;
        /// <summary>
        /// Gets or sets whether Focus cues on wizard navigation buttons are enabled. Default value is true.
        /// </summary>
        [DefaultValue(true), Category("Behavior"), Description("Indicates whether Focus cues on wizard navigation buttons are enabled.")]
        public bool ButtonFocusCuesEnabled
        {
            get { return _ButtonFocusCuesEnabled; }
            set
            {
                if (_ButtonFocusCuesEnabled != value)
                {
                    _ButtonFocusCuesEnabled = value;
                    buttonHelp.FocusCuesEnabled = value;
                    buttonCancel.FocusCuesEnabled = value;
                    buttonFinish.FocusCuesEnabled = value;
                    buttonNext.FocusCuesEnabled = value;
                    buttonBack.FocusCuesEnabled = value;
                }
            }
        }
        

        private eWizardStyle m_WizardStyle = eWizardStyle.Default;
        
        /// <summary>
        /// Gets or sets the visual style used for wizard buttons.
        /// </summary>
        [Browsable(false), DefaultValue(eWizardStyle.Default)]
        public eWizardStyle ButtonStyle
        {
            get { return m_WizardStyle; }
            set
            {
                if (m_WizardStyle != value)
                {
                    m_WizardStyle = value;
                    OnWizardStyleChanged();
                }
            }
        }

        private void OnWizardStyleChanged()
        {
            if (m_WizardStyle == eWizardStyle.Default)
            {
                buttonBack.Style = eDotNetBarStyle.Office2000;
                buttonCancel.Style = eDotNetBarStyle.Office2000;
                buttonFinish.Style = eDotNetBarStyle.Office2000;
                buttonHelp.Style = eDotNetBarStyle.Office2000;
                buttonNext.Style = eDotNetBarStyle.Office2000;
                buttonBack.ThemeAware = true;
                buttonCancel.ThemeAware = true;
                buttonFinish.ThemeAware = true;
                buttonHelp.ThemeAware = true;
                buttonNext.ThemeAware = true;
            }
            else
            {
                buttonBack.Style = eDotNetBarStyle.Office2007;
                buttonCancel.Style = eDotNetBarStyle.Office2007;
                buttonFinish.Style = eDotNetBarStyle.Office2007;
                buttonHelp.Style = eDotNetBarStyle.Office2007;
                buttonNext.Style = eDotNetBarStyle.Office2007;

                buttonBack.ThemeAware = false;
                buttonCancel.ThemeAware = false;
                buttonFinish.ThemeAware = false;
                buttonHelp.ThemeAware = false;
                buttonNext.ThemeAware = false;
            }
        }

        #endregion

        #region Event code
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
        /// Raises NextButtonClick event.
        /// </summary>
        /// <param name="e">Event arguments</param>
        protected virtual void OnNextButtonClick(CancelEventArgs e)
        {
            if (NextButtonClick != null)
                NextButtonClick(this, e);
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
        /// Raises CancelButtonClick event.
        /// </summary>
        /// <param name="e">Event arguments</param>
        protected virtual void OnCancelButtonClick(CancelEventArgs e)
        {
            if (CancelButtonClick != null)
                CancelButtonClick(this, e);
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
        /// Raises WizardPageChanging event.
        /// </summary>
        /// <param name="e">Provides event arguments</param>
        protected virtual void OnWizardPageChanging(WizardCancelPageChangeEventArgs e)
        {
            if (WizardPageChanging != null)
                WizardPageChanging(this, e);
        }

        /// <summary>
        /// Raises WizardPageChanged event.
        /// </summary>
        /// <param name="e">Provides event arguments</param>
        protected virtual void OnWizardPageChanged(WizardCancelPageChangeEventArgs e)
        {
            if (WizardPageChanged != null)
                WizardPageChanged(this, e);
        }

        protected virtual void OnLayoutWizardButtons(WizardButtonsLayoutEventArgs e)
        {
            if (LayoutWizardButtons != null)
                LayoutWizardButtons(this, e);
        }
        #endregion

		#region Licensing
#if !TRIAL
		private string m_LicenseKey = "";
		[Browsable(false), DefaultValue("")]
		public string LicenseKey
		{
			get { return m_LicenseKey; }
			set
			{
				if (NativeFunctions.ValidateLicenseKey(value))
					return;
				m_LicenseKey = (!NativeFunctions.CheckLicenseKey(value) ? "9dsjkhds7" : value);
			}
		}
#else
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);

			RemindForm frm=new RemindForm();
			frm.ShowDialog();
			frm.Dispose();
		}
#endif
        #endregion
    }
}
