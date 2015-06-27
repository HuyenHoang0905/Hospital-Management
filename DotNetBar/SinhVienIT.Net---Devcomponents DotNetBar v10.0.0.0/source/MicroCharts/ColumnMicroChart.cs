using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.ComponentModel;

namespace DevComponents.DotNetBar.MicroCharts
{
    internal class ColumnMicroChart : BarBaseMicroChart
    {
        public override void CreateChart(MicroChartRenderInfo info)
        {
            Graphics graphics = info.Graphics;

            int chartHeight = info.ChartHeight;
            int chartWidth = info.ChartWidth;

            BarMicroChartStyle style = this.Style;
            int drawStep = Math.Max(style.MinBarSize, (chartWidth / (Math.Max(1, info.DataPoints.Count - 1))));
            int dataStep = Math.Max(1, ((info.DataPoints.Count * drawStep) / chartWidth));
            int x = 0;
            double dataPointMinValue = info.DataPointMinValue;

            if (dataPointMinValue > style.ZeroLineValue)
                dataPointMinValue = style.ZeroLineValue;

            double range = info.DataPointMaxValue - dataPointMinValue;
            if (range == 0) range = 1;

            int totalPoints = (int)Math.Ceiling((double)info.DataPoints.Count / dataStep);
            MicroChartHotPoint[] microHotPoints = new MicroChartHotPoint[totalPoints];
            int index = 0;
            int zeroY = Math.Min((int)(chartHeight * (1 - (style.ZeroLineValue - dataPointMinValue) / range)), chartHeight);

            System.Drawing.Drawing2D.SmoothingMode smoothingMode = graphics.SmoothingMode;
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
            using (SolidBrush positiveBarBrush = new SolidBrush(style.PositiveBarColor))
            {
                using (SolidBrush negativeBarBrush = new SolidBrush(style.NegativeBarColor))
                {
                    for (int i = 0; i < info.DataPoints.Count; i += dataStep)
                    {
                        double value = info.DataPoints[i];
                        int y = Math.Min((int)(chartHeight * (1 - (value - dataPointMinValue) / range)), chartHeight - 1);
                        Rectangle barBounds = Rectangle.Empty;
                        if (value > style.ZeroLineValue)
                        {
                            if (zeroY == chartHeight && y == zeroY) y--;
                            barBounds = new Rectangle(x, y, drawStep, Math.Max(1, zeroY - y));
                            if (value == info.DataPointMaxValue && !style.HighPointBarColor.IsEmpty)
                            {
                                using (SolidBrush brush = new SolidBrush(style.HighPointBarColor))
                                    graphics.FillRectangle(brush, barBounds);
                            }
                            else
                                graphics.FillRectangle(positiveBarBrush, barBounds);
                            microHotPoints[index] = new MicroChartHotPoint(GetHotPointBounds(barBounds, true), style.PositiveBarColor, value, index);
                        }
                        else
                        {
                            barBounds = new Rectangle(x, zeroY, drawStep, Math.Max(1, y - zeroY));
                            if (value == info.DataPointMinValue && !style.LowPointBarColor.IsEmpty)
                            {
                                using (SolidBrush brush = new SolidBrush(style.LowPointBarColor))
                                    graphics.FillRectangle(brush, barBounds);
                            }
                            else
                                graphics.FillRectangle(negativeBarBrush, barBounds);
                            microHotPoints[index] = new MicroChartHotPoint(GetHotPointBounds(barBounds, false), style.NegativeBarColor, value, index);
                        }

                        index++;
                        x += drawStep;
                    }
                }
            }
            graphics.SmoothingMode = smoothingMode;
            info.MicroChartHotPoints = microHotPoints;
        }
    }

    /// <summary>
    /// Defines the style for the bar style micro charts.
    /// </summary>
    [System.ComponentModel.ToolboxItem(false), System.ComponentModel.DesignTimeVisible(false), TypeConverter(typeof(System.ComponentModel.ExpandableObjectConverter))]
    public class BarMicroChartStyle
    {
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


        private int _MinBarWidth = 2;
        /// <summary>
        /// Gets or sets the minimum single bar width.
        /// </summary>
        [DefaultValue(2), Category("Appearance"), Description("Indicates minimum single bar width.")]
        public int MinBarSize
        {
            get { return _MinBarWidth; }
            set
            {
                _MinBarWidth = value;
                OnStyleChanged();
            }
        }


        private Color _PositiveBarColor = Color.Black;
        /// <summary>
        /// Gets or sets the color of positive bar value.
        /// </summary>
        [Category("Appearance"), Description("Indicates color of positive bar value.")]
        public Color PositiveBarColor
        {
            get { return _PositiveBarColor; }
            set { _PositiveBarColor = value; OnStyleChanged(); }
        }
        /// <summary>
        /// Gets whether property should be serialized.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializePositiveBarColor()
        {
            return _PositiveBarColor != Color.Black;
        }
        /// <summary>
        /// Resets property to its default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetPositiveBarColor()
        {
            this.PositiveBarColor = Color.Black;
        }

        private Color _NegativeBarColor = Color.Red;
        /// <summary>
        /// Gets or sets the color of negative bar value.
        /// </summary>
        [Category("Appearance"), Description("Indicates color of negative bar value.")]
        public Color NegativeBarColor
        {
            get { return _NegativeBarColor; }
            set { _NegativeBarColor = value; OnStyleChanged(); }
        }
        /// <summary>
        /// Gets whether property should be serialized.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeNegativeBarColor()
        {
            return _NegativeBarColor != Color.Red;
        }
        /// <summary>
        /// Resets property to its default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetNegativeBarColor()
        {
            this.NegativeBarColor = Color.Red;
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

        private double _ZeroLineValue = 0d;
        /// <summary>
        /// Gets or sets the value of the zero line, i.e. pivot point that determines negative and positive values. Default value is 0.
        /// </summary>
        [DefaultValue(0d), Category("Appearance"), Description("Indicates value of the zero line, i.e. pivot point that determines negative and positive values.")]
        public double ZeroLineValue
        {
            get { return _ZeroLineValue; }
            set
            {
                _ZeroLineValue = value;
                OnStyleChanged();
            }
        }

        private Color _LowPointBarColor = Color.Empty;
        /// <summary>
        /// Gets or sets the color of the lowest value bar on graph.
        /// </summary>
        [Category("Columns"), Description("Indicates color of lowest bar on graph.")]
        public Color LowPointBarColor
        {
            get { return _LowPointBarColor; }
            set { _LowPointBarColor = value; OnStyleChanged(); }
        }
        /// <summary>
        /// Gets whether property should be serialized.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeLowPointBarColor()
        {
            return !_LowPointBarColor.IsEmpty;
        }
        /// <summary>
        /// Resets property to its default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetLowPointBarColor()
        {
            this.LowPointBarColor = Color.Empty;
        }

        private Color _HighPointBarColor = Color.Empty;
        /// <summary>
        /// Gets or sets the color of the highest value bar on graph.
        /// </summary>
        [Category("Columns"), Description("Indicates color of highest value bar on graph..")]
        public Color HighPointBarColor
        {
            get { return _HighPointBarColor; }
            set { _HighPointBarColor = value; OnStyleChanged(); }
        }
        /// <summary>
        /// Gets whether property should be serialized.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeHighPointBarColor()
        {
            return !_HighPointBarColor.IsEmpty;
        }
        /// <summary>
        /// Resets property to its default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetHighPointBarColor()
        {
            this.HighPointBarColor = Color.Empty;
        }
    }
}
