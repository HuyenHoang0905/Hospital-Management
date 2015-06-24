using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Text;

namespace DevComponents.DotNetBar.Controls
{
    /// <summary>
    /// Analog clock control.
    /// </summary>
    [Description("Analog Clock Control"), ToolboxBitmap(typeof(AnalogClockControl), "AnalogClock.AnalogClockControl.bmp"), DefaultEvent("ValueChanged"), DefaultProperty("Value")]
    public class AnalogClockControl : Control
    {
        #region Private Vars

        private const float _BaseSize = 100.0f;

        private Point _EditLoc;
        private eClockEditStates _EditState;
        private Size _LastSize;
        private Timer _Timer;
        #endregion

        #region Implementation
        /// <summary>
        /// Default minimum size. Defaults to 100, 100.
        /// </summary>
        protected override Size DefaultMinimumSize
        {
            get { return new Size(100, 100); }
        }

        private bool _AntiAlias;
        /// <summary>
        /// Gets or sets whether anti-aliasing is used when rendering the control.  Default value is true.
        /// </summary>
        [DefaultValue(true),
        Category("Appearance"),
        Description("Indicates whether Anti-aliasing is used when rendering the control.")]
        public bool AntiAlias
        {
            get { return _AntiAlias; }
            set
            {
                _AntiAlias = value;
                Invalidate();
            }
        }

        private bool _AutomaticMode;
        /// <summary>
        /// Gets or sets the state for automatic mode.  When true the clock will auto redraw once a second and display the current date/time.  Default value is false.
        /// </summary>
        [DefaultValue(false),
        Category("Behavior"),
        Description("Toggles the state for automatic mode.  When true the clock will auto redraw once a second and display the current date/time.")]
        public bool AutomaticMode
        {
            get { return _AutomaticMode; }
            set
            {
                _AutomaticMode = value;
                if (_AutomaticMode)
                {
                    _Value = DateTime.Now;
                    _IsEditable = false;
                    if (!DesignMode)
                    {
                        if (_Timer != null)
                            _Timer.Dispose();
                        _Timer = new Timer();
                        _Timer.Interval = 1000;
                        _Timer.Tick += Timer_Tick;
                        _Timer.Enabled = true;
                    }
                }
                else
                {
                    if (_Timer != null)
                    {
                        _Timer.Enabled = false;
                        _Timer.Dispose();
                        _Timer = null;
                    }
                }
                Invalidate();
            }
        }

        private eClockStyles _ClockStyle;
        /// <summary>
        /// Gets or sets clock style for this control.
        /// </summary>
        [DefaultValue(eClockStyles.Style1),
        Category("Appearance"),
        Description("The clock style for the control."),
        RefreshProperties(RefreshProperties.All)]
        public eClockStyles ClockStyle
        {
            get { return _ClockStyle; }
            set
            {
                _ClockStyle = value;
                if (_ClockStyle != eClockStyles.Custom)
                {
                    if (_ClockStyleData != null)
                    {
                        _ClockStyleData.Parent = null;
                        _ClockStyleData.Dispose();
                    }
                    _ClockStyleData = new ClockStyleData(_ClockStyle, this);
                }
                Invalidate();
            }
        }

        private ClockStyleData _ClockStyleData;
        /// <summary>
        /// Gets or sets the clock style data elements for this control.
        /// </summary>
        [Category("Appearance"),
        Description("The clock style data for the control."),
        RefreshProperties(RefreshProperties.All)]
        public ClockStyleData ClockStyleData
        {
            get { return _ClockStyleData; }
            set
            {
                if (_ClockStyleData != null) _ClockStyleData.Parent = null;
                _ClockStyleData = value;
                _ClockStyleData.Parent = this;
                _ClockStyleData.Style = eClockStyles.Custom;
                ClockStyle = eClockStyles.Custom;
                Invalidate();
            }
        }
        /// <summary>
        /// Resets the property to default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetClockStyleData()
        {
            ClockStyle = eClockStyles.Style1;
        }

        private eClockIndicatorStyles _IndicatorStyle;
        /// <summary>
        /// Gets or sets a the indicator style the clock control.  Default value is Ticks.
        /// </summary>
        [DefaultValue(eClockIndicatorStyles.Ticks),
        Category("Appearance"),
        Description("Indicates which indicator style the clock will use.")]
        public eClockIndicatorStyles IndicatorStyle
        {
            get { return _IndicatorStyle; }
            set
            {
                _IndicatorStyle = value;
                Invalidate();
            }
        }

        private bool _IsEditable;
        /// <summary>
        /// Gets or sets whether the time can be changed by moving the clock hands. Default value is false. 
        /// </summary>
        [DefaultValue(false),
        Category("Behavior"),
        Description("Indicates whether the time can be changed by moving the clock hands. If AutomaticMode is enabled, this setting is ignored.")]
        public bool IsEditable
        {
            get { return _IsEditable; }
            set
            {
                if (!AutomaticMode)
                    _IsEditable = value;
                Invalidate();
            }
        }

