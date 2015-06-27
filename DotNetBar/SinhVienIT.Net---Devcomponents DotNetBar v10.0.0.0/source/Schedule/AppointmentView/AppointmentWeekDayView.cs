#if FRAMEWORK20
using System;
using DevComponents.Schedule.Model;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Schedule
{
    public class AppointmentWeekDayView : AppointmentView
    {
        #region Private variables

        private DayColumn _DayColumn;
        private AllDayPanel _AllDayPanel;
        private eViewEnds _ViewEnds = eViewEnds.Complete;

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="baseView"></param>
        /// <param name="appointment"></param>
        public AppointmentWeekDayView(BaseView baseView, Appointment appointment)
            : base(baseView, appointment)
        {
            // Set our initial ViewEnds

            SetViewEnds();
        }

        #region Public properties

        /// <summary>
        /// Gets and sets View DayColumn
        /// </summary>
        public DayColumn DayColumn
        {
            get { return (_DayColumn); }
            set { _DayColumn = value; }
        }

        /// <summary>
        /// Gets and sets View AllDayPanel
        /// </summary>
        public AllDayPanel AllDayPanel
        {
            get { return (_AllDayPanel); }

            set
            {
                if (_AllDayPanel != value)
                {
                    _AllDayPanel = value;

                    SetViewEnds();
                }
            }
        }

        #endregion

        #region Private properties

        /// <summary>
        /// Gets the default horizontal padding
        /// </summary>
        private int HorizontalPadding
        {
            get { return (6); }
        }

        /// <summary>
        /// Gets whether the appointment is mutable
        /// </summary> 
        private bool IsMutable
        {
            get
            {
                return (IsSelected == true &&
                    Appointment.IsMultiDayOrAllDayEvent == false &&
                    Appointment.Locked == false &&
                    Appointment.IsRecurringInstance == false);
            }
        }

        #endregion

        #region Start/End TimeChanged event handling

        /// <summary>
        /// Handles StartTime value changes
        /// </summary>
        /// <param name="sender">CalendarItem</param>
        /// <param name="e">EventArgs</param>
        protected override void AppointmentView_StartTimeChanged(object sender, EventArgs e)
        {
            base.AppointmentView_StartTimeChanged(sender, e);

            SetViewEnds();
        }

        /// <summary>
        /// Handles EndTime value changes
        /// </summary>
        /// <param name="sender">CalendarItem</param>
        /// <param name="e">EventArgs</param>
        protected override void AppointmentView_EndTimeChanged(object sender, EventArgs e)
        {
            base.AppointmentView_EndTimeChanged(sender, e);

            SetViewEnds();
        }

        #endregion

        #region SetViewEnds

        /// <summary>
        /// Sets the view display end types
        /// </summary>
        private void SetViewEnds()
        {
            if (_AllDayPanel != null)
            {
                _ViewEnds = eViewEnds.Complete;

                int col, n;

                DateTime start = GetDateCol(StartTime, out col, out n);
                DateTime end = start.AddDays(n);

                // Check to see if we can only display
                // a partial representation for the view

                if (col > 0 || start > StartTime)
                    _ViewEnds |= eViewEnds.PartialLeft;

                TimeSpan ts = new TimeSpan(24, 0, 0);

                if (end > EndTime || EndTime - end > ts)
                    _ViewEnds |= eViewEnds.PartialRight;
            }
            else
            {
                _ViewEnds = eViewEnds.PartialLeft | eViewEnds.PartialRight;
            }
        }

        /// <summary>
        /// Gets the initial starting DayColumn col and max col
        /// for the given date
        /// </summary>
        /// <param name="selDate">Sel date</param>
        /// <param name="col">Column</param>
        /// <param name="n">Max col</param>
        /// <returns></returns>
        private DateTime GetDateCol(DateTime selDate, out int col, out int n)
        {
            DayColumn[] dayColumns = _AllDayPanel.WeekDayView.DayColumns;

            // Loop through each column

            col = 0;
            n = 0;

            for (int i = 0; i < dayColumns.Length; i++)
            {
                DateTime date = dayColumns[i].Date;

                // If we have found our containing column, then
                // calculate the absolute slice value and return it

                if (date.Day >= selDate.Day)
                {
                    col = i;
                    n = dayColumns.Length - col - 1;

                    return (date);
                }
            }

            return (selDate);
        }

        #endregion

        #region Appointment rendering

        /// <summary>
        /// Paint processing
        /// </summary>
        /// <param name="e">ItemPaintArgs</param>
        public override void Paint(ItemPaintArgs e)
        {
            Graphics g = e.Graphics;

            // Set our ColorTable

            AppointmentColor.SetColorTable();

            // Establish our initial view display rect

            Rectangle r = GetViewRect();

            if (r.Width > 1 && r.Height > 0)
            {
                // Draw the Appointment background and border

                if (EffectiveStyle == eDotNetBarStyle.Office2010)
                    DrawBorderOffice2010(g, r);
                else
                    DrawBorder(g, r);

                // Output the appointment content (text and image)

                DrawContent(e, r);

                // Draw the resize handles

                if (IsMutable == true)
                    DrawResizeHandles(g, r);
            }
        }

        #region GetViewRect

        /// <summary>
        /// Gets the view rect for the appointment
        /// </summary>
        /// <returns>View rect</returns>
        private Rectangle GetViewRect()
        {
            Rectangle r = this.DisplayRectangle;

            if ((_ViewEnds & eViewEnds.PartialLeft) != eViewEnds.PartialLeft)
            {
                r.X += HorizontalPadding;
                r.Width -= HorizontalPadding;
            }

            if ((_ViewEnds & eViewEnds.PartialRight) != eViewEnds.PartialRight)
                r.Width -= HorizontalPadding;

            // If the view is selected, then allow
            // for a thicker selection rect

            if (IsSelected == true && Appointment.IsMultiDayOrAllDayEvent == false)
                r.Height -= 1;

            return (r);
        }

        #endregion

        #region DrawContent

        /// <summary>
        /// DrawContent
        /// </summary>
        /// <param name="e"></param>
        /// <param name="r"></param>
        private void DrawContent(ItemPaintArgs e, Rectangle r)
        {
            Image image = Image;

            Rectangle rText = r;
            rText.X += 4;
            rText.Width -= 6;

            if (Appointment.TimeMarkedAs != null)
            {
                rText.X += 4;
                rText.Width -= 4;
            }

            Rectangle rImage = GetItemBounds(rText, ref rText, image);

            DrawContentImage(e, r, rImage, image);
            DrawContentText(e, rText);
        }

        #region DrawContentText

        /// <summary>
        /// Draws the content text
        /// </summary>
        /// <param name="e"></param>
        /// <param name="r"></param>
        private void DrawContentText(ItemPaintArgs e, Rectangle r)
        {
            Graphics g = e.Graphics;

            if (DisplayTemplateText(e, r) == false)
            {
                string s = Appointment.Subject;

                eTextFormat tf = eTextFormat.NoPadding | eTextFormat.NoPrefix;

                if (Appointment.IsMultiDayOrAllDayEvent == false)
                {
                    s += "\n" + Appointment.Description;

                    r.Y += 3;
                    r.Height -= 3;

                    tf |= eTextFormat.WordBreak;
                }
                else
                {
                    tf |= eTextFormat.VerticalCenter;
                }

                Font font = Font ?? e.Font;

                if (r.Width > 4)
                    TextDrawing.DrawString(g, s, font, TextColor, r, tf);

                Size size = TextDrawing.MeasureString(g, s, font, r.Width, tf);

                IsTextClipped = (r.Width < size.Width || r.Height < size.Height);
            }
        }

        #endregion

        #endregion

        #region DrawBorder

        /// <summary>
        /// DrawBorder
        /// </summary>
        /// <param name="g">Graphics</param>
        /// <param name="r">Bounding rectangle</param>
        private void DrawBorder(Graphics g, Rectangle r)
        {
            // Get a path that represents the rounded rect
            // and fill it with our appointment category color

            int corner = Math.Min(5, r.Height / 2);
            corner = Math.Min(corner, r.Width / 2);

            using (GraphicsPath path =
                DisplayHelp.GetRoundedRectanglePath(r, corner, corner, corner, corner))
            {
                // Fill the content area

                using (Brush br = BackBrush(r))
                    g.FillPath(br, path);

                // Draw the Time Marker

                DrawTimeMarker(g, r, corner);

                // Output the appointment text and
                // appropriately weighted bounding path

                if (IsSelected == true)
                {
                    using (Pen pen = SelectedBorderPen)
                        g.DrawPath(pen, path);
                }
                else
                {
                    using (Pen pen = BorderPen)
                        g.DrawPath(pen, path);
                }
            }
        }

        #endregion

        #region DrawBorderOffice2010

        /// <summary>
        /// DrawBorderOffice2010
        /// </summary>
        /// <param name="g">Graphics</param>
        /// <param name="r">Bounding rectangle</param>
        private void DrawBorderOffice2010(Graphics g, Rectangle r)
        {
            // Fill the content area

            using (Brush br = BackBrush(r))
                g.FillRectangle(br, r);

            // Draw the Time Marker

            DrawTimeMarker(g, r, 0);

            // Output the appointment text and
            // appropriately weighted bounding path

            if (IsSelected == true)
            {
                using (Pen pen = new Pen(Color.Black, 2))
                    g.DrawRectangle(pen, r);
            }
            else
            {
                using (Pen pen = BorderPen)
                    g.DrawRectangle(pen, r);
            }
        }

        #endregion

        #region DrawResizeHandles

        /// <summary>
        /// Draws the resize handles for the view
        /// </summary>
        /// <param name="g">Graphics</param>
        /// <param name="r">View rectangle</param>
        private void DrawResizeHandles(Graphics g, Rectangle r)
        {
            if (r.Width > 6)
            {
                Rectangle r2 =
                    new Rectangle(r.X + (r.Width / 2) - 2, r.Y - 3, 5, 5);

                // Top handle

                g.FillRectangle(Brushes.White, r2);
                g.DrawRectangle(Pens.Black, r2);

                // Bottom handle

                r2.Y = r.Bottom - 2;

                g.FillRectangle(Brushes.White, r2);
                g.DrawRectangle(Pens.Black, r2);
            }
        }

        #endregion

        #endregion

        #region Mouse processing

        #region MouseMove processing

        /// <summary>
        /// Handles mouseDown processing
        /// </summary>
        /// <param name="objArg">MouseEventArgs</param>
        public override void InternalMouseMove(MouseEventArgs objArg)
        {
            this.HitArea = GetHitArea(objArg);

            base.InternalMouseMove(objArg);
        }

        /// <summary>
        /// Gets the HitArea from the current
        /// mouse position
        /// </summary>
        /// <param name="objArg"></param>
        /// <returns>eHitArea</returns>
        private eHitArea GetHitArea(MouseEventArgs objArg)
        {
            if (IsMutable == true)
            {
                Rectangle r = GetViewRect();

                Rectangle r2 =
                    new Rectangle(r.X + (r.Width / 2) - 5, r.Y, 10, 5);

                // See if we are in the top resize area

                if (r2.Contains(objArg.Location))
                    return (eHitArea.TopResize);

                // See if we are in the bottom resize area

                r2.Y = r.Bottom - 5;

                if (r2.Contains(objArg.Location))
                    return (eHitArea.BottomResize);

                // By default we are in the move area

                return (eHitArea.Move);
            }

            // No valid area to report

            return (eHitArea.None);
        }

        #endregion

        #endregion

    }
}
#endif

