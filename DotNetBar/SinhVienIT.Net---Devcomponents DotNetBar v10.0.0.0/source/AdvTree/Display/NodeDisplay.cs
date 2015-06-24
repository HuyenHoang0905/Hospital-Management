using System;
using System.Drawing;
using DevComponents.AdvTree.Layout;
using System.Collections;
using DevComponents.DotNetBar;
using System.ComponentModel;

namespace DevComponents.AdvTree.Display
{
	/// <summary>
	/// Summary description for NodeDisplay.
	/// </summary>
	public class NodeDisplay
	{
		#region Private Variables
		private Point m_Offset=Point.Empty;
		private Point m_LockedOffset=Point.Empty;
		private AdvTree m_Tree=null;
        internal ArrayList _PaintedNodes = new ArrayList(100);
#if !TRIAL
		internal static bool keyInvalid=false;
#endif
		#endregion
		/// <summary>Creates new instance of the class</summary>
		/// <param name="tree">Object to initialize class with.</param>
		public NodeDisplay(AdvTree tree)
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
				return m_Tree.GetLayoutPosition(m_Offset);
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
				
                //if(m_Tree.NodeLayout is NodeMapLayout && m_Tree.Nodes.Count>0)
                //{
                //    if(!m_Tree.CenterContent)
                //        return new Point(Math.Abs(displayNode.ChildNodesBounds.Left),Math.Abs(displayNode.ChildNodesBounds.Top));
                //    else
                //        return new Point(m_Tree.SelectionBoxSize+(m_Tree.Width - m_Tree.SelectionBoxSize * 2 - m_Tree.NodeLayout.Width) / 2 + Math.Abs(displayNode.ChildNodesBounds.Left),
                //            m_Tree.SelectionBoxSize + (m_Tree.Height - m_Tree.SelectionBoxSize * 2 - m_Tree.NodeLayout.Height) / 2 + Math.Abs(displayNode.ChildNodesBounds.Top));
                //}
                //if(m_Tree.NodeLayout is Layout.NodeDiagramLayout)
                //{
                //    if(!m_Tree.CenterContent)
                //        return m_Tree.ClientRectangle.Location;
                //    else
                //        return new Point((m_Tree.Width-m_Tree.NodeLayout.Width)/2,(m_Tree.Height-m_Tree.NodeLayout.Height)/2);
                //}
                //else
					return m_Tree.ClientRectangle.Location;
			}
		}

		/// <summary>
		/// Gets or sets the reference to the tree control managed by display class.
		/// </summary>
		protected virtual AdvTree Tree
		{
			get {return m_Tree;}
			set {m_Tree=value;}
		}
        [EditorBrowsable(EditorBrowsableState.Never)]
		public static Rectangle GetNodeRectangle(eNodeRectanglePart part, Node node, Point offset)
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
            else if (part == eNodeRectanglePart.ExpandHitTestBounds)
            {
                Rectangle nodeBounds = GetNodeRectangle(eNodeRectanglePart.NodeBounds, node, offset);

                r = node.ExpandPartRectangleRelative;
                if (!r.IsEmpty)
                {
                    r.Offset(offset);
                    r.Offset(node.BoundsRelative.Location);
                }
                r.Y = nodeBounds.Y;
                r.Height = nodeBounds.Height;
                r.Inflate(1, 0);
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
            else if (part == eNodeRectanglePart.ChildNodeBounds)
            {
                r = node.ChildNodesBounds;
                if (!r.IsEmpty)
                {
                    //r.Offset(node.Bounds.Location);
                    r.Offset(offset);
                }
            }
            else if (part == eNodeRectanglePart.ColumnsBounds && HasColumnsVisible(node))
            {
                r = node.NodesColumns.Bounds;
                if(!r.IsEmpty)
                    r.Offset(offset);
            }
			return r;
		}

        internal static bool HasColumnsVisible(Node node)
        {
            return node.Expanded && node.HasColumns && node.NodesColumnsHeaderVisible;
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
                    r.Offset(cell.Parent.BoundsRelative.Location);
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
                case eExpandButtonType.Rectangle:
                    d = new NodeExpandRectDisplay();
                    break;
                case eExpandButtonType.Triangle:
                    d = new NodeExpandTriangleDisplay();
                    break;
				case eExpandButtonType.Ellipse:
					d=new NodeExpandEllipseDisplay();
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
            Point offset = this.Offset;
            float zoom = this.Tree.Zoom;
            foreach (Cell cell in this.Tree.HostedControlCells)
            {
                System.Windows.Forms.Control cellHostedControl = cell.HostedControl;
                if (cellHostedControl == null) continue;
                Rectangle bounds = NodeDisplay.GetCellRectangle(eCellRectanglePart.TextBounds, cell, offset);
                Rectangle screenBounds = this.Tree.GetScreenRectangle(bounds);
                if (!bounds.IsEmpty && cellHostedControl.Bounds != screenBounds)
                {
                    if (zoom != 1)
                    {
                        cell.HostedControlSize = bounds.Size;
                        cell.IgnoreHostedControlSizeChange = true;
                    }
                    else
                    {
                        cell.HostedControlSize = Size.Empty;
                        if (screenBounds.Height > cellHostedControl.Height && cellHostedControl.Height > 0)
                        {
                            screenBounds.Y += (screenBounds.Height - cellHostedControl.Height) / 2;
                            screenBounds.Height = cellHostedControl.Height;
                        }
                    }
                    cellHostedControl.Bounds = screenBounds;
                    if (zoom != 1)
                        cell.IgnoreHostedControlSizeChange = false;
                    if (cell.Parent != null)
                    {
                        bool visible = NodeOperations.GetIsNodeVisible(cell.Parent) && cell.IsVisible;
                        if (visible != cellHostedControl.Visible)
                            cellHostedControl.Visible = visible;
                    }
                }
            }
        }

        public ArrayList PaintedNodes
        {
            get
            {
                return _PaintedNodes;
            }
        }

        internal virtual void PaintColumnHeaders(ColumnHeaderCollection columns, Graphics g, bool treeControlHeader)
        {
            
        }
	} 
}
