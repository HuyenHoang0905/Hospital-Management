using System;
using System.Drawing;

namespace DevComponents.Instrumentation
{
    internal class TickPoint : ICloneable
    {
        #region Private variables

        private Point _Point;
        private float _Angle;
        private double _Interval;

        private GaugeTickMarkBase _TickMark;

        private bool _Visible = true;

        #endregion

        public TickPoint(GaugeTickMarkBase tickMark)
        {
            _TickMark = tickMark;
        }

        #region Public properties

        #region Angle

        public float Angle
        {
            get { return (_Angle); }
            set { _Angle = value; }
        }

        #endregion

        #region Point

        public Point Point
        {
            get { return (_Point); }
            set { _Point = value; }
        }

        #endregion

        #region Interval

        public double Interval
        {
            get { return (_Interval); }
            set { _Interval = value; }
        }

        #endregion

        #region TickMark

        public GaugeTickMarkBase TickMark
        {
            get { return (_TickMark); }
            set { _TickMark = value; }
        }

        #endregion

        #region Visible

        public bool Visible
        {
            get { return (_Visible); }
            set { _Visible = value; }
        }

        #endregion

        #endregion

        #region ICloneable Members

        public object Clone()
        {
            TickPoint tp = new TickPoint(_TickMark);

            tp.Point = _Point;
            tp.Angle = _Angle;
            tp.Interval = _Interval;
            tp.Visible = _Visible;

            return (tp);
        }

        #endregion
    }
}
