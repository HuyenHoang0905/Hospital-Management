using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;

namespace DevComponents.DotNetBar.MicroCharts
{
    internal class LineMicroChart : MicroChartBase
    {
        public override void CreateChart(MicroChartRenderInfo info)
        {
            Graphics graphics = info.Graphics;

            int chartHeight = info.ChartHeight - PointRadius * 2;
            int chartWidth = info.ChartWidth - PointRadius * 2;

            int drawStep = (int)Math.Max(2, chartWidth / (Math.Max(1, info.DataPoints.Count - 1)));
            int dataStep = Math.Max(1, ((info.DataPoints.Count * drawStep) / chartWidth));
            if (info.DataPoints.Count <= 2) dataStep = 1;
            int x = PointRadius;
            double dataPointMinValue = info.DataPointMinValue;
            double dataPointMaxValue = info.DataPointMaxValue;

            if (_MicroChartStyle.DrawZeroLine && dataPointMinValue > _MicroChartStyle.ZeroLineValue)
                dataPointMinValue = _MicroChartStyle.ZeroLineValue;
            if (_MicroChartStyle.DrawControlLine1)
            {
                if (dataPointMinValue > _MicroChartStyle.ControlLine1StartValue)
                    dataPointMinValue = _MicroChartStyle.ControlLine1StartValue;
                if (dataPointMinValue > _MicroChartStyle.ControlLine1EndValue)
                    dataPointMinValue = _MicroChartStyle.ControlLine1EndValue;
                if (dataPointMaxValue < _MicroChartStyle.ControlLine1StartValue)
                    dataPointMaxValue = _MicroChartStyle.ControlLine1StartValue;
                if (dataPointMaxValue < _MicroChartStyle.ControlLine1EndValue)
                    dataPointMaxValue = _MicroChartStyle.ControlLine1EndValue;
            }
            if (_MicroChartStyle.DrawControlLine2)
            {
                if (dataPointMinValue > _MicroChartStyle.ControlLine2StartValue)
                    dataPointMinValue = _MicroChartStyle.ControlLine2StartValue;
                if (dataPointMinValue > _MicroChartStyle.ControlLine2EndValue)
                    dataPointMinValue = _MicroChartStyle.ControlLine2EndValue;
                if (dataPointMaxValue < _MicroChartStyle.ControlLine2StartValue)
                    dataPointMaxValue = _MicroChartStyle.ControlLine2StartValue;
                if (dataPointMaxValue < _MicroChartStyle.ControlLine2EndValue)
                    dataPointMaxValue = _MicroChartStyle.ControlLine2EndValue;
            }

            
            double range = dataPointMaxValue - dataPointMinValue;
            if (range == 0) range = 1;

            int totalPoints = (int)Math.Ceiling((double)info.DataPoints.Count / dataStep);
            Point[] chartPoints = new Point[totalPoints];
            MicroChartHotPoint[] microHotPoints = new MicroChartHotPoint[totalPoints];
            Point lowPoint = Point.Empty, highPoint = Point.Empty;
            int index = 0;
            for (int i = 0; i < info.DataPoints.Count; i += dataStep)
            {
                double value = info.DataPoints[i];
                Point p = new Point(x, (int)Math.Min((int)(chartHeight * (1 - (value - dataPointMinValue) / range)), chartHeight - 1) + PointRadius);
                if (lowPoint.IsEmpty && value == info.DataPointMinValue)
                    lowPoint = p;
                else if (highPoint.IsEmpty && value == dataPointMaxValue)
                    highPoint = p;

                chartPoints[index] = p;
                microHotPoints[index] = new MicroChartHotPoint(GetHotPointBounds(p, info), new Rectangle(p.X - drawStep / 2, 0, drawStep, chartHeight), _MicroChartStyle.LineColor, value, index);
                index++;
                x += drawStep;
                if (x > chartWidth || i >= info.DataPoints.Count - dataStep * 2) x = chartWidth;
            }

            if (_MicroChartStyle.DrawAverageLine && !_MicroChartStyle.AverageLineColor.IsEmpty)
            {
                using (Pen pen = new Pen(_MicroChartStyle.AverageLineColor))
                    graphics.DrawLine(pen, 0, chartHeight / 2, chartWidth, chartHeight / 2);
            }

            //if (_MicroChartStyle.DrawTrendLine && !_MicroChartStyle.TrendLineColor.IsEmpty)
            //{
            //    using (Pen pen = new Pen(_MicroChartStyle.TrendLineColor))
            //        graphics.DrawLine(pen, 0, (int)(chartHeight * (1 - (info.TrendInfo.Start - dataPointMinValue) / range)),
            //            chartWidth, (int)(chartHeight * (1 - (info.TrendInfo.End - dataPointMinValue) / range)));
            //}

            if (_MicroChartStyle.DrawZeroLine && !_MicroChartStyle.ZeroLineColor.IsEmpty)
            {
                using (Pen pen = new Pen(_MicroChartStyle.ZeroLineColor))
                {
                    int y = Math.Min((int)(chartHeight * (1 - (_MicroChartStyle.ZeroLineValue - dataPointMinValue) / range)) + PointRadius, (chartHeight + PointRadius) - 1);
                    if (y < 0) y = 0;
                    graphics.DrawLine(pen, 0, y, chartWidth, y);
                }
            }

            if (_MicroChartStyle.DrawControlLine1 && !_MicroChartStyle.ControlLine1Color.IsEmpty)
            {
                using (Pen pen = new Pen(_MicroChartStyle.ControlLine1Color))
                {
                    int y1 = Math.Min((int)(chartHeight * (1 - (_MicroChartStyle.ControlLine1StartValue - dataPointMinValue) / range)) + PointRadius, (chartHeight + PointRadius) - 1);
                    if (y1 < 0) y1 = 0;
                    int y2 = Math.Min((int)(chartHeight * (1 - (_MicroChartStyle.ControlLine1EndValue - dataPointMinValue) / range)) + PointRadius, (chartHeight + PointRadius) - 1);
                    if (y2 < 0) y2 = 0;
                    graphics.DrawLine(pen, 0, y1, chartWidth, y2);
                }
            }
            if (_MicroChartStyle.DrawControlLine2 && !_MicroChartStyle.ControlLine2Color.IsEmpty)
            {
                using (Pen pen = new Pen(_MicroChartStyle.ControlLine2Color))
                {
                    int y1 = Math.Min((int)(chartHeight * (1 - (_MicroChartStyle.ControlLine2StartValue - dataPointMinValue) / range)) + PointRadius, (chartHeight + PointRadius) - 1);
                    if (y1 < 0) y1 = 0;
                    int y2 = Math.Min((int)(chartHeight * (1 - (_MicroChartStyle.ControlLine2EndValue - dataPointMinValue) / range)) + PointRadius, (chartHeight + PointRadius) - 1);
                    if (y2 < 0) y2 = 0;
                    graphics.DrawLine(pen, 0, y1, chartWidth, y2);
                }
            }

            if (chartPoints.Length > 1)
            {
                using (Pen pen = new Pen(_MicroChartStyle.LineColor))
                    graphics.DrawLines(pen, chartPoints);
            }

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

        private LineMicroChartStyle _MicroChartStyle;
        public LineMicroChartStyle Style
        {
            get { return _MicroChartStyle; }
            set { _MicroChartStyle = value; }
        }
    }

