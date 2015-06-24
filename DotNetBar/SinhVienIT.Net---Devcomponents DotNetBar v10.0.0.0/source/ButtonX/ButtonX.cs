using System;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using System.Collections;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Drawing.Design;

namespace DevComponents.DotNetBar
{
    [ToolboxBitmap(typeof(ButtonX),"ButtonX.ButtonX.ico"),ToolboxItem(true), DefaultEvent("Click"), Designer("DevComponents.DotNetBar.Design.ButtonXDesigner, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf"), System.Runtime.InteropServices.ComVisible(false)]
    public class ButtonX : PopupItemControl, IButtonControl, ICommandSource
    {
        #region Events
        /// <summary>
        /// Occurs when Checked property has changed.
        /// </summary>
        [Description("Occurs when Checked property has changed.")]
        public event EventHandler CheckedChanged;
        #endregion

        #region Private Variables
        private ButtonItem m_Button = null;
        private ColorScheme m_ColorScheme = null;
        private DialogResult m_DialogResult = DialogResult.None;
        private bool m_IsDefault = false;
        private bool m_FadeEffect = true;
        private eButtonTextAlignment m_TextAlignment = eButtonTextAlignment.Center;
        private Size m_PreferredSize = Size.Empty;
        #endregion

        internal ButtonItem ButtonItem
        {
            get { return (m_Button); }
        }

