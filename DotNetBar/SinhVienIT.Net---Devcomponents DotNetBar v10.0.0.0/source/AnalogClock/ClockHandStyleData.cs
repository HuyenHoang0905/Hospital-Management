using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Controls
{
    /// <summary>
    /// Data storage class for clock hand visual style.
    /// </summary>
    [Description("Clock Hand Style"),
    TypeConverterAttribute(typeof(ExpandableObjectConverter))]
    public class ClockHandStyleData : INotifyPropertyChanged
    {
        private bool _DrawOverCap;
        /// <summary>
        /// Gets or sets a value indicating whether the hand is drawn over the cap.
        /// </summary>
        [DefaultValue(false),
        Category("Appearance"),
        Description("Indicates whether the hand is drawn over the cap.")]
        public bool DrawOverCap
        {
            get { return _DrawOverCap; }
            set { _DrawOverCap = value; }
        }

        private void ColorPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e);
        }

        private ColorData _HandColor;
        /// <summary>
        /// Gets or sets the hand color data for this hand.
        /// </summary>
        [Category("Appearance"),
        Description("The hand color data for this hand.")]
        public ColorData HandColor
        {
            get { return _HandColor; }
            set
            {
                if (value != _HandColor)
                {
                    if (_HandColor != null) _HandColor.PropertyChanged -= ColorPropertyChanged;
                    _HandColor = value;
                    if (_HandColor != null) _HandColor.PropertyChanged += ColorPropertyChanged;
                    OnPropertyChanged(new PropertyChangedEventArgs("HandColor"));
                }
            }
        }
        /// <summary>
        /// Resets the property to default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetHandColor()
        {
            HandColor = new ColorData(eBrushTypes.Solid, Color.FromArgb(109, 127, 138), Color.FromArgb(109, 127, 138), Color.FromArgb(128, 109, 127, 138), 0.01f);
        }

        private eHandStyles _HandStyle;
        /// <summary>
        /// Gets or sets the hand style for this clock hand. Default value is Style1.
        /// </summary>
        [DefaultValue(eHandStyles.Style1),
        Category("Appearance"),
        Description("The hand style for this clock hand.")]
        public eHandStyles HandStyle
        {
            get { return _HandStyle; }
            set
            {
                if (value != _HandStyle)
                {
                    _HandStyle = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("HandStyle"));
                }
            }
        }

        private float _Length;
        /// <summary>
        /// Gets or sets the length of this clock hand as a percentage value ranging from 0.0 to 1.0, with 1.0 being half the width/height of the bounding rectangle. Default value is 1.0.
        /// </summary>
        [DefaultValue(1.0f),
        Category("Appearance"),
        Description("The length of this clock hand as a percentage value ranging from 0.0 to 1.0, with 1.0 being half the width/height of the bounding rectangle.")]
        public float Length
        {
            get { return _Length; }
            set
            {
                if (value != _Length)
                {
                    _Length = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Length"));
                }
            }
        }

        private float _Width;
        /// <summary>
        /// Gets or sets the width of this clock hand as a percentage value ranging from 0.0 to 1.0, with 1.0 being half the width/height of the bounding rectangle. Default value is 0.1.
        /// </summary>
        [DefaultValue(0.1f),
        Category("Appearance"),
        Description("The width of this clock hand as a percentage value ranging from 0.0 to 1.0, with 1.0 being half the width/height of the bounding rectangle.")]
        public float Width
        {
            get { return _Width; }
            set
            {
                if (value != _Width)
                {
                    _Width = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Width"));
                }
            }
        }

        /// <summary>
        /// Occurs when property value has changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Initializes a new instance of the ClockHand class.
        /// </summary>
        public ClockHandStyleData()
        {
            _DrawOverCap = false;
            _HandColor = new ColorData(eBrushTypes.Solid, Color.FromArgb(109, 127, 138), Color.FromArgb(109, 127, 138), Color.FromArgb(128, 109, 127, 138), 0.01f);
            _HandStyle = eHandStyles.Style1;
            _Length = 1.0f;
            _Width = 0.1f;
        }

        /// <summary>
        /// Initializes a new instance of the ClockHand class.
        /// </summary>
        /// <param name="handStyle">The hand style for this item.</param>
        /// <param name="length">The length of this clock hand as a percentage value ranging from 0.0 to 1.0, with 1.0 being half the width/height of the bounding rectangle</param>
        /// <param name="width">The width of this clock hand as a percentage value ranging from 0.0 to 1.0, with 1.0 being half the width/height of the bounding rectangle.</param>
        public ClockHandStyleData(eHandStyles handStyle, float length, float width)
        {
            _DrawOverCap = false;
            _HandColor = new ColorData(eBrushTypes.Solid, Color.FromArgb(109, 127, 138), Color.FromArgb(109, 127, 138), Color.FromArgb(128, 109, 127, 138), 0.01f);
            _HandStyle = handStyle;
            _Length = length;
            _Width = width;
        }

        /// <summary>
        /// Indicates whether the specified point is contained within the bounds of this hand.
        /// </summary>
        /// <param name="boundingRect">The bounding rectangle of the parent clock control.</param>
        /// <param name="angle">The clockwise angle for this clock hand in degrees from the 12 o'clock position.</param>
        /// <param name="pt">A Point that represents the point to test.</param>
        /// <returns></returns>
        public virtual bool ContainsPoint(RectangleF boundingRect, float angle, Point pt)
        {
            GraphicsPath path;
            bool ret;

            path = GenerateHandPath(boundingRect, angle);
            
            ret = path.IsVisible(pt);
            path.Dispose();
            return ret;
        }

        /// <summary>
        /// Generates a scaled and rotated graphics path based on the given style, rectangle and angle.
        /// </summary>
        /// <param name="boundingRect">The bounding rectangle of the parent clock control.</param>
        /// <param name="angle">The clockwise angle for this clock hand in degrees from the 12 o'clock position.</param>
        /// <returns></returns>
        public GraphicsPath GenerateHandPath(RectangleF boundingRect, float angle)
        {
            GraphicsPath path = new GraphicsPath();
            RectangleF rect;
            Matrix matrix;
            float scaleFactor;
            PointF[] pts;

            switch (_HandStyle)
            {
                case eHandStyles.Style1:
                    pts = new PointF[4];
                    pts[0].X = -0.5f;
                    pts[0].Y = 0.0f;

                    pts[1].X = -0.5f;
                    pts[1].Y = -1.0f;

                    pts[2].X = 0.5f;
                    pts[2].Y = -1.0f;

                    pts[3].X = 0.5f;
                    pts[3].Y = 0.0f;
                    path.AddPolygon(pts);
                    break;
                case eHandStyles.Style2:
                    pts = new PointF[4];
                    pts[0].X = -0.4f;
                    pts[0].Y = 0.25f;

                    pts[1].X = -0.4f;
                    pts[1].Y = -1.0f;

                    pts[2].X = 0.4f;
                    pts[2].Y = -1.0f;

                    pts[3].X = 0.4f;
                    pts[3].Y = 0.25f;
                    path.AddPolygon(pts);
                    break;
                case eHandStyles.Style3:
                    pts = new PointF[4];
                    pts[0].X = -0.5f;
                    pts[0].Y = 0.0f;

                    pts[1].X = -0.0125f;
                    pts[1].Y = -1.0f;
                    
                    pts[2].X = 0.0125f;
                    pts[2].Y = -1.0f;

                    pts[3].X = 0.5f;
                    pts[3].Y = 0.0f;

                    path.AddPolygon(pts);
                    break;
                case eHandStyles.Style4:
                    path.FillMode = FillMode.Winding;
                    pts = new PointF[4];
                    pts[0].X = -0.5f;
                    pts[0].Y = -0.05f;

                    pts[1].X = -0.5f;
                    pts[1].Y = -1.0f;

                    pts[2].X = 0.5f;
                    pts[2].Y = -1.0f;

                    pts[3].X = 0.5f;
                    pts[3].Y = -0.05f;
                    path.AddPolygon(pts);

                    rect = new RectangleF(-5.0f, -0.06f, 10.0f, 0.12f);
                    path.AddEllipse(rect);
                    break;
            }
            scaleFactor = Math.Min(boundingRect.Width, boundingRect.Height) / 2.0f;
            matrix = new Matrix();
            matrix.Translate(boundingRect.X + boundingRect.Width * 0.5f, boundingRect.Y + boundingRect.Width * 0.5f);
            matrix.Rotate(angle);
            matrix.Scale(scaleFactor * Width, scaleFactor * Length);
            path.Transform(matrix);
            matrix.Dispose();
            return path;
        }

        /// <summary>
        /// Raises the PropertyChanged event.
        /// </summary>
        /// <param name="e">Event arguments</param>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }
    }

    /// <summary>
    /// Enumeration containing the available hand styles.
    /// </summary>
    public enum eHandStyles
    {
        /// <summary>
        /// Style 1.
        /// </summary>
        Style1,

        /// <summary>
        /// Style 2.
        /// </summary>
        Style2,

        /// <summary>
        /// Style 3.
        /// </summary>
        Style3,

        /// <summary>
        /// Style 4.
        /// </summary>
        Style4,
    }

}
