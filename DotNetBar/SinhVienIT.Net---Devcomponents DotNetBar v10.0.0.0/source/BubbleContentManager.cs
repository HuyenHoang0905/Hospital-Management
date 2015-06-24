using System;
using System.Drawing;
using DevComponents.UI.ContentManager;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Represents class that provides mangification for the BubbleMar control
	/// </summary>
	public class BubbleContentManager:SerialContentLayoutManager
	{
		#region Private Variables
		private Size m_BubbleSize=Size.Empty;
		private float m_Factor1=0,m_Factor2=0,m_Factor3=0,m_Factor4=0;
		private int m_MouseOverIndex=-1;
		private int m_MouseOverPosition=-1;
		#endregion

		public BubbleContentManager()
		{
			this.ContentAlignment=eContentAlignment.Center;
		}

		#region IContentLayout

		/// <summary>
		/// Performs layout of the content block.
		/// </summary>
		/// <param name="containerBounds">Container bounds to layout content blocks in.</param>
		/// <param name="contentBlocks">Content blocks to layout.</param>
		/// <param name="blockLayout">Block layout manager that resizes the content blocks.</param>
		/// <returns>The bounds of the content blocks within the container bounds.</returns>
		public override Rectangle Layout(Rectangle containerBounds,IBlock[] contentBlocks,BlockLayoutManager blockLayout)
		{
			if(contentBlocks.Length==0)
				return Rectangle.Empty;

			if(m_MouseOverIndex==-1)
			{
				return base.Layout(containerBounds,contentBlocks,blockLayout);
			}

			BubbleButton[] buttons=new BubbleButton[contentBlocks.Length];
			contentBlocks.CopyTo(buttons,0);

			int x=0,y=0;
			if(this.ContentOrientation==eContentOrientation.Horizontal)
			{
				buttons[m_MouseOverIndex].SetMagnifiedDisplayRectangle(new Rectangle(m_MouseOverPosition,GetY(containerBounds,m_BubbleSize.Height),m_BubbleSize.Width,m_BubbleSize.Height));
				x=m_MouseOverPosition;
			}
			else
			{
				buttons[m_MouseOverIndex].SetMagnifiedDisplayRectangle(new Rectangle(GetX(containerBounds,m_BubbleSize.Width),m_MouseOverPosition,m_BubbleSize.Width,m_BubbleSize.Height));
				y=m_MouseOverPosition;
			}

			int growthWidth=m_BubbleSize.Width-contentBlocks[0].Bounds.Width;
			int growthHeight=m_BubbleSize.Height-contentBlocks[0].Bounds.Height;

			// Apply factor 2
			int index=GetPreviousButtonIndex(buttons,m_MouseOverIndex);
			if(index>=0)
				SetFactorPrevious(containerBounds,buttons[index],growthWidth,growthHeight,m_Factor2,ref x, ref y);
			
			// Apply factor 1
			index=GetPreviousButtonIndex(buttons,index);
			if(index>=0)
				SetFactorPrevious(containerBounds,buttons[index],growthWidth,growthHeight,m_Factor1,ref x, ref y);

			if(this.ContentOrientation==eContentOrientation.Horizontal)
			{
				while(index>=0)
				{
					index=GetPreviousButtonIndex(buttons,index);
					if(index>=0)
					{
						x-=(buttons[index].DisplayRectangle.Width+this.BlockSpacing);
						buttons[index].SetMagnifiedDisplayRectangle(new Rectangle(x,buttons[index].DisplayRectangle.Y,buttons[index].DisplayRectangle.Width,buttons[index].DisplayRectangle.Height));	
					}
				}
				x=m_MouseOverPosition+m_BubbleSize.Width+this.BlockSpacing;
			}
			else
			{
				while(index>=0)
				{
					index=GetPreviousButtonIndex(buttons,index);
					if(index>=0)
					{
						y-=(buttons[index].DisplayRectangle.Height+this.BlockSpacing);
						buttons[index].SetMagnifiedDisplayRectangle(new Rectangle(buttons[index].DisplayRectangle.X,y,buttons[index].DisplayRectangle.Width,buttons[index].DisplayRectangle.Height));
					}
				}

				y=m_MouseOverPosition+m_BubbleSize.Height+this.BlockSpacing;
			}
			
			// Apply factor 3
			index=GetNextButtonIndex(buttons,m_MouseOverIndex);
			if(index>=0)
				SetFactorNext(containerBounds,buttons[index],growthWidth,growthHeight,m_Factor3,ref x, ref y);

			// Apply factor 4
			if(index==-1) index=m_MouseOverIndex;
			index=GetNextButtonIndex(buttons,index);
			if(index>=0)
				SetFactorNext(containerBounds,buttons[index],growthWidth,growthHeight,m_Factor4,ref x, ref y);
			
			if(this.ContentOrientation==eContentOrientation.Horizontal)
			{
				while(index>=0)
				{
					index=GetNextButtonIndex(buttons,index);
					if(index>=0)
					{
						buttons[index].SetMagnifiedDisplayRectangle(new Rectangle(x,buttons[index].DisplayRectangle.Y,buttons[index].DisplayRectangle.Width,buttons[index].DisplayRectangle.Height));
						x+=(buttons[index].DisplayRectangle.Width+this.BlockSpacing);
					}
				}
			}
			else
			{
				while(index>=0)
				{
					index=GetNextButtonIndex(buttons,index);
					if(index>=0)
					{
						buttons[index].SetMagnifiedDisplayRectangle(new Rectangle(buttons[index].DisplayRectangle.X,y,buttons[index].DisplayRectangle.Width,buttons[index].DisplayRectangle.Height));
						y+=(buttons[index].DisplayRectangle.Height+this.BlockSpacing);
					}
				}
			}

			if(buttons.Length==1)
				return buttons[0].MagnifiedDisplayRectangle;
			
			return Rectangle.Union(buttons[0].MagnifiedDisplayRectangle,buttons[buttons.Length-1].MagnifiedDisplayRectangle);
		}

		private void SetFactorPrevious(Rectangle containerBounds, BubbleButton button, int growthWidth, int growthHeight, float factor, ref int x, ref int y)
		{
			int w=(int)(button.DisplayRectangle.Width+growthWidth*factor);
			int h=(int)(button.DisplayRectangle.Height+growthHeight*factor);
			if(this.ContentOrientation==eContentOrientation.Horizontal)
			{
				x-=(w+this.BlockSpacing);
				button.SetMagnifiedDisplayRectangle(new Rectangle(x,GetY(containerBounds,h),w,h));
			}
			else
			{
				y-=(h+this.BlockSpacing);
				button.SetMagnifiedDisplayRectangle(new Rectangle(GetX(containerBounds,w),y,w,h));
			}
		}

		private void SetFactorNext(Rectangle containerBounds, BubbleButton button, int growthWidth, int growthHeight, float factor, ref int x, ref int y)
		{
			int w=(int)(button.DisplayRectangle.Width+growthWidth*factor);
			int h=(int)(button.DisplayRectangle.Height+growthHeight*factor);
			if(this.ContentOrientation==eContentOrientation.Horizontal)
			{
				button.SetMagnifiedDisplayRectangle(new Rectangle(x,GetY(containerBounds,h),w,h));
				x+=(w+this.BlockSpacing);
			}
			else
			{
				button.SetMagnifiedDisplayRectangle(new Rectangle(GetX(containerBounds,w),y,w,h));
				y+=(h+this.BlockSpacing);
			}
		}

		private int GetNextButtonIndex(BubbleButton[] buttons, int index)
		{
			int next=-1;
			for(int i=index+1;i<buttons.Length;i++)
			{
				if(buttons[i].Visible)
				{
					next=i;
					break;
				}
			}
			return next;
		}

		private int GetPreviousButtonIndex(BubbleButton[] buttons, int index)
		{
			int next=-1;
			for(int i=index-1;i>=0;i--)
			{
				if(buttons[i].Visible)
				{
					next=i;
					break;
				}
			}
			return next;
		}

		private int GetX(Rectangle containerBounds, int width)
		{
			if(this.ContentOrientation==eContentOrientation.Vertical)
			{
				if(this.ContentVerticalAlignment==eContentVerticalAlignment.Top)
					return containerBounds.Left;
				else if(this.ContentVerticalAlignment==eContentVerticalAlignment.Middle)
					return containerBounds.Left+(containerBounds.Width-width)/2;
				else if(this.ContentVerticalAlignment==eContentVerticalAlignment.Bottom)
					return containerBounds.Right-width;
			}
			else
				throw new InvalidOperationException("Cannot use GetX method on eContentOrientation.Horizontal");
			return -1;
		}

		private int GetY(Rectangle containerBounds, int height)
		{
			if(this.ContentOrientation==eContentOrientation.Horizontal)
			{
				if(this.ContentVerticalAlignment==eContentVerticalAlignment.Top)
					return containerBounds.Top;
				else if(this.ContentVerticalAlignment==eContentVerticalAlignment.Middle)
					return containerBounds.Top+(containerBounds.Height-height)/2;
				else if(this.ContentVerticalAlignment==eContentVerticalAlignment.Bottom)
					return containerBounds.Bottom-height;
			}
			else
				throw new InvalidOperationException("Cannot use GetX method on eContentOrientation.Vertical");
			return -1;
		}
		#endregion

		#region Public Properties

		/// <summary>
		/// Gets or sets the bubble size for the mouse over item.
		/// </summary>
		public Size BubbleSize
		{
			get {return m_BubbleSize;}
			set {m_BubbleSize=value;}
		}

		/// <summary>
		/// Gets or sets magnification factor for the item that is at the position MouseOverIndex-1
		/// </summary>
		public float Factor1
		{
			get {return m_Factor1;}
			set {m_Factor1=value;}
		}

		/// <summary>
		/// Gets or sets magnification factor for the item that is at the position MouseOverIndex-2
		/// </summary>
		public float Factor2
		{
			get {return m_Factor2;}
			set {m_Factor2=value;}
		}

		/// <summary>
		/// Gets or sets magnification factor for the item that is at the position MouseOverIndex+1
		/// </summary>
		public float Factor3
		{
			get {return m_Factor3;}
			set {m_Factor3=value;}
		}

		/// <summary>
		/// Gets or sets magnification factor for the item that is at the position MouseOverIndex+2
		/// </summary>
		public float Factor4
		{
			get {return m_Factor4;}
			set {m_Factor4=value;}
		}

		/// <summary>
		/// Gets or sets the index of the item mouse is over.
		/// </summary>
		public int MouseOverIndex
		{
			get {return m_MouseOverIndex;}
			set {m_MouseOverIndex=value;}
		}

		/// <summary>
		/// Gets or sets new X coordinate of the mouse over item when in horizontal layout or Y
		/// coordinate when in vertical layout.
		/// </summary>
		public int MouseOverPosition
		{
			get {return m_MouseOverPosition;}
			set {m_MouseOverPosition=value;}
		}

		#endregion
	}
}
