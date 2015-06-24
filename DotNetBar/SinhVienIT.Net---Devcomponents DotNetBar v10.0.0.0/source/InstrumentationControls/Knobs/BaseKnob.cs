using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace DevComponents.Instrumentation.Primitives
{
    public class BaseKnob
    {
        #region Private variables

        private bool _Reset = true;

        #endregion

        #region Protected variables

        protected KnobControl Knob;             // Associated KnobControl

        protected int MajorTicks;               // Number of major ticks
        protected int MinorTicks;               // Number of minor ticks
        protected Size MajorTickSize;           // Size of a major tick
        protected Size MinorTickSize;           // Size of a minor tick

        protected int KnobWidth;                // Normalized knob width
        protected int IndTickHeight;            // Ind tick height;
        protected int MaxLabelWidth;            // Maximum label width
        protected float ZoneIndWidth;           // Zone indicator width

        protected KnobColorTable DefaultColorTable;    // Default Knob color table

        #endregion

        #region Internal variables

        internal Rectangle TickLabelBounds;     // Tick Label bounding rectangle
        internal Rectangle KnobFaceBounds;      // Knob face bounding rectangle
        internal Rectangle KnobIndicatorBounds; // Knob Indicator bounding rectangle
        internal Rectangle FocusRectBounds;     // Focus bounding rectangle
        internal Rectangle ZoneIndicatorBounds; // Zone indicator bounding rectangle

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="knobControl">Associated knob control</param>
        public BaseKnob(KnobControl knobControl)
        {
            Knob = knobControl;

            DefaultColorTable = new KnobColorTable();
        }

        #region Public Properties

        #region MajorTickColor

        /// <summary>
        /// MajorTickColor
        /// </summary>
        protected Color MajorTickColor
        {
            get
            {
                Color c = Knob.KnobColor.MajorTickColor;

                return (c.IsEmpty == false ?
                    c : DefaultColorTable.MajorTickColor);
            }
        }

        #endregion

        #region MinorTickColor

        /// <summary>
        /// MinorTickColor
        /// </summary>
        protected Color MinorTickColor
        {
            get
            {
                Color c = Knob.KnobColor.MinorTickColor;

                return (c.IsEmpty == false ?
                    c : DefaultColorTable.MinorTickColor);
            }
        }

        #endregion

        #region KnobIndicatorPointerColor

        /// <summary>
        /// KnobIndicatorPointerColor
        /// </summary>
        protected Color KnobIndicatorPointerColor
        {
            get
            {
                Color c = Knob.KnobColor.KnobIndicatorPointerColor;

                return (c.IsEmpty == false ?
                    c : DefaultColorTable.KnobIndicatorPointerColor);
            }
        }

        #endregion

        #region ZoneIndicatorColor

        /// <summary>
        /// ZoneIndicatorBaseColor
        /// </summary>
        protected Color ZoneIndicatorColor
        {
            get
            {
                Color c = Knob.KnobColor.ZoneIndicatorColor;

                return (c.IsEmpty == false ?
                    c : DefaultColorTable.ZoneIndicatorColor);
            }
        }

        #endregion

        #region KnobFaceColor

        /// <summary>
        /// KnobFaceColor
        /// </summary>
        protected LinearGradientColorTable KnobFaceColor
        {
            get
            {
                return (ApplyColor(Knob.KnobColor.KnobFaceColor,
                                   DefaultColorTable.KnobFaceColor));
            }
        }

        #endregion

        #region KnobIndicatorColor

        /// <summary>
        /// KnobIndicatorColor
        /// </summary>
        protected LinearGradientColorTable KnobIndicatorColor
        {
            get
            {
                return (ApplyColor(Knob.KnobColor.KnobIndicatorColor,
                                   DefaultColorTable.KnobIndicatorColor));
            }
        }

        #endregion

        #region LeftZoneIndicatorColor

        /// <summary>
        /// LeftZoneIndicatorColor
        /// </summary>
        protected LinearGradientColorTable LeftZoneIndicatorColor
        {
            get
            {
                return (ApplyColor(Knob.KnobColor.MinZoneIndicatorColor,
                                   DefaultColorTable.MinZoneIndicatorColor));
            }
        }

        #endregion

        #region MiddleZoneIndicatorColor

        /// <summary>
        /// MiddleZoneIndicatorColor
        /// </summary>
        protected LinearGradientColorTable MiddleZoneIndicatorColor
        {
            get
            {
                return (ApplyColor(Knob.KnobColor.MidZoneIndicatorColor,
                                   DefaultColorTable.MidZoneIndicatorColor));
            }
        }

        #endregion

        #region RightZoneIndicatorColor

        /// <summary>
        /// RightZoneIndicatorColor
        /// </summary>
        protected LinearGradientColorTable RightZoneIndicatorColor
        {
            get
            {
                return (ApplyColor(Knob.KnobColor.MaxZoneIndicatorColor,
                                   DefaultColorTable.MaxZoneIndicatorColor));
            }
        }


        #endregion

        #endregion

        #region ApplyColor

        /// <summary>
        /// ApplyColor
        /// </summary>
        /// <param name="c"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        private LinearGradientColorTable ApplyColor(
            LinearGradientColorTable c, LinearGradientColorTable d)
        {
            if (c.IsEmpty == true)
                return (d);

            if (c.Start.IsEmpty == false && c.End.IsEmpty == false)
                return (c);

            return (new LinearGradientColorTable(
                c.Start.IsEmpty ? d.Start : c.Start,
                c.End.IsEmpty ? d.End : c.End, c.GradientAngle));
        }

        #endregion

        #region Knob configuration

        #region ConfigureKnob

        /// <summary>
        /// Main control configuration routine
        /// </summary>
        /// <param name="e"></param>
        public virtual void ConfigureKnob(PaintEventArgs e)
        {
            // Calculate the bounding width for the control to be the 
            // minimum of either the width and the height

            KnobWidth = Math.Min(Knob.Width, Knob.Height);
            KnobWidth = Math.Max(KnobWidth, Knob.MinKnobSize);

            if ((KnobWidth % 2) != 0)
                KnobWidth -= 1;

            // Calculate the number of Major and Minor ticks and then
            // measure each associated label so that we can mane sure we
            // have enough room for them in the control rectangle

            CalculateTicksCounts();
            MeasureTickLabels();
        }

        #endregion

        #region ResetKnob

        /// <summary>
        /// Sets the reset state to true, signifying
        /// that the control needs to be reconfigured
        /// before it is redrawn to the screen
        /// </summary>
        public void ResetKnob()
        {
            _Reset = true;
        }

        #endregion

        #region InitRender

        /// <summary>
        /// Initializes the rendering process by making
        /// sure that the control is reconfigured if
        /// necessary
        /// </summary>
        /// <param name="e"></param>
        public void InitRender(PaintEventArgs e)
        {
            if (_Reset == true)
            {
                ConfigureKnob(e);

                _Reset = false;
            }
        }

        #endregion

        #endregion

        #region Render processing

        public virtual void RenderZoneIndicator(PaintEventArgs e) { }
        public virtual void RenderKnobFace(PaintEventArgs e) { }
        public virtual void RenderKnobIndicator(PaintEventArgs e) { }

        #region RenderTickMinor

        /// <summary>
        /// Renders the minor tick marks
        /// </summary>
        /// <param name="e"></param>
        public virtual void RenderTickMinor(PaintEventArgs e)
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
            int h = (int)(ZoneIndicatorBounds.Width / 2 + ZoneIndWidth / 2 - 1);

            Point[] pts = new Point[2];

            pts[0].X = (int)(Math.Cos(rad) * h + dx);
            pts[0].Y = (int)(Math.Sin(rad) * h + dy);

            pts[1].X = (int)(Math.Cos(rad) * (h + MinorTickSize.Height) + dx);
            pts[1].Y = (int)(Math.Sin(rad) * (h + MinorTickSize.Height) + dy);

            return (pts);
        }

        #endregion

        #region RenderTickMajor

        /// <summary>
        /// Renders the Major Tick marks
        /// </summary>
        /// <param name="e"></param>
        public virtual void RenderTickMajor(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            // Loop through each tick

            using (Pen pen1 = new Pen(MajorTickColor, MajorTickSize.Width))
            {
                for (int i = 0; i < MajorTicks; i++)
                    g.DrawLines(pen1, GetMajorTickPoints(i));
            }
        }

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
            int h = (int)(ZoneIndicatorBounds.Width / 2 + ZoneIndWidth / 2) - 1;

            Point[] pts = new Point[2];

            pts[0].X = (int)(Math.Cos(rad) * h + dx);
            pts[0].Y = (int)(Math.Sin(rad) * h + dy);

            pts[1].X = (int)(Math.Cos(rad) * (h + MajorTickSize.Height) + dx);
            pts[1].Y = (int)(Math.Sin(rad) * (h + MajorTickSize.Height) + dy);

            return (pts);
        }

        #endregion

        #region RenderTickLabel

        /// <summary>
        /// Renders the major tick label
        /// </summary>
        /// <param name="e"></param>
        public virtual void RenderTickLabel(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            // Allocate a StrFormat to use in the display
            // of out tick label text

            using (StringFormat strFormat = new StringFormat(StringFormatFlags.NoClip))
            {
                strFormat.Alignment = StringAlignment.Center;

                float radius = TickLabelBounds.Width / 2;

                // Grab our tick label font - which is
                // dynamically sized according to the control

                Font labelFont = Knob.Font;

                int dx = KnobWidth / 2;
                int dy = (KnobWidth - (int) labelFont.SizeInPoints) / 2;

                // Loop through each major tick

                float dpt = (float) (Knob.SweepAngle * Knob.MajorTickAmount / ValueCount);

                for (int i = 0; i < MajorTicks; i++)
                {
                    if (Math.Abs(dpt * i) < 360)
                    {
                        float degree = dpt * i + Knob.StartAngle;

                        if (dpt < 0)
                        {
                            if (degree < Knob.StartAngle + Knob.SweepAngle)
                                degree = Knob.StartAngle + Knob.SweepAngle;
                        }
                        else
                        {
                            if (degree > Knob.StartAngle + Knob.SweepAngle)
                                degree = Knob.StartAngle + Knob.SweepAngle;
                        }

                        decimal x = Math.Min(Knob.MinValue + (i * Knob.MajorTickAmount), Knob.MaxValue);
                        string s = ((int) x == x) ? ((int) x).ToString() : x.ToString();
                        Size sz = TextRenderer.MeasureText(s, labelFont);

                        double currentAngle = GetRadians(degree);

                        Point pt = new Point((int) (dx - sz.Width / 2 + radius * (float) Math.Cos(currentAngle)),
                                             (int) (dy + radius * (float) Math.Sin(currentAngle)));

                        Rectangle r = new Rectangle(pt, new Size(1000, 1000));

                        TextRenderer.DrawText(g, s, labelFont, r,
                                              Knob.ForeColor, TextFormatFlags.Left);
                    }
                }
            }
        }

        #endregion

        #region RenderFocusRect

        /// <summary>
        /// Renders the base focus rect
        /// </summary>
        /// <param name="e"></param>
        public virtual void RenderFocusRect(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            using (Pen pen = new Pen(Color.DarkGray))
            {
                pen.DashStyle = DashStyle.Dash;

                g.DrawEllipse(pen, FocusRectBounds);
            }
        }

        #endregion

        #endregion

        #region GetValueFromPoint

        /// <summary>
        /// Determines the control Value from
        /// a specified Point on the control
        /// </summary>
        /// <param name="pt">Point on the control</param>
        /// <returns>Value</returns>
        public decimal GetValueFromPoint(Point pt)
        {
            // Calculate the relative X and Y
            // coordinate pair for the given point

            int radius = ZoneIndicatorBounds.Width / 2;

            Point cpt = new Point(ZoneIndicatorBounds.X + radius, ZoneIndicatorBounds.Y + radius);

            int dx = pt.X - cpt.X;
            int dy = pt.Y - cpt.Y;

            // Determine the radians for the given coord pair
            // based upon which quadrant it is located in

            double radians;

            if (dx >= 0)
            {
                if (dy >= 0)
                    radians = Math.Atan((double)dy / dx);
                else
                    radians = -Math.Atan((double)dx / dy) + Math.PI * 1.5;
            }
            else
            {
                if (dy >= 0)
                    radians = -Math.Atan((double)dx / dy) + Math.PI / 2;
                else
                    radians = Math.Atan((double)dy / dx) + Math.PI;
            }

            // Convert our calculated Radians to Degrees
            // and then 'normalize' our values to the
            // current StartAngle

            int degrees = (int)GetDegrees(radians);

            decimal normal = degrees - Knob.StartAngle;

            if (Knob.SweepAngle < 0)
            {
                normal = 360 - normal;
                normal = normal % 360;
            }

            if (normal < 0)
                normal += 360;

            // If our normalized angle is within the
            // SweepAngle, then return the associated
            // range value

            int n = Math.Abs(Knob.SweepAngle);

            if (normal >= 0 && normal <= n)
                return (normal * ValueCount / n + Knob.MinValue);

            // The normalized angle is outside the SweepAngle, so
            // return either the MinValue or MaxValue (based upon
            // which one is closer)

            return ((normal >= n + (360 - n) / 2) ? Knob.MinValue : Knob.MaxValue);
        }

        #endregion

        #region CalculateTicksCounts

        /// <summary>
        /// Calculate how many major and
        /// minor ticks are presented on the control
        /// </summary>
        private void CalculateTicksCounts()
        {
            decimal count = ValueCount;

            // Calculate the number of major ticks

            MajorTicks = 0;

            if (Knob.MajorTickAmount > 0)
            {
                MajorTicks = (int)(count / Knob.MajorTickAmount);

                if (MajorTicks * Knob.MajorTickAmount < count)
                    MajorTicks++;

                MajorTicks++;
            }

            // Calculate the number of minor ticks

            MinorTicks = 0;

            if (Knob.MinorTickAmount > 0)
            {
                MinorTicks = (int)(count / Knob.MinorTickAmount);

                if (MinorTicks * Knob.MinorTickAmount < count)
                    MinorTicks++;

                MinorTicks++;
            }
        }

        #endregion

        #region MeasureTickLabels

        /// <summary>
        /// Measure the width of each text label in order to
        /// make sure we have room for it in the control
        /// </summary>
        private void MeasureTickLabels()
        {
            MaxLabelWidth = 0;

            if (Knob.ShowTickLabels == true)
            {
                Font labelFont = Knob.Font;

                for (int i = 0; i < MajorTicks; i++)
                {
                    // Save the text and width for later use

                    string s = (Knob.MinValue + (i * Knob.MajorTickAmount)).ToString();
                    int w = TextRenderer.MeasureText(s, labelFont).Width + 2;

                    // Keep track of the maximum width

                    if (w > MaxLabelWidth)
                        MaxLabelWidth = w;
                }
            }
        }

        #endregion

        #region GetTickDegree

        /// <summary>
        /// Gets the arc degree associated with
        /// the given gauge tick
        /// </summary>
        /// <param name="tickAmount">Major or minor tick amount</param>
        /// <param name="tick">The tick to convert</param>
        /// <returns></returns>
        protected float GetTickDegree(float tickAmount, int tick)
        {
            float dpt = (Knob.SweepAngle * tickAmount) / (float)ValueCount;
            float degree = dpt * tick + Knob.StartAngle;

            if (dpt < 0)
            {
                if (degree < Knob.StartAngle + Knob.SweepAngle)
                    degree = Knob.StartAngle + Knob.SweepAngle;
            }
            else
            {
                if (degree > Knob.StartAngle + Knob.SweepAngle)
                    degree = Knob.StartAngle + Knob.SweepAngle;
            }

            return (degree);
        }

        #endregion

        #region GetRadians

        /// <summary>
        /// Converts Degrees to Radians
        /// </summary>
        /// <param name="theta">Degrees</param>
        /// <returns>Radians</returns>
        public double GetRadians(float theta)
        {
            return (theta * Math.PI / 180);
        }

        #endregion

        #region GetDegrees

        /// <summary>
        /// Converts Radians to Degrees
        /// </summary>
        /// <param name="radians">Radians</param>
        /// <returns>Degrees</returns>
        public double GetDegrees(double radians)
        {
            return (radians * 180 / Math.PI);
        }

        #endregion

        #region ValueCount

        /// <summary>
        /// Gets the value range, expressed as a count
        /// </summary>
        internal decimal ValueCount
        {
            get { return (Knob.MaxValue - Knob.MinValue); }
        }

        #endregion

        #region PointInControl

        /// <summary>
        /// Determines if a given Point is within
        /// the bounds of the control
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public virtual bool PointInControl(Point pt)
        {
            // Allow a little leeway around the control

            Rectangle r = new Rectangle(ZoneIndicatorBounds.Location, ZoneIndicatorBounds.Size);

            r.Inflate(40, 40);

            int radius = r.Width / 2;

            Point cpt = new Point(r.X + radius, r.Y + radius);

            return (PointInCircle(pt, cpt, radius));      
        }

        #endregion

        #region PointInCircle

        /// <summary>
        /// Determines if a given point is within a given circle
        /// </summary>
        /// <param name="pt">Point in question</param>
        /// <param name="cpt">Center Point</param>
        /// <param name="radius">Circle radius</param>
        /// <returns></returns>
        public bool PointInCircle(Point pt, Point cpt, int radius)
        {
            int a = pt.X - cpt.X;
            int b = pt.Y - cpt.Y;

            int c = (int)Math.Sqrt(a * a + b * b);

            return (c < radius);
        }

        #endregion
    }
}
