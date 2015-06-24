using System;
using System.Drawing;
using DevComponents.Tree.Layout;

namespace DevComponents.Tree.Display
{
	/// <summary>
	/// Summary description for NodeDisplay.
	/// </summary>
	public class NodeDisplay
	{
		#region Private Variables
		private Point m_Offset=Point.Empty;
		private Point m_LockedOffset=Point.Empty;
		private TreeGX m_Tree=null;
#if !TRIAL
		internal static bool keyInvalid=false;
#endif
		#endregion
		/// <summary>Creates new instance of the class</summary>
		/// <param name="tree">Object to initialize class with.</param>
		public NodeDisplay(TreeGX tree)
		{
			m_Tree=tree;
		}

		/// <summary>
		/// Paints the layout on canvas.
		/// </summary>
		public virtual void Paint(Graphics g, Rectangle clipRectangle)
		{
		}

		/// <summary>
		/// Gets or sets the offset of the tree content relative to the size of the container control.
		/// </summary>
		public virtual Point Offset
		{
			get
			{
				if(!m_LockedOffset.IsEmpty)
					return m_LockedOffset;
				
				Node displayNode=m_Tree.GetDisplayRootNode();
				if(displayNode==null)
					return Point.Empty;;
				
				Size nodesSize = m_Tree.GetScreenSize(new Size(m_Tree.NodeLayout.Width, m_Tree.NodeLayout.Height));
				// Must change offset value since map layout assumes that root node is in the center
				// of container control.
				if(m_Tree.NodeLayout is NodeMapLayout && m_Tree.AutoScroll)
				{
					Point p=m_Offset;
					Point displayNodeChildLocation =
						new Point(Math.Abs((int)(displayNode.ChildNodesBounds.Left * m_Tree.Zoom)), Math.Abs((int)(displayNode.ChildNodesBounds.Top * m_Tree.Zoom)));
					
					p.Offset(displayNodeChildLocation.X, displayNodeChildLocation.Y);
					
					int offsetX=0, offsetY=0;
					if(m_Tree.CenterContent && m_Tree.Height>nodesSize.Height)
					{
						offsetY=(m_Tree.Height-nodesSize.Height)/2;
					}
					
					if(m_Tree.CenterContent && m_Tree.Width>nodesSize.Width)
					{
						offsetX=(m_Tree.Width-nodesSize.Width)/2;
					}
					
					p.Offset(offsetX,offsetY);
					return m_Tree.GetLayoutPosition(p);
				}
				else if(m_Tree.NodeLayout is Layout.NodeDiagramLayout && m_Tree.AutoScroll)
				{
					Point p=m_Offset;
					int offsetX=0, offsetY=0;
					if(m_Tree.CenterContent && m_Tree.Height>nodesSize.Height)
						offsetY=(m_Tree.Height-nodesSize.Height)/2;
					if(m_Tree.CenterContent && m_Tree.Width>nodesSize.Width)
						offsetX=(m_Tree.Width-nodesSize.Width)/2;
					p.Offset(offsetX,offsetY);
					return m_Tree.GetLayoutPosition(p);
				}
				else
					return m_Offset;
			}
			set {m_Offset=value;}
		}

		/// <summary>Gets or sets whether offset is locked, i.e. cannot be changed.</summary>
		public bool LockOffset
		{
			get {return (!m_LockedOffset.IsEmpty);}
			set
			{
				if(value)
					m_LockedOffset=this.Offset;
				else
					m_LockedOffset=Point.Empty;
			}
		}
		
