namespace DevComponents.DotNetBar
{
    using System;
	using System.Drawing;
	using System.Resources;
    using System.ComponentModel;

    /// <summary>
    /// Defines an item that allows the toolbar customization.
    /// </summary>
    [System.ComponentModel.ToolboxItem(false),System.ComponentModel.DesignTimeVisible(false)]
    public class CustomizeItem:PopupItem
    {
		private bool m_MouseOver;
		private const int FIXED_SIZE=14;
		private string m_CustomizeStr="",m_ResetStr="";
		
		private string m_ThisToolTip;

		private bool m_Localized=false;

		private bool m_CustomizeItemVisible=true;
		private bool m_AutoSizeMenuImages=true;
		private System.Drawing.Size m_MenuImageSize=Size.Empty;

		/// <summary>
		/// Creates new instance of CustomizeItem object.
		/// </summary>
        public CustomizeItem()//:base()
        {
			this.GlobalItem=false;
			m_MouseOver=false;
			m_SystemItem=true;
			this.CanCustomize=false;

			LoadResources();

			this.AutoCollapseOnClick=false;

			m_MenuImageSize=new Size(16,16);
        }

		/// <summary>
		/// Returns copy of CustomizeItem item
		/// </summary>
		public override BaseItem Copy()
		{
			CustomizeItem objCopy=new CustomizeItem();
			this.CopyToItem(objCopy);
			objCopy.CustomizeItemVisible=m_CustomizeItemVisible;
			return objCopy;
		}

		/// <summary>
		/// Called when item container has changed. If you override this method you must call the base implementation to allow default processing to occur.
		/// </summary>
		/// <param name="objOldContainer">Previous container of the item.</param>
		protected internal override void OnContainerChanged(object objOldContainer)
		{
			base.OnContainerChanged(objOldContainer);
			LoadResources();
		}

		/// <summary>
		/// Occurs when tooltip is about to be shown or hidden.
		/// </summary>
		/// <param name="bShow">Specifies whether tooltip is shown or hidden.</param>
		protected override void OnTooltip(bool bShow)
		{
			LoadResources();
			base.OnTooltip(bShow);
		}

        /// <summary>
        /// Loads the resources (text) used by this item.
        /// </summary>
		protected virtual void LoadResources()
		{
			if(!m_Localized)
			{
				if(this.GetOwner()!=null)
					m_Localized=true;
				using(LocalizationManager lm=new LocalizationManager(this.GetOwner() as IOwnerLocalize))
				{
					m_ThisToolTip=lm.GetLocalizedString(LocalizationKeys.CustomizeItemTooltip);
					this.Text=lm.GetLocalizedString(LocalizationKeys.CustomizeItemAddRemove);
                    m_CustomizeStr = lm.GetLocalizedString(LocalizationKeys.CustomizeItemCustomize);
					m_ResetStr=lm.GetLocalizedString(LocalizationKeys.CustomizeItemReset);
				}
			}
		}

		/// <summary>
		/// Overriden. Draws the item.
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
                if (this.IsThemed && !this.IsOnMenu)
                    PaintThemed(pa);
                else if (effectiveStyle == eDotNetBarStyle.Office2003 || effectiveStyle == eDotNetBarStyle.VS2005 || BarFunctions.IsOffice2007Style(effectiveStyle))
                {
                    PaintOffice2003(pa);
                    return;
                }
                else
                    PaintDotNet(pa);
            }
            if (BaseItem.IsOnPopup(this))
                return;

            Point[] p = new Point[3];
            if (m_Orientation == eOrientation.Vertical)
            {
                p[0].X = m_Rect.Left + 7;
                p[0].Y = m_Rect.Top + 4;
                p[1].X = p[0].X - 3;
                p[1].Y = p[0].Y + 3;
                p[2].X = p[0].X;
                p[2].Y = p[0].Y + 6;
                pa.Graphics.FillPolygon(SystemBrushes.ControlText, p);
            }
            else
            {
                p[0].X = m_Rect.Left + (m_Rect.Width / 2) - 1;
                p[0].Y = m_Rect.Bottom - 4;
                p[1].X = p[0].X - 2;
                p[1].Y = p[0].Y - 3;
                p[2].X = p[1].X + 5;
                p[2].Y = p[1].Y;
                pa.Graphics.FillPolygon(SystemBrushes.ControlText, p);
            }

