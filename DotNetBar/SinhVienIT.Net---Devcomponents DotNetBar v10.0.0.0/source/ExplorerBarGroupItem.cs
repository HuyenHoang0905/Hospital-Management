using System;
using System.Drawing;
using System.Drawing.Design;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Windows.Forms;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Represents the Explorer-Bar Group item.
	/// </summary>
    [System.ComponentModel.ToolboxItem(false), Designer("DevComponents.DotNetBar.Design.ExplorerBarGroupItemDesigner, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
	public class ExplorerBarGroupItem:ImageItem,IDesignTimeProvider
	{
		#region Private Variables
		private Rectangle m_ExpandButtonRect=Rectangle.Empty;
		private bool m_ExpandButtonHot=false;
		private int m_Margin=2;
		private bool m_MouseOver=false;
		private bool m_MouseDown=false;
		private Rectangle m_PanelRect=Rectangle.Empty;

		private ItemStyleMapper m_HeaderStyle=null;
        private ItemStyleMapper m_HeaderHotStyle=null;
		private ItemStyleMapper m_BackgroundStyle=null;

        private ElementStyle m_TitleStyle = null;
        private ElementStyle m_TitleHotStyle = null;
        private ElementStyle m_BackStyle = null;
		private eExplorerBarStockStyle m_StockStyle=eExplorerBarStockStyle.Custom;

		private bool m_ExpandButtonVisible=true;

		//private eBarImageSize m_ItemImageSize=eBarImageSize.Default;
		private System.Drawing.Point m_MouseDownPt;

		// Used to host items that have windows associated with them....
		//private ExplorerBarPanelControlHost m_ControlHost=null;

		private const int EXPAND_MARGIN=25;

		private System.Drawing.Image m_Image=null;
		private int m_ImageIndex=-1; // Image index if image from ImageList is used
		private System.Drawing.Image m_ImageCachedIdx=null;
		private int m_SubItemsMargin=4;
		private bool m_XPSpecialGroup=false;

		// Expand Button Colors
		private Color m_ExpandBorderColor=Color.DarkGray;
		private Color m_ExpandBackColor=Color.White;
		private Color m_ExpandForeColor=SystemColors.ActiveCaption;
		private Color m_ExpandHotBorderColor=Color.DarkGray;
		private Color m_ExpandHotBackColor=Color.White;
		private Color m_ExpandHotForeColor=SystemColors.ActiveCaption;

		private bool m_HeaderExpands=true;
		private bool m_WordWrapSubItems=true;

		private const string INFO_EMPTYPANEL="Right-click header and choose one of Create commands or use SubItems collection to create new buttons. You can also drag & drop controls on this panel.";

		private bool m_DropShadow=true;
		private ShadowPaintInfo m_ShadowPaintInfo=null;
		#endregion

		/// <summary>
		/// Creates new instance of ExplorerBarGroupItem.
		/// </summary>
		public ExplorerBarGroupItem():this("","") {}
		/// <summary>
		/// Creates new instance of ExplorerBarGroupItem and assigns the name to it.
		/// </summary>
		/// <param name="sItemName">Item name.</param>
		public ExplorerBarGroupItem(string sItemName):this(sItemName,"") {}
		/// <summary>
		/// Creates new instance of ExplorerBarGroupItem and assigns the name and text to it.
		/// </summary>
		/// <param name="sItemName">Item name.</param>
		/// <param name="ItemText">item text.</param>
		public ExplorerBarGroupItem(string sItemName, string ItemText):base(sItemName,ItemText)
		{
			m_IsContainer=true;
			m_AllowOnlyOneSubItemExpanded=false;

            m_TitleStyle = new ElementStyle();
            m_TitleStyle.StyleChanged += new EventHandler(this.VisualPropertyChanged);
            m_TitleHotStyle = new ElementStyle();
            m_TitleHotStyle.StyleChanged += new EventHandler(this.VisualPropertyChanged);
            m_BackStyle = new ElementStyle();
            m_BackStyle.StyleChanged += new EventHandler(this.VisualPropertyChanged);

            m_HeaderStyle = new ItemStyleMapper(m_TitleStyle);
            m_HeaderHotStyle = new ItemStyleMapper(m_TitleHotStyle);
            m_BackgroundStyle = new ItemStyleMapper(m_BackStyle);

            SubItemsImageSize = new Size(12, 12);
            ImageSize = new Size(12, 12);

            //try
            //{
            //    m_HeaderStyle.Font=new Font(System.Windows.Forms.SystemInformation.MenuFont,FontStyle.Bold);
            //    m_HeaderHotStyle.Font=new Font(System.Windows.Forms.SystemInformation.MenuFont,FontStyle.Bold);
            //}
            //catch
            //{
            //    m_HeaderStyle.Font=System.Windows.Forms.SystemInformation.MenuFont.Clone() as Font;
            //    m_HeaderHotStyle.Font=System.Windows.Forms.SystemInformation.MenuFont.Clone() as Font;
            //}

			BarFunctions.SetExplorerBarStyle(this,m_StockStyle);

            //m_BackgroundStyle.VisualPropertyChanged+=new EventHandler(this.VisualPropertyChanged);
            //m_HeaderStyle.VisualPropertyChanged+=new EventHandler(this.VisualPropertyChanged);
            //m_HeaderHotStyle.VisualPropertyChanged+=new EventHandler(this.VisualPropertyChanged);
		}

        protected override void Dispose(bool disposing)
        {
            if (BarUtilities.DisposeItemImages && !this.DesignMode)
            {
                BarUtilities.DisposeImage(ref m_Image);
                BarUtilities.DisposeImage(ref m_ImageCachedIdx);

            }
            base.Dispose(disposing);
        }

		/// <summary>
		/// Returns copy of ExplorerBarGroupItem item.
		/// </summary>
		public override BaseItem Copy()
		{
			ExplorerBarGroupItem objCopy=new ExplorerBarGroupItem();
			this.CopyToItem(objCopy);
			return objCopy;
		}
		protected override void CopyToItem(BaseItem copy)
		{
			ExplorerBarGroupItem objCopy=copy as ExplorerBarGroupItem;
			
			base.CopyToItem(objCopy);

			//objCopy.ItemImageSize=m_ItemImageSize;
		}
		private void VisualPropertyChanged(object sender, EventArgs e)
		{
			VisualPropertyChanged();
		}
		internal void VisualPropertyChanged()
		{
            //ExplorerBar eb=this.ContainerControl as ExplorerBar;
            //if(eb!=null)
            //{
            //    ColorScheme cs=eb.ColorScheme;
            //    m_BackgroundStyle.ApplyColorScheme(cs);
            //    m_HeaderHotStyle.ApplyColorScheme(cs);
            //    m_HeaderStyle.ApplyColorScheme(cs);
            //}
			OnAppearanceChanged();
		}
		public override void RecalcSize()
		{
			System.Windows.Forms.Control objCtrl=this.ContainerControl as System.Windows.Forms.Control;
			if(!IsHandleValid(objCtrl))
				return;
			
			Graphics g=Graphics.FromHwnd(objCtrl.Handle);
			SizeF objStringSize=SizeF.Empty;
			System.Drawing.Image image=this.GetImage();

			string text=m_Text;
			if(text=="")
				text=" ";
			int textArea=m_Rect.Width-m_Margin*2;
			if(m_ExpandButtonVisible)
				textArea-=EXPAND_MARGIN;
			if(image!=null)
				textArea-=(image.Width+m_Margin*2);
			if(textArea<=0)
				textArea=1;
            bool disposeFont = false;
            Font font = this.GetFont(out disposeFont);
            objStringSize = TextDrawing.MeasureString(g, text, font, textArea, m_TitleStyle.TextFormat);
            if (disposeFont) font.Dispose();
			g.Dispose();

			m_Rect.Height=23;
			if(m_Rect.Height<(int)objStringSize.Height+m_Margin*2)
				m_Rect.Height=(int)objStringSize.Height+m_Margin*2;
			
			if(image!=null)
			{
				int h=m_Rect.Height;
				if(image.Height>m_Rect.Height)
					m_Rect.Height=image.Height+m_Margin*2;
				m_PanelRect=new Rectangle(0,m_Rect.Height-h,m_Rect.Width,h);
			}
			else
				m_PanelRect=new Rectangle(0,0,m_Rect.Width,m_Rect.Height);

			if(m_ExpandButtonVisible)
			{
				m_ExpandButtonRect=new Rectangle(m_PanelRect.Right-EXPAND_MARGIN,m_PanelRect.Y,EXPAND_MARGIN,m_PanelRect.Height);
			}
			
			if(this.Expanded)
			{
				if(m_SubItems!=null)
				{
					int iTop=m_Rect.Bottom+1;
					int iLeft=m_Rect.Left+m_SubItemsMargin;
//					if(m_ControlHost!=null)
//					{
//						iTop=0;
//						iLeft=0;
//					}
					int iIndex=-1;
					foreach(BaseItem item in m_SubItems)
					{
						iIndex++;
						if(!item.Visible)
						{
							item.Displayed=false;
							continue;
						}
						item.WidthInternal=m_Rect.Width-m_SubItemsMargin*2;
                        item.RecalcSize();
						item.WidthInternal=m_Rect.Width-m_SubItemsMargin*2;
						if(item.BeginGroup)
						{
							iTop+=3;
						}
						item.TopInternal=iTop;
						item.LeftInternal=iLeft;
						iTop+=item.HeightInternal;
						item.Displayed=true;
					}
					m_Rect.Height=iTop-m_Rect.Top+2;
				}
			}
			else
			{
				foreach(BaseItem item in m_SubItems)
				{
					item.Displayed=false;
				}
			}

			if(this.Expanded && this.DesignMode && this.SubItems.Count==0 && this.Parent!=null && this.Parent.SubItems.Count==1)
			{
				m_Rect.Height+=64;
			}

			base.RecalcSize();
		}
		private Image GetImage()
		{
			if(m_Image!=null)
				return m_Image;
			if(m_ImageIndex>=0)
			{
				return GetImageFromImageList(m_ImageIndex);
			}
			return null;
		}

		private Image GetImageFromImageList(int ImageIndex)
		{
			if(ImageIndex>=0)
			{
				IOwner owner=null;
				Bar bar=null;
				if(owner==null) owner=this.GetOwner() as IOwner;
				if(bar==null) bar=this.ContainerControl as Bar; 

				if(owner!=null)
				{
					try
					{
						if(owner.ImagesMedium!=null)
						{
							if(m_ImageCachedIdx==null)
								m_ImageCachedIdx=owner.ImagesMedium.Images[ImageIndex];
							return m_ImageCachedIdx;
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

		private ShadowPaintInfo GetShadowPaintInfo()
		{
			if(m_ShadowPaintInfo==null)
				m_ShadowPaintInfo=new ShadowPaintInfo();
			m_ShadowPaintInfo.Size=3;
			return m_ShadowPaintInfo;
		}

        private bool m_IsPainting = false;
		public override void Paint(ItemPaintArgs pa)
		{
			if(this.DisplayRectangle.Width<=0 || this.DisplayRectangle.Height<=0 || m_IsPainting)
				return;

            m_IsPainting = true;
            try
            {
                if (this.IsThemed)
                {
                    PaintThemed(pa);
                    this.DrawInsertMarker(pa.Graphics);
                    return;
                }

                if (this.SuspendLayout)
                    return;
                System.Drawing.Graphics g = pa.Graphics;
                if (m_NeedRecalcSize)
                    RecalcSize();

                Rectangle r = m_PanelRect;
                r.Offset(m_Rect.X, m_Rect.Y);
                bool disposeFont = false;
                Font font = this.GetFont(out disposeFont);
                System.Windows.Forms.Control ctrl = this.ContainerControl as System.Windows.Forms.Control;

                System.Drawing.Image image = this.GetImage();
                Rectangle rText = r;
                rText.X += m_Margin;
                // This is left margin of the text
                rText.X += m_Margin * 2;
                rText.Width -= (m_Margin * 3);
                if (image != null)
                {
                    rText.Width -= (image.Width + m_Margin);
                    rText.X += (image.Width + m_Margin);
                }
                if (m_ExpandButtonVisible)
                    rText.Width -= EXPAND_MARGIN;

                ElementStyleDisplayInfo info = new ElementStyleDisplayInfo();
                info.Bounds = r;
                info.Graphics = g;
                if (m_MouseOver)
                    info.Style = m_TitleHotStyle;
                else
                    info.Style = m_TitleStyle;
                ElementStyleDisplay.Paint(info);
                if (info.Style.Font != null)
                    font = info.Style.Font;
                if (pa.RightToLeft)
                {
                    TextDrawing.DrawString(g, m_Text, font, info.Style.TextColor, GetRtlRectangle(m_PanelRect, rText), info.Style.TextFormat | eTextFormat.RightToLeft);
                }
                else
                    TextDrawing.DrawString(g, m_Text, font, info.Style.TextColor, rText, info.Style.TextFormat);

                //g.ResetClip();
                //g.Clip=oldClip;

                if (m_ExpandButtonVisible)
                {
                    Rectangle expandedRect = m_ExpandButtonRect;
                    expandedRect.Offset(m_Rect.X, m_Rect.Y);
                    if (pa.RightToLeft)
                        PaintExpandButton(pa, GetRtlRectangle(m_Rect, expandedRect), m_MouseOver, m_MouseDown, m_Expanded);
                    else
                        PaintExpandButton(pa, expandedRect, m_MouseOver, m_MouseDown, m_Expanded);
                }

                int cornerSize = m_TitleStyle.CornerDiameter;
                // Paint Background
                Rectangle backRect = new Rectangle(m_Rect.X, m_PanelRect.Bottom + m_Rect.Y, m_Rect.Width, m_Rect.Height - m_PanelRect.Bottom);
                if (backRect.Width > 0 && backRect.Height > 0)
                {
                    //g.Clip=new Region(backRect);
                    info.Style = m_BackStyle;
                    info.Bounds = backRect;
                    ElementStyleDisplay.Paint(info);
                    //m_BackgroundStyle.Paint(g,backRect,"",Rectangle.Empty,this.GetFont(),new Point[]{new Point(backRect.X,backRect.Y),new Point(backRect.X,backRect.Bottom-1),new Point(backRect.X,backRect.Bottom-1),new Point(backRect.Right-1,backRect.Bottom-1),new Point(backRect.Right-1,backRect.Bottom-1),new Point(backRect.Right-1,backRect.Y)});
                    //g.ResetClip();
                    //g.Clip=oldClip;
                    if (m_DropShadow)
                    {
                        ShadowPaintInfo shadowInfo = GetShadowPaintInfo();
                        shadowInfo.Rectangle = new Rectangle(m_Rect.X, m_Rect.Y + m_PanelRect.Top + cornerSize, m_Rect.Width, m_Rect.Height - m_PanelRect.Top - cornerSize);
                        shadowInfo.Graphics = g;
                        ShadowPainter.Paint(shadowInfo);
                    }
                }
                else
                {
                    if (m_DropShadow)
                    {
                        r.Y += cornerSize;
                        r.Height -= cornerSize;
                        ShadowPaintInfo shadowInfo = GetShadowPaintInfo();
                        shadowInfo.Rectangle = r;
                        shadowInfo.Graphics = g;
                        ShadowPainter.Paint(shadowInfo);
                    }
                }

                // Draw Image
                if (image != null)
                {
                    if (pa.RightToLeft)
                        g.DrawImage(image, GetRtlRectangle(m_PanelRect, new Rectangle(r.Left + m_Margin, r.Bottom - m_Margin - image.Height, image.Width, image.Height)));
                    else
                        g.DrawImage(image, r.Left + m_Margin, r.Bottom - m_Margin - image.Height, image.Width, image.Height);
                }

                if (this.Focused)
                {
                    if (this.DesignMode)
                    {
                        Rectangle rFocus = r;
                        rFocus.Inflate(-1, -1);
                        if (pa.RightToLeft) rFocus = GetRtlRectangle(m_PanelRect, rFocus);
                        DesignTime.DrawDesignTimeSelection(g, r, pa.Colors.ItemDesignTimeBorder);
                    }
                    else
                    {
                        Rectangle rFocus = rText;
                        rFocus.Inflate(0, -1);
                        rFocus.Width -= 2;
                        rFocus.X -= 2;
                        if (pa.RightToLeft) rFocus = GetRtlRectangle(m_PanelRect, rFocus);
                        System.Windows.Forms.ControlPaint.DrawFocusRectangle(g, rFocus);
                    }
                }

                if ((this.Expanded || this.Parent is ExplorerBarContainerItem && ((ExplorerBarContainerItem)this.Parent)._Animating) && m_SubItems != null) // && m_ControlHost==null)
                {
                    r = new Rectangle(m_Rect.X, m_Rect.Y + r.Height + 1, m_Rect.Width, m_Rect.Height - r.Height - 1);
                    for (int i = 0; i < m_SubItems.Count; i++)
                    {
                        BaseItem item = m_SubItems[i];
                        if (!item.Displayed || !item.Visible)
                            continue;
                        if (item.BeginGroup)
                        {
                            using (Pen line = new Pen(pa.Colors.ItemSeparator, 1))
                                g.DrawLine(line, item.LeftInternal + 2, item.TopInternal - 2, item.DisplayRectangle.Right - 4, item.TopInternal - 2);
                        }
                        item.Paint(pa);
                    }
                }

                if (disposeFont)
                    font.Dispose();

                this.DrawInsertMarker(pa.Graphics);

                PaintInfoText(pa);
            }
            finally
            {
                m_IsPainting = false;
            }
		}

        private Rectangle GetRtlRectangle(Rectangle bounds, Rectangle r)
        {
            return new Rectangle(bounds.Right - (r.Width + (r.X - bounds.X)), r.Y, r.Width, r.Height);
        }

		private void PaintThemed(ItemPaintArgs pa)
		{
			if(this.SuspendLayout)
				return;
			System.Drawing.Graphics g;
			if(m_NeedRecalcSize)
				RecalcSize();

			Bitmap bmp=new Bitmap(this.DisplayRectangle.Width,this.DisplayRectangle.Height,pa.Graphics);
			g=Graphics.FromImage(bmp);

			ThemeExplorerBar theme=pa.ThemeExplorerBar;
			Rectangle r=m_PanelRect;

			ThemeExplorerBarParts part=ThemeExplorerBarParts.NormalGroupHead;
			ThemeExplorerBarStates state=ThemeExplorerBarStates.NormalGroupHeadNormal;
			if(m_XPSpecialGroup)
				part=ThemeExplorerBarParts.SpecialGroupHead;
			theme.DrawBackground(g,part,state,r);

			// Paint Background
			Rectangle backRect=new Rectangle(0,m_PanelRect.Bottom,m_Rect.Width+(m_XPSpecialGroup?0:0),m_Rect.Height-m_PanelRect.Bottom);
			if(backRect.Width>0 && backRect.Height>0)
			{
				ThemeExplorerBarParts partBack;
				ThemeExplorerBarStates stateBack;
				if(m_XPSpecialGroup)
				{
					partBack=ThemeExplorerBarParts.SpecialGroupBackground;
					stateBack=ThemeExplorerBarStates.SpecialGroupBackgroundNormal;
				}
				else
				{
					partBack=ThemeExplorerBarParts.NormalGroupBackground;
					stateBack=ThemeExplorerBarStates.NormalGroupBackgroundNormal;
				}
				theme.DrawBackground(g,partBack,stateBack,backRect);
			}

			System.Drawing.Image image=this.GetImage();
			Rectangle rText=r;
			rText.X+=m_Margin;
			// This is left margin of the text
			rText.X+=m_Margin*2;
			rText.Width-=(m_Margin*5);
			if(image!=null)
			{
				rText.Width-=(image.Width+m_Margin);
				rText.X+=(image.Width+m_Margin);
			}
			if(m_ExpandButtonVisible)
				rText.Width-=EXPAND_MARGIN;

            bool disposeFont = false;
			Font font=this.GetFont(out disposeFont);
            ElementStyle style = m_TitleStyle;
            if (m_MouseOver)
                style = m_TitleHotStyle;
            if (style.Font != null)
                font = style.Font;

            if (pa.RightToLeft)
            {
                TextDrawing.DrawStringLegacy(g, m_Text, font, style.TextColor, GetRtlRectangle(m_PanelRect, rText), style.TextFormat | eTextFormat.RightToLeft);
            }
            else
                TextDrawing.DrawStringLegacy(g, m_Text, font, style.TextColor, rText, style.TextFormat);

			if(m_ExpandButtonVisible)
			{
				ThemeExplorerBarParts part2=part;
				Rectangle expandedRect=m_ExpandButtonRect;
				if(m_XPSpecialGroup)
				{
					if(this.Expanded)
					{
						part=ThemeExplorerBarParts.SpecialGroupCollapse;
						if(m_ExpandButtonHot)
							state=ThemeExplorerBarStates.SpecialGroupCollapseHot;
						else
							state=ThemeExplorerBarStates.SpecialGroupCollapseNormal;
					}
					else
					{
						part=ThemeExplorerBarParts.SpecialGroupExpand;
						if(m_ExpandButtonHot)
							state=ThemeExplorerBarStates.SpecialGroupExpandHot;
						else
							state=ThemeExplorerBarStates.SpecialGroupExpandNormal;
					}
				}
				else
				{
					if(this.Expanded)
					{
						part=ThemeExplorerBarParts.NormalGroupCollapse;
						if(m_ExpandButtonHot)
							state=ThemeExplorerBarStates.NormalGroupCollapseHot;
						else
							state=ThemeExplorerBarStates.NormalGroupCollapseNormal;
					}
					else
					{
						part=ThemeExplorerBarParts.NormalGroupExpand;
						if(m_ExpandButtonHot)
							state=ThemeExplorerBarStates.NormalGroupExpandHot;
						else
							state=ThemeExplorerBarStates.NormalGroupExpandNormal;
					}
				}
                if(pa.RightToLeft)
                    theme.DrawBackground(g, part, state, GetRtlRectangle(m_Rect, expandedRect));
                else
				    theme.DrawBackground(g,part,state, expandedRect);
			}
			
			// Draw Image
			if(image!=null)
			{
                if(pa.RightToLeft)
                    g.DrawImage(image, GetRtlRectangle(m_PanelRect, new Rectangle(r.Left + m_Margin, r.Bottom - m_Margin - image.Height, image.Width, image.Height)));
                else
				    g.DrawImage(image,r.Left+m_Margin,r.Bottom-m_Margin-image.Height,image.Width,image.Height);
			}

			if(this.Focused)
			{
				if(this.DesignMode)
				{
					Rectangle rFocus=r;
					rFocus.Inflate(-1,-1);
                    if (pa.RightToLeft) rFocus = GetRtlRectangle(m_PanelRect, rFocus);
					DesignTime.DrawDesignTimeSelection(g,r,pa.Colors.ItemDesignTimeBorder);
				}
				else
				{
					Rectangle rFocus=rText;
					rFocus.Inflate(0,-1);
					rFocus.Width-=2;
					rFocus.X-=2;
                    if (pa.RightToLeft) rFocus = GetRtlRectangle(m_PanelRect, rFocus);
					System.Windows.Forms.ControlPaint.DrawFocusRectangle(g,rFocus);
				}
			}

			g.Dispose();
			g=pa.Graphics;
			g.DrawImage(bmp,this.DisplayRectangle.X,this.DisplayRectangle.Y,bmp.Width,bmp.Height);
			bmp.Dispose();

            if (disposeFont) font.Dispose();

			if((this.Expanded || this.Parent is ExplorerBarContainerItem && ((ExplorerBarContainerItem)this.Parent)._Animating) && m_SubItems!=null) // && m_ControlHost==null)
			{
				r=new Rectangle(m_Rect.X,m_Rect.Y+r.Height+1,m_Rect.Width,m_Rect.Height-r.Height-1);
				for(int i=0;i<m_SubItems.Count;i++)
				{
					BaseItem item=m_SubItems[i];
					if(!item.Displayed || !item.Visible)
						continue;
					item.Paint(pa);
				}
			}
			PaintInfoText(pa);
		}

		private void PaintInfoText(ItemPaintArgs pa)
		{
			if(m_SubItems!=null && m_SubItems.Count==0 && this.DesignMode)
			{
				Rectangle rInfoText=m_Rect;
				if(!m_PanelRect.IsEmpty)
				{
					rInfoText.Y+=m_PanelRect.Height;
					rInfoText.Height-=m_PanelRect.Height;
				}
				string info=INFO_EMPTYPANEL;
				rInfoText.Inflate(-1,-1);
                eTextFormat format = eTextFormat.Default | eTextFormat.HorizontalCenter | eTextFormat.VerticalCenter | eTextFormat.WordBreak;
				Font font=new Font(SystemInformation.MenuFont.FontFamily,7);
				TextDrawing.DrawString(pa.Graphics,info,font,SystemColors.ControlText,rInfoText,format);
				font.Dispose();
			}
		}

		/// <summary>
		/// Specifies whether item is drawn using Themes when running on OS that supports themes like Windows XP.
		/// </summary>
		[Browsable(false),DevCoBrowsable(false),DefaultValue(false),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),Category("Appearance"),Description("Specifies whether item is drawn using Themes when running on OS that supports themes like Windows XP.")]
		public override bool ThemeAware
		{
			get
			{
				return base.ThemeAware;
			}
			set
			{
				m_ThemeAware=value;
				if(m_SubItems!=null)
				{
					foreach(BaseItem item in m_SubItems)
					{
						if(!(item is ButtonItem))
							item.ThemeAware=value;
					}
				}
			}
		}

		private void PaintThemeExpandButton()
		{
			if(!m_ExpandButtonVisible)
				return;

            System.Windows.Forms.Control container=this.ContainerControl as System.Windows.Forms.Control;
			if(!IsHandleValid(container))
				return;

			Graphics g=BarFunctions.CreateGraphics(container);
			try
			{
				ThemeExplorerBar theme=null;
				bool bDisposeTheme=false;
				if(container is IThemeCache)
					theme=((IThemeCache)container).ThemeExplorerBar;
				else
				{
					bDisposeTheme=true;
					theme=new ThemeExplorerBar(container);
				}
				ThemeExplorerBarParts part=ThemeExplorerBarParts.NormalGroupCollapse;
				ThemeExplorerBarStates state=ThemeExplorerBarStates.NormalGroupCollapseNormal;

				Rectangle expandedRect=m_ExpandButtonRect;
				expandedRect.Offset(m_Rect.X,m_Rect.Y);

				if(m_XPSpecialGroup)
				{
					if(this.Expanded)
					{
						part=ThemeExplorerBarParts.SpecialGroupCollapse;
						if(m_ExpandButtonHot)
							state=ThemeExplorerBarStates.SpecialGroupCollapseHot;
						else
							state=ThemeExplorerBarStates.SpecialGroupCollapseNormal;
					}
					else
					{
						part=ThemeExplorerBarParts.SpecialGroupExpand;
						if(m_ExpandButtonHot)
							state=ThemeExplorerBarStates.SpecialGroupExpandHot;
						else
							state=ThemeExplorerBarStates.SpecialGroupExpandNormal;
					}
				}
				else
				{
					if(this.Expanded)
					{
						part=ThemeExplorerBarParts.NormalGroupCollapse;
						if(m_ExpandButtonHot)
							state=ThemeExplorerBarStates.NormalGroupCollapseHot;
						else
							state=ThemeExplorerBarStates.NormalGroupCollapseNormal;
					}
					else
					{
						part=ThemeExplorerBarParts.NormalGroupExpand;
						if(m_ExpandButtonHot)
							state=ThemeExplorerBarStates.NormalGroupExpandHot;
						else
							state=ThemeExplorerBarStates.NormalGroupExpandNormal;
					}
				}
				theme.DrawBackground(g,part,state,expandedRect);
				
				if(bDisposeTheme)
					theme.Dispose();
			}
			finally
			{
                if (g != null)
                {
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
                    g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
                    g.Dispose();
                }
			}
		}

		private void PaintExpandButton()
		{
			if(!m_ExpandButtonVisible)
				return;

			Rectangle r=m_ExpandButtonRect;
			r.Offset(m_Rect.X,m_Rect.Y);
			System.Windows.Forms.Control control=this.ContainerControl as System.Windows.Forms.Control;
			if(control==null)
				return;
			control.Invalidate(r);
			control.Update();
			return;
		}
		private void PaintExpandButton(ItemPaintArgs pa, Rectangle r, bool bHot, bool bPressed, bool bExpanded)
		{
            Graphics g = pa.Graphics;
            ExplorerBar bar = pa.ContainerControl as ExplorerBar;
            if (bar != null && (bar.GroupButtonExpandNormal != null && !bExpanded || bar.GroupButtonCollapseNormal != null && bExpanded))
            {
                Image image = bar.GroupButtonExpandNormal;
                if (bExpanded)
                    image = bar.GroupButtonCollapseNormal;
                if (bPressed)
                {
                    if (!bExpanded && bar.GroupButtonExpandPressed!=null)
                        image = bar.GroupButtonExpandPressed;
                    else if(bExpanded && bar.GroupButtonCollapsePressed!=null)
                        image = bar.GroupButtonCollapsePressed;
                }
                else if (bHot)
                {
                    if (!bExpanded && bar.GroupButtonExpandHot != null)
                        image = bar.GroupButtonExpandHot;
                    else if(bExpanded && bar.GroupButtonCollapseHot!=null)
                        image = bar.GroupButtonCollapseHot;
                }

                Rectangle imageRect = r;
                imageRect.Y += (imageRect.Height - image.Height) / 2;

                g.DrawImageUnscaled(image, imageRect);
                return;
            }

			const int EXPAND_SIZE=16;
			using(Pen p=new Pen((bHot?m_ExpandHotBorderColor:m_ExpandBorderColor),1))
			{
				if(r.Width>EXPAND_SIZE)
					r.Offset((r.Width-EXPAND_SIZE)/2,0);
				if(r.Height>EXPAND_SIZE)
					r.Offset(0,(r.Height-EXPAND_SIZE)/2);
				r.Width=EXPAND_SIZE;
				r.Height=EXPAND_SIZE;
				Brush brush=new SolidBrush((bHot?m_ExpandHotBackColor:m_ExpandBackColor));
				g.FillEllipse(brush,r);
				brush.Dispose();
				g.DrawEllipse(p,r);
			}

			using(Pen p=new Pen((bHot?m_ExpandHotForeColor:m_ExpandForeColor),1))
			{
				if(bExpanded)
				{
					Point midPoint=new Point(r.X+EXPAND_SIZE/2,r.Y+4);
					g.DrawLine(p,midPoint.X,midPoint.Y,midPoint.X-3,midPoint.Y+3);
					g.DrawLine(p,midPoint.X,midPoint.Y,midPoint.X+3,midPoint.Y+3);
					g.DrawLine(p,midPoint.X,midPoint.Y+1,midPoint.X-3,midPoint.Y+4);
					g.DrawLine(p,midPoint.X,midPoint.Y+1,midPoint.X+3,midPoint.Y+4);

					midPoint.Y+=4;
					g.DrawLine(p,midPoint.X,midPoint.Y,midPoint.X-3,midPoint.Y+3);
					g.DrawLine(p,midPoint.X,midPoint.Y,midPoint.X+3,midPoint.Y+3);
					g.DrawLine(p,midPoint.X,midPoint.Y+1,midPoint.X-3,midPoint.Y+4);
					g.DrawLine(p,midPoint.X,midPoint.Y+1,midPoint.X+3,midPoint.Y+4);
				}
				else
				{
					Point midPoint=new Point(r.X+EXPAND_SIZE/2,r.Y+8);
					g.DrawLine(p,midPoint.X,midPoint.Y,midPoint.X-3,midPoint.Y-3);
					g.DrawLine(p,midPoint.X,midPoint.Y,midPoint.X+3,midPoint.Y-3);
					g.DrawLine(p,midPoint.X,midPoint.Y-1,midPoint.X-3,midPoint.Y-4);
					g.DrawLine(p,midPoint.X,midPoint.Y-1,midPoint.X+3,midPoint.Y-4);

					midPoint.Y+=4;
					g.DrawLine(p,midPoint.X,midPoint.Y,midPoint.X-3,midPoint.Y-3);
					g.DrawLine(p,midPoint.X,midPoint.Y,midPoint.X+3,midPoint.Y-3);
					g.DrawLine(p,midPoint.X,midPoint.Y-1,midPoint.X-3,midPoint.Y-4);
					g.DrawLine(p,midPoint.X,midPoint.Y-1,midPoint.X+3,midPoint.Y-4);
				}
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
		protected virtual Font GetFont(out bool disposeFont)
		{
            disposeFont=false;
            if (this.TitleStyle.Font != null)
                return this.TitleStyle.Font;
            System.Drawing.Font font = null; // System.Windows.Forms.SystemInformation.MenuFont;
			System.Windows.Forms.Control ctrl=this.ContainerControl as System.Windows.Forms.Control;
            if (ctrl != null)
                font = ctrl.Font;
            else
            {
                font = System.Windows.Forms.SystemInformation.MenuFont;
                disposeFont = true;
            }
            if (!font.Bold)
            {
                font = new Font(font, FontStyle.Bold);
                disposeFont = true;
            }
			return font;
		}

		/// <summary>
		/// Occurs when the mouse pointer is moved over the item. This is used by internal implementation only.
		/// </summary>
		[System.ComponentModel.Browsable(false),System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override void InternalMouseMove(System.Windows.Forms.MouseEventArgs objArg)
		{
			if(objArg.Button==System.Windows.Forms.MouseButtons.Left && (Math.Abs(objArg.X-m_MouseDownPt.X)>=4 || Math.Abs(objArg.Y-m_MouseDownPt.Y)>=4))
			{
				ExplorerBar explorerBar=this.ContainerControl as ExplorerBar;
				if(explorerBar!=null && explorerBar.AllowUserCustomize && this.CanCustomize)
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

			Rectangle r=m_PanelRect;
			r.Offset(m_Rect.Location);
			if(r.Contains(objArg.X,objArg.Y))
			{
				if(!m_MouseOver)
				{
					m_MouseOver=true;
					if(m_HeaderExpands)
						this.Cursor=System.Windows.Forms.Cursors.Hand;
					this.Refresh();
				}
			}
			else if(m_MouseOver)
			{
				if(m_HeaderExpands)
					this.Cursor=System.Windows.Forms.Cursors.Default;
				m_MouseOver=false;
				this.Refresh();
			}

			Rectangle rExpand=m_ExpandButtonRect;
			rExpand.Offset(m_Rect.Location);
			if(m_ExpandButtonVisible && rExpand.Contains(objArg.X,objArg.Y))
			{
				if(m_HotSubItem!=null)
				{
					m_HotSubItem.InternalMouseLeave();
					m_HotSubItem=null;
				}
				m_ExpandButtonHot=true;
				PaintExpandButton();
				return;
			}
			else if(m_ExpandButtonHot)
			{
				m_ExpandButtonHot=false;
				PaintExpandButton();
			}
			base.InternalMouseMove(objArg);
		}

		[System.ComponentModel.Browsable(false),System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override void InternalMouseLeave()
		{
			this.Cursor=System.Windows.Forms.Cursors.Default;
			base.InternalMouseLeave();
			m_MouseOver=false;
			m_MouseDown=false;
			if(m_ExpandButtonHot)
				m_ExpandButtonHot=false;
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

            Rectangle rExpand=m_ExpandButtonRect;
			rExpand.Offset(m_Rect.X,m_Rect.Y);
			if(m_ExpandButtonVisible && objArg.Button==System.Windows.Forms.MouseButtons.Left && rExpand.Contains(objArg.X,objArg.Y))
			{
				ExpandButtonClick();
				return;
			}

			base.InternalMouseDown(objArg);

			if(objArg.Button == System.Windows.Forms.MouseButtons.Left && r.Contains(objArg.X,objArg.Y))
			{
				m_MouseDown=true;
				if(m_HeaderExpands)
					this.Expanded=!this.Expanded;
				else
					this.Refresh();
			}
		}

		internal void ExpandButtonClick()
		{
            this.Expanded=!this.Expanded;
		}

        protected internal override void OnExpandChange()
        {
            base.OnExpandChange();
            foreach (BaseItem item in this.SubItems)
            {
                if (item.Visible) item.Displayed = this.Expanded;
            }
        }

		[System.ComponentModel.Browsable(false),System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override void InternalMouseUp(System.Windows.Forms.MouseEventArgs objArg)
		{
			if(objArg.Button==System.Windows.Forms.MouseButtons.Left)
			{
				m_MouseDown=false;
				this.Refresh();
			}

			base.InternalMouseUp(objArg);
		}

		/// <summary>
		/// Occurs when the item is clicked. This is used by internal implementation only.
		/// </summary>
		[System.ComponentModel.Browsable(false),System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override void InternalClick(System.Windows.Forms.MouseButtons mb, System.Drawing.Point mpos)
		{
			base.InternalClick(mb,mpos);
			if((this.GetOwner() as IOwner)!=null && ((IOwner)this.GetOwner()).DragInProgress)
				return;
		}

		protected internal override void OnSubItemsClear()
		{
			base.OnSubItemsClear();
			NeedRecalcSize=true;
			if(this.DesignMode)
				this.Refresh();
		}

		protected override void OnTopLocationChanged(int oldValue)
		{
			int iDiff=m_Rect.Top-oldValue;
			if(m_SubItems!=null)
			{
				foreach(BaseItem item in m_SubItems)
				{
					if(item.Visible)
					{
						// Set item position
						item.TopInternal+=iDiff;
					}
				}
			}
		}

        protected internal override void OnContainerChanged(object objOldContainer)
        {
            base.OnContainerChanged(objOldContainer);
            if (this.ContainerControl is ExplorerBar)
            {
                ExplorerBar bar = this.ContainerControl as ExplorerBar;
                m_TitleStyle.SetColorScheme(bar.ColorScheme);
                m_TitleHotStyle.SetColorScheme(bar.ColorScheme);
                m_BackStyle.SetColorScheme(bar.ColorScheme);
            }
        }

		/// <summary>
		/// Called when Visibility of the items has changed.
		/// </summary>
		/// <param name="newValue">New Visible state.</param>
		protected internal override void OnVisibleChanged(bool bVisible)
		{
			base.OnVisibleChanged(bVisible);
			if(!bVisible)
			{
				foreach(BaseItem item in this.SubItems)
					item.Displayed=false;
			}
		}

		/// <summary>
		/// Gets or sets whether expand button is visible.
		/// </summary>
		[System.ComponentModel.Browsable(true),System.ComponentModel.DefaultValue(true),DevCoBrowsable(true),System.ComponentModel.Category("Behavior"),System.ComponentModel.Description("Gets or sets whether expand button is visible.")]
		public bool ExpandButtonVisible
		{
			get
			{
				return m_ExpandButtonVisible;
			}
			set
			{
				m_ExpandButtonVisible=value;
				NeedRecalcSize=true;
				OnAppearanceChanged();
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the item is expanded or not. For Popup items this would indicate whether the item is popped up or not.
		/// </summary>
		[System.ComponentModel.Browsable(true),DevCoBrowsable(true),Category("Behavior"),System.ComponentModel.DefaultValue(false),Description("Gets or sets a value indicating whether group is expanded or not."),DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public override bool Expanded
		{
			get
			{
				return base.Expanded;
			}
			set
			{
				base.Expanded=value;
			}
		}

		/// <summary>
		/// Gets or sets whether drop shadow is displayed when non-themed display is used.
		/// </summary>
		[Browsable(true),Category("Appearance"),DefaultValue(true),Description("Indicates whether drop shadow is displayed when non-themed display is used.")]
		public bool DropShadow
		{
			get {return m_DropShadow;}
			set {m_DropShadow=value;}
		}

        /// <summary>
        /// Gets the reference to ElementStyle object which describes visual appearance of the explorer group item title.
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), Category("Style"), Description("Gets or sets a normal item style."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ElementStyle TitleStyle
        {
            get { return m_TitleStyle; }
        }

        /// <summary>
        /// Gets the reference to ElementStyle object which describes visual appearance of the explorer group item title while mouse is over the title bar.
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), Category("Style"), Description("Gets or sets a mouse over item style."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ElementStyle TitleHotStyle
        {
            get { return m_TitleHotStyle; }
        }

        /// <summary>
        /// Gets or sets the item background style.
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), Category("Style"), Description("Gets or sets group background style."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ElementStyle BackStyle
        {
            get { return m_BackStyle; }
        }

		[Browsable(false),DevCoBrowsable(false),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),Obsolete("This property is obsolete. Use TitleStyle property instead"),EditorBrowsable(EditorBrowsableState.Never)]
		public ItemStyleMapper HeaderStyle
		{
			get {return m_HeaderStyle;}
		}

        [Browsable(false), DevCoBrowsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Obsolete("This property is obsolete. Use TitleHotStyle property instead"), EditorBrowsable(EditorBrowsableState.Never)]
        public ItemStyleMapper HeaderHotStyle
		{
			get {return m_HeaderHotStyle;}
		}

        [Browsable(false), DevCoBrowsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Obsolete("This property is obsolete. Use BackStyle property instead"), EditorBrowsable(EditorBrowsableState.Never)]
        public ItemStyleMapper BackgroundStyle
		{
			get {return m_BackgroundStyle;}
		}

		/// <summary>
		/// Gets or sets whether clicking the header of the control expands the item.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),DefaultValue(true),Category("Behavior"),Description("Determines whether clicking the header of the control expands the item.")]
		public bool HeaderExpands
		{
			get {return m_HeaderExpands;}
			set {m_HeaderExpands=value;}
		}

		/// <summary>
		/// Applies the stock style to the object.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),Category("Style"),Description("Applies the stock style to the object."),DefaultValue(eExplorerBarStockStyle.Custom),DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public eExplorerBarStockStyle StockStyle
		{
			get {return m_StockStyle;}
			set
			{
				m_StockStyle=value;
				if(m_StockStyle==eExplorerBarStockStyle.Custom)
					return;
				BarFunctions.SetExplorerBarStyle(this,m_StockStyle);
				foreach(BaseItem item in this.SubItems)
				{
					if(item is ButtonItem)
						BarFunctions.SetExplorerBarStyle(item as ButtonItem,m_StockStyle);
				}
                if (m_StockStyle == eExplorerBarStockStyle.BlueSpecial || m_StockStyle == eExplorerBarStockStyle.OliveGreenSpecial || m_StockStyle == eExplorerBarStockStyle.SilverSpecial)
                    this.XPSpecialGroup = true;
                else if (m_StockStyle != eExplorerBarStockStyle.SystemColors)
                    this.XPSpecialGroup = false;
				this.Refresh();
			}
		}

		/// <summary>
		/// Applies new visual style to this the item and all of its sub-items.
		/// </summary>
		[System.ComponentModel.Browsable(false),DevCoBrowsable(false),System.ComponentModel.DefaultValue(eDotNetBarStyle.OfficeXP),System.ComponentModel.Category("Appearance"),System.ComponentModel.Description("Determines the display of the item when shown.")]
		public override eDotNetBarStyle Style
		{
			get{return base.Style;}
			set{base.Style=value;}
		}

		/// <summary>
		/// Gets or sets expand button border color.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),Category("Style"),Description("Specifies expand button border color.") ]
		public Color ExpandBorderColor
		{
			get {return m_ExpandBorderColor;}
			set
			{
				m_ExpandBorderColor=value;
				OnAppearanceChanged();
			}
		}
		[Browsable(false),DevCoBrowsable(false),EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeExpandBorderColor()
		{
			return (m_ExpandBorderColor!=Color.DarkGray && m_StockStyle==eExplorerBarStockStyle.Custom);
		}

		/// <summary>
		/// Gets or sets expand button back color.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),Category("Style"),Description("Specifies expand button back color.") ]
		public Color ExpandBackColor
		{
			get {return m_ExpandBackColor;}
			set
			{
				m_ExpandBackColor=value;
				OnAppearanceChanged();
			}
		}
		[Browsable(false),DevCoBrowsable(false),EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeExpandBackColor()
		{
			return (m_ExpandBackColor!=Color.White && m_StockStyle==eExplorerBarStockStyle.Custom);
		}

		/// <summary>
		/// Gets or sets expand button fore color.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),Category("Style"),Description("Specifies expand button fore color.") ]
		public Color ExpandForeColor
		{
			get {return m_ExpandForeColor;}
			set
			{
				m_ExpandForeColor=value;
				OnAppearanceChanged();
			}
		}
		[Browsable(false),DevCoBrowsable(false),EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeExpandForeColor()
		{
			return (m_ExpandForeColor!=SystemColors.ActiveCaption && m_StockStyle==eExplorerBarStockStyle.Custom);
		}

		/// <summary>
		/// Gets or sets hot expand button border color.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),Category("Style"),Description("Specifies hot expand button border color.") ]
		public Color ExpandHotBorderColor
		{
			get {return m_ExpandHotBorderColor;}
			set
			{
				m_ExpandHotBorderColor=value;
				OnAppearanceChanged();
			}
		}
		[Browsable(false),DevCoBrowsable(false),EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeExpandHotBorderColor()
		{
			return (m_ExpandHotBorderColor!=Color.DarkGray && m_StockStyle==eExplorerBarStockStyle.Custom);
		}

		/// <summary>
		/// Gets or sets hot expand button back color.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),Category("Style"),Description("Specifies hot expand button back color.") ]
		public Color ExpandHotBackColor
		{
			get {return m_ExpandHotBackColor;}
			set
			{
				m_ExpandHotBackColor=value;
				OnAppearanceChanged();
			}
		}
		[Browsable(false),DevCoBrowsable(false),EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeExpandHotBackColor()
		{
			return (m_ExpandHotBackColor!=Color.White && m_StockStyle==eExplorerBarStockStyle.Custom);
		}

		/// <summary>
		/// Gets or sets hot expand button fore color.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),Category("Style"),Description("Specifies hot expand button fore color.")]
		public Color ExpandHotForeColor
		{
			get {return m_ExpandHotForeColor;}
			set
			{
				m_ExpandHotForeColor=value;
				OnAppearanceChanged();
			}
		}
		[Browsable(false),DevCoBrowsable(false),EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeExpandHotForeColor()
		{
			return (m_ExpandHotForeColor!=SystemColors.ActiveCaption && m_StockStyle==eExplorerBarStockStyle.Custom);
		}

		/// <summary>
		/// Overloaded. Serializes the item and all sub-items into the XmlElement.
		/// </summary>
		/// <param name="ThisItem">XmlElement to serialize the item to.</param>
		protected internal override void Serialize(ItemSerializationContext context)
		{
			base.Serialize(context);

            System.Xml.XmlElement groupItem = context.ItemXmlElement;

			groupItem.SetAttribute("stockstyle",System.Xml.XmlConvert.ToString((int)m_StockStyle));
			groupItem.SetAttribute("expvisible",System.Xml.XmlConvert.ToString(m_ExpandButtonVisible));
			groupItem.SetAttribute("subitemsmargin",System.Xml.XmlConvert.ToString(m_SubItemsMargin));
			groupItem.SetAttribute("xpspecial",System.Xml.XmlConvert.ToString(m_XPSpecialGroup));
			
			groupItem.SetAttribute("expborder",BarFunctions.ColorToString(m_ExpandBorderColor));
			groupItem.SetAttribute("expbc",BarFunctions.ColorToString(m_ExpandBackColor));
			groupItem.SetAttribute("expfc",BarFunctions.ColorToString(m_ExpandForeColor));
			groupItem.SetAttribute("exphborder",BarFunctions.ColorToString(m_ExpandHotBorderColor));
			groupItem.SetAttribute("exphbc",BarFunctions.ColorToString(m_ExpandHotBackColor));
			groupItem.SetAttribute("exphfc",BarFunctions.ColorToString(m_ExpandHotForeColor));

			groupItem.SetAttribute("headerexp",System.Xml.XmlConvert.ToString(m_HeaderExpands));

			groupItem.SetAttribute("expanded",System.Xml.XmlConvert.ToString(m_Expanded));

			if(m_StockStyle==eExplorerBarStockStyle.Custom)
			{
				System.Xml.XmlElement style=groupItem.OwnerDocument.CreateElement("backstyle");
				groupItem.AppendChild(style);
				//m_BackgroundStyle.Serialize(style);
                SerializeElementStyle(m_BackStyle, style);
			}

			if(m_StockStyle==eExplorerBarStockStyle.Custom)
			{
				System.Xml.XmlElement style=groupItem.OwnerDocument.CreateElement("headerhotstyle");
				groupItem.AppendChild(style);
				//m_HeaderHotStyle.Serialize(style);
                SerializeElementStyle(m_TitleHotStyle, style);
			}

			if(m_StockStyle==eExplorerBarStockStyle.Custom)
			{
				System.Xml.XmlElement style=groupItem.OwnerDocument.CreateElement("headerstyle");
				groupItem.AppendChild(style);
				//m_HeaderStyle.Serialize(style);
                SerializeElementStyle(m_TitleStyle, style);
			}

			if(m_ImageIndex!=-1)
			{
				groupItem.SetAttribute("imageindex",System.Xml.XmlConvert.ToString(m_ImageIndex));
			}
			else if(m_Image!=null)
			{
                System.Xml.XmlElement image=groupItem.OwnerDocument.CreateElement("image");
				groupItem.AppendChild(image);
                BarFunctions.SerializeImage(m_Image,image);
			}
		}

        private void SerializeElementStyle(ElementStyle style, System.Xml.XmlElement xmlElement)
        {
            ElementSerializer.Serialize(style, xmlElement);
        }

        private void DeserializeElementStyle(ElementStyle style, System.Xml.XmlElement xmlElement)
        {
            ElementSerializer.Deserialize(style, xmlElement);
        }

		/// <summary>
		/// Overloaded. Deserializes the Item from the XmlElement.
		/// </summary>
		/// <param name="ItemXmlSource">Source XmlElement.</param>
		public override void Deserialize(ItemSerializationContext context)
		{
			base.Deserialize(context);

            System.Xml.XmlElement groupItem = context.ItemXmlElement;

			m_StockStyle=(eExplorerBarStockStyle)System.Xml.XmlConvert.ToInt32(groupItem.GetAttribute("stockstyle"));
			m_ExpandButtonVisible=System.Xml.XmlConvert.ToBoolean(groupItem.GetAttribute("expvisible"));
			m_SubItemsMargin=System.Xml.XmlConvert.ToInt32(groupItem.GetAttribute("subitemsmargin"));
			m_XPSpecialGroup=System.Xml.XmlConvert.ToBoolean(groupItem.GetAttribute("xpspecial"));

		
			m_ExpandBorderColor=BarFunctions.ColorFromString(groupItem.GetAttribute("expborder"));
			m_ExpandBackColor=BarFunctions.ColorFromString(groupItem.GetAttribute("expbc"));
			m_ExpandForeColor=BarFunctions.ColorFromString(groupItem.GetAttribute("expfc"));
			m_ExpandHotBorderColor=BarFunctions.ColorFromString(groupItem.GetAttribute("exphborder"));
			m_ExpandHotBackColor=BarFunctions.ColorFromString(groupItem.GetAttribute("exphbc"));
			m_ExpandHotForeColor=BarFunctions.ColorFromString(groupItem.GetAttribute("exphfc"));
			
			m_HeaderExpands=System.Xml.XmlConvert.ToBoolean(groupItem.GetAttribute("headerexp"));

			foreach(System.Xml.XmlElement xmlElem in groupItem.ChildNodes)
			{
				switch(xmlElem.Name)
				{
					case "backstyle":
						DeserializeElementStyle(m_BackStyle,xmlElem);
						break;
					case "headerhotstyle":
                        DeserializeElementStyle(m_TitleHotStyle, xmlElem);
						break;
					case "headerstyle":
                        DeserializeElementStyle(m_TitleStyle, xmlElem);
						break;
					case "image":
						m_Image=BarFunctions.DeserializeImage(xmlElem);
						break;
				}
			}

			if(groupItem.HasAttribute("imageindex"))
			{
				this.ImageIndex=System.Xml.XmlConvert.ToInt32(groupItem.GetAttribute("imageindex"));
			}

			this.RefreshImageSize();
			
			this.Expanded=System.Xml.XmlConvert.ToBoolean(groupItem.GetAttribute("expanded"));
		}

		protected internal override void OnItemAdded(BaseItem item)
		{
			NeedRecalcSize=true;
			if(item is ButtonItem)
				((ButtonItem)item)._FitContainer=m_WordWrapSubItems;
			base.OnItemAdded(item);
			if(!this.Expanded)
			{
				if(!item.Displayed)
					item.Displayed=true;
				item.Displayed=false;
			}
			if(item is ButtonItem)
			{
				ButtonItem btn=item as ButtonItem;
				btn.ThemeAware=false;
				if(btn.ForeColor.IsEmpty && btn.HotForeColor.IsEmpty)
					BarFunctions.SetExplorerBarStyle(item as ButtonItem,m_StockStyle);
				//btn.PopupSide=ePopupSide.Left;
			}
			item.NeedRecalcSize=true;

			if(this.DesignMode)
				this.Refresh();
		}

		protected internal override void OnBeforeItemRemoved(BaseItem item)
		{
			if(item is ButtonItem)
				((ButtonItem)item)._FitContainer=false;
			base.OnBeforeItemRemoved(item);
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

        /// <summary>
        /// Gets or sets a value indicating whether the item is enabled.
        /// </summary>
        [Browsable(false), DevCoBrowsable(false), DefaultValue(true), Category("Behavior"), Description("Indicates whether is item enabled.")]
        public override bool Enabled
        {
            get { return base.Enabled; }
            set { base.Enabled = value; }
        }

		[System.ComponentModel.Browsable(true),DevCoBrowsable(false),System.ComponentModel.Editor("DevComponents.DotNetBar.Design.ExplorerBarGroupItemEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(System.Drawing.Design.UITypeEditor)),System.ComponentModel.Category("Data"),System.ComponentModel.Description("Collection of sub items."),System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Content)]
		public override SubItemsCollection SubItems
		{
			get
			{
				return base.SubItems;
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
				Rectangle r=m_PanelRect;
				r.Offset(m_Rect.X,m_Rect.Y);
				if(!r.Contains(ctrl.PointToClient(System.Windows.Forms.Control.MousePosition)))
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

			if(!this.Expanded)
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
					return owner.ImagesMedium;
				return null;
			}
		}

		/// <summary>
		/// Specifies the image.
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
			}
		}

		/// <summary>
		/// Indicates whether XP themed special group colors are used for drawing.
		/// </summary>
		[System.ComponentModel.Browsable(true),DevCoBrowsable(true),System.ComponentModel.Category("Appearance"),System.ComponentModel.Description("Indicates whether XP themed special group colors are used for drawing."),System.ComponentModel.DefaultValue(false)]
		public bool XPSpecialGroup
		{
			get {return m_XPSpecialGroup;}
			set
			{
				m_XPSpecialGroup=value;
				this.Refresh();
			}
		}

		/// <summary>
		/// Gets or sets the margin in pixels between the edge of the container and the items contained inside of it. Default value is 4.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),Category("Appearance"),DefaultValue(4),Description("Indicates margin in pixels between the edge of the container and the items contained inside of it")]
		public int SubItemsMargin
		{
			get {return m_SubItemsMargin;}
			set {m_SubItemsMargin=value;}
		}

		/// <summary>
		/// Gets or sets whether text on sub items is wrapped on new line if it cannot fit the space available.
		/// </summary>
		[System.ComponentModel.Browsable(true),DevCoBrowsable(true),System.ComponentModel.Category("Behavior"),System.ComponentModel.Description("Indicates whether text on sub items is wrapped on new line if it cannot fit the space available."),System.ComponentModel.DefaultValue(true)]
		public bool WordWrapSubItems
		{
			get {return m_WordWrapSubItems;}
			set
			{
				m_WordWrapSubItems=value;
				foreach(BaseItem item in this.SubItems)
				{
					if(item is ButtonItem)
						((ButtonItem)item)._FitContainer=m_WordWrapSubItems;
				}
				NeedRecalcSize=true;
				OnAppearanceChanged();
			}
		}

		/// <summary>
		/// Specifies the index of the image if ImageList is used.
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
			}
		}

		//***********************************************
		// IDesignTimeProvider Implementation
		//***********************************************
        protected virtual InsertPosition GetContainerInsertPosition(Point pScreen, BaseItem dragItem)
        {
            return DesignTimeProviderContainer.GetInsertPosition(this, pScreen, dragItem);
        }
        InsertPosition IDesignTimeProvider.GetInsertPosition(Point pScreen, BaseItem dragItem)
        {
            return GetContainerInsertPosition(pScreen, dragItem);
        }
        void IDesignTimeProvider.DrawReversibleMarker(int iPos, bool Before)
        {
            DesignTimeProviderContainer.DrawReversibleMarker(this, iPos, Before);
        }
        void IDesignTimeProvider.InsertItemAt(BaseItem objItem, int iPos, bool Before)
        {
            DesignTimeProviderContainer.InsertItemAt(this, objItem, iPos, Before);
        }
        //InsertPosition IDesignTimeProvider.GetInsertPosition(Point pScreen, BaseItem DragItem)
        //{
        //    InsertPosition objInsertPos=null;
        //    System.Windows.Forms.Control objContainer=this.ContainerControl as System.Windows.Forms.Control;
        //    if(objContainer==null || !this.CanCustomize)
        //        return null;
        //    Point pClient=objContainer.PointToClient(pScreen);
        //    Rectangle thisRect=this.DisplayRectangle;
        //    if(thisRect.Contains(pClient))
        //    {
        //        Rectangle r=thisRect;
        //        r.Size=m_PanelRect.Size;
        //        r.X+=m_PanelRect.Left;
        //        r.Y+=m_PanelRect.Top;
        //        if(r.Contains(pClient) && !this.Expanded)
        //        {
        //            this.Expanded=true;
        //            objInsertPos=new InsertPosition();
        //            objInsertPos.TargetProvider=this;
        //            if(this.SubItems.Count==0)
        //                objInsertPos.Position=-1;
        //            else
        //            {
        //                objInsertPos.Position=0;
        //                objInsertPos.Before=true;
        //            }
        //            return objInsertPos;
        //        }
				
        //        BaseItem objItem;
        //        // Check first inside any expanded items
        //        objItem=this.ExpandedItem();
        //        if(objItem!=null)
        //        {
        //            IDesignTimeProvider provider=objItem as IDesignTimeProvider;
        //            if(provider!=null)
        //            {
        //                objInsertPos=provider.GetInsertPosition(pScreen, DragItem);
        //                if(objInsertPos!=null)
        //                    return objInsertPos;
        //            }
        //        }

        //        for(int i=0;i<this.SubItems.Count;i++)
        //        {
        //            objItem=this.SubItems[i];
        //            r=objItem.DisplayRectangle;
        //            r.Inflate(2,2);
        //            if(objItem.Visible && objItem.Displayed && r.Contains(pClient))
        //            {
        //                if(objItem.SystemItem && this.SubItems.Count!=1)
        //                {
        //                    return null;
        //                }
        //                if(objItem==DragItem)
        //                    return new InsertPosition();
        //                objInsertPos=new InsertPosition();
        //                objInsertPos.TargetProvider=this;
        //                objInsertPos.Position=i;
        //                if(pClient.Y<=objItem.TopInternal+objItem.HeightInternal/2 || objItem.SystemItem)
        //                    objInsertPos.Before=true;

        //                // We need to collapse any expanded items that are not on this bar
        //                IOwner owner=this.GetOwner() as IOwner;
        //                if(owner!=null)
        //                {
        //                    BaseItem objExp=owner.GetExpandedItem();
        //                    if(objExp!=null)
        //                    {
        //                        while(objExp.Parent!=null)
        //                            objExp=objExp.Parent;
        //                        BaseItem objParent=objItem;
        //                        while(objParent.Parent!=null)
        //                            objParent=objParent.Parent;
        //                        if(objExp!=objParent)
        //                            owner.SetExpandedItem(null);
        //                    }
        //                }

        //                if(objItem is PopupItem && (objItem.SubItems.Count>0 || objItem.IsOnMenuBar))
        //                {
        //                    if(!objItem.Expanded)
        //                        objItem.Expanded=true;
        //                }
        //                else
        //                {
        //                    CollapseSubItems(this);
        //                }
        //                break;
        //            }
        //        }
        //        if(objInsertPos==null)
        //        {
        //            // Container is empty but it can contain the items
        //            if(this.SubItems.Count>1 && this.SubItems[this.SubItems.Count-1].SystemItem)
        //                objInsertPos=new InsertPosition(this.SubItems.Count-2,true,this);
        //            else
        //                objInsertPos=new InsertPosition(this.SubItems.Count-1,false,this);
        //        }
        //    }
        //    else
        //    {
        //        foreach(BaseItem objItem in this.SubItems)
        //        {
        //            if(objItem==DragItem)
        //                continue;
        //            IDesignTimeProvider provider=objItem as IDesignTimeProvider;
        //            if(provider!=null)
        //            {
        //                objInsertPos=provider.GetInsertPosition(pScreen, DragItem);
        //                if(objInsertPos!=null)
        //                    break;
        //            }
        //        }				
        //    }
        //    return objInsertPos;
        //}
        //void IDesignTimeProvider.DrawReversibleMarker(int iPos, bool Before)
        //{
        //    System.Windows.Forms.Control objCtrl=this.ContainerControl as System.Windows.Forms.Control;
        //    if(objCtrl==null)
        //        return;

        //    BaseItem objItem=null;
        //    if(iPos>=0)
        //        objItem=this.SubItems[iPos];
        //    Rectangle r, rl,rr;
        //    if(objItem!=null)
        //    {
        //        if(objItem.DesignInsertMarker!=eDesignInsertPosition.None)
        //            objItem.DesignInsertMarker=eDesignInsertPosition.None;
        //        else if(Before)
        //            objItem.DesignInsertMarker=eDesignInsertPosition.Before;
        //        else
        //            objItem.DesignInsertMarker=eDesignInsertPosition.After;
        //        return;
        //    }
        //    else
        //    {
        //        Rectangle rTmp=this.DisplayRectangle;
        //        rTmp.Inflate(-1,-1);
        //        rTmp.Offset(m_PanelRect.X,m_PanelRect.Bottom);
        //        r=new Rectangle(rTmp.Left+2,rTmp.Top+2,rTmp.Width-4,1);
        //        rl=new Rectangle(rTmp.Left+1,rTmp.Top,1,5);
        //        rr=new Rectangle(rTmp.Right-2,rTmp.Top,1,5);
        //    }
        //    //r.Location=objCtrl.PointToScreen(r.Location);
        //    //rl.Location=objCtrl.PointToScreen(rl.Location);
        //    //rr.Location=objCtrl.PointToScreen(rr.Location);
        //    //System.Windows.Forms.ControlPaint.DrawReversibleFrame(r,SystemColors.Control,System.Windows.Forms.FrameStyle.Thick);
        //    //System.Windows.Forms.ControlPaint.DrawReversibleFrame(rl,SystemColors.Control,System.Windows.Forms.FrameStyle.Thick);
        //    //System.Windows.Forms.ControlPaint.DrawReversibleFrame(rr,SystemColors.Control,System.Windows.Forms.FrameStyle.Thick);
        //}

        //void IDesignTimeProvider.InsertItemAt(BaseItem objItem, int iPos, bool Before)
        //{
        //    if(this.ExpandedItem()!=null)
        //    {
        //        this.ExpandedItem().Expanded=false;
        //    }
        //    if(!Before)
        //    {
        //        //objItem.BeginGroup=!objItem.BeginGroup;
        //        if(iPos+1>=this.SubItems.Count)
        //        {
        //            this.SubItems.Add(objItem,GetAppendPosition(this));
        //        }
        //        else
        //        {
        //            this.SubItems.Add(objItem,iPos+1);
        //        }
        //    }
        //    else
        //    {
        //        if(iPos>=this.SubItems.Count)
        //        {
        //            this.SubItems.Add(objItem, GetAppendPosition(this));
        //        }
        //        else
        //        {
        //            this.SubItems.Add(objItem,iPos);
        //        }
        //    }
        //    if(this.ContainerControl is Bar)
        //        ((Bar)this.ContainerControl).RecalcLayout();
        //    else if(this.ContainerControl is MenuPanel)
        //        ((MenuPanel)this.ContainerControl).RecalcSize();
        //    else
        //    {
        //        this.RecalcSize();
        //        this.Refresh();
        //    }
        //}

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
        [EditorBrowsable(EditorBrowsableState.Never)]
		public static void SetDesignTimeDefaults(ButtonItem button, eExplorerBarStockStyle e)
		{
			BarFunctions.SetExplorerBarStyle(button,e);
			button.Text="New Button";
			button.ButtonStyle = eButtonStyle.ImageAndText;
			button.ImagePosition=eImagePosition.Left;
			button.HotTrackingStyle=eHotTrackingStyle.None;
			button.HotFontUnderline=true;
			button.Cursor=System.Windows.Forms.Cursors.Hand;
		}

		/// <summary>
		/// Applies default appearance to ExplorerBarGroupItem.
		/// </summary>
		public void SetDefaultAppearance()
		{
			this.StockStyle = eExplorerBarStockStyle.SystemColors;
			this.BackStyle.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
			this.BackStyle.BorderLeft =eStyleBorderType.Solid;
			this.BackStyle.BorderRight =eStyleBorderType.Solid;
			this.BackStyle.BorderTop =eStyleBorderType.Solid;
			this.BackStyle.BorderBottom =eStyleBorderType.Solid;
			this.BackStyle.BorderLeftColor =SystemColors.Window;
			this.BackStyle.BorderRightColor =SystemColors.Window;
			this.BackStyle.BorderTopColor =SystemColors.Window;
			this.BackStyle.BorderBottomColor =SystemColors.Window;
			this.BackStyle.BorderLeftWidth = 1;
			this.BackStyle.BorderRightWidth = 1;
			this.BackStyle.BorderTopWidth = 1;
			this.BackStyle.BorderBottomWidth = 1;
			
			this.ExpandBackColor=SystemColors.Window;
			this.ExpandBorderColor = SystemColors.InactiveCaption;
			this.ExpandForeColor = SystemColors.Highlight;
			this.ExpandHotBackColor=SystemColors.Window;
			this.ExpandHotForeColor = SystemColors.ActiveCaption;
			this.ExpandHotBorderColor=SystemColors.ActiveCaption;

			this.TitleHotStyle.BackColor = SystemColors.Window;
			this.TitleHotStyle.BackColor2 = SystemColors.InactiveCaption;
			this.TitleHotStyle.TextColor = SystemColors.ActiveCaption;
			this.TitleStyle.BackColor = SystemColors.Window;
			this.TitleStyle.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground2;
			this.TitleStyle.TextColor = SystemColors.ControlText;

			this.ThemeAware=true;
		}
	}
}
