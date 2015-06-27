#if FRAMEWORK20
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DevComponents.DotNetBar
{
    public class VS2008DocumentSuperTabItem : SuperTabItemBaseDisplay
    {
        #region Constants

        private const int SmRadius = 8;
        private const int LgRadius = 20;

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tabItem">Associated SuperTabItem</param>
        public VS2008DocumentSuperTabItem(SuperTabItem tabItem)
            : base(tabItem)
        {
        }

        #region ContentRectangle

        /// <summary>
        /// Returns the tab ContentRectangle
        /// </summary>
        /// <returns></returns>
        internal override Rectangle ContentRectangle()
        {
            Rectangle r = TabItem.DisplayRectangle;

            int n = TabStripItem.TabDisplay.TabOverlap;

            if (TabStripItem.IsVertical == true)
            {
                r.Y += n;
                r.Height -= n;
            }
            else
            {
                r.X += n;
                r.Width -= n;
            }

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
            
            int n = TabStripItem.TabDisplay.TabOverlap + TabStripItem.TabDisplay.TabSpacing;

            Rectangle ar = new Rectangle(r.X + n, r.Y, LgRadius, LgRadius);

            path.AddLine(r.X, r.Bottom, r.X, r.Bottom);
            path.AddArc(ar, 218, 52);

            ar = new Rectangle(r.Right - SmRadius, r.Y, SmRadius, SmRadius);
            path.AddArc(ar, 270, 90);

            path.AddLine(r.Right, r.Top + SmRadius, r.Right, r.Bottom);

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

            int n = TabStripItem.TabDisplay.TabOverlap;

            Rectangle ar = new
                Rectangle(r.Right - SmRadius, r.Bottom - SmRadius, SmRadius, SmRadius);

            path.AddLine(r.Right, r.Top, r.Right, r.Bottom - SmRadius);
            path.AddArc(ar, 0, 90);

            ar = new Rectangle(r.X + n, r.Bottom - LgRadius, LgRadius, LgRadius);
            path.AddArc(ar, 90, 62);

            path.AddLine(r.X, r.Top, r.X, r.Top);

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

            int n = TabStripItem.TabDisplay.TabOverlap;

            Rectangle ar = new
                Rectangle(r.X, r.Bottom - SmRadius, SmRadius, SmRadius);

            path.AddLine(r.Right, r.Bottom, r.X + SmRadius, r.Bottom);
            path.AddArc(ar, 90, 90);

            ar = new Rectangle(r.X, r.Y + n, LgRadius, LgRadius);
            path.AddArc(ar, 180, 62);

            path.AddLine(r.Right, r.Top, r.Right, r.Top);

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

            int n = TabStripItem.TabDisplay.TabOverlap;

            Rectangle ar = new
                Rectangle(r.Right - LgRadius, r.Top + n, LgRadius, LgRadius);

            path.AddLine(r.X, r.Top, r.X, r.Top);
            path.AddArc(ar, 360 - 62, 62);

            ar = new Rectangle(r.Right - SmRadius, r.Bottom - SmRadius, SmRadius, SmRadius);
            path.AddArc(ar, 0, 90);

            path.AddLine(r.Right - SmRadius, r.Bottom, r.X, r.Bottom);

            return (path);
        }

        #endregion

        #endregion
    }
}
#endif