using System;
using System.Text;
using System.Drawing;
using DevComponents.DotNetBar.Schedule;

namespace DevComponents.DotNetBar.Rendering
{
    public class Office2007VistaBlackColorTableFactory
    {
        #region Luna Blue

        #region ButtonItem Colors Initialization
        public static void SetBlueExpandColors(Office2007ButtonItemColorTable ct, ColorFactory factory)
        {
            Color cb = factory.GetColor(0x567DB1);
            Color cl = factory.GetColor(0xEAF2F9);
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

        public static void SetBlackExpandColors(Office2007ButtonItemColorTable ct, ColorFactory factory)
        {
            Color cb = factory.GetColor(0x464646);
            Color cl = factory.GetColor(0xDBDBDC);
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
        

        public static Office2007ButtonItemColorTable GetButtonItemOffice2007WithBackground(ColorFactory factory)
        {
            Office2007ButtonItemColorTable cb = new Office2007ButtonItemColorTable();
            cb.Default = new Office2007ButtonItemStateColorTable();

            cb.Default.TopBackground = new LinearGradientColorTable(factory.GetColor(0xCBF0FB), factory.GetColor(0x6FBBE5));
            cb.Default.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
            cb.Default.BottomBackground = new LinearGradientColorTable(factory.GetColor(0x66B5DF), factory.GetColor(0xA2EDF6));
            cb.Default.BottomBackgroundHighlight = new LinearGradientColorTable(factory.GetColor(0xB1FCFF), Color.Empty);
            cb.Default.InnerBorder = new LinearGradientColorTable(Color.White, Color.Empty);
            cb.Default.OuterBorder = new LinearGradientColorTable(factory.GetColor(0x799DB6), Color.Empty);
            cb.Default.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
            cb.Default.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);

            // Same as default
            cb.Disabled = new Office2007ButtonItemStateColorTable();
            cb.Disabled.TopBackground = new LinearGradientColorTable(factory.GetColor(Color.WhiteSmoke), factory.GetColor(Color.LightGray));
            cb.Disabled.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
            cb.Disabled.BottomBackground = new LinearGradientColorTable(factory.GetColor(Color.Silver), factory.GetColor(0xEBF3FD));
            cb.Disabled.BottomBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
            cb.Disabled.InnerBorder = new LinearGradientColorTable(Color.WhiteSmoke, Color.Empty);
            cb.Disabled.OuterBorder = new LinearGradientColorTable(factory.GetColor(0x799DB6), Color.Empty);
            cb.Disabled.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
            cb.Disabled.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
            cb.Disabled.Text = factory.GetColor(0x8D8D8D);

            cb.MouseOver = new Office2007ButtonItemStateColorTable();
            cb.MouseOver.TopBackground = new LinearGradientColorTable(factory.GetColor(0xFCF9E2), factory.GetColor(0xFBEDBF));
            cb.MouseOver.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
            cb.MouseOver.BottomBackground = new LinearGradientColorTable(factory.GetColor(0xFAD84A), factory.GetColor(0xFCE595));
            cb.MouseOver.BottomBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
            cb.MouseOver.InnerBorder = new LinearGradientColorTable(Color.White, Color.Empty);
            cb.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(0xBFA779), Color.Empty);
            cb.MouseOver.SplitBorder = new LinearGradientColorTable(factory.GetColor(0xBFA779), Color.Empty);
            cb.MouseOver.SplitBorderLight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Empty);

            cb.Pressed = new Office2007ButtonItemStateColorTable();
            cb.Pressed.TopBackground = new LinearGradientColorTable(factory.GetColor(0xF8BA78), factory.GetColor(0xFCB24E));
            cb.Pressed.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
            cb.Pressed.BottomBackground = new LinearGradientColorTable(factory.GetColor(0xFC9C0F), factory.GetColor(0xFCBA37));
            cb.Pressed.BottomBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
            cb.Pressed.InnerBorder = new LinearGradientColorTable(factory.GetColor(0xF1A74A), Color.Empty);
            cb.Pressed.OuterBorder = new LinearGradientColorTable(factory.GetColor(0x937D5A), Color.Empty);
            cb.Pressed.SplitBorder = new LinearGradientColorTable(factory.GetColor(0x937D5A), Color.Empty);
            cb.Pressed.SplitBorderLight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Empty);

