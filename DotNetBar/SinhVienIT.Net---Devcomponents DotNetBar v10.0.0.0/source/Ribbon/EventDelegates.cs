using System;
using System.Text;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Defines delegate for RenderKeyTips event.
    /// </summary>
    public delegate void KeyTipsRendererEventHandler(object sender, KeyTipsRendererEventArgs e);

    /// <summary>
    /// Defines delegate for RenderRibbonTabGroup event.
    /// </summary>
    public delegate void RibbonTabGroupRendererEventHandler(object sender, RibbonTabGroupRendererEventArgs e);

    /// <summary>
    /// Defines delegate for RenderItemContainer event.
    /// </summary>
    public delegate void ItemContainerRendererEventHandler(object sender, ItemContainerRendererEventArgs e);

    /// <summary>
    /// Defines delegate for RenderItemContainerSeparator event.
    /// </summary>
    public delegate void ItemContainerSeparatorRendererEventHandler(object sender, ItemContainerSeparatorRendererEventArgs e);

    /// <summary>
    /// Defines delegate for ButtonItem rendering events.
    /// </summary>
    public delegate void ButtonItemRendererEventHandler(object sender, ButtonItemRendererEventArgs e);

    /// <summary>
    /// Defines delegate for RibbonTabItem rendering events.
    /// </summary>
    public delegate void RibbonTabItemRendererEventHandler(object sender, RibbonTabItemRendererEventArgs e);

    /// <summary>
    /// Defines delegate for toolbar rendering events.
    /// </summary>
    public delegate void ToolbarRendererEventHandler(object sender, ToolbarRendererEventArgs e);

    /// <summary>
    /// Defines delegate for Rendering dialog launcher button rendering events.
    /// </summary>
    public delegate void RibbonBarRendererEventHandler(object sender, RibbonBarRendererEventArgs e);

    /// <summary>
    /// Defines delegate for ColorItem rendering events.
    /// </summary>
    public delegate void ColorItemRendererEventHandler(object sender, ColorItemRendererEventArgs e);

    /// <summary>
    /// Defines delegate for RibbonControl rendering events.
    /// </summary>
    public delegate void RibbonControlRendererEventHandler(object sender, RibbonControlRendererEventArgs e);

    /// <summary>
    /// Defines delegate for SystemCaptionItem rendering events.
    /// </summary>
    public delegate void SystemCaptionItemRendererEventHandler(object sender, SystemCaptionItemRendererEventArgs e);

    /// <summary>
    /// Defines delegate for MdiSystemItem rendering events.
    /// </summary>
    public delegate void MdiSystemItemRendererEventHandler(object sender, MdiSystemItemRendererEventArgs e);

    /// <summary>
    /// Defines delegate for RenderFormCaptionBackground rendering events.
    /// </summary>
    public delegate void FormCaptionRendererEventHandler(object sender, FormCaptionRendererEventArgs e);

    /// <summary>
    /// Defines delegate for CustomizeMenuPopup events.
    /// </summary>
    public delegate void CustomizeMenuPopupEventHandler(object sender, RibbonCustomizeEventArgs e);

    /// <summary>
    /// Defines delegate for the Quick Access Overflow item rendering events.
    /// </summary>
    public delegate void QatOverflowItemRendererEventHandler(object sender, QatOverflowItemRendererEventArgs e);

    /// <summary>
    /// Defines delegate for the Quick Access Customize item rendering events.
    /// </summary>
    public delegate void QatCustomizeItemRendererEventHandler(object sender, QatCustomizeItemRendererEventArgs e);

    /// <summary>
    /// Defines delegate for the Quick Access Customization dialog events.
    /// </summary>
    public delegate void QatCustomizeDialogEventHandler(object sender, QatCustomizeDialogEventArgs e);

    /// <summary>
    /// Defines delegate for the CheckBoxItem rendering events.
    /// </summary>
    public delegate void CheckBoxItemRendererEventHandler(object sender, CheckBoxItemRenderEventArgs e);

    /// <summary>
    /// Defines delegate for the ProgressBarItem rendering events.
    /// </summary>
    public delegate void ProgressBarItemRendererEventHandler(object sender, ProgressBarItemRenderEventArgs e);

    /// <summary>
    /// Defines delegate for the Navigation Pane rendering events.
    /// </summary>
    public delegate void NavPaneRendererEventHandler(object sender, NavPaneRenderEventArgs e);

    /// <summary>
    /// Defines delegate for the BeforeRibbonPanelPopupClose event.
    /// </summary>
    public delegate void RibbonPopupCloseEventHandler(object sender, RibbonPopupCloseEventArgs e);

    /// <summary>
    /// Defines delegate for the Slider item rendering events.
    /// </summary>
    public delegate void SliderItemRendererEventHandler(object sender, SliderItemRendererEventArgs e);

    /// <summary>
    /// Defines delegate for the SideBar control rendering event.
    /// </summary>
    public delegate void SideBarRendererEventHandler(object sender, SideBarRendererEventArgs e);

    /// <summary>
    /// Defines delegate for the SideBarPanelItem control rendering event.
    /// </summary>
    public delegate void SideBarPanelItemRendererEventHandler(object sender, SideBarPanelItemRendererEventArgs e);

    /// <summary>
    /// Defines delegate for the SwitchButtonItem control rendering event.
    /// </summary>
    public delegate void SwitchButtonRendererEventHandler(object sender, DevComponents.DotNetBar.Rendering.SwitchButtonRenderEventArgs e);
    
}
