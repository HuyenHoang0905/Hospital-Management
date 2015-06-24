#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Drawing;
using DevComponents.Schedule.Model;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Collections.ObjectModel;

namespace DevComponents.DotNetBar.Schedule
{
    public class BaseView : BaseItem
    {
        #region Events

        /// <summary>
        /// Occurs when SelectedItem has Changed
        /// </summary>
        public event EventHandler<SelectedItemChangedEventArgs> SelectedItemChanged;

        /// <summary>
        /// Occurs when CalendarColor has Changed
        /// </summary>
        public event EventHandler<CalendarColorChangedEventArgs> CalendarColorChanged;

        #endregion

        #region Constants

        protected const int DaysInWeek = 7;
        private const int TabRadius = 3;
        private const int TabDia = TabRadius * 2;

        #endregion

        #region Static variables

        private static DateTime _oldStartTime;          // Pre-move/resize CalendarItem StartTime
        private static DateTime _oldEndTime;            // Pre-move/resize CalendarItem EndTime
        private static string _oldOwnerKey;             // Pre-move CalendarItem OwnerKey

        #endregion

        #region SubClasses

        internal class NonClientData
        {
            #region Data members

            public eTabOrientation TabOrientation; // Tab orientation

            public int TabBorder;                  // Tab border color
            public int TabFore;                    // Tab foreground color
            public int TabBack;                    // Tab background color
            public int ContBack;                   // Tab content background
            public int TabSelFore;                 // Tab selected foreground
            public int TabSelBack;                 // Tab selected background

            #endregion

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="tabOrientation">Tab Orientation</param>
            /// <param name="tabBorder">Tab border color</param>
            /// <param name="tabFore">Tab foreground color</param>
            /// <param name="tabBack">Tab background color</param>
            /// <param name="contBack">Tab content background</param>
            /// <param name="tabSelFore">Tab selected foreground</param>
            /// <param name="tabSelBack">Tab selected background</param>
            public NonClientData(
                eTabOrientation tabOrientation,
                int tabBorder, int tabFore, int tabBack,
                int contBack, int tabSelFore, int tabSelBack)
            {
                TabOrientation = tabOrientation;

                TabBorder = tabBorder;
                TabFore = tabFore;
                TabBack = tabBack;
                ContBack = contBack;
                TabSelFore = tabSelFore;
                TabSelBack = tabSelBack;
            }
        }

        #endregion

        #region Private variables

        private CalendarView _CalendarView;             // CalendarView
        private eCalendarView _ECalendarView;           // Enum CalendarView

        private ModelViewConnector _Connector;          // Connector

        private DateTime? _DateSelectionStart;          // Date selection start
        private DateTime? _DateSelectionEnd;            // Date selection end
        private DateTime? _DateSelectionAnchor;         // Date selection anchor

        private DateTime _StartDate;                    // Display start date
        private DateTime _EndDate;                      // Display end date
        private bool _DateRangeChanged;                 // Date range has changed

        private CalendarColor _CalendarColor;           // CalendarColor

        private Rectangle _ClientRect;                  // View client area
        private NonClientData _NClientData;             // Non-client data
        private int _TabWidth;                          // Tab width

        private PosWin _PosWin;                         // Move/Resize Position window

        private bool _IsMouseDown;                      // Mouse is down
        private bool _IsMoving;                         // Flag denoting app moving
        private bool _IsStartResizing;                  // Flag denoting appt start resizing
        private bool _IsEndResizing;                    // Flag denoting appt end resizing
        private bool _IsTabMoving;                      // Flag denoting Tab is moving
        private bool _IsCondMoving;                     // Flag denoting Condensed view movement
        private bool _IsCopyDrag;                       // Flag denoting Appointment copy is dragging

        private int _LastViewIndex;                     // Last MultiUser view index

        private string _OwnerKey;                       // Owner key
        private string _DisplayName;                    // Display name
        private int _DisplayedOwnerKeyIndex = -1;       // DisplayedOwnerKey index

        private bool _IsViewSelected;                   // View selected?
        private bool _IsSelecting;                      // Selection inprogress flag
        private CalendarItem _SelectedItem;             // Selected CalendarItem

        private bool _NeedRecalcLayout = true;          // Layout needs recalculated

        private DaysOfTheWeek _DaysOfTheWeek;           // Days of the week

        private Font _Font = SystemFonts.CaptionFont;   // Display font

        private Font _BoldFont =                        // Bold display font
            new Font(SystemFonts.CaptionFont, FontStyle.Bold);

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="calendarView"></param>
        /// <param name="ecv"></param>
        public BaseView(CalendarView calendarView, eCalendarView ecv)
        {
            // Save the provided CalendarView and
            // tell the system we are a container

            _CalendarView = calendarView;
            _ECalendarView = ecv;

            SetIsContainer(true);

            SubItems.AllowParentRemove = false;

            // Make sure we see all our mouse events

            MouseDownCapture = true;
            MouseUpNotification = true;

            // Hook our events

            HookEvents(true);
        }

        #region Public properties

        #region BoldFont

        /// <summary>
        /// Gets the Bold display font
        /// </summary>
        public Font BoldFont
        {
            get { return (_BoldFont); }
        }

        #endregion

        #region Bounds

        /// <summary>
        /// Gets and sets the view bounding rectangle
        /// </summary>
        public override Rectangle Bounds
        {
            get { return (base.Bounds); }

            set
            {
                if (base.Bounds != value)
                {
                    Rectangle r = base.Bounds;
                    base.Bounds = value;

                    InvalidateRect(r);

                    NeedRecalcLayout = true;
                }
            }
        }

        #endregion

        #region DisplayedOwnerKey

        /// <summary>
        /// Gets and sets the DisplayedOwnerKey
        /// </summary>
        public string DisplayedOwnerKey
        {
            get
            {
                if (_DisplayedOwnerKeyIndex < 0 ||
                    _DisplayedOwnerKeyIndex > _CalendarView.DisplayedOwners.Count)
                {
                    return ("");
                }

                return (_CalendarView.DisplayedOwners[_DisplayedOwnerKeyIndex]);
            }
        }
        
        /// <summary>
        /// Gets and sets the DisplayedOwnerKeyIndex
        /// </summary>
        public int DisplayedOwnerKeyIndex
        {
            get { return (_DisplayedOwnerKeyIndex); }
            internal set { _DisplayedOwnerKeyIndex = value; }
        }

        /// <summary>
        /// Gets and sets the view Owner Key
        /// </summary>
        public string OwnerKey
        {
            get { return (_OwnerKey); }
            internal set { _OwnerKey = value; }
        }

        /// <summary>
        /// Gets and sets the view Display Name
        /// </summary>
        public string DisplayName
        {
            get { return (_DisplayName); }

            set
            {
                if (_DisplayName != value)
                {
                    _DisplayName = value;

                    this.Refresh();
                }
            }
        }

