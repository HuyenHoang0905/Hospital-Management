#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Globalization;
using System.Collections;
using System.Net;
using DevComponents.DotNetBar;
using System.Drawing;
using System.Windows.Forms;

namespace DevComponents.Editors.DateTimeAdv
{
    [ToolboxBitmap(typeof(DotNetBarManager), "DateTimeInput.ico"), ToolboxItem(true), Designer("DevComponents.DotNetBar.Design.DateTimeInputDesigner, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
    public class DateTimeInput : VisualControlBase, ICommandSource
    {
        #region Private Variables
        private DateTimeGroup _DateInputGroup = null;
        private ButtonItem _PopupItem = null;
        private MonthCalendarItem _MonthCalendar = null;
        #endregion

        #region Events
        /// <summary>
        /// Occurs when the Value or IsEmpty property changes.
        /// <remarks>
        /// This event is not raised when the entered date is earlier than MinDateTime or later than MaxDateTime.
        /// </remarks>
        /// </summary>
        public event EventHandler ValueChanged;
        /// <summary>
        /// Occurs when the Value or IsEmpty property changes. This event occurs at the same time and has same function as ValueChanged event. It is provided for binding support.
        /// </summary>
        public event EventHandler ValueObjectChanged;
        /// <summary>
        /// Occurs when the Format property value has changed. 
        /// </summary>
        public event EventHandler FormatChanged;
        /// <summary>
        /// Occurs when Clear button is clicked and allows you to cancel the default action performed by the button.
        /// </summary>
        public event CancelEventHandler ButtonClearClick;
        /// <summary>
        /// Occurs when Drop-Down button that shows calendar is clicked and allows you to cancel showing of the popup.
        /// </summary>
        public event CancelEventHandler ButtonDropDownClick;
        /// <summary>
        /// Occurs when ValueObject property is set and it allows you to provide custom parsing for the values.
        /// </summary>
        public event ParseDateTimeValueEventHandler ParseValue;
        /// <summary>
        /// Occurs when ShowCheckBox property is set to true and user changes the lock status of the control by clicking the check-box.
        /// </summary>
        public event EventHandler LockUpdateChanged;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the DateTimeInput class.
        /// </summary>
        public DateTimeInput()
        {
        }
        #endregion

        #region Internal Implementation
        /// <summary>
        /// Copies the current value in the control to the Clipboard.
        /// </summary>
        public virtual void Copy()
        {
            if (_DateInputGroup != null) Clipboard.SetText(this.Text);
        }
        /// <summary>
        /// Pastes the current Clipboard content if possible as the value into the control.
        /// </summary>
        public virtual void Paste()
        {
            if (_DateInputGroup != null) Text = Clipboard.GetText();
        }
        /// <summary>
        /// Moves the current control value to the Clipboard.
        /// </summary>
        public virtual void Cut()
        {
            if (_DateInputGroup != null)
            {
                Copy();
                if(this.AllowEmptyState)
                    ValueObject = null;
            }
        }

        protected override PopupItem CreatePopupItem()
        {
            ButtonItem button = new ButtonItem("sysPopupProvider");
            button.PopupClose += new EventHandler(DropDownPopupClose);
            MonthCalendarItem mc = new MonthCalendarItem();
            mc.BackgroundStyle.BackColor = SystemColors.Window;
            mc.DateChanged += new EventHandler(PopupSelectedDateChanged);
            button.SubItems.Add(mc);
            _PopupItem = button;
            _MonthCalendar = mc;
            return button;
        }

        /// <summary>
        /// Gets whether popup calendar is open.
        /// </summary>
        [Browsable(false)]
        public bool IsPopupCalendarOpen
        {
            get
            {
                return _PopupItem.Expanded;
            }
            set
            {
                _PopupItem.Expanded = value;
            }
        }


        protected override bool OnKeyDown(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
        {
            if (IsPopupCalendarOpen)
            {
                int days = 0;
                if (wParam.ToInt32() == 37) // Left Arrow
                    days = -1;
                else if (wParam.ToInt32() == 39) // Right Arrow
                    days = 1;
                else if (wParam.ToInt32() == 40) // Down Arrow
                    days = 7;
                else if (wParam.ToInt32() == 38) // Up Arrow
                    days = -7;
                else if (wParam.ToInt32() == 13 && _MonthCalendar.SelectedDate != DateTime.MinValue) // Enter
                {
                    ToggleCalendarPopup();
                    return true;
                }

                if (days != 0)
                {
                    if (_MonthCalendar.SelectedDate == DateTime.MinValue)
                        _MonthCalendar.SelectedDate = DateTime.Today;
                    else
                        _MonthCalendar.SelectedDate = _MonthCalendar.SelectedDate.AddDays(days);
                    if (_MonthCalendar.DisplayMonth.Month != _MonthCalendar.SelectedDate.Month)
                        _MonthCalendar.DisplayMonth = _MonthCalendar.SelectedDate;
                    return true;
                }
            }
            return base.OnKeyDown(hWnd, wParam, lParam);
        }

        private void PopupSelectedDateChanged(object sender, EventArgs e)
        {
            if (_MonthCalendar.SelectedDate == DateTime.MinValue)
                this.IsEmpty = true;
            else
            {
                DateTime date = _MonthCalendar.SelectedDate;
                DateTime value = this.Value;
                if (value != DateTime.MinValue && value != DateTime.MaxValue)
                    date = new DateTime(date.Year, date.Month, date.Day, value.Hour, value.Minute, value.Second);
                if (this.MinDate != DateTime.MinValue && date < this.MinDate) date = this.MinDate;
                _DateInputGroup.Value = date;
            }
        }

        protected override VisualItem CreateRootVisual()
        {
            _ButtonClear = new InputButtonSettings(this);
            _ButtonDropDown = new InputButtonSettings(this);
            _ButtonFreeText = new InputButtonSettings(this);

            _DateInputGroup = new DateTimeGroup();
            _DateInputGroup.IsRootVisual = true;
            _DateInputGroup.ValueChanged += new EventHandler(InputGroup_ValueChanged);
            FormatToDateTimeGroup();
            return _DateInputGroup;
        }

        private void InputGroup_ValueChanged(object sender, EventArgs e)
        {
            OnValueChanged(e);
        }

        protected override bool IsWatermarkRendered
        {
            get
            {
                return !(this.Focused || _FreeTextEntryBox != null && _FreeTextEntryBox.Focused) && _DateInputGroup.IsEmpty;
            }
        }

        private bool _ShowCheckBox = false;
        /// <summary>
        /// Gets or sets a value indicating whether a check box is displayed to the left of the selected date.
        /// Set to true if a check box is displayed to the left of the selected date; otherwise, false. The default is false.
        /// <remarks>
        /// When the ShowCheckBox property is set to true, a check box is displayed to the left of the date in the control. When the check box is selected, the date/time value can be updated. When the check box is cleared, the date/time value is unable to be changed.
        /// You can handle the LockUpdateChanged event to be notified when this check box is checked and unchecked. Use LockUpdateChecked property 
        /// to get or sets whether check box is checked.
        /// </remarks>
        /// </summary>
        [DefaultValue(false), Description("Indicates whether a check box is displayed to the left of the input value which allows locking of the control.")]
        public bool ShowCheckBox
        {
            get { return _ShowCheckBox; }
            set
            {
                _ShowCheckBox = value;
                OnFormatChanged();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether a spin button control (up-down control) is used to adjust the date/time value. The default is false. 
        /// <remarks>
        /// When the ShowUpDown property is set to true, a spin button control is shown to adjust value of currently focused input item. 
        /// The date and time can be adjusted by selecting each element individually and using the up and down buttons to change the value.
        /// </remarks>
        /// </summary>
        private bool _ShowUpDown = false;
        [DefaultValue(false)]
        public bool ShowUpDown
        {
            get { return _ShowUpDown; }
            set
            {
                _ShowUpDown = value;
                OnFormatChanged();
            }
        }

        /// <summary>
        /// Gets or sets the date time value of the control. You can use IsEmpty property to check whether control holds an empty value.
        /// Setting this property to System.DateTime(0) will also make the control Empty if AllowEmptyState=true.
        /// </summary>
        [Description("Indicates date time value of the control"), Bindable(BindableSupport.Yes)]
        public System.DateTime Value
        {
            get { return _DateInputGroup.Value; }
            set 
            {
                if (this.AllowEmptyState && value.Equals(new System.DateTime(((long)(0)))))
                    this.IsEmpty = true;
                else
                    _DateInputGroup.Value = value; 
            }
        }
        /// <summary>
        /// Gets whether Value property should be serialized by Windows Forms designer.
        /// </summary>
        /// <returns>true if value serialized otherwise false.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeValue()
        {
            if (!this.AllowEmptyState && this.Value.Date == DateTime.Today) return false;

            return !_DateInputGroup.IsEmpty;
        }
        /// <summary>
        /// Resets Value property to default value. Used by Windows Forms designer.
        /// </summary>
        public void ResetValue()
        {
            if (this.AllowEmptyState)
                this.IsEmpty = true;
            else
                this.Value = DateTime.Now;
        }

        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                this.ValueObject = value;
            }
        }

        /// <summary>
        /// Gets or sets the date/time value of the control as an object. This property allows you to bind to the database fields and supports
        /// null values. Expected value is DateTime object or null to indicate no date selected.
        /// </summary>
        [Bindable(true), RefreshProperties(RefreshProperties.All), TypeConverter(typeof(DateTimeConverter)), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object ValueObject
        {
            get
            {
                if (this.IsEmpty)
                    return null;
                return Value;
            }
            set
            {
                if (AcceptCustomValueObject(value))
                    return;
                else if (IsNull(value) || value is string && value=="")
                    this.IsEmpty = true;
                else if (value is System.DateTime)
                {
                    this.Value = (System.DateTime)value;
                }
                else if (value is string)
                {
                    System.DateTime d = new System.DateTime();
                    if (System.DateTime.TryParse(value.ToString(), out d))
                        this.Value = d;
                    else
                        throw new ArgumentException("ValueObject property expects either null/nothing value or DateTime type.");
                }
                else
                    throw new ArgumentException("ValueObject property expects either null/nothing value or DateTime type.");
            }
        }

        private bool AcceptCustomValueObject(object value)
        {
            ParseDateTimeValueEventArgs e = new ParseDateTimeValueEventArgs(value);
            OnParseValue(e);
            if (e.IsParsed)
            {
                this.Value = e.ParsedValue;
            }

            return e.IsParsed;
        }

        /// <summary>
        /// Raises the ParseValue event.
        /// </summary>
        /// <param name="e">Provides event arguments.</param>
        protected virtual void OnParseValue(ParseDateTimeValueEventArgs e)
        {
            if (ParseValue != null)
                ParseValue(this, e);
        }

        /// <summary>
        /// Gets or sets the values of the nested DateTimeGroup items.
        /// <remarks>
        /// When nested date-time groups are used note that some of the features of the control are disabled, notably minimum and maximum values
        /// for nested date-times.
        /// </remarks>
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public System.DateTime[] Values
        {
            get { return _DateInputGroup.Values; }
            set
            {
                _DateInputGroup.Values = value;
            }
        }

        /// <summary>
        /// Gets or sets whether empty null/nothing state of the control is allowed. Default value is true which means that IsEmpty property
        /// may return true if input value is resets or ValueObject set to null/nothing.
        /// </summary>
        [DefaultValue(true), Description("Indicates whether empty null/nothing state of the control is allowed.")]
        public bool AllowEmptyState
        {
            get { return _DateInputGroup.AllowEmptyState; }
            set { _DateInputGroup.AllowEmptyState = value; this.Invalidate(); }
        }

        /// <summary>
        /// Gets or sets whether control is empty i.e. it does not hold a valid DateTime value.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsEmpty
        {
            get { return _DateInputGroup.IsEmpty; }
            set { _DateInputGroup.IsEmpty = value; }
        }

        /// <summary>
        /// Gets or sets the minimum date and time that can be selected in the control.
        /// </summary>
        [Description("Indicates minimum date and time that can be selected in the control.")]
        public System.DateTime MinDate
        {
            get { return _DateInputGroup.MinDate; }
            set
            {
                _DateInputGroup.MinDate = value;
            }
        }
        /// <summary>
        /// Gets whether Value property should be serialized by Windows Forms designer.
        /// </summary>
        /// <returns>true if value serialized otherwise false.</returns>
        public bool ShouldSerializeMinDate()
        {
            return !MinDate.Equals(DateTimeGroup.MinDateTime);
        }
        /// <summary>
        /// Reset the MinDate property to its default value.
        /// </summary>
        public void ResetMinDate()
        {
            MinDate = DateTimeGroup.MinDateTime;
        }

        /// <summary>
        /// Gets or sets the maximum date and time that can be selected in the control.
        /// </summary>
        [Description("Indicates maximum date and time that can be selected in the control.")]
        public System.DateTime MaxDate
        {
            get { return _DateInputGroup.MaxDate; }
            set { _DateInputGroup.MaxDate = value; }
        }
        /// <summary>
        /// Gets whether Value property should be serialized by Windows Forms designer.
        /// </summary>
        /// <returns>true if value serialized otherwise false.</returns>
        public bool ShouldSerializeMaxDate()
        {
            return !_DateInputGroup.MaxDate.Equals(DateTimeGroup.MaxDateTime);
        }
        /// <summary>
        /// Reset the MaxDate property to its default value.
        /// </summary>
        public void ResetMaxDate()
        {
            MaxDate = DateTimeGroup.MaxDateTime;
        }

        private eDateTimePickerFormat _Format = eDateTimePickerFormat.Short;
        /// <summary>
        /// Gets or sets the format date/time is displayed in. To specify custom format set this value to Custom and specify custom format using CustomFormat property.
        /// </summary>
        [DefaultValue(eDateTimePickerFormat.Short)]
        public eDateTimePickerFormat Format
        {
            get { return _Format; }
            set
            {
                if (_Format != value)
                {
                    _Format = value;
                    OnFormatChanged();
                }
            }
        }

        private string _CustomFormat = "";
        /// <summary>
        /// Gets or sets the custom date/time format string. 
        /// </summary>
        /// <remarks>
        /// <para>
        /// To display string literals that contain date and time separators or format strings listed below, 
        /// you must use escape characters in the substring. For example, to display the date as "June 15 at 12:00 PM", 
        /// set the CustomFormat property to "MMMM dd 'at' t:mm tt". If the "at" substring is not enclosed by escape characters,
        /// the result is "June 15 aP 12:00PM" because the "t" character is read as the one-letter A.M./P.M. format string (see the format string table below).
        /// </para>
        /// <para>
        /// To display single quote in custom format use two single quotes characters like so '' after each other and they will be displayed as single quote.
        /// </para>
        /// The following list shows all the valid format strings and their descriptions:
        /// <list type="table">
        /// <listheader>
        /// <term>Format String</term>
        /// <description>Description</description>
        /// </listheader>
        /// <item>
        /// <term>d</term>
        /// <description>The one- or two-digit day.</description>
        /// </item>
        /// <item>
        /// <term>dd</term>
        /// <description>The two-digit day. Single-digit day values are preceded by a 0. </description>
        /// </item>
        /// <item>
        /// <term>ddd</term>
        /// <description>The three-character day-of-week abbreviation.</description>
        /// </item>
        /// <item>
        /// <term>dddd</term>
        /// <description>The full day-of-week name.</description>
        /// </item>
        /// <item>
        /// <term>jjj</term>
        /// <description>The three-digit day-of-year day. Single and two-digit values are preceded by 0.</description>
        /// </item>
        /// <item>
        /// <term>j</term>
        /// <description>The three-digit day-of-year day.</description>
        /// </item>
        /// <item>
        /// <term>h</term>
        /// <description>The one- or two-digit hour in 12-hour format.</description>
        /// </item>
        /// <item>
        /// <term>hh</term>
        /// <description>The two-digit hour in 12-hour format. Single digit values are preceded by a 0. </description>
        /// </item>
        /// <item>
        /// <term>H</term>
        /// <description>The one- or two-digit hour in 24-hour format. </description>
        /// </item>
        /// <item>
        /// <term>HH</term>
        /// <description>The two-digit hour in 24-hour format. Single digit values are preceded by a 0.</description>
        /// </item>
        /// <item>
        /// <term>m</term>
        /// <description>The one- or two-digit minute.</description>
        /// </item>
        /// <item>
        /// <term>mm</term>
        /// <description>The two-digit minute. Single digit values are preceded by a 0.</description>
        /// </item>
        /// <item>
        /// <term>M</term>
        /// <description>The one- or two-digit month number.</description>
        /// </item>
        /// <item>
        /// <term>MM</term>
        /// <description>The two-digit month number. Single digit values are preceded by a 0.</description>
        /// </item>
        /// <item>
        /// <term>MMM</term>
        /// <description>The three-character month abbreviation.</description>
        /// </item>
        /// <item>
        /// <term>MMMM</term>
        /// <description>The full month name.</description>
        /// </item>
        /// <item>
        /// <term>s</term>
        /// <description>The one- or two-digit seconds.</description>
        /// </item>
        /// <item>
        /// <term>ss</term>
        /// <description>The two-digit seconds. Single digit values are preceded by a 0.</description>
        /// </item>
        /// <item>
        /// <term>t</term>
        /// <description>The one-letter A.M./P.M. abbreviation (A.M. is displayed as "A").</description>
        /// </item>
        /// <item>
        /// <term>tt</term>
        /// <description>The two-letter A.M./P.M. abbreviation (A.M. is displayed as "AM").</description>
        /// </item>
        /// <item>
        /// <term>y</term>
        /// <description>The one-digit year (2001 is displayed as "1").</description>
        /// </item>
        /// <item>
        /// <term>yy</term>
        /// <description>The last two digits of the year (2001 is displayed as "01").</description>
        /// </item>
        /// <item>
        /// <term>yyyy</term>
        /// <description>The full year (2001 is displayed as "2001").</description>
        /// </item>
        /// <item>
        /// <term>{</term>
        /// <description>Starts the nested date-time group inside of the control. Note that nested groups must always be closed. 
        /// Nested date-time groups can be used to represent range of input date/time values in the control
        /// To access nested values use Values property. For example to have control represent the input from two time values you could set
        /// CustomFormat to 'from' {HH:mm} 'to' {HH:mm} which will create two nested date/time groups that represent the time value. Entered
        /// time values can be accessed through Values property which return an array of all input values.</description>
        /// </item>
        /// <item>
        /// <term>}</term>
        /// <description>Ends the nested date-time input group.</description>
        /// </item>
        /// </list>
        /// </remarks>
        [DefaultValue(""), Description("Indicates the custom date/time format string. "), Localizable(true)]
        public string CustomFormat
        {
            get { return _CustomFormat; }
            set
            {
                if (value == null) value = "";
                if (value != _CustomFormat)
                {
                    _CustomFormat = value;
                    OnFormatChanged();
                }
            }
        }

        private void OnFormatChanged()
        {
            FormatToDateTimeGroup();
            Invalidate();
            if (FormatChanged != null)
                FormatChanged(this, new EventArgs());
        }

        private void FormatToDateTimeGroup()
        {
            if (_CustomFormat.Length > 0 && _Format == eDateTimePickerFormat.Custom)
                ParseFormat(this.CustomFormat);
            else if (_Format != eDateTimePickerFormat.Custom)
                ParseFormat(GetSystemFormatString(_Format));
            else
                _DateInputGroup.Items.Clear();
        }

        private string GetSystemFormatString(eDateTimePickerFormat format)
        {
            if (format == eDateTimePickerFormat.Long)
                return DateTimeInput.GetActiveCulture().DateTimeFormat.LongDatePattern;
            else if (format == eDateTimePickerFormat.Short)
                return DateTimeInput.GetActiveCulture().DateTimeFormat.ShortDatePattern;
            else if (format == eDateTimePickerFormat.ShortTime)
                return DateTimeInput.GetActiveCulture().DateTimeFormat.ShortTimePattern;
            else if (format == eDateTimePickerFormat.LongTime)
                return DateTimeInput.GetActiveCulture().DateTimeFormat.LongTimePattern;

            return "";
        }

        private static CultureInfo _CurrentCulture = null;
        /// <summary>
        /// Gets or sets the CultureInfo for the culture used by the DateTime Input controls and Month Calendar controls.
        /// Default value is null which indicates that controls will use CurrentUICulture.
        /// </summary>
        public static CultureInfo CurrentCulture
        {
            get
            {
                return _CurrentCulture;
            }
            set
            {
                _CurrentCulture = value;
            }
        }

        /// <summary>
        /// Gets the Culture used by the date time input and month calendar controls
        /// </summary>
        /// <returns>reference to CultureInfo</returns>
        public static CultureInfo GetActiveCulture()
        {
            return _CurrentCulture != null ? _CurrentCulture : CultureInfo.CurrentCulture;
        }

        /// <summary>
        /// Gets or sets whether auto-overwrite functionality for input is enabled. When in auto-overwrite mode input field will erase existing entry
        /// and start new one if typing is continued after InputComplete method is called.
        /// </summary>
        [DefaultValue(true), Description("Indicates whether auto-overwrite functionality for input is enabled.")]
        public bool AutoOverwrite
        {
            get { return _DateInputGroup.AutoOverwrite; }
            set { _DateInputGroup.AutoOverwrite = value; }
        }

        private void ParseFormat(string format)
        {
            _DateInputGroup.Items.Clear();

            if (_ShowCheckBox)
            {
                LockUpdateCheckBox checkBox = new LockUpdateCheckBox();
                _DateInputGroup.Items.Add(checkBox);
                checkBox.CheckedChanged += new EventHandler(LockCheckedChanged);
            }

            if (format.Length == 0) return;

            StringBuilder inputStack = new StringBuilder(format.Length);
            bool quote = false;
            Stack<VisualGroup> groupStack = new Stack<VisualGroup>();
            VisualGroup currentGroup = _DateInputGroup;

            for (int i = 0; i < format.Length; i++)
            {
                if (format[i] == '\'') // Trigger/Exit quote mode
                {
                    if (quote)
                        quote = false;
                    else
                        quote = true;
                    continue;
                }

                if (quote) // Quote mode adds everything under the quotes
                {
                    inputStack.Append(format[i]);
                    continue;
                }

                string s = format.Substring(i, Math.Min(4, format.Length - i));
                bool match = false;
                if (s == "dddd") // The full day-of-week name or three-character day-of-week abbreviation.
                {
                    DayLabelItem dayLabel = new DayLabelItem();
                    currentGroup.Items.Add(dayLabel);
                    i += 3;
                    match = true;
                }
                else if (s == "MMMM") // Full month name
                {
                    MonthNameInput month = new MonthNameInput();
                    currentGroup.Items.Add(month);
                    i += 3;
                    match = true;
                }
                else if (s == "yyyy") // 4 digit year
                {
                    NumericYearInput year = new NumericYearInput();
                    currentGroup.Items.Add(year);
                    i += 3;
                    match = true;
                }

                if (!match)
                {
                    s = format.Substring(i, Math.Min(3, format.Length - i));
                    if (s == "ddd") // The three-character day-of-week abbreviation.
                    {
                        DayLabelItem dayLabel = new DayLabelItem();
                        dayLabel.UseAbbreviatedNames = true;
                        currentGroup.Items.Add(dayLabel);
                        i += 2;
                        match = true;
                    }
                    else if (s == "MMM") // The three-character month abbreviation.
                    {
                        MonthNameInput month = new MonthNameInput();
                        month.UseAbbreviatedNames = true;
                        currentGroup.Items.Add(month);
                        i += 2;
                        match = true;
                    }
                    else if (s == "jjj") // The three-digit day of year...
                    {
                        NumericDayOfYearInput day = new NumericDayOfYearInput();
                        day.DisplayFormat = "000";
                        currentGroup.Items.Add(day);
                        i += 2;
                        match = true;
                    }
                }

                if (!match)
                {
                    s = format.Substring(i, Math.Min(2, format.Length - i));
                    if (s == "dd") // The two-digit day. Single-digit day values are preceded by a 0.
                    {
                        NumericDayInput day = new NumericDayInput();
                        day.DisplayFormat = "00";
                        currentGroup.Items.Add(day);
                        i += 1;
                        match = true;
                    }
                    else if (s == "hh") // The two-digit hour in 12-hour format. Single digit values are preceded by a 0. 
                    {
                        NumericHourInput hour = new NumericHourInput();
                        hour.DisplayFormat = "00";
                        hour.Is24HourFormat = false;
                        currentGroup.Items.Add(hour);
                        i += 1;
                        match = true;
                    }
                    else if (s == "HH") // The two-digit hour in 24-hour format. Single digit values are preceded by a 0.
                    {
                        NumericHourInput hour = new NumericHourInput();
                        hour.DisplayFormat = "00";
                        hour.Is24HourFormat = true;
                        currentGroup.Items.Add(hour);
                        i += 1;
                        match = true;
                    }
                    else if (s == "mm") // The two-digit minute. Single digit values are preceded by a 0. 
                    {
                        NumericMinuteInput minute = new NumericMinuteInput();
                        minute.DisplayFormat = "00";
                        currentGroup.Items.Add(minute);
                        i += 1;
                        match = true;
                    }
                    else if (s == "MM") // The two-digit month number. Single digit values are preceded by a 0. 
                    {
                        NumericMonthInput month = new NumericMonthInput();
                        month.DisplayFormat = "00";
                        currentGroup.Items.Add(month);
                        i += 1;
                        match = true;
                    }
                    else if (s == "ss") // The two-digit seconds. Single digit values are preceded by a 0. 
                    {
                        NumericSecondInput second = new NumericSecondInput();
                        second.DisplayFormat = "00";
                        currentGroup.Items.Add(second);
                        i += 1;
                        match = true;
                    }
                    else if (s == "tt") // The two-letter A.M./P.M. abbreviation (A.M. is displayed as "AM").
                    {
                        HourPeriodInput period = new HourPeriodInput();
                        currentGroup.Items.Add(period);
                        i += 1;
                        match = true;
                    }
                    else if (s == "yy") // The last two digits of the year (2001 is displayed as "01"). 
                    {
                        NumericYearInput year = new NumericYearInput();
                        year.YearDisplayFormat = eYearDisplayFormat.TwoDigit;
                        currentGroup.Items.Add(year);
                        i += 1;
                        match = true;
                    }
                }

                if (!match)
                {
                    s = format[i].ToString();
                    if (s == "d") // The one- or two-digit day. 
                    {
                        NumericDayInput day = new NumericDayInput();
                        currentGroup.Items.Add(day);
                        match = true;
                    }
                    else if (s == "h") // The one- or two-digit hour in 12-hour format.
                    {
                        NumericHourInput hour = new NumericHourInput();
                        hour.Is24HourFormat = false;
                        currentGroup.Items.Add(hour);
                        match = true;
                    }
                    else if (s == "H") //The one- or two-digit hour in 24-hour format. 
                    {
                        NumericHourInput hour = new NumericHourInput();
                        hour.Is24HourFormat = true;
                        currentGroup.Items.Add(hour);
                        match = true;
                    }
                    else if (s == "m") // The one- or two-digit minute. 
                    {
                        NumericMinuteInput minute = new NumericMinuteInput();
                        currentGroup.Items.Add(minute);
                        match = true;
                    }
                    else if (s == "M") // The one- or two-digit month number. 
                    {
                        NumericMonthInput month = new NumericMonthInput();
                        currentGroup.Items.Add(month);
                        match = true;
                    }
                    else if (s == "s") // The one- or two-digit seconds. 
                    {
                        NumericSecondInput second = new NumericSecondInput();
                        currentGroup.Items.Add(second);
                        match = true;
                    }
                    else if (s == "t") // The one-letter A.M./P.M. abbreviation (A.M. is displayed as "A"). 
                    {
                        HourPeriodInput period = new HourPeriodInput();
                        period.UseSingleLetterLabel = true;
                        currentGroup.Items.Add(period);
                        match = true;
                    }
                    else if (s == "y") // The one-digit year (2001 is displayed as "1"). 
                    {
                        NumericYearInput year = new NumericYearInput();
                        year.YearDisplayFormat = eYearDisplayFormat.OneDigit;
                        currentGroup.Items.Add(year);
                        match = true;
                    }
                    else if (s == "j") // The three-digit day of year...
                    {
                        NumericDayOfYearInput day = new NumericDayOfYearInput();
                        currentGroup.Items.Add(day);
                        match = true;
                    }
                    else if (s == "{") // Begins the group of data entries
                    {
                        if (inputStack.Length > 0)
                        {
                            VisualLabel label = new VisualLabel();
                            label.Text = inputStack.ToString();
                            currentGroup.Items.Add(label);
                            inputStack = new StringBuilder(format.Length);
                        }
                        DateTimeGroup group = new DateTimeGroup();
                        currentGroup.Items.Add(group);
                        groupStack.Push(currentGroup);
                        currentGroup = group;
                        match = true;
                    }
                    else if (s == "}") // Ends the group of data entries
                    {
                        currentGroup = groupStack.Pop();
                        match = true;
                    }
                }

                if (match)
                {
                    if (inputStack.Length > 0)
                    {
                        VisualLabel label = new VisualLabel();
                        label.Text = inputStack.ToString();
                        currentGroup.Items.Insert(currentGroup.Items.Count - 1, label);
                        inputStack = new StringBuilder(format.Length);
                    }
                }
                else
                    inputStack.Append(format[i]);
            }

            if (inputStack.Length > 0)
            {
                VisualLabel label = new VisualLabel();
                label.Text = inputStack.ToString();
                currentGroup.Items.Add(label);
            }

            if (_ShowUpDown)
            {
                VisualUpDownButton upDownButton = new VisualUpDownButton();
                upDownButton.Alignment = eItemAlignment.Right;
                upDownButton.AutoChange = eUpDownButtonAutoChange.FocusedItem;
                _DateInputGroup.Items.Add(upDownButton);
            }

            RecreateButtons();

            if (_ShowCheckBox)
                this.LockUpdateCheckBox.Checked = _LockUpdateChecked;
        }

        private LockUpdateCheckBox LockUpdateCheckBox
        {
            get
            {
                if (_DateInputGroup.Items[0] is LockUpdateCheckBox)
                    return (LockUpdateCheckBox)_DateInputGroup.Items[0];
                return null;
            }
        }

        private void LockCheckedChanged(object sender, EventArgs e)
        {
            LockUpdateCheckBox checkBox = LockUpdateCheckBox;
            if (checkBox != null)
                _LockUpdateChecked = checkBox.Checked;

            OnLockUpdateChanged(e);
        }

        private bool _LockUpdateChecked = true;
        /// <summary>
        /// Gets or sets whether check box shown using ShowCheckBox property which locks/unlocks the control update is checked.
        /// </summary>
        [DefaultValue(true), Description("Indicates whether check box shown using ShowCheckBox property which locks/unlocks the control update is checked.")]
        public bool LockUpdateChecked
        {
            get { return _LockUpdateChecked; }
            set
            {
                if (_LockUpdateChecked != value)
                {
                    _LockUpdateChecked = value;
                    LockUpdateCheckBox checkBox = LockUpdateCheckBox;
                    if (checkBox != null)
                        checkBox.Checked = _LockUpdateChecked;
                }
            }
        }

        /// <summary>
        /// Raises the LockUpdateChanged event.
        /// </summary>
        /// <param name="e">Provides event data./</param>
        protected virtual void OnLockUpdateChanged(EventArgs e)
        {
            if (LockUpdateChanged != null)
                LockUpdateChanged(this, e);
        }

        /// <summary>
        /// Raises the ValueChanged event.
        /// </summary>
        /// <param name="e">Provides event arguments.</param>
        protected virtual void OnValueChanged(EventArgs e)
        {
            if (IsInitializing) return;

            if (ValueChanged != null)
                ValueChanged(this, e);
            if (ValueObjectChanged != null)
                ValueObjectChanged(this, e);
            if (this.IsEmpty)
            {
                base.Text = "";
            }
            else
            {
                string format = "";
                if (this.Format == eDateTimePickerFormat.Custom)
                    format = this.CustomFormat;
                else
                    format = GetSystemFormatString(this.Format);
                if (format == "")
                    format = GetSystemFormatString(eDateTimePickerFormat.Short);
                if (format == "")
                    base.Text = this.Value.ToString();
                else
                {
                    try
                    {
                        base.Text = this.Value.ToString(format);
                    }
                    catch
                    {
                        base.Text = "";
                    }
                }
            }
            if (FreeTextEntryMode && _FreeTextEntryBox != null)
                _FreeTextEntryBox.Text = this.Text;

            ExecuteCommand();
        }

        protected override void OnValidating(CancelEventArgs e)
        {
            _DateInputGroup.UpdateValue(false);
            base.OnValidating(e);
        }

        private InputButtonSettings _ButtonDropDown = null;
        /// <summary>
        /// Gets the object that describes the settings for the button that shows drop-down calendar when clicked.
        /// </summary>
        [Category("Buttons"), Description("Describes the settings for the button that shows drop-down calendar when clicked."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public InputButtonSettings ButtonDropDown
        {
            get
            {
                return _ButtonDropDown;
            }
        }

        private InputButtonSettings _ButtonClear = null;
        /// <summary>
        /// Gets the object that describes the settings for the button that clears the content of the control when clicked.
        /// </summary>
        [Category("Buttons"), Description("Describes the settings for the button that clears the content of the control when clicked."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public InputButtonSettings ButtonClear
        {
            get
            {
                return _ButtonClear;
            }
        }

        protected override System.Collections.SortedList CreateSortedButtonList()
        {
            SortedList list = base.CreateSortedButtonList();
            if (_ButtonClear.Visible)
            {
                VisualItem button = CreateButton(_ButtonClear);
                if (_ButtonClear.ItemReference != null)
                    _ButtonClear.ItemReference.Click -= new EventHandler(ClearButtonClick);
                _ButtonClear.ItemReference = button;
                button.Click += new EventHandler(ClearButtonClick);
                list.Add(_ButtonClear, button);
            }
            if (_ButtonDropDown.Visible)
            {
                VisualItem button = CreateButton(_ButtonDropDown);
                if (_ButtonDropDown.ItemReference != null)
                {
                    _ButtonDropDown.ItemReference.MouseDown -= new MouseEventHandler(DropDownButtonMouseDown);
                    _ButtonDropDown.ItemReference.Click -= new EventHandler(DropDownButtonClick);
                }
                _ButtonDropDown.ItemReference = button;
                button.MouseDown += new MouseEventHandler(DropDownButtonMouseDown);
                button.Click += new EventHandler(DropDownButtonClick);
                list.Add(_ButtonDropDown, button);
            }
            if (_ButtonFreeText.Visible)
            {
                VisualItem button = CreateButton(_ButtonFreeText);
                if (_ButtonFreeText.ItemReference != null)
                    _ButtonFreeText.ItemReference.Click -= new EventHandler(FreeTextButtonClick);
                _ButtonFreeText.ItemReference = button;
                button.Click += FreeTextButtonClick;
                list.Add(_ButtonFreeText, button);
            }

            return list;
        }

        private void DropDownButtonClick(object sender, EventArgs e)
        {
            if (e is KeyEventArgs)
            {
                ToggleCalendarPopup();
            }
        }

        private void DropDownButtonMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left || _CloseTime != DateTime.MinValue && DateTime.Now.Subtract(_CloseTime).TotalMilliseconds < 150)
            {
                _CloseTime = DateTime.MinValue;
                return;
            }

            ToggleCalendarPopup();
        }

        private void ToggleCalendarPopup()
        {
            CancelEventArgs cancelArgs = new CancelEventArgs();
            OnButtonDropDownClick(cancelArgs);
            if (cancelArgs.Cancel) return;

            _PopupItem.SetDisplayRectangle(this.ClientRectangle);

            _MonthCalendar.ReloadLocalizedStrings();

            // Check the day size in case larger font is applied to the control
            if (this.Font.Height > _MonthCalendar.DaySize.Height)
            {
                _MonthCalendar.DaySize = new Size((int)Math.Ceiling(this.Font.Height * 1.6f), this.Font.Height + 1);
            }

            if (this.RightToLeft == RightToLeft.No)
                _PopupItem.PopupLocation = new Point(this.Width - _PopupItem.PopupSize.Width, this.Height);

            if (this.MinDate != DateTime.MinValue)
                _MonthCalendar.MinDate = this.MinDate; //.AddDays(-(this.MinDate.Day - 1));
            else
                _MonthCalendar.MinDate = this.MinDate;

            _MonthCalendar.MaxDate = this.MaxDate;
            if (!this.IsEmpty)
                _MonthCalendar.SelectedDate = this.Value;
            else
                _MonthCalendar.SelectedDate = DateTime.MinValue;

            if (_MonthCalendar.SelectedDate != DateTime.MinValue)
                _MonthCalendar.DisplayMonth = _MonthCalendar.SelectedDate;
            else
                _MonthCalendar.DisplayMonth = DateTime.Today;
            _PopupItem.Expanded = !_PopupItem.Expanded;
        }
        private DateTime _CloseTime = DateTime.MinValue;
        private void DropDownPopupClose(object sender, EventArgs e)
        {
            _CloseTime = DateTime.Now;
        }

        //protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        //{
        //    if ((keyData & Keys.Down) == Keys.Down && Control.ModifierKeys == Keys.Alt)
        //    {
        //        if (_PopupItem != null && !_PopupItem.Expanded)
        //            ToggleCalendarPopup();

        //    }
        //    return base.ProcessCmdKey(ref msg, keyData);
        //}

        private void ClearButtonClick(object sender, EventArgs e)
        {
            CancelEventArgs cancelArgs = new CancelEventArgs();
            OnButtonClearClick(cancelArgs);
            if (cancelArgs.Cancel) return;
            this.IsEmpty = true;
        }

        private InputButtonSettings _ButtonFreeText = null;
        /// <summary>
        /// Gets the object that describes the settings for the button that switches the control into the free-text entry mode when clicked.
        /// </summary>
        [Category("Buttons"), Description("Describes the settings for the button that switches the control into the free-text entry mode when clicked."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public InputButtonSettings ButtonFreeText
        {
            get
            {
                return _ButtonFreeText;
            }
        }

        protected override VisualItem CreateButton(InputButtonSettings buttonSettings)
        {
            VisualItem item = null;
            if (buttonSettings == _ButtonDropDown)
            {
                item = new VisualDropDownButton();
                ApplyButtonSettings(buttonSettings, item as VisualButton);
            }
            else
                item = base.CreateButton(buttonSettings);
            VisualButton button = item as VisualButton;
            button.ClickAutoRepeat = false;

            if (buttonSettings == _ButtonClear)
            {
                if (buttonSettings.Image == null)
                    button.Image = DevComponents.DotNetBar.BarFunctions.LoadBitmap("SystemImages.DateReset.png");
            }
            else if (buttonSettings == _ButtonFreeText)
            {
                if (buttonSettings.Image == null)
                    button.Image = DevComponents.DotNetBar.BarFunctions.LoadBitmap("SystemImages.FreeText.png");
                button.Checked = buttonSettings.Checked;
            }

            return item;
        }

        /// <summary>
        /// Raises the ButtonClearClick event.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnButtonClearClick(CancelEventArgs e)
        {
            if (ButtonClearClick != null)
                ButtonClearClick(this, e);
        }

        /// <summary>
        /// Raises the ButtonDropDownClick event.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnButtonDropDownClick(CancelEventArgs e)
        {
            if (ButtonDropDownClick != null)
                ButtonDropDownClick(this, e);
        }

        /// <summary>
        /// Gets or sets whether input part of the control is read-only. When set to true the input part of the control becomes
        /// read-only and does not allow the typing. However, drop-down part if visible still allows user to change the value of the control
        /// Use this property to allow change of the value through drop-down picker only.
        /// </summary>
        [DefaultValue(false), Description("Indicates whether input part of the control is read-only.")]
        public bool IsInputReadOnly
        {
            get { return _DateInputGroup.IsReadOnly; }
            set
            {
                _DateInputGroup.IsReadOnly = value;
            }
        }

        /// <summary>
        /// Gets or sets whether empty input values (year, month or day) are set to defaults while user is entering data. Default value is true.
        /// </summary>
        [DefaultValue(true), Category("Behavior"), Description("Indicates whether empty input values (year, month or day) are set to defaults while user is entering data")]
        public bool DefaultInputValues
        {
            get { return _DateInputGroup.DefaultInputValues; }
            set
            {
                _DateInputGroup.DefaultInputValues = value;
            }
        }
        

        /// <summary>
        /// Gets the reference to the internal MonthCalendarItem control which is used to display calendar when drop-down is open.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Gets the reference to the internal MonthCalendarAdv control which is used to display calendar when drop-down is open.")]
        public MonthCalendarItem MonthCalendar
        {
            get
            {
                return _MonthCalendar;
            }
        }

        protected override Size DefaultSize
        {
            get
            {
                return new Size(200, base.DefaultSize.Height);
            }
        }

        private bool _AutoSelectDate = false;
        /// <summary>
        /// Gets or sets whether first day in month is automatically selected on popup date picker when month or year is changed.
        /// </summary>
        [Browsable(true), DefaultValue(false), Category("Behavior"), Description("Indicates whether first day in month is automatically selected on popup date picker when month or year is changed.")]
        public bool AutoSelectDate
        {
            get { return _AutoSelectDate; }
            set
            {
                _AutoSelectDate = value;
            }
        }

        private bool _AutoAdvance = false;
        /// <summary>
        /// Gets or sets whether input focus is automatically advanced to next input field when input is complete in current one.
        /// </summary>
        [DefaultValue(false), Category("Behavior"), Description("Indicates whether input focus is automatically advanced to next input field when input is complete in current one.")]
        public bool AutoAdvance
        {
            get { return _DateInputGroup.AutoAdvance; }
            set
            {
                if (_DateInputGroup.AutoAdvance != value)
                {
                    _DateInputGroup.AutoAdvance = value;
                }
            }
        }

        /// <summary>
        /// List of characters that when pressed would select next input field. For example if you are
        /// allowing time input you could set this property to : so when user presses the : character,
        /// the input is forwarded to the next input field.
        /// </summary>
        [DefaultValue(""), Category("Behavior"), Description("List of characters that when pressed would select next input field.")]
        public string SelectNextInputCharacters
        {
            get { return _DateInputGroup.SelectNextInputCharacters; }
            set
            {
                if (_DateInputGroup.SelectNextInputCharacters != value)
                {
                    _DateInputGroup.SelectNextInputCharacters = value;
                }
            }
        }
        #endregion

        #region ICommandSource Members
        protected virtual void ExecuteCommand()
        {
            if (_Command == null) return;
            CommandManager.ExecuteCommand(this);
        }

        /// <summary>
        /// Gets or sets the command assigned to the item. Default value is null.
        /// <remarks>Note that if this property is set to null Enabled property will be set to false automatically to disable the item.</remarks>
        /// </summary>
        [DefaultValue(null), Category("Commands"), Description("Indicates the command assigned to the item.")]
        public Command Command
        {
            get { return (Command)((ICommandSource)this).Command; }
            set
            {
                ((ICommandSource)this).Command = value;
            }
        }

        private ICommand _Command = null;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        ICommand ICommandSource.Command
        {
            get
            {
                return _Command;
            }
            set
            {
                bool changed = false;
                if (_Command != value)
                    changed = true;

                if (_Command != null)
                    CommandManager.UnRegisterCommandSource(this, _Command);
                _Command = value;
                if (value != null)
                    CommandManager.RegisterCommand(this, value);
                if (changed)
                    OnCommandChanged();
            }
        }

        /// <summary>
        /// Called when Command property value changes.
        /// </summary>
        protected virtual void OnCommandChanged()
        {
        }

        private object _CommandParameter = null;
        /// <summary>
        /// Gets or sets user defined data value that can be passed to the command when it is executed.
        /// </summary>
        [Browsable(true), DefaultValue(null), Category("Commands"), Description("Indicates user defined data value that can be passed to the command when it is executed."), System.ComponentModel.TypeConverter(typeof(System.ComponentModel.StringConverter)), System.ComponentModel.Localizable(true)]
        public object CommandParameter
        {
            get
            {
                return _CommandParameter;
            }
            set
            {
                _CommandParameter = value;
            }
        }

        #endregion

        #region Free-Text Entry Support
        /// <summary>
        /// Occurs if Free-Text entry value is not natively recognized by the control and provides you with opportunity to convert that value to the
        /// value control expects.
        /// </summary>
        [Description("Occurs if Free-Text entry value is not natively recognized by the control and provides you with opportunity to convert that value to the value control expects."), Category("Free-Text")]
        public event FreeTextEntryConversionEventHandler ConvertFreeTextEntry;
        /// <summary>
        /// Occurs when Free-Text button is clicked and allows you to cancel its default action.
        /// </summary>
        public event CancelEventHandler ButtonFreeTextClick;

        private void FreeTextButtonClick(object sender, EventArgs e)
        {
            CancelEventArgs cancelArgs = new CancelEventArgs();
            OnButtonFreeTextClick(cancelArgs);
            if (cancelArgs.Cancel) return;

            FreeTextEntryMode = !FreeTextEntryMode;
            _ButtonFreeText.Checked = FreeTextEntryMode;
            if (FreeTextEntryMode && _FreeTextEntryBox != null && !_FreeTextEntryBox.Focused)
                _FreeTextEntryBox.Focus();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnButtonFreeTextClick(CancelEventArgs e)
        {
            CancelEventHandler handler = ButtonFreeTextClick;
            if (handler != null) handler(this, e);
        }

        private bool _AutoResolveFreeTextEntries = true;
        /// <summary>
        /// Gets or sets whether free text entries are attempted to be auto-resolved to dates like Today to today's date or Now to date and time now etc. Default value is true.
        /// </summary>
        [DefaultValue(true), Category("Free-Text"), Description("Indicates whether free text entries are attempted to be auto-resolved to dates like Today to today's date or Now to date and time now etc.")]
        public bool AutoResolveFreeTextEntries
        {
            get { return _AutoResolveFreeTextEntries; }
            set
            {
                _AutoResolveFreeTextEntries = value;
            }
        }

        private bool _AutoOffFreeTextEntry = false;
        /// <summary>
        /// Gets or sets whether free-text entry is automatically turned off when control loses input focus. Default value is false.
        /// </summary>
        [DefaultValue(false), Category("Free-Text"), Description("Indicates whether free-text entry is automatically turned off when control loses input focus.")]
        public bool AutoOffFreeTextEntry
        {
            get { return _AutoOffFreeTextEntry; }
            set
            {
                _AutoOffFreeTextEntry = value;
            }
        }

        private bool _FreeTextEntryMode = false;
        /// <summary>
        /// Gets or sets whether control input is in free-text input mode. Default value is false.
        /// </summary>
        [DefaultValue(false), Category("Free-Text"), Description("Indicates whether control input is in free-text input mode.")]
        public bool FreeTextEntryMode
        {
            get { return _FreeTextEntryMode; }
            set
            {
                _FreeTextEntryMode = value;
                OnFreeTextEntryModeChanged();
            }
        }

        private void OnFreeTextEntryModeChanged()
        {
            if (!_FreeTextEntryMode)
            {
                if (_FreeTextEntryBox != null)
                {
                    _FreeTextEntryBox.ApplyValue -= ApplyFreeTextValue;
                    _FreeTextEntryBox.RevertValue -= RevertFreeTextValue;
                    _FreeTextEntryBox.LostFocus -= FreeTextLostFocus;
                    this.Controls.Remove(_FreeTextEntryBox);
                    _FreeTextEntryBox.Dispose();
                    _FreeTextEntryBox = null;
                }
            }
            else
            {
                UpdateFreeTextBoxVisibility();
            }
            if (_ButtonFreeText != null) _ButtonFreeText.Checked = _FreeTextEntryMode;
        }

        protected override void OnIsKeyboardFocusWithinChanged()
        {
            if (_FreeTextEntryMode)
            {
                UpdateFreeTextBoxVisibility();
                if (this.IsKeyboardFocusWithin)
                {
                    Control textBox = GetFreeTextBox();
                    if (!textBox.Focused)
                        textBox.Focus();
                }
            }
            base.OnIsKeyboardFocusWithinChanged();
        }

        private void UpdateFreeTextBoxVisibility()
        {
            FreeTextEntryBox textBox = (FreeTextEntryBox)GetFreeTextBox();
            if (this.IsKeyboardFocusWithin)
            {
                textBox.Visible = true;
                textBox.Text = this.Text;
                RootVisualItem.InvalidateArrange();
                this.Invalidate();
            }
            else
            {
                if (textBox.Visible)
                {
                    if (textBox.Focused)
                        textBox.HideOnLostFocus();
                    else
                        textBox.Visible = false;
                    RootVisualItem.InvalidateArrange();
                    this.Invalidate();
                }
            }
        }

        protected override bool SupportsFreeTextEntry
        {
            get
            {
                return true;
            }
        }

        private FreeTextEntryBox _FreeTextEntryBox = null;
        protected override Control GetFreeTextBox()
        {
            if (_FreeTextEntryBox == null)
            {
                _FreeTextEntryBox = new FreeTextEntryBox();
                _FreeTextEntryBox.ApplyValue += ApplyFreeTextValue;
                _FreeTextEntryBox.RevertValue += RevertFreeTextValue;
                _FreeTextEntryBox.LostFocus += FreeTextLostFocus;
                _FreeTextEntryBox.BorderStyle = BorderStyle.None;
                this.Controls.Add(_FreeTextEntryBox);
            }

            return _FreeTextEntryBox;
        }

        private void RevertFreeTextValue(object sender, EventArgs e)
        {
            if (_FreeTextEntryBox != null) _FreeTextEntryBox.Text = this.Text;
        }

        protected virtual void OnConvertFreeTextEntry(FreeTextEntryConversionEventArgs e)
        {
            FreeTextEntryConversionEventHandler handler = this.ConvertFreeTextEntry;
            if (handler != null) handler(this, e);
        }

        private void ApplyFreeTextValue(object sender, EventArgs e)
        {
            if (_FreeTextEntryBox == null) return;
            if (string.IsNullOrEmpty(_FreeTextEntryBox.Text))
                this.ValueObject = null;
            else
            {
                DateTime date;
                if (DateTime.TryParse(_FreeTextEntryBox.Text, out date) && _AutoResolveFreeTextEntries)
                {
                    this.Value = date;
                }
                else
                {
                    FreeTextEntryConversionEventArgs eventArgs = new FreeTextEntryConversionEventArgs(_FreeTextEntryBox.Text);
                    OnConvertFreeTextEntry(eventArgs);
                    if (eventArgs.IsValueConverted)
                    {
                        if (eventArgs.ControlValue is DateTime)
                            this.Value = (DateTime)eventArgs.ControlValue;
                        else if (eventArgs.ControlValue == null)
                            this.ValueObject = null;
                        else
                            throw new ArgumentException("ControlValue assigned is not DateTime type.");
                    }
                    else
                    {
                        if (_AutoResolveFreeTextEntries)
                        {
                            date=DateTime.MinValue;
                            string text = _FreeTextEntryBox.Text.ToLower();
                            if (text == "now")
                                date = DateTime.Now;
                            else if (text == "today")
                                date = DateTime.Today;
                            else if (text == "tomorrow")
                                date = DateTime.Today.AddDays(1);
                            else if (text == "yesterday")
                                date = DateTime.Today.AddDays(-1);
                            if (date == DateTime.MinValue)
                                this.ValueObject = null;
                            else
                                this.Value = date;
                        }
                    }
                }
            }
        }

        private void FreeTextLostFocus(object sender, EventArgs e)
        {
            if (_AutoOffFreeTextEntry && !this.IsKeyboardFocusWithin)
                this.FreeTextEntryMode = false;
        }

        protected override void HideFreeTextBoxEntry()
        {
            if (_FreeTextEntryBox != null) _FreeTextEntryBox.Visible = false;
        }

        protected override bool IsFreeTextEntryVisible
        {
            get
            {
                return _FreeTextEntryMode && this.IsKeyboardFocusWithin;
            }
        }
        #endregion

    }
}
#endif

