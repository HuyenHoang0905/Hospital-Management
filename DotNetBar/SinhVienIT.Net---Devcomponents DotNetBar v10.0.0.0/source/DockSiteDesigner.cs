using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms.Design;
using System.Windows.Forms;
using System.Drawing;
namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Summary description for DockSiteDesigner.
	/// </summary>
	public class DockSiteDesigner:ParentControlDesigner
	{
		private bool m_MouseDrag=false;

		public override SelectionRules SelectionRules
		{
			get{return SelectionRules.None;}
		}
		protected override void OnContextMenu(int x,int y)
		{
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
			// Unhook events
			IComponentChangeService cc=(IComponentChangeService)GetService(typeof(IComponentChangeService));
			if(cc!=null)
				cc.ComponentRemoving-=new ComponentEventHandler(this.OnComponentRemoving);
			DockSite c=this.Control as DockSite;
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
			//base.OnSetCursor();
			if(this.Control.Dock!=DockStyle.Fill)
				return;
			Point p=Control.MousePosition;
			DockSite site=this.Control as DockSite;
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
			if(this.Control.Dock==DockStyle.Fill && site!=null)
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
				NativeFunctions.RECT rect = new NativeFunctions.RECT(0,0,0,0);
				NativeFunctions.GetWindowRect(site.Handle, ref rect);
				Rectangle r=Rectangle.FromLTRB(rect.Left, rect.Top, rect.Right, rect.Bottom);
				Cursor.Clip=r;
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
