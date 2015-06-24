using System;
using System.Drawing;

namespace DevComponents.Tree
{
	namespace Layout
	{
		/// <summary>
		/// Represents class for Node's cell layout.
		/// </summary>
		internal class CellLayout
		{
			public CellLayout()
			{
			}

			/// <summary>
			/// Offset cell bounds, check box bounds, image bounds and text bounds by specified offset.
			/// </summary>
			/// <param name="cell">Cell to offset.</param>
			/// <param name="x">Horizontal offset in pixels.</param>
			/// <param name="y">Vertical offset in pixels.</param>
			public void Offset(Cell cell, int x, int y)
			{
				cell.SetBounds(new Rectangle(cell.BoundsRelative.X+x,cell.BoundsRelative.Y+y,cell.BoundsRelative.Width,cell.BoundsRelative.Height));
				if(!cell.CheckBoxBoundsRelative.IsEmpty)
					cell.SetCheckBoxBounds(new Rectangle(cell.CheckBoxBoundsRelative.X+x,cell.CheckBoxBoundsRelative.Y+y,cell.CheckBoxBoundsRelative.Width,cell.CheckBoxBoundsRelative.Height));
				if(!cell.ImageBoundsRelative.IsEmpty)
					cell.SetImageBounds(new Rectangle(cell.ImageBoundsRelative.X+x,cell.ImageBoundsRelative.Y+y,cell.ImageBoundsRelative.Width,cell.ImageBoundsRelative.Height));
				if(!cell.TextContentBounds.IsEmpty)
					cell.TextContentBounds=new Rectangle(cell.TextContentBounds.X+x,cell.TextContentBounds.Y+y,cell.TextContentBounds.Width,cell.TextContentBounds.Height);
			}

