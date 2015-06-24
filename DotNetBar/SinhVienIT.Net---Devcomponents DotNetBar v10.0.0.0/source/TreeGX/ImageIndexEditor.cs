using System;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.ComponentModel;
using System.Windows.Forms.ComponentModel;
using System.Windows.Forms.Design;

namespace DevComponents.Tree
{
	/// <summary>
	/// Represents the class used by windows forms editor for picking an image from image list.
	/// </summary>
	public class ImageIndexEditor:UITypeEditor
	{
		private ImageEditor m_ImageEditor;
		private System.Windows.Forms.ImageList m_ImageList=null;
		public ImageIndexEditor():base()
		{
			m_ImageEditor=System.ComponentModel.TypeDescriptor.GetEditor(typeof(System.Drawing.Image),typeof(UITypeEditor)) as ImageEditor;
		}
		
		public override bool GetPaintValueSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		public override void PaintValue(PaintValueEventArgs e)
		{
			try
			{
				if(e==null || e.Value==null)
					return;
				int iIndex=(int)e.Value;
				System.Drawing.Image img=this.GetImage(e.Context,iIndex);
				if(img==null)
					return;
				PaintValueEventArgs pi=new PaintValueEventArgs(e.Context,img,e.Graphics,e.Bounds);
				m_ImageEditor.PaintValue(pi);
				//m_ImageEditor.PaintValue(img,e.Graphics,e.Bounds);
			}
			catch{}
		}

		private System.Drawing.Image GetImage(System.ComponentModel.ITypeDescriptorContext context,int index)
		{
			if(m_ImageList!=null && index>=0 && index<=m_ImageList.Images.Count)
			{
				return m_ImageList.Images[index];
			}
			if(context==null)
				return null;

			object o=context.Instance;
			if(o==null)
				return null;
			System.ComponentModel.PropertyDescriptorCollection pd=System.ComponentModel.TypeDescriptor.GetProperties(o);
			if(pd==null)
				return null;

			foreach(System.ComponentModel.PropertyDescriptor prop in pd)
			{
				if(prop.PropertyType==typeof(System.Windows.Forms.ImageList))
				{
					m_ImageList=prop.GetValue(o) as System.Windows.Forms.ImageList;
					if(m_ImageList!=null && index>=0 && index<=m_ImageList.Images.Count)
					{
						return m_ImageList.Images[index];
					}
					break;
				}
			}
			return null;
		}
	}
}
