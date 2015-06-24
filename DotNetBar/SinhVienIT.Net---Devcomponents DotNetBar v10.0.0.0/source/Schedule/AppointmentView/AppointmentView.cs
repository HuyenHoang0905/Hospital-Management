#if FRAMEWORK20
using System;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DevComponents.DotNetBar.Rendering;
using DevComponents.DotNetBar.TextMarkup;
using DevComponents.Schedule.Model;
using System.Drawing;

namespace DevComponents.DotNetBar.Schedule
{
    public class AppointmentView : CalendarItem
    {
        #region enums

        [Flags]
        public enum eViewEnds          // View display ends
        {
            Complete = 0,
            PartialLeft = 1,
            PartialRight = 2
        }

        #endregion

        #region Static variables

        static private AppointmentColor _appointmentColor = new AppointmentColor();

        #endregion

        #region Private variables

        private Font _Font;
        private BaseView _BaseView;
        private Appointment _Appointment;
        private bool _IsTextClipped;
        private int _BorderWidth;

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="baseView"></param>
        /// <param name="appointment"></param>
        public AppointmentView(BaseView baseView, Appointment appointment)
        {
            _BaseView = baseView;
            _Appointment = appointment;

            MouseUpNotification = true;

            StartTime = appointment.StartTime;
            EndTime = appointment.EndTime;
            ModelItem = appointment;

            IsRecurring = (appointment.IsRecurringInstance == true ||
                appointment.Recurrence != null);

            RootItem = (appointment.IsRecurringInstance == true) ?
                appointment.RootAppointment : appointment;

            StartTimeChanged += AppointmentView_StartTimeChanged;
            EndTimeChanged += AppointmentView_EndTimeChanged;
        }

        #region Public properties

        #region Appointment

        /// <summary>
        /// Gets and sets the view Appointment
        /// </summary>
        public Appointment Appointment
        {
            get { return (_Appointment); }
            set { _Appointment = value; }
        }

        #endregion

        #region AppointmentColor

        /// <summary>
        /// Gets and sets the appointment color
        /// </summary>
        public AppointmentColor AppointmentColor
        {
            get { return (_appointmentColor); }
            set { _appointmentColor = value; }
        }

        #endregion

        #region BorderWidth

        public int BorderWidth
        {
            get { return (_BorderWidth); }

            set
            {
                if (_BorderWidth != value)
                {
                    _BorderWidth = value;

                    Invalidate(_BaseView.CalendarView);
                }
            }
        }

        #endregion

        #region Font

        /// <summary>
        /// Gets and sets the view font
        /// </summary>
        public Font Font
        {
            get { return (_Font); }
            set { _Font = value; }
        }

        #endregion

        #region IsTextClipped

        /// <summary>
        /// Gets whether the Appointment display Text is clipped
        /// </summary>
        public bool IsTextClipped
        {
            get { return (_IsTextClipped); }
            internal set { _IsTextClipped = value; }
        }

        #endregion

        #endregion

        #region Internal properties

        #region BaseView

        /// <summary>
        /// BaseView
        /// </summary>
        internal BaseView BaseView
        {
            get { return (_BaseView); }
        }

        #endregion

        #region Image

        /// <summary>
        /// Image
        /// </summary>
        internal Image Image
        {
            get
            {
                if (string.IsNullOrEmpty(Appointment.ImageKey) == true)
                    return (null);

                ImageList images;

                switch (_BaseView.CalendarView.ImageSize)
                {
                    case eBarImageSize.Medium:
                        images = _BaseView.CalendarView.ImagesMedium;
                        break;

                    case eBarImageSize.Large:
                        images = _BaseView.CalendarView.ImagesLarge;
                        break;

                    default:
                        images = _BaseView.CalendarView.Images;
                        break;
                }

                return (images != null ?
                    images.Images[Appointment.ImageKey] : null);
            }
        }

        #endregion

        #endregion

        #region Start/End TimeChanged event handling

        /// <summary>
        /// Handles StartTime value changes
        /// </summary>
        /// <param name="sender">CalendarItem</param>
        /// <param name="e">EventArgs</param>
        protected virtual void AppointmentView_StartTimeChanged(object sender, EventArgs e)
        {
            _Appointment.StartTime = StartTime;
        }

