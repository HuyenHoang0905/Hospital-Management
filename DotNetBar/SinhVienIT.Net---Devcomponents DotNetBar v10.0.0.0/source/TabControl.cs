using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Reflection;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Summary description for TabControl.
	/// </summary>
    [ToolboxItem(true), Designer("DevComponents.DotNetBar.Design.TabControlDesigner, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
	public class TabControl : System.Windows.Forms.Control,ISupportInitialize
	{
		#region Events
		/// <summary>
		/// Occurs when selected tab changes.
		/// </summary>
		public event TabStrip.SelectedTabChangedEventHandler SelectedTabChanged;
		/// <summary>
		/// Occurs before selected tab changes and gives you opportunity to cancel the change.
		/// </summary>
		public event TabStrip.SelectedTabChangingEventHandler SelectedTabChanging;
		/// <summary>
		/// Occurs when tab is dragged by user.
		/// </summary>
		public event TabStrip.TabMovedEventHandler TabMoved;
		/// <summary>
		/// Occurs when the user navigates back using the back arrow.
		/// </summary>
		public event TabStrip.UserActionEventHandler NavigateBack;
		/// <summary>
		/// Occurs when the user navigates forward using the forward arrow.
		/// </summary>
		public event TabStrip.UserActionEventHandler NavigateForward;
		/// <summary>
		/// Occurs when tab item is closing.
		/// </summary>
		public event TabStrip.UserActionEventHandler TabItemClose;
		/// <summary>
		/// Occurs when tab item is added to the tabs collection.
		/// </summary>
		public event EventHandler TabItemOpen;
		/// <summary>
		/// Occurs before control or item attached to the tab is displayed.
		/// </summary>
		public event EventHandler BeforeTabDisplay;
		/// <summary>
		/// Occurs after tab item has been removed from tabs collection.
		/// </summary>
		public event EventHandler TabRemoved;
		#endregion

        #region Private Variables & Constructor, Dispose
        private DevComponents.DotNetBar.TabStrip tabStrip1;
        //private int m_SelectedTabIndex=0;
		private System.Windows.Forms.Timer m_ClickTimer=null;
        private bool m_AutoCloseTabs = false;
        private bool m_TabStripVisible = true;
        private int m_LoadSelectedTabIndex = -1;
        private bool m_SyncingTabStripSize = false;

		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public TabControl()
		{
			this.SetStyle(ControlStyles.Opaque,true);
			this.SetStyle(DisplayHelp.DoubleBufferFlag,true);
			this.SetStyle(ControlStyles.AllPaintingInWmPaint,true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);

			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
			//tabStrip1.VariableTabWidth=false;
			tabStrip1.TabLayoutType=eTabLayoutType.FixedWithNavigationBox;
			tabStrip1.SizeRecalculated+=new EventHandler(this.TabStripSizeRecalculated);

            StyleManager.Register(this);
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
                StyleManager.Unregister(this);
			}
			base.Dispose( disposing );
        }
        #endregion

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.tabStrip1 = new DevComponents.DotNetBar.TabStrip();
			this.SuspendLayout();
			// 
			// tabStrip1
			// 
			this.tabStrip1.CanReorderTabs = true;
			this.tabStrip1.CloseButtonVisible = true;
			this.tabStrip1.Dock = System.Windows.Forms.DockStyle.Top;
			this.tabStrip1.ImageList = null;
			this.tabStrip1.Name = "tabStrip1";
			this.tabStrip1.SelectedTab = null;
			this.tabStrip1.Size = new System.Drawing.Size(256, 26);
			this.tabStrip1.Style = DevComponents.DotNetBar.eTabStripStyle.OneNote;
			this.tabStrip1.TabAlignment = DevComponents.DotNetBar.eTabStripAlignment.Top;
			this.tabStrip1.TabIndex = 0;
			this.tabStrip1.Text = "tabStrip1";
			this.tabStrip1.AutoHideSystemBox=true;
			this.tabStrip1.CloseButtonVisible=false;
			this.tabStrip1.TabRemoved += new System.EventHandler(this.tabStrip1_TabRemoved);
            this.tabStrip1.TabItemOpen += new EventHandler(tabStrip1_TabItemAdded);
			this.tabStrip1.SelectedTabChanged += new DevComponents.DotNetBar.TabStrip.SelectedTabChangedEventHandler(this.tabStrip1_SelectedTabChanged);
			this.tabStrip1.SelectedTabChanging += new DevComponents.DotNetBar.TabStrip.SelectedTabChangingEventHandler(this.tabStrip1_SelectedTabChanging);
			this.tabStrip1.TabMoved+=new TabStrip.TabMovedEventHandler(this.tabStrip1_TabMoved);
			this.tabStrip1.NavigateBack+=new TabStrip.UserActionEventHandler(this.tabStrip1_NavigateBack);
			this.tabStrip1.NavigateForward+=new TabStrip.UserActionEventHandler(this.tabStrip1_NavigateForward);
			this.tabStrip1.TabItemClose+=new TabStrip.UserActionEventHandler(this.tabStrip1_TabItemClose);
			this.tabStrip1.TabItemOpen+=new EventHandler(this.tabStrip1_TabItemOpen);
            this.tabStrip1.TabsCleared += new EventHandler(this.tabStrip1_TabsCleared);
			this.tabStrip1.BeforeTabDisplay+=new EventHandler(this.tabStrip1_BeforeTabDisplay);
			this.tabStrip1.TabStop=false;
			// 
			// TabControl
			// 
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.tabStrip1});
			this.Name = "TabControl";
			this.Size = new System.Drawing.Size(256, 152);
			this.ResumeLayout(false);

		}
		#endregion

		#region Hidden Properties
		/// <summary>
		/// Gets or sets the background color.
		/// </summary>
		[Browsable(true),Description("Indicates the background color."),Category("Style")]
		public override Color BackColor
		{
			get {return base.BackColor;}
			set
            {
                base.BackColor=value;
                if (tabStrip1 != null)
                    tabStrip1.BackColor = value;
            }
		}
        /// <summary>
        /// Gets whether property should be serialized. Used by Windows Forms designer.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeBackColor()
        {
            return (BackColor != SystemColors.Control);
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
            UpdateTabPanelStyle();
        }

        protected override void OnGotFocus(EventArgs e)
        {
            if (this.TabStripTabStop)
                tabStrip1.Focus();
            base.OnGotFocus(e);
        }
