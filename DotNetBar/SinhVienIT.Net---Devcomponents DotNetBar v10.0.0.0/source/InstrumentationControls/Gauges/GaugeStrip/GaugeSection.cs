using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Windows.Forms;
using DevComponents.Instrumentation.Primitives;

namespace DevComponents.Instrumentation
{
    /// <summary>
    /// Collection of GaugeSections
    /// </summary>
    public class GaugeSectionCollection : GenericCollection<GaugeSection>
    {
        #region ICloneable Members

        public override object Clone()
        {
            GaugeSectionCollection copy = new GaugeSectionCollection();

            CopyToItem(copy);

            return (copy);
        }

        #endregion

        #region CopyToItem

        internal void CopyToItem(GaugeSectionCollection copy)
        {
            foreach (GaugeSection item in this)
            {
                GaugeSection ic = new GaugeSection();

                item.CopyToItem(ic);

                copy.Add(ic);
            }
        }

        #endregion
    }

    [TypeConverter(typeof(GaugeSectionConvertor))]
    public class GaugeSection : GaugeStrip
    {
        #region Private variables

        private float _Width;
        private int _AbsWidth;

        #endregion

        public GaugeSection(GaugeScale scale)
            : base(scale)
        {
        }

        public GaugeSection()
        {
        }

        #region Public properties

        #region Width

        /// <summary>
        /// Gets or sets the Width of the Section, specified as a percentage
        /// </summary>
        [Browsable(true)]
        [Category("Layout"), DefaultValue(.0f)]
        [Editor("DevComponents.Instrumentation.Design.WidthRangeValueEditor, DevComponents.Instrumentation.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=76cb4c6eb576bca5", typeof(UITypeEditor))]
        [Description("Indicates the Width of the Section, specified as a percentage.")]
        public float Width
        {
            get { return (_Width); }

            set
            {
                if (value < 0 || value > 1)
                    throw new ArgumentException("Width must be between 0 and 1.");

                if (_Width != value)
                {
                    _Width = value;

                    OnGaugeItemChanged();
                }
            }
        }

        #endregion

        #endregion

        #region RecalcLayout

        public override void RecalcLayout()
        {
            base.RecalcLayout();

            if (Scale is GaugeCircularScale)
                CalcCircularLayout(Scale as GaugeCircularScale);

            else if (Scale is GaugeLinearScale)
                CalcLinearLayout(Scale as GaugeLinearScale);
        }

        #region CalcCircularLayout

        private void CalcCircularLayout(GaugeCircularScale scale)
        {
            int radius = scale.AbsRadius;
            int scaleOffset = (int)(radius * ScaleOffset);

            _AbsWidth = (int)(radius * (_Width > 0 ? _Width : scale.Width));

            Rectangle r = scale.Bounds;
            r.Inflate(scaleOffset, scaleOffset);

            Bounds = r;
        }

        #endregion

        #region CalcLinearLayout

        #region CalcLinearLayout

        private void CalcLinearLayout(GaugeLinearScale scale)
        {
            float spread = (float)(scale.MaxValue - scale.MinValue);

            if (spread == 0)
                spread = 1;

            if (scale.Orientation == Orientation.Horizontal)
                CalcHorizontalLayout(scale, spread);
            else
                CalcVerticalLayout(scale, spread);
        }

        #endregion

        #region CalcHorizontalLayout

        private void CalcHorizontalLayout(GaugeLinearScale scale, float spread)
        {
            int length = scale.ScaleBounds.Width;
            int width = scale.ScaleBounds.Height;

            float dl = length / spread;

            int start = (int)(dl * MinValue);
            int len = (int)(dl * (MaxValue - MinValue));
            int offset = (int)(width * ScaleOffset);

            _AbsWidth = (int)(_Width > 0 ? width * _Width : width);

            Rectangle r = scale.Bounds;

            if (scale.Reversed == true)
                r.X = r.Right - (start + len);
            else
                r.X += start;

            r.Width = len;
            r.Y = scale.Center.Y - (_AbsWidth / 2) + offset;
            r.Height = _AbsWidth;

            Bounds = r;
        }

