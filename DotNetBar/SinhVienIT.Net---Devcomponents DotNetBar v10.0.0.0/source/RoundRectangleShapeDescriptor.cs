using System;
using System.Text;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Globalization;
using System.ComponentModel.Design.Serialization;
using System.Reflection;

namespace DevComponents.DotNetBar
{

    /// <summary>
    /// Describes the round rectangle shape.
    /// </summary>
    [System.ComponentModel.ToolboxItem(false), System.ComponentModel.DesignTimeVisible(false), TypeConverter(typeof(RoundRectangleShapeDescriptorConverter))]
    public class RoundRectangleShapeDescriptor : ShapeDescriptor
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the RoundCornerDescriptor class.
        /// </summary>
        public RoundRectangleShapeDescriptor()
        {
        }

        /// <summary>
        /// Initializes a new instance of the RoundCornerDescriptor class.
        /// </summary>
        public RoundRectangleShapeDescriptor(int cornerSize) : this(cornerSize, cornerSize, cornerSize, cornerSize)
        {
        }

        /// <summary>
        /// Initializes a new instance of the RoundCornerDescriptor class.
        /// </summary>
        /// <param name="topLeft"></param>
        /// <param name="topRight"></param>
        /// <param name="bottomLeft"></param>
        /// <param name="bottomRight"></param>
        public RoundRectangleShapeDescriptor(int topLeft, int topRight, int bottomLeft, int bottomRight)
        {
            _TopLeft = topLeft;
            _TopRight = topRight;
            _BottomLeft = bottomLeft;
            _BottomRight = bottomRight;
        }
        #endregion

        #region Internal Implementation
        private int _TopLeft = 0;
        /// <summary>
        /// Gets or sets the top-left round corner size.
        /// </summary>
        [DefaultValue(0), Description("Gets or sets the top-left round corner size.")]
        public int TopLeft
        {
            get { return _TopLeft; }
            set
            {
                if (_TopLeft != value)
                {
                    _TopLeft = Math.Max(0, value);
                }
            }
        }

        private int _TopRight = 0;
        /// <summary>
        /// Gets or sets the top-right round corner size.
        /// </summary>
        [DefaultValue(0), Description("Gets or sets the top-right round corner size.")]
        public int TopRight
        {
            get { return _TopRight; }
            set
            {
                if (_TopRight != value)
                {
                    _TopRight = Math.Max(0, value);
                }
            }
        }

        private int _BottomLeft = 0;
        /// <summary>
        /// Gets or sets the bottom-left round corner size.
        /// </summary>
        [DefaultValue(0), Description("Gets or sets the bottom-left round corner size.")]
        public int BottomLeft
        {
            get { return _BottomLeft; }
            set
            {
                if (_BottomLeft != value)
                {
                    _BottomLeft = Math.Max(0, value);
                }
            }
        }

        private int _BottomRight = 0;
        /// <summary>
        /// Gets or sets the bottom-right round corner size.
        /// </summary>
        [DefaultValue(0), Description("Gets or sets the bottom-right round corner size.")]
        public int BottomRight
        {
            get { return _BottomRight; }
            set
            {
                if (_BottomRight != value)
                {
                    _BottomRight = Math.Max(0, value);
                }
            }
        }

        /// <summary>
        /// Gets whether all corner size values are set to zero which results in no rounded corners.
        /// </summary>
        [Browsable(false)]
        public bool IsEmpty
        {
            get { return _TopLeft == 0 && _TopRight == 0 && _BottomLeft == 0 && _BottomRight == 0; }
        }

        /// <summary>
        /// Gets whether all corner size values are set to same value.
        /// </summary>
        [Browsable(false)]
        public bool IsUniform
        {
            get { return _TopLeft == _TopRight && _TopLeft == _BottomLeft && _TopLeft == _BottomRight; }
        }
        #endregion

        #region IShapeDescriptor Members

        public override System.Drawing.Drawing2D.GraphicsPath GetShape(System.Drawing.Rectangle bounds)
        {
            if (!CanDrawShape(bounds))
                return null;

            return GetShape(bounds, _TopLeft, _TopRight, _BottomLeft, _BottomRight);
        }


