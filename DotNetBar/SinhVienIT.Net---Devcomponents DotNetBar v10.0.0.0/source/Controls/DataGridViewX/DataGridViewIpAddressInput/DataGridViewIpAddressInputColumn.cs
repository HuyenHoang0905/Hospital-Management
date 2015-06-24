using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using DevComponents.DotNetBar.Rendering;
using DevComponents.Editors;

namespace DevComponents.DotNetBar.Controls
{
    [ToolboxBitmap(typeof(DataGridViewButtonXColumn), "IpAddressInput.ico"), ToolboxItem(false), ComVisible(false)]
    public class DataGridViewIpAddressInputColumn : DataGridViewTextBoxColumn, IDataGridViewColumn
    {
        #region Events

        /// <summary>
        /// Occurs right before a IpAddressInput Cell is painted
        /// </summary>
        [Description("Occurs right before a IpAddressInput Cell is painted.")]
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

        #endregion

        #region Private variables

        private IpAddressInput _IpAddressInput;
        private Bitmap _CellBitmap;
        private bool _DisplayControlForCurrentCellOnly = true;

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public DataGridViewIpAddressInputColumn()
        {
            CellTemplate = new DataGridViewIpAddressInputCell();

            _IpAddressInput = new IpAddressInput();

            _IpAddressInput.BackgroundStyle.Class = ElementStyleClassKeys.DataGridViewIpAddressBorderKey;
        }

        #region Internal properties

        #region IpAddressInput

        /// <summary>
        /// Gets the underlying IpAddressInput control
        /// </summary>
        [Browsable(false)]
        internal IpAddressInput IpAddressInput
        {
            get { return (_IpAddressInput); }
        }

        #endregion

        #endregion

        #region Public properties

        #region AllowEmptyState

        /// <summary>
        /// Gets or sets whether empty null/nothing state of the control
        /// is allowed. Default value is true which means that Text property
        /// may return null if there is no input value.
        /// </summary>
        [Browsable(true), DefaultValue(true)]
        [Description("Indicates whether empty null/nothing state of the control is allowed.")]
        public bool AllowEmptyState
        {
            get { return (_IpAddressInput.AllowEmptyState); }
            set { _IpAddressInput.AllowEmptyState = value; }
        }

        #endregion

        #region AutoOffFreeTextEntry

        /// <summary>
        /// Gets or sets whether free-text entry is automatically
        /// turned off when control loses input focus. Default value is false.
        /// </summary>
        [Browsable(true), DefaultValue(false), Category("Free-Text")]
        [Description("Indicates whether free-text entry is automatically turned off when control loses input focus.")]
        public bool AutoOffFreeTextEntry
        {
            get { return (_IpAddressInput.AutoOffFreeTextEntry); }
            set { _IpAddressInput.AutoOffFreeTextEntry = value; }
        }

        #endregion

        #region AutoOverwrite

        /// <summary>
        /// Gets or sets whether auto-overwrite functionality for input
        /// is enabled. When in auto-overwrite mode input field will erase existing entry
        /// and start new one if typing is continued after InputComplete method is called.
        /// </summary>
        [Browsable(true), DefaultValue(false)]
        [Description("Indicates whether auto-overwrite functionality for input is enabled.")]
        public bool AutoOverwrite
        {
            get { return (_IpAddressInput.AutoOverwrite); }
            set { _IpAddressInput.AutoOverwrite = value; }
        }

        #endregion

        #region AutoResolveFreeTextEntries

        /// <summary>
        /// Gets or sets whether free text entries are attempted to be
        /// auto-resolved to IP address as host/domain names. Default value is true.
        /// </summary>
        [Browsable(true), DefaultValue(true), Category("Free-Text")]
        [Description("Indicates whether free text entries are attempted to be auto-resolved to IP address as host/domain names.")]
        public bool AutoResolveFreeTextEntries
        {
            get { return (_IpAddressInput.AutoResolveFreeTextEntries); }
            set { _IpAddressInput.AutoResolveFreeTextEntries = value; }
        }

        #endregion

        #region BackColor

        /// <summary>
        /// Gets or sets the Background color.
        /// </summary>
        [Browsable(false)]
        public Color BackColor
        {
            get { return (_IpAddressInput.BackColor); }
            set { _IpAddressInput.BackColor = value; }
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
            get { return (_IpAddressInput.BackgroundStyle); }
        }

