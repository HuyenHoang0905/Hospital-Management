using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace DevComponents.DotNetBar.MicroCharts
{
    /// <summary>
    /// Represents the base class each micro-chart implements
    /// </summary>
    internal abstract class MicroChartBase
    {
        internal const int PointRadius = 3;
        /// <summary>
        /// Creates the chart image.
        /// </summary>
        /// <param name="info">Rendering information.</param>
        /// <returns>Image of the chart.</returns>
        public abstract void CreateChart(MicroChartRenderInfo info);

        protected virtual Point[] GetChartPointBounds(Point p)
        {
            return new Point[] { new Point(p.X, p.Y - PointRadius), new Point(p.X - PointRadius, p.Y), new Point(p.X, p.Y + PointRadius), new Point(p.X + PointRadius, p.Y), new Point(p.X, p.Y - PointRadius) };
        }
    }

    internal struct TrendInfo
    {
        public double Slope;
        public double Intercept;
        public double Start;
        public double End;

        /// <summary>
        /// Initializes a new instance of the TrendInfo structure.
        /// </summary>
        /// <param name="slope"></param>
        /// <param name="intercept"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public TrendInfo(double slope, double intercept, double start, double end)
        {
            Slope = slope;
            Intercept = intercept;
            Start = start;
            End = end;
        }

        /// <summary>
        /// Initializes a new instance of the TrendInfo structure.
        /// </summary>
        static TrendInfo()
        {
            
        }
    }

    internal class MicroChartRenderInfo
    {
        public List<double> DataPoints;
        public MicroChartHotPoint[] MicroChartHotPoints;
        public Graphics Graphics;
        public int ChartWidth;
        public int ChartHeight;

        public double DataPointMaxValue;
        public double DataPointMinValue;
        public double Sum;

        public TrendInfo TrendInfo;

        /// <summary>
        /// Initializes a new instance of the MicroChartRenderInfo structure.
        /// </summary>
        /// <param name="dataPoints"></param>
        /// <param name="graphics"></param>
        /// <param name="chartWidth"></param>
        /// <param name="chartHeight"></param>
        public MicroChartRenderInfo(List<double> dataPoints, Graphics graphics, int chartWidth, int chartHeight, double dataMax, double dataMin)
        {
            DataPoints = dataPoints;
            Graphics = graphics;
            ChartWidth = chartWidth;
            ChartHeight = chartHeight;

            MicroChartHotPoints = null;
            DataPointMaxValue = dataMax;
            DataPointMinValue = dataMin;
            Sum = 0;
            TrendInfo = new TrendInfo();

            UpdateChartStats();
        }

        private void UpdateChartStats()
        {
            if (DataPoints == null || DataPoints.Count == 0) return;
            double min = DataPoints[0], max = DataPoints[0];

            // For trending
            double xxSum = 0, xySum = 0, xAxisValuesSum = 0, yAxisValuesSum = 0;
            Sum = Math.Abs(DataPoints[0]);

            for (int i = 1; i < DataPoints.Count; i++)
            {
                double value = DataPoints[i];
                if (value < min) min = value;
                if (value > max) max = value;
                Sum += Math.Abs(value);
                xySum += value * (i + 1);
                xxSum = value * value;
                yAxisValuesSum += value;
                xAxisValuesSum += i + 1;
            }


            double slope = 0, intercept = 0, start = 0, end = 0;
            try
            {
                slope = ((DataPoints.Count * xySum) - (xAxisValuesSum * yAxisValuesSum)) /
                    ((DataPoints.Count * xxSum) - (xAxisValuesSum * xAxisValuesSum));
            }
            catch (DivideByZeroException) { }
            intercept = (yAxisValuesSum - (slope * xAxisValuesSum)) / DataPoints.Count;
            start = Math.Max(0, (slope * DataPoints[0]) + intercept);
            end = (slope * DataPoints[DataPoints.Count - 1]) + intercept;
            this.TrendInfo = new TrendInfo(slope, intercept, start, end);

            if(double.IsNaN(DataPointMaxValue))
                DataPointMaxValue = max;
            if(double.IsNaN(DataPointMinValue))
                DataPointMinValue = min;
        }
    }
}
