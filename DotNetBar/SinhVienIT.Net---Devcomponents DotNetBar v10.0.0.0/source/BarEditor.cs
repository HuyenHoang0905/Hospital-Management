using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Summary description for BarEditor.
	/// </summary>
	public class BarEditor : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.TreeView barTree;
		private System.Windows.Forms.PropertyGrid propertyGrid1;
		private System.Windows.Forms.Splitter splitter1;
		private System.ComponentModel.IContainer components;
		private DevComponents.DotNetBar.DockSite barTopDockSite;
		private DevComponents.DotNetBar.DockSite barBottomDockSite;
		private DevComponents.DotNetBar.DockSite barLeftDockSite;
		private DevComponents.DotNetBar.DockSite barRightDockSite;
		private DevComponents.DotNetBar.DotNetBarManager barManager;
		private System.Windows.Forms.ImageList imageList;

		private DotNetBarManager m_DotNetBar;
		private Bar m_Bar=null;
		private TreeNode m_BarsNode=null,m_CategoriesNode=null,m_PopupsNode=null;

		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.OpenFileDialog m_OpenFileDialog;
		private Hashtable m_catTable;
		private System.Windows.Forms.SaveFileDialog m_SaveFileDialog;
		private System.Windows.Forms.ImageList m_BarImages;
		private string m_DefinitionFileName="";
		private bool m_ShowItemText=true;
		private GridViewSubclass m_GridViewSubclass=null;
		private bool m_DataChanged=false;
		private HTMLHelp m_HtmlHelp=null;

		internal IDesignerServices _barDesigner=null;

		public BarEditor(DotNetBarManager dotnetbar)
		{
			m_DotNetBar=dotnetbar;
			Initialize();
		}

		public BarEditor(Bar bar)
		{
            m_Bar=bar;
			Initialize();
		}

		private void Initialize()
		{
			InitializeComponent();
			if(m_Bar!=null)
				propertyGrid1.BrowsableAttributes=new AttributeCollection(new Attribute[] {BrowsableAttribute.Yes});
			else
				propertyGrid1.BrowsableAttributes=new AttributeCollection(new Attribute[] {DevCoBrowsable.Yes});
			propertyGrid1.CommandsVisibleIfAvailable=false;
			LoadResourceImages();
			m_catTable=new Hashtable(20);
			
			m_GridViewSubclass=new GridViewSubclass();
			m_GridViewSubclass.OnRightMouseDown+=new EventHandler(this.GridViewMouseDown);

			foreach(Control ctrl in propertyGrid1.Controls)
			{
				if(ctrl.GetType().ToString().IndexOf("PropertyGridView")>=0)
				{
					m_GridViewSubclass.AssignHandle(ctrl.Handle);
					break;
				}
			}

			if(m_DotNetBar!=null && !m_DotNetBar.IsDesignTime() || m_Bar!=null)
			{
				btnCancel.Visible=false;
				btnClose.Location=btnCancel.Location;
			}

			CreateToolbar();
			RefreshView();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(BarEditor));
			this.panel1 = new System.Windows.Forms.Panel();
			this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.barTree = new System.Windows.Forms.TreeView();
			this.imageList = new System.Windows.Forms.ImageList(this.components);
		#if TRIAL
			this.barManager = new DevComponents.DotNetBar.DotNetBarManager(this.components);
		#else
			this.barManager = new DevComponents.DotNetBar.DotNetBarManager(this.components,false);
		#endif
			this.barBottomDockSite = new DevComponents.DotNetBar.DockSite();
			this.m_BarImages = new System.Windows.Forms.ImageList(this.components);
			this.barLeftDockSite = new DevComponents.DotNetBar.DockSite();
			this.barRightDockSite = new DevComponents.DotNetBar.DockSite();
			this.barTopDockSite = new DevComponents.DotNetBar.DockSite();
			this.m_OpenFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.m_SaveFileDialog = new System.Windows.Forms.SaveFileDialog();
			this.btnClose = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.panel1.Controls.AddRange(new System.Windows.Forms.Control[] {
																				 this.propertyGrid1,
																				 this.splitter1,
																				 this.barTree});
			this.panel1.Location = new System.Drawing.Point(8, 48);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(520, 232);
			this.panel1.TabIndex = 5;
			// 
			// propertyGrid1
			// 
			this.propertyGrid1.CommandsVisibleIfAvailable = true;
			this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.propertyGrid1.LargeButtons = false;
			this.propertyGrid1.LineColor = System.Drawing.SystemColors.ScrollBar;
			this.propertyGrid1.Location = new System.Drawing.Point(182, 0);
			this.propertyGrid1.Name = "propertyGrid1";
			this.propertyGrid1.Size = new System.Drawing.Size(338, 232);
			this.propertyGrid1.TabIndex = 1;
			this.propertyGrid1.Text = "propertyGrid1";
			this.propertyGrid1.ViewBackColor = System.Drawing.SystemColors.Window;
			this.propertyGrid1.ViewForeColor = System.Drawing.SystemColors.WindowText;
			this.propertyGrid1.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.OnPropertyValueChanged);
			// 
			// splitter1
			// 
			this.splitter1.Location = new System.Drawing.Point(176, 0);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(6, 232);
			this.splitter1.TabIndex = 2;
			this.splitter1.TabStop = false;
			// 
			// barTree
			// 
			this.barTree.Dock = System.Windows.Forms.DockStyle.Left;
			this.barTree.HideSelection = false;
			this.barTree.ImageList = this.imageList;
			this.barTree.Name = "barTree";
			this.barTree.Size = new System.Drawing.Size(176, 232);
			this.barTree.TabIndex = 0;
			this.barTree.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TreeKeyDown);
			this.barTree.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TreeMouseDown);
			this.barTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.ItemSelected);
			this.barTree.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.BeforeItemSelect);
			this.barTree.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.ItemEdited);
			this.barTree.BeforeCollapse += new System.Windows.Forms.TreeViewCancelEventHandler(this.NodeCollapsing);
			this.barTree.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.NodeExpanding);
			// 
			// imageList
			// 
			this.imageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
			this.imageList.ImageSize = new System.Drawing.Size(16, 16);
			this.imageList.TransparentColor = System.Drawing.Color.Magenta;
			// 
			// barManager
			// 
			this.barManager.BottomDockSite = this.barBottomDockSite;
			this.barManager.Images = this.m_BarImages;
			this.barManager.ImagesLarge = null;
			this.barManager.ImagesMedium = null;
			this.barManager.LeftDockSite = this.barLeftDockSite;
			this.barManager.ParentForm = this;
			this.barManager.RightDockSite = this.barRightDockSite;
			this.barManager.TopDockSite = this.barTopDockSite;
			this.barManager.UseHook = true;
			this.barManager.ItemClick += new System.EventHandler(this.BarItemClick);
			// 
			// barBottomDockSite
			// 
			this.barBottomDockSite.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.barBottomDockSite.Location = new System.Drawing.Point(0, 325);
			this.barBottomDockSite.Name = "barBottomDockSite";
			this.barBottomDockSite.Size = new System.Drawing.Size(536, 0);
			this.barBottomDockSite.TabIndex = 8;
			this.barBottomDockSite.TabStop = false;
			// 
			// m_BarImages
			// 
			this.m_BarImages.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
			this.m_BarImages.ImageSize = new System.Drawing.Size(16, 16);
			this.m_BarImages.TransparentColor = System.Drawing.Color.Magenta;
			// 
			// barLeftDockSite
			// 
			this.barLeftDockSite.Dock = System.Windows.Forms.DockStyle.Left;
			this.barLeftDockSite.Name = "barLeftDockSite";
			this.barLeftDockSite.Size = new System.Drawing.Size(0, 325);
			this.barLeftDockSite.TabIndex = 9;
			this.barLeftDockSite.TabStop = false;
			// 
			// barRightDockSite
			// 
			this.barRightDockSite.Dock = System.Windows.Forms.DockStyle.Right;
			this.barRightDockSite.Location = new System.Drawing.Point(536, 0);
			this.barRightDockSite.Name = "barRightDockSite";
			this.barRightDockSite.Size = new System.Drawing.Size(0, 325);
			this.barRightDockSite.TabIndex = 10;
			this.barRightDockSite.TabStop = false;
			// 
			// barTopDockSite
			// 
			this.barTopDockSite.Dock = System.Windows.Forms.DockStyle.Top;
			this.barTopDockSite.Name = "barTopDockSite";
			this.barTopDockSite.Size = new System.Drawing.Size(536, 0);
			this.barTopDockSite.TabIndex = 7;
			this.barTopDockSite.TabStop = false;
			// 
			// m_OpenFileDialog
			// 
			this.m_OpenFileDialog.Filter = "DotNetBar Files (*.dnb)|*.dnb|All Files (*.*)|*.*";
			this.m_OpenFileDialog.ShowReadOnly = true;
			this.m_OpenFileDialog.Title = "Open DotNetBar Definition File";
			// 
			// m_SaveFileDialog
			// 
			this.m_SaveFileDialog.CreatePrompt = false;
			this.m_SaveFileDialog.DefaultExt = "dnb";
			this.m_SaveFileDialog.FileName = "dotnetbardefinition";
			this.m_SaveFileDialog.Filter = "DotNetBar Files (*.dnb)|*.dnb|XML Files (*.xml)|*.xml|All Files (*.*)|*.*";
			this.m_SaveFileDialog.Title = "Save DotNetBar Definition";
			// 
			// btnClose
			// 
			this.btnClose.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.btnClose.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnClose.Location = new System.Drawing.Point(376, 291);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(73, 24);
			this.btnClose.TabIndex = 6;
			this.btnClose.Text = "OK";
			this.btnClose.Click += new System.EventHandler(this.CloseClick);
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnCancel.Location = new System.Drawing.Point(455, 291);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(73, 24);
			this.btnCancel.TabIndex = 7;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.Click += new System.EventHandler(this.CloseClick);
			// 
			// BarEditor
			// 
			//this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(536, 325);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.panel1,
																		  this.btnClose,
																		  this.btnCancel,
																		  this.barTopDockSite,
																		  this.barBottomDockSite,
																		  this.barLeftDockSite,
																		  this.barRightDockSite});
			this.MinimizeBox = false;
			this.Name = "BarEditor";
			this.Text = "DotNetBar Editor";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.FormClosing);
			this.Load += new EventHandler(this.FormLoad);
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormLoad(object sender, EventArgs e)
		{
			try
			{
				Microsoft.Win32.RegistryKey key=Microsoft.Win32.Registry.LocalMachine;
				string helpfile="";
				try
				{
					key=key.OpenSubKey("Software\\DevComponents\\DotNetBar");
					helpfile=key.GetValue("InstallationFolder","").ToString();
				}
				finally{key.Close();}

				if(helpfile.Substring(helpfile.Length-1,1)!="\\")
					helpfile+="\\";

				if(System.IO.File.Exists(helpfile+"dotnetbar.chm"))
					helpfile+="dotnetbar.chm";
				else
					helpfile="";

				if(helpfile!="")
					m_HtmlHelp=new HTMLHelp(this,helpfile);
			}
			catch(Exception)
			{
			}

			// Load position if any
			try
			{
				Microsoft.Win32.RegistryKey key=Microsoft.Win32.Registry.CurrentUser;
				key=key.OpenSubKey("Software\\DevComponents\\DotNetBar");
				if(key!=null)
				{
					try
					{
						string s=key.GetValue("DesignerPosition","").ToString();
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
									Rectangle r=new Rectangle(int.Parse(arr[0]),int.Parse(arr[1]),int.Parse(arr[2]),int.Parse(arr[3]));
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
						s=key.GetValue("DesignerPanelSize","").ToString();
						barTree.Width=int.Parse(s);
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
		
		#region Toolbar Definition Code
		private void CreateToolbar()
		{
			Bar bar, barToolbar;
			ButtonItem item, item2, item3, popupmain;
			ButtonItem popup=new ButtonItem("popup");
			barManager.Items.Add(popup);
			bar=new Bar("Main Menu");
			barManager.Bars.Add(bar);
			bar.MenuBar=true;
			bar.Stretch=true;

			barToolbar=new Bar("Item Navigation");
			barManager.Bars.Add(barToolbar);

			// File Menu
            item=new ButtonItem("file");
			item.Text="&File";
			item.PopupType=ePopupType.Menu;
			bar.Items.Add(item);
			item2=new ButtonItem("open");
			item2.Shortcuts.Add(eShortcut.CtrlO);
			item.SubItems.Add(item2);
			item2.Text="&Open definition...";
			item2.ImageIndex=4;
			barToolbar.Items.Add(item2.Copy());
			
			item2=new ButtonItem("save");
			item2.Shortcuts.Add(eShortcut.CtrlS);
			item.SubItems.Add(item2);
			item2.Text="&Save definition...";
			item2.ImageIndex=5;
			barToolbar.Items.Add(item2.Copy());
			item2.BeginGroup=true;

			item2=new ButtonItem("saveas");
			item2.Text="&Save definition as...";
			item.SubItems.Add(item2);
			
			item2=new ButtonItem("loadbar");
			item2.Shortcuts.Add(eShortcut.CtrlL);
			item.SubItems.Add(item2);
			item2.Text="Load Bar...";
			barManager.Items.Add(item2.Copy());
			item2.BeginGroup=true;
			item2.Enabled=true;

			item2=new ButtonItem("savebaras");
			item2.Shortcuts.Add(eShortcut.CtrlD);
			item.SubItems.Add(item2);
			item2.Text="Save Bar as...";
			barManager.Items.Add(item2.Copy());
			item2.Enabled=false;

			item2=new ButtonItem("close");
			item2.Text="&Close Designer";
			item2.BeginGroup=true;
			item.SubItems.Add(item2);

			// Tools
			item=new ButtonItem("tools");
			item.Text="&Tools";
			item.PopupType=ePopupType.Menu;
			bar.Items.Add(item);
			item2=new ButtonItem("barscreation","Create Bar");
			item.SubItems.Add(item2);
			popupmain=item2.Copy() as ButtonItem;
			popup.SubItems.Add(popupmain);
			if(m_Bar!=null)
				item2.Visible=false;
			else if(m_DotNetBar.LeftDockSite==null && m_DotNetBar.RightDockSite==null && m_DotNetBar.TopDockSite==null && m_DotNetBar.BottomDockSite==null)
				item2.Enabled=false;
			item3=new ButtonItem("createbar");
			item3.Text="&Toolbar";
			item2.SubItems.Add(item3);
			popupmain.SubItems.Add(item3.Copy());
			item3=new ButtonItem("createmenubar");
			item3.Text="&Menu Bar";
			item2.SubItems.Add(item3);
			popupmain.SubItems.Add(item3.Copy());
			item3=new ButtonItem("createstatusbar","&Status Bar");
			item2.SubItems.Add(item3);
			popupmain.SubItems.Add(item3.Copy());
			item3=new ButtonItem("createdockwindow","&Dockable Window");
			item2.SubItems.Add(item3);
			popupmain.SubItems.Add(item3.Copy());
			item3=new ButtonItem("createtaskpane","&Task Pane");
			item2.SubItems.Add(item3);
			popupmain.SubItems.Add(item3.Copy());

			item2=new ButtonItem("buttonitem");
			item2.Text="Add &ButtonItem";
			item2.BeginGroup=true;
			item.SubItems.Add(item2);
			popup.SubItems.Add(item2.Copy());
			item2=new ButtonItem("textboxitem");
			item2.Text="Add &TextBoxItem";
			item.SubItems.Add(item2);
			popup.SubItems.Add(item2.Copy());
			item2=new ButtonItem("comboboxitem");
			item2.Text="Add &ComboBoxItem";
			item.SubItems.Add(item2);
			popup.SubItems.Add(item2.Copy());
			item2=new ButtonItem("labelitem");
			item2.Text="Add &LabelItem";
			item.SubItems.Add(item2);
			popup.SubItems.Add(item2.Copy());
			// Progress Bar Item
			item2=new ButtonItem("progressbaritem");
			item2.Tooltip="Displays a progress bar.";
			item2.Text="Add &ProgressBarItem";
			item.SubItems.Add(item2);
			popup.SubItems.Add(item2.Copy());

			item2=new ButtonItem("customizeitem");
			item2.BeginGroup=true;
			item2.Tooltip="Lets end users hide/show Bar items as well as open the Customize dialog.";
			item2.Text="Add Custo&mizeItem";
			item.SubItems.Add(item2);
			popup.SubItems.Add(item2.Copy());
			item2=new ButtonItem("mdiwindowlistitem");
			item2.Tooltip="Displays list of MDI Child forms.";
			item2.Text="Add &MdiWindowListItem";
			item.SubItems.Add(item2);
			popup.SubItems.Add(item2.Copy());
			item2=new ButtonItem("controlcontaineritem");
			item2.Tooltip="Allows you to host any Control on Bar or Menu-bar.";
			item2.Text="Add &ControlContainerItem";
			item.SubItems.Add(item2);
			popup.SubItems.Add(item2.Copy());
			item2=new ButtonItem("dockcontaineritem");
			item2.Tooltip="Helps you create dockable windows.";
			item2.Text="Add &DockContainerItem";
			item.SubItems.Add(item2);
			popup.SubItems.Add(item2.Copy());

			// Pop-up specific items
			item2=new ButtonItem("copyto");
            item2.Text="Copy To";
            item2.BeginGroup=true;
			item2.PopupType=ePopupType.Menu;
			popup.SubItems.Add(item2);
			item2=new ButtonItem("moveto");
			item2.Text="Move To";
			item2.PopupType=ePopupType.Menu;
			popup.SubItems.Add(item2);

			item2=new ButtonItem("delselected");
			item.SubItems.Add(item2);
			item2.Text="&Delete Selected Item";
			item2.BeginGroup=true;
			item2.ImageIndex=7;
			popup.SubItems.Add(item2.Copy());

			// Help
			item=new ButtonItem("help");
			item.Text="&Help";
			item.PopupType=ePopupType.Menu;
			bar.Items.Add(item);
			item2=new ButtonItem("contents");
			item2.Text="&Contents...";
			item.SubItems.Add(item2);
//			item2=new ButtonItem("index");
//			item2.Text="&Index...";
//			item.SubItems.Add(item2);
//			item2=new ButtonItem("search");
//			item2.Text="&Search...";
//			item.SubItems.Add(item2);
			item2=new ButtonItem("about");
			item2.Text="&About";
			item2.BeginGroup=true;
			item.SubItems.Add(item2);

			bar.DockSide=eDockSide.Top;

			// Create left bar with the item navigation buttons
			item=new ButtonItem("addnewitems");
			barToolbar.Items.Add(item);
			item.PopupType=ePopupType.Menu;
			item.ImageIndex=6;
			item.Text="Create new item";
			item.Tooltip="Press to see available items that can be created";
			item.BeginGroup=true;
			item.Enabled=true;
			item.SubItems.Add(barManager.Items["buttonitem"].Copy());
			item.SubItems.Add(barManager.Items["textboxitem"].Copy());
			item.SubItems.Add(barManager.Items["comboboxitem"].Copy());
			item.SubItems.Add(barManager.Items["customizeitem"].Copy());
			item.SubItems.Add(barManager.Items["mdiwindowlistitem"].Copy());
            item.SubItems.Add(barManager.Items["controlcontaineritem"].Copy());
			item.SubItems.Add(barManager.Items["dockcontaineritem"].Copy());
			item.SubItems.Add(barManager.Items["progressbaritem"].Copy());
			barToolbar.Items.Add(barManager.Items["delselected"].Copy());


			item=new ButtonItem("moveleft");
			barToolbar.Items.Add(item);
			item.ImageIndex=0;
			item.Text="Move Left";
			item.Tooltip="Move selected item left";
			item.BeginGroup=true;
			item.Enabled=false;
			item.ClickAutoRepeat=true;
			barManager.Items.Add(item.Copy());
			
			item=new ButtonItem("moveright");
			barToolbar.Items.Add(item);
			item.ImageIndex=1;
			item.Text="Move Right";
			item.Tooltip="Move selected item right";
			item.Enabled=false;
			item.ClickAutoRepeat=true;
			barManager.Items.Add(item.Copy());
			
			item=new ButtonItem("moveup");
			barToolbar.Items.Add(item);
			item.ImageIndex=2;
			item.Text="Move Up";
			item.Tooltip="Move selected item up";
			item.Enabled=false;
			item.ClickAutoRepeat=true;
			barManager.Items.Add(item.Copy());
			
			item=new ButtonItem("movedown");
			barToolbar.Items.Add(item);
			item.ImageIndex=3;
			item.Text="Move Down";
			item.Tooltip="Move selected item down";
			item.Enabled=false;
			item.ClickAutoRepeat=true;
			barManager.Items.Add(item.Copy());

			item=new ButtonItem("synccat");
			barToolbar.Items.Add(item);
			item.ImageIndex=8;
			item.Text="Sync Categories";
			item.Tooltip="Recreates Categories from your current definition.";
			item.Enabled=true;
			item.ButtonStyle=eButtonStyle.ImageAndText;
			item.ClickAutoRepeat=false;
			
			barToolbar.DockLine=1;
			barToolbar.DockSide=eDockSide.Top;

			popup=new ButtonItem("resetimagepopup");
			item=new ButtonItem("resetimage","Reset");
			popup.SubItems.Add(item);
			barManager.ContextMenus.Add(popup);

			barManager.Style=eDotNetBarStyle.Office2003;

		}
		#endregion

		protected void CloseClick (object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void RefreshView()
		{
			barTree.Nodes.Clear();

			if(m_Bar!=null)
			{
				try
				{
					this.Cursor=Cursors.WaitCursor;
					m_BarsNode=barTree.Nodes.Add("Bar");
					m_BarsNode.ImageIndex=2;
					m_BarsNode.SelectedImageIndex=2;

					TreeNode barNode=m_BarsNode.Nodes.Add(m_Bar.Text);
					barNode.Tag=m_Bar;
					barNode.ImageIndex=1;
					barNode.SelectedImageIndex=1;
					foreach(BaseItem objItem in m_Bar.Items)
					{
						TreeNode itemNode=barNode.Nodes.Add(GetTreeItemText(objItem));
						itemNode.Tag=objItem;

						itemNode.ImageIndex=GetItemImageIndex(objItem);
						itemNode.SelectedImageIndex=itemNode.ImageIndex;

						AddSubItems(objItem,itemNode);
					}
					m_BarsNode.Expand();
					barNode.Expand();
				}
				finally
				{
					this.Cursor=Cursors.Arrow;
				}
				return;
			}

			if(m_DotNetBar==null)
				return;

			this.Cursor=Cursors.WaitCursor;

			if(m_DotNetBar.TopDockSite==null && m_DotNetBar.BottomDockSite==null && m_DotNetBar.LeftDockSite==null && m_DotNetBar.RightDockSite==null)
			{
				m_CategoriesNode=null;
				m_BarsNode=null;
			}
			else
			{
				m_CategoriesNode=barTree.Nodes.Add("Categories");
				m_CategoriesNode.ImageIndex=5;
				m_CategoriesNode.SelectedImageIndex=5;

				m_BarsNode=barTree.Nodes.Add("Bars");
				m_BarsNode.ImageIndex=2;
				m_BarsNode.SelectedImageIndex=2;
			}

			m_PopupsNode=barTree.Nodes.Add("Context Menus");
			m_PopupsNode.ImageIndex=12;
			m_PopupsNode.SelectedImageIndex=12;

			if(m_BarsNode!=null)
			{
				foreach(Bar bar in m_DotNetBar.Bars)
				{
					TreeNode barNode=m_BarsNode.Nodes.Add(bar.Text);
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
			}

			// Load nodes from items
			m_catTable.Clear();
			if(m_CategoriesNode!=null)
			{
				foreach(DictionaryEntry o in m_DotNetBar.Items)
				{
					BaseItem objItem=o.Value as BaseItem;
					TreeNode itemNode=CategorizeItem(objItem);
					itemNode.Tag=objItem;

					itemNode.ImageIndex=GetItemImageIndex(objItem);
					itemNode.SelectedImageIndex=itemNode.ImageIndex;

					AddSubItems(objItem,itemNode);
				}
			}

			// Load all popups
			foreach(BaseItem objItem in m_DotNetBar.ContextMenus)
			{
				TreeNode itemNode=m_PopupsNode.Nodes.Add(GetTreeItemText(objItem));
				itemNode.Tag=objItem;

				itemNode.ImageIndex=GetItemImageIndex(objItem);
				itemNode.SelectedImageIndex=itemNode.ImageIndex;

				AddSubItems(objItem,itemNode);
			}

			if(m_BarsNode!=null)
				m_BarsNode.Expand();
			if(m_CategoriesNode!=null)
				m_CategoriesNode.Expand();
			m_PopupsNode.Expand();

			this.Cursor=Cursors.Arrow;
		}
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
		// TODO: Make sure that when designing the Menu Bar when new item is added default
		// PopupType is MENU not TOOLBAR
		private void ItemSelected(object sender,TreeViewEventArgs e)
		{
			barTree.LabelEdit=false;
			if(e.Node.Tag==null)
			{
				propertyGrid1.SelectedObject=null;
				return;
			}

			if(e.Node.Tag is BaseItem)
			{
				propertyGrid1.SelectedObject=e.Node.Tag;
				barTree.LabelEdit=true;

				// Now way to detect when Items collection has changed, reset changed flag if combo box is selected...
				if(e.Node.Tag is ComboBoxItem)
					m_DataChanged=true;
			}
			else if(e.Node.Tag is Bar)
			{
				propertyGrid1.SelectedObject=(Bar)e.Node.Tag;

			}
			else
			{
				propertyGrid1.SelectedObject=null;
			}

		}

		private void BeforeItemSelect(object sender, System.Windows.Forms.TreeViewCancelEventArgs e)
		{
			// Disable/Enable toolbar items...
			bool bLeftEnabled=false, bRightEnabled=false, bUpEnabled=false, bDownEnabled=false;

			BaseItem popupItem=barManager.Items["popup"];

			if(e.Node!=null && e.Node.Tag is BaseItem)
			{
				if(e.Node.Parent==m_PopupsNode)
				{
					bRightEnabled=true;
				}
				else
				{
					BaseItem objItem=e.Node.Tag as BaseItem;
					int i=0;
					if(objItem.Parent!=null)
						i=objItem.Parent.SubItems.IndexOf(objItem);
					else
						i=e.Node.Index;
					if(i>0)
					{
						bUpEnabled=true;
						bRightEnabled=true;
					}
					int iCount=0;
					if(objItem.Parent!=null)
						iCount=objItem.Parent.SubItems.Count-1;
					else
						iCount=e.Node.Parent.Nodes.Count-1;

					if(i<iCount)
						bDownEnabled=true;

					if((objItem.Parent!=null && !objItem.Parent.SystemItem && (!(objItem.Parent is GenericItemContainer) || !((GenericItemContainer)objItem.Parent).SystemContainer)) || (e.Node.Parent!=null && e.Node.Parent.Tag is BaseItem))
						bLeftEnabled=true;
				}
			}

			barManager.Items["moveleft"].Enabled=bLeftEnabled;
			barManager.Items["moveright"].Enabled=bRightEnabled;
			barManager.Items["moveup"].Enabled=bUpEnabled;
			barManager.Items["movedown"].Enabled=bDownEnabled;

			if(e.Node!=null && e.Node.Tag is BaseItem)
			{
				popupItem.SubItems["copyto"].Visible=true;
				popupItem.SubItems["moveto"].Visible=true;
			}
			else
			{
				popupItem.SubItems["copyto"].Visible=false;
				popupItem.SubItems["moveto"].Visible=false;
			}

			if(e.Node==null || m_BarsNode!=null && e.Node==m_BarsNode)
			{
				barManager.Items["buttonitem"].Enabled=false;
				barManager.Items["textboxitem"].Enabled=false;
				barManager.Items["comboboxitem"].Enabled=false;
				barManager.Items["customizeitem"].Enabled=false;
				barManager.Items["labelitem"].Enabled=false;
				barManager.Items["mdiwindowlistitem"].Enabled=false;
				barManager.Items["controlcontaineritem"].Enabled=false;
				barManager.Items["dockcontaineritem"].Enabled=false;
				barManager.Items["progressbaritem"].Enabled=false;
				barManager.Items["delselected"].Enabled=false;
				return;
			}
			else if(m_CategoriesNode!=null && e.Node==m_CategoriesNode)
			{
				barManager.Items["buttonitem"].Enabled=true;
				barManager.Items["textboxitem"].Enabled=true;
				barManager.Items["comboboxitem"].Enabled=true;
				barManager.Items["customizeitem"].Enabled=true;
				barManager.Items["labelitem"].Enabled=true;
				barManager.Items["mdiwindowlistitem"].Enabled=true;
				barManager.Items["controlcontaineritem"].Enabled=true;
				barManager.Items["dockcontaineritem"].Enabled=true;
				barManager.Items["progressbaritem"].Enabled=true;
				barManager.Items["delselected"].Enabled=false;
				return;
			}

			if(e.Node!=null && e.Node.Tag is Bar)
				barManager.Items["savebaras"].Enabled=true;
			else
                barManager.Items["savebaras"].Enabled=false;

			barManager.Items["buttonitem"].Enabled=true;
			barManager.Items["textboxitem"].Enabled=true;
			barManager.Items["comboboxitem"].Enabled=true;
			barManager.Items["customizeitem"].Enabled=true;
			barManager.Items["labelitem"].Enabled=true;
			barManager.Items["mdiwindowlistitem"].Enabled=true;
			barManager.Items["controlcontaineritem"].Enabled=true;
			barManager.Items["dockcontaineritem"].Enabled=true;
			barManager.Items["progressbaritem"].Enabled=true;

			if(m_CategoriesNode!=null && e.Node.Parent==m_CategoriesNode || e.Node.Tag is BaseItem || e.Node.Tag is Bar)
				barManager.Items["delselected"].Enabled=true;
			else
				barManager.Items["delselected"].Enabled=false;
		}

		private void TreeMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if(e.Button!=MouseButtons.Right)
				return;
            
			TreeNode node=barTree.GetNodeAt(e.X,e.Y);
			if(node!=null)
                barTree.SelectedNode=node;

			// Popup menu
			ButtonItem popup=barManager.Items["popup"] as ButtonItem;
			
			// If MoveTo and CopyTo are visible add items to them
			if(popup.SubItems["copyto"].Visible)
			{
				BaseItem itemCopyTo=popup.SubItems["copyto"];
				BaseItem itemMoveTo=popup.SubItems["moveto"];
				BaseItem itemSel=barTree.SelectedNode.Tag as BaseItem;
				itemCopyTo.SubItems.Clear();
				itemMoveTo.SubItems.Clear();
				bool bBars=true,bCategories=false, bPopups=false;
				// Don't show categories for category items...
				node=barTree.SelectedNode;
				while(node.Parent!=null)
					node=node.Parent;
				if(m_PopupsNode!=null && node!=m_PopupsNode)
					bPopups=true;
				if(m_CategoriesNode!=null && node!=m_CategoriesNode)
					bCategories=true;

				if(bBars && m_BarsNode!=null)
				{
					foreach(TreeNode barNode in m_BarsNode.Nodes)
					{
						BaseItem newItem=new ButtonItem();
						newItem.Text=barNode.Text;
						newItem.Tag=barNode;
						newItem.Click+=new System.EventHandler(this.CopyMoveToClick);
						itemCopyTo.SubItems.Add(newItem);
						itemMoveTo.SubItems.Add(newItem.Copy());
					}
				}
				if(bCategories && m_CategoriesNode!=null)
				{
					ButtonItem cat=new ButtonItem("tocategories");
					cat.Text="Categories";
					cat.BeginGroup=true;
					cat.Click+=new System.EventHandler(this.CopyMoveToClick);
					itemCopyTo.SubItems.Add(cat);
					itemMoveTo.SubItems.Add(cat.Copy());

					foreach(TreeNode catNode in m_CategoriesNode.Nodes)
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
					BaseItem pi=new ButtonItem("topopups");
					pi.BeginGroup=true;
					pi.Text="Popups";
					pi.Click+=new System.EventHandler(this.CopyMoveToClick);
					itemCopyTo.SubItems.Add(pi);
					itemMoveTo.SubItems.Add(pi.Copy());

					foreach(BaseItem objItem in m_DotNetBar.ContextMenus)
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
			pt=barTree.PointToScreen(pt);
            popup.PopupMenu(pt);
		}

		private void NodeExpanding(object sender, System.Windows.Forms.TreeViewCancelEventArgs e)
		{
			if(e.Node.ImageIndex==5)
			{
				e.Node.ImageIndex=6;
				e.Node.SelectedImageIndex=6;
			}
		}

		private void NodeCollapsing(object sender, System.Windows.Forms.TreeViewCancelEventArgs e)
		{
			if(e.Node.ImageIndex==6)
			{
				e.Node.ImageIndex=5;
				e.Node.SelectedImageIndex=5;
			}
		}

		private void BarItemClick(object sender, EventArgs e)
		{
			BaseItem objItem=sender as BaseItem;
			BaseItem newItem=null;
			if(objItem.Name=="file" || objItem.Name=="tools" || objItem.Name=="help" || objItem.Name=="")
				return;

			if(objItem.Name=="about")
			{
				MessageBox.Show("DotNetBar by DevComponents.com (c) 2001-2003 by DevComponents.com All Rights Reserved.");
				return;
			}
			else if(objItem.Name=="close")
			{
				this.DialogResult=DialogResult.OK;
				this.Close();
				return;
			}
			else if(objItem.Name=="contents")
			{
				if(m_HtmlHelp!=null)
					m_HtmlHelp.ShowContents();
			}
			else if(objItem.Name=="index")
			{
				if(m_HtmlHelp!=null)
					m_HtmlHelp.ShowContents();
			}
			else if(objItem.Name=="search")
			{
				if(m_HtmlHelp!=null)
					m_HtmlHelp.ShowSearch();
			}
			else if(objItem.Name=="resetimage")
			{
				m_DataChanged=true;
				if(propertyGrid1.SelectedGridItem!=null && propertyGrid1.SelectedGridItem.PropertyDescriptor.PropertyType==typeof(System.Drawing.Image))
				{
					propertyGrid1.SelectedGridItem.PropertyDescriptor.SetValue(propertyGrid1.SelectedObject,null);
					propertyGrid1.Refresh();
				}
				return;
			}
			else if(objItem.Name=="createbar" || objItem.Name=="createmenubar" || objItem.Name=="createstatusbar" || objItem.Name=="createtaskpane")
			{
				m_DataChanged=true;
				Bar bar;

				if(objItem.Name=="createmenubar")
				{
					bar=CreateObject(typeof(Bar)) as Bar;
					bar.Text="Main Menu";
					bar.MenuBar=true;
					bar.Stretch=true;
					bar.Name="mainmenu";
				}
				else if(objItem.Name=="createstatusbar")
				{
					bar=CreateObject(typeof(Bar)) as Bar;
					bar.Text="Status";
					bar.Stretch=true;
					bar.Name="statusBar"+((int)m_DotNetBar.Bars.Count+1).ToString();
					bar.LayoutType=eLayoutType.Toolbar;
					bar.GrabHandleStyle=eGrabHandleStyle.ResizeHandle;
					bar.ItemSpacing=2;
				}
				else if(objItem.Name=="createtaskpane")
				{
					bar=CreateObject(typeof(Bar)) as Bar;
					bar.Text="Task Pane";
					bar.Stretch=true;
					bar.Name="taskbar"+((int)m_DotNetBar.Bars.Count+1).ToString();
					bar.LayoutType=eLayoutType.TaskList;
					bar.GrabHandleStyle=eGrabHandleStyle.Caption;
				}
				else
				{
					bar=CreateObject(typeof(Bar)) as Bar;
					bar.Text="My Bar";
					bar.Name="bar"+((int)m_DotNetBar.Bars.Count+1).ToString();
				}
                
				bar.SetDesignMode(true);

				TreeNode barNode=m_BarsNode.Nodes.Add(bar.Text);
				barNode.Tag=bar;
				barNode.ImageIndex=3;
				barNode.SelectedImageIndex=3;
				bar.DockLine=m_DotNetBar.Bars.Count;
				m_DotNetBar.Bars.Add(bar);
				if(objItem.Name=="createtaskpane")
					bar.DockSide=eDockSide.Right;
				else if(objItem.Name=="createstatusbar")
					bar.DockSide=eDockSide.Bottom;
				else
					bar.DockSide=eDockSide.Top;
				
				if(objItem.Name=="createstatusbar")
				{
					LabelItem status=CreateObject(typeof(LabelItem)) as LabelItem;
					status.Name="label"+status.Id.ToString();
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

					barTree.SelectedNode=barNode;
					barNode.EnsureVisible();
				}
				else
				{
					barTree.SelectedNode=barNode;
					barNode.EnsureVisible();
				}
				return;
			}
			else if(objItem.Name=="createdockwindow")
			{
				m_DataChanged=true;
				Bar bar=CreateObject(typeof(Bar)) as Bar;;
				bar.Text="Dockable Window";
				bar.Stretch=true;
				bar.LayoutType=eLayoutType.DockContainer;
				bar.GrabHandleStyle=eGrabHandleStyle.Caption;
				bar.Name="dockwindow"+m_DotNetBar.Bars.Count.ToString();
				bar.SetDesignMode(true);

				DockContainerItem dockItem=CreateObject(typeof(DockContainerItem)) as DockContainerItem;
				dockItem.Name="item_"+dockItem.Id.ToString();
				dockItem.Text="Dock Container";
				bar.Items.Add(dockItem);

				TreeNode barNode=m_BarsNode.Nodes.Add(bar.Text);
				barNode.Tag=bar;
				barNode.ImageIndex=3;
				barNode.SelectedImageIndex=3;
				bar.DockLine=m_DotNetBar.Bars.Count;
				m_DotNetBar.Bars.Add(bar);
				bar.DockSide=eDockSide.Left;

				TreeNode itemTreeNode=barNode.Nodes.Add(GetTreeItemText(dockItem));
				itemTreeNode.Tag=dockItem;
				itemTreeNode.ImageIndex=GetItemImageIndex(dockItem);
				itemTreeNode.SelectedImageIndex=itemTreeNode.ImageIndex;
				itemTreeNode.EnsureVisible();

				barTree.SelectedNode=barNode;
				barNode.EnsureVisible();
				return;
			}
			else if(objItem.Name=="delselected")
			{
				if(barTree.SelectedNode==null)
					return;

				m_DataChanged=true;

				if(barTree.SelectedNode.Nodes.Count>0)
					if(MessageBox.Show(this,"Are you sure you want to delete selected item?","DotNetBar Editor",MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.No)
						return;

				if(barTree.SelectedNode.Tag is BaseItem)
				{
					BaseItem item=barTree.SelectedNode.Tag as BaseItem;
					Bar cont=item.ContainerControl as Bar;

					TreeNode topParentNode=barTree.SelectedNode;
					while(topParentNode.Parent!=null)
						topParentNode=topParentNode.Parent;
					
					if(item.Parent!=null)
						item.Parent.SubItems.Remove(item);
					else if(topParentNode==m_CategoriesNode)
						m_DotNetBar.Items.Remove(item);
					else if(barTree.SelectedNode.Parent==m_PopupsNode)
						m_DotNetBar.ContextMenus.Remove(item);
					
					if(_barDesigner!=null)
						_barDesigner.DestroyComponent(item);
					else
						item.Dispose();

					barTree.SelectedNode.Tag=null;
					TreeNode parentNode=barTree.SelectedNode.Parent;
					barTree.Nodes.Remove(barTree.SelectedNode);
					// If it is last node under one of the categories remove parent too
					if(parentNode!=null && parentNode.Parent==m_CategoriesNode && parentNode.Nodes.Count==0)
						barTree.Nodes.Remove(parentNode);
					if(cont!=null)
						cont.RecalcLayout();
					
				}
				else if(barTree.SelectedNode.Tag is Bar)
				{
					Bar bar=barTree.SelectedNode.Tag as Bar;

					m_DotNetBar.Bars.Remove(bar);

					barTree.SelectedNode.Tag=null;
					barTree.Nodes.Remove(barTree.SelectedNode);
				}
				else if(m_CategoriesNode!=null && barTree.SelectedNode.Parent==m_CategoriesNode)
				{
					// Delete all items within this category
					foreach(TreeNode node in barTree.SelectedNode.Nodes)
					{
						objItem=node.Tag as BaseItem;
						if(objItem!=null)
						{
							m_DotNetBar.Items.Remove(objItem);
							objItem.Dispose();
						}
						node.Tag=null;
					}
					barTree.SelectedNode.Remove();					
				}

				return;
			}
			else if(objItem.Name=="open")
			{
				if(m_OpenFileDialog.ShowDialog()==DialogResult.OK && System.IO.File.Exists(m_OpenFileDialog.FileName))
				{
					m_DotNetBar.Bars.Owner.LoadDefinition(m_OpenFileDialog.FileName);
					m_DefinitionFileName=m_OpenFileDialog.FileName;
					RefreshView();
					m_DataChanged=true;
				}
				return;
			}
			else if(objItem.Name=="save" && m_DefinitionFileName!="")
			{
				m_DotNetBar.Bars.Owner.SaveDefinition(m_DefinitionFileName);
				return;
			}
			else if(objItem.Name=="saveas" || objItem.Name=="save" && m_DefinitionFileName=="")
			{
				if(m_SaveFileDialog.ShowDialog()==DialogResult.OK)
				{
					m_DotNetBar.Bars.Owner.SaveDefinition(m_SaveFileDialog.FileName);
					m_DefinitionFileName=m_SaveFileDialog.FileName;
				}
				return;
			}
			else if(objItem.Name=="savebaras")
			{
				// Save currently selected bar
				if(barTree.SelectedNode==null || !(barTree.SelectedNode.Tag is Bar))
					return;
				
				Bar bar=barTree.SelectedNode.Tag as Bar;
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

				return;
			}
			else if(objItem.Name=="loadbar")
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
					m_DotNetBar.SuspendLayout=true;
					m_DotNetBar.Bars.Add(bar);
					bar.LoadDefinition(m_OpenFileDialog.FileName);
					m_DotNetBar.SuspendLayout=false;
				}
				m_OpenFileDialog.DefaultExt=defaultExt;
				m_OpenFileDialog.Filter=filter;
			}
			else if(objItem.Name=="moveleft" || objItem.Name=="moveright" || objItem.Name=="moveup" || objItem.Name=="movedown")
				MoveSelectedItem(objItem.Name);
			else if(objItem.Name=="addnewitems")
				objItem.Expanded=true;
			else if(objItem.Name=="synccat")
			{
				RescanCategories();
				return;
			}

			m_DataChanged=true;

			// Creation of new items only below this point
			if(objItem.Name!="buttonitem" && objItem.Name!="textboxitem" && objItem.Name!="comboboxitem" && objItem.Name!="customizeitem" && objItem.Name!="labelitem" && objItem.Name!="mdiwindowlistitem" && objItem.Name!="controlcontaineritem" && objItem.Name!="dockcontaineritem" && objItem.Name!="progressbaritem")
				return;
			
			// Item creation buttons only below!!!
			if(objItem.Name=="buttonitem")
			{
				ButtonItem btn=CreateObject(typeof(ButtonItem)) as ButtonItem;
				btn.Text="New Button";
				newItem=btn;
			}
			else if(objItem.Name=="textboxitem")
			{
				TextBoxItem tb=CreateObject(typeof(TextBoxItem)) as TextBoxItem;
				tb.Text="Text Box";
				newItem=tb;
			}
			else if(objItem.Name=="comboboxitem")
			{
				ComboBoxItem cb=CreateObject(typeof(ComboBoxItem)) as ComboBoxItem;
				cb.Text="Combo Box";
				newItem=cb;
			}
			else if(objItem.Name=="customizeitem")
			{
				CustomizeItem cust=CreateObject(typeof(CustomizeItem)) as CustomizeItem;
				newItem=cust;
			}
			else if(objItem.Name=="labelitem")
			{
				LabelItem li=CreateObject(typeof(LabelItem)) as LabelItem;
				li.Text="Label";
				li.BorderType=eBorderType.SingleLine;
				newItem=li;
			}
			else if(objItem.Name=="mdiwindowlistitem")
			{
				MdiWindowListItem mdi=CreateObject(typeof(MdiWindowListItem)) as MdiWindowListItem;
				mdi.Text="MDI Window List";
				newItem=mdi;
			}
			else if(objItem.Name=="controlcontaineritem")
			{
				ControlContainerItem cci=CreateObject(typeof(ControlContainerItem)) as ControlContainerItem;
				newItem=cci;
			}
			else if(objItem.Name=="dockcontaineritem")
			{
				DockContainerItem dci=CreateObject(typeof(DockContainerItem)) as DockContainerItem;
				dci.Text="Dockable Window";
				newItem=dci;
			}
			else if(objItem.Name=="progressbaritem")
			{
				ProgressBarItem pb=CreateObject(typeof(ProgressBarItem)) as ProgressBarItem;
				pb.Text="Progress Bar";
				pb.SetDesignMode(true);
				pb.Style=m_DotNetBar.Style;
				pb.ResetBackgroundStyle();
				newItem=pb;
			}

			if(_barDesigner==null)
			{
				string name="item_";
				long id=newItem.Id;
				if(m_DotNetBar!=null)
				{
					while(m_DotNetBar.GetItem(name+id,true)!=null)
						id++;
				}
				else if(m_Bar!=null)
				{
					while(m_Bar.GetItem(name+id)!=null)
						id++;
				}
				newItem.Name=name+id;
			}

			if(barTree.SelectedNode.Tag is BaseItem)
			{
				newItem.Style=((BaseItem)barTree.SelectedNode.Tag).Style;
				if(barTree.SelectedNode.Tag is PopupItem && newItem is PopupItem)
					((PopupItem)newItem).PopupType=((PopupItem)barTree.SelectedNode.Tag).PopupType;
			}
			else if(barTree.SelectedNode.Tag is Bar)
			{
				newItem.Style=((Bar)barTree.SelectedNode.Tag).Style;
				if(newItem is PopupItem && ((Bar)barTree.SelectedNode.Tag).MenuBar)
					((PopupItem)newItem).PopupType=ePopupType.Menu;
			}

			// We need to determine is new item being added to the Categories
			TreeNode itemNode=barTree.SelectedNode;
			while(itemNode.Parent!=null)
				itemNode=itemNode.Parent;

			if(itemNode==m_CategoriesNode)
			{
				// Assign category to new item
				if(barTree.SelectedNode==m_CategoriesNode)
					newItem.Category="(Untitled)";
				else if(barTree.SelectedNode.Parent==m_CategoriesNode)
					newItem.Category=barTree.SelectedNode.Text;
				else
					newItem.Category=((BaseItem)barTree.SelectedNode.Tag).Category;

				m_DotNetBar.Items.Add(newItem);                
				itemNode=CategorizeItem(newItem);
			}
			else if(itemNode==m_PopupsNode)
			{
				//if(barTree.SelectedNode.Tag is BaseItem && barTree.SelectedNode.Parent.Tag is BaseItem)
				if(barTree.SelectedNode.Tag is BaseItem)
				{
					//itemNode=barTree.SelectedNode.Parent.Nodes.Add(GetTreeItemText(newItem));
					itemNode=barTree.SelectedNode.Nodes.Add(GetTreeItemText(newItem));

					//BaseItem objParent=((BaseItem)barTree.SelectedNode.Tag).Parent;
					BaseItem objParent=(BaseItem)barTree.SelectedNode.Tag;
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
					itemNode=m_PopupsNode.Nodes.Add(GetTreeItemText(newItem));
					m_DotNetBar.ContextMenus.Add(newItem);
				}
			}
			else
			{
				Control cont=null;
				if(barTree.SelectedNode.Tag is BaseItem)
				{
					//itemNode=barTree.SelectedNode.Parent.Nodes.Add(GetTreeItemText(newItem));
					itemNode=barTree.SelectedNode.Nodes.Add(GetTreeItemText(newItem));
					//BaseItem objParent=((BaseItem)barTree.SelectedNode.Tag).Parent;
					BaseItem objParent=(BaseItem)barTree.SelectedNode.Tag;
					int iPos=-1;
					// New Items are always added before any system items which are by default kept at the end
					if(objParent.SubItems.Count>0 && !newItem.SystemItem)
					{
						iPos=GetAppendPosition(objParent);
					}
					objParent.SubItems.Add(newItem,iPos);
					cont=newItem.ContainerControl as Control;
					if(cont==null)
						((BaseItem)barTree.SelectedNode.Tag).Refresh();

				}
				else if(barTree.SelectedNode.Tag is Bar)
				{
					itemNode=barTree.SelectedNode.Nodes.Add(GetTreeItemText(newItem));
					Bar bar=(Bar)barTree.SelectedNode.Tag;
					int iPos=-1;
					// New Items are always added before any system items which are by default kept at the end
					if(bar.Items.Count>0 && !newItem.SystemItem)
					{
						iPos=GetAppendPosition(bar.ItemsContainer);
					}
					bar.Items.Add(newItem,iPos);
					cont=bar;
				}
				if(cont!=null && cont is Bar)
					((Bar)cont).RecalcLayout();
			}

			itemNode.Tag=newItem;
			itemNode.ImageIndex=GetItemImageIndex(newItem);
			itemNode.SelectedImageIndex=itemNode.ImageIndex;

			itemNode.EnsureVisible();
			barTree.SelectedNode=itemNode;
			//itemNode.BeginEdit();
		}

		public void RescanCategories()
		{
			if(m_DotNetBar==null)
				return;
			if(m_DotNetBar.Bars.Count==0)
				return;
			m_DotNetBar.Items.Clear();
			foreach(Bar bar in m_DotNetBar.Bars)
			{
				foreach(BaseItem item in bar.Items)
					AutoCategorizeItem(item);
			}
			m_DataChanged=true;
			RefreshView();
		}

		private void AutoCategorizeItem(BaseItem item)
		{
			if(item.Category!="" && item.Name!="" && !m_DotNetBar.Items.Contains(item.Name))
				m_DotNetBar.Items.Add(item.Copy());
			foreach(BaseItem i in item.SubItems)
				AutoCategorizeItem(i);
		}

		private void MoveSelectedItem(string Direction)
		{
			if(barTree.SelectedNode==null)
				return;
			m_DataChanged=true;
			BaseItem objItem=barTree.SelectedNode.Tag as BaseItem;
			if(objItem==null)
				return;

			bool bCategoryItem=false;
			if(barTree.SelectedNode.Parent==m_CategoriesNode)
				bCategoryItem=true;
			
			bool bPopupsItem=false;
			if(barTree.SelectedNode.Parent==m_PopupsNode)
				bPopupsItem=true;


			TreeNode selNode=barTree.SelectedNode;
			TreeNode parentNode=selNode.Parent;

			BaseItem objParent=objItem.Parent;

			int i=0;
			if(objParent!=null)
				i=objParent.SubItems.IndexOf(objItem);
			else
				i=selNode.Index;

			if(Direction=="moveup" && i>0)
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
                parentNode.Nodes.Insert(i-1,selNode);
				barTree.SelectedNode=selNode;
				selNode.EnsureVisible();
			}
			else if(Direction=="moveright" && i>0)
			{
				BaseItem objNewParent=null;
				
				if(bCategoryItem)
					m_DotNetBar.Items.Remove(objItem);
				else if(bPopupsItem)
					m_DotNetBar.ContextMenus.Remove(objItem);

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
				barTree.SelectedNode=selNode;
				selNode.EnsureVisible();
			}
			else if(Direction=="movedown" && ((objParent!=null && i<objParent.SubItems.Count-1) || (objParent==null && selNode.Index<selNode.Parent.Nodes.Count-1)))
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
				parentNode.Nodes.Insert(i+1,selNode);
				barTree.SelectedNode=selNode;
				selNode.EnsureVisible();
			}
			if(Direction=="moveleft" && objParent!=null && (!(objParent is GenericItemContainer) || !((GenericItemContainer)objParent).SystemItem))
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
				
				if(parentNode.Parent==m_CategoriesNode)
					m_DotNetBar.Items.Add(objItem);
				else if(parentNode.Parent==m_PopupsNode)
					m_DotNetBar.ContextMenus.Add(objItem);

				selNode.Remove();
				parentNode.Parent.Nodes.Insert(parentNode.Index+1,selNode);
				barTree.SelectedNode=selNode;
				selNode.EnsureVisible();
			}
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

		private void OnPropertyValueChanged(object s, System.Windows.Forms.PropertyValueChangedEventArgs e)
		{
			m_DataChanged=true;
			// If user changes the category of the item that is in categories we need to
			// reflect that change and move item to the right category
			TreeNode node=barTree.SelectedNode;
			BaseItem item=node.Tag as BaseItem;

			if(e.ChangedItem.PropertyDescriptor.Name=="Category" && e.ChangedItem.Value!=e.OldValue)
			{
				if(node.Tag==null || !(node.Tag is BaseItem))
					return;
				while(node.Parent!=null)
					node=node.Parent;
				if(node!=m_CategoriesNode)
					return;
	            
				node=barTree.SelectedNode;
				BaseItem objItem=node.Tag as BaseItem;
				node.Remove();
				node=CategorizeItem(objItem);
				node.ImageIndex=GetItemImageIndex(objItem);
				node.SelectedImageIndex=node.ImageIndex;
				AddSubItems(objItem,node);
				node.EnsureVisible();
				barTree.SelectedNode=node;
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
					bar.RecalcLayout();
			}

			if(e.ChangedItem.PropertyDescriptor.Name=="Name" && m_DotNetBar!=null)
			{
				TreeNode parent=node;
				while(parent.Parent!=null)
					parent=parent.Parent;
				if(parent==m_CategoriesNode)
				{
					if(m_DotNetBar.Items.Contains(e.ChangedItem.Value.ToString()))
					{
						item.Name=e.OldValue.ToString();
						propertyGrid1.Refresh();
						MessageBox.Show("Item with that name already exists.");
					}
					else
					{
						m_DotNetBar.Items.Remove(e.OldValue.ToString());
						m_DotNetBar.Items.Add(item);						
					}
				}
			}

			if(item!=null)
			{
				Bar barContainer=item.ContainerControl as Bar;
				if(barContainer!=null)
					barContainer.RecalcLayout();
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
				parentNode=m_CategoriesNode.Nodes.Add(newItem.Category);
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

		private void CopyMoveToClick(object sender, System.EventArgs e)
		{
			BaseItem item=sender as BaseItem;
			BaseItem itemSel=barTree.SelectedNode.Tag as BaseItem;
			BaseItem itemOriginal=barTree.SelectedNode.Tag as BaseItem;

			bool bMove=(item.Parent.Name=="moveto");
			if(!bMove)
			{
				itemSel=itemSel.Copy();
				if((Control.ModifierKeys & Keys.Control)==Keys.Control)
					itemSel.Name=itemOriginal.Name;
				else
					itemSel.Name="item_"+itemSel.Id;
			}

			if((item.Tag is string && (string)item.Tag=="cat") || item.Name=="tocategories")
			{
				// To Categories
				if(item.Name!="tocategories")
					itemSel.Category=item.Text;
				if(bMove)
				{
					if(itemSel.Parent!=null)
						itemSel.Parent.SubItems.Remove(itemSel);
					barTree.SelectedNode.Remove();
				}
				if(m_DotNetBar.Items.Contains(itemSel.Name))
				{
					string sDupName=itemSel.Name;
					itemSel.Name="item_"+itemSel.Id;
					MessageBox.Show("Item with name: '"+sDupName+"' already exists. Item that you are trying to move will be renamed.");
				}
				m_DotNetBar.Items.Add(itemSel);
				barTree.SelectedNode=CategorizeItem(itemSel);
				barTree.SelectedNode.EnsureVisible();
			}
			else if(item.Name=="topopups" || item.Tag is BaseItem)
			{
				if(bMove)
				{
					if(itemSel.Parent!=null)
						itemSel.Parent.SubItems.Remove(itemSel);
					barTree.SelectedNode.Remove();
				}

				if(item.Name=="topopups")
				{
					TreeNode itemNode=m_PopupsNode.Nodes.Add(GetTreeItemText(itemSel));
					itemNode.Tag=itemSel;
					itemNode.ImageIndex=GetItemImageIndex(itemSel);
					itemNode.SelectedImageIndex=itemNode.ImageIndex;
					AddSubItems(itemSel,itemNode);
					m_DotNetBar.ContextMenus.Add(itemSel);
					itemNode.EnsureVisible();
				}
				else
				{
                    BaseItem objParent=item.Tag as BaseItem;
					objParent.SubItems.Add(itemSel);
					foreach(TreeNode node in m_PopupsNode.Nodes)
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
					barTree.SelectedNode.Remove();
				}

				bar.Items.Add(itemSel,GetAppendPosition(bar.ItemsContainer));
				TreeNode itemNode=barNode.Nodes.Add(GetTreeItemText(itemSel));
				itemNode.Tag=itemSel;
				itemNode.ImageIndex=GetItemImageIndex(itemSel);
				itemNode.SelectedImageIndex=itemNode.ImageIndex;
				AddSubItems(itemSel,itemNode);
				barTree.SelectedNode=itemNode;
				barTree.SelectedNode.EnsureVisible();
				bar.RecalcLayout();
			}
            
			// Clear Copy To and Move To
			item=barManager.Items["popup"];
			item.SubItems["copyto"].SubItems.Clear();
			item.SubItems["moveto"].SubItems.Clear();
		}

		private void ItemEdited(object sender, System.Windows.Forms.NodeLabelEditEventArgs e)
		{
			if(e.CancelEdit)
				return;

			BaseItem objItem=e.Node.Tag as BaseItem;
			if(m_ShowItemText)
				objItem.Text=e.Label;
			else
				objItem.Name=e.Label;
			
			propertyGrid1.Refresh();
			m_DataChanged=true;
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

		private void TreeKeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if(e.KeyCode==Keys.F2 && barTree.SelectedNode!=null && barTree.LabelEdit)
				barTree.SelectedNode.BeginEdit();
		}

		private void GridViewMouseDown(object sender, EventArgs e)
		{
			if(propertyGrid1.SelectedGridItem==null)
				return;
			if(!(propertyGrid1.SelectedGridItem.PropertyDescriptor.PropertyType==typeof(System.Drawing.Image)))
				return;
            PopupItem popup=barManager.ContextMenus["resetimagepopup"] as PopupItem;
            popup.PopupMenu(Control.MousePosition);		
		}

		private void FormClosing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			m_GridViewSubclass.ReleaseHandle();
			m_GridViewSubclass=null;

			if(m_HtmlHelp!=null)
				m_HtmlHelp.CloseHelpWindow();

			// ColorScheme property when changed is not detected, so try to fix it here...
			if(m_DotNetBar!=null)
			{
				foreach(Bar bar in m_DotNetBar.Bars)
				{
					if(bar.ColorScheme._DesignTimeSchemeChanged)
					{
						m_DataChanged=true;
						bar.ColorScheme._DesignTimeSchemeChanged=false;
					}
				}
			}
			else if(m_Bar!=null)
			{
				if(m_Bar.ColorScheme._DesignTimeSchemeChanged)
				{
					m_DataChanged=true;
					m_Bar.ColorScheme._DesignTimeSchemeChanged=false;
				}
			}


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
					key.SetValue("DesignerPosition",s);
					// Save Panel size
					key.SetValue("DesignerPanelSize",barTree.Width);
					key.Close();
				}
				catch(Exception)
				{
				}
			}
		}

		private class GridViewSubclass:NativeWindow
		{
			public event EventHandler OnRightMouseDown;
			protected override void WndProc(ref Message m)
			{
				if(m.Msg==NativeFunctions.WM_CONTEXTMENU)
				{
					if(OnRightMouseDown!=null)
						OnRightMouseDown(this,new EventArgs());
					return;
				}
				base.WndProc(ref m);
			}
		}

		public bool DataChanged
		{
			get { return m_DataChanged;}
			set { m_DataChanged=value;}
		}

		public DotNetBarManager FormBarManager
		{
			get
			{
				return barManager;
			}
		}

		private void LoadResourceImages()
		{
			Image img=null;
			try
			{
				m_BarImages.Images.Clear();
				img=new Bitmap(typeof(DevComponents.DotNetBar.DotNetBarManager),"BarEditorImages.MoveItemLeft.bmp");
				m_BarImages.Images.Add(img,Color.Magenta);
				img=new Bitmap(typeof(DevComponents.DotNetBar.DotNetBarManager),"BarEditorImages.MoveItemRight.bmp");
				m_BarImages.Images.Add(img,Color.Magenta);
				img=new Bitmap(typeof(DevComponents.DotNetBar.DotNetBarManager),"BarEditorImages.MoveItemUp.bmp");
				m_BarImages.Images.Add(img,Color.Magenta);
				img=new Bitmap(typeof(DevComponents.DotNetBar.DotNetBarManager),"BarEditorImages.MoveItemDown.bmp");
				m_BarImages.Images.Add(img,Color.Magenta);
				img=new Bitmap(typeof(DevComponents.DotNetBar.DotNetBarManager),"BarEditorImages.FileOpen.bmp");
				m_BarImages.Images.Add(img,Color.Magenta);
				img=new Bitmap(typeof(DevComponents.DotNetBar.DotNetBarManager),"BarEditorImages.FileSave.bmp");
				m_BarImages.Images.Add(img,Color.Magenta);
				img=new Bitmap(typeof(DevComponents.DotNetBar.DotNetBarManager),"BarEditorImages.CreateItem.bmp");
				m_BarImages.Images.Add(img,Color.Magenta);
				img=new Bitmap(typeof(DevComponents.DotNetBar.DotNetBarManager),"BarEditorImages.DeleteItem.bmp");
				m_BarImages.Images.Add(img,Color.Magenta);
				img=new Bitmap(typeof(DevComponents.DotNetBar.DotNetBarManager),"BarEditorImages.RescanCategories.bmp");
				m_BarImages.Images.Add(img,Color.Magenta);

				imageList.Images.Clear();
				img=new Bitmap(typeof(DevComponents.DotNetBar.DotNetBarManager),"BarEditorImages.Item.bmp");
				imageList.Images.Add(img,Color.Magenta);
				img=new Bitmap(typeof(DevComponents.DotNetBar.DotNetBarManager),"BarEditorImages.Toolbar.bmp");
				imageList.Images.Add(img,Color.Magenta);
				img=new Bitmap(typeof(DevComponents.DotNetBar.DotNetBarManager),"BarEditorImages.Toolbars.bmp");
				imageList.Images.Add(img,Color.Magenta);
				img=new Bitmap(typeof(DevComponents.DotNetBar.DotNetBarManager),"BarEditorImages.Toolbar.bmp");
				imageList.Images.Add(img,Color.Magenta);
				img=new Bitmap(typeof(DevComponents.DotNetBar.DotNetBarManager),"BarEditorImages.Item.bmp");
				imageList.Images.Add(img,Color.Magenta);
				img=new Bitmap(typeof(DevComponents.DotNetBar.DotNetBarManager),"BarEditorImages.Folder.bmp");
				imageList.Images.Add(img,Color.Magenta);
				img=new Bitmap(typeof(DevComponents.DotNetBar.DotNetBarManager),"BarEditorImages.FolderOpen.bmp");
				imageList.Images.Add(img,Color.Magenta);
				img=new Bitmap(typeof(DevComponents.DotNetBar.DotNetBarManager),"BarEditorImages.ButtonItem.bmp");
				imageList.Images.Add(img,Color.Magenta);
				img=new Bitmap(typeof(DevComponents.DotNetBar.DotNetBarManager),"BarEditorImages.ComboItem.bmp");
				imageList.Images.Add(img,Color.Magenta);
				img=new Bitmap(typeof(DevComponents.DotNetBar.DotNetBarManager),"BarEditorImages.TextBoxItem.bmp");
				imageList.Images.Add(img,Color.Magenta);
				img=new Bitmap(typeof(DevComponents.DotNetBar.DotNetBarManager),"BarEditorImages.CustomizeItem.bmp");
				imageList.Images.Add(img,Color.Magenta);
				img=new Bitmap(typeof(DevComponents.DotNetBar.DotNetBarManager),"BarEditorImages.LableItem.bmp");
				imageList.Images.Add(img,Color.Magenta);
				img=new Bitmap(typeof(DevComponents.DotNetBar.DotNetBarManager),"BarEditorImages.PopupMenu.bmp");
				imageList.Images.Add(img,Color.Magenta);
                				
			}
			catch(Exception e)
			{
				MessageBox.Show("Could not load resource images. Exception was thrown: "+e.ToString());
			}
		}

		private object CreateObject(Type type)
		{
			if(_barDesigner!=null)
				return _barDesigner.CreateComponent(type);
			else
				return type.Assembly.CreateInstance(type.FullName);
		}
	}
}
