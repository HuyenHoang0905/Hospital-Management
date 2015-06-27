using System;
using System.Text;

namespace DevComponents.DotNetBar.Rendering
{
    /// <summary>
    /// Represents bases class that defines a renderer.
    /// </summary>
    public abstract class BaseRenderer
    {
        #region Events
        /// <summary>
        /// Occurs when KeyTip is rendered.
        /// </summary>
        public event KeyTipsRendererEventHandler RenderKeyTips;

        /// <summary>
        /// Occurs when ribbon tab group is rendered.
        /// </summary>
        public event RibbonTabGroupRendererEventHandler RenderRibbonTabGroup;

        /// <summary>
        /// Occurs when ItemContainer is rendered.
        /// </summary>
        public event ItemContainerRendererEventHandler RenderItemContainer;

        /// <summary>
        /// Occurs when separator is drawn for an item inside of ItemContainer.
        /// </summary>
        public event ItemContainerSeparatorRendererEventHandler RenderItemContainerSeparator;

        /// <summary>
        /// Occurs when ButtonItem is rendered.
        /// </summary>
        public event ButtonItemRendererEventHandler RenderButtonItem;

        /// <summary>
        /// Occurs when RibbonTabItem is rendered.
        /// </summary>
        public event RibbonTabItemRendererEventHandler RenderRibbonTabItem;

        /// <summary>
        /// Occurs when docked or floating toolbar is rendered.
        /// </summary>
        public event ToolbarRendererEventHandler RenderToolbarBackground;

        /// <summary>
        /// Occurs when popup toolbar is rendered.
        /// </summary>
        public event ToolbarRendererEventHandler RenderPopupToolbarBackground;

        /// <summary>
        /// Occurs when dialog launcher button on ribbon bar is rendered.
        /// </summary>
        public event RibbonBarRendererEventHandler RenderRibbonDialogLauncher;

        /// <summary>
        /// Occurs when Ribbon Control background is rendered.
        /// </summary>
        public event RibbonControlRendererEventHandler RenderRibbonControlBackground;

        /// <summary>
        /// Occurs when form caption text on ribbon control is rendered.
        /// </summary>
        public event RibbonControlRendererEventHandler RenderRibbonFormCaptionText;

        /// <summary>
        /// Occurs when Quick Access Toolbar background is rendered.
        /// </summary>
        public event RibbonControlRendererEventHandler RenderQuickAccessToolbarBackground;

        ///// <summary>
        ///// Occurs when ribbon bar background is rendered.
        ///// </summary>
        //public event RibbonBarRendererEventHandler RenderRibbonBarBackground;

        ///// <summary>
        ///// Occurs when ribbon bar title is rendered.
        ///// </summary>
        //public event RibbonBarRendererEventHandler RenderRibbonBarTitle;

        /// <summary>
        /// Occurs when ColorItem is rendered.
        /// </summary>
        public event ColorItemRendererEventHandler RenderColorItem;

        /// <summary>
        /// Occurs when SystemCaptionItem is rendered.
        /// </summary>
        public event SystemCaptionItemRendererEventHandler RenderSystemCaptionItem;

        /// <summary>
        /// Occurs when MdiSystemItem is rendered.
        /// </summary>
        public event MdiSystemItemRendererEventHandler RenderMdiSystemItem;

        /// <summary>
        /// Occurs when form caption is background is being rendered.
        /// </summary>
        public event FormCaptionRendererEventHandler RenderFormCaptionBackground;

        /// <summary>
        /// Occurs when quick access toolbar overflow item is being rendered.
        /// </summary>
        public event QatOverflowItemRendererEventHandler RenderQatOverflowItem;

        /// <summary>
        /// Occurs when quick access toolbar customize item is being rendered.
        /// </summary>
        public event QatCustomizeItemRendererEventHandler RenderQatCustomizeItem;

        /// <summary>
        /// Occurs when CheckBoxItem is being rendered.
        /// </summary>
        public event CheckBoxItemRendererEventHandler RenderCheckBoxItem;

        /// <summary>
        /// Occurs when ProgressBarItem is being rendered.
        /// </summary>
        public event ProgressBarItemRendererEventHandler RenderProgressBarItem;

