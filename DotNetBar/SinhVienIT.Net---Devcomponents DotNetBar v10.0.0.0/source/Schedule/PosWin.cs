#if FRAMEWORK20
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Globalization;

namespace DevComponents.DotNetBar.Schedule
{
    public partial class PosWin : Form
    {
        #region Private variables

        private BaseView _View;                 // BaseView

        private string _PosText = "";           // Window content
        private int _PosHeight;                 // Calculated window height

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public PosWin(BaseView view)
        {
            _View = view;

            InitializeComponent();
        }

        #region CreateParams / show support

        // This code permits us to be able to create a
        // window with a drop shadow

        private const int CsDropshadow = 0x00020000;

        protected override CreateParams CreateParams
        {
            get 
            { 
                CreateParams parameters = base.CreateParams; 
                
                parameters.ClassStyle =
                    (parameters.ClassStyle | CsDropshadow);
            
                return (parameters); 
            }
        }

        protected override bool ShowWithoutActivation
        {
            get { return true; }
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets and sets the window content text
        /// </summary>
        public string PosText
        {
            get { return (_PosText); }

            set
            {
                if (_PosText != value)
                {
                    _PosText = value;

                    this.Refresh();
                }
            }
        }

        /// <summary>
        /// Gets the calculated window height
        /// </summary>
        public int PosHeight
        {
            get
            {
                if (_PosHeight <= 0)
                {
                    using (Graphics g = CreateGraphics())
                    {
                        Size sz = TextDrawing.MeasureString(g, "ABC", SystemFonts.CaptionFont, 0,
                            eTextFormat.Default | eTextFormat.HorizontalCenter | eTextFormat.VerticalCenter);

                        _PosHeight = sz.Height + 4;
                    }
                }

                return (_PosHeight);
            }
        }

        #endregion

        #region UpdateWin

        /// <summary>
        /// Updates the posWin
        /// </summary>
        /// <param name="viewRect">View rectangle</param>
        public void UpdateWin(Rectangle viewRect)
        {
            CalendarItem item = _View.SelectedItem;

            if (item != null)
            {
                // Calculate where the window should be positioned
                // and what time should be displayed

                Point pt = item.Bounds.Location;
                DateTime time = item.StartTime;

                pt.X += item.Bounds.Width + 4;

                if (pt.X > _View.ClientRect.Right)
                    pt.X = _View.ClientRect.Right + 4;

                if (_View.NClientData.TabOrientation == eTabOrientation.Horizontal)
                {
                    if (_View.SelectedItem.HitArea == CalendarItem.eHitArea.BottomResize)
                    {
                        pt.Y += (item.Bounds.Height - PosHeight);
                        time = item.EndTime;
                    }
                }
                else
                {
                    if (_View.SelectedItem.HitArea == CalendarItem.eHitArea.RightResize)
                        time = item.EndTime;
                }
                
                if (pt.Y < viewRect.Y)
                    pt.Y = viewRect.Y;

                // Convert the point to global coordinates
                // and set our window position accordingly

                Control c = (Control)_View.GetContainerControl(true);

                if (c != null)
                    pt = c.PointToScreen(pt);

                Location = pt;

                // Set the window text and show the window

                string fmt = "t";

                if (_View.ECalendarView == eCalendarView.TimeLine)
                {
                    switch (_View.CalendarView.TimeLinePeriod)
                    {
                        case eTimeLinePeriod.Years:
                            fmt = "";
                            PosText = time.Year.ToString();
                            break;

                        case eTimeLinePeriod.Days:
                            fmt = "g";
                            break;
                    }
                }

                if (fmt != "")
                {
                    PosText = _View.CalendarView.Is24HourFormat == true
                                  ? time.ToString(fmt, DateTimeFormatInfo.InvariantInfo)
                                  : time.ToString(fmt, null);
                }

                Show();
            }
        }

        #endregion

        #region Paint processing

        /// <summary>
        /// Paint processing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PosWin_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            const eTextFormat tf =
                eTextFormat.Default | eTextFormat.HorizontalCenter | eTextFormat.VerticalCenter;

            Size sz = TextDrawing.MeasureString(g, _PosText, SystemFonts.CaptionFont, 0, tf);

            sz.Width += 6;
            sz.Height += 4;

            this.Size = sz;

            int swidth = Screen.FromControl(this).Bounds.Width;

            if (Location.X + sz.Width > swidth)
                Location = new Point(swidth - sz.Width, Location.Y);

            Rectangle r = new Rectangle(0, 0, sz.Width - 1, sz.Height - 1);

            g.DrawRectangle(Pens.Black, r);

            TextDrawing.DrawString(g, _PosText, SystemFonts.CaptionFont, Color.Black, r, tf);
        }

        #endregion
    }
}
#endif
