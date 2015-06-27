using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Controls
{
    [ToolboxBitmap(typeof(DataGridViewLabelXColumn), "Controls.LabelX.ico"), ToolboxItem(false), ComVisible(false)]
    public class DataGridViewLabelXColumn : DataGridViewButtonColumn, IDataGridViewColumn
    {
        #region Events

        [Description("Occurs right before a LabelX Cell is painted.")]
        public event EventHandler<BeforeCellPaintEventArgs> BeforeCellPaint;

        [Description("Occurs when a LabelX Cell is Clicked.")]
        public event EventHandler<EventArgs> Click;

        #endregion

        #region Private variables

        private LabelX _LabelX;
        private Bitmap _CellBitmap;

        private bool _InCellCallBack;

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public DataGridViewLabelXColumn()
        {
            CellTemplate = new DataGridViewLabelXCell();

            _LabelX = new LabelX();
            _LabelX.Visible = false;

            HookEvents(true);
        }

        #region Hidden properties

        /// <summary>
        /// Button FlatStyle
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new FlatStyle FlatStyle
        {
            get { return base.FlatStyle; }
            set { base.FlatStyle = value; }
        }

        /// <summary>
        /// Button UseColumnTextForButtonValue
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new bool UseColumnTextForButtonValue
        {
            get { return base.UseColumnTextForButtonValue; }
            set { base.UseColumnTextForButtonValue = value; }
        }

        #endregion

        #region Internal properties

        #region InCallBack

        /// <summary>
        /// InCallBack
        /// </summary>
        internal bool InCallBack
        {
            get { return (_InCellCallBack); }
            set { _InCellCallBack = value; }
        }

        #endregion

        #region LabelX

        /// <summary>
        /// Gets the Control LabelX
        /// </summary>
        internal LabelX LabelX
        {
            get { return (_LabelX); }
        }

        #endregion

        #endregion

        #region Public properties

        #region EnableMarkup

        /// <summary>
        /// Gets or sets whether text-markup support is enabled for items Text property. Default value is true.
        /// Set this property to false to display HTML or other markup in the item instead of it being parsed as text-markup.
        /// </summary>
        [Browsable(true), DefaultValue(true), Category("Appearance")]
        [Description("Indicates whether text-markup support is enabled for the item's Text property.")]
        public bool EnableMarkup
        {
            get { return (_LabelX.EnableMarkup); }
            set { _LabelX.EnableMarkup = value; }
        }

        #endregion

        #region FocusCuesEnabled

        /// <summary>
        /// Gets or sets whether control displays focus cues when focused.
        /// </summary>
        [Browsable(true), DefaultValue(false), Category("Behavior")]
        [Description("Indicates whether control displays focus cues when focused.")]
        public bool FocusCuesEnabled
        {
            get { return (_LabelX.FocusCuesEnabled); }
            set { _LabelX.FocusCuesEnabled = value; }
        }

        #endregion

        #region BorderSide

        /// <summary>
        /// Gets or sets the border sides that are displayed.
        /// Default value specifies border on all 4 sides.
        /// </summary>
        [Browsable(false), Category("Appearance"), DefaultValue(LabelItem.DEFAULT_BORDERSIDE)]
        [Description("Specifies border sides that are displayed.")]
        public eBorderSide BorderSide
        {
            get { return (_LabelX.BorderSide); }
            set { _LabelX.BorderSide = value; }
        }

        #endregion

        #region BorderType

        /// <summary>
        /// Gets or sets the type of the border drawn around the label.
        /// </summary>
        [Browsable(false), Category("Appearance"), DefaultValue(eBorderType.None)]
        [Description("Indicates the type of the border drawn around the label.")]
        public eBorderType BorderType
        {
            get { return (_LabelX.BorderType); }
            set { _LabelX.BorderType = value; }
        }

        #endregion

        #region Image

        /// <summary>
        /// Specifies label image.
        /// </summary>
        [Browsable(true), Category("Appearance"), DefaultValue(null)]
        [Description("The image that will be displayed on the face of the item.")]
        public Image Image
        {
            get { return (_LabelX.Image); }
            set { _LabelX.Image = value; }
        }

        #endregion

        #region ImagePosition

        /// <summary>
        /// Gets/Sets the image position inside the label.
        /// </summary>
        [Browsable(true), Category("Appearance"), DefaultValue(eImagePosition.Left)]
        [Description("The alignment of the image in relation to text displayed by this item.")]
        public eImagePosition ImagePosition
        {
            get { return (_LabelX.ImagePosition); }
            set { _LabelX.ImagePosition = value; }
        }

        #endregion

        #region PaddingBottom

        /// <summary>
        /// Gets or sets the bottom padding in pixels.
        /// </summary>
        [Browsable(true), DefaultValue(0), Category("Layout")]
        [Description("Indicates bottom padding in pixels.")]
        public int PaddingBottom
        {
            get { return (_LabelX.PaddingBottom); }
            set { _LabelX.PaddingBottom = value; }
        }

        #endregion

        #region PaddingLeft

        /// <summary>
        /// Gets or sets the left padding in pixels.
        /// </summary>
        [Browsable(true), DefaultValue(0), Category("Layout")]
        [Description("Indicates left padding in pixels.")]
        public int PaddingLeft
        {
            get { return (_LabelX.PaddingLeft); }
            set { _LabelX.PaddingLeft = value; }
        }

        #endregion

        #region PaddingTop

        /// <summary>
        /// Gets or sets the top padding in pixels.
        /// </summary>
        [Browsable(true), DefaultValue(0), Category("Layout")]
        [Description("Indicates top padding in pixels.")]
        public int PaddingTop
        {
            get { return (_LabelX.PaddingTop); }
            set { _LabelX.PaddingTop = value; }
        }

        #endregion

        #region PaddingRight

        /// <summary>
        /// Gets or sets the right padding in pixels.
        /// </summary>
        [Browsable(true), DefaultValue(0), Category("Layout")]
        [Description("Indicates right padding in pixels.")]
        public int PaddingRight
        {
            get { return (_LabelX.PaddingRight); }
            set { _LabelX.PaddingRight = value; }
        }

        #endregion

        #region SingleLineColor

        /// <summary>
        /// Gets or sets the border line color when border is single line.
        /// </summary>
        [Browsable(true), Category("Appearance")]
        [Description("Indicates border line color when border is single line.")]
        public Color SingleLineColor
        {
            get { return (_LabelX.SingleLineColor); }
            set { _LabelX.SingleLineColor = value; }
        }

        /// <summary>
        /// Gets whether property should be serialized.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeSingleLineColor()
        {
            return (_LabelX.ShouldSerializeSingleLineColor());
        }

        /// <summary>
        /// Resets the SingleLineColor property to its default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetSingleLineColor()
        {
            _LabelX.ResetSingleLineColor();
        }

        #endregion

        #region Text

        /// <summary>
        /// Gets or sets the text associated with this item.
        /// </summary>
        [Browsable(true), Category("Appearance")]
        [Editor("DevComponents.DotNetBar.Design.TextMarkupUIEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(System.Drawing.Design.UITypeEditor))]
        [Description("The text contained in the item.")]
        public new string Text
        {
            get
            {
                return (_InCellCallBack == true ?
                    _LabelX.Text : base.Text);
            }

            set
            {
                if (_InCellCallBack == true)
                    _LabelX.Text = value;
                else
                    base.Text = value;
            }
        }

        #endregion

        #region TextAlignment

        /// <summary>
        /// Gets or sets the horizontal text alignment.
        /// </summary>
        [Browsable(true), DefaultValue(StringAlignment.Near), DevCoBrowsable(true), Category("Layout")]
        [Description("Indicates the horizontal text alignment.")]
        public StringAlignment TextAlignment
        {
            get { return (_LabelX.TextAlignment); }
            set { _LabelX.TextAlignment = value; }
        }

        #endregion

        #region TextLineAlignment

        /// <summary>
        /// Gets or sets the vertical text alignment.
        /// </summary>
        [Browsable(true), DefaultValue(StringAlignment.Center), DevCoBrowsable(true), Category("Layout")]
        [Description("Indicates vertical text line alignment.")]
        public StringAlignment TextLineAlignment
        {
            get { return (_LabelX.TextLineAlignment); }
            set { _LabelX.TextLineAlignment = value; }
        }

        #endregion

        #region UseMnemonic

        /// <summary>
        /// Gets or sets a value indicating whether the control interprets an
        /// ampersand character (&) in the control's Text property to be an
        /// access key prefix character.
        /// </summary>
        [Browsable(true), DefaultValue(true), Category("Appearance")]
        [Description("Indicates whether the control interprets an ampersand character (&) in the control's Text property to be an access key prefix character.")]
        public bool UseMnemonic
        {
            get { return (_LabelX.UseMnemonic); }
            set { _LabelX.UseMnemonic = value; }
        }

        #endregion

        #region WordWrap

        /// <summary>
        /// Gets or sets a value that determines whether text is displayed in multiple lines or one long line.
        /// </summary>
        [Browsable(true), Category("Style"), DefaultValue(false)]
        [Description("Indicates whether text is displayed in multiple lines or one long line.")]
        public bool WordWrap
        {
            get { return (_LabelX.WordWrap); }
            set { _LabelX.WordWrap = value; }
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
                _LabelX.LabelItem.Click += LabelItemClick;
            }
            else
            {
                _LabelX.LabelItem.Click -= LabelItemClick;
            }
        }

        #endregion

        #region Event processing

        #region LabelItem_Click

        /// <summary>
        /// LabelItem_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void LabelItemClick(object sender, EventArgs e)
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
        /// Clones the LabelX Column
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            DataGridViewLabelXColumn bc = base.Clone() as DataGridViewLabelXColumn;

            if (bc != null)
            {
                _LabelX.LabelItem.InternalCopyToItem(bc.LabelX.LabelItem);

                bc.FocusCuesEnabled = FocusCuesEnabled;
                bc.Text = Text;
                bc.UseMnemonic = UseMnemonic;
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
}
