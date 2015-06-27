using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using DevComponents.DotNetBar.Rendering;
using DevComponents.Editors;

namespace DevComponents.DotNetBar.Controls
{
    [ToolboxBitmap(typeof(DataGridViewButtonXColumn), "DoubleInput.ico"), ToolboxItem(false), ComVisible(false)]
    public class DataGridViewDoubleInputColumn : DataGridViewTextBoxColumn, IDataGridViewColumn
    {
        #region Events

        /// <summary>
        /// Occurs right before a DoubleInput Cell is painted
        /// </summary>
        [Description("Occurs right before a DoubleInput Cell is painted.")]
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

        private DoubleInput _DoubleInput;
        private Bitmap _CellBitmap;
        private bool _DisplayControlForCurrentCellOnly = true;

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public DataGridViewDoubleInputColumn()
        {
            CellTemplate = new DataGridViewDoubleInputCell();

            _DoubleInput = new DoubleInput();

            _DoubleInput.BackgroundStyle.Class = ElementStyleClassKeys.DataGridViewNumericBorderKey;
            _DoubleInput.Visible = false;
        }

        #region Internal properties

        #region DoubleInput

        /// <summary>
        /// DoubleInput
        /// </summary>
        internal DoubleInput DoubleInput
        {
            get { return (_DoubleInput); }
        }

        #endregion

        #endregion

        #region Public properties

        #region BackgroundStyle

        /// <summary>
        /// Specifies the background style of the control.
        /// </summary>
        [Browsable(true), Category("Style")]
        [Description("Gets or sets control background style.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ElementStyle BackgroundStyle
        {
            get { return (_DoubleInput.BackgroundStyle); }
        }

        /// <summary>
        /// Resets style to default value. Used by windows forms designer.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetBackgroundStyle()
        {
            _DoubleInput.ResetBackgroundStyle();

            _DoubleInput.BackgroundStyle.Class = "TextBoxBorder";
            _DoubleInput.BackgroundStyle.CornerType = eCornerType.Square;
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
            get { return (_DoubleInput.ButtonClear); }
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
            get { return (_DoubleInput.ButtonCustom); }
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
            get { return (_DoubleInput.ButtonCustom2); }
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
            get { return (_DoubleInput.ButtonDropDown); }
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
            get { return (_DoubleInput.ButtonFreeText); }
        }

        #endregion

        #region DisplayFormat

        /// <summary>
        /// Gets or sets the Numeric String Format that is used to format
        /// the numeric value entered for display purpose. Read more about
        /// available formats in MSDN under "Standard Numeric Format Strings"
        /// and "Custom Numeric Format Strings" topics.
        /// <remarks>
        /// The format specified here indicates the format for display purpose
        /// only, not for the input purpose. For example to display the number
        /// in system Currency format set the DisplayFormat to 'C'.
        /// </remarks>
        /// </summary>
        [Browsable(true), DefaultValue("")]
        [Description("Indicates Numeric String Format that is used to format the numeric value entered for display purpose.")]
        public string DisplayFormat
        {
            get { return (_DoubleInput.DisplayFormat); }
            set { _DoubleInput.DisplayFormat = value; }
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
                    _DoubleInput.Invalidate();
                }
            }
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
            get { return (_DoubleInput.Enabled); }
            set { _DoubleInput.Enabled = value; }
        }

        #endregion

        #region Increment

        /// <summary>
        /// Gets or sets the value to increment or decrement the value of the
        /// control when the up or down buttons are clicked. 
        /// </summary>
        [Browsable(true), DefaultValue(1)]
        [Description("Indicates value to increment or decrement the value of the control when the up or down buttons are clicked. ")]
        public double Increment
        {
            get { return (_DoubleInput.Increment); }
            set { _DoubleInput.Increment = value; }
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
            get { return (_DoubleInput.InputHorizontalAlignment); }
            set { _DoubleInput.InputHorizontalAlignment = value; }
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
            get { return (_DoubleInput.LockUpdateChecked); }
            set { _DoubleInput.LockUpdateChecked = value; }
        }

        #endregion

        #region MaxValue

        /// <summary>
        /// Gets or sets the maximum value that can be entered.
        /// </summary>
        [Browsable(true), DefaultValue(double.MaxValue)]
        [Description("Indicates maximum value that can be entered.")]
        public double MaxValue
        {
            get { return (_DoubleInput.MaxValue); }
            set { _DoubleInput.MaxValue = value; }
        }

        #endregion

        #region MinValue

        /// <summary>
        /// Gets or sets the minimum value that can be entered.
        /// </summary>
        [Browsable(true), DefaultValue(double.MinValue)]
        [Description("Indicates minimum value that can be entered.")]
        public double MinValue
        {
            get { return (_DoubleInput.MinValue); }
            set { _DoubleInput.MinValue = value; }
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
            get { return (_DoubleInput.ShowCheckBox); }
            set { _DoubleInput.ShowCheckBox = value; }
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
            get { return (_DoubleInput.ShowUpDown); }
            set { _DoubleInput.ShowUpDown = value; }
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
            DataGridViewDoubleInputColumn dc = base.Clone() as DataGridViewDoubleInputColumn;

            if (dc != null)
            {
                dc.DisplayFormat = DisplayFormat;
                dc.Enabled = Enabled;
                dc.Increment = Increment;
                dc.InputHorizontalAlignment = InputHorizontalAlignment;
                dc.LockUpdateChecked = LockUpdateChecked;
                dc.MaxValue = MaxValue;
                dc.MinValue = MinValue;
                dc.ShowCheckBox = ShowCheckBox;
                dc.ShowUpDown = ShowUpDown;

                dc.DisplayControlForCurrentCellOnly = DisplayControlForCurrentCellOnly;

                dc.BackgroundStyle.ApplyStyle(DoubleInput.BackgroundStyle);
                dc.BackgroundStyle.Class = DoubleInput.BackgroundStyle.Class;

                DoubleInput.ButtonClear.CopyToItem(dc.DoubleInput.ButtonClear);
                DoubleInput.ButtonDropDown.CopyToItem(dc.DoubleInput.ButtonDropDown);
                DoubleInput.ButtonFreeText.CopyToItem(dc.DoubleInput.ButtonFreeText);
                DoubleInput.ButtonCustom.CopyToItem(dc.DoubleInput.ButtonCustom);
                DoubleInput.ButtonCustom2.CopyToItem(dc.DoubleInput.ButtonCustom2);
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
