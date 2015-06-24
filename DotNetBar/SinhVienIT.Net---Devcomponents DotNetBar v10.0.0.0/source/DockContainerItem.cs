using System;
using System.Drawing;
using System.ComponentModel;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Item container for dockable windows.
	/// </summary>
    [System.ComponentModel.ToolboxItem(false), System.ComponentModel.DesignTimeVisible(false), Designer("DevComponents.DotNetBar.Design.DockContainerItemDesigner, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
	public class DockContainerItem:ImageItem
	{
		/// <summary>
		/// Occurs when container control needs to be assigned to the item.
		/// </summary>
		public event EventHandler ContainerLoadControl;
		public event ControlContainerItem.ControlContainerSerializationEventHandler ContainerControlSerialize;
		public event ControlContainerItem.ControlContainerSerializationEventHandler ContainerControlDeserialize;

		private System.Windows.Forms.Control m_Control=null;
		private bool m_MouseOver=false;
		private bool m_InternalResize=false;
		private Size m_MinimumSize=new Size(32,32);
		private Size m_DefaultFloatingSize=new Size(128,128);

		private System.Drawing.Image m_Image=null;
		private int m_ImageIndex=-1;
		private System.Drawing.Image m_TabImage=null;
		private System.Drawing.Icon m_Icon=null;
		
		private int m_MinFormClientSize=64;
		private int m_DelayedWidth=-1;
		private int m_DelayedHeight=-1;
        private eTabItemColor m_PredefinedTabColor = eTabItemColor.Default;

		/// <summary>
		/// Creates new instance of ControlContainerItem and assigns item name.
		/// </summary>
		public DockContainerItem():this("","") {}
		/// <summary>
		/// Creates new instance of ControlContainerItem and assigns item name.
		/// </summary>
		/// <param name="sName">Item name.</param>
		public DockContainerItem(string sName):this(sName,""){}
		/// <summary>
		/// Creates new instance of ControlContainerItem and assigns item name and item text.
		/// </summary>
		/// <param name="sName">Item name.</param>
		/// <param name="ItemText">Item text.</param>
		public DockContainerItem(string sName, string ItemText):base(sName,ItemText)
		{
			m_SupportedOrientation=eSupportedOrientation.Both;
			this.Stretch=true;
			this.CanCustomize=false;
			m_Displayed=true;  // need so the hosted control can be hidden...
            this.GlobalItem = false;
		}

		internal void InitControl()
		{
			EventArgs e=new EventArgs();
			if(ContainerLoadControl!=null)
				ContainerLoadControl(this,e);
			IOwnerItemEvents owner=this.GetIOwnerItemEvents();
			if(owner!=null)
				owner.InvokeContainerLoadControl(this,e);
		}

		/// <summary>
		/// Overriden. Returns the copy of the ControlContainerItem.
		/// </summary>
		/// <returns>Copy of the ControlContainerItem.</returns>
		public override BaseItem Copy()
		{
            DockContainerItem objCopy = new DockContainerItem(this.Name);
			this.CopyToItem(objCopy);
			return objCopy;
		}
		protected override void CopyToItem(BaseItem copy)
		{
			DockContainerItem objCopy=copy as DockContainerItem;
			base.CopyToItem(objCopy);
			objCopy.SetImageIndex(m_ImageIndex);
			if(m_Icon!=null)
				objCopy.Icon=m_Icon.Clone() as Icon;
			if(objCopy.Image!=null)
				objCopy.Image=m_Image.Clone() as Image;
			objCopy.ContainerLoadControl=this.ContainerLoadControl;
			objCopy.InitControl();
		}
        protected override void Dispose(bool disposing)
		{
            if (BarUtilities.DisposeItemImages && !this.DesignMode)
            {
                BarUtilities.DisposeImage(ref m_Image);
                BarUtilities.DisposeImage(ref m_Icon);
            }
			m_Control=null;
			base.Dispose(disposing);
		}
		protected internal override void Serialize(ItemSerializationContext context)
		{
			base.Serialize(context);
            System.Xml.XmlElement ThisItem = context.ItemXmlElement;
			System.Xml.XmlElement xmlElem=null, xmlElem2=null;
			// Serialize Images
			if(m_Image!=null || m_ImageIndex>=0)
			{
				xmlElem=ThisItem.OwnerDocument.CreateElement("images");
				ThisItem.AppendChild(xmlElem);

				if(m_ImageIndex>=0)
					xmlElem.SetAttribute("imageindex",System.Xml.XmlConvert.ToString(m_ImageIndex));

				if(m_Image!=null)
				{
					xmlElem2=ThisItem.OwnerDocument.CreateElement("image");
					xmlElem2.SetAttribute("type","default");
					xmlElem.AppendChild(xmlElem2);
					BarFunctions.SerializeImage(m_Image,xmlElem2);
				}
			}
			else if(m_Icon!=null)
			{
				xmlElem=ThisItem.OwnerDocument.CreateElement("images");
				ThisItem.AppendChild(xmlElem);

				xmlElem2=ThisItem.OwnerDocument.CreateElement("image");
				xmlElem2.SetAttribute("type","icon");
				xmlElem.AppendChild(xmlElem2);
				BarFunctions.SerializeIcon(m_Icon,xmlElem2);
			}

			if(m_MinimumSize.Width!=32 || m_MinimumSize.Height!=32)
			{
				ThisItem.SetAttribute("minw",m_MinimumSize.Width.ToString());
				ThisItem.SetAttribute("minh",m_MinimumSize.Height.ToString());
			}

			if(m_DefaultFloatingSize.Width!=128 || m_DefaultFloatingSize.Height!=128)
			{
				ThisItem.SetAttribute("defw",m_DefaultFloatingSize.Width.ToString());
				ThisItem.SetAttribute("defh",m_DefaultFloatingSize.Height.ToString());
			}

			if(m_MinFormClientSize!=64)
			{
				ThisItem.SetAttribute("csize",m_MinFormClientSize.ToString());
			}

            if(m_PredefinedTabColor!=eTabItemColor.Default)
                ThisItem.SetAttribute("PredefinedTabColor", System.Xml.XmlConvert.ToString(((int)m_PredefinedTabColor)));


			if(ContainerControlSerialize!=null)
				this.ContainerControlSerialize(this,new ControlContainerSerializationEventArgs(ThisItem));
			IOwnerItemEvents owner=this.GetIOwnerItemEvents();
			if(owner!=null)
				owner.InvokeContainerControlSerialize(this,new ControlContainerSerializationEventArgs(ThisItem));

		}
		public override void Deserialize(ItemSerializationContext context)
		{
			base.Deserialize(context);

            System.Xml.XmlElement ItemXmlSource = context.ItemXmlElement;

			// Load Images
			foreach(System.Xml.XmlElement xmlElem in ItemXmlSource.ChildNodes)
			{
				if(xmlElem.Name=="images")
				{
					if(xmlElem.HasAttribute("imageindex"))
						m_ImageIndex=System.Xml.XmlConvert.ToInt32(xmlElem.GetAttribute("imageindex"));

					foreach(System.Xml.XmlElement xmlElem2 in xmlElem.ChildNodes)
					{
						if(xmlElem2.GetAttribute("type")=="default")
						{
							m_Image=BarFunctions.DeserializeImage(xmlElem2);
							m_ImageIndex=-1;
						}
						else if(xmlElem2.GetAttribute("type")=="icon")
						{
							m_Icon=BarFunctions.DeserializeIcon(xmlElem2);
							m_ImageIndex=-1;
						}
					}
					break;
				}
			}

			if(ItemXmlSource.HasAttribute("minw"))
			{
				m_MinimumSize=new Size(System.Xml.XmlConvert.ToInt32(ItemXmlSource.GetAttribute("minw")),System.Xml.XmlConvert.ToInt32(ItemXmlSource.GetAttribute("minh")));
			}

			if(ItemXmlSource.HasAttribute("defw"))
			{
				m_DefaultFloatingSize=new Size(System.Xml.XmlConvert.ToInt32(ItemXmlSource.GetAttribute("defw")),System.Xml.XmlConvert.ToInt32(ItemXmlSource.GetAttribute("defh")));
			}

			if(ItemXmlSource.HasAttribute("csize"))
				m_MinFormClientSize=System.Xml.XmlConvert.ToInt32(ItemXmlSource.GetAttribute("csize"));
			else
				m_MinFormClientSize=64;

            if (ItemXmlSource.HasAttribute("PredefinedTabColor"))
                m_PredefinedTabColor = (eTabItemColor)System.Xml.XmlConvert.ToInt32(ItemXmlSource.GetAttribute("PredefinedTabColor"));
            else
                m_PredefinedTabColor = eTabItemColor.Default;

			InitControl();

            if (m_Control == null && context.DockControls != null && context.DockControls.ContainsKey(this.Name))
            {
                this.Control = context.DockControls[this.Name] as System.Windows.Forms.Control;
                context.DockControls.Remove(this.Name);
            }

			if(ContainerControlDeserialize!=null)
				this.ContainerControlDeserialize(this,new ControlContainerSerializationEventArgs(ItemXmlSource));
			IOwnerItemEvents owner=this.GetIOwnerItemEvents();
			if(owner!=null)
				owner.InvokeContainerControlDeserialize(this,new ControlContainerSerializationEventArgs(ItemXmlSource));
		}

		/// <summary>
		/// Gets or sets the reference to the contained control.
		/// </summary>
		[System.ComponentModel.Browsable(true),DevCoBrowsable(false),DefaultValue(null),Description("Indicates the control hosted on dockable window"),Category("Docking")]
		public System.Windows.Forms.Control Control
		{
			get
			{
				return m_Control;
			}
			set
			{
				if(m_Control!=null)
				{
					if(m_Control.Parent!=null)
					{
						if(!(this.DesignMode && this.Site!=null))
						{
							m_Control.Visible=false;
						}
						m_Control.Parent.Controls.Remove(m_Control);
					}
				}
				m_Control=value;
				if(m_Control!=null)
				{
					if(m_Control is PanelDockContainer)
						((PanelDockContainer)m_Control).DockContainerItem=this;

					if(m_Control is System.Windows.Forms.ListBox)
						((System.Windows.Forms.ListBox)m_Control).IntegralHeight=false;

					if(m_Control.Parent!=null)
						m_Control.Parent.Controls.Remove(m_Control);

					m_Control.Dock=System.Windows.Forms.DockStyle.None;

					// Check auto-size property
					PropertyDescriptorCollection props=TypeDescriptor.GetProperties(m_Control);
					if(props.Find("AutoSize",true)!=null)
					{
						props.Find("AutoSize",true).SetValue(m_Control,false);
					}

					if(!(this.DesignMode && this.Site!=null))
					{
						m_Control.Visible=false;
					}

					System.Windows.Forms.Control objCtrl=null;
					if(this.ContainerControl!=null)
					{
						objCtrl=this.ContainerControl as System.Windows.Forms.Control;
						if(objCtrl!=null)
						{
							objCtrl.Controls.Add(m_Control);
							if(this.DesignMode && this.Site!=null && m_Control.Visible && !this.Selected)
								m_Control.SendToBack();
						}
					}

					OnDisplayedChanged();
				}
			}
		}

		/// <summary>
		/// Returns category for this item. If item cannot be customzied using the
		/// customize dialog category is empty string.
		/// </summary>
		[Browsable(false),DevCoBrowsable(false),DefaultValue(""),Category("Design"),Description("Indicates item category used to group similar items at design-time.")]
		public override string Category
		{
			get {return base.Category;}
			set {base.Category=value;}
		}

		/// <summary>
		/// Gets or sets whether Click event will be auto repeated when mouse button is kept pressed over the item.
		/// </summary>
		[Browsable(false),DevCoBrowsable(false),DefaultValue(false),Category("Behavior"),Description("Gets or sets whether Click event will be auto repeated when mouse button is kept pressed over the item.")]
		public override bool ClickAutoRepeat
		{
			get {return base.ClickAutoRepeat;}
			set {base.ClickAutoRepeat=value;}
		}

		/// <summary>
		/// Gets or sets the auto-repeat interval for the click event when mouse button is kept pressed over the item.
		/// </summary>
		[Browsable(false),DevCoBrowsable(false),DefaultValue(600),Category("Behavior"),Description("Gets or sets the auto-repeat interval for the click event when mouse button is kept pressed over the item.")]
		public override int ClickRepeatInterval
		{
			get {return base.ClickRepeatInterval;}
			set {base.ClickRepeatInterval=value;}
		}

		/// <summary>
		/// Gets or sets item description. This description is displayed in
		/// Customize dialog to describe the item function in an application.
		/// </summary>
		[Browsable(false),DevCoBrowsable(false),DefaultValue(""),Category("Design"),Description("Indicates description of the item that is displayed during design.")]
		public override string Description
		{
			get {return base.Description;}
			set {base.Description=value;}
		}

		/// <summary>
		/// Gets or sets item alignment inside the container.
		/// </summary>
		[Browsable(false),DevCoBrowsable(false),DefaultValue(eItemAlignment.Near),Category("Appearance"),Description("Determines alignment of the item inside the container.")]
		public override eItemAlignment ItemAlignment
		{
			get {return base.ItemAlignment;}
			set {base.ItemAlignment=value;}
		}

		/// <summary>
		/// Gets or sets the collection of shortcut keys associated with the item.
		/// </summary>
		[Browsable(false),DevCoBrowsable(false),Category("Design"),Description("Indicates list of shortcut keys for this item."),System.ComponentModel.Editor("DevComponents.DotNetBar.Design.ShortcutsDesigner, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf",typeof(System.Drawing.Design.UITypeEditor)),System.ComponentModel.TypeConverter("DevComponents.DotNetBar.Design.ShortcutsConverter, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf"),System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		public override ShortcutsCollection Shortcuts
		{
			get {return base.Shortcuts;}				
			set {base.Shortcuts=value;}
		}

		/// <summary>
		/// Gets or sets whether item will display sub items.
		/// </summary>
		[Browsable(false),DevCoBrowsable(false),DefaultValue(true),Category("Behavior"),Description("Determines whether sub-items are displayed.")]
		public override bool ShowSubItems
		{
			get {return base.ShowSubItems;}
			set {base.ShowSubItems=value;}
		}

//		private void ControlResize(object sender, EventArgs e)
//		{
//			if(m_Painting || m_InternalResize)
//				return;
//			m_ControlSize=m_Control.Size;
//		}
        protected override void OnExternalSizeChange()
        {
            //if (this.DesignMode)
                ResizeControl();
            base.OnExternalSizeChange();
        }
		private void ResizeControl()
		{
			if(m_InternalResize)
				return;
			m_InternalResize=true;
			try
			{
				Rectangle r=this.DisplayRectangle;
                
				Size objImageSize=GetMaxImageSize();
				bool bOnMenu=this.IsOnMenu;
                if (bOnMenu && EffectiveStyle != eDotNetBarStyle.Office2000)
				{
					objImageSize.Width+=7;
					r.Width-=objImageSize.Width;
					r.X+=objImageSize.Width;
				}

				if(this.IsOnCustomizeMenu)
				{
                    if (EffectiveStyle != eDotNetBarStyle.Office2000)
					{
						r.X+=(objImageSize.Height+8);
						r.Width-=(objImageSize.Height+8);
					}
					else
					{
						r.X+=(objImageSize.Height+4);
						r.Width-=(objImageSize.Height+4);
					}
				}

				if(m_Control!=null)
				{
                    r.Inflate(-2, -2);
                    if (r.Width > 0 && r.Height > 0)
                    {
                        if (!m_Control.Size.Equals(r.Size))
                        {
                            m_Control.SuspendLayout();
                            m_Control.Size = r.Size;
                            m_Control.ResumeLayout();
                        }
                        Point loc = r.Location;
                        loc.Offset((r.Width - m_Control.Width) / 2, (r.Height - m_Control.Height) / 2);
                        if (loc.X != m_Control.Location.X || loc.Y != m_Control.Location.Y)
                            m_Control.Location = loc;
                    }
				}
			}
			finally
			{
				m_InternalResize=false;
			}

		}

		/// <summary>
		/// Overriden. Draws the item.
		/// </summary>
		/// <param name="g">Target Graphics object.</param>
		public override void Paint(ItemPaintArgs pa)
		{
			if(this.SuspendLayout)
				return;
			System.Drawing.Graphics g=pa.Graphics;
			if(m_Control!=null && m_Control.Visible!=this.Displayed && !m_Control.Visible)
				CustomizeChanged();  // Determine based on customize status				

			Rectangle r=this.DisplayRectangle;

			Size objImageSize=GetMaxImageSize();
			bool bOnMenu=this.IsOnMenu;

            if (bOnMenu && EffectiveStyle != eDotNetBarStyle.Office2000)
			{
				objImageSize.Width+=7;
				r.Width-=objImageSize.Width;
				r.X+=objImageSize.Width;
				if(this.IsOnCustomizeMenu)
					objImageSize.Width+=objImageSize.Height+8;
				// Draw side bar
				if(!pa.Colors.MenuSide2.IsEmpty)
				{
					System.Drawing.Drawing2D.LinearGradientBrush gradient=BarFunctions.CreateLinearGradientBrush(new Rectangle(m_Rect.Left,m_Rect.Top,objImageSize.Width,m_Rect.Height),pa.Colors.MenuSide,pa.Colors.MenuSide2,pa.Colors.MenuSideGradientAngle);
					g.FillRectangle(gradient,m_Rect.Left,m_Rect.Top,objImageSize.Width,m_Rect.Height);
					gradient.Dispose();
				}
				else
					g.FillRectangle(new SolidBrush(pa.Colors.MenuSide),m_Rect.Left,m_Rect.Top,objImageSize.Width,m_Rect.Height);
			}

			if(this.IsOnCustomizeMenu)
			{
                if (EffectiveStyle != eDotNetBarStyle.Office2000)
				{
					r.X+=(objImageSize.Height+8);
					r.Width-=(objImageSize.Height+8);
				}
				else
				{
					r.X+=(objImageSize.Height+4);
					r.Width-=(objImageSize.Height+4);
				}
			}

			//if(bOnMenu && this.Style==eDotNetBarStyle.OfficeXP)
			//{
			//	g.FillRectangle(new SolidBrush(pa.Colors.MenuBackground),r);
			//}

			// Draw text if needed
			if(m_Control==null)
			{
				string text=m_Text;
				if(text=="")
					text="Container";
				eTextFormat objStringFormat=GetStringFormat();
				Font objFont=this.GetFont();
				Rectangle rText=new Rectangle(r.X+8,r.Y,r.Width,r.Height);
                if (EffectiveStyle == eDotNetBarStyle.Office2000)
				{
					TextDrawing.DrawString(g,text,objFont,SystemColors.ControlText,rText,objStringFormat);
				}
				else
				{
                    TextDrawing.DrawString(g,text, objFont, SystemColors.ControlText, rText, objStringFormat);
				}
				Size textSize=TextDrawing.MeasureString(g,text,objFont,0,objStringFormat);
				r.X+=(int)textSize.Width+8;
				r.Width-=((int)textSize.Width+8);
			}
			else if(m_Control!=null)
			{
				ResizeControl();
			}

			if(this.IsOnCustomizeMenu && this.Visible)
			{
				// Draw check box if this item is visible
				Rectangle rBox=new Rectangle(m_Rect.Left,m_Rect.Top,m_Rect.Height,m_Rect.Height);
                if (EffectiveStyle != eDotNetBarStyle.Office2000)
					rBox.Inflate(-1,-1);
                BarFunctions.DrawMenuCheckBox(pa, rBox, EffectiveStyle, m_MouseOver);
			}

			if(this.Focused && this.DesignMode)
			{
				r=this.DisplayRectangle;
				r.Inflate(-1,-1);
				DesignTime.DrawDesignTimeSelection(g,r,pa.Colors.ItemDesignTimeBorder);
			}
		}
		/// <summary>
		/// Overriden. Recalculates the size of the item.
		/// </summary>
		public override void RecalcSize()
		{
			if(this.SuspendLayout)
				return;

			bool bOnMenu=this.IsOnMenu;

			if(m_Control==null && !this.DesignMode)
				InitControl();

			if(m_Control==null)
			{
				// Default Height
				if(this.Parent!=null && this.Parent is ImageItem)
					m_Rect.Height=((ImageItem)this.Parent).SubItemsImageSize.Height+4;
				else
					m_Rect.Height=this.SubItemsImageSize.Height+4;

                if (EffectiveStyle != eDotNetBarStyle.Office2000)
				{
					if(m_Control!=null && m_Rect.Height<(m_Control.Height+2))
						m_Rect.Height=m_Control.Height+2;
				}
				else
				{
					if(m_Control!=null && m_Rect.Height<(m_Control.Height+2))
						m_Rect.Height=m_Control.Height+2;
				}
			}
			else
				m_Rect.Height=m_MinimumSize.Height+4;
                
			// Default width
			//if(m_Control!=null)
				m_Rect.Width=m_MinimumSize.Width+4;
			//else
			//	m_Rect.Width=64+4;

			// Calculate Item Height
			if(m_Control==null)
			{
				string text=m_Text;
				if(text=="")
					text="Container";
				System.Windows.Forms.Control objCtrl=this.ContainerControl as System.Windows.Forms.Control;
				if(objCtrl!=null && IsHandleValid(objCtrl))
				{
					Graphics g=BarFunctions.CreateGraphics(objCtrl);
                    try
                    {
                        Size textSize = TextDrawing.MeasureString(g, text, GetFont(), 0, GetStringFormat());
                        if (textSize.Height > this.SubItemsImageSize.Height && textSize.Height > m_Rect.Height)
                            m_Rect.Height = (int)textSize.Height + 4;
                    }
                    finally
                    {
                        g.Dispose();
                    }
				}
			}

			Size objImageSize=GetMaxImageSize();
            if (this.IsOnMenu && EffectiveStyle != eDotNetBarStyle.Office2000)
			{
				// THis is side bar that will need to be drawn for DotNet style
				m_Rect.Width+=(objImageSize.Width+7);
			}

			if(this.IsOnCustomizeMenu)
				m_Rect.Width+=(objImageSize.Height+2);

			// Always call base implementation to reset resize flag
			base.RecalcSize();
		}
		
		protected internal override void OnContainerChanged(object objOldContainer)
		{	
			base.OnContainerChanged(objOldContainer);
			ParentControlChanged();
			//this.RefreshBarText();
		}

		private void ParentControlChanged()
		{
			if(m_Control!=null)
			{
				System.Windows.Forms.Control objCtrl=this.ContainerControl as System.Windows.Forms.Control;
				if(objCtrl!=m_Control.Parent)
				{
					bool bTextBoxMultiLine=false;
					if(m_Control.Parent!=null)
					{
						// Must set the multiline=false on text box becouse it was crashing otherwise under certain conditions
						if(m_Control is System.Windows.Forms.TextBox && ((System.Windows.Forms.TextBox)m_Control).Multiline)
						{
							bTextBoxMultiLine=true;
							((System.Windows.Forms.TextBox)m_Control).Multiline=false;
						}
						m_Control.Parent.Controls.Remove(m_Control);
					}

					if(objCtrl!=null)
					{
						objCtrl.Controls.Add(m_Control);
						if(m_Control is System.Windows.Forms.TextBox && bTextBoxMultiLine)
							((System.Windows.Forms.TextBox)m_Control).Multiline=true;
						if(m_Control.Visible)
							m_Control.Refresh();
					}
				}
			}
		}

		protected internal override void OnProcessDelayedCommands()
		{
			if(m_DelayedWidth>=0)
			{
				int v=m_DelayedWidth;
				m_DelayedWidth=-1;
				this.Width=v;
			}
			if(m_DelayedHeight>0)
			{
				int v=m_DelayedHeight;
				m_DelayedHeight=-1;
				this.Height=v;
			}
		}

		protected internal override void OnVisibleChanged(bool newValue)
		{
			if(m_Control!=null && !newValue)
			{
				if(!(this.DesignMode && this.Site!=null))
					m_Control.Visible=newValue;
				else
				{
					if(newValue)
						m_Control.SendToBack();
					else
						m_Control.BringToFront();
				}
			}
			Bar bar=this.ContainerControl as Bar;
			if(bar!=null && bar.LayoutType==eLayoutType.DockContainer)
			{
				bar.OnDockContainerVisibleChanged(this);
                if (bar.AutoHide)
                {
                    bar.RefreshAutoHidePanel();
                    bar.RefreshDockTab(true);
                }
                else
                    bar.RefreshDockTab(true);
			}
			base.OnVisibleChanged(newValue);
		}
		protected override void OnDisplayedChanged()
		{
			if(this.Displayed)
			{
				this.Refresh();
				Bar bar=this.ContainerControl as Bar;
                if (bar != null)
                {
                    //bar.MinClientSize = m_MinFormClientSize;
                    bar.SyncBarCaption();
                }
			}

			if(m_Control!=null && !(this.IsOnCustomizeMenu || this.IsOnCustomizeDialog))
			{
				ResizeControl();
				if(!(this.DesignMode && this.Site!=null))
					m_Control.Visible=this.Displayed;
				else
				{
					if(this.Displayed)
                        m_Control.BringToFront();
					else
						m_Control.SendToBack();
                    ResizeControl();
				}
			}
			base.OnDisplayedChanged();
		}

		protected override void OnIsOnCustomizeDialogChanged()
		{
			base.OnIsOnCustomizeDialogChanged();
			CustomizeChanged();
		}

		protected override void OnDesignModeChanged()
		{
			base.OnDesignModeChanged();
			CustomizeChanged();
		}

		protected override void OnIsOnCustomizeMenuChanged()
		{
			base.OnIsOnCustomizeMenuChanged();
			CustomizeChanged();
		}

		private void CustomizeChanged()
		{
			if(m_Control!=null)
			{
				if(this.DesignMode && this.Site!=null)
					return;

				if(this.IsOnCustomizeMenu || this.IsOnCustomizeDialog || this.DesignMode)
				{
					m_Control.Enabled=false;
				}
				else
				{
					if(!(this.DesignMode && this.Site!=null))
						m_Control.Visible=this.Displayed;
					m_Control.Enabled=true;
				}
			}
		}

		private Size GetMaxImageSize()
		{
			if(m_Parent!=null)
			{
				ImageItem objParentImageItem=m_Parent as ImageItem;
				if(objParentImageItem!=null)
					return objParentImageItem.SubItemsImageSize;
				else
					return this.ImageSize;
			}
			else
				return this.ImageSize;
		}
        private eTextFormat GetStringFormat()
		{
            eTextFormat format = eTextFormat.Default;
            format |= eTextFormat.SingleLine;
            format |= eTextFormat.EndEllipsis;
            format |= eTextFormat.VerticalCenter;
            return format;

            //StringFormat sfmt=BarFunctions.CreateStringFormat(); //new StringFormat(StringFormat.GenericDefault);
            //sfmt.HotkeyPrefix=System.Drawing.Text.HotkeyPrefix.Show;
            ////sfmt.FormatFlags=sfmt.FormatFlags & ~(sfmt.FormatFlags & StringFormatFlags.DisableKerning);
            //sfmt.FormatFlags=sfmt.FormatFlags | StringFormatFlags.NoWrap;
            //sfmt.Trimming=StringTrimming.EllipsisCharacter;
            //sfmt.Alignment=System.Drawing.StringAlignment.Near;
            //sfmt.LineAlignment=System.Drawing.StringAlignment.Center;

            //return sfmt;
		}

		/// <summary>
		/// Returns the Font object to be used for drawing the item text.
		/// </summary>
		/// <returns>Font object.</returns>
		protected virtual Font GetFont()
		{
			System.Windows.Forms.Control objCtrl=this.ContainerControl as System.Windows.Forms.Control;
			if(objCtrl!=null)
				return (Font)objCtrl.Font;
			return (Font)System.Windows.Forms.SystemInformation.MenuFont;
		}
		protected internal override bool IsAnyOnHandle(int iHandle)
		{
			bool bRet=base.IsAnyOnHandle(iHandle);
			if(!bRet && m_Control!=null && m_Control.Handle.ToInt32()==iHandle)
				bRet=true;
			return bRet;
		}
		protected override void OnEnabledChanged()
		{
			base.OnEnabledChanged();
			
			if(this.DesignMode && this.Site!=null)
				return;

			if(m_Control!=null)
				m_Control.Enabled=this.Enabled;
		}

//		[System.ComponentModel.Browsable(false),System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
//		public override void InternalMouseEnter()
//		{
//			base.InternalMouseEnter();
//			if(!m_MouseOver)
//			{
//				m_MouseOver=true;
//				this.Refresh();
//			}
//		}
//
//		[System.ComponentModel.Browsable(false),System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
//		public override void InternalMouseLeave()
//		{
//			base.InternalMouseLeave();
//			if(m_MouseOver)
//			{
//				m_MouseOver=false;
//				this.Refresh();
//			}
//		}
        [EditorBrowsable(EditorBrowsableState.Never)]
		public override void OnGotFocus()
		{
			base.OnGotFocus();
			if(m_Control==null)
				return;
			if(m_Control.Focused || this.IsOnCustomizeMenu || this.IsOnCustomizeDialog || this.DesignMode)
				return;
			m_Control.Focus();
		}

		/// <summary>
		/// Gets or sets a value indicating whether the item is visible.
		/// </summary>
		public override bool Visible
		{
			get
			{
				if(this.IsOnMenu)
					return false;
				Bar objTlb=this.ContainerControl as Bar;
				if(objTlb!=null)
				{
					if(objTlb.BarState==eBarState.Docked || objTlb.BarState==eBarState.Floating || objTlb.BarState==eBarState.AutoHide)
						return base.Visible;
					else
						return false;
				}
				return base.Visible;
			}
			set
			{
				base.Visible=value;
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
		/// Specifies the Tab image. Image specified here is used only on Tab when there are multiple dock containers on Bar.
        /// </summary>
        [Browsable(true),DevCoBrowsable(true),DefaultValue(null),Category("Appearance"),Description("Specifies the Tab image. Image specified here is used only on Tab when there are multiple dock containers on Bar.")]
		public System.Drawing.Image Image
		{
			get
			{
				return m_Image;
			}
			set
			{
				m_Image=value;
				m_TabImage=null;
				Bar bar=this.ContainerControl as Bar;
				if(bar!=null)
					bar.RefreshDockContainerItem(this);
			}
		}

		/// <summary>
		/// Specifies the index of the Tab image if ImageList is used. Image specified here is used only on Tab when there are multiple dock containers on Bar.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),DefaultValue(-1),Category("Appearance"),Description("Specifies the index of the Tab image if ImageList is used. Image specified here is used only on Tab when there are multiple dock containers on Bar."),System.ComponentModel.Editor("DevComponents.DotNetBar.Design.ImageIndexEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(System.Drawing.Design.UITypeEditor)),System.ComponentModel.TypeConverter(typeof(System.Windows.Forms.ImageIndexConverter))]
		public int ImageIndex
		{
			get
			{
				return m_ImageIndex;
			}
			set
			{
				if(m_ImageIndex!=value)
				{
					m_ImageIndex=value;
                    if(ShouldSyncProperties)
                        BarFunctions.SyncProperty(this, "ImageIndex");
					m_TabImage=null;
					Bar bar=this.ContainerControl as Bar;
					if(bar!=null)
						bar.RefreshDockContainerItem(this);
				}
			}
		}

		/// <summary>
		/// Specifies the Button icon. Icons support multiple image sizes and alpha blending.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),Category("Appearance"),Description("Specifies the Button icon. Icons support multiple image sizes and alpha blending."),DefaultValue(null)]
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

				m_TabImage=null;
				Bar bar=this.ContainerControl as Bar;
				if(bar!=null)
					bar.RefreshDockContainerItem(this);
			}
		}

        /// <summary>
        /// Gets or sets the predefined tab color. Default value is eTabItemColor.Default which means that default color is used.
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), DefaultValue(eTabItemColor.Default), Category("Style"), Description("Applies predefined color to tab.")]
        public eTabItemColor PredefinedTabColor
        {
            get { return m_PredefinedTabColor; }
            set
            {
				if(m_PredefinedTabColor!=value)
				{
					m_PredefinedTabColor = value;
					Bar bar=this.ContainerControl as Bar;
					if(bar!=null && bar.DockTabControl!=null && this.Parent!=null && this.Parent.SubItems.IndexOf(this)<bar.DockTabControl.Tabs.Count)
					{
						if(bar.DockTabControl.Tabs[this.Parent.SubItems.IndexOf(this)].AttachedItem==this)
							bar.DockTabControl.Tabs[this.Parent.SubItems.IndexOf(this)].PredefinedColor=m_PredefinedTabColor;
					}
					if (this.DesignMode)
					{
						System.Windows.Forms.Control c = this.ContainerControl as System.Windows.Forms.Control;
                        if(c!=null)
						    c.Refresh();
					}
				}
            }
        }

		internal System.Drawing.Image TabImage
		{
			get
			{
				if(m_TabImage==null)
				{
                    System.Drawing.Image img=this.GetImage();
					if(img!=null)
					{
						if(img.Width==16 && img.Height==16)
						{
							m_TabImage=(System.Drawing.Image)img.Clone();
						}
						else
						{
							m_TabImage=new Bitmap(img,16,16);
							Graphics g=Graphics.FromImage(m_TabImage);
							g.DrawImage(img,new Rectangle(0,0,16,16),0,0,16,16,GraphicsUnit.Pixel);
							g.Dispose();
						}
					}
				}

				return m_TabImage;
			}
		}

		private Image GetImage()
		{
			if(m_Image!=null)
				return m_Image;
			return GetImageFromImageList(m_ImageIndex);
		}

		internal void SetImageIndex(int iImageIndex)
		{
			m_ImageIndex=iImageIndex;
		}

		private Image GetImageFromImageList(int ImageIndex)
		{
			if(ImageIndex>=0)
			{
				IOwner owner=this.GetOwner() as IOwner;
				if(owner!=null)
				{
					if(owner.Images!=null && owner.Images.Images.Count>0 && ImageIndex<owner.Images.Images.Count)
						return owner.Images.Images[ImageIndex];
				}
			}
			return null;
		}

		/// <summary>
		/// Gets or sets whether tab that dock container item is on is selected.
		/// </summary>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool Selected
		{
			get
			{
				if(this.Visible)
				{
					Bar bar= this.ContainerControl as Bar;
					if(bar!=null)
					{
						return (bar.SelectedDockContainerItem == this);
					}
				}
				return false;
			}
			set
			{
				if(this.Visible)
				{
					Bar bar= this.ContainerControl as Bar;
					if(bar!=null)
					{
						bar.SelectedDockContainerItem = this;
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets the width of the item in pixels.
		/// </summary>
		[Browsable(false),DevCoBrowsable(false),DefaultValue(0),Category("Layout"),Description("Indicates the width of the item in pixels."), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int Width
		{
			get
			{
				return this.DisplayRectangle.Width;
			}
			set
			{
				if(value<m_MinimumSize.Width)
					value=m_MinimumSize.Width;
				if(value>=m_MinimumSize.Width)
				{
					Bar bar=this.ContainerControl as Bar;
					GenericItemContainer parent=this.Parent as GenericItemContainer;
					if(parent!=null && parent.SystemContainer && bar!=null && bar.LayoutType==eLayoutType.DockContainer && bar.Stretch)
					{
						bar.OnDockContainerWidthChanged(this,value);
						m_DelayedWidth=-1;
					}
					else
						m_DelayedWidth=value;
				}
			}
		}

		/// <summary>
		/// Gets or sets the height of the item in pixels.
		/// </summary>
		[Browsable(false),DevCoBrowsable(true),DefaultValue(0),Category("Layout"),Description("Indicates height of the item in pixels."), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int Height
		{
			get
			{
				return this.DisplayRectangle.Height;
			}
			set
			{
				if(value<m_MinimumSize.Height)
					value=m_MinimumSize.Height;
				if(value>=m_MinimumSize.Height)
				{
					Bar bar=this.ContainerControl as Bar;
					GenericItemContainer parent=this.Parent as GenericItemContainer;
					if(parent!=null && parent.SystemContainer && bar!=null && bar.LayoutType==eLayoutType.DockContainer && bar.Stretch)
					{
						bar.OnDockContainerHeightChanged(this,value);
						m_DelayedHeight=-1;
					}
					else
                        m_DelayedHeight=value;
				}
			}
		}

		/// <summary>
		/// Gets or sets the minimum size of the item. When used please note that layout logic for dockable windows expects that
        /// all DockContainerItems that are in particular docking side have exact same minimum size. When setting this property it is
        /// best to set the same value for all DockContainerItem instances you create.
		/// </summary>
		[Browsable(false),DevCoBrowsable(false),Category("Layout"),Description("Gets or sets the minimum size of the item.")]
		public System.Drawing.Size MinimumSize
		{
			get
			{
				return m_MinimumSize;
			}
			set
			{
				m_MinimumSize=value;
				if(m_MinimumSize.Width>m_DefaultFloatingSize.Width)
					m_DefaultFloatingSize.Width=m_MinimumSize.Width;
				if(m_MinimumSize.Height>m_DefaultFloatingSize.Height)
					m_DefaultFloatingSize.Height=m_MinimumSize.Height;
				Bar bar=this.ContainerControl as Bar;
				if(bar!=null)
					bar.OnDockContainerMinimumSizeChanged(this);
				if(m_MinimumSize.Width>this.Width)
					this.Width=m_MinimumSize.Width;
				if(m_MinimumSize.Height>this.Height)
					this.Height=m_MinimumSize.Height;
			}
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeMinimumSize()
		{
			if(m_MinimumSize.Width!=32 || m_MinimumSize.Height!=32)
				return true;
			return false;
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		public void ResetMinimumSize()
		{
			this.MinimumSize = new Size(32,32);
		}

		/// <summary>
		/// Gets or sets the default floating size of the Bar that is containing this item.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),System.ComponentModel.Category("Layout"),System.ComponentModel.Description("Gets or sets the default floating size of the Bar that is containing this item.")]
		public System.Drawing.Size DefaultFloatingSize
		{
			get
			{
				return m_DefaultFloatingSize;
			}
			set
			{
				if(value.Width<m_MinimumSize.Width)
					value.Width=m_MinimumSize.Width;
				if(value.Height<m_MinimumSize.Height)
					value.Height=m_MinimumSize.Height;
				m_DefaultFloatingSize=value;
			}
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeDefaultFloatingSize()
		{
			if(m_DefaultFloatingSize.Width!=128 || m_DefaultFloatingSize.Height!=128)
				return true;
			return false;
		}

		/// <summary>
		/// Gets or sets the minimum size of the form client area that is tried to maintain when dockable window is resized.
		/// </summary>
		[Obsolete("This property is obsolete and it will be removed in next version"), EditorBrowsable(EditorBrowsableState.Never),Browsable(false),DevCoBrowsable(false),System.ComponentModel.DefaultValue(64),System.ComponentModel.Category("Layout"),System.ComponentModel.Description("Gets or sets the minimum size of the form client area that is tried to maintain when dockable window is resized."), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int MinFormClientSize
		{
			get {return m_MinFormClientSize;}
			set
			{
				m_MinFormClientSize=value;
                //if(this.Displayed)
                //{
                //    Bar bar=this.ContainerControl as Bar;
                //    if(bar!=null)
                //        bar.MinClientSize=m_MinFormClientSize;
                //}
			}
		}

		/// <summary>
		/// Gets or sets whether the item expands automatically to fill out the remaining space inside the container. Applies to Items on stretchable, no-wrap Bars only.
		/// </summary>
		[Browsable(false),DevCoBrowsable(false),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override bool Stretch
		{
			get
			{
				return base.Stretch;
			}
			set
			{
				base.Stretch=value;
			}
		}

		/// <summary>
		/// Gets or sets whether item can be customized by end user.
		/// </summary>
		[Browsable(false),DevCoBrowsable(false),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),Category("Behavior"),Description("Indicates whether item can be customized by user.")]
		public override bool CanCustomize
		{
			get
			{
				return base.CanCustomize;
			}
			set
			{
				base.CanCustomize=value;
			}
		}


		/// <summary>
		/// Occurs after an item has been removed.
		/// </summary>
		/// <param name="item">Item being removed.</param>
		protected internal override void OnAfterItemRemoved(BaseItem item)
		{
			base.OnAfterItemRemoved(item);
			ParentControlChanged();
		}

		[System.ComponentModel.Browsable(false),System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override bool IsWindowed
		{
			get {return true;}
		}

		/// <summary>
		/// Occurs after text has changed.
		/// </summary>
		protected override void OnTextChanged()
		{
			base.OnTextChanged();
			if(this.ContainerControl is Bar /*&& !this.Displayed*/) // Refresh Tab Text even when dock container item is selected.
				((Bar)this.ContainerControl).RefreshDockContainerItem(this);
		}

        protected override void OnTooltipChanged()
        {
            base.OnTooltipChanged();
            if (this.ContainerControl is Bar) // Refresh Tab Tooltip even when dock container item is selected.
                ((Bar)this.ContainerControl).RefreshDockContainerItem(this);
        }

		/// <summary>
		/// Returns whether item is in design mode or not.
		/// </summary>
		[System.ComponentModel.Browsable(false),DevCoBrowsable(false)]
		public override bool DesignMode
		{
			get
			{
				return (base.DesignMode || this.Site!=null && this.Site.DesignMode);
			}
		}

		/// <summary>
		/// Occurs after item visual style has changed.
		/// </summary>
		protected override void OnStyleChanged()
		{
			base.OnStyleChanged();
			if(m_Control is PanelDockContainer)
			{
				if(!((PanelDockContainer)m_Control).UseCustomStyle)
					((PanelDockContainer)m_Control).ColorSchemeStyle=this.Style;
			}
		}

        /// <summary>
        /// Gets or sets whether item is global or not.
        /// This flag is used to propagate property changes to all items with the same name.
        /// Setting for example Visible property on the item that has GlobalItem set to true will
        /// set visible property to the same value on all items with the same name.
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), DefaultValue(false), Category("Behavior"), Description("Indicates whether certain global properties are propagated to all items with the same name when changed.")]
        public override bool GlobalItem
        {
            get { return base.GlobalItem; }
            set { base.GlobalItem = value; }
        }

        private eDockContainerClose _CanClose = eDockContainerClose.Inherit;
        /// <summary>
        /// Gets or sets the close button behavior on the host Bar. Default value is eDockContainerClose.Inherit which means that Bar.CanHide will control whether DockContainerItem can be closed.
        /// </summary>
        [Category("Behavior"), DefaultValue(eDockContainerClose.Inherit), Description("Specifies close button behavior on the host Bar. Default value is eDockContainerClose.Inherit which means that Bar.CanHide will control whether DockContainerItem can be closed.")]
        public eDockContainerClose CanClose
        {
            get { return _CanClose; }
            set
            {
                if (value != _CanClose)
                {
                    eDockContainerClose oldValue = _CanClose;
                    _CanClose = value;
                    OnCanCloseChanged(oldValue, value);
                }
            }
        }

        private void OnCanCloseChanged(eDockContainerClose oldValue, eDockContainerClose newValue)
        {
            Bar bar = this.ContainerControl as Bar;
            if(bar!=null)
                bar.DockContainerItemCanCloseChanged(this, oldValue, newValue);
        }
	}

    /// <summary>
    /// Specifies the behavior of the close button on host bar for the DockContainerItem.
    /// </summary>
    public enum eDockContainerClose
    {
        /// <summary>
        /// Closing of the bar is inherited from the host bar. Bar.CanHide property will control close button visibility.
        /// </summary>
        Inherit,
        /// <summary>
        /// Closing of the DockContainerItem is allowed.
        /// </summary>
        Yes,
        /// <summary>
        /// Closing of DockContainerItem is not allowed.
        /// </summary>
        No
    }
}
