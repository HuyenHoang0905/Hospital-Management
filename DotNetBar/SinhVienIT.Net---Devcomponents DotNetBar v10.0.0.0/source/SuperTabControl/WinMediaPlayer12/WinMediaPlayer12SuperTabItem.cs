#if FRAMEWORK20
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DevComponents.DotNetBar
{
    public class WinMediaPlayer12SuperTabItem : SuperTabItemBaseDisplay
    {
        #region Constants

        private const int Radius = 4;

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tabItem">Associated SuperTabItem</param>
        public WinMediaPlayer12SuperTabItem(SuperTabItem tabItem)
            : base(tabItem)
        {
        }

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
            GraphicsPath path = new GraphicsPath();

            Rectangle r = TabItem.DisplayRectangle;
            r.Width -= 1;
            r.Height -= 1;

            // Allow for the TabStrip border

            if (TabItem.IsSelected == true)
                r.Height += 2;

            // Create the path
            
            int n = TabStripItem.TabDisplay.TabSpacing;

            Rectangle ar = new Rectangle(r.X, r.Y, Radius, Radius);

            path.AddLine(r.X, r.Bottom, r.X, r.Top + Radius);
            path.AddArc(ar, 180, 90);

            path.AddLine(r.X + Radius, r.Top, r.Right - n - Radius, r.Top);

            ar.X = r.Right - n - Radius;
            path.AddArc(ar, 270, 90);

            path.AddLine(r.Right - n, r.Top + Radius, r.Right - n, r.Bottom);

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
            GraphicsPath path = new GraphicsPath();

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

            Rectangle ar = new
                Rectangle(r.Right - Radius, r.Bottom - Radius, Radius, Radius);

            path.AddLine(r.Right, r.Top, r.Right, r.Bottom - Radius);
            path.AddArc(ar, 0, 90);

            path.AddLine(r.Right - Radius, r.Bottom, r.X + Radius, r.Bottom);

            ar.X = r.X;
            path.AddArc(ar, 90, 90);

            path.AddLine(r.X, r.Bottom - Radius, r.X, r.Top);

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
            GraphicsPath path = new GraphicsPath();

            Rectangle r = TabItem.DisplayRectangle;
            r.Width -= 1;
            r.Height -= 1;

            // Allow for the TabStrip border

            if (TabItem.IsSelected == true)
                r.Width += 2;

            // Create the tab path

            Rectangle ar = new
                Rectangle(r.X, r.Bottom - Radius, Radius, Radius);

            path.AddLine(r.Right, r.Bottom, r.X + Radius, r.Bottom);
            path.AddArc(ar, 90, 90);

            path.AddLine(r.X, r.Bottom - Radius, r.X, r.Top + Radius);

            ar.Y = r.Y;
            path.AddArc(ar, 180, 90);

            path.AddLine(r.X + Radius, r.Top, r.Right, r.Top);

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
            GraphicsPath path = new GraphicsPath();

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

            Rectangle ar = new
                Rectangle(r.Right - Radius, r.Top, Radius, Radius);

            path.AddLine(r.X, r.Top, r.Right - Radius, r.Top);
            path.AddArc(ar, 270, 90);

            path.AddLine(r.Right, r.Top + Radius, r.Right, r.Bottom - Radius);

            ar.Y = r.Bottom - Radius;
            path.AddArc(ar, 0, 90);

            path.AddLine(r.Right - Radius, r.Bottom, r.X, r.Bottom);

            return (path);
        }

        #endregion

        #endregion
    }
}
#endif