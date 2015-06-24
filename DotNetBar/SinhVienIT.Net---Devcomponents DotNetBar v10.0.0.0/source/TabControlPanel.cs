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
	/// Represents Panel for the Tab Control.
	/// </summary>
    [ToolboxItem(false), Designer("DevComponents.DotNetBar.Design.TabControlPanelDesigner, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
	public class TabControlPanel:PanelEx
	{
		#region Private Variables
		private const string INFO_TEXT="Drop controls on this panel to associate them with currently selected tab.";
		private TabItem m_Tab=null;
		private bool m_UseCustomStyle=false;
		#endregion

		#region Internal Implementation
		/// <summary>
		/// Default constructor.
		/// </summary>
		public TabControlPanel():base()
		{
			DockPadding.All = 1;
			this.BackColor=SystemColors.Control;
		}

		private Rectangle GetThemedRect(Rectangle r)
		{
			const int offset=6;

			switch(m_Tab.Parent.TabAlignment)
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
		
		protected override void OnPaint(PaintEventArgs e)
		{
            if (this.ClientRectangle.Width == 0 || this.ClientRectangle.Height == 0 || this.Parent is TabControl && ((TabControl)this.Parent).SelectedPanel!=this)
                return;
			bool baseCall=true;
			if(DrawThemedPane && BarFunctions.ThemedOS && m_Tab!=null && m_Tab.Parent!=null)
			{
				Rectangle r=GetThemedRect(this.ClientRectangle);
				
					Rectangle rTemp=new Rectangle(0,0,r.Width,r.Height);
					if(m_Tab.Parent.TabAlignment==eTabStripAlignment.Right || m_Tab.Parent.TabAlignment==eTabStripAlignment.Left)
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
							if(m_Tab.Parent.TabAlignment==eTabStripAlignment.Left)
								bmp.RotateFlip(RotateFlipType.Rotate270FlipNone);
							else if(m_Tab.Parent.TabAlignment==eTabStripAlignment.Right)
								bmp.RotateFlip(RotateFlipType.Rotate90FlipNone);
							else if(m_Tab.Parent.TabAlignment==eTabStripAlignment.Bottom)
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
                eTextFormat sf = eTextFormat.Default | eTextFormat.VerticalCenter | 
                    eTextFormat.HorizontalCenter | eTextFormat.EndEllipsis | eTextFormat.WordBreak;
				Font font=new Font(this.Font,FontStyle.Bold);
				TextDrawing.DrawString(e.Graphics,INFO_TEXT,font,ControlPaint.Dark(this.Style.BackColor1.Color),r,sf);
				font.Dispose();
			}
		}

		/// <summary>
		/// Gets or sets TabItem that this panel is attached to.
		/// </summary>
		[Browsable(false),DefaultValue(null)]
		public TabItem TabItem
		{
			get {return m_Tab;}
			set
			{
				m_Tab=value;
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

		protected override void OnResize(EventArgs e)
		{
			DisposeThemeCachedBitmap();
			base.OnResize(e);
		}

		/// <summary>
		/// Gets or sets which edge of the parent container a control is docked to.
		/// </summary>
		[Browsable(false),DefaultValue(DockStyle.None)]
		public override DockStyle Dock
		{
			get {return base.Dock;}
			set {base.Dock=value;}
		}
		#endregion
	}
}
