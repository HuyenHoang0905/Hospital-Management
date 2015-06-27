#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Text;

namespace DevComponents.Editors
{
    public class LockUpdateCheckBox : VisualCheckBox
    {
        #region Private Variables
        #endregion

        #region Events
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the LockUpdateCheckBox class.
        /// </summary>
        public LockUpdateCheckBox()
        {
            this.Checked = true;
        }
        #endregion

        #region Internal Implementation
        public override void PerformLayout(PaintInfo pi)
        {
            base.PerformLayout(pi);
            this.Size = new System.Drawing.Size(this.Size.Width + 4, this.Size.Height);
        }

        protected override void OnCheckedChanged(EventArgs e)
        {
            if (this.Parent is VisualGroup)
            {
                VisualGroup g = this.Parent as VisualGroup;
                for (int i = 0; i < g.Items.Count; i++)
                {
                    VisualItem item = g.Items[i];
                    if (item == this) continue;
                    item.Enabled = this.Checked;
                }
            }

            base.OnCheckedChanged(e);
        }
        #endregion

    }
}
#endif