        public override GraphicsPath GetInnerShape(Rectangle bounds, int borderSize)
        {
            return GetShape(bounds, Math.Max(0, _TopLeft - borderSize), Math.Max(0, _TopRight - borderSize), 
                Math.Max(0, _BottomLeft - borderSize), Math.Max(0, _BottomRight - borderSize));
        }

        private GraphicsPath GetShape(Rectangle bounds, int topLeft, int topRight, int bottomLeft, int bottomRight)
        {
            if (topLeft == 0 && topRight == 0 && bottomLeft == 0 && bottomRight == 0)
            {
                GraphicsPath path = new GraphicsPath();
                //bounds.Width--;
                //bounds.Height--;
                path.AddRectangle(bounds);
                return path;
            }

            return DisplayHelp.GetRoundedRectanglePath(bounds, topLeft, topRight, bottomLeft, bottomRight, eStyleBackgroundPathPart.Complete,
                eCornerType.Rounded, eCornerType.Rounded, eCornerType.Rounded, eCornerType.Rounded, 0);
        }

        public override bool CanDrawShape(System.Drawing.Rectangle bounds)
        {
            if (bounds.Width < Math.Max(_TopLeft + _TopRight, _BottomLeft + _BottomRight) + 1 ||
                bounds.Height < Math.Max(_TopLeft + _TopRight, _BottomLeft + _BottomRight) + 1 || bounds.Height <= 1 || bounds.Width <= 1) 
                return false;

            return true;
        }

        private static RoundRectangleShapeDescriptor _RectangleShape = new RoundRectangleShapeDescriptor();
        public static RoundRectangleShapeDescriptor RectangleShape
        {
            get
            {
                return _RectangleShape;
            }
        }

        private static RoundRectangleShapeDescriptor _RoundCorner2 = new RoundRectangleShapeDescriptor(2);
        public static RoundRectangleShapeDescriptor RoundCorner2
        {
            get
            {
                return _RoundCorner2;
            }
        }

        private static RoundRectangleShapeDescriptor _RoundCorner3 = new RoundRectangleShapeDescriptor(3);
        public static RoundRectangleShapeDescriptor RoundCorner3
        {
            get
            {
                return _RoundCorner3;
            }
        }
        #endregion


    }

    /// <summary>
    /// Represents DocumentDockContainer object converter.
    /// </summary>
    public class RoundRectangleShapeDescriptorConverter : TypeConverter
    {
        public RoundRectangleShapeDescriptorConverter() { }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(InstanceDescriptor))
                return true;
            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == null)
                throw new ArgumentNullException("destinationType");

            if ((destinationType == typeof(InstanceDescriptor)) && (value is RoundRectangleShapeDescriptor))
            {
                RoundRectangleShapeDescriptor doc = (RoundRectangleShapeDescriptor)value;
                Type[] constructorParams = null;
                MemberInfo constructorMemberInfo = null;
                object[] constructorValues = null;

                if (doc.IsEmpty)
                {
                    constructorParams = new Type[0];
                    constructorMemberInfo = typeof(RoundRectangleShapeDescriptor).GetConstructor(constructorParams);
                    constructorValues = new object[0];
                }
                else if (doc.IsUniform)
                {
                    constructorParams = new Type[1] { typeof(int) };
                    constructorMemberInfo = typeof(RoundRectangleShapeDescriptor).GetConstructor(constructorParams);
                    constructorValues = new object[1] { doc.TopLeft };
                }
                else
                {
                    constructorParams = new Type[4] { typeof(int), typeof(int), typeof(int), typeof(int) };
                    constructorMemberInfo = typeof(RoundRectangleShapeDescriptor).GetConstructor(constructorParams);
                    constructorValues = new object[4] { doc.TopLeft, doc.TopRight, doc.BottomLeft, doc.BottomRight};
                }

                if (constructorMemberInfo != null)
                {
                    return new InstanceDescriptor(constructorMemberInfo, constructorValues);
                }
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

    }
}
