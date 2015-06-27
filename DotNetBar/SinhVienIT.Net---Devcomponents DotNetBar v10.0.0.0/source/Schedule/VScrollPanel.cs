#if FRAMEWORK20
using System;
using System.Drawing;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Schedule
{
    public class VScrollPanel : BaseItem
    {
        #region Events
        public event EventHandler<EventArgs> ScrollBarChanged;
        #endregion

        #region Variables
        protected CalendarView CalendarView;    // CalendarView
        private VScrollBarAdv _ScrollBar;       // Scroll bar
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="calendarView"></param>
        public VScrollPanel(CalendarView calendarView)
        {
            CalendarView = calendarView;

            SetUpScrollBar();
        }

        #region Public properties

        /// <summary>
        /// Gets and sets the panel Bounds
        /// </summary>
        public override Rectangle Bounds
        {
            get { return (base.Bounds); }

            set
            {
                base.Bounds = value;

                UpdateScrollBar();
            }
        } 

        /// <summary>
        /// Gets and sets the control visibility
        /// </summary>
        public override bool Visible
        {
            get { return (base.Visible); }

            set
            {
                if (base.Visible != value)
                {
                    base.Visible = value;

                    ScrollBar.Visible = value;

                    if (value == true)
                        ScrollBar.BringToFront();
                }
            }
        }

        /// <summary>
        /// Gets the scrollBar
        /// </summary>
        public VScrollBarAdv ScrollBar
        {
            get { return (_ScrollBar); }
        }

        #endregion

        #region Private properties

        /// <summary>
        /// Gets the scrollBar SmallChange value
        /// </summary>
        protected virtual int ScrollPanelSmallChange
        {
            get { return (1); }
        }

        /// <summary>
        /// Gets the scrollBar Maximum value
        /// </summary>
        protected virtual int ScrollPanelMaximum
        {
            get { return (100); }
        }

        #endregion

        #region SetupScrollBar

        /// <summary>
        /// Performs scrollBar setup
        /// </summary>
        private void SetUpScrollBar()
        {
            _ScrollBar = new VScrollBarAdv();
            _ScrollBar.Width = SystemInformation.VerticalScrollBarWidth;

            Control c = (Control)CalendarView.CalendarPanel.GetContainerControl(true);

            if (c != null)
                c.Controls.Add(_ScrollBar);

            _ScrollBar.ValueChanged += ValueChanged;

            UpdateScrollBar();
        }

        #endregion

        #region UpdateScrollBar

        /// <summary>
        /// Updates our scrollbar
        /// </summary>
        internal void UpdateScrollBar()
        {
            _ScrollBar.Location = Bounds.Location;
            _ScrollBar.Height = Bounds.Height;

            _ScrollBar.LargeChange = Bounds.Height;
            _ScrollBar.SmallChange = 1;
            _ScrollBar.SmallChange = ScrollPanelSmallChange;
            _ScrollBar.Maximum = ScrollPanelMaximum;

            int n = _ScrollBar.Maximum - _ScrollBar.LargeChange + 1;

            if (n < 0)
            {
                _ScrollBar.Maximum = Bounds.Height;
                _ScrollBar.Value = 0;

                _ScrollBar.Enabled = false;
            }
            else
            {
                _ScrollBar.Enabled = true;

                if (_ScrollBar.Value > n)
                    _ScrollBar.Value = n;
                else
                    Refresh();
            }

            OnScrollBarUpdate();
        }

        #endregion

        #region DisableScrollBar

        /// <summary>
        /// Disables the scrollbar
        /// </summary>
        internal void DisableScrollBar()
        {
            _ScrollBar.Location = Bounds.Location;
            _ScrollBar.Height = Bounds.Height;

            _ScrollBar.LargeChange = Bounds.Height;
            _ScrollBar.SmallChange = 1;
            _ScrollBar.SmallChange = ScrollPanelSmallChange;
            _ScrollBar.Maximum = Bounds.Height;

            _ScrollBar.Value = 0;
            _ScrollBar.Enabled = false;

            OnScrollBarUpdate();
        }

        #endregion

        #region ValueChanged

        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        void ValueChanged(object sender, EventArgs e)
        {
            OnScrollBarUpdate();
        }

        /// <summary>
        /// Passes the scroll onto others
        /// </summary>
        private void OnScrollBarUpdate()
        {
            if (ScrollBarChanged != null)
                ScrollBarChanged(this, EventArgs.Empty);
        }

        #endregion

        #region IDisposable Members

        protected override void Dispose(bool disposing)
        {
            if (_ScrollBar != null)
            {
                _ScrollBar.Dispose();
                //_ScrollBar = null;
            }

            base.Dispose(disposing);
        }

        #endregion

        #region Paint

        public override void  Paint(ItemPaintArgs p)
        {
        }

        #endregion

        #region Copy

        public override BaseItem Copy()
        {
            return (null);
        }

        #endregion
    }
}
#endif
