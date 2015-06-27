using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using DevComponents.DotNetBar.Rendering;
using DevComponents.Editors;
using DevComponents.Editors.DateTimeAdv;

namespace DevComponents.DotNetBar.Controls
{
    [ToolboxBitmap(typeof(DataGridViewButtonXColumn), "DateTimeInput.ico"), ToolboxItem(false), ComVisible(false)]
    public class DataGridViewDateTimeInputColumn : DataGridViewTextBoxColumn, IDataGridViewColumn
    {
        #region Events

        /// <summary>
        /// Occurs right before a DateTimeInput Cell is painted
        /// </summary>
        [Description("Occurs right before a DateTimeInput Cell is painted.")]
        public event EventHandler<BeforeCellPaintEventArgs> BeforeCellPaint;

        /// <summary>
        /// Occurs when Clear button is clicked and allows you
        /// to cancel the default action performed by the button
        /// </summary>
        [Description("Occurs when Clear button is clicked and allows you to cancel the default action performed by the button.")]
        public event EventHandler<CancelEventArgs> ButtonClearClick;

        /// <summary>
        /// Occurs when ButtonCustom control is clicked
        /// </summary>
        [Description("Occurs when ButtonCustom control is clicked.")]
        public event EventHandler<EventArgs> ButtonCustomClick;

        /// <summary>
        /// Occurs when ButtonCustom2 control is clicked
        /// </summary>
        [Description("Occurs when ButtonCustom2 control is clicked.")]
        public event EventHandler<EventArgs> ButtonCustom2Click;

        /// <summary>
        /// Occurs when Drop-Down button that shows calendar
        /// is clicked and allows you to cancel showing of the popup
        /// </summary>
        [Description("Occurs when Drop-Down button that shows calendar is clicked and allows you to cancel showing of the popup.")]
        public event EventHandler<CancelEventArgs> ButtonDropDownClick;

        /// <summary>
        /// Occurs when Free-Text button is clicked
        /// and allows you to cancel its default action
        /// </summary>
        [Description("Occurs when Free-Text button is clicked and allows you to cancel its default action.")]
        public event EventHandler<CancelEventArgs> ButtonFreeTextClick;

        /// <summary>
        /// Occurs if Free-Text entry value is not natively recognized by
        /// the control and provides you with opportunity to convert that
        /// value to the value the control expects
        /// </summary>
        [Description("Occurs if Free-Text entry value is not natively recognized by the control and provides you with opportunity to convert that value to the value the control expects.")]
        public event EventHandler<FreeTextEntryConversionEventArgs> ConvertFreeTextEntry;

        #endregion

        #region Private variables

        private DateTimeInput _DateTimeInput;
        private Bitmap _CellBitmap;
        private bool _DisplayControlForCurrentCellOnly = true;

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public DataGridViewDateTimeInputColumn()
        {
            CellTemplate = new DataGridViewDateTimeInputCell();

            _DateTimeInput = new DateTimeInput();

            _DateTimeInput.BackgroundStyle.Class = ElementStyleClassKeys.DataGridViewDateTimeBorderKey;
            _DateTimeInput.Visible = false;
        }

        protected override void Dispose(bool disposing)
        {
            if(disposing) _DateTimeInput.Dispose();
            if(_CellBitmap!=null)
            {
                _CellBitmap.Dispose();
                _CellBitmap = null;
            }
            base.Dispose(disposing);
        }

        #region Internal properties

        #region DateTimeInput

        /// <summary>
        /// Gets the underlying DateTimeInput control
        /// </summary>
        internal DateTimeInput DateTimeInput
        {
            get { return (_DateTimeInput); }
        }

        #endregion

        #endregion

        #region Public properties

        #region AllowEmptyState

