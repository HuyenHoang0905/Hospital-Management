using System.Drawing;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Summary description for ButtonItemPainterHelper.
	/// </summary>
	internal class ButtonItemPainterHelper
	{
		public static Color GetTextColor(ButtonItem button, ItemPaintArgs pa)
		{
			Color textColor=SystemColors.ControlText;

			if(!ButtonItemPainter.IsItemEnabled(button, pa))
			{
				if(!pa.Colors.ItemDisabledText.IsEmpty)
					textColor=pa.Colors.ItemDisabledText;
				else
					textColor=SystemColors.ControlDark;
			}
			else if(button.IsMouseDown && !pa.Colors.ItemPressedText.IsEmpty)
			{
				textColor=pa.Colors.ItemPressedText;
			}
			else if(button.IsMouseOver)
			{
				if(!button.HotForeColor.IsEmpty)
					textColor=button.HotForeColor;
				else
					textColor=pa.Colors.ItemHotText;
			}
			else if(button.Expanded && !pa.Colors.ItemExpandedText.IsEmpty)
			{
				textColor=pa.Colors.ItemExpandedText;
			}
			else if(button.Checked && !pa.Colors.ItemCheckedText.IsEmpty)
			{
				textColor=pa.Colors.ItemCheckedText;
			}
			else
			{
				if(!button.ForeColor.IsEmpty)
					textColor=button.ForeColor;
				else
				{
					if(button.IsThemed && button.IsOnMenuBar && pa.Colors.ItemText==SystemColors.ControlText)
						textColor=SystemColors.MenuText;
					else
						textColor=pa.Colors.ItemText;
				}
			}

			return textColor;
		}
	}
}
