using System;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using System.Xml;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Represents Dock container with either horizontal or vertical layout.
	/// </summary>
	[DesignTimeVisible(false),ToolboxItem(false),TypeConverter(typeof(DocumentDockContainerConverter))]
	public class DocumentDockContainer:DocumentBaseContainer
	{
		#region Private Variables
		private eOrientation m_Orientation=eOrientation.Horizontal;
		private DocumentBaseContainerCollection m_Documents=new DocumentBaseContainerCollection();
		private int m_SplitterSize=3;
		private Size m_MinimumSize=Size.Empty;
        private bool m_RecordDocumentSize = false;
		#endregion

		#region Internal Implementation

		/// <summary>
		/// Creates new instance of the object and initializes it with specified values.
		/// </summary>
		/// <param name="documents">Array of documents to host in this container.</param>
		/// <param name="orientation">Container orientation</param>
		public DocumentDockContainer(DocumentBaseContainer[] documents, eOrientation orientation)
		{
			m_Orientation=orientation;
			m_Documents.Owner=this;
			m_Documents.DocumentAdded+=new EventHandler(DocumentAdded);
			m_Documents.DocumentRemoved+=new EventHandler(DocumentRemoved);

			if(documents!=null)
			{
				foreach(DocumentBaseContainer doc in documents)
					m_Documents.Add(doc);
			}
		}

		/// <summary>
		/// Creates new instance of the object.
		/// </summary>
		public DocumentDockContainer():this(null,eOrientation.Horizontal){}

		/// <summary>
		/// Resizes the object inside of the given bounds.
		/// </summary>
		/// <param name="bounds">Available area.</param>
		public override void Layout(Rectangle bounds)
		{
			if(LayoutInternal(bounds))
				this.SetDisplayBounds(bounds);
			else
				this.SetDisplayBounds(Rectangle.Empty);
		}

        private struct DocumentMeasureInfo
        {
            public int TotalSize;
            public int VisibleCount;
        }

        private DocumentMeasureInfo MeasureDocuments(Rectangle bounds)
        {
            DocumentMeasureInfo info = new DocumentMeasureInfo();

            if (m_Documents.Count == 0)
                return info;

            int boundsSize = (m_Orientation == eOrientation.Horizontal ? bounds.Width : bounds.Height);
            int defaultSize = 0; // (boundsSize - ((m_Documents.Count - 1) * m_SplitterSize)) / m_Documents.Count;
            
            // Check whether default size is below minimum size and make appropriate adjustments...
            int[] dockSizes = new int[m_Documents.Count];
            bool adjustDefaultSize = false;
            int defaultSizeCount = m_Documents.Count;
            do
            {
                defaultSize = (boundsSize - ((defaultSizeCount - 1) * m_SplitterSize)) / defaultSizeCount;
                adjustDefaultSize = false;
                for (int i = 0; i < m_Documents.Count; i++)
                {
                    if (dockSizes[i] != 0) continue;

                    DocumentBaseContainer doc = m_Documents[i];
                    if (m_Orientation == eOrientation.Horizontal && doc.LayoutBounds.Width == 0 && doc.MinimumSize.Width > defaultSize)
                    {
                        dockSizes[i] = doc.MinimumSize.Width;
                        adjustDefaultSize = true;
                        boundsSize -= dockSizes[i];
                        defaultSizeCount--;
                        break;
                    }
                    else if (m_Orientation == eOrientation.Vertical && doc.LayoutBounds.Height == 0 && doc.MinimumSize.Height > defaultSize)
                    {
                        dockSizes[i] = doc.MinimumSize.Height;
                        adjustDefaultSize = true;
                        boundsSize -= dockSizes[i];
                        defaultSizeCount--;
                        break;
                    }
                }
            } while (adjustDefaultSize && defaultSizeCount>0);

            int totalSize = 0;
            int visibleCount = 0;
            for (int i = 0; i < m_Documents.Count; i++)
            {
                DocumentBaseContainer doc = m_Documents[i];
                if (m_Orientation == eOrientation.Horizontal && doc.LayoutBounds.Width == 0)
                    doc.SetLayoutBounds(new Rectangle(0, 0, (dockSizes[i] > 0 ? dockSizes[i] : defaultSize), bounds.Height));
                else if (m_Orientation == eOrientation.Vertical && doc.LayoutBounds.Height == 0)
                    doc.SetLayoutBounds(new Rectangle(0, 0, bounds.Width, (dockSizes[i] > 0 ? dockSizes[i] : defaultSize)));

                if (doc.Visible)
                {
                    totalSize += ((m_Orientation == eOrientation.Horizontal ? doc.LayoutBounds.Width : doc.LayoutBounds.Height) + m_SplitterSize);
                    visibleCount++;
                }
            }
            if (visibleCount > 0)
                totalSize -= m_SplitterSize;

            info.TotalSize = totalSize;
            info.VisibleCount = visibleCount;
            return info;
        }

        private bool AdjustLayoutBoundsForMinimumSize(float m, Rectangle layoutBounds)
        {
            bool layoutAdjusted = false;
            foreach(DocumentBaseContainer doc in m_Documents)
            {
                if (!doc.Visible) continue;
                Rectangle docBounds;
                if (m_Orientation == eOrientation.Horizontal)
                {
                    docBounds = new Rectangle(layoutBounds.X, layoutBounds.Y, (int)(doc.LayoutBounds.Width * m), layoutBounds.Height);
                    if (docBounds.Width < doc.MinimumSize.Width)
                    {
                        layoutAdjusted = true;
                        break;
                    }
                }
                else
                {
                    docBounds = new Rectangle(layoutBounds.X, layoutBounds.Y, layoutBounds.Width, (int)(doc.LayoutBounds.Height * m));
                    if (docBounds.Height < doc.MinimumSize.Height)
                    {
                        layoutAdjusted = true;
                        break;
                    }
                }
            }

            if (layoutAdjusted)
            {
                foreach (DocumentBaseContainer doc in m_Documents)
                    doc.SetLayoutBounds(Rectangle.Empty);
            }

            return layoutAdjusted;
        }

		private bool LayoutInternal(Rectangle bounds)
		{
			if(m_Documents.Count==0)
				return false;

			int boundsSize=(m_Orientation==eOrientation.Horizontal?bounds.Width:bounds.Height);
            DocumentMeasureInfo info = MeasureDocuments(bounds);
            if (info.VisibleCount == 0)
                return false;
            int totalSize = info.TotalSize;
            int visibleCount = info.VisibleCount;
			float m=(float)boundsSize/(float)totalSize;

            if(AdjustLayoutBoundsForMinimumSize(m, bounds))
                info = MeasureDocuments(bounds);

			Rectangle layoutBounds=bounds;
			int processed=0;
			bool setSize=true;
            if (!m_RecordDocumentSize && Math.Abs(totalSize - boundsSize) / m_Documents.Count != (int)(Math.Abs(totalSize - boundsSize) / m_Documents.Count) * m_Documents.Count)
                setSize = false;
			bool resize=(totalSize!=boundsSize);

			for(int i=0;i<m_Documents.Count;i++)
			{
				DocumentBaseContainer doc=m_Documents[i];
				if(doc.Visible)
				{
					processed++;
					Rectangle docBounds;
					if(resize)
					{
						if(m_Orientation==eOrientation.Horizontal)
							docBounds=new Rectangle(layoutBounds.X,layoutBounds.Y,(int)(doc.LayoutBounds.Width*m),layoutBounds.Height);
						else
							docBounds=new Rectangle(layoutBounds.X,layoutBounds.Y,layoutBounds.Width,(int)(doc.LayoutBounds.Height*m));
					}
					else
					{
						if(m_Orientation==eOrientation.Horizontal)
							docBounds=new Rectangle(layoutBounds.X,layoutBounds.Y,doc.LayoutBounds.Width,layoutBounds.Height);
						else
							docBounds=new Rectangle(layoutBounds.X,layoutBounds.Y,layoutBounds.Width,doc.LayoutBounds.Height);
					}

					if(processed==visibleCount)
						docBounds=layoutBounds;

					doc.Layout(docBounds);

					if(setSize)	doc.SetLayoutBounds(docBounds);

					if(m_Orientation==eOrientation.Horizontal)
					{
						layoutBounds.Width-=(docBounds.Width+m_SplitterSize);
						layoutBounds.X+=(docBounds.Width+m_SplitterSize);
					}
					else
					{
						layoutBounds.Height-=(docBounds.Height+m_SplitterSize);
						layoutBounds.Y+=(docBounds.Height+m_SplitterSize);
					}
				}
			}

			return true;
		}

		/// <summary>
		/// Gets whether document is visible or not.
		/// </summary>
		public override bool Visible
		{
			get
			{
				
				foreach(DocumentBaseContainer doc in m_Documents)
				{
					if(doc.Visible)
						return true;
				}
				return false;
			}
		}

		/// <summary>
		/// Gets the orientation of the container. Default value is Horizontal.
		/// </summary>
		[Browsable(false),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public eOrientation Orientation
		{
			get {return m_Orientation;}
			set
			{
				if(m_Orientation!=value)
				{
					m_Orientation=value;
					OnOrientationChanged();
				}
			}
		}

		/// <summary>
		/// Returns collection of the documents hosted by this container.
		/// </summary>
		[Browsable(false),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public DocumentBaseContainerCollection Documents
		{
			get {return m_Documents;}
		}

		/// <summary>
		/// Occurs when width is being set on child document.
		/// </summary>
		/// <param name="doc">Reference document being changed</param>
		/// <param name="width">Width in pixels</param>
		/// <returns>True if width was applied by parent otherwise false</returns>
		protected internal override bool OnSetWidth(DocumentBaseContainer doc, int width)
		{
			bool ret=false;
			if(m_Orientation!=eOrientation.Horizontal || width<doc.MinimumSize.Width && doc.MinimumSize.Width>0)
				return ret;

			DocumentBaseContainer pairDoc=null;
			DocumentBaseContainer previousDoc=null;

			int refIndex=m_Documents.IndexOf(doc);

			// Lock in the display size if it is set
			for(int i=0;i<m_Documents.Count;i++)
			{
				DocumentBaseContainer dc=m_Documents[i];
                if (!dc.Visible) continue;
				if(dc.DisplayBounds.Width>0)
					dc.SetLayoutBounds(dc.DisplayBounds);
				if(i>refIndex && dc.Visible && pairDoc==null)
					pairDoc=dc;
				else if(i<refIndex && dc.Visible)
					previousDoc=dc;
			}

			int diff=doc.LayoutBounds.Width-width;
			if(pairDoc!=null && pairDoc.LayoutBounds.Width>0 && pairDoc.LayoutBounds.Width+diff>0 && 
				(pairDoc.LayoutBounds.Width+diff>=pairDoc.MinimumSize.Width || pairDoc.MinimumSize.Width==0))
			{
				pairDoc.SetLayoutBounds(new Rectangle(pairDoc.LayoutBounds.X,pairDoc.LayoutBounds.Y,pairDoc.LayoutBounds.Width+diff,pairDoc.LayoutBounds.Height));
				ret=true;
			}
            else if (pairDoc == null && previousDoc != null && previousDoc.LayoutBounds.Width > 0 && previousDoc.LayoutBounds.Width + diff > 0 &&
                (previousDoc.LayoutBounds.Width + diff >= previousDoc.MinimumSize.Width || previousDoc.MinimumSize.Width == 0))
            {
                doc.SetLayoutBounds(new Rectangle(doc.LayoutBounds.X, doc.LayoutBounds.Y, width, doc.LayoutBounds.Height));
                // Resetting previous document width caused problem with ever growing bar when Width of single bar is set
                // Reason is that resetting width here will cause the new space in container to be proportionally allocated thus setting single bar width which is intent of this function never works
                //previousDoc.SetLayoutBounds(new Rectangle(previousDoc.LayoutBounds.X, previousDoc.LayoutBounds.Y, 0, previousDoc.LayoutBounds.Height));
                ret = true;
            }

			return ret;
		}

		/// <summary>
		/// Occurs when height is being set on child document.
		/// </summary>
		/// <param name="doc">Reference document being changed</param>
		/// <param name="height">Height in pixels</param>
		/// <returns>True if width was applied by parent otherwise false</returns>
		protected internal override bool OnSetHeight(DocumentBaseContainer doc, int height)
		{
			bool ret=false;
			if(m_Orientation!=eOrientation.Vertical || height<doc.MinimumSize.Height && doc.MinimumSize.Height>0)
				return ret;

			DocumentBaseContainer pairDoc=null;
			DocumentBaseContainer previousDoc=null;

			int refIndex=m_Documents.IndexOf(doc);

			// Lock in the display size if it is set
			for(int i=0;i<m_Documents.Count;i++)
			{
				DocumentBaseContainer dc=m_Documents[i];
                if (!dc.Visible) continue;
				if(dc.DisplayBounds.Height>0)
					dc.SetLayoutBounds(dc.DisplayBounds);
				if(i>refIndex && dc.Visible && pairDoc==null)
					pairDoc=dc;
				else if(i<refIndex && dc.Visible)
					previousDoc=dc;
			}

			if(pairDoc==null) pairDoc=previousDoc;

			int diff=doc.LayoutBounds.Height-height;
			if(pairDoc!=null && pairDoc.LayoutBounds.Height>0 && pairDoc.LayoutBounds.Height+diff>0 && 
				(pairDoc.LayoutBounds.Height+diff>=pairDoc.MinimumSize.Height || pairDoc.MinimumSize.Height==0))
			{
				pairDoc.SetLayoutBounds(new Rectangle(pairDoc.LayoutBounds.X,pairDoc.LayoutBounds.Y,pairDoc.LayoutBounds.Width,pairDoc.LayoutBounds.Height+diff));
				ret=true;
			}
            else if (pairDoc == null && previousDoc != null && previousDoc.LayoutBounds.Height > 0 && previousDoc.LayoutBounds.Height + diff > 0 &&
                (previousDoc.LayoutBounds.Height + diff >= previousDoc.MinimumSize.Height || previousDoc.MinimumSize.Height == 0))
            {
                doc.SetLayoutBounds(new Rectangle(doc.LayoutBounds.X, doc.LayoutBounds.Y, doc.LayoutBounds.Width, height));
                previousDoc.SetLayoutBounds(new Rectangle(previousDoc.LayoutBounds.X, previousDoc.LayoutBounds.Y, previousDoc.LayoutBounds.Width, 0));
                ret = true;
            }
			return ret;
		}

		/// <summary>
		/// Returns the DocumentBarContainer object for a given bar.
		/// </summary>
		/// <param name="bar">Bar to search for.</param>
		/// <returns>Reference to container or null if bar could not be found</returns>
		public DocumentBarContainer GetBarDocumentContainer(Bar bar)
		{
			foreach(DocumentBaseContainer doc in m_Documents)
			{
				if(doc is DocumentBarContainer && ((DocumentBarContainer)doc).Bar==bar)
					return (DocumentBarContainer)doc;
				else if(doc is DocumentDockContainer)
				{
					DocumentBarContainer db=((DocumentDockContainer)doc).GetBarDocumentContainer(bar);
					if(db!=null) return db;
				}
			}
			return null;
		}

		/// <summary>
		/// Returns minimum size of the object.
		/// </summary>
		protected internal override System.Drawing.Size MinimumSize
		{
			get 
			{
				return m_MinimumSize;
			}
		}

		private void DocumentAdded(object sender, EventArgs e)
		{
			DocumentBaseContainer doc=sender as DocumentBaseContainer;
			Size s=doc.MinimumSize;
			if(m_Orientation==eOrientation.Horizontal)
			{
				m_MinimumSize.Width+=(s.Width+m_SplitterSize);
				if(s.Height>m_MinimumSize.Height)
					m_MinimumSize.Height=s.Height;
			}
			else
			{
				m_MinimumSize.Height+=(s.Height+m_SplitterSize);
				if(s.Width>m_MinimumSize.Width)
					m_MinimumSize.Width=s.Width;
			}
		}

		internal void DocumentRemoved(object sender, EventArgs e)
		{
			RefreshMinimumSize();
		}

		internal void RefreshMinimumSize()
		{
			m_MinimumSize=Size.Empty;
			foreach(DocumentBaseContainer doc in m_Documents)
			{
				Size s=doc.MinimumSize;
				if(m_Orientation==eOrientation.Horizontal)
				{
					m_MinimumSize.Width+=(s.Width+m_SplitterSize);
					if(s.Height>m_MinimumSize.Height)
						m_MinimumSize.Height=s.Height;
				}
				else
				{
					m_MinimumSize.Height+=(s.Height+m_SplitterSize);
					if(s.Width>m_MinimumSize.Width)
						m_MinimumSize.Width=s.Width;
				}
			}
		}

		private void OnOrientationChanged()
		{
			ResetDocumentsLayout();
		}

		private void ResetDocumentsLayout()
		{
			foreach(DocumentBaseContainer doc in m_Documents)
				doc.SetLayoutBounds(Rectangle.Empty);
		}

		/// <summary>
		/// Gets or sets splitter size in pixels between the documents docking inside the container. Default value is 3.
		/// </summary>
		[Browsable(true),DefaultValue(3),Description("Indicates the splitter size between the documents docking inside the container."),Category("Layout")]
		public virtual int SplitterSize
		{
			get {return m_SplitterSize;}
			set {m_SplitterSize=value;}
		}

        /// <summary>
        /// Gets or sets whether the size of the documents is recorded once the layout is calculated.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool RecordDocumentSize
        {
            get { return m_RecordDocumentSize; }
            set { m_RecordDocumentSize = value; }
        }
		#endregion
	}
}