        #endregion

        #region CalcVerticalLayout

        private void CalcVerticalLayout(GaugeLinearScale scale, float spread)
        {
            int length = scale.ScaleBounds.Height;
            int width = scale.ScaleBounds.Width;

            float dl = length / spread;

            int start = (int)(dl * MinValue);
            int len = (int)(dl * (MaxValue - MinValue));
            int offset = (int)(length * ScaleOffset);

            _AbsWidth = (int)(_Width > 0 ? width * _Width : width);

            Rectangle r = scale.Bounds;

            if (scale.Reversed == true)
                r.Y += start;
            else
                r.Y = r.Bottom - (start + len);

            r.X = scale.Center.X - (_AbsWidth / 2) + offset;
            r.Width = _AbsWidth;
            r.Height = len;

            Bounds = r;
        }

        #endregion

        #endregion

        #endregion

        #region OnPaint

        public override void OnPaint(PaintEventArgs e)
        {
            RecalcLayout();

            if (Scale.GaugeControl.OnPreRenderScaleSection(e, this) == false)
            {
                if (Scale is GaugeCircularScale)
                {
                    if (SweepAngle != 0)
                        PaintCircularSection(e, Scale as GaugeCircularScale);
                }
                else if (Scale is GaugeLinearScale)
                {
                    PaintLinearSection(e, Scale as GaugeLinearScale);
                }

                Scale.GaugeControl.OnPostRenderScaleSection(e, this);
            }
        }

        #endregion

        #region PaintCircularSection

        #region PaintCircularSection

        private void PaintCircularSection(PaintEventArgs e, GaugeCircularScale scale)
        {
            Graphics g = e.Graphics;

            int radius = scale.AbsRadius;
            int n = (int)(radius * (_Width > 0 ? _Width : scale.Width));

            if (n > 0 && Bounds.Width > 0 && Bounds.Height > 0 && Math.Abs(SweepAngle) > .05)
            {
                if (FillColor.End.IsEmpty == true || FillColor.Color1 == FillColor.Color2 ||
                    FillColor.GradientFillType == GradientFillType.None)
                {
                    using (Pen pen = new Pen(FillColor.Color1, n))
                        g.DrawArc(pen, Bounds, StartAngle, SweepAngle);
                }
                else
                {
                    switch (FillColor.GradientFillType)
                    {
                        case GradientFillType.Auto:
                        case GradientFillType.StartToEnd:
                            PaintCircularStartToEnd(g, Bounds, n, scale);
                            break;

                        case GradientFillType.Angle:
                            PaintCircularByAngle(g, Bounds, n);
                            break;

                        case GradientFillType.Center:
                            PaintCircularByCenter(g, Bounds, n);
                            break;

                        case GradientFillType.HorizontalCenter:
                            PaintCircularByHc(g, Bounds, n);
                            break;

                        case GradientFillType.VerticalCenter:
                            PaintCircularByVc(g, Bounds, n);
                            break;
                    }
                }
            }
        }

        #endregion

        #region PaintCircularStartToEnd

        private void PaintCircularStartToEnd(Graphics g,
            Rectangle r, int n, GaugeCircularScale scale)
        {
            using (PathGradientBrush br = scale.CreateGradient(
                Scale.GaugeControl.Frame.Bounds, StartAngle, SweepAngle, FillColor, 10))
            {
                using (Pen pen = new Pen(br, n))
                    g.DrawArc(pen, r, StartAngle, SweepAngle);
            }
        }

        #endregion

        #region PaintCircularByAngle

        private void PaintCircularByAngle(Graphics g, Rectangle r, int n)
        {
            Rectangle t = r;
            t.Inflate(10, 10);

            using (Brush br = FillColor.GetBrush(t))
            {
                using (Pen pen = new Pen(br, n))
                    g.DrawArc(pen, r, StartAngle, SweepAngle);
            }
        }

        #endregion

        #region PaintCircularByCenter

