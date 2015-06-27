using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using DevComponents.DotNetBar.Rendering;
using DevComponents.Editors;
using DevComponents.DotNetBar.TextMarkup;

namespace DevComponents.DotNetBar.Controls
{
	/// <summary>
	/// Represents enhanced Windows combo box control.
	/// </summary>
    [ToolboxBitmap(typeof(ComboBoxEx), "Controls.ComboBoxEx.ico"), ToolboxItem(true), Designer("DevComponents.DotNetBar.Design.ComboBoxExDesigner, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
	public class ComboBoxEx : System.Windows.Forms.ComboBox, ICommandSource
	{
		/// <summary>
		/// Represents the method that will handle the DropDownChange event.
		/// </summary>
		public delegate void OnDropDownChangeEventHandler(object sender, bool Expanded);
		/// <summary>
		/// Occurs when drop down portion of combo box is shown or hidden.
		/// </summary>
		public event OnDropDownChangeEventHandler DropDownChange;

		private eDotNetBarStyle m_Style=eDotNetBarStyle.Office2007;
		private bool m_DefaultStyle=false;			// Disables our drawing in WndProc
		private bool m_MouseOver=false;
        private bool m_MouseOverThumb=false;
		private bool m_DroppedDown=false;
		//private System.Windows.Forms.Timer m_Timer;
		private bool m_WindowsXPAware=false;
		private bool m_DisableInternalDrawing=false;
		private ImageList m_ImageList=null;
		private int m_DropDownHeight=0;
		private IntPtr m_LastFocusWindow;
		private IntPtr m_DropDownHandle=IntPtr.Zero;
		private ComboTextBoxMsgHandler m_TextWindowMsgHandler=null;
        //private ComboListBoxMsgHandler m_ListBoxMsgHandler = null;

		[DllImport("user32")]
		private static extern bool ValidateRect(IntPtr hWnd,ref NativeFunctions.RECT pRect);

		[DllImport("user32",SetLastError=true, CharSet=CharSet.Auto)]
		private static extern IntPtr GetWindow(IntPtr hWnd, uint uCmd);
		private const uint GW_CHILD=5;

		private bool m_PreventEnterBeep=false;

        private string m_WatermarkText = "";
        private bool m_Focused = false;
        private Font m_WatermarkFont = null;
        private Color m_WatermarkColor = SystemColors.GrayText;
        private bool m_IsStandalone = true;
        private Timer m_MouseOverTimer = null;
        private Rectangle m_ThumbRect = Rectangle.Empty;
        private bool _FocusHighlightEnabled = false;
        private static Color _DefaultHighlightColor = Color.FromArgb(0xFF, 0xFF, 0x88);
        private Color _FocusHighlightColor = _DefaultHighlightColor;

		/// <summary>
		/// Creates new instance of ComboBoxEx.
		/// </summary>
		public ComboBoxEx():base()
		{
			if(!ColorFunctions.ColorsLoaded)
			{
				NativeFunctions.RefreshSettings();
				NativeFunctions.OnDisplayChange();
				ColorFunctions.LoadColors();
			}
            m_MouseOverTimer = new Timer();
            m_MouseOverTimer.Interval = 10;
            m_MouseOverTimer.Enabled = false;
            m_MouseOverTimer.Tick += new EventHandler(MouseOverTimerTick);
#if FRAMEWORK20
            this.FlatStyle = FlatStyle.Flat;
#endif
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

        /// <summary>
        /// Gets or sets whether control is stand-alone control. Stand-alone flag affects the appearance of the control in Office 2007 style.
        /// </summary>
        [Browsable(true), DefaultValue(true), Category("Appearance"), Description("Indicates the appearance of the control.")]
        public bool IsStandalone
        {
            get { return m_IsStandalone; }
            set
            {
                m_IsStandalone = value;
            }
        }

        private bool _WatermarkEnabled = true;
        /// <summary>
        /// Gets or sets whether watermark text is displayed when control is empty. Default value is true.
        /// </summary>
        [DefaultValue(true), Description("Indicates whether watermark text is displayed when control is empty.")]
        public virtual bool WatermarkEnabled
        {
            get { return _WatermarkEnabled; }
            set { _WatermarkEnabled = value; this.Invalidate(); }
        }

        /// <summary>
        /// Gets or sets the watermark (tip) text displayed inside of the control when Text is not set and control does not have input focus. This property supports text-markup.
        /// Note that WatermarkText is not compatible with the auto-complete feature of .NET Framework 2.0.
        /// </summary>
        [Browsable(true), DefaultValue(""), Localizable(true), Category("Appearance"), Description("Indicates watermark text displayed inside of the control when Text is not set and control does not have input focus."), Editor("DevComponents.DotNetBar.Design.TextMarkupUIEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(System.Drawing.Design.UITypeEditor))]
        public string WatermarkText
        {
            get { return m_WatermarkText; }
            set
            {
                if (value == null) value = "";
                m_WatermarkText = value;
                MarkupTextChanged();
                this.Invalidate();
            }
        }

        private eWatermarkBehavior _WatermarkBehavior = eWatermarkBehavior.HideOnFocus;
        /// <summary>
        /// Gets or sets the watermark hiding behaviour. Default value indicates that watermark is hidden when control receives input focus.
        /// </summary>
        [DefaultValue(eWatermarkBehavior.HideOnFocus), Category("Behavior"), Description("Indicates watermark hiding behaviour.")]
        public eWatermarkBehavior WatermarkBehavior
        {
            get { return _WatermarkBehavior; }
            set { _WatermarkBehavior = value; this.Invalidate(); }
        }

        private TextMarkup.BodyElement m_TextMarkup = null;
        private void MarkupTextChanged()
        {
            m_TextMarkup = null;

            if (!TextMarkup.MarkupParser.IsMarkup(ref m_WatermarkText))
                return;

            m_TextMarkup = TextMarkup.MarkupParser.Parse(m_WatermarkText);
            ResizeMarkup();
        }

        private void ResizeMarkup()
        {
            if (m_TextMarkup != null)
            {
                using (Graphics g = this.CreateGraphics())
                {
                    MarkupDrawContext dc = GetMarkupDrawContext(g);
                    m_TextMarkup.Measure(GetWatermarkBounds().Size, dc);
                    Size sz = m_TextMarkup.Bounds.Size;
                    m_TextMarkup.Arrange(new Rectangle(GetWatermarkBounds().Location, sz), dc);
                }
            }
        }
        private MarkupDrawContext GetMarkupDrawContext(Graphics g)
        {
            return new MarkupDrawContext(g, (m_WatermarkFont == null ? this.Font : m_WatermarkFont), m_WatermarkColor, this.RightToLeft == RightToLeft.Yes);
        }

        /// <summary>
        /// Gets or sets the watermark font.
        /// </summary>
        [Browsable(true), Category("Appearance"), Description("Indicates watermark font."), DefaultValue(null)]
        public Font WatermarkFont
        {
            get { return m_WatermarkFont; }
            set { m_WatermarkFont = value; this.Invalidate(true); }
        }

        /// <summary>
        /// Gets or sets the watermark text color.
        /// </summary>
        [Browsable(true), Category("Appearance"), Description("Indicates watermark text color.")]
        public Color WatermarkColor
        {
            get { return m_WatermarkColor; }
            set { m_WatermarkColor = value; this.Invalidate(true); }
        }
        /// <summary>
        /// Indicates whether property should be serialized by Windows Forms designer.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeWatermarkColor()
        {
            return m_WatermarkColor != SystemColors.GrayText;
        }
        /// <summary>
        /// Resets the property to default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetWatermarkColor()
        {
            this.WatermarkColor = SystemColors.GrayText;
        }

        /// <summary>
        /// Gets or sets the combo box background color. Note that in Office 2007 style back color of the control is automatically managed.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                base.BackColor = value;
            }
        }

        private bool m_UseCustomBackColor=false;
        /// <summary>
        /// Gets or sets whether the BackColor value you set is used instead of the style back color automatically provided by the control. Default
        /// value is false which indicates that BackColor property is automatically managed. Set this property to true and then set BackColor property
        /// to make control use your custom back color.
        /// </summary>
        [Browsable(false), DefaultValue(false)]
        public bool UseCustomBackColor
        {
            get { return m_UseCustomBackColor; }
            set { m_UseCustomBackColor = value; }
        }

		/// <summary>
		/// Gets or sets value indicating whether system combo box appearance is used. Default value is false.
		/// </summary>
		[Browsable(false),Category("Behavior"),Description("Makes Combo box appear the same as built-in Combo box."),DefaultValue(false)]
		public bool DefaultStyle
		{
			get
			{
				return m_DefaultStyle;
			}
			set
			{
				if(m_DefaultStyle!=value)
				{
					m_DefaultStyle=value;
                    this.Invalidate(true);
				}
			}
		}

		/// <summary>
		/// Gets or sets value indicating whether the combo box is draw using the Windows XP Theme manager when running on Windows XP or theme aware OS.
		/// </summary>
		[Browsable(false),Category("Behavior"),Description("When running on WindowsXP draws control using the Windows XP Themes if theme manager is enabled."),DefaultValue(false)]
		public bool ThemeAware
		{
			get
			{
				return m_WindowsXPAware;
			}
			set
			{
				if(m_WindowsXPAware!=value)
				{
					m_WindowsXPAware=value;
					if(!m_WindowsXPAware)
						m_DefaultStyle=false;
                    else if (m_WindowsXPAware && BarFunctions.ThemedOS)
                    {
                        m_DefaultStyle = true;
                        #if FRAMEWORK20
                        this.FlatStyle = FlatStyle.Standard;
                        #endif
                    }
				}
			}
		}

		/// <summary>
		/// Disables internal drawing support for the List-box portion of Combo-box. Default value is false which means that internal drawing code is used. If
        /// you plan to provide your own drawing for combo box items you must set this property to True.
		/// </summary>
		[Browsable(true),Category("Behavior"),Description("Disables internal drawing support for the List-box portion of Combo-box."), DefaultValue(false)]
		public bool DisableInternalDrawing
		{
			get
			{
				return m_DisableInternalDrawing;
			}
			set
			{
				if(m_DisableInternalDrawing!=value)
					m_DisableInternalDrawing=value;
			}
		}

		/// <summary>
		/// Gets or sets whether combo box generates the audible alert when Enter key is pressed.
		/// </summary>
		[System.ComponentModel.Browsable(true),DevCoBrowsable(true),System.ComponentModel.Category("Behavior"),System.ComponentModel.Description("Indicates whether combo box generates the audible alert when Enter key is pressed."),System.ComponentModel.DefaultValue(false)]
		public bool PreventEnterBeep
		{
			get
			{
				return m_PreventEnterBeep;
			}
			set
			{
				m_PreventEnterBeep=value;
			}
		}

		/// <summary>
		/// The ImageList control used by Combo box to draw images.
		/// </summary>
		[Browsable(true),Category("Behavior"),Description("The ImageList control used by Combo box to draw images."), DefaultValue(null)]
		public System.Windows.Forms.ImageList Images
		{
			get
			{
				return m_ImageList;
			}
			set
			{
				m_ImageList=value;
			}
		}

		/// <summary>
		/// Determines the visual style applied to the combo box when shown. Default style is Office 2007.
		/// </summary>
		[Browsable(true),Category("Appearance"), DefaultValue(eDotNetBarStyle.Office2007) ,Description("Determines the display of the item when shown.")]
		public eDotNetBarStyle Style
		{
			get
			{
				return m_Style;
			}
			set
			{
				m_Style=value;
                UpdateBackColor();
                this.Invalidate(true);
			}
		}

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            UpdateBackColor();
            SetupTextBoxMessageHandler();
            RemoveTheme(this.Handle);
        }

		/*protected override void OnHandleDestroyed(EventArgs e)
		{
			if(m_Timer!=null)
				m_Timer.Enabled=false;
			base.OnHandleDestroyed(e);
		}*/

        protected override void OnResize(EventArgs e)
        {
            ResizeMarkup();
            this.Invalidate();
            base.OnResize(e);
        }
        
        private void MouseOverTimerTick(object sender, EventArgs e)
        {
            bool over = false;
            
            if (this.IsHandleCreated && this.Visible)
            {
                WinApi.RECT rw = new WinApi.RECT();
                WinApi.GetWindowRect(this.Handle, ref rw);
                Rectangle r = rw.ToRectangle();
                if (r.Contains(Control.MousePosition))
                    over = true;
                Point p=this.PointToClient(Control.MousePosition);
                if (m_ThumbRect.Contains(p))
                    MouseOverThumb = true;
                else
                    MouseOverThumb = false;
            }

            if (!over)
            {
                SetMouseOverTimerEnabled(false);
                m_MouseOver = false;
                UpdateBackColor();
                this.Invalidate();
                if (this.ParentItem != null) this.ParentItem.HideToolTip();
            }
        }
        internal BaseItem ParentItem = null;

        private bool MouseOverThumb
        {
            get { return m_MouseOverThumb; }
            set
            {
                if (m_MouseOverThumb != value)
                {
                    m_MouseOverThumb = value;
                    this.Invalidate();
                }
            }
        }

		protected override void OnGotFocus(EventArgs e)
		{
			base.OnGotFocus(e);
            if (!m_MouseOver)
                m_MouseOver = true;
            UpdateBackColor();
            this.Invalidate(true);
		}

		protected override void OnLostFocus(EventArgs e)
		{
			//if(!this.ClientRectangle.Contains(this.PointToClient(Control.MousePosition)))
				//m_MouseOver=false;
            SetMouseOverTimerEnabled(true);
            //Color c = GetComboColors().Background;
            //if (this.BackColor != c)
                //this.BackColor = c;
			//if(!m_MouseOver)
			//	this.Refresh();
			m_LastFocusWindow=IntPtr.Zero;
			base.OnLostFocus(e);
		}

        private Timer m_DropDownTrackTimer = null;
        private void StartDropDownTrackTimer()
        {
            if (m_DropDownTrackTimer == null)
            {
                m_DropDownTrackTimer = new Timer();
                m_DropDownTrackTimer.Tick += new EventHandler(DropDownTrackTimerTick);
                m_DropDownTrackTimer.Interval = 200;
            }
            m_DropDownTrackTimer.Start();
        }
        private void StopDropDownTrackTimer()
        {
            Timer t = m_DropDownTrackTimer;
            m_DropDownTrackTimer = null;
            if (t != null)
            {
                t.Stop();
                t.Dispose();
            }
        }

        private void DropDownTrackTimerTick(object sender, EventArgs e)
        {
            DroppedDownInternal = (WinApi.SendMessage(this.Handle, (int)WinApi.WindowsMessages.CB_GETDROPPEDSTATE, IntPtr.Zero, IntPtr.Zero) != 0);
        }

        private bool DroppedDownInternal
        {
            get
            {
                return m_DroppedDown;
            }
            set
            {
                if (m_DroppedDown != value)
                {
                    StopDropDownTrackTimer();
                    m_DroppedDown = value;
                    this.Invalidate();

                    if (DropDownChange != null)
                        DropDownChange(this, m_DroppedDown);
                    #if !FRAMEWORK20
                    if(m_DroppedDown)
                    {
                        StartDropDownTrackTimer();
                    }
                    #endif
                    if (!m_DroppedDown)
                        ReleasePopupHandler();
                }
            }
        }

#if FRAMEWORK20
        protected override void OnDropDownClosed(EventArgs e)
        {
            DroppedDownInternal = false;
            m_SelectedIndexInternal = this.SelectedIndex;
            base.OnDropDownClosed(e);
        }
#endif

        protected override void OnDropDown(EventArgs e)
        {
            base.OnDropDown(e);
            //if (DropDownChange != null)
            //    this.DropDownChange(this, true);
            DroppedDownInternal = true;
        }

        [DllImport("user32", CharSet = CharSet.Unicode)]
        private static extern int SetWindowTheme(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)] String pszSubAppName, [MarshalAs(UnmanagedType.LPWStr)] String pszSubIdList);

        private void RemoveTheme(IntPtr handle)
        {
            bool isXp = false;
            if (System.Environment.Version.Major > 5)
                isXp = true;
            else if ((System.Environment.Version.Major == 5) &&
               (System.Environment.Version.Minor >= 1))
                isXp = true;
            if (isXp)
                SetWindowTheme(handle, " ", " ");
        }

        #region Internal Combo Colors
        private class InternalComboColors
        {
            public Color Background = SystemColors.Window;
            public Color Border = SystemColors.Window;
            public LinearGradientColorTable ThumbBackground = null;
            public Color ThumbText = SystemColors.ControlText;
            public LinearGradientColorTable ThumbBorderOuter = null;
            public LinearGradientColorTable ThumbBorderInner = null;
        }

        private InternalComboColors GetComboColors()
        {
            InternalComboColors c = new InternalComboColors();
            
            bool bFocus = (m_MouseOverThumb || this.Focused || this.DroppedDownInternal && this.DropDownStyle != ComboBoxStyle.Simple);
            if (bFocus && !this.Enabled)
                bFocus = false;

            if (BarFunctions.IsOffice2007Style(this.Style) && GlobalManager.Renderer is Office2007Renderer)
            {
                Office2007ComboBoxColorTable colorTable = ((Office2007Renderer)GlobalManager.Renderer).ColorTable.ComboBox;
                Office2007ComboBoxStateColorTable stateColors = (IsToolbarStyle && !m_MouseOver ? colorTable.Default : colorTable.DefaultStandalone);
                if (bFocus)
                {
                    if (this.DroppedDown)
                        stateColors = colorTable.DroppedDown;
                    else if(bFocus)
                        stateColors = colorTable.MouseOver;
                }

                c.Background = stateColors.Background;
                c.Border = stateColors.Border;
                c.ThumbBackground = stateColors.ExpandBackground;
                c.ThumbText = stateColors.ExpandText;
                c.ThumbBorderOuter = stateColors.ExpandBorderOuter;
                c.ThumbBorderInner = stateColors.ExpandBorderInner;
            }
            else
            {
                ColorScheme cs = new ColorScheme(this.Style);
                if (bFocus)
                {
                    if (DroppedDownInternal)
                    {
                        c.ThumbBackground = new LinearGradientColorTable(cs.ItemPressedBackground, cs.ItemPressedBackground2, cs.ItemPressedBackgroundGradientAngle);
                        c.Border = cs.ItemPressedBorder;
                        c.ThumbText = cs.ItemPressedText;
                    }
                    else
                    {
                        if (m_MouseOverThumb)
                            c.ThumbBackground = new LinearGradientColorTable(cs.ItemHotBackground, cs.ItemHotBackground2, cs.ItemHotBackgroundGradientAngle);
                        else
                            c.ThumbBackground = new LinearGradientColorTable(cs.BarBackground, cs.BarBackground2, cs.BarBackgroundGradientAngle);
                        c.Border = cs.ItemHotBorder;
                        c.ThumbText = cs.ItemHotText;
                    }
                }
                else
                {
                    c.ThumbBackground = new LinearGradientColorTable(cs.BarBackground, cs.BarBackground2, cs.BarBackgroundGradientAngle);
                    if (m_MouseOver || !IsToolbarStyle)
                    {
                        c.Border = cs.ItemHotBorder;
                    }
                }
            }

            if (_FocusHighlightEnabled && this.Enabled && this.Focused)
                c.Background = _FocusHighlightColor;
            else if (!this.Enabled)
            {
                c.Background = _DisabledBackColor.IsEmpty ? SystemColors.Control : _DisabledBackColor;
                c.ThumbText = _DisabledForeColor.IsEmpty ? SystemColors.ControlDark : _DisabledForeColor; ;
            }

            return c;
        }
        #endregion
        private ComboBoxPopupMsgHandler _PopupMsgHandler = null;
        private void ReleasePopupHandler()
        {
            ComboBoxPopupMsgHandler handler = _PopupMsgHandler;
            _PopupMsgHandler = null;
            if (handler != null)
            {
                handler.ReleaseHandle();
            }
        }
        private void CreatePopupHandler(IntPtr handle)
        {
            if (_PopupMsgHandler == null)
            {
                _PopupMsgHandler = new ComboBoxPopupMsgHandler();
                _PopupMsgHandler.AssignHandle(handle);
            }
        }
        protected override void WndProc(ref Message m)
        {
            const int WM_PAINT = 0xF;
            const int WM_PRINTCLIENT = 0x0318;
            const int WM_CTLCOLORLISTBOX = 0x0134;
            const int CB_ADDSTRING = 0x143;
            const int CB_INSERTSTRING = 0x14A;
            const int CB_DELETESTRING = 0x0144;

            WinApi.RECT rect = new WinApi.RECT();

            switch (m.Msg)
            {
                case WM_CTLCOLORLISTBOX:
                    if (BarFunctions.IsWindows7)
                    {
                        if (m_DropDownHandle != m.LParam)
                            ReleasePopupHandler();

                        if (_PopupMsgHandler == null)
                            CreatePopupHandler(m.LParam);
                    }

                    m_DropDownHandle = m.LParam;

                    if (m_DropDownHeight > 0 || this.Items.Count == 0)
                    {
                        WinApi.GetWindowRect(m.LParam, ref rect);
                        int height = m_DropDownHeight;
                        if (this.Items.Count == 0)
                            height = Math.Max(18, this.ItemHeight);
                        Point thisLocation = this.PointToScreen(Point.Empty);
                        if (rect.Top < thisLocation.Y)
                            rect.Top = thisLocation.Y + this.Height;
                        NativeFunctions.SetWindowPos(m.LParam, 0, rect.Left, rect.Top, rect.Width,
                                                     height, NativeFunctions.SWP_NOACTIVATE);
                    }
                    break;

                case (int)WinApi.WindowsMessages.CB_GETDROPPEDSTATE:
                    base.WndProc(ref m);

                    if (m.Result == IntPtr.Zero && DroppedDownInternal)
                        DroppedDownInternal = false;
                    return;

                case NativeFunctions.WM_SETFOCUS:
                    if (m.WParam != this.Handle)
                        m_LastFocusWindow = m.WParam;

                    break;

                case CB_ADDSTRING:
                    if (this.Items.Count > 0)
                    {
                        ComboItem cb = this.Items[this.Items.Count - 1] as ComboItem;

                        if (cb != null)
                            cb.m_ComboBox = this;
                    }
                    break;

                case CB_INSERTSTRING:
                    int index = m.WParam.ToInt32();

                    if (index >= 0 && index < this.Items.Count)
                    {
                        ComboItem cb = this.Items[index] as ComboItem;

                        if (cb != null)
                            cb.m_ComboBox = this;
                    }

                    m_SelectedIndexInternal = -1;
                    break;

                case CB_DELETESTRING:
                    m_SelectedIndexInternal = -1;
                    break;

                case NativeFunctions.WM_USER + 7:
                    if (this.DropDownStyle == ComboBoxStyle.DropDown && !m_Focused)
                        this.SelectionLength = 0;

                    this.Invalidate(true);
                    return;
            }

            if (m_DefaultStyle)
            {
                base.WndProc(ref m);
                return;
            }

            if ((m.Msg == WM_PAINT || m.Msg == WM_PRINTCLIENT) && DrawMode != DrawMode.Normal)
            {
                WinApi.GetWindowRect(m.HWnd, ref rect);
                Rectangle r = new Rectangle(0, 0, rect.Width, rect.Height);

                if (m.Msg == WM_PAINT)
                {
                    WinApi.PAINTSTRUCT ps = new WinApi.PAINTSTRUCT();

                    IntPtr hdc = WinApi.BeginPaint(m.HWnd, ref ps);

                    using (Graphics gHdc = Graphics.FromHdc(hdc))
                    {
                        using (BufferedBitmap bmp = new BufferedBitmap(gHdc, r))
                        {
                            PaintComboBox(bmp.Graphics, r);

                            bmp.Render(gHdc);
                        }
                    }

                    WinApi.EndPaint(m.HWnd, ref ps);
                }
                else
                {
                    using (Graphics g = Graphics.FromHdc(m.WParam))
                        PaintComboBox(g, r);
                }

                if (this.Parent is ItemControl)
                    ((ItemControl)this.Parent).UpdateKeyTipsCanvas();
            }
            else
            {
                base.WndProc(ref m);
            }
        }

        private void PaintComboBox(Graphics g, Rectangle r)
        {
            InternalComboColors colors = GetComboColors();

            PaintComboBackground(g, r, colors);
            PaintComboBorder(g, r, colors);

            Rectangle contentRect = PaintComboThumb(g, r, colors);

            int selectedIndex = SelectedIndex == -1 ? -1 : m_SelectedIndexInternal;

            if (m_SelectedIndexInternal == -1 && SelectedIndex >= 0)
                selectedIndex = SelectedIndex;

            if (DropDownStyle == ComboBoxStyle.DropDownList)
            {
                if (Items.Count > 0 && (uint)selectedIndex < Items.Count)
                {
                    DrawItemState state = DrawItemState.ComboBoxEdit;

                    if (Enabled == false)
                        state |= DrawItemState.Disabled;

                    else if (Focused == true)
                        state |= (DrawItemState.Focus | DrawItemState.Selected);

                    contentRect.Inflate(-1, -1);

                    OnDrawItem(new DrawItemEventArgs(g, Font, contentRect,
                                                     selectedIndex, state, ForeColor, BackColor));
                }

                if (ShouldDrawWatermark() == true)
                    DrawWatermark(g);
            }
        }

	    private Rectangle PaintComboThumb(Graphics g, Rectangle r, InternalComboColors colors)
        {
            Rectangle contentRect = r;
            contentRect.Inflate(-2, -2);

            if (this.DropDownStyle == ComboBoxStyle.Simple || !_RenderThumb) return contentRect;
            
            int thumbWidth = SystemInformation.HorizontalScrollBarThumbWidth;
            Rectangle thumbRect = new Rectangle(r.Width - thumbWidth, r.Y, thumbWidth, r.Height);
            if (RightToLeft == RightToLeft.Yes)
                thumbRect = new Rectangle(r.X + 1, r.Y + 1, thumbWidth, r.Height - 2);
            if (!this.IsToolbarStyle)
            {
                thumbRect.Y += 2;
                thumbRect.Height -= 4;
                thumbRect.Width -= 2; ;
                if (RightToLeft == RightToLeft.Yes)
                    thumbRect.X += 2;
            }
            else if (!BarFunctions.IsOffice2007Style(this.Style))
                thumbRect.Inflate(-1, -1);

            if (RightToLeft == RightToLeft.Yes)
            {
                int diff = thumbRect.Right - contentRect.X + 2;
                contentRect.Width -= diff;
                contentRect.X += diff;
            }
            else
            {
                int diff = contentRect.Right - thumbRect.X + 2;
                contentRect.Width -= diff;
            }
            //contentRect.Y++;
            //contentRect.Height--;

            if (!this.IsToolbarStyle && BarFunctions.IsOffice2007Style(this.Style))
            {
                Office2007ButtonItemPainter.PaintBackground(g, GetOffice2007StateColorTable(), thumbRect, RoundRectangleShapeDescriptor.RectangleShape);
            }
            else
            {
                if (colors.ThumbBackground != null)
                    DisplayHelp.FillRectangle(g, thumbRect, colors.ThumbBackground);
                if (colors.ThumbBorderOuter != null)
                    DisplayHelp.DrawGradientRectangle(g, thumbRect, colors.ThumbBorderOuter, 1);
                Rectangle innerBorder = thumbRect;
                innerBorder.Inflate(-1, -1);
                if (colors.ThumbBorderInner != null)
                    DisplayHelp.DrawGradientRectangle(g, innerBorder, colors.ThumbBorderInner, 1);
            }

            using(SolidBrush brush=new SolidBrush(colors.ThumbText))
                DrawArrow(thumbRect, g, brush);

            m_ThumbRect = thumbRect;
            return contentRect;
        }

        internal Rectangle GetThumbRect(Rectangle r)
        {
            if (this.DropDownStyle == ComboBoxStyle.Simple || !_RenderThumb)
                return (Rectangle.Empty);

            int thumbWidth = SystemInformation.HorizontalScrollBarThumbWidth;
            Rectangle thumbRect = new Rectangle(r.Width - thumbWidth, r.Y, thumbWidth, r.Height);

            if (RightToLeft == RightToLeft.Yes)
                thumbRect = new Rectangle(r.X + 1, r.Y + 1, thumbWidth, r.Height - 2);

            if (!this.IsToolbarStyle)
            {
                thumbRect.Y += 2;
                thumbRect.Height -= 4;
                thumbRect.Width -= 2;

                if (RightToLeft == RightToLeft.Yes)
                    thumbRect.X += 2;
            }
            else if (!BarFunctions.IsOffice2007Style(this.Style))
                thumbRect.Inflate(-1, -1);

            return (thumbRect);
        }

	    [Browsable(false)]
        public bool IsToolbarStyle
        {
            get { return !m_IsStandalone; }
        }

        private bool _RenderThumb = true;
        /// <summary>
        /// Gets or sets whether ComboBoxEx thumb button that displays drop-down is rendered. Default value is true.
        /// </summary>
        internal bool RenderThumb
        {
            get { return _RenderThumb; }
            set
            {
                _RenderThumb = value;
            }
        }
        

        private bool _RenderBorder = true;
        /// <summary>
        /// Gets or sets whether ComboBoxEx border is rendered. Default value is true.
        /// </summary>
        internal bool RenderBorder
        {
            get { return _RenderBorder; }
            set
            {
                _RenderBorder = value;
            }
        }
        

        protected Office2007ButtonItemStateColorTable GetOffice2007StateColorTable()
        {
            bool bFocus = (m_MouseOverThumb || this.DroppedDownInternal && this.DropDownStyle != ComboBoxStyle.Simple);

            if (GlobalManager.Renderer is Office2007Renderer)
            {
                Office2007ColorTable ct = ((Office2007Renderer)GlobalManager.Renderer).ColorTable;
                Office2007ButtonItemColorTable buttonColorTable = ct.ButtonItemColors[Enum.GetName(typeof(eButtonColor), eButtonColor.OrangeWithBackground)];
                if (!this.Enabled)
                    return buttonColorTable.Disabled;
                else if (this.DroppedDownInternal)
                    return buttonColorTable.Checked;
                else if (bFocus)
                    return buttonColorTable.MouseOver;
                else
                    return buttonColorTable.Default;
            }

            return null;
        }

        private void PaintComboBorder(Graphics g, Rectangle controlBounds, InternalComboColors colors)
        {
            if (!_RenderBorder) return;
            DisplayHelp.DrawRectangle(g, colors.Border, controlBounds);
        }

        private void PaintComboBackground(Graphics g, Rectangle controlBounds, InternalComboColors colors)
        {
            DisplayHelp.FillRectangle(g, controlBounds, colors.Background);
        }

        private bool ShouldDrawWatermark()
        {
            if (_WatermarkEnabled && this.Enabled && (!this.Focused || _WatermarkBehavior== eWatermarkBehavior.HideNonEmpty) && this.Text == "" && this.SelectedIndex == -1)
                return true;
            return false;
        }

        private void DrawWatermark(Graphics g)
        {
            if (m_TextMarkup != null)
            {
                MarkupDrawContext dc = GetMarkupDrawContext(g);
                m_TextMarkup.Render(dc);
            }
            else
            {
                eTextFormat tf = eTextFormat.Left | eTextFormat.VerticalCenter;

                if (this.RightToLeft == RightToLeft.Yes) tf |= eTextFormat.RightToLeft;
                //if (this.TextAlign == HorizontalAlignment.Left)
                //    tf |= eTextFormat.Left;
                //else if (this.TextAlign == HorizontalAlignment.Right)
                //    tf |= eTextFormat.Right;
                //else if (this.TextAlign == HorizontalAlignment.Center)
                //    tf |= eTextFormat.HorizontalCenter;
                tf |= eTextFormat.EndEllipsis;
                tf |= eTextFormat.WordBreak;
                TextDrawing.DrawString(g, m_WatermarkText, (m_WatermarkFont == null ? this.Font : m_WatermarkFont),
                    m_WatermarkColor, GetWatermarkBounds(), tf);
            }
        }

        private Rectangle GetWatermarkBounds()
        {
            if (this.DropDownStyle != ComboBoxStyle.DropDownList && m_TextWindowMsgHandler!=null)
            {
                WinApi.RECT rect = new WinApi.RECT();
                WinApi.GetWindowRect(m_TextWindowMsgHandler.Handle, ref rect);
                return new Rectangle(0, 0, rect.Width, rect.Height);
            }

            Rectangle r = new Rectangle(2, 0, this.Width - 2, this.Height);
            r.Inflate(-2, -1);
            int thumbSize = SystemInformation.HorizontalScrollBarThumbWidth;
            r.Width -= thumbSize;
            if (this.RightToLeft == RightToLeft.Yes)
                r.X += thumbSize;

            return r;
        }

        //private void CreateListBoxMsgHandler(IntPtr m_DropDownHandle)
        //{
        //    DisposeListBoxMsgHandler();
        //    m_ListBoxMsgHandler = new ComboListBoxMsgHandler();
        //    m_ListBoxMsgHandler.AssignHandle(m_DropDownHandle);
        //}

        //private void DisposeListBoxMsgHandler()
        //{
        //    if (m_ListBoxMsgHandler != null)
        //    {
        //        m_ListBoxMsgHandler.ReleaseHandle();
        //        m_ListBoxMsgHandler = null;
        //    }
        //}

		private void DrawArrow(Rectangle r, Graphics g, Brush b)
		{
			Point[] p=new Point[3];
			p[0].X=r.Left+(r.Width-4)/2;
			p[0].Y=r.Top+(r.Height-3)/2 + 1;
			p[1].X=p[0].X+5;
			p[1].Y=p[0].Y;
			p[2].X=p[0].X+2;
			p[2].Y=p[0].Y+3;
			g.FillPolygon(b,p);
		}

		/*private void OnTimer(object sender, EventArgs e)
		{
			bool bRefresh=false;

			if(m_DroppedDown && !this.DroppedDown)
			{
				m_DroppedDown=false;
				
				if(DropDownChange!=null)
					this.DropDownChange(this,false);

				m_DropDownHandle=IntPtr.Zero;
				bRefresh=true;
			}

			Point mousePos=this.PointToClient(Control.MousePosition);
			if(!this.ClientRectangle.Contains(mousePos))
			{
				if(m_MouseOver && !m_DroppedDown)
				{
					m_MouseOver=false;
					bRefresh=true;
				}
			}
			else if(!m_MouseOver)
			{
				m_MouseOver=true;
				bRefresh=true;
			}

			if(bRefresh)
				this.Refresh();
		}*/

		protected override void OnVisibleChanged(EventArgs e)
		{
			base.OnVisibleChanged(e);
			m_MouseOver=false;
            if (this.DropDownStyle == ComboBoxStyle.DropDown && this.Items.Count > 0 && this.Items[0] is ComboItem && this.DisplayMember!="")
            {
                string s = this.DisplayMember;
                this.DisplayMember = "";
                this.DisplayMember = s;
            }
            if (this.IsHandleCreated && !this.IsDisposed)
            {
                UpdateBackColor();
            }
		}

        private int m_SelectedIndexInternal = -1;

		protected override void OnSelectedIndexChanged(EventArgs e)
		{
            m_SelectedIndexInternal = this.SelectedIndex;
            
			if(!this.DroppedDownInternal)
			{
				if(DroppedDownInternal)
				{
					DroppedDownInternal=false;
				}
				if(!m_MouseOver)
                    this.Invalidate(true);
			}
            if (this.SelectedIndex == -1 && _WatermarkBehavior == eWatermarkBehavior.HideNonEmpty && m_WatermarkText.Length > 0 && this.Text == "")
                this.Invalidate(true);
            else if (this.DropDownStyle == ComboBoxStyle.DropDownList)
                this.Invalidate();
            
			base.OnSelectedIndexChanged(e);

            ExecuteCommand();
		}

        protected override void OnTextChanged(EventArgs e)
        {
            if (this.SelectedIndex == -1 && _WatermarkBehavior == eWatermarkBehavior.HideNonEmpty && m_WatermarkText.Length > 0 && this.Text == "")
                this.Invalidate(true);
            base.OnTextChanged(e);
        }

		/*protected override void OnEnabledChanged(EventArgs e)
		{
			base.OnEnabledChanged(e);
			SyncTimerEnabled();
		}*/

		/*private void SyncTimerEnabled()
		{
			if(this.Visible)
			{
				if(!m_Timer.Enabled && this.Enabled && !this.DesignMode)
					m_Timer.Enabled=true;
			}
			else
			{
				if(m_Timer.Enabled)
					m_Timer.Enabled=false;
			}
		}*/

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			/*if(m_Timer!=null)
			{
				if(m_Timer.Enabled)
					m_Timer.Enabled=false;
				m_Timer.Dispose();
				m_Timer=null;
			}*/
            if (m_MouseOverTimer != null)
            {
                m_MouseOverTimer.Enabled = false;
                m_MouseOverTimer.Dispose();
                m_MouseOverTimer = null;
            }
			m_DisableInternalDrawing=true;
			if(m_TextWindowMsgHandler!=null)
			{
				m_TextWindowMsgHandler.ReleaseHandle();
				m_TextWindowMsgHandler=null;
			}
            ReleasePopupHandler();
			base.Dispose( disposing );
		}

