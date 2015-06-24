#if FRAMEWORK20
using System;
using System.Drawing;
using System.Text;
using DevComponents.DotNetBar;
using System.ComponentModel;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Globalization;

namespace DevComponents.Editors.DateTimeAdv
{
    public class MonthCalendarItem : CalendarBase
    {
        #region Private Variables
        private Size _Spacing = new Size(10, 8);
        private List<DateTime> _AnnuallyMarkedDates = new List<DateTime>();
        private List<DateTime> _MonthlyMarkedDates = new List<DateTime>();
        private List<DateTime> _MarkedDates = new List<DateTime>();
        private static string _BottomContainerName = "sysCalBottomContainer";
        private static string _TodayButtonName = "sysTodayButton";
        private static string _ClearButtonName = "sysClearSelectionButton";
        private ItemContainer _BottomContainer = null;
        private ButtonItem _TodayButton = null;
        private ButtonItem _ClearButton = null;
        #endregion

        #region Events
        /// <summary>
        /// Occurs when month displayed by the item has changed.
        /// </summary>
        public event EventHandler MonthChanged;

        /// <summary>
        /// Occurs before the month that is displayed is changed.
        /// </summary>
        public event EventHandler MonthChanging;


        /// <summary>
        /// Occurs when child label representing days is rendered and it allows you to override default rendering.
        /// </summary>
        public event DayPaintEventHandler PaintLabel;

        /// <summary>
        /// Occurs when mouse button is pressed over the day/week label inside of the calendar.
        /// </summary>
        [System.ComponentModel.Description("Occurs when mouse button is pressed over the day/week label inside of the calendar.")]
        public event System.Windows.Forms.MouseEventHandler LabelMouseDown;

        /// <summary>
        /// Occurs when mouse button is released over day/week label inside of the calendar.
        /// </summary>
        [System.ComponentModel.Description("Occurs when mouse button is released over day/week label inside of the calendar.")]
        public event System.Windows.Forms.MouseEventHandler LabelMouseUp;

        /// <summary>
        /// Occurs when mouse enters the day/week label inside of the calendar.
        /// </summary>
        [System.ComponentModel.Description("Occurs when mouse enters the day/week label inside of the calendar.")]
        public event EventHandler LabelMouseEnter;

        /// <summary>
        /// Occurs when mouse leaves the day/week label inside of the calendar.
        /// </summary>
        [System.ComponentModel.Description("Occurs when mouse leaves the day/week label inside of the calendar.")]
        public event EventHandler LabelMouseLeave;

        /// <summary>
        /// Occurs when mouse moves over the day/week label inside of the calendar.
        /// </summary>
        [System.ComponentModel.Description("Occurs when mouse moves over the day/week label inside of the calendar.")]
        public event System.Windows.Forms.MouseEventHandler LabelMouseMove;

        /// <summary>
        /// Occurs when mouse remains still inside an day/week label of the calendar for an amount of time.
        /// </summary>
        [System.ComponentModel.Description("Occurs when mouse remains still inside an day/week label of the calendar for an amount of time.")]
        public event EventHandler LabelMouseHover;

        /// <summary>
        /// Occurs when SelectedDate property has changed.
        /// </summary>
        [System.ComponentModel.Description("Occurs when SelectedDate property has changed.")]
        public event EventHandler DateChanged;

        /// <summary>
        /// Occurs when the user makes an explicit date selection using the mouse. 
        /// <remarks>
        /// </remarks>
        /// This event is similar to the DateChanged event, but it occurs at the end of a date selection made using the mouse.
        /// The DateChanged event occurs during any date selection, whether by mouse, keyboard, or code. You should handle this event
        /// when you enable multiple date selection through MultiSelect property and want to be notified after the date selection has been
        /// made. DateChanged event would fire each time selection changes during the selection of multiple dates.
        /// </summary>
        [Description("Occurs when the user makes an explicit date selection using the mouse.")]
        public event DateRangeEventHandler DateSelected;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the MonthCalendarItem class.
        /// </summary>
        public MonthCalendarItem()
        {
            _Colors.Parent = this;

            // At least one month is always visible
            SingleMonthCalendar m = new SingleMonthCalendar();
            m.MonthChanging += new EventHandler(FirstCalendarMonthChanging);
            m.MonthChanged += new EventHandler(FirstCalendarMonthChanged);
            m.PaintLabel += new DayPaintEventHandler(InternalPaintLabel);
            m.LabelMouseDown += new System.Windows.Forms.MouseEventHandler(InternalLabelMouseDown);
            m.LabelMouseEnter += new EventHandler(InternalLabelMouseEnter);
            m.LabelMouseHover += new EventHandler(InternalLabelMouseHover);
            m.LabelMouseLeave += new EventHandler(InternalLabelMouseLeave);
            m.LabelMouseMove += new System.Windows.Forms.MouseEventHandler(InternalLabelMouseMove);
            m.LabelMouseUp += new System.Windows.Forms.MouseEventHandler(InternalLabelMouseUp);
            m.NavigationBackgroundStyle.StyleChanged += new EventHandler(FirstCalendarBackStyleChanged);
            m.DateChanged += new EventHandler(SingleMonthSelectedDateChanged);
            this.SubItems.Add(m);

            _BottomContainer = new ItemContainer();
            _BottomContainer.AutoCollapseOnClick = false;
            _BottomContainer.Visible = false;
            _BottomContainer.HorizontalItemAlignment = eHorizontalItemsAlignment.Center;
            _BottomContainer.LayoutOrientation = eOrientation.Horizontal;
            _BottomContainer.ItemSpacing = 6;
            _BottomContainer.Name = _BottomContainerName;

            _TodayButton = CreateTodayButton();
            _BottomContainer.SubItems.Add(_TodayButton);
            _ClearButton = CreateClearButton();
            _BottomContainer.SubItems.Add(_ClearButton);

            this.SubItems.Add(_BottomContainer);

            UpdateSystemButtonsVisibility();
        }
        #endregion

