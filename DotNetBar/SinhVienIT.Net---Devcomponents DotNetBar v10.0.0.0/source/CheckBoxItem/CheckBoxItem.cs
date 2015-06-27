using System;
using System.Text;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Represents the Check-box item. Use a CheckBox to give the user an option, such as true/false or yes/no.
    /// </summary>
    [ToolboxItem(false), DesignTimeVisible(false), DefaultEvent("Click"), Designer("DevComponents.DotNetBar.Design.CheckBoxItemDesigner, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
    public class CheckBoxItem : BaseItem
    {
        #region Private Variables
        private bool m_Checked = false;
        private eCheckBoxStyle m_CheckBoxStyle = eCheckBoxStyle.CheckBox;
        private Size m_CheckSignSize = new Size(13, 13);
        private eCheckBoxPosition m_CheckBoxPosition = eCheckBoxPosition.Left;
        private int m_CheckTextSpacing = 6;
        private int m_VerticalPadding = 3;
        private bool m_MouseDown = false;
        private bool m_MouseOver = false;
        private bool m_TextVisible = true;
        private Color m_TextColor = Color.Empty;
        private bool m_ThreeState = false;
        private CheckState m_CheckState = CheckState.Unchecked;
        #endregion

        #region Events
        /// <summary>
        /// Occurs before Checked property is changed and allows you to cancel the change.
        /// </summary>
        public event CheckBoxChangeEventHandler CheckedChanging;
        /// <summary>
        /// Occurs after Checked property is changed. Action cannot be cancelled.
        /// </summary>
        public event CheckBoxChangeEventHandler CheckedChanged;
        /// <summary>
        /// Occurs when CheckState property has changed.
        /// </summary>
        public event EventHandler CheckStateChanged;
        /// <summary>
        /// Occurs when CheckedBindable property has changed.
        /// </summary>
        public event EventHandler CheckedBindableChanged;
        #endregion

        #region Constructor, Copy
        /// <summary>
		/// Creates new instance of CheckBoxItem.
		/// </summary>
		public CheckBoxItem():this("","") {}
		/// <summary>
		/// Creates new instance of CheckBoxItem and assigns the name to it.
		/// </summary>
		/// <param name="sItemName">Item name.</param>
		public CheckBoxItem(string sItemName):this(sItemName,"") {}
		/// <summary>
        /// Creates new instance of CheckBoxItem and assigns the name and text to it.
		/// </summary>
		/// <param name="sItemName">Item name.</param>
		/// <param name="ItemText">item text.</param>
        public CheckBoxItem(string sItemName, string ItemText)
            : base(sItemName, ItemText)
        {
        }

        /// <summary>
        /// Returns copy of the item.
        /// </summary>
        public override BaseItem Copy()
        {
            CheckBoxItem objCopy = new CheckBoxItem(m_Name);
            this.CopyToItem(objCopy);
            return objCopy;
        }

        /// <summary>
        /// Copies the CheckBoxItem specific properties to new instance of the item.
        /// </summary>
        /// <param name="copy">New CheckBoxItem instance.</param>
        internal void InternalCopyToItem(CheckBoxItem copy)
        {
            CopyToItem(copy);
        }

        /// <summary>
        /// Copies the CheckBoxItem specific properties to new instance of the item.
        /// </summary>
        /// <param name="copy">New CheckBoxItem instance.</param>
        protected override void CopyToItem(BaseItem copy)
        {
            CheckBoxItem objCopy = copy as CheckBoxItem;

            if (objCopy != null)
            {
                objCopy.CheckBoxPosition = this.CheckBoxPosition;
                objCopy.CheckBoxStyle = this.CheckBoxStyle;
                objCopy.Checked = this.Checked;
                objCopy.CheckState = this.CheckState;
                objCopy.TextColor = this.TextColor;
                objCopy.TextVisible = this.TextVisible;

                objCopy.ThreeState = this.ThreeState;
                objCopy.EnableMarkup = this.EnableMarkup;

                objCopy.CheckBoxImageChecked = this.CheckBoxImageChecked;
                objCopy.CheckBoxImageIndeterminate = this.CheckBoxImageIndeterminate;
                objCopy.CheckBoxImageUnChecked = this.CheckBoxImageUnChecked;

                base.CopyToItem(objCopy);
            }
        }
        #endregion

        #region Internal Implementation
        private bool _AutoCheck = true;
        /// <summary>
        /// Gets or set whether the Checked values and the item appearance are automatically changed when the Check-Box is clicked. Default value is true.
        /// </summary>
        [DefaultValue(true), Category("Behavior"), Description("Indicates whether the Checked values and the item appearance are automatically changed when the Check-Box is clicked.")]
        public bool AutoCheck
        {
            get { return _AutoCheck; }
            set
            {
                if (value != _AutoCheck)
                {
                    bool oldValue = _AutoCheck;
                    _AutoCheck = value;
                    OnAutoCheckChanged(oldValue, value);
                }
            }
        }
        private void OnAutoCheckChanged(bool oldValue, bool newValue)
        {
            //OnPropertyChanged(new PropertyChangedEventArgs("AutoCheck"));
        }

        public override void Paint(ItemPaintArgs p)
        {
            Rendering.BaseRenderer renderer = p.Renderer;
            if (renderer != null)
            {
                CheckBoxItemRenderEventArgs e = new CheckBoxItemRenderEventArgs(p.Graphics, this, p.Colors, p.Font, p.RightToLeft);
                e.ItemPaintArgs = p;
                renderer.DrawCheckBoxItem(e);
            }
            else
            {
                Rendering.CheckBoxItemPainter painter = PainterFactory.CreateCheckBoxItemPainter(this);
                if (painter != null)
                {
                    CheckBoxItemRenderEventArgs  e = new CheckBoxItemRenderEventArgs(p.Graphics, this, p.Colors, p.Font, p.RightToLeft);
                    e.ItemPaintArgs = p;
                    painter.Paint(e);
                }
            }

            this.DrawInsertMarker(p.Graphics);
        }

        internal int CheckTextSpacing
        {
            get { return m_CheckTextSpacing; }
        }

        internal int VerticalPadding
        {
            get { return m_VerticalPadding; }
            set { m_VerticalPadding = value; }
        }

        /// <summary>
        /// Gets or sets the size of the check or radio sign. Default value is 13x13. Minimum value is 6x6.
        /// </summary>
        [Category("Appearance"), Description("Indicates size of the check or radio sign. Default value is 13x13.")]
        public Size CheckSignSize
        {
            get { return m_CheckSignSize; }
            set 
            {
                if (value.Width < 6) value.Width = 6;
                if (value.Height < 6) value.Height = 6;

                m_CheckSignSize = value; this.NeedRecalcSize = true; this.Refresh(); 
            }
        }
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeCheckSignSize()
        {
            return m_CheckSignSize.Width != 13 || m_CheckSignSize.Height != 13;
        }
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetCheckSignSize()
        {
            this.CheckSignSize = new Size(13, 13);
        }

        internal Size GetCheckSignSize()
        {
            Size checkSignSize = m_CheckSignSize;
            if (_CheckBoxImageChecked != null || _CheckBoxImageIndeterminate != null || _CheckBoxImageUnChecked != null)
            {
                checkSignSize = Size.Empty;
                if (_CheckBoxImageChecked != null)
                    checkSignSize = new Size(Math.Max(checkSignSize.Width, _CheckBoxImageChecked.Width),
                        Math.Max(checkSignSize.Height, _CheckBoxImageChecked.Height));
                if (_CheckBoxImageIndeterminate != null)
                    checkSignSize = new Size(Math.Max(checkSignSize.Width, _CheckBoxImageIndeterminate.Width),
                        Math.Max(checkSignSize.Height, _CheckBoxImageIndeterminate.Height));
                if (_CheckBoxImageUnChecked != null)
                    checkSignSize = new Size(Math.Max(checkSignSize.Width, _CheckBoxImageUnChecked.Width),
                        Math.Max(checkSignSize.Height, _CheckBoxImageUnChecked.Height));
            }
            return checkSignSize;
        }

        public override void RecalcSize()
        {
            Control objCtrl = this.ContainerControl as Control;
            if (objCtrl == null || objCtrl.Disposing || objCtrl.IsDisposed)
                return;

            int verticalPadding = m_VerticalPadding;
            int checkTextSpacing = m_CheckTextSpacing;

            Graphics g = BarFunctions.CreateGraphics(objCtrl);
            if (g == null) return;

            Size checkSignSize = GetCheckSignSize();

            if (m_TextVisible)
            {
                try
                {
                    Size textSize = ButtonItemLayout.MeasureItemText(this, g, 0, objCtrl.Font, eTextFormat.Default, objCtrl.RightToLeft == RightToLeft.Yes);
                    textSize.Width += 1;
                    if (m_CheckBoxPosition == eCheckBoxPosition.Left || m_CheckBoxPosition == eCheckBoxPosition.Right)
                    {
                        m_Rect = new Rectangle(m_Rect.X, m_Rect.Y,
                            textSize.Width + checkTextSpacing + checkSignSize.Width,
                            Math.Max(checkSignSize.Height, textSize.Height) + verticalPadding * 2);
                    }
                    else
                    {
                        m_Rect = new Rectangle(m_Rect.X, m_Rect.Y,
                            Math.Max(textSize.Width, checkSignSize.Width) + verticalPadding * 2,
                            textSize.Height + checkTextSpacing + checkSignSize.Height);
                    }
                }
                finally
                {
                    g.Dispose();
                }
            }
            else
            {
                Size s = checkSignSize;
                s.Width += verticalPadding * 2;
                s.Height += verticalPadding * 2;
                m_Rect = new Rectangle(m_Rect.Location, s);
            }

            base.RecalcSize();
        }

        /// <summary>
        /// Gets or sets the text associated with this item.
        /// </summary>
        [Browsable(true), Editor("DevComponents.DotNetBar.Design.TextMarkupUIEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(System.Drawing.Design.UITypeEditor)), Category("Appearance"), Description("The text contained in the item."), Localizable(true), DefaultValue("")]
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

        #region Markup Implementation
        /// <summary>
        /// Gets whether item supports text markup. Default is false.
        /// </summary>
        protected override bool IsMarkupSupported
        {
            get { return _EnableMarkup; }
        }

        private bool _EnableMarkup = true;
        /// <summary>
        /// Gets or sets whether text-markup support is enabled for items Text property. Default value is true.
        /// Set this property to false to display HTML or other markup in the item instead of it being parsed as text-markup.
        /// </summary>
        [DefaultValue(true), Category("Appearance"), Description("Indicates whether text-markup support is enabled for items Text property.")]
        public bool EnableMarkup
        {
            get { return _EnableMarkup; }
            set
            {
                if (_EnableMarkup != value)
                {
                    _EnableMarkup = value;
                    NeedRecalcSize = true;
                    OnTextChanged();
                }
            }
        }
        #endregion

        /// <summary>
        /// Gets or sets whether text assigned to the check box is visible. Default value is true.
        /// </summary>
        [Browsable(true), DefaultValue(true), Category("Appearance"), Description("Indicates whether text assigned to the check box is visible.")]
        public bool TextVisible
        {
            get { return m_TextVisible; }
            set
            {
                m_TextVisible = value;
                this.NeedRecalcSize = true;
                OnAppearanceChanged();
            }
        }

        /// <summary>
        /// Gets or sets the text color. Default value is Color.Empty which indicates that default color is used.
        /// </summary>
        [Browsable(true), Category("Appearance"), Description("Indicates text color.")]
        public Color TextColor
        {
            get { return m_TextColor; }
            set
            {
                m_TextColor = value;
                OnAppearanceChanged();
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeTextColor()
        {
            return !m_TextColor.IsEmpty;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetTextColor()
        {
            this.TextColor = Color.Empty;
        }

        /// <summary>
        /// Gets or sets the appearance style of the item. Default value is CheckBox. Item can also assume the style of radio-button.
        /// </summary>
        [Browsable(true), DefaultValue(eCheckBoxStyle.CheckBox), Category("Appearance"), Description("Indicates appearance style of the item. Default value is CheckBox. Item can also assume the style of radio-button.")]
        public eCheckBoxStyle CheckBoxStyle
        {
            get { return m_CheckBoxStyle; }
            set
            {
                m_CheckBoxStyle = value;
                OnAppearanceChanged();
            }
        }

        /// <summary>
        /// Gets or sets the check box position relative to the text. Default value is Left.
        /// </summary>
        [Browsable(true), DefaultValue(eCheckBoxPosition.Left), Category("Appearance"), Description("Indicates the check box position relative to the text.")]
        public eCheckBoxPosition CheckBoxPosition
        {
            get { return m_CheckBoxPosition; }
            set
            {
                m_CheckBoxPosition = value;
                this.NeedRecalcSize = true;
                OnAppearanceChanged();
            }
        }

        public override void InternalMouseEnter()
        {
            base.InternalMouseEnter();
            if (!this.DesignMode && _AutoCheck)
            {
                m_MouseOver = true;
                if (this.GetEnabled())
                    this.Refresh();
            }
        }

        public override void InternalMouseLeave()
        {
            base.InternalMouseLeave();
            if (!this.DesignMode)
            {
                m_MouseOver = false;
                m_MouseDown = false;
                if (this.GetEnabled())
                    this.Refresh();
            }
        }

        public override void InternalMouseDown(MouseEventArgs objArg)
        {
            base.InternalMouseDown(objArg);
            if (objArg.Button == MouseButtons.Left && !this.DesignMode && _AutoCheck)
            {
                m_MouseDown = true;
                if (this.GetEnabled())
                    this.Refresh();
            }
        }

        public override void InternalMouseUp(MouseEventArgs objArg)
        {
            base.InternalMouseUp(objArg);

            if (m_MouseDown && !this.DesignMode)
            {
                m_MouseDown = false;
                if (this.GetEnabled())
                    this.Refresh();
            }
        }

        /// <summary>
        /// Gets whether mouse is over the item.
        /// </summary>
        [Browsable(false)]
        public bool IsMouseOver
        {
            get { return m_MouseOver; }
            internal set { m_MouseOver = value; }
        }

        /// <summary>
        /// Gets whether left mouse button is pressed on the item.
        /// </summary>
        [Browsable(false)]
        public bool IsMouseDown
        {
            get { return m_MouseDown; }
            internal set { m_MouseDown = value; }
        }

        /// <summary>
        /// Gets or set a value indicating whether the button is in the checked state.
        /// </summary>
        [Browsable(true), RefreshProperties(RefreshProperties.All), Category("Appearance"), Description("Indicates whether item is checked or not."), DefaultValue(false), Bindable(false)]
        public virtual bool Checked
        {
            get
            {
                return m_Checked;
            }
            set
            {
                if (m_Checked != value)
                {
                    if (m_ThreeState && value && m_CheckState != CheckState.Unchecked) return;

                    SetChecked(value, eEventSource.Code);
                }
            }
        }

        /// <summary>
        /// Gets or set a value indicating whether the button is in the checked state.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), RefreshProperties(RefreshProperties.All), Category("Appearance"), Description("Indicates whether item is checked or not."), DefaultValue(false), Bindable(true)]
        public virtual bool CheckedBindable
        {
            get
            {
                return this.Checked;
            }
            set
            {
                this.Checked = value;
            }
        }

        /// <summary>
        /// Raises the click event and provide the information about the source of the event.
        /// </summary>
        /// <param name="source"></param>
        public override void RaiseClick(eEventSource source)
        {
            if (_AutoCheck && CanRaiseClick && !(this.CheckBoxStyle == eCheckBoxStyle.RadioButton && this.Checked))
            {
                if (this.ThreeState)
                {
                    if (this.CheckState == CheckState.Unchecked)
                        SetChecked(CheckState.Checked, source);
                    else if (this.CheckState == CheckState.Checked)
                        SetChecked(CheckState.Indeterminate, source);
                    else if (this.CheckState == CheckState.Indeterminate)
                        SetChecked(CheckState.Unchecked, source);
                }
                else
                    SetChecked(!this.Checked, source);

                ExecuteCommand();
            }
            base.RaiseClick(source);
        }

        /// <summary>
        /// Sets the Checked property of the item, raises appropriate events and provides the information about the source of the change.
        /// </summary>
        /// <param name="newValue">New value for Checked property</param>
        /// <param name="source">Source of the change.</param>
        public virtual void SetChecked(bool newValue, eEventSource source)
        {
            // Allow user to cancel the checking
            if (m_CheckBoxStyle == eCheckBoxStyle.RadioButton && newValue && this.Parent != null)
            {
                CheckBoxItem b = null;
                foreach (BaseItem item in this.Parent.SubItems)
                {
                    if (item == this)
                        continue;
                    b = item as CheckBoxItem;
                    if (b != null && b.Checked && b.CheckBoxStyle == eCheckBoxStyle.RadioButton)
                    {
                        break;
                    }
                }
                CheckBoxChangeEventArgs e = new CheckBoxChangeEventArgs(b, this, source);
                InvokeCheckedChanging(e);
                if (e.Cancel)
                    return;
            }
            else
            {
                CheckBoxChangeEventArgs e = new CheckBoxChangeEventArgs(null, this, source);
                InvokeCheckedChanging(e);
                if (e.Cancel)
                    return;
            }

            m_Checked = newValue;
            if (m_Checked && m_CheckState == CheckState.Unchecked || !m_Checked && m_CheckState != CheckState.Unchecked)
            {
                m_CheckState = m_Checked ? CheckState.Checked : CheckState.Unchecked;
            }

            if (this.Command != null)
                this.Command.Checked = m_Checked;

            this.OnCheckedChanged(source);
            OnCheckedBindableChanged(EventArgs.Empty);

            if (ShouldSyncProperties)
                BarFunctions.SyncProperty(this, "Checked");

            if (this.Displayed)
                this.Refresh();
        }

        /// <summary>
        /// Called when Command property value changes.
        /// </summary>
        protected override void OnCommandChanged()
        {
            Command command = this.Command;
            if (command != null && command.Checked != this.Checked)
            {
                SetChecked(command.Checked, eEventSource.Code);
            }
            base.OnCommandChanged();
        }

        /// <summary>
        /// Sets the Checked property of the item, raises appropriate events and provides the information about the source of the change.
        /// </summary>
        /// <param name="newValue">New value for Checked property</param>
        /// <param name="source">Source of the change.</param>
        public virtual void SetChecked(CheckState newValue, eEventSource source)
        {
            CheckBoxChangeEventArgs e = new CheckBoxChangeEventArgs(null, this, source);
            InvokeCheckedChanging(e);
            if (e.Cancel)
                return;

            m_CheckState = newValue;
            m_Checked = (newValue != CheckState.Unchecked);
            if (this.Command != null)
                this.Command.Checked = m_Checked;
            this.OnCheckedChanged(source);
            OnCheckStateChanged(EventArgs.Empty);

            if (ShouldSyncProperties)
                BarFunctions.SyncProperty(this, "CheckState");

            if (this.Displayed)
                this.Refresh();
        }

        /// <summary>
        /// Raises CheckState changed event.
        /// </summary>
        /// <param name="eventArgs">Event arguments.</param>
        protected virtual void OnCheckStateChanged(EventArgs eventArgs)
        {
            if (CheckStateChanged != null)
                CheckStateChanged(this, eventArgs);
        }

        /// <summary>
        /// Raises CheckedBindableChanged changed event.
        /// </summary>
        /// <param name="eventArgs">Event arguments.</param>
        protected virtual void OnCheckedBindableChanged(EventArgs eventArgs)
        {
            if (CheckedBindableChanged != null)
                CheckedBindableChanged(this, eventArgs);
        }

        /// <summary>
        /// Called after Checked property has changed.
        /// </summary>
        protected virtual void OnCheckedChanged(eEventSource source)
        {
            CheckBoxItem previous = null;
            if (m_CheckBoxStyle == eCheckBoxStyle.RadioButton && m_Checked && this.Parent != null)
            {
                foreach (BaseItem item in this.Parent.SubItems)
                {
                    if (item == this)
                        continue;
                    CheckBoxItem b = item as CheckBoxItem;
                    if (b != null && b.Checked && b.CheckBoxStyle == eCheckBoxStyle.RadioButton)
                    {
                        b.Checked = false;
                        previous = b;
                    }
                }
            }

            InvokeCheckedChanged(new CheckBoxChangeEventArgs(previous, this, source));
        }

        /// <summary>
        /// Raises the CheckedChanging event.
        /// </summary>
        protected virtual void InvokeCheckedChanging(CheckBoxChangeEventArgs e)
        {
            if (CheckedChanging != null)
                CheckedChanging(this, e);
        }

        /// <summary>
        /// Raises the CheckedChanged event.
        /// </summary>
        protected virtual void InvokeCheckedChanged(CheckBoxChangeEventArgs e)
        {
            if (CheckedChanged != null)
                CheckedChanged(this, e);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the CheckBox will allow three check states rather than two. If the ThreeState property is set to true
        /// CheckState property should be used instead of Checked property to set the extended state of the control.
        /// </summary>
        [Browsable(true), Category("Behavior"), DefaultValue(false), Description("Indicates whether the CheckBox will allow three check states rather than two.")]
        public bool ThreeState
        {
            get { return m_ThreeState; }
            set { m_ThreeState = value; }
        }

        /// <summary>
        /// Specifies the state of a control, such as a check box, that can be checked, unchecked, or set to an indeterminate state. 
        /// </summary>
        [Browsable(true), Category("Behavior"), DefaultValue(CheckState.Unchecked), RefreshProperties(RefreshProperties.All), Description("Specifies the state of a control, such as a check box, that can be checked, unchecked, or set to an indeterminate state"), Bindable(true)]
        public CheckState CheckState
        {
            get { return m_CheckState; }
            set
            {
                if (value != m_CheckState)
                    SetChecked(value, eEventSource.Code);
            }
        }

        private Image _CheckBoxImageChecked = null;
        /// <summary>
        /// Gets or sets the custom image that is displayed instead default check box representation when check box is checked.
        /// </summary>
        [DefaultValue(null), Category("CheckBox Images"), Description("Indicates custom image that is displayed instead default check box representation when check box is checked")]
        public Image CheckBoxImageChecked
        {
            get { return _CheckBoxImageChecked; }
            set
            {
                _CheckBoxImageChecked = value;
                OnCheckBoxImageChanged();
            }
        }
        private Image _CheckBoxImageUnChecked = null;
        /// <summary>
        /// Gets or sets the custom image that is displayed instead default check box representation when check box is unchecked.
        /// </summary>
        [DefaultValue(null), Category("CheckBox Images"), Description("Indicates custom image that is displayed instead default check box representation when check box is unchecked")]
        public Image CheckBoxImageUnChecked
        {
            get { return _CheckBoxImageUnChecked; }
            set
            {
                _CheckBoxImageUnChecked = value;
                OnCheckBoxImageChanged();
            }
        }
        private Image _CheckBoxImageIndeterminate = null;
        /// <summary>
        /// Gets or sets the custom image that is displayed instead default check box representation when check box is in indeterminate state.
        /// </summary>
        [DefaultValue(null), Category("CheckBox Images"), Description("Indicates custom image that is displayed instead default check box representation when check box is in indeterminate state")]
        public Image CheckBoxImageIndeterminate
        {
            get { return _CheckBoxImageIndeterminate; }
            set
            {
                _CheckBoxImageIndeterminate = value;
                OnCheckBoxImageChanged();
            }
        }
        private void OnCheckBoxImageChanged()
        {
            NeedRecalcSize = true;
            Refresh();
        }
        #endregion
    }

    /// <summary>
    /// Delegate for OptionGroupChanging event.
    /// </summary>
    public delegate void CheckBoxChangeEventHandler(object sender, CheckBoxChangeEventArgs e);

    #region CheckBoxChangeEventArgs
    /// <summary>
    /// Represents event arguments for OptionGroupChanging event.
    /// </summary>
    public class CheckBoxChangeEventArgs : EventArgs
    {
        /// <summary>
        /// Set to true to cancel the checking on NewChecked button.
        /// </summary>
        public bool Cancel = false;
        /// <summary>
        /// Check-box that will become checked if operation is not cancelled.
        /// </summary>
        public readonly CheckBoxItem NewChecked;
        /// <summary>
        /// Check-box that is currently checked and which will be unchecked if operation is not cancelled. This property will have only valid values for eCheckBoxStyle.RadioButton style CheckBoxItems.
        /// </summary>
        public readonly CheckBoxItem OldChecked;
        /// <summary>
        /// Indicates the action that has caused the event.
        /// </summary>
        public readonly eEventSource EventSource = eEventSource.Code;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public CheckBoxChangeEventArgs(CheckBoxItem oldchecked, CheckBoxItem newchecked, eEventSource eventSource)
        {
            NewChecked = newchecked;
            OldChecked = oldchecked;
            EventSource = eventSource;
        }
    }
    #endregion
}
