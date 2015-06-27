#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using DevComponents.DotNetBar.Rendering;
using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace DevComponents.Editors
{
    public class VisualCheckBox : VisualToggleButton
    {
        #region Private Variables
        private Size _DefaultSize = new Size(13, 13);
        #endregion

        #region Events
        #endregion

        #region Constructor
        #endregion

        #region Internal Implementation
        public override void PerformLayout(PaintInfo pi)
        {
            this.Size = new System.Drawing.Size(_DefaultSize.Width + 2, _DefaultSize.Height);
            base.PerformLayout(pi);
        }

        protected override void OnPaint(PaintInfo p)
        {
            Graphics g = p.Graphics;
            Rectangle r = new Rectangle(RenderBounds.X + 1, RenderBounds.Y + (RenderBounds.Height - _DefaultSize.Height) / 2,
                _DefaultSize.Width, _DefaultSize.Height);


            Office2007CheckBoxItemPainter painter = PainterFactory.CreateCheckBoxItemPainter(null);
            Office2007CheckBoxColorTable ctt = GetCheckBoxColorTable();
            Office2007CheckBoxStateColorTable ct = ctt.Default;
            if (!this.GetIsEnabled(p))
                ct = ctt.Disabled;
            else if (this.IsMouseDown)
                ct = ctt.Pressed;
            else if (this.IsMouseOver)
                ct = ctt.MouseOver;

            painter.PaintCheckBox(g, r,
                ct, this.Checked ? CheckState.Checked : CheckState.Unchecked);

            base.OnPaint(p);
        }

        private Office2007CheckBoxColorTable GetCheckBoxColorTable()
        {
            Office2007Renderer r = this.GetRenderer() as Office2007Renderer;
            if (r != null)
            {
                return r.ColorTable.CheckBoxItem;
            }

            return null;
        }

        private DevComponents.DotNetBar.Rendering.BaseRenderer GetRenderer()
        {
            return DevComponents.DotNetBar.Rendering.GlobalManager.Renderer;

        }
        #endregion

    }
}
#endif

