using System;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;
using DevComponents.DotNetBar.Controls;

namespace DevComponents.DotNetBar.Ribbon
{
    [ToolboxItem(true), Designer("DevComponents.DotNetBar.Design.RibbonClientPanelDesigner, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
    public class RibbonClientPanel : PanelControl
    {
        #region Private Variables
        private bool m_IsShadowEnabled = true;
        #endregion

        #region Internal Implementation
        /// <summary>
        /// Gets or sets whether panel automatically provides shadows for child controls.
        /// </summary>
        [Browsable(true), DefaultValue(true), Category("Appearance"), Description("Indicates whether panel automatically provides shadows for child controls.")]
        public bool IsShadowEnabled
        {
            get { return m_IsShadowEnabled; }
            set
            {
                if (m_IsShadowEnabled != value)
                {
                    m_IsShadowEnabled = value;
                    this.Invalidate();
                }
            }
        }

        protected override void PaintInnerContent(System.Windows.Forms.PaintEventArgs e, ElementStyle style, bool paintText)
        {
            base.PaintInnerContent(e, style, paintText);

            if (!m_IsShadowEnabled) return;
            Graphics g = e.Graphics;
            ShadowPaintInfo pi = new ShadowPaintInfo();
            pi.Graphics = g;
            pi.Size = 6;
            foreach (Control c in this.Controls)
            {
                if (!c.Visible || c.BackColor == Color.Transparent && !(c is GroupPanel)) continue;
                if (c is GroupPanel)
                {
                    GroupPanel p = c as GroupPanel;
                    pi.Rectangle = new Rectangle(c.Bounds.X, c.Bounds.Y + p.GetInternalClientRectangle().Y/2, c.Bounds.Width, c.Bounds.Height - p.GetInternalClientRectangle().Y/2);
                }
                else
                    pi.Rectangle = c.Bounds;
                ShadowPainter.Paint2(pi);
            }
        }
        #endregion
    }
}