    /// <summary>
    /// Defines the style for the line micro chart.
    /// </summary>
    [System.ComponentModel.ToolboxItem(false), System.ComponentModel.DesignTimeVisible(false), TypeConverter(typeof(System.ComponentModel.ExpandableObjectConverter))]
    public class LineMicroChartStyle
    {
        private Color _LineColor = Color.Black;
        /// <summary>
        /// Gets or sets the color of the chart line.
        /// </summary>
        [Category("Appearance"), Description("Indicates color of chart line .")]
        public Color LineColor
        {
            get { return _LineColor; }
            set { _LineColor = value; OnStyleChanged(); }
        }
        /// <summary>
        /// Gets whether property should be serialized.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeLineColor()
        {
            return _LineColor != Color.Black;
        }
        /// <summary>
        /// Resets property to its default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetLineColor()
        {
            this.LineColor = Color.Black;
        }

        private bool _DrawAverageLine = false;
        /// <summary>
        /// Gets or sets whether average line is drawn.
        /// </summary>
        [DefaultValue(false), Category("Appearance")]
        public bool DrawAverageLine
        {
            get { return _DrawAverageLine; }
            set
            {
                _DrawAverageLine = value;
                OnStyleChanged();
            }
        }

        private Color _AverageLineColor = ColorScheme.GetColor(0xFAC08F);
        /// <summary>
        /// Gets or sets the color of the 
        /// </summary>
        [Category("Columns"), Description("Indicates color of.")]
        public Color AverageLineColor
        {
            get { return _AverageLineColor; }
            set { _AverageLineColor = value; OnStyleChanged(); }
        }
        /// <summary>
        /// Gets whether property should be serialized.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeAverageLineColor()
        {
            return _AverageLineColor != ColorScheme.GetColor(0xFAC08F);
        }
        /// <summary>
        /// Resets property to its default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetAverageLineColor()
        {
            this.AverageLineColor = ColorScheme.GetColor(0xFAC08F);
        }