			public void LayoutSingleCell(LayoutCellInfo info)
			{
				Size textSize=Size.Empty;
				Font font=info.Font;
				int height=0;
				if(info.LayoutStyle.Font!=null)
					font=info.LayoutStyle.Font;

				info.ContextCell.OnLayoutCell();

				if(info.ContextCell.HostedControl!=null)
				{
					Size controlSize=info.ContextCell.HostedControl.Size;
					if(!info.ContextCell.HostedControlSize.IsEmpty)
						controlSize = info.ContextCell.HostedControlSize;
					if(info.CellWidth==0)
						textSize=new Size(controlSize.Width,controlSize.Height);
					else
					{
						int availTextWidth=info.CellWidth-
						                   ElementStyleLayout.HorizontalStyleWhiteSpace(info.LayoutStyle);
						textSize=new Size(availTextWidth,controlSize.Height);
					}
				}
				else
				{
					// Calculate Text Width and Height
					if(info.CellWidth==0)
					{
						if (info.ContextCell.TextMarkupBody == null)
						{
							string text=info.ContextCell.Text;
							if(text!="")
							{
								if(info.LayoutStyle.WordWrap && info.LayoutStyle.MaximumWidth>0)
									textSize=TextDrawing.MeasureString(info.Graphics, text, font, info.LayoutStyle.MaximumWidth);
								else if(info.ContextCell.Parent!=null && info.ContextCell.Parent.Style!=null && info.ContextCell.Parent.Style.WordWrap && info.ContextCell.Parent.Style.MaximumWidth>0)
									textSize=TextDrawing.MeasureString(info.Graphics, text, font, info.ContextCell.Parent.Style.MaximumWidth);
								else
									textSize=TextDrawing.MeasureString(info.Graphics, text, font);
							}
						}
						else
						{
							Size availSize = new Size(1600, 1);
							if(info.LayoutStyle.WordWrap && info.LayoutStyle.MaximumWidth>0)
								availSize.Width = info.LayoutStyle.MaximumWidth;
							else if(info.ContextCell.Parent!=null && info.ContextCell.Parent.Style!=null && info.ContextCell.Parent.Style.WordWrap && info.ContextCell.Parent.Style.MaximumWidth>0)
								availSize.Width = info.ContextCell.Parent.Style.MaximumWidth;

							TextMarkup.MarkupDrawContext d = new TextMarkup.MarkupDrawContext(info.Graphics, font, Color.Empty, false);
							info.ContextCell.TextMarkupBody.Measure(availSize, d);
							availSize = info.ContextCell.TextMarkupBody.Bounds.Size;
							d.RightToLeft = !info.LeftToRight;
							info.ContextCell.TextMarkupBody.Arrange(new Rectangle(0, 0, availSize.Width, availSize.Height), d);

							textSize = info.ContextCell.TextMarkupBody.Bounds.Size;
						}
					}
					else
					{
						int availTextWidth=info.CellWidth-
						                   ElementStyleLayout.HorizontalStyleWhiteSpace(info.LayoutStyle);

						availTextWidth-=info.ContextCell.Images.LargestImageSize.Width;
						if(info.ContextCell.CheckBoxVisible)
							availTextWidth-=CheckBoxSize.Width;
						int cellHeight=font.Height;

						if(info.LayoutStyle.WordWrap)
						{
							cellHeight=info.LayoutStyle.MaximumHeight-info.LayoutStyle.MarginTop-
							           info.LayoutStyle.MarginBottom-info.LayoutStyle.PaddingTop-info.LayoutStyle.PaddingBottom;
							
							if (info.ContextCell.TextMarkupBody == null)
							{
								if(availTextWidth>0)
								{
									if(cellHeight>0)
										textSize=TextDrawing.MeasureString(info.Graphics, info.ContextCell.Text,font,new Size(availTextWidth,cellHeight),info.LayoutStyle.TextFormat);
									else
										textSize=TextDrawing.MeasureString(info.Graphics, info.ContextCell.Text, font, availTextWidth, info.LayoutStyle.TextFormat);
								}
							}
							else
							{
								Size availSize = new Size(availTextWidth, 1);
								TextMarkup.MarkupDrawContext d = new TextMarkup.MarkupDrawContext(info.Graphics, font, Color.Empty, false);
								info.ContextCell.TextMarkupBody.Measure(availSize, d);
								availSize = info.ContextCell.TextMarkupBody.Bounds.Size;
								availSize.Width = availTextWidth;
								d.RightToLeft = !info.LeftToRight;
								info.ContextCell.TextMarkupBody.Arrange(new Rectangle(0, 0, availSize.Width, availSize.Height), d);

								textSize = info.ContextCell.TextMarkupBody.Bounds.Size;
							}
						}
						else
							textSize=new Size(availTextWidth,cellHeight);
					}
				}

				if(info.LayoutStyle.WordWrap)
					info.ContextCell.WordWrap=true;
				else
					info.ContextCell.WordWrap=false;

				height=(int)Math.Ceiling((double)textSize.Height);
				if(info.VerticalPartAlignment)
				{
					if(info.ContextCell.Images.LargestImageSize.Height>0)
						height+=info.ContextCell.Images.LargestImageSize.Height;
					if(info.ContextCell.CheckBoxVisible)
						height+=CheckBoxSize.Height;
				}
				else
				{
					if(info.ContextCell.Images.LargestImageSize.Height>height)
						height=info.ContextCell.Images.LargestImageSize.Height;
					if(info.ContextCell.CheckBoxVisible && CheckBoxSize.Height>height)
						height=CheckBoxSize.Height;
				}

				Rectangle r=new Rectangle(info.Left+ElementStyleLayout.LeftWhiteSpace(info.LayoutStyle),
				                          info.Top+ElementStyleLayout.TopWhiteSpace(info.LayoutStyle)
				                          ,info.CellWidth,height);

				if(r.Width==0)
				{
					if(info.VerticalPartAlignment)
					{
						r.Width=(int)Math.Ceiling((double)textSize.Width);
						if(info.ContextCell.Images.LargestImageSize.Width>r.Width)
							r.Width=(info.ContextCell.Images.LargestImageSize.Width+this.ImageTextSpacing);
						if(info.ContextCell.CheckBoxVisible && CheckBoxSize.Width>r.Width)
							r.Width+=(CheckBoxSize.Width+this.ImageTextSpacing);
					}
					else
					{
						r.Width=(int)Math.Ceiling((double)textSize.Width);
						if(info.ContextCell.Images.LargestImageSize.Width>0)
							r.Width+=(info.ContextCell.Images.LargestImageSize.Width+this.ImageTextSpacing);
						if(info.ContextCell.CheckBoxVisible)
							r.Width+=(CheckBoxSize.Width+this.ImageTextSpacing);
					}
				}

				// Now that we have cell bounds store them
				Rectangle rCellBounds=new Rectangle(info.Left,info.Top,info.CellWidth,r.Height+info.LayoutStyle.MarginTop+info.LayoutStyle.MarginBottom+info.LayoutStyle.PaddingTop+info.LayoutStyle.PaddingBottom);
				if(rCellBounds.Width==0)
					rCellBounds.Width=r.Width+ElementStyleLayout.HorizontalStyleWhiteSpace(info.LayoutStyle);
				info.ContextCell.SetBounds(rCellBounds);

				// Set Position of the image
				if(!info.ContextCell.Images.LargestImageSize.IsEmpty)
				{
					eVerticalAlign va=GetVerticalAlign(info.ContextCell.ImageAlignment);
					eHorizontalAlign ha=GetHorizontalAlign(info.ContextCell.ImageAlignment,info.LeftToRight);
					if(info.VerticalPartAlignment)
						info.ContextCell.SetImageBounds(AlignContentVertical(info.ContextCell.Images.LargestImageSize, ref r, ha, va, this.ImageTextSpacing));
					else
						info.ContextCell.SetImageBounds(AlignContent(info.ContextCell.Images.LargestImageSize, ref r, ha, va, this.ImageTextSpacing));
				}
				else
					info.ContextCell.SetImageBounds(Rectangle.Empty);

				// Set position of the check box
				if(info.ContextCell.CheckBoxVisible)
				{
					eVerticalAlign va=GetVerticalAlign(info.ContextCell.CheckBoxAlignment);
					eHorizontalAlign ha=GetHorizontalAlign(info.ContextCell.CheckBoxAlignment,info.LeftToRight);
					if(info.VerticalPartAlignment)
						info.ContextCell.SetCheckBoxBounds(AlignContentVertical(this.CheckBoxSize, ref r, ha, va, this.ImageTextSpacing));
					else
						info.ContextCell.SetCheckBoxBounds(AlignContent(this.CheckBoxSize, ref r, ha, va, this.ImageTextSpacing));
				}
				else
					info.ContextCell.SetCheckBoxBounds(Rectangle.Empty);
			
				// Set position of the text
				//info.ContextCell.SetTextBounds(Rectangle.Empty);
				if(!textSize.IsEmpty)
					info.ContextCell.TextContentBounds=r;
				else
					info.ContextCell.TextContentBounds=Rectangle.Empty;
            
			}