        /// <summary>
        /// Occurs when Navigation pane button background is being rendered.
        /// </summary>
        public event NavPaneRendererEventHandler RenderNavPaneButtonBackground;

        /// <summary>
        /// Occurs when Slider item is being rendered.
        /// </summary>
        public event SliderItemRendererEventHandler RenderSliderItem;

        /// <summary>
        /// Occurs when SideBar control is being rendered.
        /// </summary>
        public event SideBarRendererEventHandler RenderSideBar;
        /// <summary>
        /// Occurs when SideBarPanelItem control is being rendered.
        /// </summary>
        public event SideBarPanelItemRendererEventHandler RenderSideBarPanelItem;

        /// <summary>
        /// Occurs when CrumbBarItemView is rendered.
        /// </summary>
        public event ButtonItemRendererEventHandler RenderCrumbBarItemView;
        /// <summary>
        /// Occurs when CrumbBarOverflowButton is rendered.
        /// </summary>
        public event ButtonItemRendererEventHandler RenderCrumbBarOverflowItem;

        /// <summary>
        /// Occurs when Slider item is being rendered.
        /// </summary>
        public event SwitchButtonRendererEventHandler RenderSwitchButton;
        #endregion

        #region Internal Implementation
        /// <summary>
        /// Raises RenderKeyTips event.
        /// </summary>
        /// <param name="e">Provides context information.</param>
        protected virtual void OnRenderKeyTips(KeyTipsRendererEventArgs e)
        {
            if (RenderKeyTips != null)
                RenderKeyTips(this, e);
        }
        /// <summary>
        /// Draws KeyTip for an object. If you need to provide custom rendering this is the method that you should override in your custom rendered. If you
        /// do not want default rendering to occur do not call the base implementation. You can call OnRenderKeyTips method so events can occur.
        /// </summary>
        /// <param name="e">Provides context information.</param>
        public virtual void DrawKeyTips(KeyTipsRendererEventArgs e)
        {
            OnRenderKeyTips(e);
        }

        /// <summary>
        /// Raises RenderRibbonTabGroup event.
        /// </summary>
        /// <param name="e">Provides context information.</param>
        protected virtual void OnRenderRibbonTabGroup(RibbonTabGroupRendererEventArgs e)
        {
            if (RenderRibbonTabGroup != null)
                RenderRibbonTabGroup(this, e);
        }
        /// <summary>
        /// Draws ribbon tab group. If you need to provide custom rendering this is the method that you should override in your custom rendered. If you
        /// do not want default rendering to occur do not call the base implementation. You can call OnRenderRibbonTabGroup method so events can occur.
        /// </summary>
        /// <param name="e">Provides context information.</param>
        public virtual void DrawRibbonTabGroup(RibbonTabGroupRendererEventArgs e)
        {
            OnRenderRibbonTabGroup(e);
        }

        /// <summary>
        /// Raises RenderItemContainer event.
        /// </summary>
        /// <param name="e">Provides context information.</param>
        protected virtual void OnRenderItemContainer(ItemContainerRendererEventArgs e)
        {
            if (RenderItemContainer != null)
                RenderItemContainer(this, e);
        }

        /// <summary>
        /// Draws the separator for an item inside of item container. If you need to provide custom rendering this is the method that you should override in your custom rendered. If you
        /// do not want default rendering to occur do not call the base implementation. You can call OnRenderItemContainerSeparator method so events can occur.
        /// </summary>
        /// <param name="e">Provides context information.</param>
        public virtual void DrawItemContainerSeparator(ItemContainerSeparatorRendererEventArgs e)
        {
            OnRenderItemContainerSeparator(e);
        }

        /// <summary>
        /// Raises RenderItemContainer event.
        /// </summary>
        /// <param name="e">Provides context information.</param>
        protected virtual void OnRenderItemContainerSeparator(ItemContainerSeparatorRendererEventArgs e)
        {
            if (RenderItemContainerSeparator != null)
                RenderItemContainerSeparator(this, e);
        }
        /// <summary>
        /// Draws item container. If you need to provide custom rendering this is the method that you should override in your custom rendered. If you
        /// do not want default rendering to occur do not call the base implementation. You can call OnRenderItemContainer method so events can occur.
        /// </summary>
        /// <param name="e">Provides context information.</param>
        public virtual void DrawItemContainer(ItemContainerRendererEventArgs e)
        {
            OnRenderItemContainer(e);
        }

