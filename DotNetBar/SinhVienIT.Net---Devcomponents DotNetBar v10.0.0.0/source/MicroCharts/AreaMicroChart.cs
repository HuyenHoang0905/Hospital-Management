using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;

namespace DevComponents.DotNetBar.MicroCharts
{
    internal class AreaMicroChart : MicroChartBase
    {
        public override void CreateChart(MicroChartRenderInfo info)
        {
            Graphics graphics = info.Graphics;

            int chartHeight = info.ChartHeight - PointRadius * 2;
            int chartWidth = info.ChartWidth - PointRadius * 2;

            int drawStep = Math.Max(3, (chartWidth / (Math.Max(1, info.DataPoints.Count - 1))));
            int dataStep = Math.Max(1, ((info.DataPoints.Count * drawStep) / chartWidth));
            int x = PointRadius;
            double dataPointMinValue = info.DataPointMinValue;

            double zeroValue = _MicroChartStyle.ZeroLineValue;
            if (dataPointMinValue > zeroValue)
                dataPointMinValue = zeroValue;

            double range = info.DataPointMaxValue - dataPointMinValue;
            if (range == 0) range = 1;
            int zeroY = Math.Min((int)(chartHeight * (1 - (zeroValue - dataPointMinValue) / range)), chartHeight - 1) + PointRadius;

            int totalPoints = (int)Math.Ceiling((double)info.DataPoints.Count / dataStep);
            Point[] chartPoints = new Point[totalPoints];
            MicroChartHotPoint[] microHotPoints = new MicroChartHotPoint[totalPoints];
            int index = 0;
            GraphicsPath areaPath = new GraphicsPath();
            Point lastPoint = new Point(0, zeroY);
            Point lowPoint = Point.Empty, highPoint = Point.Empty;

            for (int i = 0; i < info.DataPoints.Count; i += dataStep)
            {
                double value = info.DataPoints[i];
                double nextValue = (i < (info.DataPoints.Count - 1)) ? info.DataPoints[i + 1] : zeroValue;
                Point p = new Point(x, Math.Min((int)(chartHeight * (1 - (value - dataPointMinValue) / range)), chartHeight - 1) + PointRadius);
                Point endPoint = new Point(x + drawStep, Math.Min((int)(chartHeight * (1 - (nextValue - dataPointMinValue) / range)), chartHeight - 1));

                if (lowPoint.IsEmpty && value == info.DataPointMinValue)
                    lowPoint = p;
                else if (highPoint.IsEmpty && value == info.DataPointMaxValue)
                    highPoint = p;

                chartPoints[index] = p;

                if (i + dataStep >= info.DataPoints.Count)
                    areaPath.AddPolygon(new Point[] { new Point(x, zeroY), p, endPoint, new Point(chartWidth, endPoint.Y), new Point(chartWidth, zeroY) });
                else
                    areaPath.AddPolygon(new Point[] { new Point(x, zeroY), p, endPoint, new Point(endPoint.X, zeroY) });

                microHotPoints[index] = new MicroChartHotPoint(GetHotPointBounds(p, info), new Rectangle(p.X - drawStep / 2, 0, endPoint.X - p.X, chartHeight), Color.Gray /*_MicroChartStyle.LineColor*/, value, index);
                index++;
                x += drawStep;
            }

            using (SolidBrush brush = new SolidBrush(_MicroChartStyle.AreaColor))
                graphics.FillPath(brush, areaPath);
            areaPath.Dispose();

            if (!lowPoint.IsEmpty && !_MicroChartStyle.LowPointColor.IsEmpty)
            {
                using (SolidBrush brush = new SolidBrush(_MicroChartStyle.LowPointColor))
                    graphics.FillPolygon(brush, GetChartPointBounds(lowPoint));
            }

            if (!highPoint.IsEmpty && !_MicroChartStyle.HighPointColor.IsEmpty)
            {
                using (SolidBrush brush = new SolidBrush(_MicroChartStyle.HighPointColor))
                    graphics.FillPolygon(brush, GetChartPointBounds(highPoint));
            }

            if (!_MicroChartStyle.FirstPointColor.IsEmpty)
            {
                using (SolidBrush brush = new SolidBrush(_MicroChartStyle.FirstPointColor))
                    graphics.FillPolygon(brush, GetChartPointBounds(chartPoints[0]));
            }

            if (!_MicroChartStyle.LastPointColor.IsEmpty)
            {
                using (SolidBrush brush = new SolidBrush(_MicroChartStyle.LastPointColor))
                    graphics.FillPolygon(brush, GetChartPointBounds(chartPoints[chartPoints.Length - 1]));
            }

            info.MicroChartHotPoints = microHotPoints;

        }
        private static readonly int HotPointOffset = 4;
        private Rectangle GetHotPointBounds(Point hotPoint, MicroChartRenderInfo info)
        {
            //Rectangle bounds = new Rectangle(Math.Min(info.ChartWidth - HotPointOffset * 2, Math.Max(-1, hotPoint.X - HotPointOffset)),
            //    Math.Min(info.ChartHeight - HotPointOffset * 2, Math.Max(-1, hotPoint.Y - HotPointOffset)),
            //    HotPointOffset * 2,
            //    HotPointOffset * 2);
            Rectangle bounds = new Rectangle(hotPoint.X - HotPointOffset,
                hotPoint.Y - HotPointOffset,
                HotPointOffset * 2,
                HotPointOffset * 2);
            return bounds;
        }

