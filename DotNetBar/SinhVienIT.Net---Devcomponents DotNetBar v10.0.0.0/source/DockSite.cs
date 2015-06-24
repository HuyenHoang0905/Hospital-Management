namespace DevComponents.DotNetBar
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Data;
    using System.Windows.Forms;
    using DevComponents.DotNetBar.Rendering;

	#region IDockInfo
	/// <summary>
	/// Interface used for docking support.
	/// </summary>
	public interface IDockInfo
	{
		/// <summary>
		/// Returns Minimum docked size of the control.
		/// </summary>
		System.Drawing.Size MinimumDockSize(eOrientation dockOrientation);
		/// <summary>
		/// Returns Preferrred size of the docked control.
		/// </summary>
		System.Drawing.Size PreferredDockSize(eOrientation dockOrientation);
		/// <summary>
		/// Indicated whether control can be docked on top dock site.
		/// </summary>
		bool CanDockTop {get;}
		/// <summary>
		///  Indicated whether control can be docked on bottom dock site.
		/// </summary>
		bool CanDockBottom {get;}
		/// <summary>
		///  Indicated whether control can be docked on left dock site.
		/// </summary>
		bool CanDockLeft {get;}
		/// <summary>
		///  Indicated whether control can be docked on right dock site.
		/// </summary>
		bool CanDockRight {get;}
		/// <summary>
		/// Indicates whether control can be docked as document i.e. middle (fill) dock site.
		/// </summary>
		bool CanDockDocument {get;}
		/// <summary>
		/// Indicates whether control can be docked as tab to another bar.
		/// </summary>
		bool CanDockTab {get;}
		/// <summary>
		///  Indicated whether control can be stretched to fill dock site.
		/// </summary>
		bool Stretch {get;set;}
		/// <summary>
		/// Holds the left position (dock offset) of the control.
		/// </summary>
		int DockOffset {get;set;}
		/// <summary>
		/// Specifies the dock line for the control.
		/// </summary>
		int DockLine {get;set;}
		/// <summary>
		/// Specifies current dock orientation.
		/// </summary>
		eOrientation DockOrientation {get;set;}
		/// <summary>
		/// Gets whether control is docked.
		/// </summary>
		bool Docked {get;}
		/// <summary>
		/// Returns the dock site of the control.
		/// </summary>
		System.Windows.Forms.Control DockedSite {get;}
		/// <summary>
		/// Gets or sets the control dock side.
		/// </summary>
		eDockSide DockSide{get;set;}
		/// <summary>
		/// Sets the dock line for the control. Used internaly by dock manager.
		/// </summary>
		/// <param name="iLine">New Dock line.</param>
		void SetDockLine(int iLine);
		/// <summary>
		/// Gets or sets whether bar is locked to prevent docking below it.
		/// </summary>
		bool LockDockPosition{get;set;}
	}
	#endregion

	#region DockSiteInfo
	/// <summary>
	/// Represent the docking information for an control.
	/// </summary>
	public class DockSiteInfo
	{
		/// <summary>
		/// Control dock side.
		/// </summary>
		public System.Windows.Forms.DockStyle DockSide;
		/// <summary>
		/// Control dock site.
		/// </summary>
		public DockSite objDockSite;
		/// <summary>
		/// Docking offset.
		/// </summary>
		public int DockOffset;
		/// <summary>
		/// Docking line.
		/// </summary>
		public int DockLine;
		/// <summary>
		/// Docked control width.
		/// </summary>
		public int DockedWidth;
		/// <summary>
		/// Docked control height.
		/// </summary>
		public int DockedHeight;
		/// <summary>
		/// Control position.
		/// </summary>
		public int InsertPosition;
		public bool NewLine;
		public bool IsEmpty()
		{
			if(this.DockSide==System.Windows.Forms.DockStyle.None && this.objDockSite==null && this.DockOffset==0 && this.DockLine==0 && this.DockedWidth==0 && this.DockedHeight==0 && this.InsertPosition==0 && this.NewLine==false)
				return true;
			return false;
		}
		public Bar TabDockContainer;
		/// <summary>
		/// Indicates whether to use outline or not
		/// </summary>
		public bool UseOutline;
		/// <summary>
		/// Indicates that dock site should change it's Z-Order so it maximizes the space it consumes as related to other dock sites.
		/// </summary>
		public bool FullSizeDock;
		/// <summary>
		/// Indicates that dock site should change it's Z-Order so it reduces the amount of space it consumes as related to other dock sites.
		/// </summary>
		public bool PartialSizeDock;
		/// <summary>
		/// When either FullSizeDock or PartialSizeDock is set it indicates the new dock site Z-Order index.
		/// </summary>
		public int DockSiteZOrderIndex;
		/// <summary>
		/// Returns the bar that mouse is placed over.
		/// </summary>
		public Bar MouseOverBar;
		/// <summary>
		/// Returns dock side the mouse is indicating user wants to dock bar at.
		/// </summary>
		public eDockSide MouseOverDockSide;
        /// <summary>
        /// Gets the last relative docked to bar.
        /// </summary>
        public Bar LastRelativeDockToBar;
        /// <summary>
        /// Gets the last relative docked to document id.
        /// </summary>
        public long LastRelativeDocumentId;
        /// <summary>
        /// Returns side of last docked-to dock site.
        /// </summary>
        public eDockSide LastDockSiteSide;
	}
	#endregion

    /// <summary>
    ///    Dock Sites are created by DotNetBar control on each edge of the
    ///    DotNetBar container control and are used for docking purposes.
    ///    If Dock Site does not contain any controls it will be invisible.
    /// </summary>
    [ToolboxItem(false), DesignTimeVisible(false), Designer("DevComponents.DotNetBar.Design.DockSiteDesigner, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
	public class DockSite : System.Windows.Forms.Control
	{
		private eOrientation m_DockOrientation;
		private bool m_Layingout=false;
		private bool m_OversizeLayout=false;
		//private int m_DockMargin;
		private DotNetBarManager m_Owner=null;
		private bool m_NeedsLayout=false;
		private bool m_NeedsNormalize=false;

		// Theme Cache
		private ThemeRebar m_ThemeRebar=null;

		private eBackgroundImagePosition m_BackgroundImagePosition=eBackgroundImagePosition.Stretch;
		private byte m_BackgroundImageAlpha=255;
		private Color m_BackColor2=Color.Empty;
		private int m_BackColorGradientAngle=90;

		private bool m_Disposed=false;
		private DocumentDockUIManager m_DocumentUIManager=null;
		private bool m_LayoutSuspended=false;
        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool _DesignerLoading = false;
        private int m_OriginalSize = 0;
        private bool m_OptimizeLayoutRedraw = true;

		/// <summary>
		/// Creates new instance of DockSite object with specified dock style.
		/// </summary>
		/// <param name="DockSide">Specifies the position and manner in which a site is docked.</param>
		public DockSite(System.Windows.Forms.DockStyle DockSide):this()
		{
			this.Dock=DockSide;
		}

        protected override AccessibleObject CreateAccessibilityInstance()
        {
            return new DockSiteAccessibleObject(this);
        }

		/// <summary>
		/// Creates new instance of DockSite object.
		/// </summary>
		public DockSite()
		{
			this.TabStop=false;
			this.SetStyle(ControlStyles.Selectable,false);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			this.IsAccessible=true;
			this.AccessibleRole=AccessibleRole.Window;
		}

		protected override void Dispose(bool disposing)
		{
			m_Owner=null;
			m_Disposed=true;
			base.Dispose(disposing);
		}

        /// <summary>
        /// Gets or sets whether painting is disabled on dock site while layout of bars is performed. Default value is true.
        /// You might need to set this property to false if you are expirience vide flashing while using DirectX video animation in Bar controls that are part of the
        /// dock site.
        /// </summary>
        [DefaultValue(true), Description("Indicates whether painting is disabled on dock site while layout of bars is performed.")]
        public bool OptimizeLayoutRedraw
        {
            get { return m_OptimizeLayoutRedraw; }
            set { m_OptimizeLayoutRedraw = value; }
        }

		private DocumentDockContainer m_DocumentDockContainer=null;
		[Browsable(false), DefaultValue(null)]
		public DocumentDockContainer DocumentDockContainer
		{
			get {return m_DocumentDockContainer;}
			set
			{
				m_DocumentDockContainer=value;
				m_DocumentUIManager=this.GetDocumentUIManager();
				m_DocumentUIManager.RootDocumentDockContainer=m_DocumentDockContainer;
                if (this.Dock != DockStyle.Fill && value!=null) m_DocumentDockContainer.RecordDocumentSize = true;
			}
		}

		/// <summary>
		/// Returns reference to the DocumentDockUIManager object used for interaction with document docking engine.
		/// </summary>
		/// <returns>Reference to the DocumentDockUIManager object.</returns>
		public DocumentDockUIManager GetDocumentUIManager()
		{
            if (m_DocumentUIManager == null)
            {
                m_DocumentUIManager = new DocumentDockUIManager(this);
                m_DocumentUIManager.RootDocumentDockContainer = m_DocumentDockContainer;
            }
			return m_DocumentUIManager;
		}

		/// <summary>
		/// Specifies background image position when container is larger than image.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),Category("Appearance"),DefaultValue(eBackgroundImagePosition.Stretch),Description("Specifies background image position when container is larger than image.")]
		public eBackgroundImagePosition BackgroundImagePosition
		{
			get {return m_BackgroundImagePosition;}
			set
			{
				if(m_BackgroundImagePosition!=value)
				{
					m_BackgroundImagePosition=value;
					this.Refresh();
				}
			}
		}

		/// <summary>
		/// Saves layout for bars contained by dock site.
		/// </summary>
		/// <param name="xmlBars">Parent XmlElement.</param>
		public void SaveLayout(System.Xml.XmlElement xmlBars)
		{
			if(this.IsDocumentDock || m_DocumentDockContainer!=null)
			{
				this.GetDocumentUIManager().SerializeLayout(xmlBars);
			}
			else
			{
				foreach(Control ctrl in this.Controls)
				{
					DevComponents.DotNetBar.Bar bar=ctrl as DevComponents.DotNetBar.Bar;
					if(bar!=null && bar.Name!="" && bar.SaveLayoutChanges)
					{
						System.Xml.XmlElement xmlBar=xmlBars.OwnerDocument.CreateElement("bar");
						xmlBars.AppendChild(xmlBar);
						bar.SerializeLayout(xmlBar);
					}
				}
			}
		}

		/// <summary>
		/// Loads layout for the bars.
		/// </summary>
		/// <param name="xmlBars">Parent XmlElement that was passed to SaveLayout method to save layout</param>
		public void LoadLayout(System.Xml.XmlElement xmlBars)
		{
			m_LayoutSuspended=true;
			try
			{
				if(!this.IsDocumentDock &&  m_DocumentDockContainer==null)
				{
					foreach(System.Xml.XmlElement xmlBar in xmlBars.ChildNodes)
					{
						DevComponents.DotNetBar.Bar bar=null;
						if(m_Owner.Bars.Contains(m_Owner.Bars[xmlBar.GetAttribute("name")]))
							bar=m_Owner.Bars[xmlBar.GetAttribute("name")];
						else
						{
							bar=new Bar();
							m_Owner.Bars.Add(bar);
						}
						bar.DeserializeLayout(xmlBar);
					}
				}
				else
				{
					this.GetDocumentUIManager().DeserializeLayout((System.Xml.XmlElement)xmlBars.FirstChild);
				}
			}
			finally
			{
				m_LayoutSuspended=false;
				this.RecalcLayout();
			}
		}

        /// <summary>
        /// Suspends normal layout logic.
        /// </summary>
        public new void SuspendLayout()
        {
            m_LayoutSuspended = true;
            base.SuspendLayout();
        }

        /// <summary>
        /// Resumes normal layout logic.
        /// </summary>
        public new void ResumeLayout()
        {
            this.ResumeLayout(true);
        }

        /// <summary>
        /// Resumes normal layout logic. Optionally forces an immediate layout of pending layout requests.
        /// </summary>
        public new void ResumeLayout(bool performLayout)
        {
            m_LayoutSuspended = false;
            base.ResumeLayout(performLayout);
        }

		/// <summary>
		/// Specifies the transparency of background image.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),Category("Appearance"),DefaultValue((byte)255),Description("Specifies the transparency of background image.")]
		public byte BackgroundImageAlpha
		{
			get {return m_BackgroundImageAlpha;}
			set
			{
				if(m_BackgroundImageAlpha!=value)
				{
					m_BackgroundImageAlpha=value;
					this.Refresh();
				}
			}
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeBackgroundImageAlpha()
		{
			return m_BackgroundImageAlpha!=255;
		}

		/// <summary>
		/// Gets or sets the target gradient background color.
		/// </summary>
		[Browsable(true),Description("Indicates the target gradient background color."),Category("Style")]
		public Color BackColor2
		{
			get {return m_BackColor2;}
			set
			{
				m_BackColor2=value;
				if(this.DesignMode)
					this.Refresh();
			}
		}
		[Browsable(false),EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeBackColor2()
		{
			return !m_BackColor2.IsEmpty;
		}
		[Browsable(false),EditorBrowsable(EditorBrowsableState.Never)]
		public void ResetBackColor2()
		{
			m_BackColor2=Color.Empty;
		}

		/// <summary>
		/// Gets or sets gradient fill angle.
		/// </summary>
		[Browsable(true),Description("Indicates gradient fill angle."),Category("Style"),DefaultValue(90)]
		public int BackColorGradientAngle
		{
			get {return m_BackColorGradientAngle;}
			set
			{
				if(value!=m_BackColorGradientAngle)
				{
					m_BackColorGradientAngle=value;
					if(this.DesignMode)
						this.Refresh();
				}
			}
		}

		internal bool GetDesignMode()
		{
			return this.DesignMode;
		}

//		/// <summary>
//		/// Gets the collection of controls contained within the control.
//		/// </summary>
//		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),Browsable(false),DevCoBrowsable(false)]
//		public new Control.ControlCollection Controls
//		{
//			get
//			{
//				return base.Controls;
//			}
//		}

        //public bool ShouldSerializeControls()
        //{
        //    if(this.Dock==DockStyle.Fill)
        //        return true;
        //    else
        //        return false;
        //}

		protected override void OnVisibleChanged(EventArgs e)
		{
			base.OnVisibleChanged(e);
			if(this.Visible)
			{
				Control.ControlAccessibleObject acc=this.AccessibilityObject as Control.ControlAccessibleObject;
				if(acc!=null)
				{
					acc.NotifyClients(AccessibleEvents.Show);
				}
			}
		}

		protected override void OnPaintBackground(PaintEventArgs pevent)
		{
			if(IsLayoutSuspended)
				return;
			if(!m_BackColor2.IsEmpty && !this.BackColor.IsEmpty)
			{
				if(this.Width>0 && this.Height>0)
				{
                    DisplayHelp.FillRectangle(pevent.Graphics, this.ClientRectangle, this.BackColor, this.BackColor2, m_BackColorGradientAngle);
					return;
				}
			}

			if(this.Controls.Count>0 && this.Controls[0] is Bar && ((Bar)this.Controls[0]).ThemedBackground)
			{
				ThemeRebar theme=this.ThemeRebar;
				theme.DrawBackground(pevent.Graphics,ThemeRebarParts.Background,ThemeRebarStates.Normal,this.ClientRectangle);
                return;
			}
            ColorScheme cs = GetColorScheme();
            if (cs != null && this.BackColor != Color.Transparent)
                DisplayHelp.FillRectangle(pevent.Graphics, this.ClientRectangle, cs.DockSiteBackColor, cs.DockSiteBackColor2, cs.DockSiteBackColorGradientAngle);
            else
				base.OnPaintBackground(pevent);
		}

        private ColorScheme GetColorScheme()
        {
            if (m_Owner != null && m_Owner.UseGlobalColorScheme)
            {
                if ((m_Owner.Style == eDotNetBarStyle.Office2007 || m_Owner.Style == eDotNetBarStyle.StyleManagerControlled) && GlobalManager.Renderer is Office2007Renderer)
                {
                    return ((Office2007Renderer)GlobalManager.Renderer).ColorTable.LegacyColors;
                }
                return m_Owner.ColorScheme;
            }
            else if (this.Controls.Count > 0 && this.Controls[0] is Bar && (((Bar)this.Controls[0]).Style == eDotNetBarStyle.Office2003 || ((Bar)this.Controls[0]).Style == eDotNetBarStyle.VS2005 || BarFunctions.IsOffice2007Style(((Bar)this.Controls[0]).Style)) )
            {
                Bar bar = this.Controls[0] as Bar;
                if (bar.Style == eDotNetBarStyle.StyleManagerControlled && GlobalManager.Renderer is Office2007Renderer)
                    return ((Office2007Renderer)GlobalManager.Renderer).ColorTable.LegacyColors;
                return bar.GetColorScheme();
            }

            return null;
        }

		protected override void OnPaint(PaintEventArgs pevent)
		{
			if(IsLayoutSuspended)
				return;
			if(this.BackgroundImage!=null)
			{
				BarFunctions.PaintBackgroundImage(pevent.Graphics,this.ClientRectangle,this.BackgroundImage,m_BackgroundImagePosition,m_BackgroundImageAlpha);
			}
			else
				base.OnPaint(pevent);
		}

		private DevComponents.DotNetBar.ThemeRebar ThemeRebar
		{
			get
			{
				if(m_ThemeRebar==null)
				{
					m_ThemeRebar=new ThemeRebar(this);
				}
				return m_ThemeRebar;
			}
		}

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            this.RecalcLayout();
        }

		protected override void OnHandleDestroyed(EventArgs e)
		{
			if(m_ThemeRebar!=null)
			{
				m_ThemeRebar.Dispose();
				m_ThemeRebar=null;
			}
			base.OnHandleDestroyed(e);
		}
		protected override void WndProc(ref Message m)
		{
			if(m.Msg==NativeFunctions.WM_THEMECHANGED)
			{
				m_ThemeRebar=null;
			}
			base.WndProc(ref m);
		}

		/// <summary>
		/// Specifies the position and manner in which a site is docked.
		/// </summary>
		public override System.Windows.Forms.DockStyle Dock
		{
			get
			{
				return base.Dock;
			}
			set
			{
				base.Dock=value;
                if(value==System.Windows.Forms.DockStyle.Top || value==System.Windows.Forms.DockStyle.Bottom || value==DockStyle.Fill)
				{
					if(this.Controls.Count==0)
						this.Height=0;
					m_DockOrientation=eOrientation.Horizontal;
				}
				else
				{
					if(this.Controls.Count==0)
						this.Width=0;
					m_DockOrientation=eOrientation.Vertical;
				}
			}
		}

        protected override void OnControlRemoved(ControlEventArgs e)
        {
            base.OnControlRemoved(e);
            // Designer is removing controls directly during Undo operations so resize the site.
            if (this.DesignMode)
                RecalcLayout();
        }

		protected override void OnControlAdded(ControlEventArgs e)
		{
			base.OnControlAdded(e);
			
			IDockInfo pDockInfo=e.Control as IDockInfo;
            
			if(pDockInfo==null)
			{
				// Refuse controls that does not implement IDockInfo interface
				this.Controls.Remove(e.Control);
                throw new System.ArgumentException("Only Bar objects can be added to DockSite through DotNetBar designer.");
			}

			//if(IsDocumentDock)
				UpdateBarsCollection();
		}

        internal bool IsAnyControlVisible
        {
            get
            {
                foreach (Control c in this.Controls)
                {
                    if (c is Bar)
                    {
                        if(((Bar)c).IsVisible) return true;
                    }
                    else if ( c.Visible) return true;
                }
                return false;
            }
        }

		/// <summary>
		/// Docks the bar to the dock site.
		/// </summary>
		/// <param name="objBar">Bar to dock.</param>
		internal void AddBar(Control objBar)
		{
			IDockInfo pDockInfo=objBar as IDockInfo;
			if(pDockInfo==null)
			{
				// Throw exception IDockInfo must be implemented
                throw new System.ArgumentException("Only Bar objects can be added to DockSite through DotNetBar designer.");
			}

			if(!this.Visible)
			{
				if(this.Owner==null || this.Owner!=null && !this.Owner.IsLoadingDefinition)
				{
					BarFunctions.SetControlVisible(this,true);
				}
			}

			// Set Dock Orientation
			pDockInfo.DockOrientation=m_DockOrientation;

			// Find the right place for the control
			Control objCtrl=null;
			IDockInfo pCtrlDockInfo=null;
			bool bAdded=false;
			for(int i=0;i<this.Controls.Count;i++)
			{
				objCtrl=this.Controls[i];
				pCtrlDockInfo=objCtrl as IDockInfo;
				if(pCtrlDockInfo==null)
					continue;
				if(pDockInfo.DockLine<=pCtrlDockInfo.DockLine && pDockInfo.DockOffset<pCtrlDockInfo.DockOffset || pDockInfo.DockLine<pCtrlDockInfo.DockLine)
				{
					this.Controls.Add(objBar);
					this.Controls.SetChildIndex(objBar,i);
					bAdded=true;
					break;
				}
			}

			// Add it to the end if not added in the loop before
			if(!bAdded)
				this.Controls.Add(objBar);

			if(objBar is Bar && ((Bar)objBar).LayoutType==eLayoutType.DockContainer)
			{
				Bar bar=objBar as Bar;
				if(m_DockOrientation==eOrientation.Horizontal && bar.ItemsContainer.MinHeight>0)
					((Bar)objBar).SyncLineMinHeight();
				else if(m_DockOrientation==eOrientation.Vertical && bar.ItemsContainer.MinWidth>0)
					((Bar)objBar).SyncLineMinWidth();
			}
			NormalizeBars();
			LayoutBars();
		}

		/// <summary>
		/// Dockes the Bar to dock site at specified position.
		/// </summary>
		/// <param name="objBar">Bar to dock.</param>
		/// <param name="iInsertAtPosition">Bar insert position.</param>
		internal void AddBar(Control objBar, int iInsertAtPosition)
		{
			IDockInfo pDockInfo=objBar as IDockInfo;
			if(pDockInfo==null)
			{
				// Throw exception IDockInfo must be implemented
                throw new System.ArgumentException("Only Bar objects can be added to DockSite through DotNetBar designer.");
			}

			if(!this.Visible)
			{
				BarFunctions.SetControlVisible(this,true);
			}

			// Set Dock Orientation
			pDockInfo.DockOrientation=m_DockOrientation;

			this.Controls.Add(objBar);
			this.Controls.SetChildIndex(objBar,iInsertAtPosition);
			if(objBar is Bar && ((Bar)objBar).LayoutType==eLayoutType.DockContainer)
			{
				Bar bar=objBar as Bar;
				if(m_DockOrientation==eOrientation.Horizontal && bar.ItemsContainer.MinHeight>0)
					((Bar)objBar).SyncLineMinHeight();
				else if(m_DockOrientation==eOrientation.Vertical && bar.ItemsContainer.MinWidth>0)
					((Bar)objBar).SyncLineMinWidth();
			}
			NormalizeBars();
			LayoutBars();
		}

		internal void SetBarPosition(Control objBar, int iNewIndex)
		{
			if(this.Contains(objBar))
			{
				if(this.Controls.GetChildIndex(objBar)!=iNewIndex)
				{
					this.Controls.SetChildIndex(objBar,iNewIndex);
				}
				LayoutBars();
			}
		}

		internal void SetBarPosition(Control objBar, int iNewIndex, bool bInNewLine)
		{
			if(this.Contains(objBar))
			{
				if(this.Controls.GetChildIndex(objBar)!=iNewIndex)
				{
					this.Controls.SetChildIndex(objBar,iNewIndex);
				}
				if(bInNewLine)
				{
					iNewIndex=this.Controls.IndexOf(objBar);
					if(iNewIndex>=0)
					{
						if(iNewIndex>0)
							((IDockInfo)objBar).SetDockLine(((IDockInfo)this.Controls[iNewIndex-1]).DockLine+1);
						else
							((IDockInfo)objBar).SetDockLine(-1);

						for(int i=iNewIndex+1;i<this.Controls.Count;i++)
							((IDockInfo)this.Controls[i]).SetDockLine(((IDockInfo)this.Controls[i]).DockLine+2);
					}
				}
				LayoutBars();
			}
		}

		internal void AdjustBarPosition(Control objBar)
		{
			IDockInfo pDockInfo=objBar as IDockInfo;
			if(pDockInfo==null)
			{
				// Throw exception IDockInfo must be implemented
                throw new System.ArgumentException("Only Bar objects can be added to DockSite through DotNetBar designer.");
			}
			if(!this.Controls.Contains(objBar))
				throw new System.ArgumentException("Control is not part of this collection.");

			// Find the right place for the control
			Control objCtrl=null;
			IDockInfo pCtrlDockInfo=null;
			for(int i=0;i<this.Controls.Count;i++)
			{
				objCtrl=this.Controls[i];
				if(objCtrl==objBar)
					continue;
				pCtrlDockInfo=objCtrl as IDockInfo;
				if(pCtrlDockInfo==null)
					continue;
				if(pDockInfo.DockLine==pCtrlDockInfo.DockLine && pDockInfo.DockOffset<=pCtrlDockInfo.DockOffset)
				{
					if(i>0 && i>this.Controls.GetChildIndex(objBar))
						i--;
					this.Controls.SetChildIndex(objBar,i);
					return;
				}
				else if(pDockInfo.DockLine<pCtrlDockInfo.DockLine)
				{
					if(i>0)
						i--;
					this.Controls.SetChildIndex(objBar,i);
					return;
				}
			}
			// Set its position to the end
			this.Controls.SetChildIndex(objBar,this.Controls.Count);
		}

		/// <summary>
		/// Relayouts all docked controls in the site.
		/// </summary>
		public void RecalcLayout()
		{
			LayoutBars();
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Browsable(false),System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public bool NeedsLayout
		{
			get {return m_NeedsLayout;}
			set {m_NeedsLayout=value;}
		}

		private void NormalizeBars()
		{
			if(IsLayoutSuspended)
			{
				m_NeedsNormalize=true;
				return;
			}

			int iDockLine=-1, iLastDockLine=-2;
			for(int i=0;i<this.Controls.Count;i++)
			{
				Bar bar=this.Controls[i] as Bar;
				if(bar==null /*|| !bar.Visible*/) // Need to preserve dock line even for invisible bars so they retain position when they are shown 
					continue;
				if(bar.DockLine!=iLastDockLine)
				{
					iDockLine++;
					iLastDockLine=bar.DockLine;
					bar.SetDockLine(iDockLine);
				}
				else
					bar.SetDockLine(iDockLine);
			}

			m_NeedsNormalize=false;
		}

		private bool IsLayoutSuspended
		{
			get
			{
                if (m_Owner != null && m_Owner.SuspendLayout || m_LayoutSuspended || _DesignerLoading)
					return true;
				return false;
			}
		}

        private Rectangle GetDockLayoutRectangle()
        {
            Rectangle r = this.ClientRectangle;
            if (this.Dock == DockStyle.Fill || m_DocumentDockContainer== null)
                return r;
            int splitter = m_DocumentDockContainer.SplitterSize;

            if (this.Dock == DockStyle.Left)
            {
                r.Width -= splitter;
            }
            else if (this.Dock == DockStyle.Right)
            {
                r.Width -= splitter;
                r.X += splitter;
            }
            else if (this.Dock == DockStyle.Bottom)
            {
                r.Height -= splitter;
                r.Y += splitter;
            }
            else if (this.Dock == DockStyle.Top)
            {
                r.Height -= splitter;
            }

            return r;
        }

		private void LayoutBars()
		{
			if(m_Disposed || this.IsDisposed)
				return;

			if(IsLayoutSuspended)
			{
				m_NeedsLayout=true;
				return;
			}

			if(m_NeedsNormalize)
				NormalizeBars();

            if (this.Controls.Count == 0 && ((this.Dock == DockStyle.Left || this.Dock == DockStyle.Right) && this.Width == 0 || 
                (this.Dock == DockStyle.Top || this.Dock == DockStyle.Bottom) && this.Height == 0))
                return;

			// Make sure that recalc layout is not called if form is minimized OR width or height=0
			System.Windows.Forms.Form form=this.FindForm();
			if(form!=null && (form.Width==0 || form.Height==0))
				return;
			if(form!=null && form.ParentForm!=null)
				form=form.ParentForm;
			if(form!=null && (form.Width==0 || form.Height==0))
				return;
			if(form!=null && form.WindowState==FormWindowState.Minimized)
				return;

			m_NeedsLayout=false;

			if(m_Layingout)
			{
				return;
			}
			m_Layingout=true;
			
			int parentZOrder=-1;

			if(this.Dock==DockStyle.Fill || m_DocumentDockContainer!=null)
			{
                Rectangle oldBounds = this.Bounds;
                if (this.Parent != null && this.Parent.Visible && m_OptimizeLayoutRedraw) // WARNING: Sending this message when parent is not visible was causing strange effects with layout of native .NET dock controls
				{
					if(this.Parent!=null) parentZOrder=this.Parent.Controls.IndexOf(this);
					NativeFunctions.SendMessage(this.Handle,NativeFunctions.WM_SETREDRAW,0,0);
				}

                if (m_DocumentDockContainer != null)
                {
                    if (this.Dock != DockStyle.Fill) m_DocumentDockContainer.RecordDocumentSize = true;
                    m_DocumentDockContainer.Layout(this.GetDockLayoutRectangle());
                    if (this.Dock != DockStyle.Fill && m_DocumentDockContainer.DisplayBounds.IsEmpty)
                    {
                        if (this.Dock == DockStyle.Left || this.Dock == DockStyle.Right)
                            this.Width = 0;
                        else
                            this.Height = 0;
                    }
                }

				m_Layingout=false;
                if (this.Parent != null && this.Parent.Visible && m_OptimizeLayoutRedraw) // See WARNING at the beginning
				{
					NativeFunctions.SendMessage(this.Handle,NativeFunctions.WM_SETREDRAW,1,0);
                    this.Parent.Invalidate(((this.Width == 0 || this.Height == 0) && oldBounds.Width > 0 && oldBounds.Height > 0) ? oldBounds : this.Bounds, true);
					this.Parent.Update();
				}
				if(parentZOrder!=-1) this.Parent.Controls.SetChildIndex(this,parentZOrder);
				return;
			}

			IDockInfo pDockInfo=null;
			Bar objBar=null;
			Control objCtrl=null;
			System.Drawing.Size objMinSize;
			//System.Drawing.Size objOldSize;
			//System.Drawing.Point objOldLocation;
			bool bResetOffset=false;

			Rectangle oldDockSiteSize=this.Bounds;
            if (this.Parent != null && this.Parent.Visible && m_OptimizeLayoutRedraw) // WARNING: Sending this message when parent is not visible was causing strange effects with layout of native .NET dock controls
			{
				if(this.Parent!=null) parentZOrder=this.Parent.Controls.IndexOf(this);
				NativeFunctions.SendMessage(this.Handle,NativeFunctions.WM_SETREDRAW,0,0);
			}
            
			// Horizontal and vertical spacing is needed for Office2003 style support
			int iHorSpacing=0, iVerSpacing=0;

			// Need to know when we have stretched dock containers in the same line
			int[] lineDockCounts=new int[256];
			int[] lineDockProcessed=new int[256];
			int[] lineContainerMaxSize=new int[256];
			int[] lineDockSize=new int[256];
			int[] lineDefHeightCount=new int[256];
			ArrayList arrBarsInSameLine=null;
			for(int i=0;i<this.Controls.Count;i++)
			{
				objBar=this.Controls[i] as Bar;
				if(objBar!=null && !objBar.Visible)
					continue;
				if(objBar!=null && objBar.LayoutType==eLayoutType.DockContainer && objBar.Stretch)
				{
					if(objBar.DockLine>=0)
					{
						lineDockCounts[objBar.DockLine]++;
						lineDockProcessed[objBar.DockLine]++;
						if(m_DockOrientation==eOrientation.Horizontal)
						{
							if(objBar.SplitDockWidth==0)
								lineDefHeightCount[objBar.DockLine]++;
							else
								lineDockSize[objBar.DockLine]+=objBar.SplitDockWidth;
							
						}
						else
						{
							if(objBar.SplitDockHeight==0)
								lineDefHeightCount[objBar.DockLine]++;
							else
								lineDockSize[objBar.DockLine]+=objBar.SplitDockHeight;
						}
					}
				}
			}

            if (this.Controls.Count > 0 && this.Controls[0] is Bar && ((Bar)this.Controls[0]).LayoutType == eLayoutType.Toolbar && (((Bar)this.Controls[0]).Style == eDotNetBarStyle.Office2003 || ((Bar)this.Controls[0]).Style == eDotNetBarStyle.VS2005 || BarFunctions.IsOffice2007Style(((Bar)this.Controls[0]).Style)))
			{
				iHorSpacing=2;
				iVerSpacing=1;
			}

			int iMaxSize=0, iX=0, iY=0; // When in horizontal layout this is max. height, for vertical layout this is max width
			bool bSameLineDocking=false;
			int iCurrentLine=0,iLastLine=0;

			bool bRepeatLayout=false;
			bool bCurrentLineIncreased=false, bSwitchLine=false;
			do
			{
				iMaxSize=0; // When in horizontal layout this is max. height, for vertical layout this is max width
				iX=0;
				iY=0;
				iCurrentLine=0;
				iLastLine=0;
				bRepeatLayout=false;
				arrBarsInSameLine=new ArrayList(this.Controls.Count);
				if(this.Controls.Count>0)
				{
					pDockInfo=this.Controls[0] as IDockInfo;
					iLastLine=pDockInfo.DockLine;
				}
				bool bLastHidden=false;

				for(int i=0;i<this.Controls.Count;i++)
				{
					objCtrl=this.Controls[i];
					pDockInfo=objCtrl as IDockInfo;
					objBar=objCtrl as Bar;
					//objOldSize=objCtrl.Size;
					//objOldLocation=objCtrl.Location;
					bSameLineDocking=false;

					if(pDockInfo==null || !objCtrl.Visible)
					{
						// Allow hidden bars to remember the dock line they had when visible so
						// same position can be retained
						if(objBar!=null)
						{
							if(pDockInfo.DockLine>iLastLine)
							{
								iLastLine=pDockInfo.DockLine;
								if(!bCurrentLineIncreased)
								{
									iCurrentLine++;
									bSwitchLine=true;
								}
							}
						}
						bLastHidden=true;
						// end hidden bar mod
						continue;
					}

					bCurrentLineIncreased=false;

					if(m_DockOrientation==eOrientation.Horizontal)
					{
						if(bSwitchLine)
						{
							iY+=(iMaxSize+iVerSpacing);
							iX=0;
							iMaxSize=0;
							bSwitchLine=false;
						}

						System.Drawing.Size frameSize=System.Drawing.Size.Empty;
                        if(objBar.LayoutType == eLayoutType.Toolbar)
                            objMinSize = objBar.GetAdjustedFullSize(pDockInfo.MinimumDockSize(m_DockOrientation));
                        else
                            objMinSize = new Size(1, 1); // objBar.GetAdjustedFullSize(pDockInfo.MinimumDockSize(m_DockOrientation));
						if(objBar.Stretch || (i>0 && ((iX+objCtrl.Width>this.Width && objMinSize.Width+iX>this.Width) || pDockInfo.DockLine>iLastLine)))
						{
							if(objBar.Stretch && objBar.LayoutType==eLayoutType.DockContainer && objBar.DockLine>=0 && lineDockCounts[objBar.DockLine]>0)
							{
								if(lineDockCounts[objBar.DockLine]==1)
									objBar.SplitDockWidth=0;

								if(objBar.SplitDockWidth!=0 && !(lineDockProcessed[objBar.DockLine]==1 && lineDockSize[objBar.DockLine]!=this.Width))
									frameSize=new Size(objBar.SplitDockWidth,1); //objBar.Size=new Size(objBar._SplitDockWidth,1);
								else
								{
									if(lineDockProcessed[objBar.DockLine]==1) // && lineDockSize[objBar.DockLine]!=this.Width && (lineDefHeightCount[objBar.DockLine]==0 || objBar._SplitDockWidth+iX==this.Width))
									{
//										if(objBar._SplitDockWidth>0)
//											objBar.Size=new Size(this.Width-lineDockSize[objBar.DockLine]+objBar._SplitDockWidth,1);
//										else
//											objBar.Size=new Size(this.Width-lineDockSize[objBar.DockLine],1);
										frameSize=new Size(this.Width-iX,1); //objBar.Size=new Size(this.Width-iX,1);
									}
									else
										frameSize=new Size((this.Width-lineDockSize[objBar.DockLine])/(lineDefHeightCount[objBar.DockLine]==0?1:lineDefHeightCount[objBar.DockLine]),1); //objBar.Size=new Size((this.Width-lineDockSize[objBar.DockLine])/lineDefHeightCount[objBar.DockLine],1);
								}
								//objBar.Size=new Size(this.Width/lineDockCounts[objBar.DockLine],1);
								// We need to switch to new line here
								if(iLastLine!=pDockInfo.DockLine && lineDockProcessed[objBar.DockLine]==lineDockCounts[objBar.DockLine] && iX>0)
								{
									iLastLine=pDockInfo.DockLine;
									iCurrentLine++;
									iY+=(iMaxSize+iVerSpacing);
									iX=0;
									iMaxSize=0;
									if(lineDockCounts[objBar.DockLine]==1 || frameSize.IsEmpty) // added to allow split bars on top dock site
										frameSize=new Size(this.Width,1); //objBar.Size=new Size(this.Width,1);
								}
								lineDockProcessed[objBar.DockLine]--;
								bSameLineDocking=true;
								arrBarsInSameLine.Add(objBar);
							}
							else
								frameSize=new Size(this.Width,1); //objBar.Size=new Size(this.Width,1);
						}
						else
							frameSize=new Size(this.Width-iX,1); //objBar.Size=new Size(this.Width-iX,1);

						objBar.RecalcSize(frameSize);

						// If item cannot fit move it to the next line if we are not on first line
						// Or if item wants to be in new line
                        if(i>0 && pDockInfo.DockLine>iLastLine && iX>0)
                        {
                            // If it did not fit and we do same line docking we need to rearrange things
                            if(bSameLineDocking)
                            {
                                lineDockCounts[objBar.DockLine]=0;
                                lineDockProcessed[objBar.DockLine]=0;
                                bRepeatLayout=true;
                                break;
                            }

                            if(pDockInfo.DockLine>iLastLine)
                                bResetOffset=false;
                            else
                                bResetOffset=true;

                            // Remember Last Line
                            iLastLine=pDockInfo.DockLine;
                            // Go to the new line
                            iCurrentLine++;
                            iY+=(iMaxSize+iVerSpacing);
                            iX=0;
                            iMaxSize=0;

                            if(bResetOffset)
                                pDockInfo.DockOffset=0;

                            // See can we honor the DockOffset
                            if(pDockInfo.DockOffset>0)
                            {
                                if(pDockInfo.DockOffset+objCtrl.Width>this.Width)
                                    iX=this.Width-objCtrl.Width;
                                else
                                    iX=pDockInfo.DockOffset;
                                if(iX<0)
                                    iX=0;
                            }
                        }
                        else
						{
							if(iLastLine!=pDockInfo.DockLine)
							{
								iLastLine=pDockInfo.DockLine;
								if(bLastHidden)
									iCurrentLine++; // Done to retain the position of hidden bars
							}
							if(bResetOffset)
								pDockInfo.DockOffset=0;
							// Try to honor DockOffset
							if(!pDockInfo.Stretch)
							{
								if(pDockInfo.DockOffset>iX && pDockInfo.DockOffset+objCtrl.Width<=this.Width)
									iX=pDockInfo.DockOffset;
								else if(pDockInfo.DockOffset>iX && this.Width-objCtrl.Width>iX)
									iX=this.Width-objCtrl.Width;
							}
						}

						if(iX+objCtrl.Width>this.Width)
						{
							// We have to resize it to fit...
							//objCtrl.Width=this.Width-iX;
							if(objBar!=null)
								objBar.RecalcSize(new Size(this.Width-iX,objBar.Height));
							else
                                objCtrl.Width=this.Width-iX;
							// Then reset DockOffset
							pDockInfo.DockOffset=0;
						}
						//					else
						//					{
						//						if(pDockInfo.Stretch)
						//						{
						//							objCtrl.Width=this.Width;
						//							if(objBar!=null)
						//								objBar.RecalcSize();
						//						}
						//					}

						objCtrl.Location=new Point(iX,iY);

						iX+=(objCtrl.Width+iHorSpacing);
						if(bResetOffset || pDockInfo.DockLine!=iCurrentLine)
							pDockInfo.DockOffset=objCtrl.Left;

						// Get the max height
						if(objCtrl.Height>iMaxSize)
							iMaxSize=objCtrl.Height;

						if(objBar!=null && objBar.ItemsContainer.HeightInternal>lineContainerMaxSize[iCurrentLine])
							lineContainerMaxSize[iCurrentLine]=objBar.ItemsContainer.HeightInternal;

						if(pDockInfo.Stretch)
						{
							if(objBar.LayoutType==eLayoutType.DockContainer && objBar.DockLine>=0 && lineDockProcessed[objBar.DockLine]>0)
							{
								pDockInfo.SetDockLine(iCurrentLine);
							}
							else
							{
								pDockInfo.SetDockLine(iCurrentLine);
								// Remember Last Line
								iLastLine=pDockInfo.DockLine;
								// Go to the new line
								iCurrentLine++;
								bCurrentLineIncreased=true;
								iY+=(iMaxSize+iVerSpacing);
								iX=0;
								iMaxSize=0;
							}
						}
						else
							pDockInfo.SetDockLine(iCurrentLine);
					}
					else
					{
						//*********************************************************************
						// Vertical layout
						//*********************************************************************
						if(bSwitchLine)
						{
							iX+=(iMaxSize+iVerSpacing);
							iY=0;
							iMaxSize=0;
							bSwitchLine=false;
						}

						
						System.Drawing.Size frameSize=System.Drawing.Size.Empty;
                        objMinSize = new Size(1, 1); // objBar.GetAdjustedFullSize(pDockInfo.MinimumDockSize(m_DockOrientation));
						if(objBar.Stretch || (i>0 && ((iY+objCtrl.Height>this.Height && objMinSize.Height+iY>this.Height) || pDockInfo.DockLine>iLastLine)))
						{
							if(objBar.Stretch && objBar.LayoutType==eLayoutType.DockContainer && objBar.DockLine>=0 && lineDockCounts[objBar.DockLine]>0)
							{
								if(lineDockCounts[objBar.DockLine]==1)
									objBar.SplitDockHeight=0;
								if(objBar.SplitDockHeight!=0 && !(lineDockProcessed[objBar.DockLine]==1 && lineDockSize[objBar.DockLine]!=this.Height))
									frameSize=new Size(1,objBar.SplitDockHeight);// objBar.Size=new Size(1,objBar._SplitDockHeight);
								else
								{
									if(lineDockProcessed[objBar.DockLine]==1)
										frameSize=new Size(1,this.Height-iY); //objBar.Size=new Size(1,this.Height-iY);
									else
										frameSize=new Size(1,(this.Height-lineDockSize[objBar.DockLine])/(lineDefHeightCount[objBar.DockLine]==0?1:lineDefHeightCount[objBar.DockLine])); //objBar.Size=new Size(1,(this.Height-lineDockSize[objBar.DockLine])/lineDefHeightCount[objBar.DockLine]);
								}
								//objBar.Size=new Size(1,this.Height/lineDockCounts[objBar.DockLine]);
								
								// We need to switch to new line here
								if(iLastLine!=pDockInfo.DockLine && lineDockProcessed[objBar.DockLine]==lineDockCounts[objBar.DockLine] && iY>0)
								{
									iLastLine=pDockInfo.DockLine;
									iCurrentLine++;
									iX+=(iMaxSize+iVerSpacing);
									iY=0;
									iMaxSize=0;
									frameSize=new Size(1,this.Height); // objBar.Size=new Size(1,this.Height);
								}
								lineDockProcessed[objBar.DockLine]--;
								bSameLineDocking=true;
								arrBarsInSameLine.Add(objBar);
							}
							else
								frameSize=new Size(1,this.Height); //objBar.Size=new Size(1,this.Height);
						}
						else
							frameSize=new Size(1,this.Height-iY); //objBar.Size=new Size(1,this.Height-iY);

						objBar.RecalcSize(frameSize);

						// If item cannot fit move it to the next line if we are not on first line anyway
						// Or if item wants to be in new line anyway
						//if(i>0 && ((iY+objCtrl.Height>this.Height && objMinSize.Height+iY>this.Height) || pDockInfo.DockLine>iLastLine && iY>0))
						//if(i>0 && pDockInfo.DockLine>iLastLine && iY>0)
                        if(i>0 && pDockInfo.DockLine>iLastLine && iY>0)
                        {
                            // If it did not fit and we do same line docking we need to rearrange things
                            if(bSameLineDocking)
                            {
                                lineDockCounts[objBar.DockLine]=0;
                                lineDockProcessed[objBar.DockLine]=0;
                                bRepeatLayout=true;
                                break;
                            }

                            if(pDockInfo.DockLine>iLastLine)
                                bResetOffset=false;
                            else
                                bResetOffset=true;

                            // Remember Last Line
                            iLastLine=pDockInfo.DockLine;

                            // Go to the new line
                            iCurrentLine++;
                            iX+=(iMaxSize+iVerSpacing);
                            iY=0;
                            iMaxSize=0;

                            if(bResetOffset)
                                pDockInfo.DockOffset=0;

                            // See can we honor the DockOffset
                            if(pDockInfo.DockOffset>0)
                            {
                                if(pDockInfo.DockOffset+objCtrl.Height>this.Height)
                                    iY=this.Height-objCtrl.Height;
                                else
                                    iY=pDockInfo.DockOffset;
                                if(iY<0)
                                    iY=0;
                            }
                        }
                        else
						{
							if(iLastLine!=pDockInfo.DockLine)
								iLastLine=pDockInfo.DockLine;

							if(bResetOffset)
								pDockInfo.DockOffset=0;
							// Try to honor DockOffset
							if(!pDockInfo.Stretch)
							{
								if(pDockInfo.DockOffset>iY && pDockInfo.DockOffset+objCtrl.Height<=this.Height)
									iY=pDockInfo.DockOffset;
								else if(pDockInfo.DockOffset>iY && this.Height-objCtrl.Height>iY)
									iY=this.Height-objCtrl.Height;
							}
						}

						if(iY+objCtrl.Height>this.Height)
						{
							// We have to resize it to fit...
							if(objBar!=null)
								objBar.RecalcSize(new Size(objBar.Width,this.Height-iY));
							else
                                objCtrl.Height=this.Height-iY;
							// Then reset DockOffset
							pDockInfo.DockOffset=0;
						}
						//					else
						//					{
						//						if(pDockInfo.Stretch)
						//						{
						//							objCtrl.Height=this.Height;
						//							if(objBar!=null)
						//								objBar.RecalcSize();
						//						}
						//					}

						objCtrl.Location=new Point(iX,iY);

						iY+=(objCtrl.Height+iHorSpacing);
						if(bResetOffset || pDockInfo.DockLine!=iCurrentLine)
							pDockInfo.DockOffset=objCtrl.Top;
						// Get the max Width
						if(objCtrl.Width>iMaxSize)
							iMaxSize=objCtrl.Width;

						if(objBar!=null && objBar.ItemsContainer.WidthInternal>lineContainerMaxSize[iCurrentLine])
							lineContainerMaxSize[iCurrentLine]=objBar.ItemsContainer.WidthInternal;

						if(pDockInfo.Stretch)
						{
							if(objBar.LayoutType==eLayoutType.DockContainer && objBar.DockLine>=0 && lineDockProcessed[objBar.DockLine]>0)
							{
								pDockInfo.SetDockLine(iCurrentLine);
							}
							else
							{
								pDockInfo.SetDockLine(iCurrentLine);
								// Remember Last Line
								iLastLine=pDockInfo.DockLine;
								// Go to the new line
								iCurrentLine++;
								iX+=(iMaxSize+iVerSpacing);
								iY=0;
								iMaxSize=0;
							}
						}
						else
							pDockInfo.SetDockLine(iCurrentLine);
					}

					// Repaint control if needed
					//if(objOldSize.Width!=objCtrl.Width || objOldSize.Height!=objCtrl.Height)
					//	objCtrl.Refresh();
					//else
					//objCtrl.Update();
					bLastHidden=false;
				}
			} while(bRepeatLayout);

			// Make sure that bars that are stretching and sharing same line have same width/height
			foreach(Bar bar in arrBarsInSameLine)
			{
				if(m_DockOrientation==eOrientation.Horizontal)
				{
					if(bar.ItemsContainer.HeightInternal!=lineContainerMaxSize[bar.DockLine])
					{
						bar.ItemsContainer.MinHeight=lineContainerMaxSize[bar.DockLine];
						System.Drawing.Size frameSize=new Size(bar.Width,1);//bar.Size=new Size(bar.Width,1);
						bar.RecalcSize(frameSize);
					}
				}
				else
				{
					if(bar.ItemsContainer.WidthInternal!=lineContainerMaxSize[bar.DockLine])
					{
						bar.ItemsContainer.MinWidth=lineContainerMaxSize[bar.DockLine];
						System.Drawing.Size frameSize=new Size(1,bar.Height); //bar.Size=new Size(1,bar.Height);
						bar.RecalcSize(frameSize);
					}
				}
			}

			if(m_DockOrientation==eOrientation.Horizontal)
			{
                // Flip  for RTL
                //if (this.RightToLeft == RightToLeft.Yes)
                //{
                //    foreach (Control c in this.Controls)
                //    {
                //        c.Left = this.Width - (c.Left + c.Width);
                //    }
                //}
				if(this.Height!=iY+iMaxSize)
				{
					this.Height=iY+iMaxSize;
					if(this.Parent!=null)
						this.Parent.PerformLayout(this,"Height");
					this.Invalidate();
				}
			}
			else
			{
				if(this.Width!=iX+iMaxSize)
				{
					this.Width=iX+iMaxSize;
					if(this.Parent!=null)
						this.Parent.PerformLayout(this,"Width");
					this.Invalidate();
				}
			}
			if(!m_OversizeLayout)
			{
				if(this.Dock==DockStyle.Right && this.Location.X<0 || this.Dock==DockStyle.Left && this.Parent!=null && this.Right>this.Parent.Width || this.Dock==DockStyle.Top && this.Parent!=null && this.Bottom>this.Parent.Height || this.Dock==DockStyle.Bottom && this.Location.Y<0)
				{
					m_OversizeLayout=true;
					m_Layingout=false;
					EnforceClientMinSize();
					if(m_OversizeLayout)
						m_OversizeLayout=false;
				}
			}
			else
				m_OversizeLayout=false;

            if (this.Parent != null && this.Parent.Visible && m_OptimizeLayoutRedraw) // See WARNING at the beggining
			{
				NativeFunctions.SendMessage(this.Handle,NativeFunctions.WM_SETREDRAW,1,0);
				if(parentZOrder!=-1) this.Parent.Controls.SetChildIndex(this,parentZOrder);
				this.Parent.Invalidate(oldDockSiteSize,true);
				this.Parent.Invalidate(this.Bounds,true);
				this.Parent.Update();
			}
			m_Layingout=false;
		}

		protected override void OnParentChanged(EventArgs e)
		{
			base.OnParentChanged(e);
			Control parentControl=this.FindForm();
            if (parentControl != null)
                parentControl.Resize += new EventHandler(ParentResized);
		}
		private void ParentResized(object sender, EventArgs e)
		{
            Control parentControl = this.Parent;

            if (parentControl != null && (parentControl.Width == 0 || parentControl.Height == 0))
				return;

            if (parentControl != null && parentControl is Form && ((Form)parentControl).WindowState == FormWindowState.Minimized)
				return;

			EnforceClientMinSize();

		}
		private void EnforceClientMinSize()
		{
            if (this.Dock == DockStyle.Fill || this.Dock == DockStyle.None || m_DocumentDockContainer == null || this.Width == 0 || this.Height == 0)
                return;
            if(this.Parent!=null && (this.Parent.Width==0 || this.Parent.Height==0))
                return;

            bool isWidth = (this.Dock == DockStyle.Right || this.Dock == DockStyle.Left);

            int clientSize = isWidth?this.GetClientWidth():this.GetClientHeight();
            int minClientSize = 32;
            if (m_Owner != null) minClientSize = isWidth?m_Owner.MinimumClientSize.Width:m_Owner.MinimumClientSize.Height;

            if (clientSize > minClientSize)
            {
                if (m_OriginalSize > 0)
                {
                    int increase = m_OriginalSize - (isWidth?this.Width:this.Height);
                    if (clientSize - increase < minClientSize)
                        increase -= (minClientSize - (clientSize - increase));

                    if (increase > 0)
                    {
                        if(isWidth)
                            this.Width += increase;
                        else
                            this.Height += increase;
                    }

                    if (isWidth && this.Width >= m_OriginalSize || !isWidth && this.Height >= m_OriginalSize)
                        m_OriginalSize = 0;

                }
                return;
            }

            // Try to reduce size of the dock area to maintain the minimum client width
            this.GetDocumentUIManager().RootDocumentDockContainer.RefreshMinimumSize();
            DocumentDockUIManager dm = this.GetDocumentUIManager();
            Size minContainerSize = dm.GetContainerMinimumSize(dm.RootDocumentDockContainer);
            int minimumDockSize = isWidth ? minContainerSize.Width : minContainerSize.Height;
            
            int diff = minClientSize - clientSize;

            if (isWidth && this.Width - diff < minimumDockSize || !isWidth && this.Height - diff < minimumDockSize)
                diff = (isWidth ? this.Width : this.Height) - minimumDockSize;

            if (diff > 0)
            {
                if (m_OriginalSize == 0)
                    m_OriginalSize = isWidth ? this.Width : this.Height;
                if(isWidth)
                    this.Width -= diff;
                else
                    this.Height -= diff;
            }
		}

        internal int OriginalSize
        {
            get { return m_OriginalSize; }
            set
            {
                m_OriginalSize = value;
            }
        }
        

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			if(m_Layingout)
				return;

            Control parentControl = this.Parent;

            if (parentControl != null && (parentControl.Width == 0 || parentControl.Height == 0))
				return;

            if (parentControl != null && parentControl is Form && ((Form)parentControl).WindowState == FormWindowState.Minimized)
                return;

            if (parentControl != null)
			{
				if(m_Owner!=null && m_Owner.ParentForm!=null && m_Owner.ParentForm.WindowState!=FormWindowState.Minimized || m_Owner==null || m_Owner.ParentForm==null)
				{
					this.Invalidate(this.Region,false);
					LayoutBars();
				}
			}
		}

//		protected override void OnLayout(LayoutEventArgs e)
//		{
//			System.Windows.Forms.Form form=this.FindForm();
//			
//			if(form!=null && (form.Width==0 || form.Height==0))
//				return;
//
//			if(form!=null && form.ParentForm!=null)
//				form=form.ParentForm;
//
//			if(form!=null && (form.Width==0 || form.Height==0))
//				return;
//
//			if(form!=null && form.WindowState!=FormWindowState.Minimized)
//				return;
//
//			base.OnLayout(e);
//		}

        internal int GetClientWidth()
        {
            if (this.Parent == null)
                return 0;

            Control parentControl = this.Parent;
            int width = parentControl.ClientSize.Width;

            foreach (Control ctrl in parentControl.Controls)
            {
                if (ctrl.Visible && (ctrl.Dock == DockStyle.Left || ctrl.Dock == DockStyle.Right))
                    width -= ctrl.Width;
            }

            return width;
        }

        internal int GetClientHeight()
        {
            if (this.Parent == null)
                return 0;

            Control parentControl = this.Parent;
            int height = parentControl.ClientSize.Height;

            foreach (Control ctrl in parentControl.Controls)
            {
                if (ctrl.Visible && (ctrl.Dock == DockStyle.Top || ctrl.Dock == DockStyle.Bottom))
                    height -= ctrl.Height;
            }

            return height;
        }

		private int getDockLineWidth(int iStartPos, int iCurrentLine, bool bDirectionForward)
		{
			IDockInfo pDock;
			int iWidth=0, i=0;
			if(bDirectionForward)
			{
				for(i=iStartPos;i<this.Controls.Count;i++)
				{
					if(!this.Controls[i].Visible)
						continue;
					pDock=this.Controls[i] as IDockInfo;
					if(pDock.DockLine==iCurrentLine)
						iWidth+=this.Controls[i].Width;
					else
						break;
				}
			}
			else
			{
				for(i=iStartPos;i>=0;i--)
				{
					if(!this.Controls[i].Visible)
						continue;
					pDock=this.Controls[i] as IDockInfo;
					if(pDock.DockLine==iCurrentLine)
						iWidth+=this.Controls[i].Width;
					else
						break;
				}
			}

			return iWidth;
		}

		private int getDockLineHeight(int iStartPos, int iCurrentLine, bool bDirectionForward)
		{
			IDockInfo pDock;
			int iHeight=0, i=0;
			if(bDirectionForward)
			{
				for(i=iStartPos;i<this.Controls.Count;i++)
				{
					if(!this.Controls[i].Visible)
						continue;
					pDock=this.Controls[i] as IDockInfo;
					if(pDock.DockLine==iCurrentLine)
						iHeight+=this.Controls[i].Height;
					else
						break;
				}
			}
			else
			{
				for(i=iStartPos;i>=0;i--)
				{
					if(!this.Controls[i].Visible)
						continue;
					pDock=this.Controls[i] as IDockInfo;
					if(pDock.DockLine==iCurrentLine)
						iHeight+=this.Controls[i].Height;
					else
						break;
				}
			}

			return iHeight;
		}

		/*private void GetInsertPointAbove(ref DockSiteInfo pInfo, int iStartFrom, int iAboveLine, int iPosX, int iItemWidth, int iCtrlToDockIndex)
		{
			pInfo.InsertPosition=0;
			pInfo.DockLine=iAboveLine-1;

			if(iStartFrom==0 || iAboveLine==0)
			{
				// This is easy case
				if(iPosX+iItemWidth>this.Width)
				{
					pInfo.DockOffset=this.Width-iItemWidth;
					if(pInfo.DockOffset<0)
						pInfo.DockOffset=0;
				}
				else
					pInfo.DockOffset=iPosX;
				return;
			}

			// We will need to scan to find right insertion point
			Control objCtrl;
			IDockInfo pCtrlDockInfo;
			pInfo.InsertPosition=-1;
			int iStopIndex=0;
			if(iCtrlToDockIndex>0 && iCtrlToDockIndex<iStartFrom)
				iStopIndex=iCtrlToDockIndex;

			for(int i=iStartFrom;i>=iStopIndex;i--)
			{
				objCtrl=this.Controls[i];
				pCtrlDockInfo=objCtrl as IDockInfo;
				if(pCtrlDockInfo.DockLine==iAboveLine-1)
				{
					// Mouse inside this control, add after it
					if(objCtrl.Left<iPosX)
					{
						pInfo.InsertPosition=i+1;
					}
					else
					{
						// Mouse on the left side of the control, add before it
						if(i>0)
							pInfo.InsertPosition=i-1;
						break;
					}
					pInfo.DockOffset=iPosX;
				}
				else if(pCtrlDockInfo.DockLine<iAboveLine)
					break;
			}

			if(pInfo.InsertPosition<0)
			{
				// Could not determine it
				GetInsertPointAbove(ref pInfo,0,0,iPosX,iItemWidth,iCtrlToDockIndex);
			}
		}*/

		/*private void GetInsertPointBelow(ref DockSiteInfo pInfo, int iStartFrom, int iBelowLine, int iPosX, int iItemWidth, int iCtrlToDockIndex)
		{
			pInfo.InsertPosition=0;
			pInfo.DockLine=iBelowLine+1;
			int iStopIndex=this.Controls.Count;

			if(iStartFrom==this.Controls.Count)
			{
				// This is easy case
				if(iPosX+iItemWidth>this.Width)
				{
					pInfo.DockOffset=this.Width-iItemWidth;
					if(pInfo.DockOffset<0)
						pInfo.DockOffset=0;
				}
				else
					pInfo.DockOffset=iPosX;
				pInfo.InsertPosition=this.Controls.Count;

				return;
			}

			// We will need to scan to find right insertion point
			Control objCtrl;
			
			if(iCtrlToDockIndex>0)
				iStopIndex=iCtrlToDockIndex;
			
			objCtrl=null;
			IDockInfo pCtrlDockInfo;
			pInfo.InsertPosition=-1;
			for(int i=iStartFrom;i<iStopIndex;i++)
			{
				objCtrl=this.Controls[i];
				pCtrlDockInfo=objCtrl as IDockInfo;
				if(pCtrlDockInfo.DockLine==iBelowLine+1)
				{
					// Mouse inside this control, add after it
					if(objCtrl.Left<iPosX)
					{
						pInfo.InsertPosition=i+1;
					}
					else
					{
						// Mouse on the left side of the control, add before it
						if(i>0)
							pInfo.InsertPosition=i;
						break;
					}
					pInfo.DockOffset=iPosX;
				}
				else if(pCtrlDockInfo.DockLine>iBelowLine)
					break;
			}

			if(pInfo.InsertPosition<0)
			{
				// Could not determine it
				GetInsertPointBelow(ref pInfo,this.Controls.Count,iBelowLine,iPosX,iItemWidth,iCtrlToDockIndex);
			}
		}*/

		/// <summary>
		/// Returns the docking information for current position.
		/// </summary>
		/// <param name="pDock">Controls docking provider.</param>
		/// <param name="x">Horizontal assumed docking position.</param>
		/// <param name="y">Vertical assumed docking position.</param>
		/// <returns>Docking information.</returns>
		public DockSiteInfo GetDockSiteInfo(IDockInfo pDock, int x, int y)
		{
			if(m_DockOrientation==eOrientation.Horizontal)
			{
				return GetDockSiteInfoH(pDock,x,y);
			}
			else
			{
				return GetDockSiteInfoV(pDock,x,y);
			}
		}

		private DockSiteInfo GetDockSiteInfoH(IDockInfo pDock, int x, int y)
		{
			DockSiteInfo pInfo=new DockSiteInfo();
			Rectangle thisRect=new Rectangle(this.PointToScreen(new Point(0,0)),this.Size);
			Point pInsertionPoint=this.PointToClient(new Point(x,y));
			Control objCtrlToDock=pDock as Control;

			System.Drawing.Size objDockSize=pDock.PreferredDockSize(m_DockOrientation);
			System.Drawing.Size minDockSize=pDock.MinimumDockSize(m_DockOrientation);

			if(this.Height==0)
			{
				thisRect.Height=10;
				if(this.Dock==System.Windows.Forms.DockStyle.Bottom)
					thisRect.Y-=10;
			}
			else if(this.Width==0)
			{
				thisRect.Width=this.TopLevelControl.Width;
				if(this.Dock==System.Windows.Forms.DockStyle.Right)
					thisRect.X-=10;
			}

			// If mouse is not inside exit
			thisRect.Inflate(8,8); // m_DockMargin
			if(!thisRect.Contains(x,y) || pDock==null)
				return pInfo;

            bool bDockedHere=(pDock.DockedSite==this);
			int[] lineCount=new int[255];
			int iMaxDockLine=0;
			int iDockLine=-10;
			bool bDocked=false;
			foreach(Control objCtrl in this.Controls)
			{
				if(!objCtrl.Visible)
					continue;
				IDockInfo pCtrlDockInfo=objCtrl as IDockInfo;
				lineCount[pCtrlDockInfo.DockLine]++;
                iMaxDockLine=pCtrlDockInfo.DockLine;
			}
			
			int iLockedDockLine=-1;

			for(int i=0;i<this.Controls.Count;i++)
			{
				Control objCtrl=this.Controls[i];
				if(!objCtrl.Visible)
					continue;
				IDockInfo pCtrlDockInfo=objCtrl as IDockInfo;
				if(pCtrlDockInfo.LockDockPosition && objCtrlToDock is Bar && !((Bar)objCtrl).CanUndock)
				{
					if(this.Dock==DockStyle.Top)
                        iLockedDockLine=pCtrlDockInfo.DockLine;
					else if(iLockedDockLine==-1)
						iLockedDockLine=pCtrlDockInfo.DockLine;
				}
				if(pInsertionPoint.Y>objCtrl.Top+4 && ((pInsertionPoint.Y<objCtrl.Top+objDockSize.Height-4 && objDockSize.Height<=objCtrl.Height) || pInsertionPoint.Y<objCtrl.Bottom-4 && objDockSize.Height>objCtrl.Height))
				{
					if(pCtrlDockInfo.Stretch && objCtrl!=objCtrlToDock)
					{
						if(objCtrl is Bar && objCtrlToDock is Bar && ((Bar)objCtrl).LayoutType==eLayoutType.DockContainer && ((Bar)objCtrlToDock).LayoutType==eLayoutType.DockContainer && ((Bar)objCtrl).CanTearOffTabs)
						{
							if(!objCtrl.Bounds.Contains(pInsertionPoint))
								continue;
							iDockLine=pCtrlDockInfo.DockLine;
							if(((Bar)objCtrl).IsPositionOnDockTab(x,y))
							{
								pInfo.DockLine=pCtrlDockInfo.DockLine;
								pInfo.DockOffset=pCtrlDockInfo.DockOffset;
								pInfo.InsertPosition=i;
								pInfo.objDockSite=this;
								pInfo.DockSide=this.Dock;
								pInfo.TabDockContainer=objCtrl as Bar;
								return pInfo;
							}
						}
						else
						{
							iDockLine=pCtrlDockInfo.DockLine-1;
						}
					}
					else
					{
						if(objCtrl!=objCtrlToDock && objCtrlToDock is Bar && ((Bar)objCtrlToDock).Stretch)
						{
							int iInsertPosition=i;
							for(int iInsertIndex=i-1;iInsertIndex>=0;iInsertIndex--)
							{
								if(((IDockInfo)this.Controls[iInsertIndex]).DockLine==pDock.DockLine)
									iInsertPosition=iInsertIndex;
								else
									break;
							}
							pInfo.DockLine=pDock.DockLine;
							pInfo.DockOffset=pDock.DockOffset;
							pInfo.InsertPosition=iInsertPosition;
							pInfo.objDockSite=this;
							pInfo.DockSide=this.Dock;
							pInfo.NewLine=true;
							return pInfo;
						}
						else
							iDockLine=pCtrlDockInfo.DockLine;
					}

					break;
				}
				else if(objCtrl==objCtrlToDock && pInsertionPoint.Y<=objCtrl.Top+4 && pInsertionPoint.Y>objCtrl.Top && lineCount[pCtrlDockInfo.DockLine]>1)
				{
					int iInsertPosition=i;
					for(int iInsertIndex=i-1;iInsertIndex>=0;iInsertIndex--)
					{
						if(((IDockInfo)this.Controls[iInsertIndex]).DockLine==pDock.DockLine)
							iInsertPosition=iInsertIndex;
						else
							break;
					}
					pInfo.DockLine=pDock.DockLine;
					pInfo.DockOffset=pDock.DockOffset;
					pInfo.InsertPosition=iInsertPosition;
					pInfo.objDockSite=this;
					pInfo.DockSide=this.Dock;
					pInfo.NewLine=true;
					return pInfo;
				}
				else if(this.Dock==DockStyle.Top && objCtrl==objCtrlToDock && pInsertionPoint.Y>=objCtrl.Bottom-4 && pInsertionPoint.Y<objCtrl.Bottom-1 && lineCount[pCtrlDockInfo.DockLine]==1 && pDock.DockLine!=iMaxDockLine)
				{
					int iInsertPosition=i;
					for(int iInsertIndex=i+1;iInsertIndex<this.Controls.Count;iInsertIndex++)
					{
						if(((IDockInfo)this.Controls[iInsertIndex]).DockLine==pDock.DockLine+1 && (((IDockInfo)this.Controls[iInsertIndex]).DockOffset<=pDock.DockOffset || pDock.Stretch) || ((IDockInfo)this.Controls[iInsertIndex]).DockLine==pDock.DockLine)
							iInsertPosition=iInsertIndex;
						else
							break;
					}
					pInfo.DockLine=pDock.DockLine+1;
					pInfo.DockOffset=pDock.DockOffset;
					pInfo.InsertPosition=iInsertPosition;
					pInfo.objDockSite=this;
					pInfo.DockSide=this.Dock;
					return pInfo;
				}
			}

			if(iDockLine==-10)
			{
				if(pInsertionPoint.Y<=-5 && !(bDockedHere && pDock.DockLine==0 && lineCount[0]==1))
				{
					iDockLine=-1;
				}
				else if(!bDockedHere)
				{
					if(pInsertionPoint.Y>this.Height)
						iDockLine=iMaxDockLine+1;
					else
						iDockLine=iMaxDockLine;
				}
                else if (pInsertionPoint.Y > this.Height + 4 && !(bDockedHere && pDock.DockLine == iMaxDockLine && lineCount[iMaxDockLine] == 1))
				{
					iDockLine=iMaxDockLine+1;
				}
				
				if(iDockLine==-10)
				{
					pInfo.DockLine=pDock.DockLine;
					pInfo.DockOffset=pDock.DockOffset;
					pInfo.InsertPosition=this.Controls.GetChildIndex(objCtrlToDock);
					pInfo.objDockSite=this;
					pInfo.DockSide=this.Dock;
					return pInfo;
				}
			}

			if(iLockedDockLine!=-1)
			{
				if(this.Dock==DockStyle.Top && iDockLine<=iLockedDockLine)
					iDockLine=iLockedDockLine+1;
				else if(this.Dock==DockStyle.Bottom && iDockLine>=iLockedDockLine)
					iDockLine=iLockedDockLine-1;
			}

			// We have the docking line now, find right Docking offset for object to dock

			// First get the index of the docking line, default is at the end
			int iIndex=this.Controls.Count;
			for(int i=0;i<this.Controls.Count;i++)
			{
				Control objCtrl=this.Controls[i];
				if(!objCtrl.Visible)
					continue;
				IDockInfo pCtrlDockInfo=objCtrl as IDockInfo;
				if(pCtrlDockInfo.DockLine>=iDockLine)
				{
					iIndex=i;
					break;
				}
			}

			// Find the right insertion point based on X coordinate
			for(int i=iIndex;i<this.Controls.Count;i++)
			{
				Control objCtrl=this.Controls[i];
				if(!objCtrl.Visible)
					continue;
				IDockInfo pCtrlDockInfo=objCtrl as IDockInfo;
				if(pCtrlDockInfo.DockLine>iDockLine)
				{
					pInfo.InsertPosition=i;
					if(bDockedHere && i>0 && this.Controls.GetChildIndex(objCtrlToDock)<i)
						pInfo.InsertPosition=i-1;
					if(pInsertionPoint.X+objDockSize.Width>this.Width)
						pInfo.DockOffset=this.Width-objDockSize.Width;
					else
						pInfo.DockOffset=pInsertionPoint.X;
					pInfo.DockLine=iDockLine;
					pInfo.DockSide=this.Dock;
					pInfo.objDockSite=this;
					bDocked=true;
					break;
				}
				if(objCtrl.Bounds.Contains(pInsertionPoint))
				{
					if(objCtrl!=objCtrlToDock)
					{
						if(pInsertionPoint.X>=objCtrl.Left && pInsertionPoint.X<=objCtrl.Left+10)
						{
							// This is right position, insert before docked control
							pInfo.InsertPosition=i;
							if(bDockedHere && i>0 && this.Controls.GetChildIndex(objCtrlToDock)<i)
								pInfo.InsertPosition=i-1;
							pInfo.DockOffset=0;
							pInfo.DockLine=iDockLine;
							pInfo.DockSide=this.Dock;
							pInfo.objDockSite=this;
						}
						else
						{
							// Insert after docked control
							int iTmp=getDockLineWidth(i,iDockLine,false);
							if(iTmp+objDockSize.Width>=this.Width || pInsertionPoint.X<iTmp)
								pInfo.DockOffset=objCtrl.Right;
							else
								pInfo.DockOffset=pInsertionPoint.X;

							pInfo.InsertPosition=i+1;

							if(pDock.Stretch)
							{
								//if(iDockLine<iMaxDockLine)
								//	iDockLine++;
								pInfo.DockOffset=pDock.DockOffset;
							}
							if(bDockedHere && i>0 && this.Controls.GetChildIndex(objCtrlToDock)<i)
								pInfo.InsertPosition=i;

							pInfo.DockLine=iDockLine;
							pInfo.DockSide=this.Dock;
							pInfo.objDockSite=this;
						}
					}
					else
					{
						// This just tries to move control to the right, insertion point is over the object that needs to be docked
						pInfo.InsertPosition=i;
						pInfo.DockLine=iDockLine;
						pInfo.DockSide=this.Dock;
						pInfo.objDockSite=this;

						int iTmp=getDockLineWidth(i+1,pDock.DockLine,true);
						
						if(iTmp+objDockSize.Width+pInsertionPoint.X>=this.Width)
							pInfo.DockOffset=this.Width-iTmp-objDockSize.Width;
						else
							pInfo.DockOffset=pInsertionPoint.X;

						if(pInfo.DockOffset<0)
							pInfo.DockOffset=0;
					}
					bDocked=true;
					break;
				}
				else if(pInsertionPoint.X<objCtrl.Left || pInsertionPoint.Y<objCtrl.Top && iLockedDockLine<0)
				{
					// Dock it before this control, insertion point is in empty space before docked control
					pInfo.InsertPosition=i;
					if(bDockedHere && i>0 && this.Controls.GetChildIndex(objCtrlToDock)<i)
						pInfo.InsertPosition=i-1;
					if(pDock.Stretch)
						pInfo.DockOffset=pDock.DockOffset;
					else
						pInfo.DockOffset=pInsertionPoint.X;
					pInfo.DockLine=iDockLine;
					pInfo.DockSide=this.Dock;
					pInfo.objDockSite=this;
					bDocked=true;
					break;
				}
			}

			if(!bDocked)
			{
				// Add it to the end
				if(iDockLine>=0)
				{
					if(bDockedHere)
						pInfo.InsertPosition=this.Controls.Count-1;
					else
						pInfo.InsertPosition=this.Controls.Count;

				}
				else
					pInfo.InsertPosition=0;

				if(pInsertionPoint.X+objDockSize.Width>this.Width)
					pInfo.DockOffset=this.Width-objDockSize.Width;
				else
					pInfo.DockOffset=pInsertionPoint.X;
				if(iDockLine>=0)
				{
					pInfo.DockLine=iDockLine;
				}
				else
					pInfo.DockLine=-1;

				pInfo.DockSide=this.Dock;
				pInfo.objDockSite=this;
			}

			// Snap it to the left
			if(pInfo.DockOffset<10)
				pInfo.DockOffset=0;

			if(m_DockOrientation==eOrientation.Horizontal)
			{
				if(objDockSize.Width>this.Width)
					pInfo.DockedWidth=this.Width;
				else
					pInfo.DockedWidth=objDockSize.Width;

				pInfo.DockedHeight=objDockSize.Height;
			}
			else
			{
				if(objDockSize.Height>this.Height)
					pInfo.DockedHeight=this.Height;
				else
					pInfo.DockedHeight=objDockSize.Height;

				pInfo.DockedWidth=objDockSize.Width;
			}

			return pInfo;
		}

		private DockSiteInfo GetDockSiteInfoV(IDockInfo pDock, int x, int y)
		{
			DockSiteInfo pInfo=new DockSiteInfo();
			Rectangle thisRect=new Rectangle(this.PointToScreen(new Point(0,0)),this.Size);
			Point pInsertionPoint=this.PointToClient(new Point(x,y));
			Control objCtrlToDock=pDock as Control;

			System.Drawing.Size objDockSize=pDock.PreferredDockSize(m_DockOrientation);

			if(this.Height==0)
			{
				thisRect.Height=10;
				if(this.Dock==System.Windows.Forms.DockStyle.Bottom)
					thisRect.Y-=10;
			}
			else if(this.Width==0)
			{
				thisRect.Width=10;
				if(this.Dock==System.Windows.Forms.DockStyle.Right)
					thisRect.X-=10;
			}

			// If mouse is not inside exit
			thisRect.Inflate(8,8); // m_DockMargin
			if(!thisRect.Contains(x,y) || pDock==null)
				return pInfo;

			bool bDockedHere=(pDock.DockedSite==this);
			int[] lineCount=new int[255];
			int iMaxDockLine=0;
			int iDockLine=-10;
			bool bDocked=false;
			foreach(Control objCtrl in this.Controls)
			{
				if(!objCtrl.Visible)
					continue;
				IDockInfo pCtrlDockInfo=objCtrl as IDockInfo;
				lineCount[pCtrlDockInfo.DockLine]++;
				iMaxDockLine=pCtrlDockInfo.DockLine;
			}

			int iLockedDockLine=-1;

			for(int i=0;i<this.Controls.Count;i++)
			{
				Control objCtrl=this.Controls[i];
				if(!objCtrl.Visible)
					continue;
				IDockInfo pCtrlDockInfo=objCtrl as IDockInfo;
				if(pInsertionPoint.X>objCtrl.Left+4 && ((pInsertionPoint.X<objCtrl.Left+objDockSize.Width-4 && objDockSize.Width<=objCtrl.Width) || pInsertionPoint.X<objCtrl.Right-4 && objDockSize.Width>objCtrl.Width))
				{
					if(pCtrlDockInfo.Stretch && objCtrl!=objCtrlToDock)
					{
						if(objCtrl is Bar && objCtrlToDock is Bar && ((Bar)objCtrl).LayoutType==eLayoutType.DockContainer && ((Bar)objCtrlToDock).LayoutType==eLayoutType.DockContainer  && ((Bar)objCtrl).CanTearOffTabs)
						{
							if(!objCtrl.Bounds.Contains(pInsertionPoint))
								continue;
							iDockLine=pCtrlDockInfo.DockLine;
							if(((Bar)objCtrl).IsPositionOnDockTab(x,y))
							{
								pInfo.DockLine=pCtrlDockInfo.DockLine;
								pInfo.DockOffset=pCtrlDockInfo.DockOffset;
								pInfo.InsertPosition=i;
								pInfo.objDockSite=this;
								pInfo.DockSide=this.Dock;
								pInfo.TabDockContainer=objCtrl as Bar;
								return pInfo;
							}
						}
						else
						{
							iDockLine=pCtrlDockInfo.DockLine-1;
						}
					}
					else
					{
						iDockLine=pCtrlDockInfo.DockLine;
					}
					break;
				}
				else if(objCtrl==objCtrlToDock && pInsertionPoint.X<=objCtrl.Left+4 && pInsertionPoint.X>objCtrl.Left && lineCount[pCtrlDockInfo.DockLine]>1)
				{
					int iInsertPosition=i;
					for(int iInsertIndex=i-1;iInsertIndex>=0;iInsertIndex--)
					{
						if(((IDockInfo)this.Controls[iInsertIndex]).DockLine==pDock.DockLine)
							iInsertPosition=iInsertIndex;
						else
							break;
					}
					pInfo.DockLine=pDock.DockLine;
					pInfo.DockOffset=pDock.DockOffset;
					pInfo.InsertPosition=iInsertPosition;
					pInfo.objDockSite=this;
					pInfo.DockSide=this.Dock;
					pInfo.NewLine=true;
					return pInfo;
				}
				else if(this.Dock==DockStyle.Left && objCtrl==objCtrlToDock && pInsertionPoint.X>=objCtrl.Right-4 && pInsertionPoint.X<objCtrl.Right && lineCount[pCtrlDockInfo.DockLine]==1 && pDock.DockLine!=iMaxDockLine)
				{
					int iInsertPosition=i;
					for(int iInsertIndex=i+1;iInsertIndex<this.Controls.Count;iInsertIndex++)
					{
						if(((IDockInfo)this.Controls[iInsertIndex]).DockLine==pDock.DockLine+1 && (((IDockInfo)this.Controls[iInsertIndex]).DockOffset<=pDock.DockOffset || pDock.Stretch) || ((IDockInfo)this.Controls[iInsertIndex]).DockLine==pDock.DockLine)
							iInsertPosition=iInsertIndex;
						else
							break;
					}
					pInfo.DockLine=pDock.DockLine+1;
					pInfo.DockOffset=pDock.DockOffset;
					pInfo.InsertPosition=iInsertPosition;
					pInfo.objDockSite=this;
					pInfo.DockSide=this.Dock;
					return pInfo;
				}
			}

			if(iDockLine==-10)
			{
				if(pInsertionPoint.X<=-5 && !(bDockedHere && pDock.DockLine==0 && lineCount[0]==1))
				{
					iDockLine=-1;
				}
				else if(!bDockedHere)
				{
					if(pInsertionPoint.X>this.Width)
						iDockLine=iMaxDockLine+1;
					else
						iDockLine=iMaxDockLine;
				}
				else if(pInsertionPoint.X>this.Width+4 && !(bDockedHere && pDock.DockLine==iMaxDockLine && lineCount[iMaxDockLine]==1))
				{
					iDockLine=iMaxDockLine+1;
				}

				if(iDockLine==-10)
				{
					pInfo.DockLine=pDock.DockLine;
					pInfo.DockOffset=pDock.DockOffset;
					pInfo.InsertPosition=this.Controls.GetChildIndex(objCtrlToDock);
					pInfo.objDockSite=this;
					pInfo.DockSide=this.Dock;
					return pInfo;
				}
			}

			if(iLockedDockLine!=-1)
			{
				if(this.Dock==DockStyle.Top && iDockLine<=iLockedDockLine)
					iDockLine=iLockedDockLine+1;
				else if(this.Dock==DockStyle.Bottom && iDockLine>=iLockedDockLine)
					iDockLine=iLockedDockLine-1;
			}

			// We have the docking line now, find right Docking offset for object to dock

			// First get the index of the docking line, default is at the end
			int iIndex=this.Controls.Count;
			for(int i=0;i<this.Controls.Count;i++)
			{
				Control objCtrl=this.Controls[i];
				if(!objCtrl.Visible)
					continue;
				IDockInfo pCtrlDockInfo=objCtrl as IDockInfo;
				if(pCtrlDockInfo.DockLine>=iDockLine)
				{
					iIndex=i;
					break;
				}
			}

			// Find the right insertion point based on X coordinate
			for(int i=iIndex;i<this.Controls.Count;i++)
			{
				Control objCtrl=this.Controls[i];
				if(!objCtrl.Visible)
					continue;
				IDockInfo pCtrlDockInfo=objCtrl as IDockInfo;
				if(pCtrlDockInfo.DockLine>iDockLine)
				{
					pInfo.InsertPosition=i;
					if(bDockedHere && i>0 && this.Controls.GetChildIndex(objCtrlToDock)<i)
						pInfo.InsertPosition=i-1;
					if(pInsertionPoint.Y+objDockSize.Height>this.Height)
						pInfo.DockOffset=this.Height-objDockSize.Height;
					else
						pInfo.DockOffset=pInsertionPoint.Y;
					pInfo.DockLine=iDockLine;
					pInfo.DockSide=this.Dock;
					pInfo.objDockSite=this;
					bDocked=true;
					break;
				}
				if(objCtrl.Bounds.Contains(pInsertionPoint))
				{
					if(objCtrl!=objCtrlToDock)
					{
						if(pInsertionPoint.Y>=objCtrl.Top && pInsertionPoint.Y<=objCtrl.Top+10)
						{
							// This is right position, insert before docked control
							pInfo.InsertPosition=i;
							if(bDockedHere && i>0 && this.Controls.GetChildIndex(objCtrlToDock)<i)
								pInfo.InsertPosition=i-1;
							pInfo.DockOffset=0;
							pInfo.DockLine=iDockLine;
							pInfo.DockSide=this.Dock;
							pInfo.objDockSite=this;
						}
						else
						{
							// Insert after docked control
							int iTmp=getDockLineHeight(i,iDockLine,false);
							if(iTmp+objDockSize.Height>=this.Height || pInsertionPoint.Y<iTmp)
								pInfo.DockOffset=objCtrl.Bottom;
							else
								pInfo.DockOffset=pInsertionPoint.Y;
							
							pInfo.InsertPosition=i+1;

							if(pDock.Stretch)
							{
								//if(iDockLine<iMaxDockLine)
								//	iDockLine++;
								pInfo.DockOffset=pDock.DockOffset;
							}
							if(bDockedHere && i>0 && this.Controls.GetChildIndex(objCtrlToDock)<i)
								pInfo.InsertPosition=i;

							pInfo.DockLine=iDockLine;
							pInfo.DockSide=this.Dock;
							pInfo.objDockSite=this;
						}
					}
					else
					{
						// This just tries to move control to the right, insertion point is over the object that needs to be docked
						pInfo.InsertPosition=i;
						pInfo.DockLine=iDockLine;
						pInfo.DockSide=this.Dock;
						pInfo.objDockSite=this;

						int iTmp=getDockLineHeight(i+1,pDock.DockLine,true);
						
						if(iTmp+objDockSize.Height+pInsertionPoint.Y>=this.Height)
							pInfo.DockOffset=this.Height-iTmp-objDockSize.Height;
						else
							pInfo.DockOffset=pInsertionPoint.Y;

						if(pInfo.DockOffset<0)
							pInfo.DockOffset=0;
					}
					bDocked=true;
					break;
				}
				else if(pInsertionPoint.Y<objCtrl.Top || pInsertionPoint.X<objCtrl.Left && iLockedDockLine<0)
				{
					// Dock it before this control, insertion point is in empty space before docked control
					pInfo.InsertPosition=i;
					if(bDockedHere && i>0 && this.Controls.GetChildIndex(objCtrlToDock)<i)
						pInfo.InsertPosition=i-1;
					if(pDock.Stretch)
						pInfo.DockOffset=pDock.DockOffset;
					else
						pInfo.DockOffset=pInsertionPoint.Y;
					pInfo.DockLine=iDockLine;
					pInfo.DockSide=this.Dock;
					pInfo.objDockSite=this;
					bDocked=true;
					break;
				}
			}

			if(!bDocked)
			{
				// Add it to the end
				if(iDockLine>=0)
				{
					if(bDockedHere)
						pInfo.InsertPosition=this.Controls.Count-1;
					else
						pInfo.InsertPosition=this.Controls.Count;

				}
				else
					pInfo.InsertPosition=0;

				if(pInsertionPoint.Y+objDockSize.Height>this.Height)
					pInfo.DockOffset=this.Height-objDockSize.Height;
				else
					pInfo.DockOffset=pInsertionPoint.Y;
				if(iDockLine>=0)
				{
					pInfo.DockLine=iDockLine;
				}
				else
					pInfo.DockLine=-1;

				pInfo.DockSide=this.Dock;
				pInfo.objDockSite=this;
			}

			// Snap it to the left
			if(pInfo.DockOffset<10)
				pInfo.DockOffset=0;

			if(m_DockOrientation==eOrientation.Horizontal)
			{
				if(objDockSize.Width>this.Width)
					pInfo.DockedWidth=this.Width;
				else
					pInfo.DockedWidth=objDockSize.Width;

				pInfo.DockedHeight=objDockSize.Height;
			}
			else
			{
				if(objDockSize.Height>this.Height)
					pInfo.DockedHeight=this.Height;
				else
					pInfo.DockedHeight=objDockSize.Height;

				pInfo.DockedWidth=objDockSize.Width;
			}

			return pInfo;
		}

		/// <summary>
		/// Estimates the docking position and size of a given bar for outline purposes during drag&drop
		/// </summary>
		/// <param name="bar">Bar to find estimated docking position and size for</param>
		/// <param name="pDockInfo">Docking information</param>
		/// <returns>Rectangle screen coordinates.</returns>
		[EditorBrowsable(EditorBrowsableState.Never)]
        public Rectangle GetBarDockRectangle(Bar bar, ref DockSiteInfo pDockInfo)
		{
			if(pDockInfo.objDockSite==null)
				return Rectangle.Empty;

			if(pDockInfo.objDockSite.Dock==DockStyle.Fill || m_DocumentDockContainer!=null)
			{
				DocumentDockUIManager m_UIManager=GetDocumentUIManager();
				return m_UIManager.GetDockPreviewRectangle(bar,ref pDockInfo);
			}
			
			Rectangle r=Rectangle.Empty;

			if(pDockInfo.InsertPosition<0 && pDockInfo.NewLine)
				pDockInfo.DockLine=-1;
			else if(pDockInfo.InsertPosition>=this.Controls.Count && pDockInfo.NewLine)
                pDockInfo.DockLine=999;

			if(pDockInfo.TabDockContainer!=null)
			{
				r=pDockInfo.TabDockContainer.Bounds;
				r.Location=this.PointToScreen(r.Location);
			}
			else if(pDockInfo.DockLine==999 || pDockInfo.DockLine==-1) // Handle the edge cases
			{
				int fullSizeIndex=-1;
				int partialSizeIndex=-1;
				if(pDockInfo.FullSizeDock)
					fullSizeIndex=this.GetFullSizeIndex();
				else if(pDockInfo.PartialSizeDock)
					partialSizeIndex=this.GetPartialSizeIndex();

				// Docks the bar to the edge
				switch(this.Dock)
				{
					case DockStyle.Top:
					{
						r.Width=this.ClientRectangle.Width;
						r.Height=bar.GetBarDockedSize(eOrientation.Horizontal);

						if(fullSizeIndex>=0)
						{
							r.Width+=GetSiteZOrderSize(m_Owner.LeftDockSite,true).Width;
							r.Width+=GetSiteZOrderSize(m_Owner.RightDockSite,true).Width;
							pDockInfo.DockSiteZOrderIndex=fullSizeIndex;
						}
						else if(partialSizeIndex>=0)
						{
							// Reduce by the size of the left and right dock site
							r.Width-=GetSiteZOrderSize(m_Owner.LeftDockSite,false).Width;
							r.Width-=GetSiteZOrderSize(m_Owner.RightDockSite,false).Width;
							pDockInfo.DockSiteZOrderIndex=partialSizeIndex;
							
						}

						Point p=Point.Empty;
						if(pDockInfo.DockLine==-1)
						{
							p=this.PointToScreen(this.ClientRectangle.Location);
						}
						else
						{
							p=this.PointToScreen(new Point(this.ClientRectangle.X,this.ClientRectangle.Bottom));
							p.Y++;
						}
						if(fullSizeIndex>=0)
							p.X-=GetSiteZOrderSize(m_Owner.LeftDockSite,true).Width;
						else if(partialSizeIndex>=0)
							p.X+=GetSiteZOrderSize(m_Owner.LeftDockSite,false).Width;
						r.Location=p;
						break;
					}
					case DockStyle.Bottom:
					{
						r.Width=this.ClientRectangle.Width;
						r.Height=bar.GetBarDockedSize(eOrientation.Horizontal);
						if(fullSizeIndex>=0)
						{
							r.Width+=GetSiteZOrderSize(m_Owner.LeftDockSite,true).Width;
							r.Width+=GetSiteZOrderSize(m_Owner.RightDockSite,true).Width;
							pDockInfo.DockSiteZOrderIndex=fullSizeIndex;
						}
						else if(partialSizeIndex>=0)
						{
							// Reduce by the size of the left and right dock site
							r.Width-=GetSiteZOrderSize(m_Owner.LeftDockSite,false).Width;
							r.Width-=GetSiteZOrderSize(m_Owner.RightDockSite,false).Width;
							pDockInfo.DockSiteZOrderIndex=partialSizeIndex;
							
						}

						Point p=Point.Empty;
						if(pDockInfo.DockLine==-1)
						{
							p=this.PointToScreen(new Point(this.ClientRectangle.X,this.ClientRectangle.Y-r.Height));
						}
						else
						{
							p=this.PointToScreen(new Point(this.ClientRectangle.X,this.ClientRectangle.Bottom-r.Height));
							p.Y++;
						}
						if(fullSizeIndex>=0)
							p.X-=GetSiteZOrderSize(m_Owner.LeftDockSite,true).Width;
						else if(partialSizeIndex>=0)
							p.X+=GetSiteZOrderSize(m_Owner.LeftDockSite,false).Width;
						r.Location=p;
						break;
					}
					case DockStyle.Right:
					{
						r.Height=this.ClientRectangle.Height;

						if(fullSizeIndex>=0)
						{
							r.Height+=GetSiteZOrderSize(m_Owner.TopDockSite,true).Height;
							r.Height+=GetSiteZOrderSize(m_Owner.BottomDockSite,true).Height;
							pDockInfo.DockSiteZOrderIndex=fullSizeIndex;
						}
						else if(partialSizeIndex>=0)
						{
							// Reduce by the size of the top and bottom dock site
							r.Height-=GetSiteZOrderSize(m_Owner.TopDockSite,false).Height;
							r.Height-=GetSiteZOrderSize(m_Owner.BottomDockSite,false).Height;
							pDockInfo.DockSiteZOrderIndex=partialSizeIndex;
							
						}

						r.Width=bar.GetBarDockedSize(eOrientation.Vertical);
						Point p=Point.Empty;
                        if (pDockInfo.DockLine == -1)
						{
							p=this.PointToScreen(new Point(this.ClientRectangle.X-r.Width,this.ClientRectangle.Y));
						}
						else
						{
							p=this.PointToScreen(new Point(this.ClientRectangle.Right-r.Width,this.ClientRectangle.Y));
							p.X--;	
						}
						if(fullSizeIndex>=0)
							p.Y-=GetSiteZOrderSize(m_Owner.TopDockSite,true).Height;
						else if(partialSizeIndex>=0)
							p.Y+=GetSiteZOrderSize(m_Owner.TopDockSite,false).Height;
						r.Location=p;
						break;
					}
					default:
					{
						r.Height=this.ClientRectangle.Height;
						if(fullSizeIndex>=0)
						{
							r.Height+=GetSiteZOrderSize(m_Owner.TopDockSite,true).Height;
							r.Height+=GetSiteZOrderSize(m_Owner.BottomDockSite,true).Height;
							pDockInfo.DockSiteZOrderIndex=fullSizeIndex;
						}
						else if(partialSizeIndex>=0)
						{
							// Reduce by the size of the top and bottom dock site
							r.Height-=GetSiteZOrderSize(m_Owner.TopDockSite,false).Height;
							r.Height-=GetSiteZOrderSize(m_Owner.BottomDockSite,false).Height;
							pDockInfo.DockSiteZOrderIndex=partialSizeIndex;
							
						}
						r.Width=bar.GetBarDockedSize(eOrientation.Vertical);
						
						Point p=Point.Empty;
						if(pDockInfo.DockLine==-1)
						{
							p=this.PointToScreen(new Point(this.ClientRectangle.X,this.ClientRectangle.Y));
						}
						else
						{
							p=this.PointToScreen(new Point(this.ClientRectangle.Right,this.ClientRectangle.Y));
							p.X++;
						}
						if(fullSizeIndex>=0)
							p.Y-=GetSiteZOrderSize(m_Owner.TopDockSite,true).Height;
						else if(partialSizeIndex>=0)
							p.Y+=GetSiteZOrderSize(m_Owner.TopDockSite,false).Height;
						r.Location=p;
						break;
					}
				}
			}
			else
			{
				Bar barInsertPos=null;
				if(pDockInfo.InsertPosition>=0 && pDockInfo.InsertPosition<this.Controls.Count)
				{
					barInsertPos=this.Controls[pDockInfo.InsertPosition] as Bar;
					if(!pDockInfo.NewLine)
					{
						// Docking on the same line the DockLine should match
						if(barInsertPos.DockLine>pDockInfo.DockLine)
						{
							int ipos=pDockInfo.InsertPosition-1;
							while(ipos>=0)
							{
								Bar b=this.Controls[ipos] as Bar;
								if(b!=null && b.LayoutType==eLayoutType.DockContainer && b.DockLine==pDockInfo.DockLine)
								{
									barInsertPos=b;
									break;
								}
							}
						}
						else if(barInsertPos.DockLine<pDockInfo.DockLine)
						{
							int ipos=pDockInfo.InsertPosition+1;
							while(ipos<this.Controls.Count)
							{
								Bar b=this.Controls[ipos] as Bar;
								if(b!=null && b.LayoutType==eLayoutType.DockContainer && b.DockLine==pDockInfo.DockLine)
								{
									barInsertPos=b;
									break;
								}
							}
						}
					}
				}
				else if(pDockInfo.InsertPosition<0)
					barInsertPos=this.Controls[0] as Bar;
				else if(pDockInfo.InsertPosition>=this.Controls.Count)
					barInsertPos=this.Controls[this.Controls.Count-1] as Bar;

				int i=pDockInfo.InsertPosition+1;
				while(i<this.Controls.Count && (bar==null || barInsertPos.LayoutType!=eLayoutType.DockContainer || !barInsertPos.Visible))
				{
					barInsertPos=this.Controls[i] as Bar;
					i++;
				}

				if(barInsertPos==null)
					return r;

				// Docks the bar to the edge
				switch(this.Dock)
				{
					case DockStyle.Top:
					case DockStyle.Bottom:
					{
						r.Height=bar.GetBarDockedSize(eOrientation.Horizontal);
						if(pDockInfo.NewLine || barInsertPos==bar)
						{
							r.Width=barInsertPos.Width;
							if(r.Width==0)
								r.Width=this.Width;
							Point p=this.PointToScreen(new Point(barInsertPos.Left,barInsertPos.Top));
							r.Location=p;
						}
						else
						{
							if(barInsertPos.DockOffset>pDockInfo.DockOffset)
							{
								// Left half
								r.Height=barInsertPos.Height;
								r.Width=barInsertPos.Width/2;
								Point p=this.PointToScreen(new Point(barInsertPos.Left,barInsertPos.Top));
								r.Location=p;
							}
							else
							{
								// Split Right Half
								r.Height=barInsertPos.Height;
								r.Width=barInsertPos.Width/2;
								Point p=this.PointToScreen(new Point(barInsertPos.Left+r.Width,barInsertPos.Top));
								r.Location=p;
							}
						}
						break;
					}
					default: // Covers both Right and Left
					{
						r.Width=bar.GetBarDockedSize(eOrientation.Vertical);
						if(pDockInfo.NewLine || barInsertPos==bar)
						{
							r.Height=barInsertPos.Height;
							Point p=this.PointToScreen(new Point(barInsertPos.Left,barInsertPos.Top));
							r.Location=p;
						}
						else
						{
							if(barInsertPos.DockOffset<pDockInfo.DockOffset)
							{
								// Bottom half
								r.Width=barInsertPos.Width;
								r.Height=barInsertPos.Height/2;
								Point p=this.PointToScreen(new Point(barInsertPos.Left,barInsertPos.Top+r.Height));
								r.Location=p;
							}
							else
							{
								// Split Top Half
								r.Width=barInsertPos.Width;
								r.Height=barInsertPos.Height/2;
								Point p=this.PointToScreen(new Point(barInsertPos.Left,barInsertPos.Top));
								r.Location=p;
							}
						}
						break;
					}
				}
			}

			return r;
		}

		internal System.Drawing.Size GetSiteZOrderSize(DockSite dockSite, bool bFullSize)
		{
			if(dockSite==null || dockSite.Parent==null)
				return Size.Empty;
            
			System.Drawing.Size size=Size.Empty;

			if(bFullSize)
			{
				if(dockSite.Parent.Controls.IndexOf(dockSite)>this.Parent.Controls.IndexOf(this))
					size=dockSite.Size;
			}
			else
			{
				if(this.Parent.Controls.IndexOf(this)>dockSite.Parent.Controls.IndexOf(dockSite))
					size=dockSite.Size;
			}
			return size;
		}

		internal int GetPartialSizeIndex()
		{
			if(this.Parent==null)
				return -1;
			int index=1;
			if(this.Dock==DockStyle.Left || this.Dock==DockStyle.Right)
			{
				if(m_Owner==null || m_Owner.TopDockSite==null || m_Owner.BottomDockSite==null || m_Owner.TopDockSite.Parent==null || m_Owner.BottomDockSite.Parent==null)
					return -1;

				index=Math.Min(m_Owner.TopDockSite.Parent.Controls.IndexOf(m_Owner.TopDockSite),m_Owner.BottomDockSite.Parent.Controls.IndexOf(m_Owner.BottomDockSite));
			}
			else
			{
				if(m_Owner==null || m_Owner.LeftDockSite==null || m_Owner.RightDockSite==null || m_Owner.LeftDockSite.Parent==null || m_Owner.RightDockSite.Parent==null)
					return -1;

				index=Math.Min(m_Owner.LeftDockSite.Parent.Controls.IndexOf(m_Owner.LeftDockSite),m_Owner.RightDockSite.Parent.Controls.IndexOf(m_Owner.RightDockSite));
			}

			if(this.Parent.Controls.IndexOf(this)<index)
				return -1;
			return index;
		}

		internal int GetFullSizeIndex()
		{
			if(this.Parent==null)
				return -1;
			int index=-1;
			if(this.Dock==DockStyle.Left || this.Dock==DockStyle.Right)
			{
				if(m_Owner==null || m_Owner.TopDockSite==null || m_Owner.BottomDockSite==null || m_Owner.TopDockSite.Parent==null || m_Owner.BottomDockSite.Parent==null)
					return -1;

				index=Math.Max(m_Owner.TopDockSite.Parent.Controls.IndexOf(m_Owner.TopDockSite),m_Owner.BottomDockSite.Parent.Controls.IndexOf(m_Owner.BottomDockSite));
			}
			else
			{
				if(m_Owner==null || m_Owner.LeftDockSite==null || m_Owner.RightDockSite==null || m_Owner.LeftDockSite.Parent==null || m_Owner.RightDockSite.Parent==null)
					return -1;

				index=Math.Max(m_Owner.LeftDockSite.Parent.Controls.IndexOf(m_Owner.LeftDockSite),m_Owner.RightDockSite.Parent.Controls.IndexOf(m_Owner.RightDockSite));
			}
			if(this.Parent.Controls.IndexOf(this)>index)
				return -1;
			return index;
		}

		internal eOrientation DockOrientation
		{
			get{return m_DockOrientation;}
		}

		/// <summary>
		/// Undocks the control from the site.
		/// </summary>
		/// <param name="objBar">Control to undock.</param>
		internal void RemoveBar(Control objBar)
		{
			if(!this.Contains(objBar))
				return;
			this.Controls.Remove(objBar);
			LayoutBars();
		}

		internal bool IsDocumentDock
		{
			get
			{
				return (this.Dock==DockStyle.Fill || m_DocumentDockContainer!=null);
			}
		}
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
            //if(!IsDocumentDock)
            //    return;
			if(m_DocumentDockContainer!=null)
				m_DocumentUIManager.OnMouseMove(e);
		}
		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
            //if(!IsDocumentDock)
            //    return;
			if(m_DocumentDockContainer!=null)
				m_DocumentUIManager.OnMouseLeave();
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if(e.Button==MouseButtons.Right && m_DocumentDockContainer == null)
			{
				IOwnerBarSupport ownersupport=m_Owner as IOwnerBarSupport;
				if(ownersupport!=null)
					ownersupport.BarContextMenu(this,e);
			}

            //if(!IsDocumentDock)
            //    return;
			if(m_DocumentDockContainer!=null)
				m_DocumentUIManager.OnMouseDown(e);
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);

            //if(!IsDocumentDock)
            //    return;
			if(m_DocumentDockContainer!=null)
				m_DocumentUIManager.OnMouseUp(e);
		}

		private void UpdateBarsCollection()
		{
			if(m_Owner==null)
				return;
			foreach(Control c in this.Controls)
			{
				if(c is Bar)
				{
					Bar bar=c as Bar;
					if(!m_Owner.Bars.Contains(bar))
						m_Owner.Bars.Add(bar);
				}
			}
		}

		protected override void OnDockChanged(EventArgs e)
		{
			base.OnDockChanged(e);
			if(this.IsDocumentDock)
				UpdateBarsCollection();
		}

		internal void SetOwner(DotNetBarManager owner)
		{
			m_Owner=owner;
			
            UpdateBarsCollection();
		}

		/// <summary>
		/// Gets the reference to the DotNetBarManager that uses this dock site.
		/// </summary>
		[Browsable(false),DevCoBrowsable(false)]
		public DotNetBarManager Owner
		{
			get {return m_Owner;}
		}

		internal bool HasFixedBars
		{
			get
			{
				foreach(Control ctrl in this.Controls)
				{
					Bar bar=ctrl as Bar;
					if(bar!=null && bar.LockDockPosition)
					{
						return true;
					}
				}
				return false;
			}
		}

        #region DockSiteAccessibleObject
        public class DockSiteAccessibleObject : System.Windows.Forms.Control.ControlAccessibleObject
        {
            DockSite m_Owner = null;
            public DockSiteAccessibleObject(DockSite owner)
                : base(owner)
            {
                m_Owner = owner;
            }

            //public override string Name
            //{
            //    get
            //    {
            //        if (m_Owner != null && !m_Owner.IsDisposed)
            //            return m_Owner.AccessibleName;
            //        return "";
            //    }
            //    set
            //    {
            //        if (m_Owner != null && !m_Owner.IsDisposed)
            //            m_Owner.AccessibleName = value;
            //    }
            //}

            //public override string Description
            //{
            //    get
            //    {
            //        if (m_Owner != null && !m_Owner.IsDisposed)
            //            return m_Owner.AccessibleDescription;
            //        return "";
            //    }
            //}

            public override AccessibleRole Role
            {
                get
                {
                    if (m_Owner != null && !m_Owner.IsDisposed)
                        return m_Owner.AccessibleRole;
                    return System.Windows.Forms.AccessibleRole.None;
                }
            }

            public override Rectangle Bounds
            {
                get
                {
                    if (m_Owner != null && !m_Owner.IsDisposed && m_Owner.Parent != null)
                        return this.m_Owner.Parent.RectangleToScreen(m_Owner.Bounds);
                    return Rectangle.Empty;
                }
            }

            public override int GetChildCount()
            {
                if (m_Owner != null && !m_Owner.IsDisposed)
                    return m_Owner.Controls.Count;
                return 0;
            }

            public override System.Windows.Forms.AccessibleObject GetChild(int iIndex)
            {
                if (m_Owner != null && !m_Owner.IsDisposed && m_Owner.Controls.Count > 0 && m_Owner.Controls.Count > iIndex)
                    return m_Owner.Controls[iIndex].AccessibilityObject;
                return null;
            }

            public override AccessibleStates State
            {
                get
                {
                    AccessibleStates state;
                    if (m_Owner == null || m_Owner.IsDisposed)
                        return AccessibleStates.None;

                       state = AccessibleStates.None;
                    return state;
                }
            }
        }
        #endregion
    }
}