            cb.Checked = new Office2007ButtonItemStateColorTable();
            cb.Checked.TopBackground = new LinearGradientColorTable(factory.GetColor(0xFCF9E2), factory.GetColor(0xFBEDBF));
            cb.Checked.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
            cb.Checked.BottomBackground = new LinearGradientColorTable(factory.GetColor(0xFAD84A), factory.GetColor(0xFCE595));
            cb.Checked.BottomBackgroundHighlight = new LinearGradientColorTable(Color.White, Color.Empty);
            cb.Checked.InnerBorder = new LinearGradientColorTable(factory.GetColor(0xF1A74A), Color.Empty);
            cb.Checked.OuterBorder = new LinearGradientColorTable(factory.GetColor(0x937D5A), Color.Empty);
            cb.Checked.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
            cb.Checked.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);

            cb.Expanded = new Office2007ButtonItemStateColorTable();
            cb.Expanded.TopBackground = new LinearGradientColorTable(factory.GetColor(0xFCBA37), factory.GetColor(0xFC9C0F));
            cb.Expanded.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
            cb.Expanded.BottomBackground = new LinearGradientColorTable(factory.GetColor(0xFCB24E), factory.GetColor(0xF8BA78));
            cb.Expanded.BottomBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
            cb.Expanded.InnerBorder = new LinearGradientColorTable(factory.GetColor(0xF1A74A), Color.Empty);
            cb.Expanded.OuterBorder = new LinearGradientColorTable(factory.GetColor(0x937D5A), Color.Empty);
            cb.Expanded.SplitBorder = new LinearGradientColorTable(factory.GetColor(0x937D5A), Color.Empty);
            cb.Expanded.SplitBorderLight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Empty);
            return cb;
        }
        #endregion

        #region RibbonBar Initialization
        public static Office2007RibbonBarStateColorTable GetRibbonBarBlue(ColorFactory factory)
        {
            Office2007RibbonBarStateColorTable rb = new Office2007RibbonBarStateColorTable();
            rb.TopBackgroundHeight = 15;
            rb.OuterBorder = new LinearGradientColorTable(factory.GetColor(0xC5D2DF), factory.GetColor(0x9EBFDB));
            rb.InnerBorder = new LinearGradientColorTable(factory.GetColor(0xE7EEF7), factory.GetColor(0xF1F7FD));
            rb.TopBackground = new LinearGradientColorTable(factory.GetColor(0xDEE8F5), factory.GetColor(0xD1DFF0));
            rb.BottomBackground = new LinearGradientColorTable(factory.GetColor(0xC7D8ED), factory.GetColor(0xD8E8F5));
            rb.TitleBackground = new LinearGradientColorTable(factory.GetColor(0xC2D8F1), factory.GetColor(0xC0D8EF));
            rb.TitleText = factory.GetColor(0x3E6AAA);
            return rb;
        }

        public static Office2007RibbonBarStateColorTable GetRibbonBarBlueMouseOver(ColorFactory factory)
        {
            Office2007RibbonBarStateColorTable rb = new Office2007RibbonBarStateColorTable();
            rb.TopBackgroundHeight = 15;
            rb.OuterBorder = new LinearGradientColorTable(factory.GetColor(0xADC7DE), factory.GetColor(0x7EADD3));
            rb.InnerBorder = new LinearGradientColorTable(factory.GetColor(0xFFFFFF), factory.GetColor(0xFFFFFF));
            rb.TopBackground = new LinearGradientColorTable(factory.GetColor(0xE4EFFD), factory.GetColor(0xE8F0FC));
            rb.BottomBackground = new LinearGradientColorTable(factory.GetColor(0xDCEAFB), factory.GetColor(0xDCE8F8));
            rb.TitleBackground = new LinearGradientColorTable(factory.GetColor(0xC8E0FF), factory.GetColor(0xD6EDFF));
            rb.TitleText = factory.GetColor(0x3E6AB9);

            return rb;
        }

        public static Office2007RibbonBarStateColorTable GetRibbonBarBlueExpanded(ColorFactory factory)
        {
            Office2007RibbonBarStateColorTable rb = new Office2007RibbonBarStateColorTable();
            rb.TopBackgroundHeight = 15;
            rb.OuterBorder = new LinearGradientColorTable(factory.GetColor(0x869BAE), factory.GetColor(0xB1C7D9));
            rb.InnerBorder = new LinearGradientColorTable(Color.FromArgb(32, Color.White), Color.Transparent);
            rb.TopBackground = new LinearGradientColorTable(factory.GetColor(0xADC1DC), factory.GetColor(0x88A7D0));
            rb.BottomBackground = new LinearGradientColorTable(factory.GetColor(0x7699C8), factory.GetColor(0xB4D5FD));
            rb.TitleBackground = new LinearGradientColorTable(factory.GetColor(0xC3D9F2), Color.Empty);
            rb.TitleText = Color.Empty;
            return rb;
        }
        #endregion

        #region RibbonTabItem
        public static Office2007RibbonTabItemColorTable GetRibbonTabItemBlueDefault(ColorFactory factory)
        {
            Office2007RibbonTabItemColorTable rt = new Office2007RibbonTabItemColorTable();
            rt.Default.Text = factory.GetColor(0x15428B);

            // Selected Tab
            rt.Selected = new Office2007RibbonTabItemStateColorTable();
            rt.Selected.Background = new LinearGradientColorTable(factory.GetColor(0xF0F6FE), factory.GetColor(0xDBE6F5));
            rt.Selected.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
            rt.Selected.InnerBorder = new LinearGradientColorTable(factory.GetColor(0xF8FBFF), factory.GetColor(0xBFFAFF));
            rt.Selected.OuterBorder = new LinearGradientColorTable(factory.GetColor(0x97B9E6), factory.GetColor(0x8DB2E3));
            rt.Selected.Text = factory.GetColor(0x15428B);

            // Selected Tab Mouse Over
            rt.SelectedMouseOver = new Office2007RibbonTabItemStateColorTable();
            rt.SelectedMouseOver.Background = new LinearGradientColorTable(factory.GetColor(0xEBF3FE), factory.GetColor(0xE1EAF6));
            rt.SelectedMouseOver.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(32, Color.White), Color.Transparent);
            rt.SelectedMouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(0xFAEDC6), factory.GetColor(0xFFFFBD));
            rt.SelectedMouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(0xD7BC8A), factory.GetColor(0xFED15E));
            rt.SelectedMouseOver.Text = factory.GetColor(0x15428B);

            // Tab Mouse Over
            rt.MouseOver = new Office2007RibbonTabItemStateColorTable();
            rt.MouseOver.Background = new LinearGradientColorTable(factory.GetColor(0xCDDAE0), factory.GetColor(0xE4D097));
            rt.MouseOver.BackgroundHighlight = new LinearGradientColorTable(factory.GetColor(0xC4DEFF), Color.Transparent);
            rt.MouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(0xDFEDFF), factory.GetColor(0xC7DFFF));
            rt.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(0x98BBE9), factory.GetColor(0x98BBE9));
            rt.MouseOver.Text = factory.GetColor(0x15428B);

            return rt;
        }

        public static Office2007RibbonTabItemColorTable GetRibbonTabItemBlueMagenta(ColorFactory factory)
        {
            Office2007RibbonTabItemColorTable rt = new Office2007RibbonTabItemColorTable();
            rt.Default.Text = factory.GetColor(0x15428B);
            // Selected Tab
            rt.Selected = new Office2007RibbonTabItemStateColorTable();
            rt.Selected.Background = new LinearGradientColorTable(factory.GetColor(0xD4C8E2), factory.GetColor(0xECE7F2));
            rt.Selected.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
            rt.Selected.InnerBorder = new LinearGradientColorTable(factory.GetColor(0xECE7F3), factory.GetColor(0xCDC2DF));
            rt.Selected.OuterBorder = new LinearGradientColorTable(factory.GetColor(0xBEBAC1), factory.GetColor(0xC0BFC1));
            rt.Selected.Text = factory.GetColor(0x15428B);

            // Selected Tab Mouse Over
            rt.SelectedMouseOver = new Office2007RibbonTabItemStateColorTable();
            rt.SelectedMouseOver.Background = new LinearGradientColorTable(factory.GetColor(0xD4C8E2), factory.GetColor(0xECE7F2));
            rt.SelectedMouseOver.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(32, Color.White), Color.Transparent);
            rt.SelectedMouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(0xECE7F3), factory.GetColor(0xCDC2DF));
            rt.SelectedMouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(0xBEBAC1), factory.GetColor(0xC0BFC1));
            rt.SelectedMouseOver.Text = factory.GetColor(0x15428B);

            // Tab Mouse Over
            rt.MouseOver = new Office2007RibbonTabItemStateColorTable();
            rt.MouseOver.Background = new LinearGradientColorTable(factory.GetColor(0xC4D5F8), factory.GetColor(0xC2BCE8));
            rt.MouseOver.BackgroundHighlight = new LinearGradientColorTable(factory.GetColor(0xDEECFF), Color.Transparent);
            rt.MouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(0xE0EEFF), factory.GetColor(0xC7DFFF));
            rt.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(0xC1C8D1), factory.GetColor(0xC0C7D0));
            rt.MouseOver.Text = factory.GetColor(0x15428B);

            return rt;
        }

        public static Office2007RibbonTabItemColorTable GetRibbonTabItemBlueGreen(ColorFactory factory)
        {
            Office2007RibbonTabItemColorTable rt = new Office2007RibbonTabItemColorTable();
            rt.Default.Text = factory.GetColor(0x15428B);

            // Selected Tab
            rt.Selected = new Office2007RibbonTabItemStateColorTable();
            rt.Selected.Background = new LinearGradientColorTable(factory.GetColor(0xE4F3E0), factory.GetColor(0xE4F2DF));
            rt.Selected.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
            rt.Selected.InnerBorder = new LinearGradientColorTable(factory.GetColor(0xD5EAD3), factory.GetColor(0xB8DEB1));
            rt.Selected.OuterBorder = new LinearGradientColorTable(factory.GetColor(0xB9C1B7), factory.GetColor(0xBFC1BE));
            rt.Selected.Text = factory.GetColor(0x15428B);

            // Selected Tab Mouse Over
            rt.SelectedMouseOver = new Office2007RibbonTabItemStateColorTable();
            rt.SelectedMouseOver.Background = new LinearGradientColorTable(factory.GetColor(0xE4F3E0), factory.GetColor(0xE4F2DF));
            rt.SelectedMouseOver.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(32, Color.White), Color.Transparent);
            rt.SelectedMouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(0xD5EAD3), factory.GetColor(0xB8DEB1));
            rt.SelectedMouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(0xB9C1B7), factory.GetColor(0xBFC1BE));
            rt.SelectedMouseOver.Text = factory.GetColor(0x15428B);

            // Tab Mouse Over
            rt.MouseOver = new Office2007RibbonTabItemStateColorTable();
            rt.MouseOver.Background = new LinearGradientColorTable(factory.GetColor(0xB3DEE3), factory.GetColor(0x8ADE9F));
            rt.MouseOver.BackgroundHighlight = new LinearGradientColorTable(factory.GetColor(0xDEECFF), Color.Transparent);
            rt.MouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(0xE2EFFF), factory.GetColor(0xC7DFFF));
            rt.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(0xC1C8D1), factory.GetColor(0xC0C7D0));
            rt.MouseOver.Text = factory.GetColor(0x15428B);

            return rt;
        }

        public static Office2007RibbonTabItemColorTable GetRibbonTabItemBlueOrange(ColorFactory factory)
        {
            Office2007RibbonTabItemColorTable rt = new Office2007RibbonTabItemColorTable();
            rt.Default.Text = factory.GetColor(0x15428B);

            // Selected Tab
            rt.Selected = new Office2007RibbonTabItemStateColorTable();
            rt.Selected.Background = new LinearGradientColorTable(factory.GetColor(0xFFF59F), factory.GetColor(0xFFFAD5));
            rt.Selected.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
            rt.Selected.InnerBorder = new LinearGradientColorTable(factory.GetColor(0xFFFBD6), factory.GetColor(0xFDF28C));
            rt.Selected.OuterBorder = new LinearGradientColorTable(factory.GetColor(0xCAC7AC), factory.GetColor(0xC2C2C2));
            rt.Selected.Text = factory.GetColor(0x15428B);

            // Selected Tab Mouse Over
            rt.SelectedMouseOver = new Office2007RibbonTabItemStateColorTable();
            rt.SelectedMouseOver.Background = new LinearGradientColorTable(factory.GetColor(0xFFF59F), factory.GetColor(0xFFFAD5));
            rt.SelectedMouseOver.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(32, Color.White), Color.Transparent);
            rt.SelectedMouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(0xFFFBD6), factory.GetColor(0xFDF28C));
            rt.SelectedMouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(0xCAC7AC), factory.GetColor(0xC2C2C2));
            rt.SelectedMouseOver.Text = factory.GetColor(0x15428B);

            // Tab Mouse Over
            rt.MouseOver = new Office2007RibbonTabItemStateColorTable();
            rt.MouseOver.Background = new LinearGradientColorTable(factory.GetColor(0xCFE0E1), factory.GetColor(0xE9E799));
            rt.MouseOver.BackgroundHighlight = new LinearGradientColorTable(factory.GetColor(0xCBE2FF), Color.Transparent);
            rt.MouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(0xE2EFFF), factory.GetColor(0xC7DFFF));
            rt.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(0xC1C8D1), factory.GetColor(0xC0C7D0));
            rt.MouseOver.Text = factory.GetColor(0x15428B);

            return rt;
        }
        #endregion

        #region RibbonTabItemGroup
        public static Office2007RibbonTabGroupColorTable GetRibbonTabGroupBlueDefault(ColorFactory factory)
        {
            Office2007RibbonTabGroupColorTable tg = new Office2007RibbonTabGroupColorTable();
            tg.Background = new LinearGradientColorTable(Color.Transparent /*factory.GetColor(0xCADBF3)*/, factory.GetColor(0xCDBFD7));
            tg.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, factory.GetColor(0xD068C9)), Color.Transparent);
            tg.Text = factory.GetColor(0x15428B);
            tg.Border = new LinearGradientColorTable(Color.FromArgb(16, Color.DarkGray), Color.FromArgb(192, Color.DarkGray));

            return tg;
        }

        public static Office2007RibbonTabGroupColorTable GetRibbonTabGroupBlueMagenta(ColorFactory factory)
        {
            return GetRibbonTabGroupBlueDefault(factory);
        }

        public static Office2007RibbonTabGroupColorTable GetRibbonTabGroupBlueGreen(ColorFactory factory)
        {
            Office2007RibbonTabGroupColorTable tg = new Office2007RibbonTabGroupColorTable();

            tg.Background = new LinearGradientColorTable(Color.Transparent /*factory.GetColor(0xC8DDF2)*/, factory.GetColor(0xA6E28E));
            tg.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, factory.GetColor(0x6BB954)), Color.Transparent);
            tg.Border = new LinearGradientColorTable(Color.FromArgb(16, Color.DarkGray), Color.FromArgb(192, Color.DarkGray));
            tg.Text = factory.GetColor(0x15428B);

            return tg;
        }

        public static Office2007RibbonTabGroupColorTable GetRibbonTabGroupBlueOrange(ColorFactory factory)
        {
            Office2007RibbonTabGroupColorTable tg = new Office2007RibbonTabGroupColorTable();

            tg.Background = new LinearGradientColorTable(Color.Transparent /*factory.GetColor(0xCDDFF1)*/, factory.GetColor(0xECEFA9));
            tg.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, factory.GetColor(0xFFE619)), Color.Transparent);
            tg.Border = new LinearGradientColorTable(Color.FromArgb(16, Color.DarkGray), Color.FromArgb(192, Color.DarkGray));
            tg.Text = factory.GetColor(0x15428B);

            return tg;
        }
        #endregion

        #endregion

        #region Vista Black

        #region RibbonBar
        public static Office2007RibbonBarStateColorTable GetRibbonBarBlack(ColorFactory factory)
        {
            Office2007RibbonBarStateColorTable rb = new Office2007RibbonBarStateColorTable();
            rb.TopBackgroundHeight = 15;
            //rb.OuterBorder = new LinearGradientColorTable(factory.GetColor(0x000000), factory.GetColor(0x000000));
            //rb.InnerBorder = new LinearGradientColorTable(factory.GetColor(0x757D95), factory.GetColor(0x757D95));
            rb.OuterBorder = new LinearGradientColorTable(factory.GetColor(0x5C6672), factory.GetColor(0x5C6672));
            rb.InnerBorder = new LinearGradientColorTable(factory.GetColor(0xD6E5F8), factory.GetColor(0xD6E5F8));
            //rb.TopBackground = new LinearGradientColorTable(factory.GetColor(0x626C88), factory.GetColor(0x3F4756));
            //rb.BottomBackground = new LinearGradientColorTable(factory.GetColor(0x151516), factory.GetColor(0x3C476F));
            rb.TopBackground = new LinearGradientColorTable(factory.GetColor(0xEDF6FF), factory.GetColor(0xBDD3EB));
            rb.BottomBackground = new LinearGradientColorTable(factory.GetColor(0xA0B7D2), factory.GetColor(0xF0F0F0));
            
            //rb.TitleBackground = new LinearGradientColorTable(factory.GetColor(0x0D1D5A), factory.GetColor(0x000000));
            //rb.TitleText = factory.GetColor(0xFFFFFF);
            rb.TitleBackground = new LinearGradientColorTable(factory.GetColor(0x8290A0), factory.GetColor(0x616C76));
            rb.TitleText = factory.GetColor(0xFFFFFF);
            return rb;
        }

        public static Office2007RibbonBarStateColorTable GetRibbonBarBlackMouseOver(ColorFactory factory)
        {
            Office2007RibbonBarStateColorTable rb = new Office2007RibbonBarStateColorTable();
            rb.TopBackgroundHeight = 15;
            //rb.OuterBorder = new LinearGradientColorTable(factory.GetColor(0x000000), factory.GetColor(0x000000));
            //rb.InnerBorder = new LinearGradientColorTable(factory.GetColor(0x757D95), factory.GetColor(0x757D95));
            //rb.TopBackground = new LinearGradientColorTable(factory.GetColor(0x8E9EC6), factory.GetColor(0x6B7A93));
            //rb.BottomBackground = new LinearGradientColorTable(factory.GetColor(0x222325), factory.GetColor(0x526092));
            //rb.TitleBackground = new LinearGradientColorTable(factory.GetColor(0x132B86), factory.GetColor(0x030612));
            //rb.TitleText = factory.GetColor(0xFFFFFF);
            rb.OuterBorder = new LinearGradientColorTable(factory.GetColor(0x5C6672), factory.GetColor(0x5C6672));
            rb.InnerBorder = new LinearGradientColorTable(factory.GetColor(0xD6E5F8), factory.GetColor(0xD6E5F8));
            rb.TopBackground = new LinearGradientColorTable(factory.GetColor(0xEDF6FF), factory.GetColor(0xCBE3FC));
            rb.BottomBackground = new LinearGradientColorTable(factory.GetColor(0xB5D1EE), factory.GetColor(0xF0F0F0));
            rb.TitleBackground = new LinearGradientColorTable(factory.GetColor(0x9BACC0), factory.GetColor(0x616C76));
            rb.TitleText = factory.GetColor(0xFFFFFF);

            return rb;
        }

        public static Office2007RibbonBarStateColorTable GetRibbonBarBlackExpanded(ColorFactory factory)
        {
            Office2007RibbonBarStateColorTable rb = new Office2007RibbonBarStateColorTable();
            rb.TopBackgroundHeight = 15;
            rb.OuterBorder = new LinearGradientColorTable(factory.GetColor(0x5C6672), factory.GetColor(0x5C6672));
            rb.InnerBorder = new LinearGradientColorTable(factory.GetColor(0xD6E5F8), factory.GetColor(0xD6E5F8));
            rb.TopBackground = new LinearGradientColorTable(factory.GetColor(0xEDF6FF), factory.GetColor(0xCBE3FC));
            rb.BottomBackground = new LinearGradientColorTable(factory.GetColor(0xB5D1EE), factory.GetColor(0xF0F0F0));
            rb.TitleBackground = new LinearGradientColorTable(factory.GetColor(0x9BACC0), factory.GetColor(0x616C76));
            rb.TitleText = Color.Empty;

            return rb;
        }
        #endregion

        #region RibbonTabItem
        public static Office2007RibbonTabItemColorTable GetRibbonTabItemBlackDefault(ColorFactory factory)
        {
            Office2007RibbonTabItemColorTable rt = new Office2007RibbonTabItemColorTable();
            rt.Default.Text = factory.GetColor(0xFFFFFF);

            // Selected Tab
            rt.Selected = new Office2007RibbonTabItemStateColorTable();
            rt.Selected.Background = new LinearGradientColorTable(factory.GetColor(0x799EE1), factory.GetColor(0x1E449F));
            rt.Selected.BackgroundHighlight = new LinearGradientColorTable(factory.GetColor(0x57C6EF), Color.Transparent);
            rt.Selected.InnerBorder = new LinearGradientColorTable(factory.GetColor(0x84A9EB), factory.GetColor(0x1C667C));
            rt.Selected.OuterBorder = new LinearGradientColorTable(factory.GetColor(0x000000), factory.GetColor(0x000000));
            rt.Selected.Text = factory.GetColor(0xFFFFFF);

            // Selected Tab Mouse Over
            rt.SelectedMouseOver = new Office2007RibbonTabItemStateColorTable();
            rt.SelectedMouseOver.Background = new LinearGradientColorTable(factory.GetColor(0x5B93FF), factory.GetColor(0x0E3DC7));
            rt.SelectedMouseOver.BackgroundHighlight = new LinearGradientColorTable(factory.GetColor(0x6FD4FF), Color.Transparent);
            rt.SelectedMouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(0x84A9EB), factory.GetColor(0x2992AA));
            rt.SelectedMouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(0x000000), factory.GetColor(0x000000));
            rt.SelectedMouseOver.Text = factory.GetColor(0xFFFFFF);

            // Tab Mouse Over
            rt.MouseOver = new Office2007RibbonTabItemStateColorTable();
            rt.MouseOver.Background = new LinearGradientColorTable(factory.GetColor(0x9098AA), factory.GetColor(0x3F588D));
            rt.MouseOver.BackgroundHighlight = new LinearGradientColorTable(factory.GetColor(0x3C81CE), Color.Transparent);
            rt.MouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(0xBFC4CE), factory.GetColor(0x566794));
            rt.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(0x000000), factory.GetColor(0x000000));
            rt.MouseOver.Text = factory.GetColor(0xFFFFFF);

            return rt;
        }

        public static Office2007RibbonTabItemColorTable GetRibbonTabItemBlackMagenta(ColorFactory factory)
        {
            Office2007RibbonTabItemColorTable rt = new Office2007RibbonTabItemColorTable();
            rt.Default.Text = factory.GetColor(0xFFFFFF);
            // Selected Tab
            rt.Selected = new Office2007RibbonTabItemStateColorTable();
            rt.Selected.Background = new LinearGradientColorTable(factory.GetColor(0xD4C8E2), factory.GetColor(0xEBE5F2));
            rt.Selected.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
            rt.Selected.InnerBorder = new LinearGradientColorTable(factory.GetColor(0xE1D9EB), factory.GetColor(0xCDC2DF));
            rt.Selected.OuterBorder = new LinearGradientColorTable(factory.GetColor(0xBCB9C1), factory.GetColor(0xC2C2C2));
            rt.Selected.Text = factory.GetColor(0x000000);

            // Selected Tab Mouse Over
            rt.SelectedMouseOver = new Office2007RibbonTabItemStateColorTable();
            rt.SelectedMouseOver.Background = new LinearGradientColorTable(factory.GetColor(0xD4C8E2), factory.GetColor(0xEBE5F2));
            rt.SelectedMouseOver.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
            rt.SelectedMouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(0xC6B6D9), factory.GetColor(0xCDC2DF));
            rt.SelectedMouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(0xBCB9C1), factory.GetColor(0xC2C2C2));
            rt.SelectedMouseOver.Text = factory.GetColor(0x000000);

            // Tab Mouse Over
            rt.MouseOver = new Office2007RibbonTabItemStateColorTable();
            rt.MouseOver.Background = new LinearGradientColorTable(factory.GetColor(0x8B8D91), factory.GetColor(0x7F7495));
            rt.MouseOver.BackgroundHighlight = new LinearGradientColorTable(factory.GetColor(0x89769D), Color.Transparent);
            rt.MouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(0x8A8B8F), factory.GetColor(0x7F7495));
            rt.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(0x99989A), factory.GetColor(0xA09EA2));
            rt.MouseOver.Text = factory.GetColor(0xFFFFFF);
            return rt;
        }

        public static Office2007RibbonTabItemColorTable GetRibbonTabItemBlackGreen(ColorFactory factory)
        {
            Office2007RibbonTabItemColorTable rt = new Office2007RibbonTabItemColorTable();
            rt.Default.Text = factory.GetColor(0xFFFFFF);

            // Selected Tab
            rt.Selected = new Office2007RibbonTabItemStateColorTable();
            rt.Selected.Background = new LinearGradientColorTable(factory.GetColor(0xC1E2B8), factory.GetColor(0xE3F1DD));
            rt.Selected.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
            rt.Selected.InnerBorder = new LinearGradientColorTable(factory.GetColor(0xE4F3E0), factory.GetColor(0xB8DEB1));
            rt.Selected.OuterBorder = new LinearGradientColorTable(factory.GetColor(0xB0C0AC), factory.GetColor(0xC2C2C2));
            rt.Selected.Text = factory.GetColor(0x000000);

            // Selected Tab Mouse Over
            rt.SelectedMouseOver.Background = new LinearGradientColorTable(factory.GetColor(0xC1E2B8), factory.GetColor(0xE3F1DD));
            rt.SelectedMouseOver.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
            rt.SelectedMouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(0xA4D496), factory.GetColor(0xB8DEB1));
            rt.SelectedMouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(0xB0C0AC), factory.GetColor(0xC2C2C2));
            rt.SelectedMouseOver.Text = factory.GetColor(0x000000);

            // Tab Mouse Over
            rt.MouseOver = new Office2007RibbonTabItemStateColorTable();
            rt.MouseOver.Background = new LinearGradientColorTable(factory.GetColor(0x899087), factory.GetColor(0x659258));
            rt.MouseOver.BackgroundHighlight = new LinearGradientColorTable(factory.GetColor(0x6C9D5C), Color.Transparent);
            rt.MouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(0x878C86), factory.GetColor(0x659257));
            rt.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(0x979997), factory.GetColor(0x9DA39B));
            rt.MouseOver.Text = factory.GetColor(0xFFFFFF);
            return rt;
        }

        public static Office2007RibbonTabItemColorTable GetRibbonTabItemBlackOrange(ColorFactory factory)
        {
            Office2007RibbonTabItemColorTable rt = new Office2007RibbonTabItemColorTable();
            rt.Default.Text = factory.GetColor(0xFFFFFF);

            // Selected Tab
            rt.Selected = new Office2007RibbonTabItemStateColorTable();
            rt.Selected.Background = new LinearGradientColorTable(factory.GetColor(0xFFF59F), factory.GetColor(0xFFFAD5));
            rt.Selected.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
            rt.Selected.InnerBorder = new LinearGradientColorTable(factory.GetColor(0xFFFBD6), factory.GetColor(0xFCF395));
            rt.Selected.OuterBorder = new LinearGradientColorTable(factory.GetColor(0xCAC7AB), factory.GetColor(0xC2C2C2));
            rt.Selected.Text = factory.GetColor(0x000000);

            // Selected Tab Mouse Over
            rt.SelectedMouseOver = new Office2007RibbonTabItemStateColorTable();
            rt.SelectedMouseOver.Background = new LinearGradientColorTable(factory.GetColor(0xFFF59F), factory.GetColor(0xFFFAD5));
            rt.SelectedMouseOver.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
            rt.SelectedMouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(0xFFF071), factory.GetColor(0xFCF395));
            rt.SelectedMouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(0xCAC7AB), factory.GetColor(0xC2C2C2));
            rt.SelectedMouseOver.Text = factory.GetColor(0x000000);

            // Tab Mouse Over
            rt.MouseOver = new Office2007RibbonTabItemStateColorTable();
            rt.MouseOver.Background = new LinearGradientColorTable(factory.GetColor(0x929083), factory.GetColor(0xB6A933));
            rt.MouseOver.BackgroundHighlight = new LinearGradientColorTable(factory.GetColor(0xC2B531), Color.Transparent);
            rt.MouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(0x8F8D83), factory.GetColor(0xB1A433));
            rt.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(0x9C9B96), factory.GetColor(0xBEBEBE));
            rt.MouseOver.Text = factory.GetColor(0xFFFFFF);

            return rt;
        }
        #endregion

        #region ButtonItem
        public static Office2007ButtonItemColorTable GetMenuItemBlue(bool p, ColorFactory factory)
        {
            Office2007ButtonItemColorTable cb = new Office2007ButtonItemColorTable();
            cb.Default = new Office2007ButtonItemStateColorTable();
            
            cb.Default.Text = factory.GetColor(0x000000);

            // Button mouse over
            cb.MouseOver = new Office2007ButtonItemStateColorTable();
            cb.MouseOver.Background = new LinearGradientColorTable(factory.GetColor(0xE5F3FA), factory.GetColor(0xD0ECFC), 90);
            cb.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(0xB3E4F9));
            cb.MouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(0xF0F7FB), factory.GetColor(0xE3F3FC));
            cb.MouseOver.SplitBorder = new LinearGradientColorTable(factory.GetColor(0xBEBEBE));
            cb.MouseOver.SplitBorderLight = new LinearGradientColorTable(factory.GetColor(0xDBFDF9));
            cb.MouseOver.Text = factory.GetColor(0x000000);

            // Pressed
            cb.Pressed = new Office2007ButtonItemStateColorTable();
            cb.Pressed.OuterBorder = new LinearGradientColorTable(factory.GetColor(0x2C628B));
            cb.Pressed.InnerBorder = new LinearGradientColorTable(factory.GetColor(0x9EB0BA), Color.Transparent);
            cb.Pressed.TopBackground = new LinearGradientColorTable(factory.GetColor(0xE5F4FC), factory.GetColor(0xC4E5F6));
            cb.Pressed.TopBackgroundHighlight = new LinearGradientColorTable();
            cb.Pressed.BottomBackground = new LinearGradientColorTable(factory.GetColor(0x98D1EF), factory.GetColor(0x68B3DB));
            cb.Pressed.BottomBackgroundHighlight = new LinearGradientColorTable();
            cb.Pressed.SplitBorder = new LinearGradientColorTable(factory.GetColor(0xBEBEBE));
            cb.Pressed.SplitBorderLight = new LinearGradientColorTable(factory.GetColor(0xDBFDF9));
            cb.Pressed.Text = factory.GetColor(0x000000);

            // Checked
            cb.Checked = new Office2007ButtonItemStateColorTable();
            cb.Checked.OuterBorder = new LinearGradientColorTable(factory.GetColor(0x3C7FB1));
            cb.Checked.InnerBorder = new LinearGradientColorTable(Color.FromArgb(96, Color.White));
            cb.Checked.TopBackground = new LinearGradientColorTable(factory.GetColor(0xEBF5FB), factory.GetColor(0xE5F2FA));
            cb.Checked.TopBackgroundHighlight = new LinearGradientColorTable();
            cb.Checked.BottomBackground = new LinearGradientColorTable(factory.GetColor(0xCBE6F5), factory.GetColor(0xB9DCF0));
            cb.Checked.BottomBackgroundHighlight = new LinearGradientColorTable();
            cb.Checked.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
            cb.Checked.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
            cb.Checked.Text = factory.GetColor(0x000000);

            // Expanded button
            cb.Expanded = new Office2007ButtonItemStateColorTable();
            cb.Expanded.OuterBorder = new LinearGradientColorTable(factory.GetColor(0x3C7FB1));
            cb.Expanded.InnerBorder = new LinearGradientColorTable(Color.FromArgb(96, Color.White));
            cb.Expanded.TopBackground = new LinearGradientColorTable(factory.GetColor(0xEBF5FB), factory.GetColor(0xE5F2FA));
            cb.Expanded.TopBackgroundHighlight = new LinearGradientColorTable();
            cb.Expanded.BottomBackground = new LinearGradientColorTable(factory.GetColor(0xCBE6F5), factory.GetColor(0xB9DCF0));
            cb.Expanded.BottomBackgroundHighlight = new LinearGradientColorTable();
            cb.Expanded.SplitBorder = new LinearGradientColorTable(factory.GetColor(0xBEBEBE));
            cb.Expanded.SplitBorderLight = new LinearGradientColorTable(factory.GetColor(0xDBFDF9));
            cb.Expanded.Text = factory.GetColor(0x000000);

            SetBlackExpandColors(cb, factory);
            return cb;
        }

        public static Office2007ButtonItemColorTable GetButtonItemBlackOrange(bool ribbonBar, ColorFactory factory)
        {
            Office2007ButtonItemColorTable cb = new Office2007ButtonItemColorTable();
            cb.Default = new Office2007ButtonItemStateColorTable();
            //if(ribbonBar)
            cb.Default.Text = factory.GetColor(0x000000);  //factory.GetColor(0x464646);
            //else
            //    cb.Default.Text = factory.GetColor(0xFFFFFF);

            // Button mouse over
            //cb.MouseOver = new Office2007ButtonItemStateColorTable();
            //cb.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(0x333333), factory.GetColor(0x000000));
            //cb.MouseOver.InnerBorder = new LinearGradientColorTable(Color.FromArgb(128, Color.White));
            //cb.MouseOver.TopBackground = new LinearGradientColorTable(factory.GetColor(0xACABA8), factory.GetColor(0x8F7E5D));
            //cb.MouseOver.TopBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(255, factory.GetColor(0xAA8F5C)), Color.Transparent);
            //cb.MouseOver.BottomBackground = new LinearGradientColorTable(factory.GetColor(0x1F1200), factory.GetColor(0x623C25));
            //cb.MouseOver.BottomBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(164, factory.GetColor(0xDFD44E)), Color.Transparent);
            //cb.MouseOver.SplitBorder = new LinearGradientColorTable(factory.GetColor(0x333333));
            //cb.MouseOver.SplitBorderLight = new LinearGradientColorTable(factory.GetColor(0x9A9890));
            //cb.MouseOver.Text = factory.GetColor(0xFFFFFF);
            cb.MouseOver = new Office2007ButtonItemStateColorTable();
            cb.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(0xA5A5A5), factory.GetColor(0x959595));
            cb.MouseOver.InnerBorder = new LinearGradientColorTable(Color.FromArgb(128, Color.White));
            cb.MouseOver.TopBackground = new LinearGradientColorTable(factory.GetColor(0xE9F7FE), factory.GetColor(0xDDF2FD));
            cb.MouseOver.TopBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(255, factory.GetColor(0xDEF3FB)), Color.Transparent);
            cb.MouseOver.BottomBackground = new LinearGradientColorTable(factory.GetColor(0x95BED5), factory.GetColor(0xB5E4FF));
            cb.MouseOver.BottomBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(164, factory.GetColor(0xD3FFFF)), Color.Transparent);
            cb.MouseOver.SplitBorder = new LinearGradientColorTable(factory.GetColor(0xBEBEBE));
            cb.MouseOver.SplitBorderLight = new LinearGradientColorTable(factory.GetColor(0xDBFDF9));
            cb.MouseOver.Text = factory.GetColor(0x000000);

            // Pressed
            cb.Pressed = new Office2007ButtonItemStateColorTable();
            cb.Pressed.OuterBorder = new LinearGradientColorTable(factory.GetColor(0xA5A5A5), factory.GetColor(0x959595));
            cb.Pressed.InnerBorder = new LinearGradientColorTable(Color.FromArgb(128, Color.White));
            cb.Pressed.TopBackground = new LinearGradientColorTable(factory.GetColor(0xE9F7FE), factory.GetColor(0xC5E9FB));
            cb.Pressed.TopBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
            cb.Pressed.BottomBackground = new LinearGradientColorTable(factory.GetColor(0x649EBF), factory.GetColor(0x87D3FF));
            cb.Pressed.BottomBackgroundHighlight = new LinearGradientColorTable(factory.GetColor(0xD3FFFF), Color.Transparent);
            cb.Pressed.SplitBorder = new LinearGradientColorTable(factory.GetColor(0xBEBEBE));
            cb.Pressed.SplitBorderLight = new LinearGradientColorTable(factory.GetColor(0xDBFDF9));
            cb.Pressed.Text = factory.GetColor(0x000000);

            // Checked
            //cb.Checked = new Office2007ButtonItemStateColorTable();
            //cb.Checked.OuterBorder = new LinearGradientColorTable(factory.GetColor(0x5C2C00), factory.GetColor(0x290000));
            //cb.Checked.InnerBorder = new LinearGradientColorTable(Color.FromArgb(96, Color.White));
            //cb.Checked.TopBackground = new LinearGradientColorTable(factory.GetColor(0xDBAB6E), factory.GetColor(0xA47437));
            //cb.Checked.TopBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(32, Color.White), Color.Transparent);
            //cb.Checked.BottomBackground = new LinearGradientColorTable(factory.GetColor(0x290000), factory.GetColor(0x3B0C00));
            //cb.Checked.BottomBackgroundHighlight = new LinearGradientColorTable(factory.GetColor(0xFFF59F), Color.Transparent);
            //cb.Checked.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
            //cb.Checked.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
            //cb.Checked.Text = factory.GetColor(0xFFFFFF);
            cb.Checked = new Office2007ButtonItemStateColorTable();
            cb.Checked.OuterBorder = new LinearGradientColorTable(factory.GetColor(0x8F8E8E), factory.GetColor(0x636363));
            cb.Checked.InnerBorder = new LinearGradientColorTable(Color.FromArgb(96, Color.White));
            cb.Checked.TopBackground = new LinearGradientColorTable(factory.GetColor(0xF0F0F0), factory.GetColor(0xC8CAC8));
            cb.Checked.TopBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(32, Color.White), Color.Transparent);
            cb.Checked.BottomBackground = new LinearGradientColorTable(factory.GetColor(0xB7B5B5), factory.GetColor(0xBEC1BE));
            cb.Checked.BottomBackgroundHighlight = new LinearGradientColorTable(factory.GetColor(0xECECEC), Color.Transparent);
            cb.Checked.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
            cb.Checked.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
            cb.Checked.Text = factory.GetColor(0x000000);

            // Expanded button
            cb.Expanded = new Office2007ButtonItemStateColorTable();
            cb.Expanded.OuterBorder = new LinearGradientColorTable(factory.GetColor(0xA5A5A5), factory.GetColor(0x636363));
            cb.Expanded.InnerBorder = new LinearGradientColorTable(Color.FromArgb(96, Color.White));
            cb.Expanded.TopBackground = new LinearGradientColorTable(factory.GetColor(0xE9F7FE), factory.GetColor(0xDDF2FD));
            cb.Expanded.TopBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(128, Color.White), Color.Transparent);
            cb.Expanded.BottomBackground = new LinearGradientColorTable(factory.GetColor(0x95BED5), factory.GetColor(0xB5E4FF));
            cb.Expanded.BottomBackgroundHighlight = new LinearGradientColorTable(factory.GetColor(0xDFF2FD), Color.Transparent);
            cb.Expanded.SplitBorder = new LinearGradientColorTable(factory.GetColor(0xBEBEBE));
            cb.Expanded.SplitBorderLight = new LinearGradientColorTable(factory.GetColor(0xDBFDF9));
            cb.Expanded.Text = factory.GetColor(0x000000);

            SetBlackExpandColors(cb, factory);
            return cb;
        }

        public static Office2007ButtonItemColorTable GetButtonItemBlackOrangeWithBackground(ColorFactory factory)
        {
            Office2007ButtonItemColorTable cb = GetButtonItemBlackOrange(false, factory);
            cb.Default.TopBackground = new LinearGradientColorTable(factory.GetColor(0xFFFFFF), factory.GetColor(0xDBDBDB));
            cb.Default.TopBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(228, Color.White), Color.Empty);
            cb.Default.BottomBackground = new LinearGradientColorTable(factory.GetColor(0xDBDBDB), factory.GetColor(0xCDCFD2));
            cb.Default.BottomBackgroundHighlight = new LinearGradientColorTable(Color.White, Color.Transparent);
            cb.Default.InnerBorder = new LinearGradientColorTable(Color.FromArgb(128, Color.White));
            cb.Default.OuterBorder = new LinearGradientColorTable(factory.GetColor(0x7D7D7D), factory.GetColor(0x616161));
            cb.Default.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
            cb.Default.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);

            // Copy of the default theme
            cb.Disabled = new Office2007ButtonItemStateColorTable();
            cb.Disabled.TopBackground = new LinearGradientColorTable(factory.GetColor(0xCED3DA), factory.GetColor(0xC1C6CF));
            cb.Disabled.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
            cb.Disabled.BottomBackground = new LinearGradientColorTable(factory.GetColor(0xB4BBC5), factory.GetColor(0xE5ECEC));
            cb.Disabled.BottomBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
            cb.Disabled.InnerBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
            cb.Disabled.OuterBorder = new LinearGradientColorTable(factory.GetColor(0xC5D1DE), Color.Empty);
            cb.Disabled.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
            cb.Disabled.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
            cb.Disabled.Text = factory.GetColor(0x8D8D8D);
            return cb;
        }

        public static Office2007ButtonItemColorTable GetButtonItemBlackBlue(bool ribbonBar, ColorFactory factory)
        {
            Office2007ButtonItemColorTable cb = new Office2007ButtonItemColorTable();
            cb.Default = new Office2007ButtonItemStateColorTable();
            //if (ribbonBar)
            cb.Default.Text = factory.GetColor(0xFFFFFF);  //factory.GetColor(0x15428B);
            //else
            //    cb.Default.Text = factory.GetColor(0xFFFFFF);

            // Button mouse over
            cb.MouseOver = new Office2007ButtonItemStateColorTable();
            cb.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(Color.FromArgb(128, Color.White)), Color.Empty);
            cb.MouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(0x000000), factory.GetColor(0x153158));
            cb.MouseOver.TopBackground = new LinearGradientColorTable(factory.GetColor(0x9398AA), factory.GetColor(0x373E4A));
            cb.MouseOver.TopBackgroundHighlight = new LinearGradientColorTable();
            cb.MouseOver.BottomBackground = new LinearGradientColorTable(factory.GetColor(0x111520), factory.GetColor(0x566E91));
            cb.MouseOver.BottomBackgroundHighlight = new LinearGradientColorTable(factory.GetColor(0x3C81CE), Color.Transparent);
            cb.MouseOver.SplitBorder = new LinearGradientColorTable(factory.GetColor(0x000000));
            cb.MouseOver.SplitBorderLight = new LinearGradientColorTable(factory.GetColor(Color.FromArgb(128, Color.White)));
            cb.MouseOver.Text = factory.GetColor(0xFFFFFF);

            // Pressed
            cb.Pressed = new Office2007ButtonItemStateColorTable();
            cb.Pressed.OuterBorder = new LinearGradientColorTable(factory.GetColor(Color.FromArgb(96, Color.White)), Color.Empty);
            cb.Pressed.InnerBorder = new LinearGradientColorTable(factory.GetColor(0x000000), factory.GetColor(0x153158));
            cb.Pressed.TopBackground = new LinearGradientColorTable(factory.GetColor(0x505976), factory.GetColor(0x344560));
            cb.Pressed.TopBackgroundHighlight = new LinearGradientColorTable();
            cb.Pressed.BottomBackground = new LinearGradientColorTable(factory.GetColor(0x0F0F14), factory.GetColor(0x192C4C));
            cb.Pressed.BottomBackgroundHighlight = new LinearGradientColorTable(factory.GetColor(0x3176D4), Color.Transparent);
            cb.Pressed.SplitBorder = new LinearGradientColorTable(factory.GetColor(0x000000));
            cb.Pressed.SplitBorderLight = new LinearGradientColorTable(factory.GetColor(Color.FromArgb(96, Color.White)));
            cb.Pressed.Text = factory.GetColor(0xFFFFFF);

            // Checked
            cb.Checked = new Office2007ButtonItemStateColorTable();
            cb.Checked.OuterBorder = new LinearGradientColorTable(factory.GetColor(Color.FromArgb(64, Color.White)), Color.Empty);
            cb.Checked.InnerBorder = new LinearGradientColorTable(factory.GetColor(0x000000));
            cb.Checked.TopBackground = new LinearGradientColorTable(factory.GetColor(0x759BE1), factory.GetColor(0x5B78BF));
            cb.Checked.TopBackgroundHighlight = new LinearGradientColorTable();
            cb.Checked.BottomBackground = new LinearGradientColorTable(factory.GetColor(0x173499), factory.GetColor(0x299FD2));
            cb.Checked.BottomBackgroundHighlight = new LinearGradientColorTable(factory.GetColor(0x57C6EF), Color.Transparent);
            cb.Checked.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
            cb.Checked.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
            cb.Checked.Text = factory.GetColor(0xFFFFFF);

            // Expanded button
            cb.Expanded = new Office2007ButtonItemStateColorTable();
            cb.Expanded.OuterBorder = new LinearGradientColorTable(factory.GetColor(Color.FromArgb(96, Color.White)), Color.Empty);
            cb.Expanded.InnerBorder = new LinearGradientColorTable(factory.GetColor(0x000000), factory.GetColor(0x162F52));
            cb.Expanded.TopBackground = new LinearGradientColorTable(factory.GetColor(0x505976), factory.GetColor(0x344B6E));
            cb.Expanded.TopBackgroundHighlight = new LinearGradientColorTable();
            cb.Expanded.BottomBackground = new LinearGradientColorTable(factory.GetColor(0x10141D), factory.GetColor(0x1D355C));
            cb.Expanded.BottomBackgroundHighlight = new LinearGradientColorTable(factory.GetColor(0x3278D7), Color.Transparent);
            cb.Expanded.SplitBorder = new LinearGradientColorTable(factory.GetColor(0x000000));
            cb.Expanded.SplitBorderLight = new LinearGradientColorTable(factory.GetColor(Color.FromArgb(96, Color.White)));
            cb.Expanded.Text = factory.GetColor(0xFFFFFF);


            //// Menu mouse over
            //cb.MenuMouseOver = new Office2007ButtonItemStateColorTable();
            //cb.MenuMouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(0xAAC2E0), factory.GetColor(0x6591CD));
            //cb.MenuMouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(0xFFFFFB), factory.GetColor(0xE0EEFF));
            //cb.MenuMouseOver.TopBackground = new LinearGradientColorTable(factory.GetColor(0xECF4FF), factory.GetColor(0xB0D3FF));
            //cb.MenuMouseOver.TopBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(192, Color.White), Color.Transparent);
            //cb.MenuMouseOver.BottomBackground = new LinearGradientColorTable(factory.GetColor(0x89BDFF), factory.GetColor(0x2983FF));
            //cb.MenuMouseOver.BottomBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(164, Color.White), Color.Transparent);
            //cb.MenuMouseOver.SplitBorder = new LinearGradientColorTable(factory.GetColor(0x7495C2), factory.GetColor(0x90AFD6));
            //cb.MenuMouseOver.SplitBorderLight = new LinearGradientColorTable(factory.GetColor(0xD6E3F3), factory.GetColor(0x83A5D0));
            //cb.MenuMouseOver.Text = factory.GetColor(0x15428B);
            SetBlackExpandColors(cb, factory);
            return cb;
        }

        public static Office2007ButtonItemColorTable GetButtonItemBlackBlueWithBackground(ColorFactory factory)
        {
            Office2007ButtonItemColorTable cb = GetButtonItemBlackBlue(false, factory);
            cb.Default.TopBackground = new LinearGradientColorTable(factory.GetColor(0x626C88), factory.GetColor(0x3F4756));
            cb.Default.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
            cb.Default.BottomBackground = new LinearGradientColorTable(factory.GetColor(0x151516), factory.GetColor(0x3C476F));
            cb.Default.BottomBackgroundHighlight = new LinearGradientColorTable();
            cb.Default.InnerBorder = new LinearGradientColorTable(factory.GetColor(0x7A8295), factory.GetColor(0x646C89));
            cb.Default.OuterBorder = new LinearGradientColorTable(factory.GetColor(0x566976), factory.GetColor(0x000000));
            cb.Default.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
            cb.Default.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
            cb.Default.Text = Color.White;
            //cb.Default.TopBackground = new LinearGradientColorTable(factory.GetColor(0xFFFFFF), factory.GetColor(0xE0E5EB));
            //cb.Default.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
            //cb.Default.BottomBackground = new LinearGradientColorTable(factory.GetColor(0xCBD5DF), factory.GetColor(0xFFFFFF));
            //cb.Default.BottomBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(128, Color.White), Color.Transparent);
            //cb.Default.InnerBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
            //cb.Default.OuterBorder = new LinearGradientColorTable(factory.GetColor(0x898785), Color.Empty);
            //cb.Default.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
            //cb.Default.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);

            // Copy of the default theme
            cb.Disabled = new Office2007ButtonItemStateColorTable();
            cb.Disabled.TopBackground = new LinearGradientColorTable(factory.GetColor(0x626C88), factory.GetColor(0x3F4756));
            cb.Disabled.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
            cb.Disabled.BottomBackground = new LinearGradientColorTable(factory.GetColor(0x3F4756));
            cb.Disabled.BottomBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
            cb.Disabled.InnerBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
            cb.Disabled.OuterBorder = new LinearGradientColorTable(factory.GetColor(0xC5D1DE), Color.Empty);
            cb.Disabled.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
            cb.Disabled.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
            cb.Disabled.Text = factory.GetColor(0xC6C6C6);
            return cb;
        }

        public static Office2007ButtonItemColorTable GetButtonItemBlackMagenta(bool ribbonBar, ColorFactory factory)
        {
            Office2007ButtonItemColorTable cb = new Office2007ButtonItemColorTable();
            cb.Default = new Office2007ButtonItemStateColorTable();
            cb.Default.Text = factory.GetColor(0x000000);

            // Button mouse over
            cb.MouseOver = new Office2007ButtonItemStateColorTable();
            cb.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(0x6B0060), factory.GetColor(0x56014D));
            cb.MouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(0xECE7F3), factory.GetColor(0xE8D6FF));
            cb.MouseOver.TopBackground = new LinearGradientColorTable(factory.GetColor(0xF2DFEB), factory.GetColor(0xE6C6DB));
            cb.MouseOver.TopBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(96, Color.White), Color.Transparent);
            cb.MouseOver.BottomBackground = new LinearGradientColorTable(factory.GetColor(0xEAA8D4), factory.GetColor(0xE181C1));
            cb.MouseOver.BottomBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(164, Color.White), Color.Transparent);
            cb.MouseOver.SplitBorder = new LinearGradientColorTable(factory.GetColor(0xA37BD1), factory.GetColor(0x8868AE));
            cb.MouseOver.SplitBorderLight = new LinearGradientColorTable(factory.GetColor(0xD9C2F3), factory.GetColor(0xBBA6D3));
            cb.MouseOver.Text = factory.GetColor(0x000000);

            // Pressed
            cb.Pressed = new Office2007ButtonItemStateColorTable();
            cb.Pressed.OuterBorder = new LinearGradientColorTable(factory.GetColor(0x6B0060), factory.GetColor(0x56014D));
            cb.Pressed.InnerBorder = new LinearGradientColorTable(factory.GetColor(0xECE7F3), factory.GetColor(0xE8D6FF));
            cb.Pressed.TopBackground = new LinearGradientColorTable(factory.GetColor(0xD2B0C6), factory.GetColor(0xCBADC1));
            cb.Pressed.TopBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
            cb.Pressed.BottomBackground = new LinearGradientColorTable(factory.GetColor(0xD27AB5), factory.GetColor(0xC84C9F));
            cb.Pressed.BottomBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(184, Color.White), Color.Transparent);
            cb.Pressed.SplitBorder = new LinearGradientColorTable(factory.GetColor(0xA37BD1), factory.GetColor(0x8868AE));
            cb.Pressed.SplitBorderLight = new LinearGradientColorTable(factory.GetColor(0xD9C2F3), factory.GetColor(0xBBA6D3));
            cb.Pressed.Text = factory.GetColor(0x000000);

            // Checked
            cb.Checked = new Office2007ButtonItemStateColorTable();
            cb.Checked.OuterBorder = new LinearGradientColorTable(factory.GetColor(0x6B0060), factory.GetColor(0x56014D));
            cb.Checked.InnerBorder = new LinearGradientColorTable(factory.GetColor(0xECE7F3), factory.GetColor(0xE8D6FF));
            cb.Checked.TopBackground = new LinearGradientColorTable(factory.GetColor(0xE9D7E2), factory.GetColor(0xE2B0D1));
            cb.Checked.TopBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(164, Color.White), Color.Transparent);
            cb.Checked.BottomBackground = new LinearGradientColorTable(factory.GetColor(0xCF90BA), factory.GetColor(0xCF73B0));
            cb.Checked.BottomBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(92, Color.White), Color.Transparent);
            cb.Checked.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
            cb.Checked.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
            cb.Checked.Text = factory.GetColor(0x000000);

            // Expanded button
            cb.Expanded = new Office2007ButtonItemStateColorTable();
            cb.Expanded.OuterBorder = new LinearGradientColorTable(factory.GetColor(0x6B0060), factory.GetColor(0x56014D));
            cb.Expanded.InnerBorder = new LinearGradientColorTable(factory.GetColor(0xECE7F3), factory.GetColor(0xE8D6FF));
            cb.Expanded.TopBackground = new LinearGradientColorTable(factory.GetColor(0xE9D7E2), factory.GetColor(0xE2B0D1));
            cb.Expanded.TopBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(192, Color.White), Color.Transparent);
            cb.Expanded.BottomBackground = new LinearGradientColorTable(factory.GetColor(0xCF90BA), factory.GetColor(0xCF73B0));
            cb.Expanded.BottomBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(192, Color.White), Color.Transparent);
            cb.Expanded.SplitBorder = new LinearGradientColorTable(factory.GetColor(0xA37BD1), factory.GetColor(0x8868AE));
            cb.Expanded.SplitBorderLight = new LinearGradientColorTable(factory.GetColor(0xD9C2F3), factory.GetColor(0xBBA6D3));
            cb.Expanded.Text = factory.GetColor(0x000000);


            //// Menu mouse over
            //cb.MenuMouseOver = new Office2007ButtonItemStateColorTable();
            //cb.MenuMouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(0xDE9ECC), factory.GetColor(0xBD71A8));
            //cb.MenuMouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(0xECE7F3), factory.GetColor(0xE8D6FF));
            //cb.MenuMouseOver.TopBackground = new LinearGradientColorTable(factory.GetColor(0xF3EAFF), factory.GetColor(0xDBBCFF));
            //cb.MenuMouseOver.TopBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(192, Color.White), Color.Transparent);
            //cb.MenuMouseOver.BottomBackground = new LinearGradientColorTable(factory.GetColor(0xAF6BFF), factory.GetColor(0xD3AEFF));
            //cb.MenuMouseOver.BottomBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(164, Color.White), Color.Transparent);
            //cb.MenuMouseOver.SplitBorder = new LinearGradientColorTable(factory.GetColor(0xA37BD1), factory.GetColor(0x8868AE));
            //cb.MenuMouseOver.SplitBorderLight = new LinearGradientColorTable(factory.GetColor(0xD9C2F3), factory.GetColor(0xBBA6D3));
            //cb.MenuMouseOver.Text = factory.GetColor(0x15428B);
            SetBlackExpandColors(cb, factory);

            return cb;
        }

        public static Office2007ButtonItemColorTable GetButtonItemBlackMagentaWithBackground(ColorFactory factory)
        {
            Office2007ButtonItemColorTable cb = GetButtonItemBlackMagenta(false, factory);
            cb.Default.TopBackground = new LinearGradientColorTable(factory.GetColor(0xF8F8F9), factory.GetColor(0xDFE2E4));
            cb.Default.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
            cb.Default.BottomBackground = new LinearGradientColorTable(factory.GetColor(0xC7CBD1), factory.GetColor(0xDBDEE2));
            cb.Default.BottomBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(128, Color.White), Color.Transparent);
            cb.Default.InnerBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
            cb.Default.OuterBorder = new LinearGradientColorTable(factory.GetColor(0x898785), Color.Empty);
            cb.Default.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
            cb.Default.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);

            // Copy of the default theme
            cb.Disabled = new Office2007ButtonItemStateColorTable();
            cb.Disabled.TopBackground = new LinearGradientColorTable(factory.GetColor(0xCED3DA), factory.GetColor(0xC1C6CF));
            cb.Disabled.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
            cb.Disabled.BottomBackground = new LinearGradientColorTable(factory.GetColor(0xB4BBC5), factory.GetColor(0xE5ECEC));
            cb.Disabled.BottomBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
            cb.Disabled.InnerBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
            cb.Disabled.OuterBorder = new LinearGradientColorTable(factory.GetColor(0x4B4B4B), Color.Empty);
            cb.Disabled.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
            cb.Disabled.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
            cb.Disabled.Text = factory.GetColor(0x8D8D8D);
            return cb;
        }
        #endregion

        #region RibbonTabItemGroup
        //public static Office2007RibbonTabGroupColorTable GetRibbonTabGroupBlackDefault(ColorFactory factory)
        //{
        //    Office2007RibbonTabGroupColorTable tg = new Office2007RibbonTabGroupColorTable();
        //    tg.Background = new LinearGradientColorTable(Color.Transparent /*factory.GetColor(0x3C3B3C)*/, factory.GetColor(0xA0A0A0));
        //    tg.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, factory.GetColor(0xCDCDCD)), Color.Transparent);
        //    tg.Text = factory.GetColor(0xFFFFFF);
        //    tg.Border = new LinearGradientColorTable(Color.FromArgb(16, Color.DarkGray), Color.FromArgb(192, Color.DarkGray));

        //    return tg;
        //}

        //public static Office2007RibbonTabGroupColorTable GetRibbonTabGroupBlackMagenta(ColorFactory factory)
        //{
        //    Office2007RibbonTabGroupColorTable tg = new Office2007RibbonTabGroupColorTable();
        //    tg.Background = new LinearGradientColorTable(Color.Transparent /*factory.GetColor(0x3C3B3C)*/, factory.GetColor(0x90708F));
        //    tg.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, factory.GetColor(0xAF7DA2)), Color.Transparent);
        //    tg.Text = factory.GetColor(0xFFFFFF);
        //    tg.Border = new LinearGradientColorTable(Color.FromArgb(16, Color.DarkGray), Color.FromArgb(192, Color.DarkGray));

        //    return tg;
        //}

        //public static Office2007RibbonTabGroupColorTable GetRibbonTabGroupBlackGreen(ColorFactory factory)
        //{
        //    Office2007RibbonTabGroupColorTable tg = new Office2007RibbonTabGroupColorTable();

        //    tg.Background = new LinearGradientColorTable(Color.Transparent /*factory.GetColor(0x3A3D3B)*/, factory.GetColor(0x6E9F5D));
        //    tg.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, factory.GetColor(0x6BB954)), Color.Transparent);
        //    tg.Border = new LinearGradientColorTable(Color.FromArgb(16, Color.DarkGray), Color.FromArgb(192, Color.DarkGray));
        //    tg.Text = factory.GetColor(0xFFFFFF);

        //    return tg;
        //}

        //public static Office2007RibbonTabGroupColorTable GetRibbonTabGroupBlackOrange(ColorFactory factory)
        //{
        //    Office2007RibbonTabGroupColorTable tg = new Office2007RibbonTabGroupColorTable();

        //    tg.Background = new LinearGradientColorTable(Color.Transparent /*factory.GetColor(0x414038)*/, factory.GetColor(0xA69B41));
        //    tg.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, factory.GetColor(0xFFE619)), Color.Transparent);
        //    tg.Border = new LinearGradientColorTable(Color.FromArgb(16, Color.DarkGray), Color.FromArgb(192, Color.DarkGray));
        //    tg.Text = factory.GetColor(0xFFFFFF);

        //    return tg;
        //}
        #endregion

        #region Scroll Bar
        public static void InitializeScrollBarColorTable(Office2007ColorTable t, ColorFactory factory)
        {
            Office2007ScrollBarStateColorTable sct = t.ScrollBar.Default;
            sct.Background = new LinearGradientColorTable(factory.GetColor(0xFCFCFC), factory.GetColor(0xF0F0F0), 0);
            sct.Border = new LinearGradientColorTable(factory.GetColor(0xEBEDEF), Color.Empty);
            sct.ThumbSignBackground = new LinearGradientColorTable(factory.GetColor(0x6E7EA6), factory.GetColor(0x424B63), 90);
            sct.TrackBackground.Clear();
            sct.TrackBackground.AddRange(new BackgroundColorBlend[] { new BackgroundColorBlend(factory.GetColor(0xEBEEF0),0f), 
                new BackgroundColorBlend(factory.GetColor(0xE6E9F0),0.5f),
                new BackgroundColorBlend(factory.GetColor(0xD1DAE4),0.5f),
                new BackgroundColorBlend(factory.GetColor(0xBECADB),1f)});
            sct.TrackInnerBorder = new LinearGradientColorTable(factory.GetColor(0xE8E9E9), factory.GetColor(0xE9ECF2));
            sct.TrackOuterBorder = new LinearGradientColorTable(factory.GetColor(0x5F6E93), Color.Empty);
            sct.TrackSignBackground = new LinearGradientColorTable(Color.FromArgb(180, factory.GetColor(0x606060)), Color.FromArgb(64, Color.White));

            // Mouse Over
            sct = t.ScrollBar.MouseOver;
            sct.Background = t.ScrollBar.Default.Background;
            sct.Border = t.ScrollBar.Default.Border;
            sct.ThumbBackground.Clear();
            sct.ThumbBackground.AddRange(new BackgroundColorBlend[] { new BackgroundColorBlend(factory.GetColor(0xBFD1EA),0f), 
                new BackgroundColorBlend(factory.GetColor(0xCADFFA),0.4f),
                new BackgroundColorBlend(factory.GetColor(0xAACBF6),0.4f),
                new BackgroundColorBlend(factory.GetColor(0xAACBF6),0.7f),
                new BackgroundColorBlend(factory.GetColor(0xD3E4FA),1f)});
            sct.ThumbInnerBorder = new LinearGradientColorTable(factory.GetColor(0xFAFCFF), factory.GetColor(0xEEF4FC));
            sct.ThumbOuterBorder = new LinearGradientColorTable(factory.GetColor(0x8C97A5), factory.GetColor(0x66717F));
            sct.ThumbSignBackground = new LinearGradientColorTable(factory.GetColor(0x333C54), factory.GetColor(0x55658D));
            sct.TrackBackground.Clear();
            sct.TrackBackground.AddRange(new BackgroundColorBlend[] { new BackgroundColorBlend(factory.GetColor(0xBFD1EA),0f), 
                new BackgroundColorBlend(factory.GetColor(0xCADFFA),0.5f),
                new BackgroundColorBlend(factory.GetColor(0xAACBF6),0.5f),
                new BackgroundColorBlend(factory.GetColor(0xD3E4FA),1f)});
            sct.TrackInnerBorder = new LinearGradientColorTable(factory.GetColor(0xE9E9EB), factory.GetColor(0xFCFDFF));
            sct.TrackOuterBorder = new LinearGradientColorTable(factory.GetColor(0x3C6EB0), Color.Empty);
            sct.TrackSignBackground = new LinearGradientColorTable(Color.FromArgb(180, factory.GetColor(0x606060)), Color.FromArgb(64, Color.White));

            // Control Mouse Over
            sct = t.ScrollBar.MouseOverControl;
            sct.Background = t.ScrollBar.Default.Background;
            sct.Border = t.ScrollBar.Default.Border;
            sct.ThumbBackground.Clear();
            sct.ThumbBackground.AddRange(new BackgroundColorBlend[] { new BackgroundColorBlend(factory.GetColor(0xF4F9FF),0f), 
                new BackgroundColorBlend(factory.GetColor(0xF4F9FF),0.5f),
                new BackgroundColorBlend(factory.GetColor(0xCFE1F8),0.5f),
                new BackgroundColorBlend(factory.GetColor(0xDFECFC),1f)});
            sct.ThumbInnerBorder = new LinearGradientColorTable(factory.GetColor(0xFBFDFF), Color.Empty);
            sct.ThumbOuterBorder = new LinearGradientColorTable(factory.GetColor(0x8C97A5), factory.GetColor(0x66717F));
            sct.ThumbSignBackground = new LinearGradientColorTable(factory.GetColor(0x6E7EA6), factory.GetColor(0x424B63));
            sct.TrackBackground.Clear();
            sct.TrackBackground.AddRange(new BackgroundColorBlend[] { new BackgroundColorBlend(factory.GetColor(0xEBEEF0),0f), 
                new BackgroundColorBlend(factory.GetColor(0xE6E9F0),0.5f),
                new BackgroundColorBlend(factory.GetColor(0xD1DAE4),0.5f),
                new BackgroundColorBlend(factory.GetColor(0xBECADB),1f)});
            sct.TrackInnerBorder = new LinearGradientColorTable(factory.GetColor(0xE8E9E9), factory.GetColor(0xE9EDF2));
            sct.TrackOuterBorder = new LinearGradientColorTable(factory.GetColor(0x5F6E93), Color.Empty);
            sct.TrackSignBackground = new LinearGradientColorTable(Color.FromArgb(180, factory.GetColor(0x606060)), Color.FromArgb(64, Color.White));
            // Pressed
            sct = t.ScrollBar.Pressed;
            sct.Background = t.ScrollBar.Default.Background;
            sct.Border = t.ScrollBar.Default.Border;
            sct.ThumbBackground.Clear();
            sct.ThumbBackground.AddRange(new BackgroundColorBlend[] { 
                new BackgroundColorBlend(factory.GetColor(0xE6E9EC),0f), 
                new BackgroundColorBlend(factory.GetColor(0xE2E4E6),0.6f),
                new BackgroundColorBlend(factory.GetColor(0xC3CAD2),0.6f),
                new BackgroundColorBlend(factory.GetColor(0xD7DCE1),1f)});
            sct.ThumbInnerBorder = new LinearGradientColorTable(factory.GetColor(0xF5F7F8), Color.Empty);
            sct.ThumbOuterBorder = new LinearGradientColorTable(factory.GetColor(0x8C97A5), factory.GetColor(0x65707F));
            sct.ThumbSignBackground = new LinearGradientColorTable(factory.GetColor(0x6E7EA6), factory.GetColor(0x424B63));
            sct.TrackBackground.Clear();
            sct.TrackBackground.AddRange(new BackgroundColorBlend[] {
                new BackgroundColorBlend(factory.GetColor(0x9EBEE9),0f), 
                new BackgroundColorBlend(factory.GetColor(0xABCCF6),0.5f),
                new BackgroundColorBlend(factory.GetColor(0x6EA6F0),0.5f),
                new BackgroundColorBlend(factory.GetColor(0xB5D1F7),1f)});
            sct.TrackInnerBorder = new LinearGradientColorTable(factory.GetColor(0xC2D3E7), factory.GetColor(0xDDEBFB));
            sct.TrackOuterBorder = new LinearGradientColorTable(factory.GetColor(0x17498A), Color.Empty);
            sct.TrackSignBackground = new LinearGradientColorTable(Color.FromArgb(180, factory.GetColor(0x606060)), Color.FromArgb(64, Color.White));
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

        public static void InitializeAppBlueScrollBarColorTable(Office2007ColorTable t, ColorFactory factory)
        {
            Office2007ScrollBarStateColorTable sct = t.AppScrollBar.Default;
            sct.Background = new LinearGradientColorTable(factory.GetColor(0x95B0D2), factory.GetColor(0x7C9BC1), 0);
            sct.Border = new LinearGradientColorTable(factory.GetColor(0xB2D0F6));
            sct.ThumbSignBackground = new LinearGradientColorTable(factory.GetColor(0x6E7EA6), factory.GetColor(0x424B63), 90);
            sct.TrackBackground.Clear();
            sct.TrackBackground.AddRange(new BackgroundColorBlend[] { new BackgroundColorBlend(factory.GetColor(0xE3E5E7),0f), 
                new BackgroundColorBlend(factory.GetColor(0xE3E9F0),0.4f),
                new BackgroundColorBlend(factory.GetColor(0xC8D4E1),0.4f),
                new BackgroundColorBlend(factory.GetColor(0xB6C4D8),.8f),
                new BackgroundColorBlend(factory.GetColor(0xB6C4D8),.8f),
                new BackgroundColorBlend(factory.GetColor(0xE1E7EF),1f)});
            sct.TrackInnerBorder = new LinearGradientColorTable(factory.GetColor(0xE3E5E7), factory.GetColor(0xE1E7EF));
            sct.TrackOuterBorder = new LinearGradientColorTable(factory.GetColor(0x596991), Color.Empty);
            sct.TrackSignBackground = new LinearGradientColorTable(Color.FromArgb(180, factory.GetColor(0x8B8E90)), Color.FromArgb(64, Color.White));

            // Mouse Over
            sct = t.AppScrollBar.MouseOver;
            sct.Background = t.AppScrollBar.Default.Background;
            sct.Border = t.AppScrollBar.Default.Border;
            sct.ThumbBackground.Clear();
            sct.ThumbBackground.AddRange(new BackgroundColorBlend[] { new BackgroundColorBlend(factory.GetColor(0xBFD1EA),0f), 
                new BackgroundColorBlend(factory.GetColor(0xCADFFA),0.4f),
                new BackgroundColorBlend(factory.GetColor(0xAACBF6),0.4f),
                new BackgroundColorBlend(factory.GetColor(0xAACBF6),0.7f),
                new BackgroundColorBlend(factory.GetColor(0xD3E4FA),1f)});
            sct.ThumbInnerBorder = new LinearGradientColorTable(factory.GetColor(0xE9E9EB), factory.GetColor(0xFCFDFF));
            sct.ThumbOuterBorder = new LinearGradientColorTable(factory.GetColor(0x3C6EB0), Color.Empty);
            sct.ThumbSignBackground = new LinearGradientColorTable(factory.GetColor(0x6E7EA6), factory.GetColor(0x424B63), 90);
            sct.TrackBackground.Clear();
            sct.TrackBackground.CopyFrom(sct.ThumbBackground);
            sct.TrackInnerBorder = sct.ThumbInnerBorder;
            sct.TrackOuterBorder = sct.ThumbOuterBorder;
            sct.TrackSignBackground = new LinearGradientColorTable(Color.FromArgb(180, factory.GetColor(0x7D8692)), Color.FromArgb(64, Color.White));

            // Control Mouse Over
            sct = t.AppScrollBar.MouseOverControl;
            sct.Background = t.AppScrollBar.Default.Background;
            sct.Border = t.AppScrollBar.Default.Border;
            sct.ThumbBackground.Clear();
            sct.ThumbBackground.CopyFrom(t.AppScrollBar.Default.TrackBackground);
            sct.ThumbInnerBorder = t.AppScrollBar.Default.TrackInnerBorder;
            sct.ThumbOuterBorder = t.AppScrollBar.Default.TrackOuterBorder;
            sct.ThumbSignBackground = new LinearGradientColorTable(factory.GetColor(0x55658D), factory.GetColor(0x333C54));
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
                new BackgroundColorBlend(factory.GetColor(0x9DBDE7),0f), 
                new BackgroundColorBlend(factory.GetColor(0xABCCF6),0.4f),
                new BackgroundColorBlend(factory.GetColor(0x6EA6F0),0.4f),
                new BackgroundColorBlend(factory.GetColor(0x6EA6F0),0.7f),
                new BackgroundColorBlend(factory.GetColor(0xB5D1F7),1f)});
            sct.ThumbInnerBorder = new LinearGradientColorTable(factory.GetColor(0xC2D3E7), factory.GetColor(0xDDEBFB));
            sct.ThumbOuterBorder = new LinearGradientColorTable(factory.GetColor(0x17498A), Color.Empty);
            sct.ThumbSignBackground = new LinearGradientColorTable(factory.GetColor(0x6E7EA6), factory.GetColor(0x424B63), 90);
            sct.TrackBackground.Clear();
            sct.TrackBackground.CopyFrom(sct.ThumbBackground);
            //sct.TrackBackground.AddRange(new BackgroundColorBlend[] {
            //    new BackgroundColorBlend(factory.GetColor(0x9DBDE7),0f), 
            //    new BackgroundColorBlend(factory.GetColor(0xABCCF6),0.4f),
            //    new BackgroundColorBlend(factory.GetColor(0x79ADF1),0.4f),
            //    new BackgroundColorBlend(factory.GetColor(0xBBD5F8),1f)});
            sct.TrackInnerBorder = new LinearGradientColorTable(factory.GetColor(0xC2D3E7), factory.GetColor(0xDDEBFB));
            sct.TrackOuterBorder = new LinearGradientColorTable(factory.GetColor(0x17498A), Color.Empty);
            sct.TrackSignBackground = new LinearGradientColorTable(Color.FromArgb(180, factory.GetColor(0x818181)), Color.FromArgb(64, Color.White));
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

        public static void InitializeAppSilverScrollBarColorTable(Office2007ColorTable t, ColorFactory factory)
        {
            Office2007ScrollBarStateColorTable sct = t.AppScrollBar.Default;
            sct.Background = new LinearGradientColorTable(factory.GetColor(0xB4B7BF));
            sct.Border = new LinearGradientColorTable(factory.GetColor(0x9D9FA6));
            sct.ThumbSignBackground = new LinearGradientColorTable(factory.GetColor(0x7D848E), factory.GetColor(0x4B4F55), 90);
            sct.TrackBackground.Clear();
            sct.TrackBackground.AddRange(new BackgroundColorBlend[] {
                new BackgroundColorBlend(factory.GetColor(0xF5F5F6),0f), 
                new BackgroundColorBlend(factory.GetColor(0xEAEAEC),0.4f),
                new BackgroundColorBlend(factory.GetColor(0xDADBDD),0.4f),
                new BackgroundColorBlend(factory.GetColor(0xC1C1C5),1f)});
            sct.TrackInnerBorder = new LinearGradientColorTable(factory.GetColor(0xF9F9F9), factory.GetColor(0xCFCFD2));
            sct.TrackOuterBorder = new LinearGradientColorTable(factory.GetColor(0x6E6F73), Color.Empty);
            sct.TrackSignBackground = new LinearGradientColorTable(Color.FromArgb(180, factory.GetColor(0x6F6F6F)), Color.FromArgb(64, Color.White));

            // Mouse Over
            sct = t.AppScrollBar.MouseOver;
            sct.Background = t.AppScrollBar.Default.Background;
            sct.Border = t.AppScrollBar.Default.Border;
            sct.ThumbBackground.Clear();
            sct.ThumbBackground.AddRange(new BackgroundColorBlend[] {
                new BackgroundColorBlend(factory.GetColor(0xDCF0FB),0f), 
                new BackgroundColorBlend(factory.GetColor(0xC9E9FA),0.4f),
                new BackgroundColorBlend(factory.GetColor(0xA9DBF6),0.4f),
                new BackgroundColorBlend(factory.GetColor(0xA9DBF6),0.7f),
                new BackgroundColorBlend(factory.GetColor(0x94C0D8),1f)});
            sct.ThumbInnerBorder = new LinearGradientColorTable(factory.GetColor(0xFDFEFF), factory.GetColor(0xDBDDDF));
            sct.ThumbOuterBorder = new LinearGradientColorTable(factory.GetColor(0x3C7FB1), Color.Empty);
            sct.ThumbSignBackground = new LinearGradientColorTable(factory.GetColor(0x7D848E), factory.GetColor(0x4B4F55), 90);
            sct.TrackBackground.Clear();
            sct.TrackBackground.CopyFrom(sct.ThumbBackground);
            sct.TrackInnerBorder = sct.ThumbInnerBorder;
            sct.TrackOuterBorder = sct.ThumbOuterBorder;
            sct.TrackSignBackground = new LinearGradientColorTable(Color.FromArgb(180, factory.GetColor(0x6F6F6F)), Color.FromArgb(64, Color.White));

            // Control Mouse Over
            sct = t.AppScrollBar.MouseOverControl;
            sct.Background = t.AppScrollBar.Default.Background;
            sct.Border = t.AppScrollBar.Default.Border;
            sct.ThumbBackground.Clear();
            sct.ThumbBackground.CopyFrom(t.AppScrollBar.Default.TrackBackground);
            sct.ThumbInnerBorder = t.AppScrollBar.Default.TrackInnerBorder;
            sct.ThumbOuterBorder = t.AppScrollBar.Default.TrackOuterBorder;
            sct.ThumbSignBackground = new LinearGradientColorTable(factory.GetColor(0x7D848E), factory.GetColor(0x4B4F55), 90);
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
                new BackgroundColorBlend(factory.GetColor(0xC4E9F9),0f), 
                new BackgroundColorBlend(factory.GetColor(0xA5DEF6),0.4f),
                new BackgroundColorBlend(factory.GetColor(0x6FCAF0),0.4f),
                new BackgroundColorBlend(factory.GetColor(0x6FCAF0),0.7f),
                new BackgroundColorBlend(factory.GetColor(0x62B1D3),1f)});
            sct.ThumbInnerBorder = new LinearGradientColorTable(factory.GetColor(0xE3F5FC), factory.GetColor(0x9EC7D9));
            sct.ThumbOuterBorder = new LinearGradientColorTable(factory.GetColor(0x18598A), Color.Empty);
            sct.ThumbSignBackground = new LinearGradientColorTable(factory.GetColor(0x7D848E), factory.GetColor(0x4B4F55), 90);
            sct.TrackBackground.Clear();
            sct.TrackBackground.CopyFrom(sct.ThumbBackground);
            //sct.TrackBackground.AddRange(new BackgroundColorBlend[] {
            //    new BackgroundColorBlend(factory.GetColor(0x9DBDE7),0f), 
            //    new BackgroundColorBlend(factory.GetColor(0xABCCF6),0.4f),
            //    new BackgroundColorBlend(factory.GetColor(0x79ADF1),0.4f),
            //    new BackgroundColorBlend(factory.GetColor(0xBBD5F8),1f)});
            sct.TrackInnerBorder = new LinearGradientColorTable(factory.GetColor(0xE3F5FC), factory.GetColor(0x9EC7D9));
            sct.TrackOuterBorder = new LinearGradientColorTable(factory.GetColor(0x18598A), Color.Empty);
            sct.TrackSignBackground = new LinearGradientColorTable(Color.FromArgb(180, factory.GetColor(0x818181)), Color.FromArgb(64, Color.White));
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

        public static void InitializeAppBlackScrollBarColorTable(Office2007ColorTable t, ColorFactory factory)
        {
            Office2007ScrollBarStateColorTable sct = t.AppScrollBar.Default;
            sct.Background = new LinearGradientColorTable(factory.GetColor(0x404040));
            sct.Border = new LinearGradientColorTable(factory.GetColor(0x626262));
            sct.ThumbSignBackground = new LinearGradientColorTable(factory.GetColor(0xCACACA), factory.GetColor(0x797979), 90);
            sct.TrackBackground.Clear();
            sct.TrackBackground.AddRange(new BackgroundColorBlend[] {
                new BackgroundColorBlend(factory.GetColor(0xF9F9F9),0f), 
                new BackgroundColorBlend(factory.GetColor(0xEAEAEC),0.4f),
                new BackgroundColorBlend(factory.GetColor(0xDADBDD),0.4f),
                new BackgroundColorBlend(factory.GetColor(0xC1C1C5),1f)});
            sct.TrackInnerBorder = new LinearGradientColorTable(factory.GetColor(0xF9F9F9), factory.GetColor(0xCFCFD2));
            sct.TrackOuterBorder = new LinearGradientColorTable(factory.GetColor(0x2F2F2F), Color.Empty);
            sct.TrackSignBackground = new LinearGradientColorTable(Color.FromArgb(180, factory.GetColor(0x6F6F6F)), Color.FromArgb(64, Color.White));

            // Mouse Over
            sct = t.AppScrollBar.MouseOver;
            sct.Background = t.AppScrollBar.Default.Background;
            sct.Border = t.AppScrollBar.Default.Border;
            sct.ThumbBackground.Clear();
            sct.ThumbBackground.AddRange(new BackgroundColorBlend[] {
                new BackgroundColorBlend(factory.GetColor(0xDCF0FB),0f), 
                new BackgroundColorBlend(factory.GetColor(0xC9E9FA),0.4f),
                new BackgroundColorBlend(factory.GetColor(0xA9DBF6),0.4f),
                new BackgroundColorBlend(factory.GetColor(0xA9DBF6),0.7f),
                new BackgroundColorBlend(factory.GetColor(0x94C0D8),1f)});
            sct.ThumbInnerBorder = new LinearGradientColorTable(factory.GetColor(0xFDFEFF), factory.GetColor(0xDBDDDF));
            sct.ThumbOuterBorder = new LinearGradientColorTable(factory.GetColor(0x3C7FB1), Color.Empty);
            sct.ThumbSignBackground = new LinearGradientColorTable(factory.GetColor(0x838383), factory.GetColor(0x4E4E4E), 90);
            sct.TrackBackground.Clear();
            sct.TrackBackground.CopyFrom(sct.ThumbBackground);
            sct.TrackInnerBorder = sct.ThumbInnerBorder;
            sct.TrackOuterBorder = sct.ThumbOuterBorder;
            sct.TrackSignBackground = new LinearGradientColorTable(Color.FromArgb(180, factory.GetColor(0x6F6F6F)), Color.FromArgb(64, Color.White));

            // Control Mouse Over
            sct = t.AppScrollBar.MouseOverControl;
            sct.Background = t.AppScrollBar.Default.Background;
            sct.Border = t.AppScrollBar.Default.Border;
            sct.ThumbBackground.Clear();
            sct.ThumbBackground.CopyFrom(t.AppScrollBar.Default.TrackBackground);
            sct.ThumbInnerBorder = t.AppScrollBar.Default.TrackInnerBorder;
            sct.ThumbOuterBorder = t.AppScrollBar.Default.TrackOuterBorder;
            sct.ThumbSignBackground = new LinearGradientColorTable(factory.GetColor(0x838383), factory.GetColor(0x4E4E4E), 90);
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
                new BackgroundColorBlend(factory.GetColor(0xC4E9F9),0f), 
                new BackgroundColorBlend(factory.GetColor(0xA5DEF6),0.4f),
                new BackgroundColorBlend(factory.GetColor(0x6FCAF0),0.4f),
                new BackgroundColorBlend(factory.GetColor(0x6FCAF0),0.7f),
                new BackgroundColorBlend(factory.GetColor(0x62B1D3),1f)});
            sct.ThumbInnerBorder = new LinearGradientColorTable(factory.GetColor(0xE3F5FC), factory.GetColor(0x9EC7D9));
            sct.ThumbOuterBorder = new LinearGradientColorTable(factory.GetColor(0x18598A), Color.Empty);
            sct.ThumbSignBackground = new LinearGradientColorTable(factory.GetColor(0x7D848E), factory.GetColor(0x4B4F55), 90);
            sct.TrackBackground.Clear();
            sct.TrackBackground.CopyFrom(sct.ThumbBackground);
            //sct.TrackBackground.AddRange(new BackgroundColorBlend[] {
            //    new BackgroundColorBlend(factory.GetColor(0x9DBDE7),0f), 
            //    new BackgroundColorBlend(factory.GetColor(0xABCCF6),0.4f),
            //    new BackgroundColorBlend(factory.GetColor(0x79ADF1),0.4f),
            //    new BackgroundColorBlend(factory.GetColor(0xBBD5F8),1f)});
            sct.TrackInnerBorder = new LinearGradientColorTable(factory.GetColor(0xE3F5FC), factory.GetColor(0x9EC7D9));
            sct.TrackOuterBorder = new LinearGradientColorTable(factory.GetColor(0x18598A), Color.Empty);
            sct.TrackSignBackground = new LinearGradientColorTable(Color.FromArgb(180, factory.GetColor(0x818181)), Color.FromArgb(64, Color.White));
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

        public static void InitializeVistaBlackColorTable(Office2007ColorTable table, ColorFactory factory)
        {
            #region Ribbon Bar
            table.RibbonBar.Default = GetRibbonBarBlack(factory);
            table.RibbonBar.MouseOver = GetRibbonBarBlackMouseOver(factory);
            table.RibbonBar.Expanded = GetRibbonBarBlackExpanded(factory);
            #endregion

            #region RibbonTabItem
            // RibbonTabItem Default
            table.RibbonTabItemColors.Clear();
            Office2007RibbonTabItemColorTable rt = GetRibbonTabItemBlackDefault(factory);
            rt.Name = Enum.GetName(typeof(eRibbonTabColor), eRibbonTabColor.Default);
            table.RibbonTabItemColors.Add(rt);
            // Green

            rt = Office2007ColorTableFactory.GetRibbonTabItemBlackGreen(factory);
            rt.Name = Enum.GetName(typeof(eRibbonTabColor), eRibbonTabColor.Green);
            table.RibbonTabItemColors.Add(rt);
            // Magenta

            rt = Office2007ColorTableFactory.GetRibbonTabItemBlackMagenta(factory);
            rt.Name = Enum.GetName(typeof(eRibbonTabColor), eRibbonTabColor.Magenta);
            table.RibbonTabItemColors.Add(rt);
            // Orange

            rt = Office2007ColorTableFactory.GetRibbonTabItemBlackOrange(factory);
            rt.Name = Enum.GetName(typeof(eRibbonTabColor), eRibbonTabColor.Orange);
            table.RibbonTabItemColors.Add(rt);
            #endregion

            #region Ribbon Control
            table.RibbonControl = new Office2007RibbonColorTable();
            table.RibbonControl.TabsBackground = new LinearGradientColorTable(factory.GetColor(0x3C4251));
            //t.RibbonControl.InnerBorder = new LinearGradientColorTable(factory.GetColor(0xD7DADF), factory.GetColor(0x3A3A3A));
            //t.RibbonControl.OuterBorder = new LinearGradientColorTable(factory.GetColor(0xBEBEBE), factory.GetColor(0xBEBEBE));
            table.RibbonControl.InnerBorder = new LinearGradientColorTable(factory.GetColor(0x7A8295), factory.GetColor(0x646C89));
            table.RibbonControl.OuterBorder = new LinearGradientColorTable(factory.GetColor(0x111216), factory.GetColor(0x000000));
            table.RibbonControl.TabDividerBorder = factory.GetColor(0x646C89);
            table.RibbonControl.TabDividerBorderLight = factory.GetColor(0x222222);
            //t.RibbonControl.PanelTopBackground = new LinearGradientColorTable(factory.GetColor(0x626C88), factory.GetColor(0x3F4756));
            //t.RibbonControl.PanelBottomBackground = new LinearGradientColorTable(factory.GetColor(0x151516), factory.GetColor(0x3C476F));
            table.RibbonControl.PanelTopBackground = new LinearGradientColorTable(factory.GetColor(0xEDF6FF), factory.GetColor(0xBDD3EB));
            table.RibbonControl.PanelBottomBackground = new LinearGradientColorTable(factory.GetColor(0xA0B7D2), factory.GetColor(0xF0F0F0));
            table.RibbonControl.StartButtonDefault = BarFunctions.LoadBitmap("SystemImages.BlankStartButtonNormalVistaBlack.png");
            table.RibbonControl.StartButtonMouseOver = BarFunctions.LoadBitmap("SystemImages.BlankStartButtonHotVistaBlack.png");
            table.RibbonControl.StartButtonPressed = BarFunctions.LoadBitmap("SystemImages.BlankStartButtonPressedVistaBlack.png");
            #endregion

            #region ItemContainer
            table.ItemGroup.TopBackground = new LinearGradientColorTable(factory.GetColor(0xFEFEFF), factory.GetColor(0xE5EAF5));
            table.ItemGroup.BottomBackground = new LinearGradientColorTable(factory.GetColor(0xD4DBED), factory.GetColor(0xE1E6F6));
            table.ItemGroup.InnerBorder = new LinearGradientColorTable(factory.GetColor(0xFFFFFF));
            table.ItemGroup.OuterBorder = new LinearGradientColorTable(factory.GetColor(0xB2BDC7));
            table.ItemGroup.ItemGroupDividerDark = Color.FromArgb(196, factory.GetColor(0xCECECE));
            table.ItemGroup.ItemGroupDividerLight = Color.FromArgb(128, factory.GetColor(0xFFFFFF));
            #endregion

            #region Bar
            table.Bar.ToolbarTopBackground = new LinearGradientColorTable(factory.GetColor(0x5A6487), factory.GetColor(0x353A45));
            table.Bar.ToolbarBottomBackground = new LinearGradientColorTable(factory.GetColor(0x0F0F14), factory.GetColor(0x4C5E8B));
            table.Bar.ToolbarBottomBorder = factory.GetColor(0xAAB5D0);
            table.Bar.PopupToolbarBackground = new LinearGradientColorTable(factory.GetColor(0xFAFAFA), Color.Empty);
            table.Bar.PopupToolbarBorder = factory.GetColor(0x2F2F2F);
            table.Bar.StatusBarTopBorder = factory.GetColor(0x333333);
            table.Bar.StatusBarTopBorderLight = Color.Empty;
            table.Bar.StatusBarAltBackground.Clear();
            table.Bar.StatusBarAltBackground.Add(new BackgroundColorBlend(factory.GetColor(0xA8ABAB), 0f));
            table.Bar.StatusBarAltBackground.Add(new BackgroundColorBlend(factory.GetColor(0x5C6F79), 0.4f));
            table.Bar.StatusBarAltBackground.Add(new BackgroundColorBlend(factory.GetColor(0x00304B), 0.4f));
            table.Bar.StatusBarAltBackground.Add(new BackgroundColorBlend(factory.GetColor(0x2C466B), 1f));
            #endregion

            #region ButtonItem Colors Initialization
            table.ButtonItemColors.Clear();
            table.RibbonButtonItemColors.Clear();
            table.MenuButtonItemColors.Clear();
            // Orange
            Office2007ButtonItemColorTable cb = GetMenuItemBlue(false, factory);
            cb.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Orange);
            table.ButtonItemColors.Add(cb);
            // Orange with background
            cb = GetButtonItemBlackOrangeWithBackground(factory);
            cb.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.OrangeWithBackground);
            table.ButtonItemColors.Add(cb);
            // Blue
            cb = GetButtonItemBlackBlue(false, factory);
            cb.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Blue);
            table.ButtonItemColors.Add(cb);
            // Blue with background
            cb = GetButtonItemBlackBlueWithBackground(factory);
            cb.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.BlueWithBackground);
            table.ButtonItemColors.Add(cb);
            // Magenta
            cb = GetButtonItemBlackMagenta(false, factory);
            cb.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Magenta);
            table.ButtonItemColors.Add(cb);
            // Magenta with background
            cb = GetButtonItemBlackMagentaWithBackground(factory);
            cb.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.MagentaWithBackground);
            table.ButtonItemColors.Add(cb);

            // RibbonBar buttons
            cb = GetButtonItemBlackOrange(true, factory);
            cb.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Orange);
            table.RibbonButtonItemColors.Add(cb);
            // Orange with background
            cb = GetButtonItemBlackOrangeWithBackground(factory);
            cb.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.OrangeWithBackground);
            table.RibbonButtonItemColors.Add(cb);
            // Blue
            cb = GetButtonItemBlackBlue(true, factory);
            cb.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Blue);
            table.RibbonButtonItemColors.Add(cb);
            // Blue with background
            cb = GetButtonItemBlackBlueWithBackground(factory);
            cb.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.BlueWithBackground);
            table.RibbonButtonItemColors.Add(cb);
            // Magenta
            cb = GetButtonItemBlackMagenta(true, factory);
            cb.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Magenta);
            table.RibbonButtonItemColors.Add(cb);
            // Magenta with background
            cb = GetButtonItemBlackMagentaWithBackground(factory);
            cb.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.MagentaWithBackground);
            table.RibbonButtonItemColors.Add(cb);

            // MENU Orange
            cb = GetMenuItemBlue(true, factory);
            cb.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Orange);
            table.MenuButtonItemColors.Add(cb);

            cb = Office2007ColorTableFactory.GetButtonItemOffice2007WithBackground(factory);
            cb.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Office2007WithBackground);
            table.ButtonItemColors.Add(cb);

            table.ButtonItemColors.Add(ButtonItemStaticColorTables.CreateBlueOrbColorTable(factory));
            #endregion

            #region RibbonTabItemGroup Colors Initialization
            table.RibbonTabGroupColors.Clear();
            // Default
            Office2007RibbonTabGroupColorTable tg = Office2007ColorTableFactory.GetRibbonTabGroupBlackDefault(factory);
            tg.Name = Enum.GetName(typeof(eRibbonTabGroupColor), eRibbonTabGroupColor.Default);
            table.RibbonTabGroupColors.Add(tg);

            // Magenta
            tg = Office2007ColorTableFactory.GetRibbonTabGroupBlackMagenta(factory);
            tg.Name = Enum.GetName(typeof(eRibbonTabGroupColor), eRibbonTabGroupColor.Magenta);
            table.RibbonTabGroupColors.Add(tg);

            // Green
            tg = Office2007ColorTableFactory.GetRibbonTabGroupBlackGreen(factory);
            tg.Name = Enum.GetName(typeof(eRibbonTabGroupColor), eRibbonTabGroupColor.Green);
            table.RibbonTabGroupColors.Add(tg);

            // Orange
            tg = Office2007ColorTableFactory.GetRibbonTabGroupBlackOrange(factory);
            tg.Name = Enum.GetName(typeof(eRibbonTabGroupColor), eRibbonTabGroupColor.Orange);
            table.RibbonTabGroupColors.Add(tg);
            #endregion

            #region Menu
            table.Menu.Background = new LinearGradientColorTable(factory.GetColor(0xF0F0F0), Color.Empty);
            table.Menu.Border = new LinearGradientColorTable(factory.GetColor(0x646464), Color.Empty);
            table.Menu.Side = new LinearGradientColorTable(factory.GetColor(0xF1F1F1), Color.Empty);
            table.Menu.SideBorder = new LinearGradientColorTable(factory.GetColor(0xE2E3E3), Color.Empty);
            table.Menu.SideBorderLight = new LinearGradientColorTable(factory.GetColor(0xFFFFFF), Color.Empty);
            table.Menu.SideUnused = new LinearGradientColorTable(factory.GetColor(0xE5E5E5), Color.Empty);

            table.Menu.FileBackgroundBlend.Clear();
            table.Menu.FileBackgroundBlend.AddRange(new DevComponents.DotNetBar.BackgroundColorBlend[] {
                new DevComponents.DotNetBar.BackgroundColorBlend(factory.GetColor(0x626C88), 0F),
                new DevComponents.DotNetBar.BackgroundColorBlend(factory.GetColor(0x393F4D), .01F),
                new DevComponents.DotNetBar.BackgroundColorBlend(factory.GetColor(0x393F4D), 1F)});
            table.Menu.FileContainerBorder = factory.GetColor(0x222222);
            table.Menu.FileContainerBorderLight = factory.GetColor(0x7A8295);
            table.Menu.FileColumnOneBackground = factory.GetColor(0xFAFAFA);
            table.Menu.FileColumnOneBorder = factory.GetColor(0xC5C5C5);
            table.Menu.FileColumnTwoBackground = factory.GetColor(0xE9EAEE);
            table.Menu.FileBottomContainerBackgroundBlend.Clear();
            table.Menu.FileBottomContainerBackgroundBlend.AddRange(new DevComponents.DotNetBar.BackgroundColorBlend[] {
                new DevComponents.DotNetBar.BackgroundColorBlend(factory.GetColor(0x626C88), 0F),
                new DevComponents.DotNetBar.BackgroundColorBlend(factory.GetColor(0x3F4756), 0.5F),
                new DevComponents.DotNetBar.BackgroundColorBlend(factory.GetColor(0x151516), 0.5F),
                new DevComponents.DotNetBar.BackgroundColorBlend(factory.GetColor(0x3C476F), 1F)});
            #endregion

            #region ComboBox
            table.ComboBox.Default.Background = factory.GetColor(0xF2F2F2);
            table.ComboBox.Default.Border = factory.GetColor(0x8D93A0);
            table.ComboBox.Default.ExpandBackground = new LinearGradientColorTable();
            table.ComboBox.Default.ExpandBorderInner = new LinearGradientColorTable();
            table.ComboBox.Default.ExpandBorderOuter = new LinearGradientColorTable();
            table.ComboBox.Default.ExpandText = factory.GetColor(0x7C7C7C);

            table.ComboBox.DefaultStandalone.Background = factory.GetColor(0xFFFFFF);
            table.ComboBox.DefaultStandalone.Border = factory.GetColor(0x707070);
            table.ComboBox.DefaultStandalone.ExpandBackground = new LinearGradientColorTable(factory.GetColor(0xF2F2F2), factory.GetColor(0xCFCFCF), 90);
            table.ComboBox.DefaultStandalone.ExpandBorderInner = new LinearGradientColorTable();
            table.ComboBox.DefaultStandalone.ExpandBorderOuter = new LinearGradientColorTable(factory.GetColor(0x707070), Color.Empty, 90);
            table.ComboBox.DefaultStandalone.ExpandText = factory.GetColor(0x000000);

            table.ComboBox.MouseOver.Background = factory.GetColor(0xFFFFFF);
            table.ComboBox.MouseOver.Border = factory.GetColor(0x3C7FB1);
            table.ComboBox.MouseOver.ExpandBackground = new LinearGradientColorTable(factory.GetColor(0xEAF6FD), factory.GetColor(0xA7D9F5), 90);
            table.ComboBox.MouseOver.ExpandBorderInner = new LinearGradientColorTable(factory.GetColor(0xFAFDFE));
            table.ComboBox.MouseOver.ExpandBorderOuter = new LinearGradientColorTable(factory.GetColor(0x3C7FB1), Color.Empty, 90);
            table.ComboBox.MouseOver.ExpandText = factory.GetColor(0x000000);
            table.ComboBox.DroppedDown.Background = factory.GetColor(0xFFFFFF);
            table.ComboBox.DroppedDown.Border = factory.GetColor(0x2C628B);
            table.ComboBox.DroppedDown.ExpandBackground = new LinearGradientColorTable(factory.GetColor(0xE5F4FC), factory.GetColor(0x68B3DB), 90);
            table.ComboBox.DroppedDown.ExpandBorderInner = new LinearGradientColorTable(factory.GetColor(0x9EB0BA), Color.Transparent, 90);
            table.ComboBox.DroppedDown.ExpandBorderOuter = new LinearGradientColorTable(factory.GetColor(0x2C628B), Color.Empty, 90);
            table.ComboBox.DroppedDown.ExpandText = factory.GetColor(0x000000);
            #endregion

            #region Dialog Launcher
            table.DialogLauncher.Default.DialogLauncher = factory.GetColor(0x3C476F);
            table.DialogLauncher.Default.DialogLauncherShade = factory.GetColor(0xEBEBEB);

            table.DialogLauncher.MouseOver.DialogLauncher = factory.GetColor(0x3C476F);
            table.DialogLauncher.MouseOver.DialogLauncherShade = factory.GetColor(0xEBEBEB);
            table.DialogLauncher.MouseOver.TopBackground = new LinearGradientColorTable(factory.GetColor(0xE9F7FE), factory.GetColor(0xDDF2FD));
            table.DialogLauncher.MouseOver.BottomBackground = new LinearGradientColorTable(factory.GetColor(0x95BED5), factory.GetColor(0xB5E4FF));
            table.DialogLauncher.MouseOver.InnerBorder = new LinearGradientColorTable(Color.FromArgb(128, Color.White));
            table.DialogLauncher.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(0xA5A5A5), factory.GetColor(0x959595));

            table.DialogLauncher.Pressed.DialogLauncher = factory.GetColor(0x3C476F);
            table.DialogLauncher.Pressed.DialogLauncherShade = factory.GetColor(0xEBEBEB);
            table.DialogLauncher.Pressed.TopBackground = new LinearGradientColorTable(factory.GetColor(0xE9F7FE), factory.GetColor(0xC5E9FB));
            table.DialogLauncher.Pressed.BottomBackground = new LinearGradientColorTable(factory.GetColor(0x649EBF), factory.GetColor(0x87D3FF));
            table.DialogLauncher.Pressed.InnerBorder = new LinearGradientColorTable(Color.FromArgb(128, Color.White));
            table.DialogLauncher.Pressed.OuterBorder = new LinearGradientColorTable(factory.GetColor(0xA5A5A5), factory.GetColor(0x959595));
            #endregion

            #region Legacy Color Scheme
            InitializeBlackLegacyColors(table.LegacyColors, factory);
            #endregion

            #region System Button, Form

            // Default state no background
            table.SystemButton.Default = new Office2007SystemButtonStateColorTable();
            table.SystemButton.Default.Foreground = new LinearGradientColorTable(factory.GetColor(0x848C95), factory.GetColor(0x9FA9B7));
            table.SystemButton.Default.LightShade = factory.GetColor(0x9FA9B7);
            table.SystemButton.Default.DarkShade = factory.GetColor(0x818080);

            // Mouse over state
            table.SystemButton.MouseOver = new Office2007SystemButtonStateColorTable();
            table.SystemButton.MouseOver.Foreground = new LinearGradientColorTable(factory.GetColor(0x363535), factory.GetColor(0x4F5763));
            table.SystemButton.MouseOver.LightShade = factory.GetColor(0x9BA1A8);
            table.SystemButton.MouseOver.DarkShade = factory.GetColor(0x454C56);
            table.SystemButton.MouseOver.TopBackground = new LinearGradientColorTable(factory.GetColor(0xB1B8C1), factory.GetColor(0x8894A1));
            table.SystemButton.MouseOver.BottomBackground = new LinearGradientColorTable(factory.GetColor(0x687585), factory.GetColor(0x9DB1C1));
            table.SystemButton.MouseOver.TopHighlight = new LinearGradientColorTable(factory.GetColor(0xCDD3DA), Color.Transparent);
            table.SystemButton.MouseOver.BottomHighlight = new LinearGradientColorTable(factory.GetColor(0xB5DDF4), Color.Transparent);
            table.SystemButton.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(0x57606D), factory.GetColor(0x5C6269));
            table.SystemButton.MouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(0xCDD3DA), factory.GetColor(0xDADFE3));

            // Pressed
            table.SystemButton.Pressed = new Office2007SystemButtonStateColorTable();
            table.SystemButton.Pressed.Foreground = new LinearGradientColorTable(factory.GetColor(0x6D6C6C), factory.GetColor(0x8B96A4));
            table.SystemButton.Pressed.LightShade = factory.GetColor(0x7F8894);
            table.SystemButton.Pressed.DarkShade = factory.GetColor(0x6D6C6C);
            table.SystemButton.Pressed.TopBackground = new LinearGradientColorTable(factory.GetColor(0x373737), factory.GetColor(0x2B2B2B));
            table.SystemButton.Pressed.TopHighlight = new LinearGradientColorTable(factory.GetColor(0x686868), Color.Transparent);
            table.SystemButton.Pressed.BottomBackground = new LinearGradientColorTable(factory.GetColor(0x000000), factory.GetColor(0x07090B));
            table.SystemButton.Pressed.BottomHighlight = new LinearGradientColorTable(factory.GetColor(0x516982), Color.Transparent);
            table.SystemButton.Pressed.OuterBorder = new LinearGradientColorTable(factory.GetColor(0x222429), factory.GetColor(0x191919));
            table.SystemButton.Pressed.InnerBorder = new LinearGradientColorTable(factory.GetColor(0x464646), factory.GetColor(0x333333));

            // Form border
            table.Form.Active.BorderColors = new Color[] {
                factory.GetColor(0x101111),
                factory.GetColor(0x222222),
                factory.GetColor(0x4B4B4B),
                factory.GetColor(0x3C4251),
                factory.GetColor(0x3C4251)};
            table.Form.Inactive.BorderColors = new Color[] {
                factory.GetColor(0x101111),
                factory.GetColor(0x222222),
                factory.GetColor(0x4B4B4B),
                factory.GetColor(0x3C4251),
                factory.GetColor(0x3C4251)};


            // Form Caption Active
            table.Form.Active.CaptionTopBackground = new LinearGradientColorTable(factory.GetColor(0x626C88), factory.GetColor(0x393F4D));
            table.Form.Active.CaptionBottomBackground = new LinearGradientColorTable(factory.GetColor(0x151516), factory.GetColor(0x3C476F));
            table.Form.Active.CaptionBottomBorder = new Color[] { factory.GetColor(0x646C89), factory.GetColor(0x222222) };
            table.Form.Active.CaptionText = factory.GetColor(0xFFFFFF);
            table.Form.Active.CaptionTextExtra = factory.GetColor(0x14C2DC);

            // Form Caption Inactive
            table.Form.Inactive.CaptionTopBackground = new LinearGradientColorTable(factory.GetColor(0x626C88), factory.GetColor(0x393F4D));
            table.Form.Inactive.CaptionBottomBackground = new LinearGradientColorTable(factory.GetColor(0x393F4D), factory.GetColor(0x393F4D));
            table.Form.Inactive.CaptionText = factory.GetColor(0xE1E1E1);
            table.Form.Inactive.CaptionTextExtra = factory.GetColor(0xE1E1E1);

            table.Form.BackColor = factory.GetColor(0xEEF3FA);
            table.Form.TextColor = factory.GetColor(0x000000);
            #endregion

            #region Quick Access Toolbar Background
            table.QuickAccessToolbar.Active.TopBackground = new LinearGradientColorTable(factory.GetColor(0x626C88), factory.GetColor(0x5A637C));
            table.QuickAccessToolbar.Active.BottomBackground = new LinearGradientColorTable(factory.GetColor(0x5A637C), factory.GetColor(0x393F4D));
            table.QuickAccessToolbar.Active.OutterBorderColor = factory.GetColor(Color.FromArgb(32, Color.White));
            table.QuickAccessToolbar.Active.MiddleBorderColor = factory.GetColor(0x222222);
            table.QuickAccessToolbar.Active.InnerBorderColor = Color.Empty;

            table.QuickAccessToolbar.Inactive.TopBackground = new LinearGradientColorTable(factory.GetColor(0x626C88), factory.GetColor(0x5A637C));
            table.QuickAccessToolbar.Inactive.BottomBackground = new LinearGradientColorTable(factory.GetColor(0x5A637C), factory.GetColor(0x393F4D));
            table.QuickAccessToolbar.Inactive.OutterBorderColor = Color.Transparent;
            table.QuickAccessToolbar.Inactive.MiddleBorderColor = factory.GetColor(0x222222);
            table.QuickAccessToolbar.Inactive.InnerBorderColor = Color.Empty;

            table.QuickAccessToolbar.Standalone.TopBackground = new LinearGradientColorTable();
            table.QuickAccessToolbar.Standalone.BottomBackground = new LinearGradientColorTable(factory.GetColor(0xFEFEFF), factory.GetColor(0xE5EAF5));
            table.QuickAccessToolbar.Standalone.OutterBorderColor = factory.GetColor(0x474E56);
            table.QuickAccessToolbar.Standalone.MiddleBorderColor = Color.Empty;
            table.QuickAccessToolbar.Standalone.InnerBorderColor = factory.GetColor(0xCBCCCE);

            table.QuickAccessToolbar.QatCustomizeMenuLabelBackground = factory.GetColor(0xD4DBED);
            table.QuickAccessToolbar.QatCustomizeMenuLabelText = factory.GetColor(0x000000);

            table.QuickAccessToolbar.Active.GlassBorder = new LinearGradientColorTable(factory.GetColor(Color.FromArgb(132, Color.Black)), Color.FromArgb(80, Color.Black));
            table.QuickAccessToolbar.Inactive.GlassBorder = new LinearGradientColorTable(factory.GetColor(Color.FromArgb(132, Color.Black)), Color.FromArgb(80, Color.Black));
            #endregion

            #region Tab Colors
            table.TabControl.Default = new Office2007TabItemStateColorTable();
            table.TabControl.Default.TopBackground = new LinearGradientColorTable(factory.GetColor(0xF2F5FA), factory.GetColor(0xE5EAF5));
            table.TabControl.Default.BottomBackground = new LinearGradientColorTable(factory.GetColor(0xCFD7EB), factory.GetColor(0xECEEFC));
            table.TabControl.Default.InnerBorder = factory.GetColor(0xFCFDFD);
            table.TabControl.Default.OuterBorder = factory.GetColor(0x9196A2);
            table.TabControl.Default.Text = factory.GetColor(0x000000);

            table.TabControl.MouseOver = new Office2007TabItemStateColorTable();
            table.TabControl.MouseOver.TopBackground = new LinearGradientColorTable(factory.GetColor(0xEDF3FC), factory.GetColor(0xC6DCF7));
            table.TabControl.MouseOver.BottomBackground = new LinearGradientColorTable(factory.GetColor(0x99C6EE), factory.GetColor(0xD9E9F9));
            table.TabControl.MouseOver.InnerBorder = factory.GetColor(0xFEFEFF);
            table.TabControl.MouseOver.OuterBorder = factory.GetColor(0x9196A2);
            table.TabControl.MouseOver.Text = factory.GetColor(0x000000);

            table.TabControl.Selected = new Office2007TabItemStateColorTable();
            //t.TabControl.Selected.TopBackground = new LinearGradientColorTable(factory.GetColor(0xFFD29B), factory.GetColor(0xFFBB6E));
            //t.TabControl.Selected.BottomBackground = new LinearGradientColorTable(factory.GetColor(0xFFAF44), factory.GetColor(0xFEDC75));
            //t.TabControl.Selected.InnerBorder = factory.GetColor(0xCDB69C);
            //t.TabControl.Selected.OuterBorder = factory.GetColor(0x95774A);
            table.TabControl.Selected.TopBackground = new LinearGradientColorTable(factory.GetColor(0xFBFDFE), factory.GetColor(0xE7F5FB));
            table.TabControl.Selected.BottomBackground = new LinearGradientColorTable(factory.GetColor(0xCFE7FA), factory.GetColor(0xB9D1FA));
            table.TabControl.Selected.InnerBorder = factory.GetColor(0xFCFEFF);
            table.TabControl.Selected.OuterBorder = factory.GetColor(0x9196A2);
            table.TabControl.Selected.Text = factory.GetColor(0x000000);

            table.TabControl.TabBackground = new LinearGradientColorTable(factory.GetColor(0xFFFFFF), factory.GetColor(0xE3E8F4));
            table.TabControl.TabPanelBackground = new LinearGradientColorTable(factory.GetColor(0xFFFFFF));
            table.TabControl.TabPanelBorder = factory.GetColor(0x8D93A0);
            #endregion

            #region CheckBoxItem
            Office2007CheckBoxColorTable chk = table.CheckBoxItem;
            chk.Default.CheckBackground = new LinearGradientColorTable(factory.GetColor(0xF4F4F4), Color.Empty);
            chk.Default.CheckBorder = factory.GetColor(0x616161);
            chk.Default.CheckInnerBackground = new LinearGradientColorTable(Color.FromArgb(192, factory.GetColor(0xA2ACB9)), Color.FromArgb(164, factory.GetColor(0xF6F6F6)));
            chk.Default.CheckInnerBorder = factory.GetColor(0xA2ACB9);
            chk.Default.CheckSign = new LinearGradientColorTable(factory.GetColor(0x4F687D), Color.Empty);
            chk.Default.Text = factory.GetColor(0x000000);

            chk.MouseOver.CheckBackground = new LinearGradientColorTable(factory.GetColor(0xF4F4F4), Color.Empty);
            chk.MouseOver.CheckBorder = factory.GetColor(0x616161);
            chk.MouseOver.CheckInnerBackground = new LinearGradientColorTable(factory.GetColor(0xAEE4FF), factory.GetColor(0xD0ECFC), 90);
            chk.MouseOver.CheckInnerBorder = factory.GetColor(0xB3E4F9);
            chk.MouseOver.CheckSign = new LinearGradientColorTable(factory.GetColor(0x4F687D), Color.Empty);
            chk.MouseOver.Text = factory.GetColor(0x000000);

            chk.Pressed.CheckBackground = new LinearGradientColorTable(factory.GetColor(0xE5ECF7), Color.Empty);
            chk.Pressed.CheckBorder = factory.GetColor(0x616161);
            chk.Pressed.CheckInnerBackground = new LinearGradientColorTable(Color.FromArgb(192, factory.GetColor(0x68B3DB)), Color.FromArgb(164, factory.GetColor(0xE5F4FC)));
            chk.Pressed.CheckInnerBorder = factory.GetColor(0x2C628B);
            chk.Pressed.CheckSign = new LinearGradientColorTable(factory.GetColor(0x4F687D), Color.Empty);
            chk.Pressed.Text = factory.GetColor(0x000000);

            chk.Disabled.CheckBackground = new LinearGradientColorTable(factory.GetColor(0xFFFFFF), Color.Empty);
            chk.Disabled.CheckBorder = factory.GetColor(0xAEB1B5);
            chk.Disabled.CheckInnerBackground = new LinearGradientColorTable(Color.FromArgb(192, factory.GetColor(0xE0E2E5)), Color.FromArgb(164, factory.GetColor(0xFBFBFB)));
            chk.Disabled.CheckInnerBorder = factory.GetColor(0xE0E2E5);
            chk.Disabled.CheckSign = new LinearGradientColorTable(factory.GetColor(0x8D8D8D), Color.Empty);
            chk.Disabled.Text = factory.GetColor(0x8D8D8D);
            #endregion

            #region Scroll Bar
            InitializeScrollBarColorTable(table, factory);
            InitializeAppBlackScrollBarColorTable(table, factory);
            #endregion

            #region ProgressBarItem
            Office2007ProgressBarColorTable pct = table.ProgressBarItem;
            pct.BackgroundColors = new GradientColorTable(0x87898B, 0x979897);
            pct.OuterBorder = factory.GetColor(0xCCCCCC);
            pct.InnerBorder = factory.GetColor(0x252525);
            pct.Chunk = new GradientColorTable(0x679720, 0xC2FF56, 0);
            pct.ChunkOverlay = new GradientColorTable();
            pct.ChunkOverlay.LinearGradientAngle = 90;
            pct.ChunkOverlay.Colors.AddRange(new BackgroundColorBlend[] {
                new BackgroundColorBlend(Color.FromArgb(164, factory.GetColor(0xEEFFD7)), 0f),
                new BackgroundColorBlend(Color.FromArgb(128, factory.GetColor(0x8DB254)), .5f),
                new BackgroundColorBlend(Color.FromArgb(64, factory.GetColor(0x69922B)), .5f),
                new BackgroundColorBlend(Color.Transparent, 1f),
            });
            pct.ChunkShadow = new GradientColorTable(0x858585, 0x909190, 0);

            // Paused
            pct = table.ProgressBarItemPaused;
            pct.BackgroundColors = new GradientColorTable(0x87898B, 0x979897);
            pct.OuterBorder = factory.GetColor(0xCCCCCC);
            pct.InnerBorder = factory.GetColor(0x252525);
            pct.Chunk = new GradientColorTable(0xAEA700, 0xFFFDCD, 0);
            pct.ChunkOverlay = new GradientColorTable();
            pct.ChunkOverlay.LinearGradientAngle = 90;
            pct.ChunkOverlay.Colors.AddRange(new BackgroundColorBlend[] {
                new BackgroundColorBlend(Color.FromArgb(192, factory.GetColor(0xFFFBA3)), 0f),
                new BackgroundColorBlend(Color.FromArgb(128, factory.GetColor(0xD2CA00)), .5f),
                new BackgroundColorBlend(Color.FromArgb(64, factory.GetColor(0xFEF400)), .5f),
                new BackgroundColorBlend(Color.Transparent, 1f),
            });
            pct.ChunkShadow = new GradientColorTable(0x858585, 0x909190, 0);

            // Error
            pct = table.ProgressBarItemError;
            pct.BackgroundColors = new GradientColorTable(0x87898B, 0x979897);
            pct.OuterBorder = factory.GetColor(0xCCCCCC);
            pct.InnerBorder = factory.GetColor(0x252525);
            pct.Chunk = new GradientColorTable(0xD20000, 0xFFCDCD, 0);
            pct.ChunkOverlay = new GradientColorTable();
            pct.ChunkOverlay.LinearGradientAngle = 90;
            pct.ChunkOverlay.Colors.AddRange(new BackgroundColorBlend[] {
                new BackgroundColorBlend(Color.FromArgb(192, factory.GetColor(0xFF8F8F)), 0f),
                new BackgroundColorBlend(Color.FromArgb(128, factory.GetColor(0xD20000)), .5f),
                new BackgroundColorBlend(Color.FromArgb(64, factory.GetColor(0xFE0000)), .5f),
                new BackgroundColorBlend(Color.Transparent, 1f),
            });
            pct.ChunkShadow = new GradientColorTable(0x858585, 0x909190, 0);
            #endregion

            #region Gallery
            Office2007GalleryColorTable gallery = table.Gallery;
            gallery.GroupLabelBackground = factory.GetColor(0xD4DBED);
            gallery.GroupLabelText = factory.GetColor(0x000000);
            gallery.GroupLabelBorder = factory.GetColor(0xC5C5C5);
            #endregion

            #region ListViewEx
            table.ListViewEx.Border = factory.GetColor(0x8D93A0);
            table.ListViewEx.ColumnBackground = new LinearGradientColorTable(factory.GetColor(0xFFFFFF), factory.GetColor(0xF1F2F4));
            table.ListViewEx.ColumnSeparator = factory.GetColor(0xD5D5D5);
            table.ListViewEx.SelectionBackground = new LinearGradientColorTable(factory.GetColor(0xF1F8FD), factory.GetColor(0xD5EFFC));
            table.ListViewEx.SelectionBorder = factory.GetColor(0x99DEFD);
            #endregion

            #region Navigation Pane
            table.NavigationPane.ButtonBackground = new GradientColorTable();
            table.NavigationPane.ButtonBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xF9F9F9), 0));
            table.NavigationPane.ButtonBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xDADADC), .4f));
            table.NavigationPane.ButtonBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xCACACC), .4f));
            table.NavigationPane.ButtonBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xF4F4F6), 1));
            #endregion

            #region SuperTooltip
            table.SuperTooltip.BackgroundColors = new LinearGradientColorTable(factory.GetColor(0xFFFFFF), factory.GetColor(0xEDF6FF));
            table.SuperTooltip.TextColor = factory.GetColor(0x000000);
            #endregion

            #region Slider
            Office2007SliderColorTable sl = table.Slider;
            sl.Default.LabelColor = factory.GetColor(0xFFFFFF);
            sl.Default.SliderLabelColor = factory.GetColor(0x000000);
            sl.Default.PartBackground = new GradientColorTable();
            sl.Default.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xFFFFFF), 0));
            sl.Default.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xEFEFEF), .15f));
            sl.Default.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xBEC2C3), .5f));
            sl.Default.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(0x6C7178), .5f));
            sl.Default.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xDCDCDE), .85f));
            sl.Default.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xFFFFFF), 1f));
            sl.Default.PartBorderColor = factory.GetColor(0x393F46);
            sl.Default.PartBorderLightColor = Color.FromArgb(28, factory.GetColor(0xFFFFFF));
            sl.Default.PartForeColor = factory.GetColor(0x5B636C);
            sl.Default.PartForeLightColor = Color.FromArgb(168, factory.GetColor(0xEAEAEA));
            sl.Default.TrackLineColor = factory.GetColor(0x252525);
            sl.Default.TrackLineLightColor = factory.GetColor(0xCCCCCC);

            sl.MouseOver.LabelColor = factory.GetColor(0xFFFFFF);
            sl.MouseOver.SliderLabelColor = factory.GetColor(0x000000);
            sl.MouseOver.PartBackground = new GradientColorTable();
            sl.MouseOver.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xFFFFFF), 0));
            sl.MouseOver.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xF1F8FD), .1f));
            sl.MouseOver.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(0x99DEFD), .9f));
            sl.MouseOver.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xE7F5FD), 1f));
            sl.MouseOver.PartBorderColor = factory.GetColor(0x2D2D2D);
            sl.MouseOver.PartBorderLightColor = Color.FromArgb(28, factory.GetColor(0xFFFFFF));
            sl.MouseOver.PartForeColor = factory.GetColor(0x676249);
            sl.MouseOver.PartForeLightColor = Color.FromArgb(168, factory.GetColor(0xFFF4D4));
            sl.MouseOver.TrackLineColor = factory.GetColor(0x252525);
            sl.MouseOver.TrackLineLightColor = factory.GetColor(0xCCCCCC);

            sl.Pressed.LabelColor = factory.GetColor(0xFFFFFF);
            sl.Pressed.SliderLabelColor = factory.GetColor(0x000000);
            sl.Pressed.PartBackground = new GradientColorTable();
            sl.Pressed.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xE5F4FC), 0));
            sl.Pressed.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xC4E5F6), .5f));
            sl.Pressed.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(0x98D1EF), .5f));
            sl.Pressed.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(0x68B1D8), 1f));
            sl.Pressed.PartBorderColor = factory.GetColor(0x2D2D2D);
            sl.Pressed.PartBorderLightColor = Color.FromArgb(28, factory.GetColor(0xFFFFFF));
            sl.Pressed.PartForeColor = factory.GetColor(0x675343);
            sl.Pressed.PartForeLightColor = Color.FromArgb(32, factory.GetColor(0xFFDCBB));
            sl.Pressed.TrackLineColor = factory.GetColor(0x252525);
            sl.Pressed.TrackLineLightColor = factory.GetColor(0xCCCCCC);

            ColorBlendFactory df = new ColorBlendFactory(ColorScheme.GetColor(0xCFCFCF));
            sl.Disabled.LabelColor = factory.GetColor(0x8D8D8D);
            sl.Default.SliderLabelColor = factory.GetColor(0x8d8d8d);
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

            #region DataGridView
            table.DataGridView.ColumnHeaderNormalBorder = factory.GetColor(0xB6B6B6);
            table.DataGridView.ColumnHeaderNormalBackground = new LinearGradientColorTable(factory.GetColor(0xF8F8F8), factory.GetColor(0xDEDEDE), 90);
            table.DataGridView.ColumnHeaderSelectedBackground = new LinearGradientColorTable(factory.GetColor(0xF9D99F), factory.GetColor(0xF1C15F), 90);
            table.DataGridView.ColumnHeaderSelectedBorder = factory.GetColor(0xF29536);
            table.DataGridView.ColumnHeaderSelectedMouseOverBackground = new LinearGradientColorTable(factory.GetColor(0xFFD58D), factory.GetColor(0xF2923A), 90);
            table.DataGridView.ColumnHeaderSelectedMouseOverBorder = factory.GetColor(0xF29536);
            table.DataGridView.ColumnHeaderMouseOverBackground = new LinearGradientColorTable(factory.GetColor(0xE0E0E0), factory.GetColor(0xC3C3C3), 90);
            table.DataGridView.ColumnHeaderMouseOverBorder = factory.GetColor(0xB6B6B6);
            table.DataGridView.ColumnHeaderPressedBackground = new LinearGradientColorTable(factory.GetColor(0xE0E0E0), factory.GetColor(0xC3C3C3), 90);
            table.DataGridView.ColumnHeaderPressedBorder = factory.GetColor(0xFFFFFF);

            table.DataGridView.RowNormalBackground = new LinearGradientColorTable(factory.GetColor(0xEDEDED));
            table.DataGridView.RowNormalBorder = factory.GetColor(0xB6B6B6);
            table.DataGridView.RowSelectedBackground = new LinearGradientColorTable(factory.GetColor(0xF6FBFD), factory.GetColor(0xD5EFFC));
            table.DataGridView.RowSelectedBorder = factory.GetColor(0x99DEFD);
            table.DataGridView.RowSelectedMouseOverBackground = new LinearGradientColorTable(factory.GetColor(0xE8F6FD), factory.GetColor(0xC4E8FA));
            table.DataGridView.RowSelectedMouseOverBorder = factory.GetColor(0xB6E6FB);
            table.DataGridView.RowMouseOverBackground = new LinearGradientColorTable(factory.GetColor(0xF5FAFD), factory.GetColor(0xE8F5FD));
            table.DataGridView.RowMouseOverBorder = factory.GetColor(0xD8F0FA);
            table.DataGridView.RowPressedBackground = new LinearGradientColorTable(factory.GetColor(0xBCE4F9), factory.GetColor(0x8AD1F5));
            table.DataGridView.RowPressedBorder = factory.GetColor(0x4E91AE);

            table.DataGridView.GridColor = factory.GetColor(0xD0D7E5);

            table.DataGridView.SelectorBackground = new LinearGradientColorTable(factory.GetColor(0xC9C9C9));
            table.DataGridView.SelectorBorder = factory.GetColor(0xB6B6B6);
            table.DataGridView.SelectorBorderDark = factory.GetColor(0xCCCCCC);
            table.DataGridView.SelectorBorderLight = factory.GetColor(0xEBEBEB);
            table.DataGridView.SelectorSign = new LinearGradientColorTable(factory.GetColor(0x7D7D7D), factory.GetColor(0x676767));

            table.DataGridView.SelectorMouseOverBackground = new LinearGradientColorTable(factory.GetColor(0xADADAD), factory.GetColor(0x9F9F9F));
            table.DataGridView.SelectorMouseOverBorder = factory.GetColor(0xB6B6B6);
            table.DataGridView.SelectorMouseOverBorderDark = factory.GetColor(0xCCCCCC);
            table.DataGridView.SelectorMouseOverBorderLight = factory.GetColor(0xEBEBEB);
            table.DataGridView.SelectorMouseOverSign = new LinearGradientColorTable(factory.GetColor(0x7D7D7D), factory.GetColor(0x676767));
            #endregion

            #region AdvTree
