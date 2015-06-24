using System;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DevComponents.DotNetBar.Rendering
{
    /// <summary>
    /// Defines Office 2007 style QAT painter.
    /// </summary>
    internal class Office2007QatOverflowPainter : QatOverflowPainter, IOffice2007Painter
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
        public override void Paint(QatOverflowItemRendererEventArgs e)
        {
            Graphics g = e.Graphics;
            QatOverflowItem item = e.OverflowItem;
            Rectangle r = item.DisplayRectangle;
            Region oldClip = null;
            if(g.Clip!=null) oldClip = g.Clip.Clone() as Region;
            g.SetClip(item.DisplayRectangle, CombineMode.Intersect);

            
            Office2007ButtonItemColorTable buttonColorTable = GetColorTable();
            Office2007ButtonItemStateColorTable state = buttonColorTable.Default;
            
            if (item.Expanded)
                state = buttonColorTable.Expanded;
            else if (item.IsMouseOver)
                state = buttonColorTable.MouseOver;

            Office2007ButtonItemPainter.PaintBackground(g, state, r, RoundRectangleShapeDescriptor.RoundCorner2);

            SmoothingMode sm = g.SmoothingMode;
            g.SmoothingMode = SmoothingMode.Default;

            Color color = state.ExpandBackground;
            Color colorLight = state.ExpandLight;

            Presentation.Shape shape = new Presentation.Shape();
            Presentation.ShapeBorder bl = new Presentation.ShapeBorder(colorLight, 1);
            Presentation.ShapeBorder b = new Presentation.ShapeBorder(color, 1);
            shape.Children.Add(new Presentation.Line(new Presentation.Location(0, 0), new Presentation.Location(2, 2), bl));
            shape.Children.Add(new Presentation.Line(new Presentation.Location(2, 2), new Presentation.Location(0, 4), bl));
            shape.Children.Add(new Presentation.Line(new Presentation.Location(0, 1), new Presentation.Location(1, 2), b));
            shape.Children.Add(new Presentation.Line(new Presentation.Location(1, 2), new Presentation.Location(0, 3), b));
            shape.Children.Add(new Presentation.Line(new Presentation.Location(0, 3), new Presentation.Location(0, 1), b));

            Rectangle sr = new Rectangle(r.X + (r.Width - 5)/2, r.Y + (r.Height - 4) / 2, 3, 6);
            Presentation.ShapePaintInfo pi = new Presentation.ShapePaintInfo(g, sr);
            shape.Paint(pi);
            sr.Offset(4, 0);
            pi.Bounds = sr;
            shape.Paint(pi);
            g.SmoothingMode = sm;

            if (oldClip != null)
                g.Clip = oldClip;
            else
                g.ResetClip();
        }

        protected virtual Office2007ButtonItemColorTable GetColorTable()
        {
            Office2007ColorTable colorTable = this.ColorTable;
            Office2007ButtonItemColorTable buttonColorTable = null;

            eButtonColor color = eButtonColor.Orange;
            buttonColorTable = colorTable.ButtonItemColors[Enum.GetName(typeof(eButtonColor), color)];

            if (buttonColorTable == null)
                return colorTable.ButtonItemColors[0];

            return buttonColorTable;
        }
        #endregion
    }
}
