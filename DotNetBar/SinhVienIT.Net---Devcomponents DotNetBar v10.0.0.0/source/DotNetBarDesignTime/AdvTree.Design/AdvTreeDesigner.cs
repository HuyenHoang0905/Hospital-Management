using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using DevComponents.DotNetBar;
using System.Drawing.Design;
using System.Reflection;

namespace DevComponents.AdvTree.Design
{
	/// <summary>
	/// Represents windows forms designer for the control.
	/// </summary>
	public class AdvTreeDesigner:ParentControlDesigner
	{
		#region Private Variables
//		private Point m_MouseDownPosition=Point.Empty;
//		private bool m_IgnoreMouseUp=false;
//		private bool m_Capture=false;
		private bool m_DragDropStarted=false;
		const string TEMP_NAME="tempDragDropItem";
		const int WM_RBUTTONDOWN=0x0204;
		const int WM_LBUTTONDOWN=0x0201;
//		const int WM_LBUTTONUP=0x0202;
//		const int WM_RBUTTONUP=0x0205;
//		const int WM_MOUSEMOVE=0x0200;
		const int WM_LBUTTONDBLCLK=0x0203;
		
		private Timer m_TimerAdded=null;
		private Timer m_TimerDragDrop=null;
		private bool m_DragLeave=false;
		private bool m_ControlRemoved=false;
		private DateTime m_JustAdded=DateTime.MinValue;
		private bool m_NewControlAdded=false;
		private Point m_MouseDownPoint=Point.Empty;
		#endregion
		
		#region Designer Implementation
		/// <summary>Initializes designer with given component.</summary>
		/// <param name="component">Component to initialize designer with.</param>
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
			{
				cc.ComponentRemoving+=new ComponentEventHandler(this.OnComponentRemoving);
			}
			
			if(component is Control)
			{
				((Control)component).ControlAdded+=new ControlEventHandler(this.ControlAdded);
				((Control)component).ControlRemoved+=new ControlEventHandler(this.ControlRemoved);
			}
			
			#if !TRIAL
			IDesignerHost dh = this.GetService(typeof (IDesignerHost)) as IDesignerHost;
			if(dh!=null)
				dh.LoadComplete+=new EventHandler(dh_LoadComplete);			
			#endif
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
		
			if(this.Control!=null)
			{
				this.Control.ControlAdded-=new ControlEventHandler(this.ControlAdded);
				this.Control.ControlRemoved-=new ControlEventHandler(this.ControlRemoved);
			}
			
			base.Dispose (disposing);
		}

		internal void HookControl(Control c)
		{
			this.HookChildControls(c);
		}

#if FRAMEWORK20
        public override void InitializeNewComponent(IDictionary defaultValues)
        {
            SetDefaults();
            base.InitializeNewComponent(defaultValues);
        }
#else
		/// <summary>Sets design-time defaults for component.</summary>
		public override void OnSetComponentDefaults()
		{
			base.OnSetComponentDefaults();
			SetDefaults();
		}
#endif
		
		private void SetDefaults()
		{
			CreateNode(null,true);
			IDesignerHost dh=(IDesignerHost)GetService(typeof(IDesignerHost));
			if(dh==null)
				return;
			Utilities.InitializeTree(this.Control as AdvTree,new ComponentFactory(dh));
            this.Control.Size = new Size(100, 100);
#if !TRIAL
			string key=GetLicenseKey();
			AdvTree tree=this.Control as AdvTree;
			tree.LicenseKey=key;
#endif
		}

