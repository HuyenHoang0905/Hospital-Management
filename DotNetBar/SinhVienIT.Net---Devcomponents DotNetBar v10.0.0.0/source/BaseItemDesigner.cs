using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Represents designer for BaseItem objects and derived classes.
	/// </summary>
	public class BaseItemDesigner:System.ComponentModel.Design.ComponentDesigner,IDesignerServices
	{
		#region Internal Implementation
		/// <summary>
		/// Creates new instance of the class.
		/// </summary>
		public BaseItemDesigner()
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

			BaseItem c=component as BaseItem;
			if(c!=null)
				this.Visible=c.Visible;

			// If our component is removed we need to clean-up
			IComponentChangeService cc=(IComponentChangeService)GetService(typeof(IComponentChangeService));
			if(cc!=null)
			{
				cc.ComponentRemoved+=new ComponentEventHandler(this.OnComponentRemoved);
			}
		}
		protected override void Dispose(bool disposing)
		{
			ISelectionService ss =(ISelectionService)GetService(typeof(ISelectionService));
			if(ss!=null)
				ss.SelectionChanged-=new EventHandler(OnSelectionChanged);

			IComponentChangeService cc=(IComponentChangeService)GetService(typeof(IComponentChangeService));
			if(cc!=null)
			{
				cc.ComponentRemoved-=new ComponentEventHandler(this.OnComponentRemoved);
			}

			base.Dispose(disposing);
		}

		private void OnComponentRemoved(object sender,ComponentEventArgs e)
		{
			if(e.Component is BaseItem)
			{
				BaseItem parent=this.Component as BaseItem;
				BaseItem item=e.Component as BaseItem;
				if(item!=null && parent!=null && parent.SubItems.Contains(item))
				{
					IComponentChangeService cc=(IComponentChangeService)GetService(typeof(IComponentChangeService));
					if(cc!=null)
						cc.OnComponentChanging(parent,TypeDescriptor.GetProperties(parent)["SubItems"]);
					parent.SubItems.Remove(item);
					if(cc!=null)
						cc.OnComponentChanged(parent,TypeDescriptor.GetProperties(parent)["SubItems"],null,null);
					this.RecalcLayout();
				}
			}
		}

		private void OnSelectionChanged(object sender, EventArgs e) 
		{
			ISelectionService ss = (ISelectionService)sender;
			if(ss!=null && ss.PrimarySelection!=this.Component && ss.PrimarySelection is BaseItem)
			{
				BaseItem item=this.Component as BaseItem;
				if(item!=null)
				{
					BaseItem selected=ss.PrimarySelection as BaseItem;
					IOwner owner=item.GetOwner() as IOwner;
					if(owner!=null)
					{
						if(owner.GetItem(selected.Name)!=selected)
							owner.SetFocusItem(null);
					}
				}
			}
		}

		public override DesignerVerbCollection Verbs 
		{
			get 
			{
				DesignerVerb[] verbs = new DesignerVerb[]
					{
						new DesignerVerb("Create Button", new EventHandler(CreateButton)),
						new DesignerVerb("Create Text Box", new EventHandler(CreateTextBox)),
						new DesignerVerb("Create Combo Box", new EventHandler(CreateComboBox)),
						new DesignerVerb("Create Label", new EventHandler(CreateLabel)),
				};
				return new DesignerVerbCollection(verbs);
			}
		}

		protected virtual void CreateButton(object sender, EventArgs e)
		{
			CreateNewItem(typeof(ButtonItem));
		}

		protected virtual void CreateComboBox(object sender, EventArgs e)
		{
			CreateNewItem(typeof(ComboBoxItem));
		}

		protected virtual void CreateLabel(object sender, EventArgs e)
		{
			CreateNewItem(typeof(LabelItem));
		}

		protected virtual void CreateTextBox(object sender, EventArgs e)
		{
			CreateNewItem(typeof(TextBoxItem));
		}

		protected virtual void CreateNewItem(Type itemType)
		{
			BaseItem parent=this.Component as BaseItem;
			System.ComponentModel.Design.IDesignerHost dh=(System.ComponentModel.Design.IDesignerHost)GetService(typeof(System.ComponentModel.Design.IDesignerHost));
			System.ComponentModel.Design.IComponentChangeService change=this.GetService(typeof(System.ComponentModel.Design.IComponentChangeService)) as IComponentChangeService;
			if(change!=null)
				change.OnComponentChanging(this.Component,TypeDescriptor.GetProperties(parent).Find("SubItems",true));
			BaseItem item=dh.CreateComponent(itemType) as BaseItem;
			if(item==null)
				return;
			item.Text=item.Name;
			BeforeNewItemAdded(item);
			parent.SubItems.Add(item);
			AfterNewItemAdded(item);
			if(change!=null)
				change.OnComponentChanged(this.Component,TypeDescriptor.GetProperties(parent).Find("SubItems",true),null,null);
			
			this.RecalcLayout();
		}

		protected virtual void BeforeNewItemAdded(BaseItem item) {}

		protected virtual void AfterNewItemAdded(BaseItem item) {}

		protected virtual void RecalcLayout()
		{
			BaseItem item=this.Component as BaseItem;
			System.Windows.Forms.Control control=item.ContainerControl as System.Windows.Forms.Control;
			
			if(control is Bar)
				((Bar)control).RecalcLayout();
			else if(control is ExplorerBar)
				((ExplorerBar)control).RecalcLayout();
			else if(control is BarBaseControl)
				((BarBaseControl)control).RecalcLayout();
			else if(control is SideBar)
				((SideBar)control).RecalcLayout();
		}

		public override System.Collections.ICollection AssociatedComponents
		{
			get
			{
				System.Collections.ArrayList components=new System.Collections.ArrayList();
				BaseItem parent=this.Component as BaseItem;
				if(parent==null)
					return base.AssociatedComponents;
				parent.SubItems.CopyTo(components);
				return components;
			}
		}

		protected override void PreFilterProperties(IDictionary properties)
		{
			base.PreFilterProperties(properties);
			properties["Visible"] = TypeDescriptor.CreateProperty(typeof(BaseItemDesigner),(PropertyDescriptor)properties["Visible"], new Attribute[]
				{
					new DefaultValueAttribute(true),
					new BrowsableAttribute(true),
					new CategoryAttribute("Layout")});
			properties["CanCustomize"] = TypeDescriptor.CreateProperty(typeof(BaseItemDesigner),(PropertyDescriptor)properties["CanCustomize"], new Attribute[]
				{
					new DefaultValueAttribute(true),
					new BrowsableAttribute(true),
					new CategoryAttribute("Behavior"),
					new DescriptionAttribute("Indicates whether item can be customized by user.")});
		}

		/// <summary>
		/// Gets or sets whether item can be customized by end user.
		/// </summary>
		[System.ComponentModel.Browsable(true),DevCoBrowsable(true),System.ComponentModel.DefaultValue(true),System.ComponentModel.Category("Behavior"),System.ComponentModel.Description("Indicates whether item can be customized by user.")]
		public virtual bool CanCustomize
		{
			get
			{
				return (bool)ShadowProperties["CanCustomize"];
			}
			set
			{
				// this value is not passed to the actual control
				this.ShadowProperties["CanCustomize"] = value;
			}
		}

		/// <summary>
		/// Gets or sets whether item is visible.
		/// </summary>
		[DefaultValue(true),Browsable(true),DevCoBrowsable(true),Category("Layout"),Description("Gets or sets whether item is visible.")]
		public bool Visible 
		{
			get 
			{
				return (bool)ShadowProperties["Visible"];
			}
			set 
			{
				// this value is not passed to the actual control
				this.ShadowProperties["Visible"] = value;
			}
		}
		#endregion

		#region IDesignerServices Implementation
		object IDesignerServices.CreateComponent(System.Type componentClass)
		{
			IDesignerHost dh=(IDesignerHost)GetService(typeof(IDesignerHost));
			if(dh==null)
				return null;
			return dh.CreateComponent(componentClass);
		}

		void IDesignerServices.DestroyComponent(IComponent c)
		{
			IDesignerHost dh=(IDesignerHost)GetService(typeof(IDesignerHost));
			if(dh==null)
				return;
			dh.DestroyComponent(c);
		}

		object IDesignerServices.GetService(Type serviceType)
		{
			return this.GetService(serviceType);
		}
		#endregion
	}
}
