using System;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DevComponents.DotNetBar.Rendering
{
    /// <summary>
    /// Represents Office 2007 Control renderer.
    /// </summary>
    public class Office2007Renderer : BaseRenderer
    {
        #region Events
        /// <summary>
        ///  Occurs when color table is changed by setting the ColorTable property on the renderer.
        /// </summary>
        public event EventHandler ColorTableChanged;
        #endregion

        #region Private Variables
        //private Office12ColorTable m_ColorTable12 = null;
        private Office2007ColorTable m_ColorTable = null;
        #endregion

        #region Constructor
        public Office2007Renderer()
        {
            //m_ColorTable12 = new Office12ColorTable();
            m_ColorTable = new Office2007ColorTable();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets color table used by renderer.
        /// </summary>
        public Office2007ColorTable ColorTable
        {
            get { return m_ColorTable; }
            set
            {
                m_ColorTable = value;
                if (ColorTableChanged != null)
                    ColorTableChanged(this, new EventArgs());
            }
        }
        #endregion

        #region Key Tips Rendering
        /// <summary>
        /// Draws KeyTip for an object. If you need to provide custom rendering this is the method that you should override in your custom rendered. If you
        /// do not want default rendering to occur do not call the base implementation. You can call OnRenderKeyTips method so events can occur.
        /// </summary>
        /// <param name="e">Provides context information.</param>
        public override void DrawKeyTips(KeyTipsRendererEventArgs e)
        {
            KeyTipsPainter painter = PainterFactory.CreateKeyTipsPainter();
            if (painter is IOffice2007Painter)
                ((IOffice2007Painter)painter).ColorTable = m_ColorTable;

            painter.PaintKeyTips(e);

            base.DrawKeyTips(e);
        }
        #endregion

        #region Rendering Tab Group Rendering
        /// <summary>
        /// Draws ribbon tab group. If you need to provide custom rendering this is the method that you should override in your custom rendered. If you
        /// do not want default rendering to occur do not call the base implementation. You can call OnRenderRibbonTabGroup method so events can occur.
        /// </summary>
        /// <param name="e">Provides context information.</param>
        public override void DrawRibbonTabGroup(RibbonTabGroupRendererEventArgs e)
        {
            RibbonTabGroupPainter painter = PainterFactory.CreateRibbonTabGroupPainter(e.EffectiveStyle);
            if (painter is IOffice2007Painter)
                ((IOffice2007Painter)painter).ColorTable = m_ColorTable;
            painter.PaintTabGroup(e);

            base.DrawRibbonTabGroup(e);
        }
        #endregion

        #region ItemContainer Rendering
        /// <summary>
        /// Draws item container. If you need to provide custom rendering this is the method that you should override in your custom rendered. If you
        /// do not want default rendering to occur do not call the base implementation. You can call OnRenderItemContainer method so events can occur.
        /// </summary>
        /// <param name="e">Provides context information.</param>
        public override void DrawItemContainer(ItemContainerRendererEventArgs e)
        {
            ItemContainerPainter painter = PainterFactory.CreateItemContainerPainter(e.ItemContainer);
            if (painter != null)
            {
                if (painter is IOffice2007Painter)
                    ((IOffice2007Painter)painter).ColorTable = m_ColorTable;
                painter.PaintBackground(e);
            }

            base.DrawItemContainer(e);
        }

        /// <summary>
        /// Draws the separator for an item inside of item container. If you need to provide custom rendering this is the method that you should override in your custom rendered. If you
        /// do not want default rendering to occur do not call the base implementation. You can call OnRenderItemContainerSeparator method so events can occur.
        /// </summary>
        /// <param name="e">Provides context information.</param>
        public override void DrawItemContainerSeparator(ItemContainerSeparatorRendererEventArgs e)
        {
            base.DrawItemContainerSeparator(e);

            ItemContainerPainter painter = PainterFactory.CreateItemContainerPainter(e.ItemContainer);
            if (painter != null)
            {
                if (painter is IOffice2007Painter)
                    ((IOffice2007Painter)painter).ColorTable = m_ColorTable;
                painter.PaintItemSeparator(e);
            }
        }
        #endregion

        #region ButtonItem Rendering
        /// <summary>
        /// Draws ButtonItem. If you need to provide custom rendering this is the method that you should override in your custom rendered. If you
        /// do not want default rendering to occur do not call the base implementation. You can call OnRenderButtonItem method so events can occur.
        /// </summary>
        /// <param name="e">Provides context information.</param>
        public override void DrawButtonItem(ButtonItemRendererEventArgs e)
        {
            ButtonItemPainter painter = PainterFactory.CreateButtonPainter(e.ButtonItem);
            if (painter != null)
            {
                if (painter is IOffice2007Painter)
                    ((IOffice2007Painter)painter).ColorTable = m_ColorTable;
                painter.PaintButton(e.ButtonItem, e.ItemPaintArgs);
            }

            base.DrawButtonItem(e);
        }
        #endregion

        #region RibbonTabItem Rendering
        /// <summary>
        /// Draws RibbonTabItem. If you need to provide custom rendering this is the method that you should override in your custom rendered. If you
        /// do not want default rendering to occur do not call the base implementation. You can call OnRenderRibbonTabItem method so events can occur.
        /// </summary>
        /// <param name="e">Provides context information.</param>
        public override void DrawRibbonTabItem(RibbonTabItemRendererEventArgs e)
        {
            ButtonItemPainter painter = PainterFactory.CreateRibbonTabItemPainter(e.RibbonTabItem);
            if (painter != null)
            {
                if (painter is IOffice2007Painter)
                    ((IOffice2007Painter)painter).ColorTable = m_ColorTable;
                painter.PaintButton(e.RibbonTabItem, e.ItemPaintArgs);
            }

            base.DrawRibbonTabItem(e);
        }
        #endregion

        #region Popup Toolbar Rendering
        /// <summary>
        /// Draws popup toolbar background. If you need to provide custom rendering this is the method that you should override in your custom rendered. If you
        /// do not want default rendering to occur do not call the base implementation. You can call OnRenderPopupToolbarBackground method so events can occur.
        /// </summary>
        /// <param name="e">Provides context information.</param>
        public override void DrawPopupToolbarBackground(ToolbarRendererEventArgs e)
        {
            BarBackgroundPainter painter = PainterFactory.CreateBarBackgroundPainter(e.Bar);

            if (painter is IOffice2007Painter)
                ((IOffice2007Painter)painter).ColorTable = m_ColorTable;

            painter.PaintPopupBackground(e);

            base.DrawPopupToolbarBackground(e);
        }
        #endregion

        #region Docked or Floating Toolbar Rendering
        /// <summary>
        /// Draws docked or floating toolbar background. If you need to provide custom rendering this is the method that you should override in your custom rendered. If you
        /// do not want default rendering to occur do not call the base implementation. You can call OnRenderToolbarBackground method so events can occur.
        /// </summary>
        /// <param name="e">Provides context information.</param>
        public override void DrawToolbarBackground(ToolbarRendererEventArgs e)
        {
            BarBackgroundPainter painter = PainterFactory.CreateBarBackgroundPainter(e.Bar);

            if (painter is IOffice2007Painter)
                ((IOffice2007Painter)painter).ColorTable = m_ColorTable;

            if (e.Bar.BarState == eBarState.Docked)
                painter.PaintDockedBackground(e);
            else if (e.Bar.BarState == eBarState.Floating)
                painter.PaintFloatingBackground(e);

            base.DrawToolbarBackground(e);
        }

        /// <summary>
        /// Draws floating toolbar background.
        /// </summary>
        /// <param name="e">Provides context information.</param>
        protected virtual void DrawFloatingToolbarBackground(ToolbarRendererEventArgs e)
        {

        }
        #endregion

        #region Ribbon Rendering
        /// <summary>
        /// Draws ribbon bar dialog launcher button. If you need to provide custom rendering this is the method that you should override in your custom rendered. If you
        /// do not want default rendering to occur do not call the base implementation. You can call OnRenderPopupToolbarBackground method so events can occur.
        /// </summary>
        /// <param name="e">Provides context information.</param>
        public override void DrawRibbonDialogLauncher(RibbonBarRendererEventArgs e)
        {
            DialogLauncherPainter painter = PainterFactory.CreateRibbonBarPainter(e.RibbonBar);
            if (painter is IOffice2007Painter)
                ((IOffice2007Painter)painter).ColorTable = m_ColorTable;
            painter.PaintDialogLauncher(e);

            base.DrawRibbonDialogLauncher(e);
        }

        ///// <summary>
        ///// Draws ribbon bar background. If you need to provide custom rendering this is the method that you should override in your custom rendered. If you
        ///// do not want default rendering to occur do not call the base implementation. You can call OnRenderRibbonBarBackground method so events can occur.
        ///// </summary>
        ///// <param name="e">Provides context information.</param>
        //public override void DrawRibbonBarBackground(RibbonBarRendererEventArgs e)
        //{
        //    base.DrawRibbonBarBackground(e);

        //    RibbonBarPainter painter = PainterFactory.CreateRibbonBarPainter(e.RibbonBar);
        //    if (painter is IOffice2007Painter)
        //        ((IOffice2007Painter)painter).ColorTable = m_ColorTable;
        //    else if (painter is IOffice12Painter)
        //        ((IOffice12Painter)painter).ColorTable = m_ColorTable12;
        //    painter.PaintBackground(e);
        //}

        ///// <summary>
        ///// Draws ribbon bar title. If you need to provide custom rendering this is the method that you should override in your custom rendered. If you
        ///// do not want default rendering to occur do not call the base implementation. You can call OnRenderRibbonBarTitle method so events can occur.
        ///// </summary>
        ///// <param name="e">Provides context information.</param>
        //public override void DrawRibbonBarTitle(RibbonBarRendererEventArgs e)
        //{
        //    base.DrawRibbonBarTitle(e);

        //    RibbonBarPainter painter = PainterFactory.CreateRibbonBarPainter(e.RibbonBar);
        //    if (painter is IOffice2007Painter)
        //        ((IOffice2007Painter)painter).ColorTable = m_ColorTable;
        //    else if (painter is IOffice12Painter)
        //        ((IOffice12Painter)painter).ColorTable = m_ColorTable12;
        //    painter.PaintTitle(e);
        //}
        #endregion

        #region ColorItem rendering
        /// <summary>
        /// Draws ColorItem. If you need to provide custom rendering this is the method that you should override in your custom rendered. If you
        /// do not want default rendering to occur do not call the base implementation. You can call OnRenderColorItem method so events can occur.
        /// </summary>
        /// <param name="e">Provides context information.</param>
        public override void DrawColorItem(ColorItemRendererEventArgs e)
        {
            Rendering.ColorItemPainter painter = PainterFactory.CreateColorItemPainter(e.ColorItem);
            if (painter is IOffice2007Painter)
                ((IOffice2007Painter)painter).ColorTable = m_ColorTable;
            painter.PaintColorItem(e);

            base.DrawColorItem(e);
        }
        #endregion

        #region Ribbon Control Rendering
        /// <summary>
        /// Draws the background of the Ribbon Control. If you need to provide custom rendering this is the method that you should override in your custom rendered. If you
        /// do not want default rendering to occur do not call the base implementation. You can call OnRenderRibbonControlBackground method so events can occur.
        /// </summary>
        /// <param name="e">Provides context information.</param>
        public override void DrawRibbonControlBackground(RibbonControlRendererEventArgs e)
        {
            RibbonControlPainter painter = PainterFactory.CreateRibbonControlPainter(e.RibbonControl);
            if (painter == null)
                return;

            if (painter is IOffice2007Painter)
                ((IOffice2007Painter)painter).ColorTable = m_ColorTable;
            
            painter.PaintBackground(e);

            base.DrawRibbonControlBackground(e);
        }

        /// <summary>
        /// Draws the form caption text for the Ribbon Control. If you need to provide custom rendering this is the method that you should override in your custom rendered. If you
        /// do not want default rendering to occur do not call the base implementation. You can call OnRenderRibbonFormCaptionText method so events can occur.
        /// </summary>
        /// <param name="e">Provides context information.</param>
        public override void DrawRibbonFormCaptionText(RibbonControlRendererEventArgs e)
        {
            base.DrawRibbonFormCaptionText(e);

            RibbonControlPainter painter = PainterFactory.CreateRibbonControlPainter(e.RibbonControl);
            if (painter == null)
                return;

            if (painter is IOffice2007Painter)
                ((IOffice2007Painter)painter).ColorTable = m_ColorTable;

            painter.PaintCaptionText(e);
        }

        /// <summary>
        /// Draws the background of Quick Access Toolbar on Ribbon Control. If you need to provide custom rendering this is the method that you should override in your custom rendered. If you
        /// do not want default rendering to occur do not call the base implementation. You can call OnRenderQuickAccessToolbarBackground method so events can occur.
        /// </summary>
        /// <param name="e">Provides context information.</param>
        public override void DrawQuickAccessToolbarBackground(RibbonControlRendererEventArgs e)
        {
            base.DrawQuickAccessToolbarBackground(e);

            RibbonControlPainter painter = PainterFactory.CreateRibbonControlPainter(e.RibbonControl);
            if (painter == null)
                return;

            if (painter is IOffice2007Painter)
                ((IOffice2007Painter)painter).ColorTable = m_ColorTable;

            painter.PaintQuickAccessToolbarBackground(e);
        }
        #endregion

        #region SystemCaptionItem Rendering
        /// <summary>
        /// Draws the SystemCaptionItem. If you need to provide custom rendering this is the method that you should override in your custom rendered. If you
        /// do not want default rendering to occur do not call the base implementation. You can call OnRenderSystemCaptionItem method so events can occur.
        /// </summary>
        /// <param name="e">Provides context information.</param>
        public override void DrawSystemCaptionItem(SystemCaptionItemRendererEventArgs e)
        {
            SystemCaptionItemPainter painter = PainterFactory.CreateSystemCaptionItemPainter(e.SystemCaptionItem);

            if (painter == null)
                return;

            if (painter is IOffice2007Painter)
                ((IOffice2007Painter)painter).ColorTable = m_ColorTable;

            painter.Paint(e);

            base.DrawSystemCaptionItem(e);
        }
        #endregion

        #region MdiSystemItem Rendering
        /// <summary>
        /// Draws the MdiSystemItem. If you need to provide custom rendering this is the method that you should override in your custom rendered. If you
        /// do not want default rendering to occur do not call the base implementation. You can call OnRenderMdiSystemItem method so events can occur.
        /// </summary>
        /// <param name="e">Provides context information.</param>
        public override void DrawMdiSystemItem(MdiSystemItemRendererEventArgs e)
        {
            MdiSystemItemPainter painter = PainterFactory.CreateMdiSystemItemPainter(e.MdiSystemItem);

            if (painter == null)
                return;

            if (painter is IOffice2007Painter)
                ((IOffice2007Painter)painter).ColorTable = m_ColorTable;

            painter.Paint(e);

            base.DrawMdiSystemItem(e);
        }
        #endregion

        #region Form Caption
        /// <summary>
        /// Draws the form caption background. If you need to provide custom rendering this is the method that you should override in your custom rendered. If you
        /// do not want default rendering to occur do not call the base implementation. You can call OnRenderFormCaptionBackground method so events can occur.
        /// </summary>
        /// <param name="e">Provides context information.</param>
        public override void DrawFormCaptionBackground(FormCaptionRendererEventArgs e)
        {
            FormCaptionPainter painter = PainterFactory.CreateFormCaptionPainter(e.Form);

            if (painter == null)
                return;

            if (painter is IOffice2007Painter)
                ((IOffice2007Painter)painter).ColorTable = m_ColorTable;

            painter.PaintCaptionBackground(e);

            base.DrawFormCaptionBackground(e);
        }
        #endregion

        #region QAT Overflow/Customize Item rendering
        /// <summary>
        /// Draws the Quick Access Toolbar Overflow item. If you need to provide custom rendering this is the method that you should override in your custom rendered. If you
        /// do not want default rendering to occur do not call the base implementation. You can call OnRenderQatOverflowItem method so events can occur.
        /// </summary>
        /// <param name="e">Provides context information.</param>
        public override void DrawQatOverflowItem(QatOverflowItemRendererEventArgs e)
        {
            QatOverflowPainter painter = PainterFactory.CreateQatOverflowItemPainter(e.OverflowItem);

            if (painter == null)
                return;

            if (painter is IOffice2007Painter)
                ((IOffice2007Painter)painter).ColorTable = m_ColorTable;

            painter.Paint(e);

            base.DrawQatOverflowItem(e);
        }

        /// <summary>
        /// Draws the Quick Access Toolbar Overflow item. If you need to provide custom rendering this is the method that you should override in your custom rendered. If you
        /// do not want default rendering to occur do not call the base implementation. You can call OnRenderQatOverflowItem method so events can occur.
        /// </summary>
        /// <param name="e">Provides context information.</param>
        public override void DrawQatCustomizeItem(QatCustomizeItemRendererEventArgs e)
        {
            QatCustomizeItemPainter painter = PainterFactory.CreateQatCustomizeItemPainter(e.CustomizeItem);

            if (painter == null)
                return;

            if (painter is IOffice2007Painter)
                ((IOffice2007Painter)painter).ColorTable = m_ColorTable;

            painter.Paint(e);

            base.DrawQatCustomizeItem(e);
        }
        #endregion

        #region CheckBoxItem
        public override void DrawCheckBoxItem(CheckBoxItemRenderEventArgs e)
        {
            CheckBoxItemPainter painter = PainterFactory.CreateCheckBoxItemPainter(e.CheckBoxItem);

            if (painter == null)
                return;

            if (painter is IOffice2007Painter)
                ((IOffice2007Painter)painter).ColorTable = m_ColorTable;

            painter.Paint(e);

            base.DrawCheckBoxItem(e);
        }
        #endregion

        #region ProgressBarItem
        /// <summary>
        /// Draws the ProgressBarItem. If you need to provide custom rendering this is the method that you should override in your custom rendered. If you
        /// do not want default rendering to occur do not call the base implementation. You can call OnRenderProgressBarItem method so events can occur.
        /// </summary>
        /// <param name="e">Provides context information.</param>
        public override void DrawProgressBarItem(ProgressBarItemRenderEventArgs e)
        {
            ProgressBarItemPainter painter = PainterFactory.CreateProgressBarItemPainter(e.ProgressBarItem);

            if (painter == null)
                return;

            if (painter is IOffice2007Painter)
                ((IOffice2007Painter)painter).ColorTable = m_ColorTable;

            painter.Paint(e);

            base.DrawProgressBarItem(e);
        }
        #endregion

        #region Navigation Pane
        /// <summary>
        /// Draws the Navigation Pane button background. If you need to provide custom rendering this is the method that you should override in your custom rendered. If you
        /// do not want default rendering to occur do not call the base implementation. You can call OnRenderNavPaneButtonBackground method so events can occur.
        /// </summary>
        /// <param name="e">Provides context information.</param>
        public override void DrawNavPaneButtonBackground(NavPaneRenderEventArgs e)
        {
            NavigationPanePainter painter = PainterFactory.CreateNavigationPanePainter();
            
            if (painter == null)
                return;

            if (painter is IOffice2007Painter)
                ((IOffice2007Painter)painter).ColorTable = m_ColorTable;

            painter.PaintButtonBackground(e);


            base.DrawNavPaneButtonBackground(e);
        }
        #endregion

        #region SliderItem
        /// <summary>
        /// Draws the Slider item. If you need to provide custom rendering this is the method that you should override in your custom rendered. If you
        /// do not want default rendering to occur do not call the base implementation. You can call OnRenderSliderItem method so events can occur.
        /// </summary>
        /// <param name="e">Provides context information.</param>
        public override void DrawSliderItem(SliderItemRendererEventArgs e)
        {
            SliderPainter painter = PainterFactory.CreateSliderPainter();

            if (painter == null)
                return;

            if (painter is IOffice2007Painter)
                ((IOffice2007Painter)painter).ColorTable = m_ColorTable;

            painter.Paint(e);

            base.DrawSliderItem(e);
        }
        #endregion

        #region SideBar
        /// <summary>
        /// Draws the SideBar control. If you need to provide custom rendering this is the method that you should override in your custom rendered. If you
        /// do not want default rendering to occur do not call the base implementation. You can call OnRenderSideBar method so events can occur.
        /// </summary>
        /// <param name="e">Provides context information.</param>
        public override void DrawSideBar(SideBarRendererEventArgs e)
        {
            SideBarPainter painter = PainterFactory.CreateSideBarPainter();

            if (painter == null)
                return;

            if (painter is IOffice2007Painter)
                ((IOffice2007Painter)painter).ColorTable = m_ColorTable;

            painter.PaintSideBar(e);

            base.DrawSideBar(e);
        }

        /// <summary>
        /// Draws the SideBar control. If you need to provide custom rendering this is the method that you should override in your custom rendered. If you
        /// do not want default rendering to occur do not call the base implementation. You can call OnRenderSideBarPanelItem method so events can occur.
        /// </summary>
        /// <param name="e">Provides context information.</param>
        public override void DrawSideBarPanelItem(SideBarPanelItemRendererEventArgs e)
        {
            SideBarPainter painter = PainterFactory.CreateSideBarPainter();

            if (painter == null)
                return;

            if (painter is IOffice2007Painter)
                ((IOffice2007Painter)painter).ColorTable = m_ColorTable;

            painter.PaintSideBarPanelItem(e);

            base.DrawSideBarPanelItem(e);
        }
        #endregion

        #region CrumbBar
        /// <summary>
        /// Draws CrumbBarItemView. If you need to provide custom rendering this is the method that you should override in your custom rendered. If you
        /// do not want default rendering to occur do not call the base implementation. You can call OnRenderButtonItem method so events can occur.
        /// </summary>
        /// <param name="e">Provides context information.</param>
        public override void DrawCrumbBarItemView(ButtonItemRendererEventArgs e)
        {
            CrumbBarItemViewPainter painter = PainterFactory.GetCrumbBarItemViewPainter(e.ButtonItem);
            if (painter != null)
            {
                if (painter is IOffice2007Painter)
                    ((IOffice2007Painter)painter).ColorTable = m_ColorTable;
                painter.Paint(e.ButtonItem, e.ItemPaintArgs);
            }

            base.DrawCrumbBarItemView(e);
        }

        /// <summary>
        /// Draws CrumbBarOverflowButton. If you need to provide custom rendering this is the method that you should override in your custom rendered. If you
        /// do not want default rendering to occur do not call the base implementation. You can call OnRenderCrumbBarOverflowItem method so events can occur.
        /// </summary>
        /// <param name="e">Provides context information.</param>
        public override void DrawCrumbBarOverflowItem(ButtonItemRendererEventArgs e)
        {
            CrumbBarItemViewPainter painter = PainterFactory.GetCrumbBarItemViewPainter(e.ButtonItem);
            if (painter != null)
            {
                if (painter is IOffice2007Painter)
                    ((IOffice2007Painter)painter).ColorTable = m_ColorTable;
                painter.PaintOverflowButton(e.ButtonItem, e.ItemPaintArgs);
            }
            base.DrawCrumbBarOverflowItem(e);
        }
        #endregion

        #region SwitchButton
        /// <summary>
        /// Draws SwitchButton. If you need to provide custom rendering this is the method that you should override in your custom rendered. If you
        /// do not want default rendering to occur do not call the base implementation. You can call OnRenderButtonItem method so events can occur.
        /// </summary>
        /// <param name="e">Provides context information.</param>
        public override void DrawSwitchButton(SwitchButtonRenderEventArgs e)
        {
            SwitchButtonPainter painter = PainterFactory.CreateSwitchButtonPainter(e.SwitchButtonItem);
            if (painter != null)
            {
                if (painter is IOffice2007Painter)
                    ((IOffice2007Painter)painter).ColorTable = m_ColorTable;
                painter.Paint(e);
            }

            base.DrawSwitchButton(e);
        }
        #endregion

    }
}
