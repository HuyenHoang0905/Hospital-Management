using System;
using System.Drawing;
using System.Text;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Represents Office 2007 style MdiSystemItem painter
    /// </summary>
    internal class Office2007MdiSystemItemPainter : MdiSystemItemPainter, IOffice2007Painter
    {
        #region Private Variables
        #endregion
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

        public override void Paint(MdiSystemItemRendererEventArgs e)
        {
            MDISystemItem mdi = e.MdiSystemItem;
            Graphics g = e.Graphics;
            Rectangle r = mdi.DisplayRectangle;

            if (mdi.IsSystemIcon)
            {
                r.Offset((r.Width - 16) / 2, (r.Height - 16) / 2);
                if (mdi.Icon != null)
                    g.DrawIconUnstretched(mdi.Icon, r);
                return;
            }

            
            r = new Rectangle(mdi.DisplayRectangle.Location, mdi.GetButtonSize());
            if (mdi.Orientation == eOrientation.Horizontal)
                r.Offset(0, (mdi.DisplayRectangle.Height - r.Height) / 2);
            else
                r.Offset((mdi.WidthInternal - r.Width) / 2, 0);

            this.PaintButton(g, mdi, SystemButton.Minimize, r, GetColorTable(mdi, SystemButton.Minimize), m_ColorTable.LegacyColors);

            if (mdi.Orientation == eOrientation.Horizontal)
                r.Offset(r.Width, 0);
            else
                r.Offset(0, r.Height);
            this.PaintButton(g, mdi, SystemButton.Restore, r, GetColorTable(mdi, SystemButton.Restore), m_ColorTable.LegacyColors);

            if (mdi.Orientation == eOrientation.Horizontal)
                r.Offset(r.Width + 2, 0);
            else
                r.Offset(0, r.Height + 2);

            this.PaintButton(g, mdi, SystemButton.Close, r, GetColorTable(mdi, SystemButton.Close), m_ColorTable.LegacyColors);
        }

        private Office2007ButtonItemStateColorTable GetColorTable(MDISystemItem mdi, SystemButton button)
        {
            Office2007ButtonItemColorTable colors = m_ColorTable.ButtonItemColors[eButtonColor.Orange.ToString()];
            Office2007ButtonItemStateColorTable ct = colors.Default;
            if (mdi.MouseDownButton == button)
                ct = colors.Pressed;
            else if (mdi.MouseOverButton == button)
                ct = colors.MouseOver;

            return ct;
        }

        private void PaintButton(Graphics g, MDISystemItem mdi , SystemButton button, Rectangle r, Office2007ButtonItemStateColorTable ct, ColorScheme colorScheme)
        {
            Region oldClip = g.Clip;
            g.SetClip(r);

            Office2007ButtonItemPainter.PaintBackground(g, ct, r, RoundRectangleShapeDescriptor.RoundCorner2);
            r.Inflate(-1, -1);
            r.Offset(1, 0);
            using (Bitmap bmp = mdi.GetButtonBitmap(g, button, r, colorScheme))
            {
                if (button == SystemButton.Minimize && !mdi.MinimizeEnabled ||
                    button == SystemButton.Restore && !mdi.RestoreEnabled ||
                    button == SystemButton.Close && !mdi.CloseEnabled)
                {
                    float[][] array = new float[5][];
                    array[0] = new float[5] { 0, 0, 0, 0, 0 };
                    array[1] = new float[5] { 0, 0, 0, 0, 0 };
                    array[2] = new float[5] { 0, 0, 0, 0, 0 };
                    array[3] = new float[5] { .5f, .5f, .5f, .5f, 0 };
                    array[4] = new float[5] { 0, 0, 0, 0, 0 };
                    System.Drawing.Imaging.ColorMatrix grayMatrix = new System.Drawing.Imaging.ColorMatrix(array);
                    System.Drawing.Imaging.ImageAttributes disabledImageAttr = new System.Drawing.Imaging.ImageAttributes();
                    disabledImageAttr.ClearColorKey();
                    disabledImageAttr.SetColorMatrix(grayMatrix);
                    g.DrawImage(bmp, r, 0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel, disabledImageAttr);
                }
                else
                {
                    if (button == mdi.MouseDownButton)
                        r.Offset(1, 1);
                    g.DrawImageUnscaled(bmp, r);
                }
            }

            g.Clip = oldClip;
        }
    }
}
