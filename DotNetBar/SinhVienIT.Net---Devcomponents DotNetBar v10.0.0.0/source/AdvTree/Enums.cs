using System;

namespace DevComponents.AdvTree
{
    /// <summary>
    /// Specifies layout of the items in AdvTree control.
    /// </summary>
    public enum eView
    {
        /// <summary>
        /// Standard TreeView layout.
        /// </summary>
        Tree,
        /// <summary>
        /// ListView style tile layout.
        /// </summary>
        Tile
    }

	/// <summary>Specifies the way background image is displayed on background.</summary>
	public enum eStyleBackgroundImage:int
	{
		/// <summary>Image is stretched to fill the background</summary>
		Stretch=0,
		/// <summary>Image is centered inside the background</summary>
		Center=1,
		/// <summary>Image is tiled inside the background</summary>
		Tile=2,
		/// <summary>
		/// Image is drawn in top left corner of container space.
		/// </summary>
		TopLeft=3,
		/// <summary>
		/// Image is drawn in top right corner of container space.
		/// </summary>
		TopRight=4,
		/// <summary>
		/// Image is drawn in bottom left corner of container space.
		/// </summary>
		BottomLeft=5,
		/// <summary>
		/// Image is drawn in bottom right corner of container space.
		/// </summary>
		BottomRight=6
	}

	/// <summary>Indicates alignment of a part of the cell like image or check box in relation to the text.</summary>
	public enum eCellPartAlignment:int
	{
		/// <summary>
		/// Part is aligned to the left center of the text assuming left-to-right
		/// orientation.
		/// </summary>
		NearCenter=0,
		/// <summary>
		/// Part is aligned to the right center of the text assuming left-to-right
		/// orientation.
		/// </summary>
		FarCenter=1,
		/// <summary>
		/// Part is aligned to the top left of the text assuming left-to-right
		/// orientation.
		/// </summary>
		NearTop=2,
		/// <summary>Part is aligned above the text and centered.</summary>
		CenterTop=3,
		/// <summary>
		/// Part is aligned to the top right of the text assuming left-to-right
		/// orientation.
		/// </summary>
		FarTop=4,
		/// <summary>
		/// Part is aligned to the bottom left of the text assuming left-to-right
		/// orientation.
		/// </summary>
		NearBottom=5,
		/// <summary>Part is aligned below the text and centered.</summary>
		CenterBottom=6,
		/// <summary>
		/// Part is aligned to the bottom right of the text assuming left-to-right
		/// orientation.
		/// </summary>
		FarBottom=7,
        /// <summary>
        /// Part has default alignment that depends on the parent control view.
        /// </summary>
        Default = 8
	}

	/// <summary>
	/// Specifies how to trim characters from a text that does not completely fit into a element's shape.
	/// </summary>
	public enum eStyleTextTrimming
	{
		/// <summary>
		/// Specifies that the text is trimmed to the nearest character.
		/// </summary>
		Character=System.Drawing.StringTrimming.Character,
		/// <summary>
		/// Specifies that the text is trimmed to the nearest character, and an ellipsis is inserted at the end of a trimmed line.
		/// </summary>
		EllipsisCharacter=System.Drawing.StringTrimming.EllipsisCharacter,
		/// <summary>
		/// The center is removed from trimmed lines and replaced by an ellipsis. The algorithm keeps as much of the last slash-delimited segment of the line as possible.
		/// </summary>
		EllipsisPath=System.Drawing.StringTrimming.EllipsisPath,
		/// <summary>
		/// Specifies that text is trimmed to the nearest word, and an ellipsis is inserted at the end of a trimmed line.
		/// </summary>
		EllipsisWord=System.Drawing.StringTrimming.EllipsisWord,
		/// <summary>
		/// Specifies no trimming.
		/// </summary>
		None=System.Drawing.StringTrimming.None,
		/// <summary>
		/// Specifies that text is trimmed to the nearest word.
		/// </summary>
		Word=System.Drawing.StringTrimming.Word
	}

