using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.AdvTree.Display
{
	/// <summary>
	/// Represents cell display class.
	/// </summary>
	internal class CellDisplay
	{
		public CellDisplay()
		{
		}

        private static Office2007CheckBoxItemPainter _CheckBoxPainter;
        public static Office2007CheckBoxItemPainter CheckBoxPainter
        {
            get { return _CheckBoxPainter; }
            set { _CheckBoxPainter = value; }
        }

        public static Office2007CheckBoxColorTable ColorTable = null;

		public static void PaintCell(NodeCellRendererEventArgs ci)
		{
			if(ci.Cell.CheckBoxVisible)
				CellDisplay.PaintCellCheckBox(ci);
			if(!ci.Cell.Images.LargestImageSize.IsEmpty)
				CellDisplay.PaintCellImage(ci);
			CellDisplay.PaintText(ci);
		}

		public static void PaintCellCheckBox(NodeCellRendererEventArgs ci)
		{
			if(!ci.Cell.CheckBoxVisible)
				return;
            Cell cell = ci.Cell;
            Rectangle r = cell.CheckBoxBoundsRelative;
			r.Offset(ci.CellOffset);

            if (ci.CheckBoxImageChecked != null)
            {
                Image img = ci.CheckBoxImageChecked;
                if (cell.CheckState == System.Windows.Forms.CheckState.Unchecked)
                    img = ci.CheckBoxImageUnChecked;
                else if (cell.CheckState == System.Windows.Forms.CheckState.Indeterminate)
                    img = ci.CheckBoxImageIndeterminate;
                if (img != null)
                    ci.Graphics.DrawImage(img, r);
            }
            else if (_CheckBoxPainter != null)
            {
                Office2007CheckBoxStateColorTable ct = GetCheckBoxStateColorTable(ci);
                if (cell.CheckBoxStyle == eCheckBoxStyle.CheckBox)
                {
                    _CheckBoxPainter.PaintCheckBox(ci.Graphics, r, ct, cell.CheckState);
                }
                else
                {
                    _CheckBoxPainter.PaintRadioButton(ci.Graphics, r, ct, cell.Checked);
                }
            }
            else
            {
                System.Windows.Forms.ButtonState state = System.Windows.Forms.ButtonState.Normal;
                if (ci.Cell.Checked)
                    state = System.Windows.Forms.ButtonState.Checked;
                System.Windows.Forms.ControlPaint.DrawCheckBox(ci.Graphics, r, state);
            }
		}

        private static Office2007CheckBoxStateColorTable GetCheckBoxStateColorTable(NodeCellRendererEventArgs e)
        {
            Cell cell = e.Cell;

            if (ColorTable != null && BarFunctions.IsOffice2007Style(e.ColorScheme.Style))
            {
                Office2007CheckBoxColorTable ct = ColorTable;
                if (!cell.GetEnabled())
                    return ct.Disabled;
                //else if (cell.IsMouseDown)
                //    return ct.Pressed;
                //else if (cell.IsMouseOver)
                //    return ct.MouseOver;
                return ct.Default;
            }
            else
            {
                ColorScheme cs = e.ColorScheme;
                // Create color table based on the ColorScheme object...
                Office2007CheckBoxStateColorTable ct = new Office2007CheckBoxStateColorTable();
                if (!cell.GetEnabled())
                {
                    ct.CheckBackground = new LinearGradientColorTable(cs.MenuBackground, Color.Empty);
                    ct.CheckBorder = cs.ItemDisabledText;
                    ct.CheckInnerBorder = cs.ItemDisabledText;
                    ct.CheckInnerBackground = new LinearGradientColorTable();
                    ct.CheckSign = new LinearGradientColorTable(cs.ItemDisabledText, Color.Empty);
                    ct.Text = cs.ItemDisabledText;
                }
                //else if (cell.IsMouseDown)
                //{
                //    ct.CheckBackground = new LinearGradientColorTable(cs.MenuBackground, Color.Empty);
                //    ct.CheckBorder = cs.ItemPressedBorder;
                //    ct.CheckInnerBorder = cs.ItemPressedBorder;
                //    ct.CheckInnerBackground = new LinearGradientColorTable(cs.ItemPressedBackground, cs.ItemPressedBackground2);
                //    ct.CheckSign = new LinearGradientColorTable(cs.ItemPressedText, Color.Empty);
                //    ct.Text = cs.ItemPressedText;
                //}
                //else if (cell.IsMouseOver)
                //{
                //    ct.CheckBackground = new LinearGradientColorTable(cs.MenuBackground, Color.Empty);
                //    ct.CheckBorder = cs.ItemHotBorder;
                //    ct.CheckInnerBorder = cs.ItemHotBorder;
                //    ct.CheckInnerBackground = new LinearGradientColorTable(cs.ItemHotBackground, cs.ItemHotBackground2);
                //    ct.CheckSign = new LinearGradientColorTable(cs.ItemHotText, Color.Empty);
                //    ct.Text = cs.ItemHotText;
                //}
                else
                {
                    ct.CheckBackground = new LinearGradientColorTable(cs.MenuBackground, Color.Empty);
                    ct.CheckBorder = cs.PanelBorder;
                    ct.CheckInnerBorder = ColorBlendFactory.SoftLight(cs.PanelBorder, Color.White);
                    ct.CheckInnerBackground = new LinearGradientColorTable(cs.MenuBackground, Color.Empty);
                    ct.CheckSign = new LinearGradientColorTable(cs.ItemText, Color.Empty);
                    ct.Text = cs.ItemText;
                }
                return ct;
            }
        }

		public static void PaintCellImage(NodeCellRendererEventArgs ci)
		{
			if(ci.Cell.Images.LargestImageSize.IsEmpty)
				return;
			Rectangle r=ci.Cell.ImageBoundsRelative;
			r.Offset(ci.CellOffset);
			
			Image image = CellDisplay.GetCellImage(ci.Cell);
			
			if(image!=null)
			{
                ci.Graphics.DrawImage(image, r.X + (r.Width - image.Width) / 2,
                                      r.Y + (r.Height - image.Height) / 2, image.Width, image.Height);
			}
		}

		public static void PaintText(NodeCellRendererEventArgs ci)
		{
			Cell cell=ci.Cell;
			if(cell.HostedControl==null && cell.HostedItem==null && (cell.DisplayText=="" || ci.Style.TextColor.IsEmpty) || cell.TextContentBounds.IsEmpty )
				return;
            
			Rectangle bounds=ci.Cell.TextContentBounds;
			bounds.Offset(ci.CellOffset);
            
			if(cell.HostedControl!=null)
			{
				if(!cell.HostedControl.Visible)
					cell.HostedControl.Visible = true;
				return;
			}
            else if (cell.HostedItem != null)
            {
                BaseItem item = cell.HostedItem;
                item.LeftInternal = bounds.X;
                if (item.DisplayRectangle.Height < bounds.Height)
                    item.TopInternal = bounds.Y + (bounds.Height - item.DisplayRectangle.Height) / 2;
                else
                    item.TopInternal = bounds.Y;
                item.Displayed = true;
                item.Paint(ci.ItemPaintArgs);
                return;
            }

			Font font=ci.Style.Font;
            if (bounds.Width > 1 && bounds.Height > 1)
            {
                eTextFormat textFormat = ci.Style.TextFormat;
                textFormat = textFormat & ~(textFormat & eTextFormat.HidePrefix);
                textFormat |= eTextFormat.NoPrefix;
                if (cell.TextMarkupBody == null)
                {
                    TextDrawing.DrawString(ci.Graphics, cell.DisplayText, font, ci.Style.TextColor, bounds, ci.Style.TextFormat);
                }
                else
                {
                    DevComponents.DotNetBar.TextMarkup.MarkupDrawContext d = new DevComponents.DotNetBar.TextMarkup.MarkupDrawContext(ci.Graphics, font, ci.Style.TextColor, false);
                    d.HotKeyPrefixVisible = !((ci.Style.TextFormat & eTextFormat.HidePrefix) == eTextFormat.HidePrefix);
                    Rectangle mr = Rectangle.Empty;
                    eStyleTextAlignment lineAlignment = ci.Style.TextLineAlignment;
                    if (lineAlignment == eStyleTextAlignment.Center)
                    {
                        mr = new Rectangle(bounds.X, bounds.Y + (bounds.Height - cell.TextMarkupBody.Bounds.Height) / 2, cell.TextMarkupBody.Bounds.Width, cell.TextMarkupBody.Bounds.Height);
                    }
                    else if (lineAlignment == eStyleTextAlignment.Near)
                    {
                        mr = new Rectangle(bounds.X, bounds.Y, cell.TextMarkupBody.Bounds.Width, cell.TextMarkupBody.Bounds.Height);
                    }
                    else // Far
                    {
                        mr = new Rectangle(bounds.X, bounds.Y + (bounds.Height - cell.TextMarkupBody.Bounds.Height), cell.TextMarkupBody.Bounds.Width, cell.TextMarkupBody.Bounds.Height);
                    }
                    
                    cell.TextMarkupBody.Bounds = mr;
                    cell.TextMarkupBody.Render(d);
                }
            }
		}

		private static Image GetCellImage(Cell cell)
		{
			Image img=cell.Images.Image;

            bool enabled = cell.GetEnabled();

            if (!enabled && (cell.Images.ImageDisabled != null || cell.Images.ImageDisabledIndex >= 0 || cell.Images.DisabledImageGenerated != null))
            {
                if (cell.Images.DisabledImageGenerated != null) return cell.Images.DisabledImageGenerated;
                if (cell.Images.ImageDisabled != null) return cell.Images.ImageDisabled;
                if (cell.Images.ImageDisabledIndex >= 0) return cell.Images.GetImageByIndex(cell.Images.ImageDisabledIndex);
            }

            if(img == null && !string.IsNullOrEmpty(cell.Images.ImageKey))
                img = cell.Images.GetImageByKey(cell.Images.ImageKey);
            if (img == null && cell.Images.ImageIndex >= 0)
                img = cell.Images.GetImageByIndex(cell.Images.ImageIndex);

            if (!enabled && img is Bitmap)
            {
                cell.Images.DisposeGeneratedDisabledImage();
                cell.Images.DisabledImageGenerated = ImageHelper.CreateGrayScaleImage(img as Bitmap);
                if (cell.Images.DisabledImageGenerated != null) return cell.Images.DisabledImageGenerated;
                return img;
            }
			
			if(cell.IsMouseOver && (cell.Images.ImageMouseOver!=null || cell.Images.ImageMouseOverIndex>= 0 || !string.IsNullOrEmpty(cell.Images.ImageMouseOverKey)))
			{
                if (cell.Images.ImageMouseOver != null)
                    img = cell.Images.ImageMouseOver;
                else if (cell.Images.ImageMouseOverIndex >= 0)
                    img = cell.Images.GetImageByIndex(cell.Images.ImageMouseOverIndex);
                else
                    img = cell.Images.GetImageByKey(cell.Images.ImageMouseOverKey);
			}
			else if(cell.Parent.Expanded && (cell.Images.ImageExpanded!=null || cell.Images.ImageExpandedIndex>= 0 || !string.IsNullOrEmpty(cell.Images.ImageExpandedKey)))
			{
                if (cell.Images.ImageExpanded != null)
                    img = cell.Images.ImageExpanded;
                else if (cell.Images.ImageExpandedIndex >= 0)
                    img = cell.Images.GetImageByIndex(cell.Images.ImageExpandedIndex);
                else
                    img = cell.Images.GetImageByKey(cell.Images.ImageExpandedKey);
			}
			return img;
		}

		public static Font GetCellFont(AdvTree tree, Cell cell)
		{
			Font font=tree.Font;
			ElementStyle style=null;
			
			if(cell.StyleNormal!=null)
			{
				style=cell.StyleNormal;
			}
			else
			{
				if(tree.NodeStyle!=null)
					style=tree.NodeStyle;
				else
					style=new ElementStyle();

				if(tree.CellStyleDefault!=null)
					style=tree.CellStyleDefault;
				else
					style=ElementStyle.GetDefaultCellStyle(style);
			}

			if(style!=null && style.Font!=null)
				font=style.Font;

			return font;
		}
	}

	/// <summary>
	/// Represents information necessary to paint the cell on canvas.
	/// </summary>
	internal class CellDisplayInfo
	{
		public ElementStyle Style=null;
		public System.Drawing.Graphics Graphics=null;
		public Cell ContextCell=null;
		public Point CellOffset=Point.Empty;

		public CellDisplayInfo()
		{
		}

		public CellDisplayInfo(ElementStyle style, System.Drawing.Graphics g, Cell cell, Point cellOffset)
		{
			this.Style=style;
			this.Graphics=g;
			this.ContextCell=cell;
			this.CellOffset=cellOffset;
		}
	}
}
