using System;
using System.Drawing;
using System.Collections;

namespace DevComponents.Tree
{
	/// <summary>
	/// Used to pass node contextual information used for layout of the node.
	/// </summary>
	internal class NodeLayoutContextInfo
	{
		public Node ContextNode=null;
		public Rectangle ClientRectangle=Rectangle.Empty;
		public int Left;
		public int Top;
		public ArrayList DefaultColumns=null;
		public ArrayList ChildColumns=null;
		public Font DefaultFont=null;
		public ElementStyle DefaultCellStyle=null;
		public ElementStyle DefaultNodeStyle=null;
		public Size DefaultHeaderSize=Size.Empty;
		public bool LeftToRight=true;
		public bool HasExpandPart=true;
		public System.Drawing.Graphics Graphics=null;
		public ElementStyleCollection Styles=null;
		public eCellLayout CellLayout=eCellLayout.Horizontal;
		public eCellPartLayout CellPartLayout=eCellPartLayout.Horizontal;
		public bool MapPositionNear=false;
	}
}