        /// <summary>
        /// Raises RenderButtonItem event.
        /// </summary>
        /// <param name="e">Provides context information.</param>
        protected virtual void OnRenderButtonItem(ButtonItemRendererEventArgs e)
        {
            if (RenderButtonItem != null)
                RenderButtonItem(this, e);
        }
        /// <summary>
        /// Draws ButtonItem. If you need to provide custom rendering this is the method that you should override in your custom rendered. If you
        /// do not want default rendering to occur do not call the base implementation. You can call OnRenderButtonItem method so events can occur.
        /// </summary>
        /// <param name="e">Provides context information.</param>
        public virtual void DrawButtonItem(ButtonItemRendererEventArgs e)
        {
            OnRenderButtonItem(e);
        }

        /// <summary>
        /// Raises RenderRibbonTabItem event.
        /// </summary>
        /// <param name="e">Provides context information.</param>
        protected virtual void OnRenderRibbonTabItem(RibbonTabItemRendererEventArgs e)
        {
            if (RenderRibbonTabItem != null)
                RenderRibbonTabItem(this, e);
        }
        /// <summary>
        /// Draws RibbonTabItem. If you need to provide custom rendering this is the method that you should override in your custom rendered. If you
        /// do not want default rendering to occur do not call the base implementation. You can call OnRenderRibbonTabItem method so events can occur.
        /// </summary>
        /// <param name="e">Provides context information.</param>
        public virtual void DrawRibbonTabItem(RibbonTabItemRendererEventArgs e)
        {
            OnRenderRibbonTabItem(e);
        }

        /// <summary>
        /// Raises RenderToolbarBackground event.
        /// </summary>
        /// <param name="e">Provides context information.</param>
        protected virtual void OnRenderToolbarBackground(ToolbarRendererEventArgs e)
        {
            if (RenderToolbarBackground != null)
                RenderToolbarBackground(this, e);
        }
        /// <summary>
        /// Draws docked or floating toolbar background. If you need to provide custom rendering this is the method that you should override in your custom rendered. If you
        /// do not want default rendering to occur do not call the base implementation. You can call OnRenderToolbarBackground method so events can occur.
        /// </summary>
        /// <param name="e">Provides context information.</param>
        public virtual void DrawToolbarBackground(ToolbarRendererEventArgs e)
        {
            OnRenderToolbarBackground(e);
        }

        /// <summary>
        /// Raises RenderPopupToolbarBackground event.
        /// </summary>
        /// <param name="e">Provides context information.</param>
        protected virtual void OnRenderPopupToolbarBackground(ToolbarRendererEventArgs e)
        {
            if (RenderPopupToolbarBackground != null)
                RenderPopupToolbarBackground(this, e);
        }
        /// <summary>
        /// Draws popup toolbar background. If you need to provide custom rendering this is the method that you should override in your custom rendered. If you
        /// do not want default rendering to occur do not call the base implementation. You can call OnRenderPopupToolbarBackground method so events can occur.
        /// </summary>
        /// <param name="e">Provides context information.</param>
        public virtual void DrawPopupToolbarBackground(ToolbarRendererEventArgs e)
        {
            OnRenderPopupToolbarBackground(e);
        }

        /// <summary>
        /// Raises RenderRibbonDialogLauncher event.
        /// </summary>
        /// <param name="e">Provides context information.</param>
        protected virtual void OnRenderRibbonDialogLauncher(RibbonBarRendererEventArgs e)
        {
            if (RenderRibbonDialogLauncher != null)
                RenderRibbonDialogLauncher(this, e);
        }
        /// <summary>
        /// Draws ribbon bar dialog launcher button. If you need to provide custom rendering this is the method that you should override in your custom rendered. If you
        /// do not want default rendering to occur do not call the base implementation. You can call OnRenderRibbonDialogLauncher method so events can occur.
        /// </summary>
        /// <param name="e">Provides context information.</param>
        public virtual void DrawRibbonDialogLauncher(RibbonBarRendererEventArgs e)
        {
            OnRenderRibbonDialogLauncher(e);
        }