        #endregion

        #region Start/EndDate

        /// <summary>
        /// Gets and sets the display start date
        /// </summary>
        public DateTime StartDate
        {
            get { return (_StartDate); }

            set
            {
                if (_StartDate != value)
                {
                    _StartDate = value;
                    _DateRangeChanged = true;

                    NeedRecalcLayout = true;
                }
            }
        }

        /// <summary>
        /// Gets and sets the display end date
        /// </summary>
        public DateTime EndDate
        {
            get { return (_EndDate); }

            set
            {
                if (_EndDate != value)
                {
                    _EndDate = value;
                    _DateRangeChanged = true;

                    NeedRecalcLayout = true;
                }
            }
        }

        #endregion

        #region DateSelectionStart

        /// <summary>
        /// Gets and sets the date selection start
        /// </summary>
        public DateTime? DateSelectionStart
        {
            get { return (_DateSelectionStart); }

            set
            {
                if (_DateSelectionStart != value)
                {
                    _DateSelectionStart = value;

                    // Update our dayRects selection status

                    UpdateDateSelection();
                }
            }
        }

        #endregion

        #region DateSelectionEnd

        /// <summary>
        /// Gets or sets the end date of selection.
        /// </summary>
        public DateTime? DateSelectionEnd
        {
            get { return (_DateSelectionEnd); }

            set
            {
                if (_DateSelectionEnd != value)
                {
                    _DateSelectionEnd = value;

                    // Update our dayRects selection status

                    UpdateDateSelection();
                }
            }
        }

        #endregion

        #region CalendarColor

        /// <summary>
        /// Gets and sets the display calendar color scheme
        /// </summary>
        public eCalendarColor CalendarColor
        {
            get { return (_CalendarColor.ColorSch); }

            set
            {
                if (_CalendarColor.ColorSch != value)
                {
                    eCalendarColor oldValue = _CalendarColor.ColorSch;
                    _CalendarColor.ColorSch = value;

                    OnCalendarColorChanged(oldValue, value);

                    if (this.IsViewSelected == true)
                        InvalidateRect(Bounds);
                }
            }
        }

        /// <summary>
        /// OnCalendarColorChanged event propagation
        /// </summary>
        protected virtual void
            OnCalendarColorChanged(eCalendarColor oldValue, eCalendarColor newValue)
        {
            if (CalendarColorChanged != null)
            {
                CalendarColorChanged(this,
                    new CalendarColorChangedEventArgs(oldValue, newValue));
            }

            // Reflect the change in alternate views

            CalendarView.UpdateAltViewCalendarColor(newValue, DisplayedOwnerKeyIndex);
        }

        #endregion

        #region Font

        /// <summary>
        /// Get and sets the calendar font.
        /// </summary>
        public virtual Font Font
        {
            get { return (_Font); }

            set
            {
                if (value != null && _Font.Equals(value) == false)
                {
                    if (_Font != null)
                        _Font.Dispose();

                    if (_BoldFont != null)
                        _BoldFont.Dispose();

                    _Font = value;
                    _BoldFont = new Font(_Font, FontStyle.Bold);

                    InvalidateRect(true);
                }
            }
        }

        #endregion

        #region Selection support

        /// <summary>
        /// Gets and sets the currently selected CalendarItem
        /// </summary>
        public CalendarItem SelectedItem
        {
            get { return _SelectedItem; }

            set
            {
                if (_SelectedItem != value)
                {
                    CalendarItem oldValue = _SelectedItem;
                    _SelectedItem = value;

                    if (value != null)
                    {
                        CalendarView.DateSelectionStart = null;
                        CalendarView.DateSelectionEnd = null;
                        
                        DateSelectionAnchor = null;

                        UpdateItemOrder(value);
                    }

                    SetSelectedItem(oldValue, value);

                    OnSelectedItemChanged(oldValue, value);
                }
            }
        }

        internal void UpdateItemOrder(CalendarItem ci)
        {
            int n = SubItems.IndexOf(ci);
            int index = FirstCalendarItem();

            if (index >= 0 && index < n)
            {
                SubItems._RemoveAt(n);
                SubItems._Insert(index, ci);
            }
        }

        private int FirstCalendarItem()
        {
            for (int i = 0; i < SubItems.Count; i++)
            {
                if (SubItems[i] is CalendarItem)
                    return (i);
            }

            return (-1);
        }

        /// <summary>
        /// SelectedItemChanged event propagation
        /// </summary>
        protected virtual void
            OnSelectedItemChanged(CalendarItem oldValue, CalendarItem newValue)
        {
            if (SelectedItemChanged != null)
            {
                SelectedItemChanged(this,
                    new SelectedItemChangedEventArgs(oldValue, newValue));
            }
        }

        /// <summary>
        /// Gets the selected state of the view
        /// </summary>
        public bool IsViewSelected
        {
            get { return (_IsViewSelected); }
            internal set {_IsViewSelected = value; }
        }

        #endregion

        #region SelectedAppointments

        /// <summary>
        /// Gets a ReadOnlyCollection of the
        /// currently selected appointments for the view
        /// </summary>
        public ReadOnlyCollection<AppointmentView> SelectedAppointments
        {
            get
            {
                List<AppointmentView> apps = new List<AppointmentView>();

                AppointmentView view = _SelectedItem as AppointmentView;

                if (view != null)
                    apps.Add(view);

                return (new ReadOnlyCollection<AppointmentView>(apps));
            }
        }

        #endregion

        #endregion

        #region Protected properties

        #region CalendarModel

        /// <summary>
        /// Gets the View CalendarModel
        /// </summary>
        protected CalendarModel CalendarModel
        {
            get { return (CalendarView.CalendarModel); }
        }

        #endregion

        #region Mouse related properties

        /// <summary>
        /// IsMouseDown
        /// </summary>
        protected bool IsMouseDown
        {
            get { return (_IsMouseDown); }
            set { _IsMouseDown = value; }
        }

        /// <summary>
        /// IsStartResizing
        /// </summary>
        protected bool IsStartResizing
        {
            get { return (_IsStartResizing); }
            set { _IsStartResizing = value; }
        }

        /// <summary>
        /// IsEndResizing
        /// </summary>
        protected bool IsEndResizing
        {
            get { return (_IsEndResizing); }
            set { _IsEndResizing = value; }
        }

        /// <summary>
        /// IsResizing
        /// </summary>
        protected bool IsResizing
        {
            get { return (_IsStartResizing || _IsEndResizing); }
        }

        /// <summary>
        /// IsMoving
        /// </summary>
        protected bool IsMoving
        {
            get { return (_IsMoving); }
            set { _IsMoving = value; }
        }

        /// <summary>
        /// IsTabMoving
        /// </summary>
        protected bool IsTabMoving
        {
            get { return (_IsTabMoving); }
            set { _IsTabMoving = value; }
        }