		protected override void OnFontChanged(EventArgs e)
		{
			base.OnFontChanged(e);
			if(this.Disposing || this.IsDisposed)
				return;
			if(!m_DisableInternalDrawing && this.DrawMode==DrawMode.OwnerDrawFixed)
			{
				if(this.IsHandleCreated && this.Parent!=null && !this.Parent.IsDisposed)
					this.ItemHeight=this.FontHeight + 1;
			}
		}

        [EditorBrowsable(EditorBrowsableState.Never)]
        public int GetFontHeight()
        {
            return this.FontHeight;
        }

		protected override void OnMeasureItem(MeasureItemEventArgs e)
		{
			base.OnMeasureItem(e);
			if(!m_DisableInternalDrawing)
			{
				if(this.DrawMode==DrawMode.OwnerDrawFixed)
				{
                    e.ItemHeight = this.ItemHeight - 2;
				}
				else
				{
					object o=this.Items[e.Index];
					if(o is ComboItem)
					{
						if(((ComboItem)o).IsFontItem)
							MeasureFontItem(e);
						else
						{
							Size sz=GetComboItemSize(o as ComboItem);
							e.ItemHeight=sz.Height;
							e.ItemWidth=sz.Width;
                            if (BarFunctions.IsOffice2007Style(m_Style))
                            {
                                e.ItemHeight += 6;
                                e.ItemWidth += 6;
                            }
						}
					}
				}
			}
		}

