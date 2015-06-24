using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace DevComponents.DotNetBar.MicroCharts
{
    internal class BarMicroChart : BarBaseMicroChart
    {
        public override void CreateChart(MicroChartRenderInfo info)
        {
            Graphics graphics = info.Graphics;

            int chartHeight = info.ChartHeight;
            int chartWidth = info.ChartWidth;

            BarMicroChartStyle style = this.Style;
            int drawStep = Math.Max(style.MinBarSize, (chartHeight / (Math.Max(1, info.DataPoints.Count))));
            int dataStep = Math.Max(1, ((info.DataPoints.Count * (drawStep + 1)) / chartHeight));
            int y = 0;
            double dataPointMinValue = info.DataPointMinValue;

            if (dataPointMinValue > style.ZeroLineValue)
                dataPointMinValue = style.ZeroLineValue;

            double range = info.DataPointMaxValue - dataPointMinValue;
            if (range == 0) range = 1;

            int totalPoints = (int)Math.Ceiling((double)info.DataPoints.Count / dataStep);
            MicroChartHotPoint[] microHotPoints = new MicroChartHotPoint[totalPoints];
            int index = 0;
            int zeroX = Math.Min((int)(chartWidth * (1 - (style.ZeroLineValue - dataPointMinValue) / range)), chartWidth);

            System.Drawing.Drawing2D.SmoothingMode smoothingMode = graphics.SmoothingMode;
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
            using (SolidBrush positiveBarBrush = new SolidBrush(style.PositiveBarColor))
            {
                using (SolidBrush negativeBarBrush = new SolidBrush(style.NegativeBarColor))
                {
                    for (int i = 0; i < info.DataPoints.Count; i += dataStep)
                    {
                        double value = info.DataPoints[i];
                        int x = Math.Min((int)(chartWidth * (1 - (value - dataPointMinValue) / range)), chartWidth - 1);
                        Rectangle barBounds = Rectangle.Empty;
                        if (value > style.ZeroLineValue)
                        {
                            if (zeroX == chartWidth && x == zeroX) x--;
                            barBounds = new Rectangle(x, y, Math.Max(1, zeroX - x), drawStep);

                            if (value == info.DataPointMaxValue && !style.HighPointBarColor.IsEmpty)
                            {
                                using (SolidBrush brush = new SolidBrush(style.HighPointBarColor))
                                    graphics.FillRectangle(brush, barBounds);
                            }
                            else
                                graphics.FillRectangle(positiveBarBrush, barBounds);

                            microHotPoints[index] = new MicroChartHotPoint(GetHotPointBounds(barBounds, true), new Rectangle(0, barBounds.Y, chartWidth, barBounds.Height), style.PositiveBarColor, value, index);
                        }
                        else
                        {
                            barBounds = new Rectangle(zeroX, y, Math.Max(1, x - zeroX), drawStep);
                            
                            if (value == info.DataPointMinValue && !style.LowPointBarColor.IsEmpty)
                            {
                                using (SolidBrush brush = new SolidBrush(style.LowPointBarColor))
                                    graphics.FillRectangle(brush, barBounds);
                            }
                            else
                                graphics.FillRectangle(negativeBarBrush, barBounds);

                            microHotPoints[index] = new MicroChartHotPoint(GetHotPointBounds(barBounds, false), new Rectangle(0, barBounds.Y, chartWidth, barBounds.Height), style.NegativeBarColor, value, index);
                        }

                        index++;
                        y += drawStep + 1;
                    }
                }
            }
            graphics.SmoothingMode = smoothingMode;
            info.MicroChartHotPoints = microHotPoints;
        }

        protected override Rectangle GetHotPointBounds(Rectangle barBounds, bool isPositiveValue)
        {
            Rectangle bounds = Rectangle.Empty;
            if (isPositiveValue)
            {
                bounds = new Rectangle(barBounds.X - HotPointOffset,
                    barBounds.Y + (barBounds.Height - HotPointOffset * 2) / 2,
                    HotPointOffset * 2,
                    HotPointOffset * 2);
            }
            else
            {
                bounds = new Rectangle(barBounds.Right - HotPointOffset,
                    barBounds.Y + (barBounds.Height - HotPointOffset * 2) / 2,
                    HotPointOffset * 2,
                    HotPointOffset * 2);
            }
            return bounds;
        }
    }
}
