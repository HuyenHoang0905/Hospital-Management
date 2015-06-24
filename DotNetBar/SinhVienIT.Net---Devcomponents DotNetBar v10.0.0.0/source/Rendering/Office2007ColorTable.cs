using System;
using System.Text;
using System.Drawing;
using System.Collections;
using DevComponents.DotNetBar.Schedule;

namespace DevComponents.DotNetBar.Rendering
{
    /// <summary>
    /// Represents color table for Office 2007 style.
    /// </summary>
    public class Office2007ColorTable: IElementStyleClassProvider, IDisposable
    {
        #region Private Variables
        private Office2007ButtonItemColorTableCollection m_ButtonItemColors = new Office2007ButtonItemColorTableCollection();
        private Office2007ButtonItemColorTableCollection m_RibbonButtonItemColors = new Office2007ButtonItemColorTableCollection();
        private Office2007ButtonItemColorTableCollection m_MenuButtonItemColors = new Office2007ButtonItemColorTableCollection();
        private Office2007ButtonItemColorTableCollection _ApplicationButtonColors = new Office2007ButtonItemColorTableCollection();
        private Office2007ButtonItemColorTableCollection _BackstageButtonColors = new Office2007ButtonItemColorTableCollection();
        private Office2007RibbonTabItemColorTableCollection m_RibbonTabItemColors = new Office2007RibbonTabItemColorTableCollection();
        private Office2007RibbonTabGroupColorTableCollection m_RibbonTabGroupColors = new Office2007RibbonTabGroupColorTableCollection();
        private eOffice2007ColorScheme m_ColorScheme = eOffice2007ColorScheme.Blue;
        private Hashtable m_StyleClasses = new Hashtable();
        private ColorFactory m_ColorFactory = new ColorFactory();
        #endregion

        #region Constructor
        /// <summary>
        /// Creates new instance of the object.
        /// </summary>
        public Office2007ColorTable()
        {
            InitializeBlueColorTable(m_ColorFactory);
        }

        /// <summary>
        /// Initializes a new instance of the Office2007ColorTable class.
        /// </summary>
        /// <param name="colorFactory">Specifies the color factory for the color table.</param>
        public Office2007ColorTable(ColorFactory colorFactory)
        {
            m_ColorFactory = colorFactory;
            InitializeBlueColorTable(m_ColorFactory);
        }

