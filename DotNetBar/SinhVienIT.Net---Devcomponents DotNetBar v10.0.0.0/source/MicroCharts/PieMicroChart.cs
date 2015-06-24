using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace DevComponents.DotNetBar.MicroCharts
{
    internal class PieMicroChart : MicroChartBase
    {
        public override void CreateChart(MicroChartRenderInfo info)
        {
            int chartWidth = Math.Min(info.ChartHeight, info.ChartWidth) - 1;
            int x = (info.ChartWidth - chartWidth) / 2;
            int y = (info.ChartHeight - chartWidth) / 2;

            int dataStep = Math.Max(1, ((info.DataPoints.Count * 15) / 360));

            PieMicroChartStyle style = this.Style;
            Graphics graphics = info.Graphics;
            double sum = info.Sum;

            if (dataStep > 1)
            {
                sum = 0;
                for (int i = 0; i < info.DataPoints.Count; i += dataStep)
                    sum += Math.Abs(info.DataPoints[i]);
            }

            float angle = 0;
            
            int colorsCount = _Style.SliceColors.Count;
            int sliceColor = 0;
            
            int totalPoints = (int)Math.Ceiling((double)info.DataPoints.Count / dataStep);
            MicroChartHotPoint[] microHotPoints = new MicroChartHotPoint[totalPoints];
            int hotPointIndex = 0;

            using (Pen pen = new Pen(_Style.SliceOutlineColor, 1))
            {
                for (int i = 0; i < info.DataPoints.Count; i += dataStep)
                {
                    double value = Math.Abs(info.DataPoints[i]);
                    float sweepAngle = (float)Math.Max(1, Math.Round((float)(360 * (value / sum))));

                    using (SolidBrush brush = new SolidBrush(_Style.SliceColors[sliceColor]))
                        graphics.FillPie(brush, x, y, chartWidth, chartWidth, angle, sweepAngle);
                    graphics.DrawPie(pen, x, y, chartWidth, chartWidth, angle, sweepAngle);

                    Rectangle hotPointBounds = new Rectangle(
                        x + (int)(chartWidth / 2 + chartWidth / 3 * Math.Cos((angle + sweepAngle / 2) * Math.PI / 180)),
                        y + (int)(chartWidth / 2 + chartWidth / 3 * Math.Sin((angle + sweepAngle / 2) * Math.PI / 180)),
                        1, 1);
                    hotPointBounds.Inflate(4, 4);
                    microHotPoints[hotPointIndex] = new MicroChartHotPoint(hotPointBounds, _Style.SliceColors[sliceColor], value, angle, sweepAngle, i);
                    hotPointIndex++;

                    angle += sweepAngle;
                    sliceColor++;
                    if (sliceColor >= colorsCount)
                        sliceColor = 0;
                }
            }

            info.MicroChartHotPoints = microHotPoints;
        }

        private PieMicroChartStyle _Style;
        public virtual PieMicroChartStyle Style
        {
            get { return _Style; }
            set { _Style = value; }
        }
    }
    /// <summary>
    /// Defines the style for pie chart.
    /// </summary>
    [System.ComponentModel.ToolboxItem(false), System.ComponentModel.DesignTimeVisible(false), TypeConverter(typeof(System.ComponentModel.ExpandableObjectConverter))]
    public class PieMicroChartStyle
    {
        private List<Color> _SliceColors = new List<Color>();
        /// <summary>
        /// Initializes a new instance of the PieMicroChartStyle class.
        /// </summary>
        public PieMicroChartStyle()
        {
            _SliceColors.AddRange(new Color[]{ColorScheme.GetColor(0xC00000), ColorScheme.GetColor(0xFF0000), 
                ColorScheme.GetColor(0xFFC000), ColorScheme.GetColor(0xFFFF00), ColorScheme.GetColor(0x92D050),
                ColorScheme.GetColor(0x00B050), ColorScheme.GetColor(0x00B0F0), ColorScheme.GetColor(0x0070C0),
                ColorScheme.GetColor(0x002060), ColorScheme.GetColor(0x7030A0), ColorScheme.GetColor(0xEEECE1),
                ColorScheme.GetColor(0x1F497D), ColorScheme.GetColor(0x4F81BD), ColorScheme.GetColor(0xC0504D),
                ColorScheme.GetColor(0x9BBB59), ColorScheme.GetColor(0x8064A2), ColorScheme.GetColor(0x4BACC6),
                ColorScheme.GetColor(0xF79646), ColorScheme.GetColor(0xF2F2F2), ColorScheme.GetColor(0xDDD9C3),
                ColorScheme.GetColor(0xC6D9F0), ColorScheme.GetColor(0xF2DCDB), ColorScheme.GetColor(0xEBF1DD),
                ColorScheme.GetColor(0xE5E0EC), ColorScheme.GetColor(0xDBEEF3), ColorScheme.GetColor(0xFDEADA),
                ColorScheme.GetColor(0xD8D8D8), ColorScheme.GetColor(0xC4BD97), ColorScheme.GetColor(0x8DB3E2),
                ColorScheme.GetColor(0xE5B9B7), ColorScheme.GetColor(0xD7E3BC), ColorScheme.GetColor(0xCCC1D9),
                ColorScheme.GetColor(0xB7DDE8), ColorScheme.GetColor(0xFBD5B5), ColorScheme.GetColor(0xBFBFBF),
                ColorScheme.GetColor(0x938953), ColorScheme.GetColor(0x548DD4), ColorScheme.GetColor(0x95B3D7),
                ColorScheme.GetColor(0xD99694), ColorScheme.GetColor(0xC3D69B), ColorScheme.GetColor(0xB2A1C7),
                ColorScheme.GetColor(0x92CDDC), ColorScheme.GetColor(0xFAC08F), ColorScheme.GetColor(0xA5A5A5),
                ColorScheme.GetColor(0x494429), ColorScheme.GetColor(0x17365D), ColorScheme.GetColor(0x366092),
                ColorScheme.GetColor(0x953734), ColorScheme.GetColor(0x76923C), ColorScheme.GetColor(0x5F497A),
                ColorScheme.GetColor(0x31859B), ColorScheme.GetColor(0xE36C09)
            });
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

        /// <summary>
        /// Gets the pre-defined slice colors for the pie chart.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<Color> SliceColors
        {
            get { return _SliceColors; }
        }

        private Color _SliceOutlineColor = ColorScheme.GetColor(0xE2E4E7);
        /// <summary>
        /// Gets or sets the color of the slice outline.
        /// </summary>
        [Category("Appearance"), Description("Indicates color of slice outline.")]
        public Color SliceOutlineColor
        {
            get { return _SliceOutlineColor; }
            set { _SliceOutlineColor = value; OnStyleChanged(); }
        }
        /// <summary>
        /// Gets whether property should be serialized.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeSliceOutlineColor()
        {
            return _SliceOutlineColor != ColorScheme.GetColor(0xE2E4E7);
        }
        /// <summary>
        /// Resets property to its default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetSliceOutlineColor()
        {
            this.SliceOutlineColor = ColorScheme.GetColor(0xE2E4E7);
        }

    }
}