        #region Internal Implementation
        public override void RecalcSize()
        {
            SingleMonthCalendar m = this.SubItems[0] as SingleMonthCalendar;
            if (m == null)
                return;
            _UpdatingSelectedDate = true;
            int index = 1;
            int monthsVisible = 1;
            DateTime currentMonth = _DisplayMonth;
            Size newSize = Size.Empty;
            bool disposeStyle = false;
            ElementStyle backStyle = GetRenderBackgroundStyle(out disposeStyle);
            Rectangle bounds = ElementStyleLayout.GetInnerRect(backStyle, this.Bounds);
            SingleMonthCalendar firstCalendarMonth = m;
            SingleMonthCalendar lastCalendarMonth = null;
            m.TrailingDaysVisible = false;
            m.DisplayMonth = currentMonth;
            m.LeftInternal = bounds.X;
            m.TopInternal = bounds.Y;
            m.RecalcSize();
            m.Displayed = true;
            m.MouseSelectionEnabled = !_MultiSelect;
            if (!_MultiSelect)
                UpdateSelectedDates(m);
            else
                m.SelectedDate = DateTime.MinValue;
            newSize = m.Size;
            Size defaultSize = m.Size;

            Point location = bounds.Location;
            location.X += defaultSize.Width + _Spacing.Width;
            int lineWidth = newSize.Width;
            Size columnRows = new Size(1, 1);
            while (location.Y < bounds.Bottom && _CalendarDimensions.IsEmpty || !_CalendarDimensions.IsEmpty && _CalendarDimensions.Height + 1 > columnRows.Height)
            {
                if (index < this.SubItems.Count && this.SubItems[index] == _BottomContainer)
                    index++;

                if (location.X + defaultSize.Width > bounds.Right && _CalendarDimensions.IsEmpty || !_CalendarDimensions.IsEmpty && columnRows.Width >= _CalendarDimensions.Width)
                {
                    location.Y += defaultSize.Height + _Spacing.Height;
                    location.X = bounds.X;

                    if (lineWidth > newSize.Width)
                        newSize.Width = lineWidth - (columnRows.Height > 1 ? _Spacing.Width : 0);
                    lineWidth = 0;
                    columnRows.Height++;
                    columnRows.Width = 0;
                    if (location.Y + defaultSize.Height > bounds.Bottom && _CalendarDimensions.IsEmpty ||
                        !_CalendarDimensions.IsEmpty && columnRows.Height > _CalendarDimensions.Height)
                        break;
                    newSize.Height += defaultSize.Height + _Spacing.Height;
                }

                if (this.SubItems.Count == index)
                {
                    m = new SingleMonthCalendar();
                    m.DaySize = firstCalendarMonth.DaySize;
                    m.Visible = true;
                    m.Displayed = true;
                    m.TrailingDaysVisible = false;
                    m.HeaderNavigationEnabled = false;
                    m.PaintLabel += new DayPaintEventHandler(InternalPaintLabel);
                    m.LabelMouseDown += new System.Windows.Forms.MouseEventHandler(InternalLabelMouseDown);
                    m.LabelMouseEnter += new EventHandler(InternalLabelMouseEnter);
                    m.LabelMouseHover += new EventHandler(InternalLabelMouseHover);
                    m.LabelMouseLeave += new EventHandler(InternalLabelMouseLeave);
                    m.LabelMouseMove += new System.Windows.Forms.MouseEventHandler(InternalLabelMouseMove);
                    m.LabelMouseUp += new System.Windows.Forms.MouseEventHandler(InternalLabelMouseUp);
                    m.NavigationBackgroundStyle.ApplyStyle(firstCalendarMonth.NavigationBackgroundStyle);
                    m.DateChanged += new EventHandler(SingleMonthSelectedDateChanged);
                    ApplyCommonProperties(m);
                    if (this.SubItems[this.SubItems.Count - 1] == _BottomContainer)
                        this.SubItems.Insert(this.SubItems.Count - 1, m);
                    else
                        this.SubItems.Add(m);
                }
                else
                    m = (SingleMonthCalendar)this.SubItems[index];
                m.TwoLetterDayName = this.TwoLetterDayName;
                m.MinDate = this.MinDate;
                m.MaxDate = this.MaxDate;
                m.WeekOfYearRule = this.WeekOfYearRule;
                m.WeekendDaysSelectable = this.WeekendDaysSelectable;
                m.TodayDate = this.TodayDate;
                m.ShowTodayMarker = this.ShowTodayMarker;
                m.ShowWeekNumbers = this.ShowWeekNumbers;
                m.FirstDayOfWeek = this.FirstDayOfWeek;
                m.DayNames = this.DayNames;
                m.TrailingDaysVisible = false;
                m.MouseSelectionEnabled = !_MultiSelect;
                if (!_MultiSelect)
                    UpdateSelectedDates(m);
                else
                    m.SelectedDate = DateTime.MinValue;

                index++;
                monthsVisible++;
                columnRows.Width++;

                lineWidth += _Spacing.Width;
                m.TopInternal = location.Y;
                m.LeftInternal = location.X;
                currentMonth = currentMonth.AddMonths(1);
                m.DisplayMonth = currentMonth;
                m.RecalcSize();
                lastCalendarMonth = m;
                lineWidth += m.WidthInternal;
                location.X += m.WidthInternal + _Spacing.Width;
            }

            for (int i = index; i < this.SubItems.Count; i++)
            {
                m = this.SubItems[i] as SingleMonthCalendar;
                if (m != null)
                {
                    m.PaintLabel -= new DayPaintEventHandler(InternalPaintLabel);
                    m.LabelMouseDown -= new System.Windows.Forms.MouseEventHandler(InternalLabelMouseDown);
                    m.LabelMouseEnter -= new EventHandler(InternalLabelMouseEnter);
                    m.LabelMouseHover -= new EventHandler(InternalLabelMouseHover);
                    m.LabelMouseLeave -= new EventHandler(InternalLabelMouseLeave);
                    m.LabelMouseMove -= new System.Windows.Forms.MouseEventHandler(InternalLabelMouseMove);
                    m.LabelMouseUp -= new System.Windows.Forms.MouseEventHandler(InternalLabelMouseUp);
                    m.DateChanged -= new EventHandler(SingleMonthSelectedDateChanged);
                }
                if (this.SubItems[i] != _BottomContainer)
                    this.SubItems.Remove(i);
            }

            // Bottom container
            if (_BottomContainerName != null && _BottomContainer.Visible)
            {
                ItemContainer cont = _BottomContainer;
                cont.Displayed = true;
                cont.TopInternal = location.Y;
                cont.LeftInternal = location.X;
                cont.WidthInternal = newSize.Width;
                cont.RecalcSize();
                cont.WidthInternal = newSize.Width;
                newSize.Height += cont.HeightInternal + _Spacing.Height;
            }

            newSize.Width += ElementStyleLayout.HorizontalStyleWhiteSpace(backStyle);
            newSize.Height += ElementStyleLayout.VerticalStyleWhiteSpace(backStyle);

            if (this.SubItems.Count == 1 || this.SubItems.Count == 2 && _BottomContainer != null)
            {
                firstCalendarMonth._TrailingDaysAfterVisible = true;
                firstCalendarMonth._TrailingDaysBeforeVisible = true;
                firstCalendarMonth.TrailingDaysVisible = true;
            }
            else
            {
                firstCalendarMonth._TrailingDaysAfterVisible = false;
                firstCalendarMonth._TrailingDaysBeforeVisible = true;
                firstCalendarMonth.TrailingDaysVisible = true;
                lastCalendarMonth._TrailingDaysAfterVisible = true;
                lastCalendarMonth._TrailingDaysBeforeVisible = false;
                lastCalendarMonth.TrailingDaysVisible = true;
            }
            firstCalendarMonth.NavigationMonthsAheadVisibility = monthsVisible;
            this.Bounds = new Rectangle(this.Bounds.Location, newSize);
            base.RecalcSize();
            ApplySelection();
            OnMonthChanged(new EventArgs());
            _UpdatingSelectedDate = false;

            if(disposeStyle) backStyle.Dispose();
        }

        private void ApplyCommonProperties(SingleMonthCalendar m)
        {
            m.DisplayMonth = DateTime.Today;
        }

        protected internal override void OnItemAdded(BaseItem item)
        {
            if (!(item is SingleMonthCalendar) && item != _BottomContainer)
                throw new InvalidOperationException("MultiMonthCalendar can only accept CalendarMonth items.");
            base.OnItemAdded(item);
        }

        /// <summary>
        /// Returns copy of the item.
        /// </summary>
        public override BaseItem Copy()
        {
            MonthCalendarItem objCopy = new MonthCalendarItem();
            this.CopyToItem(objCopy);
            return objCopy;
        }

