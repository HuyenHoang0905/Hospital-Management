using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar.MicroCharts;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Represents Micro-Chart Control.
    /// </summary>
    [ToolboxBitmap(typeof(MicroChart), "MicroCharts.MicroChart.ico"), ToolboxItem(true), DefaultEvent("Click"), Designer("DevComponents.DotNetBar.Design.MicroChartDesigner, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf"), System.Runtime.InteropServices.ComVisible(false)]
    public class MicroChart : BaseItemControl, ICommandSource
    {
        #region Events

        #endregion

        #region Constructor
        private MicroChartItem _MicroChart = null;
        /// <summary>
        /// Initializes a new instance of the MicroChart class.
        /// </summary>
        public MicroChart()
        {
            this.SetStyle(ControlStyles.Selectable, false);
            _MicroChart = new MicroChartItem();
            _MicroChart.TextVisible = false;
            _MicroChart.MouseOverDataPointChanged += new EventHandler(MicroChartMouseOverDataPointChanged);
            this.HostItem = _MicroChart;
        }
        #endregion

        #region Implementation
        protected override void OnHandleCreated(EventArgs e)
        {
            this.RecalcLayout();
            base.OnHandleCreated(e);
        }
        /// <summary>
        /// Forces the button to perform internal layout.
        /// </summary>
        public override void RecalcLayout()
        {
            if (_MicroChart == null) return;
            Rectangle bounds = this.ClientRectangle;
            bounds.Inflate(-4, -4);
            if (bounds.Width < 4) bounds.Width = 4;
            if (bounds.Height < 4) bounds.Height = 4;
            _MicroChart.Bounds = bounds;
            _MicroChart.ChartHeight = bounds.Height;
            _MicroChart.ChartWidth = bounds.Width;

            base.RecalcLayout();
        }

        protected override Size DefaultSize
        {
            get
            {
                return new Size(96, 24);
            }
        }

        /// <summary>
        /// Gets or sets whether mouse over tooltip for data point is enabled. Default value is true.
        /// </summary>
        [Category("Behavior"), DefaultValue(true), Description("Indicates whether mouse over tooltip for data point is enabled")]
        public bool MouseOverDataPointTooltipEnabled
        {
            get { return _MicroChart.MouseOverDataPointTooltipEnabled; }
            set
            {
                _MicroChart.MouseOverDataPointTooltipEnabled = value;
            }
        }

        /// <summary>
        /// Gets the index of data point (DataPoints collection) that mouse is over or returns -1 if mouse is not over any data-point.
        /// </summary>
        [Browsable(false)]
        public int MouseOverDataPointIndex
        {
            get
            {
                return _MicroChart.MouseOverDataPointIndex;
            }
        }

        private void MicroChartMouseOverDataPointChanged(object sender, EventArgs e)
        {
            OnMouseOverDataPointChanged(e);
        }
        /// <summary>
        /// Occurs when MouseOverDataPointIndex property changes due to user moving the mouse and pointing it to different data point on chart.
        /// </summary>
        public event EventHandler MouseOverDataPointChanged;
        /// <summary>
        /// Raises MouseOverDataPointChanged event.
        /// </summary>
        /// <param name="e">Provides event arguments.</param>
        protected virtual void OnMouseOverDataPointChanged(EventArgs e)
        {
            EventHandler handler = MouseOverDataPointChanged;
            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// Gets or sets the tooltips for each data-point assigned through DataPoints property. If not set control will automatically 
        /// show tooltip based on the data-point value.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<string> DataPointTooltips
        {
            get { return _MicroChart.DataPointTooltips; }
            set
            {
                _MicroChart.DataPointTooltips = value;
            }
        }

        /// <summary>
        /// Gets or sets the chart data points. Note that if you are adding or removing points directly from this collection you must call
        /// Refresh() method on the control to refresh the display.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<double> DataPoints
        {
            get { return _MicroChart.DataPoints; }
            set
            {
                if (value != _MicroChart.DataPoints)
                {
                    List<double> oldValue = _MicroChart.DataPoints;
                    _MicroChart.DataPoints = value;
                    OnDataPointsChanged(oldValue, value);
                }
            }
        }
        private void OnDataPointsChanged(List<double> oldValue, List<double> newValue)
        {
        }

        /// <summary>
        /// Gets or sets the format string for the value when it is displayed as tool-tip for data point.
        /// </summary>
        [DefaultValue(""), Category("Appearance"), Description("Indicates format string for the value when it is displayed as tool-tip for data point.")]
        public string TooltipValueFormatString
        {
            get { return _MicroChart.TooltipValueFormatString; }
            set
            {
                _MicroChart.TooltipValueFormatString = value;
            }
        }

        /// <summary>
        /// Gets or sets whether chart is tracking mouse movement to show data-point and its value on tooltip. Default value is true.
        /// </summary>
        [DefaultValue(true), Category("Behavior"), Description("Indicates whether chart is tracking mouse movement to show data-point and its value on tooltip.")]
        public bool TrackChartPoints
        {
            get { return _MicroChart.TrackChartPoints; }
            set
            {
                _MicroChart.TrackChartPoints = value;
            }
        }

        /// <summary>
        /// Gets or sets whether transition animation between same chart with different data-points is enabled. Default value is true.
        /// </summary>
        [DefaultValue(true), Category("Behavior"), Description("Indicates  whether transition animation between same chart with different data-points is enabled.")]
        public bool AnimationEnabled
        {
            get { return _MicroChart.AnimationEnabled; }
            set
            {
                _MicroChart.AnimationEnabled = value;
            }
        }
        internal bool IsAnimationEnabled
        {
            get
            {
                if (this.DesignMode || NativeFunctions.IsTerminalSession())
                    return false;
                return true;
            }
        }

        /// <summary>
        /// Gets or sets whether button like mouse over tracking is enabled for whole control. When enabled control looks like a button when mouse is over it. Default value is false.
        /// </summary>
        [DefaultValue(false), Category("Behavior"), Description("Indicates whether button like mouse over tracking is enabled for whole control. When enabled control looks like a button when mouse is over it.")]
        public bool MouseOverEnabled
        {
            get { return _MicroChart.MouseOverEnabled; }
            set
            {
                _MicroChart.MouseOverEnabled = value;
            }
        }

        /// <summary>
        /// Invalidates the chart display and requests the re-paint.
        /// </summary>
        /// <param name="animateTransition">Indicates whether to animate transition to new chart</param>
        public void InvalidateChart(bool animateTransition)
        {
            _MicroChart.InvalidateChart(animateTransition);
        }

        /// <summary>
        /// Gets the style used to customize appearance of Line micro-chart.
        /// </summary>
        [Category("Style"), Description("Identifies style used to customize appearance of Line micro-chart."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public LineMicroChartStyle LineChartStyle
        {
            get
            {
                return _MicroChart.LineChartStyle;
            }
        }

        /// <summary>
        /// Gets the style used to customize appearance of Plot micro-chart.
        /// </summary>
        [Category("Style"), Description("Identifies style used to customize appearance of Plot micro-chart."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public PlotMicroChartStyle PlotChartStyle
        {
            get
            {
                return _MicroChart.PlotChartStyle;
            }
        }

        /// <summary>
        /// Gets the style used to customize appearance of Column micro-chart.
        /// </summary>
        [Category("Style"), Description("Identifies style used to customize appearance of Column micro-chart."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public BarMicroChartStyle ColumnChartStyle
        {
            get
            {
                return _MicroChart.ColumnChartStyle;
            }
        }

        /// <summary>
        /// Gets the style used to customize appearance of Bar micro-chart.
        /// </summary>
        [Category("Style"), Description("Identifies style used to customize appearance of Bar micro-chart."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public BarMicroChartStyle BarChartStyle
        {
            get
            {
                return _MicroChart.BarChartStyle;
            }
        }

        /// <summary>
        /// Gets the style used to customize appearance of Win-Lose micro-chart.
        /// </summary>
        [Category("Style"), Description("Identifies style used to customize appearance of Win-Lose micro-chart."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public BarMicroChartStyle WinLoseChartStyle
        {
            get
            {
                return _MicroChart.WinLoseChartStyle;
            }
        }

        /// <summary>
        /// Gets the style used to customize appearance of Pie micro-chart.
        /// </summary>
        [Category("Style"), Description("Identifies style used to customize appearance of Pie micro-chart."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public PieMicroChartStyle PieChartStyle
        {
            get
            {
                return _MicroChart.PieChartStyle;
            }
        }

        /// <summary>
        /// Gets the style used to customize appearance of Area micro-chart.
        /// </summary>
        [Category("Style"), Description("Identifies style used to customize appearance of Area micro-chart."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public AreaMicroChartStyle AreaChartStyle
        {
            get
            {
                return _MicroChart.AreaChartStyle;
            }
        }

        /// <summary>
        /// Gets the style used to customize appearance of 100% micro-chart.
        /// </summary>
        [Category("Style"), Description("Identifies style used to customize appearance of 100% micro-chart."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public HundredPctMicroChartStyle HundredPctChartStyle
        {
            get
            {
                return _MicroChart.HundredPctChartStyle;
            }
        }

        ///// <summary>
        ///// Gets or sets the width of the chart part of the control.
        ///// </summary>
        //[DefaultValue(54), Category("Appearance"), Description("Indicates width of the chart part of the control.")]
        //public int ChartWidth
        //{
        //    get { return _MicroChart.ChartWidth; }
        //    set
        //    {
        //        _MicroChart.ChartWidth = value;
        //    }
        //}

        ///// <summary>
        ///// Gets or sets the height of the chart part of the control.
        ///// </summary>
        //[DefaultValue(15), Category("Appearance"), Description("Indicates height of the chart part of the control.")]
        //public int ChartHeight
        //{
        //    get { return _MicroChart.ChartHeight; }
        //    set
        //    {
        //        _MicroChart.ChartHeight = value;
        //    }
        //}

        /// <summary>
        /// Gets or sets the type of the chart rendered.
        /// </summary>
        [DefaultValue(eMicroChartType.Line), Category("Appearance"), Description("Indicates type of the chart rendered.")]
        public eMicroChartType ChartType
        {
            get { return _MicroChart.ChartType; }
            set
            {
                _MicroChart.ChartType = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum value for data points. By default maximum data point is calculated based on that data displayed by the chart, but when
        /// two charts need to be scaled the same setting maximum and minimum values for them will ensure that scales are visually the same.
        /// </summary>
        [DefaultValue(double.NaN), Category("Behavior"), Description("Indicates maximum value for data points. By default maximum data point is calculated based on that data displayed by the chart, but when two charts need to be scaled the same setting maximum and minimum values for them will ensure that scales are visually the same.")]
        public double DataMaxValue
        {
            get { return _MicroChart.DataMaxValue; }
            set
            {
                _MicroChart.DataMaxValue = value;
            }
        }
        /// <summary>
        /// Gets or sets the minimum value for data points. By default minimum data point is calculated based on that data displayed by the chart, but when
        /// two charts need to be scaled the same setting maximum and minimum values for them will ensure that scales are visually the same.
        /// </summary>
        [DefaultValue(double.NaN), Category("Behavior"), Description("Indicates minimum value for data points. By default minimum data point is calculated based on that data displayed by the chart, but when two charts need to be scaled the same setting maximum and minimum values for them will ensure that scales are visually the same.")]
        public double DataMinValue
        {
            get { return _MicroChart.DataMinValue; }
            set
            {
                _MicroChart.DataMinValue = value;
            }
        }

        ///// <summary>
        ///// Gets or sets whether text-markup support is enabled for items Text property. Default value is true.
        ///// Set this property to false to display HTML or other markup in the item instead of it being parsed as text-markup.
        ///// </summary>
        //[DefaultValue(true), Category("Appearance"), Description("Indicates whether text-markup support is enabled for items Text property.")]
        //public bool EnableMarkup
        //{
        //    get { return _MicroChart.EnableMarkup; }
        //    set
        //    {
        //        _MicroChart.EnableMarkup = value;
        //    }
        //}

        ///// <summary>
        ///// Gets or sets text-position in relation to the chart.
        ///// </summary>
        //[DefaultValue(eMicroChartTextPosition.Left), Category("Appearance"), Description("Indicates text-position in relation to the chart.")]
        //public eMicroChartTextPosition TextPosition
        //{
        //    get { return _MicroChart.TextPosition; }
        //    set
        //    {
        //        _MicroChart.TextPosition = value;
        //    }
        //}

        ///// <summary>
        ///// Gets or sets whether caption/label set using Text property is visible.
        ///// </summary>
        //[DefaultValue(true), Category("Appearance"), Description("Indicates whether caption/label set using Text property is visible.")]
        //public bool TextVisible
        //{
        //    get { return _MicroChart.TextVisible; }
        //    set
        //    {
        //        _MicroChart.TextVisible = value;
        //    }
        //}

        ///// <summary>
        ///// Gets or sets text padding.
        ///// </summary>
        //[Browsable(true), Category("Appearance"), Description("Gets or sets text padding."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        //public Padding TextPadding
        //{
        //    get { return _MicroChart.TextPadding; }
        //}
        //[EditorBrowsable(EditorBrowsableState.Never)]
        //public bool ShouldSerializeTextPadding()
        //{
        //    return _MicroChart.ShouldSerializeTextPadding();
        //}
        //[EditorBrowsable(EditorBrowsableState.Never)]
        //public void ResetTextPadding()
        //{
        //    _MicroChart.ResetTextPadding();
        //}

        /// <summary>
        ///// Gets or sets the text color.
        ///// </summary>
        //[Category("Appearance"), Description("Indicates text color.")]
        //public Color TextColor
        //{
        //    get { return _MicroChart.TextColor; }
        //    set { _MicroChart.TextColor = value; }
        //}
        ///// <summary>
        ///// Gets whether property should be serialized.
        ///// </summary>
        //[EditorBrowsable(EditorBrowsableState.Never)]
        //public bool ShouldSerializeTextColor()
        //{
        //    return _MicroChart.ShouldSerializeTextColor();
        //}
        ///// <summary>
        ///// Resets property to its default value.
        ///// </summary>
        //[EditorBrowsable(EditorBrowsableState.Never)]
        //public void ResetTextColor()
        //{
        //    _MicroChart.ResetTextColor();
        //}
        #endregion

        #region ICommandSource Members
        /// <summary>
        /// Gets whether command is executed when control is clicked.
        /// </summary>
        protected virtual bool ExecuteCommandOnClick
        {
            get { return true; }
        }

        protected virtual void ExecuteCommand()
        {
            if (_Command == null) return;
            CommandManager.ExecuteCommand(this);
        }

        /// <summary>
        /// Gets or sets the command assigned to the item. Default value is null.
        /// <remarks>Note that if this property is set to null Enabled property will be set to false automatically to disable the item.</remarks>
        /// </summary>
        [DefaultValue(null), Category("Commands"), Description("Indicates the command assigned to the item.")]
        public Command Command
        {
            get { return (Command)((ICommandSource)this).Command; }
            set
            {
                ((ICommandSource)this).Command = value;
            }
        }

        private ICommand _Command = null;
        //[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        ICommand ICommandSource.Command
        {
            get
            {
                return _Command;
            }
            set
            {
                bool changed = false;
                if (_Command != value)
                    changed = true;

                if (_Command != null)
                    CommandManager.UnRegisterCommandSource(this, _Command);
                _Command = value;
                if (value != null)
                    CommandManager.RegisterCommand(this, value);
                if (changed)
                    OnCommandChanged();
            }
        }

        /// <summary>
        /// Called when Command property value changes.
        /// </summary>
        protected virtual void OnCommandChanged()
        {
        }

        private object _CommandParameter = null;
        /// <summary>
        /// Gets or sets user defined data value that can be passed to the command when it is executed.
        /// </summary>
        [Browsable(true), DefaultValue(null), Category("Commands"), Description("Indicates user defined data value that can be passed to the command when it is executed."), System.ComponentModel.TypeConverter(typeof(System.ComponentModel.StringConverter)), System.ComponentModel.Localizable(true)]
        public object CommandParameter
        {
            get
            {
                return _CommandParameter;
            }
            set
            {
                _CommandParameter = value;
            }
        }

        #endregion
    }
}
