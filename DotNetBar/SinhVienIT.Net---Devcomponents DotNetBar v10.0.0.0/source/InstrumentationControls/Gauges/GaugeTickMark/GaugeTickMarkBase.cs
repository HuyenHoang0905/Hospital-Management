using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace DevComponents.Instrumentation
{
    [TypeConverter(typeof(GaugeTickMarkBaseConvertor))]
    public class GaugeTickMarkBase : GaugeItem
    {
        #region Private variables

        private int _Radius;
        private int _Width;
        private int _Length;

        private int _Offset;
        private Rectangle _Bounds;

        private TickMarkLayout _Layout;
        private GaugeTickMarkRank _Rank;
        private GaugeMarker _GaugeMarker;

        private GaugeScale _Scale;

        #endregion

        public GaugeTickMarkBase(GaugeScale scale,
            GaugeTickMarkRank rank, GaugeMarkerStyle style, float width, float length)
            : this(scale, rank, new TickMarkLayout(style, width, length))
        {
        }

        public GaugeTickMarkBase(GaugeScale scale, GaugeTickMarkRank rank, TickMarkLayout layout)
        {
            _Scale = scale;
            _Rank = rank;

            _Layout = layout;
            _GaugeMarker = new GaugeMarker();

            HookEvents(true);
        }

        #region Hidden properties

        #region Name

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new string Name
        {
            get { return (base.Name); }
            set { base.Name = value; }
        }

        #endregion

        #region Tooltip

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new string Tooltip
        {
            get { return (base.Tooltip); }
            set { base.Tooltip = value; }
        }

        #endregion

        #endregion

        #region Public properties

        #region Scale

        /// <summary>
        /// Gets the associated Scale
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public GaugeScale Scale
        {
            get { return (_Scale); }
            internal set { _Scale = value; }
        }

        #endregion

        #region Layout

        /// <summary>
        /// Gets the Tickmark leyout
        /// </summary>
        [Browsable(true), Category("Layout")]
        [Description("Specifies the TickMark layout properties.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TickMarkLayout Layout
        {
            get { return (_Layout); }
        }

        #endregion

        #region Visible

        public override bool Visible
        {
            get { return (base.Visible); }

            set
            {
                if (base.Visible != value)
                {
                    base.Visible = value;

                    if (_Scale != null)
                        _Scale.Labels.NeedRecalcLayout = true;
                }
            }
        }

        #endregion

        #endregion

        #region Internal properties

        #region Bounds

        internal Rectangle Bounds
        {
            get { return (_Bounds); }
            set { _Bounds = value; }
        }

        #endregion

        #region Length

        internal int Length
        {
            get { return (_Length); }
            set { _Length = value; }
        }

        #endregion

        #region Offset

        internal int Offset
        {
            get { return (_Offset); }
            set { _Offset = value; }
        }

        #endregion

        #region Radius

        internal int Radius
        {
            get { return (_Radius); }
        }

        #endregion

        #region Rank

        internal GaugeTickMarkRank Rank
        {
            get { return (_Rank); }
        }

        #endregion

        #region Width

        internal int Width
        {
            get { return (_Width); }
            set { _Width = value; }
        }

        #endregion

        #endregion

        #region HookEvents

        private void HookEvents(bool hook)
        {
            if (hook == true)
            {
                _Layout.TickMarkLayoutChanged += TickMarkLayout_TickMarkLayoutChanged;
            }
            else
            {
                _Layout.TickMarkLayoutChanged -= TickMarkLayout_TickMarkLayoutChanged;
            }
        }

        #endregion

        #region Event processing

        void TickMarkLayout_TickMarkLayoutChanged(object sender, EventArgs e)
        {
            OnGaugeItemChanged(true);
        }

        #endregion

        #region RecalcLayout

        public override void RecalcLayout()
        {
            if (NeedRecalcLayout == true)
            {
                base.RecalcLayout();

                CalcTickMarkMetrics();

                _GaugeMarker.Clear();
            }
        }

        #region CalcTickMarkMetrics

        private void CalcTickMarkMetrics()
        {
            if (Scale is GaugeCircularScale)
                CalcCircularMetrics(Scale as GaugeCircularScale);

            else if (Scale is GaugeLinearScale)
                CalcLinearMetrics(Scale as GaugeLinearScale);
        }

        #region CalcCircularMetrics

        private void CalcCircularMetrics(GaugeCircularScale scale)
        {
            _Radius = scale.AbsRadius;
            _Length = (int)(_Radius * _Layout.Length);
            _Width = (int)(_Radius * _Layout.Width);

            int m = scale.AbsScaleWidth;
            int offset = (int)(scale.AbsRadius * _Layout.ScaleOffset);

            switch (_Layout.Placement)
            {
                case DisplayPlacement.Near:
                    _Radius -= ((_Length + m / 2) + offset);
                    break;

                case DisplayPlacement.Center:
                    _Radius += ((_Length / 2) + offset + 1);
                    break;

                case DisplayPlacement.Far:
                    _Radius += ((_Length + m / 2) + offset);
                    break;
            }
        }

        #endregion

        #region CalcLinearMetrics

        private void CalcLinearMetrics(GaugeLinearScale scale)
        {
            if (scale.Orientation == Orientation.Horizontal)
                CalcHorizontalLayout(scale);
            else
                CalcVerticalLayout(scale);
        }

        #region CalcHorizontalLayout

        private void CalcHorizontalLayout(GaugeLinearScale scale)
        {
            int n = scale.AbsWidth;

            _Length = (int)(n * _Layout.Length);
            _Width = (int)(n * _Layout.Width);

            if (_Layout.Length > 0 && _Length < 2)
                _Length = 2;

            if (_Layout.Width > 0 && _Width < 2)
                _Width = 2;

            int offset = (int) (n * _Layout.ScaleOffset);

            _Bounds = scale.ScaleBounds;
            _Bounds.Height = _Length;

            switch (_Layout.Placement)
            {
                case DisplayPlacement.Near:
                    _Bounds.Y = scale.ScaleBounds.Top - _Length - offset;
                    break;

                case DisplayPlacement.Center:
                    _Bounds.Y = scale.Center.Y - _Length / 2 - offset;
                    break;

                case DisplayPlacement.Far:
                    _Bounds.Y = scale.ScaleBounds.Bottom + offset;
                    break;
            }

            _Offset = _Bounds.Y - scale.ScaleBounds.Y;
        }

        #endregion

        #region CalcVerticalLayout

        private void CalcVerticalLayout(GaugeLinearScale scale)
        {
            int n = scale.AbsWidth;

            _Length = (int)(n * _Layout.Length);
            _Width = (int)(n * _Layout.Width);

            if (_Layout.Length > 0 && _Length < 2)
                _Length = 2;

            if (_Layout.Width > 0 && _Width < 2)
                _Width = 2;

            int offset = (int)(n * _Layout.ScaleOffset);

            _Bounds = scale.ScaleBounds;
            _Bounds.Width = _Length;

            switch (_Layout.Placement)
            {
                case DisplayPlacement.Near:
                    _Bounds.X = scale.ScaleBounds.Left - _Length - offset;
                    break;

                case DisplayPlacement.Center:
                    _Bounds.X = scale.Center.X - _Length / 2 - offset;
                    break;

                case DisplayPlacement.Far:
                    _Bounds.X = scale.ScaleBounds.Right + offset;
                    break;
            }

            _Offset = _Bounds.X - scale.ScaleBounds.X;
        }

        #endregion

        #endregion

        #endregion

        #endregion

        #region Paint support

        #region PaintTickPoint

        #region PaintTickPoint

        internal void PaintTickPoint(Graphics g, TickPoint tp)
        {
            Image image = _Layout.Image ?? GetTickMarkBitmap(g, tp);

            if (image != null)
            {
                if (Scale is GaugeCircularScale)
                    PaintCircularTickPoint(g, tp, image);

                else if (Scale is GaugeLinearScale)
                    PaintLinearTickPoint(g, tp, image, Scale as GaugeLinearScale);
            }
        }

        #endregion

        #region PaintCircularTickPoint

        private void PaintCircularTickPoint(Graphics g, TickPoint tp, Image image)
        {
            Rectangle r = new Rectangle(0, 0, _Width, _Length);

            float angle = tp.Angle + 90;

            if (_Layout.Placement == DisplayPlacement.Near)
                angle += 180;

            g.TranslateTransform(tp.Point.X, tp.Point.Y);
            g.RotateTransform(angle % 360);

            r.X -= _Width / 2;

            g.DrawImage(image, r);
            g.ResetTransform();
        }

        #endregion

        #region PaintLinearTickPoint

        private void PaintLinearTickPoint(Graphics g,
            TickPoint tp, Image image, GaugeLinearScale scale)
        {
            if (scale.Orientation == Orientation.Horizontal)
                PaintHorizontalTickPoint(g, tp, image);
            else
                PaintVerticalTickPoint(g, tp, image);
        }

        #region PaintHorizontalTickPoint

        private void PaintHorizontalTickPoint(Graphics g, TickPoint tp, Image image)
        {
            if (_Layout.Placement != DisplayPlacement.Far)
            {
                Rectangle r = new Rectangle(tp.Point.X, tp.Point.Y, _Width, _Length);
                r.X -= _Width / 2;

                g.DrawImage(image, r);
            }
            else
            {
                Rectangle r = new Rectangle(0, 0, _Width, _Length);

                g.TranslateTransform(tp.Point.X, tp.Point.Y + _Length - (_Length % 2));
                g.RotateTransform(180);

                r.X -= _Width / 2;

                g.DrawImage(image, r);
                g.ResetTransform();
            }
        }

        #endregion

        #region PaintVerticalTickPoint

        private void PaintVerticalTickPoint(Graphics g, TickPoint tp, Image image)
        {
            Rectangle r = new Rectangle(0, 0, _Width, _Length);

            if (_Layout.Placement == DisplayPlacement.Far)
            {
                g.TranslateTransform(tp.Point.X + _Length - (_Length % 2), tp.Point.Y);
                g.RotateTransform(90);
            }
            else
            {
                g.TranslateTransform(tp.Point.X, tp.Point.Y);
                g.RotateTransform(-90);
            }

            r.X -= (_Width / 2);

            g.DrawImage(image, r);
            g.ResetTransform();
        }

        #endregion

        #endregion

        #endregion

        #region GetTickMarkBitmap

        internal Bitmap GetTickMarkBitmap(Graphics g, TickPoint tp)
        {
            if (_Layout.Style != GaugeMarkerStyle.None)
            {
                if (_Width > 0 && _Length > 0)
                {
                    return (_GaugeMarker.GetMarkerBitmap(g, _Layout.Style,
                            GetTickMarkFillColor(tp), _Width, _Length));
                }
            }

            return (null);
        }

        #endregion

        #region GetTickMarkFillColor

        private GradientFillColor GetTickMarkFillColor(TickPoint tp)
        {
            if (_Rank == GaugeTickMarkRank.Custom)
                return (Layout.FillColor);

            ColorSourceFillEntry entry = (_Rank == GaugeTickMarkRank.Major)
                ? ColorSourceFillEntry.MajorTickMark : ColorSourceFillEntry.MinorTickMark;

            GradientFillColor fillColor = (Scale.GetRangeFillColor(tp.Interval, entry) ??
                                           Scale.GetSectionFillColor(tp.Interval, entry)) ?? Layout.FillColor;

            return (fillColor);
        }

        #endregion

        #endregion

        #region OnDispose

        protected override void OnDispose()
        {
            HookEvents(false);

            _GaugeMarker.Dispose();

            base.OnDispose();
        }

        #endregion

        #region ICloneable Members

        public override object Clone()
        {
            GaugeTickMarkBase copy = new GaugeTickMarkBase(
                _Scale, _Rank, _Layout.Style, _Layout.Width, _Layout.Length);

            CopyToItem(copy);

            return (copy);
        }

        #endregion

        #region CopyToItem

        public override void CopyToItem(GaugeItem copy)
        {
            GaugeTickMarkBase c = copy as GaugeTickMarkBase;

            if (c != null)
            {
                base.CopyToItem(c);

                _Layout.CopyToItem(c.Layout);
            }
        }

        #endregion
    }

    #region GaugeTickMarkBaseConvertor

    public class GaugeTickMarkBaseConvertor : ExpandableObjectConverter
    {
        public override object ConvertTo(
            ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                GaugeTickMarkBase tickMark = value as GaugeTickMarkBase;

                if (tickMark != null)
                {
                    return (String.Empty);
                }
            }

            return (base.ConvertTo(context, culture, value, destinationType));
        }
    }

    #endregion

    #region Enums

    public enum GaugeTickMarkRank
    {
        Major,
        Minor,
        Custom
    }

    public enum GaugeTickMarkOverlap
    {
        ReplaceNone,
        ReplaceLast,
        ReplaceAll
    }

    #endregion
}
