using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace DevComponents.DotNetBar.Design
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class DotNetBarDesigner : System.Windows.Forms.Form
	{
		#region Private variables
		private IDesignerServices m_Designer=null;
		internal DevComponents.DotNetBar.NavigationPane navigationPane1;
		internal DevComponents.DotNetBar.NavigationPanePanel navigationPanePanel1;
		internal DevComponents.DotNetBar.NavigationPanePanel navigationPanePanel2;
		internal DevComponents.DotNetBar.ButtonItem buttonBars;
		internal DevComponents.DotNetBar.ButtonItem buttonContext;
		private DevComponents.DotNetBar.NavigationPanePanel navigationPanePanel3;
		private DevComponents.DotNetBar.ButtonItem buttonCategories;
		private ExpandableSplitter splitter1;
		private DevComponents.DotNetBar.PanelEx panelEx1;
		private ExpandableSplitter splitter2;
		private DevComponents.DotNetBar.DotNetBarManager dotNetBarManager1;
		private DevComponents.DotNetBar.DockSite barLeftDockSite;
		private DevComponents.DotNetBar.DockSite barRightDockSite;
		private DevComponents.DotNetBar.DockSite barTopDockSite;
		private DevComponents.DotNetBar.DockSite barBottomDockSite;
		private System.Windows.Forms.PropertyGrid propertyBars;
		private System.Windows.Forms.TreeView treeBars;
		private DevComponents.DotNetBar.Design.DefinitionPreviewControl definitionPreview;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.PropertyGrid propertyCategories;
		private DevComponents.DotNetBar.ExpandableSplitter splitterOffice20031;
		private System.Windows.Forms.TreeView treeCategories;
		private bool m_DataChanged=false;
		private bool m_ShowItemText=true;
		private System.Windows.Forms.OpenFileDialog m_OpenFileDialog;
		private string m_DefinitionFileName="";
		private System.Windows.Forms.ImageList imageList;
		private System.Windows.Forms.PropertyGrid propertyMenus;
		private DevComponents.DotNetBar.ExpandableSplitter splitterOffice20032;
		private System.Windows.Forms.TreeView treeMenus;
		private Bar m_DesignBar=null;

		private Hashtable m_catTable;

		private HTMLHelp m_HtmlHelp=null;

		private GridViewSubclass m_GridSubclassBars=null;
		private GridViewSubclass m_GridSubclassMenus=null;
		private GridViewSubclass m_GridSubclassCategories=null;

		private System.Windows.Forms.SaveFileDialog m_SaveFileDialog;

		private DotNetBarManager m_ExternalManager=null;
		private Bar m_ExternalBar=null;

		private Timer m_LoadTimer=null;
		private bool m_Loaded=false;

        private ArrayList m_CommandLinksDeleted = new ArrayList();
        private ArrayList m_CommandLinksCreated = new ArrayList();
		#endregion

		#region Command Constants
		const string NEW_BUTTON="buttonNewButton";
		const string NEW_LABEL="buttonNewLabel";
		const string NEW_COLORPICKER="buttonNewColorPicker";
		const string NEW_COMBO="buttonNewCombo";
		const string NEW_PROGRESS="buttonNewProgress";
		const string NEW_TEXTBOX="buttonNewTextBox";
		const string NEW_DOCK="buttonNewDock";
		const string NEW_CUSTOMIZE="buttonNewCustomize";
		const string NEW_MDILIST="buttonNewMdiList";
		const string NEW_CONTROLCONT="buttonNewControlCont";
        const string SAVE_CLOSE = "buttonSaveAndClose";
             
		const string CONTEXT_MENU="popup";
		const string MOVE_TO="buttonMoveTo";
		const string COPY_TO="buttonCopyTo";
		const string MOVE_LEFT="buttonMoveLeft";
		const string MOVE_RIGHT="buttonMoveRight";
		const string MOVE_UP="buttonMoveUp";
		const string MOVE_DOWN="buttonMoveDown";

		const string DELETE="buttonDelete";

		const string SAVE_DEFINITION_AS="buttonSaveAs";
		const string SAVE_DEFINITION="buttonSave";
		const string SAVE_BAR_AS="buttonSaveBar";
		const string LOAD_BAR="buttonLoadBar";
		const string SYNC_CATEGORIES="buttonSyncCategories";

		const string TO_CATEGORIES="buttonToCategories";
		const string TO_CONTEXTMENUS="buttonToContextMenus";

		const string RESET_IMAGE_POPUP="resetimagepopup";
		const string RESET_IMAGE="buttonResetImage";
		const string NEW_DEFINITION="buttonNewDefinition";
		const string OPEN_DEFINITION="buttonOpen";
		const string CLOSE_DESIGNER="buttonClose";
		const string SHOW_HELP="buttonHelpContents";
		const string SHOW_HELP_SEARCH="buttonHelpSearch";
		const string NEW_TOOLBAR="buttonNewToolbar";
		const string NEW_MENUBAR="buttonNewMenubar";
		const string NEW_STATUSBAR="buttonNewStatusbar";
		const string NEW_DOCKBAR="buttonNewDockWindow";
		const string NEW_TASKBAR="buttonNewTaskPane";

		const string CREATE_BARS_PARENT="buttonCreateBar";
		#endregion

		#region Constructor, load, unload  and dispose
		public DotNetBarDesigner()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			InitializeInternal();

            this.GetDesignManager().MouseDown += new MouseEventHandler(PreviewManagerMouseDown);
		}

        private void PreviewManagerMouseDown(object sender, MouseEventArgs e)
        {
            if (sender is BaseItem)
            {
                if (!((BaseItem)sender).SystemItem && !(sender is GenericItemContainer && ((GenericItemContainer)sender).SystemContainer))
                    SelectObjectInPropertyGrid(sender);
                else
                {
                    Bar bar = ((BaseItem)sender).ContainerControl as Bar;
                    if (bar != null)
                        SelectObjectInPropertyGrid(bar);
                    else
                        SelectObjectInPropertyGrid(null);
                }
            }
            else if (sender is Bar)
                SelectObjectInPropertyGrid(sender);
            else
                SelectObjectInPropertyGrid(null);
        }

		private void InitializeInternal()
		{
			propertyBars.CommandsVisibleIfAvailable=false;
			propertyMenus.CommandsVisibleIfAvailable=false;
			propertyCategories.CommandsVisibleIfAvailable=false;

			m_catTable=new Hashtable(20);
			
			m_GridSubclassBars=new GridViewSubclass();
			m_GridSubclassBars.ParentGrid=propertyBars;
			m_GridSubclassBars.OnRightMouseDown+=new EventHandler(this.GridViewMouseDown);
			m_GridSubclassMenus=new GridViewSubclass();
			m_GridSubclassMenus.ParentGrid=propertyMenus;
			m_GridSubclassMenus.OnRightMouseDown+=new EventHandler(this.GridViewMouseDown);
			m_GridSubclassCategories=new GridViewSubclass();
			m_GridSubclassCategories.ParentGrid=propertyCategories;
			m_GridSubclassCategories.OnRightMouseDown+=new EventHandler(this.GridViewMouseDown);
			
			foreach(Control ctrl in propertyBars.Controls)
			{
				if(ctrl.GetType().ToString().IndexOf("PropertyGridView")>=0)
				{
					m_GridSubclassBars.AssignHandle(ctrl.Handle);
					break;
				}
			}
			foreach(Control ctrl in propertyMenus.Controls)
			{
				if(ctrl.GetType().ToString().IndexOf("PropertyGridView")>=0)
				{
					m_GridSubclassMenus.AssignHandle(ctrl.Handle);
					break;
				}
			}
			foreach(Control ctrl in propertyCategories.Controls)
			{
				if(ctrl.GetType().ToString().IndexOf("PropertyGridView")>=0)
				{
					m_GridSubclassCategories.AssignHandle(ctrl.Handle);
					break;
				}
			}
		}

		private void DotNetBarDesigner_Load(object sender, System.EventArgs e)
		{
            dotNetBarManager1.Style = eDotNetBarStyle.Office2003;
			try
			{
				Microsoft.Win32.RegistryKey key=Microsoft.Win32.Registry.LocalMachine;
				string helpfile="";
				try
				{
					if(key!=null)
						key=key.OpenSubKey("Software\\DevComponents\\DotNetBar");
					if(key!=null)
						helpfile=key.GetValue("InstallationFolder","").ToString();
				}
				finally{if(key!=null) key.Close();}

				if(helpfile!="")
				{
					if(helpfile.Substring(helpfile.Length-1,1)!="\\")
						helpfile+="\\";

					if(System.IO.File.Exists(helpfile+"dotnetbar.chm"))
						helpfile+="dotnetbar.chm";
					else
						helpfile="";
				}

				if(helpfile!="")
					m_HtmlHelp=new HTMLHelp(this,helpfile);
			}
			catch(Exception)
			{
			}

			LoadFormLayout();
			
			SetupProperties();
			this.RefreshView();

			SetupToolbars();

			if(m_LoadTimer!=null)
				m_LoadTimer.Enabled=true;
			m_Loaded=true;
		}

		private void CheckColorSchemeDataChanged()
		{
			if(m_DataChanged)
				return;

			if(this.GetDesignManager()!=null)
			{
				foreach(Bar bar in this.GetDesignManager().Bars)
				{
					if(bar.ColorScheme!=null && bar.ColorScheme.SchemeChanged)
					{
						m_DataChanged=true;
						break;
					}
				}
			}
			else if(this.GetDesignBar()!=null)
			{
				if(this.GetDesignBar().ColorScheme!=null && this.GetDesignBar().ColorScheme.SchemeChanged)
					m_DataChanged=true;
			}
		}

		private void DotNetBarDesigner_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			CheckColorSchemeDataChanged();

			if(m_DataChanged)
			{
				DialogResult result=DialogResult.No;
				if(m_ExternalManager!=null)
				{
					result=MessageBox.Show("Definition has changed. Would you like to keep changes?","DotNetBar Designer",MessageBoxButtons.YesNoCancel,MessageBoxIcon.Question,MessageBoxDefaultButton.Button1);
				}
				else if(m_ExternalBar==null)
				{
					// Stand-alone editing
					result=MessageBox.Show("Definition has changed. Would you like to save changes?","DotNetBar Desinger",MessageBoxButtons.YesNoCancel,MessageBoxIcon.Question,MessageBoxDefaultButton.Button1);
				}

				if(result==DialogResult.Cancel)
				{
					e.Cancel=true;
					return;
				}

				if(result==DialogResult.Yes)
				{
					if(m_ExternalManager!=null)
						m_ExternalManager.Definition=this.GetDesignManager().Definition;
					else
					{
						if(SaveDefinition(false)==DialogResult.Cancel)
						{
							e.Cancel=true;
							return;
						}
					}
					this.DialogResult=DialogResult.Yes;
				}
				else
					this.DialogResult=DialogResult.No;
			}

			SaveFormLayout();
		}

        private void DotNetBarDesigner_Closed(object sender, EventArgs e)
        {
            //ProcessCommandLinks((this.DialogResult == DialogResult.Yes));
        }

        private void ProcessCommandLinks(bool saveDefinition)
        {
            //if (m_Designer == null)
            //    return;

            //if (saveDefinition)
            //{
            //    // Delete removed CommandLinks when definition is saved
            //    foreach (CommandLink link in m_CommandLinksDeleted)
            //    {
            //        link.Manager = null;
            //        m_Designer.DestroyComponent(link);
            //    }
            //}
            //else
            //{
            //    // Delete created CommandLinks when definition is saved
            //    foreach (CommandLink link in m_CommandLinksCreated)
            //    {
            //        link.Manager = null;
            //        m_Designer.DestroyComponent(link);
            //    }
            //}
        }

		private void LoadFormLayout()
		{
			// Load position if any
			try
			{
				Microsoft.Win32.RegistryKey key=Microsoft.Win32.Registry.CurrentUser;
				key=key.OpenSubKey("Software\\DevComponents\\DotNetBar");
				if(key!=null)
				{
					try
					{
						string s=key.GetValue("Designer2Position","").ToString();
						if(s!="")
						{
							if(s=="1")
							{
								this.WindowState=FormWindowState.Maximized;
							}
							else
							{
								string[] arr=s.Split(',');
								if(arr.Length==4)
								{
									Rectangle r=new Rectangle(System.Xml.XmlConvert.ToInt32(arr[0]),System.Xml.XmlConvert.ToInt32(arr[1]),System.Xml.XmlConvert.ToInt32(arr[2]),System.Xml.XmlConvert.ToInt32(arr[3]));
									foreach(Screen screen in Screen.AllScreens)
									{
										if(screen.Bounds.IntersectsWith(r))
										{
											this.StartPosition=FormStartPosition.Manual;
											this.DesktopLocation=r.Location;
											this.Size=r.Size;
											break;
										}
									}
								}
							}
						}
						s=key.GetValue("Designer2PanelSize","").ToString();
						navigationPane1.Width=System.Xml.XmlConvert.ToInt32(s);
						s=key.GetValue("Designer2TreeHeight","").ToString();
						treeBars.Height=System.Xml.XmlConvert.ToInt32(s);
						treeMenus.Height=System.Xml.XmlConvert.ToInt32(s);
						treeCategories.Height=System.Xml.XmlConvert.ToInt32(s);

						s=key.GetValue("Designer2PropSortBars","").ToString();
						if(s=="1")
							propertyBars.PropertySort=PropertySort.Alphabetical;
						s=key.GetValue("Designer2PropSortCat","").ToString();
						if(s=="1")
							propertyCategories.PropertySort=PropertySort.Alphabetical;
						s=key.GetValue("Designer2PropSortMenus","").ToString();
						if(s=="1")
							propertyMenus.PropertySort=PropertySort.Alphabetical;
					}
					finally
					{
						key.Close();
					}
				}
			}
			catch(Exception)
			{
			}
		}

		private void SaveFormLayout()
		{
			// Save form position
			if(this.WindowState!=FormWindowState.Minimized)
			{
				string s="";
				if(this.WindowState==FormWindowState.Maximized)
				{
					s="1";
				}
				else
				{
					s=this.DesktopLocation.X+","+this.DesktopLocation.Y+","+this.Width+","+this.Height;
				}
				try
				{
					Microsoft.Win32.RegistryKey key=Microsoft.Win32.Registry.CurrentUser;
					key=key.CreateSubKey("Software\\DevComponents\\DotNetBar");
					key.SetValue("Designer2Position",s);
					// Save Panel size
					key.SetValue("Designer2PanelSize",navigationPane1.Width);
					key.SetValue("Designer2TreeHeight",treeBars.Height);
					key.SetValue("Designer2PropSortBars",(propertyBars.PropertySort==PropertySort.Alphabetical?"1":"0"));
					key.SetValue("Designer2PropSortCat",(propertyCategories.PropertySort==PropertySort.Alphabetical?"1":"0"));
					key.SetValue("Designer2PropSortMenus",(propertyMenus.PropertySort==PropertySort.Alphabetical?"1":"0"));
					key.Close();
				}
				catch(Exception)
				{
				}
			}
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}
		#endregion

		#region Preview Support
		private DotNetBarManager GetDesignManager()
		{
			if(m_ExternalBar!=null)
				return null;
			return definitionPreview.previewManager;
		}
		private Bar GetDesignBar()
		{
			return m_ExternalBar;
		}
		#endregion

		#region Categories Support
		public void RescanCategories()
		{
			DotNetBarManager manager=GetDesignManager();
			if(manager==null)
				return;
			if(manager.Bars.Count==0)
				return;
			manager.Items.Clear();
			treeCategories.Nodes.Clear();
			foreach(Bar bar in manager.Bars)
			{
				foreach(BaseItem item in bar.Items)
					AutoCategorizeItem(item);
			}
			m_DataChanged=true;
			RefreshCategories();
		}

		private void AutoCategorizeItem(BaseItem item)
		{
			DotNetBarManager manager=GetDesignManager();
			if(item.Category!="" && item.Name!="" && !manager.Items.Contains(item.Name))
				manager.Items.Add(item.Copy());
			foreach(BaseItem i in item.SubItems)
				AutoCategorizeItem(i);
		}

		private void RefreshCategories()
		{
			DotNetBarManager manager=this.GetDesignManager();
			if(manager==null)
				return;
			// Load nodes from items
			m_catTable.Clear();
			foreach(DictionaryEntry o in manager.Items)
			{
				BaseItem objItem=o.Value as BaseItem;
				TreeNode itemNode=CategorizeItem(objItem);
				itemNode.Tag=objItem;

				itemNode.ImageIndex=GetItemImageIndex(objItem);
				itemNode.SelectedImageIndex=itemNode.ImageIndex;

				AddSubItems(objItem,itemNode);
			}
		}

		private TreeNode CategorizeItem(BaseItem newItem)
		{
			// Assign item to category
			if(newItem.Category=="")
				newItem.Category="(Untitled)";

			TreeNode parentNode=null;
			if(m_catTable.ContainsKey(newItem.Category))
				parentNode=m_catTable[newItem.Category] as TreeNode;
			else
			{
				parentNode=treeCategories.Nodes.Add(newItem.Category);
				parentNode.ImageIndex=5;
				parentNode.SelectedImageIndex=5;
				m_catTable.Add(newItem.Category,parentNode);
			}
			
			TreeNode newNode=parentNode.Nodes.Add(GetTreeItemText(newItem));
			newNode.ImageIndex=GetItemImageIndex(newItem);
			newNode.SelectedImageIndex=newNode.ImageIndex;
			newNode.Tag=newItem;
			return newNode;
		}
		#endregion

		#region Bars Support
		private void RefreshBars()
		{
			treeBars.Nodes.Clear();

			if(GetDesignBar()!=null)
			{
				try
				{
					Bar bar=this.GetDesignBar();
					this.Cursor=Cursors.WaitCursor;
					
					TreeNode barNode=treeBars.Nodes.Add(bar.Text);
					barNode.Tag=bar;
					barNode.ImageIndex=1;
					barNode.SelectedImageIndex=1;
					foreach(BaseItem objItem in bar.Items)
					{
						TreeNode itemNode=barNode.Nodes.Add(GetTreeItemText(objItem));
						itemNode.Tag=objItem;

						itemNode.ImageIndex=GetItemImageIndex(objItem);
						itemNode.SelectedImageIndex=itemNode.ImageIndex;

						AddSubItems(objItem,itemNode);
					}
					barNode.Expand();
				}
				finally
				{
					this.Cursor=Cursors.Arrow;
				}
				return;
			}

			if(this.GetDesignManager()==null)
				return;

			this.Cursor=Cursors.WaitCursor;

			DotNetBarManager manager=this.GetDesignManager();
			if(manager.TopDockSite==null && manager.BottomDockSite==null && manager.LeftDockSite==null && manager.RightDockSite==null)
			{
				// Context Menu editing ONLY
			}
			else
			{
			}

			foreach(Bar bar in manager.Bars)
			{
				DisplayTreeBar(bar);
			}

			this.Cursor=Cursors.Arrow;
		}
		private void DisplayTreeBar(Bar bar)
		{
			TreeNode barNode=treeBars.Nodes.Add(bar.Text);
			barNode.Tag=bar;
			barNode.ImageIndex=1;
			barNode.SelectedImageIndex=1;
			foreach(BaseItem objItem in bar.Items)
			{
				TreeNode itemNode=barNode.Nodes.Add(GetTreeItemText(objItem));
				itemNode.Tag=objItem;

				itemNode.ImageIndex=GetItemImageIndex(objItem);
				itemNode.SelectedImageIndex=itemNode.ImageIndex;

				AddSubItems(objItem,itemNode);
			}
		}
		#endregion

		#region Context Menus Support
		private void RefreshContextMenus()
		{
			treeMenus.Nodes.Clear();
			DotNetBarManager manager=this.GetDesignManager();
			if(manager==null)
				return;

			this.Cursor=Cursors.WaitCursor;
			try
			{
				// Load all popups
				foreach(BaseItem objItem in manager.ContextMenus)
				{
					TreeNode itemNode=treeMenus.Nodes.Add(GetTreeItemText(objItem));
					itemNode.Tag=objItem;

					itemNode.ImageIndex=GetItemImageIndex(objItem);
					itemNode.SelectedImageIndex=itemNode.ImageIndex;

					AddSubItems(objItem,itemNode);
				}
			}
			finally
			{
				this.Cursor=Cursors.Arrow;
			}
		}
		#endregion

		#region Item Tree Loading
		private void AddSubItems(BaseItem objItem, TreeNode parentNode)
		{
			if(objItem.SubItems.Count==0)
				return;
			foreach(BaseItem objChild in objItem.SubItems)
			{
				TreeNode itemNode=parentNode.Nodes.Add(GetTreeItemText(objChild));
				itemNode.Tag=objChild;

				itemNode.ImageIndex=GetItemImageIndex(objChild);
				itemNode.SelectedImageIndex=itemNode.ImageIndex;

				AddSubItems(objChild,itemNode);
			}
		}

		private string GetTreeItemText(BaseItem objItem)
		{
			if(objItem==null)
				return "";

			if(m_ShowItemText)
				return objItem.Text;
			else
				return objItem.Name;
		}

		private int GetItemImageIndex(BaseItem objItem)
		{
			int index=0;
			if(objItem is ButtonItem)
			{
				index=7;
			}
			else if(objItem is ComboBoxItem)
			{
				index=8;
			}
			else if(objItem is TextBoxItem)
			{
				index=9;
			}
			else if(objItem is CustomizeItem)
			{
				index=10;
			}
			else if(objItem is LabelItem)
			{
				index=11;
			}
			return index;
		}
		private void RefreshView()
		{
			this.RefreshBars();
			this.RefreshContextMenus();
			this.RefreshCategories();
			propertyBars.SelectedObject=null;
			propertyCategories.SelectedObject=null;
			propertyMenus.SelectedObject=null;
		}

		private void SetupToolbars()
		{
			if(!dotNetBarManager1.IsDefinitionLoaded)
				return;

			bool bVisible=true;

			if(m_ExternalBar!=null)
				bVisible=false;

			dotNetBarManager1.GetItem(CREATE_BARS_PARENT,true).Visible=bVisible;
			dotNetBarManager1.GetItem(SYNC_CATEGORIES,true).Visible=bVisible;
			dotNetBarManager1.GetItem(NEW_DOCK,true).Visible=bVisible;
			dotNetBarManager1.GetItem(NEW_DEFINITION,true).Visible=bVisible;
			dotNetBarManager1.GetItem(OPEN_DEFINITION,true).Visible=bVisible;
			dotNetBarManager1.GetItem(SAVE_DEFINITION,true).Visible=bVisible;
			dotNetBarManager1.GetItem(SAVE_DEFINITION_AS,true).Visible=bVisible;
			dotNetBarManager1.GetItem(LOAD_BAR,true).Visible=bVisible;
		}

		private void OnExternalManagerChanged()
		{
			if(m_ExternalManager!=null)
			{
				if(!dotNetBarManager1.IsDefinitionLoaded)
					dotNetBarManager1.ForceDefinitionLoad();
				this.GetDesignManager().Style=m_ExternalManager.Style;
				this.GetDesignManager().ThemeAware=m_ExternalManager.ThemeAware;
				this.GetDesignManager().UseGlobalColorScheme=m_ExternalManager.UseGlobalColorScheme;
				this.GetDesignManager().ColorScheme=m_ExternalManager.ColorScheme;
				this.GetDesignManager().ShowCustomizeContextMenu=m_ExternalManager.ShowCustomizeContextMenu;
				this.GetDesignManager().ShowResetButton=m_ExternalManager.ShowResetButton;
				this.GetDesignManager().ShowToolTips=m_ExternalManager.ShowToolTips;
				this.GetDesignManager().Images=m_ExternalManager.Images;
				this.GetDesignManager().ImagesLarge=m_ExternalManager.ImagesLarge;
				this.GetDesignManager().ImagesMedium=m_ExternalManager.ImagesMedium;
				if(this.Visible || !this.HasFloatingBars(m_ExternalManager))
					this.GetDesignManager().Definition=m_ExternalManager.Definition;
				else
					this.CreateLoadTimer();
			}
			SetupNavigationPane();
			SetupProperties();
			m_DataChanged=false;
		}

		private void CreateLoadTimer()
		{
			m_LoadTimer=new Timer();
			m_LoadTimer.Enabled=false;
			m_LoadTimer.Interval=100;
			m_LoadTimer.Tick+=new EventHandler(LoadTimerTick);
			if(m_Loaded)
				m_LoadTimer.Enabled=true;
		}

		private void LoadTimerTick(object sender, EventArgs e)
		{
			if(m_Loaded)
			{
				m_LoadTimer.Enabled=false;
				this.GetDesignManager().Definition=m_ExternalManager.Definition;
				m_LoadTimer.Dispose();
				m_LoadTimer=null;
				m_DataChanged=false;
				this.RefreshView();
			}
		}

		private bool HasFloatingBars(DotNetBarManager manager)
		{
			bool bRet=false;
			foreach(Bar bar in manager.Bars)
			{
				if(bar.DockSide==eDockSide.None && !bar.AutoHide)
				{
					bRet=true;
					break;
				}
			}
			return bRet;
		}

		private void OnExternalBarChanged()
		{
			if(m_ExternalBar!=null)
			{
				DotNetBarManager manager=definitionPreview.previewManager;
				manager.SuspendLayout=true;
				manager.Bars.ClearNonDocumentBars();
				manager.Items.Clear();
				manager.ContextMenus.Clear();
				manager.Images=m_ExternalBar.Images;
				manager.ImagesLarge=m_ExternalBar.ImagesLarge;
				manager.ImagesMedium=m_ExternalBar.ImagesMedium;
				manager.SuspendLayout=false;
				manager.Bars.Add(new Bar(m_ExternalBar.Text));
				manager.Bars[0].DockSide=eDockSide.Top;
				m_DesignBar=manager.Bars[0];
				RefreshDesignBar();
			}
			else
			{
				m_DesignBar=null;
			}
			SetupNavigationPane();
			SetupProperties();
			m_DataChanged=false;
		}

		private void RefreshDesignBar()
		{
			if(m_DesignBar!=null && m_ExternalBar!=null)
			{
				m_DesignBar.Definition=m_ExternalBar.Definition;
				m_DesignBar.DockSide=eDockSide.Top;
			}
		}

		private void SetupNavigationPane()
		{
			if(m_ExternalBar!=null)
			{
				buttonContext.Visible=false;
				buttonCategories.Visible=false;
				buttonBars.Checked=true;
			}
			else
			{
				if(m_ExternalManager.TopDockSite==null && 
					m_ExternalManager.BottomDockSite==null &&
					m_ExternalManager.LeftDockSite==null &&
					m_ExternalManager.RightDockSite==null)
				{
					// Manager provides popups only
					buttonContext.Visible=true;
					buttonContext.Checked=true;
					buttonCategories.Visible=false;
					buttonBars.Visible=false;
				}
				else
				{
					buttonContext.Visible=true;
					buttonCategories.Visible=true;
					buttonBars.Checked=true;
				}
			}
			navigationPane1.RecalcLayout();
		}

		private void SetupProperties()
		{
			if(m_ExternalBar!=null)
			{
				propertyBars.BrowsableAttributes=new AttributeCollection(new Attribute[] {BrowsableAttribute.Yes});
				propertyMenus.BrowsableAttributes=new AttributeCollection(new Attribute[] {BrowsableAttribute.Yes});
				propertyCategories.BrowsableAttributes=new AttributeCollection(new Attribute[] {BrowsableAttribute.Yes});
			}
			else
			{
                propertyBars.BrowsableAttributes = new AttributeCollection(new Attribute[] { DevCoBrowsable.Yes });
                propertyMenus.BrowsableAttributes = new AttributeCollection(new Attribute[] { DevCoBrowsable.Yes });
                propertyCategories.BrowsableAttributes = new AttributeCollection(new Attribute[] { DevCoBrowsable.Yes });
			}
		}
		#endregion

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(DotNetBarDesigner));
			this.navigationPane1 = new DevComponents.DotNetBar.NavigationPane();
			this.navigationPanePanel1 = new DevComponents.DotNetBar.NavigationPanePanel();
			this.propertyBars = new System.Windows.Forms.PropertyGrid();
			this.splitter1 = new DevComponents.DotNetBar.ExpandableSplitter();
			this.treeBars = new System.Windows.Forms.TreeView();
			this.imageList = new System.Windows.Forms.ImageList(this.components);
			this.buttonBars = new DevComponents.DotNetBar.ButtonItem();
			this.navigationPanePanel3 = new DevComponents.DotNetBar.NavigationPanePanel();
			this.propertyCategories = new System.Windows.Forms.PropertyGrid();
			this.splitterOffice20031 = new DevComponents.DotNetBar.ExpandableSplitter();
			this.treeCategories = new System.Windows.Forms.TreeView();
			this.buttonCategories = new DevComponents.DotNetBar.ButtonItem();
			this.navigationPanePanel2 = new DevComponents.DotNetBar.NavigationPanePanel();
			this.propertyMenus = new System.Windows.Forms.PropertyGrid();
			this.splitterOffice20032 = new DevComponents.DotNetBar.ExpandableSplitter();
			this.treeMenus = new System.Windows.Forms.TreeView();
			this.buttonContext = new DevComponents.DotNetBar.ButtonItem();
			this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
			this.definitionPreview = new DevComponents.DotNetBar.Design.DefinitionPreviewControl();
			this.splitter2 = new DevComponents.DotNetBar.ExpandableSplitter();
			#if !TRIAL
			this.dotNetBarManager1 = new DevComponents.DotNetBar.DotNetBarManager(this.components,true);
			#else
			this.dotNetBarManager1 = new DevComponents.DotNetBar.DotNetBarManager(this.components);
			#endif
			
			this.barBottomDockSite = new DevComponents.DotNetBar.DockSite();
			this.barLeftDockSite = new DevComponents.DotNetBar.DockSite();
			this.barRightDockSite = new DevComponents.DotNetBar.DockSite();
			this.barTopDockSite = new DevComponents.DotNetBar.DockSite();
			this.m_OpenFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.m_SaveFileDialog = new System.Windows.Forms.SaveFileDialog();
			this.navigationPane1.SuspendLayout();
			this.navigationPanePanel1.SuspendLayout();
			this.navigationPanePanel3.SuspendLayout();
			this.navigationPanePanel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// navigationPane1
			// 
			this.navigationPane1.ConfigureAddRemoveVisible = false;
			this.navigationPane1.ConfigureNavOptionsVisible = false;
			this.navigationPane1.Controls.AddRange(new System.Windows.Forms.Control[] {
																						  this.navigationPanePanel1,
																						  this.navigationPanePanel3,
																						  this.navigationPanePanel2,
																						  this.navigationPane1.TitlePanel});
			this.navigationPane1.Dock = System.Windows.Forms.DockStyle.Left;
			this.navigationPane1.ItemPaddingBottom = 2;
			this.navigationPane1.ItemPaddingTop = 2;
			this.navigationPane1.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
																						   this.buttonBars,
																						   this.buttonContext,
																						   this.buttonCategories});
			this.navigationPane1.Location = new System.Drawing.Point(0, 48);
			this.navigationPane1.Name = "navigationPane1";
			this.navigationPane1.NavigationBarHeight = 88;
			this.navigationPane1.Size = new System.Drawing.Size(304, 468);
			this.navigationPane1.TabIndex = 1;
			// 
			// navigationPane1.TitlePanel
			// 
			this.navigationPane1.TitlePanel.Dock = System.Windows.Forms.DockStyle.Top;
			this.navigationPane1.TitlePanel.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.navigationPane1.TitlePanel.Name = "panelEx1";
			this.navigationPane1.TitlePanel.Size = new System.Drawing.Size(304, 24);
			this.navigationPane1.TitlePanel.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
			this.navigationPane1.TitlePanel.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
			this.navigationPane1.TitlePanel.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
			this.navigationPane1.TitlePanel.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
			this.navigationPane1.TitlePanel.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
			this.navigationPane1.TitlePanel.Style.GradientAngle = 90;
			this.navigationPane1.TitlePanel.Style.MarginLeft = 4;
			this.navigationPane1.TitlePanel.TabIndex = 0;
			this.navigationPane1.TitlePanel.Text = "Bars";
			this.navigationPane1.PanelChanging += new DevComponents.DotNetBar.PanelChangingEventHandler(this.navigationPane1_PanelChanging);
			// 
			// navigationPanePanel1
			// 
			this.navigationPanePanel1.Controls.AddRange(new System.Windows.Forms.Control[] {
																							   this.propertyBars,
																							   this.splitter1,
																							   this.treeBars});
			this.navigationPanePanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.navigationPanePanel1.DockPadding.Left = 1;
			this.navigationPanePanel1.DockPadding.Right = 1;
			this.navigationPanePanel1.DockPadding.Top = 1;
			this.navigationPanePanel1.Location = new System.Drawing.Point(0, 24);
			this.navigationPanePanel1.Name = "navigationPanePanel1";
			this.navigationPanePanel1.ParentItem = this.buttonBars;
			this.navigationPanePanel1.Size = new System.Drawing.Size(304, 356);
			this.navigationPanePanel1.Style.Alignment = System.Drawing.StringAlignment.Center;
			this.navigationPanePanel1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
			this.navigationPanePanel1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground2;
			this.navigationPanePanel1.Style.BackgroundImagePosition = DevComponents.DotNetBar.eBackgroundImagePosition.Tile;
			this.navigationPanePanel1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
			this.navigationPanePanel1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
			this.navigationPanePanel1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
			this.navigationPanePanel1.Style.GradientAngle = 90;
			this.navigationPanePanel1.TabIndex = 2;
			// 
			// propertyBars
			// 
			this.propertyBars.BackColor = System.Drawing.SystemColors.Control;
			this.propertyBars.CommandsBackColor = System.Drawing.SystemColors.Window;
			this.propertyBars.CommandsVisibleIfAvailable = true;
			this.propertyBars.Dock = System.Windows.Forms.DockStyle.Fill;
			this.propertyBars.HelpBackColor = System.Drawing.SystemColors.ControlLight;
			this.propertyBars.LargeButtons = false;
			this.propertyBars.LineColor = System.Drawing.SystemColors.ControlLight;
			this.propertyBars.Location = new System.Drawing.Point(1, 150);
			this.propertyBars.Name = "propertyBars";
			this.propertyBars.Size = new System.Drawing.Size(302, 206);
			this.propertyBars.TabIndex = 3;
			this.propertyBars.Text = "propertyGrid1";
			this.propertyBars.ViewBackColor = System.Drawing.SystemColors.Window;
			this.propertyBars.ViewForeColor = System.Drawing.SystemColors.WindowText;
			this.propertyBars.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.GridPropertyValueChanged);
			// 
			// splitter1
			// 
			this.splitter1.Expandable = false;
			this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
			this.splitter1.Location = new System.Drawing.Point(1, 144);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(302, 6);
			this.splitter1.TabIndex = 4;
			this.splitter1.TabStop = false;
			// 
			// treeBars
			// 
			this.treeBars.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.treeBars.Dock = System.Windows.Forms.DockStyle.Top;
			this.treeBars.ImageList = this.imageList;
			this.treeBars.Location = new System.Drawing.Point(1, 1);
			this.treeBars.Name = "treeBars";
			this.treeBars.Size = new System.Drawing.Size(302, 143);
			this.treeBars.TabIndex = 2;
			this.treeBars.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TreeKeyDown);
			this.treeBars.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TreeMouseDown);
			this.treeBars.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeAfterSelect);
			this.treeBars.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.TreeBeforeSelect);
			this.treeBars.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.TreeAfterLabelEdit);
			this.treeBars.BeforeCollapse += new System.Windows.Forms.TreeViewCancelEventHandler(this.TreeBeforeCollapse);
			this.treeBars.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.TreeBeforeExpand);
			this.treeBars.HideSelection=false;
			// 
			// imageList
			// 
			this.imageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
			this.imageList.ImageSize = new System.Drawing.Size(16, 16);
			this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
			this.imageList.TransparentColor = System.Drawing.Color.Magenta;
			// 
			// buttonBars
			// 
			this.buttonBars.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
			this.buttonBars.Checked = true;
			this.buttonBars.Image = ((System.Drawing.Bitmap)(resources.GetObject("buttonBars.Image")));
			this.buttonBars.Name = "buttonBars";
			this.buttonBars.OptionGroup = "navBar";
			this.buttonBars.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2003;
			this.buttonBars.Text = "Bars";
			this.buttonBars.Tooltip = "View Toolbars, Menu bars and dockable windows";
			// 
			// navigationPanePanel3
			// 
			this.navigationPanePanel3.Controls.AddRange(new System.Windows.Forms.Control[] {
																							   this.propertyCategories,
																							   this.splitterOffice20031,
																							   this.treeCategories});
			this.navigationPanePanel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.navigationPanePanel3.DockPadding.Left = 1;
			this.navigationPanePanel3.DockPadding.Right = 1;
			this.navigationPanePanel3.DockPadding.Top = 1;
			this.navigationPanePanel3.Location = new System.Drawing.Point(0, 24);
			this.navigationPanePanel3.Name = "navigationPanePanel3";
			this.navigationPanePanel3.ParentItem = this.buttonCategories;
			this.navigationPanePanel3.Size = new System.Drawing.Size(304, 356);
			this.navigationPanePanel3.Style.Alignment = System.Drawing.StringAlignment.Center;
			this.navigationPanePanel3.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
			this.navigationPanePanel3.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground2;
			this.navigationPanePanel3.Style.BackgroundImagePosition = DevComponents.DotNetBar.eBackgroundImagePosition.Tile;
			this.navigationPanePanel3.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
			this.navigationPanePanel3.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
			this.navigationPanePanel3.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
			this.navigationPanePanel3.Style.GradientAngle = 90;
			this.navigationPanePanel3.TabIndex = 4;
			// 
			// propertyCategories
			// 
			this.propertyCategories.BackColor = System.Drawing.SystemColors.Control;
			this.propertyCategories.CommandsBackColor = System.Drawing.SystemColors.Window;
			this.propertyCategories.CommandsVisibleIfAvailable = true;
			this.propertyCategories.Dock = System.Windows.Forms.DockStyle.Fill;
			this.propertyCategories.HelpBackColor = System.Drawing.SystemColors.ControlLight;
			this.propertyCategories.HelpVisible = false;
			this.propertyCategories.LargeButtons = false;
			this.propertyCategories.LineColor = System.Drawing.SystemColors.ControlLight;
			this.propertyCategories.Location = new System.Drawing.Point(1, 150);
			this.propertyCategories.Name = "propertyCategories";
			this.propertyCategories.Size = new System.Drawing.Size(302, 206);
			this.propertyCategories.TabIndex = 6;
			this.propertyCategories.Text = "propertyGrid1";
			this.propertyCategories.ToolbarVisible = false;
			this.propertyCategories.ViewBackColor = System.Drawing.SystemColors.Window;
			this.propertyCategories.ViewForeColor = System.Drawing.SystemColors.WindowText;
			this.propertyCategories.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.GridPropertyValueChanged);
			// 
			// splitterOffice20031
			// 
			this.splitterOffice20031.Dock = System.Windows.Forms.DockStyle.Top;
			this.splitterOffice20031.Location = new System.Drawing.Point(1, 144);
			this.splitterOffice20031.Name = "splitterOffice20031";
			this.splitterOffice20031.Size = new System.Drawing.Size(302, 6);
			this.splitterOffice20031.Expandable=false;
			this.splitterOffice20031.TabIndex = 7;
			this.splitterOffice20031.TabStop = false;
			// 
			// treeCategories
			// 
			this.treeCategories.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.treeCategories.Dock = System.Windows.Forms.DockStyle.Top;
			this.treeCategories.ImageList = this.imageList;
			this.treeCategories.Location = new System.Drawing.Point(1, 1);
			this.treeCategories.Name = "treeCategories";
			this.treeCategories.Size = new System.Drawing.Size(302, 143);
			this.treeCategories.TabIndex = 5;
			this.treeCategories.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TreeKeyDown);
			this.treeCategories.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TreeMouseDown);
			this.treeCategories.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeAfterSelect);
			this.treeCategories.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.TreeBeforeSelect);
			this.treeCategories.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.TreeAfterLabelEdit);
			this.treeCategories.BeforeCollapse += new System.Windows.Forms.TreeViewCancelEventHandler(this.TreeBeforeCollapse);
			this.treeCategories.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.TreeBeforeExpand);
			this.treeCategories.HideSelection=false;
			// 
			// buttonCategories
			// 
			this.buttonCategories.Image = ((System.Drawing.Bitmap)(resources.GetObject("buttonCategories.Image")));
			this.buttonCategories.Name = "buttonCategories";
			this.buttonCategories.OptionGroup = "navBar";
			this.buttonCategories.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2003;
			this.buttonCategories.Text = "Categories";
			this.buttonCategories.Tooltip = "View Command Categories";
			// 
			// navigationPanePanel2
			// 
			this.navigationPanePanel2.Controls.AddRange(new System.Windows.Forms.Control[] {
																							   this.propertyMenus,
																							   this.splitterOffice20032,
																							   this.treeMenus});
			this.navigationPanePanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.navigationPanePanel2.DockPadding.Left = 1;
			this.navigationPanePanel2.DockPadding.Right = 1;
			this.navigationPanePanel2.DockPadding.Top = 1;
			this.navigationPanePanel2.Location = new System.Drawing.Point(0, 24);
			this.navigationPanePanel2.Name = "navigationPanePanel2";
			this.navigationPanePanel2.ParentItem = this.buttonContext;
			this.navigationPanePanel2.Size = new System.Drawing.Size(304, 356);
			this.navigationPanePanel2.Style.Alignment = System.Drawing.StringAlignment.Center;
			this.navigationPanePanel2.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
			this.navigationPanePanel2.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground2;
			this.navigationPanePanel2.Style.BackgroundImagePosition = DevComponents.DotNetBar.eBackgroundImagePosition.Tile;
			this.navigationPanePanel2.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
			this.navigationPanePanel2.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
			this.navigationPanePanel2.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
			this.navigationPanePanel2.Style.GradientAngle = 90;
			this.navigationPanePanel2.TabIndex = 3;
			// 
			// propertyMenus
			// 
			this.propertyMenus.BackColor = System.Drawing.SystemColors.Control;
			this.propertyMenus.CommandsBackColor = System.Drawing.SystemColors.Window;
			this.propertyMenus.CommandsVisibleIfAvailable = true;
			this.propertyMenus.Dock = System.Windows.Forms.DockStyle.Fill;
			this.propertyMenus.HelpBackColor = System.Drawing.SystemColors.ControlLight;
			this.propertyMenus.HelpVisible = false;
			this.propertyMenus.LargeButtons = false;
			this.propertyMenus.LineColor = System.Drawing.SystemColors.ControlLight;
			this.propertyMenus.Location = new System.Drawing.Point(1, 150);
			this.propertyMenus.Name = "propertyMenus";
			this.propertyMenus.Size = new System.Drawing.Size(302, 206);
			this.propertyMenus.TabIndex = 6;
			this.propertyMenus.Text = "propertyGrid1";
			this.propertyMenus.ToolbarVisible = false;
			this.propertyMenus.ViewBackColor = System.Drawing.SystemColors.Window;
			this.propertyMenus.ViewForeColor = System.Drawing.SystemColors.WindowText;
			this.propertyMenus.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.GridPropertyValueChanged);
			// 
			// splitterOffice20032
			// 
			this.splitterOffice20032.Expandable = false;
			this.splitterOffice20032.Dock = System.Windows.Forms.DockStyle.Top;
			this.splitterOffice20032.Location = new System.Drawing.Point(1, 144);
			this.splitterOffice20032.Name = "splitterOffice20032";
			this.splitterOffice20032.Size = new System.Drawing.Size(302, 6);
			this.splitterOffice20032.TabIndex = 7;
			this.splitterOffice20032.TabStop = false;
			// 
			// treeMenus
			// 
			this.treeMenus.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.treeMenus.Dock = System.Windows.Forms.DockStyle.Top;
			this.treeMenus.ImageList = this.imageList;
			this.treeMenus.Location = new System.Drawing.Point(1, 1);
			this.treeMenus.Name = "treeMenus";
			this.treeMenus.Size = new System.Drawing.Size(302, 143);
			this.treeMenus.TabIndex = 5;
			this.treeMenus.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TreeKeyDown);
			this.treeMenus.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TreeMouseDown);
			this.treeMenus.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeAfterSelect);
			this.treeMenus.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.TreeBeforeSelect);
			this.treeMenus.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.TreeAfterLabelEdit);
			this.treeMenus.BeforeCollapse += new System.Windows.Forms.TreeViewCancelEventHandler(this.TreeBeforeCollapse);
			this.treeMenus.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.TreeBeforeExpand);
			this.treeMenus.HideSelection=false;
			// 
			// buttonContext
			// 
			this.buttonContext.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
			this.buttonContext.Image = ((System.Drawing.Bitmap)(resources.GetObject("buttonContext.Image")));
			this.buttonContext.Name = "buttonContext";
			this.buttonContext.OptionGroup = "navBar";
			this.buttonContext.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2003;
			this.buttonContext.Text = "Context Menus";
			this.buttonContext.Tooltip = "View Context Menus";
			// 
			// panelEx1
			// 
			this.panelEx1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panelEx1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.panelEx1.Location = new System.Drawing.Point(310, 48);
			this.panelEx1.Name = "panelEx1";
			this.panelEx1.Size = new System.Drawing.Size(432, 25);
			this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
			this.panelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
			this.panelEx1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
			this.panelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
			this.panelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
			this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
			this.panelEx1.Style.GradientAngle = 90;
			this.panelEx1.TabIndex = 2;
			this.panelEx1.Text = "Layout Preview";
			// 
			// definitionPreview
			// 
			this.definitionPreview.BackColor = System.Drawing.SystemColors.Control;
			this.definitionPreview.Dock = System.Windows.Forms.DockStyle.Fill;
			this.definitionPreview.Location = new System.Drawing.Point(310, 73);
			this.definitionPreview.Name = "definitionPreview";
			this.definitionPreview.Size = new System.Drawing.Size(432, 443);
			this.definitionPreview.TabIndex = 3;
			this.definitionPreview.DataChanged+=new EventHandler(this.PreviewDataChanged);
			// 
			// splitter2
			// 
			this.splitter2.Location = new System.Drawing.Point(304, 48);
			this.splitter2.Name = "splitter2";
			this.splitter2.Size = new System.Drawing.Size(8, 468);
			this.splitter2.TabIndex = 4;
			this.splitter2.TabStop = false;
			this.splitter2.Dock=DockStyle.Left;
			this.splitter2.ExpandableControl=navigationPane1;
			// 
			// dotNetBarManager1
			// 
			this.dotNetBarManager1.AutoDispatchShortcuts.Add(DevComponents.DotNetBar.eShortcut.F1);
			this.dotNetBarManager1.AutoDispatchShortcuts.Add(DevComponents.DotNetBar.eShortcut.CtrlC);
			this.dotNetBarManager1.AutoDispatchShortcuts.Add(DevComponents.DotNetBar.eShortcut.CtrlA);
			this.dotNetBarManager1.AutoDispatchShortcuts.Add(DevComponents.DotNetBar.eShortcut.CtrlV);
			this.dotNetBarManager1.AutoDispatchShortcuts.Add(DevComponents.DotNetBar.eShortcut.CtrlX);
			this.dotNetBarManager1.AutoDispatchShortcuts.Add(DevComponents.DotNetBar.eShortcut.CtrlZ);
			this.dotNetBarManager1.AutoDispatchShortcuts.Add(DevComponents.DotNetBar.eShortcut.Del);
			this.dotNetBarManager1.AutoDispatchShortcuts.Add(DevComponents.DotNetBar.eShortcut.Ins);
			this.dotNetBarManager1.BottomDockSite = this.barBottomDockSite;
			this.dotNetBarManager1.DefinitionName = "DotNetBarDesigner.dotNetBarManager1.xml";
			this.dotNetBarManager1.LeftDockSite = this.barLeftDockSite;
			this.dotNetBarManager1.ParentForm = this;
			this.dotNetBarManager1.RightDockSite = this.barRightDockSite;
			this.dotNetBarManager1.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2003;
			this.dotNetBarManager1.TopDockSite = this.barTopDockSite;
			this.dotNetBarManager1.ItemClick += new System.EventHandler(this.dotNetBarManager1_ItemClick);
			this.dotNetBarManager1.UseHook=true;
			// 
			// barBottomDockSite
			// 
			this.barBottomDockSite.AccessibleRole = System.Windows.Forms.AccessibleRole.Window;
			this.barBottomDockSite.BackgroundImageAlpha = ((System.Byte)(255));
			this.barBottomDockSite.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.barBottomDockSite.Location = new System.Drawing.Point(0, 516);
			this.barBottomDockSite.Name = "barBottomDockSite";
			this.barBottomDockSite.Size = new System.Drawing.Size(742, 0);
			this.barBottomDockSite.TabIndex = 8;
			this.barBottomDockSite.TabStop = false;
			// 
			// barLeftDockSite
			// 
			this.barLeftDockSite.AccessibleRole = System.Windows.Forms.AccessibleRole.Window;
			this.barLeftDockSite.BackgroundImageAlpha = ((System.Byte)(255));
			this.barLeftDockSite.Dock = System.Windows.Forms.DockStyle.Left;
			this.barLeftDockSite.Location = new System.Drawing.Point(0, 48);
			this.barLeftDockSite.Name = "barLeftDockSite";
			this.barLeftDockSite.Size = new System.Drawing.Size(0, 468);
			this.barLeftDockSite.TabIndex = 5;
			this.barLeftDockSite.TabStop = false;
			// 
			// barRightDockSite
			// 
			this.barRightDockSite.AccessibleRole = System.Windows.Forms.AccessibleRole.Window;
			this.barRightDockSite.BackgroundImageAlpha = ((System.Byte)(255));
			this.barRightDockSite.Dock = System.Windows.Forms.DockStyle.Right;
			this.barRightDockSite.Location = new System.Drawing.Point(742, 48);
			this.barRightDockSite.Name = "barRightDockSite";
			this.barRightDockSite.Size = new System.Drawing.Size(0, 468);
			this.barRightDockSite.TabIndex = 6;
			this.barRightDockSite.TabStop = false;
			// 
			// barTopDockSite
			// 
			this.barTopDockSite.AccessibleRole = System.Windows.Forms.AccessibleRole.Window;
			this.barTopDockSite.BackgroundImageAlpha = ((System.Byte)(255));
			this.barTopDockSite.Dock = System.Windows.Forms.DockStyle.Top;
			this.barTopDockSite.Name = "barTopDockSite";
			this.barTopDockSite.Size = new System.Drawing.Size(742, 48);
			this.barTopDockSite.TabIndex = 7;
			this.barTopDockSite.TabStop = false;
			// 
			// m_OpenFileDialog
			// 
			this.m_OpenFileDialog.Filter = "DotNetBar Files (*.dnb, *.xml)|*.dnb;*.xml|All Files (*.*)|*.*";
			this.m_OpenFileDialog.ShowReadOnly = true;
			this.m_OpenFileDialog.Title = "Open DotNetBar Definition File";
			// 
			// m_SaveFileDialog
			// 
			this.m_SaveFileDialog.DefaultExt = "dnb";
			this.m_SaveFileDialog.FileName = "dotnetbardefinition";
			this.m_SaveFileDialog.Filter = "DotNetBar Files (*.dnb)|*.dnb|XML Files (*.xml)|*.xml|All Files (*.*)|*.*";
			this.m_SaveFileDialog.Title = "Save DotNetBar Definition";
			// 
			// DotNetBarDesigner
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(742, 516);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.definitionPreview,
																		  this.panelEx1,
																		  this.splitter2,
																		  this.navigationPane1,
																		  this.barLeftDockSite,
																		  this.barRightDockSite,
																		  this.barTopDockSite,
																		  this.barBottomDockSite});
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "DotNetBarDesigner";
			this.Text = "DotNetBar Designer";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.DotNetBarDesigner_Closing);
            this.Closed += new EventHandler(DotNetBarDesigner_Closed);
			this.Load += new System.EventHandler(this.DotNetBarDesigner_Load);
			this.navigationPane1.ResumeLayout(false);
			this.navigationPanePanel1.ResumeLayout(false);
			this.navigationPanePanel3.ResumeLayout(false);
			this.navigationPanePanel2.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		#region Toolbar and menu bar support

		private void dotNetBarManager1_ItemClick(object sender, System.EventArgs e)
		{
			BaseItem item=sender as BaseItem;

			if(item==null)
				return;

			PropertyGrid grid=propertyBars;
			if(navigationPane1.SelectedPanel==navigationPanePanel2)
				grid=propertyMenus;
			if(navigationPane1.SelectedPanel==navigationPanePanel3)
				grid=propertyCategories;

			switch(item.Name)
			{
				case CREATE_BARS_PARENT:
				{
                    if (!item.Expanded)
                        item.Expanded = true;
					break;
				}
				case OPEN_DEFINITION:
				{
					if(m_OpenFileDialog.ShowDialog()==DialogResult.OK && System.IO.File.Exists(m_OpenFileDialog.FileName))
					{
						DotNetBarManager manager=this.GetDesignManager();
						manager.LoadDefinition(m_OpenFileDialog.FileName);
						m_DefinitionFileName=m_OpenFileDialog.FileName;
						SetupProperties();
						RefreshView();
						m_DataChanged=true;
					}
					break;
				}
				case RESET_IMAGE:
				{
					m_DataChanged=true;
					if(grid.SelectedGridItem!=null && (grid.SelectedGridItem.PropertyDescriptor.PropertyType==typeof(System.Drawing.Image) || grid.SelectedGridItem.PropertyDescriptor.PropertyType==typeof(System.Drawing.Icon)))
					{
						grid.SelectedGridItem.PropertyDescriptor.SetValue(grid.SelectedObject,null);
					}
					grid.Refresh();
					break;
				}
				case CLOSE_DESIGNER:
				{
					this.DialogResult=DialogResult.OK;
					this.Close();
					break;
				}
				case SHOW_HELP:
				{
					if(m_HtmlHelp!=null)
						m_HtmlHelp.ShowContents();
					break;
				}
				case SHOW_HELP_SEARCH:
				{
					if(m_HtmlHelp!=null)
						m_HtmlHelp.ShowSearch();
					break;
				}
				case NEW_TOOLBAR:
				case NEW_MENUBAR:
				case NEW_STATUSBAR:
				case NEW_TASKBAR:
				{
					m_DataChanged=true;
					Bar bar;

					if(item.Name==NEW_MENUBAR)
					{
						bar=CreateObject(typeof(Bar)) as Bar;
						bar.Text="Main Menu";
						bar.MenuBar=true;
						bar.Stretch=true;
						bar.Name="mainmenu";
					}
					else if(item.Name==NEW_STATUSBAR)
					{
						bar=CreateObject(typeof(Bar)) as Bar;
						bar.Text="Status";
						bar.Stretch=true;
						bar.Name="statusBar"+((int)this.GetDesignManager().Bars.Count+1).ToString();
						bar.LayoutType=eLayoutType.Toolbar;
						bar.GrabHandleStyle=eGrabHandleStyle.ResizeHandle;
						bar.ItemSpacing=2;
					}
					else if(item.Name==NEW_TASKBAR)
					{
						bar=CreateObject(typeof(Bar)) as Bar;
						bar.Text="Task Pane";
						bar.Stretch=true;
						bar.Name="taskbar"+((int)this.GetDesignManager().Bars.Count+1).ToString();
						bar.LayoutType=eLayoutType.TaskList;
						bar.GrabHandleStyle=eGrabHandleStyle.Caption;
					}
					else
					{
						bar=CreateObject(typeof(Bar)) as Bar;
						bar.Text="My Bar";
						bar.Name="bar"+((int)this.GetDesignManager().Bars.Count+1).ToString();
					}
                
					//bar.SetDesignMode(true);

					TreeNode barNode=treeBars.Nodes.Add(bar.Text);
					barNode.Tag=bar;
					barNode.ImageIndex=3;
					barNode.SelectedImageIndex=3;
					bar.DockLine=this.GetDesignManager().Bars.Count;
					this.GetDesignManager().Bars.Add(bar);
					if(item.Name==NEW_TASKBAR)
						bar.DockSide=eDockSide.Right;
					else if(item.Name==NEW_STATUSBAR)
						bar.DockSide=eDockSide.Bottom;
					else
						bar.DockSide=eDockSide.Top;

					if(item.Name==NEW_MENUBAR || item.Name==NEW_TOOLBAR)
					{
						ButtonItem button=CreateObject(typeof(ButtonItem)) as ButtonItem;
						AssignItemName(button);
						button.Text=button.Name;
                        //button.GenerateCommandLink = true;
                        //UpdateCommandLink(button);
						bar.Items.Add(button);
						bar.RecalcLayout();
						TreeNode itemTreeNode=barNode.Nodes.Add(GetTreeItemText(button));
						itemTreeNode.Tag=button;
						itemTreeNode.ImageIndex=GetItemImageIndex(button);
						itemTreeNode.SelectedImageIndex=itemTreeNode.ImageIndex;
						itemTreeNode.EnsureVisible();
						treeBars.SelectedNode=barNode;
						barNode.EnsureVisible();

					}
					else if(item.Name==NEW_STATUSBAR)
					{
						LabelItem status=CreateObject(typeof(LabelItem)) as LabelItem;
						AssignItemName(status);
						status.Text="Status Ready...";
						status.BorderType=eBorderType.SingleLine;
						status.Width=136;
						bar.Items.Add(status);
						bar.RecalcLayout();
						TreeNode itemTreeNode=barNode.Nodes.Add(GetTreeItemText(status));
						itemTreeNode.Tag=status;
						itemTreeNode.ImageIndex=GetItemImageIndex(status);
						itemTreeNode.SelectedImageIndex=itemTreeNode.ImageIndex;
						itemTreeNode.EnsureVisible();

						treeBars.SelectedNode=barNode;
						barNode.EnsureVisible();
					}
					else
					{
						treeBars.SelectedNode=barNode;
						barNode.EnsureVisible();
					}
					break;
				}
				case NEW_DOCKBAR:
				{
					m_DataChanged=true;
					Bar bar=CreateObject(typeof(Bar)) as Bar;;
					bar.Text="Dockable Window";
					bar.Stretch=true;
					bar.LayoutType=eLayoutType.DockContainer;
					bar.GrabHandleStyle=eGrabHandleStyle.Caption;
					bar.Name="dockwindow"+this.GetDesignManager().Bars.Count.ToString();

					DockContainerItem dockItem=CreateObject(typeof(DockContainerItem)) as DockContainerItem;
					AssignItemName(dockItem);
					dockItem.Text="Dock Container";
					bar.Items.Add(dockItem);

					TreeNode barNode=treeBars.Nodes.Add(bar.Text);
					barNode.Tag=bar;
					barNode.ImageIndex=3;
					barNode.SelectedImageIndex=3;
					bar.DockLine=this.GetDesignManager().Bars.Count;
					this.GetDesignManager().Bars.Add(bar);
					bar.DockSide=eDockSide.Left;

					TreeNode itemTreeNode=barNode.Nodes.Add(GetTreeItemText(dockItem));
					itemTreeNode.Tag=dockItem;
					itemTreeNode.ImageIndex=GetItemImageIndex(dockItem);
					itemTreeNode.SelectedImageIndex=itemTreeNode.ImageIndex;
					itemTreeNode.EnsureVisible();

					treeBars.SelectedNode=barNode;
					barNode.EnsureVisible();
					break;
				}
				case DELETE:
				{
					TreeView tree=GetSelectedTreeView();
					DotNetBarManager manager=this.GetDesignManager();

					if(tree.SelectedNode==null)
						break;

					m_DataChanged=true;

					if(tree.SelectedNode.Nodes.Count>0)
					{
						if(MessageBox.Show(this,"Are you sure you want to delete selected item?","DotNetBar Editor",MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.No)
							break;
					}

					TreeNode nextNode=tree.SelectedNode.NextVisibleNode;

					if(tree.SelectedNode.Tag is BaseItem)
					{
						BaseItem selectedItem=tree.SelectedNode.Tag as BaseItem;
						Bar cont=selectedItem.ContainerControl as Bar;

						TreeNode topParentNode=tree.SelectedNode;
						while(topParentNode.Parent!=null)
							topParentNode=topParentNode.Parent;
					
						if(selectedItem.Parent!=null)
							selectedItem.Parent.SubItems.Remove(selectedItem);
						else if(tree==treeCategories)
							manager.Items.Remove(selectedItem);
						else if(tree==treeMenus)
							manager.ContextMenus.Remove(selectedItem);
					
						DeleteComponent(selectedItem);

						tree.SelectedNode.Tag=null;
						TreeNode parentNode=tree.SelectedNode.Parent;
						tree.Nodes.Remove(tree.SelectedNode);
						// If it is last node under one of the categories remove parent too
						if(parentNode!=null && parentNode.Parent==null && parentNode.Nodes.Count==0 && tree==treeCategories)
							tree.Nodes.Remove(parentNode);
						if(cont!=null)
							cont.RecalcLayout();
					
					}
					else if(tree.SelectedNode.Tag is Bar)
					{
						Bar bar=tree.SelectedNode.Tag as Bar;

						manager.Bars.Remove(bar);
						DeleteComponent(bar);
						tree.SelectedNode.Tag=null;
						tree.Nodes.Remove(tree.SelectedNode);
					}
					else if(tree==treeCategories && tree.SelectedNode.Parent==null)
					{
						// Delete all items within this category
						foreach(TreeNode node in tree.SelectedNode.Nodes)
						{
							BaseItem childItem=node.Tag as BaseItem;
							if(childItem!=null)
							{
								manager.Items.Remove(childItem);
								DeleteComponent(childItem);
							}
							node.Tag=null;
						}
						tree.SelectedNode.Remove();
					}

					if(tree.Nodes.Count==0)
					{
						propertyBars.SelectedObject=null;
						propertyMenus.SelectedObject=null;
						propertyCategories.SelectedObject=null;
					}
					
					RefreshDesignBar();

                    EnableCommands(tree, tree.SelectedNode);
					break;
				}
				case NEW_DEFINITION:
				{
					DotNetBarManager manager=this.GetDesignManager();
					if(manager!=null)
					{
						manager.SuspendLayout=true;
						manager.Bars.ClearNonDocumentBars();
						manager.Items.Clear();
						manager.ContextMenus.Clear();
						manager.SuspendLayout=false;
						SetupProperties();
						this.RefreshView();
					}
					break;
				}
				case SAVE_DEFINITION:
				case SAVE_DEFINITION_AS:
				{
					SaveDefinition((item.Name==SAVE_DEFINITION_AS));
					break;
				}
                case SAVE_CLOSE:
                {
                    if (m_ExternalManager != null)
                        m_ExternalManager.Definition = this.GetDesignManager().Definition;
                    else
                    {
                        if (SaveDefinition(false) == DialogResult.Cancel)
                            return;
                    }
                    m_DataChanged = false;
                    this.DialogResult = DialogResult.Yes;
                    this.Close();
                    break;
                }
				case SAVE_BAR_AS:
				{
					TreeView tree=GetSelectedTreeView();

					// Save currently selected bar
					if(tree.SelectedNode==null || !(tree.SelectedNode.Tag is Bar))
						return;
				
					Bar bar=tree.SelectedNode.Tag as Bar;
					string stitle=m_SaveFileDialog.Title;
					string defaultExt=m_SaveFileDialog.DefaultExt;
					string fileName=m_SaveFileDialog.FileName;
					string filter=m_SaveFileDialog.Filter;

					m_SaveFileDialog.Title="Save Bar Definition";
					m_SaveFileDialog.DefaultExt = "xml";
					m_SaveFileDialog.FileName = (bar.Name!=""?bar.Name:"MyBar");
					m_SaveFileDialog.Filter = "DotNetBar Bar Files (*.xml)|*.xml|All Files (*.*)|*.*";
				
					if(m_SaveFileDialog.ShowDialog()==DialogResult.OK)
					{
						bar.SaveDefinition(m_SaveFileDialog.FileName);
					}

					m_SaveFileDialog.Title=stitle;
					m_SaveFileDialog.DefaultExt=defaultExt;
					m_SaveFileDialog.FileName=fileName;
					m_SaveFileDialog.Filter=filter;
					break;
				}
				case LOAD_BAR:
				{
					string defaultExt=m_OpenFileDialog.DefaultExt;
					string filter=m_OpenFileDialog.Filter;
					m_OpenFileDialog.DefaultExt="xml";
					m_OpenFileDialog.Filter = "DotNetBar Bar Files (*.xml)|*.xml|All Files (*.*)|*.*";
					if(m_OpenFileDialog.ShowDialog()==DialogResult.OK && System.IO.File.Exists(m_OpenFileDialog.FileName))
					{
						Bar bar=new Bar();
						try
						{
							bar.LoadDefinition(m_OpenFileDialog.FileName);
						}
						catch(Exception ex)
						{
							MessageBox.Show("File '"+m_OpenFileDialog.FileName+"' does not appear to be valid Bar file.\n\rException has been generated while loading: "+ex.Source+": "+ex.Message+"\n\r"+ex.StackTrace);
							return;
						}
						bar.Dispose();
						bar=new Bar();
						this.GetDesignManager().SuspendLayout=true;
						this.GetDesignManager().Bars.Add(bar);
						bar.LoadDefinition(m_OpenFileDialog.FileName);
						this.GetDesignManager().SuspendLayout=false;
						this.DisplayTreeBar(bar);
					}
					m_OpenFileDialog.DefaultExt=defaultExt;
					m_OpenFileDialog.Filter=filter;
					break;
				}
				case MOVE_LEFT:
				case MOVE_RIGHT:
				case MOVE_UP:
				case MOVE_DOWN:
				{
					MoveSelectedItem(item.Name);
					RefreshDesignBar();
					break;
				}
				case SYNC_CATEGORIES:
				{
					RescanCategories();
					break;
				}
				case NEW_BUTTON:
				{
					ButtonItem btn=CreateObject(typeof(ButtonItem)) as ButtonItem;
					AssignItemName(btn);
					btn.Text=btn.Name;
					AddNewItem(btn);
					RefreshDesignBar();
                    //if ((btn.ContainerControl is Bar || btn.Parent != null) && navigationPane1.CheckedButton != buttonCategories)
                    //{
                    //    btn.GenerateCommandLink = true;
                    //    UpdateCommandLink(btn);
                    //}
					break;
				}
				case NEW_TEXTBOX:
				{
					TextBoxItem tb=CreateObject(typeof(TextBoxItem)) as TextBoxItem;
					AssignItemName(tb);
					tb.Text=tb.Name;
					AddNewItem(tb);
					RefreshDesignBar();
					break;
				}
				case NEW_COMBO:
				{
					ComboBoxItem cb=CreateObject(typeof(ComboBoxItem)) as ComboBoxItem;
                    TypeDescriptor.GetProperties(cb)["DisplayMember"].SetValue(cb, "Text");
					AssignItemName(cb);
					cb.Text=cb.Name;
					AddNewItem(cb);
					RefreshDesignBar();
					break;
				}
				case NEW_CUSTOMIZE:
				{
					CustomizeItem cust=CreateObject(typeof(CustomizeItem)) as CustomizeItem;
					AssignItemName(cust);
					AddNewItem(cust);
					RefreshDesignBar();
					break;
				}
				case NEW_LABEL:
				{
					LabelItem li=CreateObject(typeof(LabelItem)) as LabelItem;
					li.BorderType=eBorderType.SingleLine;
					AssignItemName(li);
					li.Text=li.Name;
					AddNewItem(li);
					RefreshDesignBar();
					break;
				}
				case NEW_COLORPICKER:
				{
					ColorPickerDropDown cp=CreateObject(typeof(ColorPickerDropDown)) as ColorPickerDropDown;
					AssignItemName(cp);
					cp.Text=cp.Name;
					AddNewItem(cp);
					RefreshDesignBar();
					break;
				}
				case NEW_MDILIST:
				{
					MdiWindowListItem mdi=CreateObject(typeof(MdiWindowListItem)) as MdiWindowListItem;
					mdi.Text="MDI Window List";
					AssignItemName(mdi);
					AddNewItem(mdi);
					RefreshDesignBar();
					break;
				}
				case NEW_CONTROLCONT:
				{
					ControlContainerItem cci=CreateObject(typeof(ControlContainerItem)) as ControlContainerItem;
					AssignItemName(cci);
					AddNewItem(cci);
					RefreshDesignBar();
					break;
				}
				case NEW_DOCK:
				{
					DockContainerItem dci=CreateObject(typeof(DockContainerItem)) as DockContainerItem;
					dci.Text="Dockable Window";
					AssignItemName(dci);
					AddNewItem(dci);
					RefreshDesignBar();
					break;
				}
				case NEW_PROGRESS:
				{
					ProgressBarItem pb=CreateObject(typeof(ProgressBarItem)) as ProgressBarItem;
					pb.SetDesignMode(true);
					if(this.GetDesignManager()!=null)
						pb.Style=this.GetDesignManager().Style;
					else if(this.GetDesignBar()!=null)
						pb.Style=this.GetDesignBar().Style;
					pb.ResetBackgroundStyle();
					pb.SetDesignMode(false);
					AssignItemName(pb);
					pb.Text=pb.Name;
					AddNewItem(pb);
					RefreshDesignBar();
					break;
				}
			}
		}

		private DialogResult SaveDefinition(bool saveAs)
		{
			DialogResult result=DialogResult.OK;
			if(m_DefinitionFileName!="" && !saveAs)
				this.GetDesignManager().SaveDefinition(m_DefinitionFileName);
			else
			{
				result=m_SaveFileDialog.ShowDialog();
				// Save definition as...
				if(result==DialogResult.OK)
				{
					this.GetDesignManager().SaveDefinition(m_SaveFileDialog.FileName);
					m_DefinitionFileName=m_SaveFileDialog.FileName;
				}
			}
            return result;
		}

		private void AssignItemName(BaseItem newItem)
		{
			DotNetBarManager manager=this.GetDesignManager();
			Bar bar=this.GetDesignBar();

			if(m_Designer==null || bar==null)
			{
				string name=newItem.GetType().Name;
				long id=1;
				if(manager!=null)
				{
					while(manager.GetItem(name+id,true)!=null)
						id++;
				}
				else if(bar!=null)
				{
					while(bar.GetItem(name+id)!=null)
						id++;
				}
				newItem.Name=name+id;
			}
		}

		private void AddNewItem(BaseItem newItem)
		{
			m_DataChanged=true;
			TreeView tree=GetSelectedTreeView();
			DotNetBarManager manager=this.GetDesignManager();

			if(tree.SelectedNode!=null && tree.SelectedNode.Tag is BaseItem)
			{
				newItem.Style=((BaseItem)tree.SelectedNode.Tag).Style;
				if(tree.SelectedNode.Tag is PopupItem && newItem is PopupItem)
					((PopupItem)newItem).PopupType=((PopupItem)tree.SelectedNode.Tag).PopupType;
			}
			else if(tree.SelectedNode!=null && tree.SelectedNode.Tag is Bar)
			{
				newItem.Style=((Bar)tree.SelectedNode.Tag).Style;
				if(newItem is PopupItem && ((Bar)tree.SelectedNode.Tag).MenuBar)
					((PopupItem)newItem).PopupType=ePopupType.Menu;
			}

			// We need to determine is new item being added to the Categories
			TreeNode itemNode=null;

			if(tree==treeCategories)
			{
				// Assign category to new item
				if(tree.SelectedNode==null)
					newItem.Category="(Untitled)";
				else if(tree.SelectedNode.Parent==null)
					newItem.Category=tree.SelectedNode.Text;
				else
					newItem.Category=((BaseItem)tree.SelectedNode.Tag).Category;

				manager.Items.Add(newItem);                
				itemNode=CategorizeItem(newItem);
			}
			else if(tree==treeMenus)
			{
				if(tree.SelectedNode!=null && tree.SelectedNode.Tag is BaseItem)
				{
					itemNode=tree.SelectedNode.Nodes.Add(GetTreeItemText(newItem));

					BaseItem objParent=(BaseItem)tree.SelectedNode.Tag;
					int iPos=-1;
					// New Items are always added before any system items which are by default kept at the end
					if(objParent.SubItems.Count>0 && !newItem.SystemItem)
					{
						iPos=GetAppendPosition(objParent);
					}
					objParent.SubItems.Add(newItem,iPos);
				}
				else
				{
					itemNode=tree.Nodes.Add(GetTreeItemText(newItem));
					manager.ContextMenus.Add(newItem);
				}
			}
			else if(tree.SelectedNode!=null)
			{
				Control cont=null;
				if(tree.SelectedNode.Tag is BaseItem)
				{
					BaseItem objParent=(BaseItem)tree.SelectedNode.Tag;
					int iPos=-1;
					// New Items are always added before any system items which are by default kept at the end
					if(objParent.SubItems.Count>0 && !newItem.SystemItem)
					{
						iPos=GetAppendPosition(objParent);
					}
					objParent.SubItems.Add(newItem,iPos);
					itemNode=new TreeNode(GetTreeItemText(newItem));
					if(iPos!=-1)
						tree.SelectedNode.Nodes.Insert(iPos,itemNode);
					else
                        tree.SelectedNode.Nodes.Add(itemNode);
					cont=newItem.ContainerControl as Control;
					if(cont==null)
						((BaseItem)tree.SelectedNode.Tag).Refresh();

				}
				else if(tree.SelectedNode.Tag is Bar)
				{
					Bar bar=(Bar)tree.SelectedNode.Tag;
					int iPos=-1;
					// New Items are always added before any system items which are by default kept at the end
					if(bar.Items.Count>0 && !newItem.SystemItem)
					{
						iPos=GetAppendPosition(bar.ItemsContainer);
					}
					bar.Items.Add(newItem,iPos);
					itemNode=new TreeNode(GetTreeItemText(newItem));
					if(iPos!=-1)
						tree.SelectedNode.Nodes.Insert(iPos,itemNode);
					else
						tree.SelectedNode.Nodes.Add(itemNode);
					cont=bar;
				}
				if(cont!=null && cont is Bar)
					((Bar)cont).RecalcLayout();
			}

			itemNode.Tag=newItem;
			itemNode.ImageIndex=GetItemImageIndex(newItem);
			itemNode.SelectedImageIndex=itemNode.ImageIndex;

			itemNode.EnsureVisible();
			tree.SelectedNode=itemNode;
		}

		private TreeView GetPanelTreeView(NavigationPanePanel panel)
		{
			TreeView tree=treeBars;
			if(panel==navigationPanePanel2)
				tree=treeMenus;
			else if(panel==navigationPanePanel3)
				tree=treeCategories;
			return tree;
		}
		private TreeView GetSelectedTreeView()
		{
			TreeView tree=treeBars;
			if(navigationPane1.SelectedPanel==navigationPanePanel2)
				tree=treeMenus;
			else if(navigationPane1.SelectedPanel==navigationPanePanel3)
				tree=treeCategories;
			return tree;
		}

		private void MoveSelectedItem(string Direction)
		{
			TreeView tree=GetSelectedTreeView();
			if(tree.SelectedNode==null)
				return;
			BaseItem objItem=tree.SelectedNode.Tag as BaseItem;
			if(objItem==null)
				return;

			m_DataChanged=true;
			DotNetBarManager manager=this.GetDesignManager();

			bool bCategoryItem=false;
			if(tree==treeCategories)
				bCategoryItem=true;
			
			bool bPopupsItem=false;
			if(tree==treeMenus)
				bPopupsItem=true;

			TreeNode selNode=tree.SelectedNode;
			TreeNodeCollection parentCollection=null;
			if(selNode.Parent==null)
				parentCollection=tree.Nodes;
			else
				parentCollection=selNode.Parent.Nodes;
		

			BaseItem objParent=objItem.Parent;

			int i=0;
			if(objParent!=null)
				i=objParent.SubItems.IndexOf(objItem);
			else
				i=selNode.Index;

			if(Direction==MOVE_UP && i>0)
			{
				if(objParent!=null)
				{
					objParent.SubItems.Remove(objItem);
					objParent.SubItems.Add(objItem,i-1);
					if(objParent.ContainerControl is Bar)
						((Bar)objParent.ContainerControl).RecalcLayout();
				}
				
				i=selNode.Index;
				selNode.Remove();
				parentCollection.Insert(i-1,selNode);
				tree.SelectedNode=selNode;
				selNode.EnsureVisible();
			}
			else if(Direction==MOVE_RIGHT && i>0)
			{
				BaseItem objNewParent=null;
				
				if(bCategoryItem)
					manager.Items.Remove(objItem);
				else if(bPopupsItem)
					manager.ContextMenus.Remove(objItem);

				if(objParent!=null)
				{
					objNewParent=objParent.SubItems[i-1];
					objParent.SubItems.Remove(objItem);
					if(objParent.ContainerControl is Bar)
						((Bar)objParent.ContainerControl).RecalcLayout();
				}
				else
				{
					objNewParent=selNode.PrevNode.Tag as BaseItem;
				}
				objNewParent.SubItems.Add(objItem,GetAppendPosition(objNewParent));
				objNewParent.Refresh();
				if(objNewParent.ContainerControl is Bar)
					((Bar)objNewParent.ContainerControl).RecalcLayout();

				i=selNode.Index;
				TreeNode newParent=selNode.PrevNode;
				selNode.Remove();
				newParent.Nodes.Add(selNode);
				tree.SelectedNode=selNode;
				selNode.EnsureVisible();
			}
			else if(Direction==MOVE_DOWN && ((objParent!=null && i<objParent.SubItems.Count-1) || (objParent==null && selNode.Index<selNode.Parent.Nodes.Count-1)))
			{
				if(objParent!=null)
				{
					objParent.SubItems.Remove(objItem);
					objParent.SubItems.Add(objItem,i+1);
					if(objParent.ContainerControl is Bar)
						((Bar)objParent.ContainerControl).RecalcLayout();
				}

				i=selNode.Index;
				selNode.Remove();
				parentCollection.Insert(i+1,selNode);
				tree.SelectedNode=selNode;
				selNode.EnsureVisible();
			}
			if(Direction==MOVE_LEFT && objParent!=null && (!(objParent is GenericItemContainer) || !((GenericItemContainer)objParent).SystemItem))
			{
				BaseItem objNewParent=null;
				objNewParent=objParent.Parent;
				if(objNewParent!=null)
				{
					i=objNewParent.SubItems.IndexOf(objParent)+1;
					objParent.SubItems.Remove(objItem);
					if(objParent.ContainerControl is Bar)
						((Bar)objParent.ContainerControl).RecalcLayout();
					objNewParent.SubItems.Add(objItem,i);
					if(objNewParent.ContainerControl is Bar)
						((Bar)objNewParent.ContainerControl).RecalcLayout();
				}
				else
				{
					objParent.SubItems.Remove(objItem);
					if(objParent.ContainerControl is Bar)
						((Bar)objParent.ContainerControl).RecalcLayout();
				}
				
				if(tree==treeCategories)
					manager.Items.Add(objItem);
				else if(tree==treeMenus)
					manager.ContextMenus.Add(objItem);
				
				TreeNode nodeParent=selNode.Parent;
				selNode.Remove();
				if(nodeParent!=null && nodeParent.Parent!=null)
				{
					nodeParent.Parent.Nodes.Insert(nodeParent.Index+1,selNode);
					tree.SelectedNode=selNode;
					selNode.EnsureVisible();
				}
				else if(bPopupsItem)
				{
					if(nodeParent!=null)
					{
						tree.Nodes.Insert(nodeParent.Index+1,selNode);
					}
					else
						tree.Nodes.Add(selNode);
					tree.SelectedNode=selNode;
					selNode.EnsureVisible();
				}
			}
		}

		private void CopyMoveToClick(object sender, System.EventArgs e)
		{
			TreeView tree=GetSelectedTreeView();
			
			BaseItem item=sender as BaseItem;
			BaseItem itemSel=tree.SelectedNode.Tag as BaseItem;
			BaseItem itemOriginal=itemSel;

			bool bMove=(item.Parent.Name==MOVE_TO);
			if(!bMove)
			{
				itemSel=itemSel.Copy();
				if((Control.ModifierKeys & Keys.Control)==Keys.Control)
					itemSel.Name=itemOriginal.Name;
				else
					itemSel.Name="item_"+itemSel.Id;
			}

			if((item.Tag is string && (string)item.Tag=="cat") || item.Name==TO_CATEGORIES)
			{
				// To Categories
				if(item.Name!=TO_CATEGORIES)
					itemSel.Category=item.Text;
				if(bMove)
				{
					if(itemSel.Parent!=null)
						itemSel.Parent.SubItems.Remove(itemSel);
					tree.SelectedNode.Remove();
				}
				if(this.GetDesignManager().Items.Contains(itemSel.Name))
				{
					string sDupName=itemSel.Name;
					itemSel.Name="item_"+itemSel.Id;
					MessageBox.Show("Item with name: '"+sDupName+"' already exists. Item that you are trying to move will be renamed.");
				}
				this.GetDesignManager().Items.Add(itemSel);
				treeCategories.SelectedNode=CategorizeItem(itemSel);
				treeCategories.SelectedNode.EnsureVisible();
			}
			else if(item.Name==TO_CONTEXTMENUS || item.Tag is BaseItem)
			{
				if(bMove)
				{
					if(itemSel.Parent!=null)
						itemSel.Parent.SubItems.Remove(itemSel);
					tree.SelectedNode.Remove();
				}

				if(item.Name==TO_CONTEXTMENUS)
				{
					TreeNode itemNode=treeMenus.Nodes.Add(GetTreeItemText(itemSel));
					itemNode.Tag=itemSel;
					itemNode.ImageIndex=GetItemImageIndex(itemSel);
					itemNode.SelectedImageIndex=itemNode.ImageIndex;
					AddSubItems(itemSel,itemNode);
					this.GetDesignManager().ContextMenus.Add(itemSel);
					itemNode.EnsureVisible();
				}
				else
				{
					BaseItem objParent=item.Tag as BaseItem;
					objParent.SubItems.Add(itemSel);
					foreach(TreeNode node in treeMenus.Nodes)
					{
						if(node.Tag==objParent)
						{
							TreeNode itemNode=node.Nodes.Add(GetTreeItemText(itemSel));
							itemNode.Tag=itemSel;
							itemNode.ImageIndex=GetItemImageIndex(itemSel);
							itemNode.SelectedImageIndex=itemNode.ImageIndex;
							AddSubItems(itemSel,itemNode);
							itemNode.EnsureVisible();
							break;
						}
					}
				}
			}
			else if(item.Tag is TreeNode)
			{
				// To Bar
				TreeNode barNode=item.Tag as TreeNode;
				Bar bar=barNode.Tag as Bar;
				if(bMove)
				{
					if(itemSel.Parent!=null)
						itemSel.Parent.SubItems.Remove(itemSel);
					tree.SelectedNode.Remove();
				}

				bar.Items.Add(itemSel,GetAppendPosition(bar.ItemsContainer));
				TreeNode itemNode=barNode.Nodes.Add(GetTreeItemText(itemSel));
				itemNode.Tag=itemSel;
				itemNode.ImageIndex=GetItemImageIndex(itemSel);
				itemNode.SelectedImageIndex=itemNode.ImageIndex;
				AddSubItems(itemSel,itemNode);
				treeBars.SelectedNode=itemNode;
				treeBars.SelectedNode.EnsureVisible();
				bar.RecalcLayout();
			}
            
			// Clear Copy To and Move To
			item=dotNetBarManager1.ContextMenus[CONTEXT_MENU];
			item.SubItems[COPY_TO].SubItems.Clear();
			item.SubItems[MOVE_TO].SubItems.Clear();
		}

		private int GetAppendPosition(BaseItem objParent)
		{
			int iPos=-1;
			for(int i=objParent.SubItems.Count-1;i>=0;i--)
			{
				if(objParent.SubItems[i].SystemItem)
					iPos=i;
				else
					break;
			}
			return iPos;
		}

		private object CreateObject(Type type)
		{
			if(m_Designer!=null && m_ExternalBar!=null)
				return m_Designer.CreateComponent(type);
			else
				return type.Assembly.CreateInstance(type.FullName);
		}

        private object CreateObject(Type type, string name)
        {
            if (m_Designer != null)
            {
                return m_Designer.CreateComponent(type, name);
            }

            return null;
        }
		private void DeleteComponent(IComponent o)
		{
            if (m_Designer != null)
            {
                if (o is BaseItem)
                {
                    RemoveCommandLinks(o as BaseItem);
                }
                else if (o is Bar)
                {
                    Bar bar = o as Bar;
                    foreach (BaseItem item in bar.Items)
                        RemoveCommandLinks(item);
                }
            }

			if(m_Designer!=null && m_ExternalBar!=null)
				m_Designer.DestroyComponent(o);
			else
				o.Dispose();
		}

        private void RemoveCommandLinks(BaseItem item)
        {
            //CommandLink link = GetCommandLink(GetCommandLinkName(item));
            //if (link != null && !m_CommandLinksDeleted.Contains(link))
            //    m_CommandLinksDeleted.Add(link);

            //foreach (BaseItem sub in item.SubItems)
            //{
            //    RemoveCommandLinks(sub);
            //}
        }
		#endregion

		#region Grid and Tree support
		private void PreviewDataChanged(object sender, EventArgs e)
		{
			m_DataChanged=true;
		}

		private void GridViewMouseDown(object sender, EventArgs e)
		{
			PropertyGrid grid=sender as PropertyGrid;

			if(grid.SelectedGridItem==null)
				return;
			if(!(grid.SelectedGridItem.PropertyDescriptor.PropertyType==typeof(System.Drawing.Image)) && !(grid.SelectedGridItem.PropertyDescriptor.PropertyType==typeof(System.Drawing.Icon)))
				return;
			PopupItem popup=dotNetBarManager1.ContextMenus[RESET_IMAGE_POPUP] as PopupItem;
			popup.PopupMenu(Control.MousePosition);		
		}

		private void GridPropertyValueChanged(object s, System.Windows.Forms.PropertyValueChangedEventArgs e)
		{
			m_DataChanged=true;
			// If user changes the category of the item that is in categories we need to
			// reflect that change and move item to the right category
			TreeNode node=null;
			if(s==propertyBars)
				node=treeBars.SelectedNode;
			else if(s==propertyCategories)
				node=treeCategories.SelectedNode;
			else if(s==propertyMenus)
				node=treeMenus.SelectedNode;

            if (node == null)
                return;

			BaseItem item=node.Tag as BaseItem;

            if (e.ChangedItem.PropertyDescriptor.Name == "Name" && e.ChangedItem.Value != e.OldValue && item!=null)
            {
                //if (item.GenerateCommandLink)
                //{
                //    CommandLink link = GetCommandLink(GetCommandLinkName(e.OldValue.ToString()));
                //    if (link != null)
                //        m_CommandLinksDeleted.Add(link);
                //    UpdateCommandLink(item);
                //}
            }

			if(e.ChangedItem.PropertyDescriptor.Name=="Category" && e.ChangedItem.Value!=e.OldValue && s==propertyCategories)
			{
				if(node.Tag==null || !(node.Tag is BaseItem))
					return;
	            
				BaseItem objItem=node.Tag as BaseItem;
				node.Remove();
				node=CategorizeItem(objItem);
				node.ImageIndex=GetItemImageIndex(objItem);
				node.SelectedImageIndex=node.ImageIndex;
				AddSubItems(objItem,node);
				node.EnsureVisible();
				treeCategories.SelectedNode=node;
			}
			else if(e.ChangedItem.PropertyDescriptor.Name=="Name" && e.ChangedItem.Value!=e.OldValue && !m_ShowItemText)
			{
				node.Text=(string)e.ChangedItem.Value;
			}
			else if(e.ChangedItem.PropertyDescriptor.Name=="Text" && e.ChangedItem.Value!=e.OldValue && m_ShowItemText)
			{
				node.Text=(string)e.ChangedItem.Value;
			}
			else if(e.ChangedItem.PropertyDescriptor.Name=="DockLine" || e.ChangedItem.PropertyDescriptor.Name=="DockOffset")
			{
				Bar bar=node.Tag as Bar;
				if(bar!=null)
				{
					bar.DockLine=bar.DockLine;
					bar.RecalcLayout();
				}
			}
            else if (e.ChangedItem.PropertyDescriptor.Name == "GenerateCommandLink" && e.ChangedItem.Value != e.OldValue)
            {
                UpdateCommandLink(item);
            }

			DotNetBarManager manager=this.GetDesignManager();

			if(e.ChangedItem.PropertyDescriptor.Name=="Name" && manager!=null && s==propertyCategories)
			{
				if(manager.Items.Contains(e.ChangedItem.Value.ToString()))
				{
					item.Name=e.OldValue.ToString();
					((PropertyGrid)s).Refresh();
					MessageBox.Show("Item with that name already exists.");
				}
				else
				{
					manager.Items.Remove(e.OldValue.ToString());
					manager.Items.Add(item);						
				}
			}

			if(item!=null)
			{
				Bar barContainer=item.ContainerControl as Bar;
				if(barContainer!=null)
					barContainer.RecalcLayout();
			}

			RefreshDesignBar();
		}

        //private string GetCommandLinkName(string itemName)
        //{
        //    return CommandLink.GetCommandLinkName(itemName);
        //}

        //private string GetCommandLinkName(BaseItem item)
        //{
        //    return GetCommandLinkName(item.Name);
        //}

        //private Type GetCommandLinkType(BaseItem item)
        //{
        //    if (item is ButtonItem)
        //        return typeof(CommandLinkButtonItem);
        //    else if (item is LabelItem)
        //        return typeof(CommandLinkLabelItem);
        //    else if (item is TextBoxItem)
        //        return typeof(CommandLinkTextBoxItem);
        //    else if (item is ColorPickerDropDown)
        //        return typeof(CommandLinkColorPickerDropDown);
        //    else if (item is ControlContainerItem)
        //        return typeof(CommandLinkControlContainerItem);
        //    else if (item is DockContainerItem)
        //        return typeof(CommandLinkDockContainerItem);
        //    else if (item is ProgressBarItem)
        //        return typeof(CommandLinkProgressBarItem);
        //    else if (item is ComboBoxItem)
        //        return typeof(CommandLinkComboBoxItem);
        //    else
        //        return null;
        //}

        /// <summary>
        /// Updates design-time command link for the item
        /// </summary>
        /// <param name="item"></param>
        private void UpdateCommandLink(BaseItem item)
        {
            if (m_Designer == null)
            {
                MessageBox.Show("Designer for the DotNetBarManager not assigned. This could be becouse you are using design in stand-alone mode. To use command links open designer by double-clicking DotNetBarManager in VS.NET");
                return;
            }

            //if (item.GenerateCommandLink)
            //{
            //    CommandLink link = GetCommandLink(GetCommandLinkName(item));
            //    if (link != null)
            //    {
            //        if (m_CommandLinksDeleted.Contains(link))
            //            m_CommandLinksDeleted.Remove(link);
            //        else
            //        {
            //            if (GetCommandLinkType(item) != link.GetType())
            //            {
            //                item.GenerateCommandLink = false;
            //                MessageBox.Show("Command Link with this item name already exists and is of different type. Change your item name to generate correct Command Link.");
            //            }
            //        }
            //        return;
            //    }

            //    try
            //    {
            //        Type linkType = GetCommandLinkType(item);
            //        if(linkType!=null)
            //            link = CreateObject(linkType, GetCommandLinkName(item.Name)) as CommandLink;
            //        else
            //        {
            //            MessageBox.Show("Selected item does not support Command Links.");
            //            item.GenerateCommandLink = false;
            //        }
            //    }
            //    catch
            //    {
            //        MessageBox.Show("Cannot create command link with the name '" + GetCommandLinkName(item.Name) + "' becouse object with that name already exists on the form. Please change the name of the item.");
            //        link = null;
            //        item.GenerateCommandLink = false;
            //    }

            //    if (link != null)
            //    {
            //        m_CommandLinksCreated.Add(link);
            //        TypeDescriptor.GetProperties(link)["Manager"].SetValue(link, m_ExternalManager);
            //    }
            //}
            //else
            //{
            //    // Remove the CommandLink
            //    CommandLink link = GetCommandLink(GetCommandLinkName(item.Name));
            //    if (link != null)
            //    {
            //        m_CommandLinksDeleted.Add(link);
            //        if (m_CommandLinksCreated.Contains(link))
            //            m_CommandLinksCreated.Remove(link);
            //    }
            //}
        }

        //private CommandLink GetCommandLink(string name)
        //{
        //    foreach (CommandLink link in m_ExternalManager.CommandLinks)
        //    {
        //        if (link.Name == name)
        //            return link;
        //    }

        //    return null;
        //}

		private void TreeKeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			TreeView tree=sender as TreeView;
			if(e.KeyCode==Keys.F2 && tree.SelectedNode!=null && tree.LabelEdit)
				tree.SelectedNode.BeginEdit();
		}

		private void TreeMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if(e.Button!=MouseButtons.Right)
				return;

			TreeView tree=sender as TreeView;
            
			TreeNode node=tree.GetNodeAt(e.X,e.Y);
			if(node!=null)
				tree.SelectedNode=node;
			else if(tree==treeMenus)
				tree.SelectedNode=null;

			// Popup menu
			ButtonItem popup=dotNetBarManager1.ContextMenus[CONTEXT_MENU] as ButtonItem;
			
			// If MoveTo and CopyTo are visible add items to them
			if(popup.SubItems[COPY_TO].Visible)
			{
				BaseItem itemCopyTo=popup.SubItems[COPY_TO];
				BaseItem itemMoveTo=popup.SubItems[MOVE_TO];
				
				itemCopyTo.SubItems.Clear();
				itemMoveTo.SubItems.Clear();
				bool bBars=true,bCategories=false, bPopups=false;
				// Don't show categories for category items...
				
				if(buttonContext.Visible)
					bPopups=true;
				if(buttonCategories.Visible)
					bCategories=true;

				if(bBars)
				{
					foreach(TreeNode barNode in treeBars.Nodes)
					{
						BaseItem newItem=new ButtonItem();
						newItem.Text=barNode.Text;
						newItem.Tag=barNode;
						newItem.Click+=new System.EventHandler(this.CopyMoveToClick);
						itemCopyTo.SubItems.Add(newItem);
						itemMoveTo.SubItems.Add(newItem.Copy());
					}
				}

				if(bCategories)
				{
					ButtonItem cat=new ButtonItem(TO_CATEGORIES);
					cat.Text="Categories";
					cat.BeginGroup=true;
					cat.Click+=new System.EventHandler(this.CopyMoveToClick);
					itemCopyTo.SubItems.Add(cat);
					itemMoveTo.SubItems.Add(cat.Copy());

					foreach(TreeNode catNode in treeCategories.Nodes)
					{
						BaseItem newItem=new ButtonItem();
						newItem.Text=catNode.Text;
						newItem.Click+=new System.EventHandler(this.CopyMoveToClick);
						newItem.Tag="cat";
						itemCopyTo.SubItems.Add(newItem);
						itemMoveTo.SubItems.Add(newItem.Copy());
					}
				}
				if(bPopups)
				{
					BaseItem pi=new ButtonItem(TO_CONTEXTMENUS);
					pi.BeginGroup=true;
					pi.Text="Popups";
					pi.Click+=new System.EventHandler(this.CopyMoveToClick);
					itemCopyTo.SubItems.Add(pi);
					itemMoveTo.SubItems.Add(pi.Copy());

					foreach(BaseItem objItem in this.GetDesignManager().ContextMenus)
					{
						BaseItem newItem=new ButtonItem();
						newItem.Text=(objItem.Text=="")?objItem.Name:objItem.Text;
						newItem.Tag=objItem;
						newItem.Click+=new System.EventHandler(this.CopyMoveToClick);
						itemCopyTo.SubItems.Add(newItem);
						itemMoveTo.SubItems.Add(newItem.Copy());						
					}
				}
			}

			Point pt=new Point(e.X,e.Y);
			pt=tree.PointToScreen(pt);
			popup.PopupMenu(pt);
		}

        private void SelectObjectInPropertyGrid(object obj)
        {
            PropertyGrid propertyGrid=null;
            TreeView tree = null;
            if (navigationPane1.CheckedButton == buttonBars)
            {
                propertyGrid = propertyBars;
                tree = treeBars;
            }
            else if (navigationPane1.CheckedButton == buttonCategories)
            {
                propertyGrid = propertyCategories;
                tree = treeCategories;
            }
            else if (navigationPane1.CheckedButton == buttonContext)
            {
                propertyGrid = propertyMenus;
                tree = treeMenus;
            }

            propertyGrid.SelectedObject = obj;

            // Find object in tree and select it
            if (tree.SelectedNode != null && tree.SelectedNode.Tag == obj)
                return;

            TreeNode node = FindObjectInNodes(tree.Nodes, obj);
            tree.SelectedNode = node;
            if (node != null)
                node.EnsureVisible();
        }

        private TreeNode FindObjectInNodes(TreeNodeCollection col, object obj)
        {
            foreach (TreeNode node in col)
            {
                if (node.Tag == obj)
                    return node;
                else if (node.Nodes.Count > 0)
                {
                    TreeNode tn = FindObjectInNodes(node.Nodes, obj);
                    if (tn != null)
                        return tn;
                }
            }

            return null;
        }

		private void TreeAfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			TreeView tree=sender as TreeView;
			PropertyGrid propertyGrid=null;
			if(tree==treeBars)
				propertyGrid=propertyBars;
			else if(tree==treeCategories)
				propertyGrid=propertyCategories;
			else if(tree==treeMenus)
				propertyGrid=propertyMenus;

			tree.LabelEdit=false;
			if(e.Node.Tag==null)
			{
				propertyGrid.SelectedObject=null;
				return;
			}

			if(e.Node.Tag is BaseItem)
			{
				propertyGrid.SelectedObject=e.Node.Tag;
				tree.LabelEdit=true;

				// Now way to detect when Items collection has changed, reset changed flag if combo box is selected...
				if(e.Node.Tag is ComboBoxItem)
					m_DataChanged=true;
			}
			else if(e.Node.Tag is Bar)
			{
				propertyGrid.SelectedObject=(Bar)e.Node.Tag;

			}
			else
			{
				propertyGrid.SelectedObject=null;
			}
		}

		private void TreeBeforeSelect(object sender, System.Windows.Forms.TreeViewCancelEventArgs e)
		{
			EnableCommands(sender as TreeView,e.Node);
		}

		private void EnableCommands(TreeView tree, TreeNode selectedNode)
		{
			// Disable/Enable toolbar items...
			bool bLeftEnabled=false, bRightEnabled=false, bUpEnabled=false, bDownEnabled=false;
			DotNetBarManager manager=this.GetDesignManager();

			BaseItem popupItem=dotNetBarManager1.ContextMenus[CONTEXT_MENU];

			if(selectedNode!=null && selectedNode.Tag is BaseItem)
			{
				BaseItem objItem=selectedNode.Tag as BaseItem;
				int i=0;
				if(objItem.Parent!=null)
					i=objItem.Parent.SubItems.IndexOf(objItem);
				else
					i=selectedNode.Index;
				if(i>0)
				{
					bUpEnabled=true;
					bRightEnabled=true;
				}
				int iCount=0;
				if(objItem.Parent!=null)
					iCount=objItem.Parent.SubItems.Count-1;
				else if(selectedNode.Parent!=null)
					iCount=selectedNode.Parent.Nodes.Count-1;
				else
					iCount=selectedNode.TreeView.Nodes.Count-1;

				if(i<iCount)
					bDownEnabled=true;

				if((objItem.Parent!=null && !objItem.Parent.SystemItem && (!(objItem.Parent is GenericItemContainer) || !((GenericItemContainer)objItem.Parent).SystemContainer)) || (selectedNode.Parent!=null && selectedNode.Parent.Tag is BaseItem))
					bLeftEnabled=true;
			}

			dotNetBarManager1.GetItem(MOVE_LEFT).Enabled=bLeftEnabled;
			dotNetBarManager1.GetItem(MOVE_RIGHT).Enabled=bRightEnabled;
			dotNetBarManager1.GetItem(MOVE_UP).Enabled=bUpEnabled;
			dotNetBarManager1.GetItem(MOVE_DOWN).Enabled=bDownEnabled;

			if(selectedNode!=null && selectedNode.Tag is BaseItem)
			{
				popupItem.SubItems[COPY_TO].Visible=true;
				popupItem.SubItems[MOVE_TO].Visible=true;
			}
			else
			{
				popupItem.SubItems[COPY_TO].Visible=false;
				popupItem.SubItems[MOVE_TO].Visible=false;
			}

			if(selectedNode==null && tree==treeBars)
			{
				dotNetBarManager1.GetItem(NEW_BUTTON).Enabled=false;
				dotNetBarManager1.GetItem(NEW_TEXTBOX).Enabled=false;
				dotNetBarManager1.GetItem(NEW_COMBO).Enabled=false;
				dotNetBarManager1.GetItem(NEW_CUSTOMIZE).Enabled=false;
				dotNetBarManager1.GetItem(NEW_LABEL).Enabled=false;
				dotNetBarManager1.GetItem(NEW_COLORPICKER).Enabled=false;
				dotNetBarManager1.GetItem(NEW_MDILIST).Enabled=false;
				dotNetBarManager1.GetItem(NEW_CONTROLCONT).Enabled=false;
				dotNetBarManager1.GetItem(NEW_DOCK).Enabled=false;
				dotNetBarManager1.GetItem(NEW_PROGRESS).Enabled=false;
				dotNetBarManager1.GetItem(DELETE).Enabled=false;
				return;
			}
			else if(tree==treeCategories)
			{
				dotNetBarManager1.GetItem(NEW_BUTTON).Enabled=true;
				dotNetBarManager1.GetItem(NEW_TEXTBOX).Enabled=true;
				dotNetBarManager1.GetItem(NEW_COMBO).Enabled=true;
				dotNetBarManager1.GetItem(NEW_CUSTOMIZE).Enabled=true;
				dotNetBarManager1.GetItem(NEW_LABEL).Enabled=true;
				dotNetBarManager1.GetItem(NEW_COLORPICKER).Enabled=true;
				dotNetBarManager1.GetItem(NEW_MDILIST).Enabled=true;
				dotNetBarManager1.GetItem(NEW_CONTROLCONT).Enabled=true;
				dotNetBarManager1.GetItem(NEW_DOCK).Enabled=true;
				dotNetBarManager1.GetItem(NEW_PROGRESS).Enabled=true;
				if(selectedNode!=null && selectedNode.Tag is BaseItem)
					dotNetBarManager1.GetItem(DELETE).Enabled=true;
				else
					dotNetBarManager1.GetItem(DELETE).Enabled=false;
				return;
			}

			if(selectedNode!=null && selectedNode.Tag is Bar)
				dotNetBarManager1.GetItem(SAVE_BAR_AS).Enabled=true;
			else
				dotNetBarManager1.GetItem(SAVE_BAR_AS).Enabled=false;

			bool newButton=true, newTextBox=true, newCombo=true, newCustomize=true, newLabel=true, newMdiList=true, newControlCont=true,
				newDock=false, newProgress=true;

			if(selectedNode!=null && selectedNode.Tag is Bar && ((Bar)selectedNode.Tag).LayoutType==eLayoutType.DockContainer)
			{
				newButton=false;
				newTextBox=false;
				newCombo=false;
				newCustomize=false;
				newLabel=false;
				newMdiList=false;
				newControlCont=false;
				newDock=true;
				newProgress=false;
			}
			else if(selectedNode!=null && selectedNode.Tag is DockContainerItem)
			{
				newButton=false;
				newTextBox=false;
				newCombo=false;
				newCustomize=false;
				newLabel=false;
				newMdiList=false;
				newControlCont=false;
				newDock=false;
				newProgress=false;
			}

			dotNetBarManager1.GetItem(NEW_BUTTON).Enabled=newButton;
			dotNetBarManager1.GetItem(NEW_TEXTBOX).Enabled=newTextBox;
			dotNetBarManager1.GetItem(NEW_COMBO).Enabled=newCombo;
			dotNetBarManager1.GetItem(NEW_CUSTOMIZE).Enabled=newCustomize;
			dotNetBarManager1.GetItem(NEW_LABEL).Enabled=newLabel;
			dotNetBarManager1.GetItem(NEW_COLORPICKER).Enabled=newLabel;
			dotNetBarManager1.GetItem(NEW_MDILIST).Enabled=newMdiList;
			dotNetBarManager1.GetItem(NEW_CONTROLCONT).Enabled=newControlCont;
			dotNetBarManager1.GetItem(NEW_DOCK).Enabled=newDock;
			dotNetBarManager1.GetItem(NEW_PROGRESS).Enabled=newProgress;

			if(selectedNode!=null && (selectedNode.Tag is BaseItem || selectedNode.Tag is Bar) && !(this.GetDesignBar()!=null && selectedNode!=null && selectedNode.Tag is Bar))
				dotNetBarManager1.GetItem(DELETE).Enabled=true;
			else
				dotNetBarManager1.GetItem(DELETE).Enabled=false;
		}

		private void TreeAfterLabelEdit(object sender, System.Windows.Forms.NodeLabelEditEventArgs e)
		{
			if(e.CancelEdit)
				return;

			BaseItem objItem=e.Node.Tag as BaseItem;
			if(m_ShowItemText)
				objItem.Text=e.Label;
			else
				objItem.Name=e.Label;
			
			propertyBars.Refresh();
			propertyMenus.Refresh();
			propertyCategories.Refresh();
			m_DataChanged=true;
		}

		private void TreeBeforeCollapse(object sender, System.Windows.Forms.TreeViewCancelEventArgs e)
		{
			if(e.Node.ImageIndex==6)
			{
				e.Node.ImageIndex=5;
				e.Node.SelectedImageIndex=5;
			}
		}

		private void TreeBeforeExpand(object sender, System.Windows.Forms.TreeViewCancelEventArgs e)
		{
			if(e.Node.ImageIndex==5)
			{
				e.Node.ImageIndex=6;
				e.Node.SelectedImageIndex=6;
			}
		}

		private void navigationPane1_PanelChanging(object sender, DevComponents.DotNetBar.PanelChangingEventArgs e)
		{
			
			if(this.dotNetBarManager1.GetItem(CREATE_BARS_PARENT,true)==null)
				return;
			
			TreeView tree=GetPanelTreeView(e.NewPanel);
			if(tree.Nodes.Count==0)
				EnableCommands(tree,null);

			if(e.NewPanel==navigationPanePanel1)
				this.dotNetBarManager1.GetItem(CREATE_BARS_PARENT,true).Enabled=true;
			else
				this.dotNetBarManager1.GetItem(CREATE_BARS_PARENT,true).Enabled=false;
		}

		#endregion

		#region GridViewSubclass
		private class GridViewSubclass:NativeWindow
		{
			const int WM_CONTEXTMENU=0x007B;
			public event EventHandler OnRightMouseDown;
			public PropertyGrid ParentGrid;
			protected override void WndProc(ref Message m)
			{
				if(m.Msg==WM_CONTEXTMENU)
				{
					if(OnRightMouseDown!=null)
						OnRightMouseDown(this.ParentGrid,new EventArgs());
					return;
				}
				base.WndProc(ref m);
			}
		}
		#endregion

		#region Public Interface
		/// <summary>
		/// Gets or sets the DotNetBar manager that is being designed by this designer.
		/// </summary>
		public DotNetBarManager ExternalManager
		{
			get {return m_ExternalManager;}
			set
			{
				m_ExternalManager=value;
				this.OnExternalManagerChanged();
			}
		}

		/// <summary>
		/// Gets or sets the external bar that is designed by this designer.
		/// </summary>
		public Bar ExternalBar
		{
			get {return m_ExternalBar;}
			set
			{
				m_ExternalBar=value;
				this.OnExternalBarChanged();
			}
		}

		/// <summary>
		/// Gets or sets design-time designer services used to interact with design-time environment.
		/// </summary>
		public DevComponents.DotNetBar.Design.IDesignerServices DesignerServices
		{
			get {return m_Designer;}
			set {m_Designer=value;}
		}
		#endregion
	}
}
