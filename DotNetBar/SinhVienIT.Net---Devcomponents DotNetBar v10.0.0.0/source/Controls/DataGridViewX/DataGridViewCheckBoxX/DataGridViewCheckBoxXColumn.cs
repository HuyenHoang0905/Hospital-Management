using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Controls
{
    [ToolboxBitmap(typeof(DataGridViewButtonXColumn), "Controls.CheckBoxX.ico"), ToolboxItem(false), ComVisible(false)]
    public class DataGridViewCheckBoxXColumn : DataGridViewColumn, IDataGridViewColumn
    {
        #region Events

        [Description("Occurs right before a CheckBoxX Cell is painted.")]
        public event EventHandler<BeforeCellPaintEventArgs> BeforeCellPaint;

        [Description("Occurs when a CheckBoxX Cell is Clicked.")]
        public event EventHandler<EventArgs> Click;

        [Description("Occurs when a CheckBoxX Cell is DoubleClicked.")]
        public event EventHandler<EventArgs> DoubleClicked;

        #endregion

        #region Private variables

        private CheckBoxX _CheckBoxX;

        private int _ActiveRowIndex = -1;
        private int _CurrentRowIndex = -1;
        private int _DownRowIndex = -1;

        private bool _InCellCallBack;

        private Bitmap _CellBitmap;

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public DataGridViewCheckBoxXColumn()
        {
            CellTemplate = new DataGridViewCheckBoxXCell();

            _CheckBoxX = new CheckBoxX();
            _CheckBoxX.Visible = false;

            _CheckBoxX.CheckValueChecked = true;
            _CheckBoxX.CheckValueUnchecked = false;

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

        #region CheckBoxX

        /// <summary>
        /// Gets the Control Button
        /// </summary>
        internal CheckBoxX CheckBoxX
        {
            get { return (_CheckBoxX); }
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

        #region DownRowIndex

        /// <summary>
        /// Gets or sets the MouseDown row index
        /// </summary>
        internal int DownRowIndex
        {
            get { return (_DownRowIndex); }
            set { _DownRowIndex = value; }
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

        #region CheckBoxImageChecked
        /// <summary>
        /// Gets or sets the custom image that is displayed instead default check box representation when check box is checked.
        /// </summary>
        [DefaultValue(null), Category("CheckBox Images"), Description("Indicates custom image that is displayed instead default check box representation when check box is checked")]
        public Image CheckBoxImageChecked
        {
            get { return _CheckBoxX.CheckBoxImageChecked; }
            set
            {
                _CheckBoxX.CheckBoxImageChecked = value;
            }
        }
        #endregion
        #region CheckBoxImageUnChecked
        /// <summary>
        /// Gets or sets the custom image that is displayed instead default check box representation when check box is unchecked.
        /// </summary>
        [DefaultValue(null), Category("CheckBox Images"), Description("Indicates custom image that is displayed instead default check box representation when check box is unchecked")]
        public Image CheckBoxImageUnChecked
        {
            get { return _CheckBoxX.CheckBoxImageUnChecked; }
            set
            {
                _CheckBoxX.CheckBoxImageUnChecked = value;
            }
        }
        #endregion
        #region CheckBoxImageIndeterminate
        /// <summary>
        /// Gets or sets the custom image that is displayed instead default check box representation when check box is in indeterminate state.
        /// </summary>
        [DefaultValue(null), Category("CheckBox Images"), Description("Indicates custom image that is displayed instead default check box representation when check box is in indeterminate state")]
        public Image CheckBoxImageIndeterminate
        {
            get { return _CheckBoxX.CheckBoxImageIndeterminate; }
            set
            {
                _CheckBoxX.CheckBoxImageIndeterminate = value;
            }
        }
        #endregion

        #region CheckBoxPosition

        /// <summary>
        /// Gets or sets the check box position relative to the text.
        /// Default value is Left.
        /// </summary>
        [Browsable(true), DefaultValue(eCheckBoxPosition.Left), Category("Appearance")]
        [Description("Indicates the check box position relative to the text.")]
        public eCheckBoxPosition CheckBoxPosition
        {
            get { return (_CheckBoxX.CheckBoxPosition); }
            set { _CheckBoxX.CheckBoxPosition = value; }
        }

        #endregion

        #region Checked

        /// <summary>
        /// Gets or set a value indicating whether the button is in the checked state.
        /// </summary>
        [Browsable(false)]
        public virtual bool Checked
        {
            get { return (_CheckBoxX.Checked); }
            set { _CheckBoxX.Checked = value; }
        }

        #endregion

        #region CheckState

        /// <summary>
        /// Specifies the state of the control, that can be
        /// checked, unchecked, or set to an indeterminate state. 
        /// </summary>
        [Browsable(false)]
        public CheckState CheckState
        {
            get { return (_CheckBoxX.CheckState); }
            set { _CheckBoxX.CheckState = value; }
        }

        #endregion

        #region CheckValue

        /// <summary>
        /// CheckValue
        /// </summary>
        [Browsable(false)]
        public object CheckValue
        {
            get { return (_CheckBoxX.CheckValue); }
            set { _CheckBoxX.CheckValue = value; }
        }

        #endregion

        #region CheckValueChecked

        /// <summary>
        /// Gets or sets the value that represents the Checked state value of the check
        /// box when CheckValue property is set to that value. Default value is null.
        /// </summary>
        [Browsable(true), DefaultValue(true), Category("Behavior"), TypeConverter(typeof(StringConverter))]
        [Description("Represents the Checked state value of the check box when CheckValue property is set to that value. Note that in the designer, this value type is always a string.")]
        public object CheckValueChecked
        {
            get { return (_CheckBoxX.CheckValueChecked); }
            set { _CheckBoxX.CheckValueChecked = value; }
        }

        #endregion

        #region CheckValueIndeterminate

        /// <summary>
        /// Gets or sets the value that represents the Indeterminate state of the check
        /// box when CheckValue property is set to that value. Default value is null.
        /// </summary>
        [Browsable(true), DefaultValue(null), Category("Behavior"), TypeConverter(typeof(StringConverter))]
        [Description("Represents the Indeterminate state value of the check box when CheckValue property is set to that value. Note that in the designer, this value type is always a string.")]
        public object CheckValueIndeterminate
        {
            get { return (_CheckBoxX.CheckValueIndeterminate); }
            set { _CheckBoxX.CheckValueIndeterminate = value; }
        }

        #endregion

        #region CheckValueUnchecked

        /// <summary>
        /// Gets or sets the value that represents the Unchecked state value of check
        /// box when CheckValue property is set to that value. Default value is 'N'.
        /// </summary>
        [Browsable(true), DefaultValue(false), Category("Behavior"), TypeConverter(typeof(StringConverter))]
        [Description("Represents the Unchecked state value of the check box when CheckValue property is set to that value.  Note that in the designer, this value type is always a string.")]
        public object CheckValueUnchecked
        {
            get { return (_CheckBoxX.CheckValueUnchecked); }
            set { _CheckBoxX.CheckValueUnchecked = value; }
        }

        #endregion

        #region ConsiderEmptyStringAsNull

        /// <summary>
        /// Gets or sets whether empty string is consider as null value
        /// during CheckValue value comparison. Default value is true.
        /// </summary>
        [DefaultValue(true), Category("Behavior"), Browsable(true)]
        [Description("Indicates whether empty string is consider as null value during CheckValue value comparison.")]
        public bool ConsiderEmptyStringAsNull
        {
            get { return (_CheckBoxX.ConsiderEmptyStringAsNull); }
            set { _CheckBoxX.ConsiderEmptyStringAsNull = value; }
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
            get { return (_CheckBoxX.Enabled); }
            set { _CheckBoxX.Enabled = value; }
        }

        #endregion

        #region EnableMarkup

        /// <summary>
        /// Gets or sets whether text-markup support is enabled for controls
        /// Text property. Default value is true. Set this property to false to
        /// display HTML or other markup in the control instead of it being parsed as text-markup.
        /// </summary>
        [Browsable(true), DefaultValue(true), Category("Appearance")]
        [Description("Indicates whether text-markup support is enabled for controls Text property.")]
        public bool EnableMarkup
        {
            get { return (_CheckBoxX.EnableMarkup); }
            set { _CheckBoxX.EnableMarkup = value; }
        }

        #endregion

        #region Text

        /// <summary>
        /// Gets or sets the default Text to display
        /// </summary>
        [Browsable(true), Category("Appearance"), DefaultValue("")]
        [Description("Indicates the default Text to display.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string Text
        {
            get { return (_CheckBoxX.Text); }
            set { _CheckBoxX.Text = value; }
        }

        #endregion

        #region TextColor

        /// <summary>
        /// Gets or sets the text color. Default value is
        /// Color.Empty which indicates that default color is used.
        /// </summary>
        [Browsable(true), Category("Appearance")]
        [Description("Indicates control text color.")]
        public Color TextColor
        {
            get { return (_CheckBoxX.TextColor); }
            set { _CheckBoxX.TextColor = value; }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeTextColor()
        {
            return (_CheckBoxX.TextColor.IsEmpty == false);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetTextColor()
        {
            TextColor = Color.Empty;
        }

        #endregion

        #region TextVisible

        /// <summary>
        /// Gets or sets whether text assigned to the check box is visible.
        /// Default value is true.
        /// </summary>
        [Browsable(true), DefaultValue(true), Category("Appearance")]
        [Description("Indicates whether text assigned to the check box is visible.")]
        public bool TextVisible
        {
            get { return (_CheckBoxX.TextVisible); }
            set { _CheckBoxX.TextVisible = value; }
        }

        #endregion

        #region ThreeState

        /// <summary>
        /// Gets or sets a value indicating whether the CheckBox will allow
        /// three check states rather than two. If the ThreeState property is
        /// set to true  CheckState property should be used instead of Checked
        /// property to set the extended state of the control.
        /// </summary>
        [Browsable(true), Category("Behavior"), DefaultValue(false)]
        [Description("Indicates whether the CheckBox will allow three check states rather than two.")]
        public bool ThreeState
        {
            get { return (_CheckBoxX.ThreeState); }
            set { _CheckBoxX.ThreeState = value; }
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
            {
                _CheckBoxX.CheckBoxItem.Click += CheckBoxItem_Click;
                _CheckBoxX.CheckBoxItem.DoubleClick += CheckBoxItem_DoubleClick;
            }
            else
            {
                _CheckBoxX.CheckBoxItem.Click -= CheckBoxItem_Click;
                _CheckBoxX.CheckBoxItem.DoubleClick -= CheckBoxItem_DoubleClick;
            }
        }

        #endregion

        #region Event processing

        #region CheckBoxItem_Click

        /// <summary>
        /// CheckBoxItem_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CheckBoxItem_Click(object sender, EventArgs e)
        {
            if (Click != null)
                Click(sender, e);
        }

        #endregion

        #region CheckBoxItem_DoubleClick

        /// <summary>
        /// CheckBoxItem_DoubleClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CheckBoxItem_DoubleClick(object sender, EventArgs e)
        {
            if (DoubleClicked != null)
                DoubleClicked(sender, e);
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
            DataGridViewCheckBoxXColumn cb = base.Clone() as DataGridViewCheckBoxXColumn;

            if (cb != null)
            {
                _CheckBoxX.CheckBoxItem.InternalCopyToItem(cb.CheckBoxX.CheckBoxItem);

                cb.CheckValue = CheckValue;
                cb.CheckValueChecked = CheckValueChecked;
                cb.CheckValueIndeterminate = CheckValueIndeterminate;
                cb.CheckValueUnchecked = CheckValueUnchecked;
                cb.ConsiderEmptyStringAsNull = ConsiderEmptyStringAsNull;
                cb.Enabled = Enabled;
                cb.Text = Text;
            }

            return (cb);
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
