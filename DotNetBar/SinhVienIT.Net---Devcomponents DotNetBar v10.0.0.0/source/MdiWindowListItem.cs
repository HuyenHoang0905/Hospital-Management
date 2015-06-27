using System;
using System.Drawing;
using System.ComponentModel;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Summary description for MdiWindowListItem.
	/// </summary>
	[System.ComponentModel.ToolboxItem(false),System.ComponentModel.DesignTimeVisible(false)]
	public class MdiWindowListItem:ImageItem
	{
		private bool m_Initialized=false;
		private System.Collections.ArrayList m_MdiItems=new System.Collections.ArrayList();
		private int m_MaxCaptionLength=22;
		private bool m_ShowWindowIcons=true;
		private bool m_MdiNoFormActivateFlicker=true;
		private bool m_CreateMdiChildAccessKeys=false;

		public MdiWindowListItem():this("","") {}
		public MdiWindowListItem(string sName):this(sName,""){}
		public MdiWindowListItem(string sName, string ItemText):base(sName,ItemText)
		{
			m_SystemItem=true;
			m_Visible=false;
			this.IsAccessible=false;
		}

		[System.ComponentModel.Browsable(false),DevCoBrowsable(false),System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override bool Visible
		{
			get
			{
                return this.DesignMode && this.Site != null;
			}
			set{}
		}

        protected override void OnSiteChanged()
        {
            if (this.Site != null)
            {
                m_SystemItem = false;
                m_Visible = true;
            }
            else
            {
                m_SystemItem = true;
                m_Visible = false; 
            }
            base.OnSiteChanged();
        }

		/// <summary>
		/// Gets or sets whether the MDI Child Window Icons are displayed on items.
		/// </summary>
		[System.ComponentModel.Browsable(true),DevCoBrowsable(true),System.ComponentModel.Category("Appearance"),System.ComponentModel.Description("Specifies whether the MDI Child Window Icons are displayed on items.")]
		public bool ShowWindowIcons
		{
			get {return m_ShowWindowIcons;}
			set {m_ShowWindowIcons=value;}
		}

		public override BaseItem Copy()
		{
			MdiWindowListItem objCopy=new MdiWindowListItem(this.Name);
			this.CopyToItem(objCopy);
			return objCopy;
		}
		protected override void CopyToItem(BaseItem copy)
		{
			base.CopyToItem(copy);
		}

        protected override void Dispose(bool disposing)
		{
			if(m_MdiItems!=null && m_MdiItems.Count>0)
			{
				foreach(BaseItem item in m_MdiItems)
				{
					item.Tag=null;
					if(this.Parent!=null)
						this.Parent.SubItems.Remove(item);
				}
				m_MdiItems.Clear();
			}
			base.Dispose(disposing);
		}

		public override void Paint(ItemPaintArgs pa)
		{
            if (this.DesignMode)
            {
                Graphics g = pa.Graphics;
                TextDrawing.DrawString(g, (this.Name.Length > 0 ? this.Name : "MdiWindowListItem"), pa.Font, pa.Colors.ItemText, m_Rect, eTextFormat.Left | eTextFormat.VerticalCenter | eTextFormat.HorizontalCenter | eTextFormat.EndEllipsis);
                if (this.Focused)
                {
                    Rectangle r = m_Rect;
                    r.Inflate(-1, -1);
                    DesignTime.DrawDesignTimeSelection(g, r, pa.Colors.ItemDesignTimeBorder);
                }

                this.DrawInsertMarker(pa.Graphics);
            }
		}
		public override void RecalcSize()
		{
            if (this.DesignMode)
            {
                this.WidthInternal = 64;
                this.HeightInternal = 24;
            }
			base.RecalcSize();
		}

		/// <summary>
		/// Gets or sets maximum form caption length that will be displayed on each item. If caption length exceeds given value ... characters are added.
		/// </summary>
		[System.ComponentModel.Category("Behavior"),System.ComponentModel.Browsable(true),DevCoBrowsable(true),System.ComponentModel.DefaultValue(22),System.ComponentModel.Description("Indicates maximum form caption length that will be displayed on each item. If caption length exceeds given value ... characters are added.")]
		public int MaxCaptionLength
		{
			get {return m_MaxCaptionLength;}
			set
			{
				if(m_MaxCaptionLength!=value)
				{
					m_MaxCaptionLength=value;
					RefreshMdiList();
				}
			}
		}

		protected internal override void OnContainerChanged(object objOldContainer)
		{	
			base.OnContainerChanged(objOldContainer);
			if(this.ContainerControl==null)
				return;
			if(!m_Initialized)
				Initialize();
		}

		public override void Deserialize(ItemSerializationContext context)
		{
			base.Deserialize(context);
            System.Xml.XmlElement ItemXmlSource = context.ItemXmlElement;
			if(ItemXmlSource.HasAttribute("showicons"))
				m_ShowWindowIcons=System.Xml.XmlConvert.ToBoolean(ItemXmlSource.GetAttribute("showicons"));
			if(ItemXmlSource.HasAttribute("mdiaccesskey"))
				m_CreateMdiChildAccessKeys=System.Xml.XmlConvert.ToBoolean(ItemXmlSource.GetAttribute("mdiaccesskey"));
			if(!m_Initialized)
				Initialize();
		}
		protected internal override void Serialize(ItemSerializationContext context)
		{
			base.Serialize(context);
            System.Xml.XmlElement ThisItem = context.ItemXmlElement;
			if(!m_ShowWindowIcons)
                ThisItem.SetAttribute("showicons",System.Xml.XmlConvert.ToString(m_ShowWindowIcons));
			if(m_CreateMdiChildAccessKeys)
                ThisItem.SetAttribute("mdiaccesskey",System.Xml.XmlConvert.ToString(m_CreateMdiChildAccessKeys));
		}

        /// <summary>
        /// Initializes the item and connects it to the MDI form so it can process the events. You should not call this method directly since this process is
        /// automatically managed by the item.
        /// </summary>
        public void Initialize(System.Windows.Forms.Form mdiForm)
        {
            IOwner owner = this.GetOwner() as IOwner;
            if (owner == null) return;

            System.Windows.Forms.Form parentForm = mdiForm;
            
            System.Windows.Forms.MdiClient client = owner.GetMdiClient(parentForm);
            if (client == null)
            {
                parentForm.Load += new EventHandler(this.ParentFormLoaded);
                return;
            }

            client.ControlAdded += new System.Windows.Forms.ControlEventHandler(this.MdiFormAdded);
            client.ControlRemoved += new System.Windows.Forms.ControlEventHandler(this.MdiFormRemoved);
            parentForm.MdiChildActivate += new EventHandler(this.MdiFormActivated);
            m_Initialized = true;
            RefreshMdiList();
        }

        /// <summary>
        /// Initializes the item and connects it to the MDI form so it can process the events. You should not call this method directly since this process is
        /// automatically managed by the item.
        /// </summary>
		public void Initialize()
		{
			IOwner owner=this.GetOwner() as IOwner;
            if (owner == null || owner.ParentForm == null && !(owner is RibbonBar && ((RibbonBar)owner).IsOverflowRibbon) || m_Initialized)
				return;

            System.Windows.Forms.Form parentForm = owner.ParentForm;
            if (parentForm == null)
            {
                if (System.Windows.Forms.Form.ActiveForm != null && System.Windows.Forms.Form.ActiveForm.IsMdiChild)
                    parentForm = System.Windows.Forms.Form.ActiveForm;
                if (parentForm == null)
                    return;
            }

            Initialize(parentForm);
		}

        /// <summary>
        /// Gets whether item has been connected to the MDI form so it can process its events.
        /// </summary>
        [Browsable(false)]
        public bool IsInitialized
        {
            get { return m_Initialized; }
        }

		private void ParentFormLoaded(object sender, EventArgs e)
		{
			IOwner owner=this.GetOwner() as IOwner;
			if(owner==null || owner.ParentForm==null)
				return;

            owner.ParentForm.Load-=new EventHandler(this.ParentFormLoaded);

			if(!m_Initialized)
				Initialize();
		}

		private void RemoveEvents()
		{
			IOwner owner=this.GetOwner() as IOwner;
			if(owner==null || owner.ParentForm==null || !m_Initialized)
				return;

			System.Windows.Forms.MdiClient client=owner.GetMdiClient(owner.ParentForm);
			if(client==null)
				return;

			client.ControlAdded-=new System.Windows.Forms.ControlEventHandler(this.MdiFormAdded);
			client.ControlRemoved-=new System.Windows.Forms.ControlEventHandler(this.MdiFormRemoved);
			owner.ParentForm.MdiChildActivate-=new EventHandler(this.MdiFormActivated);
			m_Initialized=false;
		}

		[System.ComponentModel.Browsable(false),System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public void MdiFormAdded(object sender, System.Windows.Forms.ControlEventArgs e)
		{
			IOwner owner=this.GetOwner() as IOwner;
			if(owner==null || owner.ParentForm==null && !(owner is RibbonBar && ((RibbonBar)owner).IsOverflowRibbon) || !m_Initialized)
				return;

            System.Windows.Forms.Form parentForm = owner.ParentForm;
            if (parentForm == null)
            {
                parentForm = ((System.Windows.Forms.Control)sender).Parent as System.Windows.Forms.Form;
            }
			System.Windows.Forms.Form form=e.Control as System.Windows.Forms.Form;

			if(form==null)
			{
				RefreshMdiList();
				return;
			}
            
			int iInsertPosition=this.Parent.SubItems.IndexOf(this)+1;
			if(m_MdiItems.Count>0)
				iInsertPosition=this.Parent.SubItems.IndexOf((BaseItem)m_MdiItems[m_MdiItems.Count-1])+1;

			string text=GetFormText(form.Text,m_MdiItems.Count+1);
			ButtonItem btn=new ButtonItem("mdi-"+form.GetHashCode(),text);
			btn.ShouldSerialize=false;
			btn.ButtonStyle=eButtonStyle.ImageAndText;
			if(m_MdiItems.Count==0 && this.BeginGroup)
				btn.BeginGroup=true;

			btn.Tag=form;
            if (parentForm.ActiveMdiChild == form)
				btn.Checked=true;

			btn.Click+=new EventHandler(this.MdiWindowItemClick);
			btn.SetSystemItem(true);

			if(m_ShowWindowIcons && form.Icon!=null && form.ControlBox)
			{
				btn.Icon=form.Icon.Clone() as System.Drawing.Icon;
			}

			m_MdiItems.Add(btn);
			this.Parent.SubItems.Add(btn,iInsertPosition);
			btn.Visible=form.Visible;

			if(m_CreateMdiChildAccessKeys)
				RefreshMdiItemsText();

			if(this.ContainerControl is Bar)
				((Bar)this.ContainerControl).RecalcLayout();
            else if (this.ContainerControl is ItemControl)
                ((ItemControl)this.ContainerControl).RecalcLayout();

			form.TextChanged+=new EventHandler(this.FormTextChanged);
            form.VisibleChanged+=new EventHandler(this.FormVisibleChanged);
		}

		[System.ComponentModel.Browsable(false),System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public void MdiFormRemoved(object sender, System.Windows.Forms.ControlEventArgs e)
		{
			System.Windows.Forms.Form form=e.Control as System.Windows.Forms.Form;
			if(form==null)
			{
				RefreshMdiList();
				return;
			}

            form.TextChanged -= new EventHandler(this.FormTextChanged);
            form.VisibleChanged -= new EventHandler(this.FormVisibleChanged);
			
			foreach(BaseItem item in m_MdiItems)
			{
				if(item.Tag==form)
				{
					this.Parent.SubItems.Remove(item);
                    item.Click -= new EventHandler(this.MdiWindowItemClick);
                    item.Dispose();
					item.Tag=null;
					m_MdiItems.Remove(item);
					if(this.BeginGroup && m_MdiItems.Count>0)
						((BaseItem)m_MdiItems[0]).BeginGroup=true;
					break;
				}
			}
			
			if(m_MdiItems.Count==0)
			{
				if(this.ContainerControl is Bar)
					((Bar)this.ContainerControl).RecalcLayout();
			}
			else if(m_CreateMdiChildAccessKeys)
				RefreshMdiItemsText();
		}

		private void MdiFormActivated(object sender, EventArgs e)
		{
			IOwner owner=this.GetOwner() as IOwner;
            if (owner == null || owner.ParentForm == null && !(owner is RibbonBar && ((RibbonBar)owner).IsOverflowRibbon) || !m_Initialized)
				return;
            System.Windows.Forms.Form parentForm = sender as System.Windows.Forms.Form;
			System.Windows.Forms.Form form=parentForm.ActiveMdiChild;

			foreach(ButtonItem item in m_MdiItems)
			{
				if(item.Tag==form)
					item.Checked=true;
				else
					item.Checked=false;
			}
		}
		
		private void MdiWindowItemClick(object sender, EventArgs e)
		{
			ButtonItem btn=sender as ButtonItem;
			if(btn.Checked)
				return;
            System.Windows.Forms.Form form=btn.Tag as System.Windows.Forms.Form;

			IOwner owner=this.GetOwner() as IOwner;

            System.Windows.Forms.Form parentForm = form.MdiParent;
			if(owner==null || parentForm==null || !m_Initialized)
				return;
			System.Windows.Forms.Form oldActive=owner.ActiveMdiChild;
			System.Windows.Forms.MdiClient client=owner.GetMdiClient(parentForm);

			bool bSetRedraw=false;

			if(client!=null && m_MdiNoFormActivateFlicker && (form.WindowState==System.Windows.Forms.FormWindowState.Maximized || oldActive!=null && oldActive.WindowState==System.Windows.Forms.FormWindowState.Maximized))
			{
				NativeFunctions.SendMessage(client.Handle,NativeFunctions.WM_SETREDRAW,0,0);
				bSetRedraw=true;
			}

			form.Activate();

			if(bSetRedraw)
			{
				NativeFunctions.SendMessage(client.Handle,NativeFunctions.WM_SETREDRAW,1,0);
				client.Refresh();
			}

			if(oldActive!=null && oldActive.WindowState==System.Windows.Forms.FormWindowState.Normal && !(this.ContainerControl is Bar))
			{
				if(BarFunctions.ThemedOS)
				{
					// This will force repaint of Title bar since there were some repainting issues with it...
					string s=oldActive.Text;
					oldActive.Text=s+" ";
					oldActive.Text=s;
				}
			}
		}

		private void RefreshMdiList()
		{
            if (this.Parent == null || m_MdiItems == null) return;

			this.Parent.SuspendLayout=true;
			if(m_MdiItems.Count>0)
			{
				foreach(BaseItem item in m_MdiItems)
				{
					item.Tag=null;
					this.Parent.SubItems.Remove(item);
				}
				m_MdiItems.Clear();
			}
			IOwner owner=this.GetOwner() as IOwner;
            if (owner == null || owner.ParentForm == null && !(owner is RibbonBar && ((RibbonBar)owner).IsOverflowRibbon) || !m_Initialized)
			{
				this.Parent.SuspendLayout=false;
				return;
			}

            System.Windows.Forms.Form parentForm = owner.ParentForm;
            if (owner is RibbonBar && ((RibbonBar)owner).IsOverflowRibbon && parentForm==null)
            {
                if (System.Windows.Forms.Form.ActiveForm != null && System.Windows.Forms.Form.ActiveForm.IsMdiChild)
                    parentForm = System.Windows.Forms.Form.ActiveForm;

                if (parentForm == null)
                    return;
            }

			System.Windows.Forms.MdiClient client=owner.GetMdiClient(parentForm);
			if(client==null)
			{
				this.Parent.SuspendLayout=false;
				return;
			}

			int iInsertPosition=this.Parent.SubItems.IndexOf(this)+1;
			bool bFirst=true;
			int count=1;
			foreach(System.Windows.Forms.Form form in client.MdiChildren)
			{
				string text=GetFormText(form.Text,count);
				ButtonItem btn=new ButtonItem("mdi-"+form.Handle,text);
				btn.ShouldSerialize=false;
				btn.ButtonStyle=eButtonStyle.ImageAndText;
				if(bFirst && this.BeginGroup)
					btn.BeginGroup=true;
				bFirst=false;
				btn.Tag=form;
				if(parentForm.ActiveMdiChild==form)
					btn.Checked=true;
				btn.Click+=new EventHandler(this.MdiWindowItemClick);
				btn.SetSystemItem(true);
				//Bitmap img=null;
				if(m_ShowWindowIcons && form.Icon!=null && form.ControlBox)
				{
//					img=new Bitmap(16,16,System.Drawing.Imaging.PixelFormat.Format32bppArgb);
//					Graphics g=Graphics.FromImage(img);
//					g.DrawImage(form.Icon.ToBitmap(),new Rectangle(0,0,16,16));
//					g.Dispose();
//					btn.Image=img;
					btn.Icon=form.Icon.Clone() as System.Drawing.Icon;
				}
				m_MdiItems.Add(btn);
				this.Parent.SubItems.Add(btn,iInsertPosition);
				btn.Visible=form.Visible;
				iInsertPosition++;

				form.TextChanged+=new EventHandler(this.FormTextChanged);
				//btn.DisplayedChanged+=new EventHandler(this.OnMdiItemDisplayedChanged);
				count++;
			}

			this.Parent.SuspendLayout=false;

			if(m_MdiItems.Count>0 && this.ContainerControl is Bar)
				((Bar)this.ContainerControl).RecalcLayout();
		}

		private string GetFormText(string text, int index)
		{
			if(text.Length>m_MaxCaptionLength)
				text=text.Substring(0,m_MaxCaptionLength)+"...";
			if(m_CreateMdiChildAccessKeys && index<10)
				text="&" + index.ToString()+". "+text;
			return text;
		}

		/// <summary>
		/// Refresh the MDI Child form Icons that are displayed on window list items.
		/// </summary>
		public void RefreshButtonIcons()
		{
			if(!m_ShowWindowIcons || m_MdiItems==null)
				return;
			foreach(ButtonItem b in m_MdiItems)
			{
				try
				{
					if(b.Tag is System.Windows.Forms.Form && ((System.Windows.Forms.Form)b.Tag).ControlBox)
						b.Icon=((System.Windows.Forms.Form)b.Tag).Icon.Clone() as System.Drawing.Icon;
				}
				catch{};
			}
		}

		public override void SetParent(BaseItem o)
		{
			if(this.Parent!=null)
				this.Parent.ExpandChange-=new EventHandler(this.ParentExpandedChanged);
			base.SetParent(o);
			if(this.Parent!=null)
				this.Parent.ExpandChange+=new EventHandler(this.ParentExpandedChanged);
		}

		private void ParentExpandedChanged(object sender, EventArgs e)
		{
			if(this.Parent!=null && this.Parent.Expanded)
				RefreshButtonIcons();
		}

		private void FormTextChanged(object sender, EventArgs e)
		{
			System.Windows.Forms.Form form=sender as System.Windows.Forms.Form;
			if(form==null)
				return;
			for(int i=0;i<m_MdiItems.Count;i++)
			{
				BaseItem item=m_MdiItems[i] as BaseItem;
				if(item.Tag==form)
				{
					item.Text=GetFormText(form.Text,i+1);
					break;
				}
			}
		}

		private void RefreshMdiItemsText()
		{
			for(int i=0;i<m_MdiItems.Count;i++)
			{
				BaseItem item=m_MdiItems[i] as BaseItem;
				item.Text=GetFormText(((System.Windows.Forms.Form)item.Tag).Text,i+1);
			}
		}

		private void FormVisibleChanged(object sender, EventArgs e)
		{
			System.Windows.Forms.Form form=sender as System.Windows.Forms.Form;
			if(form==null)
				return;
			if(m_CreateMdiChildAccessKeys)
				RefreshMdiItemsText();
			foreach(BaseItem item in m_MdiItems)
			{
				if(item.Tag==form)
				{
					item.Visible=form.Visible;
					if(this.BeginGroup)
					{
						bool bBeginGroup=true;
						foreach(BaseItem mdiItem in m_MdiItems)
						{
							if(bBeginGroup && mdiItem.Visible)
							{
								mdiItem.BeginGroup=true;
								bBeginGroup=false;
							}
							else
								mdiItem.BeginGroup=false;

						}
					}
					if(this.ContainerControl is Bar && item.Visible)
						((Bar)this.ContainerControl).RecalcLayout();
					break;
				}
			}
		}

		/// <summary>
		/// Gets or sets whether flicker associated with switching maximized Mdi child forms is attempted to eliminate. You should set this property to false if you encounter any painting problems with your Mdi child forms.
		/// </summary>
		[System.ComponentModel.Browsable(true),DevCoBrowsable(true),System.ComponentModel.Description("Indicates whether flicker associated with switching maximized Mdi child forms is attempted to eliminate."),System.ComponentModel.Category("Mdi Support"),System.ComponentModel.DefaultValue(true)]
		public bool MdiNoFormActivateFlicker
		{
			get{return m_MdiNoFormActivateFlicker;}
			set
			{
				if(m_MdiNoFormActivateFlicker!=value)
				{
					m_MdiNoFormActivateFlicker=value;
				}
			}
		}

		/// <summary>
		/// Gets or sets whether numbered access keys are created for MDI Child window menu items for first 9 items. Access keys will start with number 1 and go through 9. Default value is false
		/// which indicates that access keys are not created.
		/// </summary>
		[System.ComponentModel.Browsable(true),DevCoBrowsable(true),System.ComponentModel.Description("Indicates whether numbered access keys are created for MDI Child window menu items for first 9 items."),System.ComponentModel.Category("Mdi Support"),System.ComponentModel.DefaultValue(false)]
		public bool CreateMdiChildAccessKeys
		{
			get {return m_CreateMdiChildAccessKeys;}
			set
			{
				if(m_CreateMdiChildAccessKeys!=value)
				{
					m_CreateMdiChildAccessKeys=value;
					this.RefreshMdiList();
				}
			}
		}
	}
}
