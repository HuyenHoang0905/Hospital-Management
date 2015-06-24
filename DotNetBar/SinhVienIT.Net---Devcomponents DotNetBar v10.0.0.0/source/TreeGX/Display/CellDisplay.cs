using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DevComponents.Tree.Display
{
	/// <summary>
	/// Represents cell display class.
	/// </summary>
	internal class CellDisplay
	{
		public CellDisplay()
		{
		}

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
			Rectangle r=ci.Cell.CheckBoxBoundsRelative;
			r.Offset(ci.CellOffset);
			System.Windows.Forms.ButtonState state=System.Windows.Forms.ButtonState.Normal;
			if(ci.Cell.Checked)
				state=System.Windows.Forms.ButtonState.Checked;
            System.Windows.Forms.ControlPaint.DrawCheckBox(ci.Graphics,r,state);

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
				ci.Graphics.DrawImage(image,r.X+(r.Width-image.Width)/2,
				                      r.Y+(r.Height-image.Height)/2);
			}
		}

		public static void PaintText(NodeCellRendererEventArgs ci)
		{
			Cell cell=ci.Cell;
			if(cell.HostedControl==null && (cell.Text=="" || ci.Style.TextColor.IsEmpty) || cell.TextContentBounds.IsEmpty )
				return;
			Rectangle bounds=ci.Cell.TextContentBounds;
			bounds.Offset(ci.CellOffset);
			
			if(cell.HostedControl!=null)
			{
//				if(ci.Graphics.Transform!=null)
//				{
//					Point[] p=new Point[] {new Point(bounds.X, bounds.Y), new Point(bounds.Right, bounds.Bottom)};
//					ci.Graphics.Transform.TransformPoints(p);
//					bounds = new Rectangle(p[0].X, p[0].Y, p[1].X-p[0].X, p[1].Y-p[0].Y);
//				}
//				if(cell.HostedControl.Bounds!=bounds)
//				{
//					cell.IgnoreHostedControlSizeChange = true;
//					cell.HostedControl.Bounds=bounds;
//					cell.IgnoreHostedControlSizeChange = false;
//				}
				if(!cell.HostedControl.Visible)
					cell.HostedControl.Visible = true;
				return;
			}

			Font font=ci.Style.Font;
			bounds.Inflate(1,1);
			if (cell.TextMarkupBody == null)
				TextDrawing.DrawString(ci.Graphics , cell.Text, font, ci.Style.TextColor, bounds, ci.Style.TextFormat);
			else
			{
				TextMarkup.MarkupDrawContext d = new TextMarkup.MarkupDrawContext(ci.Graphics, font, ci.Style.TextColor, false);
				d.HotKeyPrefixVisible = !((ci.Style.TextFormat & eTextFormat.HidePrefix) == eTextFormat.HidePrefix);
				Rectangle mr = new Rectangle(bounds.X, bounds.Y + (bounds.Height - cell.TextMarkupBody.Bounds.Height) / 2, cell.TextMarkupBody.Bounds.Width, cell.TextMarkupBody.Bounds.Height);
				mr.Offset((bounds.Width - mr.Width) / 2, 0);
				cell.TextMarkupBody.Bounds = mr;
				cell.TextMarkupBody.Render(d);
			}
		}

		private static Image GetCellImage(Cell cell)
		{
			Image img=cell.Images.Image;
			if(img==null && cell.Images.ImageIndex>=0)
				img=cell.Images.GetImageByIndex(cell.Images.ImageIndex);
			
			if(cell.IsMouseOver && (cell.Images.ImageMouseOver!=null || cell.Images.ImageMouseOverIndex>=0))
			{
				if(cell.Images.ImageMouseOver!=null)
					img=cell.Images.ImageMouseOver;
				else
					img=cell.Images.GetImageByIndex(cell.Images.ImageMouseOverIndex);
			}
			else if(cell.Parent.Expanded && (cell.Images.ImageExpanded!=null || cell.Images.ImageExpandedIndex>=0))
			{
				if(cell.Images.ImageExpanded!=null)
					img=cell.Images.ImageExpanded;
				else
					img=cell.Images.GetImageByIndex(cell.Images.ImageExpandedIndex);
			}
			return img;
		}

		public static Font GetCellFont(TreeGX tree, Cell cell)
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
	/// Represents information neccessary to paint the cell on canvas.
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