        /// <summary>
        /// Resets style to default value. Used by windows forms designer.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetBackgroundStyle()
        {
            _IpAddressInput.ResetBackgroundStyle();

            _IpAddressInput.BackgroundStyle.Class = "TextBoxBorder";
            _IpAddressInput.BackgroundStyle.CornerType = eCornerType.Square;
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
            get { return (_IpAddressInput.ButtonClear); }
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
            get { return (_IpAddressInput.ButtonCustom); }
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
            get { return (_IpAddressInput.ButtonCustom2); }
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
            get { return (_IpAddressInput.ButtonDropDown); }
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
                    _IpAddressInput.Invalidate();
                }
            }
        }

        #endregion

        #region DropDownControl

        /// <summary>
        /// Gets or sets the reference of the control that will be
        /// displayed on popup that is shown when the drop-down button is clicked.
        /// </summary>
        [DefaultValue(null)]
        [Description("Indicates reference of the control that will be displayed on popup that is shown when the drop-down button is clicked.")]
        public Control DropDownControl
        {
            get { return (_IpAddressInput.DropDownControl); }
            set { _IpAddressInput.DropDownControl = value; }
        }

        #endregion

        #region DropDownItems

        /// <summary>
        /// Returns the collection of DropDownItems.
        /// </summary>
        [Browsable(false), DevCoBrowsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SubItemsCollection DropDownItems
        {
            get { return (_IpAddressInput.DropDownItems); }
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
            get { return (_IpAddressInput.Enabled); }
            set { _IpAddressInput.Enabled = value; }
        }

        #endregion

        #region FocusHighlightColor

        /// <summary>
        /// Gets or sets the color used as background color to highlight
        /// the text box when it has input focus and FocusHighlight is enabled.
        /// </summary>
        [Browsable(true), Category("Appearance")]
        [Description("Indicates color used as background color to highlight the text box when it has input focus and FocusHighlight is enabled.")]
        public Color FocusHighlightColor
        {
            get { return (_IpAddressInput.FocusHighlightColor); }
            set { _IpAddressInput.FocusHighlightColor = value; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeFocusHighlightColor()
        {
            return (_IpAddressInput.ShouldSerializeFocusHighlightColor());
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetFocusHighlightColor()
        {
            _IpAddressInput.ResetFocusHighlightColor();
        }

        #endregion

        #region FocusHighlightEnabled

        /// <summary>
        /// Gets or sets whether FocusHighlightColor is used as
        /// background color to highlight the text box when it has
        /// input focus. Default value is false.
        /// </summary>
        [Browsable(true), DefaultValue(false), Category("Appearance")]
        [Description("Indicates whether FocusHighlightColor is used as background color to highlight the text box when it has input focus.")]
        public bool FocusHighlightEnabled
        {
            get { return (_IpAddressInput.FocusHighlightEnabled); }
            set { _IpAddressInput.FocusHighlightEnabled = value; }
        }

        #endregion

        #region ForeColor

        /// <summary>
        /// Gets or sets the foreground color.
        /// </summary>
        [Browsable(false)]
        public Color ForeColor
        {
            get { return (_IpAddressInput.ForeColor); }
            set { _IpAddressInput.ForeColor = value; }
        }

        #endregion

        #region FreeTextEntryMode

        /// <summary>
        /// Gets or sets whether control input is in
        /// free-text input mode. Default value is false.
        /// </summary>
        [Browsable(true), DefaultValue(false), Category("Free-Text")]
        [Description("Indicates whether control input is in free-text input mode.")]
        public bool FreeTextEntryMode
        {
            get { return (_IpAddressInput.FreeTextEntryMode); }
            set { _IpAddressInput.FreeTextEntryMode = value; }
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
            get { return (_IpAddressInput.ImeMode); }
            set { _IpAddressInput.ImeMode = value; }
        }

        #endregion

        #region IsInputReadOnly

        /// <summary>
        /// Gets or sets whether input part of the control is read-only. When
        /// set to true the input part of the control becomes read-only and does
        /// not allow the typing. However, drop-down part if visible still allows
        /// user to possibly change the value of the control through the method you
        /// can provide on drop-down. Use this property to allow change of the value
        /// through drop-down button only.
        /// </summary>
        [Browsable(true), DefaultValue(false)]
        [Description("Indicates whether input part of the control is read-only.")]
        public bool IsInputReadOnly
        {
            get { return (_IpAddressInput.IsInputReadOnly); }
            set { _IpAddressInput.IsInputReadOnly = value; }
        }

        #endregion

        #region LockUpdateChecked

        /// <summary>
        /// Gets or sets whether check box shown using ShowCheckBox
        /// property which locks/unlocks the control update is checked.
        /// </summary>
        [Browsable(true), DefaultValue(false)]
        [Description("Indicates whether check box shown using ShowCheckBox property which locks/unlocks the control update is checked.")]
        public bool LockUpdateChecked
        {
            get { return (_IpAddressInput.LockUpdateChecked); }
            set { _IpAddressInput.LockUpdateChecked = value; }
        }

        #endregion

        #region RightToLeft

        /// <summary>
        /// Gets or sets a value indicating whether control's
        /// elements are aligned to support locales using right-to-left fonts.
        /// </summary>
        [Browsable(true), DefaultValue(RightToLeft.Inherit)]
        [Description("Indicates the control's elements are aligned to support locales using right-to-left fonts.")]
        public RightToLeft RightToLeft
        {
            get { return (_IpAddressInput.RightToLeft); }
            set { _IpAddressInput.RightToLeft = value; }
        }

        #endregion

        #region SelectNextInputCharacters

        /// <summary>
        /// List of characters that when pressed would select next input field. For example if you are
        /// allowing time input you could set this property to : so when user presses the : character,
        /// the input is forwarded to the next input field.
        /// </summary>
        [DefaultValue("."), Category("Behavior")]
        [Description("List of characters that when pressed would select next input field.")]
        public string SelectNextInputCharacters
        {
            get { return (_IpAddressInput.SelectNextInputCharacters); }
            set { _IpAddressInput.SelectNextInputCharacters = value; }
        }

        #endregion

        #region ShowCheckBox

        /// <summary>
        /// Gets or sets a value indicating whether a check box is displayed to
        /// the left of the input value. Set to true if a check box is displayed
        /// to the left of the input value; otherwise, false. The default is false.
        /// </summary>
        [Browsable(true), DefaultValue(false)]
        [Description("Indicates whether a check box is displayed to the left of the input value which allows locking of the control.")]
        public bool ShowCheckBox
        {
            get { return (_IpAddressInput.ShowCheckBox); }
            set { _IpAddressInput.ShowCheckBox = value; }
        }

        #endregion

        #region Text

        /// <summary>
        /// Gets or sets the text as it is currently displayed to the user.
        /// </summary>
        [Browsable(false)]
        public string Text
        {
            get { return (_IpAddressInput.Text); }
            set { _IpAddressInput.Text = value; }
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
            get { return (_IpAddressInput.WatermarkColor); }
            set { _IpAddressInput.WatermarkColor = value; }
        }
        /// <summary>
        /// Indicates whether property should be serialized by Windows Forms designer.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeWatermarkColor()
        {
            return (_IpAddressInput.ShouldSerializeWatermarkColor());
        }
        /// <summary>
        /// Resets the property to default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetWatermarkColor()
        {
            _IpAddressInput.ResetWatermarkColor();
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
            get { return (_IpAddressInput.WatermarkEnabled); }
            set { _IpAddressInput.WatermarkEnabled = value; }
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
            get { return (_IpAddressInput.WatermarkFont); }
            set { _IpAddressInput.WatermarkFont = value; }
        }

        #endregion

        #region WatermarkText

        /// <summary>
        /// Gets or sets the watermark (tip) text displayed inside of
        /// the control when Text is not set and control does not have
        /// input focus. This property supports text-markup.
        /// </summary>
        [Browsable(true), DefaultValue(""), Localizable(true), Category("Appearance")]
        [Description("Indicates watermark text displayed inside of the control when Text is not set and control does not have input focus.")]
        [Editor("DevComponents.DotNetBar.Design.TextMarkupUIEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(System.Drawing.Design.UITypeEditor))]
        public string WatermarkText
        {
            get { return (_IpAddressInput.WatermarkText); }
            set { _IpAddressInput.WatermarkText = value; }
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
            DataGridViewIpAddressInputColumn dc = base.Clone() as DataGridViewIpAddressInputColumn;

            if (dc != null)
            {
                dc.AllowEmptyState = AllowEmptyState;
                dc.AutoOffFreeTextEntry = AutoOffFreeTextEntry;
                dc.AutoOverwrite = AutoOverwrite;
                dc.AutoResolveFreeTextEntries = AutoResolveFreeTextEntries;
                dc.DropDownControl = DropDownControl;
                dc.Enabled = Enabled;
                dc.FocusHighlightColor = FocusHighlightColor;
                dc.FocusHighlightEnabled = FocusHighlightEnabled;
                dc.FreeTextEntryMode = FreeTextEntryMode;
                dc.ImeMode = ImeMode;
                dc.IsInputReadOnly = IsInputReadOnly;
                dc.LockUpdateChecked = LockUpdateChecked;
                dc.RightToLeft = RightToLeft;
                dc.SelectNextInputCharacters = SelectNextInputCharacters;
                dc.ShowCheckBox = ShowCheckBox;
                dc.WatermarkColor = WatermarkColor;
                dc.WatermarkEnabled = WatermarkEnabled;
                dc.WatermarkFont = WatermarkFont;
                dc.WatermarkText = WatermarkText;

                dc.DisplayControlForCurrentCellOnly = DisplayControlForCurrentCellOnly;

                dc.BackgroundStyle.ApplyStyle(IpAddressInput.BackgroundStyle);
                dc.BackgroundStyle.Class = IpAddressInput.BackgroundStyle.Class;

                IpAddressInput.ButtonClear.CopyToItem(dc.IpAddressInput.ButtonClear);
                IpAddressInput.ButtonDropDown.CopyToItem(dc.IpAddressInput.ButtonDropDown);
                IpAddressInput.ButtonCustom.CopyToItem(dc.IpAddressInput.ButtonCustom);
                IpAddressInput.ButtonCustom2.CopyToItem(dc.IpAddressInput.ButtonCustom2);
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
