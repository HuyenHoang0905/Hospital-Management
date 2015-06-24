using System;
using System.Text;
using System.Drawing;

namespace DevComponents.DotNetBar.Rendering
{
    /// <summary>
    /// Represents color table for Office 2010 style.
    /// </summary>
    public class Office2010ColorTable : Office2007ColorTable
    {
        /// <summary>
        /// Initializes a new instance of the Office2010ColorTable class.
        /// </summary>
        public Office2010ColorTable()
        {
            Office2010BlueFactory.InitializeColorTable(this, ColorFactory.Empty);
            InitAppButtonColors(this, ColorFactory.Empty);
        }

        public Office2010ColorTable(eOffice2010ColorScheme color)
        {
            if(color == eOffice2010ColorScheme.Blue)
                Office2010BlueFactory.InitializeColorTable(this, ColorFactory.Empty);
            else if(color == eOffice2010ColorScheme.Silver)
                Office2010SilverFactory.InitializeColorTable(this, ColorFactory.Empty);
            else if (color == eOffice2010ColorScheme.Black)
                Office2010BlackFactory.InitializeColorTable(this, ColorFactory.Empty);
            else if (color == eOffice2010ColorScheme.VS2010)
                VisualStudio2010Factory.InitializeColorTable(this, ColorFactory.Empty);
            InitAppButtonColors(this, ColorFactory.Empty);
            _ColorScheme = color;
        }

        public Office2010ColorTable(eOffice2010ColorScheme color, Color blendColor)
        {
            ColorFactory factory = blendColor.IsEmpty ? ColorFactory.Empty : new ColorBlendFactory(blendColor);
            if (color == eOffice2010ColorScheme.Blue)
                Office2010BlueFactory.InitializeColorTable(this, factory);
            else if (color == eOffice2010ColorScheme.Silver)
                Office2010SilverFactory.InitializeColorTable(this, factory);
            else if (color == eOffice2010ColorScheme.Black)
                Office2010BlackFactory.InitializeColorTable(this, factory);
            else if (color == eOffice2010ColorScheme.VS2010)
                VisualStudio2010Factory.InitializeColorTable(this, factory);
            InitAppButtonColors(this, factory);
            _ColorScheme = color;
        }

        private eOffice2010ColorScheme _ColorScheme;
        /// <summary>
        /// Gets the color scheme color table is initialized with.
        /// </summary>
        public eOffice2010ColorScheme ColorScheme
        {
            get { return _ColorScheme; }
        }

        #region Application Button Colors
        internal static void InitAppButtonColors(Office2007ColorTable colorTable, ColorFactory factory)
        {
            Office2007ButtonItemColorTableCollection colors = colorTable.ApplicationButtonColors;
            colors.Clear();

            // Blue default
            Office2007ButtonItemColorTable table = new Office2007ButtonItemColorTable();
            table.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Blue);

            Office2007ButtonItemStateColorTable ct = new Office2007ButtonItemStateColorTable();
            ct.Background = new LinearGradientColorTable(factory.GetColor(0x3E7DDB), factory.GetColor(0x265FB2));
            ct.BottomBackgroundHighlight = new LinearGradientColorTable(factory.GetColor(64, 0xFFFFFF), Color.Transparent);
            ct.OuterBorder = new LinearGradientColorTable(factory.GetColor(0x1E48A0), factory.GetColor(0x244FA6));
            ct.InnerBorder = new LinearGradientColorTable(factory.GetColor(32, 0xFFFFFF));
            ct.Text = factory.GetColor(0xFFFFFF);
            table.Default = ct;

