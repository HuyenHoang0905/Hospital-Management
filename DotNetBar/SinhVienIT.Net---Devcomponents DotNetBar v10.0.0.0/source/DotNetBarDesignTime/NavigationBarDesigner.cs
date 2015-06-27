using System;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Collections;
using System.Drawing;
using System.ComponentModel.Design;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.CodeDom;

namespace DevComponents.DotNetBar.Design
{
	/// <summary>
	/// Provides design time support for NavigationBar control.
	/// </summary>
	public class NavigationBarDesigner:BarBaseControlDesigner
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public NavigationBarDesigner()
		{
			this.EnableItemDragDrop=false;
		}

		public override DesignerVerbCollection Verbs 
		{
			get 
			{
				DesignerVerb[] verbs;
				verbs = new DesignerVerb[]
				{
					new DesignerVerb("Create New Button", new EventHandler(CreateNewButton)),
				};
				return new DesignerVerbCollection(verbs);
			}
		}
		private void CreateNewButton(object sender, EventArgs e)
		{
			IDesignerHost dh=(IDesignerHost)GetService(typeof(IDesignerHost));
			NavigationBar navbar=this.Control as NavigationBar;
			if(navbar==null || dh==null)
				return;

            DesignerTransaction dt = dh.CreateTransaction();
            try
            {
                IComponentChangeService change = this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
                if (change != null)
                    change.OnComponentChanging(this.Component, null);
                ButtonItem item = null;
                try
                {
                    m_CreatingItem = true;
                    item = dh.CreateComponent(typeof(ButtonItem)) as ButtonItem;
                    item.Text = item.Name;
                    item.OptionGroup = "navBar";
                    item.Image = Helpers.LoadBitmap("SystemImages.DefaultNavBarImage.png");
                    navbar.Items.Add(item);
                }
                finally
                {
                    m_CreatingItem = false;
                }

                if (navbar.Items.Count == 1)
                    item.Checked = true;
                navbar.RecalcLayout();

                if (change != null)
                    change.OnComponentChanged(this.Component, null, null, null);
            }
            catch
            {
                dt.Cancel();
            }
            finally
            {
                if (!dt.Canceled) dt.Commit();
            }
		}

		protected override bool OnMouseDown(ref Message m, MouseButtons mb)
		{
			if(base.OnMouseDown(ref m, mb))
				return true;
			
			NavigationBar navbar=this.GetItemContainerControl() as NavigationBar;
			if(navbar==null || navbar.IsDisposed || !navbar.SplitterVisible)
				return false;

			Point pos=navbar.PointToClient(System.Windows.Forms.Control.MousePosition);
			MouseEventArgs e=new MouseEventArgs(MouseButtons.Left,0,pos.X,pos.Y,0);

			if(m.Msg==WinApi.WM_LBUTTONDOWN)
			{
				if(navbar.HitTestSplitter(e.X,e.Y))
					navbar.SplitterMouseDown(e);
				if(navbar.IsSplitterMouseDown /*|| navbar.ClientRectangle.Contains(pos)*/)
					return true;;
			}

			return false;
		}

		protected override bool OnMouseUp(ref Message m)
		{
			NavigationBar navbar=this.GetItemContainerControl() as NavigationBar;
			if(navbar!=null && !navbar.IsDisposed && navbar.SplitterVisible)
			{
				Point pos=navbar.PointToClient(System.Windows.Forms.Control.MousePosition);
				MouseEventArgs e=new MouseEventArgs(MouseButtons.Left,0,pos.X,pos.Y,0);
				// Design-time splitter support
				if(navbar.IsSplitterMouseDown)
				{
					navbar.SplitterMouseUp(e);
					this.OnNavigationBarHeightChanged(navbar.Height);
				}
				else
					navbar.SplitterMouseUp(e);
			}

			if(base.OnMouseUp(ref m))
				return true;

			return false;
		}

		protected virtual void OnNavigationBarHeightChanged(int newHeight)
		{
//			IComponentChangeService change=this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
//			if(change!=null)
//			{
//				change.OnComponentChanging(this.Control,TypeDescriptor.GetProperties(this.Control).Find("NavigationBarHeight",true));
//				change.OnComponentChanged(this.Control,TypeDescriptor.GetProperties(this.Control).Find("NavigationBarHeight",true),0,newHeight);
//			}
		}

		protected override bool OnMouseMove(ref Message m)
		{
			if(base.OnMouseMove(ref m))
				return true;
			NavigationBar navbar=this.GetItemContainerControl() as NavigationBar;
			if(navbar!=null && !navbar.IsDisposed && navbar.SplitterVisible)
			{
				Point pos=navbar.PointToClient(System.Windows.Forms.Control.MousePosition);
				MouseEventArgs e=new MouseEventArgs(System.Windows.Forms.Control.MouseButtons,0,pos.X,pos.Y,0);
				navbar.SplitterMouseMove(e);
			}
			return false;
		}

		protected override bool OnMouseLeave(ref Message m)
		{
			if(base.OnMouseLeave(ref m))
				return true;
			NavigationBar navbar=this.GetItemContainerControl() as NavigationBar;
			if(navbar!=null && !navbar.IsDisposed && navbar.SplitterVisible)
				navbar.SplitterMouseLeave();
			return false;
		}

		protected override void OnItemSelected(BaseItem item)
		{
			base.OnItemSelected(item);

			if(item is ButtonItem && !((ButtonItem)item).Checked)
			{
				ButtonItem button=item as ButtonItem;
				IComponentChangeService change=this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
				if(change!=null)
					change.OnComponentChanging(button,TypeDescriptor.GetProperties(button).Find("Checked",true));
				button.Checked=true;
				if(change!=null)
					change.OnComponentChanged(button,TypeDescriptor.GetProperties(button).Find("Checked",true),null,null);
			}
		}
	}

//	public class NavigationBarSerializer : CodeDomSerializer
//	{
//		public override object Serialize(IDesignerSerializationManager manager, object value) 
//		{
//			// first, locate and invoke the default serializer for 
//			CodeDomSerializer baseSerializer = (CodeDomSerializer)manager.GetSerializer(
//				typeof(NavigationBar).BaseType, 
//				typeof(CodeDomSerializer));
//
//			object codeObject = baseSerializer.Serialize(manager, value);
//
//			// now add some custom code
//			if (codeObject is CodeStatementCollection) 
//			{
//
//				//				// add a custom comment to the code.
//				CodeStatementCollection statements = 
//					(CodeStatementCollection)codeObject;
//
//				// call a custom method.
//				CodeExpression targetObject = 
//					base.SerializeToReferenceExpression(manager, value);
//				if(targetObject != null) 
//				{
//					
//					CodeMethodInvokeExpression methodCall = 
//						new CodeMethodInvokeExpression(targetObject, "RecalcLayout");
//					statements.Add(methodCall);
//				}
//         
//			}
//
//			// finally, return the statements that have been created
//			return codeObject;
//		}
//
//		/// The default serializer can handle the deserialization just fine.  We override this
//		/// because it is an abstract member.
//		/// </summary>
//		public override object Deserialize(IDesignerSerializationManager manager, object codeDomObject)  
//		{
//			// delegate straight through to the default serializer
//			//
//			CodeDomSerializer baseSerializer = (CodeDomSerializer)manager.GetSerializer(typeof(NavigationBar).BaseType, typeof(CodeDomSerializer));
//
//			return baseSerializer.Deserialize(manager, codeDomObject);
//		}
//	}
}
