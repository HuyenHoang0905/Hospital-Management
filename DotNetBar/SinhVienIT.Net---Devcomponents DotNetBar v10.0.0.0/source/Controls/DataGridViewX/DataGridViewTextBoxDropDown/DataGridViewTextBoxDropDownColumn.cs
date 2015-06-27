using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms;
using DevComponents.DotNetBar.Rendering;
using DevComponents.Editors;

namespace DevComponents.DotNetBar.Controls
{
    [ToolboxBitmap(typeof(DataGridViewTextBoxDropDownColumn), "Controls.TextBoxDropDown.ico"), ToolboxItem(false), ComVisible(false)]
    public class DataGridViewTextBoxDropDownColumn : DataGridViewTextBoxColumn, IDataGridViewColumn
    {
        #region Events

        /// <summary>
        /// Occurs right before a TextBoxDropDown Cell is painted
        /// </summary>
        [Description("Occurs right before a TextBoxDropDown Cell is painted.")]
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

        private TextBoxDropDown _TextBoxDropDown;
        private Bitmap _CellBitmap;
        private bool _DisplayControlForCurrentCellOnly = true;

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public DataGridViewTextBoxDropDownColumn()
        {
            CellTemplate = new DataGridViewTextBoxDropDownCell();

            _TextBoxDropDown = new TextBoxDropDown();

            _TextBoxDropDown.BackgroundStyle.Class = ElementStyleClassKeys.DataGridViewIpAddressBorderKey;
        }

        #region Internal properties

        #region TextBoxDropDown

        /// <summary>
        /// Gets the underlying TextBoxDropDown control
        /// </summary>
        [Browsable(false)]
        internal TextBoxDropDown TextBoxDropDown
        {
            get { return (_TextBoxDropDown); }
        }

        #endregion

        #endregion

        #region Public properties

        #region AutoCompleteCustomSource

        /// <summary>
        /// Gets or sets a custom StringCollection to use when
        /// the AutoCompleteSource property is set to CustomSource.
        /// <value>A StringCollection to use with AutoCompleteSource.</value>
        /// </summary>
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Localizable(true)]
        [Description("Indicates custom StringCollection to use when the AutoCompleteSource property is set to CustomSource.")]
        [Editor("System.Windows.Forms.Design.ListControlStringCollectionEditor, System.Design, Version=9.5.0.16, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        public AutoCompleteStringCollection AutoCompleteCustomSource
        {
            get { return (_TextBoxDropDown.AutoCompleteCustomSource); }
            set { _TextBoxDropDown.AutoCompleteCustomSource = value; }
        }

        #endregion

        #region AutoCompleteMode

        /// <summary>
        /// Gets or sets an option that controls
        /// how automatic completion works for the TextBox.
        /// <value>One of the values of AutoCompleteMode. The values are Append,
        /// None, Suggest, and SuggestAppend. The default is None.</value>
        /// </summary>
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), DefaultValue(0)]
        [Description("Gets or sets an option that controls how automatic completion works for the TextBox.")]
        public AutoCompleteMode AutoCompleteMode
        {
            get { return (_TextBoxDropDown.AutoCompleteMode); }
            set { _TextBoxDropDown.AutoCompleteMode = value; }
        }

        #endregion

        #region AutoCompleteSource

