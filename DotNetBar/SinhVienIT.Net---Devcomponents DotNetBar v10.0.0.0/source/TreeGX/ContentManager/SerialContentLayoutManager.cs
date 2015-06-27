using System;
using System.Collections;
using System.Drawing;

#if TREEGX
namespace DevComponents.Tree.TextMarkup
#elif DOTNETBAR
namespace DevComponents.UI.ContentManager
#endif

{
	/// <summary>
	/// Represents the serial content layout manager that arranges content blocks in series next to each other.
	/// </summary>
	public class SerialContentLayoutManager:IContentLayout
    {
        #region Events
        /// <summary>
        /// Occurs when X, Y position of next block is calcualted.
        /// </summary>
        public event LayoutManagerPositionEventHandler NextPosition;

        /// <summary>
        /// Occurs before new block is layed out.
        /// </summary>
        public event LayoutManagerLayoutEventHandler BeforeNewBlockLayout;
        #endregion

        #region Private Variables
        private int m_BlockSpacing=0;
		private bool m_FitContainerOversize=false;
		private bool m_FitContainer=false;
        private bool m_VerticalFitContainerWidth = false;
		private eContentOrientation m_ContentOrientation=eContentOrientation.Horizontal;
		private eContentAlignment m_ContentAlignment=eContentAlignment.Left;
		private eContentVerticalAlignment m_ContentVerticalAlignment=eContentVerticalAlignment.Middle;
        private eContentVerticalAlignment m_BlockLineAlignment = eContentVerticalAlignment.Middle;
		private bool m_EvenHeight=false;
		private bool m_MultiLine=false;
        private bool m_RightToLeft = false;
		#endregion

		/// <summary>
		/// Creates new instance of the class.
		/// </summary>
		public SerialContentLayoutManager()
		{
		}

		#region IContentLayout Members
		/// <summary>
		/// Performs layout of the content block.
		/// </summary>
		/// <param name="containerBounds">Container bounds to layout content blocks in.</param>
		/// <param name="contentBlocks">Content blocks to layout.</param>
		/// <param name="blockLayout">Block layout manager that resizes the content blocks.</param>
		/// <returns>The bounds of the content blocks within the container bounds.</returns>
		public virtual Rectangle Layout(Rectangle containerBounds, IBlock[] contentBlocks, BlockLayoutManager blockLayout)
		{
			Rectangle blocksBounds=Rectangle.Empty;
			Point position=containerBounds.Location;
			ArrayList lines=new ArrayList();
			lines.Add(new BlockLineInfo());
			BlockLineInfo currentLine=lines[0] as BlockLineInfo;
            bool switchToNewLine = false;
            int visibleIndex = 0;

			foreach(IBlock block in contentBlocks)
			{
				if(!block.Visible)
                {
                    block.Bounds = Rectangle.Empty;
					continue;
                }

                if (BeforeNewBlockLayout != null)
                {
                    LayoutManagerLayoutEventArgs e = new LayoutManagerLayoutEventArgs(block, position, visibleIndex);
                    BeforeNewBlockLayout(this, e);
                    position = e.CurrentPosition;
                    if (e.CancelLayout)
                        continue;
                }
                visibleIndex++;

                Size availableSize = containerBounds.Size;
                bool isBlockElement = false;
                bool isNewLineTriggger = false;
                if (block is IBlockExtended)
                {
                    IBlockExtended ex = block as IBlockExtended;
                    isBlockElement = ex.IsBlockElement;
                    isNewLineTriggger = ex.IsNewLineAfterElement;
                }

                if (!isBlockElement)
                {
                    if (m_ContentOrientation == eContentOrientation.Horizontal)
                        availableSize.Width = (containerBounds.Right - position.X);
                    else
                        availableSize.Height = (containerBounds.Bottom - position.Y);
                }

				// Resize the content block
                blockLayout.Layout(block, availableSize);

				if(m_MultiLine && currentLine.Blocks.Count > 0)
				{
                    if (m_ContentOrientation == eContentOrientation.Horizontal && position.X + block.Bounds.Width > containerBounds.Right || isBlockElement || switchToNewLine)
					{
						position.X=containerBounds.X;
						position.Y+=(currentLine.LineSize.Height+m_BlockSpacing);
						currentLine=new BlockLineInfo();
						currentLine.Line=lines.Count;
						lines.Add(currentLine);
					}
                    else if (m_ContentOrientation == eContentOrientation.Vertical && position.Y + block.Bounds.Height > containerBounds.Bottom || isBlockElement || switchToNewLine)
					{
						position.Y=containerBounds.Y;
						position.X+=(currentLine.LineSize.Width+m_BlockSpacing);
						currentLine=new BlockLineInfo();
						currentLine.Line=lines.Count;
						lines.Add(currentLine);
					}
				}

				if(m_ContentOrientation==eContentOrientation.Horizontal)
				{
					if(block.Bounds.Height>currentLine.LineSize.Height)
						currentLine.LineSize.Height=block.Bounds.Height;
					currentLine.LineSize.Width=position.X+block.Bounds.Width-containerBounds.X;
				}
				else if(m_ContentOrientation==eContentOrientation.Vertical)
				{
					if(block.Bounds.Width>currentLine.LineSize.Width)
						currentLine.LineSize.Width=block.Bounds.Width;
					currentLine.LineSize.Height=position.Y+block.Bounds.Height-containerBounds.Y;
				}

				currentLine.Blocks.Add(block);
				block.Bounds=new Rectangle(position,block.Bounds.Size);

				if(blocksBounds.IsEmpty)
					blocksBounds=block.Bounds;
				else
					blocksBounds=Rectangle.Union(blocksBounds,block.Bounds);

                switchToNewLine = isBlockElement | isNewLineTriggger;

                position=GetNextPosition(block, position);    
			}

			blocksBounds=AlignResizeBlocks(containerBounds, blocksBounds, lines);

            if (m_RightToLeft)
                blocksBounds = MirrorContent(containerBounds, blocksBounds, contentBlocks);

			return blocksBounds;
		}

		#endregion

		#region Internals

		private Rectangle AlignResizeBlocks(Rectangle containerBounds,Rectangle blocksBounds,ArrayList lines)
		{
			Rectangle newBounds=Rectangle.Empty;
			if(containerBounds.IsEmpty || blocksBounds.IsEmpty || ((BlockLineInfo)lines[0]).Blocks.Count==0)
				return newBounds;
	
			if(m_ContentAlignment==eContentAlignment.Left && m_ContentVerticalAlignment==eContentVerticalAlignment.Top && 
                !m_FitContainer && !m_FitContainerOversize && !m_EvenHeight)
				return blocksBounds;

			Point[] offset=new Point[lines.Count];
			Size[] sizeOffset=new Size[lines.Count];
			foreach(BlockLineInfo lineInfo in lines)
			{
				if(m_ContentOrientation==eContentOrientation.Horizontal)
				{
					if(m_FitContainer && containerBounds.Width>lineInfo.LineSize.Width ||
						m_FitContainerOversize && lineInfo.LineSize.Width>containerBounds.Width)
					{
						sizeOffset[lineInfo.Line].Width=(containerBounds.Width-lineInfo.LineSize.Width)/lineInfo.Blocks.Count;
						blocksBounds.Width=containerBounds.Width;
					}
				}
				else
				{
					if(m_FitContainer && containerBounds.Height>lineInfo.LineSize.Height ||
						m_FitContainerOversize && lineInfo.LineSize.Height>containerBounds.Height)
					{
						sizeOffset[lineInfo.Line].Height=(containerBounds.Height-lineInfo.LineSize.Height)/lineInfo.Blocks.Count;
						blocksBounds.Height=containerBounds.Height;
					}

                    if (m_VerticalFitContainerWidth && containerBounds.Width > blocksBounds.Width)
                        sizeOffset[lineInfo.Line].Width = (containerBounds.Width - lineInfo.LineSize.Width) / lines.Count;
				}


				if(m_ContentOrientation==eContentOrientation.Horizontal && !m_FitContainer)
				{
					if(containerBounds.Width>blocksBounds.Width && m_FitContainerOversize || !m_FitContainerOversize)
					{
						switch(m_ContentAlignment)
						{
							case eContentAlignment.Right:
								offset[lineInfo.Line].X=containerBounds.Width-lineInfo.LineSize.Width;
								break;
							case eContentAlignment.Center:
								offset[lineInfo.Line].X=(containerBounds.Width-lineInfo.LineSize.Width)/2;
								break;
						}
					}
				}

				if(m_ContentOrientation==eContentOrientation.Vertical && !m_FitContainer)
				{
					if(containerBounds.Height>blocksBounds.Height && m_FitContainerOversize || !m_FitContainerOversize)
					{
						switch(m_ContentVerticalAlignment)
						{
							case eContentVerticalAlignment.Bottom:
								offset[lineInfo.Line].Y=containerBounds.Height-lineInfo.LineSize.Height;
								break;
							case eContentVerticalAlignment.Middle:
								offset[lineInfo.Line].Y=(containerBounds.Height-lineInfo.LineSize.Height)/2;
								break;
						}
					}
				}
			}

            if (m_VerticalFitContainerWidth && containerBounds.Width > blocksBounds.Width)
                blocksBounds.Width = containerBounds.Width;

			if(m_ContentOrientation==eContentOrientation.Horizontal)
			{
				foreach(BlockLineInfo lineInfo in lines)
				{
					foreach(IBlock block in lineInfo.Blocks)
					{
						if(!block.Visible)
							continue;
						Rectangle r=block.Bounds;
						if(m_EvenHeight && lineInfo.LineSize.Height>0)
							r.Height=lineInfo.LineSize.Height;
						r.Offset(offset[lineInfo.Line]);
						
						if(m_ContentVerticalAlignment==eContentVerticalAlignment.Middle)
						{
							// Takes care of offset rounding error when both content is vertically centered and blocks in line are centered
							if (m_BlockLineAlignment == eContentVerticalAlignment.Middle)
								r.Offset(0,((containerBounds.Height-blocksBounds.Height)+(lineInfo.LineSize.Height-r.Height))/2);
							else
								r.Offset(0,(containerBounds.Height-blocksBounds.Height)/2);

							// Line alignment of the block
							if (m_BlockLineAlignment == eContentVerticalAlignment.Bottom)
								r.Offset(0, lineInfo.LineSize.Height - r.Height);
						}
						else if(m_ContentVerticalAlignment==eContentVerticalAlignment.Bottom)
							r.Offset(0,containerBounds.Height-blocksBounds.Height);
						
						// To avoid rounding offset errors when dividing this is split see upper part
						if(m_ContentVerticalAlignment!=eContentVerticalAlignment.Middle)
						{
							// Line alignment of the block
							if (m_BlockLineAlignment == eContentVerticalAlignment.Middle)
								r.Offset(0, (lineInfo.LineSize.Height - r.Height) / 2);
							else if (m_BlockLineAlignment == eContentVerticalAlignment.Bottom)
								r.Offset(0, lineInfo.LineSize.Height - r.Height);
						}

						if(sizeOffset[lineInfo.Line].Width!=0)
						{
							r.Width+=sizeOffset[lineInfo.Line].Width;
							offset[lineInfo.Line].X+=sizeOffset[lineInfo.Line].Width;
						}
						r.Height+=sizeOffset[lineInfo.Line].Height;
						block.Bounds=r;
						if(newBounds.IsEmpty)
							newBounds=block.Bounds;
						else
							newBounds=Rectangle.Union(newBounds,block.Bounds);
					}
					// Adjust for left-over size adjustment for odd difference between container width and the total block width
					if(sizeOffset[lineInfo.Line].Width!=0 && containerBounds.Width-(lineInfo.LineSize.Width+sizeOffset[lineInfo.Line].Width*lineInfo.Blocks.Count)!=0)
					{
						Rectangle r=((IBlock)lineInfo.Blocks[lineInfo.Blocks.Count-1]).Bounds;
						r.Width+=containerBounds.Width-(lineInfo.LineSize.Width+sizeOffset[lineInfo.Line].Width*lineInfo.Blocks.Count);
						((IBlock)lineInfo.Blocks[lineInfo.Blocks.Count-1]).Bounds=r;
					}
				}
			}
			else
			{
				foreach(BlockLineInfo lineInfo in lines)
				{
					foreach(IBlock block in lineInfo.Blocks)
					{
						if(!block.Visible)
							continue;
						Rectangle r=block.Bounds;
						if(m_EvenHeight && lineInfo.LineSize.Width>0)
							r.Width=lineInfo.LineSize.Width;
						r.Offset(offset[lineInfo.Line]);
						if(m_ContentAlignment==eContentAlignment.Center)
							r.Offset(((containerBounds.Width-blocksBounds.Width)+(lineInfo.LineSize.Width-r.Width))/2,0); //r.Offset((containerBounds.Width-blocksBounds.Width)/2+(lineInfo.LineSize.Width-r.Width)/2,0);
						else if(m_ContentAlignment==eContentAlignment.Right)
							r.Offset((containerBounds.Width-blocksBounds.Width)+lineInfo.LineSize.Width-r.Width,0);
						r.Width+=sizeOffset[lineInfo.Line].Width;
						if(sizeOffset[lineInfo.Line].Height!=0)
						{
							r.Height+=sizeOffset[lineInfo.Line].Height;
							offset[lineInfo.Line].Y+=sizeOffset[lineInfo.Line].Height;
						}
						block.Bounds=r;
						if(newBounds.IsEmpty)
							newBounds=block.Bounds;
						else
							newBounds=Rectangle.Union(newBounds,block.Bounds);
					}
					if(sizeOffset[lineInfo.Line].Height!=0 && containerBounds.Height-(lineInfo.LineSize.Height+sizeOffset[lineInfo.Line].Height*lineInfo.Blocks.Count)!=0)
					{
						Rectangle r=((IBlock)lineInfo.Blocks[lineInfo.Blocks.Count-1]).Bounds;
						r.Height+=containerBounds.Height-(lineInfo.LineSize.Height+sizeOffset[lineInfo.Line].Height*lineInfo.Blocks.Count);
						((IBlock)lineInfo.Blocks[lineInfo.Blocks.Count-1]).Bounds=r;
					}
				}
			}
			return newBounds;
		}

		private Point GetNextPosition(IBlock block, Point position)
		{
            if (NextPosition != null)
            {
                LayoutManagerPositionEventArgs e = new LayoutManagerPositionEventArgs();
                e.Block = block;
                e.CurrentPosition = position;
                NextPosition(this, e);
                if (e.Cancel)
                    return e.NextPosition;
            }

			if(m_ContentOrientation==eContentOrientation.Horizontal)
				position.X+=block.Bounds.Width+m_BlockSpacing;
			else
				position.Y+=block.Bounds.Height+m_BlockSpacing;
			return position;
		}

		private class BlockLineInfo
		{
			public BlockLineInfo() {}
			public ArrayList Blocks=new ArrayList();
			public Size LineSize=Size.Empty;
			public int Line=0;
		}

        private Rectangle MirrorContent(Rectangle containerBounds, Rectangle blockBounds, IBlock[] contentBlocks)
        {
            int xOffset = (blockBounds.X - containerBounds.X);

            if (blockBounds.Width < containerBounds.Width)
                blockBounds.X = containerBounds.Right - ((blockBounds.X - containerBounds.X) + blockBounds.Width);
            else if (blockBounds.Width > containerBounds.Width)
                containerBounds.Width = blockBounds.Width;
            foreach (IBlock block in contentBlocks)
            {
                if (!block.Visible)
                    continue;
                Rectangle r = block.Bounds;
                block.Bounds = new Rectangle(containerBounds.Right - ((r.X - containerBounds.X) + r.Width), r.Y, r.Width, r.Height);
            }

            return blockBounds;
        }
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the spacing in pixels between content blocks. Default value is 0.
		/// </summary>
		public virtual int BlockSpacing
		{
			get {return m_BlockSpacing;}
			set {m_BlockSpacing=value;}
		}

		/// <summary>
		/// Gets or sets whether content blocks are forced to fit the container bounds if they 
		/// occupy more space than it is available by container. Default value is false.
		/// </summary>
		public virtual bool FitContainerOversize
		{
			get {return m_FitContainerOversize;}
			set {m_FitContainerOversize=value;}
		}

		/// <summary>
		/// Gets or sets whether content blocks are resized to fit the container bound if they
		/// occupy less space than it is available by container. Default value is false.
		/// </summary>
		public virtual bool FitContainer
		{
			get {return m_FitContainer;}
			set {m_FitContainer=value;}
		}

        /// <summary>
        /// Gets or sets whether content blocks are resized (Width) to fit container bounds if they
        /// occupy less space than the actual container width. Applies to the Vertical orientation only. Default value is false.
        /// </summary>
        public virtual bool VerticalFitContainerWidth
        {
            get { return m_VerticalFitContainerWidth; }
            set { m_VerticalFitContainerWidth = value; }
        }

		/// <summary>
		/// Gets or sets the content orientation. Default value is Horizontal.
		/// </summary>
		public virtual eContentOrientation ContentOrientation
		{
			get {return m_ContentOrientation;}
			set {m_ContentOrientation=value;}
		}

		/// <summary>
		/// Gets or sets the content vertical alignment. Default value is Middle.
		/// </summary>
		public virtual eContentVerticalAlignment ContentVerticalAlignment
		{
			get {return m_ContentVerticalAlignment;}
			set {m_ContentVerticalAlignment=value;}
		}

        /// <summary>
        /// Gets or sets the block line vertical alignment. Default value is Middle.
        /// </summary>
        public virtual eContentVerticalAlignment BlockLineAlignment
        {
            get { return m_BlockLineAlignment; }
            set { m_BlockLineAlignment = value; }
        }

		/// <summary>
		/// Gets or sets the content horiznontal alignment. Default value is Left.
		/// </summary>
		public virtual eContentAlignment ContentAlignment
		{
			get {return m_ContentAlignment;}
			set {m_ContentAlignment=value;}
		}

		/// <summary>
		/// Gets or sets whether all content blocks are resized so they have same height which is height of the talles content block. Default value is false.
		/// </summary>
		public virtual bool EvenHeight
		{
			get {return m_EvenHeight;}
			set {m_EvenHeight=value;}
		}

		/// <summary>
		/// Gets or sets whether content is wrapped into new line if it exceeds the width of the container.
		/// </summary>
		public bool MultiLine
		{
			get {return m_MultiLine;}
			set {m_MultiLine=value;}
		}

        /// <summary>
        /// Gets or sets whether layout is right-to-left.
        /// </summary>
        public bool RightToLeft
        {
            get { return m_RightToLeft; }
            set { m_RightToLeft = value; }
        }

		#endregion
	}

    /// <summary>
    /// Represents event arguments for SerialContentLayoutManager.NextPosition event.
    /// </summary>
    public class LayoutManagerPositionEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the block that is layed out.
        /// </summary>
        public IBlock Block = null;
        /// <summary>
        /// Gets or sets the current block position.
        /// </summary>
        public Point CurrentPosition = Point.Empty;
        /// <summary>
        /// Gets or sets the calculated next block position.
        /// </summary>
        public Point NextPosition = Point.Empty;
        /// <summary>
        /// Cancels default position calculation.
        /// </summary>
        public bool Cancel = false;
    }

    /// <summary>
    /// Represents event arguments for the SerialContentLayoutManager layout events.
    /// </summary>
    public class LayoutManagerLayoutEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the reference block object.
        /// </summary>
        public IBlock Block = null;

        /// <summary>
        /// Gets or sets the position block will assume.
        /// </summary>
        public Point CurrentPosition = Point.Empty;

        /// <summary>
        /// Cancel the layout of the block, applies only to BeforeXXX layout event.
        /// </summary>
        public bool CancelLayout = false;

        /// <summary>
        /// Gets or sets the visibility index of the block.
        /// </summary>
        public int BlockVisibleIndex = 0;

        /// <summary>
        /// Creates new instance of the class and initializes it with default values.
        /// </summary>
        public LayoutManagerLayoutEventArgs(IBlock block, Point currentPosition, int visibleIndex)
        {
            this.Block = block;
            this.CurrentPosition = currentPosition;
            this.BlockVisibleIndex = visibleIndex;
        }
    }

    /// <summary>
    /// Delegate for SerialContentLayoutManager.NextPosition event.
    /// </summary>
    public delegate void LayoutManagerPositionEventHandler(object sender, LayoutManagerPositionEventArgs e);

    /// <summary>
    /// Delegate for the SerialContentLayoutManager layout events.
    /// </summary>
    public delegate void LayoutManagerLayoutEventHandler(object sender, LayoutManagerLayoutEventArgs e);
    

}
