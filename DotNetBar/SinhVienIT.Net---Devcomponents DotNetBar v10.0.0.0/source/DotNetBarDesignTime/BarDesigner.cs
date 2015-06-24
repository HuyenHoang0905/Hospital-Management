using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.ComponentModel.Design.Serialization;
using System.CodeDom;
using System.Drawing;
using System.Collections;
using System.Windows.Forms.Design;
using System.Drawing.Design;

namespace DevComponents.DotNetBar.Design
{
	/// <summary>
	/// Represents Windows Forms designer for Bar control.
	/// </summary>
	public class BarDesigner:BarBaseControlDesigner
	{
		#region Internal Implementation
		private int m_MouseDownSelectedTabIndex=-1;
		private bool m_IsDocking=false;
		private Form m_OutlineForm=null;
		private DockSiteInfo m_DockInfo;
		private bool m_DragDrop=false;

		public BarDesigner():base()
		{
			m_DockInfo=new DockSiteInfo();
			this.EnableItemDragDrop=true;
		}

        public override void Initialize(IComponent component)
        {
            base.Initialize(component);
            Bar bar = component as Bar;
            if (bar != null) bar.SetDesignMode(true);
			IDesignerHost ds=GetService(typeof(IDesignerHost)) as IDesignerHost;
			if(ds!=null)
				ds.LoadComplete += new EventHandler(this.DesignerLoadComplete);
        }

		public override DesignerVerbCollection Verbs 
		{
			get 
			{
				Bar bar=this.Control as Bar;
				DesignerVerb[] verbs=null;
				if(this.IsDockableWindow)
				{
					
                    verbs = new DesignerVerb[]
					{
						new DesignerVerb("Create Dock Tab", new EventHandler(CreateDocument))};
				}
				else
				{
					verbs = new DesignerVerb[]
						{
							new DesignerVerb("Add Button", new EventHandler(CreateButton)),
							new DesignerVerb("Add Text Box", new EventHandler(CreateTextBox)),
							new DesignerVerb("Add Combo Box", new EventHandler(CreateComboBox)),
							new DesignerVerb("Add Label", new EventHandler(CreateLabel)),
                            new DesignerVerb("Add Color Picker", new EventHandler(CreateColorPicker)),
                            new DesignerVerb("Add Container", new EventHandler(CreateContainer)),
							new DesignerVerb("Add Progress-Bar", new EventHandler(CreateProgressBar)),
                            new DesignerVerb("Add Circular Progress", new EventHandler(CreateCircularProgressItem)),
                            new DesignerVerb("Add Micro-Chart", new EventHandler(CreateMicroChart)),
                            new DesignerVerb("Add Switch Button", new EventHandler(CreateSwitch)),
							new DesignerVerb("Add Color Picker", new EventHandler(CreateColorPicker)),
                            new DesignerVerb("Add Check Box", new EventHandler(CreateCheckBox)),
                            new DesignerVerb("Add Slider", new EventHandler(CreateSliderItem)),
                            new DesignerVerb("Add Customize Item", new EventHandler(CreateCustomizeItem)),
                            new DesignerVerb("Add MDI Window List Item", new EventHandler(CreateMdiWindowList)),
                            new DesignerVerb("Add Rating Item", new EventHandler(CreateRatingItem)),
                            new DesignerVerb("Add Control Container", new EventHandler(CreateControlContainer))
							};
				}
				
				return new DesignerVerbCollection(verbs);
			}
		}

        protected override void PreFilterProperties(System.Collections.IDictionary properties)
        {
            base.PreFilterProperties(properties);

            properties["AutoHide"] = TypeDescriptor.CreateProperty(
                this.GetType(),
                "AutoHide",
                typeof(bool),
                new Attribute[] { new BrowsableAttribute(true), new DefaultValueAttribute(false), 
                    new CategoryAttribute("Auto-Hide"),
                    new DescriptionAttribute("Indicates whether Bar is in auto-hide state. Applies to non-document dockable bars only.") });

            properties["CanCustomize"] = TypeDescriptor.CreateProperty(
                this.GetType(),
                "CanCustomize",
                typeof(bool),
                new Attribute[] { new BrowsableAttribute(true), new DefaultValueAttribute(true), 
                    new CategoryAttribute("Behavior"),
                    new DescriptionAttribute("Gets or sets whether items on the Bar can be customized.") });

            properties["Style"] = TypeDescriptor.CreateProperty(
                this.GetType(),
                "Style",
                typeof(eDotNetBarStyle),
                new Attribute[] { new BrowsableAttribute(true), new DefaultValueAttribute(eDotNetBarStyle.OfficeXP), 
                    new CategoryAttribute("Appearance"),
                    new DescriptionAttribute("Gets or sets the control style.") });
        }

