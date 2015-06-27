using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.ComponentModel;

namespace DevComponents.DotNetBar.MicroCharts
{
    internal class HundredPctBar : MicroChartBase
    {
        public override void CreateChart(MicroChartRenderInfo info)
        {
            int chartHeight = info.ChartHeight;
            int chartWidth = info.ChartWidth;

            int x = 0;
            int y = 0;

            HundredPctMicroChartStyle style = this.Style;
            int drawStep = Math.Max(style.MinBarSize, (chartWidth / (Math.Max(1, info.DataPoints.Count - 1))));
            int dataStep = Math.Max(1, ((info.DataPoints.Count * drawStep) / chartWidth));

            Graphics graphics = info.Graphics;
            double sum = info.Sum;

            if (dataStep > 1)
            {
                sum = 0;
                for (int i = 0; i < info.DataPoints.Count; i += dataStep)
                    sum += Math.Abs(info.DataPoints[i]);
            }

            int colorsCount = style.BarColors.Count;
            int sliceColor = 0;

            int totalPoints = (int)Math.Ceiling((double)info.DataPoints.Count / dataStep);
            MicroChartHotPoint[] microHotPoints = new MicroChartHotPoint[totalPoints];
            int hotPointIndex = 0;
            
            using (Pen pen = new Pen(style.BarOutlineColor, 1))
            {
                for (int i = 0; i < info.DataPoints.Count; i += dataStep)
                {
                    double value = Math.Abs(info.DataPoints[i]);

                    Rectangle barBounds = new Rectangle(x, y, (int)Math.Round(chartWidth * value / sum), chartHeight);

                    using (SolidBrush brush = new SolidBrush(style.BarColors[sliceColor]))
                        graphics.FillRectangle(brush, barBounds);
                    graphics.DrawRectangle(pen, barBounds);

                    microHotPoints[hotPointIndex] = new MicroChartHotPoint(GetHotPointBounds(barBounds), barBounds, style.BarColors[sliceColor], value, i);

                    hotPointIndex++;
                    
                    x += barBounds.Width;

                    sliceColor++;
                    if (sliceColor >= colorsCount)
                        sliceColor = 0;
                }
                
            }

            info.MicroChartHotPoints = microHotPoints;
        }

        private HundredPctMicroChartStyle _Style;
        public virtual HundredPctMicroChartStyle Style
        {
            get { return _Style; }
            set { _Style = value; }
        }

        private static readonly int HotPointOffset = 4;
        private Rectangle GetHotPointBounds(Rectangle barBounds)
        {
            return new Rectangle(barBounds.X + (barBounds.Width - HotPointOffset * 2) / 2,
                barBounds.Y + (barBounds.Height - HotPointOffset * 2) / 2,
                HotPointOffset * 2, HotPointOffset * 2);
        }
    }

    /// <summary>
    /// Defines the style for 100% bar chart.
    /// </summary>
    [System.ComponentModel.ToolboxItem(false), System.ComponentModel.DesignTimeVisible(false), TypeConverter(typeof(System.ComponentModel.ExpandableObjectConverter))]
    public class HundredPctMicroChartStyle
    {
        private List<Color> _BarColors = new List<Color>();
        /// <summary>
        /// Initializes a new instance of the PieMicroChartStyle class.
        /// </summary>
        public HundredPctMicroChartStyle()
        {
            _BarColors.AddRange(new Color[]{ColorScheme.GetColor(0xC00000), ColorScheme.GetColor(0xFF0000), 
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

        /// <summary>
        /// Gets the pre-defined slice colors for the pie chart.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<Color> BarColors
        {
            get { return _BarColors; }
        }

        private Color _BarOutlineColor = ColorScheme.GetColor(0xE2E4E7);
        /// <summary>
        /// Gets or sets the color of the slice outline.
        /// </summary>
        [Category("Appearance"), Description("Indicates color of slice outline.")]
        public Color BarOutlineColor
        {
            get { return _BarOutlineColor; }
            set { _BarOutlineColor = value; OnStyleChanged(); }
        }
        /// <summary>
        /// Gets whether property should be serialized.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeBarOutlineColor()
        {
            return _BarOutlineColor != ColorScheme.GetColor(0xE2E4E7);
        }
        /// <summary>
        /// Resets property to its default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetBarOutlineColor()
        {
            this.BarOutlineColor = ColorScheme.GetColor(0xE2E4E7);
        }

    }
}
