using System;
using System.Text;
using System.Drawing;

namespace DevComponents.DotNetBar.Rendering
{
    /// <summary>
    /// Represents the Windows 7 blue style color initialization class.
    /// </summary>
    public class Windows7BlueFactory
    {
        public static void InitializeBlueColorTable(Office2007ColorTable table, ColorFactory factory)
        {
            #region RibbonControl Start Images
            table.RibbonControl.StartButtonDefault = BarFunctions.LoadBitmap("SystemImages.BlankWin7NormalBlue.png");
            table.RibbonControl.StartButtonMouseOver = BarFunctions.LoadBitmap("SystemImages.BlankWin7HotBlue.png");
            table.RibbonControl.StartButtonPressed = BarFunctions.LoadBitmap("SystemImages.BlankWin7PressedBlue.png");
            #endregion

            #region RibbonControl
            table.RibbonControl.OuterBorder = new LinearGradientColorTable(factory.GetColor(0xBAC9DB), factory.GetColor(0x9FAEC2));
            table.RibbonControl.InnerBorder = new LinearGradientColorTable(factory.GetColor(0xFBFDFF), factory.GetColor(0xE5F0FB));
            table.RibbonControl.TabsBackground = new LinearGradientColorTable(factory.GetColor(0xDFE9F5), Color.Empty);
            table.RibbonControl.TabDividerBorder = factory.GetColor(0xA7BAD1);
            table.RibbonControl.TabDividerBorderLight = factory.GetColor(0xF4F8FD);
            table.RibbonControl.CornerSize = 1;
            table.RibbonControl.PanelTopBackgroundHeight = 0;
            table.RibbonControl.PanelTopBackground = new LinearGradientColorTable(factory.GetColor(0xF7FBFF), factory.GetColor(0xDCE7F5));
            table.RibbonControl.PanelBottomBackground = null;// new LinearGradientColorTable(factory.GetColor(0xC9D9ED), factory.GetColor(0xE7F2FF));
            #endregion

            #region Item Group
            table.ItemGroup.OuterBorder = new LinearGradientColorTable(factory.GetColor(0xAEBDD0), factory.GetColor(0xB0C0D5));
            table.ItemGroup.InnerBorder = LinearGradientColorTable.Empty;
            table.ItemGroup.TopBackground = LinearGradientColorTable.Empty;
            table.ItemGroup.BottomBackground = LinearGradientColorTable.Empty;
            table.ItemGroup.ItemGroupDividerDark = Color.Empty;
            table.ItemGroup.ItemGroupDividerLight = Color.Empty;
            #endregion

            #region RibbonBar
            table.RibbonBar.Default = GetRibbonBar(factory);
            table.RibbonBar.MouseOver = GetRibbonBar(factory); //GetRibbonBarMouseOver(factory);
            table.RibbonBar.Expanded = Office2007ColorTableFactory.GetRibbonBarBlueExpanded(factory);
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
            cb.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(factory.GetColor(0xADBDD2));
            cb.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Blue);
            table.ButtonItemColors.Add(cb);
            // Blue with background
            cb = Office2007ColorTableFactory.GetButtonItemBlueBlueWithBackground(factory);
            cb.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
            cb.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(factory.GetColor(0xADBDD2));
            cb.MouseOverSplitInactive = cb.Default;
            cb.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.BlueWithBackground);
            table.ButtonItemColors.Add(cb);
            // Magenta
            cb = Office2007ColorTableFactory.GetButtonItemBlueMagenta(factory);
            cb.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
            cb.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(factory.GetColor(0xADBDD2));
            cb.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Magenta);
            table.ButtonItemColors.Add(cb);
            // Blue with background
            cb = Office2007ColorTableFactory.GetButtonItemBlueMagentaWithBackground(factory);
            cb.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
            cb.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(factory.GetColor(0xADBDD2));
            cb.MouseOverSplitInactive = cb.Default;
            cb.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.MagentaWithBackground);
            table.ButtonItemColors.Add(cb);

            cb = Office2007ColorTableFactory.GetButtonItemOffice2007WithBackground(factory);
            cb.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
            cb.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(factory.GetColor(0xADBDD2));
            cb.MouseOverSplitInactive = cb.Default;
            cb.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Office2007WithBackground);
            table.ButtonItemColors.Add(cb);

            table.ButtonItemColors.Add(ButtonItemStaticColorTables.CreateBlueOrbColorTable(factory));
            #endregion

            #region RibbonTabItem Colors Initialization
            table.RibbonTabItemColors.Clear();
            Office2007RibbonTabItemColorTable rt = GetRibbonTabItemBlueDefault(factory);
            rt.Name = Enum.GetName(typeof(eRibbonTabColor), eRibbonTabColor.Default);
            table.RibbonTabItemColors.Add(rt);

            // Magenta
            rt = GetRibbonTabItemBlueMagenta(factory);
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
            Office2007RibbonTabGroupColorTable tg = Office2007ColorTableFactory.GetRibbonTabGroupBlueDefault(factory);
            tg.Name = Enum.GetName(typeof(eRibbonTabGroupColor), eRibbonTabGroupColor.Default);
            table.RibbonTabGroupColors.Add(tg);

            // Magenta
            tg = Office2007ColorTableFactory.GetRibbonTabGroupBlueMagenta(factory);
            tg.Name = Enum.GetName(typeof(eRibbonTabGroupColor), eRibbonTabGroupColor.Magenta);
            table.RibbonTabGroupColors.Add(tg);

            // Green
            tg = Office2007ColorTableFactory.GetRibbonTabGroupBlueGreen(factory);
            tg.Name = Enum.GetName(typeof(eRibbonTabGroupColor), eRibbonTabGroupColor.Green);
            table.RibbonTabGroupColors.Add(tg);

            // Orange
            tg = Office2007ColorTableFactory.GetRibbonTabGroupBlueOrange(factory);
            tg.Name = Enum.GetName(typeof(eRibbonTabGroupColor), eRibbonTabGroupColor.Orange);
            table.RibbonTabGroupColors.Add(tg);
            #endregion

            #region Initialize Bar
            table.Bar.ToolbarTopBackground = new LinearGradientColorTable(factory.GetColor(0xFCFEFF), factory.GetColor(0xE8F1FB));
            table.Bar.ToolbarBottomBackground = new LinearGradientColorTable(factory.GetColor(0xDCE6F4), factory.GetColor(0xDDE9F7));
            table.Bar.ToolbarBottomBorder = factory.GetColor(0xA0AFC3);
            table.Bar.PopupToolbarBackground = new LinearGradientColorTable(factory.GetColor(0xFCFCFC), Color.Empty);
            table.Bar.PopupToolbarBorder = factory.GetColor(0x8492A6);
            table.Bar.StatusBarTopBorder = factory.GetColor(0xA0AFC3);
            table.Bar.StatusBarTopBorderLight = factory.GetColor(Color.FromArgb(148, Color.White));
            table.Bar.StatusBarAltBackground.Clear();
            table.Bar.StatusBarAltBackground.Add(new BackgroundColorBlend(factory.GetColor(0xF7FBFF), 0f));
            table.Bar.StatusBarAltBackground.Add(new BackgroundColorBlend(factory.GetColor(0xDCE7F5), 1f));
            #endregion

            #region Menu
            table.Menu.Background = new LinearGradientColorTable(factory.GetColor(0xFCFCFC), Color.Empty);
            table.Menu.Border = new LinearGradientColorTable(factory.GetColor(0x8492A6), Color.Empty);
            table.Menu.Side = new LinearGradientColorTable(factory.GetColor(0xF3F7FB), Color.Empty);
            table.Menu.SideBorder = new LinearGradientColorTable(factory.GetColor(0xCFDBEB), Color.Empty);
            table.Menu.SideBorderLight = new LinearGradientColorTable(factory.GetColor(0xFFFFFF), Color.Empty);
            table.Menu.SideUnused = new LinearGradientColorTable(factory.GetColor(0xE5E5E5), Color.Empty);
            table.Menu.FileBackgroundBlend.Clear();
            table.Menu.FileBackgroundBlend.AddRange(new DevComponents.DotNetBar.BackgroundColorBlend[] {
                new DevComponents.DotNetBar.BackgroundColorBlend(factory.GetColor(0xFCFEFF), 0F),
                new DevComponents.DotNetBar.BackgroundColorBlend(factory.GetColor(0xE8F1FB), 10F),
                new DevComponents.DotNetBar.BackgroundColorBlend(factory.GetColor(0xDCE6F4), 10F),
                new DevComponents.DotNetBar.BackgroundColorBlend(factory.GetColor(0xDDE9F7), 1F)});
            table.Menu.FileContainerBorder = factory.GetColor(0xABB8C9);
            table.Menu.FileContainerBorderLight = Color.Transparent;
            table.Menu.FileColumnOneBackground = factory.GetColor(0xFCFCFC);
            table.Menu.FileColumnOneBorder = factory.GetColor(0xCFDBEB);
            table.Menu.FileColumnTwoBackground = factory.GetColor(0xEFF5FA);
            table.Menu.FileBottomContainerBackgroundBlend.Clear();
            table.Menu.FileBottomContainerBackgroundBlend.AddRange(new DevComponents.DotNetBar.BackgroundColorBlend[] {
                new DevComponents.DotNetBar.BackgroundColorBlend(factory.GetColor(0xFAFDFF), 0F),
                new DevComponents.DotNetBar.BackgroundColorBlend(factory.GetColor(0xE2EAF9), 1F)});
            #endregion

            #region ComboBox
            table.ComboBox.Default.Background = factory.GetColor(0xF3F7FC);
            table.ComboBox.Default.Border = factory.GetColor(0xAABBD2);
            table.ComboBox.Default.ExpandBackground = new LinearGradientColorTable();
            table.ComboBox.Default.ExpandBorderInner = new LinearGradientColorTable();
            table.ComboBox.Default.ExpandBorderOuter = new LinearGradientColorTable();
            table.ComboBox.Default.ExpandText = factory.GetColor(0x4C607A);
            table.ComboBox.DefaultStandalone.Background = factory.GetColor(0xFFFFFF);
            table.ComboBox.DefaultStandalone.Border = factory.GetColor(0x9FAEC2);
            table.ComboBox.DefaultStandalone.ExpandBackground = new LinearGradientColorTable(factory.GetColor(0xE9F0F7), factory.GetColor(0xD7E1EF), 90);
            table.ComboBox.DefaultStandalone.ExpandBorderInner = new LinearGradientColorTable(factory.GetColor(0xF4F6FA));
            table.ComboBox.DefaultStandalone.ExpandBorderOuter = new LinearGradientColorTable(factory.GetColor(0xB6C4D7), Color.Empty, 90);
            table.ComboBox.DefaultStandalone.ExpandText = factory.GetColor(0x4C607A);
            table.ComboBox.MouseOver.Background = factory.GetColor(0xFFFFFF);
            table.ComboBox.MouseOver.Border = factory.GetColor(0x9FAEC2);
            table.ComboBox.MouseOver.ExpandBackground = new LinearGradientColorTable(factory.GetColor(0xFDE8D1), factory.GetColor(0xFFD168), 90);
            table.ComboBox.MouseOver.ExpandBorderInner = new LinearGradientColorTable(factory.GetColor(0xFFFFFB), factory.GetColor(0xFFFCF3), 90);
            table.ComboBox.MouseOver.ExpandBorderOuter = new LinearGradientColorTable(factory.GetColor(0xFFBF13), Color.Empty, 90);
            table.ComboBox.MouseOver.ExpandText = factory.GetColor(0x4C607A);
            table.ComboBox.DroppedDown.Background = factory.GetColor(0xFFFFFF);
            table.ComboBox.DroppedDown.Border = factory.GetColor(0x9FAEC2);
            table.ComboBox.DroppedDown.ExpandBackground = new LinearGradientColorTable(factory.GetColor(0xE4C9A7), factory.GetColor(0xEFBC61), 90);
            table.ComboBox.DroppedDown.ExpandBorderInner = new LinearGradientColorTable(factory.GetColor(0xD4C0A9), factory.GetColor(0xEFF1B9), 90);
            table.ComboBox.DroppedDown.ExpandBorderOuter = new LinearGradientColorTable(factory.GetColor(0xC2923B), Color.Empty, 90);
            table.ComboBox.DroppedDown.ExpandText = factory.GetColor(0x4C607A);
            #endregion

            #region Dialog Launcher
            table.DialogLauncher.Default.DialogLauncher = factory.GetColor(0x6B737D);
            table.DialogLauncher.Default.DialogLauncherShade = factory.GetColor(172, 0xFFFFFF);

            table.DialogLauncher.MouseOver.DialogLauncher = factory.GetColor(0x6B737D);
            table.DialogLauncher.MouseOver.DialogLauncherShade = Color.FromArgb(192, factory.GetColor(0xFFFFFF));
            table.DialogLauncher.MouseOver.TopBackground = new LinearGradientColorTable(factory.GetColor(0xFDEEDE), factory.GetColor(0xFDE0BD));
            table.DialogLauncher.MouseOver.BottomBackground = new LinearGradientColorTable(factory.GetColor(0xFFD26F), factory.GetColor(0xFFF599));
            table.DialogLauncher.MouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(0xFEF9F3), factory.GetColor(0xFFFFF3));
            table.DialogLauncher.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(0xFFB700), Color.Empty);

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
            table.SystemButton.Default.Foreground = new LinearGradientColorTable(factory.GetColor(0x798DA7), factory.GetColor(0x8BA7CC));
            table.SystemButton.Default.LightShade = factory.GetColor(0xF8F9FA);
            table.SystemButton.Default.DarkShade = factory.GetColor(0x656565);

            // Mouse over state
            table.SystemButton.MouseOver = new Office2007SystemButtonStateColorTable();
            table.SystemButton.MouseOver.Foreground = new LinearGradientColorTable(factory.GetColor(0x798DA7), factory.GetColor(0x8BA7CC));
            table.SystemButton.MouseOver.LightShade = factory.GetColor(0xF8F9FA);
            table.SystemButton.MouseOver.DarkShade = factory.GetColor(0x656565);
            table.SystemButton.MouseOver.TopBackground = new LinearGradientColorTable(factory.GetColor(0xF5F8FE), factory.GetColor(0xEAF2FF));
            table.SystemButton.MouseOver.BottomBackground = new LinearGradientColorTable(factory.GetColor(0xD2E4FE), factory.GetColor(0xD1E2FB));
            table.SystemButton.MouseOver.TopHighlight = new LinearGradientColorTable(factory.GetColor(0xFBFCFF), Color.Transparent);
            table.SystemButton.MouseOver.BottomHighlight = new LinearGradientColorTable(factory.GetColor(0xFBFCFF), Color.Transparent);
            table.SystemButton.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(0xC1D4EE), factory.GetColor(0xAAC4E9));
            table.SystemButton.MouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(0xFEFEFF), factory.GetColor(0xE6F4FE));

            // Pressed
            table.SystemButton.Pressed = new Office2007SystemButtonStateColorTable();
            table.SystemButton.Pressed.Foreground = new LinearGradientColorTable(factory.GetColor(0x798DA7), factory.GetColor(0x8BA7CC));
            table.SystemButton.Pressed.LightShade = factory.GetColor(0xF8F9FA);
            table.SystemButton.Pressed.DarkShade = factory.GetColor(0x656565);
            table.SystemButton.Pressed.TopBackground = new LinearGradientColorTable(factory.GetColor(0xBAD5F8), factory.GetColor(0x9EC2ED));
            table.SystemButton.Pressed.TopHighlight = new LinearGradientColorTable(factory.GetColor(0xB8CEE9), Color.Transparent);
            table.SystemButton.Pressed.BottomBackground = new LinearGradientColorTable(factory.GetColor(0x84B2E9), factory.GetColor(0xAFD6F7));
            table.SystemButton.Pressed.BottomHighlight = new LinearGradientColorTable(factory.GetColor(0xC6EAFD), Color.Transparent);
            table.SystemButton.Pressed.OuterBorder = new LinearGradientColorTable(factory.GetColor(0xA1BFE5), factory.GetColor(0xB2CAE7));
            table.SystemButton.Pressed.InnerBorder = new LinearGradientColorTable(factory.GetColor(0xD3E6FF), factory.GetColor(0xDCE9FB));

            // Form border
            table.Form.Active.BorderColors = new Color[] {
                factory.GetColor(0x5A5859),
                factory.GetColor(0xD4E6F5),
                factory.GetColor(0xDFE9F5),
                factory.GetColor(0xDFE9F5),
                factory.GetColor(0xDFE9F5)
            };

            table.Form.Inactive.BorderColors = new Color[] {
                factory.GetColor(0xC0C6CE),
                factory.GetColor(0xCCD6E2),
                factory.GetColor(0xDFE9F5),
                factory.GetColor(0xDFE9F5),
                factory.GetColor(0xDFE9F5)};

            // Form Caption Active
            table.Form.Active.CaptionTopBackground = new LinearGradientColorTable(factory.GetColor(0xF7FBFF), factory.GetColor(0xDFE9F5));
            table.Form.Active.CaptionBottomBackground = new LinearGradientColorTable(factory.GetColor(0xDFE9F5), factory.GetColor(0xDFE9F5));
            table.Form.Active.CaptionBottomBorder = new Color[] { factory.GetColor(0x9FAEC2), factory.GetColor(0xCEDBEB) };
            table.Form.Active.CaptionText = factory.GetColor(0x1D221E);
            table.Form.Active.CaptionTextExtra = factory.GetColor(0x254264);

            // Form Caption Inactive
            table.Form.Inactive.CaptionTopBackground = new LinearGradientColorTable(factory.GetColor(0xE3E7EC), factory.GetColor(0xDEE5ED));
            table.Form.Inactive.CaptionBottomBackground = new LinearGradientColorTable(factory.GetColor(0xD8E1EC), factory.GetColor(0xE3E8EF));
            table.Form.Inactive.CaptionText = factory.GetColor(0xA0A0A0);
            table.Form.Inactive.CaptionTextExtra = factory.GetColor(0xA0A0A0);

            table.Form.BackColor = factory.GetColor(0xFCFCFC);
            table.Form.TextColor = factory.GetColor(0x000000);
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
            table.QuickAccessToolbar.Standalone.BottomBackground = new LinearGradientColorTable(factory.GetColor(0xDFE9F5));
            table.QuickAccessToolbar.Standalone.OutterBorderColor = factory.GetColor(0xB2BFD2);
            table.QuickAccessToolbar.Standalone.MiddleBorderColor = Color.Empty;
            table.QuickAccessToolbar.Standalone.InnerBorderColor = Color.Empty; // factory.GetColor(0xDCE8F7);

            table.QuickAccessToolbar.QatCustomizeMenuLabelBackground = factory.GetColor(0xEBF2F7);
            table.QuickAccessToolbar.QatCustomizeMenuLabelText = factory.GetColor(0x4C607A);

            table.QuickAccessToolbar.Active.GlassBorder = LinearGradientColorTable.Empty; // new LinearGradientColorTable(factory.GetColor(Color.FromArgb(132, Color.Black)), Color.FromArgb(80, Color.Black));
            table.QuickAccessToolbar.Inactive.GlassBorder = LinearGradientColorTable.Empty; // new LinearGradientColorTable(factory.GetColor(Color.FromArgb(132, Color.Black)), Color.FromArgb(80, Color.Black));
            #endregion

            #region Tab Colors
            table.TabControl.Default = new Office2007TabItemStateColorTable();
            table.TabControl.Default.TopBackground = new LinearGradientColorTable(factory.GetColor(0xD7E6F9), factory.GetColor(0xC7DCF8));
            table.TabControl.Default.BottomBackground = new LinearGradientColorTable(factory.GetColor(0xB3D0F5), factory.GetColor(0xD7E5F7));
            table.TabControl.Default.InnerBorder = factory.GetColor(0xF3F7FD);
            table.TabControl.Default.OuterBorder = factory.GetColor(0x92A5C7);
            table.TabControl.Default.Text = factory.GetColor(0x4C607A);

            table.TabControl.MouseOver = new Office2007TabItemStateColorTable();
            table.TabControl.MouseOver.TopBackground = new LinearGradientColorTable(factory.GetColor(0xFFFDEB), factory.GetColor(0xFFECA8));
            table.TabControl.MouseOver.BottomBackground = new LinearGradientColorTable(factory.GetColor(0xFFDA59), factory.GetColor(0xFFE68D));
            table.TabControl.MouseOver.InnerBorder = factory.GetColor(0xFFFFFB);
            table.TabControl.MouseOver.OuterBorder = factory.GetColor(0xB69D73);
            table.TabControl.MouseOver.Text = factory.GetColor(0x4C607A);

            table.TabControl.Selected = new Office2007TabItemStateColorTable();
            table.TabControl.Selected.TopBackground = new LinearGradientColorTable(factory.GetColor(Color.White), factory.GetColor(0xFDFDFE));
            table.TabControl.Selected.BottomBackground = new LinearGradientColorTable(factory.GetColor(0xFDFDFE), factory.GetColor(0xFDFDFE));
            table.TabControl.Selected.InnerBorder = factory.GetColor(Color.White);
            table.TabControl.Selected.OuterBorder = factory.GetColor(0x92A5C7);
            table.TabControl.Selected.Text = factory.GetColor(0x4C607A);

            table.TabControl.TabBackground = new LinearGradientColorTable(factory.GetColor(0xDFE9F5), Color.Empty);
            table.TabControl.TabPanelBackground = new LinearGradientColorTable(factory.GetColor(0xF7FBFF), Color.Empty);
            table.TabControl.TabPanelBorder = factory.GetColor(0x92A5C7);
            #endregion

            #region CheckBoxItem
            Office2007CheckBoxColorTable chk = table.CheckBoxItem;
            chk.Default.CheckBackground = new LinearGradientColorTable(factory.GetColor(0xF4F4F4), Color.Empty);
            chk.Default.CheckBorder = factory.GetColor(0xABC1DE);
            chk.Default.CheckInnerBackground = new LinearGradientColorTable(Color.FromArgb(192, factory.GetColor(0xA2ACB9)), Color.FromArgb(164, factory.GetColor(0xF6F6F6)));
            chk.Default.CheckInnerBorder = factory.GetColor(0xA2ACB9);
            chk.Default.CheckSign = new LinearGradientColorTable(factory.GetColor(0x4A6B96), Color.Empty);
            chk.Default.Text = factory.GetColor(0x254264);

            chk.MouseOver.CheckBackground = new LinearGradientColorTable(factory.GetColor(0xDEEAFA), Color.Empty);
            chk.MouseOver.CheckBorder = factory.GetColor(0x5577A3);
            chk.MouseOver.CheckInnerBackground = new LinearGradientColorTable(Color.FromArgb(192, factory.GetColor(0xFAD57A)), Color.FromArgb(128, factory.GetColor(0xFEF8E7)));
            chk.MouseOver.CheckInnerBorder = factory.GetColor(0xFAD57A);
            chk.MouseOver.CheckSign = new LinearGradientColorTable(factory.GetColor(0x4A6B96), Color.Empty);
            chk.MouseOver.Text = factory.GetColor(0x254264);

            chk.Pressed.CheckBackground = new LinearGradientColorTable(factory.GetColor(0xC1D8F5), Color.Empty);
            chk.Pressed.CheckBorder = factory.GetColor(0x5577A3);
            chk.Pressed.CheckInnerBackground = new LinearGradientColorTable(Color.FromArgb(192, factory.GetColor(0xF28926)), Color.FromArgb(164, factory.GetColor(0xFFF4D5)));
            chk.Pressed.CheckInnerBorder = factory.GetColor(0xF28926);
            chk.Pressed.CheckSign = new LinearGradientColorTable(factory.GetColor(0x4A6B96), Color.Empty);
            chk.Pressed.Text = factory.GetColor(0x254264);

            chk.Disabled.CheckBackground = new LinearGradientColorTable(factory.GetColor(0xFFFFFF), Color.Empty);
            chk.Disabled.CheckBorder = factory.GetColor(0xAEB1B5);
            chk.Disabled.CheckInnerBackground = new LinearGradientColorTable(Color.FromArgb(192, factory.GetColor(0xE0E2E5)), Color.FromArgb(164, factory.GetColor(0xFBFBFB)));
            chk.Disabled.CheckInnerBorder = factory.GetColor(0xE0E2E5);
            chk.Disabled.CheckSign = new LinearGradientColorTable(factory.GetColor(0x8D8D8D), Color.Empty);
            chk.Disabled.Text = factory.GetColor(0x8D8D8D);
            #endregion

            #region Scroll Bar Colors
            Office2007ColorTableFactory.InitializeScrollBarColorTable(table, factory);
            Office2007ColorTableFactory.InitializeAppBlueScrollBarColorTable(table, factory);
            #endregion

            #region ProgressBarItem
            Office2007ProgressBarColorTable pct = table.ProgressBarItem;
            pct.BackgroundColors = new GradientColorTable(0xC6CBD5, 0xE0E4ED);
            pct.OuterBorder = factory.GetColor(0xDEE2EC);
            pct.InnerBorder = factory.GetColor(0x7496C2);
            pct.Chunk = new GradientColorTable(0x69922A, 0xE7F2D4, 0);
            pct.ChunkOverlay = new GradientColorTable();
            pct.ChunkOverlay.LinearGradientAngle = 90;
            pct.ChunkOverlay.Colors.AddRange(new BackgroundColorBlend[] {
                new BackgroundColorBlend(Color.FromArgb(192, factory.GetColor(0xEEFFD7)), 0f),
                new BackgroundColorBlend(Color.FromArgb(128, factory.GetColor(0x8DB254)), .5f),
                new BackgroundColorBlend(Color.FromArgb(64, factory.GetColor(0x69922B)), .5f),
                new BackgroundColorBlend(Color.Transparent, 1f),
            });
            pct.ChunkShadow = new GradientColorTable(0xB2B9C8, 0xD5DAE5, 0);

            // Paused State
            pct = table.ProgressBarItemPaused;
            pct.BackgroundColors = new GradientColorTable(0xC6CBD5, 0xE0E4ED);
            pct.OuterBorder = factory.GetColor(0xDEE2EC);
            pct.InnerBorder = factory.GetColor(0x7496C2);
            pct.Chunk = new GradientColorTable(0xAEA700, 0xFFFDCD, 0);
            pct.ChunkOverlay = new GradientColorTable();
            pct.ChunkOverlay.LinearGradientAngle = 90;
            pct.ChunkOverlay.Colors.AddRange(new BackgroundColorBlend[] {
                new BackgroundColorBlend(Color.FromArgb(192, factory.GetColor(0xFFFBA3)), 0f),
                new BackgroundColorBlend(Color.FromArgb(128, factory.GetColor(0xD2CA00)), .5f),
                new BackgroundColorBlend(Color.FromArgb(64, factory.GetColor(0xFEF400)), .5f),
                new BackgroundColorBlend(Color.Transparent, 1f),
            });
            pct.ChunkShadow = new GradientColorTable(0xB2B9C8, 0xD5DAE5, 0);

            // Error State
            pct = table.ProgressBarItemError;
            pct.BackgroundColors = new GradientColorTable(0xC6CBD5, 0xE0E4ED);
            pct.OuterBorder = factory.GetColor(0xDEE2EC);
            pct.InnerBorder = factory.GetColor(0x7496C2);
            pct.Chunk = new GradientColorTable(0xD20000, 0xFFCDCD, 0);
            pct.ChunkOverlay = new GradientColorTable();
            pct.ChunkOverlay.LinearGradientAngle = 90;
            pct.ChunkOverlay.Colors.AddRange(new BackgroundColorBlend[] {
                new BackgroundColorBlend(Color.FromArgb(192, factory.GetColor(0xFF8F8F)), 0f),
                new BackgroundColorBlend(Color.FromArgb(128, factory.GetColor(0xD20000)), .5f),
                new BackgroundColorBlend(Color.FromArgb(64, factory.GetColor(0xFE0000)), .5f),
                new BackgroundColorBlend(Color.Transparent, 1f),
            });
            pct.ChunkShadow = new GradientColorTable(0xB2B9C8, 0xD5DAE5, 0);
            #endregion

            #region Gallery
            Office2007GalleryColorTable gallery = table.Gallery;
            gallery.GroupLabelBackground = factory.GetColor(0xDDE7EE);
            gallery.GroupLabelText = factory.GetColor(0x00156E);
            gallery.GroupLabelBorder = factory.GetColor(0xC5C5C5);
            #endregion

            #region Legacy Colors
            table.LegacyColors.BarBackground = factory.GetColor(0xF6FBFF);
            table.LegacyColors.BarBackground2 = factory.GetColor(0xDCE6F4);
            table.LegacyColors.BarStripeColor = factory.GetColor(0x9FAEC2);
            table.LegacyColors.BarCaptionBackground = factory.GetColor(0x8C97A7);
            table.LegacyColors.BarCaptionBackground2 = Color.Empty;
            table.LegacyColors.BarCaptionInactiveBackground = factory.GetColor(0xEFF6FD);
            table.LegacyColors.BarCaptionInactiveBackground2 = Color.Empty;
            table.LegacyColors.BarCaptionInactiveText = factory.GetColor(0x083772);
            table.LegacyColors.BarCaptionText = factory.GetColor(0xFFFFFF);
            table.LegacyColors.BarFloatingBorder = factory.GetColor(0x3764A0);
            table.LegacyColors.BarPopupBackground = factory.GetColor(0xF6F6F6);
            table.LegacyColors.BarPopupBorder = factory.GetColor(0x8492A6);
            table.LegacyColors.ItemBackground = Color.Empty;
            table.LegacyColors.ItemBackground2 = Color.Empty;
            table.LegacyColors.ItemCheckedBackground = factory.GetColor(0xFCD578);
            table.LegacyColors.ItemCheckedBackground2 = factory.GetColor(0xFBC84F);
            table.LegacyColors.ItemCheckedBorder = factory.GetColor(0xBB5503);
            table.LegacyColors.ItemCheckedText = factory.GetColor(0x254264);
            table.LegacyColors.ItemDisabledBackground = Color.Empty;
            table.LegacyColors.ItemDisabledText = factory.GetColor(0x8D8D8D);
            table.LegacyColors.ItemExpandedShadow = Color.Empty;
            table.LegacyColors.ItemExpandedBackground = factory.GetColor(0xE3EFFE);
            table.LegacyColors.ItemExpandedBackground2 = factory.GetColor(0x99BFF0);
            table.LegacyColors.ItemExpandedText = factory.GetColor(0x254264);
            table.LegacyColors.ItemHotBackground = factory.GetColor(0xFFF5CC);
            table.LegacyColors.ItemHotBackground2 = factory.GetColor(0xFFDB75);
            table.LegacyColors.ItemHotBorder = factory.GetColor(0xFFBD69);
            table.LegacyColors.ItemHotText = factory.GetColor(0x254264);
            table.LegacyColors.ItemPressedBackground = factory.GetColor(0xFC973D);
            table.LegacyColors.ItemPressedBackground2 = factory.GetColor(0xFFB85E);
            table.LegacyColors.ItemPressedBorder = factory.GetColor(0xFB8C3C);
            table.LegacyColors.ItemPressedText = factory.GetColor(0x254264);
            table.LegacyColors.ItemSeparator = factory.GetColor(160, 0x505050);
            table.LegacyColors.ItemSeparatorShade = Color.FromArgb(96, factory.GetColor(0xFFFFFF));
            table.LegacyColors.ItemText = factory.GetColor(0x254264); // SystemColors.ControlTet;
            table.LegacyColors.MenuBackground = factory.GetColor(0xF6F6F6);
            table.LegacyColors.MenuBackground2 = Color.Empty; // Color.White;
            table.LegacyColors.MenuBarBackground = factory.GetColor(0xBFDBFF);
            table.LegacyColors.MenuBorder = factory.GetColor(0x8492A6);
            table.LegacyColors.ItemExpandedBorder = table.LegacyColors.MenuBorder;
            table.LegacyColors.MenuSide = factory.GetColor(0xE9EEEE);
            table.LegacyColors.MenuSide2 = Color.Empty; // factory.GetColor(0xDDE0E8);
            table.LegacyColors.MenuUnusedBackground = table.LegacyColors.MenuBackground;
            table.LegacyColors.MenuUnusedSide = factory.GetColor(0xDADADA);
            table.LegacyColors.MenuUnusedSide2 = Color.Empty;// System.Windows.Forms.ControlPaint.Light(table.LegacyColors.MenuSide2);
            table.LegacyColors.ItemDesignTimeBorder = Color.Black;
            table.LegacyColors.BarDockedBorder = factory.GetColor(0x4C607A);
            table.LegacyColors.DockSiteBackColor = factory.GetColor(0xC7D1E0);
            table.LegacyColors.DockSiteBackColor2 = Color.Empty;
            table.LegacyColors.CustomizeBackground = factory.GetColor(0xCAD4E3);
            table.LegacyColors.CustomizeBackground2 = factory.GetColor(0x919FB4);
            table.LegacyColors.CustomizeText = factory.GetColor(0x4C607A);
            table.LegacyColors.PanelBackground = factory.GetColor(0xF6FBFF);
            table.LegacyColors.PanelBackground2 = factory.GetColor(0xDCE7F5); 
            table.LegacyColors.PanelText = factory.GetColor(0x254264);
            table.LegacyColors.PanelBorder = factory.GetColor(0x8492A6);
            table.LegacyColors.ExplorerBarBackground = factory.GetColor(0xC4C8D4);
            table.LegacyColors.ExplorerBarBackground2 = factory.GetColor(0xB1B3C8);
            #endregion

            #region Navigation Pane
            table.NavigationPane.ButtonBackground = new GradientColorTable();
            table.NavigationPane.ButtonBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xEFF6FD), 0));
            table.NavigationPane.ButtonBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xE8F1FB), .4f));
            table.NavigationPane.ButtonBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xDCE6F4), .4f));
            table.NavigationPane.ButtonBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xDDE9F7), 1));
            #endregion

            #region SuperTooltip
            table.SuperTooltip.BackgroundColors = new LinearGradientColorTable(factory.GetColor(0xFFFFFF), factory.GetColor(0xC9D9EF));
            table.SuperTooltip.TextColor = factory.GetColor(0x4C4C4C);
            #endregion

            #region Slider
            Office2007SliderColorTable sl = table.Slider;
            sl.Default.LabelColor = factory.GetColor(0x0A207D);
            sl.Default.SliderLabelColor = factory.GetColor(0x254264);
            sl.Default.PartBackground = new GradientColorTable();
            sl.Default.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xFFFFFF), 0));
            sl.Default.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xF1F6FC), .15f));
            sl.Default.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xAEC9EE), .5f));
            sl.Default.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(0x6A98D0), .5f));
            sl.Default.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xFFFFFF), .85f));
            sl.Default.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xFFFFFF), 1f));
            sl.Default.PartBorderColor = factory.GetColor(0x2F578D);
            sl.Default.PartBorderLightColor = Color.FromArgb(96, factory.GetColor(0xFFFFFF));
            sl.Default.PartForeColor = factory.GetColor(0x566375);
            sl.Default.PartForeLightColor = Color.FromArgb(168, factory.GetColor(0xE7F0F9));
            sl.Default.TrackLineColor = factory.GetColor(0x7496C2);
            sl.Default.TrackLineLightColor = factory.GetColor(0xDEE2EC);

            sl.MouseOver.LabelColor = factory.GetColor(0x092061);
            sl.MouseOver.SliderLabelColor = factory.GetColor(0x254264);
            sl.MouseOver.PartBackground = new GradientColorTable();
            sl.MouseOver.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xFFFFFF), 0));
            sl.MouseOver.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xFFFDF5), .2f));
            sl.MouseOver.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xFFDF83), .5f));
            sl.MouseOver.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xDDA70D), .5f));
            sl.MouseOver.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xFFF4CE), .85f));
            sl.MouseOver.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xFFF4CE), 1f));
            sl.MouseOver.PartBorderColor = factory.GetColor(0x2F578D);
            sl.MouseOver.PartBorderLightColor = Color.FromArgb(96, factory.GetColor(0xFFFFFF));
            sl.MouseOver.PartForeColor = factory.GetColor(0x67624A);
            sl.MouseOver.PartForeLightColor = Color.FromArgb(168, factory.GetColor(0xE7F0F9));
            sl.MouseOver.TrackLineColor = factory.GetColor(0x7496C2);
            sl.MouseOver.TrackLineLightColor = factory.GetColor(0xDEE2EC);

            sl.Pressed.LabelColor = factory.GetColor(0x092061);
            sl.Pressed.SliderLabelColor = factory.GetColor(0x254264);
            sl.Pressed.PartBackground = new GradientColorTable();
            sl.Pressed.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xDB7518), 0));
            sl.Pressed.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xF7902F), .2f));
            sl.Pressed.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xF9C18B), .5f));
            sl.Pressed.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xED7400), .5f));
            sl.Pressed.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xFFC188), .85f));
            sl.Pressed.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xF2D2B5), 1f));
            sl.Pressed.PartBorderColor = factory.GetColor(0x2F578D);
            sl.Pressed.PartBorderLightColor = Color.FromArgb(96, factory.GetColor(0xFFFFFF));
            sl.Pressed.PartForeColor = factory.GetColor(0x67513E);
            sl.Pressed.PartForeLightColor = Color.FromArgb(168, factory.GetColor(0xE7F0F9));
            sl.Pressed.TrackLineColor = factory.GetColor(0x7496C2);
            sl.Pressed.TrackLineLightColor = factory.GetColor(0xDEE2EC);

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
            table.ListViewEx.Border = factory.GetColor(0x8492A6);
            table.ListViewEx.ColumnBackground = new LinearGradientColorTable(factory.GetColor(0xFFFFFF), factory.GetColor(0xC4DDFF));
            table.ListViewEx.ColumnSeparator = factory.GetColor(0x9AC6FF);
            table.ListViewEx.SelectionBackground = new LinearGradientColorTable(factory.GetColor(0xA7CDF0), Color.Empty);
            table.ListViewEx.SelectionBorder = factory.GetColor(0xE3EFFF);
            #endregion

            #region DataGridView
            table.DataGridView.ColumnHeaderNormalBorder = factory.GetColor(0x9EB6CE);
            table.DataGridView.ColumnHeaderNormalBackground = new LinearGradientColorTable(factory.GetColor(0xF9FCFD), factory.GetColor(0xD3DBE9), 90);
            table.DataGridView.ColumnHeaderSelectedBackground = new LinearGradientColorTable(factory.GetColor(0xF9D99F), factory.GetColor(0xF1C15F), 90);
            table.DataGridView.ColumnHeaderSelectedBorder = factory.GetColor(0xF29536);
            table.DataGridView.ColumnHeaderSelectedMouseOverBackground = new LinearGradientColorTable(factory.GetColor(0xFFD58D), factory.GetColor(0xF2923A), 90);
            table.DataGridView.ColumnHeaderSelectedMouseOverBorder = factory.GetColor(0xF29536);
            table.DataGridView.ColumnHeaderMouseOverBackground = new LinearGradientColorTable(factory.GetColor(0xDFE2E4), factory.GetColor(0xBCC5D2), 90);
            table.DataGridView.ColumnHeaderMouseOverBorder = factory.GetColor(0x879FB7);
            table.DataGridView.ColumnHeaderPressedBackground = new LinearGradientColorTable(factory.GetColor(0xBCC5D2), factory.GetColor(0xDFE2E4), 90);
            table.DataGridView.ColumnHeaderPressedBorder = factory.GetColor(0xFFFFFF);

            table.DataGridView.RowNormalBackground = new LinearGradientColorTable(factory.GetColor(0xE4ECF7));
            table.DataGridView.RowNormalBorder = factory.GetColor(0x9EB6CE);
            table.DataGridView.RowSelectedBackground = new LinearGradientColorTable(factory.GetColor(0xFFD58D));
            table.DataGridView.RowSelectedBorder = factory.GetColor(0xF29536);
            table.DataGridView.RowSelectedMouseOverBackground = new LinearGradientColorTable(factory.GetColor(0xF1C05C));
            table.DataGridView.RowSelectedMouseOverBorder = factory.GetColor(0xF29536);
            table.DataGridView.RowMouseOverBackground = new LinearGradientColorTable(factory.GetColor(0xF1C05C));
            table.DataGridView.RowMouseOverBorder = factory.GetColor(0xF29536);
            table.DataGridView.RowPressedBackground = new LinearGradientColorTable(factory.GetColor(0xBBC4D1));
            table.DataGridView.RowPressedBorder = factory.GetColor(0xFFFFFF);

            table.DataGridView.GridColor = factory.GetColor(0xD0D7E5);

            table.DataGridView.SelectorBackground = new LinearGradientColorTable(factory.GetColor(0xA9C4E9));
            table.DataGridView.SelectorBorder = factory.GetColor(0x9EB6CE);
            table.DataGridView.SelectorBorderDark = factory.GetColor(0xB0CFF7);
            table.DataGridView.SelectorBorderLight = factory.GetColor(0xD5E4F2);
            table.DataGridView.SelectorSign = new LinearGradientColorTable(factory.GetColor(0xF9FAFB), factory.GetColor(0xD7DAE2));

            table.DataGridView.SelectorMouseOverBackground = new LinearGradientColorTable(factory.GetColor(0x8BA0B5));
            table.DataGridView.SelectorMouseOverBorder = factory.GetColor(0x9EB6CE);
            table.DataGridView.SelectorMouseOverBorderDark = factory.GetColor(0xB0CFF7);
            table.DataGridView.SelectorMouseOverBorderLight = factory.GetColor(0xD5E4F2);
            table.DataGridView.SelectorMouseOverSign = new LinearGradientColorTable(factory.GetColor(0xF9FAFB), factory.GetColor(0xD7DAE2));
            #endregion

            #region SideBar
            table.SideBar.Background = new LinearGradientColorTable(factory.GetColor(Color.White));
            table.SideBar.Border = factory.GetColor(0x8492A6);
            table.SideBar.SideBarPanelItemText = factory.GetColor(0x254264);
            table.SideBar.SideBarPanelItemDefault = new GradientColorTable();
            table.SideBar.SideBarPanelItemDefault.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xF9FCFD), 0));
            table.SideBar.SideBarPanelItemDefault.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xE1EDF9), .4f));
            table.SideBar.SideBarPanelItemDefault.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xD5E1F2), .4f));
            table.SideBar.SideBarPanelItemDefault.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xD6E5F6), 1));
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
            crumbBarViewTable.Foreground = factory.GetColor(0x254264);
            crumbBarViewTable = new CrumbBarItemViewStateColorTable();
            table.CrumbBarItemView.MouseOver = crumbBarViewTable;
            crumbBarViewTable.Foreground = factory.GetColor(0x254264);
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
            crumbBarViewTable.Foreground = factory.GetColor(0x254264);
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
            crumbBarViewTable.Foreground = factory.GetColor(0x254264);
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
            table.WarningBox.BackColor = factory.GetColor(0xFCFCFC);
            table.WarningBox.WarningBorderColor = factory.GetColor(0x8492A6);
            table.WarningBox.WarningBackColor1 = factory.GetColor(0xF6FBFF);
            table.WarningBox.WarningBackColor2 = factory.GetColor(0xDCE7F5);
            #endregion

            #region SuperTab

            #region SuperTab

            table.SuperTab.Background = new SuperTabLinearGradientColorTable(factory.GetColor(0xDFE9F5));
            table.SuperTab.InnerBorder = factory.GetColor(0xFDFEFF);
            table.SuperTab.OuterBorder = factory.GetColor(0xBAC9DB);

            table.SuperTab.ControlBoxDefault.Image = factory.GetColor(0x254264);

            table.SuperTab.ControlBoxMouseOver.Background = factory.GetColor(0xCEEDFA);
            table.SuperTab.ControlBoxMouseOver.Border = factory.GetColor(0x3399FF);
            table.SuperTab.ControlBoxMouseOver.Image = factory.GetColor(0xFF, 0x000000);

            table.SuperTab.ControlBoxPressed.Background = factory.GetColor(0xB7CAE0);
            table.SuperTab.ControlBoxPressed.Border = factory.GetColor(0x3399FF);
            table.SuperTab.ControlBoxPressed.Image = factory.GetColor(0xFF, 0x000000);

            table.SuperTab.InsertMarker = factory.GetColor(0xFF, 0x000000);

            #endregion

            #region SuperTabItem

            // Top Default

            table.SuperTabItem.Default.Normal.Background = new SuperTabLinearGradientColorTable();
            table.SuperTabItem.Default.Normal.Text = factory.GetColor(0x254264);
            table.SuperTabItem.Default.Normal.CloseMarker = factory.GetColor(0x254264);

            // Top Selected

            table.SuperTabItem.Default.Selected.Background = new SuperTabLinearGradientColorTable(
                new Color[] { factory.GetColor(0xFDFEFF), factory.GetColor(0xF7FBFF)},
                new float[] { 0, 1 });

            table.SuperTabItem.Default.Selected.InnerBorder = factory.GetColor(0xFDFEFF);
            table.SuperTabItem.Default.Selected.OuterBorder = factory.GetColor(0x92A5C7);
            table.SuperTabItem.Default.Selected.Text = factory.GetColor(0x254264);
            table.SuperTabItem.Default.Selected.CloseMarker = factory.GetColor(0x254264);

            // Top SelectedMouseOver

            table.SuperTabItem.Default.SelectedMouseOver.Background = new SuperTabLinearGradientColorTable(
                new Color[] { factory.GetColor(0xFFFEFE), factory.GetColor(0xFFFAFC)},
                new float[] { 0, 1 });

            table.SuperTabItem.Default.SelectedMouseOver.InnerBorder = factory.GetColor(0xFFFEFE);
            table.SuperTabItem.Default.SelectedMouseOver.OuterBorder = factory.GetColor(0xFFB700);
            table.SuperTabItem.Default.SelectedMouseOver.Text = factory.GetColor(0x254264);
            table.SuperTabItem.Default.SelectedMouseOver.CloseMarker = factory.GetColor(0x254264);

            // Top MouseOver

            table.SuperTabItem.Default.MouseOver.Background = new SuperTabLinearGradientColorTable(
                new Color[] { factory.GetColor(0xE9F0F7), factory.GetColor(0xE9EFF8)},
                new float[] { 0, 1 });

            table.SuperTabItem.Default.MouseOver.InnerBorder = factory.GetColor(0xFFFEFC);
            table.SuperTabItem.Default.MouseOver.OuterBorder = factory.GetColor(0xDEC067);
            table.SuperTabItem.Default.MouseOver.Text = factory.GetColor(0x254264);
            table.SuperTabItem.Default.MouseOver.CloseMarker = factory.GetColor(0x254264);

            // Left, Bottom, Right

            table.SuperTabItem.Left = table.SuperTabItem.Default;
            table.SuperTabItem.Bottom = table.SuperTabItem.Default;
            table.SuperTabItem.Right = table.SuperTabItem.Right;

            #endregion

            #region SuperTabPanel

            table.SuperTabPanel.Default.Background = new SuperTabLinearGradientColorTable(factory.GetColor(0xF7FBFF), Color.Empty);
            table.SuperTabPanel.Default.InnerBorder = factory.GetColor(Color.Empty);
            table.SuperTabPanel.Default.OuterBorder = factory.GetColor(0x92A5C7);

            table.SuperTabPanel.Left = table.SuperTabPanel.Default;
            table.SuperTabPanel.Bottom = table.SuperTabPanel.Default;
            table.SuperTabPanel.Right = table.SuperTabPanel.Default;

            #endregion

            #endregion

            #region Backstage

            #region Backstage

            table.Backstage.Background = new SuperTabLinearGradientColorTable(factory.GetColor(0xDFE9F5));
            table.Backstage.InnerBorder = factory.GetColor(0xFDFEFF);
            table.Backstage.OuterBorder = factory.GetColor(0xBAC9DB);

            table.Backstage.ControlBoxDefault.Image = factory.GetColor(0x254264);

            table.Backstage.ControlBoxMouseOver.Background = factory.GetColor(0xCEEDFA);
            table.Backstage.ControlBoxMouseOver.Border = factory.GetColor(0x3399FF);
            table.Backstage.ControlBoxMouseOver.Image = factory.GetColor(0xFF, 0x000000);

            table.Backstage.ControlBoxPressed.Background = factory.GetColor(0xB7CAE0);
            table.Backstage.ControlBoxPressed.Border = factory.GetColor(0x3399FF);
            table.Backstage.ControlBoxPressed.Image = factory.GetColor(0xFF, 0x000000);

            table.Backstage.InsertMarker = factory.GetColor(0xFF, 0x000000);

            #endregion

            #region BackstageItem

            // Top Default

            table.BackstageItem.Default.Normal.Background = new SuperTabLinearGradientColorTable();
            table.BackstageItem.Default.Normal.Text = factory.GetColor(0x254264);
            table.BackstageItem.Default.Normal.CloseMarker = factory.GetColor(0x254264);

            // Top Selected

            table.BackstageItem.Default.Selected.Background = new SuperTabLinearGradientColorTable(
                new Color[] { factory.GetColor(0xFDFEFF), factory.GetColor(0xF7FBFF) },
                new float[] { 0, 1 });

            table.BackstageItem.Default.Selected.InnerBorder = factory.GetColor(0xFDFEFF);
            table.BackstageItem.Default.Selected.OuterBorder = factory.GetColor(0x92A5C7);
            table.BackstageItem.Default.Selected.Text = factory.GetColor(0x254264);
            table.BackstageItem.Default.Selected.CloseMarker = factory.GetColor(0x254264);

            // Top SelectedMouseOver

            table.BackstageItem.Default.SelectedMouseOver.Background = new SuperTabLinearGradientColorTable(
                new Color[] { factory.GetColor(0xFFFEFE), factory.GetColor(0xFFFAFC) },
                new float[] { 0, 1 });

            table.BackstageItem.Default.SelectedMouseOver.InnerBorder = factory.GetColor(0xFFFEFE);
            table.BackstageItem.Default.SelectedMouseOver.OuterBorder = factory.GetColor(0xFFB700);
            table.BackstageItem.Default.SelectedMouseOver.Text = factory.GetColor(0x254264);
            table.BackstageItem.Default.SelectedMouseOver.CloseMarker = factory.GetColor(0x254264);

            // Top MouseOver

            table.BackstageItem.Default.MouseOver.Background = new SuperTabLinearGradientColorTable(
                new Color[] { factory.GetColor(0xE9F0F7), factory.GetColor(0xE9EFF8) },
                new float[] { 0, 1 });

            table.BackstageItem.Default.MouseOver.InnerBorder = factory.GetColor(0xFFFEFC);
            table.BackstageItem.Default.MouseOver.OuterBorder = factory.GetColor(0xDEC067);
            table.BackstageItem.Default.MouseOver.Text = factory.GetColor(0x254264);
            table.BackstageItem.Default.MouseOver.CloseMarker = factory.GetColor(0x254264);

            // Left, Bottom, Right

            table.BackstageItem.Left = table.BackstageItem.Default;
            table.BackstageItem.Bottom = table.BackstageItem.Default;
            table.BackstageItem.Right = table.BackstageItem.Right;

            #endregion

            #region BackstagePanel

            table.BackstagePanel.Default.Background = new SuperTabLinearGradientColorTable(factory.GetColor(0xF7FBFF), Color.Empty);
            table.BackstagePanel.Default.InnerBorder = factory.GetColor(Color.Empty);
            table.BackstagePanel.Default.OuterBorder = factory.GetColor(0x92A5C7);

            table.BackstagePanel.Left = table.BackstagePanel.Default;
            table.BackstagePanel.Bottom = table.BackstagePanel.Default;
            table.BackstagePanel.Right = table.BackstagePanel.Default;

            #endregion

            #endregion

            #region SwitchButton
            SwitchButtonColorTable sbt = new SwitchButtonColorTable();
            sbt.BorderColor = factory.GetColor(0xAABBD2);
            sbt.OffBackColor = factory.GetColor(0xF6FBFF);
            sbt.OffTextColor = factory.GetColor(0x254264);
            sbt.OnBackColor = factory.GetColor(0x92D050);
            sbt.OnTextColor = factory.GetColor(0x254264);
            sbt.SwitchBackColor = factory.GetColor(0xDCE7F5);
            sbt.SwitchBorderColor = factory.GetColor(0x9EB2C9);
            sbt.TextColor = factory.GetColor(0x254264);
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
            style.BorderColor = factory.GetColor(0xAABBD2);
            style.Border = eStyleBorderType.Solid;
            style.BorderWidth = 1;
            style.CornerDiameter = 2;
            style.CornerType = eCornerType.Rounded;
            style.BackColor = factory.GetColor(0xF3F7FC);
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
            style = Office2007ColorTableFactory.GetTextBoxStyle(factory.GetColor(0x8492A6));
            table.StyleClasses.Add(style.Class, style);
            // ItemPanel
            style = Office2007ColorTableFactory.GetItemPanelStyle(factory.GetColor(0x8492A6));
            table.StyleClasses.Add(style.Class, style);
            // DateTimeInput background
            style = Office2007ColorTableFactory.GetDateTimeInputBackgroundStyle(factory.GetColor(0x8492A6));
            table.StyleClasses.Add(style.Class, style);
            // Ribbon Client Panel
            style = Office2007ColorTableFactory.GetRibbonClientPanelStyle(factory, eOffice2007ColorScheme.Blue);
            table.StyleClasses.Add(style.Class, style);
            // ListView Border
            style = Office2007ColorTableFactory.GetListViewBorderStyle(table.ListViewEx);
            table.StyleClasses.Add(style.Class, style);
            style = Office2007ColorTableFactory.GetStatusBarAltStyle(table.Bar);
            table.StyleClasses.Add(style.Class, style);
