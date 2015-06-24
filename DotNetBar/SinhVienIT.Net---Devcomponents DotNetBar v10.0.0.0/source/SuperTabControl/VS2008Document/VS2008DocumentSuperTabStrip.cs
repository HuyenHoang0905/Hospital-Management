#if FRAMEWORK20
using System.Drawing;
using DevComponents.UI.ContentManager;

namespace DevComponents.DotNetBar
{
    internal class VS2008DocumentSuperTabStrip : SuperTabStripBaseDisplay
    {
        #region Constants

        private const int HvTabSpacing = 0;
        private const int HvTabOverLap = 20;

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tabStripItem">Associated SuperTabStripItem</param>
        public VS2008DocumentSuperTabStrip(SuperTabStripItem tabStripItem)
            : base(tabStripItem)
        {
        }

        #region Internal properties

        #region TabOverlap

        /// <summary>
        /// Gets the TabOverlap
        /// </summary>
        internal override int TabOverlap
        {
            get { return (HvTabOverLap); }
        }

        #endregion

        #region TabSpacing

        /// <summary>
        /// Gets the TabSpacing
        /// </summary>
        internal override int TabSpacing
        {
            get { return (HvTabSpacing); }
        }

        #endregion

        #region TabOverlapLeft

        /// <summary>
        /// Gets the TabOverlapLeft
        /// </summary>
        internal override bool TabOverlapLeft
        {
            get { return (true); }
        }

        #endregion

        #region TabLayoutOffset

        /// <summary>
        /// Gets the TabLayoutOffset
        /// </summary>
        internal override Size TabLayoutOffset
        {
            get { return (new Size(3, 2)); }
        }

        #endregion

        #region MinTabSize

        /// <summary>
        /// Gets the MinTabSize
        /// </summary>
        internal override Size MinTabSize
        {
            get
            {
                if (TabStripItem.IsVertical == true)
                    return (new Size(33, 35));
                
                return (new Size(33, 16));
            }
        }

        #endregion

        #endregion

        #region NextBlockPosition (LayoutManager)

        /// <summary>
        /// Gets the LayoutManager NextBlockPosition
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

        #region NextBlockPosition (PromoteSelectedTab)

        /// <summary>
        /// Gets the PromoteSelectedTab NextBlockPosition
        /// </summary>
        /// <param name="item"></param>
        /// <param name="vItem"></param>
        /// <returns></returns>
        internal override Rectangle NextBlockPosition(BaseItem item, BaseItem vItem)
        {
            Rectangle r = base.NextBlockPosition(item, vItem);

            if (item is SuperTabItem && vItem is SuperTabItem)
            {
                if (IsVertical == true)
                    r.Y -= HvTabOverLap;
                else
                    r.X -= HvTabOverLap;
            }

            return (r);
        }

        #endregion

    }
}
#endif