		protected override void OnDrawItem(DrawItemEventArgs e)
		{
			base.OnDrawItem(e);
			InternalDrawItem(e);
		}

		private void InternalDrawItem(DrawItemEventArgs e)
		{
			if(!m_DisableInternalDrawing && e.Index>=0)
			{
				object o=this.Items[e.Index];
				if(o is ComboItem)
					DrawComboItem(e);
				else
					DrawObjectItem(e);
			}
		}

		protected virtual Size GetComboItemSize(ComboItem item)
		{
			Size size=Size.Empty;
			if(BarFunctions.IsHandleValid(this))
			{
				Graphics g=this.CreateGraphics();
				try
				{
					size=GetComboItemSize(item, g);
				}
				finally
				{
					g.Dispose();
				}
			}
			return size;
		}

		protected virtual void DrawObjectItem(DrawItemEventArgs e)
		{
			Graphics g=e.Graphics;
			string text=GetItemText(this.Items[e.Index]);
            Color textColor = Color.Empty;
            Office2007ButtonItemStateColorTable ct = null;
            
            if (BarFunctions.IsOffice2007Style(m_Style))
            {
                Office2007ButtonItemColorTable bct = ((Office2007Renderer)GlobalManager.Renderer).ColorTable.ButtonItemColors[0];
                if ((e.State & DrawItemState.Selected) != 0 || (e.State & DrawItemState.HotLight) != 0)
                    ct = bct.MouseOver;
                else if ((e.State & DrawItemState.Disabled) != 0 || !this.Enabled)
                    ct = bct.Disabled;
                else
                    ct = bct.Default;

                //if (ct != null)
                //    textColor = ct.Text;
                if ((e.State & DrawItemState.Disabled) != 0 || !this.Enabled)
                    textColor = _DisabledForeColor.IsEmpty ? SystemColors.ControlDark : _DisabledForeColor;
                else
                    textColor = this.ForeColor;
                if ((e.State & DrawItemState.HotLight) != 0 || (e.State & DrawItemState.Selected) != 0)
                {
                    Rectangle r = e.Bounds;
                    r.Width--;
                    r.Height--;
                    Office2007ButtonItemPainter.PaintBackground(g, ct, r, RoundRectangleShapeDescriptor.RoundCorner2);
                }
                else
                    e.DrawBackground();
            }
            else
            {
                e.DrawBackground();

                if ((e.State & DrawItemState.HotLight) != 0 || (e.State & DrawItemState.Selected) != 0)
                {
                    g.FillRectangle(SystemBrushes.Highlight, e.Bounds);
                    textColor = SystemColors.HighlightText;
                }
                else if ((e.State & DrawItemState.Disabled) != 0 || (e.State & DrawItemState.Grayed) != 0)
                    textColor = _DisabledForeColor.IsEmpty ? SystemColors.ControlDark : _DisabledForeColor;
                else
                    textColor = SystemColors.ControlText;
            }

			if((e.State & DrawItemState.Focus)!=0)
                DrawFocusRectangle(e);
            Rectangle rText = e.Bounds;
            
            if ((e.State & DrawItemState.ComboBoxEdit) == DrawItemState.ComboBoxEdit)
            {
                //rText.Inflate(-1, 0);
                
            }
            else if (BarFunctions.IsOffice2007Style(m_Style))
                rText.Inflate(-3, 0);
            else
                rText.Inflate(-2, 0);
			TextDrawing.DrawString(g,text,this.Font,textColor,rText,eTextFormat.Default | eTextFormat.NoClipping | eTextFormat.NoPrefix);
		}

