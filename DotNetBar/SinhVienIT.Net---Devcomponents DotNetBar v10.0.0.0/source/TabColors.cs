using System;
using System.ComponentModel;
using System.Drawing;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Represents colors for the active tab.
	/// </summary>
	[ToolboxItem(false), TypeConverter(typeof(System.ComponentModel.ExpandableObjectConverter))]
	public class TabColors
	{
		#region Events
		/// <summary>
		/// Occurs after color property has changed.
		/// </summary>
		public event EventHandler ColorChanged;
		#endregion

		#region Private Variables
		private Color m_BackColor=Color.Empty;
		private Color m_BackColor2=Color.Empty;
		private int m_BackColorGradientAngle=90;
		private Color m_LightBorderColor=Color.Empty;
		private Color m_DarkBorderColor=Color.Empty;
		private Color m_BorderColor=Color.Empty;
		private Color m_TextColor=Color.Empty;
        private BackgroundColorBlendCollection m_BackgroundColorBlend = new BackgroundColorBlendCollection();
		#endregion

		#region Internal Implementation
		/// <summary>
		/// Creates new instance of the object.
		/// </summary>
		public TabColors(){}
		/// <summary>
		/// Gets or sets the background color of the tab when inactive.
		/// </summary>
		[Browsable(true),Description("Indicates the inactive tab background color."),Category("Style")]
		public Color BackColor
		{
			get {return m_BackColor;}
			set
			{
				m_BackColor=value;
				this.Refresh();
			}
		}
		/// <summary>
		/// Returns whether property should be serialized.
		/// </summary>
		/// <returns>true if property should be serialized otherwise false.</returns>
		[Browsable(false),EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeBackColor()
		{
			return !m_BackColor.IsEmpty;
		}
		/// <summary>
		/// Resets property to the default value.
		/// </summary>
		[Browsable(false),EditorBrowsable(EditorBrowsableState.Never)]
		public void ResetBackColor()
		{
			BackColor=Color.Empty;
		}

		/// <summary>
		/// Gets or sets the target gradient background color of the tab when inactive.
		/// </summary>
		[Browsable(true),Description("Indicates the inactive tab target gradient background color."),Category("Style")]
		public Color BackColor2
		{
			get {return m_BackColor2;}
			set
			{
				m_BackColor2=value;
				this.Refresh();
			}
		}
		/// <summary>
		/// Returns whether property should be serialized.
		/// </summary>
		/// <returns>true if property should be serialized otherwise false.</returns>
		[Browsable(false),EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeBackColor2()
		{
			return !m_BackColor2.IsEmpty;
		}
		/// <summary>
		/// Resets property to the default value.
		/// </summary>
		[Browsable(false),EditorBrowsable(EditorBrowsableState.Never)]
		public void ResetBackColor2()
		{
			BackColor2=Color.Empty;
		}

		/// <summary>
		/// Gets or sets the gradient angle.
		/// </summary>
		[Browsable(true),Description("Indicates the gradient angle."),Category("Style"),DefaultValue(90)]
		public int BackColorGradientAngle
		{
			get {return m_BackColorGradientAngle;}
			set {m_BackColorGradientAngle=value;this.Refresh();}
		}

        /// <summary>
        /// Gets the collection that defines the multi-color gradient background for tab item..
        /// </summary>
        [Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Collection that defines the multicolor gradient background.")]
        public BackgroundColorBlendCollection BackgroundColorBlend
        {
            get { return m_BackgroundColorBlend; }
        }

		/// <summary>
		/// Gets or sets the light border color when tab is inactive.
		/// </summary>
		[Browsable(true),Description("Indicates the inactive tab light border color."),Category("Style")]
		public Color LightBorderColor
		{
			get {return m_LightBorderColor;}
			set
			{
				m_LightBorderColor=value;
				this.Refresh();
			}
		}
		/// <summary>
		/// Returns whether property should be serialized.
		/// </summary>
		/// <returns>true if property should be serialized otherwise false.</returns>
		[Browsable(false),EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeLightBorderColor()
		{
			return !m_LightBorderColor.IsEmpty;
		}
		/// <summary>
		/// Resets property to the default value.
		/// </summary>
		[Browsable(false),EditorBrowsable(EditorBrowsableState.Never)]
		public void ResetLightBorderColor()
		{
			LightBorderColor=Color.Empty;
		}

		/// <summary>
		/// Gets or sets the dark border color when tab is inactive.
		/// </summary>
		[Browsable(true),Description("Indicates the inactive tab dark border color."),Category("Style")]
		public Color DarkBorderColor
		{
			get {return m_DarkBorderColor;}
			set
			{
				m_DarkBorderColor=value;
				this.Refresh();
			}
		}
		/// <summary>
		/// Returns whether property should be serialized.
		/// </summary>
		/// <returns>true if property should be serialized otherwise false.</returns>
		[Browsable(false),EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeDarkBorderColor()
		{
			return !m_DarkBorderColor.IsEmpty;
		}
		/// <summary>
		/// Resets property to the default value.
		/// </summary>
		[Browsable(false),EditorBrowsable(EditorBrowsableState.Never)]
		public void ResetDarkBorderColor()
		{
			DarkBorderColor=Color.Empty;
		}

		/// <summary>
		/// Gets or sets the border color when tab is inactive.
		/// </summary>
		[Browsable(true),Description("Indicates the inactive tab border color."),Category("Style")]
		public Color BorderColor
		{
			get {return m_BorderColor;}
			set
			{
				m_BorderColor=value;
				this.Refresh();
			}
		}
		/// <summary>
		/// Returns whether property should be serialized.
		/// </summary>
		/// <returns>true if property should be serialized otherwise false.</returns>
		[Browsable(false),EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeBorderColor()
		{
			return !m_BorderColor.IsEmpty;
		}
		/// <summary>
		/// Resets property to the default value.
		/// </summary>
		[Browsable(false),EditorBrowsable(EditorBrowsableState.Never)]
		public void ResetBorderColor()
		{
			BorderColor=Color.Empty;
		}

		/// <summary>
		/// Gets or sets the text color when tab is inactive.
		/// </summary>
		[Browsable(true),Description("Indicates the inactive tab text color."),Category("Style")]
		public Color TextColor
		{
			get {return m_TextColor;}
			set
			{
				m_TextColor=value;
				this.Refresh();
			}
		}
		/// <summary>
		/// Returns whether property should be serialized.
		/// </summary>
		/// <returns>true if property should be serialized otherwise false.</returns>
		[Browsable(false),EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeTextColor()
		{
			return !m_TextColor.IsEmpty;
		}
		/// <summary>
		/// Resets property to the default value.
		/// </summary>
		[Browsable(false),EditorBrowsable(EditorBrowsableState.Never)]
		public void ResetTextColor()
		{
			TextColor=Color.Empty;
		}

		private void Refresh()
		{
			if(ColorChanged!=null)
				ColorChanged(this,new EventArgs());
		}
		#endregion
	}
}
