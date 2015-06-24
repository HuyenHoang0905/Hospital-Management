#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using DevComponents.DotNetBar;
using System.Windows.Forms;
using System.Drawing;
using System.Globalization;

namespace DevComponents.Editors.DateTimeAdv
{
    /// <summary>
    /// Represents a control that enables the user to select a date using a visual monthly calendar display. 
    /// </summary>
    [ToolboxBitmap(typeof(DotNetBarManager), "MonthCalendarAdv.ico"), ToolboxItem(true), Designer("DevComponents.DotNetBar.Design.MonthCalendarAdvDesigner, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
    public class MonthCalendarAdv : ItemControl
    {
        #region Private Variables
        private MonthCalendarItem _MonthCalendar = null;
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
        /// Initializes a new instance of the MonthCalendarAdv class.
        /// </summary>
        public MonthCalendarAdv()
        {
            _MonthCalendar = new MonthCalendarItem();
            _MonthCalendar.GlobalItem = false;
            _MonthCalendar.ContainerControl = this;
            _MonthCalendar.Stretch = false;
            _MonthCalendar.Displayed = true;
            _MonthCalendar.Style = eDotNetBarStyle.Office2007;
            this.ColorScheme.Style = eDotNetBarStyle.Office2007;
            _MonthCalendar.SetOwner(this);

            _MonthCalendar.MonthChanged += new EventHandler(MonthCalendar_MonthChanged);
            _MonthCalendar.MonthChanging += new EventHandler(MonthCalendar_MonthChanging);
            _MonthCalendar.PaintLabel += new DayPaintEventHandler(MonthCalendar_PaintLabel);
            _MonthCalendar.LabelMouseDown += new MouseEventHandler(MonthCalendar_LabelMouseDown);
            _MonthCalendar.LabelMouseUp += new MouseEventHandler(MonthCalendar_LabelMouseUp);
            _MonthCalendar.LabelMouseEnter += new EventHandler(MonthCalendar_LabelMouseEnter);
            _MonthCalendar.LabelMouseLeave += new EventHandler(MonthCalendar_LabelMouseLeave);
            _MonthCalendar.LabelMouseMove += new MouseEventHandler(MonthCalendar_LabelMouseMove);
            _MonthCalendar.LabelMouseHover += new EventHandler(MonthCalendar_LabelMouseHover);
            _MonthCalendar.DateChanged += new EventHandler(MonthCalendar_DateChanged);
            _MonthCalendar.DateSelected += new DateRangeEventHandler(MonthCalendar_DateSelected);
            this.SetBaseItemContainer(_MonthCalendar);
        }
        #endregion

        #region Internal Implementation
        private void MonthCalendar_DateSelected(object sender, DateRangeEventArgs e)
        {
            OnDateSelected(e);
        }

        private void MonthCalendar_DateChanged(object sender, EventArgs e)
        {
            OnDateChanged(e);
        }
        private void MonthCalendar_LabelMouseHover(object sender, EventArgs e)
        {
            OnLabelMouseHover(sender, e);
        }

        private void MonthCalendar_LabelMouseMove(object sender, MouseEventArgs e)
        {
            OnLabelMouseMove(sender, e);
        }

        private void MonthCalendar_LabelMouseLeave(object sender, EventArgs e)
        {
            OnLabelMouseLeave(sender, e);
        }

        private void MonthCalendar_LabelMouseEnter(object sender, EventArgs e)
        {
            OnLabelMouseEnter(sender, e);
        }

        private void MonthCalendar_LabelMouseUp(object sender, MouseEventArgs e)
        {
            OnLabelMouseUp(sender, e);
        }

        private void MonthCalendar_LabelMouseDown(object sender, MouseEventArgs e)
        {
            OnLabelMouseDown(sender, e);
        }

        private void MonthCalendar_PaintLabel(object sender, DayPaintEventArgs e)
        {
            OnPaintLabel(sender, e);
        }
        private void MonthCalendar_MonthChanging(object sender, EventArgs e)
        {
            OnMonthChanging(e);
        }
        private void MonthCalendar_MonthChanged(object sender, EventArgs e)
        {
            OnMonthChanged(e);
        }

        //protected override Rectangle GetItemContainerRectangle()
        //{
        //    Rectangle r = base.GetItemContainerRectangle();
        //    ElementStyle style = GetBackgroundStyle();
        //    if (style != null)
        //    {
        //        if (style.PaintBorder && (style.CornerType == eCornerType.Rounded || style.CornerType == eCornerType.Diagonal))
        //            r.Inflate(-style.BorderWidth / 2, -style.BorderWidth / 2);
        //    }

        //    return r;
        //}

        /// <summary>
        /// Returns the DayLabel at given client coordinates or null/nothing if there is no label at give location.
        /// </summary>
        /// <param name="x">X - position in client coordinates.</param>
        /// <param name="y">Y - position in client coordinates.</param>
        /// <returns>DayLabel at given coordinates or null/nothing.</returns>
        public DayLabel GetDayAt(int x, int y)
        {
            return _MonthCalendar.GetDayAt(x, y);
        }

        /// <summary>
        /// Returns the DayLabel that represents the date.
        /// </summary>
        /// <param name="date">Date to find label for.</param>
        /// <returns>DayLabel object or nothing if date cannot be founds.</returns>
        public DayLabel GetDay(DateTime date)
        {
            return _MonthCalendar.GetDay(date);
        }

        /// <summary>
        /// Gets or sets the array of DateTime objects that determine which monthly days to mark using Colors.MonthlyMarker settings. 
        /// Make sure to call UpdateMarkedDates() method to update calendar display with marked dates.
        /// </summary>
        [DefaultValue(null), Localizable(true), Description("Indicates array of DateTime objects that determine which monthly days to mark using Colors.MonthlyMarker settings. ")]
        public DateTime[] MonthlyMarkedDates
        {
            get { return _MonthCalendar.MonthlyMarkedDates; }
            set { _MonthCalendar.MonthlyMarkedDates = value; }
        }
        /// <summary>
        /// Returns whether property should be serialized. Provided for Windows Forms designer support.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeMonthlyMarkedDates()
        {
            return _MonthCalendar.ShouldSerializeMonthlyMarkedDates();
        }
        /// <summary>
        /// Resets property to its default value. Provided for Windows Forms designer support.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetMonthlyMarkedDates()
        {
            _MonthCalendar.ResetMonthlyMarkedDates();
        }

        /// <summary>
        /// Removes all monthly marked dates. Note that you must call UpdateMarkedDates method to reflect these changes on calendar.
        /// </summary>
        public void RemoveAllMonthlyMarkedDates()
        {
            _MonthCalendar.RemoveAllMonthlyMarkedDates();
        }

        /// <summary>
        /// Removes the date from the MonthlyMarkedDates. Note that you must call UpdateMarkedDates method to reflect these changes on calendar.
        /// </summary>
        /// <param name="dt"></param>
        public void RemoveMonthlyMarkedDate(DateTime dt)
        {
            _MonthCalendar.RemoveMonthlyMarkedDate(dt);
        }

        /// <summary>
        /// Gets or sets the array of DayOfWeek members that determine which days of week to mark using Colors.WeeklyMarker settings. 
        /// </summary>
        [DefaultValue(null), Localizable(true), Description("Indicates array of DateTime objects that determine which monthly days to mark using Colors.MonthlyMarker settings. ")]
        public DayOfWeek[] WeeklyMarkedDays
        {
            get { return _MonthCalendar.WeeklyMarkedDays; }
            set { _MonthCalendar.WeeklyMarkedDays = value; }
        }
        /// <summary>
        /// Returns whether property should be serialized. Provided for Windows Forms designer support.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeWeeklyMarkedDays()
        {
            return _MonthCalendar.ShouldSerializeWeeklyMarkedDays();
        }
        /// <summary>
        /// Resets property to its default value. Provided for Windows Forms designer support.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetWeeklyMarkedDays()
        {
            _MonthCalendar.ResetWeeklyMarkedDays();
        }

        /// <summary>
        /// Removes all weekly marked dates. Note that you must call UpdateMarkedDates method to reflect these changes on calendar.
        /// </summary>
        public void RemoveAllWeeklyMarkedDays()
        {
            _MonthCalendar.RemoveAllWeeklyMarkedDays();
        }

        /// <summary>
        /// Removes the day from the WeeklyMarkedDays. Note that you must call UpdateMarkedDates method to reflect these changes on calendar.
        /// </summary>
        /// <param name="dt"></param>
        public void RemoveWeeklyMarkedDay(DayOfWeek dt)
        {
            _MonthCalendar.RemoveWeeklyMarkedDay(dt);
        }

        /// <summary>
        /// Gets or sets the array of DateTime objects that determines which annual days are marked using Colors.AnnualMarker settings.
        /// Make sure to call UpdateMarkedDates() method to update calendar display with marked dates.
        /// </summary>
        [DefaultValue(null), Localizable(true), Description("Indicates array of DateTime objects that determines which annual days are marked using Colors.AnnualMarker settings.")]
        public DateTime[] AnnuallyMarkedDates
        {
            get { return _MonthCalendar.AnnuallyMarkedDates; }
            set { _MonthCalendar.AnnuallyMarkedDates = value; }
        }
        /// <summary>
        /// Returns whether property should be serialized. Provided for Windows Forms designer support.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeAnnuallyMarkedDates()
        {
            return _MonthCalendar.ShouldSerializeAnnuallyMarkedDates();
        }
        /// <summary>
        /// Resets property to its default value. Provided for Windows Forms designer support.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetAnnuallyMarkedDates()
        {
            _MonthCalendar.ResetAnnuallyMarkedDates();
        }

        /// <summary>
        /// Removes all annually marked dates. Note that you must call UpdateMarkedDates method to reflect these changes on calendar.
        /// </summary>
        public void RemoveAllAnnuallyMarkedDates()
        {
            _MonthCalendar.RemoveAllAnnuallyMarkedDates();
        }

        /// <summary>
        /// Removes the date from the AnnuallyMarkedDates. Note that you must call UpdateMarkedDates method to reflect these changes on calendar.
        /// </summary>
        /// <param name="dt"></param>
        public void RemoveAnnuallyMarkedDate(DateTime dt)
        {
            _MonthCalendar.RemoveAnnuallyMarkedDate(dt);
        }

        /// <summary>
        /// Gets or sets the array of DateTime objects that determines which non-recurring dates are marked using Colors.DayMarker settings. 
        /// Make sure to call UpdateMarkedDates() method to update calendar display with marked dates.
        /// </summary>
        [DefaultValue(null), Localizable(true), Description("Indicates array of DateTime objects that determines which non-recurring dates are marked using Colors.DayMarker settings.")]
        public DateTime[] MarkedDates
        {
            get { return _MonthCalendar.MarkedDates; }
            set { _MonthCalendar.MarkedDates = value; }
        }
        /// <summary>
        /// Returns whether property should be serialized. Provided for Windows Forms designer support.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeMarkedDates()
        {
            return _MonthCalendar.ShouldSerializeMarkedDates();
        }
        /// <summary>
        /// Resets property to its default value. Provided for Windows Forms designer support.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetMarkedDates()
        {
            _MonthCalendar.ResetMarkedDates();
        }

        /// <summary>
        /// Removes all marked dates set through MarkedDates property. Note that you must call UpdateMarkedDates method to reflect these changes on calendar.
        /// </summary>
        public void RemoveAllMarkedDates()
        {
            _MonthCalendar.RemoveAllMarkedDates();
        }

        /// <summary>
        /// Removes the date from the MarkedDates collection. Note that you must call UpdateMarkedDates method to reflect these changes on calendar.
        /// </summary>
        /// <param name="dt"></param>
        public void RemoveMarkedDate(DateTime dt)
        {
            _MonthCalendar.RemoveMarkedDate(dt);
        }

        /// <summary>
        /// Repaints the marked dates to reflect the dates set in the lists of marked dates.
        /// Use this method to reflect the changes made to the AnnuallyMarkedDates, MonthlyMarkedDates or MarkedDates properties as well
        /// as change to the marked Colors properties.
        /// </summary>
        public void UpdateMarkedDates()
        {
            _MonthCalendar.UpdateMarkedDates();
        }

        internal bool GetDesignModeInternal()
        {
            return DesignMode;
        }

        /// <summary>
        /// Gets or sets the first month displayed by the control.
        /// </summary>
        [Description("Indicates the first month displayed on the control.")]
        public DateTime DisplayMonth
        {
            get { return _MonthCalendar.DisplayMonth; }
            set { _MonthCalendar.DisplayMonth = value; }
        }

        /// <summary>
        /// Gets number of months displayed on the control.
        /// </summary>
        [Browsable(false)]
        public int DisplayedMonthCount
        {
            get
            {
                return _MonthCalendar.DisplayedMonthCount;
            }
        }

        /// <summary>
        /// Gets the calendar colors used by the control.
        /// </summary>
        [NotifyParentPropertyAttribute(true), Category("Appearance"), Description("Gets colors used by control."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public MonthCalendarColors Colors
        {
            get { return _MonthCalendar.Colors; }
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

        /// <summary>
        /// Raises the MonthChanged event.
        /// </summary>
        /// <param name="e">Provides additional event data.</param>
        protected virtual void OnMonthChanged(EventArgs e)
        {
            if (MonthChanged != null)
                MonthChanged(this, e);
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

        /// <summary>
        /// Raises the PaintLabel event.
        /// </summary>
        /// <param name="e">Provides event data.</param>
        protected virtual void OnPaintLabel(object label, DayPaintEventArgs e)
        {
            if (PaintLabel != null)
                PaintLabel(label, e);
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

        /// <summary>
        /// Gets or sets the size of each day item on the calendar. Default value is 24, 15.
        /// </summary>
        [Description("Indicate size of each day item on the calendar.")]
        public Size DaySize
        {
            get { return _MonthCalendar.DaySize; }
            set
            {
                _MonthCalendar.DaySize = value;
                InvalidateAutoSize();
            }
        }
        /// <summary>
        /// Gets whether property should be serialized. Provided for designer support.
        /// </summary>
        /// <returns></returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeDaySize()
        {
            return _MonthCalendar.ShouldSerializeDaySize();
        }
        /// <summary>
        /// Reset property to default value. Provided for designer support.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetDaySize()
        {
            _MonthCalendar.ResetDaySize();
        }

        /// <summary>
        /// Gets or sets the minimum date and time that can be selected in the control.
        /// </summary>
        [Description("Indicates minimum date and time that can be selected in the control.")]
        public System.DateTime MinDate
        {
            get { return _MonthCalendar.MinDate; }
            set { _MonthCalendar.MinDate = value; }
        }
        /// <summary>
        /// Gets whether Value property should be serialized by Windows Forms designer.
        /// </summary>
        /// <returns>true if value serialized otherwise false.</returns>
        public bool ShouldSerializeMinDate()
        {
            return _MonthCalendar.ShouldSerializeMinDate();
        }
        /// <summary>
        /// Reset the MinDate property to its default value.
        /// </summary>
        public void ResetMinDate()
        {
            _MonthCalendar.ResetMinDate();
        }

        /// <summary>
        /// Gets or sets the maximum date and time that can be selected in the control.
        /// </summary>
        [Description("Indicates maximum date and time that can be selected in the control.")]
        public System.DateTime MaxDate
        {
            get { return _MonthCalendar.MaxDate; }
            set { _MonthCalendar.MaxDate = value; }
        }
        /// <summary>
        /// Gets whether Value property should be serialized by Windows Forms designer.
        /// </summary>
        /// <returns>true if value serialized otherwise false.</returns>
        public bool ShouldSerializeMaxDate()
        {
            return _MonthCalendar.ShouldSerializeMaxDate();
        }
        /// <summary>
        /// Reset the MaxDate property to its default value.
        /// </summary>
        public void ResetMaxDate()
        {
            _MonthCalendar.ResetMaxDate();
        }

        /// <summary>
        /// Specifies the commands container background style. Commands container displays Today and Clear buttons if they are visible.
        /// </summary>
        [Browsable(true), Category("Style"), Description("Specifies the commands container background style. Commands container displays Today and Clear buttons if they are visible."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ElementStyle CommandsBackgroundStyle
        {
            get { return _MonthCalendar.CommandsBackgroundStyle; }
        }

        /// <summary>
        /// Specifies the navigation container background style. Navigation container displays month, year and optional buttons. Default value is an empty style which means that container does not display any background.
        /// BeginGroup property set to true will override this style on some styles.
        /// </summary>
        [Browsable(true), Category("Style"), Description("Indicates navigation container background style."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ElementStyle NavigationBackgroundStyle
        {
            get { return _MonthCalendar.NavigationBackgroundStyle; }
        }

        /// <summary>
        /// Gets or sets the number of columns and rows of months displayed on control. Default value is 0,0 which indicates that
        /// calendar will display as many columns and rows as it is possible to fit into container space available.
        /// </summary>
        [Description("Indicates number of columns and rows of months displayed on control.")]
        public Size CalendarDimensions
        {
            get { return _MonthCalendar.CalendarDimensions; }
            set { _MonthCalendar.CalendarDimensions = value; InvalidateAutoSize(); }
        }
        /// <summary>
        /// Resets property to its default value. Provided for design-time support.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetCalendarDimensions()
        {
            _MonthCalendar.ResetCalendarDimensions();
            InvalidateAutoSize();
        }
        /// <summary>
        /// Gets whether property should be serialized. Provided for design-time support.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeCalendarDimensions()
        {
            return _MonthCalendar.ShouldSerializeCalendarDimensions();
        }

        /// <summary>
        /// Sets the number of columns and rows of months to display. 
        /// </summary>
        /// <param name="columns">The number of columns. </param>
        /// <param name="rows">The number of rows. </param>
        /// <remarks>ArgumentOutOfRangeException will be raised if any value is less than zero or one value is grater than zero and other is zero.</remarks>
        public void SetCalendarDimensions(int columns, int rows)
        {
            _MonthCalendar.SetCalendarDimensions(columns, rows);
        }

        /// <summary>
        /// Gets or sets whether weekend days can be selected. Default value is true.
        /// </summary>
        [DefaultValue(true), Description("Indicates whether weekend days can be selected.")]
        public bool WeekendDaysSelectable
        {
            get { return _MonthCalendar.WeekendDaysSelectable; }
            set { _MonthCalendar.WeekendDaysSelectable = value; }
        }

        /// <summary>
        /// Gets or sets the rule used to determine first week of the year for week of year display on calendar. Default value is first-day.
        /// </summary>
        [DefaultValue(CalendarWeekRule.FirstDay), Description("Indicates rule used to determine first week of the year for week of year display on calendar.")]
        public CalendarWeekRule WeekOfYearRule
        {
            get { return _MonthCalendar.WeekOfYearRule; }
            set { _MonthCalendar.WeekOfYearRule = value; }
        }

        /// <summary>
        /// Gets or sets the value that is used by calendar as today's date.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DateTime TodayDate
        {
            get { return _MonthCalendar.TodayDate; }
            set { _MonthCalendar.TodayDate = value; }
        }

        /// <summary>
        /// Gets a value indicating whether the TodayDate property has been explicitly set. 
        /// </summary>
        [Browsable(false)]
        public bool TodayDateSet
        {
            get { return _MonthCalendar.TodayDateSet; }
        }

        /// <summary>
        /// Gets or sets whether today marker that indicates TodayDate is visible on the calendar. Default value is true.
        /// </summary>
        [DefaultValue(true), Description("Indicates whether today marker that indicates TodayDate is visible on the calendar.")]
        public bool ShowTodayMarker
        {
            get { return _MonthCalendar.ShowTodayMarker; }
            set { _MonthCalendar.ShowTodayMarker = value; }
        }

        /// <summary>
        /// Gets or sets whether week of year is visible. Default value is false.
        /// </summary>
        [DefaultValue(false), Description("Indicates whether week of year is visible.")]
        public bool ShowWeekNumbers
        {
            get { return _MonthCalendar.ShowWeekNumbers; }
            set { _MonthCalendar.ShowWeekNumbers = value; InvalidateAutoSize(); }
        }

        /// <summary>
        /// Gets or sets the array of custom names for days displayed on calendar header. The array must have exactly 7 elements representing day names from 0 to 6.
        /// </summary>
        [DefaultValue(null), Description("Indicates array of custom names for days displayed on calendar header."), Localizable(true)]
        public string[] DayNames
        {
            get { return _MonthCalendar.DayNames; }
            set { _MonthCalendar.DayNames = value; }
        }

        /// <summary>
        /// Gets or sets the first day of week displayed on the calendar. Default value is Sunday.
        /// </summary>
        [DefaultValue(DayOfWeek.Sunday), Description("Indicates first day of week displayed on the calendar.")]
        public DayOfWeek FirstDayOfWeek
        {
            get { return _MonthCalendar.FirstDayOfWeek; }
            set { _MonthCalendar.FirstDayOfWeek = value; }
        }

        /// <summary>
        /// Gets or sets whether control uses the two letter day names. Default value is true.
        /// </summary>
        [DefaultValue(true), Description("Indicates whether control uses the two letter day names.")]
        public bool TwoLetterDayName
        {
            get { return _MonthCalendar.TwoLetterDayName; }
            set
            {
                _MonthCalendar.TwoLetterDayName = value;
            }
        }

        /// <summary>
        /// Gets or sets the calendar selected date. Note that SelectedDate property should be used only when MultiSelect property is set to false.
        /// When multiple dates can be selected use range selection properties: SelectionStart, SelectionEnd and SelectionRange.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DateTime SelectedDate
        {
            get
            {
                return _MonthCalendar.SelectedDate;
            }
            set
            {
                _MonthCalendar.SelectedDate = value;
            }
        }

        /// <summary>
        /// Gets or sets whether selection of multiple dates up to the MaxSelectionCount is enabled. Default value is false which indicates that only
        /// single day can be selected.
        /// </summary>
        [DefaultValue(false), Description("Indicates whether selection of multiple dates up to the MaxSelectionCount is enabled.")]
        public bool MultiSelect
        {
            get { return _MonthCalendar.MultiSelect; }
            set { _MonthCalendar.MultiSelect = value; }
        }

        /// <summary>
        /// Gets the reference to the bottom container that parents the Today, and Clear system buttons.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ItemContainer BottomContainer
        {
            get
            {
                return _MonthCalendar.BottomContainer;
            }
        }

        /// <summary>
        /// Gets or sets whether Today button displayed at the bottom of the calendar is visible. Default value is false.
        /// </summary>
        [DefaultValue(false), Description("Indicates whether Today button displayed at the bottom of the calendar is visible.")]
        public bool TodayButtonVisible
        {
            get { return _MonthCalendar.TodayButtonVisible; }
            set { _MonthCalendar.TodayButtonVisible = value; }
        }

        /// <summary>
        /// Gets or sets whether Clear button displayed at the bottom of the calendar is visible. Clear button clears the currently selected date. Default value is false.
        /// </summary>
        [DefaultValue(false), Description("Indicates whether Clear button displayed at the bottom of the calendar is visible. Clear button clears the currently selected date.")]
        public bool ClearButtonVisible
        {
            get { return _MonthCalendar.ClearButtonVisible; }
            set { _MonthCalendar.ClearButtonVisible = value; }
        }

        /// <summary>
        /// Gets or sets the maximum number of days that can be selected in a month calendar control. 
        /// </summary>
        [DefaultValue(7), Description("Gets or sets the maximum number of days that can be selected in a month calendar control.")]
        public int MaxSelectionCount
        {
            get { return _MonthCalendar.MaxSelectionCount; }
            set { _MonthCalendar.MaxSelectionCount = value; }
        }

        /// <summary>
        /// Gets or sets the start date of the selected range of dates. 
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DateTime SelectionStart
        {
            get { return _MonthCalendar.SelectionStart; }
            set
            {
                _MonthCalendar.SelectionStart = value;
            }
        }

        /// <summary>
        /// Gets or sets the end date of the selected range of dates. 
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DateTime SelectionEnd
        {
            get { return _MonthCalendar.SelectionEnd; }
            set
            {
                _MonthCalendar.SelectionEnd = value;
            }
        }

        /// <summary>
        /// Gets or sets the selected range of dates for a month calendar control. 
        /// <remarks>
        /// Setting this property is functionally equivalent to using the SetSelectionRange method. You can set the start and end dates separately by setting either the SelectionStart or SelectionEnd properties. You cannot change the start and end dates by setting the SelectionRange.Start or SelectionRange.End property values of the SelectionRange property. You should use SelectionStart, SelectionEnd, or SetSelectionRange.
        /// If the Start property value of the SelectionRange is greater than its End property value, the dates are swapped; the End property value becomes the starting date, and Start property value becomes the end date.</remarks>
        /// </summary>
        [Bindable(true), Description("Indicates selected range of dates for a month calendar control. ")]
        public SelectionRange SelectionRange
        {
            get
            {
                return _MonthCalendar.SelectionRange;
            }
            set
            {
                _MonthCalendar.SelectionRange = value;
            }
        }
        /// <summary>
        /// Returns whether property should be serialized by Windows Forms designer.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeSelectionRange()
        {
            return _MonthCalendar.ShouldSerializeSelectionRange();
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
            _MonthCalendar.SetSelectionRange(startDate, endDate);
        }

        private Size _PreferredSize = Size.Empty;
        /// <summary>
        /// Invalidates control auto-size and resizes the control if AutoSize is set to true.
        /// </summary>
        public void InvalidateAutoSize()
        {
            _PreferredSize = Size.Empty;
            AdjustSize();
        }
#if FRAMEWORK20
        /// <summary>
        /// Gets or sets a value indicating whether the control is automatically resized to display its entire contents. You can set MaximumSize.Width property to set the maximum width used by the control.
        /// </summary>
        [Browsable(true), DefaultValue(false), EditorBrowsable(EditorBrowsableState.Always), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public override bool AutoSize
        {
            get
            {
                return base.AutoSize;
            }
            set
            {
                if (this.AutoSize != value)
                {
                    base.AutoSize = value;
                    AdjustSize();
                }
            }
        }

        private void AdjustSize()
        {
            if (this.AutoSize)
            {
                this.Size = base.PreferredSize;
            }
        }

        protected override void OnFontChanged(EventArgs e)
        {
            InvalidateAutoSize();
            base.OnFontChanged(e);
        }

        public override Size GetPreferredSize(Size proposedSize)
        {
            if (!_PreferredSize.IsEmpty) return _PreferredSize;

            if (!BarFunctions.IsHandleValid(this))
                return base.GetPreferredSize(proposedSize);

            ElementStyle style = this.GetBackgroundStyle();

            _MonthCalendar.RecalcSize();
            _PreferredSize = _MonthCalendar.Size;
            if (style != null)
            {
                _PreferredSize.Width += ElementStyleLayout.HorizontalStyleWhiteSpace(style);
                _PreferredSize.Height += ElementStyleLayout.VerticalStyleWhiteSpace(style);
            }

            return _PreferredSize;
        }

        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            if (this.AutoSize)
            {
                Size preferredSize = base.PreferredSize;
                width = preferredSize.Width;
                height = preferredSize.Height;
            }
            base.SetBoundsCore(x, y, width, height, specified);
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            if (this.AutoSize)
                this.AdjustSize();
            base.OnHandleCreated(e);
        }

        [Browsable(false)]
        public override bool ThemeAware
        {
            get
            {
                return base.ThemeAware;
            }
            set
            {
                base.ThemeAware = value;
            }
        }
#endif
        #endregion

    }
}
#endif

