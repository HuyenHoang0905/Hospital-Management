using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Represents Windows Forms designer for BubbleBar control.
	/// </summary>
	public class BubbleBarDesigner:System.Windows.Forms.Design.ControlDesigner
	{
		#region Private Variables
		private bool m_IgnoreMouseUp=false;
		private Point m_MouseDownPosition=Point.Empty;
		private bool m_Capture=false;

		#endregion


		public BubbleBarDesigner()
		{
		}

		public override void Initialize(IComponent component) 
		{
			base.Initialize(component);
			if(!component.Site.DesignMode)
				return;

			ISelectionService ss =(ISelectionService)GetService(typeof(ISelectionService));
			if(ss!=null)
				ss.SelectionChanged+=new EventHandler(OnSelectionChanged);

			// If our component is removed we need to clean-up
			IComponentChangeService cc=(IComponentChangeService)GetService(typeof(IComponentChangeService));
			if(cc!=null)
				cc.ComponentRemoving+=new ComponentEventHandler(this.OnComponentRemoving);
		}

		protected override void Dispose(bool disposing)
		{
			ISelectionService ss =(ISelectionService)GetService(typeof(ISelectionService));
			if(ss!=null)
				ss.SelectionChanged-=new EventHandler(OnSelectionChanged);

			// If our component is removed we need to clean-up
			IComponentChangeService cc=(IComponentChangeService)GetService(typeof(IComponentChangeService));
			if(cc!=null)
				cc.ComponentRemoving-=new ComponentEventHandler(this.OnComponentRemoving);

			base.Dispose(disposing);
		}

		public override DesignerVerbCollection Verbs 
		{
			get 
			{
				DesignerVerb[] verbs = new DesignerVerb[]
					{
						new DesignerVerb("Create Tab", new EventHandler(CreateTab)),
						new DesignerVerb("Create Button", new EventHandler(CreateButton))};
				return new DesignerVerbCollection(verbs);
			}
		}

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
            BubbleBar bar = this.Component as BubbleBar;
            if (bar == null)
                return;
            // Setup default back style
            bar.ButtonBackAreaStyle.SetColorScheme(new ColorScheme(eDotNetBarStyle.Office2003));
            bar.ButtonBackAreaStyle.BackColor = Color.FromArgb(66, Color.DimGray);
            bar.ButtonBackAreaStyle.BorderColor = Color.FromArgb(180, Color.WhiteSmoke);
            bar.ButtonBackAreaStyle.BorderTop = eStyleBorderType.Solid;
            bar.ButtonBackAreaStyle.BorderTopWidth = 1;
            bar.ButtonBackAreaStyle.BorderBottom = eStyleBorderType.Solid;
            bar.ButtonBackAreaStyle.BorderBottomWidth = 1;
            bar.ButtonBackAreaStyle.BorderLeft = eStyleBorderType.Solid;
            bar.ButtonBackAreaStyle.BorderLeftWidth = 1;
            bar.ButtonBackAreaStyle.BorderRight = eStyleBorderType.Solid;
            bar.ButtonBackAreaStyle.BorderRightWidth = 1;
            bar.ButtonBackAreaStyle.PaddingBottom = 3;
            bar.ButtonBackAreaStyle.PaddingTop = 3;
            bar.ButtonBackAreaStyle.PaddingLeft = 3;
            bar.ButtonBackAreaStyle.PaddingRight = 3;
            bar.SelectedTabColors.BorderColor = Color.Black;
            bar.MouseOverTabColors.BorderColor = SystemColors.Highlight;
        }

		private void OnSelectionChanged(object sender, EventArgs e) 
		{
			ISelectionService ss =(ISelectionService)GetService(typeof(ISelectionService));
			if(ss==null)
				return;

			bool refresh=ResetFocus();
			if(ss.PrimarySelection is BubbleBarTab)
			{
				if(((BubbleBarTab)ss.PrimarySelection).Parent==this.Control)
				{
					((BubbleBarTab)ss.PrimarySelection).Focus=true;
					refresh=true;
				}
			}
			else if(ss.PrimarySelection is BubbleButton)
			{
				if(((BubbleButton)ss.PrimarySelection).Parent.Parent==this.Control)
				{
					((BubbleButton)ss.PrimarySelection).Focus=true;
					refresh=true;
				}
			}

			if(refresh)
				this.Control.Refresh();
		}

		private bool ResetFocus()
		{
			bool refresh=false;
			BubbleBar bar=this.Control as BubbleBar;
			foreach(BubbleBarTab tab in bar.Tabs)
			{
				if(tab.Focus)
				{
					tab.Focus=false;
					refresh=true;
				}
				foreach(BubbleButton button in tab.Buttons)
				{
					if(button.Focus)
					{
						button.Focus=false;
						refresh=true;
					}
				}
			}
			return refresh;
		}

		private void OnComponentRemoving(object sender,ComponentEventArgs e)
		{
			if(e.Component==this.Component)
				ThisComponentRemoving(sender,e);
			else if(e.Component is BubbleBarTab)
			{
				BubbleBarTab tab=e.Component as BubbleBarTab;
				if(tab.Parent==this.Control)
				{
					IDesignerHost dh=(IDesignerHost)GetService(typeof(IDesignerHost));
					foreach(BubbleButton button in tab.Buttons)
						dh.DestroyComponent(button);
					BubbleBar bar=this.Component as BubbleBar;
					bar.Tabs.Remove(tab);
					bar.RecalcLayout();
					bar.Refresh();
				}
			}
			else if(e.Component is BubbleButton)
			{
				BubbleButton button=e.Component as BubbleButton;
				if(button.GetBubbleBar()==this.Control)
				{
					button.Parent.Buttons.Remove(button);
					BubbleBar bar=this.Component as BubbleBar;
					bar.RecalcLayout();
					bar.Refresh();
				}
			}
		}

		/// <summary>
		/// Removes all tabs and buttons.
		/// </summary>
		protected virtual void ThisComponentRemoving(object sender, ComponentEventArgs e)
		{
			//m_InternalRemoving=true;
//			try
//			{
				// Unhook events
				IComponentChangeService cc=(IComponentChangeService)GetService(typeof(IComponentChangeService));
				if(cc!=null)
					cc.ComponentRemoving-=new ComponentEventHandler(this.OnComponentRemoving);

				ISelectionService ss =(ISelectionService)GetService(typeof(ISelectionService));
				if(ss!=null)
					ss.SelectionChanged-=new EventHandler(OnSelectionChanged);

				IDesignerHost dh=(IDesignerHost)GetService(typeof(IDesignerHost));
				BubbleBar bar=this.Component as BubbleBar;
				if(dh==null)
					return;

				foreach(BubbleBarTab tab in bar.Tabs)
				{
					foreach(BubbleButton button in tab.Buttons)
						dh.DestroyComponent(button);
					dh.DestroyComponent(tab);
				}
			//}
//			finally
//			{
//				m_InternalRemoving=false;
//			}
		}

		private void CreateButton(object sender,EventArgs e)
		{
			BubbleBar bar=this.Control as BubbleBar;
			if(bar==null)
				return;

			if(bar.SelectedTab==null)
			{
				BubbleBarTab tab=CreateTab();
				IComponentChangeService change=this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
				if(change!=null)
					change.OnComponentChanging(this.Component,TypeDescriptor.GetProperties(bar).Find("SelectedTab",true));
				bar.SelectedTab=tab;
				if(change!=null)
					change.OnComponentChanged(this.Component,TypeDescriptor.GetProperties(bar).Find("SelectedTab",true),null,null);
			}

			if(bar.SelectedTab==null)
				return;

			BubbleButton button=CreateButton(bar.SelectedTab);

			if(button==null)
				return;

			ISelectionService ss=(ISelectionService)GetService(typeof(ISelectionService));
			if(ss!=null)
			{
				ArrayList list=new ArrayList();
				list.Add(button);
				ss.SetSelectedComponents(list);
			}
		}

		private void CreateTab(object sender,EventArgs e)
		{
			BubbleBar bar=this.Control as BubbleBar;
			if(bar==null)
				return;

			BubbleBarTab tab=CreateTab();
			if(tab!=null)
			{
				if(bar.SelectedTab!=tab)
				{
					IComponentChangeService change=this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
					if(change!=null)
						change.OnComponentChanging(this.Component,TypeDescriptor.GetProperties(bar).Find("SelectedTab",true));
					bar.SelectedTab=tab;
					if(change!=null)
						change.OnComponentChanged(this.Component,TypeDescriptor.GetProperties(bar).Find("SelectedTab",true),null,null);
				}

				ISelectionService ss=(ISelectionService)GetService(typeof(ISelectionService));
				if(ss!=null)
				{
					ArrayList list=new ArrayList();
					list.Add(tab);
					ss.SetSelectedComponents(list);
				}
			}
		}

		private BubbleBarTab CreateTab()
		{
			BubbleBar bar=this.Control as BubbleBar;
			if(bar==null)
				return null;
			
			IDesignerHost dh=(IDesignerHost)GetService(typeof(IDesignerHost));
			if(dh==null)
				return null;

			IComponentChangeService change=this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
			if(change!=null)
				change.OnComponentChanging(this.Component,TypeDescriptor.GetProperties(bar).Find("Tabs",true));

			BubbleBarTab tab=dh.CreateComponent(typeof(BubbleBarTab)) as BubbleBarTab;
			if(tab==null)
				return null;
            
			tab.Text=tab.Name;
			eTabItemColor color=eTabItemColor.Blue;
			if(bar.Tabs.Count>0)
			{
				int tt=bar.Tabs.Count+1;
				Type t=typeof(eTabItemColor);
				FieldInfo[] fi=t.GetFields(BindingFlags.Public | BindingFlags.Static);
				int count=fi.Length;
                while(tt>count)
					tt-=count;
				if(tt==0) tt++;
				color=(eTabItemColor)Enum.Parse(typeof(eTabItemColor),fi[tt].Name);
			}
			tab.PredefinedColor=color;

			bar.Tabs.Add(tab);

			if(change!=null)
				change.OnComponentChanged(this.Component,TypeDescriptor.GetProperties(bar).Find("Tabs",true),null,null);

			return tab;
		}

		private BubbleButton CreateButton(BubbleBarTab tab)
		{
			BubbleBar bar=this.Control as BubbleBar;
			if(bar==null)
				return null;
			
			IDesignerHost dh=(IDesignerHost)GetService(typeof(IDesignerHost));
			if(dh==null)
				return null;

			IComponentChangeService change=this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
			if(change!=null)
				change.OnComponentChanging(this.Component,TypeDescriptor.GetProperties(tab).Find("Buttons",true));

			BubbleButton button=dh.CreateComponent(typeof(BubbleButton)) as BubbleButton;
			if(button==null)
				return null;
            button.Image=BarFunctions.LoadBitmap("SystemImages.Note24.png");
			button.ImageLarge=BarFunctions.LoadBitmap("SystemImages.Note64.png");
			tab.Buttons.Add(button);

			if(change!=null)
				change.OnComponentChanged(this.Component,TypeDescriptor.GetProperties(tab).Find("Buttons",true),null,null);

			return button;
		}

		/// <summary>
		/// Returns all components associated with this control
		/// </summary>
		public override System.Collections.ICollection AssociatedComponents
		{
			get
			{
				ArrayList c=new ArrayList(base.AssociatedComponents);
				BubbleBar bar=this.Control as BubbleBar;
				if(bar!=null)
				{
					foreach(BubbleBarTab tab in bar.Tabs)
					{
						c.Add(tab);
						GetTabComponents(tab,c);
					}
				}
				return c;
			}
		}

		private void GetTabComponents(BubbleBarTab tab, ArrayList c)
		{
			foreach(BubbleButton b in tab.Buttons)
				c.Add(b);
		}

		protected override void OnSetCursor()
		{
			BubbleBar bar=this.Control as BubbleBar;
			Point pos=bar.PointToClient(System.Windows.Forms.Control.MousePosition);
			BubbleButton button=bar.GetButtonAt(pos);
			if(button!=null)
			{
				Cursor.Current=Cursors.Default;
				return;
			}
			BubbleBarTab tab=bar.GetTabAt(pos);
			if(tab!=null)
			{
				Cursor.Current=Cursors.Default;
				return;
			}
			base.OnSetCursor();
		}

		/// <summary>
		/// Selection support for items on container.
		/// </summary>
		protected override void WndProc(ref Message m)
		{
			switch(m.Msg)
			{
				case NativeFunctions.WM_LBUTTONDOWN:
				case NativeFunctions.WM_RBUTTONDOWN:
				{
					if(OnMouseDown(ref m))
						return;
					break;
				}
				case NativeFunctions.WM_RBUTTONUP:
				case NativeFunctions.WM_LBUTTONUP:
				{
					if(OnMouseUp(ref m))
						return;
					break;
				}
				case NativeFunctions.WM_MOUSEMOVE:
				{
					if(OnMouseMove(ref m))
						return;
					break;
				}
				case NativeFunctions.WM_LBUTTONDBLCLK:
				{
					if(OnMouseDoubleClick(m))
						return;
					break;
				}
			}

			base.WndProc(ref m);
		}

		private bool OnMouseDoubleClick(Message m)
		{
			bool processed=false;

			ISelectionService selection = (ISelectionService) this.GetService(typeof(ISelectionService));
			if(selection!=null && selection.PrimarySelection is BubbleButton && ((BubbleButton)selection.PrimarySelection).GetBubbleBar()==this.Control)
			{
				IDesignerHost host=(IDesignerHost) this.GetService(typeof(IDesignerHost));
				if(host!=null)
				{
					IDesigner designer=host.GetDesigner(selection.PrimarySelection as IComponent);
					if(designer!=null)
					{
						designer.DoDefaultAction();
						processed=true;
					}
				}
			}

			return processed;
		}

		protected virtual bool OnMouseDown(ref Message m)
		{
			BubbleBar bar=this.Control as BubbleBar;

			if(bar==null)
				return false;

			Point pos=bar.PointToClient(System.Windows.Forms.Control.MousePosition);
			m_MouseDownPosition=pos;

			BubbleButton button=bar.GetButtonAt(pos);
			if(button!=null)
			{
				ISelectionService selection = (ISelectionService) this.GetService(typeof(ISelectionService));
				if(selection!=null)
				{
					ArrayList arr=new ArrayList(1);
					arr.Add(button);
#if FRAMEWORK20
                    selection.SetSelectedComponents(arr, SelectionTypes.Primary);
#else
                    selection.SetSelectedComponents(arr,SelectionTypes.MouseDown);
#endif
                    m_IgnoreMouseUp =true;

					if(m.Msg==NativeFunctions.WM_RBUTTONDOWN)
					{
						this.OnContextMenu(System.Windows.Forms.Control.MousePosition.X,System.Windows.Forms.Control.MousePosition.Y);
					}

					return true;
				}
			}

			BubbleBarTab tab=bar.GetTabAt(pos);
			if(tab!=null)
			{
				ISelectionService selection = (ISelectionService) this.GetService(typeof(ISelectionService));
				if(selection!=null && selection.PrimarySelection!=tab)
				{
					ArrayList arr=new ArrayList(1);
					arr.Add(tab);
#if FRAMEWORK20
                    selection.SetSelectedComponents(arr, SelectionTypes.Primary);
#else
                    selection.SetSelectedComponents(arr,SelectionTypes.MouseDown);
#endif

                    if (bar.SelectedTab!=tab)
					{
						IComponentChangeService change=this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
						if(change!=null)
							change.OnComponentChanging(this.Component,TypeDescriptor.GetProperties(bar).Find("SelectedTab",true));

						bar.SelectedTab=tab;

						if(change!=null)
							change.OnComponentChanged(this.Component,TypeDescriptor.GetProperties(bar).Find("SelectedTab",true),null,null);
					}
					m_IgnoreMouseUp=true;
					if(m.Msg!=NativeFunctions.WM_RBUTTONDOWN)
						return true;
				}

				if(m.Msg==NativeFunctions.WM_RBUTTONDOWN)
				{
					this.OnContextMenu(System.Windows.Forms.Control.MousePosition.X,System.Windows.Forms.Control.MousePosition.Y);
					return true;
				}
			}
					
			return false;
		}

		protected virtual bool OnMouseMove(ref Message m)
		{
			BubbleBar bar=this.Control as BubbleBar;
			Point pos=bar.PointToClient(System.Windows.Forms.Control.MousePosition);

			if(Control.MouseButtons==MouseButtons.Left)
			{
				if(bar.DragInProgress)
				{
					bar.DragMouseMove(pos);
					return true;
				}
				else if(!m_MouseDownPosition.IsEmpty && Math.Abs(m_MouseDownPosition.X-pos.X)>=2 || Math.Abs(m_MouseDownPosition.Y-pos.Y)>=2)
				{
					BubbleBarTab tabDrag=bar.GetTabAt(pos);
					if(tabDrag!=null)
					{
						bar.StartDrag(tabDrag);
						System.Windows.Forms.Control c=System.Windows.Forms.Control.FromHandle(m.HWnd);
						if(c!=null)
						{
							m_Capture=true;
							c.Capture=true;
						}
						return true;
					}
					BubbleButton buttonDrag=bar.GetButtonAt(pos);
					if(buttonDrag!=null)
					{
						bar.StartDrag(buttonDrag);
						System.Windows.Forms.Control c=System.Windows.Forms.Control.FromHandle(m.HWnd);
						if(c!=null)
						{
							m_Capture=true;
							c.Capture=true;
						}
						return true;
					}
					m_MouseDownPosition=Point.Empty;
				}
			}

			//BubbleButton button=bar.GetButtonAt(pos);
			BubbleBarTab tab=bar.GetTabAt(pos);
			if(tab!=null)
				bar.SetMouseOverTab(tab);
			else
				bar.SetMouseOverTab(null);

			return false;
		}

		private bool OnMouseUp(ref Message m)
		{
			if(m_Capture)
			{
				System.Windows.Forms.Control c=System.Windows.Forms.Control.FromHandle(m.HWnd);
				if(c!=null)
					c.Capture=false;
				m_Capture=false;
			}

            BubbleBar bar=this.Control as BubbleBar;
			if(bar!=null && bar.DragInProgress)
			{
				Point pos=bar.PointToClient(System.Windows.Forms.Control.MousePosition);
				bar.DragMouseUp(pos);
				
				BubbleBarTab tab=bar.SelectedTab;
				if(tab!=null)
				{
					IComponentChangeService change=this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
					if(change!=null)
						change.OnComponentChanging(this.Component,TypeDescriptor.GetProperties(tab).Find("Buttons",true));
					if(change!=null)
						change.OnComponentChanged(this.Component,TypeDescriptor.GetProperties(tab).Find("Buttons",true),null,null);
				}
			}

			if(m_IgnoreMouseUp)
			{
				m_IgnoreMouseUp=false;
				return true;
			}

			return false;
		}
	}
}