        /// <summary>
        /// Gets or sets a value specifying the source of complete strings used for automatic completion.
        /// <value>One of the values of AutoCompleteSource. The options are AllSystemSources, AllUrl, FileSystem, HistoryList, RecentlyUsedList, CustomSource, and None. The default is None.</value>
        /// </summary>
        [ Browsable(true), DefaultValue(0x80), TypeConverter(typeof(TextBoxAutoCompleteSourceConverter))]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Description("Gets or sets a value specifying the source of complete strings used for automatic completion.")]
        public AutoCompleteSource AutoCompleteSource
        {
            get { return (_TextBoxDropDown.AutoCompleteSource); }
            set { _TextBoxDropDown.AutoCompleteSource = value; }
        }

        #endregion

        #region BackColor

        /// <summary>
        /// Gets or sets the Background color.
        /// </summary>
        [Browsable(false)]
        public Color BackColor
        {
            get { return (_TextBoxDropDown.BackColor); }
            set { _TextBoxDropDown.BackColor = value; }
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
            get { return (_TextBoxDropDown.BackgroundStyle); }
        }

        /// <summary>
        /// Resets style to default value. Used by windows forms designer.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetBackgroundStyle()
        {
            _TextBoxDropDown.ResetBackgroundStyle();

            _TextBoxDropDown.BackgroundStyle.Class = "TextBoxBorder";
            _TextBoxDropDown.BackgroundStyle.CornerType = eCornerType.Square;
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
            get { return (_TextBoxDropDown.ButtonClear); }
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
            get { return (_TextBoxDropDown.ButtonCustom); }
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
            get { return (_TextBoxDropDown.ButtonCustom2); }
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
            get { return (_TextBoxDropDown.ButtonDropDown); }
        }

        #endregion

        #region CharacterCasing

        /// <summary>
        /// Gets or sets whether the TextBox control
        /// modifies the case of characters as they are typed.
        /// <value>One of the CharacterCasing enumeration values that specifies
        /// whether the TextBox control modifies the case of characters.
        /// The default is CharacterCasing.Normal.</value>
        /// </summary>
        [Browsable(true), Category("Behavior"), DefaultValue(0)]
        [Description("Indicates whether the TextBox control modifies the case of characters as they are typed.")]
        public CharacterCasing CharacterCasing
        {
            get { return (_TextBoxDropDown.CharacterCasing); }
            set { _TextBoxDropDown.CharacterCasing = value; }
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
                    _TextBoxDropDown.Invalidate();
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
            get { return (_TextBoxDropDown.DropDownControl); }
            set { _TextBoxDropDown.DropDownControl = value; }
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
            get { return (_TextBoxDropDown.Enabled); }
            set { _TextBoxDropDown.Enabled = value; }
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
            get { return (_TextBoxDropDown.FocusHighlightColor); }
            set { _TextBoxDropDown.FocusHighlightColor = value; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeFocusHighlightColor()
        {
            return (_TextBoxDropDown.ShouldSerializeFocusHighlightColor());
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetFocusHighlightColor()
        {
            _TextBoxDropDown.ResetFocusHighlightColor();
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
            get { return (_TextBoxDropDown.FocusHighlightEnabled); }
            set { _TextBoxDropDown.FocusHighlightEnabled = value; }
        }

        #endregion

        #region ForeColor

        /// <summary>
        /// Gets or sets the foreground color.
        /// </summary>
        [Browsable(false)]
        public Color ForeColor
        {
            get { return (_TextBoxDropDown.ForeColor); }
            set { _TextBoxDropDown.ForeColor = value; }
        }

        #endregion

        #region HideSelection

        /// <summary>
        /// Gets or sets a value indicating whether the selected text in
        /// the text box control remains highlighted when the control loses focus.
        /// <value>true if the selected text does not appear highlighted when the
        /// text box control loses focus; false, if the selected text remains
        /// highlighted when the text box control loses focus. The default is true.</value>
        /// </summary>
        [Browsable(true), Category("Behavior"), DefaultValue(true)]
        [Description("Gets or sets a value indicating whether the selected text in the text box control remains highlighted when the control loses focus.")]
        public bool HideSelection
        {
            get { return (_TextBoxDropDown.HideSelection); }
            set { _TextBoxDropDown.HideSelection = value; }
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
            get { return (_TextBoxDropDown.ImeMode); }
            set { _TextBoxDropDown.ImeMode = value; }
        }

        #endregion

        #region MaxLength

        /// <summary>
        /// Gets or sets the maximum number of characters the user can type or paste into the text box control.
        /// <value>The number of characters that can be entered into the control. The default is 32767.</value>
        /// </summary>
        [Category("Behavior"), DefaultValue(0x7fff), Localizable(true)]
        [Description("Gets or sets the maximum number of characters the user can type or paste into the text box control.")]
        public int MaxLength
        {
            get { return (_TextBoxDropDown.MaxLength); }
            set { _TextBoxDropDown.MaxLength = value; }
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
            get { return (_TextBoxDropDown.PasswordChar); }
            set { _TextBoxDropDown.PasswordChar = value; }
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
            get { return (_TextBoxDropDown.RightToLeft); }
            set { _TextBoxDropDown.RightToLeft = value; }
        }

        #endregion

        #region SelectedText

        /// <summary>
        /// Gets or sets a value indicating the currently selected text in the control.
        /// <value>A string that represents the currently selected text in the text box.</value>
        /// </summary>
        [Browsable(false), Category("Appearance"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Gets or sets a value indicating the currently selected text in the control.")]
        public virtual string SelectedText
        {
            get { return (_TextBoxDropDown.SelectedText); }
            set { _TextBoxDropDown.SelectedText = value; }
        }

        #endregion

        #region SelectionLength

        /// <summary>
        /// Gets or sets the number of characters selected in the text box.
        /// <value>The number of characters selected in the text box.</value>
        /// </summary>
        [Browsable(false), Category("Appearance")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Gets or sets the number of characters selected in the text box.")]
        public int SelectionLength
        {
            get { return (_TextBoxDropDown.SelectionLength); }
            set { _TextBoxDropDown.SelectionLength = value; }
        }

        #endregion

        #region SelectionStart

        /// <summary>
        /// Gets or sets the starting point of text selected in the text box.
        /// <value>The starting position of text selected in the text box.</value>
        /// </summary>
        [Browsable(false), Category("Appearance")]
        [Description("Gets or sets the starting point of text selected in the text box.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int SelectionStart
        {
            get { return (_TextBoxDropDown.SelectionStart); }
            set { _TextBoxDropDown.SelectionStart = value; }
        }

        #endregion

        #region Text

        /// <summary>
        /// Gets or sets the text as it is currently displayed to the user.
        /// </summary>
        [Browsable(false)]
        public string Text
        {
            get { return (_TextBoxDropDown.Text); }
            set { _TextBoxDropDown.Text = value; }
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
            get { return (_TextBoxDropDown.TextAlign); }
            set { _TextBoxDropDown.TextAlign = value; }
        }

        #endregion

        #region TextLength

        /// <summary>
        /// Gets the length of text in the control.
        /// Returns number of characters contained in the text of the control.
        /// </summary>
        [Browsable(false)]
        public virtual int TextLength
        {
            get { return (_TextBoxDropDown.TextLength); }
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
            get { return (_TextBoxDropDown.UseSystemPasswordChar); }
            set { _TextBoxDropDown.UseSystemPasswordChar = value; }
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
            get { return (_TextBoxDropDown.WatermarkBehavior); }
            set { _TextBoxDropDown.WatermarkBehavior = value; }
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
            get { return (_TextBoxDropDown.WatermarkColor); }
            set { _TextBoxDropDown.WatermarkColor = value; }
        }
        /// <summary>
        /// Indicates whether property should be serialized by Windows Forms designer.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeWatermarkColor()
        {
            return (_TextBoxDropDown.ShouldSerializeWatermarkColor());
        }
        /// <summary>
        /// Resets the property to default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetWatermarkColor()
        {
            _TextBoxDropDown.ResetWatermarkColor();
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
            get { return (_TextBoxDropDown.WatermarkEnabled); }
            set { _TextBoxDropDown.WatermarkEnabled = value; }
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
            get { return (_TextBoxDropDown.WatermarkFont); }
            set { _TextBoxDropDown.WatermarkFont = value; }
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
        [Editor("DevComponents.DotNetBar.Design.TextMarkupUIEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
        public string WatermarkText
        {
            get { return (_TextBoxDropDown.WatermarkText); }
            set { _TextBoxDropDown.WatermarkText = value; }
        }

        #endregion

        #endregion

        #region Method forwarding

        #region AppendText

        /// <summary>
        /// Appends text to the current text of a text box.
        /// </summary>
        /// <param name="text">The text to append to the current contents of the text box. </param>
        public void AppendText(string text)
        {
            _TextBoxDropDown.AppendText(text);
        }

        #endregion

        #region Clear

        /// <summary>
        /// Clears all text from the text box control.
        /// </summary>
        public void Clear()
        {
            _TextBoxDropDown.Clear();
        }

        #endregion

        #region ClearUndo

        /// <summary>
        /// Clears information about the most recent operation from the undo buffer of the text box.
        /// </summary>
        public void ClearUndo()
        {
            _TextBoxDropDown.ClearUndo();
        }

        #endregion

        #region Copy

        /// <summary>
        /// Copies the current selection in the text box to the Clipboard.
        /// </summary>
        [UIPermission(SecurityAction.Demand, Clipboard = UIPermissionClipboard.OwnClipboard)]
        public void Copy()
        {
            _TextBoxDropDown.Copy();
        }

        #endregion

        #region Cut

        /// <summary>
        /// Moves the current selection in the text box to the Clipboard.
        /// </summary>
        public void Cut()
        {
            _TextBoxDropDown.Cut();
        }

        #endregion

        #region DeselectAll

        /// <summary>
        /// Specifies that the value of the SelectionLength
        /// property is zero so that no characters are selected in the control.
        /// </summary>
        public void DeselectAll()
        {
            _TextBoxDropDown.DeselectAll();
        }

        #endregion

        #region GetCharFromPosition

        /// <summary>
        /// Retrieves the character that is closest to the specified location within the control.
        /// </summary>
        /// <param name="pt">The location from which to seek the nearest character. </param>
        /// <returns>The character at the specified location.</returns>
        public char GetCharFromPosition(Point pt)
        {
            return (_TextBoxDropDown.GetCharFromPosition(pt));
        }

        #endregion

        #region GetCharIndexFromPosition

        /// <summary>
        /// Retrieves the index of the character nearest to the specified location.
        /// </summary>
        /// <param name="pt">The location to search.</param>
        /// <returns>The zero-based character index at the specified location.</returns>
        public int GetCharIndexFromPosition(Point pt)
        {
            return (_TextBoxDropDown.GetCharIndexFromPosition(pt));
        }

        #endregion

        #region GetFirstCharIndexFromLine

        /// <summary>
        /// Retrieves the index of the first character of a given line.
        /// </summary>
        /// <param name="lineNumber">The line for which to get the index of its first character. </param>
        /// <returns>The zero-based character index in the specified line.</returns>
        public int GetFirstCharIndexFromLine(int lineNumber)
        {
            return (_TextBoxDropDown.GetFirstCharIndexFromLine(lineNumber));
        }

        #endregion

        #region GetFirstCharIndexOfCurrentLine

        /// <summary>
        /// Retrieves the index of the first character of the current line.
        /// </summary>
        /// <returns>The zero-based character index in the current line.</returns>
        public int GetFirstCharIndexOfCurrentLine()
        {
            return (_TextBoxDropDown.GetFirstCharIndexOfCurrentLine());
        }

        #endregion

        #region GetLineFromCharIndex

        /// <summary>
        /// Retrieves the line number from the specified character position within the text of the control.
        /// </summary>
        /// <param name="index">The character index position to search. </param>
        /// <returns>The zero-based line number in which the character index is located.</returns>
        public int GetLineFromCharIndex(int index)
        {
            return (_TextBoxDropDown.GetLineFromCharIndex(index));
        }

        /// <summary>
        /// Retrieves the location within the control at the specified character index.
        /// </summary>
        /// <param name="index">The index of the character for which to retrieve the location. </param>
        /// <returns>The location of the specified character.</returns>
        public virtual Point GetPositionFromCharIndex(int index)
        {
            return _TextBoxDropDown.GetPositionFromCharIndex(index);
        }

        /// <summary>
        /// Replaces the current selection in the text box with the contents of the Clipboard.
        /// </summary>
        [UIPermission(SecurityAction.Demand, Clipboard = UIPermissionClipboard.OwnClipboard)]
        public void Paste()
        {
            _TextBoxDropDown.Paste();
        }

        /// <summary>
        /// Selects a range of text in the text box.
        /// </summary>
        /// <param name="start">The position of the first character in the current text selection within the text box. </param>
        /// <param name="length">The number of characters to select. </param>
        public void Select(int start, int length)
        {
            _TextBoxDropDown.Select(start, length);
        }

        /// <summary>
        /// Selects all text in the text box.
        /// </summary>
        public void SelectAll()
        {
            _TextBoxDropDown.SelectAll();
        }

        /// <summary>
        /// Undoes the last edit operation in the text box.
        /// </summary>
        public void Undo()
        {
            _TextBoxDropDown.Undo();
        }

        /// <summary>
        /// Replaces the specified selection in the TextBox with the contents of the Clipboard.
        /// </summary>
        /// <param name="text">The text to replace.</param>
        public void Paste(string text)
        {
            _TextBoxDropDown.Paste(text);
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
            DataGridViewTextBoxDropDownColumn dc = base.Clone() as DataGridViewTextBoxDropDownColumn;

            if (dc != null)
            {
                dc.AutoCompleteCustomSource = AutoCompleteCustomSource;
                dc.AutoCompleteMode = AutoCompleteMode;
                dc.AutoCompleteSource = AutoCompleteSource;
                dc.BackColor = BackColor;
                dc.CharacterCasing = CharacterCasing;
                dc.DropDownControl = DropDownControl;
                dc.Enabled = Enabled;
                dc.FocusHighlightColor = FocusHighlightColor;
                dc.FocusHighlightEnabled = FocusHighlightEnabled;
                dc.ForeColor = ForeColor;
                dc.HideSelection = HideSelection;
                dc.ImeMode = ImeMode;
                dc.MaxLength = MaxLength;
                dc.PasswordChar = PasswordChar;
                dc.RightToLeft = RightToLeft;
                dc.TextAlign = TextAlign;
                dc.UseSystemPasswordChar = UseSystemPasswordChar;
                dc.WatermarkBehavior = WatermarkBehavior;
                dc.WatermarkColor = WatermarkColor;
                dc.WatermarkEnabled = WatermarkEnabled;
                dc.WatermarkFont = WatermarkFont;
                dc.WatermarkText = WatermarkText;

                dc.DisplayControlForCurrentCellOnly = DisplayControlForCurrentCellOnly;

                dc.BackgroundStyle.ApplyStyle(TextBoxDropDown.BackgroundStyle);
                dc.BackgroundStyle.Class = TextBoxDropDown.BackgroundStyle.Class;

                TextBoxDropDown.ButtonClear.CopyToItem(dc.TextBoxDropDown.ButtonClear);
                TextBoxDropDown.ButtonDropDown.CopyToItem(dc.TextBoxDropDown.ButtonDropDown);
                TextBoxDropDown.ButtonCustom.CopyToItem(dc.TextBoxDropDown.ButtonCustom);
                TextBoxDropDown.ButtonCustom2.CopyToItem(dc.TextBoxDropDown.ButtonCustom2);
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