        private void DrawFocusRectangle(DrawItemEventArgs e)
        {
            if (_FocusCuesEnabled && ((e.State & DrawItemState.Focus) == DrawItemState.Focus) && ((e.State & DrawItemState.NoFocusRect) != DrawItemState.NoFocusRect))
            {
                Rectangle r = e.Bounds;
                //r.Width--;
                //r.Height--;
                ControlPaint.DrawFocusRectangle(e.Graphics, r, e.ForeColor, e.BackColor);
            }
        }

        private Color _DisabledForeColor = Color.Empty;
        /// <summary>
        /// Gets or sets the text color for the text in combo-box when control Enabled property is set to false.
        /// Setting this property is effective only for DropDownList ComboBox style.
        /// </summary>
        [Category("Appearance"), Description("Indicates text color for the text in combo-box when control Enabled property is set to false. Setting this property is effective only for DropDownList ComboBox style.")]
        public Color DisabledForeColor
        {
            get { return _DisabledForeColor; }
            set 
            { 
                _DisabledForeColor = value;
                if (!this.Enabled)
                    this.Invalidate();
            }
        }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool ShouldSerializeDisabledForeColor()
        {
            return !_DisabledForeColor.IsEmpty;
        }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public void ResetDisabledForeColor()
        {
            DisabledForeColor = Color.Empty;
        }

