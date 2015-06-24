using System;
using System.Text;
using System.Drawing;
using System.Reflection;

namespace DevComponents.DotNetBar.Design
{
    internal static class Helpers
    {
        internal static Bitmap LoadBitmap(string imageName)
        {
            DotNetBarResourcesAttribute att = Attribute.GetCustomAttribute(System.Reflection.Assembly.GetExecutingAssembly(), typeof(DotNetBarResourcesAttribute)) as DotNetBarResourcesAttribute;
            if (att != null && att.NamespacePrefix != "")
            {
                return new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream(att.NamespacePrefix + "." + imageName));
            }
            else
                return new Bitmap(typeof(DevComponents.DotNetBar.DotNetBarManager), imageName);
        }

        internal static Icon LoadIcon(string imageName)
        {
            DotNetBarResourcesAttribute att = Attribute.GetCustomAttribute(System.Reflection.Assembly.GetExecutingAssembly(), typeof(DotNetBarResourcesAttribute)) as DotNetBarResourcesAttribute;
            if (att != null && att.NamespacePrefix != "")
            {
                return new Icon(Assembly.GetExecutingAssembly().GetManifestResourceStream(att.NamespacePrefix + "." + imageName));
            }
            else
                return new Icon(typeof(DevComponents.DotNetBar.DotNetBarManager), imageName);
        }

        public static void DrawDesignTimeSelection(Graphics g, Rectangle r, Color backColor, Color border, int penWidth)
        {
            if (r.Width <= 0 || r.Height <= 0)
                return;
            if (!backColor.IsEmpty && backColor != Color.Transparent)
            {
                if ((double)backColor.GetBrightness() < 0.5)
                    border = System.Windows.Forms.ControlPaint.Light(backColor);
                else
                    border = System.Windows.Forms.ControlPaint.Dark(backColor);
            }
            using (Pen pen = new Pen(border, penWidth))
            {
                pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                r.Width--;
                r.Height--;
                g.DrawRectangle(pen, r);
            }
        }

        public static bool IsOffice2007Style(eDotNetBarStyle style)
        {
            if (style == eDotNetBarStyle.StyleManagerControlled)
                style = StyleManager.GetEffectiveStyle();
            return (style == eDotNetBarStyle.Office2007 || style == eDotNetBarStyle.Office2010 || style == eDotNetBarStyle.Windows7);
        }

        internal static string GetItemErrorInfo(System.Xml.XmlElement xmlItem)
        {
            string s = "";
            if (xmlItem.HasAttribute("assembly"))
                s = s + xmlItem.GetAttribute("assembly");
            if (xmlItem.HasAttribute("class"))
                s = s + xmlItem.GetAttribute("class");
            return s;
        }
    }
}
