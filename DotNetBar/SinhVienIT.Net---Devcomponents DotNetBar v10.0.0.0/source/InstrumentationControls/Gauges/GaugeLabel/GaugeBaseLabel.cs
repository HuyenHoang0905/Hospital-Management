using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace DevComponents.Instrumentation
{
    [TypeConverter(typeof(GaugeBaseLabelConvertor))]
    public class GaugeBaseLabel : GaugeItem
    {
        #region Private variables

        private int _Radius;
        private int _Offset;

        private LabelLayout _Layout;
        private GaugeScale _Scale;

        #endregion

        public GaugeBaseLabel()
        {
            _Layout = new LabelLayout();

            HookEvents(true);
        }

        #region Hidden properties

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

        #region Layout

        /// <summary>
        /// Gets the label Layout
        /// </summary>
        [Browsable(true), Category("Appearance")]
        [Description("Contains the Label layout properties.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public LabelLayout Layout
        {
            get { return (_Layout); }
        }

        #endregion

        #region Scale

        /// <summary>
        /// Gets the associated Scale
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual GaugeScale Scale
        {
            get { return (_Scale); }

            internal set
            {
                _Scale = value;

                OnGaugeItemChanged(true);
            }
        }

        #endregion

        #endregion

        #region Internal properties

        #region AbsFont

        internal Font AbsFont
        {
            get
            {
                if (_Layout.AutoSize == false)
                    return (_Layout.Font);

                if (_Layout.AbsFont == null)
                {
                    if (Scale is GaugeCircularScale)
                        _Layout.AbsFont = GetAbsFont(Scale as GaugeCircularScale);

                    else if (Scale is GaugeLinearScale)
                        _Layout.AbsFont = GetAbsFont(Scale as GaugeLinearScale);
                }

                return (_Layout.AbsFont);
            }

            set { _Layout.AbsFont = value; }
        }

        private Font GetAbsFont(GaugeCircularScale scale)
        {
            float emSize = _Layout.Font.SizeInPoints;
            emSize = (emSize / 120) * scale.AbsRadius;

            if (emSize <= 0)
                emSize = 1;

            return (new Font(_Layout.Font.FontFamily, emSize, _Layout.Font.Style));
        }

        private Font GetAbsFont(GaugeLinearScale scale)
        {
            float emSize = _Layout.Font.SizeInPoints;

            emSize = (emSize / 120) * scale.AbsWidth;

            return (new Font(_Layout.Font.FontFamily, emSize, _Layout.Font.Style));
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

        #endregion

        #region HookEvents

        private void HookEvents(bool hook)
        {
            if (hook == true)
            {
                _Layout.LabelLayoutChanged += Layout_LabelLayoutChanged;
            }
            else
            {
                _Layout.LabelLayoutChanged -= Layout_LabelLayoutChanged;
            }
        }

        #endregion

        #region Event handling

        void Layout_LabelLayoutChanged(object sender, EventArgs e)
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

                CalcLabelMetrics();

                AbsFont = null;
                _Layout.AbsFont = null;
            }
        }

        #region CalcLabelMetrics

        private void CalcLabelMetrics()
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

            if (_Radius > 0)
            {
                int offset = (int)(_Radius * _Layout.ScaleOffset);

                switch (_Layout.Placement)
                {
                    case DisplayPlacement.Near:
                        _Radius = scale.GetNearLabelRadius() - offset;
                        break;

                    case DisplayPlacement.Center:
                        _Radius += offset;
                        break;

                    case DisplayPlacement.Far:
                        _Radius = scale.GetFarLabelRadius() + offset;
                        break;
                }
            }
        }

        #endregion

        #region CalcLinearMetrics

        private void CalcLinearMetrics(GaugeLinearScale scale)
        {
            int width = scale.AbsWidth;
            int offset = (int)(width * _Layout.ScaleOffset);

            switch (_Layout.Placement)
            {
                case DisplayPlacement.Near:
                    _Offset = GetNearLabelOffset(scale) - offset;
                    break;

                case DisplayPlacement.Center:
                    _Offset = scale.AbsScaleWidth / 2 - offset;
                    break;

                case DisplayPlacement.Far:
                    _Offset = GetFarLabelOffset(scale) + offset;
                    break;
            }
        }

        #region GetNearLabelOffset

        private int GetNearLabelOffset(GaugeLinearScale scale)
        {
            int offset = 0;

            if (scale.MajorTickMarks.Visible &&
                scale.MajorTickMarks.Layout.Placement == DisplayPlacement.Near
                && scale.MajorTickMarks.Offset < offset)
            {
                offset = scale.MajorTickMarks.Offset;
            }

            if (scale.MinorTickMarks.Visible &&
                scale.MinorTickMarks.Layout.Placement == DisplayPlacement.Near &&
                scale.MinorTickMarks.Offset < offset)
            {
                offset = scale.MinorTickMarks.Offset;
            }

            return (offset);
        }

        #endregion

        #region GetFarLabelOffset

        private int GetFarLabelOffset(GaugeLinearScale scale)
        {
            int offset = scale.AbsScaleWidth;

            if (scale.MajorTickMarks.Visible &&
                scale.MajorTickMarks.Layout.Placement == DisplayPlacement.Far)
            {
                int n = scale.MajorTickMarks.Offset + scale.MajorTickMarks.Length;

                if (n > offset)
                    offset = n;
            }

            if (scale.MinorTickMarks.Visible &&
                scale.MinorTickMarks.Layout.Placement == DisplayPlacement.Far)
            {
                int n = scale.MinorTickMarks.Offset + scale.MinorTickMarks.Length;

                if (n > offset)
                    offset = n;
            }

            return (offset);
        }

        #endregion

        #endregion

        #endregion

        #endregion

        #region PaintLabel

        internal void PaintLabel(Graphics g,
            string text, Brush br, LabelPoint lp, Font font)
        {
            if (Scale.Style == GaugeScaleStyle.Circular)
            {
                if (Layout.AdaptiveLabel == true)
                {
                    PaintAdaptiveLabel(g, text, br, lp, font);
                }
                else
                {
                    if (Layout.RotateLabel == true)
                        PaintRotatedLabel(g, text, br, lp, font);
                    else
                        PaintNonRotatedLabel(g, text, br, lp, font);
                }
            }
            else
            {
                PaintRotatedLabel(g, text, br, lp, font);
            }
        }

        #endregion

        #region PaintRotatedLabel

        internal void PaintRotatedLabel(Graphics g,
            string text, Brush br, LabelPoint lp, Font font)
        {
            if (Scale is GaugeCircularScale)
                PaintCircularRotatedLabel(g, text, br, lp, font);

            else if (Scale is GaugeLinearScale)
                PaintLinearRotatedLabel(g, text, br, lp, font, Scale as GaugeLinearScale);
        }

        #region PaintCircularRotatedLabel

        private void PaintCircularRotatedLabel(
            Graphics g, string text, Brush br, LabelPoint lp, Font font)
        {
            SizeF sz = g.MeasureString(text, font);
            Size size = sz.ToSize();

            float fontAngle = Layout.Angle;

            if (Layout.AutoOrientLabel == true)
            {
                if (((fontAngle + lp.Angle) % 360) < 180)
                    fontAngle += 180;
            }

            g.TranslateTransform(lp.Point.X, lp.Point.Y);
            g.RotateTransform((lp.Angle + 90) % 360);

            g.TranslateTransform(0, GetRadiusDelta(size.Width, size.Height, fontAngle));
            g.RotateTransform(fontAngle % 360);

            g.DrawString(text, font, br,
                new Point(-size.Width / 2, -size.Height / 2));

            g.ResetTransform();
        }

        #endregion

        #region PaintLinearRotatedLabel

        private void PaintLinearRotatedLabel(Graphics g, string text,
            Brush br, LabelPoint lp, Font font, GaugeLinearScale scale)
        {
            SizeF sz = g.MeasureString(text, font);
            Size size = sz.ToSize();

            float fontAngle = Layout.Angle;

            g.TranslateTransform(lp.Point.X, lp.Point.Y);

            if (scale.Orientation == Orientation.Horizontal)
                g.TranslateTransform(0, -GetRadiusDelta(size.Width, size.Height, fontAngle));
            else
                g.TranslateTransform(-GetRadiusDelta(size.Height, size.Width, fontAngle), 0);

            g.RotateTransform(fontAngle % 360);

            g.DrawString(text, font, br,
                         new Point(-size.Width / 2, -size.Height / 2));

            g.ResetTransform();
        }

        #endregion

        #region GetRadiusDelta

        private int GetRadiusDelta(int width, int height, float fontAngle)
        {
            if (Layout.Placement == DisplayPlacement.Center)
                return (0);

            float spd = (float)((width / 2) - (height / 2)) / 90;
            float angle = fontAngle % 180;

            int delta = (int)((height / 2) + 
                ((angle > 90) ? (180 - angle) : angle) * spd) + 2;

            return (Layout.Placement == DisplayPlacement.Near ? delta : - delta);
        }

        #endregion

        #endregion

        #region PaintNonRotatedLabel

        internal void PaintNonRotatedLabel(Graphics g,
            string text, Brush br, LabelPoint lp, Font font)
        {
            SizeF sz = g.MeasureString(text, font);
            Size size = sz.ToSize();

            float fontAngle = 360 - ((lp.Angle + Layout.Angle + 90) % 360);

            g.TranslateTransform(lp.Point.X, lp.Point.Y);
            g.RotateTransform((lp.Angle + 90) % 360);

            g.TranslateTransform(0, GetRadiusDelta(size.Width, size.Height, fontAngle));
            g.RotateTransform(fontAngle % 360);

            g.DrawString(text, font, br,
                new Point(-size.Width / 2, -size.Height / 2));

            g.ResetTransform();
        }

        #endregion

        #region PaintAdaptiveLabel

        internal void PaintAdaptiveLabel(Graphics g,
            string text, Brush br, LabelPoint lp, Font font)
        {
            SizeF tw = g.MeasureString(text, font);

            if (Layout.Placement == DisplayPlacement.Near)
                tw.Width += text.Length;

            float c = (float)(Math.PI * _Radius * 2);

            if (c > 0)
            {
                float radians = (float)GetRadians(lp.Angle);
                float radOffset = (float)GetRadians((180 * tw.Width) / c);
                float radCenter = radians;

                radians -= radOffset;

                bool flip = false;

                if (_Layout.AutoOrientLabel == true)
                {
                    flip = ((GetDegrees(radCenter) % 360) < 180);

                    if (flip == true)
                        radians += (radOffset * 2);
                }

                int n = (int)Math.Ceiling((double)text.Length / 32);

                for (int i = 0; i < n; i++)
                {
                    int len = Math.Min(text.Length - (i * 32), 32);

                    radians = PaintAdaptiveText(g,
                        text.Substring(i * 32, len), br, font, radians, flip);
                }
            }
        }

        #endregion

        #region PaintAdaptiveText

        private float PaintAdaptiveText(Graphics g,
            string text, Brush br, Font font, float radians, bool flip)
        {
            CharacterRange[] crs = new CharacterRange[text.Length];

            for (int j = 0; j < text.Length; j++)
                crs[j] = new CharacterRange(j, 1);

            using (StringFormat sf = new StringFormat())
            {
                sf.FormatFlags = StringFormatFlags.NoClip | StringFormatFlags.MeasureTrailingSpaces;
                sf.SetMeasurableCharacterRanges(crs);

                Rectangle r = new Rectangle(0, 0, 1000, 1000);
                Region[] rgns = g.MeasureCharacterRanges(text, font, r, sf);

                for (int j = 0; j < text.Length; j++)
                {
                    RectangleF t = rgns[j].GetBounds(g);

                    float z = (flip ? _Radius - t.Height / 2 : _Radius + t.Height / 2);

                    switch (Layout.Placement)
                    {
                        case DisplayPlacement.Near:
                            t.Width += 1;
                            z -= t.Height/2;
                            break;

                        case DisplayPlacement.Far:
                            z += t.Height/2;
                            break;
                    }

                    float y = (float)(_Scale.Center.Y + z * Math.Sin(radians));
                    float x = (float)(_Scale.Center.X + z * Math.Cos(radians));

                    float rad = (float)Math.Asin(t.Width / (_Radius * 2));

                    radians += (flip ? -rad : rad);

                    g.TranslateTransform(x, y);
                    g.RotateTransform((flip ? -90 : 90) + (float)GetDegrees(radians));

                    g.DrawString(text[j].ToString(), font, br, 0, 0);

                    g.ResetTransform();

                    radians += (flip ? -rad : rad);
                }
            }

            return (radians);
        }

        #endregion

        #region GetDegrees

        internal double GetDegrees(float radians)
        {
            return (radians * 180 / Math.PI);
        }

        #endregion

        #region GetRadians

        /// <summary>
        /// Converts Degrees to Radians
        /// </summary>
        /// <param name="theta">Degrees</param>
        /// <returns>Radians</returns>
        internal double GetRadians(float theta)
        {
            return (theta * Math.PI / 180);
        }

        #endregion

        #region OnDispose

        protected override void OnDispose()
        {
            HookEvents(false);

            base.OnDispose();
        }

        #endregion

        #region ICloneable Members

        public override object Clone()
        {
            GaugeBaseLabel copy = new GaugeBaseLabel();

            CopyToItem(copy);

            return (copy);
        }

        #endregion

        #region CopyToItem

        public override void CopyToItem(GaugeItem copy)
        {
            GaugeBaseLabel c = copy as GaugeBaseLabel;

            if (c != null)
            {
                base.CopyToItem(c);

                _Layout.CopyToItem(c.Layout);
            }
        }

        #endregion
    }

    #region GaugeBaseLabelConvertor

    public class GaugeBaseLabelConvertor : ExpandableObjectConverter
    {
        public override object ConvertTo(
            ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                GaugeBaseLabel label = value as GaugeBaseLabel;

                if (label != null)
                    return (String.Empty);
            }

            return (base.ConvertTo(context, culture, value, destinationType));
        }
    }

    #endregion

}
