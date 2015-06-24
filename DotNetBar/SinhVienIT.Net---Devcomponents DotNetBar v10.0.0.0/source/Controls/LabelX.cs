using System;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using System.Security.Permissions;

namespace DevComponents.DotNetBar
{
#if FRAMEWORK20
    [Designer("DevComponents.DotNetBar.Design.LabelXDesigner, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
#endif
    [ToolboxBitmap(typeof(LabelX), "Controls.LabelX.ico"), ToolboxItem(true), System.Runtime.InteropServices.ComVisible(false)]
    public class LabelX : BaseItemControl, ICommandSource
    {
        #region Private Variables
        private LabelItem m_Label = null;
        private bool m_UseMnemonic = true;
        private Size m_PreferredSize = Size.Empty;
        #endregion

        #region Events
        /// <summary>
        /// Occurs when text markup link is clicked. Markup links can be created using "a" tag, for example:
        /// <a name="MyLink">Markup link</a>
        /// </summary>
        public event MarkupLinkClickEventHandler MarkupLinkClick;
        #endregion

        #region Constructor, Dispose
        public LabelX()
        {
            m_Label = new LabelItem();
            m_Label.Style = eDotNetBarStyle.Office2007;
            m_Label.MarkupLinkClick += new MarkupLinkClickEventHandler(LabelMarkupLinkClick);
            this.FocusCuesEnabled = false;
            this.HostItem = m_Label;
            this.TabStop = false;
            this.SetStyle(ControlStyles.Selectable, false);
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
            get { return m_Label.EnableMarkup; }
            set
            {
                m_Label.EnableMarkup = value;
            }
        }

        /// <summary>
        /// Gets or sets whether control displays focus cues when focused.
        /// </summary>
        [DefaultValue(false), Category("Behavior"), Description("Indicates whether control displays focus cues when focused.")]
        public override bool FocusCuesEnabled
        {
            get
            {
                return base.FocusCuesEnabled;
            }
            set
            {
                base.FocusCuesEnabled = value;
            }
        }
        protected override void OnHandleCreated(EventArgs e)
        {
#if FRAMEWORK20
            if (this.AutoSize)
                this.AdjustSize();
#endif
            this.RecalcLayout();
            base.OnHandleCreated(e);
        }
        /// <summary>
        /// Recalculates the size of the internal item.
        /// </summary>
        protected override void RecalcSize()
        {
            m_Label.SuspendPaint = true;
            m_Label.Width = m_Label.Bounds.Width;
            m_Label.Height = m_Label.Bounds.Height;
            m_Label.SuspendPaint = false;
            base.RecalcSize();
        }

        protected override void OnBackColorChanged(EventArgs e)
        {
            m_Label.BackColor = this.BackColor;
            base.OnBackColorChanged(e);
        }

        protected override void OnForeColorChanged(EventArgs e)
        {
            if (this.ForeColor == SystemColors.ControlText)
                m_Label.ForeColor = Color.Empty;
            else
                m_Label.ForeColor = this.ForeColor;
            base.OnForeColorChanged(e);
        }

        /// <summary>
        /// Gets or sets the border sides that are displayed. Default value specifies border on all 4 sides.
        /// </summary>
        [Browsable(false), Category("Appearance"), DefaultValue(LabelItem.DEFAULT_BORDERSIDE), Description("Specifies border sides that are displayed.")]
        public eBorderSide BorderSide
        {
            get { return m_Label.BorderSide; }
            set { m_Label.BorderSide = value; InvalidateAutoSize(); }
        }

        /// <summary>
        /// Gets or sets the type of the border drawn around the label.
        /// </summary>
        [Browsable(false), Category("Appearance"), DefaultValue(eBorderType.None) , Description("Indicates the type of the border drawn around the label.")]
        public eBorderType BorderType
        {
            get { return m_Label.BorderType; }
            set { m_Label.BorderType = value; InvalidateAutoSize(); }
        }

        /// <summary>
        /// Specifies label image.
        /// </summary>
        [Browsable(true), Category("Appearance"), Description("The image that will be displayed on the face of the item."), DefaultValue(null)]
        public System.Drawing.Image Image
        {
            get { return m_Label.Image; }
            set { m_Label.Image = value; InvalidateAutoSize(); }
        }

        /// <summary>
        /// Gets/Sets the image position inside the label.
        /// </summary>
        [Browsable(true), Category("Appearance"), Description("The alignment of the image in relation to text displayed by this item."), DefaultValue(eImagePosition.Left)]
        public eImagePosition ImagePosition
        {
            get { return m_Label.ImagePosition; }
            set { m_Label.ImagePosition = value; InvalidateAutoSize(); }
        }

        /// <summary>
        /// Gets or sets the border line color when border is single line.
        /// </summary>
        [Browsable(true), Category("Appearance"), Description("Indicates border line color when border is single line.")]
        public Color SingleLineColor
        {
            get { return m_Label.SingleLineColor; }
            set { m_Label.SingleLineColor = value; }
        }

        /// <summary>
        /// Gets whether property should be serialized.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeSingleLineColor()
        {
            return m_Label.ShouldSerializeSingleLineColor();
        }

        /// <summary>
        /// Resets the SingleLineColor property to its default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetSingleLineColor()
        {
            m_Label.ResetSingleLineColor();
        }

        /// <summary>
		/// Gets or sets the text associated with this item.
		/// </summary>
        [Browsable(true), Editor("DevComponents.DotNetBar.Design.TextMarkupUIEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(System.Drawing.Design.UITypeEditor)), Category("Appearance"), Description("The text contained in the item.")]
        public override string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        /// <summary>
        /// Gets or sets the horizontal text alignment.
        /// </summary>
        [Browsable(true), DefaultValue(StringAlignment.Near), DevCoBrowsable(true), Category("Layout"), Description("Indicates text alignment.")]
        public System.Drawing.StringAlignment TextAlignment
        {
            get { return m_Label.TextAlignment; }
            set 
            {
                m_Label.TextAlignment = value;
                this.RecalcLayout();
            }
        }

        /// <summary>
        /// Gets or sets the text vertical alignment.
        /// </summary>
        [Browsable(true), DefaultValue(System.Drawing.StringAlignment.Center), DevCoBrowsable(true), Category("Layout"), Description("Indicates text line alignment.")]
        public System.Drawing.StringAlignment TextLineAlignment
        {
            get  { return m_Label.TextLineAlignment; }
            set 
            {
                m_Label.TextLineAlignment = value;
                this.RecalcLayout();
            }
        }

        /// <summary>
        /// Gets or sets a value that determines whether text is displayed in multiple lines or one long line.
        /// </summary>
        [Browsable(true), Category("Style"), DefaultValue(false), Description("Gets or sets a value that determines whether text is displayed in multiple lines or one long line.")]
        public bool WordWrap
        {
            get { return m_Label.WordWrap; }
            set { m_Label.WordWrap = value; RecalcLayout(); }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new bool TabStop
        {
            get { return base.TabStop; }
            set { base.TabStop = value; }
        }

        /// <summary>
        /// Gets or sets the left padding in pixels.
        /// </summary>
        [Browsable(true), DefaultValue(0), Category("Layout"), Description("Indicates left padding in pixels.")]
        public int PaddingLeft
        {
            get { return m_Label.PaddingLeft;  }
            set { m_Label.PaddingLeft = value; InvalidateAutoSize(); }
        }

        /// <summary>
        /// Gets or sets the right padding in pixels.
        /// </summary>
        [Browsable(true), DefaultValue(0), Category("Layout"), Description("Indicates right padding in pixels.")]
        public int PaddingRight
        {
            get
            {
                return m_Label.PaddingRight;
            }
            set
            {
                m_Label.PaddingRight = value;
                InvalidateAutoSize();
            }
        }

        /// <summary>
        /// Gets or sets the top padding in pixels.
        /// </summary>
        [Browsable(true), DefaultValue(0), Category("Layout"), Description("Indicates top padding in pixels.")]
        public int PaddingTop
        {
            get
            {
                return m_Label.PaddingTop;
            }
            set
            {
                m_Label.PaddingTop = value;
                InvalidateAutoSize();
            }
        }

        /// <summary>
        /// Gets or sets the bottom padding in pixels.
        /// </summary>
        [Browsable(true), DefaultValue(0), Category("Layout"), Description("Indicates bottom padding in pixels.")]
        public int PaddingBottom
        {
            get
            {
                return m_Label.PaddingBottom;
            }
            set
            {
                m_Label.PaddingBottom = value;
                InvalidateAutoSize();
            }
        }

        private void LabelMarkupLinkClick(object sender, MarkupLinkClickEventArgs e)
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

        /// <summary>
        /// Gets or sets a value indicating whether the control interprets an ampersand character (&) in the control's Text property to be an access key prefix character.
        /// </summary>
        [Browsable(true), DefaultValue(true), Category("Appearance"), Description("Indicates whether the control interprets an ampersand character (&) in the control's Text property to be an access key prefix character.")]
        public bool UseMnemonic
        {
            get { return m_UseMnemonic; }
            set
            {
                m_UseMnemonic = value;
                InvalidateAutoSize();
                this.Invalidate();
            }
        }

        private void InvalidateAutoSize()
        {
            m_PreferredSize = Size.Empty;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (m_UseMnemonic)
                m_Label.ShowPrefix = true;
            else
                m_Label.ShowPrefix = false;

            base.OnPaint(e);
        }

        private bool CanProcessMnemonic()
        {
            if (!this.Enabled || !this.Visible)
                return false;
            return true;
        }

        [UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
        protected override bool ProcessMnemonic(char charCode)
        {
            if ((!this.UseMnemonic || !Control.IsMnemonic(charCode, this.Text)) || !this.CanProcessMnemonic() || Control.ModifierKeys != Keys.Alt)
            {
                return false;
            }
            Control parent = this.Parent;
            if (parent != null)
            {
                if (parent.SelectNextControl(this, true, false, true, false) && !parent.ContainsFocus)
                {
                    parent.Focus();
                }
            }
            return true;
        }

#if FRAMEWORK20
        [Localizable(true), Browsable(false)]
        public new System.Windows.Forms.Padding Padding
        {
            get { return base.Padding; }
            set { base.Padding = value; }
        }

        public override Size GetPreferredSize(Size proposedSize)
        {
            if (!m_PreferredSize.IsEmpty) return m_PreferredSize;

            if (!BarFunctions.IsHandleValid(this) || this.DesignMode && !this.AutoSize)
                return base.GetPreferredSize(proposedSize);
            if (this.Text.Length == 0)
                return base.GetPreferredSize(proposedSize);

            int oldWidth = m_Label.Width, oldHeight = m_Label.Height;
            m_Label.SuspendPaint = true;
            m_Label.Width = 0;
            m_Label.Height = 0;

            if ((proposedSize.Width > 0 && proposedSize.Width < 500000 || this.MaximumSize.Width > 0) && m_Label.TextMarkupBody != null)
            {
                if(TextOrientation == eOrientation.Horizontal)
                    m_Label.RecalcSizeMarkup((this.MaximumSize.Width > 0 ? this.MaximumSize.Width : proposedSize.Width));
                else
                    m_Label.RecalcSizeMarkup((this.MaximumSize.Height > 0 ? this.MaximumSize.Height : proposedSize.Height));
            }
            else
            {
                m_Label.RecalcSize();
                if (this.WordWrap && m_Label.WidthInternal > this.MaximumSize.Width && this.MaximumSize.Width > 0)
                {
                    m_Label.Height = 0;
                    m_Label.Width = this.MaximumSize.Width;
                    m_Label.RecalcSize();
                }
            }
            Size s = m_Label.Size;
            s.Height += 2;
            if (this.Font.Size > 13)
                s.Height += 2;
            m_Label.Width = oldWidth;
            m_Label.Height = oldHeight;
            m_Label.SuspendPaint = false;
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
                    AdjustSize();
                }
            }
        }

        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            if (this.AutoSize)
            {
                Size preferredSize = base.PreferredSize;
                width = preferredSize.Width;
                height = preferredSize.Height;
            }
            base.SetBoundsCore(x, y, width, height, specified);
        }

        private void AdjustSize()
        {
            if (this.AutoSize)
            {
                this.Size = base.PreferredSize;
            }
        }

        protected override void OnFontChanged(EventArgs e)
        {
            InvalidateAutoSize();
            base.OnFontChanged(e);
        }

        protected override void OnTextChanged(EventArgs e)
        {
            InvalidateAutoSize();
            base.OnTextChanged(e);
            this.AdjustSize();
        }
#endif
        /// <summary>
        /// Gets or sets text-orientation. Default is horizontal.
        /// </summary>
        [DefaultValue(eOrientation.Horizontal), Category("Appearance"), Description("Indicates text-orientation")]
        public eOrientation TextOrientation
        {
            get { return m_Label.TextOrientation; }
            set
            {
                m_Label.TextOrientation = value;
                InvalidateAutoSize();
                this.AdjustSize();
            }
        }

        /// <summary>
        /// Gets or sets how vertical text is rotated when TextOrientation = Vertical.
        /// </summary>
        [DefaultValue(true), Category("Appearance"), Description("Indicates how vertical text is rotated when TextOrientation = Vertical.")]
        public bool VerticalTextTopUp
        {
            get { return m_Label.VerticalTextTopUp; }
            set
            {
                m_Label.VerticalTextTopUp = value;
                if (TextOrientation == eOrientation.Vertical)
                {
                    this.Invalidate();
                }
            }
        }

        /// <summary>
        /// Gets the underlying LabelItem
        /// </summary>
        internal LabelItem LabelItem
        {
            get { return (m_Label); }
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
                if (changed)
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
}
