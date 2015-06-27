#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar.Controls
{
    /// <summary>
    /// Represents non-intrusive Warning Box control with Options and Close button.
    /// </summary>
    [ToolboxBitmap(typeof(WarningBox), "Controls.WarningBox.ico"), ToolboxItem(true), DefaultEvent("OptionsClick"), System.Runtime.InteropServices.ComVisible(false), Designer("DevComponents.DotNetBar.Design.WarningBoxDesigner, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
    public partial class WarningBox : UserControl
    {
        #region Events
        /// <summary>
        /// Occurs when Close button is clicked.
        /// </summary>
        [Description("Occurs when Close button is clicked.")]
        public event EventHandler CloseClick;
        /// <summary>
        /// Occurs when Options button is clicked.
        /// </summary>
        [Description("Occurs when Options button is clicked.")]
        public event EventHandler OptionsClick;
        /// <summary>
        /// Occurs when warning text markup link is clicked. Markup links can be created using "a" tag, for example:
        /// <a name="MyLink">Markup link</a>
        /// </summary>
        public event MarkupLinkClickEventHandler MarkupLinkClick;
        #endregion

        #region Constructors
        public WarningBox()
        {
            InitializeComponent();
            CloseButton.Image = BarFunctions.LoadBitmap("SystemImages.CloseButton.png");
            StyleManager.Register(this);
        }
        #endregion

        #region Internal Implementation
        /// <summary>
        /// Called by StyleManager to notify control that style on manager has changed and that control should refresh its appearance if
        /// its style is controlled by StyleManager.
        /// </summary>
        /// <param name="newStyle">New active style.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void StyleManagerStyleChanged(eDotNetBarStyle newStyle)
        {
            UpdateColorScheme();
        }

        /// <summary>
        /// Raises the CloseClick event.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnCloseClick(EventArgs e)
        {
            EventHandler handler = CloseClick;
            if (handler != null) handler(this, e);
        }

        /// <summary>
        /// Raises the OptionsClick event.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnOptionsClick(EventArgs e)
        {
            EventHandler handler = OptionsClick;
            if (handler != null) handler(this, e);
        }

        private void OptionsButton_Click(object sender, EventArgs e)
        {
            OnOptionsClick(e);
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            OnCloseClick(e);
        }

        /// <summary>
        /// Gets or sets the text displayed on close button tooltip.
        /// </summary>
        [Browsable(true), Category("Warning"), Localizable(true), DefaultValue("Close")]
        public string CloseButtonTooltip
        {
            get
            {
                return CloseButton.Tooltip;
            }
            set
            {
                CloseButton.Tooltip = value;
            }
        }

        /// <summary>
        /// Gets or sets the text displayed on warning control label. Supports text-markup.
        /// </summary>
        [Browsable(true), Editor("DevComponents.DotNetBar.Design.TextMarkupUIEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(System.Drawing.Design.UITypeEditor)), Category("Warning"), Localizable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), DefaultValue("")]
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
            }
        }

        protected override void OnTextChanged(EventArgs e)
        {
            WarningLabel.Text = this.Text;
            base.OnTextChanged(e);
        }

        /// <summary>
        /// Gets or sets whether text is wrapped on multiple lines if it cannot fit the space allocated to the control.
        /// </summary>
        [DefaultValue(false), Category("Appearance"), Description("Indicates whether text is wrapped on multiple lines if it cannot fit the space allocated to the control.")]
        public bool WordWrap
        {
            get { return WarningLabel.WordWrap; }
            set
            {
                WarningLabel.WordWrap = value;
            }
        }


        /// <summary>
        /// Gets or sets the image displayed next to the warning label text. Default value is null.
        /// </summary>
        [Browsable(true), Category("Warning"), DefaultValue(null), Localizable(true)]
        public Image Image
        {
            get { return WarningLabel.Image; }
            set
            {
                WarningLabel.Image = value;
            }
        }
        /// <summary>
        /// Gets or sets the text for the Options buttons.
        /// </summary>
        [Browsable(true), Category("Warning"), DefaultValue("Options..."), Localizable(true)]
        public string OptionsText
        {
            get { return OptionsButton.Text; }
            set
            {
                OptionsButton.Text = value;
            }
        }

        private bool _OptionsButtonVisible = true;
        /// <summary>
        /// Gets or sets whether Options button is visible. Default value is true.
        /// </summary>
        [Browsable(true), Category("Warning"), DefaultValue(true)]
        public bool OptionsButtonVisible
        {
            get { return _OptionsButtonVisible; }
            set
            {
                if (_OptionsButtonVisible != value)
                {
                    _OptionsButtonVisible = value;
                    OptionsButton.Visible = _OptionsButtonVisible;
                    OnOptionsButtonVisibleChanged();
                }
            }
        }

        private void OnOptionsButtonVisibleChanged()
        {
            if (_OptionsButtonVisible)
            {
                WarningLabel.Width = PanelWarning.Width - (OptionsButton.Width + 3 + PanelWarning.Padding.Horizontal);
            }
            else
            {
                WarningLabel.Width = PanelWarning.Width - (PanelWarning.Padding.Right + WarningLabel.Left);
            }
        }

        private bool _CloseButtonVisible = true;
        /// <summary>
        /// Gets or sets whether Close button is visible. Default value is true.
        /// </summary>
        [Browsable(true), Category("Warning"), DefaultValue(true)]
        public bool CloseButtonVisible
        {
            get { return _CloseButtonVisible; }
            set
            {
                if (_CloseButtonVisible != value)
                {
                    _CloseButtonVisible = value;
                    PanelClose.Visible = _CloseButtonVisible;
                }
            }
        }

        /// <summary>
        /// Updates control color scheme based on currently selected Office 2007 Color Table. Usually it is not necessary to
        /// call this method manually. You need to call it to update the colors on control if you customize the Office2007ColorTable.WarningBox values.
        /// </summary>
        public void UpdateColorScheme()
        {
            Office2007Renderer renderer = (Office2007Renderer)GlobalManager.Renderer;
            Office2007WarningBoxColorTable colorTable = null;
            if (renderer == null)
                colorTable = new Office2007WarningBoxColorTable(); // Default blue
            else
                colorTable = renderer.ColorTable.WarningBox;

            if (_ColorScheme == eWarningBoxColorScheme.Default)
            {
                this.BackColor = colorTable.BackColor;

                this.PanelWarning.Style.BorderColor.Color = colorTable.WarningBorderColor;
                this.PanelWarning.Style.BackColor1.Color = colorTable.WarningBackColor1;
                this.PanelWarning.Style.BackColor2.Color = colorTable.WarningBackColor2;

                this.OptionsButton.Style.BackColor1.ColorSchemePart = eColorSchemePart.Custom;
                this.OptionsButton.Style.BackColor1.Color = colorTable.WarningBackColor1;
                this.OptionsButton.Style.BackColor2.ColorSchemePart = eColorSchemePart.Custom;
                this.OptionsButton.Style.BackColor2.Color = colorTable.WarningBackColor2;
                this.OptionsButton.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.Custom;
                this.OptionsButton.Style.BorderColor.Color = colorTable.WarningBorderColor;
            }
            else if (_ColorScheme == eWarningBoxColorScheme.Green)
            {
                this.BackColor = colorTable.GreenBackColor;

                this.PanelWarning.Style.BorderColor.Color = colorTable.GreenWarningBorderColor;
                this.PanelWarning.Style.BackColor1.Color = colorTable.GreenWarningBackColor1;
                this.PanelWarning.Style.BackColor2.Color = colorTable.GreenWarningBackColor2;

                this.OptionsButton.Style.BackColor1.ColorSchemePart = eColorSchemePart.Custom;
                this.OptionsButton.Style.BackColor1.Color = colorTable.GreenWarningBackColor1;
                this.OptionsButton.Style.BackColor2.ColorSchemePart = eColorSchemePart.Custom;
                this.OptionsButton.Style.BackColor2.Color = colorTable.GreenWarningBackColor2;
                this.OptionsButton.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.Custom;
                this.OptionsButton.Style.BorderColor.Color = colorTable.GreenWarningBorderColor;
            }
            else if (_ColorScheme == eWarningBoxColorScheme.Yellow)
            {
                this.BackColor = colorTable.YellowBackColor;

                this.PanelWarning.Style.BorderColor.Color = colorTable.YellowWarningBorderColor;
                this.PanelWarning.Style.BackColor1.Color = colorTable.YellowWarningBackColor1;
                this.PanelWarning.Style.BackColor2.Color = colorTable.YellowWarningBackColor2;

                this.OptionsButton.Style.BackColor1.ColorSchemePart = eColorSchemePart.Custom;
                this.OptionsButton.Style.BackColor1.Color = colorTable.YellowWarningBackColor1;
                this.OptionsButton.Style.BackColor2.ColorSchemePart = eColorSchemePart.Custom;
                this.OptionsButton.Style.BackColor2.Color = colorTable.YellowWarningBackColor2;
                this.OptionsButton.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.Custom;
                this.OptionsButton.Style.BorderColor.Color = colorTable.YellowWarningBorderColor;
            }
            else if (_ColorScheme == eWarningBoxColorScheme.Red)
            {
                this.BackColor = colorTable.RedBackColor;

                this.PanelWarning.Style.BorderColor.Color = colorTable.RedWarningBorderColor;
                this.PanelWarning.Style.BackColor1.Color = colorTable.RedWarningBackColor1;
                this.PanelWarning.Style.BackColor2.Color = colorTable.RedWarningBackColor2;

                this.OptionsButton.Style.BackColor1.ColorSchemePart = eColorSchemePart.Custom;
                this.OptionsButton.Style.BackColor1.Color = colorTable.RedWarningBackColor1;
                this.OptionsButton.Style.BackColor2.ColorSchemePart = eColorSchemePart.Custom;
                this.OptionsButton.Style.BackColor2.Color = colorTable.RedWarningBackColor2;
                this.OptionsButton.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.Custom;
                this.OptionsButton.Style.BorderColor.Color = colorTable.RedWarningBorderColor;
            }

            this.Invalidate(true);
        }

        private void WarningLabel_MarkupLinkClick(object sender, MarkupLinkClickEventArgs e)
        {
            OnMarkupLinkClick(e);
        }

        /// <summary>
        /// Invokes the MarkupLinkClick event.
        /// </summary>
        /// <param name="e">Provides additional data about event.</param>
        protected virtual void OnMarkupLinkClick(MarkupLinkClickEventArgs e)
        {
            if (MarkupLinkClick != null)
                MarkupLinkClick(this, e);
        }

        private int _AutoCloseTimeout = 0;
        /// <summary>
        /// Gets or sets the timeout in seconds after which the control automatically closes itself. Default value is 0 which indicates that auto-close
        /// is disabled.
        /// </summary>
        [DefaultValue(0), Category("Behavior"), Description("Indicates timeout in seconds after which the control automatically closes itself.")]
        public int AutoCloseTimeout
        {
            get { return _AutoCloseTimeout; }
            set
            {
                if (value < 0) value = 0;
                _AutoCloseTimeout = value;
                OnAutoCloseTimeoutChanged();
            }
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            DestroyAutoCloseTimer();
            base.OnHandleDestroyed(e);
        }

        private void OnAutoCloseTimeoutChanged()
        {
            if (_AutoCloseTimeout == 0)
                DestroyAutoCloseTimer();
            else if (this.Visible)
                SetupAutoCloseTimer();
        }

        private Timer _AutoCloseTimer = null;
        private void SetupAutoCloseTimer()
        {
            if (_AutoCloseTimeout == 0 || _AutoCloseTimer != null || this.DesignMode) return;

            _AutoCloseTimer = new Timer();
            _AutoCloseTimer.Interval = _AutoCloseTimeout * 1000;
            _AutoCloseTimer.Tick += new EventHandler(AutoCloseTimerTick);
            _AutoCloseTimer.Start();
        }

        void AutoCloseTimerTick(object sender, EventArgs e)
        {
            _AutoCloseTimer.Stop();
            DestroyAutoCloseTimer();
            AutoClose();
        }

        private void AutoClose()
        {
            OnCloseClick(EventArgs.Empty);
            this.Visible = false;
        }

        private void DestroyAutoCloseTimer()
        {
            Timer timer = _AutoCloseTimer;
            _AutoCloseTimer = null;
            if (timer == null) return;
            timer.Stop();
            timer.Dispose();
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            if (this.Visible)
            {
                if (_AutoCloseTimeout > 0) SetupAutoCloseTimer();
            }
            else
                DestroyAutoCloseTimer();

            base.OnVisibleChanged(e);
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            if (_ColorScheme == eWarningBoxColorScheme.Default)
                UpdateColorScheme();
            base.OnHandleCreated(e);
        }

        private eWarningBoxColorScheme _ColorScheme = eWarningBoxColorScheme.Default;
        /// <summary>
        /// Gets or sets the control's color scheme.
        /// </summary>
        [DefaultValue(eWarningBoxColorScheme.Default), Category("Appearance"), Description("Indicates control's color scheme.")]
        public eWarningBoxColorScheme ColorScheme
        {
            get { return _ColorScheme; }
            set
            {
                _ColorScheme = value;
                UpdateColorScheme();
                this.Invalidate(true);
            }
        }

        private bool _AntiAlias = true;
        /// <summary>
        /// Gets or sets whether anti-alias smoothing is used while painting. Default value is true.
        /// </summary>
        [DefaultValue(true), Category("Appearance")]
        public bool AntiAlias
        {
            get { return _AntiAlias; }
            set
            {
                if (value != _AntiAlias)
                {
                    bool oldValue = _AntiAlias;
                    _AntiAlias = value;
                    OnAntiAliasChanged(oldValue, value);
                }
            }
        }
        /// <summary>
        /// Called when AntiAlias property has changed.
        /// </summary>
        /// <param name="oldValue">Old property value</param>
        /// <param name="newValue">New property value</param>
        protected virtual void OnAntiAliasChanged(bool oldValue, bool newValue)
        {
            //OnPropertyChanged(new PropertyChangedEventArgs("AntiAlias"));
            PanelWarning.AntiAlias = newValue;
            CloseButton.AntiAlias = newValue;
            OptionsButton.AntiAlias = newValue;
            WarningLabel.AntiAlias = newValue;
            this.Invalidate(true);
        }
        #endregion
    }
    /// <summary>
    /// Defines available WarningBox control color schemes.
    /// </summary>
    public enum eWarningBoxColorScheme
    {
        Default,
        Green,
        Yellow,
        Red
    }
}
#endif