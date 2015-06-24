using System;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar
{
    public abstract class PopupItemControl: Control, IThemeCache, IMessageHandlerClient,
        IOwnerMenuSupport, IOwner, IRenderingSupport, IAccessibilitySupport, IOwnerLocalize
    {
        #region Events
        /// <summary>
        /// Occurs when popup of type container is loading.
        /// </summary>
        [Description("Occurs when popup of type container is loading.")]
        public event EventHandler PopupContainerLoad;

        /// <summary>
        /// Occurs when popup of type container is unloading.
        /// </summary>
        [Description("Occurs when popup of type container is unloading.")]
        public event EventHandler PopupContainerUnload;

        /// <summary>
        /// Occurs when popup item is about to open.
        /// </summary>
        [Description("Occurs when popup item is about to open.")]
        public event EventHandler PopupOpen;

        /// <summary>
        /// Occurs when popup item is closing.
        /// </summary>
        [Description("Occurs when popup item is closing.")]
        public event EventHandler PopupClose;

        /// <summary>
        /// Occurs just before popup window is shown.
        /// </summary>
        [Description("Occurs just before popup window is shown.")]
        public event EventHandler PopupShowing;

        /// <summary>
        /// Occurs when Control is looking for translated text for one of the internal text that are
        /// displayed on menus, toolbars and customize forms. You need to set Handled=true if you want
        /// your custom text to be used instead of the built-in system value.
        /// </summary>
        public event DotNetBarManager.LocalizeStringEventHandler LocalizeString;
        #endregion

        #region Private Variables
        private PopupItem m_PopupItem = null;
        private ColorScheme m_ColorScheme = null;
        private bool m_AntiAlias = false;
        private bool m_FilterInstalled = false;
        private bool m_MenuEventSupport = false;
        private bool m_MenuFocus = false;
        private Timer m_ActiveWindowTimer = null;
        private IntPtr m_ForegroundWindow = IntPtr.Zero;
        private IntPtr m_ActiveWindow = IntPtr.Zero;
        private Hashtable m_ShortcutTable = new Hashtable();
        private BaseItem m_ExpandedItem = null;
        private BaseItem m_FocusItem = null;
		private System.Windows.Forms.ImageList m_ImageList=null;
		private System.Windows.Forms.ImageList m_ImageListMedium=null;
		private System.Windows.Forms.ImageList m_ImageListLarge=null;
        private bool m_DisabledImagesGrayScale = true;

        // Theme Caching Support
        private ThemeWindow m_ThemeWindow = null;
        private ThemeRebar m_ThemeRebar = null;
        private ThemeToolbar m_ThemeToolbar = null;
        private ThemeHeader m_ThemeHeader = null;
        private ThemeScrollBar m_ThemeScrollBar = null;
        private ThemeExplorerBar m_ThemeExplorerBar = null;
        private ThemeProgress m_ThemeProgress = null;
        private ThemeButton m_ThemeButton = null;
        private BaseItem m_DoDefaultActionItem = null;
        
        #endregion

        #region Constructor
        public PopupItemControl()
        {
            PainterFactory.InitFactory();
            if (!ColorFunctions.ColorsLoaded)
            {
                NativeFunctions.RefreshSettings();
                NativeFunctions.OnDisplayChange();
                ColorFunctions.LoadColors();
            }
            
            m_PopupItem = CreatePopupItem();
            m_PopupItem.GlobalItem = false;
            m_PopupItem.ContainerControl = this;
            m_PopupItem.Style = eDotNetBarStyle.Office2007;
            m_PopupItem.SetOwner(this);
            m_ColorScheme = new ColorScheme(m_PopupItem.EffectiveStyle);

            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.Opaque, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(DisplayHelp.DoubleBufferFlag, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.SetStyle(ControlStyles.ContainerControl, false);

            this.SetStyle(ControlStyles.Selectable, true);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (m_PopupItem != null)
                {
                    m_PopupItem.Dispose();
                    //m_PopupItem = null; In designer the control keeps on being used after it has been disposed and can generate unpredictable results crashing the designer.
                }
            }
            base.Dispose(disposing);
        }

        protected abstract PopupItem CreatePopupItem();

        /// <summary>
        /// Notifies the accessibility client applications of the specified AccessibleEvents for the specified child control.
        /// </summary>
        /// <param name="accEvent">The AccessibleEvents object to notify the accessibility client applications of. </param>
        /// <param name="childID">The child Control to notify of the accessible event.</param>
        internal void InternalAccessibilityNotifyClients(AccessibleEvents accEvent, int childID)
        {
            this.AccessibilityNotifyClients(accEvent, childID);
        }
        #endregion

        #region Internal Implementation
        #if FRAMEWORK20
        protected override void OnBindingContextChanged(EventArgs e)
        {
            base.OnBindingContextChanged(e);
            if (m_PopupItem != null) m_PopupItem.UpdateBindings();
        }
        #endif
        protected override void OnFontChanged(EventArgs e)
        {
            if (m_PopupItem != null)
                BarUtilities.InvalidateFontChange(m_PopupItem);
            base.OnFontChanged(e);
            this.RecalcLayout();
        }
        
        protected override void OnEnabledChanged(EventArgs e)
        {
            if (m_PopupItem != null) m_PopupItem.Enabled = this.Enabled;
            base.OnEnabledChanged(e);
        }
       
        protected internal virtual ColorScheme GetColorScheme()
        {
            if (BarFunctions.IsOffice2007Style(this.Style))
            {
                BaseRenderer r = GetRenderer();
                if (r is Office2007Renderer)
                    return ((Office2007Renderer)r).ColorTable.LegacyColors;
            }
            return m_ColorScheme;
        }

        /// <summary>
        /// Forces the button to perform internal layout.
        /// </summary>
        public void RecalcLayout()
        {
            RecalcSize();
            this.Invalidate();
        }

        protected abstract void RecalcSize();
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void SetDesignMode(bool value)
        {
            if (m_PopupItem != null)
                m_PopupItem.SetDesignMode(value);
        }

        /// <summary>
        /// Gets/Sets the visual style for the button.
        /// </summary>
        [Category("Appearance"), Description("Specifies the visual style of the button."), DefaultValue(eDotNetBarStyle.Office2007)]
        public virtual eDotNetBarStyle Style
        {
            get
            {
                return m_PopupItem.Style;
            }
            set
            {
                m_ColorScheme.Style = value;
                m_PopupItem.Style = value;
                this.RecalcLayout();
            }
        }

        /// <summary>
        /// Creates the Graphics object for the control.
        /// </summary>
        /// <returns>The Graphics object for the control.</returns>
        public new Graphics CreateGraphics()
        {
            Graphics g = base.CreateGraphics();
            if (m_AntiAlias)
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
				#if FRAMEWORK20
                if (!SystemInformation.IsFontSmoothingEnabled)
                #endif
                    g.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
            }
            return g;
        }

        internal virtual ItemPaintArgs GetItemPaintArgs(Graphics g)
        {
            ItemPaintArgs pa = new ItemPaintArgs(this, this, g, GetColorScheme());
            pa.Renderer = this.GetRenderer();
            pa.ButtonStringFormat = pa.ButtonStringFormat & ~(pa.ButtonStringFormat & eTextFormat.SingleLine);
            pa.ButtonStringFormat |= (eTextFormat.WordBreak | eTextFormat.EndEllipsis);
            return pa;
        }

        private Rendering.BaseRenderer m_DefaultRenderer = null;
        private Rendering.BaseRenderer m_Renderer = null;
        private eRenderMode m_RenderMode = eRenderMode.Global;
        /// <summary>
        /// Returns the renderer control will be rendered with.
        /// </summary>
        /// <returns>The current renderer.</returns>
        public virtual Rendering.BaseRenderer GetRenderer()
        {
            if (m_RenderMode == eRenderMode.Global && Rendering.GlobalManager.Renderer != null)
                return Rendering.GlobalManager.Renderer;
            else if (m_RenderMode == eRenderMode.Custom && m_Renderer != null)
                return m_Renderer;

            if (m_DefaultRenderer == null)
            {
                if (BarFunctions.IsOffice2007Style(this.Style))
                    m_DefaultRenderer = new Rendering.Office2007Renderer();
                //else
                //    m_DefaultRenderer = new Rendering.Office12Renderer();
            }

            return m_Renderer;
        }

        /// <summary>
        /// Gets or sets the rendering mode used by control. Default value is eRenderMode.Global which means that static GlobalManager.Renderer is used. If set to Custom then Renderer property must
        /// also be set to the custom renderer that will be used.
        /// </summary>
        [Browsable(false), DefaultValue(eRenderMode.Global)]
        public eRenderMode RenderMode
        {
            get { return m_RenderMode; }
            set
            {
                if (m_RenderMode != value)
                {
                    m_RenderMode = value;
                    this.Invalidate(true);
                }
            }
        }

        /// <summary>
        /// Gets or sets the custom renderer used by the items on this control. RenderMode property must also be set to eRenderMode.Custom in order renderer
        /// specified here to be used.
        /// </summary>
        [Browsable(false), DefaultValue(null)]
        public DevComponents.DotNetBar.Rendering.BaseRenderer Renderer
        {
            get
            {
                return m_Renderer;
            }
            set { m_Renderer = value; }
        }

        /// <summary>
        /// Gets or sets whether anti-alias smoothing is used while painting. Default value is false.
        /// </summary>
        [DefaultValue(false), Browsable(true), Category("Appearance"), Description("Gets or sets whether anti-aliasing is used while painting.")]
        public bool AntiAlias
        {
            get { return m_AntiAlias; }
            set
            {
                if (m_AntiAlias != value)
                {
                    m_AntiAlias = value;
                    InvalidateAutoSize();
                    this.Invalidate();
                }
            }
        }

        protected virtual void InvalidateAutoSize()
        {
        }

        /// <summary>
        /// Gets or sets button Color Scheme. ColorScheme does not apply to Office2007 styled buttons.
        /// </summary>
        [Browsable(false), DevCoBrowsable(false), Category("Appearance"), Description("Gets or sets Bar Color Scheme."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ColorScheme ColorScheme
        {
            get { return m_ColorScheme; }
            set
            {
                if (value == null)
                    throw new ArgumentException("NULL is not a valid value for this property.");
                m_ColorScheme = value;
                if (this.Visible)
                    this.Invalidate();
            }
        }
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        private bool ShouldSerializeColorScheme()
        {
            return m_ColorScheme.SchemeChanged;
        }

        /// <summary>
        /// Specifies whether button is drawn using Windows Themes when running on OS that supports themes like Windows XP.
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), DefaultValue(false), Category("Appearance"), Description("Specifies whether button is drawn using Themes when running on OS that supports themes like Windows XP.")]
        public virtual bool ThemeAware
        {
            get { return m_PopupItem.ThemeAware; }
            set
            {
                m_PopupItem.ThemeAware = value;
                this.RecalcLayout();
            }
        }

        /// <summary>
        /// Gets whether Windows Themes should be used to draw the button.
        /// </summary>
        protected bool IsThemed
        {
            get
            {
                if (ThemeAware && m_PopupItem.EffectiveStyle != eDotNetBarStyle.Office2000 && BarFunctions.ThemedOS && Themes.ThemesActive)
                    return true;
                return false;
            }
        }
        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public BaseItem InternalItem
        {
            get { return m_PopupItem; }
        }

        #endregion

        #region IOwnerMenuSupport Implementation
        private bool GetDesignMode()
        {
            return this.DesignMode;
        }
        private Hook m_Hook = null;
        // IOwnerMenuSupport
        private ArrayList m_RegisteredPopups = new ArrayList();
        bool IOwnerMenuSupport.PersonalizedAllVisible { get { return false; } set { } }
        bool IOwnerMenuSupport.ShowFullMenusOnHover { get { return true; } set { } }
        bool IOwnerMenuSupport.AlwaysShowFullMenus { get { return false; } set { } }

        void IOwnerMenuSupport.RegisterPopup(PopupItem objPopup)
        {
            this.MenuFocus = true;

            if (m_RegisteredPopups.Contains(objPopup))
                return;

            if (!this.GetDesignMode())
            {
                if (!m_FilterInstalled)
                {
                    MessageHandler.RegisterMessageClient(this);
                    m_FilterInstalled = true;
                }
            }
            else
            {
                if (m_Hook == null)
                {
                    m_Hook = new Hook(this);
                }
            }

            if (!m_MenuEventSupport)
                MenuEventSupportHook();

            m_RegisteredPopups.Add(objPopup);
            if (objPopup.GetOwner() != this)
                objPopup.SetOwner(this);
        }
        void IOwnerMenuSupport.UnregisterPopup(PopupItem objPopup)
        {
            if (m_RegisteredPopups.Contains(objPopup))
                m_RegisteredPopups.Remove(objPopup);
            if (m_RegisteredPopups.Count == 0)
            {
                MenuEventSupportUnhook();
                if (m_Hook != null)
                {
                    m_Hook.Dispose();
                    m_Hook = null;
                }
                this.MenuFocus = false;
            }
        }
        bool IOwnerMenuSupport.RelayMouseHover()
        {
            foreach (PopupItem popup in m_RegisteredPopups)
            {
                Control ctrl = popup.PopupControl;
                if (ctrl != null && ctrl.DisplayRectangle.Contains(MousePosition))
                {
                    if (ctrl is MenuPanel)
                        ((MenuPanel)ctrl).InternalMouseHover();
                    else if (ctrl is Bar)
                        ((Bar)ctrl).InternalMouseHover();
                    return true;
                }
            }
            return false;
        }

        void IOwnerMenuSupport.ClosePopups()
        {
            ClosePopups();
        }

        private void ClosePopups()
        {
            ArrayList popupList = new ArrayList(m_RegisteredPopups);
            foreach (PopupItem objPopup in popupList)
                objPopup.ClosePopup();
        }

        // Events
        void IOwnerMenuSupport.InvokePopupClose(PopupItem item, EventArgs e)
        {
            if (PopupClose != null)
                PopupClose(item, e);
        }
        void IOwnerMenuSupport.InvokePopupContainerLoad(PopupItem item, EventArgs e)
        {
            if (PopupContainerLoad != null)
                PopupContainerLoad(item, e);
        }
        void IOwnerMenuSupport.InvokePopupContainerUnload(PopupItem item, EventArgs e)
        {
            if (PopupContainerUnload != null)
                PopupContainerUnload(item, e);
        }
        void IOwnerMenuSupport.InvokePopupOpen(PopupItem item, PopupOpenEventArgs e)
        {
            if (PopupOpen != null)
                PopupOpen(item, e);
        }
        void IOwnerMenuSupport.InvokePopupShowing(PopupItem item, EventArgs e)
        {
            if (PopupShowing != null)
                PopupShowing(item, e);
        }
        bool IOwnerMenuSupport.ShowPopupShadow { get { return true; } }
        eMenuDropShadow IOwnerMenuSupport.MenuDropShadow { get { return eMenuDropShadow.SystemDefault; } set { } }
        ePopupAnimation IOwnerMenuSupport.PopupAnimation { get { return ePopupAnimation.SystemDefault; } set { } }
        bool IOwnerMenuSupport.AlphaBlendShadow { get { return true; } set { } }

        internal bool MenuFocus
        {
            get
            {
                return m_MenuFocus;
            }
            set
            {
                if (m_MenuFocus != value)
                {
                    m_MenuFocus = value;
                    if (m_MenuFocus)
                    {
                        SetupActiveWindowTimer();
                    }
                    else
                    {
                        ReleaseActiveWindowTimer();
                        ClosePopups();
                    }
                    this.Invalidate();
                }
            }
        }
        #endregion

        #region Active Window Changed Handling
        /// <summary>
        /// Sets up timer that watches when active window changes.
        /// </summary>
        protected virtual void SetupActiveWindowTimer()
        {
            if (m_ActiveWindowTimer != null)
                return;
            m_ActiveWindowTimer = new Timer();
            m_ActiveWindowTimer.Interval = 100;
            m_ActiveWindowTimer.Tick += new EventHandler(ActiveWindowTimer_Tick);

            m_ForegroundWindow = NativeFunctions.GetForegroundWindow();
            m_ActiveWindow = NativeFunctions.GetActiveWindow();

            m_ActiveWindowTimer.Start();
        }

        private void ActiveWindowTimer_Tick(object sender, EventArgs e)
        {
            if (m_ActiveWindowTimer == null)
                return;

            IntPtr f = NativeFunctions.GetForegroundWindow();
            IntPtr a = NativeFunctions.GetActiveWindow();

            if (f != m_ForegroundWindow || a != m_ActiveWindow)
            {
                Control c = Control.FromChildHandle(a);
                if (c != null)
                {
                    do
                    {
                        if ((c is MenuPanel || c is Bar || c is PopupContainer || c is PopupContainerControl))
                            return;
                        c = c.Parent;
                    } while (c!=null && c.Parent != null);
                }
                m_ActiveWindowTimer.Stop();
                OnActiveWindowChanged();
            }
        }

        /// <summary>
        /// Called after change of active window has been detected. SetupActiveWindowTimer must be called to enable detection.
        /// </summary>
        protected virtual void OnActiveWindowChanged()
        {
            if (this.MenuFocus)
                this.MenuFocus = false;
        }

        /// <summary>
        /// Releases and disposes the active window watcher timer.
        /// </summary>
        protected virtual void ReleaseActiveWindowTimer()
        {
            if (m_ActiveWindowTimer != null)
            {
                Timer timer = m_ActiveWindowTimer;
                m_ActiveWindowTimer = null;
                timer.Stop();
                timer.Tick -= new EventHandler(ActiveWindowTimer_Tick);
                timer.Dispose();
            }
        }
        #endregion

        #region IMessageHandlerClient Implementation
        bool IMessageHandlerClient.IsModal
        {
            get
            {
                Form form = this.FindForm();
                if (form != null && form.Modal && Form.ActiveForm == form)
                    return true;
                return false;
            }
        }

        bool IMessageHandlerClient.OnMouseWheel(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
        {
            return OnMouseWheel(hWnd, wParam, lParam);
        }

        protected virtual bool OnMouseWheel(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
        {
            return false;
        }

        bool IMessageHandlerClient.OnKeyDown(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
        {
            return OnKeyDown(hWnd, wParam, lParam);
        }

        protected virtual bool OnKeyDown(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
        {
            if (m_RegisteredPopups.Count > 0)
            {
                if (((BaseItem)m_RegisteredPopups[m_RegisteredPopups.Count - 1]).Parent == null)
                {
                    PopupItem objItem = (PopupItem)m_RegisteredPopups[m_RegisteredPopups.Count - 1];

                    Control ctrl = objItem.PopupControl as Control;
                    Control ctrl2 = FromChildHandle(hWnd);

                    if (ctrl2 != null)
                    {
                        while (ctrl2.Parent != null)
                            ctrl2 = ctrl2.Parent;
                    }

                    bool bIsOnHandle = false;
                    if (ctrl2 != null && objItem != null)
                        bIsOnHandle = objItem.IsAnyOnHandle(ctrl2.Handle.ToInt32());

                    bool bNoEat = ctrl != null && ctrl2 != null && ctrl.Handle == ctrl2.Handle || bIsOnHandle;

                    if (!bIsOnHandle)
                    {
                        Keys key = (Keys)NativeFunctions.MapVirtualKey((uint)wParam, 2);
                        if (key == Keys.None)
                            key = (Keys)wParam.ToInt32();
                        objItem.InternalKeyDown(new KeyEventArgs(key));
                    }

                    // Don't eat the message if the pop-up window has focus
                    if (bNoEat)
                        return false;
                    return true;
                }
            }

            if (this.MenuFocus)
            {
                bool bPassToMenu = true;
                Control ctrl2 = Control.FromChildHandle(hWnd);
                if (ctrl2 != null)
                {
                    while (ctrl2.Parent != null)
                        ctrl2 = ctrl2.Parent;
                    if ((ctrl2 is MenuPanel || ctrl2 is ItemControl || ctrl2 is PopupContainer || ctrl2 is PopupContainerControl) && ctrl2.Handle != hWnd)
                        bPassToMenu = false;
                }

                if (bPassToMenu)
                {
                    Keys key = (Keys)NativeFunctions.MapVirtualKey((uint)wParam, 2);
                    if (key == Keys.None)
                        key = (Keys)wParam.ToInt32();
                    m_PopupItem.InternalKeyDown(new KeyEventArgs(key));
                    return true;
                }
            }

            if (!this.IsParentFormActive)
                return false;

            if (wParam.ToInt32() >= 0x70 || ModifierKeys != Keys.None || (lParam.ToInt32() & 0x1000000000) != 0 || wParam.ToInt32() == 0x2E || wParam.ToInt32() == 0x2D) // 2E=VK_DELETE, 2D=VK_INSERT
            {
                int i = (int)ModifierKeys | wParam.ToInt32();
                return ProcessShortcut((eShortcut)i);
            }
            return false;
        }

        private bool ProcessShortcut(eShortcut key)
        {
            foreach (eShortcut k in m_PopupItem.Shortcuts)
            {
                if (k == key)
                {
                    PerformClick();
                    return true;
                }
            }
            return BarFunctions.ProcessItemsShortcuts(key, m_ShortcutTable);
        }

        /// <summary>
        /// Generates a Click event for the control.
        /// </summary>
        public abstract void PerformClick();

        protected bool IsParentFormActive
        {
            get
            {
                // Process only if parent form is active
                Form form = this.FindForm();
                if (form == null)
                    return false;
                if (form.IsMdiChild)
                {
                    if (form.MdiParent == null)
                        return false;
                    if (form.MdiParent.ActiveMdiChild != form)
                        return false;
                }
                else if (form != Form.ActiveForm)
                    return false;
                return true;
            }
        }

        private PopupDelayedClose m_DelayClose = null;
        private PopupDelayedClose GetDelayClose()
        {
            if (m_DelayClose == null)
                m_DelayClose = new PopupDelayedClose();
            return m_DelayClose;
        }

        internal void DesignerNewItemAdded()
        {
            this.GetDelayClose().EraseDelayClose();
        }

        bool IMessageHandlerClient.OnMouseDown(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
        {
            if (m_RegisteredPopups.Count == 0)
                return false;

            BaseItem[] popups = new BaseItem[m_RegisteredPopups.Count];
            m_RegisteredPopups.CopyTo(popups);
            for (int i = popups.Length - 1; i >= 0; i--)
            {
                PopupItem objPopup = popups[i] as PopupItem;
                bool bChildHandle = objPopup.IsAnyOnHandle(hWnd.ToInt32());

                if (!bChildHandle)
                {
                    System.Windows.Forms.Control cTmp = System.Windows.Forms.Control.FromChildHandle(hWnd);
                    if (cTmp != null)
                    {
                        if (cTmp is MenuPanel)
                        {
                            bChildHandle = true;
                        }
                        else
                        {
                            while (cTmp.Parent != null)
                            {
                                cTmp = cTmp.Parent;
                                if (cTmp.GetType().FullName.IndexOf("DropDownHolder") >= 0 || cTmp is MenuPanel || cTmp is PopupContainerControl)
                                {
                                    bChildHandle = true;
                                    break;
                                }
                            }
                        }
                        if (!bChildHandle)
                            bChildHandle = objPopup.IsAnyOnHandle(cTmp.Handle.ToInt32());
                    }
                    else
                    {
                        string s = NativeFunctions.GetClassName(hWnd);
                        s = s.ToLower();
                        if (s.IndexOf("combolbox") >= 0)
                            bChildHandle = true;
                    }
                }

                if (!bChildHandle)
                {
                    Control popupContainer = objPopup.PopupControl;
                    if (popupContainer != null)
                        while (popupContainer.Parent != null) popupContainer = popupContainer.Parent;
                    if (popupContainer != null && popupContainer.Bounds.Contains(Control.MousePosition))
                        bChildHandle = true;
                }

                if (bChildHandle)
                    break;

                if (objPopup.Displayed)
                {
                    // Do not close if mouse is inside the popup parent button
                    Point p = this.PointToClient(MousePosition);
                    if (objPopup.DisplayRectangle.Contains(p))
                        break;
                }

                if (this.GetDesignMode())
                {
                    this.GetDelayClose().DelayClose(objPopup);
                }
                else
                    objPopup.ClosePopup();

                if (m_RegisteredPopups.Count == 0)
                    break;
            }
            if (m_RegisteredPopups.Count == 0)
                this.MenuFocus = false;
            return false;
        }
        bool IMessageHandlerClient.OnMouseMove(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
        {
            if (m_RegisteredPopups.Count > 0)
            {
                foreach (BaseItem item in m_RegisteredPopups)
                {
                    if (item.Parent == null)
                    {
                        Control ctrl = ((PopupItem)item).PopupControl;
                        if (ctrl != null && ctrl.Handle != hWnd && !item.IsAnyOnHandle(hWnd.ToInt32()) && !(ctrl.Parent != null && ctrl.Parent.Handle != hWnd))
                            return true;
                    }
                }
            }
            return false;
        }
        bool IMessageHandlerClient.OnSysKeyDown(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
        {
            return OnSysKeyDown(hWnd, wParam, lParam);
        }

        protected virtual bool OnSysKeyDown(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
        {
            if (!this.GetDesignMode())
            {
                // Check Shortcuts
                if (ModifierKeys != Keys.None || wParam.ToInt32() >= (int)eShortcut.F1 && wParam.ToInt32() <= (int)eShortcut.F12)
                {
                    int i = (int)ModifierKeys | wParam.ToInt32();
                    if (ProcessShortcut((eShortcut)i))
                        return true;
                }
            }
            return false;
        }

        bool IMessageHandlerClient.OnSysKeyUp(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
        {
            return OnSysKeyUp(hWnd, wParam, lParam);
        }

        protected virtual bool OnSysKeyUp(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
        {
            return false;
        }

        private void MenuEventSupportHook()
        {
            if (m_MenuEventSupport)
                return;
            m_MenuEventSupport = true;

            Form parentForm = this.FindForm();
            if (parentForm == null)
            {
                m_MenuEventSupport = false;
                return;
            }

            parentForm.Resize += new EventHandler(this.ParentResize);
            parentForm.Deactivate += new EventHandler(this.ParentDeactivate);

            DotNetBarManager.RegisterParentMsgHandler(this, parentForm);
        }

        private void MenuEventSupportUnhook()
        {
            if (!m_MenuEventSupport)
                return;
            m_MenuEventSupport = false;

            Form parentForm = this.FindForm();
            if (parentForm == null)
                return;
            DotNetBarManager.UnRegisterParentMsgHandler(this, parentForm);
            parentForm.Resize -= new EventHandler(this.ParentResize);
            parentForm.Deactivate -= new EventHandler(this.ParentDeactivate);
        }
        private void ParentResize(object sender, EventArgs e)
        {
            Form parentForm = this.FindForm();
            if (parentForm != null && parentForm.WindowState == FormWindowState.Minimized)
                ((IOwner)this).OnApplicationDeactivate();
        }
        private void ParentDeactivate(object sender, EventArgs e)
        {
            Form parentForm = this.FindForm();
            if (parentForm != null && parentForm.WindowState == FormWindowState.Minimized)
                ((IOwner)this).OnApplicationDeactivate();
        }
        #endregion

        #region IThemeCache Implementation
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == NativeFunctions.WM_THEMECHANGED)
            {
                this.RefreshThemes();
            }
            else if (m.Msg == NativeFunctions.WM_USER + 107)
            {
                if (m_DoDefaultActionItem != null)
                {
                    if (!m_DoDefaultActionItem._AccessibleExpandAction)
                        this.PerformClick();
                    m_DoDefaultActionItem.DoAccesibleDefaultAction();
                    m_DoDefaultActionItem = null;
                }
            }
            base.WndProc(ref m);
        }
        protected void RefreshThemes()
        {
            if (m_ThemeWindow != null)
            {
                m_ThemeWindow.Dispose();
                m_ThemeWindow = new ThemeWindow(this);
            }
            if (m_ThemeRebar != null)
            {
                m_ThemeRebar.Dispose();
                m_ThemeRebar = new ThemeRebar(this);
            }
            if (m_ThemeToolbar != null)
            {
                m_ThemeToolbar.Dispose();
                m_ThemeToolbar = new ThemeToolbar(this);
            }
            if (m_ThemeHeader != null)
            {
                m_ThemeHeader.Dispose();
                m_ThemeHeader = new ThemeHeader(this);
            }
            if (m_ThemeScrollBar != null)
            {
                m_ThemeScrollBar.Dispose();
                m_ThemeScrollBar = new ThemeScrollBar(this);
            }
            if (m_ThemeProgress != null)
            {
                m_ThemeProgress.Dispose();
                m_ThemeProgress = new ThemeProgress(this);
            }
            if (m_ThemeExplorerBar != null)
            {
                m_ThemeExplorerBar.Dispose();
                m_ThemeExplorerBar = new ThemeExplorerBar(this);
            }
            if (m_ThemeButton != null)
            {
                m_ThemeButton.Dispose();
                m_ThemeButton = new ThemeButton(this);
            }
        }
        private void DisposeThemes()
        {
            if (m_ThemeWindow != null)
            {
                m_ThemeWindow.Dispose();
                m_ThemeWindow = null;
            }
            if (m_ThemeRebar != null)
            {
                m_ThemeRebar.Dispose();
                m_ThemeRebar = null;
            }
            if (m_ThemeToolbar != null)
            {
                m_ThemeToolbar.Dispose();
                m_ThemeToolbar = null;
            }
            if (m_ThemeHeader != null)
            {
                m_ThemeHeader.Dispose();
                m_ThemeHeader = null;
            }
            if (m_ThemeScrollBar != null)
            {
                m_ThemeScrollBar.Dispose();
                m_ThemeScrollBar = null;
            }
            if (m_ThemeProgress != null)
            {
                m_ThemeProgress.Dispose();
                m_ThemeProgress = null;
            }
            if (m_ThemeExplorerBar != null)
            {
                m_ThemeExplorerBar.Dispose();
                m_ThemeExplorerBar = null;
            }
            if (m_ThemeButton != null)
            {
                m_ThemeButton.Dispose();
                m_ThemeButton = null;
            }
        }
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            if (!m_FilterInstalled && !this.DesignMode)
            {
                MessageHandler.RegisterMessageClient(this);
                m_FilterInstalled = true;
            }

#if FRAMEWORK20
            if (this.AutoSize)
                this.AdjustSize();
#endif
            this.RecalcLayout();
        }
#if FRAMEWORK20
        protected virtual void AdjustSize() { }
#endif
        protected override void OnHandleDestroyed(EventArgs e)
        {
            DisposeThemes();
            MenuEventSupportUnhook();
            base.OnHandleDestroyed(e);

            if (m_FilterInstalled)
            {
                MessageHandler.UnregisterMessageClient(this);
                m_FilterInstalled = false;
            }
        }
        ThemeWindow IThemeCache.ThemeWindow
        {
            get
            {
                if (m_ThemeWindow == null)
                    m_ThemeWindow = new ThemeWindow(this);
                return m_ThemeWindow;
            }
        }
        ThemeRebar IThemeCache.ThemeRebar
        {
            get
            {
                if (m_ThemeRebar == null)
                    m_ThemeRebar = new ThemeRebar(this);
                return m_ThemeRebar;
            }
        }
        ThemeToolbar IThemeCache.ThemeToolbar
        {
            get
            {
                if (m_ThemeToolbar == null)
                    m_ThemeToolbar = new ThemeToolbar(this);
                return m_ThemeToolbar;
            }
        }
        ThemeHeader IThemeCache.ThemeHeader
        {
            get
            {
                if (m_ThemeHeader == null)
                    m_ThemeHeader = new ThemeHeader(this);
                return m_ThemeHeader;
            }
        }
        ThemeScrollBar IThemeCache.ThemeScrollBar
        {
            get
            {
                if (m_ThemeScrollBar == null)
                    m_ThemeScrollBar = new ThemeScrollBar(this);
                return m_ThemeScrollBar;
            }
        }
        ThemeExplorerBar IThemeCache.ThemeExplorerBar
        {
            get
            {
                if (m_ThemeExplorerBar == null)
                    m_ThemeExplorerBar = new ThemeExplorerBar(this);
                return m_ThemeExplorerBar;
            }
        }
        ThemeProgress IThemeCache.ThemeProgress
        {
            get
            {
                if (m_ThemeProgress == null)
                    m_ThemeProgress = new ThemeProgress(this);
                return m_ThemeProgress;
            }
        }
        ThemeButton IThemeCache.ThemeButton
        {
            get
            {
                if (m_ThemeButton == null)
                    m_ThemeButton = new ThemeButton(this);
                return m_ThemeButton;
            }
        }

        #endregion

        #region IOwner Implementation
        /// <summary>
        /// Gets or sets the form button is attached to.
        /// </summary>
        Form IOwner.ParentForm
        {
            get
            {
                return base.FindForm();
            }
            set { }
        }

        /// <summary>
        /// Returns the collection of items with the specified name. This member is not implemented and should not be used.
        /// </summary>
        /// <param name="ItemName">Item name to look for.</param>
        /// <returns></returns>
        ArrayList IOwner.GetItems(string ItemName)
        {
            ArrayList list = new ArrayList(15);
            BarFunctions.GetSubItemsByName(m_PopupItem, ItemName, list);
            return list;
        }

        /// <summary>
        /// Returns the collection of items with the specified name and type. This member is not implemented and should not be used.
        /// </summary>
        /// <param name="ItemName">Item name to look for.</param>
        /// <param name="itemType">Item type to look for.</param>
        /// <returns></returns>
        ArrayList IOwner.GetItems(string ItemName, Type itemType)
        {
            ArrayList list = new ArrayList(15);
            BarFunctions.GetSubItemsByNameAndType(m_PopupItem, ItemName, list, itemType);
            return list;
        }

        /// <summary>
        /// Returns the collection of items with the specified name and type. This member is not implemented and should not be used.
        /// </summary>
        /// <param name="ItemName">Item name to look for.</param>
        /// <param name="itemType">Item type to look for.</param>
        /// <param name="useGlobalName">Indicates whether GlobalName property is used for searching.</param>
        /// <returns></returns>
        ArrayList IOwner.GetItems(string ItemName, Type itemType, bool useGlobalName)
        {
            ArrayList list = new ArrayList(15);
            BarFunctions.GetSubItemsByNameAndType(m_PopupItem, ItemName, list, itemType, useGlobalName);
            return list;
        }

        /// <summary>
        /// Returns the first item that matches specified name.  This member is not implemented and should not be used.
        /// </summary>
        /// <param name="ItemName">Item name to look for.</param>
        /// <returns></returns>
        BaseItem IOwner.GetItem(string ItemName)
        {
            BaseItem item = BarFunctions.GetSubItemByName(m_PopupItem, ItemName);
            if (item != null)
                return item;
            return null;
        }

        // Only one Popup Item can be expanded at a time. This is used
        // to track the currently expanded popup item and to close the popup item
        // if another item is expanding.
        void IOwner.SetExpandedItem(BaseItem objItem)
        {
            if (objItem != null && objItem.Parent is PopupItem)
                return;
            if (m_ExpandedItem != null)
            {
                if (m_ExpandedItem.Expanded)
                    m_ExpandedItem.Expanded = false;
                m_ExpandedItem = null;
            }
            m_ExpandedItem = objItem;
        }

        BaseItem IOwner.GetExpandedItem()
        {
            return m_ExpandedItem;
        }

        // Currently we are using this to communicate "focus" when control is in
        // design mode. This can be used later if we decide to add focus
        // handling to our BaseItem class.
        void IOwner.SetFocusItem(BaseItem objFocusItem)
        {
            if (m_FocusItem != null && m_FocusItem != objFocusItem)
            {
                m_FocusItem.OnLostFocus();
            }
            m_FocusItem = objFocusItem;
            if (m_FocusItem != null)
                m_FocusItem.OnGotFocus();
        }

        BaseItem IOwner.GetFocusItem()
        {
            return m_FocusItem;
        }

        void IOwner.DesignTimeContextMenu(BaseItem objItem)
        {
        }

        bool IOwner.DesignMode
        {
            get { return this.GetDesignMode(); }
        }

        void IOwner.RemoveShortcutsFromItem(BaseItem objItem)
        {
            ShortcutTableEntry objEntry = null;
            if (objItem.ShortcutString != "")
            {
                foreach (eShortcut key in objItem.Shortcuts)
                {
                    if (m_ShortcutTable.ContainsKey(key))
                    {
                        objEntry = (ShortcutTableEntry)m_ShortcutTable[key];
                        try
                        {
                            objEntry.Items.Remove(objItem.Id);
                            if (objEntry.Items.Count == 0)
                                m_ShortcutTable.Remove(objEntry.Shortcut);
                        }
                        catch (ArgumentException) { }
                    }
                }
            }
            IOwner owner = this as IOwner;
            foreach (BaseItem objTmp in objItem.SubItems)
                owner.RemoveShortcutsFromItem(objTmp);
        }

        void IOwner.AddShortcutsFromItem(BaseItem objItem)
        {
            ShortcutTableEntry objEntry = null;
            if (objItem.ShortcutString != "")
            {
                foreach (eShortcut key in objItem.Shortcuts)
                {
                    if (m_ShortcutTable.ContainsKey(key))
                        objEntry = (ShortcutTableEntry)m_ShortcutTable[objItem.Shortcuts[0]];
                    else
                    {
                        objEntry = new ShortcutTableEntry(key);
                        m_ShortcutTable.Add(objEntry.Shortcut, objEntry);
                    }
                    try
                    {
                        objEntry.Items.Add(objItem.Id, objItem);
                    }
                    catch (ArgumentException) { }
                }
            }
            IOwner owner = this as IOwner;
            foreach (BaseItem objTmp in objItem.SubItems)
                owner.AddShortcutsFromItem(objTmp);
        }

        Form IOwner.ActiveMdiChild
        {
            get
            {
                Form form = base.FindForm();
                if (form == null)
                    return null;
                if (form.IsMdiContainer)
                {
                    return form.ActiveMdiChild;
                }
                return null;
            }
        }

        bool IOwner.AlwaysDisplayKeyAccelerators
        {
            get { return true; }
            set { }
        }

        /// <summary>
        /// Invokes the DotNetBar Customize dialog.
        /// </summary>
        void IOwner.Customize()
        {
        }

        void IOwner.InvokeResetDefinition(BaseItem item, EventArgs e)
        {
        }

        /// <summary>
        /// Indicates whether Reset buttons is shown that allows end-user to reset the toolbar state.
        /// </summary>
        bool IOwner.ShowResetButton
        {
            get { return false; }
            set { }
        }

        void IOwner.OnApplicationActivate() { }
        void IOwner.OnApplicationDeactivate()
        {
            ClosePopups();
        }
        void IOwner.OnParentPositionChanging() { }

        void IOwner.StartItemDrag(BaseItem item) { }

        bool IOwner.DragInProgress
        {
            get { return false; }
        }

        BaseItem IOwner.DragItem
        {
            get { return null; }
        }

        void IOwner.InvokeUserCustomize(object sender, EventArgs e)  {}

        void IOwner.InvokeEndUserCustomize(object sender, EndUserCustomizeEventArgs e) { }

        MdiClient IOwner.GetMdiClient(Form MdiForm)
        {
            return BarFunctions.GetMdiClient(MdiForm);
        }

		/// <summary>
		/// ImageList for images used on Items. Images specified here will always be used on menu-items and are by default used on all Bars.
		/// </summary>
		[System.ComponentModel.Browsable(false),System.ComponentModel.Category("Data"),DefaultValue(null),System.ComponentModel.Description("ImageList for images used on Items. Images specified here will always be used on menu-items and are by default used on all Bars.")]
		public System.Windows.Forms.ImageList Images
		{
			get
			{
				return m_ImageList;
			}
			set
			{
				if(m_ImageList!=null)
					m_ImageList.Disposed-=new EventHandler(this.ImageListDisposed);
				m_ImageList=value;
				if(m_ImageList!=null)
					m_ImageList.Disposed+=new EventHandler(this.ImageListDisposed);
			}
		}

		/// <summary>
		/// ImageList for medium-sized images used on Items.
		/// </summary>
		[System.ComponentModel.Browsable(false),System.ComponentModel.Category("Data"),DefaultValue(null),System.ComponentModel.Description("ImageList for medium-sized images used on Items.")]
		public System.Windows.Forms.ImageList ImagesMedium
		{
			get
			{
				return m_ImageListMedium;
			}
			set
			{
				if(m_ImageListMedium!=null)
					m_ImageListMedium.Disposed-=new EventHandler(this.ImageListDisposed);
				m_ImageListMedium=value;
				if(m_ImageListMedium!=null)
					m_ImageListMedium.Disposed+=new EventHandler(this.ImageListDisposed);
			}
		}

		/// <summary>
		/// ImageList for large-sized images used on Items.
		/// </summary>
		[System.ComponentModel.Browsable(false),System.ComponentModel.Category("Data"),DefaultValue(null),System.ComponentModel.Description("ImageList for large-sized images used on Items.")]
		public System.Windows.Forms.ImageList ImagesLarge
		{
			get
			{
				return m_ImageListLarge;
			}
			set
			{
				if(m_ImageListLarge!=null)
					m_ImageListLarge.Disposed-=new EventHandler(this.ImageListDisposed);
				m_ImageListLarge=value;
				if(m_ImageListLarge!=null)
					m_ImageListLarge.Disposed+=new EventHandler(this.ImageListDisposed);
			}
		}

		private void ImageListDisposed(object sender, EventArgs e)
		{
			if(sender==m_ImageList)
			{
				m_ImageList=null;
			}
			else if(sender==m_ImageListLarge)
			{
				m_ImageListLarge=null;
			}
			else if(sender==m_ImageListMedium)
			{
				m_ImageListMedium=null;
			}
		}


        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
            if (this.DesignMode)
                m_PopupItem.SetDesignMode(this.DesignMode);
        }

        void IOwner.InvokeDefinitionLoaded(object sender, EventArgs e) {}

        /// <summary>
        /// Indicates whether Tooltips are shown on Bars and menus.
        /// </summary>
        //[Browsable(false), DefaultValue(true), Category("Run-time Behavior"), Description("Indicates whether Tooltips are shown on Bars and menus.")]
        bool IOwner.ShowToolTips
        {
            get
            {
                return true;
            }
            set
            {
                //m_ShowToolTips = value;
            }
        }

        /// <summary>
        /// Indicates whether item shortcut is displayed in Tooltips.
        /// </summary>
        //[Browsable(false), DefaultValue(false), Category("Run-time Behavior"), Description("Indicates whether item shortcut is displayed in Tooltips.")]
        bool IOwner.ShowShortcutKeysInToolTips
        {
            get
            {
                return true;
            }
            set
            {
                //m_ShowShortcutKeysInToolTips = value;
            }
        }

        /// <summary>
        /// Gets or sets whether gray-scale algorithm is used to create automatic gray-scale images. Default is true.
        /// </summary>
        [Browsable(true), DefaultValue(true), Category("Appearance"), Description("Gets or sets whether gray-scale algorithm is used to create automatic gray-scale images.")]
        public virtual bool DisabledImagesGrayScale
        {
            get
            {
                return m_DisabledImagesGrayScale;
            }
            set
            {
                m_DisabledImagesGrayScale = value;
            }
        }
        #endregion

        #region IOwnerLocalize Members
        void IOwnerLocalize.InvokeLocalizeString(LocalizeEventArgs e)
        {
            if (LocalizeString != null)
                LocalizeString(this, e);
        }
        #endregion

        #region IAccessibilitySupport Members

        BaseItem IAccessibilitySupport.DoDefaultActionItem
        {
            get
            {
                return m_DoDefaultActionItem;
            }
            set
            {
                m_DoDefaultActionItem = value; ;
            }
        }
        #endregion
    }
}
