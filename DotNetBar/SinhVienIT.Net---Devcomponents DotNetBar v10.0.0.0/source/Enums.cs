using System;
using System.Drawing;
using System.Windows.Forms;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Specifies the appearance of a item.
	/// </summary>
	public enum eDotNetBarStyle:int
	{
		OfficeXP = 0,
		Office2000 = 1,
		Office2003 = 2,
		VS2005 = 3,
        Office2007 = 4,
        Office2010 = 5,
        Windows7 = 6,
        StyleManagerControlled = 7
	}

    /// <summary>
    /// Specifies the CrumbBar control style.
    /// </summary>
    public enum eCrumbBarStyle : int
    {
        Office2007 = 4, // Same as eDotNetBarStyle
        Vista = 5
    }

	/// <summary>
	/// Specifies the Bar state.
	/// </summary>
	public enum eBarState:int
	{
		Popup=0,
		Floating=1,
		Docked=2,
		AutoHide=3
	}

	/// <summary>
	/// Specifies the Bar grab handle style.
	/// </summary>
	public enum eGrabHandleStyle:int
	{
		None=0,
		Single=1,
		Dotted=2,
		Double=3,
		DoubleThin=4,
		DoubleFlat=5,
		Stripe=6,
		StripeFlat=7,
		Caption=8,
		ResizeHandle=9,
		Office2003=10,
		Office2003SingleDot=11,
		CaptionTaskPane=12,
        CaptionDotted = 13
	}

	/// <summary>
	/// Specifies the Orientation of the item within container.
	/// </summary>
	public enum eOrientation:int
	{
		Horizontal=0,
		Vertical=1
	}

    /// <summary>
    /// Specifies the design-marker orientation for the item.
    /// </summary>
    public enum eDesignMarkerOrientation
    {
        NotSet,
        Horizontal,
        Vertical
    }

	/// <summary>
	/// Specifies the supported orientations by the item.
	/// </summary>
	public enum eSupportedOrientation:int
	{
		Both=0,
		Horizontal=1,
		Vertical=2
	}

	/// <summary>
	/// Specifies the docked Bar border type.
	/// </summary>
	public enum eBorderType:int
	{
		None=0,
		SingleLine	=1,
		DoubleLine	=2,
		Sunken		=3,
		Raised		=4,
        RaisedInner =7,
		Etched		=5,
		Bump		=6
	}

	/// <summary>
	/// Specifes vertical alignment.
	/// </summary>
	public enum eAlignment:int
	{
		Top=0,
		Middle=1,
		Bottom=2
	}

	/// <summary>
	/// Specifies item alignment.
	/// </summary>
	public enum eItemAlignment:int
	{
		Near=0,
		Far=1,
        Center=2
	}

	/// <summary>
	/// Specifies the popup type.
	/// </summary>
	public enum ePopupType:int
	{
		Menu=0,
		ToolBar=1,
		Container=2
	}

	/// <summary>
	/// Specifies the dock side.
	/// </summary>
	public enum eDockSide:int
	{
		None=0,
		Left=1,
		Right=2,
		Top=3,
		Bottom=4,
		Document=5
	}

	/// <summary>
	/// Specifies the item shortcut.
	/// </summary>
	public enum eShortcut:int
	{
		AltBksp = (System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Back),// 0x00040008,
		AltF1 = 0x00040070,
		AltF10 = 0x00040079,
		AltF11 = 0x0004007A,
		AltF12 = 0x0004007B,
		AltF2 = 0x00040071,
		AltF3 = 0x00040072,
		AltF4 = 0x00040073,
		AltF5 = 0x00040074,
		AltF6 = 0x00040075,
		AltF7 = 0x00040076,
		AltF8 = 0x00040077,
		AltF9 = 0x00040078,
		AltLeft = System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Left,
		AltRight = System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Right,
		AltUp = System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Up,
		AltDown = System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Down,
		CtrlA = 0x00020041,
		CtrlB = 0x00020042,
		CtrlC = 0x00020043,
		CtrlD = 0x00020044,
		CtrlDel = 0x0002002E,
		CtrlE = 0x00020045,
		CtrlF = 0x00020046,
		CtrlF1 = 0x00020070,
		CtrlF11 = 0x0002007A,
		CtrlF12 = 0x0002007B,
		CtrlF2 = 0x00020071,
		CtrlF3 = 0x00020072,
		CtrlF4 = 0x00020073,
		CtrlF5 = 0x00020074,
		CtrlF6 = 0x00020075,
		CtrlF7 = 0x00020076,
		CtrlF8 = 0x00020077,
		CtrlF9 = 0x00020078,
		CtrlG = 0x00020047,
		CtrlH = 0x00020048,
		CtrlI = 0x00020049,
		CtrlIns = 0x0002002D,
		CtrlJ = 0x0002004A,
		CtrlK = 0x0002004B,
		CtrlL = 0x0002004C,
		CtrlM = 0x0002004D,
		CtrlN = 0x0002004E,
		CtrlO = 0x0002004F,
		CtrlP = 0x00020050,
		CtrlQ = 0x00020051,
		CtrlR = 0x00020052,
		CtrlS = 0x00020053,
		CtrlShiftA = 0x00030041,
		CtrlShiftB = 0x00030042,
		CtrlShiftC = 0x00030043,
		CtrlShiftD = 0x00030044,
		CtrlShiftE = 0x00030045,
		CtrlShiftF = 0x00030046,
		CtrlShiftF1 = 0x00030070,
		CtrlShiftF11 = 0x0003007A,
		CtrlShiftF12 = 0x0003007B,
		CtrlShiftF2 = 0x00030071,
		CtrlShiftF3 = 0x00030072,
		CtrlShiftF4 = 0x00030073,
		CtrlShiftF5 = 0x00030074,
		CtrlShiftF6 = 0x00030075,
		CtrlShiftF7 = 0x00030076,
		CtrlShiftF8 = 0x00030077,
		CtrlShiftF9 = 0x00030078,
		CtrlShiftG = 0x00030047,
		CtrlShiftH = 0x00030048,
		CtrlShiftI = 0x00030049,
		CtrlShiftJ = 0x0003004A,
		CtrlShiftK = 0x0003004B,
		CtrlShiftL = 0x0003004C,
		CtrlShiftM = 0x0003004D,
		CtrlShiftN = 0x0003004E,
		CtrlShiftO = 0x0003004F,
		CtrlShiftP = 0x00030050,
		CtrlShiftQ = 0x00030051,
		CtrlShiftR = 0x00030052,
		CtrlShiftS = 0x00030053,
		CtrlShiftT = 0x00030054,
		CtrlShiftU = 0x00030055,
		CtrlShiftV = 0x00030056,
		CtrlShiftW = 0x00030057,
		CtrlShiftX = 0x00030058,
		CtrlShiftY = 0x00030059,
		CtrlShiftZ = 0x0003005A,
		CtrlLeft = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Left,
		CtrlRight = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Right,
		CtrlUp = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Up,
		CtrlDown = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Down,
        CtrlEnter = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Enter,
		CtrlT = 0x00020054,
		CtrlU = 0x00020055,
		CtrlV = 0x00020056,
		CtrlW = 0x00020057,
		CtrlX = 0x00020058,
		CtrlY = 0x00020059,
		CtrlZ = 0x0002005A,
        Ctrl0 = Keys.Control | Keys.D0,
        Ctrl1 = Keys.Control | Keys.D1,
        Ctrl2 = Keys.Control | Keys.D2,
        Ctrl3 = Keys.Control | Keys.D3,
        Ctrl4 = Keys.Control | Keys.D4,
        Ctrl5 = Keys.Control | Keys.D5,
        Ctrl6 = Keys.Control | Keys.D6,
        Ctrl7 = Keys.Control | Keys.D7,
        Ctrl8 = Keys.Control | Keys.D8,
        Ctrl9 = Keys.Control | Keys.D9,
		Del = 0x0000002E,
		F1 = 0x00000070,
		F11 = 0x0000007A,
		F12 = 0x0000007B,
		F2 = 0x00000071,
		F3 = 0x00000072,
		F4 = 0x00000073,
		F5 = 0x00000074,
		F6 = 0x00000075,
		F7 = 0x00000076,
		F8 = 0x00000077,
		F9 = 0x00000078,
		F10 = System.Windows.Forms.Keys.F10,
		Ins = 0x0000002D,
		None = 0x00000000,
		ShiftDel = 0x0001002E,
		ShiftF1 = 0x00010070,
		ShiftF11 = 0x0001007A,
		ShiftF12 = 0x0001007B,
		ShiftF2 = 0x00010071,
		ShiftF3 = 0x00010072,
		ShiftF4 = 0x00010073,
		ShiftF5 = 0x00010074,
		ShiftF6 = 0x00010075,
		ShiftF7 = 0x00010076,
		ShiftF8 = 0x00010077,
		ShiftF9 = 0x00010078,
		ShiftIns = 0x0001002D,
		ShiftAltA = System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.A,
		ShiftAltB = System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.B,
		ShiftAltC = System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.C,
		ShiftAltD = System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.D,
		ShiftAltE = System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.E,
		ShiftAltF = System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F,
		ShiftAltF1 = System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F1,
		ShiftAltF11 = System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F11,
		ShiftAltF12 = System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F12,
		ShiftAltF2 = System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F2,
		ShiftAltF3 = System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F3,
		ShiftAltF4 = System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4,
		ShiftAltF5 = System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F5,
		ShiftAltF6 = System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F6,
		ShiftAltF7 = System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F7,
		ShiftAltF8 = System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F8,
		ShiftAltF9 = System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F9,
		ShiftAltG = System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.G,
		ShiftAltH = System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.H,
		ShiftAltI = System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.I,
		ShiftAltJ = System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.J,
		ShiftAltK = System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.K,
		ShiftAltL = System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.L,
		ShiftAltM = System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.M,
		ShiftAltN = System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.N,
		ShiftAltO = System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.O,
		ShiftAltP = System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.P,
		ShiftAltQ = System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Q,
		ShiftAltR = System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.R,
		ShiftAltS = System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.S,
		ShiftAltT = System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.T,
		ShiftAltU = System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.U,
		ShiftAltV = System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.V,
		ShiftAltW = System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.W,
		ShiftAltX = System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.X,
		ShiftAltY = System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Y,
		ShiftAltZ = System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Z,
		AltEnter= System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Enter
	}

	internal enum SystemButton:int
	{
		None=-1,
		Minimize=System.Windows.Forms.CaptionButton.Minimize,
		Restore=System.Windows.Forms.CaptionButton.Restore,
		Close=System.Windows.Forms.CaptionButton.Close,
		Help=System.Windows.Forms.CaptionButton.Help,
		Maximize=System.Windows.Forms.CaptionButton.Maximize,
		NextWindow=10
	}

	/// <summary>
	/// Specifies the item menu visibility.
	/// </summary>
	public enum eMenuVisibility:int
	{
		VisibleAlways=0,
		VisibleIfRecentlyUsed=1
	}

	/// <summary>
	/// Specifies the item behavior personalized menus.
	/// </summary>
	public enum ePersonalizedMenus:int
	{
		Disabled=0,
		DisplayOnHover=1,
		DisplayOnClick=2,
		Both=3
	}

	/// <summary>
	/// Specifies the popup animation.
	/// </summary>
	public enum ePopupAnimation:int
	{
		None=0,
		ManagerControlled=1,
		Slide=2,
		Unfold=3,
		Fade=4,
		Random=5,
		SystemDefault=6
	}

	/// <summary>
	/// Specifies ButtonItem style.
	/// </summary>
	public enum eButtonStyle:int
	{
		Default=0,				// Default style, Image and Text on menus, Image on Bar
		TextOnlyAlways=1,		// Text always everywhere
		ImageAndText=2			// Always display Image and text
	}

	/// <summary>
	/// Specifies the image position.
	/// </summary>
	public enum eImagePosition:int
	{
		Left=0,
		Right=1,
		Top=2,
		Bottom=3
	}

	/// <summary>
	/// Specifies the hot tracking style for buttons.
	/// </summary>
	public enum eHotTrackingStyle:int
	{
		Default=0,
		Color=1,
		None=2,
		Image=3
	}

	/// <summary>
	/// Specifies the menu drop shadow.
	/// </summary>
	public enum eMenuDropShadow:int
	{
		SystemDefault=0,
		Show=1,
		Hide=2
	}

	/// <summary>
	/// Specifies the image size for the items on the Bar.
	/// </summary>
	public enum eBarImageSize:int
	{
		Default=0,
		Medium=1,
		Large=2
	}
    /// <summary>
    /// Specifies button image list selection.
    /// </summary>
    public enum eButtonImageListSelection:int
    {
        NotSet = -1,
        Default = 0,
        Medium = 1,
        Large = 2
    }


	internal enum eDesignInsertPosition
	{
		None=0,
		Before=1,
		After=2
	}

	public enum eLayoutType:int
	{
		Toolbar=0,
		TaskList=1,
		DockContainer=2
	}

	public enum eBackgroundImagePosition:int
	{
		/// <summary>
		/// Indicates that image is stretched to fill the container space.
		/// </summary>
		Stretch=0,
		/// <summary>
		/// Image is centered inside of container space.
		/// </summary>
		Center=1,
		/// <summary>
		/// Image is tiled to fill container space.
		/// </summary>
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
		BottomRight=6,
        /// <summary>
        /// Image is drawn on the left side in the middle of the container space.
        /// </summary>
        CenterLeft = 7,
        /// <summary>
        /// Image is drawn on the right side in the middle of the container space.
        /// </summary>
        CenterRight = 8,
	}

	public enum eExplorerBarStockStyle:int
	{
		Blue=0,
		BlueSpecial=1,
		OliveGreen=2,
		OliveGreenSpecial=3,
		Silver=4,
		SilverSpecial=5,
        SystemColors = 98,
		Custom=99
	}

	public enum eMouseState:int
	{
		None=0,
		Hot=1,
		Down=2
	}

	internal enum ImageState:int
	{
		Default=0,
		Disabled=1,
		Hover=2,
		Pressed=3
	}
	
	/// <summary>
	/// Specifies the sides of a rectangle to apply a border to.
	/// </summary>
	[System.Flags()]
	public enum eBorderSide
	{
		/// <summary>
		/// No Border.
		/// </summary>
		None=0,
		/// <summary>
		/// Border on the Left edge.
		/// </summary>
		Left=1,
		/// <summary>
		/// Border on the Right edge.
		/// </summary>
		Right=2,
		/// <summary>
		/// Border on the Top edge.
		/// </summary>
		Top=4,
		/// <summary>
		/// Border on the Bottom edge.
		/// </summary>
		Bottom=8,
		/// <summary>
		/// Border on all 4 sides.
		/// </summary>
		All=(Left | Right | Top | Bottom)
	}

	/// <summary>
	/// Specifies appearance type of the Side Bar control.
	/// </summary>
	public enum eSideBarAppearance
	{
		/// <summary>
		/// Traditional Side Bar appearance with 3D panels.
		/// </summary>
		Traditional,
		/// <summary>
		/// Improved Flat Side Bar appearance with extended appearance options.
		/// </summary>
		Flat
	}

	/// <summary>
	/// Specifies predefined side bar color scheme.
	/// </summary>
	public enum eSideBarColorScheme
	{
		SystemColors,
		Blue,
		Silver,
		Green,
		Orange,
		Red,
		LightBlue,
		Money,
		Brick,
		Wheat,
		Storm,
		Spruce,
		Slate,
		Rose,
		Fire,
		Pumpkin,
		Plum,
		Marine,
		Sunset
	}

	/// <summary>
	/// Specifies the side popup is displayed in relation to it's parent.
	/// </summary>
	public enum ePopupSide
	{
		Default=0,
		Left=1,
		Right=2,
		Top=3,
		Bottom=4
	}

	/// <summary>
	/// Indicates layout type used for items within side bar panel.
	/// </summary>
	public enum eSideBarLayoutType
	{
		/// <summary>
		/// Default layout all items arranged in one column.
		/// </summary>
		Default=0,
		/// <summary>
		/// Items arranged in multiple columns determined by the width of the panel.
		/// </summary>
		MultiColumn=1
	}

	/// <summary>
	/// Indicates color scheme assigned to the tab item.
	/// </summary>
	public enum eTabItemColor
	{
		Default=0,
		Blue=1,
		Yellow=2,
		Green=3,
		Red=4,
		Purple=5,
		Cyan=6,
		Orange=7,
		Magenta=8,
		BlueMist=9,
		PurpleMist=10,
		Tan=11,
		Lemon=12,
		Apple=13,
		Teal=14,
		Silver=15
	}

	/// <summary>
	/// Indicates the action end user took during toolbar/menubar customization.
	/// </summary>
	public enum eEndUserCustomizeAction
	{
		/// <summary>
		/// User has changed the visibility of the bar.
		/// </summary>
		BarVisibilityChanged,
		/// <summary>
		/// Indicates that item visibility has changed i.e. it's visible property.
		/// </summary>
		ItemVisibilityChanged,
		/// <summary>
		/// Indicates that an item has been moved to different location.
		/// </summary>
		ItemMoved,
		/// <summary>
		/// Indicates that an item has been removed from the bar.
		/// </summary>
		ItemDeleted,
		/// <summary>
		/// Indicates that item's text has been changed.
		/// </summary>
		ItemTextChanged,
		/// <summary>
		/// Indicates that style of the button i.e. ButtonStyle property has changed.
		/// </summary>
		ItemStyleChanged,
		/// <summary>
		/// Indicates that item's BeginGroup property has changed.
		/// </summary>
		ItemBeginGroupChanged,
		/// <summary>
		/// Indicates that user has created a new bar.
		/// </summary>
		NewBarCreated,
		/// <summary>
		/// Indicates that user has renamed the bar i.e. changed it's Text property.
		/// </summary>
		BarRenamed,
		/// <summary>
		/// Indicates that user has deleted the bar.
		/// </summary>
		BarDeleted
	}

	/// <summary>
	/// Specifies the type of the layout for tabs.
	/// </summary>
	public enum eTabLayoutType
	{
		/// <summary>
		/// Tabs are auto-sized to fit the width of the container.
		/// </summary>
		FitContainer,
		/// <summary>
		/// Tab's width is calculated based on the image and text and navigation box is displayed
		/// when tabs cannot fit the container.
		/// </summary>
		FixedWithNavigationBox,
		/// <summary>
		/// Tab are wrapping on multiple lines based on the width and navigation box is displayed.
		/// </summary>
		MultilineWithNavigationBox,
		/// <summary>
		/// Tab are wrapping on multiple lines based on the width and NO navigation box is displayed.
		/// </summary>
		MultilineNoNavigationBox
	}

	/// <summary>
	/// Indicates the corner type.
	/// </summary>
	public enum eCornerType
	{
		/// <summary>
		/// Inherits setting if applies.
		/// </summary>
		Inherit,
		/// <summary>
		/// Specifies square corner.
		/// </summary>
		Square,
		/// <summary>
		/// Specifies rounded corner.
		/// </summary>
		Rounded,
		/// <summary>
		/// Specifies diagonal corner.
		/// </summary>
		Diagonal
	}

	/// <summary>
	/// Specifies the action that raised a event
	/// </summary>
	public enum eEventSource
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
		/// The event is caused programmatically from user code.
		/// </summary>
		Code
	}

	/// <summary>
	/// Specifies the button alignment inside of the BubbleBar.
	/// </summary>
	public enum eBubbleButtonAlignment
	{
		/// <summary>
		/// Buttons are aligned to the top and arranged horizontally.
		/// </summary>
		Top,
		/// <summary>
		/// Buttons are aligned to the bottom and arranged horizontally.
		/// </summary>
		Bottom,
//		/// <summary>
//		/// Buttons are aligned to the left and arranged vertically.
//		/// </summary>
//		Left,
//		/// <summary>
//		/// Buttons are aligned to the right and arranged vertically.
//		/// </summary>
//		Right
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
		BottomRight=6,
        /// <summary>
        /// Imaged is centered and fills the control but aspect ratio is unchanged.
        /// </summary>
        Zoom
	}

	/// <summary>
	/// Specifies the border type for style element.
	/// </summary>
	public enum eStyleBorderType:int
	{
		/// <summary>Indicates no border</summary>
		None,
		/// <summary>Border is a solid line</summary>
		Solid,
		/// <summary>Border is a solid dash line</summary>
		Dash,
		/// <summary>Border is solid dash-dot line</summary>
		DashDot,
		/// <summary>Border is solid dash-dot-dot line</summary>
		DashDotDot,
		/// <summary>Border consists of dots</summary>
		Dot,
        /// <summary>Border consists light and dark part creating an etched effect</summary>
        Etched,
        /// <summary>Border consists dark and light part. Light part is the inside border.</summary>
        Double
	}

	/// <summary>
	/// Specifies the alignment of a text string relative to its element's rectangle.
	/// </summary>
	public enum eStyleTextAlignment
	{
		/// <summary>
		/// Specifies the text be aligned near from the origin position of the element's rectangle. In a left-to-right layout, the near position is left. In a right-to-left layout, the near position is right.
		/// </summary>
		Near=System.Drawing.StringAlignment.Near,
		/// <summary>
		/// Specifies that text is aligned in the center of the element's rectangle.
		/// </summary>
		Center=System.Drawing.StringAlignment.Center,
		/// <summary>
		/// Specifies that text is aligned far from the origin position of the element's rectangle. In a left-to-right layout, the far position is right. In a right-to-left layout, the far position is left.
		/// </summary>
		Far=System.Drawing.StringAlignment.Far
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

    /// <summary>
    /// Specifies the alignment of buttons in title bar.
    /// </summary>
    public enum eTitleButtonAlignment
    {
        /// <summary>
        /// Buttons are left aligned.
        /// </summary>
        Left=0,
        /// <summary>
        /// Buttons are right aligned.
        /// </summary>
        Right=1
    }

	/// <summary>
	/// Indicates white-space part of the style.
	/// </summary>
	[Flags()]
	public enum eSpacePart
	{
		/// <summary>
		/// Represents style padding.
		/// </summary>
		Padding=1,
		/// <summary>
		/// Represents style border.
		/// </summary>
		Border=2,
		/// <summary>
		/// Represents style margin.
		/// </summary>
		Margin=4
	}
	
	/// <summary>
	/// Indicates the style side.
	/// </summary>
	public enum eStyleSide
	{
		/// <summary>
		/// Specifies left side of the style.
		/// </summary>
		Left,
		/// <summary>
		/// Specifies right side of the style.
		/// </summary>
		Right,
		/// <summary>
		/// Specifies top side of the style.
		/// </summary>
		Top,
		/// <summary>
		/// Specifies bottom side of the style.
		/// </summary>
		Bottom
	}

	/// <summary>
	/// Specifies the button state.
	/// </summary>
	public enum eButtonState
	{
		/// <summary>
		/// Button is in it's default state.
		/// </summary>
		Normal,
		/// <summary>
		/// Button is disabled
		/// </summary>
		Disabled,
		/// <summary>
		/// Mouse is over the button
		/// </summary>
		MouseOver,
		/// <summary>
		/// Left mouse button is pressed
		/// </summary>
		MouseDownLeft,
		/// <summary>
		/// Right mouse button is pressed
		/// </summary>
		MouseDownRight,
	}

    /// <summary>
    /// Indicates predefined color scheme assigned to super tooltip.
    /// </summary>
    public enum eTooltipColor
    {
        Default,
        Blue,
        Yellow,
        Green,
        Red,
        Purple,
        Cyan,
        Orange,
        Magenta,
        BlueMist,
        PurpleMist,
        Tan,
        Lemon,
        Apple,
        Teal,
        Silver,
        Office2003,
        Gray,
        System
    }

    /// <summary>
    /// Specifies predefined color assigned to ribbon items.
    /// </summary>
    public enum eRibbonTabColor
    {
        Default,
        Magenta,
        Orange,
        Green
    }

    /// <summary>
    /// Specifies predefined color assigned to ribbon tab groups.
    /// </summary>
    public enum eRibbonTabGroupColor
    {
        Default,
        Magenta,
        Orange,
        Green
    }

    /// <summary>
    /// Specifies the state of the Wizard button.
    /// </summary>
    public enum eWizardButtonState
    {
        True,
        False,
        Auto
    }

    /// <summary>
    /// Specifies the button that caused the wizard page change.
    /// </summary>
    public enum eWizardPageChangeSource
    {
        /// <summary>
        /// Page change was started using Wizard Back button.
        /// </summary>
        BackButton,
        /// <summary>
        /// Page change was started using Wizard Next button.
        /// </summary>
        NextButton,
        /// <summary>
        /// Page change was started from code.
        /// </summary>
        Code
    }

    /// <summary>
    /// Specifies wizard button that is clicked when the user presses the ENTER key.
    /// </summary>
    public enum eWizardFormAcceptButton
    {
        /// <summary>
        /// If finish button is enabled and visible it will be clicked otherwise click next button
        /// </summary>
        FinishAndNext,
        /// <summary>
        /// Click finish button
        /// </summary>
        Finish,
        /// <summary>
        /// Click next button
        /// </summary>
        Next,
        /// <summary>
        /// No button will be clicked
        /// </summary>
        None
    }

    /// <summary>
    /// Specifies wizard button that is clicked when the user presses the Escape key.
    /// </summary>
    public enum eWizardFormCancelButton
    {
        /// <summary>
        /// Cancel button will be clicked
        /// </summary>
        Cancel,
        /// <summary>
        /// No button will be clicked
        /// </summary>
        None
    }

    /// <summary>
    /// Specifies border around ColorItem.
    /// </summary>
    [System.Flags()]
	public enum eColorItemBorder
	{
		/// <summary>
		/// No Border.
		/// </summary>
		None=0,
		/// <summary>
		/// Border on the Left edge.
		/// </summary>
		Left=1,
		/// <summary>
		/// Border on the Right edge.
		/// </summary>
		Right=2,
		/// <summary>
		/// Border on the Top edge.
		/// </summary>
		Top=4,
		/// <summary>
		/// Border on the Bottom edge.
		/// </summary>
		Bottom=8,
		/// <summary>
		/// Border on all 4 sides.
		/// </summary>
		All=(Left | Right | Top | Bottom)
	}

    /// <summary>
    /// Specifies the line alignment of the items inside of the container.
    /// </summary>
    public enum eContainerVerticalAlignment
    {
        /// <summary>
        /// Items are aligned to the top.
        /// </summary>
        Top,
        /// <summary>
        /// Items are aligned to the middle point of the line.
        /// </summary>
        Middle,
        /// <summary>
        /// Items are aligned to the bottom.
        /// </summary>
        Bottom
    }

    /// <summary>
    /// Specifies the alignment of the items inside of the container in horizontal layout.
    /// </summary>
    public enum eHorizontalItemsAlignment
    {
        /// <summary>
        /// Items are left aligned.
        /// </summary>
        Left,
        /// <summary>
        /// Items are centered.
        /// </summary>
        Center,
        /// <summary>
        /// Items are right aligned.
        /// </summary>
        Right
    }

    /// <summary>
    /// Specifies the alignment of the items inside of the container in vertical layout.
    /// </summary>
    public enum eVerticalItemsAlignment
    {
        /// <summary>
        /// Items are top aligned.
        /// </summary>
        Top,
        /// <summary>
        /// Items are in the middle.
        /// </summary>
        Middle,
        /// <summary>
        /// Items are Bottom aligned.
        /// </summary>
        Bottom
    }

    /// <summary>
    /// Specifies the rendering mode used by a user interface element.
    /// </summary>
    public enum eRenderMode
    {
        /// <summary>
        /// Indicates that rendering on user interface element instance set through Renderer property is used.
        /// </summary>
        Instance,
        /// <summary>
        /// Indicates that global application wide renderer is used as specified by GlobalManager.Renderer property.
        /// </summary>
        Global,
        /// <summary>
        /// Indicates that custom rendered will be used for an user interface element. Renderer property must be set when using this value to the renderer
        /// that will be used.
        /// </summary>
        Custom
    }

    /// <summary>
    /// Specifies the predefined color table for button.
    /// </summary>
    public enum eButtonColor
    {
        Blue,
        BlueWithBackground,
        Orange,
        OrangeWithBackground,
        Magenta,
        MagentaWithBackground,
        Office2007WithBackground,
        BlueOrb,
        Flat
    }

    /// <summary>
    /// Specifies the color table used to render ProgressBarItem in Office 2007 style.
    /// </summary>
    public enum eProgressBarItemColor
    {
        /// <summary>
        /// Indicates default Normal color table, usually green.
        /// </summary>
        Normal,
        /// <summary>
        /// Indicates Pause state color table, usually yellow.
        /// </summary>
        Paused,
        /// <summary>
        /// Indicates Error state color table, usually red.
        /// </summary>
        Error
    }

    /// <summary>
    /// Specifies the position of ribbon title.
    /// </summary>
    public enum eRibbonTitlePosition
    {
        /// <summary>
        /// Title is positioned on the top of the ribbon.
        /// </summary>
        Top,
        /// <summary>
        /// Title is positioned on the bottom of the ribbon.
        /// </summary>
        Bottom
    }

    /// <summary>
    /// Specifies text alignment on the button.
    /// </summary>
    public enum eButtonTextAlignment
    {
        /// <summary>
        /// Specifies the left aligned text.
        /// </summary>
        Left,
        /// <summary>
        /// Specifies the center aligned text.
        /// </summary>
        Center,
        /// <summary>
        /// Specifies the right aligned text.
        /// </summary>
        Right
    }

    /// <summary>
    /// Specifies the position of the tab close button.
    /// </summary>
    public enum eTabCloseButtonPosition
    {
        /// <summary>
        /// Close button is on the left side of the tab.
        /// </summary>
        Left,
        /// <summary>
        /// Close button is on the right side of the tab.
        /// </summary>
        Right
    }

    /// <summary>
    /// Describes the bar type.
    /// </summary>
    public enum eBarType
    {
        /// <summary>
        /// Indicates that bar is toolbar.
        /// </summary>
        Toolbar,
        /// <summary>
        /// Indicates that bar is menu bar.
        /// </summary>
        MenuBar,
        /// <summary>
        /// Indicates that bar is status bar.
        /// </summary>
        StatusBar,
        /// <summary>
        /// Indicates that bar is dock window.
        /// </summary>
        DockWindow
    }

    /// <summary>
    /// Describes the categorization mode used to categorize items on the Customize Ribbon dialog.
    /// </summary>
    public enum eCategorizeMode
    {
        /// <summary>
        /// Items are automatically categorized by the ribbon bar they appear on.
        /// </summary>
        RibbonBar,
        /// <summary>
        /// Items are categorized by the Category property on each item. Category property should be set on each item.
        /// </summary>
        Categories
    }

    /// <summary>
    /// Describes the check-box item appearance style
    /// </summary>
    public enum eCheckBoxStyle
    {
        /// <summary>
        /// Standard check-box style.
        /// </summary>
        CheckBox,
        /// <summary>
        /// Radio button style. Only one button can be selected/checked in given container.
        /// </summary>
        RadioButton
    }

    /// <summary>
    /// Indicates the position of the check box sign related to the text for CheckBoxItem.
    /// </summary>
    public enum eCheckBoxPosition
    {
        /// <summary>
        /// Check box sign is positioned on the left side of the text.
        /// </summary>
        Left,
        /// <summary>
        /// Check box sign is positioned on the right side of the text.
        /// </summary>
        Right,
        /// <summary>
        /// Check box sign is positioned above the text.
        /// </summary>
        Top,
        /// <summary>
        /// Check box sing is positioned below the text
        /// </summary>
        Bottom
    }

    /// <summary>
    /// Indicates the type of the progress bar.
    /// </summary>
    public enum eProgressItemType
    {
        /// <summary>
        /// Standard step based progress bar.
        /// </summary>
        Standard,
        /// <summary>
        /// The automatically moving progress bar.
        /// </summary>
        Marquee
    }

    /// <summary>
    /// Defines the direction of collapsing/expanding 
    /// </summary>
    public enum eCollapseDirection
    {
        /// <summary>
        /// Control is collapsed from bottom to top.
        /// </summary>
        BottomToTop,
        /// <summary>
        /// Control is collapsed from top to bottom.
        /// </summary>
        TopToBottom,
        /// <summary>
        /// Control is collapsed from right to left.
        /// </summary>
        RightToLeft,
        /// <summary>
        /// Control is collapsed from left to right.
        /// </summary>
        LeftToRight
    }

    /// <summary>
    /// Describes the scroll bar skinning applied to the controls.
    /// </summary>
    public enum eScrollBarSkin
    {
        /// <summary>
        /// No scrollbar skinning is applied to the control.
        /// </summary>
        None,
        /// <summary>
        /// Optimized scrollbar skinning algorithm is used. Might provide better appearance in certain scenarios.
        /// </summary>
        Optimized,
        /// <summary>
        /// Unoptimized scrollbar skinning algorithm is used. Might provide better appearance in certain scenarios.
        /// </summary>
        Unoptimized
    }

    /// <summary>
    /// Indicates the position of the slider label text related to the slider part.
    /// </summary>
    public enum eSliderLabelPosition
    {
        /// <summary>
        /// Label is positioned on the left side of the slider.
        /// </summary>
        Left,
        /// <summary>
        /// Label is positioned on the right side of the slider.
        /// </summary>
        Right,
        /// <summary>
        /// Label is positioned above the slider.
        /// </summary>
        Top,
        /// <summary>
        /// Label is positioned below the slider.
        /// </summary>
        Bottom
    }

    /// <summary>
    /// Defines the slider item parts.
    /// </summary>
    public enum eSliderPart
    {
        /// <summary>
        /// Indicates no part.
        /// </summary>
        None,
        /// <summary>
        /// Indicates the increase button of slider control.
        /// </summary>
        IncreaseButton,
        /// <summary>
        /// Indicates the decrease button of slider control.
        /// </summary>
        DecreaseButton,
        /// <summary>
        /// Indicates the label part of slider control.
        /// </summary>
        Label,
        /// <summary>
        /// Indicates the track area part of the control.
        /// </summary>
        TrackArea
    }

    /// <summary>
    /// Specifies the position of the title image.
    /// </summary>
    public enum eTitleImagePosition
    {
        /// <summary>
        /// Image is positioned on the left side.
        /// </summary>
        Left,
        /// <summary>
        /// Image is centered.
        /// </summary>
        Center,
        /// <summary>
        /// Image is positioned on the right side.
        /// </summary>
        Right
    }

    /// <summary>
    /// Specifies the Wizard control Appearance.
    /// </summary>
    public enum eWizardStyle
    {
        /// <summary>
        /// Indicates default Wizard 97 style.
        /// </summary>
        Default,
        /// <summary>
        /// Indicates the Office 2007 Style Wizard Appearance.
        /// </summary>
        Office2007
    }

    /// <summary>
    /// Specifies wizard title image alignment
    /// </summary>
    public enum eWizardTitleImageAlignment
    {
        /// <summary>
        /// Image is aligned to left
        /// </summary>
        Left,
        /// <summary>
        /// Image is aligned to right
        /// </summary>
        Right
    }

    /// <summary>
    /// Specifies the behaviour used to hide watermark.
    /// </summary>
    public enum eWatermarkBehavior
    {
        /// <summary>
        /// Watermark for control is hidden when control receives the input focus.
        /// </summary>
        HideOnFocus,
        /// <summary>
        /// Watermark for control is hidden when control has non-empty input value.
        /// </summary>
        HideNonEmpty
    }

    /// <summary>
    /// Specifies the advanced ScrollBar appearance.
    /// </summary>
    public enum eScrollBarAppearance
    {
        /// <summary>
        /// Default scroll bar appearance.
        /// </summary>
        Default,
        /// <summary>
        /// Office 2007 style Application scroll bar appearance.
        /// </summary>
        ApplicationScroll
    }

    /// <summary>
    /// Defines text position in relation to the content of the item..
    /// </summary>
    public enum eTextPosition
    {
        /// <summary>
        /// Text is positioned to the left of the content.
        /// </summary>
        Left,
        /// <summary>
        /// Text is positioned to the right of the content.
        /// </summary>
        Right,
        /// <summary>
        /// Text is positioned on top of the content.
        /// </summary>
        Top,
        /// <summary>
        /// Text is positioned on bottom of the content.
        /// </summary>
        Bottom
    }
}