#if !NOTREE
            // Tree Border/Background
            style = Office2007ColorTableFactory.GetAdvTreeStyle(factory.GetColor(0x8492A6));
            table.StyleClasses.Add(style.Class, style);
            style = Office2007ColorTableFactory.GetAdvTreeColumnsHeaderStyle(factory.GetColor(0xF9FCFD), factory.GetColor(0xD3DBE9), factory.GetColor(0x9EB6CE));
            table.StyleClasses.Add(style.Class, style);
            style = Office2007ColorTableFactory.GetAdvTreeNodesColumnsHeaderStyle(factory.GetColor(0xF9FCFD), factory.GetColor(0xD3DBE9), factory.GetColor(0x9EB6CE));
            table.StyleClasses.Add(style.Class, style);
            style = Office2007ColorTableFactory.GetAdvTreeColumnStyle(factory.GetColor(0x000000));
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
            rb.TopBackgroundHeight = 15;
            rb.OuterBorder = new LinearGradientColorTable(factory.GetColor(0x9DB2C9), factory.GetColor(0xA5B8D0));
            rb.InnerBorder = new LinearGradientColorTable(factory.GetColor(160, 0xFFFFFF));
            rb.TopBackground = new LinearGradientColorTable(factory.GetColor(0xF7FBFF), factory.GetColor(0xDCE7F5));
            rb.BottomBackground = null;// new LinearGradientColorTable(factory.GetColor(0xC7D8ED), factory.GetColor(0xD8E8F5));
            rb.TitleBackground = null;// new LinearGradientColorTable(factory.GetColor(0xC2D8F1), factory.GetColor(0xC0D8EF));
            rb.TitleText = factory.GetColor(0x738399);
            return rb;
        }

        public static Office2007RibbonBarStateColorTable GetRibbonBarMouseOver(ColorFactory factory)
        {
            Office2007RibbonBarStateColorTable rb = new Office2007RibbonBarStateColorTable();
            rb.TopBackgroundHeight = 15;
            rb.OuterBorder = new LinearGradientColorTable(factory.GetColor(0x9CB1C7), factory.GetColor(0xA4B7CF));
            rb.InnerBorder = new LinearGradientColorTable(factory.GetColor(0xF6F9FE), factory.GetColor(0xEBF0F9));
            rb.TopBackground = new LinearGradientColorTable(factory.GetColor(0xF5F6FB), factory.GetColor(0xDCE7F5));
            rb.BottomBackground = null;// new LinearGradientColorTable(factory.GetColor(0xC7D8ED), factory.GetColor(0xD8E8F5));
            rb.TitleBackground = new LinearGradientColorTable(factory.GetColor(0xE7F9FC), Color.Transparent);
            rb.TitleText = factory.GetColor(0x738399);
            return rb;
        }
        #endregion

        #region Buttons
        public static Office2007ButtonItemColorTable GetButtonItemBlueOrange(ColorFactory factory)
        {
            Office2007ButtonItemColorTable cb = new Office2007ButtonItemColorTable();
            cb.Default = new Office2007ButtonItemStateColorTable();
            cb.Default.Text = factory.GetColor(0x254264);

            // Button mouse over
            cb.MouseOver = new Office2007ButtonItemStateColorTable();
            cb.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(0xFFB700), factory.GetColor(0xFFB700));
            cb.MouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(0xFEF9F4), factory.GetColor(0xFFFEE8));
            cb.MouseOver.TopBackground = new LinearGradientColorTable(factory.GetColor(0xFDECDB), factory.GetColor(0xFDDEB8));
            cb.MouseOver.TopBackgroundHighlight = LinearGradientColorTable.Empty; //new LinearGradientColorTable(Color.FromArgb(192, Color.White), Color.Transparent);
            cb.MouseOver.BottomBackground = new LinearGradientColorTable(factory.GetColor(0xFFCE68), factory.GetColor(0xFFDE8D));
            cb.MouseOver.BottomBackgroundHighlight = new LinearGradientColorTable(factory.GetColor(0xFFFD9C), Color.Transparent);
            cb.MouseOver.SplitBorder = new LinearGradientColorTable(factory.GetColor(0xFFB700), Color.Empty);
            cb.MouseOver.SplitBorderLight = new LinearGradientColorTable(factory.GetColor(0xFEE9BA), Color.Empty);
            cb.MouseOver.Text = factory.GetColor(0x254264);

            cb.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
            cb.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(factory.GetColor(0xADBDD2));

            // Pressed
            cb.Pressed = new Office2007ButtonItemStateColorTable();
            cb.Pressed.OuterBorder = new LinearGradientColorTable(factory.GetColor(0xC2923B), factory.GetColor(0xC2B359));
            cb.Pressed.InnerBorder = new LinearGradientColorTable(factory.GetColor(0xD4C0AC), Color.Transparent);
            cb.Pressed.TopBackground = new LinearGradientColorTable(factory.GetColor(0xE4CAAE), factory.GetColor(0xEDC78D));
            cb.Pressed.TopBackgroundHighlight = LinearGradientColorTable.Empty;
            cb.Pressed.BottomBackground = new LinearGradientColorTable(factory.GetColor(0xEFBC61), factory.GetColor(0xEED084));
            cb.Pressed.BottomBackgroundHighlight = new LinearGradientColorTable(factory.GetColor(0xEFF1BD), Color.Transparent);
            cb.Pressed.SplitBorder = new LinearGradientColorTable(factory.GetColor(0xC2A14D), Color.Empty);
            cb.Pressed.SplitBorderLight = new LinearGradientColorTable(factory.GetColor(0xD6B36F), Color.Empty);
            cb.Pressed.Text = factory.GetColor(0x254264);

            // Checked
            cb.Checked = new Office2007ButtonItemStateColorTable();
            cb.Checked.OuterBorder = new LinearGradientColorTable(factory.GetColor(0xC2923B), factory.GetColor(0xC2B14F));
            cb.Checked.InnerBorder = new LinearGradientColorTable(factory.GetColor(0xE2C9AE), Color.Transparent);
            cb.Checked.TopBackground = new LinearGradientColorTable(factory.GetColor(0xF4D4B1), factory.GetColor(0xFDD08D));
            cb.Checked.TopBackgroundHighlight = LinearGradientColorTable.Empty;
            cb.Checked.BottomBackground = new LinearGradientColorTable(factory.GetColor(0xFFC45E), factory.GetColor(0xFDE894));
            cb.Checked.BottomBackgroundHighlight = new LinearGradientColorTable(factory.GetColor(0xFFFFC5), Color.Transparent);
            cb.Checked.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
            cb.Checked.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
            cb.Checked.Text = factory.GetColor(0x254264);

            // Expanded button
            cb.Expanded = new Office2007ButtonItemStateColorTable();
            cb.Expanded.OuterBorder = new LinearGradientColorTable(factory.GetColor(0xC2923B), factory.GetColor(0xC2B14F));
            cb.Expanded.InnerBorder = new LinearGradientColorTable(factory.GetColor(0xE2C9AE), Color.Transparent);
            cb.Expanded.TopBackground = new LinearGradientColorTable(factory.GetColor(0xF4D4B1), factory.GetColor(0xFDD08D));
            cb.Expanded.TopBackgroundHighlight = LinearGradientColorTable.Empty;
            cb.Expanded.BottomBackground = new LinearGradientColorTable(factory.GetColor(0xFFC45E), factory.GetColor(0xFDE894));
            cb.Expanded.BottomBackgroundHighlight = new LinearGradientColorTable(factory.GetColor(0xFFFFC5), Color.Transparent);
            cb.Expanded.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
            cb.Expanded.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
            cb.Expanded.Text = factory.GetColor(0x254264);

            SetBlueExpandColors(cb, factory);

            return cb;
        }

        public static Office2007ButtonItemColorTable GetButtonItemBlueOrangeWithBackground(ColorFactory factory)
        {
            Office2007ButtonItemColorTable cb = GetButtonItemBlueOrange(factory);
            cb.Default.TopBackground = new LinearGradientColorTable(factory.GetColor(0xF9FCFD), factory.GetColor(0xE1EDF9));
            cb.Default.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
            cb.Default.BottomBackground = new LinearGradientColorTable(factory.GetColor(0xD5E1F2), factory.GetColor(0xD6E5F6));
            cb.Default.BottomBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
            cb.Default.InnerBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
            cb.Default.OuterBorder = new LinearGradientColorTable(factory.GetColor(0x8D9FB7), Color.Empty);
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
            Color cb = factory.GetColor(0x4C607A);
            Color cl = Color.FromArgb(192, Color.White);
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
        public static Office2007RibbonTabItemColorTable GetRibbonTabItemBlueDefault(ColorFactory factory)
        {
            Office2007RibbonTabItemColorTable rt = new Office2007RibbonTabItemColorTable();
            rt.Default.Text = factory.GetColor(0x254264);
            rt.CornerSize = 2;

            // Selected Tab
            rt.Selected = new Office2007RibbonTabItemStateColorTable();
            rt.Selected.Background = new LinearGradientColorTable(factory.GetColor(0xFDFEFF), factory.GetColor(0xF7FBFF));
            rt.Selected.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(32, Color.White), Color.Transparent);
            rt.Selected.InnerBorder = new LinearGradientColorTable(factory.GetColor(0xFDFEFF), factory.GetColor(0xFCFEFF));
            rt.Selected.OuterBorder = new LinearGradientColorTable(factory.GetColor(0xBAC9DB), factory.GetColor(0xBAC9DB));
            rt.Selected.Text = factory.GetColor(0x254264);

            // Selected Tab Mouse Over
            rt.SelectedMouseOver = new Office2007RibbonTabItemStateColorTable();
            rt.SelectedMouseOver.Background = new LinearGradientColorTable(factory.GetColor(0xFFFEFE), factory.GetColor(0xFFFAFC));
            rt.SelectedMouseOver.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(32, Color.White), Color.Transparent);
            rt.SelectedMouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(0xFFFEFE), factory.GetColor(0xFFFDFD));
            rt.SelectedMouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(0xFFB700), factory.GetColor(0xFFA84E));
            rt.SelectedMouseOver.Text = factory.GetColor(0x254264);

            // Tab Mouse Over
            rt.MouseOver = new Office2007RibbonTabItemStateColorTable();
            rt.MouseOver.Background = new LinearGradientColorTable(factory.GetColor(0xE9F0F7), factory.GetColor(0xE9EFF8));
            rt.MouseOver.BackgroundHighlight = LinearGradientColorTable.Empty;
            rt.MouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(0xFFFEFC), factory.GetColor(0xF2F5F9));
            rt.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(0xFFB700), factory.GetColor(0xDEC067));
            rt.MouseOver.Text = factory.GetColor(0x254264);

            return rt;
        }

        public static Office2007RibbonTabItemColorTable GetRibbonTabItemBlueMagenta(ColorFactory factory)
        {
            Office2007RibbonTabItemColorTable rt = new Office2007RibbonTabItemColorTable();
            rt.Default.Text = factory.GetColor(0x254264);
            // Selected Tab
            rt.Selected = new Office2007RibbonTabItemStateColorTable();
            rt.Selected.Background = new LinearGradientColorTable(factory.GetColor(0xD4C8E2), factory.GetColor(0xECE7F2));
            rt.Selected.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
            rt.Selected.InnerBorder = new LinearGradientColorTable(factory.GetColor(0xECE7F3), factory.GetColor(0xCDC2DF));
            rt.Selected.OuterBorder = new LinearGradientColorTable(factory.GetColor(0xBEBAC1), factory.GetColor(0xC0BFC1));
            rt.Selected.Text = factory.GetColor(0x254264);

            // Selected Tab Mouse Over
            rt.SelectedMouseOver = new Office2007RibbonTabItemStateColorTable();
            rt.SelectedMouseOver.Background = new LinearGradientColorTable(factory.GetColor(0xD4C8E2), factory.GetColor(0xECE7F2));
            rt.SelectedMouseOver.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(32, Color.White), Color.Transparent);
            rt.SelectedMouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(0xECE7F3), factory.GetColor(0xCDC2DF));
            rt.SelectedMouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(0xBEBAC1), factory.GetColor(0xC0BFC1));
            rt.SelectedMouseOver.Text = factory.GetColor(0x254264);

            // Tab Mouse Over
            rt.MouseOver = new Office2007RibbonTabItemStateColorTable();
            rt.MouseOver.Background = new LinearGradientColorTable(factory.GetColor(0xC4D5F8), factory.GetColor(0xC2BCE8));
            rt.MouseOver.BackgroundHighlight = new LinearGradientColorTable(factory.GetColor(0xDEECFF), Color.Transparent);
            rt.MouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(0xE0EEFF), factory.GetColor(0xC7DFFF));
            rt.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(0xC1C8D1), factory.GetColor(0xC0C7D0));
            rt.MouseOver.Text = factory.GetColor(0x254264);

            return rt;
        }

        public static Office2007RibbonTabItemColorTable GetRibbonTabItemBlueGreen(ColorFactory factory)
        {
            Office2007RibbonTabItemColorTable rt = new Office2007RibbonTabItemColorTable();
            rt.Default.Text = factory.GetColor(0x254264);

            // Selected Tab
            rt.Selected = new Office2007RibbonTabItemStateColorTable();
            rt.Selected.Background = new LinearGradientColorTable(factory.GetColor(0xE4F3E0), factory.GetColor(0xE4F2DF));
            rt.Selected.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
            rt.Selected.InnerBorder = new LinearGradientColorTable(factory.GetColor(0xD5EAD3), factory.GetColor(0xB8DEB1));
            rt.Selected.OuterBorder = new LinearGradientColorTable(factory.GetColor(0xB9C1B7), factory.GetColor(0xBFC1BE));
            rt.Selected.Text = factory.GetColor(0x254264);

            // Selected Tab Mouse Over
            rt.SelectedMouseOver = new Office2007RibbonTabItemStateColorTable();
            rt.SelectedMouseOver.Background = new LinearGradientColorTable(factory.GetColor(0xE4F3E0), factory.GetColor(0xE4F2DF));
            rt.SelectedMouseOver.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(32, Color.White), Color.Transparent);
            rt.SelectedMouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(0xD5EAD3), factory.GetColor(0xB8DEB1));
            rt.SelectedMouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(0xB9C1B7), factory.GetColor(0xBFC1BE));
            rt.SelectedMouseOver.Text = factory.GetColor(0x254264);

            // Tab Mouse Over
            rt.MouseOver = new Office2007RibbonTabItemStateColorTable();
            rt.MouseOver.Background = new LinearGradientColorTable(factory.GetColor(0xB3DEE3), factory.GetColor(0x8ADE9F));
            rt.MouseOver.BackgroundHighlight = new LinearGradientColorTable(factory.GetColor(0xDEECFF), Color.Transparent);
            rt.MouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(0xE2EFFF), factory.GetColor(0xC7DFFF));
            rt.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(0xC1C8D1), factory.GetColor(0xC0C7D0));
            rt.MouseOver.Text = factory.GetColor(0x254264);

            return rt;
        }

        public static Office2007RibbonTabItemColorTable GetRibbonTabItemBlueOrange(ColorFactory factory)
        {
            Office2007RibbonTabItemColorTable rt = new Office2007RibbonTabItemColorTable();
            rt.Default.Text = factory.GetColor(0x254264);

            // Selected Tab
            rt.Selected = new Office2007RibbonTabItemStateColorTable();
            rt.Selected.Background = new LinearGradientColorTable(factory.GetColor(0xFFF59F), factory.GetColor(0xFFFAD5));
            rt.Selected.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
            rt.Selected.InnerBorder = new LinearGradientColorTable(factory.GetColor(0xFFFBD6), factory.GetColor(0xFDF28C));
            rt.Selected.OuterBorder = new LinearGradientColorTable(factory.GetColor(0xCAC7AC), factory.GetColor(0xC2C2C2));
            rt.Selected.Text = factory.GetColor(0x254264);

            // Selected Tab Mouse Over
            rt.SelectedMouseOver = new Office2007RibbonTabItemStateColorTable();
            rt.SelectedMouseOver.Background = new LinearGradientColorTable(factory.GetColor(0xFFF59F), factory.GetColor(0xFFFAD5));
            rt.SelectedMouseOver.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(32, Color.White), Color.Transparent);
            rt.SelectedMouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(0xFFFBD6), factory.GetColor(0xFDF28C));
            rt.SelectedMouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(0xCAC7AC), factory.GetColor(0xC2C2C2));
            rt.SelectedMouseOver.Text = factory.GetColor(0x254264);

            // Tab Mouse Over
            rt.MouseOver = new Office2007RibbonTabItemStateColorTable();
            rt.MouseOver.Background = new LinearGradientColorTable(factory.GetColor(0xCFE0E1), factory.GetColor(0xE9E799));
            rt.MouseOver.BackgroundHighlight = new LinearGradientColorTable(factory.GetColor(0xCBE2FF), Color.Transparent);
            rt.MouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(0xE2EFFF), factory.GetColor(0xC7DFFF));
            rt.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(0xC1C8D1), factory.GetColor(0xC0C7D0));
            rt.MouseOver.Text = factory.GetColor(0x254264);

            return rt;
        }
        #endregion

        #region Style Class Creation
        public static ElementStyle GetFileMenuContainerStyle(Office2007ColorTable table)
        {
            ElementStyle style = new ElementStyle();
            style.Class = ElementStyleClassKeys.RibbonFileMenuContainerKey;
            Rendering.Office2007MenuColorTable mc = table.Menu;

            style.PaddingBottom = 0;
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

            BackgroundColorBlend[] blend = new BackgroundColorBlend[mc.FileBottomContainerBackgroundBlend.Count];
            mc.FileBottomContainerBackgroundBlend.CopyTo(blend);
            style.BackColorBlend.Clear();
            style.BackColorBlend.AddRange(blend);
            style.BackColorGradientAngle = 90;
            style.MarginTop = 0;
            style.MarginRight = 2;
            style.PaddingTop = 1;
            return style;
        }
        #endregion
    }
}