        /// <summary>
        /// IsConMoving
        /// </summary>
        protected bool IsCondMoving
        {
            get { return (_IsCondMoving); }
            set { _IsCondMoving = value; }
        }

        /// <summary>
        /// CanDrag
        /// </summary>
        protected bool CanDrag
        {
            get { return (SelectedItem != null && CalendarView.EnableDragDrop == true); }
        }

        /// <summary>
        /// Pre-move/resize StartTime
        /// </summary>
        protected DateTime OldStartTime
        {
            get { return (_oldStartTime); }
            set { _oldStartTime = value; }
        }

        /// <summary>
        /// Pre-move/resize EndTime
        /// </summary>
        protected DateTime OldEndTime
        {
            get { return (_oldEndTime); }
            set { _oldEndTime = value; }
        }

        #endregion

        #region Cursor properties

        /// <summary>
        /// Sets local view cursor
        /// </summary>
        protected Cursor MyCursor
        {
            set { CalendarView.ViewCursor = value; }
        }

        /// <summary>
        /// Gets CalendarView default cursor
        /// </summary>
        protected Cursor DefaultCursor
        {
            get { return (CalendarView.DefaultViewCursor); }
        }

        #endregion

        #region PosWin

        /// <summary>
        /// Gets and sets the view position window
        /// </summary>
        protected PosWin PosWin
        {
            get { return (_PosWin); }
            set { _PosWin = value; }
        }

        #endregion

        #endregion

        #region Internal properties

        /// <summary>
        /// Gets the CalendarView
        /// </summary>
        internal CalendarView CalendarView
        {
            get { return (_CalendarView); }
        }

        /// <summary>
        /// Gets the ECalendarView
        /// </summary>
        internal eCalendarView ECalendarView
        {
            get { return (_ECalendarView); }
        }

        /// <summary>
        /// Gets and sets the ModelViewConnector
        /// </summary>
        internal ModelViewConnector Connector
        {
            get { return (_Connector); }

            set
            {
                if (value != _Connector)
                {
                    _Connector = value;

                    _Connector.DisplayOwnerKey = DisplayedOwnerKey;
                    _Connector.Connect();
                }
            }
        }

        /// <summary>
        /// Gets the DayOfWeekHeader height
        /// </summary>
        internal int DayOfWeekHeaderHeight
        {
            get { return (Font.Height + 4); }
        }

        /// <summary>
        /// IsCopyDrag
        /// </summary>
        internal bool IsCopyDrag
        {
            get { return (_IsCopyDrag); }
            set { _IsCopyDrag = value; }
        }

        /// <summary>
        /// OldOwnerKey
        /// </summary>
        internal string OldOwnerKey
        {
            get { return (_oldOwnerKey); }
            set { _oldOwnerKey = value; }
        }

        #region MonthHeaderHeight

        internal int MonthHeaderHeight
        {
            get { return (Font.Height + 4); }
        }

        #endregion

        /// <summary>
        /// Gets the MultiUserTabHeight
        /// </summary>
        internal int MultiUserTabHeight
        {
            get { return (_CalendarView.MultiUserTabHeight); }
        }

        /// <summary>
        /// Gets the MultiUserTabWidth
        /// </summary>
        internal int MultiUserTabWidth
        {
            get { return (_CalendarView.TimeLineMultiUserTabWidth); }
        }

        /// <summary>
        /// Gets the Appointment height
        /// </summary>
        internal int AppointmentHeight
        {
            get { return (Font.Height + 4); }
        }

        /// <summary>
        /// Gets and sets the date selection anchor
        /// </summary>
        internal DateTime? DateSelectionAnchor
        {
            get { return (_DateSelectionAnchor); }
            set { _DateSelectionAnchor = value; }
        }

        /// <summary>
        /// Gets and sets the Days of the week object
        /// </summary>
        internal DaysOfTheWeek DaysOfTheWeek
        {
            get { return (_DaysOfTheWeek); }
            set { _DaysOfTheWeek = value; }
        }

        /// <summary>
        /// Gets and sets the selecting status
        /// </summary>
        internal bool IsSelecting
        {
            get { return (_IsSelecting); }
            set { _IsSelecting = value; }
        }

        /// <summary>
        /// Gets and sets recalc layout need
        /// </summary>
        internal bool NeedRecalcLayout
        {
            get { return (_NeedRecalcLayout); }

            set
            {
                _NeedRecalcLayout = value;

                if (value == true)
                    InvalidateRect(true);
            }
        }

        /// <summary>
        /// Gets and sets the view client rectangle
        /// </summary>
        internal Rectangle ClientRect
        {
            get { return (_ClientRect); }
            set { _ClientRect = value; }
        }

        /// <summary>
        /// Gets and sets the CalendarColorTable
        /// </summary>
        internal CalendarColor CalendarColorTable
        {
            get { return (_CalendarColor); }
            set { _CalendarColor = value; }
        }

        /// <summary>
        /// Gets and sets the DateRangeChanged state
        /// </summary>
        internal bool DateRangeChanged
        {
            get { return (_DateRangeChanged); }
            set { _DateRangeChanged = value; }
        }

        /// <summary>
        /// Gets and sets the base non-client data
        /// </summary>
        internal NonClientData NClientData
        {
            get { return (_NClientData); }
            set { _NClientData = value; }
        }

        #endregion

        #region HookEvents

        /// <summary>
        /// Hooks and unhooks our object events
        /// </summary>
        /// <param name="hook">True - hook, false - unhook</param>
        private void HookEvents(bool hook)
        {
            if (hook == true)
            {
                CalendarView.ModelChanged += CalendarViewModelChanged;
                CalendarView.DateSelectionStartChanged += CalendarViewDateSelectionStartChanged;
                CalendarView.DateSelectionEndChanged += CalendarViewDateSelectionEndChanged;
            }
            else
            {
                CalendarView.ModelChanged -= CalendarViewModelChanged;
                CalendarView.DateSelectionStartChanged -= CalendarViewDateSelectionStartChanged;
                CalendarView.DateSelectionEndChanged -= CalendarViewDateSelectionEndChanged;
            }
        }

        #endregion

        #region Event processing

        /// <summary>
        /// DateSelectionEndChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CalendarViewDateSelectionEndChanged(object sender, DateSelectionEventArgs e)
        {
            if (CalendarView.SelectedOwnerIndex == DisplayedOwnerKeyIndex)
                this.DateSelectionEnd = e.NewValue;
            else
                this.DateSelectionEnd = null;
        }

        /// <summary>
        /// DateSelectionStartChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CalendarViewDateSelectionStartChanged(object sender, DateSelectionEventArgs e)
        {
            if (CalendarView.SelectedOwnerIndex == DisplayedOwnerKeyIndex)
                this.DateSelectionStart = e.NewValue;
            else
                this.DateSelectionStart = null;
        }

