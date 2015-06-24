using System;
using System.Drawing;

namespace DevComponents.Tree
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
			Node node=layoutInfo.ContextNode;
            int height=0;
			
			foreach(ColumnHeader col in node.NodesColumns)
			{
				if(!col.Visible)
					continue;

				if(col.SizeChanged)
				{
					// Column for child nodes is always placed below the current node and
					// is not included in the node's rectangle
					Rectangle bounds=Rectangle.Empty;
					bounds.X=x;
					bounds.Y=y;
					if(col.Width.Relative>0)
						bounds.Width=(clientWidth*col.Width.Relative)/100-1;
					else
						bounds.Width=col.Width.Absolute;

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

					col.SetBounds(bounds);
					col.SizeChanged=false;
					
					x+=(bounds.Width+cellHorizontalSpacing);

					if(bounds.Height>height)
						height=bounds.Height;
				}
				else if(col.Bounds.Height>height)
					height=col.Bounds.Height;
			}

			return height;
		}

	}
}
