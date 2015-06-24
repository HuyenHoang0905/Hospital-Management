using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DevComponents.Instrumentation
{
    internal class GaugeFrameCircularRenderer : GaugeFrameRenderer
    {
        public GaugeFrameCircularRenderer(GaugeFrame gaugeFrame)
            : base(gaugeFrame)
        {
        }

        #region SetFrameRegion

        internal override void SetFrameRegion()
        {
            if (GaugeFrame.GaugeControl != null)
            {
                using (GraphicsPath path = new GraphicsPath())
                {
                    path.AddEllipse(GaugeFrame.Bounds);

                    GaugeFrame.GaugeControl.Region = new Region(path);
                }
            }
        }

        #endregion

        #region SetBackClipRegion

        internal override void SetBackClipRegion(PaintEventArgs e)
        {
            int inside = GaugeFrame.AbsBevelInside;
            int outside = GaugeFrame.AbsBevelOutside;

            Rectangle r = GaugeFrame.Bounds;
            r.Inflate(-outside -inside, -outside -inside);

            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddEllipse(r);

                e.Graphics.SetClip(path, CombineMode.Intersect);
            }
        }

        #endregion

        #region RenderFrame

        #region RenderFrameByAngle

        protected override void RenderFrameByAngle(PaintEventArgs e, Rectangle r)
        {
            Graphics g = e.Graphics;

            Matrix myMatrix = new Matrix();
            myMatrix.RotateAt(GaugeFrame.FrameColor.GradientAngle, GaugeFrame.Center);

            e.Graphics.Transform = myMatrix;

            using (Brush br = GaugeFrame.FrameColor.GetBrush(GaugeFrame.Bounds, 0))
                g.FillEllipse(br, GaugeFrame.Bounds);

            RenderFrameBorder(g);

            using (Brush br = GaugeFrame.FrameColor.GetBrush(r, 180))
                g.FillEllipse(br, r);

            g.ResetTransform();
        }

        #endregion

        #region RenderFrameByCenter

        protected override void RenderFrameByCenter(PaintEventArgs e, Rectangle r)
        {
            Graphics g = e.Graphics;

            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddEllipse(GaugeFrame.Bounds);

                using (PathGradientBrush br = new PathGradientBrush(path))
                {
                    br.CenterPoint = GaugeFrame.Center;
                    br.CenterColor = GaugeFrame.FrameColor.Start;
                    br.SurroundColors = new Color[] { GaugeFrame.FrameColor.End };

                    br.SetSigmaBellShape(GaugeFrame.FrameSigmaFocus, GaugeFrame.FrameSigmaScale);

                    g.FillEllipse(br, GaugeFrame.Bounds);
                }

                path.AddEllipse(r);

                using (PathGradientBrush br = new PathGradientBrush(path))
                {
                    br.CenterPoint = GaugeFrame.Center;
                    br.CenterColor = GaugeFrame.FrameColor.End;
                    br.SurroundColors = new Color[] { GaugeFrame.FrameColor.Start };

                    g.FillEllipse(br, r);
                }
            }

            RenderFrameBorder(g);
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

                g.FillEllipse(br, GaugeFrame.Bounds);
            }

            t = r;
            t.Height /= 2;
            t.Width /= 2;

            using (Brush br = GaugeFrame.FrameColor.GetBrush(t, angle + 180))
            {
                if (br is LinearGradientBrush)
                    ((LinearGradientBrush)br).WrapMode = WrapMode.TileFlipXY;

                g.FillEllipse(br, r);
            }

            RenderFrameBorder(g);
        }

        #endregion

        #region RenderFrameByNone

        protected override void RenderFrameByNone(PaintEventArgs e, Rectangle r)
        {
            Graphics g = e.Graphics;

            using (Brush br = new SolidBrush(GaugeFrame.FrameColor.Start))
                g.FillEllipse(br, GaugeFrame.Bounds);

            if (GaugeFrame.FrameColor.End.IsEmpty == false)
            {
                using (Brush br = new SolidBrush(GaugeFrame.FrameColor.End))
                    g.FillEllipse(br, r);
            }

            RenderFrameBorder(g);
        }

        #endregion

        #region RenderFrameBorder

        private void RenderFrameBorder(Graphics g)
        {
            if (GaugeFrame.FrameColor.BorderWidth > 0)
            {
                using (Pen pen = new Pen(
                    GaugeFrame.FrameColor.BorderColor, GaugeFrame.FrameColor.BorderWidth))
                {
                    pen.Alignment = PenAlignment.Inset;

                    g.DrawEllipse(pen, GaugeFrame.Bounds);
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

            Matrix myMatrix = new Matrix();
            myMatrix.RotateAt(GaugeFrame.BackColor.GradientAngle, GaugeFrame.Center);

            e.Graphics.Transform = myMatrix;

            using (Brush br = GaugeFrame.BackColor.GetBrush(r, 0))
                g.FillEllipse(br, r);

            g.ResetTransform();

            RenderBackBorder(g, r);
        }

        #endregion

        #region RenderBackByCenter

        protected override void RenderBackByCenter(PaintEventArgs e, Rectangle r)
        {
            Graphics g = e.Graphics;

            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddEllipse(r);

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

                g.FillEllipse(br, r);
            }

            RenderBackBorder(g, r);
        }

        #endregion

        #region RenderBackByNone

        protected override void RenderBackByNone(PaintEventArgs e, Rectangle r)
        {
            Graphics g = e.Graphics;

            using (Brush br = new SolidBrush(GaugeFrame.BackColor.Start))
                g.FillEllipse(br, r);

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

                    g.DrawEllipse(pen, r);
                }
            }
        }

        #endregion

        #endregion

        #region PreRenderContent

        internal override void PreRenderContent(PaintEventArgs e)
        {
            if (GaugeFrame.AddGlassEffect == true)
                AddGlassEffect(e, 45);
        }

        #endregion

        #region PostRenderContent

        internal override void PostRenderContent(PaintEventArgs e)
        {
            if (GaugeFrame.AddGlassEffect == true)
                AddGlassEffect(e, 90);
        }

        #endregion

        #region AddGlassEffect

        private void AddGlassEffect(PaintEventArgs e, float angle)
        {
            Graphics g = e.Graphics;

            Rectangle r = GaugeFrame.BackBounds;
            r.Height /= 2;

            if (r.Height > 0)
            {
                Color color1 = Color.FromArgb(100, Color.White);

                using (LinearGradientBrush br =
                    new LinearGradientBrush(r, color1, Color.Transparent, angle))
                {
                    g.FillEllipse(br, r);
                }
            }
        }

        #endregion
    }
}