#if !NOTREE
            table.AdvTree = new DevComponents.AdvTree.Display.TreeColorTable();
            DevComponents.AdvTree.Display.ColorTableInitializer.InitOffice2007VistaGlass(table.AdvTree, factory);
#endif
            #endregion

            #region CrumbBar
            table.CrumbBarItemView = CrumbBar.GetCrumbBarVistaColorTable(factory);
            #endregion

            #region ElementStyle Classes
            table.StyleClasses.Clear();
            ElementStyle style = new ElementStyle();
            style.Class = ElementStyleClassKeys.RibbonGalleryContainerKey;
            style.BorderColor = factory.GetColor(0x5C6672);
            style.Border = eStyleBorderType.Solid;
            style.BorderWidth = 1;
            style.CornerDiameter = 2;
            style.CornerType = eCornerType.Rounded;
            style.BackColor = factory.GetColor(0xEAF4FE);
            table.StyleClasses.Add(style.Class, style);
            // FileMenuContainer
            style = Office2007ColorTableFactory.GetFileMenuContainerStyle(table);
            table.StyleClasses.Add(style.Class, style);
            // Two Column File Menu Container
            style = Office2007ColorTableFactory.GetTwoColumnMenuContainerStyle(table);
            table.StyleClasses.Add(style.Class, style);
            // Column one File Menu Container
            style = Office2007ColorTableFactory.GetMenuColumnOneContainerStyle(table);
            table.StyleClasses.Add(style.Class, style);
            // Column two File Menu Container
            style = Office2007ColorTableFactory.GetMenuColumnTwoContainerStyle(table);
            table.StyleClasses.Add(style.Class, style);
            // Bottom File Menu Container
            style = Office2007ColorTableFactory.GetMenuBottomContainer(table);
            table.StyleClasses.Add(style.Class, style);
            // TextBox border
            style = Office2007ColorTableFactory.GetTextBoxStyle(factory.GetColor(0x8D93A0));
            table.StyleClasses.Add(style.Class, style);
            // ItemPanel
            style = Office2007ColorTableFactory.GetItemPanelStyle(factory.GetColor(0x8D93A0));
            table.StyleClasses.Add(style.Class, style);
            // DateTimeInput background
            style = Office2007ColorTableFactory.GetDateTimeInputBackgroundStyle(factory.GetColor(0x8D93A0));
            table.StyleClasses.Add(style.Class, style);
            // Ribbon Client Panel
            style = Office2007ColorTableFactory.GetRibbonClientPanelStyle(factory, eOffice2007ColorScheme.Black);
            table.StyleClasses.Add(style.Class, style);
            // ListView Border
            style = Office2007ColorTableFactory.GetListViewBorderStyle(table.ListViewEx);
            table.StyleClasses.Add(style.Class, style);
            style = GetStatusBarAltStyle(table.Bar);
            table.StyleClasses.Add(style.Class, style);

