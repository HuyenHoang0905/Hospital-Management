using System;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DevComponents.DotNetBar.Rendering
{
    /// <summary>
    /// Defines painter for the Office 2007 style QAT Customize Item.
    /// </summary>
    internal class Office2007QatCustomizeItemPainter : QatCustomizeItemPainter, IOffice2007Painter
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
        public override void Paint(QatCustomizeItemRendererEventArgs e)
        {
            Graphics g = e.Graphics;
            QatCustomizeItem item = e.CustomizeItem;
            Rectangle r = item.DisplayRectangle;
            Region oldClip = null;
            if (g.Clip != null) oldClip = g.Clip as Region;
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
            shape.Children.Add(new Presentation.Line(new Presentation.Location(0, 0), new Presentation.Location(4, 0), b));
            shape.Children.Add(new Presentation.Line(new Presentation.Location(0, 1), new Presentation.Location(4, 1), bl));

            shape.Children.Add(new Presentation.Line(new Presentation.Location(0, 3), new Presentation.Location(4, 3), b));
            shape.Children.Add(new Presentation.Line(new Presentation.Location(4, 3), new Presentation.Location(2, 5), b));
            shape.Children.Add(new Presentation.Line(new Presentation.Location(2, 5), new Presentation.Location(0, 3), b));
            shape.Children.Add(new Presentation.Line(new Presentation.Location(1, 4), new Presentation.Location(3, 4), b));
            shape.Children.Add(new Presentation.Line(new Presentation.Location(4, 4), new Presentation.Location(2, 6), bl));
            shape.Children.Add(new Presentation.Line(new Presentation.Location(2, 6), new Presentation.Location(0, 4), bl));

            Rectangle sr = new Rectangle(r.X + (r.Width - 5) / 2, r.Y + (r.Height - 7) / 2, 5, 7);
            Presentation.ShapePaintInfo pi = new Presentation.ShapePaintInfo(g, sr);
            shape.Paint(pi);

            g.SmoothingMode = sm;

            if (oldClip != null)
            {
                g.Clip = oldClip;
                oldClip.Dispose();
            }
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
