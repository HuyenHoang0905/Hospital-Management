using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Design
{
	/// <summary>
	/// Represents designer for BaseItem objects and derived classes.
	/// </summary>
	public class BaseItemDesigner:System.ComponentModel.Design.ComponentDesigner,IDesignerServices
    {
        #region Private Variables
        protected bool m_AddingItem = false;
        protected bool m_CreatingItem = false;
        protected DesignerTransaction m_InsertItemTransaction = null;
        #endregion

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
                cc.ComponentAdding += new ComponentEventHandler(ComponentChangeComponentAdding);
                cc.ComponentAdded += new ComponentEventHandler(ComponentChangeComponentAdded);
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

        protected virtual void SetDesignTimeDefaults()
        {
        }

        protected virtual void ComponentChangeComponentAdded(object sender, ComponentEventArgs e)
        {
            if (m_AddingItem)
            {
                m_AddingItem = false;
                IComponentChangeService cc = this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
                BaseItem parent = this.Component as BaseItem;

                if (cc != null)
                    cc.OnComponentChanging(parent, TypeDescriptor.GetProperties(parent)["SubItems"]);
                
                parent.SubItems.Add(e.Component as BaseItem);
                
                if (cc != null)
                    cc.OnComponentChanged(parent, TypeDescriptor.GetProperties(parent)["SubItems"], null, null);

                m_InsertItemTransaction.Commit();
                m_InsertItemTransaction = null;
                this.RecalcLayout();
            }
        }

        protected virtual void ComponentChangeComponentAdding(object sender, ComponentEventArgs e)
        {
            if (m_InsertItemTransaction == null && !m_AddingItem && !m_CreatingItem && e.Component is BaseItem)
            {
                ISelectionService ss = this.GetService(typeof(ISelectionService)) as ISelectionService;
                if (ss != null && ss.PrimarySelection == this.Component)
                {
                    m_AddingItem = true;
                    IDesignerHost dh = this.GetService(typeof(IDesignerHost)) as IDesignerHost;
                    m_InsertItemTransaction = dh.CreateTransaction("Adding Item Clip");
                }
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
            ComponentRemoved(e);
		}

        protected virtual void ComponentRemoved(ComponentEventArgs e) { }

		private void OnSelectionChanged(object sender, EventArgs e) 
		{
			ISelectionService ss = (ISelectionService)sender;
            
            if (ss != null && ss.PrimarySelection != this.Component)
            {
                ((BaseItem)this.Component).DesignTimeMouseDownPoint = Point.Empty;
            }

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

            if (ss != null)
            {
                ICollection selectedComponents = ss.GetSelectedComponents();
                bool selected = false;
                foreach(object o in selectedComponents)
                {
                    if (o == this.Component)
                    {
                        selected = true;
                        break;
                    }
                }

                BaseItem item = this.Component as BaseItem;
                if (selected)
                {
                    if (!item.Focused)
                        item.OnGotFocus();
                }
                else if (item.Focused)
                    item.OnLostFocus();
            }
		}

        protected virtual DesignerVerb[] GetVerbs()
        {
            DesignerVerb[] verbs = new DesignerVerb[]
                							{
                								new DesignerVerb("Add Button", new EventHandler(CreateButton)),
                								new DesignerVerb("Add Text Box", new EventHandler(CreateTextBox)),
                								new DesignerVerb("Add Combo Box", new EventHandler(CreateComboBox)),
                								new DesignerVerb("Add Label", new EventHandler(CreateLabel)),
                		                        new DesignerVerb("Add Color Picker", new EventHandler(CreateColorPicker)),
                		                        new DesignerVerb("Add Check Box", new EventHandler(CreateCheckBox)),
                		                        new DesignerVerb("Add Control Container", new EventHandler(CreateControlContainer)),
                                                new DesignerVerb("Add Micro-Chart", new EventHandler(CreateMicroChart)),
                                                new DesignerVerb("Add Switch Button", new EventHandler(CreateSwitch)),
                		                        new DesignerVerb("Add Slider", new EventHandler(CreateSlider)),
                		                        new DesignerVerb("Add Rating Item", new EventHandler(CreateRatingItem)),
                		                        new DesignerVerb("Add Horizontal Container", new EventHandler(CreateHorizontalContainer)),
                		                        new DesignerVerb("Add Vertical Container", new EventHandler(CreateVerticalContainer)),
                		                        new DesignerVerb("Add Gallery Container", new EventHandler(CreateGallery))
                						};
            return verbs;
        }

        public override DesignerVerbCollection Verbs 
		{
			get 
			{
                DesignerVerb[] verbs = GetVerbs();
				return new DesignerVerbCollection(verbs);
			}
		}

        protected virtual void CreateMicroChart(object sender, EventArgs e)
        {
            CreateNewItem(typeof(MicroChartItem));
        }


        protected virtual void CreateCheckBox(object sender, EventArgs e)
        {
            CreateNewItem(typeof(CheckBoxItem));
        }

        protected virtual void CreateRatingItem(object sender, EventArgs e)
        {
            CreateNewItem(typeof(RatingItem));
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

        protected virtual void CreateColorPicker(object sender, EventArgs e)
        {
            CreateNewItem(typeof(ColorPickerDropDown));
        }

        protected virtual void CreateControlContainer(object sender, EventArgs e)
        {
            CreateNewItem(typeof(ControlContainerItem));
        }

        protected virtual void CreateSlider(object sender, EventArgs e)
        {
            CreateNewItem(typeof(SliderItem));
        }

        protected virtual void CreateSwitch(object sender, EventArgs e)
        {
            CreateNewItem(typeof(SwitchButtonItem));
        }

        protected virtual void CreateProgressBar(object sender, EventArgs e)
        {
            CreateNewItem(typeof(ProgressBarItem));
        }

        protected virtual void CreateCircularProgressItem(object sender, EventArgs e)
        {
            CreateNewItem(typeof(CircularProgressItem));
        }
#if FRAMEWORK20
        protected virtual void CreateMonthCalendar(object sender, EventArgs e)
        {
            CreateNewItem(typeof(DevComponents.Editors.DateTimeAdv.MonthCalendarItem));
        }
#endif
        private void CreateVerticalContainer(object sender, EventArgs e)
        {
            CreateContainer(eOrientation.Vertical);
        }

        private void CreateHorizontalContainer(object sender, EventArgs e)
        {
            CreateContainer(eOrientation.Horizontal);
        }

        private void CreateContainer(eOrientation orientation)
        {
            try
            {
                m_CreatingItem = true;
                DesignerSupport.CreateItemContainer(this, (BaseItem)this.Component, orientation);
            }
            finally
            {
                m_CreatingItem = false;
            }
            this.RecalcLayout();
        }

        protected virtual void CreateGallery(object sender, EventArgs e)
        {
            CreateNewItem(typeof(GalleryContainer));
        }

		protected virtual void CreateNewItem(Type itemType)
		{
			BaseItem parent=this.Component as BaseItem;
			System.ComponentModel.Design.IDesignerHost dh=(System.ComponentModel.Design.IDesignerHost)GetService(typeof(System.ComponentModel.Design.IDesignerHost));

            DesignerTransaction trans = dh.CreateTransaction("Creating New Item");
            try
            {
                m_CreatingItem = true;
                BaseItem item = dh.CreateComponent(itemType) as BaseItem;
                if (item == null)
                    return;
                if (itemType != typeof(RatingItem))
                    item.Text = item.Name;
                BeforeNewItemAdded(item);

                AddNewItem(item);

                AfterNewItemAdded(item);
                this.RecalcLayout();
                this.NewItemAdded(item);
            }
            catch
            {
                trans.Cancel();
                throw;
            }
            finally
            {
                m_CreatingItem = false;
                if (!trans.Canceled)
                    trans.Commit();
            }
		}

        protected virtual void AddNewItem(BaseItem newItem)
        {
            BaseItem parent = this.Component as BaseItem;
            System.ComponentModel.Design.IComponentChangeService change = this.GetService(typeof(System.ComponentModel.Design.IComponentChangeService)) as IComponentChangeService;
            if (change != null)
                change.OnComponentChanging(this.Component, TypeDescriptor.GetProperties(parent).Find("SubItems", true));

            parent.SubItems.Add(newItem);

            if (change != null)
                change.OnComponentChanged(this.Component, TypeDescriptor.GetProperties(parent).Find("SubItems", true), null, null);
        }

		protected virtual void BeforeNewItemAdded(BaseItem item) {}

		protected virtual void AfterNewItemAdded(BaseItem item)
        {
            if (item is LabelItem && Helpers.IsOffice2007Style(item.EffectiveStyle))
            {
                LabelItem label = item as LabelItem;
                if (item.Parent is PopupItem && ((PopupItem)item.Parent).PopupType == ePopupType.Menu)
                {
                    label.BackColor = ColorScheme.GetColor("DDE7EE");
                    label.BorderType = eBorderType.SingleLine;
                    label.SingleLineColor = ColorScheme.GetColor("C5C5C5");
                    label.ForeColor = ColorScheme.GetColor("00156E");
                    label.BorderSide = eBorderSide.Bottom;
                    label.PaddingTop = 1;
                    label.PaddingLeft = 10;
                    label.PaddingBottom = 1;
                }
            }
            else if (item is GalleryContainer && item.Parent is ButtonItem)
            {
                TypeDescriptor.GetProperties(item)["MinimumSize"].SetValue(item, new Size(150, 200));
                item.NeedRecalcSize = true;
                TypeDescriptor.GetProperties(((GalleryContainer)item).BackgroundStyle)["Class"].SetValue(((GalleryContainer)item).BackgroundStyle, "");
                if(item.Parent is PopupItem)
                    TypeDescriptor.GetProperties(item)["EnableGalleryPopup"].SetValue(item, false);
            }
            else if (item is GalleryContainer && item.Parent is ItemContainer && !(item.ContainerControl is RibbonBar))
            {
                TypeDescriptor.GetProperties(item)["MinimumSize"].SetValue(item, new Size(150, 200));
                TypeDescriptor.GetProperties(((GalleryContainer)item).BackgroundStyle)["Class"].SetValue(((GalleryContainer)item).BackgroundStyle, "");
                TypeDescriptor.GetProperties(item)["EnableGalleryPopup"].SetValue(item, false);
                TypeDescriptor.GetProperties(item)["LayoutOrientation"].SetValue(item, DevComponents.DotNetBar.eOrientation.Vertical);
                TypeDescriptor.GetProperties(item)["MultiLine"].SetValue(item, false);
                TypeDescriptor.GetProperties(item)["PopupUsesStandardScrollbars"].SetValue(item, false);
                item.NeedRecalcSize = true;
            }
        }

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
			else if(control is ItemControl)
				((ItemControl)control).RecalcLayout();
            else if (control is MenuPanel)
                ((MenuPanel)control).RecalcSize();

            if (item.Expanded && item is PopupItem && ((PopupItem)item).PopupControl != null)
            {
                control = ((PopupItem)item).PopupControl;
                if (control is MenuPanel)
                    ((MenuPanel)control).RecalcSize();
                else if (control is Bar)
                    ((Bar)control).RecalcLayout();
            }
		}

        protected virtual void NewItemAdded(BaseItem itemAdded)
        {
            BaseItem item=this.Component as BaseItem;
			System.Windows.Forms.Control control=item.GetOwner() as System.Windows.Forms.Control;
            
            //if(control is Bar)
            //    ((Bar)control).RecalcLayout();
            //else if(control is ExplorerBar)
            //    ((ExplorerBar)control).RecalcLayout();
            //else if(control is BarBaseControl)
            //    ((BarBaseControl)control).RecalcLayout();
            //else if(control is SideBar)
            //    ((SideBar)control).RecalcLayout();
			if(control is ItemControl)
                ((ItemControl)control).DesignerNewItemAdded();
            //else if (control is MenuPanel)
            //    ((MenuPanel)control).RecalcSize();
            
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
            IDesignerHost dh = (IDesignerHost)GetService(typeof(IDesignerHost));
            if (dh == null)
                return null;
            object comp = null;
            try
            {
                m_CreatingItem = true;
                comp = dh.CreateComponent(componentClass);
            }
            finally
            {
                m_CreatingItem = false;
            }

            return comp;
        }

        object IDesignerServices.CreateComponent(System.Type componentClass, string name)
        {
            IDesignerHost dh = (IDesignerHost)GetService(typeof(IDesignerHost));
            if (dh == null)
                return null;
            object comp = null;
            try
            {
                m_CreatingItem = true;
                comp = dh.CreateComponent(componentClass, name);
            }
            finally
            {
                m_CreatingItem = false;
            }

            return comp;
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
