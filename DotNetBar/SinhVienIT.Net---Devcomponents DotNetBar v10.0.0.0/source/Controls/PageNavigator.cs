#if FRAMEWORK20
using System;
using System.Runtime.InteropServices;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Controls
{
    /// <summary>
    /// Represents the PageNavigator control
    /// </summary>
    [ToolboxBitmap(typeof(PageNavigator), "PageNavigator.PageNavigator.ico"), ToolboxItem(true)]
    [DefaultEvent("ValueChanged"), ComVisible(false)] //, Designer(typeof(Design.PageNavigatorDesigner))]
    public class PageNavigator : BaseItemControl
    {
        #region Private Variables

        private PageNavigatorItem _PageNavItem;     // Navigation item

        #endregion

        #region Events

        /// <summary>
        /// Occurs when NavigateNextPage button is clicked
        /// </summary>
        [Description("Occurs when NavigateNextPage button is clicked.")]
        public event EventHandler NavigateNextPage;

        /// <summary>
        /// Occurs when NavigateToday button is clicked
        /// </summary>
        [Description("Occurs when NavigateToday button is clicked.")]
        public event EventHandler NavigateToday;

        /// <summary>
        /// Occurs when NavigatePreviousPage button is clicked
        /// </summary>
        [Description("Occurs when NavigatePreviousPage button is clicked.")]
        public event EventHandler NavigatePreviousPage;

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public PageNavigator()
        {
            _PageNavItem = new PageNavigatorItem();
            _PageNavItem.Style = eDotNetBarStyle.StyleManagerControlled;
            this.Size = _PageNavItem.Size;
            this.HostItem = _PageNavItem;

            HookEvents(true);
        }

        #region DefaultSize

        /// <summary>
        /// DefaultSize
        /// </summary>
        protected override Size DefaultSize
        {
            get
            {
                // Set a default size based upon
                // the current object layout orientation

                int m = SystemInformation.VerticalScrollBarWidth;
                int n = m * 3;

                if (_PageNavItem != null &&
                    _PageNavItem.Orientation == eOrientation.Horizontal)
                {
                    return (new Size(n, m));
                }

                return (new Size(m, n));
            }
        }
        #endregion

        #region Public properties

        #region Orientation

        /// <summary>
        /// Gets or sets the layout orientation. Default value is horizontal.
        /// </summary>
        [DefaultValue(eOrientation.Horizontal), Category("Appearance")]
        [Description("Indicates control layout orientation.")]
        public eOrientation Orientation
        {
            get { return (_PageNavItem.Orientation); }

            set
            {
                if (_PageNavItem.Orientation != value)
                {
                    _PageNavItem.Orientation = value;

                    this.RecalcLayout();
                }
            }
        }

        #endregion

        #region PreviousPageTooltip

        /// <summary>
        /// Gets or sets the tooltip for the PreviousPage button of the control
        /// </summary>
        [Browsable(true), DefaultValue(""), Localizable(true), Category("Appearance")]
        [Description("Indicates tooltip for the PreviousPage button of the control.")]
        public string PreviousPageTooltip
        {
            get { return (_PageNavItem.PreviousPageTooltip); }
            set { _PageNavItem.PreviousPageTooltip = value; }
        }

        #endregion

        #region TodayTooltip

        /// <summary>
        /// Gets or sets the tooltip for the Today button
        /// </summary>
        [Browsable(true), DefaultValue(""), Localizable(true), Category("Appearance")]
        [Description("Indicates tooltip for the TodayPage button of the control.")]
        public string TodayTooltip
        {
            get { return (_PageNavItem.TodayTooltip); }
            set { _PageNavItem.TodayTooltip = value; }
        }

        #endregion

        #region NextPageTooltip

        /// <summary>
        /// Gets or sets the tooltip for the NextPage button
        /// </summary>
        [Browsable(true), DefaultValue(""), Localizable(true), Category("Appearance")]
        [Description("Indicates tooltip for the NextPage button of the control.")]
        public string NextPageTooltip
        {
            get { return (_PageNavItem.NextPageTooltip); }
            set { _PageNavItem.NextPageTooltip = value; }
        }

        #endregion

        #region Style
        /// <summary>
        /// Gets/Sets the visual style for the control.
        /// </summary>
        [Browsable(false), DefaultValue(eDotNetBarStyle.StyleManagerControlled)]
        public override eDotNetBarStyle Style
        {
            get
            {
                return base.Style;
            }
            set
            {
                base.Style = value;
            }
        }
        #endregion

        #endregion

        #region HookEvents

        /// <summary>
        /// Hooks or unhooks our control events
        /// </summary>
        /// <param name="hook">true to hook, false to unhook</param>
        private void HookEvents(bool hook)
        {
            if (hook == true)
            {
                _PageNavItem.NavigatePreviousPage += PageNavNavigatePreviousPage;
                _PageNavItem.NavigateNextPage += PageNavNavigateNextPage;
                _PageNavItem.NavigateToday += PageNavNavigateToday;
            }
            else
            {
                _PageNavItem.NavigatePreviousPage -= PageNavNavigatePreviousPage;
                _PageNavItem.NavigateNextPage -= PageNavNavigateNextPage;
                _PageNavItem.NavigateToday -= PageNavNavigateToday;
            }
        }

        #endregion

        #region Event handling

        #region PageNav_NavigatePreviousPage

        /// <summary>
        /// Handles NavigatePreviousPage events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PageNavNavigatePreviousPage(object sender, EventArgs e)
        {
            OnNavigatePreviousPage();
        }

        /// <summary>
        /// Raises the NavigatePreviousPage event
        /// </summary>
        private void OnNavigatePreviousPage()
        {
            if (NavigatePreviousPage != null)
                NavigatePreviousPage(this, EventArgs.Empty);
        }

        #endregion

        #region PageNav_NavigateToday

        /// <summary>
        /// Handles NavigateToday events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PageNavNavigateToday(object sender, EventArgs e)
        {
            OnNavigateToday();
        }

        /// <summary>
        /// Raises the NavigateToday event
        /// </summary>
        private void OnNavigateToday()
        {
            if (NavigateToday != null)
                NavigateToday(this, EventArgs.Empty);
        }

        #endregion

        #region PageNav_NavigateNextPage

        /// <summary>
        /// Handles NavigateNextPage events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PageNavNavigateNextPage(object sender, EventArgs e)
        {
            OnNavigateNextPage();
        }

        /// <summary>
        /// Raises the NavigateNextPage event
        /// </summary>
        private void OnNavigateNextPage()
        {
            if (NavigateNextPage != null)
                NavigateNextPage(this, EventArgs.Empty);
        }

        #endregion

        #endregion

        #region RecalcLayout

        /// <summary>
        /// Forces the button to perform internal layout.
        /// </summary>
        public override void RecalcLayout()
        {
            base.RecalcLayout();
        }

        #endregion

        #region Dispose

        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing == true)
                HookEvents(false);

            base.Dispose(disposing);
        }

        #endregion
    }

    #region enums

    /// <summary>
    /// PageNavigator buttons
    /// </summary>
    public enum PageNavigatorButton
    {
        /// <summary>
        /// Previous page
        /// </summary>
        PreviousPage,

        /// <summary>
        /// Today
        /// </summary>
        Today,

        /// <summary>
        /// Next page
        /// </summary>
        NextPage
    }

    #endregion
}
#endif