    ///// <summary>
    ///// Specifies the border type for style element.
    ///// </summary>
    //public enum eStyleBorderType:int
    //{
    //    /// <summary>Indicates no border</summary>
    //    None,
    //    /// <summary>Border is a solid line</summary>
    //    Solid,
    //    /// <summary>Border is a solid dash line</summary>
    //    Dash,
    //    /// <summary>Border is solid dash-dot line</summary>
    //    DashDot,
    //    /// <summary>Border is solid dash-dot-dot line</summary>
    //    DashDotDot,
    //    /// <summary>Border consists of dots</summary>
    //    Dot,
    //    /// <summary>Etched Border</summary>
    //    Etched,
    //    /// <summary>Double Border</summary>
    //    Double
    //}

	/// <summary>
	/// Indicates absolute vertical alignment of the content.
	/// </summary>
	internal enum eVerticalAlign
	{
		/// <summary>
		/// Content is aligned to the top
		/// </summary>
		Top,
		/// <summary>
		/// Content is aligned in the middle
		/// </summary>
		Middle,
		/// <summary>
		/// Content is aligned at the bottom
		/// </summary>
		Bottom
	}

	/// <summary>
	/// Indicates absolute horizontal alignment
	/// </summary>
	public enum eHorizontalAlign
	{
		/// <summary>
		/// Content is left aligned
		/// </summary>
		Left,
		/// <summary>
		/// Content is centered
		/// </summary>
		Center,
		/// <summary>
		/// Content is right aligned
		/// </summary>
		Right
	}

	/// <summary>
	/// Indicates prefered node layout position on Map tree layout when node is the child node of the top-level root node.
	/// </summary>
	public enum eMapPosition
	{
		/// <summary>
		/// Node is positioned based on default algorithm.
		/// </summary>
		Default,
		/// <summary>
		/// Sub-root node and all nodes after it are positioned to the left of the root.
		/// </summary>
		Near,
		/// <summary>
		/// Sub-root node and all nodes before it are positioned to the right of the root.
		/// </summary>
		Far
	}

    ///// <summary>
    ///// Indicates corner type for the border around visual element.
    ///// </summary>
    //public enum eCornerType
    //{
    //    /// <summary>
    //    /// Specifies that corner type is inherited from parent setting.
    //    /// </summary>
    //    Inherit,
    //    /// <summary>
    //    /// Specifies square corner.
    //    /// </summary>
    //    Square,
    //    /// <summary>
    //    /// Specifies rounded corner.
    //    /// </summary>
    //    Rounded,
    //    /// <summary>
    //    /// Specifies diagonal corner.
    //    /// </summary>
    //    Diagonal
    //}

	/// <summary>
	/// Specifies the column header visibility for the node.
	/// </summary>
	public enum eNodeHeaderVisibility
	{
		/// <summary>
		/// Column header is automatically shown/hidden based on the node's position in the tree. When
		/// Node is first child node i.e. with index=0 the header will be shown, otherwise header will
		/// be hidden.
		/// </summary>
		Automatic,
		/// <summary>
		/// Column header is always displayed regardless of node's position.
		/// </summary>
		AlwaysShow,
		/// <summary>
		/// Column header is always hidden regardless of node's position.
		/// </summary>
		AlwaysHide
	}

	/// <summary>
	/// Indicates the part of the node.
	/// </summary>
    public enum eNodeRectanglePart
	{
		/// <summary>
		/// Bounds of complete node content except expand button. This also includes the child node bounds if node is expanded.
		/// </summary>
		NodeContentBounds,
		/// <summary>
		/// Bounds of the expand button which collapses/expands the node.
		/// </summary>
		ExpandBounds,
        /// <summary>
        /// Hit test bounds of the expand button which collapses/expands the node used by mouse routines to trigger node expansion/collapse.
        /// </summary>
        ExpandHitTestBounds,
		/// <summary>
		/// Bounds of all child nodes of give node.
		/// </summary>
		ChildNodeBounds,
		/// <summary>
		/// Bounds for cells inside a node.
		/// </summary>
		CellsBounds,
		/// <summary>
		/// Complete node bounds including expand button.
		/// </summary>
		NodeBounds,
		/// <summary>
		/// Bounds of the command button.
		/// </summary>
		CommandBounds,
        /// <summary>
        /// Bounds of child node columns if node has columns defined.
        /// </summary>
        ColumnsBounds
	}