        //private bool _DrawTrendLine = false;
        //[DefaultValue(false), Category("Appearance")]
        //public bool DrawTrendLine
        //{
        //    get { return _DrawTrendLine; }
        //    set
        //    {
        //        _DrawTrendLine = value;
        //    }
        //}

        //private Color _TrendLineColor = ColorScheme.GetColor(0xBFBFBF);
        ///// <summary>
        ///// Gets or sets the color of the trend line.
        ///// </summary>
        //[Category("Columns"), Description("Indicates color of trend line.")]
        //public Color TrendLineColor
        //{
        //    get { return _TrendLineColor; }
        //    set { _TrendLineColor = value; }
        //}
        ///// <summary>
        ///// Gets whether property should be serialized.
        ///// </summary>
        //[EditorBrowsable(EditorBrowsableState.Never)]
        //public bool ShouldSerializeTrendLineColor()
        //{
        //    return !_TrendLineColor.IsEmpty;
        //}
        ///// <summary>
        ///// Resets property to its default value.
        ///// </summary>
        //[EditorBrowsable(EditorBrowsableState.Never)]
        //public void ResetTrendLineColor()
        //{
        //    this.TrendLineColor = Color.Empty;
        //}

        private bool _DrawZeroLine = true;
        /// <summary>
        /// Gets or sets whether zero-line is drawn on chart.
        /// </summary>
        [DefaultValue(true), Category("Appearance"), Description("Gets or sets whether zero-line is drawn on chart.")]
        public bool DrawZeroLine
        {
            get { return _DrawZeroLine; }
            set
            {
                _DrawZeroLine = value;
                OnStyleChanged();
            }
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

        private Color _ZeroLineColor = Color.Red;
        /// <summary>
        /// Gets or sets the color of the 
        /// </summary>
        [Category("Columns"), Description("Indicates color of.")]
        public Color ZeroLineColor
        {
            get { return _ZeroLineColor; }
            set { _ZeroLineColor = value; OnStyleChanged(); }
        }
        /// <summary>
        /// Gets whether property should be serialized.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeZeroLineColor()
        {
            return _ZeroLineColor != Color.Red;
        }
        /// <summary>
        /// Resets property to its default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetZeroLineColor()
        {
            this.ZeroLineColor = Color.Red;
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

        private bool _DrawControlLine1 = false;
        /// <summary>
        /// Gets or sets whether control line is drawn. Default value is false. Control lines can be used to display for example low and high control bounds for the chart.
        /// </summary>
        [DefaultValue(false), Description("Indicates whether control line is drawn. Control lines can be used to display for example low and high control bounds for the chart")]
        public bool DrawControlLine1
        {
            get { return _DrawControlLine1; }
            set { _DrawControlLine1 = value; OnStyleChanged(); }
        }

        private Color _ControlLine1Color = Color.Empty;
        /// <summary>
        /// Gets or sets the color of the first control line.
        /// </summary>
        [Category("Columns"), Description("Indicates color of.")]
        public  Color ControlLine1Color
        {
        	get {	return _ControlLine1Color; }
            set { _ControlLine1Color = value; OnStyleChanged(); }
        }
        /// <summary>
        /// Gets whether property should be serialized.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeControlLine1Color()
        {
            return !_ControlLine1Color.IsEmpty;
        }
        /// <summary>
        /// Resets property to its default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetControlLine1Color()
        {
            this.ControlLine1Color = Color.Empty;
        }
        private double _ControlLine1StartValue = 0d;
        /// <summary>
        /// Gets or sets starting value that is used to draw first control line.
        /// </summary>
        [DefaultValue(0d), Description("Indicates the starting value that is used to draw first control line.")]
        public double ControlLine1StartValue
        {
        	get {	return _ControlLine1StartValue; }
            set { _ControlLine1StartValue = value; OnStyleChanged(); }
        }
        private double _ControlLine1EndValue = 0d;
        /// <summary>
        /// Gets or sets end value that is used to draw first control line.
        /// </summary>
        [DefaultValue(0d), Description("Indicates the end value that is used to draw first control line.")]
        public double ControlLine1EndValue
        {
        	get {	return _ControlLine1EndValue; }
            set { _ControlLine1EndValue = value; OnStyleChanged(); }
        }


        private bool _DrawControlLine2 = false;
        /// <summary>
        /// Gets or sets whether control line is drawn. Default value is false. Control lines can be used to display for example low and high control bounds for the chart.
        /// </summary>
        [DefaultValue(false), Description("Indicates whether control line is drawn. Control lines can be used to display for example low and high control bounds for the chart")]
        public bool DrawControlLine2
        {
            get { return _DrawControlLine2; }
            set { _DrawControlLine2 = value; OnStyleChanged(); }
        }

        private Color _ControlLine2Color = Color.Empty;
        /// <summary>
        /// Gets or sets the color of the second control line.
        /// </summary>
        [Category("Columns"), Description("Indicates color of second control line.")]
        public Color ControlLine2Color
        {
            get { return _ControlLine2Color; }
            set { _ControlLine2Color = value; OnStyleChanged(); }
        }
        /// <summary>
        /// Gets whether property should be serialized.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeControlLine2Color()
        {
            return !_ControlLine2Color.IsEmpty;
        }
        /// <summary>
        /// Resets property to its default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetControlLine2Color()
        {
            this.ControlLine2Color = Color.Empty;
        }
        private double _ControlLine2StartValue = 0d;
        /// <summary>
        /// Gets or sets starting value that is used to draw second control line.
        /// </summary>
        [DefaultValue(0d), Description("Indicates the starting value that is used to draw first second line.")]
        public double ControlLine2StartValue
        {
            get { return _ControlLine2StartValue; }
            set { _ControlLine2StartValue = value; OnStyleChanged(); }
        }
        private double _ControlLine2EndValue = 0d;
        /// <summary>
        /// Gets or sets end value that is used to draw second control line.
        /// </summary>
        [DefaultValue(0d), Description("Indicates the end value that is used to draw second control line.")]
        public double ControlLine2EndValue
        {
            get { return _ControlLine2EndValue; }
            set { _ControlLine2EndValue = value; OnStyleChanged(); }
        }
    }
}
