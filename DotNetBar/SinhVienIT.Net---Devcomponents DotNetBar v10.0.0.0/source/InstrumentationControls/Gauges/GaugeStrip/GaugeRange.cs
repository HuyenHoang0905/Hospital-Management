using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using DevComponents.Instrumentation.Primitives;

namespace DevComponents.Instrumentation
{
    /// <summary>
    /// Collection of GaugeRanges
    /// </summary>
    public class GaugeRangeCollection : GenericCollection<GaugeRange>
    {
        #region ICloneable Members

        public override object Clone()
        {
            GaugeRangeCollection copy = new GaugeRangeCollection();

            CopyToItem(copy);

            return (copy);
        }

        #endregion

        #region CopyToItem

        internal void CopyToItem(GaugeRangeCollection copy)
        {
            foreach (GaugeRange item in this)
            {
                GaugeRange ic = new GaugeRange();

                item.CopyToItem(ic);

                copy.Add(ic);
            }
        }

        #endregion
    }

    public class GaugeRange : GaugeStrip
    {
        #region Private variables

        private DisplayPlacement _Placement;

        private float _StartWidth;
        private float _EndWidth;
        private int _Radius;

        #endregion

        public GaugeRange(GaugeScale scale)
            : base(scale)
        {
            InitGaugeRange();
        }

        public GaugeRange()
        {
            InitGaugeRange();
        }

        #region InitGaugeRange

        private void InitGaugeRange()
        {
            _Placement = DisplayPlacement.Near;

            _StartWidth = .15f;
            _EndWidth = .30f;
        }

        #endregion

        #region Public properties

        #region EndWidth