        /// <summary>
        /// Copies the CalendarMonth specific properties to new instance of the item.
        /// </summary>
        /// <param name="c">New ButtonItem instance.</param>
        internal void InternalCopyToItem(MonthCalendarItem copy)
        {
            CopyToItem(copy);
        }

        /// <summary>
        /// Copies the CalendarMonth specific properties to new instance of the item.
        /// </summary>
        /// <param name="c">New ButtonItem instance.</param>
        protected override void CopyToItem(BaseItem c)
        {
            MonthCalendarItem copy = c as MonthCalendarItem;
            copy.AnnuallyMarkedDates = this.AnnuallyMarkedDates;
            copy.CalendarDimensions = this.CalendarDimensions;
            copy.ClearButtonVisible = this.ClearButtonVisible;
            copy.CommandsBackgroundStyle.ApplyStyle(this.CommandsBackgroundStyle);
            copy.DayNames = this.DayNames;
            copy.DaySize = this.DaySize;
            copy.DisplayMonth = this.DisplayMonth;
            copy.FirstDayOfWeek = this.FirstDayOfWeek;
            copy.MarkedDates = this.MarkedDates;
            copy.MaxDate = this.MaxDate;
            copy.MaxSelectionCount = this.MaxSelectionCount;
            copy.MinDate = this.MinDate;
            copy.MonthlyMarkedDates = this.MonthlyMarkedDates;
            copy.MultiSelect=this.MultiSelect;
            copy.NavigationBackgroundStyle.ApplyStyle(this.NavigationBackgroundStyle);
            copy.SelectedDate = this.SelectedDate;
            copy.SelectionRange = this.SelectionRange;
            copy.ShowTodayMarker = this.ShowTodayMarker;
            copy.ShowWeekNumbers=this.ShowWeekNumbers;
            copy.TodayButtonVisible = this.TodayButtonVisible;
            copy.TodayDate = this.TodayDate;
            copy.TwoLetterDayName = this.TwoLetterDayName;
            copy.WeekendDaysSelectable = this.WeekendDaysSelectable;
            copy.WeeklyMarkedDays = this.WeeklyMarkedDays;
            copy.WeekOfYearRule=this.WeekOfYearRule;

            this.Colors.AnnualMarker.ApplyTo(copy.Colors.AnnualMarker);
            this.Colors.Day.ApplyTo(copy.Colors.Day);
            this.Colors.DayLabel.ApplyTo(copy.Colors.DayLabel);
            this.Colors.DayMarker.ApplyTo(copy.Colors.DayMarker);
            copy.Colors.DaysDividerBorderColors = this.Colors.DaysDividerBorderColors;
            this.Colors.MonthlyMarker.ApplyTo(copy.Colors.MonthlyMarker);
            this.Colors.Selection.ApplyTo(copy.Colors.Selection);
            this.Colors.Today.ApplyTo(copy.Colors.Today);
            this.Colors.TrailingDay.ApplyTo(copy.Colors.TrailingDay);
            this.Colors.TrailingWeekend.ApplyTo(copy.Colors.TrailingWeekend);
            this.Colors.Weekend.ApplyTo(copy.Colors.Weekend);
            this.Colors.WeeklyMarker.ApplyTo(copy.Colors.WeeklyMarker);
            this.Colors.WeekOfYear.ApplyTo(copy.Colors.WeekOfYear);
            
            //base.CopyToItem(copy);
        }

        private int _MaxSelectionCount = 7;
        /// <summary>
        /// Gets or sets the maximum number of days that can be selected in a month calendar control. 
        /// </summary>
        [DefaultValue(7), Description("Gets or sets the maximum number of days that can be selected in a month calendar control.")]
        public int MaxSelectionCount
        {
            get { return _MaxSelectionCount; }
            set
            {
                _MaxSelectionCount = value;
            }
        }

        private DateTime _SelectionStart = DateTime.MinValue;
        /// <summary>
        /// Gets or sets the start date of the selected range of dates. 
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DateTime SelectionStart
        {
            get { return _SelectionStart; }
            set
            {
                if (_SelectionStart != value)
                {
                    SetSelectionRange(value, (this.SelectionEnd == DateTime.MinValue ? value : this.SelectionEnd));
                }
            }
        }

        private DateTime _SelectionEnd = DateTime.MinValue;
        /// <summary>
        /// Gets or sets the end date of the selected range of dates. 
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DateTime SelectionEnd
        {
            get { return _SelectionEnd; }
            set
            {
                if (_SelectionEnd != value)
                {
                    SetSelectionRange((this.SelectionStart == DateTime.MinValue ? value : this.SelectionStart), value);
                }
            }
        }

        /// <summary>
        /// Gets or sets the selected range of dates for a month calendar control. 
        /// <remarks>
        /// Setting this property is functionally equivalent to using the SetSelectionRange method. You can set the start and end dates separately by setting either the SelectionStart or SelectionEnd properties. You cannot change the start and end dates by setting the SelectionRange.Start or SelectionRange.End property values of the SelectionRange property. You should use SelectionStart, SelectionEnd, or SetSelectionRange.
        /// If the Start property value of the SelectionRange is greater than its End property value, the dates are swapped; the End property value becomes the starting date, and Start property value becomes the end date.</remarks>
        /// </summary>
        [Bindable(true), Description("Indicates selected range of dates for a month calendar control.")]
        public SelectionRange SelectionRange
        {
            get
            {
                return new SelectionRange(this.SelectionStart, this.SelectionEnd);
            }
            set
            {
                this.SetSelectionRange(value.Start, value.End);
            }
        }
        /// <summary>
        /// Returns whether property should be serialized by Windows Forms desginer.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeSelectionRange()
        {
            return !DateTime.Equals(this.SelectionEnd, this.SelectionStart);
        }

        /// <summary>
        /// Sets the selected dates in a month calendar control to the specified date range. 
        /// </summary>
        /// <param name="startDate">The beginning date of the selection range.</param>
        /// <param name="endDate">The end date of the selection range.</param>
        /// <exception cref="ArgumentOutOfRangeException">startDate is less than the minimum date allowable for a month calendar control.<br />
        /// -or- <br />
        /// startDate is greater than the maximum allowable date for a month calendar control.<br />
        /// -or- <br />
        /// endDate is less than the minimum date allowable for a month calendar control.<br />
        /// -or- endDate is greater than the maximum allowable date for a month calendar control. <br />
        /// </exception>
        /// <remarks>
        /// If the startDate value is greater than endDate property value, the dates are swapped; the endDate value becomes the starting date, and startDate value becomes the end date.
        /// </remarks>
        public void SetSelectionRange(DateTime startDate, DateTime endDate)
        {
            if (_SelectionStart != DateTime.MinValue || _SelectionEnd != DateTime.MinValue)
            {
                if (startDate > endDate)
                {
                    DateTime d = startDate;
                    startDate = endDate;
                    endDate = d;
                }

                // Validate dates
                if (startDate < this.MinDate)
                    throw new ArgumentOutOfRangeException(
                        "startDate is less than the minimum date allowable for a month calendar control");
                else if (startDate > this.MaxDate)
                    throw new ArgumentOutOfRangeException(
                        "startDate is greater than the maximum allowable date for a month calendar control");
                else if (endDate < this.MinDate)
                    throw new ArgumentOutOfRangeException(
                        "endDate is less than the minimum date allowable for a month calendar control");
                else if (endDate > this.MaxDate)
                    throw new ArgumentOutOfRangeException(
                        "endDate is greater than the maximum allowable date for a month calendar control");
            }

            _SelectionStart = startDate;
            _SelectionEnd = endDate;

            ApplySelection();

            OnDateChanged(new EventArgs());
        }

