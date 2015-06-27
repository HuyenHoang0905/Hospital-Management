using System;
using System.Drawing;
using System.Windows.Forms;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Serves as integration of regular Image class and Icon class
	/// </summary>
	internal class CompositeImage:IDisposable
	{
		private bool m_DisposeImage=false;

		private Image m_Image=null;
		private Icon m_Icon=null;
		private Size m_ImageSizeOverride=Size.Empty;
        private ImageList m_ImageList = null;
        private int m_ImageIndex = -1;

        public CompositeImage(int imageIndex, ImageList imageList)
        {
            m_ImageIndex = imageIndex;
            m_ImageList = imageList;
        }

		public CompositeImage(Image image, bool dispose)
		{
			m_Image=image;
			m_DisposeImage=dispose;
		}
		public CompositeImage(Icon icon, bool dispose)
		{
			m_Icon=icon;
			m_DisposeImage=dispose;
		}
		public CompositeImage(Image image, bool dispose, Size overrideSize)
		{
			m_Image=image;
			m_DisposeImage=dispose;
			m_ImageSizeOverride=overrideSize;
		}
		public CompositeImage(Icon icon, bool dispose, Size overrideSize)
		{
			m_Icon=icon;
			m_DisposeImage=dispose;
			m_ImageSizeOverride=overrideSize;
		}
        ~CompositeImage()  // destructor
        {
            if (m_DisposeImage && (m_Image != null || m_Icon != null))
            {
                Dispose();
            }
        }

		public void Dispose()
		{
			if(m_DisposeImage)
			{
				if(m_Image!=null)
					m_Image.Dispose();
				if(m_Icon!=null)
					m_Icon.Dispose();
			}

			m_Image=null;
			m_Icon=null;
		}

		public void DrawImage(Graphics g, Rectangle rect)
		{
			if(m_Image!=null)
				g.DrawImage(m_Image,rect);
			else if(m_Icon!=null)
				DrawIcon(g,rect);
            else if (m_ImageIndex >= 0 && m_ImageList != null && m_ImageIndex < m_ImageList.Images.Count)
                m_ImageList.Draw(g, rect.X, rect.Y, rect.Width, rect.Height, m_ImageIndex);
		}

		private void DrawIcon(Graphics g, Rectangle rect)
		{
			if(System.Environment.Version.Build<=3705 && System.Environment.Version.Revision==288 && System.Environment.Version.Major==1 && System.Environment.Version.Minor==0)
			{
				if(g.ClipBounds.IntersectsWith(rect) && rect.Width>0 && rect.Height>0 && m_Icon.Handle!=IntPtr.Zero)
				{
					IntPtr hdc=g.GetHdc();
					try
					{
						NativeFunctions.DrawIconEx(hdc,rect.X,rect.Y,m_Icon.Handle,rect.Width,rect.Height,0,IntPtr.Zero,3);
					}
					finally
					{
						g.ReleaseHdc(hdc);
					}
				}
			}
			else if(rect.Width>0 && rect.Height>0 && m_Icon.Handle!=IntPtr.Zero)
			{
				try
				{
					g.DrawIcon(m_Icon,rect);
				}
				catch{}
			}
		}

		public void DrawImage(Graphics g,Rectangle destRect,int srcX,int srcY,int srcWidth,int srcHeight,GraphicsUnit srcUnit,System.Drawing.Imaging.ImageAttributes imageAttrs)
		{
            if (m_Image != null)
            {
                g.DrawImage(m_Image, destRect, srcX, srcY, srcWidth, srcHeight, srcUnit, imageAttrs);
            }
            else if (m_Icon != null)
            {
                // Attempt to solve issues with icon drawing...
                //DrawIcon(g,destRect);
                Bitmap bmp = new Bitmap(this.Width, this.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                bmp.MakeTransparent();
                Graphics gBmp = Graphics.FromImage(bmp);
                gBmp.DrawIcon(m_Icon, 0, 0);
                gBmp.Dispose();
                g.DrawImage(bmp, destRect, srcX, srcY, srcWidth, srcHeight, srcUnit, imageAttrs);
                bmp.Dispose();
            }
            else if (m_ImageIndex >= 0 && m_ImageList != null && m_ImageIndex < m_ImageList.Images.Count)
                m_ImageList.Draw(g, srcX, srcY, srcWidth, srcHeight, m_ImageIndex);
		}

		public bool IsIcon
		{
			get { return (m_Icon!=null);}
		}

		public int Width
		{
			get
			{
				if(!m_ImageSizeOverride.IsEmpty)
					return m_ImageSizeOverride.Width;
				if(m_Image!=null)
					return m_Image.Width;
				if(m_Icon!=null)
					return m_Icon.Width;
                if (m_ImageIndex >= 0 && m_ImageList != null)
                    return m_ImageList.ImageSize.Width;
				return 0;
			}
		}

		public int Height
		{
			get
			{
				if(!m_ImageSizeOverride.IsEmpty)
					return m_ImageSizeOverride.Height;
				if(m_Image!=null)
					return m_Image.Height;
				if(m_Icon!=null)
					return m_Icon.Height;
                if (m_ImageIndex >= 0 && m_ImageList != null)
                    return m_ImageList.ImageSize.Height;
				return 0;
			}
		}

		public System.Drawing.Size Size
		{
			get
			{
				if(!m_ImageSizeOverride.IsEmpty)
					return m_ImageSizeOverride;
				if(m_Image!=null)
					return m_Image.Size;
				if(m_Icon!=null)
					return m_Icon.Size;
                if (m_ImageIndex >= 0 && m_ImageList != null)
                    return m_ImageList.ImageSize;
				return System.Drawing.Size.Empty;
			}
		}

		public System.Drawing.Image Image
		{
			get
            {
                if (m_ImageIndex >= 0 && m_ImageList != null)
                    return m_ImageList.Images[m_ImageIndex];
                return m_Image;
            }
		}

		public System.Drawing.Icon Icon
		{
			get {return m_Icon;}
		}

		internal int RealHeight
		{
			get
			{
				if(m_Image!=null)
					return m_Image.Height;
				if(m_Icon!=null)
					return m_Icon.Height;
                if (m_ImageIndex >= 0 && m_ImageList != null)
                    return m_ImageList.ImageSize.Height;
				return 0;
			}
		}

		internal int RealWidth
		{
			get
			{
				if(m_Image!=null)
					return m_Image.Width;
				if(m_Icon!=null)
					return m_Icon.Width;
                if (m_ImageIndex >= 0 && m_ImageList != null)
                    return m_ImageList.ImageSize.Width;
				return 0;
			}
		}
	}
}