        /// <summary>
        /// Raises RenderColorItem event event.
        /// </summary>
        /// <param name="e">Provides context information.</param>
        protected virtual void OnRenderColorItem(ColorItemRendererEventArgs e)
        {
            if (RenderColorItem != null)
                RenderColorItem(this, e);
        }
        /// <summary>
        /// Draws ColorItem. If you need to provide custom rendering this is the method that you should override in your custom rendered. If you
        /// do not want default rendering to occur do not call the base implementation. You can call OnRenderColorItem method so events can occur.
        /// </summary>
        /// <param name="e">Provides context information.</param>
        public virtual void DrawColorItem(ColorItemRendererEventArgs e)
        {
            OnRenderColorItem(e);
        }

        ///// <summary>
        ///// Raises RenderRibbonBarBackground event.
        ///// </summary>
        ///// <param name="e">Provides context information.</param>
        //protected virtual void OnRenderRibbonBarBackground(RibbonBarRendererEventArgs e)
        //{
        //    if (RenderRibbonBarBackground != null)
        //        RenderRibbonBarBackground(this, e);
        //}
        ///// <summary>
        ///// Draws ribbon bar background. If you need to provide custom rendering this is the method that you should override in your custom rendered. If you
        ///// do not want default rendering to occur do not call the base implementation. You can call OnRenderRibbonBarBackground method so events can occur.
        ///// </summary>
        ///// <param name="e">Provides context information.</param>
        //public virtual void DrawRibbonBarBackground(RibbonBarRendererEventArgs e)
        //{
        //    OnRenderRibbonBarBackground(e);
        //}

        ///// <summary>
        ///// Raises RenderRibbonBarTitle event.
        ///// </summary>
        ///// <param name="e">Provides context information.</param>
        //protected virtual void OnRenderRibbonBarTitle(RibbonBarRendererEventArgs e)
        //{
        //    if (RenderRibbonBarTitle != null)
        //        RenderRibbonBarTitle(this, e);
        //}
        ///// <summary>
        ///// Draws ribbon bar title. If you need to provide custom rendering this is the method that you should override in your custom rendered. If you
        ///// do not want default rendering to occur do not call the base implementation. You can call OnRenderRibbonBarTitle method so events can occur.
        ///// </summary>
        ///// <param name="e">Provides context information.</param>
        //public virtual void DrawRibbonBarTitle(RibbonBarRendererEventArgs e)
        //{
        //    OnRenderRibbonBarTitle(e);
        //}

        /// <summary>
        /// Raises RenderRibbonControlBackground event event.
        /// </summary>
        /// <param name="e">Provides context information.</param>
        protected virtual void OnRenderRibbonControlBackground(RibbonControlRendererEventArgs e)
        {
            if (RenderRibbonControlBackground != null)
                RenderRibbonControlBackground(this, e);
        }
        /// <summary>
        /// Draws the background of the Ribbon Control. If you need to provide custom rendering this is the method that you should override in your custom rendered. If you
        /// do not want default rendering to occur do not call the base implementation. You can call OnRenderRibbonControlBackground method so events can occur.
        /// </summary>
        /// <param name="e">Provides context information.</param>
        public virtual void DrawRibbonControlBackground(RibbonControlRendererEventArgs e)
        {
            OnRenderRibbonControlBackground(e);
        }

        /// <summary>
        /// Raises RenderSystemCaptionItem event event.
        /// </summary>
        /// <param name="e">Provides context information.</param>
        protected virtual void OnRenderSystemCaptionItem(SystemCaptionItemRendererEventArgs e)
        {
            if (RenderSystemCaptionItem != null)
                RenderSystemCaptionItem(this, e);
        }
        /// <summary>
        /// Draws the SystemCaptionItem. If you need to provide custom rendering this is the method that you should override in your custom rendered. If you
        /// do not want default rendering to occur do not call the base implementation. You can call OnRenderSystemCaptionItem method so events can occur.
        /// </summary>
        /// <param name="e">Provides context information.</param>
        public virtual void DrawSystemCaptionItem(SystemCaptionItemRendererEventArgs e)
        {
            OnRenderSystemCaptionItem(e);
        }