        /// <summary>
        /// Applies current date selection to the control. You are usually not required to make calls to this method directly since it is automatically
        /// done by the control when selection changes. This method returns immediately if MultiSelect
        /// property is set to false or SelectionStart or SelectionEnd properties have DateTime.MinValue.
        /// </summary>
        public void ApplySelection()
        {
            if (!_MultiSelect || _SelectionStart == DateTime.MinValue || _SelectionEnd == DateTime.MinValue) return;

            DateTime start = _SelectionStart, end = _SelectionEnd;

            foreach (BaseItem item in this.SubItems)
            {
                SingleMonthCalendar m = item as SingleMonthCalendar;
                if (m == null) continue;
                foreach (BaseItem item2 in m.SubItems)
                {
                    DayLabel day = item2 as DayLabel;
                    if (day == null || !day.Selectable || day.Date == DateTime.MinValue) continue;
                    if (day.Date >= start && day.Date <= end)
                        day.IsSelected = true;
                    else
                        day.IsSelected = false;
                }
            }
        }

        private bool _MultiSelect = false;
        /// <summary>
        /// Gets or sets whether selection of multiple dates up to the MaxSelectionCount is enabled. Default value is false which indicates that only
        /// single day can be selected.
        /// </summary>
        [DefaultValue(false), Description("Indicates whether selection of multiple dates up to the MaxSelectionCount is enabled.")]
        public bool MultiSelect
        {
            get { return _MultiSelect; }
            set
            {
                if (_MultiSelect != value)
                {
                    _MultiSelect = value;
                    OnMultiSelectChanged();
                }
            }
        }

        private void OnMultiSelectChanged()
        {
            foreach (BaseItem item in this.SubItems)
            {
                SingleMonthCalendar m = item as SingleMonthCalendar;
                if (m != null)
                {
                    m.MouseSelectionEnabled = !_MultiSelect;
                    if (_MultiSelect)
                        m.SelectedDate = DateTime.MinValue;
                }
            }
        }

        Point _MouseDownLocation = Point.Empty;
        DateTime _MouseDownDate = DateTime.MinValue;
        public override void InternalMouseDown(MouseEventArgs objArg)
        {
            base.InternalMouseDown(objArg);
            if (!_MultiSelect || objArg.Button != MouseButtons.Left) return;
            int x = objArg.X, y = objArg.Y;
            _MouseDownLocation = new Point(x, y);

            DayLabel day = GetDayAt(x, y);
            if (day != null && day.Date > this.MinDate && day.Date < this.MaxDate)
            {
                _MouseDownDate = day.Date;
                SetSelectionRange(day.Date, day.Date);
            }
        }

        /// <summary>
        /// Returns the DayLabel at given client coordinates or null/nothing if there is no label at give location.
        /// </summary>
        /// <param name="x">X - position in client coordinates.</param>
        /// <param name="y">Y - position in client coordinates.</param>
        /// <returns>DayLabel at given coordinates or null/nothing.</returns>
        public DayLabel GetDayAt(int x, int y)
        {
            BaseItem item = ItemAtLocation(x, y);
            if (item != null && item is SingleMonthCalendar)
            {
                DayLabel day = item.ItemAtLocation(x, y) as DayLabel;
                if (day != null && day.Date > DateTime.MinValue)
                {
                    return day;
                }
            }

            return null;
        }

        /// <summary>
        /// Returns the DayLabel that represents the date.
        /// </summary>
        /// <param name="date">Date to find label for.</param>
        /// <returns>DayLabel object or nothing if date cannot be founds.</returns>
        public DayLabel GetDay(DateTime date)
        {
            foreach (BaseItem item in this.SubItems)
            {
                SingleMonthCalendar mc = item as SingleMonthCalendar;
                if (mc != null)
                {
                    DayLabel day = mc.GetDayLabel(date);
                    if (day != null) return day;
                }
            }

            return null;
        }

        public override void InternalMouseMove(MouseEventArgs objArg)
        {
            base.InternalMouseMove(objArg);
            if (!_MultiSelect || objArg.Button != MouseButtons.Left) return;

            DayLabel day = GetDayAt(objArg.X, objArg.Y);
            if (day != null && this.SelectionStart > DateTime.MinValue)
            {
                DateTime end = day.Date;
                DateTime start = _MouseDownDate;
                if (Math.Abs(start.Subtract(end).TotalDays) > this.MaxSelectionCount)
                    end = start.AddDays((this.MaxSelectionCount - 1) * Math.Sign(end.Subtract(start).TotalDays));

                if (start < MinDate) start = MinDate;
                if (start > MaxDate) start = MaxDate;
                if (end > MaxDate) end = MaxDate;
                if (end < MinDate) end = MinDate;

                this.SetSelectionRange(start, end);
            }
        }

        public override void InternalMouseUp(MouseEventArgs objArg)
        {
            base.InternalMouseUp(objArg);
            if (objArg.Button == MouseButtons.Left && _MouseDownDate > DateTime.MinValue)
            {
                _MouseDownDate = DateTime.MinValue;
                OnDateSelected(new DateRangeEventArgs(this.SelectionStart, this.SelectionEnd));
            }
        }

        /// <summary>
        /// Gets or sets the array of DateTime objects that determine which monthly days to mark using Colors.MonthlyMarker settings. 
        /// </summary>
        [DefaultValue(null), Localizable(true), Description("Indicates array of DateTime objects that determine which monthly days to mark using Colors.MonthlyMarker settings. ")]
        public DateTime[] MonthlyMarkedDates
        {
            get { return _MonthlyMarkedDates.ToArray(); }
            set
            {
                OnMonthlyMarkedDatesChanged(value);
            }
        }
        /// <summary>
        /// Returns whether property should be serialized. Provided for Windows Forms designer support.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeMonthlyMarkedDates()
        {
            return _MonthlyMarkedDates.Count > 0;
        }
        /// <summary>
        /// Resets property to its default value. Provided for Windows Forms designer support.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetMonthlyMarkedDates()
        {
            MonthlyMarkedDates = null;
        }

        private void OnMonthlyMarkedDatesChanged(DateTime[] dates)
        {
            _MonthlyMarkedDates.Clear();
            if (dates != null)
                _MonthlyMarkedDates.AddRange(dates);
            if (this.GetDesignMode())
                UpdateMarkedDates();
        }

        /// <summary>
        /// Removes all monthly marked dates. Note that you must call UpdateMarkedDates method to reflect these changes on calendar.
        /// </summary>
        public void RemoveAllMonthlyMarkedDates()
        {
            MonthlyMarkedDates = null;
        }

        /// <summary>
        /// Removes the date from the MonthlyMarkedDates. Note that you must call UpdateMarkedDates method to reflect these changes on calendar.
        /// </summary>
        /// <param name="dt"></param>
        public void RemoveMonthlyMarkedDate(DateTime dt)
        {
            DateTime[] dates = this.MonthlyMarkedDates;
            foreach (DateTime d in dates)
            {
                if (d.Day == dt.Day)
                    _MonthlyMarkedDates.Remove(d);
            }
        }

