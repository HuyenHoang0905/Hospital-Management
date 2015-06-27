using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Controls
{
    [ToolboxBitmap(typeof(DataGridViewButtonXColumn), "ButtonX.ButtonX.ico"), ToolboxItem(false), ComVisible(false)]
    public class DataGridViewButtonXColumn : DataGridViewButtonColumn, IDataGridViewColumn
    {
        #region Events

        [Description("Occurs right before a Button Cell is painted.")]
        public event EventHandler<BeforeCellPaintEventArgs> BeforeCellPaint;

        [Description("Occurs when a Button Cell is Clicked.")]
        public event EventHandler<EventArgs> Click;

        #endregion

        #region Private variables

        private ButtonX _ButtonX;

        private int _ActiveRowIndex = -1;
        private int _CurrentRowIndex = -1;

        private bool _ExpandClosed;
        private bool _InCellCallBack;

        private Bitmap _CellBitmap;

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public DataGridViewButtonXColumn()
        {
            CellTemplate = new DataGridViewButtonXCell();

            _ButtonX = new ButtonX();

            _ButtonX.Visible = false;
            _ButtonX.FadeEffect = false;

            HookEvents(true);
        }

        #region Internal properties

        #region ActiveRowIndex

        /// <summary>
        /// Gets or sets the active row index
        /// </summary>
        internal int ActiveRowIndex
        {
            get { return (_ActiveRowIndex); }
            set { _ActiveRowIndex = value; }
        }

        #endregion

        #region ButtonX

        /// <summary>
        /// Gets the Control Button
        /// </summary>
        internal ButtonX ButtonX
        {
            get { return (_ButtonX); }
        }

        #endregion

        #region CurrentRowIndex

        /// <summary>
        /// Gets or sets the Current row index
        /// </summary>
        internal int CurrentRowIndex
        {
            get { return (_CurrentRowIndex); }
            set { _CurrentRowIndex = value; }
        }

        #endregion

        #region ExpandClosed

        /// <summary>
        /// Gets or sets a expanded button
        /// was just closed
        /// </summary>
        internal bool ExpandClosed
        {
            get { return (_ExpandClosed); }
            set { _ExpandClosed = value; }
        }

        #endregion

        #region InCellCallBack

        /// <summary>
        /// Gets or sets the cell callback state
        /// </summary>
        internal bool InCellCallBack
        {
            get { return (_InCellCallBack); }
            set { _InCellCallBack = value; }
        }

        #endregion

        #endregion

        #region Public properties

        #region AutoCheckOnClick

        /// <summary>
        /// Gets or sets whether Checked property is automatically inverted, button checked/unchecked, when button is clicked. Default value is false.
        /// </summary>
        [Browsable(true), DefaultValue(false), Category("Behavior")]
        [Description("Indicates whether Checked property is automatically inverted when button is clicked.")]
        public bool AutoCheckOnClick
        {
            get { return (_ButtonX.AutoCheckOnClick); }
            set { _ButtonX.AutoCheckOnClick = value; }
        }

        #endregion

        #region AutoExpandOnClick

        /// <summary>
        /// Indicates whether the button will auto-expand when clicked. 
        /// When button contains sub-items, sub-items will be shown only if user
        /// click the expand part of the button. Setting this property to true will expand the button and show sub-items when user
        /// clicks anywhere inside of the button. Default value is false which indicates that button is expanded only
        /// if its expand part is clicked.
        /// </summary>
        [DefaultValue(false), Browsable(true), DevCoBrowsable(true), Category("Behavior")]
        [Description("Indicates whether the button will auto-expand (display pop-up menu or toolbar) when clicked.")]
        public virtual bool AutoExpandOnClick
        {
            get { return (_ButtonX.AutoExpandOnClick); }
            set { _ButtonX.AutoExpandOnClick = value; }
        }

        #endregion

        #region Checked

        /// <summary>
        /// Gets or set a value indicating whether the button is in the checked state.
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), Category("Appearance"), DefaultValue(false)]
        [Description("Indicates whether item is checked or not.")]
        public virtual bool Checked
        {
            get { return (_ButtonX.Checked); }
            set { _ButtonX.Checked = value; }
        }

        #endregion

        #region ColorScheme

        /// <summary>
        /// Gets or sets button Color Scheme. ColorScheme does not apply to Office2007 styled buttons.
        /// </summary>
        [Browsable(false), DevCoBrowsable(false), Category("Appearance")]
        [Description("Gets or sets Bar Color Scheme."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ColorScheme ColorScheme
        {
            get { return (_ButtonX.ColorScheme); }
            set { _ButtonX.ColorScheme = value; }
        }

        #endregion

        #region ColorTable

        /// <summary>
        /// Gets or sets the predefined color of the button. Color specified applies to buttons with Office 2007 style only. It does not have
        /// any effect on other styles. Default value is eButtonColor.BlueWithBackground
        /// </summary>
        [Browsable(true), DevCoBrowsable(false), DefaultValue(eButtonColor.BlueWithBackground), Category("Appearance")]
        [Description("Indicates predefined color of button when Office 2007 style is used.")]
        [NotifyParentProperty(true)]
        public eButtonColor ColorTable
        {
            get { return (_ButtonX.ColorTable); }
            set { _ButtonX.ColorTable = value; }
        }

        #endregion

        #region CustomColorName

        /// <summary>
        /// Gets or sets the custom color name. Name specified here must be represented by the corresponding object with the same name that is part
        /// of the Office2007ColorTable.ButtonItemColors collection. See documentation for Office2007ColorTable.ButtonItemColors for more information.
        /// If color table with specified name cannot be found default color will be used. Valid settings for this property override any
        /// setting to the Color property. Applies to items with Office 2007 style only.
        /// </summary>
        [Browsable(true), DevCoBrowsable(false), DefaultValue(""), Category("Appearance")]
        [Description("Indicates custom color table name for the button when Office 2007 style is used.")]
        public string CustomColorName
        {
            get { return (_ButtonX.CustomColorName); }
            set { _ButtonX.CustomColorName = value; }
        }

        #endregion

        #region DisabledImage

        /// <summary>
        /// Specifies the image for the button when items Enabled property is set to false.
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), DefaultValue(null), Category("Appearance")]
        [Description("The image that will be displayed when item is disabled.")]
        public Image DisabledImage
        {
            get { return (_ButtonX.DisabledImage); }
            set { _ButtonX.DisabledImage = value; }
        }

        #endregion

        #region EnableMarkup

        /// <summary>
        /// Gets or sets whether text-markup support is enabled for items Text property. Default value is true.
        /// Set this property to false to display HTML or other markup in the item instead of it being parsed as text-markup.
        /// </summary>
        [Browsable(true), DefaultValue(true), Category("Appearance")]
        [Description("Indicates whether text-markup support is enabled for items Text property.")]
        public bool EnableMarkup
        {
            get { return (_ButtonX.EnableMarkup); }
            set { _ButtonX.EnableMarkup = value; }
        }

        #endregion

        #region Enabled

        /// <summary>
        /// Gets or sets whether the control can respond to user interaction
        /// </summary>
        [Browsable(true), DefaultValue(true), Category("Behavior")]
        [Description("Indicates whether the control can respond to user interaction.")]
        public bool Enabled
        {
            get { return (_ButtonX.Enabled); }
            set { _ButtonX.Enabled = value; }
        }

        #endregion

        #region HotTrackingStyle

        /// <summary>
        /// Indicates the way button is rendering the mouse over state. Setting the value to
        /// Color will render the image in gray-scale when mouse is not over the item.
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), DefaultValue(eHotTrackingStyle.Default), Category("Appearance")]
        [Description("Indicates the button mouse over tracking style. Setting the value to Color will render the image in gray-scale when mouse is not over the item.")]
        public eHotTrackingStyle HotTrackingStyle
        {
            get { return (_ButtonX.HotTrackingStyle); }
            set { _ButtonX.HotTrackingStyle = value; }
        }

        #endregion

        #region HoverImage

        /// <summary>
        /// Specifies the image for the button when mouse is over the item.
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), DefaultValue(null), Category("Appearance")]
        [Description("The image that will be displayed when mouse hovers over the item.")]
        public Image HoverImage
        {
            get { return (_ButtonX.HoverImage); }
            set { _ButtonX.HoverImage = value; }
        }

        #endregion

        #region Image

        /// <summary>
        /// Specifies the Button image.
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), DefaultValue(null), Category("Appearance")]
        [Description("The image that will be displayed on the face of the button.")]
        public Image Image
        {
            get { return (_ButtonX.Image); }
            set { _ButtonX.Image = value; }
        }

        #endregion

        #region ImageFixedSize

        /// <summary>
        /// Sets fixed size of the image. Image will be scaled and painted it size specified.
        /// </summary>
        [Browsable(true)]
        public Size ImageFixedSize
        {
            get { return (_ButtonX.ImageFixedSize); }
            set { _ButtonX.ImageFixedSize = value; }
        }

        /// <summary>
        /// Gets whether ImageFixedSize property should be serialized.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeImageFixedSize()
        {
            return (_ButtonX.ShouldSerializeImageFixedSize());
        }

        #endregion

        #region ImagePosition

        /// <summary>
        /// Gets/Sets the image position inside the button.
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), DefaultValue(eImagePosition.Left), Category("Appearance")]
        [Description("The alignment of the image in relation to text displayed by this item.")]
        public eImagePosition ImagePosition
        {
            get { return (_ButtonX.ImagePosition); }
            set { _ButtonX.ImagePosition = value; }
        }

        #endregion

        #region ImageTextSpacing

        /// <summary>
        /// Gets or sets the amount of spacing between button image if specified and text.
        /// </summary>
        [Browsable(true), DefaultValue(0), Category("Layout")]
        [Description("Indicates amount of spacing between button image if specified and text.")]
        public int ImageTextSpacing
        {
            get { return (_ButtonX.ImageTextSpacing); }
            set { _ButtonX.ImageTextSpacing = value; }

        }

        #endregion

        #region PopupSide

        /// <summary>
        /// Gets or sets the location of popup in relation to it's parent.
        /// </summary>
        [Browsable(true), DevCoBrowsable(false), DefaultValue(ePopupSide.Default)]
        [Description("Indicates location of popup in relation to it's parent.")]
        public ePopupSide PopupSide
        {
            get { return (_ButtonX.PopupSide); }
            set { _ButtonX.PopupSide = value; }
        }

        #endregion

        #region PressedImage

        /// <summary>
        /// Specifies the image for the button when mouse left button is pressed.
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), DefaultValue(null), Category("Appearance")]
        [Description("The image that will be displayed when item is pressed.")]
        public Image PressedImage
        {
            get { return (_ButtonX.PressedImage); }
            set { _ButtonX.PressedImage = value; }
        }

        #endregion

        #region Shape

        /// <summary>
        /// Gets or sets an shape descriptor for the button
        /// which describes the shape of the button. Default value is null
        /// which indicates that system default shape is used.
        /// </summary>
        [DefaultValue(null)]
        [Editor("DevComponents.DotNetBar.Design.ShapeTypeEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
        [TypeConverter("DevComponents.DotNetBar.Design.ShapeStringConverter, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
        public ShapeDescriptor Shape
        {
            get { return (_ButtonX.Shape); }
            set { _ButtonX.Shape = value; }
        }

        #endregion

        #region ShowSubItems

        /// <summary>
        /// Gets or sets whether button displays the expand part that indicates that button has popup.
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), DefaultValue(true), Category("Behavior")]
        [Description("Determines whether sub-items are displayed.")]
        public bool ShowSubItems
        {
            get { return (_ButtonX.ShowSubItems); }
            set { _ButtonX.ShowSubItems = value; }
        }

        #endregion

        #region SplitButton

        /// <summary>
        /// Gets or sets whether button appears as split button. Split button appearance
        /// divides button into two parts. Image which raises the click event when clicked
        /// and text and expand sign which shows button sub items on popup menu when clicked.
        /// Button must have both text and image visible (ButtonStyle property) in order to
        /// appear as a full split button.
        /// </summary>
        [Browsable(true), DefaultValue(false), Category("Appearance")]
        [Description("Indicates whether button appears as split button.")]
        public bool SplitButton
        {
            get { return (_ButtonX.SplitButton); }
            set { _ButtonX.SplitButton = value; }
        }

        #endregion

        #region Style

        /// <summary>
        /// Gets/Sets the visual style for the button.
        /// </summary>
        [Category("Appearance"), Description("Specifies the visual style of the button.")]
        [DefaultValue(eDotNetBarStyle.Office2007)]
        public eDotNetBarStyle Style
        {
            get { return (_ButtonX.Style); }
            set { _ButtonX.Style = value; }
        }

        #endregion

        #region SubItems

        /// <summary>
        /// Returns the collection of sub items.
        /// </summary>
        [Browsable(false), DevCoBrowsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SubItemsCollection SubItems
        {
            get { return (_ButtonX.SubItems); }
        }

        #endregion

        #region SubItemsExpandWidth

        /// <summary>
        /// Gets or sets the width of the expand part of the button item.
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), DefaultValue(12), Category("Behavior")]
        [Description("Indicates the width of the expand part of the button item.")]
        public int SubItemsExpandWidth
        {
            get { return (_ButtonX.SubItemsExpandWidth); }
            set { _ButtonX.SubItemsExpandWidth = value; }
        }

        #endregion

        #region Text

        /// <summary>
        /// Gets or sets the default Text to display on the Button
        /// </summary>
        [Browsable(true), Category("Appearance"), DefaultValue("")]
        [Description("Indicates the default Text to display on the Button.")]
        public new string Text
        {
            get
            {
                return (_InCellCallBack == true ?
                    _ButtonX.Text : base.Text);
            }

            set
            {
                if (_InCellCallBack == true)
                    _ButtonX.Text = value;
                else
                    base.Text = value;
            }
        }

        #endregion

        #region TextAlignment

        /// <summary>
        /// Gets or sets the text alignment. Applies only when button text is not composed using text markup. Default value is center.
        /// </summary>
        [Browsable(true), DefaultValue(eButtonTextAlignment.Center), Category("Appearance")]
        [Description("Indicates text alignment. Applies only when button text is not composed using text markup. Default value is center.")]
        public eButtonTextAlignment TextAlignment
        {
            get { return (_ButtonX.TextAlignment); }
            set { _ButtonX.TextAlignment = value; }
        }

        #endregion

        #endregion

        #region HookEvents

        /// <summary>
        /// Hooks or unhooks our system events
        /// </summary>
        /// <param name="hook"></param>
        private void HookEvents(bool hook)
        {
            if (hook == true)
            {
                _ButtonX.ButtonItem.Click += ButtonItem_Click;
                _ButtonX.ButtonItem.ExpandChange += ButtonItem_ExpandChange;
            }
            else
            {
                _ButtonX.ButtonItem.Click -= ButtonItem_Click;
                _ButtonX.ButtonItem.ExpandChange -= ButtonItem_ExpandChange;
            }
        }

        #endregion

        #region Event processing

        #region ButtonItem_ExpandChange

        /// <summary>
        /// Processes Button expand changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ButtonItem_ExpandChange(object sender, EventArgs e)
        {
            if (_ButtonX.ButtonItem.Expanded == false)
            {
                DataGridView.InvalidateCell(Index, ActiveRowIndex);

                _ExpandClosed = (_ActiveRowIndex == _CurrentRowIndex);
                _ActiveRowIndex = -1;

                if (_CurrentRowIndex >= 0)
                {
                    DataGridViewButtonXCell cell =
                        DataGridView.Rows[_CurrentRowIndex].Cells[Index] as DataGridViewButtonXCell;

                    if (cell != null)
                        cell.DoMouseEnter(_CurrentRowIndex);
                }
            }
        }

        #endregion

        #region ButtonItem_Click

        /// <summary>
        /// ButtonItem_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ButtonItem_Click(object sender, EventArgs e)
        {
            if (Click != null)
                Click(sender, e);
        }

        #endregion

        #endregion

        #region GetCellBitmap

        /// <summary>
        /// Gets the cell paint bitmap
        /// </summary>
        /// <param name="cellBounds"></param>
        /// <returns></returns>
        internal Bitmap GetCellBitmap(Rectangle cellBounds)
        {
            if (_CellBitmap == null ||
                (_CellBitmap.Width != cellBounds.Width || _CellBitmap.Height < cellBounds.Height))
            {
                if (_CellBitmap != null)
                    _CellBitmap.Dispose();

                _CellBitmap = new Bitmap(cellBounds.Width, cellBounds.Height);
            }

            return (_CellBitmap);
        }

        #endregion

        #region OnBeforeCellPaint

        /// <summary>
        /// Invokes BeforeCellPaint user events
        /// </summary>
        /// <param name="rowIndex">Row index</param>
        /// <param name="columnIndex">Column index</param>
        internal void OnBeforeCellPaint(int rowIndex, int columnIndex)
        {
            if (BeforeCellPaint != null)
                BeforeCellPaint(this, new BeforeCellPaintEventArgs(rowIndex, columnIndex));
        }

        #endregion

        #region ICloneable members

        /// <summary>
        /// Clones the ButtonX Column
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            DataGridViewButtonXColumn bc = base.Clone() as DataGridViewButtonXColumn;

            if (bc != null)
            {
                _ButtonX.ButtonItem.InternalCopyToItem(bc.ButtonX.ButtonItem);

                bc.Enabled = Enabled;
                bc.TextAlignment = TextAlignment;
                bc.ImageTextSpacing = ImageTextSpacing;

                bc.Shape = Shape;
            }

            return (bc);
        }

        #endregion

        #region IDataGridViewColumn Members

        /// <summary>
        /// Gets the Cell paint setting for the ButtonX control
        /// </summary>
        [Browsable(false)]
        public bool OwnerPaintCell
        {
            get { return (true); }
        }

        #endregion

        #region Dispose

        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            HookEvents(false);
            base.Dispose(disposing);
        }

        #endregion
    }

    #region BeforeCellPaintEventArgs

    /// <summary>
    /// BeforeCellPaintEventArgs
    /// </summary>
    public class BeforeCellPaintEventArgs : EventArgs
    {
        #region Private variables

        private int _RowIndex;
        private int _ColumnIndex;

        #endregion

        public BeforeCellPaintEventArgs(int rowIndex, int columnIndex)
        {
            _RowIndex = rowIndex;
            _ColumnIndex = columnIndex;
        }

        #region Public properties

        /// <summary>
        /// RowIndex of cell being painted
        /// </summary>
        public int RowIndex
        {
            get { return (_RowIndex); }
        }

        /// <summary>
        /// ColumnIndex of cell being painted
        /// </summary>
        public int ColumnIndex
        {
            get { return (_ColumnIndex); }
        }

        #endregion
    }

    #endregion
}
