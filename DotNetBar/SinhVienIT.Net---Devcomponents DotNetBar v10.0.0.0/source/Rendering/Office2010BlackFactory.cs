using System;
using System.Text;
using System.Drawing;
using DevComponents.DotNetBar.Schedule;

namespace DevComponents.DotNetBar.Rendering
{
    internal static class Office2010BlackFactory
    {
        public static void InitializeColorTable(Office2007ColorTable table, ColorFactory factory)
        {
            #region RibbonControl Start Images
            table.RibbonControl.StartButtonDefault = BarFunctions.LoadBitmap("SystemImages.BlankOffice2010NormalSilver.png");
            table.RibbonControl.StartButtonMouseOver = BarFunctions.LoadBitmap("SystemImages.BlankOffice2010HotSilver.png");
            table.RibbonControl.StartButtonPressed = BarFunctions.LoadBitmap("SystemImages.BlankOffice2010PressedSilver.png");
            #endregion

            #region RibbonControl
            table.RibbonControl.OuterBorder = new LinearGradientColorTable(factory.GetColor(0x3E3F3F), factory.GetColor(0x4E4E4E));
            table.RibbonControl.InnerBorder = LinearGradientColorTable.Empty;
            table.RibbonControl.TabsBackground = new LinearGradientColorTable(factory.GetColor(0x717171));
            table.RibbonControl.TabsGlassBackground = new LinearGradientColorTable(Color.Transparent, factory.GetColor(0x494949));
            table.RibbonControl.TabDividerBorder = Color.Empty; // factory.GetColor(0xA7BAD1);
            table.RibbonControl.TabDividerBorderLight = Color.Empty; // factory.GetColor(0xF4F8FD);
            table.RibbonControl.CornerSize = 1;
            table.RibbonControl.PanelTopBackgroundHeight = 0;
            table.RibbonControl.PanelTopBackground = new LinearGradientColorTable(factory.GetColor(0xC9C9C9), factory.GetColor(0xA3A3A3));
            table.RibbonControl.PanelBottomBackground = null;
            #endregion

            #region Item Group
            table.ItemGroup.OuterBorder = LinearGradientColorTable.Empty; //new LinearGradientColorTable(factory.GetColor(0xC0C3C8));
            table.ItemGroup.InnerBorder = LinearGradientColorTable.Empty;
            table.ItemGroup.TopBackground = LinearGradientColorTable.Empty;
            table.ItemGroup.BottomBackground = LinearGradientColorTable.Empty;
            table.ItemGroup.ItemGroupDividerDark = Color.Empty;
            table.ItemGroup.ItemGroupDividerLight = Color.Empty;
            #endregion

            #region RibbonBar
            table.RibbonBar.Default = GetRibbonBar(factory);
            table.RibbonBar.MouseOver = GetRibbonBarMouseOver(factory);
            table.RibbonBar.Expanded = GetRibbonBarExpanded(factory);
            #endregion

            #region ButtonItem Colors Initialization
            table.RibbonButtonItemColors.Clear();
            table.ButtonItemColors.Clear();
            table.MenuButtonItemColors.Clear();
            // Orange
            Office2007ButtonItemColorTable cb = GetButtonItemBlueOrange(factory);
            cb.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Orange);
            table.ButtonItemColors.Add(cb);
            // Orange with background
            cb = GetButtonItemBlueOrangeWithBackground(factory);
            cb.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.OrangeWithBackground);
            table.ButtonItemColors.Add(cb);
            // Blue
            cb = Office2007ColorTableFactory.GetButtonItemBlueBlue(factory);
            cb.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
            cb.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(factory.GetColor(0xB1BAC4));
            cb.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Blue);
            table.ButtonItemColors.Add(cb);
            // Blue with background
            cb = Office2007ColorTableFactory.GetButtonItemBlueBlueWithBackground(factory);
            cb.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
            cb.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(factory.GetColor(0xB1BAC4));
            cb.MouseOverSplitInactive = cb.Default;
            cb.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.BlueWithBackground);
            table.ButtonItemColors.Add(cb);
            // Magenta
            cb = Office2007ColorTableFactory.GetButtonItemBlueMagenta(factory);
            cb.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
            cb.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(factory.GetColor(0xB1BAC4));
            cb.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Magenta);
            table.ButtonItemColors.Add(cb);
            // Blue with background
            cb = Office2007ColorTableFactory.GetButtonItemBlueMagentaWithBackground(factory);
            cb.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
            cb.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(factory.GetColor(0xB1BAC4));
            cb.MouseOverSplitInactive = cb.Default;
            cb.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.MagentaWithBackground);
            table.ButtonItemColors.Add(cb);

            cb = Office2010BlueFactory.GetButtonItemOffice2007WithBackground(factory);
            cb.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
            cb.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(factory.GetColor(0xB1BAC4));
            cb.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Office2007WithBackground);
            table.ButtonItemColors.Add(cb);

            table.ButtonItemColors.Add(ButtonItemStaticColorTables.CreateBlueOrbColorTable(factory));

            table.BackstageButtonItemColors.Clear();
            table.BackstageButtonItemColors.Add(GetButtonItemBackstageDefault(factory));
            #endregion

            #region RibbonTabItem Colors Initialization
            table.RibbonTabItemColors.Clear();
            Office2007RibbonTabItemColorTable rt = GetRibbonTabItemDefault(factory);
            rt.Name = Enum.GetName(typeof(eRibbonTabColor), eRibbonTabColor.Default);
            table.RibbonTabItemColors.Add(rt);

            // Magenta
            rt = GetRibbonTabItemMagenta(factory);
            rt.CornerSize = 2;
            rt.Name = Enum.GetName(typeof(eRibbonTabColor), eRibbonTabColor.Magenta);
            table.RibbonTabItemColors.Add(rt);

            // Green
            rt = GetRibbonTabItemBlueGreen(factory);
            rt.CornerSize = 2;
            rt.Name = Enum.GetName(typeof(eRibbonTabColor), eRibbonTabColor.Green);
            table.RibbonTabItemColors.Add(rt);

            // Orange
            rt = GetRibbonTabItemBlueOrange(factory);
            rt.CornerSize = 2;
            rt.Name = Enum.GetName(typeof(eRibbonTabColor), eRibbonTabColor.Orange);
            table.RibbonTabItemColors.Add(rt);
            #endregion

            #region RibbonTabItemGroup Colors Initialization
            table.RibbonTabGroupColors.Clear();
            // Default
            Office2007RibbonTabGroupColorTable tg = Office2010BlueFactory.GetRibbonTabGroupDefault(factory);
            tg.Name = Enum.GetName(typeof(eRibbonTabGroupColor), eRibbonTabGroupColor.Default);
            table.RibbonTabGroupColors.Add(tg);

            // Magenta
            tg = Office2010BlueFactory.GetRibbonTabGroupMagenta(factory);
            tg.Name = Enum.GetName(typeof(eRibbonTabGroupColor), eRibbonTabGroupColor.Magenta);
            table.RibbonTabGroupColors.Add(tg);

            // Green
            tg = Office2010BlueFactory.GetRibbonTabGroupGreen(factory);
            tg.Name = Enum.GetName(typeof(eRibbonTabGroupColor), eRibbonTabGroupColor.Green);
            table.RibbonTabGroupColors.Add(tg);

            // Orange
            tg = Office2010BlueFactory.GetRibbonTabGroupOrange(factory);
            tg.Name = Enum.GetName(typeof(eRibbonTabGroupColor), eRibbonTabGroupColor.Orange);
            table.RibbonTabGroupColors.Add(tg);
            #endregion

            #region Initialize Bar
            table.Bar.ToolbarTopBackground = new LinearGradientColorTable(factory.GetColor(0x4B4B4B), factory.GetColor(0x3F3F3F));
            table.Bar.ToolbarBottomBackground = new LinearGradientColorTable(factory.GetColor(0x3F3F3F), factory.GetColor(0x333333));
            table.Bar.ToolbarBottomBorder = factory.GetColor(0x525252);
            table.Bar.PopupToolbarBackground = new LinearGradientColorTable(factory.GetColor(0xFFFFFF), Color.Empty);
            table.Bar.PopupToolbarBorder = factory.GetColor(0xA7ABB0);
            table.Bar.StatusBarTopBorder = factory.GetColor(0x2C2C2C);
            table.Bar.StatusBarTopBorderLight = factory.GetColor(Color.FromArgb(32, Color.White));
            table.Bar.StatusBarAltBackground.Clear();
            table.Bar.StatusBarAltBackground.Add(new BackgroundColorBlend(factory.GetColor(0x4B4B4B), 0f));
            table.Bar.StatusBarAltBackground.Add(new BackgroundColorBlend(factory.GetColor(0x333333), 1f));
            #endregion

            #region Menu
            table.Menu.Background = new LinearGradientColorTable(factory.GetColor(0xFFFFFF), Color.Empty);
            table.Menu.Border = new LinearGradientColorTable(factory.GetColor(0xA7ABB0), Color.Empty);
            table.Menu.Side = new LinearGradientColorTable(factory.GetColor(0xFFFFFF), Color.Empty);
            table.Menu.SideBorder = new LinearGradientColorTable(factory.GetColor(0xE2E4E7), Color.Empty);
            table.Menu.SideBorderLight = new LinearGradientColorTable(factory.GetColor(0xFFFFFF), Color.Empty);
            table.Menu.SideUnused = new LinearGradientColorTable(factory.GetColor(0xE5E5E5), Color.Empty);
            table.Menu.FileBackgroundBlend.Clear();
            table.Menu.FileBackgroundBlend.AddRange(new DevComponents.DotNetBar.BackgroundColorBlend[] {
                new DevComponents.DotNetBar.BackgroundColorBlend(factory.GetColor(0xFFFFFF), 0F),
                new DevComponents.DotNetBar.BackgroundColorBlend(factory.GetColor(0xFFFFFF), 1F)});
            table.Menu.FileContainerBorder = factory.GetColor(0xA7ABB0);
            table.Menu.FileContainerBorderLight = Color.Transparent;
            table.Menu.FileColumnOneBackground = factory.GetColor(0xFFFFFF);
            table.Menu.FileColumnOneBorder = factory.GetColor(0xA7ABB0);
            table.Menu.FileColumnTwoBackground = factory.GetColor(0xFFFFFF);
            table.Menu.FileBottomContainerBackgroundBlend.Clear();
            //table.Menu.FileBottomContainerBackgroundBlend.AddRange(new DevComponents.DotNetBar.BackgroundColorBlend[] {
            //    new DevComponents.DotNetBar.BackgroundColorBlend(factory.GetColor(0xEBF3FC), 0F),
            //    new DevComponents.DotNetBar.BackgroundColorBlend(factory.GetColor(0xEBF3FC), 1F)});
            #endregion

            #region ComboBox
            table.ComboBox.Default.Background = factory.GetColor(0xC6C6C6);
            table.ComboBox.Default.Border = factory.GetColor(0x919191);
            table.ComboBox.Default.ExpandBackground = new LinearGradientColorTable();
            table.ComboBox.Default.ExpandBorderInner = new LinearGradientColorTable();
            table.ComboBox.Default.ExpandBorderOuter = new LinearGradientColorTable();
            table.ComboBox.Default.ExpandText = factory.GetColor(0x505153);
            table.ComboBox.DefaultStandalone.Background = factory.GetColor(0xFFFFFF);
            table.ComboBox.DefaultStandalone.Border = factory.GetColor(0x313131);
            table.ComboBox.DefaultStandalone.ExpandBackground = new LinearGradientColorTable(factory.GetColor(0xE2E2E2), factory.GetColor(0xC4C4C4), 90);
            table.ComboBox.DefaultStandalone.ExpandBorderInner = LinearGradientColorTable.Empty;// new LinearGradientColorTable(factory.GetColor(0xFFFFFF));
            table.ComboBox.DefaultStandalone.ExpandBorderOuter = new LinearGradientColorTable(factory.GetColor(0x919191), Color.Empty, 90);
            table.ComboBox.DefaultStandalone.ExpandText = factory.GetColor(0x555658);
            table.ComboBox.MouseOver.Background = factory.GetColor(0xFFFFFF);
            table.ComboBox.MouseOver.Border = factory.GetColor(0x919191);
            table.ComboBox.MouseOver.ExpandBackground = new LinearGradientColorTable(factory.GetColor(0xF5E4A1), factory.GetColor(0xF6EEC9), 90);
            table.ComboBox.MouseOver.ExpandBorderInner = new LinearGradientColorTable(factory.GetColor(0xFBF6E3));
            table.ComboBox.MouseOver.ExpandBorderOuter = new LinearGradientColorTable(factory.GetColor(0xE8C14F), Color.Empty, 90);
            table.ComboBox.MouseOver.ExpandText = factory.GetColor(0x5C5641);
            table.ComboBox.DroppedDown.Background = factory.GetColor(0xFFFFFF);
            table.ComboBox.DroppedDown.Border = factory.GetColor(0x919191);
            table.ComboBox.DroppedDown.ExpandBackground = new LinearGradientColorTable(factory.GetColor(0xE4C9A7), factory.GetColor(0xEFBC61), 90);
            table.ComboBox.DroppedDown.ExpandBorderInner = new LinearGradientColorTable(factory.GetColor(0xD4C0A9), factory.GetColor(0xEFF1B9), 90);
            table.ComboBox.DroppedDown.ExpandBorderOuter = new LinearGradientColorTable(factory.GetColor(0xC2923B), Color.Empty, 90);
            table.ComboBox.DroppedDown.ExpandText = factory.GetColor(0x454F5A);
            #endregion

            #region Dialog Launcher
            table.DialogLauncher.Default.DialogLauncher = factory.GetColor(0x6B6B6B);
            table.DialogLauncher.Default.DialogLauncherShade = Color.Transparent;// factory.GetColor(172, 0xFFFFFF);

            table.DialogLauncher.MouseOver.DialogLauncher = factory.GetColor(0x968C64);
            table.DialogLauncher.MouseOver.DialogLauncherShade = Color.Transparent;
            table.DialogLauncher.MouseOver.TopBackground = new LinearGradientColorTable(factory.GetColor(0xF1DF99), factory.GetColor(0xF1D77B));
            table.DialogLauncher.MouseOver.BottomBackground = new LinearGradientColorTable(factory.GetColor(0xF1D77B), factory.GetColor(0xF1E3AF));
            table.DialogLauncher.MouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(0xFAF5E2), factory.GetColor(0xFAF7EE));
            table.DialogLauncher.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(0xE5BF4B), Color.Empty);

            table.DialogLauncher.Pressed.DialogLauncher = factory.GetColor(0x6B737D);
            table.DialogLauncher.Pressed.DialogLauncherShade = Color.FromArgb(128, factory.GetColor(0xFFFFFF));
            table.DialogLauncher.Pressed.TopBackground = new LinearGradientColorTable(factory.GetColor(0xE4C8A5), factory.GetColor(0xEDC891));
            table.DialogLauncher.Pressed.BottomBackground = new LinearGradientColorTable(factory.GetColor(0xEEBC62), factory.GetColor(0xEEE48B));
            table.DialogLauncher.Pressed.InnerBorder = new LinearGradientColorTable(factory.GetColor(0xD4BFA8), factory.GetColor(0xEFF1C2));
            table.DialogLauncher.Pressed.OuterBorder = new LinearGradientColorTable(factory.GetColor(0xC2923B), factory.GetColor(0xC2B462));
            #endregion

            #region System Button, Form
            // Default state no background
            table.SystemButton.Default = new Office2007SystemButtonStateColorTable();
            table.SystemButton.Default.Foreground = new LinearGradientColorTable(factory.GetColor(0xDCDCDC));
            table.SystemButton.Default.LightShade = Color.Empty;
            table.SystemButton.Default.DarkShade = factory.GetColor(0x525565);

            // Mouse over state
            table.SystemButton.MouseOver = new Office2007SystemButtonStateColorTable();
            table.SystemButton.MouseOver.Foreground = new LinearGradientColorTable(factory.GetColor(0xDCDCDC));
            table.SystemButton.MouseOver.LightShade = Color.Empty;
            table.SystemButton.MouseOver.DarkShade = factory.GetColor(0x525565);
            table.SystemButton.MouseOver.TopBackground = new LinearGradientColorTable(factory.GetColor(0x949494), factory.GetColor(0x8B8B8B));
            table.SystemButton.MouseOver.BottomBackground = new LinearGradientColorTable(factory.GetColor(0x8B8B8B), factory.GetColor(0x777777));
            table.SystemButton.MouseOver.TopHighlight = null; // new LinearGradientColorTable(factory.GetColor(0xFBFCFF), Color.Transparent);
            table.SystemButton.MouseOver.BottomHighlight = null; // new LinearGradientColorTable(factory.GetColor(0xFBFCFF), Color.Transparent);
            table.SystemButton.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(0x515151));
            table.SystemButton.MouseOver.InnerBorder = LinearGradientColorTable.Empty;

            // Pressed
            table.SystemButton.Pressed = new Office2007SystemButtonStateColorTable();
            table.SystemButton.Pressed.Foreground = new LinearGradientColorTable(factory.GetColor(0xDCDCDC));
            table.SystemButton.Pressed.LightShade = Color.Empty;
            table.SystemButton.Pressed.DarkShade = factory.GetColor(0x525565);
            table.SystemButton.Pressed.TopBackground = new LinearGradientColorTable(factory.GetColor(0x686868));
            table.SystemButton.Pressed.TopHighlight = null;// new LinearGradientColorTable(factory.GetColor(0xB8CEE9), Color.Transparent);
            table.SystemButton.Pressed.BottomBackground = new LinearGradientColorTable(factory.GetColor(0x717171));
            table.SystemButton.Pressed.BottomHighlight = null; // new LinearGradientColorTable(factory.GetColor(0xC6EAFD), Color.Transparent);
            table.SystemButton.Pressed.OuterBorder = new LinearGradientColorTable(factory.GetColor(0x515151));
            table.SystemButton.Pressed.InnerBorder = new LinearGradientColorTable(factory.GetColor(0x636363));

            // CLOSE Default state no background
            table.SystemButtonClose = new Office2007SystemButtonColorTable();
            table.SystemButtonClose.Default = new Office2007SystemButtonStateColorTable();
            table.SystemButtonClose.Default.Foreground = new LinearGradientColorTable(factory.GetColor(0xDCDCDC));
            table.SystemButtonClose.Default.LightShade = Color.Empty;
            table.SystemButtonClose.Default.DarkShade = factory.GetColor(0x525565);

            // Mouse over state
            table.SystemButtonClose.MouseOver = new Office2007SystemButtonStateColorTable();
            table.SystemButtonClose.MouseOver.Foreground = new LinearGradientColorTable(factory.GetColor(0xDCDCDC));
            table.SystemButtonClose.MouseOver.LightShade = Color.Empty;
            table.SystemButtonClose.MouseOver.DarkShade = factory.GetColor(0x9B3D3D);
            table.SystemButtonClose.MouseOver.TopBackground = new LinearGradientColorTable(factory.GetColor(0xFF8482), factory.GetColor(0xFB7F7E));
            table.SystemButtonClose.MouseOver.BottomBackground = new LinearGradientColorTable(factory.GetColor(0xF67979), factory.GetColor(0xE36162));
            table.SystemButtonClose.MouseOver.TopHighlight = null; // new LinearGradientColorTable(factory.GetColor(0xFBFCFF), Color.Transparent);
            table.SystemButtonClose.MouseOver.BottomHighlight = null; // new LinearGradientColorTable(factory.GetColor(0xFBFCFF), Color.Transparent);
            table.SystemButtonClose.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(0x9B3D3D));
            table.SystemButtonClose.MouseOver.InnerBorder = LinearGradientColorTable.Empty;

            // Pressed
            table.SystemButtonClose.Pressed = new Office2007SystemButtonStateColorTable();
            table.SystemButtonClose.Pressed.Foreground = new LinearGradientColorTable(factory.GetColor(0xDCDCDC));
            table.SystemButtonClose.Pressed.LightShade = Color.Empty;
            table.SystemButtonClose.Pressed.DarkShade = factory.GetColor(0x9B3D3D);
            table.SystemButtonClose.Pressed.TopBackground = new LinearGradientColorTable(factory.GetColor(0xF27776));
            table.SystemButtonClose.Pressed.TopHighlight = null;// new LinearGradientColorTable(factory.GetColor(0xB8CEE9), Color.Transparent);
            table.SystemButtonClose.Pressed.BottomBackground = new LinearGradientColorTable(factory.GetColor(0xF27776));
            table.SystemButtonClose.Pressed.BottomHighlight = null; // new LinearGradientColorTable(factory.GetColor(0xC6EAFD), Color.Transparent);
            table.SystemButtonClose.Pressed.OuterBorder = new LinearGradientColorTable(factory.GetColor(0x9B3D3D));
            table.SystemButtonClose.Pressed.InnerBorder = new LinearGradientColorTable(factory.GetColor(0xDA6163));

            // Form border
            table.Form.Active.BorderColors = new Color[] {
                factory.GetColor(0x636363),
                factory.GetColor(0x838383),
                factory.GetColor(0x777777),
                factory.GetColor(0x777777),
                factory.GetColor(0x777777)
            };

            table.Form.Inactive.BorderColors = new Color[] {
                factory.GetColor(0x777777),
                factory.GetColor(0x9E9E9E),
                factory.GetColor(0x9E9E9E),
                factory.GetColor(0x9E9E9E),
                factory.GetColor(0x9E9E9E)
            };

            // Form Caption Active
            table.Form.Active.CaptionTopBackground = new LinearGradientColorTable(factory.GetColor(0x797979), factory.GetColor(0x797979));
            table.Form.Active.CaptionBottomBackground = new LinearGradientColorTable(factory.GetColor(0x787878), factory.GetColor(0x717171));
            table.Form.Active.CaptionBottomBorder = new Color[] { factory.GetColor(0x5E5E5E), factory.GetColor(0x838383) };
            table.Form.Active.CaptionText = factory.GetColor(0xFFFFFF);
            table.Form.Active.CaptionTextExtra = factory.GetColor(0xE2E2E2);

            // Form Caption Inactive
            table.Form.Inactive.CaptionTopBackground = new LinearGradientColorTable(factory.GetColor(0x9E9E9E));
            table.Form.Inactive.CaptionBottomBackground = new LinearGradientColorTable(factory.GetColor(0x9E9E9E));
            table.Form.Inactive.CaptionText = factory.GetColor(0xD4D4D4);
            table.Form.Inactive.CaptionTextExtra = factory.GetColor(0xD4D4D4);

            table.Form.BackColor = factory.GetColor(0xD3D3D3);
            table.Form.TextColor = factory.GetColor(0x000000);

            table.Form.MdiClientBackgroundImage = BarFunctions.LoadBitmap("SystemImages.Office2010BlackClientBackground.png");
            #endregion

            #region Quick Access Toolbar Background
            table.QuickAccessToolbar.Active.TopBackground = LinearGradientColorTable.Empty; // new LinearGradientColorTable(factory.GetColor(0xDEE7F4), factory.GetColor(0xE6EEF9));
            table.QuickAccessToolbar.Active.BottomBackground = LinearGradientColorTable.Empty; //new LinearGradientColorTable(factory.GetColor(0xDBE7F7), factory.GetColor(0xC9D9EE));
            table.QuickAccessToolbar.Active.OutterBorderColor = Color.Empty; // factory.GetColor(0xF6F9FC);
            table.QuickAccessToolbar.Active.MiddleBorderColor = Color.Empty; // factory.GetColor(0x9AB3D5);
            table.QuickAccessToolbar.Active.InnerBorderColor = Color.Empty; //  factory.GetColor(0xD2E3F9);

            table.QuickAccessToolbar.Inactive.TopBackground = LinearGradientColorTable.Empty; //new LinearGradientColorTable(factory.GetColor(0xE6ECF3), factory.GetColor(0xCED8E6));
            table.QuickAccessToolbar.Inactive.BottomBackground = LinearGradientColorTable.Empty; // new LinearGradientColorTable(factory.GetColor(0xCED8E6), factory.GetColor(0xC8D3E3));
            table.QuickAccessToolbar.Inactive.OutterBorderColor = Color.Empty; // factory.GetColor(0xF6F9FC);
            table.QuickAccessToolbar.Inactive.MiddleBorderColor = Color.Empty; // factory.GetColor(0x9AB3D5);
            table.QuickAccessToolbar.Inactive.InnerBorderColor = Color.Empty;

            table.QuickAccessToolbar.Standalone.TopBackground = new LinearGradientColorTable();
            table.QuickAccessToolbar.Standalone.BottomBackground = new LinearGradientColorTable(factory.GetColor(0x797979));
            table.QuickAccessToolbar.Standalone.OutterBorderColor = factory.GetColor(0x848484);
            table.QuickAccessToolbar.Standalone.MiddleBorderColor = Color.Empty;
            table.QuickAccessToolbar.Standalone.InnerBorderColor = Color.Empty; // factory.GetColor(0xDCE8F7);

            table.QuickAccessToolbar.QatCustomizeMenuLabelBackground = factory.GetColor(0xF0F2F5);
            table.QuickAccessToolbar.QatCustomizeMenuLabelText = factory.GetColor(0x3B3B3B);

            table.QuickAccessToolbar.Active.GlassBorder = LinearGradientColorTable.Empty; // new LinearGradientColorTable(factory.GetColor(Color.FromArgb(132, Color.Black)), Color.FromArgb(80, Color.Black));
            table.QuickAccessToolbar.Inactive.GlassBorder = LinearGradientColorTable.Empty; // new LinearGradientColorTable(factory.GetColor(Color.FromArgb(132, Color.Black)), Color.FromArgb(80, Color.Black));
            #endregion

            #region Tab Colors
            table.TabControl.Default = new Office2007TabItemStateColorTable();
            table.TabControl.Default.TopBackground = new LinearGradientColorTable(factory.GetColor(0x919191), factory.GetColor(0x7E7E7E));
            table.TabControl.Default.BottomBackground = new LinearGradientColorTable(factory.GetColor(0x7E7E7E), factory.GetColor(0x6C6C6C));
            table.TabControl.Default.InnerBorder = factory.GetColor(0x919191);
            table.TabControl.Default.OuterBorder = factory.GetColor(0x313131);
            table.TabControl.Default.Text = factory.GetColor(0xFFFFFF);

            table.TabControl.MouseOver = new Office2007TabItemStateColorTable();
            table.TabControl.MouseOver.TopBackground = new LinearGradientColorTable(factory.GetColor(0xC1C1C1), factory.GetColor(0xAEAEAE));
            table.TabControl.MouseOver.BottomBackground = new LinearGradientColorTable(factory.GetColor(0xAEAEAE), factory.GetColor(0x919191));
            table.TabControl.MouseOver.InnerBorder = factory.GetColor(0x919191);
            table.TabControl.MouseOver.OuterBorder = factory.GetColor(0x343434);
            table.TabControl.MouseOver.Text = factory.GetColor(0x000000);

            table.TabControl.Selected = new Office2007TabItemStateColorTable();
            table.TabControl.Selected.TopBackground = new LinearGradientColorTable(factory.GetColor(0x919191), factory.GetColor(0xFFFFFF));
            table.TabControl.Selected.BottomBackground = new LinearGradientColorTable(factory.GetColor(0xFFFFFF), factory.GetColor(0xFFFFFF));
            table.TabControl.Selected.InnerBorder = factory.GetColor(0x919191);
            table.TabControl.Selected.OuterBorder = factory.GetColor(0x343434);
            table.TabControl.Selected.Text = factory.GetColor(0x000000);

            table.TabControl.TabBackground = new LinearGradientColorTable(factory.GetColor(0x6A6A6A), Color.Empty);
            table.TabControl.TabPanelBackground = new LinearGradientColorTable(factory.GetColor(0xFFFFFF), Color.Empty);
            table.TabControl.TabPanelBorder = factory.GetColor(0x313131);
            #endregion

            #region CheckBoxItem
            Office2007CheckBoxColorTable chk = table.CheckBoxItem;
            chk.Default.CheckBackground = new LinearGradientColorTable(factory.GetColor(0xF2F3F5), Color.Empty);
            chk.Default.CheckBorder = factory.GetColor(0x898B8C);
            chk.Default.CheckInnerBackground = new LinearGradientColorTable(factory.GetColor(0xE8E9EB), factory.GetColor(0xF3F3F4));
            chk.Default.CheckInnerBorder = factory.GetColor(0xB6B8BA);
            chk.Default.CheckSign = new LinearGradientColorTable(factory.GetColor(0x717374), Color.Empty);
            chk.Default.Text = factory.GetColor(0x000000);

            chk.MouseOver.CheckBackground = new LinearGradientColorTable(factory.GetColor(0xFCF1C2), Color.Empty);
            chk.MouseOver.CheckBorder = factory.GetColor(0xCF9037);
            chk.MouseOver.CheckInnerBackground = new LinearGradientColorTable(factory.GetColor(0xFAECC8), factory.GetColor(0xFCF3DD));
            chk.MouseOver.CheckInnerBorder = factory.GetColor(0xFCAD5D);
            chk.MouseOver.CheckSign = new LinearGradientColorTable(factory.GetColor(0x4B4944), Color.Empty);
            chk.MouseOver.Text = factory.GetColor(0x000000);

            chk.Pressed.CheckBackground = new LinearGradientColorTable(factory.GetColor(0xFCE7BD));
            chk.Pressed.CheckBorder = factory.GetColor(0xC26D1D);
            chk.Pressed.CheckInnerBackground = new LinearGradientColorTable(factory.GetColor(0xFAE0A2), factory.GetColor(0xFDF2D7));
            chk.Pressed.CheckInnerBorder = factory.GetColor(0xFCA558);
            chk.Pressed.CheckSign = new LinearGradientColorTable(factory.GetColor(0x4B4944), Color.Empty);
            chk.Pressed.Text = factory.GetColor(0x3B3B3B);

            chk.Disabled.CheckBackground = new LinearGradientColorTable(factory.GetColor(0xFFFFFF), Color.Empty);
            chk.Disabled.CheckBorder = factory.GetColor(0xB6B8BA);
            chk.Disabled.CheckInnerBackground = new LinearGradientColorTable(factory.GetColor(0xF8F8F9));
            chk.Disabled.CheckInnerBorder = factory.GetColor(0xDFE0E0);
            chk.Disabled.CheckSign = new LinearGradientColorTable(factory.GetColor(0xB1B3B4), Color.Empty);
            chk.Disabled.Text = factory.GetColor(0x8D8D8D);
            #endregion

            #region Scroll Bar Colors
            InitializeScrollBarColorTable(table, factory);
            InitializeAppScrollBarColorTable(table, factory);
            #endregion

            #region ProgressBarItem
            Office2007ProgressBarColorTable pct = table.ProgressBarItem;
            pct.BackgroundColors = new GradientColorTable(0x87898B, 0x979897);
            pct.OuterBorder = factory.GetColor(0x373739);
            pct.InnerBorder = factory.GetColor(0xA6A6A6);
            pct.Chunk = new GradientColorTable(0x69922A, 0xE7F2D4, 0);
            pct.ChunkOverlay = new GradientColorTable();
            pct.ChunkOverlay.LinearGradientAngle = 90;
            pct.ChunkOverlay.Colors.AddRange(new BackgroundColorBlend[] {
                new BackgroundColorBlend(Color.FromArgb(192, factory.GetColor(0xEEFFD7)), 0f),
                new BackgroundColorBlend(Color.FromArgb(128, factory.GetColor(0x8DB254)), .5f),
                new BackgroundColorBlend(Color.FromArgb(64, factory.GetColor(0x69922B)), .5f),
                new BackgroundColorBlend(Color.Transparent, 1f),
            });
            pct.ChunkShadow = new GradientColorTable(0x7D7E7F, 0x909190, 0);

            // Paused State
            pct = table.ProgressBarItemPaused;
            pct.BackgroundColors = new GradientColorTable(0x87898B, 0x979897);
            pct.OuterBorder = factory.GetColor(0x373739);
            pct.InnerBorder = factory.GetColor(0xA6A6A6);
            pct.Chunk = new GradientColorTable(0xAEA700, 0xFFFDCD, 0);
            pct.ChunkOverlay = new GradientColorTable();
            pct.ChunkOverlay.LinearGradientAngle = 90;
            pct.ChunkOverlay.Colors.AddRange(new BackgroundColorBlend[] {
                new BackgroundColorBlend(Color.FromArgb(192, factory.GetColor(0xFFFBA3)), 0f),
                new BackgroundColorBlend(Color.FromArgb(128, factory.GetColor(0xD2CA00)), .5f),
                new BackgroundColorBlend(Color.FromArgb(64, factory.GetColor(0xFEF400)), .5f),
                new BackgroundColorBlend(Color.Transparent, 1f),
            });
            pct.ChunkShadow = new GradientColorTable(0x7D7E7F, 0x909190, 0);

            // Error State
            pct = table.ProgressBarItemError;
            pct.BackgroundColors = new GradientColorTable(0x87898B, 0x979897);
            pct.OuterBorder = factory.GetColor(0x373739);
            pct.InnerBorder = factory.GetColor(0xA6A6A6);
            pct.Chunk = new GradientColorTable(0xD20000, 0xFFCDCD, 0);
            pct.ChunkOverlay = new GradientColorTable();
            pct.ChunkOverlay.LinearGradientAngle = 90;
            pct.ChunkOverlay.Colors.AddRange(new BackgroundColorBlend[] {
                new BackgroundColorBlend(Color.FromArgb(192, factory.GetColor(0xFF8F8F)), 0f),
                new BackgroundColorBlend(Color.FromArgb(128, factory.GetColor(0xD20000)), .5f),
                new BackgroundColorBlend(Color.FromArgb(64, factory.GetColor(0xFE0000)), .5f),
                new BackgroundColorBlend(Color.Transparent, 1f),
            });
            pct.ChunkShadow = new GradientColorTable(0x7D7E7F, 0x909190, 0);
            #endregion

            #region Gallery
            Office2007GalleryColorTable gallery = table.Gallery;
            gallery.GroupLabelBackground = factory.GetColor(0xF0F2F5);
            gallery.GroupLabelText = factory.GetColor(0x3B3B3B);
            gallery.GroupLabelBorder = factory.GetColor(0xF0F2F5);
            #endregion

            #region Legacy Colors
            table.LegacyColors.BarBackground = factory.GetColor(0xC9C9C9);
            table.LegacyColors.BarBackground2 = factory.GetColor(0xA3A3A3);
            table.LegacyColors.BarStripeColor = factory.GetColor(0x525252);
            table.LegacyColors.BarCaptionBackground = factory.GetColor(0x686868);
            table.LegacyColors.BarCaptionBackground2 = Color.Empty;
            table.LegacyColors.BarCaptionInactiveBackground = factory.GetColor(0x7c7c7c);
            table.LegacyColors.BarCaptionInactiveBackground2 = Color.Empty;
            table.LegacyColors.BarCaptionInactiveText = factory.GetColor(0xFFFFFF);
            table.LegacyColors.BarCaptionText = factory.GetColor(0xFFFFFF);
            table.LegacyColors.BarFloatingBorder = factory.GetColor(0x444444);
            table.LegacyColors.BarPopupBackground = factory.GetColor(0xFFFFFF);
            table.LegacyColors.BarPopupBorder = factory.GetColor(0xA7ABB0);
            table.LegacyColors.ItemBackground = Color.Empty;
            table.LegacyColors.ItemBackground2 = Color.Empty;
            table.LegacyColors.ItemCheckedBackground = factory.GetColor(0xFCD578);
            table.LegacyColors.ItemCheckedBackground2 = factory.GetColor(0xFBC84F);
            table.LegacyColors.ItemCheckedBorder = factory.GetColor(0xBB5503);
            table.LegacyColors.ItemCheckedText = factory.GetColor(0x000000);
            table.LegacyColors.ItemDisabledBackground = Color.Empty;
            table.LegacyColors.ItemDisabledText = factory.GetColor(0x8D8D8D);
            table.LegacyColors.ItemExpandedShadow = Color.Empty;
            table.LegacyColors.ItemExpandedBackground = factory.GetColor(0xE3EFFE);
            table.LegacyColors.ItemExpandedBackground2 = factory.GetColor(0x99BFF0);
            table.LegacyColors.ItemExpandedText = factory.GetColor(0x000000);
            table.LegacyColors.ItemHotBackground = factory.GetColor(0xFFF5CC);
            table.LegacyColors.ItemHotBackground2 = factory.GetColor(0xFFDB75);
            table.LegacyColors.ItemHotBorder = factory.GetColor(0xFFBD69);
            table.LegacyColors.ItemHotText = factory.GetColor(0x3B3B3B);
            table.LegacyColors.ItemPressedBackground = factory.GetColor(0xFC973D);
            table.LegacyColors.ItemPressedBackground2 = factory.GetColor(0xFFB85E);
            table.LegacyColors.ItemPressedBorder = factory.GetColor(0xFB8C3C);
            table.LegacyColors.ItemPressedText = factory.GetColor(0x3B3B3B);
            table.LegacyColors.ItemSeparator = factory.GetColor(0x868B91);
            table.LegacyColors.ItemSeparatorShade = Color.FromArgb(192, factory.GetColor(0xFDFDFE));
            table.LegacyColors.ItemText = factory.GetColor(0x000000); // SystemColors.ControlTet;
            table.LegacyColors.MenuBackground = factory.GetColor(0xFFFFFF);
            table.LegacyColors.MenuBackground2 = Color.Empty; // Color.White;
            table.LegacyColors.MenuBarBackground = factory.GetColor(0xC9C9C9);
            table.LegacyColors.MenuBorder = factory.GetColor(0xA7ABB0);
            table.LegacyColors.ItemExpandedBorder = table.LegacyColors.MenuBorder;
            table.LegacyColors.MenuSide = factory.GetColor(0xFFFFFF);
            table.LegacyColors.MenuSide2 = Color.Empty; // factory.GetColor(0xDDE0E8);
            table.LegacyColors.MenuUnusedBackground = table.LegacyColors.MenuBackground;
            table.LegacyColors.MenuUnusedSide = factory.GetColor(0xDADADA);
            table.LegacyColors.MenuUnusedSide2 = Color.Empty;// System.Windows.Forms.ControlPaint.Light(table.LegacyColors.MenuSide2);
            table.LegacyColors.ItemDesignTimeBorder = Color.Black;
            table.LegacyColors.BarDockedBorder = factory.GetColor(0x5E5E5E);
            table.LegacyColors.DockSiteBackColor = factory.GetColor(0xC9C9C9);
            table.LegacyColors.DockSiteBackColor2 = Color.Empty;
            table.LegacyColors.CustomizeBackground = factory.GetColor(0x848484);
            table.LegacyColors.CustomizeBackground2 = factory.GetColor(0x797979);
            table.LegacyColors.CustomizeText = factory.GetColor(0x000000);
            table.LegacyColors.PanelBackground = factory.GetColor(0xCFCFCF);
            table.LegacyColors.PanelBackground2 = factory.GetColor(0xC9C9C9);
            table.LegacyColors.PanelText = factory.GetColor(0x000000);
            table.LegacyColors.PanelBorder = factory.GetColor(0x323131);
            table.LegacyColors.ExplorerBarBackground = factory.GetColor(0xCFCFCF);
            table.LegacyColors.ExplorerBarBackground2 = factory.GetColor(0xC9C9C9);
            #endregion

            #region Navigation Pane
            table.NavigationPane.ButtonBackground = new GradientColorTable();
            table.NavigationPane.ButtonBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xA6A6A6), 0));
            table.NavigationPane.ButtonBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xA6A6A6), 1));
            #endregion

            #region SuperTooltip
            table.SuperTooltip.BackgroundColors = new LinearGradientColorTable(factory.GetColor(0xFFFFFF), factory.GetColor(0xE4E4F0));
            table.SuperTooltip.TextColor = factory.GetColor(0x4C4C4C);
            #endregion

            #region Slider
            Office2007SliderColorTable sl = table.Slider;
            sl.Default.LabelColor = factory.GetColor(0x000000);
            sl.Default.SliderLabelColor = factory.GetColor(0x000000);
            sl.Default.PartBackground = new GradientColorTable();
            sl.Default.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xC0C0C0), 0));
            sl.Default.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(0x7C7C7C), 1f));
            sl.Default.PartBorderColor = factory.GetColor(0x262626);
            sl.Default.PartBorderLightColor = Color.Transparent; // Color.FromArgb(96, factory.GetColor(0xFFFFFF));
            sl.Default.PartForeColor = factory.GetColor(0x333333);
            sl.Default.PartForeLightColor = Color.Empty;
            sl.Default.TrackLineColor = factory.GetColor(0x262626);
            sl.Default.TrackLineLightColor = Color.FromArgb(96, factory.GetColor(0xFFFFFF));

            sl.MouseOver.LabelColor = factory.GetColor(0x000000);
            sl.MouseOver.SliderLabelColor = factory.GetColor(0x000000);
            sl.MouseOver.PartBackground = new GradientColorTable();
            sl.MouseOver.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xFFFFFF), 0));
            sl.MouseOver.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xFFFDF5), .2f));
            sl.MouseOver.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xFFDF83), .5f));
            sl.MouseOver.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xDDA70D), .5f));
            sl.MouseOver.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xFFF4CE), .85f));
            sl.MouseOver.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xFFF4CE), 1f));
            sl.MouseOver.PartBorderColor = factory.GetColor(0x7F8994);
            sl.MouseOver.PartBorderLightColor = Color.FromArgb(96, factory.GetColor(0xFFFFFF));
            sl.MouseOver.PartForeColor = factory.GetColor(0x4E5052);
            sl.MouseOver.PartForeLightColor = Color.Empty;
            sl.MouseOver.TrackLineColor = factory.GetColor(0x262626);
            sl.MouseOver.TrackLineLightColor = factory.GetColor(0xCCCCCC);

            sl.Pressed.LabelColor = factory.GetColor(0x000000);
            sl.Pressed.SliderLabelColor = factory.GetColor(0x000000);
            sl.Pressed.PartBackground = new GradientColorTable();
            sl.Pressed.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xDB7518), 0));
            sl.Pressed.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xF7902F), .2f));
            sl.Pressed.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xF9C18B), .5f));
            sl.Pressed.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xED7400), .5f));
            sl.Pressed.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xFFC188), .85f));
            sl.Pressed.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xF2D2B5), 1f));
            sl.Pressed.PartBorderColor = factory.GetColor(0x7F8994);
            sl.Pressed.PartBorderLightColor = Color.FromArgb(96, factory.GetColor(0xFFFFFF));
            sl.Pressed.PartForeColor = factory.GetColor(0x4E5052);
            sl.Pressed.PartForeLightColor = Color.Empty;
            sl.Pressed.TrackLineColor = factory.GetColor(0x262626);
            sl.Pressed.TrackLineLightColor = factory.GetColor(0xCCCCCC);

            ColorBlendFactory df = new ColorBlendFactory(ColorScheme.GetColor(0xCFCFCF));
            sl.Disabled.LabelColor = factory.GetColor(0x8D8D8D);
            sl.Disabled.SliderLabelColor = factory.GetColor(0x8D8D8D);
            sl.Disabled.PartBackground = new GradientColorTable();
            foreach (BackgroundColorBlend b in sl.Default.PartBackground.Colors)
                sl.Disabled.PartBackground.Colors.Add(new BackgroundColorBlend(df.GetColor(b.Color), b.Position));
            sl.Disabled.PartBorderColor = df.GetColor(sl.Default.PartBorderColor);
            sl.Disabled.PartBorderLightColor = df.GetColor(sl.Default.PartBorderLightColor);
            sl.Disabled.PartForeColor = df.GetColor(sl.Default.PartForeColor);
            sl.Disabled.PartForeLightColor = df.GetColor(sl.Default.PartForeLightColor);
            sl.Disabled.TrackLineColor = df.GetColor(sl.Default.TrackLineColor);
            sl.Disabled.TrackLineLightColor = df.GetColor(sl.Default.TrackLineLightColor);
            #endregion

            #region ListViewEx
            table.ListViewEx.Border = factory.GetColor(0x313131);
            table.ListViewEx.ColumnBackground = new LinearGradientColorTable(factory.GetColor(0xEAEAEA), factory.GetColor(0xDFDFDF));
            table.ListViewEx.ColumnSeparator = factory.GetColor(0x3B3B3B);
            table.ListViewEx.SelectionBackground = new LinearGradientColorTable(factory.GetColor(0xA7CDF0), Color.Empty);
            table.ListViewEx.SelectionBorder = factory.GetColor(0x58320F);
            #endregion

            #region DataGridView
            table.DataGridView.ColumnHeaderNormalBorder = factory.GetColor(0x868B91);
            table.DataGridView.ColumnHeaderNormalBackground = new LinearGradientColorTable(factory.GetColor(0xEBEDF0), factory.GetColor(0xD5D8DC), 90);
            table.DataGridView.ColumnHeaderSelectedBackground = new LinearGradientColorTable(factory.GetColor(0xF9D99F), factory.GetColor(0xF1C15F), 90);
            table.DataGridView.ColumnHeaderSelectedBorder = factory.GetColor(0xF29536);
            table.DataGridView.ColumnHeaderSelectedMouseOverBackground = new LinearGradientColorTable(factory.GetColor(0xFFD58D), factory.GetColor(0xF2923A), 90);
            table.DataGridView.ColumnHeaderSelectedMouseOverBorder = factory.GetColor(0xF29536);
            table.DataGridView.ColumnHeaderMouseOverBackground = new LinearGradientColorTable(factory.GetColor(0xDFE2E4), factory.GetColor(0xBCC5D2), 90);
            table.DataGridView.ColumnHeaderMouseOverBorder = factory.GetColor(0x879FB7);
            table.DataGridView.ColumnHeaderPressedBackground = new LinearGradientColorTable(factory.GetColor(0xBCC5D2), factory.GetColor(0xDFE2E4), 90);
            table.DataGridView.ColumnHeaderPressedBorder = factory.GetColor(0xFFFFFF);

            table.DataGridView.RowNormalBackground = new LinearGradientColorTable(factory.GetColor(0xEBEDF0));
            table.DataGridView.RowNormalBorder = factory.GetColor(0x868B91);
            table.DataGridView.RowSelectedBackground = new LinearGradientColorTable(factory.GetColor(0xFFD58D));
            table.DataGridView.RowSelectedBorder = factory.GetColor(0xF29536);
            table.DataGridView.RowSelectedMouseOverBackground = new LinearGradientColorTable(factory.GetColor(0xF1C05C));
            table.DataGridView.RowSelectedMouseOverBorder = factory.GetColor(0xF29536);
            table.DataGridView.RowMouseOverBackground = new LinearGradientColorTable(factory.GetColor(0xDFE2E4));
            table.DataGridView.RowMouseOverBorder = factory.GetColor(0x879FB7);
            table.DataGridView.RowPressedBackground = new LinearGradientColorTable(factory.GetColor(0xBBC4D1));
            table.DataGridView.RowPressedBorder = factory.GetColor(0xFFFFFF);

            table.DataGridView.GridColor = factory.GetColor(0xAAAAAA);

            table.DataGridView.SelectorBackground = new LinearGradientColorTable(factory.GetColor(0xC6C6C6));
            table.DataGridView.SelectorBorder = factory.GetColor(0x909192);
            table.DataGridView.SelectorBorderDark = factory.GetColor(0xC3C3C3);
            table.DataGridView.SelectorBorderLight = factory.GetColor(0xF9F9F9);
            table.DataGridView.SelectorSign = new LinearGradientColorTable(factory.GetColor(0xF2F2F2));

            table.DataGridView.SelectorMouseOverBackground = new LinearGradientColorTable(factory.GetColor(0x8BA0B5));
            table.DataGridView.SelectorMouseOverBorder = factory.GetColor(0x9EB6CE);
            table.DataGridView.SelectorMouseOverBorderDark = factory.GetColor(0xB0CFF7);
            table.DataGridView.SelectorMouseOverBorderLight = factory.GetColor(0xD5E4F2);
            table.DataGridView.SelectorMouseOverSign = new LinearGradientColorTable(factory.GetColor(0xF9FAFB), factory.GetColor(0xD7DAE2));
            #endregion

            #region SideBar
            table.SideBar.Background = new LinearGradientColorTable(factory.GetColor(Color.White));
            table.SideBar.Border = factory.GetColor(0x313131);
            table.SideBar.SideBarPanelItemText = factory.GetColor(0x3B3B3B);
            table.SideBar.SideBarPanelItemDefault = new GradientColorTable();
            table.SideBar.SideBarPanelItemDefault.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xDBE1E7), 0));
            table.SideBar.SideBarPanelItemDefault.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xBBC3CD), 1));
            // Expanded
            table.SideBar.SideBarPanelItemExpanded = new GradientColorTable();
            table.SideBar.SideBarPanelItemExpanded.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xFBDBB5), 0));
            table.SideBar.SideBarPanelItemExpanded.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xFEC778), .4f));
            table.SideBar.SideBarPanelItemExpanded.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xFEB456), .4f));
            table.SideBar.SideBarPanelItemExpanded.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xFDEB9F), 1));
            // MouseOver
            table.SideBar.SideBarPanelItemMouseOver = new GradientColorTable();
            table.SideBar.SideBarPanelItemMouseOver.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xFFFCD9), 0));
            table.SideBar.SideBarPanelItemMouseOver.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xFFE78D), .4f));
            table.SideBar.SideBarPanelItemMouseOver.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xFFD748), .4f));
            table.SideBar.SideBarPanelItemMouseOver.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xFFE793), 1));
            // Pressed
            table.SideBar.SideBarPanelItemPressed = new GradientColorTable();
            table.SideBar.SideBarPanelItemPressed.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xF8B869), 0));
            table.SideBar.SideBarPanelItemPressed.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xFDA361), .4f));
            table.SideBar.SideBarPanelItemPressed.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xFB8A3C), .4f));
            table.SideBar.SideBarPanelItemPressed.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xFEBB60), 1));
            #endregion

            #region AdvTree