        /// <summary>
        /// ModelChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CalendarViewModelChanged(object sender, ModelEventArgs e)
        {
            ResetView();
        }

        #endregion

        #region RecalcSize

        /// <summary>
        /// Performs NeedRecalcSize requests
        /// </summary>
        public override void RecalcSize()
        {
            base.RecalcSize();

            Rectangle r = Bounds;

            if (CalendarView.IsMultiCalendar == true)
            {
                if (NClientData.TabOrientation == eTabOrientation.Horizontal)
                {
                    r.Inflate(-2, -2);

                    if (CalendarView.ShowTabs == true)
                    {
                        r.Y += MultiUserTabHeight;
                        r.Height -= MultiUserTabHeight;
                    }
                }
                else
                {
                    if (CalendarView.ShowTabs == true)
                    {
                        if (CalendarView.TimeLineMultiUserTabOrientation == eOrientation.Vertical)
                        {
                            r.X += MultiUserTabHeight + 4;
                            r.Width -= (MultiUserTabHeight + 4);
                        }
                        else
                        {
                            r.X += MultiUserTabWidth + 4;
                            r.Width -= (MultiUserTabWidth + 4);
                        }
                    }
                }
            }

            ClientRect = r;
        }

        #endregion

        #region Paint processing

        /// <summary>
        /// Paint processing
        /// </summary>
        /// <param name="e"></param>
        public override void Paint(ItemPaintArgs e)
        {
            if (CalendarView.IsMultiCalendar == true)
            {
                if (NClientData.TabOrientation == eTabOrientation.Horizontal)
                    DrawHorizontalLayout(e);
                else
                    DrawVerticalLayout(e);
            }
        }

        #region DrawHorizontalLayout

        /// <summary>
        /// Draws horizontal tab layout
        /// </summary>
        /// <param name="e"></param>
        private void DrawHorizontalLayout(ItemPaintArgs e)
        {
            Graphics g = e.Graphics;

            Rectangle r = Bounds;
            r.Width = _ClientRect.Width;
            r.Height = MultiUserTabHeight + 3;

            if (r.Width > 0)
            {
                // Size-up the tab dimensions

                string s = string.IsNullOrEmpty(DisplayName) ? OwnerKey : DisplayName;
                Size sz = TextDrawing.MeasureString(g, s, Font);

                // Draw the control background border, and tab,

                DrawBackBorder(g, sz.Width);
                DrawHTab(e, s, sz.Width, r);
            }
        }

        #region DrawBackBorder

        /// <summary>
        /// Draws the background border around
        /// the entire control view
        /// </summary>
        /// <param name="g"></param>
        /// <param name="width"></param>
        private void DrawBackBorder(Graphics g, int width)
        {
            Rectangle r = _ClientRect;

            if (g.ClipBounds.X > r.X)
            {
                r.Width -= (int)(g.ClipBounds.X - r.X);
                r.X = (int)g.ClipBounds.X + 1;
            }

            if (g.ClipBounds.Right < r.Right)
                r.Width = (int)g.ClipBounds.Right - r.X - 1;

            using (Pen pen = new
                Pen(_CalendarColor.GetColor(_NClientData.ContBack)))
            {
                g.DrawLines(pen, new Point[] {
                    new Point(r.Left - 1, r.Y - 1),
                    new Point(r.Right, r.Top - 1),
                    new Point(r.Right, r.Bottom),
                    new Point(r.Left - 1, r.Bottom),
                    new Point(r.Left - 1, r.Top - 2),
                    new Point(r.Right, r.Top - 2), });
            }

            using (Pen pen = new
                Pen(_CalendarColor.GetColor(_NClientData.TabBorder)))
            {
                // Draw the tab border

                if (width + MultiUserTabHeight < _ClientRect.Width)
                {
                    g.DrawLine(pen,
                       _ClientRect.Left + width + MultiUserTabHeight, _ClientRect.Top - 2,
                       _ClientRect.Right + 1, _ClientRect.Top - 2);
                }

                g.DrawLines(pen, new Point[]
                {
                    new Point(_ClientRect.Right + 1, _ClientRect.Top - 2),
                    new Point(_ClientRect.Right + 1, _ClientRect.Bottom + 1),
                    new Point(_ClientRect.Left - 2, _ClientRect.Bottom + 1),
                    new Point(_ClientRect.Left - 2, _ClientRect.Top - 2),
                });
            }
        }

        #endregion

        #region DrawHTab

        /// <summary>
        /// Draws the tab - border and content
        /// </summary>
        /// <param name="e">ItemPaintArgs</param>
        /// <param name="text">Tab text</param>
        /// <param name="width">Text measured width</param>
        /// <param name="r">Bounding rectangle</param>
        private void DrawHTab(ItemPaintArgs e, string text, int width, Rectangle r)
        {
            _TabWidth = Math.Min(width, _ClientRect.Width);

            if (CalendarView.ShowTabs == true)
            {
                if (e.ClipRectangle.Y < _ClientRect.Y)
                {
                    Graphics g = e.Graphics;

                    bool isSelected = 
                        (CalendarView.SelectedOwnerIndex == DisplayedOwnerKeyIndex);

                    // Draw the tab

                    using (GraphicsPath path = GetHTabPath(_TabWidth))
                    {
                        if (CalendarView.DoRenderTabBackground(g, path, this, isSelected) == false)
                        {
                            using (Brush br = _CalendarColor.BrushPart(
                                isSelected ? _NClientData.TabSelBack : _NClientData.TabBack, r))
                            {
                                g.FillPath(br, path);
                            }

                            using (Pen pen = new
                                Pen(_CalendarColor.GetColor(_NClientData.TabBorder)))
                            {
                                g.DrawPath(pen, path);
                            }
                        }

                        // Draw the tab text

                        if (CalendarView.DoRenderTabContent(
                            g, path, this, ref text, isSelected) == false)
                        {
                            r.X += 4;

                            Color color = _CalendarColor.GetColor(isSelected
                                ? _NClientData.TabSelFore : _NClientData.TabFore);

                            TextDrawing.DrawString(g, text, Font, color, r, eTextFormat.VerticalCenter);
                        }
                    }
                }
            }
        }

        #endregion

        #region GetHTabPath

