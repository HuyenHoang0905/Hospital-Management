using System;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Collections;
using System.Drawing;
using System.ComponentModel.Design;
using System.ComponentModel;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Represents panel that is hosted by DockContainerItem as docked control.
	/// </summary>
    [ToolboxItem(false), Designer("DevComponents.DotNetBar.Design.PanelDockContainerDesigner, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
	public class PanelDockContainer:PanelEx
	{
		#region Private Variables
		private const string INFO_TEXT="Drop controls here. Drag and Drop tabs to perform docking.";
		private DockContainerItem m_DockContainerItem=null;
		private bool m_UseCustomStyle=false;
		#endregion

		#region Internal Implementation
#if FRAMEWORK20
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override bool AutoSize
        {
            get
            {
                return base.AutoSize;
            }
            set
            {
                base.AutoSize = value;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override AutoSizeMode AutoSizeMode
        {
            get
            {
                return base.AutoSizeMode;
            }
            set
            {
                base.AutoSizeMode = value;
            }
        }
#endif
		/// <summary>
		/// Creates new instance of the panel.
		/// </summary>
		public PanelDockContainer():base()
		{
			this.BackColor=SystemColors.Control;
		}

		private Rectangle GetThemedRect(Rectangle r)
		{
			const int offset=6;

			switch(this.TabAlignment)
			{
				case eTabStripAlignment.Top:
				{
					r.Y-=offset;
					r.Height+=offset;
					break;
				}
				case eTabStripAlignment.Left:
				{
					r.X-=offset;
					r.Width+=offset;
					break;
				}
				case eTabStripAlignment.Right:
				{
					r.Width+=offset;
					break;
				}
				case eTabStripAlignment.Bottom:
				{
					r.Height+=offset;
					break;
				}
					
			}
			return r;
		}

		private eTabStripAlignment TabAlignment
		{
			get
			{
				if(m_DockContainerItem!=null)
				{
					if(m_DockContainerItem.ContainerControl is Bar)
					{
						return ((Bar)m_DockContainerItem.ContainerControl).DockTabAlignment;
					}
				}
				return eTabStripAlignment.Top;
			}
		}
		
		protected override void OnPaint(PaintEventArgs e)
		{
			bool baseCall=true;
			if(DrawThemedPane && BarFunctions.ThemedOS && m_DockContainerItem!=null)
			{
				Rectangle r=GetThemedRect(this.ClientRectangle);
				eTabStripAlignment tabAlignment=this.TabAlignment;
				
				Rectangle rTemp=new Rectangle(0,0,r.Width,r.Height);
				if(tabAlignment==eTabStripAlignment.Right || tabAlignment==eTabStripAlignment.Left)
					rTemp=new Rectangle(0,0,rTemp.Height,rTemp.Width);
				if(m_ThemeCachedBitmap==null || m_ThemeCachedBitmap.Size!=rTemp.Size)
				{
					DisposeThemeCachedBitmap();
					Bitmap bmp=new Bitmap(rTemp.Width,rTemp.Height,e.Graphics);
					try
					{
						Graphics gTemp=Graphics.FromImage(bmp);
						try
						{
							using(SolidBrush brush=new SolidBrush(Color.Transparent))
								gTemp.FillRectangle(brush,0,0,bmp.Width,bmp.Height);
							this.ThemeTab.DrawBackground(gTemp,ThemeTabParts.Pane,ThemeTabStates.Normal,rTemp);
						}
						finally
						{
							gTemp.Dispose();
						}
					}
					finally
					{
						if(tabAlignment==eTabStripAlignment.Left)
							bmp.RotateFlip(RotateFlipType.Rotate270FlipNone);
						else if(tabAlignment==eTabStripAlignment.Right)
							bmp.RotateFlip(RotateFlipType.Rotate90FlipNone);
						else if(tabAlignment==eTabStripAlignment.Bottom)
							bmp.RotateFlip(RotateFlipType.Rotate180FlipNone);
						e.Graphics.DrawImageUnscaled(bmp,r.X,r.Y);							
						m_ThemeCachedBitmap=bmp;
					}
				}
				else
					e.Graphics.DrawImageUnscaled(m_ThemeCachedBitmap,r.X,r.Y);

				baseCall=false;
			}

			if(baseCall)
				base.OnPaint(e);

			if(this.DesignMode && this.Controls.Count==0 && this.Text=="")
			{
				Rectangle r=this.ClientRectangle;
				r.Inflate(-2,-2);
                eTextFormat sf = eTextFormat.HorizontalCenter | eTextFormat.VerticalCenter | eTextFormat.EndEllipsis | eTextFormat.WordBreak;
				Font font=new Font(this.Font,FontStyle.Bold);
				TextDrawing.DrawString(e.Graphics,INFO_TEXT,font,ControlPaint.Dark(this.Style.BackColor1.Color),r,sf);
				font.Dispose();
			}
		}

		/// <summary>
		/// Indicates whether style of the panel is managed by tab control automatically.
		/// Set this to true if you would like to control style of the panel.
		/// </summary>
		[Browsable(true),DefaultValue(false),Category("Appearance"),Description("Indicates whether style of the panel is managed by tab control automatically. Set this to true if you would like to control style of the panel.")]
		public bool UseCustomStyle
		{
			get {return m_UseCustomStyle;}
			set {m_UseCustomStyle=value;}
		}

		/// <summary>
		/// Gets or sets TabItem that this panel is attached to.
		/// </summary>
		[Browsable(false),DefaultValue(null),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public DockContainerItem DockContainerItem
		{
			get {return m_DockContainerItem;}
			set	{m_DockContainerItem=value;}
		}

		protected override void OnResize(EventArgs e)
		{
			DisposeThemeCachedBitmap();
			base.OnResize(e);
		}

		/// <summary>
		/// Gets or sets which edge of the parent container a control is docked to.
		/// </summary>
		[Browsable(false),DefaultValue(DockStyle.None),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override DockStyle Dock
		{
			get {return base.Dock;}
			set {base.Dock=value;}
		}

		/// <summary>
		/// Gets or sets the size of the control.
		/// </summary>
		[Browsable(false)]
		public new System.Drawing.Size Size
		{
			get {return base.Size;}
			set {base.Size=value;}
		}

		/// <summary>
		/// Gets or sets the coordinates of the upper-left corner of the control relative to the upper-left corner of its container.
		/// </summary>
		[Browsable(false)]
		public new Point Location
		{
			get {return base.Location;}
			set {base.Location=value;}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the control is displayed.
		/// </summary>
		[Browsable(false)]
		public new bool Visible
		{
			get {return base.Visible;}
			set {base.Visible=value;}
		}

		/// <summary>
		/// Gets or sets which edges of the control are anchored to the edges of its container.
		/// </summary>
		[Browsable(false)]
		public override AnchorStyles Anchor
		{
			get {return base.Anchor;}
			set {base.Anchor=value;}
		}

		private TabStrip GetDockTabStrip()
		{
			TabStrip tabStrip=null;
			
			if(m_DockContainerItem!=null && m_DockContainerItem.ContainerControl is Bar)
			{
				Bar bar=m_DockContainerItem.ContainerControl as Bar;
				if(bar.DockSide==eDockSide.Document)
					tabStrip=bar.DockTabControl;
			}

			return tabStrip;
		}

		protected override void OnEnter(EventArgs e)
		{
            // Notify DotNetBarManager on activation
            DotNetBarManager manager = GetDotNetBarManager();
            if (manager != null) manager.InternalDockContainerActivated(m_DockContainerItem);

			TabStrip tabStrip=GetDockTabStrip();
			if(tabStrip==null)
				return;
			try
			{
				if(tabStrip.SelectedTabFont == null)
					tabStrip.SelectedTabFont = new Font(tabStrip.Font, FontStyle.Bold);
			}
			catch{}

            if (this.Controls.Count > 0)
            {
                Form f = this.FindForm();
                if (f != null)
                {
                    if (f.ActiveControl == this || f.ActiveControl == tabStrip && tabStrip != null)
                        this.SelectNextControl(null, true, true, true, true);
                }
            }

            base.OnEnter(e);
		}

        private DotNetBarManager GetDotNetBarManager()
        {
            if (m_DockContainerItem != null && m_DockContainerItem.ContainerControl is Bar)
            {
                return ((Bar)m_DockContainerItem.ContainerControl).Owner as DotNetBarManager;
            }
            return null;
        }
        protected override void OnLeave(EventArgs e)
		{
            // Notify DotNetBarManager on activation
            DotNetBarManager manager = GetDotNetBarManager();
            if (manager != null) manager.InternalDockContainerDeactivated(m_DockContainerItem);

			TabStrip tabStrip=GetDockTabStrip();
			if(tabStrip==null)
				return;
			if(tabStrip.SelectedTabFont != null)
				tabStrip.SelectedTabFont = null;

            base.OnLeave(e);
		}
		#endregion
	}
}
