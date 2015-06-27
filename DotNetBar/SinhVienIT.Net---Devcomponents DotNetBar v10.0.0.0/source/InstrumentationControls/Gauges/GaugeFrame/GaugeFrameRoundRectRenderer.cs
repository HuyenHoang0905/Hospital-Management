using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DevComponents.Instrumentation
{
    internal class GaugeFrameRoundRectRenderer : GaugeFrameRenderer
    {
        public GaugeFrameRoundRectRenderer(GaugeFrame gaugeFrame)
            : base(gaugeFrame)
        {
        }

        #region SetFrameRegion

        internal override void SetFrameRegion()
        {
            if (GaugeFrame.GaugeControl != null)
            {
                using (GraphicsPath path = GetRoundRectPath(GaugeFrame.Bounds))
                    GaugeFrame.GaugeControl.Region = new Region(path);
            }
        }

        #endregion

        #region SetBackClipRegion

        internal override void SetBackClipRegion(PaintEventArgs e)
        {
            int inside = GaugeFrame.AbsBevelInside;
            int outside = GaugeFrame.AbsBevelOutside;

            Rectangle r = GaugeFrame.Bounds;
            r.Inflate(-outside - inside, -outside - inside);

            if (r.Width > 0 && r.Height > 0)
            {
                using (GraphicsPath path = GetRoundRectPath(r))
                    e.Graphics.SetClip(path, CombineMode.Intersect);
            }
        }

        #endregion

        #region RenderFrame

        #region RenderFrameByAngle

        protected override void RenderFrameByAngle(PaintEventArgs e, Rectangle r)
        {
            Graphics g = e.Graphics;

            using (GraphicsPath path = GetRoundRectPath(GaugeFrame.Bounds))
            {
                using (Brush br = GaugeFrame.FrameColor.GetBrush(GaugeFrame.Bounds))
                    g.FillPath(br, path);

                RenderFrameBorder(g, path);
            }

            using (GraphicsPath path = GetRoundRectPath(r))
            {
                using (Brush br = GaugeFrame.FrameColor.GetBrush(r, GaugeFrame.FrameColor.GradientAngle + 180))
                    g.FillPath(br, path);
            }
        }

        #endregion

        #region RenderFrameByCenter

        protected override void RenderFrameByCenter(PaintEventArgs e, Rectangle r)
        {
            Graphics g = e.Graphics;

            using (GraphicsPath path = GetRoundRectPath(GaugeFrame.Bounds))
            {
                using (PathGradientBrush br = new PathGradientBrush(path))
                {
                    br.CenterPoint = GaugeFrame.Center;
                    br.CenterColor = GaugeFrame.FrameColor.Start;
                    br.SurroundColors = new Color[] { GaugeFrame.FrameColor.End };

                    br.SetSigmaBellShape(GaugeFrame.FrameSigmaFocus, GaugeFrame.FrameSigmaScale);

                    g.FillRectangle(br, GaugeFrame.Bounds);
                }

                RenderFrameBorder(g, path);
            }

            using (GraphicsPath path = GetRoundRectPath(r))
            {
                using (PathGradientBrush br = new PathGradientBrush(path))
                {
                    br.CenterPoint = GaugeFrame.Center;
                    br.CenterColor = GaugeFrame.FrameColor.End;
                    br.SurroundColors = new Color[] { GaugeFrame.FrameColor.Start };

                    g.FillRectangle(br, r);
                }
            }
        }

        #endregion

        #region PaintCircularFrameByHvCenter

        protected override void RenderFrameByHorizontalCenter(PaintEventArgs e, Rectangle r)
        {
            RenderFrameByHvCenter(e, r, 90);
        }

        protected override void RenderFrameByVerticalCenter(PaintEventArgs e, Rectangle r)
        {
            RenderFrameByHvCenter(e, r, 0);
        }

        private void RenderFrameByHvCenter(PaintEventArgs e, Rectangle r, int angle)
        {
            Graphics g = e.Graphics;

            Rectangle t = GaugeFrame.Bounds;
            t.Height /= 2;
            t.Width /= 2;

            using (GraphicsPath path = GetRoundRectPath(GaugeFrame.Bounds))
            {
                using (Brush br = GaugeFrame.FrameColor.GetBrush(t, angle))
                {
                    if (br is LinearGradientBrush)
                        ((LinearGradientBrush)br).WrapMode = WrapMode.TileFlipXY;

                    g.FillPath(br, path);
                }

                RenderFrameBorder(g, path);
            }

            t = r;
            t.Height /= 2;
            t.Width /= 2;

            using (GraphicsPath path = GetRoundRectPath(r))
            {
                using (Brush br = GaugeFrame.FrameColor.GetBrush(t, angle + 180))
                {
                    if (br is LinearGradientBrush)
                        ((LinearGradientBrush)br).WrapMode = WrapMode.TileFlipXY;

                    g.FillPath(br, path);
                }
            }
        }

        #endregion

        #region RenderFrameByNone

        protected override void RenderFrameByNone(PaintEventArgs e, Rectangle r)
        {
            Graphics g = e.Graphics;

            using (GraphicsPath path = GetRoundRectPath(GaugeFrame.Bounds))
            {
                using (Brush br = new SolidBrush(GaugeFrame.FrameColor.Start))
                    g.FillPath(br, path);

                RenderFrameBorder(g, path);
            }

            if (GaugeFrame.FrameColor.End.IsEmpty == false)
            {
                using (GraphicsPath path = GetRoundRectPath(r))
                {
                    using (Brush br = new SolidBrush(GaugeFrame.FrameColor.End))
                        g.FillPath(br, path);
                }
            }
        }

        #endregion

        #region RenderFrameBorder

        private void RenderFrameBorder(Graphics g, GraphicsPath path)
        {
            if (GaugeFrame.FrameColor.BorderWidth > 0)
            {
                using (Pen pen = new Pen(
                    GaugeFrame.FrameColor.BorderColor, GaugeFrame.FrameColor.BorderWidth))
                {
                    pen.Alignment = PenAlignment.Inset;

                    g.DrawPath(pen, path);
                }
            }
        }

        #endregion

        #endregion

        #region RenderBack

        #region RenderBackByAngle

        protected override void RenderBackByAngle(PaintEventArgs e, Rectangle r)
        {
            Graphics g = e.Graphics;

            using (GraphicsPath path = GetRoundRectPath(r))
            {
                using (Brush br = GaugeFrame.BackColor.GetBrush(r))
                {
                    if (br is LinearGradientBrush)
                        ((LinearGradientBrush)br).WrapMode = WrapMode.TileFlipXY;

                    g.FillPath(br, path);

                    RenderBackBorder(g, path);
                }
            }
        }

        #endregion

        #region RenderBackByCenter

        protected override void RenderBackByCenter(PaintEventArgs e, Rectangle r)
        {
            Graphics g = e.Graphics;

            using (GraphicsPath path = GetRoundRectPath(r))
            {
                using (PathGradientBrush br = new PathGradientBrush(path))
                {
                    br.CenterColor = GaugeFrame.BackColor.Start;
                    br.SurroundColors = new Color[] { GaugeFrame.BackColor.End };
                    br.CenterPoint = GaugeFrame.Center;

                    br.SetSigmaBellShape(GaugeFrame.BackSigmaFocus, GaugeFrame.BackSigmaScale);

                    g.FillRectangle(br, r);
                }

                RenderBackBorder(g, path);
            }
        }

        #endregion

        #region RenderBackByHvCenter

        protected override void RenderBackByHorizontalCenter(PaintEventArgs e, Rectangle r)
        {
            RenderBackByHvCenter(e, r, 90);
        }

        protected override void RenderBackByVerticalCenter(PaintEventArgs e, Rectangle r)
        {
            RenderBackByHvCenter(e, r, 0);
        }

        private void RenderBackByHvCenter(PaintEventArgs e, Rectangle r, int angle)
        {
            Graphics g = e.Graphics;

            Rectangle t = r;
            t.Height /= 2;
            t.Width /= 2;

            using (GraphicsPath path = GetRoundRectPath(r))
            {
                using (Brush br = GaugeFrame.BackColor.GetBrush(t, angle))
                {
                    if (br is LinearGradientBrush)
                        ((LinearGradientBrush)br).WrapMode = WrapMode.TileFlipXY;

                    g.FillPath(br, path);
                }

                RenderBackBorder(g, path);
            }
        }

        #endregion

        #region RenderBackByNone

        protected override void RenderBackByNone(PaintEventArgs e, Rectangle r)
        {
            Graphics g = e.Graphics;

            using (GraphicsPath path = GetRoundRectPath(r))
            {
                using (Brush br = new SolidBrush(GaugeFrame.BackColor.Start))
                    g.FillPath(br, path);

                RenderBackBorder(g, path);
            }
        }

        #endregion

        #region RenderBackBorder

        private void RenderBackBorder(Graphics g, GraphicsPath path)
        {
            if (GaugeFrame.BackColor.BorderWidth > 0)
            {
                using (Pen pen = new Pen(
                    GaugeFrame.BackColor.BorderColor, GaugeFrame.BackColor.BorderWidth))
                {
                    pen.Alignment = PenAlignment.Inset;

                    g.DrawPath(pen, path);
                }
            }
        }

        #endregion

        #endregion

        #region GetRoundRectPath

        private GraphicsPath GetRoundRectPath(Rectangle r)
        {
            GraphicsPath path = new GraphicsPath();

            int m = Math.Min(r.Width, r.Height);
            int n = (int)(m * GaugeFrame.RoundRectangleArc) + 1;

            Rectangle t = new Rectangle(r.Right - n, r.Bottom - n, n, n);
            path.AddArc(t, 0, 90);

            t.X = r.X;
            path.AddArc(t, 90, 90);

            t.Y = r.Y;
            path.AddArc(t, 180, 90);

            t.X = r.Right - n;
            path.AddArc(t, 270, 90);

            path.CloseAllFigures();

            return (path);
        }

        #endregion

        #region PreRenderContent

        internal override void PreRenderContent(PaintEventArgs e)
        {
            if (GaugeFrame.AddGlassEffect == true)
                AddGlassEffect(e, 2.5f, 45);
        }

        #endregion

        #region PostRenderContent

        internal override void PostRenderContent(PaintEventArgs e)
        {
            if (GaugeFrame.AddGlassEffect == true)
                AddGlassEffect(e, 2, 90);
        }

        #endregion

        #region AddGlassEffect

        private void AddGlassEffect(PaintEventArgs e, float f, float angle)
        {
            Graphics g = e.Graphics;

            Rectangle r = GaugeFrame.BackBounds;

            float x = Math.Max(r.Height, r.Width);

            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddLines(new Point[] {
                    new Point(r.X, r.Y),
                    new Point(r.X, (int)(r.Y + x / f)),
                    new Point(r.X + (int)(x / f), r.Y)});

                path.CloseFigure();

                Color color1 = Color.FromArgb(40, Color.White);

                using (LinearGradientBrush br =
                    new LinearGradientBrush(r, color1, Color.Transparent, angle))
                {
                    g.FillPath(br, path);
                }
            }
        }

        #endregion

    }
}

