using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms.Design;
using System.Windows.Forms;
using System.Drawing;
namespace DevComponents.DotNetBar.Design
{
	/// <summary>
	/// Summary description for DockSiteDesigner.
	/// </summary>
	public class DockSiteDesigner:ParentControlDesigner
	{
		private bool m_MouseDrag=false;
        internal bool DotNetBarManagerRemoving = false;

		public override SelectionRules SelectionRules
		{
			get{return SelectionRules.None;}
		}
		protected override void OnContextMenu(int x,int y)
		{
		}

        public override bool CanParent(Control control)
        {
            if (control is IDockInfo && !(control is ContextMenuBar))
                return true;
            return false;
        }

		public override void Initialize(IComponent component) 
		{
			base.Initialize(component);
			if(!component.Site.DesignMode)
				return;
			// If our component is removed we need to clean-up
			IComponentChangeService cc=(IComponentChangeService)GetService(typeof(IComponentChangeService));
			if(cc!=null)
			{
				cc.ComponentRemoving+=new ComponentEventHandler(this.OnComponentRemoving);
			}

            IDesignerHost dh = GetService(typeof(IDesignerHost)) as IDesignerHost;
            if (dh != null && dh.Loading)
            {
                ((DockSite)component)._DesignerLoading = true;
                dh.LoadComplete += new EventHandler(DesignerHostLoadComplete);
            }
		}

        private void DesignerHostLoadComplete(object sender, EventArgs e)
        {
			DockSite site = (DockSite)this.Control;
            site._DesignerLoading = false;

            IDesignerHost dh = GetService(typeof(IDesignerHost)) as IDesignerHost;
            if (dh != null)
                dh.LoadComplete -= new EventHandler(DesignerHostLoadComplete);
#if !FRAMEWORK20
			if(site.Dock == DockStyle.Fill && site.Controls.Count > 0)
				site.RecalcLayout();
#endif
        }

		protected override void Dispose(bool disposing)
		{
			// Unhook events
			IComponentChangeService cc=(IComponentChangeService)GetService(typeof(IComponentChangeService));
			if(cc!=null)
			{
				cc.ComponentRemoving-=new ComponentEventHandler(this.OnComponentRemoving);
			}

			base.Dispose(disposing);
		}

		private void OnComponentRemoving(object sender,ComponentEventArgs e)
		{
			
			if(e.Component!=this.Component)
			{
				return;
			}

            DockSite c = this.Control as DockSite;
            if (c.Dock != DockStyle.Fill && !DotNetBarManagerRemoving)
            {
                throw new InvalidOperationException("DotNetBarManager dock sites must not be removed manually. Delete DotNetBarManager component to remove all dock sites.");
            }
            else
            {
                IDesignerHost dh = (IDesignerHost)GetService(typeof(IDesignerHost));
                if (dh != null)
                {
                    Bar[] bars = new Bar[c.Controls.Count];
                    c.Controls.CopyTo(bars, 0);
                    foreach (Bar bar in bars)
                    {
                        if (bar != null)
                        {
                            dh.DestroyComponent(bar);
                        }
                    }
                }
            }

			// Unhook events
			IComponentChangeService cc=(IComponentChangeService)GetService(typeof(IComponentChangeService));
			if(cc!=null)
				cc.ComponentRemoving-=new ComponentEventHandler(this.OnComponentRemoving);
			
			if(c==null)
				return;
			if(c.Owner!=null)
			{
				if(c.Owner.FillDockSite==c)
					c.Owner.FillDockSite=null;
				else if(c.Owner.LeftDockSite==c)
					c.Owner.LeftDockSite=null;
				else if(c.Owner.RightDockSite==c)
					c.Owner.RightDockSite=null;
				else if(c.Owner.TopDockSite==c)
					c.Owner.TopDockSite=null;
				else if(c.Owner.BottomDockSite==c)
					c.Owner.BottomDockSite=null;
			}
		}

		protected override void OnSetCursor()
		{
            DockSite site = this.Control as DockSite;
			if(site.Dock!=DockStyle.Fill && site.DocumentDockContainer==null)
				return;
			Point p=Control.MousePosition;
			if(site==null || site.Controls.Count==0) return;

			DocumentDockUIManager m=site.GetDocumentUIManager();
			Point pClient=site.PointToClient(p);
			Cursor c=m.GetCursor(pClient.X,pClient.Y);
			if(c!=null)
				Cursor.Current=c;
			else
				Cursor.Current=Cursors.Default;
		}

		protected override void OnMouseDragBegin(int x, int y)
		{
			DockSite site=this.Control as DockSite;
            if (this.Control.Dock == DockStyle.Fill && site != null || site.DocumentDockContainer != null)
			{
				DocumentDockUIManager m=site.GetDocumentUIManager();
				Point pClient=site.PointToClient(new Point(x,y));
				Cursor c=m.GetCursor(pClient.X,pClient.Y);
				if(c!=null)
				{
					m.OnMouseDown(new MouseEventArgs(MouseButtons.Left,0,pClient.X,pClient.Y,0));
					m_MouseDrag=true;
				}
			}

			base.OnMouseDragBegin(x,y);

			if(m_MouseDrag)
			{
				site.Capture = true;
                if (site.Dock == DockStyle.Fill || site.GetDocumentUIManager().IsResizingDocument)
                {
                    WinApi.RECT rect = new WinApi.RECT(0, 0, 0, 0);
                    WinApi.GetWindowRect(site.Handle, ref rect);
                    Rectangle r = Rectangle.FromLTRB(rect.Left, rect.Top, rect.Right, rect.Bottom);
                    Cursor.Clip = r;
                }
				#if !FRAMEWORK20
				else
					Cursor.Clip = Rectangle.Empty;
				#endif
			}
		}

		protected override void OnMouseDragMove(int x, int y)
		{
			DockSite site=this.Control as DockSite;
			if(m_MouseDrag && site!=null)
			{
				DocumentDockUIManager m=site.GetDocumentUIManager();
				Point pClient=site.PointToClient(new Point(x,y));
				m.OnMouseMove(new MouseEventArgs(MouseButtons.Left,0,pClient.X,pClient.Y,0));
			}
			else
				base.OnMouseDragMove(x,y);
		}

		protected override void OnMouseDragEnd(bool cancel)
		{
			if(m_MouseDrag)
			{
				this.Control.Capture = false;
				Cursor.Clip = Rectangle.Empty;
			}
			DockSite site=this.Control as DockSite;
			if(m_MouseDrag && site!=null)
			{
				DocumentDockUIManager m=site.GetDocumentUIManager();
				Point pClient=site.PointToClient(Control.MousePosition);
				
				IComponentChangeService cc=(IComponentChangeService)GetService(typeof(IComponentChangeService));
				if(cc!=null)
				{
					cc.OnComponentChanging(site,TypeDescriptor.GetProperties(typeof(DockSite))["DocumentDockContainer"]);
				}

				m.OnMouseUp(new MouseEventArgs(MouseButtons.Left,0,pClient.X,pClient.Y,0));

				if(cc!=null)
				{
					cc.OnComponentChanged(site,TypeDescriptor.GetProperties(typeof(DockSite))["DocumentDockContainer"],null,null);
				}
			}
			m_MouseDrag=false;
			base.OnMouseDragEnd(cancel);
		}
	}
}
