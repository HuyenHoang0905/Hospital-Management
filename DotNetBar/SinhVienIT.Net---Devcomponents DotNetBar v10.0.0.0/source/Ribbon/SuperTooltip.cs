using System;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;
using System.Collections;
using System.Reflection;
using DevComponents.DotNetBar.Controls;

namespace DevComponents.DotNetBar
{
    [ToolboxBitmap(typeof(SuperTooltip),"Ribbon.SuperTooltip.ico"),ToolboxItem(true), ProvideProperty("SuperTooltip",typeof(IComponent)),
   System.Runtime.InteropServices.ComVisible(false), Designer("DevComponents.DotNetBar.Design.SuperTooltipDesigner, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
    public class SuperTooltip : Component , IExtenderProvider
    {
        #region Private Variables & Constructor
        /// <summary>
        /// Occurs just before tooltip is displayed and gives you opportunity to cancel showing of tooltip.
        /// </summary>
        public event SuperTooltipEventHandler BeforeTooltipDisplay;

        /// <summary>
        /// Occurs after tooltip has been closed.
        /// </summary>
        public event EventHandler TooltipClosed;

        /// <summary>
        /// Occurs when text markup link is clicked. Markup links can be created using "a" tag, for example:
        /// <a name="MyLink">Markup link</a>
        /// </summary>
        public event MarkupLinkClickEventHandler MarkupLinkClick;

        private bool m_PositionBelowControl = true;
        private Hashtable m_SuperTooltipInfo = new Hashtable();
        private SuperTooltipControl m_Tooltip = null;
        private Timer m_ActiveWindowTimer = null;
        private Timer m_HideDelayedTimer = null;
        private bool m_CheckOnScreenPosition = true;
        private IntPtr m_ActiveWindowPtr = IntPtr.Zero;
        private int m_TooltipDuration = 20;
        private long m_DurationDisplayed = 0;
        private SuperTooltipInfo m_DefaultTooltipSettings = new SuperTooltipInfo();
        private static SuperTooltipInfo s_DefaultSuperTooltipInfo = null;
		private Font m_DefaultFont=null;
		private int m_HoverCount=0;
		private int m_HoverTrigger=2;
        private Size m_MinimumTooltipSize = new Size(150, 24);
        private bool m_Enabled = true;
		private bool m_AntiAlias = true;
        private int m_DelayTooltipHideDuration = 0;
        private bool m_ShowTooltipImmediately = false;
        private bool m_IgnoreFormActiveState = false;
        private int m_MaximumWidth = 0;
        private bool m_CheckTooltipPosition = true;
        private bool m_ShowTooltipDescription = true;
        private bool m_ShowTooltipForFocusedControl = true;
        private const string TREEGX_NODE_CLASS_NAME = "DevComponents.Tree.Node";
        #endregion

        #region Internal Implementation
        /// <summary>
        /// Gets or sets the maximum width of the super tooltip. Default value is 0 which indicates that maximum width is not used. The maximum width property
        /// will not be used if custom size is specified.
        /// </summary>
        [Browsable(true), DefaultValue(0), Category("Appearance"), Description("Indicates maximum width of the super tooltip.")]
        public int MaximumWidth
        {
            get { return m_MaximumWidth; }
            set { m_MaximumWidth = value; }
        }

        /// <summary>
        /// Gets or sets whether form active state is ignored when control is deciding whether to show tooltip. By default this property is set to false
        /// which indicates that tooltip will be shown only if parent form is active. When set to true the form active state is ignored when
        /// deciding whether to show tooltip.
        /// </summary>
        [Browsable(true), DefaultValue(false), Category("Behavior"), Description("Indicates whether form active state is ignored when control is deciding whether to show tooltip.")]
        public bool IgnoreFormActiveState
        {
            get { return m_IgnoreFormActiveState; }
            set { m_IgnoreFormActiveState = value; }
        }
		/// <summary>
		/// Gets or sets whether anti-alias smoothing is used while painting. Default value is true.
		/// </summary>
		[DefaultValue(true),Browsable(true),Category("Appearance"),Description("Gets or sets whether anti-aliasing is used while painting.")]
		public bool AntiAlias
		{
			get {return m_AntiAlias;}
			set
			{
				m_AntiAlias=value;
			}
		}

        protected override void Dispose(bool disposing)
        {
            HideTooltip();
            DestroyActiveWindowTimer();
            DisposeHideDelayedTimer();
            base.Dispose(disposing);
        }

        /// <summary>
        /// Gets or sets whether SuperTooltip will be shown for the controls assigned to it. Default value is true. You can set
        /// this property to false to disable SuperTooltip for all controls assigned to it.
        /// </summary>
        [Browsable(true), DefaultValue(true), Category("Behavior"), Description("Indicates whether SuperTooltip will be shown for the controls assigned to it.")]
        public bool Enabled
        {
            get { return m_Enabled; }
            set { m_Enabled = value; }
        }

        /// <summary>
        /// Gets or sets whether tooltip is shown immediately after the mouse enters the control. The default value is false which indicates
        /// that tooltip is shown after system hover timeout has expired which provides slight delay before tooltip is shown.
        /// </summary>
        [Browsable(true), DefaultValue(false), Category("Behavior"), Description("Indicates whether tooltip is shown immediately after the mouse enters the control.")]
        public bool ShowTooltipImmediately
        {
            get { return m_ShowTooltipImmediately; }
            set { m_ShowTooltipImmediately = value; }
        }

        /// <summary>
        /// Gets or sets the minimum tooltip size. Default value is 150 width and 24 height.
        /// </summary>
        [Browsable(true), Category("Appearance"), Description("Indicates minimum tooltip size.")]
        public Size MinimumTooltipSize
        {
            get { return m_MinimumTooltipSize; }
            set { m_MinimumTooltipSize = value; }
        }

        /// <summary>
        /// Gets whether MinimumTooltipSize property should be serialized.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeMinimumTooltipSize()
        {
            return (m_MinimumTooltipSize.Width != 150 || m_MinimumTooltipSize.Height != 24);
        }

        /// <summary>
        /// Gets or sets the duration in seconds that tooltip is kept on screen after it is displayed. Default value is 20 seconds.
        /// You can set this value to 0 to keep tooltip displayed until user moves mouse out of control or application loses focus.
        /// </summary>
        [Browsable(true), DefaultValue(20), Category("Behavior"), Description("Indicates duration in seconds that tooltip is kept on screen after it is displayed.")]
        public int TooltipDuration
        {
            get { return m_TooltipDuration; }
            set
            {
                if (value < 0) value = 0;
                m_TooltipDuration = value;
            }
        }

        /// <summary>
        /// 	Gets or sets the delay time for hiding the tooltip in milliseconds after
        ///     mouse has left the control. Default value is 0 which means that tooltip will be
        ///     hidden as soon as mouse leaves the control tooltip was displayed for. You can use
        ///     this property to provide the user with enough time to move the mouse cursor to the
        ///     tooltip so user can act on the content of the tooltip, like hyper links.
        /// </summary>
        [Browsable(true), DefaultValue(0), Category("Behavior"), Description("Indicates delay time for hiding the tooltip in milliseconds after mouse has left the control.")]
        public int DelayTooltipHideDuration
        {
            get { return m_DelayTooltipHideDuration; }
            set
            {
                if (value < 0) value = 0;
                m_DelayTooltipHideDuration = value;
            }
        }

        /// <summary>
        /// Gets or sets whether tooltip position is checked before tooltip is displayed and adjusted to tooltip always
        /// falls into screen bounds. Default value is true.
        /// </summary>
        [Browsable(true), DefaultValue(true), Category("Behavior"), Description("Indicates whether tooltip position is checked before tooltip is displayed and adjusted to tooltip always falls into screen bounds.")]
        public bool CheckOnScreenPosition
        {
            get { return m_CheckOnScreenPosition; }
            set { m_CheckOnScreenPosition = value; }
        }

        /// <summary>
        /// Gets or sets whether tooltip position is checked before tooltip is displayed and adjusted so tooltip does not overlaps the
        /// control it is displayed for. Default value is true.
        /// </summary>
        [Browsable(true), DefaultValue(true), Category("Behavior"), Description("Indicates whether tooltip position is checked before tooltip is displayed and adjusted so tooltip does not overlap control its displayed for")]
        public bool CheckTooltipPosition
        {
            get { return m_CheckTooltipPosition; }
            set { m_CheckTooltipPosition = value; }
        }

        /// <summary>
        /// Gets or sets the tooltip position in relationship to the control tooltip is providing information for.
        /// Set this property to false if you want tooltip to be displayed below the mouse cursor. Default value is
        /// true which indicates that tooltip is displayed below mouse cursor but it is positioned below the control
        /// that it provides the information for so it is not covering its content.
        /// </summary>
        [Browsable(true),DefaultValue(true),Category("Behavior"),Description("")]
        public bool PositionBelowControl
        {
            get { return m_PositionBelowControl; }
            set { m_PositionBelowControl = value; }
        }

        /// <summary>
        /// Gets or sets default setting for new Tooltips you create in design time. If all your tooltips have common elements
        /// you can change this property to reflect these default setting before you start writing tooltips for all controls on the form.
        /// As you start creating new tooltips for controls on the form default values specified here will be used as starting values
        /// for new tooltip you are creating.
        /// </summary>
        [Localizable(true), Editor("DevComponents.DotNetBar.Design.SuperTooltipInfoEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(System.Drawing.Design.UITypeEditor)), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), DefaultValue(null), Description("Indicates default setting for new Tooltips you create in design time.")]
        public SuperTooltipInfo DefaultTooltipSettings
        {
            get { return m_DefaultTooltipSettings; }
            set
            {
                m_DefaultTooltipSettings = value;
                s_DefaultSuperTooltipInfo = value;
            }
        }

        /// <summary>
        /// Returns instance of default tooltip information used in design-time.
        /// </summary>
        [Browsable(false)]
        public static SuperTooltipInfo DefaultSuperTooltipInfo
        {
            get { return s_DefaultSuperTooltipInfo; }
        }

        /// <summary>
		/// Retrieves SuperTooltipInfo for given component or return null if component does not have tooltip associated with it.
		/// </summary>
        [Editor("DevComponents.DotNetBar.Design.SuperTooltipInfoEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(System.Drawing.Design.UITypeEditor)), DefaultValue(null), Localizable(true)]
        public SuperTooltipInfo GetSuperTooltip(IComponent c)
		{
            if (m_SuperTooltipInfo.Contains(c))
			{
                SuperTooltipInfo info = m_SuperTooltipInfo[c] as SuperTooltipInfo;
				if(info!=null)
					return info;
			}
			return null;
		}

		/// <summary>
		/// Associates SuperTooltipInfo with given component.
		/// </summary>
		/// <param name="c">Reference to supported component.</param>
		/// <param name="info">Instance of SuperTooltipInfo class. If null is passed the SuperTooltip is detached from the given component.</param>
        public void SetSuperTooltip(IComponent c, SuperTooltipInfo info)
        {
            if (info != null)
            {
                if (info.BodyText == null) info.BodyText = "";
                if (info.FooterText == null) info.FooterText = "";
            }

            if (m_SuperTooltipInfo.Contains(c))
            {
                if (info == null)
                    this.RemoveSuperTooltipInfo(c);
                else
                    m_SuperTooltipInfo[c]=info;
            }
            else if (info != null)
            {
                this.AddSuperTooltipInfo(c, info);
            }
#if (FRAMEWORK20)
            if (c is MaskedTextBoxAdv && !this.DesignMode)
                SetSuperTooltip(((MaskedTextBoxAdv)c).MaskedTextBox, info);
            else if (c is TextBoxDropDown)
                SetSuperTooltip(((TextBoxDropDown)c).TextBox, info);
#endif
        }

        /// <summary>
        /// Gets the reference to internal Hashtable that contains reference to all controls and assigned SuperTooltips. This
        /// collection must not be modified directly and it is automatically managed by the SuperTooltip component. You can use it
        /// for example to change the color for all SuperTooltips managed by the component but you should not add or remove items to it.
        /// Instead use SetSuperTooltip methods to add or remove the tooltip for a component.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public Hashtable SuperTooltipInfoTable
        {
            get { return m_SuperTooltipInfo; }
        }

        private void RemoveSuperTooltipInfo(IComponent c)
        {
            m_SuperTooltipInfo.Remove(c);
            if (this.DesignMode)
                return;
#if (FRAMEWORK20)
            if (c is MaskedTextBoxAdv)
                RemoveSuperTooltipInfo(((MaskedTextBoxAdv)c).MaskedTextBox);
            else if(c is TextBoxDropDown)
                RemoveSuperTooltipInfo(((TextBoxDropDown)c).TextBox);
#endif
            if (c is ComboBoxItem)
            {
                ComboBoxItem item = c as ComboBoxItem;
                item.MouseHover -= new EventHandler(this.ComponentMouseHover);
                item.MouseLeave -= new EventHandler(ComponentMouseLeave);
                item.MouseEnter -= new EventHandler(ComponentMouseEnter);
                item.MouseDown -= new MouseEventHandler(ComponentMouseDown);
                item.ComboBoxEx.MouseHover -= new EventHandler(this.ComponentMouseHover);
                item.ComboBoxEx.MouseLeave -= new EventHandler(ComponentMouseLeave);
                item.ComboBoxEx.MouseEnter -= new EventHandler(ComponentMouseEnter);
                item.ComboBoxEx.MouseDown -= new MouseEventHandler(ComponentMouseDown);
            }
            else if (c is TextBoxItem)
            {
                TextBoxItem item = c as TextBoxItem;
                item.MouseHover -= new EventHandler(this.ComponentMouseHover);
                item.MouseLeave -= new EventHandler(ComponentMouseLeave);
                item.MouseEnter -= new EventHandler(ComponentMouseEnter);
                item.MouseDown -= new MouseEventHandler(ComponentMouseDown);
                item.TextBox.MouseHover -= new EventHandler(this.ComponentMouseHover);
                item.TextBox.MouseLeave -= new EventHandler(ComponentMouseLeave);
                item.TextBox.MouseEnter -= new EventHandler(ComponentMouseEnter);
                item.TextBox.MouseDown -= new MouseEventHandler(ComponentMouseDown);
            }
            else if (c is BaseItem)
            {
                BaseItem item = c as BaseItem;
                item.MouseHover -= new EventHandler(this.ComponentMouseHover);
                item.MouseLeave -= new EventHandler(ComponentMouseLeave);
                item.MouseEnter -= new EventHandler(ComponentMouseEnter);
                item.MouseDown -= new MouseEventHandler(ComponentMouseDown);
                
            }
            else if (c is RibbonBar)
            {
                RibbonBar bar = c as RibbonBar;
                bar.DialogLauncherMouseHover -= new EventHandler(this.ComponentMouseHover);
                bar.DialogLauncherMouseLeave -= new EventHandler(ComponentMouseLeave);
                bar.DialogLauncherMouseDown -= new MouseEventHandler(ComponentMouseDown);
                bar.DialogLauncherMouseEnter -= new EventHandler(ComponentMouseEnter);
            }
            else if (c is Control)
            {
                Control ctrl = c as Control;
                ctrl.MouseHover -= new EventHandler(this.ComponentMouseHover);
                ctrl.MouseLeave -= new EventHandler(ComponentMouseLeave);
                ctrl.MouseDown -= new MouseEventHandler(ComponentMouseDown);
                ctrl.MouseEnter -= new EventHandler(ComponentMouseEnter);
            }
            else if (c is ISuperTooltipInfoProvider)
            {
                ISuperTooltipInfoProvider ip = c as ISuperTooltipInfoProvider;
                ip.DisplayTooltip -= new EventHandler(this.ComponentMouseHover);
                ip.HideTooltip -= new EventHandler(ComponentHideTooltip);
            }
#if (FRAMEWORK20)
            else if (c is ToolStripButton)
            {
                ToolStripButton item = c as ToolStripButton;
                item.MouseHover -= new EventHandler(this.ComponentMouseHover);
                item.MouseLeave -= new EventHandler(ComponentMouseLeave);
                item.MouseEnter -= new EventHandler(ComponentMouseEnter);
                item.MouseDown -= new MouseEventHandler(ComponentMouseDown);
            }
#endif
            else if (c is AdvTree.Node || IsTreeGXType(c.GetType()))
            {
                Type nodeType = c.GetType();
                EventInfo ei = nodeType.GetEvent("NodeMouseHover");
                if (ei != null)
                {
                    Delegate handler = Delegate.CreateDelegate(typeof(EventHandler),
                    this, "ComponentMouseHover");
                    ei.RemoveEventHandler(c, handler);
                }

                ei = nodeType.GetEvent("NodeMouseLeave");
                if (ei != null)
                {
                    Delegate handler = Delegate.CreateDelegate(typeof(EventHandler),
                    this, "ComponentMouseLeave");
                    ei.RemoveEventHandler(c, handler);
                }

                ei = nodeType.GetEvent("NodeMouseEnter");
                if (ei != null)
                {
                    Delegate handler = Delegate.CreateDelegate(typeof(EventHandler),
                    this, "ComponentMouseEnter");
                    ei.RemoveEventHandler(c, handler);
                }

                ei = nodeType.GetEvent("NodeMouseDown");
                if (ei != null)
                {
                    Delegate handler = Delegate.CreateDelegate(typeof(MouseEventHandler),
                    this, "ComponentMouseDown");
                    ei.RemoveEventHandler(c, handler);
                }
            }
            else if (c is TabItem)
            {
                TabItem item = c as TabItem;
                item.MouseHover -= new EventHandler(this.ComponentMouseHover);
                item.MouseLeave -= new EventHandler(ComponentMouseLeave);
                item.MouseEnter -= new EventHandler(ComponentMouseEnter);
                item.MouseDown -= new MouseEventHandler(ComponentMouseDown);

            }
        }

        private void AddSuperTooltipInfo(IComponent c, SuperTooltipInfo info)
        {
            m_SuperTooltipInfo[c] = info;

            if (c is ISuperTooltipInfoProvider)
            {
                ISuperTooltipInfoProvider ip = c as ISuperTooltipInfoProvider;
                ip.DisplayTooltip += new EventHandler(this.ComponentMouseHover);
                ip.HideTooltip += new EventHandler(ComponentHideTooltip);
            }
            else if (c is ComboBoxItem)
            {
                ComboBoxItem item = c as ComboBoxItem;
                item.MouseHover += new EventHandler(this.ComponentMouseHover);
                item.MouseLeave += new EventHandler(ComponentMouseLeave);
                item.MouseEnter += new EventHandler(ComponentMouseEnter);
                item.MouseDown += new MouseEventHandler(ComponentMouseDown);
                item.ComboBoxEx.MouseHover += new EventHandler(this.ComponentMouseHover);
                item.ComboBoxEx.MouseLeave += new EventHandler(ComponentMouseLeave);
                item.ComboBoxEx.MouseEnter += new EventHandler(ComponentMouseEnter);
                item.ComboBoxEx.MouseDown += new MouseEventHandler(ComponentMouseDown);
            }
            else if (c is TextBoxItem)
            {
                TextBoxItem item = c as TextBoxItem;
                item.MouseHover += new EventHandler(this.ComponentMouseHover);
                item.MouseLeave += new EventHandler(ComponentMouseLeave);
                item.MouseEnter += new EventHandler(ComponentMouseEnter);
                item.MouseDown += new MouseEventHandler(ComponentMouseDown);
                item.TextBox.MouseHover += new EventHandler(this.ComponentMouseHover);
                item.TextBox.MouseLeave += new EventHandler(ComponentMouseLeave);
                item.TextBox.MouseEnter += new EventHandler(ComponentMouseEnter);
                item.TextBox.MouseDown += new MouseEventHandler(ComponentMouseDown);
            }
            else if (c is BaseItem)
            {
                BaseItem item = c as BaseItem;
                item.MouseHover += new EventHandler(this.ComponentMouseHover);
                item.MouseLeave += new EventHandler(ComponentMouseLeave);
                item.MouseEnter += new EventHandler(ComponentMouseEnter);
                item.MouseDown += new MouseEventHandler(ComponentMouseDown);
            }
            else if (c is RibbonBar)
            {
                RibbonBar bar = c as RibbonBar;
                bar.DialogLauncherMouseHover += new EventHandler(this.ComponentMouseHover);
                bar.DialogLauncherMouseEnter += new EventHandler(ComponentMouseEnter);
                bar.DialogLauncherMouseLeave += new EventHandler(ComponentMouseLeave);
                bar.DialogLauncherMouseDown += new MouseEventHandler(ComponentMouseDown);
            }
            else if (c is Control)
            {
                Control ctrl = c as Control;
                ctrl.MouseHover += new EventHandler(this.ComponentMouseHover);
                ctrl.MouseLeave += new EventHandler(ComponentMouseLeave);
                ctrl.MouseDown += new MouseEventHandler(ComponentMouseDown);
                ctrl.MouseEnter += new EventHandler(ComponentMouseEnter);
            }
#if (FRAMEWORK20)
            else if (c is ToolStripButton)
            {
                ToolStripButton item = c as ToolStripButton;
                item.MouseHover += new EventHandler(this.ComponentMouseHover);
                item.MouseLeave += new EventHandler(ComponentMouseLeave);
                item.MouseEnter += new EventHandler(ComponentMouseEnter);
                item.MouseDown += new MouseEventHandler(ComponentMouseDown);
                item.AutoToolTip = false;
                item.ToolTipText = "";
            }
#endif
            else if (c is AdvTree.Node || IsTreeGXType(c.GetType()))
            {
                Type nodeType = c.GetType();
                EventInfo ei = nodeType.GetEvent("NodeMouseHover");
                if (ei != null)
                {
                    Delegate handler = Delegate.CreateDelegate(typeof(EventHandler),
                    this, "ComponentMouseHover");
                    ei.AddEventHandler(c, handler);
                }

                ei = nodeType.GetEvent("NodeMouseLeave");
                if (ei != null)
                {
                    Delegate handler = Delegate.CreateDelegate(typeof(EventHandler),
                    this, "ComponentMouseLeave");
                    ei.AddEventHandler(c, handler);
                }

                ei = nodeType.GetEvent("NodeMouseEnter");
                if (ei != null)
                {
                    Delegate handler = Delegate.CreateDelegate(typeof(EventHandler),
                    this, "ComponentMouseEnter");
                    ei.AddEventHandler(c, handler);
                }

                ei = nodeType.GetEvent("NodeMouseDown");
                if (ei != null)
                {
                    Delegate handler = Delegate.CreateDelegate(typeof(MouseEventHandler),
                    this, "ComponentMouseDown");
                    ei.AddEventHandler(c, handler);
                }
            }
            else if (c is TabItem)
            {
                TabItem tab = c as TabItem;
                tab.MouseHover += new EventHandler(this.ComponentMouseHover);
                tab.MouseLeave += new EventHandler(ComponentMouseLeave);
                tab.MouseEnter += new EventHandler(ComponentMouseEnter);
                tab.MouseDown += new MouseEventHandler(ComponentMouseDown);
            }
        }

        void ComponentHideTooltip(object sender, EventArgs e)
        {
            this.HideTooltip();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ComponentMouseDown(object sender, MouseEventArgs e)
        {
            this.HideTooltip();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ComponentMouseEnter(object sender, EventArgs e)
        {
            if(sender is Control)
                ResetHover(sender as Control);

            if (m_ShowTooltipImmediately && m_Enabled)
            {
                ShowTooltip(sender);
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ComponentMouseLeave(object sender, EventArgs e)
        {
            if (!IsMouseOverSuperTooltip)
                this.HideDelayed();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ComponentMouseHover(object sender, EventArgs e)
        {
            if (m_ShowTooltipImmediately) return;

			m_HoverCount++;
			
            if(sender is Control && m_HoverCount<m_HoverTrigger)
				ResetHover(sender as Control);
            else if (sender is BaseItem && m_HoverCount < m_HoverTrigger)
            {
                Control container = GetContainerControl((BaseItem)sender);
                if (container != null)
                    ResetHover((Control)container);
            }
            else if (sender is TabItem && m_HoverCount < m_HoverTrigger)
                ResetHover((Control)((TabItem)sender).Parent);
            else if (sender is AdvTree.Node && m_HoverCount < m_HoverTrigger)
                ResetHover((Control)((AdvTree.Node)sender).TreeControl);
            else if (m_Enabled)
                ShowTooltip(sender);
        }

        private Control GetContainerControl(BaseItem item)
        {
            object container = item.ContainerControl;
            if (container is BaseItem)
                container = ((BaseItem)container).ContainerControl;
            return container as Control;
        }

        /// <summary>
        /// Gets or sets the hover delay multiplier which controls how fast tooltip appears. The value set here indicates
        /// how many hover events are needed to occur before the tooltip is displayed.
        /// </summary>
        [DefaultValue(2), Category("Behavior"), Description("Indicates hover delay multiplier which controls how fast tooltip appears. The value set here indicates how many hover events are needed to occur before the tooltip is displayed.")]
        public int HoverDelayMultiplier
        {
            get { return m_HoverTrigger; }
            set
            {
                m_HoverTrigger = value;
            }
        }
        

        private ComboBoxItem GetComboBoxItemFromComboBoxEx(Controls.ComboBoxEx c)
        {
            foreach (object o in m_SuperTooltipInfo.Keys)
            {
                if (o is ComboBoxItem && ((ComboBoxItem)o).ComboBoxEx == c)
                    return o as ComboBoxItem;
            }
            return null;
        }

        private bool IsMouseOverSuperTooltip
        {
            get
            {
                if (m_MouseOverSuperTooltip)
                    return true;

                if (m_Tooltip != null && m_Tooltip.Visible && m_Tooltip.Bounds.Contains(Control.MousePosition))
                    return true;

                return false;
            }
        }

        /// <summary>
        /// Raises the MarkupLinkClick event.
        /// </summary>
        protected virtual void OnMarkupLinkClick(MarkupLinkClickEventArgs e)
        {
            if (MarkupLinkClick != null)
                MarkupLinkClick(this, e);
        }

        /// <summary>
        /// Shows SuperTooltip for given object that has been registered using SetSuperTooltip method at specific location on the screen.
        /// </summary>
        /// <param name="sender">Object to show tooltip for. Object must be registered using SetSuperTooltip method before tooltip is shown for it.</param>
        /// <param name="screenPosition">Specifies the explicit position of the SuperTooltip in screen coordinates.</param>
        public void ShowTooltip(object sender, Point screenPosition)
        {
            ShowTooltip(sender, screenPosition, true);
        }

        /// <summary>
        /// Shows SuperTooltip for given object that has been registered using SetSuperTooltip method.
        /// </summary>
        /// <param name="sender">Object to show tooltip for. Object must be registered using SetSuperTooltip method before tooltip is shown for it.</param>
        public void ShowTooltip(object sender)
        {
            ShowTooltip(sender, Point.Empty, false);
        }

        /// <summary>
        /// Shows SuperTooltip for given object that has been registered using SetSuperTooltip method.
        /// </summary>
        /// <param name="sender">Object to show tooltip for. Object must be registered using SetSuperTooltip method before tooltip is shown for it.</param>
        private void ShowTooltip(object sender, Point screenPosition, bool useScreenPosition)
        {
            if (sender is Control && ((Control)sender).Focused && !m_ShowTooltipForFocusedControl)
                return;

            Rectangle controlRect = Rectangle.Empty;
            Rectangle actualControlRect = Rectangle.Empty;
            bool rightToLeft = false;

            // Check whether form that is hosting the control for super-tooltip is active and if not exit
            if (!IsFormActive(sender)) return;

            if (sender is Controls.ComboBoxEx)
            {
                BaseItem combo = GetComboBoxItemFromComboBoxEx(sender as Controls.ComboBoxEx);
                if (combo != null) sender = combo;
            }

            if (sender is BaseItem)
            {
                BaseItem item = sender as BaseItem;
                Control containerControl = GetContainerControl(item);
                if (item is PopupItem && ((PopupItem)item).Expanded && (containerControl is RibbonBar || containerControl is RibbonStrip))
                    return;
                Control itemControl = containerControl;
                if (itemControl != null)
                {
                    if (m_PositionBelowControl && !(itemControl is RibbonStrip && ((RibbonStrip)itemControl).QuickToolbarItems.Contains(item) || !(itemControl is RibbonBar)))
                        controlRect = new Rectangle(itemControl.PointToScreen(Point.Empty), itemControl.Size);
                    else
                        controlRect = new Rectangle(itemControl.PointToScreen(item.DisplayRectangle.Location), item.DisplayRectangle.Size);
                    actualControlRect = new Rectangle(itemControl.PointToScreen(item.DisplayRectangle.Location), item.DisplayRectangle.Size);
                    rightToLeft = containerControl.RightToLeft == RightToLeft.Yes;
                }
            }
            else if (sender is Control)
            {
                Control c = sender as Control;
                controlRect = new Rectangle(c.PointToScreen(Point.Empty), c.Size);
                actualControlRect = controlRect;
                rightToLeft = (c.RightToLeft == RightToLeft.Yes);
            }
            else if (sender is TabItem)
            {
                TabItem tab = sender as TabItem;
                TabStrip strip = tab.Parent;
                controlRect = new Rectangle(strip.PointToScreen(Point.Empty), strip.Size);
                actualControlRect = controlRect;
                rightToLeft = (strip.RightToLeft == RightToLeft.Yes);
            }
            else if (sender is ISuperTooltipInfoProvider)
            {
                ISuperTooltipInfoProvider ip = sender as ISuperTooltipInfoProvider;
                controlRect = ip.ComponentRectangle;
                actualControlRect = controlRect;
            }
            else if (sender is AdvTree.Node || IsTreeGXType(sender.GetType()))
            {
                controlRect = (Rectangle)TypeDescriptor.GetProperties(sender)["CellsBounds"].GetValue(sender);
                Control c = TypeDescriptor.GetProperties(sender)["TreeControl"].GetValue(sender) as Control;
                if (c != null)
                {
                    if (m_PositionBelowControl)
                        controlRect = new Rectangle(c.PointToScreen(Point.Empty), c.Size);
                    else
                        controlRect = new Rectangle(c.PointToScreen(controlRect.Location), controlRect.Size);
                    rightToLeft = (c.RightToLeft == RightToLeft.Yes);
                }
                actualControlRect = controlRect;
            }
#if (FRAMEWORK20)
            else if (sender is ToolStripButton)
            {
                ToolStripButton toolButton = sender as ToolStripButton;
                controlRect = toolButton.Bounds;
                actualControlRect = toolButton.Bounds;
            }
#endif

            if (controlRect.IsEmpty)
                return;

            SuperTooltipInfo info = GetSuperTooltipInfo(sender);
            if (info == null && sender is BaseItem)
            {
                // Try to find it by name...
                foreach (object o in m_SuperTooltipInfo.Keys)
                {
                    if (o is BaseItem && ((BaseItem)o).Name == ((BaseItem)sender).Name)
                    {
                        info = m_SuperTooltipInfo[o] as SuperTooltipInfo;
                        break;
                    }
                }
            }
            if (info == null)
                return;

            HideTooltip();

            Point p=new Point(Control.MousePosition.X,Control.MousePosition.Y+(int)(SystemInformation.CursorSize.Height * .6f));
            if (m_PositionBelowControl)
            {
                p.Y = Math.Max(controlRect.Bottom + 1, p.Y);
            }

            // Set the position explicitly
            if (useScreenPosition)
                p = screenPosition;

            m_Tooltip = new SuperTooltipControl();
            m_Tooltip.ShowTooltipDescription = m_ShowTooltipDescription;
            m_Tooltip.MaximumWidth = m_MaximumWidth;
            m_Tooltip.MouseActivateEnabled = false;
            m_Tooltip.MouseEnter += new EventHandler(SuperTooltip_MouseEnter);
            m_Tooltip.MouseLeave += new EventHandler(SuperTooltip_MouseLeave);
            m_Tooltip.MarkupLinkClick += new MarkupLinkClickEventHandler(Tooltip_MarkupLinkClick);
			if(m_DefaultFont!=null)
				m_Tooltip.Font=m_DefaultFont;
            if (rightToLeft)
                m_Tooltip.RightToLeft = RightToLeft.Yes;
            m_Tooltip.MinimumTooltipSize = m_MinimumTooltipSize;
			m_Tooltip.AntiAlias = m_AntiAlias;

            Size tooltipSize = Size.Empty;
            if (m_CheckTooltipPosition || m_CheckOnScreenPosition)
            {
                if (!info.CustomSize.IsEmpty && info.CustomSize.Width > 0 && info.CustomSize.Height > 0)
                    tooltipSize = info.CustomSize;
                else
                {
                    m_Tooltip.UpdateWithSuperTooltipInfo(info);
                    m_Tooltip.UpdateSuperTooltipSize(info); // RecalcSize();
                    tooltipSize = m_Tooltip.Size;
                }
            }

            if (m_CheckOnScreenPosition)
            {
                ScreenInformation screen = BarFunctions.ScreenFromPoint(p);
                if (screen != null)
                {
                    Rectangle r = new Rectangle(p, tooltipSize);
                    System.Drawing.Size layoutArea = screen.WorkingArea.Size;

                    if (r.Right > screen.WorkingArea.Right)
                    {
                        r.X = r.X - (r.Right - screen.WorkingArea.Right);
                        //if (r.IntersectsWith(controlRect))
                        //{
                        //    r.X = controlRect.X - r.Width;
                        //}
                    }
                    if (r.Bottom > screen.Bounds.Bottom)
                    {
                        r.Y = screen.Bounds.Bottom - r.Height;
                        //if (r.IntersectsWith(controlRect))
                        //{
                        //    r.Y = controlRect.Y - r.Height;
                        //}
                    }

                    p = r.Location;
                }
            }

            if (m_CheckTooltipPosition && !actualControlRect.IsEmpty)
            {
                Rectangle r = new Rectangle(p, tooltipSize);

                if (r.IntersectsWith(actualControlRect))
                {
                    if (actualControlRect.Y - r.Height >= 0)
                        r.Y = actualControlRect.Y - r.Height;
                    else if (actualControlRect.X - r.Width >= 0)
                        r.Width = actualControlRect.X - r.Width;

                    p = r.Location;
                }
            }
            
            if (BeforeTooltipDisplay != null)
            {
                SuperTooltipEventArgs eventArgs = new SuperTooltipEventArgs(sender, info, p);
                BeforeTooltipDisplay(this, eventArgs);
                if (eventArgs.Cancel)
                {
                    m_Tooltip.Dispose();
                    m_Tooltip = null;
                    return;
                }
                p = eventArgs.Location;
            }

            m_Tooltip.ShowTooltip(info, p.X, p.Y, false);
            m_DurationDisplayed = 0;
            CreateActiveWindowTimer();
        }

        /// <summary>
        /// Raises the TooltipClosed event.
        /// </summary>
        /// <param name="e">Provides event arguments.</param>
        protected virtual void OnTooltipClosed(EventArgs e)
        {
            if(TooltipClosed!=null)
                TooltipClosed(this, e);
        }

        private SuperTooltipInfo GetSuperTooltipInfo(object sender)
        {
            if (sender is TextBoxX && !m_SuperTooltipInfo.Contains(sender))
            {
                foreach (object o in m_SuperTooltipInfo.Keys)
                {
                    if(o is TextBoxItem && ((TextBoxItem)o).TextBox == sender)
                        return m_SuperTooltipInfo[o] as SuperTooltipInfo;
                }
            }
            else if (sender is ComboBoxEx && !m_SuperTooltipInfo.Contains(sender))
            {
                foreach (object o in m_SuperTooltipInfo.Keys)
                {
                    if (o is ComboBoxItem && ((ComboBoxItem)o).ComboBoxEx == sender)
                        return m_SuperTooltipInfo[o] as SuperTooltipInfo;
                }
            }

            return m_SuperTooltipInfo[sender] as SuperTooltipInfo;
        }

        private bool IsFormActive(object sender)
        {
            if (m_IgnoreFormActiveState) return true;
            Control c = null;
            if (sender is Control)
                c = sender as Control;
            else if (sender is BaseItem)
                c = GetContainerControl((BaseItem)sender);

            if (c != null)
            {
                Form f = c.FindForm();
                if (f!=null && f.IsMdiChild && f.MdiParent.ActiveMdiChild == f)
                    return true;
                if (f != null && Form.ActiveForm != f)
                    return false;
            }
            return true;
        }

        private void Tooltip_MarkupLinkClick(object sender, MarkupLinkClickEventArgs e)
        {
            OnMarkupLinkClick(e);
        }

        private bool m_MouseOverSuperTooltip = false;
        private void SuperTooltip_MouseLeave(object sender, EventArgs e)
        {
            m_MouseOverSuperTooltip = false;
            HideTooltip();
        }

        private void SuperTooltip_MouseEnter(object sender, EventArgs e)
        {
            m_MouseOverSuperTooltip = true;
        }

        /// <summary>
        /// Hides tooltip if it is visible.
        /// </summary>
        public void HideTooltip()
        {
			m_HoverCount=0;
            DestroyActiveWindowTimer();
            DisposeHideDelayedTimer();
            if (m_Tooltip != null)
            {
                m_Tooltip.Hide();
                m_Tooltip.Dispose();
                m_Tooltip = null;
                OnTooltipClosed(new EventArgs());
            }
            m_MouseOverSuperTooltip = false;
        }

        private void HideDelayed()
        {
            if (m_DelayTooltipHideDuration <= 100)
            {
                HideTooltip();
                return;
            }

            if (m_HideDelayedTimer == null)
            {
                m_HideDelayedTimer = new Timer();
                m_HideDelayedTimer.Tick += new EventHandler(HideDelayedTimer_Tick);
                m_HideDelayedTimer.Interval = m_DelayTooltipHideDuration;
                m_HideDelayedTimer.Enabled = true;
            }
        }

        private void HideDelayedTimer_Tick(object sender, EventArgs e)
        {
            if(!IsMouseOverSuperTooltip)
                HideTooltip();
        }

        private void DisposeHideDelayedTimer()
        {
            if (m_HideDelayedTimer != null)
            {
                m_HideDelayedTimer.Stop();
                m_HideDelayedTimer.Enabled = false;
                m_HideDelayedTimer.Dispose();
                m_HideDelayedTimer = null;
            }
        }

        private void CreateActiveWindowTimer()
        {
            if (m_ActiveWindowTimer != null || m_TimerOperation)
                return;
            m_TimerOperation = true;
            try
            {
                m_ActiveWindowPtr = NativeFunctions.GetActiveWindow();
                m_ActiveWindowTimer = new Timer();
                m_ActiveWindowTimer.Interval = 300;
                m_ActiveWindowTimer.Tick += new EventHandler(ActiveWindowTimerTick);
                m_ActiveWindowTimer.Start();
            }
            finally
            {
                m_TimerOperation = false;
            }
        }

        void ActiveWindowTimerTick(object sender, EventArgs e)
        {
            m_DurationDisplayed += m_ActiveWindowTimer.Interval;
            if (m_ActiveWindowPtr != IntPtr.Zero)
            {
                IntPtr active = NativeFunctions.GetActiveWindow();
                if (active != m_ActiveWindowPtr)
                {
                    m_ActiveWindowTimer.Stop();
                    HideTooltip();
                    return;
                }
            }
            
            if (m_TooltipDuration > 0 && m_DurationDisplayed > m_TooltipDuration * 1000 && !m_MouseOverSuperTooltip)
            {
                m_ActiveWindowTimer.Stop();
                HideTooltip();
            }
        }

        private bool m_TimerOperation = false;
        private void DestroyActiveWindowTimer()
        {
            if (m_TimerOperation) return;
            m_TimerOperation = true;
            try
            {
                if (m_ActiveWindowTimer != null)
                {
                    m_ActiveWindowTimer.Enabled = false;
                    m_ActiveWindowTimer.Stop();
                    m_ActiveWindowTimer.Tick -= new EventHandler(ActiveWindowTimerTick);
                    m_ActiveWindowTimer.Dispose();
                    m_ActiveWindowTimer = null;
                }
            }
            finally
            {
                m_TimerOperation = false;
            }
        }

        /// <summary>
        /// Returns whether tooltip is visible.
        /// </summary>
        [Browsable(false)]
        public bool IsTooltipVisible
        {
            get
            {
                if (m_Tooltip != null && m_Tooltip.Visible)
                    return true;
                return false;
            }
        }

        /// <summary>
        /// Gets reference to instance of tooltip control if any has been created at the time call is made.
        /// </summary>
        [Browsable(false)]
        public SuperTooltipControl SuperTooltipControl
        {
            get { return m_Tooltip; }
        }

		/// <summary>
		/// Gets or sets default tooltip font. Default value is null which means that default system font is used.
		/// </summary>
		[Browsable(true), DefaultValue(null), Category("Appearance"), Description("Indicates default tooltip font.")]
		public Font DefaultFont
		{
			get {return m_DefaultFont;}
			set {m_DefaultFont=value;}
		}

        /// <summary>
        /// Resets Hoover timer.
        /// </summary>
        private void ResetHover(Control c)
        {
            if (c==null || !BarFunctions.IsHandleValid(c))
                return;
            // We need to reset hover thing since it is fired only first time mouse hovers inside the window and we need it for each of our items
            NativeFunctions.TRACKMOUSEEVENT tme = new NativeFunctions.TRACKMOUSEEVENT();
            tme.dwFlags = NativeFunctions.TME_QUERY;
            tme.hwndTrack = c.Handle;
            tme.cbSize = System.Runtime.InteropServices.Marshal.SizeOf(tme);
            NativeFunctions.TrackMouseEvent(ref tme);
            tme.dwFlags = tme.dwFlags | NativeFunctions.TME_HOVER;
            NativeFunctions.TrackMouseEvent(ref tme);
        }

        /// <summary>
        /// Gets or sets whether complete tooltip is shown including header, body and footer. Default value is true. When set to false only tooltip header will be shown.
        /// Providing this option to your end users as part of your application setting allows them to customize the level of information displayed and reduce it after they are familiar with your product.
        /// </summary>
        [Browsable(true), DefaultValue(true), Category("Behavior"), Description("Indicates whether complete tooltip is shown including header, body and footer. When set to false only tooltip header is shown.")]
        public bool ShowTooltipDescription
        {
            get
            {
                return m_ShowTooltipDescription;
            }
            set
            {
                m_ShowTooltipDescription = value;
            }
        }

        /// <summary>
        /// Gets or sets whether tooltip is shown when control that tooltip is assigned to is focused. You can set this value to false to disable Tooltip display when control receives input focus. Default value is true.
        /// </summary>
        /// <remarks>This property is effective only when Super Tooltip is assigned to the controls that inherit from System.Windows.Forms.Control class and it relies on Focused property of respective control for proper function.</remarks>
        [Browsable(true), DefaultValue(true), Category("Behavior"), Description("Indicates whether tooltip is shown when control that tooltip is assigned to is focused.")]
        public bool ShowTooltipForFocusedControl
        {
            get
            {
                return m_ShowTooltipForFocusedControl;
            }
            set
            {
                m_ShowTooltipForFocusedControl = value;
            }
        }

        private bool IsTreeGXType(Type type)
        {
            return type.BaseType.FullName == TREEGX_NODE_CLASS_NAME || type.FullName == TREEGX_NODE_CLASS_NAME;
        }
        #endregion

        #region IExtenderProvider Members

        public bool CanExtend(object extendee)
        {
            if (extendee is Control || extendee is BaseItem ||
                extendee is ISuperTooltipInfoProvider || IsTreeGXType(extendee.GetType()) ||
                extendee is TabItem)
                return true;
#if FRAMEWORK20
            if (extendee is ToolStripButton) return true;
#endif
#if (!NOTREE)
            if (extendee is DevComponents.AdvTree.Node) return true;
#endif
            return false;
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
#endif
        #endregion
    }

    #region SuperTooltipInfo Class
    /// <summary>
    /// Provides information about SuperTooltip attached to a component.
    /// </summary>
    [TypeConverter("DevComponents.DotNetBar.Design.SuperTooltipInfoConverter, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf"), DesignTimeVisible(false), ToolboxItem(false), Localizable(true)]
    public class SuperTooltipInfo
    {
        private bool m_HeaderVisible=true;
        private bool m_FooterVisible=true;
        private string m_HeaderText="";
        private string m_FooterText="";
        private Image m_FooterImage=null;
        private string m_BodyText="";
        private Image m_BodyImage=null;
        private Size m_CustomSize = Size.Empty;
        private eTooltipColor m_Color = eTooltipColor.Gray;

        /// <summary>
        /// Creates new instance of the class.
        /// </summary>
        public SuperTooltipInfo()
        {
        }

        /// <summary>
        /// Creates new instance of the class with specified parameters.
        /// </summary>
        public SuperTooltipInfo(string headerText, string footerText, string bodyText, Image bodyImage, Image footerImage, eTooltipColor color, bool headerVisible, bool footerVisible, Size customSize)
        {
            m_HeaderText = headerText;
            m_FooterText = footerText;
            m_BodyText = bodyText;
            m_BodyImage = bodyImage;
			m_FooterImage = footerImage;
            m_HeaderVisible = headerVisible;
            m_FooterVisible = footerVisible;
            m_CustomSize = customSize;
			m_Color=color;
        }

        /// <summary>
        /// Creates new instance of the class with specified parameters.
        /// </summary>
        public SuperTooltipInfo(string headerText, string footerText, string bodyText, Image bodyImage, Image footerImage, eTooltipColor color)
        {
            m_HeaderText = headerText;
            m_FooterText = footerText;
            m_BodyText = bodyText;
            m_BodyImage = bodyImage;
            m_FooterImage = footerImage;
            m_Color = color;
        }

        /// <summary>
        /// Gets or sets whether tooltip header text is visible or not. Default value is true.
        /// </summary>
        [Browsable(true), DefaultValue(true), Description("Indicates whether header text is visible."), Localizable(true)]
        public bool HeaderVisible
        {
            get { return m_HeaderVisible; }
            set { m_HeaderVisible = value; }
        }

        /// <summary>
        /// Gets or sets whether tooltip footer text is visible or not. Default value is true.
        /// </summary>
        [Browsable(true), DefaultValue(true), Description("Indicates whether footer text is visible."), Localizable(true)]
        public bool FooterVisible
        {
            get { return m_FooterVisible; }
            set { m_FooterVisible = value; }
        }

        /// <summary>
        /// Gets or sets the header text.
        /// </summary>
        [Browsable(true), DefaultValue(""), Description("Indicates header text."), Localizable(true)]
        public string HeaderText
        {
            get { return m_HeaderText; }
            set { m_HeaderText = value; }
        }

        /// <summary>
        /// Gets or sets the footer text.
        /// </summary>
        [Browsable(true), DefaultValue(""), Description("Indicates footer text."), Localizable(true)]
        public string FooterText
        {
            get { return m_FooterText; }
            set { m_FooterText = value; }
        }

        /// <summary>
        /// Gets or sets body text.
        /// </summary>
        [Browsable(true), DefaultValue(""), Description("Indicates body text."), Localizable(true)]
        public string BodyText
        {
            get { return m_BodyText; }
            set { m_BodyText = value; }
        }

        /// <summary>
        /// Gets or sets body image displayed to the left of body text.
        /// </summary>
        [Browsable(true), DefaultValue(null), Description("Indicates body image displayed to the left of body text."), Localizable(true)]
        public Image BodyImage
        {
            get { return m_BodyImage; }
            set { m_BodyImage = value; }
        }

        /// <summary>
        /// Gets or sets footer image displayed to the left of footer text.
        /// </summary>
        [Browsable(true), DefaultValue(null), Description("Indicates footer image displayed to the left of footer text."), Localizable(true)]
        public Image FooterImage
        {
            get { return m_FooterImage; }
            set { m_FooterImage = value; }
        }


        /// <summary>
        /// Gets or sets the custom size for tooltip. Default value is 0,0 which indicates that tooltip is automatically
        /// resized based on the content.
        /// </summary>
        [Browsable(true), Description("Indicates custom size for tooltip."), Localizable(true)]
        public Size CustomSize
        {
            get { return m_CustomSize; }
            set { m_CustomSize = value; }
        }

        /// <summary>
        /// Returns whether property should be serialized.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeCustomSize()
        {
            return !m_CustomSize.IsEmpty;
        }

        /// <summary>
        /// Gets or sets predefined tooltip color.
        /// </summary>
        [Browsable(true), DefaultValue(eTooltipColor.Gray), Description("Indicates predefined tooltip color."), Localizable(true)]
        public eTooltipColor Color
        {
            get { return m_Color; }
            set { m_Color = value; }
        }
    }
    #endregion

    #region ISuperTooltipInfoProvider
    /// <summary>
    /// Extensibility interface that custom components may implement
    /// so SuperTooltip can be provided for them.
    /// </summary>
    public interface ISuperTooltipInfoProvider
    {
        /// <summary>
        /// This event should be triggered by the component when it wants to display SuperTooltip. Normally
        /// this event would be equivalent of MouseHover event but you can trigger it in response to anything else.
        /// Sender for this event must be object that implements ISuperTooltipInfoProvider interface.
        /// </summary>
        event EventHandler DisplayTooltip;
        /// <summary>
        /// This event should be triggered by component when it wants to hide SuperTooltip. For example this event
        /// might be triggered when mouse leaves your component. Sender of this event must be object that implements ISuperTooltipInfoProvider interface.
        /// </summary>
        event EventHandler HideTooltip;
        /// <summary>
        /// Returns rectangle of the visible area of the component in screen coordinates. This rectangle is used
        /// to position SuperTooltip on the screen.
        /// </summary>
        Rectangle ComponentRectangle { get;}
    }
    #endregion

    #region SuperTooltipEventArgs
    /// <summary>
    /// Delegate for SuperTooltip events.
    /// </summary>
    public delegate void SuperTooltipEventHandler(object sender, SuperTooltipEventArgs e);

    /// <summary>
    /// Represents event arguments for PanelChanging event.
    /// </summary>
    public class SuperTooltipEventArgs : EventArgs
    {
        /// <summary>
        /// Set to true to cancel display of tooltip.
        /// </summary>
        public bool Cancel = false;
        /// <summary>
        /// Object that has triggered displaying of tooltip
        /// </summary>
        public readonly object Source;
        /// <summary>
        /// Information that will be used to populate tooltip.
        /// </summary>
        public readonly SuperTooltipInfo TooltipInfo;
        /// <summary>
        /// Location where tooltip will be displayed. You can change the location here to display tooltip at different position.
        /// </summary>
        public Point Location;
        /// <summary>
        /// Default constructor.
        /// </summary>
        public SuperTooltipEventArgs(object source, SuperTooltipInfo info, Point location)
        {
            this.Source = source;
            this.TooltipInfo = info;
            this.Location = location;
        }
    }
    #endregion
}
