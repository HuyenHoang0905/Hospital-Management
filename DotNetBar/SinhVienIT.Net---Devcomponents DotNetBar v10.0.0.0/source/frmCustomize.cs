namespace DevComponents.DotNetBar
{
	using System;
	using System.Drawing;
	using System.Collections;
	using System.ComponentModel;
	using System.Windows.Forms;

	/// <summary>
	///		Summary description for frmCustomize.
	/// </summary>
	[ToolboxItem(false),System.Runtime.InteropServices.ComVisible(false)]
	public class frmCustomize : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TabControl tabCtrl;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.TabPage tabPage3;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.Button cmdNew;
		private System.Windows.Forms.Button cmdDelete;
		private System.Windows.Forms.Button cmdRename;
		private System.Windows.Forms.Button cmdReset;
		private System.Windows.Forms.CheckedListBox lstBars;
		private DotNetBarManager m_DotNetBar;
		private System.Windows.Forms.Button cmdKeyboard;
		private System.Windows.Forms.Button cmdClose;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.ListBox lstCategories;
		private ItemsListBox lstCommands;
		private bool m_ItemDrag;
		private Cursor m_MoveCursor, m_CopyCursor, m_NACursor;
		private IDesignTimeProvider m_DesignTimeProvider;
		private int m_InsertPosition;
		private bool m_InsertBefore;
		private bool m_DragCopy;
		private Point m_MouseDownPt;

		public BaseItem DragItem;
		private BaseItem m_EditItem;
		private ButtonItem m_PopupMenu;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.CheckBox chkShowFullMenus;
		private System.Windows.Forms.CheckBox chkFullAfterDelay;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.CheckBox chkShowScreenTips;
		private System.Windows.Forms.CheckBox chkTipsShowShortcuts;
		private System.Windows.Forms.Label label7;
		private DevComponents.Editors.ComboItem comboItem1;
		private DevComponents.Editors.ComboItem comboItem2;
		private DevComponents.Editors.ComboItem comboItem3;
		private DevComponents.Editors.ComboItem comboItem4;
		private DevComponents.Editors.ComboItem comboItem5;
		private DevComponents.DotNetBar.Controls.ComboBoxEx cboAnimations;
		private DevComponents.Editors.ComboItem comboItem6;

		private System.Windows.Forms.Timer m_Timer=null;

		/// <summary>
		///		Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components;

		public frmCustomize(DotNetBarManager ctrl)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			m_DotNetBar=ctrl;
			this.StartPosition=FormStartPosition.CenterScreen;
			m_ItemDrag=false;
			try
			{
				m_MoveCursor=new Cursor(typeof(DevComponents.DotNetBar.DotNetBarManager),"DRAGMOVE.CUR");
				m_CopyCursor=new Cursor(typeof(DevComponents.DotNetBar.DotNetBarManager),"DRAGCOPY.CUR");
				m_NACursor=new Cursor(typeof(DevComponents.DotNetBar.DotNetBarManager),"DRAGNONE.CUR");
			}
			catch(Exception)
			{
				m_MoveCursor=null;
				m_CopyCursor=null;
				m_NACursor=null;
			}

			m_DesignTimeProvider=null;
			m_DragCopy=false;
			m_EditItem=null;

			// Load localized strings...
			using(LocalizationManager lm=new LocalizationManager(m_DotNetBar))
			{
				this.cmdNew.Text=lm.GetLocalizedString(this.cmdNew.Text);
				this.tabPage1.Text=lm.GetLocalizedString(this.tabPage1.Text);
				this.cmdReset.Text=lm.GetLocalizedString(this.cmdReset.Text);
				this.cmdRename.Text=lm.GetLocalizedString(this.cmdRename.Text);
				this.cmdDelete.Text=lm.GetLocalizedString(this.cmdDelete.Text);
				this.label1.Text=lm.GetLocalizedString(this.label1.Text);
				this.tabPage2.Text=lm.GetLocalizedString(this.tabPage2.Text);
				this.label4.Text=lm.GetLocalizedString(this.label4.Text);
				this.label3.Text=lm.GetLocalizedString(this.label3.Text);
				this.label2.Text=lm.GetLocalizedString(this.label2.Text);
				this.tabPage3.Text=lm.GetLocalizedString(this.tabPage3.Text);
				this.cmdKeyboard.Text=lm.GetLocalizedString(this.cmdKeyboard.Text);
				this.cmdClose.Text=lm.GetLocalizedString(this.cmdClose.Text);
				this.label5.Text=lm.GetLocalizedString(this.label5.Text);
				this.chkShowFullMenus.Text=lm.GetLocalizedString(this.chkShowFullMenus.Text);
				this.chkFullAfterDelay.Text=lm.GetLocalizedString(this.chkFullAfterDelay.Text);
				this.label6.Text=lm.GetLocalizedString(this.label6.Text);
				this.button1.Text=lm.GetLocalizedString(this.button1.Text);
				this.chkShowScreenTips.Text=lm.GetLocalizedString(this.chkShowScreenTips.Text);
				this.chkTipsShowShortcuts.Text=lm.GetLocalizedString(this.chkTipsShowShortcuts.Text);
				this.label7.Text=lm.GetLocalizedString(this.label7.Text);
				this.comboItem1.Text=lm.GetLocalizedString(this.comboItem1.Text);
				this.comboItem2.Text=lm.GetLocalizedString(this.comboItem2.Text);
				this.comboItem3.Text=lm.GetLocalizedString(this.comboItem3.Text);
				this.comboItem4.Text=lm.GetLocalizedString(this.comboItem4.Text);
				this.comboItem5.Text=lm.GetLocalizedString(this.comboItem5.Text);
				this.comboItem6.Text=lm.GetLocalizedString(this.comboItem6.Text);
				this.Text=lm.GetLocalizedString(this.Text);
			}

			this.cmdReset.Visible=m_DotNetBar.ShowResetButton;

			//If we don't run on XP use Flat Style Buttons
			// W2K and gang
//			if(Environment.OSVersion.Version.Major==5 && Environment.OSVersion.Version.Minor<1 || Environment.OSVersion.Version.Major<5)
//			{
//				this.cmdReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
//                this.cmdRename.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
//                this.cmdDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
//                this.cmdNew.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
//                this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
//				this.cmdKeyboard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
//				this.cmdClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
//			}

			cmdKeyboard.Visible=false;
			this.KeyPreview=true;

			if(!BarFunctions.SupportsAnimation)
			{
				cboAnimations.Visible=false;
				this.label7.Visible=false;
			}

			lstCommands.Style=m_DotNetBar.Style;
			
		}

		/// <summary>
		///		Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if(m_PopupMenu!=null)
			{
				m_PopupMenu.Dispose();
				m_PopupMenu=null;
			}

			m_DotNetBar=null;

			if(m_MoveCursor!=null)
				m_MoveCursor.Dispose();
			if(m_CopyCursor!=null)
				m_CopyCursor.Dispose();
			if(m_NACursor!=null)
				m_NACursor.Dispose();
			m_MoveCursor=null;
			m_CopyCursor=null;
			m_NACursor=null;

			if(components!=null)
				components.Dispose();
			components=null;
			base.Dispose(disposing);
		}

		private bool m_BarsLoading=false;
		public void RefreshBars()
		{
			m_BarsLoading=true;
			try
			{
				CheckState check=CheckState.Checked;
				lstBars.Items.Clear();

				foreach(DevComponents.DotNetBar.Bar objBar in m_DotNetBar.Bars)
				{
					if(!objBar.CanHide && !objBar.CanCustomize && objBar.LayoutType==eLayoutType.Toolbar || objBar.LayoutType!=eLayoutType.Toolbar)
					{
						objBar.SetDesignMode(true);
						objBar.RecalcLayout();
						continue;
					}
					if(objBar.Visible)
						check=CheckState.Checked;
					else
						check=CheckState.Unchecked;
					lstBars.Items.Add(objBar,check);
					objBar.SetDesignMode(true);
					objBar.RecalcLayout();
				}
			}
			finally
			{
				m_BarsLoading=false;
			}
		}

		public void RefreshCategories()
		{
			Hashtable h=new Hashtable();
			lstCategories.Items.Clear();
			lstCategories.Sorted=true;
			for(int i=0;i<m_DotNetBar.Items.Count;i++)
			{
				BaseItem objItem=m_DotNetBar.Items[i];
				if(objItem.Category!="" && !h.ContainsKey(objItem.Category))
					h.Add(objItem.Category,objItem.Category);
			}
			foreach(string s in h.Values)
				lstCategories.Items.Add(s);
//			if(h.Count>0)
//				lstCategories.SelectedIndex=0;
		}

		protected void BarsCheck(object sender,ItemCheckEventArgs ec)
		{
			if(m_BarsLoading)
				return;

			DevComponents.DotNetBar.Bar objBar=lstBars.Items[ec.Index] as Bar;
			if(objBar==null)
				return;
			if(!objBar.CanHide && objBar.Visible)
			{
				ec.NewValue=CheckState.Checked;
				return;
			}

			if(ec.NewValue==CheckState.Checked)
			{
				if(!objBar.Visible)
					objBar.ShowBar();
			}
			else
			{
				if(objBar.Visible)
					objBar.HideBar();
			}
			objBar.InvokeUserVisibleChanged();
			((IOwner)m_DotNetBar).InvokeUserCustomize(objBar,new EventArgs());
			((IOwner)m_DotNetBar).InvokeEndUserCustomize(objBar,new EndUserCustomizeEventArgs(eEndUserCustomizeAction.BarVisibilityChanged));
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			RefreshBars();
			RefreshCategories();

			// Load the customization settings
			chkShowFullMenus.Checked=m_DotNetBar.AlwaysShowFullMenus;
			chkFullAfterDelay.Checked=m_DotNetBar.ShowFullMenusOnHover;
			chkShowScreenTips.Checked=m_DotNetBar.ShowToolTips;
			chkTipsShowShortcuts.Checked=m_DotNetBar.ShowShortcutKeysInToolTips;
			switch(m_DotNetBar.PopupAnimation)
			{
				case ePopupAnimation.Fade:
					cboAnimations.SelectedItem=this.comboItem5;
					break;
				case ePopupAnimation.None:
					cboAnimations.SelectedItem=this.comboItem1;
					break;
				case ePopupAnimation.Random:
					cboAnimations.SelectedItem=this.comboItem2;
					break;
				case ePopupAnimation.Slide:
					cboAnimations.SelectedItem=this.comboItem4;
					break;
				case ePopupAnimation.SystemDefault:
					cboAnimations.SelectedItem=this.comboItem6;
					break;
				case ePopupAnimation.Unfold:
					cboAnimations.SelectedItem=this.comboItem3;
					break;
			}

		}

		protected override void OnClosing(CancelEventArgs e)
		{
			IOwner owner=m_DotNetBar as IOwner;
			owner.SetFocusItem(null);
			foreach(DevComponents.DotNetBar.Bar objBar in m_DotNetBar.Bars)
			{
				objBar.SetDesignMode(false);
				objBar.RecalcLayout();
			}
			
			// Update user settings
			m_DotNetBar.AlwaysShowFullMenus=chkShowFullMenus.Checked;
			m_DotNetBar.ShowFullMenusOnHover=chkFullAfterDelay.Checked;
			m_DotNetBar.ShowToolTips=chkShowScreenTips.Checked;
			m_DotNetBar.ShowShortcutKeysInToolTips=chkTipsShowShortcuts.Checked;
			if(cboAnimations.SelectedItem==this.comboItem5)
			{
				m_DotNetBar.PopupAnimation=ePopupAnimation.Fade;
			}
			else if(cboAnimations.SelectedItem==this.comboItem1)
			{
				m_DotNetBar.PopupAnimation=ePopupAnimation.None;
			}
			else if(cboAnimations.SelectedItem==this.comboItem2)
			{
				m_DotNetBar.PopupAnimation=ePopupAnimation.Random;
			}
			else if(cboAnimations.SelectedItem==this.comboItem4)
			{
				m_DotNetBar.PopupAnimation=ePopupAnimation.Slide;
			}
			else if(cboAnimations.SelectedItem==this.comboItem6)
			{
				m_DotNetBar.PopupAnimation=ePopupAnimation.SystemDefault;
			}
			else if(cboAnimations.SelectedItem==this.comboItem3)
			{
				m_DotNetBar.PopupAnimation=ePopupAnimation.Unfold;
			}
			else
			{
				m_DotNetBar.PopupAnimation=ePopupAnimation.SystemDefault;
			}
			
			m_DotNetBar.CustomizeClosing();
			base.OnClosing(e);
		}

		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);
			if(m_DotNetBar!=null && m_DotNetBar.ParentForm!=null)
				m_DotNetBar.ParentForm.BringToFront();
		}

		protected void Close_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		protected void CatSelectedIndexChanged(object sender, System.EventArgs e)
		{
			if(lstCategories.SelectedIndex<0)
			{
                lstCommands.Items.Clear();
				return;
			}
            string sCat=lstCategories.Items[lstCategories.SelectedIndex] as string;
			ArrayList lst=new ArrayList();
			BaseItem objCopy=null;
			for(int i=0;i<m_DotNetBar.Items.Count;i++)
			{
				BaseItem objItem=m_DotNetBar.Items[i];
				if(objItem.Category!=sCat || objItem.SystemItem)
					continue;
				objCopy=objItem.Copy();
				//objCopy.Enabled=true;  Issue: B1002
				objCopy.SetDesignMode(true);
				objCopy.SetOwner(m_DotNetBar);
				lst.Add(objCopy);
			}
            lstCommands.SetItems(lst);
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
		}

		private void MouseMoveDrag(Point pScreen)
		{
			if(m_ItemDrag)
			{
				if(m_DesignTimeProvider!=null)
				{
					m_DesignTimeProvider.DrawReversibleMarker(m_InsertPosition,m_InsertBefore);
					m_DesignTimeProvider=null;
				}
				foreach(DevComponents.DotNetBar.Bar bar in m_DotNetBar.Bars)
				{
					if(!bar.Visible || !bar.AcceptDropItems)
						continue;
					InsertPosition pos=((IDesignTimeProvider)bar.ItemsContainer).GetInsertPosition(pScreen, DragItem);
					
					if(pos!=null)
					{
						if(pos.TargetProvider==null)
						{
							// Cursor is over drag item
							if(m_NACursor!=null)
								System.Windows.Forms.Cursor.Current=m_NACursor;
							else
								System.Windows.Forms.Cursor.Current=System.Windows.Forms.Cursors.No;
							break;
						}
						pos.TargetProvider.DrawReversibleMarker(pos.Position,pos.Before);
						m_InsertPosition=pos.Position;
						m_InsertBefore=pos.Before;
						m_DesignTimeProvider=pos.TargetProvider;
						if(m_DragCopy)
						{
							if(m_CopyCursor!=null)
								System.Windows.Forms.Cursor.Current=m_CopyCursor;
							else
								System.Windows.Forms.Cursor.Current=System.Windows.Forms.Cursors.Hand;
						}
						else
						{
							if(m_MoveCursor!=null)
								System.Windows.Forms.Cursor.Current=m_MoveCursor;
							else
								System.Windows.Forms.Cursor.Current=System.Windows.Forms.Cursors.Hand;
						}
						break;
					}
					else
					{
						if(m_NACursor!=null)
							System.Windows.Forms.Cursor.Current=m_NACursor;
						else
							System.Windows.Forms.Cursor.Current=System.Windows.Forms.Cursors.No;
					}
				}
			}
		}

		public bool DragInProgress
		{
			get
			{
				return m_ItemDrag;
			}
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
		}

		private void MouseUpDrag(Point pScreen)
		{
			if(m_ItemDrag)
			{
				DestroyTimer();
				this.DragItem.InternalMouseLeave();
				if(m_DesignTimeProvider!=null)
				{
					m_DesignTimeProvider.DrawReversibleMarker(m_InsertPosition, m_InsertBefore);
					BaseItem objParent=this.DragItem.Parent;
					if(objParent!=null)
					{
						if(objParent==(BaseItem)m_DesignTimeProvider && m_InsertPosition>0)
						{
							if(objParent.SubItems.IndexOf(this.DragItem)<m_InsertPosition)
								m_InsertPosition--;
						}
						else
						{
							Bar bar=objParent.ContainerControl as Bar;
							if(bar!=null && this.DragItem.OriginalBarName=="")
								this.DragItem.OriginalBarName=bar.Name;
							else if(bar!=null && bar.Name==this.DragItem.OriginalBarName)
								this.DragItem.OriginalBarName="";
						}

						if(this.DragItem.OriginalPosition<0)
						{
							if(objParent==(BaseItem)m_DesignTimeProvider)
							{
								if(objParent.SubItems.IndexOf(this.DragItem)!=m_InsertPosition)
									this.DragItem.OriginalPosition=objParent.SubItems.IndexOf(this.DragItem);
							}
                            else
								this.DragItem.OriginalPosition=objParent.SubItems.IndexOf(this.DragItem);
						}
						else if(objParent==(BaseItem)m_DesignTimeProvider && m_InsertPosition==this.DragItem.OriginalPosition)
							this.DragItem.OriginalPosition=-1;

						objParent.SubItems.Remove(this.DragItem);
						Control ctrl=objParent.ContainerControl as Control;
						if(ctrl is Bar)
							((Bar)ctrl).RecalcLayout();
						else if(ctrl is MenuPanel)
							((MenuPanel)ctrl).RecalcSize();
					}
					else
						this.DragItem.OriginalPosition=0;
					m_DesignTimeProvider.InsertItemAt(this.DragItem,m_InsertPosition,m_InsertBefore);
					m_DesignTimeProvider=null;
					((IOwner)m_DotNetBar).InvokeUserCustomize(this.DragItem,new EventArgs());
					((IOwner)m_DotNetBar).InvokeEndUserCustomize(this.DragItem,new EndUserCustomizeEventArgs(eEndUserCustomizeAction.ItemMoved));
				}
				else if(this.DragItem.Parent!=null)
				{
					// Remove item if item is out of bounds of the main form
					Point p=pScreen;
					if(m_DotNetBar.ParentForm!=null && !m_DotNetBar.ParentForm.Bounds.Contains(p))
					{
						BaseItem objParent=this.DragItem.Parent;
						objParent.SubItems.Remove(this.DragItem);
						Control ctrl=objParent.ContainerControl as Control;
						if(ctrl is Bar)
							((Bar)ctrl).RecalcLayout();
						else if(ctrl is MenuPanel)
							((MenuPanel)ctrl).RecalcSize();
						((IOwner)m_DotNetBar).InvokeUserCustomize(this.DragItem,new EventArgs());
						((IOwner)m_DotNetBar).InvokeEndUserCustomize(this.DragItem,new EndUserCustomizeEventArgs(eEndUserCustomizeAction.ItemDeleted));
					}
				}
				m_ItemDrag=false;
				System.Windows.Forms.Cursor.Current=System.Windows.Forms.Cursors.Default;
				this.Capture=false;
				IOwner owner=m_DotNetBar as IOwner;
				owner.SetFocusItem(null);
			}
		}

		protected override bool ProcessDialogKey(Keys keyData)
		{
			if(keyData==Keys.Escape && m_ItemDrag)
			{
                CancelDrag();
				return true;			
			}
			return base.ProcessDialogKey(keyData);
		}

		internal void CancelDrag()
		{
			if(m_ItemDrag)
			{
				DestroyTimer();
				// Cancel item drag...
				if(m_DesignTimeProvider!=null)
					m_DesignTimeProvider.DrawReversibleMarker(m_InsertPosition, m_InsertBefore);
				m_ItemDrag=false;
				System.Windows.Forms.Cursor.Current=System.Windows.Forms.Cursors.Default;
				this.Capture=false;
				IOwner owner=m_DotNetBar as IOwner;
				owner.SetFocusItem(null);
				if(this.DragItem!=null)
				{
					this.DragItem.InternalMouseUp(new MouseEventArgs(MouseButtons.Left,1,this.DragItem.LeftInternal+1,this.DragItem.TopInternal+1,0));
					this.DragItem.Refresh();
				}
				this.DragItem=null;
			}
		}

		protected void Commands_OnMouseDown(object sender, MouseEventArgs e)
		{
			m_MouseDownPt=new Point(e.X,e.Y);
		}

		protected void Commands_OnMouseMove(object sender, MouseEventArgs e)
		{
			if(e.Button==MouseButtons.Left && (Math.Abs(m_MouseDownPt.X-e.X)>2 || Math.Abs(m_MouseDownPt.Y-e.Y)>2) && !m_ItemDrag)
			{
				if(lstCommands.SelectedIndex<0)
					return;
				BaseItem objItem=lstCommands.Items[lstCommands.SelectedIndex] as BaseItem;
				if(objItem==null)
					return;

				this.DragItem=objItem.Copy();
				this.DragItem.SetDesignMode(true);
				m_ItemDrag=true;
				m_DragCopy=true;
				this.Capture=true;
				CreateTimer();
				if(m_NACursor!=null)
					System.Windows.Forms.Cursor.Current=m_NACursor;
				else
					System.Windows.Forms.Cursor.Current=System.Windows.Forms.Cursors.No;
			}
		}

		public void DesignTimeContextMenu(BaseItem objItem)
		{
			using(LocalizationManager lm=new LocalizationManager(m_DotNetBar))
			{
				ButtonItem btn;
				m_EditItem=objItem;
				if(m_PopupMenu!=null)
					m_PopupMenu.Dispose();

				m_PopupMenu=new ButtonItem("syscustomizepopupmenu");
				m_PopupMenu.Style=objItem.Style;

				btn=new ButtonItem("reset");
				btn.Text=lm.GetLocalizedString(LocalizationKeys.CustomizeMenuReset);
				btn.Click+=new EventHandler(this.ResetItemClick);
				m_PopupMenu.SubItems.Add(btn);
				
				btn=new ButtonItem("delete");
				btn.Text=lm.GetLocalizedString(LocalizationKeys.CustomizeMenuDelete);
				btn.Click+=new System.EventHandler(this.DeleteClick);
				m_PopupMenu.SubItems.Add(btn);

				TextBoxItem tx=new TextBoxItem("name");
				tx.Text=lm.GetLocalizedString(LocalizationKeys.CustomizeMenuChangeName);
				tx.BeginGroup=true;
				tx.ControlText=objItem.Text;
				tx.LostFocus+=new System.EventHandler(this.ItemNameLostFocus);
				
				m_PopupMenu.SubItems.Add(tx);

				if(m_EditItem is ButtonItem)
				{
					ButtonItem objButton=m_EditItem as ButtonItem;
					btn=new ButtonItem("defaultstyle");
					btn.Text=lm.GetLocalizedString(LocalizationKeys.CustomizeMenuDefaultStyle);
					btn.BeginGroup=true;
					btn.Click+=new System.EventHandler(this.ButtonStyleClick);
					if(objButton.ButtonStyle==eButtonStyle.Default)
						btn.Checked=true;
					m_PopupMenu.SubItems.Add(btn);

					btn=new ButtonItem("textonly");
					btn.Text=lm.GetLocalizedString(LocalizationKeys.CustomizeMenuTextOnly);
					btn.Click+=new System.EventHandler(this.ButtonStyleClick);
					if(objButton.ButtonStyle==eButtonStyle.TextOnlyAlways)
						btn.Checked=true;
					m_PopupMenu.SubItems.Add(btn);

					btn=new ButtonItem("imageandtext");
					btn.Text=lm.GetLocalizedString(LocalizationKeys.CustomizeMenuImageAndText);
					btn.Click+=new System.EventHandler(this.ButtonStyleClick);
					if(objButton.ButtonStyle==eButtonStyle.ImageAndText)
						btn.Checked=true;
					m_PopupMenu.SubItems.Add(btn);

				}

				btn=new ButtonItem("begingroup");
				btn.BeginGroup=true;
				btn.Text=lm.GetLocalizedString(LocalizationKeys.CustomizeMenuBeginGroup);
				btn.Checked=m_EditItem.BeginGroup;
				btn.Click+=new System.EventHandler(this.BeginGroupClick);
			
				m_PopupMenu.SubItems.Add(btn);
	
				m_DotNetBar.OnCustomizeContextMenu(this,m_PopupMenu);
		        
				//m_DotNetBar.RegisterPopup(m_PopupMenu);
				m_PopupMenu.SetOwner(m_DotNetBar);
				m_PopupMenu.PopupMenu(Control.MousePosition);
			}
		}

		private void ResetItemClick(object sender, EventArgs e)
		{
			((IOwner)m_DotNetBar).InvokeResetDefinition(m_EditItem, e);
		}

		private void ItemNameLostFocus(object sender, System.EventArgs arg)
		{
			m_EditItem.Text=((TextBoxItem)m_PopupMenu.SubItems["name"]).ControlText;
			m_EditItem.Refresh();
			((IOwner)m_DotNetBar).InvokeUserCustomize(m_EditItem,new EventArgs());
			((IOwner)m_DotNetBar).InvokeEndUserCustomize(m_EditItem,new EndUserCustomizeEventArgs(eEndUserCustomizeAction.ItemTextChanged));
		}

		private void ButtonStyleClick(object sender, System.EventArgs e)
		{
            ButtonItem objButton=sender as ButtonItem;
			ButtonItem objEditItem=m_EditItem as ButtonItem;

			if(objButton==null)
				return;

			if(objButton.Name=="defaultstyle" && !objButton.Checked)
			{
				objEditItem.ButtonStyle=eButtonStyle.Default;
				objEditItem.Refresh();
				((IOwner)m_DotNetBar).InvokeUserCustomize(m_EditItem,new EventArgs());
				((IOwner)m_DotNetBar).InvokeEndUserCustomize(m_EditItem,new EndUserCustomizeEventArgs(eEndUserCustomizeAction.ItemStyleChanged));
			}
			else if(objButton.Name=="textonly" && !objButton.Checked)
			{
				objEditItem.ButtonStyle=eButtonStyle.TextOnlyAlways;
				objEditItem.Refresh();
				((IOwner)m_DotNetBar).InvokeUserCustomize(m_EditItem,new EventArgs());
				((IOwner)m_DotNetBar).InvokeEndUserCustomize(m_EditItem,new EndUserCustomizeEventArgs(eEndUserCustomizeAction.ItemStyleChanged));
			}
			else if(objButton.Name=="imageandtext" && !objButton.Checked)
			{
				objEditItem.ButtonStyle=eButtonStyle.ImageAndText;
				objEditItem.Refresh();
				((IOwner)m_DotNetBar).InvokeUserCustomize(m_EditItem,new EventArgs());
				((IOwner)m_DotNetBar).InvokeEndUserCustomize(m_EditItem,new EndUserCustomizeEventArgs(eEndUserCustomizeAction.ItemStyleChanged));
			}
		}

		private void BeginGroupClick(object sender, System.EventArgs e)
		{
			ButtonItem objItem=sender as ButtonItem;
			
			if(objItem==null)
				return;

			objItem.Checked=!objItem.Checked;
			m_EditItem.BeginGroup=objItem.Checked;
			objItem.Refresh();
			m_EditItem.Refresh();
			((IOwner)m_DotNetBar).InvokeUserCustomize(m_EditItem,new EventArgs());
			((IOwner)m_DotNetBar).InvokeEndUserCustomize(m_EditItem,new EndUserCustomizeEventArgs(eEndUserCustomizeAction.ItemBeginGroupChanged));
		}

		private void DeleteClick(object sender, System.EventArgs e)
		{
			BaseItem objParent=m_EditItem.Parent;
			objParent.SubItems.Remove(m_EditItem);
			objParent.Refresh();
			Bar bar=objParent.ContainerControl as Bar;
			if(bar!=null)
				bar.RecalcLayout();
			((IOwner)m_DotNetBar).InvokeUserCustomize(m_EditItem,new EventArgs());
			((IOwner)m_DotNetBar).InvokeEndUserCustomize(m_EditItem,new EndUserCustomizeEventArgs(eEndUserCustomizeAction.ItemDeleted));
		}

		protected override void WndProc(ref Message m)
		{
			if(m.Msg==NativeFunctions.WM_USER+707)
			{
				this.Capture=true;
				if(m_MoveCursor!=null)
                    System.Windows.Forms.Cursor.Current=m_MoveCursor;
				else
					System.Windows.Forms.Cursor.Current=System.Windows.Forms.Cursors.Hand;
				m_ItemDrag=true;
				this.Focus();
				CreateTimer();
				return;
			}
			base.WndProc(ref m);
		}

		#region Windows Form Designer generated code
		/// <summary>
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.chkShowScreenTips = new System.Windows.Forms.CheckBox();
			this.tabCtrl = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.cmdReset = new System.Windows.Forms.Button();
			this.cmdRename = new System.Windows.Forms.Button();
			this.cmdDelete = new System.Windows.Forms.Button();
			this.cmdNew = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.lstBars = new System.Windows.Forms.CheckedListBox();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.lstCommands = new DevComponents.DotNetBar.ItemsListBox();
			this.lstCategories = new System.Windows.Forms.ListBox();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.tabPage3 = new System.Windows.Forms.TabPage();
            this.cboAnimations = new Controls.ComboBoxEx();
			this.comboItem1 = new DevComponents.Editors.ComboItem();
			this.comboItem6 = new DevComponents.Editors.ComboItem();
			this.comboItem2 = new DevComponents.Editors.ComboItem();
			this.comboItem3 = new DevComponents.Editors.ComboItem();
			this.comboItem4 = new DevComponents.Editors.ComboItem();
			this.comboItem5 = new DevComponents.Editors.ComboItem();
			this.label7 = new System.Windows.Forms.Label();
			this.chkTipsShowShortcuts = new System.Windows.Forms.CheckBox();
			this.button1 = new System.Windows.Forms.Button();
			this.label6 = new System.Windows.Forms.Label();
			this.chkFullAfterDelay = new System.Windows.Forms.CheckBox();
			this.chkShowFullMenus = new System.Windows.Forms.CheckBox();
			this.label5 = new System.Windows.Forms.Label();
			this.cmdKeyboard = new System.Windows.Forms.Button();
			this.cmdClose = new System.Windows.Forms.Button();
			this.tabCtrl.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.tabPage3.SuspendLayout();
			this.SuspendLayout();
			// 
			// chkShowScreenTips
			// 
			this.chkShowScreenTips.Location = new System.Drawing.Point(24, 152);
			this.chkShowScreenTips.Name = "chkShowScreenTips";
			this.chkShowScreenTips.Size = new System.Drawing.Size(320, 16);
			this.chkShowScreenTips.TabIndex = 3;
			this.chkShowScreenTips.Text = "cust_chk_showst";
			// 
			// tabCtrl
			// 
			this.tabCtrl.Controls.AddRange(new System.Windows.Forms.Control[] {
																				  this.tabPage1,
																				  this.tabPage2,
																				  this.tabPage3});
			this.tabCtrl.Location = new System.Drawing.Point(6, 6);
			this.tabCtrl.Name = "tabCtrl";
			this.tabCtrl.SelectedIndex = 0;
			this.tabCtrl.Size = new System.Drawing.Size(354, 303);
			this.tabCtrl.TabIndex = 0;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.AddRange(new System.Windows.Forms.Control[] {
																				   this.cmdReset,
																				   this.cmdRename,
																				   this.cmdDelete,
																				   this.cmdNew,
																				   this.label1,
																				   this.lstBars});
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Size = new System.Drawing.Size(346, 277);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "cust_tab_toolbars";
			this.tabPage1.Visible = false;
			// 
			// cmdReset
			// 
			//this.cmdReset.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.cmdReset.Enabled = true;
			this.cmdReset.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.cmdReset.Location = new System.Drawing.Point(250, 118);
			this.cmdReset.Name = "cmdReset";
			this.cmdReset.Size = new System.Drawing.Size(90, 24);
			this.cmdReset.TabIndex = 4;
			this.cmdReset.Text = "cust_btn_reset";
			this.cmdReset.Visible = false;
			this.cmdReset.Click += new System.EventHandler(this.ResetBar);
			// 
			// cmdRename
			// 
			//this.cmdRename.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.cmdRename.Enabled = false;
			this.cmdRename.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.cmdRename.Location = new System.Drawing.Point(250, 52);
			this.cmdRename.Name = "cmdRename";
			this.cmdRename.Size = new System.Drawing.Size(90, 24);
			this.cmdRename.TabIndex = 2;
			this.cmdRename.Text = "cust_btn_rename";
			this.cmdRename.Click += new System.EventHandler(this.RenameBar);
			// 
			// cmdDelete
			// 
			//this.cmdDelete.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.cmdDelete.Enabled = false;
			this.cmdDelete.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.cmdDelete.Location = new System.Drawing.Point(250, 85);
			this.cmdDelete.Name = "cmdDelete";
			this.cmdDelete.Size = new System.Drawing.Size(90, 24);
			this.cmdDelete.TabIndex = 3;
			this.cmdDelete.Text = "cust_btn_delete";
			this.cmdDelete.Click += new System.EventHandler(this.DeleteBar);
			// 
			// cmdNew
			// 
			//this.cmdNew.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.cmdNew.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.cmdNew.Location = new System.Drawing.Point(250, 19);
			this.cmdNew.Name = "cmdNew";
			this.cmdNew.Size = new System.Drawing.Size(90, 24);
			this.cmdNew.TabIndex = 1;
			this.cmdNew.Text = "cust_btn_new";
			this.cmdNew.Click += new System.EventHandler(this.NewBar);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(5, 5);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(259, 12);
			this.label1.TabIndex = 0;
			this.label1.Text = "cust_lbl_tlbs";
			// 
			// lstBars
			// 
			//this.lstBars.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
			//	| System.Windows.Forms.AnchorStyles.Left) 
			//	| System.Windows.Forms.AnchorStyles.Right);
			this.lstBars.Location = new System.Drawing.Point(5, 19);
			this.lstBars.Name = "lstBars";
			this.lstBars.IntegralHeight=false;
			this.lstBars.Size = new System.Drawing.Size(239, 249);
			this.lstBars.TabIndex = 0;
			this.lstBars.SelectedIndexChanged += new System.EventHandler(this.BarSelectionChanged);
			this.lstBars.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.BarsCheck);
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.AddRange(new System.Windows.Forms.Control[] {
																				   this.lstCategories,
																				   this.lstCommands,
																				   this.label4,
																				   this.label3,
																				   this.label2});
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Size = new System.Drawing.Size(346, 277);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "cust_tab_commands";
			this.tabPage2.Visible = false;
			// 
			// lstCommands
			// 
			this.lstCommands.BackColor = System.Drawing.SystemColors.Control;
			this.lstCommands.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.lstCommands.IntegralHeight = false;
			this.lstCommands.Location = new System.Drawing.Point(136, 71);
			this.lstCommands.Name = "lstCommands";
			this.lstCommands.Size = new System.Drawing.Size(200, 197);
			this.lstCommands.TabIndex = 2;
			this.lstCommands.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Commands_OnMouseDown);
			this.lstCommands.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Commands_OnMouseMove);
			// 
			// lstCategories
			// 
			this.lstCategories.Location = new System.Drawing.Point(5, 71);
			this.lstCategories.Name = "lstCategories";
			this.lstCategories.IntegralHeight=false;
			this.lstCategories.Size = new System.Drawing.Size(121, 197);
			this.lstCategories.TabIndex = 2;
			this.lstCategories.SelectedIndexChanged += new System.EventHandler(this.CatSelectedIndexChanged);
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(136, 56);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(197, 13);
			this.label4.TabIndex = 1;
			this.label4.Text = "cust_lbl_cmds";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(5, 56);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(123, 13);
			this.label3.TabIndex = 1;
			this.label3.Text = "cust_lbl_cats";
			// 
			// label2
			// 
			//this.label2.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
			//	| System.Windows.Forms.AnchorStyles.Right);
			this.label2.Location = new System.Drawing.Point(4, 4);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(336, 46);
			this.label2.TabIndex = 0;
			this.label2.Text = "cust_lbl_cmdsins";
			// 
			// tabPage3
			// 
			this.tabPage3.Controls.AddRange(new System.Windows.Forms.Control[] {
																				   this.chkShowFullMenus,
																				   this.chkFullAfterDelay,
																				   this.button1,
																				   this.chkShowScreenTips,
																				   this.chkTipsShowShortcuts,
																				   this.cboAnimations,
																				   this.label7,
																				   this.label6,
																				   this.label5});
			this.tabPage3.Location = new System.Drawing.Point(4, 22);
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.Size = new System.Drawing.Size(346, 277);
			this.tabPage3.TabIndex = 2;
			this.tabPage3.Text = "cust_tab_options";
			this.tabPage3.Visible = false;
			// 
			// cboAnimations
			// 
			this.cboAnimations.DefaultStyle = false;
			this.cboAnimations.DisableInternalDrawing = false;
			this.cboAnimations.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboAnimations.DropDownWidth = 120;
			this.cboAnimations.Images = null;
			this.cboAnimations.Items.AddRange(new object[] {
															   this.comboItem1,
															   this.comboItem6,
															   this.comboItem2,
															   this.comboItem3,
															   this.comboItem4,
															   this.comboItem5});
			this.cboAnimations.Location = new System.Drawing.Point(24, 224);
			this.cboAnimations.Name = "cboAnimations";
			this.cboAnimations.Size = new System.Drawing.Size(120, 21);
			this.cboAnimations.Style = DevComponents.DotNetBar.eDotNetBarStyle.OfficeXP;
			this.cboAnimations.TabIndex = 5;
			// 
			// comboItem1
			// 
			this.comboItem1.BackColor = System.Drawing.Color.Empty;
			this.comboItem1.FontName = "";
			this.comboItem1.FontSize = 8F;
			this.comboItem1.FontStyle = System.Drawing.FontStyle.Regular;
			this.comboItem1.ForeColor = System.Drawing.Color.Empty;
			this.comboItem1.Image = null;
			this.comboItem1.ImageIndex = -1;
			this.comboItem1.ImagePosition = System.Windows.Forms.HorizontalAlignment.Left;
			this.comboItem1.Tag = null;
			this.comboItem1.Text = "cust_cbo_none";
			this.comboItem1.TextAlignment = System.Drawing.StringAlignment.Near;
			this.comboItem1.TextLineAlignment = System.Drawing.StringAlignment.Near;
			// 
			// comboItem6
			// 
			this.comboItem6.BackColor = System.Drawing.Color.Empty;
			this.comboItem6.FontName = "";
			this.comboItem6.FontSize = 8F;
			this.comboItem6.FontStyle = System.Drawing.FontStyle.Regular;
			this.comboItem6.ForeColor = System.Drawing.Color.Empty;
			this.comboItem6.Image = null;
			this.comboItem6.ImageIndex = -1;
			this.comboItem6.ImagePosition = System.Windows.Forms.HorizontalAlignment.Left;
			this.comboItem6.Tag = null;
			this.comboItem6.Text = "cust_cbo_system";
			this.comboItem6.TextAlignment = System.Drawing.StringAlignment.Near;
			this.comboItem6.TextLineAlignment = System.Drawing.StringAlignment.Near;
			// 
			// comboItem2
			// 
			this.comboItem2.BackColor = System.Drawing.Color.Empty;
			this.comboItem2.FontName = "";
			this.comboItem2.FontSize = 8F;
			this.comboItem2.FontStyle = System.Drawing.FontStyle.Regular;
			this.comboItem2.ForeColor = System.Drawing.Color.Empty;
			this.comboItem2.Image = null;
			this.comboItem2.ImageIndex = -1;
			this.comboItem2.ImagePosition = System.Windows.Forms.HorizontalAlignment.Left;
			this.comboItem2.Tag = null;
			this.comboItem2.Text = "cust_cbo_random";
			this.comboItem2.TextAlignment = System.Drawing.StringAlignment.Near;
			this.comboItem2.TextLineAlignment = System.Drawing.StringAlignment.Near;
			// 
			// comboItem3
			// 
			this.comboItem3.BackColor = System.Drawing.Color.Empty;
			this.comboItem3.FontName = "";
			this.comboItem3.FontSize = 8F;
			this.comboItem3.FontStyle = System.Drawing.FontStyle.Regular;
			this.comboItem3.ForeColor = System.Drawing.Color.Empty;
			this.comboItem3.Image = null;
			this.comboItem3.ImageIndex = -1;
			this.comboItem3.ImagePosition = System.Windows.Forms.HorizontalAlignment.Left;
			this.comboItem3.Tag = null;
			this.comboItem3.Text = "cust_cbo_unfold";
			this.comboItem3.TextAlignment = System.Drawing.StringAlignment.Near;
			this.comboItem3.TextLineAlignment = System.Drawing.StringAlignment.Near;
			// 
			// comboItem4
			// 
			this.comboItem4.BackColor = System.Drawing.Color.Empty;
			this.comboItem4.FontName = "";
			this.comboItem4.FontSize = 8F;
			this.comboItem4.FontStyle = System.Drawing.FontStyle.Regular;
			this.comboItem4.ForeColor = System.Drawing.Color.Empty;
			this.comboItem4.Image = null;
			this.comboItem4.ImageIndex = -1;
			this.comboItem4.ImagePosition = System.Windows.Forms.HorizontalAlignment.Left;
			this.comboItem4.Tag = null;
			this.comboItem4.Text = "cust_cbo_slide";
			this.comboItem4.TextAlignment = System.Drawing.StringAlignment.Near;
			this.comboItem4.TextLineAlignment = System.Drawing.StringAlignment.Near;
			// 
			// comboItem5
			// 
			this.comboItem5.BackColor = System.Drawing.Color.Empty;
			this.comboItem5.FontName = "";
			this.comboItem5.FontSize = 8F;
			this.comboItem5.FontStyle = System.Drawing.FontStyle.Regular;
			this.comboItem5.ForeColor = System.Drawing.Color.Empty;
			this.comboItem5.Image = null;
			this.comboItem5.ImageIndex = -1;
			this.comboItem5.ImagePosition = System.Windows.Forms.HorizontalAlignment.Left;
			this.comboItem5.Tag = null;
			this.comboItem5.Text = "cust_cbo_fade";
			this.comboItem5.TextAlignment = System.Drawing.StringAlignment.Near;
			this.comboItem5.TextLineAlignment = System.Drawing.StringAlignment.Near;
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(24, 208);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(320, 16);
			this.label7.TabIndex = 6;
			this.label7.Text = "cust_lbl_menuan";
			// 
			// chkTipsShowShortcuts
			// 
			this.chkTipsShowShortcuts.Location = new System.Drawing.Point(40, 176);
			this.chkTipsShowShortcuts.Name = "chkTipsShowShortcuts";
			this.chkTipsShowShortcuts.Size = new System.Drawing.Size(304, 16);
			this.chkTipsShowShortcuts.TabIndex = 4;
			this.chkTipsShowShortcuts.Text = "cust_chk_showsk";
			// 
			// button1
			// 
			this.button1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.button1.Location = new System.Drawing.Point(24, 80);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(180, 24);
			this.button1.TabIndex = 2;
			this.button1.Text = "cust_btn_resetusage";
			this.button1.Click += new System.EventHandler(this.ResetUsageData);
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(8, 128);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(336, 16);
			this.label6.TabIndex = 7;
			this.label6.Text = "cust_lbl_other";
			// 
			// chkFullAfterDelay
			// 
			this.chkFullAfterDelay.Location = new System.Drawing.Point(40, 56);
			this.chkFullAfterDelay.Name = "chkFullAfterDelay";
			this.chkFullAfterDelay.Size = new System.Drawing.Size(304, 16);
			this.chkFullAfterDelay.TabIndex = 1;
			this.chkFullAfterDelay.Text = "cust_chk_delay";
			// 
			// chkShowFullMenus
			// 
			this.chkShowFullMenus.Location = new System.Drawing.Point(24, 32);
			this.chkShowFullMenus.Name = "chkShowFullMenus";
			this.chkShowFullMenus.Size = new System.Drawing.Size(320, 16);
			this.chkShowFullMenus.TabIndex = 0;
			this.chkShowFullMenus.Text = "cust_chk_fullmenus";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(8, 8);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(336, 16);
			this.label5.TabIndex = 8;
			this.label5.Text = "cust_lbl_pmt";
			// 
			// cmdKeyboard
			// 
			//this.cmdKeyboard.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.cmdKeyboard.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.cmdKeyboard.Location = new System.Drawing.Point(160, 317);
			this.cmdKeyboard.Name = "cmdKeyboard";
			this.cmdKeyboard.Size = new System.Drawing.Size(96, 24);
			this.cmdKeyboard.TabIndex = 1;
			this.cmdKeyboard.Text = "cust_btn_keyboard";
			// 
			// cmdClose
			// 
			//this.cmdClose.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.cmdClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cmdClose.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.cmdClose.Location = new System.Drawing.Point(264, 317);
			this.cmdClose.Name = "cmdClose";
			this.cmdClose.Size = new System.Drawing.Size(96, 24);
			this.cmdClose.TabIndex = 2;
			this.cmdClose.Text = "cust_btn_close";
			this.cmdClose.Click += new System.EventHandler(this.Close_Click);
			// 
			// frmCustomize
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(368, 347);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.cmdClose,
																		  this.cmdKeyboard,
																		  this.tabCtrl});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmCustomize";
			this.ShowInTaskbar = false;
			this.Text = "cust_caption";
			this.tabCtrl.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage2.ResumeLayout(false);
			this.tabPage3.ResumeLayout(false);
			this.AcceptButton=cmdClose;
			this.CancelButton=cmdClose;
			this.ResumeLayout(false);

		}
		#endregion

		private void NewBar(object sender, System.EventArgs e)
		{
			ToolbarName nt=new ToolbarName();
			nt.txtName.Text="Custom Bar";
			using(LocalizationManager lm=new LocalizationManager(m_DotNetBar))
			{
				nt.txtName.Text=lm.GetLocalizedString("sys_custombar");
			}

			nt.StartPosition=FormStartPosition.CenterParent;

			if(nt.ShowDialog(this)==DialogResult.OK)
			{
				Bar bar=new Bar(nt.txtName.Text);
				bar.CustomBar=true;
				bar.CanHide=true;
				bar.SetDesignMode(true);
				bar.GrabHandleStyle=eGrabHandleStyle.StripeFlat;

				string name="userBar";
				int i=0;
				while(m_DotNetBar.Bars.Contains(name+i.ToString()))
					i++;
				bar.Name=name+i.ToString();

				m_DotNetBar.Bars.Add(bar);
				bar.DockSide=eDockSide.None;
				lstBars.Items.Add(bar,CheckState.Checked);
				if(m_DotNetBar.AllowUserBarCustomize)
					bar.Items.Add(new CustomizeItem());
				((IOwner)m_DotNetBar).InvokeUserCustomize(bar,new EventArgs());
				((IOwner)m_DotNetBar).InvokeEndUserCustomize(bar,new EndUserCustomizeEventArgs(eEndUserCustomizeAction.NewBarCreated));
			}
			nt.Close();
			nt.Dispose();
		}

		private void BarSelectionChanged(object sender, System.EventArgs e)
		{
			if(lstBars.SelectedIndex>=0)
			{
				Bar bar=lstBars.SelectedItem as Bar;
				if(bar.CustomBar)
				{
					cmdRename.Enabled=true;
					cmdReset.Enabled=false; // Work like Office, disable reset for custom bars
					cmdDelete.Enabled=true;
				}
				else
				{
					cmdRename.Enabled=false;
					cmdReset.Enabled=m_DotNetBar.ShowResetButton;
					cmdDelete.Enabled=false;
				}
			}
			else
			{
				cmdRename.Enabled=false;
				cmdReset.Enabled=m_DotNetBar.ShowResetButton;
				cmdDelete.Enabled=false;
			}
		}

		private void RenameBar(object sender, System.EventArgs e)
		{
			if(lstBars.SelectedIndex<0)
				return;
			Bar bar=lstBars.SelectedItem as Bar;
			if(bar==null)
				return;
            ToolbarName tn=new ToolbarName();
			tn.RenameDialog=true;
			tn.txtName.Text=bar.Text;
			tn.StartPosition=FormStartPosition.CenterParent;
			if(tn.ShowDialog(this)==DialogResult.OK)
			{
				bar.Text=tn.txtName.Text;
				lstBars.Refresh();
				((IOwner)m_DotNetBar).InvokeUserCustomize(bar,new EventArgs());
				((IOwner)m_DotNetBar).InvokeEndUserCustomize(bar,new EndUserCustomizeEventArgs(eEndUserCustomizeAction.BarRenamed));
			}
			tn.Close();
			tn.Dispose();
		}

		private void DeleteBar(object sender, System.EventArgs e)
		{
			if(lstBars.SelectedIndex<0)
				return;
			Bar bar=lstBars.SelectedItem as Bar;
			if(bar==null || !bar.CustomBar)
				return;
			using(LocalizationManager lm=new LocalizationManager(m_DotNetBar))
			{
				if(MessageBox.Show(lm.GetLocalizedString(LocalizationKeys.CustomizeDialogOptionsConfirmDelete).Replace("<barname>","'"+bar.Text+"'"),this.Text,MessageBoxButtons.YesNo)==DialogResult.Yes)
				{
					lstBars.Items.Remove(bar);
					m_DotNetBar.Bars.Remove(bar);
					((IOwner)m_DotNetBar).InvokeUserCustomize(bar,new EventArgs());
					((IOwner)m_DotNetBar).InvokeEndUserCustomize(bar,new EndUserCustomizeEventArgs(eEndUserCustomizeAction.BarDeleted));
					bar.Dispose();
				}
			}
		}

		private void ResetBar(object sender, System.EventArgs e)
		{
			IOwner owner=m_DotNetBar as IOwner;
			if(lstBars.SelectedIndex>=0)
			{
				Bar bar=lstBars.SelectedItem as Bar;
				if(bar!=null && (bar.CustomBar || m_DotNetBar.ShowResetButton))
				{
					if(owner!=null)
						owner.InvokeResetDefinition(bar.ItemsContainer,new EventArgs());
				}
			}
			else if(owner!=null)
                owner.InvokeResetDefinition(null,new EventArgs());
		}

		private void ResetUsageData(object sender, System.EventArgs e)
		{
			m_DotNetBar.ResetUsageData();
		}

		private void CreateTimer()
		{
			if(m_Timer!=null)
			{
				m_Timer.Start();
				return;
			}
			m_Timer=new Timer();
			m_Timer.Interval=100;
			m_Timer.Tick+=new EventHandler(this.TimerTick);
			m_Timer.Start();
		}
		private void TimerTick(object sender, EventArgs e)
		{
			if(Control.MouseButtons==MouseButtons.Left)
			{
				MouseMoveDrag(Control.MousePosition);
			}
			else
			{
				MouseUpDrag(Control.MousePosition);
			}
		}
		private void DestroyTimer()
		{
			if(m_Timer==null)
				return;
			m_Timer.Stop();
			m_Timer.Dispose();
			m_Timer=null;
		}

		internal DotNetBarManager GetDotNetBarManager()
		{
			return m_DotNetBar;
		}
	}
}
