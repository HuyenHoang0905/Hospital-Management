using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Controls
{
    [ToolboxBitmap(typeof(DataGridViewButtonXColumn), "Controls.ComboBoxEx.ico"), ToolboxItem(false), ComVisible(false)]
    public class DataGridViewComboBoxExColumn : DataGridViewTextBoxColumn, IDataGridViewColumn
    {
        #region Events

        /// <summary>
        /// Occurs right before a ComboBox Cell is painted
        /// </summary>
        [Description("Occurs right before a ComboBox Cell is painted.")]
        public event EventHandler<BeforeCellPaintEventArgs> BeforeCellPaint;

        /// <summary>
        /// Occurs when the ComboBox is clicked
        /// </summary>
        [Description("Occurs when the ComboBox is clicked.")]
        public event EventHandler<EventArgs> Click;

        /// <summary>
        /// Occurs when a visual aspect of an owner-drawn ComboBox  changes
        /// </summary>
        [Description("Occurs when a visual aspect of an owner-drawn ComboBox  changes.")]
        public event EventHandler<DrawItemEventArgs> DrawItem;

        /// <summary>
        /// Occurs when drop down portion of the ComboBox is shown or hidden.
        /// </summary>
        [Description("Occurs when drop down portion of the ComboBox is shown or hidden.")]
        public event EventHandler<DropDownChangeEventArgs> DropDownChange;

        /// <summary>
        /// Occurs each time an owner-drawn ComboBox item needs
        /// to be drawn and when the sizes of the list items are determined
        /// </summary>
        [Description("Occurs each time an owner-drawn ComboBox item needs to be drawn and when the sizes of the list items are determined.")]
        public event EventHandler<MeasureItemEventArgs> MeasureItem;

        #endregion

        #region Private variables

        private ComboBoxEx _ComboBoxEx;
        private Bitmap _CellBitmap;
        private CurrencyManager _CurrencyManager;
        private bool _DisplayControlForCurrentCellOnly = true;

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public DataGridViewComboBoxExColumn()
        {
            CellTemplate = new DataGridViewComboBoxExCell();

            _ComboBoxEx = new ComboBoxEx();

            _ComboBoxEx.RenderBorder = false;
            _ComboBoxEx.Visible = false;

            HookEvents(true);
        }

        #region Internal properties

        #region ComboBoxEx

        /// <summary>
        /// Gets the underlying ComboBoxEx control
        /// </summary>
        internal ComboBoxEx ComboBoxEx
        {
            get { return (_ComboBoxEx); }
        }

        #endregion

        #region CurrencyManager

        /// <summary>
        /// Gets or sets the ComboBox CurrencyManager
        /// </summary>
        internal CurrencyManager CurrencyManager
        {
            get { return (_CurrencyManager); }
            set { _CurrencyManager = value; }
        }

        #endregion

        #endregion

        #region Public properties

        #region AutoCompleteCustomSource

        /// <summary>
        /// Gets or sets a custom System.Collections.Specialized.StringCollection
        /// to use when the AutoCompleteSource  property is set to CustomSource.
        /// </summary>
        [Browsable(true), DefaultValue(null)]
        [Description("Gets or sets a custom Specialized.StringCollection to use when the AutoCompleteSource property is set to CustomSource.")]
        public AutoCompleteStringCollection AutoCompleteCustomSource
        {
            get { return (_ComboBoxEx.AutoCompleteCustomSource); }
            set { _ComboBoxEx.AutoCompleteCustomSource = value; }
        }

        #endregion

        #region AutoCompleteMode

        /// <summary>
        /// Gets or sets an option that controls how automatic completion works for the ComboBox.
        /// </summary>
        [Browsable(true), DefaultValue(AutoCompleteMode.None)]
        [Description("Indicates how automatic completion works for the ComboBox.")]
        public AutoCompleteMode AutoCompleteMode
        {
            get { return (_ComboBoxEx.AutoCompleteMode); }
            set { _ComboBoxEx.AutoCompleteMode = value; }
        }

        #endregion

        #region AutoCompleteSource

        /// <summary>
        /// Gets or sets a value specifying the source of complete strings used for automatic completion.
        /// </summary>
        [Browsable(true), DefaultValue(AutoCompleteSource.None)]
        [Description("Indicates the source of complete strings used for automatic completion.")]
        public AutoCompleteSource AutoCompleteSource
        {
            get { return (_ComboBoxEx.AutoCompleteSource); }
            set { _ComboBoxEx.AutoCompleteSource = value; }
        }

        #endregion

        #region DataSource

        /// <summary>
        /// Gets or sets the data source that populates the selections for the combo box
        /// </summary>
        [Browsable(true), Category("Data"), DefaultValue((string)null)]
        [AttributeProvider(typeof(IListSource))]
        [Description("Indicates the data source that populates the selections for the combo box.")]
        public object DataSource
        {
            get { return (_ComboBoxEx.DataSource); }
            set { _ComboBoxEx.DataSource = value; }
        }

        #endregion

        #region DisplayMember

        /// <summary>
        /// Gets or sets a string that specifies the property or column
        /// from which to retrieve strings for display in the combo box.
        /// </summary>
        [Browsable(true), DefaultValue(""), Category("Data")]
        [Description("Indicates a string that specifies the property or column from which to retrieve strings for display in the combo box")]
        [Editor("System.Windows.Forms.Design.DataMemberFieldEditor, System.Design, Version=9.5.0.16, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        [TypeConverter("System.Windows.Forms.Design.DataMemberFieldConverter, System.Design, Version=9.5.0.16, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
        public string DisplayMember
        {
            get { return (_ComboBoxEx.DisplayMember); }
            set { _ComboBoxEx.DisplayMember = value; }
        }

        #endregion

        #region DisplayControlForCurrentCellOnly

        /// <summary>
        /// Gets or sets whether the control
        /// will be displayed for the current cell only.
        /// </summary>
        [Browsable(true), DefaultValue(true), Category("Appearance")]
        [Description("Indicates whether the control will be displayed for the current cell only.")]
        public bool DisplayControlForCurrentCellOnly
        {
            get { return (_DisplayControlForCurrentCellOnly); }

            set
            {
                if (_DisplayControlForCurrentCellOnly != value)
                {
                    _DisplayControlForCurrentCellOnly = value;
                    _ComboBoxEx.Invalidate();
                }
            }
        }

        #endregion

        #region DrawMode

        /// <summary>
        /// Gets or sets a value indicating whether user code or 
        /// operating system code will handle drawing of elements in the list.
        /// </summary>
        [Browsable(true), DefaultValue(DrawMode.Normal)]
        [Description("Indicates whether user code or operating system code will handle drawing of elements in the list.")]
        public DrawMode DrawMode
        {
            get { return (_ComboBoxEx.DrawMode); }
            set { _ComboBoxEx.DrawMode = value; }
        }

        #endregion

        #region DropDownStyle

        /// <summary>
        /// Gets or sets a value specifying the style of the combo box.
        /// </summary>
        [Browsable(true), DefaultValue(ComboBoxStyle.DropDown)]
        [Description("Indicates the style of the combo box.")]
        public ComboBoxStyle DropDownStyle
        {
            get { return (_ComboBoxEx.DropDownStyle); }
            set { _ComboBoxEx.DropDownStyle = value; }
        }

        #endregion

        #region DropDownHeight

        /// <summary>
        /// Gets or sets the height in pixels of the drop-down portion of the ComboBox.
        /// </summary>
        [Browsable(true), DefaultValue(100)]
        [Description("Indicates the height in pixels of the drop-down portion of the ComboBox.")]
        public int DropDownHeight
        {
            get { return (_ComboBoxEx.DropDownHeight); }
            set { _ComboBoxEx.DropDownHeight = value; }
        }

        #endregion

        #region DropDownWidth

        /// <summary>
        /// Gets or sets the width of the drop-down lists of the combo box.
        /// </summary>
        [Browsable(true), Category("Behavior"), DefaultValue(1)]
        [Description("Indicates the width of the drop-down lists of the combo box")]
        public int DropDownWidth
        {
            get { return (_ComboBoxEx.DropDownWidth); }
            set { _ComboBoxEx.DropDownWidth = value; }
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
            get { return (_ComboBoxEx.Enabled); }
            set { _ComboBoxEx.Enabled = value; }
        }

        #endregion

        #region FlatStyle

        /// <summary>
        /// Gets or sets the flat style appearance of the column's cells.
        /// </summary>
        [Browsable(true), Category("Appearance"), DefaultValue(FlatStyle.Standard)]
        [Description("Indicates the flat style appearance of the column's cells")]
        public FlatStyle FlatStyle
        {
            get { return (_ComboBoxEx.FlatStyle); }
            set { _ComboBoxEx.FlatStyle = value; }
        }

        #endregion

        #region FocusCuesEnabled

        /// <summary>
        /// Gets or sets whether control displays focus cues when focused.
        /// </summary>
        [DefaultValue(true), Category("Behavior")]
        [Description("Indicates whether control displays focus cues when focused.")]
        public bool FocusCuesEnabled
        {
            get { return (_ComboBoxEx.FocusCuesEnabled); }
            set { _ComboBoxEx.FocusCuesEnabled = value; }
        }

        #endregion

        #region FormatString

        /// <summary>
        /// Gets or sets the format-specifier
        /// characters that indicate how a value is to be displayed.
        /// </summary>
        [Browsable(true), DefaultValue("")]
        [Description("Indicates the format-specifier characters that indicate how a value is to be displayed.")]
        public string FormatString
        {
            get { return (_ComboBoxEx.FormatString); }
            set { _ComboBoxEx.FormatString = value; }
        }

        #endregion

        #region FormattingEnabled

        /// <summary>
        /// Gets or sets a value indicating whether formatting is applied to the DisplayMember property.
        /// </summary>
        [Browsable(true), DefaultValue(false)]
        [Description("Indicates whether formatting is applied to the DisplayMember.")]
        public bool FormattingEnabled
        {
            get { return (_ComboBoxEx.FormattingEnabled); }
            set { _ComboBoxEx.FormattingEnabled = value; }
        }

        #endregion

        #region Images

        /// <summary>
        /// Gets or sets the ImageList control used by Combo box to draw images.
        /// </summary>
        [Browsable(true), Category("Behavior"), DefaultValue(null)]
        [Description("The ImageList control used by Combo box to draw images.")]
        public ImageList Images
        {
            get { return (_ComboBoxEx.Images); }
            set { _ComboBoxEx.Images = value; }
        }

        #endregion

        #region ImeMode

        /// <summary>
        /// Gets or sets the Input Method Editor (IME) mode of the control.
        /// </summary>
        [Browsable(true), DefaultValue(ImeMode.Inherit)]
        [Description("Indicates the Input Method Editor (IME) mode of the control.")]
        public ImeMode ImeMode
        {
            get { return (_ComboBoxEx.ImeMode); }
            set { _ComboBoxEx.ImeMode = value; }
        }

        #endregion

        #region IntegralHeight

        /// <summary>
        /// Gets or sets a value indicating whether
        /// the control should resize to avoid showing partial items.
        /// </summary>
        [Browsable(true), DefaultValue(true)]
        [Description("Indicates whether the control should resize to avoid showing partial items.")]
        public bool IntegralHeight
        {
            get { return (_ComboBoxEx.IntegralHeight); }
            set { _ComboBoxEx.IntegralHeight = value; }
        }

        #endregion

        #region IsStandalone

        /// <summary>
        /// Gets or sets whether control is stand-alone control.
        /// Stand-alone flag affects the appearance of the control in Office 2007 style.
        /// </summary>
        [Browsable(true), DefaultValue(true), Category("Appearance")]
        [Description("Indicates the appearance of the control.")]
        public bool IsStandalone
        {
            get { return (_ComboBoxEx.IsStandalone); }
            set { _ComboBoxEx.IsStandalone = value; }
        }

        #endregion

        #region ItemHeight

        /// <summary>
        /// Gets or sets the height of an item in the combo box.
        /// </summary>
        [Browsable(true), DefaultValue(12)]
        [Description("Indicates the height of an item in the combo box.")]
        public int ItemHeight
        {
            get { return (_ComboBoxEx.ItemHeight); }
            set { _ComboBoxEx.ItemHeight = value; }
        }

        #endregion

        #region Items

        /// <summary>
        /// Gets the collection of objects used as selections in the combo box.
        /// </summary>
        [Browsable(true), Category("Data")]
        [Description("Indicates the collection of objects used as selections in the combo box")]
        [Editor("System.Windows.Forms.Design.StringCollectionEditor, System.Design, Version=9.5.0.16, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ComboBox.ObjectCollection Items
        {
            get { return (_ComboBoxEx.Items); }
        }

        #endregion

        #region MaxDropDownItems

        /// <summary>
        /// Gets or sets the maximum number of items
        /// in the drop-down list of the cells in the column.
        /// </summary>
        [Browsable(true), DefaultValue(8), Category("Behavior")]
        [Description("Indicates the maximum number of items in the drop-down list of the cells in the column.")]
        public int MaxDropDownItems
        {
            get { return (_ComboBoxEx.MaxDropDownItems); }
            set { _ComboBoxEx.MaxDropDownItems = value; }
        }

        #endregion

        #region MaxLength

        /// <summary>
        /// Gets or sets the number of characters a user can type into the ComboBox.
        /// </summary>
        [Browsable(true), DefaultValue(0)]
        [Description("Indicates the number of characters a user can type into the ComboBox.")]
        public int MaxLength
        {
            get { return (_ComboBoxEx.MaxLength); }
            set { _ComboBoxEx.MaxLength = value; }
        }

        #endregion

        #region RightToLeft

        /// <summary>
        /// Gets or sets a value indicating whether control's
        /// elements are aligned to support locales using right-to-left fonts.
        /// </summary>
        [Browsable(true), DefaultValue(RightToLeft.Inherit)]
        [Description("Indicates control's elements are aligned to support locales using right-to-left fonts.")]
        public RightToLeft RightToLeft
        {
            get { return (_ComboBoxEx.RightToLeft); }
            set { _ComboBoxEx.RightToLeft = value; }
        }

        #endregion

        #region Sorted

        /// <summary>
        /// Gets or sets whether the items in the combo box are sorted.
        /// </summary>
        [Browsable(true), DefaultValue(false), Category("Behavior")]
        [Description("Indicates whether the items in the combo box are sorted.")]
        public bool Sorted
        {
            get { return (_ComboBoxEx.Sorted); }
            set { _ComboBoxEx.Sorted = value; }
        }

        #endregion

        #region Style

        /// <summary>
        /// Determines the visual style applied to
        /// the combo box when shown. Default style is Office 2007.
        /// </summary>
        [Browsable(true), Category("Appearance"), DefaultValue(eDotNetBarStyle.Office2007)]
        [Description("Determines the display of the item when shown.")]
        public eDotNetBarStyle Style
        {
            get { return (_ComboBoxEx.Style); }
            set { _ComboBoxEx.Style = value; }
        }

        #endregion

        #region Text

        /// <summary>
        /// Gets or sets the text associated with this control.
        /// </summary>
        [Browsable(true), DefaultValue("")]
        [Description("Indicates the text associated with this control.")]
        public string Text
        {
            get { return (_ComboBoxEx.Text); }
            set { _ComboBoxEx.Text = value; }
        }

        #endregion

        #region ValueMember

        /// <summary>
        /// Gets or sets a string that specifies the property or column
        /// from which to get values that correspond to the selections in the drop-down list.
        /// </summary>
        [Browsable(true), Category("Data"), DefaultValue("")]
        [Description("Indicates a string that specifies the property or column from which to get values that correspond to the selections in the drop-down list.")]
        [Editor("System.Windows.Forms.Design.DataMemberFieldEditor, System.Design, Version=9.5.0.16, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        [TypeConverter("System.Windows.Forms.Design.DataMemberFieldConverter, System.Design, Version=9.5.0.16, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
        public string ValueMember
        {
            get { return (_ComboBoxEx.ValueMember); }
            set { _ComboBoxEx.ValueMember = value; }
        }

        #endregion

        #region WatermarkBehavior

        /// <summary>
        /// Gets or sets the watermark hiding behaviour. Default value
        /// indicates that watermark is hidden when control receives input focus.
        /// </summary>
        [DefaultValue(eWatermarkBehavior.HideOnFocus), Category("Behavior")]
        [Description("Indicates watermark hiding behaviour.")]
        public eWatermarkBehavior WatermarkBehavior
        {
            get { return (_ComboBoxEx.WatermarkBehavior); }
            set { _ComboBoxEx.WatermarkBehavior = value; }
        }

        #endregion

        #region WatermarkColor

        /// <summary>
        /// Gets or sets the watermark text color.
        /// </summary>
        [Browsable(true), Category("Appearance")]
        [Description("Indicates watermark text color.")]
        public Color WatermarkColor
        {
            get { return (_ComboBoxEx.WatermarkColor); }
            set { _ComboBoxEx.WatermarkColor = value; }
        }
        /// <summary>
        /// Indicates whether property should be serialized by Windows Forms designer.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeWatermarkColor()
        {
            return (_ComboBoxEx.ShouldSerializeWatermarkColor());
        }
        /// <summary>
        /// Resets the property to default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetWatermarkColor()
        {
            _ComboBoxEx.ResetWatermarkColor();
        }

        #endregion

        #region WatermarkEnabled

        /// <summary>
        /// Gets or sets whether watermark text is
        /// displayed when control is empty. Default value is true.
        /// </summary>
        [Browsable(true), DefaultValue(true)]
        [Description("Indicates whether watermark text is displayed when control is empty.")]
        public virtual bool WatermarkEnabled
        {
            get { return (_ComboBoxEx.WatermarkEnabled); }
            set { _ComboBoxEx.WatermarkEnabled = value; }
        }

        #endregion

        #region WatermarkFont

        /// <summary>
        /// Gets or sets the watermark font.
        /// </summary>
        [Browsable(true), Category("Appearance"), DefaultValue(null)]
        [Description("Indicates watermark font.")]
        public Font WatermarkFont
        {
            get { return (_ComboBoxEx.WatermarkFont); }
            set { _ComboBoxEx.WatermarkFont = value; }
        }

        #endregion

        #region WatermarkText

        /// <summary>
        /// Gets or sets the watermark (tip) text displayed inside of the control when Text
        /// is not set and control does not have input focus. This property supports text-markup.
        /// Note that WatermarkText is not compatible with the auto-complete feature of .NET Framework 2.0.
        /// </summary>
        [Browsable(true), DefaultValue(""), Localizable(true), Category("Appearance")]
        [Description("Indicates watermark text displayed inside of the control when Text is not set and control does not have input focus.")]
        [Editor("DevComponents.DotNetBar.Design.TextMarkupUIEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
        public string WatermarkText
        {
            get { return (_ComboBoxEx.WatermarkText); }
            set { _ComboBoxEx.WatermarkText = value; }
        }

        #endregion

        #endregion

        #region HookEvents

        /// <summary>
        /// HookEvents
        /// </summary>
        /// <param name="hook"></param>
        private void HookEvents(bool hook)
        {
            if (hook == true)
                _ComboBoxEx.DataSourceChanged += ComboBoxEx_DataSourceChanged;
            else
                _ComboBoxEx.DataSourceChanged -= ComboBoxEx_DataSourceChanged;
        }

        #endregion

        #region Event processing

        #region ComboBoxEx_DataSourceChanged

        /// <summary>
        /// ComboBoxEx_DataSourceChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ComboBoxEx_DataSourceChanged(object sender, EventArgs e)
        {
            _CurrencyManager = null;

            _ComboBoxEx.Items.Clear();
            _ComboBoxEx.DisplayMember = null;
            _ComboBoxEx.ValueMember = null;
        }

        #endregion

        #region DoComboBoxEx_Click

        /// <summary>
        /// DoComboBoxEx_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal void DoComboBoxEx_Click(object sender, EventArgs e)
        {
            if (Click != null)
                Click(sender, e);
        }

        #endregion

        #region DoComboBoxEx_DrawItem

        /// <summary>
        /// DoComboBoxEx_DrawItem
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal void DoComboBoxEx_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (DrawItem != null)
                DrawItem(sender, e);
        }

        #endregion

        #region DoComboBoxEx_DropDownChange

        /// <summary>
        /// DoComboBoxEx_DropDownChange
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal void DoComboBoxEx_DropDownChange(object sender, DropDownChangeEventArgs e)
        {
            if (DropDownChange != null)
                DropDownChange(sender, e);
        }

        #endregion

        #region DoComboBoxEx_MeasureItem

        /// <summary>
        /// DoComboBoxEx_MeasureItem
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal void DoComboBoxEx_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            if (MeasureItem != null)
                MeasureItem(sender, e);
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

        #region ToString

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("DataGridViewComboBoxExColumn { Name=");
            builder.Append(Name);
            builder.Append(", Index=");
            builder.Append(Index.ToString(CultureInfo.CurrentCulture));
            builder.Append(" }");

            return (builder.ToString());
        }

        #endregion

        #region ICloneable members

        /// <summary>
        /// Clones the ButtonX Column
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            DataGridViewComboBoxExColumn cb = base.Clone() as DataGridViewComboBoxExColumn;

            if (cb != null)
            {
                cb.AutoCompleteCustomSource = AutoCompleteCustomSource;
                cb.AutoCompleteMode = AutoCompleteMode;
                cb.AutoCompleteSource = AutoCompleteSource;
                cb.DrawMode = DrawMode;
                cb.DropDownHeight = DropDownHeight;
                cb.DropDownStyle = DropDownStyle;
                cb.Enabled = Enabled;
                cb.FlatStyle = FlatStyle;
                cb.FocusCuesEnabled = FocusCuesEnabled;
                cb.FormatString = FormatString;
                cb.FormattingEnabled = FormattingEnabled;
                cb.Images = Images;
                cb.ImeMode = ImeMode;
                cb.IntegralHeight = IntegralHeight;
                cb.IsStandalone = IsStandalone;
                cb.ItemHeight = ItemHeight;
                cb.MaxDropDownItems = MaxDropDownItems;
                cb.MaxLength = MaxLength;
                cb.RightToLeft = RightToLeft;
                cb.Style = Style;
                cb.Text = Text;
                cb.WatermarkBehavior = WatermarkBehavior;
                cb.WatermarkColor = WatermarkColor;
                cb.WatermarkEnabled = WatermarkEnabled;
                cb.WatermarkFont = WatermarkFont;
                cb.WatermarkText = WatermarkText;

                cb.DataSource = DataSource;
                cb.DisplayMember = DisplayMember;
                cb.ValueMember = ValueMember;

                if (cb.DataSource == null)
                {
                    for (int i = 0; i < Items.Count; i++)
                        cb.Items.Add(Items[i]);
                }

                cb.DisplayControlForCurrentCellOnly = DisplayControlForCurrentCellOnly;
            }

            return (cb);
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

        #region IDataGridViewColumn Members

        /// <summary>
        /// Gets the Cell paint setting for the control
        /// </summary>
        [Browsable(false)]
        public bool OwnerPaintCell
        {
            get { return (true); }
        }

        #endregion
    }

    #region DropDownChangeEventArgs

    /// <summary>
    /// DropDownChangeEventArgs
    /// </summary>
    public class DropDownChangeEventArgs : EventArgs
    {
        #region Private variables

        private int _RowIndex;
        private int _ColumnIndex;
        private bool _Expanded;

        #endregion

        public DropDownChangeEventArgs(int rowIndex, int columnIndex, bool expanded)
        {
            _RowIndex = rowIndex;
            _ColumnIndex = columnIndex;
            _Expanded = expanded;
        }

        #region Public properties

        /// <summary>
        /// ColumnIndex
        /// </summary>
        public int ColumnIndex
        {
            get { return (_ColumnIndex); }
        }

        /// <summary>
        /// Expanded state
        /// </summary>
        public bool Expanded
        {
            get { return (_Expanded); }
        }

        /// <summary>
        /// RowIndex
        /// </summary>
        public int RowIndex
        {
            get { return (_RowIndex); }
        }

        #endregion
    }

    #endregion
}