			private Rectangle AlignContent(System.Drawing.Size contentSize, ref Rectangle boundingRectangle, eHorizontalAlign horizAlign, eVerticalAlign vertAlign, int contentSpacing)
			{
				Rectangle contentRect=new Rectangle(Point.Empty,contentSize);
				switch(horizAlign)
				{
					case eHorizontalAlign.Right:
						{
							contentRect.X=boundingRectangle.Right-contentRect.Width;
							boundingRectangle.Width-=(contentRect.Width+contentSpacing);
							break;
						}
						//case eHorizontalAlign.Left:
					default:
						{
							contentRect.X=boundingRectangle.X;
							boundingRectangle.X=contentRect.Right+contentSpacing;
							boundingRectangle.Width-=(contentRect.Width+contentSpacing);
							break;
						}
//				case eHorizontalAlign.Center:
//				{
//					contentRect.X=boundingRectangle.X+(boundingRectangle.Width-contentRect.Width)/2;
//					break;
//				}
				}

				switch(vertAlign)
				{
					case eVerticalAlign.Top:
						{
							contentRect.Y=boundingRectangle.Y;
							break;
						}
					case eVerticalAlign.Middle:
						{
							contentRect.Y=boundingRectangle.Y+(boundingRectangle.Height-contentRect.Height)/2;
							break;
						}
					case eVerticalAlign.Bottom:
						{
							contentRect.Y=boundingRectangle.Bottom-contentRect.Height;
							break;
						}
				}

				return contentRect;
			}

