using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Windows.Forms;
using DevComponents.Instrumentation.Primitives;

namespace DevComponents.Instrumentation
{
    public class GaugeScaleCollection : GenericCollection<GaugeScale>
    {
    }

    public class GaugeScale : GaugeItem
    {
        #region Private variables

        private GaugeScaleStyle _Style;

        private float _Width;

        private double _MinValue;
        private double _MaxValue;

        private double _MaxLimit;
        private double _MinLimit;

        private Color _BorderColor;
        private int _BorderWidth;

        private GaugeIntervalLabel _Labels;
        private GaugeCustomLabelCollection _CustomLabels;
        private GaugePointerCollection _Pointers;
        private GaugeRangeCollection _Ranges;
        private GaugeSectionCollection _Sections;

        private GaugeTickMark _MajorTickMarks;
        private GaugeTickMark _MinorTickMarks;

        private List<TickPoint> _TickMarkList;

        private GaugePin _MaxPin;
        private GaugePin _MinPin;

        private bool _Reversed;

        private GaugeControl _GaugeControl;

        private Rectangle _Bounds;
        private Point _Center;

        #endregion

        public GaugeScale(GaugeControl gaugeControl)
            : this()
        {
            _GaugeControl = gaugeControl;
        }

        public GaugeScale()
        {
            _Style = GaugeScaleStyle.Circular;

            InitGaugeScale();

            HookEvents(true);
        }

        #region InitGaugeScale

        private void InitGaugeScale()
        {
            _Labels = new GaugeIntervalLabel(this);

            _MajorTickMarks = new GaugeTickMark(this,
                GaugeTickMarkRank.Major, GaugeMarkerStyle.Trapezoid, .09f, .14f, 10);

            _MinorTickMarks = new GaugeTickMark(this,
                GaugeTickMarkRank.Minor, GaugeMarkerStyle.Rectangle, .045f, .1f, 2);

            _MaxPin = new GaugePin(this, true, "MaxPin");
            _MinPin = new GaugePin(this, false, "MinPin");

            _BorderColor = Color.Black;
            _BorderWidth = 0;

            _MinValue = 0;
            _MaxValue = 100;

            _MinLimit = double.NaN;
            _MaxLimit = double.NaN;

            _Width = .065f;
        }

        #endregion

        #region Hidden properties

        #region Tooltip

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new string Tooltip
        {
            get { return (base.Tooltip); }
            set { base.Tooltip = value; }
        }

        #endregion

        #endregion

        #region Public properties

        #region BorderColor