        /// <summary>
        /// Handles EndTime value changes
        /// </summary>
        /// <param name="sender">CalendarItem</param>
        /// <param name="e">EventArgs</param>
        protected virtual void AppointmentView_EndTimeChanged(object sender, EventArgs e)
        {
            _Appointment.EndTime = EndTime;
        }

        #endregion

        #region DisplayTemplateText

        /// <summary>
        /// DisplayTemplateText
        /// </summary>
        /// <param name="e"></param>
        /// <param name="r"></param>
        /// <returns>true is displayed</returns>
        internal bool DisplayTemplateText(ItemPaintArgs e, Rectangle r)
        {
            if (_BaseView.CalendarView.EnableMarkup == true)
            {
                if (string.IsNullOrEmpty(Appointment.DisplayTemplate) == false)
                {
                    Text = GetDisplayTemplateText(r);
                }
                else
                {
                    string text = Appointment.Subject;

                    if (TextMarkupBody != null)
                    {
                        if (Appointment.IsMultiDayOrAllDayEvent == false)
                            text += "<br/>" + Appointment.Description;
                    }

                    Text = text;
                }

                if (TextMarkupBody != null)
                {
                    Graphics g = e.Graphics;

                    MarkupDrawContext d =
                        new MarkupDrawContext(g, Font ?? e.Font, TextColor, true);

                    Size size = new Size(5000, 5000);

                    TextMarkupBody.InvalidateElementsSize();
                    TextMarkupBody.Measure(size, d);

                    d.RightToLeft = false;

                    r.Y += 3;
                    r.Height -= 3;

                    TextMarkupBody.Arrange(new Rectangle(r.Location, TextMarkupBody.Bounds.Size), d);

                    _IsTextClipped = (TextMarkupBody.Bounds.Size.Width > r.Width ||
                        TextMarkupBody.Bounds.Size.Height > r.Height);

                    Region oldClip = g.Clip;
                    Rectangle clipRect = r;

                    g.SetClip(clipRect, CombineMode.Intersect);

                    TextMarkupBody.Render(d);

                    g.Clip = oldClip;

                    return (true);
                }
            }

            return (false);
        }

        #endregion

        #region GetDisplayTemplateText

        /// <summary>
        /// GetDisplayTemplateText
        /// </summary>
        /// <param name="r"></param>
        /// <returns>Templatized text</returns>
        private string GetDisplayTemplateText(Rectangle r)
        {
            string s = Appointment.DisplayTemplate;

            if (String.IsNullOrEmpty(s) == true)
                return (s);

            Regex reg = new Regex("\\[\\w+\\]");
            MatchCollection mc = reg.Matches(s);

            int index = 0;
            StringBuilder sb = new StringBuilder();

            sb.Append("<p width=\"" + r.Width.ToString() + "\">");

            for (int i = 0; i < mc.Count; i++)
            {
                Match ma = mc[i];

                if (ma.Index > index)
                    sb.Append(s.Substring(index, ma.Index - index));

                switch (ma.Value)
                {
                    case "[StartTime]":
                        sb.Append(Appointment.StartTime.ToString("t",
                            _BaseView.CalendarView.Is24HourFormat ? DateTimeFormatInfo.InvariantInfo : null));
                        break;

                    case "[StartDate]":
                        sb.Append(Appointment.StartTime.ToShortDateString());
                        break;

                    case "[EndTime]":
                        sb.Append(Appointment.EndTime.ToString("t",
                            _BaseView.CalendarView.Is24HourFormat ? DateTimeFormatInfo.InvariantInfo : null));
                        break;

                    case "[EndDate]":
                        sb.Append(Appointment.EndTime.ToShortDateString());
                        break;

                    case "[Subject]":
                        sb.Append(Appointment.Subject);
                        break;

                    case "[Description]":
                        sb.Append(Appointment.Description);
                        break;

                    case "[Id]":
                        sb.Append(Appointment.Id);
                        break;

                    case "[Tag]":
                        sb.Append(Appointment.Tag.ToString());
                        break;

                    default:
                        sb.Append(_BaseView.CalendarView.DoGetDisplayTemplateText(this, ma.Value, ma.Value));
                        break;
                }

                index = ma.Index + ma.Length;
            }

            if (s.Length > index)
                sb.Append(s.Substring(index));

            sb.Append("</p>");

            return (sb.ToString());
        }

