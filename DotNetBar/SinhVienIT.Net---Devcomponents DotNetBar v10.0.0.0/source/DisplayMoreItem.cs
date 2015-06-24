using System;
using System.Drawing;
using System.ComponentModel;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// System item that displays the items that could not fit inside the container on popup menu or toolbar.
    /// </summary>
    [System.ComponentModel.ToolboxItem(false),System.ComponentModel.DesignTimeVisible(false)]
    public class DisplayMoreItem:PopupItem
    {
		private bool m_MouseOver;
		private const int FIXED_SIZE=14;
		private bool m_PopupSubItemsOnMenu;
		private eOrientation m_OldOrientation=eOrientation.Horizontal;
		private bool m_Localized=false;

		/// <summary>
		/// Create new instance of DisplayMoreItem object.
		/// </summary>
        public DisplayMoreItem()
        {
			this.GlobalItem=false;
			m_MouseOver=false;
			m_PopupSubItemsOnMenu=false;
			m_Tooltip="More Buttons";
			m_SystemItem=true;
			this.CanCustomize=false;
			m_ShouldSerialize=false;

			// Localization loading will repeat in OnContainerChanged becouse at this time
			// DotNetBarManager owner is not known and LocalizeString event cannot be invoked
			using(LocalizationManager lm=new LocalizationManager(null))
			{
				m_Tooltip=lm.GetLocalizedString(LocalizationKeys.OverlfowDisplayMoreTooltip);
			}
        }

		/// <summary>
		/// Returns copy of DisplayMoreItem item
		/// </summary>
		public override BaseItem Copy()
		{
			DisplayMoreItem objCopy=new DisplayMoreItem();
			this.CopyToItem(objCopy);
			return objCopy;
		}

		/// <summary>
		/// Called when item container has changed. If you override this method you must call the base implementation to allow default processing to occur.
		/// </summary>
		/// <param name="objOldContainer">Previous container of the item.</param>
		protected internal override void OnContainerChanged(object objOldContainer)
		{
			base.OnContainerChanged(objOldContainer);
			if(!m_Localized)
			{
				m_Localized=true;
				using(LocalizationManager lm=new LocalizationManager(this.GetOwner() as IOwnerLocalize))
				{
					m_Tooltip=lm.GetLocalizedString(LocalizationKeys.OverlfowDisplayMoreTooltip);
				}
			}
		}

		/// <summary>
		/// Returns the fixed size of the item.
		/// </summary>
		public static int FixedSize
		{
			get
			{
				return FIXED_SIZE;
			}
		}

		/// <summary>
		/// Overridden. Draws the item.
		/// </summary>
		/// <param name="g">Target Graphics object.</param>
        public override void Paint(ItemPaintArgs pa)
        {
            if (this.SuspendLayout)
                return;
            eDotNetBarStyle effectiveStyle = EffectiveStyle;
            if (effectiveStyle == eDotNetBarStyle.Office2000)
                PaintOffice(pa);
            else
            {
                if (this.IsThemed)
                {
                    PaintThemed(pa);
                    return;
                }
                else if (effectiveStyle == eDotNetBarStyle.Office2003 || effectiveStyle == eDotNetBarStyle.VS2005 || BarFunctions.IsOffice2007Style(effectiveStyle))
                {
                    PaintOffice2003(pa);
                    return;
                }
                else
                    PaintDotNet(pa);
            }

            System.Drawing.Graphics g = pa.Graphics;

            if (m_Orientation == eOrientation.Vertical)
            {
                Point[] p = new Point[3];
                p[0].X = m_Rect.Right - 8;
                p[0].Y = m_Rect.Top + 4;
                p[1].X = p[0].X + 2;
                p[1].Y = p[0].Y + 2;
                p[2].X = p[0].X + 4;
                p[2].Y = p[0].Y;
                g.DrawLines(SystemPens.ControlText, p);
                p[0].Y += 1;
                p[1].Y += 1;
                p[2].Y += 1;
                g.DrawLines(SystemPens.ControlText, p);

                p[0].Y += 3;
                p[1].Y += 3;
                p[2].Y += 3;
                g.DrawLines(SystemPens.ControlText, p);
                p[0].Y += 1;
                p[1].Y += 1;
                p[2].Y += 1;
                g.DrawLines(SystemPens.ControlText, p);

                p[0].X = m_Rect.Left + 7;
                p[0].Y = m_Rect.Top + 4;
                p[1].X = p[0].X - 3;
                p[1].Y = p[0].Y + 3;
                p[2].X = p[0].X;
                p[2].Y = p[0].Y + 6;
                g.FillPolygon(SystemBrushes.ControlText, p);
            }
            else
            {
                Point[] p = new Point[3];
                p[0].X = m_Rect.Left + (m_Rect.Width - 7) / 2 - 1;
                p[0].Y = m_Rect.Top + 4;
                p[1].X = p[0].X + 2;
                p[1].Y = p[0].Y + 2;
                p[2].X = p[0].X;
                p[2].Y = p[0].Y + 4;
                g.DrawLines(SystemPens.ControlText, p);
                p[0].X += 1;
                p[1].X += 1;
                p[2].X += 1;
                g.DrawLines(SystemPens.ControlText, p);

                p[0].X += 3;
                p[1].X += 3;
                p[2].X += 3;
                g.DrawLines(SystemPens.ControlText, p);
                p[0].X += 1;
                p[1].X += 1;
                p[2].X += 1;
                g.DrawLines(SystemPens.ControlText, p);

                p[0].X = m_Rect.Left + (m_Rect.Width / 2) - 1;
                p[0].Y = m_Rect.Bottom - 4;
                p[1].X = p[0].X - 2;
                p[1].Y = p[0].Y - 3;
                p[2].X = p[1].X + 5;
                p[2].Y = p[1].Y;
                g.FillPolygon(SystemBrushes.ControlText, p);
            }
        }

		private void PaintDotNet(ItemPaintArgs pa)
		{
			Rectangle r;
			System.Drawing.Graphics g=pa.Graphics;
			if(m_Expanded)
			{
				r=new Rectangle(m_Rect.Left+2,m_Rect.Top+2,m_Rect.Width-2,m_Rect.Height-2);
				if(!pa.Colors.ItemExpandedShadow.IsEmpty)
				{
					using(SolidBrush shadow=new SolidBrush(pa.Colors.ItemExpandedShadow))
						g.FillRectangle(shadow,r);
				}
				r.Offset(-2,-2);
				r.Height+=2;
				if(pa.Colors.ItemExpandedBackground2.IsEmpty)
					g.FillRectangle(new SolidBrush(pa.Colors.ItemExpandedBackground),r);
				else
				{
					System.Drawing.Drawing2D.LinearGradientBrush gradient=BarFunctions.CreateLinearGradientBrush(r,pa.Colors.ItemExpandedBackground,pa.Colors.ItemExpandedBackground2,pa.Colors.ItemExpandedBackgroundGradientAngle);
					g.FillRectangle(gradient,r);
					gradient.Dispose();
				}
                // TODO: Beta 2 fix --> g.DrawRectangle(new Pen(ColorFunctions.MenuFocusBorderColor(g),1),r);
				NativeFunctions.DrawRectangle(g,new Pen(pa.Colors.MenuBorder,1),r);
			}
			else if(m_MouseOver)
			{
				r=new Rectangle(m_Rect.Left,m_Rect.Top,m_Rect.Width-2,m_Rect.Height);
				if(!pa.Colors.ItemHotBackground2.IsEmpty)
				{
					System.Drawing.Drawing2D.LinearGradientBrush gradient=BarFunctions.CreateLinearGradientBrush(r,pa.Colors.ItemHotBackground,pa.Colors.ItemHotBackground2,pa.Colors.ItemHotBackgroundGradientAngle);
					g.FillRectangle(gradient,r);
					gradient.Dispose();
				}
				else
					g.FillRectangle(new SolidBrush(pa.Colors.ItemHotBackground),r);
				// TODO: Beta 2 Fix --> g.DrawRectangle(SystemPens.Highlight,r);
				NativeFunctions.DrawRectangle(g,new Pen(pa.Colors.ItemHotBorder),r);
			}
		}

		private void PaintThemed(ItemPaintArgs pa)
		{
			ThemeRebar theme=pa.ThemeRebar;
			ThemeRebarParts part=ThemeRebarParts.Chevron;
			ThemeRebarStates state=ThemeRebarStates.ChevronNormal;
			if(m_Expanded)
				state=ThemeRebarStates.ChevronPressed;
			else if(m_MouseOver)
				state=ThemeRebarStates.ChevronHot;

			theme.DrawBackground(pa.Graphics,part,state,m_Rect);
		}

		private void PaintOffice(ItemPaintArgs pa)
		{
			System.Drawing.Graphics g=pa.Graphics;
			if(m_Expanded)
			{
				System.Windows.Forms.ControlPaint.DrawBorder3D(g,m_Rect,System.Windows.Forms.Border3DStyle.SunkenOuter);
			}
			else if(m_MouseOver)
			{
				System.Windows.Forms.ControlPaint.DrawBorder3D(g,m_Rect,System.Windows.Forms.Border3DStyle.RaisedInner);
			}
		}

		private void PaintOffice2003(ItemPaintArgs pa)
		{
			Graphics g=pa.Graphics;

			// When on docked toolbar it has a special look...
			Rectangle r=m_Rect;

            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            if (this.Orientation == eOrientation.Vertical)
            {
                // When on docked toolbar it has a special look...
                r.Y += 2;
                r.Height -= 1;
                r.X -= 2;
                r.Width += 2;

                path.AddLine(r.X, r.Y, r.X + 2, r.Y + 2);
                path.AddLine(r.Right - 2, r.Y + 2, r.Right, r.Y);
                path.AddLine(r.Right, r.Bottom - 2, r.Right - 2, r.Bottom);
                path.AddLine(r.X + 2, r.Bottom, r.X, r.Bottom - 2);
                path.CloseAllFigures();
            }
            else
            {
                // When on docked toolbar it has a special look...
                r.X += 2;
                r.Width -= 1;
                r.Y -= 2;
                r.Height += 3;

                path.AddLine(r.X, r.Y, r.X + 2, r.Y + 2);
                path.AddLine(r.X + 2, r.Bottom - 2, r.X, r.Bottom);
                path.AddLine(r.Right - 2, r.Bottom, r.Right, r.Bottom - 2);
                path.AddLine(r.Right, r.Y + 2, r.Right - 2, r.Y);
                path.CloseAllFigures();
            }

			System.Drawing.Drawing2D.SmoothingMode smooth=g.SmoothingMode;
			g.SmoothingMode=System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
			if(this.Expanded)
			{
				System.Drawing.Drawing2D.LinearGradientBrush gradient=BarFunctions.CreateLinearGradientBrush(r,pa.Colors.ItemPressedBackground,pa.Colors.ItemPressedBackground2,pa.Colors.ItemPressedBackgroundGradientAngle);
				g.FillPath(gradient,path);
				gradient.Dispose();
			}
			else if(m_MouseOver)
			{
				System.Drawing.Drawing2D.LinearGradientBrush gradient=BarFunctions.CreateLinearGradientBrush(r,pa.Colors.ItemHotBackground,pa.Colors.ItemHotBackground2,pa.Colors.ItemHotBackgroundGradientAngle);
				g.FillPath(gradient,path);
				gradient.Dispose();
			}
			else
			{
				System.Drawing.Drawing2D.LinearGradientBrush gradient=BarFunctions.CreateLinearGradientBrush(r,pa.Colors.CustomizeBackground,pa.Colors.CustomizeBackground2,pa.Colors.CustomizeBackgroundGradientAngle);
				g.FillPath(gradient,path);
				gradient.Dispose();
			}
			g.SmoothingMode=smooth;

            if (this.Orientation == eOrientation.Vertical)
            {
                g.DrawLine(SystemPens.HighlightText, r.Left + (m_Rect.Width - 4) / 2, r.Bottom - 10 + 1, r.Left + (m_Rect.Width - 4) / 2 + 4, r.Bottom - 10 + 1);

                g.DrawLine(SystemPens.HighlightText, r.Left + (m_Rect.Width - 4) / 2 - 1 + 1, r.Top + 6 + 1, r.Left + (m_Rect.Width - 4) / 2 - 1 + 1, r.Top + 8 + 1);
                g.DrawLine(SystemPens.HighlightText, r.Left + (m_Rect.Width - 4) / 2 + 3 + 1, r.Top + 6 + 1, r.Left + (m_Rect.Width - 4) / 2 + 3 + 1, r.Top + 8 + 1);
                g.DrawLine(SystemPens.HighlightText, r.Left + (m_Rect.Width - 4) / 2 - 1 + 1, r.Top + 7 + 1, r.Left + (m_Rect.Width - 4) / 2 + 1, r.Top + 7 + 1);
                g.DrawLine(SystemPens.HighlightText, r.Left + (m_Rect.Width - 4) / 2 + 3 + 1, r.Top + 7 + 1, r.Left + (m_Rect.Width - 4) / 2 + 4 + 1, r.Top + 7 + 1);

                // Draw Arrow
                using (Pen pen = new Pen(pa.Colors.CustomizeText, 1))
                {
                    g.DrawLine(pen, r.Left + (m_Rect.Width - 4) / 2, r.Bottom - 10, r.Left + (m_Rect.Width - 4) / 2 + 4, r.Bottom - 10);

                    g.DrawLine(pen, r.Left + (m_Rect.Width - 4) / 2 - 1, r.Top + 6, r.Left + (m_Rect.Width - 4) / 2 - 1, r.Top + 8);
                    g.DrawLine(pen, r.Left + (m_Rect.Width - 4) / 2 + 3, r.Top + 6, r.Left + (m_Rect.Width - 4) / 2 + 3, r.Top + 8);
                    g.DrawLine(pen, r.Left + (m_Rect.Width - 4) / 2 - 1, r.Top + 7, r.Left + (m_Rect.Width - 4) / 2, r.Top + 7);
                    g.DrawLine(pen, r.Left + (m_Rect.Width - 4) / 2 + 3, r.Top + 7, r.Left + (m_Rect.Width - 4) / 2 + 4, r.Top + 7);
                }
            }
            else
            {
                g.DrawLine(SystemPens.HighlightText, r.Left + (m_Rect.Width - 4) / 2 + 1, r.Bottom - 11 + 1, r.Left + (m_Rect.Width - 4) / 2 + 4 + 1, r.Bottom - 11 + 1);

                g.DrawLine(SystemPens.HighlightText, r.Left + (m_Rect.Width - 4) / 2 - 1 + 1, r.Top + 6 + 1, r.Left + (m_Rect.Width - 4) / 2 - 1 + 1, r.Top + 8 + 1);
                g.DrawLine(SystemPens.HighlightText, r.Left + (m_Rect.Width - 4) / 2 + 3 + 1, r.Top + 6 + 1, r.Left + (m_Rect.Width - 4) / 2 + 3 + 1, r.Top + 8 + 1);
                g.DrawLine(SystemPens.HighlightText, r.Left + (m_Rect.Width - 4) / 2 - 1 + 1, r.Top + 7 + 1, r.Left + (m_Rect.Width - 4) / 2 + 1, r.Top + 7 + 1);
                g.DrawLine(SystemPens.HighlightText, r.Left + (m_Rect.Width - 4) / 2 + 3 + 1, r.Top + 7 + 1, r.Left + (m_Rect.Width - 4) / 2 + 4 + 1, r.Top + 7 + 1);

                // Draw Arrow Shade
                Point[] p = new Point[3];
                p[0].X = r.Left + (m_Rect.Width - 4) / 2 + 2 + 1;
                p[0].Y = r.Bottom - 5 + 1;
                p[1].X = p[0].X - 2;
                p[1].Y = p[0].Y - 3;
                p[2].X = p[1].X + 5;
                p[2].Y = p[1].Y;
                using (SolidBrush brush = new SolidBrush(SystemColors.HighlightText))
                    g.FillPolygon(brush, p);

                // Draw Arrow
                using (Pen pen = new Pen(pa.Colors.CustomizeText, 1))
                {
                    g.DrawLine(pen, r.Left + (m_Rect.Width - 4) / 2, r.Bottom - 11, r.Left + (m_Rect.Width - 4) / 2 + 4, r.Bottom - 11);

                    g.DrawLine(pen, r.Left + (m_Rect.Width - 4) / 2 - 1, r.Top + 6, r.Left + (m_Rect.Width - 4) / 2 - 1, r.Top + 8);
                    g.DrawLine(pen, r.Left + (m_Rect.Width - 4) / 2 + 3, r.Top + 6, r.Left + (m_Rect.Width - 4) / 2 + 3, r.Top + 8);
                    g.DrawLine(pen, r.Left + (m_Rect.Width - 4) / 2 - 1, r.Top + 7, r.Left + (m_Rect.Width - 4) / 2, r.Top + 7);
                    g.DrawLine(pen, r.Left + (m_Rect.Width - 4) / 2 + 3, r.Top + 7, r.Left + (m_Rect.Width - 4) / 2 + 4, r.Top + 7);
                }
                p = new Point[3];
                p[0].X = r.Left + (m_Rect.Width - 4) / 2 + 2;
                p[0].Y = r.Bottom - 5;
                p[1].X = p[0].X - 2;
                p[1].Y = p[0].Y - 3;
                p[2].X = p[1].X + 5;
                p[2].Y = p[1].Y;
                using (SolidBrush brush = new SolidBrush(pa.Colors.CustomizeText))
                    g.FillPolygon(brush, p);
            }
		}

		/// <summary>
		/// Overriden. Recalculates the size of the item.
		/// </summary>
		public override void RecalcSize()
		{
			if(this.SuspendLayout)
				return;

			if(m_Orientation==eOrientation.Vertical)
			{
				// Take suggested width
				m_Rect.Height=FIXED_SIZE;
			}
			else
			{
				// Take suggested height
				m_Rect.Width=FIXED_SIZE;
			}
			base.RecalcSize();
		}

		[System.ComponentModel.Browsable(false),System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override void InternalMouseEnter()
		{
			base.InternalMouseEnter();
			m_MouseOver=true;
			this.Refresh();
		}

		[System.ComponentModel.Browsable(false),System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override void InternalMouseLeave()
		{
			base.InternalMouseLeave();
			m_MouseOver=false;
			this.Refresh();
		}

		[System.ComponentModel.Browsable(false),System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override void InternalMouseDown(System.Windows.Forms.MouseEventArgs objArg)
		{
			base.InternalMouseDown(objArg);
			if(objArg.Button != System.Windows.Forms.MouseButtons.Left)
				return;
			//if(m_Expanded)
			//	m_Parent.AutoExpand=false;
			this.Expanded=!m_Expanded;
		}

		/// <summary>
		/// Overridden. Displays the sub-items on popup.
		/// </summary>
		/// <param name="p">Popup location.</param>
		public override void Popup(Point p)
		{
			Popup(p.X,p.Y);
		}

		/// <summary>
		/// Overridden. Displays the sub-items on popup.
		/// </summary>
		/// <param name="x">Horizontal coordinate in pixels of the upper left corner of a popup.</param>
		/// <param name="y">Vertical coordinate in pixels of the upper left corner of a popup.</param>
		public override void Popup(int x, int y)
		{
			if(m_PopupSubItemsOnMenu)
				PopupMenu(x,y);
			else
				PopupBar(x,y);
		}

		/// <summary>
		/// Overridden. Displays the sub-items on popup toolbar.
		/// </summary>
		/// <param name="x">Horizontal coordinate in pixels of the upper left corner of a popup.</param>
		/// <param name="y">Vertical coordinate in pixels of the upper left corner of a popup.</param>
		public override void PopupBar(int x, int y)
		{
			AddItems();
			// Reset Need Recalc Size since adding the items will trigger this flag
			m_NeedRecalcSize=false;
			base.PopupBar(x,y);
            m_NeedRecalcSize = false;
		}

		/// <summary>
		/// Overridden. Displays the sub-items on popup menu.
		/// </summary>
		/// <param name="x">Horizontal coordinate in pixels of the upper left corner of a popup.</param>
		/// <param name="y">Vertical coordinate in pixels of the upper left corner of a popup.</param>
		public override void PopupMenu(int x, int y)
		{
			AddItems();
			// Reset Need Recalc Size since adding the items will trigger this flag
			m_NeedRecalcSize=false;
			base.PopupMenu(x,y);
            m_NeedRecalcSize = false;
		}

		/// <summary>
		/// Overridden. Close the popup window if open.
		/// </summary>
		public override void ClosePopup()
		{
			base.ClosePopup();
			this.SubItems.Clear();
		}

		/// <summary>
		/// Get or sets whether item has been changed in a way that it needs its size recalculated. This is internal
		/// property and it should not be used by your code.
		/// </summary>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override bool NeedRecalcSize
		{
			get
			{
				return base.NeedRecalcSize;
			}
			set
			{
				m_NeedRecalcSize=value;
			}
		}

        /// <summary>
        /// Adds the items that are not visible to the overflow popup.
        /// </summary>
		protected virtual void AddItems()
		{
			if(m_SubItems==null)
			{
				m_SubItems=new SubItemsCollection(this);
			}

			m_OldOrientation=this.Orientation;
			foreach(BaseItem objItem in m_Parent.SubItems)
			{
				if(!objItem.Displayed && objItem.Visible)
				{
					m_SubItems._Add(objItem);
					ImageItem objImageItem=objItem as ImageItem;
					if(objImageItem!=null)
					{
						if(objImageItem.ImageSize.Width>this.SubItemsImageSize.Width)
							this.SubItemsImageSize=new Size(objImageItem.ImageSize.Width,this.SubItemsImageSize.Height);
						if(objImageItem.ImageSize.Height>this.SubItemsImageSize.Height)
							this.SubItemsImageSize=new Size(this.SubItemsImageSize.Width,objImageItem.ImageSize.Height);
					}
				}
			}
			
			foreach(BaseItem objItem in m_SubItems)
			{
				object objItemContainer=objItem.ContainerControl;
				m_Parent.SubItems._Remove(objItem);
				objItem.SetOrientation(eOrientation.Horizontal);
				objItem.SetParent(this);
				objItem.ContainerControl=null;
				objItem.OnContainerChanged(objItemContainer);
			}
			NeedRecalcSize=true;
		}

        /// <summary>
        /// Returns the insertion index for the items removed from overflow popup. Assumes that right-most items are removed first by the layout manager.
        /// </summary>
        /// <returns></returns>
        protected virtual int GetReInsertIndex()
        {
            int insertPos = m_Parent.SubItems.Count;
            for (int i = insertPos - 1; i >= 0; i--)
            {
                if (m_Parent.SubItems[i] is CustomizeItem)
                {
                    insertPos = i;
                    break;
                }
            }
            return insertPos;
        }

        /// <summary>
        /// Removes the items from the overflow and adds them back to the parent item.
        /// </summary>
        protected virtual void RemoveItems()
        {
            if (this.PopupControl is Bar)
            {
                ((Bar)this.PopupControl).ParentItem = null;
            }
            // Return the items back to the parent...
            m_HotSubItem = null;
            // Need to leave customize item at the end of the collection
            int insertPos = GetReInsertIndex();

            foreach (BaseItem objItem in m_SubItems)
            {
                object objItemContainer = objItem.ContainerControl;
                objItem.Orientation = m_OldOrientation;
                m_Parent.SubItems._Add(objItem, insertPos);
                insertPos++;
                objItem.SetParent(m_Parent);
                objItem.ContainerControl = null;
            }
            System.Collections.ArrayList col = new System.Collections.ArrayList(m_SubItems.Count);
            m_SubItems.CopyTo(col);
            while (m_SubItems.Count > 0)
                m_SubItems._Remove(m_SubItems[0]);
            foreach (BaseItem item in col)
                item.Displayed = false;

            base.OnExpandChange();

            BaseItem objParent = m_Parent;

            System.Windows.Forms.Control objCtrl = null;
            if (objParent != null)
                objCtrl = objParent.ContainerControl as System.Windows.Forms.Control;
            bool recalcSize = true;
            if (objCtrl != null)
            {
                recalcSize= !BarFunctions.InvokeRecalcLayout(objCtrl, false);
            }
            if (recalcSize && objParent != null)
            {
                objParent.RecalcSize();
                objParent.Refresh();
            }
        }

		protected internal override void OnExpandChange()
		{
			if(!this.Expanded && m_SubItems!=null && m_Parent!=null)
			{
                RemoveItems();
				return;
			}
			base.OnExpandChange();
		}

		/// <summary>
		/// Forces the repaint the item.
		/// </summary>
		public override void Refresh()
		{
			if(this.SuspendLayout)
				return;
            if ((EffectiveStyle == eDotNetBarStyle.Office2003 || EffectiveStyle == eDotNetBarStyle.VS2005 || BarFunctions.IsOffice2007Style(EffectiveStyle)) && !BaseItem.IsOnPopup(this))
			{
				if((m_Visible || this.IsOnCustomizeMenu) && m_Displayed)
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
						Rectangle r=m_Rect;
						r.Inflate(2,2);
						objCtrl.Invalidate(r,true);
					}
				}
			}
			else
				base.Refresh();
		}

        /// <summary>
        /// Gets whether the mouse is over the item.
        /// </summary>
        [Browsable(false)]
        public bool IsMouseOver
        {
            get { return m_MouseOver; }
        }
    }
}
