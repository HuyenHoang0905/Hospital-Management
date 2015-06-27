using System;
using System.ComponentModel;
using System.Drawing;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Represents Document container base class.
	/// </summary>
	public abstract class DocumentBaseContainer
	{
		#region Private Variables and Constructor
		private Rectangle m_LayoutBounds=Rectangle.Empty;
		private Rectangle m_DisplayBounds=Rectangle.Empty;
		private DocumentBaseContainer m_Parent=null;
        private static long IdCounter = 0;
        private long _Id = 0;
		/// <summary>
		/// Creates new instance of the class.
		/// </summary>
		public DocumentBaseContainer()
		{
            IdCounter++;
            _Id = IdCounter;
		}
		#endregion

		#region Internal Implementation
        /// <summary>
        /// Gets the unique ID for the container.
        /// </summary>
        public long Id
        {
            get { return _Id; }
        }
        
		/// <summary>
		/// Returns actual display bounds of the document.
		/// </summary>
		[Browsable(false)]
		public virtual Rectangle DisplayBounds
		{
			get {return m_DisplayBounds;}
		}

		/// <summary>
		/// Returns layout bounds of the document. Layout bounds are proposed bounds of the layout and might not be the same
		/// as DisplayBounds.
		/// </summary>
		[Browsable(false)]
		public virtual Rectangle LayoutBounds
		{
			get {return m_LayoutBounds;}
		}

		/// <summary>
		/// Gets the parent container.
		/// </summary>
		public virtual DocumentBaseContainer Parent
		{
			get {return m_Parent;}
		}

		/// <summary>
		/// Resizes the document within specified bounds.
		/// </summary>
		/// <param name="bounds">Area available for the document.</param>
		public abstract void Layout(Rectangle bounds);

		/// <summary>
		/// Sets the display bounds.
		/// </summary>
		/// <param name="r">New display bounds.</param>
		internal void SetDisplayBounds(Rectangle r)
		{
			m_DisplayBounds=r;
		}

		/// <summary>
		/// Sets layout bounds.
		/// </summary>
		/// <param name="r">New layout bounds.</param>
		internal void SetLayoutBounds(Rectangle r)
		{
			m_LayoutBounds=r;
		}

        /// <summary>
        /// Resets the layout bounds for the document base container to the empty bounds.
        /// </summary>
        public void ResetLayoutBounds()
        {
            m_LayoutBounds = Rectangle.Empty;
        }

        /// <summary>
        /// Resets the layout bounds for the document base container to the empty bounds.
        /// </summary>
        public void ResetDisplayBounds()
        {
            m_DisplayBounds = Rectangle.Empty;
        }

		/// <summary>
		/// Sets the parent document.
		/// </summary>
		/// <param name="parent">Parent container.</param>
		internal void SetParent(DocumentBaseContainer parent)
		{
			m_Parent=parent;
		}

		/// <summary>
		/// Sets the width of the document.
		/// </summary>
		/// <param name="width">Width in pixels</param>
		public virtual void SetWidth(int width)
		{
			if(m_Parent!=null)
			{
				//if(!m_Parent.OnSetWidth(this,width))
					//return;
                m_Parent.OnSetWidth(this, width);
			}

            if (width >= this.MinimumSize.Width || this.MinimumSize.Width == 0)
            {
                ResetDisplayBounds();
                m_LayoutBounds.Width = width;
            }
		}

		/// <summary>
		/// Sets the height of the document.
		/// </summary>
		/// <param name="height">Height in pixels.</param>
		public virtual void SetHeight(int height)
		{
			if(m_Parent!=null)
				m_Parent.OnSetHeight(this,height);

            if (height >= this.MinimumSize.Height || this.MinimumSize.Height == 0)
            {
                ResetDisplayBounds();
                m_LayoutBounds.Height = height;
            }
		}

		/// <summary>
		/// Occurs when width is being set on child document.
		/// </summary>
		/// <param name="doc">Reference document being changed</param>
		/// <param name="width">Width in pixels</param>
		/// <returns>True if width was applied by parent otherwise false</returns>
		protected internal virtual bool OnSetWidth(DocumentBaseContainer doc, int width){return false;}

		/// <summary>
		/// Occurs when height is being set on child document.
		/// </summary>
		/// <param name="doc">Reference document being changed</param>
		/// <param name="height">Height in pixels</param>
		/// <returns>True if width was applied by parent otherwise false</returns>
		protected internal virtual bool OnSetHeight(DocumentBaseContainer doc, int height){return false;}

		/// <summary>
		/// Gets whether document is visible or not.
		/// </summary>
		public abstract bool Visible {get;}

		/// <summary>
		/// Gets minimum size of the document.
		/// </summary>
		protected internal abstract System.Drawing.Size MinimumSize {get;}

		#endregion
	}
}
