namespace DevComponents.DotNetBar
{
	using System;
	using System.Windows.Forms;
	using System.Collections;

	/// <summary>
	///		
	/// </summary>
	internal class ItemsListBox : System.Windows.Forms.ListBox
	{
		private ArrayList m_Items;
		internal eDotNetBarStyle Style=eDotNetBarStyle.Office2003;
		internal ItemsListBox()
		{
			this.DrawMode=DrawMode.OwnerDrawFixed;
			m_Items=null;
			this.IntegralHeight=false;
			this.BackColor=System.Drawing.SystemColors.Control;
		}

		protected override void OnDrawItem(DrawItemEventArgs e)
		{
			base.OnDrawItem(e);
			if(m_Items==null || e.Index<0)
				return;

			BaseItem objItem=this.Items[e.Index] as BaseItem;
			e.DrawBackground();
			if(objItem==null)
			{
				//e.DrawBackground();
				return;
			}
			objItem.LeftInternal=e.Bounds.Left;
			objItem.TopInternal=e.Bounds.Top;
			
			objItem.WidthInternal=e.Bounds.Width;

            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                objItem.InternalMouseEnter();
                objItem.InternalMouseMove(new MouseEventArgs(MouseButtons.None, 0, objItem.LeftInternal + 2, objItem.TopInternal + 2, 0));
            }
			ItemPaintArgs pa=new ItemPaintArgs(null,this,e.Graphics,new ColorScheme(Style)); // TODO: ADD COLOR SCHEME
            pa.Renderer = GetRenderer();
			objItem.Paint(pa);

			if((e.State & DrawItemState.Selected)==DrawItemState.Selected)
				objItem.InternalMouseLeave();
		}
        /// <summary>
        /// Returns the renderer control will be rendered with.
        /// </summary>
        /// <returns>The current renderer.</returns>
        private Rendering.BaseRenderer GetRenderer()
        {
            return Rendering.GlobalManager.Renderer;
        }

		/*protected virtual void OnMeasureItem(MeasureItemEventArgs e)
		{
			base.OnMeasureItem(e);
		}*/
		internal void SetItems(ArrayList DotNetBarItems)
		{
			m_Items=DotNetBarItems;
			this.Items.Clear();
			int iMaxHeight=0;
			foreach(BaseItem objItem in m_Items)
			{
				objItem.ContainerControl=this;
				objItem.SetIsOnCustomizeDialog(true);
				objItem.Visible=true;
				objItem.WidthInternal=this.ClientSize.Width;
				objItem.RecalcSize();
				if(objItem.HeightInternal>iMaxHeight)
					iMaxHeight=objItem.HeightInternal;
			}
			if(iMaxHeight==0)
				iMaxHeight=16;
			this.ItemHeight=iMaxHeight;
			foreach(BaseItem objItem in m_Items)
			{
				objItem.HeightInternal=iMaxHeight;
				this.Items.Add(objItem);
			}
		}
	}
}