        /// <summary>
        /// Gets the tab graphics path
        /// </summary>
        /// <param name="tabWidth">Tab width</param>
        /// <returns>GraphicsPath</returns>
        protected GraphicsPath GetHTabPath(int tabWidth)
        {
            GraphicsPath path = new GraphicsPath();

            Rectangle r = ClientRect;

            r.Width = tabWidth;
            r.Height = MultiUserTabHeight;

            path.AddLine(ClientRect.Left - 2, ClientRect.Top - 2,
                ClientRect.Left - 2, Bounds.Top + TabRadius);

            path.AddArc(ClientRect.Left - 2, Bounds.Y, TabDia, TabDia, 180, 90);

            if (tabWidth + MultiUserTabHeight < ClientRect.Width)
            {
                // Full tab

                 path.AddLines(new Point[] {
                    new Point(ClientRect.Left - 2 + TabRadius, Bounds.Top),
                    new Point(ClientRect.Left + tabWidth, Bounds.Top),
                    new Point(ClientRect.Left + tabWidth + MultiUserTabHeight, ClientRect.Top - 2) });
            }
            else if (tabWidth < ClientRect.Width)                   
            {
                // Partial tab with end slant

                int n = ClientRect.Width - tabWidth + 2;

                path.AddLines(new Point[] {
                    new Point(ClientRect.Left - 2 + TabRadius, Bounds.Top),
                    new Point(ClientRect.Left + tabWidth, Bounds.Top),
                    new Point(ClientRect.Left - 2 + tabWidth + n, Bounds.Top + n),
                    new Point(ClientRect.Left - 2 + tabWidth + n, ClientRect.Top - 2) });
            }
            else
            {
                // partial tab with no end slant

                path.AddLines(new Point[] {
                    new Point(ClientRect.Left - 2 + TabRadius, Bounds.Top),
                    new Point(ClientRect.Right + 1, Bounds.Top),
                    new Point(ClientRect.Right + 1, ClientRect.Top - 2) });
            }

            return (path);
        }

        #endregion

        #endregion

        #region DrawVerticalLayout

        /// <summary>
        /// Draws vertical tab layout
        /// </summary>
        /// <param name="e"></param>
        private void DrawVerticalLayout(ItemPaintArgs e)
        {
            if (CalendarView.ShowTabs == true)
            {
                Rectangle r = ClientRect;

                if (r.Height > 0)
                {
                    if (CalendarView.TimeLineMultiUserTabOrientation == eOrientation.Vertical)
                    {
                        r.X -= MultiUserTabHeight;
                        r.Width = MultiUserTabHeight;

                        DrawVTab(e, string.IsNullOrEmpty(DisplayName)
                                        ? OwnerKey
                                        : DisplayName, r);
                    }
                    else
                    {
                        r.X -= MultiUserTabWidth;
                        r.Width = MultiUserTabWidth;

                        DrawVTab2(e, string.IsNullOrEmpty(DisplayName)
                                         ? OwnerKey
                                         : DisplayName, r);
                    }
                }
            }
        }

        #region DrawVTab

        /// <summary>
        /// Draws the tab - border and content
        /// </summary>
        /// <param name="e">ItemPaintArgs</param>
        /// <param name="text">Tab text</param>
        /// <param name="r">Bounding rectangle</param>
        private void DrawVTab(ItemPaintArgs e, string text, Rectangle r)
        {
            if (e.ClipRectangle.X < _ClientRect.X)
            {
                Graphics g = e.Graphics;

                _TabWidth = _ClientRect.Height - MultiUserTabHeight;

                if (_TabWidth < MultiUserTabHeight)
                    _TabWidth = Math.Min(MultiUserTabHeight, Math.Max(0, _ClientRect.Height - 4));

                bool isSelected = CalendarView.SelectedOwnerIndex == DisplayedOwnerKeyIndex;

                using (GraphicsPath path = GetVTabPath(_TabWidth))
                {
                    // Draw the tab

                    if (CalendarView.DoRenderTabBackground(g, path, this, isSelected) == false)
                    {
                        using (Brush br = _CalendarColor.BrushPart(
                            isSelected ? _NClientData.TabSelBack : _NClientData.TabBack, r))
                        {
                            g.FillPath(br, path);
                        }

                        using (Pen pen = new
                            Pen(_CalendarColor.GetColor(_NClientData.TabBorder)))
                        {
                            g.DrawPath(pen, path);
                        }
                    }

                    // Draw the tab text

                    if (CalendarView.DoRenderTabContent(g, path, this, ref text, isSelected) == false)
                    {
                        r.Height -= 4;

                        // Set uo our output to have vertically centered text

                        StringFormat sf = new StringFormat();

                        sf.FormatFlags = StringFormatFlags.NoWrap;
                        sf.Alignment = StringAlignment.Center;

                        // Calc our rect origin and prepare to
                        // output the SideBar text

                        PointF ptf = new PointF(r.X, r.Bottom);
                        Rectangle rf = new Rectangle(0, 0, _TabWidth, MultiUserTabHeight);

                        g.TranslateTransform(ptf.X, ptf.Y);
                        g.RotateTransform(-90);

                        using (Brush br = _CalendarColor.BrushPart(
                            isSelected ? _NClientData.TabSelFore : _NClientData.TabFore, rf))
                        {
                            g.DrawString(text, Font, br, rf, sf);
                        }

                        g.ResetTransform();
                    }
                }
            }
        }

        #endregion

        #region GetVTabPath

        /// <summary>
        /// Gets the tab graphics path
        /// </summary>
        /// <param name="tabWidth">Tab width</param>
        /// <returns>GraphicsPath</returns>
        protected GraphicsPath GetVTabPath(int tabWidth)
        {
            GraphicsPath path = new GraphicsPath();

            Rectangle r = ClientRect;

            r.X -= (MultiUserTabHeight + 1);
            r.Width = MultiUserTabHeight;

            path.AddLine(r.Right, r.Bottom - 1,
                r.X + TabRadius, r.Bottom - 1);

            path.AddArc(r.X, r.Bottom - 1 - TabDia, TabDia, TabDia, 90, 90);

            if (r.Height > MultiUserTabHeight * 2)
            {
                // Full tab

                 path.AddLines(new Point[] {
                    new Point(r.X, r.Bottom - TabRadius),
                    new Point(r.X, r.Bottom - tabWidth),
                    new Point(r.Right, r.Bottom - tabWidth - MultiUserTabHeight) });
            }
            else if (r.Height > MultiUserTabHeight)
            {
                // Partial tab with end slant

                int n = r.Height - MultiUserTabHeight;

                path.AddLines(new Point[] {
                    new Point(r.X, r.Bottom - TabRadius),
                    new Point(r.X, r.Top + n),
                    new Point(r.X + n, r.Y),
                    new Point(r.Right, r.Y) });
            }
            else
            {
                // partial tab with no end slant

                path.AddLines(new Point[] {
                    new Point(r.X, Bounds.Bottom - TabRadius),
                    new Point(r.X, Bounds.Top),
                    new Point(r.X + MultiUserTabHeight, Bounds.Top) });
            }

            return (path);
        }

        #endregion

        #region DrawVTab2

