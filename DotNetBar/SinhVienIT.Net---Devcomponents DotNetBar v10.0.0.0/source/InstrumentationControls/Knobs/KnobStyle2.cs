using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DevComponents.Instrumentation.Primitives
{
    public class KnobStyle2 : BaseKnob
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="knobControl">Associated knob control</param>
        public KnobStyle2(KnobControl knobControl)
            : base(knobControl)
        {
        }

        #region Knob Configuration

        #region ConfigureKnob

        /// <summary>
        /// Configures the given knob control
        /// by establishing various default object parameters
        /// </summary>
        /// <param name="e"></param>
        public override void ConfigureKnob(PaintEventArgs e)
        {
            base.ConfigureKnob(e);

            // Calculate default sizes and bounding object rectangles

            ZoneIndWidth = KnobWidth * 0.047f;

            MajorTickSize = new Size((int)(KnobWidth * 0.008), (int)(KnobWidth * 0.017));
            MinorTickSize = new Size(1, (int)(KnobWidth * 0.02));

            IndTickHeight = (int)ZoneIndWidth / 3;

            CalculateBoundingRects();

            // Fill in the default knob colors

            DefaultColorTable.MajorTickColor = Color.Black;
            DefaultColorTable.MinorTickColor = Color.Gray;
            DefaultColorTable.ZoneIndicatorColor = Color.SteelBlue;
            DefaultColorTable.KnobIndicatorPointerColor = Color.Black;

            DefaultColorTable.KnobFaceColor = new LinearGradientColorTable(Color.White, Color.LightGray);
            DefaultColorTable.KnobIndicatorColor = new LinearGradientColorTable(0xFFFFFF, 0x1A507B);
            DefaultColorTable.MinZoneIndicatorColor = new LinearGradientColorTable(Color.White, Color.Green);
            DefaultColorTable.MaxZoneIndicatorColor = new LinearGradientColorTable(Color.Yellow, Color.Red);
        }

        #endregion

        #region CalculateBoundingRects

        /// <summary>
        /// Calculates several default control
        /// // bounding rectangles
        /// </summary>
        private void CalculateBoundingRects()
        {
            // Calculate the bounding Zone indicator rectangle and width

            int delta = MaxLabelWidth + MajorTickSize.Height + (int)(KnobWidth * 0.05f);

            ZoneIndicatorBounds = new Rectangle(delta, delta,
                KnobWidth - (delta * 2), KnobWidth - (delta * 2));

            // Calculate the KnobFace and inset face rectangles

            delta += (int)ZoneIndWidth + 1;

            KnobFaceBounds = new Rectangle(delta, delta,
                KnobWidth - (delta * 2), KnobWidth - (delta * 2));

            // Calculate the KnobIndicator bounding rectangle

            delta += (int)(KnobWidth * .068f);

            KnobIndicatorBounds = new Rectangle(delta, delta,
                KnobWidth - (delta * 2), KnobWidth - (delta * 2));

            // Calculate the TickLabel bounding rect

            delta = (int)((MaxLabelWidth + IndTickHeight) * .60f + IndTickHeight);

            TickLabelBounds = new Rectangle(ZoneIndicatorBounds.Location, ZoneIndicatorBounds.Size);
            TickLabelBounds.Inflate(delta, delta);

            // Calculate the focus rectangle

            FocusRectBounds = new Rectangle(KnobFaceBounds.Location, KnobFaceBounds.Size);

            delta = (int)(KnobFaceBounds.Width * .025);

            FocusRectBounds.Inflate(-delta, -delta);
        }

        #endregion

        #endregion

        #region Part rendering code

        #region RenderZoneIndicator

        /// <summary>
        /// Renders the zone indicator
        /// </summary>
        /// <param name="e"></param>
        public override void RenderZoneIndicator(PaintEventArgs e)
        {
            if (ZoneIndicatorBounds.Width > 10 && ZoneIndicatorBounds.Height > 10)
            {
                Graphics g = e.Graphics;

                Color c = ControlPaint.Light(ZoneIndicatorColor, .30f);

                int passes = (int) (ZoneIndWidth);
                int adj = 100 / passes;

                Rectangle r = new Rectangle(ZoneIndicatorBounds.X,
                                            ZoneIndicatorBounds.Y, ZoneIndicatorBounds.Width, ZoneIndicatorBounds.Height);

                r.Inflate(-passes, -passes);

                for (int i = 0; i < passes; i++)
                {
                    using (Pen pen = new Pen(c, 2))
                    {
                        g.DrawEllipse(pen, r);

                        r.Inflate(1, 1);

                        c = Color.FromArgb(Math.Max(0, c.R - adj),
                                           Math.Max(0, c.G - adj), Math.Max(0, c.B - adj));
                    }
                }
            }
        }

        #endregion

        #region RenderTickMinor

        /// <summary>
        /// Renders the minor tick marks
        /// </summary>
        /// <param name="e"></param>
        public override void RenderTickMinor(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            using (Pen pen = new Pen(MinorTickColor, MinorTickSize.Width))
            {
                for (int i = 0; i < MinorTicks; i++)
                {
                    // Don't draw a minor tick if it overlaps
                    // with a previous major tick

                    if (Knob.MajorTickAmount > 0)
                    {
                        if ((i == 0 || i == MinorTicks - 1) ||
                            ((Knob.MinorTickAmount * i) % Knob.MajorTickAmount == 0))
                            continue;
                    }

                    g.DrawLines(pen, GetMinorTickPoints(i));
                }
            }
        }

        /// <summary>
        /// Calculates a series of points
        /// that defines the tick mark
        /// </summary>
        /// <param name="tick">Tick to calculate</param>
        /// <returns>An array of points that defines the tick</returns>
        private Point[] GetMinorTickPoints(int tick)
        {
            float degree = GetTickDegree((float)Knob.MinorTickAmount, tick);
            double rad = GetRadians(degree);

            int dx = ZoneIndicatorBounds.X + ZoneIndicatorBounds.Width / 2;
            int dy = ZoneIndicatorBounds.Y + ZoneIndicatorBounds.Height / 2;
            int h = (int)(ZoneIndicatorBounds.Width / 2 + ZoneIndWidth * .1f);

            Point[] pts = new Point[2];

            pts[0].X = (int)(Math.Cos(rad) * h + dx);
            pts[0].Y = (int)(Math.Sin(rad) * h + dy);

            pts[1].X = (int)(Math.Cos(rad) * (h + MinorTickSize.Height) + dx);
            pts[1].Y = (int)(Math.Sin(rad) * (h + MinorTickSize.Height) + dy);

            return (pts);
        }

        #endregion

        #region RenderKnobFace

        /// <summary>
        /// Renders the knob face
        /// </summary>
        /// <param name="e"></param>
        public override void RenderKnobFace(PaintEventArgs e)
        {
            if (KnobFaceBounds.Width > 10 && KnobFaceBounds.Height > 10)
            {
                Graphics g = e.Graphics;

                // Allocate a path to accumulate the knob
                // drawing pieces

                using (GraphicsPath path = new GraphicsPath())
                {
                    // Draw the 14 arc segments all the way to the
                    // center of the knob just in case the user wants
                    // to render a different sized/shaped knob indicator

                    Rectangle rInset = new Rectangle(KnobFaceBounds.X, KnobFaceBounds.Y,
                                                     KnobFaceBounds.Width, KnobFaceBounds.Height);

                    int w = KnobFaceBounds.Width/2 - 1;

                    rInset.Inflate(-w, -w);

                    const float arc = 360f/14;

                    for (int i = 0; i < 14; i++)
                    {
                        if (i%2 == 0)
                        {
                            path.AddArc(KnobFaceBounds, i*arc, arc);
                            path.AddArc(rInset, (i + 1)*arc, -arc);

                            // Let Windows connect the arcs

                            path.CloseFigure();
                        }
                    }

                    // Select an appropriate hatch size given
                    // the overall size of the control

                    HatchStyle hs = (KnobWidth < 250)
                                        ? HatchStyle.LightUpwardDiagonal
                                        : HatchStyle.WideUpwardDiagonal;

                    // Fill the face and then draw the outlining path

                    using (HatchBrush hbr = new HatchBrush(hs, KnobFaceColor.Start, KnobFaceColor.End))
                        g.FillPath(hbr, path);

                    using (Pen pen = new Pen(KnobFaceColor.End))
                        g.DrawPath(pen, path);
                }
            }
        }

        #endregion

        #region RenderKnobIndicator

        #region RenderKnobIndicator

        /// <summary>
        /// Renders the knob face
        /// </summary>
        /// <param name="e"></param>
        public override void RenderKnobIndicator(PaintEventArgs e)
        {
            if (KnobIndicatorBounds.Width > 10 && KnobIndicatorBounds.Height > 10)
            {
                Graphics g = e.Graphics;

                // Allocate a graphics path and draw the
                // arrow lines into it

                using (GraphicsPath path = new GraphicsPath())
                {
                    path.AddLines(GetIndicatorPoints());
                    path.CloseFigure();

                    // Fill the closed arrow

                    using (SolidBrush br = new SolidBrush(KnobIndicatorPointerColor))
                        g.FillPath(br, path);

                    // Fill the knob face

                    using (Brush br = KnobIndicatorColor.GetBrush(KnobIndicatorBounds))
                        g.FillEllipse(br, KnobIndicatorBounds);

                    // Reset the path and add a glossy looking
                    // hilight to the control face

                    path.Reset();
                    path.AddEllipse(KnobIndicatorBounds);

                    using (PathGradientBrush pgb = new PathGradientBrush(path))
                    {
                        pgb.CenterColor = KnobIndicatorColor.Start;
                        pgb.SurroundColors = new Color[] {KnobIndicatorColor.End};

                        int radius = KnobIndicatorBounds.Width/2;
                        int dr = Math.Max((int) (radius*.1f), 5);

                        float dx = (radius - dr)*(float) Math.Sin(GetRadians(60));
                        float dy = (radius - dr)*(float) Math.Cos(GetRadians(60));

                        pgb.CenterPoint = new PointF(KnobIndicatorBounds.X + radius - dx,
                                                     KnobIndicatorBounds.Y + radius - dy);

                        g.FillRectangle(pgb, KnobIndicatorBounds);
                    }
                }
            }
        }

        #endregion

        #region GetIndicatorPoints

        /// <summary>
        /// Calculates a series of points that
        /// defines the indicator arrow
        /// </summary>
        /// <returns>An array of defining points</returns>
        private Point[] GetIndicatorPoints()
        {
            float degrees = (float)(Knob.SweepAngle / ValueCount) *
                (float)(Knob.Value - Knob.MinValue) + Knob.StartAngle;

            int arrowHeight = (int)(KnobWidth * .052f);

            double rad0 = GetRadians(degrees - 17);
            double rad1 = GetRadians(degrees);
            double rad2 = GetRadians(degrees + 17);

            int n = (int)(KnobIndicatorBounds.Width * .03f);

            if (n < 2)
                n = 2;

            int dx = KnobIndicatorBounds.X + KnobIndicatorBounds.Width / 2 + 1;
            int dy = KnobIndicatorBounds.Y + KnobIndicatorBounds.Height / 2 + 1;
            int h = KnobIndicatorBounds.Width / 2 + n;

            Point[] pts = new Point[3];

            pts[0].X = (int)(Math.Cos(rad0) * h + dx);
            pts[0].Y = (int)(Math.Sin(rad0) * h + dy);

            pts[1].X = (int)(Math.Cos(rad1) * (h + arrowHeight) + dx);
            pts[1].Y = (int)(Math.Sin(rad1) * (h + arrowHeight) + dy);

            pts[2].X = (int)(Math.Cos(rad2) * h + dx);
            pts[2].Y = (int)(Math.Sin(rad2) * h + dy);

            return (pts);
        }

        #endregion

        #endregion

        #endregion

    }
}
