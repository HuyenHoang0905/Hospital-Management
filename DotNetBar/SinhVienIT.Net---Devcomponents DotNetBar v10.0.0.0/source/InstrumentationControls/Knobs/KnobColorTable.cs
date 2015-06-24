using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using DevComponents.Instrumentation.Primitives;

namespace DevComponents.Instrumentation
{
    [TypeConverter(typeof(KnobColorTableConvertor))]
    public class KnobColorTable
    {
        public static readonly KnobColorTable Empty = new KnobColorTable();

        #region Events

        /// <summary>
        /// Event raised when ColorTable has changed
        /// </summary>
        [Description("Event raised when ColorTable has changed.")]
        public event EventHandler<EventArgs> ColorTableChanged;

        #endregion

        #region Private variables

        private Color _MajorTickColor = Color.Empty;
        private Color _MinorTickColor = Color.Empty;
        private Color _ZoneIndicatorColor = Color.Empty;
        private Color _KnobIndicatorPointerColor = Color.Empty;

        private LinearGradientColorTable _KnobFaceColor = new LinearGradientColorTable();
        private LinearGradientColorTable _KnobIndicatorColor = new LinearGradientColorTable();
        private LinearGradientColorTable _MinZoneIndicatorColor = new LinearGradientColorTable();
        private LinearGradientColorTable _MaxZoneIndicatorColor = new LinearGradientColorTable();
        private LinearGradientColorTable _MidZoneIndicatorColor = new LinearGradientColorTable();

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public KnobColorTable()
        {
            _KnobFaceColor = new LinearGradientColorTable();
            _KnobFaceColor.ColorTableChanged += KnobColorTableChanged;

            _KnobIndicatorColor = new LinearGradientColorTable();
            _KnobIndicatorColor.ColorTableChanged += KnobColorTableChanged;

            _MinZoneIndicatorColor = new LinearGradientColorTable();
            _MinZoneIndicatorColor.ColorTableChanged += KnobColorTableChanged;

            _MaxZoneIndicatorColor = new LinearGradientColorTable();
            _MaxZoneIndicatorColor.ColorTableChanged += KnobColorTableChanged;

            _MidZoneIndicatorColor = new LinearGradientColorTable();
            _MidZoneIndicatorColor.ColorTableChanged += KnobColorTableChanged;
        }

        #region Public properties

        #region MajorTickColor