        #endregion

        #region Paint

        /// <summary>
        /// Paint processing
        /// </summary>
        /// <param name="e">ItemPaintArgs</param>
        public override void Paint(ItemPaintArgs e)
        {
        }

        #region GetItemBounds

        /// <summary>
        /// Gets the item text and image bounds
        /// </summary>
        /// <param name="r"></param>
        /// <param name="rText"></param>
        /// <param name="image"></param>
        /// <returns></returns>
        protected virtual Rectangle GetItemBounds(Rectangle r, ref Rectangle rText, Image image)
        {
            Rectangle ri = r;

            if (Image != null)
            {
                const int vpad = 2;
                const int hpad = 3;

                ri.Inflate(0, -vpad);

                switch (Appointment.ImageAlign)
                {
                    case eImageContentAlignment.TopLeft:
                        rText.X += (image.Size.Width + hpad);
                        rText.Width -= (image.Size.Width + hpad);
                        break;

                    case eImageContentAlignment.TopRight:
                        ri.X = rText.Right - image.Size.Width;
                        rText.Width -= image.Size.Width;
                        break;

                    case eImageContentAlignment.TopCenter:
                        ri.X += (ri.Width - image.Size.Width) / 2;
                        rText.Y += (image.Size.Height + hpad);
                        rText.Height -= (image.Size.Height + hpad);
                        break;

                    case eImageContentAlignment.MiddleLeft:
                        ri.Y += (ri.Height - image.Size.Height) / 2;
                        rText.X += (image.Size.Width + hpad);
                        rText.Width -= (image.Size.Width + hpad);
                        break;

                    case eImageContentAlignment.MiddleRight:
                        ri.X = ri.Right - image.Size.Width;
                        ri.Y += (ri.Height - image.Size.Height) / 2;
                        rText.Width -= (image.Size.Width + hpad);
                        break;

                    case eImageContentAlignment.MiddleCenter:
                        ri.X = ri.X + (ri.Width - image.Size.Width) / 2;
                        ri.Y += (ri.Height - image.Size.Height) / 2;
                        break;

                    case eImageContentAlignment.BottomLeft:
                        ri.Y = ri.Bottom - image.Size.Height;
                        rText.X += (image.Size.Width + hpad);
                        rText.Width -= (image.Size.Width + hpad);
                        break;

                    case eImageContentAlignment.BottomRight:
                        ri.X = ri.Right - image.Size.Width;
                        ri.Y = ri.Bottom - image.Size.Height;
                        rText.Width -= (image.Size.Width - hpad);
                        break;

                    default:
                        ri.X = ri.X + (ri.Width - image.Size.Width) / 2;
                        ri.Y = ri.Bottom - image.Size.Height;
                        break;
                }

                ri.Size = image.Size;

                if (ri.X < r.X)
                    ri.X = r.X;

                if (ri.Y < r.Y + vpad)
                    ri.Y = r.Y + vpad;
            }

            return (ri);
        }

        #endregion

        #region DrawContentImage

        /// <summary>
        /// DrawContentImage
        /// </summary>
        /// <param name="e"></param>
        /// <param name="r"></param>
        /// <param name="rImage"></param>
        /// <param name="image"></param>
        protected virtual void DrawContentImage(
            ItemPaintArgs e, Rectangle r, Rectangle rImage, Image image)
        {
            if (image != null)
            {
                Graphics g = e.Graphics;

                rImage.Intersect(r);
                g.DrawImageUnscaledAndClipped(image, rImage);
            }
        }

        #endregion

        #region DrawTimeMarker

        #region DrawTimeMarker

