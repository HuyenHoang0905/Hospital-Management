#if FRAMEWORK20
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DevComponents.DotNetBar
{
    public class OneNote2007SuperTabItem : SuperTabItemBaseDisplay
    {
        #region Constants

        private const int Radius = 10;

        #endregion

        /// <summary>
        /// Constructor for OneNote2007 style SuperTabItem base display
        /// </summary>
        /// <param name="tabItem">Associated SuperTabItem</param>
        public OneNote2007SuperTabItem(SuperTabItem tabItem)
            : base(tabItem)
        {
        }

        #region ContentRectangle

        /// <summary>
        /// Calculates the Content Rectangle for the tab
        /// </summary>
        /// <returns>Content Rectangle</returns>
        internal override Rectangle ContentRectangle()
        {
            Rectangle r = TabItem.DisplayRectangle;

            int n = TabStripItem.TabDisplay.TabOverlap + TabStripItem.TabDisplay.TabSpacing;

            if (TabStripItem.IsVertical == true)
                r.Height -= n;
            else
                r.Width -= n;

            return (r);
        }

        #endregion

        #region TabItemPath

        /// <summary>
        /// Creates the tab item GraphicsPath
        /// </summary>
        /// <returns>Tab path</returns>
        internal override GraphicsPath TabItemPath()
        {
            GraphicsPath path = base.TabItemPath();

            if (path != null)
                return (path);

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

            int n = TabStripItem.TabDisplay.TabOverlap + TabStripItem.TabDisplay.TabSpacing * 2;

            Rectangle ar = new Rectangle(r.X, r.Y, Radius, Radius);

            path.AddLine(r.X, r.Bottom, r.X, r.Top + Radius);
            path.AddArc(ar, 180, 90);
            path.AddLine(r.Right - n, r.Top, r.Right, r.Bottom);

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

            int n = TabStripItem.TabDisplay.TabOverlap + TabStripItem.TabDisplay.TabSpacing * 2;

            Rectangle ar = new
                Rectangle(r.X, r.Bottom - Radius, Radius, Radius);

            path.AddLine(r.Right, r.Top, r.Right - n, r.Bottom);
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
            Rectangle r = TabItem.DisplayRectangle;
            r.Width -= 1;
            r.Height -= 1;

            // Allow for the TabStrip border

            if (TabItem.IsSelected == true)
                r.Width += 2;

            // Create the tab path

            GraphicsPath path = new GraphicsPath();

            int n = TabStripItem.TabDisplay.TabOverlap + TabStripItem.TabDisplay.TabSpacing;

            Rectangle ar = new
                Rectangle(r.X, r.Top, Radius, Radius);

            path.AddLine(r.Right, r.Top, r.X + Radius, r.Top);
            path.AddArc(ar, -90, -90);

            path.AddLine(r.X, r.Top + Radius, r.X, r.Bottom - n);
            path.AddLine(r.X, r.Bottom - n, r.Right, r.Bottom);

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

            int n = TabStripItem.TabDisplay.TabOverlap + TabStripItem.TabDisplay.TabSpacing;

            if (TabStripItem.HorizontalText == false)
                n += TabStripItem.TabDisplay.TabSpacing;

            Rectangle ar = new
                Rectangle(r.Right - Radius, r.Top, Radius, Radius);

            path.AddLine(r.X, r.Top, r.Right - Radius, r.Top);
            path.AddArc(ar, 270, 90);

            path.AddLine(r.Right, r.Top + Radius, r.Right, r.Bottom - n);
            path.AddLine(r.Right, r.Bottom - n, r.X, r.Bottom);

            return (path);
        }

        #endregion

        #endregion
    }
}
#endif