		private Node CreateNode(Node parentNode, bool addToCollections)
		{
			AdvTree tree=this.Control as AdvTree;

			IDesignerHost dh=(IDesignerHost)GetService(typeof(IDesignerHost));
			if(dh==null)
				return null;
            
			Node node=null;
			tree.BeginUpdate();
			try
			{
				IComponentChangeService change=this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
				if(addToCollections)
				{
					if(change!=null)
					{
						if(parentNode!=null)
							change.OnComponentChanging(this.Component,TypeDescriptor.GetProperties(parentNode).Find("Nodes",true));
						else
							change.OnComponentChanging(this.Component,TypeDescriptor.GetProperties(tree).Find("Nodes",true));
					}
				}

				node=dh.CreateComponent(typeof(Node)) as Node;
				
				if(node!=null)
				{
					node.Text=node.Name;
					node.Expanded = true;
					if(addToCollections)
					{
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
			}
			finally
			{
				tree.EndUpdate();
			}

			return node;
		}
		
		private void OnSelectionChanged(object sender,EventArgs e)
		{
			ISelectionService ss = (ISelectionService)sender;
            if (ss.PrimarySelection == this.Component)
            {
                AdvTree tree = this.Control as AdvTree;
                tree.SelectedNode = null;
            }
		}

		/// <summary>Called when component is about to be removed from designer.</summary>
		/// <param name="sender">Event sender.</param>
		/// <param name="e">Event arguments.</param>
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
			else if(e.Component is Node && ((Node)e.Component).TreeControl==this.Control)
			{
				OnNodeRemoving(e.Component as Node);
			}
		}

		private void OnNodeRemoving(Node node)
		{
			IComponentChangeService cc=this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
			IDesignerHost dh=(IDesignerHost)GetService(typeof(IDesignerHost));

            if (node.Parent != null)
            {
                Node parent = node.Parent;
                if (cc != null)
                    cc.OnComponentChanging(parent, TypeDescriptor.GetProperties(parent)["Nodes"]);
                node.Remove();
                if (cc != null)
                    cc.OnComponentChanged(parent, TypeDescriptor.GetProperties(parent)["Nodes"], null, null);
            }
            else
            {
                // Root node
                AdvTree tree = this.Control as AdvTree;
                if (cc != null)
                    cc.OnComponentChanging(tree, TypeDescriptor.GetProperties(tree)["Nodes"]);
                node.Remove();
                if (cc != null)
                    cc.OnComponentChanged(tree, TypeDescriptor.GetProperties(tree)["Nodes"], null, null);
            }

			if(node.Nodes.Count>0)
			{
				Node[] nodes=new Node[node.Nodes.Count];
				node.Nodes.CopyTo(nodes);
				foreach(Node n in nodes)
				{
					n.Remove();

					if(n.ParentConnector!=null && dh!=null)
						dh.DestroyComponent(n.ParentConnector);

					if(dh!=null)
						dh.DestroyComponent(n);
				}
			}

			this.RecalcLayout();
		}

		/// <summary>
		/// Returns all components associated with this control
		/// </summary>
		public override ICollection AssociatedComponents
		{
			get
			{
				ArrayList c=new ArrayList(base.AssociatedComponents);
				AdvTree tree=this.Control as AdvTree;
				if(tree!=null)
				{
					foreach(Node node in tree.Nodes)
						GetNodesRecursive(node,c);

					foreach(ElementStyle style in tree.Styles)
						c.Add(style);

					if(tree.NodesConnector!=null)
						c.Add(tree.NodesConnector);
                    //if(tree.RootConnector!=null)
                    //    c.Add(tree.RootConnector);
                    //if(tree.LinkConnector!=null)
                    //    c.Add(tree.LinkConnector);
					if(tree.SelectedPathConnector!=null)
						c.Add(tree.SelectedPathConnector);
                    foreach (ColumnHeader ch in tree.Columns)
                    {
                        c.Add(ch);
                    }
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
                if (node.Cells.Count > 1)
                {
                    for (int i = 1; i < node.Cells.Count; i++)
                    {
                        c.Add(node.Cells[i]);
                    }
                }
                if (node.NodesColumns.Count > 0)
                {
                    foreach (ColumnHeader ch in node.NodesColumns)
                    {
                        c.Add(ch);
                    }
                }
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
//				case WM_LBUTTONUP:
//				{
//					if(OnMouseUp(ref m,MouseButtons.Left))
//						return;
//					break;
//				}
//				case WM_RBUTTONUP:
//				{
//					if(OnMouseUp(ref m,MouseButtons.Right))
//						return;
//					break;
//				}
//				case WM_MOUSEMOVE:
//				{
//					if(OnMouseMove(ref m))
//						return;
//					break;
//				}
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
			
			AdvTree tree=this.Control as AdvTree;

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

				tree.SelectedNode=node;
				this.OnContextMenu(System.Windows.Forms.Control.MousePosition.X,System.Windows.Forms.Control.MousePosition.Y);

				return true;
			}
					
			return false;
		}

        /// <summary>Specifies selection rules for designer.</summary>
        public override SelectionRules SelectionRules
        {
            get
            {
                return (SelectionRules.AllSizeable | SelectionRules.Moveable | SelectionRules.Visible);
            }
        }
		#endregion
		
		#region Drag & Drop External Control Support
		private void DestroyComponent(IComponent c)
		{
			IDesignerHost dh=(IDesignerHost)GetService(typeof(IDesignerHost));
			if(dh==null)
				return;
			dh.DestroyComponent(c);
		}
		
		private void RecalcLayout()
		{
			AdvTree tree = this.Control as AdvTree;
			if(tree!=null)
			{
				tree.RecalcLayout();
				tree.Refresh();
			}
		}
		private void ControlAdded(object sender, ControlEventArgs e)
		{
			if(!m_NewControlAdded)
			{
				if(!m_NewControlAdded)
				{
					if(!OnControlAdded(e))
						return;
				}
				else
					return;
			}

			m_TimerAdded=new Timer();
			m_TimerAdded.Tick+=new EventHandler(this.TimerTick);
			m_TimerAdded.Interval=50;
			m_TimerAdded.Enabled=true;
			m_TimerAdded.Start();
			m_NewControlAdded=false;
		}

		/// <summary>
		/// Called after control has been added to container but not through drag & drop. Control added could also be
		/// internal control by the bar container.
		/// </summary>
		/// <param name="e">Event arguments</param>
		/// <returns>true if acted upon this new control otherwise false.</returns>
		protected virtual bool OnControlAdded(ControlEventArgs e)
		{
			return false;
		}

		private void ControlRemoved(object sender, ControlEventArgs e)
		{
			IDesignerHost dh=(IDesignerHost)GetService(typeof(IDesignerHost));
			if(dh==null || dh.Loading)
				return;

			if(m_JustAdded!=DateTime.MinValue && DateTime.Now.Subtract(m_JustAdded).Seconds<2)
			{
				m_JustAdded=DateTime.MinValue;
				return;
			}
			m_JustAdded=DateTime.MinValue;
			if(m_DragLeave)
				ControlRemoved(e.Control);
			else if(m_TimerDragDrop!=null)
				m_ControlRemoved=true;
			else
			{
				ISelectionService ss =(ISelectionService)GetService(typeof(ISelectionService));
				if(ss!=null && ss.PrimarySelection==e.Control && Utilities.FindNodeForControl(this.Control as AdvTree, e.Control)!=null)
				{
					ControlRemoved(e.Control);
				}
			}
		}

		private void ControlRemoved(Control control)
		{
			AdvTree tree = this.Control as AdvTree;
			
			if(control!=null)
			{
				Node node=Utilities.FindNodeForControl(tree,control);
				if(node!=null)
				{
					if(m_DragDropStarted)
					{
						tree.InternalDragLeave();
						m_DragDropStarted = false;
					}
					
					if(node.Parent!=null)
					{
						Node parent=node.Parent;
						IComponentChangeService cc = this.GetService(typeof (IComponentChangeService)) as IComponentChangeService;
						if(cc!=null)
							cc.OnComponentChanging(parent, TypeDescriptor.GetProperties(parent)["Nodes"]);
						node.Remove();
						if(cc!=null)
							cc.OnComponentChanged(parent, TypeDescriptor.GetProperties(parent)["Nodes"],null,null);
					}
					
					this.DestroyComponent(node);
					this.RecalcLayout();
				}
			}
		}

		private void TimerTick(object sender, EventArgs e)
		{
			m_TimerAdded.Stop();
			m_TimerAdded.Enabled=false;
			m_TimerAdded=null;
			this.RecalcLayout();
			ISelectionService sel=(ISelectionService)this.GetService(typeof(ISelectionService));
			if(sel!=null && sel.PrimarySelection is Control && this.Control.Controls.Contains((Control)sel.PrimarySelection))
			{
				IComponentChangeService cc=this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
				cc.OnComponentChanged(sel.PrimarySelection,null,null,null);
			}
		}
		
		private void TimerTickDragDrop(object sender, EventArgs e)
		{
			Point p=this.Control.PointToClient(System.Windows.Forms.Control.MousePosition);
			if(this.Control.Bounds.Contains(p))
				m_DragLeave=false;
			else
				m_DragLeave=true;

			if(System.Windows.Forms.Control.MouseButtons!=MouseButtons.Left)
			{
				m_TimerDragDrop.Enabled=false;
				m_TimerDragDrop.Stop();
				m_TimerDragDrop.Tick-=new EventHandler(TimerTickDragDrop);
				m_TimerDragDrop.Dispose();
				m_TimerDragDrop=null;
				if(m_ControlRemoved)
				{
					m_ControlRemoved=false;
					ISelectionService sel=this.GetService(typeof(ISelectionService)) as ISelectionService;
					if(sel!=null && sel.PrimarySelection is Control)
						ControlRemoved((Control)sel.PrimarySelection);
				}
			}
		}
		
		protected override void OnDragLeave(EventArgs e)
		{
			if(m_DragDropStarted)
			{
				AdvTree tree=this.Control as AdvTree;
				tree.InternalDragLeave();
				m_DragDropStarted = false;
			}
			base.OnDragLeave (e);
		}
		
		protected override void OnDragOver(DragEventArgs de)
		{
			AdvTree tree=this.Control as AdvTree;
			if(tree==null)
			{
				base.OnDragOver(de);
				return;
			}
			
			if(m_DragDropStarted)
			{
				DragEventArgs d=new DragEventArgs(null,de.KeyState,de.X,de.Y,DragDropEffects.All,DragDropEffects.Move);
				tree.InternalDragOver(d);
				de.Effect=DragDropEffects.Move;
				return;
			}

			ISelectionService sel=(ISelectionService)this.GetService(typeof(ISelectionService));
			if(sel!=null && sel.PrimarySelection!=this.Component)
			{
				if(sel.PrimarySelection is Control && this.Control.Controls.Contains((Control)sel.PrimarySelection))
				{			
					Node node=Utilities.FindNodeForControl(tree,sel.PrimarySelection as Control);
					if(node!=null)
					{
						if(tree.StartDragDrop(node))
						{
							if(m_TimerDragDrop==null)
							{
								m_TimerDragDrop=new Timer();
								m_TimerDragDrop.Tick+=new EventHandler(this.TimerTickDragDrop);
								m_TimerDragDrop.Interval=100;
								m_TimerDragDrop.Enabled=true;
								m_TimerDragDrop.Start();
							}
							m_DragDropStarted = true;
						}
					}
					return;
				}
				else if(sel.SelectionCount>1)
				{
					de.Effect=DragDropEffects.None;
					return;
				}
				else if(sel.PrimarySelection is Control && ((Control)sel.PrimarySelection).Parent!=null)
				{
					// New control being added to the container
					Node node=new Node();
					node.Name=TEMP_NAME;
					node.Text = ((Control) sel.PrimarySelection).Name;
					if(tree.StartDragDrop(node))
						m_DragDropStarted = true;
				}
			}
			
			base.OnDragOver (de);
		}

		protected override void OnDragDrop(DragEventArgs de)
		{
			AdvTree tree=this.Control as AdvTree;
			if(tree==null)
			{
				base.OnDragDrop(de);
				return;
			}
			
			ISelectionService sel=(ISelectionService)this.GetService(typeof(ISelectionService));
            IComponentChangeService change = this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;

			if(sel!=null && sel.PrimarySelection is Control && this.Control.Controls.Contains((Control)sel.PrimarySelection))
			{
				de.Effect=DragDropEffects.Move;
				tree.InternalDragDrop(new DragEventArgs(null,0,0,0,DragDropEffects.Move,DragDropEffects.All));

                
                if (change != null)
                    change.OnComponentChanged(this.Control, TypeDescriptor.GetProperties(tree).Find("Nodes", true), null, null);
			}
			else
			{
				if(sel.SelectionCount>1)
				{
					de.Effect=DragDropEffects.None;
					return;
				}
				else
				{
					Node node = tree.GetDragNode();
                    Node[] dragNodes = node.Tag as Node[];
                    Node dragNode = null;
                    if (dragNodes != null && dragNodes.Length > 0)
                        dragNode = dragNodes[0];

                    if (node != null && dragNode != null && dragNode.Name == TEMP_NAME)
                    {
                        m_JustAdded = DateTime.Now;
                        dragNode = CreateNode(null, false);
                        TypeDescriptor.GetProperties(dragNode)["HostedControl"].SetValue(dragNode, sel.PrimarySelection as Control);
                        TypeDescriptor.GetProperties(dragNode)["Text"].SetValue(dragNode, dragNode.HostedControl.Name);
                        node.Tag = new Node[] { dragNode };

                        Node newParent=tree.GetDragNode().Parent;
						Node oldParent=dragNode.Parent;

                        tree.InternalDragDrop(new DragEventArgs(null, 0, 0, 0, DragDropEffects.Move, DragDropEffects.All));
                        m_NewControlAdded = true;
                        m_DragDropStarted = false;
                        newParent = dragNode.Parent;
                        
                        if (change != null)
                        {
                            if (oldParent != null)
                                change.OnComponentChanged(this.Component, TypeDescriptor.GetProperties(oldParent).Find("Nodes", true), null, null);
                            else
                                change.OnComponentChanged(this.Component, TypeDescriptor.GetProperties(tree).Find("Nodes", true), null, null);
                            if (newParent != null)
                                change.OnComponentChanged(this.Component, TypeDescriptor.GetProperties(newParent).Find("Nodes", true), null, null);
                            else
                                change.OnComponentChanged(this.Component, TypeDescriptor.GetProperties(tree).Find("Nodes", true), null, null);
                        }
                        tree.RecalcLayout();
                    }
				}
			}
		
			base.OnDragDrop(de);
		}
		#endregion

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
			AdvTree tree=this.Control as AdvTree;
			if(tree==null)
			{
				base.OnMouseDragBegin(x,y);
				return;
			}
			
			Point pos=tree.PointToClient(new Point(x,y));
			Node node = tree.GetNodeAt(pos);
			
			if(node!=null)
			{
                Rectangle expandedRect = Display.NodeDisplay.GetNodeRectangle(eNodeRectanglePart.ExpandHitTestBounds, node, tree.NodeDisplay.Offset);
				if(!expandedRect.IsEmpty && expandedRect.Contains(pos))
				{
					node.Expanded=!node.Expanded;
					return;
				}
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
			
			m_MouseDownPoint=new Point(x,y);
			this.Control.Capture = true;
		}
		
		protected override void OnMouseDragMove(int x, int y)
		{
			AdvTree tree=this.Control as AdvTree;
			if(!m_MouseDownPoint.IsEmpty && tree.SelectedNode!=null)
			{
				if(Math.Abs(m_MouseDownPoint.X-x)>=SystemInformation.DragSize.Width || Math.Abs(m_MouseDownPoint.Y-y)>=SystemInformation.DragSize.Height)
				{
					tree.StartDragDrop(tree.SelectedNode);
					m_MouseDownPoint=Point.Empty;
					m_DragDropStarted=true;
					return;
				}
			}

			if(m_DragDropStarted)
			{
				DragEventArgs de=new DragEventArgs(null,(int)Control.ModifierKeys,x,y,DragDropEffects.All,DragDropEffects.Move);
				tree.InternalDragOver(de);
			}
		}
		
		protected override void OnMouseDragEnd(bool cancel)
		{
			this.Control.Capture = false;
			Cursor.Clip = Rectangle.Empty;
			AdvTree tree=this.Control as AdvTree;
			if(m_DragDropStarted)
			{
				if(tree!=null && tree.IsDragDropInProgress)
				{
					if(cancel)
						tree.InternalDragLeave();
					else if(tree.GetDragNode()!=null)
					{
						IComponentChangeService change=this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;

                        Node[] dragNodes = tree.GetDragNode().Tag as Node[];
                        Node dragNode = null;
                        if (dragNodes != null && dragNodes.Length > 0)
                            dragNode = dragNodes[0];
						if(dragNode!=null)
						{
							Node newParent=tree.GetDragNode().Parent;
							Node parent=dragNode.Parent;

							if(change!=null)
							{
								if(parent!=null)
									change.OnComponentChanging(this.Component,TypeDescriptor.GetProperties(parent).Find("Nodes",true));
								else
									change.OnComponentChanging(this.Component,TypeDescriptor.GetProperties(tree).Find("Nodes",true));
							}

							tree.InternalDragDrop(new DragEventArgs(null,0,0,0,DragDropEffects.None,DragDropEffects.None));

                            newParent = dragNode.Parent;

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
				}
				cancel=true;
			}
			else
			{
                if (tree.SelectedNode != null)
                    cancel = true;
			}
			
			m_DragDropStarted = false;
			base.OnMouseDragEnd(cancel);
		}
#if FRAMEWORK20
        private DesignerActionListCollection _ActionLists = null;
        public override DesignerActionListCollection ActionLists
        {
            get
            {
                if (this._ActionLists == null)
                {
                    this._ActionLists = new DesignerActionListCollection();
                    this._ActionLists.Add(new AdvTreeActionList(this));
                }
                return this._ActionLists;
            }
        }
#else
        /// <summary>Returns design-time commands applicable to this designer.</summary>
        public override DesignerVerbCollection Verbs
        {
            get
            {
                DesignerVerb[] verbs = new DesignerVerb[]
					{
						new DesignerVerb("Create Node", new EventHandler(CreateNode)),
                        new DesignerVerb("Edit Columns...", new EventHandler(EditColumns))
				};
                return new DesignerVerbCollection(verbs);
            }
        }
#endif

        private void EditColumns(object sender, EventArgs e)
        {
            EditColumns();
        }

        internal void EditColumns()
        {
            AdvTree tree = this.Component as AdvTree;
            EditValue(this, tree, "Columns");
        }

        internal static object EditValue(ComponentDesigner designer, object objectToChange, string propName)
        {
            Type t = Type.GetType("System.Windows.Forms.Design.EditorServiceContext");
            if (t == null) t = Type.GetType("System.Windows.Forms.Design.EditorServiceContext, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
            if (t != null)
            {
                MethodInfo mi = t.GetMethod("EditValue");
                if (mi != null)
                    return mi.Invoke(null, new object[] { designer, objectToChange, propName });
            }
            return null;
        }

        private void CreateNode(object sender, EventArgs e)
        {
            CreateNode();
        }

        internal void CreateNode()
        {
            Node node = CreateNode(this.Component as AdvTree);
            if (node != null)
            {
                ISelectionService sel = this.GetService(typeof(ISelectionService)) as ISelectionService;
                ArrayList list = new ArrayList(1);
                list.Add(node);
                if (sel != null)
                {
                    sel.SetSelectedComponents(list, SelectionTypes.MouseDown);
                    node.TreeControl.SelectedNode = node;
                }
            }
        }

        private Node CreateNode(AdvTree tree)
        {
            IDesignerHost dh = (IDesignerHost)GetService(typeof(IDesignerHost));
            if (dh == null)
                return null;

            Node node = null;
            tree.BeginUpdate();
            try
            {
                IComponentChangeService change = this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
                if (change != null)
                {
                    change.OnComponentChanging(this.Component, TypeDescriptor.GetProperties(tree).Find("Nodes", true));
                }

                node = dh.CreateComponent(typeof(Node)) as Node;
                if (node != null)
                {
                    node.Text = node.Name;
                    node.Expanded = true;
                    tree.Nodes.Add(node);
                    
                    if (change != null)
                        change.OnComponentChanged(this.Component, TypeDescriptor.GetProperties(tree).Find("Nodes", true), null, null);
                }
            }
            finally
            {
                tree.EndUpdate();
            }

            return node;
        }

        protected override bool GetHitTest(Point pt)
        {
            AdvTree tree = this.Control as AdvTree;
            if (tree != null && tree.IsHandleCreated && tree.AutoScroll)
            {
                Point p = tree.PointToClient(pt);
                if (tree.VScrollBar != null && tree.VScrollBar.Bounds.Contains(p))
                    return true;
                if (tree.HScrollBar != null && tree.HScrollBar.Bounds.Contains(p))
                    return true;
                
            }
            return base.GetHitTest(pt);
        }
		#endregion
		
		#region Licensing Stuff
		#if !TRIAL
		private string GetLicenseKey()
		{
			string key="";
			Microsoft.Win32.RegistryKey regkey=Microsoft.Win32.Registry.LocalMachine;
			regkey=regkey.OpenSubKey("Software\\DevComponents\\Licenses",false);
			if(regkey!=null)
			{
				object keyValue=regkey.GetValue("DevComponents.DotNetBar.DotNetBarManager2");
				if(keyValue!=null)
					key=keyValue.ToString();
			}
			return key;
		}
		private void dh_LoadComplete(object sender, EventArgs e)
		{
			IDesignerHost dh = this.GetService(typeof (IDesignerHost)) as IDesignerHost;
			if(dh!=null)
				dh.LoadComplete-=new EventHandler(dh_LoadComplete);
			
			string key=GetLicenseKey();
            
			AdvTree tree=this.Control as AdvTree;
            if (key != "" && tree != null && tree.LicenseKey == "" && tree.LicenseKey != key)
                TypeDescriptor.GetProperties(tree)["LicenseKey"].SetValue(tree, key);
		}
		#endif
		#endregion
	}
}
