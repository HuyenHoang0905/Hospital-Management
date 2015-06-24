using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace DevComponents.Instrumentation.Design
{
    [ToolboxItem(false)]
    public partial class PivotPointDropDown : UserControl
    {
        #region Constants

        private const int DotRadius = 4;
        private const int ScaleWidth = 3;
        private const float BevelInside = .035f;
        private const float BevelOutside = .05f;
        private const float RoundRectangleArc = .125f;

        #endregion

        #region Private variables

        private GaugeFrameStyle _Style;

        private PointF _PivotPoint;
        private Rectangle _DotBounds;
        private Rectangle _FrameBounds;
        private Rectangle _ScaleBounds;

        private float _ScaleRadius;
        private float _StartAngle;
        private float _SweepAngle;

        private bool _InFrame;
        private bool _InPivotDot;
        private bool _PivotMoving;

        private bool _EscapePressed;

        private IWindowsFormsEditorService _EditorService;
        private ITypeDescriptorContext _Context;

        #endregion

        public PivotPointDropDown(PointF value, GaugeFrameStyle style,
            float scaleRadius, float startAngle, float sweepAngle,
            IWindowsFormsEditorService editorService, ITypeDescriptorContext context)
        {
            _Style = style;
            _ScaleRadius = scaleRadius;
            _StartAngle = startAngle;
            _SweepAngle = sweepAngle;

            _ScaleRadius = Math.Max(_ScaleRadius, .07f);

            Initialize();

            _EditorService = editorService;
            _Context = context;

            PivotPoint = value;
        }

        public PivotPointDropDown()
        {
            Initialize();
        }

        #region Initialize

        private void Initialize()
        {
            InitializeComponent();

            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        #endregion

        #region Public properties

        #region EditorService

        public IWindowsFormsEditorService EditorService
        {
            get { return (_EditorService); }
            set { _EditorService = value; }
        }

        #endregion

        #region EscapePressed

        public bool EscapePressed
        {
            get { return (_EscapePressed); }
            set { _EscapePressed = value; }
        }

        #endregion

        #region PivotPoint

        public PointF PivotPoint
        {
            get { return (_PivotPoint); }

            set
            {
                _PivotPoint = value;

                RecalcLayout();
                Invalidate();

                _Context.OnComponentChanging();
                _Context.PropertyDescriptor.SetValue(_Context.Instance, value);
                _Context.OnComponentChanged();
            }
        }

        #endregion

        #endregion

        #region RecalcLayout

        private void RecalcLayout()
        {
            int n = Math.Min(Bounds.Width - 4, Bounds.Height - 4);

            _FrameBounds = new Rectangle(2, 2, n, n);
            _FrameBounds.X = (Bounds.Width - n) / 2;
            _FrameBounds.Y = (Bounds.Height - n) / 2;

            int radius = (int)(n * _ScaleRadius);

            _ScaleBounds.Width = radius * 2;
            _ScaleBounds.Height = radius * 2;

            int x = _FrameBounds.X + (int)(_PivotPoint.X * _FrameBounds.Width);
            int y = _FrameBounds.Y + (int)(_PivotPoint.Y * _FrameBounds.Height);

            _ScaleBounds.X = x - radius;
            _ScaleBounds.Y = y - radius;

            _DotBounds = new Rectangle(x - DotRadius, y - DotRadius, DotRadius * 2, DotRadius * 2);
        }

        #endregion

        #region Paint support

        #region OnPaint

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;

            g.SmoothingMode = SmoothingMode.AntiAlias;

            RecalcLayout();

            DrawFrame(g);
            DrawScale(g);
            DrawPivotDot(g);
        }

        #endregion

        #region DrawFrame

        private void DrawFrame(Graphics g)
        {
            switch (_Style)
            {
                case GaugeFrameStyle.Circular:
                    DrawCircularFrame(g);
                    break;

                case GaugeFrameStyle.Rectangular:
                    DrawRectangularFrame(g);
                    break;

                case GaugeFrameStyle.RoundedRectangular:
                    DrawRoundedRectFrame(g);
                    break;

                default:
                    DrawBlankFrame(g);
                    break;
            }
        }

        #region DrawCircularFrame

        private void DrawCircularFrame(Graphics g)
        {
            using (LinearGradientBrush br = new LinearGradientBrush(_FrameBounds, Color.WhiteSmoke, Color.Gray, 45))
                g.FillEllipse(br, _FrameBounds);

            int bevelIn = (int)(_FrameBounds.Width * BevelInside);
            int bevelOut = (int)(_FrameBounds.Width * BevelOutside);

            Rectangle r = _FrameBounds;
            r.Inflate(-bevelOut, -bevelOut);

            using (LinearGradientBrush br = new LinearGradientBrush(r, Color.Silver, Color.Gray, 45 + 180))
                g.FillEllipse(br, r);

            r.Inflate(-bevelIn, -bevelIn);

            using (LinearGradientBrush br = new LinearGradientBrush(r, Color.Silver, Color.LightGray, 45))
                g.FillEllipse(br, r);
        }

        #endregion

        #region DrawRectangularFrame

        private void DrawRectangularFrame(Graphics g)
        {
            using (LinearGradientBrush br = new LinearGradientBrush(_FrameBounds, Color.WhiteSmoke, Color.Gray, 45))
                g.FillRectangle(br, _FrameBounds);

            int bevelIn = (int)(_FrameBounds.Width * BevelInside);
            int bevelOut = (int)(_FrameBounds.Width * BevelOutside);

            Rectangle r = _FrameBounds;
            r.Inflate(-bevelOut, -bevelOut);

            using (LinearGradientBrush br = new LinearGradientBrush(r, Color.Silver, Color.Gray, 45 + 180))
                g.FillRectangle(br, r);

            r.Inflate(-bevelIn, -bevelIn);

            using (LinearGradientBrush br = new LinearGradientBrush(r, Color.Silver, Color.LightGray, 45))
                g.FillRectangle(br, r);
        }

        #endregion

        #region DrawRoundedRectFrame

        private void DrawRoundedRectFrame(Graphics g)
        {
            using (GraphicsPath path = GetRoundRectPath(_FrameBounds))
            {
                using (LinearGradientBrush br = new LinearGradientBrush(_FrameBounds, Color.WhiteSmoke, Color.Gray, 45))
                    g.FillPath(br, path);
            }

            int bevelIn = (int)(_FrameBounds.Width * BevelInside);
            int bevelOut = (int)(_FrameBounds.Width * BevelOutside);

            Rectangle r = _FrameBounds;
            r.Inflate(-bevelOut, -bevelOut);

            using (GraphicsPath path = GetRoundRectPath(r))
            {
                using (LinearGradientBrush br = new LinearGradientBrush(r, Color.Silver, Color.Gray, 45 + 180))
                    g.FillPath(br, path);
            }

            r.Inflate(-bevelIn, -bevelIn);

            using (GraphicsPath path = GetRoundRectPath(r))
            {
                using (LinearGradientBrush br = new LinearGradientBrush(r, Color.Silver, Color.LightGray, 45))
                    g.FillPath(br, path);
            }
        }

        #region GetRoundRectPath

        private GraphicsPath GetRoundRectPath(Rectangle r)
        {
            GraphicsPath path = new GraphicsPath();

            int m = Math.Min(r.Width, r.Height);
            int n = (int)(m * RoundRectangleArc * 2) + 1;

            Rectangle t = new Rectangle(r.Right - n, r.Bottom - n, n, n);
            path.AddArc(t, 0, 90);

            t.X = r.X;
            path.AddArc(t, 90, 90);

            t.Y = r.Y;
            path.AddArc(t, 180, 90);

            t.X = r.Right - n;
            path.AddArc(t, 270, 90);

            path.CloseAllFigures();

            return (path);
        }

        #endregion

        #endregion

        #region DrawBlankFrame

        private void DrawBlankFrame(Graphics g)
        {
            using (Brush br = new SolidBrush(Color.LightGray))
                g.FillRectangle(br, _FrameBounds);

            g.DrawRectangle(Pens.DimGray, _FrameBounds);
        }

        #endregion

        #endregion

        #region DrawScale

        private void DrawScale(Graphics g)
        {
            using (Brush br = new SolidBrush(Color.CornflowerBlue))
            {
                using (Pen pen = new Pen(br, ScaleWidth))
                    g.DrawArc(pen, _ScaleBounds, _StartAngle, _SweepAngle);
            }
        }

        #endregion

        #region DrawPivotDot

        private void DrawPivotDot(Graphics g)
        {
            g.FillEllipse(Brushes.SkyBlue, _DotBounds);
            g.DrawEllipse(Pens.DimGray, _DotBounds);
        }

        #endregion

        #endregion

        #region Mouse support

        #region OnMouseMove

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            _InFrame = (_FrameBounds.Contains(e.Location) == true);

            if (_PivotMoving == true && _InFrame == true)
            {
                PivotPoint = new PointF(
                    (float)(e.X - _FrameBounds.X) / _FrameBounds.Width,
                    (float)(e.Y - _FrameBounds.Y) / _FrameBounds.Height);
            }

            _InPivotDot = (_DotBounds.Contains(e.Location) == true);

            Cursor = (_InPivotDot == true) ? Cursors.Hand : Cursors.Default;
        }

        #endregion

        #region OnMouseLeave

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            _InFrame = false;
            _InPivotDot = false;
        }

        #endregion

        #region OnMouseDown

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.Button == MouseButtons.Left)
            {
                if (_InPivotDot == true)
                    _PivotMoving = true;
            }
        }

        #endregion

        #region OnMouseUp

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            _PivotMoving = false;

            _InFrame = (_FrameBounds.Contains(e.Location) == true);
            _InPivotDot = (_DotBounds.Contains(e.Location) == true);
        }

        #endregion

        #endregion

        #region PivotPointDropDown_PreviewKeyDown

        private void PivotPointDropDown_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                _EscapePressed = true;
        }

        #endregion
    }
}
