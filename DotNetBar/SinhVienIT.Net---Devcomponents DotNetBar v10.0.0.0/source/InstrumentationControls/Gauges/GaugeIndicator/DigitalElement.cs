using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DevComponents.Instrumentation
{
    public class DigitalElement : NumericElement
    {
        #region Private variables

        private int _ElemWidth;
        private int _ElemHeight;
        private int _ElemWidthHalf;
        private int _ElemHeightHalf;

        private int _SegStep;
        private int _SegWidth;
        private int _SegWidthHalf;

        private bool _NeedRecalcSegments;
        private bool _NeedSegments;

        private Point[][] _SegPoints;
        private int _Segments;

        #endregion

        public DigitalElement(NumericIndicator numIndicator, int segments, int points)
            : base(numIndicator)
        {
            _SegPoints = new Point[segments][];

            for (int i = 0; i < segments; i++)
                _SegPoints[i] = new Point[points];

            _NeedRecalcSegments = true;
            _NeedSegments = true;
        }

        #region Public properties

        #region Value

        public override char Value
        {
            get { return (base.Value); }

            set
            {
                if (base.Value != value)
                {
                    base.Value = value;

                    _NeedSegments = true;
                }
            }
        }

        #endregion

        #endregion

        #region Internal properties

        #region ElemWidth

        internal int ElemWidth
        {
            get { return (_ElemWidth); }
        }

        #endregion

        #region ElemWidthHalf

        internal int ElemWidthHalf
        {
            get { return (_ElemWidthHalf); }
        }

        #endregion

        #region ElemHeight

        internal int ElemHeight
        {
            get { return (_ElemHeight); }
        }

        #endregion

        #region ElemHeightHalf

        internal int ElemHeightHalf
        {
            get { return (_ElemHeightHalf); }
        }

        #endregion

        #region NeedRecalcSegments

        internal bool NeedRecalcSegments
        {
            get { return (_NeedRecalcSegments); }
            set { _NeedRecalcSegments = value; }
        }

        #endregion

        #region Segments

        internal int Segments
        {
            get { return (_Segments); }

            set
            {
                if (_Segments != value)
                {
                    _Segments = value;

                    NumIndicator.Refresh();
                }
            }
        }

        #endregion

        #region SegPoints

        internal Point[][] SegPoints
        {
            get { return (_SegPoints); }
        }

        #endregion

        #region SegStep

        internal int SegStep
        {
            get { return (_SegStep); }
        }

        #endregion

        #region SegWidth

        internal int SegWidth
        {
            get { return (_SegWidth); }
        }

        #endregion

        #region SegWidthHalf

        internal int SegWidthHalf
        {
            get { return (_SegWidthHalf); }
        }

        #endregion

        #endregion

        #region RecalcLayout

        public override void RecalcLayout()
        {
            if (NeedRecalcLayout == true)
            {
                base.RecalcLayout();

                if (_NeedRecalcSegments == true)
                {
                    _NeedRecalcSegments = false;

                    _ElemWidth = 50;
                    _ElemHeight = 100;

                    _ElemWidthHalf = _ElemWidth / 2;
                    _ElemHeightHalf = _ElemHeight / 2;

                    _SegWidth = (int)(_ElemWidth * NumIndicator.SegmentWidth / 4);
                    _SegWidth = Math.Max(2, _SegWidth);

                    _SegWidthHalf = _SegWidth / 2;
                    _SegStep = _SegWidth / 3;

                    RecalcSegments();
                }
            }

            if (_NeedSegments == true)
            {
                _NeedSegments = false;

                _Segments = NumIndicator.GaugeControl.
                    OnGetDigitSegments(NumIndicator, Value, GetDigitSegments(Value));
            }
        }

        #region RecalcSegments

        public virtual void RecalcSegments()
        {
        }

        #endregion

        #endregion

        #region OnGetDigitSegments

        public virtual int GetDigitSegments(char value)
        {
            return (0);
        }

        #endregion

        #region RefreshElements

        internal override void RefreshElements()
        {
            _NeedSegments = true;

            NeedRecalcLayout = true;
        }

        #endregion

        #region OnPaint

        public override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;

            if (Bounds.Width > 0 && Bounds.Height > 0)
            {
                RenderBackground(g, Bounds);

                int dx = (int)(Bounds.Width * .2f);
                int dy = (int)(Bounds.Height * .2f);

                Rectangle r = Bounds;
                r.Inflate(-dx, -dy);

                Rectangle srcRect = new Rectangle(0, 0, _ElemWidth, _ElemHeight);
                GraphicsContainer containerState = g.BeginContainer(r, srcRect, GraphicsUnit.Pixel);

                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.PixelOffsetMode = PixelOffsetMode.Default;

                if (NumIndicator.ShearFactor != 0)
                {
                    Matrix trans = new Matrix();
                    trans.Shear(NumIndicator.ShearFactor, 0.0F);

                    g.Transform = trans;
                }

                Brush brushOn = BrushOn;
                Brush brushOff = BrushOff;

                RenderSegments(g, brushOn, brushOff);
                RenderPoints(g, brushOn, brushOff);

                g.EndContainer(containerState);
            }
        }

        #region RenderBackground

        private void RenderBackground(Graphics g, Rectangle r)
        {
            if (NumIndicator.ShearFactor == 0)
            {
                if (BackColor != null && BackColor.IsEmpty == false)
                {
                    using (Brush br = BackColor.GetBrush(r))
                        g.FillRectangle(br, r);
                }
            }
        }

        #endregion

        #region RenderSegments

        private void RenderSegments(Graphics g, Brush brushOn, Brush brushOff)
        {
            for (int i = 0; i < _SegPoints.Length; i++)
            {
                if ((_Segments & (1 << i)) != 0)
                {
                    if (brushOn != null)
                        g.FillPolygon(brushOn, _SegPoints[i]);
                }
                else if (ShowDimSegments == true)
                {
                    if (brushOff != null)
                        g.FillPolygon(brushOff, _SegPoints[i]);
                }
            }
        }

        #endregion

        #region RenderPoints

        private void RenderPoints(Graphics g, Brush brushOn, Brush brushOff)
        {
            int dpWidth = (_SegWidth * 3) / 2;

            if (ColonPointsOn == true)
                RenderColonPoints(g, brushOn, dpWidth);

            else if (ShowDimColonPoints == true)
                RenderColonPoints(g, brushOff, dpWidth);

            if (DecimalPointOn == true)
                RenderDecimalPoint(g, brushOn, dpWidth);

            else if (ShowDimDecimalPoint == true)
                RenderDecimalPoint(g, brushOff, dpWidth);
        }

        #endregion

        #region RenderDecimalPoint

        private void RenderDecimalPoint(Graphics g, Brush br, int dpWidth)
        {
            if (br != null)
            {
                g.FillEllipse(br, _ElemWidth + _SegWidth,
                    _ElemHeight - dpWidth, dpWidth, dpWidth);
            }
        }

        #endregion

        #region RenderColonPoints

        private void RenderColonPoints(Graphics g, Brush br, int dpWidth)
        {
            if (br != null)
            {
                g.FillEllipse(br, _ElemWidth + _SegWidth,
                    _ElemHeightHalf - _SegWidth * 3, dpWidth, dpWidth);

                g.FillEllipse(br, _ElemWidth + _SegWidth,
                    _ElemHeightHalf + _SegWidth * 2, dpWidth, dpWidth);
            }
        }

        #endregion

        #endregion
    }
}