		/// <summary>
		/// Sets locked offset to specific value. Point.Empty means there is no locked offset set.
		/// </summary>
		/// <param name="p">New locked offset.</param>
		public void SetLockedOffset(Point p)
		{
			m_LockedOffset=p;
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public Point GetLockedOffset()
		{
			return m_LockedOffset;
		}

		/// <summary>
		/// Returns the default offset for the tree content relative to the size of the container.
		/// </summary>
		public virtual Point DefaultOffset
		{
			get
			{
				Node displayNode=m_Tree.GetDisplayRootNode();
				if(displayNode==null)
					return Point.Empty;;
				
				if(m_Tree.NodeLayout is NodeMapLayout && m_Tree.Nodes.Count>0)
				{
					if(!m_Tree.CenterContent)
						return new Point(Math.Abs(displayNode.ChildNodesBounds.Left),Math.Abs(displayNode.ChildNodesBounds.Top));
					else
                        return new Point(m_Tree.SelectionBoxSize+(m_Tree.Width - m_Tree.SelectionBoxSize * 2 - m_Tree.NodeLayout.Width) / 2 + Math.Abs(displayNode.ChildNodesBounds.Left),
                            m_Tree.SelectionBoxSize + (m_Tree.Height - m_Tree.SelectionBoxSize * 2 - m_Tree.NodeLayout.Height) / 2 + Math.Abs(displayNode.ChildNodesBounds.Top));
				}
				if(m_Tree.NodeLayout is Layout.NodeDiagramLayout)
				{
					if(!m_Tree.CenterContent)
						return m_Tree.ClientRectangle.Location;
					else
						return new Point((m_Tree.Width-m_Tree.NodeLayout.Width)/2,(m_Tree.Height-m_Tree.NodeLayout.Height)/2);
				}
				else
					return m_Tree.ClientRectangle.Location;
			}
		}

		/// <summary>
		/// Gets or sets the reference to the tree control managed by display class.
		/// </summary>
		protected virtual TreeGX Tree
		{
			get {return m_Tree;}
			set {m_Tree=value;}
		}

		internal static Rectangle GetNodeRectangle(eNodeRectanglePart part, Node node, Point offset)
		{
			Rectangle r=Rectangle.Empty;
			if(part==eNodeRectanglePart.CellsBounds)
			{
				r=node.CellsBoundsRelative;
				if(!r.IsEmpty)
				{
					r.Offset(offset);
					r.Offset(node.BoundsRelative.Location);
				}
			}
			else if(part==eNodeRectanglePart.ExpandBounds)
			{
				r=node.ExpandPartRectangleRelative;
				if(!r.IsEmpty)
				{
					r.Offset(offset);
					r.Offset(node.BoundsRelative.Location);
				}
			}
			else if(part==eNodeRectanglePart.CommandBounds)
			{
				r=node.CommandBoundsRelative;
				if(!r.IsEmpty)
				{
					r.Offset(offset);
					r.Offset(node.BoundsRelative.Location);
				}
			}	
			else if(part==eNodeRectanglePart.NodeContentBounds)
			{
				r=node.ContentBounds;
				if(!r.IsEmpty)
				{
					r.Offset(offset);
					r.Offset(node.BoundsRelative.Location);
				}
			}
			else if(part==eNodeRectanglePart.NodeBounds)
			{
				r=node.BoundsRelative;
				if(!r.IsEmpty)
					r.Offset(offset);
			}
			else if(part==eNodeRectanglePart.ChildNodeBounds)
			{
				r=node.ChildNodesBounds;
				if(!r.IsEmpty)
				{
					//r.Offset(node.Bounds.Location);
					r.Offset(offset);
				}
			}
			return r;
		}

		internal static Rectangle GetCellRectangle(eCellRectanglePart part, Cell cell, Point offset)
		{
			Rectangle r=Rectangle.Empty;
			
			// If cell parent is not assigned rectangle cannot be returned.
			if(cell.Parent==null)
				return r;

			if(part==eCellRectanglePart.CheckBoxBounds)
			{
				r=cell.CheckBoxBoundsRelative;
				if(!r.IsEmpty)
				{
					r.Offset(offset);
					r.Offset(cell.Parent.BoundsRelative.Location);
				}
			}
			else if(part==eCellRectanglePart.ImageBounds)
			{
				r=cell.ImageBoundsRelative;
				if(!r.IsEmpty)
				{
					r.Offset(offset);
					r.Offset(cell.Parent.BoundsRelative.Location);
				}
			}
			else if(part==eCellRectanglePart.TextBounds)
			{
				r=cell.TextContentBounds;
				if(!r.IsEmpty)
				{
					r.Offset(offset);
					r.Offset(cell.Parent.BoundsRelative.Location);
				}
			}
			else if(part==eCellRectanglePart.CellBounds)
			{
				r=cell.BoundsRelative;
				if(!r.IsEmpty)
				{
					r.Offset(offset);
				}
			}
			return r;
		}

		internal static bool DrawExpandPart(Node node)
		{
			if(node.Nodes.Count>0 && node.ExpandVisibility!=eNodeExpandVisibility.Hidden || node.ExpandVisibility==eNodeExpandVisibility.Visible)
				return true;
			return false;
		}

		protected NodeExpandDisplay GetExpandDisplay(eExpandButtonType e)
		{
			NodeExpandDisplay d=null;
			switch(e)
			{
				case eExpandButtonType.Ellipse:
					d=new NodeExpandEllipseDisplay();
					break;
				case eExpandButtonType.Rectangle:
					d=new NodeExpandRectDisplay();
					break;
				case eExpandButtonType.Image:
					d=new NodeExpandImageDisplay();
					break;
			}
			return d;
		}

		protected bool IsRootNode(Node node)
		{
			return NodeOperations.IsRootNode(m_Tree,node);
		}

		protected ElementStyle GetDefaultNodeStyle()
		{
			ElementStyle style=new ElementStyle();
			style.TextColorSchemePart=eColorSchemePart.ItemText;

			return style;
		}

		public void MoveHostedControls()
		{
			Point offset=this.Offset;
			float zoom = this.Tree.Zoom;
			foreach(Cell cell in this.Tree.HostedControlCells)
			{
				Rectangle bounds = NodeDisplay.GetCellRectangle(eCellRectanglePart.TextBounds, cell, offset);
				Rectangle screenBounds=this.Tree.GetScreenRectangle(bounds);
				if(!bounds.IsEmpty && cell.HostedControl.Bounds!=screenBounds)
				{
					if(zoom!=1)
					{
						cell.HostedControlSize = bounds.Size;
						cell.IgnoreHostedControlSizeChange = true;
					}
					else
					{
						cell.HostedControlSize = Size.Empty;
					}
					cell.HostedControl.Bounds=screenBounds;
					if(zoom!=1)
						cell.IgnoreHostedControlSizeChange = false;
					if(cell.Parent!=null)
					{
						bool visible = NodeOperations.GetIsNodeVisible(cell.Parent);
						if(visible!=cell.HostedControl.Visible)
							cell.HostedControl.Visible = visible;
					}
				}
			}
		}
	}
}
