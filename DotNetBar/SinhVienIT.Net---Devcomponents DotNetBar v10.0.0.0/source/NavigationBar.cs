using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Xml;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Represents Outlook 2003 like Navigation Bar.
	/// </summary>
    [ToolboxItem(true), System.Runtime.InteropServices.ComVisible(false), Designer("DevComponents.DotNetBar.Design.NavigationBarDesigner, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
	public class NavigationBar: BarBaseControl,ISupportInitialize
	{
		private NavigationBarContainer m_ItemContainer;
		private bool m_SplitterVisible=false;
		const int SPLITTER_SIZE=6;
		private Cursor m_SavedSplitterCursor=null;
		private bool m_SplitterMouseDown=false;
        /// <summary>
        /// Occurs after Options dialog which is used to customize control's content has closed by user using OK button.
        /// </summary>
        public event EventHandler OptionsDialogClosed;
		/// <summary>
		/// Default constructor.
		/// </summary>
		public NavigationBar():base()
		{
			m_ItemContainer=new NavigationBarContainer();
			m_ItemContainer.GlobalItem=false;
			m_ItemContainer.ContainerControl=this;
			m_ItemContainer.Stretch=false;
			m_ItemContainer.Displayed=true;
			m_ItemContainer.Style=eDotNetBarStyle.Office2003;
			m_ItemContainer.SetOwner(this);
			this.SetBaseItemContainer(m_ItemContainer);

			SetDesignTimeDefaults();
		}

        internal void InvokeOptionsDialogClosed()
        {
            OnOptionsDialogClosed(new EventArgs());
        }
        protected virtual void OnOptionsDialogClosed(EventArgs e)
        {
            EventHandler handler = OptionsDialogClosed;
            if (handler != null) handler(this, e);
        }
        protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
            if (this.Width == 0 || this.Height == 0)
                return;
			this.RecalcSize();
		}

		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			this.RecalcSize();
		}

		protected override void RecalcSize()
		{
			if(m_Initializing || !BarFunctions.IsHandleValid(this))
				return;

			base.RecalcSize();

			if(m_ItemContainer.IsRecalculatingSize)
				return;

			Rectangle r=this.GetItemContainerRectangle();
			if(m_ItemContainer.HeightInternal>0)
			{
				this.Height=m_ItemContainer.HeightInternal+r.Top+(this.ClientRectangle.Bottom-r.Bottom);
			}
		}

		/// <summary>
		/// Returns collection of items on a bar.
		/// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Browsable(false)]
		public SubItemsCollection Items
		{
			get
			{
				return m_ItemContainer.SubItems;
			}
		}

		/// <summary>
		/// Applies design-time defaults to control.
		/// </summary>
		public override void SetDesignTimeDefaults()
		{
			base.SetDesignTimeDefaults();

			ApplyColorSchemeDefaults();

			this.BackgroundStyle.BorderColor.ColorSchemePart=eColorSchemePart.PanelBorder;
			this.BackgroundStyle.Border=eBorderType.SingleLine;
		}

		private void ApplyColorSchemeDefaults()
		{
			// ColorScheme Defaults
            this.ColorScheme.ItemHotBorder = Color.Empty;
            this.ColorScheme.ItemBackground = this.ColorScheme.BarBackground;
            this.ColorScheme.ItemBackground2 = this.ColorScheme.BarBackground2;
            this.ColorScheme.ItemHotBorder = Color.Empty;
            this.ColorScheme.ItemPressedBorder = Color.Empty;
            this.ColorScheme.ItemCheckedBorder = Color.Empty;
            this.ColorScheme.ItemExpandedBackground = this.ColorScheme.ItemHotBackground;
            this.ColorScheme.ItemExpandedBackground2 = this.ColorScheme.ItemHotBackground2;
            this.ColorScheme.ItemExpandedShadow = Color.Empty;
            this.ColorScheme.ItemExpandedBorder = Color.Empty;
            this.ColorScheme.ResetChangedFlag();

			this.BackgroundStyle.ApplyColorScheme(this.ColorScheme);
		}

		protected override void OnSystemColorsChanged(EventArgs e)
		{
			base.OnSystemColorsChanged(e);
			Application.DoEvents();
			this.ColorScheme.Refresh();
			ApplyColorSchemeDefaults();
		}

		/// <summary>
		/// Gets or sets whether Configure Buttons button is visible.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),Category("Behavior"),Description("Indicates Configure Buttons button is visible."),System.ComponentModel.DefaultValue(true)]
		public bool ConfigureItemVisible
		{
			get {return m_ItemContainer.ConfigureItemVisible;}
			set
			{
				m_ItemContainer.ConfigureItemVisible=value;
				this.RecalcLayout();
			}
		}

		/// <summary>
		/// Gets or sets whether Show More Buttons and Show Fewer Buttons menu items are visible on Configure buttons menu.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),Category("Behavior"),Description("Indicates whether Show More Buttons and Show Fewer Buttons menu items are visible on Configure buttons menu."),System.ComponentModel.DefaultValue(true)]
		public bool ConfigureShowHideVisible
		{
			get {return m_ItemContainer.ConfigureShowHideVisible;}
			set
			{
				m_ItemContainer.ConfigureShowHideVisible=value;
				this.RecalcLayout();
			}
		}

		/// <summary>
		/// Gets or sets whether Navigation Pane Options menu item is visible on Configure buttons menu.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),Category("Behavior"),Description("Gets or sets whether Navigation Pane Options menu item is visible on Configure buttons menu."),System.ComponentModel.DefaultValue(true)]
		public bool ConfigureNavOptionsVisible
		{
			get {return m_ItemContainer.ConfigureNavOptionsVisible;}
			set
			{
				m_ItemContainer.ConfigureNavOptionsVisible=value;
				this.RecalcLayout();
			}
		}

		/// <summary>
		/// Gets or sets whether Navigation Pane Add/Remove Buttons menu item is visible on Configure buttons menu.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),Category("Behavior"),Description("Indicates whether Navigation Pane Add/Remove Buttons menu item is visible on Configure buttons menu."),System.ComponentModel.DefaultValue(true)]
		public bool ConfigureAddRemoveVisible
		{
			get {return m_ItemContainer.ConfigureAddRemoveVisible;}
			set
			{
				m_ItemContainer.ConfigureAddRemoveVisible=value;
				this.RecalcLayout();
			}
		}

		/// <summary>
		/// Gets or sets whether summary line is visible.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),Category("Behavior"),Description("Indicates whether summary line is visible."),System.ComponentModel.DefaultValue(true)]
		public bool SummaryLineVisible
		{
			get {return m_ItemContainer.SummaryLineVisible;}
			set
			{
				m_ItemContainer.SummaryLineVisible=value;
				this.RecalcLayout();
			}
		}

		/// <summary>
		/// Gets or sets the padding in pixels at the top portion of the item. Height of each item will be increased by padding amount.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),Category("Layout"),Description("Indicates the padding in pixels at the top portion of the item."),DefaultValue(4)]
		public int ItemPaddingTop
		{
			get {return m_ItemContainer.ItemPaddingTop;}
			set
			{
				m_ItemContainer.ItemPaddingTop=value;
				if(this.DesignMode)
					this.RecalcLayout();
			}
		}

		/// <summary>
		/// Gets or sets the padding in pixels for bottom portion of the item. Height of each item will be increased by padding amount.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),Category("Layout"),Description("Indicates the padding in pixels at the bottom of the item."),DefaultValue(4)]
		public int ItemPaddingBottom
		{
			get {return m_ItemContainer.ItemPaddingBottom;}
			set
			{
				m_ItemContainer.ItemPaddingBottom=value;
				if(this.DesignMode)
					this.RecalcLayout();
			}
		}

		/// <summary>
		/// Increases the size of the navigation bar if possible by showing more buttons on the top.
		/// </summary>
		public void ShowMoreButtons()
		{
			m_ItemContainer.ShowMoreButtons();
		}

		/// <summary>
		/// Reduces the size of the navigation bar if possible by showing fewer buttons on the top.
		/// </summary>
		public void ShowFewerButtons()
		{
			m_ItemContainer.ShowFewerButtons();
		}

		protected override Rectangle GetItemContainerRectangle()
		{
			Rectangle r=base.GetItemContainerRectangle();
			if(m_SplitterVisible)
			{
				r.Y+=SPLITTER_SIZE;
				r.Height-=SPLITTER_SIZE;
			}
			return r;
		}

		protected override void PaintControlBackground(ItemPaintArgs pa)
		{
			base.PaintControlBackground(pa);
			PaintSplitter(pa);
		}

		/// <summary>
		/// Returns items container.
		/// </summary>
		[Browsable(false)]
		public NavigationBarContainer ItemsContainer
		{
			get {return m_ItemContainer;}
		}

		/// <summary>
		/// Returns reference to currently checked button.
		/// </summary>
		[Browsable(false)]
		public ButtonItem CheckedButton
		{
			get
			{
				foreach(BaseItem item in m_ItemContainer.SubItems)
				{
                    if(item is ButtonItem && ((ButtonItem)item).Checked)
						return item as ButtonItem;
				}
				return null;
			}
		}

		/// <summary>
		/// Gets or sets whether images are automatically resized to size specified in ImageSizeSummaryLine when button is on the bottom summary line of navigation bar.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),Category("Appearance"),Description("Indicates whether images are automatically resized to size specified in ImageSizeSummaryLine when button is on the bottom summary line of navigation bar."),DefaultValue(true)]
		public bool AutoSizeButtonImage
		{
			get {return m_ItemContainer.AutoSizeButtonImage;}
			set {m_ItemContainer.AutoSizeButtonImage=value;}
		}

		/// <summary>
		/// Gets or sets size of the image that will be use to resize images to when button button is on the bottom summary line of navigation bar and AutoSizeButtonImage=true.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),Category("Appearance"),Description("Indicates size of the image that will be use to resize images to when button button is on the bottom summary line of navigation bar and AutoSizeButtonImage=true.")]
		public Size ImageSizeSummaryLine
		{
			get {return m_ItemContainer.ImageSizeSummaryLine;}
			set {m_ItemContainer.ImageSizeSummaryLine=value;}
		}
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeImageSizeSummaryLine()
		{
			return (m_ItemContainer.ImageSizeSummaryLine.Width!=16 || m_ItemContainer.ImageSizeSummaryLine.Height!=16);
		}
		
		private bool m_Initializing=false;
		void ISupportInitialize.BeginInit()
		{
			m_Initializing=true;
		}
		void ISupportInitialize.EndInit()
		{
			m_Initializing=false;
			this.RecalcLayout();
		}

		/// <summary>
		/// Saves current visual layout of navigation bar control to XML based file.
		/// </summary>
		/// <param name="fileName">File name to save layout to.</param>
		public void SaveLayout(string fileName)
		{
			System.Xml.XmlDocument xmlDoc=new System.Xml.XmlDocument();
			XmlElement parent=xmlDoc.CreateElement("navbarlayout");
			xmlDoc.AppendChild(parent);

			this.SaveLayout(parent);

			xmlDoc.Save(fileName);
		}

		/// <summary>
		/// Saves current visual layout of navigation bar control to XmlElement.
		/// </summary>
		/// <param name="xmlParent">XmlElement object that will act as a parent for the layout definition. Exact same element should be passed into the LoadLayout method to load the layout.</param>
		public void SaveLayout(XmlElement xmlParent)
		{
			if(this.Items.Count==0)
				return;
			XmlElement root=xmlParent.OwnerDocument.CreateElement("navigationbar");
			xmlParent.AppendChild(root);
			root.SetAttribute("height",this.Height.ToString());
			foreach(BaseItem item in this.Items)
			{
				if(item is ButtonItem && item.Name!="")
				{
					XmlElement xmlItem=xmlParent.OwnerDocument.CreateElement("item");
					root.AppendChild(xmlItem);
					xmlItem.SetAttribute("index",this.Items.IndexOf(item).ToString());
					xmlItem.SetAttribute("name",item.Name);
					xmlItem.SetAttribute("visible",XmlConvert.ToString(item.Visible));
					xmlItem.SetAttribute("checked",XmlConvert.ToString(((ButtonItem)item).Checked));
				}
			}
		}

		/// <summary>
		/// Gets or sets the navigation bar definition string.
		/// </summary>
		[Browsable(false),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string LayoutDefinition
		{
			get
			{
				System.Xml.XmlDocument xmlDoc=new System.Xml.XmlDocument();
				XmlElement parent=xmlDoc.CreateElement("navbarlayout");
				xmlDoc.AppendChild(parent);
				this.SaveLayout(parent);
				return xmlDoc.OuterXml;
			}
			set
			{
				System.Xml.XmlDocument xmlDoc=new System.Xml.XmlDocument();
				xmlDoc.LoadXml(value);
				LoadLayout(xmlDoc.FirstChild as XmlElement);
			}
		}

		/// <summary>
		/// Loads navigation bar layout that was saved using SaveLayout method. Note that this method must be called after all items are created and added to the control.
		/// </summary>
		/// <param name="fileName">File to load layout from.</param>
		public void LoadLayout(string fileName)
		{
			System.Xml.XmlDocument xmlDoc=new System.Xml.XmlDocument();
			xmlDoc.Load(fileName);
			this.LoadLayout(xmlDoc.FirstChild as XmlElement);
		}

		/// <summary>
		/// Loads navigation bar layout that was saved using SaveLayout method. Note that this method must be called after all items are created and added to the control.
		/// </summary>
		/// <param name="xmlParent">Parent XML element that is used to load layout from. Note that this must be the same element that was passed into the SaveLayout method.</param>
		public void LoadLayout(XmlElement xmlParent)
		{
			if(this.Items.Count==0)
				return;

			XmlElement root=xmlParent.FirstChild as XmlElement;
			if(root==null || root.Name!="navigationbar")
				throw new InvalidOperationException("xmlParent parameter in LoadLayout does not contain expected XML child note with name navigationbar. Invalid XML layout file.");
			
			try
			{
				((ISupportInitialize)this).BeginInit();
				this.Height=XmlConvert.ToInt32(root.GetAttribute("height"));
				foreach(XmlElement xmlItem in root.ChildNodes)
				{
					if(!this.Items.Contains(xmlItem.GetAttribute("name")))
						continue;
					BaseItem item=this.Items[xmlItem.GetAttribute("name")];
					if(item is ButtonItem)
					{
						ButtonItem button=item as ButtonItem;
						button.Checked=XmlConvert.ToBoolean(xmlItem.GetAttribute("checked"));
						button.Visible=XmlConvert.ToBoolean(xmlItem.GetAttribute("visible"));
						int i=XmlConvert.ToInt32(xmlItem.GetAttribute("index"));
						if(this.Items.IndexOf(item)!=i)
						{
							this.Items.RemoveAt(this.Items.IndexOf(item));
							this.Items.Insert(i,item);
						}
					}
				}
			}
			finally
			{
				((ISupportInitialize)this).EndInit();
			}
		}

		protected override bool ProcessMnemonic(char charCode)
		{
			string s="&"+charCode.ToString();
			s=s.ToLower();
			foreach(BaseItem item in this.Items)
			{
				string text=item.Text.ToLower();
				if(text.IndexOf(s)>=0)
				{
					if(item is ButtonItem)
					{
						((ButtonItem)item).Checked=true;
						return true;
					}
				}
			}
			return false;
		}

        /// <summary>
        /// Gets/Sets the visual style for items and color scheme.
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), Category("Appearance"), Description("Specifies the visual style of the control."), DefaultValue(eDotNetBarStyle.Office2003)]
        public eDotNetBarStyle Style
        {
            get
            {
                return m_ItemContainer.Style;
            }
            set
            {
                this.ColorScheme.Style = value;
                m_ItemContainer.Style = value;
                ApplyColorSchemeDefaults();
                this.RecalcLayout();
            }
        }

        private static RoundRectangleShapeDescriptor _ButtonShape = new RoundRectangleShapeDescriptor();
        internal IShapeDescriptor ButtonShape
        {
            get
            {
                return _ButtonShape;
            }
        }

		#region Splitter Support
		protected virtual void PaintSplitter(ItemPaintArgs pa)
		{
			if(m_SplitterVisible)
			{
				Rectangle r=this.GetSplitterRectangle();

				using(System.Drawing.Drawing2D.LinearGradientBrush brush=BarFunctions.CreateLinearGradientBrush(r,pa.Colors.PanelBackground,pa.Colors.PanelBackground2,90))
					pa.Graphics.FillRectangle(brush,r);
                
				using(SolidBrush brush=new SolidBrush(Color.White))
				{
					int x=r.X+(r.Width-34)/2;
					int y=r.Y+2;
					for(int i=0;i<9;i++)
					{
						pa.Graphics.FillRectangle(brush,x,y,2,2);
						x+=4;
					}
				}

				using(SolidBrush brush=new SolidBrush(Color.FromArgb(128,ControlPaint.Dark(pa.Colors.PanelBackground))))
				{
					int x=r.X+(r.Width-34)/2-1;
					int y=r.Y+1;
					for(int i=0;i<9;i++)
					{
						pa.Graphics.FillRectangle(brush,x,y,2,2);
						x+=4;
					}
				}

                DisplayHelp.DrawLine(pa.Graphics, r.X, r.Bottom - 1, r.Right, r.Bottom - 1, pa.Colors.PanelBorder, 1);
			}
		}

		protected virtual Rectangle GetSplitterRectangle()
		{
			Rectangle r=new Rectangle(this.ClientRectangle.X,this.ClientRectangle.Y,this.Width,SPLITTER_SIZE);
			if(this.BackgroundStyle.Border==eBorderType.SingleLine)
			{
				if((this.BackgroundStyle.BorderSide & eBorderSide.Top)==eBorderSide.Top)
					r.Y++;
				if((this.BackgroundStyle.BorderSide & eBorderSide.Right)==eBorderSide.Right)
					r.Width--;
				if((this.BackgroundStyle.BorderSide & eBorderSide.Left)==eBorderSide.Left)
				{
					r.X++;
					r.Width--;
				}
			}
			else if(this.BackgroundStyle.Border!=eBorderType.None)
			{
				if((this.BackgroundStyle.BorderSide & eBorderSide.Top)==eBorderSide.Top)
					r.Y+=2;
				if((this.BackgroundStyle.BorderSide & eBorderSide.Right)==eBorderSide.Right)
					r.Width-=2;
				if((this.BackgroundStyle.BorderSide & eBorderSide.Left)==eBorderSide.Left)
				{
					r.X+=2;
					r.Width-=2;
				}
			}
			return r;
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			SplitterMouseMove(e);
			if(!m_SplitterMouseDown)
				base.OnMouseMove(e);
		}
        [EditorBrowsable(EditorBrowsableState.Never)]
		public void SplitterMouseMove(MouseEventArgs e)
		{
			if(m_SplitterVisible && !m_SplitterMouseDown)
			{
				if(HitTestSplitter(e.X,e.Y))
				{
					if(m_SavedSplitterCursor==null || this.GetDesignMode())
					{
						if(m_SavedSplitterCursor==null)
						{
							if(this.GetDesignMode())
								m_SavedSplitterCursor=Cursor.Current;
							else
								m_SavedSplitterCursor=this.Cursor;
						}

						if(this.GetDesignMode())
							Cursor.Current=Cursors.SizeNS;
						else
							this.Cursor=Cursors.SizeNS;
					}
				}
				else if(m_SavedSplitterCursor!=null)
				{
					if(this.GetDesignMode())
						Cursor.Current=m_SavedSplitterCursor;
					else
						this.Cursor=m_SavedSplitterCursor;
					m_SavedSplitterCursor=null;
				}
			}

			if(m_SplitterMouseDown)
			{
				bool reduceSize=false;
				BaseItem firstDisplayed=m_ItemContainer.GetFirstVisibleItem();
				if(firstDisplayed==null)
					reduceSize=false;
				else if(e.Y>=firstDisplayed.DisplayRectangle.Bottom-SPLITTER_SIZE)
					reduceSize=true;

//				BaseItem referenceItem=m_ItemContainer.GetResizeReferenceItem(reduceSize);
//				if(referenceItem==null)
//					return;
//				int itemHeight=referenceItem.HeightInternal;
//				if(referenceItem is ButtonItem && m_ItemContainer.AutoSizeButtonImage && !m_ItemContainer.ImageSizeSummaryLine.IsEmpty && !((ButtonItem)referenceItem).ImageFixedSize.IsEmpty)
//				{
//					Size size=((ButtonItem)referenceItem).ImageFixedSize;
//					((ButtonItem)referenceItem).ImageFixedSize=Size.Empty;
//					referenceItem.RecalcSize();
//					itemHeight=referenceItem.HeightInternal;
//					itemHeight+=(m_ItemContainer.ItemPaddingBottom+m_ItemContainer.ItemPaddingTop);
//                    ((ButtonItem)referenceItem).ImageFixedSize=size;
//					referenceItem.RecalcSize();
//				}

				int itemHeight=m_ItemContainer.GetChangeSizeHeight(reduceSize);

				if(firstDisplayed!=null && e.Y>=firstDisplayed.DisplayRectangle.Bottom-SPLITTER_SIZE && this.GetItemContainerRectangle().Height>itemHeight+SPLITTER_SIZE)
				{
					//this.Height-=itemHeight;
					m_ItemContainer.ShowFewerButtons();
				}
				else if(e.Y<0 && Math.Abs(e.Y)>=itemHeight+SPLITTER_SIZE)
				{
//					itemHeight+=SPLITTER_SIZE;
//					this.Height+=itemHeight+1;
					m_ItemContainer.ShowMoreButtons();
				}
				else
					this.RecalcLayout();
			}
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			SplitterMouseLeave();
			base.OnMouseLeave(e);
		}
        [EditorBrowsable(EditorBrowsableState.Never)]
		public void SplitterMouseLeave()
		{
			if(m_SavedSplitterCursor!=null)
			{
				if(this.GetDesignMode())
					Cursor.Current=m_SavedSplitterCursor;
				else
					this.Cursor=m_SavedSplitterCursor;
				m_SavedSplitterCursor=null;
			}
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			SplitterMouseDown(e);

			if(!m_SplitterMouseDown)
				base.OnMouseDown(e);
		}
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void SplitterMouseDown(MouseEventArgs e)
		{
			if(this.Dock==DockStyle.Bottom && HitTestSplitter(e.X,e.Y))
			{
				this.Capture=true;
				m_SplitterMouseDown=true;
			}
		}
        [EditorBrowsable(EditorBrowsableState.Never)]
		public bool HitTestSplitter(int x, int y)
		{
			if(m_SplitterVisible)
			{
				Rectangle r=this.GetSplitterRectangle();
				if(r.Contains(x,y))
				{
					return true;
				}
			}
			return false;
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			SplitterMouseUp(e);
			base.OnMouseUp(e);
		}
        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool IsSplitterMouseDown
		{
			get {return m_SplitterMouseDown;}
		}
        [EditorBrowsable(EditorBrowsableState.Never)]
		public void SplitterMouseUp(MouseEventArgs e)
		{
			if(m_SplitterMouseDown)
			{
				m_SplitterMouseDown=false;
				this.Capture=false;

				if(m_SavedSplitterCursor!=null)
				{
					this.Cursor=m_SavedSplitterCursor;
					m_SavedSplitterCursor=null;
				}
			}
		}

		// Internal Splitter sizing logic assumes that control is docked to bottom and it will
		// change height ONLY. It is not capable of handling the generic splitter case!!!!!
		// OnMouseDown is hard-coded to check for Dock=Bottom to prevent any problems
		/// <summary>
		/// Indicates whether splitter on top of the navigation bar is visible. When activated splitter will let user change the height of the
		/// control to show fewer or more buttons. It is recommended to have navigation bar docked to bottom (Dock=Bottom) to maintain
		/// proper layout.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),Category("Layout"),Description("Indicates whether splitter on top of the navigation bar is visible."),DefaultValue(false)]
		public bool SplitterVisible
		{
			get{return m_SplitterVisible;}
			set
			{
				m_SplitterVisible=value;
				if(this.DesignMode)
					this.RecalcLayout();
			}
		}
		#endregion
	}
}
