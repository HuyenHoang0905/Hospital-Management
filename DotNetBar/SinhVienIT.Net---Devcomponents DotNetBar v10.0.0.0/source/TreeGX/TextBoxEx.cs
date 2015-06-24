using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;

namespace DevComponents.Tree
{
	/// <summary>
	/// Represents the text box for editing cell's text.
	/// </summary>
	[Designer(typeof(System.Windows.Forms.Design.ControlDesigner)),ToolboxItem(false)]
	internal class TextBoxEx:TextBox
	{
		#region Private variables
		private bool m_WordWrap=false;
		#endregion

		#region Events
		public event EventHandler EndEdit;
		public event EventHandler CancelEdit;
		#endregion

		#region Constructor
		public TextBoxEx():base()
		{
			this.AutoSize=false;
			this.BorderStyle=System.Windows.Forms.BorderStyle.None;
		}
		#endregion

		#region Internal Implementation
		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);

			if(e.KeyCode==Keys.Enter && !m_WordWrap || e.KeyCode==Keys.Enter && e.Modifiers==Keys.Control)
			{
				if(EndEdit!=null)
					EndEdit(this, new EventArgs());
			}
			else if(e.KeyCode==Keys.Escape)
			{
				if(CancelEdit!=null)
					CancelEdit(this,new EventArgs());
			}
		}

		protected override void OnTextChanged(EventArgs e)
		{
			base.OnTextChanged (e);
			ResizeControl();
		}

		private void ResizeControl()
		{
			Graphics g=this.CreateGraphics();
			SizeF size=g.MeasureString(this.Text,this.Font);
			int width=(int)Math.Ceiling(size.Width);
			int height=(int)Math.Ceiling(size.Height);
			if(this.Parent!=null && this.Right+(width-this.Width)>this.Parent.Right)
				return;
			if(width>this.Width)
				this.Width=width;

			if(m_WordWrap)
			{
				if(this.Parent!=null && this.Bottom+(height-this.Height)>this.Parent.Bottom)
					return;
				if(height>this.Height)
					this.Height=height;
			}
		}
		#endregion

		#region Public Properties

		/// <summary>
		/// Gets or sets whether the editing is in word-wrap mode.
		/// </summary>
		public bool EditWordWrap
		{
			get {return m_WordWrap;}
			set
			{
				m_WordWrap=value;
				if(m_WordWrap)
				{
					this.Multiline=true;
					this.ScrollBars=ScrollBars.None;
				}
				else
				{
					this.Multiline=false;
				}
			}
		}
		#endregion
	}
}