        /// <summary>
        /// Gets or sets whether empty null/nothing state of the control is
        /// allowed. Default value is true which means that IsEmpty property may
        /// return true if input value is resets or ValueObject set to null/nothing.
        /// </summary>
        [Browsable(true), DefaultValue(true)]
        [Description("Indicates whether empty null/nothing state of the control is allowed.")]
        public bool AllowEmptyState
        {
            get { return (_DateTimeInput.AllowEmptyState); }
            set { _DateTimeInput.AllowEmptyState = value; }
        }

        #endregion

        #region AutoAdvance

        /// <summary>
        /// Gets or sets whether input focus is automatically advanced
        /// to next input field when input is complete in current one.
        /// </summary>
        [Browsable(true), DefaultValue(false), Category("Behavior")]
        [Description("Indicates whether input focus is automatically advanced to next input field when input is complete in current one.")]
        public bool AutoAdvance
        {
            get { return (_DateTimeInput.AutoAdvance); }
            set { _DateTimeInput.AutoAdvance = value; }
        }

        #endregion

        #region AutoSelectDate

        /// <summary>
        /// Gets or sets whether first day in month is automatically
        /// selected on popup date picker when month or year is changed.
        /// </summary>
        [Browsable(true), DefaultValue(false), Category("Behavior")]
        [Description("Indicates whether first day in month is automatically selected on popup date picker when month or year is changed.")]
        public bool AutoSelectDate
        {
            get { return (_DateTimeInput.AutoSelectDate); }
            set { _DateTimeInput.AutoSelectDate = value; }
        }

        #endregion

        #region AutoOverwrite

        /// <summary>
        /// Gets or sets whether auto-overwrite functionality for input is enabled.
        /// When in auto-overwrite mode input field will erase existing entry
        /// and start new one if typing is continued after InputComplete method is called.
        /// </summary>
        [Browsable(true), DefaultValue(true)]
        [Description("Indicates whether auto-overwrite functionality for input is enabled.")]
        public bool AutoOverwrite
        {
            get { return (_DateTimeInput.AutoOverwrite); }
            set { _DateTimeInput.AutoOverwrite = value; }
        }

        #endregion

        #region BackgroundStyle