        private Color _DisabledBackColor = Color.Empty;
        /// <summary>
        /// Gets or sets the control background color when control is disabled. Default value is an empty color which indicates that system background color is used when control is disabled.
        /// </summary>
        [Description("Indicates control background color when control is disabled"), Category("Appearance")]
        public Color DisabledBackColor
        {
            get { return _DisabledBackColor; }
            set
            {
                if (_DisabledBackColor != value)
                {
                    _DisabledBackColor = value;
                    if (!this.Enabled) this.Invalidate();
                }
            }
        }
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeDisabledBackColor()
        {
            return !_DisabledBackColor.IsEmpty;
        }
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetDisabledBackColor()
        {
            DisabledBackColor = Color.Empty;
        }

		protected virtual void DrawComboItem(DrawItemEventArgs e)
		{
			ComboItem item=this.Items[e.Index] as ComboItem;
			if(item.IsFontItem)
			{
				this.DrawFontItem(e);
				return;
			}

			Graphics g=e.Graphics;
			Image img=null;
			Color clr;
            Color textColor = item.ForeColor;
            if (textColor.IsEmpty || (e.State & DrawItemState.Selected) != 0 || (e.State & DrawItemState.HotLight) != 0)
                textColor = this.ForeColor;

            if (item.ImageIndex >= 0 && m_ImageList != null && m_ImageList.Images.Count > item.ImageIndex)
                img = m_ImageList.Images[item.ImageIndex];
            else if (item.Image != null)
                img = item.Image;

            Office2007ButtonItemStateColorTable ct = null;
            if (BarFunctions.IsOffice2007Style(m_Style))
            {
                Office2007ButtonItemColorTable bct = ((Office2007Renderer)GlobalManager.Renderer).ColorTable.ButtonItemColors[0];
                if ((e.State & DrawItemState.Selected) != 0 || (e.State & DrawItemState.HotLight) != 0)
                    ct = bct.MouseOver;
                else if ((e.State & DrawItemState.Disabled) != 0 || !this.Enabled)
                    ct = bct.Disabled;
                else
                    ct = bct.Default;
                //if (ct == null)
                if (!this.Enabled)
                    textColor = _DisabledForeColor.IsEmpty ? SystemColors.ControlDark : _DisabledForeColor;
                //else
                //    textColor = SystemColors.ControlText;
                //else
                  //  textColor = ct.Text;
            }

			int contWidth=this.DropDownWidth;
			if(item.ImagePosition!=HorizontalAlignment.Center && img!=null)
			{
				contWidth-=img.Width;
				if(contWidth<=0)
					contWidth=this.DropDownWidth;
			}

			// Back Color
			if((e.State & DrawItemState.Selected)!=0 || (e.State & DrawItemState.HotLight)!=0)
			{
                if (BarFunctions.IsOffice2007Style(m_Style))
                    Office2007ButtonItemPainter.PaintBackground(g, ct, e.Bounds, RoundRectangleShapeDescriptor.RoundCorner2);
                else
                    e.DrawBackground();
                DrawFocusRectangle(e);
			}
			else
			{
				clr=item.BackColor;
				if(item.BackColor.IsEmpty)
					clr=e.BackColor;
                using(SolidBrush clrb=new SolidBrush(clr))
				    g.FillRectangle(clrb,e.Bounds);
			}

			// Draw Image
			Rectangle rImg=e.Bounds;
			Rectangle rText=e.Bounds;
            //if (e.State != DrawItemState.ComboBoxEdit)
            {
                if ((e.State & DrawItemState.ComboBoxEdit) == DrawItemState.ComboBoxEdit)
                {
                    if (img != null)
                    {
                        rText.X += 3;
                        rText.Width -= 3;
                    }
                    //rText.Inflate(-1, 0);
                    //rText.Y++;
                    //rText.Height--;
                }
                else if (BarFunctions.IsOffice2007Style(m_Style))
                    rText.Inflate(-3, 0);
                else
                    rText.Inflate(-2, 0);
            }
			if(img!=null)
			{
                rImg.Width=img.Width;
				rImg.Height=img.Height;
				if(item.ImagePosition==HorizontalAlignment.Left)
				{
					// Left
					if(e.Bounds.Height>img.Height)
						rImg.Y+=(e.Bounds.Height-img.Height)/2;
					rText.Width-=rImg.Width;
					rText.X+=rImg.Width;
				}
				else if(item.ImagePosition==HorizontalAlignment.Right)
				{
					// Right
					if(e.Bounds.Height>img.Height)
						rImg.Y+=(e.Bounds.Height-img.Height)/2;
					rImg.X=e.Bounds.Right-img.Width;
					rText.Width-=rImg.Width;
				}
				else
				{
					// Center
					rImg.X+=(e.Bounds.Width-img.Width)/2;
					rText.Y=rImg.Bottom;
				}
				g.DrawImage(img,rImg);
			}
			
			// Draw Text
			if(item.Text!="")
			{
				System.Drawing.Font f=e.Font;
				bool bDisposeFont=false;
				try
				{
					if(item.FontName!="")
					{
						f=new Font(item.FontName,item.FontSize,item.FontStyle);
						bDisposeFont=true;
					}
					else if(item.FontStyle!=f.Style)
					{
                        f = new Font(f, item.FontStyle);
						bDisposeFont=true;
					}
				}
				catch
				{
					f=e.Font;
					if(f==null)
					{
						f=System.Windows.Forms.SystemInformation.MenuFont.Clone() as Font;
						bDisposeFont=true;
					}
				}

                eTextFormat format = eTextFormat.Default | eTextFormat.NoClipping | eTextFormat.NoPrefix;
                if (item.TextFormat.Alignment == StringAlignment.Center)
                    format = eTextFormat.HorizontalCenter;
                else if (item.TextFormat.Alignment == StringAlignment.Far)
                    format = eTextFormat.Right;
                if (item.TextLineAlignment == StringAlignment.Center)
                    format |= eTextFormat.VerticalCenter;
                else if (item.TextLineAlignment == StringAlignment.Far)
                    format |= eTextFormat.Bottom;
                TextDrawing.DrawString(g, item.Text, f, textColor, rText, format);
                
				if(bDisposeFont)
					f.Dispose();
			}
			
		}

