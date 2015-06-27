using System;
using System.Drawing;
using System.Collections;
using DevComponents.DotNetBar;

namespace DevComponents.AdvTree.Layout
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
		public NodeColumnInfo DefaultColumns=null;
        public NodeColumnInfo ChildColumns = null;
		public Font DefaultFont=null;
		public ElementStyle DefaultCellStyle=null;
		public ElementStyle DefaultNodeStyle=null;
		public Size DefaultHeaderSize=Size.Empty;
		public bool LeftToRight=true;
		public bool HasExpandPart=true;
		public System.Drawing.Graphics Graphics=null;
		public ElementStyleCollection Styles=null;
		public eCellLayout CellLayout=eCellLayout.Default;
		public eCellPartLayout CellPartLayout=eCellPartLayout.Horizontal;
		public bool MapPositionNear=false;
        public bool ExpandPartAlignedLeft = false;
        public ColumnHeaderCollection TreeColumns = null;
        public ArrayList FullRowBackgroundNodes = null;
        public int ExpandPartWidth = 0;
        public int CurrentLineHeight = 0; // Used by tile layout
        public int CurrentLevelLeft = 0; // Used by tile layout
        public eView View = eView.Tree; // Current control view
        public Size TileSize = Size.Empty; // Tile size
        public bool IsViewGroupping = false; // Tile view grouping enabled
        public ElementStyle ColumnStyle = null;
        public int LayoutNodeExpandPartWidth = 0;
	}
}