	/// <summary>
	/// Indicates the part of the cell.
	/// </summary>
	internal enum eCellRectanglePart
	{
		/// <summary>
		/// Bounds of check box or Rectangle.Empty if there is no check-box.
		/// </summary>
		CheckBoxBounds,
		/// <summary>
		/// Bounds of image inside the cell or Rectangle.Empty if there is no image.
		/// </summary>
		ImageBounds,
		/// <summary>
		/// Text bounds inside of cell.
		/// </summary>
		TextBounds,
		/// <summary>
		/// Cell bounds
		/// </summary>
		CellBounds
	}

	/// <summary>
	/// Indicates part of the node mouse is placed over.
	/// </summary>
	internal enum eMouseOverNodePart
	{
		/// <summary>
		/// Mouse is not over any node part.
		/// </summary>
		None,
		/// <summary>
		/// Mouse is placed over the node.
		/// </summary>
		Node,
		/// <summary>
		/// Mouse is placed over node expand button.
		/// </summary>
		Expand,
		/// <summary>
		/// Mouse is placed over the cell.
		/// </summary>
		Cell,
		/// <summary>
		/// Mouse is placed over the command button.
		/// </summary>
		Command
	}

    ///// <summary>
    ///// Indicates white-space part of the style.
    ///// </summary>
    //[Flags()]
    //public enum eSpacePart
    //{
    //    /// <summary>
    //    /// Represents style padding.
    //    /// </summary>
    //    Padding=1,
    //    /// <summary>
    //    /// Represents style border.
    //    /// </summary>
    //    Border=2,
    //    /// <summary>
    //    /// Represents style margin.
    //    /// </summary>
    //    Margin=4
    //}

    ///// <summary>
    ///// Indicates the style side.
    ///// </summary>
    //public enum eStyleSide
    //{
    //    /// <summary>
    //    /// Specifies left side of the style.
    //    /// </summary>
    //    Left,
    //    /// <summary>
    //    /// Specifies right side of the style.
    //    /// </summary>
    //    Right,
    //    /// <summary>
    //    /// Specifies top side of the style.
    //    /// </summary>
    //    Top,
    //    /// <summary>
    //    /// Specifies bottom side of the style.
    //    /// </summary>
    //    Bottom
    //}

	/// <summary>
	/// Indicates the visibility of node expand part which allows user to expand/collaps node.
	/// </summary>
	public enum eNodeExpandVisibility
	{
		/// <summary>
		/// Default setting which indicates that when node has child nodes expand part is visible otherwise it is hidden.
		/// </summary>
		Auto,
		/// <summary>
		/// Expand part is always visible regardless of whether child nodes are present or not.
		/// </summary>
		Visible,
		/// <summary>
		/// Expand part is always hidden regardless of whether child nodes are present or not.
		/// </summary>
		Hidden
	}

	/// <summary>
	/// Specifies the action that raised a AdvTreeEventArgs event
	/// </summary>
	public enum eTreeAction
	{
		/// <summary>
		/// The event was caused by a keystroke.
		/// </summary>
		Keyboard,
		/// <summary>
		/// The event was caused by a mouse operation.
		/// </summary>
		Mouse,
		/// <summary>
		/// The event was caused by the Node collapsing.
		/// </summary>
		Collapse,
		/// <summary>
		/// The event was caused by the Node expanding.
		/// </summary>
		Expand,
		/// <summary>
		/// The event is caused programmatically from user code.
		/// </summary>
		Code
	}

	/// <summary>
	/// Specifies node connector type. Node connector is the type of the line/connection that is drawn to connect child node to it's parent node.
	/// </summary>
	public enum eNodeConnectorType
	{
        ///// <summary>
        ///// Curved line connector type.
        ///// </summary>
        //Curve,
		/// <summary>
		/// Straight line connector type.
		/// </summary>
		Line
	}