		protected virtual Size GetComboItemSize(ComboItem item, Graphics g)
		{
			if(this.DrawMode==DrawMode.OwnerDrawFixed)
				return new Size(this.DropDownWidth,this.ItemHeight);

			Size sz=Size.Empty;
			Size textSize=Size.Empty;
			Image img=null;
			if(item.ImageIndex>=0)
				img=m_ImageList.Images[item.ImageIndex];
			else if(item.Image!=null)
				img=item.Image;
			
			int contWidth=this.DropDownWidth;
			if(item.ImagePosition!=HorizontalAlignment.Center && img!=null)
			{
				contWidth-=img.Width;
				if(contWidth<=0)
					contWidth=this.DropDownWidth;
			}
			
			Font font=this.Font;
			if(item.FontName!="")
			{
				try
				{
					font=new Font(item.FontName,item.FontSize,item.FontStyle);
				}
				catch
				{
					font=this.Font;
				}
			}

            eTextFormat format = eTextFormat.Default;
            if (item.TextFormat.Alignment == StringAlignment.Center)
                format = eTextFormat.HorizontalCenter;
            else if (item.TextFormat.Alignment == StringAlignment.Far)
                format = eTextFormat.Right;
            if (item.TextLineAlignment == StringAlignment.Center)
                format |= eTextFormat.VerticalCenter;
            else if (item.TextLineAlignment == StringAlignment.Far)
                format |= eTextFormat.Bottom;

			textSize=TextDrawing.MeasureString(g,item.Text,font,this.DropDownWidth,format);
            textSize.Width += 2;
            sz.Width=textSize.Width;
			sz.Height=textSize.Height;
            if(sz.Width<this.DropDownWidth)
				sz.Width=this.DropDownWidth;

			if(item.ImagePosition==HorizontalAlignment.Center && img!=null)
                sz.Height+=img.Height;
			else if(img!=null && img.Height>sz.Height)
				sz.Height=img.Height;

			return sz;
		}

