#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace DevComponents.Editors
{
    public class VisualCustomButton : VisualButton
    {
        #region Private Variables
        #endregion

        #region Events
        #endregion

        #region Constructor
        #endregion

        #region Internal Implementation
        protected override void PaintButtonBackground(PaintInfo p, DevComponents.DotNetBar.Rendering.Office2007ButtonItemStateColorTable ct)
        {
            base.PaintButtonBackground(p, ct);

            if (this.Text.Length == 0 && this.Image == null)
            {
                Point pt = new Point(RenderBounds.X + (RenderBounds.Width - 7) / 2, RenderBounds.Bottom - 6);
                using (SolidBrush brush = new SolidBrush(ct.Text))
                {
                    Size rs = new Size(2, 2);
                    for (int i = 0; i < 3; i++)
                    {
                        p.Graphics.FillRectangle(brush, new Rectangle(pt, rs));
                        pt.X += rs.Width + 1;
                    }
                }
            }
        }
        #endregion

    }
}
#endif