        /// <summary>
        /// Draws the tab - border and content
        /// </summary>
        /// <param name="e">ItemPaintArgs</param>
        /// <param name="text">Tab text</param>
        /// <param name="r">Bounding rectangle</param>
        private void DrawVTab2(ItemPaintArgs e, string text, Rectangle r)
        {
            if (e.ClipRectangle.X < _ClientRect.X)
            {
                Graphics g = e.Graphics;

                _TabWidth = _ClientRect.Width - MultiUserTabWidth;

                if (_TabWidth < MultiUserTabWidth)
                    _TabWidth = Math.Min(MultiUserTabWidth, Math.Max(0, _ClientRect.Width - 4));

                bool isSelected = CalendarView.SelectedOwnerIndex == DisplayedOwnerKeyIndex;

                using (GraphicsPath path = GetVTab2Path(_TabWidth))
                {
                    // Draw the tab

                    if (CalendarView.DoRenderTabBackground(g, path, this, isSelected) == false)
                    {
                        using (Brush br = _CalendarColor.BrushPart(
                            isSelected ? _NClientData.TabSelBack : _NClientData.TabBack, r))
                        {
                            g.FillPath(br, path);
                        }

                        using (Pen pen = new
                            Pen(_CalendarColor.GetColor(_NClientData.TabBorder)))
                        {
                            g.DrawPath(pen, path);
                        }
                    }

                    // Draw the tab text

                    if (CalendarView.DoRenderTabContent(g, path, this, ref text, isSelected) == false)
                    {
                        r.Y += 2;
                        r.Height -= 4;

                        // Set uo our output to have vertically centered text

                        StringFormat sf = new StringFormat();

                        sf.FormatFlags = StringFormatFlags.NoWrap;
                        sf.Alignment = StringAlignment.Center;
                        sf.LineAlignment = StringAlignment.Center;

                        // Calc our rect origin and prepare to
                        // output the SideBar text

                        using (Brush br = _CalendarColor.BrushPart(
                            isSelected ? _NClientData.TabSelFore : _NClientData.TabFore, r))
                        {
                            g.DrawString(text, Font, br, r, sf);
                        }
                    }
                }
            }
        }

        #endregion

        #region GetVTab2Path

        /// <summary>
        /// Gets the tab graphics path
        /// </summary>
        /// <param name="tabWidth">Tab width</param>
        /// <returns>GraphicsPath</returns>
        protected GraphicsPath GetVTab2Path(int tabWidth)
        {
            GraphicsPath path = new GraphicsPath();

            Rectangle r = ClientRect;

            r.X -= (MultiUserTabWidth + 1);
            r.Width = MultiUserTabWidth;

            path.AddLine(r.Right, r.Bottom - 1,
                r.X + TabRadius, r.Bottom - 1);

            path.AddArc(r.X, r.Bottom - 1 - TabDia, TabDia, TabDia, 90, 90);
            path.AddArc(r.X, r.Top, TabDia, TabDia, 180, 90);

            path.AddLine(r.Right, r.Top, r.Right, r.Bottom - 1);

            path.CloseFigure();

            return (path);
        }

        #endregion

        #endregion

        #endregion

        #region GetTabPath

        /// <summary>
        /// Gets the multiuser tab path
        /// </summary>
        /// <param name="tabWidth"></param>
        /// <returns></returns>
        GraphicsPath GetTabPath(int tabWidth)
        {
            if (NClientData.TabOrientation == eTabOrientation.Horizontal)
                return (GetHTabPath(tabWidth));

            return (GetVTabPath(tabWidth));
        }

        #endregion

        #region PointInTab

        /// <summary>
        /// Determines if the given Point is
        /// within the View tab area
        /// </summary>
        /// <param name="pt">Point in question</param>
        /// <returns>true if the Point is in the tab</returns>
        private bool PointInTab(Point pt)
        {
            bool isInTab = false;

            if (CalendarView.IsMultiCalendar == true)
            {
                if (NClientData.TabOrientation == eTabOrientation.Horizontal)
                {
                    if (pt.Y < ClientRect.Y)
                    {
                        using (GraphicsPath path = GetHTabPath(_TabWidth))
                        {
                            if (path.IsVisible(pt) == true)
                                isInTab = true;
                        }
                    }
                }
                else
                {
                    if (pt.X < ClientRect.X)
                    {
                        if (CalendarView.TimeLineMultiUserTabOrientation == eOrientation.Vertical)
                        {
                            using (GraphicsPath path = GetVTabPath(_TabWidth))
                            {
                                if (path.IsVisible(pt) == true)
                                    isInTab = true;
                            }
                        }
                        else
                        {
                            using (GraphicsPath path = GetVTab2Path(_TabWidth))
                            {
                                if (path.IsVisible(pt) == true)
                                    isInTab = true;
                            }
                        }
                    }
                }

            }

            return (isInTab);
        }

        #endregion

        #region Mouse support

        #region InternalMouseDown

        /// <summary>
        /// MouseDown processing
        /// </summary>
        /// <param name="objArg">MouseEventArgs</param>
        public override void InternalMouseDown(MouseEventArgs objArg)
        {
            // Pass the event on through

            base.InternalMouseDown(objArg);

            // Check to see if the use is attempting to reorder
            // the view via MouseDown in the view tab

            Point pt = objArg.Location;

            if (PointInTab(pt) == true)
            {
                SelectedItem = null;

                _CalendarView.SelectedOwnerIndex = DisplayedOwnerKeyIndex;
                
                if (objArg.Button == MouseButtons.Left)
                {
                    if (CalendarView.AllowTabReorder && CalendarView.DisplayedOwners.Count > 1)
                    {
                        _IsTabMoving = true;
                        _LastViewIndex = CalendarView.GetViewIndexFromPoint(pt);
                    }
                }
            }
            else if (pt.Y >= ClientRect.Y)
            {
                _CalendarView.SelectedOwnerIndex = DisplayedOwnerKeyIndex;
            }
        }

        #endregion

        #region InternalMouseMove

        /// <summary>
        /// MouseMove processing
        /// </summary>
        /// <param name="objArg"></param>
        public override void InternalMouseMove(MouseEventArgs objArg)
        {
            // Pass the event on through

            base.InternalMouseMove(objArg);

            // If the user has initiated a tab reorder, then
            // track the tab movement

            if (_IsTabMoving == true)
            {
                Point pt = objArg.Location;

                int viewIndex = CalendarView.GetViewIndexFromPoint(pt);

                // If the user has dragged the tab to another view location
                // then reorder the tabs accordingly

                if (viewIndex >= 0 && viewIndex != _LastViewIndex)
                {
                    CalendarView.ReorderViews(_LastViewIndex, viewIndex);
                    _LastViewIndex = viewIndex;
                }

                MyCursor = Cursors.SizeAll;
            }
        }

        #endregion

        #region InternalMouseUp