#if !NOTREE
            table.AdvTree = new DevComponents.AdvTree.Display.TreeColorTable();
            DevComponents.AdvTree.Display.ColorTableInitializer.InitOffice2007Blue(table.AdvTree, factory);
#endif
            #endregion

            #region CrumbBar
            table.CrumbBarItemView = new CrumbBarItemViewColorTable();
            CrumbBarItemViewStateColorTable crumbBarViewTable = new CrumbBarItemViewStateColorTable();
            table.CrumbBarItemView.Default = crumbBarViewTable;
            crumbBarViewTable.Foreground = factory.GetColor(0x3B3B3B);
            crumbBarViewTable = new CrumbBarItemViewStateColorTable();
            table.CrumbBarItemView.MouseOver = crumbBarViewTable;
            crumbBarViewTable.Foreground = factory.GetColor(0x3B3B3B);
            crumbBarViewTable.Background = new BackgroundColorBlendCollection();
            crumbBarViewTable.Background.AddRange(new BackgroundColorBlend[]{
                new BackgroundColorBlend(factory.GetColor("FFFCD9"), 0f),
                new BackgroundColorBlend(factory.GetColor("FFE78D"), .4f),
                new BackgroundColorBlend(factory.GetColor("FFD748"), .4f),
                new BackgroundColorBlend(factory.GetColor("FFE793"), 1f)});
            crumbBarViewTable.Border = factory.GetColor("FFB8A98E");
            crumbBarViewTable.BorderLight = factory.GetColor("90FFFFFF");
            crumbBarViewTable = new CrumbBarItemViewStateColorTable();
            table.CrumbBarItemView.MouseOverInactive = crumbBarViewTable;
            crumbBarViewTable.Foreground = factory.GetColor(0x3B3B3B);
            crumbBarViewTable.Background = new BackgroundColorBlendCollection();
            crumbBarViewTable.Background.AddRange(new BackgroundColorBlend[]{
                new BackgroundColorBlend(factory.GetColor("FFFFFDEC"), 0f),
                new BackgroundColorBlend(factory.GetColor("FFFFF4CA"), .4f),
                new BackgroundColorBlend(factory.GetColor("FFFFEBA6"), .4f),
                new BackgroundColorBlend(factory.GetColor("FFFFF2C5"), 1f)});
            crumbBarViewTable.Border = factory.GetColor("FF8E8F8F");
            crumbBarViewTable.BorderLight = factory.GetColor("90FFFFFF");
            crumbBarViewTable = new CrumbBarItemViewStateColorTable();
            table.CrumbBarItemView.Pressed = crumbBarViewTable;
            crumbBarViewTable.Foreground = factory.GetColor(0x3B3B3B);
            crumbBarViewTable.Background = new BackgroundColorBlendCollection();
            crumbBarViewTable.Background.AddRange(new BackgroundColorBlend[]{
                new BackgroundColorBlend(factory.GetColor("FFC59B61"), 0f),
                new BackgroundColorBlend(factory.GetColor("FFEEB469"), .1f),
                new BackgroundColorBlend(factory.GetColor("FFFCA060"), .6f),
                new BackgroundColorBlend(factory.GetColor("FFFB8E3D"), .6f),
                new BackgroundColorBlend(factory.GetColor("FFFEBB60"), 1f)});
            crumbBarViewTable.Border = factory.GetColor("FF8B7654");
            crumbBarViewTable.BorderLight = factory.GetColor("408B7654");

            #endregion

            #region WarningBox
            table.WarningBox.BackColor = factory.GetColor(factory.GetColor(0xD3D3D3));
            table.WarningBox.WarningBorderColor = factory.GetColor(0x313131);
            table.WarningBox.WarningBackColor1 = factory.GetColor(0xfefefe);
            table.WarningBox.WarningBackColor2 = factory.GetColor(0xE5E9EE);
            #endregion

            #region CalendarView

            #region WeekDayViewColors

            table.CalendarView.WeekDayViewColors = new ColorDef[]
            {
                new ColorDef(factory.GetColor(0xB0B6BE)),           // DayViewBorder
                new ColorDef(factory.GetColor(0x363636)),           // DayHeaderForeground

                new ColorDef(factory.GetColor(0xC7CBD1)),             // DayHeaderBackground

                new ColorDef(factory.GetColor(0xB0B6BE)),           // DayHeaderBorder

                new ColorDef(factory.GetColor(0xFFFFFF)),           // DayWorkHoursBackground
                new ColorDef(factory.GetColor(0xC7CBD1)),           // DayAllDayEventBackground
                new ColorDef(factory.GetColor(0xE8EAEC)),           // DayOffWorkHoursBackground

                new ColorDef(factory.GetColor(0xC7CBD1)),           // DayHourBorder
                new ColorDef(factory.GetColor(0xE8EAEC)),           // DayHalfHourBorder

                new ColorDef(factory.GetColor(0x4C535C)),           // SelectionBackground

                new ColorDef(factory.GetColor(0x9199A4)),           // OwnerTabBorder

                new ColorDef(new Color[] {factory.GetColor(0xCFD2D8), factory.GetColor(0xB0B6BE)},    // OwnerTabBackground
                new float[] {0f, 1f}, 90f),

                new ColorDef(factory.GetColor(0x000000)),           // OwnerTabForeground
                new ColorDef(factory.GetColor(0xB0B6BE)),           // OwnerTabContentBackground
                new ColorDef(factory.GetColor(0x000000)),           // OwnerTabSelectedForeground
                new ColorDef(factory.GetColor(0xFFFFFF)),           // OwnerTabSelectionBackground

                new ColorDef(factory.GetColor(0xF5F5F5)),           // CondensedViewBackground

                new ColorDef(factory.GetColor(0xEB8900)),           // NowDayViewBorder
                new ColorDef(factory.GetColor(0x000000)),           // NowDayHeaderForeground - 0x15428B

                new ColorDef(new Color[] {factory.GetColor(0xFFED79), factory.GetColor(0xFFD86B), factory.GetColor(0xFFBB00), factory.GetColor(0xFFEA77)},
                new float[] {0f, .55f ,58f, 1f}, 90f),              // NowDayHeaderBackground
                
                new ColorDef(new Color[] {factory.GetColor(0xFFED79), factory.GetColor(0xFFD86B), factory.GetColor(0xFFBB00), factory.GetColor(0xFFEA77)},
                new float[] {0f, .55f ,58f, 1f}, 90f),              // TimeIndicator

                new ColorDef(factory.GetColor(0xEB8900)),           // TimeIndicatorBorder
            };

            #endregion

            #region HourRulerColors

            table.CalendarView.TimeRulerColors = new ColorDef[]
            {
                new ColorDef(factory.GetColor(0xFFFFFF)),           // TimeRulerBackground
                new ColorDef(factory.GetColor(0x363636)),           // TimeRulerForeground
                new ColorDef(factory.GetColor(0xB8B8B8)),           // TimeRulerBorder
                new ColorDef(factory.GetColor(0xB8B8B8)),           // TimeRulerTickBorder

                new ColorDef(new Color[] {factory.GetColor(0xFFED79), factory.GetColor(0xFFD86B), factory.GetColor(0xFFBB00), factory.GetColor(0xFFEA77)},
                new float[] {0f, .55f ,58f, 1f}, 90f),              // TimeRulerIndicator

                new ColorDef(factory.GetColor(0xEB8900)),           // TimeRulerIndicatorBorder
            };

            #endregion

            #region MonthViewColors

            table.CalendarView.MonthViewColors = new ColorDef[]
            {
              new ColorDef(factory.GetColor(0x9199A4)),           // DayOfWeekHeaderBorder

                new ColorDef(new Color[] {factory.GetColor(0xDCDFE2), factory.GetColor(0xD3D6DA), factory.GetColor(0xB4BAC1), factory.GetColor(0xCBCED4)},
                new float[] {0, .55f, .58f, 1}),                    // DayOfWeekHeaderBackground

                new ColorDef(factory.GetColor(0x616A76)),           // DayOfWeekHeaderForeground
                new ColorDef(factory.GetColor(0x9199A4)),           // SideBarBorder

                new ColorDef(new Color[] {factory.GetColor(0xDCDFE2), factory.GetColor(0xD2D5DA), factory.GetColor(0xB7BCC3), factory.GetColor(0xCACED4)},
                new float[] {0, .6f, .6f, 1}, 0),                   // SideBarBackground

                new ColorDef(factory.GetColor(0x000000)),           // SideBarForeground
                new ColorDef(factory.GetColor(0x9199A4)),           // DayHeaderBorder

                new ColorDef(factory.GetColor(0xC7CBD1)),           // DayHeaderBackground

                new ColorDef(factory.GetColor(0x000000)),           // DayHeaderForeground
                new ColorDef(factory.GetColor(0x9199A4)),           // DayContentBorder
                new ColorDef(factory.GetColor(0xE8EAEC)),           // DayContentSelectionBackground
                new ColorDef(factory.GetColor(0xFFFFFF)),           // DayContentActiveDayBackground
                new ColorDef(factory.GetColor(0xC7CBD1)),           // DayContentInactiveDayBackground

                new ColorDef(factory.GetColor(0x9199A4)),           // OwnerTabBorder

                new ColorDef(new Color[] {factory.GetColor(0xCFD2D8), factory.GetColor(0xB0B6BE)},    // OwnerTabBackground
                new float[] {0f, 1f}, 90f),

                new ColorDef(factory.GetColor(0x000000)),           // OwnerTabForeground
                new ColorDef(factory.GetColor(0xB0B6BE)),           // OwnerTabContentBackground
                new ColorDef(factory.GetColor(0x000000)),           // OwnerTabSelectedForeground
                new ColorDef(factory.GetColor(0xFFFFFF)),           // OwnerTabSelectionBackground

                new ColorDef(factory.GetColor(0xEB8900)),           // NowDayViewBorder
                new ColorDef(factory.GetColor(0x000000)),           // NowDayHeaderForeground - 0x15428B

                new ColorDef(new Color[] {factory.GetColor(0xFFED79), factory.GetColor(0xFFD86B), factory.GetColor(0xFFBB00), factory.GetColor(0xFFEA77)},
                new float[] {0f, .55f ,58f, 1f}, 90f),              // NowDayHeaderBackground
            };

            #endregion

            #region AppointmentColors

            table.CalendarView.AppointmentColors = new ColorDef[]
            {
                new ColorDef(factory.GetColor(0x4B71A2)),           // DefaultBorder

                new ColorDef(new Color[] {factory.GetColor(0xFDFEFF), factory.GetColor(0xC1D3EA)},
                             new float[] {0f, 1f}, 90f),            // DefaultBackground

                new ColorDef(factory.GetColor(0x28518E)),           // BlueBorder

                new ColorDef(new Color[] {factory.GetColor(0xB1C5EC), factory.GetColor(0x759DDA)}, 
                             new float[] {0f, 1f}, 90f),            // BlueBackground

                new ColorDef(factory.GetColor(0x2C6524)),           // GreenBorder

                new ColorDef(new Color[] {factory.GetColor(0xC2E8BC), factory.GetColor(0x84D17B)},
                             new float[] {0f, 1f}, 90f),            // GreenBackground

                new ColorDef(factory.GetColor(0x8B3E0A)),           // OrangeBorder

                new ColorDef(new Color[] {factory.GetColor(0xF9C7A0), factory.GetColor(0xF49758)},
                             new float[] {0f, 1f}, 90f),            // OrangeBackground

                new ColorDef(factory.GetColor(0x3E2771)),           // PurpleBorder

                new ColorDef(new Color[] {factory.GetColor(0xC5B5E6), factory.GetColor(0x957BD2)},
                             new float[] {0f, 1f}, 90f),            // PurpleBackground

                new ColorDef(factory.GetColor(0x86171C)),           // RedBorder

                new ColorDef(new Color[] {factory.GetColor(0xF1AAAC), factory.GetColor(0xE5676E)},
                             new float[] {0f, 1f}, 90f),            // RedBackground

                new ColorDef(factory.GetColor(0x7C7814)),           // YellowBorder

                new ColorDef(new Color[] {factory.GetColor(0xFFFCAA), factory.GetColor(0xFFF958)},
                             new float[] {0f, 1f}, 90f),            // YellowBackground

                new ColorDef(factory.GetColor(-1)),                 // BusyTimeMarker
                new ColorDef(factory.GetColor(0xFFFFFF)),           // FreeTimeMarker
                new ColorDef(factory.GetColor(0x800080))            // OutOfOfficeTimeMarker
            };

            #endregion

            #endregion

            #region SuperTab

            #region SuperTab

            table.SuperTab.Background = new SuperTabLinearGradientColorTable(
                factory.GetColor(0xE5E9ED), Color.Empty);

            table.SuperTab.InnerBorder = factory.GetColor(0xDFE3E8);
            table.SuperTab.OuterBorder = factory.GetColor(0x7F8388);

            table.SuperTab.ControlBoxDefault.Image = factory.GetColor(0xFF, 0x000000);

            table.SuperTab.ControlBoxMouseOver.Background = factory.GetColor(0xFFE7A2);
            table.SuperTab.ControlBoxMouseOver.Border = factory.GetColor(0xFFBD69);
            table.SuperTab.ControlBoxMouseOver.Image = factory.GetColor(0xFF, 0x000000);

            table.SuperTab.ControlBoxPressed.Background = factory.GetColor(0xFB8C3C);
            table.SuperTab.ControlBoxPressed.Border = factory.GetColor(0xFFBD69);
            table.SuperTab.ControlBoxPressed.Image = factory.GetColor(0xFF, 0x000000);

            table.SuperTab.InsertMarker = factory.GetColor(0xFF, 0x000080);

            #endregion

            #region SuperTabItem

            // Top Default

            table.SuperTabItem.Default.Normal.Background = new SuperTabLinearGradientColorTable(
                new Color[] { factory.GetColor(0xEFF1F3), factory.GetColor(0xE1E4E9), factory.GetColor(0xEDEFF2), factory.GetColor(0xEFF1F3) },
                new float[] { 0, .5f, .5f, 1 });

            table.SuperTabItem.Default.Normal.InnerBorder = factory.GetColor(0xDFE3E8);
            table.SuperTabItem.Default.Normal.OuterBorder = factory.GetColor(0x7F8388);
            table.SuperTabItem.Default.Normal.Text = factory.GetColor(0x4D5359);
            table.SuperTabItem.Default.Normal.CloseMarker = factory.GetColor(0x406F9F);

            // Top Selected

            table.SuperTabItem.Default.Selected.Background = new SuperTabLinearGradientColorTable(
                new Color[] { factory.GetColor(0xE2E6EB), factory.GetColor(0xF9FAFB), factory.GetColor(0xF9FAFB), factory.GetColor(0xFFFFFF) },
                new float[] { 0, .5f, .5f, 1 });

            table.SuperTabItem.Default.Selected.InnerBorder = factory.GetColor(0xDFE3E8);
            table.SuperTabItem.Default.Selected.OuterBorder = factory.GetColor(0x7F8388);
            table.SuperTabItem.Default.Selected.Text = factory.GetColor(0x4D5359);
            table.SuperTabItem.Default.Selected.CloseMarker = factory.GetColor(0x406F9F);

            // Top SelectedMouseOver

            table.SuperTabItem.Default.SelectedMouseOver.Background = new SuperTabLinearGradientColorTable(
                new Color[] { factory.GetColor(0xDFE3E8), factory.GetColor(0xF7F8F9), factory.GetColor(0xF7F8F9), factory.GetColor(0xFEFEFF) },
                new float[] { 0, .5f, .5f, 1 });

            table.SuperTabItem.Default.SelectedMouseOver.InnerBorder = factory.GetColor(0xF3F4F6);
            table.SuperTabItem.Default.SelectedMouseOver.OuterBorder = factory.GetColor(0x868B91);
            table.SuperTabItem.Default.SelectedMouseOver.Text = factory.GetColor(0x4D5359);
            table.SuperTabItem.Default.SelectedMouseOver.CloseMarker = factory.GetColor(0x406F9F);

            // Top MouseOver

            table.SuperTabItem.Default.MouseOver.Background = new SuperTabLinearGradientColorTable(
                new Color[] { factory.GetColor(0xDFE3E8), factory.GetColor(0xF7F8F9), factory.GetColor(0xF7F8F9), factory.GetColor(0xFEFEFF) },
                new float[] { 0, .5f, .5f, 1 });

            table.SuperTabItem.Default.MouseOver.InnerBorder = factory.GetColor(0xF3F4F6);
            table.SuperTabItem.Default.MouseOver.OuterBorder = factory.GetColor(0x868B91);
            table.SuperTabItem.Default.MouseOver.Text = factory.GetColor(0x4D5359);
            table.SuperTabItem.Default.MouseOver.CloseMarker = factory.GetColor(0x406F9F);

            // Left, Bottom, Right

            table.SuperTabItem.Left = table.SuperTabItem.Default;
            table.SuperTabItem.Bottom = table.SuperTabItem.Default;
            table.SuperTabItem.Right = table.SuperTabItem.Default;

            #endregion

            #region SuperTabPanel

            table.SuperTabPanel.Default.Background = new SuperTabLinearGradientColorTable(factory.GetColor(0xFFFFFF), Color.Empty);
            table.SuperTabPanel.Default.InnerBorder = factory.GetColor(0xDFE3E8);
            table.SuperTabPanel.Default.OuterBorder = factory.GetColor(0x4D5359);

            table.SuperTabPanel.Left = table.SuperTabPanel.Default;
            table.SuperTabPanel.Bottom = table.SuperTabPanel.Default;
            table.SuperTabPanel.Right = table.SuperTabPanel.Default;

            #endregion

            #endregion

            #region Backstage

            #region Backstage
            SuperTabStyleColorFactory.GetOffice2010BackstageBlackColorTable(table.Backstage, factory);
            #endregion

            #region BackstageItem
            SuperTabStyleColorFactory.GetOffice2010BackstageBlackItemColorTable(table.BackstageItem, factory);
            #endregion

            #region BackstagePanel
            SuperTabStyleColorFactory.GetOffice2010BackstageBlackPanelColorTable(table.BackstagePanel, factory);
            #endregion

            #endregion

            #region SwitchButton
            SwitchButtonColorTable sbt = new SwitchButtonColorTable();
            sbt.BorderColor = factory.GetColor(0x919191);
            sbt.OffBackColor = factory.GetColor(0xC9C9C9);
            sbt.OffTextColor = factory.GetColor(0x000000);
            sbt.OnBackColor = factory.GetColor(0x92D050);
            sbt.OnTextColor = factory.GetColor(0x000000);
            sbt.SwitchBackColor = factory.GetColor(0xA4A4A4);
            sbt.SwitchBorderColor = factory.GetColor(0x555555);
            sbt.TextColor = factory.GetColor(0x000000);
            table.SwitchButton = new SwitchButtonColors();
            table.SwitchButton.Default = sbt;
            table.SwitchButton.Disabled.BorderColor = table.CheckBoxItem.Disabled.CheckBorder;
            table.SwitchButton.Disabled.SwitchBorderColor = table.SwitchButton.Disabled.BorderColor;
            table.SwitchButton.Disabled.OffTextColor = table.CheckBoxItem.Disabled.Text;
            table.SwitchButton.Disabled.OnTextColor = table.SwitchButton.Disabled.OffTextColor;
            table.SwitchButton.Disabled.TextColor = table.SwitchButton.Disabled.OffTextColor;
            table.SwitchButton.Disabled.SwitchBackColor = table.CheckBoxItem.Disabled.CheckInnerBackground.Start;
            table.SwitchButton.Disabled.OffBackColor = table.CheckBoxItem.Disabled.CheckInnerBackground.Start;
            table.SwitchButton.Disabled.OnBackColor = table.SwitchButton.Disabled.OffBackColor;
            #endregion

            #region ElementStyle Classes
            table.StyleClasses.Clear();
            ElementStyle style = new ElementStyle();
            style.Class = ElementStyleClassKeys.RibbonGalleryContainerKey;
            style.BorderColor = factory.GetColor(0x848484);
            style.Border = eStyleBorderType.Solid;
            style.BorderWidth = 1;
            style.CornerDiameter = 2;
            style.CornerType = eCornerType.Rounded;
            style.BackColor = factory.GetColor(0xBBBBBB);
            table.StyleClasses.Add(style.Class, style);
            // FileMenuContainer
            style = GetFileMenuContainerStyle(table);
            table.StyleClasses.Add(style.Class, style);
            // Two Column File Menu Container
            style = GetTwoColumnMenuContainerStyle(table);
            table.StyleClasses.Add(style.Class, style);
            // Column one File Menu Container
            style = GetMenuColumnOneContainerStyle(table);
            table.StyleClasses.Add(style.Class, style);
            // Column two File Menu Container
            style = GetMenuColumnTwoContainerStyle(table);
            table.StyleClasses.Add(style.Class, style);
            // Bottom File Menu Container
            style = GetMenuBottomContainer(table);
            table.StyleClasses.Add(style.Class, style);
            // TextBox border
            style = Office2007ColorTableFactory.GetTextBoxStyle(factory.GetColor(0x313131));
            table.StyleClasses.Add(style.Class, style);
            // ItemPanel
            style = Office2007ColorTableFactory.GetItemPanelStyle(factory.GetColor(0x313131));
            table.StyleClasses.Add(style.Class, style);
            // DateTimeInput background
            style = Office2007ColorTableFactory.GetDateTimeInputBackgroundStyle(factory.GetColor(0x313131));
            table.StyleClasses.Add(style.Class, style);
            // Ribbon Client Panel
            style = Office2010BlueFactory.GetRibbonClientPanelStyle(factory, eOffice2010ColorScheme.Black);
            table.StyleClasses.Add(style.Class, style);
            // ListView Border
            style = Office2007ColorTableFactory.GetListViewBorderStyle(table.ListViewEx);
            table.StyleClasses.Add(style.Class, style);
            style = Office2007ColorTableFactory.GetStatusBarAltStyle(table.Bar);
            table.StyleClasses.Add(style.Class, style);
