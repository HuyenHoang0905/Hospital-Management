#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Text;
using DevComponents.DotNetBar;
using System.Drawing;

namespace DevComponents.Editors
{
    public class VisualDropDownButton : VisualButton
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
                using (SolidBrush brush = new SolidBrush(ct.Text))
                    p.Graphics.FillPolygon(brush, Office2007ButtonItemPainter.GetExpandPolygon(this.RenderBounds, ePopupSide.Default));
            }
        }
        #endregion

    }
}
#endif

