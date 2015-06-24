using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DevComponents.Instrumentation.Primitives
{
    public class KnobStyle3 : BaseKnob
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="knobControl">Associated knob control</param>
        public KnobStyle3(KnobControl knobControl)
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

            ZoneIndWidth = KnobWidth * 0.034f;

            MajorTickSize = new Size((int)(KnobWidth * 0.018), (int)(KnobWidth * 0.05));
            MinorTickSize = new Size(MajorTickSize.Width / 2, MajorTickSize.Height);

            IndTickHeight = MajorTickSize.Height / 3;

            CalculateBoundingRects();

            // Fill in the default knob colors

            DefaultColorTable.MajorTickColor = Color.Black;
            DefaultColorTable.MinorTickColor = Color.Black;
            DefaultColorTable.ZoneIndicatorColor = Color.Black;
            DefaultColorTable.KnobIndicatorPointerColor = Color.Black;

            DefaultColorTable.KnobFaceColor = new LinearGradientColorTable(Color.Goldenrod, Color.Goldenrod, 40);
            DefaultColorTable.KnobIndicatorColor = new LinearGradientColorTable(Color.White, Color.LightGray);
            DefaultColorTable.MinZoneIndicatorColor = new LinearGradientColorTable(Color.White, Color.Green);
            DefaultColorTable.MaxZoneIndicatorColor = new LinearGradientColorTable(Color.Yellow, Color.Red);
            DefaultColorTable.MidZoneIndicatorColor = new LinearGradientColorTable(Color.CornflowerBlue);
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

            int delta = MaxLabelWidth + MajorTickSize.Height;

            ZoneIndicatorBounds = new Rectangle(delta, delta,
                KnobWidth - (delta * 2), KnobWidth - (delta * 2));

            // Calculate the KnobFace and inset face rectangles

            delta = (int)(ZoneIndicatorBounds.Width * .025f + ZoneIndWidth);

            KnobFaceBounds = ZoneIndicatorBounds;
            KnobFaceBounds.Inflate(-delta, -delta);

            // Calculate the KnobIndicator bounding rectangle

            delta = (int)(ZoneIndicatorBounds.Width * .14f);

            KnobIndicatorBounds = ZoneIndicatorBounds;
            KnobIndicatorBounds.Inflate(-delta, -delta);

            // Calculate the TickLabel bounding rect

            delta = (int)((MaxLabelWidth + IndTickHeight) * .60f);

            TickLabelBounds = new Rectangle(ZoneIndicatorBounds.Location, ZoneIndicatorBounds.Size);
            TickLabelBounds.Inflate(delta, delta);

            // Calculate the focus rectangle

            FocusRectBounds = KnobIndicatorBounds;

            delta = (int)(KnobIndicatorBounds.Width * .025);
            FocusRectBounds.Inflate(-delta, -delta);
        }

        #endregion

        #endregion

        #region Part rendering code

        #region RenderZoneIndicator

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

                int leftEndAngle = Knob.StartAngle;
                int rightStartAngle = Knob.StartAngle;

                // Render the left zone - if present

                if (Knob.MinZonePercentage > 0)
                {
                    float pct = Knob.MinZonePercentage / 100f;

                    RenderArc(g, Knob.StartAngle,
                              Knob.SweepAngle * pct, LeftZoneIndicatorColor);

                    leftEndAngle = (int)(Knob.StartAngle + Knob.SweepAngle * pct);
                }
                
                // Render the right-hand zone - if present

                if (Knob.MaxZonePercentage > 0)
                {
                    float pct = Knob.MaxZonePercentage / 100f;

                    RenderArc(g, Knob.StartAngle + Knob.SweepAngle * (1 - pct),
                              Knob.SweepAngle * pct, RightZoneIndicatorColor);

                    rightStartAngle = (int)(Knob.StartAngle + Knob.SweepAngle * (1 - pct));
                }

                // Render the middle zone

                if (rightStartAngle != leftEndAngle)
                {
                    RenderArc(g, leftEndAngle,
                        rightStartAngle - leftEndAngle, MiddleZoneIndicatorColor);
                }
            }
        }

        #endregion

        #region RenderArc

        /// <summary>
        /// Renders a gradient indicator arc by dividing
        /// the arc into sub-arcs, enabling us to utilize normal
        /// rectangle gradient support
        /// </summary>
        /// <param name="g"></param>
        /// <param name="a1">Starting angle</param>
        /// <param name="s1">Sweep angle</param>
        /// <param name="ct"></param>
        private void RenderArc(Graphics g, float a1, float s1, LinearGradientColorTable ct)
        {
            float n1 = Math.Abs(s1);

            Color c1 = ct.Start;
            Color c2 = ct.End.IsEmpty == false ? ct.End : ct.Start;

            // Calculate our rect and inflate to
            // make room for the indicator arc

            Rectangle rect = new
                Rectangle(ZoneIndicatorBounds.Location, ZoneIndicatorBounds.Size);

            int dw = (int)(ZoneIndWidth / 2);

            rect.Inflate(-dw, -dw);

            // Calculate the RGB color deltas

            float dr = (c2.R - c1.R) / n1;
            float dg = (c2.G - c1.G) / n1;
            float db = (c2.B - c1.B) / n1;

            // Set our initial starting color and range

            Color c3 = Color.FromArgb(c1.ToArgb());

            float s2 = s1;
            float a2 = a1;

            int pa = (s1 > 0) ? 100 : -100;

            // Loop through the arc, processing sub-arcs less
            // than 180 degrees so that we can use GDI+ built-in
            // gradient rectangle support

            while (s2 != 0)
            {
                // Limit our sweep angle pass to 90 degrees

                float sw = (s2 > 0) ? 
                    Math.Min(s2, 90) : Math.Max(s2, -90);

                // Calculate our sub-sweep angle points.  This
                // enables us to create an associated bounding
                // rectangle for the sub-sweep arc

                Point pt1 = CalcCoord(a2);
                Point pt2 = CalcCoord(a2 + sw);

                Rectangle r = new Rectangle();

                r.Location = new Point(Math.Min(pt1.X, pt2.X), Math.Min(pt1.Y, pt2.Y));
                r.Size = new Size(Math.Abs(pt1.X - pt2.X), Math.Abs(pt1.Y - pt2.Y));

                if (r.Width == 0)
                    r.Width = 1;

                if (r.Height == 0)
                    r.Height = 1;

                // Calculate the terminal color for the
                // sub-sweep arc

                float n = Math.Abs(sw);

                int red = (int)(c3.R + n * dr);
                red = Math.Max(Math.Min(red, 255), 0);

                int green = (int)(c3.G + n * dg);
                green = Math.Max(Math.Min(green, 255), 0);

                int blue = (int)(c3.B + n * db);
                blue = Math.Max(Math.Min(blue, 255), 0);
                
                Color c4 = Color.FromArgb(red, green, blue);

                // Tally up this sub-sweep

                s2 -= sw;

                // Render the sub-arc with an appropriately
                // orientated gradient brush, and draw the arc

                using (LinearGradientBrush lbr = new LinearGradientBrush(r, c3, c4, a2 + pa))
                {
                    using (Pen pen = new Pen(lbr, (int)ZoneIndWidth))
                        g.DrawArc(pen, rect, a2, sw);
                }

                // Bump up our starting angle to reflect the
                // processing of this sub-arc

                a2 += sw;

                // Set the next starting color to the
                // ending color for this arc

                c3 = c4;
            }

            // Render the bounding indicator arcs

            using (GraphicsPath p1 = new GraphicsPath())
            {
                rect.Inflate(-dw, -dw);

                p1.AddArc(ZoneIndicatorBounds, a1, s1);
                p1.AddArc(rect, a1 + s1, -s1);

                // Let Windows close the arcs, and then
                // render them

                p1.CloseFigure();

                using (Pen pen = new Pen(ZoneIndicatorColor, MinorTickSize.Width))
                    g.DrawPath(pen, p1);
            }
        }

        #endregion

        #region CalcCoord

        /// <summary>
        /// Calculates the arc coordinates for 
        /// a given angle
        /// </summary>
        /// <param name="a2">Angle</param>
        /// <returns></returns>
        private Point CalcCoord(float a2)
        {
            Point pt = new Point();

            // Normalize the angle and calculate some
            // working vars

            if (a2 < 0)
                a2 += 360;

            a2 = a2 % 360;

            int delta = ZoneIndicatorBounds.X + ZoneIndicatorBounds.Width / 2;
            int radius = ZoneIndicatorBounds.Width / 2;
            int d = (int)(ZoneIndWidth / 2);

            // Determine the angle quadrant, and then calculate
            // the intersecting coordinate accordingly

            if (a2 < 90)
            {
                pt.X = (int)(Math.Cos(GetRadians(a2 % 90)) * (radius + d));
                pt.Y = (int)(Math.Sin(GetRadians(a2 % 90)) * (radius + d));
            }
            else if (a2 < 180)
            {
                pt.X = -(int)(Math.Sin(GetRadians(a2 % 90)) * (radius + d));
                pt.Y = (int)(Math.Cos(GetRadians(a2 % 90)) * (radius + d));
            }
            else if (a2 < 270)
            {
                pt.X = -(int)(Math.Cos(GetRadians(a2 % 90)) * (radius + d));
                pt.Y = -(int)(Math.Sin(GetRadians(a2 % 90)) * (radius + d));
            }
            else
            {
                pt.X = (int)(Math.Sin(GetRadians(a2 % 90)) * (radius + d));
                pt.Y = -(int)(Math.Cos(GetRadians(a2 % 90)) * (radius + d));
            }

            // Adjust the point to the intersecting arc

            pt.X += delta;
            pt.Y += delta;

            return (pt);
        }

        #endregion

        #endregion

        #region RenderTickMinor

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

        #endregion

        #region GetMinorTickPoints

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
            int h = ZoneIndicatorBounds.Width / 2 - (int)ZoneIndWidth;

            Point[] pts = new Point[2];

            pts[0].X = (int)(Math.Cos(rad) * (h) + dx);
            pts[0].Y = (int)(Math.Sin(rad) * (h) + dy);

            pts[1].X = (int)(Math.Cos(rad) * (h + MinorTickSize.Height) + dx);
            pts[1].Y = (int)(Math.Sin(rad) * (h + MinorTickSize.Height) + dy);

            return (pts);
        }

        #endregion

        #endregion

        #region RenderTickMajor

        #region RenderTickMajor

        /// <summary>
        /// Renders the Major Tick marks
        /// </summary>
        /// <param name="e"></param>
        public override void RenderTickMajor(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            // Loop through each tick

            using (Pen pen1 = new Pen(MajorTickColor, MajorTickSize.Width))
            {
                for (int i = 0; i < MajorTicks; i++)
                    g.DrawLines(pen1, GetMajorTickPoints(i));
            }
        }

        #endregion

        #region GetMajorTickPoints

        /// <summary>
        /// Calculates a series of points
        /// that defines the tick mark
        /// </summary>
        /// <param name="tick">Tick to calculate</param>
        /// <returns>An array of points that defines the tick</returns>
        private Point[] GetMajorTickPoints(int tick)
        {
            float degree = GetTickDegree((float)Knob.MajorTickAmount, tick);
            double rad = GetRadians(degree);

            int dx = ZoneIndicatorBounds.X + ZoneIndicatorBounds.Width / 2;
            int dy = ZoneIndicatorBounds.Y + ZoneIndicatorBounds.Height / 2;
            int h = ZoneIndicatorBounds.Width / 2 - (int)ZoneIndWidth;

            Point[] pts = new Point[2];

            pts[0].X = (int)(Math.Cos(rad) * h + dx);
            pts[0].Y = (int)(Math.Sin(rad) * h + dy);

            pts[1].X = (int)(Math.Cos(rad) * (h + MajorTickSize.Height + 1) + dx);
            pts[1].Y = (int)(Math.Sin(rad) * (h + MajorTickSize.Height + 1) + dy);

            return (pts);
        }

        #endregion

        #endregion

        #region RenderKnobFace

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
                    // Render the face shadow

                    RenderInset(g, path, Pens.Gray, MinorTickSize.Width);

                    using (SolidBrush br =
                        new SolidBrush(Color.FromArgb(160, Color.Gray)))
                    {
                        g.FillPath(br, path);
                    }

                    path.Reset();

                    // Render the face front

                    using (Pen pen = new Pen(Color.White, MinorTickSize.Width))
                        RenderInset(g, path, pen, 0);

                    using (Brush br = KnobFaceColor.GetBrush(KnobFaceBounds))
                        g.FillPath(br, path);
                }
            }
        }

        #endregion

        #region RenderInset

        /// <summary>
        /// Renders the face, including the arc insets
        /// as well as the connecting segments
        /// </summary>
        /// <param name="g"></param>
        /// <param name="path">Path to render to</param>
        /// <param name="pen">Outlining pen</param>
        /// <param name="offset">Delta offset - used for shadowing</param>
        private void RenderInset(Graphics g, GraphicsPath path, Pen pen, int offset)
        {
            // Calculate the bounding rectangle
            // for the inset segments

            Rectangle rInset = KnobFaceBounds;

            int w = (int)(ZoneIndWidth);
            rInset.Inflate(-w, -w);

            // Calculate the bounding rectangle
            // for the inset arcs

            int radius = (int)(KnobWidth * .055f);

            Rectangle r = new Rectangle(0, 0, radius, radius);

            // Loop through each arc point and
            // render the individual arc insets

            for (int i = 0; i < 12; i++)
            {
                // Calculate the inset location and
                // render the inset arc

                r.Location = GetArcPoint(rInset, i * 30f, (int)(offset - radius * .5f));

                path.AddArc(r, (i * 30) - 90, -180);
            }

            // Let Windows connect the arcs, and
            // then draw the completed path

            path.CloseFigure();

            g.DrawPath(pen, path);
        }

        #endregion

        #region GetArcPoint

        /// <summary>
        /// Calculates the arc point at the given
        /// degree and offset
        /// </summary>
        /// <param name="rInset">Inset bounding rectangle</param>
        /// <param name="degree">Degree to position arc inset</param>
        /// <param name="offset">Offset (used for shading)</param>
        /// <returns></returns>
        private Point GetArcPoint(Rectangle rInset, float degree, int offset)
        {
            // Calculate the default position

            double rad = GetRadians(degree);

            int dx = rInset.X + rInset.Width / 2;
            int dy = rInset.Y + rInset.Height / 2;
            int h = rInset.Width / 2 + (int)ZoneIndWidth;

            Point pt = new Point();

            pt.X = (int)(Math.Cos(rad) * h + dx);
            pt.Y = (int)(Math.Sin(rad) * h + dy);

            // Offset the point if so requested
            // (face shadow support)

            pt.Offset(offset, offset);

            return (pt);
        }

        #endregion

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

                // Allocate a graphics path and calculate
                // its bounding rectangle

                using (GraphicsPath path = new GraphicsPath())
                {
                    Rectangle r = new
                        Rectangle(KnobIndicatorBounds.Location, KnobIndicatorBounds.Size);

                    int delta = (int) (KnobWidth*.020f);

                    // Render the knob, it's shadow, and
                    // the hilight crescent

                    r.Offset(delta, delta);
                    RenderIndFace(g, path, r);

                    r.Offset(-delta*2, -delta*2);
                    RenderIndCrescent(g, path, r);

                    // Render the indicator arrow

                    path.AddLines(GetIndicatorPoints());
                    path.CloseFigure();

                    // Fill the closed arrow

                    using (SolidBrush br = new SolidBrush(KnobIndicatorPointerColor))
                        g.FillPath(br, path);
                }
            }
        }

        #endregion

        #region RenderIndCrescent

        /// <summary>
        /// Renders the hilight crescent
        /// </summary>
        /// <param name="g"></param>
        /// <param name="path">Accumulating GraphicsPath</param>
        /// <param name="r">Bounding rectangle</param>
        private void RenderIndCrescent(Graphics g, GraphicsPath path, Rectangle r)
        {
            // Add the bottom crescent arc

            path.AddArc(r, 8, 78);

            // Calculate the intersecting upper arc
            // and add it to the path

            int cradius = r.Width / 2;
            int nradius = (int)(cradius * 1.5f);

            int dr = nradius - cradius;
            int dz = (int)(dr * Math.Cos(45) * 1.6f);

            Rectangle r2 = new Rectangle(r.X - dr - dz, r.Y - dr - dz, nradius * 2, nradius * 2);

            path.AddArc(r2, 70, -50);

            Color c = Color.FromArgb(170, KnobIndicatorColor.Start);

            using (LinearGradientBrush lbr =
                new LinearGradientBrush(KnobIndicatorBounds, Color.White, c, 45f))
            {
                g.FillPath(lbr, path);
            }

            path.Reset();
        }

        #endregion

        #region RenderIndFace

        /// <summary>
        /// Renders the face of the knob indicator
        /// </summary>
        /// <param name="g"></param>
        /// <param name="path">Accumulating GraphicsPath</param>
        /// <param name="r">Bounding rectangle</param>
        private void RenderIndFace(Graphics g, GraphicsPath path, Rectangle r)
        {
            // Render the knob shadow

            Color c3 = Color.FromArgb(100, ControlPaint.Dark(KnobIndicatorColor.End));

            using (SolidBrush br = new SolidBrush(c3))
                g.FillEllipse(br, r);

            // Render the knob

            using (Brush br = KnobIndicatorColor.GetBrush(KnobIndicatorBounds))
                g.FillEllipse(br, KnobIndicatorBounds);

            path.Reset();
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

            int arrowHeight = (int)(KnobWidth * .025f);

            double rad0 = GetRadians(degrees - 19);
            double rad1 = GetRadians(degrees);
            double rad2 = GetRadians(degrees + 19);

            int dx = KnobIndicatorBounds.X + KnobIndicatorBounds.Width / 2 + 1;
            int dy = KnobIndicatorBounds.Y + KnobIndicatorBounds.Height / 2 + 1;
            int h = KnobIndicatorBounds.Width / 2;

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
