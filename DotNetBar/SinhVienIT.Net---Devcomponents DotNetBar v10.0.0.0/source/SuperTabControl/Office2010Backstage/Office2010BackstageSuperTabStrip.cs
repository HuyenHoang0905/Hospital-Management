#if FRAMEWORK20
using System.Drawing;
using DevComponents.DotNetBar.Rendering;
using DevComponents.UI.ContentManager;

namespace DevComponents.DotNetBar
{
    internal class Office2010BackstageSuperTabStrip : SuperTabStripBaseDisplay
    {
        #region Constants

        private const int SelectedPadding = 5;
        private const int HvTabOverLap = -1;

        #endregion

        public Office2010BackstageSuperTabStrip(SuperTabStripItem tabStripItem)
            : base(tabStripItem)
        {
            tabStripItem.ControlBox.MenuBox.Style = eDotNetBarStyle.Office2010;
        }

        #region Internal properties

        #region SelectedPaddingWidth

        /// <summary>
        /// SelectedPaddingWidth
        /// </summary>
        internal override int SelectedPaddingWidth
        {
            get { return (SelectedPadding); }
        }

        #endregion

        #endregion

        #region NextBlockPosition (LayoutManager)

        /// <summary>
        /// Gets the Layout manager NextBlockPosition
        /// </summary>
        /// <param name="e"></param>
        protected override void NextBlockPosition(LayoutManagerPositionEventArgs e)
        {
            int n = Tabs.IndexOf((BaseItem)e.Block);

            if (n >= 0 && n + 1 < Tabs.Count)
            {
                SuperTabItem tab1 = Tabs[n] as SuperTabItem;

                if (tab1 != null)
                {
                    SuperTabItem tab2 = Tabs[n + 1] as SuperTabItem;

                    if (tab2 != null)
                    {
                        e.NextPosition = e.CurrentPosition;

                        if (TabStripItem.IsVertical == true)
                            e.NextPosition.Y += (e.Block.Bounds.Height - HvTabOverLap);
                        else
                            e.NextPosition.X += (e.Block.Bounds.Width - HvTabOverLap);

                        e.Cancel = true;
                    }
                }
            }
        }

        #endregion        

        #region NextBlockPosition (PromoteSelTab)

        /// <summary>
        /// Gets the "PromoteSelTab" NextBlockPosition
        /// </summary>
        /// <param name="item"></param>
        /// <param name="vItem"></param>
        /// <returns></returns>
        internal override Rectangle NextBlockPosition(BaseItem item, BaseItem vItem)
        {
            Rectangle r = base.NextBlockPosition(item, vItem);

            if (item is SuperTabItem && vItem is SuperTabItem)
            {
                if (TabStripItem.IsVertical == true)
                    r.Y -= HvTabOverLap;
                else
                    r.X -= HvTabOverLap;
            }

            return (r);
        }

        #endregion

        #region DrawBackground

        /// <summary>
        /// Draws the background
        /// </summary>
        /// <param name="p"></param>
        /// <param name="ct"></param>
        protected override void DrawBackground(ItemPaintArgs p, SuperTabColorTable ct)
        {
            Graphics g = p.Graphics;
            Rectangle r = TabStripItem.DisplayRectangle;

            int angle = ct.Background.GradientAngle;

            switch (TabStripItem.TabAlignment)
            {
                case eTabStripAlignment.Bottom:
                    r.Y -= 1;
                    break;

                case eTabStripAlignment.Right:
                    r.X -= 1;
                    break;

                case eTabStripAlignment.Top:
                    angle += 180;
                    break;
            }

            using (Brush lbr = ct.Background.GetBrush(r, angle))
            {
                if (lbr != null)
                    g.FillRectangle(lbr, r);
            }
        }

        #endregion
    }
}
#endif