        private bool _ShowGlassOverlay;
        /// <summary>
        /// Gets or sets a value indicating whether to display the glass overlay on the clock control.  Default value is true.
        /// </summary>
        [DefaultValue(true),
        Category("Appearance"),
        Description("Indicates whether the glass overlay on the clock control will be displayed or not.")]
        public bool ShowGlassOverlay
        {
            get { return _ShowGlassOverlay; }
            set
            {
                _ShowGlassOverlay = value;
                Invalidate();
            }
        }

        private bool _ShowSecondHand;
        /// <summary>
        /// Gets or sets a value indicating whether to display the second hand on the clock control.  Default value is true.
        /// </summary>
        [DefaultValue(true),
        Category("Appearance"),
        Description("Indicates whether the second hand on the clock control will be displayed or not.")]
        public bool ShowSecondHand
        {
            get { return _ShowSecondHand; }
            set
            {
                _ShowSecondHand = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Occurs while user is dragging the mouse in order to change time.
        /// </summary>
        [Description("Occurs while user is dragging the mouse in order to change time.")]
        public event TimeValueChangingEventHandler ValueChanging;
        /// <summary>
        /// Raises ValueChanging event.
        /// </summary>
        /// <param name="e">Provides event arguments.</param>
        protected virtual void OnValueChanging(TimeValueChangingEventArgs e)
        {
            TimeValueChangingEventHandler handler = ValueChanging;
            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// Occurs when Value i.e. time clock is displaying has changed.
        /// </summary>
        [Description("Occurs when Value i.e. time clock is displaying has changed.")]
        public event EventHandler ValueChanged;
        /// <summary>
        /// Raises ValueChanged event.
        /// </summary>
        /// <param name="e">Provides event arguments.</param>
        protected virtual void OnValueChanged(EventArgs e)
        {
            EventHandler handler = ValueChanged;
            if (handler != null)
                handler(this, e);
        }

        private DateTime _Value;
        /// <summary>
        /// Gets or sets the current date/time value for this control.
        /// </summary>
        [Category("Behavior"),
        Description("The current date/time value for this control. If AutomaticMode is enabled, assigned values are ignored.")]
        public DateTime Value
        {
            get { return _Value; }
            set
            {
                _Value = value;
                OnValueChanged(EventArgs.Empty);
                Invalidate();
            }
        }
        /// <summary>
        /// Resets the property to default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetValue()
        {
            Value = DateTime.Now;
        }

        ///// <summary>
        ///// Occurs when subproperty value has changed.
        ///// </summary>
        //public event DevComponents.Schedule.Model.SubPropertyChangedEventHandler SubPropertyChanged;

        /// <summary>
        /// Initializes a new instance of the ClockControl class 
        /// </summary>
        public AnalogClockControl()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint |
                    ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw |
                    ControlStyles.SupportsTransparentBackColor, true);

            _AntiAlias = true;
            _AutomaticMode = false;
            _ClockStyle = eClockStyles.Style1;
            _ClockStyleData = new ClockStyleData();
            _ClockStyleData.Parent = this;
            _EditState = eClockEditStates.None;
            _IndicatorStyle = eClockIndicatorStyles.Ticks;
            _IsEditable = false;
            _ShowGlassOverlay = true;
            _ShowSecondHand = true;
            _Value = DateTime.Now;

            MinimumSize = new Size((int)_BaseSize, (int)_BaseSize); ;
            Size = MinimumSize;
        }

        /// <summary>
        /// Releases all resources used by the class. 
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                if (_ClockStyleData != null)
                {
                    _ClockStyleData.Parent = null;
                    _ClockStyleData.Dispose();
                }

                if (_Timer != null)
                {
                    _Timer.Dispose();
                    _Timer = null;
                }
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            RectangleF rect;
            float bezelWidth, angle;

            if (e.Button != MouseButtons.Left || !_IsEditable)
                return;

            bezelWidth = _ClockStyleData.BezelWidth * (ClientRectangle.Width - 1);
            rect = new RectangleF(ClientRectangle.X + bezelWidth,
                                  ClientRectangle.Y + bezelWidth,
                                  (ClientRectangle.Width - 1) - bezelWidth * 2.0f,
                                  (ClientRectangle.Height - 1) - bezelWidth * 2.0f);

            angle = RoundToExactHour((float)_Value.TimeOfDay.TotalHours * 30.0f);
            if (_ClockStyleData.HourHandStyle.ContainsPoint(rect, angle, e.Location))
            {
                _LastMouseMoveHour = _Value.Hour;
                _NewMouseMoveHour = _Value.Hour;
                if (_Value.Hour > 12) _LastMouseMoveHour -= 12;
                _EditState = eClockEditStates.Hour;
            }
            else
            {
                angle = (float)_Value.TimeOfDay.TotalMinutes * 6.0f;
                if (_ClockStyleData.MinuteHandStyle.ContainsPoint(rect, angle, e.Location))
                    _EditState = eClockEditStates.Minute;
                else
                {
                    angle = (float)_Value.TimeOfDay.Seconds * 6.0f;
                    if (_ShowSecondHand && _ClockStyleData.SecondHandStyle.ContainsPoint(rect, angle, e.Location))
                        _EditState = eClockEditStates.Second;
                }
            }
            if (_EditState != eClockEditStates.None)
                _EditLoc = e.Location;
        }