        private List<DayOfWeek> _WeeklyMarkedDays = new List<DayOfWeek>();
        /// <summary>
        /// Gets or sets the array of DayOfWeek members that determine which days of week to mark using Colors.WeeklyMarker settings. 
        /// </summary>
        [DefaultValue(null), Localizable(true), Description("Indicates array of DayOfWeek members that determine which days of week to mark using Colors.WeeklyMarker settings. ")]
        public DayOfWeek[] WeeklyMarkedDays
        {
            get { return _WeeklyMarkedDays.ToArray(); }
            set
            {
                OnWeeklyMarkedDaysChanged(value);
            }
        }
        /// <summary>
        /// Returns whether property should be serialized. Provided for Windows Forms designer support.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeWeeklyMarkedDays()
        {
            return _WeeklyMarkedDays.Count > 0;
        }
        /// <summary>
        /// Resets property to its default value. Provided for Windows Forms designer support.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetWeeklyMarkedDays()
        {
            MonthlyMarkedDates = null;
        }

        private void OnWeeklyMarkedDaysChanged(DayOfWeek[] days)
        {
            _WeeklyMarkedDays.Clear();
            if (days != null)
                _WeeklyMarkedDays.AddRange(days);
            if (this.GetDesignMode())
                UpdateMarkedDates();
        }

        /// <summary>
        /// Removes all weekly marked days. Note that you must call UpdateMarkedDates method to reflect these changes on calendar.
        /// </summary>
        public void RemoveAllWeeklyMarkedDays()
        {
            WeeklyMarkedDays = null;
        }

        /// <summary>
        /// Removes the date from the MonthlyMarkedDates. Note that you must call UpdateMarkedDates method to reflect these changes on calendar.
        /// </summary>
        /// <param name="dt"></param>
        public void RemoveWeeklyMarkedDay(DayOfWeek day)
        {
            DayOfWeek[] dates = this.WeeklyMarkedDays;
            foreach (DayOfWeek d in dates)
            {
                if (d == day)
                    _WeeklyMarkedDays.Remove(d);
            }
        }


        /// <summary>
        /// Gets or sets the array of DateTime objects that determines which annual days are marked using Colors.AnnualMarker settings.
        /// </summary>
        [DefaultValue(null), Localizable(true), Description("Indicates array of DateTime objects that determines which annual days are marked using Colors.AnnualMarker settings.")]
        public DateTime[] AnnuallyMarkedDates
        {
            get { return _AnnuallyMarkedDates.ToArray(); }
            set
            {
                OnAnnuallyMarkedDatesChanged(value);
            }
        }
        /// <summary>
        /// Returns whether property should be serialized. Provided for Windows Forms designer support.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeAnnuallyMarkedDates()
        {
            return _AnnuallyMarkedDates.Count > 0;
        }
        /// <summary>
        /// Resets property to its default value. Provided for Windows Forms designer support.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetAnnuallyMarkedDates()
        {
            AnnuallyMarkedDates = null;
        }

        private void OnAnnuallyMarkedDatesChanged(DateTime[] dates)
        {
            _AnnuallyMarkedDates.Clear();
            if (dates != null)
                _AnnuallyMarkedDates.AddRange(dates);
            if (this.GetDesignMode())
                UpdateMarkedDates();
        }

        /// <summary>
        /// Removes all annually marked dates. Note that you must call UpdateMarkedDates method to reflect these changes on calendar.
        /// </summary>
        public void RemoveAllAnnuallyMarkedDates()
        {
            AnnuallyMarkedDates = null;
        }

        /// <summary>
        /// Removes the date from the AnnuallyMarkedDates. Note that you must call UpdateMarkedDates method to reflect these changes on calendar.
        /// </summary>
        /// <param name="dt"></param>
        public void RemoveAnnuallyMarkedDate(DateTime dt)
        {
            DateTime[] dates = this.AnnuallyMarkedDates;
            foreach (DateTime d in dates)
            {
                if (d.Month == dt.Month && dt.Day == d.Day)
                    _AnnuallyMarkedDates.Remove(d);
            }
        }

        /// <summary>
        /// Gets or sets the array of DateTime objects that determines which nonrecurring dates are marked using Colors.DayMarker settings. 
        /// </summary>
        [DefaultValue(null), Localizable(true), Description("Indicates array of DateTime objects that determines which nonrecurring dates are marked using Colors.DayMarker settings.")]
        public DateTime[] MarkedDates
        {
            get { return _MarkedDates.ToArray(); }
            set
            {
                OnMarkedDatesChanged(value);
            }
        }
        /// <summary>
        /// Returns whether property should be serialized. Provided for Windows Forms designer support.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeMarkedDates()
        {
            return _MarkedDates.Count > 0;
        }
        /// <summary>
        /// Resets property to its default value. Provided for Windows Forms designer support.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetMarkedDates()
        {
            MarkedDates = null;
        }

        private void OnMarkedDatesChanged(DateTime[] dates)
        {
            _MarkedDates.Clear();
            if (dates != null)
                _MarkedDates.AddRange(dates);
            if (this.GetDesignMode())
                UpdateMarkedDates();
        }

        /// <summary>
        /// Removes all marked dates set through MarkedDates property. Note that you must call UpdateMarkedDates method to reflect these changes on calendar.
        /// </summary>
        public void RemoveAllMarkedDates()
        {
            MarkedDates = null;
        }

        /// <summary>
        /// Removes the date from the MarkedDates collection. Note that you must call UpdateMarkedDates method to reflect these changes on calendar.
        /// </summary>
        /// <param name="dt"></param>
        public void RemoveMarkedDate(DateTime dt)
        {
            DateTime[] dates = this.MarkedDates;
            foreach (DateTime d in dates)
            {
                if (d == dt)
                    _MarkedDates.Remove(d);
            }
        }

        /// <summary>
        /// Repaints the marked dates to reflect the dates set in the lists of marked dates.
        /// Use this method to reflect the changes made to the AnnuallyMarkedDates, MonthlyMarkedDates or MarkedDates properties as well
        /// as change to the marked Colors properties.
        /// </summary>
        public void UpdateMarkedDates()
        {
            this.DisplayMonth = _DisplayMonth;
        }

        private bool GetDesignMode()
        {
            if (this.DesignMode) return true;
            if(this.ContainerControl is MonthCalendarAdv)
                return ((MonthCalendarAdv)this.ContainerControl).GetDesignModeInternal();
            return false;
        }

        private DateTime _DisplayMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        /// <summary>
        /// Gets or sets the first month displayed by the control.
        /// </summary>
        [Description("Indicates the first month displayed on the control.")]
        public DateTime DisplayMonth
        {
            get { return _DisplayMonth; }
            set
            {
                _DisplayMonth = value.Date;
                m_NeedRecalcSize = true;

                if(this.ContainerControl is Control)
                    BarFunctions.InvokeRecalcLayout((Control)this.ContainerControl, true);
                ApplySelection();
                this.Refresh();
            }
        }

        /// <summary>
        /// Gets number of months displayed on the control.
        /// </summary>
        [Browsable(false)]
        public int DisplayedMonthCount
        {
            get 
            {
                int c = 0;
                foreach (BaseItem item in this.SubItems)
                {
                    if (item is SingleMonthCalendar)
                        c++;
                }
                return c;
            }
        }
        

        private void FirstCalendarMonthChanging(object sender, EventArgs e)
        {
            OnMonthChanging(e);
        }

