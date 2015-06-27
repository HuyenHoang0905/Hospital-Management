using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;

namespace DevComponents.AdvTree
{
	/// <summary>
	/// Represents the text box for editing cell's text.
	/// </summary>
	[Designer("System.Windows.Forms.Design.ControlDesigner, System.Design, Version=9.5.0.16, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"),ToolboxItem(false)]
	internal class TextBoxEx:TextBox, ICellEditControl
	{
		#region Private variables
		private bool m_WordWrap=false;
		#endregion

		#region Events
		public event EventHandler EditComplete;
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
				if(EditComplete!=null)
					EditComplete(this, new EventArgs());
			}
			else if(e.KeyCode==Keys.Escape)
			{
				if(CancelEdit!=null)
					CancelEdit(this,new EventArgs());
			}
		}
#if FRAMEWORK20
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if (!m_WordWrap && _PreventEnterBeep && e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
            }
            base.OnKeyPress(e);
        }
#endif

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Tab)
            {
                if (EditComplete != null)
                    EditComplete(this, new EventArgs());
            }
            return base.ProcessCmdKey(ref msg, keyData);
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
            if (this.Parent != null && this.Right + (width - this.Width) > (this.Parent.ClientRectangle.Right - SystemInformation.VerticalScrollBarWidth - 2))
                return;
            if (width > this.Width)
                this.Width = width;

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

        private bool _PreventEnterBeep = false;
        /// <summary>
        /// Gets or sets whether control prevents Beep sound when Enter key is pressed.
        /// </summary>
        [DefaultValue(false), Category("Behavior"), Description("Gets or sets whether control prevents Beep sound when Enter key is pressed.")]
        public bool PreventEnterBeep
        {
            get { return _PreventEnterBeep; }
            set
            {
                _PreventEnterBeep = value;
            }
        }
        
		#endregion

        #region ICellEditControl Members

        public void BeginEdit()
        {
            
        }

        public void EndEdit()
        {
            
        }

        public object CurrentValue
        {
            get
            {
                return this.Text;
            }
            set
            {
                this.Text = value.ToString();
            }
        }
        #endregion
    }
}
