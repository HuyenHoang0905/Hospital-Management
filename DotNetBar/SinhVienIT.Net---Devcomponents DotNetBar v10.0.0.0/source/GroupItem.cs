//using System;
//using System.ComponentModel;
//using System.Drawing;
//
//namespace DevComponents.DotNetBar
//{
//	/// <summary>
//	/// Represents container item which contains it's subitems and arranges them vertically.
//	/// </summary>
//	public class GroupItem:ImageItem,IContainerWordWrap
//	{
//		#region Private Variables
//		private bool m_WordWrapSubItems=false;
//		private int m_ContentLeftIndentation=0;
//		private int m_ContentRightIndentation=0;
//		private int m_ItemSpacing=0;
//		private Rectangle m_HeaderRect=Rectangle.Empty;
//
//		// Expand/Collapse button image
//		private Image m_ButtonExpand=null;
//		private Image m_ButtonCollapse=null;
//		private eItemAlignment m_ExpandButtonAlignment=eItemAlignment.Near;
//		private bool m_ExpandButtonVisible=true;
//
//		// Image support
//		private System.Drawing.Image m_Image=null;
//		private int m_ImageIndex=-1; // Image index if image from ImageList is used
//		private System.Drawing.Image m_ImageCachedIdx=null;
//		#endregion
//
//		#region Constructor
//		/// <summary>
//		/// Creates new instance of VerticalGroupItem.
//		/// </summary>
//		public GroupItem():this("","") {}
//		/// <summary>
//		/// Creates new instance of VerticalGroupItem and assigns the name to it.
//		/// </summary>
//		/// <param name="sItemName">Item name.</param>
//		public GroupItem(string sItemName):this(sItemName,"") {}
//		/// <summary>
//		/// Creates new instance of VerticalGroupItem and assigns the name and text to it.
//		/// </summary>
//		/// <param name="sItemName">Item name.</param>
//		/// <param name="ItemText">item text.</param>
//		public GroupItem(string sItemName, string ItemText):base(sItemName,ItemText)
//		{
//			m_IsContainer=true;
//			m_AllowOnlyOneSubItemExpanded=false;
////			try
////			{
////				m_HeaderStyle.Font=new Font(System.Windows.Forms.SystemInformation.MenuFont,FontStyle.Bold);
////				m_HeaderHotStyle.Font=new Font(System.Windows.Forms.SystemInformation.MenuFont,FontStyle.Bold);
////			}
////			catch(Exception e)
////			{
////				#if DEBUG
////				throw(e);
////				#endif
////				m_HeaderStyle.Font=System.Windows.Forms.SystemInformation.MenuFont.Clone() as Font;
////				m_HeaderHotStyle.Font=System.Windows.Forms.SystemInformation.MenuFont.Clone() as Font;
////			}
////
////			BarFunctions.SetExplorerBarStyle(this,m_StockStyle);
////
////			m_BackgroundStyle.VisualPropertyChanged+=new EventHandler(this.VisualPropertyChanged);
////			m_HeaderStyle.VisualPropertyChanged+=new EventHandler(this.VisualPropertyChanged);
////			m_HeaderHotStyle.VisualPropertyChanged+=new EventHandler(this.VisualPropertyChanged);
//		}
//		#endregion
//
//		#region Internal Implementation
//		/// <summary>
//		/// Returns copy of ExplorerBarGroupItem item.
//		/// </summary>
//		public override BaseItem Copy()
//		{
//			VerticalGroupItem objCopy=new VerticalGroupItem();
//			this.CopyToItem(objCopy);
//			return objCopy;
//		}
//		/// <summary>
//		/// Copies the VerticalGroupItem to different instance of the same type.
//		/// </summary>
//		/// <param name="copy">Destination VerticalGroupItem.</param>
//		protected override void CopyToItem(BaseItem copy)
//		{
//			VerticalGroupItem objCopy=copy as VerticalGroupItem;
//						
//			base.CopyToItem(objCopy);
//		}
//		private void VisualPropertyChanged(object sender, EventArgs e)
//		{
//			VisualPropertyChanged();
//		}
//		/// <summary>
//		/// Refreshes the display and applies any color scheme changes to the styles used by the item.
//		/// </summary>
//		internal void VisualPropertyChanged()
//		{
//			BarBaseControl eb=this.ContainerControl as BarBaseControl;
//			if(eb!=null)
//			{
//				ColorScheme cs=eb.ColorScheme;
//				m_BackgroundStyle.ApplyColorScheme(cs);
//				m_HeaderHotStyle.ApplyColorScheme(cs);
//				m_HeaderStyle.ApplyColorScheme(cs);
//			}
//			if(this.DesignMode)
//			{
//				m_NeedRecalcSize=true;
//				this.Refresh();
//			}
//		}
//		private Image GetImage()
//		{
//			if(m_Image!=null)
//				return m_Image;
//			if(m_ImageIndex>=0)
//			{
//				return GetImageFromImageList(m_ImageIndex);
//			}
//			return null;
//		}
//
//		private Image GetImageFromImageList(int ImageIndex)
//		{
//			if(ImageIndex>=0)
//			{
//				IOwner owner=null;
//				BarBaseControl bar=null;
//				if(owner==null) owner=this.GetOwner() as IOwner;
//				if(bar==null) bar=this.ContainerControl as BarBaseControl; 
//
//				if(owner!=null)
//				{
//					try
//					{
//						if(owner.ImagesMedium!=null)
//						{
//							if(m_ImageCachedIdx==null)
//								m_ImageCachedIdx=owner.ImagesMedium.Images[ImageIndex];
//							return m_ImageCachedIdx;
//						}
//					}
//					catch(Exception)
//					{
//						return null;
//					}
//				}
//			}
//			return null;
//		}
//
//		// Property Editor support for ImageIndex selection
//		[System.ComponentModel.Browsable(false),System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
//		public System.Windows.Forms.ImageList ImageList
//		{
//			get
//			{
//				IOwner owner=this.GetOwner() as IOwner;
//				if(owner!=null)
//					return owner.ImagesMedium;
//				return null;
//			}
//		}
//
//		/// <summary>
//		/// Specifies the image for the group.
//		/// </summary>
//		[System.ComponentModel.Browsable(true),DevCoBrowsable(true),System.ComponentModel.Category("Appearance"),System.ComponentModel.Description("The image that will be displayed on the face of the item."),System.ComponentModel.DefaultValue(null),System.ComponentModel.Editor(typeof(DevComponents.DotNetBar.ImageUITypeEditor), typeof(System.Drawing.Design.UITypeEditor)),System.ComponentModel.TypeConverter(typeof(ImageConverter2))]
//		public System.Drawing.Image Image
//		{
//			get
//			{
//				return m_Image;
//			}
//			set
//			{
//				m_NeedRecalcSize=true;
//				m_Image=value;
//				this.OnImageChanged();
//				this.Refresh();
//			}
//		}
//
//		/// <summary>
//		/// Specifies the index of the image if ImageList is used. Note that GroupItem is using the image list specified in ImagesMedium property of control it belongs to.
//		/// </summary>
//		[System.ComponentModel.Browsable(true),DevCoBrowsable(true),System.ComponentModel.Category("Appearance"),System.ComponentModel.Description("The image list image index of the image that will be displayed on the face of the item."),System.ComponentModel.Editor(typeof(DevComponents.Editors.ImageIndexEditor), typeof(System.Drawing.Design.UITypeEditor)),System.ComponentModel.TypeConverter(typeof(System.Windows.Forms.ImageIndexConverter)),System.ComponentModel.DefaultValue(-1)]
//		public int ImageIndex
//		{
//			get
//			{
//				return m_ImageIndex;
//			}
//			set
//			{
//				m_ImageCachedIdx=null;
//				if(m_ImageIndex!=value)
//				{
//					m_ImageIndex=value;
//					if(m_Name!="" && this.GlobalItem)
//					{
//						BarFunctions.SetProperty(this.GetOwner(),this.GetType(),m_Name,System.ComponentModel.TypeDescriptor.GetProperties(this)["ImageIndex"],m_ImageIndex);
//					}
//					if(m_Parent!=null)
//					{
//						OnImageChanged();
//						m_NeedRecalcSize=true;
//						this.Refresh();
//					}
//				}
//			}
//		}
//		#endregion
//
//		#region Sizing and Painting
//		public override void RecalcSize()
//		{
//			System.Windows.Forms.Control objCtrl=this.ContainerControl as System.Windows.Forms.Control;
//			if(!IsHandleValid(objCtrl))
//				return;
//			
//			bool bLeft=(objCtrl.RightToLeft==System.Windows.Forms.RightToLeft.No);
//			Graphics g=Graphics.FromHwnd(objCtrl.Handle);
//			SizeF objStringSize=SizeF.Empty;
//			System.Drawing.Size expandButtonSize=this.GetExpandButtonSize();
//			System.Drawing.Image image=this.GetImage();
//
//			string text=m_Text;
//			if(text=="")
//				text=" ";
//			int textArea=m_Rect.Width-m_Margin*2;
//			if(m_ExpandButtonVisible)
//				textArea-=expandButtonSize.Width;
//			if(image!=null)
//				textArea-=(image.Width+m_Margin*2);
//			if(textArea<=0)
//				textArea=1;
//			objStringSize=g.MeasureString(text,this.GetFont(),textArea,m_HeaderStyle.StringFormat);
//			g.Dispose();
//
//			m_Rect.Height=expandButtonSize.Height+m_Margin*2;
//			if(m_Rect.Height<(int)objStringSize.Height+m_Margin*2)
//				m_Rect.Height=(int)objStringSize.Height+m_Margin*2;
//			
//			if(image!=null)
//			{
//				int h=m_Rect.Height;
//				if(image.Height>m_Rect.Height)
//					m_Rect.Height=image.Height+m_Margin*2;
//			}
//			m_HeaderRect=new Rectangle(0,0,m_Rect.Width,m_Rect.Height);
//
//			if(m_ExpandButtonVisible)
//			{
//				if(m_ExpandButtonAlignment==eItemAlignment.Near && bLeft || m_ExpandButtonAlignment==eItemAlignment.Far && !bLeft)
//					m_ExpandButtonRect=new Rectangle(0,0,expandButtonSize.Width,m_HeaderRect.Height);
//				else
//					m_ExpandButtonRect=new Rectangle(m_HeaderRect.Right-expandButtonSize.Width,m_HeaderRect.Y,expandButtonSize.Width,m_HeaderRect.Height);
//			}
//			
//			if(this.Expanded)
//			{
//				if(m_SubItems!=null)
//				{
//					int iTop=m_Rect.Bottom+1;
//					int iLeft=m_Rect.Left+m_LeftIndentation;
//
//					int iIndex=-1;
//					foreach(BaseItem item in m_SubItems)
//					{
//						iIndex++;
//						if(!item.Visible)
//						{
//							item.Displayed=false;
//							continue;
//						}
//						item.WidthInternal=m_Rect.Width-(m_LeftIndentation+m_RightIndentation);
//						item.RecalcSize();
//						item.WidthInternal=m_Rect.Width-(m_LeftIndentation+m_RightIndentation);
//						if(item.BeginGroup)
//						{
//							iTop+=3;
//						}
//						item.TopInternal=iTop;
//						item.LeftInternal=iLeft;
//						iTop+=(item.HeightInternal+m_ItemSpacing);;
//						item.Displayed=true;
//					}
//					m_Rect.Height=iTop-m_Rect.Top+2;
//				}
//			}
//			else
//			{
//				foreach(BaseItem item in m_SubItems)
//				{
//					item.Displayed=false;
//				}
//			}
//
//			if(this.Expanded && this.DesignMode && this.SubItems.Count==0 && this.Parent!=null && this.Parent.SubItems.Count==1)
//			{
//				// Empty space to display instruction text that tells user what to do...
//				m_Rect.Height+=64;
//			}
//
//			base.RecalcSize();
//		}
//
//		private Size GetExpandButtonSize()
//		{
//			Size size=Size.Empty;
//			if(m_ButtonExpand!=null)
//				size=m_ButtonExpand.Size;
//			if(m_ButtonCollapse!=null)
//			{
//				if(m_ButtonCollapse.Height>size.Height)
//					size.Height=m_ButtonCollapse.Height;
//				if(m_ButtonCollapse.Width>size.Width)
//					size.Width=m_ButtonCollapse.Width;
//			}
//
//			return size;
//		}
//
//		#endregion
//
//		#region IContainerWordWrap
//		/// <summary>
//		/// Gets or sets whether items hosted inside of the container will word-wrap the text.
//		/// </summary>
//		[Browsable(true),DevCoBrowsable(true),DefaultValue(false),Category("Appearance"),Description("Indicates whether items hosted inside of the container will word-wrap the text.")]
//		public bool WordWrapSubItems
//		{
//			get {return m_WordWrapSubItems;}
//			set
//			{
//				m_WordWrapSubItems=value;
//				m_NeedRecalcSize=true;
//				if(this.DesignMode)
//				{
//					BarBaseControl bar=this.ContainerControl as BarBaseControl;
//					if(bar!=null)
//						bar.RecalcLayout();
//				}
//			}
//		}
//		#endregion
//	}
//}