            if (this.DesignMode && this.Focused)
            {
                Rectangle r = m_Rect;
                //r.Inflate(-1,-1);
                DesignTime.DrawDesignTimeSelection(pa.Graphics, r, pa.Colors.ItemDesignTimeBorder);
            }
        }

		private void PaintThemed(ItemPaintArgs pa)
		{
			ThemeToolbar theme=pa.ThemeToolbar;
			ThemeToolbarParts part=ThemeToolbarParts.Button;
			ThemeToolbarStates state=ThemeToolbarStates.Normal;
			if(m_Expanded)
				state=ThemeToolbarStates.Pressed;
			else if(m_MouseOver)
				state=ThemeToolbarStates.Hot;

			theme.DrawBackground(pa.Graphics,part,state,m_Rect);

			theme=null;

			if(!BaseItem.IsOnPopup(this))
				return;

			Font objFont=null;
			eTextFormat objStringFormat=GetStringFormat();
			Point[] p;

			objFont=GetFont();
			Rectangle rect=m_Rect;
			rect.Inflate(-1,-1);
			rect.Width-=6;
			
			TextDrawing.DrawString(pa.Graphics,m_Text,objFont,SystemColors.ControlText,rect,objStringFormat);

			p=new Point[3];
			p[0].X=m_Rect.Right-8;
			p[0].Y=m_Rect.Top+m_Rect.Height/2+3;
			p[1].X=p[0].X-2;
			p[1].Y=p[0].Y-3;
			p[2].X=p[1].X+5;
			p[2].Y=p[1].Y;
			pa.Graphics.FillPolygon(SystemBrushes.ControlText,p);

		}

		private void PaintDotNet(ItemPaintArgs pa)
		{
			Rectangle r;
			System.Drawing.Graphics g=pa.Graphics;
			
			if(this.Expanded)
			{
				r=new Rectangle(m_Rect.Left+2,m_Rect.Top+2,m_Rect.Width-2,m_Rect.Height-2);
				g.FillRectangle(SystemBrushes.ControlDark,r);
				r.Offset(-2,-2);
				r.Height+=2;
                DisplayHelp.FillRectangle(g, r, pa.Colors.ItemExpandedBackground, pa.Colors.ItemExpandedBackground2, pa.Colors.ItemExpandedBackgroundGradientAngle);
                DisplayHelp.DrawRectangle(g, pa.Colors.MenuBorder, r);
			}
			else if(m_MouseOver)
			{
				r=new Rectangle(m_Rect.Left,m_Rect.Top,m_Rect.Width-2,m_Rect.Height);

                DisplayHelp.FillRectangle(g, r, pa.Colors.ItemHotBackground, pa.Colors.ItemHotBackground2, pa.Colors.ItemHotBackgroundGradientAngle);
                DisplayHelp.DrawRectangle(g, pa.Colors.ItemHotBorder, r);
			}
			if(!BaseItem.IsOnPopup(this))
				return;

			Font objFont=null;
			eTextFormat objStringFormat=GetStringFormat();
			Point[] p;

			if(this.Expanded)
			{
                DisplayHelp.FillRectangle(g, m_Rect, pa.Colors.ItemExpandedBackground, pa.Colors.ItemExpandedBackground2, pa.Colors.ItemExpandedBackgroundGradientAngle);
                DisplayHelp.DrawRectangle(g, pa.Colors.MenuBorder, m_Rect);
			}

			objFont=GetFont();
            Rectangle rect=m_Rect;
			rect.Inflate(-1,-1);
			rect.Width-=6;
            rect.X += 4;
			
			TextDrawing.DrawString(g,m_Text,objFont,SystemColors.ControlText,rect,objStringFormat);

			p=new Point[3];
			p[0].X=m_Rect.Right-6;
			p[0].Y=m_Rect.Top+m_Rect.Height/2+3;
            p[1].X=p[0].X-2;
			p[1].Y=p[0].Y-3;
			p[2].X=p[1].X+5;
			p[2].Y=p[1].Y;
			g.FillPolygon(SystemBrushes.ControlText,p);

		}

		private void PaintOffice2003(ItemPaintArgs pa)
		{
			// When on popup the Customize Item is painted same as in .NET style...
			if(BaseItem.IsOnPopup(this))
			{
				PaintDotNet(pa);
				return;
			}
			
			Graphics g=pa.Graphics;
            Rectangle r = m_Rect;
            
			System.Drawing.Drawing2D.GraphicsPath path=new System.Drawing.Drawing2D.GraphicsPath();
            if (this.Orientation == eOrientation.Vertical)
            {
                // When on docked toolbar it has a special look...
                r.Y += 2;
                r.Height -= 1;
                r.X -= 2;
                r.Width += 2;

                if (pa.RightToLeft)
                {
                    path.AddLine(r.Right, r.Y, r.Right - 2, r.Y + 2);
                    path.AddLine(r.X + 2, r.Y + 2, r.X, r.Y);
                    path.AddLine(r.X, r.Bottom - 2, r.X + 2, r.Bottom);
                    path.AddLine(r.Right - 2, r.Bottom, r.Right, r.Bottom - 2);
                }
                else
                {
                    path.AddLine(r.X, r.Y, r.X + 2, r.Y + 2);
                    path.AddLine(r.Right - 2, r.Y + 2, r.Right, r.Y);
                    path.AddLine(r.Right, r.Bottom - 2, r.Right - 2, r.Bottom);
                    path.AddLine(r.X + 2, r.Bottom, r.X, r.Bottom - 2);
                }
                path.CloseAllFigures();
            }
            else
            {
                // When on docked toolbar it has a special look...
                r.X += 2;
                r.Width -= 1;
                r.Y -= 2;
                r.Height += 3;

                if (pa.RightToLeft)
                {
                    r.X-=2;
                    path.AddLine(r.Right, r.Y, r.Right - 2, r.Y + 2);
                    path.AddLine(r.Right - 2, r.Bottom - 2, r.Right, r.Bottom);
                    path.AddLine(r.X + 2, r.Bottom, r.X, r.Bottom - 2);
                    path.AddLine(r.X, r.Y + 2, r.X + 2, r.Y);
                }
                else
                {
                    path.AddLine(r.X, r.Y, r.X + 2, r.Y + 2);
                    path.AddLine(r.X + 2, r.Bottom - 2, r.X, r.Bottom);
                    path.AddLine(r.Right - 2, r.Bottom, r.Right, r.Bottom - 2);
                    path.AddLine(r.Right, r.Y + 2, r.Right - 2, r.Y);
                }
                path.CloseAllFigures();
            }
			
            System.Drawing.Drawing2D.SmoothingMode smooth=g.SmoothingMode;
			g.SmoothingMode=System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
			
            if(this.Expanded)
                DisplayHelp.FillPath(g, path, pa.Colors.ItemPressedBackground, pa.Colors.ItemPressedBackground2, pa.Colors.ItemPressedBackgroundGradientAngle);
			else if(m_MouseOver)
                DisplayHelp.FillPath(g, path, pa.Colors.ItemHotBackground, pa.Colors.ItemHotBackground2, pa.Colors.ItemHotBackgroundGradientAngle);
			else
                DisplayHelp.FillPath(g, path, pa.Colors.CustomizeBackground, pa.Colors.CustomizeBackground2, pa.Colors.CustomizeBackgroundGradientAngle);

			g.SmoothingMode=smooth;

            //using(Pen pen=new Pen(SystemColors.Window,1))
            //    g.DrawLine(pen,r.Left+(m_Rect.Width-4)/2+1,r.Bottom-11+1,r.Left+(m_Rect.Width-4)/2+4+1,r.Bottom-11+1);

            if (this.Orientation == eOrientation.Vertical)
            {
                // Draw Arrow Shade
                Point[] p = new Point[3];
                p[0].X = r.Left + (m_Rect.Width - 4) / 2 + 2 + 1;
                p[0].Y = r.Bottom - 3 + 1;
                p[1].X = p[0].X - 2;
                p[1].Y = p[0].Y - 3;
                p[2].X = p[1].X + 5;
                p[2].Y = p[1].Y;
                using (SolidBrush brush = new SolidBrush(SystemColors.Window)) // SystemColors.HighlightText))
                    g.FillPolygon(brush, p);

                // Draw Arrow
                using (Pen pen = new Pen(pa.Colors.CustomizeText, 1))
                    g.DrawLine(pen, r.Left + (m_Rect.Width - 4) / 2, r.Bottom - 9, r.Left + (m_Rect.Width - 4) / 2 + 4, r.Bottom - 9);
                p = new Point[3];
                p[0].X = r.Left + (m_Rect.Width - 4) / 2 + 2;
                p[0].Y = r.Bottom - 3;
                p[1].X = p[0].X - 2;
                p[1].Y = p[0].Y - 3;
                p[2].X = p[1].X + 5;
                p[2].Y = p[1].Y;
                using (SolidBrush brush = new SolidBrush(pa.Colors.CustomizeText))
                    g.FillPolygon(brush, p);
            }
            else
            {
                // Draw Arrow Shade
                Point[] p = new Point[3];
                p[0].X = r.Left + (m_Rect.Width - 4) / 2 + 2 + 1;
                p[0].Y = r.Bottom - 5 + 1;
                p[1].X = p[0].X - 2;
                p[1].Y = p[0].Y - 3;
                p[2].X = p[1].X + 5;
                p[2].Y = p[1].Y;
                using (SolidBrush brush = new SolidBrush(SystemColors.Window)) // SystemColors.HighlightText))
                    g.FillPolygon(brush, p);

                // Draw Arrow
                using (Pen pen = new Pen(pa.Colors.CustomizeText, 1))
                    g.DrawLine(pen, r.Left + (m_Rect.Width - 4) / 2, r.Bottom - 11, r.Left + (m_Rect.Width - 4) / 2 + 4, r.Bottom - 11);
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

		private void PaintOffice(ItemPaintArgs pa)
		{
			System.Drawing.Graphics g=pa.Graphics;
			if(this.Expanded)
			{
				System.Windows.Forms.ControlPaint.DrawBorder3D(g,m_Rect,System.Windows.Forms.Border3DStyle.SunkenOuter);
			}
			else if(m_MouseOver)
			{
				System.Windows.Forms.ControlPaint.DrawBorder3D(g,m_Rect,System.Windows.Forms.Border3DStyle.RaisedInner);
			}
			if(!BaseItem.IsOnPopup(this))
				return;
			
			Font objFont=null;
			eTextFormat objStringFormat=GetStringFormat();

			objFont=GetFont();
            Rectangle rect=m_Rect;
			rect.Inflate(-1,-1);
			rect.Width-=6;
			if(this.Expanded)
			{
				rect.Offset(1,1);
				rect.Width-=1;
				rect.Height-=1;
			}
			TextDrawing.DrawString(g,m_Text,objFont,SystemColors.ControlText,rect,objStringFormat);

			Point[] p=new Point[3];
			p[0].X=m_Rect.Right-8;
			p[0].Y=m_Rect.Top+m_Rect.Height/2+3;
            p[1].X=p[0].X-2;
			p[1].Y=p[0].Y-3;
			p[2].X=p[1].X+5;
			p[2].Y=p[1].Y;
			g.FillPolygon(SystemBrushes.ControlText,p);
		}

        /// <summary>
        /// Sets the custom system tooltip text for the item.
        /// </summary>
        /// <param name="text">Tooltip text.</param>
        protected virtual void SetCustomTooltip(string text)
        {
            this.Tooltip = text;
        }

		/// <summary>
		/// Overriden. Recalculates the size of the item.
		/// </summary>
		public override void RecalcSize()
		{
			if(this.SuspendLayout)
				return;

			if(!BaseItem.IsOnPopup(this))
			{
				if(m_Orientation==eOrientation.Vertical)
				{
					// Take suggested width
					m_Rect.Height=FIXED_SIZE;
					m_Rect.Width=22;
				}
				else
				{
					// Take suggested height
					m_Rect.Width=FIXED_SIZE;
					m_Rect.Height=22;
				}
				m_BeginGroup=false;
                SetCustomTooltip(GetTooltipText());
			}
			else
			{
				SetCustomTooltip("");
				m_BeginGroup=true;
				System.Windows.Forms.Control objCtrl=this.ContainerControl as System.Windows.Forms.Control;
				if(!IsHandleValid(objCtrl))
					return;
				Graphics g=Graphics.FromHwnd(objCtrl.Handle);
				// Get the right image size that we will use for calculation
				Size objImageSize=Size.Empty;
				if(m_Parent!=null)
				{
					ImageItem objParentImageItem=m_Parent as ImageItem;
					if(objParentImageItem!=null)
						objImageSize=new Size(objParentImageItem.SubItemsImageSize.Width,objParentImageItem.SubItemsImageSize.Height);
					else
						objImageSize=this.ImageSize;
				}
				else
					objImageSize=this.ImageSize;
				// Measure string
				Font objCurrentFont=null;
				objCurrentFont=GetFont();

				Size objStringSize=Size.Empty;
				eTextFormat objStringFormat=GetStringFormat();
				
				if(m_Text!="")
				{
					objStringSize=TextDrawing.MeasureString(g,m_Text,objCurrentFont,512,objStringFormat);
					objStringSize.Width+=2;
				}

				// Calculate item height				
				if(objStringSize.Height>objImageSize.Height)
					m_Rect.Height=(int)objStringSize.Height+4;
				else
					m_Rect.Height=objImageSize.Height+4;
				m_Rect.Width=(int)objStringSize.Width+10;
			}

			base.RecalcSize();
		}

        /// <summary>
        /// Gets localized tooltip text for this instance of the item.
        /// </summary>
        /// <returns>Tooltip text.</returns>
        protected virtual string GetTooltipText()
        {
            return m_ThisToolTip;
        }

		/*private bool IsOnPopUp()
		{
			if(this.ContainerControl is PopupMenu)
				return true;
			Bar objTlb=this.ContainerControl as Bar;
			if(objTlb!=null && objTlb.BarState==eBarState.Popup)
				return true;
			return false;
		}*/

		[System.ComponentModel.Browsable(false),System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override void InternalMouseEnter()
		{
			base.InternalMouseEnter();
			m_MouseOver=true;
			this.Refresh();
		}

		[System.ComponentModel.Browsable(false),System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override void InternalMouseHover()
		{
			base.InternalMouseHover();
            MouseHoverCustomize();
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
			if(objArg.Button != System.Windows.Forms.MouseButtons.Left || this.DesignMode)
				return;
            MouseDownAction();
		}

        protected virtual void MouseDownAction()
        {
            this.Expanded = !m_Expanded;
        }

        /// <summary>
        /// Called when mouse hovers over the customize item.
        /// </summary>
        protected virtual void MouseHoverCustomize()
        {
            if (!this.Expanded && BaseItem.IsOnPopup(this))
                this.Expanded = true;
        }

        /// <summary>
        /// Gets whether the mouse is over the item.
        /// </summary>
        [Browsable(false)]
        public bool IsMouseOver
        {
            get { return m_MouseOver; }
        }

		protected internal override void OnExpandChange()
		{
            if (this.Expanded)
                SetupCustomizeItem();

			base.OnExpandChange();
            if (!this.Expanded)
                ClearCustomizeItem();
		}

        protected virtual void SetupCustomizeItem()
        {
            if (BaseItem.IsOnPopup(this))
            {
                AddCustomizeItems();
                this.PopupType = ePopupType.Menu;
            }
            else
            {
                this.SubItems.Clear();
                CustomizeItem btn = new CustomizeItem();
                btn.CustomizeItemVisible = this.CustomizeItemVisible;
                btn.IsRightToLeft = this.IsRightToLeft;
                this.SubItems.Add(btn);
                this.PopupType = ePopupType.ToolBar;
            }
        }

        protected virtual void ClearCustomizeItem()
        {
            this.SubItems.Clear();
        }

		private void AddRemoveClick(object sender)
		{
			((ButtonItem)sender).Expanded=!((ButtonItem)sender).Expanded;
		}

		private void ShowHideClick(object sender, System.EventArgs e)
		{
			BaseItem objBtn=((BaseItem)sender).Tag as BaseItem;
			bool bGlobal=objBtn.GlobalItem;
			objBtn.GlobalItem=false;
			objBtn.SetVisibleDirect(!objBtn.Visible);
            if (objBtn is TextBoxItem || objBtn is ComboBoxItem || objBtn is ControlContainerItem || objBtn.ContainerControl is NavigationBar)
                objBtn.OnVisibleChanged(objBtn.Visible);
            objBtn.UserCustomized = true;
			objBtn.GlobalItem=bGlobal;
			((BaseItem)sender).Visible=!((BaseItem)sender).Visible;
			
			if(objBtn.ContainerControl is Bar)
				((Bar)objBtn.ContainerControl).RecalcLayout();
			else if(objBtn.ContainerControl is MenuPanel)
				((MenuPanel)objBtn.ContainerControl).RecalcSize();
			else if(objBtn.ContainerControl is BarBaseControl)
				((BarBaseControl)objBtn.ContainerControl).RecalcLayout();
			
			((BaseItem)sender).Refresh();
			IOwner owner=this.GetOwner() as IOwner;
			if(owner!=null)
			{
				owner.InvokeUserCustomize(objBtn,new EventArgs());
				owner.InvokeEndUserCustomize(objBtn,new EndUserCustomizeEventArgs(eEndUserCustomizeAction.ItemVisibilityChanged));
			}
		}

		private void CustomizeClick(object sender, System.EventArgs e)
		{
			IOwner owner=this.GetOwner() as IOwner;
			if(owner==null)
				return;
			CollapseAll(this);
			owner.Customize();
		}

		private void ResetClick(object sender, System.EventArgs e)
		{
			IOwner owner=this.GetOwner() as IOwner;
			if(owner==null)
				return;
			BaseItem item=this;
			if(BaseItem.IsOnPopup(this) && this.Parent!=null)
				item=this.Parent;
			CollapseAll(this);
			owner.InvokeResetDefinition(item,new EventArgs());
		}

		/// <summary>
		/// Forces the repaint the item.
		/// </summary>
		public override void Refresh()
		{
			if(this.SuspendLayout)
				return;
            if ((EffectiveStyle == eDotNetBarStyle.Office2003 || EffectiveStyle == eDotNetBarStyle.VS2005 || BarFunctions.IsOffice2007Style(this.EffectiveStyle)) && !BaseItem.IsOnPopup(this))
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

		private void AddCustomizeItems()
		{
			BaseItem objTmp;
			BaseItem objParent;

			this.SubItems.Clear();

			// Find the right parent item
			/*System.Windows.Forms.Control objCtrl=this.ContainerControl as System.Windows.Forms.Control;
			if(objCtrl is Bar)
			{
				Bar objTlb=objCtrl as Bar;
				if(objTlb.Parent==null)
					objParent=this.Parent;
				else
					objParent=objTlb.Parent;
			}
			else
			{
				objParent=this.Parent;
			}*/
			objParent=this.Parent;

			while(objParent!=null && objParent.SystemItem && !(objParent.SystemItem && objParent is GenericItemContainer))
				objParent=objParent.Parent;

			if(objParent==null)
				return;

			foreach(BaseItem objItem in objParent.SubItems)
			{
				if(!objItem.SystemItem && objItem.CanCustomize)
				{
					objTmp=objItem.Copy();
					objTmp.GlobalItem=false;
					objTmp.ClearClick();
					objTmp.BeginGroup=false;
					objTmp.Enabled=true;
					objTmp.SubItems.Clear();
					objTmp.Tooltip="";
					objTmp.SetIsOnCustomizeMenu(true);
					if(objItem is ButtonItem)
					{
						((ButtonItem)objTmp).HotTrackingStyle=eHotTrackingStyle.Default;
                        if (m_AutoSizeMenuImages && !m_MenuImageSize.IsEmpty && ((ButtonItem)objTmp).ImageSize != m_MenuImageSize)
                        {
                            ((ButtonItem)objTmp).ImageFixedSize = m_MenuImageSize;
                            ((ButtonItem)objTmp).UseSmallImage = true;
                        }
					}
					objTmp.Click+=new System.EventHandler(ShowHideClick);
					objTmp.Tag=objItem;
					this.SubItems.Add(objTmp);
				}
			}
			if(objParent is GenericItemContainer && ((GenericItemContainer)objParent).MoreItems!=null)
			{
				BaseItem objMore=((GenericItemContainer)objParent).MoreItems;
				foreach(BaseItem objItem in objMore.SubItems)
				{
					if(!objItem.SystemItem)
					{
						objTmp=objItem.Copy();
						objTmp.GlobalItem=false;
						objTmp.ClearClick();
						objTmp.BeginGroup=false;
						objTmp.Enabled=true;
						objTmp.SubItems.Clear();
						objTmp.Tooltip="";
						objTmp.SetIsOnCustomizeMenu(true);
						objTmp.Click+=new System.EventHandler(ShowHideClick);
						objTmp.Tag=objItem;
						this.SubItems.Add(objTmp);
					}
				}
			}

			objTmp=null;

			ButtonItem objBtn=null;
			IOwner owner=this.GetOwner() as IOwner;
			if(owner!=null && owner.ShowResetButton)
			{
				// Reset Bar Item
				objBtn=new ButtonItem();
				objBtn.GlobalItem=false;
				objBtn.BeginGroup=true;
				objBtn.Text=m_ResetStr; // "&Reset Bar";
				objBtn.SetIsOnCustomizeMenu(true);
				objBtn.SetSystemItem(true);
				objBtn.Orientation=eOrientation.Horizontal;
				objBtn.Click+=new System.EventHandler(ResetClick);
				this.SubItems.Add(objBtn);
			}
			if(m_CustomizeItemVisible)
			{
				// Customize
				objBtn=new ButtonItem();
				objBtn.GlobalItem=false;
				if(owner==null || owner!=null && !owner.ShowResetButton)
					objBtn.BeginGroup=true;
				objBtn.Text=m_CustomizeStr; //"&Customize...";
				objBtn.SetIsOnCustomizeMenu(true);
				objBtn.SetSystemItem(true);
				objBtn.Click+=new System.EventHandler(CustomizeClick);
				this.SubItems.Add(objBtn);
			}

			m_NeedRecalcSize=false;
		}

        private eTextFormat GetStringFormat()
		{
            eTextFormat format = eTextFormat.Default;
            format |= eTextFormat.SingleLine;
            //format |= eTextFormat.EndEllipsis;
            format |= eTextFormat.VerticalCenter;
            return format;
            //StringFormat sfmt=BarFunctions.CreateStringFormat(); //new StringFormat(StringFormat.GenericDefault);
            //sfmt.HotkeyPrefix=System.Drawing.Text.HotkeyPrefix.Show;
            ////sfmt.FormatFlags=sfmt.FormatFlags & ~(sfmt.FormatFlags & StringFormatFlags.DisableKerning);
            //sfmt.FormatFlags=sfmt.FormatFlags | StringFormatFlags.NoWrap;
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

		/// <summary>
		/// Gets or sets a value indicating whether the item is visible.
		/// </summary>
        [Browsable(true), DevCoBrowsable(true), DefaultValue(true), Category("Behavior"), Description("Determines whether the item is visible or hidden.")]
        public override bool Visible
		{
			get
			{
				Bar objTlb=this.ContainerControl as Bar;
				if(objTlb!=null)
				{
					if(objTlb.BarState==eBarState.Docked || objTlb.BarState==eBarState.Popup)
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

		/// <summary>
		/// Gets or sets whether Customize menu item is visible.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),Category("Behavior"),Description("Indicates whether Customize menu item is visible."),DefaultValue(true)]
		public virtual bool CustomizeItemVisible
		{
			get {return m_CustomizeItemVisible;}
			set {m_CustomizeItemVisible=value;}
		}

		/// <summary>
		/// Overloaded. Serializes the item and all sub-items into the XmlElement.
		/// </summary>
		/// <param name="ThisItem">XmlElement to serialize the item to.</param>
		protected internal override void Serialize(ItemSerializationContext context)
		{
			base.Serialize(context);
            System.Xml.XmlElement ThisItem = context.ItemXmlElement;
			if(!m_CustomizeItemVisible)
				ThisItem.SetAttribute("customizeitemvisible",System.Xml.XmlConvert.ToString(m_CustomizeItemVisible));
		}

		/// <summary>
		/// Overloaded. Deserializes the Item from the XmlElement.
		/// </summary>
		/// <param name="ItemXmlSource">Source XmlElement.</param>
		public override void Deserialize(ItemSerializationContext context)
		{
			base.Deserialize(context);
            System.Xml.XmlElement ItemXmlSource = context.ItemXmlElement;
			if(ItemXmlSource.HasAttribute("customizeitemvisible"))
				m_CustomizeItemVisible=System.Xml.XmlConvert.ToBoolean(ItemXmlSource.GetAttribute("customizeitemvisible"));
		}

        /// <summary>
        /// Indicates whether the item will auto-collapse (fold) when clicked. 
        /// When item is on popup menu and this property is set to false, menu will not
        /// close when item is clicked.
        /// </summary>
        [Browsable(false), DevCoBrowsable(false), Category("Behavior"), DefaultValue(false), Description("Indicates whether the item will auto-collapse (fold) when clicked.")]
        public override bool AutoCollapseOnClick
        {
            get { return base.AutoCollapseOnClick; }
            set {base.AutoCollapseOnClick = value; }
        }

        /// <summary>
        /// Gets or sets whether item can be customized by end user.
        /// </summary>
        [Browsable(false), DevCoBrowsable(false), DefaultValue(false), Category("Behavior"), Description("Indicates whether item can be customized by user.")]
        public override bool CanCustomize
        {
            get { return base.CanCustomize; }
            set { base.CanCustomize = value; }
        }

        /// <summary>
        /// Gets or sets whether item is global or not.
        /// This flag is used to propagate property changes to all items with the same name.
        /// Setting for example Visible property on the item that has GlobalItem set to true will
        /// set visible property to the same value on all items with the same name.
        /// </summary>
        [Browsable(false), DevCoBrowsable(false), DefaultValue(false), Category("Behavior"), Description("Indicates whether certain global properties are propagated to all items with the same name when changed.")]
        public override bool GlobalItem
        {
            get { return base.GlobalItem; }
            set { base.GlobalItem = value; }
        }
    }
}