        private int _LastMouseMoveHour = -1;
        private int _NewMouseMoveHour = -1;
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            RectangleF rect;
            float bezelWidth, angle;

            if (!_IsEditable)
                return;

            if (_EditState != eClockEditStates.None)
            {
                _EditLoc = e.Location;

                // We need to record the direction of hour hand movement so AM/PM can be switched
                if (_EditState == eClockEditStates.Hour)
                {
                    int hour = GetHourFromPoint(_EditLoc);
                    if (hour == 1 && _LastMouseMoveHour == 12)
                        _LastMouseMoveHour = 0;
                    else if (hour == 12 && _LastMouseMoveHour == 1)
                        _LastMouseMoveHour = 13;
                    _NewMouseMoveHour += (hour - _LastMouseMoveHour);
                    if (_NewMouseMoveHour < 0)
                        _NewMouseMoveHour = 23;
                    else if (_NewMouseMoveHour > 23)
                        _NewMouseMoveHour = 0;
                    _LastMouseMoveHour = hour;
                    
                }

                OnValueChanging(new TimeValueChangingEventArgs(GetCurrentMouseTime()));

                Invalidate();
            }
            else
            {
                bezelWidth = _ClockStyleData.BezelWidth * (ClientRectangle.Width - 1);
                rect = new RectangleF(ClientRectangle.X + bezelWidth,
                                      ClientRectangle.Y + bezelWidth,
                                      (ClientRectangle.Width - 1) - bezelWidth * 2.0f,
                                      (ClientRectangle.Height - 1) - bezelWidth * 2.0f);

                angle = RoundToExactHour((float)_Value.TimeOfDay.TotalHours * 30.0f);
                if (_ClockStyleData.HourHandStyle.ContainsPoint(rect, angle, e.Location))
                    Cursor = Cursors.Hand;
                else
                {
                    angle = (float)_Value.TimeOfDay.TotalMinutes * 6.0f;
                    if (_ClockStyleData.MinuteHandStyle.ContainsPoint(rect, angle, e.Location))
                        Cursor = Cursors.Hand;
                    else
                    {
                        angle = (float)_Value.TimeOfDay.Seconds * 6.0f;
                        if (_ShowSecondHand && _ClockStyleData.SecondHandStyle.ContainsPoint(rect, angle, e.Location))
                            Cursor = Cursors.Hand;
                        else if (Cursor != Cursors.Default)
                            Cursor = Cursors.Default;
                    }
                }
            }
        }

        private float GetClockAngleFromPoint(Point p)
        {
            float angle = (float)MathHelper.GetDegrees(GetAngleFromPoint(p));
            angle += 90.0f;
            while (angle < 0)
                angle += 360;
            while (angle >= 360)
                angle -= 360;
            return angle;
        }
        private int GetHourFromPoint(Point p)
        {
            float angle = RoundToExactHour(GetClockAngleFromPoint(p));
            int hour = (int)Math.Round(angle / 30.0, 0);
            if (hour == 0) hour = 12;
            return hour;
        }
        private int GetMinuteFromPoint(Point p)
        {
            float angle = GetClockAngleFromPoint(p);
            int minute = (int)Math.Round(angle / 6.0, 0);
            if (minute == 60)
                minute = 0;
            return minute;
        }
        private int GetSecondFromPoint(Point p)
        {
            float angle = GetClockAngleFromPoint(p);
            int minute = (int)Math.Round(angle / 6.0, 0);
            if (minute == 60)
                minute = 0;
            return minute;
        }

