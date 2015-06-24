using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing.Imaging;
using DevComponents.DotNetBar.MicroCharts;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Represents the micro-chart item.
    /// </summary>
    [ToolboxItem(false), DesignTimeVisible(false), DefaultEvent("Click"), Designer("DevComponents.DotNetBar.Design.MicroChartItemDesigner, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
    public class MicroChartItem : PopupItem, IPersonalizedMenuItem
    {
        #region Events

        #endregion

        #region Constructor
        internal static readonly int TextChartSpacing = 3;

        /// <summary>
        /// Creates new instance of MicroChartItem.
        /// </summary>
        public MicroChartItem() : this("", "") { }
        /// <summary>
        /// Creates new instance of MicroChartItem and assigns the name to it.
        /// </summary>
        /// <param name="sItemName">Item name.</param>
        public MicroChartItem(string sItemName) : this(sItemName, "") { }
        /// <summary>
        /// Creates new instance of MicroChartItem and assigns the name and text to it.
        /// </summary>
        /// <param name="sItemName">Item name.</param>
        /// <param name="ItemText">item text.</param>
        public MicroChartItem(string sItemName, string ItemText)
            : base(sItemName, ItemText)
        {
            _Margin.PropertyChanged += new PropertyChangedEventHandler(MarginPropertyChanged);

            _LineMicroChartStyle = new LineMicroChartStyle();
            _LineMicroChartStyle.StyleChanged += new EventHandler(ChartStyleChanged);

            _PlotMicroChartStyle = new PlotMicroChartStyle();
            _PlotMicroChartStyle.StyleChanged += new EventHandler(ChartStyleChanged);

            _ColumnMicroChartStyle = new BarMicroChartStyle();
            _ColumnMicroChartStyle.StyleChanged += new EventHandler(ChartStyleChanged);

            _BarMicroChartStyle = new BarMicroChartStyle();
            _BarMicroChartStyle.StyleChanged += new EventHandler(ChartStyleChanged);

            _WinLoseMicroChartStyle = new BarMicroChartStyle();
            _WinLoseMicroChartStyle.StyleChanged += new EventHandler(ChartStyleChanged);

            _PieMicroChartStyle = new PieMicroChartStyle();
            _PieMicroChartStyle.StyleChanged += new EventHandler(ChartStyleChanged);

            _AreaMicroChartStyle = new AreaMicroChartStyle();
            _AreaMicroChartStyle.StyleChanged += new EventHandler(ChartStyleChanged);

            _HundredPctMicroChartStyle = new HundredPctMicroChartStyle();
            _HundredPctMicroChartStyle.StyleChanged += new EventHandler(ChartStyleChanged);
        }

        /// <summary>
        /// Returns copy of the item.
        /// </summary>
        public override BaseItem Copy()
        {
            MicroChartItem objCopy = new MicroChartItem(m_Name);
            this.CopyToItem(objCopy);
            return objCopy;
        }

        /// <summary>
        /// Copies the MicroChartItem specific properties to new instance of the item.
        /// </summary>
        /// <param name="copy">New MicroChartItem instance.</param>
        internal void InternalCopyToItem(MicroChartItem copy)
        {
            CopyToItem(copy);
        }

        /// <summary>
        /// Copies the MicroChartItem specific properties to new instance of the item.
        /// </summary>
        /// <param name="copy">New MicroChartItem instance.</param>
        protected override void CopyToItem(BaseItem copy)
        {
            MicroChartItem item = copy as MicroChartItem;

            if (item != null)
            {
                item.ChartHeight = this.ChartHeight;
                item.ChartType = this.ChartType;
                item.ChartWidth = this.ChartWidth;
                item.DataPoints = this.DataPoints;
                item.MouseOverEnabled = this.MouseOverEnabled;
                item.TextVisible = this.TextVisible;
                item.TrackChartPoints = this.TrackChartPoints;

                base.CopyToItem(item);
            }
        }
        #endregion

        #region Implementation
        protected override void Dispose(bool disposing)
        {
            if (_TransitionWorker != null)
            {
                _TransitionWorker.Dispose();
                _TransitionWorker = null;
            }
            Image image = _ChartImage;
            _ChartImage = null;
            if (image != null)
                image.Dispose();

            base.Dispose(disposing);
        }

        private int _TextWidth = 0;
        /// <summary>
        /// Gets or sets the suggested text-width. If you want to make sure that text you set wraps over multiple lines you can set suggested text-width so word break is performed.
        /// </summary>
        [DefaultValue(0), Category("Appearance"), Description("Indicates suggested text-width. If you want to make sure that text you set wraps over multiple lines you can set suggested text-width so word break is performed.")]
        public int TextWidth
        {
            get { return _TextWidth; }
            set
            {
                _TextWidth = value;
                NeedRecalcSize = true;
                Refresh();
            }
        }


        private Size _TextSize = Size.Empty;
        /// <summary>
        /// Recalculate the size of the item. If overridden base implementation must be called so default processing can occur.
        /// </summary>
        public override void RecalcSize()
        {
            m_Rect.Width = _ChartWidth + _Margin.Horizontal + 8;
            m_Rect.Height = _ChartHeight + _Margin.Vertical + 8;

            bool onMenu = this.IsOnMenu && !(this.Parent is ItemContainer);

            if (onMenu)
            {
                Size sideBarSize = GetMaxImageSize();
                // Get the right image size that we will use for calculation
                m_Rect.Width += (sideBarSize.Width + 7);
                if (this.IsOnCustomizeMenu)
                    m_Rect.Width += (sideBarSize.Height + 2);
            }

            if (_TextVisible && !string.IsNullOrEmpty(this.Text))
            {
                Control parent = this.ContainerControl as Control;
                if (parent != null)
                {
                    Font font = parent.Font;
                    using (Graphics g = parent.CreateGraphics())
                    {
                        Size textSize = ButtonItemLayout.MeasureItemText(this, g, _TextWidth, font, (_TextWidth > 0 ? eTextFormat.WordBreak : eTextFormat.SingleLine), parent.RightToLeft == RightToLeft.Yes);
                        _TextSize = textSize;
                        textSize.Width += _TextPadding.Horizontal;
                        textSize.Height += _TextPadding.Vertical;
                        if (_TextPosition == eMicroChartTextPosition.Left || _TextPosition == eMicroChartTextPosition.Right)
                        {
                            textSize.Width += TextChartSpacing;
                            m_Rect.Width += textSize.Width;
                            m_Rect.Height = Math.Max(m_Rect.Height, textSize.Height);
                        }
                        else
                        {
                            textSize.Height += TextChartSpacing;
                            m_Rect.Height += textSize.Height;
                            m_Rect.Width = Math.Max(m_Rect.Width, textSize.Width);
                        }

                    }
                }
            }

            base.RecalcSize();
        }
        private Size GetMaxImageSize()
        {
            if (m_Parent != null)
            {
                ImageItem objParentImageItem = m_Parent as ImageItem;
                if (objParentImageItem != null)
                    return objParentImageItem.SubItemsImageSize;
                else
                    return this.ImageSize;
            }
            else
                return this.ImageSize;
        }

        private string _TooltipValueFormatString = "";
        /// <summary>
        /// Gets or sets the format string for the value when it is displayed as tool-tip for data point.
        /// </summary>
        [DefaultValue(""), Category("Appearance"), Description("Indicates format string for the value when it is displayed as tool-tip for data point.")]
        public string TooltipValueFormatString
        {
            get { return _TooltipValueFormatString; }
            set
            {
                if (value == null) value = string.Empty;
                if (value != _TooltipValueFormatString)
                {
                    string oldValue = _TooltipValueFormatString;
                    _TooltipValueFormatString = value;
                    OnTooltipValueFormatStringChanged(oldValue, value);
                }
            }
        }

        private void OnTooltipValueFormatStringChanged(string oldValue, string newValue)
        {
            //OnPropertyChanged(new PropertyChangedEventArgs("TooltipValueFormatString"));
        }

        private List<double> _DataPoints = new List<double>();
        /// <summary>
        /// Gets or sets the chart data points. Note that if you are adding or removing points directly from this collection you must call
        /// Refresh() method on the control to refresh the display.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<double> DataPoints
        {
            get { return _DataPoints; }
            set
            {
                if (value != _DataPoints)
                {
                    List<double> oldValue = _DataPoints;
                    _DataPoints = value;
                    OnDataPointsChanged(oldValue, value);
                }
            }
        }

        private void OnDataPointsChanged(List<double> oldValue, List<double> newValue)
        {
            //OnPropertyChanged(new PropertyChangedEventArgs("DataPoints"));
            TransitionChart();
        }

        private List<string> _DataPointTooltips = new List<string>();
        /// <summary>
        /// Gets or sets the tooltips for each data-point assigned through DataPoints property. If not set control will automatically 
        /// show tooltip based on the data-point value.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<string> DataPointTooltips
        {
            get { return _DataPointTooltips; }
            set
            {
                if (value != _DataPointTooltips)
                {
                    List<string> oldValue = _DataPointTooltips;
                    _DataPointTooltips = value;
                    OnDataPointTooltipsChanged(oldValue, value);
                }
            }
        }

        private void OnDataPointTooltipsChanged(List<string> oldValue, List<string> newValue)
        {
            //OnPropertyChanged(new PropertyChangedEventArgs("DataPointTooltips"));
        }

        private Image _TransitionImage = null;
        private float _TransitionOpacity = 0f;
        private BackgroundWorker _TransitionWorker = null;
        private void TransitionChart()
        {
            if (_ChartImage != null)
            {
                Control cc = this.ContainerControl as Control;
                if (cc != null && cc.IsHandleCreated && IsAnimationEnabled)
                {
                    Image image = _TransitionImage;
                    _TransitionImage = _ChartImage;
                    if (image != null)
                        image.Dispose();
                    _ChartImage = null;
                    InvalidateChartImage(false);
                    if (_TransitionWorker == null)
                    {
                        _TransitionWorker = new BackgroundWorker();
                        _TransitionWorker.WorkerSupportsCancellation = true;
                        _TransitionWorker.DoWork += new DoWorkEventHandler(TransitionWorkerDoWork);
                        _TransitionWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(TransitionWorkerCompleted);
                        _TransitionWorker.RunWorkerAsync();
                    }
                    return;
                }
            }
            InvalidateChartImage(false);
        }

        void TransitionWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            BackgroundWorker worker = _TransitionWorker;
            Image image = _TransitionImage;
            _TransitionImage = null;
            image.Dispose();

            _TransitionWorker = null;
            worker.Dispose();
            this.Refresh();
        }

        void TransitionWorkerDoWork(object sender, DoWorkEventArgs e)
        {
            while (_TransitionOpacity < 1)
            {
                _TransitionOpacity += 0.1f;
                if (e.Cancel) break;
                this.Refresh();
                System.Threading.Thread.Sleep(60);
            }
            _TransitionOpacity = 0f;
        }

        private bool _AnimationEnabled = true;
        /// <summary>
        /// Gets or sets whether transition animation between same chart with different data-points is enabled. Default value is true.
        /// </summary>
        [DefaultValue(true), Category("Behavior"), Description("Indicates  whether transition animation between same chart with different data-points is enabled.")]
        public bool AnimationEnabled
        {
            get { return _AnimationEnabled; }
            set
            {
                _AnimationEnabled = value;
            }
        }
        /// <summary>
        /// Gets whether fade effect is enabled.
        /// </summary>
        private bool IsAnimationEnabled
        {
            get
            {
                //if (!_AnimationEnabled) return false;

                //System.Windows.Forms.Control cc = this.ContainerControl as System.Windows.Forms.Control;
                //if (cc != null)
                //{
                //    if (cc is ItemControl)
                //        return ((ItemControl)cc).IsFadeEnabled;
                //    else if (cc is MicroChart)
                //        return ((MicroChart)cc).IsAnimationEnabled;
                //    else if (cc is Bar)
                //        return ((Bar)cc).IsFadeEnabled;
                //    else if (cc is ButtonX)
                //        return ((ButtonX)cc).IsFadeEnabled;
                //}
                return _AnimationEnabled;
            }
        }


        private bool _TrackChartPoints = true;
        /// <summary>
        /// Gets or sets whether chart is tracking mouse movement to show data-point and its value on tooltip. Default value is true.
        /// </summary>
        [DefaultValue(true), Category("Behavior"), Description("Indicates whether chart is tracking mouse movement to show data-point and its value on tooltip.")]
        public bool TrackChartPoints
        {
            get { return _TrackChartPoints; }
            set
            {
                _TrackChartPoints = value;
            }
        }

        private bool _MouseOverEnabled = false;
        /// <summary>
        /// Gets or sets whether button like mouse over tracking is enabled for whole control. When enabled control looks like a button when mouse is over it. Default value is false.
        /// </summary>
        [DefaultValue(false), Category("Behavior"), Description("Indicates whether button like mouse over tracking is enabled for whole control. When enabled control looks like a button when mouse is over it.")]
        public bool MouseOverEnabled
        {
            get { return _MouseOverEnabled; }
            set
            {
                _MouseOverEnabled = value;
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
                if (_HotPoint.IsEmpty) return -1;
                return _HotPoint.DataPointIndex;
            }
        }

        private bool _MouseOverDataPointTooltipEnabled = true;
        /// <summary>
        /// Gets or sets whether mouse over tooltip for data point is enabled. Default value is true.
        /// </summary>
        [Category("Behavior"), DefaultValue(true), Description("Indicates whether mouse over tooltip for data point is enabled")]
        public bool MouseOverDataPointTooltipEnabled
        {
            get { return _MouseOverDataPointTooltipEnabled; }
            set
            {
                _MouseOverDataPointTooltipEnabled = value;
            }
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

        private MicroChartHotPoint _HotPoint = MicroChartHotPoint.Empty;
        private MicroChartHotPoint HotPoint
        {
            get
            {
                return _HotPoint;
            }
            set
            {
                if (_HotPoint.HotPointBounds != value.HotPointBounds)
                {
                    _HotPoint = value;
                    if (_HotPoint.IsEmpty && this.ToolTipVisible)
                    {
                        this.HideToolTip();
                        this.Tooltip = "";
                    }
                    else if (_TrackChartPoints && _MouseOverDataPointTooltipEnabled)
                    {
                        if (_DataPointTooltips.Count > 0 && _DataPointTooltips.Count > value.DataPointIndex)
                        {
                            string tooltip = _DataPointTooltips[value.DataPointIndex];
                            if (tooltip.IndexOf("{0}") >= 0)
                                tooltip = string.Format(tooltip, value.Value);
                            this.Tooltip = tooltip;
                        }
                        else if (string.IsNullOrEmpty(_TooltipValueFormatString))
                            this.Tooltip = value.Value.ToString();
                        else
                            this.Tooltip = value.Value.ToString(_TooltipValueFormatString);
                        if (!this.ToolTipVisible) this.ShowToolTip();
                    }
                    OnMouseOverDataPointChanged(EventArgs.Empty);
                    this.Refresh();
                }
            }
        }

        private bool _IsMouseOver = false;
        public override void InternalMouseEnter()
        {
            base.InternalMouseEnter();
            if (!_IsMouseOver)
            {
                _IsMouseOver = true;
                this.Refresh();
            }
        }

        public override void InternalMouseLeave()
        {
            HotPoint = MicroChartHotPoint.Empty;
            if (_IsMouseOver)
            {
                _IsMouseOver = false;
                this.Refresh();
            }
            base.InternalMouseLeave();
        }

        private bool _IsPressed = false;
        public override void InternalMouseDown(MouseEventArgs objArg)
        {
            if (!_IsPressed && objArg.Button == MouseButtons.Left)
            {
                _IsPressed = true;
                this.Refresh();
            }
            base.InternalMouseDown(objArg);
        }

        public override void InternalMouseUp(MouseEventArgs objArg)
        {
            if (_IsPressed && objArg.Button == MouseButtons.Left)
            {
                _IsPressed = false;
                this.Refresh();
            }
            base.InternalMouseUp(objArg);
        }

        public override void InternalMouseMove(MouseEventArgs objArg)
        {
            if (m_Rect.Contains(objArg.X, objArg.Y) && _ChartHotPoints != null)
            {
                MicroChartHotPoint newHotPoint = MicroChartHotPoint.Empty;
                Rectangle chartBounds = new Rectangle(_ChartLocation.X, _ChartLocation.Y, _ChartWidth, _ChartHeight);
                if (chartBounds.Contains(objArg.X, objArg.Y))
                {
                    foreach (MicroChartHotPoint point in _ChartHotPoints)
                    {
                        Point chartPoint = new Point(objArg.X - _ChartLocation.X, objArg.Y - _ChartLocation.Y);
                        Rectangle r = point.HotPointBounds;
                        Rectangle sliceBounds = point.ChartSliceBounds;
                        r.Offset(_ChartLocation);
                        if (!sliceBounds.IsEmpty)
                            sliceBounds.Offset(_ChartLocation);

                        if (_ChartType == eMicroChartType.Pie && PieSliceContains(chartPoint, point) ||
                            _ChartType != eMicroChartType.Pie && (r.Contains(objArg.X, objArg.Y) || sliceBounds.Contains(objArg.X, objArg.Y)))
                        {
                            newHotPoint = point;
                            break;
                        }
                    }
                }

                HotPoint = newHotPoint;
            }
            base.InternalMouseMove(objArg);
        }

        private bool PieSliceContains(Point point, MicroChartHotPoint hotPoint)
        {
            int diameter = Math.Min(_ChartHeight, _ChartWidth);
            double a = diameter / 2 + (_ChartWidth - diameter) / 2;
            double b = diameter / 2 + (_ChartHeight - diameter) / 2; ;
            double x = point.X - a;
            double y = point.Y - b;
            double angle = Math.Atan2(y, x);
            if (angle < 0)
                angle += 2 * Math.PI;
            double angleDegrees = angle * 180 / Math.PI;
            // point is inside the pie slice only if between start and end angle

            if (angleDegrees >= hotPoint.StartAngle &&
                       angleDegrees <= hotPoint.StartAngle + hotPoint.SweepAngle)
            {
                return true;
            }
            return false;
        }


        /// <summary>
        /// Invalidates the chart display and requests the re-paint.
        /// </summary>
        /// <param name="animateTransition">Indicates whether to animate transition to new chart</param>
        public void InvalidateChart(bool animateTransition)
        {
            if (animateTransition)
                TransitionChart();
            else
                InvalidateChartImage(false);
        }

        private void InvalidateChartImage(bool isPainting)
        {
            Image image = _ChartImage;
            _ChartImage = null;
            if (image != null)
                image.Dispose();
            if (!isPainting)
                this.Refresh();
        }

        private Bitmap _ChartImage = null;
        private Point _ChartLocation = Point.Empty;
        /// <summary>
        /// Draws the chart.
        /// </summary>
        /// <param name="p">Paint arguments</param>
        public override void Paint(ItemPaintArgs p)
        {
            if (_DataPoints != null && _LastDataPointCount != _DataPoints.Count)
                InvalidateChartImage(true);

            if (_ChartImage == null)
                _ChartImage = CreateChartImage();

            Graphics g = p.Graphics;

            Rectangle contentBounds = m_Rect;
            bool onMenu = this.IsOnMenu && !(this.Parent is ItemContainer);
            bool rtl = p.RightToLeft;
            eDotNetBarStyle effectiveStyle = this.EffectiveStyle;
            bool enabled = GetEnabled(p.ContainerControl);

            if (onMenu)
            {
                Size sideBarSize = GetMaxImageSize();
                if (!rtl)
                    contentBounds.X += sideBarSize.Width + 7;
                contentBounds.Width -= sideBarSize.Width + 7;
                DrawMenuSideBar(p, effectiveStyle);
            }

            if (_MouseOverEnabled && enabled && (_IsMouseOver || this.Expanded))
            {
                DrawMouseOverState(p, effectiveStyle);
            }

            if (_BackgroundImage != null)
                BarFunctions.PaintBackgroundImage(g, contentBounds, _BackgroundImage, _BackgroundImagePosition, 255); 

            Point chartLocation = new Point(contentBounds.X + 4 + _Margin.Left, contentBounds.Y + 4 + _Margin.Top);

            if (_TextVisible && !string.IsNullOrEmpty(this.Text))
            {
                Rectangle textRect = contentBounds;

                if (_TextPosition == eMicroChartTextPosition.Left && !rtl || _TextPosition == eMicroChartTextPosition.Right && rtl)
                {
                    textRect.Width = _TextSize.Width;
                    textRect.Y += _TextPadding.Top + _Margin.Top + 1;
                    textRect.X += _Margin.Left + _TextPadding.Left + 1;
                    textRect.Height = Math.Max(_TextSize.Height + 2, _ChartHeight + 6);
                    chartLocation.X = textRect.Right + _TextPadding.Right + TextChartSpacing;
                }
                else if (_TextPosition == eMicroChartTextPosition.Right && !rtl || _TextPosition == eMicroChartTextPosition.Left && rtl)
                {
                    textRect.Width = _TextSize.Width;
                    textRect.Y += _TextPadding.Top + _Margin.Top + 1;
                    textRect.X += _Margin.Left + _TextPadding.Left + _ChartWidth + TextChartSpacing + 1;
                    textRect.Height = Math.Max(_TextSize.Height + 2, _ChartHeight + 6);
                }
                else if (_TextPosition == eMicroChartTextPosition.Top)
                {
                    textRect.Width = _TextSize.Width;
                    textRect.Y += _TextPadding.Top + _Margin.Top + 1;
                    textRect.X += _Margin.Left;
                    textRect.Height = _TextSize.Height;
                    chartLocation.Y = textRect.Bottom + _TextPadding.Bottom + TextChartSpacing;
                    if (_TextSize.Width > _ChartWidth)
                        chartLocation.X += (_TextSize.Width - _ChartWidth) / 2;
                    else
                        textRect.X = chartLocation.X + (_ChartWidth - _TextSize.Width) / 2;
                }
                else if (_TextPosition == eMicroChartTextPosition.Bottom)
                {
                    textRect.Width = _TextSize.Width;
                    textRect.Y += _TextPadding.Top + _Margin.Top + _ChartHeight + TextChartSpacing + 1;
                    textRect.X += _Margin.Left;
                    textRect.Height = _TextSize.Height;
                    if (_TextSize.Width > _ChartWidth)
                        chartLocation.X += (_TextSize.Width - _ChartWidth) / 2;
                    else
                        textRect.X = chartLocation.X + (_ChartWidth - _TextSize.Width) / 2;
                }

                Color textColor = this.TextColor;
                if (textColor.IsEmpty)
                {
                    textColor = LabelItem.GetTextColor(p, this.EffectiveStyle, GetEnabled(), textColor);
                }

                Font textFont = p.Font;
                eTextFormat tf = eTextFormat.Left | eTextFormat.VerticalCenter | eTextFormat.WordBreak;
                if (this.TextMarkupBody != null)
                {
                    TextMarkup.MarkupDrawContext d = new TextMarkup.MarkupDrawContext(g, textFont, textColor, rtl);
                    d.HotKeyPrefixVisible = !((tf & eTextFormat.HidePrefix) == eTextFormat.HidePrefix);
                    if ((tf & eTextFormat.VerticalCenter) == eTextFormat.VerticalCenter)
                        textRect.Y = this.TopInternal + (contentBounds.Height - this.TextMarkupBody.Bounds.Height) / 2;
                    else if ((tf & eTextFormat.Bottom) == eTextFormat.Bottom)
                        textRect.Y += (this.TextMarkupBody.Bounds.Height - textRect.Height) + 1;
                    textRect.Height = this.TextMarkupBody.Bounds.Height;
                    this.TextMarkupBody.Bounds = textRect;
                    this.TextMarkupBody.Render(d);
                }
                else
                {
                    if (p != null && p.GlassEnabled && this.Parent is CaptionItemContainer && !(p.ContainerControl is DevComponents.DotNetBar.Ribbon.QatToolbar))
                    {
                        if (!p.CachedPaint)
                            Office2007RibbonControlPainter.PaintTextOnGlass(g, this.Text, textFont, textRect, TextDrawing.GetTextFormat(tf));
                    }
                    else
                        TextDrawing.DrawString(g, this.Text, textFont, textColor, textRect, tf);
                }
            }

            if (_TransitionImage != null)
            {
                Rectangle chartRect = new Rectangle(chartLocation.X, chartLocation.Y, _ChartWidth, _ChartHeight);
                System.Drawing.Imaging.ColorMatrix matrix1 = new System.Drawing.Imaging.ColorMatrix();
                matrix1[3, 3] = (1 - _TransitionOpacity);
                using (System.Drawing.Imaging.ImageAttributes imageAtt = new System.Drawing.Imaging.ImageAttributes())
                {
                    imageAtt.SetColorMatrix(matrix1);
                    g.DrawImage(_TransitionImage, chartRect, 0, 0, chartRect.Width, chartRect.Height, GraphicsUnit.Pixel, imageAtt);
                }
                matrix1[3, 3] = _TransitionOpacity;
                using (System.Drawing.Imaging.ImageAttributes imageAtt = new System.Drawing.Imaging.ImageAttributes())
                {
                    imageAtt.SetColorMatrix(matrix1);
                    g.DrawImage(_ChartImage, chartRect, 0, 0, chartRect.Width, chartRect.Height, GraphicsUnit.Pixel, imageAtt);
                }
            }
            else
            {
                g.DrawImage(_ChartImage, chartLocation);
            }

            _ChartLocation = chartLocation;

            if (_TrackChartPoints && !_HotPoint.IsEmpty)
            {
                Rectangle hotPointBounds = _HotPoint.HotPointBounds;
                hotPointBounds.Offset(chartLocation);
                using (Pen pen = new Pen(Brushes.DarkGray))
                {
                    g.FillEllipse(Brushes.White, hotPointBounds);
                    g.DrawEllipse(pen, hotPointBounds);
                }
                hotPointBounds.Inflate(-2, -2);
                using (SolidBrush brush = new SolidBrush(_HotPoint.Color))
                    g.FillEllipse(brush, hotPointBounds);
            }

            //g.DrawRectangle(Pens.Yellow, new Rectangle(m_Rect.X, m_Rect.Y, m_Rect.Width - 1, m_Rect.Height - 1));

            if (this.Focused && this.DesignMode)
            {
                Rectangle r = m_Rect;
                r.Inflate(-1, -1);
                DesignTime.DrawDesignTimeSelection(g, r, p.Colors.ItemDesignTimeBorder);
            }

            this.DrawInsertMarker(p.Graphics);
        }

        private static RoundRectangleShapeDescriptor _MenuShape = new RoundRectangleShapeDescriptor(3);
        private void DrawMouseOverState(ItemPaintArgs p, eDotNetBarStyle effectiveStyle)
        {
            Rectangle rHover = this.DisplayRectangle;
            rHover.Inflate(-1, 0);
            Graphics g = p.Graphics;

            if (BarFunctions.IsOffice2007Style(effectiveStyle) && !(p.Owner is DotNetBarManager) && GlobalManager.Renderer is Office2007Renderer)
            {
                Office2007ButtonItemStateColorTable bt;
                if (this.Expanded)
                    bt = ((Office2007Renderer)GlobalManager.Renderer).ColorTable.ButtonItemColors[0].Expanded;
                else if (_IsPressed)
                    bt = ((Office2007Renderer)GlobalManager.Renderer).ColorTable.ButtonItemColors[0].Pressed;
                else
                    bt = ((Office2007Renderer)GlobalManager.Renderer).ColorTable.ButtonItemColors[0].MouseOver;
                Office2007ButtonItemPainter.PaintBackground(g, bt, rHover, _MenuShape);
            }
            else
            {
                if (!p.Colors.ItemHotBackground2.IsEmpty)
                {
                    System.Drawing.Drawing2D.LinearGradientBrush gradient = BarFunctions.CreateLinearGradientBrush(rHover, p.Colors.ItemHotBackground, p.Colors.ItemHotBackground2, p.Colors.ItemHotBackgroundGradientAngle);
                    g.FillRectangle(gradient, rHover);
                    gradient.Dispose();
                }
                else
                    g.FillRectangle(new SolidBrush(p.Colors.ItemHotBackground), rHover);
                NativeFunctions.DrawRectangle(g, new Pen(p.Colors.ItemHotBorder), rHover);
            }
        }

        private void DrawMenuSideBar(ItemPaintArgs pa, eDotNetBarStyle effectiveStyle)
        {
            Rectangle sideRect = GetMenuSideBounds(pa.RightToLeft);
            Graphics g = pa.Graphics;

            // Draw side bar
            if (!BarFunctions.IsOffice2007Style(effectiveStyle))
            {
                if (this.MenuVisibility == eMenuVisibility.VisibleIfRecentlyUsed && !this.RecentlyUsed)
                {
                    if (!pa.Colors.MenuUnusedSide2.IsEmpty)
                    {
                        System.Drawing.Drawing2D.LinearGradientBrush gradient = BarFunctions.CreateLinearGradientBrush(new Rectangle(m_Rect.Left, m_Rect.Top, sideRect.Width, m_Rect.Height), pa.Colors.MenuUnusedSide, pa.Colors.MenuUnusedSide2, pa.Colors.MenuUnusedSideGradientAngle);
                        g.FillRectangle(gradient, sideRect);
                        gradient.Dispose();
                    }
                    else
                        g.FillRectangle(new SolidBrush(pa.Colors.MenuUnusedSide), sideRect);
                }
                else
                {
                    if (!pa.Colors.MenuSide2.IsEmpty)
                    {
                        System.Drawing.Drawing2D.LinearGradientBrush gradient = BarFunctions.CreateLinearGradientBrush(sideRect, pa.Colors.MenuSide, pa.Colors.MenuSide2, pa.Colors.MenuSideGradientAngle);
                        g.FillRectangle(gradient, sideRect);
                        gradient.Dispose();
                    }
                    else
                        g.FillRectangle(new SolidBrush(pa.Colors.MenuSide), sideRect);
                }
            }
            if (BarFunctions.IsOffice2007Style(effectiveStyle) && GlobalManager.Renderer is Office2007Renderer)
            {
                Office2007MenuColorTable mt = ((Office2007Renderer)GlobalManager.Renderer).ColorTable.Menu;
                if (mt != null)
                {
                    // Draw side bar
                    if (this.MenuVisibility == eMenuVisibility.VisibleIfRecentlyUsed && !this.RecentlyUsed)
                        DisplayHelp.FillRectangle(g, sideRect, mt.SideUnused);
                    else
                        DisplayHelp.FillRectangle(g, sideRect, mt.Side);
                }

                if (mt != null && !mt.SideBorder.IsEmpty)
                {
                    if (pa.RightToLeft)
                        DisplayHelp.DrawGradientLine(g, sideRect.X, sideRect.Y, sideRect.X, sideRect.Bottom - 1, mt.SideBorder, 1);
                    else
                        DisplayHelp.DrawGradientLine(g, sideRect.Right - 2, sideRect.Y, sideRect.Right - 2, sideRect.Bottom - 1, mt.SideBorder, 1);
                }
                if (mt != null && !mt.SideBorderLight.IsEmpty)
                {
                    if (pa.RightToLeft)
                        DisplayHelp.DrawGradientLine(g, sideRect.X + 1, sideRect.Y, sideRect.X + 1, sideRect.Bottom - 1, mt.SideBorderLight, 1);
                    else
                        DisplayHelp.DrawGradientLine(g, sideRect.Right - 1, sideRect.Y, sideRect.Right - 1, sideRect.Bottom - 1, mt.SideBorderLight, 1);
                }
            }
        }

        private Rectangle GetMenuSideBounds(bool rtl)
        {
            Size sideBarSize = GetMaxImageSize();
            sideBarSize.Width += 7;
            if (this.IsOnCustomizeMenu)
                sideBarSize.Width += sideBarSize.Height + 8;
            Rectangle bounds = m_Rect;
            Rectangle sideRect = new Rectangle(bounds.Left, bounds.Top, sideBarSize.Width, bounds.Height);
            if (rtl)
                sideRect.X = bounds.Right - sideRect.Width;
            return sideRect;
        }

        private MicroChartHotPoint[] _ChartHotPoints = new MicroChartHotPoint[0];
        private int _LastDataPointCount = -1;
        private Bitmap CreateChartImage()
        {
            Bitmap image = new Bitmap(_ChartWidth, _ChartHeight, PixelFormat.Format32bppArgb);

            using (Graphics g = Graphics.FromImage(image))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                MicroChartBase draw = GetMicroChart();
                MicroChartRenderInfo info = new MicroChartRenderInfo(_DataPoints, g, _ChartWidth, _ChartHeight, _DataMaxValue, _DataMinValue);
                draw.CreateChart(info);
                _ChartHotPoints = info.MicroChartHotPoints;
            }

            if (_DataPoints != null)
                _LastDataPointCount = _DataPoints.Count;
            else
                _LastDataPointCount = -1;

            return image;
        }

        private void ChartStyleChanged(object sender, EventArgs e)
        {
            InvalidateChartImage(false);
        }

        private LineMicroChartStyle _LineMicroChartStyle = null;
        /// <summary>
        /// Gets the style used to customize appearance of Line micro-chart.
        /// </summary>
        [Category("Style"), Description("Identifies style used to customize appearance of Line micro-chart."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public LineMicroChartStyle LineChartStyle
        {
            get
            {
                return _LineMicroChartStyle;
            }
        }

        private PlotMicroChartStyle _PlotMicroChartStyle = null;
        /// <summary>
        /// Gets the style used to customize appearance of Plot micro-chart.
        /// </summary>
        [Category("Style"), Description("Identifies style used to customize appearance of Plot micro-chart."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public PlotMicroChartStyle PlotChartStyle
        {
            get
            {
                return _PlotMicroChartStyle;
            }
        }

        private BarMicroChartStyle _ColumnMicroChartStyle = null;
        /// <summary>
        /// Gets the style used to customize appearance of Column micro-chart.
        /// </summary>
        [Category("Style"), Description("Identifies style used to customize appearance of Column micro-chart."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public BarMicroChartStyle ColumnChartStyle
        {
            get
            {
                return _ColumnMicroChartStyle;
            }
        }

        private BarMicroChartStyle _BarMicroChartStyle = null;
        /// <summary>
        /// Gets the style used to customize appearance of Bar micro-chart.
        /// </summary>
        [Category("Style"), Description("Identifies style used to customize appearance of Bar micro-chart."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public BarMicroChartStyle BarChartStyle
        {
            get
            {
                return _BarMicroChartStyle;
            }
        }

        private BarMicroChartStyle _WinLoseMicroChartStyle = null;
        /// <summary>
        /// Gets the style used to customize appearance of Win-Lose micro-chart.
        /// </summary>
        [Category("Style"), Description("Identifies style used to customize appearance of Win-Lose micro-chart."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public BarMicroChartStyle WinLoseChartStyle
        {
            get
            {
                return _WinLoseMicroChartStyle;
            }
        }

        private PieMicroChartStyle _PieMicroChartStyle = null;
        /// <summary>
        /// Gets the style used to customize appearance of Pie micro-chart.
        /// </summary>
        [Category("Style"), Description("Identifies style used to customize appearance of Pie micro-chart."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public PieMicroChartStyle PieChartStyle
        {
            get
            {
                return _PieMicroChartStyle;
            }
        }

        private AreaMicroChartStyle _AreaMicroChartStyle = null;
        /// <summary>
        /// Gets the style used to customize appearance of Area micro-chart.
        /// </summary>
        [Category("Style"), Description("Identifies style used to customize appearance of Area micro-chart."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public AreaMicroChartStyle AreaChartStyle
        {
            get
            {
                return _AreaMicroChartStyle;
            }
        }

        private HundredPctMicroChartStyle _HundredPctMicroChartStyle = null;
        /// <summary>
        /// Gets the style used to customize appearance of 100% micro-chart.
        /// </summary>
        [Category("Style"), Description("Identifies style used to customize appearance of 100% micro-chart."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public HundredPctMicroChartStyle HundredPctChartStyle
        {
            get
            {
                return _HundredPctMicroChartStyle;
            }
        }

        private MicroChartBase GetMicroChart()
        {
            MicroChartBase chart = null;
            if (_ChartType == eMicroChartType.Line)
            {
                LineMicroChart lineChart = new LineMicroChart();
                lineChart.Style = _LineMicroChartStyle;
                chart = lineChart;
            }
            else if (_ChartType == eMicroChartType.Plot)
            {
                PlotMicroChart plotChart = new PlotMicroChart();
                plotChart.Style = _PlotMicroChartStyle;
                chart = plotChart;
            }
            else if (_ChartType == eMicroChartType.Column)
            {
                ColumnMicroChart columnChart = new ColumnMicroChart();
                columnChart.Style = _ColumnMicroChartStyle;
                chart = columnChart;
            }
            else if (_ChartType == eMicroChartType.Bar)
            {
                BarMicroChart barChart = new BarMicroChart();
                barChart.Style = _BarMicroChartStyle;
                chart = barChart;
            }
            else if (_ChartType == eMicroChartType.WinLose)
            {
                WinLoseMicroChart columnChart = new WinLoseMicroChart();
                columnChart.Style = _WinLoseMicroChartStyle;
                chart = columnChart;
            }
            else if (_ChartType == eMicroChartType.Pie)
            {
                PieMicroChart pieChart = new PieMicroChart();
                pieChart.Style = _PieMicroChartStyle;
                chart = pieChart;
            }
            else if (_ChartType == eMicroChartType.Area)
            {
                AreaMicroChart areaChart = new AreaMicroChart();
                areaChart.Style = _AreaMicroChartStyle;
                chart = areaChart;
            }
            else if (_ChartType == eMicroChartType.HundredPercentBar)
            {
                HundredPctBar pctChart = new HundredPctBar();
                pctChart.Style = _HundredPctMicroChartStyle;
                chart = pctChart;
            }
            return chart;
        }

        private int _ChartWidth = 54;
        /// <summary>
        /// Gets or sets the width of the chart part of the control.
        /// </summary>
        [DefaultValue(54), Category("Appearance"), Description("Indicates width of the chart part of the control.")]
        public int ChartWidth
        {
            get { return _ChartWidth; }
            set
            {
                if (value != _ChartWidth)
                {
                    if (_ChartWidth <= 0)
                        throw new ArgumentException("CharWidth must be greater than 0");
                    int oldValue = _ChartWidth;
                    _ChartWidth = value;
                    OnChartWidthChanged(oldValue, value);
                }
            }
        }
        private void OnChartWidthChanged(int oldValue, int newValue)
        {
            //OnPropertyChanged(new PropertyChangedEventArgs("ChartWidth"));
            InvalidateChartImage(false);
        }

        private int _ChartHeight = 15;
        /// <summary>
        /// Gets or sets the height of the chart part of the control.
        /// </summary>
        [DefaultValue(15), Category("Appearance"), Description("Indicates height of the chart part of the control.")]
        public int ChartHeight
        {
            get { return _ChartHeight; }
            set
            {
                if (value != _ChartHeight)
                {
                    if (_ChartWidth <= 0)
                        throw new ArgumentException("CharHeight must be greater than 0");
                    int oldValue = _ChartHeight;
                    _ChartHeight = value;
                    OnChartHeightChanged(oldValue, value);
                }
            }
        }

        private void OnChartHeightChanged(int oldValue, int newValue)
        {
            //OnPropertyChanged(new PropertyChangedEventArgs("ChartHeight"));
            InvalidateChartImage(false);
        }

        private eMicroChartType _ChartType = eMicroChartType.Line;
        /// <summary>
        /// Gets or sets the type of the chart rendered.
        /// </summary>
        [DefaultValue(eMicroChartType.Line), Category("Appearance"), Description("Indicates type of the chart rendered.")]
        public eMicroChartType ChartType
        {
            get { return _ChartType; }
            set
            {
                if (value != _ChartType)
                {
                    eMicroChartType oldValue = _ChartType;
                    _ChartType = value;
                    OnChartTypeChanged(oldValue, value);
                }
            }
        }

        private void OnChartTypeChanged(eMicroChartType oldValue, eMicroChartType newValue)
        {
            //OnPropertyChanged(new PropertyChangedEventArgs("ChartType"));
            InvalidateChartImage(false);
        }

        /// <summary>
        /// Gets whether item supports text markup. Default is false.
        /// </summary>
        protected override bool IsMarkupSupported
        {
            get { return _EnableMarkup; }
        }

        private bool _EnableMarkup = true;
        /// <summary>
        /// Gets or sets whether text-markup support is enabled for items Text property. Default value is true.
        /// Set this property to false to display HTML or other markup in the item instead of it being parsed as text-markup.
        /// </summary>
        [DefaultValue(true), Category("Appearance"), Description("Indicates whether text-markup support is enabled for items Text property.")]
        public bool EnableMarkup
        {
            get { return _EnableMarkup; }
            set
            {
                if (_EnableMarkup != value)
                {
                    _EnableMarkup = value;
                    NeedRecalcSize = true;
                    OnTextChanged();
                }
            }
        }

        private eMicroChartTextPosition _TextPosition = eMicroChartTextPosition.Left;
        /// <summary>
        /// Gets or sets text-position in relation to the chart.
        /// </summary>
        [DefaultValue(eMicroChartTextPosition.Left), Category("Appearance"), Description("Indicates text-position in relation to the chart.")]
        public eMicroChartTextPosition TextPosition
        {
            get { return _TextPosition; }
            set
            {
                if (value != _TextPosition)
                {
                    eMicroChartTextPosition oldValue = _TextPosition;
                    _TextPosition = value;
                    OnTextPositionChanged(oldValue, value);
                }
            }
        }

        private void OnTextPositionChanged(eMicroChartTextPosition oldValue, eMicroChartTextPosition newValue)
        {
            //OnPropertyChanged(new PropertyChangedEventArgs("TextPosition"));
            NeedRecalcSize = true;
            this.Refresh();
        }

        /// <summary>
        /// Gets or sets the text associated with this item.
        /// </summary>
        [System.ComponentModel.Browsable(true), Editor("DevComponents.DotNetBar.Design.TextMarkupUIEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(System.Drawing.Design.UITypeEditor)), System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("The text contained in the item."), System.ComponentModel.Localizable(true), System.ComponentModel.DefaultValue("")]
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
            }
        }

        private bool _TextVisible = true;
        /// <summary>
        /// Gets or sets whether caption/label set using Text property is visible.
        /// </summary>
        [DefaultValue(true), Category("Appearance"), Description("Indicates whether caption/label set using Text property is visible.")]
        public bool TextVisible
        {
            get { return _TextVisible; }
            set
            {
                _TextVisible = value;
                NeedRecalcSize = true;
                this.Refresh();
            }
        }

        private Padding _TextPadding = new Padding(0, 0, 0, 0);
        /// <summary>
        /// Gets or sets text padding.
        /// </summary>
        [Browsable(true), Category("Appearance"), Description("Gets or sets text padding."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Padding TextPadding
        {
            get { return _TextPadding; }
        }
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeTextPadding()
        {
            return _TextPadding.Bottom != 0 || _TextPadding.Top != 0 || _TextPadding.Left != 0 || _TextPadding.Right != 0;
        }
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetTextPadding()
        {
            _TextPadding = new Padding(0, 0, 0, 0);
        }
        private void TextPaddingPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            NeedRecalcSize = true;
            this.Refresh();
        }

        private Padding _Margin = new Padding(0, 0, 0, 0);
        /// <summary>
        /// Gets or sets switch margin.
        /// </summary>
        [Browsable(true), Category("Appearance"), Description("Gets or sets switch margin."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Padding Margin
        {
            get { return _Margin; }
        }
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeMargin()
        {
            return _Margin.Bottom != 0 || _Margin.Top != 0 || _Margin.Left != 0 || _Margin.Right != 0;
        }
        [EditorBrowsable(EditorBrowsableState.Never)]
        private void ResetMargin()
        {
            _Margin = new Padding(0, 0, 0, 0);
        }
        private void MarginPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            NeedRecalcSize = true;
            this.Refresh();
        }

        private Color _TextColor = Color.Empty;
        /// <summary>
        /// Gets or sets the text color.
        /// </summary>
        [Category("Appearance"), Description("Indicates text color.")]
        public Color TextColor
        {
            get { return _TextColor; }
            set { _TextColor = value; }
        }
        /// <summary>
        /// Gets whether property should be serialized.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeTextColor()
        {
            return !_TextColor.IsEmpty;
        }
        /// <summary>
        /// Resets property to its default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetTextColor()
        {
            this.TextColor = Color.Empty;
        }

        private eBackgroundImagePosition _BackgroundImagePosition = eBackgroundImagePosition.Tile;
        /// <summary>
        /// Gets or sets the background image position
        /// </summary>
        [Browsable(true), Category("Appearance"), DefaultValue(eBackgroundImagePosition.Tile), Description("Specifies background image position")]
        public eBackgroundImagePosition BackgroundImagePosition
        {
            get { return _BackgroundImagePosition; }
            set
            {
                if (_BackgroundImagePosition != value)
                {
                    _BackgroundImagePosition = value;
                    this.Refresh();
                }
            }
        }

        private Image _BackgroundImage = null;
        /// <summary>
        /// Gets or sets the background image used by the control.
        /// </summary>
        [Browsable(true), Category("Appearance"), DefaultValue(null), Description("Indicates background image used by the control.")]
        public Image BackgroundImage
        {
            get { return _BackgroundImage; }
            set
            {
                _BackgroundImage = value;
                this.Refresh();
            }
        }

        private double _DataMaxValue = double.NaN;
        /// <summary>
        /// Gets or sets the maximum value for data points. By default maximum data point is calculated based on that data displayed by the chart, but when
        /// two charts need to be scaled the same setting maximum and minimum values for them will ensure that scales are visually the same.
        /// </summary>
        [DefaultValue(double.NaN), Category("Behavior"), Description("Indicates maximum value for data points. By default maximum data point is calculated based on that data displayed by the chart, but when two charts need to be scaled the same setting maximum and minimum values for them will ensure that scales are visually the same.")]
        public double DataMaxValue
        {
            get { return _DataMaxValue; }
            set
            {
                if (value != _DataMaxValue)
                {
                    double oldValue = _DataMaxValue;
                    _DataMaxValue = value;
                    InvalidateChart(false);
                    this.Refresh();
                    OnDataMaxValueChanged(oldValue, value);
                }
            }
        }
        /// <summary>
        /// Called when DataMaxValue property has changed.
        /// </summary>
        /// <param name="oldValue">Old property value</param>
        /// <param name="newValue">New property value</param>
        protected virtual void OnDataMaxValueChanged(double oldValue, double newValue)
        {
            //OnPropertyChanged(new PropertyChangedEventArgs("DataMaxValue"));
        }

        private double _DataMinValue = double.NaN;
        /// <summary>
        /// Gets or sets the minimum value for data points. By default minimum data point is calculated based on that data displayed by the chart, but when
        /// two charts need to be scaled the same setting maximum and minimum values for them will ensure that scales are visually the same.
        /// </summary>
        [DefaultValue(double.NaN), Category("Behavior"), Description("Indicates minimum value for data points. By default minimum data point is calculated based on that data displayed by the chart, but when two charts need to be scaled the same setting maximum and minimum values for them will ensure that scales are visually the same.")]
        public double DataMinValue
        {
            get { return _DataMinValue; }
            set
            {
                if (value != _DataMinValue)
                {
                    double oldValue = _DataMinValue;
                    _DataMinValue = value;
                    InvalidateChart(false);
                    this.Refresh();
                    OnDataMinValueChanged(oldValue, value);
                }
            }
        }
        /// <summary>
        /// Called when DataMinValue property has changed.
        /// </summary>
        /// <param name="oldValue">Old property value</param>
        /// <param name="newValue">New property value</param>
        protected virtual void OnDataMinValueChanged(double oldValue, double newValue)
        {
            //OnPropertyChanged(new PropertyChangedEventArgs("DataMinValue"));
        }
        #endregion

        #region IPersonalizedMenuItem Members
        private eMenuVisibility _MenuVisibility = eMenuVisibility.VisibleAlways;
        /// <summary>
        /// Indicates item's visibility when on pop-up menu.
        /// </summary>
        [DefaultValue(eMenuVisibility.VisibleAlways), Browsable(true), DevCoBrowsable(true), System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("Indicates item's visibility when on pop-up menu.")]
        public eMenuVisibility MenuVisibility
        {
            get
            {
                return _MenuVisibility;
            }
            set
            {
                if (_MenuVisibility != value)
                {
                    _MenuVisibility = value;
                    if (ShouldSyncProperties)
                        BarFunctions.SyncProperty(this, "MenuVisibility");
                }
            }
        }

        private bool _RecentlyUsed = false;
        /// <summary>
        /// Indicates whether item was recently used.
        /// </summary>
        [Browsable(false), DefaultValue(false)]
        public bool RecentlyUsed
        {
            get
            {
                return _RecentlyUsed;
            }
            set
            {
                if (_RecentlyUsed != value)
                {
                    _RecentlyUsed = value;
                    if (ShouldSyncProperties)
                        BarFunctions.SyncProperty(this, "RecentlyUsed");
                }
            }
        }

        #endregion
    }

    /// <summary>
    /// Represents MicroChart hot-points.
    /// </summary>
    internal struct MicroChartHotPoint
    {
        public Rectangle HotPointBounds;
        public Rectangle ChartSliceBounds;
        public Color Color;
        public double Value;
        public static readonly MicroChartHotPoint Empty = new MicroChartHotPoint();
        public int DataPointIndex;

        /// <summary>
        /// Start angle for pie slice.
        /// </summary>
        public double StartAngle;
        /// <summary>
        /// Sweep angle for pie slice.
        /// </summary>
        public double SweepAngle;

        public bool IsEmpty
        {
            get
            {
                return this.HotPointBounds.IsEmpty;
            }
        }
        /// <summary>
        /// Initializes a new instance of the MicroChartHotPoint structure.
        /// </summary>
        /// <param name="hotPoint"></param>
        /// <param name="hotPointColor"></param>
        public MicroChartHotPoint(Rectangle hotPoint, Color hotPointColor, double value, int dataPointIndex)
        {
            HotPointBounds = hotPoint;
            Color = hotPointColor;
            Value = value;
            StartAngle = 0;
            SweepAngle = 0;
            ChartSliceBounds = Rectangle.Empty;
            DataPointIndex = dataPointIndex;
        }

        /// <summary>
        /// Initializes a new instance of the MicroChartHotPoint structure.
        /// </summary>
        /// <param name="hotPointBounds"></param>
        /// <param name="chartSliceBounds"></param>
        /// <param name="color"></param>
        /// <param name="value"></param>
        public MicroChartHotPoint(Rectangle hotPointBounds, Rectangle chartSliceBounds, Color color, double value, int dataPointIndex)
        {
            HotPointBounds = hotPointBounds;
            ChartSliceBounds = chartSliceBounds;
            Color = color;
            Value = value;
            StartAngle = 0;
            SweepAngle = 0;
            DataPointIndex = dataPointIndex;
        }

        /// <summary>
        /// Initializes a new instance of the MicroChartHotPoint structure.
        /// </summary>
        /// <param name="bounds"></param>
        /// <param name="color"></param>
        /// <param name="value"></param>
        /// <param name="startAngle"></param>
        /// <param name="sweepAngle"></param>
        public MicroChartHotPoint(Rectangle bounds, Color color, double value, double startAngle, double sweepAngle, int dataPointIndex)
        {
            HotPointBounds = bounds;
            Color = color;
            Value = value;
            StartAngle = startAngle;
            SweepAngle = sweepAngle;
            ChartSliceBounds = Rectangle.Empty;
            DataPointIndex = dataPointIndex;
        }
    }

    /// <summary>
    /// Defines available MicroChar types.
    /// </summary>
    public enum eMicroChartType
    {
        /// <summary>
        /// Identifies Plot chart.
        /// </summary>
        Plot,
        /// <summary>
        /// Identifies WinLose chart.
        /// </summary>
        WinLose,
        /// <summary>
        /// Identifies Area chart.
        /// </summary>
        Area,
        /// <summary>
        /// Identifies Line chart.
        /// </summary>
        Line,
        /// <summary>
        /// Identifies Column chart.
        /// </summary>
        Column,
        /// <summary>
        /// Identifies Bar chart.
        /// </summary>
        Bar,
        /// <summary>
        /// Identifies Pie chart.
        /// </summary>
        Pie,
        /// <summary>
        /// Identifies 100% bar.
        /// </summary>
        HundredPercentBar
    }


    /// <summary>
    /// Defines micro-chart text position in relation to the chart.
    /// </summary>
    public enum eMicroChartTextPosition
    {
        /// <summary>
        /// Text is positioned to the left of the chart.
        /// </summary>
        Left,
        /// <summary>
        /// Text is positioned to the right of the chart.
        /// </summary>
        Right,
        /// <summary>
        /// Text is positioned on top of the chart.
        /// </summary>
        Top,
        /// <summary>
        /// Text is positioned on bottom of the chart.
        /// </summary>
        Bottom
    }
}
