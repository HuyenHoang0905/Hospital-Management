using System;
using System.Windows.Forms;
using System.Drawing;
using System.Xml;
using System.ComponentModel;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Represents class that manages document docking UI interaction.
	/// </summary>
	public class DocumentDockUIManager
	{
		#region Private Variables, Constructor
		private DocumentDockContainer m_DocumentDockContainer=null;
		private Cursor m_ContainerCursor=null;
		private DockSite m_Container=null;
		private Point m_MouseDownPoint=Point.Empty;
		private bool m_ResizePreview=false;
		private Form m_Splitter=null;
		private DocumentDockContainer m_ResizeDockContainer=null;
		private DocumentBaseContainer m_ResizeDocument=null;
		private int m_SplitterWidth=3;
		private bool m_LoadingLayout=false;

		public DocumentDockUIManager(DockSite container)
		{
			m_Container=container;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the root document dock container object.
		/// </summary>
		public DocumentDockContainer RootDocumentDockContainer
		{
			get {return m_DocumentDockContainer;}
			set {m_DocumentDockContainer=value;}
		}

		/// <summary>
		/// Gets or sets the container of the document. 
		/// </summary>
		public DockSite Container
		{
			get {return m_Container;}
			set {m_Container=value;}
		}
		#endregion

		#region Methods
		/// <summary>
		/// Estimates the docking position and size of a given bar for outline purposes during drag&drop
		/// </summary>
		/// <param name="bar">Bar to find estimated docking position and size for</param>
		/// <param name="pDockInfo">Docking information</param>
		/// <returns>Preview Rectangle in screen coordinates.</returns>
		internal Rectangle GetDockPreviewRectangle(Bar bar, ref DockSiteInfo dockInfo)
		{
			DocumentBaseContainer referenceDoc=GetDocumentFromBar(dockInfo.MouseOverBar);
			Rectangle r=Rectangle .Empty;

            // Handle edge case docking first
            if (m_Container.Dock!= DockStyle.Fill && (dockInfo.DockLine == 999 || dockInfo.DockLine == -1))
            {
                return GetEdgeDockPreviewRectangle(bar, ref dockInfo);
            }
			else if(referenceDoc!=null)
			{
				if(dockInfo.MouseOverDockSide==eDockSide.Left)
				{
					r=referenceDoc.DisplayBounds;
					r.Width=r.Width/2;
				}
				else if(dockInfo.MouseOverDockSide==eDockSide.Right)
				{
					r=referenceDoc.DisplayBounds;
					r.X+=r.Width/2;
					r.Width=r.Width/2;
				}
				else if(dockInfo.MouseOverDockSide==eDockSide.Top)
				{
					r=referenceDoc.DisplayBounds;
					r.Height=r.Height/2;
				}
				else if(dockInfo.MouseOverDockSide==eDockSide.Bottom)
				{
					r=referenceDoc.DisplayBounds;
					r.Y+=r.Height/2;
					r.Height=r.Height/2;
				}
				else if(dockInfo.MouseOverDockSide==eDockSide.Document)
				{
					r=referenceDoc.DisplayBounds;
				}
				r.Location=m_Container.PointToScreen(r.Location);
			}
			else
			{
                if (m_Container.Dock == DockStyle.Fill)
                {
                    r = m_Container.ClientRectangle;
                    r.Location = m_Container.PointToScreen(r.Location);
                }
                else if (m_Container.Dock == DockStyle.Right)
                {
                    r = m_Container.ClientRectangle;
                    r.Location = m_Container.PointToScreen(r.Location);
                    if (r.Width == 0)
                    {
                        r.Width = bar.GetBarDockedSize(eOrientation.Vertical);
                        r.X -= r.Width;
                    }
                }
                else if (m_Container.Dock == DockStyle.Left)
                {
                    r = m_Container.ClientRectangle;
                    r.Location = m_Container.PointToScreen(r.Location);
                    if (r.Width == 0)
                    {
                        r.Width = bar.GetBarDockedSize(eOrientation.Vertical);
                    }
                }
                else if (m_Container.Dock == DockStyle.Top)
                {
                    r = m_Container.ClientRectangle;
                    r.Location = m_Container.PointToScreen(r.Location);
                    if (r.Height == 0)
                    {
                        r.Height = bar.GetBarDockedSize(eOrientation.Horizontal);
                    }
                }
                else if (m_Container.Dock == DockStyle.Bottom)
                {
                    r = m_Container.ClientRectangle;
                    r.Location = m_Container.PointToScreen(r.Location);
                    if (r.Height == 0)
                    {
                        r.Height = bar.GetBarDockedSize(eOrientation.Horizontal);
                        r.Y -= r.Height;
                    }
                }
			}

			return r;
		}

        private Rectangle GetEdgeDockPreviewRectangle(Bar bar, ref DockSiteInfo dockInfo)
        {
            Rectangle r = Rectangle.Empty;
            int fullSizeIndex = -1;
            int partialSizeIndex = -1;
            if (dockInfo.FullSizeDock)
                fullSizeIndex = m_Container.GetFullSizeIndex();
            else if (dockInfo.PartialSizeDock)
                partialSizeIndex = m_Container.GetPartialSizeIndex();

            // Docks the bar to the edge
            switch (m_Container.Dock)
            {
                case DockStyle.Top:
                    {
                        r.Width = m_Container.ClientRectangle.Width;
                        r.Height = bar.GetBarDockedSize(eOrientation.Horizontal);

                        if (fullSizeIndex >= 0)
                        {
                            r.Width += m_Container.GetSiteZOrderSize(m_Container.Owner.LeftDockSite, true).Width;
                            r.Width += m_Container.GetSiteZOrderSize(m_Container.Owner.RightDockSite, true).Width;
                            dockInfo.DockSiteZOrderIndex = fullSizeIndex;
                        }
                        else if (partialSizeIndex >= 0)
                        {
                            // Reduce by the size of the left and right dock site
                            r.Width -= m_Container.GetSiteZOrderSize(m_Container.Owner.LeftDockSite, false).Width;
                            r.Width -= m_Container.GetSiteZOrderSize(m_Container.Owner.RightDockSite, false).Width;
                            dockInfo.DockSiteZOrderIndex = partialSizeIndex;

                        }

                        Point p = Point.Empty;
                        if (dockInfo.DockLine == -1)
                        {
                            p = m_Container.PointToScreen(m_Container.ClientRectangle.Location);
                        }
                        else
                        {
                            p = m_Container.PointToScreen(new Point(m_Container.ClientRectangle.X, m_Container.ClientRectangle.Bottom));
                            p.Y++;
                        }
                        if (fullSizeIndex >= 0)
                            p.X -= m_Container.GetSiteZOrderSize(m_Container.Owner.LeftDockSite, true).Width;
                        else if (partialSizeIndex >= 0)
                            p.X += m_Container.GetSiteZOrderSize(m_Container.Owner.LeftDockSite, false).Width;
                        r.Location = p;
                        break;
                    }
                case DockStyle.Bottom:
                    {
                        r.Width = m_Container.ClientRectangle.Width;
                        r.Height = bar.GetBarDockedSize(eOrientation.Horizontal);
                        if (fullSizeIndex >= 0)
                        {
                            r.Width += m_Container.GetSiteZOrderSize(m_Container.Owner.LeftDockSite, true).Width;
                            r.Width += m_Container.GetSiteZOrderSize(m_Container.Owner.RightDockSite, true).Width;
                            dockInfo.DockSiteZOrderIndex = fullSizeIndex;
                        }
                        else if (partialSizeIndex >= 0)
                        {
                            // Reduce by the size of the left and right dock site
                            r.Width -= m_Container.GetSiteZOrderSize(m_Container.Owner.LeftDockSite, false).Width;
                            r.Width -= m_Container.GetSiteZOrderSize(m_Container.Owner.RightDockSite, false).Width;
                            dockInfo.DockSiteZOrderIndex = partialSizeIndex;

                        }

                        Point p = Point.Empty;
                        if (dockInfo.DockLine == -1)
                        {
                            p = m_Container.PointToScreen(new Point(m_Container.ClientRectangle.X, m_Container.ClientRectangle.Y - r.Height));
                        }
                        else
                        {
                            p = m_Container.PointToScreen(new Point(m_Container.ClientRectangle.X, m_Container.ClientRectangle.Bottom - r.Height));
                            p.Y++;
                        }
                        if (fullSizeIndex >= 0)
                            p.X -= m_Container.GetSiteZOrderSize(m_Container.Owner.LeftDockSite, true).Width;
                        else if (partialSizeIndex >= 0)
                            p.X += m_Container.GetSiteZOrderSize(m_Container.Owner.LeftDockSite, false).Width;
                        r.Location = p;
                        break;
                    }
                case DockStyle.Right:
                    {
                        r.Height = m_Container.ClientRectangle.Height;

                        if (fullSizeIndex >= 0)
                        {
                            r.Height += m_Container.GetSiteZOrderSize(m_Container.Owner.TopDockSite, true).Height;
                            r.Height += m_Container.GetSiteZOrderSize(m_Container.Owner.BottomDockSite, true).Height;
                            dockInfo.DockSiteZOrderIndex = fullSizeIndex;
                        }
                        else if (partialSizeIndex >= 0)
                        {
                            // Reduce by the size of the top and bottom dock site
                            r.Height -= m_Container.GetSiteZOrderSize(m_Container.Owner.TopDockSite, false).Height;
                            r.Height -= m_Container.GetSiteZOrderSize(m_Container.Owner.BottomDockSite, false).Height;
                            dockInfo.DockSiteZOrderIndex = partialSizeIndex;

                        }

                        r.Width = bar.GetBarDockedSize(eOrientation.Vertical);
                        Point p = Point.Empty;
                        if (dockInfo.DockLine == -1)
                        {
                            p = m_Container.PointToScreen(new Point(m_Container.ClientRectangle.X - r.Width, m_Container.ClientRectangle.Y));
                        }
                        else
                        {
                            p = m_Container.PointToScreen(new Point(m_Container.ClientRectangle.Right - r.Width, m_Container.ClientRectangle.Y));
                            p.X--;
                        }
                        if (fullSizeIndex >= 0)
                            p.Y -= m_Container.GetSiteZOrderSize(m_Container.Owner.TopDockSite, true).Height;
                        else if (partialSizeIndex >= 0)
                            p.Y += m_Container.GetSiteZOrderSize(m_Container.Owner.TopDockSite, false).Height;
                        r.Location = p;
                        break;
                    }
                default:
                    {
                        r.Height = m_Container.ClientRectangle.Height;
                        if (fullSizeIndex >= 0)
                        {
                            r.Height += m_Container.GetSiteZOrderSize(m_Container.Owner.TopDockSite, true).Height;
                            r.Height += m_Container.GetSiteZOrderSize(m_Container.Owner.BottomDockSite, true).Height;
                            dockInfo.DockSiteZOrderIndex = fullSizeIndex;
                        }
                        else if (partialSizeIndex >= 0)
                        {
                            // Reduce by the size of the top and bottom dock site
                            r.Height -= m_Container.GetSiteZOrderSize(m_Container.Owner.TopDockSite, false).Height;
                            r.Height -= m_Container.GetSiteZOrderSize(m_Container.Owner.BottomDockSite, false).Height;
                            dockInfo.DockSiteZOrderIndex = partialSizeIndex;

                        }
                        r.Width = bar.GetBarDockedSize(eOrientation.Vertical);

                        Point p = Point.Empty;
                        if (dockInfo.DockLine == -1)
                        {
                            p = m_Container.PointToScreen(new Point(m_Container.ClientRectangle.X, m_Container.ClientRectangle.Y));
                        }
                        else
                        {
                            p = m_Container.PointToScreen(new Point(m_Container.ClientRectangle.Right, m_Container.ClientRectangle.Y));
                            p.X++;
                        }
                        if (fullSizeIndex >= 0)
                            p.Y -= m_Container.GetSiteZOrderSize(m_Container.Owner.TopDockSite, true).Height;
                        else if (partialSizeIndex >= 0)
                            p.Y += m_Container.GetSiteZOrderSize(m_Container.Owner.TopDockSite, false).Height;
                        r.Location = p;
                        break;
                    }
            }

            return r;
        }

		/// <summary>
		/// Returns DocumentBaseContainer that holds the reference bar.
		/// </summary>
		/// <param name="bar">Bar to return document container for.</param>
		/// <returns>Returns null if document container cannot  be found or reference to the document container.</returns>
		public DocumentBaseContainer GetDocumentFromBar(Bar bar)
		{
			if(bar==null)
				return null;
			return GetDocumentFromBar(bar,m_DocumentDockContainer);
		}

		private DocumentBaseContainer GetDocumentFromBar(Bar bar, DocumentDockContainer dockContainer)
		{
			foreach(DocumentBaseContainer doc in dockContainer.Documents)
			{
				if(doc is DocumentBarContainer && ((DocumentBarContainer)doc).Bar==bar)
					return doc;
				else if(doc is DocumentDockContainer)
				{
					DocumentBaseContainer d=GetDocumentFromBar(bar,doc as DocumentDockContainer);
					if(d!=null)
						return d;
				}
			}
			return null;
		}

        private bool IsAddedInFront(eDockSide dockSide)
        {
            if (m_Container.Dock == DockStyle.Left && dockSide == eDockSide.Left ||
                m_Container.Dock == DockStyle.Right && dockSide == eDockSide.Left ||
                m_Container.Dock == DockStyle.Top && dockSide== eDockSide.Top || 
                m_Container.Dock == DockStyle.Bottom && dockSide == eDockSide.Top)
                return true;

            return false;
        }

		/// <summary>
		/// Docks specified bar by appending it to the container. Bar will be added according to the container's orientation.
		/// </summary>
		/// <param name="barToDock">Bar to dock.</param>
		public void Dock(Bar barToDock)
		{
			Dock(null,barToDock,eDockSide.None);
		}

        /// <summary>
        /// Docks specified bar by appending it to the container. Bar will be added according to the container's orientation.
        /// </summary>
        /// <param name="barToDock">Bar to dock.</param>
        /// <param name="dockSide">Side to dock bar at.</param>
		public void Dock(Bar referenceBar, Bar barToDock, eDockSide dockSide)
		{
			if(dockSide==eDockSide.None && barToDock.Parent==m_Container)
			{
				ApplyBarStyle(barToDock);
				return;
			}

			if(barToDock.Parent is DockSite && ((DockSite)barToDock.Parent).DocumentDockContainer!=null)
				((DockSite)barToDock.Parent).GetDocumentUIManager().UnDock(barToDock);
			else if(barToDock.Parent is DockSite && barToDock.Parent!=m_Container)
				((DockSite)barToDock.Parent).RemoveBar(barToDock);
            else if (barToDock.Parent != null && barToDock.Parent != m_Container)
            {
                if (barToDock.Parent is FloatingContainer)
                {
                    barToDock.RemoveFromFloatingContainer();
                    barToDock.SetBarState(eBarState.Docked);
                }
                else
                    barToDock.Parent.Controls.Remove(barToDock);
            }

			if(!m_LoadingLayout)
			{
				// TODO: Add Docking as Document, i.e. add DockContainerItems from barToDock...
				DocumentBarContainer doc=this.CreateDocumentBarContainer(barToDock);
                DocumentBaseContainer referenceDoc = this.GetDocumentFromBar(referenceBar);

                if (referenceBar == null || dockSide == eDockSide.None || referenceBar == barToDock || referenceDoc == null)
				{
                    if (m_Container.Dock == DockStyle.Fill)
                        m_DocumentDockContainer.Documents.Add(doc);
                    else
                    {
                        eOrientation containerOrientation = eOrientation.Horizontal; // Needed container orientation
                        if (m_Container.Dock == DockStyle.Top || m_Container.Dock == DockStyle.Bottom)
                            containerOrientation = eOrientation.Vertical;

                        // Switch orientation when adding new bar if possible
                        if (m_DocumentDockContainer.Orientation != containerOrientation)
                        {
                            if (m_DocumentDockContainer.Documents.Count <= 1)
                            {
                                m_DocumentDockContainer.Orientation = containerOrientation;
                                if (IsAddedInFront(dockSide))
                                    m_DocumentDockContainer.Documents.Insert(0, doc);
                                else
                                    m_DocumentDockContainer.Documents.Add(doc);
                            }
                            else
                            {
                                DocumentBaseContainer[] docs = new DocumentBaseContainer[m_DocumentDockContainer.Documents.Count];
                                m_DocumentDockContainer.Documents.CopyTo(docs);
                                m_DocumentDockContainer.Documents.Clear();

                                DocumentDockContainer newParent = new DocumentDockContainer(docs, m_DocumentDockContainer.Orientation);
								newParent.SetLayoutBounds(m_DocumentDockContainer.DisplayBounds);
                                m_DocumentDockContainer.Orientation = containerOrientation;
                                m_DocumentDockContainer.Documents.Add(newParent);
                                if (IsAddedInFront(dockSide))
                                    m_DocumentDockContainer.Documents.Insert(0, doc);
                                else
                                    m_DocumentDockContainer.Documents.Add(doc);
                            }
                        }
                        else
                        {
                            if (IsAddedInFront(dockSide))
                                m_DocumentDockContainer.Documents.Insert(0, doc);
                            else
                                m_DocumentDockContainer.Documents.Add(doc);
                        }
                    }
				}
				else
				{
					DocumentDockContainer parent=referenceDoc.Parent as DocumentDockContainer;

					referenceDoc.SetLayoutBounds(Rectangle.Empty);
					doc.SetLayoutBounds(Rectangle.Empty);

					if((parent.Orientation==eOrientation.Horizontal && (dockSide==eDockSide.Left || dockSide==eDockSide.Right)) ||
						(parent.Orientation==eOrientation.Vertical && (dockSide==eDockSide.Top || dockSide==eDockSide.Bottom)))
					{
						if(dockSide==eDockSide.Right || dockSide==eDockSide.Bottom)
							parent.Documents.Insert(parent.Documents.IndexOf(referenceDoc)+1,doc);
						else
							parent.Documents.Insert(parent.Documents.IndexOf(referenceDoc),doc);
					}
					else if(parent.Documents.Count==1)
					{
						// Orientation of the parent dock container can be changed
						if(parent.Orientation==eOrientation.Vertical)
							parent.Orientation=eOrientation.Horizontal;
						else
							parent.Orientation=eOrientation.Vertical;

						if(dockSide==eDockSide.Right || dockSide==eDockSide.Bottom)
							parent.Documents.Insert(parent.Documents.IndexOf(referenceDoc)+1,doc);
						else
							parent.Documents.Insert(parent.Documents.IndexOf(referenceDoc),doc);
					}
					else
					{
						// New DocumentDockContainer needs to be inserted with appropriate orientation and referenceBar needs
						// to be moved into it.
						DocumentDockContainer newParent=new DocumentDockContainer();
						if(parent.Orientation==eOrientation.Horizontal)
							newParent.Orientation=eOrientation.Vertical;
						else
							newParent.Orientation=eOrientation.Horizontal;

						parent.Documents.Insert(parent.Documents.IndexOf(referenceDoc),newParent);
						parent.Documents.Remove(referenceDoc);
	                    
						if(dockSide==eDockSide.Right || dockSide==eDockSide.Bottom)
						{
							newParent.Documents.Add(referenceDoc);
							newParent.Documents.Add(doc);
						}
						else
						{
							newParent.Documents.Add(doc);
							newParent.Documents.Add(referenceDoc);
						}
					}
				}

                AdjustContainerSize(barToDock, false);
			}

			if(m_Container!=null)
			{
				if(barToDock.Parent==null)
					m_Container.Controls.Add(barToDock);
                ApplyBarStyle(barToDock);	
				m_Container.RecalcLayout();
			}
			else
				ApplyBarStyle(barToDock);
		}

        /// <summary>
        /// Adjusts the size of the dock container if needed after a bar has been docked to it.
        /// </summary>
        /// <param name="barToDock">Bar object that has been docked.</param>
        /// <param name="visibleChanged">Indicates that bar was already docked but its Visible property has changed</param>
        internal void AdjustContainerSize(Bar barToDock, bool visibleChanged)
        {
            if (m_Container.Owner != null && m_Container.Owner.IsLoadingLayout) return;

            // Adjust the size of the container since it dictates the size of the bars docked inside
            if (m_Container.Dock != DockStyle.Fill)
            {
                DocumentBaseContainer doc = this.GetDocumentFromBar(barToDock);
                if (doc == null) return;
                if (m_Container.Dock == DockStyle.Left || m_Container.Dock == DockStyle.Right)
                {
                    if (m_Container.Width == 0 || doc.Parent == m_DocumentDockContainer &&
                        (m_DocumentDockContainer.Orientation == eOrientation.Horizontal || visibleChanged && m_DocumentDockContainer.Documents.Count == 1))
                    {
                        int width = barToDock.GetBarDockedSize(eOrientation.Vertical);
                        if (barToDock.Width > 0 && barToDock.Width<m_Container.Width) width = barToDock.Width;
                        int clientWidth = GetClientWidth();

                        if (barToDock.Visible)
                        {
                            if (width > clientWidth)
                                width = barToDock.MinimumDockSize(eOrientation.Vertical).Width * 2;
                            if (width > clientWidth)
                                width = barToDock.MinimumDockSize(eOrientation.Vertical).Width;
                        }
                        
						if(doc.LayoutBounds.Width == 0)
							doc.SetLayoutBounds(new Rectangle(doc.LayoutBounds.X, doc.LayoutBounds.Y, width, doc.LayoutBounds.Height));
                        width += m_DocumentDockContainer.SplitterSize;
                        if (visibleChanged)
                        {
                            if(barToDock.Visible)
                                m_Container.Width += width;
                            else if(m_Container.Width>0)
                                m_Container.Width -= Math.Min(width, m_Container.Width);
                        }
                        else
                            m_Container.Width += width;
                        m_Container.Invalidate();
                    }
                }
                else if (m_Container.Dock == DockStyle.Top || m_Container.Dock == DockStyle.Bottom)
                {
                    if (m_Container.Height == 0 || doc.Parent == m_DocumentDockContainer &&
                        (m_DocumentDockContainer.Orientation == eOrientation.Vertical || visibleChanged && m_DocumentDockContainer.Documents.Count == 1))
                    {
                        int height = barToDock.GetBarDockedSize(eOrientation.Horizontal);
                        if (barToDock.Height > 0 && barToDock.Height<m_Container.Height) height = barToDock.Height;
                        int clientHeight = GetClientHeight();

                        if (barToDock.Visible)
                        {
                            if (height > clientHeight)
                                height = barToDock.MinimumDockSize(eOrientation.Horizontal).Height * 2;
                            if (height > clientHeight)
                                height = barToDock.MinimumDockSize(eOrientation.Horizontal).Height;
                        }
                        
						if(doc.LayoutBounds.Height == 0)
							doc.SetLayoutBounds(new Rectangle(doc.LayoutBounds.X, doc.LayoutBounds.Y, doc.LayoutBounds.Width, height));
                        height += m_DocumentDockContainer.SplitterSize;
                        if (visibleChanged)
                        {
                            if (barToDock.Visible)
                                m_Container.Height += height;
                            else if (m_Container.Height > 0)
                                m_Container.Height -= Math.Min(height, m_Container.Height);
                        }
                        else
                            m_Container.Height += height;
                        m_Container.Invalidate();
                    }
                }
            }
        }

        private int GetClientWidth()
        {
            if (m_Container == null)
                return 0;

            return m_Container.GetClientWidth();
        }

        private int GetClientHeight()
        {
            if (m_Container == null)
                return 0;
            return m_Container.GetClientHeight();
        }

		public void UnDock(Bar barToUnDock)
		{
			UnDock(barToUnDock,true);
		}

        internal void SaveLastDockInfo(Bar bar, DocumentBarContainer doc, DockSiteInfo info)
        {
            if (bar.m_LastDockSiteInfo == null) return;
            //bar.m_LastDockSiteInfo.LastRelativeDockToBar = null;
            //bar.m_LastDockSiteInfo.LastRelativeDocumentId = -1;
            // Tries to find relative dock-to-bar so un-docked bar can be returned back
            if (!(doc.Parent is DocumentDockContainer)) return;
            DocumentDockContainer dockParent = (DocumentDockContainer)doc.Parent;
            DocumentBaseContainer nextBarDocument = GetNextDocument(dockParent, dockParent.Documents.IndexOf(doc));
            if (nextBarDocument == null)
            {
                DocumentBaseContainer prevBarDocument = GetPreviousDocument(dockParent, dockParent.Documents.IndexOf(doc));
                if (prevBarDocument != null)
                {
                    Bar prevBar = null;
                    if (prevBarDocument is DocumentBarContainer)
                    {
                        prevBar = ((DocumentBarContainer)prevBarDocument).Bar;
                        bar.m_LastDockSiteInfo.LastRelativeDockToBar = prevBar;
                        prevBar.m_LastDockSiteInfo.LastRelativeDockToBar = bar;
                        prevBar.m_LastDockSiteInfo.LastRelativeDocumentId = doc.Id;
                    }
                    bar.m_LastDockSiteInfo.LastRelativeDocumentId = prevBarDocument.Id;
                    if (dockParent.Orientation == eOrientation.Horizontal)
                    {
                        bar.LastDockSide = eDockSide.Right;
                        if (prevBar != null) prevBar.LastDockSide = eDockSide.Left;
                    }
                    else
                    {
                        bar.LastDockSide = eDockSide.Bottom;
                        if (prevBar != null) prevBar.LastDockSide = eDockSide.Top;
                    }
                }
            }
            else
            {
                Bar nextBar = null;
                if (nextBarDocument is DocumentBarContainer)
                {
                    nextBar = ((DocumentBarContainer)nextBarDocument).Bar;
                    bar.m_LastDockSiteInfo.LastRelativeDockToBar = nextBar;
                    nextBar.m_LastDockSiteInfo.LastRelativeDockToBar = bar;
                    nextBar.m_LastDockSiteInfo.LastRelativeDocumentId = doc.Id;
                }
                bar.m_LastDockSiteInfo.LastRelativeDocumentId = nextBarDocument.Id;
                if (dockParent.Orientation == eOrientation.Horizontal)
                {
                    bar.LastDockSide = eDockSide.Left;
                    if (nextBar != null) nextBar.LastDockSide = eDockSide.Right;
                }
                else
                {
                    bar.LastDockSide = eDockSide.Top;
                    if (nextBar != null) nextBar.LastDockSide = eDockSide.Bottom;
                }
            }
            if (GetDockSide(bar) != eDockSide.None)
                bar.m_LastDockSiteInfo.LastDockSiteSide = GetDockSide(bar);
        }

        private eDockSide GetDockSide(Bar bar)
        {
            if (bar.Parent == null)
                return eDockSide.None;
            else if (bar.Parent.Dock == DockStyle.Left)
                return eDockSide.Left;
            else if (bar.Parent.Dock == DockStyle.Right)
                return eDockSide.Right;
            else if (bar.Parent.Dock == DockStyle.Top)
                return eDockSide.Top;
            else if (bar.Parent.Dock == DockStyle.Bottom)
                return eDockSide.Bottom;
            else if (bar.Parent.Dock == DockStyle.Fill)
                return eDockSide.Document;
            return eDockSide.None;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
		public void UnDock(Bar barToUnDock, bool removeFromContainer)
		{
			DocumentBarContainer doc=m_DocumentDockContainer.GetBarDocumentContainer(barToUnDock);

            if (barToUnDock.DockSide != eDockSide.None) barToUnDock.LastDockSide = barToUnDock.DockSide;
            if (doc != null)
                SaveLastDockInfo(barToUnDock, doc, barToUnDock.m_LastDockSiteInfo);
            
            int containerSizeReduction = 0;
            // Calculate the size reduction of container if needed
            if (doc != null && m_Container != null && m_Container.Dock != DockStyle.Fill)
            {
                if (m_Container.Dock == DockStyle.Left || m_Container.Dock == DockStyle.Right)
                {
                    if (doc.Parent != null && doc.Parent == m_DocumentDockContainer && m_DocumentDockContainer.Orientation == eOrientation.Horizontal)
                        containerSizeReduction = barToUnDock.Width + m_DocumentDockContainer.SplitterSize;
                }
                else if (m_Container.Dock == DockStyle.Top || m_Container.Dock == DockStyle.Bottom)
                {
                    if (doc.Parent != null && doc.Parent == m_DocumentDockContainer && m_DocumentDockContainer.Orientation == eOrientation.Vertical)
                        containerSizeReduction = barToUnDock.Height + m_DocumentDockContainer.SplitterSize;
                }
            }

            if(doc==null)
            {
                if (removeFromContainer && barToUnDock.Parent == m_Container)
                    m_Container.Controls.Remove(barToUnDock);
				RestoreBarStyle(barToUnDock);
                return;
            }
			
			DocumentDockContainer parent=doc.Parent as DocumentDockContainer;
			if(parent!=null)
			{
				parent.Documents.Remove(doc);
				if(parent!=m_DocumentDockContainer)
					RemoveEmptyContainers(parent);
				else if(parent.Documents.Count==1 && parent.Documents[0] is DocumentDockContainer)
					RemoveEmptyContainers(parent.Documents[0] as DocumentDockContainer);
			}

			if(m_Container!=null)
			{
				if(removeFromContainer)
					m_Container.Controls.Remove(barToUnDock);
				RestoreBarStyle(barToUnDock);
                bool isLoadingLayout = false;
                if (m_Container.Owner != null) isLoadingLayout = m_Container.Owner.IsLoadingLayout;
                if (!isLoadingLayout && !m_Container.IsAnyControlVisible)
                {
                    if (m_Container.Dock == DockStyle.Left || m_Container.Dock == DockStyle.Right)
                        m_Container.Width = 0;
                    else if (m_Container.Dock == DockStyle.Top || m_Container.Dock == DockStyle.Bottom)
                        m_Container.Height = 0;
                }
                else if (!isLoadingLayout && containerSizeReduction != 0)
                {
                    if (m_Container.Dock == DockStyle.Left || m_Container.Dock == DockStyle.Right)
                        m_Container.Width -= containerSizeReduction;
                    else if (m_Container.Dock == DockStyle.Top || m_Container.Dock == DockStyle.Bottom)
                        m_Container.Height -= containerSizeReduction;
                }
                else
                    m_Container.RecalcLayout();
			}
			else
				RestoreBarStyle(barToUnDock);
		}

		private void RemoveEmptyContainers(DocumentDockContainer parent)
		{
			while(parent.Documents.Count==0 && parent.Parent!=null && parent.Parent is DocumentDockContainer ||
				parent.Parent!=null && parent.Parent is DocumentDockContainer && ((DocumentDockContainer)parent.Parent).Documents.Count==1)
			{
				if(parent.Parent!=null && parent.Parent is DocumentDockContainer && ((DocumentDockContainer)parent.Parent).Documents.Count==1)
				{
					// Removes the instances where DocumentDockContainer hosts another one and only one DocumentDockContainer object
					DocumentDockContainer p=parent.Parent as DocumentDockContainer;
					p.Documents.Remove(parent);
					p.Orientation=parent.Orientation;
					DocumentBaseContainer[] documents=new DocumentBaseContainer[parent.Documents.Count];
					parent.Documents.CopyTo(documents);
					p.Documents.AddRange(documents);
					parent=p;
				}
				else
				{
					DocumentDockContainer p=parent.Parent as DocumentDockContainer;
					p.Documents.Remove(parent);
					parent=p;
				}
			}
		}

		private void ApplyBarStyle(Bar bar)
		{
			if(m_Container!=null && m_Container.Owner!=null && m_Container.Owner.ApplyDocumentBarStyle && m_Container.Dock == DockStyle.Fill)
			{
				BarFunctions.ApplyAutoDocumentBarStyle(bar);
			}
		}

		private void RestoreBarStyle(Bar bar)
		{
            if (m_Container != null && m_Container.Owner != null && m_Container.Owner.ApplyDocumentBarStyle && m_Container.Dock == DockStyle.Fill)
			{
				BarFunctions.RestoreAutoDocumentBarStyle(bar);
			}
		}

		private DocumentBarContainer CreateDocumentBarContainer(Bar bar)
		{
			DocumentBarContainer doc=new DocumentBarContainer(bar);
			return doc;
		}

		private DocumentDockContainer CreateDocumentDockContainer()
		{
			DocumentDockContainer dock=new DocumentDockContainer();
			return dock;
		}

		#endregion

		#region Mouse Resize Handling
        /// <summary>
        /// Gets whether document/bar is being resized.
        /// </summary>
        public bool IsResizingDocument
        {
            get
            {
                return (m_ResizeDocument != null);
            }
        }
		/// <summary>
		/// Processes OnMouseMove events from DockSite. This routine assumes that mouse messages are received only when mouse is actualy over
		/// the dock site and not containing bars. This is significant becouse it will receive messages only if mouse is over the splitter
		/// space between bars.
		/// </summary>
		/// <param name="e">Mouse event arguments.</param>
		public void OnMouseMove(MouseEventArgs e)
		{
			int x=e.X,y=e.Y;
			if(e.Button==MouseButtons.None)
			{
				Cursor c=this.GetCursor(x,y);
				if(c==null)
				{
					if(m_ContainerCursor!=null)
						this.Container.Cursor=m_ContainerCursor;
					m_ContainerCursor=null;
				}
				else
				{
					if(m_ContainerCursor==null)
						m_ContainerCursor=this.Container.Cursor;
					this.Container.Cursor=c;
				}
			}
			else if(e.Button==MouseButtons.Left)
			{
				Point p=GetResizeToPoint(x,y);
				if(p.IsEmpty)
					return;

				if(!m_ResizePreview)
					SplitterMouseMove(p.X,p.Y);
				else
				{
					ResizeTo(p.X,p.Y);
					m_MouseDownPoint=p;
				}
			}
		}
        [EditorBrowsable(EditorBrowsableState.Never)]
		public Cursor GetCursor(int x, int y)
		{
			Cursor c=null;
			DocumentDockContainer context=this.GetContainerFromRoot(x,y);
			if(context!=null)
			{
				if(context.Orientation==eOrientation.Horizontal)
					c=Cursors.VSplit;
				else if(context.Orientation==eOrientation.Vertical)
					c=Cursors.HSplit;
			}
            else if (m_Container != null && m_Container.Dock != DockStyle.Fill)
            {
                if(m_Container.Dock == DockStyle.Top || m_Container.Dock == DockStyle.Bottom)
                    c = Cursors.HSplit;
                else if (m_Container.Dock == DockStyle.Left || m_Container.Dock == DockStyle.Right)
                    c = Cursors.VSplit;
            }

			return c;
		}

		public void OnMouseDown(MouseEventArgs e)
		{
			int x=e.X, y=e.Y;
			if(e.Button==MouseButtons.Left)
			{
				m_MouseDownPoint=new Point(x,y);
				DocumentDockContainer context=this.GetContainerFromRoot(x,y);
                
                if(context!=null)
				{
					m_ResizeDocument=this.GetClosestDocument(context,x,y);
					if(m_ResizeDocument==null)
						return;
					m_ResizeDockContainer=context;
					if(!m_ResizePreview)
					{
						CreateSplitter();
						SplitterMouseMove(x,y);
					}
				}
                else if (m_Container != null && m_Container.Dock != DockStyle.Fill)
                {
                    m_ResizeDockContainer = m_DocumentDockContainer;
                    if (!m_ResizePreview)
                    {
                        CreateSplitter();
                        SplitterMouseMove(x, y);
                    }
                }
			}
		}

		public void OnMouseUp(MouseEventArgs e)
		{
			if(m_ResizeDockContainer!=null && !m_MouseDownPoint.IsEmpty)
			{
				ResizeTo(e.X,e.Y);
			}

			m_MouseDownPoint=Point.Empty;
			m_ResizeDockContainer=null;
			m_ResizeDocument=null;
			DestroySplitter();
		}

		public void OnMouseLeave()
		{
			if(m_ContainerCursor!=null)
				this.Container.Cursor=m_ContainerCursor;
			m_ContainerCursor=null;
		}

		private DocumentBaseContainer GetClosestDocument(DocumentDockContainer parent, int x, int y)
		{
			DocumentBaseContainer ret=null;
			eOrientation orientation=parent.Orientation;
			foreach(DocumentBaseContainer doc in parent.Documents)
			{
				if(orientation==eOrientation.Horizontal && doc.DisplayBounds.X>x && doc.Visible)
                    break;
                else if(orientation==eOrientation.Vertical && doc.DisplayBounds.Y>y && doc.Visible)
					break;
                if(doc.Visible)
				    ret=doc;
			}
			return ret;
		}

		/// <summary>
		/// Returns reference to the DocumentDockContainer that contains specified coordinates. Searches through the Documents collection first.
		/// </summary>
		/// <param name="x">X coordinate</param>
		/// <param name="y">Y coordinate</param>
		/// <returns></returns>
		private DocumentDockContainer GetContainerFromRoot(int x, int y)
		{
			return GetContainer(m_DocumentDockContainer,x,y);
		}

		private DocumentDockContainer GetContainer(DocumentDockContainer parent, int x, int y)
		{
			if(parent==null || !parent.DisplayBounds.Contains(x,y) || !parent.Visible)
				return null;
			DocumentDockContainer ret=null;
			foreach(DocumentBaseContainer doc in parent.Documents)
			{
				if(doc is DocumentDockContainer)
				{
					ret=this.GetContainer((DocumentDockContainer)doc,x,y);
					if(ret!=null)
						break;
				}
			}

            if (ret == null)
                ret = parent;
			return ret;
		}
		
		private void CreateSplitter()
		{
            if (m_ResizeDockContainer == null)
				return;

			if(m_Splitter==null)
				m_Splitter=BarFunctions.CreateOutlineForm();
		}

		private void SplitterMouseMove(int x, int y)
		{
			if(m_Splitter==null || m_ResizeDockContainer==null)
				return;
			
            Rectangle splitterBounds=Rectangle.Empty;

			// Set splitter size
            if (m_ResizeDockContainer.Orientation == eOrientation.Horizontal && m_ResizeDocument != null || 
                m_ResizeDocument == null && (m_Container.Dock == DockStyle.Left || m_Container.Dock == DockStyle.Right))
			{
				Point p=m_Container.PointToScreen(new Point(x-m_SplitterWidth/2,m_ResizeDockContainer.DisplayBounds.Y));
				splitterBounds.X=p.X;
				splitterBounds.Y=p.Y;
				splitterBounds.Height=m_ResizeDockContainer.DisplayBounds.Height;
				splitterBounds.Width=m_SplitterWidth;
			}
            else if (m_ResizeDockContainer.Orientation == eOrientation.Vertical && m_ResizeDocument != null || 
                m_ResizeDocument == null && (m_Container.Dock == DockStyle.Bottom || m_Container.Dock == DockStyle.Top))
			{
				Point p=m_Container.PointToScreen(new Point(m_ResizeDockContainer.DisplayBounds.X,y-m_SplitterWidth/2));
				splitterBounds.X=p.X;
				splitterBounds.Y=p.Y;
				splitterBounds.Width=m_ResizeDockContainer.DisplayBounds.Width;
				splitterBounds.Height=m_SplitterWidth;
			}

			NativeFunctions.SetWindowPos(m_Splitter.Handle,NativeFunctions.HWND_TOP,splitterBounds.X,splitterBounds.Y,splitterBounds.Width,splitterBounds.Height,NativeFunctions.SWP_SHOWWINDOW | NativeFunctions.SWP_NOACTIVATE);
		}

		private void DestroySplitter()
		{
			if(m_Splitter!=null)
			{
				m_Splitter.Visible=false;
				m_Splitter.Dispose();
				m_Splitter=null;
			}
		}

        internal Size GetContainerMinimumSize(DocumentDockContainer dockContainer)
        {
            Size minimumSize = Size.Empty;

            foreach (DocumentBaseContainer d in dockContainer.Documents)
            {
                Size dm = Size.Empty;

                if (d is DocumentBarContainer)
                {
                    if (!((DocumentBarContainer)d).Visible) continue;
                    dm = d.MinimumSize;
                }
                else if (d is DocumentDockContainer)
                {
                    dm = GetContainerMinimumSize(d as DocumentDockContainer);
                }

                if (dockContainer.Orientation == eOrientation.Horizontal)
                {
                    minimumSize.Width += dm.Width + dockContainer.SplitterSize;
                    if (dm.Height < minimumSize.Height || minimumSize.Height == 0)
                        minimumSize.Height = dm.Height;
                }
                else
                {
                    minimumSize.Height += dm.Height + dockContainer.SplitterSize;
                    if (dm.Width < minimumSize.Width || minimumSize.Width == 0)
                        minimumSize.Width = dm.Width;
                }
            }

            if (minimumSize.Width > 0 && dockContainer.Orientation == eOrientation.Horizontal)
                minimumSize.Width += dockContainer.SplitterSize;
            else if (minimumSize.Height > 0 && dockContainer.Orientation == eOrientation.Vertical)
                minimumSize.Height += dockContainer.SplitterSize;

            if (minimumSize.Width == 0)
                minimumSize.Width = 32;
            if (minimumSize.Height == 0)
                minimumSize.Height = 32;

            return minimumSize;
        }

		private Point GetResizeToPoint(int x, int y)
		{
			if(m_ResizeDockContainer==null)
				return Point.Empty;

			Point resizeTo=new Point(x,y);

			int xDiff=x-m_MouseDownPoint.X;
			int yDiff=y-m_MouseDownPoint.Y;

            if (m_ResizeDocument == null)
            {
                // Resizing the dock site
                if (m_Container.Dock == DockStyle.Left || m_Container.Dock == DockStyle.Right)
                {
                    if (m_Container.Dock == DockStyle.Right) xDiff *= -1;

                    Size minimumSize = GetContainerMinimumSize(m_ResizeDockContainer);
                    if (m_ResizeDockContainer.DisplayBounds.Width + xDiff < minimumSize.Width)
                        resizeTo.X -= ((m_ResizeDockContainer.DisplayBounds.Width + xDiff) - minimumSize.Width) * ((m_Container.Dock == DockStyle.Right ? -1 : 1));
                    else
                    {
                        int clientWidth = GetClientWidth();
                        int minClientWidth = 32;
                        if (m_Container.Owner != null)
                            minClientWidth = m_Container.Owner.MinimumClientSize.Width;

                        if (clientWidth - xDiff < minClientWidth)
                            resizeTo.X -= (minClientWidth - (clientWidth - xDiff)) * ((m_Container.Dock == DockStyle.Right ? -1 : 1));
                    }

                    return resizeTo;
                }
                else if (m_Container.Dock == DockStyle.Top || m_Container.Dock == DockStyle.Bottom)
                {
                    if (m_Container.Dock == DockStyle.Bottom) yDiff *= -1;

                    Size minimumSize = GetContainerMinimumSize(m_ResizeDockContainer);
                    if (m_ResizeDockContainer.DisplayBounds.Height + yDiff < minimumSize.Height)
                        resizeTo.Y -= ((m_ResizeDockContainer.DisplayBounds.Height + yDiff) - minimumSize.Height) * ((m_Container.Dock == DockStyle.Bottom ? -1 : 1));
                    else
                    {
                        int clientHeight = GetClientHeight();
                        int minClientHeight = 32;
                        if (m_Container.Owner != null)
                            minClientHeight = m_Container.Owner.MinimumClientSize.Height;
                        
                        if (clientHeight - yDiff < minClientHeight)
                            resizeTo.Y -= (minClientHeight - (clientHeight - yDiff)) * ((m_Container.Dock == DockStyle.Bottom ? -1 : 1));
                    }

                    return resizeTo;
                }

                return Point.Empty;
            }
			else if(m_ResizeDockContainer.Orientation==eOrientation.Horizontal)
			{
				if(m_ResizeDocument.DisplayBounds.Width+xDiff>=m_ResizeDocument.MinimumSize.Width)
				{
					// Verify document next to it
					DocumentBaseContainer nextDoc=GetNextDocument(m_ResizeDockContainer,m_ResizeDockContainer.Documents.IndexOf(m_ResizeDocument));
					if(nextDoc!=null)
					{
						if(nextDoc.DisplayBounds.Width-xDiff<nextDoc.MinimumSize.Width)
							resizeTo.X+=(nextDoc.DisplayBounds.Width-xDiff-nextDoc.MinimumSize.Width);
					}
				}
				else
					resizeTo.X-=(m_ResizeDocument.DisplayBounds.Width+xDiff-m_ResizeDocument.MinimumSize.Width);
			}
			else if(m_ResizeDockContainer.Orientation==eOrientation.Vertical)
			{
				if(m_ResizeDocument.DisplayBounds.Height+yDiff>=m_ResizeDocument.MinimumSize.Height)
				{
					DocumentBaseContainer nextDoc=GetNextDocument(m_ResizeDockContainer,m_ResizeDockContainer.Documents.IndexOf(m_ResizeDocument));
					if(nextDoc!=null)
					{
						if(nextDoc.DisplayBounds.Height-yDiff<nextDoc.MinimumSize.Height)
							resizeTo.Y+=(nextDoc.DisplayBounds.Height-yDiff-nextDoc.MinimumSize.Height);
					}
				}
				else
					resizeTo.Y-=(m_ResizeDocument.DisplayBounds.Height+yDiff-m_ResizeDocument.MinimumSize.Height);
			}

			return resizeTo;
		}

		private void ResetLayoutBounds(DocumentDockContainer doc)
		{
			foreach(DocumentBaseContainer d in doc.Documents)
				d.SetLayoutBounds(Rectangle.Empty);
		}

		private void ResizeTo(int x, int y)
		{
			if(m_ResizeDockContainer==null)
				return;

			Point resizeTo=GetResizeToPoint(x,y);
			
            int xDiff=resizeTo.X-m_MouseDownPoint.X;
			int yDiff=resizeTo.Y-m_MouseDownPoint.Y;

            if (m_ResizeDocument == null)
            {
                m_Container.SuspendLayout();
                if (m_Container.Dock == DockStyle.Left && xDiff!=0)
                {
                    if (m_DocumentDockContainer.Documents.Count > 0)
                    {
                        DocumentBaseContainer lastDoc = m_DocumentDockContainer.Documents[m_DocumentDockContainer.Documents.Count - 1];
                        int docWidth = lastDoc.DisplayBounds.Width + xDiff;
                        if (docWidth > lastDoc.MinimumSize.Width)
                            lastDoc.SetLayoutBounds(new Rectangle(lastDoc.DisplayBounds.X, lastDoc.DisplayBounds.Y, docWidth, lastDoc.DisplayBounds.Height));
						else
							ResetLayoutBounds(m_DocumentDockContainer);
                    }

                    m_Container.Width = m_Container.Width + xDiff;
					if(m_Container.Width <= this.GetContainerMinimumSize(m_DocumentDockContainer).Width)
						ResetLayoutBounds(m_DocumentDockContainer);
                }
                else if (m_Container.Dock == DockStyle.Right && xDiff != 0)
                {
                    if (m_DocumentDockContainer.Documents.Count > 0)
                    {
                        DocumentBaseContainer lastDoc = m_DocumentDockContainer.Documents[0];
                        int docWidth = lastDoc.DisplayBounds.Width - xDiff;
                        if (docWidth > lastDoc.MinimumSize.Width)
                            lastDoc.SetLayoutBounds(new Rectangle(lastDoc.DisplayBounds.X, lastDoc.DisplayBounds.Y, docWidth, lastDoc.DisplayBounds.Height));
                        else
                            ResetLayoutBounds(m_DocumentDockContainer);
                    }

                    m_Container.Width = m_Container.Width - xDiff;
					if(m_Container.Width <= this.GetContainerMinimumSize(m_DocumentDockContainer).Width)
						ResetLayoutBounds(m_DocumentDockContainer);
                }
                else if (m_Container.Dock == DockStyle.Top && yDiff != 0)
                {
                    if (m_DocumentDockContainer.Documents.Count > 0)
                    {
                        DocumentBaseContainer lastDoc = m_DocumentDockContainer.Documents[m_DocumentDockContainer.Documents.Count - 1];
                        int docHeight = lastDoc.DisplayBounds.Height + yDiff;
                        if (docHeight > lastDoc.MinimumSize.Height)
                            lastDoc.SetLayoutBounds(new Rectangle(lastDoc.DisplayBounds.X, lastDoc.DisplayBounds.Y, lastDoc.DisplayBounds.Width, docHeight));
                        else
                            ResetLayoutBounds(m_DocumentDockContainer);
                    }

                    m_Container.Height = m_Container.Height + yDiff;
					if(m_Container.Height <= this.GetContainerMinimumSize(m_DocumentDockContainer).Height)
						ResetLayoutBounds(m_DocumentDockContainer);
                }
                else if (m_Container.Dock == DockStyle.Bottom && yDiff != 0)
                {
                    if (m_DocumentDockContainer.Documents.Count > 0)
                    {
                        DocumentBaseContainer lastDoc = m_DocumentDockContainer.Documents[0];
                        int docHeight = lastDoc.DisplayBounds.Height - yDiff;
                        if (docHeight > lastDoc.MinimumSize.Height)
                            lastDoc.SetLayoutBounds(new Rectangle(lastDoc.DisplayBounds.X, lastDoc.DisplayBounds.Y, lastDoc.DisplayBounds.Width, docHeight));
                        else
                            ResetLayoutBounds(m_DocumentDockContainer);
                    }

                    m_Container.Height = m_Container.Height - yDiff;
					if(m_Container.Height <= this.GetContainerMinimumSize(m_DocumentDockContainer).Height)
						ResetLayoutBounds(m_DocumentDockContainer);
                }
                m_Container.ResumeLayout();
            }
			else if(m_ResizeDockContainer.Orientation==eOrientation.Horizontal && xDiff!=0)
			{
				m_ResizeDocument.SetWidth(m_ResizeDocument.DisplayBounds.Width+xDiff);
			}
            else if (m_ResizeDockContainer.Orientation == eOrientation.Vertical && yDiff != 0)
			{
				m_ResizeDocument.SetHeight(m_ResizeDocument.DisplayBounds.Height+yDiff);
			}

			m_Container.RecalcLayout();

            if (m_Container.Owner != null)
                ((IOwner)m_Container.Owner).InvokeUserCustomize(m_ResizeDocument, EventArgs.Empty);
		}

		private DocumentBaseContainer GetNextDocument(DocumentDockContainer parent, int refIndex)
		{
			int count=parent.Documents.Count;
			for(int i=refIndex+1;i<count;i++)
			{
				if(parent.Documents[i].Visible)
					return parent.Documents[i];
			}
			return null;
		}
        private DocumentBaseContainer GetPreviousDocument(DocumentDockContainer parent, int refIndex)
        {
            int count = parent.Documents.Count;
            for (int i = refIndex - 1; i >= 0; i--)
            {
                if (parent.Documents[i].Visible)
                    return parent.Documents[i];
            }
            return null;
        }

		/// <summary>
		/// Sets the width of the bar that is managed by this document layout. Width can be set only if parent container has
		/// Horizontal orientation. Note that bar minimum size is respected by this method and
		/// it will be enforced. If width is less than minimum width bar's width will be set to minimum width.
		/// </summary>
		/// <param name="bar">Reference to bar object.</param>
		/// <param name="width">Desired width.</param>
		public void SetBarWidth(Bar bar, int width)
		{
			DocumentBarContainer docBar=GetDocumentFromBar(bar) as DocumentBarContainer;
			if(docBar==null)
				return;
			if(docBar.Parent == m_DocumentDockContainer && (m_Container.Dock == DockStyle.Left || m_Container.Dock == DockStyle.Right)
				/*&& (m_DocumentDockContainer.Orientation == eOrientation.Vertical || m_DocumentDockContainer.Documents.Count == 1)*/)
			{
				m_Container.SuspendLayout();
				int diff = width;
				if(docBar.DisplayBounds.Width == 0)
					diff -= bar.GetBarDockedSize(eOrientation.Vertical);
				else
					diff -= docBar.DisplayBounds.Width;
                if (diff != 0)
                {
                    m_Container.Width += diff;
                    docBar.SetWidth(width);
                }
				//((DocumentDockContainer)docBar.Parent).SetWidth(width);
				m_Container.ResumeLayout(true);
			}
            else if (docBar.Parent is DocumentDockContainer && ((DocumentDockContainer)docBar.Parent).Orientation == eOrientation.Vertical)
            {
                ((DocumentDockContainer)docBar.Parent).SetWidth(width);
            }
            else
            {
                if (width < docBar.MinimumSize.Width)
                    width = docBar.MinimumSize.Width;
                docBar.SetWidth(width);
            }
			m_Container.RecalcLayout();
		}

		/// <summary>
		/// Sets the height of the bar that is managed by this document layout. Height can be set only if parent container has
		/// vertical orientation. Note that bar minimum size is respected by this method and
		/// it will be enforced. If height is less than minimum height bar's height will be set to minimum height.
		/// </summary>
		/// <param name="bar">Reference to bar object.</param>
		/// <param name="height">Desired height.</param>
		public void SetBarHeight(Bar bar, int height)
		{
			DocumentBarContainer docBar=GetDocumentFromBar(bar) as DocumentBarContainer;
			if(docBar==null)
				return;

            if (m_Container!=null && (m_Container.Dock == DockStyle.Top || m_Container.Dock== DockStyle.Bottom) && 
                docBar.Parent == m_DocumentDockContainer
                /*&& (m_DocumentDockContainer.Orientation == eOrientation.Horizontal || m_DocumentDockContainer.Documents.Count == 1)*/)
            {
                //m_Container.Height += (height - bar.Height);
                //m_DocumentDockContainer.SetHeight(height);
                m_Container.SuspendLayout();
                int diff = height;
                if (docBar.DisplayBounds.Height == 0)
                    diff -= bar.GetBarDockedSize(eOrientation.Vertical);
                else
                    diff -= docBar.DisplayBounds.Height;
                m_Container.Height += diff;
                //((DocumentDockContainer)docBar.Parent).SetHeight(height);
                docBar.SetHeight(height);
                m_Container.ResumeLayout(true);
            }
            else if (docBar.Parent is DocumentDockContainer && ((DocumentDockContainer)docBar.Parent).Orientation == eOrientation.Horizontal)
            {
                ((DocumentDockContainer)docBar.Parent).SetHeight(height);
            }
            else
            {
                if (height < docBar.MinimumSize.Height)
                    height = docBar.MinimumSize.Height;
                docBar.SetHeight(height);
            }
			m_Container.RecalcLayout();
		}
		#endregion

		#region Serialization
		internal void SerializeDefinition(XmlElement parent)
		{
            XmlElement docs = null;
            if (m_Container.Dock == DockStyle.Fill)
                docs = parent.OwnerDocument.CreateElement(DocumentSerializationXml.Documents);
            else
            {
                docs = parent.OwnerDocument.CreateElement(DocumentSerializationXml.DockSite);
                docs.SetAttribute(DocumentSerializationXml.DockSiteSize, XmlConvert.ToString((m_Container.Dock == DockStyle.Left || m_Container.Dock == DockStyle.Right) ? m_Container.Width : m_Container.Height));
                docs.SetAttribute(DocumentSerializationXml.DockingSide, Enum.GetName(typeof(DockStyle), m_Container.Dock));
            }

			parent.AppendChild(docs);

			SerializeDocumentDockContainer(docs,m_DocumentDockContainer, true);
		}

		internal void SerializeLayout(XmlElement parent)
		{
            XmlElement docs = null;
            if (m_Container.Dock == DockStyle.Fill)
                docs = parent.OwnerDocument.CreateElement(DocumentSerializationXml.Documents);
            else
            {
                docs = parent.OwnerDocument.CreateElement(DocumentSerializationXml.DockSite);
                docs.SetAttribute(DocumentSerializationXml.DockSiteSize, XmlConvert.ToString((m_Container.Dock == DockStyle.Left || m_Container.Dock == DockStyle.Right) ? m_Container.Width : m_Container.Height));
                docs.SetAttribute(DocumentSerializationXml.DockingSide, Enum.GetName(typeof(DockStyle), m_Container.Dock));
                docs.SetAttribute(DocumentSerializationXml.OriginalDockSiteSize, XmlConvert.ToString(m_Container.OriginalSize));
            }

			parent.AppendChild(docs);

			SerializeDocumentDockContainer(docs,m_DocumentDockContainer,false);
		}

        private bool SerializeDocument(DocumentDockContainer instance)
        {
            if (instance.Documents.Count == 0) return false;
            foreach (DocumentBaseContainer doc in instance.Documents)
            {
                DocumentBarContainer bar = doc as DocumentBarContainer;
                if (bar != null)
                {
                    if (bar.Bar != null && bar.Bar.SaveLayoutChanges) return true;
                }
                else
                    return true;
            }
            return false;
        }

		private void SerializeDocumentDockContainer(XmlElement parent, DocumentDockContainer instance, bool definitionSerialization)
		{
            if (!SerializeDocument(instance))
				return;
            XmlElement document = null;
			
            document=parent.OwnerDocument.CreateElement(DocumentSerializationXml.DockContainer);
			parent.AppendChild(document);
			document.SetAttribute(DocumentSerializationXml.Orientation,XmlConvert.ToString((int)instance.Orientation));
            document.SetAttribute(DocumentSerializationXml.Width, instance.LayoutBounds.Width.ToString());
            document.SetAttribute(DocumentSerializationXml.Height, instance.LayoutBounds.Height.ToString());

			foreach(DocumentBaseContainer doc in instance.Documents)
			{
				if(doc is DocumentDockContainer)
                    SerializeDocumentDockContainer(document,(DocumentDockContainer)doc,definitionSerialization);
				else if(doc is DocumentBarContainer)
					SerializeDocumentBarContainer(document,(DocumentBarContainer)doc,definitionSerialization);
			}
		}

		private void SerializeDocumentBarContainer(XmlElement parent, DocumentBarContainer instance, bool definitionSerialization)
		{
			if(instance.Bar==null || !instance.Bar.SaveLayoutChanges)
				return;
            
			XmlElement barDocument=parent.OwnerDocument.CreateElement(DocumentSerializationXml.BarContainer);
			parent.AppendChild(barDocument);
			
			barDocument.SetAttribute(DocumentSerializationXml.Width,instance.LayoutBounds.Width.ToString());
			barDocument.SetAttribute(DocumentSerializationXml.Height,instance.LayoutBounds.Height.ToString());
			
			XmlElement barElement=parent.OwnerDocument.CreateElement(BarSerializationXml.Bar);
			barDocument.AppendChild(barElement);
			
			if(definitionSerialization)
				instance.Bar.Serialize(barElement);
			else
				instance.Bar.SerializeLayout(barElement);
		}

		internal void DeserializeLayout(XmlElement parent)
		{
            if (parent.Name != DocumentSerializationXml.Documents && parent.Name != DocumentSerializationXml.DockSite || m_LoadingLayout)
				return;

			m_LoadingLayout=true;
            ItemSerializationContext context = new ItemSerializationContext();
			try
			{
				foreach(XmlElement elem in parent.ChildNodes)
				{
					if(elem.Name==DocumentSerializationXml.DockContainer)
					{
                        context.ItemXmlElement = elem;
						DeserializeDocumentDockContainer(context,m_DocumentDockContainer,false);
						break;
					}
				}
			}
			finally
			{
				m_LoadingLayout=false;
			}
            if (parent.Name == DocumentSerializationXml.DockSite)
            {
                if (m_Container.Dock == DockStyle.Left || m_Container.Dock == DockStyle.Right)
                    m_Container.Width = XmlConvert.ToInt32(parent.GetAttribute(DocumentSerializationXml.DockSiteSize));
                else if (m_Container.Dock == DockStyle.Top || m_Container.Dock == DockStyle.Bottom)
                    m_Container.Height = XmlConvert.ToInt32(parent.GetAttribute(DocumentSerializationXml.DockSiteSize));

                if (parent.HasAttribute(DocumentSerializationXml.OriginalDockSiteSize))
                    m_Container.OriginalSize = XmlConvert.ToInt32(parent.GetAttribute(DocumentSerializationXml.OriginalDockSiteSize));
            }

			m_Container.RecalcLayout();
		}

		internal void DeserializeDefinition(ItemSerializationContext context)
		{
            XmlElement parent = context.ItemXmlElement;
            if ((parent.Name != DocumentSerializationXml.Documents && parent.Name != DocumentSerializationXml.DockSite) || m_LoadingLayout)
				return;

			m_LoadingLayout=true;
			try
			{
				foreach(XmlElement elem in parent.ChildNodes)
				{
					if(elem.Name==DocumentSerializationXml.DockContainer)
					{
                        context.ItemXmlElement = elem;
						DeserializeDocumentDockContainer(context,m_DocumentDockContainer,true);
						break;
					}
				}

                if (parent.HasAttribute(DocumentSerializationXml.DockSiteSize))
                {
                    if (m_Container.Dock == DockStyle.Left || m_Container.Dock == DockStyle.Right)
                        m_Container.Width = XmlConvert.ToInt32(parent.GetAttribute(DocumentSerializationXml.DockSiteSize));
                    else if (m_Container.Dock == DockStyle.Top || m_Container.Dock == DockStyle.Bottom)
                        m_Container.Height = XmlConvert.ToInt32(parent.GetAttribute(DocumentSerializationXml.DockSiteSize));
                }
			}
			finally
			{
				m_LoadingLayout=false;
			}
			m_Container.RecalcLayout();
		}

		private void DeserializeDocumentDockContainer(ItemSerializationContext context, DocumentDockContainer instance, bool deserializeDefinition)
		{
            XmlElement docElement = context.ItemXmlElement;
			if(docElement.Name!=DocumentSerializationXml.DockContainer)
				return;

			instance.Documents.Clear();
			instance.Orientation=(eOrientation)XmlConvert.ToInt32(docElement.GetAttribute(DocumentSerializationXml.Orientation));
            if (docElement.HasAttribute(DocumentSerializationXml.Width) && docElement.HasAttribute(DocumentSerializationXml.Height))
            {
                instance.SetLayoutBounds(new Rectangle(0,0,
                    XmlConvert.ToInt32(docElement.GetAttribute(DocumentSerializationXml.Width)),
                    XmlConvert.ToInt32(docElement.GetAttribute(DocumentSerializationXml.Height))));
            }

			foreach(XmlElement elem in docElement.ChildNodes)
			{
				bool add=true;
				DocumentBaseContainer doc=DocumentSerializationXml.CreateDocument(elem.Name);
                context.ItemXmlElement = elem;
				if(doc is DocumentDockContainer)
					DeserializeDocumentDockContainer(context,(DocumentDockContainer)doc,deserializeDefinition);
				else if(doc is DocumentBarContainer)
                    add=DeserializeDocumentBarContainer(context,(DocumentBarContainer)doc,deserializeDefinition);
				if(add)
					instance.Documents.Add(doc);
			}
		}

		private bool DeserializeDocumentBarContainer(ItemSerializationContext context, DocumentBarContainer instance, bool deserializeDefinition)
		{
            XmlElement docElement = context.ItemXmlElement;
			if(docElement.Name!=DocumentSerializationXml.BarContainer)
				return false;
			
			instance.SetLayoutBounds(new Rectangle(0,0,XmlConvert.ToInt32(docElement.GetAttribute(DocumentSerializationXml.Width)),XmlConvert.ToInt32(docElement.GetAttribute(DocumentSerializationXml.Height))));
			
			foreach(XmlElement elem in docElement.ChildNodes)
			{
				if(elem.Name==BarSerializationXml.Bar)
				{
					instance.Bar=m_Container.Owner.Bars[elem.GetAttribute(BarSerializationXml.Name)];
					if(deserializeDefinition || instance.Bar==null && elem.HasAttribute(BarSerializationXml.Custom) && XmlConvert.ToBoolean(elem.GetAttribute(BarSerializationXml.Custom)))
					{
						// New bar that user has created try to deserialize but if it does not have items ignore it
						instance.Bar=new Bar();
						m_Container.Owner.Bars.Add(instance.Bar);
						if(deserializeDefinition)
							instance.Bar.Deserialize(elem, context);
						else
							instance.Bar.DeserializeLayout(elem);
						if(instance.Bar.Items.Count==0 && !deserializeDefinition)
						{
							m_Container.Owner.Bars.Remove(instance.Bar);
							return false;
						}
						return true;
					}

                    if (instance.Bar != null)
                    {
                        instance.Bar.DeserializeLayout(elem);
                        // Restore hidden dock container size so it can be used when bar is shown
                        if (!instance.Bar.IsVisible)
                        {
                            if (instance.LayoutBounds.Height > 0 && (instance.Bar.DockSide == eDockSide.Top || instance.Bar.DockSide == eDockSide.Bottom))
                            {
                                instance.Bar.Height = instance.LayoutBounds.Height;
                            }
                            else if (instance.LayoutBounds.Width > 0 && (instance.Bar.DockSide == eDockSide.Left || instance.Bar.DockSide == eDockSide.Right))
                            {
                                instance.Bar.Width = instance.LayoutBounds.Width;
                            }
                        }
                    }
                    else
                        return false;
					break;
				}
			}

			return true;
		}
		
		#endregion
	}
}