        private DateTime GetCurrentMouseTime()
        {
            DateTime time = _Value;
            switch (_EditState)
            {
                case eClockEditStates.Hour:
                    time = new DateTime(_Value.Year, _Value.Month, _Value.Day, _NewMouseMoveHour, _Value.Minute, _Value.Second);
                    break;
                case eClockEditStates.Minute:
                    time = new DateTime(_Value.Year, _Value.Month, _Value.Day, _Value.Hour, GetMinuteFromPoint(_EditLoc), _Value.Second);
                    break;
                case eClockEditStates.Second:
                    time = new DateTime(_Value.Year, _Value.Month, _Value.Day, _Value.Hour, _Value.Minute, GetSecondFromPoint(_EditLoc));
                    break;
            }

            return time;
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (_EditState != eClockEditStates.None)
            {
                _EditLoc = e.Location;
                Value = GetCurrentMouseTime();
                _EditLoc = Point.Empty;
                _EditState = eClockEditStates.None;
                Cursor = Cursors.Default;
            }
            base.OnMouseUp(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g;
            RectangleF rect;
            float bezelWidth;

            g = e.Graphics;

            TextRenderingHint textHint = g.TextRenderingHint;
            SmoothingMode sm = g.SmoothingMode;
            if (_AntiAlias)
            {
                g.TextRenderingHint = BarUtilities.AntiAliasTextRenderingHint;
                g.SmoothingMode = SmoothingMode.AntiAlias;
            }

            bezelWidth = _ClockStyleData.BezelWidth * (ClientRectangle.Width - 1);
            rect = new RectangleF(ClientRectangle.X + bezelWidth,
                                  ClientRectangle.Y + bezelWidth,
                                  (ClientRectangle.Width - 1) - bezelWidth * 2.0f,
                                  (ClientRectangle.Height - 1) - bezelWidth * 2.0f);


            e.Graphics.TranslateTransform(rect.X + rect.Width * 0.5f, rect.Y + rect.Height * 0.5f);
            switch (_IndicatorStyle)
            {
                case eClockIndicatorStyles.Ticks:
                    DrawTicks(g, rect);
                    break;
                case eClockIndicatorStyles.Numbers:
                    DrawNumbers(g, rect);
                    break;
            }

            DrawHands(g, rect, false);
            DrawCap(g, rect);
            DrawHands(g, rect, true);

            if (_ShowGlassOverlay)
                DrawGlassOverlay(g, rect);

            g.TextRenderingHint = textHint;
            g.SmoothingMode = sm;
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            base.OnPaintBackground(pevent);
            Graphics gfx;
            RectangleF innerRect, outerRect;
            GraphicsPath outerPath, innerPath;
            Brush brush;
            Pen pen;
            RectangleF imageRect = new RectangleF();
            float aspect, bezelWidth;
            float scaleFactor;

            gfx = pevent.Graphics;
            gfx.SmoothingMode = _AntiAlias ? SmoothingMode.AntiAlias : SmoothingMode.Default;

            Rectangle clientRectangle = ClientRectangle;
            if (_ClockStyleData.BezelColor.BorderWidth > 0)
            {
                int borderInflateSize = (int)((_ClockStyleData.BezelColor.BorderWidth * Math.Min(clientRectangle.Width, clientRectangle.Height)) / 2);
                clientRectangle.Inflate(-borderInflateSize, -borderInflateSize);
            }

            scaleFactor = Math.Min(clientRectangle.Width, clientRectangle.Height);
            bezelWidth = _ClockStyleData.BezelWidth * (scaleFactor - 1);

            innerRect = new RectangleF(clientRectangle.X + bezelWidth,
                                       clientRectangle.Y + bezelWidth,
                                       (clientRectangle.Width - 1) - bezelWidth * 2.0f,
                                       (clientRectangle.Height - 1) - bezelWidth * 2.0f);
            outerRect = new RectangleF(clientRectangle.X,
                                       clientRectangle.Y,
                                       clientRectangle.Width - 1,
                                       clientRectangle.Height - 1);

            outerPath = new GraphicsPath();
            innerPath = new GraphicsPath();

            switch (_ClockStyleData.ClockShape)
            {
                case eClockShapes.Round:
                    outerPath.AddEllipse(outerRect);
                    outerPath.CloseAllFigures();
                    innerPath.AddEllipse(innerRect);
                    innerPath.CloseAllFigures();
                    break;
                default:
                    outerPath.AddRectangle(outerRect);
                    innerPath.AddRectangle(innerRect);
                    break;
            }


            if (_ClockStyleData.FaceBackgroundImage != null)
            {
                gfx.SetClip(outerPath);
                if (_ClockStyleData.FaceBackgroundImage.Width < _ClockStyleData.FaceBackgroundImage.Height)
                    aspect = (float)innerRect.Width / (float)_ClockStyleData.FaceBackgroundImage.Width;
                else
                    aspect = (float)innerRect.Height / (float)_ClockStyleData.FaceBackgroundImage.Height;
                imageRect.X = innerRect.X + ((innerRect.Width / 2.0f) - (aspect * _ClockStyleData.FaceBackgroundImage.Width) / 2.0f);
                imageRect.Y = innerRect.Y + ((innerRect.Height / 2.0f) - (aspect * _ClockStyleData.FaceBackgroundImage.Height) / 2.0f);
                imageRect.Width = aspect * _ClockStyleData.FaceBackgroundImage.Width;
                imageRect.Height = aspect * _ClockStyleData.FaceBackgroundImage.Height;
                gfx.DrawImage(_ClockStyleData.FaceBackgroundImage, imageRect);

            }

            gfx.ResetClip();
            gfx.SetClip(innerPath, CombineMode.Exclude);
            brush = _ClockStyleData.BezelColor.GetBrush(outerPath);
            gfx.FillPath(brush, outerPath);
            brush.Dispose();
            if (_ClockStyleData.BezelColor.BorderWidth > 0)
            {
                pen = _ClockStyleData.BezelColor.GetBorderPen(scaleFactor, PenAlignment.Center);
                gfx.DrawPath(pen, outerPath);
                pen.Dispose();
            }

            gfx.ResetClip();
            if (_ClockStyleData.FaceBackgroundImage == null)
            {
                brush = _ClockStyleData.FaceColor.GetBrush(innerPath, new PointF(0.50f, 0.50f));
                gfx.FillPath(brush, innerPath);
                brush.Dispose();
            }
            if (_ClockStyleData.FaceColor.BorderWidth > 0)
            {
                pen = _ClockStyleData.FaceColor.GetBorderPen(scaleFactor, PenAlignment.Center);
                gfx.DrawPath(pen, innerPath);
                pen.Dispose();
            }
            innerPath.Dispose();
            outerPath.Dispose();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (Width < 100)
            {
                Width = 100;
                Height = 100;
            }
            else if (_LastSize.Width != Width)
                Height = Width;
            else
                Width = Height;

            _LastSize = Size;
        }

        /// <summary>
        /// Renders the clock's center cap.
        /// </summary>
        /// <param name="gfx">Graphics object used for rendering.</param>
        /// <param name="rect">Bounding rectangle.</param>
        protected virtual void DrawCap(Graphics gfx, RectangleF rect)
        {
            float scaleFactor;
            GraphicsPath path;
            RectangleF capRect;
            Pen pen;
            Brush brush;
            float capSize;
            scaleFactor = Math.Min(rect.Width, rect.Height);

            capSize = _ClockStyleData.CapSize * scaleFactor;
            capRect = new RectangleF(-capSize / 2.0f, -capSize / 2.0f, capSize, capSize);
            path = new GraphicsPath();
            path.AddEllipse(capRect);

            brush = _ClockStyleData.CapColor.GetBrush(path);
            gfx.FillPath(brush, path);
            if (_ClockStyleData.CapColor.BorderWidth > 0)
            {
                pen = _ClockStyleData.CapColor.GetBorderPen(scaleFactor, PenAlignment.Outset);
                gfx.DrawPath(pen, path);
                pen.Dispose();
            }
            brush.Dispose();
            path.Dispose();
        }

        /// <summary>
        /// Renders the clock's glass overlay.
        /// </summary>
        /// <param name="gfx">Graphics object used for rendering.</param>
        /// <param name="rect">Bounding rectangle.</param>
        protected virtual void DrawGlassOverlay(Graphics gfx, RectangleF rect)
        {
            GraphicsState gState;
            PathGradientBrush brush;
            GraphicsPath path, brushPath;
            PointF[] curvePoints;
            float radius;

            gState = gfx.Save();
            rect.Width *= 0.95f;
            rect.Height *= 0.95f;
            rect.X = -rect.Width / 2.0f;
            rect.Y = -rect.Height / 2.0f;
            radius = Math.Min(rect.Width, rect.Height) / 2.0f;

            brushPath = new GraphicsPath();
            brushPath.AddEllipse(rect);
            brush = new PathGradientBrush(brushPath);
            brush.CenterPoint = new PointF(0.0f, 0.0f);
            brush.CenterColor = Color.FromArgb(192, Color.White);
            brush.SurroundColors = new Color[] { Color.FromArgb(64, Color.White) };

            path = new GraphicsPath();
            path.AddArc(rect, 180, 180);
            curvePoints = new PointF[5];
            curvePoints[0].X = -radius;
            curvePoints[0].Y = 0.0f;

            curvePoints[1].X = -radius * 0.5f;
            curvePoints[1].Y = -radius * 0.175f;

            curvePoints[2].X = 0.0f;
            curvePoints[2].Y = -radius * 0.25f;

            curvePoints[3].X = radius * 0.5f;
            curvePoints[3].Y = -radius * 0.175f;

            curvePoints[4].X = radius;
            curvePoints[4].Y = 0.0f;

            path.AddCurve(curvePoints);
            path.CloseAllFigures();

            gfx.RotateTransform(_ClockStyleData.GlassAngle);
            gfx.FillPath(brush, path);

            gfx.Restore(gState);
            brush.Dispose();
            brushPath.Dispose();
            path.Dispose();

        }

        private static float RoundToExactHour(float angle)
        {
            return (float)Math.Floor((angle / 30f)) * 30f;
        }
        /// <summary>
        /// Renders the clock's hands.
        /// </summary>
        /// <param name="gfx">Graphics object used for rendering.</param>
        /// <param name="rect">Bounding rectangle.</param>
        /// <param name="overCap">True if this is the rending pass after the cap has been rendered.</param>
        protected virtual void DrawHands(Graphics gfx, RectangleF rect, bool overCap)
        {
            GraphicsState gState;
            GraphicsPath path;
            Pen pen;
            Brush brush;
            float scaleFactor, angle;

            scaleFactor = Math.Min(rect.Width, rect.Height);
            gState = gfx.Save();
            gfx.ResetTransform();

            //Hour Hand
            if (_ClockStyleData.HourHandStyle.DrawOverCap == overCap)
            {
                if (_EditState == eClockEditStates.Hour)
                    angle = (float)MathHelper.GetDegrees(GetAngleFromPoint(_EditLoc)) + 90.0f;
                else
                    angle = (float)(_Value.TimeOfDay.TotalHours > 12 ? _Value.TimeOfDay.TotalHours - 12 : _Value.TimeOfDay.TotalHours) * 30.0f;

                // When clock is editable round up the angle to point at the hour exactly
                if (_IsEditable && !_AutomaticMode)
                {
                    angle = RoundToExactHour(angle);
                }
                path = _ClockStyleData.HourHandStyle.GenerateHandPath(rect, angle);
                brush = _ClockStyleData.HourHandStyle.HandColor.GetBrush(path, _ClockStyleData.HourHandStyle.HandColor.BrushAngle + angle);
                gfx.FillPath(brush, path);
                if (_ClockStyleData.HourHandStyle.HandColor.BorderWidth > 0)
                {
                    pen = _ClockStyleData.HourHandStyle.HandColor.GetBorderPen(scaleFactor, PenAlignment.Outset);
                    gfx.DrawPath(pen, path);
                    pen.Dispose();
                }
                brush.Dispose();
                path.Dispose();
            }

            //Minute Hand
            if (_ClockStyleData.MinuteHandStyle.DrawOverCap == overCap)
            {
                if (_EditState == eClockEditStates.Minute)
                    angle = (float)MathHelper.GetDegrees(GetAngleFromPoint(_EditLoc)) + 90.0f;
                else
                    angle = (float)_Value.TimeOfDay.TotalMinutes * 6.0f;
                path = _ClockStyleData.MinuteHandStyle.GenerateHandPath(rect, angle);
                brush = _ClockStyleData.MinuteHandStyle.HandColor.GetBrush(path, _ClockStyleData.MinuteHandStyle.HandColor.BrushAngle + angle);
                gfx.FillPath(brush, path);
                if (_ClockStyleData.MinuteHandStyle.HandColor.BorderWidth > 0)
                {
                    pen = _ClockStyleData.MinuteHandStyle.HandColor.GetBorderPen(scaleFactor, PenAlignment.Outset);
                    gfx.DrawPath(pen, path);
                    pen.Dispose();
                }
                brush.Dispose();
                path.Dispose();
            }

            //Second Hand
            if (_ShowSecondHand && _ClockStyleData.SecondHandStyle.DrawOverCap == overCap)
            {
                if (_EditState == eClockEditStates.Second)
                    angle = (float)MathHelper.GetDegrees(GetAngleFromPoint(_EditLoc)) + 90.0f;
                else
                    angle = (float)_Value.TimeOfDay.Seconds * 6.0f;
                path = _ClockStyleData.SecondHandStyle.GenerateHandPath(rect, angle);
                brush = _ClockStyleData.SecondHandStyle.HandColor.GetBrush(path, _ClockStyleData.SecondHandStyle.HandColor.BrushAngle + angle);
                gfx.FillPath(brush, path);
                if (_ClockStyleData.SecondHandStyle.HandColor.BorderWidth > 0)
                {
                    pen = _ClockStyleData.SecondHandStyle.HandColor.GetBorderPen(scaleFactor, PenAlignment.Outset);
                    gfx.DrawPath(pen, path);
                    pen.Dispose();
                }
                brush.Dispose();
                path.Dispose();
            }
            gfx.Restore(gState);
        }

        /// <summary>
        /// Renders the clock's numeric hour indicators.
        /// </summary>
        /// <param name="gfx">Graphics object used for rendering.</param>
        /// <param name="rect">Bounding rectangle.</param>
        protected virtual void DrawNumbers(Graphics gfx, RectangleF rect)
        {
            PointF center, txtCenter;
            float scaleFactor, radius, tickIncrement, angle;
            Brush brush;
            StringFormat strFormat;
            Font fnt;

            strFormat = new StringFormat();
            strFormat.Alignment = StringAlignment.Center;
            strFormat.LineAlignment = StringAlignment.Center;

            scaleFactor = Math.Min(rect.Width, rect.Height) / _BaseSize;
            fnt = new Font(_ClockStyleData.NumberFont.FontFamily, _ClockStyleData.NumberFont.Size * scaleFactor, _ClockStyleData.NumberFont.Style, GraphicsUnit.Pixel);
            center = new PointF(rect.X + rect.Width / 2.0f, rect.Y + rect.Height / 2.0f);
            radius = (rect.Width / 2.0f) - (gfx.MeasureString("12", fnt).Height) / 1.25f;
            tickIncrement = (float)MathHelper.GetRadians(30.0f);
            angle = (float)MathHelper.GetRadians(-60.0f);
            brush = new SolidBrush(_ClockStyleData.NumberColor);

            txtCenter = new PointF();
            for (int i = 1; i <= 12; i++)
            {
                txtCenter.X = (float)(radius * Math.Cos(angle));
                txtCenter.Y = (float)(radius * Math.Sin(angle));
                gfx.DrawString(i.ToString(), fnt, brush, txtCenter, strFormat);
                angle += tickIncrement;
            }

            brush.Dispose();
            fnt.Dispose();
        }

        /// <summary>
        /// Renders the clock's tick hour/minute indicators.
        /// </summary>
        /// <param name="gfx">Graphics object used for rendering.</param>
        /// <param name="rect">Bounding rectangle.</param>
        protected virtual void DrawTicks(Graphics gfx, RectangleF rect)
        {
            float scaleFactor;
            PointF[] largePts, smallPts;
            GraphicsState gState;
            Brush largeBrush, smallBrush;
            Pen largePen = null, smallPen = null;
            GraphicsPath largePath, smallPath;

            scaleFactor = Math.Min(rect.Width, rect.Height);

            largePts = new PointF[4];
            largePts[0].X = (-_ClockStyleData.LargeTickWidth * 0.5f) * scaleFactor;
            largePts[0].Y = -0.45f * scaleFactor;

            largePts[1].X = (-_ClockStyleData.LargeTickWidth * 0.5f) * scaleFactor;
            largePts[1].Y = (-0.45f + _ClockStyleData.LargeTickLength) * scaleFactor;

            largePts[2].X = (_ClockStyleData.LargeTickWidth * 0.5f) * scaleFactor;
            largePts[2].Y = (-0.45f + _ClockStyleData.LargeTickLength) * scaleFactor;

            largePts[3].X = (_ClockStyleData.LargeTickWidth * 0.5f) * scaleFactor;
            largePts[3].Y = -0.45f * scaleFactor;

            smallPts = new PointF[4];
            smallPts[0].X = (-_ClockStyleData.SmallTickWidth * -0.5f) * scaleFactor;
            smallPts[0].Y = -0.45f * scaleFactor;

            smallPts[1].X = (-_ClockStyleData.SmallTickWidth * -0.5f) * scaleFactor;
            smallPts[1].Y = (-0.45f + _ClockStyleData.SmallTickLength) * scaleFactor;

            smallPts[2].X = (_ClockStyleData.SmallTickWidth * -0.5f) * scaleFactor;
            smallPts[2].Y = (-0.45f + _ClockStyleData.SmallTickLength) * scaleFactor;

            smallPts[3].X = (_ClockStyleData.SmallTickWidth * -0.5f) * scaleFactor;
            smallPts[3].Y = -0.45f * scaleFactor;

            largePath = new GraphicsPath();
            largePath.AddPolygon(largePts);

            smallPath = new GraphicsPath();
            smallPath.AddPolygon(smallPts);

            largeBrush = _ClockStyleData.LargeTickColor.GetBrush(largePath);
            if (_ClockStyleData.LargeTickColor.BorderWidth > 0)
                largePen = _ClockStyleData.LargeTickColor.GetBorderPen(scaleFactor, PenAlignment.Center);

            smallBrush = _ClockStyleData.LargeTickColor.GetBrush(largePath);
            if (_ClockStyleData.SmallTickColor.BorderWidth > 0)
                smallPen = _ClockStyleData.SmallTickColor.GetBorderPen(scaleFactor, PenAlignment.Center);

            gState = gfx.Save();
            for (int i = 0; i < 12; i++)
            {
                gfx.FillPolygon(largeBrush, largePts);
                if (largePen != null)
                    gfx.DrawPolygon(largePen, largePts);
                for (int j = 0; j < 4; j++)
                {
                    gfx.RotateTransform(6);
                    gfx.FillPolygon(smallBrush, smallPts);
                    if (smallPen != null)
                        gfx.DrawPolygon(smallPen, smallPts);
                }
                gfx.RotateTransform(6);
            }
            gfx.Restore(gState);

            largePath.Dispose();
            smallPath.Dispose();
            largeBrush.Dispose();
            smallBrush.Dispose();
            if (largePen != null)
                largePen.Dispose();
            if (smallPen != null)
                smallPen.Dispose();
        }

        private float GetAngleFromPoint(PointF pt)
        {
            float angle;
            PointF center = new PointF(ClientRectangle.X + ClientRectangle.Width / 2.0f, ClientRectangle.Y + ClientRectangle.Height / 2.0f);
            pt.X -= center.X;
            pt.Y -= center.Y;
            angle = (float)Math.Atan2((double)(pt.Y), (double)(pt.X));
            return angle;
        }

        private void UpdateAutomaticTime()
        {
            if (_TimeZoneInfo != null)
                Value = DevComponents.Schedule.TimeZoneInfo.ConvertTime(DateTime.Now, _TimeZoneInfo);
            else
                Value = DateTime.Now;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            UpdateAutomaticTime();
        }

        private DevComponents.Schedule.TimeZoneInfo _TimeZoneInfo = null;
        private string _TimeZone = string.Empty;
        /// <summary>
        /// Gets or sets the time-zone string identifier that is used to display the time when AutomaticMode=true and clock is displaying current time.
        /// </summary>
        [DefaultValue(""), Editor("DevComponents.DotNetBar.Design.TimeZoneSelectionEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(System.Drawing.Design.UITypeEditor)), Category("Behavior"), Description("Indicates time-zone string identifier that is used to display the time when AutomaticMode=true and clock is displaying current time.")]
        public string TimeZone
        {
            get { return _TimeZone; }
            set
            {
                if (value != _TimeZone)
                {
                    DevComponents.Schedule.TimeZoneInfo timeZoneInfo = null;
                    if (!string.IsNullOrEmpty(value))
                        timeZoneInfo = DevComponents.Schedule.TimeZoneInfo.FindSystemTimeZoneById(value);
                    if (timeZoneInfo != null || string.IsNullOrEmpty(value))
                    {
                        string oldValue = _TimeZone;
                        _TimeZone = value;
                        _TimeZoneInfo = timeZoneInfo;
                        OnTimeZoneChanged(oldValue, value);
                    }
                }
            }
        }
        /// <summary>
        /// Called when TimeZone property has changed.
        /// </summary>
        /// <param name="oldValue">Old property value</param>
        /// <param name="newValue">New property value</param>
        protected virtual void OnTimeZoneChanged(string oldValue, string newValue)
        {
            if (_AutomaticMode)
                UpdateAutomaticTime();
            //OnPropertyChanged(new PropertyChangedEventArgs("TimeZone"));
        }

        ///// <summary>
        ///// Raises the SubPropertyChanged event.
        ///// </summary>
        ///// <param name="e">Event arguments</param>
        //public void OnSubPropertyChanged(DevComponents.Schedule.Model.SubPropertyChangedEventArgs e)
        //{
        //    if (SubPropertyChanged != null)
        //        SubPropertyChanged(this, e);
        //    Invalidate();
        //}

        #endregion
    }

    /// <summary>
    /// Enumeration containing the available hour/minute indicators.
    /// </summary>
    public enum eClockIndicatorStyles
    {
        /// <summary>
        /// Control will use ticks for hour/minute indicators.
        /// </summary>
        Ticks,

        /// <summary>
        /// Control will use numbers for hour indicators.
        /// </summary>
        Numbers
    }

    /// <summary>
    /// Enumeration containing the available mouse edit states.
    /// </summary>
    public enum eClockEditStates
    {
        /// <summary>
        /// Control is not currently in an edit state.
        /// </summary>
        None,

        /// <summary>
        /// Control is currently in an hour edit state.
        /// </summary>
        Hour,

        /// <summary>
        /// Control is currently in an minute edit state.
        /// </summary>
        Minute,

        /// <summary>
        /// Control is currently in an second edit state.
        /// </summary>
        Second
    }

    /// <summary>
    /// Provides event arguments for TimeValueChanging event.
    /// </summary>
    public class TimeValueChangingEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the current time represented by the control.
        /// </summary>
        public readonly DateTime Time;
        /// <summary>
        /// Initializes a new instance of the TimeValueChangingEventArgs class.
        /// </summary>
        /// <param name="time"></param>
        public TimeValueChangingEventArgs(DateTime time)
        {
            Time = time;
        }

    }
    /// <summary>
    /// Defines delegate for TimeValueChanging event.
    /// </summary>
    /// <param name="sender">Source of event.</param>
    /// <param name="e">Event arguments</param>
    public delegate void TimeValueChangingEventHandler(object sender, TimeValueChangingEventArgs e);
}