        private void InitializeBlueColorTable(ColorFactory factory)
        {
            #region RibbonControl Start Images
            this.RibbonControl.StartButtonDefault = BarFunctions.LoadBitmap("SystemImages.BlankStartButtonNormal.png");
            this.RibbonControl.StartButtonMouseOver = BarFunctions.LoadBitmap("SystemImages.BlankStartButtonHot.png");
            this.RibbonControl.StartButtonPressed = BarFunctions.LoadBitmap("SystemImages.BlankStartButtonPressed.png");
            #endregion

            #region RibbonControl
            this.RibbonControl.OuterBorder = new LinearGradientColorTable(factory.GetColor(0x8DB2E3), factory.GetColor(0x88A1C2));
            this.RibbonControl.InnerBorder = new LinearGradientColorTable(factory.GetColor(0xE7EFF8), factory.GetColor(0xC0F9FF));
            this.RibbonControl.TabsBackground = new LinearGradientColorTable(factory.GetColor(0xBFDBFF), Color.Empty);
            this.RibbonControl.TabDividerBorder = factory.GetColor(0xAECAF0);
            this.RibbonControl.TabDividerBorderLight = factory.GetColor(0xD4E3F5);
            this.RibbonControl.CornerSize = 3;
            this.RibbonControl.PanelTopBackgroundHeight = 15;
            this.RibbonControl.PanelTopBackground = new LinearGradientColorTable(factory.GetColor(0xDBE6F4), factory.GetColor(0xCFDDEF));
            this.RibbonControl.PanelBottomBackground = new LinearGradientColorTable(factory.GetColor(0xC9D9ED), factory.GetColor(0xE7F2FF));
            #endregion

            #region Item Group
            this.ItemGroup.OuterBorder = new LinearGradientColorTable(factory.GetColor(0x99B6E0), factory.GetColor(0x7394BD));
            this.ItemGroup.InnerBorder = new LinearGradientColorTable(factory.GetColor(0xD5E3F1), factory.GetColor(0xE3EDFB));
            this.ItemGroup.TopBackground = new LinearGradientColorTable(factory.GetColor(0xC8DBEE), factory.GetColor(0xC9DDF6));
            this.ItemGroup.BottomBackground = new LinearGradientColorTable(factory.GetColor(0xBCD0E9), factory.GetColor(0xD0E1F7));
            this.ItemGroup.ItemGroupDividerDark = Color.FromArgb(196, factory.GetColor(0xB8C8DC));
            this.ItemGroup.ItemGroupDividerLight = Color.FromArgb(128, factory.GetColor(0xFFFFFF));
            #endregion

            #region RibbonBar
            RibbonBar.Default = Office2007ColorTableFactory.GetRibbonBarBlue(factory);
            RibbonBar.MouseOver = Office2007ColorTableFactory.GetRibbonBarBlueMouseOver(factory);
            RibbonBar.Expanded = Office2007ColorTableFactory.GetRibbonBarBlueExpanded(factory);
            #endregion

            #region ButtonItem Colors Initialization
            // Orange
            Office2007ButtonItemColorTable cb = Office2007ColorTableFactory.GetButtonItemBlueOrange(factory);
            cb.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Orange);
            m_ButtonItemColors.Add(cb);
            // Orange with background
            cb = Office2007ColorTableFactory.GetButtonItemBlueOrangeWithBackground(factory);
            cb.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.OrangeWithBackground);
            m_ButtonItemColors.Add(cb);
            // Blue
            cb = Office2007ColorTableFactory.GetButtonItemBlueBlue(factory);
            cb.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Blue);
            m_ButtonItemColors.Add(cb);
            // Blue with background
            cb = Office2007ColorTableFactory.GetButtonItemBlueBlueWithBackground(factory);
            cb.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.BlueWithBackground);
            m_ButtonItemColors.Add(cb);
            // Magenta
            cb = Office2007ColorTableFactory.GetButtonItemBlueMagenta(factory);
            cb.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Magenta);
            m_ButtonItemColors.Add(cb);
            // Blue with background
            cb = Office2007ColorTableFactory.GetButtonItemBlueMagentaWithBackground(factory);
            cb.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.MagentaWithBackground);
            m_ButtonItemColors.Add(cb);

            cb = Office2007ColorTableFactory.GetButtonItemOffice2007WithBackground(factory);
            cb.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Office2007WithBackground);
            m_ButtonItemColors.Add(cb);

            m_ButtonItemColors.Add(ButtonItemStaticColorTables.CreateBlueOrbColorTable(factory));
            #endregion

            #region RibbonTabItem Colors Initialization
            Office2007RibbonTabItemColorTable rt = Office2007ColorTableFactory.GetRibbonTabItemBlueDefault(factory);
            rt.Name = Enum.GetName(typeof(eRibbonTabColor), eRibbonTabColor.Default);
            m_RibbonTabItemColors.Add(rt);

            // Magenta
            rt = Office2007ColorTableFactory.GetRibbonTabItemBlueMagenta(factory);
            rt.Name = Enum.GetName(typeof(eRibbonTabColor), eRibbonTabColor.Magenta);
            m_RibbonTabItemColors.Add(rt);

            // Green
            rt = Office2007ColorTableFactory.GetRibbonTabItemBlueGreen(factory);
            rt.Name = Enum.GetName(typeof(eRibbonTabColor), eRibbonTabColor.Green);
            m_RibbonTabItemColors.Add(rt);

            // Orange
            rt = Office2007ColorTableFactory.GetRibbonTabItemBlueOrange(factory);
            rt.Name = Enum.GetName(typeof(eRibbonTabColor), eRibbonTabColor.Orange);
            m_RibbonTabItemColors.Add(rt);
            #endregion

            #region RibbonTabItemGroup Colors Initialization
            // Default
            Office2007RibbonTabGroupColorTable tg = Office2007ColorTableFactory.GetRibbonTabGroupBlueDefault(factory);
            tg.Name = Enum.GetName(typeof(eRibbonTabGroupColor), eRibbonTabGroupColor.Default);
            m_RibbonTabGroupColors.Add(tg);

            // Magenta
            tg = Office2007ColorTableFactory.GetRibbonTabGroupBlueMagenta(factory);
            tg.Name = Enum.GetName(typeof(eRibbonTabGroupColor), eRibbonTabGroupColor.Magenta);
            m_RibbonTabGroupColors.Add(tg);

            // Green
            tg = Office2007ColorTableFactory.GetRibbonTabGroupBlueGreen(factory);
            tg.Name = Enum.GetName(typeof(eRibbonTabGroupColor), eRibbonTabGroupColor.Green);
            m_RibbonTabGroupColors.Add(tg);

            // Orange
            tg = Office2007ColorTableFactory.GetRibbonTabGroupBlueOrange(factory);
            tg.Name = Enum.GetName(typeof(eRibbonTabGroupColor), eRibbonTabGroupColor.Orange);
            m_RibbonTabGroupColors.Add(tg);
            #endregion

            #region Initialize Bar
            Bar.ToolbarTopBackground = new LinearGradientColorTable(factory.GetColor(0xD7E6F9), factory.GetColor(0xC7DCF8));
            Bar.ToolbarBottomBackground = new LinearGradientColorTable(factory.GetColor(0xB3D0F5), factory.GetColor(0xD7E5F7));
            Bar.ToolbarBottomBorder = factory.GetColor(0xBAD4F7);
            Bar.PopupToolbarBackground = new LinearGradientColorTable(factory.GetColor(0xFAFAFA), Color.Empty);
            Bar.PopupToolbarBorder = factory.GetColor(0x868686);
            Bar.StatusBarTopBorder = factory.GetColor(0x567DB0);
            Bar.StatusBarTopBorderLight = factory.GetColor(Color.FromArgb(148, Color.White));
            Bar.StatusBarAltBackground.Clear();
            Bar.StatusBarAltBackground.Add(new BackgroundColorBlend(factory.GetColor(0xC5DCF8), 0f));
            Bar.StatusBarAltBackground.Add(new BackgroundColorBlend(factory.GetColor(0xA9CAF7), 0.4f));
            Bar.StatusBarAltBackground.Add(new BackgroundColorBlend(factory.GetColor(0x90B6EA), 0.4f));
            Bar.StatusBarAltBackground.Add(new BackgroundColorBlend(factory.GetColor(0x7495C2), 1f));
            #endregion

            #region Menu
            this.Menu.Background = new LinearGradientColorTable(factory.GetColor(0xFAFAFA), Color.Empty);
            this.Menu.Border = new LinearGradientColorTable(factory.GetColor(0x868686), Color.Empty);
            this.Menu.Side = new LinearGradientColorTable(factory.GetColor(0xE9EEEE), Color.Empty);
            this.Menu.SideBorder = new LinearGradientColorTable(factory.GetColor(0xC5C5C5), Color.Empty);
            this.Menu.SideBorderLight = new LinearGradientColorTable(factory.GetColor(0xF5F5F5), Color.Empty);
            this.Menu.SideUnused = new LinearGradientColorTable(factory.GetColor(0xE5E5E5), Color.Empty);
            this.Menu.FileBackgroundBlend.Clear();
            this.Menu.FileBackgroundBlend.AddRange(new DevComponents.DotNetBar.BackgroundColorBlend[] {
                new DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(229)))), ((int)(((byte)(247))))), 0F),
                new DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(231)))), ((int)(((byte)(247))))), 4F),
                new DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(((int)(((byte)(207)))), ((int)(((byte)(223)))), ((int)(((byte)(243))))), 4F),
                new DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(211)))), ((int)(((byte)(239))))), 1F)});
            this.Menu.FileContainerBorder = Color.White;
            this.Menu.FileContainerBorderLight = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(175)))), ((int)(((byte)(202)))));
            this.Menu.FileColumnOneBackground = factory.GetColor(0xFAFAFA);
            this.Menu.FileColumnOneBorder = factory.GetColor(0xC5C5C5);
            this.Menu.FileColumnTwoBackground = factory.GetColor(0xE9EAEE);
            this.Menu.FileBottomContainerBackgroundBlend.Clear();
            this.Menu.FileBottomContainerBackgroundBlend.AddRange(new DevComponents.DotNetBar.BackgroundColorBlend[] {
                new DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(211)))), ((int)(((byte)(239))))), 0F),
                new DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(212)))), ((int)(((byte)(240))))), 0.4F),
                new DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(201)))), ((int)(((byte)(234))))), 0.4F),
                new DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.FromArgb(((int)(((byte)(207)))), ((int)(((byte)(224)))), ((int)(((byte)(245))))), 1F)});
            #endregion

            #region ComboBox
            this.ComboBox.Default.Background = factory.GetColor(0xEAF2FB);
            this.ComboBox.Default.Border = factory.GetColor(0xABC1DE);
            this.ComboBox.Default.ExpandBackground = new LinearGradientColorTable();
            this.ComboBox.Default.ExpandBorderInner = new LinearGradientColorTable();
            this.ComboBox.Default.ExpandBorderOuter = new LinearGradientColorTable();
            this.ComboBox.Default.ExpandText = factory.GetColor(0x15428B);
            this.ComboBox.DefaultStandalone.Background = factory.GetColor(0xFFFFFF);
            this.ComboBox.DefaultStandalone.Border = factory.GetColor(0xB3C7E1);
            this.ComboBox.DefaultStandalone.ExpandBackground = new LinearGradientColorTable(factory.GetColor(0xE4F0FE), factory.GetColor(0xCFDFF3), 90);
            this.ComboBox.DefaultStandalone.ExpandBorderInner = new LinearGradientColorTable();
            this.ComboBox.DefaultStandalone.ExpandBorderOuter = new LinearGradientColorTable(factory.GetColor(0xAFC4DF), Color.Empty, 90);
            this.ComboBox.DefaultStandalone.ExpandText = factory.GetColor(0x15428B);
            this.ComboBox.MouseOver.Background = factory.GetColor(0xFFFFFF);
            this.ComboBox.MouseOver.Border = factory.GetColor(0xB3C7E1);
            this.ComboBox.MouseOver.ExpandBackground = new LinearGradientColorTable(factory.GetColor(0xFFFCE2), factory.GetColor(0xFFE7A5), 90);
            this.ComboBox.MouseOver.ExpandBorderInner = new LinearGradientColorTable(factory.GetColor(0xFFFFFB), factory.GetColor(0xFFFCF3), 90);
            this.ComboBox.MouseOver.ExpandBorderOuter = new LinearGradientColorTable(factory.GetColor(0xDBCE99), Color.Empty, 90);
            this.ComboBox.MouseOver.ExpandText = factory.GetColor(0x15428B);
            this.ComboBox.DroppedDown.Background = factory.GetColor(0xFFFFFF);
            this.ComboBox.DroppedDown.Border = factory.GetColor(0xB3C7E1);
            this.ComboBox.DroppedDown.ExpandBackground = new LinearGradientColorTable(factory.GetColor(0xEAE0BF), factory.GetColor(0xFFD456), 90);
            this.ComboBox.DroppedDown.ExpandBorderInner = new LinearGradientColorTable(factory.GetColor(0xF1EBD5), factory.GetColor(0xFFE694), 90);
            this.ComboBox.DroppedDown.ExpandBorderOuter = new LinearGradientColorTable(factory.GetColor(0x9A8F63), Color.Empty, 90);
            this.ComboBox.DroppedDown.ExpandText = factory.GetColor(0x15428B);
            #endregion

            #region Dialog Launcher
            this.DialogLauncher.Default.DialogLauncher = factory.GetColor(0x668EAF);
            this.DialogLauncher.Default.DialogLauncherShade = factory.GetColor(0xFFFFFF);

            this.DialogLauncher.MouseOver.DialogLauncher = factory.GetColor(0x668EAF);
            this.DialogLauncher.MouseOver.DialogLauncherShade = Color.FromArgb(192, factory.GetColor(0xFFFFFF));
            this.DialogLauncher.MouseOver.TopBackground = new LinearGradientColorTable(factory.GetColor(0xFFFCDF), factory.GetColor(0xFFEFA7));
            this.DialogLauncher.MouseOver.BottomBackground = new LinearGradientColorTable(factory.GetColor(0xFFD975), factory.GetColor(0xFFE398));
            this.DialogLauncher.MouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(0xFFFFFB), factory.GetColor(0xFFFBF2));
            this.DialogLauncher.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(0xDBCE99), Color.Empty);

            this.DialogLauncher.Pressed.DialogLauncher = factory.GetColor(0x668EAF);
            this.DialogLauncher.Pressed.DialogLauncherShade = Color.FromArgb(192, factory.GetColor(0xFFFFFF));
            this.DialogLauncher.Pressed.TopBackground = new LinearGradientColorTable(factory.GetColor(0xE8DEBD), factory.GetColor(0xEAC68D));
            this.DialogLauncher.Pressed.BottomBackground = new LinearGradientColorTable(factory.GetColor(0xFFA738), factory.GetColor(0xFFCC4E));
            this.DialogLauncher.Pressed.InnerBorder = new LinearGradientColorTable(factory.GetColor(0xF0EAD4), factory.GetColor(0xFFE391));
            this.DialogLauncher.Pressed.OuterBorder = new LinearGradientColorTable(factory.GetColor(0x9A8F63), factory.GetColor(0xB0A472));
            #endregion

            #region System Button, Form
            // Default state no background
            this.SystemButton.Default = new Office2007SystemButtonStateColorTable();
            this.SystemButton.Default.Foreground = new LinearGradientColorTable(factory.GetColor(0x798DA7), factory.GetColor(0x8BA7CC));
            this.SystemButton.Default.LightShade = factory.GetColor(0xF8F9FA);
            this.SystemButton.Default.DarkShade = factory.GetColor(0x656565);

            // Mouse over state
            this.SystemButton.MouseOver = new Office2007SystemButtonStateColorTable();
            this.SystemButton.MouseOver.Foreground = new LinearGradientColorTable(factory.GetColor(0x798DA7), factory.GetColor(0x8BA7CC));
            this.SystemButton.MouseOver.LightShade = factory.GetColor(0xF8F9FA);
            this.SystemButton.MouseOver.DarkShade = factory.GetColor(0x656565);
            this.SystemButton.MouseOver.TopBackground = new LinearGradientColorTable(factory.GetColor(0xF5F8FE), factory.GetColor(0xEAF2FF));
            this.SystemButton.MouseOver.BottomBackground = new LinearGradientColorTable(factory.GetColor(0xD2E4FE), factory.GetColor(0xD1E2FB));
            this.SystemButton.MouseOver.TopHighlight = new LinearGradientColorTable(factory.GetColor(0xFBFCFF), Color.Transparent);
            this.SystemButton.MouseOver.BottomHighlight = new LinearGradientColorTable(factory.GetColor(0xFBFCFF), Color.Transparent);
            this.SystemButton.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(0xC1D4EE), factory.GetColor(0xAAC4E9));
            this.SystemButton.MouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(0xFEFEFF), factory.GetColor(0xE6F4FE));

            // Pressed
            this.SystemButton.Pressed = new Office2007SystemButtonStateColorTable();
            this.SystemButton.Pressed.Foreground = new LinearGradientColorTable(factory.GetColor(0x798DA7), factory.GetColor(0x8BA7CC));
            this.SystemButton.Pressed.LightShade = factory.GetColor(0xF8F9FA);
            this.SystemButton.Pressed.DarkShade = factory.GetColor(0x656565);
            this.SystemButton.Pressed.TopBackground = new LinearGradientColorTable(factory.GetColor(0xBAD5F8), factory.GetColor(0x9EC2ED));
            this.SystemButton.Pressed.TopHighlight = new LinearGradientColorTable(factory.GetColor(0xB8CEE9), Color.Transparent);
            this.SystemButton.Pressed.BottomBackground = new LinearGradientColorTable(factory.GetColor(0x84B2E9), factory.GetColor(0xAFD6F7));
            this.SystemButton.Pressed.BottomHighlight = new LinearGradientColorTable(factory.GetColor(0xC6EAFD), Color.Transparent);
            this.SystemButton.Pressed.OuterBorder = new LinearGradientColorTable(factory.GetColor(0xA1BFE5), factory.GetColor(0xB2CAE7));
            this.SystemButton.Pressed.InnerBorder = new LinearGradientColorTable(factory.GetColor(0xD3E6FF), factory.GetColor(0xDCE9FB));

            // Form border
            this.Form.Active.BorderColors = new Color[] {
                factory.GetColor(0x3B5A82),
                factory.GetColor(0xB1C6E1),
                factory.GetColor(0xC2D9F7),
                factory.GetColor(0xBFDBFF),
                factory.GetColor(0xBFDBFF)};

            this.Form.Inactive.BorderColors = new Color[] {
                factory.GetColor(0xC0C6CE),
                factory.GetColor(0xCCD6E2),
                factory.GetColor(0xD4DEEC),
                factory.GetColor(0xBFDBFF),
                factory.GetColor(0xBFDBFF)};

            // Form Caption Active
            this.Form.Active.CaptionTopBackground = new LinearGradientColorTable(factory.GetColor(0xE4EBF6), factory.GetColor(0xDAE9FD));
            this.Form.Active.CaptionBottomBackground = new LinearGradientColorTable(factory.GetColor(0xCADEF7), factory.GetColor(0xE4EFFD));
            this.Form.Active.CaptionBottomBorder = new Color[] { factory.GetColor(0xDCF4FE), factory.GetColor(0xB0CFF7) };
            this.Form.Active.CaptionText = factory.GetColor(0x3E6AAA);
            this.Form.Active.CaptionTextExtra = factory.GetColor(0x697079);

            // Form Caption Inactive
            this.Form.Inactive.CaptionTopBackground = new LinearGradientColorTable(factory.GetColor(0xE3E7EC), factory.GetColor(0xDEE5ED));
            this.Form.Inactive.CaptionBottomBackground = new LinearGradientColorTable(factory.GetColor(0xD8E1EC), factory.GetColor(0xE3E8EF));
            this.Form.Inactive.CaptionText = factory.GetColor(0xA0A0A0);
            this.Form.Inactive.CaptionTextExtra = factory.GetColor(0xA0A0A0);

            this.Form.BackColor = factory.GetColor(0xC2D9F7);
            this.Form.TextColor = factory.GetColor(0x15428B);
            #endregion

            #region Quick Access Toolbar Background
            this.QuickAccessToolbar.Active.TopBackground = new LinearGradientColorTable(factory.GetColor(0xDEE7F4), factory.GetColor(0xE6EEF9));
            this.QuickAccessToolbar.Active.BottomBackground = new LinearGradientColorTable(factory.GetColor(0xDBE7F7), factory.GetColor(0xC9D9EE));
            this.QuickAccessToolbar.Active.OutterBorderColor = factory.GetColor(0xF6F9FC);
            this.QuickAccessToolbar.Active.MiddleBorderColor = factory.GetColor(0x9AB3D5);
            this.QuickAccessToolbar.Active.InnerBorderColor = Color.Empty; //  factory.GetColor(0xD2E3F9);

            this.QuickAccessToolbar.Inactive.TopBackground = new LinearGradientColorTable(factory.GetColor(0xE6ECF3), factory.GetColor(0xCED8E6));
            this.QuickAccessToolbar.Inactive.BottomBackground = new LinearGradientColorTable(factory.GetColor(0xCED8E6), factory.GetColor(0xC8D3E3));
            this.QuickAccessToolbar.Inactive.OutterBorderColor = factory.GetColor(0xF6F9FC);
            this.QuickAccessToolbar.Inactive.MiddleBorderColor = factory.GetColor(0x9AB3D5);
            this.QuickAccessToolbar.Inactive.InnerBorderColor = Color.Empty;

            this.QuickAccessToolbar.Standalone.TopBackground = new LinearGradientColorTable();
            this.QuickAccessToolbar.Standalone.BottomBackground = new LinearGradientColorTable(factory.GetColor(0xB2CDED), factory.GetColor(0xAAC5EA));
            this.QuickAccessToolbar.Standalone.OutterBorderColor = factory.GetColor(0x7EA1CD);
            this.QuickAccessToolbar.Standalone.MiddleBorderColor = Color.Empty;
            this.QuickAccessToolbar.Standalone.InnerBorderColor = factory.GetColor(0xDCE8F7);

            this.QuickAccessToolbar.QatCustomizeMenuLabelBackground = factory.GetColor(0xDDE7EE);
            this.QuickAccessToolbar.QatCustomizeMenuLabelText = factory.GetColor(0x00156E);

            this.QuickAccessToolbar.Active.GlassBorder = new LinearGradientColorTable(factory.GetColor(Color.FromArgb(132, Color.Black)), Color.FromArgb(80, Color.Black));
            this.QuickAccessToolbar.Inactive.GlassBorder = new LinearGradientColorTable(factory.GetColor(Color.FromArgb(132, Color.Black)), Color.FromArgb(80, Color.Black));
            #endregion

            #region Tab Colors
            this.TabControl.Default = new Office2007TabItemStateColorTable();
            this.TabControl.Default.TopBackground = new LinearGradientColorTable(factory.GetColor(0xD7E6F9), factory.GetColor(0xC7DCF8));
            this.TabControl.Default.BottomBackground = new LinearGradientColorTable(factory.GetColor(0xB3D0F5), factory.GetColor(0xD7E5F7));
            this.TabControl.Default.InnerBorder = factory.GetColor(0xF3F7FD);
            this.TabControl.Default.OuterBorder = factory.GetColor(0x92A5C7);
            this.TabControl.Default.Text = factory.GetColor(0x154A93);

            this.TabControl.MouseOver = new Office2007TabItemStateColorTable();
            this.TabControl.MouseOver.TopBackground = new LinearGradientColorTable(factory.GetColor(0xFFFDEB), factory.GetColor(0xFFECA8));
            this.TabControl.MouseOver.BottomBackground = new LinearGradientColorTable(factory.GetColor(0xFFDA59), factory.GetColor(0xFFE68D));
            this.TabControl.MouseOver.InnerBorder = factory.GetColor(0xFFFFFB);
            this.TabControl.MouseOver.OuterBorder = factory.GetColor(0xB69D73);
            this.TabControl.MouseOver.Text = factory.GetColor(0x154A93);

            this.TabControl.Selected = new Office2007TabItemStateColorTable();
            //this.TabControl.Selected.TopBackground = new LinearGradientColorTable(factory.GetColor(0xFFD29B), factory.GetColor(0xFFBB6E));
            this.TabControl.Selected.TopBackground = new LinearGradientColorTable(factory.GetColor(Color.White), factory.GetColor(0xFDFDFE));
            //this.TabControl.Selected.BottomBackground = new LinearGradientColorTable(factory.GetColor(0xFFAF44), factory.GetColor(0xFEDC75));
            this.TabControl.Selected.BottomBackground = new LinearGradientColorTable(factory.GetColor(0xFDFDFE), factory.GetColor(0xFDFDFE));
            //this.TabControl.Selected.InnerBorder = factory.GetColor(0xCDB69C);
            this.TabControl.Selected.InnerBorder = factory.GetColor(Color.White);
            //this.TabControl.Selected.OuterBorder = factory.GetColor(0x95774A);
            this.TabControl.Selected.OuterBorder = factory.GetColor(0x92A5C7);
            this.TabControl.Selected.Text = factory.GetColor(0x154A93);

            this.TabControl.TabBackground = new LinearGradientColorTable(factory.GetColor(0xE3EFFF), factory.GetColor(0xB0D2FF));
            this.TabControl.TabPanelBackground = new LinearGradientColorTable(factory.GetColor(0xFDFDFE), factory.GetColor(0x9DBCE3));
            this.TabControl.TabPanelBorder = factory.GetColor(0x92A5C7);
            #endregion

            #region CheckBoxItem
            Office2007CheckBoxColorTable chk = this.CheckBoxItem;
            chk.Default.CheckBackground = new LinearGradientColorTable(factory.GetColor(0xF4F4F4), Color.Empty);
            chk.Default.CheckBorder = factory.GetColor(0xABC1DE);
            chk.Default.CheckInnerBackground = new LinearGradientColorTable(Color.FromArgb(192, factory.GetColor(0xA2ACB9)), Color.FromArgb(164, factory.GetColor(0xF6F6F6)));
            chk.Default.CheckInnerBorder = factory.GetColor(0xA2ACB9);
            chk.Default.CheckSign = new LinearGradientColorTable(factory.GetColor(0x4A6B96), Color.Empty);
            chk.Default.Text = factory.GetColor(0x15428B);

            chk.MouseOver.CheckBackground = new LinearGradientColorTable(factory.GetColor(0xDEEAFA), Color.Empty);
            chk.MouseOver.CheckBorder = factory.GetColor(0x5577A3);
            chk.MouseOver.CheckInnerBackground = new LinearGradientColorTable(Color.FromArgb(192, factory.GetColor(0xFAD57A)), Color.FromArgb(128, factory.GetColor(0xFEF8E7)));
            chk.MouseOver.CheckInnerBorder = factory.GetColor(0xFAD57A);
            chk.MouseOver.CheckSign = new LinearGradientColorTable(factory.GetColor(0x4A6B96), Color.Empty);
            chk.MouseOver.Text = factory.GetColor(0x15428B);

            chk.Pressed.CheckBackground = new LinearGradientColorTable(factory.GetColor(0xC1D8F5), Color.Empty);
            chk.Pressed.CheckBorder = factory.GetColor(0x5577A3);
            chk.Pressed.CheckInnerBackground = new LinearGradientColorTable(Color.FromArgb(192, factory.GetColor(0xF28926)), Color.FromArgb(164, factory.GetColor(0xFFF4D5)));
            chk.Pressed.CheckInnerBorder = factory.GetColor(0xF28926);
            chk.Pressed.CheckSign = new LinearGradientColorTable(factory.GetColor(0x4A6B96), Color.Empty);
            chk.Pressed.Text = factory.GetColor(0x15428B);

            chk.Disabled.CheckBackground = new LinearGradientColorTable(factory.GetColor(0xFFFFFF), Color.Empty);
            chk.Disabled.CheckBorder = factory.GetColor(0xAEB1B5);
            chk.Disabled.CheckInnerBackground = new LinearGradientColorTable(Color.FromArgb(192, factory.GetColor(0xE0E2E5)), Color.FromArgb(164, factory.GetColor(0xFBFBFB)));
            chk.Disabled.CheckInnerBorder = factory.GetColor(0xE0E2E5);
            chk.Disabled.CheckSign = new LinearGradientColorTable(factory.GetColor(0x8D8D8D), Color.Empty);
            chk.Disabled.Text = factory.GetColor(0x8D8D8D);
            #endregion

            #region Scroll Bar Colors
            Office2007ColorTableFactory.InitializeScrollBarColorTable(this, factory);
            Office2007ColorTableFactory.InitializeAppBlueScrollBarColorTable(this, factory);
            #endregion

            #region ProgressBarItem
            Office2007ProgressBarColorTable pct = this.ProgressBarItem;
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
            pct = this.ProgressBarItemPaused;
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
            pct = this.ProgressBarItemError;
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
            Office2007GalleryColorTable gallery = this.Gallery;
            gallery.GroupLabelBackground = factory.GetColor(0xDDE7EE);
            gallery.GroupLabelText = factory.GetColor(0x00156E);
            gallery.GroupLabelBorder = factory.GetColor(0xC5C5C5);
            #endregion

            #region Legacy Colors
            this.LegacyColors.BarBackground = factory.GetColor(0xE3EFFF);
            this.LegacyColors.BarBackground2 = factory.GetColor(0xA9CEFE);
            this.LegacyColors.BarStripeColor = factory.GetColor(0x6F9DD9);
            this.LegacyColors.BarCaptionBackground = factory.GetColor(0x6593CF);
            this.LegacyColors.BarCaptionBackground2 = factory.GetColor(0x3764A0);
            this.LegacyColors.BarCaptionInactiveBackground = factory.GetColor(0xE3EFFF);
            this.LegacyColors.BarCaptionInactiveBackground2 = factory.GetColor(0xAFD2FF);
            this.LegacyColors.BarCaptionInactiveText = factory.GetColor(0x083772);
            this.LegacyColors.BarCaptionText = factory.GetColor(0xFFFFFF);
            this.LegacyColors.BarFloatingBorder = factory.GetColor(0x3764A0);
            this.LegacyColors.BarPopupBackground = factory.GetColor(0xF6F6F6);
            this.LegacyColors.BarPopupBorder = factory.GetColor(0x6593CF);
            this.LegacyColors.ItemBackground = Color.Empty;
            this.LegacyColors.ItemBackground2 = Color.Empty;
            this.LegacyColors.ItemCheckedBackground = factory.GetColor(0xFCD578);
            this.LegacyColors.ItemCheckedBackground2 = factory.GetColor(0xFBC84F);
            this.LegacyColors.ItemCheckedBorder = factory.GetColor(0xBB5503);
            this.LegacyColors.ItemCheckedText = factory.GetColor(0x000000);
            this.LegacyColors.ItemDisabledBackground = Color.Empty;
            this.LegacyColors.ItemDisabledText = factory.GetColor(0x8D8D8D);
            this.LegacyColors.ItemExpandedShadow = Color.Empty;
            this.LegacyColors.ItemExpandedBackground = factory.GetColor(0xE3EFFE);
            this.LegacyColors.ItemExpandedBackground2 = factory.GetColor(0x99BFF0);
            this.LegacyColors.ItemExpandedText = factory.GetColor(0x000000);
            this.LegacyColors.ItemHotBackground = factory.GetColor(0xFFF5CC);
            this.LegacyColors.ItemHotBackground2 = factory.GetColor(0xFFDB75);
            this.LegacyColors.ItemHotBorder = factory.GetColor(0xFFBD69);
            this.LegacyColors.ItemHotText = factory.GetColor(0x000000);
            this.LegacyColors.ItemPressedBackground = factory.GetColor(0xFC973D);
            this.LegacyColors.ItemPressedBackground2 = factory.GetColor(0xFFB85E);
            this.LegacyColors.ItemPressedBorder = factory.GetColor(0xFB8C3C);
            this.LegacyColors.ItemPressedText = factory.GetColor(0x000000);
            this.LegacyColors.ItemSeparator = Color.FromArgb(80, factory.GetColor(0x9AC6FF));
            this.LegacyColors.ItemSeparatorShade = Color.FromArgb(250, factory.GetColor(0xFFFFFF));
            this.LegacyColors.ItemText = factory.GetColor(0x000000); // SystemColors.ControlTet;
            this.LegacyColors.MenuBackground = factory.GetColor(0xF6F6F6);
            this.LegacyColors.MenuBackground2 = Color.Empty; // Color.White;
            this.LegacyColors.MenuBarBackground = factory.GetColor(0xBFDBFF);
            this.LegacyColors.MenuBorder = factory.GetColor(0x6593CF);
            this.LegacyColors.ItemExpandedBorder = this.LegacyColors.MenuBorder;
            this.LegacyColors.MenuSide = factory.GetColor(0xE9EEEE);
            this.LegacyColors.MenuSide2 = Color.Empty; // factory.GetColor(0xDDE0E8);
            this.LegacyColors.MenuUnusedBackground = this.LegacyColors.MenuBackground;
            this.LegacyColors.MenuUnusedSide = factory.GetColor(0xDADADA);
            this.LegacyColors.MenuUnusedSide2 = Color.Empty;// System.Windows.Forms.ControlPaint.Light(this.LegacyColors.MenuSide2);
            this.LegacyColors.ItemDesignTimeBorder = Color.Black;
            this.LegacyColors.BarDockedBorder = factory.GetColor(0x6F9DD9);
            this.LegacyColors.DockSiteBackColor = factory.GetColor(0xBFDBFF);
            this.LegacyColors.DockSiteBackColor2 = Color.Empty;
            this.LegacyColors.CustomizeBackground = factory.GetColor(0xD7E8FF);
            this.LegacyColors.CustomizeBackground2 = factory.GetColor(0x6F9DD9);
            this.LegacyColors.CustomizeText = factory.GetColor(0x000000);
            this.LegacyColors.PanelBackground = factory.GetColor(0xE3EFFF);
            this.LegacyColors.PanelBackground2 = factory.GetColor(0xAFD2FF);
            this.LegacyColors.PanelText = factory.GetColor(0x083772);
            this.LegacyColors.PanelBorder = factory.GetColor(0x6593CF);
            this.LegacyColors.ExplorerBarBackground = factory.GetColor(0xC4C8D4);
            this.LegacyColors.ExplorerBarBackground2 = factory.GetColor(0xB1B3C8);
            #endregion

            #region Navigation Pane
            this.NavigationPane.ButtonBackground = new GradientColorTable();
            this.NavigationPane.ButtonBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xE3EFFF), 0));
            this.NavigationPane.ButtonBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xC4DDFF), .4f));
            this.NavigationPane.ButtonBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xADD1FF), .4f));
            this.NavigationPane.ButtonBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xC0DBFF), 1));
            #endregion

            #region SuperTooltip
            this.SuperTooltip.BackgroundColors = new LinearGradientColorTable(factory.GetColor(0xFFFFFF), factory.GetColor(0xC9D9EF));
            this.SuperTooltip.TextColor = factory.GetColor(0x4C4C4C);
            #endregion

            #region Slider
            Office2007SliderColorTable sl = this.Slider;
            sl.Default.LabelColor = factory.GetColor(0x0A207D);
            sl.Default.SliderLabelColor = factory.GetColor(0x1E395B);
            sl.Default.PartBackground = new GradientColorTable();
            sl.Default.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xFFFFFF), 0));
            sl.Default.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xF1F6FC), .15f));
            sl.Default.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xAEC9EE), .5f));
            sl.Default.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(0x6A98D0), .5f));
            sl.Default.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xFFFFFF), .85f));
            sl.Default.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xFFFFFF), 1f));
            sl.Default.PartBorderColor = factory.GetColor(0x2F578D);
            sl.Default.PartBorderLightColor = Color.FromArgb(96,factory.GetColor(0xFFFFFF));
            sl.Default.PartForeColor = factory.GetColor(0x566375);
            sl.Default.PartForeLightColor = Color.FromArgb(168,factory.GetColor(0xE7F0F9));
            sl.Default.TrackLineColor = factory.GetColor(0x7496C2);
            sl.Default.TrackLineLightColor = factory.GetColor(0xDEE2EC);

            sl.MouseOver.LabelColor = factory.GetColor(0x092061);
            sl.MouseOver.SliderLabelColor = factory.GetColor(0x1E395B);
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
            sl.Pressed.SliderLabelColor = factory.GetColor(0x1E395B);
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
            this.ListViewEx.Border = factory.GetColor(0xB3C7E1);
            this.ListViewEx.ColumnBackground = new LinearGradientColorTable(factory.GetColor(0xFFFFFF), factory.GetColor(0xC4DDFF));
            this.ListViewEx.ColumnSeparator = factory.GetColor(0x9AC6FF);
            this.ListViewEx.SelectionBackground = new LinearGradientColorTable(factory.GetColor(0xA7CDF0), Color.Empty);
            this.ListViewEx.SelectionBorder = factory.GetColor(0xE3EFFF);
            #endregion

            #region DataGridView
            this.DataGridView.ColumnHeaderNormalBorder = factory.GetColor(0x9EB6CE);
            this.DataGridView.ColumnHeaderNormalBackground = new LinearGradientColorTable(factory.GetColor(0xF9FCFD), factory.GetColor(0xD3DBE9), 90);
            this.DataGridView.ColumnHeaderSelectedBackground = new LinearGradientColorTable(factory.GetColor(0xF9D99F), factory.GetColor(0xF1C15F), 90);
            this.DataGridView.ColumnHeaderSelectedBorder = factory.GetColor(0xF29536);
            this.DataGridView.ColumnHeaderSelectedMouseOverBackground = new LinearGradientColorTable(factory.GetColor(0xFFD58D), factory.GetColor(0xF2923A), 90);
            this.DataGridView.ColumnHeaderSelectedMouseOverBorder = factory.GetColor(0xF29536);
            this.DataGridView.ColumnHeaderMouseOverBackground = new LinearGradientColorTable(factory.GetColor(0xDFE2E4), factory.GetColor(0xBCC5D2), 90);
            this.DataGridView.ColumnHeaderMouseOverBorder = factory.GetColor(0x879FB7);
            this.DataGridView.ColumnHeaderPressedBackground = new LinearGradientColorTable(factory.GetColor(0xBCC5D2), factory.GetColor(0xDFE2E4), 90);
            this.DataGridView.ColumnHeaderPressedBorder = factory.GetColor(0xFFFFFF);

            this.DataGridView.RowNormalBackground = new LinearGradientColorTable(factory.GetColor(0xE4ECF7));
            this.DataGridView.RowNormalBorder = factory.GetColor(0x9EB6CE);
            this.DataGridView.RowSelectedBackground = new LinearGradientColorTable(factory.GetColor(0xFFD58D));
            this.DataGridView.RowSelectedBorder = factory.GetColor(0xF29536);
            this.DataGridView.RowSelectedMouseOverBackground = new LinearGradientColorTable(factory.GetColor(0xF1C05C));
            this.DataGridView.RowSelectedMouseOverBorder = factory.GetColor(0xF29536);
            this.DataGridView.RowMouseOverBackground = new LinearGradientColorTable(factory.GetColor(0xF1C05C));
            this.DataGridView.RowMouseOverBorder = factory.GetColor(0xF29536);
            this.DataGridView.RowPressedBackground = new LinearGradientColorTable(factory.GetColor(0xBBC4D1));
            this.DataGridView.RowPressedBorder = factory.GetColor(0xFFFFFF);

            this.DataGridView.GridColor = factory.GetColor(0xD0D7E5);

            this.DataGridView.SelectorBackground = new LinearGradientColorTable(factory.GetColor(0xA9C4E9));
            this.DataGridView.SelectorBorder = factory.GetColor(0x9EB6CE);
            this.DataGridView.SelectorBorderDark = factory.GetColor(0xB0CFF7);
            this.DataGridView.SelectorBorderLight = factory.GetColor(0xD5E4F2);
            this.DataGridView.SelectorSign = new LinearGradientColorTable(factory.GetColor(0xF9FAFB), factory.GetColor(0xD7DAE2));

            this.DataGridView.SelectorMouseOverBackground = new LinearGradientColorTable(factory.GetColor(0x8BA0B5));
            this.DataGridView.SelectorMouseOverBorder = factory.GetColor(0x9EB6CE);
            this.DataGridView.SelectorMouseOverBorderDark = factory.GetColor(0xB0CFF7);
            this.DataGridView.SelectorMouseOverBorderLight = factory.GetColor(0xD5E4F2);
            this.DataGridView.SelectorMouseOverSign = new LinearGradientColorTable(factory.GetColor(0xF9FAFB), factory.GetColor(0xD7DAE2));
            #endregion

            #region SideBar
            this.SideBar.Background = new LinearGradientColorTable(factory.GetColor(Color.White));
            this.SideBar.Border = factory.GetColor(0x6593CF);
            this.SideBar.SideBarPanelItemText = factory.GetColor(0x15428B);
            this.SideBar.SideBarPanelItemDefault = new GradientColorTable();
            this.SideBar.SideBarPanelItemDefault.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xE3EFFF), 0));
            this.SideBar.SideBarPanelItemDefault.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xC4DDFF), .4f));
            this.SideBar.SideBarPanelItemDefault.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xADD1FF), .4f));
            this.SideBar.SideBarPanelItemDefault.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xC0DBFF), 1));
            // Expanded
            this.SideBar.SideBarPanelItemExpanded = new GradientColorTable();
            this.SideBar.SideBarPanelItemExpanded.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xFBDBB5), 0));
            this.SideBar.SideBarPanelItemExpanded.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xFEC778), .4f));
            this.SideBar.SideBarPanelItemExpanded.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xFEB456), .4f));
            this.SideBar.SideBarPanelItemExpanded.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xFDEB9F), 1));
            // MouseOver
            this.SideBar.SideBarPanelItemMouseOver = new GradientColorTable();
            this.SideBar.SideBarPanelItemMouseOver.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xFFFCD9), 0));
            this.SideBar.SideBarPanelItemMouseOver.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xFFE78D), .4f));
            this.SideBar.SideBarPanelItemMouseOver.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xFFD748), .4f));
            this.SideBar.SideBarPanelItemMouseOver.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xFFE793), 1));
            // Pressed
            this.SideBar.SideBarPanelItemPressed = new GradientColorTable();
            this.SideBar.SideBarPanelItemPressed.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xF8B869), 0));
            this.SideBar.SideBarPanelItemPressed.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xFDA361), .4f));
            this.SideBar.SideBarPanelItemPressed.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xFB8A3C), .4f));
            this.SideBar.SideBarPanelItemPressed.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xFEBB60), 1));
            #endregion

            #region AdvTree
            #if !NOTREE
            this.AdvTree = new DevComponents.AdvTree.Display.TreeColorTable();
            DevComponents.AdvTree.Display.ColorTableInitializer.InitOffice2007Blue(this.AdvTree, factory);
            #endif
            #endregion

            #region CrumbBar
            this.CrumbBarItemView = new CrumbBarItemViewColorTable();
            CrumbBarItemViewStateColorTable crumbBarViewTable = new CrumbBarItemViewStateColorTable();
            this.CrumbBarItemView.Default = crumbBarViewTable;
            crumbBarViewTable.Foreground = factory.GetColor(0x15428B);
            crumbBarViewTable = new CrumbBarItemViewStateColorTable();
            this.CrumbBarItemView.MouseOver = crumbBarViewTable;
            crumbBarViewTable.Foreground = factory.GetColor(0x15428B);
            crumbBarViewTable.Background=new BackgroundColorBlendCollection();
            crumbBarViewTable.Background.AddRange(new BackgroundColorBlend[]{
                new BackgroundColorBlend(factory.GetColor("FFFCD9"), 0f),
                new BackgroundColorBlend(factory.GetColor("FFE78D"), .4f),
                new BackgroundColorBlend(factory.GetColor("FFD748"), .4f),
                new BackgroundColorBlend(factory.GetColor("FFE793"), 1f)});
            crumbBarViewTable.Border = factory.GetColor("FFB8A98E");
            crumbBarViewTable.BorderLight = factory.GetColor("90FFFFFF");
            crumbBarViewTable = new CrumbBarItemViewStateColorTable();
            this.CrumbBarItemView.MouseOverInactive = crumbBarViewTable;
            crumbBarViewTable.Foreground = factory.GetColor(0x15428B);
            crumbBarViewTable.Background = new BackgroundColorBlendCollection();
            crumbBarViewTable.Background.AddRange(new BackgroundColorBlend[]{
                new BackgroundColorBlend(factory.GetColor("FFFFFDEC"), 0f),
                new BackgroundColorBlend(factory.GetColor("FFFFF4CA"), .4f),
                new BackgroundColorBlend(factory.GetColor("FFFFEBA6"), .4f),
                new BackgroundColorBlend(factory.GetColor("FFFFF2C5"), 1f)});
            crumbBarViewTable.Border = factory.GetColor("FF8E8F8F");
            crumbBarViewTable.BorderLight = factory.GetColor("90FFFFFF");
            crumbBarViewTable = new CrumbBarItemViewStateColorTable();
            this.CrumbBarItemView.Pressed = crumbBarViewTable;
            crumbBarViewTable.Foreground = factory.GetColor(0x15428B);
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
            this.WarningBox.BackColor = factory.GetColor(Color.FromArgb(255, 196, 219, 249));
            this.WarningBox.WarningBorderColor = factory.GetColor(Color.FromArgb(255, 162, 188, 213));
            this.WarningBox.WarningBackColor1 = factory.GetColor(Color.FromArgb(255, 255, 255, 255));
            this.WarningBox.WarningBackColor2 = factory.GetColor(Color.FromArgb(255, 229, 244, 254));
            #endregion

            #region CalendarView

            #region WeekDayViewColors

            this.CalendarView.WeekDayViewColors = new ColorDef[]
            {
                new ColorDef(factory.GetColor(0x5D8CC9)),           // DayViewBorder
                new ColorDef(factory.GetColor(0x000000)),           // DayHeaderForeground - 0x15428B

                new ColorDef(new Color[] {factory.GetColor(0xE4ECF6), factory.GetColor(0xD6E2F1), factory.GetColor(0xC2D4EB), factory.GetColor(0xD0DEEF)},
                new float[] {0f, .55f, .58f, 1f}, 90f),             // DayHeaderBackground

                new ColorDef(factory.GetColor(0x8DAED9)),           // DayHeaderBorder

                new ColorDef(factory.GetColor(0xFFFFFF)),           // DayWorkHoursBackground
                new ColorDef(factory.GetColor(0x8DAED9)),           // DayAllDayEventBackground
                new ColorDef(factory.GetColor(0xE6EDF7)),           // DayOffWorkHoursBackground

                new ColorDef(factory.GetColor(0xA5BFE1)),           // DayHourBorder
                new ColorDef(factory.GetColor(0xD5E1F1)),           // DayHalfHourBorder

                new ColorDef(factory.GetColor(0x294C7A)),           // SelectionBackground
                
                new ColorDef(factory.GetColor(0x5D8CC9)),           // OwnerTabBorder

                new ColorDef(new Color[] {factory.GetColor(0xBBCFE9), factory.GetColor(0x8DAED9)},
                new float[] {0f, 1f}, 90f),                         // OwnerTabBackground

                new ColorDef(factory.GetColor(0x000000)),           // OwnerTabForeground
                new ColorDef(factory.GetColor(0x8DAED9)),           // OwnerTabContentBackground
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

            #region TimeRulerColors

            this.CalendarView.TimeRulerColors = new ColorDef[]
            {
                new ColorDef(factory.GetColor(0xE3EFFF)),           // TimeRulerBackground
                new ColorDef(factory.GetColor(0x6593CF)),           // TimeRulerForeground
                new ColorDef(factory.GetColor(0x6593CF)),           // TimeRulerBorder
                new ColorDef(factory.GetColor(0x6593CF)),           // TimeRulerTickBorder

                new ColorDef(new Color[] {factory.GetColor(0xFFED79), factory.GetColor(0xFFD86B), factory.GetColor(0xFFBB00), factory.GetColor(0xFFEA77)},
                new float[] {0f, .55f ,58f, 1f}, 90f),              // TimeRulerIndicator

                new ColorDef(factory.GetColor(0xEB8900)),           // TimeRulerIndicatorBorder
            };

            #endregion

            #region MonthViewColors

            this.CalendarView.MonthViewColors = new ColorDef[]
            {
                new ColorDef(factory.GetColor(0x8DAED9)),           // DayOfWeekHeaderBorder

                new ColorDef(new Color[] { factory.GetColor(0xE1E9F4), factory.GetColor(0xD6E2F1), factory.GetColor(0xC2D4EB), factory.GetColor(0xD0DEEF) },
                new float[] { 0, .6f, .6f, 1 }),                    // DayOfWeekHeaderBackground

                new ColorDef(factory.GetColor(0x000000)),           // DayOfWeekHeaderForeground - 0x15428B
                new ColorDef(factory.GetColor(0x5D8CC9)),           // SideBarBorder

                new ColorDef(new Color[] { factory.GetColor(0xE1E9F4), factory.GetColor(0xD6E2F1), factory.GetColor(0xC2D4EB), factory.GetColor(0xD0DEEF) },
                new float[] { 0, .6f, .6f, 1 }, 0),                 // SideBarBackground

                new ColorDef(factory.GetColor(0x000000)),           // SideBarForeground - 0x15428B
                new ColorDef(factory.GetColor(0x5D8CC9)),           // DayHeaderBorder

                new ColorDef(new Color[] { factory.GetColor(0xE1E9F4), factory.GetColor(0xD6E2F1), factory.GetColor(0xC2D4EB), factory.GetColor(0xD0DEEF) },
                new float[] { 0, .6f, .6f, 1 }),                    // DayHeaderBackground

                new ColorDef(factory.GetColor(0x000000)),           // DayHeaderForeground
                new ColorDef(factory.GetColor(0x8DAED9)),           // DayContentBorder
                new ColorDef(factory.GetColor(0xE6EDF7)),           // DayContentSelectionBackground
                new ColorDef(factory.GetColor(0xFFFFFF)),           // DayContentActiveDayBackground
                new ColorDef(factory.GetColor(0xA5BFE1)),           // DayContentInactiveDayBackground
                
                new ColorDef(factory.GetColor(0x5D8CC9)),           // OwnerTabBorder

                new ColorDef(new Color[] {factory.GetColor(0xBBCFE9), factory.GetColor(0x8DAED9)},    // OwnerTabBackground
                new float[] {0f, 1f}, 90f),

                new ColorDef(factory.GetColor(0x000000)),           // OwnerTabForeground
                new ColorDef(factory.GetColor(0x8DAED9)),           // OwnerTabContentBackground
                new ColorDef(factory.GetColor(0x000000)),           // OwnerTabSelectedForeground
                new ColorDef(factory.GetColor(0xFFFFFF)),           // OwnerTabSelectionBackground

                new ColorDef(factory.GetColor(0xEB8900)),           // NowDayViewBorder
                new ColorDef(factory.GetColor(0x000000)),           // NowDayHeaderForeground - 0x15428B

                new ColorDef(new Color[] {factory.GetColor(0xFFED79), factory.GetColor(0xFFD86B), factory.GetColor(0xFFBB00), factory.GetColor(0xFFEA77)},
                new float[] {0f, .55f ,58f, 1f}, 90f),              // NowDayHeaderBackground
            };

            #endregion

            #region AppointmentColors

            this.CalendarView.AppointmentColors = new ColorDef[]
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
            SuperTab = Office2007ColorTableFactory.GetSuperTabBlueDefault(factory);
            #endregion

            #region SuperTabItem
            SuperTabItem = Office2007ColorTableFactory.GetSuperTabItemBlueDefault(factory);
            #endregion

            #region SuperTabPanel
            SuperTabPanel = Office2007ColorTableFactory.GetSuperTabPanelBlueDefault(factory);
            #endregion

            #endregion

            #region Backstage

            #region Backstage
            Backstage = Office2007ColorTableFactory.GetBackstageBlueDefault(factory);
            #endregion

            #region BackstageItem
            BackstageItem = Office2007ColorTableFactory.GetBackstageItemBlueDefault(factory);
            #endregion

            #region BackstagePanel
            BackstagePanel = Office2007ColorTableFactory.GetBackstagePanelBlueDefault(factory);
            #endregion

            #endregion

            #region SwitchButton
            SwitchButtonColorTable sbt = new SwitchButtonColorTable();
            sbt.BorderColor = factory.GetColor(0xABC1DE);
            sbt.OffBackColor = factory.GetColor(0xDAE5F3);
            sbt.OffTextColor = factory.GetColor(0x15428B);
            sbt.OnBackColor = factory.GetColor(0x92D050);
            sbt.OnTextColor = factory.GetColor(0x15428B);
            sbt.SwitchBackColor = factory.GetColor(0xC1D8F0);
            sbt.SwitchBorderColor = factory.GetColor(0x86A5CF);
            sbt.TextColor = factory.GetColor(0x15428B);
            this.SwitchButton = new SwitchButtonColors();
            this.SwitchButton.Default = sbt;
            this.SwitchButton.Disabled.BorderColor = this.CheckBoxItem.Disabled.CheckBorder;
            this.SwitchButton.Disabled.SwitchBorderColor = this.SwitchButton.Disabled.BorderColor;
            this.SwitchButton.Disabled.OffTextColor = this.CheckBoxItem.Disabled.Text;
            this.SwitchButton.Disabled.OnTextColor = this.SwitchButton.Disabled.OffTextColor;
            this.SwitchButton.Disabled.TextColor = this.SwitchButton.Disabled.OffTextColor;
            this.SwitchButton.Disabled.SwitchBackColor = this.CheckBoxItem.Disabled.CheckInnerBackground.Start;
            this.SwitchButton.Disabled.OffBackColor = this.CheckBoxItem.Disabled.CheckInnerBackground.Start;
            this.SwitchButton.Disabled.OnBackColor = this.SwitchButton.Disabled.OffBackColor;
            #endregion

            #region ElementStyle Classes
            ElementStyle style = new ElementStyle();
            style.Class = ElementStyleClassKeys.RibbonGalleryContainerKey;
            style.BorderColor = factory.GetColor(0xB9D0ED);
            style.Border = eStyleBorderType.Solid;
            style.BorderWidth = 1;
            style.CornerDiameter = 2;
            style.CornerType = eCornerType.Rounded;
            style.BackColor = factory.GetColor(0xD4E6F8);
            m_StyleClasses.Add(style.Class, style);
            // FileMenuContainer
            style = Office2007ColorTableFactory.GetFileMenuContainerStyle(this);
            m_StyleClasses.Add(style.Class, style);
            // Two Column File Menu Container
            style = Office2007ColorTableFactory.GetTwoColumnMenuContainerStyle(this);
            m_StyleClasses.Add(style.Class, style);
            // Column one File Menu Container
            style = Office2007ColorTableFactory.GetMenuColumnOneContainerStyle(this);
            m_StyleClasses.Add(style.Class, style);
            // Column two File Menu Container
            style = Office2007ColorTableFactory.GetMenuColumnTwoContainerStyle(this);
            m_StyleClasses.Add(style.Class, style);
            // Bottom File Menu Container
            style = Office2007ColorTableFactory.GetMenuBottomContainer(this);
            m_StyleClasses.Add(style.Class, style);
            // TextBox border
            style = Office2007ColorTableFactory.GetTextBoxStyle(factory.GetColor(0xB3C7E1));
            m_StyleClasses.Add(style.Class, style);
            // ItemPanel
            style = Office2007ColorTableFactory.GetItemPanelStyle(factory.GetColor(0xB3C7E1));
            m_StyleClasses.Add(style.Class, style);
            // DateTimeInput background
            style = Office2007ColorTableFactory.GetDateTimeInputBackgroundStyle(factory.GetColor(0xB3C7E1));
            m_StyleClasses.Add(style.Class, style);
            // Ribbon Client Panel
            style = Office2007ColorTableFactory.GetRibbonClientPanelStyle(factory, eOffice2007ColorScheme.Blue);
            m_StyleClasses.Add(style.Class, style);
            // ListView Border
            style = Office2007ColorTableFactory.GetListViewBorderStyle(this.ListViewEx);
            m_StyleClasses.Add(style.Class, style);
            style = Office2007ColorTableFactory.GetStatusBarAltStyle(this.Bar);
            m_StyleClasses.Add(style.Class, style);
            #if !NOTREE
            // Tree Border/Background
            style = Office2007ColorTableFactory.GetAdvTreeStyle(factory.GetColor(0xB3C7E1));
            m_StyleClasses.Add(style.Class, style);
            style = Office2007ColorTableFactory.GetAdvTreeColumnsHeaderStyle(factory.GetColor(0xF9FCFD), factory.GetColor(0xD3DBE9), factory.GetColor(0x9EB6CE));
            m_StyleClasses.Add(style.Class, style);
            style = Office2007ColorTableFactory.GetAdvTreeNodesColumnsHeaderStyle(factory.GetColor(0xF9FCFD), factory.GetColor(0xD3DBE9), factory.GetColor(0x9EB6CE));
            m_StyleClasses.Add(style.Class, style);
            style = Office2007ColorTableFactory.GetAdvTreeColumnStyle(factory.GetColor(0x000000));
            m_StyleClasses.Add(style.Class, style);
            // CrumbBar
            style = Office2007ColorTableFactory.GetCrumbBarBackgroundStyle(factory.GetColor(Color.White), factory.GetColor("FF567DB0"), factory.GetColor("FF2F578D"));
            m_StyleClasses.Add(style.Class, style);
            // DataGridView border
            style = Office2007ColorTableFactory.GetDataGridViewStyle();
            m_StyleClasses.Add(style.Class, style);
            // DataGridViewDateTime border
            style = Office2007ColorTableFactory.GetDataGridViewDateTimeStyle();
            m_StyleClasses.Add(style.Class, style);
            // DataGridViewNumeric border
            style = Office2007ColorTableFactory.GetDataGridViewNumericStyle();
            m_StyleClasses.Add(style.Class, style);
            // DataGridViewIpAddress border
            style = Office2007ColorTableFactory.GetDataGridViewIpAddressStyle();
            m_StyleClasses.Add(style.Class, style);
#endif
            #endregion

            #region App Button Colors
            Office2010ColorTable.InitAppButtonColors(this, ColorFactory.Empty);
            #endregion
        }

        /// <summary>
        /// Creates new instance of the object.
        /// </summary>
        public Office2007ColorTable(eOffice2007ColorScheme scheme)
        {
            if (scheme == eOffice2007ColorScheme.Black)
                Office2007ColorTableFactory.InitializeVistaBlackColorTable(this, m_ColorFactory);
            else if (scheme == eOffice2007ColorScheme.Silver)
                Office2007SilverColorTableFactory.InitializeColorTable(this, m_ColorFactory);
            else if (scheme == eOffice2007ColorScheme.VistaGlass)
                Office2007VistaBlackColorTableFactory.InitializeVistaBlackColorTable(this, m_ColorFactory);
            else
                InitializeBlueColorTable(m_ColorFactory);
            m_ColorScheme = scheme;
        }

        /// <summary>
        /// Creates new instance of the object.
        /// </summary>
        public Office2007ColorTable(eOffice2007ColorScheme scheme, ColorFactory colorFactory)
        {
            m_ColorFactory = colorFactory;
            if (scheme == eOffice2007ColorScheme.Black)
                Office2007ColorTableFactory.InitializeVistaBlackColorTable(this, m_ColorFactory);
            else if (scheme == eOffice2007ColorScheme.Silver)
                Office2007SilverColorTableFactory.InitializeColorTable(this, m_ColorFactory);
            else if (scheme == eOffice2007ColorScheme.VistaGlass)
                Office2007VistaBlackColorTableFactory.InitializeVistaBlackColorTable(this, m_ColorFactory);
            else
                InitializeBlueColorTable(m_ColorFactory);
            m_ColorScheme = scheme;
        }
        
        /// <summary>
        /// Creates new instance of the color table and initializes it with custom color scheme.
        /// </summary>
        /// <param name="scheme">Predefined color scheme to be used as starting color scheme.</param>
        /// <param name="baseSchemeColor">Color to use as basis for new color scheme</param>
        public Office2007ColorTable(eOffice2007ColorScheme scheme, Color baseSchemeColor)
        {
            if (baseSchemeColor.IsEmpty)
                m_ColorFactory = ColorFactory.Empty;
            else
                m_ColorFactory = new ColorBlendFactory(baseSchemeColor);
            if (scheme == eOffice2007ColorScheme.Black)
                Office2007ColorTableFactory.InitializeVistaBlackColorTable(this, m_ColorFactory);
            else if (scheme == eOffice2007ColorScheme.Silver)
                Office2007SilverColorTableFactory.InitializeColorTable(this, m_ColorFactory);
            else if (scheme == eOffice2007ColorScheme.VistaGlass)
                Office2007VistaBlackColorTableFactory.InitializeVistaBlackColorTable(this, m_ColorFactory);
            else
                InitializeBlueColorTable(m_ColorFactory);
            m_ColorScheme = scheme;
        }
        #endregion

        #region Ribbon Bar Colors
        /// <summary>
        /// Gets or sets the RibbonBar color table.
        /// </summary>
        public Office2007RibbonBarColorTable RibbonBar = new Office2007RibbonBarColorTable();
        #endregion

        #region ButtonItem
        /// <summary>
        /// Gets the reference to collection of Office2007ButtonItemColorTable objects the describe colors used by a button with Office 2007 style.
        /// The collection by default has elements that are created to
        /// represents the members of eButtonColor enumeration. The name of each color table object is the same as the string
        /// enum representation. You can add custom members to this collection or modify the existing ones. Note that you must specify the
        /// unique name for the new color table elements. Name specified there can be used in ButtonItem.CustomColorName property to specify
        /// custom color table for an ButtonItem.
        /// </summary>
        public Office2007ButtonItemColorTableCollection ButtonItemColors
        {
            get { return m_ButtonItemColors; }
        }

        /// <summary>
        /// Gets the reference to collection of Office2007ButtonItemColorTable objects the describe colors used by a button with Office 2007 style when
        /// button is on RibbonBar control. When collection is empty the values from the ButtonItemColors collections are used instead.
        /// </summary>
        public Office2007ButtonItemColorTableCollection RibbonButtonItemColors
        {
            get { return m_RibbonButtonItemColors; }
        }

        /// <summary>
        /// Gets the reference to collection of Office2007ButtonItemColorTable objects the describe colors used by a button with Office 2007 style when
        /// button is on menu bar. When collection is empty the values from the ButtonItemColors collections are used instead.
        /// </summary>
        public Office2007ButtonItemColorTableCollection MenuButtonItemColors
        {
            get { return m_MenuButtonItemColors; }
        }

        /// <summary>
        /// Gets the reference to collection of Office2007ButtonItemColorTable objects the describe colors used by Ribbon Application Menu Button in Office 2010 style.
        /// </summary>
        public Office2007ButtonItemColorTableCollection ApplicationButtonColors
        {
            get { return _ApplicationButtonColors; }
        }

        /// <summary>
        /// Gets the reference to collection of Office2007ButtonItemColorTable objects the describe colors used by buttons that are on Backstage tab-strip.
        /// </summary>
        public Office2007ButtonItemColorTableCollection BackstageButtonItemColors
        {
            get { return _BackstageButtonColors; }
        }
        
        #endregion

        #region RibbonTabItem
        /// <summary>
        /// Gets the reference to collection of Office2007RibbonTabItemColorTable objects the describe colors used by a ribbon tab with Office 2007 style.
        /// The collection by default has elements that are created to
        /// represents the members of eRibbonTabColor enumeration. The name of each color table object is the same as the string
        /// enum representation. You can add custom members to this collection or modify the existing ones. Note that you must specify the
        /// unique name for the new color table elements. Name specified there can be used in RibbonTabItem.CustomColorName property to specify
        /// custom color table for an ButtonItem.
        /// </summary>
        public Office2007RibbonTabItemColorTableCollection RibbonTabItemColors
        {
            get { return m_RibbonTabItemColors; }
        }
        #endregion

        #region RibbonTabItemGroup
        /// <summary>
        /// Gets the reference to collection of Office2007RibbonTabGroupColorTableCollection objects the describe colors used by a ribbon tab groups with Office 2007 style.
        /// The collection by default has elements that are created to
        /// represents the members of eRibbonTabGroupColor enumeration. The name of each color table object is the same as the string
        /// enum representation. You can add custom members to this collection or modify the existing ones. Note that you must specify the
        /// unique name for the new color table elements.
        /// </summary>
        public Office2007RibbonTabGroupColorTableCollection RibbonTabGroupColors
        {
            get { return m_RibbonTabGroupColors; }
        }
        #endregion

        #region Item Group
        /// <summary>
        /// Gets or sets the background colors for the ItemContainer with BeginGroup property set to true.
        /// </summary>
        public Office2007ItemGroupColorTable ItemGroup = new Office2007ItemGroupColorTable();
        #endregion

        #region Bar
        /// <summary>
        /// Gets or sets the background colors for the Bar object.
        /// </summary>
        public Office2007BarColorTable Bar = new Office2007BarColorTable();
        #endregion

        #region Ribbon Control
        /// <summary>
        /// Gets or sets the colors for the RibbonControl.
        /// </summary>
        public Office2007RibbonColorTable RibbonControl = new Office2007RibbonColorTable();
        #endregion

        #region ColorItem
        /// <summary>
        /// Gets or sets the colors for the ColorItem which is used by drop-down Color Picker.
        /// </summary>
        public Office2007ColorItemColorTable ColorItem = new Office2007ColorItemColorTable();
        #endregion

        #region Menu
        /// <summary>
        /// Gets or sets the color table for menus.
        /// </summary>
        public Office2007MenuColorTable Menu = new Office2007MenuColorTable();
        #endregion

        #region ComboBox
        /// <summary>
        /// Gets or sets the color table for ComboBoxItem.
        /// </summary>
        public Office2007ComboBoxColorTable ComboBox = new Office2007ComboBoxColorTable();
        #endregion

        #region Dialog Launcher
        /// <summary>
        /// Gets or sets the colors for the Ribbon Bar dialog launcher button.
        /// </summary>
        public Office2007DialogLauncherColorTable DialogLauncher = new Office2007DialogLauncherColorTable();
        #endregion

        #region Legacy ColorScheme
        /// <summary>
        /// Gets or sets the legacy color scheme object that applies to the user interface elements not covered by color table.
        /// </summary>
        public ColorScheme LegacyColors = new ColorScheme(eDotNetBarStyle.Office2007);
        #endregion

        #region System Button
        /// <summary>
        /// Gets or sets the color table of the system buttons displayed in form caption.
        /// </summary>
        public Office2007SystemButtonColorTable SystemButton = new Office2007SystemButtonColorTable();
        /// <summary>
        /// Gets or sets the color table of the close system button displayed in form caption. Applies to Office 2010 styles and later only.
        /// </summary>
        public Office2007SystemButtonColorTable SystemButtonClose = null;

        /// <summary>
        /// Gets or sets the color table for the form caption.
        /// </summary>
        public Office2007FormColorTable Form = new Office2007FormColorTable();
        #endregion

        #region Quick Access Toolbar Background
        /// <summary>
        /// Gets or sets the bacgkround colors for the quick access toolbar.
        /// </summary>
        public Office2007QuickAccessToolbarColorTable QuickAccessToolbar = new Office2007QuickAccessToolbarColorTable();
        #endregion

        #region Tab Control
        /// <summary>
        /// Gets or sets the colors for the tab and tab strip control.
        /// </summary>
        public Office2007TabColorTable TabControl = new Office2007TabColorTable();
        #endregion

        #region KeyTips
        /// <summary>
        /// Gets or sets the KeyTips color table.
        /// </summary>
        public Office2007KeyTipsColorTable KeyTips = new Office2007KeyTipsColorTable();
        #endregion

        #region CheckBoxItem Colors
        /// <summary>
        /// Gets or sets the color table for the CheckBoxItem.
        /// </summary>
        public Office2007CheckBoxColorTable CheckBoxItem = new Office2007CheckBoxColorTable();
        #endregion

        #region Scrollbar Colors
        /// <summary>
        /// Gets or sets the scroll bar colors.
        /// </summary>
        public Office2007ScrollBarColorTable ScrollBar = new Office2007ScrollBarColorTable();
        /// <summary>
        /// Gets or sets the application style scroll bar colors.
        /// </summary>
        public Office2007ScrollBarColorTable AppScrollBar = new Office2007ScrollBarColorTable();
        #endregion

        #region ProgressBarItem Colors
        /// <summary>
        /// Gets or sets the color table for the ProgressBarItem.
        /// </summary>
        public Office2007ProgressBarColorTable ProgressBarItem = new Office2007ProgressBarColorTable();

        /// <summary>
        /// Gets or sets the color table for the paused ProgressBarItem.
        /// </summary>
        public Office2007ProgressBarColorTable ProgressBarItemPaused = new Office2007ProgressBarColorTable();

        /// <summary>
        /// Gets or sets the color table for the error state of ProgressBarItem.
        /// </summary>
        public Office2007ProgressBarColorTable ProgressBarItemError = new Office2007ProgressBarColorTable();
        #endregion

        #region Gallery
        /// <summary>
        /// Gets or sets the color table for the galleries.
        /// </summary>
        public Office2007GalleryColorTable Gallery = new Office2007GalleryColorTable();
        #endregion

        #region Navigation Pane
        /// <summary>
        /// Gets or sets the color table for the NavigationPane control.
        /// </summary>
        public Office2007NavigationPaneColorTable NavigationPane = new Office2007NavigationPaneColorTable();
        #endregion

        #region Slider
        /// <summary>
        /// Gets or sets the color table for the Slider item.
        /// </summary>
        public Office2007SliderColorTable Slider = new Office2007SliderColorTable();
        #endregion

        #region SuperTooltip
        /// <summary>
        /// Gets the SuperTooltip color table.
        /// </summary>
        public Office2007SuperTooltipColorTable SuperTooltip = new Office2007SuperTooltipColorTable();
        #endregion

        #region ListViewEx
        /// <summary>
        /// Gets the color table for the ListViewEx control.
        /// </summary>
        public Office2007ListViewColorTable ListViewEx = new Office2007ListViewColorTable();
        #endregion

        #region DataGridView
        /// <summary>
        /// Gets the color table for the ListViewEx control.
        /// </summary>
        public Office2007DataGridViewColorTable DataGridView = new Office2007DataGridViewColorTable();
        #endregion

        #region SideBar
        /// <summary>
        /// Gets the color table used by SideBar control.
        /// </summary>
        public Office2007SideBarColorTable SideBar = new Office2007SideBarColorTable();
        #endregion

        #region AdvTree