        /// <summary>
        /// Specifies the background style of the control.
        /// </summary>
        [Browsable(true), Category("Style")]
        [Description("Gets or sets control background style.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ElementStyle BackgroundStyle
        {
            get { return (_DateTimeInput.BackgroundStyle); }
        }

        /// <summary>
        /// Resets style to default value. Used by windows forms designer.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetBackgroundStyle()
        {
            _DateTimeInput.ResetBackgroundStyle();

            _DateTimeInput.BackgroundStyle.Class = "TextBoxBorder";
            _DateTimeInput.BackgroundStyle.CornerType = eCornerType.Square;
        }

        #endregion

        #region ButtonClear

        /// <summary>
        /// Gets the object that describes the settings for the button
        /// that clears the content of the control when clicked.
        /// </summary>
        [Browsable(true), Category("Buttons")]
        [Description("Describes the settings for the button that clears the content of the control when clicked.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public InputButtonSettings ButtonClear
        {
            get { return (_DateTimeInput.ButtonClear); }
        }

        #endregion

        #region ButtonCustom

        /// <summary>
        /// Gets the object that describes the settings for the custom button
        /// that can execute an custom action of your choosing when clicked.
        /// </summary>
        [Browsable(true), Category("Buttons")]
        [Description("Describes the settings for the custom button that can execute an custom action of your choosing when clicked.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public InputButtonSettings ButtonCustom
        {
            get { return (_DateTimeInput.ButtonCustom); }
        }

        #endregion

        #region ButtonCustom2

        /// <summary>
        /// Gets the object that describes the settings for the custom button that can execute an custom action of your choosing when clicked.
        /// </summary>
        [Browsable(true), Category("Buttons")]
        [Description("Describes the settings for the custom button that can execute an custom action of your choosing when clicked.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public InputButtonSettings ButtonCustom2
        {
            get { return (_DateTimeInput.ButtonCustom2); }
        }

        #endregion

        #region ButtonDropDown

        /// <summary>
        /// Gets the object that describes the settings for the button
        /// that shows drop-down when clicked.
        /// </summary>
        [Browsable(true), Category("Buttons")]
        [Description("Describes the settings for the button that shows drop-down when clicked.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public InputButtonSettings ButtonDropDown
        {
            get { return (_DateTimeInput.ButtonDropDown); }
        }

        #endregion

        #region ButtonFreeText

        /// <summary>
        /// Gets the object that describes the settings for the button
        /// that switches the control into the free-text entry mode when clicked.
        /// </summary>
        [Browsable(true), Category("Buttons")]
        [Description("Describes the settings for the button that switches the control into the free-text entry mode when clicked.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public InputButtonSettings ButtonFreeText
        {
            get { return (_DateTimeInput.ButtonFreeText); }
        }

        #endregion

        #region CustomFormat

        /// <summary>
        /// Gets or sets the custom date/time format string. 
        /// </summary>
        [Browsable(true), DefaultValue("")]
        [Description("Indicates the custom date/time format string. "), Localizable(true)]
        public string CustomFormat
        {
            get { return (_DateTimeInput.CustomFormat); }
            set { _DateTimeInput.CustomFormat = value; }
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
                    _DateTimeInput.Invalidate();
                }
            }
        }

        #endregion

        #region DefaultInputValues

        /// <summary>
        /// Gets or sets whether empty input values (year, month or day) are
        /// set to defaults while user is entering data. Default value is true.
        /// </summary>
        [Browsable(true), DefaultValue(true), Category("Behavior")]
        [Description("Indicates whether empty input values (year, month or day) are set to defaults while user is entering data")]
        public bool DefaultInputValues
        {
            get { return (_DateTimeInput.DefaultInputValues); }
            set { _DateTimeInput.DefaultInputValues = value; }
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
            get { return (_DateTimeInput.Enabled); }
            set { _DateTimeInput.Enabled = value; }
        }

        #endregion

        #region Format

        /// <summary>
        /// Gets or sets the format date/time is displayed in. To specify
        /// custom format set this value to Custom and specify custom format
        /// using CustomFormat property.
        /// </summary>
        [Browsable(true), DefaultValue(eDateTimePickerFormat.Short)]
        [Description("Indicates the format date/time is displayed in. To specify custom format set this value to Custom and specify custom format using CustomFormat property.")]
        public eDateTimePickerFormat Format
        {
            get { return (_DateTimeInput.Format); }
            set { _DateTimeInput.Format = value; }
        }

        #endregion

        #region InputHorizontalAlignment

        /// <summary>
        /// Gets or sets the input field alignment inside the control
        /// </summary>
        [Browsable(true), DefaultValue(eHorizontalAlignment.Right), Category("Appearance")]
        [Description("Indicates alignment of input fields inside of the control.")]
        public eHorizontalAlignment InputHorizontalAlignment
        {
            get { return (_DateTimeInput.InputHorizontalAlignment); }
            set { _DateTimeInput.InputHorizontalAlignment = value; }
        }

        #endregion

        #region IsInputReadOnly

        /// <summary>
        /// Gets or sets whether input part of the control is read-only. When set
        /// to true the input part of the control becomes read-only and does not allow
        /// the typing. However, drop-down part if visible still allows user to change
        /// the value of the control. Use this property to allow change of the value
        /// through drop-down picker only.
        /// </summary>
        [Browsable(true), DefaultValue(false)]
        [Description("Indicates whether input part of the control is read-only.")]
        public bool IsInputReadOnly
        {
            get { return (_DateTimeInput.IsInputReadOnly); }
            set { _DateTimeInput.IsInputReadOnly = value; }
        }

        #endregion

        #region LockUpdateChecked

        /// <summary>
        /// Gets or sets whether check box shown using ShowCheckBox
        /// property which locks/unlocks the control update is checked.
        /// </summary>
        [Browsable(true), DefaultValue(true)]
        [Description("Indicates whether check box shown using ShowCheckBox property which locks/unlocks the control update is checked.")]
        public bool LockUpdateChecked
        {
            get { return (_DateTimeInput.LockUpdateChecked); }
            set { _DateTimeInput.LockUpdateChecked = value; }
        }

        #endregion

        #region MaxDate

        /// <summary>
        /// Gets or sets the maximum date and time that can be selected in the control.
        /// </summary>
        [Browsable(true)]
        [Description("Indicates maximum date and time that can be selected in the control.")]
        public DateTime MaxDate
        {
            get { return (_DateTimeInput.MaxDate); }
            set { _DateTimeInput.MaxDate = value; }
        }

        /// <summary>
        /// Gets whether Value property should be serialized by Windows Forms designer.
        /// </summary>
        /// <returns>true if value serialized otherwise false.</returns>
        public bool ShouldSerializeMaxDate()
        {
            return (MaxDate.Equals(DateTimeGroup.MaxDateTime) == false);
        }

        #endregion

        #region MinDate

        /// <summary>
        /// Gets or sets the minimum date and time that can be selected in the control.
        /// </summary>
        [Browsable(true)]
        [Description("Indicates minimum date and time that can be selected in the control.")]
        public DateTime MinDate
        {
            get { return (_DateTimeInput.MinDate); }
            set { _DateTimeInput.MinDate = value; }
        }

        /// <summary>
        /// Gets whether Value property should be serialized by Windows Forms designer.
        /// </summary>
        /// <returns>true if value serialized otherwise false.</returns>
        public bool ShouldSerializeMinDate()
        {
            return (MinDate.Equals(DateTimeGroup.MinDateTime) == false);
        }

        #endregion

        #region MonthCalendar

        /// <summary>
        /// Gets the reference to the internal MonthCalendarItem control which is used to display calendar when drop-down is open.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Description("Gets the reference to the internal MonthCalendarAdv control which is used to display calendar when drop-down is open.")]
        public MonthCalendarItem MonthCalendar
        {
            get { return (_DateTimeInput.MonthCalendar); }
        }

        #endregion

        #region SelectNextInputCharacters

        /// <summary>
        /// List of characters that when pressed would select next input field. For example if you are
        /// allowing time input you could set this property to : so when user presses the : character,
        /// the input is forwarded to the next input field.
        /// </summary>
        [Browsable(true), DefaultValue(""), Category("Behavior")]
        [Description("List of characters that when pressed would select next input field.")]
        public string SelectNextInputCharacters
        {
            get { return (_DateTimeInput.SelectNextInputCharacters); }
            set { _DateTimeInput.SelectNextInputCharacters = value; }
        }

        #endregion

        #region ShowCheckBox

        /// <summary>
        /// Gets or sets a value indicating whether a check box is
        /// displayed to the left of the input value. Set to true if a check box
        /// is displayed to the left of the input value; otherwise, false. The default is false.
        /// <remarks>
        /// When the ShowCheckBox property is set to true, a check box is displayed
        /// to the left of the input in the control. When the check box is selected, the value
        /// can be updated. When the check box is cleared, the value is unable to be changed.
        /// You can handle the LockUpdateChanged event to be notified when this check box is checked
        /// and unchecked. Use LockUpdateChecked property to get or sets whether check box is checked.
        /// </remarks>
        /// </summary>
        [Browsable(true), DefaultValue(false)]
        [Description("Indicates whether a check box is displayed to the left of the input value which allows locking of the control.")]
        public bool ShowCheckBox
        {
            get { return (_DateTimeInput.ShowCheckBox); }
            set { _DateTimeInput.ShowCheckBox = value; }
        }

        #endregion

        #region ShowUpDown

        /// <summary>
        /// Gets or sets a value indicating whether a spin button control
        /// (up-down control) is used to adjust the current value. The default is false. 
        /// <remarks>
        /// When the ShowUpDown property is set to true, a spin button control
        /// is shown to adjust value of currently focused input item. The value can
        /// be adjusted by using the up and down buttons to change the value.
        /// </remarks>
        /// </summary>
        [Browsable(true), DefaultValue(false)]
        public bool ShowUpDown
        {
            get { return (_DateTimeInput.ShowUpDown); }
            set { _DateTimeInput.ShowUpDown = value; }
        }

        #endregion

        #endregion

        #region Event processing

        #region DoButtonClearClick

        /// <summary>
        /// DoButtonClearClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal void DoButtonClearClick(object sender, CancelEventArgs e)
        {
            if (ButtonClearClick != null)
                ButtonClearClick(this, e);
        }

        #endregion

        #region DoButtonCustomClick

        /// <summary>
        /// DoButtonCustomClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal void DoButtonCustomClick(object sender, EventArgs e)
        {
            if (ButtonCustomClick != null)
                ButtonCustomClick(this, e);
        }

        #endregion

        #region DoButtonCustom2Click

        /// <summary>
        /// DoButtonCustom2Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal void DoButtonCustom2Click(object sender, EventArgs e)
        {
            if (ButtonCustom2Click != null)
                ButtonCustom2Click(this, e);
        }

        #endregion

        #region DoButtonDropDownClick

        /// <summary>
        /// DoButtonDropDownClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal void DoButtonDropDownClick(object sender, CancelEventArgs e)
        {
            if (ButtonDropDownClick != null)
                ButtonDropDownClick(this, e);
        }

        #endregion

        #region DoButtonFreeTextClick

        /// <summary>
        /// DoButtonFreeTextClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal void DoButtonFreeTextClick(object sender, CancelEventArgs e)
        {
            if (ButtonFreeTextClick != null)
                ButtonFreeTextClick(this, e);
        }

        #endregion

        #region DoConvertFreeTextEntry

        /// <summary>
        /// DoConvertFreeTextEntry
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal void DoConvertFreeTextEntry(object sender, FreeTextEntryConversionEventArgs e)
        {
            if (ConvertFreeTextEntry != null)
                ConvertFreeTextEntry(this, e);
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
            DataGridViewDateTimeInputColumn dc = base.Clone() as DataGridViewDateTimeInputColumn;

            if (dc != null)
            {
                dc.AllowEmptyState = AllowEmptyState;
                dc.AutoAdvance = AutoAdvance;
                dc.AutoOverwrite = AutoOverwrite;
                dc.AutoSelectDate = AutoSelectDate;
                dc.CustomFormat = CustomFormat;
                dc.DefaultInputValues = DefaultInputValues;
                dc.Enabled = Enabled;
                dc.Format = Format;
                dc.InputHorizontalAlignment = InputHorizontalAlignment;
                dc.IsInputReadOnly = IsInputReadOnly;
                dc.LockUpdateChecked = LockUpdateChecked;
                dc.MinDate = MinDate;
                dc.MaxDate = MaxDate;
                dc.SelectNextInputCharacters = SelectNextInputCharacters;
                dc.ShowCheckBox = ShowCheckBox;
                dc.ShowUpDown = ShowUpDown;

                dc.DisplayControlForCurrentCellOnly = DisplayControlForCurrentCellOnly;

                DateTimeInput.MonthCalendar.InternalCopyToItem(dc.DateTimeInput.MonthCalendar);

                dc.BackgroundStyle.ApplyStyle(DateTimeInput.BackgroundStyle);
                dc.BackgroundStyle.Class = DateTimeInput.BackgroundStyle.Class;

                DateTimeInput.ButtonClear.CopyToItem(dc.DateTimeInput.ButtonClear);
                DateTimeInput.ButtonDropDown.CopyToItem(dc.DateTimeInput.ButtonDropDown);
                DateTimeInput.ButtonFreeText.CopyToItem(dc.DateTimeInput.ButtonFreeText);
                DateTimeInput.ButtonCustom.CopyToItem(dc.DateTimeInput.ButtonCustom);
                DateTimeInput.ButtonCustom2.CopyToItem(dc.DateTimeInput.ButtonCustom2);
            }

            return (dc);
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
}