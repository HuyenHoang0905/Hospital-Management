using System;
using System.Text;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace DevComponents.DotNetBar
{
    public abstract class ShapeDescriptor : IShapeDescriptor
    {
        #region IShapeDescriptor Members

        public abstract System.Drawing.Drawing2D.GraphicsPath GetShape(System.Drawing.Rectangle bounds);

        public abstract System.Drawing.Drawing2D.GraphicsPath GetInnerShape(System.Drawing.Rectangle bounds, int borderSize);

        public abstract bool CanDrawShape(System.Drawing.Rectangle bounds);
        #endregion
    }
}
