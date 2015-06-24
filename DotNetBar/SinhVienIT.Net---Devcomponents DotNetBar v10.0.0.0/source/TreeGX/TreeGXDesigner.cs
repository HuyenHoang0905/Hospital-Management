using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace DevComponents.Tree
{
	/// <summary>
	/// Represents windows forms designer for the control.
	/// </summary>
	public class TreeGXDesigner:ParentControlDesigner
	{
		#region Private Variables
//		private Point m_MouseDownPosition=Point.Empty;
//		private bool m_IgnoreMouseUp=false;
//		private bool m_Capture=false;
		private bool m_DragDropStarted=false;

		const int WM_RBUTTONDOWN=0x0204;
		const int WM_LBUTTONDOWN=0x0201;
		const int WM_LBUTTONUP=0x0202;
		const int WM_RBUTTONUP=0x0205;
		const int WM_MOUSEMOVE=0x0200;
		const int WM_LBUTTONDBLCLK=0x0203;
		#endregion
		
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

//		public override DesignerVerbCollection Verbs 
//		{
//			get 
//			{
//				DesignerVerb[] verbs = new DesignerVerb[]
//					{
//						new DesignerVerb("Create Node", new EventHandler(CreateNode))};
//				return new DesignerVerbCollection(verbs);
//			}
//		}

		public override void OnSetComponentDefaults()
		{
			base.OnSetComponentDefaults();
			CreateNode(null);
			IDesignerHost dh=(IDesignerHost)GetService(typeof(IDesignerHost));
			if(dh==null)
				return;
			TreeUtilites.InitializeTree(this.Control as TreeGX,new ComponentFactory(dh));
		}

		private Node CreateNode(Node parentNode)
		{
			TreeGX tree=this.Control as TreeGX;

			IDesignerHost dh=(IDesignerHost)GetService(typeof(IDesignerHost));
			if(dh==null)
				return null;
            
			Node node=null;
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
						parentNode.Nodes.Add(node);

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

		#region Drag & Drop support
		/// <summary>
		/// Called after node has been selected by designer as response to mouse action
		/// </summary>
		/// <param name="node">Node that is selected</param>
		protected virtual void OnNodeSelected(Node node)
		{
			
		}
		
		/// <summary>
		/// Returns whether specified node can be dragged and dropped
		/// </summary>
		/// <param name="node">Node to verify</param>
		/// <returns>true if node can be dragged and dropped</returns>
		protected virtual bool CanDragNode(Node node)
		{
			return true;
		}
		
		protected override void OnMouseDragBegin(int x, int y)
		{
			TreeGX tree=this.Control as TreeGX;
			if(tree==null)
			{
				base.OnMouseDragBegin(x,y);
				return;
			}
			
			Point pos=tree.PointToClient(new Point(x,y));
			Node node = tree.GetNodeAt(pos);
			
			if(node!=null)
			{
				ISelectionService selection = (ISelectionService) this.GetService(typeof(ISelectionService));
				if(selection!=null)
				{
					ArrayList arr=new ArrayList(1);
					arr.Add(node);
					selection.SetSelectedComponents(arr,SelectionTypes.MouseDown);
					OnNodeSelected(node);
				}
				tree.SelectedNode = node;
			}
			else
				tree.SelectedNode = null;

			if(node==null || !CanDragNode(node))
			{
				if(node==null)
					base.OnMouseDragBegin(x,y);
				else
					this.Control.Capture = true; // Does same as base implementation
				return;
			}
			
			if(tree.StartDragDrop(node))
			{
				m_DragDropStarted=true;
//				DevComponents.Tree.Interop.WinA.RECT rect = new NativeFunctions.RECT(0,0,0,0);
//				NativeFunctions.GetWindowRect(bar.Handle, ref rect);
//				Rectangle r=Rectangle.FromLTRB(rect.Left, rect.Top, rect.Right, rect.Bottom);
//				Cursor.Clip=r;
				
				this.Control.Capture = true;
			}
			else
				base.OnMouseDragBegin(x,y);
		}
		
		protected override void OnMouseDragMove(int x, int y)
		{
			if(m_DragDropStarted)
			{
				TreeGX tree=this.Control as TreeGX;
				DragEventArgs de=new DragEventArgs(null,(int)Control.ModifierKeys,x,y,DragDropEffects.All,DragDropEffects.Move);
				tree.InternalDragOver(de);
			}
		}
		
		protected override void OnMouseDragEnd(bool cancel)
		{
			this.Control.Capture = false;
			Cursor.Clip = Rectangle.Empty;
			TreeGX tree=this.Control as TreeGX;

			if(m_DragDropStarted)
			{
				if(tree!=null && tree.IsDragDropInProgress)
				{
					if(cancel)
						tree.InternalDragLeave();
					else
					{
						IComponentChangeService change=this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;

						Node dragNode=tree.GetDragNode().Tag as Node;
						Node newParent=tree.GetDragNode().Parent;
						Node parent=dragNode.Parent;

						if(change!=null)
						{
							if(parent!=null)
								change.OnComponentChanging(this.Component,TypeDescriptor.GetProperties(parent).Find("Nodes",true));
							else
								change.OnComponentChanging(this.Component,TypeDescriptor.GetProperties(tree).Find("Nodes",true));

							if(newParent!=null)
								change.OnComponentChanging(this.Component,TypeDescriptor.GetProperties(newParent).Find("Nodes",true));
							else
								change.OnComponentChanging(this.Component,TypeDescriptor.GetProperties(tree).Find("Nodes",true));
						}

						tree.InternalDragDrop(new DragEventArgs(null,0,0,0,DragDropEffects.None,DragDropEffects.None));
					
						if(change!=null)
						{
							if(parent!=null)
								change.OnComponentChanged(this.Component,TypeDescriptor.GetProperties(parent).Find("Nodes",true),null,null);
							else
								change.OnComponentChanged(this.Component,TypeDescriptor.GetProperties(tree).Find("Nodes",true),null,null);
							if(newParent!=null)
								change.OnComponentChanged(this.Component,TypeDescriptor.GetProperties(newParent).Find("Nodes",true),null,null);
							else
								change.OnComponentChanged(this.Component,TypeDescriptor.GetProperties(tree).Find("Nodes",true),null,null);
						}
					}
				}
				cancel=true;
			}
			else
			{
				if(tree.SelectedNode!=null)
					cancel=true;
			}
			
			m_DragDropStarted = false;
			base.OnMouseDragEnd(cancel);
		}
		#endregion
		

		private void OnSelectionChanged(object sender,EventArgs e)
		{
			ISelectionService ss = (ISelectionService)sender;
			if(ss.PrimarySelection==this.Component)
			{
				TreeGX tree=this.Control as TreeGX;
				tree.SelectedNode=null;
			}
		}

		public void OnComponentRemoving(object sender,ComponentEventArgs e)
		{
			if(e.Component==this.Component)
			{
				IDesignerHost dh=(IDesignerHost)GetService(typeof(IDesignerHost));
				if(dh==null)
					return;
				
				ArrayList list = new ArrayList(this.AssociatedComponents);
				foreach(IComponent c in list)
					dh.DestroyComponent(c);
			}
		}

		/// <summary>
		/// Returns all components associated with this control
		/// </summary>
		public override ICollection AssociatedComponents
		{
			get
			{
				ArrayList c=new ArrayList(base.AssociatedComponents);
				TreeGX tree=this.Control as TreeGX;
				if(tree!=null)
				{
					foreach(Node node in tree.Nodes)
						GetNodesRecursive(node,c);

					foreach(ElementStyle style in tree.Styles)
						c.Add(style);

					if(tree.NodesConnector!=null)
						c.Add(tree.NodesConnector);
					if(tree.RootConnector!=null)
						c.Add(tree.RootConnector);
					if(tree.LinkConnector!=null)
						c.Add(tree.LinkConnector);
				}
				return c;
			}
		}

		private void GetNodesRecursive(Node parent,ArrayList c)
		{
			c.Add(parent);
			if(parent.ParentConnector!=null)
				c.Add(parent.ParentConnector);
			foreach(Node node in parent.Nodes)
			{
				c.Add(node);
				GetNodesRecursive(node,c);
			}
		}

		/// <summary>
		/// Selection support for items on container.
		/// </summary>
		protected override void WndProc(ref Message m)
		{
			switch(m.Msg)
			{
				case WM_LBUTTONDOWN:
				{
					if(OnMouseDown(ref m,MouseButtons.Left))
						return;
					break;
				}
				case WM_RBUTTONDOWN:
				{
					if(OnMouseDown(ref m,MouseButtons.Right))
						return;
					break;
				}
				case WM_LBUTTONUP:
				{
					if(OnMouseUp(ref m,MouseButtons.Left))
						return;
					break;
				}
				case WM_RBUTTONUP:
				{
					if(OnMouseUp(ref m,MouseButtons.Right))
						return;
					break;
				}
				case WM_MOUSEMOVE:
				{
					if(OnMouseMove(ref m))
						return;
					break;
				}
				case WM_LBUTTONDBLCLK:
				{
					if(OnMouseDoubleClick())
						return;
					break;
				}
			}

			base.WndProc(ref m);
		}

		private bool OnMouseDoubleClick()
		{
			bool processed=false;

			ISelectionService selection = (ISelectionService) this.GetService(typeof(ISelectionService));
			if(selection.PrimarySelection is Node && ((Node)selection.PrimarySelection).TreeControl==this.Control)
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

		protected virtual bool OnMouseDown(ref Message m, MouseButtons button)
		{
			
			TreeGX tree=this.Control as TreeGX;

			if(tree==null)
				return false;

			Point pos=tree.PointToClient(System.Windows.Forms.Control.MousePosition);
			//m_MouseDownPosition=pos;

			Node node=tree.GetNodeAt(pos);
			if(node!=null && button==MouseButtons.Right)
			{
				ISelectionService selection = (ISelectionService) this.GetService(typeof(ISelectionService));
				ArrayList arr=new ArrayList(1);
				arr.Add(node);
				selection.SetSelectedComponents(arr,SelectionTypes.MouseDown);

				this.OnContextMenu(System.Windows.Forms.Control.MousePosition.X,System.Windows.Forms.Control.MousePosition.Y);
				tree.SelectedNode=node;
				return true;
			}
					
			return false;
		}


		protected virtual bool OnMouseMove(ref Message m)
		{
//			TreeGX tree=this.Control as TreeGX;
//			Point posScreen=System.Windows.Forms.Control.MousePosition;
//			Point pos=tree.PointToClient(posScreen);
//
//			if(Control.MouseButtons==MouseButtons.Left)
//			{
//				if(m_DragDropStarted)
//				{
//					DragEventArgs de=new DragEventArgs(null,(int)Control.ModifierKeys,posScreen.X,posScreen.Y,DragDropEffects.All,DragDropEffects.Move);
//					tree.InternalDragOver(de);
//					return true;
//				}
//				if(Control.MouseButtons==MouseButtons.Left && !tree.IsCellEditing && tree.SelectedNode!=null && 
//					(Math.Abs(m_MouseDownPosition.X-pos.X)>=SystemInformation.DragSize.Width || Math.Abs(m_MouseDownPosition.Y-pos.Y)>=SystemInformation.DragSize.Height))
//				{
//					if(tree.StartDragDrop(tree.SelectedNode))
//					{
//						m_DragDropStarted=true;
//						System.Windows.Forms.Control c=System.Windows.Forms.Control.FromHandle(m.HWnd);
//						if(c!=null)
//						{
//							m_Capture=true;
//							c.Capture=true;
//						}
//						return true;
//					}
//					m_MouseDownPosition=Point.Empty;
//				}
//
//			}

			return false;
		}

		private bool OnMouseUp(ref Message m, MouseButtons button)
		{
//			if(m_Capture)
//			{
//				System.Windows.Forms.Control c=System.Windows.Forms.Control.FromHandle(m.HWnd);
//				if(c!=null)
//					c.Capture=false;
//				m_Capture=false;
//			}
//			m_DragDropStarted=false;
//
//			TreeGX tree=this.Control as TreeGX;
//			if(tree!=null && tree.IsDragDropInProgress)
//			{
//				IComponentChangeService change=this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
//
//				Node dragNode=tree.GetDragNode().Tag as Node;
//				Node newParent=tree.GetDragNode().Parent;
//				Node parent=dragNode.Parent;
//
//				if(change!=null)
//				{
//					if(parent!=null)
//						change.OnComponentChanging(this.Component,TypeDescriptor.GetProperties(parent).Find("Nodes",true));
//					else
//						change.OnComponentChanging(this.Component,TypeDescriptor.GetProperties(tree).Find("Nodes",true));
//
//					if(newParent!=null)
//						change.OnComponentChanging(this.Component,TypeDescriptor.GetProperties(newParent).Find("Nodes",true));
//					else
//						change.OnComponentChanging(this.Component,TypeDescriptor.GetProperties(tree).Find("Nodes",true));
//				}
//
//				tree.InternalDragDrop(new DragEventArgs(null,0,0,0,DragDropEffects.None,DragDropEffects.None));
//				
//				if(change!=null)
//				{
//					if(parent!=null)
//						change.OnComponentChanged(this.Component,TypeDescriptor.GetProperties(parent).Find("Nodes",true),null,null);
//					else
//						change.OnComponentChanged(this.Component,TypeDescriptor.GetProperties(tree).Find("Nodes",true),null,null);
//					if(newParent!=null)
//						change.OnComponentChanged(this.Component,TypeDescriptor.GetProperties(newParent).Find("Nodes",true),null,null);
//					else
//						change.OnComponentChanged(this.Component,TypeDescriptor.GetProperties(tree).Find("Nodes",true),null,null);
//				}
//
//			}
//
//			if(m_IgnoreMouseUp)
//			{
//				m_IgnoreMouseUp=false;
//				return true;
//			}

			return false;
		}

		public override SelectionRules SelectionRules
		{
			get
			{
				return (SelectionRules.AllSizeable | SelectionRules.Moveable | SelectionRules.Visible);
			}
		}
	}
}