#if !NOTREE
            // Tree Border/Background
            style = Office2007ColorTableFactory.GetAdvTreeStyle(factory.GetColor(0x313131));
            table.StyleClasses.Add(style.Class, style);
            style = Office2007ColorTableFactory.GetAdvTreeColumnsHeaderStyle(table.DataGridView.ColumnHeaderNormalBackground.Start, table.DataGridView.ColumnHeaderNormalBackground.End, table.DataGridView.ColumnHeaderNormalBorder);
            table.StyleClasses.Add(style.Class, style);
            style = Office2007ColorTableFactory.GetAdvTreeNodesColumnsHeaderStyle(table.DataGridView.ColumnHeaderNormalBackground.Start, table.DataGridView.ColumnHeaderNormalBackground.End, table.DataGridView.ColumnHeaderNormalBorder);
            table.StyleClasses.Add(style.Class, style);
            style = Office2007ColorTableFactory.GetAdvTreeColumnStyle(factory.GetColor(0x313131));
            table.StyleClasses.Add(style.Class, style);
            // CrumbBar
            style = Office2007ColorTableFactory.GetCrumbBarBackgroundStyle(factory.GetColor(Color.White), factory.GetColor("FF567DB0"), factory.GetColor("FF2F578D"));
            table.StyleClasses.Add(style.Class, style);
