using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DevComponents.Instrumentation.Primitives
{
    public class KnobStyle1 : BaseKnob
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="knobControl">Associated knob control</param>
        public KnobStyle1(KnobControl knobControl)
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

            ZoneIndWidth = 2;

            MajorTickSize = new Size((int)(KnobWidth * 0.015), (int)(KnobWidth * 0.017));
            MinorTickSize = new Size(MajorTickSize.Width / 2, MajorTickSize.Height);

            IndTickHeight = (int)ZoneIndWidth;

            CalculateBoundingRects();

            // Fill in the default knob colors

            DefaultColorTable.MajorTickColor = Color.Black;
            DefaultColorTable.MinorTickColor = Color.Black;
            DefaultColorTable.ZoneIndicatorColor = Color.Black;
            DefaultColorTable.KnobIndicatorPointerColor = Color.Gray;

            DefaultColorTable.KnobFaceColor = new LinearGradientColorTable(Color.Black, Color.Gray, 40);
            DefaultColorTable.KnobIndicatorColor = new LinearGradientColorTable(Color.Gray, Color.White, 40);
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

            int delta = MaxLabelWidth + MajorTickSize.Height + (int)(KnobWidth * 0.04f);

            ZoneIndicatorBounds = new Rectangle(delta, delta,
                KnobWidth - (delta * 2), KnobWidth - (delta * 2));

            // Calculate the KnobFace and inset face rectangles

            delta += 1;

            KnobFaceBounds = new Rectangle(delta, delta,
                KnobWidth - (delta * 2), KnobWidth - (delta * 2));

            // Calculate the KnobIndicator bounding rectangle

            delta += (int)(KnobWidth * .07f);

            KnobIndicatorBounds = new Rectangle(delta, delta,
                KnobWidth - (delta * 2), KnobWidth - (delta * 2));

            // Calculate the TickLabel bounding rect

            delta = (int)((MaxLabelWidth + IndTickHeight) * .85f);

            TickLabelBounds = new Rectangle(ZoneIndicatorBounds.Location, ZoneIndicatorBounds.Size);
            TickLabelBounds.Inflate(delta, delta);

            // Calculate the focus rectangle

            FocusRectBounds = new Rectangle(KnobIndicatorBounds.Location, KnobIndicatorBounds.Size);

            delta = (int)(KnobIndicatorBounds.Width * .035);

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

                using (Pen pen = new Pen(ZoneIndicatorColor, 2))
                    g.DrawEllipse(pen, ZoneIndicatorBounds);
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

                using (Brush br = KnobFaceColor.GetBrush(KnobFaceBounds))
                    g.FillEllipse(br, KnobFaceBounds);
            }
        }

        #endregion

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

                // Render the Indicator shadow

                Rectangle r = new Rectangle(KnobIndicatorBounds.X, KnobIndicatorBounds.Y,
                                            KnobIndicatorBounds.Width, KnobIndicatorBounds.Height);

                int delta = (int) (KnobWidth * .025f);

                r.Offset(delta, delta);

                using (SolidBrush br = new
                    SolidBrush(Color.FromArgb(160, KnobFaceColor.Start)))
                {
                    g.FillEllipse(br, r);
                }

                r.Offset(-delta, -delta);

                // Render the indicator

                using (LinearGradientBrush br = new
                    LinearGradientBrush(KnobIndicatorBounds, KnobIndicatorColor.End, KnobIndicatorColor.Start, -40f))
                    g.FillEllipse(br, KnobIndicatorBounds);

                delta = (int) (r.Width * .02f);

                r.Inflate(-delta, -delta);

                using (Brush br = KnobIndicatorColor.GetBrush(r))
                    g.FillEllipse(br, r);

                // Render the indicator dimple

                r = GetIndicatorRect();

                delta = (int) (r.Width * .05f);

                r.Inflate(delta, delta);

                using (Brush lgb = new
                    LinearGradientBrush(r, Color.White, KnobIndicatorPointerColor, -40f))
                {
                    g.FillEllipse(lgb, r);
                }

                r.Inflate(-delta, -delta);

                using (LinearGradientBrush lgb = new
                    LinearGradientBrush(r, KnobIndicatorPointerColor, Color.White, 220f))
                {
                    g.FillEllipse(lgb, r);
                }
            }
        }

        /// <summary>
        /// Returns the knob indicator rectangle
        /// </summary>
        /// <returns></returns>
        private Rectangle GetIndicatorRect()
        {
            float degrees = (float)(Knob.SweepAngle / ValueCount) *
                (float)(Knob.Value - Knob.MinValue) + Knob.StartAngle;

            double radians = GetRadians(degrees);

            int dx = KnobIndicatorBounds.X + KnobIndicatorBounds.Width / 2;
            int dy = KnobIndicatorBounds.Y + KnobIndicatorBounds.Height / 2;
            int h = (int)(KnobIndicatorBounds.Width * .3f);

            int x = (int)(Math.Cos(radians) * h + dx);
            int y = (int)(Math.Sin(radians) * h + dy);

            int radius = (int)(KnobIndicatorBounds.Width * .12f);

            Rectangle r = new Rectangle(x - radius, y - radius, radius * 2, radius * 2);

            return (r);
        }

        #endregion

        #endregion
    }
}