        /// <summary>
        /// Gets or sets the End Width of the Range, specified as a percentage
        /// </summary>
        [Browsable(true)]
        [Category("Layout"), DefaultValue(.3f)]
        [Editor("DevComponents.Instrumentation.Design.WidthMaxRangeValueEditor, DevComponents.Instrumentation.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=76cb4c6eb576bca5", typeof(UITypeEditor))]
        [Description("Indicates the End Width of the Range, specified as a percentage.")]
        public float EndWidth
        {
            get { return (_EndWidth); }

            set
            {
                if (_EndWidth != value)
                {
                    if (value < 0 || value > 1)
                        throw new ArgumentException("Width must be bwtween 0 and 1");

                    _EndWidth = value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        #endregion

        #region Placement

        /// <summary>
        /// Gets or sets the Placement of the Range with respect to the Scale
        /// </summary>
        [Browsable(true)]
        [Category("Layout"), DefaultValue(DisplayPlacement.Near)]
        [Description("Indicates the Placement of the Range with respect to the Scale.")]
        public DisplayPlacement Placement
        {
            get { return (_Placement); }

            set
            {
                if (_Placement != value)
                {
                    _Placement = value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        #endregion

        #region StartWidth

        /// <summary>
        /// Gets or sets the Start Width of the Range, specified as a percentage
        /// </summary>
        [Browsable(true)]
        [Category("Layout"), DefaultValue(.15f)]
        [Editor("DevComponents.Instrumentation.Design.WidthMaxRangeValueEditor, DevComponents.Instrumentation.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=76cb4c6eb576bca5", typeof(UITypeEditor))]
        [Description("Indicates the Start Width of the Range, specified as a percentage.")]
        public float StartWidth
        {
            get { return (_StartWidth); }

            set
            {
                if (_StartWidth != value)
                {
                    if (value < 0 || value > 1)
                        throw new ArgumentException("Width must be bwtween 0 and 1");

                    _StartWidth = value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        #endregion

        #endregion

        #region Internal properties

        #region AbsScaleOffset

        internal int AbsScaleOffset
        {
            get
            {
                if (Scale is GaugeCircularScale)
                    return ((int)(((GaugeCircularScale)Scale).AbsRadius * ScaleOffset));

                return ((int)(((GaugeLinearScale)Scale).AbsScaleWidth * ScaleOffset));
            }
        }

        #endregion

        #endregion

        #region RecalcLayout

        public override void RecalcLayout()
        {
            if (NeedRecalcLayout == true)
            {
                base.RecalcLayout();

                if (Scale is GaugeCircularScale)
                    RecalcCircularLayout(Scale as GaugeCircularScale);

                else if (Scale is GaugeLinearScale)
                    RecalcLinearLayout(Scale as GaugeLinearScale);
            }
        }

        #region RecalcCircularLayout

        private void RecalcCircularLayout(GaugeCircularScale scale)
        {
            _Radius = scale.AbsRadius;

            Point center = Scale.Center;
            int offset = AbsScaleOffset;
            int scaleWidth = (int)(_Radius * scale.Width);

            switch (_Placement)
            {
                case DisplayPlacement.Near:
                    _Radius -= ((scaleWidth / 2) + offset);
                    break;

                case DisplayPlacement.Center:
                    _Radius += offset;

                    break;

                case DisplayPlacement.Far:
                    _Radius += ((scaleWidth / 2) + offset);
                    break;
            }

            Bounds = new Rectangle(center.X - _Radius,
                                   center.Y - _Radius, _Radius * 2, _Radius * 2);
        }

        #endregion

        #region RecalcLinearLayout

        private void RecalcLinearLayout(GaugeLinearScale scale)
        {
            float spread = (float)Math.Abs(scale.MaxValue - scale.MinValue);

            if (spread == 0)
                spread = 1;

            if (scale.Orientation == Orientation.Horizontal)
                CalcHorizontalMetrics(scale, spread);
            else
                CalcVerticalMetrics(scale, spread);
        }

        #region CalcHorizontalMetrics

        private void CalcHorizontalMetrics(GaugeLinearScale scale, float spread)
        {
            int length = scale.ScaleBounds.Width;
            int width = scale.Bounds.Height;
            int scaleWidth = scale.ScaleBounds.Height;

            float dl = length / spread;

            int start = (int)(dl * MinValue);
            int len = (int)(dl * (MaxValue - MinValue));
            int offset = (int)(width * ScaleOffset);

            int y = 0;

            switch (_Placement)
            {
                case DisplayPlacement.Near:
                    y -= ((scaleWidth / 2) + offset);
                    break;

                case DisplayPlacement.Center:
                    y += offset;
                    break;

                case DisplayPlacement.Far:
                    y += ((scaleWidth / 2) + offset);
                    break;
            }

            Rectangle r = scale.Bounds;

            if (scale.Reversed == true)
                r.X = r.Right - (start + len);
            else
                r.X += start;

            r.Width = len;
            r.Y = scale.Center.Y + y;
            r.Height = 0;

            Bounds = r;
        }

        #endregion

        #region CalcVerticalMetrics

        private void CalcVerticalMetrics(GaugeLinearScale scale, float spread)
        {
            int length = scale.ScaleBounds.Height;
            int width = scale.Bounds.Width;
            int scaleWidth = scale.ScaleBounds.Width;

            float dl = length / spread;

            int start = (int)(dl * MinValue);
            int len = (int)(dl * (MaxValue - MinValue));
            int offset = (int)(width * ScaleOffset);

            int x = 0;

            switch (_Placement)
            {
                case DisplayPlacement.Near:
                    x -= ((scaleWidth / 2) + offset);
                    break;

                case DisplayPlacement.Center:
                    x += offset;
                    break;

                case DisplayPlacement.Far:
                    x += ((scaleWidth / 2) + offset);
                    break;
            }

            Rectangle r = scale.Bounds;

            if (scale.Reversed == true)
                r.Y += start;
            else
                r.Y = r.Bottom - (start + len);

            r.X = scale.Center.X + x;
            r.Width = 0;
            r.Height = len;

            Bounds = r;
        }

        #endregion

        #endregion

        #endregion

        #region OnPaint

        public override void OnPaint(PaintEventArgs e)
        {
            RecalcLayout();

            if (Scale.GaugeControl.OnPreRenderScaleRange(e, this) == false)
            {
                if (Scale is GaugeCircularScale)
                    PaintCircularRange(e, Scale as GaugeCircularScale);

                else if (Scale is GaugeLinearScale)
                    PaintLinearRange(e, Scale as GaugeLinearScale);

                Scale.GaugeControl.OnPostRenderScaleRange(e, this);
            }
        }

        #endregion

        #region PaintCircularRange

        private void PaintCircularRange(PaintEventArgs e, GaugeCircularScale scale)
        {
            GraphicsPath path = GetRangePath();

            if (path != null)
            {
                if (SweepAngle != 0 && _Radius > 1)
                {
                    int radius = scale.AbsRadius;

                    int n1 = (int)(radius * _StartWidth);
                    int n2 = (int)(radius * _EndWidth);

                    if (_Radius - n1 <= 0)
                        n1 = _Radius - 1;

                    if (_Radius - n2 <= 0)
                        n2 = _Radius - 1;

                    PaintRange(e.Graphics, path, n1, n2);
                }
            }
        }

        #region GetCircularRangePath

        private GraphicsPath GetCircularRangePath(int n1, int n2)
        {
            switch (_Placement)
            {
                case DisplayPlacement.Near:
                    return (GetNearCircularRangePath(n1, n2));

                case DisplayPlacement.Center:
                    return (GetCenterCircularRangePath(n1, n2));

                default:
                    return (GetFarCircularRangePath(n1, n2));
            }
        }

        #region GetNearCircularRangePath

        private GraphicsPath GetNearCircularRangePath(int n1, int n2)
        {
            GraphicsPath path = new GraphicsPath();

            float angle = StartAngle + SweepAngle;

            path.AddArc(Bounds, StartAngle, SweepAngle);

            if (n1 == n2)
            {
                Rectangle r = Bounds;
                r.Inflate(-n1, -n1);

                path.AddArc(r, angle, -SweepAngle);
            }
            else
            {
                int n = path.PointCount;

                float dx = SweepAngle / n;
                float dy = (float)(n2 - n1) / n;
                float radius = _Radius - n2;

                PointF[] pts = new PointF[n + 1];

                for (int i = 0; i < n; i++)
                {
                    pts[i] = GetPoint(radius, angle);

                    radius += dy;
                    angle -= dx;
                }

                pts[n] = GetPoint(_Radius - n1, StartAngle);

                path.AddCurve(pts);
            }

            path.CloseFigure();

            return (path);
        }

        #endregion

        #region GetCenterCircularRangePath

        private GraphicsPath GetCenterCircularRangePath(int n1, int n2)
        {
            GraphicsPath path = new GraphicsPath();

            Rectangle r = Bounds;
            r.Inflate(n1 / 2, n1 / 2);

            path.AddArc(r, StartAngle, SweepAngle);

            if (n1 == n2)
            {
                r.Inflate(-n1, -n1);

                path.AddArc(r, StartAngle + SweepAngle, -SweepAngle);
            }
            else
            {
                int n = path.PointCount;
                path.Reset();

                float angle = StartAngle;
                float radius = _Radius + (n1 / 2);

                float dx = SweepAngle / n;
                float dy = (float)(n2 - n1) / n / 2;

                PointF[] pts = new PointF[n + 1];

                for (int i = 0; i < n; i++)
                {
                    pts[i] = GetPoint(radius, angle);

                    radius += dy;
                    angle += dx;
                }

                pts[n] = GetPoint(radius, angle);

                path.AddCurve(pts);

                radius = _Radius - (n2 / 2);

                for (int i = 0; i < n; i++)
                {
                    pts[i] = GetPoint(radius, angle);

                    radius += dy;
                    angle -= dx;
                }

                pts[n] = GetPoint(radius, angle);

                path.AddCurve(pts);
            }

            path.CloseFigure();

            return (path);
        }

        #endregion

        #region GetFarCircularRangePath

        private GraphicsPath GetFarCircularRangePath(int n1, int n2)
        {
            GraphicsPath path = new GraphicsPath();

            float angle = StartAngle + SweepAngle;

            path.AddArc(Bounds, StartAngle, SweepAngle);

            if (n1 == n2)
            {
                Rectangle r = Bounds;
                r.Inflate(n1, n1);

                path.AddArc(r, angle, -SweepAngle);
            }
            else
            {
                int n = path.PointCount;

                float radius = _Radius + n2;
                float dx = SweepAngle/n;
                float dy = (float)(n2 - n1) / n;

                PointF[] pts = new PointF[n + 1];

                for (int i = 0; i < n; i++)
                {
                    pts[i] = GetPoint(radius, angle);

                    radius -= dy;
                    angle -= dx;
                }

                pts[n] = GetPoint(_Radius + n1, StartAngle);

                path.AddCurve(pts);
            }

            path.CloseFigure();

            return (path);
        }

        #endregion

        #endregion

        #endregion

        #region PaintLinearRange

        private void PaintLinearRange(PaintEventArgs e, GaugeLinearScale scale)
        {
            GraphicsPath path = GetRangePath();

            if (path != null)
            {
                int width = scale.AbsWidth;

                int n1 = (int)(width * _StartWidth);
                int n2 = (int)(width * _EndWidth);

                if (n1 > 0 || n2 > 0)
                    PaintRange(e.Graphics, path, n1, n2);
            }
        }

        #region GetLinearRangePath

        private GraphicsPath GetLinearRangePath(
            int n1, int n2, Orientation orientation)
        {
            if (Scale.Reversed == true)
            {
                int n3 = n1;

                n1 = n2;
                n2 = n3;
            }

            switch (_Placement)
            {
                case DisplayPlacement.Near:
                    return (GetNearLinearRangePath(n1, n2, orientation));

                case DisplayPlacement.Center:
                    return (GetCenterLinearRangePath(n1, n2, orientation));

                default:
                    return (GetFarLinearRangePath(n1, n2, orientation));
            }
        }

        #region GetNearLinearRangePath

        private GraphicsPath GetNearLinearRangePath(
            int n1, int n2, Orientation orientation)
        {
            GraphicsPath path = new GraphicsPath();

            if (orientation == Orientation.Horizontal)
            {
                Point[] pts = new Point[] {
                    new Point(Bounds.X, Bounds.Y),
                    new Point(Bounds.Right, Bounds.Y),
                    new Point(Bounds.Right, Bounds.Y - n2),
                    new Point(Bounds.X, Bounds.Y - n1),
                    new Point(Bounds.X, Bounds.Y),
                };

                path.AddLines(pts);
            }
            else
            {
                Point[] pts = new Point[] {
                    new Point(Bounds.X, Bounds.Y),
                    new Point(Bounds.X, Bounds.Bottom),
                    new Point(Bounds.X - n1, Bounds.Bottom),
                    new Point(Bounds.X - n2, Bounds.Y),
                    new Point(Bounds.X, Bounds.Y),
                };

                path.AddLines(pts);
            }

            return (path);
        }

        #endregion

        #region GetCenterLinearRangePath

        private GraphicsPath GetCenterLinearRangePath(
            int n1, int n2, Orientation orientation)
        {
            GraphicsPath path = new GraphicsPath();

            int dn1 = n1 / 2;
            int dn2 = n2 / 2;

            if (orientation == Orientation.Horizontal)
            {
                Point[] pts = new Point[] {
                    new Point(Bounds.X, Bounds.Y + dn1),
                    new Point(Bounds.Right, Bounds.Y + dn2),
                    new Point(Bounds.Right, Bounds.Y - dn2),
                    new Point(Bounds.X, Bounds.Y - dn1),
                    new Point(Bounds.X, Bounds.Y + dn1),
                };

                path.AddLines(pts);
            }
            else
            {
                Point[] pts = new Point[] {
                    new Point(Bounds.X + dn2, Bounds.Y),
                    new Point(Bounds.Right + dn1, Bounds.Bottom),
                    new Point(Bounds.Right - dn1, Bounds.Bottom),
                    new Point(Bounds.X - dn2, Bounds.Y),
                    new Point(Bounds.X + dn2, Bounds.Y),
                };

                path.AddLines(pts);
            }

            return (path);
        }

        #endregion

        #region GetFarLinearRangePath

        private GraphicsPath GetFarLinearRangePath(
            int n1, int n2, Orientation orientation)
        {
            GraphicsPath path = new GraphicsPath();

            if (orientation == Orientation.Horizontal)
            {
                Point[] pts = new Point[] {
                    new Point(Bounds.X, Bounds.Y),
                    new Point(Bounds.Right, Bounds.Y),
                    new Point(Bounds.Right, Bounds.Y + n2),
                    new Point(Bounds.X, Bounds.Y + n1),
                    new Point(Bounds.X, Bounds.Y),
                };

                path.AddLines(pts);
            }
            else
            {
                Point[] pts = new Point[] {
                    new Point(Bounds.X, Bounds.Y),
                    new Point(Bounds.X, Bounds.Bottom),
                    new Point(Bounds.Right + n1, Bounds.Bottom),
                    new Point(Bounds.Right + n2 , Bounds.Y),
                    new Point(Bounds.X, Bounds.Y),
                };

                path.AddLines(pts);
            }
            
            return (path);
        }

        #endregion

        #endregion

        #endregion

        #region PaintRange

        #region PaintRange

        private void PaintRange(Graphics g, GraphicsPath path, int n1, int n2)
        {
            if (FillColor.End.IsEmpty == true || FillColor.Color1 == FillColor.Color2 ||
                FillColor.GradientFillType == GradientFillType.None)
            {
                using (Brush br = new SolidBrush(FillColor.Color1))
                    g.FillPath(br, path);
            }
            else
            {
                switch (FillColor.GradientFillType)
                {
                    case GradientFillType.Auto:
                    case GradientFillType.StartToEnd:
                        PaintRangeStartToEnd(g, path, n1, n2);
                        break;

                    case GradientFillType.Angle:
                        PaintRangeByAngle(g, path);
                        break;

                    case GradientFillType.Center:
                        PaintRangeByCenter(g, path, n1, n2);
                        break;

                    case GradientFillType.HorizontalCenter:
                        PaintRangeByHc(g, path);
                        break;

                    case GradientFillType.VerticalCenter:
                        PaintRangeByVc(g, path);
                        break;
                }
            }

            if (FillColor.BorderWidth > 0)
            {
                using (Pen pen = new Pen(FillColor.BorderColor, FillColor.BorderWidth))
                {
                    pen.Alignment = PenAlignment.Inset;

                    g.DrawPath(pen, path);
                }
            }
        }

        #endregion

        #region PaintRangeStartToEnd

        private void PaintRangeStartToEnd(
            Graphics g, GraphicsPath path, int n1, int n2)
        {
            if (Scale is GaugeCircularScale)
                PaintCircularRangeStartToEnd(g, path, n1, n2, Scale as GaugeCircularScale);
            else
                PaintLinearRangeStartToEnd(g, path, Scale as GaugeLinearScale);
        }

        #region PaintCircularRangeStartToEnd

        private void PaintCircularRangeStartToEnd(Graphics g,
            GraphicsPath path, int n1, int n2, GaugeCircularScale scale)
        {
            if (FillColor.Color2.IsEmpty == true || (FillColor.Color1 == FillColor.Color2))
            {
                using (Brush br = new SolidBrush(FillColor.Color1))
                    g.FillPath(br, path);
            }
            else
            {
                int n = Math.Max(n1, n2) * 3;

                if (n > 0)
                {
                    using (PathGradientBrush br =
                        scale.CreateGradient(Bounds, StartAngle, SweepAngle, FillColor, n))
                    {
                        g.FillPath(br, path);
                    }
                }
            }
        }

        #endregion

        #region PaintLinearRangeStartToEnd

        private void PaintLinearRangeStartToEnd(Graphics g, GraphicsPath path, GaugeLinearScale scale)
        {
            if (FillColor.Color2.IsEmpty == true || (FillColor.Color1 == FillColor.Color2))
            {
                using (Brush br = new SolidBrush(FillColor.Color1))
                    g.FillPath(br, path);
            }
            else
            {
                int angle = (scale.Orientation == Orientation.Horizontal ? 0 : -90);

                if (Scale.Reversed == true)
                    angle += 180;

                Rectangle r = Rectangle.Round(path.GetBounds());
                r.Inflate(1, 1);

                using (Brush br = FillColor.GetBrush(r, angle))
                    g.FillPath(br, path);
            }
        }

        #endregion

        #endregion

        #region PaintRangeByAngle

        private void PaintRangeByAngle(Graphics g, GraphicsPath path)
        {
            Rectangle r = Rectangle.Round(path.GetBounds());
            r.Inflate(1, 1);

            using (Brush br = FillColor.GetBrush(r))
                g.FillPath(br, path);
        }

        #endregion

        #region PaintRangeByCenter

        private void PaintRangeByCenter(Graphics g, GraphicsPath path, int n1, int n2)
        {
            if (Scale is GaugeCircularScale)
                PaintCircularRangeByCenter(g, path, n1, n2);
            else
                PaintLinearRangeByCenter(g, path);
        }

        #region PaintCircularRangeByCenter

        private void PaintCircularRangeByCenter(
            Graphics g, GraphicsPath path, int n1, int n2)
        {
            using (PathGradientBrush br = new PathGradientBrush(path))
            {
                br.CenterColor = FillColor.Color1;
                br.SurroundColors = new Color[] { FillColor.Color2 };
                br.CenterPoint = Scale.Center;

                float m = (float)Math.Max(n1, n2) / _Radius;

                Blend blnd = new Blend();
                blnd.Positions = new float[] { 0f, m, 1f };
                blnd.Factors = new float[] { 1f, 0f, 0f };
                br.Blend = blnd;

                g.FillPath(br, path);
            }
        }

        #endregion

        #region PaintLinearRangeByCenter

        private void PaintLinearRangeByCenter(Graphics g, GraphicsPath path)
        {
            using (PathGradientBrush br = new PathGradientBrush(path))
            {
                br.CenterColor = FillColor.Color1;
                br.SurroundColors = new Color[] { FillColor.Color2 };

                g.FillPath(br, path);
            }
        }

        #endregion

        #endregion

        #region PaintRangeByHc

        private void PaintRangeByHc(Graphics g, GraphicsPath path)
        {
            Rectangle r = Rectangle.Round(path.GetBounds());
            r.Height /= 2;

            using (LinearGradientBrush br = new
                LinearGradientBrush(r, FillColor.Color1, FillColor.Color2, 90))
            {
                br.WrapMode = WrapMode.TileFlipXY;

                g.FillPath(br, path);
            }
        }

        #endregion

        #region PaintRangeByVc

        private void PaintRangeByVc(Graphics g, GraphicsPath path)
        {
            Rectangle r = Rectangle.Round( path.GetBounds());
            r.Width /= 2;

            using (LinearGradientBrush br = new
                LinearGradientBrush(r, FillColor.Color1, FillColor.Color2, 0f))
            {
                br.WrapMode = WrapMode.TileFlipXY;

                g.FillPath(br, path);
            }
        }

        #endregion

        #endregion

        #region FindItem

        #region FindItem

        internal override GaugeItem FindItem(Point pt)
        {
            GraphicsPath path = GetRangePath();

            if (path != null)
            {
                if (path.IsVisible(pt) == true)
                    return (this);
            }

            return (null);
        }

        #endregion

        #region GetRangePath

        public GraphicsPath GetRangePath()
        {
            if (StripePath == null)
            {
                if (Scale is GaugeCircularScale)
                    StripePath = GetCRangePathEx(Scale as GaugeCircularScale);
                else
                    StripePath = GetLRangePathEx(Scale as GaugeLinearScale);
            }

            return (StripePath);
        }

        #endregion

        #region GetCRangePathEx

        private GraphicsPath GetCRangePathEx(GaugeCircularScale scale)
        {
            if (SweepAngle != 0 && _Radius > 1)
            {
                int radius = scale.AbsRadius;

                int n1 = (int)(radius * _StartWidth);
                int n2 = (int)(radius * _EndWidth);

                if (_Radius - n1 <= 0)
                    n1 = _Radius - 1;

                if (_Radius - n2 <= 0)
                    n2 = _Radius - 1;

                return (GetCircularRangePath(n1, n2));
            }

            return (null);
        }

        #endregion

        #region GetLRangePathEx

        private GraphicsPath GetLRangePathEx(GaugeLinearScale scale)
        {
            if ((scale.Orientation == Orientation.Horizontal && Bounds.Width > 0) ||
                (scale.Orientation == Orientation.Vertical && Bounds.Height > 0))
            {
                int width = scale.AbsWidth;

                int n1 = (int)(width * _StartWidth);
                int n2 = (int)(width * _EndWidth);

                if (n1 > 0 || n2 > 0)
                    return (GetLinearRangePath(n1, n2, scale.Orientation));
            }

            return (null);
        }

        #endregion

        #endregion

        #region ICloneable Members

        public override object Clone()
        {
            GaugeRange copy = new GaugeRange();

            CopyToItem(copy);

            return (copy);
        }

        #endregion

        #region CopyToItem

        public override void CopyToItem(GaugeItem copy)
        {
            GaugeRange c = copy as GaugeRange;

            if (c != null)
            {
                base.CopyToItem(c);

                c.EndWidth = _EndWidth;
                c.Placement = _Placement;
                c.StartWidth = _StartWidth;
            }
        }

        #endregion
    }
}