            ct = new Office2007ButtonItemStateColorTable();
            ct.Background = new LinearGradientColorTable(factory.GetColor(0x4786E5), factory.GetColor(0x2D65BC));
            ct.BottomBackgroundHighlight = new LinearGradientColorTable(factory.GetColor(82, 0xFFFFFF), Color.Transparent);
            ct.OuterBorder = new LinearGradientColorTable(factory.GetColor(0x1F48A1), factory.GetColor(0x2954A9));
            ct.InnerBorder = new LinearGradientColorTable(factory.GetColor(72, 0xFFFFFF), factory.GetColor(48, 0xFFFFFF)); //new LinearGradientColorTable(factory.GetColor(0x55A1F3), factory.GetColor(0x4F9EEE));
            ct.Text = factory.GetColor(0xFFFFFF);
            table.MouseOver = ct;

            ct = new Office2007ButtonItemStateColorTable();
            ct.Background = new LinearGradientColorTable(factory.GetColor(0x2861B6), factory.GetColor(0x1D5AB2));
            ct.BottomBackgroundHighlight = new LinearGradientColorTable(factory.GetColor(82, 0xFFFFFF), Color.Transparent);
            ct.OuterBorder = new LinearGradientColorTable(factory.GetColor(0x1F48A1), factory.GetColor(0x224EA6));
            ct.InnerBorder = new LinearGradientColorTable(factory.GetColor(72, 0xFFFFFF), factory.GetColor(48, 0xFFFFFF)); //new LinearGradientColorTable(factory.GetColor(0x55A1F3), factory.GetColor(0x4F9EEE));
            ct.Text = factory.GetColor(0xFFFFFF);
            table.Pressed = ct;

            table.Expanded = table.Pressed;
            table.Checked = table.Pressed;

            colors.Add(table);

            // Magenta
            table = new Office2007ButtonItemColorTable();
            table.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Magenta);

            ct = new Office2007ButtonItemStateColorTable();
            ct.Background = new LinearGradientColorTable(factory.GetColor(0xCC256B), factory.GetColor(0xB10851));
            ct.BottomBackgroundHighlight = new LinearGradientColorTable(factory.GetColor(64, 0xFFFFFF), Color.Transparent);
            ct.OuterBorder = new LinearGradientColorTable(factory.GetColor(0x8F013C), factory.GetColor(0x940741));
            ct.InnerBorder = new LinearGradientColorTable(factory.GetColor(32, 0xFFFFFF));
            ct.Text = factory.GetColor(0xFFFFFF);
            table.Default = ct;

            ct = new Office2007ButtonItemStateColorTable();
            ct.Background = new LinearGradientColorTable(factory.GetColor(0xD63272), factory.GetColor(0xB10B52));
            ct.BottomBackgroundHighlight = new LinearGradientColorTable(factory.GetColor(82, 0xFFFFFF), Color.Transparent);
            ct.OuterBorder = new LinearGradientColorTable(factory.GetColor(0x8F013C), factory.GetColor(0x950942));
            ct.InnerBorder = new LinearGradientColorTable(factory.GetColor(72, 0xFFFFFF), factory.GetColor(48, 0xFFFFFF)); //new LinearGradientColorTable(factory.GetColor(0x55A1F3), factory.GetColor(0x4F9EEE));
            ct.Text = factory.GetColor(0xFFFFFF);
            table.MouseOver = ct;

            ct = new Office2007ButtonItemStateColorTable();
            ct.Background = new LinearGradientColorTable(factory.GetColor(0xB50C53), factory.GetColor(0xB00B52));
            ct.BottomBackgroundHighlight = new LinearGradientColorTable(factory.GetColor(82, 0xFFFFFF), Color.Transparent);
            ct.OuterBorder = new LinearGradientColorTable(factory.GetColor(0x8F013D), factory.GetColor(0x950741));
            ct.InnerBorder = new LinearGradientColorTable(factory.GetColor(72, 0xFFFFFF), factory.GetColor(48, 0xFFFFFF)); //new LinearGradientColorTable(factory.GetColor(0x55A1F3), factory.GetColor(0x4F9EEE));
            ct.Text = factory.GetColor(0xFFFFFF);
            table.Pressed = ct;

            table.Expanded = table.Pressed;
            table.Checked = table.Pressed;

