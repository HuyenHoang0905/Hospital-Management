using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Design
{
	/// <summary>
	/// Summary description for DefinitionPreviewControl.
	/// </summary>
	[ToolboxItem(false)]
	internal class DefinitionPreviewControl : System.Windows.Forms.UserControl
	{
		private DevComponents.DotNetBar.DockSite barLeftDockSite;
		private DevComponents.DotNetBar.DockSite barRightDockSite;
		private DevComponents.DotNetBar.DockSite barTopDockSite;
		private DevComponents.DotNetBar.DockSite barBottomDockSite;
		internal DevComponents.DotNetBar.DotNetBarManager previewManager;
		private System.ComponentModel.IContainer components;

		public event EventHandler DataChanged;

		public DefinitionPreviewControl()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			previewManager.DefinitionLoaded+=new EventHandler(this.DefinitionLoaded);

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

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			#if !TRIAL
			this.previewManager = new DevComponents.DotNetBar.DotNetBarManager(this.components,true);
			#else
			this.previewManager = new DevComponents.DotNetBar.DotNetBarManager(this.components);
			#endif
			this.barBottomDockSite = new DevComponents.DotNetBar.DockSite();
			this.barLeftDockSite = new DevComponents.DotNetBar.DockSite();
			this.barRightDockSite = new DevComponents.DotNetBar.DockSite();
			this.barTopDockSite = new DevComponents.DotNetBar.DockSite();
			this.SuspendLayout();
			// 
			// previewManager
			// 
			this.previewManager.AutoDispatchShortcuts.Add(DevComponents.DotNetBar.eShortcut.F1);
			this.previewManager.AutoDispatchShortcuts.Add(DevComponents.DotNetBar.eShortcut.CtrlC);
			this.previewManager.AutoDispatchShortcuts.Add(DevComponents.DotNetBar.eShortcut.CtrlA);
			this.previewManager.AutoDispatchShortcuts.Add(DevComponents.DotNetBar.eShortcut.CtrlV);
			this.previewManager.AutoDispatchShortcuts.Add(DevComponents.DotNetBar.eShortcut.CtrlX);
			this.previewManager.AutoDispatchShortcuts.Add(DevComponents.DotNetBar.eShortcut.CtrlZ);
			this.previewManager.AutoDispatchShortcuts.Add(DevComponents.DotNetBar.eShortcut.Del);
			this.previewManager.AutoDispatchShortcuts.Add(DevComponents.DotNetBar.eShortcut.Ins);
			this.previewManager.BottomDockSite = this.barBottomDockSite;
			this.previewManager.DefinitionName = "";
			this.previewManager.LeftDockSite = this.barLeftDockSite;
			this.previewManager.ParentForm = null;
			this.previewManager.RightDockSite = this.barRightDockSite;
			this.previewManager.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2003;
			this.previewManager.TopDockSite = this.barTopDockSite;
			this.previewManager.UseCustomCustomizeDialog = true;
			this.previewManager.UseHook = true;
			this.previewManager.BarClosing += new DevComponents.DotNetBar.DotNetBarManager.BarClosingEventHandler(this.OnBarClosing);
			this.previewManager.BarUndock += new System.EventHandler(this.OnDefinitionChanged);
			this.previewManager.BarTearOff += new System.EventHandler(this.OnDefinitionChanged);
			this.previewManager.BarDock += new System.EventHandler(this.OnDefinitionChanged);
			this.previewManager.AutoHideChanged += new System.EventHandler(this.OnDefinitionChanged);
			this.previewManager.EnterCustomize += new System.EventHandler(this.previewManager_EnterCustomize);
			// 
			// barBottomDockSite
			// 
			this.barBottomDockSite.AccessibleRole = System.Windows.Forms.AccessibleRole.Window;
			this.barBottomDockSite.BackgroundImageAlpha = ((System.Byte)(255));
			this.barBottomDockSite.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.barBottomDockSite.Location = new System.Drawing.Point(0, 176);
			this.barBottomDockSite.Name = "barBottomDockSite";
			this.barBottomDockSite.Size = new System.Drawing.Size(240, 0);
			this.barBottomDockSite.TabIndex = 3;
			this.barBottomDockSite.TabStop = false;
			// 
			// barLeftDockSite
			// 
			this.barLeftDockSite.AccessibleRole = System.Windows.Forms.AccessibleRole.Window;
			this.barLeftDockSite.BackgroundImageAlpha = ((System.Byte)(255));
			this.barLeftDockSite.Dock = System.Windows.Forms.DockStyle.Left;
			this.barLeftDockSite.Location = new System.Drawing.Point(0, 0);
			this.barLeftDockSite.Name = "barLeftDockSite";
			this.barLeftDockSite.Size = new System.Drawing.Size(0, 176);
			this.barLeftDockSite.TabIndex = 0;
			this.barLeftDockSite.TabStop = false;
			// 
			// barRightDockSite
			// 
			this.barRightDockSite.AccessibleRole = System.Windows.Forms.AccessibleRole.Window;
			this.barRightDockSite.BackgroundImageAlpha = ((System.Byte)(255));
			this.barRightDockSite.Dock = System.Windows.Forms.DockStyle.Right;
			this.barRightDockSite.Location = new System.Drawing.Point(240, 0);
			this.barRightDockSite.Name = "barRightDockSite";
			this.barRightDockSite.Size = new System.Drawing.Size(0, 176);
			this.barRightDockSite.TabIndex = 1;
			this.barRightDockSite.TabStop = false;
			// 
			// barTopDockSite
			// 
			this.barTopDockSite.AccessibleRole = System.Windows.Forms.AccessibleRole.Window;
			this.barTopDockSite.BackgroundImageAlpha = ((System.Byte)(255));
			this.barTopDockSite.Dock = System.Windows.Forms.DockStyle.Top;
			this.barTopDockSite.Location = new System.Drawing.Point(0, 0);
			this.barTopDockSite.Name = "barTopDockSite";
			this.barTopDockSite.Size = new System.Drawing.Size(240, 0);
			this.barTopDockSite.TabIndex = 2;
			this.barTopDockSite.TabStop = false;
			// 
			// DefinitionPreviewControl
			// 
			this.BackColor = System.Drawing.SystemColors.Control;
			this.Controls.Add(this.barLeftDockSite);
			this.Controls.Add(this.barRightDockSite);
			this.Controls.Add(this.barTopDockSite);
			this.Controls.Add(this.barBottomDockSite);
			this.Name = "DefinitionPreviewControl";
			this.Size = new System.Drawing.Size(240, 176);
			this.ResumeLayout(false);

		}
		#endregion

		#region Event Handlers
		private void DefinitionLoaded(object sender, EventArgs e)
		{
			foreach(Bar bar in previewManager.Bars)
			{
				bar.SizeChanged+=new EventHandler(this.BarSizeChanged);
				bar.Disposed+=new EventHandler(this.BarDisposed);
			}
		}
		private void BarDisposed(object sender, EventArgs e)
		{
			try
			{
				if(sender is Bar)
					((Bar)sender).SizeChanged-=new EventHandler(this.BarSizeChanged);
			}
			catch{}
		}
		private void BarSizeChanged(object sender, EventArgs e)
		{
			this.InvokeDataChanged();
		}
		private void OnBarClosing(object sender, BarClosingEventArgs e)
		{
			InvokeDataChanged();
		}
		private void OnDefinitionChanged(object sender, EventArgs e)
		{
			InvokeDataChanged();
		}
		#endregion

		private void InvokeDataChanged()
		{
			if(DataChanged!=null)
				DataChanged(this,new EventArgs());
		}

		private void previewManager_EnterCustomize(object sender, System.EventArgs e)
		{
		
		}
	}
}