    ///// <summary>
    ///// Specifies the cap style with which the connector line will start or end.
    ///// </summary>
    //public enum eConnectorCap
    //{
    //    /// <summary>
    //    /// Specifies no cap.
    //    /// </summary>
    //    None,
    //    /// <summary>
    //    /// Round cap type.
    //    /// </summary>
    //    Ellipse,
    //    /// <summary>
    //    /// Arrow cap type.
    //    /// </summary>
    //    Arrow
    //}

	/// <summary>
	/// Specifies the layout type used to position the cells within the nodes.
	/// </summary>
	public enum eCellLayout
	{
		/// <summary>
		/// Specifies that default setting is to be used for cell layout. Default is Horizontal. When set to default on the Node, setting from Tree control is used.
		/// </summary>
		Default,
		/// <summary>Horizontal layout positions the cells horizontally next to each other.</summary>
		Horizontal,
		/// <summary>
		/// Vertical layout positions cell vertically on top of each other.
		/// </summary>
		Vertical
	}

	/// <summary>
	/// Specifies the layout type used to position the parts of the cell like image, checkbox and text.
	/// </summary>
	public enum eCellPartLayout
	{
		/// <summary>
		/// Specifies that default setting is to be used for cell parts layout. Default is Horizontal. When set to default on the Cell, setting from Tree control is used.
		/// </summary>
		Default,
		/// <summary>Horizontal layout positions the parts of the cell horizontally next to each other.</summary>
		Horizontal,
		/// <summary>
		/// Vertical layout positions parts of the cell vertically on top of each other.
		/// </summary>
		Vertical
	}

    /// <summary>
    /// Specifies the color scheme loaded by ColorScheme object.
    /// </summary>
    public enum eColorSchemeStyle
    {
        /// <summary>
        /// Indicates Office 2003 like color scheme.
        /// </summary>
        Office2003,
        /// <summary>
        /// Indicates VS.NET 2005 like color scheme.
        /// </summary>
        VS2005,
        /// <summary>
        /// Indicates Office 2007 like color scheme.
        /// </summary>
        Office2007
    }

	/// <summary>
	/// Specifies the currently selected system color scheme if running on Windows XP.
	/// </summary>
	internal enum eWinXPColorScheme
	{
		/// <summary>
		/// Color scheme cannot be determined.
		/// </summary>
		Undetermined,
		/// <summary>
		/// Blue color scheme.
		/// </summary>
		Blue,
		/// <summary>
		/// Olive green color scheme.
		/// </summary>
		OliveGreen,
		/// <summary>
		/// Silver color scheme.
		/// </summary>
		Silver
	}

	/// <summary>
	/// Specifies the flow of diagram layout related to the root node.
	/// </summary>
	public enum eDiagramFlow
	{
		/// <summary>
		/// Nodes are positioned from left to right with root node being the left-most node.
		/// </summary>
		LeftToRight,
		/// <summary>
		/// Nodes are positioned from right to left with root node being the right-most
		/// node.
		/// </summary>
		RightToLeft,
		/// <summary>
		/// Nodes are positioned from top to bottom with root node being the top node.
		/// </summary>
		TopToBottom,
		/// <summary>
		/// Nodes are positioned from bottom to top with root node being bottom node.
		/// </summary>
		BottomToTop
	}
	
	/// <summary>
	/// Specifies the flow of the map layout.
	/// </summary>
	public enum eMapFlow
	{
		/// <summary>
		/// Nodes are arranged around the root node.
		/// </summary>
		Spread,
		/// <summary>
		/// Nodes are arranged from below the root node.
		/// </summary>
		TopToBottom,
		/// <summary>
		/// Nodes are arranged above the root node.
		/// </summary>
		BottomToTop,
		/// <summary>
		/// Nodes are arranged to the right of the root node.
		/// </summary>
		LeftToRight,
		/// <summary>
		/// Nodes are arranged to the left of the root node.
		/// </summary>
		RightToLeft
	}

	/// <summary>
	/// Specifies the type of the expand button.
	/// </summary>
	public enum eExpandButtonType
	{
		/// <summary>
		/// Indicates elliptical expand button.
		/// </summary>
		Ellipse,
		/// <summary>
		/// Indicates rectangular expand button.
		/// </summary>
		Rectangle,
		/// <summary>
		/// Indicates that images are used for expand button. 
		/// </summary>
		Image,
        /// <summary>
        /// Indicates the Windows Vista style expand button.
        /// </summary>
        Triangle
	}
	