        #region Constructor
        public ButtonX()
        {
            this.IsAccessible = true;
            this.AccessibleRole = AccessibleRole.PushButton;
            base.SetStyle(ControlStyles.StandardDoubleClick | ControlStyles.StandardClick, false);
            StyleManager.Register(this);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                StyleManager.Unregister(this);
                //m_Button.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Called by StyleManager to notify control that style on manager has changed and that control should refresh its appearance if
        /// its style is controlled by StyleManager.
        /// </summary>
        /// <param name="newStyle">New active style.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void StyleManagerStyleChanged(eDotNetBarStyle newStyle)
        {
            this.Style = this.Style;
        }

        protected virtual ButtonItem CreateButtonItem()
        {
            return new ButtonItem();
        }

        protected override PopupItem CreatePopupItem()
        {
            m_Button = CreateButtonItem();
            m_Button.GlobalItem = false;
            m_Button.Displayed = true;
            m_Button.ContainerControl = this;
            m_Button.ColorTable = eButtonColor.BlueWithBackground;
            m_Button.ButtonStyle = eButtonStyle.ImageAndText;
            m_Button._FitContainer = true;
            m_Button.Style = eDotNetBarStyle.Office2007;
            m_Button.SetOwner(this);
            m_Button.CheckedChanged += new EventHandler(OnCheckedChanged);
            return m_Button;
        }

        private void OnCheckedChanged(object sender, EventArgs e)
        {
            if (CheckedChanged != null)
                CheckedChanged(this, e);
        }

        /// <summary>
        /// Creates new accessibility instance.
        /// </summary>
        /// <returns>Reference to AccessibleObject.</returns>
        protected override AccessibleObject CreateAccessibilityInstance()
        {
            return new ButtonXAccessibleObject(this);
        }
        #endregion

        #region Internal Implementation
        /// <summary>
        /// Gets or sets whether text-markup support is enabled for controls Text property. Default value is true.
        /// Set this property to false to display HTML or other markup in the control instead of it being parsed as text-markup.
        /// </summary>
        [DefaultValue(true), Category("Appearance"), Description("Indicates whether text-markup support is enabled for controls Text property.")]
        public bool EnableMarkup
        {
            get { return m_Button.EnableMarkup; }
            set
            {
                m_Button.EnableMarkup = value;
            }
        }

        /// <summary>
        /// Starts the button pulse effect which alternates slowly between the mouse over and the default state. The pulse effect
        /// continues indefinitely until it is stopped by call to StopPulse method.
        /// </summary>
        public void Pulse()
        {
            m_Button.Pulse();
        }

        /// <summary>
        /// Starts the button pulse effect which alternates slowly between the mouse over and the default state. Pulse effect
        /// will alternate between the pulse state for the number of times specified by the pulseBeatCount parameter.
        /// </summary>
        /// <param name="pulseBeatCount">Specifies the number of times button alternates between pulse states. 0 indicates indefinite pulse</param>
        public void Pulse(int pulseBeatCount)
        {
            m_Button.Pulse(pulseBeatCount);
        }

        /// <summary>
        /// Stops the button Pulse effect.
        /// </summary>
        public void StopPulse()
        {
            m_Button.StopPulse();
        }

        /// <summary>
        /// Gets whether the button is currently pulsing, alternating slowly between the mouse over and default state.
        /// </summary>
        [Browsable(false)]
        public bool IsPulsing
        {
            get { return m_Button.IsPulsing; }
        }

        /// <summary>
        /// Gets or sets whether pulse effect started with StartPulse method stops automatically when mouse moves over the button. Default value is true.
        /// </summary>
        [DefaultValue(true), Browsable(true), Category("Behavior"), Description("Indicates whether pulse effect started with Pulse method stops automatically when mouse moves over the button.")]
        public bool StopPulseOnMouseOver
        {
            get { return m_Button.StopPulseOnMouseOver; }
            set { m_Button.StopPulseOnMouseOver = value; }
        }

        /// <summary>
        /// Gets or sets the pulse speed. The value must be greater than 0 and less than 128. Higher values indicate faster pulse. Default value is 12.
        /// </summary>
        [Browsable(true), DefaultValue(12), Category("Behavior"), Description("Indicates pulse speed. The value must be greater than 0 and less than 128.")]
        public int PulseSpeed
        {
            get { return m_Button.PulseSpeed; }
            set
            {
                m_Button.PulseSpeed = value;
            }
        }

        /// <summary>
        /// Sets fixed size of the image. Image will be scaled and painted it size specified.
        /// </summary>
        [Browsable(true)]
        public System.Drawing.Size ImageFixedSize
        {
            get { return m_Button.ImageFixedSize; }
            set
            {
                m_Button.ImageFixedSize = value;
                this.RecalcLayout();
            }
        }
        /// <summary>
        /// Gets whether ImageFixedSize property should be serialized.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeImageFixedSize()
        {
            return m_Button.ShouldSerializeImageFixedSize();
        }
        
        /// <summary>
        /// Gets or sets the text alignment. Applies only when button text is not composed using text markup. Default value is center.
        /// </summary>
        [Browsable(true), DefaultValue(eButtonTextAlignment.Center), Category("Appearance"), Description("Indicates text alignment. Applies only when button text is not composed using text markup. Default value is center.")]
        public eButtonTextAlignment TextAlignment
        {
            get { return m_TextAlignment; }
            set
            {
                m_TextAlignment = value;
                this.RecalcLayout();
            }
        }

        private bool _CallBasePaintBackground = true;
        /// <summary>
        /// Gets or sets whether during painting OnPaintBackground on base control is called when BackColor=Transparent.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool CallBasePaintBackground
        {
            get { return _CallBasePaintBackground; }
            set
            {
                _CallBasePaintBackground = value;
            }
        }

        internal void InternalPaint(PaintEventArgs e)
        {
            OnPaint(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if ((this.BackColor.IsEmpty || this.BackColor == Color.Transparent || this.BackgroundImage != null) && _CallBasePaintBackground)
            {
                base.OnPaintBackground(e);
            }
            else
            {
                DisplayHelp.FillRectangle(e.Graphics, this.ClientRectangle, this.BackColor, System.Drawing.Color.Empty);
            }

            Rectangle r = this.ClientRectangle;
            Graphics g=e.Graphics;

            ColorScheme cs = this.GetColorScheme();

            if (!IsThemed)
            {
                if (BarFunctions.IsOffice2007Style(m_Button.EffectiveStyle))
                {
                    //int cornerSize = this.CornerSize;
                    //SmoothingMode sm = g.SmoothingMode;
                    //g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    //DisplayHelp.FillRoundedRectangle(g, r, cornerSize, cs.BarBackground, cs.BarBackground2, cs.BarBackgroundGradientAngle);
                    //DisplayHelp.DrawRoundedRectangle(g, cs.BarDockedBorder, r, cornerSize);
                    //g.SmoothingMode = sm;
                }
                else
                {
                    DisplayHelp.FillRectangle(g, r, cs.BarBackground, cs.BarBackground2, cs.BarBackgroundGradientAngle);
                    DisplayHelp.DrawRectangle(g, cs.BarDockedBorder, r);
                }
            }

            SmoothingMode sm = g.SmoothingMode;
            TextRenderingHint th = g.TextRenderingHint;

            ItemPaintArgs pa = GetItemPaintArgs(g);
            
            if (this.AntiAlias)
            {
                pa.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                pa.Graphics.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
            }

            IShapeDescriptor shape = GetButtonShape();
            if (!(shape is RoundRectangleShapeDescriptor))
            {
                Rectangle rs = this.ClientRectangle;
                rs.Y--;
                rs.X--;
                rs.Width++;
                rs.Height++;
                if (shape.CanDrawShape(rs))
                {
                    using (GraphicsPath path = shape.GetShape(rs))
                        g.SetClip(path);
                }
            }

            m_Button.Paint(pa);

            g.SmoothingMode = sm;
            g.TextRenderingHint = th;

            base.OnPaint(e);
        }

        protected override void OnResize(EventArgs e)
        {
            RecalcLayout();
            base.OnResize(e);
        }

        protected override void OnTextChanged(EventArgs e)
        {
            m_Button.Text = this.Text;
            InvalidateAutoSize();
#if FRAMEWORK20
            this.AdjustSize();
#endif
            this.RecalcLayout();
            base.OnTextChanged(e);
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            InvalidateAutoSize();
        }

        protected override void OnForeColorChanged(EventArgs e)
        {
            if (this.ForeColor != SystemColors.ControlText)
                m_Button.ForeColor = this.ForeColor;
            else
                m_Button.ForeColor = Color.Empty;

            base.OnForeColorChanged(e);
        }

        protected override void RecalcSize()
        {
            m_Button.Bounds = this.ClientRectangle;
            m_Button.RecalcSize();
            m_Button.Bounds = this.ClientRectangle;
        }

        private bool _IsSpaceKeyDown = false;
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Space && !m_Button.Expanded)
            {
                if (m_Button.GetShouldAutoExpandOnClick())
                    m_Button.Expanded = true;
                else
                    m_Button.SetMouseDown(true);
                this.Invalidate();
            }
            _IsSpaceKeyDown = (e.KeyCode == Keys.Space);
            base.OnKeyDown(e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space && _IsSpaceKeyDown && !m_Button.Expanded)
            {
                m_Button.SetMouseDown(false);
                this.Invalidate();
                PerformClick();
            }
            _IsSpaceKeyDown = false;
            base.OnKeyUp(e);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            m_Button.InternalMouseEnter();
            base.OnMouseEnter(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            m_Button.InternalMouseMove(e);
            base.OnMouseMove(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            m_Button.InternalMouseLeave();
            base.OnMouseLeave(e);
        }

        protected override void OnMouseHover(EventArgs e)
        {
            m_Button.InternalMouseHover();
            base.OnMouseHover(e);
        }

        private Point _MouseDownPoint = Point.Empty;
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (!this.Focused)
                {
                    if (!this.Focus())
                        return;
                }
                _MouseDownPoint = new Point(e.X, e.Y);
            }

            m_Button.InternalMouseDown(e);
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            m_Button.InternalMouseUp(e);
            if (e.Button == MouseButtons.Left && this.ClientRectangle.Contains(e.X, e.Y) && !_MouseDownPoint.IsEmpty)
            {
#if FRAMEWORK20
                this.OnMouseClick(e);
#endif
                this.OnClick(e);
            }
            _MouseDownPoint = Point.Empty;
            base.OnMouseUp(e);
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            if(m_Button.IsMouseOver)
                m_Button.InternalMouseLeave();

            base.OnVisibleChanged(e);
        }