#if !NOTREE
        /// <summary>
        /// Gets or sets the color table for AdvTree control.
        /// </summary>
        public DevComponents.AdvTree.Display.TreeColorTable AdvTree = null;
#endif
        #endregion

        #region CrumbBar
        /// <summary>
        /// Gets or sets the CrumBarItem color table.
        /// </summary>
        public CrumbBarItemViewColorTable CrumbBarItemView = null;
        #endregion

        #region WarningBox
        public Office2007WarningBoxColorTable WarningBox = new Office2007WarningBoxColorTable();
        #endregion

        #region CalendarView
        /// <summary>
        /// Gets the color table used by Schedule control.
        /// </summary>
        public Office2007CalendarViewColorTable CalendarView = new Office2007CalendarViewColorTable();
        #endregion

        #region SuperTabControl

        #region Office2007 Style
        public SuperTabColorTable SuperTab = new SuperTabColorTable();
        public SuperTabItemColorTable SuperTabItem = new SuperTabItemColorTable();
        public SuperTabPanelColorTable SuperTabPanel = new SuperTabPanelColorTable();
        #endregion

        #region BackStage Style
        public SuperTabColorTable Backstage = new SuperTabColorTable();
        public SuperTabItemColorTable BackstageItem = new SuperTabItemColorTable();
        public SuperTabPanelColorTable BackstagePanel = new SuperTabPanelColorTable();
        #endregion

        #endregion

        #region SwitchButton
        /// <summary>
        /// Gets or sets SwitchButton color table.
        /// </summary>
        public SwitchButtonColors SwitchButton = null;
        #endregion

        /// <summary>
        /// Returns the color scheme table was initialized with.
        /// </summary>
        public eOffice2007ColorScheme InitialColorScheme
        {
            get { return m_ColorScheme; }
        }

        #region IElementStyleClassProvider Members
        /// <summary>
        /// Returns the instance of the ElementStyle with given class name or null if there is no class with that name defined.
        /// </summary>
        /// <param name="className">Class name. See static members of ElementStyleClassKeys class for the list of available keys.</param>
        /// <returns>Instance of ElementStyle for given class name or null if class cannot be found.</returns>
        public ElementStyle GetClass(string className)
        {
            if (m_StyleClasses.ContainsKey(className))
                return (ElementStyle)m_StyleClasses[className];
            return null;
        }

        #endregion

        /// <summary>
        /// Get the reference to the collection that holds system cached styles. In most cases there is no need for you to modify members of this collection.
        /// </summary>
        public Hashtable StyleClasses
        {
            get { return m_StyleClasses; }
        }

        private static bool _CloneImagesOnAccess = false;
        /// <summary>
        /// Gets or sets whether images like Start button image, are cloned when painted. This is reduces the performance but is necessary if they can be accessed from multiple threads.
        /// </summary>
        public static bool CloneImagesOnAccess
        {
            get { return _CloneImagesOnAccess; }
            set
            {
                _CloneImagesOnAccess = value;
            }
        }
        

        #region IDisposable Members

        public void Dispose()
        {
            if (this.RibbonControl.StartButtonDefault != null)
            {
                this.RibbonControl.StartButtonDefault.Dispose();
                this.RibbonControl.StartButtonDefault = null;
            }
            if (this.RibbonControl.StartButtonMouseOver != null)
            {
                this.RibbonControl.StartButtonMouseOver.Dispose();
                this.RibbonControl.StartButtonMouseOver = null;
            }
            if (this.RibbonControl.StartButtonPressed != null)
            {
                this.RibbonControl.StartButtonPressed.Dispose();
                this.RibbonControl.StartButtonPressed = null;
            }
        }

        #endregion
    }

    /// <summary>
    /// Defines the color scheme type for the Office2007ColorTable.
    /// </summary>
    public enum eOffice2007ColorScheme
    {
        /// <summary>
        /// Blue color scheme.
        /// </summary>
        Blue,
        /// <summary>
        /// Black color scheme.
        /// </summary>
        Black,
        /// <summary>
        /// Silver color scheme.
        /// </summary>
        Silver,
        /// <summary>
        /// Windows Vista Glass inspired color scheme.
        /// </summary>
        VistaGlass
    }

    /// <summary>
    /// Defines the delegate which retrieves the color for specific integer value.
    /// </summary>
    /// <param name="rgb">color represented as integer.</param>
    /// <returns>Reference to Color object.</returns>
    public delegate Color GetColorDelegate(int rgb);
}