        private void PaintCircularByCenter(Graphics g, Rectangle r, int n)
        {
            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddArc(r, StartAngle, SweepAngle);

                using (Pen pen = new Pen(Color.Black, n))
                    path.Widen(pen);

                using (PathGradientBrush br = new PathGradientBrush(path))
                {
                    br.CenterColor = FillColor.Color1;
                    br.SurroundColors = new Color[] {FillColor.Color2};
                    br.CenterPoint = Scale.Center;

                    float m = (float)n / (r.Width / 2);

                    Blend blnd = new Blend();
                    blnd.Positions = new float[] { 0f, m, 1f };
                    blnd.Factors = new float[] { 1f, 0f, 0f };
                    br.Blend = blnd;

                    g.FillPath(br, path);
                }
            }
        }

        #endregion

        #region PaintCircularByHc

        private void PaintCircularByHc(Graphics g, Rectangle r, int n)
        {
            Rectangle t = r;
            t.Height /= 2;

            using (LinearGradientBrush br = new
                LinearGradientBrush(t, FillColor.Color1, FillColor.Color2, 90))
            {
                br.WrapMode = WrapMode.TileFlipXY;

                using (Pen pen = new Pen(br, n))
                    g.DrawArc(pen, r, StartAngle, SweepAngle);
            }
        }

        #endregion

        #region PaintCircularByVc

        private void PaintCircularByVc(Graphics g, Rectangle r, int n)
        {
            Rectangle t = r;
            t.Width /= 2;

            using (LinearGradientBrush br = new
                LinearGradientBrush(t, FillColor.Color1, FillColor.Color2, 0f))
            {
                br.WrapMode = WrapMode.TileFlipXY;

                using (Pen pen = new Pen(br, n))
                    g.DrawArc(pen, r, StartAngle, SweepAngle);
            }
        }

        #endregion

        #endregion

        #region PaintLinearSection

        #region PaintLinearSection

        private void PaintLinearSection(PaintEventArgs e, GaugeLinearScale scale)
        {
            Graphics g = e.Graphics;

            if (Bounds.Width > 0 && Bounds.Height > 0)
            {
                if (FillColor.End.IsEmpty == true || FillColor.Color1 == FillColor.Color2 ||
                    FillColor.GradientFillType == GradientFillType.None)
                {
                    using (Brush br = new SolidBrush(FillColor.Color1))
                        g.FillRectangle(br, Bounds);
                }
                else
                {
                    switch (FillColor.GradientFillType)
                    {
                        case GradientFillType.Auto:
                        case GradientFillType.StartToEnd:
                            PaintLinearStartToEnd(g, Bounds, scale);
                            break;

                        case GradientFillType.Angle:
                            PaintLinearByAngle(g, Bounds);
                            break;

                        case GradientFillType.Center:
                            PaintLinearByCenter(g, Bounds);
                            break;

                        case GradientFillType.HorizontalCenter:
                            PaintLinearByHc(g, Bounds);
                            break;

                        case GradientFillType.VerticalCenter:
                            PaintLinearByVc(g, Bounds);
                            break;
                    }
                }
            }
        }

        #endregion

        #region PaintLinearStartToEnd

        private void PaintLinearStartToEnd(
            Graphics g, Rectangle r, GaugeLinearScale scale)
        {
            int angle = scale.Orientation == Orientation.Horizontal ? 0 : -90;

            if (scale.Reversed == true)
                angle += 180;

            using (Brush br = FillColor.GetBrush(r, angle))
                g.FillRectangle(br, r);
        }

        #endregion

        #region PaintLinearByAngle

        private void PaintLinearByAngle(Graphics g, Rectangle r)
        {
            using (Brush br = FillColor.GetBrush(r))
                g.FillRectangle(br, r);
        }

        #endregion

        #region PaintLinearByCenter

