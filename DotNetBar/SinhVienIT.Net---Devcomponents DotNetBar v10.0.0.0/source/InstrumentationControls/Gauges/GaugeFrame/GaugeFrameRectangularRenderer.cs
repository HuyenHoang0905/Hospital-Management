using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DevComponents.Instrumentation
{
    internal class GaugeFrameRectangularRenderer : GaugeFrameRenderer
    {
        public GaugeFrameRectangularRenderer(GaugeFrame gaugeFrame)
            : base(gaugeFrame)
        {
        }

        #region SetBackClipRegion

        internal override void SetBackClipRegion(PaintEventArgs e)
        {
            int inside = GaugeFrame.AbsBevelInside;
            int outside = GaugeFrame.AbsBevelOutside;

            Rectangle r = GaugeFrame.Bounds;
            r.Inflate(-outside - inside, -outside - inside);

            e.Graphics.SetClip(r, CombineMode.Intersect);
        }

        #endregion

        #region RenderFrame

        #region RenderFrameByAngle

        protected override void RenderFrameByAngle(PaintEventArgs e, Rectangle r)
        {
            Graphics g = e.Graphics;

            using (Brush br = GaugeFrame.FrameColor.GetBrush(GaugeFrame.Bounds))
                g.FillRectangle(br, GaugeFrame.Bounds);

            using (Brush br = GaugeFrame.FrameColor.GetBrush(r, GaugeFrame.FrameColor.GradientAngle + 180))
                g.FillRectangle(br, r);

            RenderFrameBorder(g, GaugeFrame.Bounds);
        }

        #endregion

        #region RenderFrameByCenter

        protected override void RenderFrameByCenter(PaintEventArgs e, Rectangle r)
        {
            Graphics g = e.Graphics;

            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddRectangle(GaugeFrame.Bounds);

                using (PathGradientBrush br = new PathGradientBrush(path))
                {
                    br.CenterPoint = GaugeFrame.Center;
                    br.CenterColor = GaugeFrame.FrameColor.Start;
                    br.SurroundColors = new Color[] { GaugeFrame.FrameColor.End };

                    br.SetSigmaBellShape(GaugeFrame.FrameSigmaFocus, GaugeFrame.FrameSigmaScale);

                    g.FillRectangle(br, GaugeFrame.Bounds);
                }

                path.AddRectangle(r);

                using (PathGradientBrush br = new PathGradientBrush(path))
                {
                    br.CenterPoint = GaugeFrame.Center;
                    br.CenterColor = GaugeFrame.FrameColor.End;
                    br.SurroundColors = new Color[] { GaugeFrame.FrameColor.Start };

                    g.FillRectangle(br, r);
                }
            }

            RenderFrameBorder(g, GaugeFrame.Bounds);
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

            using (Brush br = GaugeFrame.FrameColor.GetBrush(t, angle))
            {
                if (br is LinearGradientBrush)
                    ((LinearGradientBrush)br).WrapMode = WrapMode.TileFlipXY;

                g.FillRectangle(br, GaugeFrame.Bounds);
            }

            t = r;
            t.Height /= 2;
            t.Width /= 2;

            using (Brush br = GaugeFrame.FrameColor.GetBrush(t, angle + 180))
            {
                if (br is LinearGradientBrush)
                    ((LinearGradientBrush)br).WrapMode = WrapMode.TileFlipXY;

                g.FillRectangle(br, r);
            }

            RenderFrameBorder(g, GaugeFrame.Bounds);
        }

        #endregion

        #region RenderFrameByNone

        protected override void RenderFrameByNone(PaintEventArgs e, Rectangle r)
        {
            Graphics g = e.Graphics;

            using (Brush br = new SolidBrush(GaugeFrame.FrameColor.Start))
                g.FillRectangle(br, GaugeFrame.Bounds);

            if (GaugeFrame.FrameColor.End.IsEmpty == false)
            {
                using (Brush br = new SolidBrush(GaugeFrame.FrameColor.End))
                    g.FillRectangle(br, r);
            }

            RenderFrameBorder(g, GaugeFrame.Bounds);
        }

        #endregion

        #region RenderFrameBorder

        private void RenderFrameBorder(Graphics g, Rectangle r)
        {
            if (GaugeFrame.FrameColor.BorderWidth > 0)
            {
                using (Pen pen = new Pen(
                    GaugeFrame.FrameColor.BorderColor, GaugeFrame.FrameColor.BorderWidth))
                {
                    pen.Alignment = PenAlignment.Inset;

                    r.Width--;
                    r.Height--;

                    g.DrawRectangle(pen, r);
                }
            }
        }

        #endregion

        #endregion

        #region RenderrBack

        #region RenderBackByAngle

        protected override void RenderBackByAngle(PaintEventArgs e, Rectangle r)
        {
            Graphics g = e.Graphics;

            using (Brush br = GaugeFrame.BackColor.GetBrush(r))
            {
                if (br is LinearGradientBrush)
                    ((LinearGradientBrush)br).WrapMode = WrapMode.TileFlipXY;

                g.FillRectangle(br, r);
            }

            RenderBackBorder(g, r);
        }

        #endregion

        #region RenderBackByCenter

        protected override void RenderBackByCenter(PaintEventArgs e, Rectangle r)
        {
            Graphics g = e.Graphics;

            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddRectangle(r);

                using (PathGradientBrush br = new PathGradientBrush(path))
                {
                    br.CenterColor = GaugeFrame.BackColor.Start;
                    br.SurroundColors = new Color[] { GaugeFrame.BackColor.End };
                    br.CenterPoint = GaugeFrame.Center;

                    br.SetSigmaBellShape(GaugeFrame.BackSigmaFocus, GaugeFrame.BackSigmaScale);

                    g.FillRectangle(br, r);
                }
            }

            RenderBackBorder(g, r);
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

            using (Brush br = GaugeFrame.BackColor.GetBrush(t, angle))
            {
                if (br is LinearGradientBrush)
                    ((LinearGradientBrush)br).WrapMode = WrapMode.TileFlipXY;

                g.FillRectangle(br, r);
            }

            RenderBackBorder(g, r);
        }

        #endregion

        #region RenderBackByNone

        protected override void RenderBackByNone(PaintEventArgs e, Rectangle r)
        {
            Graphics g = e.Graphics;

            using (Brush br = new SolidBrush(GaugeFrame.BackColor.Start))
                g.FillRectangle(br, r);

            RenderBackBorder(g, r);
        }

        #endregion

        #region RenderBackBorder

        private void RenderBackBorder(Graphics g, Rectangle r)
        {
            if (GaugeFrame.BackColor.BorderWidth > 0)
            {
                using (Pen pen = new Pen(
                    GaugeFrame.BackColor.BorderColor, GaugeFrame.BackColor.BorderWidth))
                {
                    pen.Alignment = PenAlignment.Inset;

                    r.Width--;
                    r.Height--;

                    g.DrawRectangle(pen, r);
                }
            }
        }

        #endregion

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