			private Rectangle AlignContentVertical(System.Drawing.Size contentSize, ref Rectangle boundingRectangle, eHorizontalAlign horizAlign, eVerticalAlign vertAlign, int contentSpacing)
			{
				Rectangle contentRect=new Rectangle(Point.Empty,contentSize);
				switch(horizAlign)
				{
					case eHorizontalAlign.Left:
						{
							contentRect.X=boundingRectangle.X;
							break;
						}
					case eHorizontalAlign.Right:
						{
							contentRect.X=boundingRectangle.Right-contentRect.Width;
							break;
						}
					case eHorizontalAlign.Center:
						{
							contentRect.X=boundingRectangle.X+(boundingRectangle.Width-contentRect.Width)/2;
							break;
						}
				}

				switch(vertAlign)
				{
					case eVerticalAlign.Bottom:
						{
							contentRect.Y=boundingRectangle.Bottom-contentRect.Height;
							boundingRectangle.Height-=(contentRect.Height+contentSpacing);
							break;
						}
						//case eVerticalAlign.Top:
					default:
						{
							contentRect.Y=boundingRectangle.Y;
							boundingRectangle.Y=contentRect.Bottom+contentSpacing;
							boundingRectangle.Height-=(contentRect.Height+contentSpacing);
							break;
						}
//				case eVerticalAlign.Middle:
//				{
//					contentRect.Y=boundingRectangle.Y+(boundingRectangle.Height-contentRect.Height)/2;
//					break;
//				}
				}

				return contentRect;
			}

			private eHorizontalAlign GetHorizontalAlign(eCellPartAlignment align, bool leftToRight)
			{
				if(((align==eCellPartAlignment.NearBottom || align==eCellPartAlignment.NearCenter ||
				     align==eCellPartAlignment.NearTop) && leftToRight) ||
				   ((align==eCellPartAlignment.FarBottom || align==eCellPartAlignment.FarCenter ||
				     align==eCellPartAlignment.FarTop) && !leftToRight))
					return eHorizontalAlign.Left;
				else if(align==eCellPartAlignment.CenterBottom || align==eCellPartAlignment.CenterTop)
					return eHorizontalAlign.Center;
				return eHorizontalAlign.Right;
			}

			private eVerticalAlign GetVerticalAlign(eCellPartAlignment align)
			{
				eVerticalAlign va=eVerticalAlign.Middle;

				switch(align)
				{
					case eCellPartAlignment.FarBottom:
					case eCellPartAlignment.NearBottom:
					case eCellPartAlignment.CenterBottom:
						va=eVerticalAlign.Bottom;
						break;
					case eCellPartAlignment.FarTop:
					case eCellPartAlignment.NearTop:
					case eCellPartAlignment.CenterTop:
						va=eVerticalAlign.Top;
						break;
				}

				return va;
			}

			private System.Drawing.Size CheckBoxSize
			{
				get
				{
					return new Size(12,12);
				}
			}

			/// <summary>
			/// Returns spacing between check box and image if both are displayed
			/// </summary>
			private int ImageCheckBoxSpacing
			{
				get {return 1;}
			}

			/// <summary>
			/// Returns spacing between image or checkbox and text
			/// </summary>
			private int ImageTextSpacing
			{
				get {return 1;}
			}

			/// <summary>
			/// Returns horizontal spacing between cells in a node
			/// </summary>
			public int CellHorizontalSpacing
			{
				get {return 1;}
			}

