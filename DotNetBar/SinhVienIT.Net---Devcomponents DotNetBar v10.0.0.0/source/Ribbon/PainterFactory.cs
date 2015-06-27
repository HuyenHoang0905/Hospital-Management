using System.Windows.Forms;
using System;
using DevComponents.DotNetBar.Rendering;
namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Summary description for PainterFactory.
	/// </summary>
	internal class PainterFactory
	{
		private static Office2003ButtonItemPainter m_Office2003Painter=new Office2003ButtonItemPainter();
        private static Office2007ButtonItemPainter m_Office2007Painter = new Office2007ButtonItemPainter();
		private static Office2003RibbonTabItemPainter m_RibbonTabItemOffice2003Painter=new Office2003RibbonTabItemPainter();
        private static Office2007ItemContainerPainter m_Office2007ItemContainerPainter = new Office2007ItemContainerPainter();
        private static Office2007BarBackgroundPainter m_Office2007BarBackgroundPainter = new Office2007BarBackgroundPainter();
        private static Office2007KeyTipsPainter m_Office2007KeyTipsPainter = new Office2007KeyTipsPainter();
        private static Office2007DialogLauncherPainter m_Office2007RibbonBarPainter = new Office2007DialogLauncherPainter();
        private static Office2007RibbonControlPainter m_Office2007RibbonControlPainter = new Office2007RibbonControlPainter();
        private static Office2007RibbonTabItemPainter m_Office2007RibbonTabItemPainter = new Office2007RibbonTabItemPainter();
        //private static Office2007RibbonTabGroupPainter m_Office2007RibbonTabGroupPainter = new Office2007RibbonTabGroupPainter();
        private static Rendering.Office2007ColorItemPainter m_Office2007ColorItemPainter = new Rendering.Office2007ColorItemPainter();
        private static Office2007SystemCaptionItemPainter m_Office2007SystemCaptionItemPainter = null;
        private static Office2007MdiSystemItemPainter m_Office2007MdiSystemItemPainter = new Office2007MdiSystemItemPainter();
        private static Office2007FormCaptionPainter m_Office2007FormCaptionPainter = new Office2007FormCaptionPainter();
        private static Rendering.Office2007RibbonOverflowPainter m_Office2007RibbonOverflowPainter = new DevComponents.DotNetBar.Rendering.Office2007RibbonOverflowPainter();
        private static Rendering.Office2007QatOverflowPainter m_Office2007QatOverflowPainter = new DevComponents.DotNetBar.Rendering.Office2007QatOverflowPainter();
        private static Rendering.Office2007QatCustomizeItemPainter m_Office2007QatCustomizePainter = new DevComponents.DotNetBar.Rendering.Office2007QatCustomizeItemPainter();
        private static Rendering.Office2007CheckBoxItemPainter m_Office2007CheckBoxItemPainter = new Rendering.Office2007CheckBoxItemPainter();
        private static Rendering.Office2007ProgressBarItemPainter m_Office2007ProgressBarPainter = new DevComponents.DotNetBar.Rendering.Office2007ProgressBarItemPainter();
        private static Rendering.Office2007NavigationPanePainter m_Office2007NavPanePainter = new DevComponents.DotNetBar.Rendering.Office2007NavigationPanePainter();
        private static Rendering.SliderPainter m_SliderPainter = new DevComponents.DotNetBar.Rendering.Office2007SliderPainter();
        private static Rendering.SideBarPainter m_SideBarPainter = new DevComponents.DotNetBar.Rendering.Office2007SideBarPainter();
        private static CrumbBarItemViewPainter m_CrumbBarItemViewPainter = null;
        private static SwitchButtonPainter m_SwitchButtonPainter = null;

        public static ButtonItemPainter CreateButtonPainter(ButtonItem button)
        {
            if (button is RibbonTabItem)
            {
                return CreateRibbonTabItemPainter((RibbonTabItem)button);
            }
            if (button is RibbonOverflowButtonItem)
            {
                ButtonItemPainter p = CreateRibbonOverflowButtonPainter((RibbonOverflowButtonItem)button);
                if (p != null)
                    return p;
            }
            eDotNetBarStyle buttonEffectiveStyle = button.EffectiveStyle;
            if (buttonEffectiveStyle == eDotNetBarStyle.Office2010 && button is Office2007StartButton)
                return Office2010ApplicationButtonPainter;

            if (buttonEffectiveStyle == eDotNetBarStyle.Office2003 || buttonEffectiveStyle == eDotNetBarStyle.VS2005 || buttonEffectiveStyle == eDotNetBarStyle.OfficeXP || buttonEffectiveStyle == eDotNetBarStyle.Office2000)
            {
                return m_Office2003Painter;
            }
            else if (BarFunctions.IsOffice2007Style(buttonEffectiveStyle))
            {
                if (button.ContainerControl is RibbonBar)
                    return m_Office2007Painter;
                return m_Office2007Painter;
            }
            return null;
        }

        private static ButtonItemPainter _Office2010ApplicationButtonPainter;
        public static ButtonItemPainter Office2010ApplicationButtonPainter
        {
            get
            {
                if (_Office2010ApplicationButtonPainter == null) _Office2010ApplicationButtonPainter = new Office2010AppButtonPainter();
                return _Office2010ApplicationButtonPainter;
            }
        }

        public static CrumbBarItemViewPainter GetCrumbBarItemViewPainter(ButtonItem button)
        {
            if (m_CrumbBarItemViewPainter == null)
                m_CrumbBarItemViewPainter = new CrumbBarItemViewPainter();
            return m_CrumbBarItemViewPainter;
        }

        public static ButtonItemPainter CreateRibbonOverflowButtonPainter(RibbonOverflowButtonItem button)
        {
            if (BarFunctions.IsOffice2007Style(button.EffectiveStyle))
                return m_Office2007RibbonOverflowPainter;
            return null;
        }

		public static ButtonItemPainter CreateRibbonTabItemPainter(RibbonTabItem tab)
		{
            if (BarFunctions.IsOffice2007Style(tab.EffectiveStyle) && !tab.IsOnMenu)
            {
                return m_Office2007RibbonTabItemPainter;
            }
            if ((tab.EffectiveStyle == eDotNetBarStyle.Office2003 || tab.EffectiveStyle == eDotNetBarStyle.VS2005) && !tab.IsOnMenu)
			{
				return m_RibbonTabItemOffice2003Painter;
			}
			
			return m_Office2003Painter;
		}

        public static ItemContainerPainter CreateItemContainerPainter(ItemContainer container)
        {
            if (BarFunctions.IsOffice2007Style(container.EffectiveStyle))
                return m_Office2007ItemContainerPainter;
            return null;
        }

        public static BarBackgroundPainter CreateBarBackgroundPainter(Bar bar)
        {
            return m_Office2007BarBackgroundPainter;
        }

        public static KeyTipsPainter CreateKeyTipsPainter()
        {
            return m_Office2007KeyTipsPainter;
        }

        public static DialogLauncherPainter CreateRibbonBarPainter(RibbonBar ribbon)
        {
            if (BarFunctions.IsOffice2007Style(ribbon.EffectiveStyle))
                return m_Office2007RibbonBarPainter;
            return null;
        }

        private static RibbonTabGroupPainter _Office2010RibbonTabGroupPainter = null;
        private static RibbonTabGroupPainter Office2010RibbonTabGroupPainter
        {
            get
            {
                if (_Office2010RibbonTabGroupPainter == null) _Office2010RibbonTabGroupPainter = new Office2010RibbonTabGroupPainter();
                return _Office2010RibbonTabGroupPainter;
            }
        }
        private static RibbonTabGroupPainter _Office2007RibbonTabGroupPainter = null;
        private static RibbonTabGroupPainter Office2007RibbonTabGroupPainter
        {
            get
            {
                if (_Office2007RibbonTabGroupPainter == null) _Office2007RibbonTabGroupPainter = new Office2007RibbonTabGroupPainter();
                return _Office2007RibbonTabGroupPainter;
            }
        }
        public static RibbonTabGroupPainter CreateRibbonTabGroupPainter(eDotNetBarStyle style)
        {
            if (style == eDotNetBarStyle.Office2010)
                return Office2010RibbonTabGroupPainter;
            else
                return Office2007RibbonTabGroupPainter;
        }

        public static Rendering.ColorItemPainter CreateColorItemPainter(ColorItem item)
        {
            return m_Office2007ColorItemPainter;
        }

        public static RibbonControlPainter CreateRibbonControlPainter(RibbonControl r)
        {
            if (BarFunctions.IsOffice2007Style(r.EffectiveStyle))
                return m_Office2007RibbonControlPainter;
            return null;
        }

        private static Office2010SystemCaptionItemPainter _Office2010SystemCaptionItemPainter=null;
        private static SystemCaptionItemPainter Office2010SystemCaptionItemPainter
        {
            get
            {
                if (_Office2010SystemCaptionItemPainter == null) _Office2010SystemCaptionItemPainter = new Office2010SystemCaptionItemPainter();
                return _Office2010SystemCaptionItemPainter;
            }
        }
        private static Office2007SystemCaptionItemPainter _Office2007SystemCaptionItemPainter = null;
        private static SystemCaptionItemPainter Office2007SystemCaptionItemPainter
        {
            get
            {
                if (_Office2007SystemCaptionItemPainter == null) _Office2007SystemCaptionItemPainter = new Office2007SystemCaptionItemPainter();
                return _Office2007SystemCaptionItemPainter;
            }
        }
        public static SystemCaptionItemPainter CreateSystemCaptionItemPainter(SystemCaptionItem item)
        {
            eDotNetBarStyle effectiveStyle = item.EffectiveStyle;
            if (effectiveStyle == eDotNetBarStyle.Office2010)
                return Office2010SystemCaptionItemPainter;
            if (BarFunctions.IsOffice2007Style(effectiveStyle))
                return Office2007SystemCaptionItemPainter;
            return null;
        }

        public static MdiSystemItemPainter CreateMdiSystemItemPainter(MDISystemItem mdiSystemItem)
        {
            if (BarFunctions.IsOffice2007Style(mdiSystemItem.EffectiveStyle))
                return m_Office2007MdiSystemItemPainter;
            return null;
        }

        public static FormCaptionPainter CreateFormCaptionPainter(Form form)
        {
            return m_Office2007FormCaptionPainter;
        }

        public static DevComponents.DotNetBar.Rendering.QatOverflowPainter CreateQatOverflowItemPainter(QatOverflowItem ribbonQatOverflowItem)
        {
            return m_Office2007QatOverflowPainter;
        }

        public static DevComponents.DotNetBar.Rendering.QatCustomizeItemPainter CreateQatCustomizeItemPainter(QatCustomizeItem qatCustomizeItem)
        {
            return m_Office2007QatCustomizePainter;
        }

        public static Rendering.Office2007CheckBoxItemPainter CreateCheckBoxItemPainter(CheckBoxItem item)
        {
            return m_Office2007CheckBoxItemPainter;
        }

        /// <summary>
        /// Forces the creation of the objects inside of factory.
        /// </summary>
        public static void InitFactory() { }

        public static DevComponents.DotNetBar.Rendering.ProgressBarItemPainter CreateProgressBarItemPainter(ProgressBarItem progressBarItem)
        {
            return m_Office2007ProgressBarPainter;
        }

        internal static DevComponents.DotNetBar.Rendering.NavigationPanePainter CreateNavigationPanePainter()
        {
            return m_Office2007NavPanePainter;
        }

        internal static DevComponents.DotNetBar.Rendering.SliderPainter CreateSliderPainter()
        {
            return m_SliderPainter;
        }

        internal static DevComponents.DotNetBar.Rendering.SideBarPainter CreateSideBarPainter()
        {
            return m_SideBarPainter;
        }

        internal static SwitchButtonPainter CreateSwitchButtonPainter(SwitchButtonItem item)
        { 
            if (m_SwitchButtonPainter == null)
                m_SwitchButtonPainter = new Office2010SwitchButtonPainter();
            return m_SwitchButtonPainter;
        }
      
    }
}
