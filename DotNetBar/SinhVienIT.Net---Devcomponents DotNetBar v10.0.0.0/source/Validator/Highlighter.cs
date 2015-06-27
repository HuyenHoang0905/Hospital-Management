#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;

namespace DevComponents.DotNetBar.Validator
{
    [ToolboxBitmap(typeof(Highlighter), "Validator.Highlighter.ico"), ToolboxItem(true), ProvideProperty("HighlightColor", typeof(Control)),
    ProvideProperty("HighlightOnFocus", typeof(Control)),
   System.Runtime.InteropServices.ComVisible(false), Designer("DevComponents.DotNetBar.Design.HighlighterDesigner, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
    public class Highlighter : Component, IExtenderProvider, IErrorProvider
    {
        #region Private Variables
        private Dictionary<Control, eHighlightColor> _Highlights = new Dictionary<Control, eHighlightColor>();
        private Dictionary<Control, bool> _HighlightOnFocus = new Dictionary<Control, bool>();
        #endregion

        #region Implementation
        protected override void Dispose(bool disposing)
        {
            if (_HighlightPanel != null && _HighlightPanel.Parent == null && !_HighlightPanel.IsDisposed)
            {
                _HighlightPanel.Dispose();
                _HighlightPanel = null;
            }
            else
                _HighlightPanel = null;

            base.Dispose(disposing);
        }


        /// <summary>
        /// Retrieves whether control is highlighted when it receives input focus.
        /// </summary>
        [DefaultValue(false), Localizable(true), Description("Indicates whether control is highlighted when it receives input focus.")]
        public bool GetHighlightOnFocus(Control c)
        {
            if (_HighlightOnFocus.ContainsKey(c))
            {
                return _HighlightOnFocus[c];
            }
            return false;
        }
        /// <summary>
        /// Sets whether control is highlighted when it receives input focus.
        /// </summary>
        /// <param name="c">Reference to supported control.</param>
        /// <param name="highlight">Indicates whether to highlight control on focus.</param>
        public void SetHighlightOnFocus(Control c, bool highlight)
        {
            if (c == null) throw new NullReferenceException();

            if (_HighlightOnFocus.ContainsKey(c))
            {
                if (!highlight)
                {
                    RemoveHighlightOnFocus(_HighlightOnFocus, c);
                }
                return;
            }
            if(highlight)
                AddHighlightOnFocus(_HighlightOnFocus, c);
        }
        private void AddHighlightOnFocus(Dictionary<Control, bool> highlightOnFocus, Control c)
        {
            c.Enter += ControlHighlightEnter;
            c.Leave += ControlHighlightLeave;
            c.VisibleChanged += ControlHighlightVisibleChanged;
            highlightOnFocus.Add(c, true);
        }

        void ControlHighlightVisibleChanged(object sender, EventArgs e)
        {
            if(_HighlightPanel!=null && _HighlightPanel.FocusHighlightControl == sender)
                UpdateHighlighterRegion();
        }
        void ControlHighlightLeave(object sender, EventArgs e)
        {
            if (_HighlightPanel != null) _HighlightPanel.FocusHighlightControl = null;
            UpdateHighlighterRegion();
        }
        void ControlHighlightEnter(object sender, EventArgs e)
        {
            if (_HighlightPanel != null)
            {
                if (!_HighlightPanel.Visible) _HighlightPanel.Visible = true;
                _HighlightPanel.BringToFront();
                _HighlightPanel.FocusHighlightControl = (Control)sender;
            }
            UpdateHighlighterRegion();
        }
        private void RemoveHighlightOnFocus(Dictionary<Control, bool> highlightOnFocus, Control c)
        {
            c.Enter -= ControlHighlightEnter;
            c.Leave -= ControlHighlightLeave;
            c.VisibleChanged -= ControlHighlightVisibleChanged;
            highlightOnFocus.Remove(c);
        }

        /// <summary>
        /// Retrieves the highlight color that is applied to the control.
        /// </summary>
        [DefaultValue(eHighlightColor.None), Localizable(true), Description("Indicates the highlight color that is applied to the control.")]
        public eHighlightColor GetHighlightColor(Control c)
        {
            if (_Highlights.ContainsKey(c))
            {
                return _Highlights[c];
            }
            return eHighlightColor.None;
        }
        /// <summary>
        /// Sets the highlight color for the control.
        /// </summary>
        /// <param name="c">Reference to supported control.</param>
        /// <param name="highlightColor">Highlight color.</param>
        public void SetHighlightColor(Control c, eHighlightColor highlightColor)
        {
            if (_Highlights.ContainsKey(c))
            {
                if (highlightColor == eHighlightColor.None)
                {
                    RemoveHighlight(_Highlights, c);
                }
                else
                {
                    eHighlightColor color = _Highlights[c];
                    RemoveHighlight(_Highlights, c);
                    AddHighlight(_Highlights, c, highlightColor);
                }
            }
            else if (highlightColor != eHighlightColor.None)
            {
                AddHighlight(_Highlights, c, highlightColor);
            }
        }

        private Dictionary<TabControl, int> _TabControl1 = new Dictionary<TabControl, int>();
        private Dictionary<SuperTabControl, int> _SuperTabControl1 = new Dictionary<SuperTabControl, int>();
        private Dictionary<System.Windows.Forms.TabControl, int> _TabControl2 = new Dictionary<System.Windows.Forms.TabControl, int>();
        private Dictionary<Panel, int> _ParentPanel = new Dictionary<Panel, int>();

        private void AddHighlight(Dictionary<Control, eHighlightColor> highlights, Control c, eHighlightColor highlightColor)
        {
            highlights.Add(c, highlightColor);
            c.LocationChanged += new EventHandler(ControlLocationChanged);
            c.SizeChanged += new EventHandler(ControlSizeChanged);
            c.VisibleChanged += new EventHandler(ControlVisibleChanged);
            if (_HighlightPanel != null)
            {
                if (!_HighlightPanel.Visible) _HighlightPanel.Visible = true;
                _HighlightPanel.BringToFront();
            }

            if(c.Parent == null)
                c.ParentChanged += ControlParentChanged;
            else
                AddTabControlHandlers(c);

            UpdateHighlighterRegion();
        }

        private void ControlParentChanged(object sender, EventArgs e)
        {
            Control c = (Control)sender;
            c.ParentChanged -= ControlParentChanged;
            AddTabControlHandlers(c);
        }

        private void AddTabControlHandlers(Control c)
        {
            TabControl tab1 = GetParentControl(c, typeof(TabControl)) as TabControl;

            if (tab1 != null)
            {
                if (_TabControl1.ContainsKey(tab1))
                    _TabControl1[tab1] = _TabControl1[tab1] + 1;
                else
                {
                    _TabControl1.Add(tab1, 1);
                    tab1.SelectedTabChanged += TabControl1SelectedTabChanged;
                }
            }
            else
            {
                SuperTabControl tab = GetParentControl(c, typeof(SuperTabControl)) as SuperTabControl;

                if (tab != null)
                {
                    if (_SuperTabControl1.ContainsKey(tab))
                        _SuperTabControl1[tab] = _SuperTabControl1[tab] + 1;
                    else
                    {
                        _SuperTabControl1.Add(tab, 1);
                        tab.SelectedTabChanged += SuperTabControl1SelectedTabChanged;
                    }
                }
                else
                {
                    System.Windows.Forms.TabControl tab2 =
                        GetParentControl(c, typeof (System.Windows.Forms.TabControl)) as System.Windows.Forms.TabControl;

                    if (tab2 != null)
                    {
                        if (_TabControl2.ContainsKey(tab2))
                            _TabControl2[tab2] = _TabControl2[tab2] + 1;
                        else
                        {
                            _TabControl2.Add(tab2, 1);
                            tab2.SelectedIndexChanged += WinFormsTabSelectedIndexChanged;
                        }
                    }
                    else
                    {
                        Panel parentPanel = GetParentControl(c, typeof (Panel)) as Panel;

                        if (parentPanel != null)
                        {
                            if (_ParentPanel.ContainsKey(parentPanel))
                                _ParentPanel[parentPanel] = _ParentPanel[parentPanel] + 1;
                            else
                            {
                                _ParentPanel.Add(parentPanel, 1);
                                parentPanel.Resize += ParentPanelResized;
                                parentPanel.LocationChanged += ParentPanelLocationChanged;
                            }
                        }
                    }
                }
            }
        }

        private void ParentPanelLocationChanged(object sender, EventArgs e)
        {
            UpdateHighlights();
        }
        private void ParentPanelResized(object sender, EventArgs e)
        {
            UpdateHighlights();
        }
        private void WinFormsTabSelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateHighlighterRegion();
        }

        private void TabControl1SelectedTabChanged(object sender, TabStripTabChangedEventArgs e)
        {
            UpdateHighlighterRegion();
        }

        private void SuperTabControl1SelectedTabChanged(object sender, SuperTabStripSelectedTabChangedEventArgs e)
        {
            UpdateHighlighterRegion();
        }

        private Control GetParentControl(Control c, Type parentType)
        {
            Control parent = c.Parent;
            while (parent != null)
            {
                if (parentType.IsAssignableFrom(parent.GetType())) return parent;
                parent = parent.Parent;
            }
            return null;
        }

        void ControlVisibleChanged(object sender, EventArgs e)
        {
            UpdateHighlighterRegion();
        }

        private void ControlSizeChanged(object sender, EventArgs e)
        {
            UpdateHighlighterRegion();
        }
        private void ControlLocationChanged(object sender, EventArgs e)
        {
            UpdateHighlighterRegion();
        }

        private void UpdateHighlighterRegion()
        {
            if (_HighlightPanel != null) _HighlightPanel.UpdateRegion();
        }

        /// <summary>
        /// Updates the highlighted controls border. Usually call to this method is not needed but under
        /// certain scenarios where highlighter does not automatically detects the change in visibility of 
        /// the highlighted control call to this method is necessary.
        /// </summary>
        public void UpdateHighlights()
        {
            UpdateHighlighterRegion();
        }

        private void RemoveHighlight(Dictionary<Control, eHighlightColor> highlights, Control c)
        {
            highlights.Remove(c);
            c.LocationChanged -= new EventHandler(ControlLocationChanged);
            c.SizeChanged -= new EventHandler(ControlSizeChanged);
            c.VisibleChanged -= new EventHandler(ControlVisibleChanged);

            TabControl tab1 = GetParentControl(c, typeof(TabControl)) as TabControl;
            if (tab1 != null)
            {
                if (_TabControl1.ContainsKey(tab1))
                {
                    if (_TabControl1[tab1] == 1)
                    {
                        _TabControl1.Remove(tab1);
                        tab1.SelectedTabChanged -= TabControl1SelectedTabChanged;
                    }
                    else
                        _TabControl1[tab1] = _TabControl1[tab1] - 1;
                }
            }
            else
            {
                SuperTabControl tab = GetParentControl(c, typeof(SuperTabControl)) as SuperTabControl;

                if (tab != null)
                {
                    if (_SuperTabControl1.ContainsKey(tab))
                    {
                        if (_SuperTabControl1[tab] == 1)
                        {
                            _SuperTabControl1.Remove(tab);
                            tab.SelectedTabChanged -= SuperTabControl1SelectedTabChanged;
                        }
                        else
                            _SuperTabControl1[tab] = _SuperTabControl1[tab] - 1;
                    }
                }
                else
                {
                    System.Windows.Forms.TabControl tab2 =
                        GetParentControl(c, typeof (System.Windows.Forms.TabControl)) as System.Windows.Forms.TabControl;

                    if (tab2 != null)
                    {
                        if (_TabControl2.ContainsKey(tab2))
                        {
                            if (_TabControl2[tab2] == 1)
                            {
                                _TabControl2.Remove(tab2);
                                tab2.SelectedIndexChanged -= WinFormsTabSelectedIndexChanged;
                            }
                            else
                                _TabControl2[tab2] = _TabControl2[tab2] - 1;
                        }
                    }
                    else
                    {
                        Panel parentPanel = GetParentControl(c, typeof (Panel)) as Panel;

                        if (parentPanel != null)
                        {
                            if (_ParentPanel.ContainsKey(parentPanel))
                            {
                                if (_ParentPanel[parentPanel] == 1)
                                {
                                    _ParentPanel.Remove(parentPanel);
                                    parentPanel.LocationChanged -= ParentPanelLocationChanged;
                                    parentPanel.SizeChanged -= ParentPanelResized;
                                }
                                else
                                    _ParentPanel[parentPanel] = _ParentPanel[parentPanel] - 1;
                            }
                        }
                    }
                }
            }

            UpdateHighlighterRegion();
        }

        internal Dictionary<Control, eHighlightColor> Highlights
        {
            get
            {
                return _Highlights;
            }
        }

        private eHighlightColor _FocusHighlightColor = eHighlightColor.Blue;
        /// <summary>
        /// Indicates the highlight focus color.
        /// </summary>
        [DefaultValue(eHighlightColor.Blue), Category("Appearance"), Description("Indicates the highlight focus color."), Localizable(true)]
        public eHighlightColor FocusHighlightColor
        {
            get { return _FocusHighlightColor; }
            set
            {
                _FocusHighlightColor = value;
                if (_HighlightPanel != null)
                {
                    _HighlightPanel.FocusHighlightColor = value;
                    UpdateHighlighterRegion();
                }
            }
        }

        private HighlightPanel _HighlightPanel = null;
        private Control _ContainerControl = null;
        /// <summary>
        /// Gets or sets the container control highlighter is bound to. The container control must be set in order for highlighter to work.
        /// Container control should always be a form.
        /// </summary>
        [DefaultValue(null), Description("Indicates container control highlighter is bound to. Should be set to parent form."), Category("Behavior")]
        public Control ContainerControl
        {
            get
            {
                return _ContainerControl;
            }
            set
            {
                if (this.DesignMode)
                {
                    _ContainerControl = value;
                    return;
                }

                if (_ContainerControl != value)
                {
                    if (_ContainerControl != null)
                    {
                        _ContainerControl.SizeChanged -= ContainerControlSizeChanged;
                        if (_HighlightPanel != null && _HighlightPanel.Parent == _ContainerControl)
                            _ContainerControl.Controls.Remove(_HighlightPanel);
                    }

                    _ContainerControl = value;

                    if (_ContainerControl != null)
                    {
                        if (_HighlightPanel == null)
                        {
                            _HighlightPanel = new HighlightPanel(_Highlights);
                            _HighlightPanel.FocusHighlightColor = _FocusHighlightColor;
                            _HighlightPanel.Margin = new System.Windows.Forms.Padding(0);
                            _HighlightPanel.Padding = new System.Windows.Forms.Padding(0);
                            _HighlightPanel.CustomHighlightColors = _CustomHighlightColors;
                            _HighlightPanel.Visible = false;
                        }
                        _ContainerControl.SizeChanged += ContainerControlSizeChanged;
                        _ContainerControl.Controls.Add(_HighlightPanel);
                        UpdateHighlightPanelBounds();
                    }

                }
            }
        }

        private void UpdateHighlightPanelBounds()
        {
            Rectangle bounds = new Rectangle(0, 0, _ContainerControl.ClientRectangle.Width, _ContainerControl.ClientRectangle.Height);
            if (_HighlightPanel.Parent is Form)
            {
                Form form = _HighlightPanel.Parent as Form;
                if (form.AutoSize)
                {
                    bounds.X += form.Padding.Left;
                    bounds.Y += form.Padding.Top;
                    bounds.Width -= form.Padding.Horizontal;
                    bounds.Height -= form.Padding.Vertical;
                }
            }
            if(_HighlightPanel.Bounds.Equals(bounds))
                _HighlightPanel.UpdateRegion();
            else
                _HighlightPanel.Bounds = bounds;
            //_HighlightPanel.UpdateRegion();
            _HighlightPanel.BringToFront();
        }

        private Timer _DelayTimer = null;
        private void ContainerControlSizeChanged(object sender, EventArgs e)
        {
            if (!BarFunctions.IsVista)
            {
                Form form = sender as Form;
                if (form != null)
                {
                    if (_DelayTimer == null)
                    {
                        _DelayTimer = new Timer();
                        _DelayTimer.Interval = 100;
                        _DelayTimer.Tick += new EventHandler(DelayTimerTick);
                        _DelayTimer.Start();
                    }
                    return;
                }
            }
            UpdateHighlightPanelBounds();
        }

        void DelayTimerTick(object sender, EventArgs e)
        {
            Timer timer = _DelayTimer;
            _DelayTimer = null;
            timer.Tick -= new EventHandler(DelayTimerTick);
            timer.Stop();
            timer.Dispose();
            UpdateHighlightPanelBounds();
        }

        private Color[] _CustomHighlightColors = null;
        /// <summary>
        /// Gets or sets the array of colors used to render custom highlight color. Control expects 3 colors in array to be specified which define the highlight border.
        /// </summary>
        [DefaultValue(null), Category("Appearance"), Description("Array of colors used to render custom highlight color. Control expects 3 colors in array to be specified which define the highlight border.")]
        public Color[] CustomHighlightColors
        {
            get { return _CustomHighlightColors; }
            set 
            {
                _CustomHighlightColors = value;
                if (_HighlightPanel != null)
                {
                    _HighlightPanel.CustomHighlightColors = _CustomHighlightColors;
                    _HighlightPanel.Invalidate();
                }
            }
        }
        #endregion

        #region IExtenderProvider Members

        public bool CanExtend(object extendee)
        {
            return (extendee is Control);
        }

        #endregion

        #region Licensing
#if !TRIAL
        private string _LicenseKey = "";
        [Browsable(false), DefaultValue("")]
        public string LicenseKey
        {
            get { return _LicenseKey; }
            set
            {
                if (NativeFunctions.ValidateLicenseKey(value))
                    return;
                _LicenseKey = (!NativeFunctions.CheckLicenseKey(value) ? "9dsjkhds7" : value);
            }
        }
#endif
        #endregion

        #region IErrorProvider Members

        void IErrorProvider.SetError(Control control, string value)
        {
            this.SetHighlightColor(control, eHighlightColor.Red);
        }

        void IErrorProvider.ClearError(Control control)
        {
            this.SetHighlightColor(control, eHighlightColor.None);
        }

        #endregion
    }
    /// <summary>
    /// Defines highlight colors provided by Highlighter control.
    /// </summary>
    public enum eHighlightColor
    {
        None,
        Red,
        Blue,
        Green,
        Orange,
        Custom
    }
}
#endif