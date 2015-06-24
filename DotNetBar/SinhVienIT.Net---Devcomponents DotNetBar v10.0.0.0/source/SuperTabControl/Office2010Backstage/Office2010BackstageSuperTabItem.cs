#if FRAMEWORK20
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar
{
    public class Office2010BackstageSuperTabItem : SuperTabItemBaseDisplay
    {
        #region Constants

        private const int MarkerHeight = 7;
        private const int MarkerWidth = 13;

        #endregion

        /// <summary>
        /// Office 2010 Backstage SuperTabItem display constructor
        /// </summary>
        /// <param name="tabItem">Associated SuperTabItem</param>
        public Office2010BackstageSuperTabItem(SuperTabItem tabItem)
            : base(tabItem)
        {
        }

        #region DrawTabBorder

        /// <summary>
        /// Draws the tab border
        /// </summary>
        /// <param name="g"></param>
        /// <param name="path"></param>
        /// <param name="ct"></param>
        protected override void DrawTabBorder(Graphics g,
            GraphicsPath path, SuperTabItemStateColorTable ct)
        {
            base.DrawTabBorder(g, path, ct);

            // Draw the Backstage selection marker

            if (ct.SelectionMarker.IsEmpty == false)
                DrawSelMarker(g, ct);
        }

        #region DrawSelMarker

        /// <summary>
        /// Draws the Backstage SelectedItem marker
        /// </summary>
        /// <param name="g"></param>
        /// <param name="ct"></param>
        private void DrawSelMarker(Graphics g, SuperTabItemStateColorTable ct)
        {
            Rectangle r = TabItem.DisplayRectangle;

            Point[] pts;

            switch (TabStripItem.TabAlignment)
            {
                case eTabStripAlignment.Top:
                    r.X += (r.Width - MarkerWidth) / 2;
                    r.Y = r.Bottom - MarkerHeight + 2;

                    r.Width = MarkerWidth;
                    r.Height = MarkerHeight;

                    pts = new Point[] {
                        new Point(r.X, r.Bottom),
                        new Point(r.X + MarkerWidth / 2, r.Y),
                        new Point(r.Right, r.Bottom)};

                    break;

                case eTabStripAlignment.Left:
                    r.X = r.Right - MarkerHeight + 2;
                    r.Y += (r.Height - MarkerWidth) / 2;

                    r.Width = MarkerHeight;
                    r.Height = MarkerWidth;

                    pts = new Point[] {
                        new Point(r.Right, r.Bottom),
                        new Point(r.X, r.Y + MarkerWidth / 2),
                        new Point(r.Right, r.Y)};

                    break;

                case eTabStripAlignment.Bottom:
                    r.X += (r.Width - MarkerWidth) / 2;
                    r.Y -= 3;

                    r.Width = MarkerWidth;
                    r.Height = MarkerHeight;

                    pts = new Point[] {
                        new Point(r.X, r.Y),
                        new Point(r.X + MarkerWidth / 2, r.Bottom),
                        new Point(r.Right, r.Y)};

                    break;

                default:
                    r.X -= 3;
                    r.Y += (r.Height - MarkerWidth) / 2;

                    r.Width = MarkerHeight;
                    r.Height = MarkerWidth;

                    pts = new Point[] {
                        new Point(r.X, r.Y),
                        new Point(r.Right, r.Y + MarkerWidth / 2),
                        new Point(r.X, r.Bottom)};

                    break;
            }

            using (Brush br = new SolidBrush(ct.SelectionMarker))
                g.FillPolygon(br, pts);
        }

        #endregion

        #endregion

        #region DrawTabItemBackground

        /// <summary>
        /// DrawTabItemBackground
        /// </summary>
        /// <param name="g"></param>
        /// <param name="path"></param>
        /// <param name="tabColors"></param>
        protected override void DrawTabItemBackground(Graphics g,
            GraphicsPath path, SuperTabItemStateColorTable tabColors)
        {
            Rectangle r = Rectangle.Round(path.GetBounds());

            if (r.Width > 0 && r.Height > 0)
            {
                if (tabColors.Background.Colors != null)
                {
                    if (tabColors.Background.Colors.Length == 1)
                    {
                        DrawTabItemSolidBackground(g, path, tabColors, r);

                        if (TabItem.IsSelected == true)
                            DrawTabItemHighLight(g, path, tabColors, r);
                    }
                    else
                    {
                        DrawTabItemGradientBackground(g, path, tabColors, r);
                    }
                }
            }
        }

        #endregion

        #region DrawTabItemHighLight

        /// <summary>
        /// DrawTabItemHighLight
        /// </summary>
        /// <param name="g"></param>
        /// <param name="path"></param>
        /// <param name="tabColors"></param>
        /// <param name="r"></param>
        private void DrawTabItemHighLight(
            Graphics g, GraphicsPath path, SuperTabItemStateColorTable tabColors, Rectangle r)
        {
            Region clip = g.Clip;
            g.SetClip(path, CombineMode.Intersect);

            using (GraphicsPath path2 = new GraphicsPath())
            {
                Rectangle t = r;
                t.Inflate(r.Width / 3, r.Height / 3);

                path2.AddEllipse(t);

                using (PathGradientBrush pbr = new PathGradientBrush(path2))
                {
                    pbr.CenterPoint = new
                        PointF(r.X + r.Width / 2, r.Bottom - (r.Height / 4));

                    pbr.CenterColor = ControlPaint.LightLight(tabColors.Background.Colors[0]);
                    pbr.SurroundColors = new Color[] {Color.Empty};

                    g.FillPath(pbr, path2);
                }
            }

            g.Clip = clip;
        }

        #endregion

        #region TabItemPath

        /// <summary>
        /// Creates the tab item GraphicsPath
        /// </summary>
        /// <returns>Tab path</returns>
        internal override GraphicsPath TabItemPath()
        {
            // Check to see if the user is supplying a path

            GraphicsPath path = base.TabItemPath();

            if (path != null)
                return (path);

            // Supply the default path based upon
            // the current tab alignment

            switch (TabItem.TabAlignment)
            {
                case eTabStripAlignment.Top:
                    return (TopTabPath());

                case eTabStripAlignment.Bottom:
                    return (BottomTabPath());

                case eTabStripAlignment.Left:
                    return (LeftTabPath());

                default:
                    return (RightTabPath());
            }
        }

        #region TopTabPath

        /// <summary>
        /// Create the Top tab path
        /// </summary>
        /// <returns>GraphicsPath</returns>
        private GraphicsPath TopTabPath()
        {
            Rectangle r = TabItem.DisplayRectangle;
            r.Width -= 1;
            r.Height -= 1;

            // Allow for the TabStrip border

            if (TabItem.IsSelected == true)
                r.Height += 2;

            // Create the path

            GraphicsPath path = new GraphicsPath();

            path.AddLines(new Point[]
            {
                new Point(r.X, r.Bottom), 
                new Point(r.X, r.Top),
                new Point(r.Right, r.Top),
                new Point(r.Right, r.Bottom),
            });

            return (path);
        }

        #endregion

        #region BottomTabPath

        /// <summary>
        /// Creates the Bottom tab path
        /// </summary>
        /// <returns>GraphicsPath</returns>
        private GraphicsPath BottomTabPath()
        {
            Rectangle r = TabItem.DisplayRectangle;
            r.Width -= 1;
            r.Height -= 1;

            // Allow for the TabStrip border

            if (TabItem.IsSelected == true)
            {
                r.Y -= 2;
                r.Height += 2;
            }

            // Create the path
            
            GraphicsPath path = new GraphicsPath();

            path.AddLines(new Point[]
            {
                new Point(r.Right, r.Top),
                new Point(r.Right, r.Bottom),
                new Point(r.X, r.Bottom), 
                new Point(r.X, r.Top),
            });

            return (path);
        }

        #endregion

        #region LeftTabPath

        /// <summary>
        /// Creates the Left tab path
        /// </summary>
        /// <returns>GraphicsPath</returns>
        private GraphicsPath LeftTabPath()
        {
            Rectangle r = TabItem.DisplayRectangle;
            r.Width -= 1;
            r.Height -= 1;

            // Allow for the TabStrip border

            if (TabItem.IsSelected == true)
                r.Width += 2;

            GraphicsPath path = new GraphicsPath();

            // Create the tab path

            path.AddLines(new Point[]
            {
                new Point(r.Right, r.Bottom),
                new Point(r.X, r.Bottom), 
                new Point(r.X, r.Top),
                new Point(r.Right, r.Top),
            });

            return (path);
        }

        #endregion

        #region RightTabPath

        /// <summary>
        /// Create the Right tab path
        /// </summary>
        /// <returns>GraphicsPath</returns>
        private GraphicsPath RightTabPath()
        {
            Rectangle r = TabItem.DisplayRectangle;
            r.Width -= 1;
            r.Height -= 1;

            // Allow for the TabStrip border

            if (TabItem.IsSelected == true)
            {
                r.X -= 2;
                r.Width += 2;
            }

            // Create the tab path
            
            GraphicsPath path = new GraphicsPath();

            path.AddLines(new Point[]
            {
                new Point(r.X, r.Top),
                new Point(r.Right, r.Top),
                new Point(r.Right, r.Bottom),
                new Point(r.X, r.Bottom), 
            });

            return (path);
        }

        #endregion

        #endregion
    }
}
#endif