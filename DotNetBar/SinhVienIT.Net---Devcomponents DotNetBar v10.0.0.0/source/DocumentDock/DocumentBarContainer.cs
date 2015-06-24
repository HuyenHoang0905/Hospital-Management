using System;
using System.ComponentModel;
using System.Drawing;
using System.Xml;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Represents class that is a bar container for document docking.
	/// </summary>
	[DesignTimeVisible(false),ToolboxItem(false),TypeConverter(typeof(DocumentBarContainerConverter))]
	public class DocumentBarContainer:DocumentBaseContainer
	{
		#region Private Variables and Constructor
		private Bar m_Bar=null;

		/// <summary>
		/// Creates new instance of the class.
		/// </summary>
		public DocumentBarContainer(){}

		/// <summary>
		/// Creates new instance of the class and initializes it with the Bar object.
		/// </summary>
		/// <param name="bar">Bar to contain on document.</param>
		public DocumentBarContainer(Bar bar)
		{
			m_Bar=bar;
            if(m_Bar.SplitDockHeight>0 || m_Bar.SplitDockWidth>0)
				this.SetLayoutBounds(new Rectangle(0,0,m_Bar.SplitDockWidth,m_Bar.SplitDockHeight));
		}

		/// <summary>
		/// Creates new instance of the class and initializes it with the bar and propesed width and height.
		/// </summary>
		/// <param name="bar">Bar to contain on document.</param>
		/// <param name="proposedWidth">Proposed width of the document in pixels</param>
		/// <param name="proposedHeight">Proposed height of the document in pixels</param>
		public DocumentBarContainer(Bar bar, int proposedWidth, int proposedHeight)
		{
			m_Bar=bar;
			this.SetLayoutBounds(new Rectangle(0,0,proposedWidth,proposedHeight));
		}
		#endregion

		#region Internal Implementation
		/// <summary>
		/// Gets or sets the bar that is contained by this document.
		/// </summary>
		[Browsable(false),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Bar Bar
		{
			get {return m_Bar;}
			set {m_Bar=value;}
		}

		/// <summary>
		/// Resizes document object.
		/// </summary>
		/// <param name="bounds">Available bounds for the document.</param>
		public override void Layout(Rectangle bounds)
		{
            if (m_Bar != null)
            {
                m_Bar.SplitDockHeight = 0;
                m_Bar.SplitDockWidth = 0;
                m_Bar.MinHeight = 0;
                m_Bar.RecalcSize(bounds.Size);
                m_Bar.Location = bounds.Location;
            }
			SetDisplayBounds(bounds);
		}

		/// <summary>
		/// Gets whether document is visible.
		/// </summary>
		public override bool Visible
		{
			get
			{
                if (m_Bar == null)
                    return false;
				if(m_Bar.GetDesignMode())
					return true;
                return m_Bar.IsVisible;
			}
		}

		/// <summary>
		/// Gets the minimum size of the document.
		/// </summary>
		protected internal override System.Drawing.Size MinimumSize
		{
			get {return m_Bar.GetAdjustedFullSize(m_Bar.MinimumDockSize(eOrientation.Horizontal));}
		}
		#endregion
	}
}
