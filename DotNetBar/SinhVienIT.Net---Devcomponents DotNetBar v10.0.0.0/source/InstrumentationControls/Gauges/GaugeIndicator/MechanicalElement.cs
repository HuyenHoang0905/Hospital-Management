using System.Drawing;
using System.Windows.Forms;

namespace DevComponents.Instrumentation
{
    public class MechanicalElement : NumericElement
    {
        #region Private variables

        private int _Descent;

        #endregion

        public MechanicalElement(NumericIndicator numIndicator)
            : base(numIndicator)
        {
        }

        #region RecalcLayout

        public override void RecalcLayout()
        {
            base.RecalcLayout();

            Font font = NumIndicator.AbsFont;
            FontFamily family = new FontFamily(font.Name);

            int descent = family.GetCellDescent(font.Style);
            float descentPixel = font.Size * descent / family.GetEmHeight(font.Style);

            _Descent = (int)(descentPixel / 2) + 1;
        }

        #endregion

        #region OnPaint

        public override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (Bounds.Width > 0 && Bounds.Height > 0)
            {
                Graphics g = e.Graphics;

                if (BackColor != null && BackColor.IsEmpty == false)
                {
                    Rectangle r = GetPaddedRect(Bounds);

                    using (Brush br = BackColor.GetBrush(r))
                        g.FillRectangle(br, r);
                }

                using (StringFormat sf = new StringFormat())
                {
                    sf.Alignment = StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Center;

                    Rectangle r = Bounds;
                    r.Y += _Descent;

                    g.DrawString(Value.ToString(), NumIndicator.AbsFont, BrushOn, r, sf);
                }
            }
        }

        #endregion
    }
}