            colors.Add(table);

            // Orange
            table = new Office2007ButtonItemColorTable();
            table.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Orange);

            ct = new Office2007ButtonItemStateColorTable();
            ct.Background = new LinearGradientColorTable(factory.GetColor(0xF27350), factory.GetColor(0xE5552F));
            ct.BottomBackgroundHighlight = new LinearGradientColorTable(factory.GetColor(0xF89E42), Color.Transparent);
            ct.OuterBorder = new LinearGradientColorTable(factory.GetColor(0xCC2B12), factory.GetColor(0xCF3415));
            ct.InnerBorder = new LinearGradientColorTable(factory.GetColor(0xF68954), factory.GetColor(0xF78B3E));
            ct.Text = factory.GetColor(0xFFFFFF);
            table.Default = ct;

            ct = new Office2007ButtonItemStateColorTable();
            ct.Background = new LinearGradientColorTable(factory.GetColor(0xF87E4D), factory.GetColor(0xE6552E));
            ct.BottomBackgroundHighlight = new LinearGradientColorTable(factory.GetColor(0xFBBD5E), Color.Transparent);
            ct.OuterBorder = new LinearGradientColorTable(factory.GetColor(0xCA2810), factory.GetColor(0xCD3217));
            ct.InnerBorder = new LinearGradientColorTable(factory.GetColor(0xFBAA5A), factory.GetColor(0xFCB857)); //new LinearGradientColorTable(factory.GetColor(0x55A1F3), factory.GetColor(0x4F9EEE));
            ct.Text = factory.GetColor(0xFFFFFF);
            table.MouseOver = ct;

            ct = new Office2007ButtonItemStateColorTable();
            ct.Background = new LinearGradientColorTable(factory.GetColor(0xE3531D), factory.GetColor(0xE04E19));
            ct.BottomBackgroundHighlight = new LinearGradientColorTable(factory.GetColor(0xFDAF4C), Color.Transparent);
            ct.OuterBorder = new LinearGradientColorTable(factory.GetColor(0xCA2810), factory.GetColor(0xCD3013));
            ct.InnerBorder = new LinearGradientColorTable(factory.GetColor(0xE86229), factory.GetColor(0xFB8D44)); //new LinearGradientColorTable(factory.GetColor(0x55A1F3), factory.GetColor(0x4F9EEE));
            ct.Text = factory.GetColor(0xFFFFFF);
            table.Pressed = ct;

            table.Expanded = table.Pressed;
            table.Checked = table.Pressed;

            colors.Add(table);

            // Green
            table = new Office2007ButtonItemColorTable();
            table.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.BlueWithBackground);

            ct = new Office2007ButtonItemStateColorTable();
            ct.Background = new LinearGradientColorTable(factory.GetColor(0x459731), factory.GetColor(0x2B7F2C));
            ct.BottomBackgroundHighlight = new LinearGradientColorTable(factory.GetColor(0x6BCA45), Color.Transparent);
            ct.OuterBorder = new LinearGradientColorTable(factory.GetColor(0x186337), factory.GetColor(0x1E6A39));
            ct.InnerBorder = new LinearGradientColorTable(factory.GetColor(0x4CA231), factory.GetColor(0x53B331));
            ct.Text = factory.GetColor(0xFFFFFF);
            table.Default = ct;

            ct = new Office2007ButtonItemStateColorTable();
            ct.Background = new LinearGradientColorTable(factory.GetColor(0x469734), factory.GetColor(0x267C2B));
            ct.BottomBackgroundHighlight = new LinearGradientColorTable(factory.GetColor(0x89D668), Color.Transparent);
            ct.OuterBorder = new LinearGradientColorTable(factory.GetColor(0x196437), factory.GetColor(0x216B3C));
            ct.InnerBorder = new LinearGradientColorTable(factory.GetColor(0x65B943), factory.GetColor(0x70CC4A));
            ct.Text = factory.GetColor(0xFFFFFF);
            table.MouseOver = ct;

            ct = new Office2007ButtonItemStateColorTable();
            ct.Background = new LinearGradientColorTable(factory.GetColor(0x2F822A), factory.GetColor(0x2A7E2C));
            ct.BottomBackgroundHighlight = new LinearGradientColorTable(factory.GetColor(0x68CB38), Color.Transparent);
            ct.OuterBorder = new LinearGradientColorTable(factory.GetColor(0x186437), factory.GetColor(0x1D6A38));
            ct.InnerBorder = new LinearGradientColorTable(factory.GetColor(0x368F2B), factory.GetColor(0x59BD2D)); //new LinearGradientColorTable(factory.GetColor(0x55A1F3), factory.GetColor(0x4F9EEE));
            ct.Text = factory.GetColor(0xFFFFFF);
            table.Pressed = ct;

            table.Expanded = table.Pressed;
            table.Checked = table.Pressed;

            colors.Add(table);

            // Teal
            table = new Office2007ButtonItemColorTable();
            table.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.MagentaWithBackground);

            ct = new Office2007ButtonItemStateColorTable();
            ct.Background = new LinearGradientColorTable(factory.GetColor(0x159795), factory.GetColor(0x018281));
            ct.BottomBackgroundHighlight = new LinearGradientColorTable(factory.GetColor(0x20B7B4), Color.Transparent);
            ct.OuterBorder = new LinearGradientColorTable(factory.GetColor(0x055E5E), factory.GetColor(0x076464));
            ct.InnerBorder = new LinearGradientColorTable(factory.GetColor(0x1BA29D), factory.GetColor(0x1FB1A9));
            ct.Text = factory.GetColor(0xFFFFFF);
            table.Default = ct;

            ct = new Office2007ButtonItemStateColorTable();
            ct.Background = new LinearGradientColorTable(factory.GetColor(0x1F9C99), factory.GetColor(0x038584));
            ct.BottomBackgroundHighlight = new LinearGradientColorTable(factory.GetColor(0x36CDCA), Color.Transparent);
            ct.OuterBorder = new LinearGradientColorTable(factory.GetColor(0x055E5E), factory.GetColor(0x096767));
            ct.InnerBorder = new LinearGradientColorTable(factory.GetColor(0x30B8B3), factory.GetColor(0x34C2BE));
            ct.Text = factory.GetColor(0xFFFFFF);
            table.MouseOver = ct;

            ct = new Office2007ButtonItemStateColorTable();
            ct.Background = new LinearGradientColorTable(factory.GetColor(0x028482), factory.GetColor(0x028081));
            ct.BottomBackgroundHighlight = new LinearGradientColorTable(factory.GetColor(0x2CC2BE), Color.Transparent);
            ct.OuterBorder = new LinearGradientColorTable(factory.GetColor(0x055E5E), factory.GetColor(0x086565));
            ct.InnerBorder = new LinearGradientColorTable(factory.GetColor(0x028F8D), factory.GetColor(0x1DB4AD)); //new LinearGradientColorTable(factory.GetColor(0x55A1F3), factory.GetColor(0x4F9EEE));
            ct.Text = factory.GetColor(0xFFFFFF);
            table.Pressed = ct;

            table.Expanded = table.Pressed;
            table.Checked = table.Pressed;

            colors.Add(table);
        }
        #endregion
    }

    /// <summary>
    /// Defines the color scheme type for the Office2010ColorTable.
    /// </summary>
    public enum eOffice2010ColorScheme
    {
        /// <summary>
        /// Silver color scheme.
        /// </summary>
        Silver,
        /// <summary>
        /// Blue color scheme.
        /// </summary>
        Blue,
        /// <summary>
        /// Black color scheme.
        /// </summary>
        Black,
        /// <summary>
        /// Visual Studio 2010 Blue color scheme.
        /// </summary>
        VS2010
    }

}