		/// <summary>
		/// Loads all fonts available on system into the combo box.
		/// </summary>
		public void LoadFonts()
		{
			this.Items.Clear();

			System.Drawing.Text.InstalledFontCollection colInstalledFonts = new System.Drawing.Text.InstalledFontCollection();
			FontFamily[] aFamilies = colInstalledFonts.Families;
			foreach(FontFamily ff in aFamilies)
			{
				ComboItem item=new ComboItem();
				item.IsFontItem=true;
				item.FontName=ff.GetName(0);
				item.FontSize=this.Font.Size;
				item.Text=ff.GetName(0);
				this.Items.Add(item);
			}
			this.DropDownWidth=this.Width*2;
		}

		private void DrawFontItem(DrawItemEventArgs e)
		{
			FontStyle[] styles=new FontStyle[4]{FontStyle.Regular,FontStyle.Bold,FontStyle.Italic,FontStyle.Bold | FontStyle.Italic};
            if (!BarFunctions.IsOffice2007Style(m_Style))
			    e.DrawBackground();
			string fontname = this.Items[e.Index].ToString();
			FontFamily family = new FontFamily(fontname);

			int iWidth = this.DropDownWidth/2-4;
			if(iWidth<=0)
				iWidth=this.Width;
			foreach(FontStyle style in styles)
			{
				if(family.IsStyleAvailable(style))
				{
                    eTextFormat format = eTextFormat.Default | eTextFormat.NoPrefix;
                    Color textColor = (e.State & DrawItemState.Selected) != 0 ? SystemColors.HighlightText : SystemColors.ControlText;

                    Office2007ButtonItemStateColorTable ct = null;
                    if (BarFunctions.IsOffice2007Style(m_Style))
                    {
                        Office2007ButtonItemColorTable bct = ((Office2007Renderer)GlobalManager.Renderer).ColorTable.ButtonItemColors[0];
                        if ((e.State & DrawItemState.Selected) != 0 || (e.State & DrawItemState.HotLight) != 0)
                            ct = bct.MouseOver;
                        else if ((e.State & DrawItemState.Disabled) != 0 || !this.Enabled)
                            ct = bct.Disabled;
                        else
                            ct = bct.Default;
                        //if (ct == null)
                        if (!this.Enabled)
                            textColor = _DisabledForeColor.IsEmpty ? SystemColors.ControlDark : _DisabledForeColor;
                        else
                            textColor = SystemColors.ControlText;

                        if ((e.State & DrawItemState.Selected) != 0 || (e.State & DrawItemState.HotLight) != 0)
                            Office2007ButtonItemPainter.PaintBackground(e.Graphics, ct, e.Bounds, RoundRectangleShapeDescriptor.RoundCorner2);
                        else
                        {
                            e.DrawBackground();
                        }
                    }

                    if ((e.State & DrawItemState.ComboBoxEdit) == DrawItemState.ComboBoxEdit)
                    {
                        Rectangle rt = e.Bounds;
                        //rt.Y += 1;
                        //rt.Height -= 1;
                        TextDrawing.DrawString(e.Graphics, fontname, this.Font, textColor, rt, format);
                    }
                    else
                    {
                        Size szFont = TextDrawing.MeasureString(e.Graphics, fontname, this.Font);
                        int iDiff = (int)((e.Bounds.Height - szFont.Height) / 2);
                        Rectangle rFontName = new Rectangle(e.Bounds.X, e.Bounds.Y + iDiff, 
                            ((e.State & DrawItemState.Disabled)==DrawItemState.Disabled?e.Bounds.Width: Math.Max(e.Bounds.Width - 100, 32)), e.Bounds.Height - iDiff);
                        TextDrawing.DrawString(e.Graphics, fontname, this.Font, textColor, rFontName, format);
                        Rectangle rRemainder = new Rectangle(e.Bounds.X + iWidth + 4, e.Bounds.Y, e.Bounds.Width + 100, e.Bounds.Height);
                        using(Font f = new Font(family, (float)e.Bounds.Height - 8, style))
                            TextDrawing.DrawString(e.Graphics, fontname, f, textColor, rRemainder, format);
                    }
					break;
				}
			}
            if (family != null) family.Dispose();
		}

		private void MeasureFontItem(MeasureItemEventArgs e)
		{
			e.ItemHeight = 18;
		}

#if !FRAMEWORK20
		/// <summary>
		/// Specifies the height of the drop-down portion of combo box.
		/// </summary>
		[Browsable(true),Category("Behavior"),Description("The height, in pixels, of drop down box in a combo box."), DefaultValue(0)]
		public int DropDownHeight
		{
			get
			{
				return m_DropDownHeight;
			}
			set
			{
				m_DropDownHeight=value;
			}
		}
#endif

		/// <summary>
		/// Releases the focus from combo box. The control that last had focus will receive focus back when this method is called.
		/// </summary>
		public void ReleaseFocus()
		{
			if(this.Focused && m_LastFocusWindow!=IntPtr.Zero)
			{
				Control ctrl=Control.FromChildHandle(new System.IntPtr(m_LastFocusWindow.ToInt32()));
				if(ctrl!=this)
				{
					if(ctrl!=null)
						ctrl.Focus();
					else
					{
						NativeFunctions.SetFocus(m_LastFocusWindow.ToInt32());
					}
					this.OnLostFocus(new System.EventArgs());
				}
				m_LastFocusWindow=IntPtr.Zero;
				
			}
		}