        /// <summary>
        /// Raises RenderRibbonFormCaptionText event event.
        /// </summary>
        /// <param name="e">Provides context information.</param>
        protected virtual void OnRenderRibbonFormCaptionText(RibbonControlRendererEventArgs e)
        {
            if (RenderRibbonFormCaptionText != null)
                RenderRibbonFormCaptionText(this, e);
        }
        /// <summary>
        /// Draws the form caption text for the Ribbon Control. If you need to provide custom rendering this is the method that you should override in your custom rendered. If you
        /// do not want default rendering to occur do not call the base implementation. You can call OnRenderRibbonFormCaptionText method so events can occur.
        /// </summary>
        /// <param name="e">Provides context information.</param>
        public virtual void DrawRibbonFormCaptionText(RibbonControlRendererEventArgs e)
        {
            OnRenderRibbonFormCaptionText(e);
        }

        /// <summary>
        /// Raises RenderQuickAccessToolbarBackground event event.
        /// </summary>
        /// <param name="e">Provides context information.</param>
        protected virtual void OnRenderQuickAccessToolbarBackground(RibbonControlRendererEventArgs e)
        {
            if (RenderQuickAccessToolbarBackground != null)
                RenderQuickAccessToolbarBackground(this, e);
        }
        /// <summary>
        /// Draws the background of Quick Access Toolbar on Ribbon Control. If you need to provide custom rendering this is the method that you should override in your custom rendered. If you
        /// do not want default rendering to occur do not call the base implementation. You can call OnRenderQuickAccessToolbarBackground method so events can occur.
        /// </summary>
        /// <param name="e">Provides context information.</param>
        public virtual void DrawQuickAccessToolbarBackground(RibbonControlRendererEventArgs e)
        {
            OnRenderQuickAccessToolbarBackground(e);
        }

        /// <summary>
        /// Raises RenderMdiSystemItem event.
        /// </summary>
        /// <param name="e">Provides context information.</param>
        protected virtual void OnRenderMdiSystemItem(MdiSystemItemRendererEventArgs e)
        {
            if (RenderMdiSystemItem != null)
                RenderMdiSystemItem(this, e);
        }
        /// <summary>
        /// Draws the MdiSystemItem. If you need to provide custom rendering this is the method that you should override in your custom rendered. If you
        /// do not want default rendering to occur do not call the base implementation. You can call OnRenderMdiSystemItem method so events can occur.
        /// </summary>
        /// <param name="e">Provides context information.</param>
        public virtual void DrawMdiSystemItem(MdiSystemItemRendererEventArgs e)
        {
            OnRenderMdiSystemItem(e);
        }
        #endregion

        #region Form Caption
        /// <summary>
        /// Raises RenderFormCaptionBackground event.
        /// </summary>
        /// <param name="e">Provides context information.</param>
        protected virtual void OnRenderFormCaptionBackground(FormCaptionRendererEventArgs e)
        {
            if (RenderFormCaptionBackground != null)
                RenderFormCaptionBackground(this, e);
        }
        /// <summary>
        /// Draws the form caption background. If you need to provide custom rendering this is the method that you should override in your custom rendered. If you
        /// do not want default rendering to occur do not call the base implementation. You can call OnRenderFormCaptionBackground method so events can occur.
        /// </summary>
        /// <param name="e">Provides context information.</param>
        public virtual void DrawFormCaptionBackground(FormCaptionRendererEventArgs e)
        {
            OnRenderFormCaptionBackground(e);
        }
        #endregion

        #region QAT Overflow/Customize Item
        /// <summary>
        /// Raises RenderQatOverflowItem event.
        /// </summary>
        /// <param name="e">Provides context information.</param>
        protected virtual void OnRenderQatOverflowItem(QatOverflowItemRendererEventArgs e)
        {
            if (RenderQatOverflowItem != null)
                RenderQatOverflowItem(this, e);
        }
        /// <summary>
        /// Draws the Quick Access Toolbar Overflow item. If you need to provide custom rendering this is the method that you should override in your custom rendered. If you
        /// do not want default rendering to occur do not call the base implementation. You can call OnRenderQatOverflowItem method so events can occur.
        /// </summary>
        /// <param name="e">Provides context information.</param>
        public virtual void DrawQatOverflowItem(QatOverflowItemRendererEventArgs e)
        {
            OnRenderQatOverflowItem(e);
        }

