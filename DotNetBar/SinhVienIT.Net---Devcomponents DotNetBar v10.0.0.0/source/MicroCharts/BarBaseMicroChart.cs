using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace DevComponents.DotNetBar.MicroCharts
{
    internal abstract class BarBaseMicroChart : MicroChartBase
    {
        internal static readonly int HotPointOffset = 4;
        protected virtual Rectangle GetHotPointBounds(Rectangle barBounds, bool isPositiveValue)
        {
            Rectangle bounds = Rectangle.Empty;
            if (isPositiveValue)
            {
                bounds = new Rectangle(barBounds.X + (barBounds.Width - HotPointOffset * 2) / 2,
                    barBounds.Y - HotPointOffset,
                    HotPointOffset * 2,
                    HotPointOffset * 2);
            }
            else
            {
                bounds = new Rectangle(barBounds.X + (barBounds.Width - HotPointOffset * 2) / 2,
                    barBounds.Bottom - HotPointOffset,
                    HotPointOffset * 2,
                    HotPointOffset * 2);
            }
            return bounds;
        }

        private BarMicroChartStyle _Style;
        public virtual BarMicroChartStyle Style
        {
            get { return _Style; }
            set { _Style = value; }
        }
    }
}