		protected override bool ProcessCmdKey(ref Message msg,Keys keyData)
		{
			if(keyData==Keys.Enter && m_PreventEnterBeep)
			{
				this.OnKeyPress(new KeyPressEventArgs((char)13));
				return true;
			}
			return base.ProcessCmdKey(ref msg, keyData);
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
            if (!IsStandalone)
            {
                if (e.KeyCode == Keys.Enter)
                    ReleaseFocus();
                else if (e.KeyCode == Keys.Escape)
                {
                    ReleaseFocus();
                }
            }

			base.OnKeyDown(e);
		}

		/// <summary>
		/// Gets the window handle that the drop down list is bound to.
		/// </summary>
		[Browsable(false)]
		public IntPtr DropDownHandle
		{
			get
			{
				return m_DropDownHandle;
			}
		}

		internal bool MouseOver
		{
			get
			{
				return m_MouseOver;
			}
			set
			{
				if(m_MouseOver!=value)
				{
					m_MouseOver=value;
					this.Invalidate(true);
				}
			}
		}

        [System.ComponentModel.Editor("DevComponents.DotNetBar.Design.ComboItemsEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(System.Drawing.Design.UITypeEditor)), Localizable(true)]
		public new ComboBox.ObjectCollection Items
		{
            get { return base.Items; }
		}

        protected override void OnEnabledChanged(EventArgs e)
        {
            UpdateBackColor();
            base.OnEnabledChanged(e);
        }

        private void UpdateBackColor()
        {
            if (!m_UseCustomBackColor)
            {
                Color c = GetComboColors().Background;
                if (this.BackColor != c)
                    this.BackColor = c;
            }
        }

		protected override void OnMouseEnter(EventArgs e)
		{
			if(!m_MouseOver)
			{
				m_MouseOver=true;
                UpdateBackColor();
                this.Invalidate(true);
			}
            SetMouseOverTimerEnabled(true);
			base.OnMouseEnter(e);
		}
		protected override void OnMouseLeave(EventArgs e)
		{
            //if(this.DroppedDown)
            //{
            //    m_MouseOver=false;
            //}
            //else if(this.DropDownStyle!=ComboBoxStyle.DropDownList && m_MouseOver)
            //{
            //    // Get the mouse position
            //    Point p=this.PointToClient(Control.MousePosition);
            //    if(!this.ClientRectangle.Contains(p))
            //    {
            //        m_MouseOver=false;
            //    }
            //}
            //else if(m_MouseOver)
            //{
            //    m_MouseOver=false;
            //}
            SetMouseOverTimerEnabled(true);
            //Color c = GetComboColors().Background;
            //if (this.BackColor != c)
            //    this.BackColor = c;
            //this.Refresh();
			base.OnMouseLeave(e);
		}

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (!m_MouseOver)
            {
                //if (!m_Focused)
                {
                    m_MouseOver = true;
                    this.Invalidate();
                    SetMouseOverTimerEnabled(true);
                }
            }
        }

        private void SetMouseOverTimerEnabled(bool value)
        {
            if (m_MouseOverTimer != null) m_MouseOverTimer.Enabled = value;
        }

        private void SetupTextBoxMessageHandler()
        {
#if FRAMEWORK20
            if (this.DropDownStyle != ComboBoxStyle.DropDownList && this.AutoCompleteMode == AutoCompleteMode.None)
#else
            if (this.DropDownStyle != ComboBoxStyle.DropDownList)
#endif
            {
                if (m_TextWindowMsgHandler == null)
                {
                    // Get hold of the text box
                    IntPtr hwnd = GetWindow(this.Handle, GW_CHILD);
                    // Subclass so we can track messages
                    if (hwnd != IntPtr.Zero)
                    {
                        m_TextWindowMsgHandler = new ComboTextBoxMsgHandler();
                        m_TextWindowMsgHandler.MouseLeave += new EventHandler(this.TextBoxMouseLeave);
                        m_TextWindowMsgHandler.Paint += new PaintEventHandler(TextBoxPaint);
                        m_TextWindowMsgHandler.AssignHandle(hwnd);
                    }
                }
            }
            else if (m_TextWindowMsgHandler != null)
            {
                m_TextWindowMsgHandler.ReleaseHandle();
                m_TextWindowMsgHandler = null;
            }
        }
		
		protected override void OnDropDownStyleChanged(EventArgs e)
		{
            SetupTextBoxMessageHandler();
			base.OnDropDownStyleChanged(e);
		}

        private void TextBoxPaint(object sender, PaintEventArgs e)
        {
            if (ShouldDrawWatermark())
                DrawWatermark(e.Graphics);
        }

		private void TextBoxMouseLeave(object sender, EventArgs e)
		{
			if(!m_MouseOver)
				return;
            SetMouseOverTimerEnabled(true);
		}

        /// <summary>
        /// Gets or sets whether FocusHighlightColor is used as background color to highlight text box when it has input focus. Default value is false.
        /// </summary>
        [DefaultValue(false), Browsable(true), Category("Appearance"), Description("Indicates whether FocusHighlightColor is used as background color to highlight text box when it has input focus.")]
        public virtual bool FocusHighlightEnabled
        {
            get { return _FocusHighlightEnabled; }
            set
            {
                if (_FocusHighlightEnabled != value)
                {
                    _FocusHighlightEnabled = value;
                    if (this.Focused)
                        this.Invalidate();
                }
            }
        }

        /// <summary>
        /// Gets or sets the color used as background color to highlight text box when it has input focus and focus highlight is enabled.
        /// </summary>
        [Browsable(true), Category("Appearance"), Description("Indicates color used as background color to highlight text box when it has input focus and focus highlight is enabled.")]
        public virtual Color FocusHighlightColor
        {
            get { return _FocusHighlightColor; }
            set
            {
                if (_FocusHighlightColor != value)
                {
                    _FocusHighlightColor = value;
                    if (this.Focused)
                        this.Invalidate();
                }
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeFocusHighlightColor()
        {
            return !_FocusHighlightColor.Equals(_DefaultHighlightColor);
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetFocusHighlightColor()
        {
            FocusHighlightColor = _DefaultHighlightColor;
        }

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
        

		private class ComboTextBoxMsgHandler:NativeWindow
		{
			private const int WM_MOUSELEAVE=0x02A3;
			private const int WM_MOUSEMOVE=0x0200;
			private const int TME_LEAVE=0x02;
			[DllImport("user32",SetLastError=true, CharSet=CharSet.Auto)]
			private static extern bool TrackMouseEvent(ref TRACKMOUSEEVENT lpEventTrack);

			public event EventHandler MouseLeave;
            public event PaintEventHandler Paint;

			private struct TRACKMOUSEEVENT 
			{
				public int cbSize;
				public int dwFlags;
				public int hwndTrack;
				public int dwHoverTime;
			}

			private bool m_MouseTracking=false;

			protected override void WndProc(ref Message m)
			{
                const int WM_PAINT = 0xF;
				if(m.Msg==WM_MOUSEMOVE && !m_MouseTracking)
				{
                    m_MouseTracking = true;
                    TRACKMOUSEEVENT tme = new TRACKMOUSEEVENT();
                    tme.dwFlags = TME_LEAVE;
                    tme.cbSize = Marshal.SizeOf(tme);
                    tme.hwndTrack = this.Handle.ToInt32();
                    tme.dwHoverTime = 0;
                    m_MouseTracking = TrackMouseEvent(ref tme);
				}
				else if(m.Msg==WM_MOUSELEAVE)
				{
					if(MouseLeave!=null)
						MouseLeave(this,new EventArgs());
					m_MouseTracking=false;
				}
                else if (m.Msg == WM_PAINT)
                {
                    base.WndProc(ref m);
                    if (Paint != null)
                    {
                        using (Graphics g = Graphics.FromHwnd(m.HWnd))
                            Paint(this, new PaintEventArgs(g, Rectangle.Empty));
                    }
                    return;
                }
				base.WndProc(ref m);
			}
		}

        private class ComboBoxPopupMsgHandler : NativeWindow
        {
            protected override void WndProc(ref Message m)
            {
                if (m.Msg == (int)WinApi.WindowsMessages.WM_VSCROLL)
                {
                    base.WndProc(ref m);
                    WinApi.RedrawWindow(m.HWnd, 
                        IntPtr.Zero, 
                        IntPtr.Zero, 
                        WinApi.RedrawWindowFlags.RDW_INVALIDATE | WinApi.RedrawWindowFlags.RDW_UPDATENOW);
                    return;
                }
                base.WndProc(ref m);
            }
        }
	}
}