#if !NOTREE
            // Tree Border/Background
            style = Office2007ColorTableFactory.GetAdvTreeStyle(factory.GetColor(0x8D93A0));
            table.StyleClasses.Add(style.Class, style);
            style = Office2007ColorTableFactory.GetAdvTreeColumnsHeaderStyle(factory.GetColor(0xF8F8F8), factory.GetColor(0xDEDEDE), factory.GetColor(0xB6B6B6));
            table.StyleClasses.Add(style.Class, style);
            style = Office2007ColorTableFactory.GetAdvTreeNodesColumnsHeaderStyle(factory.GetColor(0xF8F8F8), factory.GetColor(0xDEDEDE), factory.GetColor(0xB6B6B6));
            table.StyleClasses.Add(style.Class, style);
            style = Office2007ColorTableFactory.GetAdvTreeColumnStyle(factory.GetColor(0x000000));
            table.StyleClasses.Add(style.Class, style);
#endif
            // CrumbBar
            style = Office2007ColorTableFactory.GetCrumbBarBackgroundStyle(factory.GetColor(Color.White), factory.GetColor("FF333333"), factory.GetColor("FF252525"));
            table.StyleClasses.Add(style.Class, style);
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

            #region SideBar
            table.SideBar.Background = new LinearGradientColorTable(factory.GetColor(Color.White));
            table.SideBar.Border = factory.GetColor(0x8D93A0);
            table.SideBar.SideBarPanelItemText = factory.GetColor(Color.Black);
            table.SideBar.SideBarPanelItemDefault = new GradientColorTable();
            table.SideBar.SideBarPanelItemDefault.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xF8F8F9), 0));
            table.SideBar.SideBarPanelItemDefault.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xDFE2E4), .4f));
            table.SideBar.SideBarPanelItemDefault.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xC7CBD1), .4f));
            table.SideBar.SideBarPanelItemDefault.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xDBDEE2), 1));
            // Expanded
            table.SideBar.SideBarPanelItemExpanded = new GradientColorTable();
            table.SideBar.SideBarPanelItemExpanded.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xE3F7FF), 0));
            table.SideBar.SideBarPanelItemExpanded.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xE3F7FF), .4f));
            table.SideBar.SideBarPanelItemExpanded.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xBDEDFF), .4f));
            table.SideBar.SideBarPanelItemExpanded.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xB7E7FB), 1));
            // MouseOver
            table.SideBar.SideBarPanelItemMouseOver = new GradientColorTable();
            table.SideBar.SideBarPanelItemMouseOver.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xE5F3FA), 0));
            table.SideBar.SideBarPanelItemMouseOver.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xD0ECFC), 1));
            // Pressed
            table.SideBar.SideBarPanelItemPressed = new GradientColorTable();
            table.SideBar.SideBarPanelItemPressed.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xE5F4FC), 0));
            table.SideBar.SideBarPanelItemPressed.Colors.Add(new BackgroundColorBlend(factory.GetColor(0xC4E5F6), .4f));
            table.SideBar.SideBarPanelItemPressed.Colors.Add(new BackgroundColorBlend(factory.GetColor(0x98D1EF), .4f));
            table.SideBar.SideBarPanelItemPressed.Colors.Add(new BackgroundColorBlend(factory.GetColor(0x68B3DB), 1));
            #endregion

            #region WarningBox
            table.WarningBox.BackColor = factory.GetColor(Color.FromArgb(64, 64, 64));
            table.WarningBox.WarningBorderColor = factory.GetColor(Color.FromArgb(86, 86, 86));
            table.WarningBox.WarningBackColor1 = factory.GetColor(Color.FromArgb(255, 255, 255));
            table.WarningBox.WarningBackColor2 = factory.GetColor(Color.FromArgb(234, 234, 234));
            #endregion

            #region CalendarView

            #region WeekDayViewColors

            table.CalendarView.WeekDayViewColors = new ColorDef[]
            {
                new ColorDef(factory.GetColor(0x9199A4)),           // DayViewBorder
                new ColorDef(factory.GetColor(0x000000)),           // DayHeaderForeground

                new ColorDef(new Color[] {factory.GetColor(0xDCDFE2), factory.GetColor(0xD3D6DA), factory.GetColor(0xB4BAC1), factory.GetColor(0xCBCED4)},
                new float[] {0f, .55f, .58f, 1f}, 90f),             // DayHeaderBackground

                new ColorDef(factory.GetColor(0xB0B6BE)),           // DayHeaderBorder

                new ColorDef(factory.GetColor(0xFFFFFF)),           // DayWorkHoursBackground
                new ColorDef(factory.GetColor(0xC7CBD1)),           // DayAllDayEventBackground
                new ColorDef(factory.GetColor(0xE8EAEC)),           // DayOffWorkHoursBackground

                new ColorDef(factory.GetColor(0xC7CBD1)),           // DayHourBorder
                new ColorDef(factory.GetColor(0xE8EAEC)),           // DayHalfHourBorder

                new ColorDef(factory.GetColor(0x4C535C)),           // SelectionBackground

                new ColorDef(factory.GetColor(0x6197B1)),           // OwnerTabBorder

                new ColorDef(new Color[] {factory.GetColor(0xBCD2DE), factory.GetColor(0x90B6C8)},    // OwnerTabBackground
                new float[] {0f, 1f}, 90f),

                new ColorDef(factory.GetColor(0x000000)),           // OwnerTabForeground
                new ColorDef(factory.GetColor(0x90B6C8)),           // OwnerTabContentBackground
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
                new ColorDef(factory.GetColor(0xF0F1F2)),           // TimeRulerBackground
                new ColorDef(factory.GetColor(0x333333)),           // TimeRulerForeground
                new ColorDef(factory.GetColor(0x333333)),           // TimeRulerBorder
                new ColorDef(factory.GetColor(0x333333)),           // TimeRulerTickBorder

                new ColorDef(new Color[] {factory.GetColor(0xFFED79), factory.GetColor(0xFFD86B), factory.GetColor(0xFFBB00), factory.GetColor(0xFFEA77)},
                new float[] {0f, .55f ,58f, 1f}, 90f),              // TimeRulerIndicator

                new ColorDef(factory.GetColor(0xEB8900)),           // TimeRulerIndicatorBorder
            };

            #endregion

            #region MonthViewColors

            table.CalendarView.MonthViewColors = new ColorDef[]
            {
                new ColorDef(factory.GetColor(0x6197B1)),           // DayOfWeekHeaderBorder

                new ColorDef(new Color[] {factory.GetColor(0xE5EEF2), factory.GetColor(0xD8E5EB), factory.GetColor(0xC3D7E1), factory.GetColor(0xD2E2E9)}, 
                new float[] {0, .55f, .58f, 1}),                    // DayOfWeekHeaderBackground

                new ColorDef(factory.GetColor(0x446A96)),           // DayOfWeekHeaderForeground
                new ColorDef(factory.GetColor(0x6197B1)),           // SideBarBorder

                new ColorDef(new Color[] { factory.GetColor(0xE5EEF2), factory.GetColor(0xD8E5EB), factory.GetColor(0xC3D7E1), factory.GetColor(0xD2E2E9) },
                new float[] { 0, .55f, .58f, 1 }, 0),               // SideBarBackground
                
                new ColorDef(factory.GetColor(0x000000)),           // SideBarForeground
                new ColorDef(factory.GetColor(0x446A7C)),           // DayHeaderBorder

                new ColorDef(new Color[] { factory.GetColor(0xE5EEF2), factory.GetColor(0xD8E5EB), factory.GetColor(0xC3D7E1), factory.GetColor(0xD2E2E9) },
                new float[] { 0, .55f, .58f, 1 }),                  // DayHeaderBackground
                
                new ColorDef(factory.GetColor(0x000000)),           // DayHeaderForeground
                new ColorDef(factory.GetColor(0x6197B1)),           // DayContentBorder
                new ColorDef(factory.GetColor(0xE7EFF3)),           // DayContentSelectionBackground
                new ColorDef(factory.GetColor(0xFFFFFF)),           // DayContentActiveDayBackground
                new ColorDef(factory.GetColor(0xA8C5D4)),           // DayContentInactiveDayBackground

                new ColorDef(factory.GetColor(0x6197B1)),           // OwnerTabBorder

                new ColorDef(new Color[] {factory.GetColor(0xBCD2DE), factory.GetColor(0x90B6C8)},    // OwnerTabBackground
                new float[] {0f, 1f}, 90f),

                new ColorDef(factory.GetColor(0x000000)),           // OwnerTabForeground
                new ColorDef(factory.GetColor(0x90B6C8)),           // OwnerTabContentBackground
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
                factory.GetColor(0xFFFFFF), factory.GetColor(0xE3E8F4));

            table.SuperTab.InnerBorder = factory.GetColor(0xFCFEFF);
            table.SuperTab.OuterBorder = factory.GetColor(0x9196A2);

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
                new Color[] { factory.GetColor(0xF2F5FA), factory.GetColor(0xE5EAF5), factory.GetColor(0xCFD7EB), factory.GetColor(0xECEEFC) },
                new float[] { 0, .5f, .5f, 1 });

            table.SuperTabItem.Default.Normal.InnerBorder = factory.GetColor(0xFCFDFD);
            table.SuperTabItem.Default.Normal.OuterBorder = factory.GetColor(0x9196A2);
            table.SuperTabItem.Default.Normal.Text = factory.GetColor(0x000000);
            table.SuperTabItem.Default.Normal.CloseMarker = factory.GetColor(0x406F9F);

            // Top Selected

            table.SuperTabItem.Default.Selected.Background = new SuperTabLinearGradientColorTable(
                new Color[] { factory.GetColor(0xFBFDFE), factory.GetColor(0xE7F5FB), factory.GetColor(0xCFE7FA), factory.GetColor(0xB9D1FA) },
                new float[] { 0, .5f, .5f, 1 });

            table.SuperTabItem.Default.Selected.InnerBorder = factory.GetColor(0xFCFEFF);
            table.SuperTabItem.Default.Selected.OuterBorder = factory.GetColor(0x9196A2);
            table.SuperTabItem.Default.Selected.Text = factory.GetColor(0x000000);
            table.SuperTabItem.Default.Selected.CloseMarker = factory.GetColor(0x406F9F);

            // Top SelectedMouseOver

            table.SuperTabItem.Default.SelectedMouseOver.Background = new SuperTabLinearGradientColorTable(
                new Color[] { factory.GetColor(0xEDF3FC), factory.GetColor(0xC6DCF7), factory.GetColor(0x99C6EE), factory.GetColor(0xD9E9F9) },
                new float[] { 0, .5f, .5f, 1 });

            table.SuperTabItem.Default.SelectedMouseOver.InnerBorder = factory.GetColor(0xFEFEFF);
            table.SuperTabItem.Default.SelectedMouseOver.OuterBorder = factory.GetColor(0x9196A2);
            table.SuperTabItem.Default.SelectedMouseOver.Text = factory.GetColor(0x000000);
            table.SuperTabItem.Default.SelectedMouseOver.CloseMarker = factory.GetColor(0x406F9F);

            // Top MouseOver

            table.SuperTabItem.Default.MouseOver.Background = new SuperTabLinearGradientColorTable(
                new Color[] { factory.GetColor(0xEDF3FC), factory.GetColor(0xC6DCF7), factory.GetColor(0x99C6EE), factory.GetColor(0xD9E9F9) },
                new float[] { 0, .5f, .5f, 1 });

            table.SuperTabItem.Default.MouseOver.InnerBorder = factory.GetColor(0xFEFEFF);
            table.SuperTabItem.Default.MouseOver.OuterBorder = factory.GetColor(0x9196A2);
            table.SuperTabItem.Default.MouseOver.Text = factory.GetColor(0x000000);
            table.SuperTabItem.Default.MouseOver.CloseMarker = factory.GetColor(0x406F9F);

            // Left, Bottom, Right

            table.SuperTabItem.Left = table.SuperTabItem.Default;
            table.SuperTabItem.Bottom = table.SuperTabItem.Default;
            table.SuperTabItem.Right = table.SuperTabItem.Default;

            #endregion

            #region SuperTabPanel

            table.SuperTabPanel.Default.Background = new SuperTabLinearGradientColorTable(factory.GetColor(0xFFFFFF));
            table.SuperTabPanel.Default.InnerBorder = factory.GetColor(0xFCFEFF);
            table.SuperTabPanel.Default.OuterBorder = factory.GetColor(0x8D93A0);

            table.SuperTabPanel.Left = table.SuperTabPanel.Default;
            table.SuperTabPanel.Bottom = table.SuperTabPanel.Default;
            table.SuperTabPanel.Right = table.SuperTabPanel.Default;

            #endregion

            #endregion

            #region Backstage

            #region Backstage
            SuperTabStyleColorFactory.GetOffice2007BackstageVistaGlassColorTable(table.Backstage, factory);
            #endregion

            #region BackstageItem
            SuperTabStyleColorFactory.GetOffice2007BackstageVistaGlassItemColorTable(table.BackstageItem, factory);
            #endregion

            #region BackstagePanel
            SuperTabStyleColorFactory.GetOffice2007BackstageVistaGlassPanelColorTable(table.BackstagePanel, factory);
            #endregion

            #endregion

            #region SwitchButton
            SwitchButtonColorTable sbt = new SwitchButtonColorTable();
            sbt.BorderColor = factory.GetColor(0x8D93A0);
            sbt.OffBackColor = factory.GetColor(0xD5E5F5);
            sbt.OffTextColor = factory.GetColor(0x000000);
            sbt.OnBackColor = factory.GetColor(0x92D050);
            sbt.OnTextColor = factory.GetColor(0x000000);
            sbt.SwitchBackColor = factory.GetColor(0xA2B9D3);
            sbt.SwitchBorderColor = factory.GetColor(0x5C6672);
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
        }

        /// <summary>
        /// Initializes ColorScheme object with the black color scheme.
        /// </summary>
        /// <param name="c">ColorScheme object to initialize.</param>
        public static void InitializeBlackLegacyColors(ColorScheme c, ColorFactory factory)
        {
            c.BarBackground = factory.GetColor(0xFEFEFF);
            c.BarBackground2 = factory.GetColor(0xD4DBED);
            c.BarStripeColor = factory.GetColor(0x66707C);
            c.BarCaptionBackground = factory.GetColor(0x5C5C5C);
            c.BarCaptionBackground2 = factory.GetColor(0x000000);
            c.BarCaptionInactiveBackground = factory.GetColor(0x626C88);
            c.BarCaptionInactiveBackground2 = factory.GetColor(0x393F4D);
            c.BarCaptionInactiveText = factory.GetColor(0xFFFFFF);
            c.BarCaptionText = factory.GetColor(0xFFFFFF);
            c.BarFloatingBorder = factory.GetColor(0x485057);
            c.BarPopupBackground = factory.GetColor(0xF2F2F2);
            c.BarPopupBorder = factory.GetColor(0x646464);
            c.ItemBackground = Color.Empty;
            c.ItemBackground2 = Color.Empty;
            c.ItemCheckedBackground = factory.GetColor(0xEBF5FB);
            c.ItemCheckedBackground2 = factory.GetColor(0xCBE6F5);
            c.ItemCheckedBorder = factory.GetColor(0x3C7FB1);
            c.ItemCheckedText = factory.GetColor(0x000000);
            c.ItemDisabledBackground = Color.Empty;
            c.ItemDisabledText = factory.GetColor(0x8D8D8D);
            c.ItemExpandedShadow = Color.Empty;
            c.ItemExpandedBackground = factory.GetColor(0xEBF5FB);
            c.ItemExpandedBackground2 = factory.GetColor(0xCBE6F5);
            c.ItemExpandedText = factory.GetColor(0x000000);
            c.ItemHotBackground = factory.GetColor(0xE5F3FA);
            c.ItemHotBackground2 = factory.GetColor(0xD0ECFC);
            c.ItemHotBorder = factory.GetColor(0x96DBFA);
            c.ItemHotText = factory.GetColor(0x000000);
            c.ItemPressedBackground = factory.GetColor(0xE5F4FC);
            c.ItemPressedBackground2 = factory.GetColor(0x68B3DB);
            c.ItemPressedBorder = factory.GetColor(0x2C628B);
            c.ItemPressedText = factory.GetColor(0x000000);
            c.ItemSeparator = Color.FromArgb(225, factory.GetColor(0x9199A4));
            c.ItemSeparatorShade = Color.FromArgb(180, factory.GetColor(0xFFFFFF));
            c.ItemText = factory.GetColor(0x000000); // SystemColors.ControlText;
            c.MenuBackground = factory.GetColor(0xF0F0F0);
            c.MenuBackground2 = Color.Empty; // Color.White;
            c.MenuBarBackground = factory.GetColor(0xE5EAF5);
            c.MenuBorder = factory.GetColor(0x646464);
            c.ItemExpandedBorder = c.MenuBorder;
            c.MenuSide = factory.GetColor(0xF1F1F1);
            c.MenuSide2 = Color.Empty;
            c.MenuUnusedBackground = c.MenuBackground;
            c.MenuUnusedSide = factory.GetColor(0xE5E5E5);
            c.MenuUnusedSide2 = Color.Empty;
            c.ItemDesignTimeBorder = Color.Black;
            c.BarDockedBorder = factory.GetColor(0x65707B);

            c.DockSiteBackColor = factory.GetColor(0xEEF3FA);
            c.DockSiteBackColor2 = Color.Empty;

            c.CustomizeBackground = factory.GetColor(0x82AABF);
            c.CustomizeBackground2 = factory.GetColor(0x1E5E81);
            c.CustomizeText = factory.GetColor(0xFFFFFF);

            c.PanelBackground = factory.GetColor(0xFEFEFF);
            c.PanelBackground2 = factory.GetColor(0xE5EAF5);
            c.PanelText = Color.Black;
            c.PanelBorder = factory.GetColor(0x646E79);

            c.ExplorerBarBackground = factory.GetColor(0xD4DBED);
            c.ExplorerBarBackground2 = factory.GetColor(0xE1E6F6);

            c.MdiSystemItemForeground = Color.LightGray;
        }
        #endregion

        #region Style Class Creation

        public static ElementStyle GetStatusBarAltStyle(Office2007BarColorTable t)
        {
            ElementStyle style = new ElementStyle();
            style.Class = ElementStyleClassKeys.Office2007StatusBarBackground2Key;
            style.PaddingLeft = 4;
            style.BorderLeft = eStyleBorderType.Etched;
            style.BorderLeftWidth = 1;
            style.BorderLeftColor = Color.FromArgb(196, t.StatusBarTopBorder);
            style.BorderColorLight = Color.FromArgb(128, Color.White);
            if (t.StatusBarAltBackground.Count > 0)
            {
                style.BackColorBlend.CopyFrom(t.StatusBarAltBackground);
                style.BackColorGradientAngle = 90;
            }

            return style;
        }

        #endregion
    }
}
