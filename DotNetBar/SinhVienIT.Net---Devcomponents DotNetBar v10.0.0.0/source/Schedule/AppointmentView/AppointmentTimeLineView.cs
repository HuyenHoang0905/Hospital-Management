#if FRAMEWORK20
using DevComponents.Schedule.Model;
using System.Drawing;

namespace DevComponents.DotNetBar.Schedule
{
    public class AppointmentTimeLineView : AppointmentHView
    {
        #region Private variables
        private TimeLineView _TimeLineView;
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="baseView"></param>
        /// <param name="appointment"></param>
        public AppointmentTimeLineView(BaseView baseView, Appointment appointment)
            : base(baseView, appointment)
        {
        }

        #region Public properties

        public TimeLineView TimeLineView
        {
            get { return (_TimeLineView); }

            set
            {
                if (_TimeLineView != value)
                {
                    _TimeLineView = value;

                    if (_TimeLineView != null)
                        SetViewEnds();
                }
            }
        }

        #endregion

        #region SetViewEnds

        /// <summary>
        /// Sets the view display end types
        /// </summary>
        protected override void SetViewEnds()
        {
            ViewEnds = eViewEnds.Complete;

            Rectangle r = DisplayRectangle;
            Rectangle p = ParentBounds;

            if (r.Left < p.Left)
                ViewEnds |= eViewEnds.PartialLeft;

            if (r.Right > p.Right)
                ViewEnds |= eViewEnds.PartialRight;
        }

        #endregion

        #region Protected properties

        protected override Rectangle ParentBounds
        {
            get
            {
                if (_TimeLineView != null)
                    return (_TimeLineView.ClientRect);

                return (base.ParentBounds);
            }
        }

        #endregion
    }
}
#endif

