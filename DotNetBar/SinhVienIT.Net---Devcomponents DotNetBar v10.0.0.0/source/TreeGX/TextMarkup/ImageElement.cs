using System;
using System.Text;
using System.Drawing;
using System.Xml;
using System.Reflection;

#if TREEGX
namespace DevComponents.Tree.TextMarkup
#elif DOTNETBAR
namespace DevComponents.DotNetBar.TextMarkup
#elif SUPERGRID
namespace DevComponents.SuperGrid.TextMarkup
#endif

{
    internal class ImageElement : MarkupElement
    {
        #region Private Variables
        private Size m_ImageSize = Size.Empty;
        private string m_ImageSource = "";
        private Image m_Image = null;
        #endregion

        #region Internal Implementation
        public override void Measure(System.Drawing.Size availableSize, MarkupDrawContext d)
        {
            if (!m_ImageSize.IsEmpty)
                this.Bounds = new Rectangle(Point.Empty, m_ImageSize);
            else if (m_ImageSource.Length == 0)
                this.Bounds = Rectangle.Empty;
            else
            {
                Image img = this.GetImage();
                if (img != null)
                    this.Bounds = new Rectangle(Point.Empty, img.Size);
                else
                    this.Bounds = new Rectangle(Point.Empty, new Size(16,16));
            }
        }

        public override bool IsBlockElement
        {
            get
            {
                return false;
            }
        }

        private Image GetImage()
        {
            if (m_Image != null) return m_Image;
            Assembly a = null;

            // Load from format: ClassLibrary1/ClassLibrary1.MyImage.png or ClassLibrary1/global::ClassLibrary1.Resources.MyImage
            if (m_ImageSource.IndexOf('/') >= 0)
            {
                string[] parts = m_ImageSource.Split('/');
                a = Assembly.Load(parts[0]);
                string ResourceName = parts[1];
                if (a != null)
                {
                    m_Image = LoadImageGlobalResource(parts[1], a);
                    if (m_Image == null)
                        m_Image = LoadImageResource(parts[1], a);

                    if (m_Image != null) return m_Image;
                }
            }

            // Probe Executing Assembly
            a = Assembly.GetExecutingAssembly();
            m_Image = LoadImageGlobalResource(m_ImageSource, a);
            if(m_Image==null)
                m_Image = LoadImageResource(m_ImageSource, a);

            // Probe Entry Assembly
            if (m_Image == null)
            {
                a = Assembly.GetEntryAssembly();
                m_Image = LoadImageGlobalResource(m_ImageSource, a);
                if (m_Image == null)
                    m_Image = LoadImageResource(m_ImageSource, a);
            }

            return m_Image;
        }

        private Image LoadImageGlobalResource(string imageSource, Assembly a)
        {
            Image img = null;
#if FRAMEWORK20
            if (imageSource.StartsWith("global::"))
            {
                string name = imageSource.Substring(8);
                if (name.Length > 0)
                {
                    try
                    {
                        int i = name.LastIndexOf('.');
                        string resName = name.Substring(0, i);
                        name = name.Substring(i + 1);
                        System.Resources.ResourceManager r = new System.Resources.ResourceManager(resName, a);
                        object obj = r.GetObject(name);
                        img = (Bitmap)obj;
                    }
                    catch
                    {
                        img = null;
                    }
                }
            }
#endif
            return img;
        }

        private Image LoadImageResource(string imageSource, Assembly a)
        {
            Image img = null;
            try
            {
                img = new Bitmap(a.GetManifestResourceStream(imageSource));
            }
            catch { }

            return img;
        }

        public override void Render(MarkupDrawContext d)
        {
            Rectangle r = this.Bounds;
            r.Offset(d.Offset);

            if (!d.ClipRectangle.IsEmpty && !r.IntersectsWith(d.ClipRectangle))
                return;

            Image img = this.GetImage();
            if (img != null)
            {
                Rectangle imageRect = r;
                imageRect.Size = img.Size;
                d.Graphics.DrawImage(img, imageRect);
            }
            else
            {
                using (SolidBrush brush = new SolidBrush(Color.White))
                {
                    d.Graphics.FillRectangle(brush, r);
                }
                using (Pen pen = new Pen(Color.DarkGray, 1))
                {
                    d.Graphics.DrawRectangle(pen, r);
                    d.Graphics.DrawLine(pen, r.X, r.Y, r.Right, r.Bottom);
                    d.Graphics.DrawLine(pen, r.Right, r.Y, r.X, r.Bottom);
                }
            }
        }

        protected override void ArrangeCore(System.Drawing.Rectangle finalRect, MarkupDrawContext d)  { }

        public override void ReadAttributes(XmlTextReader reader)
        {
            m_ImageSize = Size.Empty;
            m_ImageSource = "";
            m_Image = null;

            for (int i = 0; i < reader.AttributeCount; i++)
            {
                reader.MoveToAttribute(i);
                if (reader.Name.ToLower() == "width")
                {
                    string s = reader.Value;
                    m_ImageSize.Width = int.Parse(s);
                }
                else if (reader.Name.ToLower() == "height")
                {
                    string s = reader.Value;
                    m_ImageSize.Height = int.Parse(s);
                }
                else if (reader.Name.ToLower() == "src")
                {
                    m_ImageSource = reader.Value;
                }
            }
        }
        #endregion
    }
}
