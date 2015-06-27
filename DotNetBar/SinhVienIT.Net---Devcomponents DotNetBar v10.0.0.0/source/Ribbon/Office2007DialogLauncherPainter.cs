using System;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using DevComponents.DotNetBar.Rendering;


namespace DevComponents.DotNetBar
{
    internal class Office2007DialogLauncherPainter : DialogLauncherPainter, IOffice2007Painter
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

        
        #region Dialog Launcher Painter
        public override void PaintDialogLauncher(RibbonBarRendererEventArgs e)
        {
            Rectangle r = e.Bounds;
            Graphics g = e.Graphics;
            bool rightToLeft = (e.RibbonBar.RightToLeft == System.Windows.Forms.RightToLeft.Yes);
            Office2007DialogLauncherStateColorTable c = GetColorTable(e);

            if (!c.TopBackground.IsEmpty && !c.BottomBackground.IsEmpty)
            {
                Presentation.Rectangle pr = new Presentation.Rectangle(
                    new Presentation.ShapeBorder(c.OuterBorder), new Presentation.ShapeFill(c.TopBackground));
                pr.Padding = new Presentation.PaddingInfo(1, 1, 1, 1);
                Presentation.Rectangle prb = new Presentation.Rectangle(new Presentation.ShapeFill(c.BottomBackground));
                prb.Size.Height = r.Height / 2;
                pr.Children.Add(prb);
                prb = new Presentation.Rectangle(new Presentation.ShapeBorder(c.InnerBorder));
                pr.Children.Add(prb);
                Presentation.ShapePaintInfo pi = new Presentation.ShapePaintInfo(g, r);
                pr.Paint(pi);
            }

            Size size = new Size(8, 8);

            // Get final dialog launcher bounds
            if (rightToLeft)
                r = new Rectangle(r.X + (r.Width - size.Width) / 2, r.Y + (r.Height - size.Height) / 2, size.Width, size.Height);
            else
                r = new Rectangle(r.X + (r.Width - size.Width)/2, r.Y + (r.Height - size.Height) / 2, size.Width, size.Height);

            SmoothingMode sm = g.SmoothingMode;
            g.SmoothingMode = SmoothingMode.Default;
            
            // Create the dialog launcher shape
            Presentation.ShapeBorder border = new Presentation.ShapeBorder(c.DialogLauncherShade, 1);
            Presentation.ShapeFill fill = new Presentation.ShapeFill(c.DialogLauncherShade);
            Presentation.Shape shape = new Presentation.Shape();

            // Horizontal line
            Presentation.Line line = new Presentation.Line(new Presentation.Location(),
                new Presentation.Location(6, 0), border);
            shape.Children.Add(line);

            // Vertical line
            line = new Presentation.Line(new Presentation.Location(),
                new Presentation.Location(0, 6), border);
            shape.Children.Add(line);

            Presentation.Rectangle rect = new Presentation.Rectangle();
            rect.Fill = fill;
            rect.Location.X = 5;
            rect.Location.Y = 5;
            rect.Size.Width = 3;
            rect.Size.Height = 3;
            shape.Children.Add(rect);

            // Arrow line vertical
            line = new Presentation.Line(new Presentation.Location(7, 4),
                new Presentation.Location(7, 7), border);
            shape.Children.Add(line);
            // Arrow line horizontal
            line = new Presentation.Line(new Presentation.Location(4, 7),
                new Presentation.Location(7, 7), border);
            shape.Children.Add(line);
            // Arrow line cross
            line = new Presentation.Line(new Presentation.Location(4, 4),
                new Presentation.Location(5, 5), border);
            shape.Children.Add(line);

            r.Offset(1, 1);
            Presentation.ShapePaintInfo p = new Presentation.ShapePaintInfo(g, r);
            shape.Paint(p);

            border.Color1 = c.DialogLauncher;
            fill.Color1 = c.DialogLauncher;
            r.Offset(-1, -1);
            p.Bounds = r;
            shape.Paint(p);

            g.SmoothingMode = sm;
        }

        private Office2007DialogLauncherStateColorTable GetColorTable(RibbonBarRendererEventArgs e)
        {
            if (e.Pressed)
                return m_ColorTable.DialogLauncher.Pressed;
            else if (e.MouseOver)
                return m_ColorTable.DialogLauncher.MouseOver;
            else
                return m_ColorTable.DialogLauncher.Default;
        }


        #endregion
    }
}
