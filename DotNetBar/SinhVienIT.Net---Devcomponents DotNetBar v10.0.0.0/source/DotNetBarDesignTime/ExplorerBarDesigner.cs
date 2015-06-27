using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.ComponentModel.Design.Serialization;
using System.CodeDom;
using System.Drawing;
using System.Collections;

namespace DevComponents.DotNetBar.Design
{
	/// <summary>
	/// Summary description for ExplorerBarDesigner.
	/// </summary>
	public class ExplorerBarDesigner:BarBaseControlDesigner
	{
		#region Internal Implementation
		public ExplorerBarDesigner():base()
		{
			this.EnableItemDragDrop=true;
		}

		public override void Initialize(IComponent component) 
		{
			base.Initialize(component);
			if(!component.Site.DesignMode)
				return;
			ExplorerBar bar=this.Control as ExplorerBar;
			if(bar!=null)
				bar.SetDesignMode();
		}

		public override DesignerVerbCollection Verbs 
		{
			get 
			{
				DesignerVerb[] verbs = new DesignerVerb[]
					{
						new DesignerVerb("Create Group", new EventHandler(OnAddGroup)),
						new DesignerVerb("Create Button", new EventHandler(OnAddButton))
					};
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
            if (this.Control is ExplorerBar)
            {
                ExplorerBar eb = this.Control as ExplorerBar;
                eb.ApplyDefaultSettings();
                //eb.ThemeAware = true;
                eb.StockStyle = eExplorerBarStockStyle.SystemColors;
            }
        }

		protected override BaseItem GetItemContainer()
		{
			ExplorerBar bar=this.Control as ExplorerBar;
			if(bar!=null)
				return bar.ItemsContainer;
			return null;
		}

		protected override void RecalcLayout()
		{
			ExplorerBar bar=this.GetItemContainerControl() as ExplorerBar;
			if(bar!=null)
				bar.RecalcLayout();
		}

		protected override void OnSubItemsChanging()
		{
			base.OnSubItemsChanging();
			IComponentChangeService change=this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
			if(change!=null)
			{
				ExplorerBar bar=this.GetItemContainerControl() as ExplorerBar;
				change.OnComponentChanging(this.Component,TypeDescriptor.GetProperties(bar).Find("Groups",true));
			}
		}

		protected override void OnSubItemsChanged()
		{
			base.OnSubItemsChanged();
			IComponentChangeService change=this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
			if(change!=null)
			{
				ExplorerBar bar=this.GetItemContainerControl() as ExplorerBar;
				change.OnComponentChanged(this.Component,TypeDescriptor.GetProperties(bar).Find("Groups",true),null,null);
			}
		}

//		/// <summary>
//		/// Triggered when some other component on the form is removed.
//		/// </summary>
//		protected override void ComponentRemoved(object sender, ComponentEventArgs e)
//		{
//			base.ComponentRemoved(sender,e);
//			if(e.Component is ExplorerBarGroupItem)
//			{
//				ExplorerBar bar=this.GetItemContainerControl() as ExplorerBar;
//				if(bar.Groups.Contains(e.Component as BaseItem))
//					bar.Groups.Remove(e.Component as BaseItem);
//				DestroySubItems(e.Component as BaseItem);
//				bar.RecalcLayout();
//			}
//		}
		#endregion

		#region Design-Time Item Creation
		private void OnAddGroup(object sender, EventArgs e)
		{
            IDesignerHost dh = (IDesignerHost)GetService(typeof(IDesignerHost));
            DesignerTransaction dt = dh.CreateTransaction();
            try
            {
                CreateGroup();
            }
            catch
            {
                dt.Cancel();
            }
            finally
            {
                if(!dt.Canceled)
                    dt.Commit();
            }
		}

		private ExplorerBarGroupItem CreateGroup()
		{
			ExplorerBar bar=this.Component as ExplorerBar;
			ExplorerBarGroupItem item=null;
			IDesignerHost dh=(IDesignerHost)GetService(typeof(IDesignerHost));

			if(bar!=null && dh!=null)
			{
				OnSubItemsChanging();
                try
                {
                    m_CreatingItem = true;
                    item = dh.CreateComponent(typeof(ExplorerBarGroupItem)) as ExplorerBarGroupItem;
                    if (item == null)
                        return null;
                    item.SetDefaultAppearance();
                    item.Text = "New Group";
                    item.Expanded = true;
                    bar.Groups.Add(item);
                    OnSubItemsChanged();
                }
                finally
                {
                    m_CreatingItem = false;
                }
			}

			return item;
		}

		private void OnAddButton(object sender, EventArgs e)
		{
			ExplorerBar bar=this.Component as ExplorerBar;
			ExplorerBarGroupItem group=null;

			IDesignerHost dh=(IDesignerHost)GetService(typeof(IDesignerHost));
			ISelectionService selection = (ISelectionService) this.GetService(typeof(ISelectionService));

            DesignerTransaction dt = dh.CreateTransaction();
            try
            {
                if (selection != null && selection.PrimarySelection is ExplorerBarGroupItem)
                {
                    group = selection.PrimarySelection as ExplorerBarGroupItem;
                }
                else if (bar.Groups.Count > 0)
                {
                    System.Drawing.Point point = bar.PointToClient(Form.MousePosition);
                    if (bar.Bounds.Contains(point))
                    {
                        foreach (BaseItem item in bar.Groups)
                        {
                            if (item.DisplayRectangle.Contains(point))
                            {
                                group = item as ExplorerBarGroupItem;
                                break;
                            }
                        }
                    }
                    if (group == null)
                    {
                        foreach (BaseItem item in bar.Groups)
                        {
                            if (item.Visible)
                            {
                                group = item as ExplorerBarGroupItem;
                                break;
                            }
                        }
                    }
                }

                if (group == null)
                    group = CreateGroup();

                IComponentChangeService change = this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
                if (change != null)
                    change.OnComponentChanging(this.Component, TypeDescriptor.GetProperties(group).Find("SubItems", true));
                try
                {
                    m_CreatingItem = true;
                    ButtonItem button = dh.CreateComponent(typeof(ButtonItem)) as ButtonItem;
                    if (button == null)
                        return;
                    ExplorerBarGroupItem.SetDesignTimeDefaults(button, group.StockStyle);
                    group.SubItems.Add(button);
                    if (change != null)
                        change.OnComponentChanged(this.Component, TypeDescriptor.GetProperties(group).Find("SubItems", true), null, null);
                }
                finally
                {
                    m_CreatingItem = false;
                }

                this.RecalcLayout();
            }
            catch
            {
                dt.Cancel();
            }
            finally
            {
                if(!dt.Canceled)
                    dt.Commit();
            }
		}

		protected override bool CanDragItem(BaseItem item)
		{
			if(item is ExplorerBarGroupItem)
				return false;
			return base.CanDragItem(item);
		}
		#endregion
	}
}
