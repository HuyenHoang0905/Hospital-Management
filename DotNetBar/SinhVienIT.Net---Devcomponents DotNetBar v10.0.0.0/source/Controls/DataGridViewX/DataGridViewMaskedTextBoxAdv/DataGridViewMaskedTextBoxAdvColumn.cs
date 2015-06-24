using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using DevComponents.DotNetBar.Rendering;
using DevComponents.Editors;

namespace DevComponents.DotNetBar.Controls
{
    [ToolboxBitmap(typeof(DataGridViewMaskedTextBoxAdvColumn), "Controls.MaskedTextBoxAdv.ico"), ToolboxItem(false), ComVisible(false)]
    public class DataGridViewMaskedTextBoxAdvColumn : DataGridViewTextBoxColumn, IDataGridViewColumn
    {
        #region Events

        /// <summary>
        /// Occurs right before a MaskedTextBoxAdv Cell is painted
        /// </summary>
        [Description("Occurs right before a MaskedTextBoxAdv Cell is painted.")]
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
        /// Occurs when Drop-Down button is clicked and allows you to cancel showing of the popup
        /// </summary>
        [Description("Occurs when Drop-Down button is clicked and allows you to cancel showing of the popup.")]
        public event EventHandler<CancelEventArgs> ButtonDropDownClick;

        #endregion

        #region Private variables

        private MaskedTextBoxAdv _MaskedTextBoxAdv;
        private Bitmap _CellBitmap;
        private bool _DisplayControlForCurrentCellOnly = true;

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public DataGridViewMaskedTextBoxAdvColumn()
        {
            CellTemplate = new DataGridViewMaskedTextBoxAdvCell();

            _MaskedTextBoxAdv = new MaskedTextBoxAdv();

            _MaskedTextBoxAdv.BackgroundStyle.Class = ElementStyleClassKeys.DataGridViewBorderKey;
        }

        #region Internal properties

        #region MaskedTextBoxAdv

        /// <summary>
        /// Gets the underlying MaskedTextBoxAdv control
        /// </summary>
        [Browsable(false)]
        internal MaskedTextBoxAdv MaskedTextBoxAdv
        {
            get { return (_MaskedTextBoxAdv); }
        }

        #endregion

        #endregion

        #region Public properties

        #region AllowPromptAsInput

        /// <summary>
        /// Gets or sets a value indicating whether PromptChar can be entered as valid data.
        /// </summary>
        [Browsable(true), DefaultValue(true)]
        [Description("Indicates whether PromptChar can be entered as valid data.")]
        public bool AllowPromptAsInput
        {
            get { return (_MaskedTextBoxAdv.AllowPromptAsInput); }
            set { _MaskedTextBoxAdv.AllowPromptAsInput = value; }
        }

        #endregion

        #region AsciiOnly

        /// <summary>
        /// Gets or sets a value indicating whether characters outside of the ASCII character set will be accepted.
        /// </summary>
        [Browsable(true), DefaultValue(false)]
        [Description("Indicates whether whether characters outside of the ASCII character set will be accepted.")]
        public bool AsciiOnly
        {
            get { return (_MaskedTextBoxAdv.AsciiOnly); }
            set { _MaskedTextBoxAdv.AsciiOnly = value; }
        }

        #endregion

        #region BackColor

        /// <summary>
        /// Gets or sets the Background color.
        /// </summary>
        [Browsable(false)]
        public Color BackColor
        {
            get { return (_MaskedTextBoxAdv.BackColor); }
            set { _MaskedTextBoxAdv.BackColor = value; }
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
            get { return (_MaskedTextBoxAdv.BackgroundStyle); }
        }

        /// <summary>
        /// Resets style to default value. Used by windows forms designer.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetBackgroundStyle()
        {
            _MaskedTextBoxAdv.ResetBackgroundStyle();

            _MaskedTextBoxAdv.BackgroundStyle.Class = "TextBoxBorder";
            _MaskedTextBoxAdv.BackgroundStyle.CornerType = eCornerType.Square;
        }

        #endregion

        #region BeepOnError

        /// <summary>
        /// Gets or sets a value indicating whether the masked text box
        /// control raises the system beep for each user key stroke that it rejects.
        /// </summary>
        [Browsable(true), DefaultValue(false)]
        [Description("Indicates whether the masked text box control raises the system beep for each user key stroke that it rejects.")]
        public bool BeepOnError
        {
            get { return (_MaskedTextBoxAdv.BeepOnError); }
            set { _MaskedTextBoxAdv.BeepOnError = value; }
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
            get { return (_MaskedTextBoxAdv.ButtonClear); }
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
            get { return (_MaskedTextBoxAdv.ButtonCustom); }
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
            get { return (_MaskedTextBoxAdv.ButtonCustom2); }
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
            get { return (_MaskedTextBoxAdv.ButtonDropDown); }
        }

