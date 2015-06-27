using System;
using System.Text;
using System.Windows.Forms.Design;
using System.Collections;
using DevComponents.DotNetBar.Controls;
using System.Drawing;

namespace DevComponents.DotNetBar.Design
{
    /// <summary>
    /// Represents Windows Forms designer for ReflectionImage control.
    /// </summary>
    public class ReflectionImageDesigner : ControlDesigner
    {
#if FRAMEWORK20
        public override void InitializeNewComponent(IDictionary defaultValues)
        {
            base.InitializeNewComponent(defaultValues);
            SetDesignTimeDefaults();
        }
#else
		public override void OnSetComponentDefaults()
		{
			base.OnSetComponentDefaults();
			SetDesignTimeDefaults();
		}
#endif

        protected virtual void SetDesignTimeDefaults()
        {
            ReflectionImage m = this.Control as ReflectionImage;
            m.Image = LoadReflectionImage();
            if (m.Image != null)
            {
                m.BackgroundStyle.TextAlignment = eStyleTextAlignment.Center;
            }
        }

        private static Image LoadReflectionImage()
        {
            string imageName = "ReflectionImage.png";
            try
            {
                Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine;
                string path = "";
                try
                {
                    if (key != null)
                        key = key.OpenSubKey("Software\\DevComponents\\DotNetBar");
                    if (key != null)
                        path = key.GetValue("InstallationFolder", "").ToString();
                }
                finally { if (key != null) key.Close(); }

                if (path != "")
                {
                    if (path.Substring(path.Length - 1, 1) != "\\")
                        path += "\\";
                    path += "Images\\";
                    if (System.IO.File.Exists(path + imageName))
                        path += imageName;
                    else
                        path = "";
                }

                if (path != "")
                {
                    return new Bitmap(path);
                }
            }
            catch (Exception)
            {
            }
            return null;
        }
    }
}