	/// <summary>
	/// Specifies the visual style for the tree control.
	/// </summary>
	public enum eVisualStyle
	{
		/// <summary>
		/// Indicates default visual style.
		/// </summary>
		Default
	}

	/// <summary>
	/// Specifies the layout type for the nodes.
	/// </summary>
	public enum eNodeLayout
	{
		/// <summary>
		/// Nodes are arranged around root node in map format.
		/// </summary>
		Map,
		/// <summary>
		/// Nodes are arranged from left-to-right in diagram format.
		/// </summary>
		Diagram
	}
	
	/// <summary>
	/// Specifies renderer type used to render nodes.
	/// </summary>
	public enum eNodeRenderMode
	{
		
		/// <summary>
		/// Specifies default renderer which allows most customization through AdvTree
		/// properties. Default renderer integrates with the Style architecture to provide
		/// customization on renderer behavior.
		/// </summary>
		Default,
        ///// <summary>
        ///// 	<para>Specifies professional renderer. Professional renderer is custom renderer
        /////     which does not rely on Style architecture for customization of renderer appearance
        /////     since it provides much richer appearance than Default renderer.</para>
        ///// 	<para>Professional renderer colors can be controls through
        /////     NodeProfessionalColorTable object which is exposed by
        /////     NodeProfessionalRenderer.ColorTable property.</para>
        ///// </summary>
        //Professional,
		/// <summary>
		/// Specifies that custom renderer is used. When set you must also set NodeRenderer
		/// to renderer you want to use.
		/// </summary>
		Custom
	}

    /// <summary>
    /// Specifies the node selection style.
    /// </summary>
    public enum eSelectionStyle
    {
        /// <summary>
        /// Node selector highlights the complete node row when node is selected.
        /// </summary>
        FullRowSelect,
        /// <summary>
        /// Node selector draws the rectangle that highlights the node content. Appearance similar to system tree view in Windows Vista.
        /// </summary>
        HighlightCells,
        /// <summary>
        /// Node selector draws hollow selection rectangle around the node.
        /// </summary>
        NodeMarker,
    }

    /// <summary>
    /// Specifies the rule for multi-node selection.
    /// </summary>
    public enum eMultiSelectRule
    {
        /// <summary>
        /// Allows multiple selection of nodes with same parent node only.
        /// </summary>
        SameParent,
        /// <summary>
        /// Allows multiple selection of any node.
        /// </summary>
        AnyNode
    }

    /// <summary>
    /// Gets or sets the image alignment inside of column header.
    /// </summary>
    public enum eColumnImageAlignment
    {
        /// <summary>
        /// Image is left aligned.
        /// </summary>
        Left,
        /// <summary>
        /// Image is right aligned.
        /// </summary>
        Right
    }

    /// <summary>
    /// Specifies the editor type used when cell is edited.
    /// </summary>
    public enum eCellEditorType
    {
        /// <summary>
        /// Indicates default, text based editor.
        /// </summary>
        Default,
#if FRAMEWORK20
        /// <summary>
        /// Indicates that Integer numeric editor will be used for editing the value of the cell or column.
        /// </summary>
        NumericInteger,
        /// <summary>
        /// Indicates that Double numeric editor will be used for editing the value of the cell or column.
        /// </summary>
        NumericDouble,
        /// <summary>
        /// Indicates that Currency numeric editor will be used for editing the value of the cell or column.
        /// </summary>
        NumericCurrency,
#endif
        /// <summary>
        /// Indicates that cell will use custom editor that you provide by handling AdvTree.ProvideCustomCellEditor event.
        /// </summary>
        Custom
    }
    /// <summary>
    /// Specifies the sort direction for the column header.
    /// </summary>
    public enum eSortDirection
    {
        /// <summary>
        /// No sort is specified.
        /// </summary>
        None,
        /// <summary>
        /// Ascending sorting is in effect, i.e. A-Z
        /// </summary>
        Ascending,
        /// <summary>
        /// Descending sorting is in effect, i.e. Z-A
        /// </summary>
        Descending
    }
}