        #endregion

        #region Culture

        /// <summary>
        /// Gets or sets the culture information associated with the masked text box.
        /// </summary>
        [Browsable(true), DefaultValue(false)]
        [Description("Indicates the culture information associated with the masked text box.")]
        public CultureInfo Culture
        {
            get { return (_MaskedTextBoxAdv.Culture); }
            set { _MaskedTextBoxAdv.Culture = value; }
        }

        #endregion

        #region CutCopyMaskFormat

        /// <summary>
        /// Gets or sets a value that determines whether
        /// literals and prompt characters are copied to the clipboard
        /// </summary>
        [Browsable(true), DefaultValue(MaskFormat.IncludeLiterals)]
        [Description("Indicates whether literals and prompt characters are copied to the clipboard.")]
        public MaskFormat CutCopyMaskFormat
        {
            get { return (_MaskedTextBoxAdv.CutCopyMaskFormat); }
            set { _MaskedTextBoxAdv.CutCopyMaskFormat = value; }
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
                    _MaskedTextBoxAdv.Invalidate();
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
            get { return (_MaskedTextBoxAdv.DropDownControl); }
            set { _MaskedTextBoxAdv.DropDownControl = value; }
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
            get { return (_MaskedTextBoxAdv.Enabled); }
            set { _MaskedTextBoxAdv.Enabled = value; }
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
            get { return (_MaskedTextBoxAdv.FocusHighlightColor); }
            set { _MaskedTextBoxAdv.FocusHighlightColor = value; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeFocusHighlightColor()
        {
            return (_MaskedTextBoxAdv.ShouldSerializeFocusHighlightColor());
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetFocusHighlightColor()
        {
            _MaskedTextBoxAdv.ResetFocusHighlightColor();
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
            get { return (_MaskedTextBoxAdv.FocusHighlightEnabled); }
            set { _MaskedTextBoxAdv.FocusHighlightEnabled = value; }
        }

        #endregion

        #region ForeColor

        /// <summary>
        /// Gets or sets the foreground color.
        /// </summary>
        [Browsable(false)]
        public Color ForeColor
        {
            get { return (_MaskedTextBoxAdv.ForeColor); }
            set { _MaskedTextBoxAdv.ForeColor = value; }
        }

        #endregion

        #region HidePromptOnLeave

        /// <summary>
        /// Gets or sets a value indicating whether the prompt characters
        /// in the input mask are hidden when the masked text box loses focus.
        /// </summary>
        [Browsable(true), DefaultValue(false)]
        [Description("Indicates whether the prompt characters in the input mask are hidden when the masked text box loses focus.")]
        public bool HidePromptOnLeave
        {
            get { return (_MaskedTextBoxAdv.HidePromptOnLeave); }
            set { _MaskedTextBoxAdv.HidePromptOnLeave = value; }
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
            get { return (_MaskedTextBoxAdv.ImeMode); }
            set { _MaskedTextBoxAdv.ImeMode = value; }
        }

        #endregion

        #region InsertKeyMode

        /// <summary>
        /// Gets or sets the text insertion mode of the masked text box control.
        /// </summary>
        [Browsable(true), DefaultValue(InsertKeyMode.Default)]
        [Description("Indicates the text insertion mode of the masked text box control.")]
        public InsertKeyMode InsertKeyMode
        {
            get { return (_MaskedTextBoxAdv.InsertKeyMode); }
            set { _MaskedTextBoxAdv.InsertKeyMode = value; }
        }

        #endregion

        #region Mask

        /// <summary>
        /// Gets or sets the input mask to use at run time.
        /// </summary>
        [Browsable(true), DefaultValue(null)]
        [Description("Indicates the input mask to use at run time.")]
        public string Mask
        {
            get { return (_MaskedTextBoxAdv.Mask); }
            set { _MaskedTextBoxAdv.Mask = value; }
        }

        #endregion

        #region PasswordChar

        /// <summary>
        /// Gets or sets the character to be displayed in substitute for user input.
        /// </summary>
        [Browsable(true), DefaultValue(null)]
        [Description("Indicates the character to be displayed in substitute for user input.")]
        public char PasswordChar
        {
            get { return (_MaskedTextBoxAdv.PasswordChar); }
            set { _MaskedTextBoxAdv.PasswordChar = value; }
        }

        #endregion

        #region PromptChar

        /// <summary>
        /// Gets or sets the character used to represent the absence of user input.
        /// </summary>
        [Browsable(true), DefaultValue('_')]
        [Description("Indicates the character used to represent the absence of user input.")]
        public char PromptChar
        {
            get { return (_MaskedTextBoxAdv.PromptChar); }
            set { _MaskedTextBoxAdv.PromptChar = value; }
        }

        #endregion

        #region RejectInputOnFirstFailure

        /// <summary>
        /// Gets or sets a value indicating whether the parsing of
        /// user input should stop after the first invalid character is reached.
        /// </summary>
        [Browsable(true), DefaultValue(false)]
        [Description("Indicates the parsing of user input should stop after the first invalid character is reached.")]
        public bool RejectInputOnFirstFailure
        {
            get { return (_MaskedTextBoxAdv.RejectInputOnFirstFailure); }
            set { _MaskedTextBoxAdv.RejectInputOnFirstFailure = value; }
        }

        #endregion

        #region ResetOnPrompt

        /// <summary>
        /// Gets or sets a value that determines how an input
        /// character that matches the prompt character should be handled.
        /// </summary>
        [Browsable(true), DefaultValue(true)]
        [Description("Indicates how an input character that matches the prompt character should be handled.")]
        public bool ResetOnPrompt
        {
            get { return (_MaskedTextBoxAdv.ResetOnPrompt); }
            set { _MaskedTextBoxAdv.ResetOnPrompt = value; }
        }

        #endregion

        #region ResetOnSpace

        /// <summary>
        /// Gets or sets a value that determines how
        /// a space input character should be handled.
        /// </summary>
        [Browsable(true), DefaultValue(true)]
        [Description("Indicates how a space input character should be handled.")]
        public bool ResetOnSpace
        {
            get { return (_MaskedTextBoxAdv.ResetOnSpace); }
            set { _MaskedTextBoxAdv.ResetOnSpace = value; }
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
            get { return (_MaskedTextBoxAdv.RightToLeft); }
            set { _MaskedTextBoxAdv.RightToLeft = value; }
        }

        #endregion

        #region SkipLiterals

        /// <summary>
        /// Gets or sets a value indicating
        /// whether the user is allowed to reenter literal values.
        /// </summary>
        [Browsable(true), DefaultValue(true)]
        [Description("Indicates the user is allowed to reenter literal values.")]
        public bool SkipLiterals
        {
            get { return (_MaskedTextBoxAdv.SkipLiterals); }
            set { _MaskedTextBoxAdv.SkipLiterals = value; }
        }

        #endregion

        #region Text

        /// <summary>
        /// Gets or sets the text as it is currently displayed to the user.
        /// </summary>
        [Browsable(false)]
        public string Text
        {
            get { return (_MaskedTextBoxAdv.Text); }
            set { _MaskedTextBoxAdv.Text = value; }
        }

        #endregion

        #region TextAlign

        /// <summary>
        /// Gets or sets how text is aligned in a masked text box control.
        /// </summary>
        [Browsable(true), DefaultValue(HorizontalAlignment.Left)]
        [Description("Indicates how text is aligned in a masked text box control.")]
        public HorizontalAlignment TextAlign
        {
            get { return (_MaskedTextBoxAdv.TextAlign); }
            set { _MaskedTextBoxAdv.TextAlign = value; }
        }

        #endregion

        #region TextMaskFormat

        /// <summary>
        /// Gets or sets a value that determines whether literals
        /// and prompt characters are included in the formatted string.
        /// </summary>
        [Browsable(true), DefaultValue(MaskFormat.IncludeLiterals)]
        [Description("Indicates whether literals and prompt characters are included in the formatted string.")]
        public MaskFormat TextMaskFormat
        {
            get { return (_MaskedTextBoxAdv.TextMaskFormat); }
            set { _MaskedTextBoxAdv.TextMaskFormat = value; }
        }

        #endregion

        #region UseSystemPasswordChar

        /// <summary>
        /// Gets or sets a value indicating whether
        /// the operating system-supplied password character should be used.
        /// </summary>
        [Browsable(true), DefaultValue(false)]
        [Description("Indicates whether the operating system-supplied password character should be used.")]
        public bool UseSystemPasswordChar
        {
            get { return (_MaskedTextBoxAdv.UseSystemPasswordChar); }
            set { _MaskedTextBoxAdv.UseSystemPasswordChar = value; }
        }

        #endregion

        #region ValidatingType

        /// <summary>
        /// Gets or sets the data type used to verify the data input by the user.
        /// </summary>
        [Browsable(false), DefaultValue(null)]
        [Description("Indicates the data type used to verify the data input by the user.")]
        public Type ValidatingType
        {
            get { return (_MaskedTextBoxAdv.ValidatingType); }
            set { _MaskedTextBoxAdv.ValidatingType = value; }
        }

        #endregion

        #region WatermarkBehavior

        /// <summary>
        /// Gets or sets the watermark hiding behaviour. Default value
        /// indicates that watermark is hidden when control receives input focus.
        /// </summary>
        [Browsable(true), DefaultValue(eWatermarkBehavior.HideOnFocus), Category("Behavior")]
        [Description("Indicates watermark hiding behaviour.")]
        public eWatermarkBehavior WatermarkBehavior
        {
            get { return (_MaskedTextBoxAdv.WatermarkBehavior); }
            set { _MaskedTextBoxAdv.WatermarkBehavior = value; }
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
            get { return (_MaskedTextBoxAdv.WatermarkColor); }
            set { _MaskedTextBoxAdv.WatermarkColor = value; }
        }
        /// <summary>
        /// Indicates whether property should be serialized by Windows Forms designer.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeWatermarkColor()
        {
            return (_MaskedTextBoxAdv.ShouldSerializeWatermarkColor());
        }
        /// <summary>
        /// Resets the property to default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetWatermarkColor()
        {
            _MaskedTextBoxAdv.ResetWatermarkColor();
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
            get { return (_MaskedTextBoxAdv.WatermarkEnabled); }
            set { _MaskedTextBoxAdv.WatermarkEnabled = value; }
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
            get { return (_MaskedTextBoxAdv.WatermarkFont); }
            set { _MaskedTextBoxAdv.WatermarkFont = value; }
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
            get { return (_MaskedTextBoxAdv.WatermarkText); }
            set { _MaskedTextBoxAdv.WatermarkText = value; }
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
            DataGridViewMaskedTextBoxAdvColumn dc = base.Clone() as DataGridViewMaskedTextBoxAdvColumn;

            if (dc != null)
            {
                dc.AllowPromptAsInput = AllowPromptAsInput;
                dc.AsciiOnly = AsciiOnly;
                dc.BeepOnError = BeepOnError;
                dc.Culture = Culture;
                dc.CutCopyMaskFormat = CutCopyMaskFormat;
                dc.DropDownControl = DropDownControl;
                dc.Enabled = Enabled;
                dc.FocusHighlightColor = FocusHighlightColor;
                dc.FocusHighlightEnabled = FocusHighlightEnabled;
                dc.HidePromptOnLeave = HidePromptOnLeave;
                dc.ImeMode = ImeMode;
                dc.InsertKeyMode = InsertKeyMode;
                dc.Mask = Mask;
                dc.PasswordChar = PasswordChar;
                dc.PromptChar = PromptChar;
                dc.RejectInputOnFirstFailure = RejectInputOnFirstFailure;
                dc.ResetOnPrompt = ResetOnPrompt;
                dc.ResetOnSpace = ResetOnSpace;
                dc.RightToLeft = RightToLeft;
                dc.SkipLiterals = SkipLiterals;
                dc.TextAlign = TextAlign;
                dc.TextMaskFormat = TextMaskFormat;
                dc.UseSystemPasswordChar = UseSystemPasswordChar;
                dc.ValidatingType = ValidatingType;
                dc.WatermarkBehavior = WatermarkBehavior;
                dc.WatermarkColor = WatermarkColor;
                dc.WatermarkEnabled = WatermarkEnabled;
                dc.WatermarkFont = WatermarkFont;
                dc.WatermarkText = WatermarkText;

                dc.DisplayControlForCurrentCellOnly = DisplayControlForCurrentCellOnly;

                dc.BackgroundStyle.ApplyStyle(MaskedTextBoxAdv.BackgroundStyle);
                dc.BackgroundStyle.Class = MaskedTextBoxAdv.BackgroundStyle.Class;

                MaskedTextBoxAdv.ButtonClear.CopyToItem(dc.MaskedTextBoxAdv.ButtonClear);
                MaskedTextBoxAdv.ButtonDropDown.CopyToItem(dc.MaskedTextBoxAdv.ButtonDropDown);
                MaskedTextBoxAdv.ButtonCustom.CopyToItem(dc.MaskedTextBoxAdv.ButtonCustom);
                MaskedTextBoxAdv.ButtonCustom2.CopyToItem(dc.MaskedTextBoxAdv.ButtonCustom2);
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
