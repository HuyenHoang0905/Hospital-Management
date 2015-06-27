using System;
using System.ComponentModel.Design;
using System.Drawing;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using DevComponents.UI.ContentManager;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Represents item container that arranges items horizontally or vertically.
	/// </summary>
	[ToolboxItem(false),DesignTimeVisible(false),Designer(typeof(ItemContainerDesigner))]
	public class ItemContainer:ImageItem , IDesignTimeProvider
	{
		#region Private Variables & Constructor
		private eOrientation m_LayoutOrientation=eOrientation.Horizontal;
		private Size m_EmptyDesignTimeSize=new Size(24,24);
		private bool m_SystemContainer=false;
		
		/// <summary>
		/// Creates new instance of the ItemContainer object.
		/// </summary>
		public ItemContainer()
		{
			m_IsContainer=true;
			this.AutoCollapseOnClick=true;
			this.AccessibleRole=System.Windows.Forms.AccessibleRole.Grouping;
		}
		#endregion

		#region Internal Implementation
		/// <summary>
		/// Must be overriden by class that is inheriting to provide the painting for the item.
		/// </summary>
		public override void Paint(ItemPaintArgs p)
		{
			ItemDisplay display=GetItemDisplay();
			display.Paint(this,p);

			if(this.DesignMode && !this.SystemContainer && p.DesignerSelection)
			{
				Graphics g=p.Graphics;
				Rectangle r=this.DisplayRectangle;
				using(Pen pen=new Pen(Color.FromArgb(120,Color.Red),1))
				{
					pen.DashStyle=DashStyle.Dash;
					Display.DrawRoundedRectangle(g,pen,r,3);
				}

				Image image=BarFunctions.LoadBitmap("SystemImages.AddMoreItemsContainer.png");
				g.DrawImageUnscaled(image,r.X+1,r.Y+1);
				return;
			}
		}

		/// <summary>
		/// Recalcualtes the size of the container. Assumes that DisplayRectangle.Location is set to the upper left location of this container.
		/// </summary>
		public override void RecalcSize()
		{
			if(this.SuspendLayout)
				return;

			if(this.SubItems.Count==0)
			{
				if(this.DesignMode && !this.SystemContainer)
				{
					m_Rect.Size=m_EmptyDesignTimeSize;
					if(m_LayoutOrientation==eOrientation.Horizontal)
						m_Rect.Width+=12;
					else
						m_Rect.Height+=12;
				}
				else
				{
					m_Rect=Rectangle.Empty;
				}
				return;
			}
			
			IContentLayout layout=this.GetContentLayout();
			BlockLayoutManager blockLayout=this.GetBlockLayoutManager();
			BaseItem[] elements=new BaseItem[this.SubItems.Count];
			this.SubItems.CopyTo(elements,0);
			if(m_Rect.Width==0)
				m_Rect.Width=16;
			if(m_Rect.Height==0)
				m_Rect.Height=16;
			m_Rect=layout.Layout(m_Rect,elements,blockLayout);
			base.RecalcSize();
		}

		/// <summary>
		/// Occurs when the mouse pointer is over the item and a mouse button is pressed. This is used by internal implementation only.
		/// </summary>
		[System.ComponentModel.Browsable(false),System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override void InternalMouseDown(System.Windows.Forms.MouseEventArgs objArg)
		{
			if(this.DesignMode && !this.SystemContainer)
			{
				Rectangle r=this.DisplayRectangle;
				r.Width=14;
				r.Height=14;
				if(r.Contains(objArg.X,objArg.Y))
				{
					IOwner owner=this.GetOwner() as IOwner;
					if(owner!=null)
					{
						owner.SetFocusItem(this);
						return;
					}
				}
			}
			base.InternalMouseDown(objArg);
		}

		/// <summary>
		/// Gets or sets orientation inside the container. Do not change the value of this property. It is managed by system only.
		/// </summary>
		[Browsable(false),DevCoBrowsable(false),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override eOrientation Orientation
		{
			get {return eOrientation.Horizontal;}
			set	{}
		}

		/// <summary>
		/// Gets or sets orientation inside the container.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),DefaultValue(eOrientation.Horizontal),DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public virtual eOrientation LayoutOrientation
		{
			get {return m_LayoutOrientation;}
			set
			{
				m_LayoutOrientation=value;
				OnOrientationChanged();
			}
		}

		private void OnOrientationChanged()
		{
			m_NeedRecalcSize=true;
			if(m_LayoutManager!=null)
				m_LayoutManager.ContentOrientation=(this.LayoutOrientation==eOrientation.Horizontal?eContentOrientation.Horizontal:eContentOrientation.Vertical);
		}

		/// <summary>
		/// IBlock member implementation
		/// </summary>
		[Browsable(false),DevCoBrowsable(false),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),EditorBrowsable(EditorBrowsableState.Never)]
		public override System.Drawing.Rectangle Bounds
		{
			get
			{
				return base.Bounds;
			}
			set
			{
				Point offset=new Point(value.X-m_Rect.X,value.Y-m_Rect.Y);
				m_Rect=value;
				if(!offset.IsEmpty)
				{
					foreach(IBlock b in this.SubItems)
					{
						Rectangle r=b.Bounds;
						r.Offset(offset);
						b.Bounds=r;
					}
				}
			}
		}

		/// <summary>
		/// Returns copy of the item.
		/// </summary>
		public override BaseItem Copy()
		{
			ItemContainer objCopy=new ItemContainer();
			this.CopyToItem(objCopy);
			return objCopy;
		}
		/// <summary>
		/// Copies the ButtonItem specific properties to new instance of the item.
		/// </summary>
		/// <param name="c">New ButtonItem instance.</param>
		protected override void CopyToItem(BaseItem c)
		{
			ItemContainer copy=c as ItemContainer;
			base.CopyToItem(copy);
			
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
				if(!value)
					BaseItem.CollapseSubItems(this);
			}
		}

		/// <summary>
		/// Occurs when sub item expanded state has changed.
		/// </summary>
		/// <param name="item">Sub item affected.</param>
		protected internal override void OnSubItemExpandChange(BaseItem item)
		{
			base.OnSubItemExpandChange(item);
			if(item.Expanded)
				this.Expanded=true;
		}
		
		/// <summary>
		/// Returns whether instance of the item container is used as system container internally by DotNetBar.
		/// </summary>
		public bool SystemContainer
		{
			get {return m_SystemContainer;}
		}

		/// <summary>
		/// Sets whether container is used as system container internally by DotNetBar.
		/// </summary>
		/// <param name="b">true or false to indicate whether container is system container or not.</param>
		internal void SetSystemContainer(bool b)
		{
			m_SystemContainer=b;
		}

		/// <summary>
		/// Gets or sets the accessible role of the item.
		/// </summary>
		[DevCoBrowsable(true),Browsable(true),Category("Accessibility"),Description("Gets or sets the accessible role of the item."),DefaultValue(System.Windows.Forms.AccessibleRole.Grouping)]
		public override System.Windows.Forms.AccessibleRole AccessibleRole
		{
			get {return base.AccessibleRole;}
			set {base.AccessibleRole=value;}
		}
		#endregion

		#region Factories
		private SerialContentLayoutManager m_LayoutManager=null;
		internal IContentLayout GetContentLayout()
		{
			if(m_LayoutManager==null)
			{
				m_LayoutManager=new SerialContentLayoutManager();
				m_LayoutManager.BlockSpacing=1;
				m_LayoutManager.ContentAlignment=eContentAlignment.Left;
				m_LayoutManager.ContentLineAlignment=eContentLineAlignment.Top;
				m_LayoutManager.ContentOrientation=(this.LayoutOrientation==eOrientation.Horizontal?eContentOrientation.Horizontal:eContentOrientation.Vertical);
				m_LayoutManager.EvenHeight=(this.LayoutOrientation==eOrientation.Horizontal?true:false);
				m_LayoutManager.FitContainer=false;
				m_LayoutManager.FitContainerOversize=false;
				m_LayoutManager.MultiLine=false;
			}
			return m_LayoutManager;
		}

		private BlockLayoutManager GetBlockLayoutManager()
		{
			return new ItemBlockLayoutManager();
		}
		
		private ItemDisplay m_ItemDisplay=null;
		private ItemDisplay GetItemDisplay()
		{
			if(m_ItemDisplay==null)
				m_ItemDisplay=new ItemDisplay();
			return m_ItemDisplay;
		}
		#endregion

		#region IDesignTimeProvider Implementation
		
		InsertPosition IDesignTimeProvider.GetInsertPosition(Point pScreen, BaseItem dragItem)
		{
			return DesignTimeProviderContainer.GetInsertPosition(this,pScreen,dragItem);
		}
		void IDesignTimeProvider.DrawReversibleMarker(int iPos, bool Before)
		{
			DesignTimeProviderContainer.DrawReversibleMarker(this,iPos,Before);
		}
		void IDesignTimeProvider.InsertItemAt(BaseItem objItem, int iPos, bool Before)
		{
			DesignTimeProviderContainer.InsertItemAt(this,objItem,iPos,Before);
		}

		#endregion
	}
}