        /// <summary>
        /// Gets or sets the color of the Major Tick marks
        /// </summary>
        [Browsable(true)]
        [Description("Indicates the color of the Major Tick marks")]
        public Color MajorTickColor
        {
            get { return (_MajorTickColor); }

            set
            {
                if (_MajorTickColor != value)
                {
                    _MajorTickColor = value;

                    OnColorTableChange();
                }
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeMajorTickColor()
        {
            return (_MajorTickColor.IsEmpty == false);
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetMajorTickColor()
        {
            MajorTickColor = Color.Empty;
        }

        #endregion

        #region MinorTickColor

        /// <summary>
        /// Gets or sets the color of the Minor Tick marks
        /// </summary>
        [Browsable(true)]
        [Description("Indicates the color of the Minor Tick marks")]
        public Color MinorTickColor
        {
            get { return (_MinorTickColor); }

            set
            {
                if (_MinorTickColor != value)
                {
                    _MinorTickColor = value;

                    OnColorTableChange();
                }
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeMinorTickColor()
        {
            return (_MinorTickColor.IsEmpty == false);
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetMinorTickColor()
        {
            MinorTickColor = Color.Empty;
        }

        #endregion

        #region KnobIndicatorPointerColor

        /// <summary>
        /// Gets or sets the color of the KnobIndicatorPointer
        /// </summary>
        [Browsable(true)]
        [Description("Indicates the color of the KnobIndicatorPointer")]
        public Color KnobIndicatorPointerColor
        {
            get { return (_KnobIndicatorPointerColor); }

            set
            {
                if (_KnobIndicatorPointerColor != value)
                {
                    _KnobIndicatorPointerColor = value;

                    OnColorTableChange();
                }
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeKnobIndicatorPointerColor()
        {
            return (_KnobIndicatorPointerColor.IsEmpty == false);
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetKnobIndicatorPointerColor()
        {
            KnobIndicatorPointerColor = Color.Empty;
        }
        #endregion

        #region ZoneIndicatorColor

        /// <summary>
        /// Gets or sets the color of the ZoneIndicator
        /// </summary>
        [Browsable(true)]
        [Description("Indicates the color of the ZoneIndicator")]
        public Color ZoneIndicatorColor
        {
            get { return (_ZoneIndicatorColor); }

            set
            {
                if (_ZoneIndicatorColor != value)
                {
                    _ZoneIndicatorColor = value;

                    OnColorTableChange();
                }
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeZoneIndicatorColor()
        {
            return (_ZoneIndicatorColor.IsEmpty == false);
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetZoneIndicatorColor()
        {
            ZoneIndicatorColor = Color.Empty;
        }

        #endregion

        #region KnobFaceColor

        /// <summary>
        /// Gets or sets the color of the KnobFace
        /// </summary>
        [Browsable(true)]
        [Description("Indicates the color of the KnobFace")]
        public LinearGradientColorTable KnobFaceColor
        {
            get { return (_KnobFaceColor); }

            set
            {
                if (_KnobFaceColor != value)
                {
                    if (_KnobFaceColor != null)
                        _KnobFaceColor.ColorTableChanged -= KnobColorTableChanged;

                    _KnobFaceColor = value;

                    if (_KnobFaceColor != null)
                        _KnobFaceColor.ColorTableChanged += KnobColorTableChanged;

                    OnColorTableChange();
                }
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeKnobFaceColor()
        {
            return (_KnobFaceColor.IsEmpty == false);
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetKnobFaceColor()
        {
            KnobFaceColor = LinearGradientColorTable.Empty;
        }

        #endregion

        #region KnobIndicatorColor

        /// <summary>
        /// Gets or sets the color of the KnobIndicator
        /// </summary>
        [Browsable(true)]
        [Description("Indicates the color of the KnobIndicator")]
        public LinearGradientColorTable KnobIndicatorColor
        {
            get { return (_KnobIndicatorColor); }

            set
            {
                if (_KnobIndicatorColor != value)
                {
                    if (_KnobIndicatorColor != null)
                        _KnobIndicatorColor.ColorTableChanged -= KnobColorTableChanged;

                    _KnobIndicatorColor = value;

                    if (_KnobIndicatorColor != null)
                        _KnobIndicatorColor.ColorTableChanged += KnobColorTableChanged;

                    OnColorTableChange();
                }
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeKnobIndicatorColor()
        {
            return (_KnobIndicatorColor.IsEmpty == false);
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetKnobIndicatorColor()
        {
            KnobIndicatorColor = LinearGradientColorTable.Empty;
        }

        #endregion

        #region MinZoneIndicatorColor

        /// <summary>
        /// Gets or sets the color of the MinZoneIndicator
        /// </summary>
        [Browsable(true)]
        [Description("Indicates the color of the MinZoneIndicator")]
        public LinearGradientColorTable MinZoneIndicatorColor
        {
            get { return (_MinZoneIndicatorColor); }

            set
            {
                if (_MinZoneIndicatorColor != value)
                {
                    if (_MinZoneIndicatorColor != null)
                        _MinZoneIndicatorColor.ColorTableChanged -= KnobColorTableChanged;

                    _MinZoneIndicatorColor = value;

                    if (_MinZoneIndicatorColor != null)
                        _MinZoneIndicatorColor.ColorTableChanged += KnobColorTableChanged;

                    OnColorTableChange();
                }
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeMinZoneIndicatorColor()
        {
            return (_MinZoneIndicatorColor.IsEmpty == false);
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetMinZoneIndicatorColor()
        {
            MinZoneIndicatorColor = LinearGradientColorTable.Empty;
        }

        #endregion

        #region MaxZoneIndicatorColor

        /// <summary>
        /// Gets or sets the color of the MaxZoneIndicator
        /// </summary>
        [Browsable(true)]
        [Description("Indicates the color of the MaxZoneIndicator")]
        public LinearGradientColorTable MaxZoneIndicatorColor
        {
            get { return (_MaxZoneIndicatorColor); }

            set
            {
                if (_MaxZoneIndicatorColor != value)
                {
                    if (_MaxZoneIndicatorColor != null)
                        _MaxZoneIndicatorColor.ColorTableChanged -= KnobColorTableChanged;

                    _MaxZoneIndicatorColor = value;

                    if (_MaxZoneIndicatorColor != null)
                        _MaxZoneIndicatorColor.ColorTableChanged += KnobColorTableChanged;
                    
                    OnColorTableChange();
                }
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeMaxZoneIndicatorColor()
        {
            return (_MaxZoneIndicatorColor.IsEmpty == false);
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetMaxZoneIndicatorColor()
        {
            MaxZoneIndicatorColor = LinearGradientColorTable.Empty;
        }

        #endregion

        #region MidZoneIndicatorColor

        /// <summary>
        /// Gets or sets the color of the MidZoneIndicator
        /// </summary>
        [Browsable(true)]
        [Description("Indicates the color of the MidZoneIndicator")]
        public LinearGradientColorTable MidZoneIndicatorColor
        {
            get { return (_MidZoneIndicatorColor); }

            set
            {
                if (_MidZoneIndicatorColor != value)
                {
                    if (_MidZoneIndicatorColor != null)
                        _MidZoneIndicatorColor.ColorTableChanged -= KnobColorTableChanged;

                    _MidZoneIndicatorColor = value;

                    if (_MidZoneIndicatorColor != null)
                        _MidZoneIndicatorColor.ColorTableChanged += KnobColorTableChanged;

                    OnColorTableChange();
                }
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeMidZoneIndicatorColor()
        {
            return (_MidZoneIndicatorColor.IsEmpty == false);
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetMidZoneIndicatorColor()
        {
            MidZoneIndicatorColor = LinearGradientColorTable.Empty;
        }

        #endregion

        #region IsEmpty

        [Browsable(false)]
        public bool IsEmpty
        {
            get
            {
                return (_MajorTickColor.IsEmpty && _MinorTickColor.IsEmpty && _KnobIndicatorPointerColor.IsEmpty &&
                        _KnobFaceColor.IsEmpty && _ZoneIndicatorColor.IsEmpty && _MinZoneIndicatorColor.IsEmpty &&
                        _MaxZoneIndicatorColor.IsEmpty && _MidZoneIndicatorColor.IsEmpty && _KnobIndicatorColor.IsEmpty);
            }
        }

        #endregion

        #endregion

        #region KnobColorTableChanged

        /// <summary>
        /// KnobColorTableChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void KnobColorTableChanged(object sender, EventArgs e)
        {
            OnColorTableChange();
        }

        #endregion

        #region OnColorTableChange

        /// <summary>
        /// OnColorTableChange
        /// </summary>
        private void OnColorTableChange()
        {
            if (ColorTableChanged != null)
                ColorTableChanged(this, EventArgs.Empty);
        }

        #endregion
    }

    #region KnobColorTableConvertor

    /// <summary>
    /// KnobColorTableConvertor
    /// </summary>
    public class KnobColorTableConvertor : ExpandableObjectConverter
    {
        public override object ConvertTo(
            ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                KnobColorTable kct = value as KnobColorTable;

                if (kct != null)
                {
                    ColorConverter cvt = new ColorConverter();

                    string s;

                    if ((s = MyColorConverter(kct.KnobFaceColor, cvt)) != null)
                        return (s);

                    if ((s = MyColorConverter(kct.KnobIndicatorColor, cvt)) != null)
                        return (s);

                    if ((s = MyColorConverter(kct.MinZoneIndicatorColor, cvt)) != null)
                        return (s);

                    if ((s = MyColorConverter(kct.MidZoneIndicatorColor, cvt)) != null)
                        return (s);

                    if ((s = MyColorConverter(kct.MaxZoneIndicatorColor, cvt)) != null)
                        return (s);

                    if (kct.MinorTickColor.IsEmpty == false)
                        return (cvt.ConvertToString(kct.MinorTickColor));

                    if (kct.MajorTickColor.IsEmpty == false)
                        return (cvt.ConvertToString(kct.MajorTickColor));

                    if (kct.KnobIndicatorPointerColor.IsEmpty == false)
                        return (cvt.ConvertToString(kct.KnobIndicatorPointerColor));

                    if (kct.ZoneIndicatorColor.IsEmpty == false)
                        return (cvt.ConvertToString(kct.ZoneIndicatorColor));

                    return (String.Empty);
                }
            }

            return (base.ConvertTo(context, culture, value, destinationType));
        }

        #region MyColorConverter

        /// <summary>
        /// MyColorConverter
        /// </summary>
        /// <param name="ct">ColorTable</param>
        /// <param name="cvt">ColorConverter</param>
        /// <returns>string or null</returns>
        private string MyColorConverter(LinearGradientColorTable ct, ColorConverter cvt)
        {
            if (ct.Start.IsEmpty == false)
                return (cvt.ConvertToString(ct.Start));

            if (ct.End.IsEmpty == false)
                return (cvt.ConvertToString(ct.Start));

            return (null);
        }

        #endregion
    }

    #endregion
}