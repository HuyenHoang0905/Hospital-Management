#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using DevComponents.DotNetBar;

namespace DevComponents.Editors
{
    public class VisualListInput : VisualInputBase
    {
        #region Private Variables
        #endregion

        #region Events
        #endregion

        #region Constructor
        #endregion

        #region Internal Implementation
        protected override bool OnCmdKey(ref System.Windows.Forms.Message msg, System.Windows.Forms.Keys keyData)
        {
            if (keyData == System.Windows.Forms.Keys.Up)
            {
                SelectPrevious();
                return true;
            }
            else if (keyData == System.Windows.Forms.Keys.Down)
            {
                SelectNext();
                return true;
            }
            return base.OnCmdKey(ref msg, keyData);
        }

        protected override void OnMouseWheel(System.Windows.Forms.MouseEventArgs e)
        {
            if (!this.IsReadOnly)
            {
                if (e.Delta > 0)
                {
                    SelectNext();
                }
                else
                {
                    SelectPrevious();
                }
            }

            base.OnMouseWheel(e);
        }

        protected override bool ValidateNewInputStack(string s)
        {
            if (s.Length == 0 && !_AllowEmptyState)
                return false;
            return base.ValidateNewInputStack(s);
        }

        public virtual void SelectNext()
        {
            if (this.IsReadOnly) return;
            ResetInputStack();
            InputComplete(false);
        }

        public virtual void SelectPrevious()
        {
            if (this.IsReadOnly) return;

            ResetInputStack();
            InputComplete(false);
        }

        protected override void ResetValue()
        {
            if (SetInputStack(""))
            {
                SetInputPosition(InputStack.Length);
            }
            base.ResetValue();
        }

        public override void PerformLayout(PaintInfo p)
        {
            Size size = Size.Empty;
            Graphics g = p.Graphics;
            Font font = p.DefaultFont;
            eTextFormat textFormat = eTextFormat.Default | eTextFormat.NoPadding;

            string s = GetMeasureString();

            size = TextDrawing.MeasureString(g, s, font, 0, textFormat);
            size.Width++; // Additional pixel for selection
            this.Size = size;

            base.PerformLayout(p);
        }

        protected virtual string GetMeasureString()
        {
            string s = GetRenderString();
            if (s.Length == 0) s = "T";
            return s;
        }

        protected override void OnPaint(PaintInfo p)
        {
            Graphics g = p.Graphics;
            Font font = p.DefaultFont;
            Color color = p.ForeColor;
            if (!GetIsEnabled(p))
                color = p.DisabledForeColor;
            eTextFormat textFormat = eTextFormat.Default | eTextFormat.NoPadding;

            string text = GetRenderString();
            Rectangle selectionBounds = this.RenderBounds;
            Region oldClip = null;

            if (this.IsFocused && this.InputStack.Length > 0 && this.InputStack.Length < text.Length)
            {
                // Render partial selection based on the input stack
                Size inputSize = TextDrawing.MeasureString(g, text.Substring(0, this.InputStack.Length), font);
                oldClip = g.Clip;
                Rectangle newClip = selectionBounds;
                if (this.IsRightToLeft)
                {
                    newClip.X += newClip.Width - inputSize.Width;
                    newClip.Width = inputSize.Width;
                    selectionBounds.Width -= inputSize.Width;
                }
                else
                {
                    newClip.Width = inputSize.Width;
                    selectionBounds.X += inputSize.Width;
                    selectionBounds.Width -= inputSize.Width;
                }
                g.SetClip(newClip, System.Drawing.Drawing2D.CombineMode.Intersect);
                TextDrawing.DrawString(g, text, font, color, RenderBounds, textFormat);
                g.Clip = oldClip;
                g.SetClip(selectionBounds, System.Drawing.Drawing2D.CombineMode.Intersect);
            }


            if (this.IsFocused)
            {
                if (p.Colors.Highlight.IsEmpty)
                    g.FillRectangle(SystemBrushes.Highlight, selectionBounds);
                else
                {
                    using (SolidBrush brush = new SolidBrush(p.Colors.Highlight))
                        g.FillRectangle(brush, selectionBounds);
                }
                color = p.Colors.HighlightText.IsEmpty ? SystemColors.HighlightText : p.Colors.HighlightText;
            }

            if (!this.IsEmpty)
            {
                TextDrawing.DrawString(g, text, font, color, RenderBounds, textFormat);
            }

            if (oldClip != null)
                g.Clip = oldClip;

            base.OnPaint(p);
        }

        protected virtual string GetRenderString()
        {
            return GetInputStringValue();
        }

        public override bool IsEmpty
        {
            get
            {
                return GetInputStringValue().Length == 0;
            }
            set
            {
                if (value != IsEmpty)
                {
                    if (value && _AllowEmptyState)
                    {
                        ResetValue();
                    }
                }
            }
        }


        private bool _AllowEmptyState = true;
        /// <summary>
        /// Gets or sets whether control allows empty input state i.e. does not have an text entered. Default value is true.
        /// </summary>
        public bool AllowEmptyState
        {
            get { return _AllowEmptyState; }
            set { _AllowEmptyState = value; }
        }
        #endregion

    }
}
#endif

