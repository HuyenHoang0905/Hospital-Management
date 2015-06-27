using System;
using System.Drawing;
using System.Windows.Forms;

namespace DevComponents.Instrumentation
{
    public class NumericElement : IDisposable
    {
        #region Private variables

        private Rectangle _Bounds;
        private NumericIndicator _NumIndicator;

        private char _Value;
        private bool _ColonPointsOn;
        private bool _DecimalPointOn;
        private bool _ShowDimColonPoints;
        private bool _ShowDimDecimalPoint;
        private bool _ShowDimSegments;

        private Color _DigitColor;
        private Color _DigitDimColor;
        private GradientFillColor _BackColor;

        private Brush _BrushOn;
        private Brush _BrushOff;

        private bool _NeedRecalcLayout;

        private bool _InRender;

        #endregion

        public NumericElement(NumericIndicator numIndicator)
        {
            _NumIndicator = numIndicator;

            _ShowDimColonPoints = true;
            _ShowDimDecimalPoint = true;
            _ShowDimSegments = true;
        }

        #region Public properties

        #region BackColor

        public GradientFillColor BackColor
        {
            get { return (_BackColor); }

            set
            {
                _BackColor = value;

                Refresh();
            }
        }

        #endregion

        #region Bounds

        public Rectangle Bounds
        {
            get { return (_Bounds); }

            internal set
            {
                if (_Bounds.Equals(value) == false)
                {
                    _Bounds = value;

                    _NeedRecalcLayout = true;
                }
            }
        }

        #endregion

        #region ColonPointsOn

        public bool ColonPointsOn
        {
            get { return (_ColonPointsOn); }

            set
            {
                if (_ColonPointsOn != value)
                {
                    _ColonPointsOn = value;

                    Refresh();
                }
            }
        }

        #endregion

        #region DecimalPointOn

        public bool DecimalPointOn
        {
            get { return (_DecimalPointOn); }

            set
            {
                if (_DecimalPointOn != value)
                {
                    _DecimalPointOn = value;

                    Refresh();
                }
            }
        }

        #endregion

        #region DigitColor

        /// <summary>
        /// Gets or sets the Digit Color
        /// </summary>
        public Color DigitColor
        {
            get { return (_DigitColor); }

            set
            {
                if (_DigitColor != value)
                {
                    BrushOn = null;
                    _DigitColor = value;

                    Refresh();
                }
            }
        }

        #endregion

        #region DigitDimColor

        /// <summary>
        /// Gets or sets the default Digit Dim Color (Dim LED color)
        /// </summary>
        public Color DigitDimColor
        {
            get { return (_DigitDimColor); }

            set
            {
                if (_DigitDimColor != value)
                {
                    BrushOff = null;
                    _DigitDimColor = value;

                    Refresh();
                }
            }
        }

        #endregion

        #region ShowDimColonPoints

        public bool ShowDimColonPoints
        {
            get { return (_ShowDimColonPoints); }

            set
            {
                if (_ShowDimColonPoints != value)
                {
                    _ShowDimColonPoints = value;

                    Refresh();
                }
            }
        }

        #endregion

        #region ShowDimDecimalPoint

        public bool ShowDimDecimalPoint
        {
            get { return (_ShowDimDecimalPoint); }

            set
            {
                if (_ShowDimDecimalPoint != value)
                {
                    _ShowDimDecimalPoint = value;

                    Refresh();
                }
            }
        }

        #endregion

        #region ShowDimSegments

        /// <summary>
        /// Gets or sets the whether dim segments are displayed
        /// </summary>
        public bool ShowDimSegments
        {
            get { return (_ShowDimSegments); }

            set
            {
                if (_ShowDimSegments != value)
                {
                    BrushOff = null;
                    _ShowDimSegments = value;

                    Refresh();
                }
            }
        }

        #endregion

        #region Value

        public virtual char Value
        {
            get { return (_Value); }

            set
            {
                if (_Value != value)
                {
                    _Value = value;

                    Refresh();
                }
            }
        }

        #endregion

        #endregion

        #region Internal properties

        #region AbsSegmentWidth

        internal int AbsSegmentWidth
        {
            get { return ((int)(Bounds.Width * _NumIndicator.SegmentWidth)); }
        }

        #endregion

        #region BrushOn

        internal Brush BrushOn
        {
            get
            {
                if (_BrushOn == null)
                {
                    if (_DigitColor.IsEmpty == false)
                        _BrushOn = new SolidBrush(_DigitColor);
                }

                return (_BrushOn);
            }

            set
            {
                if (_BrushOn != null)
                    _BrushOn.Dispose();

                _BrushOn = value;
            }
        }

        #endregion

        #region BrushOff

        internal Brush BrushOff
        {
            get
            {
                if (_BrushOff == null)
                {
                    if (DigitDimColor.IsEmpty == false)
                        _BrushOff = new SolidBrush(_DigitDimColor);
                }

                return (_BrushOff);
            }

            set
            {
                if (_BrushOff != null)
                    _BrushOff.Dispose();

                _BrushOff = value;
            }
        }

        #endregion

        #region CanRefresh

        internal bool CanRefresh
        {
            get { return (_InRender == false); }
        }

        #endregion

        #region InPaint

        internal bool InRenderCallout
        {
            get { return (_InRender); }
            set { _InRender = value; }
        }

        #endregion

        #region NeedRecalcLayout

        internal bool NeedRecalcLayout
        {
            get { return (_NeedRecalcLayout); }
            set { _NeedRecalcLayout = value; }
        }

        #endregion

        #region NumIndicator

        /// <summary>
        /// Gets or sets the owning NumericIndicator
        /// </summary>
        internal NumericIndicator NumIndicator
        {
            get { return (_NumIndicator); }
            set { _NumIndicator = value; }
        }

        #endregion

        #endregion

        #region RecalcLayout

        public virtual void RecalcLayout()
        {
            if (_NeedRecalcLayout == true)
                _NeedRecalcLayout = false;
        }

        #endregion

        #region OnPaint

        public virtual void OnPaint(PaintEventArgs e)
        {
            RecalcLayout();
        }

        #endregion

        #region Refresh

        internal void Refresh()
        {
            if (CanRefresh == true)
                NumIndicator.Refresh();
        }

        #endregion

        #region RefreshElements

        internal virtual void RefreshElements()
        {
        }

        #endregion

        #region GetPaddedRect

        internal Rectangle GetPaddedRect(Rectangle r)
        {
            Padding padding = _NumIndicator.Padding;

            r.Y -= padding.Top;
            r.Height += (padding.Top + padding.Bottom);

            if (_NumIndicator.Digits[0] == this)
            {
                r.X -= padding.Left;
                r.Width += padding.Left;
            }

            if (_NumIndicator.Digits[_NumIndicator.Digits.Length - 1] == this)
                r.Width += padding.Right;

            return (r);
        }

        #endregion

        #region IDisposable Members

        public virtual void Dispose()
        {
            BrushOn = null;
            BrushOff = null;
        }

        #endregion

        #region CopyToItem

        public virtual void CopyToItem(NumericElement copy)
        {
            copy.BackColor = _BackColor;
            copy.Bounds = _Bounds;
            copy.DigitDimColor = _DigitDimColor;
            copy.ColonPointsOn = _ColonPointsOn;
            copy.DigitColor = _DigitColor;
            copy.DigitDimColor = _DigitDimColor;
            copy.ShowDimColonPoints = _ShowDimColonPoints;
            copy.ShowDimDecimalPoint = _ShowDimDecimalPoint;
            copy.ShowDimSegments = _ShowDimSegments;
            copy.Value = _Value;
        }

        #endregion
    }
}