        /// <summary>
        /// Initiates the drawing of the appointment Time Marker
        /// </summary>
        /// <param name="g">Graphics</param>
        /// <param name="r">Appointment rectangle</param>
        /// <param name="corner">Corner radius</param>
        protected virtual void DrawTimeMarker(Graphics g, Rectangle r, int corner)
        {
            if (Appointment.TimeMarkedAs != null)
            {
                if (Appointment.TimeMarkedAs.Equals(Appointment.TimerMarkerTentative))
                {
                    using (HatchBrush br =
                        new HatchBrush(HatchStyle.WideUpwardDiagonal, BorderColor, Color.White))
                    {
                        g.RenderingOrigin = new Point(r.X, r.Y);

                        RenderMarker(g, br, r, corner);
                    }
                }
                else
                {
                    using (Brush br = TimeMarkerBrush(r))
                    {
                        if (br != null)
                            RenderMarker(g, br, r, corner);
                    }
                }
            }
        }

        #endregion

        #region RenderMarker

        /// <summary>
        /// RenderMarker
        /// </summary>
        /// <param name="g"></param>
        /// <param name="br">Brush</param>
        /// <param name="r">Rectangle</param>
        /// <param name="corner">Corner</param>
        private void RenderMarker(Graphics g, Brush br, Rectangle r, int corner)
        {
            using (Pen pen = BorderPen)
            {
                if (corner > 0)
                {
                    using (GraphicsPath path = GetLeftRoundedRectanglePath(r, corner))
                    {
                        g.FillPath(br, path);
                        g.DrawPath(pen, path);
                    }
                }
                else
                {
                    int x = Math.Min(5, r.Height / 2);
                    x = Math.Min(x, r.Width / 2);

                    r.Width = x;

                    g.FillRectangle(br, r);
                    g.DrawRectangle(pen, r);
                }
            }
        }

        #endregion

        #region GetLeftRoundedRectanglePath

        /// <summary>
        /// Creates a left rounded rectangle path to be
        /// used for the Time Marker
        /// </summary>
        /// <param name="r">Appointment rectangle</param>
        /// <param name="corner">Corner radius</param>
        /// <returns>Graphics path</returns>
        private GraphicsPath GetLeftRoundedRectanglePath(Rectangle r, int corner)
        {
            GraphicsPath path = new GraphicsPath();

            ElementStyleDisplay.AddCornerArc(path, r, corner, eCornerArc.TopLeft);
            path.AddLine(r.X + corner, r.Y, r.X + corner, r.Bottom);
            ElementStyleDisplay.AddCornerArc(path, r, corner, eCornerArc.BottomLeft);

            return (path);
        }

        #endregion

        #endregion

        #endregion

        #region Color support

        #region BackBrush

        /// <summary>
        /// Gets the appointment BackGround brush
        /// </summary>
        /// <param name="r">Bounding rectangle</param>
        /// <returns></returns>
        protected Brush BackBrush(Rectangle r)
        {
            string category = _Appointment.CategoryColor;

            if (category != null)
            {
                // Check to see if we have any user defined
                // AppointmentCategoryColors

                if (_BaseView.CalendarView.HasCategoryColors == true)
                {
                    AppointmentCategoryColor acc =
                        _BaseView.CalendarView.CategoryColors[category];

                    if (acc != null)
                        return (_appointmentColor.BrushPart(acc.BackColor, r));
                }

                // Just use the default color set

                if (category.StartsWith("#") == true)
                    return (new SolidBrush(ColorFactory.Empty.GetColor(category.Substring(1))));

                if (category.Equals(Appointment.CategoryBlue))
                    return (_appointmentColor.BrushPart((int)eAppointmentPart.BlueBackground, r));

                if (category.Equals(Appointment.CategoryGreen))
                    return (_appointmentColor.BrushPart((int)eAppointmentPart.GreenBackground, r));

                if (category.Equals(Appointment.CategoryOrange))
                    return (_appointmentColor.BrushPart((int)eAppointmentPart.OrangeBackground, r));

                if (category.Equals(Appointment.CategoryPurple))
                    return (_appointmentColor.BrushPart((int)eAppointmentPart.PurpleBackground, r));

                if (category.Equals(Appointment.CategoryRed))
                    return (_appointmentColor.BrushPart((int)eAppointmentPart.RedBackground, r));

                if (category.Equals(Appointment.CategoryYellow))
                    return (_appointmentColor.BrushPart((int)eAppointmentPart.YellowBackground, r));
            }

            return (_appointmentColor.BrushPart((int)eAppointmentPart.DefaultBackground, r));
        }

        #endregion

