using System;
using System.Drawing;
using System.Drawing.Design;
using System.ComponentModel;
using System.ComponentModel.Design;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Represents the Side-Bar Panel item.
	/// </summary>
    [System.ComponentModel.ToolboxItem(false), Designer("DevComponents.DotNetBar.Design.SideBarPanelItemDesigner, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
	public class SideBarPanelItem:ImageItem,IDesignTimeProvider
	{
		private int m_TopItemIndex=0;
		private bool m_ScrollDownButton=false;
		private Rectangle[] m_ScrollButtonRect={Rectangle.Empty,Rectangle.Empty};
		private int m_ScrollButtonHot=-1;
		private int m_Margin=3;
		private bool m_MouseOver=false;
		private bool m_MouseDown=false;
		private Rectangle m_PanelRect=Rectangle.Empty;
		//private Color m_BackColor=SystemColors.Control;
		private Color m_ForeColor=SystemColors.ControlText;
		private Color m_HotForeColor=Color.Empty;
		private bool m_HotFontUnderline=false;
		private bool m_HotFontBold=false;
        private bool m_FontBold = false;
		private eBarImageSize m_ItemImageSize=eBarImageSize.Default;
		private System.Drawing.Point m_MouseDownPt;
		// Used to host items that have windows associated with them....
		private SideBarPanelControlHost m_ControlHost=null;

		private ItemStyle m_BackgroundStyle=new ItemStyle();
		private ItemStyle m_HeaderStyle=null, m_HeaderHotStyle=null, m_HeaderMouseDownStyle=null;
		private ItemStyle m_HeaderSideStyle=null, m_HeaderSideHotStyle, m_HeaderSideMouseDownStyle;

		private bool m_WordWrap=false;

		private const string INFO_EMPTYPANEL="Right-click header and choose Add New Button or use SubItems collection to create new buttons.";

		private System.Drawing.Image m_Image;
		private int m_ImageIndex=-1; // Image index if image from ImageList is used
		private System.Drawing.Image m_HoverImage;
		private int m_HoverImageIndex=-1; // Image index if image from ImageList is used
		private System.Drawing.Image m_PressedImage;
		private int m_PressedImageIndex=-1;  // Image index if image from ImageList is used
		System.Drawing.Icon m_Icon=null;
		private System.Drawing.Image m_ImageCachedIdx=null;
		private ItemPaintArgs _ItemPaintArgs=null;
		private eSideBarAppearance m_Appearance=eSideBarAppearance.Traditional;
		
		private const int FLAT_NOIMAGESIDEWIDTH=26;
		private const int IMAGE_MARGIN=2;

		private bool m_IgnoreNextClick=false;
		private DateTime m_IgnoreClickDateTime=DateTime.MinValue;
		private Rectangle m_IgnoreScrollRect=Rectangle.Empty;

		private bool m_EnableScrollButtons=true;

		private eSideBarLayoutType m_LayoutType=eSideBarLayoutType.Default;

		/// <summary>
		/// Creates new instance of SideBarPanelItem.
		/// </summary>
		public SideBarPanelItem():this("","") {}
		/// <summary>
		/// Creates new instance of SideBarPanelItem and assigns the name to it.
		/// </summary>
		/// <param name="sItemName">Item name.</param>
		public SideBarPanelItem(string sItemName):this(sItemName,"") {}
		/// <summary>
		/// Creates new instance of SideBarPanelItem and assigns the name and text to it.
		/// </summary>
		/// <param name="sItemName">Item name.</param>
		/// <param name="ItemText">item text.</param>
		public SideBarPanelItem(string sItemName, string ItemText):base(sItemName,ItemText)
		{
			m_IsContainer=true;
			m_BackgroundStyle.VisualPropertyChanged+=new EventHandler(this.VisualPropertyChanged);
		}

        protected override void Dispose(bool disposing)
        {
            if (BarUtilities.DisposeItemImages && !this.DesignMode)
            {
                BarUtilities.DisposeImage(ref m_Image);
                BarUtilities.DisposeImage(ref m_ImageCachedIdx);
                BarUtilities.DisposeImage(ref m_Icon);
                BarUtilities.DisposeImage(ref m_HoverImage);
                BarUtilities.DisposeImage(ref m_PressedImage);
            }
            base.Dispose(disposing);
        }

		/// <summary>
		/// Returns copy of SideBarPanelItem item.
		/// </summary>
		public override BaseItem Copy()
		{
			SideBarPanelItem objCopy=new SideBarPanelItem();
			this.CopyToItem(objCopy);
			return objCopy;
		}
		protected override void CopyToItem(BaseItem copy)
		{
			SideBarPanelItem objCopy=copy as SideBarPanelItem;
			
			base.CopyToItem(objCopy);

			objCopy.HotFontBold=m_HotFontBold;
			objCopy.HotFontUnderline=m_HotFontUnderline;
			objCopy.HotForeColor=m_HotForeColor;
			objCopy.ForeColor=m_ForeColor;
			//objCopy.BackColor=m_BackColor;
			objCopy.ItemImageSize=m_ItemImageSize;
			objCopy.SetBackgroundStyle(m_BackgroundStyle.Clone() as ItemStyle);
		}
		private void VisualPropertyChanged(object sender, EventArgs e)
		{
			RefreshItemStyleSystemColors();
			NeedRecalcSize=true;
			OnAppearanceChanged();
		}
		internal void RefreshItemStyleSystemColors()
		{
			// Apply any color scheme changes needed
			SideBar sb=this.ContainerControl as SideBar;
			if(sb!=null)
			{
				if(m_BackgroundStyle!=null)
					m_BackgroundStyle.ApplyColorScheme(sb.ColorScheme);
				if(m_HeaderStyle!=null)
					m_HeaderStyle.ApplyColorScheme(sb.ColorScheme);
				if(m_HeaderHotStyle!=null)
					m_HeaderHotStyle.ApplyColorScheme(sb.ColorScheme);
				if(m_HeaderMouseDownStyle!=null)
					m_HeaderMouseDownStyle.ApplyColorScheme(sb.ColorScheme);
				if(m_HeaderSideStyle!=null)
					m_HeaderSideStyle.ApplyColorScheme(sb.ColorScheme);
				if(m_HeaderSideHotStyle!=null)
					m_HeaderSideHotStyle.ApplyColorScheme(sb.ColorScheme);
				if(m_HeaderSideMouseDownStyle!=null)
					m_HeaderSideMouseDownStyle.ApplyColorScheme(sb.ColorScheme);
			}
		}
		public override void RecalcSize()
		{
			m_ScrollDownButton=false;

			System.Windows.Forms.Control objCtrl=this.ContainerControl as System.Windows.Forms.Control;
			if(!IsHandleValid(objCtrl))
				return;
            
			CompositeImage image=this.GetImage();
			System.Drawing.Size imageSize=System.Drawing.Size.Empty;
			if(image!=null)
			{
				imageSize=image.Size;
				imageSize.Width+=(IMAGE_MARGIN*2);
				imageSize.Height+=(IMAGE_MARGIN*2);
			}
			image=null;

			Size objStringSize=Size.Empty;
			string text=m_Text;

            Graphics g = BarFunctions.CreateGraphics(objCtrl);
            try
            {
                if (text != "")
                    objStringSize = TextDrawing.MeasureString(g, text, this.GetFont(), m_Rect.Width, this.GetStringFormat());
            }
            finally
            {
                g.Dispose();
            }

			if(imageSize.Height>objStringSize.Height+m_Margin*2)
				m_PanelRect=new Rectangle(0,0,m_Rect.Width,imageSize.Height);
			else
			{
				m_PanelRect=new Rectangle(0,0,m_Rect.Width,(int)objStringSize.Height+m_Margin*2);
				if(m_Appearance==eSideBarAppearance.Flat)
					m_PanelRect.Height+=(IMAGE_MARGIN*2);
			}

			if(this.Expanded)
			{
				// Take suggested height and set the actual height only when suggested is less than minimum
				if(m_Rect.Height<m_PanelRect.Height)
					m_Rect.Height=m_PanelRect.Height;
				int clientWidth=m_Rect.Width;

				bool bMultiColumnPass=false;
				if(m_LayoutType==eSideBarLayoutType.MultiColumn && m_TopItemIndex>0)
					bMultiColumnPass=true;

				if(m_SubItems!=null)
				{
					int iRepeat=0;
					int iTop=0, iHeightThreshold=0;
					do
					{
						iRepeat++;
						iTop=m_Rect.Top+m_PanelRect.Height+1;
						int iLeft=m_Rect.Left;
						iHeightThreshold=m_Rect.Bottom;
						if(m_ControlHost!=null)
						{
							iTop=0;
							iLeft=0;
							iHeightThreshold=m_Rect.Height-(m_PanelRect.Height+1);
						}
						int iIndex=-1;
						int iVisibleHeight=iTop;
						int itemWidth=m_Rect.Width;
						
						if(m_Appearance==eSideBarAppearance.Flat)
						{
							if(imageSize.IsEmpty)
								itemWidth-=FLAT_NOIMAGESIDEWIDTH;
							else
								itemWidth-=imageSize.Width;
							if(itemWidth<=0)
								itemWidth=m_Rect.Width;
							if(itemWidth>16)
							{
								itemWidth-=4;
								iLeft+=m_Rect.Width-itemWidth-2;
							}
							else
							{
								iLeft+=m_Rect.Width-itemWidth;
							}
							clientWidth=itemWidth+4;
							if(m_ControlHost!=null)
								iLeft=2;
						}

						// Used for column based layout
						int initalLeft=iLeft;
						int lineHeight=0;

						foreach(BaseItem item in m_SubItems)
						{
							iIndex++;
							if(!item.Visible || iTop>iHeightThreshold && !bMultiColumnPass)
							{
								if(m_EnableScrollButtons && item.Visible)
									m_ScrollDownButton=true;
								if(item.Visible && iTop>iHeightThreshold)
								{
									item.RecalcSize();
									iVisibleHeight+=item.HeightInternal;
								}
								item.Displayed=false;
								continue;
							}
							else if(iIndex<m_TopItemIndex && !bMultiColumnPass)
							{
								item.WidthInternal=itemWidth;
								item.RecalcSize();
								iVisibleHeight+=item.HeightInternal;
								item.Displayed=false;
								continue;
							}
							if(m_LayoutType==eSideBarLayoutType.MultiColumn)
							{
								item.WidthInternal=itemWidth;
								item.RecalcSize();
								if(item.WidthInternal>itemWidth && lineHeight==0)
								{
									item.WidthInternal=itemWidth;
									lineHeight=item.HeightInternal;
								}
								else
								{
									if(iLeft+item.WidthInternal-initalLeft>itemWidth)
									{
										// Switch the line
										iLeft=initalLeft;
										iTop+=lineHeight;
										iVisibleHeight+=lineHeight;
										lineHeight=0;
									}
									if(item.HeightInternal>lineHeight)
										lineHeight=item.HeightInternal;
									item.TopInternal=iTop;
									item.LeftInternal=iLeft;
									iLeft+=item.WidthInternal;
								}
							}
							else
							{
								item.WidthInternal=itemWidth;
								item.RecalcSize();
								item.WidthInternal=itemWidth;
								if(this.DesignMode && item.IsWindowed && iTop+item.HeightInternal>iHeightThreshold)
								{
									iVisibleHeight+=item.HeightInternal;
									item.Displayed=false;
									continue;
								}
								else
								{
									if(item.BeginGroup)
									{
										iTop+=3;
										iVisibleHeight+=3;
									}
									item.TopInternal=iTop;
									item.LeftInternal=iLeft;
									iTop+=item.HeightInternal;
									iVisibleHeight+=item.HeightInternal;
								}
							}
							item.Displayed=true;
						}

						if(m_LayoutType==eSideBarLayoutType.MultiColumn && lineHeight>0)
						{
							iVisibleHeight+=lineHeight;
							iTop+=lineHeight;
						}
						
						if(bMultiColumnPass)
						{
							// Make sure that top index is correct
							int iTopY=this.SubItems[m_TopItemIndex].TopInternal;
							int iTopIndex=m_TopItemIndex;
							while(iTopIndex>0)
							{
								iTopIndex--;
								if(this.SubItems[iTopIndex].Visible && this.SubItems[iTopIndex].TopInternal<iTopY)
								{
									iTopIndex++;
									break;
								}
							}
							m_TopItemIndex=iTopIndex;
							bMultiColumnPass=false;
							iRepeat=2; // Make sure this loop repeats
						}
						else
						{
							if(iVisibleHeight>0 && iVisibleHeight<iHeightThreshold && m_TopItemIndex>0)
							{
								m_TopItemIndex=0;
								m_ScrollDownButton=false;
								iRepeat++;
							}
						}
					}while(iRepeat==2);
                    if(iTop>iHeightThreshold && m_EnableScrollButtons)
						m_ScrollDownButton=true;
				}
				if(m_ControlHost!=null)
				{
					System.Drawing.Size controlSize=Size.Empty;
					if(m_Appearance==eSideBarAppearance.Traditional)
						controlSize=new Size(m_Rect.Width,m_Rect.Height-(m_PanelRect.Height+1));
					else
						controlSize=new Size(clientWidth,m_Rect.Height-m_PanelRect.Height);
					System.Drawing.Point location=System.Drawing.Point.Empty;
					if(m_Appearance==eSideBarAppearance.Traditional)
						location=new Point(m_Rect.Left,m_Rect.Top+m_PanelRect.Height+1);
					else
						location=new Point(m_Rect.Right-clientWidth,m_Rect.Top+m_PanelRect.Height);

					m_ControlHost.Size=controlSize;
					m_ControlHost.Location=location;
					m_ControlHost.Visible=true;
				}
			}
			else
			{
				if(m_ControlHost!=null)
					m_ControlHost.Visible=false;
				// When not expanded just set the height and return
                m_Rect.Height=m_PanelRect.Height;
				foreach(BaseItem item in m_SubItems)
				{
					item.Displayed=false;
				}
			}

			if(m_ControlHost!=null && this.Expanded)
				m_ControlHost.SetupScrollButtons();
			base.RecalcSize();
		}


        /// <summary>
        /// Gets whether mouse is over the panel header.
        /// </summary>
        [Browsable(false)]
        public bool IsMouseOver
        {
            get { return m_MouseOver; }
        }

        /// <summary>
        /// Gets whether mouse is pressed over the panel header.
        /// </summary>
        [Browsable(false)]
        public bool IsMouseDown
        {
            get { return m_MouseDown; }
        }

		public override void Paint(ItemPaintArgs pa)
		{
            if (m_WordWrap)
            {
                pa.ButtonStringFormat = pa.ButtonStringFormat & ~(pa.ButtonStringFormat & eTextFormat.SingleLine);
                pa.ButtonStringFormat |= eTextFormat.WordBreak;
            }
			_ItemPaintArgs=pa;

            Rectangle r = m_Rect;
            Font font = this.GetFont();
            System.Drawing.Graphics g = pa.Graphics;
            if (m_NeedRecalcSize)
                RecalcSize();

            if (BarFunctions.IsOffice2007Style(this.EffectiveStyle))
            {
                SideBarPanelItemRendererEventArgs renderArgs = new SideBarPanelItemRendererEventArgs(this, pa.Graphics);
                renderArgs.ItemPaintArgs = pa;
                pa.Renderer.DrawSideBarPanelItem(renderArgs);

                r = m_PanelRect;
                r.Offset(m_Rect.X, m_Rect.Y);
            }
            else
            {
                if (this.IsThemed)
                {
                    PaintThemed(pa);
                    if (m_WordWrap)
                        pa.ButtonStringFormat = pa.ButtonStringFormat | eTextFormat.SingleLine;
                    _ItemPaintArgs = null;
                    return;
                }

                if (this.SuspendLayout)
                {
                    if (m_WordWrap)
                        pa.ButtonStringFormat = pa.ButtonStringFormat | eTextFormat.SingleLine;
                    _ItemPaintArgs = null;
                    return;
                }
                
                CompositeImage image = this.GetImage();
                Rectangle rText = Rectangle.Empty;

                if (m_Appearance == eSideBarAppearance.Traditional)
                {
                    Color backColor = SystemColors.Control;
                    Color foreColor = m_ForeColor;
                    System.Windows.Forms.Control ctrl = this.ContainerControl as System.Windows.Forms.Control;

                    if (!m_BackgroundStyle.BackColor1.IsEmpty || m_BackgroundStyle.BackgroundImage != null)
                    {
                        m_BackgroundStyle.Paint(g, m_Rect);
                        backColor = m_BackgroundStyle.BackColor1.Color;
                    }

                    if (ctrl != null)
                    {
                        backColor = ctrl.BackColor;
                        if (m_ForeColor == SystemColors.ControlText)
                            foreColor = ctrl.ForeColor;
                    }

                    if (m_MouseOver && !m_HotForeColor.IsEmpty)
                        foreColor = m_HotForeColor;

                    r = m_PanelRect;
                    r.Offset(m_Rect.X, m_Rect.Y);

                    if (m_Text != "")
                    {
                        if (m_MouseDown)
                            BarFunctions.DrawBorder3D(g, r.X, r.Y, r.Width, r.Height, System.Windows.Forms.Border3DStyle.Sunken, backColor);
                        else if (m_MouseOver)
                            BarFunctions.DrawBorder3D(g, r.X, r.Y, r.Width, r.Height, System.Windows.Forms.Border3DStyle.Raised, backColor);
                        else
                            BarFunctions.DrawBorder3D(g, r.X, r.Y, r.Width, r.Height, System.Windows.Forms.Border3DStyle.RaisedInner, backColor);
                    }

                    r.Inflate(-2, -2);

                    if (image != null)
                    {
                        r.X += 2;
                        r.Width -= 2;
                        image.DrawImage(g, new Rectangle(r.X, r.Y + (r.Height - image.Height) / 2, image.Width, image.Height));
                        r.X += (image.Width + 4);
                        r.Width -= (image.Width + 8);
                    }

                    if (m_Text != "")
                    {
                        TextDrawing.DrawString(g, m_Text, font, foreColor, r, this.GetStringFormat());
                        rText = r;
                    }
                }
                else
                {
                    int iSideWidth = FLAT_NOIMAGESIDEWIDTH;
                    if (image != null)
                        iSideWidth = image.Width + IMAGE_MARGIN * 2;

                    if (iSideWidth >= m_Rect.Width)
                        iSideWidth = 0;

                    r.Offset(iSideWidth, 0);
                    r.Width -= iSideWidth;
                    if (!m_BackgroundStyle.BackColor1.IsEmpty || m_BackgroundStyle.BackgroundImage != null)
                    {
                        m_BackgroundStyle.Paint(g, r);
                    }
                    // Paint Header
                    r = m_PanelRect;
                    r.Offset(m_Rect.Location);

                    if (iSideWidth > 0)
                    {
                        Rectangle sideRect = new Rectangle(r.X, r.Y, iSideWidth + 1, r.Height);
                        r.Offset(iSideWidth + 1, 0);
                        r.Width -= iSideWidth + 1;
                        if (m_HeaderSideStyle != null)
                        {
                            ItemStyle style = m_HeaderSideStyle.Clone() as ItemStyle;
                            if (m_MouseDown && !this.Expanded)
                                style.ApplyStyle(m_HeaderSideMouseDownStyle);
                            else if (m_MouseOver && !this.Expanded)
                                style.ApplyStyle(m_HeaderSideHotStyle);
                            style.Paint(g, sideRect);
                            if (image != null)
                                image.DrawImage(g, new Rectangle(sideRect.X + (sideRect.Width - image.Width) / 2, sideRect.Y + (sideRect.Height - image.Height) / 2, image.Width, image.Height));
                        }
                    }

                    if (m_HeaderStyle != null)
                    {
                        ItemStyle style = m_HeaderStyle.Clone() as ItemStyle;
                        if (m_MouseDown && !this.Expanded)
                            style.ApplyStyle(m_HeaderMouseDownStyle);
                        else if (m_MouseOver && !this.Expanded)
                            style.ApplyStyle(m_HeaderHotStyle);
                        else if (this.Expanded && !m_BackgroundStyle.ForeColor.IsEmpty)
                            style.ForeColor.Color = m_BackgroundStyle.ForeColor.Color;
                        rText = r;
                        rText.Offset(4, 0);
                        rText.Width -= 4;
                        if (this.Expanded)
                            style.PaintText(g, m_Text, rText, font);
                        else
                            style.Paint(g, r, m_Text, rText, font);
                    }
                }
                image = null;

                if (this.Focused)
                {
                    if (this.DesignMode)
                    {
                        Rectangle rFocus = m_PanelRect;
                        rFocus.Offset(m_Rect.X, m_Rect.Y);
                        rFocus.Inflate(-2, -2);
                        DesignTime.DrawDesignTimeSelection(g, rFocus, pa.Colors.ItemDesignTimeBorder);
                    }
                    else if (!rText.IsEmpty)
                    {
                        Rectangle rFocus = rText;
                        System.Windows.Forms.ControlPaint.DrawFocusRectangle(g, rFocus);
                    }
                }
            }

			if(this.Expanded && m_SubItems!=null && m_ControlHost==null)
			{
				r=new Rectangle(m_Rect.X,m_Rect.Y+r.Height+1,m_Rect.Width,m_Rect.Height-r.Height-1);
				g.SetClip(r);
				for(int i=m_TopItemIndex;i<m_SubItems.Count;i++)
				{
					BaseItem item=m_SubItems[i];
					if(!item.Displayed || !item.Visible)
						continue;
					if(item.BeginGroup && m_LayoutType==eSideBarLayoutType.Default)
					{
						Color divider=SystemColors.ControlDark;
						if(m_HeaderStyle!=null && !m_HeaderStyle.BorderColor.IsEmpty)
							divider=m_HeaderStyle.BorderColor.Color;
						using(Pen line=new Pen(divider,1))
							g.DrawLine(line,item.LeftInternal+2,item.TopInternal-2,item.DisplayRectangle.Right-4,item.TopInternal-2);
					}
					item.Paint(pa);
				}
				g.ResetClip();
			}

			if(m_ControlHost==null)
			{
				// Draw scroll up button
				if(m_TopItemIndex>0 && m_EnableScrollButtons)
				{
                    if (BarFunctions.IsOffice2007Style(this.EffectiveStyle))
                    {
                        Office2007SideBarColorTable ct = ((Office2007Renderer)pa.Renderer).ColorTable.SideBar;
                        GradientColorTable t = ct.SideBarPanelItemDefault;
                        if (t != null)
                        {
                            m_ScrollButtonRect[0] = new Rectangle(r.Right - 18, r.Top + 4, 16, 16);
                            using (Brush brush = DisplayHelp.CreateBrush(r, t))
                            {
                                g.FillRectangle(brush, m_ScrollButtonRect[0]);
                            }
                            DisplayHelp.DrawRectangle(g, ct.Border, m_ScrollButtonRect[0]);
                            PaintArrow(g, m_ScrollButtonRect[0], ct.SideBarPanelItemText, true);
                        }
                    }
					else if(m_Appearance==eSideBarAppearance.Traditional || m_HeaderStyle==null)
					{
						m_ScrollButtonRect[0]=new Rectangle(r.Right-18,r.Top+4,16,16);
						BarFunctions.DrawBorder3D(g,m_ScrollButtonRect[0],System.Windows.Forms.Border3DStyle.Raised);
						PaintArrow(g,m_ScrollButtonRect[0],SystemColors.ControlText,true);
					}
					else
					{
						m_ScrollButtonRect[0]=new Rectangle(r.Right-16,r.Top+2,14,14);
						ItemStyle style=m_HeaderStyle.Clone() as ItemStyle;
						style.BorderSide=eBorderSide.All;
						style.Paint(g,m_ScrollButtonRect[0]);
						//style.Dispose();
						PaintArrow(g,m_ScrollButtonRect[0],SystemColors.ControlText,true);
					}
				}
				else
					m_ScrollButtonRect[0]=Rectangle.Empty;

				// Draw scroll down button
				if(m_ScrollDownButton)
				{
                    if (BarFunctions.IsOffice2007Style(this.EffectiveStyle))
                    {
                        Office2007SideBarColorTable ct = ((Office2007Renderer)pa.Renderer).ColorTable.SideBar;
                        GradientColorTable t = ct.SideBarPanelItemDefault;
                        if (t != null)
                        {
                            m_ScrollButtonRect[1] = new Rectangle(m_Rect.Right - 18, m_Rect.Bottom - 18, 16, 16);
                            using (Brush brush = DisplayHelp.CreateBrush(r, t))
                            {
                                g.FillRectangle(brush, m_ScrollButtonRect[1]);
                            }
                            DisplayHelp.DrawRectangle(g, ct.Border, m_ScrollButtonRect[1]);
                            PaintArrow(g, m_ScrollButtonRect[1], ct.SideBarPanelItemText, false);
                        }
                    }
                    else if(m_Appearance==eSideBarAppearance.Traditional || m_HeaderStyle==null)
					{
						m_ScrollButtonRect[1]=new Rectangle(m_Rect.Right-18,m_Rect.Bottom-18,16,16);
						BarFunctions.DrawBorder3D(g,m_ScrollButtonRect[1],System.Windows.Forms.Border3DStyle.Raised);
						PaintArrow(g,m_ScrollButtonRect[1],SystemColors.ControlText,false);
					}
					else
					{
						m_ScrollButtonRect[1]=new Rectangle(m_Rect.Right-16,m_Rect.Bottom-16,14,14);
						ItemStyle style=m_HeaderStyle.Clone() as ItemStyle;
						style.BorderSide=eBorderSide.All;
						style.Paint(g,m_ScrollButtonRect[1]);
						//style.Dispose();
						PaintArrow(g,m_ScrollButtonRect[1],SystemColors.ControlText,false);
					}
				}
				else
					m_ScrollButtonRect[1]=Rectangle.Empty;
			}

			if(m_WordWrap)
				pa.ButtonStringFormat=pa.ButtonStringFormat | eTextFormat.SingleLine;

			PaintInfoText(pa);

			_ItemPaintArgs=null;
		}

		private void PaintThemed(ItemPaintArgs pa)
		{
			if(this.SuspendLayout)
				return;
			System.Drawing.Graphics g=pa.Graphics;
			if(m_NeedRecalcSize)
				RecalcSize();

			ThemeHeader theme=pa.ThemeHeader;
			Rectangle r=m_PanelRect;
			r.Offset(m_Rect.X,m_Rect.Y);
			Font font=this.GetFont();
			
//			if(this.Expanded)
//			{
//				SizeF size=SizeF.Empty;
//				if(m_Text!="")
//					size=g.MeasureString(m_Text,font,m_Rect.Width-m_Margin*2,this.GetStringFormat());
//				r.Height=(int)size.Height+m_Margin*2;
//			}

			ThemeHeaderParts part=ThemeHeaderParts.HeaderItemLeft;
			ThemeHeaderStates state=ThemeHeaderStates.ItemNormal;

			if(m_Text!="")
			{
				if(m_MouseDown)
				{
					state=ThemeHeaderStates.ItemPressed;
					part=ThemeHeaderParts.HeaderItem;
				}
				else if(m_MouseOver)
				{
					state=ThemeHeaderStates.ItemHot;
					part=ThemeHeaderParts.HeaderItem;
				}

				theme.DrawBackground(g,part,state,r);
			}

			CompositeImage image=this.GetImage();

			r.Inflate(-2,-2);

			if(image!=null)
			{
				r.X+=2;
				r.Width-=2;
				image.DrawImage(g,new Rectangle(r.X,r.Y+(r.Height-image.Height)/2,image.Width,image.Height));
				r.X+=(image.Width+4);
				r.Width-=(image.Width+4);
				r.Inflate(0,-2);
			}

			if(m_Text!="")
				theme.DrawText(g,m_Text,font,r,part,state,ThemeTextFormat.VCenter | ThemeTextFormat.Center | ThemeTextFormat.EndEllipsis,!m_Enabled);

			if(this.DesignMode && this.Focused)
			{
				Rectangle rFocus=m_PanelRect;
				rFocus.Offset(m_Rect.X,m_Rect.Y);
				rFocus.Inflate(-2,-2);
				DesignTime.DrawDesignTimeSelection(g,rFocus,pa.Colors.ItemDesignTimeBorder);
			}

			if(this.Expanded && m_SubItems!=null && m_ControlHost==null)
			{
				r=new Rectangle(m_Rect.X,m_Rect.Y+r.Height+1,m_Rect.Width,m_Rect.Height-r.Height-1);
				g.SetClip(r);

				IntPtr hdc=g.GetHdc();
				int rgn=0;
				try
				{
					rgn=NativeFunctions.CreateRectRgn(r.Left,r.Top,r.Right,r.Bottom);
					if(rgn!=0)
						NativeFunctions.SelectClipRgn(hdc,rgn);
				}
				finally
				{
					g.ReleaseHdc(hdc);
				}

				for(int i=m_TopItemIndex;i<m_SubItems.Count;i++)
				{
					BaseItem item=m_SubItems[i];
					if(!item.Displayed || !item.Visible)
						continue;
					item.Paint(pa);
				}
				g.ResetClip();
				if(rgn!=0)
				{
					hdc=g.GetHdc();
                    try
                    {
                        NativeFunctions.SelectClipRgn(hdc, 0);
                        WinApi.DeleteObject(rgn);
                    }
                    finally
                    {
                        g.ReleaseHdc(hdc);
                    }
				}
			}

			if(m_ControlHost==null)
			{
				// Draw scroll up button
				if(m_TopItemIndex>0)
				{
					ThemeScrollBar scroll=pa.ThemeScrollBar;
					m_ScrollButtonRect[0]=new Rectangle(r.Right-17,r.Top+4,17,16);
					scroll.DrawBackground(g,ThemeScrollBarParts.ArrowBtn, ThemeScrollBarStates.ArrowBtnUpNormal,m_ScrollButtonRect[0]);
				}
				else
					m_ScrollButtonRect[0]=Rectangle.Empty;

				// Draw scroll down button
				if(m_ScrollDownButton)
				{
					ThemeScrollBar scroll=pa.ThemeScrollBar;
					m_ScrollButtonRect[1]=new Rectangle(m_Rect.Right-17,m_Rect.Bottom-18,17,16);
					scroll.DrawBackground(g,ThemeScrollBarParts.ArrowBtn, ThemeScrollBarStates.ArrowBtnDownNormal,m_ScrollButtonRect[1]);
				}
				else
					m_ScrollButtonRect[1]=Rectangle.Empty;
			}

			PaintInfoText(pa);
		}

		private void PaintInfoText(ItemPaintArgs pa)
		{
			if(m_SubItems!=null && m_SubItems.Count==0 && this.DesignMode && this.Expanded)
			{
				Rectangle rInfoText=m_Rect;
				if(!m_PanelRect.IsEmpty)
				{
					rInfoText.Y+=m_PanelRect.Height;
					rInfoText.Height-=m_PanelRect.Height;
				}
				string info=INFO_EMPTYPANEL;
				rInfoText.Inflate(-1,-1);
				if(m_Appearance==eSideBarAppearance.Flat)
				{
					CompositeImage image=this.GetImage();
					int iSideWidth=FLAT_NOIMAGESIDEWIDTH;
					if(image!=null)
						iSideWidth=image.Width+IMAGE_MARGIN*2;
					if(iSideWidth>=m_Rect.Width)
						iSideWidth=0;
					rInfoText.Offset(iSideWidth,0);
					rInfoText.Width-=iSideWidth;
				}
                Font f1=this.GetFont();
                if(f1!=null)
                {
                    Font font = new Font(f1.FontFamily, f1.Size - 1);
                    try
                    {
                        eTextFormat format = eTextFormat.Default | eTextFormat.HorizontalCenter | eTextFormat.VerticalCenter | eTextFormat.WordBreak;
                        TextDrawing.DrawString(pa.Graphics, info, font, SystemColors.ControlDark, rInfoText, format);
                    }
                    finally
                    {
                        font.Dispose();
                    }
                }
			}
		}

		private void PaintThemeScrollState()
		{
            System.Windows.Forms.Control container=this.ContainerControl as System.Windows.Forms.Control;
			if(!IsHandleValid(container))
				return;
			Graphics g=BarFunctions.CreateGraphics(container);
			try
			{
				ThemeScrollBar scroll=null;
				bool bDisposeTheme=false;
				if(container is IThemeCache)
					scroll=((IThemeCache)container).ThemeScrollBar;
				else if(container is Bar)
					scroll=((Bar)container).ThemeScrollBar;
				else
				{
					bDisposeTheme=true;
					scroll=new ThemeScrollBar(container);
				}
				ThemeScrollBarParts part=ThemeScrollBarParts.ArrowBtn;
				ThemeScrollBarStates state=ThemeScrollBarStates.ArrowBtnUpNormal;
				if(m_TopItemIndex>0)
				{
					if(m_ScrollButtonHot==0)
						state=ThemeScrollBarStates.ArrowBtnUpHot;
					scroll.DrawBackground(g,part,state,m_ScrollButtonRect[0]);
				}
				if(m_ScrollDownButton)
				{
					state=ThemeScrollBarStates.ArrowBtnDownNormal;
					if(m_ScrollButtonHot==1)
						state=ThemeScrollBarStates.ArrowBtnDownHot;
                    scroll.DrawBackground(g,part,state,m_ScrollButtonRect[1]);
				}
				if(bDisposeTheme)
					scroll.Dispose();
			}
			finally
			{
				if(g!=null)
					g.Dispose();
			}
		}

		public override void SubItemSizeChanged(BaseItem objChildItem)
		{
			NeedRecalcSize=true;
		}

		private void PaintArrow(Graphics g, Rectangle rect, Color c, bool up)
		{
			Point[] p=new Point[3];
			if(up)
			{
				p[0].X=rect.Left+(rect.Width-9)/2;
				p[0].Y=rect.Top+rect.Height/2+1;
				p[1].X=p[0].X+8;
				p[1].Y=p[0].Y;
				p[2].X=p[0].X+4;
				p[2].Y=p[0].Y-5;
			}
			else
			{
				p[0].X=rect.Left+(rect.Width-7)/2;
				p[0].Y=rect.Top+(rect.Height-4)/2;
				p[1].X=p[0].X+7;
				p[1].Y=p[0].Y;
				p[2].X=p[0].X+3;
				p[2].Y=p[0].Y+4;
			}
			g.FillPolygon(new SolidBrush(c),p);
		}

		/// <summary>
		/// Returns the Font object to be used for drawing the item text.
		/// </summary>
		/// <returns>Font object.</returns>
		internal virtual Font GetFont()
		{
			System.Drawing.Font font=System.Windows.Forms.SystemInformation.MenuFont;
			System.Windows.Forms.Control ctrl=this.ContainerControl as System.Windows.Forms.Control;
			if(ctrl!=null)
				font=ctrl.Font;
			if(m_MouseOver && (m_HotFontBold || m_HotFontUnderline))
			{
				FontStyle style=FontStyle.Regular;
				if(m_HotFontBold)
					style=style | FontStyle.Bold;
				if(m_HotFontUnderline)
					style=style | FontStyle.Underline;
				try
				{
					font=new Font(font,style);
				}
				catch
				{
					font=System.Windows.Forms.SystemInformation.MenuFont.Clone() as Font;
				}
			}
            else if (m_FontBold)
            {
                FontStyle style = font.Style;
                style = style | FontStyle.Bold;
                try
                {
                    font = new Font(font, style);
                }
                catch
                {
                    font = System.Windows.Forms.SystemInformation.MenuFont.Clone() as Font;
                }
            }
			return font;
		}

        internal eTextFormat GetStringFormat()
		{
            eTextFormat format = eTextFormat.VerticalCenter |
                eTextFormat.EndEllipsis | eTextFormat.ExternalLeading;
            
            if (m_TextAlignment == eStyleTextAlignment.Center)
                format |= eTextFormat.HorizontalCenter;
            else if (m_TextAlignment == eStyleTextAlignment.Far)
                format |= eTextFormat.Right;

            if (!m_WordWrap)
                format |= eTextFormat.SingleLine;
            else
                format |= eTextFormat.WordBreak;
            return format;
		}

        private eStyleTextAlignment m_TextAlignment = eStyleTextAlignment.Center;
        /// <summary>
        /// Specifies panel title text alignment. Default value is Center.
        /// </summary>
        [DevCoSerialize(), Browsable(true), Category("Appearance"), DefaultValue(eStyleTextAlignment.Center), Description("Specifies alignment of the text.")]
        public eStyleTextAlignment TextAlignment
        {
            get { return m_TextAlignment; }
            set
            {
                m_TextAlignment = value;
                this.OnAppearanceChanged();
            }
        }

		/// <summary>
		/// Occurs when the mouse pointer is moved over the item. This is used by internal implementation only.
		/// </summary>
		[System.ComponentModel.Browsable(false),System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override void InternalMouseMove(System.Windows.Forms.MouseEventArgs objArg)
		{
			if(objArg.Button==System.Windows.Forms.MouseButtons.Left && (Math.Abs(objArg.X-m_MouseDownPt.X)>=4 || Math.Abs(objArg.Y-m_MouseDownPt.Y)>=4))
			{
				SideBar sidebar=this.ContainerControl as SideBar;
				if(sidebar!=null && sidebar.AllowUserCustomize && this.CanCustomize)
				{
					BaseItem active=m_HotSubItem;
					if(active!=null && active.CanCustomize && !active.SystemItem)
					{
						IOwner owner=this.GetOwner() as IOwner;
						if(owner!=null && owner.DragItem==null)
						{
							owner.StartItemDrag(active);
							return;
						}
					}
				}
			}

			bool bScrollPaint=false;
			if(m_ScrollButtonRect[0].Contains(objArg.X,objArg.Y) || m_ScrollButtonRect[1].Contains(objArg.X,objArg.Y))
			{
				if(m_HotSubItem!=null)
				{
					m_HotSubItem.InternalMouseLeave();
					m_HotSubItem=null;
				}
				if(m_ScrollButtonRect[0].Contains(objArg.X,objArg.Y))
					m_ScrollButtonHot=0;
				else
					m_ScrollButtonHot=1;
				if(this.IsThemed)
					PaintThemeScrollState();
				return;
			}
			else if(m_ScrollButtonHot>=0)
			{
				m_ScrollButtonHot=-1;
				bScrollPaint=true;
			}
			
			if(m_ControlHost!=null)
			{
				m_IsContainer=false;
				base.InternalMouseMove(objArg);
				m_IsContainer=true;
			}
			else
				base.InternalMouseMove(objArg);

			Rectangle r=m_PanelRect;
			r.Offset(m_Rect.Location);
			if(r.Contains(objArg.X,objArg.Y))
			{
				m_MouseOver=true;
				this.Refresh();
			}
			else if(m_MouseOver)
			{
				m_MouseOver=false;
				this.Refresh();
			}
			else if(bScrollPaint && this.IsThemed)
				PaintThemeScrollState();
		}

		[System.ComponentModel.Browsable(false),System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override void InternalMouseLeave()
		{
			base.InternalMouseLeave();
			m_MouseOver=false;
			m_MouseDown=false;
			if(m_ScrollButtonHot>=0)
				m_ScrollButtonHot=-1;
			this.Refresh();
		}

		/// <summary>
		/// Occurs when the mouse pointer is over the item and a mouse button is pressed. This is used by internal implementation only.
		/// </summary>
		[System.ComponentModel.Browsable(false),System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override void InternalMouseDown(System.Windows.Forms.MouseEventArgs objArg)
		{
			Rectangle r=m_PanelRect;
			r.Offset(m_Rect.Location);
			m_MouseDownPt=new System.Drawing.Point(objArg.X,objArg.Y);

			if(this.DesignMode)
			{
				if(this.ItemAtLocation(objArg.X,objArg.Y)==null && !r.Contains(objArg.X,objArg.Y) && this.Expanded)
				{
					IOwner owner=this.GetOwner() as IOwner;
					if(owner!=null)
						owner.SetFocusItem(null);
				}
				else if(!r.Contains(objArg.X,objArg.Y))
					base.InternalMouseDown(objArg);
				return;
			}

			if(objArg.Button==System.Windows.Forms.MouseButtons.Left && m_ScrollButtonRect[0].Contains(objArg.X,objArg.Y))
			{
				// Scroll up
				ScrollButtonClick(true);
				return;
			}
			else if(objArg.Button==System.Windows.Forms.MouseButtons.Left && m_ScrollButtonRect[1].Contains(objArg.X,objArg.Y))
			{
                ScrollButtonClick(false);
				return;
			}
			
			if(objArg.Button == System.Windows.Forms.MouseButtons.Left && r.Contains(objArg.X,objArg.Y))
			{
				m_MouseDown=true;
				this.Refresh();
			}

			if(m_IgnoreClickDateTime!=DateTime.MinValue)
			{
				TimeSpan elapsed=DateTime.Now.Subtract(m_IgnoreClickDateTime);
				if(elapsed.TotalMilliseconds<1000 && m_IgnoreScrollRect.Contains(objArg.X,objArg.Y))
					return;
				m_IgnoreClickDateTime=DateTime.MinValue;
			}

			base.InternalMouseDown(objArg);
		}

		internal void ScrollButtonClick(bool up)
		{
			if(up)
			{
				// Scroll up
				if(m_TopItemIndex>0)
				{
					m_IgnoreScrollRect=m_ScrollButtonRect[0];
					if(m_LayoutType==eSideBarLayoutType.MultiColumn)
					{
						// Look for line switch
						while(m_TopItemIndex>0)
						{
							m_TopItemIndex--;
							if(this.SubItems[m_TopItemIndex].Visible)
								break;
						}
					}
					else
						m_TopItemIndex--;
					NeedRecalcSize=true;
					this.Refresh();
					m_IgnoreNextClick=true;
				}
			}
			else
			{
				m_IgnoreScrollRect=m_ScrollButtonRect[1];
				// Scroll Down
				if(m_LayoutType==eSideBarLayoutType.MultiColumn)
				{
					// Look for line switch
					int iY=this.SubItems[m_TopItemIndex].TopInternal;
					while(m_TopItemIndex+1<this.SubItems.Count)
					{
						m_TopItemIndex++;
						if(this.SubItems[m_TopItemIndex].Visible && this.SubItems[m_TopItemIndex].TopInternal>iY)
							break;
					}
				}
				else
					m_TopItemIndex++;
				NeedRecalcSize=true;
				this.Refresh();
				m_IgnoreNextClick=true;
			}
			if(m_ControlHost!=null)
				m_ControlHost.Refresh();
		}

		[System.ComponentModel.Browsable(false),System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override void InternalMouseUp(System.Windows.Forms.MouseEventArgs objArg)
		{
			if(objArg.Button==System.Windows.Forms.MouseButtons.Left)
			{
				m_MouseDown=false;
				this.Refresh();
			}
			if(m_IgnoreNextClick)
			{
				m_IgnoreNextClick=false;
				m_IgnoreClickDateTime=DateTime.Now;
			}
			else if(m_IgnoreClickDateTime!=DateTime.MinValue)
				return;

			Rectangle r=m_PanelRect;
			r.Offset(m_Rect.Location);

			base.InternalMouseUp(objArg);

			if((this.GetOwner() as IOwner)!=null && ((IOwner)this.GetOwner()).DragInProgress)
				return;
			
			if(!this.Expanded && objArg.Button==System.Windows.Forms.MouseButtons.Left && r.Contains(objArg.X,objArg.Y))
				this.Expanded=true;
		}

		/// <summary>
		/// Occurs when the item is clicked. This is used by internal implementation only.
		/// </summary>
		[System.ComponentModel.Browsable(false),System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override void InternalClick(System.Windows.Forms.MouseButtons mb, System.Drawing.Point mpos)
		{
			base.InternalClick(mb,mpos);
//			if((this.GetOwner() as IOwner)!=null && ((IOwner)this.GetOwner()).DragInProgress)
//				return;
//			if(!this.Expanded)
//				this.Expanded=true;
		}

		protected internal override void OnExpandChange()
		{
			base.OnExpandChange();
			m_TopItemIndex=0;
		}

		/// <summary>
		/// Gets or sets the layout type for the items. Default layout orders items in a single column. Multi-column layout will order
		/// items in multiple colums based on the width of the control.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),DefaultValue(eSideBarLayoutType.Default),Category("Appearance"),Description("Gets or sets the layout type for the items.")]
		public eSideBarLayoutType LayoutType
		{
			get {return m_LayoutType;}
			set
			{
				if(m_LayoutType!=value)
				{
					m_LayoutType=value;
					NeedRecalcSize=true;
					if(this.DesignMode)
						this.Refresh();
				}
			}
		}

		/// <summary>
		/// Gets or sets the text associated with this item.
		/// </summary>
		[System.ComponentModel.Browsable(true),DevCoBrowsable(true),System.ComponentModel.Category("Appearance"),System.ComponentModel.Description("The text contained in the item."),System.ComponentModel.Localizable(true)]
		public override string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				base.Text=value;
				if(m_Text=="")
					m_Margin=0;
				else
					m_Margin=3;
				this.NeedRecalcSize=true;
				if(this.Parent!=null)
					this.Parent.NeedRecalcSize=true;
				if(this.DesignMode)
					this.Refresh();
				OnAppearanceChanged();
			}
		}

		/// <summary>
		/// Gets or sets the item background style.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),Category("Style"),Description("Gets or sets group background style."),DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ItemStyle BackgroundStyle
		{
			get {return m_BackgroundStyle;}
		}
		public void ResetBackgroundStyle()
		{
			SetBackgroundStyle(new ItemStyle());
		}
		internal void SetBackgroundStyle(ItemStyle newStyle)
		{
			if(m_BackgroundStyle!=null)
				m_BackgroundStyle.VisualPropertyChanged-=new EventHandler(this.VisualPropertyChanged);
			m_BackgroundStyle=newStyle;
			if(m_BackgroundStyle!=null)
				m_BackgroundStyle.VisualPropertyChanged+=new EventHandler(this.VisualPropertyChanged);
		}

		internal bool HasFlatStyle
		{
			get
			{
				if(m_HeaderStyle!=null)
					return true;
				return false;
			}
		}
		/// <summary>
		/// Gets or sets the item header style. Applies only when SideBar.Appearance is set to Flat.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),NotifyParentPropertyAttribute(true),Category("Style"),Description("Gets or sets the item header style. Applies only when SideBar.Appearance is set to Flat."),DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ItemStyle HeaderStyle
		{
			get
			{
				if(m_HeaderStyle==null)
				{
					SetHeaderStyle(new ItemStyle());
				}
				return m_HeaderStyle;
			}
		}
		public void ResetHeaderStyle()
		{
			SetHeaderStyle(null);
		}
		internal void SetHeaderStyle(ItemStyle style)
		{
			if(m_HeaderStyle!=null)
				m_HeaderStyle.VisualPropertyChanged-=new EventHandler(this.VisualPropertyChanged);
			m_HeaderStyle=style;
			if(m_HeaderStyle!=null)
				m_HeaderStyle.VisualPropertyChanged+=new EventHandler(this.VisualPropertyChanged);
		}

		/// <summary>
		/// Gets or sets the item header style when mouse is over the header. Applies only when SideBar.Appearance is set to Flat.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),NotifyParentPropertyAttribute(true),Category("Style"),Description("Gets or sets the item header style when mouse is over the header. Applies only when SideBar.Appearance is set to Flat."),DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ItemStyle HeaderHotStyle
		{
			get
			{
				if(m_HeaderHotStyle==null)
				{
					SetHeaderHotStyle(new ItemStyle());
				}
				return m_HeaderHotStyle;
			}
		}
		public void ResetHeaderHotStyle()
		{
			SetHeaderHotStyle(null);
		}
		internal void SetHeaderHotStyle(ItemStyle style)
		{
			if(m_HeaderHotStyle!=null)
				m_HeaderHotStyle.VisualPropertyChanged-=new EventHandler(this.VisualPropertyChanged);
			m_HeaderHotStyle=style;
			if(m_HeaderHotStyle!=null)
				m_HeaderHotStyle.VisualPropertyChanged+=new EventHandler(this.VisualPropertyChanged);
		}

		/// <summary>
		/// Gets or sets the item header style when left mouse button is pressed on header. Applies only when SideBar.Appearance is set to Flat.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),NotifyParentPropertyAttribute(true),Category("Style"),Description("Gets or sets the item header style when left mouse button is pressed on header. Applies only when SideBar.Appearance is set to Flat."),DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ItemStyle HeaderMouseDownStyle
		{
			get
			{
				if(m_HeaderMouseDownStyle==null)
				{
					SetHeaderMouseDownStyle(new ItemStyle());
				}
				return m_HeaderMouseDownStyle;
			}
		}
		public void ResetHeaderMouseDownStyle()
		{
			SetHeaderMouseDownStyle(null);
		}
		internal void SetHeaderMouseDownStyle(ItemStyle style)
		{
			if(m_HeaderMouseDownStyle!=null)
				m_HeaderMouseDownStyle.VisualPropertyChanged-=new EventHandler(this.VisualPropertyChanged);
			m_HeaderMouseDownStyle=style;
			if(m_HeaderMouseDownStyle!=null)
				m_HeaderMouseDownStyle.VisualPropertyChanged+=new EventHandler(this.VisualPropertyChanged);
		}
		
		/// <summary>
		/// Gets or sets the item header side style. Applies only when SideBar.Appearance is set to Flat.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),NotifyParentPropertyAttribute(true),Category("Style"),Description("Gets or sets the item header side style. Applies only when SideBar.Appearance is set to Flat."),DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ItemStyle HeaderSideStyle
		{
			get
			{
				if(m_HeaderSideStyle==null)
				{
					SetHeaderSideStyle(new ItemStyle());
				}
				return m_HeaderSideStyle;
			}
		}
		public void ResetHeaderSideStyle()
		{
			SetHeaderSideStyle(null);
		}
		internal void SetHeaderSideStyle(ItemStyle style)
		{
			if(m_HeaderSideStyle!=null)
				m_HeaderSideStyle.VisualPropertyChanged-=new EventHandler(this.VisualPropertyChanged);
			m_HeaderSideStyle=style;
			if(m_HeaderSideStyle!=null)
				m_HeaderSideStyle.VisualPropertyChanged+=new EventHandler(this.VisualPropertyChanged);
		}

		/// <summary>
		/// Gets or sets the item header side style when mouse is over the header. Applies only when SideBar.Appearance is set to Flat.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),NotifyParentPropertyAttribute(true),Category("Style"),Description("Gets or sets the item header side style when mouse is over the header. Applies only when SideBar.Appearance is set to Flat."),DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ItemStyle HeaderSideHotStyle
		{
			get
			{
				if(m_HeaderSideHotStyle==null)
				{
					SetHeaderSideHotStyle(new ItemStyle());
				}
				return m_HeaderSideHotStyle;
			}
		}
		public void ResetHeaderSideHotStyle()
		{
			SetHeaderSideHotStyle(null);
		}
		internal void SetHeaderSideHotStyle(ItemStyle style)
		{
			if(m_HeaderSideHotStyle!=null)
				m_HeaderSideHotStyle.VisualPropertyChanged-=new EventHandler(this.VisualPropertyChanged);
			m_HeaderSideHotStyle=style;
			if(m_HeaderSideHotStyle!=null)
				m_HeaderSideHotStyle.VisualPropertyChanged+=new EventHandler(this.VisualPropertyChanged);
		}

		/// <summary>
		/// Gets or sets the item header side style when left mouse button is pressed on header. Applies only when SideBar.Appearance is set to Flat.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),NotifyParentPropertyAttribute(true),Category("Style"),Description("Gets or sets the item header side style when left mouse button is pressed on header. Applies only when SideBar.Appearance is set to Flat."),DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ItemStyle HeaderSideMouseDownStyle
		{
			get
			{
				if(m_HeaderSideMouseDownStyle==null)
				{
					SetHeaderSideMouseDownStyle(new ItemStyle());
				}
				return m_HeaderSideMouseDownStyle;
			}
		}
		public void ResetHeaderSideMouseDownStyle()
		{
			SetHeaderSideMouseDownStyle(null);
		}
		internal void SetHeaderSideMouseDownStyle(ItemStyle style)
		{
			if(m_HeaderSideMouseDownStyle!=null)
				m_HeaderSideMouseDownStyle.VisualPropertyChanged-=new EventHandler(this.VisualPropertyChanged);
			m_HeaderSideMouseDownStyle=style;
			if(m_HeaderSideMouseDownStyle!=null)
				m_HeaderSideMouseDownStyle.VisualPropertyChanged+=new EventHandler(this.VisualPropertyChanged);
		}

        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public eSideBarAppearance Appearance
		{
			get {return m_Appearance;}
			set
			{
				if(m_Appearance!=value)
				{
					m_Appearance=value;
					NeedRecalcSize=true;
				}
			}
		}

        /// <summary>
        /// Gets or sets whether the font used to draw the item text is bold.
        /// </summary>
        [System.ComponentModel.Browsable(true), DevCoBrowsable(true), System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("Specifies that text font is bold when text is rendered."), System.ComponentModel.DefaultValue(false)]
        public bool FontBold
        {
            get
            {
                return m_FontBold;
            }
            set
            {
                if (m_FontBold != value)
                {
                    m_FontBold = value;
                    if (ShouldSyncProperties)
                        BarFunctions.SyncProperty(this, "FontBold");
                    if (this.Displayed)
                        this.Refresh();
                }
                OnAppearanceChanged();
            }
        }

		/// <summary>
		/// Gets or sets whether the font used to draw the item text is bold when mouse is over the item.
		/// </summary>
		[System.ComponentModel.Browsable(true),DevCoBrowsable(true),System.ComponentModel.Category("Appearance"),System.ComponentModel.Description("Specifies that text font is bold when mouse is over the item."),System.ComponentModel.DefaultValue(false)]
		public bool HotFontBold
		{
			get
			{
				return m_HotFontBold;
			}
			set
			{
				if(m_HotFontBold!=value)
				{
					m_HotFontBold=value;
                    if(ShouldSyncProperties)
                        BarFunctions.SyncProperty(this, "HotFontBold");
					if(this.Displayed && m_MouseOver)
						this.Refresh();
				}
				OnAppearanceChanged();
			}
		}

		/// <summary>
		/// Gets or sets whether the font used to draw the item text is underlined when mouse is over the item.
		/// </summary>
		[System.ComponentModel.Browsable(true),DevCoBrowsable(true),System.ComponentModel.Category("Appearance"),System.ComponentModel.Description("Specifies that text font is underlined when mouse is over the item."), System.ComponentModel.DefaultValue(false)]
		public bool HotFontUnderline
		{
			get
			{
				return m_HotFontUnderline;
			}
			set
			{
				if(m_HotFontUnderline!=value)
				{
					m_HotFontUnderline=value;
                    if(ShouldSyncProperties)
                        BarFunctions.SyncProperty(this, "HotFontUnderline");
					if(this.Displayed && m_MouseOver)
						this.Refresh();
				}
				OnAppearanceChanged();
			}
		}

		/// <summary>
		/// Gets or sets the text color of the button when mouse is over the item.
		/// </summary>
		[System.ComponentModel.Browsable(true),DevCoBrowsable(true),System.ComponentModel.Category("Appearance"),System.ComponentModel.Description("The foreground color used to display text when mouse is over the item.")]
		public Color HotForeColor
		{
			get
			{
				return m_HotForeColor;
			}
			set
			{
				if(m_HotForeColor!=value)
				{
					m_HotForeColor=value;
                    if(ShouldSyncProperties)
                        BarFunctions.SyncProperty(this, "HotForeColor");
					if(this.Displayed && m_MouseOver)
						this.Refresh();
				}
				OnAppearanceChanged();
			}
		}

		public bool ShouldSerializeHotForeColor()
		{
			if(m_HotForeColor!=Color.Empty)
				return true;
			return false;
		}

		/// <summary>
		/// Gets or sets the text color of the button.
		/// </summary>
		[System.ComponentModel.Browsable(true),DevCoBrowsable(true),System.ComponentModel.Category("Appearance"),System.ComponentModel.Description("The foreground color used to display text.")]
		public Color ForeColor
		{
			get
			{
				return m_ForeColor;
			}
			set
			{
				if(m_ForeColor!=value)
				{
					m_ForeColor=value;
                    if(ShouldSyncProperties)
                        BarFunctions.SyncProperty(this, "ForeColor");
					if(this.Displayed)
						this.Refresh();
				}
				OnAppearanceChanged();
			}
		}

		public bool ShouldSerializeForeColor()
		{
			if(m_ForeColor!=SystemColors.ControlText)
				return true;
			return false;
		}

		/// <summary>
		/// Overloaded. Serializes the item and all sub-items into the XmlElement.
		/// </summary>
		/// <param name="ThisItem">XmlElement to serialize the item to.</param>
		protected internal override void Serialize(ItemSerializationContext context)
		{
			base.Serialize(context);
            System.Xml.XmlElement ThisItem = context.ItemXmlElement;
			if(m_ForeColor!=SystemColors.ControlText)
				ThisItem.SetAttribute("forecolor",BarFunctions.ColorToString(m_ForeColor));

//			if(m_BackColor!=SystemColors.Control)
//				ThisItem.SetAttribute("backcolor",BarFunctions.ColorToString(m_BackColor));

			if(m_HotFontBold)
				ThisItem.SetAttribute("hotfb",System.Xml.XmlConvert.ToString(m_HotFontBold));

			if(m_HotFontUnderline)
				ThisItem.SetAttribute("hotfu",System.Xml.XmlConvert.ToString(m_HotFontUnderline));

			if(!m_HotForeColor.IsEmpty)
				ThisItem.SetAttribute("hotclr",BarFunctions.ColorToString(m_HotForeColor));

			if(m_ItemImageSize!=eBarImageSize.Default)
				ThisItem.SetAttribute("itemimagesize",System.Xml.XmlConvert.ToString((int)m_ItemImageSize));

			if(!m_EnableScrollButtons)
				ThisItem.SetAttribute("enablescrollbuttons",System.Xml.XmlConvert.ToString(m_EnableScrollButtons));

			// Serialize Images
			System.Xml.XmlElement xmlElem=null, xmlElem2=null;
			if(m_Image!=null || m_ImageIndex>=0 || m_HoverImage!=null || m_HoverImageIndex>=0 || m_PressedImage!=null || m_PressedImageIndex>=0 || m_Icon!=null)
			{
				xmlElem=ThisItem.OwnerDocument.CreateElement("images");
				ThisItem.AppendChild(xmlElem);

				if(m_ImageIndex>=0)
					xmlElem.SetAttribute("imageindex",System.Xml.XmlConvert.ToString(m_ImageIndex));
				if(m_HoverImageIndex>=0)
					xmlElem.SetAttribute("hoverimageindex",System.Xml.XmlConvert.ToString(m_HoverImageIndex));
				if(m_PressedImageIndex>=0)
					xmlElem.SetAttribute("pressedimageindex",System.Xml.XmlConvert.ToString(m_PressedImageIndex));

				if(m_Image!=null)
				{
					xmlElem2=ThisItem.OwnerDocument.CreateElement("image");
					xmlElem2.SetAttribute("type","default");
					xmlElem.AppendChild(xmlElem2);
					BarFunctions.SerializeImage(m_Image,xmlElem2);
				}
				if(m_HoverImage!=null)
				{
					xmlElem2=ThisItem.OwnerDocument.CreateElement("image");
					xmlElem2.SetAttribute("type","hover");
					xmlElem.AppendChild(xmlElem2);
					BarFunctions.SerializeImage(m_HoverImage,xmlElem2);
				}
				if(m_PressedImage!=null)
				{
					xmlElem2=ThisItem.OwnerDocument.CreateElement("image");
					xmlElem2.SetAttribute("type","pressed");
					xmlElem.AppendChild(xmlElem2);
					BarFunctions.SerializeImage(m_PressedImage,xmlElem2);
				}
				if(m_Icon!=null)
				{
					xmlElem2=ThisItem.OwnerDocument.CreateElement("image");
					xmlElem2.SetAttribute("type","icon");
					xmlElem.AppendChild(xmlElem2);
					BarFunctions.SerializeIcon(m_Icon,xmlElem2);
				}
			}
		}

		/// <summary>
		/// Overloaded. Deserializes the Item from the XmlElement.
		/// </summary>
		/// <param name="ItemXmlSource">Source XmlElement.</param>
		public override void Deserialize(ItemSerializationContext context)
		{
			base.Deserialize(context);

            System.Xml.XmlElement ItemXmlSource = context.ItemXmlElement;

			if(ItemXmlSource.HasAttribute("forecolor"))
				m_ForeColor=BarFunctions.ColorFromString(ItemXmlSource.GetAttribute("forecolor"));
			else
				m_ForeColor=SystemColors.ControlText;

//			if(ItemXmlSource.HasAttribute("backcolor"))
//				m_BackColor=BarFunctions.ColorFromString(ItemXmlSource.GetAttribute("backcolor"));
//			else
//				m_BackColor=SystemColors.Control;

			if(ItemXmlSource.HasAttribute("hotclr"))
				m_HotForeColor=BarFunctions.ColorFromString(ItemXmlSource.GetAttribute("hotclr"));
			else
				m_HotForeColor=Color.Empty;

			if(ItemXmlSource.HasAttribute("hotfb"))
				m_HotFontBold=System.Xml.XmlConvert.ToBoolean(ItemXmlSource.GetAttribute("hotfb"));
			else
				m_HotFontBold=false;
			if(ItemXmlSource.HasAttribute("hotfu"))
				m_HotFontUnderline=System.Xml.XmlConvert.ToBoolean(ItemXmlSource.GetAttribute("hotfu"));
			else
				m_HotFontUnderline=false;

			if(ItemXmlSource.HasAttribute("itemimagesize"))
				m_ItemImageSize=(eBarImageSize)System.Xml.XmlConvert.ToInt32(ItemXmlSource.GetAttribute("itemimagesize"));
			else
				m_ItemImageSize=eBarImageSize.Default;

			if(ItemXmlSource.HasAttribute("enablescrollbuttons"))
				m_EnableScrollButtons=System.Xml.XmlConvert.ToBoolean(ItemXmlSource.GetAttribute("enablescrollbuttons"));
			else
				m_EnableScrollButtons=true;

			// Load Images
			foreach(System.Xml.XmlElement xmlElem in ItemXmlSource.ChildNodes)
			{
				if(xmlElem.Name=="images")
				{
					if(xmlElem.HasAttribute("imageindex"))
						m_ImageIndex=System.Xml.XmlConvert.ToInt32(xmlElem.GetAttribute("imageindex"));
					if(xmlElem.HasAttribute("hoverimageindex"))
						m_HoverImageIndex=System.Xml.XmlConvert.ToInt32(xmlElem.GetAttribute("hoverimageindex"));
					if(xmlElem.HasAttribute("pressedimageindex"))
						m_PressedImageIndex=System.Xml.XmlConvert.ToInt32(xmlElem.GetAttribute("pressedimageindex"));

					foreach(System.Xml.XmlElement xmlElem2 in xmlElem.ChildNodes)
					{
						switch(xmlElem2.GetAttribute("type"))
						{
							case "default":
							{
								m_Image=BarFunctions.DeserializeImage(xmlElem2);
								m_ImageIndex=-1;
								break;
							}
							case "icon":
							{
								m_Icon=BarFunctions.DeserializeIcon(xmlElem2);
								m_ImageIndex=-1;
								break;
							}
							case "hover":
							{
								m_HoverImage=BarFunctions.DeserializeImage(xmlElem2);
								m_HoverImageIndex=-1;
								break;
							}
							case "pressed":
							{
								m_PressedImage=BarFunctions.DeserializeImage(xmlElem2);
								m_PressedImageIndex=-1;
								break;
							}
						}
					}
					break;
				}
			}
			this.RefreshImageSize();
			NeedRecalcSize=true;
		}

		protected internal override void OnItemAdded(BaseItem item)
		{
			NeedRecalcSize=true;
			if(item.IsWindowed && !this.DesignMode)
				CreateControlPanelHost();
			base.OnItemAdded(item);
			if(!this.Expanded)
			{
				if(!item.Displayed)
					item.Displayed=true;
				item.Displayed=false;
			}
			if(m_ControlHost!=null)
				item.ContainerControl=m_ControlHost;
			if(item is ButtonItem)
			{
				((ButtonItem)item)._FitContainer=m_WordWrap;
				//((ButtonItem)item).PopupSide=ePopupSide.Left;
			}
		}

		protected internal override void OnBeforeItemRemoved(BaseItem item)
		{
			if(item is ButtonItem)
				((ButtonItem)item)._FitContainer=false;
			base.OnBeforeItemRemoved(item);
		}

		private void CreateControlPanelHost()
		{
			System.Windows.Forms.Control ctrl=this.ContainerControl as System.Windows.Forms.Control;
            if (ctrl == null || m_ControlHost != null)
                return;
            m_ControlHost=new SideBarPanelControlHost(this);
			m_ControlHost.Visible=false;
			ctrl.Controls.Add(m_ControlHost);
		}

		[System.ComponentModel.Browsable(false),DevCoBrowsable(false),System.ComponentModel.DefaultValue(eItemAlignment.Near),System.ComponentModel.Category("Appearance"),System.ComponentModel.Description("Determines alignment of the item inside the container.")]
		public override eItemAlignment ItemAlignment
		{
			get{return base.ItemAlignment;}
			set{base.ItemAlignment=value;}
		}
		[System.ComponentModel.Browsable(false),DevCoBrowsable(false),System.ComponentModel.DefaultValue(false),System.ComponentModel.Category("Appearance"),System.ComponentModel.Description("Indicates whether item will stretch to consume empty space. Items on stretchable, no-wrap Bars only.")]
		public override bool Stretch
		{
			get {return base.Stretch;}
			set {base.Stretch=value;}
		}
//		[System.ComponentModel.Browsable(false),System.ComponentModel.DefaultValue(eDotNetBarStyle.OfficeXP),System.ComponentModel.Category("Appearance"),System.ComponentModel.Description("Determines the display of the item when shown.")]
//		public override eDotNetBarStyle Style
//		{
//			get {return base.Style;}
//			set {base.Style=value;}
//		}
		[System.ComponentModel.Browsable(false),DevCoBrowsable(false),System.ComponentModel.Category("Behavior"),System.ComponentModel.DefaultValue(true),System.ComponentModel.Description("Indicates whether the item will auto-collapse (fold) when clicked.")]
		public override bool AutoCollapseOnClick
		{
			get {return base.AutoCollapseOnClick;}
			set {base.AutoCollapseOnClick=value;}
		}
		[System.ComponentModel.Browsable(false),DevCoBrowsable(false),System.ComponentModel.DefaultValue(true),System.ComponentModel.Category("Behavior"),System.ComponentModel.Description("Determines whether sub-items are displayed.")]
		public override bool ShowSubItems
		{
			get {return base.ShowSubItems;}
			set {base.ShowSubItems=value;}
		}
		[System.ComponentModel.Browsable(false),DevCoBrowsable(false),System.ComponentModel.DefaultValue(""),System.ComponentModel.Category("Design"),System.ComponentModel.Description("Indicates item category used to group similar items at design-time.")]
		public override string Category
		{
			get {return base.Category;}
			set {base.Category=value;}
		}
		[System.ComponentModel.Browsable(false),DevCoBrowsable(false),System.ComponentModel.DefaultValue(""),System.ComponentModel.Category("Design"),System.ComponentModel.Description("Indicates description of the item that is displayed during design.")]
		public override string Description
		{
			get {return base.Description;}
			set {base.Description=value;}
		}
		[System.ComponentModel.Browsable(false),DevCoBrowsable(false),System.ComponentModel.DefaultValue(false),System.ComponentModel.Category("Appearance"),System.ComponentModel.Description("Indicates whether this item is beginning of the group.")]
		public override bool BeginGroup
		{
			get {return base.BeginGroup;}
			set {base.BeginGroup=value;}
		}

        [System.ComponentModel.Browsable(true), DevCoBrowsable(false), System.ComponentModel.Editor("DevComponents.DotNetBar.Design.SideBarPanelItemEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(System.Drawing.Design.UITypeEditor)), System.ComponentModel.Category("Data"), System.ComponentModel.Description("Collection of sub items."), System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Content)]
		public override SubItemsCollection SubItems
		{
			get
			{
				return base.SubItems;
			}
		}

		/// <summary>
		/// Gets/Sets the Image size for all sub-items on the Bar.
		/// </summary>
		[System.ComponentModel.Browsable(true),DevCoBrowsable(true),System.ComponentModel.DefaultValue(eBarImageSize.Default),System.ComponentModel.Category("Behavior"),System.ComponentModel.Description("Specifies the Image size that will be used by items on this bar.")]
		public eBarImageSize ItemImageSize
		{
			get
			{
				return m_ItemImageSize;
			}
			set
			{
//				if(m_ItemImageSize==value)
//					return;
				m_ItemImageSize=value;
				
				eImagePosition pos=eImagePosition.Left;
				if(m_ItemImageSize!=eBarImageSize.Default)
					pos=eImagePosition.Top;

				foreach(BaseItem item in this.SubItems)
				{
					ButtonItem button=item as ButtonItem;
					if(button!=null)
						button.ImagePosition=pos;
				}
				m_TopItemIndex=0;
				this.RefreshImageSize();
				this.RecalcSize();
				this.Refresh();
			}
		}


		/// <summary>
		/// Gets the rectangle of the panel item Button.
		/// </summary>
		[System.ComponentModel.Browsable(false),System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		public Rectangle PanelRect
		{
			get
			{
				return m_PanelRect;
			}
		}

		/// <summary>
		/// Gets or sets the index of the first visible item on the panel.
		/// </summary>
		[Browsable(false),DevCoBrowsable(false),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int TopItemIndex
		{
			get
			{
				return m_TopItemIndex;
			}
			set
			{
				m_TopItemIndex=value;
				NeedRecalcSize=true;
				this.Refresh();
			}
		}

		/// <summary>
		/// Gets or sets a value that determines whether text is displayed in multiple lines or one long line.
		/// This setting applies to the buttons inside Panel as well.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),DefaultValue(false),Description("Indicates a value that determines whether text is displayed in multiple lines or one long line."),Category("Layout")]
		public bool WordWrap
		{
			get {return m_WordWrap;}
			set
			{
				m_WordWrap=value;
				foreach(BaseItem item in this.SubItems)
				{
					if(item is ButtonItem)
						((ButtonItem)item)._FitContainer=m_WordWrap;
				}
				NeedRecalcSize=true;
				if(this.DesignMode)
				{
					SideBar sidebar=this.ContainerControl as SideBar;
					if(sidebar!=null)
						sidebar.RecalcLayout();
				}
				OnAppearanceChanged();
			}
		}

		/// <summary>
		/// Shows tooltip for this item.
		/// </summary>
		public override void ShowToolTip()
		{
			if(this.DesignMode)
				return;

			if(this.Visible && this.Displayed)
			{
				IOwner owner=this.GetOwner() as IOwner;
				if(owner!=null && !owner.ShowToolTips)
					return;
				System.Windows.Forms.Control ctrl=this.ContainerControl as System.Windows.Forms.Control;
				if(ctrl==null)
					return;
				if(!m_PanelRect.Contains(ctrl.PointToClient(System.Windows.Forms.Control.MousePosition)))
					return;

				OnTooltip(true);
				if(m_Tooltip!="")
				{
					if(m_ToolTipWnd==null)
						m_ToolTipWnd=new ToolTip();
					m_ToolTipWnd.Text=m_Tooltip;
					if(owner!=null && owner.ShowShortcutKeysInToolTips && this.Shortcuts.Count>0)
						m_ToolTipWnd.Text+=(" ("+this.ShortcutString+")");
					IOwnerItemEvents ownerEvents=this.GetIOwnerItemEvents();
					if(ownerEvents!=null)
						ownerEvents.InvokeToolTipShowing(this,new EventArgs());
					m_ToolTipWnd.ShowToolTip();
				}
			}
		}

		/// <summary>
		///     Gets or sets whether scroll buttons are displayed when content of the panel exceeds it's height.
		/// </summary>
		[System.ComponentModel.Browsable(true),DevCoBrowsable(true),System.ComponentModel.DefaultValue(true),System.ComponentModel.Category("Behavior"),System.ComponentModel.Description("Specifies whether scroll buttons are displayed when content of the panel exceeds it's height.")]
		public bool EnableScrollButtons
		{
			get {return m_EnableScrollButtons;}
			set
			{
				m_EnableScrollButtons=value;
				if(!m_EnableScrollButtons)
				{
					NeedRecalcSize=true;
					if(this.DesignMode)
						this.Refresh();
				}
			}
		}

		internal bool ScrollDownButton
		{
			get{return m_ScrollDownButton;}
		}

		internal bool UseThemes
		{
			get {return this.IsThemed;}
		}

		/// <summary>
		/// Forces the repaint the item.
		/// </summary>
		public override void Refresh()
		{
			if(this.SuspendLayout)
				return;

			if(m_ControlHost==null || !this.Expanded)
				base.Refresh();
			else
			{
				if((this.Visible || this.IsOnCustomizeMenu) && this.Displayed)
				{
					System.Windows.Forms.Control objCtrl=this.ContainerControl as System.Windows.Forms.Control;
					if(objCtrl!=null && IsHandleValid(objCtrl) && !(objCtrl is ItemsListBox))
					{
						if(m_NeedRecalcSize)
						{
							RecalcSize();
							if(m_Parent!=null)
								m_Parent.SubItemSizeChanged(this);
						}
						Rectangle r=m_PanelRect;
						r.Inflate(4,4);
						objCtrl.Invalidate(m_Rect,false);
					}
				}
			}
		}

		// Property Editor support for ImageIndex selection
		[System.ComponentModel.Browsable(false),System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public System.Windows.Forms.ImageList ImageList
		{
			get
			{
				IOwner owner=this.GetOwner() as IOwner;
				if(owner!=null)
					return owner.Images;
				return null;
			}
		}

		/// <summary>
		/// Specifies the Button icon. Icons support multiple image sizes and alpha blending.
		/// </summary>
		[System.ComponentModel.Browsable(true),DevCoBrowsable(true),System.ComponentModel.Category("Appearance"),System.ComponentModel.Description("Specifies the Button icon. Icons support multiple image sizes and alpha blending."),System.ComponentModel.DefaultValue(null)]
		public System.Drawing.Icon Icon
		{
			get
			{
				return m_Icon;
			}
			set
			{
				NeedRecalcSize=true;
				m_Icon=value;
				this.OnImageChanged();
				this.Refresh();
				OnAppearanceChanged();
			}
		}

		/// <summary>
		/// Specifies the Button image.
        /// </summary>
        [System.ComponentModel.Browsable(true),DevCoBrowsable(true),System.ComponentModel.Category("Appearance"),System.ComponentModel.Description("The image that will be displayed on the face of the item."),System.ComponentModel.DefaultValue(null)]
		public System.Drawing.Image Image
		{
			get
			{
				return m_Image;
			}
			set
			{
				NeedRecalcSize=true;
				m_Image=value;
				this.OnImageChanged();
				this.Refresh();
				OnAppearanceChanged();
			}
		}

		/// <summary>
		/// Specifies the index of the image for the button if ImageList is used.
		/// </summary>
		[System.ComponentModel.Browsable(true),DevCoBrowsable(true),System.ComponentModel.Category("Appearance"),System.ComponentModel.Description("The image list image index of the image that will be displayed on the face of the item."),System.ComponentModel.Editor("DevComponents.DotNetBar.Design.ImageIndexEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(System.Drawing.Design.UITypeEditor)),System.ComponentModel.TypeConverter(typeof(System.Windows.Forms.ImageIndexConverter)),System.ComponentModel.DefaultValue(-1)]
		public int ImageIndex
		{
			get
			{
				return m_ImageIndex;
			}
			set
			{
				m_ImageCachedIdx=null;
				if(m_ImageIndex!=value)
				{
					m_ImageIndex=value;
                    if(ShouldSyncProperties)
                        BarFunctions.SyncProperty(this, "ImageIndex");
					if(m_Parent!=null)
					{
						OnImageChanged();
						NeedRecalcSize=true;
						this.Refresh();
					}
				}
				OnAppearanceChanged();
			}
		}

		internal void SetImageIndex(int iImageIndex)
		{
			m_ImageIndex=iImageIndex;
		}

		/// <summary>
		/// Specifies the image for the button when mouse is over the item.
        /// </summary>
        [System.ComponentModel.Browsable(true),DevCoBrowsable(true),System.ComponentModel.Category("Appearance"),System.ComponentModel.Description("The image that will be displayed when mouse hovers over the item."),System.ComponentModel.DefaultValue(null)]
		public System.Drawing.Image HoverImage
		{
			get
			{
				return m_HoverImage;
			}
			set 
			{
				m_HoverImage=value;
				OnAppearanceChanged();
			}
		}

		/// <summary>
		/// Specifies the index of the image for the button when mouse is over the item when ImageList is used.
		/// </summary>
		[System.ComponentModel.Browsable(true),DevCoBrowsable(true),System.ComponentModel.Category("Appearance"),System.ComponentModel.Description("The image list image index of the image that will be displayed when mouse hovers over the item."),System.ComponentModel.Editor("DevComponents.DotNetBar.Design.ImageIndexEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(System.Drawing.Design.UITypeEditor)),System.ComponentModel.TypeConverter(typeof(System.Windows.Forms.ImageIndexConverter)),System.ComponentModel.DefaultValue(-1)]
		public int HoverImageIndex
		{
			get
			{
				return m_HoverImageIndex;
			}
			set
			{
				if(m_HoverImageIndex!=value)
				{
					m_HoverImageIndex=value;
                    if(ShouldSyncProperties)
                        BarFunctions.SyncProperty(this, "HoverImageIndex");
					this.Refresh();
				}
				OnAppearanceChanged();
			}
		}

		/// <summary>
		/// Specifies the image for the button when mouse left button is pressed.
        /// </summary>
        [System.ComponentModel.Browsable(true),DevCoBrowsable(true),System.ComponentModel.Category("Appearance"),System.ComponentModel.Description("The image that will be displayed when item is pressed."),System.ComponentModel.DefaultValue(null)]
		public System.Drawing.Image PressedImage
		{
			get
			{
				return m_PressedImage;
			}
			set 
			{
				m_PressedImage=value;
				OnAppearanceChanged();
			}
		}

		/// <summary>
		/// Specifies the index of the image for the button when mouse left button is pressed and ImageList is used.
		/// </summary>
		[System.ComponentModel.Browsable(true),DevCoBrowsable(true),System.ComponentModel.Category("Appearance"),System.ComponentModel.Description("The image list image index of the image that will be displayed when item is pressed."),System.ComponentModel.Editor("DevComponents.DotNetBar.Design.ImageIndexEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(System.Drawing.Design.UITypeEditor)),System.ComponentModel.TypeConverter(typeof(System.Windows.Forms.ImageIndexConverter)),System.ComponentModel.DefaultValue(-1)]
		public int PressedImageIndex
		{
			get
			{
				return m_PressedImageIndex;
			}
			set
			{
				if(m_PressedImageIndex!=value)
				{
					m_PressedImageIndex=value;
                    if(ShouldSyncProperties)
                        BarFunctions.SyncProperty(this, "PressedImageIndex");
					this.Refresh();
				}
				OnAppearanceChanged();
			}
		}

		internal CompositeImage GetImage()
		{
			if(m_MouseDown)
				return GetImage(ImageState.Pressed);
			else if(m_MouseOver)
				return GetImage(ImageState.Hover);
			return GetImage(ImageState.Default);
		}
		private CompositeImage GetImage(ImageState state)
		{
			Image image=null;

			if(m_Icon!=null)
			{
				System.Drawing.Size iconSize=this.IconSize;
				System.Drawing.Icon icon=null;
				try
				{
					icon=new System.Drawing.Icon(m_Icon,iconSize);
				}
				catch{icon=null;}
				if(icon==null)
					return new CompositeImage(m_Icon,false);
				else
					return new CompositeImage(icon,true);
			}

			if(state==ImageState.Hover && (m_HoverImage!=null || m_HoverImageIndex>=0))
			{
				if(m_HoverImage!=null)
					return new CompositeImage(m_HoverImage,false);
				if(m_HoverImageIndex>=0)
				{
					image=GetImageFromImageList(m_HoverImageIndex);
					if(image!=null)
						return new CompositeImage(image,false);
					return null;
				}
			}

			if(state==ImageState.Pressed && (m_PressedImage!=null || m_PressedImageIndex>=0))
			{
				if(m_PressedImage!=null)
					return new CompositeImage(m_PressedImage,false);
				if(m_PressedImageIndex>=0)
				{
					image=GetImageFromImageList(m_PressedImageIndex);
					if(image!=null)
						return new CompositeImage(image,false);
					return null;
				}
			}

			if(m_Image!=null)
			{
				return new CompositeImage(m_Image,false);
			}
			if(m_ImageIndex>=0)
			{
				image=GetImageFromImageList(m_ImageIndex);
				if(image!=null)
					return new CompositeImage(image,false);
			}

			return null;
		}

		private Image GetImageFromImageList(int ImageIndex)
		{
			if(ImageIndex>=0)
			{
				IOwner owner=null;
				IBarImageSize iImageSize=null;
				if(_ItemPaintArgs!=null)
				{
					owner=_ItemPaintArgs.Owner;
					iImageSize=_ItemPaintArgs.ContainerControl as IBarImageSize;
				}
				if(owner==null) owner=this.GetOwner() as IOwner;
				if(iImageSize==null) iImageSize=this.ContainerControl as IBarImageSize; 

				if(owner!=null)
				{
					try
					{
						if(iImageSize!=null && iImageSize.ImageSize!=eBarImageSize.Default)
						{
							if(iImageSize.ImageSize==eBarImageSize.Medium && owner.ImagesMedium!=null) // && owner.ImagesMedium.Images.Count>0 && ImageIndex<owner.ImagesMedium.Images.Count)
								return owner.ImagesMedium.Images[ImageIndex];
							else if(iImageSize.ImageSize==eBarImageSize.Large && owner.ImagesLarge!=null) // && owner.ImagesLarge.Images.Count>0  && ImageIndex<owner.ImagesLarge.Images.Count)
								return owner.ImagesLarge.Images[ImageIndex];
							else if(owner.Images!=null)// && owner.Images.Images.Count>0 && ImageIndex<owner.Images.Images.Count)
							{
								if(ImageIndex==m_ImageIndex)
								{
									if(m_ImageCachedIdx==null)
										m_ImageCachedIdx=owner.Images.Images[ImageIndex];
									return m_ImageCachedIdx; //owner.Images.Images[ImageIndex];
								}
								else
									return owner.Images.Images[ImageIndex];
							}
						}
						else if(m_Parent is SideBarPanelItem && ((SideBarPanelItem)m_Parent).ItemImageSize!=eBarImageSize.Default)
						{
							eBarImageSize imgSize=((SideBarPanelItem)m_Parent).ItemImageSize;
							if(imgSize==eBarImageSize.Medium && owner.ImagesMedium!=null) // && owner.ImagesMedium.Images.Count>0 && ImageIndex<owner.ImagesMedium.Images.Count)
								return owner.ImagesMedium.Images[ImageIndex];
							else if(imgSize==eBarImageSize.Large && owner.ImagesLarge!=null) // && owner.ImagesLarge.Images.Count>0  && ImageIndex<owner.ImagesLarge.Images.Count)
								return owner.ImagesLarge.Images[ImageIndex];
							else if(owner.Images!=null)// && owner.Images.Images.Count>0 && ImageIndex<owner.Images.Images.Count)
								return owner.Images.Images[ImageIndex];
						}
						else if(owner.Images!=null)// && owner.Images.Images.Count>0 && ImageIndex<owner.Images.Images.Count)
						{
							if(ImageIndex==m_ImageIndex)
							{
								if(m_ImageCachedIdx==null)
									m_ImageCachedIdx=owner.Images.Images[ImageIndex];

								return m_ImageCachedIdx; //owner.Images.Images[ImageIndex];
							}
							else
								return owner.Images.Images[ImageIndex];
						}
					}
					catch(Exception)
					{
						return null;
					}
				}
			}
			return null;
		}

		private System.Drawing.Size IconSize
		{
			get
			{
				// Default Icon Size
				System.Drawing.Size size=new Size(16,16);
				IBarImageSize iImageSize=null;
				if(_ItemPaintArgs!=null)
				{
					iImageSize=_ItemPaintArgs.ContainerControl as IBarImageSize;
				}
				if(iImageSize==null) iImageSize=this.ContainerControl as IBarImageSize; 

				try
				{
					if(iImageSize!=null && iImageSize.ImageSize!=eBarImageSize.Default)
					{
						if(iImageSize.ImageSize==eBarImageSize.Medium)
							size=new Size(24,24);
						else if(iImageSize.ImageSize==eBarImageSize.Large)
							size=new Size(32,32);
					}
					else if(m_Parent is SideBarPanelItem && ((SideBarPanelItem)m_Parent).ItemImageSize!=eBarImageSize.Default)
					{
						eBarImageSize imgSize=((SideBarPanelItem)m_Parent).ItemImageSize;
						if(imgSize==eBarImageSize.Medium)
							size=new Size(24,24);
						else if(imgSize==eBarImageSize.Large)
							size=new Size(32,32);
					}
				}
				catch(Exception)
				{
				}

				return size;
			}
		}

		/// <summary>
		/// Must be called by any sub item that implements the image when image has changed
		/// </summary>
//		public override void OnSubItemImageSizeChanged(BaseItem objItem)
//		{
//            base.OnSubItemImageSizeChanged(objItem);
//			if(this.Expanded)
//			{
//				this.RecalcSize();
//				this.Refresh();
//			}
//		}

		//***********************************************
		// IDesignTimeProvider Implementation
		//***********************************************
		InsertPosition IDesignTimeProvider.GetInsertPosition(Point pScreen, BaseItem DragItem)
		{
			InsertPosition objInsertPos=null;
			System.Windows.Forms.Control objContainer=this.ContainerControl as System.Windows.Forms.Control;
			if(objContainer==null || !this.CanCustomize)
				return null;
			Point pClient=objContainer.PointToClient(pScreen);
			Rectangle thisRect=this.DisplayRectangle;
			if(thisRect.Contains(pClient))
			{
				Rectangle r=thisRect;
				r.Size=m_PanelRect.Size;
				r.X+=m_PanelRect.Left;
				r.Y+=m_PanelRect.Top;
				if(r.Contains(pClient) && !this.Expanded)
				{
					this.Expanded=true;
					objInsertPos=new InsertPosition();
					objInsertPos.TargetProvider=this;
					if(this.SubItems.Count==0)
						objInsertPos.Position=-1;
					else
					{
						objInsertPos.Position=0;
						objInsertPos.Before=true;
					}
					return objInsertPos;
				}
				
				BaseItem objItem;
				// Check first inside any expanded items
				objItem=this.ExpandedItem();
				if(objItem!=null)
				{
					IDesignTimeProvider provider=objItem as IDesignTimeProvider;
					if(provider!=null)
					{
						objInsertPos=provider.GetInsertPosition(pScreen, DragItem);
						if(objInsertPos!=null)
							return objInsertPos;
					}
				}

				for(int i=0;i<this.SubItems.Count;i++)
				{
					objItem=this.SubItems[i];
					r=objItem.DisplayRectangle;
					r.Inflate(2,2);
					if(objItem.Visible && objItem.Displayed && r.Contains(pClient))
					{
						if(objItem.SystemItem && this.SubItems.Count!=1)
						{
							return null;
						}
						if(objItem==DragItem)
							return new InsertPosition();
						objInsertPos=new InsertPosition();
						objInsertPos.TargetProvider=this;
						objInsertPos.Position=i;
						if(pClient.Y<=objItem.TopInternal+objItem.HeightInternal/2 || objItem.SystemItem)
							objInsertPos.Before=true;

						// We need to collapse any expanded items that are not on this bar
						IOwner owner=this.GetOwner() as IOwner;
						if(owner!=null)
						{
							BaseItem objExp=owner.GetExpandedItem();
							if(objExp!=null)
							{
								while(objExp.Parent!=null)
									objExp=objExp.Parent;
								BaseItem objParent=objItem;
								while(objParent.Parent!=null)
									objParent=objParent.Parent;
								if(objExp!=objParent)
									owner.SetExpandedItem(null);
							}
						}

						if(objItem is PopupItem && (objItem.SubItems.Count>0 || objItem.IsOnMenuBar))
						{
							if(!objItem.Expanded)
								objItem.Expanded=true;
						}
						else
						{
							CollapseSubItems(this);
						}
						break;
					}
				}
				if(objInsertPos==null)
				{
					// Container is empty but it can contain the items
					if(this.SubItems.Count>1 && this.SubItems[this.SubItems.Count-1].SystemItem)
						objInsertPos=new InsertPosition(this.SubItems.Count-2,true,this);
					else
						objInsertPos=new InsertPosition(this.SubItems.Count-1,false,this);
				}
			}
			else
			{
				foreach(BaseItem objItem in this.SubItems)
				{
					if(objItem==DragItem)
						continue;
					IDesignTimeProvider provider=objItem as IDesignTimeProvider;
					if(provider!=null)
					{
						objInsertPos=provider.GetInsertPosition(pScreen, DragItem);
						if(objInsertPos!=null)
							break;
					}
				}				
			}
			return objInsertPos;
		}
		void IDesignTimeProvider.DrawReversibleMarker(int iPos, bool Before)
		{
			System.Windows.Forms.Control objCtrl=this.ContainerControl as System.Windows.Forms.Control;
			if(objCtrl==null)
				return;

			BaseItem objItem=null;
			if(iPos>=0)
				objItem=this.SubItems[iPos];
			Rectangle r, rl,rr;
			if(objItem!=null)
			{
				if(objItem.DesignInsertMarker!=eDesignInsertPosition.None)
					objItem.DesignInsertMarker=eDesignInsertPosition.None;
				else if(Before)
					objItem.DesignInsertMarker=eDesignInsertPosition.Before;
				else
					objItem.DesignInsertMarker=eDesignInsertPosition.After;
				return;
			}
			else
			{
				Rectangle rTmp=this.DisplayRectangle;
				rTmp.Inflate(-1,-1);
				rTmp.Offset(m_PanelRect.X,m_PanelRect.Bottom);
				r=new Rectangle(rTmp.Left+2,rTmp.Top+2,rTmp.Width-4,1);
				rl=new Rectangle(rTmp.Left+1,rTmp.Top,1,5);
				rr=new Rectangle(rTmp.Right-2,rTmp.Top,1,5);
			}
            //r.Location=objCtrl.PointToScreen(r.Location);
            //rl.Location=objCtrl.PointToScreen(rl.Location);
            //rr.Location=objCtrl.PointToScreen(rr.Location);
            //System.Windows.Forms.ControlPaint.DrawReversibleFrame(r,SystemColors.Control,System.Windows.Forms.FrameStyle.Thick);
            //System.Windows.Forms.ControlPaint.DrawReversibleFrame(rl,SystemColors.Control,System.Windows.Forms.FrameStyle.Thick);
            //System.Windows.Forms.ControlPaint.DrawReversibleFrame(rr,SystemColors.Control,System.Windows.Forms.FrameStyle.Thick);
		}

		void IDesignTimeProvider.InsertItemAt(BaseItem objItem, int iPos, bool Before)
		{
			if(this.ExpandedItem()!=null)
			{
				this.ExpandedItem().Expanded=false;
			}
			if(!Before)
			{
				//objItem.BeginGroup=!objItem.BeginGroup;
				if(iPos+1>=this.SubItems.Count)
				{
					this.SubItems.Add(objItem,GetAppendPosition(this));
				}
				else
				{
					this.SubItems.Add(objItem,iPos+1);
				}
			}
			else
			{
				if(iPos>=this.SubItems.Count)
				{
					this.SubItems.Add(objItem, GetAppendPosition(this));
				}
				else
				{
					this.SubItems.Add(objItem,iPos);
				}
			}
			if(this.ContainerControl is Bar)
				((Bar)this.ContainerControl).RecalcLayout();
			else if(this.ContainerControl is MenuPanel)
				((MenuPanel)this.ContainerControl).RecalcSize();
			else
			{
				this.RecalcSize();
				this.Refresh();
			}
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
	}
}
