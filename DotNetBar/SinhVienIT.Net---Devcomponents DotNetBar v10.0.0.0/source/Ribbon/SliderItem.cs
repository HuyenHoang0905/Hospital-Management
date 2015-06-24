using System;
using System.Text;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Represents the slider item which allows you to select a value from predefined range.
    /// </summary>
    [Browsable(false), Designer("DevComponents.DotNetBar.Design.SimpleItemDesigner, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
    public class SliderItem : BaseItem
    {
        #region Private Variables
        private int m_Maximum = 100;
        private int m_Minimum = 0;
        private int m_Value = 0;
        private int m_Step = 1;
        private bool m_LabelVisible = true;
        private int m_Width = 136;
        private int m_LabelWidth = 38;
        private eSliderLabelPosition m_LabelPosition = eSliderLabelPosition.Left;
        private eSliderPart m_MouseOverPart = eSliderPart.None;
        private eSliderPart m_MouseDownPart = eSliderPart.None;
        private int m_ButtonSize = 18;
        private Size m_SlideSize = new Size(11, 15);
        private bool m_TrackMarker = true;
        private Color m_TextColor = Color.Empty;
        private string m_DecreaseTooltip = "";
        private string m_IncreaseTooltip = "";
        private string m_ItemTooltip = "";
        #endregion

        #region Events
        /// <summary>
        /// Occurs after Value property has changed.
        /// </summary>
        public event EventHandler ValueChanged;

        /// <summary>
        /// Occurs before Value property has changed.
        /// </summary>
        public event CancelIntValueEventHandler ValueChanging;
        /// <summary>
        /// Occurs when Increase button is clicked using mouse.
        /// </summary>
        public event EventHandler IncreaseButtonClick;
        /// <summary>
        /// Occurs when Decrease button is clicked using mouse.
        /// </summary>
        public event EventHandler DecreaseButtonClick;
        #endregion

        #region Constructor, Copy
        /// <summary>
        /// Creates new instance of SliderItem.
		/// </summary>
		public SliderItem():this("","") {}
		/// <summary>
        /// Creates new instance of SliderItem and assigns the name to it.
		/// </summary>
		/// <param name="sItemName">Item name.</param>
		public SliderItem(string sItemName):this(sItemName,"") {}
		/// <summary>
        /// Creates new instance of SliderItem and assigns the name and text to it.
		/// </summary>
		/// <param name="sItemName">Item name.</param>
		/// <param name="ItemText">item text.</param>
        public SliderItem(string sItemName, string ItemText)
            : base(sItemName, ItemText)
        {
            this.ClickRepeatInterval = 200;
            this.MouseUpNotification = true;
            this.MouseDownCapture = true;
        }

        /// <summary>
        /// Returns copy of the item.
        /// </summary>
        public override BaseItem Copy()
        {
            SliderItem objCopy = new SliderItem(m_Name);
            this.CopyToItem(objCopy);
            return objCopy;
        }

        /// <summary>
        /// Copies the SliderItem specific properties to new instance of the item.
        /// </summary>
        /// <param name="copy">New SliderItem instance.</param>
        internal void InternalCopyToItem(SliderItem copy)
        {
            CopyToItem(copy);
        }

        /// <summary>
        /// Copies the SliderItem specific properties to new instance of the item.
        /// </summary>
        /// <param name="copy">New SliderItem instance.</param>
        protected override void CopyToItem(BaseItem copy)
        {
            SliderItem c = copy as SliderItem;
            base.CopyToItem(c);

            c.Maximum = this.Maximum;
            c.Minimum = this.Minimum;
            c.Value = this.Value;
            c.Text = this.Text;
            c.LabelWidth = this.LabelWidth;
            c.LabelVisible = this.LabelVisible;
            c.LabelPosition = this.LabelPosition;
            c.Width = this.Width;
        }

        protected override void Dispose(bool disposing)
        {
            DisposeClickTimer();
            base.Dispose(disposing);
        }
        #endregion

        #region Internal Implementation
        public override void Paint(ItemPaintArgs p)
        {
            Rendering.BaseRenderer renderer = p.Renderer;
            if (renderer != null)
            {
                SliderItemRendererEventArgs e = new SliderItemRendererEventArgs(this, p.Graphics);
                e.ItemPaintArgs = p;
                renderer.DrawSliderItem(e);
            }
            else
            {
                Rendering.SliderPainter painter = PainterFactory.CreateSliderPainter();
                if (painter != null)
                {
                    SliderItemRendererEventArgs e = new SliderItemRendererEventArgs(this, p.Graphics);
                    e.ItemPaintArgs = p;
                    painter.Paint(e);
                }
            }

            if (this.DesignMode && this.Focused)
            {
                Rectangle r = m_Rect;
                r.Inflate(-1, -1);
                DesignTime.DrawDesignTimeSelection(p.Graphics, r, p.Colors.ItemDesignTimeBorder);
            }
        }

        public override void RecalcSize()
        {
            Font font = GetFont(null);
            int sliderHeight = m_ButtonSize;

            Size sz = new Size(m_Width, sliderHeight);

            Control objCtrl = this.ContainerControl as Control;
            if (objCtrl == null || objCtrl.Disposing || objCtrl.IsDisposed)
                return;
            Graphics g = BarFunctions.CreateGraphics(objCtrl);
            if (g == null) return;
            try
            {
                if (m_LabelVisible)
                {
                    Size textSize = ButtonItemLayout.MeasureItemText(this, g, sz.Width, font, eTextFormat.Default, this.IsRightToLeft);
                    if (m_LabelPosition == eSliderLabelPosition.Left || m_LabelPosition == eSliderLabelPosition.Right)
                    {
                        sz.Height = Math.Max(sliderHeight, Math.Max(textSize.Height, font.Height));
                        sz.Width += m_LabelWidth;
                    }
                    else
                    {
                        sz.Height = sliderHeight + Math.Max(textSize.Height, font.Height);
                    }
                }
                if (_SliderOrientation == eOrientation.Vertical)
                    sz = new Size(sz.Height, sz.Width);

                m_Rect.Size = sz;
                UpdateLayout();

                base.RecalcSize();
            }
            finally
            {
                g.Dispose();
            }
        }

        public eSliderPart HitTest(int x, int y)
        {
            eSliderPart mo = eSliderPart.None;

            Rectangle r = this.DecreaseBounds;
            r.Offset(this.Bounds.Location);
            if (r.Contains(x, y))
                mo = eSliderPart.DecreaseButton;
            else
            {
                r = this.IncreaseBounds;
                r.Offset(this.Bounds.Location);
                if (r.Contains(x, y))
                    mo = eSliderPart.IncreaseButton;
                else
                {
                    r = this.SlideBounds;
                    r.Offset(this.Bounds.Location);
                    if (r.Contains(x, y))
                        mo = eSliderPart.TrackArea;
                }
            }

            return mo;
        }

        public override void InternalMouseMove(System.Windows.Forms.MouseEventArgs objArg)
        {
            if (this.GetEnabled() && !this.DesignMode)
            {
                if (this.MouseDownPart == eSliderPart.None)
                {
                    eSliderPart mo = HitTest(objArg.X, objArg.Y);

                    if (MouseOverPart != mo)
                        MouseOverPart = mo;
                }
                else if (this.MouseDownPart == eSliderPart.TrackArea)
                {
                    // Calculate and move the tracker to the position indicates by the mouse
                    if(this.SliderOrientation == eOrientation.Horizontal)
                        this.Value = GetValueFromX(objArg.X);
                    else
                        this.Value = GetValueFromY(objArg.Y);
                }
            }
            base.InternalMouseMove(objArg);
        }

        public override void InternalMouseLeave()
        {
            if (m_MouseOverPart != eSliderPart.None)
                MouseOverPart = eSliderPart.None;
            DisposeClickTimer();
            if (this.MouseDownPart != eSliderPart.None)
                this.MouseDownPart = eSliderPart.None;

            base.InternalMouseLeave();
        }

        private void DisposeClickTimer()
        {
            if (m_ClickTimer != null)
            {
                m_ClickTimer.Stop();
                m_ClickTimer.Dispose();
                m_ClickTimer = null;
            }
        }

        /// <summary>
        /// Raises the IncreaseButtonClick event.
        /// </summary>
        /// <param name="e">Provides event arguments</param>
        protected virtual void OnIncreaseButtonClick(EventArgs e)
        {
            EventHandler handler = IncreaseButtonClick;
            if (handler != null) handler(this, e);
        }
        /// <summary>
        /// Raises the DecreaseButtonClick event.
        /// </summary>
        /// <param name="e">Provides event arguments</param>
        protected virtual void OnDecreaseButtonClick(EventArgs e)
        {
            EventHandler handler = DecreaseButtonClick;
            if (handler != null) handler(this, e);
        }

        private System.Windows.Forms.Timer m_ClickTimer = null;
        public override void InternalMouseDown(System.Windows.Forms.MouseEventArgs objArg)
        {
            if (this.GetEnabled() && !this.DesignMode && objArg.Button == MouseButtons.Left)
            {
                eSliderPart mo = HitTest(objArg.X, objArg.Y);
                this.MouseDownPart = mo;

                if (this.MouseDownPart == eSliderPart.DecreaseButton)
                {
                    this.Value -= this.Step;
                    OnDecreaseButtonClick(EventArgs.Empty);
                }
                else if (this.MouseDownPart == eSliderPart.IncreaseButton)
                {
                    this.PerformStep();
                    OnIncreaseButtonClick(EventArgs.Empty);
                }
                else if (mo == eSliderPart.TrackArea)
                {
                    if (!m_TrackBounds.Contains(objArg.X, objArg.Y))
                    {
                        // Calculate and move the tracker to the position indicates by the mouse
                        if (this.SliderOrientation == eOrientation.Horizontal)
                            this.Value = GetValueFromX(objArg.X);
                        else
                            this.Value = GetValueFromY(objArg.Y);
                    }
                }

                if (this.ClickRepeatInterval > 0 && (mo == eSliderPart.IncreaseButton || mo == eSliderPart.DecreaseButton))
                {
                    if (m_ClickTimer == null)
                        m_ClickTimer = new Timer();
                    m_ClickTimer.Interval = this.ClickRepeatInterval;
                    m_ClickTimer.Tick += new EventHandler(this.ClickTimerTick);
                    m_ClickTimer.Start();
                }
            }

            base.InternalMouseDown(objArg);
        }

        private int GetValueFromX(int x)
        {
            x -= (m_SlideSize.Width - 2) / 2;
            Rectangle r = this.SlideBounds;
            r.Width -= m_SlideSize.Width;
            r.Offset(this.DisplayRectangle.Location);
            if (x <= r.X)
            {
                if (this.IsRightToLeft)
                    return this.Maximum;
                else
                    return this.Minimum;
            }
            else if (x >= r.Right)
            {
                if (this.IsRightToLeft)
                    return this.Minimum;
                else
                    return this.Maximum;
            }

            float f = (float)(x - r.X) / (float)(r.Right - r.X);
            if (this.IsRightToLeft) f = 1 - f;
            int v = this.Minimum + (int)((this.Maximum - this.Minimum) * f);

            if (this.Step > 1)
            {
                if (Math.Ceiling((float)((v - this.Minimum) / (float)this.Step)) * this.Step != v - this.Minimum)
                {
                    v = this.Minimum + (int)Math.Ceiling((float)((v - this.Minimum) / (float)this.Step)) * this.Step;
                }
            }

            return v;
        }

        private int GetValueFromY(int y)
        {
            y -= (m_SlideSize.Width - 2) / 2;
            Rectangle r = this.SlideBounds;
            r.Height -= m_SlideSize.Width;
            r.Offset(this.DisplayRectangle.Location);
            if (y <= r.Y)
            {
                return this.Maximum;
            }
            else if (y >= r.Bottom)
            {
                return this.Minimum;
            }

            float f = 1- (float)(y - r.Y) / (float)(r.Bottom - r.Y);
            int v = this.Minimum + (int)((this.Maximum - this.Minimum) * f);

            if (this.Step > 1)
            {
                if (Math.Ceiling((float)((v - this.Minimum) / (float)this.Step)) * this.Step != v - this.Minimum)
                {
                    v = this.Minimum + (int)Math.Ceiling((float)((v - this.Minimum) / (float)this.Step)) * this.Step;
                }
            }

            return v;
        }

        private void ClickTimerTick(object sender, EventArgs e)
        {
            if (this.MouseDownPart == eSliderPart.DecreaseButton)
            {
                this.Value -= this.Step;
                OnDecreaseButtonClick(EventArgs.Empty);
            }
            else if (this.MouseDownPart == eSliderPart.IncreaseButton)
            {
                this.PerformStep();
                OnIncreaseButtonClick(EventArgs.Empty);
            }
        }

        public override void InternalMouseUp(System.Windows.Forms.MouseEventArgs objArg)
        {
            DisposeClickTimer();
            if(this.MouseDownPart!= eSliderPart.None)
                this.MouseDownPart = eSliderPart.None;
            base.InternalMouseUp(objArg);
        }

        /// <summary>
        /// Gets or sets the slider mouse over part.
        /// </summary>
        internal eSliderPart MouseOverPart
        {
            get { return m_MouseOverPart; }
            set
            {
                m_MouseOverPart = value;
                UpdateTooltip();
                this.Refresh();
            }
        }

        /// <summary>
        /// Gets or sets the slider part that mouse is pressed over. This property should not be modified and it is for internal usage only.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public eSliderPart MouseDownPart
        {
            get { return m_MouseDownPart; }
            set
            {
                m_MouseDownPart = value;
                this.Refresh();
            }
        }

        protected override void OnExternalSizeChange()
        {
            UpdateLayout();
            base.OnExternalSizeChange();
        }

        private void UpdateLayout()
        {
            bool rtl = this.IsRightToLeft;
            Rectangle r = new Rectangle(Point.Empty, this.DisplayRectangle.Size);

            if (m_LabelVisible)
            {
                if (m_LabelPosition == eSliderLabelPosition.Left && (!rtl || _SliderOrientation == eOrientation.Vertical) || m_LabelPosition == eSliderLabelPosition.Right && rtl)
                {
                    if (_SliderOrientation == eOrientation.Vertical)
                    {
                        m_LabelBounds = new Rectangle(0, 2, r.Width, m_LabelWidth - 2);
                        r.Height -= m_LabelWidth;
                        r.Y += m_LabelWidth;
                    }
                    else
                    {
                        m_LabelBounds = new Rectangle(2, 0, m_LabelWidth - 2, r.Height);
                        r.Width -= m_LabelWidth;
                        r.X += m_LabelWidth;
                    }
                }
                else if (m_LabelPosition == eSliderLabelPosition.Left && rtl || m_LabelPosition == eSliderLabelPosition.Right && (!rtl || _SliderOrientation == eOrientation.Vertical))
                {
                    if (_SliderOrientation == eOrientation.Vertical)
                    {
                        m_LabelBounds = new Rectangle(0, r.Height - m_LabelWidth + 2, r.Width, m_LabelWidth - 2);
                        r.Height -= m_LabelWidth;
                    }
                    else
                    {
                        m_LabelBounds = new Rectangle(r.Width - m_LabelWidth + 2, 0, m_LabelWidth - 2, r.Height);
                        r.Width -= m_LabelWidth;
                    }
                }
                else if (m_LabelPosition == eSliderLabelPosition.Top)
                {
                    Font font = GetFont(null);
                    int h = (font != null) ? font.Height : 14;
                    if (this.TextMarkupBody != null)
                        h = this.TextMarkupBody.Bounds.Height;
                    int th = m_ButtonSize + h;
                    if (_SliderOrientation == eOrientation.Vertical)
                    {
                        m_LabelBounds = new Rectangle((r.Width - th) / 2, 0, h, r.Height);
                        r.X = m_LabelBounds.Right;
                        r.Width = m_ButtonSize;
                    }
                    else
                    {
                        m_LabelBounds = new Rectangle(0, (r.Height - th) / 2, r.Width, h);
                        r.Y = m_LabelBounds.Bottom;
                        r.Height = m_ButtonSize;
                    }
                }
                else if (m_LabelPosition == eSliderLabelPosition.Bottom)
                {
                    Font font = GetFont(null);
                    int h = (font != null) ? font.Height : 14;
                    if (this.TextMarkupBody != null)
                        h = this.TextMarkupBody.Bounds.Height;
                    int th = m_ButtonSize + h;
                    if (_SliderOrientation == eOrientation.Vertical)
                    {
                        m_LabelBounds = new Rectangle((r.Width - th) / 2 + m_ButtonSize, 0, h, r.Height);
                        r.X = (r.Width - th) / 2;
                        r.Width = m_ButtonSize;
                    }
                    else
                    {
                        m_LabelBounds = new Rectangle(0, (r.Height - th) / 2 + m_ButtonSize, r.Width, h);
                        r.Y = (r.Height - th) / 2;
                        r.Height = m_ButtonSize;
                    }
                }
            }
            else
                m_LabelBounds = Rectangle.Empty;


            if (rtl && _SliderOrientation == eOrientation.Horizontal)
            {
                m_DecreaseBounds = new Rectangle(r.Right - m_ButtonSize, r.Y + (r.Height - m_ButtonSize) / 2, m_ButtonSize, m_ButtonSize);
                m_IncreaseBounds = new Rectangle(r.X, r.Y + (r.Height - m_ButtonSize) / 2, m_ButtonSize, m_ButtonSize);
            }
            else
            {
                if (_SliderOrientation == eOrientation.Horizontal)
                {
                    m_DecreaseBounds = new Rectangle(r.X, r.Y + (r.Height - m_ButtonSize) / 2, m_ButtonSize, m_ButtonSize);
                    m_IncreaseBounds = new Rectangle(r.Right - m_ButtonSize, r.Y + (r.Height - m_ButtonSize) / 2, m_ButtonSize, m_ButtonSize);
                }
                else
                {
                    m_DecreaseBounds = new Rectangle(r.X + (r.Width - m_ButtonSize) / 2, r.Bottom - m_ButtonSize, m_ButtonSize, m_ButtonSize);
                    m_IncreaseBounds = new Rectangle(r.X + (r.Width - m_ButtonSize) / 2, r.Y, m_ButtonSize, m_ButtonSize);
                }
            }

            if (_SliderOrientation == eOrientation.Horizontal)
                m_SlideBounds = new Rectangle(r.X + m_ButtonSize, r.Y, r.Width - m_ButtonSize * 2, r.Height);
            else
                m_SlideBounds = new Rectangle(r.X, r.Y + m_ButtonSize, r.Width, r.Height - m_ButtonSize * 2);
        }

        private Rectangle m_LabelBounds = Rectangle.Empty;
        /// <summary>
        /// Returns the label bounds inside of the control.
        /// </summary>
        internal Rectangle LabelBounds
        {
            get
            {
                return m_LabelBounds;
            }
        }

        private Rectangle m_DecreaseBounds = Rectangle.Empty;
        internal Rectangle DecreaseBounds
        {
            get
            {
                return m_DecreaseBounds;
            }
        }

        private Rectangle m_IncreaseBounds = Rectangle.Empty;
        internal Rectangle IncreaseBounds
        {
            get
            {
                return m_IncreaseBounds;
            }
        }

        private Rectangle m_SlideBounds = Rectangle.Empty;
        internal Rectangle SlideBounds
        {
            get
            {
                return m_SlideBounds;
            }
        }

        private Rectangle m_TrackBounds = Rectangle.Empty;
        internal Rectangle TrackBounds
        {
            get { return m_TrackBounds; }
            set { m_TrackBounds = value; }
        }

        /// <summary>
        /// Gets or sets the text associated with this item.
        /// </summary>
        [System.ComponentModel.Browsable(true), DevCoBrowsable(true), Editor("DevComponents.DotNetBar.Design.TextMarkupUIEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(System.Drawing.Design.UITypeEditor)), System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("The text contained in the item."), System.ComponentModel.Localizable(true), System.ComponentModel.DefaultValue("")]
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
        /// Returns the Font object to be used for drawing the item text.
        /// </summary>
        /// <returns>Font object.</returns>
        private Font GetFont(ItemPaintArgs pa)
        {
            System.Drawing.Font objFont = null;

            if (pa != null)
                objFont = pa.Font;

            if (objFont == null)
            {
                System.Windows.Forms.Control objCtrl = null;
                if (pa != null)
                    objCtrl = pa.ContainerControl;
                if (objCtrl == null)
                    objCtrl = this.ContainerControl as System.Windows.Forms.Control;
                if (objCtrl != null && objCtrl.Font != null)
                    objFont = (Font)objCtrl.Font;
                else
                    objFont = (Font)System.Windows.Forms.SystemInformation.MenuFont;
            }

            return objFont;
        }

        /// <summary>
        /// Gets or sets the maximum value of the range of the control.
        /// </summary>
        [DevCoBrowsable(true), Browsable(true), Description("Gets or sets the maximum value of the range of the control."), Category("Behavior"), DefaultValue(100)]
        public int Maximum
        {
            get
            {
                return m_Maximum;
            }
            set
            {
                m_Maximum = value;
                this.Refresh();
                OnAppearanceChanged();
            }
        }

        /// <summary>
        /// Gets or sets the minimum value of the range of the control.
        /// </summary>
        [DevCoBrowsable(true), Browsable(true), Description("Gets or sets the minimum value of the range of the control."), Category("Behavior"), DefaultValue(0)]
        public int Minimum
        {
            get
            {
                return m_Minimum;
            }
            set
            {
                m_Minimum = value;
                this.Refresh();
                OnAppearanceChanged();
            }
        }

        /// <summary>
        /// Gets or sets the current position of the slider.
        /// </summary>
        [DevCoBrowsable(true), Browsable(true), Description("Gets or sets the current position of the slider."), Category("Behavior")]
        public int Value
        {
            get { return m_Value; }

            set
            {
                if (m_Value != value)
                {
                    CancelIntValueEventArgs e = new CancelIntValueEventArgs();
                    e.NewValue = value;

                    OnValueChanging(e);

                    if (e.Cancel)
                        return;

                    if (value < m_Minimum)
                        m_Value = m_Minimum;
                    else if (value > m_Maximum)
                        m_Value = m_Maximum;
                    else
                        m_Value = value;

                    OnValueChanged();

                    if (ShouldSyncProperties)
                        BarFunctions.SyncProperty(this, "Value");

                    this.Refresh();
                    OnAppearanceChanged();
                    ExecuteCommand();
                }
            }
        }

        /// <summary>
        /// Gets or sets the amount by which a call to the PerformStep method increases the current position of the slider. Value must be greater than 0.
        /// </summary>
        [DevCoBrowsable(true), Browsable(true), Description("Gets or sets the amount by which a call to the PerformStep method increases the current position of the slider."), Category("Behavior"), DefaultValue(1)]
        public int Step
        {
            get
            {
                return m_Step;
            }
            set
            {
                if (m_Step <= 0) return;
                m_Step = value;
            }
        }

        /// <summary>
        /// Advances the current position of the slider by the amount of the Step property.
        /// </summary>
        public virtual void PerformStep()
        {
            this.Value += m_Step;
        }

        /// <summary>
        /// Advances the current position of the slider by the specified amount.
        /// </summary>
        /// <param name="value">The amount by which to increment the sliders current position. </param>
        public virtual void Increment(int value)
        {
            this.Value += value;
        }

        /// <summary>
        /// Gets or sets whether the text label next to the slider is displayed.
        /// </summary>
        [DevCoBrowsable(true), Browsable(true), Description("Gets or sets whether the label text is displayed."), Category("Behavior"), DefaultValue(true)]
        public bool LabelVisible
        {
            get
            {
                return m_LabelVisible;
            }
            set
            {
                m_LabelVisible = value;
                OnAppearanceChanged();
                this.Refresh();
            }
        }

        /// <summary>
        /// Gets or sets the width of the slider part of the item in pixels. Value must be greater than 0. Default value is 136.
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), DefaultValue(136), System.ComponentModel.Category("Layout"), System.ComponentModel.Description("Indicates the width of the slider part of the item in pixels.")]
        public int Width
        {
            get
            {
                return m_Width;
            }
            set
            {
                if (m_Width == value || value<=0)
                    return;
                m_Width = value;
                NeedRecalcSize = true;
                OnAppearanceChanged();
                this.Refresh();
            }
        }

        /// <summary>
        /// Gets or sets the width of the label part of the item in pixels. Value must be greater than 0. Default value is 38.
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), DefaultValue(38), System.ComponentModel.Category("Layout"), System.ComponentModel.Description("Indicates width of the label part of the item in pixels.")]
        public int LabelWidth
        {
            get
            {
                return m_LabelWidth;
            }
            set
            {
                if (m_LabelWidth == value || value<=0)
                    return;
                m_LabelWidth = value;
                NeedRecalcSize = true;
                OnAppearanceChanged();
                this.Refresh();
            }
        }

        /// <summary>
        /// Gets or sets the text label position in relationship to the slider. Default value is Left.
        /// </summary>
        [Browsable(true), DefaultValue(eSliderLabelPosition.Left), Category("Layout"), Description("Indicates text label position in relationship to the slider")]
        public eSliderLabelPosition LabelPosition
        {
            get { return m_LabelPosition; }
            set
            {
                m_LabelPosition = value;
                NeedRecalcSize = true;
                this.OnAppearanceChanged();
                this.Refresh();
            }
        }

        /// <summary>
        /// Raises the ValueChanged event.
        /// </summary>
        protected virtual void OnValueChanged()
        {
            if (ValueChanged != null)
                ValueChanged(this, new EventArgs());
        }

        /// <summary>
        /// Raises the ValueChanging event.
        /// </summary>
        protected virtual void OnValueChanging(CancelIntValueEventArgs e)
        {
            if (ValueChanging != null)
                ValueChanging(this, e);
        }

        /// <summary>
        /// Gets or sets whether Click event will be auto repeated when mouse button is kept pressed over the item.
        /// </summary>
        [Browsable(false), DevCoBrowsable(false), DefaultValue(false), Category("Behavior"), Description("Gets or sets whether Click event will be auto repeated when mouse button is kept pressed over the item.")]
        public override bool ClickAutoRepeat
        {
            get
            {
                return base.ClickAutoRepeat;
            }
            set
            {
                base.ClickAutoRepeat = value;
            }
        }

        /// <summary>
        /// Gets or sets the auto-repeat interval for the click event when mouse button is kept pressed over the item.
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), System.ComponentModel.DefaultValue(200), System.ComponentModel.Category("Behavior"), System.ComponentModel.Description("Gets or sets the auto-repeat interval for the click event when mouse button is kept pressed over the item.")]
        public override int ClickRepeatInterval
        {
            get
            {
                return base.ClickRepeatInterval;
            }
            set
            {
                base.ClickRepeatInterval = value;
            }
        }

        /// <summary>
        /// Gets or sets whether vertical line track marker is displayed on the slide line. Default value is true.
        /// </summary>
        [Browsable(true), Category("Appearance"), DefaultValue(true), Description("Indicates whether vertical line track marker is displayed on the slide line.")]
        public virtual bool TrackMarker
        {
            get { return m_TrackMarker; }
            set
            {
                m_TrackMarker = value;
                this.Refresh();
            }
        }

        /// <summary>
        /// Gets or sets the color of the label text.
        /// </summary>
        [Browsable(true), Category("Appearance"), Description("Indicates color of the label text.")]
        public Color TextColor
        {
            get { return m_TextColor; }
            set
            {
                m_TextColor = value;
                this.Refresh();
            }
        }
        /// <summary>
        /// Returns whether property should be serialized. Used by Windows Forms designer.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeTextColor()
        {
            return !m_TextColor.IsEmpty;
        }
        /// <summary>
        /// Resets the property to default value. Used by Windows Forms designer.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetTextColor()
        {
            TextColor = Color.Empty;
        }

        /// <summary>
        /// Gets or sets the Key Tips access key or keys for the item when on Ribbon Control or Ribbon Bar. Use KeyTips property
        /// when you want to assign the one or more letters to be used to access an item. For example assigning the FN to KeyTips property
        /// will require the user to press F then N keys to select an item. Pressing the F letter will show only keytips for the items that start with letter F.
        /// </summary>
        [Browsable(false), Category("Appearance"), DefaultValue(""), Description("Indicates the Key Tips access key or keys for the item when on Ribbon Control or Ribbon Bar.")]
        public override string KeyTips
        {
            get { return base.KeyTips; }
            set
            {
                base.KeyTips = value;
            }
        }

        /// <summary>
        /// Gets or sets the collection of shortcut keys associated with the item.
        /// </summary>
        [Browsable(false), DevCoBrowsable(false), System.ComponentModel.Category("Design"), System.ComponentModel.Description("Indicates list of shortcut keys for this item."), System.ComponentModel.Editor("DevComponents.DotNetBar.Design.ShortcutsDesigner, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(System.Drawing.Design.UITypeEditor)), System.ComponentModel.TypeConverter("DevComponents.DotNetBar.Design.ShortcutsConverter, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf"), System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Content)]
        public override ShortcutsCollection Shortcuts
        {
            get
            {
                return base.Shortcuts;
            }
            set
            {
                base.Shortcuts = value;
            }
        }

        /// <summary>
        /// Gets or sets whether item will display sub items.
        /// </summary>
        [Browsable(false), DevCoBrowsable(false), System.ComponentModel.DefaultValue(true), System.ComponentModel.Category("Behavior"), System.ComponentModel.Description("Determines whether sub-items are displayed.")]
        public override bool ShowSubItems
        {
            get
            {
                return base.ShowSubItems;
            }
            set
            {
                base.ShowSubItems = value;
            }
        }

        /// <summary>
        /// Specifies whether item is drawn using Themes when running on OS that supports themes like Windows XP.
        /// </summary>
        [Browsable(false), DevCoBrowsable(false), DefaultValue(false), Category("Appearance"), Description("Specifies whether item is drawn using Themes when running on OS that supports themes like Windows XP.")]
        public override bool ThemeAware
        {
            get
            {
                return base.ThemeAware;
            }
            set
            {
                base.ThemeAware = value;
            }
        }

        /// <summary>
        /// Gets or sets whether the item expands automatically to fill out the remaining space inside the container. Applies to Items on stretchable, no-wrap Bars only.
        /// </summary>
        [Browsable(false), DevCoBrowsable(false), System.ComponentModel.DefaultValue(false), System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("Indicates whether item will stretch to consume empty space. Items on stretchable, no-wrap Bars only.")]
        public override bool Stretch
        {
            get
            {
                return base.Stretch;
            }
            set
            {
                base.Stretch = value;
            }
        }

        /// <summary>
        /// Gets or sets the tooltip for the Increase button of the slider.
        /// </summary>
        [Browsable(true), DefaultValue(""), Localizable(true), Category("Appearance"), Description("Indicates tooltip for the Increase button of the slider.")]
        public string IncreaseTooltip
        {
            get { return m_IncreaseTooltip; }
            set { m_IncreaseTooltip = value; UpdateTooltip(); }
        }

        /// <summary>
        /// Gets or sets the tooltip for the Decrease button of the slider.
        /// </summary>
        [Browsable(true), DefaultValue(""), Localizable(true), Category("Appearance"), Description("Indicates tooltip for the Increase button of the slider.")]
        public string DecreaseTooltip
        {
            get { return m_DecreaseTooltip; }
            set { m_DecreaseTooltip = value; UpdateTooltip(); }
        }

        protected override void OnTooltipChanged()
        {
            m_ItemTooltip = this.Tooltip;
            UpdateTooltip();
            base.OnTooltipChanged();
        }

        private void UpdateTooltip()
        {
            if (this.DesignMode) return;

            if (m_MouseOverPart == eSliderPart.DecreaseButton && m_DecreaseTooltip != "")
            {
                SetInternalTooltip(m_DecreaseTooltip);
            }
            else if (m_MouseOverPart == eSliderPart.IncreaseButton && m_IncreaseTooltip != "")
                SetInternalTooltip(m_IncreaseTooltip);
            else if (m_Tooltip != m_ItemTooltip)
                SetInternalTooltip(m_ItemTooltip);
        }

        private void SetInternalTooltip(string t)
        {
            if (m_Tooltip != t)
            {
                m_Tooltip = t;
                if (this.ToolTipVisible)
                    this.ShowToolTip();
            }
        }

        private eOrientation _SliderOrientation = eOrientation.Horizontal;
        /// <summary>
        /// Gets or sets the slider orientation. Default value is horizontal.
        /// </summary>
        [DefaultValue(eOrientation.Horizontal), Category("Appearance"), Description("Indicates slider orientation.")]
        public eOrientation SliderOrientation
        {
            get { return _SliderOrientation; }
            set 
            {
                if (_SliderOrientation != value)
                {
                    _SliderOrientation = value;
                    NeedRecalcSize = true;
                    this.Refresh();
                }
            }
        }
        #endregion
    }

}
