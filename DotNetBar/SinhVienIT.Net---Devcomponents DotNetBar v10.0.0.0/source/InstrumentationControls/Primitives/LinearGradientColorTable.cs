using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;

namespace DevComponents.Instrumentation.Primitives
{
    /// <summary>
    /// Represents the color table of linear gradient.
    /// </summary>
    [TypeConverter(typeof(LinearGradientColorTableConvertor))]
    public class LinearGradientColorTable
    {
        public static readonly LinearGradientColorTable Empty = new LinearGradientColorTable();

        #region Events

        public event EventHandler<EventArgs> ColorTableChanged;

        #endregion

        #region Private variables

        private Color _Start = Color.Empty;
        private Color _End = Color.Empty;
        private int _GradientAngle = 90;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates new instance of the object.
        /// </summary>
        public LinearGradientColorTable() { }
        /// <summary>
        /// Creates new instance of the object.
        /// </summary>
        /// <param name="start">Start color.</param>
        public LinearGradientColorTable(Color start)
        {
            this.Start = start;
        }

        /// <summary>
        /// Creates new instance of the object.
        /// </summary>
        /// <param name="start">Start color.</param>
        /// <param name="end">End color.</param>
        public LinearGradientColorTable(Color start, Color end)
        {
            this.Start = start;
            this.End = end;
        }

        /// <summary>
        /// Creates new instance of the object.
        /// </summary>
        /// <param name="start">Start color in hexadecimal representation like FFFFFF.</param>
        /// <param name="end">End color in hexadecimal representation like FFFFFF.</param>
        public LinearGradientColorTable(string start, string end)
        {
            this.Start = ColorFactory.GetColor(start);
            this.End = ColorFactory.GetColor(end);
        }

        /// <summary>
        /// Creates new instance of the object.
        /// </summary>
        /// <param name="start">Start color in 32-bit RGB representation.</param>
        /// <param name="end">End color in 32-bit RGB representation.</param>
        public LinearGradientColorTable(int start, int end)
        {
            this.Start = ColorFactory.GetColor(start);
            this.End = ColorFactory.GetColor(end);
        }

        /// <summary>
        /// Creates new instance of the object.
        /// </summary>
        /// <param name="start">Start color in 32-bit RGB representation.</param>
        /// <param name="end">End color in 32-bit RGB representation.</param>
        /// <param name="gradientAngle">Gradient angle.</param>
        public LinearGradientColorTable(int start, int end, int gradientAngle)
        {
            this.Start = ColorFactory.GetColor(start);
            this.End = ColorFactory.GetColor(end);
            this.GradientAngle = gradientAngle;
        }

        /// <summary>
        /// Creates new instance of the object.
        /// </summary>
        /// <param name="start">Start color.</param>
        /// <param name="end">End color.</param>
        /// <param name="gradientAngle">Gradient angle.</param>
        public LinearGradientColorTable(Color start, Color end, int gradientAngle)
        {
            this.Start = start;
            this.End = end;
            this.GradientAngle = gradientAngle;
        }

        #endregion

        #region Public properties

        #region Start

        /// <summary>
        /// Gets or sets the start color.
        /// </summary>
        [Browsable(true)]
        [NotifyParentProperty(true)]
        [Description("Indicates the Starting Gradient Color.")]
        public Color Start
        {
            get { return (_Start); }

            set
            {
                if (_Start != value)
                {
                    _Start = value;

                    OnColorTableChanged();
                }
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        internal virtual bool ShouldSerializeStart()
        {
            return (_Start.IsEmpty == false);
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        internal virtual void ResetStart()
        {
            Start = Color.Empty;
        }

        #endregion

        #region End

        /// <summary>
        /// Gets or sets the end color.
        /// </summary>
        [Browsable(true)]
        [NotifyParentProperty(true)]
        [Description("Indicates the Ending Gradient Color.")]
        public Color End
        {
            get { return (_End); }

            set
            {
                if (_End != value)
                {
                    _End = value;

                    OnColorTableChanged();
                }
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        internal virtual bool ShouldSerializeEnd()
        {
            return (_End.IsEmpty == false);
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        internal virtual void ResetEnd()
        {
            End = Color.Empty;
        }
        #endregion

        #region GradientAngle

        /// <summary>
        /// Gets or sets the gradient angle. Default value is 90.
        /// </summary>
        [Browsable(true), DefaultValue(90)]
        [NotifyParentProperty(true)]
        [Description("Indicates the Gradient Angle.  Default is 90")]
        public int GradientAngle
        {
            get { return (_GradientAngle); }

            set
            {
                if (_GradientAngle != value)
                {
                    _GradientAngle = value;

                    OnColorTableChanged();
                }
            }
        }

        #endregion

        #region IsEmpty

        /// <summary>
        /// Gets whether both colors assigned are empty.
        /// </summary>
        [Browsable(false)]
        public virtual bool IsEmpty
        {
            get { return (Start.IsEmpty && End.IsEmpty && GradientAngle == 90); }
        }

        #endregion

        #endregion

        #region OnColorTableChanged

        protected void OnColorTableChanged()
        {
            if (ColorTableChanged != null)
                ColorTableChanged(this, EventArgs.Empty);
        }

        #endregion

        #region GetBrush

        public Brush GetBrush(Rectangle r)
        {
            return (GetBrush(r, _GradientAngle));
        }

        public Brush GetBrush(Rectangle r, int angle)
        {
            if (_End.IsEmpty == true)
                return (new SolidBrush(_Start));

            LinearGradientBrush lbr =
                new LinearGradientBrush(r, _Start, _End, angle);

            return (lbr);
        }

        #endregion
    }

    #region LinearGradientColorTableConvertor

    public class LinearGradientColorTableConvertor : ExpandableObjectConverter
    {
        public override object ConvertTo(
            ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                LinearGradientColorTable lct = value as LinearGradientColorTable;

                if (lct != null)
                {
                    ColorConverter cvt = new ColorConverter();

                    if (lct.Start != Color.Empty)
                        return (cvt.ConvertToString(lct.Start));

                    if (lct.End != Color.Empty)
                        return (cvt.ConvertToString(lct.End));

                    if (lct.GradientAngle != 90)
                        return (lct.GradientAngle.ToString());

                    return (String.Empty);
                }
            }

            return (base.ConvertTo(context, culture, value, destinationType));
        }
    }

    #endregion

}

