using System;
using System.Text;
using DevComponents.DotNetBar.Rendering;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Defines the Office 2007 style form caption painter.
    /// </summary>
    internal class Office2007FormCaptionPainter : FormCaptionPainter, IOffice2007Painter
    {
        #region IOffice2007Painter
        private Office2007ColorTable m_ColorTable = null; //new Office2007ColorTable();

        /// <summary>
        /// Gets or sets color table used by renderer.
        /// </summary>
        public Office2007ColorTable ColorTable
        {
            get { return m_ColorTable; }
            set { m_ColorTable = value; }
        }
        #endregion

        #region Internal Implementation
        public override void PaintCaptionBackground(FormCaptionRendererEventArgs e)
        {
            Graphics g = e.Graphics;
            Rendering.Office2007FormStateColorTable formct = m_ColorTable.Form.Active;
            System.Windows.Forms.Form form = e.Form;
            bool drawBottomBorder = false;
            if (form != null && form is Office2007RibbonForm)
            {
                if(!((Office2007RibbonForm)form).NonClientActive)
                    formct = m_ColorTable.Form.Inactive;
            }
            else if (form != null && (form != System.Windows.Forms.Form.ActiveForm && form.MdiParent == null ||
                form.MdiParent != null && form.MdiParent.ActiveMdiChild != form))
                formct = m_ColorTable.Form.Inactive;
            if (form is Office2007Form) drawBottomBorder = true;

            Rectangle captionRect = e.Bounds;

            SmoothingMode sm = g.SmoothingMode;
            g.SmoothingMode = SmoothingMode.Default;
            // Top Part
            Rectangle topCaptionPart = new Rectangle(captionRect.X, captionRect.Y, captionRect.Width, (int)(captionRect.Height * .3));
            DisplayHelp.FillRectangle(g, topCaptionPart, formct.CaptionTopBackground);
            Rectangle bottomCaptionPart = new Rectangle(captionRect.X, topCaptionPart.Bottom, captionRect.Width, captionRect.Height - topCaptionPart.Height);
            DisplayHelp.FillRectangle(g, bottomCaptionPart, formct.CaptionBottomBackground);
            if (drawBottomBorder && formct.CaptionBottomBorder != null && formct.CaptionBottomBorder.Length > 0)
            {
                int lines = formct.CaptionBottomBorder.Length;
                for (int i = 0; i < lines; i++)
                {
                    DisplayHelp.DrawLine(g, captionRect.X, captionRect.Bottom - lines + i, captionRect.Right, captionRect.Bottom - lines + i, formct.CaptionBottomBorder[i], 1);
                }
            }
            g.SmoothingMode = sm;
        }
        #endregion
    }
}
