using System.Drawing;

namespace DevComponents.Instrumentation
{
    internal class LabelPoint
    {
        #region Private variables

        private Point _Point;
        private float _Angle;
        private double _Interval;

        private bool _Visible = true;

        #endregion

        #region Public properties

        #region Angle

        public float Angle
        {
            get { return (_Angle); }
            set { _Angle = value; }
        }

        #endregion

        #region Point

        public Point Point
        {
            get { return (_Point); }
            set { _Point = value; }
        }

        #endregion

        #region Interval

        public double Interval
        {
            get { return (_Interval); }
            set { _Interval = value; }
        }

        #endregion

        #region Visible

        public bool Visible
        {
            get { return (_Visible); }
            set { _Visible = value; }
        }

        #endregion

        #endregion

        #region PaintRotatedLabel

        public void PaintRotatedLabel(Graphics g,
            string text, LabelLayout layout, int height, Brush br, Font font)
        {
            SizeF sz = g.MeasureString(text, font);
            Size size = sz.ToSize();

            Point pt = new Point(0, 0);

            switch (layout.Placement)
            {
                case DisplayPlacement.Far:
                    pt.Y = -height / 2;
                    break;

                case DisplayPlacement.Near:
                    pt.Y = height / 2;
                    break;
            }

            g.TranslateTransform(_Point.X, _Point.Y);
            g.RotateTransform((_Angle + 90) % 360);
            g.TranslateTransform(pt.X, pt.Y);

            float fontAngle = layout.Angle;

            if (layout.AutoOrientLabel == true)
            {
                if (((_Angle + layout.Angle) % 360) < 180)
                    fontAngle += 180;
            }

            g.RotateTransform(fontAngle);

            g.DrawString(text, font, br,
                new Point(-size.Width / 2, -size.Height / 2));

            g.ResetTransform();
        }

        #endregion

        #region PaintNonRotatedLabel

        public void PaintNonRotatedLabel(Graphics g, string text,
            LabelLayout layout, Point center, int radius, Brush br, Font font)
        {
            SizeF sz = g.MeasureString(text, font);

            int x = _Point.X - center.X;
            int y = _Point.Y - center.Y;

            Point pt = new Point();

            switch (layout.Placement)
            {
                case DisplayPlacement.Far:
                    {
                        int dx = (int)(((float)(x - radius) / (radius * 2)) * sz.Width);
                        int dy = (int)(((float)(y - radius) / (radius * 2)) * sz.Height);

                        pt = new Point(_Point.X + dx, _Point.Y + dy);
                    }
                    break;

                case DisplayPlacement.Near:
                    {
                        int dx = (int)(((float)(x + radius) / (radius * 2)) * sz.Width);
                        int dy = (int)(((float)(y + radius) / (radius * 2)) * sz.Height);

                        pt = new Point(_Point.X - dx, _Point.Y - dy);
                    }
                    break;

                case DisplayPlacement.Center:
                    {
                        int dx = (int)(sz.Width / 2);
                        int dy = (int)(sz.Height / 2);

                        pt = new Point(_Point.X - dx, _Point.Y - dy);
                    }
                    break;
            }

            g.DrawString(text, font, br, pt);
        }

        #endregion
    }

}
