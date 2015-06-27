using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DevComponents.DotNetBar.Controls
{
    /// <summary>
    /// Data storage and utility class for defining gradient colors.
    /// </summary>
    [Description("Color Data Class"),
    TypeConverterAttribute(typeof(ExpandableObjectConverter))]
    public class ColorData : INotifyPropertyChanged
    {
        private Color _BorderColor;
        /// <summary>
        /// Gets or sets the border color for this item. Default value is white.
        /// </summary>
        [DefaultValue(typeof(Color), "255, 255, 255"),
        Category("Appearance"),
        Description("The border color for this item.")]
        public Color BorderColor
        {
            get { return _BorderColor; }
            set
            {
                if (value != _BorderColor)
                {
                    _BorderColor = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("BorderColor"));
                }
            }        
        }

        private float _BorderWidth;
        /// <summary>
        /// Gets or sets the border width for this item. Default value is 0.
        /// </summary>
        [DefaultValue(0.0f),
        Category("Appearance"),
        Description("The border width for this item.")]
        public float BorderWidth
        {
            get { return _BorderWidth; }
            set
            {
                if (value != _BorderWidth)
                {
                    _BorderWidth = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("BorderWidth"));
                }
            }        
        }

        private float _BrushAngle;
        /// <summary>
        /// Gets or sets the brush angle for this item. Only applies to Linear and Reflected brush types. Default value is 0.
        /// </summary>
        [DefaultValue(0.0f),
        Category("Appearance"),
        Description("The brush angle for this item. Only applies to Linear and Reflected brush types.")]
        public float BrushAngle
        {
            get { return _BrushAngle; }
            set
            {
                if (value != _BrushAngle)
                {
                    _BrushAngle = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("BrushAngle"));
                }
            }        
        }

        private float _BrushSBSFocus;
        /// <summary>
        /// Gets or sets the brush SigmaBellShape focus for this item. Only applies to Reflected brush types. Default value is 0.5.
        /// </summary>
        [DefaultValue(0.5f),
        Category("Appearance"),
        Description("The brush SigmaBellShape focus for this item. Only applies to Linear and Reflected brush types.")]
        public float BrushSBSFocus
        {
            get { return _BrushSBSFocus; }
            set
            {
                if (value != _BrushSBSFocus)
                {
                    _BrushSBSFocus = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("BrushSBSFocus"));
                }
            }        
        }

        private float _BrushSBSScale;
        /// <summary>
        /// Gets or sets the brush SigmaBellShape scale for this item. Only applies to Reflected brush types. Default value is 0.5.
        /// </summary>
        [DefaultValue(0.5f),
        Category("Appearance"),
        Description("The brush SigmaBellShape scale for this item. Only applies to Linear and Reflected brush types.")]
        public float BrushSBSScale
        {
            get { return _BrushSBSScale; }
            set
            {
                if (value != _BrushSBSScale)
                {
                    _BrushSBSScale = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("BrushSBSScale"));
                }
            }        
        }

        private eBrushTypes _BrushType;
        /// <summary>
        /// Gets or sets the brush type for this item. Default value is Solid.
        /// </summary>
        [DefaultValue(eBrushTypes.Solid),
        Category("Appearance"),
        Description("The brush type for this item.")]
        public eBrushTypes BrushType
        {
            get { return _BrushType; }
            set
            {
                if (value != _BrushType)
                {
                    _BrushType = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("BrushType"));
                }
            }
        }

        private Color _Color1;
        /// <summary>
        /// Gets or sets the first color for this item. Default value is white.
        /// </summary>
        [DefaultValue(typeof(Color), "255, 255, 255"),
        Category("Appearance"),
        Description("The first color for this item.")]
        public Color Color1
        {
            get { return _Color1; }
            set
            {
                if (value != _Color1)
                {
                    _Color1 = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Color1"));
                }
            }
        }

        private Color _Color2;
        /// <summary>
        /// Gets or sets the second color for this item. Default value is white.
        /// </summary>
        [DefaultValue(typeof(Color), "255, 255, 255"),
        Category("Appearance"),
        Description("The second color for this item.")]
        public Color Color2
        {
            get { return _Color2; }
            set 
            {
                if (value != _Color2)
                {
                    _Color2 = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Color2"));
                }
            }
        }

        /// <summary>
        /// Occurs when property value has changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Creates new instance of the object. 
        /// </summary>
        public ColorData()
        {
            LoadData(eBrushTypes.Solid, Color.White, Color.White, Color.White, 0.0f, 0.0f, 0.5f, 1.0f);
        }

        /// <summary>
        /// Creates new instance of the object. 
        /// </summary>
        /// <param name="color1">The first color for this entry.</param>
        /// <param name="color2">The second color for this entry.</param>
        public ColorData(eBrushTypes brushType, Color color1, Color color2) 
        {
            LoadData(brushType, color1, color2, Color.White, 0.0f, 0.0f, 0.5f, 1.0f);
        }

        /// <summary>
        /// Creates new instance of the object. 
        /// </summary>
        /// <param name="color1">The first color for this entry.</param>
        /// <param name="color2">The second color for this entry.</param>
        /// <param name="borderColor">The border color for this entry.</param>
        /// <param name="borderWidth">The border width for this entry.</param>
        public ColorData(eBrushTypes brushType, Color color1, Color color2, Color borderColor, float borderWidth)
        {
            LoadData(brushType, color1, color2, borderColor, borderWidth, 0.0f, 0.5f, 1.0f);
        }

        /// <summary>
        /// Creates new instance of the object. 
        /// </summary>
        /// <param name="color1">The first color for this entry.</param>
        /// <param name="color2">The second color for this entry.</param>
        /// <param name="borderColor">The border color for this entry.</param>
        /// <param name="borderWidth">The border width for this entry.</param>
        /// <param name="brushAngle">The gradient angle.</param>
        public ColorData(eBrushTypes brushType, Color color1, Color color2, Color borderColor, float borderWidth, float brushAngle)
        {
            LoadData(brushType, color1, color2, borderColor, borderWidth, brushAngle, 0.5f, 1.0f);
        }

        /// <summary>
        /// Creates new instance of the object. 
        /// </summary>
        /// <param name="color1">The first color for this entry.</param>
        /// <param name="color2">The second color for this entry.</param>
        /// <param name="borderColor">The border color for this entry.</param>
        /// <param name="borderWidth">The border width for this entry.</param>
        /// <param name="brushSBSFocus">The focus for the SigmaBellShape.</param>
        /// <param name="brushSBSScale">The scale for the SigmaBellShape.</param>
        public ColorData(eBrushTypes brushType, Color color1, Color color2, Color borderColor, float borderWidth, float brushSBSFocus, float brushSBSScale)
        {
            LoadData(brushType, color1, color2, borderColor, borderWidth, 0.0f, brushSBSFocus, brushSBSScale);
        }

        /// <summary>
        /// Loads data into the class, called by constructors. 
        /// </summary>
        /// <param name="color1">The first color for this entry.</param>
        /// <param name="color2">The second color for this entry.</param>
        /// <param name="borderColor">The border color for this entry.</param>
        /// <param name="borderWidth">The border width for this entry.</param>
        /// <param name="brushSBSFocus">The focus for the SigmaBellShape.</param>
        /// <param name="brushSBSScale">The scale for the SigmaBellShape.</param>
        protected void LoadData(eBrushTypes brushType, Color color1, Color color2, Color borderColor, float borderWidth, float brushAngle, float brushSBSFocus, float brushSBSScale)
        {
            _BorderColor = borderColor;
            _BorderWidth = borderWidth;
            _Color1 = color1;
            _Color2 = color2;
            _BrushType = brushType;
            _BrushAngle = brushAngle;
            _BrushSBSFocus = brushSBSFocus;
            _BrushSBSScale = brushSBSScale;
        }

        /// <summary>
        /// Creates Pen object using the BorderColor and BorderWidth properties.
        /// </summary>
        public Pen GetBorderPen(float scaleFactor, PenAlignment penAlignment)
        {
            Pen pen = new Pen(_BorderColor, (float)Math.Round(_BorderWidth * scaleFactor, 0));
            pen.Alignment = penAlignment;
            return pen;
        }

        /// <summary>
        /// Creates a brush of the type specified by BrushType.
        /// </summary>
        /// <param name="path">The graphics path used to construct the brush.</param>
        public Brush GetBrush(GraphicsPath path)
        {
            return GetBrush(path, new PointF(0.5f, 0.5f), _BrushAngle);
        }

        /// <summary>
        /// Creates a brush of the type specified by BrushType.
        /// </summary>
        /// <param name="path">The graphics path used to construct the brush.</param>
        /// <param name="angle">The angle used for the gradients, allowing an override of BrushAngle</param>
        public Brush GetBrush(GraphicsPath path, float angle)
        {
            return GetBrush(path, new PointF(0.5f, 0.5f), angle);
        }

        /// <summary>
        /// Creates a brush of the type specified by BrushType.
        /// </summary>
        /// <param name="path">The graphics path used to construct the brush.</param>
        /// <param name="center">The center point of the gradient as a percentage value typically ranging from 0.0 to 1.0.</param>
        public Brush GetBrush(GraphicsPath path, PointF center)
        {
            return GetBrush(path, center, _BrushAngle);
        }

        /// <summary>
        /// Creates a brush of the type specified by BrushType.
        /// </summary>
        /// <param name="path">The graphics path used to construct the brush.</param>
        /// <param name="center">The center point of the gradient as a percentage value typically ranging from 0.0 to 1.0.</param>
        /// <param name="angle">The angle used for the gradients, allowing an override of BrushAngle</param>
        public Brush GetBrush(GraphicsPath path, PointF center, float angle)
        {
            RectangleF rect;
            switch (_BrushType)
            {
                case eBrushTypes.Solid:
                    return new SolidBrush(_Color1);
                case eBrushTypes.Linear:
                    path.Flatten();
                    rect = path.GetBounds();
                    if (rect.Width > 0.0f && rect.Height > 0.0f)
                        return new LinearGradientBrush(path.GetBounds(), _Color1, _Color2, angle, false);
                    else
                        return null;
                case eBrushTypes.Reflected:
                    LinearGradientBrush lBrush = new LinearGradientBrush(path.GetBounds(), _Color1, _Color2, angle, false);
                    lBrush.SetSigmaBellShape(_BrushSBSFocus, _BrushSBSScale);
                    return lBrush;
                case eBrushTypes.Centered:
                    PointF pt = new PointF();
                    rect = path.GetBounds();
                    PathGradientBrush pBrush = new PathGradientBrush(path);
                    pt.X = rect.X + rect.Width * center.X;
                    pt.Y = rect.Y + rect.Height * center.Y;
                    pBrush.CenterPoint = pt;
                    pBrush.CenterColor = _Color1;
                    pBrush.SurroundColors = new Color[] { _Color2 };
                    return pBrush;
                default:
                    return new SolidBrush(_Color1);
            }
        }

        /// <summary>
        /// Creates SolidBrushObject using Color1.
        /// </summary>
        public Brush GetSolidBrush()
        {
            return new SolidBrush(_Color1);
        }

        /// <summary>
        /// Creates a LinearGradientBrush object.
        /// </summary>
        /// <param name="path">The graphics path used to construct the brush.</param>
        /// <param name="angle">The gradient angle.</param>
        public LinearGradientBrush GetLinearBrush(GraphicsPath path, int angle)
        {
            return new LinearGradientBrush(path.GetBounds(), _Color1, _Color2, angle, false);
        }

        /// <summary>
        /// Creates a PathGradientBrush object.
        /// </summary>
        /// <param name="path">The graphics path used to construct the brush.</param>
        /// <param name="center">The center point of the gradient.</param>
        public PathGradientBrush GetCenteredBrush(GraphicsPath path, PointF center)
        {
            PathGradientBrush brush;

            brush = new PathGradientBrush(path);
            brush.CenterPoint = center;
            brush.CenterColor = _Color1;
            brush.SurroundColors = new Color[] { _Color2 };

            return brush;
        }

        /// <summary>
        /// Creates a LinearGradientBrush object.
        /// </summary>
        /// <param name="path">The graphics path used to construct the brush.</param>
        /// <param name="angle">The gradient angle.</param>
        public LinearGradientBrush GetReflectedBrush(GraphicsPath path, int angle)
        {
            return GetReflectedBrush(path, angle, 0.5f, 1.0f);
        }

        /// <summary>
        /// Creates a LinearGradientBrush object.
        /// </summary>
        /// <param name="path">The graphics path used to construct the brush.</param>
        /// <param name="angle">The gradient angle.</param>
        /// <param name="focus">The focus for the SigmaBellShape.</param>
        public LinearGradientBrush GetReflectedBrush(GraphicsPath path, int angle, float focus)
        {
            return GetReflectedBrush(path, angle, focus, 1.0f);
        }

        /// <summary>
        /// Creates a LinearGradientBrush object.
        /// </summary>
        /// <param name="path">The graphics path used to construct the brush.</param>
        /// <param name="angle">The gradient angle.</param>
        /// <param name="focus">The focus for the SigmaBellShape.</param>
        /// <param name="scale">The scale for the SigmaBellShape.</param>
        public LinearGradientBrush GetReflectedBrush(GraphicsPath path, int angle, float focus, float scale)
        {
            LinearGradientBrush brush = new LinearGradientBrush(path.GetBounds(), _Color1, _Color2, angle, false);
            brush.SetSigmaBellShape(focus, scale);
            return brush;
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
    /// Enumeration containing predefined brush types for the ColorData class.
    /// </summary>
    public enum eBrushTypes
    {
        /// <summary>
        /// Solid brush.
        /// </summary>
        Solid,
        /// <summary>
        /// Linear gradient brush.
        /// </summary>
        Linear,

        /// <summary>
        /// Centered path gradient brush.
        /// </summary>
        Centered,

        /// <summary>
        /// Reflected linear gradient brush.
        /// </summary>
        Reflected
    }

}
