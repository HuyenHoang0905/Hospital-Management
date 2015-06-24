using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace DevComponents.AdvTree.Design
{
	/// <summary>
	/// Represents Windows Forms designer for Node object.
	/// </summary>
	public class NodeDesigner:ComponentDesigner
	{
		/// <summary>
		/// Initializes designer with given component.
		/// </summary>
		/// <param name="component">Component to initialize designer with.</param>
		public override void Initialize(IComponent component) 
		{
			base.Initialize(component);
			if(!component.Site.DesignMode)
				return;

			ISelectionService ss =(ISelectionService)GetService(typeof(ISelectionService));
			if(ss!=null)
				ss.SelectionChanged+=new EventHandler(OnSelectionChanged);
			
			Node n=component as Node;
			if(n!=null)
				this.Visible=n.Visible;
		}
		private void OnSelectionChanged(object sender, EventArgs e) 
		{
			ISelectionService ss = (ISelectionService)sender;
            if (this.Component != null && ss.PrimarySelection != this.Component)
            {
                Node node = this.Component as Node;
                if (ss.PrimarySelection is Node)
                {
                    Node selected = ss.PrimarySelection as Node;

                    if (selected.TreeControl != node.TreeControl)
                    {
                        node.TreeControl.SelectedNode = null;
                    }
                }
                else if (node != null && node.TreeControl != null)
                    node.TreeControl.SelectedNode = null;
            }
		}

		protected override void PreFilterProperties(System.Collections.IDictionary properties)
		{
			base.PreFilterProperties(properties);
			properties["Visible"] = TypeDescriptor.CreateProperty(typeof(NodeDesigner),(PropertyDescriptor)properties["Visible"], new Attribute[]
				{
					new DefaultValueAttribute(true),
					new BrowsableAttribute(true),
					new CategoryAttribute("Layout")});
//			properties["HostedControl"] = TypeDescriptor.CreateProperty(typeof(NodeDesigner),(PropertyDescriptor)properties["HostedControl"], new Attribute[]
//				{
//					new DefaultValueAttribute(null),
//					new BrowsableAttribute(true),
//					new CategoryAttribute("Behavior"),
//					new DescriptionAttribute("Indicates control hosted inside of the cell.")});

		}
		
		/// <summary>
		/// Gets or sets whether item is visible.
		/// </summary>
		[DefaultValue(true),Browsable(true),Category("Layout"),Description("Gets or sets whether node is visible.")]
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
		
//		/// <summary>
//		/// Gets or sets whether item is visible.
//		/// </summary>
//		[DefaultValue(null),Browsable(true),Category("Behavior"),Description("Indicates control hosted inside of the cell.")]
//		public Control HostedControl 
//		{
//			get 
//			{
//				Node node = this.Component as Node;
//				return node.HostedControl;
//			}
//			set 
//			{
//				Node node = this.Component as Node;
//				node.HostedControl = value;
//				if(value!=null)
//				{
//					IDesignerHost dh = this.GetService(typeof (IDesignerHost)) as IDesignerHost;
//					if(dh!=null)
//					{
//						AdvTreeDesigner ds = dh.GetDesigner(node.TreeControl) as AdvTreeDesigner;
//						if(ds!=null)
//							ds.HookControl(value);
//					}
//				}
//			}
//		}
		
		/// <summary>Returns design-time commands applicable to this designer.</summary>
		public override DesignerVerbCollection Verbs 
		{
			get 
			{
				DesignerVerb[] verbs = new DesignerVerb[]
					{
						new DesignerVerb("Create Child Node", new EventHandler(CreateNode)),
                        new DesignerVerb("Edit Cells...", new EventHandler(EditCells)),
                        new DesignerVerb("Edit Columns...", new EventHandler(EditColumns))
				};
				return new DesignerVerbCollection(verbs);
			}
		}

        private void EditCells(object sender, EventArgs e)
        {
            AdvTreeDesigner.EditValue(this, this.Component, "Cells");
        }

        private void EditColumns(object sender, EventArgs e)
        {
            AdvTreeDesigner.EditValue(this, this.Component, "NodesColumns");
        }

		private void CreateNode(object sender, EventArgs e)
		{
			Node node=CreateNode(this.Component as Node);
			if(node!=null)
			{
				ISelectionService sel = this.GetService(typeof (ISelectionService)) as ISelectionService;
				ArrayList list=new ArrayList(1);
				list.Add(node);
				if(sel!=null)
				{
					sel.SetSelectedComponents(list, SelectionTypes.MouseDown);
					node.TreeControl.SelectedNode = node;
				}
			}
		}
		
		private Node CreateNode(Node parentNode)
		{
			IDesignerHost dh=(IDesignerHost)GetService(typeof(IDesignerHost));
			if(dh==null)
				return null;
            
			Node node=null;
			AdvTree tree=((Node)this.Component).TreeControl;
			tree.BeginUpdate();
			try
			{
				IComponentChangeService change=this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
				if(change!=null)
				{
					if(parentNode!=null)
						change.OnComponentChanging(this.Component,TypeDescriptor.GetProperties(parentNode).Find("Nodes",true));
					else
						change.OnComponentChanging(this.Component,TypeDescriptor.GetProperties(tree).Find("Nodes",true));
				}

				node=dh.CreateComponent(typeof(Node)) as Node;
				if(node!=null)
				{
					node.Text=node.Name;
					node.Expanded = true;
					if(parentNode==null)
						tree.Nodes.Add(node);
					else
					{
						parentNode.Nodes.Add(node);
						parentNode.Expand();
						TypeDescriptor.GetProperties(node)["Style"].SetValue(node,parentNode.Style);
					}
					
					if(change!=null)
					{
						if(parentNode!=null)
							change.OnComponentChanged(this.Component,TypeDescriptor.GetProperties(parentNode).Find("Nodes",true),null,null);
						else
							change.OnComponentChanged(this.Component,TypeDescriptor.GetProperties(tree).Find("Nodes",true),null,null);
					}
				}
			}
			finally
			{
				tree.EndUpdate();
			}

			return node;
		}
	}
}