        /// <summary>
        /// MouseUp processing
        /// </summary>
        /// <param name="objArg"></param>
        public override void InternalMouseUp(MouseEventArgs objArg)
        {
            base.InternalMouseUp(objArg);

            // Hide our PosWindow if it is visible

            if (_PosWin != null)
                _PosWin.Hide();

            if (SelectedItem != null)
            {
                // DoAppointmentViewChanged

                if (IsMoving == true)
                {
                    if (TimeChanged(SelectedItem) == true || OwnerChanged() == true || IsCopyDrag == true)
                        DoViewChanged(SelectedItem, eViewOperation.AppointmentMove);
                }
                else if (IsResizing == true)
                {
                    if (TimeChanged(SelectedItem) == true)
                        DoViewChanged(SelectedItem, eViewOperation.AppointmentResize);
                }
            }

            // Reset our mouse states

            ClearMouseStates();
        }

        private bool TimeChanged(CalendarItem item)
        {
            return (item.StartTime != _oldStartTime || item.EndTime != _oldEndTime);
        }

        private bool OwnerChanged()
        {
            if (OwnerKey != null)
                return (OwnerKey.Equals(_oldOwnerKey) == false);

            return (false);
        }

        private void DoViewChanged(CalendarItem item, eViewOperation eop)
        {
            if (item.StartTime != _oldStartTime)
                UpdateReminders(item, _oldStartTime);

            CalendarView.DoAppointmentViewChanged(item, _oldStartTime, _oldEndTime, eop, _IsCopyDrag);
        }

        /// <summary>
        /// Clears all mouse related state flags
        /// </summary>
        protected void ClearMouseStates()
        {
            _IsMouseDown = false;
            _IsMoving = false;
            _IsStartResizing = false;
            _IsEndResizing = false;
            _IsTabMoving = false;
            _IsCondMoving = false;
            _IsCopyDrag = false;
        }

        #endregion

        #endregion

        #region DragItem

        /// <summary>
        /// Initiates dragging of a copy of the
        /// current selectedItem - if the ctrl-key is pressed
        /// </summary>
        /// <returns></returns>
        protected bool DragCopy()
        {
            if (_IsCopyDrag == false &&
                (Control.ModifierKeys & Keys.Control) == Keys.Control)
            {
                AppointmentView av = SelectedItem as AppointmentView;

                if (av != null)
                {
                    Appointment ap = av.Appointment.Copy();

                    CalendarModel.Appointments.Add(ap);
                }

                _IsCopyDrag = true;

                return (true);
            }

            return (false);
        }

        #endregion

        #region UpdateReminders

        /// <summary>
        /// Updates associated appointment reminders
        /// after an appointment has been moved
        /// </summary>
        protected void UpdateReminders(CalendarItem item, DateTime oldStartTime)
        {
            //AppointmentView view = item as AppointmentView;

            //if (view != null)
            //{
            //    ReminderCollection rc = view.Appointment.Reminders;

            //    if (rc != null && rc.Count > 0)
            //    {
            //        TimeSpan ts = item.StartTime - oldStartTime;

            //        for (int i = 0; i < rc.Count; i++)
            //            rc[i].ReminderTime = rc[i].ReminderTime.Add(ts);
            //    }
            //}
        }

        #endregion

        #region GetViewAreaFromPoint

        /// <summary>
        /// Gets the view area under the given mouse
        /// point (tab, header, content, etc)
        /// </summary>
        /// <param name="pt">Point</param>
        /// <returns>eViewArea</returns>
        public virtual eViewArea GetViewAreaFromPoint(Point pt)
        {
            if (Bounds.Contains(pt) == true)
            {
                if (NClientData.TabOrientation == eTabOrientation.Horizontal)
                {
                    if (pt.Y < ClientRect.Y)
                    {
                        using (GraphicsPath path = GetTabPath(_TabWidth))
                        {
                            if (path.IsVisible(pt) == true)
                                return (eViewArea.InTab);
                        }
                    }
                }
                else
                {
                    if (pt.X < ClientRect.X)
                    {
                        using (GraphicsPath path = GetTabPath(_TabWidth))
                        {
                            if (path.IsVisible(pt) == true)
                                return (eViewArea.InTab);
                        }
                    }
                }
            }

            return (eViewArea.NotInView);
        }

        #endregion

        #region GetDateSelectionFromPoint

        /// <summary>
        /// Gets the date selection from the given point. The startDate
        /// and endDate will vary based upon the view type (WeekDay / Month)
        /// </summary>
        /// <param name="pt">Point in question</param>
        /// <param name="startDate">out start date</param>
        /// <param name="endDate">out end date</param>
        /// <returns>True if a valid selection exists
        /// at the given point</returns>
        public virtual bool GetDateSelectionFromPoint(
            Point pt, out DateTime startDate, out DateTime endDate)
        {
            startDate = new DateTime();
            endDate = new DateTime();

            return (false);
        }

        #endregion

        #region GetAppointmentView

        /// <summary>
        /// Gets the appointment view created for an appointment in this view
        /// </summary>
        /// <param name="appointment">The appointment</param>
        /// <returns>Reference to AppointmentView or null if no view is found</returns>
        public virtual AppointmentView GetAppointmentView(Appointment appointment)
        {
            for (int i = 0; i < SubItems.Count; i++)
            {
                AppointmentView view = SubItems[i] as AppointmentView;

                if (view != null)
                {
                    if (view.Appointment == appointment)
                        return (view);
                }
            }

            return (null);
        }

        #endregion

        #region CustomCalendarItem

        /// <summary>
        /// Gets the CustomCalendarItem created for this view
        /// </summary>
        /// <returns>Reference to CustomCalendarItem or null if none found</returns>
        public virtual CustomCalendarItem GetCustomCalendarItem(CustomCalendarItem item)
        {
            if (item.BaseCalendarItem != null)
                item = item.BaseCalendarItem;

            for (int i = 0; i < SubItems.Count; i++)
            {
                CustomCalendarItem view = SubItems[i] as CustomCalendarItem;

                if (view != null)
                {
                    if (view == item || view.BaseCalendarItem == item)
                        return (view);
                }
            }

            return (null);
        }

        #endregion

        #region Invalidate routines

        /// <summary>
        /// Invalidates the given rectangle
        /// </summary>
        internal void InvalidateRect()
        {
            InvalidateRect(ClientRect, false);
        }

        /// <summary>
        /// Invalidates the entire calendar
        /// bounding rect area
        /// </summary>
        /// <param name="needRecalc">NeedRecalcSize flag</param>
        internal void InvalidateRect(bool needRecalc)
        {
            InvalidateRect(ClientRect, needRecalc);
        }

        /// <summary>
        /// Invalidates the entire calendar
        /// bounding rect area
        /// </summary>
        internal void InvalidateRect(Rectangle rect)
        {
            InvalidateRect(rect, false);
        }

        /// <summary>
        /// Invalidates the given rectangle
        /// </summary>
        /// <param name="rect">Rectangle to invalidate</param>
        /// <param name="needRecalc">NeedRecalcSize flag</param>
        internal void InvalidateRect(Rectangle rect, bool needRecalc)
        {
            Control c = (Control)this.GetContainerControl(true);

            if (c != null)
            {
                rect.Height += 0;
                rect.Width += 0;

                if (rect.Bottom >= ClientRect.Bottom)
                    rect.Width += 0;

                c.Invalidate(rect);
            }

            if (needRecalc == true)
                this.NeedRecalcSize = true;
        }

