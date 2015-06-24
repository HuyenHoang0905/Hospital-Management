#if FRAMEWORK20
using System;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Drawing;
using DevComponents.DotNetBar.Controls;
using DevComponents.Schedule.Model;
using DevComponents.DotNetBar.Rendering;
using System.Collections.ObjectModel;

namespace DevComponents.DotNetBar.Schedule
{
    [ToolboxItem(true), ToolboxBitmap(typeof(CalendarView), "Schedule.CalendarView.ico")]
    [Designer("DevComponents.DotNetBar.Design.CalendarViewDesigner, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
    public class CalendarView : ItemControl
    {
        #region Constants

        private const int MinutesPerHour = 60;
        private const int HoursPerDay = 24;
        private const int DaysPerYear = 365;

        private const int MinutesPerDay = MinutesPerHour * HoursPerDay;
        private const int MinutesPerYear = MinutesPerDay * DaysPerYear;

        private const int MaxMinuteDuration = 60;
        private const int MaxHourDuration = 24;
        private const int MaxDayDuration = 365;
        private const int MaxYearDuration = 10;

        #endregion

        #region Events

        #region BaseView Events

        /// <summary>
        /// Occurs when CalendarModel has changed
        /// </summary>
        [Description("Occurs when CalendarModel has changed.")]
        public event EventHandler<ModelEventArgs> ModelChanged;

        /// <summary>
        /// Occurs when DateSelectionStart has changed
        /// </summary>
        [Description("Occurs when DateSelectionStart has changed.")]
        public event EventHandler<DateSelectionEventArgs> DateSelectionStartChanged;

        /// <summary>
        /// Occurs when DateSelectionEnd has changed
        /// </summary>
        [Description("Occurs when DateSelectionEnd has changed.")]
        public event EventHandler<DateSelectionEventArgs> DateSelectionEndChanged;

        /// <summary>
        /// Occurs when the Day, Week, or Month view date(s) have changed
        /// </summary>
        [Description("Occurs when the Day, Week, or Month view date(s) have changed.")]
        public event EventHandler<ViewDateChangedEventArgs> ViewDateChanged;

        /// <summary>
        /// Occurs when SelectedView has Changed
        /// </summary>
        [Description("Occurs when SelectedView has Changed.")]
        public event EventHandler<SelectedViewEventArgs> SelectedViewChanged;

        /// <summary>
        /// Occurs when SelectedOwner has Changed
        /// </summary>
        [Description("Occurs when SelectedOwner has Changed.")]
        public event EventHandler<SelectedOwnerChangedEventArgs> SelectedOwnerChanged;

        /// <summary>
        /// Occurs when EnableDragDrop has changed
        /// </summary>
        [Description("Occurs when EnableDragDrop has changed.")]
        public event EventHandler<EnableDragDropChangedEventArgs> EnableDragDropChanged;

        /// <summary>
        /// Occurs when DisplayedOwners has Changed
        /// </summary>
        [Description("Occurs when DisplayedOwners has Changed.")]
        public event EventHandler<EventArgs> DisplayedOwnersChanged;

        /// <summary>
        /// Occurs when TimeIndicators has changed
        /// </summary>
        [Description("Occurs when TimeIndicators has changed.")]
        public event EventHandler<EventArgs> TimeIndicatorsChanged;

        /// <summary>
        /// Occurs when a TimeIndicator time has changed
        /// </summary>
        [Description("Occurs when a TimeIndicator time has changed.")]
        public event EventHandler<TimeIndicatorTimeChangedEventArgs> TimeIndicatorTimeChanged;

        /// <summary>
        /// Occurs when a TimeIndicator Color has changed
        /// </summary>
        [Description("Occurs when a TimeIndicator Color has changed.")]
        public event EventHandler<TimeIndicatorColorChangedEventArgs> TimeIndicatorColorChanged;

        /// <summary>
        /// Occurs when ViewDisplayCustomizations have changed
        /// </summary>
        [Description("Occurs when ViewDisplayCustomizations have changed.")]
        public event EventHandler<EventArgs> ViewDisplayCustomizationsChanged;

        /// <summary>
        /// Occurs when a Tab's background needs rendered
        /// </summary>
        [Description("Occurs when a Tab's background needs rendered.")]
        public event EventHandler<RenderTabBackgroundEventArgs> RenderTabBackground;
        
        /// <summary>
        /// Occurs when a Tab's content (text, etc) needs rendered
        /// </summary>
        [Description("Occurs when a Tab's content (text, etc)  needs rendered.")]
        public event EventHandler<RenderTabContentEventArgs> RenderTabContent;

        /// <summary>
        /// Occurs when a view load/reload has occurred
        /// </summary>
        [Description("Occurs when a view load/reload has occurred.")]
        public event EventHandler<ViewLoadCompleteEventArgs> ViewLoadComplete;
   
        #endregion

        #region AppointmentView Events

        /// <summary>
        /// Occurs before an AppointmentView is about to change
        /// </summary>
        [Description("Occurs before an AppointmentView is about to change.")]
        public event EventHandler<BeforeAppointmentViewChangeEventArgs> BeforeAppointmentViewChange;

        /// <summary>
        /// Occurs when an AppointmentView is changing
        /// </summary>
        [Description("Occurs when an AppointmentView is changing.")]
        public event EventHandler<AppointmentViewChangingEventArgs> AppointmentViewChanging;

        /// <summary>
        /// Occurs when an AppointmentView has changed
        /// </summary>
        [Description("Occurs when an AppointmentView has changed.")]
        public event EventHandler<AppointmentViewChangedEventArgs> AppointmentViewChanged;

        /// <summary>
        /// Occurs when Appointment Reminder has been reached.")]
        /// </summary>
        [Description("Occurs when Appointment Reminder has been reached.")]
        public event EventHandler<ReminderEventArgs> AppointmentReminder;

        /// <summary>
        /// Occurs when Appointment StartTime has been reached.")]
        /// </summary>
        [Description("Occurs when Appointment StartTime has been reached.")]
        public event EventHandler<AppointmentEventArgs> AppointmentStartTimeReached;

        /// <summary>
        /// Occurs when Appointment DisplayTemplate text is needed
        /// </summary>
        [Description("Occurs when Appointment DisplayTemplate text is needed.")]
        public event EventHandler<GetDisplayTemplateTextEventArgs> GetDisplayTemplateText;

        #endregion

        #region DayView Events

        /// <summary>
        /// Occurs when DayViewDate has Changed
        /// </summary>
        [Description("Occurs when DayViewDate has Changed.")]
        public event EventHandler<DateChangeEventArgs> DayViewDateChanged;

        #endregion

        #region WeekView Events

        /// <summary>
        /// Occurs when WeekViewStartDate has Changed
        /// </summary>
        [Description("Occurs when WeekViewStartDate has Changed.")]
        public event EventHandler<DateChangeEventArgs> WeekViewStartDateChanged;

        /// <summary>
        /// Occurs when WeekViewEndDate has Changed
        /// </summary>
        [Description("Occurs when WeekViewEndDate has Changed.")]
        public event EventHandler<DateChangeEventArgs> WeekViewEndDateChanged;

        #endregion

        #region WeekDayView Events

        /// <summary>
        /// Occurs when LabelTimeSlots has changed
        /// </summary>
        [Description("Occurs when LabelTimeSlots has changed.")]
        public event EventHandler<LabelTimeSlotsChangedEventArgs> LabelTimeSlotsChanged;

        /// <summary>
        /// Occurs when TimeSlotDuration has changed
        /// </summary>
        [Description("Occurs when TimeSlotDuration has changed.")]
        public event EventHandler<TimeSlotDurationChangedEventArgs> TimeSlotDurationChanged;

        /// <summary>
        /// Occurs when Is24HourFormat has changed
        /// </summary>
        [Description("Occurs when Is24HourFormat has changed.")]
        public event EventHandler<Is24HourFormatChangedEventArgs> Is24HourFormatChanged;

        /// <summary>
        /// Occurs when FixedAllDayPanelHeight has changed
        /// </summary>
        [Description("Occurs when FixedAllDayPanelHeight has changed.")]
        public event EventHandler<FixedAllDayPanelHeightChangedEventArgs> FixedAllDayPanelHeightChanged;

        /// <summary>
        /// Occurs when MaximumAllDayPanelHeight has Changed
        /// </summary>
        [Description("Occurs when MaximumAllDayPanelHeight has Changed.")]
        public event EventHandler<MaximumAllDayPanelHeightChangedEventArgs> MaximumAllDayPanelHeightChanged;

        /// <summary>
        /// Occurs when IsTimeRulerVisible has changed
        /// </summary>
        [Description("Occurs when IsTimeRulerVisible has changed.")]
        public event EventHandler<IsTimeRulerVisibleChangedEventArgs> IsTimeRulerVisibleChanged;

        /// <summary>
        /// Occurs when ShowOnlyWorkDayHours has changed
        /// </summary>
        [Description("Occurs when ShowOnlyWorkDayHours has changed.")]
        public event EventHandler<ShowOnlyWorkDayHoursChangedEventArgs> ShowOnlyWorkDayHoursChanged;

        /// <summary>
        /// Occurs when DaySlotAppearance Text needs rendered
        /// </summary>
        [Description("Occurs when DaySlotAppearance Text needs rendered.")]
        public event EventHandler<RenderDaySlotAppearanceTextEventArgs> RenderDaySlotAppearanceText;

        #endregion

        #region MonthView Events

        /// <summary>
        /// Occurs when MonthViewStartDate has Changed
        /// </summary>
        [Description("Occurs when MonthViewStartDate has Changed.")]
        public event EventHandler<DateChangeEventArgs> MonthViewStartDateChanged;

        /// <summary>
        /// Occurs when MonthViewEndDate has Changed
        /// </summary>
        [Description("Occurs when MonthViewEndDate has Changed.")]
        public event EventHandler<DateChangeEventArgs> MonthViewEndDateChanged;

        /// <summary>
        /// Occurs when IsMonthSideBarVisible has changed
        /// </summary>
        [Description("Occurs when IsMonthSideBarVisible has changed.")]
        public event EventHandler<IsMonthSideBarVisibleChangedEventArgs> IsMonthSideBarVisibleChanged;

        /// <summary>
        /// Occurs when IsMonthMoreItemsIndicatorVisible has changed
        /// </summary>
        [Description("Occurs when IsMonthMoreItemsIndicatorVisible has changed.")]
        public event EventHandler<IsMonthMoreItemsIndicatorVisibleChangedEventArgs> IsMonthMoreItemsIndicatorVisibleChanged;

        /// <summary>
        /// Occurs when a MonthView Slot Background is about to be rendered
        /// </summary>
        [Description("Occurs when a MonthView Slot Background is about to be rendered.")]
        public event EventHandler<MonthViewPreRenderSlotBackgroundEventArgs> MonthViewPreRenderSlotBackground;

        /// <summary>
        /// Occurs when a MonthView Slot Background has just been rendered
        /// </summary>
        [Description("Occurs when a MonthView Slot Background has just been rendered.")]
        public event EventHandler<MonthViewPostRenderSlotBackgroundEventArgs> MonthViewPostRenderSlotBackground;

        #endregion

        #region YearView Events

        /// <summary>
        /// Occurs when YearViewStartDate has Changed
        /// </summary>
        [Description("Occurs when YearViewStartDate has Changed.")]
        public event EventHandler<DateChangeEventArgs> YearViewStartDateChanged;

        /// <summary>
        /// Occurs when YearViewEndDate has Changed
        /// </summary>
        [Description("Occurs when YearViewEndDate has Changed.")]
        public event EventHandler<DateChangeEventArgs> YearViewEndDateChanged;

        /// <summary>
        /// Occurs when YearViewShowGridLines has changed
        /// </summary>
        [Description("Occurs when YearViewShowGridLines has changed.")]
        public event EventHandler<ShowGridLinesChangedEventArgs> YearViewShowGridLinesChanged;

        /// <summary>
        /// Occurs when YearViewAllowDateSelection has changed
        /// </summary>
        [Description("Occurs when YearViewAllowDateSelection has changed.")]
        public event EventHandler<AllowDateSelectionChangedEventArgs> YearViewAllowDateSelectionChanged;

        /// <summary>
        /// Occurs when YearViewLinkView has changed
        /// </summary>
        [Description("Occurs when YearViewLinkView has changed.")]
        public event EventHandler<LinkViewChangedEventArgs> YearViewLinkViewChanged;

        /// <summary>
        /// Occurs when YearViewAppointmentLink has changed
        /// </summary>
        [Description("Occurs when YearViewAppointmentLink has changed.")]
        public event EventHandler<DayLinkChangedEventArgs> YearViewAppointmentLinkChanged;

        /// <summary>
        /// Occurs when YearViewLinkAction has changed
        /// </summary>
        [Description("Occurs when YearViewLinkAction has changed.")]
        public event EventHandler<LinkViewActionChangedEventArgs> YearViewLinkActionChanged;

        /// <summary>
        /// Occurs when YearViewNonAppointmentLink has changed
        /// </summary>
        [Description("Occurs when YearViewNonAppointmentLink has changed.")]
        public event EventHandler<DayLinkChangedEventArgs> YearViewNonAppointmentLinkChanged;

        /// <summary>
        /// Occurs when YearViewLinkStyle has changed
        /// </summary>
        [Description("Occurs when YearViewLinkStyle has changed.")]
        public event EventHandler<LinkViewStyleChangedEventArgs> YearViewLinkStyleChanged;

        /// <summary>
        /// Occurs when YearViewLink has been selected
        /// </summary>
        [Description("Occurs when YearViewLink has been selected.")]
        public event EventHandler<LinkViewSelectedEventArgs> YearViewLinkSelected;

        /// <summary>
        /// Occurs when YearView Day Background needs drawn
        /// </summary>
        [Description("Occurs when YearView Day Background needs drawn.")]
        public event EventHandler<YearViewDrawDayBackgroundEventArgs> YearViewDrawDayBackground;

        /// <summary>
        /// Occurs when YearView Day Text needs drawn
        /// </summary>
        [Description("Occurs when YearView Day Text needs drawn.")]
        public event EventHandler<YearViewDrawDayTextEventArgs> YearViewDrawDayText;

        #endregion

        #region TimeLine Events

        /// <summary>
        /// Occurs when TimeLineViewStartDate has Changed
        /// </summary>
        [Description("Occurs when TimeLineViewStartDate has Changed.")]
        public event EventHandler<DateChangeEventArgs> TimeLineViewStartDateChanged;

        /// <summary>
        /// Occurs when TimeLineViewEndDate has Changed
        /// </summary>
        [Description("Occurs when TimeLineViewEndDate has Changed.")]
        public event EventHandler<DateChangeEventArgs> TimeLineViewEndDateChanged;

        /// <summary>
        /// Occurs when TimeLineViewViewScrollDate has Changed
        /// </summary>
        [Description("Occurs when TimeLineViewViewScrollDate has Changed.")]
        public event EventHandler<DateChangeEventArgs> TimeLineViewScrollDateChanged;

        /// <summary>
        /// Occurs when TimeLineInterval has Changed
        /// </summary>
        [Description("Occurs when TimeLineInterval has Changed.")]
        public event EventHandler<TimeLineIntervalChangedEventArgs> TimeLineIntervalChanged;

        /// <summary>
        /// Occurs when TimeLineIntervalPeriod has Changed
        /// </summary>
        [Description("Occurs when TimeLineIntervalPeriod has Changed.")]
        public event EventHandler<TimeLineIntervalPeriodChangedEventArgs> TimeLineIntervalPeriodChanged;

        /// <summary>
        /// Occurs when TimeLineColumnWidth has changed
        /// </summary>
        [Description("Occurs when TimeLineColumnWidth has changed.")]
        public event EventHandler<TimeLineColumnWidthChangedEventArgs> TimeLineColumnWidthChanged;

        /// <summary>
        /// Occurs when TimeLineMaxColumnCount has changed
        /// </summary>
        [Description("Occurs when TimeLineMaxColumnCount has changed.")]
        public event EventHandler<TimeLineMaxColumnCountChangedEventArgs> TimeLineMaxColumnCountChanged;

        /// <summary>
        /// Occurs when TimeLineShowPeriodHeader has changed
        /// </summary>
        [Description("Occurs when TimeLineShowPeriodHeader has changed.")]
        public event EventHandler<TimeLineShowPeriodHeaderChangedEventArgs> TimeLineShowPeriodHeaderChanged;

        /// <summary>
        /// Occurs when TimeLineShowIntervalHeader has changed
        /// </summary>
        [Description("Occurs when TimeLineShowIntervalHeader has changed.")]
        public event EventHandler<TimeLineShowIntervalHeaderChangedEventArgs> TimeLineShowIntervalHeaderChanged;

        /// <summary>
        /// Occurs when TimeLineShowPageNavigation has changed
        /// </summary>
        [Description("Occurs when TimeLineShowPageNavigation has changed.")]
        public event EventHandler<TimeLineShowPageNavigationChangedEventArgs> TimeLineShowPageNavigationChanged;
        
        /// <summary>
        /// Occurs when TimeLineCondensedViewVisibility has changed
        /// </summary>
        [Description("Occurs when TimeLineCondensedViewVisibility has changed.")]
        public event EventHandler<TimeLineCondensedViewVisibilityChangedEventArgs> TimeLineCondensedViewVisibilityChanged;

        /// <summary>
        /// Occurs when TimeLineCondensedViewHeight has changed
        /// </summary>
        [Description("Occurs when TimeLineCondensedViewHeight has changed.")]
        public event EventHandler<TimeLineCondensedViewHeightChangedEventArgs> TimeLineCondensedViewHeightChanged;

        /// <summary>
        /// Occurs when TimeLineView Period Header needs rendered
        /// </summary>
        [Description("Occurs when TimeLineView Period Header needs rendered.")]
        public event EventHandler<TimeLineRenderPeriodHeaderEventArgs> TimeLineRenderPeriodHeader;

        /// <summary>
        /// Occurs when a TimeLineView Slot Background is about to be rendered
        /// </summary>
        [Description("Occurs when a TimeLineView Slot Background is about to be rendered.")]
        public event EventHandler<TimeLinePreRenderSlotBackgroundEventArgs> TimeLinePreRenderSlotBackground;

        /// <summary>
        /// Occurs when a TimeLineView Slot Background has just been rendered
        /// </summary>
        [Description("Occurs when a TimeLineView Slot Background has just been rendered.")]
        public event EventHandler<TimeLinePostRenderSlotBackgroundEventArgs> TimeLinePostRenderSlotBackground;

        /// <summary>
        /// Occurs when a TimeLineView Slot Border needs rendered
        /// </summary>
        [Description("Occurs when a TimeLineView Slot Border needs rendered.")]
        public event EventHandler<TimeLineRenderSlotBorderEventArgs> TimeLineRenderSlotBorder;

        /// <summary>
        /// Occurs when TimeLineView needs to get the Appointment row height
        /// </summary>
        [Description("Occurs when TimeLineView needs to get the Appointment row height.")]
        public event EventHandler<TimeLineGetRowHeightEventArgs> TimeLineGetRowHeight;

        /// <summary>
        /// Occurs when TimeLineView needs to get the row collate Id (used to group rows).
        /// </summary>
        [Description("Occurs when TimeLineView needs to get the row collate Id (used to group rows).")]
        public event EventHandler<TimeLineGetRowCollateIdEventArgs> TimeLineGetRowCollateId;

        /// <summary>
        /// Occurs when a PageNavigator control button has been clicked
        /// </summary>
        [Description("Occurs when a PageNavigator control button has been clicked.")]
        public event EventHandler<PageNavigatorClickEventArgs> PageNavigatorClick;

        #endregion

        #endregion

        #region Private variables

        #region Base variables

        private CalendarModel _CalendarModel;               // Calendar Model
        private CalendarPanel _CalendarPanel;               // Calendar Panel

        private DateTime _AutoSyncDate;                     // AutoSync Start Date
        private DateTime? _DateSelectionStart;              // Selection start
        private DateTime? _DateSelectionEnd;                // Selection ent

        private eCalendarView _SelectedView = eCalendarView.None;   // Selected view

        private DisplayedOwnerCollection _DisplayedOwners;
        private int _SelectedOwnerIndex = -1;               // Selected owner index
        private int _ViewWidth = 250;

        private bool _AllowTabReorder = true;               // Can reorder multi-user tabs?
        private bool _AutoSyncViewDates = true;             // View dates AutoSynced?
        private bool _EnableDragDrop = true;                // DragDrop enabled state
        private bool _EnableMarkup = true;
        private bool _SetCursor;                            // Local cursor set flag
        private bool _HighlightCurrentDay;

        private Cursor _DefaultViewCursor;                  // Default View cursor
        private int _DefaultCalendarColor;

        private AppointmentCategoryColorCollection _CategoryColors;

        private TimeIndicatorCollection _TimeIndicators;
        private TimeIndicator _TimeIndicator;

        private CustomCalendarItemCollection
            _CustomItems = new CustomCalendarItemCollection();

        private ViewDisplayCustomizations _ViewDisplayCustomizations;

        private bool _ShowTabs = true;
        private int _AppointmentBorderWidth = 1;

        #endregion

        #region WeekDay variables

        private DayView _DayView;
        private DateTime _DayViewDate;

        private WeekView _WeekView;
        private DateTime _WeekViewStartDate;
        private DateTime _WeekViewEndDate;

        private CalendarViewCollection<DayView> _MultiCalendarDayViews;
        private CalendarViewCollection<WeekView> _MultiCalendarWeekViews;

        private bool _Is24HourFormat;
        private bool _IsTimeRulerVisible = true;
        private bool _LabelTimeSlots;
        private bool _ShowOnlyWorkDayHours;

        private int _TimeSlotDuration = 30;
        private int _FixedAllDayPanelHeight = -1;
        private int _MaximumAllDayPanelHeight = 120;
        private int _NumberOfSlices;
        private int _StartSlice;
        private int _SlotsPerHour;
        private float _TimeSliceHeight;

        private TimeRulerPanel _TimeRulerPanel;
        private WeekDayVScrollPanel _WeekDayVScrollPanel;

        #endregion

        #region Month variables

        private MonthView _MonthView;
        private DateTime _MonthViewStartDate;
        private DateTime _MonthViewEndDate;

        private CalendarViewCollection<MonthView> _MultiCalendarMonthViews;

        private bool _IsMonthSideBarVisible = true;
        private bool _IsMonthMoreItemsIndicatorVisible = true;

        #endregion

        #region YearView variables

        private YearView _YearView;
        private DateTime _YearViewStartDate;
        private DateTime _YearViewEndDate;

        private CalendarViewCollection<YearView> _MultiCalendarYearViews;

        private bool _YearViewShowGridLines = true;
        private bool _YearViewAllowDateSelection = true;

        private eCalendarView _YearViewLinkView = eCalendarView.Day;
        private eYearViewLinkAction _YearViewLinkAction = eYearViewLinkAction.GoToDate;
        private eYearViewDayLink _YearViewAppointmentLink = eYearViewDayLink.Click;
        private eYearViewDayLink _YearViewNonAppointmentLink = eYearViewDayLink.DoubleClick;
        private eYearViewLinkStyle _YearViewAppointmentLinkStyle = eYearViewLinkStyle.Style1;

        private YearVScrollPanel _YearVScrollPanel;

        private int _YearViewMax;

        #endregion

        #region TimeLineView variables

        private TimeLineView _TimeLineView;
        private DateTime _TimeLineViewStartDate;
        private DateTime _TimeLineViewEndDate;

        private CalendarViewCollection<TimeLineView> _MultiCalendarTimeLineViews;

        private bool _TimeLinePeriodHeaderEnableMarkup = true;
        private bool _TimeLineShowPeriodHeader = true;
        private bool _TimeLineShowIntervalHeader = true;
        private bool _TimeLineShowPageNavigation = true;
        private bool _TimeLineStretchRowHeight;
        private bool _TimeLineCanExtendRange = true;
        private bool _TimeLineShowCollateLines;

        private int _TimeLineColumnCount;
        private int _TimeLineMaxColumnCount = 500;
        private int _TimeLineColumnWidth = 80;
        private int _TimeLineHeight = 100;
        private int _TimeLineCondensedViewHeight = 20;
        private int _TimeLinePeriodHeaderHeight = -1;
        private int _TimeLineInterval = 30;
        private double _BaseInterval = 30;

        private eOrientation _TimeLineMultiUserTabOrientation = eOrientation.Vertical;
        private int _TimeLineMultiUserTabWidth = 80;

        private DateTime _TimeLineViewScrollStartDate;
        private TimeLineHeaderPanel _TimeLineHeaderPanel;
        private TimeLineHScrollPanel _TimeLineHScrollPanel;

        private eItemAlignment _TimeLinePeriodHeaderAlignment = eItemAlignment.Center;

        private eCondensedViewVisibility
            _TimeLineCondensedViewVisibility = eCondensedViewVisibility.AllResources;

        private eTimeLinePeriod _TimeLinePeriod = eTimeLinePeriod.Minutes;

        private string _TimeLinePageNavigatorTodayTooltip = "Today";
        private string _TimeLinePageNavigatorNextPageTooltip = "Next Page";
        private string _TimeLinePageNavigatorPreviousPageTooltip = "Previous Page";

        #endregion

        #endregion

        /// <summary>
        /// CalendarView constructor
        /// </summary>
        public CalendarView()
        {
            // Allocate our CalendarPanel to hold all our
            // BaseView and TimeRulerPanel objects

            _CalendarPanel = new CalendarPanel(this);

            _CalendarPanel.Bounds = ClientRectangle;

            _CalendarPanel.GlobalItem = false;
            _CalendarPanel.ContainerControl = this;
            _CalendarPanel.Stretch = false;
            _CalendarPanel.Displayed = true;
            _CalendarPanel.SetOwner(this);

            SetBaseItemContainer(_CalendarPanel);

            // Allocate our DisplayedOwners collection, used
            // to track all multi-user calendar views

            _DisplayedOwners = new DisplayedOwnerCollection(this);

            // Establish our default view dates and
            // set our default view to single-user Day

            InitDefaultViews();
            SelectedView = eCalendarView.Day;

            // Register with the StyleManager

            StyleManager.Register(this);
            CalendarPanel.Style = eDotNetBarStyle.StyleManagerControlled;

            // Alloc our default CalendarModel

            CalendarModel = new CalendarModel();
        }

        #region DefaultSize

        protected override Size DefaultSize
        {
            get
            {
                return new Size(300, 150);
            }
        }
        #endregion

        #region Public properties

        #region Licensing
#if !TRIAL
        private string _LicenseKey = "";
        [Browsable(false), DefaultValue("")]
        public string LicenseKey
        {
            get { return _LicenseKey; }
            set
            {
                if (NativeFunctions.ValidateLicenseKey(value))
                    return;
                _LicenseKey = (!NativeFunctions.CheckLicenseKey(value) ? "9dsjkhds7" : value);
            }
        }
#endif
        #endregion

        #region Browsable properties

        #region AllowTabReorder

        /// <summary>
        /// Gets and sets whether the control will permit
        /// tab reordering via the user interface
        /// </summary>
        [DefaultValue(true)]
        [Category("Behavior")]
        [Description("Indicates whether the user can reorder Multi-User tabs through the user interface.")]
        public bool AllowTabReorder
        {
            get { return (_AllowTabReorder); }
            set { _AllowTabReorder = value; }
        }

        #endregion

        #region AutoSyncViewDates
        /// <summary>
        /// Gets or sets whether view dates are automatically
        /// synced to the currently viewed date range
        /// </summary>
        [DefaultValue(true)]
        [Category("Behavior")]
        [Description("Indicates whether view dates are automatically synced to the currently viewed date range.")]
        public bool AutoSyncViewDates
        {
            get { return (_AutoSyncViewDates); }
            set { _AutoSyncViewDates = value; }
        }

        #endregion

        #region AppointmentBorderWidth

        /// <summary>
        /// Gets and sets the default appointment border width
        /// </summary>
        [DefaultValue(1)]
        [Category("Appearance")]
        [Description("Indicates whether the default appointment border width.")]
        public int AppointmentBorderWidth
        {
            get { return (_AppointmentBorderWidth); }

            set
            {
                if (_AppointmentBorderWidth != value)
                {
                    _AppointmentBorderWidth = value;

                    Refresh();
                }
            }
        }

        #endregion

        #region EnableMarkup

        /// <summary>
        /// Gets or sets whether Appointment text-markup support is enabled
        /// </summary>
        [DefaultValue(true), Category("Appearance")]
        [Description("Indicates whether Appointment text-markup support is enabled.")]
        public bool EnableMarkup
        {
            get { return (_EnableMarkup); }

            set
            {
                if (_EnableMarkup != value)
                {
                    _EnableMarkup = value;
                    Refresh();
                }
            }
        }

        #endregion

        #region SelectedView

        /// <summary>
        /// Gets and sets the selected calendar view
        /// </summary>
        [DefaultValue(eCalendarView.Day)]
        [Category("Appearance")]
        [Description("Indicates whether the Day, Week, or Month view is displayed.")]
        public eCalendarView SelectedView
        {
            get { return (_SelectedView); }

            set
            {
                if (_SelectedView != value)
                {
                    eCalendarView oldView = _SelectedView;

                    SetSelectedView(value);

                    OnSelectedViewChanged(oldView, _SelectedView);
                }
            }
        }

        /// <summary>
        /// SelectedViewChanged event propagation
        /// </summary>
        protected virtual void OnSelectedViewChanged(eCalendarView oldView, eCalendarView newView)
        {
            if (SelectedViewChanged != null)
                SelectedViewChanged(this, new SelectedViewEventArgs(oldView, newView));
        }

        #endregion

        #region FixedAllDayPanelHeight

        /// <summary>
        /// Gets and sets the fixed (constant) AllDayPanel
        /// height for all WeekDay views. Setting this value to -1
        /// will let the height change dynamically
        /// </summary>
        [DefaultValue(-1)]
        [Category("Layout")]
        [Description("Indicates the fixed (constant) AllDayPanel height for all Day and Week views. Setting this value to -1 will let the height change dynamically.")]
        public int FixedAllDayPanelHeight
        {
            get { return (_FixedAllDayPanelHeight); }

            set
            {
                if (_FixedAllDayPanelHeight != value)
                {
                    int oldValue = _FixedAllDayPanelHeight;
                    _FixedAllDayPanelHeight = value;

                    OnFixedAllDayPanelHeightChanged(oldValue, value);
                }
            }
        }

        /// <summary>
        /// Propagates FixedAllDayPanelHeightChanged events
        /// </summary>
        /// <param name="oldValue">Old value</param>
        /// <param name="newValue">New value</param>
        private void OnFixedAllDayPanelHeightChanged(int oldValue, int newValue)
        {
            if (FixedAllDayPanelHeightChanged != null)
            {
                FixedAllDayPanelHeightChanged(this,
                    new FixedAllDayPanelHeightChangedEventArgs(oldValue, newValue));
            }
        }

        #endregion

        #region HilightCurrentDay

        /// <summary>
        /// Gets or sets whether the current calendar day is highlighted
        /// </summary>
        [DefaultValue(false)]
        [Category("Appearance")]
        [Description("Indicates whether the current calendar day is highlighted.")]
        public bool HighlightCurrentDay
        {
            get { return (_HighlightCurrentDay); }

            set
            {
                if (_HighlightCurrentDay != value)
                {
                    _HighlightCurrentDay = value;

                    Refresh();
                }
            }
        }

        #endregion

        #region MaximumAllDayPanelHeight

        /// <summary>
        /// Gets or sets the maximum height of the All Day Appointment panel
        /// </summary>
        [DefaultValue(120)]
        [Category("Layout")]
        [Description("Indicates the maximum AllDayPanel height for all Day and Week views.")]
        public int MaximumAllDayPanelHeight
        {
            get { return (_MaximumAllDayPanelHeight); }

            set
            {
                if (_MaximumAllDayPanelHeight != value)
                {
                    int oldValue = _MaximumAllDayPanelHeight;
                    _MaximumAllDayPanelHeight = value;

                    OnMaximumAllDayPanelHeightChanged(oldValue, value);
                }
            }
        }

        /// <summary>
        /// Propagates MaximumAllDayPanelHeightChanged events
        /// </summary>
        /// <param name="oldValue">Old value</param>
        /// <param name="newValue">New value</param>
        private void OnMaximumAllDayPanelHeightChanged(int oldValue, int newValue)
        {
            if (MaximumAllDayPanelHeightChanged != null)
            {
                MaximumAllDayPanelHeightChanged(this,
                    new MaximumAllDayPanelHeightChangedEventArgs(oldValue, newValue));
            }
        }

        #endregion

        #region TimeIndicator

        /// <summary>
        /// Gets or sets the default TimeIndicator
        /// </summary>
        [Browsable(true)]
        [Description("Default CalendarView TimeIndicator.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TimeIndicator TimeIndicator
        {
            get
            {
                if (_TimeIndicator == null)
                    TimeIndicator = new TimeIndicator();

                return (_TimeIndicator);
            }
 
            set
            {
                if (_TimeIndicator != null)
                {
                    _TimeIndicator.IsProtected = false;

                    TimeIndicators.Remove(_TimeIndicator);
                }

                _TimeIndicator = value;

                if (value != null)
                {
                    _TimeIndicator.IsProtected = true;
                    _TimeIndicator.IsDesignMode = DesignMode;

                    TimeIndicators.Add(_TimeIndicator);
                }
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetTimeIndicator()
        {
            TimeIndicator = new TimeIndicator();
        }

        #endregion

        #region TimeSlotDuration

        /// <summary>
        /// Gets and sets the Time slot duration for all Day and Week views.
        /// This value, in minutes, must be greater than 0 and less than or
        /// equal to 30. Set values must also evenly divide the hour. This means
        /// that values like 6, 10 and 20 are valid values, whereas 7, 11 and 31 are not
        /// </summary>
        [DefaultValue(MaxMinuteDuration)]
        [Category("Layout")]
        [Description("Indicates the Time Slot Duration for all Day and Week views. This value, in minutes, must be greater than 0 and less than or equal to 30. Set values must also evenly divide the hour. This means that values like 6, 10 and 20 are valid values, whereas 7, 11 and 31 are not.")]
        public int TimeSlotDuration
        {
            get { return (_TimeSlotDuration); }

            set
            {
                value = GetValidIntervalMinutes(value);

                if (_TimeSlotDuration != value)
                {
                    int oldValue = _TimeSlotDuration;
                    _TimeSlotDuration = value;

                    _SlotsPerHour = MinutesPerHour / _TimeSlotDuration;

                    OnTimeSlotDurationChanged(oldValue, value);
                }
            }
        }

        /// <summary>
        /// OnTimeSlotDurationChanged event propagation
        /// </summary>
        protected virtual void OnTimeSlotDurationChanged(int oldVal, int newVal)
        {
            if (TimeSlotDurationChanged != null)
                TimeSlotDurationChanged(this, new TimeSlotDurationChangedEventArgs(oldVal, newVal));
        }

        #endregion

        #region Is24HourFormat

        /// <summary>
        /// Gets and sets the 12 or 24 hour
        /// formatting that is used in the Day and Week views
        /// </summary>
        [DefaultValue(false)]
        [Category("Appearance")]
        [Description("Indicates whether 12 or 24 hour formatting is used in the Day and Week views.")]
        public bool Is24HourFormat
        {
            get { return (_Is24HourFormat); }

            set
            {
                if (_Is24HourFormat != value)
                {
                    _Is24HourFormat = value;

                    OnIs24HourFormatChanged(!value, value);
                }
            }
        }

        /// <summary>
        /// OnIs24HourFormatChanged event propagation
        /// </summary>
        private void OnIs24HourFormatChanged(bool oldValue, bool newValue)
        {
            if (Is24HourFormatChanged != null)
                Is24HourFormatChanged(this, new Is24HourFormatChangedEventArgs(oldValue, newValue));
        }

        #endregion

        #region LabelTimeSlots

        /// <summary>
        /// Gets and sets whether time slot labels are
        /// displayed in the Day and Week view TimeRulerPanel
        /// </summary>
        [DefaultValue(false)]
        [Category("Appearance")]
        [Description("Indicates whether minor time labels are displayed in the Day and Week view TimeRulerPanel.")]
        public bool LabelTimeSlots
        {
            get { return (_LabelTimeSlots); }

            set
            {
                if (_LabelTimeSlots != value)
                {
                    bool oldValue = _LabelTimeSlots;
                    _LabelTimeSlots = value;

                    OnLabelTimeSlotsChanged(oldValue, value);
                }
            }
        }

        /// <summary>
        /// OnLabelTimeSlotsChanged event propagation
        /// </summary>
        protected virtual void OnLabelTimeSlotsChanged(bool oldVal, bool newVal)
        {
            if (LabelTimeSlotsChanged != null)
                LabelTimeSlotsChanged(this, new LabelTimeSlotsChangedEventArgs(oldVal, newVal));
        }

        #endregion

        #region ShowOnlyWorkDayHours

        /// <summary>
        /// Gets and sets whether only WorkDay hours are
        /// displayed in the Day and Week views
        /// </summary>
        [DefaultValue(false)]
        [Category("Appearance")]
        [Description("Indicates whether only WorkDay hours are displayed in the Day and Week views.")]
        public bool ShowOnlyWorkDayHours
        {
            get { return (_ShowOnlyWorkDayHours); }

            set
            {
                if (_ShowOnlyWorkDayHours != value)
                {
                    bool oldValue = _ShowOnlyWorkDayHours;
                    _ShowOnlyWorkDayHours = value;

                    CalendarPanel.RecalcSize();

                    OnShowOnlyWorkDayHoursChanged(oldValue, value);
                }
            }
        }

        /// <summary>
        /// OnShowOnlyWorkDayHoursChanged event propagation
        /// </summary>
        protected virtual void OnShowOnlyWorkDayHoursChanged(bool oldVal, bool newVal)
        {
            if (ShowOnlyWorkDayHoursChanged != null)
                ShowOnlyWorkDayHoursChanged(this, new ShowOnlyWorkDayHoursChangedEventArgs(oldVal, newVal));
        }

        #endregion

        #region ShowTabs

        /// <summary>
        /// Gets and sets multi-user tab visibility
        /// </summary>
        [Browsable(true), DefaultValue(true), Category("Appearance")]
        [Description("Indicates multi-user tab visibility.")]
        public bool ShowTabs
        {
            get { return (_ShowTabs); }

            set
            {
                if (_ShowTabs != value)
                {
                    _ShowTabs = value;

                    CalendarPanel.NeedRecalcSize = true;
                    CalendarPanel.Refresh();
                }
            }
        }

        #endregion

        #region IsMonthSideBarVisible

        /// <summary>
        /// Gets and sets the default Month view SideBar visibility
        /// </summary>
        [DefaultValue(true)]
        [Category("Behavior")]
        [Description("Indicates the default Month view SideBar visibility.")]
        public bool IsMonthSideBarVisible
        {
            get { return (_IsMonthSideBarVisible); }

            set
            {
                if (_IsMonthSideBarVisible != value)
                {
                    bool oldValue = _IsMonthSideBarVisible;
                    _IsMonthSideBarVisible = value;

                    OnIsMonthSideBarVisibleChanged(oldValue, value);

                    Refresh();
                }
            }
        }

        /// <summary>
        /// OnIsMonthSideBarVisibleChanged event propagation
        /// </summary>
        protected virtual void OnIsMonthSideBarVisibleChanged(bool oldVal, bool newVal)
        {
            if (IsMonthSideBarVisibleChanged != null)
                IsMonthSideBarVisibleChanged(this, new IsMonthSideBarVisibleChangedEventArgs(oldVal, newVal));
        }

        #endregion

        #region IsMonthMoreItemsIndicatorVisible

        /// <summary>
        /// Gets and sets the Month view 'More Items' indicator visibility
        /// </summary>
        [DefaultValue(true)]
        [Category("Behavior")]
        [Description("Indicates the Month view 'More Items' indicator visibility.")]
        public bool IsMonthMoreItemsIndicatorVisible
        {
            get { return (_IsMonthMoreItemsIndicatorVisible); }

            set
            {
                if (_IsMonthMoreItemsIndicatorVisible != value)
                {
                    bool oldValue = _IsMonthMoreItemsIndicatorVisible;
                    _IsMonthMoreItemsIndicatorVisible = value;

                    OnIsMonthMoreItemsIndicatorVisibleChanged(oldValue, value);

                    Refresh();
                }
            }
        }

        /// <summary>
        /// OnIsMonthMoreItemsIndicatorVisibleChanged event propagation
        /// </summary>
        protected virtual void OnIsMonthMoreItemsIndicatorVisibleChanged(bool oldVal, bool newVal)
        {
            if (IsMonthMoreItemsIndicatorVisibleChanged != null)
                IsMonthMoreItemsIndicatorVisibleChanged(this, new IsMonthMoreItemsIndicatorVisibleChangedEventArgs(oldVal, newVal));
        }

        #endregion

        #region IsTimeRulerVisible

        /// <summary>
        /// Gets and sets whether the Week/Day view TimeRuler is visible
        /// </summary>
        [DefaultValue(true)]
        [Category("Behavior")]
        [Description("Indicates whether the Week/Day view TimeRuler is visible.")]
        public bool IsTimeRulerVisible
        {
            get { return (_IsTimeRulerVisible); }

            set
            {
                if (_IsTimeRulerVisible != value)
                {
                    bool oldValue = _IsTimeRulerVisible;
                    _IsTimeRulerVisible = value;

                    OnIsTimeRulerVisibleChanged(oldValue, value);

                    SetSelectedView(SelectedView);
                }
            }
        }

        /// <summary>
        /// OnIsTimeRulerVisibleChanged event propagation
        /// </summary>
        protected virtual void OnIsTimeRulerVisibleChanged(bool oldVal, bool newVal)
        {
            if (IsTimeRulerVisibleChanged != null)
                IsTimeRulerVisibleChanged(this, new IsTimeRulerVisibleChangedEventArgs(oldVal, newVal));
        }

        #endregion

        #region EnableDragDrop

        /// <summary>
        /// Gets and sets whether DragDrop across calendar views is enabled
        /// </summary>
        [DefaultValue(true)]
        [Category("Behavior")]
        [Description("Indicates whether DragDrop operations are enabled between calendar views.")]
        public bool EnableDragDrop
        {
            get { return (_EnableDragDrop); }

            set
            {
                if (_EnableDragDrop != value)
                {
                    _EnableDragDrop = value;

                    OnEnableDragDropChanged(!value, value);
                }
            }
        }

        /// <summary>
        /// OnEnableDragDropChanged event propagation
        /// </summary>
        protected virtual void OnEnableDragDropChanged(bool oldVal, bool newVal)
        {
            if (EnableDragDropChanged != null)
                EnableDragDropChanged(this, new EnableDragDropChangedEventArgs(oldVal, newVal));
        }

        #endregion

        #region YearViewAllowDateSelection

        /// <summary>
        /// Gets and sets whether date selection is permitted
        /// </summary>
        [Browsable(true), DefaultValue(true), Category("Behavior")]
        [Description("Indicates whether YearView date selection is permitted.")]
        public bool YearViewAllowDateSelection
        {
            get { return (_YearViewAllowDateSelection); }

            set
            {
                if (_YearViewAllowDateSelection != value)
                {
                    bool oldValue = _YearViewAllowDateSelection;
                    _YearViewAllowDateSelection = value;

                    OnYearViewAllowDateSelectionChanged(oldValue, value);
                }
            }
        }

        /// <summary>
        /// YearViewAllowDateSelectionChanged event propagation
        /// </summary>
        protected virtual void OnYearViewAllowDateSelectionChanged(bool oldVal, bool newVal)
        {
            if (YearViewAllowDateSelectionChanged != null)
                YearViewAllowDateSelectionChanged(this, new AllowDateSelectionChangedEventArgs(oldVal, newVal));
        }

        #endregion

        #region YearViewAppointmentLink

        /// <summary>
        /// Gets and sets the YearView Appointment Link mode. This defines the
        /// interaction between the mouse and YearView days that contain appointments.
        /// </summary>
        [Browsable(true), DefaultValue(eYearViewDayLink.Click), Category("Behavior")]
        [Description("Indicates the YearView Appointment Link mode. This defines the interaction between the mouse and YearView days that contain appointments.")]
        public eYearViewDayLink YearViewAppointmentLink
        {
            get { return (_YearViewAppointmentLink); }

            set
            {
                if (_YearViewAppointmentLink != value)
                {
                    eYearViewDayLink oldValue = _YearViewAppointmentLink;
                    _YearViewAppointmentLink = value;

                    OnYearViewAppointmentLinkChanged(oldValue, value);
                }
            }
        }

        /// <summary>
        /// YearViewAppointmentLinkChanged event propagation
        /// </summary>
        private void OnYearViewAppointmentLinkChanged(eYearViewDayLink oldVal, eYearViewDayLink newVal)
        {
            if (YearViewAppointmentLinkChanged != null)
                YearViewAppointmentLinkChanged(this, new DayLinkChangedEventArgs(oldVal, newVal));
        }

        #endregion

        #region YearViewNonAppointmentLink

        /// <summary>
        /// Gets and sets the YearView Non-Appointment Link mode. This defines the
        /// interaction between the mouse and YearView days that do not contain appointments.
        /// </summary>
        [Browsable(true), DefaultValue(eYearViewDayLink.DoubleClick), Category("Behavior")]
        [Description("Indicates the YearView Non-Appointment Link mode. This defines the interaction between the mouse and YearView days that do not contain appointments.")]
        public eYearViewDayLink YearViewNonAppointmentLink
        {
            get { return (_YearViewNonAppointmentLink); }

            set
            {
                if (_YearViewNonAppointmentLink != value)
                {
                    eYearViewDayLink oldValue = _YearViewNonAppointmentLink;
                    _YearViewNonAppointmentLink = value;

                    OnYearViewNonAppointmentLinkChanged(oldValue, value);
                }
            }
        }

        /// <summary>
        /// YearViewNonAppointmentLinkChanged event propagation
        /// </summary>
        private void OnYearViewNonAppointmentLinkChanged(eYearViewDayLink oldVal, eYearViewDayLink newVal)
        {
            if (YearViewNonAppointmentLinkChanged != null)
                YearViewNonAppointmentLinkChanged(this, new DayLinkChangedEventArgs(oldVal, newVal));
        }

        #endregion

        #region YearViewLinkView

        /// <summary>
        /// Gets or sets the Link Calendar View. This defines the
        /// View that is activated when a YearView date 'link' is selected.
        /// </summary>
        [Browsable(true), DefaultValue(eCalendarView.Day), Category("Behavior")]
        [Description("Indicates the Link Calendar View. This defines the View that is activated when a YearView date 'link' is selected.")]
        public eCalendarView YearViewLinkView
        {
            get { return (_YearViewLinkView); }

            set
            {
                if (_YearViewLinkView != value)
                {
                    eCalendarView oldValue = _YearViewLinkView;
                    _YearViewLinkView = value;

                    OnYearYearViewLinkViewChanged(oldValue, value);
                }
            }
        }

        /// <summary>
        /// YearYearViewLinkViewChanged event propagation
        /// </summary>
        protected virtual void OnYearYearViewLinkViewChanged(eCalendarView oldVal, eCalendarView newVal)
        {
            if (YearViewLinkViewChanged != null)
                YearViewLinkViewChanged(this, new LinkViewChangedEventArgs(oldVal, newVal));
        }

        #endregion

        #region YearViewLinkAction

        /// <summary>
        /// Gets or sets the Link action. This defines the
        /// action that is taken when a YearView date 'link' is selected.
        /// </summary>
        [Browsable(true), DefaultValue(eYearViewLinkAction.GoToDate), Category("Behavior")]
        [Description("Indicates the Link Calendar View. This defines the View that is activated when a YearView date 'link' is selected.")]
        public eYearViewLinkAction YearViewLinkAction
        {
            get { return (_YearViewLinkAction); }

            set
            {
                if (_YearViewLinkAction != value)
                {
                    eYearViewLinkAction oldValue = _YearViewLinkAction;
                    _YearViewLinkAction = value;

                    OnYearViewLinkActionChanged(oldValue, value);
                }
            }
        }

        /// <summary>
        /// YearViewLinkActionChanged event propagation
        /// </summary>
        private void OnYearViewLinkActionChanged(eYearViewLinkAction oldVal, eYearViewLinkAction newVal)
        {
            if (YearViewLinkActionChanged != null)
                YearViewLinkActionChanged(this, new LinkViewActionChangedEventArgs(oldVal, newVal));
        }

        #endregion

        #region YearViewAppointmentLinkStyle

        /// <summary>
        /// Gets or sets the AppointmentLink display style. This defines the
        /// style that is used when 'highlighting' YearView date links.
        /// </summary>
        [Browsable(true), DefaultValue(eYearViewLinkStyle.Style1), Category("Appearance")]
        [Description("Indicates the AppointmentLink display style. This defines the style that is used when 'highlighting' YearView date links.")]
        public eYearViewLinkStyle YearViewAppointmentLinkStyle
        {
            get { return (_YearViewAppointmentLinkStyle); }

            set
            {
                if (_YearViewAppointmentLinkStyle != value)
                {
                    eYearViewLinkStyle oldValue = _YearViewAppointmentLinkStyle;
                    _YearViewAppointmentLinkStyle = value;

                    OnYearViewLinkStyleChanged(oldValue, value);

                    Refresh();
                }
            }
        }

        /// <summary>
        /// YearViewLinkStyleChanged event propagation
        /// </summary>
        private void OnYearViewLinkStyleChanged(eYearViewLinkStyle oldVal, eYearViewLinkStyle newVal)
        {
            if (YearViewLinkStyleChanged != null)
                YearViewLinkStyleChanged(this, new LinkViewStyleChangedEventArgs(oldVal, newVal));
        }

        #endregion

        #region YearViewShowGridLines

        /// <summary>
        /// Gets and sets the YearView grid lines visibility
        /// </summary>
        [Browsable(true), DefaultValue(true), Category("Behavior")]
        [Description("Indicates the YearView grid lines visibility.")]
        public bool YearViewShowGridLines
        {
            get { return (_YearViewShowGridLines); }

            set
            {
                if (_YearViewShowGridLines != value)
                {
                    bool oldValue = _YearViewShowGridLines;
                    _YearViewShowGridLines = value;

                    OnYearViewShowGridLinesChanged(oldValue, value);

                    Refresh();
                }
            }
        }

        /// <summary>
        /// OnYearViewShowGridLinesChanged event propagation
        /// </summary>
        protected virtual void OnYearViewShowGridLinesChanged(bool oldVal, bool newVal)
        {
            if (YearViewShowGridLinesChanged != null)
                YearViewShowGridLinesChanged(this, new ShowGridLinesChangedEventArgs(oldVal, newVal));
        }

        #endregion

        #endregion

        #region Non-Browsable properties

        #region CalendarModel

        /// <summary>
        /// Gets and sets the calendar Model
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CalendarModel CalendarModel
        {
            get { return (_CalendarModel); }

            set
            {
                if (_CalendarModel != value)
                {
                    if (_CalendarModel != null)
                        HookModelEvents(false);

                    CalendarModel oldModel = _CalendarModel;
                    _CalendarModel = value;

                    HookModelEvents(true);

                    OnModelChanged(oldModel, value);
                }
            }
        }

        /// <summary>
        /// OnModelChanged event propagation
        /// </summary>
        protected virtual void OnModelChanged(CalendarModel oldModel, CalendarModel newModel)
        {
            if (ModelChanged != null)
                ModelChanged(this, new ModelEventArgs(oldModel, newModel));
        }

        #endregion

        #region CategoryColors

        /// <summary>
        /// Appointment CategoryColors
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public AppointmentCategoryColorCollection CategoryColors
        {
            get
            {
                if (_CategoryColors == null)
                {
                    _CategoryColors = new AppointmentCategoryColorCollection();

                    _CategoryColors.AppointmentCategoryColorCollectionChanged +=
                        CategoryColorsAppointmentCategoryColorCollectionChanged;
                }

                return (_CategoryColors);
            }
        }
            
        #endregion

        #region CustomItems

        /// <summary>
        /// Gets the collection of user defined custom
        /// CalendarItems
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CustomCalendarItemCollection CustomItems
        {
            get { return (_CustomItems); }
        }

        #endregion

        #region DateSelectionStart

        /// <summary>
        /// Gets or sets the selection start date
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DateTime? DateSelectionStart
        {
            get { return (_DateSelectionStart); }

            set
            {
                if (_DateSelectionStart != value)
                {
                    DateTime? oldDate = _DateSelectionStart;
                    _DateSelectionStart = value;

                    OnDateSelectionStartChanged(oldDate, value);
                }
            }
        }

        /// <summary>
        /// OnDateSelectionStartChanged event propagation
        /// </summary>
        protected virtual void OnDateSelectionStartChanged(DateTime? oldDate, DateTime? newDate)
        {
            if (DateSelectionStartChanged != null)
                DateSelectionStartChanged(this, new DateSelectionEventArgs(oldDate, newDate));
        }

        #endregion

        #region DateSelectionEnd

        /// <summary>
        /// Gets or sets the end date selection
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DateTime? DateSelectionEnd
        {
            get { return (_DateSelectionEnd); }

            set
            {
                if (_DateSelectionEnd != value)
                {
                    DateTime? oldDate = _DateSelectionEnd;
                    _DateSelectionEnd = value;

                    OnDateSelectionEndChanged(oldDate, value);
                }
            }
        }

        /// <summary>
        /// OnDateSelectionEndChanged event propagation
        /// </summary>
        protected virtual void OnDateSelectionEndChanged(DateTime? oldDate, DateTime? newDate)
        {
            if (DateSelectionEndChanged != null)
                DateSelectionEndChanged(this, new DateSelectionEventArgs(oldDate, newDate));
        }

        #endregion

        #region DayView properties

        /// <summary>
        /// Gets the Day View
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DayView DayView
        {
            get
            {
                if (_DayView == null)
                {
                    // Single-user DayView is not accessible
                    // when in multi-user mode

                    if (IsMultiCalendar == false)
                    {
                        _DayView = new DayView(this);

                        _DayView.StartDate = _DayViewDate;
                        _DayView.EndDate = _DayViewDate;

                        _DayView.CalendarColor = GetViewColor(_DayView, 0);
                    }
                }

                return (_DayView);
            }

            internal set
            {
                if (_DayView != value)
                {
                    if (_DayView != null)
                        _DayView.Dispose();

                    _DayView = value;
                }
            }
        }

        #region DayViewDate

        /// <summary>
        /// Gets and sets the DayView date
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DateTime DayViewDate
        {
            get { return (_DayViewDate); }

            set
            {
                value = value.Date;

                if (_DayViewDate != value)
                {
                    DateTime oldDate = _DayViewDate;
                    _DayViewDate = value;

                    // Inform interested parties of the change

                    OnDayViewDateChanged(oldDate, value);
                    OnViewDateChanged(eCalendarView.Day, oldDate, value, oldDate, value);

                    // Save AutoSyncDate

                    _AutoSyncDate = value;
                }
            }
        }

        /// <summary>
        /// Sends DayViewDateChanged event
        /// </summary>
        /// <param name="oldDate">Old date</param>
        /// <param name="newDate">New date</param>
        private void OnDayViewDateChanged(DateTime oldDate, DateTime newDate)
        {
            if (DayViewDateChanged != null)
                DayViewDateChanged(this, new DateChangeEventArgs(oldDate, newDate));
        }

        /// <summary>
        /// Sends ViewDateChanged events
        /// </summary>
        /// <param name="calendarView">eCalendarView</param>
        /// <param name="oldStartDate">Old start date</param>
        /// <param name="newStartDate">New start date</param>
        /// <param name="oldEndDate">Old end date</param>
        /// <param name="newEndDate">New end date</param>
        private void OnViewDateChanged(eCalendarView calendarView, DateTime oldStartDate,
            DateTime newStartDate, DateTime oldEndDate, DateTime newEndDate)
        {
            if (ViewDateChanged != null)
            {
                ViewDateChanged(this, new ViewDateChangedEventArgs(
                    calendarView, oldStartDate, newStartDate, oldEndDate, newEndDate));
            }
        }

        #endregion

        #endregion

        #region WeekView properties

        /// <summary>
        /// Gets the Week View
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public WeekView WeekView
        {
            get
            {
                if (_WeekView == null)
                {
                    // Single-user WeekView is not accessible
                    // when in multi-user mode

                    if (IsMultiCalendar == false)
                    {
                        _WeekView = new WeekView(this);

                        _WeekView.StartDate = _WeekViewStartDate;
                        _WeekView.EndDate = _WeekViewEndDate;

                        _WeekView.CalendarColor = GetViewColor(_WeekView, 0);
                    }
                }

                return (_WeekView);
            }

            internal set
            {
                if (_WeekView != value)
                {
                    if (_WeekView != null)
                        _WeekView.Dispose();

                    _WeekView = value;
                }
            }
        }

        #region WeekViewStartDate

        /// <summary>
        /// Gets and sets the week start date
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DateTime WeekViewStartDate
        {
            get { return (_WeekViewStartDate); }

            set
            {
                value = value.Date;

                if (_WeekViewStartDate.Equals(value) == false)
                {
                    DateTime oldDate = _WeekViewStartDate;
                    _WeekViewStartDate = value;

                    // Inform interested parties of the change

                    OnWeekViewStartDateChanged(oldDate, value);

                    OnViewDateChanged(eCalendarView.Week,
                        oldDate, value, _WeekViewEndDate, _WeekViewEndDate);

                    // Save AutoSyncDate

                    _AutoSyncDate = value;
                }
            }
        }

        /// <summary>
        /// Sends WeekViewStartDateChanged event
        /// </summary>
        /// <param name="oldDate">Old date</param>
        /// <param name="newDate">New date</param>
        private void OnWeekViewStartDateChanged(DateTime oldDate, DateTime newDate)
        {
            if (WeekViewStartDateChanged != null)
                WeekViewStartDateChanged(this, new DateChangeEventArgs(oldDate, newDate));
        }

        #endregion

        #region WeekViewEndDate

        /// <summary>
        /// Gets the week end date
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DateTime WeekViewEndDate
        {
            get { return (_WeekViewEndDate); }

            set
            {
                value = value.Date;

                if (_WeekViewEndDate.Equals(value) == false)
                {
                    DateTime oldDate = _WeekViewEndDate;
                    _WeekViewEndDate = value;

                    // Inform interested parties of the change

                    OnWeekViewEndDateChanged(oldDate, value);

                    OnViewDateChanged(eCalendarView.Week,
                        _WeekViewStartDate, _WeekViewStartDate, oldDate, value);
                }
            }
        }

        /// <summary>
        /// Sends WeekViewEndDateChanged event
        /// </summary>
        /// <param name="oldDate">Old date</param>
        /// <param name="newDate">New date</param>
        private void OnWeekViewEndDateChanged(DateTime oldDate, DateTime newDate)
        {
            if (WeekViewEndDateChanged != null)
                WeekViewEndDateChanged(this, new DateChangeEventArgs(oldDate, newDate));
        }

        #endregion

        #endregion

        #region MonthView properties

        /// <summary>
        /// Gets the Month View
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MonthView MonthView
        {
            get
            {
                if (_MonthView == null)
                {
                    // Single-user MonthView is not accessible
                    // when in multi-user mode

                    if (IsMultiCalendar == false)
                    {
                        _MonthView = new MonthView(this);

                        _MonthView.StartDate = _MonthViewStartDate;
                        _MonthView.EndDate = _MonthViewEndDate;

                        _MonthView.IsSideBarVisible = _IsMonthSideBarVisible;
                        _MonthView.CalendarColor = GetViewColor(_MonthView, 0);
                    }
                }

                return (_MonthView);
            }

            internal set
            {
                if (_MonthView != value)
                {
                    if (_MonthView != null)
                        _MonthView.Dispose();

                    _MonthView = value;
                }
            }
        }

        #region MonthViewStartDate

        /// <summary>
        /// Gets and sets the month start date
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DateTime MonthViewStartDate
        {
            get { return (_MonthViewStartDate); }

            set
            {
                value = value.Date;

                if (_MonthViewStartDate.Equals(value) == false)
                {
                    DateTime oldDate = _MonthViewStartDate;
                    _MonthViewStartDate = value;

                    // Inform interested parties of the change

                    OnMonthViewStartDateChanged(oldDate, value);

                    OnViewDateChanged(eCalendarView.Month,
                        oldDate, value, _MonthViewEndDate, _MonthViewEndDate);

                    // Save AutoSyncDate

                    _AutoSyncDate = value;
                }
            }
        }

        /// <summary>
        /// Sends MonthViewStartDateChanged event
        /// </summary>
        /// <param name="oldDate">Old date</param>
        /// <param name="newDate">New date</param>
        private void OnMonthViewStartDateChanged(DateTime oldDate, DateTime newDate)
        {
            if (MonthViewStartDateChanged != null)
                MonthViewStartDateChanged(this, new DateChangeEventArgs(oldDate, newDate));
        }

        #endregion

        #region MonthViewEndDate

        /// <summary>
        /// Gets the month end date
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DateTime MonthViewEndDate
        {
            get { return (_MonthViewEndDate); }

            set
            {
                value = value.Date;

                if (_MonthViewEndDate.Equals(value) == false)
                {
                    DateTime oldDate = _MonthViewEndDate;
                    _MonthViewEndDate = value;

                    // Inform interested parties of the change

                    OnMonthViewEndDateChanged(oldDate, value);

                    OnViewDateChanged(eCalendarView.Month,
                        _MonthViewStartDate, _MonthViewStartDate, oldDate, value);
                }
            }
        }

        /// <summary>
        /// Sends MonthViewEndDateChanged event
        /// </summary>
        /// <param name="oldDate">Old date</param>
        /// <param name="newDate">New date</param>
        private void OnMonthViewEndDateChanged(DateTime oldDate, DateTime newDate)
        {
            if (MonthViewEndDateChanged != null)
                MonthViewEndDateChanged(this, new DateChangeEventArgs(oldDate, newDate));
        }

        #endregion

        #endregion

        #region YearView properties

        /// <summary>
        /// Gets the Year View
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public YearView YearView
        {
            get
            {
                if (_YearView == null)
                {
                    // Single-user YearView is not accessible
                    // when in multi-user mode

                    if (IsMultiCalendar == false)
                    {
                        _YearView = new YearView(this);

                        _YearView.StartDate = _YearViewStartDate;
                        _YearView.EndDate = _YearViewEndDate;

                        _YearView.CalendarColor = GetViewColor(_YearView, 0);
                    }
                }

                return (_YearView);
            }

            internal set
            {
                if (_YearView != value)
                {
                    if (_YearView != null)
                        _YearView.Dispose();

                    _YearView = value;
                }
            }
        }

        #region YearViewStartDate

        /// <summary>
        /// Gets and sets the YearView start date
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DateTime YearViewStartDate
        {
            get { return (_YearViewStartDate); }

            set
            {
                value = value.Date;

                if (_YearViewStartDate.Equals(value) == false)
                {
                    DateTime oldDate = _YearViewStartDate;
                    _YearViewStartDate = value;

                    // Inform interested parties of the change

                    OnYearViewStartDateChanged(oldDate, value);

                    OnViewDateChanged(eCalendarView.Year,
                        oldDate, value, _YearViewStartDate, _YearViewEndDate);

                    // Save AutoSyncDate

                    _AutoSyncDate = value;
                }
            }
        }

        /// <summary>
        /// Sends OnYearViewStartDateChanged event
        /// </summary>
        /// <param name="oldDate">Old date</param>
        /// <param name="newDate">New date</param>
        private void OnYearViewStartDateChanged(DateTime oldDate, DateTime newDate)
        {
            if (YearViewStartDateChanged != null)
                YearViewStartDateChanged(this, new DateChangeEventArgs(oldDate, newDate));
        }

        #endregion

        #region YearViewEndDate

        /// <summary>
        /// Gets the YearView end date
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DateTime YearViewEndDate
        {
            get { return (_YearViewEndDate); }

            set
            {
                value = value.Date;

                if (_YearViewEndDate.Equals(value) == false)
                {
                    DateTime oldDate = _YearViewEndDate;
                    _YearViewEndDate = value;

                    // Inform interested parties of the change

                    OnYearViewEndDateChanged(oldDate, value);

                    OnViewDateChanged(eCalendarView.Month,
                        _YearViewStartDate, _YearViewStartDate, oldDate, value);
                }
            }
        }

        /// <summary>
        /// Sends OnYearViewEndDateChanged event
        /// </summary>
        /// <param name="oldDate">Old date</param>
        /// <param name="newDate">New date</param>
        private void OnYearViewEndDateChanged(DateTime oldDate, DateTime newDate)
        {
            if (YearViewEndDateChanged != null)
                YearViewEndDateChanged(this, new DateChangeEventArgs(oldDate, newDate));
        }

        #endregion

        #endregion

        #region TimeLineView properties

        /// <summary>
        /// Gets the TimeLine View
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TimeLineView TimeLineView
        {
            get
            {
                if (_TimeLineView == null)
                {
                    _TimeLineView = new TimeLineView(this, eCalendarView.TimeLine);

                    _TimeLineView.StartDate = _TimeLineViewStartDate;
                    _TimeLineView.EndDate = _TimeLineViewEndDate;
                }

                return (_TimeLineView);
            }

            internal set
            {
                if (_TimeLineView != value)
                {
                    if (_TimeLineView != null)
                        _TimeLineView.Dispose();

                    _TimeLineView = value;
                }
            }
        }

        #region TimeLineViewStartDate

        /// <summary>
        /// Gets and sets the TimeLine start date
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DateTime TimeLineViewStartDate
        {
            get { return (_TimeLineViewStartDate); }

            set
            {
                if (_TimeLineViewStartDate.Equals(value) == false)
                {
                    DateTime oldDate = _TimeLineViewStartDate;
                    _TimeLineViewStartDate = value;

                    // Update our column count and scrollBar values

                    CalcTimeLineColumnCount(true);
                    TimeLineHScrollPanel.UpdatePanel();

                    // Inform interested parties of the change

                    OnTimeLineViewStartDateChanged(oldDate, value);

                    OnViewDateChanged(eCalendarView.TimeLine,
                        oldDate, value, _TimeLineViewEndDate, _TimeLineViewEndDate);

                    // Save AutoSyncDate

                    _AutoSyncDate = TimeLineViewScrollStartDate;
                }
            }
        }

        /// <summary>
        /// Sends TimeLineViewStartDateChanged event
        /// </summary>
        /// <param name="oldDate">Old date</param>
        /// <param name="newDate">New date</param>
        private void OnTimeLineViewStartDateChanged(DateTime oldDate, DateTime newDate)
        {
            if (TimeLineViewStartDateChanged != null)
                TimeLineViewStartDateChanged(this, new DateChangeEventArgs(oldDate, newDate));
        }

        #endregion

        #region TimeLineViewEndDate

        /// <summary>
        /// Gets the TimeLine end date
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DateTime TimeLineViewEndDate
        {
            get { return (_TimeLineViewEndDate); }

            set
            {
                if (_TimeLineViewEndDate.Equals(value) == false)
                {
                    DateTime oldDate = _TimeLineViewEndDate;
                    _TimeLineViewEndDate = value;

                    // Update our column count and scrollBar values

                    CalcTimeLineColumnCount(false);
                    TimeLineHScrollPanel.UpdatePanel();

                    // Inform interested parties of the change

                    OnTimeLineViewEndDateChanged(oldDate, value);

                    OnViewDateChanged(eCalendarView.TimeLine,
                        _TimeLineViewStartDate, _TimeLineViewStartDate, oldDate, value);
                }
            }
        }

        /// <summary>
        /// Sends TimeLineViewEndDateChanged event
        /// </summary>
        /// <param name="oldDate">Old date</param>
        /// <param name="newDate">New date</param>
        private void OnTimeLineViewEndDateChanged(DateTime oldDate, DateTime newDate)
        {
            if (TimeLineViewEndDateChanged != null)
                TimeLineViewEndDateChanged(this, new DateChangeEventArgs(oldDate, newDate));
        }

        #endregion

        #region TimeLineViewScrollStartDate

        /// <summary>
        /// Gets and sets the TimeLine Scrolled start date
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DateTime TimeLineViewScrollStartDate
        {
            get { return (_TimeLineViewScrollStartDate); }

            set
            {
                if (_TimeLineViewScrollStartDate.Equals(value) == false)
                {
                    DateTime oldValue = _TimeLineViewScrollStartDate;
                    _TimeLineViewScrollStartDate = value;

                    value = UpdateViewScroll(value);

                    // Tell others about the change

                    OnTimeLineViewScrollDateChanged(oldValue, value);
                }
            }
        }

        /// <summary>
        /// Updates the TimeLine scroll value
        /// </summary>
        /// <param name="value">Value</param>
        private DateTime UpdateViewScroll(DateTime value)
        {
            TimeLineHScrollPanel.BeginUpdate();

            bool canScroll = _TimeLineCanExtendRange;

            if (value < _TimeLineViewStartDate)
            {
                if (canScroll == true)
                    TimeLineViewStartDate = value;
            }
            else
            {
                DateTime endDate;
                int n = Bounds.Width / TimeLineColumnWidth;

                try
                {
                    endDate = value.AddMinutes(n * BaseInterval);

                    if (endDate > _TimeLineViewEndDate)
                    {
                        if (canScroll == true)
                            TimeLineViewEndDate = endDate;
                    }
                    else
                    {
                        canScroll = true;
                    }
                }
                catch
                {
                    if (canScroll == true)
                    {
                        endDate = new DateTime(9970, 1, 1);
                        value = endDate.AddMinutes(-n * BaseInterval);

                        _TimeLineViewScrollStartDate = value;
                        TimeLineViewEndDate = endDate;
                    }
                }
            }

            if (canScroll == true)
            {
                TimeSpan ts = value - _TimeLineViewStartDate;

                TimeLineHScrollPanel.ScrollBar.Value =
                    (int)(ts.TotalMinutes / _BaseInterval);
            }

            TimeLineHScrollPanel.EndUpdate();

            return (value);
        }

        /// <summary>
        /// Sends TimeLineViewScrollDateChanged event
        /// </summary>
        /// <param name="oldDate">Old date</param>
        /// <param name="newDate">New date</param>
        private void OnTimeLineViewScrollDateChanged(DateTime oldDate, DateTime newDate)
        {
            if (TimeLineViewScrollDateChanged != null)
                TimeLineViewScrollDateChanged(this, new DateChangeEventArgs(oldDate, newDate));
        }
        
        #endregion

        #region TimeLineViewScrollEndDate

        /// <summary>
        /// Gets the TimeLine Scrolled end date
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DateTime TimeLineViewScrollEndDate
        {
            get
            {
                DateTime date = TimeLineViewScrollStartDate;

                if (TimeLineHScrollPanel != null)
                {
                    int intervals =
                        _TimeLineHeaderPanel.Bounds.Width / _TimeLineColumnWidth;

                    date = date.AddMinutes(_BaseInterval * intervals);
                }

                return (date);
            }
        }

        #endregion

        #region CalcTimeLineColumnCount

        /// <summary>
        /// Calculates number of TimeLine columns
        /// </summary>
        /// <param name="alterEnd"></param>
        private void CalcTimeLineColumnCount(bool alterEnd)
        {
            TimeSpan ts = _TimeLineViewEndDate - _TimeLineViewStartDate;

            double tc = Math.Max(1, ts.TotalMinutes / _BaseInterval);

            if (Math.Floor(tc) > _TimeLineMaxColumnCount)
            {
                AdjustViewTime(alterEnd, _TimeLineMaxColumnCount);
            }
            else if (tc * TimeLineColumnWidth >= Int32.MaxValue)
            {
                AdjustViewTime(alterEnd, Int32.MaxValue / TimeLineColumnWidth);
            }
            else
            {
                if (tc * TimeLineColumnWidth < Bounds.Width)
                    tc = Bounds.Width / TimeLineColumnWidth * 2;

                _TimeLineColumnCount = (int)tc;
            }
        }

        private void AdjustViewTime(bool alterEnd, int tc)
        {
            if (alterEnd == true)
                TimeLineViewEndDate = TimeLineAddInterval(TimeLineViewStartDate, tc);
            else
                TimeLineViewStartDate = TimeLineAddInterval(TimeLineViewEndDate, -tc);
        }

        #endregion

        #region TimeLineColumnWidth

        /// <summary>
        /// Gets and sets the Calendar TimeLineColumnWidth
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int TimeLineColumnWidth
        {
            get { return (_TimeLineColumnWidth); }

            set
            {
                if (_TimeLineColumnWidth != value)
                {
                    int oldValue = _TimeLineColumnWidth;
                    _TimeLineColumnWidth = value;

                    CalcTimeLineColumnCount(true);
                    TimeLineHScrollPanel.UpdatePanel();

                    // Inform interested parties of the change

                    OnTimeLineColumnWidthChanged(oldValue, value);
                }
            }
        }

        /// <summary>
        /// Sends TimeLineColumnWidthChanged event
        /// </summary>
        /// <param name="oldValue">Old width</param>
        /// <param name="newValue">New width</param>
        private void OnTimeLineColumnWidthChanged(int oldValue, int newValue)
        {
            if (TimeLineColumnWidthChanged != null)
                TimeLineColumnWidthChanged(this, new TimeLineColumnWidthChangedEventArgs(oldValue, newValue));
        }

        #endregion

        #region TimeLinePeriodHeaderAlignment

        /// <summary>
        /// Gets or sets the text alignment for the TimeLineView Period Header text
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public eItemAlignment TimeLinePeriodHeaderAlignment
        {
            get { return (_TimeLinePeriodHeaderAlignment); }

            set
            {
                if (_TimeLinePeriodHeaderAlignment != value)
                {
                    _TimeLinePeriodHeaderAlignment = value;

                    CalendarPanel.Refresh();
                }
            }
        }

        #endregion

        #region TimeLinePeriodHeaderEnableMarkup

        /// <summary>
        /// Gets or sets whether text-markup support is enabled for the
        /// TimeLineView Period Header text
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool TimeLinePeriodHeaderEnableMarkup
        {
            get { return (_TimeLinePeriodHeaderEnableMarkup); }

            set
            {
                if (_TimeLinePeriodHeaderEnableMarkup != value)
                {
                    _TimeLinePeriodHeaderEnableMarkup = value;

                    CalendarPanel.Refresh();
                }
            }
        }

        #endregion

        #region TimeLineMaxColumnCount

        /// <summary>
        /// Gets and sets the Calendar TimeLineMaxColumnCount
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int TimeLineMaxColumnCount
        {
            get { return (_TimeLineMaxColumnCount); }

            set
            {
                value = Math.Max(100, value);

                if (_TimeLineMaxColumnCount != value)
                {
                    int oldValue = _TimeLineMaxColumnCount;
                    _TimeLineMaxColumnCount = value;

                    CalcTimeLineColumnCount(true);
                    TimeLineHScrollPanel.UpdatePanel();

                    // Inform interested parties of the change

                    OnTimeLineMaxColumnCountChanged(oldValue, value);
                }
            }
        }

        /// <summary>
        /// Sends TimeLineMaxColumnCountChanged event
        /// </summary>
        /// <param name="oldValue">Old width</param>
        /// <param name="newValue">New width</param>
        private void OnTimeLineMaxColumnCountChanged(int oldValue, int newValue)
        {
            if (TimeLineMaxColumnCountChanged != null)
                TimeLineMaxColumnCountChanged(this, new TimeLineMaxColumnCountChangedEventArgs(oldValue, newValue));
        }

        #endregion

        #region TimeLineInterval

        /// <summary>
        /// Gets and sets the Calendar TimeLineInterval
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int TimeLineInterval
        {
            get { return (_TimeLineInterval); }

            set
            {
                value = GetValidInterval(value);

                if (_TimeLineInterval != value)
                {
                    int oldValue = _TimeLineInterval;
                    _TimeLineInterval = value;

                    UpdateBaseInterval();

                    DateTime date =
                        TimeLineViewStartDate.Date.AddMinutes(_BaseInterval * 200);

                    if (date > TimeLineViewEndDate)
                    {
                        TimeLineViewEndDate = date.Date;
                    }
                    else
                    {
                        TimeLineViewStartDate = TimeLineViewStartDate.Date;
                        CalcTimeLineColumnCount(true);
                    }

                    TimeLineHScrollPanel.ScrollBar.Value = 0;
                    TimeLineHScrollPanel.UpdatePanel();

                    OnTimeLineIntervalChanged(oldValue, value);
                }
            }
        }

        /// <summary>
        /// Sends TimeLineIntervalChanged events
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        private void OnTimeLineIntervalChanged(int oldValue, int newValue)
        {
            if (TimeLineIntervalChanged != null)
                TimeLineIntervalChanged(this, new TimeLineIntervalChangedEventArgs(oldValue, newValue));
        }

        #region GetValidInterval

        /// <summary>
        /// Coerces the user supplied interval period into
        /// an evenly divisible duration
        /// </summary>
        /// <param name="value">Original value</param>
        /// <returns>Validated value</returns>
        private int GetValidInterval(int value)
        {
            if (value < 1)
                return (1);

            switch (_TimeLinePeriod)
            {
                case eTimeLinePeriod.Minutes:
                    return (GetValidIntervalMinutes(value));

                case eTimeLinePeriod.Hours:
                    return (GetValidIntervalHours(value));

                case eTimeLinePeriod.Days:
                    return (value >= MaxDayDuration ? MaxDayDuration : value);

                case eTimeLinePeriod.Years:
                    return (value >= MaxYearDuration ? MaxYearDuration : value);
            }

            return (1);
        }

        #region GetValidIntervalMinutes

        /// <summary>
        /// Gets a valid minute interval from the
        /// given user supplied value
        /// </summary>
        /// <param name="value">Supplied value</param>
        /// <returns>Valid interval</returns>
        private int GetValidIntervalMinutes(int value)
        {
            if (value < MaxMinuteDuration)
            {
                for (int i = 0; i < MaxMinuteDuration; i++)
                {
                    if (MaxMinuteDuration % (value + i) == 0)
                        return (value + i);
                }
            }

            return (MaxMinuteDuration);
        }

        #endregion

        #region GetValidIntervalHours

        /// <summary>
        /// Gets a valid hour interval from the
        /// given user supplied value
        /// </summary>
        /// <param name="value">Supplied value</param>
        /// <returns>Valid interval</returns>
        private int GetValidIntervalHours(int value)
        {
            if (value < MaxHourDuration)
            {
                for (int i = 0; i < MaxHourDuration; i++)
                {
                    if (HoursPerDay % (value + i) == 0)
                        return (value + i);
                }
            }

            return (MaxHourDuration);
        }

        #endregion

        #endregion

        #region UpdateBaseInterval

        /// <summary>
        /// Updates the BaseInterval value (interval total minutes)
        /// </summary>
        private void UpdateBaseInterval()
        {
            switch (_TimeLinePeriod)
            {
                case eTimeLinePeriod.Minutes:
                    _BaseInterval = _TimeLineInterval;
                    break;

                case eTimeLinePeriod.Hours:
                    _BaseInterval = _TimeLineInterval * MinutesPerHour;
                    break;

                case eTimeLinePeriod.Days:
                    _BaseInterval = _TimeLineInterval * MinutesPerDay;
                    break;

                case eTimeLinePeriod.Years:
                    _BaseInterval = _TimeLineInterval * MinutesPerYear;
                    break;
            }

            TimeLineViewStartDate = (_TimeLinePeriod == eTimeLinePeriod.Years) ?
                new DateTime(TimeLineViewStartDate.Year, 1, 1) : TimeLineViewStartDate.Date;
        }

        #endregion

        #endregion

        #region TimeLinePeriod

        /// <summary>
        /// Gets and sets the Calendar TimeLinePeriod
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public eTimeLinePeriod TimeLinePeriod
        {
            get { return (_TimeLinePeriod); }

            set
            {
                if (_TimeLinePeriod != value)
                {
                    eTimeLinePeriod oldValue = _TimeLinePeriod;
                    _TimeLinePeriod = value;

                    TimeLineInterval =
                        (value == eTimeLinePeriod.Minutes) ? 30 : 1;

                    UpdateBaseInterval();
                    UpdateBasePeriod();

                    TimeLineHScrollPanel.ScrollBar.Value = 0;

                    OnTimeLineIntervalPeriodChanged(oldValue, value);
                }
            }
        }

        /// <summary>
        /// Sends TimeLineIntervalPeriodChanged events
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        private void OnTimeLineIntervalPeriodChanged(
            eTimeLinePeriod oldValue, eTimeLinePeriod newValue)
        {
            if (TimeLineIntervalPeriodChanged != null)
                TimeLineIntervalPeriodChanged(this, new TimeLineIntervalPeriodChangedEventArgs(oldValue, newValue));
        }

        #region UpdateBasePeriod

        /// <summary>
        /// Updates the view end date period given the
        /// new base interval
        /// </summary>
        private void UpdateBasePeriod()
        {
            int intervals = 1;

            switch (_TimeLinePeriod)
            {
                case eTimeLinePeriod.Minutes:
                    intervals = (7 * 24 * 2);
                    break;

                case eTimeLinePeriod.Hours:
                    intervals = (7 * 24);
                    break;

                case eTimeLinePeriod.Days:
                    intervals = (365 / 2);
                    break;

                case eTimeLinePeriod.Years:
                    intervals = _TimeLineMaxColumnCount;
                    break;
            }

            TimeLineViewStartDate = (_TimeLinePeriod == eTimeLinePeriod.Years) ?
                new DateTime(TimeLineViewStartDate.Year, 1, 1) : TimeLineViewStartDate.Date;

            TimeLineViewEndDate = TimeLineAddInterval(TimeLineViewStartDate, intervals);
        }

        #endregion

        #endregion

        #region TimeLinePeriodHeaderHeight

        /// <summary>
        /// Gets or sets the TimeLine period header height. Set to -1 for default.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int TimeLinePeriodHeaderHeight
        {
            get
            { 
                if (_TimeLinePeriodHeaderHeight < 0)
                    return (Font.Height + 7);

                return (_TimeLinePeriodHeaderHeight);
            }

            set
            {
                if (_TimeLinePeriodHeaderHeight != value)
                {
                    _TimeLinePeriodHeaderHeight = value;

                    CalendarPanel.NeedRecalcSize = true;
                    CalendarPanel.Refresh();
                }
            }
        }

        #endregion

        #region TimeLineHeight

        /// <summary>
        /// Gets and sets the Calendar TimeLineHeight
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int TimeLineHeight
        {
            get { return (_TimeLineHeight); }

            set
            {
                if (_TimeLineHeight != value)
                {
                    _TimeLineHeight = value;

                    CalendarPanel.NeedRecalcSize = true;
                    CalendarPanel.Refresh();
                }
            }
        }

        #endregion

        #region TimeLineShowPeriodHeader

        /// <summary>
        /// Gets and sets the Calendar TimeLineShowPeriodHeader
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool TimeLineShowPeriodHeader
        {
            get { return (_TimeLineShowPeriodHeader); }

            set
            {
                if (_TimeLineShowPeriodHeader != value)
                {
                    _TimeLineShowPeriodHeader = value;

                    OnTimeLineShowPeriodHeaderChanged(!value, value);

                    CalendarPanel.NeedRecalcSize = true;
                    CalendarPanel.Refresh();
                }
            }
        }

        /// <summary>
        /// Sends TimeLineShowPeriodHeaderChanged event
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        private void OnTimeLineShowPeriodHeaderChanged(bool oldValue, bool newValue)
        {
            if (TimeLineShowPeriodHeaderChanged != null)
                TimeLineShowPeriodHeaderChanged(this, new TimeLineShowPeriodHeaderChangedEventArgs(oldValue, newValue));
        }

        #endregion

        #region TimeLineShowIntervalHeader

        /// <summary>
        /// Gets and sets the Calendar TimeLineShowIntervalHeader
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool TimeLineShowIntervalHeader
        {
            get { return (_TimeLineShowIntervalHeader); }

            set
            {
                if (_TimeLineShowIntervalHeader != value)
                {
                    _TimeLineShowIntervalHeader = value;

                    OnTimeLineShowIntervalHeaderChanged(!value, value);

                    CalendarPanel.NeedRecalcSize = true;
                    CalendarPanel.Refresh();
                }
            }
        }

        /// <summary>
        /// Sends TimeLineShowIntervalHeader event
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        private void OnTimeLineShowIntervalHeaderChanged(bool oldValue, bool newValue)
        {
            if (TimeLineShowIntervalHeaderChanged != null)
                TimeLineShowIntervalHeaderChanged(this, new TimeLineShowIntervalHeaderChangedEventArgs(oldValue, newValue));
        }

        #endregion

        #region TimeLineShowPageNavigation

        /// <summary>
        /// Gets and sets the Calendar TimeLineShowPageNavigation
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool TimeLineShowPageNavigation
        {
            get { return (_TimeLineShowPageNavigation); }

            set
            {
                if (_TimeLineShowPageNavigation != value)
                {
                    _TimeLineShowPageNavigation = value;

                    OnTimeLineShowPageNavigationChanged(!value, value);

                    CalendarPanel.NeedRecalcSize = true;
                    CalendarPanel.Refresh();
                }
            }
        }

        /// <summary>
        /// Sends TimeLineShowPageNavigationChanged event
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        private void OnTimeLineShowPageNavigationChanged(bool oldValue, bool newValue)
        {
            if (TimeLineShowPageNavigationChanged != null)
                TimeLineShowPageNavigationChanged(this, new TimeLineShowPageNavigationChangedEventArgs(oldValue, newValue));
        }

        #endregion

        #region TimeLineCondensedViewVisibility

        /// <summary>
        /// Gets and sets the Condensed View visibility
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public eCondensedViewVisibility TimeLineCondensedViewVisibility
        {
            get { return (_TimeLineCondensedViewVisibility); }

            set
            {
                if (_TimeLineCondensedViewVisibility != value)
                {
                    eCondensedViewVisibility oldValue = _TimeLineCondensedViewVisibility;
                    _TimeLineCondensedViewVisibility = value;

                    OnTimeLineCondensedViewVisibilityChanged(oldValue, value);

                    CalendarPanel.NeedRecalcSize = true;
                    CalendarPanel.Refresh();
                }
            }
        }

        /// <summary>
        /// Sends OnTimeLineCondensedViewVisibilityChanged event
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        private void OnTimeLineCondensedViewVisibilityChanged(
            eCondensedViewVisibility oldValue, eCondensedViewVisibility newValue)
        {
            if (TimeLineCondensedViewVisibilityChanged != null)
                TimeLineCondensedViewVisibilityChanged(this,
                    new TimeLineCondensedViewVisibilityChangedEventArgs(oldValue, newValue));
        }

        #endregion

        #region TimeLineCondensedViewHeight

        /// <summary>
        /// Gets and sets the Condensed View height
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int TimeLineCondensedViewHeight
        {
            get { return (_TimeLineCondensedViewHeight); }

            set
            {
                if (_TimeLineCondensedViewHeight != value)
                {
                    int oldValue = _TimeLineCondensedViewHeight;
                    _TimeLineCondensedViewHeight = value;

                    OnTimeLineCondensedViewHeightChanged(oldValue, value);

                    CalendarPanel.NeedRecalcSize = true;
                    CalendarPanel.Refresh();
                }
            }
        }

        /// <summary>
        /// Sends OnTimeLineCondensedViewHeightChanged event
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        private void OnTimeLineCondensedViewHeightChanged(int oldValue, int newValue)
        {
            if (TimeLineCondensedViewHeightChanged != null)
                TimeLineCondensedViewHeightChanged(this,
                    new TimeLineCondensedViewHeightChangedEventArgs(oldValue, newValue));
        }

        #endregion

        #region TimeLineStretchRowHeight

        /// <summary>
        /// Gets or sets whether the row height is stretched
        /// to fill the TimeLine appointment content area
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool TimeLineStretchRowHeight
        {
            get { return (_TimeLineStretchRowHeight); }

            set
            {
                if (_TimeLineStretchRowHeight != value)
                {
                    _TimeLineStretchRowHeight = value;

                    CalendarPanel.Refresh();
                }
            }
        }

        #endregion

        #region TimeLineCanExtendRange

        /// <summary>
        /// Gets or sets whether the TimeLine Start and End dates can be
        /// can be automatically extended by the control
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool TimeLineCanExtendRange
        {
            get { return (_TimeLineCanExtendRange); }
            set { _TimeLineCanExtendRange = value; }
        }

        #endregion

        #region TimeLineShowCollateLines

        /// <summary>
        /// Gets or sets whether the TimeLine view will draw collate lines
        /// between each group of collated rows (see TimeLineViewGetRowCollateId event)
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool TimeLineShowCollateLines
        {
            get { return (_TimeLineShowCollateLines); }
            set { _TimeLineShowCollateLines = value; }
        }

        #endregion

        #region TimeLineMultiUserTabWidth

        /// <summary>
        /// Gets and sets the Calendar TimeLine horizontal tab width
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int TimeLineMultiUserTabWidth
        {
            get { return (_TimeLineMultiUserTabWidth); }

            set
            {
                if (_TimeLineMultiUserTabWidth != value)
                {
                    _TimeLineMultiUserTabWidth = value;

                    CalendarPanel.NeedRecalcSize = true;
                    CalendarPanel.Refresh();
                }
            }
        }

        #endregion

        #region TimeLineMultiUserTabOrientation

        /// <summary>
        /// Gets and sets the Calendar TimeLine horizontal tab orientation
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public eOrientation TimeLineMultiUserTabOrientation
        {
            get { return (_TimeLineMultiUserTabOrientation); }

            set
            {
                if (_TimeLineMultiUserTabOrientation != value)
                {
                    _TimeLineMultiUserTabOrientation = value;

                    CalendarPanel.NeedRecalcSize = true;
                    CalendarPanel.Refresh();
                }
            }
        }

        #endregion

        #region TimeLinePageNavigatorTodayTooltip

        /// <summary>
        /// Gets or sets the Calendar TimeLineView PageNavigator TodayTooltip
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string TimeLinePageNavigatorTodayTooltip
        {
            get { return (_TimeLinePageNavigatorTodayTooltip); }

            set
            {
                _TimeLinePageNavigatorTodayTooltip = value;

                if (_TimeLineHScrollPanel != null && _TimeLineHScrollPanel.PageNavigator != null)
                    _TimeLineHScrollPanel.PageNavigator.TodayTooltip = value;
            }
        }

        #endregion

        #region TimeLinePageNavigatorPreviousPageTooltip

        /// <summary>
        /// Gets or sets the Calendar TimeLineView PageNavigator PreviousPageTooltip
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string TimeLinePageNavigatorPreviousPageTooltip
        {
            get { return (_TimeLinePageNavigatorPreviousPageTooltip); }

            set
            {
                _TimeLinePageNavigatorPreviousPageTooltip = value;

                if (_TimeLineHScrollPanel != null && _TimeLineHScrollPanel.PageNavigator != null)
                    _TimeLineHScrollPanel.PageNavigator.PreviousPageTooltip = value;
            }
        }

        #endregion

        #region TimeLinePageNavigatorNextPageTooltip

        /// <summary>
        /// Gets or sets the Calendar TimeLineView PageNavigator NextPageTooltip
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string TimeLinePageNavigatorNextPageTooltip
        {
            get { return (_TimeLinePageNavigatorNextPageTooltip); }

            set
            {
                _TimeLinePageNavigatorNextPageTooltip = value;

                if (_TimeLineHScrollPanel != null && _TimeLineHScrollPanel.PageNavigator != null)
                    _TimeLineHScrollPanel.PageNavigator.NextPageTooltip = value;
            }
        }

        #endregion

        #endregion

        #region MultiCalendarView properties

        /// <summary>
        /// Gets the multiCalendar state or mode
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsMultiCalendar
        {
            get { return (MultiCalendarDayViews != null); }
        }

        /// <summary>
        /// Gets the MultiCalendarDayViews collection
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CalendarViewCollection<DayView> MultiCalendarDayViews
        {
            get { return (_MultiCalendarDayViews); }
        }

        /// <summary>
        /// Gets the MultiCalendarWeekViews collection
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CalendarViewCollection<WeekView> MultiCalendarWeekViews
        {
            get { return (_MultiCalendarWeekViews); }
        }

        /// <summary>
        /// Gets the MultiCalendarMonthViews collection
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CalendarViewCollection<MonthView> MultiCalendarMonthViews
        {
            get { return (_MultiCalendarMonthViews); }
        }

        /// <summary>
        /// Gets the MultiCalendarYearViews collection
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CalendarViewCollection<YearView> MultiCalendarYearViews
        {
            get { return (_MultiCalendarYearViews); }
        }

        /// <summary>
        /// Gets the MultiCalendarTimeLineViews collection
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CalendarViewCollection<TimeLineView> MultiCalendarTimeLineViews
        {
            get { return (_MultiCalendarTimeLineViews); }
        }
        
        #endregion

        #region DisplayedOwners

        /// <summary>
        /// Gets the DisplayedOwners collection
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DisplayedOwnerCollection DisplayedOwners
        {
            get { return (_DisplayedOwners); }
        }

        #endregion

        #region SelectedOwner

        /// <summary>
        /// Gets and sets the current selected multi-user owner
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string SelectedOwner
        {
            get
            {
                if (_SelectedOwnerIndex >= 0 && _SelectedOwnerIndex < _DisplayedOwners.Count)
                    return (_DisplayedOwners[_SelectedOwnerIndex]);

                return ("");
            }

            set
            {
                SelectedOwnerIndex = _DisplayedOwners.IndexOf(value);
            }
        }

        /// <summary>
        /// Gets and sets the current selected multi-user owner
        /// using a DisplayedOwner index
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int SelectedOwnerIndex
        {
            get { return (_SelectedOwnerIndex); }

            set
            {
                if (_SelectedOwnerIndex != value)
                {
                    if (value < 0 || value >= _DisplayedOwners.Count)
                        value = -1;

                    int oldValue = _SelectedOwnerIndex;
                    _SelectedOwnerIndex = value;

                    RefreshDisplayTabs(oldValue, value);

                    DateSelectionStart = null;
                    DateSelectionEnd = null;

                    OnSelectedOwnerChanged(oldValue, value);
                }
            }
        }

        #region RefreshDisplayTabs

        /// <summary>
        /// Refreshes the deselected and newly selected tabs
        /// </summary>
        /// <param name="oldValue">Old tab index</param>
        /// <param name="newValue">New tab index</param>
        private void RefreshDisplayTabs(int oldValue, int newValue)
        {
            if (IsMultiCalendar == true)
            {
                switch (SelectedView)
                {
                    case eCalendarView.Day:
                        if (oldValue >= 0 && oldValue < MultiCalendarDayViews.Views.Count)
                            RefreshHTab(MultiCalendarDayViews.Views[oldValue], true);

                        if (newValue >= 0 && newValue < MultiCalendarDayViews.Views.Count)
                            RefreshHTab(MultiCalendarDayViews.Views[newValue], false);
                        break;

                    case eCalendarView.Week:
                        if (oldValue >= 0 && oldValue < MultiCalendarWeekViews.Views.Count)
                            RefreshHTab(MultiCalendarWeekViews.Views[oldValue], true);

                        if (newValue >= 0 && newValue < MultiCalendarWeekViews.Views.Count)
                            RefreshHTab(MultiCalendarWeekViews.Views[newValue], false);
                        break;

                    case eCalendarView.Month:
                        if (oldValue >= 0 && oldValue < MultiCalendarMonthViews.Views.Count)
                            RefreshHTab(MultiCalendarMonthViews.Views[oldValue], true);

                        if (newValue >= 0 && newValue < MultiCalendarMonthViews.Views.Count)
                            RefreshHTab(MultiCalendarMonthViews.Views[newValue], false);
                        break;

                    case eCalendarView.Year:
                        if (oldValue >= 0 && oldValue < MultiCalendarYearViews.Views.Count)
                            RefreshHTab(MultiCalendarYearViews.Views[oldValue], true);

                        if (newValue >= 0 && newValue < MultiCalendarYearViews.Views.Count)
                            RefreshHTab(MultiCalendarYearViews.Views[newValue], false);
                        break;

                    case eCalendarView.TimeLine:
                        if (oldValue >= 0 && oldValue < MultiCalendarTimeLineViews.Views.Count)
                            RefreshVTab(MultiCalendarTimeLineViews.Views[oldValue], true);

                        if (newValue >= 0 && newValue < MultiCalendarTimeLineViews.Views.Count)
                            RefreshVTab(MultiCalendarTimeLineViews.Views[newValue], false);
                        break;
                }
            }
        }

        #endregion

        #region RefreshHTab

        /// <summary>
        /// Invalidates the given view tab
        /// </summary>
        /// <param name="view"></param>
        /// <param name="resetSelItem"></param>
        private void RefreshHTab(BaseView view, bool resetSelItem)
        {
            Rectangle r = view.Bounds;
            r.Height = MultiUserTabHeight;

            view.InvalidateRect(r);

            // Reset the selected item for the old view

            if (resetSelItem == true)
                view.SelectedItem = null;

            // Invalidate any active TimeIndicators

            InvalidateTimeIndicators(view);
        }

        #endregion

        #region RefreshVTab

        /// <summary>
        /// Invalidates the given view tab
        /// </summary>
        /// <param name="view"></param>
        /// <param name="resetSelItem"></param>
        private void RefreshVTab(TimeLineView view, bool resetSelItem)
        {
            Rectangle r = view.Bounds;

            if (_TimeLineMultiUserTabOrientation == eOrientation.Vertical)
                r.Width = view.MultiUserTabHeight + 4;
            else
                r.Width = _TimeLineMultiUserTabWidth + 4;

            view.InvalidateRect(r);

            // Reset the selected item for the old view

            if (resetSelItem == true)
                view.SelectedItem = null;

            // Invalidate any condensed views and any
            // active TimeIndicators

            InvalidateCondensedViews(view);
            InvalidateTimeIndicators(view);
        }

        #endregion

        #region InvalidateCondensedViews

        /// <summary>
        /// InvalidateCondensedViews
        /// </summary>
        /// <param name="view"></param>
        private void InvalidateCondensedViews(TimeLineView view)
        {
            // If the user has condensed TimeLine showing
            // then make sure it gets refreshed

            if (TimeLineCondensedViewVisibility != eCondensedViewVisibility.Hidden)
            {
                view.UpdateCondensedColumnList();

                Rectangle r = view.ClientRect;

                r.Y = r.Bottom - TimeLineCondensedViewHeight;
                r.Height = TimeLineCondensedViewHeight;

                view.InvalidateRect(r);
            }
        }

        #endregion

        #region InvalidateTimeIndicators

        /// <summary>
        /// InvalidateTimeIndicators
        /// </summary>
        /// <param name="view"></param>
        private void InvalidateTimeIndicators(BaseView view)
        {
            for (int i = 0; i < TimeIndicators.Count; i++)
            {
                TimeIndicator ti = TimeIndicators[i];

                if (ti.Visibility == eTimeIndicatorVisibility.SelectedResource)
                    InvalidateTimeIndicator(view, ti, ti.IndicatorDisplayTime);
            }
        }

        #endregion

        #region InvalidateTimeIndicator

        /// <summary>
        /// Invalidates TimeIndicator display area
        ///  for the given view DateTime
        /// </summary>
        /// <param name="view"></param>
        /// <param name="ti"></param>
        /// <param name="dateTime"></param>
        private void InvalidateTimeIndicator(BaseView view, TimeIndicator ti, DateTime dateTime)
        {
            Rectangle r = view.GetIndicatorRect(ti, dateTime);

            if (r.IsEmpty == false)
            {
                r.Inflate(4, 4);

                view.InvalidateRect(r);
            }
        }

        #endregion

        #region OnSelectedOwnerChanged

        /// <summary>
        /// Propagates SelectedOwnerChanged events
        /// </summary>
        /// <param name="oldValue">Old index</param>
        /// <param name="newValue">New index</param>
        private void OnSelectedOwnerChanged(int oldValue, int newValue)
        {
            if (SelectedOwnerChanged != null)
            {
                SelectedOwnerChanged(this,
                    new SelectedOwnerChangedEventArgs(oldValue, newValue));
            }
        }

        #endregion

        #endregion

        #region SelectedAppointments

        /// <summary>
        /// Gets the read-only collection of currently selected
        /// appointments in the current view
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ReadOnlyCollection<AppointmentView> SelectedAppointments
        {
            get
            {
                if (IsMultiCalendar == true)
                {
                    switch (_SelectedView)
                    {
                        case eCalendarView.Day:
                            return (_MultiCalendarDayViews[_SelectedOwnerIndex].SelectedAppointments);

                        case eCalendarView.Week:
                            return (_MultiCalendarWeekViews[_SelectedOwnerIndex].SelectedAppointments);

                        case eCalendarView.Month:
                            return (_MultiCalendarMonthViews[_SelectedOwnerIndex].SelectedAppointments);
                    }
                }
                else
                {
                    switch (SelectedView)
                    {
                        case eCalendarView.Day:
                            return (DayView.SelectedAppointments);

                        case eCalendarView.Week:
                            return (WeekView.SelectedAppointments);

                        case eCalendarView.Month:
                            return (MonthView.SelectedAppointments);
                    }
                }

                return (null);
            }
        }

        #endregion

        #region TimeIndicators

        /// <summary>
        /// TimeIndicators
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TimeIndicatorCollection TimeIndicators
        {
            get
            {
                if (_TimeIndicators == null)
                {
                    _TimeIndicators = new TimeIndicatorCollection();

                    _TimeIndicators.TimeIndicatorCollectionChanged += TimeIndicatorsTimeIndicatorCollectionChanged;
                    _TimeIndicators.TimeIndicatorTimeChanged += TimeIndicatorsTimeIndicatorTimeChanged;
                    _TimeIndicators.TimeIndicatorColorChanged += TimeIndicatorsTimeIndicatorColorChanged;
                }

                return (_TimeIndicators);
            }
        }

        #endregion

        #region ViewDisplayCustomizations

        /// <summary>
        /// Gets the CalendarView ViewDisplayCustomizations
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ViewDisplayCustomizations ViewDisplayCustomizations
        {
            get
            {
                if (_ViewDisplayCustomizations == null)
                {
                    _ViewDisplayCustomizations = new ViewDisplayCustomizations(this);

                    _ViewDisplayCustomizations.CollectionChanged += ViewDisplayCustomizationsCollectionChanged;
                }

                return (_ViewDisplayCustomizations);
            }
        }

        #endregion

        #region ViewWidth

        /// <summary>
        /// Gets and sets the Calendar ViewWidth
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [DefaultValue(250)]
        public int ViewWidth
        {
            get { return (_ViewWidth); }

            set
            {
                if (_ViewWidth != value)
                {
                    _ViewWidth = value;

                    if (CalendarPanel.HScrollBar != null)
                        CalendarPanel.HScrollBar.Value = 0;

                    CalendarPanel.NeedRecalcSize = true;
                    CalendarPanel.Refresh();
                }
            }
        }

        #endregion

        #region Style

        /// <summary>
        /// Gets/Sets the visual style of the CalendarView
        /// </summary>
        [Browsable(false), DevCoBrowsable(true)]
        [Category("Appearance"), Description("Specifies the visual style of the Control.")]
        [DefaultValue(eDotNetBarStyle.StyleManagerControlled)]
        public eDotNetBarStyle Style
        {
            get
            {
                return (CalendarPanel.Style);
            }
            set
            {
                if (CalendarPanel.Style != value)
                {
                    CalendarPanel.Style = value;

                    Invalidate();
                }
            }
        }

        #endregion

        #endregion

        #endregion

        #region Private properties

        /// <summary>
        /// Gets the default (cycling) color scheme
        /// </summary>
        private eCalendarColor DefaultCalendarColor
        {
            get
            {
                if (IsMultiCalendar == true)
                {
                    eCalendarColor c = (eCalendarColor)_DefaultCalendarColor++;

                    if (_DefaultCalendarColor >= (int)eCalendarColor.Automatic)
                        _DefaultCalendarColor = 0;

                    return (c);
                }
                
                return (eCalendarColor.Automatic);
            }
        }

        #endregion

        #region Internal properties

        #region Base properties

        /// <summary>
        /// Gets the CalendarPanel object
        /// </summary>
        internal CalendarPanel CalendarPanel
        {
            get { return (_CalendarPanel); }
        }

        /// <summary>
        /// Gets the width of a vertical scrollbar
        /// </summary>
        internal int VsWidth
        {
            get { return (SystemInformation.VerticalScrollBarWidth); }
        }

        /// <summary>
        /// Gets the height of a horizontal scrollbar
        /// </summary>
        internal int HsHeight
        {
            get { return (SystemInformation.HorizontalScrollBarHeight); }
        }

        /// <summary>
        /// Gets the multiuser tab height
        /// </summary>
        internal int MultiUserTabHeight
        {
            get { return (Font.Height + 6); }
        }

        /// <summary>
        /// Gets and sets the AutoSyncDate
        /// </summary>
        internal DateTime AutoSyncDate
        {
            get { return (_AutoSyncDate); }
            set { _AutoSyncDate = value; }
        }

        /// <summary>
        /// Gets whether any CategoryColors have been defined
        /// </summary>
        internal bool HasCategoryColors
        {
            get { return (_CategoryColors != null); }
        }

        #endregion

        #region WeekDay properties

        /// <summary>
        /// Gets the default TimeSlice height
        /// </summary>
        internal float TimeSliceHeight
        {
            get { return (_TimeSliceHeight); }

            set
            {
                if (value < Font.Height + 7)
                    value = Font.Height + 7;

                _TimeSliceHeight = value;
            }
        }

        /// <summary>
        /// Gets the number of WeekDay time slices
        /// </summary>
        internal int NumberOfSlices
        {
            get { return (_NumberOfSlices); }
            set { _NumberOfSlices = value; }
        }

        /// <summary>
        /// Gets the NumberOfActiveSlices
        /// </summary>
        internal int NumberOfActiveSlices
        {
            get { return (_NumberOfSlices - 1); }
        }

        /// <summary>
        /// WeekDay starting Slice
        /// </summary>
        internal int StartSlice
        {
            get { return (_StartSlice); }
            set { _StartSlice = value; }
        }

        /// <summary>
        /// Gets the number of slots per hour
        /// </summary>
        internal int SlotsPerHour
        {
            get { return (_SlotsPerHour); }
        }

        /// <summary>
        /// Gets the WeekDay Vertical Scroll panel
        /// </summary>
        internal WeekDayVScrollPanel WeekDayVScrollPanel
        {
            get { return (_WeekDayVScrollPanel); }
        }

        /// <summary>
        /// Gets the Year Vertical Scroll panel
        /// </summary>
        internal YearVScrollPanel YearVScrollPanel
        {
            get { return (_YearVScrollPanel); }
        }

        /// <summary>
        /// Gets the AllDay panel height
        /// </summary>
        internal int AllDayPanelHeight
        {
            get
            {
                for (int i = 0; i < _CalendarPanel.SubItems.Count; i++)
                {
                    WeekDayView wv = _CalendarPanel.SubItems[i] as WeekDayView;

                    if (wv != null)
                        return (wv.AllDayPanel.PanelHeight);
                }

                return (0);
            }
        }

        /// <summary>
        /// Gets the TimerRuler width
        /// </summary>
        internal int TimeRulerWidth
        {
            get { return (40); }
        }

        #endregion

        #region Year properties

        internal int YearViewMax
        {
            get { return (_YearViewMax); }
            set { _YearViewMax = value; }
        }

        #endregion

        #region TimeLine properties

        /// <summary>
        /// Gets the base interval (total minutes)
        /// </summary>
        internal double BaseInterval
        {
            get { return (_BaseInterval); }
        }

        /// <summary>
        /// Gets the TimeLine Interval header height
        /// </summary>
        internal int TimeLineIntervalHeaderHeight
        {
            get { return (Font.Height + 7); }
        }

        /// <summary>
        /// Gets the TimeLine column count
        /// </summary>
        internal int TimeLineColumnCount
        {
            get { return (_TimeLineColumnCount); }
        }

        /// <summary>
        /// Gets the TimeLine Horizontal Scroll panel
        /// </summary>
        internal TimeLineHScrollPanel TimeLineHScrollPanel
        {
            get
            {
                if (_TimeLineHScrollPanel == null)
                    _TimeLineHScrollPanel = new TimeLineHScrollPanel(this);

                return (_TimeLineHScrollPanel);
            }
        }

        internal bool HasTimeLineSlotBackgroundCallout
        {
            get
            {
                return (TimeLinePreRenderSlotBackground != null ||
                        TimeLinePostRenderSlotBackground != null);
            }
        }

        internal bool HasTimeLineGetRowCollateIdCallout
        {
            get { return (TimeLineGetRowCollateId != null); }
        }

        #endregion

        #region HasViewDisplayCustomizations

        /// <summary>
        /// Gets whether the user has defined any ViewDisplayCustomizations
        /// </summary>
        internal bool HasViewDisplayCustomizations
        {
            get { return (_ViewDisplayCustomizations != null); }
        }

        #endregion

        #region Cursor support

        /// <summary>
        /// Sets the local view cursor
        /// </summary>
        internal Cursor ViewCursor
        {
            set
            {
                try
                {
                    _SetCursor = true;

                    Cursor = value;
                }
                finally
                {
                    _SetCursor = false;
                }
            }
        }

        /// <summary>
        /// Gets the default cursor
        /// </summary>
        internal Cursor DefaultViewCursor
        {
            get { return (_DefaultViewCursor); }
        }

        #endregion

        #endregion

        #region Public methods

        #region ShowDate

        /// <summary>
        /// Navigates the current calendar view to show the given date
        /// </summary>
        /// <param name="date">Date to show in the calendar view</param>
        public void ShowDate(DateTime date)
        {
            ShowViewDate(SelectedView, date.Date);
        }

        /// <summary>
        /// Navigates the given view to show date
        /// </summary>
        /// <param name="view">View to navigate</param>
        /// <param name="date">Date to navigate to</param>
        internal void ShowViewDate(eCalendarView view, DateTime date)
        {
            if (view == eCalendarView.Day)
            {
                DayViewDate = date;
            }
            else if (view == eCalendarView.Week &&
                (date < WeekViewStartDate || date > WeekViewEndDate))
            {
                WeekViewStartDate = DateHelper.GetDateForDayOfWeek(date, DateHelper.GetFirstDayOfWeek());
                WeekViewEndDate = WeekViewStartDate.AddDays(6);
            }
            else if (view == eCalendarView.Month &&
                (date < MonthViewStartDate || date > MonthViewEndDate))
            {
                MonthViewStartDate = date.AddDays(-(date.Day - 1));
                MonthViewEndDate = MonthViewStartDate.AddMonths(1).AddMinutes(-1);
            }
            else if (view == eCalendarView.Year &&
                (date < YearViewStartDate || date > YearViewEndDate))
            {
                int monthSpan = (YearViewEndDate.Year * 12 + YearViewEndDate.Month) -
                    (YearViewStartDate.Year * 12 + YearViewStartDate.Month);

                YearViewStartDate = date.AddDays(-(date.Day - 1));
                YearViewEndDate = YearViewStartDate.AddMonths(monthSpan);
            }
            else if (view == eCalendarView.TimeLine &&
                (date < TimeLineViewScrollStartDate || date > TimeLineViewScrollEndDate))
            {
                TimeLineViewScrollStartDate = date;
            }
        }

        #endregion

        #region ScrollToTime

        /// <summary>
        /// Scrolls the Day/Week calendar view to the
        /// specified hour and minute
        /// </summary>
        /// <param name="hour">Hour to scroll to</param>
        /// <param name="minute">Minute to scroll to</param>
        public void ScrollToTime(int hour, int minute)
        {
            if (SelectedView == eCalendarView.Day || SelectedView == eCalendarView.Week)
            {
                int offset = (int)(hour * _SlotsPerHour * TimeSliceHeight +
                    (TimeSliceHeight * minute) / _TimeSlotDuration);

                offset -= (int)(StartSlice * TimeSliceHeight);

                offset = Math.Min(offset, _WeekDayVScrollPanel.ScrollBar.Maximum);
                offset = Math.Max(offset, 0);

                _WeekDayVScrollPanel.ScrollBar.Value = offset;
            }
        }

        #endregion

        #region EnsureVisible

        /// <summary>
        /// Ensures that the given Appointment is visible
        /// in the current view. It will change the view date and
        /// scroll as necessary to ensure the appointment is visible
        /// </summary>
        /// <param name="appointment">Appointment to bring into view</param>
        public void EnsureVisible(Appointment appointment)
        {
            EnsureVisible(appointment.StartTime, appointment.EndTime);
        }

        /// <summary>
        /// Ensures that the given CustomCalendarItem is visible
        /// in the current view. It will change the view date and
        /// scroll as necessary to ensure the item is visible
        /// </summary>
        /// <param name="item">Appointment to bring into view</param>
        public void EnsureVisible(CustomCalendarItem item)
        {
            EnsureVisible(item.StartTime, item.EndTime);
        }

        /// <summary>
        /// Ensures that the given calendarItem is visible
        /// in the current view. It will change the view date and
        /// scroll as necessary to ensure it is visible
        /// </summary>
        /// <param name="startTime">Item start time</param>
        /// <param name="endTime">Item end time</param>
        public void EnsureVisible(DateTime startTime, DateTime endTime)
        {
            bool multiday = (startTime < endTime &&
                (endTime.Subtract(startTime).TotalDays >= 1 ||
                DateTimeHelper.IsSameDay(startTime, endTime) == false));

            switch (SelectedView)
            {
                case eCalendarView.Day:
                    DayViewDate = startTime.Date;

                    if (multiday == false)
                        ScrollToTime(startTime.Hour, startTime.Minute);
                    break;

                case eCalendarView.Week:
                    if (startTime < WeekViewStartDate || startTime > WeekViewEndDate)
                    {
                        TimeSpan span = TimeSpan.FromDays(6);

                        if (WeekViewStartDate != DateTime.MinValue && WeekViewEndDate != DateTime.MinValue)
                            span = WeekViewEndDate.Subtract(WeekViewStartDate);

                        DateTime date = DateHelper.GetDateForDayOfWeek(startTime,
                                                                       DateHelper.GetFirstDayOfWeek());

                        WeekViewStartDate = date.Date;
                        WeekViewEndDate = WeekViewStartDate.Add(span);
                    }

                    if (multiday == false)
                        ScrollToTime(startTime.Hour, startTime.Minute);
                    break;

                case eCalendarView.Month:
                    if (startTime < MonthViewStartDate || startTime > MonthViewEndDate)
                    {
                        DateTime date = startTime.Date;

                        MonthViewStartDate = date.Date.AddDays(-(date.Day - 1));
                        MonthViewEndDate = MonthViewStartDate.AddMonths(1).AddMinutes(-1);
                    }
                    break;

                case eCalendarView.TimeLine:
                    if (startTime < TimeLineViewScrollStartDate ||
                        startTime > TimeLineViewScrollEndDate)
                    {
                        TimeLineViewScrollStartDate = startTime;
                    }
                    break;
            }
        }

        #endregion

        #region GetAppointmentView

        /// <summary>
        /// Gets the AppointmentView in the current displayed
        /// view that was created for the given appointment
        /// </summary>
        /// <returns>Reference to AppointmentView or null if not found</returns>
        public AppointmentView GetAppointmentView(Appointment appointment)
        {
            for (int i = 0; i < _CalendarPanel.SubItems.Count; i++)
            {
                BaseView view = _CalendarPanel.SubItems[i] as BaseView;

                if (view != null)
                {
                    AppointmentView apv = view.GetAppointmentView(appointment);

                    if (apv != null)
                        return (apv);
                }
            }

            return (null);
        }

        #endregion

        #region GetDateSelectionFromPoint

        /// <summary>
        /// Gets the default date selection from the given point. The startDate
        /// and endDate will vary based upon the view type (WeekDay / Month)
        /// </summary>
        /// <param name="pt">Point in question</param>
        /// <param name="startDate">[out] Start date</param>
        /// <param name="endDate">[out] End date</param>
        /// <returns>True if a valid selection exists
        /// at the given point</returns>
        public bool GetDateSelectionFromPoint(
            Point pt, out DateTime startDate, out DateTime endDate)
        {
            BaseView bv = GetViewFromPoint(pt);

            if (bv != null)
                return (bv.GetDateSelectionFromPoint(pt, out startDate, out endDate));

            startDate = new DateTime();
            endDate = new DateTime();

            return (false);
        }

        #endregion

        #region GetViewFromPoint

        /// <summary>
        /// Gets the View that contains the given point
        /// </summary>
        /// <param name="pt">Point in question</param>
        /// <returns>BaseView containing the given point, or null</returns>
        public BaseView GetViewFromPoint(Point pt)
        {
            if (_CalendarPanel != null)
            {
                for (int i = 0; i < _CalendarPanel.SubItems.Count; i++)
                {
                    BaseView bv = _CalendarPanel.SubItems[i] as BaseView;

                    if (bv != null)
                    {
                        if (bv.Bounds.Contains(pt))
                            return (bv);
                    }
                }
            }

            return (null);
        }

        #endregion

        #region GetSelectedView

        /// <summary>
        /// Gets the current selected BaseView
        /// </summary>
        /// <returns></returns>
        public BaseView GetSelectedView()
        {
            if (_CalendarPanel != null)
            {
                for (int i = 0; i < _CalendarPanel.SubItems.Count; i++)
                {
                    BaseView bv = _CalendarPanel.SubItems[i] as BaseView;

                    if (bv != null)
                    {
                        if (bv.IsViewSelected == true &&
                            bv.DisplayedOwnerKeyIndex == SelectedOwnerIndex)
                        {
                            return (bv);
                        }
                    }
                }
            }

            return (null);
        }

        #endregion

        #endregion

        #region Internal methods

        internal DateTime TimeLineAddInterval(DateTime startDate, int interval)
        {
            return ((TimeLinePeriod == eTimeLinePeriod.Years) ?
                startDate.AddYears(interval * TimeLineInterval) :
                startDate.AddMinutes(interval * BaseInterval));
        }

        #endregion

        #region HookModelEvents

        /// <summary>
        /// Hooks needed events
        /// </summary>
        /// <param name="hook">True to hook, false to unhook</param>
        private void HookModelEvents(bool hook)
        {
            if (hook == true)
            {
                _CalendarModel.AppointmentStartTimeReached +=
                    CalendarModelAppointmentStartTimeReached;

                _CalendarModel.ReminderNotification +=
                    CalendarModelReminderNotification;
            }
            else
            {
                _CalendarModel.AppointmentStartTimeReached -=
                    CalendarModelAppointmentStartTimeReached;

                _CalendarModel.ReminderNotification -=
                    CalendarModelReminderNotification;
            }
        }

        #endregion

        #region Event handling

        #region Theme Changing

        /// <summary>
        /// Called by StyleManager to notify control that style on manager has changed and that control should refresh its appearance if
        /// its style is controlled by StyleManager.
        /// </summary>
        /// <param name="newStyle">New active style.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void StyleManagerStyleChanged(eDotNetBarStyle newStyle)
        {
            if (BarFunctions.IsHandleValid(this))
                this.Invalidate(true);

            BackColor = ((Office2007Renderer)
                GlobalManager.Renderer).ColorTable.CalendarView.TimeRulerColors[0].Colors[0];
        }

        #endregion

        #region ReminderNotification

        /// <summary>
        /// ReminderNotification
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CalendarModelReminderNotification(object sender, ReminderEventArgs e)
        {
            if (AppointmentReminder != null)
                ReminderNotification(e);
        }

        /// <summary>
        /// Dispatches ReminderNotification to UI thread
        /// </summary>
        /// <param name="e"></param>
        private void ReminderNotification(ReminderEventArgs e)
        {
            if (this.InvokeRequired)
            {
                BeginInvoke(new
                    MethodInvoker(delegate { ReminderNotification(e); }));
            }
            else
            {
                AppointmentReminder(this, e);
            }
        }

        #endregion

        #region AppointmentStartTimeReached

        /// <summary>
        /// AppointmentStartTimeReached
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CalendarModelAppointmentStartTimeReached(object sender, AppointmentEventArgs e)
        {
            if (AppointmentStartTimeReached != null)
                StartTimeReachedNotification(e);
        }

        /// <summary>
        /// Dispatches AppointmentStartTimeReached to UI thread
        /// </summary>
        /// <param name="e"></param>
        private void StartTimeReachedNotification(AppointmentEventArgs e)
        {
            if (this.InvokeRequired)
            {
                BeginInvoke(new
                    MethodInvoker(delegate { StartTimeReachedNotification(e); }));
            }
            else
            {
                AppointmentStartTimeReached(this, e);
            }
        }

        #endregion

        #region AppointmentCategoryColorCollectionChanged

        /// <summary>
        /// AppointmentCategoryColorCollectionChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CategoryColorsAppointmentCategoryColorCollectionChanged(object sender, EventArgs e)
        {
            Invalidate();
        }

        #endregion

        #region TimeIndicatorCollectionChanged

        /// <summary>
        /// TimeIndicatorCollectionChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TimeIndicatorsTimeIndicatorCollectionChanged(object sender, EventArgs e)
        {
            if (TimeIndicatorsChanged != null)
                TimeIndicatorsChanged(this, EventArgs.Empty);

            Invalidate();
        }

        #endregion

        #region TimeIndicatorColorChanged

        /// <summary>
        /// Processes TimeIndicatorColorChanged events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TimeIndicatorsTimeIndicatorColorChanged(object sender, TimeIndicatorColorChangedEventArgs e)
        {
            if (TimeIndicatorColorChanged != null)
                TimeIndicatorColorChanged(this, e);

            TimeIndicator ti = e.TimeIndicator;

            if (ti.Visibility != eTimeIndicatorVisibility.Hidden &&
                (ti.IndicatorArea == eTimeIndicatorArea.All ||
                 ti.IndicatorArea == eTimeIndicatorArea.Content))
            {
                for (int i = 0; i < CalendarPanel.SubItems.Count; i++)
                {
                    BaseView view = CalendarPanel.SubItems[i] as BaseView;

                    if (view != null && view.Displayed == true)
                        InvalidateTimeIndicator(view, ti, ti.IndicatorDisplayTime);
                }
            }
        }

        #endregion

        #region TimeIndicatorTimeChanged

        /// <summary>
        /// Processes TimeIndicatorTimeChanged events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TimeIndicatorsTimeIndicatorTimeChanged(object sender, TimeIndicatorTimeChangedEventArgs e)
        {
            if (TimeIndicatorTimeChanged != null)
                TimeIndicatorTimeChanged(this, e);

            TimeIndicator ti = e.TimeIndicator;

            if (ti.Visibility != eTimeIndicatorVisibility.Hidden &&
                (ti.IndicatorArea == eTimeIndicatorArea.All ||
                 ti.IndicatorArea == eTimeIndicatorArea.Content))
            {
                for (int i = 0; i < CalendarPanel.SubItems.Count; i++)
                {
                    BaseView view = CalendarPanel.SubItems[i] as BaseView;

                    if (view != null && view.Displayed == true)
                    {
                        InvalidateTimeIndicator(view, ti, e.OldTime);
                        InvalidateTimeIndicator(view, ti, e.NewTime);
                    }
                }
            }
        }

        #endregion

        #region ViewDisplayCustomizations_CollectionChanged

        void ViewDisplayCustomizationsCollectionChanged(object sender, EventArgs e)
        {
            if (ViewDisplayCustomizationsChanged != null)
                ViewDisplayCustomizationsChanged(this, e);
        }

        #endregion

        #endregion

        #region InitDefaultViews

        /// <summary>
        /// InitDefaultViews
        /// </summary>
        private void InitDefaultViews()
        {
            _DayViewDate = DateTime.Today;
            _AutoSyncDate = _DayViewDate;
            _WeekViewStartDate = DateHelper.GetDateForDayOfWeek(DateTime.Today, DateHelper.GetFirstDayOfWeek());
            _WeekViewEndDate = WeekViewStartDate.AddDays(6);

            _MonthViewStartDate = DateTime.Today.AddDays(-(DateTime.Today.Day - 1));
            _MonthViewEndDate = MonthViewStartDate.AddMonths(1).AddDays(-1);

            _YearViewStartDate = DateTime.Today.AddDays(-(DateTime.Today.Day - 1));
            _YearViewEndDate = _YearViewStartDate.AddMonths(12).AddDays(-1);

            _TimeLineViewStartDate = DateTime.Today;
            _TimeLineViewEndDate = _TimeLineViewStartDate.AddDays(7);

            CalcTimeLineColumnCount(true);

            _NumberOfSlices = HoursPerDay * (MinutesPerHour / _TimeSlotDuration);
            _SlotsPerHour = MinutesPerHour / _TimeSlotDuration;
        }

        #endregion

        #region SetSelectedView

        /// <summary>
        /// Sets the selected view
        /// </summary>
        /// <param name="value">View to select</param>
        internal void SetSelectedView(eCalendarView value)
        {
            _SelectedView = value;

            _CalendarPanel.SubItems.Clear();

            if (IsMultiCalendar)
                InstallMultiCalendar(value);
            else
                InstallSingleCalendar(value);

            _CalendarPanel.RecalcSize();
            _CalendarPanel.Refresh();
        }

        #region Single Calendar Install

        /// <summary>
        /// Installs single-user calendar
        /// </summary>
        /// <param name="value">eCalendarView</param>
        private void InstallSingleCalendar(eCalendarView value)
        {
            switch (value)
            {
                case eCalendarView.Day:
                    SetupWeekDayView();
                    InstallView(DayView);
                    break;

                case eCalendarView.Week:
                    SetupWeekDayView();
                    InstallView(WeekView);
                    break;

                case eCalendarView.Month:
                    SetupMonthView();
                    InstallView(MonthView);
                    break;

                case eCalendarView.Year:
                    SetupYearView();
                    InstallView(YearView);
                    break;

                case eCalendarView.TimeLine:
                    SetupTimeLineView();
                    InstallView(TimeLineView);
                    break;
            }
        }

        #endregion

        #region MultiCalendar Install

        /// <summary>
        /// Installs multi-user calendar
        /// </summary>
        /// <param name="value"></param>
        private void InstallMultiCalendar(eCalendarView value)
        {
            switch (value)
            {
                case eCalendarView.Day:
                    InstallMultiDayCalendar();
                    break;

                case eCalendarView.Week:
                    InstallMultiWeekCalendar();
                    break;

                case eCalendarView.Month:
                    InstallMultiMonthCalendar();
                    break;

                case eCalendarView.Year:
                    InstallMultiYearCalendar();
                    break;

                case eCalendarView.TimeLine:
                    InstallMultiTimeLineCalendar();
                    break;
            }
        }

        #region InstallMultiDayCalendar

        /// <summary>
        /// Installs multi-user DayView calendar
        /// </summary>
        private void InstallMultiDayCalendar()
        {
            if (_MultiCalendarDayViews.Count > 0)
            {
                SetupWeekDayView();

                for (int i = 0; i < _MultiCalendarDayViews.Count; i++)
                    InstallView(_MultiCalendarDayViews[i]);
            }
        }

        #endregion

        #region InstallMultiWeekCalendar

        /// <summary>
        /// Installs multi-user WeekView calendar
        /// </summary>
        private void InstallMultiWeekCalendar()
        {
            if (_MultiCalendarWeekViews.Count > 0)
            {
                SetupWeekDayView();

                for (int i = 0; i < _MultiCalendarWeekViews.Count; i++)
                    InstallView(_MultiCalendarWeekViews[i]);
            }
        }

        #endregion

        #region InstallMultiMonthCalendar

        /// <summary>
        /// Installs multi-user MonthView calendar
        /// </summary>
        private void InstallMultiMonthCalendar()
        {
            if (_MultiCalendarMonthViews.Count > 0)
            {
                SetupMonthView();

                for (int i = 0; i < _MultiCalendarMonthViews.Count; i++)
                    InstallView(_MultiCalendarMonthViews[i]);
            }
        }

        #endregion

        #region InstallMultiYearCalendar

        /// <summary>
        /// Installs multi-user YearView calendar
        /// </summary>
        private void InstallMultiYearCalendar()
        {
            if (_MultiCalendarYearViews.Count > 0)
            {
                SetupYearView();

                for (int i = 0; i < _MultiCalendarYearViews.Count; i++)
                    InstallView(_MultiCalendarYearViews[i]);
            }
        }

        #endregion

        #region InstallMultiTimeLineCalendar

        /// <summary>
        /// Installs multi-user TimeLineView calendar
        /// </summary>
        private void InstallMultiTimeLineCalendar()
        {
            if (_MultiCalendarTimeLineViews.Count > 0)
            {
                SetupTimeLineView();

                for (int i = 0; i < _MultiCalendarTimeLineViews.Count; i++)
                    InstallView(_MultiCalendarTimeLineViews[i]);
            }
        }

        #endregion

        #region TimeRulePanel support

        /// <summary>
        /// Installs the TimeRulerPanel
        /// </summary>
        private void InstallTimerPanel()
        {
            if (_IsTimeRulerVisible == true)
            {
                if (_TimeRulerPanel == null)
                    _TimeRulerPanel = new TimeRulerPanel(this);

                _TimeRulerPanel.Displayed = true;

                _CalendarPanel.SubItems.Add(_TimeRulerPanel);
            }
        }

        /// <summary>
        /// Release the TimeRulerPanel
        /// </summary>
        private void ReleaseTimeRulerPanel()
        {
            if (_TimeRulerPanel != null)
                _TimeRulerPanel.Displayed = false;
        }

        #endregion

        #region WeekDayVScrollPanel support

        private void InstallWeekDayVScrollPanel()
        {
            if (_WeekDayVScrollPanel == null)
                _WeekDayVScrollPanel = new WeekDayVScrollPanel(this);

            _WeekDayVScrollPanel.Visible = true;
            _WeekDayVScrollPanel.Displayed = true;

            _CalendarPanel.SubItems.Add(_WeekDayVScrollPanel);
        }

        private void ReleaseWeekDayVScrollPanel()
        {
            if (_WeekDayVScrollPanel != null)
                _WeekDayVScrollPanel.Visible = false;
        }

        #endregion

        #region YearVScrollPanel support

        private void InstallYearVScrollPanel()
        {
            if (_YearVScrollPanel == null)
                _YearVScrollPanel = new YearVScrollPanel(this);

            _CalendarPanel.SubItems.Add(_YearVScrollPanel);
        }

        private void ReleaseYearVScrollPanel()
        {
            if (_YearVScrollPanel != null)
                _YearVScrollPanel.Visible = false;
        }

        #endregion

        #region TimeLineHeaderPanel support

        private void InstallTimeLineHeaderPanel()
        {
            if (_TimeLineHeaderPanel == null)
                _TimeLineHeaderPanel = new TimeLineHeaderPanel(this);

            _TimeLineHeaderPanel.Displayed = true;

            _CalendarPanel.SubItems.Add(_TimeLineHeaderPanel);
        }

        /// <summary>
        /// Release the TimeLineHeaderPanel
        /// </summary>
        private void ReleaseTimeLineHeaderPanel()
        {
            if (_TimeLineHeaderPanel != null)
                _TimeLineHeaderPanel.Displayed = false;
        }

        #endregion

        #region TimeLineHScrollPanel support

        /// <summary>
        /// Installs the TimeLine Horizontal Scroll Panel
        /// </summary>
        private void InstallTimeLineHScrollPanel()
        {
            if (_TimeLineHScrollPanel == null)
                _TimeLineHScrollPanel = new TimeLineHScrollPanel(this);

            _TimeLineHScrollPanel.ScrollPanelChanged += ScrollPanelChanged;

            _TimeLineHScrollPanel.Visible = true;
            _TimeLineHScrollPanel.Displayed = true;

            _CalendarPanel.SubItems.Add(_TimeLineHScrollPanel);
        }

        /// <summary>
        /// Handles Scroll Panel updates
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScrollPanelChanged(object sender, EventArgs e)
        {
            TimeLineViewScrollStartDate =
                _TimeLineViewStartDate.AddMinutes(_BaseInterval * TimeLineHScrollPanel.ScrollBar.Value);
        }

        /// <summary>
        /// Releases the Horizontal Scroll Panel
        /// </summary>
        private void ReleaseTimeLineHScrollPanel()
        {
            if (_TimeLineHScrollPanel != null)
            {
                _TimeLineHScrollPanel.Visible = false;

                _TimeLineHScrollPanel.ScrollPanelChanged -= ScrollPanelChanged;
            }
        }

        #endregion

        #endregion

        #region SetupView support

        #region SetupWeekDayView

        private void SetupWeekDayView()
        {
            ReleaseTimeLineHeaderPanel();
            ReleaseTimeLineHScrollPanel();
            ReleaseYearVScrollPanel();

            InstallWeekDayVScrollPanel();
            InstallTimerPanel();
        }

        #endregion

        #region SetupMonthView

        private void SetupMonthView()
        {
            ReleaseTimeRulerPanel();
            ReleaseWeekDayVScrollPanel();
            ReleaseYearVScrollPanel();
            ReleaseTimeLineHeaderPanel();
            ReleaseTimeLineHScrollPanel();
        }

        #endregion

        #region SetupYearView

        private void SetupYearView()
        {
            ReleaseTimeRulerPanel();
            ReleaseWeekDayVScrollPanel();
            ReleaseTimeLineHeaderPanel();
            ReleaseTimeLineHScrollPanel();

            InstallYearVScrollPanel();
        }

        #endregion

        #region SetupTimeLineView

        private void SetupTimeLineView()
        {
            ReleaseTimeRulerPanel();
            ReleaseWeekDayVScrollPanel();
            ReleaseYearVScrollPanel();

            InstallTimeLineHScrollPanel();
            InstallTimeLineHeaderPanel();
        }

        #endregion

        #endregion

        #region InstallView

        /// <summary>
        /// Installs the given View into the CalendarPanel
        /// </summary>
        /// <param name="view">View to install</param>
        private void InstallView(BaseView view)
        {
            if (view != null)
            {
                view.Displayed = true;
                view.IsViewSelected = true;
                view.Style = _CalendarPanel.Style;

                _CalendarPanel.SubItems.Add(view);
            }
        }

        #endregion

        #endregion

        #region AppointmentViewChange

        /// <summary>
        /// Routine to initiate the OnChange user events
        /// </summary>
        /// <param name="calendarItem">Affected item</param>
        /// <param name="startTime">New start time</param>
        /// <param name="endTime">New end time</param>
        /// <param name="viewOperation">Move, resize, etc</param>
        /// <param name="isCopyDrag"></param>
        /// <returns></returns>
        internal bool DoAppointmentViewChanging(CalendarItem calendarItem,
            DateTime startTime, DateTime endTime, eViewOperation viewOperation, bool isCopyDrag)
        {
            bool cancel = OnAppointmentViewChanging(
                calendarItem, startTime, endTime, viewOperation, isCopyDrag);

            return (cancel);
        }

        /// <summary>
        /// Routine to initiate the BeforeAppointmentViewChange event
        /// </summary>
        /// <param name="baseView">BaseView</param>
        /// <param name="calendarItem">Affected item</param>
        /// <param name="viewOperation">Move, resize, etc</param>
        /// <returns></returns>
        internal bool DoBeforeAppointmentViewChange(BaseView baseView,
            CalendarItem calendarItem, eViewOperation viewOperation)
        {
            return (OnBeforeAppointmentViewChange(baseView, calendarItem, viewOperation));
        }

        #region OnBeforeAppointmentViewChange

        /// <summary>
        /// OnBeforeAppointmentViewChange event propagation
        /// </summary>
        protected virtual bool OnBeforeAppointmentViewChange(BaseView baseView,
            CalendarItem calendarItem, eViewOperation viewOperation)
        {
            if (BeforeAppointmentViewChange != null)
            {
                bool isCopyDrag = ((baseView.IsCopyDrag == false) &&
                    (ModifierKeys & Keys.Control) == Keys.Control);

                BeforeAppointmentViewChangeEventArgs ev = new
                    BeforeAppointmentViewChangeEventArgs(calendarItem, viewOperation, isCopyDrag);

                BeforeAppointmentViewChange(this, ev);

                return (ev.Cancel);
            }

            return (false);
        }

        #endregion

        #region OnAppointmentViewChanging

        /// <summary>
        /// OnAppointmentViewChanging event propagation
        /// </summary>
        protected virtual bool OnAppointmentViewChanging(CalendarItem calendarItem,
            DateTime startTime, DateTime endTime, eViewOperation viewOperation, bool isCopyDrag)
        {
            if (AppointmentViewChanging != null)
            {
                AppointmentViewChangingEventArgs ev = new AppointmentViewChangingEventArgs(
                    calendarItem, startTime, endTime, viewOperation, isCopyDrag);

                AppointmentViewChanging(this, ev);

                return (ev.Cancel);
            }

            return (false);
        }

        #endregion

        #region DoAppointmentViewChanged

        /// <summary>
        /// DoAppointmentViewChanged event propagation
        /// </summary>
        internal virtual void DoAppointmentViewChanged(CalendarItem calendarItem,
            DateTime oldStartTime, DateTime oldEndTime, eViewOperation viewOperation, bool isCopyDrag)
        {
            if (AppointmentViewChanged != null)
            {
                AppointmentViewChanged(this, new AppointmentViewChangedEventArgs(
                        calendarItem, oldStartTime, oldEndTime, viewOperation, isCopyDrag));
            }
        }

        #endregion

        #region DoGetDisplayTemplateText

        /// <summary>
        /// DoGetDisplayTemplateText event propagation
        /// </summary>
        internal virtual string DoGetDisplayTemplateText(
            CalendarItem calendarItem, string displayTemplate, string displayText)
        {
            if (GetDisplayTemplateText != null)
            {
                GetDisplayTemplateTextEventArgs e =
                    new GetDisplayTemplateTextEventArgs(calendarItem, displayTemplate, displayText);

                GetDisplayTemplateText(this, e);

                return (e.DisplayText);
            }

            return (displayText);
        }

        #endregion

        #endregion

        #region DoTimeLineViewRenderPeriodHeader

        /// <summary>
        /// Handles invocation of DoTimeLineViewRenderPeriodHeader event
        /// </summary>
        /// <param name="g"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="bounds"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        internal bool DoTimeLineViewRenderPeriodHeader(Graphics g,
            DateTime startTime, DateTime endTime, Rectangle bounds, ref string text)
        {
            if (TimeLineRenderPeriodHeader != null)
            {
                TimeLineRenderPeriodHeaderEventArgs ev = new
                    TimeLineRenderPeriodHeaderEventArgs(g, startTime, endTime, bounds, text);

                TimeLineRenderPeriodHeader(this, ev);

                text = ev.Text;

                return (ev.Cancel);
            }

            return (false);
        }

        #endregion

        #region DoTimeLineViewPreRenderSlotBackground

        /// <summary>
        /// Handles invocation of DoTimeLineViewPreRenderSlotBackground event
        /// </summary>
        /// <param name="g"></param>
        /// <param name="endTime"></param>
        /// <param name="bounds"></param>
        /// <param name="state"></param>
        /// <param name="view"></param>
        /// <param name="startTime"></param>
        /// <returns></returns>
        internal bool DoTimeLineViewPreRenderSlotBackground(Graphics g, BaseView view,
            DateTime startTime, DateTime endTime, Rectangle bounds, ref eSlotDisplayState state)
        {
            if (TimeLinePreRenderSlotBackground != null)
            {
                TimeLinePreRenderSlotBackgroundEventArgs ev = new
                    TimeLinePreRenderSlotBackgroundEventArgs(g, view, startTime, endTime, bounds, state);

                TimeLinePreRenderSlotBackground(this, ev);

                state = ev.State;

                return (ev.Cancel);
            }

            return (false);
        }

        #endregion

        #region DoTimeLineViewPostRenderSlotBackground

        /// <summary>
        /// Handles invocation of DoTimeLineViewPostRenderSlotBackground event
        /// </summary>
        /// <param name="g"></param>
        /// <param name="endTime"></param>
        /// <param name="bounds"></param>
        /// <param name="state"></param>
        /// <param name="view"></param>
        /// <param name="startTime"></param>
        /// <returns></returns>
        internal void DoTimeLineViewPostRenderSlotBackground(Graphics g, BaseView view,
            DateTime startTime, DateTime endTime, Rectangle bounds, eSlotDisplayState state)
        {
            if (TimeLinePostRenderSlotBackground != null)
            {
                TimeLinePostRenderSlotBackgroundEventArgs ev = new
                    TimeLinePostRenderSlotBackgroundEventArgs(g, view, startTime, endTime, bounds, state);

                TimeLinePostRenderSlotBackground(this, ev);
            }
        }

        #endregion

        #region DoTimeLineViewRenderSlotBorder

        /// <summary>
        /// DoTimeLineViewRenderSlotBorder
        /// </summary>
        /// <param name="g"></param>
        /// <param name="view"></param>
        /// <param name="col"></param>
        /// <param name="hourly"></param>
        /// <param name="state"></param>
        /// <param name="pt1"></param>
        /// <param name="pt2"></param>
        /// <param name="pen"></param>
        /// <returns></returns>
        internal bool DoTimeLineViewRenderSlotBorder(
            Graphics g, TimeLineView view, int col, bool hourly,
            eSlotDisplayState state, Point pt1, Point pt2, Pen pen)
        {
            if (TimeLineRenderSlotBorder != null)
            {
                DateTime startTime = view.StartDate.AddMinutes(col * _BaseInterval);

                TimeLineRenderSlotBorderEventArgs ev = new
                    TimeLineRenderSlotBorderEventArgs(g, startTime, hourly, state, pt1, pt2, pen.Color);

                TimeLineRenderSlotBorder(this, ev);

                return (ev.Cancel);
            }

            return (false);
        }

        #endregion

        #region DoTimeLineGetRowHeight

        /// <summary>
        /// DoTimeLineGetRowHeight
        /// </summary>
        /// <param name="e"></param>
        internal void DoTimeLineGetRowHeight(TimeLineGetRowHeightEventArgs e)
        {
            if (TimeLineGetRowHeight != null)
                TimeLineGetRowHeight(this, e);
        }

        #endregion

        #region DoTimeLineGetRowCollateId

        /// <summary>
        /// DoTimeLineGetRowCollateId
        /// </summary>
        /// <param name="e"></param>
        internal void DoTimeLineGetRowCollateId(TimeLineGetRowCollateIdEventArgs e)
        {
            if (TimeLineGetRowCollateId != null)
                TimeLineGetRowCollateId(this, e);
        }

        #endregion

        #region DoPageNavigatorClick

        /// <summary>
        /// Handles invocation of PageNavigatorClick events
        /// </summary>
        /// <returns></returns>
        internal bool DoPageNavigatorClick(BaseView view,
            PageNavigator pageNavigator, PageNavigatorButton button, ref DateTime navigateTime)
        {
            if (PageNavigatorClick != null)
            {
                PageNavigatorClickEventArgs ev = new
                    PageNavigatorClickEventArgs(view, pageNavigator, button, navigateTime);

                PageNavigatorClick(this, ev);

                navigateTime = ev.NavigateTime;

                return (ev.Cancel);
            }

            return (false);
        }

        #endregion

        #region DoYearViewLinkSelected

        /// <summary>
        /// Handles invocation of DoYearViewLinkSelected event
        /// </summary>
        internal bool DoYearViewLinkSelected(
            ref DateTime startDate, ref DateTime endDate, ref eCalendarView calendarView)
        {
            if (YearViewLinkSelected != null)
            {
                LinkViewSelectedEventArgs ev = new
                    LinkViewSelectedEventArgs(startDate, endDate, calendarView);

                YearViewLinkSelected(this, ev);

                startDate = ev.StartDate;
                endDate = ev.EndDate;
                calendarView = ev.ECalendarView;

                return (ev.Cancel);
            }

            return (false);
        }

        #endregion

        #region DoYearViewDrawDayBackground

        /// <summary>
        /// Handles invocation of YearViewDrawDayBackground event
        /// </summary>
        internal bool DoYearViewDrawDayBackground(Graphics g,
            YearMonth yearMonth, DateTime date, Rectangle bounds, ref eYearViewLinkStyle style)
        {
            if (YearViewDrawDayBackground != null)
            {
                YearViewDrawDayBackgroundEventArgs ev = new
                    YearViewDrawDayBackgroundEventArgs(g, yearMonth, date, bounds, style);

                YearViewDrawDayBackground(this, ev);

                style = ev.LinkStyle;

                return (ev.Cancel);
            }

            return (false);
        }

        #endregion

        #region DoYearViewDrawDayText

        /// <summary>
        /// Handles invocation of YearViewDrawDayText event
        /// </summary>
        internal bool DoYearViewDrawDayText(Graphics g,
            YearMonth yearMonth, DateTime date, Rectangle bounds)
        {
            if (YearViewDrawDayText != null)
            {
                YearViewDrawDayTextEventArgs ev = new
                    YearViewDrawDayTextEventArgs(g, yearMonth, date, bounds);

                YearViewDrawDayText(this, ev);

                return (ev.Cancel);
            }

            return (false);
        }

        #endregion

        #region DoMonthViewPreRenderSlotBackground

        /// <summary>
        /// Handles invocation of MonthViewPreRenderSlotBackground event
        /// </summary>
        /// <param name="g"></param>
        /// <param name="endTime"></param>
        /// <param name="bounds"></param>
        /// <param name="state"></param>
        /// <param name="view"></param>
        /// <param name="startTime"></param>
        /// <returns></returns>
        internal bool DoMonthViewPreRenderSlotBackground(Graphics g, BaseView view,
            DateTime startTime, DateTime endTime, Rectangle bounds, ref eSlotDisplayState state)
        {
            if (MonthViewPreRenderSlotBackground != null)
            {
                MonthViewPreRenderSlotBackgroundEventArgs ev = new
                    MonthViewPreRenderSlotBackgroundEventArgs(g, view, startTime, endTime, bounds, state);

                MonthViewPreRenderSlotBackground(this, ev);

                state = ev.State;

                return (ev.Cancel);
            }

            return (false);
        }

        #endregion

        #region DoMonthViewPostRenderSlotBackground

        /// <summary>
        /// Handles invocation of MonthViewPostRenderSlotBackground event
        /// </summary>
        /// <param name="g"></param>
        /// <param name="endTime"></param>
        /// <param name="bounds"></param>
        /// <param name="state"></param>
        /// <param name="view"></param>
        /// <param name="startTime"></param>
        /// <returns></returns>
        internal void DoMonthViewPostRenderSlotBackground(Graphics g, BaseView view,
            DateTime startTime, DateTime endTime, Rectangle bounds, eSlotDisplayState state)
        {
            if (MonthViewPostRenderSlotBackground != null)
            {
                MonthViewPostRenderSlotBackgroundEventArgs ev = new
                    MonthViewPostRenderSlotBackgroundEventArgs(g, view, startTime, endTime, bounds, state);

                MonthViewPostRenderSlotBackground(this, ev);
            }
        }

        #endregion

        #region DoRenderDaySlotAppearanceText

        /// <summary>
        /// Handles invocation of RenderDaySlotAppearanceText events
        /// </summary>
        internal bool DoRenderDaySlotAppearanceText(Graphics g, Rectangle bounds,
            DaySlotAppearance appearance, DateTime startTime, DateTime endTime, bool selected, ref string text)
        {
            if (RenderDaySlotAppearanceText != null)
            {
                RenderDaySlotAppearanceTextEventArgs ev = new
                    RenderDaySlotAppearanceTextEventArgs(g, bounds, appearance, startTime, endTime, selected);

                RenderDaySlotAppearanceText(this, ev);

                text = ev.Text;

                return (ev.Cancel);
            }

            return (false);
        }

        #endregion

        #region DoRenderTabBackground

        /// <summary>
        /// Handles invocation of RenderTabBackground event
        /// </summary>
        internal bool DoRenderTabBackground(
            Graphics g, GraphicsPath path, BaseView view, bool isSelected)
        {
            if (RenderTabBackground != null)
            {
                RenderTabBackgroundEventArgs ev = new
                    RenderTabBackgroundEventArgs(g, path, view, isSelected);

                RenderTabBackground(this, ev);

                return (ev.Cancel);
            }

            return (false);
        }

        #endregion

        #region DoRenderTabContent

        /// <summary>
        /// Handles invocation of RenderTabContent events
        /// </summary>
        internal bool DoRenderTabContent(Graphics g,
            GraphicsPath path, BaseView view, ref string text, bool isSelected)
        {
            if (RenderTabContent != null)
            {
                RenderTabContentEventArgs ev = new
                    RenderTabContentEventArgs(g, path, view, isSelected, text);

                RenderTabContent(this, ev);

                text = ev.Text;

                return (ev.Cancel);
            }

            return (false);
        }

        #endregion

        #region DoViewLoadComplete

        /// <summary>
        /// Handles invocation of ViewLoadComplete events
        /// </summary>
        internal void DoViewLoadComplete(BaseView view)
        {
            if (ViewLoadComplete != null)
            {
                ViewLoadCompleteEventArgs ev = new
                    ViewLoadCompleteEventArgs(view);

                ViewLoadComplete(this, ev);
            }
        }

        #endregion

        #region DisplayedsOwner support

        #region DisplayedOwnersAdded

        /// <summary>
        /// This routine is called after an element has been
        /// added to the DisplayedOwners list.  It is responsible
        /// for updating the MultiCalendar views accordingly.
        /// </summary>
        internal void DisplayedOwnersAdded(int index)
        {
            if (_MultiCalendarDayViews == null)
            {
                _MultiCalendarDayViews = new CalendarViewCollection<DayView>(this);
                _MultiCalendarWeekViews = new CalendarViewCollection<WeekView>(this);
                _MultiCalendarMonthViews = new CalendarViewCollection<MonthView>(this);
                _MultiCalendarYearViews = new CalendarViewCollection<YearView>(this);
                _MultiCalendarTimeLineViews = new CalendarViewCollection<TimeLineView>(this);

                // No need to keep the single-user views
                // while we are in multi-user mode

                DayView = null;
                WeekView = null;
                MonthView = null;
                YearView = null;
                TimeLineView = null;

                SelectedOwnerIndex = 0;
            }

            // Are we appending, or inserting?

            if (index == _MultiCalendarDayViews.Count)
            {
                // Loop through all newly added DisplayOwners, creating
                // corresponding Day, Week, and Month views for each one

                for (int i = _MultiCalendarDayViews.Count; i < _DisplayedOwners.Count; i++)
                {
                    _MultiCalendarDayViews.Views.Add(null);
                    _MultiCalendarWeekViews.Views.Add(null);
                    _MultiCalendarMonthViews.Views.Add(null);
                    _MultiCalendarYearViews.Views.Add(null);
                    _MultiCalendarTimeLineViews.Views.Add(null);
                }
            }
            else
            {
                // Add the new view to the end of the list and
                // then reorder the lists to reflect the change

                _MultiCalendarDayViews.Views.Add(null);
                _MultiCalendarWeekViews.Views.Add(null);
                _MultiCalendarMonthViews.Views.Add(null);
                _MultiCalendarYearViews.Views.Add(null);
                _MultiCalendarTimeLineViews.Views.Add(null);

                ReorderDayViews(_DisplayedOwners.Count - 1, index);
                ReorderWeekViews(_DisplayedOwners.Count - 1, index);
                ReorderMonthViews(_DisplayedOwners.Count - 1, index);
                ReorderYearViews(_DisplayedOwners.Count - 1, index);
                ReorderTimeLineViews(_DisplayedOwners.Count - 1, index);
            }

            // Reflect the changes to the display

            SetSelectedView(SelectedView);

            // Tell the user things changed

            OnDisplayedOwnersChanged();
        }

        #endregion

        #region DisplayedOwnersRemoved

        /// <summary>
        /// This routine is called after an element has been
        /// removed from the DisplayedOwners list.  It is responsible
        /// for updating the MultiCalendar views accordingly.
        /// </summary>
        /// <param name="start">Starting index to remove</param>
        /// <param name="end">Ending index to remove</param>
        internal void DisplayedOwnersRemoved(int start, int end)
        {
            // Remove each view individually

            for (int i = end - 1; i >= start; i--)
                RemoveView(i);

            // If all multi-user entries have been removed
            // then revert back to single-user mode

            if (_MultiCalendarDayViews == null || _MultiCalendarDayViews.Count == 0)
            {
                _MultiCalendarDayViews = null;
                _MultiCalendarWeekViews = null;
                _MultiCalendarMonthViews = null;
                _MultiCalendarYearViews = null;
                _MultiCalendarTimeLineViews = null;
            }

            // Reflect the changes to the user

            int n = _SelectedOwnerIndex;

            if (n >= _DisplayedOwners.Count)
                n = _DisplayedOwners.Count - 1;

            _SelectedOwnerIndex = -1;
            SelectedOwnerIndex = n;

            SetSelectedView(_SelectedView);

            // Tell the user things changed

            OnDisplayedOwnersChanged();
        }

        /// <summary>
        /// Removes individual views
        /// </summary>
        /// <param name="index">Index to remove</param>
        private void RemoveView(int index)
        {
            // Day views

            if (_MultiCalendarDayViews.Views[index] != null)
                _MultiCalendarDayViews.Views[index].Dispose();

            _MultiCalendarDayViews.Views.RemoveAt(index);

            for (int i = index; i < _MultiCalendarDayViews.Views.Count; i++)
            {
                if (_MultiCalendarDayViews.Views[i] != null)
                    _MultiCalendarDayViews.Views[i].DisplayedOwnerKeyIndex = i;
            }

            // Week views

            if (_MultiCalendarWeekViews.Views[index] != null)
                _MultiCalendarWeekViews.Views[index].Dispose();

            _MultiCalendarWeekViews.Views.RemoveAt(index);

            for (int i = index; i < _MultiCalendarWeekViews.Views.Count; i++)
            {
                if (_MultiCalendarWeekViews.Views[i] != null)
                    _MultiCalendarWeekViews.Views[i].DisplayedOwnerKeyIndex = i;
            }

            // Month views

            if (_MultiCalendarMonthViews.Views[index] != null)
                _MultiCalendarMonthViews.Views[index].Dispose();

            _MultiCalendarMonthViews.Views.RemoveAt(index);

            for (int i = index; i < _MultiCalendarMonthViews.Views.Count; i++)
            {
                if (_MultiCalendarMonthViews.Views[i] != null)
                    _MultiCalendarMonthViews.Views[i].DisplayedOwnerKeyIndex = i;
            }

            // Year views

            if (_MultiCalendarYearViews.Views[index] != null)
                _MultiCalendarYearViews.Views[index].Dispose();

            _MultiCalendarYearViews.Views.RemoveAt(index);

            for (int i = index; i < _MultiCalendarYearViews.Views.Count; i++)
            {
                if (_MultiCalendarYearViews.Views[i] != null)
                    _MultiCalendarYearViews.Views[i].DisplayedOwnerKeyIndex = i;
            }

            // TimeLine views

            if (_MultiCalendarTimeLineViews.Views[index] != null)
                _MultiCalendarTimeLineViews.Views[index].Dispose();

            _MultiCalendarTimeLineViews.Views.RemoveAt(index);

            for (int i = index; i < _MultiCalendarTimeLineViews.Views.Count; i++)
            {
                if (_MultiCalendarTimeLineViews.Views[i] != null)
                    _MultiCalendarTimeLineViews.Views[i].DisplayedOwnerKeyIndex = i;
            }
        }

        #endregion

        #region DisplayedOwnersSet

        /// <summary>
        /// This routine is called after an element has been
        /// reset in the DisplayedOwners list.  It is responsible
        /// for updating the MultiCalendar views accordingly.
        /// </summary>
        /// <param name="index"></param>
        internal void DisplayedOwnersSet(int index)
        {
            // Day view

            if (_MultiCalendarDayViews.Views[index] != null)
                SetView(_MultiCalendarDayViews.Views[index], index);

            // Week view

            if (_MultiCalendarWeekViews.Views[index] != null)
                SetView(_MultiCalendarWeekViews.Views[index], index);

            // Month view

            if (_MultiCalendarMonthViews.Views[index] != null)
                SetView(_MultiCalendarMonthViews.Views[index], index);

            // Year view

            if (_MultiCalendarYearViews.Views[index] != null)
                SetView(_MultiCalendarYearViews.Views[index], index);

            // TimeLine view

            if (_MultiCalendarTimeLineViews.Views[index] != null)
                SetView(_MultiCalendarTimeLineViews.Views[index], index);

            if (SelectedOwnerIndex == index)
            {
                _SelectedOwnerIndex = -1;

                SelectedOwnerIndex = index;
            }

            // Tell the user things changed

            OnDisplayedOwnersChanged();
        }

        #endregion

        #region GetViewIndexFromPoint

        /// <summary>
        /// Gets the view index from a given Point
        /// </summary>
        /// <param name="pt">Point</param>
        /// <returns></returns>
        internal int GetViewIndexFromPoint(Point pt)
        {
            if (IsMultiCalendar == true)
            {
                switch (SelectedView)
                {
                    case eCalendarView.Day:
                        for (int i = 0; i < MultiCalendarDayViews.Count; i++)
                        {
                            if (MultiCalendarDayViews.Views[i].Bounds.Contains(pt))
                                return (i);
                        }
                        break;

                    case eCalendarView.Week:
                        for (int i = 0; i < MultiCalendarWeekViews.Count; i++)
                        {
                            if (MultiCalendarWeekViews.Views[i].Bounds.Contains(pt))
                                return (i);
                        }
                        break;

                    case eCalendarView.Month:
                        for (int i = 0; i < MultiCalendarMonthViews.Count; i++)
                        {
                            if (MultiCalendarMonthViews.Views[i].Bounds.Contains(pt))
                                return (i);
                        }
                        break;

                    case eCalendarView.Year:
                        for (int i = 0; i < MultiCalendarYearViews.Count; i++)
                        {
                            if (MultiCalendarYearViews.Views[i].Bounds.Contains(pt))
                                return (i);
                        }
                        break;

                    case eCalendarView.TimeLine:
                        for (int i = 0; i < MultiCalendarTimeLineViews.Count; i++)
                        {
                            if (MultiCalendarTimeLineViews.Views[i].Bounds.Contains(pt))
                                return (i);
                        }
                        break;
                }
            }

            return (-1);
        }

        #endregion

        #region ReorderViews

        #region ReorderViews

        /// <summary>
        /// Reorders views in the MultiCalendar and
        /// DisplayOwner arrays
        /// </summary>
        /// <param name="startIndex">Starting index</param>
        /// <param name="endIndex">Ending index</param>
        internal void ReorderViews(int startIndex, int endIndex)
        {
            if (IsMultiCalendar == true && startIndex != endIndex)
            {
                try
                {
                    DisplayedOwners.SuspendUpdate = true;

                    ReorderDayViews(startIndex, endIndex);
                    ReorderWeekViews(startIndex, endIndex);
                    ReorderMonthViews(startIndex, endIndex);
                    ReorderYearViews(startIndex, endIndex);
                    ReorderTimeLineViews(startIndex, endIndex);

                    ReorderDisplayedOwners(startIndex, endIndex);
                }
                finally
                {
                    DisplayedOwners.SuspendUpdate = false;
                }

                SelectedOwnerIndex = endIndex;
                SetSelectedView(SelectedView);

                // Tell the user things changed

                OnDisplayedOwnersChanged();
            }
        }

        /// <summary>
        /// SelectedViewChanged event propagation
        /// </summary>
        protected virtual void OnDisplayedOwnersChanged()
        {
            if (DisplayedOwnersChanged != null)
                DisplayedOwnersChanged(this, EventArgs.Empty);
        }

        #endregion

        #region ReorderDayViews

        /// <summary>
        /// Reorders Day view in the MultiCalendar and
        /// DisplayOwner arrays
        /// </summary>
        /// <param name="startIndex">Starting index</param>
        /// <param name="endIndex">Ending index</param>
        private void ReorderDayViews(int startIndex, int endIndex)
        {
            DayView dayView = MultiCalendarDayViews.Views[startIndex];

            int n = (startIndex > endIndex) ? -1 : 1;

            while (startIndex != endIndex)
            {
                MultiCalendarDayViews.Views[startIndex] = MultiCalendarDayViews.Views[startIndex + n];

                if (MultiCalendarDayViews.Views[startIndex] != null)
                    MultiCalendarDayViews.Views[startIndex].DisplayedOwnerKeyIndex = startIndex;

                startIndex += n;
            }

            MultiCalendarDayViews.Views[endIndex] = dayView;

            if (dayView != null)
                dayView.DisplayedOwnerKeyIndex = endIndex;
        }

        #endregion

        #region ReorderWeekViews

        /// <summary>
        /// Reorders Week view in the MultiCalendar and
        /// DisplayOwner arrays
        /// </summary>
        /// <param name="startIndex">Starting index</param>
        /// <param name="endIndex">Ending index</param>
        private void ReorderWeekViews(int startIndex, int endIndex)
        {
            WeekView weekView = MultiCalendarWeekViews.Views[startIndex];

            int n = (startIndex > endIndex) ? -1 : 1;

            while (startIndex != endIndex)
            {
                MultiCalendarWeekViews.Views[startIndex] = MultiCalendarWeekViews.Views[startIndex + n];

                if (MultiCalendarWeekViews.Views[startIndex] != null)
                    MultiCalendarWeekViews.Views[startIndex].DisplayedOwnerKeyIndex = startIndex;

                startIndex += n;
            }

            MultiCalendarWeekViews.Views[endIndex] = weekView;

            if (weekView != null)
                weekView.DisplayedOwnerKeyIndex = endIndex;
        }

        #endregion

        #region ReorderMonthViews

        /// <summary>
        /// Reorders Month view in the MultiCalendar and
        /// DisplayOwner arrays
        /// </summary>
        /// <param name="startIndex">Starting index</param>
        /// <param name="endIndex">Ending index</param>
        private void ReorderMonthViews(int startIndex, int endIndex)
        {
            MonthView monthView = MultiCalendarMonthViews.Views[startIndex];

            int n = (startIndex > endIndex) ? -1 : 1;

            while (startIndex != endIndex)
            {
                MultiCalendarMonthViews.Views[startIndex] = MultiCalendarMonthViews.Views[startIndex + n];

                if (MultiCalendarMonthViews.Views[startIndex] != null)
                    MultiCalendarMonthViews.Views[startIndex].DisplayedOwnerKeyIndex = startIndex;

                startIndex += n;
            }

            MultiCalendarMonthViews.Views[endIndex] = monthView;

            if (monthView != null)
                monthView.DisplayedOwnerKeyIndex = endIndex;
        }

        #endregion

        #region ReorderYearViews

        /// <summary>
        /// Reorders Year view in the MultiCalendar and
        /// DisplayOwner arrays
        /// </summary>
        /// <param name="startIndex">Starting index</param>
        /// <param name="endIndex">Ending index</param>
        private void ReorderYearViews(int startIndex, int endIndex)
        {
            YearView yearView = MultiCalendarYearViews.Views[startIndex];

            int n = (startIndex > endIndex) ? -1 : 1;

            while (startIndex != endIndex)
            {
                MultiCalendarYearViews.Views[startIndex] = MultiCalendarYearViews.Views[startIndex + n];

                if (MultiCalendarYearViews.Views[startIndex] != null)
                    MultiCalendarYearViews.Views[startIndex].DisplayedOwnerKeyIndex = startIndex;

                startIndex += n;
            }

            MultiCalendarYearViews.Views[endIndex] = yearView;

            if (yearView != null)
                yearView.DisplayedOwnerKeyIndex = endIndex;
        }

        #endregion

        #region ReorderTimeLineViews

        /// <summary>
        /// Reorders TimeLine view in the MultiCalendar and
        /// DisplayOwner arrays
        /// </summary>
        /// <param name="startIndex">Starting index</param>
        /// <param name="endIndex">Ending index</param>
        private void ReorderTimeLineViews(int startIndex, int endIndex)
        {
            TimeLineView timeView = MultiCalendarTimeLineViews.Views[startIndex];

            int n = (startIndex > endIndex) ? -1 : 1;

            while (startIndex != endIndex)
            {
                MultiCalendarTimeLineViews.Views[startIndex] =
                    MultiCalendarTimeLineViews.Views[startIndex + n];

                if (MultiCalendarTimeLineViews.Views[startIndex] != null)
                    MultiCalendarTimeLineViews.Views[startIndex].DisplayedOwnerKeyIndex = startIndex;

                startIndex += n;
            }

            MultiCalendarTimeLineViews.Views[endIndex] = timeView;

            if (timeView != null)
                timeView.DisplayedOwnerKeyIndex = endIndex;
        }

        #endregion

        #region ReorderDisplayedOwners

        /// <summary>
        /// Reorders the DisplayedOwner list
        /// </summary>
        /// <param name="startIndex">Starting index</param>
        /// <param name="endIndex">Ending index</param>
        private void ReorderDisplayedOwners(int startIndex, int endIndex)
        {
            string owner = DisplayedOwners[startIndex];

            int n = (startIndex > endIndex) ? -1 : 1;

            while (startIndex != endIndex)
            {
                DisplayedOwners[startIndex] = DisplayedOwners[startIndex + n];
                startIndex += n;
            }

            DisplayedOwners[endIndex] = owner;
        }

        #endregion

        #endregion

        #endregion

        #region NewView

        /// <summary>
        /// Creates a new calendar base view of
        /// the given type
        /// </summary>
        /// <param name="type">Type of view to create</param>
        /// <param name="index">DisplayedOwner index</param>
        /// <returns>Created view</returns>
        internal object NewView(Type type, int index)
        {
            if (type == typeof(DayView))
                return (NewDayView(index));

            if (type == typeof(WeekView))
                return (NewWeekView(index));

            if (type == typeof(MonthView))
                return (NewMonthView(index));

            if (type == typeof(YearView))
                return (NewYearView(index));

            return (NewTimeLineView(index));
        }

        /// <summary>
        /// Creates new DayViews
        /// </summary>
        /// <param name="index">DisplayedOwner index</param>
        /// <returns>Created DayView</returns>
        private DayView NewDayView(int index)
        {
            DayView dv = new DayView(this);

            dv.StartDate = _DayViewDate;
            dv.EndDate = _DayViewDate;

            SetView(dv, index);

            return (dv);
        }

        /// <summary>
        /// Creates new WeekViews
        /// </summary>
        /// <param name="index">DisplayedOwner index</param>
        /// <returns>Created WeekView</returns>
        private WeekView NewWeekView(int index)
        {
            WeekView wv = new WeekView(this);

            wv.StartDate = _WeekViewStartDate;
            wv.EndDate = _WeekViewEndDate;

            SetView(wv, index);

            return (wv);
        }

        /// <summary>
        /// Creates new MonthViews
        /// </summary>
        /// <param name="index">DisplayedOwner index</param>
        /// <returns>Created MonthView</returns>
        private MonthView NewMonthView(int index)
        {
            MonthView mv = new MonthView(this);

            mv.StartDate = _MonthViewStartDate;
            mv.EndDate = _MonthViewEndDate;

            mv.IsSideBarVisible = _IsMonthSideBarVisible;

            SetView(mv, index);

            return (mv);
        }

        /// <summary>
        /// Creates new YearView
        /// </summary>
        /// <param name="index">DisplayedOwner index</param>
        /// <returns>Created YearView</returns>
        private YearView NewYearView(int index)
        {
            YearView yv = new YearView(this);

            yv.StartDate = _YearViewStartDate;
            yv.EndDate = _YearViewEndDate;

            SetView(yv, index);

            return (yv);
        }

        /// <summary>
        /// Creates new TimeLineViews
        /// </summary>
        /// <param name="index">DisplayedOwner index</param>
        /// <returns>Created WeekView</returns>
        private TimeLineView NewTimeLineView(int index)
        {
            TimeLineView tv = new TimeLineView(this, eCalendarView.TimeLine);

            tv.StartDate = _TimeLineViewStartDate;
            tv.EndDate = _TimeLineViewEndDate;

            SetView(tv, index);

            return (tv);
        }

        #endregion

        #region SetView

        /// <summary>
        /// Completes the setup of the created view
        /// </summary>
        /// <param name="view">Newly created view</param>
        /// <param name="index">DisplayedOwner index</param>
        private void SetView(BaseView view, int index)
        {
            view.DisplayedOwnerKeyIndex = index;
            view.OwnerKey = _DisplayedOwners[index];

            // Check to see it the CalendarModel has
            // any new info to give us for this view

            Owner owner = _CalendarModel.Owners[view.OwnerKey];

            if (owner != null)
            {
                view.DisplayName = owner.DisplayName;
                view.CalendarColor = owner.ColorScheme;
            }
            else
            {
                view.DisplayName = view.OwnerKey;
                view.CalendarColor = GetViewColor(view, index);
            }

            // View is all setup, let it know it
            // needs to recalculate it's entire layout

            view.ResetView();
            view.NeedRecalcLayout = true;
        }

        /// <summary>
        /// Gets the default eCalendarColor for the view
        /// </summary>
        /// <param name="view">View in question</param>
        /// <param name="index">Index of view</param>
        /// <returns>View color</returns>
        private eCalendarColor GetViewColor(BaseView view, int index)
        {
            if (IsMultiCalendar == true)
            {
                BaseView bv = MultiCalendarDayViews.Views[index];

                if (bv != null && bv != view)
                    return (bv.CalendarColor);

                bv = MultiCalendarWeekViews.Views[index];

                if (bv != null && bv != view)
                    return (bv.CalendarColor);

                bv = MultiCalendarMonthViews.Views[index];

                if (bv != null && bv != view)
                    return (bv.CalendarColor);

                bv = MultiCalendarYearViews.Views[index];

                if (bv != null && bv != view)
                    return (bv.CalendarColor);

                bv = MultiCalendarTimeLineViews.Views[index];

                if (bv != null && bv != view)
                    return (bv.CalendarColor);
            }
            else
            {
                if (_DayView != null && view != _DayView)
                    return (_DayView.CalendarColor);

                if (_WeekView != null && view != _WeekView)
                    return (_WeekView.CalendarColor);

                if (_MonthView != null && view != _MonthView)
                    return (_MonthView.CalendarColor);

                if (_YearView != null && view != _YearView)
                    return (_YearView.CalendarColor);

                if (_TimeLineView != null && view != _TimeLineView)
                    return (_TimeLineView.CalendarColor);
            }

            return (DefaultCalendarColor);
        }

        #endregion

        #region UpdateAltViewCalendarColor

        /// <summary>
        /// Updates all view CalendarColor settings
        /// </summary>
        /// <param name="color">New color</param>
        /// <param name="index">Multi-user index</param>
        internal void UpdateAltViewCalendarColor(eCalendarColor color, int index)
        {
            if (IsMultiCalendar == true)
            {
                if (MultiCalendarDayViews.Views[index] != null)
                    MultiCalendarDayViews.Views[index].CalendarColor = color;

                if (MultiCalendarWeekViews.Views[index] != null)
                    MultiCalendarWeekViews.Views[index].CalendarColor = color;

                if (MultiCalendarMonthViews.Views[index] != null)
                    MultiCalendarMonthViews.Views[index].CalendarColor = color;

                if (MultiCalendarYearViews.Views[index] != null)
                    MultiCalendarYearViews.Views[index].CalendarColor = color;

                if (MultiCalendarTimeLineViews.Views[index] != null)
                    MultiCalendarTimeLineViews.Views[index].CalendarColor = color;
            }
            else
            {
                if (_DayView != null)
                    _DayView.CalendarColor = color;

                if (_WeekView != null)
                    _WeekView.CalendarColor = color;

                if (_MonthView != null)
                    _MonthView.CalendarColor = color;

                if (_YearView != null)
                    _YearView.CalendarColor = color;

                if (_TimeLineView != null)
                    _TimeLineView.CalendarColor = color;
            }
        }

        #endregion

        #region OnMousewheel support

        /// <summary>
        /// Mouse Wheel support
        /// </summary>
        /// <param name="hWnd">Window handle</param>
        /// <param name="wParam">wParam</param>
        /// <param name="lParam">lParam</param>
        /// <returns>false</returns>
        protected override bool OnMouseWheel(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
        {
            Rectangle r = this.DisplayRectangle;
            r.Location = this.PointToScreen(r.Location);

            Point mousePos = MousePosition;

            if (r.Contains(mousePos))
            {
                IntPtr handle = NativeFunctions.WindowFromPoint(new NativeFunctions.POINT(mousePos));
                Control c = FromChildHandle(handle) ?? FromHandle(handle);

                if (c is CalendarView)
                {
                    int detents = WinApi.HIWORD(wParam);

                    int delta = (detents / SystemInformation.MouseWheelScrollDelta);
                    int value = -(delta * SystemInformation.MouseWheelScrollLines);

                    switch (SelectedView)
                    {
                        case eCalendarView.Week:
                        case eCalendarView.Day:
                            value *= (int)TimeSliceHeight;
                            value += _WeekDayVScrollPanel.ScrollBar.Value;

                            value = Math.Max(value, 0);
                            value = Math.Min(value, _WeekDayVScrollPanel.ScrollBar.Maximum);

                            _WeekDayVScrollPanel.ScrollBar.Value = value;
                            break;

                        case eCalendarView.Year:
                            if (_YearVScrollPanel.Visible == true)
                            {
                                value *= 10;
                                value += _YearVScrollPanel.ScrollBar.Value;

                                value = Math.Max(value, 0);
                                value = Math.Min(value, _YearVScrollPanel.ScrollBar.Maximum);

                                _YearVScrollPanel.ScrollBar.Value = value;
                            }
                            break;

                        case eCalendarView.TimeLine:
                            if (CalendarPanel.VScrollBar != null && CalendarPanel.VScrollBar.Visible)
                            {
                                value = -delta * TimeLineHeight;
                                value += CalendarPanel.VScrollBar.Value;

                                value = Math.Max(value, 0);
                                value = Math.Min(value, CalendarPanel.VScrollBar.Maximum);

                                CalendarPanel.VScrollBar.Value = value;
                            }
                            break;
                    }
                }
            }

            return (false);
        }

        #endregion

        #region OnMouseDown support

        /// <summary>
        /// OnMouseDown
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            this.Focus();
        }

        #endregion

        #region ProcessCmdKey

        /// <summary>
        /// Processes KeyDown events
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            BaseView view = GetSelectedView();

            if (view != null)
            {
                KeyEventArgs args = new KeyEventArgs(keyData);

                view.InternalKeyDown(args);

                if (args.Handled == true)
                    return (true);
            }

            return (base.ProcessCmdKey(ref msg, keyData));
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);

            BaseView view = GetSelectedView();

            if (view != null)
                view.InternalKeyUp(e);
        }

        #endregion

        #region OnResize handling

        /// <summary>
        /// Control resize processing
        /// </summary>
        /// <param name="e"></param>
        protected override void OnResize(EventArgs e)
        {
            _CalendarPanel.Bounds = this.ClientRectangle;

            _CalendarPanel.RecalcSize();
            _CalendarPanel.Refresh();

            base.OnResize(e);
        }

        #endregion

        #region OnCursorChanged

        /// <summary>
        /// OnCursorChanged
        /// </summary>
        /// <param name="e"></param>
        protected override void OnCursorChanged(EventArgs e)
        {
            base.OnCursorChanged(e);

            if (_SetCursor == false)
                _DefaultViewCursor = Cursor;
        }

        #endregion

        #region Trial Version
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
#if TRIAL
            Graphics g = e.Graphics;
            using (Font font = new Font(this.Font.FontFamily, 14, FontStyle.Bold))
            {
                using (SolidBrush brush = new SolidBrush(Color.FromArgb(40, Color.Black)))
                    g.DrawString("DotNetBar Trial", font, brush, (this.ClientRectangle.Width - 64) / 2, this.ClientRectangle.Height - font.SizeInPoints - 14);
            }
#endif
        }
        #endregion
    }

    #region enums

    /// <summary>
    /// Defines the available view operations
    /// </summary>
    public enum eViewOperation
    {
        AppointmentMove,                // Appointment move
        AppointmentResize               // Appointment resize
    }

    /// <summary>
    /// Defines views available on CalendarView control.
    /// </summary>
    public enum eCalendarView
    {
        Day,
        Week,
        Month,
        Year,
        TimeLine,

        None
    }

    /// <summary>
    /// Defines TimeLine interval period
    /// </summary>
    public enum eTimeLinePeriod
    {
        Minutes,
        Hours,
        Days,
        Years
    }

    /// <summary>
    /// Defines Condensed View visibility
    /// </summary>
    public enum eCondensedViewVisibility
    {
        AllResources,
        SelectedResource,
        Hidden
    }

    /// <summary>
    /// Defines link mode for YearView dates containing appointments
    /// </summary>
    [Flags]
    public enum eYearViewDayLink
    {
        None = 0,

        Click = 1<<0,
        CtrlClick = 1<<1,
        DoubleClick = 1<<2
    }

    /// <summary>
    /// Defines link action for YearView dates containing Appointments or CalendarItems
    /// </summary>
    public enum eYearViewLinkAction
    {
        GoToDate,
        GoToFirstCalendarItem,
    }

    /// <summary>
    /// Defines link mode for YearView dates containing appointments
    /// </summary>
    public enum eYearViewLinkStyle
    {
        None,

        Style1,
        Style2,
        Style3,
        Style4,
        Style5
    }

    [Flags]
    public enum eSlotDisplayState
    {
        None = 0,

        Work = (1<<0),
        Selected = (1<<2)
    }

    #endregion
    
    #region ValueChangedEventArgs

    /// <summary>
    /// Generic ValueChangedEventArgs
    /// </summary>
    /// <typeparam name="T1">oldValue type</typeparam>
    /// <typeparam name="T2">newValue type</typeparam>
    public class ValueChangedEventArgs<T1, T2> : EventArgs
    {
        #region Private variables

        private T1 _OldValue;
        private T2 _NewValue;

        #endregion

        public ValueChangedEventArgs(T1 oldValue, T2 newValue)
        {
            _OldValue = oldValue;
            _NewValue = newValue;
        }

        #region Public properties

        /// <summary>
        /// Gets the old value
        /// </summary>
        public T1 OldValue
        {
            get { return (_OldValue); }
        }

        /// <summary>
        /// Gets the new value
        /// </summary>
        public T2 NewValue
        {
            get { return (_NewValue); }
        }

        #endregion
    }

    #endregion

    #region SelectedViewEventArgs
    /// <summary>
    /// SelectedViewEventArgs
    /// </summary>
    public class SelectedViewEventArgs : ValueChangedEventArgs<eCalendarView, eCalendarView>
    {
        public SelectedViewEventArgs(eCalendarView oldValue, eCalendarView newValue)
            : base(oldValue, newValue)
        {
        }
    }
    #endregion

    #region ModelEventArgs
    /// <summary>
    /// ModelEventArgs
    /// </summary>
    public class ModelEventArgs : ValueChangedEventArgs<CalendarModel, CalendarModel>
    {
        public ModelEventArgs(CalendarModel oldValue, CalendarModel newValue)
            : base(oldValue, newValue)
        {
        }
    }
    #endregion

    #region DateSelectionEventArgs
    /// <summary>
    /// DateSelectionEventArgs
    /// </summary>
    public class DateSelectionEventArgs : ValueChangedEventArgs<DateTime?, DateTime?>
    {
        public DateSelectionEventArgs(DateTime? oldValue, DateTime? newValue)
            : base(oldValue, newValue)
        {
        }
    }
    #endregion

    #region LabelTimeSlotsChangedEventArgs
    /// <summary>
    /// LabelTimeSlotsChangedEventArgs
    /// </summary>
    public class LabelTimeSlotsChangedEventArgs : ValueChangedEventArgs<bool, bool>
    {
        public LabelTimeSlotsChangedEventArgs(bool oldValue, bool newValue)
            : base(oldValue, newValue)
        {
        }
    }
    #endregion

    #region TimeSlotDurationChangedEventArgs
    /// <summary>
    /// TimeSlotDurationChangedEventArgs
    /// </summary>
    public class TimeSlotDurationChangedEventArgs : ValueChangedEventArgs<int, int>
    {
        public TimeSlotDurationChangedEventArgs(int oldValue, int newValue)
            : base(oldValue, newValue)
        {
        }
    }
    #endregion

    #region Is24HourFormatChangedEventArgs
    /// <summary>
    /// Is24HourFormatChangedEventArgs
    /// </summary>
    public class Is24HourFormatChangedEventArgs : ValueChangedEventArgs<bool, bool>
    {
        public Is24HourFormatChangedEventArgs(bool oldValue, bool newValue)
            : base(oldValue, newValue)
        {
        }
    }
    #endregion

    #region IsMonthSideBarVisibleChangedEventArgs
    /// <summary>
    /// IsMonthSideBarVisibleChangedEventArgs
    /// </summary>
    public class IsMonthSideBarVisibleChangedEventArgs : ValueChangedEventArgs<bool, bool>
    {
        public IsMonthSideBarVisibleChangedEventArgs(bool oldValue, bool newValue)
            : base(oldValue, newValue)
        {
        }
    }
    #endregion

    #region IsMonthMoreItemsIndicatorVisibleChangedEventArgs
    /// <summary>
    /// IsMonthMoreItemsIndicatorVisibleChangedEventArgs
    /// </summary>
    public class IsMonthMoreItemsIndicatorVisibleChangedEventArgs : ValueChangedEventArgs<bool, bool>
    {
        public IsMonthMoreItemsIndicatorVisibleChangedEventArgs(bool oldValue, bool newValue)
            : base(oldValue, newValue)
        {
        }
    }
    #endregion

    #region IsTimeRulerVisibleChangedEventArgs
    /// <summary>
    /// IsTimeRulerVisibleChangedEventArgs
    /// </summary>
    public class IsTimeRulerVisibleChangedEventArgs : ValueChangedEventArgs<bool, bool>
    {
        public IsTimeRulerVisibleChangedEventArgs(bool oldValue, bool newValue)
            : base(oldValue, newValue)
        {
        }
    }
    #endregion

    #region BeforeAppointmentViewChangeEventArgs
    /// <summary>
    /// BeforeAppointmentViewChangeEventArgs
    /// </summary>
    public class BeforeAppointmentViewChangeEventArgs : CancelEventArgs
    {
        #region Private variables

        private CalendarItem _CalendarItem;
        private eViewOperation _eViewOperation;
        private bool _IsCopyDrag;

        #endregion

        public BeforeAppointmentViewChangeEventArgs(
            CalendarItem calendarItem, eViewOperation eViewOperation, bool isCopyDrag)
        {
            _CalendarItem = calendarItem;
            _eViewOperation = eViewOperation;
            _IsCopyDrag = isCopyDrag;
        }

        #region Public properties

        /// <summary>
        /// Gets the CalendarItem being affected
        /// </summary>
        public CalendarItem CalendarItem
        {
            get { return (_CalendarItem); }
        }

        /// <summary>
        /// Gets the operation to be performed
        /// </summary>
        public eViewOperation eViewOperation
        {
            get { return (_eViewOperation); }
        }

        /// <summary>
        /// Gets whether the operation is being 
        /// performed on a drag created copy item
        /// </summary>
        public bool IsCopyDrag
        {
            get { return (_IsCopyDrag); }
        }

        #endregion
    }
    #endregion

    #region AppointmentViewChangingEventArgs
    /// <summary>
    /// AppointmentViewChangingEventArgs
    /// </summary>
    public class AppointmentViewChangingEventArgs : CancelEventArgs
    {
        #region Private variables

        private CalendarItem _CalendarItem;

        private DateTime _StartTime;
        private DateTime _EndTime;

        private eViewOperation _eViewOperation;
        private bool _IsCopyDrag;

        #endregion

        public AppointmentViewChangingEventArgs(CalendarItem calendarItem,
            DateTime startTime, DateTime endTime, eViewOperation eViewOperation, bool isCopyDrag)
        {
            _CalendarItem = calendarItem;

            _StartTime = startTime;
            _EndTime = endTime;

            _eViewOperation = eViewOperation;
            _IsCopyDrag = isCopyDrag;
        }

        #region Public properties

        /// <summary>
        /// Gets the CalendarItem being changed
        /// </summary>
        public CalendarItem CalendarItem
        {
            get { return (_CalendarItem); }
        }

        /// <summary>
        /// Gets the new StartTime to be applied
        /// </summary>
        public DateTime StartTime
        {
            get { return (_StartTime); }
        }

        /// <summary>
        /// Gets the new EndTime to be applied
        /// </summary>
        public DateTime EndTime
        {
            get { return (_EndTime); }
        }

        /// <summary>
        /// Gets the operation to be performed
        /// </summary>
        public eViewOperation eViewOperation
        {
            get { return (_eViewOperation); }
        }

        /// <summary>
        /// Gets whether the operation is being 
        /// performed on a drag created copy item
        /// </summary>
        public bool IsCopyDrag
        {
            get { return (_IsCopyDrag); }
        }
        #endregion
    }
    #endregion

    #region AppointmentViewChangedEventArgs
    /// <summary>
    /// AppointmentViewChangedEventArgs
    /// </summary>
    public class AppointmentViewChangedEventArgs : EventArgs
    {
        #region Private variables

        private CalendarItem _CalendarItem;

        private DateTime _OldStartTime;
        private DateTime _OldEndTime;

        private eViewOperation _eViewOperation;
        private bool _IsCopyDrag;

        #endregion

        public AppointmentViewChangedEventArgs(CalendarItem calendarItem,
            DateTime oldStartTime, DateTime oldEndTime, eViewOperation eViewOperation, bool isCopyDrag)
        {
            _CalendarItem = calendarItem;

            _OldStartTime = oldStartTime;
            _OldEndTime = oldEndTime;

            _eViewOperation = eViewOperation;
            _IsCopyDrag = isCopyDrag;
        }

        #region Public properties

        /// <summary>
        /// Gets the CalendarItem that was changed
        /// </summary>
        public CalendarItem CalendarItem
        {
            get { return (_CalendarItem); }
        }

        /// <summary>
        /// Gets th old, previous start time
        /// </summary>
        public DateTime OldStartTime
        {
            get { return (_OldStartTime); }
        }

        /// <summary>
        /// Gets the old, previous end time
        /// </summary>
        public DateTime OldEndTime
        {
            get { return (_OldEndTime); }
        }

        /// <summary>
        /// Gets the operation that was performed
        /// </summary>
        public eViewOperation eViewOperation
        {
            get { return (_eViewOperation); }
        }

        /// <summary>
        /// Gets whether the operation is being 
        /// performed on a drag created copy item
        /// </summary>
        public bool IsCopyDrag
        {
            get { return (_IsCopyDrag); }
        }

        #endregion
    }
    #endregion

    #region GetDisplayTemplateTextEventArgs
    /// <summary>
    /// GetDisplayTemplateTextEventArgs
    /// </summary>
    public class GetDisplayTemplateTextEventArgs : EventArgs
    {
        #region Private variables

        private CalendarItem _CalendarItem;

        private string _DisplayTemplate;
        private string _DisplayText;

        #endregion

        public GetDisplayTemplateTextEventArgs(CalendarItem calendarItem,
            string displayTemplate, string displayText)
        {
            _CalendarItem = calendarItem;

            _DisplayTemplate = displayTemplate;
            _DisplayText = displayText;
        }

        #region Public properties

        /// <summary>
        /// Gets the CalendarItem that was changed
        /// </summary>
        public CalendarItem CalendarItem
        {
            get { return (_CalendarItem); }
        }

        /// <summary>
        /// Gets the DisplayTemplate
        /// </summary>
        public string DisplayTemplate
        {
            get { return (_DisplayTemplate); }
        }

        /// <summary>
        /// Gets or sets the Display Text for the given DisplayTemplate
        /// </summary>
        public string DisplayText
        {
            get { return (_DisplayText); }
            set { _DisplayText = value; }
        }

        #endregion
    }
    #endregion

    #region DateChangeEventArgs
    /// <summary>
    /// DateChangeEventArgs
    /// </summary>
    public class DateChangeEventArgs : ValueChangedEventArgs<DateTime, DateTime>
    {
        public DateChangeEventArgs(DateTime oldValue, DateTime newValue)
            : base(oldValue, newValue)
        {
        }
    }
    #endregion

    #region ViewDateChangedEventArgs
    /// <summary>
    /// ViewDateChangedEventArgs
    /// </summary>
    public class ViewDateChangedEventArgs : EventArgs
    {
        #region Private variables

        private eCalendarView _eCalendarView;

        private DateTime _OldStartTime;
        private DateTime _NewStartTime;
        private DateTime _OldEndTime;
        private DateTime _NewEndTime;

        #endregion

        public ViewDateChangedEventArgs(eCalendarView eCalendarView,
            DateTime oldStartTime, DateTime newStartTime, DateTime oldEndTime, DateTime newEndTime)
        {
            _eCalendarView = eCalendarView;

            _OldStartTime = oldStartTime;
            _NewStartTime = newStartTime;
            _OldEndTime = oldEndTime;
            _NewEndTime = newEndTime;
        }

        #region Public properties

        /// <summary>
        /// Gets the eCalendarView
        /// </summary>
        public eCalendarView eCalendarView
        {
            get { return (_eCalendarView); }
        }

        /// <summary>
        /// Gets th old, previous start time
        /// </summary>
        public DateTime OldStartTime
        {
            get { return (_OldStartTime); }
        }

        /// <summary>
        /// Gets th new start time
        /// </summary>
        public DateTime NewStartTime
        {
            get { return (_NewStartTime); }
        }

        /// <summary>
        /// Gets the old, previous end time
        /// </summary>
        public DateTime OldEndTime
        {
            get { return (_OldEndTime); }
        }

        /// <summary>
        /// Gets the new end time
        /// </summary>
        public DateTime NewEndTime
        {
            get { return (_NewEndTime); }
        }

        #endregion
    }

    #endregion

    #region SelectedOwnerChangedEventArgs
    /// <summary>
    /// SelectedOwnerChangedEventArgs
    /// </summary>
    /// 
    public class SelectedOwnerChangedEventArgs : ValueChangedEventArgs<int, int>
    {
        public SelectedOwnerChangedEventArgs(int oldValue, int newValue)
            : base(oldValue, newValue)
        {
        }
    }
    #endregion

    #region FixedAllDayPanelHeightChangedEventArgs
    /// <summary>
    /// FixedAllDayPanelHeightChangedEventArgs
    /// </summary>
    /// 
    public class FixedAllDayPanelHeightChangedEventArgs : ValueChangedEventArgs<int, int>
    {
        public FixedAllDayPanelHeightChangedEventArgs(int oldValue, int newValue)
            : base(oldValue, newValue)
        {
        }
    }
    #endregion

    #region MaximumAllDayPanelHeightChangedEventArgs
    /// <summary>
    /// MaximumAllDayPanelHeightChangedEventArgs
    /// </summary>
    /// 
    public class MaximumAllDayPanelHeightChangedEventArgs : ValueChangedEventArgs<int, int>
    {
        public MaximumAllDayPanelHeightChangedEventArgs(int oldValue, int newValue)
            : base(oldValue, newValue)
        {
        }
    }

    #endregion

    #region TimeLineIntervalChangedEventArgs
    /// <summary>
    /// TimeLineIntervalChangedEventArgs
    /// </summary>
    public class TimeLineIntervalChangedEventArgs : ValueChangedEventArgs<int, int>
    {
        public TimeLineIntervalChangedEventArgs(int oldValue, int newValue)
            : base(oldValue, newValue)
        {
        }
    }
    #endregion

    #region TimeLineIntervalPeriodChangedEventArgs
    /// <summary>
    /// TimeLineIntervalPeriodChangedEventArgs
    /// </summary>
    public class TimeLineIntervalPeriodChangedEventArgs : ValueChangedEventArgs<eTimeLinePeriod, eTimeLinePeriod>
    {
        public TimeLineIntervalPeriodChangedEventArgs(eTimeLinePeriod oldValue, eTimeLinePeriod newValue)
            : base(oldValue, newValue)
        {
        }
    }
    #endregion

    #region EnableDragDropChangedEventArgs
    /// <summary>
    /// EnableDragDropChangedEventArgs
    /// </summary>
    public class EnableDragDropChangedEventArgs : ValueChangedEventArgs<bool, bool>
    {
        public EnableDragDropChangedEventArgs(bool oldValue, bool newValue)
            : base(oldValue, newValue)
        {
        }
    }
    #endregion

    #region TimeLineColumnWidthChangedEventArgs
    /// <summary>
    /// TimeLineColumnWidthChangedEventArgs
    /// </summary>
    public class TimeLineColumnWidthChangedEventArgs : ValueChangedEventArgs<int, int>
    {
        public TimeLineColumnWidthChangedEventArgs(int oldValue, int newValue)
            : base(oldValue, newValue)
        {
        }
    }
    #endregion

    #region TimeLineMaxColumnCountChangedEventArgs
    /// <summary>
    /// TimeLineMaxColumnCountChangedEventArgs
    /// </summary>
    public class TimeLineMaxColumnCountChangedEventArgs : ValueChangedEventArgs<int, int>
    {
        public TimeLineMaxColumnCountChangedEventArgs(int oldValue, int newValue)
            : base(oldValue, newValue)
        {
        }
    }
    #endregion

    #region TimeLineShowPeriodHeaderChangedEventArgs
    /// <summary>
    /// TimeLineShowPeriodHeaderChangedEventArgs
    /// </summary>
    public class TimeLineShowPeriodHeaderChangedEventArgs : ValueChangedEventArgs<bool, bool>
    {
        public TimeLineShowPeriodHeaderChangedEventArgs(bool oldValue, bool newValue)
            : base(oldValue, newValue)
        {
        }
    }
    #endregion

    #region TimeLineShowIntervalHeaderChangedEventArgs
    /// <summary>
    /// TimeLineShowIntervalHeaderChangedEventArgs
    /// </summary>
    public class TimeLineShowIntervalHeaderChangedEventArgs : ValueChangedEventArgs<bool, bool>
    {
        public TimeLineShowIntervalHeaderChangedEventArgs(bool oldValue, bool newValue)
            : base(oldValue, newValue)
        {
        }
    }
    #endregion

    #region TimeLineShowPageNavigationChangedEventArgs
    /// <summary>
    /// TimeLineShowPageNavigationChangedEventArgs
    /// </summary>
    public class TimeLineShowPageNavigationChangedEventArgs : ValueChangedEventArgs<bool, bool>
    {
        public TimeLineShowPageNavigationChangedEventArgs(bool oldValue, bool newValue)
            : base(oldValue, newValue)
        {
        }
    }
    #endregion

    #region TimeLineCondensedViewVisibilityChangedEventArgs
    /// <summary>
    /// TimeLineCondensedViewVisibilityChangedEventArgs
    /// </summary>
    public class TimeLineCondensedViewVisibilityChangedEventArgs :
        ValueChangedEventArgs<eCondensedViewVisibility, eCondensedViewVisibility>
    {
        public TimeLineCondensedViewVisibilityChangedEventArgs(
            eCondensedViewVisibility oldValue, eCondensedViewVisibility newValue)
            : base(oldValue, newValue)
        {
        }
    }
    #endregion

    #region TimeLineCondensedViewHeightChangedEventArgs
    /// <summary>
    /// TimeLineCondensedViewHeightChangedEventArgs
    /// </summary>
    public class TimeLineCondensedViewHeightChangedEventArgs : ValueChangedEventArgs<int, int>
    {
        public TimeLineCondensedViewHeightChangedEventArgs(int oldValue, int newValue)
            : base(oldValue, newValue)
        {
        }
    }
    #endregion

    #region TimeLineRenderPeriodHeaderEventArgs
    /// <summary>
    /// TimeLineRenderPeriodHeaderEventArgs
    /// </summary>
    public class TimeLineRenderPeriodHeaderEventArgs : CancelEventArgs
    {
        #region Private variables

        private Graphics _Graphics;
        private DateTime _StartTime;
        private DateTime _EndTime;
        private Rectangle _Bounds;
        private string _Text;

        #endregion

        public TimeLineRenderPeriodHeaderEventArgs(Graphics graphics,
            DateTime startTime, DateTime endTime, Rectangle bounds, string text)
        {
            _Graphics = graphics;

            _StartTime = startTime;
            _EndTime = endTime;

            _Bounds = bounds;
            _Text = text;
        }

        #region Public properties

        /// <summary>
        /// Gets the Graphics object used to render
        /// the Period Header
        /// </summary>
        public Graphics Graphics
        {
            get { return (_Graphics); }
        }

        /// <summary>
        /// Gets the Bounding Text Rectangle
        /// </summary>
        public Rectangle Bounds
        {
            get { return (_Bounds); }
        }

        /// <summary>
        /// Gets the visible display StartTime
        /// </summary>
        public DateTime StartTime
        {
            get { return (_StartTime); }
        }

        /// <summary>
        /// Gets the visible display EndTime
        /// </summary>
        public DateTime EndTime
        {
            get { return (_EndTime); }
        }

        /// <summary>
        /// Gets or sets the header Text
        /// </summary>
        public string Text
        {
            get { return (_Text); }
            set {_Text = value; }
        }

        #endregion
    }
    #endregion

    #region TimeLinePreRenderSlotBackgroundEventArgs
    /// <summary>
    /// TimeLinePreRenderSlotBackgroundEventArgs
    /// </summary>
    public class TimeLinePreRenderSlotBackgroundEventArgs : TimeLinePostRenderSlotBackgroundEventArgs
    {
        #region Private variables

        private bool _Cancel;

        #endregion

        public TimeLinePreRenderSlotBackgroundEventArgs(Graphics graphics, BaseView view,
            DateTime startTime, DateTime endTime, Rectangle bounds, eSlotDisplayState state)
            : base(graphics, view, startTime, endTime, bounds, state)
        {
        }

        #region Public properties

        /// <summary>
        /// Gets or Sets whether the event should be canceled
        /// </summary>
        public bool Cancel
        {
            get { return (_Cancel); }
            set { _Cancel = value; }
        }

        #endregion
    }
    #endregion

    #region TimeLinePostRenderSlotBackgroundEventArgs
    /// <summary>
    /// TimeLinePreRenderSlotBackgroundEventArgs
    /// </summary>
    public class TimeLinePostRenderSlotBackgroundEventArgs : EventArgs
    {
        #region Private variables

        private Graphics _Graphics;
        private DateTime _StartTime;
        private DateTime _EndTime;
        private Rectangle _Bounds;

        private eSlotDisplayState _State;
        private BaseView _View;

        #endregion

        public TimeLinePostRenderSlotBackgroundEventArgs(Graphics graphics, BaseView view,
            DateTime startTime, DateTime endTime, Rectangle bounds, eSlotDisplayState state)
        {
            _Graphics = graphics;

            _View = view;
            _StartTime = startTime;
            _EndTime = endTime;

            _Bounds = bounds;
            _State = state;
        }

        #region Public properties

        /// <summary>
        /// Gets the Graphics object used to render
        /// the slot
        /// </summary>
        public Graphics Graphics
        {
            get { return (_Graphics); }
        }

        /// <summary>
        /// Gets the slot bounding Rectangle
        /// </summary>
        public Rectangle Bounds
        {
            get { return (_Bounds); }
        }

        /// <summary>
        /// Gets the slot StartTime
        /// </summary>
        public DateTime StartTime
        {
            get { return (_StartTime); }
        }

        /// <summary>
        /// Gets the slot EndTime
        /// </summary>
        public DateTime EndTime
        {
            get { return (_EndTime); }
        }

        /// <summary>
        /// Gets the slot display state
        /// </summary>
        public eSlotDisplayState State
        {
            get { return (_State); }
            set { _State = value; }
        }

        /// <summary>
        /// Gets the associated View
        /// </summary>
        public BaseView View
        {
            get { return (_View); }
        }

        #endregion
    }
    #endregion

    #region TimeLineRenderSlotBorderEventArgs
    /// <summary>
    /// TimeLineRenderSlotBorderEventArgs
    /// </summary>
    public class TimeLineRenderSlotBorderEventArgs : CancelEventArgs
    {
        #region Private variables

        private Graphics _Graphics;
        private DateTime _StartTime;
        private bool _Hourly;
        private eSlotDisplayState _State;
        private Point _StartPoint;
        private Point _EndPoint;
        private Color _Color;

        #endregion

        public TimeLineRenderSlotBorderEventArgs(Graphics graphics,
            DateTime startTime, bool hourly, eSlotDisplayState state, Point pt1, Point pt2, Color color)
        {
            _Graphics = graphics;

            _StartTime = startTime;
            _Hourly = hourly;
            _State = state;
            _StartPoint = pt1;
            _EndPoint = pt2;
            _Color = color;
        }

        #region Public properties

        /// <summary>
        /// Gets the Graphics object used to render
        /// the slot
        /// </summary>
        public Graphics Graphics
        {
            get { return (_Graphics); }
        }

        /// <summary>
        /// Gets whether the border if an hourly border
        /// </summary>
        public bool Hourly
        {
            get { return (_Hourly); }
        }

        /// <summary>
        /// Gets the slot display state
        /// </summary>
        public eSlotDisplayState State
        {
            get { return (_State); }
            set { _State = value; }
        }

        /// <summary>
        /// Gets the slot Starting Time
        /// </summary>
        public DateTime StartTime
        {
            get { return (_StartTime); }
        }

        /// <summary>
        /// Gets the slot starting Point
        /// </summary>
        public Point StartPoint
        {
            get { return (_StartPoint); }
        }

        /// <summary>
        /// Gets the slot ending Point
        /// </summary>
        public Point EndPoint
        {
            get { return (_EndPoint); }
        }

        /// <summary>
        /// Gets the slot border Color
        /// </summary>
        public Color Color
        {
            get { return (_Color); }
        }

        #endregion
    }
    #endregion

    #region ShowOnlyWorkDayHoursChangedEventArgs
    /// <summary>
    /// ShowOnlyWorkDayHoursChangedEventArgs
    /// </summary>
    public class ShowOnlyWorkDayHoursChangedEventArgs : ValueChangedEventArgs<bool, bool>
    {
        public ShowOnlyWorkDayHoursChangedEventArgs(bool oldValue, bool newValue)
            : base(oldValue, newValue)
        {
        }
    }
    #endregion

    #region TimeLineGetRowHeightEventArgs
    /// <summary>
    /// TimeLineGetRowHeightEventArgs
    /// </summary>
    public class TimeLineGetRowHeightEventArgs : EventArgs
    {
        #region Private variables

        private CalendarItem _CalendarItem;
        private int _Height;

        #endregion

        public TimeLineGetRowHeightEventArgs(CalendarItem calendarItem, int height)
        {
            _CalendarItem = calendarItem;
            _Height = height;
        }

        #region Public properties

        /// <summary>
        /// Gets or sets the Height
        /// </summary>
        public int Height
        {
            get { return (_Height); }
            set { _Height = value; }
        }

        /// <summary>
        /// Gets the associated CalendarItem
        /// </summary>
        public CalendarItem CalendarItem
        {
            get { return (_CalendarItem); }
        }

        #endregion
    }

    #endregion

    #region TimeLineGetRowCollateIdEventArgs
    /// <summary>
    /// TimeLineGetRowCollateIdEventArgs
    /// </summary>
    public class TimeLineGetRowCollateIdEventArgs : EventArgs
    {
        #region Private variables

        private CalendarItem _CalendarItem;
        private int _CollateId;

        #endregion

        public TimeLineGetRowCollateIdEventArgs(CalendarItem calendarItem)
        {
            _CalendarItem = calendarItem;
        }

        #region Public properties

        /// <summary>
        /// Gets or sets the row CollateId
        /// </summary>
        public int CollateId
        {
            get { return (_CollateId); }

            set
            {
                if (value < 0)
                    throw new Exception("CollateId must be 0 or greater.");

                _CollateId = value;
            }
        }

        /// <summary>
        /// Gets the associated CalendarItem
        /// </summary>
        public CalendarItem CalendarItem
        {
            get { return (_CalendarItem); }
        }

        #endregion
    }

    #endregion

    #region PageNavigatorClickEventArgs
    /// <summary>
    /// PageNavigatorClickEventArgs
    /// </summary>
    public class PageNavigatorClickEventArgs : CancelEventArgs
    {
        #region Private variables

        private BaseView _View;
        private PageNavigator _PageNavigator;
        private PageNavigatorButton _Button;
        private DateTime _NavigateTime;

        #endregion

        public PageNavigatorClickEventArgs(BaseView view,
            PageNavigator pageNavigator, PageNavigatorButton button, DateTime navigateTime)
        {
            _View = view;
            _PageNavigator = pageNavigator;
            _Button = button;
            _NavigateTime = navigateTime;
        }

        #region Public properties

        /// <summary>
        /// Gets the PageNavigator
        /// </summary>
        public PageNavigator PageNavigator
        {
            get { return (_PageNavigator); }
        }

        /// <summary>
        /// Gets the associated CalendarView
        /// </summary>
        public BaseView View
        {
            get { return (_View); }
        }

        /// <summary>
        /// Gets which button was clicked
        /// </summary>
        public PageNavigatorButton Button
        {
            get { return (_Button); }
        }

        /// <summary>
        /// Gets or sets the time to navigate to 
        /// </summary>
        public DateTime NavigateTime
        {
            get { return (_NavigateTime); }
            set { _NavigateTime = value; }
        }

        #endregion
    }

    #endregion

    #region AllowDateSelectionChangedEventArgs
    /// <summary>
    /// AllowDateSelectionChangedEventArgs
    /// </summary>
    public class AllowDateSelectionChangedEventArgs : ValueChangedEventArgs<bool, bool>
    {
        public AllowDateSelectionChangedEventArgs(bool oldValue, bool newValue)
            : base(oldValue, newValue)
        {
        }
    }
    #endregion

    #region DayLinkChangedEventArgs
    /// <summary>
    /// DayLinkChangedEventArgs
    /// </summary>
    public class DayLinkChangedEventArgs : ValueChangedEventArgs<eYearViewDayLink, eYearViewDayLink>
    {
        public DayLinkChangedEventArgs(eYearViewDayLink oldValue, eYearViewDayLink newValue)
            : base(oldValue, newValue)
        {
        }
    }
    #endregion

    #region LinkViewChangedEventArgs
    /// <summary>
    /// LinkViewChangedEventArgs
    /// </summary>
    public class LinkViewChangedEventArgs : ValueChangedEventArgs<eCalendarView, eCalendarView>
    {
        public LinkViewChangedEventArgs(eCalendarView oldValue, eCalendarView newValue)
            : base(oldValue, newValue)
        {
        }
    }
    #endregion

    #region LinkViewActionChangedEventArgs
    /// <summary>
    /// LinkViewActionChangedEventArgs
    /// </summary>
    public class LinkViewActionChangedEventArgs : ValueChangedEventArgs<eYearViewLinkAction, eYearViewLinkAction>
    {
        public LinkViewActionChangedEventArgs(eYearViewLinkAction oldValue, eYearViewLinkAction newValue)
            : base(oldValue, newValue)
        {
        }
    }
    #endregion

    #region LinkViewStyleChangedEventArgs
    /// <summary>
    /// LinkViewStyleChangedEventArgs
    /// </summary>
    public class LinkViewStyleChangedEventArgs : ValueChangedEventArgs<eYearViewLinkStyle, eYearViewLinkStyle>
    {
        public LinkViewStyleChangedEventArgs(eYearViewLinkStyle oldValue, eYearViewLinkStyle newValue)
            : base(oldValue, newValue)
        {
        }
    }
    #endregion

    #region LinkViewSelectedEventArgs
    /// <summary>
    /// LinkViewSelectedEventArgs
    /// </summary>
    public class LinkViewSelectedEventArgs : CancelEventArgs
    {
        #region Private variables

        private DateTime _StartDate;
        private DateTime _EndDate;
        private eCalendarView _ECalendarView;

        #endregion

        public LinkViewSelectedEventArgs(
            DateTime startDate, DateTime endDate, eCalendarView calendarView)
        {
            _StartDate = startDate;
            _EndDate = endDate;
            _ECalendarView = calendarView;
        }

        #region Public properties

        /// <summary>
        /// Gets the selected StartDate
        /// </summary>
        public DateTime StartDate
        {
            get { return (_StartDate); }
            set { _StartDate = value; }
        }
        
        /// <summary>
        /// Gets the selected EndDate
        /// </summary>
        public DateTime EndDate
        {
            get { return (_EndDate); }
            set { _EndDate = value; }
        }

        /// <summary>
        /// Gets the eCalendarView to activate
        /// </summary>
        public eCalendarView ECalendarView
        {
            get { return (_ECalendarView); }
            set { _ECalendarView = value; }
        }
        #endregion
    }
    #endregion

    #region ShowGridLinesChangedEventArgs
    /// <summary>
    /// ShowGridLinesChangedEventArgs
    /// </summary>
    public class ShowGridLinesChangedEventArgs : ValueChangedEventArgs<bool, bool>
    {
        public ShowGridLinesChangedEventArgs(bool oldValue, bool newValue)
            : base(oldValue, newValue)
        {
        }
    }
    #endregion

    #region YearViewDrawDayBackgroundEventArgs
    /// <summary>
    /// YearViewDrawDayBackgroundEventArgs
    /// </summary>
    public class YearViewDrawDayBackgroundEventArgs : CancelEventArgs
    {
        #region Private variables

        private Graphics _Graphics;
        private YearMonth _YearMonth;
        private DateTime _Date;
        private Rectangle _Bounds;
        private eYearViewLinkStyle _LinkStyle;

        #endregion

        public YearViewDrawDayBackgroundEventArgs(Graphics g, YearMonth yearMonth,
            DateTime date, Rectangle bounds, eYearViewLinkStyle linkStyle)
        {
            _Graphics = g;
            _YearMonth = yearMonth;
            _Date = date;
            _Bounds = bounds;
            _LinkStyle = linkStyle;
        }

        #region Public properties

        /// <summary>
        /// Gets the Graphics object
        /// </summary>
        public Graphics Graphics
        {
            get { return (_Graphics); }
        }

        /// <summary>
        /// Gets the YearMonth
        /// </summary>
        public YearMonth YearMonth
        {
            get { return (_YearMonth); }
        }

        /// <summary>
        /// Gets the date to draw
        /// </summary>
        public DateTime Date
        {
            get { return (_Date); }
        }

        /// <summary>
        /// Gets the bounding rectangle
        /// </summary>
        public Rectangle Bounds
        {
            get { return (_Bounds); }
        }

        /// <summary>
        /// Gets or sets the Appointment Link style
        /// </summary>
        public eYearViewLinkStyle LinkStyle
        {
            get { return (_LinkStyle); }
            set { _LinkStyle = value; }
        }

        #endregion
    }
    #endregion

    #region YearViewDrawDayTextEventArgs
    /// <summary>
    /// YearViewDrawDayTextEventArgs
    /// </summary>
    public class YearViewDrawDayTextEventArgs : CancelEventArgs
    {
        #region Private variables

        private Graphics _Graphics;
        private YearMonth _YearMonth;
        private DateTime _Date;
        private Rectangle _Bounds;

        #endregion

        public YearViewDrawDayTextEventArgs(Graphics g,
            YearMonth yearMonth, DateTime date, Rectangle bounds)
        {
            _Graphics = g;
            _YearMonth = yearMonth;
            _Date = date;
            _Bounds = bounds;
        }

        #region Public properties

        /// <summary>
        /// Gets the Graphics object
        /// </summary>
        public Graphics Graphics
        {
            get { return (_Graphics); }
        }

        /// <summary>
        /// Gets the YearMonth
        /// </summary>
        public YearMonth YearMonth
        {
            get { return (_YearMonth); }
        }

        /// <summary>
        /// Gets the date to draw
        /// </summary>
        public DateTime Date
        {
            get { return (_Date); }
        }

        /// <summary>
        /// Gets the bounding rectangle
        /// </summary>
        public Rectangle Bounds
        {
            get { return (_Bounds); }
        }

        #endregion
    }
    #endregion

    #region RenderDaySlotAppearanceTextEventArgs
    /// <summary>
    /// RenderDaySlotAppearanceTextEventArgs
    /// </summary>
    public class RenderDaySlotAppearanceTextEventArgs : CancelEventArgs
    {
        #region Private variables

        private Graphics _Graphics;
        private Rectangle _Bounds;
        private DaySlotAppearance _Appearance;
        private DateTime _StartTime;
        private DateTime _EndTime;
        private string _Text;
        private bool _Selected;

        #endregion

        public RenderDaySlotAppearanceTextEventArgs(Graphics g, Rectangle bounds,
            DaySlotAppearance appearance, DateTime startTime, DateTime endTime, bool selected)
        {
            _Graphics = g;
            _Bounds = bounds;
            _Appearance = appearance;
            _StartTime = startTime;
            _EndTime = endTime;
            _Selected = selected;

            _Text = appearance.Text;
        }

        #region Public properties

        /// <summary>
        /// Gets the Graphics object
        /// </summary>
        public Graphics Graphics
        {
            get { return (_Graphics); }
        }

        /// <summary>
        /// Gets the DaySlotAppearance
        /// </summary>
        public DaySlotAppearance Appearance
        {
            get { return (_Appearance); }
        }

        /// <summary>
        /// Gets the start DateTime
        /// </summary>
        public DateTime StartTime
        {
            get { return (_StartTime); }
        }

        /// <summary>
        /// Gets the bounding rectangle
        /// </summary>
        public Rectangle Bounds
        {
            get { return (_Bounds); }
        }

        /// <summary>
        /// Gets the end DateTime
        /// </summary>
        public DateTime EndTime
        {
            get { return (_EndTime); }
        }

        /// <summary>
        /// Gets whether the area is selected
        /// </summary>
        public bool Selected
        {
            get { return (_Selected); }
        }

        /// <summary>
        /// Gets or Sets the Text to render
        /// </summary>
        public string Text
        {
            get { return (_Text); }
            set { _Text = value; }
        }

        #endregion
    }
    #endregion

    #region MonthViewPreRenderSlotBackgroundEventArgs
    /// <summary>
    /// MonthViewPreRenderSlotBackgroundEventArgs
    /// </summary>
    public class MonthViewPreRenderSlotBackgroundEventArgs : TimeLinePreRenderSlotBackgroundEventArgs
    {
        public MonthViewPreRenderSlotBackgroundEventArgs(Graphics graphics, BaseView view,
            DateTime startTime, DateTime endTime, Rectangle bounds, eSlotDisplayState state)
            : base(graphics, view, startTime, endTime, bounds, state)
        {
        }
    }
    #endregion

    #region MonthViewPostRenderSlotBackgroundEventArgs
    /// <summary>
    /// MonthViewPreRenderSlotBackgroundEventArgs
    /// </summary>
    public class MonthViewPostRenderSlotBackgroundEventArgs : TimeLinePostRenderSlotBackgroundEventArgs
    {
        public MonthViewPostRenderSlotBackgroundEventArgs(Graphics graphics, BaseView view,
            DateTime startTime, DateTime endTime, Rectangle bounds, eSlotDisplayState state)
            : base(graphics, view, startTime, endTime, bounds, state)
        {
        }
    }
    #endregion

    #region RenderTabBackgroundEventArgs
    /// <summary>
    /// RenderTabBackgroundEventArgs
    /// </summary>
    public class RenderTabBackgroundEventArgs : CancelEventArgs
    {
        #region Private variables

        private Graphics _Graphics;
        private GraphicsPath _Path;
        private BaseView _View;
        private bool _IsSelected;

        #endregion

        public RenderTabBackgroundEventArgs(
            Graphics graphics, GraphicsPath path, BaseView view, bool isSelected)
        {
            _Graphics = graphics;
            _Path = path;
            _View = view;
            _IsSelected = isSelected;
        }

        #region Public properties

        /// <summary>
        /// Gets the Graphics object
        /// </summary>
        public Graphics Graphics
        {
            get { return (_Graphics); }
        }

        /// <summary>
        /// Gets the tab GraphicsPath
        /// </summary>
        public GraphicsPath Path
        {
            get { return (_Path); }
        }

        /// <summary>
        /// Gets the tab BaseView
        /// </summary>
        public BaseView View
        {
            get { return (_View); }
        }

        /// <summary>
        /// Gets whether the tab is selected or not
        /// </summary>
        public bool IsSelected
        {
            get { return (_IsSelected); }
        }

        #endregion
    }
    #endregion

    #region RenderTabBackgroundEventArgs
    /// <summary>
    /// RenderTabBackgroundEventArgs
    /// </summary>
    public class RenderTabContentEventArgs : RenderTabBackgroundEventArgs
    {
        #region Private variables

        private string _Text;

        #endregion

        public RenderTabContentEventArgs(Graphics graphics,
            GraphicsPath path, BaseView view, bool isSelected, string text)
            : base (graphics, path, view, isSelected)
        {
            _Text = text;
        }

        #region Public properties

        /// <summary>
        /// Gets or sets the tab text
        /// </summary>
        public string Text
        {
            get { return (_Text); }
            set { _Text = value; }
        }

        #endregion
    }
    #endregion

    #region ViewLoadCompleteEventArgs
    /// <summary>
    /// ViewLoadCompleteEventArgs
    /// </summary>
    public class ViewLoadCompleteEventArgs : EventArgs
    {
        #region Private variables

        private BaseView _View;

        #endregion

        public ViewLoadCompleteEventArgs(BaseView view)
        {
            _View = view;
        }

        #region Public properties

        /// <summary>
        /// Gets the tab BaseView
        /// </summary>
        public BaseView View
        {
            get { return (_View); }
        }

        #endregion
    }
    #endregion
}
#endif