        private void FirstCalendarMonthChanged(object sender, EventArgs e)
        {
            DevComponents.DotNetBar.Events.EventSourceArgs source = e as DevComponents.DotNetBar.Events.EventSourceArgs;
            if (source != null && source.Source == eEventSource.Mouse)
            {
                SingleMonthCalendar m = this.SubItems[0] as SingleMonthCalendar;
                _DisplayMonth = m.DisplayMonth;
                DateTime currentMonth = _DisplayMonth.AddMonths(1);
                _UpdatingSelectedDate = true;
                for (int i = 1; i < this.SubItems.Count; i++)
                {
                    SingleMonthCalendar month = this.SubItems[i] as SingleMonthCalendar;
                    if (month != null)
                    {
                        month.DisplayMonth = currentMonth;
                        currentMonth = currentMonth.AddMonths(1);
                        UpdateSelectedDates(month);
                    }
                }
                _UpdatingSelectedDate = false;
                this.NeedRecalcSize = false;
            }
            if (!this.NeedRecalcSize)
                OnMonthChanged(e);
        }

        private MonthCalendarColors _Colors = new MonthCalendarColors();
        /// <summary>
        /// Gets the calendar colors used by the control.
        /// </summary>
        [NotifyParentPropertyAttribute(true), Category("Appearance"), Description("Gets colors used by control."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public MonthCalendarColors Colors
        {
            get { return _Colors; }
        }

        /// <summary>
        /// Raises the MonthChanged event.
        /// </summary>
        /// <param name="e">Provides additional event data.</param>
        protected virtual void OnMonthChanged(EventArgs e)
        {
            if (MonthChanged != null)
            {
                bool b = this.NeedRecalcSize;
                MonthChanged(this, e);
                if (this.NeedRecalcSize != b)
                    this.NeedRecalcSize = false;
            }
        }

        /// <summary>
        /// Raises the MonthChanging event.
        /// </summary>
        /// <param name="e">Provides additional event data.</param>
        protected virtual void OnMonthChanging(EventArgs e)
        {
            if (MonthChanging != null)
                MonthChanging(this, e);
        }


        private void InternalPaintLabel(object sender, DayPaintEventArgs e)
        {
            OnPaintLabel(e._Item, e);
        }

        /// <summary>
        /// Raises the PaintLabel event.
        /// </summary>
        /// <param name="e">Provides event data.</param>
        protected virtual void OnPaintLabel(DayLabel label, DayPaintEventArgs e)
        {
            if (PaintLabel != null)
                PaintLabel(label, e);
        }

        private void InternalLabelMouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            OnLabelMouseUp(sender, e);
        }

        private void InternalLabelMouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            OnLabelMouseMove(sender, e);
        }

        private void InternalLabelMouseLeave(object sender, EventArgs e)
        {
            OnLabelMouseLeave(sender, e);
        }

        private void InternalLabelMouseHover(object sender, EventArgs e)
        {
            OnLabelMouseHover(sender, e);
        }

        private void InternalLabelMouseEnter(object sender, EventArgs e)
        {
            OnLabelMouseEnter(sender, e);
        }

        private void InternalLabelMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            OnLabelMouseDown(sender, e);
        }

        /// <summary>
        /// Raises the LabelMouseDown event.
        /// </summary>
        /// <param name="e">Provides event data.</param>
        protected virtual void OnLabelMouseDown(object sender, MouseEventArgs e)
        {
            if (LabelMouseDown != null)
                LabelMouseDown(sender, e);
        }

        /// <summary>
        /// Raises the LabelMouseUp event.
        /// </summary>
        /// <param name="e">Provides event data.</param>
        protected virtual void OnLabelMouseUp(object sender, MouseEventArgs e)
        {
            if (LabelMouseUp != null)
                LabelMouseUp(sender, e);
        }

        /// <summary>
        /// Raises the LabelMouseEnter event.
        /// </summary>
        /// <param name="e">Provides event data.</param>
        protected virtual void OnLabelMouseEnter(object sender, EventArgs e)
        {
            if (LabelMouseEnter != null)
                LabelMouseEnter(sender, e);
        }

        /// <summary>
        /// Raises the LabelMouseLeave event.
        /// </summary>
        /// <param name="e">Provides event data.</param>
        protected virtual void OnLabelMouseLeave(object sender, EventArgs e)
        {
            if (LabelMouseLeave != null)
                LabelMouseLeave(sender, e);
        }

        /// <summary>
        /// Raises the LabelMouseMove event.
        /// </summary>
        /// <param name="e">Provides event data.</param>
        protected virtual void OnLabelMouseMove(object sender, MouseEventArgs e)
        {
            if (LabelMouseMove != null)
                LabelMouseMove(sender, e);
        }

        /// <summary>
        /// Raises the LabelMouseHover event.
        /// </summary>
        /// <param name="e">Provides event data.</param>
        protected virtual void OnLabelMouseHover(object sender, EventArgs e)
        {
            if (LabelMouseHover != null)
                LabelMouseHover(sender, e);
        }

        private SingleMonthCalendar FirstCalendar
        {
            get
            {
                return ((SingleMonthCalendar)this.SubItems[0]);
            }
        }

        /// <summary>
        /// Gets or sets the size of each day item on the calendar. Default value is 24, 15.
        /// </summary>
        [Description("Indicate size of each day item on the calendar.")]
        public Size DaySize
        {
            get { return FirstCalendar.DaySize; }
            set
            {
                foreach (BaseItem item in this.SubItems)
                {
                    SingleMonthCalendar m = item as SingleMonthCalendar;
                    if (m != null)
                        m.DaySize = value;
                }
                this.NeedRecalcSize = true;
                OnAppearanceChanged();
                this.Refresh();
            }
        }
        /// <summary>
        /// Gets whether property should be serialized. Provided for designer support.
        /// </summary>
        /// <returns></returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeDaySize()
        {
            return DaySize.Width != SingleMonthCalendar._DefaultDaySize.Width || DaySize.Height != SingleMonthCalendar._DefaultDaySize.Height;
        }
        /// <summary>
        /// Reset property to default value. Provided for designer support.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetDaySize()
        {
            this.DaySize = SingleMonthCalendar._DefaultDaySize;
        }

