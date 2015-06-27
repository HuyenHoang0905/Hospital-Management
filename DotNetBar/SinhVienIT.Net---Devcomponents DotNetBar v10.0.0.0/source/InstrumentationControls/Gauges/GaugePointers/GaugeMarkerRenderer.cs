using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DevComponents.Instrumentation
{
    internal class GaugeMarkerRenderer : GaugePointerRenderer, IDisposable
    {
        #region Private variables

        private int _LastWidth;
        private int _LastLength;
        private GaugeMarkerStyle _LastMarkerStyle;

        private Rectangle _Bounds;

        private GaugeMarker _GaugeMarker;

        #endregion

        internal GaugeMarkerRenderer(GaugePointer gaugePointer)
            : base(gaugePointer)
        {
            _GaugeMarker = new GaugeMarker();
        }

        #region Internal properties

        #region Bounds

        internal Rectangle Bounds
        {
            get { return (_Bounds); }
            set { _Bounds = value; }
        }

        #endregion

        #endregion

        #region RecalcLayout

        public override void RecalcLayout()
        {
            base.RecalcLayout();

            if (Scale is GaugeCircularScale)
                CalcCircularMarkerPoint(Scale as GaugeCircularScale);

            else if (Scale is GaugeLinearScale)
                CalcLinearMarkerPoint(Scale as GaugeLinearScale);

            if ((Width != _LastWidth || Length != _LastLength) ||
                (GaugePointer.MarkerStyle != _LastMarkerStyle))
            {
                _GaugeMarker.Clear();

                _LastWidth = Width;
                _LastLength = Length;
                _LastMarkerStyle = GaugePointer.MarkerStyle;
            }
        }

        #region CalcCircularMarkerPoint

        private void CalcCircularMarkerPoint(GaugeCircularScale scale)
        {
            int radius = Radius;

            switch (GaugePointer.Placement)
            {
                case DisplayPlacement.Near:
                    radius -= Length;
                    break;

                case DisplayPlacement.Center:
                    radius += (Length / 2);
                    break;

                case DisplayPlacement.Far:
                    radius += Length;
                    break;
            }

            Dpt = scale.SweepAngle / scale.Spread;

            double marker = GetInterval(Value) - scale.MinValue;

            IntervalAngle = (float)(scale.Reversed
                ? scale.StartAngle + scale.SweepAngle - (marker * Dpt)
                : scale.StartAngle + (marker * Dpt));

            IntervalPoint = scale.GetPoint(radius, IntervalAngle);
        }

        #endregion

        #region CalcLinearMarkerPoint

        private void CalcLinearMarkerPoint(GaugeLinearScale scale)
        {
            if (scale.Orientation == Orientation.Horizontal)
                CalcHorizontalMarkerPoint(scale);
            else
                CalcVerticalMarkerPoint(scale);
        }

        #region  CalcHorizontalMarkerPoint

        private void CalcHorizontalMarkerPoint(GaugeLinearScale scale)
        {
            int offset = GaugePointer.Offset;

            _Bounds = new Rectangle(0, 0, Width, Length);

            int y = scale.ScaleBounds.Y + scale.ScaleBounds.Height / 2 + offset;

            switch (GaugePointer.Placement)
            {
                case DisplayPlacement.Near:
                    y -= Length;
                    break;

                case DisplayPlacement.Center:
                    y -= (Length / 2);
                    break;

                case DisplayPlacement.Far:
                    break;
            }

            Dpt = scale.ScaleBounds.Width / scale.Spread;

            double marker = GetInterval(Value) - scale.MinValue;

            int n = _Bounds.Width / 2;

            int x = (scale.Reversed == true)
                ? scale.ScaleBounds.Right - (int)(marker * Dpt) - n
                : scale.ScaleBounds.X + (int)(marker * Dpt) - n;

            _Bounds.Location = new Point(x, y);

            IntervalPoint = new Point(_Bounds.X + n, _Bounds.Y + _Bounds.Height / 2);
        }

        #endregion

        #region CalcVerticalMarkerPoint

        private void CalcVerticalMarkerPoint(GaugeLinearScale scale)
        {
            int offset = GaugePointer.Offset;

            _Bounds = new Rectangle(0, 0, Width, Length);

            int x = scale.ScaleBounds.X + scale.ScaleBounds.Width / 2 + offset;

            switch (GaugePointer.Placement)
            {
                case DisplayPlacement.Near:
                    x -= Width;
                    break;

                case DisplayPlacement.Center:
                    x -= (Width / 2);
                    break;

                case DisplayPlacement.Far:
                    break;
            }

            Dpt = scale.ScaleBounds.Height / scale.Spread;

            double marker = GetInterval(Value) - scale.MinValue;

            int n = _Bounds.Height / 2;

            int y = (scale.Reversed == true)
                ? scale.ScaleBounds.Top + (int)(marker * Dpt) - n
                : scale.ScaleBounds.Bottom - (int)(marker * Dpt) - n;

            _Bounds.Location = new Point(x, y);

            IntervalPoint = new Point(_Bounds.X + _Bounds.Width / 2, _Bounds.Y + n);
        }

        #endregion

        #endregion

        #endregion

        #region RenderCircular

        public override void RenderCircular(PaintEventArgs e)
        {
            if (IntervalPoint.IsEmpty == false &&
                (Width > 0 && Length > 0 && Radius > 0))
            {
                Graphics g = e.Graphics;

                Rectangle r = new Rectangle(0, 0, Width, Length);
                r.X -= Width / 2;

                float angle = IntervalAngle + 90;

                if (GaugePointer.Placement == DisplayPlacement.Near)
                    angle += 180;

                g.TranslateTransform(IntervalPoint.X, IntervalPoint.Y);
                g.RotateTransform(angle % 360);

                if (GaugePointer.Image != null)
                {
                    g.DrawImage(GaugePointer.Image, r);
                }
                else
                {
                    GraphicsPath path = GetMarkerPath(r);

                    if (path != null)
                    {
                        GradientFillColor fillColor =
                            GetPointerFillColor(GaugePointer.Value);

                        _GaugeMarker.FillMarkerPath(g, path, r, MarkerStyle, fillColor);
                    }
                }

                g.ResetTransform();
            }
        }

        #endregion

        #region RenderLinear

        public override void RenderLinear(PaintEventArgs e)
        {
            if (_Bounds.Width > 0 && _Bounds.Height > 0)
            {
                Graphics g = e.Graphics;

                Rectangle r = new Rectangle(0, 0, Width, Length);
                r.X -= Width / 2;
                r.Y -= Length / 2;

                g.TranslateTransform(IntervalPoint.X, IntervalPoint.Y);

                if (((GaugeLinearScale)Scale).Orientation == Orientation.Horizontal)
                {
                    if (GaugePointer.Placement == DisplayPlacement.Far)
                        g.RotateTransform(180);
                }
                else
                {
                    int angle = (GaugePointer.Placement == DisplayPlacement.Far) ? 90 : -90;

                    g.RotateTransform(angle);
                }

                if (GaugePointer.Image != null)
                {
                    g.DrawImage(GaugePointer.Image, r);
                }
                else
                {
                    GraphicsPath path = GetMarkerPath(r);

                    if (path != null)
                    {
                        GradientFillColor fillColor =
                            GetPointerFillColor(GaugePointer.Value);

                        _GaugeMarker.FillMarkerPath(g, path, r, MarkerStyle, fillColor);
                    }
               }

                g.ResetTransform();
            }
        }

        #endregion

        #region GetPointerPath

        public override GraphicsPath GetPointerPath()
        {
            if (PointerPath == null)
            {
                if (MarkerStyle != GaugeMarkerStyle.None)
                {
                    if (Width > 0 && Length > 0)
                    {
                        if (Scale is GaugeCircularScale)
                            GetCPointerPath();
                        else
                            GetLPointerPath();
                    }
                }
            }

            return (PointerPath);
        }

        #region GetCPointerPath

        private void GetCPointerPath()
        {
            Rectangle r = new Rectangle(IntervalPoint.X, IntervalPoint.Y, Width, Length);
            r.X -= Width / 2;

            PointerPath = GetMarkerPath(r);

            if (PointerPath != null)
            {
                float angle = IntervalAngle + 90;

                if (GaugePointer.Placement == DisplayPlacement.Near)
                    angle += 180;

                Matrix matrix = new Matrix();
                matrix.RotateAt(angle % 360, IntervalPoint);

                PointerPath.Transform(matrix);
            }
        }

        #endregion

        #region GetLPointerPath

        private void GetLPointerPath()
        {
            if (((GaugeLinearScale)Scale).Orientation == Orientation.Horizontal)
                GetLhPointerPath();
            else
                GetLvPointerPath();
        }

        #region GetLhPointerPath

        private void GetLhPointerPath()
        {
            Rectangle r = new Rectangle(IntervalPoint.X, IntervalPoint.Y, Width, Length);

            r.X -= Width / 2;
            r.Y -= Length / 2;

            PointerPath = GetMarkerPath(r);

            if (GaugePointer.Placement == DisplayPlacement.Far)
            {
                Matrix matrix = new Matrix();
                matrix.RotateAt(180, IntervalPoint);

                PointerPath.Transform(matrix);
            }
        }

        #endregion

        #region GetLvPointerPath

        private void GetLvPointerPath()
        {
            Rectangle r = new Rectangle(IntervalPoint.X, IntervalPoint.Y, Width, Length);

            r.X -= Width / 2;
            r.Y -= Length / 2;

            PointerPath = GetMarkerPath(r);

            int angle = (GaugePointer.Placement == DisplayPlacement.Far) ? 90 : -90;

            Matrix matrix = new Matrix();
            matrix.RotateAt(angle, IntervalPoint);

            PointerPath.Transform(matrix);
        }

        #endregion

        #endregion

        #region GetMarkerPath

        private GraphicsPath GetMarkerPath(Rectangle r)
        {
            GraphicsPath path = GaugePointer.Scale.GaugeControl.OnGetPointerPath(GaugePointer, r);

            if (path == null)
                path = _GaugeMarker.GetMarkerPath(MarkerStyle, r);
            else
                PointerPath = null;

            return (path);
        }

        #endregion

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            PointerPath = null;

            _GaugeMarker.Dispose();
        }

        #endregion
    }
}