        #region BorderColor

        /// <summary>
        /// Gets the border color for the Category
        /// </summary>
        protected Color BorderColor
        {
            get
            {
                string category = _Appointment.CategoryColor;

                if (category != null)
                {
                    // Check to see if we have any user defined
                    // AppointmentCategoryColors

                    if (_BaseView.CalendarView.HasCategoryColors == true)
                    {
                        AppointmentCategoryColor acc =
                            _BaseView.CalendarView.CategoryColors[category];

                        if (acc != null)
                            return (acc.BorderColor);
                    }

                    // Just use the default color set

                    if (category.Equals(Appointment.CategoryBlue))
                        return (_appointmentColor.GetColor((int)eAppointmentPart.BlueBorder));

                    if (category.Equals(Appointment.CategoryGreen))
                        return (_appointmentColor.GetColor((int)eAppointmentPart.GreenBorder));

                    if (category.Equals(Appointment.CategoryOrange))
                        return (_appointmentColor.GetColor((int)eAppointmentPart.OrangeBorder));

                    if (category.Equals(Appointment.CategoryPurple))
                        return (_appointmentColor.GetColor((int)eAppointmentPart.PurpleBorder));

                    if (category.Equals(Appointment.CategoryRed))
                        return (_appointmentColor.GetColor((int)eAppointmentPart.RedBorder));

                    if (category.Equals(Appointment.CategoryYellow))
                        return (_appointmentColor.GetColor((int)eAppointmentPart.YellowBorder));
                }

                return (_appointmentColor.GetColor((int)eAppointmentPart.DefaultBorder));
            }
        }

        #endregion

        #region BorderPen

        /// <summary>
        /// Gets the border pen
        /// </summary>
        protected Pen BorderPen
        {
            get
            {
                int n = (_BorderWidth > 0) ?
                    _BorderWidth : _BaseView.CalendarView.AppointmentBorderWidth;

                return (new Pen(BorderColor, n));
            }
        }

        #endregion

        #region SelectedBorderPen

        /// <summary>
        /// Gets the selected border pen
        /// </summary>
        protected Pen SelectedBorderPen
        {
            get
            {
                int n = (_BorderWidth > 0) ?
                    _BorderWidth : _BaseView.CalendarView.AppointmentBorderWidth;

                return (new Pen(Color.Black, n + 1));
            }
        }

        #endregion

        #region TextColor

        /// <summary>
        /// Gets the Text color for the Category
        /// </summary>
        protected Color TextColor
        {
            get
            {
                string category = _Appointment.CategoryColor;

                // Check to see if we have any user defined
                // AppointmentCategoryColors

                if (category != null)
                {
                    if (_BaseView.CalendarView.HasCategoryColors == true)
                    {
                        AppointmentCategoryColor acc =
                            _BaseView.CalendarView.CategoryColors[category];

                        if (acc != null)
                            return (acc.TextColor);
                    }
                }

                return (Color.Black);
            }
        }

        #endregion

        #region TimeMarkerBrush

        /// <summary>
        /// Gets the appointment TimeMarkerBrush
        /// </summary>
        /// <param name="r">Bounding rectangle</param>
        /// <returns></returns>
        protected Brush TimeMarkerBrush(Rectangle r)
        {
            string timeMarkedAs = _Appointment.TimeMarkedAs;

            if (timeMarkedAs != null)
            {
                if (timeMarkedAs.StartsWith("#") == true)
                    return (new SolidBrush(ColorFactory.Empty.GetColor(timeMarkedAs.Substring(1))));

                if (timeMarkedAs.Equals(Appointment.TimerMarkerFree))
                    return (_appointmentColor.BrushPart((int)eAppointmentPart.FreeTimeMarker, r));

                if (timeMarkedAs.Equals(Appointment.TimerMarkerBusy))
                    return (_appointmentColor.BrushPart((int)eAppointmentPart.BusyTimeMarker, r));

                if (timeMarkedAs.Equals(Appointment.TimerMarkerOutOfOffice))
                    return (_appointmentColor.BrushPart((int)eAppointmentPart.OutOfOfficeTimeMarker, r));
            }

            return (null);
        }

        #endregion

        #endregion

    }
}
#endif