        private AreaMicroChartStyle _MicroChartStyle;
        public AreaMicroChartStyle Style
        {
            get { return _MicroChartStyle; }
            set { _MicroChartStyle = value; }
        }
    }

    /// <summary>
    /// Defines the style for Area micro chart.
    /// </summary>
    [System.ComponentModel.ToolboxItem(false), System.ComponentModel.DesignTimeVisible(false), TypeConverter(typeof(System.ComponentModel.ExpandableObjectConverter))]
    public class AreaMicroChartStyle
    {
        /// <summary>
        /// Initializes a new instance of the AreaMicroChartStyle class.
        /// </summary>
        public AreaMicroChartStyle()
        {
        }

        /// <summary>
        /// Occurs when style appearance changes.
        /// </summary>
        public event EventHandler StyleChanged;
        /// <summary>
        /// Raises StyleChanged event.
        /// </summary>
        /// <param name="e">Provides event arguments.</param>
        protected virtual void OnStyleChanged(EventArgs e)
        {
            EventHandler handler = StyleChanged;
            if (handler != null)
                handler(this, e);
        }
        private void OnStyleChanged()
        {
            OnStyleChanged(EventArgs.Empty);
        }

        private double _ZeroLineValue = 0d;
        /// <summary>
        /// Gets or sets the value of the zero line, i.e. where zero line is drawn. Default value is 0.
        /// </summary>
        [DefaultValue(0d), Category("Appearance"), Description("Indicates value of the zero line, i.e. where zero line is drawn.")]
        public double ZeroLineValue
        {
            get { return _ZeroLineValue; }
            set
            {
                _ZeroLineValue = value;
                OnStyleChanged();
            }
        }

        private Color _AreaColor = ColorScheme.GetColor(0x938953);
        /// <summary>
        /// Gets or sets the chart area color.
        /// </summary>
        [Category("Columns"), Description("Indicates chart area color.")]
        public Color AreaColor
        {
            get { return _AreaColor; }
            set { _AreaColor = value; OnStyleChanged(); }
        }
        /// <summary>
        /// Gets whether property should be serialized.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeAreaColor()
        {
            return _AreaColor != ColorScheme.GetColor(0x938953);
        }
        /// <summary>
        /// Resets property to its default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetAreaColor()
        {
            this.AreaColor = ColorScheme.GetColor(0x938953);
        }

        private Color _HighPointColor = Color.Empty;
        /// <summary>
        /// Gets or sets the color of the high point dot on chart.
        /// </summary>
        [Category("Columns"), Description("Indicates color of high point dot on chart..")]
        public Color HighPointColor
        {
            get { return _HighPointColor; }
            set { _HighPointColor = value; OnStyleChanged(); }
        }
        /// <summary>
        /// Gets whether property should be serialized.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeHighPointColor()
        {
            return !_HighPointColor.IsEmpty;
        }
        /// <summary>
        /// Resets property to its default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetHighPointColor()
        {
            this.HighPointColor = Color.Empty;
        }

        private Color _LowPointColor = Color.Empty;
        /// <summary>
        /// Gets or sets the color of the low point dot on chart.
        /// </summary>
        [Category("Columns"), Description("Indicates color of low point dot on chart.")]
        public Color LowPointColor
        {
            get { return _LowPointColor; }
            set { _LowPointColor = value; OnStyleChanged(); }
        }
        /// <summary>
        /// Gets whether property should be serialized.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeLowPointColor()
        {
            return !_LowPointColor.IsEmpty;
        }
        /// <summary>
        /// Resets property to its default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetLowPointColor()
        {
            this.LowPointColor = Color.Empty;
        }

        private Color _FirstPointColor = Color.Empty;
        /// <summary>
        /// Gets or sets the color of the first point dot on chart.
        /// </summary>
        [Category("Columns"), Description("Indicates color of first point dot on chart..")]
        public Color FirstPointColor
        {
            get { return _FirstPointColor; }
            set { _FirstPointColor = value; OnStyleChanged(); }
        }
        /// <summary>
        /// Gets whether property should be serialized.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeFirstPointColor()
        {
            return !_FirstPointColor.IsEmpty;
        }
        /// <summary>
        /// Resets property to its default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetFirstPointColor()
        {
            this.FirstPointColor = Color.Empty;
        }

        private Color _LastPointColor = Color.Empty;
        /// <summary>
        /// Gets or sets the color of the last point dot on chart.
        /// </summary>
        [Category("Columns"), Description("Indicates color of last point dot on chart.")]
        public Color LastPointColor
        {
            get { return _LastPointColor; }
            set { _LastPointColor = value; OnStyleChanged(); }
        }
        /// <summary>
        /// Gets whether property should be serialized.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeLastPointColor()
        {
            return !_LastPointColor.IsEmpty;
        }
        /// <summary>
        /// Resets property to its default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetLastPointColor()
        {
            this.LastPointColor = Color.Empty;
        }
    }
}
