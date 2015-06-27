using System;
using System.ComponentModel;
using System.ComponentModel.Design;

namespace DevComponents.Tree
{
	/// <summary>
	/// Represents Windows Forms designer for Node object.
	/// </summary>
	public class NodeDesigner:ComponentDesigner
	{
		/// <summary>
		/// Creates news instance of the class.
		/// </summary>
		public NodeDesigner()
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
		}
		private void OnSelectionChanged(object sender, EventArgs e) 
		{
			ISelectionService ss = (ISelectionService)sender;
			if(ss.PrimarySelection!=this.Component)
			{
				Node node = this.Component as Node;
				if(ss.PrimarySelection is Node)
				{
					Node selected=ss.PrimarySelection as Node;
					
					if(selected.TreeControl!=node.TreeControl)
					{
						node.TreeControl.SelectedNode = null;
					}
				}
				else
                    node.TreeControl.SelectedNode=null;
			}
		}

		public override DesignerVerbCollection Verbs 
		{
			get 
			{
				DesignerVerb[] verbs = new DesignerVerb[]
					{
						new DesignerVerb("Create Child Node", new EventHandler(CreateNode))
				};
				return new DesignerVerbCollection(verbs);
			}
		}

		private void CreateNode(object sender, EventArgs e)
		{
			CreateNode(this.Component as Node);
		}
		
		private Node CreateNode(Node parentNode)
		{
			IDesignerHost dh=(IDesignerHost)GetService(typeof(IDesignerHost));
			if(dh==null)
				return null;
            
			Node node=null;
			TreeGX tree=((Node)this.Component).TreeControl;
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