			/// <summary>
			/// Returns vertical spacing between cells in a node
			/// </summary>
			public int CellVerticalSpacing
			{
				get {return 1;}
			}

			/// <summary>
			/// Spacing between different parts of the cell, like image, option button, text and expand button area
			/// </summary>
			public int CellPartSpacing
			{
				get {return 1;}
			}

			public Size LayoutCells(NodeLayoutContextInfo layoutInfo, int x, int y)
			{
				eCellLayout layout=layoutInfo.CellLayout;
				if(layoutInfo.ContextNode.CellLayout!=layoutInfo.CellLayout && layoutInfo.ContextNode.CellLayout!=eCellLayout.Default)
					layout=layoutInfo.ContextNode.CellLayout;
				if(layout==eCellLayout.Horizontal || layout==eCellLayout.Default)
					return this.LayoutCellsHorizontal(layoutInfo,x,y);
				else
					return this.LayoutCellsVertical(layoutInfo,x,y);
			}

			private Size LayoutCellsHorizontal(NodeLayoutContextInfo layoutInfo, int x, int y)
			{
				Node node=layoutInfo.ContextNode;
				int height=0, width=0;

				for(int i=0;i<node.Cells.Count;i++)
				{
					Cell cell=node.Cells[i];

					bool bCellVisible=true;

					// Setup cell layout helper class
					LayoutCellInfo cellLayout=this.GetLayoutCellInfo();
					cellLayout.Top=y;
					cellLayout.Left=x;
					cellLayout.CellWidth=0;
					cellLayout.ContextCell=cell;
					cellLayout.Graphics=layoutInfo.Graphics;
					cellLayout.LeftToRight=layoutInfo.LeftToRight;
					cellLayout.Font=layoutInfo.DefaultFont;
					if(cell.Layout!=eCellPartLayout.Default)
						cellLayout.VerticalPartAlignment=(cell.Layout==eCellPartLayout.Vertical);
					else if(layoutInfo.CellPartLayout!=eCellPartLayout.Default)
						cellLayout.VerticalPartAlignment=(layoutInfo.CellPartLayout==eCellPartLayout.Vertical);

				
					if(layoutInfo.DefaultColumns.Count>0 || layoutInfo.ChildColumns!=null && layoutInfo.ChildColumns.Count>0)
					{
						ColumnInfo ci=null;
						if(layoutInfo.ChildColumns!=null && layoutInfo.ChildColumns.Count>0)
							ci=layoutInfo.ChildColumns[i] as ColumnInfo;
						else
							ci=layoutInfo.DefaultColumns[i] as ColumnInfo;
					
						bCellVisible=ci.Visible;
						cellLayout.CellWidth=ci.Width;
					}

					// Prepare union style
					if(cell.StyleNormal!=null)
						cellLayout.LayoutStyle=cell.StyleNormal;
					else
					{
						if(layoutInfo.ContextNode.Style!=null)
						{
							ElementStyle styleCopy = layoutInfo.DefaultCellStyle.Copy();
							styleCopy.ApplyStyle(layoutInfo.ContextNode.Style);
							cellLayout.LayoutStyle = styleCopy;
						}
						else
							cellLayout.LayoutStyle=layoutInfo.DefaultCellStyle;
					}

					this.LayoutSingleCell(cellLayout);

					if(bCellVisible)
					{
						x+=cell.BoundsRelative.Width;
						width+=cell.BoundsRelative.Width;
						if(cell.BoundsRelative.Width>0)
						{
							x+=this.CellHorizontalSpacing;
							width+=this.CellHorizontalSpacing;
						}
						if(cell.BoundsRelative.Height>height)
							height=cell.BoundsRelative.Height;
					}
				}

				// Take last added spacing off
				x-=this.CellHorizontalSpacing;
				width-=this.CellHorizontalSpacing;

				return new Size(width,height);
			}