#endif
            // DataGridView border
            style = Office2007ColorTableFactory.GetDataGridViewStyle();
            table.StyleClasses.Add(style.Class, style);
            // DataGridViewDateTime border
            style = Office2007ColorTableFactory.GetDataGridViewDateTimeStyle();
            table.StyleClasses.Add(style.Class, style);
            // DataGridViewNumeric border
            style = Office2007ColorTableFactory.GetDataGridViewNumericStyle();
            table.StyleClasses.Add(style.Class, style);
            // DataGridViewIpAddress border
            style = Office2007ColorTableFactory.GetDataGridViewIpAddressStyle();
            table.StyleClasses.Add(style.Class, style);
            #endregion

        }

        #region RibbonBar
        public static Office2007RibbonBarStateColorTable GetRibbonBar(ColorFactory factory)
        {
            Office2007RibbonBarStateColorTable rb = new Office2007RibbonBarStateColorTable();
            rb.TopBackgroundHeight = 0.8f;
            rb.OuterBorder = new LinearGradientColorTable(factory.GetColor(0xB8B8B8), factory.GetColor(0x525252));
            rb.InnerBorder = new LinearGradientColorTable(factory.GetColor(0xCBCBCB), factory.GetColor(0xAFAFAF));
            rb.TopBackground = new LinearGradientColorTable(factory.GetColor(0xC9C9C9), factory.GetColor(0xA3A3A3));
            rb.BottomBackground = null; // new LinearGradientColorTable(factory.GetColor(0xC1C1C1), factory.GetColor(0xA3A3A3));
            rb.TitleBackground = null;// new LinearGradientColorTable(factory.GetColor(0xC2D8F1), factory.GetColor(0xC0D8EF));
            rb.TitleText = factory.GetColor(0x242424);
            return rb;
        }

        public static Office2007RibbonBarStateColorTable GetRibbonBarMouseOver(ColorFactory factory)
        {
            return GetRibbonBar(factory);
        }

        public static Office2007RibbonBarStateColorTable GetRibbonBarExpanded(ColorFactory factory)
        {
            Office2007RibbonBarStateColorTable rb = new Office2007RibbonBarStateColorTable();
            rb.TopBackgroundHeight = 0.8f;
            rb.OuterBorder = new LinearGradientColorTable(factory.GetColor(0xB8B8B8), factory.GetColor(0x525252));
            rb.InnerBorder = new LinearGradientColorTable(factory.GetColor(0xCBCBCB), factory.GetColor(0xAFAFAF));
            rb.TopBackground = new LinearGradientColorTable(factory.GetColor(0xC9C9C9), factory.GetColor(0xA3A3A3));
            rb.BottomBackground = null; // new LinearGradientColorTable(factory.GetColor(0xC1C1C1), factory.GetColor(0xA3A3A3));
            rb.TitleBackground = null;// new LinearGradientColorTable(factory.GetColor(0xC2D8F1), factory.GetColor(0xC0D8EF));
            rb.TitleText = factory.GetColor(0x242424);
            return rb;
        }
        #endregion

        #region Buttons
        private static Office2007ButtonItemColorTable GetButtonItemBackstageDefault(ColorFactory factory)
        {
            Office2007ButtonItemColorTable ct = Office2010BlueFactory.GetButtonItemBackstageDefault(factory);
            ct.Default.Text = factory.GetColor(0xFFFFFF);
            ct.Checked.Text = factory.GetColor(0xFFFFFF);
            ct.Expanded.Text = factory.GetColor(0xFFFFFF);
            ct.MouseOver.Text = factory.GetColor(0xFFFFFF);
            ct.Pressed.Text = factory.GetColor(0xFFFFFF);
            return ct;
        }
        public static Office2007ButtonItemColorTable GetButtonItemBlueOrange(ColorFactory factory)
        {
            Office2007ButtonItemColorTable cb = new Office2007ButtonItemColorTable();
            cb.Default = new Office2007ButtonItemStateColorTable();
            cb.Default.Text = factory.GetColor(0x000000);

            // Button mouse over
            cb.MouseOver = new Office2007ButtonItemStateColorTable();
            cb.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(0xF7C840), factory.GetColor(0xF8D427));
            cb.MouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(0xFFFAE6), factory.GetColor(0xFFFFF0));
            cb.MouseOver.TopBackground = new LinearGradientColorTable(factory.GetColor(0xFFEEA8), factory.GetColor(0xFFE37C));
            cb.MouseOver.TopBackgroundHighlight = LinearGradientColorTable.Empty; //new LinearGradientColorTable(Color.FromArgb(192, Color.White), Color.Transparent);
            cb.MouseOver.BottomBackground = new LinearGradientColorTable(factory.GetColor(0xFFE37C), factory.GetColor(0xFFFCE0));
            cb.MouseOver.BottomBackgroundHighlight = LinearGradientColorTable.Empty;// new LinearGradientColorTable(factory.GetColor(0xFFFD9C), Color.Transparent);
            cb.MouseOver.SplitBorder = new LinearGradientColorTable(factory.GetColor(0xE8C04E), Color.Empty);
            cb.MouseOver.SplitBorderLight = new LinearGradientColorTable(factory.GetColor(0xFFFAE8), Color.Empty);
            cb.MouseOver.Text = factory.GetColor(0x464646);

            cb.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
            cb.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(factory.GetColor(0xF7C740));

            // Pressed
            cb.Pressed = new Office2007ButtonItemStateColorTable();
            cb.Pressed.OuterBorder = new LinearGradientColorTable(factory.GetColor(0xC2762B), factory.GetColor(0xC29E47));
            cb.Pressed.InnerBorder = new LinearGradientColorTable(factory.GetColor(0xF6C867));
            cb.Pressed.TopBackground = new LinearGradientColorTable(factory.GetColor(0xFBD678), factory.GetColor(0xFFE48A));
            cb.Pressed.TopBackgroundHighlight = LinearGradientColorTable.Empty;
            cb.Pressed.BottomBackground = new LinearGradientColorTable(factory.GetColor(0xFFE48A), factory.GetColor(0xFEE287));
            cb.Pressed.BottomBackgroundHighlight = LinearGradientColorTable.Empty; // new LinearGradientColorTable(factory.GetColor(0xEFF1BD), Color.Transparent);
            cb.Pressed.SplitBorder = new LinearGradientColorTable(factory.GetColor(0xE8C04E), Color.Empty);
            cb.Pressed.SplitBorderLight = new LinearGradientColorTable(factory.GetColor(0xFFFAE8), Color.Empty);
            cb.Pressed.Text = factory.GetColor(0x464646);

            // Checked
            cb.Checked = new Office2007ButtonItemStateColorTable();
            cb.Checked.OuterBorder = new LinearGradientColorTable(factory.GetColor(0xC28A30), factory.GetColor(0xC2A44D));
            cb.Checked.InnerBorder = new LinearGradientColorTable(factory.GetColor(0xFFE575), Color.Transparent);
            cb.Checked.TopBackground = new LinearGradientColorTable(factory.GetColor(0xFFE575), factory.GetColor(0xFFD86B));
            cb.Checked.TopBackgroundHighlight = LinearGradientColorTable.Empty;
            cb.Checked.BottomBackground = new LinearGradientColorTable(factory.GetColor(0xFFD86B), factory.GetColor(0xFFEC79));
            cb.Checked.BottomBackgroundHighlight = new LinearGradientColorTable(factory.GetColor(0xFFF480), Color.Transparent);
            cb.Checked.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
            cb.Checked.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
            cb.Checked.Text = factory.GetColor(0x464646);

            // Expanded button
            cb.Expanded = new Office2007ButtonItemStateColorTable();
            cb.Expanded.OuterBorder = new LinearGradientColorTable(factory.GetColor(0xC28A30), factory.GetColor(0xC2A44D));
            cb.Expanded.InnerBorder = new LinearGradientColorTable(factory.GetColor(0xFFE575), Color.Transparent);
            cb.Expanded.TopBackground = new LinearGradientColorTable(factory.GetColor(0xFFE575), factory.GetColor(0xFFD86B));
            cb.Expanded.TopBackgroundHighlight = LinearGradientColorTable.Empty;
            cb.Expanded.BottomBackground = new LinearGradientColorTable(factory.GetColor(0xFFD86B), factory.GetColor(0xFFED7A));
            cb.Expanded.BottomBackgroundHighlight = new LinearGradientColorTable(factory.GetColor(0xFFF480), Color.Transparent);
            cb.Expanded.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
            cb.Expanded.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
            cb.Expanded.Text = factory.GetColor(0x464646);

            SetBlueExpandColors(cb, factory);

            return cb;
        }

        public static Office2007ButtonItemColorTable GetButtonItemBlueOrangeWithBackground(ColorFactory factory)
        {
            Office2007ButtonItemColorTable cb = GetButtonItemBlueOrange(factory);

            cb.Default.TopBackground = new LinearGradientColorTable(factory.GetColor(0xE2E2E2), factory.GetColor(0xD9D9D9));
            cb.Default.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
            cb.Default.BottomBackground = new LinearGradientColorTable(factory.GetColor(0xD9D9D9), factory.GetColor(0xC4C4C4));
            cb.Default.BottomBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
            cb.Default.InnerBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
            cb.Default.OuterBorder = new LinearGradientColorTable(factory.GetColor(0x919191));
            cb.Default.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
            cb.Default.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
            cb.MouseOverSplitInactive = cb.Default;

            // Same as default
            cb.Disabled = new Office2007ButtonItemStateColorTable();
            cb.Disabled.TopBackground = new LinearGradientColorTable(factory.GetColor(0xF5F5F5), factory.GetColor(0xECECEC));
            cb.Disabled.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
            cb.Disabled.BottomBackground = new LinearGradientColorTable(factory.GetColor(0xE5E5E5), factory.GetColor(0xE7E7E7));
            cb.Disabled.BottomBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
            cb.Disabled.InnerBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
            cb.Disabled.OuterBorder = new LinearGradientColorTable(factory.GetColor(0xBCBCBC), Color.Empty);
            cb.Disabled.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
            cb.Disabled.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
            cb.Disabled.Text = factory.GetColor(0x8D8D8D);
            return cb;
        }

        public static void SetBlueExpandColors(Office2007ButtonItemColorTable ct, ColorFactory factory)
        {
            Color cb = factory.GetColor(0x5E563D);
            Color cl = Color.Empty; // Color.FromArgb(192, Color.White);
            ct.Default.ExpandBackground = cb;
            ct.Default.ExpandLight = cl;

            ct.Checked.ExpandBackground = cb;
            ct.Checked.ExpandLight = cl;

            //ct.Disabled.ExpandBackground = factory.GetColor(0xB7B7B7);
            //ct.Disabled.ExpandLight = factory.GetColor(0xEDEDED);

            ct.Expanded.ExpandBackground = cb;
            ct.Expanded.ExpandLight = cl;

            ct.MouseOver.ExpandBackground = cb;
            ct.MouseOver.ExpandLight = cl;

            ct.Pressed.ExpandBackground = cb;
            ct.Pressed.ExpandLight = cl;
        }
        #endregion

        #region RibbonTabItem
        public static Office2007RibbonTabItemColorTable GetRibbonTabItemDefault(ColorFactory factory)
        {
            Office2007RibbonTabItemColorTable rt = new Office2007RibbonTabItemColorTable();
            rt.Default.Text = factory.GetColor(0xFFFFFF);
            rt.CornerSize = 2;

            // Selected Tab
            rt.Selected = new Office2007RibbonTabItemStateColorTable();
            rt.Selected.Background = new LinearGradientColorTable(factory.GetColor(0xCFCFCF), factory.GetColor(0xC9C9C9));
            rt.Selected.BackgroundHighlight = LinearGradientColorTable.Empty; // new LinearGradientColorTable(Color.FromArgb(32, Color.White), Color.Transparent);
            rt.Selected.InnerBorder = LinearGradientColorTable.Empty; // new LinearGradientColorTable(factory.GetColor(0xFFFFFF));
            rt.Selected.OuterBorder = new LinearGradientColorTable(factory.GetColor(0x5E5E5E));
            rt.Selected.Text = factory.GetColor(0x000000);

            // Selected Tab Mouse Over
            rt.SelectedMouseOver = new Office2007RibbonTabItemStateColorTable();
            rt.SelectedMouseOver.Background = new LinearGradientColorTable(factory.GetColor(0xCFCFCF), factory.GetColor(0xC9C9C9));
            rt.SelectedMouseOver.BackgroundHighlight = LinearGradientColorTable.Empty; //new LinearGradientColorTable(Color.FromArgb(32, Color.White), Color.Transparent);
            rt.SelectedMouseOver.InnerBorder = LinearGradientColorTable.Empty; // new LinearGradientColorTable(factory.GetColor(0xFFFFFF));
            rt.SelectedMouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(0x5E5E5E));
            rt.SelectedMouseOver.Text = factory.GetColor(0x000000);

            // Tab Mouse Over
            rt.MouseOver = new Office2007RibbonTabItemStateColorTable();
            rt.MouseOver.Background = new LinearGradientColorTable(factory.GetColor(0x8B8C8E), factory.GetColor(0x575757));
            rt.MouseOver.BackgroundHighlight = LinearGradientColorTable.Empty;
            rt.MouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(0xB2B2B2), factory.GetColor(0xA4A4A4));
            rt.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(0x5E5E5E));
            rt.MouseOver.Text = factory.GetColor(0xFFFFFF);

            return rt;
        }

        public static Office2007RibbonTabItemColorTable GetRibbonTabItemMagenta(ColorFactory factory)
        {
            Office2007RibbonTabItemColorTable rt = new Office2007RibbonTabItemColorTable();
            rt.Default.Text = factory.GetColor(0xFFFFFF);
            rt.CornerSize = 2;

            // Selected Tab
            rt.Selected = new Office2007RibbonTabItemStateColorTable();
            rt.Selected.Background = new LinearGradientColorTable(factory.GetColor(0xB9A2B1), factory.GetColor(0xC9C9C9));
            rt.Selected.BackgroundHighlight = LinearGradientColorTable.Empty; // new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
            rt.Selected.InnerBorder = new LinearGradientColorTable(factory.GetColor(0xC8C7C8), Color.Transparent);
            rt.Selected.OuterBorder = new LinearGradientColorTable(factory.GetColor(0xB06195), factory.GetColor(0x9C2F77));
            rt.Selected.Text = factory.GetColor(0x000000);

            // Selected Tab Mouse Over
            rt.SelectedMouseOver = new Office2007RibbonTabItemStateColorTable();
            rt.SelectedMouseOver.Background = new LinearGradientColorTable(factory.GetColor(0xB9A2B1), factory.GetColor(0xC9C9C9));
            rt.SelectedMouseOver.BackgroundHighlight = LinearGradientColorTable.Empty; // new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
            rt.SelectedMouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(0xC8C7C8), Color.Transparent);
            rt.SelectedMouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(0xB06195), factory.GetColor(0x9C2F77));
            rt.SelectedMouseOver.Text = factory.GetColor(0x000000);

            // Tab Mouse Over
            rt.MouseOver = new Office2007RibbonTabItemStateColorTable();
            rt.MouseOver.Background = new LinearGradientColorTable(factory.GetColor(0xA68098), factory.GetColor(0x595457));
            rt.MouseOver.BackgroundHighlight = LinearGradientColorTable.Empty; // new LinearGradientColorTable(factory.GetColor(0xFCFBFB), Color.Transparent);
            rt.MouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(0xB58CA5), factory.GetColor(0x8C8088));
            rt.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(0x9C2F77));
            rt.MouseOver.Text = factory.GetColor(0xFFFFFF);

            return rt;
        }

        public static Office2007RibbonTabItemColorTable GetRibbonTabItemBlueGreen(ColorFactory factory)
        {
            Office2007RibbonTabItemColorTable rt = new Office2007RibbonTabItemColorTable();
            rt.Default.Text = factory.GetColor(0xFFFFFF);
            rt.CornerSize = 2;

            // Selected Tab
            rt.Selected = new Office2007RibbonTabItemStateColorTable();
            rt.Selected.Background = new LinearGradientColorTable(factory.GetColor(0xB0C0AC), factory.GetColor(0xC9C9C9));
            rt.Selected.BackgroundHighlight = LinearGradientColorTable.Empty;// new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
            rt.Selected.InnerBorder = new LinearGradientColorTable(factory.GetColor(0xC8C7C8), Color.Transparent);
            rt.Selected.OuterBorder = new LinearGradientColorTable(factory.GetColor(0x589D53), factory.GetColor(0x117311));
            rt.Selected.Text = factory.GetColor(0x000000);

            // Selected Tab Mouse Over
            rt.SelectedMouseOver = new Office2007RibbonTabItemStateColorTable();
            rt.SelectedMouseOver.Background = new LinearGradientColorTable(factory.GetColor(0xB0C0AC), factory.GetColor(0xC9C9C9));
            rt.SelectedMouseOver.BackgroundHighlight = LinearGradientColorTable.Empty;// new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
            rt.SelectedMouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(0xC8C7C8), Color.Transparent);
            rt.SelectedMouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(0x589D53), factory.GetColor(0x117311));
            rt.SelectedMouseOver.Text = factory.GetColor(0x000000);

            // Tab Mouse Over
            rt.MouseOver = new Office2007RibbonTabItemStateColorTable();
            rt.MouseOver.Background = new LinearGradientColorTable(factory.GetColor(0x84A47C), factory.GetColor(0x545A53));
            rt.MouseOver.BackgroundHighlight = LinearGradientColorTable.Empty; // new LinearGradientColorTable(factory.GetColor(0xDEECFF), Color.Transparent);
            rt.MouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(0x8DB482), factory.GetColor(0x7E9475));
            rt.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(0x007E00), factory.GetColor(0x007E00));
            rt.MouseOver.Text = factory.GetColor(0xFFFFFF);

            return rt;
        }

        public static Office2007RibbonTabItemColorTable GetRibbonTabItemBlueOrange(ColorFactory factory)
        {
            Office2007RibbonTabItemColorTable rt = new Office2007RibbonTabItemColorTable();
            rt.Default.Text = factory.GetColor(0xFFFFFF);
            rt.CornerSize = 2;

            // Selected Tab
            rt.Selected = new Office2007RibbonTabItemStateColorTable();
            rt.Selected.Background = new LinearGradientColorTable(factory.GetColor(0xBAABA4), factory.GetColor(0xC9C9C9));
            rt.Selected.BackgroundHighlight = LinearGradientColorTable.Empty; // new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
            rt.Selected.InnerBorder = new LinearGradientColorTable(factory.GetColor(0xC8C7C8), Color.Transparent);
            rt.Selected.OuterBorder = new LinearGradientColorTable(factory.GetColor(0xA0662B), factory.GetColor(0xA85600));
            rt.Selected.Text = factory.GetColor(0x000000);

            // Selected Tab Mouse Over
            rt.SelectedMouseOver = new Office2007RibbonTabItemStateColorTable();
            rt.SelectedMouseOver.Background = new LinearGradientColorTable(factory.GetColor(0xBAABA4), factory.GetColor(0xC9C9C9));
            rt.SelectedMouseOver.BackgroundHighlight = LinearGradientColorTable.Empty; // new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
            rt.SelectedMouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(0xC8C7C8), Color.Transparent);
            rt.SelectedMouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(0xA0662B), factory.GetColor(0xA85600));
            rt.SelectedMouseOver.Text = factory.GetColor(0x000000);

            // Tab Mouse Over
            rt.MouseOver = new Office2007RibbonTabItemStateColorTable();
            rt.MouseOver.Background = new LinearGradientColorTable(factory.GetColor(0xAE976B), factory.GetColor(0x5D594E));
            rt.MouseOver.BackgroundHighlight = LinearGradientColorTable.Empty; // new LinearGradientColorTable(factory.GetColor(0xCBE2FF), Color.Transparent);
            rt.MouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(0xC1AB62), factory.GetColor(0x9E955A));
            rt.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(0xA85600));
            rt.MouseOver.Text = factory.GetColor(0xFFFFFF);

            return rt;
        }
        #endregion

        #region Style Class Creation
        public static ElementStyle GetFileMenuContainerStyle(Office2007ColorTable table)
        {
            ElementStyle style = new ElementStyle();
            style.Class = ElementStyleClassKeys.RibbonFileMenuContainerKey;
            Rendering.Office2007MenuColorTable mc = table.Menu;

            style.PaddingBottom = 3;
            style.PaddingLeft = 0;
            style.PaddingRight = 0;
            style.PaddingTop = 22;
            style.Border = eStyleBorderType.Solid;
            style.CornerType = eCornerType.Rounded;
            style.BorderWidth = 1;
            style.BorderColor = Color.Transparent;
            style.CornerDiameter = 3;
            BackgroundColorBlend[] blend = new BackgroundColorBlend[mc.FileBackgroundBlend.Count];
            mc.FileBackgroundBlend.CopyTo(blend);
            style.BackColorBlend.Clear();
            style.BackColorBlend.AddRange(blend);
            style.BackColorGradientAngle = 90;
            return style;
        }

        public static ElementStyle GetTwoColumnMenuContainerStyle(Office2007ColorTable table)
        {
            ElementStyle style = new ElementStyle();
            style.Class = ElementStyleClassKeys.RibbonFileMenuTwoColumnContainerKey;
            Rendering.Office2007MenuColorTable mc = table.Menu;

            style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Double;
            style.BorderBottomWidth = 0;
            style.BorderColor = mc.FileContainerBorder;
            //style.BorderColorLight = mc.FileContainerBorderLight;
            style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.None;
            style.BorderLeftWidth = 0;
            style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.None;
            style.BorderRightWidth = 0;
            style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            style.BorderTopWidth = 1;
            style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            style.BorderBottomWidth = 1;
            style.PaddingBottom = 0;
            style.PaddingLeft = 0;
            style.PaddingRight = 0;
            style.PaddingTop = 0;

            return style;
        }

        public static ElementStyle GetMenuColumnOneContainerStyle(Office2007ColorTable table)
        {
            ElementStyle style = new ElementStyle();
            style.Class = ElementStyleClassKeys.RibbonFileMenuColumnOneContainerKey;
            Rendering.Office2007MenuColorTable mc = table.Menu;

            style.BackColor = mc.FileColumnOneBackground;
            style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            style.BorderRightColor = mc.FileColumnOneBorder;
            style.BorderRightWidth = 1;
            style.PaddingRight = 1;

            return style;
        }

        public static ElementStyle GetMenuColumnTwoContainerStyle(Office2007ColorTable table)
        {
            ElementStyle style = new ElementStyle();
            style.Class = ElementStyleClassKeys.RibbonFileMenuColumnTwoContainerKey;
            Rendering.Office2007MenuColorTable mc = table.Menu;

            style.BackColor = mc.FileColumnTwoBackground;

            return style;
        }

        public static ElementStyle GetMenuBottomContainer(Office2007ColorTable table)
        {
            ElementStyle style = new ElementStyle();
            style.Class = ElementStyleClassKeys.RibbonFileMenuBottomContainerKey;
            Rendering.Office2007MenuColorTable mc = table.Menu;

            //BackgroundColorBlend[] blend = new BackgroundColorBlend[mc.FileBottomContainerBackgroundBlend.Count];
            //mc.FileBottomContainerBackgroundBlend.CopyTo(blend);
            //style.BackColorBlend.Clear();
            //style.BackColorBlend.AddRange(blend);
            //style.BackColorGradientAngle = 90;
            style.MarginTop = 2;
            style.MarginRight = 2;
            return style;
        }
        #endregion

        #region Scroll Bar
        public static void InitializeScrollBarColorTable(Office2007ColorTable t, ColorFactory factory)
        {
            Office2007ScrollBarStateColorTable sct = t.ScrollBar.Default;
            sct.Background = new LinearGradientColorTable(factory.GetColor(0xEFEFEF), factory.GetColor(0xFCFCFC), 0);
            sct.Border = new LinearGradientColorTable(factory.GetColor(0xEBEDEF), Color.Empty);
            sct.ThumbSignBackground = new LinearGradientColorTable(factory.GetColor(0x6E7EA6), factory.GetColor(0x424B63), 90);
            sct.TrackBackground.Clear();
            sct.TrackBackground.AddRange(new BackgroundColorBlend[] { new BackgroundColorBlend(factory.GetColor(0xF2F3F3),0f), 
                new BackgroundColorBlend(factory.GetColor(0xF0F0F0),0.5f),
                new BackgroundColorBlend(factory.GetColor(0xEBEBEC),0.5f),
                new BackgroundColorBlend(factory.GetColor(0xE6E7E8),1f)});
            sct.TrackInnerBorder = LinearGradientColorTable.Empty;
            sct.TrackOuterBorder = new LinearGradientColorTable(factory.GetColor(0xB6BABF), Color.Empty);
            sct.TrackSignBackground = new LinearGradientColorTable(Color.FromArgb(180, factory.GetColor(0x8F8F8F)), Color.FromArgb(64, Color.White));

            // Mouse Over
            sct = t.ScrollBar.MouseOver;
            sct.Background = t.ScrollBar.Default.Background;
            sct.Border = t.ScrollBar.Default.Border;
            sct.ThumbBackground.Clear();
            sct.ThumbBackground.AddRange(new BackgroundColorBlend[] { new BackgroundColorBlend(factory.GetColor(0xFFFFFF),0f), 
                new BackgroundColorBlend(factory.GetColor(0xFCFDFD),0.8f),
                new BackgroundColorBlend(factory.GetColor(0xF2F3F4),0.8f),
                new BackgroundColorBlend(factory.GetColor(0xF2F3F4),1f)});
            sct.ThumbInnerBorder = LinearGradientColorTable.Empty;
            sct.ThumbOuterBorder = new LinearGradientColorTable(factory.GetColor(0xB6BABF));
            sct.ThumbSignBackground = new LinearGradientColorTable(factory.GetColor(0x6E7EA6), factory.GetColor(0x424B63), 90);
            sct.TrackBackground.Clear();
            sct.TrackBackground.AddRange(new BackgroundColorBlend[] { new BackgroundColorBlend(factory.GetColor(0xFFFFFF),0f), 
                new BackgroundColorBlend(factory.GetColor(0xFCFDFD),0.8f),
                new BackgroundColorBlend(factory.GetColor(0xF2F3F4),0.8f),
                new BackgroundColorBlend(factory.GetColor(0xF2F3F4),1f)});
            sct.TrackInnerBorder = LinearGradientColorTable.Empty;
            sct.TrackOuterBorder = new LinearGradientColorTable(factory.GetColor(0xAEB3B8), Color.Empty);
            sct.TrackSignBackground = new LinearGradientColorTable(factory.GetColor(0x8F8F8F));

            // Control Mouse Over
            sct = t.ScrollBar.MouseOverControl;
            sct.Background = t.ScrollBar.Default.Background;
            sct.Border = t.ScrollBar.Default.Border;
            sct.ThumbBackground.Clear();
            sct.ThumbBackground.AddRange(new BackgroundColorBlend[] { new BackgroundColorBlend(factory.GetColor(0xFFFFFF),0f), 
                new BackgroundColorBlend(factory.GetColor(0xFCFDFD),0.8f),
                new BackgroundColorBlend(factory.GetColor(0xF2F3F4),0.8f),
                new BackgroundColorBlend(factory.GetColor(0xF2F3F4),1f)});
            sct.ThumbInnerBorder = LinearGradientColorTable.Empty;
            sct.ThumbOuterBorder = new LinearGradientColorTable(factory.GetColor(0xB6BABF));
            sct.ThumbSignBackground = new LinearGradientColorTable(factory.GetColor(0x6E7EA6), factory.GetColor(0x424B63), 90);
            sct.TrackBackground.Clear();
            sct.TrackBackground.AddRange(new BackgroundColorBlend[] { new BackgroundColorBlend(factory.GetColor(0xFFFFFF),0f), 
                new BackgroundColorBlend(factory.GetColor(0xFCFDFD),0.8f),
                new BackgroundColorBlend(factory.GetColor(0xF2F3F4),0.8f),
                new BackgroundColorBlend(factory.GetColor(0xF2F3F4),1f)});
            sct.TrackInnerBorder = LinearGradientColorTable.Empty;
            sct.TrackOuterBorder = new LinearGradientColorTable(factory.GetColor(0xAEB3B8), Color.Empty);
            sct.TrackSignBackground = new LinearGradientColorTable(factory.GetColor(0x8F8F8F));

            // Pressed
            sct = t.ScrollBar.Pressed;
            sct.Background = t.ScrollBar.Default.Background;
            sct.Border = t.ScrollBar.Default.Border;
            sct.ThumbBackground.Clear();
            sct.ThumbBackground.AddRange(new BackgroundColorBlend[] { 
                new BackgroundColorBlend(factory.GetColor(0xE1E2E3),0f), 
                new BackgroundColorBlend(factory.GetColor(0xE1E2E3),1f)
            });
            sct.ThumbInnerBorder = LinearGradientColorTable.Empty;
            sct.ThumbOuterBorder = new LinearGradientColorTable(factory.GetColor(0xAEB3B8));
            sct.ThumbSignBackground = new LinearGradientColorTable(factory.GetColor(0x888889));
            sct.TrackBackground.Clear();
            sct.TrackBackground.AddRange(new BackgroundColorBlend[] {
                new BackgroundColorBlend(factory.GetColor(0xE1E2E3),0f), 
                new BackgroundColorBlend(factory.GetColor(0xE1E2E3),1f)
            });
            sct.TrackInnerBorder = LinearGradientColorTable.Empty;
            sct.TrackOuterBorder = new LinearGradientColorTable(factory.GetColor(0xAEB3B8), Color.Empty);
            sct.TrackSignBackground = new LinearGradientColorTable(factory.GetColor(0x8F8F8F));
            // Disabled
            sct = t.ScrollBar.Disabled;
            sct.Background = t.ScrollBar.Default.Background;
            sct.Border = t.ScrollBar.Default.Border;
            sct.ThumbBackground.Clear();
            sct.ThumbInnerBorder = new LinearGradientColorTable();
            sct.ThumbOuterBorder = new LinearGradientColorTable();
            sct.ThumbSignBackground = new LinearGradientColorTable(factory.GetColor(0xBFCFF7), factory.GetColor(0x727C94));
            sct.TrackBackground.Clear();
            sct.TrackInnerBorder = new LinearGradientColorTable();
            sct.TrackOuterBorder = new LinearGradientColorTable();
            sct.TrackSignBackground = new LinearGradientColorTable();
        }

        public static void InitializeAppScrollBarColorTable(Office2007ColorTable t, ColorFactory factory)
        {
            Office2007ScrollBarStateColorTable sct = t.AppScrollBar.Default;
            sct.Background = new LinearGradientColorTable(factory.GetColor(0x636363), factory.GetColor(0x686868), 0);
            sct.Border = new LinearGradientColorTable(factory.GetColor(0x4E4E4E));
            sct.ThumbSignBackground = new LinearGradientColorTable(factory.GetColor(0x343434), factory.GetColor(0x1F1F1F), 90);
            sct.TrackBackground.Clear();
            sct.TrackBackground.AddRange(new BackgroundColorBlend[] { 
                new BackgroundColorBlend(factory.GetColor(0x747474),0f), 
                new BackgroundColorBlend(factory.GetColor(0x606060),.8f),
                new BackgroundColorBlend(factory.GetColor(0x595959),1f)});
            sct.TrackInnerBorder = LinearGradientColorTable.Empty;
            sct.TrackOuterBorder = new LinearGradientColorTable(factory.GetColor(0x242424), Color.Empty);
            sct.TrackSignBackground = new LinearGradientColorTable(factory.GetColor(0x989898));

            // Mouse Over
            sct = t.AppScrollBar.MouseOver;
            sct.Background = t.AppScrollBar.Default.Background;
            sct.Border = t.AppScrollBar.Default.Border;
            sct.ThumbBackground.Clear();
            sct.ThumbBackground.AddRange(new BackgroundColorBlend[] { 
                new BackgroundColorBlend(factory.GetColor(0x7F7F7F),0f), 
                new BackgroundColorBlend(factory.GetColor(0x626262),1f)});
            sct.ThumbInnerBorder = new LinearGradientColorTable(factory.GetColor(0x7F7F7F), factory.GetColor(0x6E6E6E));
            sct.ThumbOuterBorder = new LinearGradientColorTable(factory.GetColor(0x242424), Color.Empty);
            sct.ThumbSignBackground = t.AppScrollBar.Default.ThumbSignBackground;
            sct.TrackBackground.Clear();
            sct.TrackBackground.CopyFrom(sct.ThumbBackground);
            sct.TrackInnerBorder = sct.ThumbInnerBorder;
            sct.TrackOuterBorder = sct.ThumbOuterBorder;
            sct.TrackSignBackground = new LinearGradientColorTable(factory.GetColor(0x989898));

            // Control Mouse Over
            sct = t.AppScrollBar.MouseOverControl;
            sct.Background = t.AppScrollBar.Default.Background;
            sct.Border = t.AppScrollBar.Default.Border;
            sct.ThumbBackground.Clear();
            sct.ThumbBackground.CopyFrom(t.AppScrollBar.Default.TrackBackground);
            sct.ThumbInnerBorder = t.AppScrollBar.Default.TrackInnerBorder;
            sct.ThumbOuterBorder = t.AppScrollBar.Default.TrackOuterBorder;
            sct.ThumbSignBackground = t.AppScrollBar.Default.ThumbSignBackground;
            sct.TrackBackground.Clear();
            sct.TrackBackground.CopyFrom(t.AppScrollBar.Default.TrackBackground);
            sct.TrackInnerBorder = t.AppScrollBar.Default.TrackInnerBorder;
            sct.TrackOuterBorder = t.AppScrollBar.Default.TrackOuterBorder;
            sct.TrackSignBackground = t.AppScrollBar.Default.TrackSignBackground;

            // Pressed
            sct = t.AppScrollBar.Pressed;
            sct.Background = t.AppScrollBar.Default.Background;
            sct.Border = t.AppScrollBar.Default.Border;
            sct.ThumbBackground.Clear();
            sct.ThumbBackground.AddRange(new BackgroundColorBlend[] { 
                new BackgroundColorBlend(factory.GetColor(0x6B6B6B),0f), 
                new BackgroundColorBlend(factory.GetColor(0x4E4E4E),1f)});
            sct.ThumbInnerBorder = LinearGradientColorTable.Empty;
            sct.ThumbOuterBorder = new LinearGradientColorTable(factory.GetColor(0x242424), Color.Empty);
            sct.ThumbSignBackground = t.AppScrollBar.Default.ThumbSignBackground;
            sct.TrackBackground.Clear();
            sct.TrackBackground.CopyFrom(sct.ThumbBackground);
            sct.TrackInnerBorder = LinearGradientColorTable.Empty;
            sct.TrackOuterBorder = new LinearGradientColorTable(factory.GetColor(0x242424), Color.Empty);
            sct.TrackSignBackground = t.AppScrollBar.Default.TrackSignBackground;
            // Disabled
            sct = t.AppScrollBar.Disabled;
            sct.Background = t.AppScrollBar.Default.Background;
            sct.Border = t.AppScrollBar.Default.Border;
            sct.ThumbBackground.Clear();
            sct.ThumbInnerBorder = new LinearGradientColorTable();
            sct.ThumbOuterBorder = new LinearGradientColorTable();
            sct.ThumbSignBackground = new LinearGradientColorTable(factory.GetColor(0xBFCFF7), factory.GetColor(0x727C94));
            sct.TrackBackground.Clear();
            sct.TrackInnerBorder = new LinearGradientColorTable();
            sct.TrackOuterBorder = new LinearGradientColorTable();
            sct.TrackSignBackground = new LinearGradientColorTable();
        }
        #endregion
    }
}