        private void PaintLinearByCenter(Graphics g, Rectangle r)
        {
            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddRectangle(r);

                using (PathGradientBrush br = new PathGradientBrush(path))
                {
                    Point pt = new Point(r.X + r.Width / 2, r.Y + r.Height / 2);

                    br.CenterColor = FillColor.Color1;
                    br.SurroundColors = new Color[] { FillColor.Color2 };
                    br.CenterPoint = pt;

                    g.FillPath(br, path);
                }
            }
        }

        #endregion

        #region PaintLinearByHc

        private void PaintLinearByHc(Graphics g, Rectangle r)
        {
            Rectangle t = r;

            if (r.Height >= 2)
                t.Height /= 2;

            using (LinearGradientBrush br = new
                LinearGradientBrush(t, FillColor.Color1, FillColor.Color2, 90))
            {
                br.WrapMode = WrapMode.TileFlipXY;

                g.FillRectangle(br, r);
            }
        }

        #endregion

        #region PaintLinearByVc

        private void PaintLinearByVc(Graphics g, Rectangle r)
        {
            Rectangle t = r;

            if (t.Width >= 2)
                t.Width /= 2;

            using (LinearGradientBrush br = new
                LinearGradientBrush(t, FillColor.Color1, FillColor.Color2, 0f))
            {
                br.WrapMode = WrapMode.TileFlipXY;

                g.FillRectangle(br, r);
            }
        }

        #endregion

        #endregion

        #region FindItem

        internal override GaugeItem FindItem(Point pt)
        {
            GraphicsPath path = GetSectionPath();

            if (path != null)
            {
                if (path.IsVisible(pt) == true)
                    return (this);
            }

            return (null);
        }

        #endregion

        #region GetSectionPath

        public GraphicsPath GetSectionPath()
        {
            if (StripePath == null)
            {
                if (Scale is GaugeCircularScale)
                    StripePath = GetCSectionPath(Scale as GaugeCircularScale);
                else
                    StripePath = GetLSectionPath();
            }

            return (StripePath);
        }

        #endregion

        #region GetCSectionPath

        private GraphicsPath GetCSectionPath(GaugeCircularScale scale)
        {
            int radius = scale.AbsRadius;
            int n = (int)(radius * (_Width > 0 ? _Width : scale.Width));

            if (n > 0 && Bounds.Width > 0 && Bounds.Height > 0)
            {
                GraphicsPath path = new GraphicsPath();

                Rectangle r = Bounds;

                r.Inflate(n / 2, n / 2);
                path.AddArc(r, StartAngle, SweepAngle);

                r.Inflate(-n, -n);
                path.AddArc(r, StartAngle + SweepAngle, -SweepAngle);

                path.CloseAllFigures();

                return (path);
            }

            return (null);
        }

        #endregion

        #region GetLSectionPath

        private GraphicsPath GetLSectionPath()
        {
            if (Bounds.Width > 0 && Bounds.Height > 0)
            {
                GraphicsPath path = new GraphicsPath();

                path.AddRectangle(Bounds);

                return (path);
            }

            return (null);
        }

        #endregion

        #region ICloneable Members

        public override object Clone()
        {
            GaugeSection copy = new GaugeSection();

            CopyToItem(copy);

            return (copy);
        }

        #endregion

        #region CopyToItem

        public override void CopyToItem(GaugeItem copy)
        {
            GaugeSection c = copy as GaugeSection;

            if (c != null)
            {
                base.CopyToItem(c);

                c.Width = _Width;
            }
        }

        #endregion
    }

    #region GaugeScaleConvertor

    public class GaugeSectionConvertor : ExpandableObjectConverter
    {
        public override object ConvertTo(
            ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                GaugeSection section = value as GaugeSection;

                if (section != null)
                {
                    //ColorConverter cvt = new ColorConverter();

                    //if (lct.Start != Color.Empty)
                    //    return (cvt.ConvertToString(lct.Start));

                    //if (lct.End != Color.Empty)
                    //    return (cvt.ConvertToString(lct.End));

                    //if (lct.GradientAngle != 90)
                    //    return (lct.GradientAngle.ToString());

                    return (String.Empty);
                }
            }

            return (base.ConvertTo(context, culture, value, destinationType));
        }
    }

    #endregion
}
