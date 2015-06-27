using System;
using System.Text;
using System.Drawing;

namespace DevComponents.DotNetBar.Rendering
{
    public class ButtonItemStaticColorTables
    {
        public static Office2007ButtonItemColorTable CreateBlueOrbColorTable()
        {
            return CreateBlueOrbColorTable(new ColorFactory());
        }

        public static Office2007ButtonItemColorTable CreateBlueOrbColorTable(ColorFactory factory)
        {
            DevComponents.DotNetBar.Rendering.Office2007ButtonItemColorTable table = new DevComponents.DotNetBar.Rendering.Office2007ButtonItemColorTable();
            table.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.BlueOrb);

            // Define standard colors when mouse is not over the button
            table.Default = new DevComponents.DotNetBar.Rendering.Office2007ButtonItemStateColorTable();
            table.Default.OuterBorder.Start = factory.GetColor(0x5273AF);
            table.Default.OuterBorder.End = factory.GetColor(0x08387F);
            table.Default.InnerBorder.Start = factory.GetColor(0xD6DFEF);
            table.Default.InnerBorder.End = factory.GetColor(192, 0x5699D4);
            table.Default.TopBackground.Start = factory.GetColor(0xB5BEDE);
            table.Default.TopBackground.End = factory.GetColor(0x3568B1);
            table.Default.BottomBackground.Start = factory.GetColor(0x001B5F);
            table.Default.BottomBackground.End = factory.GetColor(0x4BD6FF);
            table.Default.BottomBackgroundHighlight.Start = factory.GetColor(0x56DAFF);
            table.Default.BottomBackgroundHighlight.End = Color.Transparent;
            table.Default.Text = factory.GetColor(0xFFFFFF);

            // Define colors when mouse is over the button
            table.MouseOver = new DevComponents.DotNetBar.Rendering.Office2007ButtonItemStateColorTable();
            table.MouseOver.OuterBorder.Start = factory.GetColor(0x5185C8);
            table.MouseOver.OuterBorder.End = factory.GetColor(0x0754D2);
            table.MouseOver.InnerBorder.Start = factory.GetColor(0xDBE7F7);
            table.MouseOver.InnerBorder.End = factory.GetColor(128, 0x6677AB);
            table.MouseOver.TopBackground.Start = factory.GetColor(0xB5CAEB);
            table.MouseOver.TopBackground.End = factory.GetColor(0x448BD6);
            table.MouseOver.BottomBackground.Start = factory.GetColor(0x003B91);
            table.MouseOver.BottomBackground.End = factory.GetColor(0x68C5FB);
            table.MouseOver.BottomBackgroundHighlight.Start = factory.GetColor(0xBDFFFF);
            table.MouseOver.BottomBackgroundHighlight.End = Color.Transparent;
            table.MouseOver.Text = factory.GetColor(0xFFFFFF);

            // Define colors when mouse is pressed
            table.Pressed = new DevComponents.DotNetBar.Rendering.Office2007ButtonItemStateColorTable();
            table.Pressed.OuterBorder.Start = factory.GetColor(0x303949);
            table.Pressed.OuterBorder.End = factory.GetColor(0x022963);
            table.Pressed.InnerBorder.Start = factory.GetColor(0x9CA8BD);
            table.Pressed.InnerBorder.End = Color.Transparent;
            table.Pressed.TopBackground.Start = factory.GetColor(0x747C95);
            table.Pressed.TopBackground.End = factory.GetColor(0x324D7B);
            table.Pressed.BottomBackground.Start = factory.GetColor(0x003A70);
            table.Pressed.BottomBackground.End = factory.GetColor(0x00599B);
            table.Pressed.BottomBackgroundHighlight.Start = factory.GetColor(0x61DAFF);
            table.Pressed.BottomBackgroundHighlight.End = Color.Transparent;
            table.Pressed.Text = factory.GetColor(0xFFFFFF);

            // Define disabled button colors
            table.Disabled = new DevComponents.DotNetBar.Rendering.Office2007ButtonItemStateColorTable();
            table.Disabled.OuterBorder.Start = factory.GetColor(0x8498A9);
            table.Disabled.OuterBorder.End = factory.GetColor(0x343947);
            table.Disabled.InnerBorder.Start = Color.Transparent;
            table.Disabled.InnerBorder.End = factory.GetColor(0xD7E4F3);
            table.Disabled.TopBackground.Start = factory.GetColor(0xF4F7FA);
            table.Disabled.TopBackground.End = factory.GetColor(0xC5D9E8);
            table.Disabled.BottomBackground.Start = factory.GetColor(0xAEC2D8);
            table.Disabled.BottomBackground.End = factory.GetColor(0xBED2E8);
            table.Disabled.Text = factory.GetColor(0xDCEAF2);

            // Define colors button is expanded
            table.Expanded = new DevComponents.DotNetBar.Rendering.Office2007ButtonItemStateColorTable();
            table.Expanded.OuterBorder.Start = factory.GetColor(0x5273AF);
            table.Expanded.OuterBorder.End = factory.GetColor(0x08387F);
            table.Expanded.InnerBorder.Start = factory.GetColor(0xD6DFEF);
            table.Expanded.InnerBorder.End = factory.GetColor(192, 0x5699D4);
            table.Expanded.TopBackground.Start = factory.GetColor(0xB5BEDE);
            table.Expanded.TopBackground.End = factory.GetColor(0x3568B1);
            table.Expanded.BottomBackground.Start = factory.GetColor(0x001656);
            table.Expanded.BottomBackground.End = factory.GetColor(0x4BD6FF);
            table.Expanded.BottomBackgroundHighlight.Start = factory.GetColor(0x56DAFF);
            table.Expanded.BottomBackgroundHighlight.End = Color.Transparent;
            table.Expanded.Text = factory.GetColor(0xFFFFFF);

            // Define colors when mouse is pressed
            table.Checked = new DevComponents.DotNetBar.Rendering.Office2007ButtonItemStateColorTable();
            table.Checked.OuterBorder.Start = factory.GetColor(0x303949);
            table.Checked.OuterBorder.End = factory.GetColor(0x022963);
            table.Checked.InnerBorder.Start = factory.GetColor(0x9CA8BD);
            table.Checked.InnerBorder.End = Color.Transparent;
            table.Checked.TopBackground.Start = factory.GetColor(0xB5BEDE);
            table.Checked.TopBackground.End = factory.GetColor(0x3568B1);
            table.Checked.BottomBackground.Start = factory.GetColor(0x000636);
            table.Checked.BottomBackground.End = factory.GetColor(0x00599B);
            table.Checked.BottomBackgroundHighlight.Start = factory.GetColor(0x61DAFF);
            table.Checked.BottomBackgroundHighlight.End = Color.Transparent;
            table.Checked.Text = factory.GetColor(0xFFFFFF);

            return table;
        }
    }
}
