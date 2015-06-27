using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace DevComponents.Tree
{
	/// <summary>
	/// Represents controls that provides user interface to select root node from TreeGX control.
	/// </summary>
	[ToolboxItem(false)]
	public class TreeGXRootSelector : System.Windows.Forms.UserControl
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public TreeGXRootSelector()
		{
			InitializeComponent();
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
			// 
			// TreeGXRootSelector
			// 
			this.Name = "TreeGXRootSelector";
			this.Size = new System.Drawing.Size(400, 256);

		}
		#endregion
	}
}