			private Size LayoutCellsVertical(NodeLayoutContextInfo layoutInfo, int x, int y)
			{
				Node node=layoutInfo.ContextNode;
				int height=0, width=0;
				eHorizontalAlign align=eHorizontalAlign.Center;
				int iVisibleCells=0;

				for(int i=0;i<node.Cells.Count;i++)
				{
					Cell cell=node.Cells[i];

					bool bCellVisible=true;

					// Setup cell layout helper class
					LayoutCellInfo cellLayout=this.GetLayoutCellInfo();
					cellLayout.Top=y;
					cellLayout.Left=x;
					cellLayout.CellWidth=0;
					cellLayout.ContextCell=cell;
					cellLayout.Graphics=layoutInfo.Graphics;
					cellLayout.LeftToRight=layoutInfo.LeftToRight;
					cellLayout.Font=layoutInfo.DefaultFont;
					if(cell.Layout!=eCellPartLayout.Default)
						cellLayout.VerticalPartAlignment=(cell.Layout==eCellPartLayout.Vertical);
					else if(layoutInfo.CellPartLayout!=eCellPartLayout.Default)
						cellLayout.VerticalPartAlignment=(layoutInfo.CellPartLayout==eCellPartLayout.Vertical);
					
					if(layoutInfo.DefaultColumns.Count>0 || layoutInfo.ChildColumns!=null && layoutInfo.ChildColumns.Count>0)
					{
						ColumnInfo ci=null;
						if(layoutInfo.ChildColumns!=null && layoutInfo.ChildColumns.Count>0)
							ci=layoutInfo.ChildColumns[i] as ColumnInfo;
						else
							ci=layoutInfo.DefaultColumns[i] as ColumnInfo;
					
						bCellVisible=ci.Visible;
						cellLayout.CellWidth=ci.Width;
					}

					// Prepare union style
					if(cell.StyleNormal!=null)
						cellLayout.LayoutStyle=cell.StyleNormal;
					else
						cellLayout.LayoutStyle=layoutInfo.DefaultCellStyle;

					this.LayoutSingleCell(cellLayout);

					cell.SetVisible(bCellVisible);
					if(bCellVisible)
					{
						iVisibleCells++;
						y+=cell.BoundsRelative.Height;
						height+=cell.BoundsRelative.Height;
						if(cell.BoundsRelative.Height>0)
						{
							y+=this.CellVerticalSpacing;
							height+=this.CellVerticalSpacing;
						}
						if(cell.BoundsRelative.Width>width)
							width=cell.BoundsRelative.Width;
					}
				}

				// Take last added spacing off
				y-=this.CellVerticalSpacing;
				height-=this.CellVerticalSpacing;

				// Additional pass needed if horizontal alignment is other than left and there is more than one cell visible
				if(align!=eHorizontalAlign.Left && iVisibleCells>1)
				{
					foreach(Cell cell in node.Cells)
					{
						if(!cell.IsVisible)
							continue;
						if(align==eHorizontalAlign.Center)
							this.Offset(cell,(width-cell.BoundsRelative.Width)/2,0);
						else // Right aligned cells
							this.Offset(cell,width-cell.BoundsRelative.Width,0);
					}
				}

				return new Size(width,height);
			}

			private LayoutCellInfo m_LayoutCellInfo=null;
			private LayoutCellInfo GetLayoutCellInfo()
			{
				if(m_LayoutCellInfo==null)
					m_LayoutCellInfo=new LayoutCellInfo();
				return m_LayoutCellInfo;
			}
		}
	}

	internal class LayoutCellInfo
	{
		public Cell ContextCell=null;
		public int CellWidth=0;
		public System.Drawing.Graphics Graphics=null;
		public System.Drawing.Font Font=null;
		public int Left=0;
		public int Top=0;
		public ElementStyle LayoutStyle=null;
		public bool LeftToRight=true;
		public bool VerticalPartAlignment=false;

		public LayoutCellInfo()
		{
		}
	}

	internal class ColumnInfo
	{
		public bool Visible;
		public int Width;
		public ColumnInfo(int width, bool visible)
		{
			this.Width=width;
			this.Visible=visible;
		}
	}
}