        #endregion

        #region ResetView

        /// <summary>
        /// Disconnects and resets the Model connection
        /// </summary>
        internal virtual void ResetView()
        {
            if (_Connector != null)
            {
                // Disconnect the Model

                _Connector.Disconnect();
                _Connector = null;

                // Reset our selected item

                SelectedItem = null;
            }

            DateRangeChanged = false;

            NeedRecalcLayout = true;
            NeedRecalcSize = true;
        }

        #endregion

        #region SetSelectedItem

        /// <summary>
        /// Sets the current selected item
        /// </summary>
        /// <param name="pci">Previously selected CalendarItem</param>
        /// <param name="nci">New CalendarItem to select</param>
        /// <returns>Base selected CalendarItem</returns>
        protected virtual CalendarItem SetSelectedItem(CalendarItem pci, CalendarItem nci)
        {
            return (null);
        }

        #endregion

        #region UpdateDateSelection

        /// <summary>
        /// Updates each monthWeeks DayRects to reflect
        /// the date selection start and end values
        /// </summary>
        protected virtual void UpdateDateSelection()
        {
        }

        internal void SetDateSelection(DateTime dateStart, DateTime dateEnd)
        {
            UpdateDateSelection();
        }

        #endregion

        #region AutoSyncViewDate

        /// <summary>
        /// AutoSync our view start date
        /// </summary>
        /// <param name="syncView"></param>
        protected void AutoSyncViewDate(eCalendarView syncView)
        {
            if (CalendarView.AutoSyncViewDates == true)
            {
                if (CalendarView.AutoSyncDate != DateTime.MinValue)
                {
                    DateTime syncDate = CalendarView.AutoSyncDate;

                    CalendarView.ShowViewDate(this.ECalendarView, syncDate);

                    CalendarView.AutoSyncDate = syncDate;
                }
            }
        }

        #endregion

        #region ExtendSelection

        /// <summary>
        /// Extends the selection if the shift-key
        /// is pressed with selection
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        protected void ExtendSelection(ref DateTime startDate, ref DateTime endDate)
        {
            if (DateSelectionAnchor.HasValue == false ||
                ((Control.ModifierKeys & Keys.Shift) != Keys.Shift))
            {
                DateSelectionAnchor = startDate;
            }
            else
            {
                if (DateSelectionAnchor < startDate)
                    startDate = DateSelectionAnchor.Value;

                else
                {
                    DateTime anchorEnd = DateSelectionAnchor.Value.Add(endDate - startDate);

                    if (anchorEnd > endDate)
                        endDate = anchorEnd;
                }
            }

            CalendarView.AutoSyncDate = startDate;
        }

        #endregion

        #region ValidDateSelection

        protected bool ValidDateSelection()
        {
            if (DateSelectionStart.HasValue && DateSelectionEnd.HasValue &&
                DateSelectionAnchor.HasValue)
            {
                return (DateSelectionStart <= DateSelectionEnd);
            }

            return (false);
        }

        #endregion

        #region PosWindow support

        /// <summary>
        /// Pos window update
        /// </summary>
        protected void UpdatePosWin(Rectangle viewRect)
        {
            if (IsStartResizing || IsEndResizing || IsMoving)
            {
                // Only display if the Alt key is pressed

                if ((Control.ModifierKeys & Keys.Alt) == Keys.Alt)
                {
                    // Allocate our pos window if not
                    // previously allocated

                    if (PosWin == null)
                    {
                        PosWin = new PosWin(this);
                        PosWin.Size = new Size(1, 1);

                        Control c = (Control)this.GetContainerControl(true);

                        if (c != null)
                            PosWin.Owner = c.FindForm();
                    }

                    PosWin.UpdateWin(viewRect);
                }
                else
                {
                    if (PosWin != null)
                        PosWin.Hide();
                }
            }
        }

        #endregion

        #region GetIndicatorRect

        internal virtual Rectangle GetIndicatorRect(TimeIndicator ti)
        {
            return (Rectangle.Empty);
        }

        internal virtual Rectangle GetIndicatorRect(TimeIndicator ti, DateTime time)
        {
            return (Rectangle.Empty);
        }


        #endregion

        #region InternalKeyUp

        internal virtual void InternalKeyUp(KeyEventArgs e)
        {
        }

        #endregion

        #region Copy implementation

        /// <summary>
		/// Returns copy of the item.
		/// </summary>
        public override BaseItem Copy()
        {
            BaseView objCopy = new BaseView(_CalendarView, _ECalendarView);
            CopyToItem(objCopy);

            return (objCopy);
        }

        /// <summary>
		/// Copies the BaseView specific properties to new instance of the item.
		/// </summary>
		/// <param name="copy">New BaseView instance</param>
        protected override void CopyToItem(BaseItem copy)
        {
            BaseView objCopy = copy as BaseView;

            if (objCopy != null)
            {
                base.CopyToItem(objCopy);

                objCopy._DateSelectionStart = this._DateSelectionStart;
                objCopy._DateSelectionEnd = this._DateSelectionEnd;
                objCopy._DisplayName = this._DisplayName;
                objCopy._EndDate = this._EndDate;
                objCopy._StartDate = this._StartDate;
            }
        }

        #endregion
  
        #region IDisposable Members

        protected override void Dispose(bool disposing)
        {
            if (disposing == true && IsDisposed == false)
            {
                ResetView();
                HookEvents(false);
            }

            base.Dispose(disposing);
        }

        #endregion
    }

    #region enums

    public enum eViewArea
    {
        NotInView,

        InTab,
        InContent,
        InAllDayPanel,
        InDayOfWeekHeader,
        InWeekHeader,
        InSideBarPanel,
        InIntervalHeader,
        InPeriodHeader,
        InCondensedView,
        InMonthHeader
    }

    public enum eTabOrientation
    {
        Vertical,
        Horizontal,
    }

    #endregion

    #region EventsArgs
    /// <summary>
    /// SelectedItemChangedEventArgs
    /// </summary>
    public class SelectedItemChangedEventArgs : ValueChangedEventArgs<CalendarItem, CalendarItem>
    {
        public SelectedItemChangedEventArgs(CalendarItem oldValue, CalendarItem newValue)
            : base(oldValue, newValue)
        {
        }
    }

    /// <summary>
    /// CalendarColorChangedEventArgs
    /// </summary>
    public class CalendarColorChangedEventArgs : ValueChangedEventArgs<eCalendarColor, eCalendarColor>
    {
        public CalendarColorChangedEventArgs(eCalendarColor oldValue, eCalendarColor newValue)
            : base(oldValue, newValue)
        {
        }
    }

    #endregion
}
#endif

