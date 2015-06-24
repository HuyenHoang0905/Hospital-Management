#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using DevComponents.DotNetBar;

namespace DevComponents.Editors
{
    public class VisualLabel : VisualItem
    {
        #region Private Variables
        #endregion

        #region Events
        #endregion

        #region Constructor
        #endregion

        #region Internal Implementation
        private string _Text = "";
        /// <summary>
        /// Gets or sets the text displayed by the label.
        /// </summary>
        public string Text
        {
            get { return _Text; }
            set
            {
                if (value == null) value = "";
                if (_Text != value)
                {
                    _Text = value;
                    InvalidateArrange();
                }
            }
        }

        public override void PerformLayout(PaintInfo p)
        {
            Size size = Size.Empty;
            Graphics g = p.Graphics;
            Font font = p.DefaultFont;
            eTextFormat textFormat = eTextFormat.Default | eTextFormat.NoPadding;
            if (_Text.Length > 0)
            {
                if (Text.Trim().Length == 0)
                {
                    size = TextDrawing.MeasureString(g, Text.Replace(' ', '|'), font, 0, textFormat);
                }
                else
                    size = TextDrawing.MeasureString(g, Text, font, 0, textFormat);
            }

            this.Size = size;
            base.PerformLayout(p);
        }

        protected override void OnPaint(PaintInfo p)
        {

            Graphics g = p.Graphics;
            Font font = p.DefaultFont;
            Rectangle r = this.RenderBounds;
            eTextFormat textFormat = eTextFormat.Default | eTextFormat.NoPadding;
            Color color = p.ForeColor;
            if (!GetIsEnabled(p))
                color = p.DisabledForeColor;
            if (_Text.Length > 0)
                TextDrawing.DrawString(g, _Text, font, color, r, textFormat);

            base.OnPaint(p);
        }
        #endregion
    }
}
#endif

