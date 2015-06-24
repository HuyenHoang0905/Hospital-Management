using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace DevComponents.DotNetBar.MicroCharts
{
    internal class WinLoseMicroChart : BarBaseMicroChart
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

            double zeroValue = style.ZeroLineValue;

            int totalPoints = (int)Math.Ceiling((double)info.DataPoints.Count / dataStep);
            MicroChartHotPoint[] microHotPoints = new MicroChartHotPoint[totalPoints];
            int index = 0;
            int zeroY = chartHeight / 2;

            System.Drawing.Drawing2D.SmoothingMode smoothingMode = graphics.SmoothingMode;
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;

            using (SolidBrush positiveBarBrush = new SolidBrush(style.PositiveBarColor))
            {
                using (SolidBrush negativeBarBrush = new SolidBrush(style.NegativeBarColor))
                {
                    for (int i = 0; i < info.DataPoints.Count; i += dataStep)
                    {
                        double value = info.DataPoints[i];
                        if (value > style.ZeroLineValue)
                        {
                            Rectangle barBounds = new Rectangle(x, 0, drawStep, Math.Max(1, zeroY));
                            
                            if (value == info.DataPointMaxValue && !style.HighPointBarColor.IsEmpty)
                            {
                                using (SolidBrush brush = new SolidBrush(style.HighPointBarColor))
                                    graphics.FillRectangle(brush, barBounds);
                            }
                            else
                                graphics.FillRectangle(positiveBarBrush, barBounds);

                            microHotPoints[index] = new MicroChartHotPoint(GetHotPointBounds(barBounds, true), new Rectangle(barBounds.X, 0, barBounds.Width, chartHeight), style.PositiveBarColor, value, index);
                        }
                        else
                        {
                            Rectangle barBounds = new Rectangle(x, zeroY, drawStep, Math.Max(1, chartHeight - zeroY));

                            if (value == info.DataPointMinValue && !style.LowPointBarColor.IsEmpty)
                            {
                                using (SolidBrush brush = new SolidBrush(style.LowPointBarColor))
                                    graphics.FillRectangle(brush, barBounds);
                            }
                            else
                                graphics.FillRectangle(negativeBarBrush, barBounds);

                            microHotPoints[index] = new MicroChartHotPoint(GetHotPointBounds(barBounds, false), new Rectangle(barBounds.X, 0, barBounds.Width, chartHeight), style.NegativeBarColor, value, index);
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
}
