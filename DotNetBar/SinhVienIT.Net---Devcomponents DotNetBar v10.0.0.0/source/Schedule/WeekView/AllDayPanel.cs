#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Schedule
{
    public class AllDayPanel : BaseItem
    {
        #region Private variables

        private WeekDayView _WeekDayView;       // Assoc WeekDayView

        private List<CalendarItem>              // CalendarItems list
            _CalendarItems = new List<CalendarItem>();

        private VScrollBarAdv _VScrollBar;      // Vertical scroll bar
        private int _VScrollPos;                // Vertical scrollbar pos

        private int _PanelHeight;               // Panel height (current)
        private int _MaximumPanelHeight;        // Panel height (max)

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="weekDayView">WeekDayView</param>
        public AllDayPanel(WeekDayView weekDayView)
        {
            // Save the provided weekDayView and
            // tell the system we are a container

            _WeekDayView = weekDayView;

            SetIsContainer(true);

            SubItems.AllowParentRemove = false;
        }

        #region Public properties

        /// <summary>
        /// Gets and sets the panel bounding rectangle
        /// </summary>
        public override Rectangle Bounds
        {
            get { return (base.Bounds); }

            set
            {
                base.Bounds = value;

                UpdateVScrollBar();
            }
        }

        /// <summary>
        /// Gets the DayPanel Height
        /// </summary>
        public int PanelHeight
        {
            get { return (_PanelHeight); }
        }

        /// <summary>
        /// Gets the panel's CalendarItem list
        /// </summary>
        public List<CalendarItem> CalendarItems
        {
            get { return (_CalendarItems); }
        }

        /// <summary>
        /// Gets WeekDayView
        /// </summary>
        public WeekDayView WeekDayView
        {
            get { return (_WeekDayView); }
        }

        #endregion

        #region Private properties
        
        /// <summary>
        /// gets the Fixed AllDayPanel height
        /// </summary>
        private int FixedAllDayPanelHeight
        {
            get { return (_WeekDayView.CalendarView.FixedAllDayPanelHeight); }
        }

        /// <summary>
        /// gets the Maximum AllDayPanel height
        /// </summary>
        private int MaximumAllDayPanelHeight
        {
            get { return (_WeekDayView.CalendarView.MaximumAllDayPanelHeight); }
        }

        /// <summary>
        /// Gets the Appointment height
        /// </summary>
        private int AppointmentPadding
        {
            get { return (6); }
        }

        /// <summary>
        /// Gets the width of a vertical scrollbar
        /// </summary>
        private int VsWidth
        {
            get { return (SystemInformation.VerticalScrollBarWidth); }
        }

        #endregion

        #region RecalcSize

        /// <summary>
        /// Performs panel recalc support
        /// </summary>
        public override void RecalcSize()
        {
            // Reset our panel heights

            _PanelHeight = 0;
            _MaximumPanelHeight = 0;

            if (_CalendarItems.Count > 0)
            {
                // Sort the items

                List<CalendarItem> items = SortCalendarItems();

                // Go through our CalendarItems on a per column basis
                // accumulating extended appointment data

                int n = _WeekDayView.NumberOfColumns;

                int[] acc = new int[n];

                for (int i = 0; i < items.Count; i++)
                    CalcAppointmentBounds(items[i], ref acc);

                // Determine our current panel height from our processed data

                if (_WeekDayView.CalendarView.IsMultiCalendar == false)
                {
                    for (int i = 0; i < n; i++)
                    {
                        if (acc[i] > _MaximumPanelHeight)
                            _MaximumPanelHeight = acc[i];
                    }

                    // Set out current and maximum panel heights

                    _MaximumPanelHeight += AppointmentPadding;
                    _PanelHeight = _MaximumPanelHeight;

                    if (_PanelHeight > MaximumAllDayPanelHeight)
                        _PanelHeight = MaximumAllDayPanelHeight;
                }
            }

            // Take into account the FixedAllDayPanelHeight setting

            if (_WeekDayView.CalendarView.IsMultiCalendar == true)
            {
                _PanelHeight = (FixedAllDayPanelHeight >= 0) ? FixedAllDayPanelHeight : 50;
                _MaximumPanelHeight = _PanelHeight;
            }
            else if (FixedAllDayPanelHeight >= 0)
            {
                _PanelHeight = FixedAllDayPanelHeight;
            }

            HeightInternal = _PanelHeight;

            base.RecalcSize();
        }

        #region SortCalendarItems

        /// <summary>
        /// Sorts the CalendarItems
        /// </summary>
        private List<CalendarItem> SortCalendarItems()
        {
            List<CalendarItem> items =
                new List<CalendarItem>(_CalendarItems.Count);

            items.AddRange(_CalendarItems);

            items.Sort(

               delegate(CalendarItem c1, CalendarItem c2)
               {
                   if (c1.StartTime > c2.StartTime)
                       return (1);

                   if (c1.StartTime < c2.StartTime)
                       return (-1);

                   return (0);
               }
            );

            return (items);
        }

        #endregion

        #region CalcAppointmentBounds

        /// <summary>
        /// Calculates the display bounds for the AppointmentView
        /// </summary>
        /// <param name="item">CalendarItem</param>
        /// <param name="acc">Row accumulator</param>
        private void CalcAppointmentBounds(CalendarItem item, ref int[] acc)
        {
            // Determine the starting day index for
            // the given appointment

            int ns = GetDayIndex(item);

            // Calculate the top and height for the item

            Rectangle r = _WeekDayView.DayColumns[ns].Bounds;

            r.X += 2;
            r.Width -= 3;

            r.Y = _WeekDayView.ClientRect.Y + _WeekDayView.DayOfWeekHeaderHeight + _VScrollPos + 2;
            r.Height = _WeekDayView.AppointmentHeight;

            // Check to see if the appointment spans
            // multiple days

            if (item.StartTime.Day != item.EndTime.Day ||
                (item.EndTime - item.StartTime).TotalDays > 1)
            {
                // Determine the ending day index

                DateTime st = _WeekDayView.DayColumns[ns].Date;

                int ne = ns + (item.EndTime - st).Days;

                if (item.EndTime.Hour > 0 || item.EndTime.Minute > 0 || item.EndTime.Second > 0)
                    ne++;

                if (ne > acc.Length)
                    ne = acc.Length;

                // Loop through each covered day, accumulating
                // the day width and max key slot height

                int maxAcc = acc[ns];

                for (int i = ns + 1; i < ne; i++)
                {
                    r.Width += _WeekDayView.DayColumns[i].Bounds.Width;

                    if (acc[i] > maxAcc)
                        maxAcc = acc[i];
                }

                // Set the top of this item to the calculated
                // slot height

                r.Y += maxAcc;

                // Loop through each effected slot and adjust it's
                // accumulated values accordingly

                maxAcc += _WeekDayView.AppointmentHeight;

                for (int i = ns; i < ne; i++)
                    acc[i] = maxAcc;
            }
            else
            {
                // This is a single day appointment, so set and
                // adjust it's values accordingly

                r.Y += acc[ns];
                acc[ns] += _WeekDayView.AppointmentHeight;
            }

            // Now that we have calculated the items height and
            // width, invoke a Recalc on the item

            item.WidthInternal = r.Width;
            item.HeightInternal = r.Height - 1;

            item.RecalcSize();

            // Set our bounds for the item

            r.Width = item.WidthInternal;
            r.Height = item.HeightInternal;

            item.Bounds = r;

            // Set it's display state

            item.Displayed = true;
        }

        /// <summary>
        /// Gets the starting day index for the given appointment
        /// </summary>
        /// <returns>Day of week index (0-6)</returns>
        private int GetDayIndex(CalendarItem item)
        {
            DateTime date = _WeekDayView.DayColumns[0].Date;

            for (int i = 0; i < _WeekDayView.DayColumns.Length; i++)
            {
                date = date.AddDays(1);

                if (date > item.StartTime)
                    return (i);
            }

            return (0);
        }

        #endregion

        #endregion

        #region Scrollbar routines

        #region UpdateVScrollBar

        /// <summary>
        /// Updates our vertical scrollbar
        /// </summary>
        private void UpdateVScrollBar()
        {
            if (_PanelHeight > 0 && _PanelHeight < _MaximumPanelHeight)
            {
                // If we don't have one already, allocate it

                if (_VScrollBar == null)
                {
                    _VScrollBar = new VScrollBarAdv();
                    _VScrollBar.Width = VsWidth;

                    Control c = (Control)this.GetContainerControl(true);

                    if (c != null)
                        c.Controls.Add(_VScrollBar);

                    _VScrollBar.ValueChanged += _VExtScrollBar_ValueChanged;
                }

                // Initialize the scrollbar

                _VScrollBar.Location = new Point(Bounds.Right + 1, Bounds.Y);
                _VScrollBar.Height = _PanelHeight;

                _VScrollBar.SmallChange = _PanelHeight / 2;
                _VScrollBar.LargeChange = _WeekDayView.AppointmentHeight * 2;

                _VScrollBar.Maximum = _MaximumPanelHeight -
                    _VScrollBar.Height + _VScrollBar.LargeChange;

                if (_VScrollBar.Visible == false)
                {
                    _VScrollPos = 0;
                    _VScrollBar.Value = 0;

                    _VScrollBar.Show();
                    _VScrollBar.BringToFront();
                }
                else
                {
                    if (_VScrollBar.Value > _VScrollBar.Maximum - _VScrollBar.LargeChange)
                        _VScrollBar.Value = _VScrollBar.Maximum - _VScrollBar.LargeChange;
                }

                _VScrollBar.Refresh();
            }
            else if (_VScrollBar != null)
            {
                _VScrollPos = 0;

                _VScrollBar.ValueChanged -= _VExtScrollBar_ValueChanged;

                _VScrollBar.Dispose();
                _VScrollBar = null;
            }
        }

        #endregion

        #region _VExtScrollBar_ValueChanged

        /// <summary>
        /// Processes Extended appointments scrollBar changes
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        void _VExtScrollBar_ValueChanged(object sender, EventArgs e)
        {
            int vdelta = -_VScrollBar.Value - _VScrollPos;

            if (vdelta != 0)
            {
                _VScrollPos = -_VScrollBar.Value;

                // Now that we have calculated the items height and
                // width, invoke a Recalc on the item

                for (int i = 0; i < _CalendarItems.Count; i++)
                {
                    CalendarItem item = _CalendarItems[i];

                    Rectangle r = item.Bounds;
                    r.Y += vdelta;

                    item.Bounds = r;
                }

                Refresh();
            }
        }

        #endregion

        #endregion

        #region Reset / update view

        /// <summary>
        /// Resets the AllDayPanel view
        /// </summary>
        public void ResetView()
        {
            _VScrollPos = 0;

            if (_VScrollBar != null)
            {
                _VScrollBar.ValueChanged -= _VExtScrollBar_ValueChanged;
                _VScrollBar.Dispose();
                _VScrollBar = null;
            }
        }

        /// <summary>
        /// Updates the AllDayPanel view
        /// </summary>
        public void UpdateView()
        {
            if (_WeekDayView.IsViewSelected == true)
            {
                if (_VScrollBar != null)
                    _VScrollBar.Show();
            }
            else
            {
                if (_VScrollBar != null)
                    _VScrollBar.Hide();
            }
        }

        #endregion

        #region Paint processing

        /// <summary>
        /// Draws extended appointments
        /// </summary>
        /// <param name="e">ItemPaintArgs</param>
        public override void Paint(ItemPaintArgs e)
        {
            Graphics g = e.Graphics;

            if (e.ClipRectangle.IntersectsWith(Bounds))
            {
                Region regSave = g.Clip;
                g.SetClip(Bounds, CombineMode.Intersect);

                using (Brush br =
                    _WeekDayView.WeekDayColor.BrushPart(
                        (int) eCalendarWeekDayPart.DayAllDayEventBackground, Bounds))
                {
                    g.FillRectangle(br, Bounds);
                }

                using (Pen pen = new Pen(
                    _WeekDayView.WeekDayColor.GetColor((int) eCalendarWeekDayPart.DayViewBorder)))
                {
                    g.DrawLine(pen, Bounds.X, Bounds.Y, Bounds.X, Bounds.Bottom);
                    g.DrawLine(pen, Bounds.Right - 1, Bounds.Y, Bounds.Right - 1, Bounds.Bottom);
                }

                // Loop through each day in each week, displaying
                // the associated day content

                int selItem = -1;

                for (int i = 0; i < _CalendarItems.Count; i++)
                {
                    // Initiate the paint

                    if (_CalendarItems[i].Displayed == true)
                    {
                        if (_CalendarItems[i].IsSelected == true)
                            selItem = i;
                        else
                            _CalendarItems[i].Paint(e);
                    }
                }

                if (selItem >= 0)
                    _CalendarItems[selItem].Paint(e);

                // Restore the original clip region

                g.Clip = regSave;
            }
        }

        #endregion

        #region Copy implementation

        /// <summary>
        /// Returns copy of the item.
        /// </summary>
        public override BaseItem Copy()
        {
            AllDayPanel objCopy = new AllDayPanel(WeekDayView);
            CopyToItem(objCopy);

            return (objCopy);
        }

        /// <summary>
        /// Copies the AllDayPanel specific properties to new instance of the item.
        /// </summary>
        /// <param name="copy">New AllDayPanel instance</param>
        protected override void CopyToItem(BaseItem copy)
        {
            AllDayPanel objCopy = copy as AllDayPanel;
            base.CopyToItem(objCopy);
        }

        #endregion

    }
}
#endif