        protected override void OnClick(EventArgs e)
        {
            // Ignore Click event if it is fired when click occurred on sub items rectangle...
            if (!m_Button.SubItemsRect.IsEmpty)
            {
                Point p = this.PointToClient(Control.MousePosition);
                if (m_Button.SubItemsRect.Contains(p))
                    return;
            }
            
            if (this.SplitButton && !m_Button.TextDrawRect.IsEmpty)
            {
                Point p = this.PointToClient(Control.MousePosition);
                if (m_Button.TextDrawRect.Contains(p))
                    return;
            }

            Form form1 = this.FindForm();
            if (form1 != null)
            {
                form1.DialogResult = this.DialogResult;
            }

            base.OnClick(e);

            if(ExecuteCommandOnClick)
                ExecuteCommand();
        }

        /// <summary>
        /// Gets whether command is executed when button is clicked.
        /// </summary>
        protected virtual bool ExecuteCommandOnClick
        {
            get { return true; }
        }

        protected override void OnGotFocus(EventArgs e)
        {
            m_Button.OnGotFocus();
            base.OnGotFocus(e);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            m_Button.OnLostFocus();
            base.OnLostFocus(e);
        }

        /// <summary>
        /// Specifies the Button image.
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), Category("Appearance"), Description("The image that will be displayed on the face of the button."), DefaultValue(null)]
        public System.Drawing.Image Image
        {
            get { return m_Button.Image; }
            set
            {
                m_Button.Image = value;
                InvalidateAutoSize();
#if FRAMEWORK20
                this.AdjustSize();
#endif
            }
        }

        /// <summary>
        /// Specifies the image for the button when mouse is over the item.
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), Category("Appearance"), Description("The image that will be displayed when mouse hovers over the item."), DefaultValue(null)]
        public System.Drawing.Image HoverImage
        {
            get { return m_Button.HoverImage; }
            set { m_Button.HoverImage = value; }
        }

        /// <summary>
        /// Specifies the image for the button when items Enabled property is set to false.
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), Category("Appearance"), Description("The image that will be displayed when item is disabled."),DefaultValue(null)]
        public System.Drawing.Image DisabledImage
        {
            get { return m_Button.DisabledImage; }
            set { m_Button.DisabledImage = value; }
        }

        /// <summary>
        /// Specifies the image for the button when mouse left button is pressed.
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), Category("Appearance"), Description("The image that will be displayed when item is pressed."), DefaultValue(null)]
        public System.Drawing.Image PressedImage
        {
            get { return m_Button.PressedImage; }
            set { m_Button.PressedImage = value; }
        }

        /// <summary>
        /// Gets or sets the location of popup in relation to it's parent.
        /// </summary>
        [Browsable(true), DevCoBrowsable(false), DefaultValue(ePopupSide.Default), Description("Indicates location of popup in relation to it's parent.")]
        public ePopupSide PopupSide
        {
            get { return m_Button.PopupSide; }
            set { m_Button.PopupSide = value; InvalidateAutoSize(); }
        }

        /// <summary>
        /// Returns the collection of sub items.
        /// </summary>
        [Browsable(true), DevCoBrowsable(false), Editor("DevComponents.DotNetBar.Design.ButtonItemEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(System.Drawing.Design.UITypeEditor)), Category("Data"), Description("Collection of sub items."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual SubItemsCollection SubItems
        {
            get
            {
                return m_Button.SubItems;
            }
        }

        /// <summary>
        /// Gets or sets whether button appears as split button. Split button appearance divides button into two parts. Image which raises the click event
        /// when clicked and text and expand sign which shows button sub items on popup menu when clicked. Button must have both text and image visible (ButtonStyle property) in order to appear as a full split button.
        /// </summary>
        [Browsable(true), DefaultValue(false), Category("Appearance"), Description("Indicates whether button appears as split button.")]
        public bool SplitButton
        {
            get { return m_Button.SplitButton; }
            set { m_Button.SplitButton = value; InvalidateAutoSize(); }
        }

        /// <summary>
        /// Gets or sets whether button displays the expand part that indicates that button has popup.
        /// </summary>
        [System.ComponentModel.Browsable(true), DevCoBrowsable(true), System.ComponentModel.DefaultValue(true), System.ComponentModel.Category("Behavior"), System.ComponentModel.Description("Determines whether sub-items are displayed.")]
        public bool ShowSubItems
        {
            get
            {
                return m_Button.ShowSubItems;
            }
            set
            {
                m_Button.ShowSubItems = value;
            }
        }

        /// <summary>
        /// Gets/Sets the image position inside the button.
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), Category("Appearance"), Description("The alignment of the image in relation to text displayed by this item."), DefaultValue(eImagePosition.Left)]
        public eImagePosition ImagePosition
        {
            get { return m_Button.ImagePosition; }
            set
            {
                m_Button.ImagePosition = value;
                InvalidateAutoSize();
                this.RecalcLayout();
            }
        }

        /// <summary>
        /// Gets or sets whether mouse over fade effect is enabled. Default value is true.
        /// </summary>
        [Browsable(true), DefaultValue(true), Category("Appearance"), Description("Indicates whether mouse over fade effect is enabled")]
        public bool FadeEffect
        {
            get { return m_FadeEffect; }
            set
            {
                m_FadeEffect = value;
            }
        }

        internal bool IsFadeEnabled
        {
            get
            {
                if (this.DesignMode || (!BarFunctions.IsOffice2007Style(m_Button.EffectiveStyle)) ||
                    m_FadeEffect && NativeFunctions.IsTerminalSession() || IsThemed || TextDrawing.UseTextRenderer)
                    return false;
                return m_FadeEffect;
            }
        }

        /// <summary>
        /// Indicates the way button is rendering the mouse over state. Setting the value to Color will render the image in gray-scale when mouse is not over the item.
        /// </summary>
        [System.ComponentModel.Browsable(true), DevCoBrowsable(true), System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("Indicates the button mouse over tracking style. Setting the value to Color will render the image in gray-scale when mouse is not over the item."), System.ComponentModel.DefaultValue(eHotTrackingStyle.Default)]
        public virtual eHotTrackingStyle HotTrackingStyle
        {
            get { return m_Button.HotTrackingStyle; }
            set
            {
                m_Button.HotTrackingStyle = value;
            }
        }

        internal override ItemPaintArgs GetItemPaintArgs(Graphics g)
        {
            ItemPaintArgs pa = base.GetItemPaintArgs(g);
            pa.IsDefaultButton = m_IsDefault && _FocusCuesEnabled;
            
            return pa;
        }

        /// <summary>
        /// Gets or sets the width of the expand part of the button item.
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), Category("Behavior"), Description("Indicates the width of the expand part of the button item."), DefaultValue(12)]
        public virtual int SubItemsExpandWidth
        {
            get { return m_Button.SubItemsExpandWidth; }
            set
            {
                m_Button.SubItemsExpandWidth = value;
                this.RecalcLayout();
            }
        }

        /// <summary>
        /// Gets or sets the text associated with this button.
        /// </summary>
        [Browsable(true), Editor("DevComponents.DotNetBar.Design.TextMarkupUIEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(System.Drawing.Design.UITypeEditor)), Category("Appearance"), Description("Indicates text associated with this button.."), Localizable(true), DefaultValue("")]
        public override string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        /// <summary>
        /// Gets/Sets informational text (tooltip) for the button.
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), DefaultValue(""), Editor("DevComponents.DotNetBar.Design.TextMarkupUIEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(System.Drawing.Design.UITypeEditor)), Category("Appearance"), Description("Indicates informational text (tooltip) for the button."), Localizable(true)]
        public virtual string Tooltip
        {
            get { return m_Button.Tooltip; }
            set { m_Button.Tooltip = value; }
        }

        private bool _EnableMnemonicWithAltKeyOnly = false;
        /// <summary>
        /// Gets or sets whether mnemonic character assigned to button is processed only if Alt key is pressed. Default value is false which indicate that Alt key is not required.
        /// </summary>
        [DefaultValue(false), Category("Behavior"), Description("Indicates whether mnemonic character assigned to button is processed only if Alt key is pressed")]
        public bool EnableMnemonicWithAltKeyOnly
        {
            get { return _EnableMnemonicWithAltKeyOnly; }
            set
            {
                _EnableMnemonicWithAltKeyOnly = value;
            }
        }

        protected override bool ProcessMnemonic(char charCode)
        {
            if (CanSelect && IsMnemonic(charCode, this.Text) && this.Enabled && 
                (!_EnableMnemonicWithAltKeyOnly || Control.ModifierKeys == Keys.Alt || this.Focused))
            {
                if (Focus())
                {
                    PerformClick();
                    return true;
                }
            }
            return base.ProcessMnemonic(charCode);
        }

        /// <summary>
        /// Indicates whether the button will auto-expand when clicked. 
        /// When button contains sub-items, sub-items will be shown only if user
        /// click the expand part of the button. Setting this property to true will expand the button and show sub-items when user
        /// clicks anywhere inside of the button. Default value is false which indicates that button is expanded only
        /// if its expand part is clicked.
        /// </summary>
        [DefaultValue(false), Browsable(true), DevCoBrowsable(true), Category("Behavior"), Description("Indicates whether the button will auto-expand (display pop-up menu or toolbar) when clicked.")]
        public virtual bool AutoExpandOnClick
        {
            get { return m_Button.AutoExpandOnClick; }
            set
            {
                m_Button.AutoExpandOnClick = value;
                this.RecalcLayout();
            }
        }

        /// <summary>
        /// Gets or sets whether Checked property is automatically inverted, button checked/unchecked, when button is clicked. Default value is false.
        /// </summary>
        [Browsable(true), DefaultValue(false), Category("Behavior"), Description("Indicates whether Checked property is automatically inverted when button is clicked.")]
        public bool AutoCheckOnClick
        {
            get { return m_Button.AutoCheckOnClick; }
            set { m_Button.AutoCheckOnClick = value; }
        }

        /// <summary>
        /// Gets or set a value indicating whether the button is in the checked state.
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), Category("Appearance"), Description("Indicates whether item is checked or not."), DefaultValue(false)]
        public virtual bool Checked
        {
            get
            {
                return m_Button.Checked;
            }
            set
            {
                m_Button.Checked = value;
                this.Invalidate();
            }
        }

        private static RoundRectangleShapeDescriptor _DefaultButtonShape = new RoundRectangleShapeDescriptor(2);
        internal IShapeDescriptor GetButtonShape()
        {
            if (this.Shape != null)
                return this.Shape;
            return _DefaultButtonShape;
        }

        private ShapeDescriptor _Shape = null;
        /// <summary>
        /// Gets or sets an shape descriptor for the button which describes the shape of the button. Default value is null
        /// which indicates that system default shape is used.
        /// </summary>
        [DefaultValue(null), Editor("DevComponents.DotNetBar.Design.ShapeTypeEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor)), TypeConverter("DevComponents.DotNetBar.Design.ShapeStringConverter, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf"), MergableProperty(false)]
        public ShapeDescriptor Shape
        {
            get { return _Shape; }
            set
            {
                if (_Shape != value)
                {
                    _Shape = value;
                    this.Invalidate();
                }
            }
        }

        private bool _FocusCuesEnabled = true;
        /// <summary>
        /// Gets or sets whether control displays focus cues when focused.
        /// </summary>
        [DefaultValue(true), Category("Behavior"), Description("Indicates whether control displays focus cues when focused.")]
        public virtual bool FocusCuesEnabled
        {
            get { return _FocusCuesEnabled; }
            set
            {
                _FocusCuesEnabled = value;
                if (this.Focused) this.Invalidate();
            }
        }

        internal int CornerSize
        {
            get { return 2; }
        }

        /// <summary>
        /// Gets or sets the custom color name. Name specified here must be represented by the corresponding object with the same name that is part
        /// of the Office2007ColorTable.ButtonItemColors collection. See documentation for Office2007ColorTable.ButtonItemColors for more information.
        /// If color table with specified name cannot be found default color will be used. Valid settings for this property override any
        /// setting to the Color property.
        /// Applies to items with Office 2007 style only.
        /// </summary>
        [Browsable(true), DevCoBrowsable(false), DefaultValue(""), Category("Appearance"), Description("Indicates custom color table name for the button when Office 2007 style is used.")]
        public string CustomColorName
        {
            get { return m_Button.CustomColorName; }
            set
            {
                m_Button.CustomColorName = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the predefined color of the button. Color specified applies to buttons with Office 2007 style only. It does not have
        /// any effect on other styles. Default value is eButtonColor.Default
        /// </summary>
        [Browsable(true), DevCoBrowsable(false), DefaultValue(eButtonColor.BlueWithBackground), Category("Appearance"), Description("Indicates predefined color of button when Office 2007 style is used.")]
        public eButtonColor ColorTable
        {
            get { return m_Button.ColorTable; }
            set
            {
                if (m_Button.ColorTable != value)
                {
                    m_Button.ColorTable = value;
                    this.Invalidate();
                }
            }
        }
        protected override void InvalidateAutoSize()
        {
            m_PreferredSize = Size.Empty;
        }

#if FRAMEWORK20
        //[Localizable(true), Browsable(false)]
        //public new System.Windows.Forms.Padding Padding
        //{
        //    get { return base.Padding; }
        //    set { base.Padding = value; }
        //}

        public override Size GetPreferredSize(Size proposedSize)
        {
            if (!m_PreferredSize.IsEmpty) return m_PreferredSize;

            if (!BarFunctions.IsHandleValid(this))
                return base.GetPreferredSize(proposedSize);
            if (this.Text.Length == 0 && this.Image == null)
                return base.GetPreferredSize(proposedSize);

            int oldWidth = m_Button.WidthInternal, oldHeight = m_Button.HeightInternal;

            m_Button._FitContainer = false;
            m_Button.RecalcSize();

            Size s = m_Button.Size;
            if (s.Width < this.MinimumSize.Width)
                s.Width = this.MinimumSize.Width;
            if (s.Height < this.MinimumSize.Height)
                s.Height = this.MinimumSize.Height;

            if (m_AutoSizeMode == AutoSizeMode.GrowOnly)
            {
                if (s.Width < this.Size.Width)
                    s.Width = this.Size.Width;
                if (s.Height < this.Size.Height)
                    s.Height = this.Size.Height;

                if (proposedSize.Width > 0 && proposedSize.Width < 50000 && s.Width < proposedSize.Width)
                    s.Width = proposedSize.Width;
                if (proposedSize.Height > 0 && proposedSize.Height < 50000 && s.Height < proposedSize.Height)
                    s.Height = proposedSize.Height;
                    
            }
            m_Button._FitContainer = true;
            RecalcSize();
            m_PreferredSize = s;
            return m_PreferredSize;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control is automatically resized to display its entire contents. You can set MaximumSize.Width property to set the maximum width used by the control.
        /// </summary>
        [Browsable(true), DefaultValue(false), EditorBrowsable(EditorBrowsableState.Always), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public override bool AutoSize
        {
            get
            {
                return base.AutoSize;
            }
            set
            {
                if (this.AutoSize != value)
                {
                    base.AutoSize = value;
                    InvalidateAutoSize();
                    AdjustSize();
                }
            }
        }

        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            if (this.AutoSize)
            {
                Size preferredSize = base.PreferredSize;
                if(preferredSize.Width>0)
                    width = preferredSize.Width;
                if (preferredSize.Height > 0)
                    height = preferredSize.Height;
            }
            base.SetBoundsCore(x, y, width, height, specified);
        }

        protected override void AdjustSize()
        {
            if (this.AutoSize)
            {
                System.Drawing.Size prefSize = base.PreferredSize;
                if(prefSize.Width>0 && prefSize.Height>0)
                    this.Size = base.PreferredSize;
            }
        }

        private AutoSizeMode m_AutoSizeMode = AutoSizeMode.GrowOnly;
        /// <summary>
        /// Gets or sets the mode by which the Button automatically resizes itself. 
        /// </summary>
        [LocalizableAttribute(true), Browsable(true), DefaultValue(AutoSizeMode.GrowOnly), Category("Layout"), Description("Indicates the mode by which the Button automatically resizes itself. ")]
        public AutoSizeMode AutoSizeMode
        {
            get { return m_AutoSizeMode; }
            set
            {
                if (m_AutoSizeMode != value)
                {
                    m_AutoSizeMode = value;
                    InvalidateAutoSize();
                    AdjustSize();
                }
            }
        }

        //[Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        //public override Image BackgroundImage
        //{
        //    get { return base.BackgroundImage; }
        //    set { base.BackgroundImage = value; }
        //}
        //[Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        //public override ImageLayout BackgroundImageLayout
        //{
        //    get
        //    {
        //        return base.BackgroundImageLayout;
        //    }
        //    set
        //    {
        //        base.BackgroundImageLayout = value;
        //    }
        //}
#endif
        private int _ImageTextSpacing = 0;
        /// <summary>
        /// Gets or sets the amount of spacing between button image if specified and text.
        /// </summary>
        [Browsable(true), DefaultValue(0), Category("Layout"), Description("Indicates amount of spacing between button image if specified and text.")]
        public virtual int ImageTextSpacing
        {
            get { return _ImageTextSpacing; }
            set
            {
                _ImageTextSpacing = value;
                RecalcLayout();
            }
        }
        #endregion

        #region IButtonControl Members
        /// <summary>
        /// Gets or sets the value returned to the parent form when the button is clicked.
        /// </summary>
        [Browsable(true), Category("Behavior"), DefaultValue(DialogResult.None), Description("Gets or sets the value returned to the parent form when the button is clicked.")]
        public DialogResult DialogResult
        {
            get
            {
                return m_DialogResult;
            }

            set
            {
                if (Enum.IsDefined(typeof(DialogResult), value))
                {
                    m_DialogResult = value;
                }
            }
        }

        /// <summary>
        /// Notifies a control that it is the default button so that its appearance and behavior is adjusted accordingly.
        /// </summary>
        /// <param name="value">true if the control should behave as a default button; otherwise false.</param>
        public void NotifyDefault(bool value)
        {
            if (m_IsDefault != value)
            {
                m_IsDefault = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// Generates a Click event for the control.
        /// </summary>
        public override void PerformClick()
        {
            if (!this.Enabled) return;

            Form form1 = this.FindForm();
            if (form1 != null)
            {
                form1.DialogResult = this.DialogResult;
            }
            if (this.AutoCheckOnClick) this.Checked = !this.Checked;

            if (ExecuteCommandOnClick)
                ExecuteCommand();

            // Must call base since this class overrides OnClick to prevent it from firing when sub-items rect is clicked
            base.OnClick(EventArgs.Empty);
        }
        #endregion

        #region IOwner 

        /// <summary>
        /// Gets or sets a value indicating whether the button is expanded (displays drop-down) or not.
        /// </summary>
        [Browsable(false), DefaultValue(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual bool Expanded
        {
            get { return m_Button.Expanded; }
            set { m_Button.Expanded = value; }
        }

        /// <summary>
        /// Gets or sets the collection of shortcut keys associated with the button. When shortcut key is pressed button Click event is raised.
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), Category("Design"), Description("Indicates list of shortcut keys for this button."), Editor("DevComponents.DotNetBar.Design.ShortcutsDesigner, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(System.Drawing.Design.UITypeEditor)), System.ComponentModel.TypeConverter("DevComponents.DotNetBar.Design.ShortcutsConverter, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual ShortcutsCollection Shortcuts
        {
            get { return m_Button.Shortcuts; }
            set { m_Button.Shortcuts = value; }
        }

        /// <summary>
		/// Displays the sub-items on popup specified by PopupType.
		/// </summary>
		/// <param name="p">Popup location.</param>
		public virtual void Popup(Point p)
		{
            m_Button.Popup(p);
		}

		/// <summary>
		/// Displays the sub-items on popup specified by PopupType.
		/// </summary>
		/// <param name="x">Horizontal coordinate in pixels of the upper left corner of a popup.</param>
		/// <param name="y">Vertical coordinate in pixels of the upper left corner of a popup.</param>
		public virtual void Popup(int x, int y)
		{
            m_Button.Popup(x, y);
		}
        #endregion

        #region ICommandSource Members
        protected virtual void ExecuteCommand()
        {
            if (_Command == null) return;
            CommandManager.ExecuteCommand(this);
        }

        /// <summary>
        /// Gets or sets the command assigned to the item. Default value is null.
        /// <remarks>Note that if this property is set to null Enabled property will be set to false automatically to disable the item.</remarks>
        /// </summary>
        [DefaultValue(null), Category("Commands"), Description("Indicates the command assigned to the item.")]
        public Command Command
        {
            get { return (Command)((ICommandSource)this).Command; }
            set
            {
                ((ICommandSource)this).Command = value;
            }
        }

        private ICommand _Command = null;
        //[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        ICommand ICommandSource.Command
        {
            get
            {
                return _Command;
            }
            set
            {
                bool changed = false;
                if (_Command != value)
                    changed = true;

                if (_Command != null)
                    CommandManager.UnRegisterCommandSource(this, _Command);
                _Command = value;
                if (value != null)
                    CommandManager.RegisterCommand(this, value);
                if(changed)
                    OnCommandChanged();
            }
        }

        /// <summary>
        /// Called when Command property value changes.
        /// </summary>
        protected virtual void OnCommandChanged()
        {
        }

        private object _CommandParameter = null;
        /// <summary>
        /// Gets or sets user defined data value that can be passed to the command when it is executed.
        /// </summary>
        [Browsable(true), DefaultValue(null), Category("Commands"), Description("Indicates user defined data value that can be passed to the command when it is executed."), System.ComponentModel.TypeConverter(typeof(System.ComponentModel.StringConverter)), System.ComponentModel.Localizable(true)]
        public object CommandParameter
        {
            get
            {
                return _CommandParameter;
            }
            set
            {
                _CommandParameter = value;
            }
        }

        #endregion
    }

    #region ButtonXAccessibleObject
    /// <summary>
    /// Represents class for Accessibility support.
    /// </summary>
    public class ButtonXAccessibleObject : Control.ControlAccessibleObject
    {
        private ButtonX m_Owner = null;
        /// <summary>
        /// Creates new instance of the object and initializes it with owner control.
        /// </summary>
        /// <param name="owner">Reference to owner control.</param>
        public ButtonXAccessibleObject(ButtonX owner)
            : base(owner)
        {
            m_Owner = owner;
        }

        protected ButtonX Owner
        {
            get { return m_Owner; }
        }

        ///// <summary>
        ///// Gets or sets accessible name.
        ///// </summary>
        //public override string Name
        //{
        //    get
        //    {
        //        if (m_Owner != null && !m_Owner.IsDisposed && !string.IsNullOrEmpty(m_Owner.AccessibleName))
        //            return m_Owner.AccessibleName;
        //        if (m_Owner != null) return TextWithoutMnemonics(m_Owner.Text);
        //        return "";
        //    }
        //    set
        //    {
        //        if (m_Owner != null && !m_Owner.IsDisposed)
        //            m_Owner.AccessibleName = value;
        //    }
        //}

        //internal static string TextWithoutMnemonics(string text)
        //{
        //    if (text == null)
        //    {
        //        return null;
        //    }
        //    int index = text.IndexOf('&');
        //    if (index == -1)
        //    {
        //        return text;
        //    }
        //    StringBuilder builder = new StringBuilder(text.Substring(0, index));
        //    while (index < text.Length)
        //    {
        //        if (text[index] == '&')
        //        {
        //            index++;
        //        }
        //        if (index < text.Length)
        //        {
        //            builder.Append(text[index]);
        //        }
        //        index++;
        //    }
        //    return builder.ToString();
        //}

        ///// <summary>
        ///// Gets accessible description.
        ///// </summary>
        //public override string Description
        //{
        //    get
        //    {
        //        if (m_Owner != null && !m_Owner.IsDisposed)
        //            return m_Owner.AccessibleDescription;
        //        return "";
        //    }
        //}

        /// <summary>
        /// Gets accessible role.
        /// </summary>
        public override AccessibleRole Role
        {
            get
            {
                if (m_Owner != null && !m_Owner.IsDisposed)
                    return m_Owner.AccessibleRole;
                return System.Windows.Forms.AccessibleRole.None;
            }
        }

        /// <summary>
        /// Gets parent accessibility object.
        /// </summary>
        public override AccessibleObject Parent
        {
            get
            {
                if (m_Owner != null && !m_Owner.IsDisposed)
                    return m_Owner.Parent.AccessibilityObject;
                return null;
            }
        }

        /// <summary>
        /// Returns bounds of the control.
        /// </summary>
        public override Rectangle Bounds
        {
            get
            {
                if (m_Owner != null && !m_Owner.IsDisposed && m_Owner.Parent != null)
                    return this.m_Owner.Parent.RectangleToScreen(m_Owner.Bounds);
                return Rectangle.Empty;
            }
        }

        /// <summary>
        /// Returns number of child objects.
        /// </summary>
        /// <returns>Total number of child objects.</returns>
        public override int GetChildCount()
        {
            if (m_Owner != null && !m_Owner.IsDisposed)
                return m_Owner.InternalItem.AccessibleObject.GetChildCount();
            return 0;
        }

        /// <summary>
        /// Returns reference to child object given the index.
        /// </summary>
        /// <param name="iIndex">0 based index of child object.</param>
        /// <returns>Reference to child object.</returns>
        public override System.Windows.Forms.AccessibleObject GetChild(int iIndex)
        {
            if (m_Owner != null && !m_Owner.IsDisposed)
                return m_Owner.InternalItem.AccessibleObject.GetChild(iIndex);  //return m_Owner.SubItems[iIndex].AccessibleObject;
            return null;
        }

        /// <summary>
        /// Returns current accessible state.
        /// </summary>
        public override AccessibleStates State
        {
            get
            {
                return m_Owner.InternalItem.AccessibleObject.State;
            }
        }

        /// <summary>
        /// Gets or sets the value of an accessible object.
        /// </summary>
        public override string Value
        {
            get
            {
                return m_Owner.Text;
            }
            set
            {
                m_Owner.Text = value;
            }
        }

        public override void DoDefaultAction()
        {
            if (m_Owner != null)
                m_Owner.PerformClick();
        }

        public override string DefaultAction
        {
            get
            {
                if (m_Owner != null && m_Owner.AccessibleDefaultActionDescription != "")
                    return m_Owner.AccessibleDefaultActionDescription;

                return "Click";
            }
        }
    }
    #endregion
}
