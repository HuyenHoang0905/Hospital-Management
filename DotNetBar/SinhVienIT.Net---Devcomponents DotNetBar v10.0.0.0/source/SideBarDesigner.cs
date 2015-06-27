using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.ComponentModel.Design.Serialization;
using System.CodeDom;
using System.Drawing;
using System.Collections;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Represents Windows Forms designer for SideBar control.
	/// </summary>
	public class SideBarDesigner:BarBaseControlDesigner
	{
		#region Internal Implementation
		public SideBarDesigner():base()
		{
			this.EnableItemDragDrop=true;
		}

		public override void Initialize(IComponent component) 
		{
			base.Initialize(component);
			if(!component.Site.DesignMode)
				return;
			SideBar bar=this.Control as SideBar;
			if(bar!=null)
				bar.SetDesignMode();
		}

		public override DesignerVerbCollection Verbs 
		{
			get 
			{
				DesignerVerb[] verbs;
				verbs = new DesignerVerb[]
				{
					new DesignerVerb("Create Panel", new EventHandler(OnAddGroup)),
					new DesignerVerb("Create Button", new EventHandler(OnAddButton)),
					new DesignerVerb("Choose Color Scheme", new EventHandler(OnPickColorScheme))
				};
				return new DesignerVerbCollection(verbs);
			}
		}

		protected override BaseItem GetItemContainer()
		{
			SideBar bar=this.Control as SideBar;
			if(bar!=null)
				return bar.ItemsContainer;
			return null;
		}

		protected override void RecalcLayout()
		{
			SideBar bar=this.GetItemContainerControl() as SideBar;
			if(bar!=null)
				bar.RecalcLayout();
		}

		protected override void OnSubItemsChanging()
		{
			base.OnSubItemsChanging();
			IComponentChangeService change=this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
			if(change!=null)
			{
				SideBar bar=this.GetItemContainerControl() as SideBar;
				change.OnComponentChanging(this.Component,TypeDescriptor.GetProperties(bar).Find("Panels",true));
			}
		}

		protected override void OnSubItemsChanged()
		{
			base.OnSubItemsChanged();
			IComponentChangeService change=this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
			if(change!=null)
			{
				SideBar bar=this.GetItemContainerControl() as SideBar;
				change.OnComponentChanged(this.Component,TypeDescriptor.GetProperties(bar).Find("Panels",true),null,null);
			}
		}

		private void OnAddGroup(object sender, EventArgs e)
		{
			IDesignerHost dh=this.GetService(typeof(IDesignerHost)) as IDesignerHost;
			DesignerTransaction trans=null;
			if(dh!=null)
				trans=dh.CreateTransaction("New SideBarPanelItem");
			try
			{
				CreateNewPanel();
			}
			finally
			{
				if(trans!=null && !trans.Canceled)
					trans.Commit();
			}
			
			this.RecalcLayout();
		}
		
		private SideBarPanelItem CreateNewPanel()
		{
			SideBarPanelItem item=null;
			SideBar bar=this.Component as SideBar;
			IDesignerHost dh=GetIDesignerHost();
			if(bar!=null && dh!=null)
			{
				OnSubItemsChanging();
				item=dh.CreateComponent(typeof(SideBarPanelItem)) as SideBarPanelItem;
				if(item==null)
					return null;
				
				IComponentChangeService change=this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
				if(change!=null)
					change.OnComponentChanging(item,null);
				item.Text=item.Name;
				bar.Panels.Add(item);
                item.Appearance = bar.Appearance;
                if (item.Appearance == eSideBarAppearance.Flat)
                    SideBar.ApplyColorScheme(item, bar.PredefinedColorScheme);

				if(change!=null)
					change.OnComponentChanged(item,null,null,null);
				OnSubItemsChanged();
			}
			return item;
		}

		private void OnAddButton(object sender, EventArgs e)
		{
			SideBar bar=this.Component as SideBar;
			IDesignerHost dh=GetIDesignerHost();
			if(bar!=null && dh!=null)
			{
				ISelectionService selection = (ISelectionService) this.GetService(typeof(ISelectionService));
				if(selection==null)
					return;
				SideBarPanelItem panel=null;
				if(!(selection.PrimarySelection is SideBarPanelItem))
				{
					if(bar.ExpandedPanel==null)
						panel=CreateNewPanel();
					else
						panel=bar.ExpandedPanel;
				}
				else
					panel=selection.PrimarySelection as SideBarPanelItem;

				if(panel==null)
					return;

				IComponentChangeService change=this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
				if(change!=null)
					change.OnComponentChanging(this.Component,TypeDescriptor.GetProperties(panel).Find("SubItems",true));
				ButtonItem item=dh.CreateComponent(typeof(ButtonItem)) as ButtonItem;
				if(item==null)
					return;
				item.Text="New Button";
				if(bar.Appearance==eSideBarAppearance.Flat)
					item.ImagePosition=eImagePosition.Left;
				else
					item.ImagePosition=eImagePosition.Top;
				item.ButtonStyle=eButtonStyle.ImageAndText;
				panel.SubItems.Add(item);
				if(change!=null)
					change.OnComponentChanged(this.Component,TypeDescriptor.GetProperties(panel).Find("SubItems",true),null,null);

				this.RecalcLayout();
			}
		}

		private void OnPickColorScheme(object sender, EventArgs e)
		{
			using(SideBarColorSchemePicker picker=new SideBarColorSchemePicker())
			{
				picker.ShowDialog();
				if(picker.DialogResult==DialogResult.OK)
				{
					SetColorScheme(picker.SelectedColorScheme);
				}
			}
		}

		private void SetColorScheme(eSideBarColorScheme scheme)
		{
			SideBar sidebar=this.Control as SideBar;
			if(sidebar!=null)
			{
				if(sidebar.Appearance!=eSideBarAppearance.Flat)
                    TypeDescriptor.GetProperties(sidebar)["Appearance"].SetValue(sidebar, eSideBarAppearance.Flat);
                TypeDescriptor.GetProperties(sidebar)["PredefinedColorScheme"].SetValue(sidebar, scheme);
				sidebar.ApplyPredefinedColorScheme(scheme);
				this.RaiseStyleChanged();
			}
		}
		private void RaiseStyleChanged()
		{
			IDesignerHost dh=GetIDesignerHost();
			DesignerTransaction trans=null;
			if(dh!=null)
				trans=dh.CreateTransaction();
			SideBar sidebar=this.Control as SideBar;
			IComponentChangeService change=this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
			if(change!=null)
			{
				change.OnComponentChanging(this.Control,null);
				change.OnComponentChanged(this.Control,null,null,null);

				change.OnComponentChanging(this.Component,TypeDescriptor.GetProperties(sidebar).Find("ColorScheme",true));
				change.OnComponentChanged(this.Component,TypeDescriptor.GetProperties(sidebar).Find("ColorScheme",true),null,null);

				change.OnComponentChanging(this.Component,TypeDescriptor.GetProperties(sidebar).Find("Panels",true));
				change.OnComponentChanged(this.Component,TypeDescriptor.GetProperties(sidebar).Find("Panels",true),null,null);

				foreach(SideBarPanelItem panel in sidebar.Panels)
				{
					change.OnComponentChanging(panel,TypeDescriptor.GetProperties(typeof(SideBarPanelItem))["HeaderStyle"]);
					change.OnComponentChanged(panel,TypeDescriptor.GetProperties(typeof(SideBarPanelItem))["HeaderStyle"],null,null);

					change.OnComponentChanging(panel,TypeDescriptor.GetProperties(typeof(SideBarPanelItem))["HeaderHotStyle"]);
					change.OnComponentChanged(panel,TypeDescriptor.GetProperties(typeof(SideBarPanelItem))["HeaderHotStyle"],null,null);

					change.OnComponentChanging(panel,TypeDescriptor.GetProperties(typeof(SideBarPanelItem))["HeaderMouseDownStyle"]);
					change.OnComponentChanged(panel,TypeDescriptor.GetProperties(typeof(SideBarPanelItem))["HeaderMouseDownStyle"],null,null);

					change.OnComponentChanging(panel,TypeDescriptor.GetProperties(typeof(SideBarPanelItem))["HeaderSideHotStyle"]);
					change.OnComponentChanged(panel,TypeDescriptor.GetProperties(typeof(SideBarPanelItem))["HeaderSideHotStyle"],null,null);

					change.OnComponentChanging(panel,TypeDescriptor.GetProperties(typeof(SideBarPanelItem))["HeaderSideMouseDownStyle"]);
					change.OnComponentChanged(panel,TypeDescriptor.GetProperties(typeof(SideBarPanelItem))["HeaderSideMouseDownStyle"],null,null);

					change.OnComponentChanging(panel,TypeDescriptor.GetProperties(typeof(SideBarPanelItem))["HeaderSideStyle"]);
					change.OnComponentChanged(panel,TypeDescriptor.GetProperties(typeof(SideBarPanelItem))["HeaderSideStyle"],null,null);

				}							
			}
			if(trans!=null)
				trans.Commit();
		}

		private IDesignerHost GetIDesignerHost()
		{
			try
			{
				IDesignerHost dh=(IDesignerHost)GetService(typeof(IDesignerHost));
				return dh;
			}
			catch{}
			return null;
		}

		protected override void PreFilterProperties(
			IDictionary properties) 
		{
			base.PreFilterProperties(properties);
			// shadow the checked property so we can intercept the set.
			//
			properties["Appearance"] = 
				TypeDescriptor.CreateProperty(typeof(SideBarDesigner), 
				(PropertyDescriptor)properties["Appearance"], 
				new Attribute[0]);
		}

		/// <summary>
		/// Gets or sets visual appearance for the control.
		/// </summary>
		[Browsable(true),DefaultValue(eSideBarAppearance.Traditional),Category("Appearance"),Description("Indicates visual appearance for the control.")]
		public eSideBarAppearance Appearance
		{
			get 
			{
				return ((SideBar)Control).Appearance;
			}
			set 
			{
				// set the value into the control
				if(((SideBar)Control).Appearance != value)
				{
					((SideBar)Control).Appearance=value;
					if(value==eSideBarAppearance.Flat)
					{
						SetColorScheme(((SideBar)Control).PredefinedColorScheme);
					}
					else if(value==eSideBarAppearance.Traditional)
					{
						((SideBar)Control).ResetTraditional();
						RaiseStyleChanged();
					}
				}
			}
		}

		protected override bool CanDragItem(BaseItem item)
		{
			if(item is SideBarPanelItem)
				return false;
			return base.CanDragItem(item);
		}
		#endregion
	}
}