#if FRAMEWORK20
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public virtual bool ValidateChildren()
        {
            return this.ValidateChildren(ValidationConstraints.Selectable);
        }

        [Browsable(false)]
        public virtual bool ValidateChildren(ValidationConstraints validationConstraints)
        {
            if ((validationConstraints < ValidationConstraints.None) || (validationConstraints > (ValidationConstraints.ImmediateChildren | ValidationConstraints.TabStop | ValidationConstraints.Visible | ValidationConstraints.Enabled | ValidationConstraints.Selectable)))
            {
                throw new InvalidEnumArgumentException("validationConstraints", (int)validationConstraints, typeof(ValidationConstraints));
            }
            Type t = typeof(Control);
            MethodInfo mi = t.GetMethod("PerformContainerValidation", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Instance);
            if (mi != null)
            {
                bool b = (bool)mi.Invoke(this, new object[] { validationConstraints });
                return !b;
            }
            return true;
        }
#endif

        /// <summary>
        /// Gets the collection of all tabs.
        /// </summary>
        [Browsable(false), System.ComponentModel.Editor("DevComponents.DotNetBar.Design.TabStripTabsEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(System.Drawing.Design.UITypeEditor)), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), System.ComponentModel.Category("Data"), System.ComponentModel.Description("Returns the collection of Tabs.")]
        public TabsCollection Tabs
        {
            get { return tabStrip1.Tabs; }
        }

        /// <summary>
        /// Gets or sets whether tabs are visible. Default value is true.
        /// </summary>
        [DefaultValue(true), Category("Appearance"), Description("Indicates whether tabs are visible")]
        public bool TabsVisible
        {
            get { return m_TabStripVisible; }
            set
            {
                m_TabStripVisible = value;
                tabStrip1.Visible = value;
                RefreshPanelsStyle();
            }
        }

        /// <summary>
        /// Returns reference to internal tab strip control.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TabStrip TabStrip
        {
            get { return tabStrip1; }
        }

        /// <summary>
        /// Gets or sets whether TabStrip will get focus when Tab key is used. Default value is false.
        /// </summary>
        [DefaultValue(false), Category("Behavior"), Description("Gets or sets whether TabStrip will get focus when Tab key is used.")]
        public bool TabStripTabStop
        {
            get { return tabStrip1.TabStop; }
            set
            {
                tabStrip1.TabStop = value;
            }
        }
        

        public void RecalcLayout()
        {
            tabStrip1.Refresh();
            this.SyncTabStripSize();
        }


		private void tabStrip1_SelectedTabChanged(object sender, DevComponents.DotNetBar.TabStripTabChangedEventArgs e)
		{
			if(e.NewTab!=null && e.NewTab.AttachedControl!=null)
				e.NewTab.AttachedControl.BringToFront();

			InvokeSelectedTabChanged(e);
		}

		protected virtual void InvokeSelectedTabChanged(DevComponents.DotNetBar.TabStripTabChangedEventArgs e)
		{
			if(SelectedTabChanged!=null)
				SelectedTabChanged(this,e);
		}

		private void tabStrip1_SelectedTabChanging(object sender, DevComponents.DotNetBar.TabStripTabChangingEventArgs e)
		{
			InvokeSelectedTabChanging(e);
		}

		protected virtual void InvokeSelectedTabChanging(DevComponents.DotNetBar.TabStripTabChangingEventArgs e)
		{
			if(SelectedTabChanging!=null)
				SelectedTabChanging(this,e);
		}

		private void tabStrip1_TabMoved(object sender, TabStripTabMovedEventArgs e)
		{
			InvokeTabMoved(e);
		}

		protected virtual void InvokeTabMoved(TabStripTabMovedEventArgs e)
		{
			if(TabMoved!=null)
				TabMoved(this,e);
		}

		private void tabStrip1_NavigateBack(object sender, TabStripActionEventArgs e)
		{
			InvokeNavigateBack(e);
		}

		protected virtual void InvokeNavigateBack(TabStripActionEventArgs e)
		{
			if(NavigateBack!=null)
				NavigateBack(this,e);
		}

		private void tabStrip1_NavigateForward(object sender, TabStripActionEventArgs e)
		{
			InvokeNavigateForward(e);
		}

		protected virtual void InvokeNavigateForward(TabStripActionEventArgs e)
		{
			if(NavigateForward!=null)
				NavigateForward(this,e);
		}

		private void tabStrip1_TabItemClose(object sender, TabStripActionEventArgs e)
		{
			InvokeTabItemClose(e);

            if (!e.Cancel && m_AutoCloseTabs && !this.DesignMode)
            {
                TabItem tab = tabStrip1.SelectedTab;
                if (tab != null)
                {
                    tabStrip1.Tabs.Remove(tab);
                    Control c = tab.AttachedControl;
                    if (c != null)
                    {
                        if (this.Controls.Contains(c))
                            this.Controls.Remove(c);
                        c.Dispose();
                    }
                }
                this.RecalcLayout();
            }
		}

		protected virtual void InvokeTabItemClose(TabStripActionEventArgs e)
		{
			if(TabItemClose!=null)
				TabItemClose(this,e);
		}

		private void tabStrip1_TabItemOpen(object sender, EventArgs e)
		{
			InvokeTabItemOpen(e);
		}

		protected virtual void InvokeTabItemOpen(EventArgs e)
		{
			if(TabItemOpen!=null)
				TabItemOpen(this,e);
		}

		private void tabStrip1_BeforeTabDisplay(object sender, EventArgs e)
		{
			InvokeBeforeTabDisplay(e);
		}

		protected virtual void InvokeBeforeTabDisplay(EventArgs e)
		{
			if(BeforeTabDisplay!=null)
				BeforeTabDisplay(this,e);
		}

		protected override void OnControlAdded(ControlEventArgs e)
		{
			base.OnControlAdded(e);

            if (this.DesignMode)
                return;
			TabControlPanel panel=e.Control as TabControlPanel;
			if(panel==null)
				return;
			if(panel.TabItem!=null)
			{
				if(this.Tabs.Contains(panel.TabItem) && this.SelectedTab==panel.TabItem)
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

		protected override void OnControlRemoved(ControlEventArgs e)
		{
			base.OnControlRemoved(e);

			TabControlPanel panel=e.Control as TabControlPanel;
			if(panel==null)
				return;
			if(panel.TabItem!=null)
			{
				if(this.Tabs.Contains(panel.TabItem))
					this.Tabs.Remove(panel.TabItem);
			}
		}

//		protected override void OnEnter(EventArgs e)
//		{
//			base.OnEnter(e);
//			if(this.Controls.Count>0)
//			{
//				foreach(Control c in this.Controls)
//				{
//					if(c.Visible && c.Controls.Count>0)
//					{
//						this.SelectNextControl(c,true,true,true,true);
//					}
//				}
//			}
//		}

		private void tabStrip1_TabRemoved(object sender, System.EventArgs e)
		{
			if(sender is TabItem)
			{
				TabItem tab=sender as TabItem;
				if(tab.AttachedControl!=null && this.Controls.Contains(tab.AttachedControl))
				{
					this.Controls.Remove(tab.AttachedControl);
				}
			}

			InvokeTabRemoved(e);
		}

        void tabStrip1_TabItemAdded(object sender, EventArgs e)
        {
            TabItem item = sender as TabItem;
            if (!this.DesignMode && !m_Initializing && item!=null && item.AttachedControl!=null && !this.Controls.Contains(item.AttachedControl))
            {
                this.Controls.Add(item.AttachedControl);
            }
        }

		protected virtual void InvokeTabRemoved(EventArgs e)
		{
			if(TabRemoved!=null)
				TabRemoved(this,e);
		}

        private void tabStrip1_TabsCleared(object sender, EventArgs e)
        {
            if (this.DesignMode) return;
            Control[] cs = new Control[this.Controls.Count];
            this.Controls.CopyTo(cs, 0);
            foreach (Control c in cs)
            {
                if (c is TabControlPanel)
                {
                    this.Controls.Remove(c);
                }
            }
        }

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
            
            if (this.BackColor == Color.Transparent || this.ColorScheme.TabBackground == Color.Transparent)
            {
                base.OnPaintBackground(e);
            }

			if(this.Controls.Count<=1)
			{
				using(SolidBrush brush=new SolidBrush(SystemColors.Control))
					e.Graphics.FillRectangle(brush,this.DisplayRectangle);
			}
		}

		protected override void OnResize(EventArgs e)
		{
            base.OnResize(e);

            if (this.Width == 0 || this.Height == 0)
                return;

			if(tabStrip1.IsMultiLine)
			{
				CreateSyncSizeTimer();
			}
		}

		private void CreateSyncSizeTimer()
		{
			if(m_ClickTimer!=null)
				return;
			m_ClickTimer=new Timer();
			m_ClickTimer.Interval=10;
			m_ClickTimer.Tick+=new EventHandler(this.SyncSizeTimerTick);
			m_ClickTimer.Enabled=true;
			m_ClickTimer.Start();
		}
		private void SyncSizeTimerTick(object sender, EventArgs e)
		{
			m_ClickTimer.Stop();
			m_ClickTimer.Enabled=false;
			m_ClickTimer=null;
			try
			{
				this.PerformLayout();
			}
			catch{}
		}

		/// <summary>
		/// Applies default tab colors to the panel
		/// </summary>
		/// <param name="panel">Panel to apply colors to.</param>
		public void ApplyDefaultPanelStyle(TabControlPanel panel)
		{
			if(panel==null || panel.UseCustomStyle)
				return;

			if(panel.TabItem!=null && !panel.TabItem.BackColor.IsEmpty)
			{
				if(!panel.TabItem.BackColor2.IsEmpty)
				{
					panel.Style.BackColor1.Color=panel.TabItem.BackColor;
					panel.Style.BackColor2.Color=panel.TabItem.BackColor2;
					switch(tabStrip1.TabAlignment)
					{
						case eTabStripAlignment.Top:
							panel.Style.GradientAngle=panel.TabItem.BackColorGradientAngle-180;
							break;
						case eTabStripAlignment.Bottom:
							panel.Style.GradientAngle=panel.TabItem.BackColorGradientAngle;
							break;
						case eTabStripAlignment.Left:
							panel.Style.GradientAngle=panel.TabItem.BackColorGradientAngle-90;
							break;
						case eTabStripAlignment.Right:
							panel.Style.GradientAngle=panel.TabItem.BackColorGradientAngle+90;
							break;
					}
				}
			}
			else
			{
				panel.Style.BackColor1.Color=tabStrip1.ColorScheme.TabPanelBackground;
				panel.Style.BackColor2.Color=tabStrip1.ColorScheme.TabPanelBackground2;
				
				switch(tabStrip1.TabAlignment)
				{
					case eTabStripAlignment.Top:
					{
						panel.Style.GradientAngle=tabStrip1.ColorScheme.TabPanelBackgroundGradientAngle;
						break;
					}
					case eTabStripAlignment.Bottom:
					{
						panel.Style.GradientAngle=tabStrip1.ColorScheme.TabPanelBackgroundGradientAngle-180;
						break;
					}
					case eTabStripAlignment.Left:
					{
						panel.Style.GradientAngle=tabStrip1.ColorScheme.TabPanelBackgroundGradientAngle-90;
						break;
					}
					case eTabStripAlignment.Right:
					{
						panel.Style.GradientAngle=tabStrip1.ColorScheme.TabPanelBackgroundGradientAngle+90;
						break;
					}
				}
				panel.Style.BorderColor.Color=tabStrip1.ColorScheme.TabPanelBorder;
				
			}

			panel.Style.Border=eBorderType.SingleLine;

            if (m_TabStripVisible)
            {
                switch (tabStrip1.TabAlignment)
                {
                    case eTabStripAlignment.Top:
                        panel.Style.BorderSide = (eBorderSide.Left | eBorderSide.Right | eBorderSide.Bottom);
                        break;
                    case eTabStripAlignment.Bottom:
                        panel.Style.BorderSide = (eBorderSide.Left | eBorderSide.Right | eBorderSide.Top);
                        break;
                    case eTabStripAlignment.Left:
                        panel.Style.BorderSide = (eBorderSide.Top | eBorderSide.Right | eBorderSide.Bottom);
                        break;
                    case eTabStripAlignment.Right:
                        panel.Style.BorderSide = (eBorderSide.Left | eBorderSide.Top | eBorderSide.Bottom);
                        break;
                }
            }
            else
                panel.Style.BorderSide = eBorderSide.All;


			if(tabStrip1.IsThemed)
				panel.DrawThemedPane=true;
			else
                panel.DrawThemedPane=false;
		}

        private void UpdateTabPanelStyle()
        {
            foreach (Control c in this.Controls)
            {
                if (c is TabControlPanel)
                {
                    TabControlPanel panel = c as TabControlPanel;
                    ApplyDefaultPanelStyle(panel);
                }
            }
        }
        protected override void OnSystemColorsChanged(EventArgs e)
		{
			base.OnSystemColorsChanged(e);
			Application.DoEvents();
            UpdateTabPanelStyle();
		}

		/// <summary>
		/// Resumes normal layout logic. Optionally forces an immediate layout of pending layout requests.
		/// </summary>
		public new void ResumeLayout(bool performLayout)
		{
			base.ResumeLayout(performLayout);
            if (!this.IsHandleCreated)
            {
                m_DelayedSync = true;
            }
            else
            {
                tabStrip1.ResetTabHeight();
                SyncTabStripSize();
            }
		}
        private bool m_DelayedSync = false;
        protected override void OnHandleCreated(EventArgs e)
        {
            if (m_DelayedSync || this.DesignMode)
            {
                m_DelayedSync = false;
                tabStrip1.ResetTabHeight();
                SyncTabStripSize();

                if (this.DesignMode) // There is bug in VS2010 designer where tab alignment is not updated we need to remedy that
                {
                    InvokeDelayed(new MethodInvoker(delegate { this.PerformLayout(); }));
                }
            }

            base.OnHandleCreated(e);
        }

		private void RefreshPanelsStyle()
		{
            if (m_Initializing) return;
			foreach(Control c in this.Controls)
			{
				if(c is TabControlPanel)
					ApplyDefaultPanelStyle(c as TabControlPanel);
			}
		}

		/// <summary>
		/// Use TabControlPanel.Style property to set the background image for each tab panel.
		/// </summary>
		[System.ComponentModel.Browsable(false),System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override Image BackgroundImage
		{
			get{return base.BackgroundImage;}
			set{base.BackgroundImage=value;}
        }

        private void TabStripSizeRecalculated(object sender, EventArgs e)
        {
            //if (tabStrip1.IsMultiLine)
                SyncTabStripSize();
        }

        /// <summary>
        /// Resizes the portion of the control that holds the tabs.
        /// </summary>
        public void SyncTabStripSize()
        {
            if (m_SyncingTabStripSize)
                return;
            m_SyncingTabStripSize = true;
            try
            {
                switch (tabStrip1.Dock)
                {
                    case DockStyle.Left:
                    case DockStyle.Right:
                        tabStrip1.Width = tabStrip1.MinTabStripHeight;
                        break;
                    default:
                        tabStrip1.Height = tabStrip1.MinTabStripHeight;
                        break;
                }
                //this.PerformLayout();
                tabStrip1.SendToBack();
            }
            finally
            {
                m_SyncingTabStripSize = false;
            }
            //this.ResumeLayout();
        }
        private void InvokeDelayed(MethodInvoker method)
        {
            Timer delayedInvokeTimer = new Timer();
            delayedInvokeTimer = new Timer();
            delayedInvokeTimer.Tag = method;
            delayedInvokeTimer.Interval = 10;
            delayedInvokeTimer.Tick += new EventHandler(DelayedInvokeTimerTick);
            delayedInvokeTimer.Start();
        }
        void DelayedInvokeTimerTick(object sender, EventArgs e)
        {
            Timer timer = (Timer)sender;
            MethodInvoker method = (MethodInvoker)timer.Tag;
            timer.Stop();
            timer.Dispose();
            method.Invoke();
        }

        /// <summary>
        /// Creates new tab and tab panel and adds it to the Tabs collection.
        /// </summary>
        /// <param name="tabText">Tab text.</param>
        /// <returns>Reference to newly created TabItem.</returns>
        public TabItem CreateTab(string tabText)
        {
            return CreateTab(tabText, -1);
        }

        /// <summary>
        /// Creates new tab and tab panel and inserts it at specified position inside of Tabs collection.
        /// </summary>
        /// <param name="tabText">Tab text.</param>
        /// <param name="insertAt">Index to insert newly created tab at. -1 will append tab to the end of Tabs collection.</param>
        /// <returns>Reference to newly created TabItem.</returns>
        public TabItem CreateTab(string tabText, int insertAt)
        {
            TabItem tab = new TabItem();
            tab.Text = tabText;
            TabControlPanel panel = new TabControlPanel();
            this.ApplyDefaultPanelStyle(panel);
            tab.AttachedControl = panel;
            panel.TabItem = tab;

            if (insertAt < 0 || insertAt >= tabStrip1.Tabs.Count)
            {
                tabStrip1.Tabs.Add(tab);
                this.Controls.Add(panel);
            }
            else
            {
                tabStrip1.Tabs.Insert(insertAt, tab);
                this.Controls.Add(panel);
            }

            panel.Dock = DockStyle.Fill;
            panel.SendToBack();

            this.RecalcLayout();

            return tab;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Tab | Keys.Control))
            {
                tabStrip1.SelectNextTab(eEventSource.Keyboard, true);
                return true;
            }
            else if (keyData == (Keys.Tab | Keys.Control | Keys.Shift))
            {
                tabStrip1.SelectPreviousTab(eEventSource.Keyboard, true);
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        #endregion

        #region ISupportInitialize
        private bool m_Initializing=false;
		void ISupportInitialize.BeginInit()
		{
            m_Initializing = true;
		}
		void ISupportInitialize.EndInit()
		{
            m_Initializing = false;
			// Apply default panel style
			RefreshPanelsStyle();
            if(m_LoadSelectedTabIndex>=0)
                tabStrip1.SelectedTabIndex = m_LoadSelectedTabIndex;
            m_LoadSelectedTabIndex = -1;
			if(tabStrip1.SelectedTab!=null && tabStrip1.SelectedTab.AttachedControl!=null)
			{
				//if(!this.DesignMode)
					tabStrip1.SelectedTab.AttachedControl.Visible=true;
			}
		}
		#endregion

		#region Properties
        /// <summary>
        /// Gets or sets whether tabs use anti-alias smoothing when painted. Default value is false.
        /// </summary>
        [DefaultValue(false), Browsable(true), Category("Appearance"), Description("Gets or sets whether tabs use anti-aliasing when painted.")]
        public bool AntiAlias
        {
            get { return tabStrip1.AntiAlias; }
            set
            {
                tabStrip1.AntiAlias = value;
            }
        }

		/// <summary>
		/// Gets or sets whether the tab scrolling is animated.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),Description("Indicates whether the tab scrolling is animanted."),Category("Behavior"),DefaultValue(true)]
		public bool Animate
		{
			get{return tabStrip1.Animate;}
			set {tabStrip1.Animate=value;}
		}
		/// <summary>
		/// Gets or sets whether system box that enables scrolling and closing of the tabs is automatically hidden when tab items size does not exceed the size of the control.
		/// </summary>
		[Browsable(true),DefaultValue(true),Category("Behavior"),Description("Indicates whether system box that enables scrolling and closing of the tabs is automatically hidden when tab items size does not exceed the size of the control.")]
		public bool AutoHideSystemBox
		{
			get {return tabStrip1.AutoHideSystemBox;}
			set
			{
				tabStrip1.AutoHideSystemBox=value;
				if(this.DesignMode)
					this.Refresh();
			}
		}
		/// <summary>
		/// Specifes whether end-user can reorder the tabs.
		/// </summary>
		[System.ComponentModel.Browsable(true),DevCoBrowsable(true),System.ComponentModel.Category("Behavior"),System.ComponentModel.Description("Specifes whether end-user can reorder the tabs.")]
		public bool CanReorderTabs
		{
			get
			{
				return tabStrip1.CanReorderTabs;
			}
			set
			{
				tabStrip1.CanReorderTabs=value;
			}
		}

        /// <summary>
        /// Gets or sets whether tab is automatically closed when close button is clicked. Closing the tab will remove tab being closed from Tabs collection
        /// and it will remove the panel as well. Default value is false which means that tab will not be closed and you should handle TabClose event to
        /// perform desired action as result of user closing the tab.
        /// </summary>
        [Browsable(true), DefaultValue(false), Category("Close Button"), Description("Indicates whether tab is automatically closed when close button is clicked.")]
        public bool AutoCloseTabs
        {
            get { return m_AutoCloseTabs; }
            set { m_AutoCloseTabs = value; }
        }

		/// <summary>
		/// Gets or sets whether the Close button that closes the active tab is visible on system box.
		/// </summary>
        [Browsable(true), DevCoBrowsable(true), Category("Close Button"), Description("Indicates whether the Close button that closes the active tab is visible."), DefaultValue(false)]
		public bool CloseButtonVisible
		{
			get
			{
				return tabStrip1.CloseButtonVisible;
			}
			set
			{
				tabStrip1.CloseButtonVisible=value;
			}
		}

        /// <summary>
        /// Gets or sets whether close button is visible on each tab instead of in system box.
        /// </summary>
        [Browsable(true), DefaultValue(false), DevCoBrowsable(true), Category("Close Button"), Description("Indicates whether close button is visible on each tab instead of in system box.")]
        public bool CloseButtonOnTabsVisible
        {
            get
            {
                return tabStrip1.CloseButtonOnTabsVisible;
            }
            set
            {
                tabStrip1.CloseButtonOnTabsVisible = value;
                this.Refresh();
            }
        }

        /// <summary>
        /// Gets or sets whether close button on tabs when visible is displayed for every tab state. Default value is true. When set to false
        /// the close button will be displayed only for selected and tab that mouse is currently over.
        /// </summary>
        [Browsable(true), DefaultValue(true), DevCoBrowsable(true), Category("Close Button"), Description("Indicates whether close button on tabs when visible is displayed for every tab state.")]
        public bool CloseButtonOnTabsAlwaysDisplayed
        {
            get
            {
                return tabStrip1.CloseButtonOnTabsAlwaysDisplayed;
            }
            set
            {
                tabStrip1.CloseButtonOnTabsAlwaysDisplayed = value;
            }
        }

        /// <summary>
        /// Gets or sets the position of the close button displayed on each tab. Default value is Left.
        /// </summary>
        [Browsable(true), DefaultValue(eTabCloseButtonPosition.Left), DevCoBrowsable(true), Category("Close Button"), Description("Indicates position of the close button displayed on each tab.")]
        public eTabCloseButtonPosition CloseButtonPosition
        {
            get { return tabStrip1.CloseButtonPosition; }
            set
            {
                tabStrip1.CloseButtonPosition = value;
            }
        }


        /// <summary>
        /// Gets or sets custom image that is used on tabs as Close button that allows user to close the tab.
        /// Use TabCloseButtonHot property to specify image that is used when mouse is over the close button. Note that image size must
        /// be same for both images.
        /// Default value is null
        /// which means that internal representation of close button is used.
        /// </summary>
        [Browsable(true), DefaultValue(null), Category("Close Button"), Description("Indicates custom image that is used on tabs as Close button that allows user to close the tab.")]
        public Image TabCloseButtonNormal
        {
            get { return tabStrip1.TabCloseButtonNormal; }
            set {tabStrip1.TabCloseButtonNormal = value; }
        }

        /// <summary>
        /// Gets or sets custom image that is used on tabs as Close button whem mouse is over the close button.
        /// To use this property you must set TabCloseButtonNormal as well. Note that image size for both images must be same.
        /// Default value is null which means that internal representation of close button is used.
        /// </summary>
        [Browsable(true), DefaultValue(null), Category("Close Button"), Description("Indicates custom image that is used on tabs as Close button whem mouse is over the close button.")]
        public Image TabCloseButtonHot
        {
            get { return tabStrip1.TabCloseButtonHot; }
            set {tabStrip1.TabCloseButtonHot = value;}
        }

		/// <summary>
		/// Gets or sets whether only selected tab is displaying it's text.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),Category("Behavior"),Description("Specifes whether only selected tab is displaying it's text."),DefaultValue(false)]
		public bool DisplaySelectedTextOnly
		{
			get {return tabStrip1.DisplaySelectedTextOnly;}
			set
			{
				tabStrip1.DisplaySelectedTextOnly=value;
			}
		}

		/// <summary>
		/// Gets or sets the image list used by tab items.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),Category("Appearance"),DefaultValue(null)]
		public System.Windows.Forms.ImageList ImageList
		{
			get
			{
				return tabStrip1.ImageList;
			}
			set
			{
				tabStrip1.ImageList=value;
			}
		}
		/// <summary>
		/// Gets or sets scrolling offset of the first tab. You can use this property to programmatically scroll the tab strip.
		/// </summary>
		[Browsable(false),DevCoBrowsable(false),DefaultValue(0),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int ScrollOffset
		{
			get {return tabStrip1.ScrollOffset;}
			set {tabStrip1.ScrollOffset=value;}
		}
		/// <summary>
		///     Selects previous visible tab. Returns true if previous tab was found for selection.
		/// </summary>
		public bool SelectPreviousTab()
		{
			return tabStrip1.SelectPreviousTab();
		}
		/// <summary>
		///     Selects next visible tab. Returns true if next tab was found for selection.
		/// </summary>
		public bool SelectNextTab()
		{
			return tabStrip1.SelectNextTab();
		}
		/// <summary>
		/// Gets or sets Tab Control style. Theme style is supported only on themed OS and only for bottom or top aligned tabs.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),Category("Appearance"),Description("Specifies the Tab Control style."),DefaultValue(eTabStripStyle.OneNote)]
		public eTabStripStyle Style
		{
			get
			{
				return tabStrip1.Style;
			}
			set
			{
				tabStrip1.Style=value;
				this.SyncTabStripSize();
				this.RefreshPanelsStyle();
				if(this.DesignMode)
					this.Refresh();
			}
		}

        protected override void OnParentChanged(EventArgs e)
        {
            if (this.Style == eTabStripStyle.Office2007Dock || this.Style == eTabStripStyle.Office2007Document)
            {
                tabStrip1.Style = this.Style;
                this.RefreshPanelsStyle();
            }
            base.OnParentChanged(e);
        }

		/// <summary>
		/// Specifies whether tab is drawn using Themes when running on OS that supports themes like Windows XP.
		/// </summary>
		[System.ComponentModel.Browsable(true),DevCoBrowsable(true),System.ComponentModel.DefaultValue(false),System.ComponentModel.Category("Appearance"),System.ComponentModel.Description("Specifies whether tab is drawn using Themes when running on OS that supports themes like Windows XP.")]
		public virtual bool ThemeAware
		{
			get
			{
				return tabStrip1.ThemeAware;
			}
			set
			{
				tabStrip1.ThemeAware=value;
				this.RefreshPanelsStyle();
				if(this.DesignMode)
					this.Refresh();
			}
		}

		/// <summary>
		/// Gets or sets whether tabs are scrolled continuously while mouse is pressed over the scroll tab button.
		/// </summary>
		[System.ComponentModel.Browsable(true),DevCoBrowsable(true),System.ComponentModel.DefaultValue(false),System.ComponentModel.Category("Behavior"),System.ComponentModel.Description("Indicates whether tabs are scrolled continuously while mouse is pressed over the scroll tab button.")]
		public virtual bool TabScrollAutoRepeat
		{
			get
			{
				return tabStrip1.TabScrollAutoRepeat;
			}
			set
			{
				tabStrip1.TabScrollAutoRepeat=value;                
			}
		}

		/// <summary>
		/// Gets or sets the auto-repeat interval for the tab scrolling while mouse button is kept pressed over the scroll tab button.
		/// </summary>
		[System.ComponentModel.Browsable(true),DevCoBrowsable(true),System.ComponentModel.DefaultValue(300),System.ComponentModel.Category("Behavior"),System.ComponentModel.Description("Indicates auto-repeat interval for the tab scrolling while mouse button is kept pressed over the scroll tab button.")]
		public virtual int TabScrollRepeatInterval
		{
			get
			{
				return tabStrip1.TabScrollRepeatInterval;
			}
			set
			{
				tabStrip1.TabScrollRepeatInterval=value;                
			}
		}

		/// <summary>
		/// Gets or sets Tab Color Scheme.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),Category("Style"),Description("Gets or sets Tab Color Scheme."),DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public TabColorScheme ColorScheme
		{
			get {return tabStrip1.ColorScheme;}
			set
			{
				tabStrip1.ColorScheme=value;
				if(this.DesignMode)
					this.Refresh();
			}
		}
		[Browsable(false),EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeColorScheme()
		{
			return tabStrip1.ColorScheme.SchemeChanged;
		}
		/// <summary>
		/// Resets color scheme to default value.
		/// </summary>
		public void ResetColorScheme()
		{
			tabStrip1.ResetColorScheme();
			if(this.DesignMode)
				this.Refresh();
		}

		/// <summary>
		/// Gets or sets the tab alignment within the Tab-Strip control.
		/// </summary>
		[DefaultValue(eTabStripAlignment.Top),Browsable(true),DevCoBrowsable(true),Category("Appearance"),Description("Indicates the tab alignment within the Tab-Strip control.")]
		public eTabStripAlignment TabAlignment
		{
			get {return tabStrip1.TabAlignment;}
			set
			{
				if(tabStrip1.TabAlignment==value)
					return;
				this.SuspendLayout();
				try
				{
					switch(value)
					{
						case eTabStripAlignment.Top:
							tabStrip1.Dock=DockStyle.Top;
							break;
						case eTabStripAlignment.Bottom:
							tabStrip1.Dock=DockStyle.Bottom;
							break;
						case eTabStripAlignment.Left:
							tabStrip1.Dock=DockStyle.Left;
							break;
						case eTabStripAlignment.Right:
							tabStrip1.Dock=DockStyle.Right;
							break;
					}
					tabStrip1.TabAlignment=value;
					RefreshPanelsStyle();
				}
				finally
				{
					this.ResumeLayout(true);
				}
                if (this.Visible)
                    SyncTabStripSize();
			}
		}

		/// <summary>
		/// Gets or sets the selected tab.
		/// </summary>
		[Browsable(false),DevCoBrowsable(false),Category("Behavior"),Description("Indicates selected tab."),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public TabItem SelectedTab
		{
			get
			{
				return tabStrip1.SelectedTab;
			}
			set
			{
				tabStrip1.SelectedTab=value;
			}
		}

		/// <summary>
		/// Gets or sets the selected tab Font
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),Category("Style"),DefaultValue(null),Description("Gets or sets the selected tab Font")]
		public System.Drawing.Font SelectedTabFont
		{
			get
			{
				return tabStrip1.SelectedTabFont;
			}
			set
			{
				tabStrip1.SelectedTabFont=value;
			}
		}
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public void ResetSelectedTabFont()
		{
			tabStrip1.ResetSelectedTabFont();
		}

		/// <summary>
		/// Gets or sets whether focus rectangle is displayed on the tab when tab has input focus.
		/// </summary>
		[Browsable(true),DefaultValue(true),Category("Behavior"),Description("Indicates whether focus rectangle is displayed on the tab when tab has input focus.")]
		public bool ShowFocusRectangle
		{
			get {return tabStrip1.ShowFocusRectangle;}
			set {tabStrip1.ShowFocusRectangle=value;}
		}

        /// <summary>
        /// Gets or sets whether keyboard navigation using Left and Right arrow keys to select tabs is enabled. Default value is true.
        /// </summary>
        [Category("Behavior"), DefaultValue(true), Description("Indicates whether keyboard navigation using Left and Right arrow keys to select tabs is enabled.")]
        public bool KeyboardNavigationEnabled
        {
            get { return tabStrip1.KeyboardNavigationEnabled; }
            set
            {
                tabStrip1.KeyboardNavigationEnabled = value;
            }
        }

		/// <summary>
		/// Gets or sets the index of the selected tab.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),Category("Behavior"),Description("Indicates the index of selected tab.")]
		public int SelectedTabIndex
		{
            get { return tabStrip1.SelectedTabIndex; }
			set
			{
                if (m_Initializing)
                    m_LoadSelectedTabIndex = value;
                else
                    tabStrip1.SelectedTabIndex = value;
			}
		}

		/// <summary>
		/// Gets or sets selected tab panel.
		/// </summary>
		[Browsable(false),DevCoBrowsable(false),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public TabControlPanel SelectedPanel
		{
			get
			{
				if(tabStrip1.SelectedTab!=null)
					return tabStrip1.SelectedTab.AttachedControl as TabControlPanel;
				return null;
			}
			set
			{
				if(value!=null && this.Controls.Contains(value) && value.TabItem!=null && this.Tabs.Contains(value.TabItem))
				{
					tabStrip1.SelectedTab=value.TabItem;
				}
			}
		}

		/// <summary>
		/// Gets or sets the type of the tab layout.
		/// </summary>
		[Browsable(true),DefaultValue(eTabLayoutType.FitContainer),Category("Appearance"),Description("Indicates the type of the tab layout.")]
		public eTabLayoutType TabLayoutType
		{
			get {return tabStrip1.TabLayoutType;}
			set {tabStrip1.TabLayoutType=value;}
		}

        /// <summary>
        /// Gets or sets the fixed tab size in pixels. Either Height or Width can be set or both.
        /// Value of 0 indicates that size is automatically calculated which is
        /// default behavior.
        /// </summary>
        [Browsable(true), Category("Appearance"), Description("Gets or sets the fixed tab size in pixels. Either Height or Width can be set or both.")]
        public Size FixedTabSize
        {
            get { return tabStrip1.FixedTabSize; }
            set
            {
                tabStrip1.FixedTabSize = value;
                this.RecalcLayout();
            }
        }
        /// <summary>
        /// Memeber used by Windows Forms designer.
        /// </summary>
        /// <returns>true if property should be serialized.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeFixedTabSize()
        {
            return tabStrip1.ShouldSerializeFixedTabSize();
        }
        /// <summary>
        /// Memeber used by Windows Forms designer to reset property to default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetFixedTabSize()
        {
            TypeDescriptor.GetProperties(this)["FixedTabSize"].SetValue(this, Size.Empty);
        }
		#endregion
	}
}