        /// <summary>
        /// Raises RenderQatCustomizeItem event.
        /// </summary>
        /// <param name="e">Provides context information.</param>
        protected virtual void OnRenderQatCustomizeItem(QatCustomizeItemRendererEventArgs e)
        {
            if (RenderQatCustomizeItem != null)
                RenderQatCustomizeItem(this, e);
        }
        /// <summary>
        /// Draws the Quick Access Toolbar Customize Item. If you need to provide custom rendering this is the method that you should override in your custom rendered. If you
        /// do not want default rendering to occur do not call the base implementation. You can call OnRenderQatCustomizeItem method so events can occur.
        /// </summary>
        /// <param name="e">Provides context information.</param>
        public virtual void DrawQatCustomizeItem(QatCustomizeItemRendererEventArgs e)
        {
            OnRenderQatCustomizeItem(e);
        }
        #endregion

        #region CheckBoxItem
        /// <summary>
        /// Raises RenderCheckBoxItem event.
        /// </summary>
        /// <param name="e">Provides context information.</param>
        protected virtual void OnRenderCheckBoxItem(CheckBoxItemRenderEventArgs e)
        {
            if (RenderCheckBoxItem != null)
                RenderCheckBoxItem(this, e);
        }
        /// <summary>
        /// Draws the CheckBoxItem. If you need to provide custom rendering this is the method that you should override in your custom rendered. If you
        /// do not want default rendering to occur do not call the base implementation. You can call OnRenderCheckBoxItem method so events can occur.
        /// </summary>
        /// <param name="e">Provides context information.</param>
        public virtual void DrawCheckBoxItem(CheckBoxItemRenderEventArgs e)
        {
            OnRenderCheckBoxItem(e);
        }
        #endregion

        #region ProgressBarItem
        /// <summary>
        /// Raises RenderCheckBoxItem event.
        /// </summary>
        /// <param name="e">Provides context information.</param>
        protected virtual void OnRenderProgressBarItem(ProgressBarItemRenderEventArgs e)
        {
            if (RenderProgressBarItem != null)
                RenderProgressBarItem(this, e);
        }
        /// <summary>
        /// Draws the ProgressBarItem. If you need to provide custom rendering this is the method that you should override in your custom rendered. If you
        /// do not want default rendering to occur do not call the base implementation. You can call OnRenderProgressBarItem method so events can occur.
        /// </summary>
        /// <param name="e">Provides context information.</param>
        public virtual void DrawProgressBarItem(ProgressBarItemRenderEventArgs e)
        {
            OnRenderProgressBarItem(e);
        }
        #endregion

        #region Navigation Pane
        /// <summary>
        /// Raises RenderNavPaneButtonBackground event.
        /// </summary>
        /// <param name="e">Provides context information.</param>
        protected virtual void OnRenderNavPaneButtonBackground(NavPaneRenderEventArgs e)
        {
            if (RenderNavPaneButtonBackground != null)
                RenderNavPaneButtonBackground(this, e);
        }

        /// <summary>
        /// Draws the Navigation Pane button background. If you need to provide custom rendering this is the method that you should override in your custom rendered. If you
        /// do not want default rendering to occur do not call the base implementation. You can call OnRenderNavPaneButtonBackground method so events can occur.
        /// </summary>
        /// <param name="e">Provides context information.</param>
        public virtual void DrawNavPaneButtonBackground(NavPaneRenderEventArgs e)
        {
            OnRenderNavPaneButtonBackground(e);
        }
        #endregion

        #region SliderItem
        /// <summary>
        /// Raises RenderSliderItem event.
        /// </summary>
        /// <param name="e">Provides context information.</param>
        protected virtual void OnRenderSliderItem(SliderItemRendererEventArgs e)
        {
            if (RenderSliderItem!= null)
                RenderSliderItem(this, e);
        }

        /// <summary>
        /// Draws the Slider item. If you need to provide custom rendering this is the method that you should override in your custom rendered. If you
        /// do not want default rendering to occur do not call the base implementation. You can call OnRenderSliderItem method so events can occur.
        /// </summary>
        /// <param name="e">Provides context information.</param>
        public virtual void DrawSliderItem(SliderItemRendererEventArgs e)
        {
            OnRenderSliderItem(e);
        }
        #endregion

