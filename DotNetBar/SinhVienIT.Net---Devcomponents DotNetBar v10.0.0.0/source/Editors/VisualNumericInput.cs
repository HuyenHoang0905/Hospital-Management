#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using DevComponents.DotNetBar;

namespace DevComponents.Editors
{
    public class VisualNumericInput : VisualInputBase
    {
        #region Private Variables
        #endregion

        #region Events
        /// <summary>
        /// Occurs when Value property has changed.
        /// </summary>
        public event EventHandler ValueChanged;
        #endregion

        #region Constructor
        #endregion

        #region Internal Implementation
        protected override void OnKeyDown(System.Windows.Forms.KeyEventArgs e)
        {
            if ((e.KeyCode == System.Windows.Forms.Keys.Subtract || e.KeyCode == System.Windows.Forms.Keys.OemMinus) && AllowsNegativeValue && this.InputStack.Length>0)
            {
                if (!this.IsEmpty)
                    NegateValue();
                e.Handled = true;
            }

            base.OnKeyDown(e);
        }

        protected override bool OnCmdKey(ref System.Windows.Forms.Message msg, System.Windows.Forms.Keys keyData)
        {
            if (!this.IsReadOnly)
            {
                if (keyData == System.Windows.Forms.Keys.Up)
                {
                    IncreaseValue();
                    return true;
                }
                else if (keyData == System.Windows.Forms.Keys.Down)
                {
                    DecreaseValue();
                    return true;
                }
            }
            return base.OnCmdKey(ref msg, keyData);
        }

        protected override void OnMouseWheel(System.Windows.Forms.MouseEventArgs e)
        {
            if (!this.IsReadOnly)
            {
                if (e.Delta > 0)
                {
                    IncreaseValue();
                }
                else
                {
                    DecreaseValue();
                }
            }

            base.OnMouseWheel(e);
        }

        public virtual void DecreaseValue()
        {
            InputComplete(false);
            ResetInputPosition();
        }

        public virtual void IncreaseValue()
        {
            InputComplete(false);
            ResetInputPosition();
        }

        protected virtual void NegateValue()
        {
        }

        protected override bool AcceptKeyPress(System.Windows.Forms.KeyPressEventArgs e)
        {
            if (e.KeyChar >= 48 && e.KeyChar <= 57 || 
                this.AllowsNegativeValue && e.KeyChar == '-' && (this.IsEmpty || !this.IsEmpty && (this.InputStack=="" || this.InputStack=="0")))
            {
                e.Handled = true;
                return true;
            }
            return false;
        }

        public override void PerformLayout(PaintInfo p)
        {
            Size size = Size.Empty;
            Graphics g = p.Graphics;
            Font font = p.DefaultFont;
            eTextFormat textFormat = eTextFormat.Default | eTextFormat.NoPadding;

            string s = GetMeasureString();

            size = TextDrawing.MeasureString(g, s, font, 0, textFormat);
            size.Width++;
            this.Size = size;

            base.PerformLayout(p);
        }

        protected virtual string GetMeasureString()
        {
            return "";
        }

        protected override void OnPaint(PaintInfo p)
        {
            Graphics g = p.Graphics;
            Font font = p.DefaultFont;
            Color color = p.ForeColor;
            if (!this.GetIsEnabled(p))
                color = p.DisabledForeColor;
            eTextFormat textFormat = eTextFormat.Default | eTextFormat.NoPadding;
            if (this.IsFocused)
            {
                if (p.Colors.Highlight.IsEmpty)
                    g.FillRectangle(SystemBrushes.Highlight, this.RenderBounds);
                else
                {
                    using (SolidBrush brush = new SolidBrush(p.Colors.Highlight))
                        g.FillRectangle(brush, this.RenderBounds);
                }
                color = p.Colors.HighlightText.IsEmpty ? SystemColors.HighlightText : p.Colors.HighlightText;
            }

            if (!(this.IsEmpty && this.AllowEmptyState))
            {
         
                string text = GetRenderString();
                TextDrawing.DrawString(g, text, font, color, RenderBounds, textFormat);
            }

            base.OnPaint(p);
        }

        protected virtual string GetRenderString()
        {
            return "";
        }

        private bool _AllowEmptyState = true;
        /// <summary>
        /// Gets or sets whether control allows empty input state i.e. does not have an number entered. Default value is true. When control is empty
        /// IsEmpty property returns true and control does not display number. Set to false to always force control to have number displayed.
        /// </summary>
        public bool AllowEmptyState
        {
            get { return _AllowEmptyState; }
            set { _AllowEmptyState = value; }
        }

        private bool _AllowsNegativeValue = true;
        public bool AllowsNegativeValue
        {
            get { return _AllowsNegativeValue; }
            set { _AllowsNegativeValue = value; }
        }

        protected override bool SetInputStack(string s)
        {
            s = ProcessNewInputStack(s);
            return base.SetInputStack(s);
        }
        private bool _LeadingZero = false;
        protected virtual string ProcessNewInputStack(string s)
        {
            if (this.InputStack == "0" && s != "0" && s.StartsWith("0"))
            {
                s = s.Substring(1);
                this.SetInputPosition(this.InputPosition - 1);
                _LeadingZero = true;
            }
            else
                _LeadingZero = false;
            return s;
        }
        protected virtual bool IsLeadingZero
        { 
            get
            {
                return _LeadingZero;
            }
        }
        protected override void ResetInputStack()
        {
            _LeadingZero = false;
            base.ResetInputStack();
        }

        protected virtual void OnValueChanged()
        {
            if (ValueChanged != null)
                ValueChanged(this, new EventArgs());
        }

        protected override void OnInputLostFocus()
        {
            if (this.InputStack == "-")
                OnClear();
            else
                ResetInputStack();
            base.OnInputLostFocus();
        }
        #endregion

    }
}
#endif