        /// <summary>
        /// Gets or sets the Color of the Border
        /// </summary>
        [Browsable(true), Category("Appearance"), DefaultValue(typeof(Color), "Black")]
        [Description("Indicates the Color of the Border.")]
        public Color BorderColor
        {
            get { return (_BorderColor); }

            set
            {
                if (_BorderColor != value)
                {
                    _BorderColor = value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        #endregion

        #region BorderWidth

        /// <summary>
        /// Gets or sets the width of the Border, specified as pixels
        /// </summary>
        [Browsable(true), Category("Appearance"), DefaultValue(0)]
        [Description("Indicates the width of the Border, specified as pixels.")]
        public int BorderWidth
        {
            get { return (_BorderWidth); }

            set
            {
                if (value < 0)
                    throw new ArgumentException("Value can not be less than zero.");

                if (_BorderWidth != value)
                {                    
                    _BorderWidth = value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        #endregion

        #region Bounds

        /// <summary>
        /// Gets the Bounds of the scale
        /// </summary>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Rectangle Bounds
        {
            get { return (_Bounds); }
            internal set { _Bounds = value; }
        }

        #endregion

        #region CustomLabels

        /// <summary>
        /// Gets or sets the collection of Custom Labels
        /// </summary>
        [Browsable(true), Category("Appearance")]
        [Description("Indicates the collection of Custom Labels associated with the Scale.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Editor("DevComponents.Instrumentation.Design.GaugeCollectionEditor, DevComponents.Instrumentation.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=76cb4c6eb576bca5", typeof(UITypeEditor))]
        public GaugeCustomLabelCollection CustomLabels
        {
            get
            {
                if (_CustomLabels == null)
                {
                    _CustomLabels = new GaugeCustomLabelCollection();
                    _CustomLabels.CollectionChanged += CustomLabels_CollectionChanged;
                }

                return (_CustomLabels);
            }

            set
            {
                if (_CustomLabels != null)
                    _CustomLabels.CollectionChanged -= CustomLabels_CollectionChanged;

                _CustomLabels = value;

                if (_CustomLabels != null)
                    _CustomLabels.CollectionChanged += CustomLabels_CollectionChanged;

                OnGaugeItemChanged(true);
            }
        }                

        #endregion

        #region GaugeControl

        /// <summary>
        /// Gets the associated GaugeControl
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public GaugeControl GaugeControl
        {
            get { return (_GaugeControl); }
            internal set { _GaugeControl = value; }
        }

        #endregion

        #region Labels

        /// <summary>
        /// Gets the default Scale Label properties
        /// </summary>
        [Browsable(true), Category("Appearance")]
        [Description("Indicates the default Scale Label properties.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public GaugeIntervalLabel Labels
        {
            get { return (_Labels); }
        }

        #endregion

        #region MajorTickMarks

        /// <summary>
        ///  Gets the Scale MajorTickMarks
        /// </summary>
        [Browsable(true), Category("Appearance")]
        [Description("Indicates the Scale MajorTickMarks.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public GaugeTickMark MajorTickMarks
        {
            get { return (_MajorTickMarks); }
        }

        #endregion

        #region MinorTickMarks

        /// <summary>
        /// Gets the Scale MinorTickMarks
        /// </summary>
        [Browsable(true), Category("Appearance")]
        [Description("Indicates the Scale MinorTickMarks.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public GaugeTickMark MinorTickMarks
        {
            get { return (_MinorTickMarks); }
        }

        #endregion

        #region MaxPin

        /// <summary>
        /// Gets the properties for the Scale Maximum Pin
        /// </summary>
        [Browsable(true), Category("Layout")]
        [Description("Specifies properties for the Scale Maximum Pin.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public GaugePin MaxPin
        {
            get { return (_MaxPin); }
        }

        #endregion

        #region MinPin

        /// <summary>
        /// Gets the properties for the Scale Minimum Pin
        /// </summary>
        [Browsable(true), Category("Layout")]
        [Description("Specifies properties for the Scale Minimum Pin.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public GaugePin MinPin
        {
            get { return (_MinPin); }
        }

        #endregion

        #region MinLimit

        /// <summary>
        /// Gets or sets the absolute minimum value for the scale.
        /// Setting this values will permit the scale to scroll beyond the
        /// current MinValue range.
        /// 
        /// Both MinLimit and MaxLimit values must both be set to 
        /// valid min/max values, or both reset to their defaults (double.NaN)
        /// </summary>
        [Browsable(true), Category("Layout"), DefaultValue(double.NaN)]
        [Description("Indicates the absolute minimum value for the scale. Setting this values will permit the scale to scroll beyond the current MinValue range.")]

        public double MinLimit
        {
            get { return (_MinLimit); }

            set
            {
                if (_MinLimit != value)
                {
                    if (value.Equals(double.NaN) == false)
                    {
                        if (_MaxLimit.Equals(double.NaN) && value >= _MaxLimit)
                            throw new ArgumentException("MinLimit must be less than MaxLimit."); 

                        if (value > _MinValue)
                            throw new ArgumentException("MinLimit must be less or equal to MinValue.");
                    }

                    _MinLimit = value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        #endregion

        #region MaxLimit

        /// <summary>
        /// Gets or sets the absolute maximum value for the scale.
        /// Setting this values will permit the scale to scroll beyond the
        /// current MaxValue range.
        /// 
        /// Both MinLimit and MaxLimit values must both be set to 
        /// valid min/max values, or both reset to their defaults (double.NaN)
        /// </summary>
        [Browsable(true), Category("Layout"), DefaultValue(double.NaN)]
        [Description("Indicates the absolute minimum value for the scale. Setting this values will permit the scale to scroll beyond the current MinValue range.")]

        public double MaxLimit
        {
            get { return (_MaxLimit); }

            set
            {
                if (_MaxLimit != value)
                {
                    if (value.Equals(double.NaN) == false)
                    {
                        if (_MinLimit.Equals(double.NaN) && value <= _MinLimit)
                            throw new ArgumentException("MaxLimit must be greater than MinLimit.");

                        if (value < _MaxValue)
                            throw new ArgumentException("MaxLimit must be greater or equal to MaxValue.");
                    }

                    _MaxLimit = value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        #endregion

        #region MaxValue

        /// <summary>
        /// Gets or sets the Maximum value for the Scale
        /// </summary>
        [Browsable(true), Category("Layout"), DefaultValue(100d)]
        [Description("Indicates the Maximum value for the Scale.")]
        public double MaxValue
        {
            get { return (_MaxValue); }

            set
            {
                if (_MaxValue != value)
                {
                    if (_MaxLimit.Equals(double.NaN) == false && value > _MaxLimit)
                        throw new ArgumentException("Value must be less than or equal to MaxLimit.");
                    
                    _MaxValue = value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        #endregion

        #region MinValue

        /// <summary>
        /// Gets or sets the Minimum value for the Scale
        /// </summary>
        [Browsable(true), Category("Layout"), DefaultValue(0d)]
        [Description("Indicates the Minimum value for the Scale.")]
        public double MinValue
        {
            get { return (_MinValue); }

            set
            {
                if (_MinValue != value)
                {
                    if (_MinLimit.Equals(double.NaN) == false && value < _MinLimit)
                        throw new ArgumentException("Value must be less than or equal to MinLimit.");

                    _MinValue = value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        #endregion

        #region Pointers

        /// <summary>
        /// Gets the collection of Pointers associated with the Scale
        /// </summary>
        [Browsable(true), Category("Appearance")]
        [Description("Indicates the collection of Pointers associated with the Scale.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Editor("DevComponents.Instrumentation.Design.GaugeCollectionEditor, DevComponents.Instrumentation.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=76cb4c6eb576bca5", typeof(UITypeEditor))]
        public GaugePointerCollection Pointers
        {
            get
            {
                if (_Pointers == null)
                {
                    _Pointers = new GaugePointerCollection();
                    _Pointers.CollectionChanged += Pointers_CollectionChanged;
                }

                return (_Pointers);
            }

            set
            {
                if (_Pointers != null)
                    _Pointers.CollectionChanged -= Pointers_CollectionChanged;

                _Pointers = value;

                if (_Pointers != null)
                    _Pointers.CollectionChanged += Pointers_CollectionChanged;

                OnGaugeItemChanged(true);
            }
        }

        #endregion

        #region Ranges

        /// <summary>
        /// Gets the collection of Ranges associated with the Scale
        /// </summary>
        [Browsable(true), Category("Appearance")]
        [Description("Indicates the collection of Ranges associated with the Scale.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Editor("DevComponents.Instrumentation.Design.GaugeCollectionEditor, DevComponents.Instrumentation.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=76cb4c6eb576bca5", typeof(UITypeEditor))]
        public GaugeRangeCollection Ranges
        {
            get
            {
                if (_Ranges == null)
                {
                    _Ranges = new GaugeRangeCollection();
                    _Ranges.CollectionChanged += Ranges_CollectionChanged;
                }

                return (_Ranges);
            }

            set
            {
                if (_Ranges != null)
                    _Ranges.CollectionChanged -= Ranges_CollectionChanged;

                _Ranges = value;

                if (_Ranges != null)
                    _Ranges.CollectionChanged += Ranges_CollectionChanged;

                OnGaugeItemChanged(true);
            }
        }

        #endregion

        #region Reversed

        /// <summary>
        /// Gets or sets whether the Scale min/max direction is reversed
        /// </summary>
        [Browsable(true), Category("Layout"), DefaultValue(false)]
        [Description("Indicates that the Scale min/max direction is reversed.")]
        public bool Reversed
        {
            get { return (_Reversed); }

            set
            {
                if (_Reversed != value)
                {
                    _Reversed = value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        #endregion

        #region Sections

        /// <summary>
        /// Gets the collection of Sections associated with the Scale
        /// </summary>
        [Browsable(true), Category("Appearance")]
        [Description("Indicates the collection of Sections associated with the Scale.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Editor("DevComponents.Instrumentation.Design.GaugeCollectionEditor, DevComponents.Instrumentation.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=76cb4c6eb576bca5", typeof(UITypeEditor))]
        [NotifyParentProperty(true)]
        public GaugeSectionCollection Sections
        {
            get
            {
                if (_Sections == null)
                {
                    _Sections = new GaugeSectionCollection();
                    _Sections.CollectionChanged += Sections_CollectionChanged;
                }

                return (_Sections);
            }

            set
            {
                if (_Sections != null)
                    _Sections.CollectionChanged -= Sections_CollectionChanged;

                _Sections = value;

                if (_Sections != null)
                    _Sections.CollectionChanged += Sections_CollectionChanged;

                OnGaugeItemChanged(true);
            }
        }

        #endregion

        #region Width

        /// <summary>
        /// Gets or sets the Width of the Scale, specified as a percentage
        /// </summary>
        [Browsable(true)]
        [Category("Layout"), DefaultValue(.065f)]
        [Editor("DevComponents.Instrumentation.Design.WidthRangeValueEditor, DevComponents.Instrumentation.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=76cb4c6eb576bca5", typeof(UITypeEditor))]
        [Description("Indicates the Width of the Scale, specified as a percentage.")]
        public float Width
        {
            get { return (_Width); }

            set
            {
                if (value < 0)
                    throw new ArgumentException("Value can not be less than zero.");

                if (_Width != value)
                {
                    _Width = value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        #endregion

        #endregion

        #region Internal properties

        #region AbsMaxLimit

        internal double AbsMaxLimit
        {
            get { return (_MaxLimit.Equals(double.NaN) ? _MaxValue : _MaxLimit); }
        }

        #endregion

        #region AbsMinLimit

        internal double AbsMinLimit
        {
            get { return (_MinLimit.Equals(double.NaN) ? _MinValue : _MinLimit); }
        }

        #endregion

        #region Center

        internal Point Center
        {
            get { return (_Center); }
            set { _Center = value; }
        }

        #endregion

        #region HasCustomLabels

        internal bool HasCustomLabels
        {
            get { return (_CustomLabels != null); }
        }

        #endregion

        #region HasPointers

        internal bool HasPointers
        {
            get { return (_Pointers != null); }
        }

        #endregion

        #region HasRanges

        internal bool HasRanges
        {
            get { return (_Ranges != null); }
        }

        #endregion

        #region HasSections

        internal bool HasSections
        {
            get { return (_Sections != null); }
        }

        #endregion

        #region MaxPinEndOffset

        internal float MaxPinEndOffset
        {
            get
            {
                if (_MaxPin != null && _MaxPin.Visible == true)
                    return (_MaxPin.EndOffset);

                return (0);
            }
        }

        #endregion

        #region MinPinEndOffset

        internal float MinPinEndOffset
        {
            get
            {
                if (_MinPin != null && _MinPin.Visible == true)
                    return (_MinPin.EndOffset);

                return (0);
            }
        }

        #endregion

        #region NeedLabelRecalcLayout

        internal bool NeedLabelRecalcLayout
        {
            set
            {
                if (value == true)
                {
                    _Labels.NeedRecalcLayout = true;

                    if (_CustomLabels != null)
                    {
                        for (int i = 0; i < _CustomLabels.Count; i++)
                        {
                            GaugeCustomLabel label = _CustomLabels[i];

                            if (label.Visible == true)
                                label.NeedRecalcLayout = true;
                        }
                    }
                }
            }
        }

        #endregion

        #region NeedPinRecalcLayout

        internal bool NeedPinRecalcLayout
        {
            set
            {
                if (value == true)
                {
                    if (_MinPin != null)
                        _MinPin.NeedRecalcLayout = true;

                    if (_MaxPin != null)
                        _MaxPin.NeedRecalcLayout = true;
                }
            }
        }

        #endregion

        #region NeedPointerRecalcLayout

        internal bool NeedPointerRecalcLayout
        {
            set
            {
                if (value == true)
                {
                    if (_Pointers != null)
                    {
                        for (int i = 0; i < _Pointers.Count; i++)
                        {
                            GaugePointer pointer = _Pointers[i];

                            if (pointer.Visible == true)
                                pointer.NeedRecalcLayout = true;
                        }
                    }
                }
            }
        }

        #endregion

        #region NeedRangeRecalcLayout

        internal bool NeedRangeRecalcLayout
        {
            set
            {
                if (value == true)
                {
                    if (_Ranges != null)
                    {
                        for (int i = 0; i < _Ranges.Count; i++)
                        {
                            GaugeRange range = _Ranges[i];

                            if (range.Visible == true)
                                range.NeedRecalcLayout = true;
                        }
                    }
                }
            }
        }

        #endregion

        #region NeedSectionRecalcLayout

        internal bool NeedSectionRecalcLayout
        {
            set
            {
                if (value == true)
                {
                    if (_Sections != null)
                    {
                        for (int i = 0; i < _Sections.Count; i++)
                        {
                            GaugeSection section = _Sections[i];

                            if (section.Visible == true)
                                section.NeedRecalcLayout = true;
                        }
                    }
                }
            }
        }

        #endregion

        #region NeedTickMarkRecalcLayout

        internal bool NeedTickMarkRecalcLayout
        {
            set
            {
                if (value == true)
                {
                    _MajorTickMarks.NeedRecalcLayout = true;
                    _MinorTickMarks.NeedRecalcLayout = true;
                }
            }
        }

        #endregion

        #region Spread

        internal double Spread
        {
            get { return (_MaxValue - _MinValue); }
        }

        #endregion

        #region Style

        internal GaugeScaleStyle Style
        {
            get { return (_Style); }

            set
            {
                if (_Style != value)
                {
                    _Style = value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        #endregion

        #endregion

        #region HookEvents

        private void HookEvents(bool hook)
        {
            if (hook == true)
            {
                _MajorTickMarks.GaugeItemChanged += MajorTickMarks_GaugeItemChanged;
                _MinorTickMarks.GaugeItemChanged += MinorTickMarks_GaugeItemChanged;

                _MinPin.GaugeItemChanged += MinPin_GaugeItemChanged;
                _MaxPin.GaugeItemChanged += MaxPin_GaugeItemChanged;

                _Labels.GaugeItemChanged += Labels_GaugeItemChanged;
            }
            else
            {
                _MajorTickMarks.GaugeItemChanged -= MajorTickMarks_GaugeItemChanged;
                _MinorTickMarks.GaugeItemChanged -= MinorTickMarks_GaugeItemChanged;

                if (_Sections != null)
                {
                    foreach (GaugeSection section in _Sections)
                        section.GaugeItemChanged -= Section_GaugeItemChanged;

                    _Sections.CollectionChanged -= Sections_CollectionChanged;
                }

                if (_Ranges != null)
                    _Ranges.CollectionChanged -= Ranges_CollectionChanged;

                if (_Pointers != null)
                    _Pointers.CollectionChanged -= Pointers_CollectionChanged;

                _Labels.GaugeItemChanged -= Labels_GaugeItemChanged;

                if (_CustomLabels != null)
                    _CustomLabels.CollectionChanged -= CustomLabels_CollectionChanged;

                _MinPin.GaugeItemChanged -= MinPin_GaugeItemChanged;
                _MaxPin.GaugeItemChanged -= MaxPin_GaugeItemChanged;
            }
        }

        #endregion

        #region Event processing

        #region Section processing

        void Sections_CollectionChanged(object sender, EventArgs e)
        {
            foreach (GaugeSection section in _Sections)
            {
                section.Scale = this;
                section.NeedRecalcLayout = true;

                if (section.FillColor.IsEmpty)
                    section.FillColor = new GradientFillColor(Color.Lime);

                section.GaugeItemChanged -= Section_GaugeItemChanged;
                section.GaugeItemChanged += Section_GaugeItemChanged;

                section.StripCoverChanged -= StripCoverChanged;
                section.StripCoverChanged += StripCoverChanged;
            }

            OnStripCoverChanged();
        }

        void Section_GaugeItemChanged(object sender, EventArgs e)
        {
            OnGaugeItemChanged();
        }

        #endregion

        #region Range processing

        void Ranges_CollectionChanged(object sender, EventArgs e)
        {
            foreach (GaugeRange range in _Ranges)
            {
                range.Scale = this;
                range.NeedRecalcLayout = true;

                if (range.FillColor.IsEmpty)
                {
                    range.FillColor = new GradientFillColor(Color.Lime, Color.Red);
                    range.FillColor.BorderColor = Color.Black;
                    range.FillColor.BorderWidth = 1;
                }

                range.GaugeItemChanged -= Range_GaugeItemChanged;
                range.GaugeItemChanged += Range_GaugeItemChanged;

                range.StripCoverChanged -= StripCoverChanged;
                range.StripCoverChanged += StripCoverChanged;
            }

            OnStripCoverChanged();
        }

        void Range_GaugeItemChanged(object sender, EventArgs e)
        {
            OnGaugeItemChanged();
        }

        #endregion

        #region StripCoverChanged

        void StripCoverChanged(object sender, EventArgs e)
        {
            OnStripCoverChanged();
        }

        private void OnStripCoverChanged()
        {
            if (_GaugeControl != null && _GaugeControl.InitComplete == true)
            {
                UpdateValueData();
                OnGaugeItemChanged();
            }
        }

        #endregion

        #region Pointer processing

        void Pointers_CollectionChanged(object sender, EventArgs e)
        {
            foreach (GaugePointer pointer in _Pointers)
            {
                pointer.Scale = this;
                pointer.NeedRecalcLayout = true;

                if (pointer.FillColor.IsEmpty)
                {
                    pointer.FillColor = new GradientFillColor(Color.WhiteSmoke, Color.Red);
                    pointer.FillColor.BorderColor = Color.Black;
                    pointer.FillColor.BorderWidth = 1;
                }

                pointer.GaugeItemChanged -= Pointer_GaugeItemChanged;
                pointer.GaugeItemChanged += Pointer_GaugeItemChanged;
            }

            OnGaugeItemChanged();
        }

        void Pointer_GaugeItemChanged(object sender, EventArgs e)
        {
            OnGaugeItemChanged();
        }

        #endregion

        #region Label processing

        void Labels_GaugeItemChanged(object sender, EventArgs e)
        {
            OnGaugeItemChanged();
        }

        void CustomLabels_CollectionChanged(object sender, EventArgs e)
        {
            if (_CustomLabels != null)
            {
                foreach (GaugeCustomLabel label in _CustomLabels)
                {
                    label.Scale = this;
                    label.NeedRecalcLayout = true;

                    label.GaugeItemChanged -= CustomLabel_GaugeItemChanged;
                    label.GaugeItemChanged += CustomLabel_GaugeItemChanged;
                }

                OnGaugeItemChanged();
            }
        }

        #endregion

        #region TickMark processing

        void CustomLabel_GaugeItemChanged(object sender, EventArgs e)
        {
            OnGaugeItemChanged();
        }

        void MajorTickMarks_GaugeItemChanged(object sender, EventArgs e)
        {
            OnGaugeItemChanged();
        }

        void MinorTickMarks_GaugeItemChanged(object sender, EventArgs e)
        {
            OnGaugeItemChanged();
        }

        #endregion

        #region Pin processing

        void MaxPin_GaugeItemChanged(object sender, EventArgs e)
        {
            OnGaugeItemChanged(true);
        }

        void MinPin_GaugeItemChanged(object sender, EventArgs e)
        {
            OnGaugeItemChanged(true);
        }

        #endregion

        #endregion

        #region RecalcLayout

        public override void RecalcLayout()
        {
            if (NeedRecalcLayout == true)
            {
                base.RecalcLayout();

                NeedLabelRecalcLayout = true;
                NeedSectionRecalcLayout = true;
                NeedRangeRecalcLayout = true;
                NeedTickMarkRecalcLayout = true;
                NeedPointerRecalcLayout = true;
                NeedPinRecalcLayout = true;
            }
        }

        #endregion

        #region GetIntervalValue

        internal double GetIntervalValue(double interval)
        {
            return (Math.Round(_MinValue + interval, 10));
        }

        #endregion

        #region GetSectionFillColor

        internal GradientFillColor GetSectionFillColor(double interval, ColorSourceFillEntry entry)
        {
            if (_Sections != null)
            {
                if (interval.Equals(double.NaN) == true)
                    interval = 0;

                double value = GetIntervalValue(interval);

                for (int i = _Sections.Count - 1; i >= 0; i--)
                {
                    GaugeSection section = _Sections[i];

                    if (section.Visible == true)
                    {
                        GradientFillColor fillColor = GetStripFillColor(value, entry, section);

                        if (fillColor != null)
                            return (fillColor);
                    }
                }
            }

            return (null);
        }

        #endregion

        #region GetRangeFillColor

        internal GradientFillColor GetRangeFillColor(double interval, ColorSourceFillEntry entry)
        {
            if (_Ranges != null)
            {
                if (interval.Equals(double.NaN) == true)
                    interval = 0;

                double value = _MinValue + interval;

                for (int i = _Ranges.Count - 1; i >= 0; i--)
                {
                    GaugeRange range = _Ranges[i];

                    if (range.Visible == true)
                    {
                        GradientFillColor fillColor = GetStripFillColor(value, entry, range);

                        if (fillColor != null)
                            return (fillColor);
                    }
                }
            }

            return (null);
        }

        #endregion

        #region GetStripFillColor

        internal GradientFillColor GetStripFillColor(
            double value, ColorSourceFillEntry entry, GaugeStrip strip)
        {
            if (strip.ValueInRange(value) == true)
            {
                switch (entry)
                {
                    case ColorSourceFillEntry.MajorTickMark:
                        if (strip.HasMajorTickMarkFillColor)
                            return (strip.MajorTickMarkFillColor);
                        break;

                    case ColorSourceFillEntry.MinorTickMark:
                        if (strip.HasMinorTickMarkFillColor)
                            return (strip.MinorTickMarkFillColor);
                        break;

                    case ColorSourceFillEntry.Pointer:
                        if (strip.HasPointerFillColor)
                            return (strip.PointerFillColor);
                        break;

                    case ColorSourceFillEntry.Cap:
                        if (strip.HasCapFillColor)
                            return (strip.CapFillColor);
                        break;
                }
            }

            return (null);
        }

        #endregion

        #region GetSectionLabelColor

        internal Color GetSectionLabelColor(double interval)
        {
            if (_Sections != null)
            {
                double value = _MinValue + interval;

                for (int i = _Sections.Count - 1; i >= 0; i--)
                {
                    GaugeSection section = _Sections[i];

                    if (section.Visible == true)
                    {
                        Color labelColor = GetStripLabelColor(value, section);

                        if (labelColor != Color.Empty)
                            return (labelColor);
                    }
                }
            }

            return (Color.Empty);
        }

        #endregion

        #region GetRangeLabelColor

        internal Color GetRangeLabelColor(double interval)
        {
            if (_Ranges != null)
            {
                double value = _MinValue + interval;

                for (int i = _Ranges.Count - 1; i >= 0; i--)
                {
                    GaugeRange range = _Ranges[i];

                    if (range.Visible == true)
                    {
                        Color labelColor = GetStripLabelColor(value, range);

                        if (labelColor != Color.Empty)
                            return (labelColor);
                    }
                }
            }

            return (Color.Empty);
        }

        #endregion

        #region GetStripLabelColor

        internal Color GetStripLabelColor(double value, GaugeStrip strip)
        {
            if (strip.ValueInRange(value) == true)
                return (strip.LabelColor);

            return (Color.Empty);
        }

        #endregion

        #region FindItem

        internal override GaugeItem FindItem(Point pt)
        {
            GaugeItem item;

            if ((item = FindPointerItem(pt)) != null)
                return (item);

            if ((item = FindRangeItem(pt)) != null)
                return (item);

            if ((item = FindSectionItem(pt)) != null)
                return (item);

            if (_MinPin.Visible == true)
            {
                if (_MinPin.Bounds.Contains(pt))
                    return (_MinPin);
            }

            if (_MaxPin.Visible == true)
            {
                if (_MaxPin.Bounds.Contains(pt))
                    return (_MaxPin);
            }

            return (null);
        }

        #region FindPointerItem

        internal GaugeItem FindPointerItem(Point pt)
        {
            if (HasPointers == true)
            {
                for (int i = Pointers.Count - 1; i >= 0; i--)
                {
                    GaugePointer pointer = Pointers[i];

                    if (pointer.Visible == true)
                    {
                        if (pointer.UnderTickMarks == false)
                        {
                            if (pointer.FindItem(pt) != null)
                                return (pointer);
                        }
                    }
                }

                for (int i = Pointers.Count - 1; i >= 0; i--)
                {
                    GaugePointer pointer = Pointers[i];

                    if (pointer.Visible == true)
                    {
                        if (pointer.UnderTickMarks == true)
                        {
                            if (pointer.FindItem(pt) != null)
                                return (pointer);
                        }
                    }
                }
            }

            return (null);
        }

        #endregion

        #region FindRangeItem

        internal GaugeItem FindRangeItem(Point pt)
        {
            if (HasRanges == true)
            {
                for (int i = Ranges.Count - 1; i >= 0; i--)
                {
                    GaugeRange range = Ranges[i];

                    if (range.Visible == true)
                    {
                        if (range.FindItem(pt) != null)
                            return (range);
                    }
                }
            }

            return (null);
        }

        #endregion

        #region FindSectionItem

        internal GaugeItem FindSectionItem(Point pt)
        {
            if (HasSections == true)
            {
                for (int i = Sections.Count - 1; i >= 0; i--)
                {
                    GaugeSection section = Sections[i];

                    if (section.Visible == true)
                    {
                        if (section.FindItem(pt) != null)
                            return (section);
                    }
                }
            }

            return (null);
        }

        #endregion

        #endregion

        #region UpdateValueData

        internal void UpdateValueData()
        {
            bool minNan = _MinLimit.Equals(double.NaN);
            bool maxNan = _MaxLimit.Equals(double.NaN);

            if (minNan != maxNan)
            {
                throw new Exception("MinLimit and MaxLimit must both either be set to " +
                    "valid min/max values, or both reset to their default values (double.NaN).");
            }

            if (_Pointers != null)
            {
                for (int i = 0; i < _Pointers.Count; i++)
                {
                    GaugePointer pointer = _Pointers[i];

                    if (pointer.Visible == true)
                        pointer.UpdateStripData();
                }
            }
        }

        #endregion

        #region OnPaint

        public override void OnPaint(PaintEventArgs e)
        {
            if (_MaxValue > _MinValue)
            {
                RecalcLayout();

                if (GaugeControl.OnPreRenderScale(e, this) == false)
                {
                    PaintRanges(e);
                    PaintSections(e);
                    PaintPointers(e, true);
                    PaintTickMarks(e);
                    PaintPins(e);
                    PaintLabels(e);

                    GaugeControl.OnPostRenderScale(e, this);
                }
            }
        }

        #region PaintRanges

        private void PaintRanges(PaintEventArgs e)
        {
            if (GaugeControl.OnPreRenderScaleRanges(e, this) == false)
            {
                if (_Ranges != null)
                {
                    for (int i = 0; i < _Ranges.Count; i++)
                    {
                        GaugeRange range = _Ranges[i];

                        if (range.Visible == true)
                            range.OnPaint(e);
                    }
                }

                GaugeControl.OnPostRenderScaleRanges(e, this);
            }
        }

        #endregion

        #region PaintSections

        private void PaintSections(PaintEventArgs e)
        {
            if (GaugeControl.OnPreRenderScaleSections(e, this) == false)
            {
                if (_Sections != null)
                {
                    for (int i = 0; i < _Sections.Count; i++)
                    {
                        GaugeSection section = _Sections[i];

                        if (section.Visible == true)
                            section.OnPaint(e);
                    }
                }

                GaugeControl.OnPostRenderScaleSections(e, this);
            }

            PaintBorder(e);
        }

        #endregion

        #region PaintBorder

        protected virtual void PaintBorder(PaintEventArgs e)
        {
        }

        #endregion

        #region PaintTickMarks

        private void PaintTickMarks(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            if (GaugeControl.OnPreRenderScaleTickMarks(e, this) == false)
            {
                BuildTickMarkList();

                if (_TickMarkList != null)
                {
                    foreach (TickPoint tp in _TickMarkList)
                    {
                        if (tp.TickMark.Visible == true)
                            tp.TickMark.PaintTickPoint(g, tp);
                    }
                }

                GaugeControl.OnPostRenderScaleTickMarks(e, this);
            }
        }

        #region BuildTickMarkList

        private void BuildTickMarkList()
        {
            if (TickMarkListIsValid() == false)
            {
                if (_TickMarkList != null)
                    _TickMarkList.Clear();
                else
                    _TickMarkList = new List<TickPoint>();

                AddMinorTickMarks();
                AddMajorTickMarks();
                AddCustomLabelTickMarks();
            }
        }

        #endregion

        #region AddMinorTickMarks

        private void AddMinorTickMarks()
        {
            if (_MinorTickMarks.Visible == true)
                _MinorTickMarks.RecalcLayout();

            if (_MinorTickMarks.TickPoints != null)
                _TickMarkList.AddRange(_MinorTickMarks.TickPoints);
        }

        #endregion

        #region AddMajorTickMarks

        private void AddMajorTickMarks()
        {
            if (_MajorTickMarks.Visible == true)
            {
                _MajorTickMarks.RecalcLayout();

                if (_MajorTickMarks.TickPoints != null)
                {
                    if (_MajorTickMarks.Layout.Overlap == GaugeTickMarkOverlap.ReplaceNone)
                    {
                        _TickMarkList.AddRange(_MajorTickMarks.TickPoints);
                    }
                    else
                    {
                        foreach (TickPoint tp in _MajorTickMarks.TickPoints)
                        {
                            if (tp.Visible == true)
                            {
                                int n = GetTickPointIndex(tp, _MajorTickMarks.Layout.Overlap);

                                if (n >= 0)
                                    _TickMarkList[n] = tp;
                                else
                                    _TickMarkList.Add(tp);
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region AddCustomLabelTickMarks

        private void AddCustomLabelTickMarks()
        {
            if (_CustomLabels != null)
            {
                foreach (GaugeCustomLabel label in _CustomLabels)
                {
                    label.RecalcLayout();

                    if (label.Visible == true)
                    {
                        TickPoint tp = label.TickMark.TickPoint;

                        if (tp != null && label.TickMark.Visible == true)
                        {
                            int n = -1;

                            if (tp.TickMark.Layout.Overlap != GaugeTickMarkOverlap.ReplaceNone)
                                n = GetTickPointIndex(tp, tp.TickMark.Layout.Overlap);

                            if (n >= 0)
                                _TickMarkList[n] = tp;
                            else
                                _TickMarkList.Add(tp);
                        }
                    }
                }
            }
        }

        #endregion

        #region GetTickPointIndex

        private int GetTickPointIndex(TickPoint tp, GaugeTickMarkOverlap overlap)
        {
            int n = -1;
            double interval = GetIntervalValue(tp.Interval);

            for (int i = _TickMarkList.Count - 1; i >= 0; i--)
            {
                if (GetIntervalValue(_TickMarkList[i].Interval) == interval)
                {
                    if (overlap == GaugeTickMarkOverlap.ReplaceLast)
                        return (i);

                    if (n == -1)
                    {
                        n = i;
                    }
                    else
                    {
                        _TickMarkList.RemoveAt(n);
                        n = i;
                    }
                }
            }

            return (n);
        }

        #endregion

        #region TickMarkListIsValid

        private bool TickMarkListIsValid()
        {
            if (_TickMarkList == null)
                return (false);

            if ((_MajorTickMarks.Visible == true && _MajorTickMarks.NeedRecalcLayout) ||
                (_MinorTickMarks.Visible == true && _MinorTickMarks.NeedRecalcLayout))
            {
                return (false);
            }

            if (_CustomLabels != null)
            {
                for (int i = 0; i < _CustomLabels.Count; i++)
                {
                    GaugeCustomLabel label = _CustomLabels[i];

                    if (label.NeedRecalcLayout)
                        return (false);
                }
            }

            return (true);
        }

        #endregion

        #endregion

        #region PaintPointers

        internal void PaintPointers(PaintEventArgs e, bool under)
        {
            if (_MaxValue > _MinValue)
            {
                if (GaugeControl.OnPreRenderScalePointers(e, this) == false)
                {
                    if (_Pointers != null)
                    {
                        for (int i = 0; i < _Pointers.Count; i++)
                        {
                            GaugePointer pointer = _Pointers[i];

                            if (pointer.Visible == true)
                            {
                                if (pointer.UnderTickMarks == under)
                                    pointer.OnPaint(e);
                            }
                        }
                    }
                }

                GaugeControl.OnPostRenderScalePointers(e, this);
            }
        }

        #endregion

        #region PaintPins

        private void PaintPins(PaintEventArgs e)
        {
            if (_MinPin.Visible == true)
                _MinPin.OnPaint(e);

            if (_MaxPin.Visible == true)
                _MaxPin.OnPaint(e);
        }

        #endregion

        #region PaintLabels

        private void PaintLabels(PaintEventArgs e)
        {
            if (_Labels.Visible == true)
                _Labels.OnPaint(e);

            if (_CustomLabels != null)
            {
                for (int i = 0; i < _CustomLabels.Count; i++)
                {
                    GaugeCustomLabel label = _CustomLabels[i];

                    if (label.Visible == true)
                        label.OnPaint(e);
                }
            }
        }

        #endregion

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
            GaugeScale copy = new GaugeScale();

            CopyToItem(copy);

            return (copy);
        }

        #endregion

        #region CopyToItem

        public override void CopyToItem(GaugeItem copy)
        {
            GaugeScale c = copy as GaugeScale;

            if (c != null)
            {
                base.CopyToItem(c);

                c.BorderColor = _BorderColor;
                c.BorderWidth = _BorderWidth;

                c.MaxLimit = _MaxLimit;
                c.MinLimit = _MinLimit;
                c.MaxValue = _MaxValue;
                c.MinValue = _MinValue;

                c.Reversed = _Reversed;
                c.Width = _Width;

                _Labels.CopyToItem(c.Labels);
                c.Labels.Scale = c;

                _MajorTickMarks.CopyToItem(c.MajorTickMarks);
                c.MajorTickMarks.Scale = c;

                _MinorTickMarks.CopyToItem(c.MinorTickMarks);
                c.MinorTickMarks.Scale = c;

                _MaxPin.CopyToItem(c.MaxPin);
                c.MaxPin.Scale = c;

                _MinPin.CopyToItem(c.MinPin);
                c.MinPin.Scale = c;

                if (_CustomLabels != null)
                {
                    c.CustomLabels = new GaugeCustomLabelCollection();
                    c.CustomLabels.CollectionChanged += CustomLabels_CollectionChanged;

                    _CustomLabels.CopyToItem(c.CustomLabels);
                }

                if (_Pointers != null)
                {
                    c.Pointers = new GaugePointerCollection();
                    c.Pointers.CollectionChanged += Pointers_CollectionChanged;

                    _Pointers.CopyToItem(c.Pointers);
                }

                if (_Ranges != null)
                {
                    c.Ranges = new GaugeRangeCollection();
                    c.Ranges.CollectionChanged += Ranges_CollectionChanged;

                    _Ranges.CopyToItem(c.Ranges);
                }

                if (_Sections != null)
                {
                    c.Sections = new GaugeSectionCollection();
                    c.Sections.CollectionChanged += Sections_CollectionChanged;

                    _Sections.CopyToItem(c.Sections);
                }
            }
        }

        #endregion
    }

    #region Enums

    public enum GaugeScaleStyle
    {
        Circular,
        Linear
    }

    #endregion

    #region GaugeScaleConvertor

    public class GaugeScaleConvertor : ExpandableObjectConverter
    {
        public override object ConvertTo(
            ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                GaugeScale scale = value as GaugeScale;

                if (scale != null)
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
