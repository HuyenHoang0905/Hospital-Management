using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DevComponents.Instrumentation
{
    internal class GaugeNeedleRenderer : GaugePointerRenderer, IDisposable
    {
        #region Private variables

        private int _CapWidth;
        private Rectangle _CapBounds;

        #endregion

        internal GaugeNeedleRenderer(GaugePointer gaugePointer)
            : base(gaugePointer)
        {
        }

        #region Public properties

        #region CapBounds

        public Rectangle CapBounds
        {
            get { return (_CapBounds); }
        }

        #endregion

        #endregion

        #region RecalcLayout

        public override void RecalcLayout()
        {
            base.RecalcLayout();

            CalcMarkerPoint();
        }

        #region CalcMarkerPoint

        private void CalcMarkerPoint()
        {
            if (Scale is GaugeCircularScale)
                CalcCircularMarkerPoint(Scale as GaugeCircularScale);
        }

        #region CalcCircularMarkerPoint

        private void CalcCircularMarkerPoint(GaugeCircularScale scale)
        {
            Dpt = scale.SweepAngle / scale.Spread;

            double marker = GetInterval(Value) - scale.MinValue;

            IntervalAngle = (float)(scale.Reversed
                ? scale.StartAngle + scale.SweepAngle - (marker * Dpt)
                : scale.StartAngle + (marker * Dpt));

            IntervalPoint = scale.GetPoint(Radius, IntervalAngle);

            _CapWidth = (int)(scale.AbsRadius * GaugePointer.CapWidth);

            if (_CapWidth % 2 != 0)
                _CapWidth++;

            int n = _CapWidth / 2;

            _CapBounds = new Rectangle(
                scale.Center.X - n, scale.Center.Y - n, _CapWidth, _CapWidth);
        }

        #endregion

        #endregion

        #endregion

        #region RenderCircular

        public override void RenderCircular(PaintEventArgs e)
        {
            if (IntervalPoint.IsEmpty == false && Radius > 0)
            {
                Graphics g = e.Graphics;

                if (GaugePointer.CapOnTop == false)
                    RenderCap(g);

                RenderNeedle(g);

                if (GaugePointer.CapOnTop == true)
                    RenderCap(g);
            }
        }

        #region RenderNeedle

        #region RenderNeedle

        private void RenderNeedle(Graphics g)
        {
            if (GaugePointer.NeedleStyle != NeedlePointerStyle.None)
            {
                g.TranslateTransform(IntervalPoint.X, IntervalPoint.Y);
                g.RotateTransform((IntervalAngle + 90) % 360);

                int length = Math.Max(Radius, Radius + Length);

                if (Width > 0 && length > 0)
                {
                    Rectangle r = new Rectangle(0, 0, Width, length);
                    r.X -= Width/2;

                    if (GaugePointer.Image != null)
                    {
                        g.DrawImage(GaugePointer.Image, r);
                    }
                    else
                    {
                        GradientFillColor fillColor = GaugePointer.FillColorEx;

                        if (fillColor != null)
                        {
                            GradientFillType fillType = (fillColor.GradientFillType == GradientFillType.Auto)
                                                            ? GradientFillType.Center
                                                            : fillColor.GradientFillType;

                            using (GraphicsPath path = GetNeedlePath(r))
                            {
                                RenderFill(g, r, path, fillColor, fillType, 0, false);
                                RenderBorder(g, path, fillColor);
                            }
                        }
                    }
                }

                g.ResetTransform();
            }
        }

        #endregion

        #region GetNeedlePath

        private GraphicsPath GetNeedlePath(Rectangle r)
        {
            GaugeControl gc = GaugePointer.Scale.GaugeControl;

            return (gc.OnGetPointerPath(GaugePointer, r) ?? GetNeedlePathEx(r));
        }

        private GraphicsPath GetNeedlePathEx(Rectangle r)
        {
            switch (GaugePointer.NeedleStyle)
            {
                case NeedlePointerStyle.Style1:
                    return (GetNeedlePathStyle1(r));

                case NeedlePointerStyle.Style2:
                    return (GetNeedlePathStyle2(r));

                case NeedlePointerStyle.Style3:
                    return (GetNeedlePathStyle3(r));

                case NeedlePointerStyle.Style4:
                    return (GetNeedlePathStyle4(r));

                case NeedlePointerStyle.Style5:
                    return (GetNeedlePathStyle5(r));

                case NeedlePointerStyle.Style6:
                    return (GetNeedlePathStyle6(r));

                case NeedlePointerStyle.Style7:
                    return (GetNeedlePathStyle7(r));

                case NeedlePointerStyle.Style8:
                    return (GetNeedlePathStyle8(r));

                default:
                    return (GetNeedlePathStyle1(r));
            }
        }

        #region GetNeedlePathStyle1

        private GraphicsPath GetNeedlePathStyle1(Rectangle r)
        {
            Point[] pts = new Point[]
            {
                new Point(r.Left, r.Bottom),
                new Point(r.Left + (r.Width / 2), r.Top),
                new Point(r.Right, r.Bottom),
            };

            GraphicsPath path = new GraphicsPath();

            path.AddLines(pts);
            path.CloseFigure();

            return (path);
        }

        #endregion

        #region GetNeedlePathStyle2

        private GraphicsPath GetNeedlePathStyle2(Rectangle r)
        {
            GraphicsPath path = new GraphicsPath();

            path.AddRectangle(r);

            return (path);
        }

        #endregion

        #region GetNeedlePathStyle3

        private GraphicsPath GetNeedlePathStyle3(Rectangle r)
        {
            GraphicsPath path = new GraphicsPath();
            Rectangle t = r;

            t.Height = t.Width;
            path.AddArc(t, 180, 180);

            t.Y = r.Bottom - t.Height;
            path.AddArc(t, 0, 180);

            path.CloseFigure();

            return (path);
        }

        #endregion

        #region GetNeedlePathStyle4

        private GraphicsPath GetNeedlePathStyle4(Rectangle r)
        {
            GraphicsPath path = new GraphicsPath();

            Rectangle t = r;
            t.Width = t.Width / 2;
            t.Height = t.Width;
            t.X += t.Width / 2;

            path.AddArc(t, 180, 180);
            path.AddLine(r.Right, r.Bottom, r.Left, r.Bottom);

            path.CloseFigure();

            return (path);
        }

        #endregion

        #region GetNeedlePathStyle5

        private GraphicsPath GetNeedlePathStyle5(Rectangle r)
        {
            int n = r.Width / 4;

            Point[] pts = new Point[]
            {
                new Point(r.Left + n, r.Top + r.Width),
                new Point(r.Left + r.Width / 2, r.Top),
                new Point(r.Right - n, r.Top + r.Width)
            };

            GraphicsPath path = new GraphicsPath();

            path.AddLines(pts);
            path.AddLine(r.Right, r.Bottom, r.Left, r.Bottom);

            path.CloseFigure();

            return (path);
        }

        #endregion

        #region GetNeedlePathStyle6

        private GraphicsPath GetNeedlePathStyle6(Rectangle r)
        {
            if (r.Height <= Radius)
                return (GetNeedlePathStyle1(r));

            GraphicsPath path = new GraphicsPath();

            Point[] pts = new Point[]
            {
                new Point(r.Left, r.Y + Radius),
                new Point(r.Left + (r.Width / 2), r.Top),
                new Point(r.Right, r.Y + Radius),
                new Point(r.Left + (r.Width / 2), r.Bottom),
                new Point(r.Left, r.Y + Radius),
            };

            path.AddLines(pts);

            return (path);
        }

        #endregion

        #region GetNeedlePathStyle7

        private GraphicsPath GetNeedlePathStyle7(Rectangle r)
        {
            if (r.Height <= Radius)
                return (GetNeedlePathStyle1(r));

            GraphicsPath path = new GraphicsPath();

            Point[] pts = 
            {
                new Point(r.X + r.Width / 2, r.Y),
                new Point(r.Right, r.Y + (r.Height / 3)),
                new Point(r.Right - (r.Width / 4), r.Y + (r.Height / 3)),
                new Point(r.Right, r.Bottom),
                new Point(r.X + r.Width / 2, r.Bottom - r.Width / 2),
                new Point(r.X, r.Bottom),
                new Point(r.X + r.Width / 2, r.Y),
            };

            path.AddLines(pts);

            return (path);
        }

        #endregion

        #region GetNeedlePathStyle8

        private GraphicsPath GetNeedlePathStyle8(Rectangle r)
        {
            if (r.Height <= Radius)
                return (GetNeedlePathStyle1(r));

            GraphicsPath path = new GraphicsPath();

            Point[] pts = {
                    new Point(r.X + r.Width / 2, r.Y),
                    new Point(r.Right, r.Y + (r.Height / 3)),
                    new Point(r.Right - (r.Width / 4), r.Y + (r.Height / 3)),
                    new Point(r.Right, r.Bottom),
                    new Point(r.X + r.Width / 2, r.Bottom - r.Width / 2),
                    new Point(r.X, r.Bottom),
                    new Point(r.X + (r.Width / 4), r.Y + (r.Height / 3)),
                    new Point(r.X, r.Y + (r.Height / 3)),
                };

            path.AddLines(pts);

            return (path);
        }

        #endregion

        #endregion

        #endregion

        #region RenderCap

        #region RenderCap

        public void RenderCap(Graphics g)
        {
            if (_CapWidth > 2)
            {
                if (GaugePointer.CapImage != null)
                    RenderCapImage(g);
                else
                    RenderCapStyle(g);
            }
        }

        #region RenderCapStyle

        private void RenderCapStyle(Graphics g)
        {
            if (GaugePointer.CapStyle != NeedlePointerCapStyle.None)
            {
                GradientFillColor fillColor = GaugePointer.CapFillColorEx;

                if (fillColor != null)
                {
                    GradientFillType fillType = (fillColor.GradientFillType == GradientFillType.Auto)
                                                    ? GradientFillType.Angle
                                                    : fillColor.GradientFillType;

                    if (fillType == GradientFillType.Angle && fillColor.GradientAngle % 90 != 0)
                    {
                        Matrix myMatrix = new Matrix();
                        myMatrix.RotateAt(fillColor.GradientAngle, Scale.Center);

                        g.Transform = myMatrix;
                    }

                    switch (GaugePointer.CapStyle)
                    {
                        case NeedlePointerCapStyle.Style1:
                            RenderCapPathSytle1(g, _CapBounds, fillColor, fillType);
                            break;

                        case NeedlePointerCapStyle.Style2:
                            RenderCapPathSytle2(g, _CapBounds, fillColor, fillType);
                            break;
                    }

                    g.ResetTransform();
                }
            }
        }

        #endregion

        #region RenderCapImage

        private void RenderCapImage(Graphics g)
        {
            if (GaugePointer.RotateCap == true)
            {
                g.TranslateTransform(Scale.Center.X, Scale.Center.Y);
                g.RotateTransform((IntervalAngle + 90) % 360);

                Rectangle r = _CapBounds;
                r.Location = new Point(-_CapBounds.Width / 2, -_CapBounds.Height / 2);

                g.DrawImage(GaugePointer.CapImage, r);

                g.ResetTransform();
            }
            else
            {
                g.DrawImage(GaugePointer.CapImage, _CapBounds);
            }
        }

        #endregion

        #endregion

        #region RenderCapPathSytle1

        private void RenderCapPathSytle1(Graphics g,
            Rectangle r, GradientFillColor fillColor, GradientFillType fillType)
        {
            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddEllipse(r);

                RenderFill(g, r, path, fillColor, fillType, 0, true);
                RenderBorder(g, path, fillColor);
            }
        }

        #endregion

        #region RenderCapPathSytle2

        private void RenderCapPathSytle2(Graphics g,
            Rectangle r, GradientFillColor fillColor, GradientFillType fillType)
        {
            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddEllipse(r);

                RenderFill(g, r, path, fillColor, fillType, 0, true);
                RenderBorder(g, path, fillColor);

                path.Reset();

                int angle = 180;
                int outer = (int)(r.Width * GaugePointer.CapOuterBevel);

                if (outer > 0)
                {
                    r.Inflate(-outer, -outer);

                    if (r.Width >= 2)
                    {
                        path.AddEllipse(r);
                        RenderFill(g, r, path, fillColor, fillType, 180, true);

                        path.Reset();

                        angle = 0;
                    }
                }

                int inner = (int) (r.Width*GaugePointer.CapInnerBevel);

                if (inner > 0)
                {
                    r.Inflate(-inner, -inner);

                    if (r.Width >= 2)
                    {
                        path.AddEllipse(r);
                        RenderFill(g, r, path, fillColor, fillType, angle, true);
                    }
                }
            }
        }

        #endregion

        #endregion

        #region RenderFill

        private void RenderFill(Graphics g, Rectangle r, GraphicsPath path,
            GradientFillColor fillColor, GradientFillType fillType, int angle, bool offset)
        {
            if (fillColor.Color2.IsEmpty)
                fillType = GradientFillType.None;

            switch (fillType)
            {
                case GradientFillType.Auto:
                case GradientFillType.Angle:
                    if (offset == false)
                        angle += fillColor.GradientAngle;

                    using (Brush br = fillColor.GetBrush(r, angle))
                    {
                        if (br is LinearGradientBrush)
                            ((LinearGradientBrush)br).WrapMode = WrapMode.TileFlipXY;

                        g.FillPath(br, path);
                    }
                    break;

                case GradientFillType.StartToEnd:
                    using (Brush br = fillColor.GetBrush(r, 90 + angle))
                    {
                        if (br is LinearGradientBrush)
                            ((LinearGradientBrush)br).WrapMode = WrapMode.TileFlipXY;

                        g.FillPath(br, path);
                    }
                    break;

                case GradientFillType.HorizontalCenter:
                    if (r.Height >= 2)
                        r.Height /= 2;

                    using (LinearGradientBrush br = new
                        LinearGradientBrush(r, fillColor.Start, fillColor.End, 90 + angle))
                    {
                        br.WrapMode = WrapMode.TileFlipXY;

                        g.FillPath(br, path);
                    }
                    break;

                case GradientFillType.VerticalCenter:
                    if (r.Width >= 2)
                        r.Width /= 2;

                    using (LinearGradientBrush br = new
                        LinearGradientBrush(r, fillColor.Start, fillColor.End, 0f + angle))
                    {
                        br.WrapMode = WrapMode.TileFlipXY;

                        g.FillPath(br, path);
                    }
                    break;

                case GradientFillType.Center:
                    using (PathGradientBrush br = new PathGradientBrush(path))
                    {
                        if (offset == true && Scale is GaugeCircularScale)
                            br.CenterPoint = ((GaugeCircularScale)Scale).GetPoint((int) (r.Width * .45f), 180 + 45 + angle);

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
        }

        #endregion

        #region RenderBorder

        private void RenderBorder(
            Graphics g, GraphicsPath path, GradientFillColor fillColor)
        {
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

        #endregion

        #region RenderLinear

        public override void RenderLinear(PaintEventArgs e)
        {
        }

        #endregion

        #region GetPointerPath

        public override GraphicsPath GetPointerPath()
        {
            if (PointerPath == null)
            {
                if (GaugePointer.NeedleStyle != NeedlePointerStyle.None)
                {
                    int length = Math.Max(Radius, Radius + Length);

                    if (Width > 0 && length > 0)
                    {
                        Rectangle r = new Rectangle(IntervalPoint.X, IntervalPoint.Y, Width, length);
                        r.X -= Width / 2;

                        PointerPath = GetNeedlePath(r);

                        Matrix matrix = new Matrix();
                        matrix.RotateAt((IntervalAngle + 90) % 360, IntervalPoint);

                        PointerPath.Transform(matrix);
                    }
                }
            }

            return (PointerPath);
        }

        #endregion

        #region OnMouseDown

        internal override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            GaugePointer.MouseDownAngle = IntervalAngle;
            GaugePointer.MouseDownRadians = GetPointRadians(e.Location);
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            PointerPath = null;
        }

        #endregion
    }

    #region Enums

    public enum NeedlePointerStyle
    {
        None,
        Style1,
        Style2,
        Style3,
        Style4,
        Style5,
        Style6,
        Style7,
        Style8
    }

    public enum NeedlePointerCapStyle
    {
        None,
        Style1,
        Style2,
    }

    #endregion
}
