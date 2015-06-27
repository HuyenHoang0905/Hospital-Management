using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DevComponents.Instrumentation
{
    internal class GaugeMarker : IDisposable
    {
        #region Private variables

        private List<BitmapEntry> _Bitmaps;

        #endregion

        public GaugeMarker()
        {
            _Bitmaps = new List<BitmapEntry>();
        }

        #region Clear

        public void Clear()
        {
            foreach (BitmapEntry entry in _Bitmaps)
                entry.Bitmap.Dispose();

            _Bitmaps.Clear();
        }

        #endregion

        #region FindBitmap

        public Bitmap FindBitmap(GradientFillColor fillColor)
        {
            foreach (BitmapEntry entry in _Bitmaps)
            {
                if (entry.FillColor.IsEqualTo(fillColor) == true)
                    return (entry.Bitmap);
            }

            return (null);
        }

        #endregion

        #region GetMarkerBitmap

        public Bitmap GetMarkerBitmap(Graphics g,
            GaugeMarkerStyle style, GradientFillColor fillColor, int width, int length)
        {
            Bitmap bitmap = FindBitmap(fillColor);

            if (bitmap == null)
            {
                width = Math.Max(width, 3);
                length = Math.Max(length, 3);

                bitmap = new Bitmap(width, length, g);

                _Bitmaps.Add(new BitmapEntry(fillColor, bitmap));

                using (Graphics gBmp = Graphics.FromImage(bitmap))
                {
                    gBmp.SmoothingMode = SmoothingMode.AntiAlias;

                    Rectangle r = new Rectangle();
                    r.Height = length;
                    r.Width = width;

                    using (GraphicsPath path = GetMarkerPath(style, r))
                        FillMarkerPath(gBmp, path, r, style, fillColor);
                }
            }

            return (bitmap);
        }

        #endregion

        #region FillMarkerPath

        internal void FillMarkerPath(Graphics g, GraphicsPath path,
            Rectangle r, GaugeMarkerStyle style, GradientFillColor fillColor)
        {
            GradientFillType fillType = GetMarkerFillType(style, fillColor);

            switch (fillType)
            {
                case GradientFillType.Angle:
                    using (Brush br = fillColor.GetBrush(r))
                    {
                        if (br is LinearGradientBrush)
                            ((LinearGradientBrush)br).WrapMode = WrapMode.TileFlipXY;

                        g.FillPath(br, path);
                    }
                    break;

                case GradientFillType.StartToEnd:
                    using (Brush br = fillColor.GetBrush(r, 90))
                    {
                        if (br is LinearGradientBrush)
                            ((LinearGradientBrush)br).WrapMode = WrapMode.TileFlipXY;

                        g.FillPath(br, path);
                    }
                    break;

                case GradientFillType.HorizontalCenter:
                    r.Height /= 2;

                    if (r.Height <= 0)
                        r.Height = 1;

                    using (LinearGradientBrush br = new
                        LinearGradientBrush(r, fillColor.Start, fillColor.End, 90))
                    {
                        br.WrapMode = WrapMode.TileFlipXY;

                        g.FillPath(br, path);
                    }
                    break;

                case GradientFillType.VerticalCenter:
                    r.Width /= 2;

                    if (r.Width <= 0)
                        r.Width = 1;

                    using (LinearGradientBrush br = new
                        LinearGradientBrush(r, fillColor.Start, fillColor.End, 0f))
                    {
                        br.WrapMode = WrapMode.TileFlipXY;

                        g.FillPath(br, path);
                    }
                    break;

                case GradientFillType.Center:
                    using (PathGradientBrush br = new PathGradientBrush(path))
                    {
                        br.CenterColor = fillColor.Start;
                        br.SurroundColors = new Color[] { fillColor.End };

                        g.FillPath(br, path);
                    }
                    break;

                default:
                    using (Brush br = new SolidBrush(fillColor.Start))
                        g.FillPath(br, path);

                    break;
            }

            if (fillColor.BorderWidth > 0)
            {
                using (Pen pen = new Pen(fillColor.BorderColor, fillColor.BorderWidth))
                {
                    pen.Alignment = PenAlignment.Inset;

                    g.DrawPath(pen, path);
                }
            }
        }

        #endregion

        #region GetMarkerFillType

        private GradientFillType GetMarkerFillType(GaugeMarkerStyle style, GradientFillColor fillColor)
        {
            if (fillColor.End.IsEmpty == true)
                return (GradientFillType.None);

            if (fillColor.GradientFillType == GradientFillType.Auto)
            {
                switch (style)
                {
                    case GaugeMarkerStyle.Circle:
                    case GaugeMarkerStyle.Star5:
                    case GaugeMarkerStyle.Star7:
                        return (GradientFillType.Center);

                    default:
                        return (GradientFillType.VerticalCenter);
                }
            }

            return (fillColor.GradientFillType);
        }

        #endregion

        #region GetMarkerPath

        internal GraphicsPath GetMarkerPath(GaugeMarkerStyle style, Rectangle r)
        {
            r.Inflate(-1, -1);

            if (r.Width > 0 && r.Height > 0)
            {
                switch (style)
                {
                    case GaugeMarkerStyle.Circle:
                        return (GetCirclePath(r));

                    case GaugeMarkerStyle.Diamond:
                        return (GetDiamondPath(r));

                    case GaugeMarkerStyle.Hexagon:
                        return (GetPolygonPath(r, 6, 90));

                    case GaugeMarkerStyle.Pentagon:
                        return (GetPolygonPath(r, 5, 90));

                    case GaugeMarkerStyle.Rectangle:
                        return (GetRectanglePath(r));

                    case GaugeMarkerStyle.Star5:
                        return (GetStarPath(r, 5));

                    case GaugeMarkerStyle.Star7:
                        return (GetStarPath(r, 7));

                    case GaugeMarkerStyle.Trapezoid:
                        return (GetTrapezoidPath(r));

                    case GaugeMarkerStyle.Triangle:
                        return (GetTrianglePath(r));

                    case GaugeMarkerStyle.Wedge:
                        return (GetWedgePath(r));

                    default:
                        return (GetRectanglePath(r));
                }
            }

            return (null);
        }

        #region GetCirclePath

        private GraphicsPath GetCirclePath(Rectangle r)
        {
            GraphicsPath path = new GraphicsPath();

            path.AddEllipse(r);

            return (path);
        }

        #endregion

        #region GetDiamondPath

        private GraphicsPath GetDiamondPath(Rectangle r)
        {
            GraphicsPath path = new GraphicsPath();

            int dx = r.Width / 2;
            int dy = r.Height / 2;

            Point[] pts = {
                new Point(r.Left + dx, r.Top), 
                new Point(r.Right, r.Top + dy),
                new Point(r.Left + dx, r.Bottom),
                new Point(r.Left, r.Top + dy),
            };

            path.AddPolygon(pts);

            path.CloseAllFigures();

            return (path);
        }

        #endregion

        #region GetPolygonPath

        private GraphicsPath GetPolygonPath(Rectangle r, int sides, float angle)
        {
            GraphicsPath path = new GraphicsPath();

            double dx = (double)r.Width / 2;
            double radians = GetRadians(angle);
            double delta = GetRadians((float)360 / sides);

            Point[] pts = new Point[sides];

            for (int i = 0; i < sides; i++)
            {
                pts[i] = new Point(
                  (int)(dx * Math.Cos(radians) + dx + r.X),
                  (int)(dx * Math.Sin(radians) + dx + r.Y));

                radians += delta;
            }

            path.AddPolygon(pts);

            path.CloseAllFigures();

            return (path);
        }

        #endregion

        #region GetRectanglePath

        private GraphicsPath GetRectanglePath(Rectangle r)
        {
            GraphicsPath path = new GraphicsPath();

            path.AddRectangle(r);

            return (path);
        }

        #endregion

        #region GetStarPath

        private GraphicsPath GetStarPath(Rectangle r, int points)
        {
            GraphicsPath path = new GraphicsPath();

            PointF[] pts = new PointF[2 * points];

            double rx1 = r.Width / 2;
            double ry1 = r.Height / 2;

            if (rx1 < 2)
                rx1 = 2;

            if (ry1 < 2)
                ry1 = 2;

            double rx2 = rx1 / 2;
            double ry2 = ry1 / 2;

            double cx = r.X + rx1;
            double cy = r.Y + ry1;

            double theta = GetRadians(90);
            double dtheta = Math.PI / points;

            for (int i = 0; i < 2 * points; i += 2)
            {
                pts[i] = new PointF(
                    (float)(cx + rx1 * Math.Cos(theta)),
                    (float)(cy + ry1 * Math.Sin(theta)));

                theta += dtheta;

                pts[i + 1] = new PointF(
                    (float)(cx + rx2 * Math.Cos(theta)),
                    (float)(cy + ry2 * Math.Sin(theta)));

                theta += dtheta;
            }

            path.AddPolygon(pts);

            path.CloseAllFigures();

            return (path);
        }

        #endregion

        #region GetTrianglePath

        private GraphicsPath GetTrianglePath(Rectangle r)
        {
            GraphicsPath path = new GraphicsPath();

            int dx = r.Width / 2;

            Point[] pts = {
                new Point(r.Left, r.Top), 
                new Point(r.Right, r.Top),
                new Point(r.Left + dx, r.Bottom),
            };

            path.AddPolygon(pts);

            path.CloseAllFigures();

            return (path);
        }

        #endregion

        #region GetTrapezoidPath

        private GraphicsPath GetTrapezoidPath(Rectangle r)
        {
            GraphicsPath path = new GraphicsPath();

            int dx = (int)(r.Width * .28f);

            if (dx <= 0)
                dx = 1;

            Point[] pts = {
                new Point(r.Left, r.Top), 
                new Point(r.Right, r.Top),
                new Point(r.Right - dx, r.Bottom),
                new Point(r.Left + dx, r.Bottom),
            };

            path.AddPolygon(pts);

            path.CloseAllFigures();

            return (path);
        }

        #endregion

        #region GetWedgePath

        private GraphicsPath GetWedgePath(Rectangle r)
        {
            GraphicsPath path = new GraphicsPath();

            int dx = r.Width / 2;

            if ((r.Width % 2) == 1)
                dx++;

            if (dx <= 0)
                dx = 1;

            int dy = (int)Math.Sqrt((r.Width * r.Width) - (dx * dx));

            int y = Math.Max(r.Y, r.Bottom - dy);

            Point[] pts = {
                new Point(r.X, r.Top), 
                new Point(r.Right, r.Top),
                new Point(r.Right, y),
                new Point(r.X + dx, r.Bottom),
                new Point(r.X, y)
            };

            path.AddPolygon(pts);

            path.CloseAllFigures();

            return (path);
        }

        #endregion

        #endregion

        #region GetRadians

        /// <summary>
        /// Converts Degrees to Radians
        /// </summary>
        /// <param name="theta">Degrees</param>
        /// <returns>Radians</returns>
        internal double GetRadians(float theta)
        {
            return (theta * Math.PI / 180);
        }

        #endregion

        #region BitmapEntry

        private class BitmapEntry
        {
            public GradientFillColor FillColor;
            public Bitmap Bitmap;

            public BitmapEntry(GradientFillColor fillColor, Bitmap bitmap)
            {
                FillColor = (GradientFillColor)fillColor.Clone();
                Bitmap = bitmap;
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Clear();
        }

        #endregion
    }

    #region Enums

    public enum GaugeMarkerStyle
    {
        Circle,
        Diamond,
        Hexagon,
        Pentagon,
        Rectangle,
        Star5,
        Star7,
        Trapezoid,
        Triangle,
        Wedge,
        None
    }

    #endregion
}
