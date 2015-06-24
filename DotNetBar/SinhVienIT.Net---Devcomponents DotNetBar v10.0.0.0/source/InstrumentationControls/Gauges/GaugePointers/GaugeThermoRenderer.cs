using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DevComponents.Instrumentation
{
    internal class GaugeThermoRenderer : GaugeBarRenderer
    {
        #region Private variables

        private Rectangle _BackBounds;
        private Rectangle _BulbBounds;

        #endregion

        internal GaugeThermoRenderer(GaugePointer gaugePointer)
            : base(gaugePointer)
        {
        }

        #region RecalcLayout

        #region  CalcHorizontalMarkerBounds

        protected override void CalcHorizontalMarkerBounds(GaugeLinearScale scale, int y)
        {
            int n = (int)(GaugePointer.BulbOffset * scale.ScaleBounds.Width);

            if (Marker < Origin)
            {
                RoundAngle = 180;

                int len = Math.Max(1, (int)((Origin - Marker) * Dpt) + n);
                int x;

                if (scale.Reversed == true)
                {
                    x = scale.ScaleBounds.Right - (int)(Origin * Dpt) - n;
                }
                else
                {
                    x = scale.ScaleBounds.X + (int)(Marker * Dpt);
                    int x2 = scale.ScaleBounds.X + (int)(Origin * Dpt) + n;

                    len = Math.Max(1, x2 - x + 1);
                }

                Bounds = new Rectangle(x, y, len, Width);

                double maxMarker = GetInterval(double.MinValue) - scale.MinValue;

                _BackBounds = Bounds;
                _BackBounds.Width = (int)((Origin - maxMarker) * Dpt + n);

                if (scale.Reversed == false)
                    _BackBounds.X = scale.ScaleBounds.Left + (int)(maxMarker * Dpt);
            }
            else
            {
                RoundAngle = 0;

                int len = Math.Max(1, (int)((Marker - Origin) * Dpt) + n);
                int x;
                
                if (scale.Reversed == true)
                    x = scale.ScaleBounds.Right - (int)(Origin * Dpt) - len + n;
                else
                    x = scale.ScaleBounds.X + (int)(Origin * Dpt) - n;

                Bounds = new Rectangle(x, y, len, Width);

                double maxMarker = GetInterval(double.MaxValue) - scale.MinValue;

                _BackBounds = Bounds;
                _BackBounds.Width = (int)((maxMarker - Origin) * Dpt + n);

                if (scale.Reversed == true)
                    _BackBounds.X = scale.ScaleBounds.Right - (int)(maxMarker * Dpt) + 1;
            }
        }

        #endregion

        #region CalcVerticalMarkerBounds

        protected override void CalcVerticalMarkerBounds(GaugeLinearScale scale, int x)
        {
            int n = (int)(GaugePointer.BulbOffset * scale.ScaleBounds.Height);

            if (Marker < Origin)
            {
                RoundAngle = 90;

                int len = Math.Max(1, (int)((Origin - Marker) * Dpt) + n);
                int y;

                if (scale.Reversed == true)
                {
                    y = scale.ScaleBounds.Y + (int)(Marker * Dpt);
                    int y2 = scale.ScaleBounds.Y + (int)(Origin * Dpt) + n;

                    len = Math.Max(1, y2 - y);
                }
                else
                {
                    y = scale.ScaleBounds.Bottom - (int)(Origin * Dpt) - n;
                }

                Bounds = new Rectangle(x, y, Width, len);

                _BackBounds = Bounds;

                double minMarker = GetInterval(double.MinValue) - scale.MinValue;

                if (scale.Reversed == true)
                {
                    _BackBounds.Y = scale.ScaleBounds.Top + (int)(minMarker * Dpt);
                    _BackBounds.Height = Bounds.Bottom - _BackBounds.Y;
                }
                else
                {
                    _BackBounds.Height = (int)((Origin - minMarker) * Dpt + n);
                }
            }
            else
            {
                RoundAngle = -90;

                int len = Math.Max(1, (int)((Marker - Origin) * Dpt) + n);
                int y;

                if (scale.Reversed == true)
                {
                    y = scale.ScaleBounds.Top + (int)(Origin * Dpt) - n;
                }
                else
                {
                    y = scale.ScaleBounds.Bottom - (int)(Marker * Dpt);
                    int y2 = scale.ScaleBounds.Bottom - (int)(Origin * Dpt) + n;

                    len = Math.Max(1, y2 - y);
                }

                Bounds = new Rectangle(x, y, Width, len);

                _BackBounds = Bounds;

                double maxMarker = GetInterval(double.MaxValue) - scale.MinValue;

                if (scale.Reversed == false)
                {
                    _BackBounds.Y = scale.ScaleBounds.Bottom - (int)(maxMarker * Dpt);
                    _BackBounds.Height = Bounds.Bottom - _BackBounds.Y;
                }
                else
                {
                    _BackBounds.Height = (int)((maxMarker - Origin) * Dpt + n);
                }
            }
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
                if (Bounds.Width > 0 && Bounds.Height > 0)
                {
                    using (GraphicsPath path = GetBulbBackPath(scale))
                        RenderBar(g, path, GaugePointer.ThermoBackColor);

                    using (GraphicsPath path = GetBulbFillPath(scale))
                        RenderBar(g, path);
                }
            }
        }

        #region GetBulbBackPath

        private GraphicsPath GetBulbBackPath(GaugeLinearScale scale)
        {
            return (GetBulbPath(scale, _BackBounds, 1));
        }

        #endregion

        #region GetBulbFillPath

        private GraphicsPath GetBulbFillPath(GaugeLinearScale scale)
        {
            return (GetBulbPath(scale, Bounds, 0));
        }

        #endregion

        #region GetBulbPath

        private GraphicsPath GetBulbPath(
            GaugeLinearScale scale, Rectangle r, int inf)
        {
            GraphicsPath path = new GraphicsPath();

            r.Inflate(inf, inf);

            if (scale.Orientation == Orientation.Horizontal)
            {
                if (Marker < Origin != scale.Reversed)
                    return (GetHorizontalRightBulbPath(scale, path, r, inf));

                return (GetHorizontalLeftBulbPath(scale, path, r, inf));
            }

            if (Marker < Origin != scale.Reversed)
                return (GetVerticalTopBulbPath(scale, path, r, inf));

            return (GetVerticalBottomBulbPath(scale, path, r, inf));
        }

        #region GetHorizontalLeftBulbPath

        private GraphicsPath GetHorizontalLeftBulbPath(
            GaugeLinearScale scale, GraphicsPath path, Rectangle t, int inflate)
        {
            AddLeftTube(path, t, inflate);
            AddLeftBulb(scale, path, t, inflate);

            path.CloseAllFigures();

            return (path);
        }

        #region AddLeftTube

        private void AddLeftTube(GraphicsPath path, Rectangle t, int inflate)
        {
            if (GaugePointer.BarStyle == BarPointerStyle.Square || inflate > 0)
            {
                Point[] pts = new Point[] {
                    new Point(t.X, t.Y),
                    new Point(t.Right, t.Y),
                    new Point(t.Right, t.Bottom),
                    new Point(t.X, t.Bottom) };

                path.AddLines(pts);
            }
            else
            {
                int x = Math.Max(t.Right - Width / 2, t.X);
                int n = t.Right - x;

                Rectangle r = new Rectangle(x - n, t.Y, n * 2, t.Height);

                Point[] pts = new Point[] {
                    new Point(t.X, t.Y),
                    new Point (x, t.Y)};

                path.AddLines(pts);

                if (GaugePointer.BarStyle == BarPointerStyle.Rounded)
                {
                    path.AddArc(r, 270, 180);
                }
                else
                {
                    path.AddLine(new Point(r.X + r.Width / 2, r.Y),
                                 new Point(r.Right, r.Y + r.Height / 2));
                }

                pts = new Point[] {
                    new Point(x, t.Bottom),
                    new Point (t.X, t.Bottom)};

                path.AddLines(pts);
            }
        }

        #endregion

        #region AddLeftBulb

        private void AddLeftBulb(GaugeLinearScale scale, GraphicsPath path, Rectangle t, int inflate)
        {
            int bulbRadius = (int)(GaugePointer.BulbSize * scale.Bounds.Height);

            if (bulbRadius * 2 < t.Height)
                bulbRadius = t.Height / 2;

            int m = (int)Math.Sqrt((bulbRadius * bulbRadius) - (t.Height * t.Height / 4)) + 4;

            _BulbBounds = new Rectangle(
                t.X - m - bulbRadius,
                t.Y - bulbRadius + t.Height / 2, bulbRadius * 2, bulbRadius * 2);

            if (inflate > 0)
                _BulbBounds.Inflate(inflate, inflate);

            float angle = (GaugePointer.BulbStyle == BulbStyle.Flask)
                ? 90 : (float)(Math.Asin((double)(Width / 2) / bulbRadius) * 180 / Math.PI);

            path.AddArc(_BulbBounds, angle, 360 - (angle * 2));
        }

        #endregion

        #endregion

        #region GetHorizontalRightBulbPath

        private GraphicsPath GetHorizontalRightBulbPath(
            GaugeLinearScale scale, GraphicsPath path, Rectangle t, int inflate)
        {
            AddRightTube(path, t, inflate);
            AddRightBulb(scale, path, t, inflate);

            path.CloseAllFigures();

            return (path);
        }

        #region AddRightTube

        private void AddRightTube(GraphicsPath path, Rectangle t, int inflate)
        {
            if (GaugePointer.BarStyle == BarPointerStyle.Square || inflate > 0)
            {
                Point[] pts = new Point[] {
                      new Point(t.Right, t.Bottom),
                      new Point(t.X, t.Bottom),
                      new Point(t.X, t.Y),
                      new Point(t.Right, t.Y) };

                path.AddLines(pts);
            }
            else
            {
                int x = Math.Min(t.X + Width / 2, t.Right);
                int n = x - t.X;

                Rectangle r = new Rectangle(t.X, t.Y, n * 2, t.Height);

                Point[] pts = new Point[] {
                    new Point(t.Right, t.Bottom),
                    new Point (x, t.Bottom)};

                path.AddLines(pts);

                if (GaugePointer.BarStyle == BarPointerStyle.Rounded)
                {
                    path.AddArc(r, 90, 180);
                }
                else
                {
                    path.AddLine(new Point(r.X + r.Width / 2, r.Bottom),
                                 new Point(r.X, r.Y + r.Height / 2));
                }

                pts = new Point[] {
                    new Point(x, t.Y),
                    new Point (t.Right, t.Y)};

                path.AddLines(pts);
            }
        }

        #endregion

        #region AddRightBulb

        private void AddRightBulb(GaugeLinearScale scale, GraphicsPath path, Rectangle t, int inflate)
        {
            int bulbRadius = (int)(GaugePointer.BulbSize * scale.Bounds.Height);

            if (bulbRadius * 2 < t.Height)
                bulbRadius = t.Height / 2;

            int m = (int)Math.Sqrt((bulbRadius * bulbRadius) - (t.Height * t.Height / 4));

            _BulbBounds = new Rectangle(
                t.Right - bulbRadius + m,
                t.Y - bulbRadius + t.Height / 2, bulbRadius * 2, bulbRadius * 2);

            if (inflate > 0)
                _BulbBounds.Inflate(inflate, inflate);

            float angle = (GaugePointer.BulbStyle == BulbStyle.Flask)
                              ? 90 : (float)(Math.Asin((double)(Width / 2) / bulbRadius) * 180 / Math.PI);

            path.AddArc(_BulbBounds, angle + 180, 360 - (angle * 2));
        }

        #endregion

        #endregion

        #region GetVerticalBottomBulbPath

        private GraphicsPath GetVerticalBottomBulbPath(
            GaugeLinearScale scale, GraphicsPath path, Rectangle t, int inflate)
        {
            AddBottomTube(path, t, inflate);
            AddBottomBulb(scale, path, t, inflate);

            path.CloseAllFigures();

            return (path);
        }

        #region AddBottomTube

        private void AddBottomTube(GraphicsPath path, Rectangle t, int inflate)
        {
            if (GaugePointer.BarStyle == BarPointerStyle.Square || inflate > 0)
            {
                Point[] pts = new Point[] {
                    new Point(t.X, t.Bottom),
                    new Point(t.X, t.Y),
                    new Point(t.Right, t.Y),
                    new Point(t.Right, t.Bottom) };

                path.AddLines(pts);
            }
            else
            {
                int y = Math.Min(t.Y + t.Width / 2, t.Bottom);
                int n = y - t.Y;

                Rectangle r = new Rectangle(t.X, y - n, t.Width, n * 2);

                Point[] pts = new Point[] {
                    new Point(t.X, t.Bottom),
                    new Point (t.X, y)};

                path.AddLines(pts);

                if (GaugePointer.BarStyle == BarPointerStyle.Rounded)
                {
                    path.AddArc(r, 180, 180);
                }
                else
                {
                    path.AddLine(new Point(r.X, r.Y + r.Height / 2),
                                 new Point(r.X + r.Width / 2, r.Y));
                }

                pts = new Point[] {
                    new Point(t.Right, y),
                    new Point (t.Right, t.Bottom)};

                path.AddLines(pts);
            }
        }

        #endregion

        #region AddBottomBulb

        private void AddBottomBulb(GaugeLinearScale scale, GraphicsPath path, Rectangle t, int inflate)
        {
            int bulbRadius = (int)(GaugePointer.BulbSize * scale.Bounds.Width);

            if (bulbRadius * 2 < t.Width)
                bulbRadius = t.Width / 2;

            int m = (int)Math.Sqrt((bulbRadius * bulbRadius) - (Width * Width / 4)) + 4;

            _BulbBounds = new Rectangle(
                t.X - bulbRadius + t.Width / 2,
                t.Bottom - bulbRadius + m - inflate, bulbRadius * 2, bulbRadius * 2);

            if (inflate > 0)
                _BulbBounds.Inflate(inflate, inflate);

            float angle = ((GaugePointer.BulbStyle == BulbStyle.Flask)
                              ? 90 : (float)(Math.Asin((double)(Width / 2) / bulbRadius) * 180 / Math.PI));

            path.AddArc(_BulbBounds, angle + 270, 360 - (angle * 2));
        }

        #endregion

        #endregion

        #region GetVerticalTopBulbPath

        private GraphicsPath GetVerticalTopBulbPath(
            GaugeLinearScale scale, GraphicsPath path, Rectangle t, int inflate)
        {
            AddTopTube(path, t, inflate);
            AddTopBulb(scale, path, t, inflate);

            path.CloseAllFigures();

            return (path);
        }

        #region AddTopTube

        private void AddTopTube(GraphicsPath path, Rectangle t, int inflate)
        {
            if (GaugePointer.BarStyle == BarPointerStyle.Square || inflate > 0)
            {
                Point[] pts = new Point[] {
                    new Point(t.Right, t.Y),
                    new Point(t.Right, t.Bottom),
                    new Point(t.X, t.Bottom),
                    new Point(t.X, t.Y) };

                path.AddLines(pts);
            }
            else
            {
                int y = Math.Max(t.Bottom - t.Width / 2, t.Y);
                int n = t.Bottom - y;

                Rectangle r = new Rectangle(t.X, y - n, t.Width, n * 2);

                Point[] pts = new Point[] {
                    new Point(t.Right, t.Y),
                    new Point(t.Right, y)};

                path.AddLines(pts);

                if (GaugePointer.BarStyle == BarPointerStyle.Rounded)
                {
                    path.AddArc(r, 0, 180);
                }
                else
                {
                    path.AddLine(new Point(r.Right, r.Y + r.Height / 2),
                                 new Point(r.X + r.Width / 2, r.Bottom));
                }

                pts = new Point[] {
                    new Point(t.X, y),
                    new Point (t.X, t.Y)};

                path.AddLines(pts);
            }
        }

        #endregion

        #region AddTopBulb

        private void AddTopBulb(GaugeLinearScale scale, GraphicsPath path, Rectangle t, int inflate)
        {
            int bulbRadius = (int)(GaugePointer.BulbSize * scale.Bounds.Width);

            if (bulbRadius * 2 < t.Width)
                bulbRadius = t.Width / 2;

            int m = (int)Math.Sqrt((bulbRadius * bulbRadius) - (Width * Width / 4));

            _BulbBounds = new Rectangle(
                t.X - bulbRadius + t.Width / 2,
                t.Y - (bulbRadius + m) + inflate, bulbRadius * 2, bulbRadius * 2);

            if (inflate > 0)
                _BulbBounds.Inflate(inflate, inflate);

            float angle = (GaugePointer.BulbStyle == BulbStyle.Flask)
                ? 90 : (float)(Math.Asin((double)(t.Width / 2) / bulbRadius) * 180 / Math.PI);

            path.AddArc(_BulbBounds, angle + 90, 360 - (angle * 2));
        }

        #endregion

        #endregion

        #endregion

        #region RenderBarByCenter

        protected override void RenderBarByCenter(
            Graphics g, GraphicsPath path, GradientFillColor fillColor)
        {
            using (PathGradientBrush br = new PathGradientBrush(path))
            {
                br.WrapMode = WrapMode.TileFlipXY;

                br.CenterColor = fillColor.Color1;
                br.SurroundColors = new Color[] { fillColor.Color2 };

                br.CenterPoint = new PointF(
                    _BulbBounds.X + _BulbBounds.Width / 2, _BulbBounds.Y + _BulbBounds.Height / 2);

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
                GaugeLinearScale scale = Scale as GaugeLinearScale;

                if (scale != null)
                {
                    if (Bounds.Width > 0 && Bounds.Height > 0)
                        PointerPath = GetBulbBackPath(scale);
                }
            }

            return (PointerPath);
        }

        #endregion
    }

    #region Enums

    public enum BulbStyle
    {
        Bulb,
        Flask,
    }

    #endregion
}