        /// <summary>
        /// Gets or sets the minimum date and time that can be selected in the control.
        /// </summary>
        [Description("Indicates minimum date and time that can be selected in the control.")]
        public System.DateTime MinDate
        {
            get { return FirstCalendar.MinDate; }
            set
            {
                foreach (BaseItem item in this.SubItems)
                {
                    SingleMonthCalendar m = item as SingleMonthCalendar;
                    if (m != null)
                        m.MinDate = value;
                }
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
            get { return FirstCalendar.MaxDate; }
            set
            {
                foreach (BaseItem item in this.SubItems)
                {
                    SingleMonthCalendar m = item as SingleMonthCalendar;
                    if (m != null)
                        m.MaxDate = value;
                }
            }
        }
        /// <summary>
        /// Gets whether Value property should be serialized by Windows Forms designer.
        /// </summary>
        /// <returns>true if value serialized otherwise false.</returns>
        public bool ShouldSerializeMaxDate()
        {
            return !MaxDate.Equals(DateTimeGroup.MaxDateTime);
        }
        /// <summary>
        /// Reset the MaxDate property to its default value.
        /// </summary>
        public void ResetMaxDate()
        {
            MaxDate = DateTimeGroup.MaxDateTime;
        }

        /// <summary>
        /// Specifies the commands container background style. Commands container displays Today and Clear buttons if they are visible.
        /// </summary>
        [Browsable(true), Category("Style"), Description("Specifies the commands container background style. Commands container displays Today and Clear buttons if they are visible."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ElementStyle CommandsBackgroundStyle
        {
            get { return _BottomContainer.BackgroundStyle; }
        }

        /// <summary>
        /// Specifies the navigation container background style. Navigation container displays month, year and optional buttons. Default value is an empty style which means that container does not display any background.
        /// BeginGroup property set to true will override this style on some styles.
        /// </summary>
        [Browsable(true), Category("Style"), Description("Indicates navigation container background style."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ElementStyle NavigationBackgroundStyle
        {
            get { return FirstCalendar.NavigationBackgroundStyle; }
        }

        private void FirstCalendarBackStyleChanged(object sender, EventArgs e)
        {
            SingleMonthCalendar firstMonth = FirstCalendar;
            foreach (BaseItem item in this.SubItems)
            {
                SingleMonthCalendar m = item as SingleMonthCalendar;
                if (m != null && m != firstMonth)
                {
                    m.NavigationBackgroundStyle.Reset();
                    m.NavigationBackgroundStyle.ApplyStyle(firstMonth.NavigationBackgroundStyle);
                }
            }
        }

        private Size _CalendarDimensions = Size.Empty;
        /// <summary>
        /// Gets or sets the number of columns and rows of months displayed on control. Default value is 0,0 which indicates that
        /// calendar will display as many columns and rows as it is possible to fit into container space available.
        /// </summary>
        [Description("Indicates number of columns and rows of months displayed on control.")]
        public Size CalendarDimensions
        {
            get { return _CalendarDimensions; }
            set
            {
                if (value.Width < 0 || value.Height < 0)
                    throw new ArgumentOutOfRangeException("Calendar dimension values must be greater or equal to zero.");
                if (value.Width == 0 && value.Height > 0 || value.Width > 0 && value.Height == 0)
                    throw new ArgumentOutOfRangeException("Calendar dimension values must be both set to values grater than zero.");
                if (_CalendarDimensions != value)
                {
                    _CalendarDimensions = value;
                    NeedRecalcSize = true;
                    OnAppearanceChanged();
                    this.Refresh();
                }
            }
        }
        /// <summary>
        /// Resets property to its default value. Provided for design-time support.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetCalendarDimensions()
        {
            CalendarDimensions = Size.Empty;
        }
        /// <summary>
        /// Gets whether property should be serialized. Provided for design-time support.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeCalendarDimensions()
        {
            return CalendarDimensions != Size.Empty;
        }

        /// <summary>
        /// Sets the number of columns and rows of months to display. 
        /// </summary>
        /// <param name="columns">The number of columns. </param>
        /// <param name="rows">The number of rows. </param>
        /// <remarks>ArgumentOutOfRangeException will be raised if any value is less than zero or one value is grater than zero and other is zero.</remarks>
        public void SetCalendarDimensions(int columns, int rows)
        {
            this.CalendarDimensions = new Size(columns, rows);
        }

        /// <summary>
        /// Gets or sets whether weekend days can be selected. Default value is true.
        /// </summary>
        [DefaultValue(true), Description("Indicates whether weekend days can be selected.")]
        public bool WeekendDaysSelectable
        {
            get { return FirstCalendar.WeekendDaysSelectable; }
            set
            {
                if (WeekendDaysSelectable != value)
                {
                    foreach (BaseItem item in this.SubItems)
                    {
                        SingleMonthCalendar m = item as SingleMonthCalendar;
                        if (m != null)
                            m.WeekendDaysSelectable = value;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the rule used to determine first week of the year for week of year display on calendar. Default value is first-day.
        /// </summary>
        [DefaultValue(CalendarWeekRule.FirstDay), Description("Indicates rule used to determine first week of the year for week of year display on calendar.")]
        public CalendarWeekRule WeekOfYearRule
        {
            get { return FirstCalendar.WeekOfYearRule; }
            set
            {
                if (WeekOfYearRule != value)
                {
                    foreach (BaseItem item in this.SubItems)
                    {
                        SingleMonthCalendar m = item as SingleMonthCalendar;
                        if (m != null)
                            m.WeekOfYearRule = value;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the value that is used by calendar as today's date.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DateTime TodayDate
        {
            get { return FirstCalendar.TodayDate; }
            set
            {
                value = value.Date;
                if (TodayDate != value)
                {
                    foreach (BaseItem item in this.SubItems)
                    {
                        SingleMonthCalendar m = item as SingleMonthCalendar;
                        if (m != null)
                            m.TodayDate = value;
                    }
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the TodayDate property has been explicitly set. 
        /// </summary>
        [Browsable(false)]
        public bool TodayDateSet
        {
            get { return FirstCalendar.TodayDateSet; }
        }

        /// <summary>
        /// Gets or sets whether today marker that indicates TodayDate is visible on the calendar. Default value is true.
        /// </summary>
        [DefaultValue(true), Description("Indicates whether today marker that indicates TodayDate is visible on the calendar.")]
        public bool ShowTodayMarker
        {
            get { return FirstCalendar.ShowTodayMarker; }
            set
            {
                if (ShowTodayMarker != value)
                {
                    foreach (BaseItem item in this.SubItems)
                    {
                        SingleMonthCalendar m = item as SingleMonthCalendar;
                        if (m != null)
                            m.ShowTodayMarker = value;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets whether week of year is visible. Default value is false.
        /// </summary>
        [DefaultValue(false), Description("Indicates whether week of year is visible.")]
        public bool ShowWeekNumbers
        {
            get { return FirstCalendar.ShowWeekNumbers; }
            set
            {
                if (ShowWeekNumbers != value)
                {
                    FirstCalendar.ShowWeekNumbers = value;
                    NeedRecalcSize = true;
                    this.OnAppearanceChanged();
                    this.Refresh();
                }
            }
        }

        /// <summary>
        /// Gets or sets whether control uses the two letter day names. Default value is true.
        /// </summary>
        [DefaultValue(true), Description("Indicates whether control uses the two letter day names.")]
        public bool TwoLetterDayName
        {
            get { return FirstCalendar.TwoLetterDayName; }
            set
            {
                FirstCalendar.TwoLetterDayName = value;
                NeedRecalcSize = true;
                this.OnAppearanceChanged();
                this.Refresh();
            }
        }

        /// <summary>
        /// Gets or sets the array of custom names for days displayed on calendar header. The array must have exactly 7 elements representing day names from 0 to 6.
        /// </summary>
        [DefaultValue(null), Description("Indicates array of custom names for days displayed on calendar header."), Localizable(true)]
        public string[] DayNames
        {
            get { return FirstCalendar.DayNames; }
            set
            {
                if (value != null && value.Length != 7)
                    throw new ArgumentException("DayNames must have exactly 7 items in collection.");
                foreach (BaseItem item in this.SubItems)
                {
                    SingleMonthCalendar m = item as SingleMonthCalendar;
                    if (m != null)
                        m.DayNames = value;
                }
                NeedRecalcSize = true;
                this.OnAppearanceChanged();
                this.Refresh();
            }
        }

        /// <summary>
        /// Gets or sets the first day of week displayed on the calendar. Default value is Sunday.
        /// </summary>
        [DefaultValue(DayOfWeek.Sunday), Description("Indicates first day of week displayed on the calendar.")]
        public DayOfWeek FirstDayOfWeek
        {
            get { return FirstCalendar.FirstDayOfWeek; }
            set
            {
                if (FirstDayOfWeek != value)
                {
                    foreach (BaseItem item in this.SubItems)
                    {
                        SingleMonthCalendar m = item as SingleMonthCalendar;
                        if (m != null)
                            m.FirstDayOfWeek = value;
                    }
                }
                this.OnAppearanceChanged();
                this.Refresh();
            }
        }

        private bool _UpdatingSelectedDate = false;
        private DateTime _SelectedDate = DateTime.MinValue;
        /// <summary>
        /// Gets or sets the calendar selected date. Note that SelectedDate property should be used only when MultiSelect property is set to false.
        /// When multiple dates can be selected use range selection properties: SelectionStart, SelectionEnd and SelectionRange.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DateTime SelectedDate
        {
            get
            {
                return _SelectedDate;
            }
            set
            {
                if (_SelectedDate != value)
                {
                    _SelectedDate = value;
                    try
                    {
                        _UpdatingSelectedDate = true;
                        foreach (BaseItem item in this.SubItems)
                        {
                            SingleMonthCalendar m = item as SingleMonthCalendar;
                            if (m != null)
                                UpdateSelectedDates(m);
                        }
                        OnDateChanged(new EventArgs());
                        OnDateSelected(new DateRangeEventArgs(value, value));
                    }
                    finally
                    {
                        _UpdatingSelectedDate = false;
                    }
                }
            }
        }

        private void UpdateSelectedDates(SingleMonthCalendar m)
        {
            if (m.DisplayMonth.Month == _SelectedDate.Month && m.DisplayMonth.Year == _SelectedDate.Year ||
                m.TrailingDaysVisible && m.GetDayLabel(_SelectedDate) != null)
                m.SelectedDate = _SelectedDate;
            else if (m.SelectedDate != DateTime.MinValue)
                m.SelectedDate = DateTime.MinValue;
        }

        /// <summary>
        /// Raises the DateChanged event.
        /// </summary>
        /// <param name="e">Provides event data.</param>
        protected virtual void OnDateChanged(EventArgs e)
        {
            if (DateChanged != null)
                DateChanged(this, e);
        }

        /// <summary>
        /// Raises the DateSelected event.
        /// </summary>
        /// <param name="e">Provides event data.</param>
        protected virtual void OnDateSelected(DateRangeEventArgs e)
        {
            if (DateSelected != null)
                DateSelected(this, e);
        }

        private void SingleMonthSelectedDateChanged(object sender, EventArgs e)
        {
            if (_UpdatingSelectedDate) return;
            SingleMonthCalendar m = sender as SingleMonthCalendar;
            this.SelectedDate = m.SelectedDate;
        }

        /// <summary>
        /// Gets the reference to the bottom container that parents the Today, and Clear system buttons.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ItemContainer BottomContainer
        {
            get
            {
                return _BottomContainer;
            }
        }

        private ButtonItem CreateTodayButton()
        {
            ButtonItem b = new ButtonItem(_TodayButtonName);
            b.GlobalItem = false;
            b.Text = GetTodayButtonText();
            b.AutoCollapseOnClick = false;
            b._FadeEnabled = false;
            b.Click += new EventHandler(TodayButtonClick);
            return b;
        }

        /// <summary>
        /// Gets reference to internal Today button on calendar.
        /// </summary>
        [Browsable(false)]
        public ButtonItem TodayButton
        {
            get
            {
                return _TodayButton;
            }
        }

        private void TodayButtonClick(object sender, EventArgs e)
        {
            DateTime d = TodayDateSet ? TodayDate : DateTime.Today;
            this.DisplayMonth = d;
            this.SelectedDate = d;
        }

        private string GetTodayButtonText()
        {
            string s = "";
            using (LocalizationManager lm = new LocalizationManager(this.GetOwner() as IOwnerLocalize))
                s = lm.GetLocalizedString(LocalizationKeys.MonthCalendarTodayButtonText);
            if (s == "") s = "Today";
            return s;
        }

        /// <summary>
        /// Reloads the localized strings for Today and Clear buttons.
        /// </summary>
        public void ReloadLocalizedStrings()
        {
            if (_TodayButton != null) _TodayButton.Text = GetTodayButtonText();
            if (_ClearButton != null) _ClearButton.Text = GetClearButtonText();
        }

        private ButtonItem CreateClearButton()
        {
            ButtonItem b = new ButtonItem(_ClearButtonName);
            b.GlobalItem = false;
            b.Text = GetClearButtonText();
            b.AutoCollapseOnClick = false;
            b._FadeEnabled = false;
            b.Click += new EventHandler(ClearButtonClick);
            return b;
        }

        private void ClearButtonClick(object sender, EventArgs e)
        {
            this.SelectedDate = DateTime.MinValue;
        }

        private string GetClearButtonText()
        {
            string s = "";
            using (LocalizationManager lm = new LocalizationManager(this.GetOwner() as IOwnerLocalize))
                s = lm.GetLocalizedString(LocalizationKeys.MonthCalendarClearButtonText);
            if (s == "") s = "Clear";
            return s;
        }

        private bool _TodayButtonVisible = false;
        /// <summary>
        /// Gets or sets whether Today button displayed at the bottom of the calendar is visible. Default value is false.
        /// </summary>
        [DefaultValue(false), Description("Indicates whether Today button displayed at the bottom of the calendar is visible.")]
        public bool TodayButtonVisible
        {
            get { return _TodayButtonVisible; }
            set
            {
                if (_TodayButtonVisible != value)
                {
                    _TodayButtonVisible = value;
                    NeedRecalcSize = true;
                    UpdateSystemButtonsVisibility();
                    OnAppearanceChanged();
                    Refresh();
                }
            }
        }

        private bool _ClearButtonVisible = false;
        /// <summary>
        /// Gets or sets whether Clear button displayed at the bottom of the calendar is visible. Clear button clears the currently selected date. Default value is false.
        /// </summary>
        [DefaultValue(false), Description("Indicates whether Clear button displayed at the bottom of the calendar is visible. Clear button clears the currently selected date.")]
        public bool ClearButtonVisible
        {
            get { return _ClearButtonVisible; }
            set
            {
                if (_ClearButtonVisible != value)
                {
                    _ClearButtonVisible = value;
                    NeedRecalcSize = true;
                    UpdateSystemButtonsVisibility();
                    OnAppearanceChanged();
                    Refresh();
                }
            }
        }

        private void UpdateSystemButtonsVisibility()
        {
            if (_TodayButtonVisible != _TodayButton.Visible)
                _TodayButton.Visible = _TodayButtonVisible;
            if (_ClearButtonVisible != _ClearButton.Visible)
                _ClearButton.Visible = _ClearButtonVisible;

            bool b = _ClearButtonVisible | _TodayButtonVisible;
            if (b != _BottomContainer.Visible)
                _BottomContainer.Visible = b;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the item is expanded or not. For Popup items this would indicate whether the item is popped up or not.
        /// </summary>
        [System.ComponentModel.Browsable(false), System.ComponentModel.DefaultValue(false), System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public override bool Expanded
        {
            get
            {
                return m_Expanded;
            }
            set
            {
                base.Expanded = value;
                if (!value)
                    BaseItem.CollapseSubItems(this);
            }
        }
        #endregion

    }
}
#endif