        /// <summary>
        /// Gets/Sets the visual style of the Bar.
        /// </summary>
        [Browsable(true), Category("Appearance"), Description("Specifies the visual style of the Bar."), DefaultValue(eDotNetBarStyle.OfficeXP)]
        public eDotNetBarStyle Style
        {
            get
            {
                Bar b = this.Control as Bar;
                return b.Style;
            }
            set
            {
                Bar b = this.Control as Bar;
                bool isChanged = (b.Style != value);
                b.Style = value;
                if (isChanged && b.Owner is DotNetBarManager)
                {
                    IDesignerHost ds = GetService(typeof(IDesignerHost)) as IDesignerHost;
                    if (ds != null && !ds.Loading)
                    {
                        DotNetBarManager dnb = b.Owner as DotNetBarManager;
                        TypeDescriptor.GetProperties(dnb)["Style"].SetValue(dnb, value);
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets whether items on the Bar can be customized.
        /// </summary>
        [Browsable(true), Category("Behavior"), Description("Gets or sets whether items on the Bar can be customized."), DefaultValue(true)]
        public bool CanCustomize
        {
            get
            {
                return (bool)ShadowProperties["CanCustomize"];
            }
            set
            {
                ShadowProperties["CanCustomize"] = value;
            }
        }

		private void DesignerLoadComplete(object sender, EventArgs e)
		{
			if(this.IsDockableWindow)
			{
				Bar bar= this.Control as Bar;
				DockContainerItem dc = bar.SelectedDockContainerItem;
				if(dc!=null && dc.Control!=null)
				{
					if(bar.Controls.IndexOf(dc.Control)>0)
						bar.Controls.SetChildIndex(dc.Control, 0);
				}
			}

			IDesignerHost ds=GetService(typeof(IDesignerHost)) as IDesignerHost;
			if(ds!=null)
				ds.LoadComplete -= new EventHandler(this.DesignerLoadComplete);
		}

        /// <summary>
        /// Indicates whether Bar is in auto-hide state.
        /// </summary>
        [Browsable(true), DefaultValue(false), Description("Indicates whether Bar is in auto-hide state. Applies to non-document dockable bars only.")]
        public bool AutoHide
        {
            set
            {
                ShadowProperties["AutoHide"] = value;
            }
            get
            {
                return (bool)ShadowProperties["AutoHide"];
            }
        }
        //public override void DoDefaultAction()
        //{
        //    ISelectionService selection = (ISelectionService) this.GetService(typeof(ISelectionService));
        //    if(!this.IsDockableWindow && selection!=null && selection.PrimarySelection==this.Control)
        //        OpenDesigner();
        //    else
        //        base.DoDefaultAction();
        //}

        //private void OpenDesigner()
        //{
        //    Bar bar=this.Control as Bar;
        //    if(bar!=null)	
        //    {
        //        DotNetBarManagerDesigner.OpenDesignerEditor(null,bar,this);
        //    }
        //}

#if FRAMEWORK20
        public override void InitializeNewComponent(IDictionary defaultValues)
        {
            base.InitializeNewComponent(defaultValues);
            SetDesignTimeDefaults();
        }
#else
		public override void OnSetComponentDefaults()
		{
			base.OnSetComponentDefaults();
			SetDesignTimeDefaults();
		}
#endif

        private void SetDesignTimeDefaults()
        {
            Bar bar = this.Component as Bar;
            if (bar == null)
                return;
            bar.Style = eDotNetBarStyle.StyleManagerControlled;
            bar.AntiAlias = true;
        }

		protected override BaseItem GetItemContainer()
		{
			Bar bar=this.Control as Bar;
			if(bar!=null)
				return bar.ItemsContainer;
			return null;
		}

		protected override void RecalcLayout()
		{
			Bar bar=this.GetItemContainerControl() as Bar;
			if(bar!=null)
				bar.RecalcLayout();
		}

		protected override void OnSubItemsChanging()
		{
			base.OnSubItemsChanging();
			IComponentChangeService change=this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
			if(change!=null)
			{
				Bar bar=this.GetItemContainerControl() as Bar;
				change.OnComponentChanging(this.Component,TypeDescriptor.GetProperties(bar).Find("Items",true));
			}
		}

		protected override void OnSubItemsChanged()
		{
			base.OnSubItemsChanged();
			IComponentChangeService change=this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
			if(change!=null)
			{
				Bar bar=this.GetItemContainerControl() as Bar;
				change.OnComponentChanged(this.Component,TypeDescriptor.GetProperties(bar).Find("Items",true),null,null);
			}
		}

		
		#endregion

		#region Support For Dockable Windows

		public override SelectionRules SelectionRules
		{
			get
			{
				if(this.IsDockableWindow || this.Control.Parent is DockSite)
					return (SelectionRules.Locked);
				return base.SelectionRules;
			}
		}

		public override bool CanParent(Control control)
		{
			if(control.Contains(this.Control) || control is Bar)
				return false;
			if(this.IsDockableWindow && !(control is PanelDockContainer))
				return false;
			return base.CanParent(control);
		}

		protected override bool IsDockableWindow
		{
			get
			{
				Bar bar=this.Control as Bar;
				if(bar!=null && bar.LayoutType==eLayoutType.DockContainer)
					return true;
				return false;
			}
		}

        private bool IsDockableToolbar
        {
            get
            {
                Bar bar = this.Control as Bar;
                if (bar != null && bar.LayoutType == eLayoutType.Toolbar && bar.Parent is DockSite)
                    return true;
                return false;
            }
        }

        private bool IsDocumentDock
        {
            get
            {
                Bar bar = this.Control as Bar;
                if (bar == null || bar.LayoutType != eLayoutType.DockContainer)
                    return false;
                if (bar.Parent is DockSite && bar.Parent.Dock == DockStyle.Fill)
                    return true;
                return false;
            }
        }

		private void SelectDockTab(int index)
		{
			Bar bar=this.Control as Bar;
			if(bar==null) return;

			IComponentChangeService cc=(IComponentChangeService)GetService(typeof(IComponentChangeService));
			if(cc!=null)
				cc.OnComponentChanging(bar,TypeDescriptor.GetProperties(typeof(Bar))["SelectedDockTab"]);
			bar.SelectedDockTab=index;
			if(cc!=null)
				cc.OnComponentChanged(bar,TypeDescriptor.GetProperties(typeof(Bar))["SelectedDockTab"],null,null);
			if(bar.SelectedDockTab>=0)
			{
#if FRAMEWORK20
                SelectComponent(bar.Items[bar.SelectedDockTab], SelectionTypes.Primary);
#else
                SelectComponent(bar.Items[bar.SelectedDockTab],SelectionTypes.MouseDown);
#endif
                DockContainerItem dock=bar.Items[bar.SelectedDockTab] as DockContainerItem;
				if(dock!=null && dock.Control!=null)
				{
					if(cc!=null)
						cc.OnComponentChanging(bar,TypeDescriptor.GetProperties(typeof(Bar))["Controls"]);
					dock.Control.BringToFront();
					if(cc!=null)
						cc.OnComponentChanged(bar,TypeDescriptor.GetProperties(typeof(Bar))["Controls"],null,null);
				}
			}
		}

		protected override eDotNetBarStyle InternalStyle
		{
			get
			{
				Bar bar=this.Control as Bar;
				if(bar!=null)
					return bar.Style;
				return base.InternalStyle;;
			}
		}

		protected override void OtherComponentRemoving(object sender, ComponentEventArgs e)
		{
			Bar bar=this.Control as Bar;
			if(e.Component is Control)
			{
				BaseItem item=GetControlItem(e.Component as Control);
				if(item!=null && item.Parent!=null && item.Parent.SubItems.Contains(item))
				{
					if(item is DockContainerItem)
						((DockContainerItem)item).Control=null;
                    else if(item is ControlContainerItem)
                        ((ControlContainerItem)item).Control = null;
                    if (item.Parent != null)
                        item.Parent.SubItems.Remove(item);
					DestroySubItems(item);
					this.RecalcLayout();
					if(bar!=null && bar.Items.Count>0)
						SelectDockTab(0);
				}
			}
			else if(!m_InternalRemoving && bar!=null && e.Component is DockContainerItem && bar.Items.Contains((BaseItem)e.Component))
			{
				// Throw exception to stop removing the last dock container item.
				if(bar.VisibleItemCount==1)
					throw new InvalidOperationException("Cannot delete last DockContainerItem object. Select and Delete Bar object instead");
			}
			base.OtherComponentRemoving(sender,e);
		}

		protected override bool OnMouseDown(ref Message m, MouseButtons mb)
		{
			Bar bar=this.Control as Bar;
			if(!this.IsDockableWindow || bar==null)
				return base.OnMouseDown(ref m, mb);
			Point p=Control.MousePosition;
			int i=GetTabAt(p.X,p.Y);
			if(i>=0 && m.Msg==WinApi.WM_RBUTTONDOWN)
			{
				ISelectionService selection = (ISelectionService) this.GetService(typeof(ISelectionService));
				if(selection!=null && selection.PrimarySelection!=bar.Items[i])
				{
					ArrayList arr=new ArrayList(1);
					arr.Add(bar.Items[i]);
#if FRAMEWORK20
                    selection.SetSelectedComponents(arr, SelectionTypes.Primary);
#else
                    selection.SetSelectedComponents(arr,SelectionTypes.MouseDown);
#endif

                    SelectDockTab(i);
					this.OnContextMenu(System.Windows.Forms.Control.MousePosition.X,System.Windows.Forms.Control.MousePosition.Y);
					return true;
				}
			}


			return base.OnMouseDown(ref m, mb);
		}

		protected override void OnMouseDragBegin(int x, int y)
		{
			Bar bar=this.Control as Bar;
			m_DragDrop=false;

			if(bar!=null && this.IsDockableWindow)
			{
				m_MouseDownSelectedTabIndex=GetTabAt(x,y);
				if(m_MouseDownSelectedTabIndex!=-1)
				{
					if(bar.SelectedDockTab!=m_MouseDownSelectedTabIndex)
						SelectDockTab(m_MouseDownSelectedTabIndex);
					m_DragDrop=true;
				}
				else if(IsInTabSystemBox(x,y))
				{
					MouseDownTabSystemBox(x,y);
				}
				else if(IsCaptionGrabHandle(bar))
				{
					Point clientPos=bar.PointToClient(new Point(x,y));
					if(bar.GrabHandleRect.Contains(clientPos))
						m_DragDrop=true;
				}
			}
            else if (bar != null && this.IsDockableToolbar && bar.GrabHandleStyle != eGrabHandleStyle.None)
            {
                Point clientPos = bar.PointToClient(new Point(x, y));
                if (bar.GrabHandleRect.Contains(clientPos))
                    m_DragDrop = true;
            }

			base.OnMouseDragBegin(x,y);
			
			if(m_MouseDownSelectedTabIndex!=-1)
			{
				ISelectionService selection = (ISelectionService) this.GetService(typeof(ISelectionService));
				if(selection!=null && selection.PrimarySelection!=bar.Items[m_MouseDownSelectedTabIndex])
				{
					ArrayList arr=new ArrayList(1);
					arr.Add(bar.Items[m_MouseDownSelectedTabIndex]);
#if FRAMEWORK20
                    selection.SetSelectedComponents(arr, SelectionTypes.Primary);
#else
                    selection.SetSelectedComponents(arr,SelectionTypes.Click);
#endif
                }
			}

			if(m_DragDrop)
			{
				bar.Capture = true;
				if (IsDocumentDock)
				{
					WinApi.RECT rect = new WinApi.RECT(0, 0, 0, 0);
					WinApi.GetWindowRect(bar.Parent.Handle, ref rect);
					Rectangle r = Rectangle.FromLTRB(rect.Left, rect.Top, rect.Right, rect.Bottom);
					Cursor.Clip = r;
				}
				#if !FRAMEWORK20
				else
				{
					Cursor.Clip = Rectangle.Empty;
				}
				#endif
			}
		}

		private bool IsCaptionGrabHandle(Bar bar)
		{
			return (bar.GrabHandleStyle==eGrabHandleStyle.Caption || bar.GrabHandleStyle==eGrabHandleStyle.CaptionTaskPane);
		}

		protected override void OnMouseDragMove(int x, int y)
		{
			if(!this.IsDockableWindow && !this.IsDockableToolbar || !m_DragDrop)
			{
				base.OnMouseDragMove(x,y);
				return;
			}

			Point screenPos=new Point(x,y);
			
			Bar bar=this.Control as Bar;
			if(bar==null) return;

            if (this.IsDockableToolbar)
            {
                IOwnerBarSupport ownerDock = bar.Owner as IOwnerBarSupport;
                m_DockInfo = ownerDock.GetDockInfo(bar, screenPos.X, screenPos.Y);
                if (m_DockInfo.objDockSite != null)
                {
                    IComponentChangeService cc = this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
                    Control oldParent = null;
                    bool offsetChange = false;
                    if (m_DockInfo.objDockSite != bar.Parent)
                    {
                        oldParent = bar.Parent;
                        cc.OnComponentChanging(bar.Parent, null);
                        cc.OnComponentChanging(bar, TypeDescriptor.GetProperties(bar)["DockSide"]);
                    }
                    cc.OnComponentChanging(m_DockInfo.objDockSite, null);

                    if (m_DockInfo.DockOffset != bar.DockOffset)
                    {
                        offsetChange = true;
                        cc.OnComponentChanging(bar, TypeDescriptor.GetProperties(bar)["DockOffset"]);
                        cc.OnComponentChanging(bar, TypeDescriptor.GetProperties(bar)["Location"]);
                    }

                    bar.DockingHandler(m_DockInfo, screenPos);

                    if (oldParent != null)
                    {
                        cc.OnComponentChanged(bar, TypeDescriptor.GetProperties(bar)["DockSide"], null, null);
                        cc.OnComponentChanged(oldParent, null, null, null);
                    }

                    cc.OnComponentChanged(m_DockInfo.objDockSite, null, null, null);
                    if (offsetChange)
                    {
                        cc.OnComponentChanged(bar, TypeDescriptor.GetProperties(bar)["DockOffset"], null, null);
                        cc.OnComponentChanged(bar, TypeDescriptor.GetProperties(bar)["Location"], null, null);
                    }
                }
                bar.Refresh();
                return;
            }

            Point tabPos = Point.Empty;
            if (bar.DockTabControl != null) tabPos = bar.DockTabControl.PointToClient(screenPos);

			if(bar.DockTabControl!=null && bar.DockTabControl.ClientRectangle.Contains(tabPos))
			{
				if(m_IsDocking)
				{
					EndBarOwnerDocking(bar);
					m_IsDocking=false;
					m_DockInfo=new DockSiteInfo();
				}
				MouseEventArgs e=new MouseEventArgs(MouseButtons.Left,0,tabPos.X,tabPos.Y,0);
				bar.DockTabControl.InternalOnMouseMove(e);
			}
			else
			{
				m_IsDocking=true;
				IOwnerBarSupport ownerDock=bar.Owner as IOwnerBarSupport;
				m_DockInfo=ownerDock.GetDockInfo(bar,screenPos.X,screenPos.Y);
				if(m_DockInfo.objDockSite==null)
				{
					if(m_OutlineForm!=null)
						m_OutlineForm.Hide();
				}
				else
				{
					Rectangle r=m_DockInfo.objDockSite.GetBarDockRectangle(bar,ref m_DockInfo);
					if(!r.IsEmpty)
					{
						if(m_OutlineForm==null)
							m_OutlineForm=bar.CreateOutlineForm();
                        WinApi.SetWindowPos(m_OutlineForm.Handle, WinApi.HWND_TOP, r.X, r.Y, r.Width, r.Height, WinApi.SWP_SHOWWINDOW | WinApi.SWP_NOACTIVATE);
					}
					else if(m_OutlineForm!=null)
						m_OutlineForm.Hide();
				}
			}
		}

		protected override void OnMouseDragEnd(bool cancel)
		{
			if(!this.IsDockableWindow)
			{
				base.OnMouseDragEnd(cancel);
				return;
			}

			this.Control.Capture = false;
			Cursor.Clip = Rectangle.Empty;
			
			Bar bar=this.Control as Bar;

			if(cancel || bar==null || !m_DragDrop)
			{
				base.OnMouseDragEnd(cancel);
				return;
			}

			m_DragDrop=false;

			IDesignerHost designerHost=this.GetService(typeof(IDesignerHost)) as IDesignerHost;
			if(designerHost==null)
			{
				EndBarOwnerDocking(bar);
				m_IsDocking=false;
				m_MouseDownSelectedTabIndex=-1;
				base.OnMouseDragEnd(cancel);
				return;
			}

			if(m_IsDocking)
			{
				EndBarOwnerDocking(bar);
				// Moves and docks the selected DockContainerItem or bar
				Bar referenceBar=m_DockInfo.MouseOverBar;
				if(m_DockInfo.MouseOverDockSide!=eDockSide.None && m_DockInfo.MouseOverDockSide!=eDockSide.Document && (referenceBar!=bar || m_MouseDownSelectedTabIndex!=-1 && bar.VisibleItemCount>1) ||
                    m_DockInfo.DockSide != DockStyle.None && m_DockInfo.MouseOverDockSide != eDockSide.Document)
				{
					DesignerTransaction trans=designerHost.CreateTransaction("DotNetBar Docking");
					IComponentChangeService cc=(IComponentChangeService)GetService(typeof(IComponentChangeService));
					try
					{
						Bar newBar=null;
                        DockSite newDockSite = m_DockInfo.objDockSite;
                        DockSite oldDockSite = bar.Parent as DockSite;

                        if (m_MouseDownSelectedTabIndex != -1 && bar.VisibleItemCount > 1)
                        {
                            Bar copy = designerHost.CreateComponent(typeof(Bar)) as Bar;
                            newBar = BarFunctions.CreateDuplicateDockBar(bar, copy);
                        }
                        else
                            newBar = bar;

						cc.OnComponentChanging(bar,TypeDescriptor.GetProperties(typeof(Bar))["SelectedDockTab"]);
						
						if(m_MouseDownSelectedTabIndex!=-1 && bar.VisibleItemCount>1)
						{
							DockContainerItem item=bar.Items[bar.SelectedDockTab] as DockContainerItem;
							cc.OnComponentChanging(bar,TypeDescriptor.GetProperties(typeof(Bar))["Controls"]);
							cc.OnComponentChanging(bar,TypeDescriptor.GetProperties(typeof(Bar))["Items"]);
							bar.Items.Remove(item);
							cc.OnComponentChanged(bar,TypeDescriptor.GetProperties(typeof(Bar))["Items"],null,null);
							cc.OnComponentChanged(bar,TypeDescriptor.GetProperties(typeof(Bar))["Controls"],null,null);

							cc.OnComponentChanging(newBar,TypeDescriptor.GetProperties(typeof(Bar))["Controls"]);
							cc.OnComponentChanging(newBar,TypeDescriptor.GetProperties(typeof(Bar))["Items"]);
							newBar.Items.Add(item);
							cc.OnComponentChanged(newBar,TypeDescriptor.GetProperties(typeof(Bar))["Items"],null,null);
							cc.OnComponentChanged(newBar,TypeDescriptor.GetProperties(typeof(Bar))["Controls"],null,null);							
						}
						
						cc.OnComponentChanging(newDockSite,TypeDescriptor.GetProperties(typeof(DockSite))["Controls"]);
						cc.OnComponentChanging(newBar,null);
                        if(referenceBar!=null)
						    cc.OnComponentChanging(referenceBar,null);
						cc.OnComponentChanging(newDockSite,TypeDescriptor.GetProperties(typeof(DockSite))["DocumentDockContainer"]);
                        if (newDockSite != oldDockSite && oldDockSite != null)
                        {
                            cc.OnComponentChanging(oldDockSite, TypeDescriptor.GetProperties(typeof(DockSite))["Controls"]);
                            cc.OnComponentChanging(oldDockSite, TypeDescriptor.GetProperties(typeof(DockSite))["DocumentDockContainer"]);
                        }

                        m_DockInfo.MouseOverBar = referenceBar;
                        newBar.DockDocumentManager(m_DockInfo);
						//newDockSite.GetDocumentUIManager().Dock(referenceBar,newBar,m_DockInfo.MouseOverDockSide);
                        
                        if (newDockSite.Width == 0 && (newDockSite.Dock == DockStyle.Left || newDockSite.Dock == DockStyle.Right))
                            newDockSite.Width = newBar.GetBarDockedSize(eOrientation.Vertical);
                        else if (newDockSite.Height == 0 && (newDockSite.Dock == DockStyle.Top || newDockSite.Dock == DockStyle.Bottom))
                            newDockSite.Height = newBar.GetBarDockedSize(eOrientation.Horizontal);

                        if (newDockSite != oldDockSite && oldDockSite != null)
                        {
                            cc.OnComponentChanged(oldDockSite, TypeDescriptor.GetProperties(typeof(DockSite))["DocumentDockContainer"], null, null);
                            cc.OnComponentChanged(oldDockSite, TypeDescriptor.GetProperties(typeof(DockSite))["Controls"], null, null);
                        }

						cc.OnComponentChanged(newDockSite,TypeDescriptor.GetProperties(typeof(DockSite))["DocumentDockContainer"],null,null);
                        if (referenceBar != null)
						    cc.OnComponentChanged(referenceBar,null,null,null);
						cc.OnComponentChanged(newBar,null,null,null);
						cc.OnComponentChanged(newDockSite,TypeDescriptor.GetProperties(typeof(DockSite))["Controls"],null,null);
						Form f = newBar.FindForm();
						if(f!=null) f.Refresh();
					}
					catch
					{
						trans.Cancel();
						throw;
					}
					finally
					{
						if(!trans.Canceled)
							trans.Commit();
					}
				}
				else if(m_DockInfo.MouseOverDockSide==eDockSide.Document && bar!=referenceBar)
				{
					BarDesigner referenceDesigner=designerHost.GetDesigner(referenceBar) as BarDesigner;
					if(referenceDesigner!=null)
						referenceDesigner.DelayedDockTabs(bar,m_MouseDownSelectedTabIndex);
				}

				m_DockInfo=new DockSiteInfo();
			}
			else if(m_MouseDownSelectedTabIndex!=-1)
			{
				if(m_MouseDownSelectedTabIndex!=bar.SelectedDockTab)
				{
					IComponentChangeService cc=(IComponentChangeService)GetService(typeof(IComponentChangeService));
					if(cc!=null)
					{
						cc.OnComponentChanged(bar,TypeDescriptor.GetProperties(typeof(Bar))["Items"],null,null);
						cc.OnComponentChanged(bar,TypeDescriptor.GetProperties(typeof(Bar))["SelectedDockTab"],null,null);
						cc.OnComponentChanged(bar,TypeDescriptor.GetProperties(typeof(Bar))["Controls"],null,null);
					}
				}
			}
			m_IsDocking=false;
			m_MouseDownSelectedTabIndex=-1;
			base.OnMouseDragEnd(cancel);
		}

		private Timer m_TimerDelayRemove=null;
		private Bar m_DelayDockBar=null;
		private int m_DelayDockTabIndex=-1;
		internal void DelayedDockTabs(Bar bar, int tabIndex)
		{
			if(m_TimerDelayRemove==null)
			{
				m_DelayDockBar=bar;
				m_DelayDockTabIndex=tabIndex;
				m_TimerDelayRemove=new Timer();
				m_TimerDelayRemove.Tick+=new EventHandler(this.TimerTickDelayRemove);
				m_TimerDelayRemove.Interval=200;
				m_TimerDelayRemove.Enabled=true;
				m_TimerDelayRemove.Start();
			}
		}
		private void TimerTickDelayRemove(object sender, EventArgs e)
		{
			m_TimerDelayRemove.Stop();
			m_TimerDelayRemove.Enabled=false;
			m_TimerDelayRemove.Dispose();
			m_TimerDelayRemove=null;

			DockTabs(m_DelayDockBar,m_DelayDockTabIndex,this.Control as Bar);

			m_DelayDockBar=null;
			m_DelayDockTabIndex=-1;
		}

		private void DockTabs(Bar sourceBar, int selectedTabIndex, Bar targetBar)
		{
			// Move Dock-container item to different bar
			IDesignerHost designerHost=this.GetService(typeof(IDesignerHost)) as IDesignerHost;
			DesignerTransaction trans=designerHost.CreateTransaction("DotNetBar Docking");
			IComponentChangeService cc=(IComponentChangeService)GetService(typeof(IComponentChangeService));
			try
			{
				DockContainerItem[] items=null;
				if(selectedTabIndex!=-1)
					items=new DockContainerItem[] {sourceBar.Items[sourceBar.SelectedDockTab] as DockContainerItem};
				else
				{
					items=new DockContainerItem[sourceBar.Items.Count];
					sourceBar.Items.CopyTo(items,0);
				}

				cc.OnComponentChanging(sourceBar,TypeDescriptor.GetProperties(typeof(Bar))["Controls"]);
				cc.OnComponentChanging(sourceBar,TypeDescriptor.GetProperties(typeof(Bar))["Items"]);
				sourceBar.Items.RemoveRange(items);
				cc.OnComponentChanged(sourceBar,TypeDescriptor.GetProperties(typeof(Bar))["Items"],null,null);
				cc.OnComponentChanged(sourceBar,TypeDescriptor.GetProperties(typeof(Bar))["Controls"],null,null);

				cc.OnComponentChanging(targetBar,TypeDescriptor.GetProperties(typeof(Bar))["Controls"]);
				cc.OnComponentChanging(targetBar,TypeDescriptor.GetProperties(typeof(Bar))["Items"]);
				targetBar.Items.AddRange(items);
				cc.OnComponentChanged(targetBar,TypeDescriptor.GetProperties(typeof(Bar))["Items"],null,null);
				cc.OnComponentChanged(targetBar,TypeDescriptor.GetProperties(typeof(Bar))["Controls"],null,null);

				if(sourceBar.Items.Count==0)
				{
					DockSite sourceDockSite=sourceBar.Parent as DockSite;
					cc.OnComponentChanging(sourceDockSite,TypeDescriptor.GetProperties(typeof(DockSite))["DocumentDockContainer"]);
					cc.OnComponentChanging(sourceBar,null);
					sourceDockSite.GetDocumentUIManager().UnDock(sourceBar,false);
					cc.OnComponentChanged(sourceBar,null,null,null);
					cc.OnComponentChanged(sourceDockSite,TypeDescriptor.GetProperties(typeof(DockSite))["DocumentDockContainer"],null,null);

					designerHost.DestroyComponent(sourceBar);
				}

				if(targetBar!=null && targetBar.SelectedDockTab>=0)
				{
					DockContainerItem dock=targetBar.Items[targetBar.SelectedDockTab] as DockContainerItem;
					if(dock!=null && dock.Control!=null)
					{
						cc.OnComponentChanged(targetBar,TypeDescriptor.GetProperties(typeof(Bar))["Controls"],null,null);
						dock.Control.BringToFront();
						cc.OnComponentChanged(targetBar,TypeDescriptor.GetProperties(typeof(Bar))["Controls"],null,null);
					}
				}
			}
			catch
			{
				trans.Cancel();
				throw;
			}
			finally
			{
				if(!trans.Canceled)
					trans.Commit();
			}
		}

		private void EndBarOwnerDocking(Bar bar)
		{
			IOwnerBarSupport barSupport=bar.Owner as IOwnerBarSupport;
			if(barSupport!=null)
				barSupport.DockComplete();
			if(m_OutlineForm!=null)
			{
				m_OutlineForm.Hide();
				m_OutlineForm.Dispose();
				m_OutlineForm=null;
			}
		}

        protected override IOwner GetIOwner()
        {
            Bar bar = this.Control as Bar;
            if (bar.Owner is IOwner)
                return bar.Owner as IOwner;
            return base.GetIOwner();
        }

        protected override IOwnerMenuSupport GetIOwnerMenuSupport()
        {
            Bar bar = this.Control as Bar;
            if (bar.Owner is IOwnerMenuSupport)
                return bar.Owner as IOwnerMenuSupport;
            return base.GetIOwnerMenuSupport();
        }

		/// <summary>
		/// Returns tab index under specified coordinates.
		/// </summary>
		/// <param name="x">Screen X coordinate</param>
		/// <param name="y">Screen Y coordinate</param>
		/// <returns>Tab index or -1 if tab was not found</returns>
		private int GetTabAt(int x, int y)
		{
			Bar bar=this.Control as Bar;
			if(bar==null) return -1;
			
			// Select dockable tab if mouse is clicked over the tab
			if(this.IsDockableWindow && bar.DockTabControl!=null)
			{
				Point posTab=bar.DockTabControl.PointToClient(new Point(x,y));
				if(bar.DockTabControl._TabSystemBox.Visible && bar.DockTabControl._TabSystemBox.DisplayRectangle.Contains(posTab))
				{
					return -1;
				}
				TabItem tab=bar.DockTabControl.HitTest(posTab.X,posTab.Y);
				if(tab!=null)
					return bar.Items.IndexOf(tab.AttachedItem);
			}

			return -1;
		}

		private bool IsInTabSystemBox(int x, int y)
		{
			Bar bar=this.Control as Bar;
			if(bar==null) return false;

			// Select dockable tab if mouse is clicked over the tab
			if(this.IsDockableWindow && bar.DockTabControl!=null)
			{
				Point posTab=bar.DockTabControl.PointToClient(new Point(x,y));
				if(bar.DockTabControl._TabSystemBox.Visible && bar.DockTabControl._TabSystemBox.DisplayRectangle.Contains(posTab))
					return true;
			}

			return false;
		}

		private void MouseDownTabSystemBox(int x, int y)
		{
			Bar bar=this.Control as Bar;
			if(bar==null) return;

			// Select dockable tab if mouse is clicked over the tab
			if(this.IsDockableWindow && bar.DockTabControl!=null)
			{
				Point posTab=bar.DockTabControl.PointToClient(new Point(x,y));
				if(bar.DockTabControl._TabSystemBox.Visible && bar.DockTabControl._TabSystemBox.DisplayRectangle.Contains(posTab))
				{
					if(bar.DockTabControl._TabSystemBox.ForwardEnabled && bar.DockTabControl._TabSystemBox.ForwardRect.Contains(posTab))
					{
						bar.DockTabControl.ScrollForward();
					}
					else if(bar.DockTabControl._TabSystemBox.BackEnabled && bar.DockTabControl._TabSystemBox.BackRect.Contains(posTab))
					{
						bar.DockTabControl.ScrollBackwards();
					}
				}
			}

		}

		/// <summary>
		/// Removes all subitems from container.
		/// </summary>
		protected override void ThisComponentRemoving(object sender, ComponentEventArgs e)
		{
			if(!m_InternalRemoving)
			{
				m_InternalRemoving=true;
				try
				{
					if(this.IsDockableWindow)
					{
						Bar bar=this.Control as Bar;
						if(bar!=null && bar.Parent is DockSite && ((DockSite)bar.Parent).DocumentDockContainer!=null)
						{
							IComponentChangeService cc=this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
							if(cc!=null)
								cc.OnComponentChanging(((DockSite)bar.Parent),TypeDescriptor.GetProperties(typeof(DockSite)).Find("DocumentDockContainer",true));
							((DockSite)bar.Parent).GetDocumentUIManager().UnDock(bar,false);
							if(cc!=null)
								cc.OnComponentChanged(((DockSite)bar.Parent),TypeDescriptor.GetProperties(typeof(DockSite)).Find("DocumentDockContainer",true),null,null);
						}
					}
				}
				finally
				{
					m_InternalRemoving=false;
				}
			}

			base.ThisComponentRemoving(sender,e);
		}

//#if FRAMEWORK20
//        public override DesignerActionListCollection ActionLists
//        {
//            get
//            {
//                DesignerActionListCollection lists = new DesignerActionListCollection();
//                lists.AddRange(base.ActionLists);

//                if (this.IsDockableWindow)
//                {
//                    lists.Add(
//                        new BarDockableDesignerActionList(this.Component));
//                }
//                return lists;
//            }
//        }
//#endif
		#endregion
	}

//#if FRAMEWORK20
//    public class BarDockableDesignerActionList : DesignerActionList
//    {
//#region Private Variables
//        private Bar m_Bar = null;
//#endregion

//        #region Internal Implementation
//        public BarDockableDesignerActionList(IComponent c)
//            : base(c)
//        {
//            m_Bar = (Bar)c;

//        }

//        [Editor(typeof(SelectedDockTabEditor), typeof(UITypeEditor))]
//        public int Selected
//        {
//            get
//            {
//                return m_Bar.SelectedDockTab;
//            }
//            set
//            {
//                TypeDescriptor.GetProperties(m_Bar)["SelectedDockTab"].SetValue(m_Bar, value);
//            }
//        }

//        public override DesignerActionItemCollection GetSortedActionItems()
//        {
//            DesignerActionItemCollection items = new DesignerActionItemCollection();
            
//            //Define static section header entries.
//            items.Add(new DesignerActionHeaderItem("Dock Tabs"));

//            items.Add(new DesignerActionPropertyItem("Selected",
//                         "Selected", "Dock Tabs",
//                         "Changes selected dock tab"));

//            return items;

//        }
//        #endregion
//    }

//    /// <summary>
//    /// Represents Selected dock tab editor.
//    /// </summary>
//    public class SelectedDockTabEditor : UITypeEditor
//    {
//        private IWindowsFormsEditorService edSvc = null;
//        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
//        {
//            if (context != null
//                && context.Instance != null
//                && provider != null)
//            {
//                edSvc = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
//                Bar bar = ((BarDockableDesignerActionList)context.Instance).Component as Bar;

//                int index=-1;
//                if (value != null && value is int)
//                    index = (int)value;

//                if (bar != null && edSvc != null)
//                {
//                    ListBox lb = new ListBox();
//                    lb.BorderStyle = BorderStyle.None;
//                    lb.SelectedIndexChanged += new EventHandler(SelectedIndexChanged);

//                    for(int i = 0; i<bar.Items.Count;i++)
//                    {
//                        BaseItem item = bar.Items[i];
//                        lb.Items.Add(item.Text + "[" + item.Name + "]");
//                        if (i == index)
//                            lb.SelectedIndex = i;
//                    }
//                    edSvc.DropDownControl(lb);
//                    if (lb.SelectedItem != null)
//                        return lb.SelectedIndex;
//                    else
//                        return -1;
//                }
//            }

//            return value;
//        }

//        private void SelectedIndexChanged(object sender, EventArgs e)
//        {
//            if (edSvc != null)
//                edSvc.CloseDropDown();
//        }

//        /// <summary>
//        /// Gets the editor style used by the EditValue method.
//        /// </summary>
//        /// <param name="context">An ITypeDescriptorContext that can be used to gain additional context information.</param>
//        /// <returns>A UITypeEditorEditStyle value that indicates the style of editor used by EditValue. If the UITypeEditor does not support this method, then GetEditStyle will return None.
//        /// </returns>
//        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
//        {
//            if (context != null && context.Instance != null)
//            {
//                return UITypeEditorEditStyle.DropDown;
//            }
//            return base.GetEditStyle(context);
//        }
//    }
//#endif
}