        #region SideBar Control
        /// <summary>
        /// Raises RenderSideBar event.
        /// </summary>
        /// <param name="e">Provides context information.</param>
        protected virtual void OnRenderSideBar(SideBarRendererEventArgs e)
        {
            if (RenderSideBar != null)
                RenderSideBar(this, e);
        }

        /// <summary>
        /// Draws the SideBar control. If you need to provide custom rendering this is the method that you should override in your custom rendered. If you
        /// do not want default rendering to occur do not call the base implementation. You can call OnRenderSideBar method so events can occur.
        /// </summary>
        /// <param name="e">Provides context information.</param>
        public virtual void DrawSideBar(SideBarRendererEventArgs e)
        {
            OnRenderSideBar(e);
        }

        /// <summary>
        /// Raises RenderSideBarPanelItem event.
        /// </summary>
        /// <param name="e">Provides context information.</param>
        protected virtual void OnRenderSideBarPanelItem(SideBarPanelItemRendererEventArgs e)
        {
            if (RenderSideBarPanelItem != null)
                RenderSideBarPanelItem(this, e);
        }

        /// <summary>
        /// Draws the SideBar control. If you need to provide custom rendering this is the method that you should override in your custom rendered. If you
        /// do not want default rendering to occur do not call the base implementation. You can call OnRenderSideBarPanelItem method so events can occur.
        /// </summary>
        /// <param name="e">Provides context information.</param>
        public virtual void DrawSideBarPanelItem(SideBarPanelItemRendererEventArgs e)
        {
            OnRenderSideBarPanelItem(e);
        }
        #endregion

        #region CrumbBar
        /// <summary>
        /// Raises RenderCrumbBarItemView event.
        /// </summary>
        /// <param name="e">Provides context information.</param>
        protected virtual void OnRenderCrumbBarItemView(ButtonItemRendererEventArgs e)
        {
            if (RenderCrumbBarItemView != null)
                RenderCrumbBarItemView(this, e);
        }
        /// <summary>
        /// Draws CrumbBarItemView. If you need to provide custom rendering this is the method that you should override in your custom rendered. If you
        /// do not want default rendering to occur do not call the base implementation. You can call OnRenderCrumbBarItemView method so events can occur.
        /// </summary>
        /// <param name="e">Provides context information.</param>
        public virtual void DrawCrumbBarItemView(ButtonItemRendererEventArgs e)
        {
            OnRenderCrumbBarItemView(e);
        }

        /// <summary>
        /// Raises RenderCrumbBarOverflowItem event.
        /// </summary>
        /// <param name="e">Provides context information.</param>
        protected virtual void OnRenderCrumbBarOverflowItem(ButtonItemRendererEventArgs e)
        {
            if (RenderCrumbBarOverflowItem != null)
                RenderCrumbBarOverflowItem(this, e);
        }
        /// <summary>
        /// Draws CrumbBarOverflowButton. If you need to provide custom rendering this is the method that you should override in your custom rendered. If you
        /// do not want default rendering to occur do not call the base implementation. You can call OnRenderCrumbBarOverflowItem method so events can occur.
        /// </summary>
        /// <param name="e">Provides context information.</param>
        public virtual void DrawCrumbBarOverflowItem(ButtonItemRendererEventArgs e)
        {
            OnRenderCrumbBarOverflowItem(e);
        }
        #endregion

        #region SwitchButton
        /// <summary>
        /// Raises RenderSwitchButton event.
        /// </summary>
        /// <param name="e">Provides context information.</param>
        protected virtual void OnRenderSwitchButton(SwitchButtonRenderEventArgs e)
        {
            if (RenderSwitchButton != null)
                RenderSwitchButton(this, e);
        }

        /// <summary>
        /// Draws the Switch Button. If you need to provide custom rendering this is the method that you should override in your custom rendered. If you
        /// do not want default rendering to occur do not call the base implementation. You can call OnRenderSwitchButton method so events can occur.
        /// </summary>
        /// <param name="e">Provides context information.</param>
        public virtual void DrawSwitchButton(SwitchButtonRenderEventArgs e)
        {
            OnRenderSwitchButton(e);
        }
        #endregion
    }
}