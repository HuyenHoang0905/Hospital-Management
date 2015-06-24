using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System;
using System.Collections;
using System.Text;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Represents Rendering control composed of two parts, RibbonStrip and multiple RibbonBar controls per strip.
	/// </summary>
    [ToolboxBitmap(typeof(RibbonControl), "Ribbon.RibbonControl.ico"), ToolboxItem(true), Designer("DevComponents.DotNetBar.Design.RibbonControlDesigner, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf"), System.Runtime.InteropServices.ComVisible(false)]
	public class RibbonControl:System.Windows.Forms.ContainerControl
    {
        #region Events
        /// <summary>
        /// Occurs when Quick Access Toolbar placement is changed, i.e. below or above the Ribbon.
        /// </summary>
        [Description("Occurs when Quick Access Toolbar placement is changed, i.e. below or above the Ribbon.")]
        public event EventHandler QatPlacementChanged;
        /// <summary>
        /// Occurs just before the customize popup menu is displayed and provides the ability to cancel the menu display as well
        /// as to add/remove the menu items from the customize popup menu.
        /// </summary>
        public event CustomizeMenuPopupEventHandler BeforeCustomizeMenuPopup;

        /// <summary>
        /// Occurs before an item is added to the quick access toolbar as result of user action. This event provides ability to
        /// cancel the addition of the item by setting the Cancel=true of event arguments.
        /// </summary>
        public event CustomizeMenuPopupEventHandler BeforeAddItemToQuickAccessToolbar;

        /// <summary>
        /// Occurs before an item is removed from the quick access toolbar as result of user action. This event provides ability to
        /// cancel the addition of the item by setting the Cancel=true of event arguments.
        /// </summary>
        public event CustomizeMenuPopupEventHandler BeforeRemoveItemFromQuickAccessToolbar;

        /// <summary>
        /// Occurs when DotNetBar is looking for translated text for one of the internal text that are
        /// displayed on menus, toolbars and customize forms. You need to set Handled=true if you want
        /// your custom text to be used instead of the built-in system value.
        /// </summary>
        public event DotNetBarManager.LocalizeStringEventHandler LocalizeString;

        /// <summary>
        /// Occurs when Item on ribbon tab strip or quick access toolbar is clicked.
        /// </summary>
        [Description("Occurs when Item on ribbon tab strip or quick access toolbar is clicked.")]
        public event EventHandler ItemClick;

        /// <summary>
        /// Occurs before Quick Access Toolbar dialog is displayed. This event provides the opportunity to cancel the showing of
        /// built-in dialog and display custom customization dialog. You can also set the Dialog property of the event arguments to
        /// the custom dialog you want used instead of the DotNetBar system customization dialog.
        /// </summary>
        public event QatCustomizeDialogEventHandler BeforeQatCustomizeDialog;
        /// <summary>
        /// Occurs after the Quick Access Toolbar dialog is closed.
        /// </summary>
        public event QatCustomizeDialogEventHandler AfterQatCustomizeDialog;

        /// <summary>
        /// Occurs after any changes done on the Quick Access Toolbar dialog are applied to the actual Quick Access Toolbar.
        /// </summary>
        public event EventHandler AfterQatDialogChangesApplied;

        /// <summary>
        ///     Occurs after selected Ribbon tab has changed. You can use
        ///     <see cref="SelectedRibbonTabItem">RibbonControl.SelectedRibbonTabItem</see>
        ///     property to get reference to newly selected tab.
        /// </summary>
        public event EventHandler SelectedRibbonTabChanged;

        /// <summary>
        /// Occurs before selected RibbonPanel is displayed on popup while ribbon is collapsed. This event gives you the opportunity to cancel the popup of the ribbon panel.
        /// </summary>
        [Description("Occurs before selected RibbonPanel is displayed on popup while ribbon is collapsed.")]
        public event CancelEventHandler BeforeRibbonPanelPopup;
        /// <summary>
        /// Occurs after selected RibbonPanel is displayed on popup while ribbon is collapsed.
        /// </summary>
        [Description("Occurs after selected RibbonPanel is displayed on popup while ribbon is collapsed.")]
        public event EventHandler AfterRibbonPanelPopup;

        /// <summary>
        /// Occurs before RibbonPanel popup is closed and provides opportunity to cancel the closing. Note that if you cancel closing of ribbon popup you are
        /// responsible for closing the popup.
        /// </summary>
        [Description("Occurs before RibbonPanel popup is closed.")]
        public event RibbonPopupCloseEventHandler BeforeRibbonPanelPopupClose;

        /// <summary>
        /// Occurs after RibbonPanel popup is closed.
        /// </summary>
        [Description("Occurs after RibbonPanel popup is closed.")]
        public event EventHandler AfterRibbonPanelPopupClose;

        /// <summary>
        /// Occurs when text markup link from TitleText markup is clicked. Markup links can be created using "a" tag, for example:
        /// <a name="MyLink">Markup link</a>
        /// </summary>
        [Description("Occurs when text markup link from TitleText markup is clicked.")]
        public event MarkupLinkClickEventHandler TitleTextMarkupLinkClick;

        /// <summary>
        /// Occurs after Expanded property has changed.
        /// </summary>
        [Description("Occurs after Expanded property has changed.")]
        public event EventHandler ExpandedChanged;
        #endregion

        #region Private Variables and Constructor
        private RibbonStrip m_RibbonStrip=null;
		private bool m_Expanded=true;
		//private int m_ExpandedHeight=0;
        private bool m_MdiSystemItemVisible = true;
        private Form m_ActiveMdiChild = null;
        private bool m_MdiChildMaximized = false;
        private bool m_AutoSize = false;
        private ShadowPaintInfo m_ShadowPaintInfo = null;
        private bool m_UseCustomizeDialog = true;
        private bool m_EnableQatPlacement = true;
        
        private eCategorizeMode m_CategorizeMode = eCategorizeMode.RibbonBar;
        private Ribbon.QatToolbar m_QatToolbar = null;
        private bool m_QatPositionedBelow = false;
        private int DefaultBottomDockPadding = 2;
        private Ribbon.SubItemsQatCollection m_QatSubItemsCollection = null;
        private bool m_QatLayoutChanged = false;
        private RibbonLocalization m_SystemText = new RibbonLocalization();
        private Form m_PreviousMergedForm = null;
        private bool m_AllowMerge = true;
        private bool m_UseExternalCustomization = false;
        private const string SYS_CUSTOMIZE_POPUP_MENU = "syscustomizepopupmenu";
        private bool m_ExpandedQatBelowRibbon = false;
        private int m_ExpandedQatHeight = 0;
        private bool m_PopupMode = false;
        private RibbonPanel m_PopupRibbonPanel = null;
        private bool m_MenuTabsEnabled = true;
        private ContextMenuBar m_GlobalContextMenuBar = null;
        private SubItemsCollection m_QatFrequentCommands = new SubItemsCollection(null);

        /// <summary>
        /// Gets the name of the QAT Customize Item which is used to display the QAT Customize Dialog box.
        /// </summary>
        public static readonly string SysQatCustomizeItemName = "sysCustomizeQuickAccessToolbar";
        /// <summary>
        /// Gets the name of the Add to Quick Access Toolbar context menu item.
        /// </summary>
        public static readonly string SysQatAddToItemName = "sysAddToQuickAccessToolbar";
        /// <summary>
        /// Gets the name of the Remove from Quick Access Toolbar context menu item.
        /// </summary>
        public static readonly string SysQatRemoveFromItemName = "sysRemoveFromQuickAccessToolbar";
        /// <summary>
        /// Gets the name of the QAT placement change context menu item.
        /// </summary>
        public static readonly string SysQatPlaceItemName = "sysPlaceQuickAccessToolbar";
        /// <summary>
        /// Gets the name of the Minimize Ribbon Item which is used to minimize the ribbon.
        /// </summary>
        public static readonly string SysMinimizeRibbon = "sysMinimizeRibbon";
        /// <summary>
        /// Gets the name of the Maximize Ribbon Item which is used to maximize the ribbon.
        /// </summary>
        public static readonly string SysMaximizeRibbon = "sysMaximizeRibbon";
        /// <summary>
        /// Gets the name of the label displayed on Quick Access Toolbar customize popup menu.
        /// </summary>
        public static readonly string SysQatCustomizeLabelName = "sysCustomizeQuickAccessToolbarLabel";
        /// <summary>
        /// Gets the string that is used as starting name for the frequently used QAT menu items created when QAT Customize menu is displayed.
        /// </summary>
        public static readonly string SysFrequentlyQatNamePart = "sysQatFrequent_";

		public RibbonControl()
        {
            // This forces the initialization out of paint loop which speeds up how fast components show up
            BaseRenderer renderer = Rendering.GlobalManager.Renderer;

            m_QatFrequentCommands.IgnoreEvents = true;
            m_QatFrequentCommands.AllowParentRemove = false;

            this.SetStyle(ControlStyles.AllPaintingInWmPaint
                  | ControlStyles.ResizeRedraw
                  | DisplayHelp.DoubleBufferFlag
                  | ControlStyles.UserPaint
                  | ControlStyles.Opaque
                  , true);
			m_RibbonStrip=new RibbonStrip();
			m_RibbonStrip.Dock=DockStyle.Top;
			m_RibbonStrip.Height=32;
            m_RibbonStrip.ItemAdded += new System.EventHandler(RibbonStripItemAdded);
            m_RibbonStrip.SizeChanged += new System.EventHandler(RibbonStripSizeChanged);
            m_RibbonStrip.ItemRemoved += new ItemControl.ItemRemovedEventHandler(RibbonStripItemRemoved);
            m_RibbonStrip.LocalizeString += new DotNetBarManager.LocalizeStringEventHandler(RibbonStripLocalizeString);
            m_RibbonStrip.ItemClick += new System.EventHandler(RibbonStripItemClick);
            m_RibbonStrip.ButtonCheckedChanged += new EventHandler(RibbonStripButtonCheckedChanged);
            m_RibbonStrip.TitleTextMarkupLinkClick += new MarkupLinkClickEventHandler(RibbonStripTitleTextMarkupLinkClick);
			this.Controls.Add(m_RibbonStrip);
            this.TabStop = false;
            this.DockPadding.Bottom = DefaultBottomDockPadding;

            StyleManager.Register(this);
		}
        protected override void Dispose(bool disposing)
        {
            StyleManager.Unregister(this);
            ReleaseParentForm();
            base.Dispose(disposing);
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
            SetStyle();
        }

        /// <summary>
        /// Gets the collection of the Quick Access Toolbar Frequently used commands. You should add existing buttons to this collection that
        /// you already have on the RibbonBar controls or on the application menu. The list will be used to construct the frequently used
        /// menu that is displayed when Customize Quick Access Toolbar menu is displayed and it allows end-user to remove and add these
        /// frequently used commands to the QAT directly from this menu.
        /// Note that items you add here should not be items that are already on Quick Access Toolbar, i.e. in RibbonControl.QuickToolbarItems collection.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SubItemsCollection QatFrequentCommands
        {
            get
            {
                return m_QatFrequentCommands;
            }
        }

        /// <summary>
        /// Gets or sets whether KeyTips functionality is enabled. Default value is true.
        /// </summary>
        [DefaultValue(true), Browsable(true), Category("Behavior"), Description("Indicates whether KeyTips functionality is enabled.")]
        public bool KeyTipsEnabled
        {
            get { return m_RibbonStrip.KeyTipsEnabled; }
            set { m_RibbonStrip.KeyTipsEnabled = value; }
        }

        internal bool IsDesignMode
        {
            get { return this.DesignMode; }
        }

        protected virtual void OnSelectedRibbonTabChanged(EventArgs e)
        {
            if (SelectedRibbonTabChanged != null)
                SelectedRibbonTabChanged(this, e);
        }

        void RibbonStripButtonCheckedChanged(object sender, EventArgs e)
        {
            if (sender is RibbonTabItem && ((RibbonTabItem)sender).Checked)
            {
                AutoSyncSize();
                OnSelectedRibbonTabChanged(new EventArgs());
            }
        }

        private void RibbonStripItemClick(object sender, System.EventArgs e)
        {
            if (ItemClick != null)
                ItemClick(sender, e);
        }

        private void RibbonStripLocalizeString(object sender, LocalizeEventArgs e)
        {
            if (LocalizeString != null)
                LocalizeString(this, e);
        }

        /// <summary>
        /// Gets or sets whether merge functionality is enabled for the control. Default value is true.
        /// </summary>
        [Browsable(true), DefaultValue(true), Category("Behaviour"), Description("Indicates whether merge functionality is enabled for the control.")]
        public bool AllowMerge
        {
            get { return m_AllowMerge; }
            set { m_AllowMerge = value; }
        }

        /// <summary>
        /// Gets or sets whether control height is set automatically based on the content. Default value is false.
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), Category("Layout"), Description("Indicates whether control height is set automatically."), DefaultValue(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
#if FRAMEWORK20
        public override bool AutoSize
#else
        public virtual bool AutoSize
#endif
        {
            get { return m_AutoSize; }
            set
            {
                m_AutoSize = value;
                OnAutoSizeChanged();
            }
        }
        private void OnAutoSizeChanged()
        {
            AutoSyncSize();
        }

        /// <summary>
        /// Sets the height of the control to the automatically calcualted height based on content.
        /// </summary>
        public virtual void AutoSyncSize()
        {
            AutoSyncSize(true);
        }

        private int m_SyncingSize = 0;
        /// <summary>
        /// Sets the height of the control to the automatically calcualted height based on content.
        /// </summary>
        private void AutoSyncSize(bool layoutSelectedPanel)
        {
            if (!m_AutoSize || m_SyncingSize > 0 || !BarFunctions.IsHandleValid(this))
                return;

            m_SyncingSize++;
            try
            {
                RecalculateAutoSize(layoutSelectedPanel);
            }
            finally
            {
                m_SyncingSize--;
            }
        }

        private int GetItemControlHeight(ItemControl c)
        {
            int height = c.GetAutoSizeHeight();
#if FRAMEWORK20
            if (!c.MinimumSize.IsEmpty && c.MinimumSize.Height > height)
                height = (c.MinimumSize.Height + ElementStyleLayout.VerticalStyleWhiteSpace(c.GetPaintBackgroundStyle()));
            else if (!c.MaximumSize.IsEmpty && height > c.MaximumSize.Height)
                height = c.MaximumSize.Height + ElementStyleLayout.VerticalStyleWhiteSpace(c.GetPaintBackgroundStyle());
#endif
            return height;
        }

        private void RecalculateAutoSize()
        {
            RecalculateAutoSize(true);
        }

        private int GetAutoPanelHeight(RibbonPanel panel)
        {
            int height = 0;
            if (panel == null) return height;
            int maxHeight = 0;
            foreach (Control c in panel.Controls)
            {
                ItemControl ic = c as ItemControl;
                if (ic == null) continue;
                int ah = ic.GetAutoSizeHeight();
                if (ic != null && ah > maxHeight)
                {
                    int h = GetItemControlHeight(ic);
                    if (h > maxHeight)
                        maxHeight = h;
                }
            }
            
            if (maxHeight > 0)
                height += maxHeight;
            
            return height;
        }

        private void RecalculateAutoSize(bool layoutSelectedPanel)
        {
            int height = m_RibbonStrip.Height;
            if (this.Expanded)
            {
                if (this.SelectedRibbonTabItem != null)
                {
                    RibbonPanel panel = this.SelectedRibbonTabItem.Panel;
                    if (panel != null)
                    {
                        int maxHeight = GetAutoPanelHeight(panel);

                        if (maxHeight > 0)
                            height += maxHeight;
                        else
                            height = 0;
                    }
                    else
                        height = 0;
                }
                else
                    height = 0;
            }
            else
                height -= 4;

            if (height > 0)
            {
                height += this.DockPadding.Bottom + this.DockPadding.Top;
                if (m_QatPositionedBelow)
                {
                    height += (m_QatToolbar.Height);
                }
            }
            
            if (this.Height != height && height > 0)
            {
                RibbonPanel selectedPanel = null;
                if (!layoutSelectedPanel)
                {
                    RibbonTabItem tab = this.SelectedRibbonTabItem;
                    if (tab != null && tab.Panel != null)
                    {
                        selectedPanel = tab.Panel;
                        selectedPanel.SuspendLayout();
                    }
                }
                    
                this.Height = height;

                if (selectedPanel != null)
                    selectedPanel.ResumeLayout(false);
            }
        }

        /// <summary>
        /// Gets or sets whether anti-alias smoothing is used while painting. Default value is false.
        /// </summary>
        [DefaultValue(true), Browsable(true), Category("Appearance"), Description("Gets or sets whether anti-aliasing is used while painting.")]
        public bool AntiAlias
        {
            get { return m_RibbonStrip.AntiAlias; }
            set
            {
                m_RibbonStrip.AntiAlias = value;
                this.Invalidate();
            }
        }

        internal Rendering.Office2007ColorTable GetOffice2007ColorTable()
        {
            Rendering.Office2007Renderer r = Rendering.GlobalManager.Renderer as Rendering.Office2007Renderer;
            if (r != null)
                return r.ColorTable;
            return new Rendering.Office2007ColorTable();
        }

		/// <summary>
		/// Performs the setup of the RibbonPanel with the current style of the Ribbon Control.
		/// </summary>
		/// <param name="panel">Panel to apply style changes to.</param>
        public void SetRibbonPanelStyle(RibbonPanel panel)
        {
            if (this.DesignMode)
            {
                TypeDescriptor.GetProperties(panel.DockPadding)["Left"].SetValue(panel.DockPadding, 3);
                TypeDescriptor.GetProperties(panel.DockPadding)["Right"].SetValue(panel.DockPadding, 3);
                TypeDescriptor.GetProperties(panel.DockPadding)["Bottom"].SetValue(panel.DockPadding, 3);
            }
            else
            {
                panel.DockPadding.Left = 3;
                panel.DockPadding.Right = 3;
                panel.DockPadding.Bottom = 3;
            }
        }

        /// <summary>
        /// Creates new Rendering Tab at specified position, creates new associated panel and adds them to the control.
        /// </summary>
        /// <param name="text">Specifies the text displayed on the tab.</param>
        /// <param name="name">Specifies the name of the tab</param>
        /// <param name="insertPosition">Specifies the position of the new tab inside of Items collection.</param>
        /// <returns>New instance of the RibbonTabItem that was created.</returns>
        public RibbonTabItem CreateRibbonTab(string text, string name, int insertPosition)
        {
            RibbonTabItem item = new RibbonTabItem();
            item.Text = text;
            item.Name = name;

            RibbonPanel panel = new RibbonPanel();
            panel.Dock = DockStyle.Fill;
            panel.Width = this.Width - 4;
            SetRibbonPanelStyle(panel);
            this.Controls.Add(panel);
            panel.SendToBack();

            item.Panel = panel;
            if (insertPosition < 0)
            {
                insertPosition = this.Items.Count;
                for (int i = 0; i < this.Items.Count;i++ )
                {
                    if (this.Items[i].ItemAlignment == eItemAlignment.Far)
                    {
                        insertPosition = i;
                        break;
                    }
                }
                if (insertPosition >= this.Items.Count)
                    this.Items.Add(item);
                else
                    this.Items.Insert(insertPosition, item);
            }
            else if (insertPosition > this.Items.Count - 1)
                this.Items.Add(item);
            else
                this.Items.Insert(insertPosition, item);

            return item;
        }

        /// <summary>
        /// Creates new Rendering Tab and associated panel and adds them to the control.
        /// </summary>
        /// <param name="text">Specifies the text displayed on the tab.</param>
        /// <param name="name">Specifies the name of the tab</param>
        /// <returns>New instance of the RibbonTabItem that was created.</returns>
        public RibbonTabItem CreateRibbonTab(string text, string name)
        {
            return CreateRibbonTab(text, name, -1);
        }

        /// <summary>
        /// Recalculates layout of the control and applies any changes made to the size or position of the items contained.
        /// </summary>
		public void RecalcLayout()
		{
			m_RibbonStrip.RecalcLayout();
            if (m_QatToolbar != null && m_QatToolbar.Visible)
                m_QatToolbar.RecalcLayout();
		}

		protected override void OnHandleCreated(System.EventArgs e)
		{
			base.OnHandleCreated (e);
            foreach (Control c in this.Controls)
            {
                IntPtr h = c.Handle;
                if (c is RibbonPanel)
                {
                    foreach (Control r in c.Controls)
                        h = r.Handle;
                }
            }
            UpdateRegion();
			this.RecalcLayout();
            if (m_SetExpandedDelayed)
                this.Expanded = m_ExpandedDelayed;
		}

		protected override void OnControlAdded(ControlEventArgs e)
		{
			base.OnControlAdded(e);

			if(this.DesignMode)
				return;
			RibbonPanel panel=e.Control as RibbonPanel;
			if(panel==null)
				return;
			if(panel.RibbonTabItem!=null)
			{
				if(this.Items.Contains(panel.RibbonTabItem) && this.SelectedRibbonTabItem==panel.RibbonTabItem)
				{
					panel.Visible=true;
					panel.BringToFront();
				}
				else
					panel.Visible=false;
			}
			else
				panel.Visible=false;
		}

        private void RibbonStripSizeChanged(object sender, System.EventArgs e)
        {
            if (!m_RibbonControlResized)
            {
                if(m_AutoSize)
                    this.AutoSyncSize();
            }
            m_RibbonControlResized = false;

            if (!m_Expanded && !m_AutoSize)
            {
                this.Height = GetCollapsedHeight();
            }
        }

        private void RibbonStripItemAdded(object sender, System.EventArgs e)
        {
            if (this.DesignMode)
                return;

            if (sender is RibbonTabItem)
            {
                RibbonTabItem tab = sender as RibbonTabItem;
                if (tab.Panel != null)
                {
                    if (this.Controls.Contains(tab.Panel) && tab.Checked)
                    {
                        tab.Panel.Visible = true;
                        tab.Panel.BringToFront();
                    }
                    else
                        tab.Panel.Visible = false;
                    tab.CheckedChanged += new System.EventHandler(RibbonTabCheckedChanged);
                }
            }
        }
        
        private void RibbonStripItemRemoved(object sender, ItemRemovedEventArgs e)
        {
            if(sender is RibbonTabItem)
                ((RibbonTabItem)sender).CheckedChanged -= new System.EventHandler(RibbonTabCheckedChanged);
        }

        private void RibbonTabCheckedChanged(object sender, System.EventArgs e)
        {
            if (sender is RibbonTabItem && ((RibbonTabItem)sender).Checked)
                AutoSyncSize();
        }

		protected override void OnControlRemoved(ControlEventArgs e)
		{
			base.OnControlRemoved(e);
            if (m_PopupMode) return;
			RibbonPanel panel=e.Control as RibbonPanel;
			if(panel==null)
				return;
			if(panel.RibbonTabItem!=null)
			{
				if(this.Items.Contains(panel.RibbonTabItem))
					this.Items.Remove(panel.RibbonTabItem);
			}
		}
        [EditorBrowsable(EditorBrowsableState.Never)]
		public void SetDesignMode()
		{
			m_RibbonStrip.SetDesignMode(true);
		}

        private ElementStyle GetBackgroundStyle()
        {
            return m_RibbonStrip.InternalGetBackgroundStyle();
        }
        
        protected override void OnPaint(PaintEventArgs e)
        {
            ElementStyle style = GetBackgroundStyle();
            if (style.BackColor.A < 255 && !style.BackColor.IsEmpty ||
                this.BackColor == Color.Transparent)
            {
                base.OnPaintBackground(e);
            }
            else
            {
                using (SolidBrush brush = new SolidBrush(this.BackColor))
                    e.Graphics.FillRectangle(brush, this.ClientRectangle);
            }

            if (this.IsGlassEnabled)
            {
                Office2007RibbonForm f = this.FindForm() as Office2007RibbonForm;
                if (f != null)
                    e.Graphics.SetClip(new Rectangle(0, 0, this.Width, f.GlassHeight), CombineMode.Exclude);
            }

            ElementStyleDisplayInfo info = new ElementStyleDisplayInfo(style, e.Graphics, this.ClientRectangle);
            ElementStyleDisplay.PaintBackground(info);
            
            if (!m_QatPositionedBelow && m_RibbonStrip.HasVisibleTabs)
            {
                ShadowPaintInfo shadowInfo = GetShadowPaintInfo();
                shadowInfo.Rectangle = new Rectangle(-2, 0, this.Bounds.Width - shadowInfo.Size + 1, this.Bounds.Height - shadowInfo.Size);
                shadowInfo.Graphics = e.Graphics;
                ShadowPainter.Paint(shadowInfo);
            }
        }

        private ShadowPaintInfo GetShadowPaintInfo()
        {
            if (m_ShadowPaintInfo == null)
                m_ShadowPaintInfo = new ShadowPaintInfo();
            m_ShadowPaintInfo.Size = 3;
            eDotNetBarStyle effectiveStyle = this.EffectiveStyle;
            m_ShadowPaintInfo.IsSquare = (effectiveStyle == eDotNetBarStyle.Office2010 || effectiveStyle == eDotNetBarStyle.Windows7);
            return m_ShadowPaintInfo;
        }

        private bool m_RibbonControlResized = false;
        protected override void OnResize(System.EventArgs e)
        {
            m_RibbonControlResized = true;
            UpdateRegion();
            base.OnResize(e);
            this.AutoSyncSize();
        }

        protected override void NotifyInvalidate(Rectangle invalidatedArea)
        {
            base.NotifyInvalidate(invalidatedArea);
            this.UpdateRegion();
        }

        private void UpdateRegion()
        {
            if (this.Parent is Office2007RibbonForm && m_RibbonStrip.CaptionVisible && !this.IsGlassEnabled)
            {
                // Calculate and set the region for the control so the form is displayed properly
                this.Region = GetControlRegion();
            }
        }

        private bool IsGlassEnabled
        {
            get { if (this.DesignMode || !CanSupportGlass) return false; return WinApi.IsGlassEnabled; }
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == (int)WinApi.WindowsMessages.WM_NCHITTEST)
            {
                // Get position being tested...
                int x = WinApi.LOWORD(m.LParam);
                int y = WinApi.HIWORD(m.LParam);
                Point p = PointToClient(new Point(x, y));
                if (IsGlassEnabled && this.CaptionVisible)
                {
                    Rectangle r = new Rectangle(this.Width - SystemInformation.CaptionButtonSize.Width * 3, 0, SystemInformation.CaptionButtonSize.Width * 3, SystemInformation.CaptionButtonSize.Height);
                    if (r.Contains(p))
                    {
                        m.Result = new IntPtr((int)WinApi.WindowHitTestRegions.TransparentOrCovered);
                        return;
                    }

                    if (m_RibbonStrip != null && !m_RibbonStrip.IsMaximized)
                    {
                        r = new Rectangle(0, 0, this.Width, 4);
                        if (r.Contains(p))
                        {
                            m.Result = new IntPtr((int)WinApi.WindowHitTestRegions.TransparentOrCovered);
                            return;
                        }
                    }

                    if (BarFunctions.IsWindows7 && m_RibbonStrip != null && m_RibbonStrip.IsMaximized)
                    {
                        r = m_RibbonStrip.CaptionBounds;
                        if (r.Contains(p))
                        {
                            m.Result = new IntPtr((int)WinApi.WindowHitTestRegions.TransparentOrCovered);
                            return;
                        }
                    }
                }
                else if (this.CaptionVisible && m_RibbonStrip!=null && !m_RibbonStrip.IsMaximized)
                {
                    Rectangle r = new Rectangle(0, 0, this.Width, 4);
                    if (r.Contains(p))
                    {
                        m.Result = new IntPtr((int)WinApi.WindowHitTestRegions.TransparentOrCovered);
                        return;
                    }
                }
                if (this.EffectiveStyle == eDotNetBarStyle.Office2010 && (p.X < 28 && this.RightToLeft == RightToLeft.No || p.X > this.Width - 28 && this.RightToLeft == RightToLeft.Yes) && p.Y < 28)
                {
                    m.Result = new IntPtr((int)WinApi.WindowHitTestRegions.TransparentOrCovered);
                    return;
                }
                else if (this.EffectiveStyle == eDotNetBarStyle.Windows7 && (p.X < 28 && this.RightToLeft == RightToLeft.No || p.X > this.Width - 28 && this.RightToLeft == RightToLeft.Yes) && p.Y < 28)
                {
                    m.Result = new IntPtr((int)WinApi.WindowHitTestRegions.TransparentOrCovered);
                    return;
                }
            }
            base.WndProc(ref m);
        }

        /// <summary>
        /// Returns Control region if any when control is hosted by Office2007RibbonForm
        /// </summary>
        /// <returns></returns>
        protected virtual Region GetControlRegion()
        {
            if (this.Parent is Office2007RibbonForm && ((Form)this.Parent).WindowState == FormWindowState.Maximized)
                return null;

            GraphicsPath path = new GraphicsPath();
            Rectangle r = new Rectangle(0, 0, this.Width, this.Height); //this.DisplayRectangle;
            if (this.EffectiveStyle == eDotNetBarStyle.Office2010) r.Y++;
            bool rightToLeft = (this.RightToLeft == RightToLeft.Yes);
            
            int topLeftCornerSize = 2, topRightCornerSize = 2;
            if (this.Parent is Office2007RibbonForm)
            {
                Office2007RibbonForm f = this.Parent as Office2007RibbonForm;
                topLeftCornerSize = f.TopLeftCornerSize - 2;
                topRightCornerSize = f.TopRightCornerSize - 1;
                if (topLeftCornerSize <= 0) topLeftCornerSize = 0;
                if (topRightCornerSize <= 0) topLeftCornerSize = 0;
            }
            else
                return null;

            if (rightToLeft)
            {
                int temp = topLeftCornerSize;
                topLeftCornerSize = topRightCornerSize;
                topRightCornerSize = temp;
            }

            ArcData arc;
            if (topLeftCornerSize > 0)
            {
                arc = ElementStyleDisplay.GetCornerArc(r, topLeftCornerSize, eCornerArc.TopLeft);
                path.AddArc(arc.X, arc.Y, arc.Width, arc.Height, arc.StartAngle, arc.SweepAngle);
            }
            else
            {
                // Top Left
                path.AddLine(r.X, r.Y+2, r.X, r.Y);
                path.AddLine(r.X, r.Y, r.X + 2, r.Y);
            }

            if (topRightCornerSize > 0)
            {
                arc = ElementStyleDisplay.GetCornerArc(r, topRightCornerSize, eCornerArc.TopRight);
                path.AddArc(arc.X, arc.Y, arc.Width, arc.Height, arc.StartAngle, arc.SweepAngle);
            }
            else
            {
                // Top Right
                path.AddLine(r.Right - 2, r.Y, r.Right, r.Y);
                path.AddLine(r.Right, r.Y, r.Right, r.Y + 2);
            }

            // Bottom Right
            path.AddLine(r.Right, r.Bottom - 2, r.Right, r.Bottom);
            path.AddLine(r.Right, r.Bottom, r.Right - 2, r.Bottom);

            // Bottom Left
            path.AddLine(r.X + 2, r.Bottom, r.X, r.Bottom);
            path.AddLine(r.X, r.Bottom, r.X, r.Bottom - 2);

            path.CloseAllFigures();

            Region reg = new Region();
            reg.MakeEmpty();
            reg.Union(path);

            // Widen path for the border...
            path.Widen(SystemPens.Control);
            Region r2 = new Region(path);
            reg.Union(path);
            return reg;
        }
		#endregion

		#region Properties
        /// <summary>
        /// Gets or sets the rich text displayed on Ribbon Title instead of the Form.Text property. This property supports text-markup.
        /// You can use <font color="SysCaptionTextExtra"> markup to instruct the markup renderer to use Office 2007 system caption extra text color which
        /// changes depending on the currently selected color table. Note that when using this property you should manage also the Form.Text property since
        /// that is the text that will be displayed in Windows task-bar and elsewhere where system Form.Text property is used.
        /// You can also use the hyperlinks as part of the text markup and handle the TitleTextMarkupLinkClick event to be notified when they are clicked.
        /// </summary>
        [Browsable(true), DefaultValue(""), Editor("DevComponents.DotNetBar.Design.TextMarkupUIEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(System.Drawing.Design.UITypeEditor)), EditorBrowsable(EditorBrowsableState.Always), Category("Appearance"), Description("Indicates text displayed on Ribbon Title instead of the Form.Text property.")]
        public string TitleText
        {
            get { return m_RibbonStrip.TitleText; }
            set { m_RibbonStrip.TitleText = value; }
        }

        /// <summary>
        /// Occurs when text markup link is clicked.
        /// </summary>
        private void RibbonStripTitleTextMarkupLinkClick(object sender, MarkupLinkClickEventArgs e)
        {
            if (TitleTextMarkupLinkClick != null)
                TitleTextMarkupLinkClick(this, new MarkupLinkClickEventArgs(e.Name, e.HRef));
        }

        /// <summary>
        /// Gets or sets the Context menu bar associated with the this control which is used as part of Global Items feature. The context menu 
        /// bar assigned here will be used to search for the items with the same Name or GlobalName property so global properties can be propagated when changed.
        /// You should assign this property to enable the Global Items feature to reach your ContextMenuBar.
        /// </summary>
        [DefaultValue(null), Description("Indicates Context menu bar associated with the ribbon control which is used as part of Global Items feature."), Category("Data")]
        public ContextMenuBar GlobalContextMenuBar
        {
            get { return m_GlobalContextMenuBar; }
            set
            {
                if (m_GlobalContextMenuBar != null)
                    m_GlobalContextMenuBar.GlobalParentComponent = null;
                m_GlobalContextMenuBar = value;
                if (m_GlobalContextMenuBar != null)
                    m_GlobalContextMenuBar.GlobalParentComponent = this;
            }
        }

        /// <summary>
        /// Gets or sets whether custom caption and quick access toolbar provided by the control is visible. Default value is false.
        /// This property should be set to true when control is used on Office2007RibbonForm.
        /// </summary>
        [Browsable(true), DefaultValue(false), Category("Caption"), Description("Indicates whether custom caption and quick access toolbar provided by the control is visible.")]
        public bool CaptionVisible
        {
            get { return m_RibbonStrip.CaptionVisible; }
            set
            {
                m_RibbonStrip.CaptionVisible = value;
                if (!value)
                {
                    Office2007RibbonForm f = this.FindForm() as Office2007RibbonForm;
                    if (f != null)
                        f.UpdateGlass();
                }
            }
        }

        /// <summary>
        /// Gets or sets the font for the form caption text when CaptionVisible=true. Default value is NULL which means that system font is used.
        /// </summary>
        [Browsable(true), DefaultValue(null), Category("Caption"), Description("Indicates font for the form caption text when CaptionVisible=true.")]
        public Font CaptionFont
        {
            get { return m_RibbonStrip.CaptionFont; }
            set
            {
                m_RibbonStrip.CaptionFont = value;
            }
        }

        /// <summary>
        /// Gets or sets the explicit height of the caption provided by control. Caption height when set is composed of the TabGroupHeight and
        /// the value specified here. Default value is 0 which means that system default caption size is used.
        /// </summary>
        [Browsable(true), DefaultValue(0), Category("Caption"), Description("Indicates explicit height of the caption provided by control")]
        public int CaptionHeight
        {
            get { return m_RibbonStrip.CaptionHeight; }
            set
            {
                m_RibbonStrip.CaptionHeight = value;
                this.RecalcLayout();
            }
        }

        /// <summary>
        /// Gets or sets the indent of the ribbon strip. The indent setting is useful when control is used with caption visible and the Office 2007
        /// style start button. The indent specified here will move the ribbon strip so the start button does not overlap the tabs.
        /// Value of this property is used only when CaptionVisible = true.
        /// Default value is 46.
        /// </summary>
        [Browsable(true), DefaultValue(46), Category("Layout"), Description("Indicates indent of the ribbon strip.")]
        public int RibbonStripIndent
        {
            get { return m_RibbonStrip.RibbonStripIndent; }
            set { m_RibbonStrip.RibbonStripIndent = value; }
        }

		/// <summary>
		/// Gets or sets whether mouse over fade effect is enabled. Default value is true.
		/// </summary>
		[Browsable(true), DefaultValue(true), Category("Appearance"), Description("Indicates whether mouse over fade effect is enabled")]
		public bool FadeEffect
		{
			get { return m_RibbonStrip.FadeEffect; }
			set
			{
				m_RibbonStrip.FadeEffect = value;
			}
		}

        /// <summary>
        /// Gets or sets the font that is used to display Key Tips (accelerator keys) when they are displayed. Default value is null which means
        /// that control Font is used for Key Tips display.
        /// </summary>
        [Browsable(true), DefaultValue(null), Category("Appearance"), Description("Indicates font that is used to display Key Tips (accelerator keys) when they are displayed.")]
        public virtual Font KeyTipsFont
        {
            get { return m_RibbonStrip.KeyTipsFont; }
            set { m_RibbonStrip.KeyTipsFont = value; }
        }

		/// <summary>
		/// Collection of RibbonTabItemGroup items. Groups are assigned optionally to one or more RibbonTabItem object through the RibbonTabItem.Group
		/// property to visually group tabs that belong to same functions. These tabs should be positioned next to each other.
		/// </summary>
        [Editor("DevComponents.DotNetBar.Design.RibbonTabItemGroupCollectionEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(System.Drawing.Design.UITypeEditor)), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Tab Groups"), Browsable(true), DevCoBrowsable(true)]
		public RibbonTabItemGroupCollection TabGroups
		{
			get {return m_RibbonStrip.TabGroups;}
		}

		/// <summary>
		/// Gets or sets the height in pixels of tab group line that is displayed above the RibbonTabItem objects that have group assigned.
		/// Default value is 10 pixels. To show tab groups you need to assign the RibbonTabItem.Group property and set TabGroupsVisible=true.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),DefaultValue(10),Category("Tab Groups"),Description("Indicates height in pixels of tab group line that is displayed above the RibbonTabItem objects that have group assigned.")]
		public int TabGroupHeight
		{
			get {return m_RibbonStrip.TabGroupHeight;}
			set
			{
				m_RibbonStrip.TabGroupHeight=value;
				if(this.DesignMode)
					this.RecalcLayout();
			}
		}

		/// <summary>
		/// Gets or sets whether tab group line that is displayed above the RibbonTabItem objects that have group assigned is visible.
		/// Default value is false. To show tab groups you need to assign the RibbonTabItem.Group property and set TabGroupsVisible=true. Use TabGroupHeight
		/// property to control height of the group line.
		/// </summary>
        [Browsable(true), DevCoBrowsable(true), DefaultValue(false), Category("Tab Groups"), Description("Indicates whether tab group line that is displayed above the RibbonTabItem objects that have group assigned is visible.")]
		public bool TabGroupsVisible
		{
			get {return m_RibbonStrip.TabGroupsVisible;}
			set
			{
				m_RibbonStrip.TabGroupsVisible=value;
				if(this.DesignMode)
					this.RecalcLayout();
			}
		}

		/// <summary>
		/// Gets or sets default font for tab groups. This font will be used if font is not specified by group style element.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),Category("Tab Groups"),DefaultValue(null)]
		public Font DefaultGroupFont
		{
			get {return m_RibbonStrip.DefaultGroupFont;}
			set
			{
				m_RibbonStrip.DefaultGroupFont=value;
				if(this.DesignMode)
					this.Refresh();
			}
		}
		/// <summary>
		/// Resets DefaultGroupFont property to default value null.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void ResetDefaultGroupFont()
		{
			TypeDescriptor.GetProperties(this)["DefaultGroupFont"].SetValue(this,null);
		}

		/// <summary>
		/// Specifies the background style of the control.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),Category("Style"),Description("Gets or sets bar background style."),DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ElementStyle BackgroundStyle
		{
			get {return m_RibbonStrip.BackgroundStyle;}
		}

		/// <summary>
		/// Gets or sets the currently selected RibbonTabItem. RibbonTabItems are selected using the Checked property. Only a single
		/// RibbonTabItem can be selected (Checked) at any given time.
		/// </summary>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public RibbonTabItem SelectedRibbonTabItem
		{
			get {return m_RibbonStrip.SelectedRibbonTabItem;}
            set
            {
                if (value != null) value.Checked = true;
            }
		}

		/// <summary>
		/// Returns reference to internal ribbon strip control.
		/// </summary>
		[Browsable(false),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public RibbonStrip RibbonStrip
		{
			get {return m_RibbonStrip;}
		}

		/// <summary>
		/// Returns collection of items on a bar.
		/// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Browsable(false)]
		public SubItemsCollection Items
		{
			get {return m_RibbonStrip.Items;}
		}

        /// <summary>
        /// Returns collection of quick toolbar access and caption items.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Browsable(false)]
        public SubItemsCollection QuickToolbarItems
        {
            get { return GetQatSubItemsCollection(); }
        }

        private SubItemsCollection GetQatSubItemsCollection()
        {
            if (m_QatPositionedBelow)
            {
                if (m_QatSubItemsCollection == null)
                {
                    m_QatSubItemsCollection = new Ribbon.SubItemsQatCollection(m_QatToolbar);
                    BaseItem startButton = GetApplicationButton();
                    if (startButton != null)
                        m_QatSubItemsCollection._Add(startButton);
                    foreach (BaseItem item in m_QatToolbar.Items)
                        m_QatSubItemsCollection._Add(item);
                }

                return m_QatSubItemsCollection;
            }
            else
                return m_RibbonStrip.QuickToolbarItems; 
        }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public eDotNetBarStyle EffectiveStyle
        {
            get
            {
                return m_RibbonStrip.EffectiveStyle;
            }
        }

		/// <summary>
        /// Gets/Sets the visual style of the control. If you are changing style to Office 2007 or Office 2010 use RibbonPredefinedColorSchemes.ChangeStyle method instead to ensure
        /// all controls are switched properly.
		/// </summary>
		[Browsable(false),Category("Appearance"),Description("Specifies the visual style of the control."),DefaultValue(eDotNetBarStyle.Office2003)]
        public eDotNetBarStyle Style
        {
            get { return m_RibbonStrip.Style; }
            set
            {
                if (value == eDotNetBarStyle.StyleManagerControlled)
                {
                    SetStyle();
                }
                else
                    ChangeControlStyle(value);
            }
        }


        private static BaseItem GetAppButton(RibbonControl ribbon)
        {
            BaseItem appButton = null;
            for (int i = 0; i < ribbon.QuickToolbarItems.Count; i++)
            {
                if (ribbon.QuickToolbarItems[i] is Office2007StartButton)
                {
                    appButton = ribbon.QuickToolbarItems[i];
                    break;
                }

            }
            return appButton;
        }
        protected override void OnBackColorChanged(EventArgs e)
        {
            base.OnBackColorChanged(e);
        }
        private void SetStyle()
        {
            eDotNetBarStyle effectiveStyle = StyleManager.GetEffectiveStyle();
            RibbonControl ribbon = this;
            if (effectiveStyle == eDotNetBarStyle.Office2010 || effectiveStyle == eDotNetBarStyle.Windows7)
            {
                BaseItem appButton = GetAppButton(ribbon);
                if (appButton != null)
                {
                    if (appButton is ButtonItem && ((ButtonItem)appButton).ImageFixedSize.IsEmpty)
                    {
                        ((ButtonItem)appButton).ImageFixedSize = new Size(16, 16);
                    }
                    if (appButton is ButtonItem)
                    {
                        ButtonItem tempButton = (ButtonItem)appButton;
                        tempButton.ImagePaddingHorizontal = 0;
                        tempButton.ImagePaddingVertical = 0;
                    }
                    if(appButton.Parent!=null)
                        appButton.Parent.SubItems.Remove(appButton);
                    ribbon.Items.Insert(0, appButton);
                }
            }
            else if (effectiveStyle == eDotNetBarStyle.Office2007)
            {
                if (ribbon.Items.Count > 0 && ribbon.Items[0] is Office2007StartButton)
                {
                    Office2007StartButton appButton = (Office2007StartButton)ribbon.Items[0];
                    if (!appButton.ImageFixedSize.IsEmpty)
                        appButton.ImageFixedSize = Size.Empty;
                    appButton.ImagePaddingHorizontal = 2;
                    appButton.ImagePaddingVertical = 2;
                    ribbon.Items.Remove(appButton);
                    if (m_QatPositionedBelow)
                        m_RibbonStrip.CaptionContainerItem.SubItems.Insert(0, appButton);
                    else
                        ribbon.QuickToolbarItems.Insert(0, appButton);
                }
            }
            
            foreach (Control c in this.Controls)
            {
                if (c is RibbonPanel)
                {
                    ((RibbonPanel)c).ColorSchemeStyle = eDotNetBarStyle.StyleManagerControlled;
                    RibbonPanel ribbonPanel = (RibbonPanel)c;
                    foreach (Control r in c.Controls)
                    {
                        if (r is RibbonBar)
                        {
                            RibbonBar rb = r as RibbonBar;
                            if (rb.GalleryStretch == null && !ribbonPanel.DefaultLayout && !this.DesignMode) rb.Width += 4;
                            TypeDescriptor.GetProperties(rb)["Style"].SetValue(rb, eDotNetBarStyle.StyleManagerControlled);
                        }
                    }
                }
            }
            m_RibbonStrip.Style = eDotNetBarStyle.StyleManagerControlled;

            ribbon.RibbonStrip.InitDefaultStyles();
            ribbon.Invalidate(true);
            if (ribbon.IsHandleCreated && ribbon.SelectedRibbonTabItem != null && ribbon.SelectedRibbonTabItem.Panel != null)
            {
                ribbon.SelectedRibbonTabItem.Panel.PerformLayout();
            }

            if (m_QatToolbar != null) m_QatToolbar.Style = eDotNetBarStyle.StyleManagerControlled;
            if (this.IsHandleCreated)
            {
                this.Invalidate(true);
                this.RecalcLayout();
            }
        }

        private void ChangeControlStyle(eDotNetBarStyle newStyle)
        {
            if (newStyle != eDotNetBarStyle.StyleManagerControlled)
            {
                if (newStyle == eDotNetBarStyle.Office2010)
                {
                    // Ensure that proper color table is selected.
                    if (GlobalManager.Renderer is Office2007Renderer)
                    {
                        if (!(((Office2007Renderer)GlobalManager.Renderer).ColorTable is Office2010ColorTable))
                        {
                            ((Office2007Renderer)GlobalManager.Renderer).ColorTable = new Office2010ColorTable();
                        }

                    }
                }
                else if (newStyle == eDotNetBarStyle.Windows7)
                {
                    // Ensure that proper color table is selected.
                    if (GlobalManager.Renderer is Office2007Renderer)
                    {
                        if (!(((Office2007Renderer)GlobalManager.Renderer).ColorTable is Windows7ColorTable))
                        {
                            ((Office2007Renderer)GlobalManager.Renderer).ColorTable = new Windows7ColorTable();
                        }

                    }
                }
                else if (newStyle == eDotNetBarStyle.Office2007)
                {
                    // Ensure that proper color table is selected.
                    if (GlobalManager.Renderer is Office2007Renderer)
                    {
                        if ((((Office2007Renderer)GlobalManager.Renderer).ColorTable is Office2010ColorTable) ||
                            (((Office2007Renderer)GlobalManager.Renderer).ColorTable is Windows7ColorTable))
                            ((Office2007Renderer)GlobalManager.Renderer).ColorTable = new Office2007ColorTable(this.Office2007ColorTable);

                    }
                }
            }
            
            m_RibbonStrip.Style = newStyle;
            foreach (Control c in this.Controls)
            {
                if (c is RibbonPanel)
                {
                    ((RibbonPanel)c).ColorSchemeStyle = newStyle;

                    foreach (Control r in c.Controls)
                    {
                        if (r is RibbonBar)
                        {
                            RibbonBar rb = r as RibbonBar;
                            TypeDescriptor.GetProperties(rb)["Style"].SetValue(rb, newStyle);
                        }
                    }
                }
            }

            if (m_QatToolbar != null) m_QatToolbar.Style = newStyle;
            if (this.IsHandleCreated)
            {
                this.Invalidate(true);
                this.RecalcLayout();
            }

            if (!this.DesignMode)
            {
                Office2007RibbonForm form = this.FindForm() as Office2007RibbonForm;
                if (form != null) form.UpdateGlass();
            }

        }

        private Rendering.eOffice2007ColorScheme m_DelayedColorTableChange = DevComponents.DotNetBar.Rendering.eOffice2007ColorScheme.Blue;
        private bool m_SetColorTableDelayed = false;
        /// <summary>
        /// Gets or sets the Office 2007 Renderer global Color Table. Setting this property will affect all controls on the form that are using Office 2007 global renderer.
        /// </summary>
        [Browsable(false), Category("Appearance"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden) ,DefaultValue(Rendering.eOffice2007ColorScheme.Blue), Description("Indicates the Office 2007 Renderer global Color Table."), Obsolete("This property is obsolete and it does not do anything. It has been replaced by StyleManager")]
        public Rendering.eOffice2007ColorScheme Office2007ColorTable
        {
            get
            {
                Rendering.Office2007Renderer r = m_RibbonStrip.GetRenderer() as Rendering.Office2007Renderer;
                if (r != null)
                    return r.ColorTable.InitialColorScheme;
                return Rendering.eOffice2007ColorScheme.Blue;
            }
            set
            {
                //Form f = this.FindForm();
                //if (f == null && this.Parent == null)
                //{
                //    m_SetColorTableDelayed = true;
                //    m_DelayedColorTableChange = value;
                //    return;
                //}
                //else if (f == null && this.Parent != null)
                //{
                //    RibbonPredefinedColorSchemes.ChangeOffice2007ColorTable(this.Parent, value);
                //}
                //else
                //    RibbonPredefinedColorSchemes.ChangeOffice2007ColorTable(f, value);
                
            }
        }

        /// <summary>
        /// Raises the BeforeRibbonPanelPopupClose event.
        /// </summary>
        protected virtual void OnBeforeRibbonPanelPopupClose(RibbonPopupCloseEventArgs e)
        {
            if (BeforeRibbonPanelPopupClose != null)
                BeforeRibbonPanelPopupClose(this, e);
        }

        /// <summary>
        /// Raises the AfterRibbonPanelPopupClose event.
        /// </summary>
        protected virtual void OnAfterRibbonPanelPopupClose(EventArgs e)
        {
            if (AfterRibbonPanelPopupClose != null)
                AfterRibbonPanelPopupClose(this, e);
        }

        /// <summary>
        /// Raises the BeforeRibbonPanelPopup event.
        /// </summary>
        /// <param name="ce"></param>
        protected virtual void OnBeforeRibbonPanelPopup(CancelEventArgs ce)
        {
            if(BeforeRibbonPanelPopup!=null)
                BeforeRibbonPanelPopup(this, ce);
        }

        /// <summary>
        /// Raises the BeforeRibbonPanelPopup event.
        /// </summary>
        /// <param name="ce"></param>
        protected virtual void OnAfterRibbonPanelPopup(EventArgs e)
        {
            if (AfterRibbonPanelPopup != null)
                AfterRibbonPanelPopup(this, e);
        }

        /// <summary>
        /// Gets whether collapsed ribbon is displaying the selected ribbon panel as popup.
        /// </summary>
        [Browsable(false)]
        public bool IsPopupMode
        {
            get { return m_PopupMode; }
        }

        internal void OnChildItemClick(BaseItem item)
        {
            m_RibbonStrip.OnChildItemClick(item);
            if (!m_PopupMode || item == null) return;
            if (item is RibbonTabItem || item.IsContainer || item.Name.StartsWith("sysgallery") || item is CheckBoxItem || !item.AutoCollapseOnClick) return;
            if(item is PopupItem && item.Expanded) return;

            this.CloseRibbonMenu(item, eEventSource.Mouse);
        }

        private Timer m_ActiveWindowTimer = null;
        private IntPtr m_ForegroundWindow = IntPtr.Zero;
        private IntPtr m_ActiveWindow = IntPtr.Zero;
        /// <summary>
        /// Sets up timer that watches when active window changes.
        /// </summary>
        private void SetupActiveWindowTimer()
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
                if (a != IntPtr.Zero)
                {
                    Control c = Control.FromHandle(a);
                    if (c is PopupContainer || c is PopupContainerControl || c is Balloon)
                        return;
                }
                m_ActiveWindowTimer.Stop();
                OnActiveWindowChanged();
            }
        }

        /// <summary>
        /// Called after change of active window has been detected. SetupActiveWindowTimer must be called to enable detection.
        /// </summary>
        private void OnActiveWindowChanged()
        {
            if (m_PopupMode)
                this.CloseRibbonMenu(this, eEventSource.Code);
            ReleaseActiveWindowTimer();
        }

        /// <summary>
        /// Releases and disposes the active window watcher timer.
        /// </summary>
        private void ReleaseActiveWindowTimer()
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
#if TRIAL
        private bool _ShownOnce = false;
#endif
        protected override void OnVisibleChanged(EventArgs e)
        {
            if (!this.Visible)
                OnActiveWindowChanged();
            else
            {
#if TRIAL
                if(!this.DesignMode && !_ShownOnce)
                {
				    RemindForm frm=new RemindForm();
				    frm.ShowDialog();
				    frm.Dispose();
                    _ShownOnce = true;
                }
#endif

            }
            base.OnVisibleChanged(e);
        }

        internal void OnEscapeKeyDown()
        {
            OnActiveWindowChanged();
        }


        internal void OnSysMouseDown(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
        {
            if (!m_PopupMode)
                return;
            Control cTmp = FromChildHandle(hWnd);
            if (cTmp == null)
            {
                string s = NativeFunctions.GetClassName(hWnd);
                s = s.ToLower();
                if (s.IndexOf("combolbox") >= 0)
                    return;
                OnActiveWindowChanged();
                return;
            }

            do
            {
                if (cTmp is MenuPanel || cTmp is PopupContainer || cTmp is PopupContainerControl || cTmp is RibbonBar || cTmp is RibbonStrip || cTmp is RibbonPanel || cTmp is Balloon)
                {
                    return;
                }
                cTmp = cTmp.Parent;
            } while (cTmp != null);

            OnActiveWindowChanged();
            return;
        }

        /// <summary>
        /// Displays the active ribbon panel on the popup if ribbon control is collapsed.
        /// </summary>
        /// <param name="source">Reference to the object that was cause of the event. This is provided to the BeforeRibbonPanelPopupClose event if an menu needs to be closed.</param>
        /// <param name="eventSource">Indicates the event source. This is provided to the BeforeRibbonPanelPopupClose event if an menu needs to be closed.</param>
        public void PopupRibbon(object source, eEventSource eventSource)
        {
            if (this.Expanded || this.SelectedRibbonTabItem == null || this.SelectedRibbonTabItem.Panel == null)
                return;
            if (m_PopupRibbonPanel == this.SelectedRibbonTabItem.Panel)
                return;

            Control f = this.FindForm();
            if (f == null) f = this.Parent;
            if (f == null) return;

            // Close any currently open panels.
            RibbonPanel previousVisiblePanel = m_PopupRibbonPanel;
            if (previousVisiblePanel != null)
            {
                RibbonPopupCloseEventArgs pce = new RibbonPopupCloseEventArgs(source, eventSource);
                OnBeforeRibbonPanelPopupClose(pce);
                if (pce.Cancel) return;
            }

            //Raise events
            CancelEventArgs ce = new CancelEventArgs();
            OnBeforeRibbonPanelPopup(ce);
            if (ce.Cancel) return;

            m_PopupMode = true;
            RibbonPanel panel = this.SelectedRibbonTabItem.Panel;
            panel.SuspendLayout();
            this.Controls.Remove(panel);
            panel.Visible = false;
            panel.Dock = DockStyle.None;
            f.Controls.Add(panel);
            Point p = m_RibbonStrip.PointToScreen(new Point(0, m_RibbonStrip.Height));
            p = f.PointToClient(p);
            panel.Font = this.Font;
            panel.Bounds = new Rectangle(p.X, p.Y - DefaultBottomDockPadding - 1, this.Width, GetPopupRibbonPanelHeight(panel));
            panel.SetPopupMode(true, this);
            panel.ResumeLayout();
            panel.Visible = true;
            panel.BringToFront();
            CloseRibbonMenu(previousVisiblePanel);
            SetupActiveWindowTimer();
            m_PopupRibbonPanel = panel;
            // Raise events...
            OnAfterRibbonPanelPopup(new EventArgs());
        }

        private void CloseRibbonMenu(RibbonPanel panel)
        {
            if (panel == null || panel.IsDisposed || panel.Parent == null) return;

            this.SuspendLayout();
            panel.Visible = false;
            panel.Parent.Controls.Remove(panel);
            panel.SetPopupMode(false, this);
            panel.Dock = DockStyle.Fill;
            this.Controls.Add(panel);
            if (this.SelectedRibbonTabItem != null)
            {
                this.SelectedRibbonTabItem.Refresh();
                if(this.SelectedRibbonTabItem.Panel == panel)
                    panel.Visible = true;
            }
            panel.BringToFront();
            this.ResumeLayout(true);

            OnAfterRibbonPanelPopupClose(new EventArgs()); 
        }

        /// <summary>
        /// Closes the Ribbon tab menu with source set to null and event source set to Code.
        /// </summary>
        public void CloseRibbonMenu()
        {
            CloseRibbonMenu(null, eEventSource.Code);
        }

        /// <summary>
        /// Closes the Ribbon Menu if one is currently displayed.
        /// </summary>
        public void CloseRibbonMenu(object source, eEventSource eventSource)
        {
            if (!m_PopupMode || m_PopupRibbonPanel == null) return;

            RibbonPopupCloseEventArgs pce = new RibbonPopupCloseEventArgs(source, eventSource);
            OnBeforeRibbonPanelPopupClose(pce);
            if (pce.Cancel) return;

            CloseRibbonMenu(m_PopupRibbonPanel);
            
            m_PopupRibbonPanel = null;
            m_PopupMode = false;
        }

        private int GetPopupRibbonPanelHeight(RibbonPanel panel)
        {
            if (this.AutoSize)
                return GetAutoPanelHeight(panel);
            
            int height = m_ExpandedHeight - m_RibbonStrip.Height;
            if (m_ExpandedQatBelowRibbon)
                height -= m_ExpandedQatHeight + 1;
            //else
            //    height -= DefaultBottomDockPadding;

            return height;
        }

        internal void RibbonTabItemClick(RibbonTabItem rt)
        {
            if (!this.Expanded)
            {
                if (m_MenuTabsEnabled)
                {
                    if (rt.Panel == m_PopupRibbonPanel)
                        CloseRibbonMenu(rt, eEventSource.Mouse);
                    else
                        PopupRibbon(rt, eEventSource.Mouse);
                }
                else
                    this.Expanded = true;
            }
        }

        internal void RibbonTabItemDoubleClick(RibbonTabItem ribbonTabItem)
        {
            if (m_MenuTabsEnabled && !this.Expanded)
            {
                this.CloseRibbonMenu(ribbonTabItem, eEventSource.Code);
                this.Expanded = true;
            }
            else
                this.Expanded = false;
        }

        private bool _MouseWheelTabScrollEnabled = true;
        /// <summary>
        /// Gets or sets whether mouse wheel scrolls through the ribbon tabs. Default value is true.
        /// </summary>
        [DefaultValue(true), Category("Behavior"), Description("Indicates whether mouse wheel scrolls through the ribbon tabs.")]
        public bool MouseWheelTabScrollEnabled
        {
            get { return _MouseWheelTabScrollEnabled; }
            set
            {
                _MouseWheelTabScrollEnabled = value;
            }
        }
        

        private bool m_SetExpandedDelayed = false;
        private bool m_ExpandedDelayed = false;
		/// <summary>
		/// Gets or sets whether control is expanded or not. When control is expanded both the tabs and the tab ribbons are visible. When collapsed
		/// only tabs are visible.
		/// </summary>
		[DefaultValue(true),Browsable(true),DevCoBrowsable(true),Category("Layout"),Description("Gets or sets whether control is expanded or not. When control is expanded both the tabs and the tab ribbons are visible.")]
		public bool Expanded
		{
			get {return m_Expanded;}
			set
			{
				if(m_Expanded!=value)
				{
                    if (m_IsLayoutSuspended || !BarFunctions.IsHandleValid(this))
                    {
                        m_SetExpandedDelayed = true;
                        m_ExpandedDelayed = value;
                        return;
                    }
                    m_SetExpandedDelayed = false;
                    m_Expanded = value;
                    OnExpandedChanged();
				}
			}
		}

        private int m_ExpandedHeight = 0;
        private void OnExpandedChanged()
        {
            if(m_Expanded && m_QatToolbar!=null)
                m_QatToolbar.SendToBack();
            else if (!m_Expanded && m_QatToolbar != null)
                m_QatToolbar.BringToFront();
            m_RibbonStrip.Invalidate();

            if (this.AutoSize)
                RecalculateAutoSize();
            else
            {
                int height = 0; // m_RibbonStrip.Height;
                
                if (!this.Expanded)
                {
                    height = GetCollapsedHeight();
                    m_ExpandedQatBelowRibbon = m_QatPositionedBelow;
                    if (m_QatPositionedBelow)
                    {
                        //height += m_QatToolbar.Height;
                        m_ExpandedQatHeight = m_QatToolbar.Height;
                    }
                }
                
                if (this.Expanded)
                {
                    height = m_ExpandedHeight;
                    // If QAT Position changed substract its height
                    if (!m_QatPositionedBelow && m_ExpandedQatBelowRibbon) // Qat was below but while collapsed moved up
                        height -= m_ExpandedQatHeight;
                    else if (m_QatPositionedBelow && !m_ExpandedQatBelowRibbon) // Qat was above but while collapsed moved down
                        height += m_QatToolbar.Height;
                }
                else
                    m_ExpandedHeight = this.Height;

                if (this.Height != height && height > 0)
                    this.Height = height;
            }

            if (ExpandedChanged != null)
                ExpandedChanged(this, new EventArgs());
        }

        private int GetCollapsedHeight()
        {
            int height = m_RibbonStrip.Height;
            if (m_QatPositionedBelow)
                height += m_QatToolbar.Height;
            return height;
        }

        /// <summary>
        /// Gets or sets whether control is collapsed when RibbonTabItem is double clicked and expanded when RibbonTabItem is clicked.
        /// Default value is true.
        /// </summary>
        [DefaultValue(true), Browsable(true), DevCoBrowsable(true), Category("Layout"), Description("Indicates whether control is collapsed when RibbonTabItem is double clicked and expanded when RibbonTabItem is clicked.")]
        public virtual bool AutoExpand
        {
            get { return m_RibbonStrip.AutoExpand; }
            set { m_RibbonStrip.AutoExpand = value; }
        }

        /// <summary>
        /// ImageList for images used on Items. Images specified here will always be used on menu-items and are by default used on all Bars.
        /// </summary>
        [Browsable(true), Category("Data"), DefaultValue(null), Description("ImageList for images used on Items. Images specified here will always be used on menu-items and are by default used on all Bars.")]
        public ImageList Images
        {
            get { return m_RibbonStrip.Images; }
            set { m_RibbonStrip.Images = value; }
        }

        /// <summary>
        /// ImageList for medium-sized images used on Items.
        /// </summary>
        [Browsable(true), Category("Data"), DefaultValue(null), Description("ImageList for medium-sized images used on Items.")]
        public ImageList ImagesMedium
        {
            get { return m_RibbonStrip.ImagesMedium; }
            set { m_RibbonStrip.ImagesMedium = value; }
        }

        /// <summary>
        /// ImageList for large-sized images used on Items.
        /// </summary>
        [Browsable(true), Category("Data"), DefaultValue(null), Description("ImageList for large-sized images used on Items.")]
        public ImageList ImagesLarge
        {
            get { return m_RibbonStrip.ImagesLarge; }
            set { m_RibbonStrip.ImagesLarge = value; }
        }

        protected override void OnTabStopChanged(System.EventArgs e)
        {
            base.OnTabStopChanged(e);
            m_RibbonStrip.TabStop = this.TabStop;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the user can give the focus to this control using the TAB key. Default value is false.
        /// </summary>
        [Browsable(true), DefaultValue(false), Category("Behavior"), Description("Indicates whether the user can give the focus to this control using the TAB key.")]
        public new bool TabStop
        {
            get { return base.TabStop; }
            set { base.TabStop = value; }
        }
		#endregion

        #region Mdi Child System Item handling
        /// <summary>
        /// Specifies whether the MDI system buttons are displayed in ribbon strip when MDI Child window is maximized.
        /// </summary>
        [System.ComponentModel.Browsable(true), DefaultValue(true), System.ComponentModel.Category("Run-time Behavior"), System.ComponentModel.Description("Specifies whether the MDI system buttons are displayed in menu bar when MDI Child window is maximized.")]
        public bool MdiSystemItemVisible
        {
            get
            {
                return m_MdiSystemItemVisible;
            }
            set
            {
                if (m_MdiSystemItemVisible != value)
                {
                    m_MdiSystemItemVisible = value;
                    if (!this.DesignMode)
                    {
                        SyncMdiChildSystemItem();
                        if (!m_MdiSystemItemVisible)
                        {
                            if (m_ActiveMdiChild != null)
                            {
                                // Release the form
                                m_ActiveMdiChild.Resize -= new System.EventHandler(MdiChildFormResize);
                                m_ActiveMdiChild.VisibleChanged -= new System.EventHandler(MdiChildVisibleChanged);
                            }

                            //// Remove event handler
                            //Form form = this.FindForm();
                            //if (form != null)
                            //    form.MdiChildActivate -= new System.EventHandler(ParentFormMdiChildActivate);
                        }
                        //else
                        //{
                        //    // Hook into the parent form if any...
                        //    Form form = this.FindForm();
                        //    if (form != null)
                        //        form.MdiChildActivate += new System.EventHandler(ParentFormMdiChildActivate);
                        //}
                    }
                }
            }
        }

        private bool m_IsLayoutSuspended = false;
        /// <summary>
        /// Suspends the form layout.
        /// </summary>
        public new void SuspendLayout()
        {
            base.SuspendLayout();
            m_IsLayoutSuspended = true;
        }

        /// <summary>
        /// Suspends the form layout.
        /// </summary>
        public new void ResumeLayout()
        {
            m_IsLayoutSuspended = false;
            if (m_SetColorTableDelayed)
            {
                m_SetColorTableDelayed = false;
                this.Office2007ColorTable = m_DelayedColorTableChange;
            }
            base.ResumeLayout();
        }

        /// <summary>
        /// Suspends the form layout.
        /// </summary>
        public new void ResumeLayout(bool performLayout)
        {
            m_IsLayoutSuspended = false;
            if (m_SetColorTableDelayed)
            {
                m_SetColorTableDelayed = false;
                this.Office2007ColorTable = m_DelayedColorTableChange;
            }
            base.ResumeLayout(performLayout);
        }

        /// <summary>
        /// Gets or sets whether Ribbon control employs the Windows Vista Glass support when available. This is managed automatically by Ribbon Control and
        /// no setting is necessary on your part.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual bool CanSupportGlass
        {
            get { return m_RibbonStrip.CanSupportGlass; }
            set { m_RibbonStrip.CanSupportGlass = value; }
        }

        private Form _ParentForm = null;
        private void ReleaseParentForm()
        {
            Form parentForm = _ParentForm;
            _ParentForm = null;
            if (parentForm != null)
            {
                parentForm.MdiChildActivate -= ParentFormMdiChildActivate;
            }
        }
        protected override void OnParentChanged(System.EventArgs e)
        {
            ReleaseParentForm();

            base.OnParentChanged(e);

            Form form= this.FindForm();
            if (form != null)
            {
                form.MdiChildActivate += ParentFormMdiChildActivate;
                _ParentForm = form;
            }

            if (m_SetColorTableDelayed && !m_IsLayoutSuspended)
            {
                m_SetColorTableDelayed = false;
                this.Office2007ColorTable = m_DelayedColorTableChange;
            }
        }

        private void  ParentFormMdiChildActivate(object sender, System.EventArgs e)
        {
 	        Form form= this.FindForm();
            if (form == null)
                return;
            
            Form mdiChild = form.ActiveMdiChild;
            if (m_MdiSystemItemVisible)
            {
                if (mdiChild != null)
                {
                    mdiChild.Resize += new System.EventHandler(MdiChildFormResize);
                    mdiChild.VisibleChanged += new System.EventHandler(MdiChildVisibleChanged);
                }

                if(m_ActiveMdiChild!=null)
                {
                    m_ActiveMdiChild.Resize -= new System.EventHandler(MdiChildFormResize);
                    m_ActiveMdiChild.VisibleChanged -= new System.EventHandler(MdiChildVisibleChanged);
                }
                m_MdiChildMaximized = false;
                m_ActiveMdiChild = mdiChild;
                SyncMdiChildSystemItem();
            }

            UpdateMerging();
            
        }

        private void UpdateMerging()
        {
            if (!m_AllowMerge) return;

            Form form = this.FindForm();
            if (form == null)
                return;

            Form mdiChild = form.ActiveMdiChild;

            if (mdiChild == m_PreviousMergedForm)
                return;
            bool selectedRemoved = false;
            this.SuspendLayout();
            m_RibbonStrip.BeginUpdate();
            if (m_PreviousMergedForm != null)
            {
                ArrayList list = GetMergeContainers(m_PreviousMergedForm);
                foreach (RibbonBarMergeContainer cont in list)
                {
                    if (cont.RibbonTabItem == this.SelectedRibbonTabItem)
                        selectedRemoved = true;
                    cont.RemoveMergedRibbonBars(this);
                }
                m_PreviousMergedForm = null;
            }

            if (mdiChild != null)
            {
                ArrayList list = GetMergeContainers(mdiChild);
                if (list.Count > 0)
                {
                    bool first = true;
                    foreach (RibbonBarMergeContainer cont in list)
                    {
                        cont.MergeRibbonBars(this);
                        if (first && cont.RibbonTabItem != null && cont.AutoActivateTab)
                        {
                            cont.RibbonTabItem.Checked = true;
                            selectedRemoved = false;
                        }
                    }
                    m_PreviousMergedForm = mdiChild;
                }
            }

            if (selectedRemoved)
                SelectFirstVisibleRibbonTab();

            m_RibbonStrip.EndUpdate(false);
            this.ResumeLayout(true);
            this.RecalcLayout();
        }

        /// <summary>
        /// Selects first visible RibbonTab.
        /// </summary>
        /// <returns>Returns true if selection is performed otherwise false.</returns>
        public bool SelectFirstVisibleRibbonTab()
        {
            // Select first visible
            foreach (BaseItem item in this.Items)
            {
                if (item is RibbonTabItem && item.Visible)
                {
                    ((RibbonTabItem)item).Checked = true;
                    return true;
                }
            }

            return false;
        }

        private ArrayList GetMergeContainers(Form form)
        {
            ArrayList list = new ArrayList();
            foreach (Control c in form.Controls)
            {
                if (c is RibbonBarMergeContainer && ((RibbonBarMergeContainer)c).AllowMerge)
                {
                    list.Add(c);
                }
            }

            return list;
        }

        private void MdiChildVisibleChanged(object sender, System.EventArgs e)
        {
            SyncMdiChildSystemItem();
        }

        private void MdiChildFormResize(object sender, System.EventArgs e)
        {
            SyncMdiChildSystemItem();
        }

        private void SyncMdiChildSystemItem()
        {
            if (m_MdiSystemItemVisible && m_ActiveMdiChild != null &&
                IsMaximized(m_ActiveMdiChild) && m_ActiveMdiChild.Visible)
            {
                if (!m_MdiChildMaximized)
                {
                    m_RibbonStrip.ShowMDIChildSystemItems(m_ActiveMdiChild, true);
                    m_MdiChildMaximized = true;
                }
            }
            else
            {
                m_RibbonStrip.ClearMDIChildSystemItems(true);
                m_MdiChildMaximized = false;
            }
        }

        private bool IsMaximized(Form form)
        {
            if (form.WindowState == FormWindowState.Maximized)
            {
                if (form.IsMdiChild && !WinApi.IsZoomed(form.Handle))
                    return false;
                return true;
            }

            return false;
        }
        #endregion

        #region Ribbon Customization

        private bool DisplayCustomizeContextMenu
        {
            get { return this.CanCustomize && (this.CaptionVisible || m_UseExternalCustomization); }
        }
        /// <summary>
        /// Called when right-mouse button is pressed over RibbonBar
        /// </summary>
        /// <param name="ribbonBar">Reference to RibbonBar object.</param>
        internal void OnRibbonBarRightClick(RibbonBar ribbonBar, int x, int y)
        {
            if (!DisplayCustomizeContextMenu)
                return;

            if (ribbonBar.CanCustomize && (ribbonBar.TitleRectangle.Contains(x, y) || ribbonBar.OverflowState))
            {
                ShowCustomizeContextMenu(ribbonBar, false);
            }
            else
            {
                BaseItem item = ribbonBar.HitTest(x, y);
                if (item != null && item.CanCustomize && !item.SystemItem)
                {
                    ShowCustomizeContextMenu(item, false);
                }
            }
            
        }

        /// <summary>
        /// Called when right-mouse button is pressed over RibbonStrip
        /// </summary>
        /// <param name="ribbonStrip">Reference to RibbonStrip object.</param>
        internal void OnRibbonStripRightClick(RibbonStrip ribbonStrip, int x, int y)
        {
            if (!DisplayCustomizeContextMenu)
                return;
            BaseItem item = ribbonStrip.HitTest(x, y);
            if (item != null && item.CanCustomize && !item.SystemItem)
            {
                ShowCustomizeContextMenu(item, true);
            }
        }

        /// <summary>
        /// Displays popup customize context menu for given customization object.
        /// </summary>
        /// <param name="o">Object that should be customized, usually an instance of BaseItem.</param>
        /// <param name="ribbonStrip">Indicates whether customize menu is displayed over ribbon strip</param>
        internal virtual void ShowCustomizeContextMenu(object o, bool ribbonStrip)
        {
            if (o == null || !m_UseCustomizeDialog)
                return;

            m_RibbonStrip.ClosePopup(SYS_CUSTOMIZE_POPUP_MENU);

            ButtonItem cont = new ButtonItem(SYS_CUSTOMIZE_POPUP_MENU);
            cont.Style = eDotNetBarStyle.Office2007;
            cont.SetOwner(m_RibbonStrip);

            if ((CanCustomizeItem(o as BaseItem) || o is RibbonBar) && !m_UseExternalCustomization)
            {
                if (o is BaseItem && this.QuickToolbarItems.Contains((BaseItem)o))
                {

                    ButtonItem b = new ButtonItem(SysQatRemoveFromItemName);
                    b.Text = this.SystemText.QatRemoveItemText; 
                    b.Click += new System.EventHandler(CustomizeRemoveFromQuickAccessToolbar);
                    b.Tag = o;
                    cont.SubItems.Add(b);
                }
                else
                {
                    BaseItem itemToCustomize = o as BaseItem;

                    ButtonItem b = new ButtonItem(SysQatAddToItemName);
                    b.Text = this.SystemText.QatAddItemText;
                    b.Click += new System.EventHandler(CustomizeAddToQuickAccessToolbar);
                    b.Tag = o;
                    cont.SubItems.Add(b);

                    if (itemToCustomize!=null && this.QuickToolbarItems.Contains(itemToCustomize.Name) || 
                        o is RibbonBar && this.QuickToolbarItems.Contains(GetQATRibbonBarName(o as RibbonBar)))
                        b.Enabled = false;

                    if (itemToCustomize != null && BaseItem.IsOnPopup(itemToCustomize) && itemToCustomize.Parent!=null)
                    {
                        Control c = itemToCustomize.ContainerControl as Control;
                        if (c != null) c.VisibleChanged += new EventHandler(CustomizePopupItemParentVisibleChange);
                    }
                }
            }

            if (m_UseCustomizeDialog)
            {
                ButtonItem b = new ButtonItem(SysQatCustomizeItemName);
                b.Text = this.SystemText.QatCustomizeText; 
                b.BeginGroup = true;
                b.Click += new EventHandler(CustomizeQuickAccessToolbarDialog);
                cont.SubItems.Add(b);
            }

            if (m_EnableQatPlacement && !m_UseExternalCustomization)
            {
                ButtonItem b = new ButtonItem(SysQatPlaceItemName);
                if (m_QatPositionedBelow)
                    b.Text = this.SystemText.QatPlaceAboveRibbonText;
                else
                    b.Text = this.SystemText.QatPlaceBelowRibbonText;
                b.Click += new EventHandler(QuickAccessToolbarChangePlacement);
                cont.SubItems.Add(b);
            }

            if (this.AutoExpand)
            {
                ButtonItem b = new ButtonItem(this.Expanded ? SysMinimizeRibbon : SysMaximizeRibbon, this.Expanded?this.SystemText.MinimizeRibbonText:this.SystemText.MaximizeRibbonText);
                b.Click += new EventHandler(MinMaxRibbonClick);
                b.BeginGroup = true;
                cont.SubItems.Add(b);
            }

            RibbonCustomizeEventArgs e = new RibbonCustomizeEventArgs(o, cont);
            OnBeforeCustomizeMenuPopup(e);
            if (e.Cancel)
            {
                cont.Dispose();
                return;
            }

            ((IOwnerMenuSupport)m_RibbonStrip).RegisterPopup(cont);
            cont.Popup(Control.MousePosition);
        }

        private void MinMaxRibbonClick(object sender, EventArgs e)
        {
            this.Expanded = !this.Expanded;
        }

        private void CustomizePopupItemParentVisibleChange(object sender, EventArgs e)
        {
            Control c = sender as Control;
            if (c == null) return;
            c.VisibleChanged -= new EventHandler(CustomizePopupItemParentVisibleChange);
            // Close the Customize Context menu we displayed
            m_RibbonStrip.ClosePopup(SYS_CUSTOMIZE_POPUP_MENU);
        }

        private void QuickAccessToolbarChangePlacement(object sender, EventArgs e)
        {
            QuickAccessToolbarChangePlacement();
        }

        internal void QuickAccessToolbarChangePlacement()
        {
            m_RibbonStrip.ClosePopups();
            if (m_QatPositionedBelow)
            {
                m_QatToolbar.ClosePopups();
                m_QatToolbar.Visible = false;
                ArrayList qatItems = new ArrayList(m_QatToolbar.Items.Count);
                m_QatToolbar.Items.CopyTo(qatItems);
                m_QatToolbar.Items.Clear();
                
                foreach (BaseItem item in qatItems)
                    m_RibbonStrip.CaptionContainerItem.SubItems.Add(item);
                if (m_QatSubItemsCollection != null)
                {
                    m_QatSubItemsCollection._Clear();
                    m_QatSubItemsCollection = null;
                }
                this.DockPadding.Bottom = DefaultBottomDockPadding;
                m_QatPositionedBelow = false;
                m_RibbonStrip.RecalcLayout();
                if (m_AutoSize)
                    this.AutoSyncSize();
                else
                    this.Height -= (m_QatToolbar.Height + m_QatToolbar.DockPadding.Top);
                m_QatToolbar.Dispose();
                m_QatToolbar = null;
            }
            else
            {
                if (m_QatToolbar == null)
                    CreateQatToolbar();
                this.DockPadding.Bottom = 1;
                m_QatToolbar.BeginUpdate();
                ArrayList qatItems = GetQatItems(true);
                foreach (BaseItem item in qatItems)
                {
                    this.QuickToolbarItems.Remove(item);
                    m_QatToolbar.Items.Add(item);
                }
                m_QatToolbar.EndUpdate();
                m_QatToolbar.Height = m_QatToolbar.GetAutoSizeHeight();
                m_QatPositionedBelow = true;
                if (m_AutoSize)
                    this.AutoSyncSize();
                else
                    this.Height += (m_QatToolbar.Height + m_QatToolbar.DockPadding.Top);
            }
            this.RecalcLayout();
            m_QatLayoutChanged = true;
            OnQatPlacementChanged(EventArgs.Empty);
        }

        /// <summary>
        /// Raises the QatPlacementChanged event.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnQatPlacementChanged(EventArgs e)
        {
            EventHandler handler = QatPlacementChanged;
            if (handler != null) handler(this, e);
        }
        private ArrayList GetQatItems(bool includeCustomizeItem)
        {
            ArrayList list = new ArrayList();
            foreach (BaseItem item in this.QuickToolbarItems)
            {
                if (IsSystemItem(item) || item is ButtonItem && ((ButtonItem)item).HotTrackingStyle == eHotTrackingStyle.Image && this.QuickToolbarItems.IndexOf(item) == 0)
                {
                    if (includeCustomizeItem && item is CustomizeItem)
                        list.Add(item);
                    continue;
                }
                list.Add(item);
            }

            return list;
        }

        internal Ribbon.QatToolbar QatToolbar
        {
            get { return m_QatToolbar; }
        }

        private void CreateQatToolbar()
        {
            m_QatToolbar = new Ribbon.QatToolbar();
            m_QatToolbar.FadeEffect = m_RibbonStrip.FadeEffect;
            m_QatToolbar.Style = this.Style;

            //if (this.Style == eDotNetBarStyle.Office2007)
            //{
            //    RibbonPredefinedColorSchemes.ApplyOffice2007ColorScheme(m_QatToolbar);
            //}
            //else
            //{
            //    m_QatToolbar.BackgroundStyle.ApplyStyle(this.BackgroundStyle);
            //}
            m_QatToolbar.Dock = DockStyle.Bottom;
            this.Controls.Add(m_QatToolbar);
            if (!this.Expanded)
                m_QatToolbar.BringToFront();
        }

        private void CustomizeQuickAccessToolbarDialog(object sender, EventArgs e)
        {
            ShowQatCustomizeDialog();
        }
        
        private bool CanCustomizeItem(BaseItem item)
        {
            if (item == null)
                return false;

            if (!item.CanCustomize || item.SystemItem)
                return false;

            if (item is ItemContainer) // && ((ItemContainer)item).SystemContainer)
                return false;

            if (item is ButtonItem && this.QuickToolbarItems.IndexOf(item) == 0)
                return false;

            return true;
        }

        private string GetQATRibbonBarName(RibbonBar bar)
        {
            return "qt_" + bar.Name;
        }

        private void CustomizeRemoveFromQuickAccessToolbar(object sender, System.EventArgs e)
        {
            ButtonItem b = sender as ButtonItem;
            if (b.Tag is BaseItem)
            {
                BaseItem item = b.Tag as BaseItem;
                RemoveItemFromQuickAccessToolbar(item);
            }

            b.Tag = null;
        }

        /// <summary>
        /// Removes an item from the Quick Access Toolbar.
        /// </summary>
        /// <param name="item">Reference to the item that is already part of Quick Access Toolbar.</param>
        public void RemoveItemFromQuickAccessToolbar(BaseItem item)
        {
            RibbonCustomizeEventArgs rc = new RibbonCustomizeEventArgs(item, null);
            OnBeforeRemoveItemFromQuickAccessToolbar(rc);

            if (!rc.Cancel)
            {
                this.QuickToolbarItems.Remove(item);
                //item.Parent.SubItems.Remove(item);
                this.RecalcLayout();
                m_QatLayoutChanged = true;
            }
        }

        private void CustomizeAddToQuickAccessToolbar(object sender, System.EventArgs e)
        {
            ButtonItem b = sender as ButtonItem;

            AddItemToQuickAccessToolbar(b.Tag);

            b.Tag=null;
        }

        /// <summary>
        /// Adds an instance of base type BaseItem or RibbonBar to the Quick Access Toolbar. Note that this method creates
        /// new instance of the item or an representation of the item being added and adds that to the Quick Access Toolbar.
        /// </summary>
        /// <param name="originalItem">Reference to the item to add, must be an BaseItem type or RibbonBar type.</param>
        public void AddItemToQuickAccessToolbar(object originalItem)
        {
            BaseItem copy = GetQatItemCopy(originalItem);
            if (copy != null)
            {
                RibbonCustomizeEventArgs re = new RibbonCustomizeEventArgs(copy, null);
                OnBeforeAddItemToQuickAccessToolbar(re);
                if (!re.Cancel)
                {
                    this.QuickToolbarItems.Add(copy);
                    this.RecalcLayout();
                    m_QatLayoutChanged = true;
                }
            }
        }

        private BaseItem GetQatItemCopy(object o)
        {
            BaseItem copy = null;
            if (o is ButtonItem)
            {
                ButtonItem button = ((ButtonItem)o).Copy() as ButtonItem;
                button.KeyTips = "";
                button.ImageFixedSize = Size.Empty;
                button.FixedSize = Size.Empty;
                button.ButtonStyle = eButtonStyle.Default;
                button.ImagePosition = eImagePosition.Left;
                button.BeginGroup = false;
                button.UseSmallImage = true;
                
                if (button.TextMarkupBody != null && button.TextMarkupBody.HasExpandElement)
                    button.Text = TextMarkup.MarkupParser.RemoveExpand(button.Text);

                if (button.Image == null && button.ImageIndex == -1 && button.Icon == null)
                    button.Image = BarFunctions.LoadBitmap("SystemImages.GreenLight.png");
                else
                    button.ImageFixedSize = new Size(16, 16);
                copy = button;
            }
            else if (o is BaseItem)
            {
                copy = ((BaseItem)o).Copy();
                copy.KeyTips = "";
            }
            else if (o is RibbonBar)
            {
                RibbonBar bar = o as RibbonBar;
                ButtonItem item = new ButtonItem(GetQATRibbonBarName(bar));
                item.Image = bar.GetOverflowButtonImage();
                item.ImageFixedSize = new Size(16, 16);
                item.AutoExpandOnClick = true;
                item.Tooltip = bar.Text;
                item.Text = bar.Text;
                RibbonBar qatBar = null;
                bool recalcLayout = false;
                if (bar.IsItemsAutoSizeActive)
                {
                    bar.RestoreAutoSizedItems();
                    qatBar = bar.CreateOverflowRibbon(true);
                    recalcLayout = true;
                }
                else
                    qatBar = bar.CreateOverflowRibbon(true);
                qatBar.IsOnQat = true;
                qatBar.QatButtonParent = item;
                foreach (BaseItem child in bar.Items)
                {
                    BaseItem childCopy = child.Copy();
                    qatBar.Items.Add(childCopy);
                }
                if (recalcLayout) bar.RecalcLayout();

                ItemContainer ic = new ItemContainer();
                ic.Stretch = true;
                ControlContainerItem cont = new ControlContainerItem();
                cont.AllowItemResize = false;
                ic.SubItems.Add(cont);
                cont.Control = qatBar;
                item.SubItems.Add(ic);

                copy = item;
            }

            return copy;
        }

        /// <summary>
        /// Raises the BeforeCustomizeMenuPopup event.
        /// </summary>
        /// <param name="e">Event arguments</param>
        protected virtual void OnBeforeCustomizeMenuPopup(RibbonCustomizeEventArgs e)
        {
            if (BeforeCustomizeMenuPopup != null)
                BeforeCustomizeMenuPopup(this, e);
        }

        /// <summary>
        /// Raises the BeforeAddItemToQuickAccessToolbar event.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnBeforeAddItemToQuickAccessToolbar(RibbonCustomizeEventArgs e)
        {
            if (BeforeAddItemToQuickAccessToolbar != null)
                BeforeAddItemToQuickAccessToolbar(this, e);
        }

        protected virtual void OnBeforeRemoveItemFromQuickAccessToolbar(RibbonCustomizeEventArgs e)
        {
            if (BeforeRemoveItemFromQuickAccessToolbar != null)
                BeforeRemoveItemFromQuickAccessToolbar(this, e);
        }

        /// <summary>
        /// Gets or sets whether control can be customized and items added by end-user using context menu to the quick access toolbar.
        /// Caption of the control must be visible for customization to be enabled. Default value is true.
        /// </summary>
        [DefaultValue(true), Browsable(true), Category("Quick Access Toolbar"), Description("Indicates whether control can be customized. Caption must be visible for customization to be fully enabled.")]
        public bool CanCustomize
        {
            get { return m_RibbonStrip.CanCustomize; }
            set { m_RibbonStrip.CanCustomize = value; }
        }

        /// <summary>
        /// Gets or sets whether external implementation for ribbon bar and menu item customization will be used for customizing the ribbon control. When set to true
        /// it enables the displaying of RibbonBar and menu item context menus which allow customization. You are responsible for
        /// adding the menu items to context menu to handle all aspects of item customization. See "Ribbon Control Quick Access Toolbar Customization" topic in help file under How To.
        /// Default value is false.
        /// </summary>
        [DefaultValue(false), Browsable(true), Category("Quick Access Toolbar"), Description("Indicates whether external implementation for ribbon bar and menu item customization will be used for customizing the ribbon control.")]
        public bool UseExternalCustomization
        {
            get { return m_UseExternalCustomization; }
            set { m_UseExternalCustomization = value; }
        }

        /// <summary>
        /// Gets or sets whether end-user customization of the placement of the Quick Access Toolbar is enabled. User
        /// can change the position of the Quick Access Toolbar using the customize menu. Default value is true.
        /// </summary>
        [Browsable(true), DefaultValue(true), Category("Quick Access Toolbar"), Description("Indicates whether end-user customization of the placement of the Quick Access Toolbar is enabled.")]
        public bool EnableQatPlacement
        {
            get { return m_EnableQatPlacement; }
            set { m_EnableQatPlacement = value; }
        }

        /// <summary>
        /// Gets or sets whether customize dialog is used to customize the quick access toolbar. You can handle the EnterCustomize event 
        /// to display your custom dialog instead of built-in dialog for customization. Default value is true.
        /// </summary>
        [DefaultValue(true), Browsable(true), Category("Quick Access Toolbar"), Description("Indicates whether customize dialog is used to customize the quick access toolbar.")]
        public bool UseCustomizeDialog
        {
            get { return m_UseCustomizeDialog; }
            set { m_UseCustomizeDialog = value; }
        }

        /// <summary>
        /// Gets or sets the categorization mode for the items on Quick Access Toolbar customize dialog box. Default value categorizes
        /// items by the ribbon bar they appear on.
        /// </summary>
        [DefaultValue(eCategorizeMode.RibbonBar), Browsable(true), Category("Quick Access Toolbar"), Description("Indicates categorization mode for the items on Quick Access Toolbar customize dialog box.")]
        public eCategorizeMode CategorizeMode
        {
            get { return m_CategorizeMode; }
            set { m_CategorizeMode = value; }
        }

        /// <summary>
        /// Shows the quick access toolbar customize dialog.
        /// </summary>
        public virtual void ShowQatCustomizeDialog()
        {
            Form qatDialog = CreateQatCustomizeDialog();

            if (BeforeQatCustomizeDialog != null)
            {
                QatCustomizeDialogEventArgs ce = new QatCustomizeDialogEventArgs(qatDialog);
                BeforeQatCustomizeDialog(this, ce);
                if (ce.Cancel || ce.Dialog==null)
                {
                    qatDialog.Dispose();
                    return;
                }

                qatDialog = ce.Dialog;
            }

            if (qatDialog is Ribbon.QatCustomizeDialog)
                ((Ribbon.QatCustomizeDialog)qatDialog).LoadItems(this);

            Form form = this.FindForm();
            qatDialog.StartPosition = FormStartPosition.CenterParent;
            DialogResult result = qatDialog.ShowDialog(form);

            if (AfterQatCustomizeDialog != null)
            {
                QatCustomizeDialogEventArgs ce = new QatCustomizeDialogEventArgs(qatDialog);
                AfterQatCustomizeDialog(this, ce);
                if (ce.Cancel)
                {
                    qatDialog.Dispose();
                    return;
                }
            }

            if (result == DialogResult.Cancel)
            {
                qatDialog.Dispose();
                return;
            }

            // Apply changes to the Quick Access Toolbar
            Ribbon.QatCustomizeDialog qd = qatDialog as Ribbon.QatCustomizeDialog;
            if (qd == null || !qd.QatCustomizePanel.DataChanged) return;
            ApplyQatCustomizePanelChanges(qd.QatCustomizePanel);
            qatDialog.Dispose();
        }

        /// <summary>
        /// Applies the Quick Access Toolbar customization changes made on QatCustomizePanel to the Ribbon Control Quick Access Toolbar. Note that QatCustomizePanel.DataChanged property indicates whether user made any changes to the data on the panel.
        /// </summary>
        /// <param name="customizePanel">Reference to the QatCustomizePanel</param>
        public void ApplyQatCustomizePanelChanges(Ribbon.QatCustomizePanel customizePanel)
        {
            if (customizePanel == null || !customizePanel.DataChanged) return;
            m_RibbonStrip.BeginUpdate();
            try
            {
                ItemPanel itemPanelQat = customizePanel.itemPanelQat;

                int start = 0;
                BaseItem startButton = GetApplicationButton();
                if (startButton != null)
                    start = this.QuickToolbarItems.IndexOf(startButton) + 1;

                ArrayList removeList = new ArrayList();
                SubItemsCollection qatList = new SubItemsCollection(null);

                for (int i = start; i < this.QuickToolbarItems.Count; i++)
                {
                    BaseItem item = this.QuickToolbarItems[i];
                    if (IsSystemItem(item))
                        continue;

                    if (!itemPanelQat.Items.Contains(item.Name))
                        removeList.Add(item);
                    else
                        qatList._Add(item);
                }

                foreach (BaseItem item in removeList)
                    this.QuickToolbarItems.Remove(item);
                foreach (BaseItem item in qatList)
                    this.QuickToolbarItems.Remove(item);

                foreach (BaseItem item in itemPanelQat.Items)
                {
                    // Already exists on Quick Access Toolbar
                    if (item.Tag != null)
                    {
                        BaseItem copy = GetQatItemCopy(item.Tag as BaseItem);
                        this.QuickToolbarItems.Add(copy);
                    }
                    else
                    {
                        BaseItem qatItem = qatList[item.Name];
                        if (qatItem != null)
                            this.QuickToolbarItems.Add(qatItem);
                    }
                }

                m_QatLayoutChanged = true;
            }
            finally
            {
                m_RibbonStrip.EndUpdate();
            }
            if (customizePanel.checkQatBelowRibbon.Checked != m_QatPositionedBelow)
                QuickAccessToolbarChangePlacement();

            OnAfterQatDialogChangesApplied();
        }

        /// <summary>
        /// Raises the AfterQatDialogChangesApplied event.
        /// </summary>
        protected virtual void OnAfterQatDialogChangesApplied()
        {
            if (AfterQatDialogChangesApplied != null)
                AfterQatDialogChangesApplied(this, new EventArgs());
        }

        private Ribbon.QatCustomizeDialog CreateQatCustomizeDialog()
        {
            Ribbon.QatCustomizeDialog qat = new Ribbon.QatCustomizeDialog();
            qat.Text = SystemText.QatDialogCaption;
            qat.buttonCancel.Text = SystemText.QatDialogCancelButton;
            qat.buttonOK.Text = SystemText.QatDialogOkButton;
            qat.QatCustomizePanel.buttonAddToQat.Text = SystemText.QatDialogAddButton;
            qat.QatCustomizePanel.buttonRemoveFromQat.Text = SystemText.QatDialogRemoveButton;
            qat.QatCustomizePanel.labelCategories.Text = SystemText.QatDialogCategoriesLabel;
            qat.QatCustomizePanel.checkQatBelowRibbon.Text = SystemText.QatDialogPlacementCheckbox;

            return qat;
        }

        private bool IsSystemItem(BaseItem item)
        {
            if (item.SystemItem || item is ItemContainer || item is CustomizeItem || item is SystemCaptionItem)
                return true;
            return false;
        }
        /// <summary>
        /// Returns the ribbon Application Button.
        /// </summary>
        /// <returns>reference to Application Button or null if button is not found.</returns>
        public BaseItem GetApplicationButton()
        {
			return m_RibbonStrip.GetApplicationButton();
        }

        /// <summary>
        /// Gets or sets whether Quick Access toolbar is positioned below the ribbon.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool QatPositionedBelowRibbon
        {
            get { return m_QatPositionedBelow; }
            set
            {
                if (m_QatPositionedBelow != value)
                    QuickAccessToolbarChangePlacement();
            }
        }

        /// <summary>
        /// Gets or sets the Quick Access Toolbar layout description. You can use the value obtained from this property to save
        /// the customized Quick Access Toolbar into registry or into any other storage object. You can also set the saved layout description back
        /// to restore user customize layout.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string QatLayout
        {
            get
            {
                return GetQatLayoutDescription();
            }
            set
            {
                SetQatLayoutDescription(value);
            }
        }

        private string GetQatLayoutDescription()
        {
            ArrayList qatList = GetQatItems(false);
            StringBuilder layoutDesc = new StringBuilder();
            if (m_QatPositionedBelow)
                layoutDesc.Append("1");
            else
                layoutDesc.Append("0");
            foreach (BaseItem item in qatList)
            {
                if (item.Name != "")
                {
                    layoutDesc.Append("," + item.Name);
                }
            }

            return layoutDesc.ToString();
        }

        private void SetQatLayoutDescription(string layoutDesc)
        {
            if (layoutDesc == "" || layoutDesc == null)
                return;

            string[] values = layoutDesc.Split(',');
            if (values.Length == 0)
                return;
            bool qatBelow = values[0] == "1";
            ArrayList qatItems = new ArrayList();
            for (int i = 1; i < values.Length; i++)
            {
                if (this.QuickToolbarItems.Contains(values[i]))
                    qatItems.Add(this.QuickToolbarItems[values[i]]);
                else
                {
                    if (values[i].StartsWith("qt_"))
                    {
                        RibbonBar bar = GetRibbonBar(values[i].Substring(3));
                        if(bar!=null)
                            qatItems.Add(GetQatItemCopy(bar));
                    }
                    else
                    {
                        BaseItem item = m_RibbonStrip.GetItem(values[i]);
                        if (item != null && CanCustomizeItem(item))
                            qatItems.Add(GetQatItemCopy(item));
                    }

                }
            }

            ArrayList removeItems = GetQatItems(false);
            foreach (BaseItem item in removeItems)
                this.QuickToolbarItems.Remove(item);

            foreach (BaseItem item in qatItems)
                this.QuickToolbarItems.Add(item);

            if (m_QatPositionedBelow != qatBelow)
                QuickAccessToolbarChangePlacement();

            m_QatLayoutChanged = false;
        }

        private RibbonBar GetRibbonBar(string name)
        {
            foreach (Control c in this.Controls)
            {
                RibbonBar bar = GetRibbonBar(c, name);
                if (bar != null)
                    return bar;
            }
            return null;
        }

        private RibbonBar GetRibbonBar(Control parent, string name)
        {
            if (parent is RibbonBar && parent.Name == name)
                return parent as RibbonBar;
            foreach (Control c in parent.Controls)
            {
                RibbonBar bar = GetRibbonBar(c, name);
                if (bar != null)
                    return bar;
            }

            return null;
        }

        /// <summary>
        /// Gets or sets whether Quick Access Toolbar has been customized by end-user. You can use value of this property to determine
        /// whether Quick Access Toolbar layout that can be accessed using QatLayout property should be saved.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool QatLayoutChanged
        {
            get { return m_QatLayoutChanged; }
            set { m_QatLayoutChanged = value; }
        }

        /// <summary>
        /// Gets the reference to the ribbon localization object which holds all system text used by the component.
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), NotifyParentPropertyAttribute(true), Category("Localization"), Description("Gets system text used by the component.."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public RibbonLocalization SystemText
        {
            get { return m_SystemText; }
        }
        #endregion
    }
}
