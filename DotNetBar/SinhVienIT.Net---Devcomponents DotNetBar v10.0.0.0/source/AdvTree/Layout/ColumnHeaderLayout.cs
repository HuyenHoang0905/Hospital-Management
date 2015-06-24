using System;
using System.Drawing;
using DevComponents.DotNetBar;

namespace DevComponents.AdvTree.Layout
{
	/// <summary>
	/// Class that is used to layout column header.
	/// </summary>
	internal class ColumnHeaderLayout
	{
		public ColumnHeaderLayout()
		{
		}

		// Assumes that layoutInfo is up-to-date and that Node that is connected with
		// columns is already processed and it's size and location calculated.
		// layoutInfo.Top member reflects the next position below the node
		// layoutInfo.LevelOffset should reflect the X offset for the child nodes.
		public static int LayoutColumnHeader(NodeLayoutContextInfo layoutInfo,int x, int y, int clientWidth, int cellHorizontalSpacing)
		{
            ColumnHeaderCollection columns = null;
			Node node=layoutInfo.ContextNode;
            if (node == null)
                columns = layoutInfo.TreeColumns;
            else
                columns = node.NodesColumns;
            columns.UsesRelativeSize = false;
            int height=0;
            bool adjustHeight = false;
            Rectangle totalBounds = Rectangle.Empty;
            ColumnHeader lastVisibleColumn = null;
            ColumnHeader stretchToFillColumn = null;
            bool allRelative = true;
            bool firstVisible = true;
            for (int i = 0; i < columns.Count; i++)
			{
                ColumnHeader col = columns.ColumnAtDisplayIndex(i);
                col.IsLastVisible = false;
				if(!col.Visible)
					continue;
                col.IsFirstVisible = firstVisible;
                firstVisible = false;
				//if(col.SizeChanged)
				{
					// Column for child nodes is always placed below the current node and
					// is not included in the node's rectangle
					Rectangle bounds=Rectangle.Empty;
					bounds.X=x;
					bounds.Y=y;
                    if (col.Width.AutoSize)
                    {
                        int autoWidth = col.Width.AutoSizeWidth;
                        if (col.Width.AutoSizeMinHeader)
                        {
                            Font headerFont = layoutInfo.DefaultFont;
                            if (!string.IsNullOrEmpty(col.StyleNormal))
                            {
                                ElementStyle style = layoutInfo.Styles[col.StyleNormal];
                                if (style != null && style.Font != null)
                                    headerFont = style.Font;
                            }
                            else if (layoutInfo.ColumnStyle != null && layoutInfo.ColumnStyle.Font != null)
                                headerFont = layoutInfo.ColumnStyle.Font;
                            if (headerFont != null)
                            {
                                int columnHeaderTextWidth = (int)Math.Ceiling(layoutInfo.Graphics.MeasureString(col.Text, headerFont).Width) + 2;
                                autoWidth = Math.Max(autoWidth, columnHeaderTextWidth);
                            }
                        }
                        bounds.Width = autoWidth;
                        allRelative = false;
                    }
                    else if (col.Width.Absolute > 0)
                    {
                        bounds.Width = col.Width.Absolute;
                        allRelative = false;
                    }
                    else if (col.Width.Absolute == -1)
                    {
                        bounds.Width = 0;
                        allRelative = false;
                    }
                    else if (col.Width.Relative > 0)
                    {
                        if (col.IsFirstVisible)
                        {
                            clientWidth -= layoutInfo.ExpandPartWidth;
                            bounds.Width = (clientWidth * col.Width.Relative) / 100 - cellHorizontalSpacing;
                            bounds.Width += layoutInfo.ExpandPartWidth;
                        }
                        else
                            bounds.Width = (clientWidth * col.Width.Relative) / 100 - cellHorizontalSpacing;
                        columns.UsesRelativeSize = true;
                    }
                    lastVisibleColumn = col;

                    if (col.StretchToFill)
                        stretchToFillColumn = col;

					if(col.StyleNormal=="" && col.StyleMouseDown=="" && col.StyleMouseOver=="")
					{
						bounds.Height=layoutInfo.DefaultHeaderSize.Height;
					}
					else
					{
						Size sz=Size.Empty;
						if(col.StyleNormal!="")
						{
							ElementStyleLayout.CalculateStyleSize(layoutInfo.Styles[col.StyleNormal],layoutInfo.DefaultFont);
							sz=layoutInfo.Styles[col.StyleNormal].Size;
						}
												
						if(sz.Height==0)
							bounds.Height=layoutInfo.DefaultHeaderSize.Height;
						else
							bounds.Height=sz.Height;
					}

                    if (col.Image != null && col.Image.Height+4>bounds.Height)
                    {
                        bounds.Height = col.Image.Height + 4;
                    }

					col.SetBounds(bounds);
					col.SizeChanged=false;

                    x += (bounds.Width + cellHorizontalSpacing);

                    if (bounds.Height > height)
                    {
                        if (height > 0)
                            adjustHeight = true;
                        height = bounds.Height;
                    }
                    else if (bounds.Height < height)
                        adjustHeight = true;
				}
                
                if (totalBounds.IsEmpty)
                    totalBounds = col.Bounds;
                else
                    totalBounds = Rectangle.Union(totalBounds, col.Bounds);
			}
            if (adjustHeight)
            {
                foreach (ColumnHeader col in columns)
                {
                    col.SetBounds(new Rectangle(col.Bounds.X, col.Bounds.Y, col.Bounds.Width, height));
                }
            }
            if (lastVisibleColumn != null && allRelative)
            {
                lastVisibleColumn.SetBounds(new Rectangle(lastVisibleColumn.Bounds.X, lastVisibleColumn.Bounds.Y, lastVisibleColumn.Bounds.Width + cellHorizontalSpacing, lastVisibleColumn.Bounds.Height));
                totalBounds = Rectangle.Union(totalBounds, lastVisibleColumn.Bounds);
            }

            if (stretchToFillColumn != null && totalBounds.Width < clientWidth)
            {
                int stretch = clientWidth - totalBounds.Width;
                stretchToFillColumn.SetBounds(new Rectangle(stretchToFillColumn.Bounds.X, stretchToFillColumn.Bounds.Y,
                    stretchToFillColumn.Bounds.Width + stretch, stretchToFillColumn.Bounds.Height));
                totalBounds = Rectangle.Union(totalBounds, stretchToFillColumn.Bounds);
                if (!stretchToFillColumn.IsLastVisible) // Offset columns to the right if this was not last visible column
                {
                    int startIndex = columns.GetDisplayIndex(stretchToFillColumn) + 1;
                    for (int i = startIndex; i < columns.Count; i++)
                    {
                        ColumnHeader col = columns.ColumnAtDisplayIndex(i);
                        if (!col.Visible) continue;
                        col.SetBounds(new Rectangle(col.Bounds.X + stretch, col.Bounds.Y, col.Bounds.Width, col.Bounds.Height));
                        totalBounds = Rectangle.Union(totalBounds, col.Bounds);
                    }
                }
            }

            if (lastVisibleColumn != null) lastVisibleColumn.IsLastVisible = true;
            columns.SetBounds(totalBounds);
			return height;
		}

	}
}
