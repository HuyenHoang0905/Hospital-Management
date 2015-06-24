namespace DevComponents.DotNetBar
{
    using System;
	using System.Drawing;
	using System.Collections;
    using System.Windows.Forms;
	
	/// <summary>
    ///  Defines the generic container item that is used by toolbar, menu bar and other control for item layout.
    /// </summary>
    [System.ComponentModel.ToolboxItem(false),System.ComponentModel.DesignTimeVisible(false)]
    public class GenericItemContainer:ImageItem, IDesignTimeProvider
    {
        #region Private Variables
        protected int m_PaddingLeft, m_PaddingTop, m_PaddingBottom, m_PaddingRight, m_ItemSpacing;
        private int m_FirstItemSpacing = 0;
		protected eBorderType m_BorderType;
		protected internal Color m_BackgroundColor;
		protected bool m_WrapItems;
		protected DisplayMoreItem m_MoreItems;
		private bool m_MoreItemsOnMenu;
		private bool m_EqualButtonSize;
		private int m_DisplayedItems=0;

		//private bool m_Stretch;
		private bool m_SystemContainer;
		private bool m_HaveCustomizeItem=false;

		private eLayoutType m_LayoutType=eLayoutType.Toolbar;

		private bool m_Scroll=false;  // Indicates whether we use scrolling in container. Applies only to the "Task List" container.
		private int m_ScrollTopPosition=0; // Indicates the first visible item from the top when scroll is used.
		private bool m_TopScroll=false, m_BottomScroll=false;
		private ScrollButton m_TopScrollButton=null,m_BottomScrollButton=null;

		// Used to eat the mouse move messages when user is using the keyboard to browse through the
		// top level menus. The problem was that on each repaint the mouse move was fired even though the
		// mouse did not move at all. So if mouse was over an menu item it was not possible to switch to the
		// new menu item becouse mouse was "holding" the focus.
		private bool m_IgnoreDuplicateMouseMove=false;
		private System.Windows.Forms.MouseEventArgs m_LastMouseMoveEvent=null;
		private int m_MinWidth=0, m_MinHeight=0;
		private bool m_Painting=false;
		private bool m_EventHeight=true;
		private bool m_UseMoreItemsButton=true;
        private eContainerVerticalAlignment m_ToolbarItemsAlign = eContainerVerticalAlignment.Bottom;
        private bool m_TrackSubItemsImageSize = true;
        #endregion

        #region Events
        /// <summary>
        /// Occurs when new item is added to the container.
        /// </summary>
        internal EventHandler ItemAdded;
        #endregion

        public GenericItemContainer()
        {
			// We contain other controls
			m_IsContainer=true;
			m_PaddingLeft=1;
			m_PaddingTop=1;
			m_PaddingBottom=1;
			m_PaddingRight=1;
			m_ItemSpacing=0;
			m_BackgroundColor=Color.Empty;
			m_BorderType=eBorderType.None;
			m_WrapItems=false;
			m_MoreItems=null;
			m_EqualButtonSize=false;
			//m_Stretch=false;
			m_MoreItemsOnMenu=false;
			m_SystemContainer=false;
			this.AccessibleRole=System.Windows.Forms.AccessibleRole.Grouping;
        }

        protected override void Dispose(bool disposing)
		{
			if(m_TopScrollButton!=null)
			{
				if(m_TopScrollButton.Parent!=null)
					m_TopScrollButton.Parent.Controls.Remove(m_TopScrollButton);
				m_TopScrollButton.Dispose();
				m_TopScrollButton=null;
			}

			if(m_BottomScrollButton!=null)
			{
				if(m_BottomScrollButton.Parent!=null)
					m_BottomScrollButton.Parent.Controls.Remove(m_BottomScrollButton);
				m_BottomScrollButton.Dispose();
				m_BottomScrollButton=null;
			}
			
			base.Dispose(disposing);
		}

		internal int MinHeight
		{
			get {return m_MinHeight;}
			set {m_MinHeight=value;}
		}
		internal int MinWidth
		{
			get {return m_MinWidth;}
			set {m_MinWidth=value;}
		}

		/// <summary>
		/// Returns copy of GenericItemContainer item
		/// </summary>
		public override BaseItem Copy()
		{
			GenericItemContainer objCopy=new GenericItemContainer();
			this.CopyToItem(objCopy);
			
			return objCopy;
		}
		protected override void CopyToItem(BaseItem copy)
		{
			GenericItemContainer objCopy=copy as GenericItemContainer;
			base.CopyToItem(objCopy);

			objCopy.WrapItems=m_WrapItems;
			objCopy.EqualButtonSize=m_EqualButtonSize;
		}

		/// <summary>
		/// Paints this base container
		/// </summary>
        public override void Paint(ItemPaintArgs pa)
        {
            if (this.SuspendLayout || m_Painting)
                return;
            eDotNetBarStyle effectiveStyle = EffectiveStyle;
            m_Painting = true;
            try
            {
                System.Drawing.Graphics g = pa.Graphics;

                if (m_NeedRecalcSize)
                {
                    RecalcSize();
                }
                int x = m_Rect.Left, y = m_Rect.Top;
                Rectangle thisRect = new Rectangle(m_Rect.Left, m_Rect.Top, m_Rect.Width, m_Rect.Height);

                switch (m_BorderType)
                {
                    case eBorderType.Bump:
                        System.Windows.Forms.ControlPaint.DrawBorder3D(g, m_Rect, System.Windows.Forms.Border3DStyle.Bump, System.Windows.Forms.Border3DSide.All);
                        thisRect.Inflate(-2, -2);
                        break;
                    case eBorderType.Etched:
                        System.Windows.Forms.ControlPaint.DrawBorder3D(g, m_Rect, System.Windows.Forms.Border3DStyle.Etched, System.Windows.Forms.Border3DSide.All);
                        thisRect.Inflate(-2, -2);
                        break;
                    case eBorderType.Raised:
                        System.Windows.Forms.ControlPaint.DrawBorder3D(g, m_Rect, System.Windows.Forms.Border3DStyle.RaisedInner, System.Windows.Forms.Border3DSide.All);
                        thisRect.Inflate(-2, -2);
                        break;
                    case eBorderType.Sunken:
                        System.Windows.Forms.ControlPaint.DrawBorder3D(g, m_Rect, System.Windows.Forms.Border3DStyle.SunkenOuter, System.Windows.Forms.Border3DSide.All);
                        thisRect.Inflate(-2, -2);
                        break;
                    case eBorderType.SingleLine:
                        // TODO: Beta 2 fix --> g.DrawRectangle(new Pen(SystemBrushes.ControlDark,1),m_Rect);
                        Pen pen = new Pen(SystemBrushes.ControlDark, 1);
                        NativeFunctions.DrawRectangle(g, pen, m_Rect);
                        pen.Dispose();
                        thisRect.Inflate(-1, -1);
                        break;
                    default:
                        break;
                }

                if (!m_BackgroundColor.IsEmpty)
                {
                    SolidBrush brush = new SolidBrush(m_BackgroundColor);
                    g.FillRectangle(brush, thisRect);
                    brush.Dispose();
                }
                //else if(m_Style==eDotNetBarStyle.Office2000 || this.IsOnMenuBar)
                //	g.FillRectangle(SystemBrushes.Control,thisRect);
                //else
                //	g.FillRectangle(new SolidBrush(ColorFunctions.ToolMenuFocusBackColor(g)),thisRect);
                g.SetClip(thisRect);
                if (m_SubItems != null)
                {
                    BaseItem objItem = null;
                    int iDisplayed = 0;
                    ThemeToolbar theme = null;
                    ThemeToolbarParts part = ThemeToolbarParts.Separator;
                    ThemeToolbarStates state = ThemeToolbarStates.Normal;
                    if (this.IsThemed)
                        theme = pa.ThemeToolbar;

                    for (int iCurrentItem = 0; iCurrentItem < m_SubItems.Count; iCurrentItem++)
                    {
                        objItem = m_SubItems[iCurrentItem] as BaseItem;
                        if (objItem.Visible && objItem.Displayed)
                        {
                            if (objItem.BeginGroup && iDisplayed > 0 && m_LayoutType != eLayoutType.DockContainer && !(objItem is ItemContainer && BarFunctions.IsOffice2007Style(objItem.EffectiveStyle)))
                            {
                                if (m_Orientation == eOrientation.Vertical || m_LayoutType == eLayoutType.TaskList)
                                {
                                    if (objItem.TopInternal == m_PaddingTop + m_Rect.Top || objItem.LeftInternal > m_PaddingLeft + m_Rect.Left)
                                    {
                                        // Vertical line on the left side
                                        if (effectiveStyle == eDotNetBarStyle.Office2000)
                                            System.Windows.Forms.ControlPaint.DrawBorder3D(g, objItem.LeftInternal - 4, objItem.TopInternal + 1, 2, m_Rect.Height - m_PaddingTop - m_PaddingBottom - 4, System.Windows.Forms.Border3DStyle.Etched, System.Windows.Forms.Border3DSide.Left);
                                        else
                                        {
                                            if (theme != null)
                                            {
                                                part = ThemeToolbarParts.Separator;
                                                theme.DrawBackground(g, part, state, new Rectangle(objItem.LeftInternal - 4, objItem.TopInternal + 1, 6, m_Rect.Height - m_PaddingBottom - 2));
                                            }
                                            else
                                            {
                                                using (Pen pen = new Pen(pa.Colors.ItemSeparator))
                                                {
                                                    if (effectiveStyle == eDotNetBarStyle.OfficeXP)
                                                        g.DrawLine(pen, objItem.LeftInternal - 2, objItem.TopInternal + 1, objItem.LeftInternal - 2, m_Rect.Bottom - m_PaddingBottom - 2);
                                                    else
                                                    {
                                                        g.DrawLine(pen, objItem.LeftInternal - 2, objItem.TopInternal + 2, objItem.LeftInternal - 2, m_Rect.Bottom - m_PaddingBottom - 4);
                                                        Pen penLight = new Pen(pa.Colors.ItemSeparatorShade, 1);
                                                        g.DrawLine(penLight, objItem.LeftInternal - 1, objItem.TopInternal + 3, objItem.LeftInternal - 1, m_Rect.Bottom - m_PaddingBottom - 5);
                                                        penLight.Dispose();
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        Point start = new Point(objItem.LeftInternal + m_PaddingLeft, objItem.TopInternal - 2);
                                        Point end = new Point(objItem.DisplayRectangle.Right - m_PaddingRight, objItem.TopInternal - 2);
                                        if (this.LayoutType == eLayoutType.TaskList)
                                        {
                                            start.X = this.LeftInternal + 1;
                                            end.X = this.DisplayRectangle.Right - 2;
                                        }

                                        // Horizontal line
                                        if (effectiveStyle == eDotNetBarStyle.Office2000)
                                            System.Windows.Forms.ControlPaint.DrawBorder3D(g, m_Rect.Left + m_PaddingLeft + 2, objItem.TopInternal - 4, objItem.WidthInternal - 2, 2, System.Windows.Forms.Border3DStyle.Etched, System.Windows.Forms.Border3DSide.Top);
                                        else
                                        {
                                            if (theme != null)
                                            {
                                                part = ThemeToolbarParts.SeparatorVert;
                                                theme.DrawBackground(g, part, state, new Rectangle(start.X, start.Y, end.X - start.X, 6));
                                            }
                                            else
                                            {
                                                using (Pen pen = new Pen(pa.Colors.ItemSeparator))
                                                {
                                                    if (effectiveStyle == eDotNetBarStyle.OfficeXP)
                                                        g.DrawLine(pen, start, end);
                                                    else
                                                    {
                                                        g.DrawLine(pen, start, end);
                                                        start.Offset(0, 1);
                                                        end.Offset(0, 1);
                                                        using (Pen penLight = new Pen(pa.Colors.ItemSeparatorShade, 1))
                                                            g.DrawLine(penLight, start, end);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (objItem.TopInternal == m_PaddingTop + m_Rect.Top || objItem.LeftInternal > m_PaddingLeft + m_Rect.Left)
                                    {
                                        // Vertical line on the left side
                                        if (effectiveStyle == eDotNetBarStyle.Office2000)
                                            System.Windows.Forms.ControlPaint.DrawBorder3D(g, objItem.LeftInternal - 4, objItem.TopInternal + 1, 2, objItem.HeightInternal - 2, System.Windows.Forms.Border3DStyle.Etched, System.Windows.Forms.Border3DSide.Left);
                                        else
                                        {
                                            if (theme != null)
                                            {
                                                part = ThemeToolbarParts.Separator;
                                                theme.DrawBackground(g, part, state, new Rectangle(objItem.LeftInternal - 4, objItem.TopInternal + 1, 6, objItem.HeightInternal - 3));
                                            }
                                            else
                                            {
                                                using (Pen pen = new Pen(pa.Colors.ItemSeparator))
                                                {
                                                    if (effectiveStyle == eDotNetBarStyle.OfficeXP)
                                                        g.DrawLine(pen, objItem.LeftInternal - 2, objItem.TopInternal + 1, objItem.LeftInternal - 2, objItem.TopInternal + 1 + objItem.HeightInternal - 3);
                                                    else
                                                    {
                                                        g.DrawLine(pen, objItem.LeftInternal - 2, objItem.TopInternal + 4, objItem.LeftInternal - 2, objItem.TopInternal + 4 + objItem.HeightInternal - 8);
                                                        Pen penLight = new Pen(pa.Colors.ItemSeparatorShade, 1);
                                                        g.DrawLine(penLight, objItem.LeftInternal - 1, objItem.TopInternal + 5, objItem.LeftInternal - 1, objItem.TopInternal + 5 + objItem.HeightInternal - 8);
                                                        penLight.Dispose();
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        // Horizontal line
                                        if (effectiveStyle == eDotNetBarStyle.Office2000)
                                            System.Windows.Forms.ControlPaint.DrawBorder3D(g, m_Rect.Left + m_PaddingLeft + 2, objItem.TopInternal - 4, m_Rect.Width - m_PaddingLeft - m_PaddingRight - 4, 2, System.Windows.Forms.Border3DStyle.Etched, System.Windows.Forms.Border3DSide.Top);
                                        else
                                        {
                                            if (theme != null)
                                            {
                                                part = ThemeToolbarParts.SeparatorVert;
                                                theme.DrawBackground(g, part, state, new Rectangle(m_Rect.Left + m_PaddingLeft + 2, objItem.TopInternal - 4, m_Rect.Width - m_PaddingRight - 2, 6));
                                            }
                                            else
                                            {
                                                using (Pen pen = new Pen(pa.Colors.ItemSeparator))
                                                {
                                                    if (effectiveStyle == eDotNetBarStyle.OfficeXP)
                                                        g.DrawLine(pen, m_Rect.Left + m_PaddingLeft + 2, objItem.TopInternal - 2, m_Rect.Right - m_PaddingRight - 2, objItem.TopInternal - 2);
                                                    else
                                                    {
                                                        g.DrawLine(pen, m_Rect.Left + m_PaddingLeft + 2, objItem.TopInternal - 2, m_Rect.Right - m_PaddingRight - 2, objItem.TopInternal - 2);
                                                        Pen penLight = new Pen(pa.Colors.ItemSeparatorShade, 1);
                                                        g.DrawLine(penLight, m_Rect.Left + m_PaddingLeft + 6, objItem.TopInternal - 1, m_Rect.Right - m_PaddingRight - 4, objItem.TopInternal - 1);
                                                        penLight.Dispose();
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            if (g.ClipBounds.IntersectsWith(objItem.DisplayRectangle))
                                objItem.Paint(pa);
                            iDisplayed++;
                        }

                        // Paint the Display More Items button if needed
                        if (m_MoreItems != null)
                        {
                            //Point loc = GetMoreItemsLocation();
                            //m_MoreItems.LeftInternal = loc.X;
                            //m_MoreItems.TopInternal = loc.Y;

                            //if(m_Orientation==eOrientation.Vertical)
                            //{
                            //    m_MoreItems.LeftInternal=m_Rect.Left+m_PaddingLeft;
                            //    m_MoreItems.TopInternal=m_Rect.Bottom-m_MoreItems.HeightInternal-m_PaddingBottom;
                            //}
                            //else
                            //{
                            //    m_MoreItems.LeftInternal=m_Rect.Right-m_MoreItems.WidthInternal-m_PaddingRight;
                            //    m_MoreItems.TopInternal=m_Rect.Top+m_PaddingTop;
                            //}
                            m_MoreItems.Paint(pa);
                        }

                        //					if(m_LayoutType==eLayoutType.TaskList && m_Scroll)
                        //					{
                        //
                        //					}
                    }
                }
                g.ResetClip();
            }
            finally
            {
                m_Painting = false;
            }
        }

		public eBorderType BorderType
		{
			get
			{
				return m_BorderType;
			}
			set
			{
				m_BorderType=value;
			}
		}
		
		/// <summary>
		/// Set/Get does container wraps item into the new line when they exceed the container size
		/// </summary>
		public bool WrapItems
		{
			get
			{
				return m_WrapItems;
			}
			set
			{
				m_WrapItems=value;
			}
		}

		public int ItemSpacing
		{
			get
			{
				return m_ItemSpacing;
			}
			set
			{
				if(m_ItemSpacing==value)
					return;
				m_ItemSpacing=value;
				NeedRecalcSize=true;
			}
		}

        protected virtual int FirstItemSpacing
        {
            get { return m_FirstItemSpacing; }
            set { m_FirstItemSpacing = value; }
        }

		/// <summary>
		/// Specifies whether to display more items on popup menu or Bar.
		/// </summary>
		public bool MoreItemsOnMenu
		{
			get
			{
				return m_MoreItemsOnMenu;
			}
			set
			{
				m_MoreItemsOnMenu=value;
			}
		}

		internal DisplayMoreItem MoreItems
		{
			get
			{
				return m_MoreItems;
			}
		}

		public override void RecalcSize()
		{
			if(this.SuspendLayout)
				return;

			m_RecalculatingSize=true;

			switch(m_LayoutType)
			{
				case eLayoutType.Toolbar:
                    RecalcSizeToolbar();
					break;
				case eLayoutType.TaskList:
					RecalcSizeTaskList();
					break;
				case eLayoutType.DockContainer:
					RecalcSizeDockContainer(false);
					break;
			}
			base.RecalcSize();
			m_RecalculatingSize=false;
		}

		private void RecalcSizeDockContainer(bool bFitContainer)
		{
			int iX=0, iY=0, iMaxWidth=0, iMaxHeight=0;
			bool bOneDisplayed=false;
			if(this.SuspendLayout)
				return;

			m_DisplayedItems=0;

			iX=m_PaddingLeft+m_Rect.Left;
			iY=m_PaddingTop+m_Rect.Top;
			if(m_SubItems!=null)
			{
				for(int itemIndex=0;itemIndex<this.SubItems.Count;itemIndex++)
				{
					BaseItem item=this.SubItems[itemIndex];
					item.RecalcSize();
                    if(item.WidthInternal>iMaxWidth)
						iMaxWidth=item.WidthInternal;
					if(item.HeightInternal>iMaxHeight)
						iMaxHeight=item.HeightInternal;
					
					item.LeftInternal=iX;
					item.TopInternal=iY;

					if(!item.Visible)
					{
						item.Displayed=false;
						continue;
					}
					else
						m_DisplayedItems++;
					if(!bOneDisplayed && (item.Displayed || itemIndex==this.SubItems.Count-1))
					{
						item.Displayed=true;
						bOneDisplayed=true;
					}
					else
					{
						item.Displayed=false;
					}
				}

				if(!bOneDisplayed)
				{
					foreach(BaseItem item in m_SubItems)
						if(item.Visible)
						{
							item.Displayed=true;
							break;
						}
				}
			}

			if((bFitContainer || this.Stretch))
			{
				if(iMaxWidth+m_PaddingLeft+m_PaddingRight<m_Rect.Width || iMaxHeight+m_PaddingTop+m_PaddingBottom<m_Rect.Height)
				{
					if(iMaxWidth+m_PaddingLeft+m_PaddingRight<m_Rect.Width)
						iMaxWidth=m_Rect.Width-m_PaddingLeft-m_PaddingRight;
					if(iMaxHeight+m_PaddingTop+m_PaddingBottom<m_Rect.Height)
						iMaxHeight=m_Rect.Height-m_PaddingTop-m_PaddingBottom;
				}
				Bar bar=this.ContainerControl as Bar;
				// Following code lets us resized Dockable Bars when docked...
				if(bar!=null && (bar.BarState==eBarState.Docked || bar.BarState==eBarState.AutoHide))
				{
					if(MinWidth>0 && iMaxWidth+m_PaddingLeft+m_PaddingRight<MinWidth && this.Orientation==eOrientation.Vertical)
						iMaxWidth=MinWidth-(m_PaddingLeft+m_PaddingRight);
					else if(MinHeight>0 && iMaxHeight+m_PaddingTop+m_PaddingBottom<MinHeight && this.Orientation==eOrientation.Horizontal)
						iMaxHeight=MinHeight-(m_PaddingTop+m_PaddingBottom);
				}
			}

            Size newSize = new Size(iMaxWidth, iMaxHeight);
			foreach(BaseItem item in m_SubItems)
                item.Size = newSize;

			if(this.Stretch)
			{
				if(m_Orientation==eOrientation.Horizontal)
				{
					m_Rect.Height=m_PaddingTop+iMaxHeight+m_PaddingBottom;
					// Only if suggested Width is less than our width change it
					if(m_Rect.Width<iMaxWidth+m_PaddingLeft+m_PaddingRight)
						m_Rect.Width=m_PaddingLeft+iMaxWidth+m_PaddingRight;
				}
				else if(m_Orientation==eOrientation.Vertical)
				{
					m_Rect.Width=m_PaddingLeft+iMaxWidth+m_PaddingRight;
					// Only if suggested Height is less than our height change it
					if(m_Rect.Height<iMaxHeight+m_PaddingBottom+m_PaddingTop)
						m_Rect.Height=m_PaddingTop+iMaxHeight+m_PaddingBottom;
				}
			}
			else
			{
				m_Rect.Width=m_PaddingLeft+iMaxWidth+m_PaddingRight;
				m_Rect.Height=m_PaddingTop+iMaxHeight+m_PaddingBottom;
			}
		}

		/// <summary>
		/// Recalculate Size of this item
		/// </summary>
		private void RecalcSizeToolbar()
		{
			if(this.SuspendLayout)
				return;

            bool isRightToLeft = false;
            Control containerControl = this.ContainerControl as Control;
            if (containerControl != null)
                isRightToLeft = (containerControl.RightToLeft == RightToLeft.Yes);

			// This object for now have only border but it will have more potentially
			int iX=0, iY=0, iMaxItem=0, iMaxItemWidth=0, iMaxItemHeight=0;
			int iWidth=0, iHeight=0;
			int iGroupLineSize=3;
			int iLines=0;
			bool bDisplayed=true;
			bool bAdjustSize=false, bSetSize=false;  // If horizontal orientation we need to make sure that Height of all items is same
			int iDisplayed=0;
			BaseItem objItem=null;
			ArrayList FarAlignLines=null;
			Stack FarAlignItems=null;
			ArrayList StretchLines=null;
			ArrayList StretchLinesISCount=null;  // Holds the count of number of items stretched per line
			ArrayList ItemsToStretch=null;

			if(m_MoreItems!=null && m_MoreItems.Expanded)
				m_MoreItems.Expanded=false;

            if (EffectiveStyle == eDotNetBarStyle.Office2000)
				iGroupLineSize=6;

			iX=m_PaddingLeft+m_Rect.Left;
			iY=m_PaddingTop+m_Rect.Top;

			int iVisibleCount=this.VisibleSubItems;
			int iVisible=0;

			if(m_SubItems!=null)
			{
				bool bRepeat=false;
				int iRepeatCount=0;
				
				if(m_WrapItems || this.Stretch)
					FarAlignLines=new ArrayList(3);

				if(this.Stretch)
				{
					StretchLines=new ArrayList(3);
					StretchLinesISCount=new ArrayList(3);
				}

                bool callRecalcSize = OnBeforeLayout();

				do
				{
					iDisplayed=0;
					for (int iCurrentItem=0;iCurrentItem<m_SubItems.Count;iCurrentItem++)
					{
						objItem=m_SubItems[iCurrentItem] as BaseItem;
						if(objItem!=null && objItem.Visible)
						{
							iVisible++;
							if((objItem.SupportedOrientation==eSupportedOrientation.Horizontal && m_Orientation==eOrientation.Vertical) || (objItem.SupportedOrientation==eSupportedOrientation.Vertical && m_Orientation==eOrientation.Horizontal))
							{
								objItem.Displayed=false;
								continue;
							}

                            int itemSpacing = (iDisplayed > 1 || m_FirstItemSpacing == 0) ? m_ItemSpacing : m_FirstItemSpacing;

                            if(callRecalcSize)
							    objItem.RecalcSize();
							if(bSetSize)
							{
								if(m_Orientation==eOrientation.Vertical)
									objItem.WidthInternal=iMaxItemWidth;
								else
									objItem.HeightInternal=iMaxItemHeight;
							}

							switch(m_Orientation)
							{
								case eOrientation.Horizontal:
								{
									// Wrap to next line if needed
									if(m_WrapItems)
									{
										if(objItem.BeginGroup && iDisplayed>0)
											iX+=iGroupLineSize;
                                        if (iX + objItem.WidthInternal + itemSpacing > m_Rect.Right && iX > m_PaddingLeft + m_Rect.Left)
										{
                                            iY += (iMaxItem + itemSpacing);
											if(objItem.BeginGroup)
												iY+=iGroupLineSize;
											iX=m_PaddingLeft+m_Rect.Left;
											iMaxItem=0;
											iLines++;

											if(FarAlignItems!=null)
												FarAlignItems=null;
											if(ItemsToStretch!=null)
												ItemsToStretch=null;
										}
									}
									else if(bDisplayed)
									{
										if(objItem.BeginGroup && iDisplayed>0)
											iX+=iGroupLineSize;
										// If not wrapping then make sure that items fits into one line, otherwise we need to display the mark that lets them see the rest
                                        if (iX + objItem.WidthInternal + itemSpacing > m_Rect.Right - m_PaddingRight && iX > m_PaddingLeft + m_Rect.Left || (iX + objItem.WidthInternal + itemSpacing + DisplayMoreItem.FixedSize > m_Rect.Right && iCurrentItem + 1 < m_SubItems.Count && iVisible < iVisibleCount))
										{
											if(iCurrentItem==0)
												iMaxItem=objItem.HeightInternal;
											// Items don't fit any more the rest won't be visible
                                            iX += (itemSpacing + DisplayMoreItem.FixedSize);
					                        bDisplayed=false;
										}
									}
									
									if(bDisplayed)
									{
										if(iDisplayed>0)
                                            iX += itemSpacing;
                                        objItem.LeftInternal = GetItemLayoutX(objItem, iX);
                                        objItem.TopInternal = GetItemLayoutY(objItem, iY);
										//iX+=objItem.WidthInternal+m_ItemSpacing;
										iX+=GetItemLayoutWidth(objItem);
										if(objItem.HeightInternal>iMaxItem)
										{
											iMaxItem=objItem.HeightInternal;
										}
									}
									break;
								}
								case eOrientation.Vertical:
								{
									// Wrap to next line if needed
									if(m_WrapItems)
									{
										if(objItem.BeginGroup && iDisplayed>0)
											iY+=iGroupLineSize;
                                        if (iY + objItem.HeightInternal + itemSpacing > m_Rect.Bottom && iY > m_PaddingTop + m_Rect.Top)
										{
                                            iX += (iMaxItem + itemSpacing);
											if(objItem.BeginGroup)
												iX+=iGroupLineSize;
											iY=m_PaddingTop+m_Rect.Top;
											iMaxItem=0;
											iLines++;

											if(FarAlignItems!=null)
												FarAlignItems=null;
											if(ItemsToStretch!=null)
												ItemsToStretch=null;
										}
									}
									else if(bDisplayed)
									{
										if(objItem.BeginGroup && iDisplayed>0)
											iY+=iGroupLineSize;
										// If not wrapping then make sure that items fits into one line, otherwise we need to display the mark that lets them see the rest
                                        if (iY + objItem.HeightInternal + itemSpacing > m_Rect.Bottom && iY > m_PaddingTop + m_Rect.Top || (iY + objItem.HeightInternal + itemSpacing + DisplayMoreItem.FixedSize > m_Rect.Bottom && iCurrentItem + 1 < m_SubItems.Count && iVisible < iVisibleCount))
										{
											if(iCurrentItem==0)
												iMaxItem=objItem.WidthInternal;
											// Items don't fit any more the rest won't be visible
                                            iY += (itemSpacing + DisplayMoreItem.FixedSize);
					                        bDisplayed=false;
										}
									}
									if(bDisplayed)
									{
										if(iDisplayed>0)
                                            iY += itemSpacing;
										objItem.LeftInternal=iX;
										objItem.TopInternal=iY;
										iY+=GetItemLayoutHeight(objItem);
										if(objItem.WidthInternal>iMaxItem)
										{
											iMaxItem=objItem.WidthInternal;
										}
									}
									break;
								}
								default:
									break;
							}

							if(bDisplayed)
							{
								if(objItem.WidthInternal!=iMaxItemWidth)
								{
									if(m_Orientation==eOrientation.Vertical && iMaxItemWidth!=0 && !bSetSize)
										bAdjustSize=true;
									if(objItem.WidthInternal>iMaxItemWidth)
										iMaxItemWidth=objItem.WidthInternal;
								}
								if(objItem.HeightInternal!=iMaxItemHeight)
								{
									if(m_Orientation==eOrientation.Horizontal && iMaxItemHeight!=0 && !bSetSize)
										bAdjustSize=true;
									if(objItem.HeightInternal>iMaxItemHeight)
										iMaxItemHeight=objItem.HeightInternal;
								}
								iDisplayed++;
							}

							objItem.Displayed=bDisplayed;

							// Save right (Far) aligned items so we can adjust thiere position at the end
							if(bDisplayed && FarAlignLines!=null && objItem.ItemAlignment==eItemAlignment.Far || FarAlignItems!=null)
							{
								if(FarAlignItems==null)
								{
									FarAlignItems=new Stack();
									FarAlignLines.Add(FarAlignItems);
								}
								FarAlignItems.Push(objItem);
								// no stretch after item has been far aligned
								StretchLines=null;
							}

							// Save Items that we need to stretch
							if(bDisplayed && objItem.Stretch && StretchLines!=null || ItemsToStretch!=null)
							{
								if(ItemsToStretch==null)
								{
									ItemsToStretch=new ArrayList(5);
									StretchLines.Add(ItemsToStretch);
									StretchLinesISCount.Add(0);
								}
								ItemsToStretch.Add(objItem);
								if(objItem.Stretch)
									StretchLinesISCount[StretchLinesISCount.Count-1]=(int)StretchLinesISCount[StretchLinesISCount.Count-1]+1;
							}


							// Track the size if this container
							if(iX>iWidth)
								iWidth=iX;
							if(iY>iHeight)
								iHeight=iY;
						}
						else if(objItem!=null)
							objItem.Displayed=false;
					}
					// Check do we have to repeat the process
					// This is needed if container is in wrap mode and some items do not fit
					// This is done to ensure the minimum size of the container for example
					// if container with is smaller than width of one single item inside the
					// container width has to be set to widest item in the list and then the
					// size has to be recalculated again.
					if(m_WrapItems && iLines>0 && iRepeatCount<1)
					{
						if(m_Orientation==eOrientation.Horizontal && (iWidth>m_Rect.Width || bAdjustSize))
						{
							bRepeat=true;
							m_Rect.Width=iWidth+m_PaddingLeft+m_PaddingRight;
							iX=m_PaddingLeft+m_Rect.Left;
							iY=m_PaddingTop+m_Rect.Top;
							iWidth=0;
							iHeight=0;
						}
						else if(m_Orientation==eOrientation.Vertical && (iHeight>m_Rect.Height || bAdjustSize))
						{
							bRepeat=true;
							m_Rect.Height=iHeight+m_PaddingTop+m_PaddingBottom;
							iX=m_PaddingLeft+m_Rect.Left;
							iY=m_PaddingTop+m_Rect.Top;
							iWidth=0;
							iHeight=0;
						}
						if(bAdjustSize)
						{
							if(m_EventHeight)
								bSetSize=true;
							bAdjustSize=false;
						}
						// Reset alignment collections since new iteration can cause different lines
						if(bRepeat && iRepeatCount+1<2)
						{
							if(FarAlignLines!=null && FarAlignLines.Count>0)
								FarAlignLines=new ArrayList(3);
							// Reset stretch collection for same reason
							if(StretchLines!=null && StretchLines.Count>0)
							{
								StretchLines=new ArrayList(3);
								StretchLinesISCount=new ArrayList(3);
							}
						}
					}
					iRepeatCount++;
				} while (bRepeat && iRepeatCount<2);
			}

			if(m_EqualButtonSize)
			{
				// Make all buttons same width, do the same thing as loop above
				iX=m_PaddingLeft+m_Rect.Left;
				iY=m_PaddingTop+m_Rect.Top;
				iWidth=0;
				iHeight=0;
				bDisplayed=true;
				iLines=0;
				bAdjustSize=false;  // Size will be adjusted reset
				iDisplayed=0;

				// Reset alignment collections since new iteration can cause different lines
				if(FarAlignLines!=null && FarAlignLines.Count>0)
					FarAlignLines=new ArrayList(3);
				// Reset stretch collection for same reason
				if(StretchLines!=null && StretchLines.Count>0)
				{
					StretchLines=new ArrayList(3);
					StretchLinesISCount=new ArrayList(3);
				}

				for (int iCurrentItem=0;iCurrentItem<m_SubItems.Count;iCurrentItem++)
				{
					objItem=m_SubItems[iCurrentItem] as BaseItem;
					if(objItem!=null && objItem.Visible)
					{
						if((objItem.SupportedOrientation==eSupportedOrientation.Horizontal && m_Orientation==eOrientation.Vertical) || (objItem.SupportedOrientation==eSupportedOrientation.Vertical && m_Orientation==eOrientation.Horizontal))
						{
							objItem.Displayed=false;
							continue;
						}
                        int itemSpacing = (iDisplayed > 1 || m_FirstItemSpacing == 0) ? m_ItemSpacing : m_FirstItemSpacing;
						if(objItem.SystemItem)
						{
							if(m_Orientation==eOrientation.Vertical)
								objItem.WidthInternal=iMaxItemWidth;
							else
								objItem.HeightInternal=iMaxItemHeight;
						}
						else
						{
							objItem.WidthInternal=iMaxItemWidth;
							objItem.HeightInternal=iMaxItemHeight;
						}

						switch(m_Orientation)
						{
							case eOrientation.Horizontal:
							{
								// Wrap to next line if needed
								if(m_WrapItems)
								{
									if(objItem.BeginGroup)
										iX+=iGroupLineSize;
                                    if (iX + objItem.WidthInternal + itemSpacing > m_Rect.Right && iX > m_PaddingLeft + m_PaddingRight)
									{
                                        iY += (iMaxItem + itemSpacing);
										if(objItem.BeginGroup)
											iY+=iGroupLineSize;
										iX=m_PaddingLeft+m_Rect.Left;
										iMaxItem=0;
										iLines++;

										if(FarAlignItems!=null)
											FarAlignItems=null;
										if(ItemsToStretch!=null)
											ItemsToStretch=null;
									}
								}
								else if(bDisplayed)
								{
									if(objItem.BeginGroup)
										iX+=iGroupLineSize;
									// If not wrapping then make sure that items fits into one line, otherwise we need to display the mark that lets them see the rest
                                    if (iX + objItem.WidthInternal + itemSpacing > m_Rect.Right && iX > m_PaddingLeft + m_Rect.Left || (iX + objItem.WidthInternal + itemSpacing + DisplayMoreItem.FixedSize > m_Rect.Right && iCurrentItem + 1 < m_SubItems.Count && iVisible < iVisibleCount))
									{
										// Items don't fit any more the rest won't be visible
                                        iX += (itemSpacing + DisplayMoreItem.FixedSize);
                                        bDisplayed=false;
									}
								}
								
								if(bDisplayed)
								{
									objItem.LeftInternal=iX;
									objItem.TopInternal=iY;
                                    iX += objItem.WidthInternal + itemSpacing;
									if(objItem.HeightInternal>iMaxItem)
									{
										iMaxItem=objItem.HeightInternal;
									}
								}
								break;
							}
							case eOrientation.Vertical:
							{
								// Wrap to next line if needed
								if(m_WrapItems)
								{
                                    if (iY + objItem.HeightInternal + itemSpacing > m_Rect.Bottom && iY > m_PaddingTop + m_PaddingBottom)
									{
                                        iX += (iMaxItem + itemSpacing);
										iY=m_PaddingTop+m_Rect.Top;
										iMaxItem=0;
										iLines++;

										if(FarAlignItems!=null)
											FarAlignItems=null;
										if(ItemsToStretch!=null)
											ItemsToStretch=null;
									}
								}
								else if(bDisplayed)
								{
									// If not wrapping then make sure that items fits into one line, otherwise we need to display the mark that lets them see the rest
                                    if (iY + objItem.HeightInternal + itemSpacing > m_Rect.Bottom && iY > m_PaddingTop + m_Rect.Top || (iY + objItem.HeightInternal + itemSpacing + DisplayMoreItem.FixedSize > m_Rect.Bottom && iCurrentItem + 1 < m_SubItems.Count && iVisible < iVisibleCount))
									{
										// Items don't fit any more the rest won't be visible
                                        iY += (itemSpacing + DisplayMoreItem.FixedSize);
                                        bDisplayed=false;
									}
								}
								
								if(bDisplayed)
								{
									objItem.LeftInternal=iX;
									objItem.TopInternal=iY;
                                    iY += objItem.HeightInternal + itemSpacing;
									if(objItem.WidthInternal>iMaxItem)
									{
										iMaxItem=objItem.WidthInternal;
									}
								}
								break;
							}
							default:
								break;
						}
						objItem.Displayed=bDisplayed;
						if(bDisplayed)
							iDisplayed++;

						// Save right (Far) aligned items so we can adjust thiere position at the end
						if(bDisplayed && FarAlignLines!=null && objItem.ItemAlignment==eItemAlignment.Far || FarAlignItems!=null)
						{
							if(FarAlignItems==null)
							{
								FarAlignItems=new Stack();
								FarAlignLines.Add(FarAlignItems);
							}
							FarAlignItems.Push(objItem);
							// no stretch after item has been far aligned
							StretchLines=null;
						}

						// Save Items that we need to stretch, but not after item is far aligned
						if(bDisplayed && objItem.Stretch && StretchLines!=null || ItemsToStretch!=null)
						{
							if(ItemsToStretch==null)
							{
								ItemsToStretch=new ArrayList(5);
								StretchLines.Add(ItemsToStretch);
								StretchLinesISCount.Add(0);
							}
							ItemsToStretch.Add(objItem);
							if(objItem.Stretch)
								StretchLinesISCount[StretchLinesISCount.Count-1]=(int)StretchLinesISCount[StretchLinesISCount.Count-1]+1;
						}

						// Track the size if this container
						if(iX>iWidth)
							iWidth=iX;
						if(iY>iHeight)
							iHeight=iY;
					}
				}
			}

			m_DisplayedItems=iDisplayed;

			if(bAdjustSize)
			{
				if(m_Orientation==eOrientation.Vertical)
				{
					foreach(BaseItem objTmp in this.SubItems)
					{
						if(m_EventHeight)
							objTmp.WidthInternal=iMaxItemWidth;
                        else if (m_ToolbarItemsAlign == eContainerVerticalAlignment.Bottom)
							objTmp.LeftInternal+=(iMaxItemWidth-objTmp.WidthInternal);
                        else if (m_ToolbarItemsAlign == eContainerVerticalAlignment.Middle)
                            objTmp.LeftInternal += (iMaxItemWidth - objTmp.WidthInternal) / 2;
					}
				}
				else
				{
					foreach(BaseItem objTmp in this.SubItems)
					{
                        if (m_EventHeight)
                            objTmp.HeightInternal = iMaxItemHeight;
                        else if (m_ToolbarItemsAlign == eContainerVerticalAlignment.Bottom)
                            objTmp.TopInternal += (iMaxItemHeight - objTmp.HeightInternal);
                        else if (m_ToolbarItemsAlign == eContainerVerticalAlignment.Middle && iMaxItemHeight != objTmp.HeightInternal)
                            objTmp.TopInternal += (int)Math.Ceiling((float)(iMaxItemHeight - objTmp.HeightInternal) / 2) + 1;
					}
				}
			}

			// Add max width or height depending on arrange type...
			switch(m_Orientation)
			{
				case eOrientation.Horizontal:
				{
					iHeight+=iMaxItem;
					break;
				}
				case eOrientation.Vertical:
				{
					iWidth+=iMaxItem;
					break;
				}
				default:
					break;
			}

			if(this.Stretch)
			{
				if(m_Orientation==eOrientation.Horizontal)
				{
					m_Rect.Height=iHeight+m_PaddingBottom-m_Rect.Top;

					// Only if suggested Width is less than our width change it
					// This will disable stretchable items support for wrapable containers, every line width
					// would have to be tracked so stretchable items can work
					if(StretchLines!=null && StretchLines.Count>0)
						StretchItems(StretchLines,StretchLinesISCount,iWidth+m_PaddingRight-m_Rect.Left);
					if(m_Rect.Width<iWidth+m_PaddingRight-m_Rect.Left)
						m_Rect.Width=iWidth+m_PaddingRight-m_Rect.Left;
					//else if(StretchLines!=null && StretchLines.Count>0)
					//	StretchItems(StretchLines,StretchLinesISCount,iWidth+m_PaddingRight-m_Rect.Left);
				}
				else if(m_Orientation==eOrientation.Vertical)
				{
					m_Rect.Width=iWidth+m_PaddingRight-m_Rect.Left;

					if(StretchLines!=null && StretchLines.Count>0)
						StretchItems(StretchLines,StretchLinesISCount,iHeight+m_PaddingBottom-m_Rect.Top);
					// Only if suggested Height is less than our height change it
					if(m_Rect.Height<iHeight+m_PaddingBottom-m_Rect.Top)
						m_Rect.Height=iHeight+m_PaddingBottom-m_Rect.Top;
					//else if(StretchLines!=null && StretchLines.Count>0)
					//	StretchItems(StretchLines,StretchLinesISCount,iHeight+m_PaddingBottom-m_Rect.Top);
				}
				else
				{
					m_Rect.Width=iWidth+m_PaddingRight-m_Rect.Left;
					m_Rect.Height=iHeight+m_PaddingBottom-m_Rect.Top;
				}
			}
			else
			{
				// If all items did not fit and we are not wrapping take suggested sizes
				if(!m_WrapItems && !bDisplayed)
				{
					// Horizontal, just adjust Height, we will honor width
					if(m_Orientation==eOrientation.Horizontal)
						m_Rect.Height=iHeight+m_PaddingBottom-m_Rect.Top;
					else if(m_Orientation==eOrientation.Vertical)
						m_Rect.Width=iWidth+m_PaddingRight-m_Rect.Left;
				}
				else
				{
					// If there are no items take suggested width by container control
					if(m_DisplayedItems>0)
					{
						m_Rect.Width=iWidth+m_PaddingRight-m_Rect.Left;
						m_Rect.Height=iHeight+m_PaddingBottom-m_Rect.Top;
					}
					/*else
					{
						m_Rect.Width=m_PaddingLeft+m_PaddingRight;
						m_Rect.Height=m_PaddingTop+m_PaddingBottom;
					}*/
				}
			}

			// If we don't wrap and there are items left to display
			if(!m_WrapItems && !bDisplayed && m_UseMoreItemsButton)
				CreateMoreItemsButton(isRightToLeft);
			else if(m_MoreItems!=null)
			{
				// Clean up, we don't need this anymore
				m_MoreItems.Dispose();
				m_MoreItems=null;
			}

			// Align Items if needed
			if(FarAlignLines!=null && FarAlignLines.Count>0)
				AlignItemsFar(FarAlignLines);

            if (isRightToLeft && m_Orientation == eOrientation.Horizontal)
                MirrorPositionItems();
			
			m_NeedRecalcSize=false;
			if(m_Parent!=null)
			{
				m_Parent.SubItemSizeChanged(this);
			}
		}

        protected virtual int GetItemLayoutX(BaseItem objItem, int iX)
        {
            return iX;
        }

        protected virtual int GetItemLayoutY(BaseItem objItem, int iY)
        {
            return iY;
        }

        protected virtual int GetItemLayoutHeight(BaseItem objItem)
        {
            return objItem.HeightInternal;
        }

        protected virtual int GetItemLayoutWidth(BaseItem objItem)
        {
            return objItem.WidthInternal;
        }

        protected virtual bool OnBeforeLayout()
        {
            return true;
        }

        protected void MirrorPositionItems()
        {
            Rectangle bounds = this.DisplayRectangle;
            foreach (BaseItem item in this.SubItems)
            {
                item.LeftInternal = bounds.Right - ((item.DisplayRectangle.X-bounds.X) + item.DisplayRectangle.Width);
            }
        }

		private void RecalcSizeTaskList()
		{
			int iX=m_Rect.Left+m_PaddingLeft, iY=m_Rect.Top+m_PaddingTop, iMaxItemWidth=0, iMaxItemHeight=0;
			int iWidth=0, iHeight=0;
			int iGroupLineSize=3;
			bool bDisplayed=true;
			int iDisplayed=0;
			Stack FarAlignItems=null;

            if (EffectiveStyle == eDotNetBarStyle.Office2000)
				iGroupLineSize=6;

			if(m_EqualButtonSize)
			{
				foreach(BaseItem item in m_SubItems)
				{
					item.Orientation=eOrientation.Horizontal;
					item.RecalcSize();
					if(item.WidthInternal>iMaxItemWidth)
						iMaxItemWidth=item.WidthInternal;
					if(item.HeightInternal>iMaxItemHeight)
						iMaxItemHeight=item.HeightInternal;
				}
			}

            for(int iCurrentItem=0;iCurrentItem<m_SubItems.Count;iCurrentItem++)
			{
				BaseItem objItem=m_SubItems[iCurrentItem] as BaseItem;
				objItem.Orientation=eOrientation.Horizontal;
				if(objItem==null || !objItem.Visible)
					continue;

				if(!m_EqualButtonSize)
					objItem.RecalcSize();
				
				if(objItem.BeginGroup && iDisplayed>0)
					iY+=iGroupLineSize;

				if(!m_EqualButtonSize)
				{
					if(objItem.WidthInternal>iMaxItemWidth)
						iMaxItemWidth=objItem.WidthInternal;
					if(objItem.HeightInternal>iMaxItemHeight)
						iMaxItemHeight=objItem.HeightInternal;
				}
				else
				{
					objItem.WidthInternal=iMaxItemWidth;
					objItem.HeightInternal=iMaxItemHeight;
				}

				objItem.TopInternal=iY;
				objItem.LeftInternal=iX;
				iY+=objItem.HeightInternal;
				objItem.Displayed=bDisplayed;
				if(!bDisplayed)
					continue;

				if(iY>m_Rect.Bottom)
				{
					bDisplayed=false;
					FarAlignItems=null;
					objItem.Displayed=false;
					continue;
				}

				if(FarAlignItems!=null && FarAlignItems.Count>0)
					FarAlignItems.Push(objItem);

				iDisplayed++;
				if(objItem.ItemAlignment==eItemAlignment.Far)
				{
					if(FarAlignItems==null)
						FarAlignItems=new Stack(10);
					FarAlignItems.Push(objItem);
				}
			}
			iWidth=iMaxItemWidth+m_PaddingLeft+m_PaddingRight;
			iHeight=iY+m_PaddingBottom-m_Rect.Top;
			m_DisplayedItems=iDisplayed;
			if(this.Stretch)
			{
				if(bDisplayed && FarAlignItems!=null && FarAlignItems.Count>0)
				{
					// We need to move "far" align items to the appropriate position at the bottom
					AlignItemsFar(FarAlignItems, eOrientation.Vertical); // Vertical becouse this is "vertical layout"
				}
				Bar bar=this.ContainerControl as Bar;
				if(bar==null || bar!=null && bar.DockSide!=eDockSide.Top && bar.DockSide!=eDockSide.Bottom)
					m_Rect.Width=iWidth;
				if(m_Rect.Height<iHeight)
				{
					if(m_Rect.Height<36)
						m_Rect.Height=36;
					// We need to scroll content
					m_Scroll=true;
					RepositionItems();
				}
				else
				{
					m_Scroll=false;
					m_ScrollTopPosition=0;
				}
			}
			else
			{
				m_Rect.Width=iWidth;
				if(m_Rect.Height>iHeight && bDisplayed)
					m_Rect.Height=iHeight;
				if(!bDisplayed)
				{
					if(m_Rect.Height<36)
						m_Rect.Height=36;
					// We need to scroll content
					m_Scroll=true;
					RepositionItems();
				}
				else
				{
					m_Scroll=false;
					m_ScrollTopPosition=0;
				}
			}

			if(m_MoreItems!=null)
			{
				m_MoreItems.Dispose();
				m_MoreItems=null;
			}
            
			SetupScrollButtons();
		}
		private int GroupLineSize
		{
			get
			{
                if (EffectiveStyle == eDotNetBarStyle.Office2000)
					return 6;
				return 3;
			}
		}
		private void RepositionItems()
		{
			int iTop=0;
			int iDisplayed=0;
			int iGroupLineSize=this.GroupLineSize;
			bool bDisplayed=true;
			m_TopScroll=false;
			m_BottomScroll=false;

			// Move "invisible" items up
			for(int i=m_ScrollTopPosition-1;i>=0;i--)
			{
				BaseItem objItem=m_SubItems[i];
				if(!objItem.Visible)
					continue;
				iTop-=objItem.HeightInternal;
				objItem.TopInternal=iTop;
				objItem.Displayed=false;
				if(objItem.BeginGroup && i>0)
					iTop-=iGroupLineSize;
			}
			// Move visible items down
			iTop=m_Rect.Top;
			for(int i=m_ScrollTopPosition;i<m_SubItems.Count;i++)
			{
                BaseItem objItem=m_SubItems[i];
				if(!objItem.Visible)
					continue;
				objItem.Displayed=bDisplayed;
				if(!bDisplayed)
					continue;
				if(objItem.BeginGroup && iDisplayed>0)
					iTop+=iGroupLineSize;
				objItem.TopInternal=iTop;
				iTop+=objItem.HeightInternal;
				if(iTop>m_Rect.Bottom)
				{
					objItem.Displayed=false;
					bDisplayed=false;
					continue;
				}
				iDisplayed++;
			}

			m_DisplayedItems=iDisplayed;

			if(!bDisplayed)
				m_BottomScroll=true;
			if(m_ScrollTopPosition>0)
				m_TopScroll=true;
		}

		private void SetupScrollButtons()
		{
			bool bDestroyTop=false,bDestroyBottom=false;
			if(!m_Scroll)
			{
				bDestroyTop=true;
				bDestroyBottom=true;
			}
			else
			{
				if(!m_TopScroll)
					bDestroyTop=true;
				if(!m_BottomScroll)
					bDestroyBottom=true;
			}
			if(bDestroyTop)
			{
				if(m_TopScrollButton!=null)
				{
					if(m_TopScrollButton.Parent!=null)
						m_TopScrollButton.Parent.Controls.Remove(m_TopScrollButton);
					m_TopScrollButton.Dispose();
					m_TopScrollButton=null;
				}
			}
			else
			{
				if(m_TopScrollButton==null)
				{
					System.Windows.Forms.Control ctrl=this.ContainerControl as System.Windows.Forms.Control;
					if(ctrl!=null)
					{
						m_TopScrollButton=new ScrollButton();
						m_TopScrollButton.Orientation=eOrientation.Vertical;
						m_TopScrollButton.ButtonAlignment=eItemAlignment.Near;
						ctrl.Controls.Add(m_TopScrollButton);
						m_TopScrollButton.Click+=new EventHandler(ScrollClick);
					}
				}
				if(m_TopScrollButton!=null)
				{
					m_TopScrollButton.Location=m_Rect.Location;
					m_TopScrollButton.Size=new Size(m_Rect.Width,10);
					m_TopScrollButton.BringToFront();
				}
			}

			if(bDestroyBottom)
			{
				if(m_BottomScrollButton!=null)
				{
					if(m_BottomScrollButton.Parent!=null)
						m_BottomScrollButton.Parent.Controls.Remove(m_BottomScrollButton);
					m_BottomScrollButton.Dispose();
					m_BottomScrollButton=null;
				}
			}
			else
			{
				if(m_BottomScrollButton==null)
				{
					System.Windows.Forms.Control ctrl=this.ContainerControl as System.Windows.Forms.Control;
					if(ctrl!=null)
					{
						m_BottomScrollButton=new ScrollButton();
						m_BottomScrollButton.Orientation=eOrientation.Vertical;
						m_BottomScrollButton.ButtonAlignment=eItemAlignment.Far;
						ctrl.Controls.Add(m_BottomScrollButton);
						m_BottomScrollButton.Click+=new EventHandler(ScrollClick);
					}
				}
				if(m_BottomScrollButton!=null)
				{
					m_BottomScrollButton.Location=new Point(m_Rect.X,m_Rect.Bottom-10);
					m_BottomScrollButton.Size=new Size(m_Rect.Width,10);
					m_BottomScrollButton.BringToFront();
				}
			}
		}

		private void ScrollClick(object sender, EventArgs e)
		{
			if(sender==m_TopScrollButton)
			{
				m_ScrollTopPosition--;
			}
			else
			{
				m_ScrollTopPosition++;
			}
			RepositionItems();
			SetupScrollButtons();
			this.Refresh();
		}

		private void AlignItemsFar(Stack FarAlignItems, eOrientation orientation)
		{
			int iPos=0;
			int iGroupLineSize=this.GroupLineSize;
			if(orientation==eOrientation.Horizontal)
			{
				iPos=m_Rect.Right-m_PaddingRight;
				if(m_MoreItems!=null)
					iPos-=m_MoreItems.WidthInternal;
				while(FarAlignItems.Count>0)
				{
					BaseItem objItem=FarAlignItems.Pop() as BaseItem;
					if(objItem.Displayed && objItem.Visible)
					{
						objItem.LeftInternal=iPos- GetItemLayoutWidth(objItem);
						iPos=objItem.LeftInternal-m_ItemSpacing;
						if(objItem.BeginGroup)
							iPos-=iGroupLineSize;
					}
				}
			}
			else
			{
				iPos=m_Rect.Bottom-m_PaddingBottom;
				if(m_MoreItems!=null)
					iPos-=m_MoreItems.HeightInternal;
				while(FarAlignItems.Count>0)
				{
					BaseItem objItem=FarAlignItems.Pop() as BaseItem;
					if(objItem.Displayed && objItem.Visible)
					{
						objItem.TopInternal=iPos-GetItemLayoutHeight(objItem);
						iPos=objItem.TopInternal-m_ItemSpacing;
						if(objItem.BeginGroup)
							iPos-=iGroupLineSize;
					}
				}
			}
		}
		private void AlignItemsFar(ArrayList FarAlignLines, eOrientation orientation)
		{
			if(orientation==eOrientation.Horizontal)
			{
				foreach(Stack FarAlignItems in FarAlignLines)
					AlignItemsFar(FarAlignItems,orientation);
			}
			else
			{
				foreach(Stack FarAlignItems in FarAlignLines)
					AlignItemsFar(FarAlignItems,orientation);
			}
		}
		private void AlignItemsFar(ArrayList FarAlignLines)
		{
			AlignItemsFar(FarAlignLines, this.Orientation);
		}

		private void StretchItems(ArrayList StretchLines, ArrayList StretchLinesISCount, int CalculatedSize)
		{
            int iOffset=0;
            int iStretch=0;
			for(int i=0;i<StretchLines.Count;i++)
			{
				ArrayList StretchLine=StretchLines[i] as ArrayList;
				if(m_Orientation==eOrientation.Horizontal)
					iStretch=(this.WidthInternal-CalculatedSize)/(int)StretchLinesISCount[i];
				else
					iStretch=(this.HeightInternal-CalculatedSize)/(int)StretchLinesISCount[i];

				foreach(BaseItem objItem in StretchLine)
				{
					if(objItem.Stretch)
					{
						if(m_Orientation==eOrientation.Horizontal)
						{
							objItem.LeftInternal+=iOffset;
							objItem.WidthInternal+=iStretch;
						}
						else
						{
							objItem.TopInternal+=iOffset;
							objItem.HeightInternal+=iStretch;
						}
						iOffset+=iStretch;
					}
					else
					{
						if(m_Orientation==eOrientation.Horizontal)
							objItem.LeftInternal+=iOffset;
						else
							objItem.TopInternal+=iOffset;
					}
				}
			}
		}

        protected virtual void CreateMoreItemsButton(bool isRightToLeft)
		{
			if(m_MoreItems==null)
			{
				m_MoreItems=new DisplayMoreItem();
				m_MoreItems.Style=m_Style;
				m_MoreItems.SetParent(this);
				m_MoreItems.ThemeAware=this.ThemeAware;
			}
			if(m_MoreItemsOnMenu)
				m_MoreItems.PopupType=ePopupType.Menu;
			else
                m_MoreItems.PopupType=ePopupType.ToolBar;
			m_MoreItems.Orientation=m_Orientation;
			m_MoreItems.Displayed=true;

			if(m_Orientation==eOrientation.Vertical)
			{
				m_MoreItems.WidthInternal=m_Rect.Width-(m_PaddingLeft+m_PaddingRight);
				m_MoreItems.RecalcSize();
			}
			else
			{
				m_MoreItems.HeightInternal=m_Rect.Height-(m_PaddingTop+m_PaddingBottom);
				m_MoreItems.RecalcSize();
			}

            Point loc = GetMoreItemsLocation(isRightToLeft);
            m_MoreItems.LeftInternal = loc.X;
            m_MoreItems.TopInternal = loc.Y;
		}

        protected virtual Point GetMoreItemsLocation(bool isRightToLeft)
        {
            if (m_MoreItems == null)
                return Point.Empty;
            Point p = Point.Empty;
            if (m_Orientation == eOrientation.Vertical)
            {
                p.X = m_Rect.Left + m_PaddingLeft;
                p.Y = m_Rect.Bottom - m_MoreItems.HeightInternal - m_PaddingBottom;
            }
            else
            {
                if (isRightToLeft)
                {
                    p.X = m_Rect.X + m_PaddingLeft;
                }
                else
                {
                    p.X = m_Rect.Right - m_MoreItems.WidthInternal - m_PaddingRight;
                }
                p.Y = m_Rect.Top + m_PaddingTop;
            }

            return p;
        }

		/// <summary>
		/// This must be called by child item to let the parent know that its size
		/// has been changed.
		/// </summary>
		public override void SubItemSizeChanged(BaseItem objChildItem)
		{
			//if(!objChildItem.Visible || !objChildItem.Displayed)
			//{
			//	return;
			//}
			NeedRecalcSize=true;
			if(this.Displayed)
			{
				if(m_Parent==null)
				{
					if(this.ContainerControl is Bar)
						((Bar)this.ContainerControl).RecalcLayout();
					else if(this.ContainerControl is MenuPanel)
						((MenuPanel)this.ContainerControl).RecalcSize();
				}
				else
					m_Parent.SubItemSizeChanged(this);
			}
			else
			{
				if(m_Parent==null)
					this.RecalcSize();
				else
					m_Parent.SubItemSizeChanged(this);
			}
		}

		public override void InternalMouseHover()
		{
			if(m_MoreItems!=null && m_MoreItems.Expanded && m_HotSubItem!=m_MoreItems && this.DesignMode)
			{
				m_MoreItems.Expanded=false;
			}
			base.InternalMouseHover();
		}

		public override void InternalMouseDown(System.Windows.Forms.MouseEventArgs objArg)
		{
			if(m_MoreItems!=null && m_MoreItems.Expanded && m_HotSubItem!=m_MoreItems && !this.DesignMode)
			{
				m_MoreItems.Expanded=false;
			}
			base.InternalMouseDown(objArg);
		}

		/// <summary>
		/// Occurs when the mouse pointer is over the item and a mouse button is released. This is used by internal implementation only.
		/// </summary>
		[System.ComponentModel.Browsable(false),System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override void InternalMouseUp(System.Windows.Forms.MouseEventArgs objArg)
		{
			base.InternalMouseUp(objArg);
			if(m_HotSubItem==null && (this.IsOnMenuBar || this.LayoutType==eLayoutType.Toolbar))
			{
				BaseItem expanded=this.ExpandedItem();
				if(expanded!=null && expanded is PopupItem)
					expanded.InternalMouseUp(objArg);
			}
		}

		public override void InternalMouseMove(System.Windows.Forms.MouseEventArgs objArg)
		{
			if(m_IgnoreDuplicateMouseMove)
			{
				if(m_LastMouseMoveEvent==null)
				{
					m_LastMouseMoveEvent=new System.Windows.Forms.MouseEventArgs(objArg.Button,objArg.Clicks,objArg.X,objArg.Y,objArg.Delta);
					return;
				}
				if(m_LastMouseMoveEvent.X!=objArg.X || m_LastMouseMoveEvent.Y!=objArg.Y ||
					m_LastMouseMoveEvent.Button!=objArg.Button)
				{
					m_IgnoreDuplicateMouseMove=false;
					m_LastMouseMoveEvent=null;
				}
				else
					return;
			}

			//DotNetBarManager owner=this.GetOwner();
			//if(!this.DesignMode && owner!=null && owner.GetFocusItem()!=null)
			//	return;

			// Call base implementation if we don't have the more items button
			if(m_MoreItems==null || !m_MoreItems.DisplayRectangle.Contains(objArg.X,objArg.Y) || this.DesignMode)
			{
				bool bMenuBar=this.IsOnMenuBar;
                if (bMenuBar || this.LayoutType == eLayoutType.Toolbar)
                {
                    BaseItem expanded = this.ExpandedItem();
                    if (expanded != null)
                        expanded.InternalMouseMove(objArg);
                    m_CheckMouseMovePressed = true;
                }
                else
                    m_CheckMouseMovePressed = false;
				
				if(objArg.Button==System.Windows.Forms.MouseButtons.None || this.DesignMode || (bMenuBar || this.LayoutType==eLayoutType.Toolbar))
					base.InternalMouseMove(objArg);

				return;
			}
			// Mouse is over our more items button
			BaseItem objNew=m_MoreItems;
			if(objNew!=m_HotSubItem)
			{
				if(m_HotSubItem!=null)
				{
					m_HotSubItem.InternalMouseLeave();
					//if(objNew!=null && m_AutoExpand && m_HotSubItem.Expanded)
					if(objNew!=null && m_HotSubItem.Expanded)
						m_HotSubItem.Expanded=false;
				}
				
				if(objNew!=null)
				{
					if(m_AutoExpand)
					{
						BaseItem objItem=ExpandedItem();
						if(objItem!=null && objItem!=objNew)
							objItem.Expanded=false;
					}
					objNew.InternalMouseEnter();
					objNew.InternalMouseMove(objArg);
                    if (m_AutoExpand && objNew.GetEnabled() && objNew.ShowSubItems)
					{
						if(objNew is PopupItem)
						{
							PopupItem pi=objNew as PopupItem;
							ePopupAnimation oldAnim=pi.PopupAnimation;
							pi.PopupAnimation=ePopupAnimation.None;
							objNew.Expanded=true;
                            pi.PopupAnimation=oldAnim;
						}
						else
							objNew.Expanded=true;
					}
					m_HotSubItem=objNew;
				}
				else
					m_HotSubItem=null;
			}
			else if(m_HotSubItem!=null)
			{
				m_HotSubItem.InternalMouseMove(objArg);
			}
		}

		public override void InternalKeyDown(System.Windows.Forms.KeyEventArgs objArg)
		{
			base.InternalKeyDown(objArg);
			if(SubItems.Count==0 || objArg.Handled)
				return;

			BaseItem objExpanded=this.ExpandedItem();

			if(objExpanded!=null)
			{
				objExpanded.InternalKeyDown(objArg);
				if(objArg.Handled)
					return;
			}
			
			if(objArg.KeyCode==System.Windows.Forms.Keys.Right || objArg.KeyCode==System.Windows.Forms.Keys.Tab || objArg.KeyCode==System.Windows.Forms.Keys.Left)
			{
				m_IgnoreDuplicateMouseMove=true;
				// Select next object
				if(m_HotSubItem!=null)
				{
					m_HotSubItem.InternalMouseLeave();
					if(m_AutoExpand && m_HotSubItem.Expanded)
					{
						m_HotSubItem.Expanded=false;
					}
				}
				if(objArg.KeyCode==System.Windows.Forms.Keys.Left)
				{
					int iIndex=0;
					if(m_HotSubItem!=null)
						iIndex=this.SubItems.IndexOf(m_HotSubItem)-1;
					if(iIndex<0)
						iIndex=this.SubItems.Count-1;
					BaseItem objNew=null;
					bool bRepeat=false;
					do
					{
						for(int i=iIndex;i>=0;i--)
						{
							objNew=this.SubItems[i];
                            if (objNew.Visible && objNew.GetEnabled())
							{
								iIndex=i;
								break;
							}
						}
                        if (!this.SubItems[iIndex].Visible || !this.SubItems[iIndex].GetEnabled())
						{
							if(!bRepeat)
							{
								iIndex=this.SubItems.Count-1;
								bRepeat=true;
							}
							else
								bRepeat=false;
						}
						else
							bRepeat=false;
					} while(bRepeat);
					m_HotSubItem=this.SubItems[iIndex];
				}
				else
				{
					int iIndex=0;
					if(m_HotSubItem!=null)
						iIndex=this.SubItems.IndexOf(m_HotSubItem)+1;
                    while (iIndex < this.SubItems.Count && (!this.SubItems[iIndex].Visible || !this.SubItems[iIndex].GetEnabled()))
						iIndex++;
					if(iIndex>=this.SubItems.Count)
						iIndex=0;
					BaseItem objNew=null;
					for(int i=iIndex;i<this.SubItems.Count;i++)
					{
						objNew=this.SubItems[i];
                        if (objNew.Visible && objNew.GetEnabled())
						{
							iIndex=i;
							break;
						}
					}
					m_HotSubItem=this.SubItems[iIndex];
				}

				m_HotSubItem.InternalMouseEnter();
                m_HotSubItem.InternalMouseMove(new MouseEventArgs(MouseButtons.None, 0, m_HotSubItem.LeftInternal + 2, m_HotSubItem.TopInternal + 2, 0));
                //if(m_AutoExpand)
                //{
					BaseItem objItem=this.ExpandedItem();
					if(objItem!=null && objItem!=m_HotSubItem)
						objItem.Expanded=false;
				//}
				if(m_AutoExpand && m_HotSubItem.ShowSubItems && (this.IsOnMenuBar || this.IsOnMenu && m_HotSubItem.SubItems.Count>0))
				{
					m_HotSubItem.Expanded=true;
					if(m_HotSubItem is PopupItem && ((PopupItem)m_HotSubItem).PopupControl is MenuPanel)
						((MenuPanel)((PopupItem)m_HotSubItem).PopupControl).SelectFirstItem();
				}

				objArg.Handled=true;
			}
            if (objArg.KeyCode == System.Windows.Forms.Keys.Down && this.IsOnMenuBar)
            {
                m_IgnoreDuplicateMouseMove = true;
                // Select next object
                if (m_HotSubItem != null && !m_HotSubItem.Expanded && m_HotSubItem.VisibleSubItems > 0)
                    m_HotSubItem.Expanded = true;
                
            }
            else if (objArg.KeyCode == System.Windows.Forms.Keys.Escape)
            {
                if (objExpanded != null)
                {
                    objExpanded.Expanded = false;
                    objArg.Handled = true;
                }
                else
                {
                    Control cc = this.ContainerControl as Control;
                    if (cc is Bar)
                    {
                        Bar bar = cc as Bar;
                        if (bar.BarState == eBarState.Popup)
                        {
                            bar.ParentItem.Expanded = false;
                        }
                        else
                        {
                            if (this.AutoExpand)
                                this.AutoExpand = false;
                            else if (bar.Focused || bar.MenuFocus)
                            {
                                bar.MenuFocus = false;
                                bar.ReleaseFocus();
                            }
                        }
                        objArg.Handled = true;
                    }
                    else if (cc is ItemControl)
                    {
                        ItemControl ic = cc as ItemControl;
                        if (this.AutoExpand)
                            this.AutoExpand = false;
                        else if (ic.Focused || ic.MenuFocus)
                        {
                            ic.MenuFocus = false;
                            ic.ReleaseFocus();
                        }
                    }
                }
            }
            else
            {
                BaseItem objItem = this.ExpandedItem();
                if (objItem != null)
                    objItem.InternalKeyDown(objArg);
                else
                {
                    int key = 0;
                    if (objArg.Shift)
                    {
                        try
                        {
                            byte[] keyState = new byte[256];
                            if (NativeFunctions.GetKeyboardState(keyState))
                            {
                                byte[] chars = new byte[2];
                                if (NativeFunctions.ToAscii((uint)objArg.KeyValue, 0, keyState, chars, 0) != 0)
                                {
                                    key = chars[0];
                                }
                            }
                        }
                        catch (Exception)
                        {
                            key = 0;
                        }
                    }

                    if (key == 0)
                        key = (int)NativeFunctions.MapVirtualKey((uint)objArg.KeyValue, 2);

                    bool b = false;
                    if (key > 0)
                        b = AccessKeyDown(key);
                    if (b)
                        objArg.Handled = true;
                    if (!b && m_HotSubItem != null)
                        m_HotSubItem.InternalKeyDown(objArg);
                }
            }
		}

		// Checks whether Access key is pressed and expands item if it was. Applies to menu bar items only
		// Assumes that NO item is expanded when call is made
		private bool AccessKeyDown(int KeyCode)
		{
			char[] ch=new char[1];
			byte[] by=new byte[1];
			try
			{
				by[0]=System.Convert.ToByte(KeyCode);
				System.Text.Encoding.Default.GetDecoder().GetChars(by,0,1,ch,0);
			}
			catch(Exception)
			{
				return false;
			}
			string s=ch[0].ToString();
			if(s=="")
				return false;
			ch[0]=(s.ToLower())[0];
			BaseItem expandedItem=this.ExpandedItem();
			if(expandedItem!=null && expandedItem is PopupItem && ((PopupItem)expandedItem).PopupType==ePopupType.Menu)
			{
				System.Windows.Forms.KeyEventArgs ke=new System.Windows.Forms.KeyEventArgs((System.Windows.Forms.Keys)KeyCode);
				expandedItem.InternalKeyDown(ke);
				if(ke.Handled)
				{
					m_IgnoreDuplicateMouseMove=true;
					return true;
				}
			}

			foreach(BaseItem objItem in this.SubItems)
			{
                if (objItem.Visible && objItem.GetEnabled() && objItem.Displayed && objItem.AccessKey == ch[0])
				{
					if(objItem is ButtonItem && ((ButtonItem)objItem).ButtonStyle==eButtonStyle.Default && !this.IsOnMenuBar && !this.IsOnMenu)
						continue;
                    if (objItem.SubItems.Count > 0 && objItem.ShowSubItems && objItem.GetEnabled())
					{
						if(objItem.Expanded)
						{
							objItem.InternalKeyDown(new System.Windows.Forms.KeyEventArgs((System.Windows.Forms.Keys)KeyCode));
						}
						else
							ExpandItem(objItem);
					}
					else
						objItem.RaiseClick();
					m_IgnoreDuplicateMouseMove=true;
					return true;
				}
			}
			return false;
		}

		internal bool SysKeyDown(int KeyCode)
		{
			return AccessKeyDown(KeyCode);
//			char[] ch=new char[1];
//			byte[] by=new byte[1];
//			by[0]=System.Convert.ToByte(KeyCode);
//			System.Text.Encoding.Default.GetDecoder().GetChars(by,0,1,ch,0);
//			string s=ch[0].ToString();
//			ch[0]=(s.ToLower())[0];
//			foreach(BaseItem objItem in this.SubItems)
//			{
//				if(objItem.Displayed && objItem.AccessKey==ch[0])
//				{
//					if(!objItem.Expanded)
//					{
//						ExpandItem(objItem);
////						// Set the focus here so it can be properly released...
////						Bar bar=this.ContainerControl as Bar;
////						if(bar!=null)
////							bar.Focus();
////						if(m_HotSubItem!=null)
////							m_HotSubItem.InternalMouseLeave();
////						m_HotSubItem=objItem;
////						objItem.InternalMouseEnter();
////						objItem.Expanded=true;
////						if(objItem is PopupItem)
////							this.AutoExpand=true;
//					}
//					return true;
//				}
//			}
//			return false;
		}

		private void ExpandItem(BaseItem objItem)
		{
			// Set the focus here so it can be properly released...
            Control cc = this.ContainerControl as Control;
			//if(bar!=null && !bar.Focused) // Commented out to fix menu bar taking the focus
			//	bar.Focus();
            if (cc is Bar && !cc.Focused)
                ((Bar)cc).MenuFocus = true;

			if(m_HotSubItem!=null)
				m_HotSubItem.InternalMouseLeave();
			m_HotSubItem=objItem;
			objItem.InternalMouseEnter();
            objItem.InternalMouseMove(new MouseEventArgs(MouseButtons.None, 0, objItem.LeftInternal + 2, objItem.TopInternal + 2, 0));
			objItem.Expanded=true;
			if(objItem is PopupItem)
			{
				this.AutoExpand=true;
				// If it is a menu select first menu item inside...
				PopupItem popup=objItem as PopupItem;
				if(popup.PopupType==ePopupType.Menu && popup.PopupControl is MenuPanel)
				{
					((MenuPanel)popup.PopupControl).SelectFirstItem();
				}
			}
		}

		public bool EqualButtonSize
		{
			get
			{
				return m_EqualButtonSize;
			}
			set
			{
				if(m_EqualButtonSize!=value)
				{
					m_EqualButtonSize=value;
					NeedRecalcSize=true;
				}
			}
		}

		/*public bool Stretch
		{
			get
			{
				return m_Stretch;
			}
			set
			{
				if(m_Stretch!=value)
				{
					m_Stretch=value;
					m_NeedRecalcSize=true;
				}
			}
		}*/

		public override void ContainerLostFocus(bool appLostFocus)
		{
            base.ContainerLostFocus(appLostFocus);
			if(m_MoreItems!=null)
                m_MoreItems.ContainerLostFocus(appLostFocus);
			IOwnerMenuSupport ownersupport=this.GetOwner() as IOwnerMenuSupport;
			if(this.IsOnMenuBar && ownersupport!=null)
				ownersupport.PersonalizedAllVisible=false;
				
		}

		/// <summary>
		/// Called when item owner has changed.
		/// </summary>
		protected override void OnOwnerChanged()
		{
			base.OnOwnerChanged();
			if(this.GetOwner() is Bar)
			{
				this.RefreshImageSize();
			}
		}

		protected internal override void OnSubItemExpandChange(BaseItem objItem)
		{
			base.OnSubItemExpandChange(objItem);
			if(objItem.Expanded)
			{
				foreach(BaseItem objExp in this.SubItems)
				{
					if(objExp!=objItem && objExp is PopupItem && objExp.Expanded)
						objExp.Expanded=false;
				}
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the item is expanded or not. For Popup items this would indicate whether the item is popped up or not.
		/// </summary>
		[System.ComponentModel.Browsable(false),System.ComponentModel.DefaultValue(false),System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		public override bool Expanded
		{
			get
			{
				return m_Expanded;
			}
			set
			{
				base.Expanded=value;
				if(!value && this.ContainerControl is ItemControl)
					BaseItem.CollapseSubItems(this);
			}
		}

		internal void SetSystemFocus()
		{
			if(m_HotSubItem!=null || this.SubItems.Count==0)
				return;

            BaseItem exp = this.ExpandedItem();
            if (exp != null)
            {
                m_HotSubItem = exp;
                m_HotSubItem.InternalMouseEnter();
                m_HotSubItem.InternalMouseMove(new MouseEventArgs(MouseButtons.None, 0, m_HotSubItem.LeftInternal + 2, m_HotSubItem.TopInternal + 2, 0));
                return;
            }

			foreach(BaseItem objItem in this.SubItems)
			{
				if(!objItem.SystemItem && objItem.Displayed && objItem.Visible)
				{
					m_HotSubItem=objItem;
					m_HotSubItem.InternalMouseEnter();
                    m_HotSubItem.InternalMouseMove(new MouseEventArgs(MouseButtons.None, 0, m_HotSubItem.LeftInternal + 2, m_HotSubItem.TopInternal + 2, 0));
					break;
				}
			}
		}

		internal void ReleaseSystemFocus()
		{
			CollapseSubItems(this);
			if(m_HotSubItem!=null)
			{
                Control c = this.ContainerControl as Control;
                if (c != null)
                {
                    Point p = c.PointToClient(Control.MousePosition);
                    if (m_HotSubItem.DisplayRectangle.Contains(p))
                        return;
                }
				m_HotSubItem.InternalMouseLeave();
				m_HotSubItem=null;
			}
		}

		/// <summary>
		/// Gets or sets whether container is system container used internally by DotNetBar.
		/// </summary>
		[System.ComponentModel.Browsable(false),DevCoBrowsable(false),System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden),System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public bool SystemContainer
		{
			get
			{
				return m_SystemContainer;
			}
			set
			{
				m_SystemContainer=value;
			}
		}

		public bool HaveCustomizeItem
		{
			get
			{
				return m_HaveCustomizeItem;
			}
		}

		protected internal override void OnItemAdded(BaseItem objItem)
		{
			base.OnItemAdded(objItem);
            if (ItemAdded != null)
                ItemAdded(objItem, new EventArgs());
			if(objItem is CustomizeItem)
				m_HaveCustomizeItem=true;
			else if(this.SystemContainer && objItem is DockContainerItem)
			{
				Bar bar=this.ContainerControl as Bar;
				if(bar!=null && bar.LayoutType==eLayoutType.DockContainer)
				{
					if(this.VisibleSubItems>1)
					{
						objItem.Displayed=false;
					}
					bar.RefreshDockTab(true);
				}
			}
		}
		protected internal override void OnAfterItemRemoved(BaseItem objItem)
		{
			base.OnAfterItemRemoved(objItem);
			if(objItem is CustomizeItem)
				m_HaveCustomizeItem=false;
			else if(this.SystemContainer)
			{
				Bar bar=this.ContainerControl as Bar;
				if(bar!=null)
					bar.OnSubItemRemoved(objItem);
			}
		}

		protected internal override bool IsAnyOnHandle(int iHandle)
		{
			if(m_MoreItems!=null)
			{
				if(m_MoreItems.IsAnyOnHandle(iHandle))
					return true;
			}
			return base.IsAnyOnHandle(iHandle);
		}

        /// <summary>
        /// Return Sub Item at specified location
        /// </summary>
        public override BaseItem ItemAtLocation(int x, int y)
        {
            if (m_SubItems != null)
            {
                foreach (BaseItem objSub in m_SubItems)
                {
                    if (objSub.IsContainer)
                    {
                        BaseItem item = objSub.ItemAtLocation(x, y);
                        if (item != null)
                            return item;
                    }
                    else
                    {
                        if ((objSub.Visible || this.IsOnCustomizeMenu) && objSub.Displayed && objSub.DisplayRectangle.Contains(x, y))
                        {
                            return objSub;
                        }
                    }
                }
            }

            return null;
        }


		public override bool AutoExpand
		{
			get
			{
				return base.AutoExpand;
			}
			set
			{
				base.AutoExpand=value;
				if(this.SystemContainer)
				{
					Bar bar=this.ContainerControl as Bar;
					if(bar!=null)
					{
						// TODO: To fix the focus menu expand comment this out
//						if(this.AutoExpand && !bar.Focused)
//                            bar.Focus();
//						else if(!this.AutoExpand && bar.Focused)
//							bar.ReleaseFocus();
						if(this.AutoExpand && !bar.MenuFocus)
						    bar.MenuFocus=true;
						else if(!this.AutoExpand && bar.MenuFocus)
							bar.MenuFocus=false;
					}
				}
			}
		}

		internal bool EventHeight
		{
			get {return m_EventHeight;}
			set {m_EventHeight=value;}
		}

		internal bool UseMoreItemsButton
		{
			get {return m_UseMoreItemsButton;}
			set {m_UseMoreItemsButton=value;}
		}

        internal eContainerVerticalAlignment ToolbarItemsAlign
        {
            get { return m_ToolbarItemsAlign; }
            set { m_ToolbarItemsAlign = value; }
        }

		public virtual eLayoutType LayoutType
		{
			get
			{
				return m_LayoutType;
			}
			set
			{
				if(m_LayoutType==value)
					return;
				m_LayoutType=value;
				NeedRecalcSize=true;
				if(m_LayoutType==eLayoutType.TaskList)
				{
					base.Orientation=eOrientation.Horizontal;
				}
				else
				{
					if(this.ContainerControl!=null && this.ContainerControl is Bar)
					{
						Bar bar=this.ContainerControl as Bar;
						if(bar.DockedSite!=null && bar.DockedSite is DockSite)
                            base.Orientation=((DockSite)bar.DockedSite).DockOrientation;
					}
				}
			}
		}

		[System.ComponentModel.Browsable(false)]
		public override eOrientation Orientation
		{
			get
			{
				if(m_LayoutType==eLayoutType.TaskList)
					return eOrientation.Vertical;
				return base.Orientation;
			}
			set
			{
				if(m_LayoutType==eLayoutType.TaskList)
					return;
				base.Orientation=value;
			}
		}

		public Color BackColor
		{
			get
			{
				if(!m_BackgroundColor.IsEmpty)
					return m_BackgroundColor;
                else if (EffectiveStyle == eDotNetBarStyle.Office2000 || this.IsOnMenuBar)
					return SystemColors.Control;
				else
					return ColorFunctions.ToolMenuFocusBackColor();
			}
			set
			{
				if(m_BackgroundColor!=value)
				{
					m_BackgroundColor=value;
					this.Refresh();
				}
			}
		}

        /// <summary>
        /// When parent items does recalc size for its sub-items it should query
        /// image size and store biggest image size into this property.
        /// </summary>
        [System.ComponentModel.Browsable(false), System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden), System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public override System.Drawing.Size SubItemsImageSize
        {
            get
            {
                return base.SubItemsImageSize;
            }
            set
            {
                if(m_TrackSubItemsImageSize)
                    base.SubItemsImageSize = value;
            }
        }

        internal bool TrackSubItemsImageSize
        {
            get { return m_TrackSubItemsImageSize; }
            set
            {
                m_TrackSubItemsImageSize = value;
                if (!m_TrackSubItemsImageSize)
                    base.SubItemsImageSize = new Size(16, 16);
            }
        }

		//***********************************************
		// IDesignTimeProvider Implementation
		//***********************************************
		public InsertPosition GetInsertPosition(Point pScreen, BaseItem DragItem)
		{
            return DesignTimeProviderContainer.GetInsertPosition(this, pScreen, DragItem);
		}
		public void DrawReversibleMarker(int iPos, bool Before)
		{
            DesignTimeProviderContainer.DrawReversibleMarker(this, iPos, Before);
            return;
		}

		public void InsertItemAt(BaseItem objItem, int iPos, bool Before)
		{
            DesignTimeProviderContainer.InsertItemAt(this, objItem, iPos, Before);
            return;
		}

		private int GetAppendPosition(BaseItem objParent)
		{
			int iPos=-1;
			for(int i=objParent.SubItems.Count-1;i>=0;i--)
			{
				if(objParent.SubItems[i].SystemItem)
					iPos=i;
				else
					break;
			}
			return iPos;
		}

		public int PaddingLeft
		{
			get{return m_PaddingLeft;}
			set{m_PaddingLeft=value;}
		}
		public int PaddingRight
		{
			get{return m_PaddingRight;}
			set{m_PaddingRight=value;}
		}
		public int PaddingTop
		{
			get{return m_PaddingTop;}
			set{m_PaddingTop=value;}
		}
		public int PaddingBottom
		{
			get{return m_PaddingBottom;}
			set{m_PaddingBottom=value;}
		}
    }
}
