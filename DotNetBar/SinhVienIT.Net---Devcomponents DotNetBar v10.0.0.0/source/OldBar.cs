namespace DevComponents.DotNetBar
{
    using System;

    /// <summary>
    ///    Summary description for bar.
    /// </summary>
    public sealed class Bar
    {
		/*private string m_Caption;
		private bool m_MoreButtonsVisible;
		private int m_Width;
		private int m_Height;
		private int m_DockedWidth;
		private int m_DockedHeight;
		private int m_DockSide;
		private int m_Flags;
        private int m_Items;
		private bool m_WrapTools;
		private bool m_Visible;*/

		private Toolbar m_Toolbar;
		private string m_Name;
        public Bar(string sName)
        {
			m_Toolbar=new Toolbar();
			m_Toolbar.ToolBarState=eBarState.Floating;
			m_Name=sName;
        }

		public string Caption
		{
			get
			{
				return m_Toolbar.Caption;
			}
			set
			{
				m_Toolbar.Caption=value;
			}
		}

		public eBarState BarState
		{
			get
			{
				return m_Toolbar.ToolBarState;
			}
			set
			{
				m_Toolbar.ToolBarState=value;
			}
		}

		public GenericItemContainer BarItems
		{
			get
			{
				return m_Toolbar.Items as GenericItemContainer;
			}
		}

		public int Width
		{
			get
			{
				return m_Toolbar.Width;
			}
		}

		public int Height
		{
			get
			{
				return m_Toolbar.Height;
			}
		}

		public bool Visible
		{
			get
			{
				return m_Toolbar.Visible;
			}
			set
			{
				m_Toolbar.Visible=value;
			}
		}

		public int DockLine
		{
			get
			{
				return m_Toolbar.DockLine;
			}
		}

		public int DockOffset
		{
			get
			{
				return m_Toolbar.DockOffset;
			}
		}

		public bool WrapItems
		{
			get
			{
				return m_Toolbar.WrapItems;
			}
			set
			{
				m_Toolbar.WrapItemsDock=value;
			}
		}

		public string Name
		{
			get
			{
				return m_Name;
			}
		}
    }
}