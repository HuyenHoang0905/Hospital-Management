using System;
using System.ComponentModel;

namespace DevComponents.AdvTree
{
	/// <summary>
	/// Represents the width of the Column. Supports absolute width in Pixels and
	/// relative width as percentage of the width of parent control.
	/// </summary>
	[ToolboxItem(false) ,System.ComponentModel.DesignTimeVisible(false),TypeConverter(typeof(System.ComponentModel.ExpandableObjectConverter))]
    public class ColumnWidth
	{
		private int m_Relative=0;
		private int m_Absolute=0;
		internal event EventHandler WidthChanged;

		/// <summary>
		/// Creates new instance of the object.
		/// </summary>
		public ColumnWidth()
		{
		}

		/// <summary>
		/// Gets or sets relative width expressed as percentage between 1-100. 0 indicates that
		/// absolute width will be used.
		/// </summary>
		/// <remarks>
		/// Relative width is expressed as percentage between 1-100 of the parent controls
		/// width. 0 indicates that absolute width will be used. Absolute width always takes
		/// priority over relative width. For example value of 30 assigned to this property
		/// indicates that width of the column will be 30% of the total client width of the
		/// control.
		/// </remarks>
		[DefaultValue(0),Browsable(true),Description("Gets or sets relative width in percent. Valid values are between 1-100 with 0 indicating that absolute width will be used.")]
		public int Relative
		{
			get {return m_Relative;}
			set
			{
				if(m_Relative!=value)
				{
					m_Relative=value;
					OnSizeChanged();
				}
			}
		}

		/// <summary>Gets or sets the absolute width of the column in pixels.</summary>
		/// <remarks>
		/// Absolute width always takes precedence over the relative width of the
		/// column.
		/// </remarks>
		[DefaultValue(0),Browsable(true),Description("Gets or sets the absolute width of the column in pixels.")]
		public int Absolute
		{
			get {return m_Absolute;}
			set
			{
				if(m_Absolute<0)
					return;
				if(m_Absolute!=value)
				{
					m_Absolute=value;
					if(m_Absolute!=0)
						m_Relative=0;
					OnSizeChanged();
				}
			}
		}

        internal void SetAbsolute(int value)
        {
            m_Absolute = value;
        }

        internal int GetWidth(int containerWidth)
        {
            if (m_Absolute > 0)
                return m_Absolute;
            if(m_Relative>0)
                return (100 / m_Relative) * containerWidth;
            return 0;
        }

		private void OnSizeChanged()
		{
			if(WidthChanged!=null)
				WidthChanged(this,new EventArgs());
		}

        private bool _AutoSize = false;
        /// <summary>
        /// Gets or sets whether column width is automatically set based on the column's content. Default value is false.
        /// When set absolute and relative size values are ignored.
        /// </summary>
        [DefaultValue(false), Description("Indicates whether column is sized based on the content.")]
        public bool AutoSize
        {
            get { return _AutoSize; }
            set
            {
                if (_AutoSize != value)
                {
                    _AutoSize = value;
                    OnSizeChanged();
                }
            }
        }

        internal void SetAutoSize(bool autoSize)
        {
            _AutoSize = autoSize;
        }

        private bool _AutoSizeMinHeader = false;
        /// <summary>
        /// Gets or sets whether column auto-width is set to minimum of the column header text width. Applies to AutoSize=true only.
        /// </summary>
        [DefaultValue(false), Description("Indicates whether column auto-width is set to minimum of the column header text width. Applies to AutoSize=true only.")]
        public bool AutoSizeMinHeader
        {
            get { return _AutoSizeMinHeader; }
            set
            {
                _AutoSizeMinHeader = value;
                OnSizeChanged();
            }
        }
        

        private int _AutoSizeWidth = 0;
        /// <summary>
        /// Gets the auto-size calculated width of the column after tree layout is performed and column has AutoSize=true.
        /// </summary>
        [Browsable(false)]
        public int AutoSizeWidth
        {
            get { return _AutoSizeWidth; }
//            internal set
//            {
//                _AutoSizeWidth = value;
//            }
        }
		internal void SetAutoSizeWidth(int value)
		{
			_AutoSizeWidth = value;
		}
        
	}
}
