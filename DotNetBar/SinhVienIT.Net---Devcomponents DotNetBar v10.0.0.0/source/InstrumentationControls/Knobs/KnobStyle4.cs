using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DevComponents.Instrumentation.Primitives
{
    public class KnobStyle4 : BaseKnob
    {
        #region Private instance variables

        private Rectangle _KnobFaceInset;   // Knob face inset rect

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="knobControl">Associated knob control</param>
        public KnobStyle4(KnobControl knobControl)
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

            ZoneIndWidth = KnobWidth * 0.025f;

            MajorTickSize = new Size((int)(KnobWidth * 0.01), (int)(KnobWidth * 0.04));
            MinorTickSize = new Size(MajorTickSize.Width, (int)(MajorTickSize.Height * .60f));

            IndTickHeight = (int)ZoneIndWidth + MajorTickSize.Height;

            CalculateBoundingRects();

            // Fill in the default knob colors

            DefaultColorTable.MajorTickColor = Color.Gray;
            DefaultColorTable.MinorTickColor = Color.DeepSkyBlue;
            DefaultColorTable.ZoneIndicatorColor = Color.DeepSkyBlue;
            DefaultColorTable.KnobIndicatorPointerColor = Color.White;

            DefaultColorTable.KnobFaceColor = new LinearGradientColorTable(Color.White, ColorFactory.GetColor(0xAAAAAA), 40);
            DefaultColorTable.KnobIndicatorColor = new LinearGradientColorTable(Color.White, ColorFactory.GetColor(0xAAAAAA), 20);
            DefaultColorTable.MinZoneIndicatorColor = new LinearGradientColorTable(Color.White, Color.Green);
            DefaultColorTable.MaxZoneIndicatorColor =new LinearGradientColorTable( Color.Yellow, Color.Red);
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

            int delta = MaxLabelWidth + MajorTickSize.Height + (int)(KnobWidth * 0.04f);

            ZoneIndicatorBounds = new Rectangle(delta, delta,
                KnobWidth - (delta * 2), KnobWidth - (delta * 2));

            // Calculate the KnobFace and inset face rectangles

            delta += (int)(ZoneIndWidth);

            KnobFaceBounds = new Rectangle(delta, delta,
                KnobWidth - (delta * 2), KnobWidth - (delta * 2));

            _KnobFaceInset = new Rectangle(KnobFaceBounds.X, KnobFaceBounds.Y,
                KnobFaceBounds.Width, KnobFaceBounds.Height);

            _KnobFaceInset.Inflate(-(int)(KnobWidth * .02f), -(int)(KnobWidth * .02f));

            // Calculate the KnobIndicator bounding rectangle

            delta += (int)(KnobFaceBounds.Width * .23f);

            KnobIndicatorBounds = new Rectangle(delta, delta,
                KnobWidth - (delta * 2), KnobWidth - (delta * 2));

            // Calculate the TickLabel bounding rect

            delta = (int)((MaxLabelWidth + IndTickHeight) * .60f);

            TickLabelBounds = new Rectangle(ZoneIndicatorBounds.Location, ZoneIndicatorBounds.Size);
            TickLabelBounds.Inflate(delta, delta);

            // Calculate the focus rectangle

            FocusRectBounds = new Rectangle(KnobIndicatorBounds.Location, KnobIndicatorBounds.Size);

            delta = (int)(KnobIndicatorBounds.Width * .045);

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

                using (Pen pen = new Pen(ZoneIndicatorColor, ZoneIndWidth))
                    g.DrawArc(pen, ZoneIndicatorBounds, Knob.StartAngle - 1, Knob.SweepAngle + 2);
            }
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

                // allocate a path to accumulate the knob
                // drawing pieces

                using (GraphicsPath path = new GraphicsPath())
                {
                    // Draw the 12, 30 degree arc segments

                    for (int i = 0; i < 12; i++)
                    {
                        path.AddArc((i % 2 == 0)
                            ? KnobFaceBounds : _KnobFaceInset, i * 30 - 15, 30);
                    }

                    path.CloseFigure();

                    // Fill the face and then draw the outlining path

                    using (Brush br = KnobFaceColor.GetBrush(KnobFaceBounds))
                        g.FillPath(br, path);

                    g.DrawPath(Pens.Gray, path);
                }
            }
        }

        #endregion

        #region RenderKnobIndicator

        #region RenderKnobIndicator

        /// <summary>
        /// Renders the knob indicator
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

                    // Outline the arrow and draw the knob face

                    using (Pen pen = new Pen(Color.Gray, 2))
                        g.DrawPath(pen, path);

                    Color c = ControlPaint.Dark(KnobIndicatorColor.End, .4f);

                    using (Pen pen = new Pen(c, 2))
                        g.DrawArc(pen, KnobIndicatorBounds, 0, 360);

                    // Fill the knob face

                    using (Brush br = KnobIndicatorColor.GetBrush(KnobIndicatorBounds))
                        g.FillEllipse(br, KnobIndicatorBounds);

                    // Reset the path and add a glossy looking
                    // hilight to the top of the control

                    path.Reset();

                    path.AddArc(KnobFaceBounds, 180, 180);
                    path.CloseFigure();

                    using (LinearGradientBrush lgb =
                        new LinearGradientBrush(KnobFaceBounds, Color.FromArgb(75, Color.White),
                                                Color.Transparent, LinearGradientMode.Vertical))
                    {
                        g.FillPath(lgb, path);
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

            int arrowHeight = (int)(KnobIndicatorBounds.Width * .25f);

            double rad0 = GetRadians(degrees - 10);
            double rad1 = GetRadians(degrees);
            double rad2 = GetRadians(degrees + 10);

            int dx = KnobIndicatorBounds.X + KnobIndicatorBounds.Width / 2 + 1;
            int dy = KnobIndicatorBounds.Y + KnobIndicatorBounds.Height / 2 + 1;
            int h = KnobIndicatorBounds.Width / 2 + 4;

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