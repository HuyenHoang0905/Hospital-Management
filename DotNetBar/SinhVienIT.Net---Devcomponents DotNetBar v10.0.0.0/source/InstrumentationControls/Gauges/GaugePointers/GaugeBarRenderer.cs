using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DevComponents.Instrumentation
{
    internal class GaugeBarRenderer : GaugePointerRenderer
    {
        #region Private variables

        private float _StartAngle;
        private float _SweepAngle;
        private float _RoundAngle;

        private double _Marker;
        private double _Origin;

        private Rectangle _Bounds;

        #endregion

        internal GaugeBarRenderer(GaugePointer gaugePointer)
            : base(gaugePointer)
        {
        }

        #region Internal properties

        #region Bounds

        internal Rectangle Bounds
        {
            get { return (_Bounds); }
            set { _Bounds = value; }
        }

        #endregion

        #region Marker

        internal double Marker
        {
            get { return (_Marker); }
        }

        #endregion

        #region Origin

        internal double Origin
        {
            get { return (_Origin); }
        }

        #endregion

        #region RoundAngle

        internal float RoundAngle
        {
            get { return (_RoundAngle); }
            set { _RoundAngle = value; }
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
        }

        #region CalcCircularMarkerPoint

        private void CalcCircularMarkerPoint(GaugeCircularScale scale)
        {
            int radius = Radius;

            switch (GaugePointer.Placement)
            {
                case DisplayPlacement.Near:
                    radius -= (Width / 2);
                    break;

                case DisplayPlacement.Far:
                    radius += (Width / 2);
                    break;
            }

            _Bounds.Size = new Size(radius * 2, radius * 2);
            _Bounds.Location = new Point(scale.Center.X - radius, scale.Center.Y - radius);

            Dpt = scale.SweepAngle / scale.Spread;

            _Origin = GetOriginInterval() - scale.MinValue;
            _Marker = GetInterval(Value) - scale.MinValue;

            if (scale.Reversed == true)
            {
                IntervalAngle = (float)(scale.StartAngle + scale.SweepAngle  - (_Marker * Dpt));
                _StartAngle = (float)(scale.StartAngle + scale.SweepAngle - (_Origin * Dpt));
            }
            else
            {
                IntervalAngle = (float)(scale.StartAngle + _Marker * Dpt);
                _StartAngle = (float)(scale.StartAngle + _Origin * Dpt);
            }

            IntervalPoint = scale.GetPoint(radius, IntervalAngle);
            _SweepAngle = IntervalAngle - _StartAngle;

            if (GaugePointer.BarStyle != BarPointerStyle.Square)
            {
                _RoundAngle = 0;

                if (_SweepAngle != 0)
                {
                    float x = (float)((360 * Width / 2) / (radius * 2 * Math.PI));

                    _RoundAngle = (_SweepAngle > 0)
                        ? Math.Min(x, _SweepAngle) : Math.Max(-x, _SweepAngle);

                    _SweepAngle -= _RoundAngle;
                }
            }
        }

        #endregion

        #region CalcLinearMarkerPoint

        protected virtual void CalcLinearMarkerPoint(GaugeLinearScale scale)
        {
            if (scale.Orientation == Orientation.Horizontal)
                CalcHorizontalMarkerPoint(scale);
            else
                CalcVerticalMarkerPoint(scale);
        }

        #region  CalcHorizontalMarkerPoint

        protected virtual void CalcHorizontalMarkerPoint(GaugeLinearScale scale)
        {
            int offset = GaugePointer.Offset;

            int y = scale.ScaleBounds.Y + scale.ScaleBounds.Height / 2 + offset;

            switch (GaugePointer.Placement)
            {
                case DisplayPlacement.Near:
                    y -= Width;
                    break;

                case DisplayPlacement.Center:
                    y -= (Width / 2);
                    break;

                case DisplayPlacement.Far:
                    break;
            }

            Dpt = scale.ScaleBounds.Width / scale.Spread;

            _Origin = GetOriginInterval() - scale.MinValue;
            _Marker = GetInterval(Value) - scale.MinValue;

            CalcHorizontalMarkerBounds(scale, y);
        }

        #region CalcHorizontalMarkerBounds

        protected virtual void CalcHorizontalMarkerBounds(GaugeLinearScale scale, int y)
        {
            double origin = _Origin;
            double marker = _Marker;

            _RoundAngle = 0;

            if (marker < origin)
            {
                _RoundAngle += 180;

                SwapDoubles(ref marker, ref origin);
            }

            int len = (int)((marker - origin) * Dpt);

            int x = (scale.Reversed == true)
                ? scale.ScaleBounds.Right - (int)(origin * Dpt) - len
                : scale.ScaleBounds.X + (int)(origin * Dpt);

            _Bounds = new Rectangle(0, 0, len, Width);
            _Bounds.Location = new Point(x, y);
        }

        #endregion

        #endregion

        #region CalcVerticalMarkerPoint

        protected virtual void CalcVerticalMarkerPoint(GaugeLinearScale scale)
        {
            int offset = GaugePointer.Offset;

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

            _Origin = GetOriginInterval() - scale.MinValue;
            _Marker = GetInterval(Value) - scale.MinValue;

            CalcVerticalMarkerBounds(scale, x);
        }

        #region CalcVerticalMarkerBounds

        protected virtual void CalcVerticalMarkerBounds(GaugeLinearScale scale, int x)
        {
            double origin = _Origin;
            double marker = _Marker;

            _RoundAngle = -90;

            if (marker < origin)
            {
                _RoundAngle += 180;

                SwapDoubles(ref marker, ref origin);
            }

            int len = (int)((marker - origin) * Dpt);

            int y = (scale.Reversed == true)
                ? scale.ScaleBounds.Top + (int)(marker * Dpt) - len
                : scale.ScaleBounds.Bottom - (int)(marker * Dpt);

            _Bounds = new Rectangle(0, 0, Width, len);
            _Bounds.Location = new Point(x, y);
        }

        #endregion

        #endregion

        #endregion

        #region GetOriginInterval

        private double GetOriginInterval()
        {
            switch (GaugePointer.Origin)
            {
                case PointerOrigin.Minimum:
                    return (GetInterval(double.MinValue));

                case PointerOrigin.Maximum:
                    return (GetInterval(double.MaxValue));

                default:
                    return (GetInterval(GaugePointer.OriginInterval));
            }
        }

        #endregion

        #endregion

        #region RenderCircular

        public override void RenderCircular(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            GaugeCircularScale scale = Scale as GaugeCircularScale;

            if (scale != null)
            {
                if (Width > 0 && Radius > 0 && (_SweepAngle != 0 || _RoundAngle != 0))
                {
                    using (GraphicsPath path = GetCircularBarPath(scale))
                        RenderBar(g, path);
                }
            }
        }

        #region GetCircularBarPath

        private GraphicsPath GetCircularBarPath(GaugeCircularScale scale)
        {
            GraphicsPath path = new GraphicsPath();

            Rectangle r = _Bounds;
            r.Inflate(Width / 2, Width / 2);

            path.AddArc(r, _StartAngle, _SweepAngle);

            if (_RoundAngle != 0)
            {
                if (GaugePointer.BarStyle != BarPointerStyle.Square)
                {
                    Point pt = scale.GetPoint(_Bounds.Width/2, _StartAngle + _SweepAngle);
                    Point pt2 = scale.GetPoint(_Bounds.Width/2, _StartAngle + _SweepAngle + _RoundAngle);

                    int dx = pt2.X - pt.X;
                    int dy = pt2.Y - pt.Y;

                    double n = Math.Max(1, Math.Sqrt((dx * dx) + (dy * dy)));

                    float angle = (_StartAngle + _SweepAngle) % 360;

                    using (GraphicsPath path2 = new GraphicsPath())
                    {
                        Matrix matrix = new Matrix();
                        matrix.RotateAt(angle, pt);

                        r.X = pt.X - Width/2 + 1;
                        r.Y = pt.Y - (int) n;
                        r.Width = Width - 1;
                        r.Height = (int) (n*2);

                        if (GaugePointer.BarStyle == BarPointerStyle.Rounded)
                        {
                            path2.AddArc(r, 0, _SweepAngle + _RoundAngle > 0 ? 180 : -180);
                        }
                        else
                        {
                            path2.AddLine(new Point(r.Right, r.Y + r.Height/2),
                                new Point(r.X + Width/2, _SweepAngle + _RoundAngle > 0 ? r.Bottom : r.Y));
                        }

                        path2.Transform(matrix);

                        path.AddPath(path2, true);
                    }
                }
            }

            r = _Bounds;
            r.Inflate(-Width / 2, -Width / 2);

            path.AddArc(r, _StartAngle + _SweepAngle, -_SweepAngle);

            path.CloseFigure();

            return (path);
        }

        #endregion

        #endregion

        #region RenderLinear

        public override void RenderLinear(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            GaugeLinearScale scale = Scale as GaugeLinearScale;

            if (scale != null)
            {
                if (_Bounds.Width > 0 && _Bounds.Height > 0)
                {
                    using (GraphicsPath path = GetLinearBarPath(scale, _Bounds))
                        RenderBar(g, path);
                }
            }
        }

        #region GetLinearBarPath

        private GraphicsPath GetLinearBarPath(GaugeLinearScale scale, Rectangle b)
        {
            GraphicsPath path = new GraphicsPath();

            if (GaugePointer.BarStyle == BarPointerStyle.Square)
            {
                path.AddRectangle(b);
            }
            else
            {
                return ((scale.Orientation == Orientation.Horizontal)
                            ? GetHorizontalBarPath(path, b)
                            : GetVerticalBarPath(path, b));
            }

            return (path);
        }

        #endregion

        #region GetHorizontalBarPath

        private GraphicsPath GetHorizontalBarPath(GraphicsPath path, Rectangle b)
        {
            if (_Marker < _Origin != Scale.Reversed)
            {
                int x = Math.Min(b.X + Width / 2, b.Right);

                Point[] pts = new Point[] {
                        new Point(x, b.Y),
                        new Point (b.Right, b.Y),
                        new Point (b.Right, b.Bottom),
                        new Point (x, b.Bottom)};

                path.AddLines(pts);

                Rectangle r = new Rectangle(b.X, b.Y, (x - b.X) * 2, Width);

                if (GaugePointer.BarStyle == BarPointerStyle.Rounded)
                {
                    path.AddArc(r, 90, 180);
                }
                else
                {
                    path.AddLine(new Point(r.X + r.Width/2, r.Bottom),
                                 new Point(r.X, r.Y + r.Height/2));
                }

                path.CloseAllFigures();
            }
            else
            {
                int x = Math.Max(b.Right - Width / 2, b.X);

                Point[] pts = new Point[] {
                        new Point(x, b.Bottom),
                        new Point (b.X, b.Bottom),
                        new Point (b.X, b.Y),
                        new Point (x, b.Y)};

                path.AddLines(pts);

                int n = b.Right - x;

                Rectangle r = new Rectangle(x - n, b.Y, n * 2, Width);

                if (GaugePointer.BarStyle == BarPointerStyle.Rounded)
                {
                    path.AddArc(r, 270, 180);
                }
                else
                {
                    path.AddLine(new Point(r.X + r.Width / 2, r.Y),
                                 new Point(r.Right, r.Y + r.Height / 2));
                }

                path.CloseAllFigures();
            }

            return (path);
        }

        #endregion

        #region GetVerticalBarPath

        private GraphicsPath GetVerticalBarPath(GraphicsPath path, Rectangle b)
        {
            if (_Marker < _Origin != Scale.Reversed)
            {
                int y = Math.Max(b.Bottom - Width / 2, b.Y);

                Point[] pts = new Point[] {
                        new Point(b.X, y),
                        new Point (b.X, b.Y),
                        new Point (b.Right, b.Y),
                        new Point (b.Right, y)};

                path.AddLines(pts);

                int n = b.Bottom - y;

                Rectangle r = new Rectangle(b.X, y - n, Width, n * 2);

                if (GaugePointer.BarStyle == BarPointerStyle.Rounded)
                {
                    path.AddArc(r, 0, 180);
                }
                else
                {
                    path.AddLine(new Point(r.Right, r.Y + r.Height / 2),
                                 new Point(r.X + r.Width / 2, r.Bottom ));
                }
            }
            else
            {
                int y = Math.Min(b.Y + Width / 2, b.Bottom);

                Point[] pts = new Point[] {
                        new Point(b.Right, y),
                        new Point (b.Right, b.Bottom),
                        new Point (b.X, b.Bottom),
                        new Point (b.X, y)};

                path.AddLines(pts);

                Rectangle r = new Rectangle(b.X, b.Y, Width, (y - b.Y) * 2);

                if (GaugePointer.BarStyle == BarPointerStyle.Rounded)
                {
                    path.AddArc(r, 180, 180);
                }
                else
                {
                    path.AddLine(new Point(r.X, r.Y + r.Height / 2),
                                 new Point(r.X + r.Width / 2, r.Y));
                }
            }

            path.CloseAllFigures();

            return (path);
        }

        #endregion

        #endregion

        #region RenderBar

        #region RenderBar

        protected void RenderBar(Graphics g, GraphicsPath path)
        {
            RenderBar(g, path, GetPointerFillColor(GaugePointer.Value));
        }

        protected void RenderBar(Graphics g, GraphicsPath path, GradientFillColor fillColor)
        {

            if (fillColor.End.IsEmpty == true || fillColor.Color1 == fillColor.Color2 ||
                fillColor.GradientFillType == GradientFillType.None)
            {
                using (Brush br = new SolidBrush(fillColor.Color1))
                    g.FillPath(br, path);
            }
            else
            {
                GradientFillType fillType = (fillColor.GradientFillType == GradientFillType.Auto)
                                                ? GradientFillType.Center
                                                : fillColor.GradientFillType;
                switch (fillType)
                {
                    case GradientFillType.Auto:
                    case GradientFillType.StartToEnd:
                        RenderBarStartToEnd(g, path, fillColor);
                        break;

                    case GradientFillType.Angle:
                        RenderBarByAngle(g, path, fillColor);
                        break;

                    case GradientFillType.Center:
                        RenderBarByCenter(g, path, fillColor);
                        break;

                    case GradientFillType.HorizontalCenter:
                        RenderBarByHc(g, path, fillColor);
                        break;

                    case GradientFillType.VerticalCenter:
                        RenderBarByVc(g, path, fillColor);
                        break;
                }
            }

            if (fillColor.BorderWidth > 0)
            {
                using (Pen pen = new Pen(fillColor.BorderColor, fillColor.BorderWidth))
                    g.DrawPath(pen, path);
            }
        }

        #endregion

        #region RenderBarStartToEnd

        private void RenderBarStartToEnd(Graphics g,
            GraphicsPath path, GradientFillColor fillColor)
        {
            if (Scale is GaugeCircularScale)
            {
                using (PathGradientBrush br = ((GaugeCircularScale)Scale).CreateGradient(
                    Scale.GaugeControl.Frame.Bounds, _StartAngle, _SweepAngle, fillColor, Width * 2))
                {
                    if (GaugePointer.BarStyle == BarPointerStyle.Rounded)
                    {
                        using (Brush br2 = new SolidBrush(fillColor.Color2))
                            g.FillPath(br2, path);
                    }

                    g.FillPath(br, path);
                }
            }
            else
            {
                Rectangle t = Rectangle.Round(path.GetBounds());

                float angle = _RoundAngle + (Scale.Reversed ? 180 : 0);

                using (Brush br = fillColor.GetBrush(t, (int)angle))
                    g.FillPath(br, path);
            }
        }

        #endregion

        #region RenderBarByAngle

        private void RenderBarByAngle(Graphics g, GraphicsPath path, GradientFillColor fillColor)
        {
            int n = Width / 2;

            Rectangle t = Rectangle.Round(path.GetBounds());
            t.Inflate(n, n);

            using (Brush br = fillColor.GetBrush(t))
                g.FillPath(br, path);
        }

        #endregion

        #region RenderBarByCenter

        protected virtual void RenderBarByCenter(Graphics g, GraphicsPath path, GradientFillColor fillColor)
        {
            using (PathGradientBrush br = new PathGradientBrush(path))
            {
                br.WrapMode = WrapMode.TileFlipXY;

                br.CenterColor = fillColor.Color1;
                br.SurroundColors = new Color[] {fillColor.Color2};

                if (Scale is GaugeCircularScale)
                {
                    br.CenterPoint = Scale.Center;

                    float m = (float)Width / (_Bounds.Width / 2);

                    Blend blnd = new Blend();
                    blnd.Positions = new float[] { 0f, m, 1f };
                    blnd.Factors = new float[] { 1f, 0f, 0f };
                    br.Blend = blnd;
                }

                g.FillPath(br, path);
            }
        }

        #endregion

        #region RenderBarByHc

        private void RenderBarByHc(Graphics g, GraphicsPath path, GradientFillColor fillColor)
        {
            Rectangle t = Rectangle.Round(path.GetBounds());
            t.Height /= 2;

            using (LinearGradientBrush br = new
                LinearGradientBrush(t, fillColor.Color1, fillColor.Color2, 90))
            {
                br.WrapMode = WrapMode.TileFlipXY;

                g.FillPath(br, path);
            }
        }

        #endregion

        #region RenderBarByVc

        private void RenderBarByVc(Graphics g, GraphicsPath path, GradientFillColor fillColor)
        {
            Rectangle t = Rectangle.Round(path.GetBounds());
            t.Width /= 2;

            using (LinearGradientBrush br = new
                LinearGradientBrush(t, fillColor.Color1, fillColor.Color2, 0f))
            {
                br.WrapMode = WrapMode.TileFlipXY;

                g.FillPath(br, path);
            }
        }

        #endregion

        #endregion

        #region GetPointerPath

        public override GraphicsPath GetPointerPath()
        {
            if (PointerPath == null)
            {
                if (Width > 0)
                {
                    PointerPath = Scale.GaugeControl.OnGetPointerPath(GaugePointer, Scale.Bounds);

                    if (PointerPath == null)
                    {
                        if (Scale is GaugeCircularScale)
                            GetCPointerPath(Scale as GaugeCircularScale);
                        else
                            GetLPointerPath(Scale as GaugeLinearScale);
                    }

                }
            }

            return (PointerPath);
        }

        #endregion

        #region GetCPointerPath

        private void GetCPointerPath(GaugeCircularScale scale)
        {
            if (Radius > 0 && (_SweepAngle != 0 || _RoundAngle != 0))
                PointerPath = GetCircularBarPath(scale);
        }

        #endregion

        #region GetLPointerPath

        private void GetLPointerPath(GaugeLinearScale scale)
        {
            if (_Bounds.Width > 0 && _Bounds.Height > 0)
                PointerPath = GetLinearBarPath(scale, _Bounds);
        }

        #endregion

        #region OnMouseDown

        internal override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            GaugePointer.ValueEx = GetValueFromPoint(e.Location);
        }

        #endregion
    }

    #region Enums

    public enum BarPointerStyle
    {
        Square,
        Rounded,
        Pointed
    }

    #endregion
}
