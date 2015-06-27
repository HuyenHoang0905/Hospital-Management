using System;
using System.Drawing;
using System.Reflection;
using System.Xml;
using System.Resources;
using System.Collections;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Summary description for BarFunctions.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public sealed class BarFunctions
    {
        public const int ANIMATION_INTERVAL = 100;

        private static string ms_ResourceName = "";
        const string DEFAULT_RESOURCE = ".Strings";
        private static bool m_ThemedOS = false;
        private static bool m_IsVista = false;
        private static bool _IsWindows7 = false;
        private static bool m_SupportsAnimation = true;
        private static bool _IsWindowsXP = false;

        static BarFunctions()
        {

            m_ThemedOS = false;

            NativeFunctions.OSVERSIONINFO os = new NativeFunctions.OSVERSIONINFO();
            os.dwOSVersionInfoSize = System.Runtime.InteropServices.Marshal.SizeOf(typeof(NativeFunctions.OSVERSIONINFO));
            NativeFunctions.GetVersionEx(ref os);
            if (os.dwPlatformId == 2 && os.dwMajorVersion == 4)
                m_SupportsAnimation = false;
            if (os.dwMajorVersion == 5 && os.dwMinorVersion >= 1 && os.dwPlatformId == 2 ||
                os.dwMajorVersion > 5 && os.dwPlatformId == 2)
                m_ThemedOS = System.Windows.Forms.OSFeature.Feature.IsPresent(System.Windows.Forms.OSFeature.Themes);
            Version osVersion = System.Environment.OSVersion.Version;
            _IsWindowsXP = osVersion.Major <= 5;
            m_IsVista = osVersion.Major >= 6;
            _IsWindows7 = osVersion.Major >= 6 && osVersion.Minor >= 1 && osVersion.Build >= 7000;
            RefreshScreens();
        }

        public static bool IsWindowsXP
        {
            get
            {
                return _IsWindowsXP;
            }
        }

        public static bool IsVista
        {
            get { return m_IsVista; }
        }
        public static bool IsWindows7
        {
            get
            {
                return _IsWindows7;
            }
        }

        internal static char GetCharForKeyValue(int keyValue)
        {
            byte[] chars = new byte[2];
            try
            {
                byte[] keyState = new byte[256];
                if (NativeFunctions.GetKeyboardState(keyState))
                {
                    if (NativeFunctions.ToAscii((uint)keyValue, 0, keyState, chars, 0) != 0)
                    {
                        return (char)chars[0];
                    }
                }
            }
            catch (Exception)
            {
                return char.MinValue;
            }

            return char.MinValue;
        }

        public static Color Darken(Color color, int percent)
        {
            ColorFunctions.HLSColor h = ColorFunctions.RGBToHSL(color.R, color.G, color.B);
            h.Lightness *= (double)(100 - percent) / 100;
            return ColorFunctions.HLSToRGB(h);
        }

        public static Color Ligten(Color color, int percent)
        {
            ColorFunctions.HLSColor h = ColorFunctions.RGBToHSL(color.R, color.G, color.B);
            h.Lightness *= (1 + (double)percent / 100);
            return ColorFunctions.HLSToRGB(h);
        }

        /// <summary>
        /// Tries to invoke the RecalcLayout method on the control and return true if such method was invoked.
        /// </summary>
        /// <param name="c">Reference to the control</param>
        /// <param name="invalidate">Indicates whether to invalidate control if no recalc layout method is found</param>
        /// <returns>return true if method is invoked.</returns>
        public static bool InvokeRecalcLayout(Control c, bool invalidate)
        {
            if (c is ItemControl)
            {
                ((ItemControl)c).RecalcLayout();
                return true;
            }
            else if (c is Bar)
            {
                ((Bar)c).RecalcLayout();
                return true;
            }
            else if (c is ExplorerBar)
            {
                ((ExplorerBar)c).RecalcLayout();
                return true;
            }
            else if (c is BaseItemControl)
            {
                ((BaseItemControl)c).RecalcLayout();
                return true;
            }
            else if (c is BarBaseControl)
            {
                ((BarBaseControl)c).RecalcLayout();
                return true;
            }
            else if (c is PopupItemControl)
            {
                ((PopupItemControl)c).RecalcLayout();
                return true;
            }



            MethodInfo m = c.GetType().GetMethod("RecalcLayout");

            if (m != null)
            {
                m.Invoke(c, null);
                return true;
            }
            else if (invalidate)
            {
                c.Invalidate(true);
                c.Update();
            }
            return false;
        }

        public static bool ThemedOS
        {
            get { return m_ThemedOS; }
            set { m_ThemedOS = value; }
        }

        public static StringFormat CreateStringFormat()
        {
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Near;
            sf.LineAlignment = StringAlignment.Near;
            sf.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.None;
            sf.Trimming = StringTrimming.Character;
            return sf;
            //return new StringFormat(StringFormat.GenericDefault);
        }

        public static void SetControlVisible(Control c, bool visible)
        {
            if (visible)
            {
                int indexZOrder = -1;
                if (c.Parent != null && c.Dock != DockStyle.None) indexZOrder = c.Parent.Controls.IndexOf(c);
                c.Visible = true;
                if (indexZOrder != -1) c.Parent.Controls.SetChildIndex(c, indexZOrder);
            }
            else
                c.Visible = false;
        }

        public static bool ProcessItemsShortcuts(eShortcut key, Hashtable itemsShortcuts)
        {
            bool eat = false;
            if (itemsShortcuts.Contains(key))
            {
                ShortcutTableEntry objEntry = (ShortcutTableEntry)itemsShortcuts[key];
                // Must convert to new array, since if this is for example
                // close command first Click will destroy the collection we are
                // iterating through and exception will be raised.
                BaseItem[] arr = new BaseItem[objEntry.Items.Values.Count];
                objEntry.Items.Values.CopyTo(arr, 0);
                Hashtable hnames = new Hashtable(arr.Length);
                foreach (BaseItem objItem in arr)
                {
                    if (objItem.CanRaiseClick && (objItem.Name == "" || !hnames.Contains(objItem.Name)))
                    {
                        if (!objItem.GlobalItem || objItem.GlobalName == "" || !hnames.Contains(objItem.GlobalName))
                        {
                            eat = true;
                            objItem.RaiseClick(eEventSource.Keyboard);
                            if (objItem.Name != "")
                            {
                                hnames.Add(objItem.Name, "");
                            }
                            if (objItem.GlobalItem && objItem.GlobalName != "" && objItem.GlobalName != objItem.Name && !hnames.Contains(objItem.GlobalName))
                                hnames.Add(objItem.GlobalName, "");
                        }
                    }
                }
            }
            return eat;
        }

        /// <summary>
        /// Creates copy of a bar to be used as new dock bar. This function is used to create new bar for tabs that are torn off the existing dock bars.
        /// </summary>
        /// <param name="instance">Original base bar to base the new bar on.</param>
        /// <returns>New instance of a bar. Note that bar is not added to the DotNetBarManager.Bars collection and DockSide is not set.</returns>
        public static Bar CreateDuplicateDockBar(Bar instance)
        {
            // Create new Bar and invoke the drag there
            Bar bar = new Bar(instance.Text);
            return CreateDuplicateDockBar(instance, bar);
        }

        ///// <summary>
        ///// Creates copy of a bar to be used as new dock bar. This function is used to create new bar for tabs that are torn off the existing dock bars.
        ///// </summary>
        ///// <param name="instance">Original base bar to base the new bar on.</param>
        ///// <param name="services">IDesignerServices to use for creation of the new instance of the object.</param>
        ///// <returns>New instance of a bar. Note that bar is not added to the DotNetBarManager.Bars collection and DockSide is not set.</returns>
        //public static Bar CreateDuplicateDockBar(Bar instance, IDesignerServices services)
        //{
        //    Bar bar = services.CreateComponent(typeof(Bar)) as Bar;
        //    return CreateDuplicateDockBar(instance, bar);
        //}
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static Bar CreateDuplicateDockBar(Bar instance, Bar bar)
        {
            bar.Text = instance.Text;
            bar.ItemsContainer.MinHeight = instance.MinHeight;
            bar.ItemsContainer.MinWidth = instance.ItemsContainer.MinWidth;
            bar.CanDockBottom = instance.CanDockBottom;
            bar.CanDockLeft = instance.CanDockLeft;
            bar.CanDockRight = instance.CanDockRight;
            bar.CanDockTop = instance.CanDockTop;
            bar.CanDockDocument = instance.CanDockDocument;
            bar.CanDockTab = instance.CanDockTab;
            bar.CanUndock = instance.CanUndock;
            bar.CanAutoHide = instance.CanAutoHide;
            bar.DockTabAlignment = instance.DockTabAlignment;
            bar.CanCustomize = instance.CanCustomize;
            bar.AutoHideAnimationTime = instance.AutoHideAnimationTime;
            bar.AlwaysDisplayDockTab = instance.AlwaysDisplayDockTab;
            bar.AutoCreateCaptionMenu = instance.AutoCreateCaptionMenu;
            bar.AutoSyncBarCaption = instance.AutoSyncBarCaption;
            bar.HideFloatingInactive = instance.HideFloatingInactive;
            bar.CloseSingleTab = instance.CloseSingleTab;
            bar.DockTabCloseButtonVisible = instance.DockTabCloseButtonVisible;
            bar.CaptionHeight = instance.CaptionHeight;

            if (!instance.CaptionBackColor.IsEmpty)
                bar.CaptionBackColor = instance.CaptionBackColor;
            if (!instance.CaptionForeColor.IsEmpty)
                bar.CaptionForeColor = instance.CaptionForeColor;
            if (!instance.ItemsContainer.m_BackgroundColor.IsEmpty)
                bar.ItemsContainer.BackColor = instance.ItemsContainer.m_BackgroundColor;
            if (instance.DockedBorderStyle != eBorderType.None)
                bar.DockedBorderStyle = instance.DockedBorderStyle;

            bar.Style = instance.Style;

            if (instance.ColorScheme.SchemeChanged)
                bar.ColorScheme = instance.ColorScheme;

            bar.LayoutType = instance.LayoutType;
            bar.GrabHandleStyle = instance.GrabHandleStyle;
            bar.Stretch = instance.Stretch;
            bar.CanHide = instance.CanHide;
            bar.ThemeAware = instance.ThemeAware;
            bar.DockedBorderStyle = instance.DockedBorderStyle;

            return bar;
        }

        public static void ApplyAutoDocumentBarStyle(Bar bar)
        {
            bar.SetDockTabStyle(bar.Style);
            if (!bar.AlwaysDisplayDockTab)
                bar.AlwaysDisplayDockTab = true;
            if (bar.DockTabAlignment != eTabStripAlignment.Top)
                bar.DockTabAlignment = eTabStripAlignment.Top;
            if (bar.GrabHandleStyle != eGrabHandleStyle.None)
                bar.GrabHandleStyle = eGrabHandleStyle.None;
        }

        public static void RestoreAutoDocumentBarStyle(Bar bar)
        {
            bar.SetDockTabStyle(bar.Style);
            if (bar.AlwaysDisplayDockTab)
                bar.AlwaysDisplayDockTab = false;
            if (bar.DockTabAlignment != eTabStripAlignment.Bottom)
                bar.DockTabAlignment = eTabStripAlignment.Bottom;
            if (bar.GrabHandleStyle != eGrabHandleStyle.Caption)
                bar.GrabHandleStyle = eGrabHandleStyle.Caption;
        }

        /// <summary>
        /// Returns if passed control is ready for painting.
        /// </summary>
        /// <param name="objCtrl">Control to test.</param>
        /// <returns>true if handle is valid otherwise false</returns>
        public static bool IsHandleValid(System.Windows.Forms.Control objCtrl)
        {
            return (objCtrl != null && !objCtrl.Disposing && !objCtrl.IsDisposed && objCtrl.IsHandleCreated);
        }

        public static void DrawMenuCheckBox(ItemPaintArgs pa, System.Drawing.Rectangle r, eDotNetBarStyle style, bool MouseOver)
        {
            System.Drawing.Graphics g = pa.Graphics;
            Color clr;
            if (style != eDotNetBarStyle.Office2000)
            {
                if (MouseOver)
                {
                    //clr=g.GetNearestColor(Color.FromArgb(45,SystemColors.Highlight));
                    //SolidBrush objBrush=new SolidBrush(clr);
                    //g.FillRectangle(objBrush,r);
                    //objBrush.Dispose();
                }
                else
                {
                    //clr=g.GetNearestColor(Color.FromArgb(96,ColorFunctions.HoverBackColor()));
                    clr = pa.Colors.ItemCheckedBackground; //ColorFunctions.CheckBoxBackColor(g);
                    SolidBrush objBrush = new SolidBrush(clr);
                    g.FillRectangle(objBrush, r);
                    objBrush.Dispose();
                }
                //clr=g.GetNearestColor(Color.FromArgb(200,SystemColors.Highlight));
                clr = pa.Colors.ItemCheckedBorder; // SystemColors.Highlight;
                Pen objPen = new Pen(clr, 1);
                // TODO: Beta 2 fix --> g.DrawRectangle(objPen,r);
                NativeFunctions.DrawRectangle(g, objPen, r);
                objPen.Dispose();
                // Draw checker...
                Point[] pt = new Point[3];
                pt[0].X = r.Left + (r.Width - 5) / 2 - 1;
                pt[0].Y = r.Top + (r.Height - 6) / 2 + 3;
                pt[1].X = pt[0].X + 2;
                pt[1].Y = pt[0].Y + 2;
                pt[2].X = pt[1].X + 4;
                pt[2].Y = pt[1].Y - 4;
                objPen = new Pen(pa.Colors.ItemCheckedText);
                g.DrawLines(objPen, pt);
                pt[0].X++;
                pt[1].X++;
                pt[2].X++;
                g.DrawLines(objPen, pt);
                objPen.Dispose();
            }
            else if (style == eDotNetBarStyle.Office2000)
            {
                // Draw checked box
                System.Windows.Forms.ControlPaint.DrawBorder3D(g, r, System.Windows.Forms.Border3DStyle.SunkenOuter, System.Windows.Forms.Border3DSide.All);
                if (!MouseOver)
                {
                    r.Inflate(-1, -1);
                    g.FillRectangle(ColorFunctions.GetPushedBrush(), r);
                }
                // Draw checker...
                Point[] pt = new Point[3];
                pt[0].X = r.Left + (r.Width - 6) / 2;
                pt[0].Y = r.Top + (r.Height - 6) / 2 + 3;
                pt[1].X = pt[0].X + 2;
                pt[1].Y = pt[0].Y + 2;
                pt[2].X = pt[1].X + 4;
                pt[2].Y = pt[1].Y - 4;
                g.DrawLines(SystemPens.ControlText, pt);
                pt[0].X++;
                pt[1].X++;
                pt[2].X++;
                g.DrawLines(SystemPens.ControlText, pt);
            }
        }

        public static void SerializeImage(System.Drawing.Image image, XmlElement xml)
        {
            if (image == null)
                return;

            System.IO.MemoryStream mem = new System.IO.MemoryStream(1024);
            // TODO: Beta 2 issue with the ImageFormat. RawFormat on image object does not return the actual image format
            // Right now it is hard coded to PNG but in final version we should get the original image format
            image.Save(mem, System.Drawing.Imaging.ImageFormat.Png);

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            System.IO.StringWriter sw = new System.IO.StringWriter(sb);

            System.Xml.XmlTextWriter xt = new System.Xml.XmlTextWriter(sw);
            xt.WriteBase64(mem.GetBuffer(), 0, (int)mem.Length);

            xml.InnerText = sb.ToString();
        }

        public static void SerializeIcon(System.Drawing.Icon icon, XmlElement xml)
        {
            if (icon == null)
                return;

            System.IO.MemoryStream mem = new System.IO.MemoryStream(1024);
            // TODO: Beta 2 issue with the ImageFormat. RawFormat on image object does not return the actual image format
            // Right now it is hard coded to PNG but in final version we should get the original image format
            icon.Save(mem);

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            System.IO.StringWriter sw = new System.IO.StringWriter(sb);

            System.Xml.XmlTextWriter xt = new System.Xml.XmlTextWriter(sw);

            xml.SetAttribute("encoding", "binhex");
            //xt.WriteBase64(mem.GetBuffer(),0,(int)mem.Length);
            xt.WriteBinHex(mem.GetBuffer(), 0, (int)mem.Length);

            xml.InnerText = sb.ToString();
        }

        public static System.Windows.Forms.Form CreateOutlineForm()
        {
            System.Windows.Forms.Form form = new System.Windows.Forms.Form();
            try
            {
                form.Size = new Size(0, 0);
            }
            catch
            {
                form = new System.Windows.Forms.Form();
            }
            form.BackColor = SystemColors.Highlight;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            if (NativeFunctions.AlphaBlendingSupported)
                form.Opacity = .5;
            else
                form.BackColor = System.Windows.Forms.ControlPaint.LightLight(SystemColors.Highlight);
            form.ShowInTaskbar = false;
            form.Text = "";
            form.CreateControl();
            return form;
        }

        /// <summary>
        /// XML element is expected to be something like <image>Image data Base64 encoded</image>
        /// </summary>
        /// <param name="xml">Image data</param>
        /// <returns></returns>
        public static System.Drawing.Image DeserializeImage(XmlElement xml)
        {
            System.Drawing.Image img = null;
            if (xml == null || xml.InnerText == "")
                return null;

            System.IO.StringReader sr = new System.IO.StringReader(xml.OuterXml);
            System.Xml.XmlTextReader xr = new System.Xml.XmlTextReader(sr);
            System.IO.MemoryStream mem = new System.IO.MemoryStream(1024);
            // Skip <image> to data
            xr.Read();

            byte[] base64 = new byte[1024];
            int base64len = 0;
            do
            {
                base64len = xr.ReadBase64(base64, 0, 1024);
                if (base64len > 0)
                    mem.Write(base64, 0, base64len);

            } while (base64len != 0);

            img = System.Drawing.Image.FromStream(mem);

            return img;
        }

        public static System.Drawing.Icon DeserializeIcon(XmlElement xml)
        {
            System.Drawing.Icon img = null;
            if (xml == null || xml.InnerText == "")
                return null;
            bool bDecodeBinHex = false;
            if (xml.HasAttribute("encoding") && xml.GetAttribute("encoding") == "binhex")
                bDecodeBinHex = true;
            System.IO.StringReader sr = new System.IO.StringReader(xml.OuterXml);
            System.Xml.XmlTextReader xr = new System.Xml.XmlTextReader(sr);
            System.IO.MemoryStream mem = new System.IO.MemoryStream(1024);
            // Skip <image> to data
            xr.Read();

            byte[] base64 = new byte[1024];
            int base64len = 0;
            if (bDecodeBinHex)
            {
                do
                {
                    base64len = xr.ReadBinHex(base64, 0, 1024);
                    if (base64len > 0)
                        mem.Write(base64, 0, base64len);

                } while (base64len != 0);
            }
            else
            {
                do
                {
                    base64len = xr.ReadBase64(base64, 0, 1024);
                    if (base64len > 0)
                        mem.Write(base64, 0, base64len);

                } while (base64len != 0);
            }
            mem.Position = 0;
            img = new System.Drawing.Icon(mem);

            return img;
        }

        internal static BaseItem CreateItemFromXml(System.Xml.XmlElement xmlItem)
        {
            string cl = xmlItem.GetAttribute("class");
            BaseItem returnItem = null;
            switch (cl)
            {
                case "DevComponents.DotNetBar.ButtonItem":
                    returnItem = new ButtonItem();
                    break;
                case "DevComponents.DotNetBar.TextBoxItem":
                    returnItem = new TextBoxItem();
                    break;
                case "DevComponents.DotNetBar.ComboBoxItem":
                    returnItem = new ComboBoxItem();
                    break;
                case "DevComponents.DotNetBar.LabelItem":
                    returnItem = new LabelItem();
                    break;
                case "DevComponents.DotNetBar.CustomizeItem":
                    returnItem = new CustomizeItem();
                    break;
                case "DevComponents.DotNetBar.ControlContainerItem":
                    returnItem = new ControlContainerItem();
                    break;
                case "DevComponents.DotNetBar.DockContainerItem":
                    returnItem = new DockContainerItem();
                    break;
                case "DevComponents.DotNetBar.MdiWindowListItem":
                    returnItem = new MdiWindowListItem();
                    break;
                case "DevComponents.DotNetBar.SideBarContainerItem":
                    returnItem = new SideBarContainerItem();
                    break;
                case "DevComponents.DotNetBar.SideBarPanelItem":
                    returnItem = new SideBarPanelItem();
                    break;
                case "DevComponents.DotNetBar.ExplorerBarGroupItem":
                    returnItem = new ExplorerBarGroupItem();
                    break;
                case "DevComponents.DotNetBar.ExplorerBarContainerItem":
                    returnItem = new ExplorerBarContainerItem();
                    break;
                case "DevComponents.DotNetBar.ProgressBarItem":
                    returnItem = new ProgressBarItem();
                    break;
                case "DevComponents.DotNetBar.ColorPickerDropDown":
                    returnItem = new ColorPickerDropDown();
                    break;
                default:
                    {
                        try
                        {
                            //System.Windows.Forms.MessageBox.Show("Loading custom: "+xmlItem.GetAttribute("assembly")+"   "+xmlItem.GetAttribute("class"));
                            System.Reflection.Assembly a = System.Reflection.Assembly.Load(xmlItem.GetAttribute("assembly"));
                            if (a == null)
                                return null;
                            BaseItem item = a.CreateInstance(xmlItem.GetAttribute("class")) as BaseItem;
                            returnItem = item;
                        }
                        catch (Exception e)
                        {
                            throw new ArgumentException("Could not create item from XML. Assembly=" + xmlItem.GetAttribute("assembly") + ", Class=" + xmlItem.GetAttribute("class") + ", Inner Exception: " + e.Message + ", Source=" + e.Source);
                        }
                        break;
                    }
            }
            return returnItem;
        }

        internal static BaseItem CreateItemFromXml(System.Xml.XmlElement xmlItem, System.ComponentModel.Design.IDesignerHost dh, string name)
        {
            string cl = xmlItem.GetAttribute("class");
            BaseItem returnItem = null;
            switch (cl)
            {
                case "DevComponents.DotNetBar.ButtonItem":
                    if (name != "")
                        returnItem = dh.CreateComponent(typeof(ButtonItem), name) as ButtonItem;
                    else
                        returnItem = dh.CreateComponent(typeof(ButtonItem)) as ButtonItem;
                    break;
                case "DevComponents.DotNetBar.TextBoxItem":
                    if (name != "")
                        returnItem = dh.CreateComponent(typeof(TextBoxItem), name) as TextBoxItem;
                    else
                        returnItem = dh.CreateComponent(typeof(TextBoxItem)) as TextBoxItem;
                    break;
                case "DevComponents.DotNetBar.ComboBoxItem":
                    if (name != "")
                        returnItem = dh.CreateComponent(typeof(ComboBoxItem), name) as ComboBoxItem;
                    else
                        returnItem = dh.CreateComponent(typeof(ComboBoxItem)) as ComboBoxItem;
                    break;
                case "DevComponents.DotNetBar.LabelItem":
                    if (name != "")
                        returnItem = dh.CreateComponent(typeof(LabelItem), name) as LabelItem;
                    else
                        returnItem = dh.CreateComponent(typeof(LabelItem)) as LabelItem;
                    break;
                case "DevComponents.DotNetBar.CustomizeItem":
                    if (name != "")
                        returnItem = dh.CreateComponent(typeof(CustomizeItem), name) as CustomizeItem;
                    else
                        returnItem = dh.CreateComponent(typeof(CustomizeItem)) as CustomizeItem;
                    break;
                case "DevComponents.DotNetBar.ControlContainerItem":
                    if (name != "")
                        returnItem = dh.CreateComponent(typeof(ControlContainerItem), name) as ControlContainerItem;
                    else
                        returnItem = dh.CreateComponent(typeof(ControlContainerItem)) as ControlContainerItem;
                    break;
                case "DevComponents.DotNetBar.DockContainerItem":
                    if (name != "")
                        returnItem = dh.CreateComponent(typeof(DockContainerItem), name) as DockContainerItem;
                    else
                        returnItem = dh.CreateComponent(typeof(DockContainerItem)) as DockContainerItem;
                    break;
                case "DevComponents.DotNetBar.MdiWindowListItem":
                    if (name != "")
                        returnItem = dh.CreateComponent(typeof(MdiWindowListItem), name) as MdiWindowListItem;
                    else
                        returnItem = dh.CreateComponent(typeof(MdiWindowListItem)) as MdiWindowListItem;
                    break;
                case "DevComponents.DotNetBar.SideBarContainerItem":
                    if (name != "")
                        returnItem = dh.CreateComponent(typeof(SideBarContainerItem), name) as SideBarContainerItem;
                    else
                        returnItem = dh.CreateComponent(typeof(SideBarContainerItem)) as SideBarContainerItem;
                    break;
                case "DevComponents.DotNetBar.SideBarPanelItem":
                    if (name != "")
                        returnItem = dh.CreateComponent(typeof(SideBarPanelItem), name) as SideBarPanelItem;
                    else
                        returnItem = dh.CreateComponent(typeof(SideBarPanelItem)) as SideBarPanelItem;
                    break;
                case "DevComponents.DotNetBar.ExplorerBarGroupItem":
                    if (name != "")
                        returnItem = dh.CreateComponent(typeof(ExplorerBarGroupItem), name) as ExplorerBarGroupItem;
                    else
                        returnItem = dh.CreateComponent(typeof(ExplorerBarGroupItem)) as ExplorerBarGroupItem;
                    break;
                case "DevComponents.DotNetBar.ExplorerBarContainerItem":
                    if (name != "")
                        returnItem = dh.CreateComponent(typeof(ExplorerBarContainerItem), name) as ExplorerBarContainerItem;
                    else
                        returnItem = dh.CreateComponent(typeof(ExplorerBarContainerItem)) as ExplorerBarContainerItem;
                    break;
                case "DevComponents.DotNetBar.ProgressBarItem":
                    if (name != "")
                        returnItem = dh.CreateComponent(typeof(ProgressBarItem), name) as ProgressBarItem;
                    else
                        returnItem = dh.CreateComponent(typeof(ProgressBarItem)) as ProgressBarItem;
                    break;
                case "DevComponents.DotNetBar.ColorPickerDropDown":
                    if (name != "")
                        returnItem = dh.CreateComponent(typeof(ColorPickerDropDown), name) as ColorPickerDropDown;
                    else
                        returnItem = dh.CreateComponent(typeof(ColorPickerDropDown)) as ColorPickerDropDown;
                    break;
                default:
                    {
                        try
                        {
                            //System.Windows.Forms.MessageBox.Show("Loading custom: "+xmlItem.GetAttribute("assembly")+"   "+xmlItem.GetAttribute("class"));
                            System.Reflection.Assembly a = System.Reflection.Assembly.Load(xmlItem.GetAttribute("assembly"));
                            if (a == null)
                                return null;
                            BaseItem item = a.CreateInstance(xmlItem.GetAttribute("class")) as BaseItem;
                            returnItem = dh.CreateComponent(item.GetType()) as BaseItem;
                        }
                        catch (Exception e)
                        {
                            throw new ArgumentException("Could not create item from XML. Assembly=" + xmlItem.GetAttribute("assembly") + ", Class=" + xmlItem.GetAttribute("class") + ", Inner Exception: " + e.Message + ", Source=" + e.Source);
                        }
                        break;
                    }
            }
            return returnItem;
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

        internal static void PaintSystemButton(System.Drawing.Graphics g, SystemButton btn, Rectangle r, bool MouseDown, bool MouseOver, bool Disabled)
        {
            // Draw state if any
            if (MouseDown)
            {
                g.FillRectangle(new SolidBrush(ColorFunctions.PressedBackColor(g)), r);
                NativeFunctions.DrawRectangle(g, SystemPens.Highlight, r);
            }
            else if (MouseOver)
            {
                g.FillRectangle(new SolidBrush(ColorFunctions.HoverBackColor(g)), r);
                NativeFunctions.DrawRectangle(g, SystemPens.Highlight, r);
            }

            Bitmap bmp = new Bitmap(r.Width, r.Height, g);
            Graphics gBmp = Graphics.FromImage(bmp);
            Rectangle rBtn = new Rectangle(0, 0, r.Width, r.Height);
            rBtn.Inflate(0, -1);
            Rectangle rClip = rBtn;
            rClip.Inflate(-1, -1);
            using (SolidBrush brush = new SolidBrush(SystemColors.Control))
                gBmp.FillRectangle(brush, 0, 0, r.Width, r.Height);
            gBmp.SetClip(rClip);
            System.Windows.Forms.ControlPaint.DrawCaptionButton(gBmp, rBtn, (System.Windows.Forms.CaptionButton)btn, System.Windows.Forms.ButtonState.Flat);
            gBmp.ResetClip();
            gBmp.Dispose();

            bmp.MakeTransparent(SystemColors.Control);
            if (Disabled)
            {
                float[][] array = new float[5][];
                array[0] = new float[5] { 0, 0, 0, 0, 0 };
                array[1] = new float[5] { 0, 0, 0, 0, 0 };
                array[2] = new float[5] { 0, 0, 0, 0, 0 };
                array[3] = new float[5] { .5f, .5f, .5f, .5f, 0 };
                array[4] = new float[5] { 0, 0, 0, 0, 0 };
                System.Drawing.Imaging.ColorMatrix grayMatrix = new System.Drawing.Imaging.ColorMatrix(array);
                System.Drawing.Imaging.ImageAttributes disabledImageAttr = new System.Drawing.Imaging.ImageAttributes();
                disabledImageAttr.ClearColorKey();
                disabledImageAttr.SetColorMatrix(grayMatrix);
                g.DrawImage(bmp, r, 0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel, disabledImageAttr);
            }
            else
            {
                if (MouseDown)
                    r.Offset(1, 1);
                g.DrawImageUnscaled(bmp, r);
            }

        }

        internal static void SyncProperty(BaseItem item, string propertyName)
        {
            if (item.GlobalName.Length > 0)
            {
                PropertyDescriptor propDesc = TypeDescriptor.GetProperties(item)[propertyName];
                SetPropertyByGlobalName(GetOwner(item), item.GetType(), item.GlobalName, propDesc, propDesc.GetValue(item));
            }
            else if (item.Name.Length > 0)
            {
                PropertyDescriptor propDesc = TypeDescriptor.GetProperties(item)[propertyName];
                SetProperty(GetOwner(item), item.GetType(), item.Name, propDesc, propDesc.GetValue(item));
            }
        }

        private static object GetOwner(BaseItem item)
        {
            object owner = item.GetOwner();
            if (owner is RibbonBar && ((RibbonBar)owner).IsOverflowRibbon)
            {
                if (((RibbonBar)owner).IsOnQat)
                    owner = ((RibbonBar)owner).QatButtonParent.GetOwner();
                else
                    owner = ((RibbonBar)owner).OverflowParent;
            }
            return owner;
        }

        internal static void SetProperty(object owner, System.Type itemType, string itemName, System.ComponentModel.PropertyDescriptor prop, object value)
        {
            IOwner manager = owner as IOwner;
            DotNetBarManager dnbmanager = owner as DotNetBarManager;

            if (manager == null || itemName == "" || prop == null)
                return;

            System.Collections.ArrayList list = null;
            if (dnbmanager != null)
            {
                if (!dnbmanager.IsDisposed)
                    list = dnbmanager.GetItems(itemName, itemType, true);
            }
            else
                list = manager.GetItems(itemName, itemType);
            if (list == null)
                return;
            foreach (BaseItem objItem in list)
            {
                object propertyValue = prop.GetValue(objItem);
                if (!(propertyValue == value || propertyValue != null && propertyValue.Equals(value)))
                    prop.SetValue(objItem, value);
            }
        }

        internal static void SetPropertyByGlobalName(object owner, System.Type itemType, string itemName, System.ComponentModel.PropertyDescriptor prop, object value)
        {
            IOwner manager = owner as IOwner;
            DotNetBarManager dnbmanager = owner as DotNetBarManager;

            if (manager == null || itemName == "" || prop == null)
                return;

            System.Collections.ArrayList list = null;
            if (dnbmanager != null)
            {
                if (!dnbmanager.IsDisposed)
                    list = dnbmanager.GetItems(itemName, itemType, true, true);
            }
            else
                list = manager.GetItems(itemName, itemType, true);
            if (list == null)
                return;
            foreach (BaseItem objItem in list)
            {
                if (prop.GetValue(objItem) != value)
                    prop.SetValue(objItem, value);
            }
        }

        internal static ResourceManager GetResourceManager(bool bDefault)
        {
            string defaultResource = DEFAULT_RESOURCE;
            DotNetBarResourcesAttribute att = Attribute.GetCustomAttribute(System.Reflection.Assembly.GetExecutingAssembly(), typeof(DotNetBarResourcesAttribute)) as DotNetBarResourcesAttribute;
            if (att != null && att.NamespacePrefix != "")
                defaultResource = att.NamespacePrefix + defaultResource;
            else
                defaultResource = "DevComponents.DotNetBar" + defaultResource;

            ResourceManager rm = new ResourceManager(defaultResource, System.Reflection.Assembly.GetExecutingAssembly());
            return rm;
        }
        internal static ResourceManager GetResourceManager()
        {
            string defaultResource = DEFAULT_RESOURCE;
            DotNetBarResourcesAttribute att = Attribute.GetCustomAttribute(System.Reflection.Assembly.GetExecutingAssembly(), typeof(DotNetBarResourcesAttribute)) as DotNetBarResourcesAttribute;
            if (att != null && att.NamespacePrefix != "")
                defaultResource = att.NamespacePrefix + defaultResource;
            else
                defaultResource = "DevComponents.DotNetBar" + defaultResource;

            if (ms_ResourceName == "")
            {
                System.Globalization.CultureInfo cu = System.Threading.Thread.CurrentThread.CurrentUICulture;
                System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
                string[] arr = assembly.GetManifestResourceNames();
                int count = 0; // Make sure this exits
                while (cu.LCID != 127 && count < 16)
                {
                    if (assembly.GetManifestResourceInfo(defaultResource + "_" + cu.Name.ToLower() + ".resources") != null)
                    {
                        ms_ResourceName = defaultResource + "_" + cu.Name.ToLower();
                        break;
                    }
                    else if (assembly.GetManifestResourceInfo(defaultResource + "_" + cu.TwoLetterISOLanguageName.ToLower() + ".resources") != null)
                    {
                        ms_ResourceName = defaultResource + "_" + cu.TwoLetterISOLanguageName.ToLower();
                        break;
                    }
                    cu = cu.Parent;
                    count++;
                }
                if (ms_ResourceName == "")
                    ms_ResourceName = defaultResource;
            }

            ResourceManager rm = new ResourceManager(ms_ResourceName, System.Reflection.Assembly.GetExecutingAssembly());
            return rm;
        }

        internal static void DrawBorder(Graphics g, eBorderType bordertype, Rectangle r, Color singleLineColor)
        {
            DrawBorder(g, bordertype, r, singleLineColor, eBorderSide.Left | eBorderSide.Right | eBorderSide.Top | eBorderSide.Bottom);
        }
        internal static void DrawBorder(Graphics g, eBorderType bordertype, Rectangle r, Color singleLineColor, eBorderSide side)
        {
            DrawBorder(g, bordertype, r, singleLineColor, side, System.Drawing.Drawing2D.DashStyle.Solid);
        }
        internal static void DrawBorder(Graphics g, eBorderType bordertype, Rectangle r, Color singleLineColor, eBorderSide side, System.Drawing.Drawing2D.DashStyle borderDashStyle)
        {
            DrawBorder(g, bordertype, r, singleLineColor, side, borderDashStyle, 1);
        }
        internal static void DrawBorder(Graphics g, eBorderType bordertype, Rectangle r, Color singleLineColor, eBorderSide side, System.Drawing.Drawing2D.DashStyle borderDashStyle, int lineWidth)
        {
            System.Windows.Forms.Border3DSide border3dside;
            if (side == eBorderSide.All)
                border3dside = System.Windows.Forms.Border3DSide.All;
            else
                border3dside = (((side | eBorderSide.Left) != 0) ? System.Windows.Forms.Border3DSide.Left : 0) |
                (((side | eBorderSide.Right) != 0) ? System.Windows.Forms.Border3DSide.Right : 0) |
                (((side | eBorderSide.Top) != 0) ? System.Windows.Forms.Border3DSide.Top : 0) |
                (((side | eBorderSide.Bottom) != 0) ? System.Windows.Forms.Border3DSide.Bottom : 0);

            switch (bordertype)
            {
                case eBorderType.Bump:
                    {
                        System.Windows.Forms.ControlPaint.DrawBorder3D(g, r, System.Windows.Forms.Border3DStyle.Bump, border3dside);
                        break;
                    }
                case eBorderType.Etched:
                    System.Windows.Forms.ControlPaint.DrawBorder3D(g, r, System.Windows.Forms.Border3DStyle.Etched, border3dside);
                    break;
                case eBorderType.Raised:
                    System.Windows.Forms.ControlPaint.DrawBorder3D(g, r, System.Windows.Forms.Border3DStyle.RaisedInner, border3dside);
                    break;
                case eBorderType.Sunken:
                    System.Windows.Forms.ControlPaint.DrawBorder3D(g, r, System.Windows.Forms.Border3DStyle.SunkenOuter, border3dside);
                    break;
                case eBorderType.SingleLine:
                    {
                        SmoothingMode sm = g.SmoothingMode;
                        g.SmoothingMode = SmoothingMode.None;
                        using (Pen pen = new Pen(singleLineColor, lineWidth))
                        {
                            pen.DashStyle = borderDashStyle;
                            int offset = lineWidth / 2;
                            if ((side & eBorderSide.Left) != 0)
                                g.DrawLine(pen, r.X + offset, r.Y, r.X + offset, r.Bottom - 1);
                            if ((side & eBorderSide.Top) != 0)
                                g.DrawLine(pen, r.X, r.Y + offset, r.Right - 1, r.Y + offset);
                            if (offset == 0) offset = 1;
                            if ((side & eBorderSide.Right) != 0)
                                g.DrawLine(pen, r.Right - offset, r.Y, r.Right - offset, r.Bottom - 1);
                            if ((side & eBorderSide.Bottom) != 0)
                                g.DrawLine(pen, r.X, r.Bottom - offset, r.Right - 1, r.Bottom - offset);
                        }
                        g.SmoothingMode = sm;
                        break;
                    }
                case eBorderType.DoubleLine:
                    {
                        using (Pen pen = new Pen(singleLineColor, lineWidth))
                        {
                            pen.DashStyle = borderDashStyle;
                            for (int i = 0; i < lineWidth + 1; i += lineWidth)
                            {
                                if ((side & eBorderSide.Left) != 0)
                                    g.DrawLine(pen, r.X, r.Y, r.X, r.Bottom - 1);
                                if ((side & eBorderSide.Top) != 0)
                                    g.DrawLine(pen, r.X, r.Y, r.Right - 1, r.Y);
                                if ((side & eBorderSide.Right) != 0)
                                    g.DrawLine(pen, r.Right - 1, r.Y, r.Right - 1, r.Bottom - 1);
                                if ((side & eBorderSide.Bottom) != 0)
                                    g.DrawLine(pen, r.X, r.Bottom - 1, r.Right - 1, r.Bottom - 1);
                                r.Inflate(-1, -1);
                            }

                        }
                        break;
                    }
                default:
                    break;
            }
        }

        internal static void DrawBorder3D(Graphics g, int x, int y, int width, int height, System.Windows.Forms.Border3DStyle style, System.Windows.Forms.Border3DSide side)
        {
            DrawBorder3D(g, x, y, width, height, style, side, SystemColors.Control, true);
        }
        internal static void DrawBorder3D(Graphics g, int x, int y, int width, int height, System.Windows.Forms.Border3DStyle style, Color clr)
        {
            DrawBorder3D(g, x, y, width, height, style, System.Windows.Forms.Border3DSide.All, clr, true);
        }
        internal static void DrawBorder3D(Graphics g, int x, int y, int width, int height, System.Windows.Forms.Border3DStyle style)
        {
            DrawBorder3D(g, x, y, width, height, style, System.Windows.Forms.Border3DSide.All, SystemColors.Control, true);
        }
        internal static void DrawBorder3D(Graphics g, Rectangle r, System.Windows.Forms.Border3DStyle style, System.Windows.Forms.Border3DSide side, Color baseColor)
        {
            DrawBorder3D(g, r.X, r.Y, r.Width, r.Height, style, side, baseColor, true);
        }
        internal static void DrawBorder3D(Graphics g, Rectangle r, System.Windows.Forms.Border3DStyle style, System.Windows.Forms.Border3DSide side, Color baseColor, bool bFillInner)
        {
            DrawBorder3D(g, r.X, r.Y, r.Width, r.Height, style, side, baseColor, bFillInner);
        }
        internal static void DrawBorder3D(Graphics g, Rectangle r, System.Windows.Forms.Border3DStyle style)
        {
            DrawBorder3D(g, r.X, r.Y, r.Width, r.Height, style, System.Windows.Forms.Border3DSide.All, SystemColors.Control, true);
        }
        internal static void DrawBorder3D(Graphics g, int x, int y, int width, int height, System.Windows.Forms.Border3DStyle style, System.Windows.Forms.Border3DSide side, Color baseColor)
        {
            DrawBorder3D(g, x, y, width, height, style, side, baseColor, true);
        }
        internal static void DrawBorder3D(Graphics g, int x, int y, int width, int height, System.Windows.Forms.Border3DStyle style, System.Windows.Forms.Border3DSide side, Color baseColor, bool bFillInner)
        {
            if (bFillInner)
                g.FillRectangle(new SolidBrush(baseColor), x, y, width, height);

            Color colorLight = System.Windows.Forms.ControlPaint.Light(baseColor);
            Color colorDark = System.Windows.Forms.ControlPaint.Dark(baseColor);
            Pen penLight = null;
            Pen penDark = null;
            Pen penBase = new Pen(baseColor);
            height--;
            width--;
            switch (style)
            {
                case System.Windows.Forms.Border3DStyle.RaisedInner:
                    {
                        penLight = new Pen(colorLight, 1);
                        penDark = new Pen(colorDark, 1);
                        if ((side & System.Windows.Forms.Border3DSide.Top) != 0)
                            g.DrawLine(penLight, x, y, x + width, y);
                        if ((side & System.Windows.Forms.Border3DSide.Left) != 0)
                            g.DrawLine(penLight, x, y, x, y + height);
                        if ((side & System.Windows.Forms.Border3DSide.Right) != 0)
                            g.DrawLine(penDark, x + width, y, x + width, y + height);
                        if ((side & System.Windows.Forms.Border3DSide.Bottom) != 0)
                            g.DrawLine(penDark, x, y + height, x + width, y + height);
                        break;
                    }
                case System.Windows.Forms.Border3DStyle.SunkenOuter:
                    {
                        penLight = new Pen(colorLight, 1);
                        penDark = new Pen(colorDark, 1);
                        if ((side & System.Windows.Forms.Border3DSide.Top) != 0)
                            g.DrawLine(penDark, x, y, x + width, y);
                        if ((side & System.Windows.Forms.Border3DSide.Left) != 0)
                            g.DrawLine(penDark, x, y, x, y + height);
                        if ((side & System.Windows.Forms.Border3DSide.Right) != 0)
                            g.DrawLine(penLight, x + width, y, x + width, y + height);
                        if ((side & System.Windows.Forms.Border3DSide.Bottom) != 0)
                            g.DrawLine(penLight, x, y + height, x + width, y + height);
                        break;
                    }
                case System.Windows.Forms.Border3DStyle.Raised:
                    {
                        if ((side & System.Windows.Forms.Border3DSide.Top) != 0 &&
                            (side & System.Windows.Forms.Border3DSide.Left) != 0 &&
                            (side & System.Windows.Forms.Border3DSide.Right) != 0 &&
                            (side & System.Windows.Forms.Border3DSide.Bottom) != 0)
                        {

                            penLight = new Pen(Color.White, 1);
                            g.DrawRectangle(penBase, x, y, width, height);
                            g.DrawLine(penLight, x + 1, y + 1, x + width - 1, y + 1);
                            g.DrawLine(penLight, x + 1, y + 1, x + 1, y + height - 1);
                        }
                        else
                        {
                            penDark = new Pen(colorLight, 1);
                            penLight = new Pen(System.Windows.Forms.ControlPaint.LightLight(baseColor), 1);
                            if ((side & System.Windows.Forms.Border3DSide.Top) != 0)
                            {
                                g.DrawLine(penDark, x, y, x + width, y);
                                g.DrawLine(penLight, x + 1, y + 1, x + width - 2, y + 1);
                            }
                            if ((side & System.Windows.Forms.Border3DSide.Left) != 0)
                            {
                                g.DrawLine(penDark, x, y, x, y + height);
                                g.DrawLine(penLight, x + 1, y + 1, x + 1, y + height - 2);
                            }

                            penDark.Dispose();
                            penLight.Dispose();

                            penDark = new Pen(System.Windows.Forms.ControlPaint.DarkDark(baseColor), 1);
                            penLight = new Pen(colorDark, 1);

                            if ((side & System.Windows.Forms.Border3DSide.Right) != 0)
                            {
                                g.DrawLine(penDark, x + width, y, x + width, y + height);
                                g.DrawLine(penLight, x + width - 1, y + 1, x + width - 1, y + height - 1);
                            }
                            if ((side & System.Windows.Forms.Border3DSide.Bottom) != 0)
                            {
                                g.DrawLine(penDark, x, y + height, x + width, y + height);
                                g.DrawLine(penLight, x + 1, y + height - 1, x + width - 1, y + height - 1);
                            }
                        }
                        break;
                    }
                case System.Windows.Forms.Border3DStyle.Sunken:
                    {
                        penDark = new Pen(System.Windows.Forms.ControlPaint.DarkDark(baseColor), 1);
                        penLight = new Pen(colorDark, 1);

                        if ((side & System.Windows.Forms.Border3DSide.Top) != 0)
                        {
                            g.DrawLine(penDark, x, y, x + width, y);
                            g.DrawLine(penLight, x + 1, y + 1, x + width - 2, y + 1);
                        }
                        if ((side & System.Windows.Forms.Border3DSide.Left) != 0)
                        {
                            g.DrawLine(penDark, x, y, x, y + height);
                            g.DrawLine(penLight, x + 1, y + 1, x + 1, y + height - 2);
                        }

                        penDark.Dispose();
                        penLight.Dispose();

                        penDark = new Pen(colorLight, 1);
                        penLight = new Pen(System.Windows.Forms.ControlPaint.LightLight(baseColor), 1);

                        if ((side & System.Windows.Forms.Border3DSide.Right) != 0)
                        {
                            g.DrawLine(penDark, x + width, y, x + width, y + height);
                            g.DrawLine(penLight, x + width - 1, y + 1, x + width - 1, y + height - 1);
                        }
                        if ((side & System.Windows.Forms.Border3DSide.Bottom) != 0)
                        {
                            g.DrawLine(penDark, x, y + height, x + width, y + height);
                            g.DrawLine(penLight, x + 1, y + height - 1, x + width - 1, y + height - 1);
                        }
                        break;
                    }
            }
            if (penLight != null)
                penLight.Dispose();
            if (penDark != null)
                penDark.Dispose();
            penBase.Dispose();
        }

        internal static System.Drawing.Drawing2D.LinearGradientBrush CreateLinearGradientBrush(Rectangle r, Color color1, Color color2, float gradientAngle)
        {
            if (r.Width <= 0)
                r.Width = 1;
            if (r.Height <= 0)
                r.Height = 1;
            return new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(r.X, r.Y - 1, r.Width, r.Height + 1), color1, color2, gradientAngle);
        }

        internal static System.Drawing.Drawing2D.LinearGradientBrush CreateLinearGradientBrush(RectangleF r, Color color1, Color color2, float gradientAngle)
        {
            if (r.Width <= 0)
                r.Width = 1;
            if (r.Height <= 0)
                r.Height = 1;
            return new System.Drawing.Drawing2D.LinearGradientBrush(new RectangleF(r.X, r.Y - 1, r.Width, r.Height + 1), color1, color2, gradientAngle);
        }

        internal static System.Drawing.Drawing2D.LinearGradientBrush CreateLinearGradientBrush(Rectangle r, Color color1, Color color2, float gradientAngle, bool isAngleScalable)
        {
            if (r.Width <= 0)
                r.Width = 1;
            if (r.Height <= 0)
                r.Height = 1;
            return new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(r.X, r.Y - 1, r.Width, r.Height + 1), color1, color2, gradientAngle, isAngleScalable);
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

        public static BaseItem GetSubItemByName(BaseItem objParent, string ItemName)
        {
            return GetSubItemByName(objParent, ItemName, false);
        }

        public static BaseItem GetSubItemByName(BaseItem objParent, string ItemName, bool useGlobalName)
        {
            if (objParent == null)
                return null;
            foreach (BaseItem objItem in objParent.SubItems)
            {
                if (useGlobalName && objItem.GlobalName == ItemName || !useGlobalName && objItem.Name == ItemName)
                    return objItem;
                if (objItem.SubItems.Count > 0)
                {
                    BaseItem item = GetSubItemByName(objItem, ItemName, useGlobalName);
                    if (item != null)
                        return item;
                }
            }

            if (objParent is GalleryContainer)
            {
                return GetSubItemByName(((GalleryContainer)objParent).PopupGalleryItem, ItemName, useGlobalName);
            }

            return null;
        }

        public static void GetSubItemsByName(BaseItem objParent, string ItemName, ArrayList list)
        {
            GetSubItemsByName(objParent, ItemName, list, false);
        }

        public static void GetSubItemsByName(BaseItem objParent, string ItemName, ArrayList list, bool useGlobalName)
        {
            if (objParent == null) return;
            foreach (BaseItem objItem in objParent.SubItems)
            {
                if (useGlobalName && objItem.GlobalName == ItemName || !useGlobalName && objItem.Name == ItemName)
                    list.Add(objItem);
                else if (objItem is ControlContainerItem)
                {
                    ControlContainerItem cc = objItem as ControlContainerItem;
                    if (cc.Control is RibbonBar)
                    {
                        RibbonBar rb = cc.Control as RibbonBar;
                        ArrayList rbList = rb.GetItems(ItemName);
                        list.AddRange(rbList);
                    }
                }

                if (objItem.SubItems.Count > 0)
                    GetSubItemsByName(objItem, ItemName, list, useGlobalName);
            }

            if (objParent is GalleryContainer)
            {
                GetSubItemsByName(((GalleryContainer)objParent).PopupGalleryItem, ItemName, list, useGlobalName);
            }
        }

        public static void GetSubItemsByNameAndType(BaseItem objParent, string ItemName, ArrayList list, Type itemType)
        {
            GetSubItemsByNameAndType(objParent, ItemName, list, itemType, false);
        }

        public static void GetSubItemsByNameAndType(BaseItem objParent, string ItemName, ArrayList list, Type itemType, bool useGlobalName)
        {
            if (objParent == null) return;
            foreach (BaseItem objItem in objParent.SubItems)
            {
                if (objItem.GetType() == itemType && (useGlobalName && objItem.GlobalName == ItemName || !useGlobalName && objItem.Name == ItemName))
                    list.Add(objItem);
                else if (objItem is ControlContainerItem)
                {
                    ControlContainerItem cc = objItem as ControlContainerItem;
                    if (cc.Control is RibbonBar)
                    {
                        RibbonBar rb = cc.Control as RibbonBar;
                        ArrayList rbList = rb.GetItems(ItemName, itemType, useGlobalName);
                        list.AddRange(rbList);
                    }
                }

                if (objItem.SubItems.Count > 0)
                    GetSubItemsByNameAndType(objItem, ItemName, list, itemType, useGlobalName);
            }
        }

        public static string ColorToString(Color clr)
        {
            if (clr.IsSystemColor)
                return ("." + clr.Name);
            else
                return clr.ToArgb().ToString();
        }

        public static Color ColorFromString(string sclr)
        {
            if (sclr == "")
                return Color.Empty;
            if (sclr[0] == '.')
                return Color.FromName(sclr.Substring(1));
            else
                return Color.FromArgb(System.Xml.XmlConvert.ToInt32(sclr));
        }

        public static bool SupportsAnimation
        {
            get { return m_SupportsAnimation; }
        }

        internal static ScreenInformation ScreenFromPoint(Point pScreen)
        {
            if (m_Screens.Count == 0)
                RefreshScreens();
            //foreach (System.Windows.Forms.Screen s in System.Windows.Forms.Screen.AllScreens) 
            foreach (ScreenInformation s in m_Screens)
            {
                if (s.Bounds.Contains(pScreen))
                {
                    return s;
                }
            }

            System.Windows.Forms.Screen scr = System.Windows.Forms.Screen.FromPoint(pScreen);
            if (scr != null)
                return new ScreenInformation(scr.Bounds, scr.WorkingArea);

            return null;
        }
        internal static ScreenInformation ScreenFromControl(System.Windows.Forms.Control control)
        {
            Rectangle r;
            if (control.Parent != null)
            {
                Point screenLocation = control.PointToScreen(control.Location);
                r = new Rectangle(screenLocation, control.Size);
            }
            else
                r = new Rectangle(control.Location, control.Size);
            if (m_Screens.Count == 0)
                RefreshScreens();
            //foreach (System.Windows.Forms.Screen s in System.Windows.Forms.Screen.AllScreens)
            foreach (ScreenInformation s in m_Screens)
            {
                //if(s.Bounds.Contains(r)) 
                if (s.Bounds.Contains(r))
                {
                    return s;
                }
            }
            System.Windows.Forms.Screen scr = System.Windows.Forms.Screen.FromControl(control);
            if (scr != null)
                return new ScreenInformation(scr.Bounds, scr.WorkingArea);
            return null;
        }

        private static ArrayList m_Screens = new ArrayList(2);
        public static void RefreshScreens()
        {
            m_Screens.Clear();
            foreach (System.Windows.Forms.Screen s in System.Windows.Forms.Screen.AllScreens)
                m_Screens.Add(new ScreenInformation(s.Bounds, s.WorkingArea));
        }

        public static void SetExplorerBarStyle(ExplorerBar bar, eExplorerBarStockStyle stockStyle)
        {
            if (stockStyle == eExplorerBarStockStyle.SystemColors)
            {
                bar.BackStyle.Reset();
                bar.BackStyle.BackColorSchemePart = eColorSchemePart.ExplorerBarBackground;
                bar.BackStyle.BackColor2SchemePart = eColorSchemePart.ExplorerBarBackground2;
                bar.BackStyle.BackColorGradientAngle = bar.ColorScheme.ExplorerBarBackgroundGradientAngle;
            }
            else if (stockStyle != eExplorerBarStockStyle.Custom)
            {
                ePredefinedColorScheme scheme = ePredefinedColorScheme.Blue2003;
                if (stockStyle == eExplorerBarStockStyle.Silver || stockStyle == eExplorerBarStockStyle.SilverSpecial)
                    scheme = ePredefinedColorScheme.Silver2003;
                else if (stockStyle == eExplorerBarStockStyle.OliveGreen || stockStyle == eExplorerBarStockStyle.OliveGreenSpecial)
                    scheme = ePredefinedColorScheme.OliveGreen2003;
                ColorScheme cs = new ColorScheme(eDotNetBarStyle.Office2003);
                cs.PredefinedColorScheme = scheme;

                bar.BackStyle.Reset();
                bar.BackStyle.BackColor = cs.ExplorerBarBackground;
                bar.BackStyle.BackColor2 = cs.ExplorerBarBackground2;
                bar.BackStyle.BackColorGradientAngle = cs.ExplorerBarBackgroundGradientAngle;
            }
        }
        public static void SetExplorerBarStyle(ExplorerBarGroupItem group, eExplorerBarStockStyle stockStyle)
        {
            if (stockStyle == eExplorerBarStockStyle.SystemColors)
            {
                eExplorerBarStockStyle stock = eExplorerBarStockStyle.Blue;
                eExplorerBarStockStyle special = eExplorerBarStockStyle.BlueSpecial;

                if (SystemColors.Control.ToArgb() == Color.FromArgb(224, 223, 227).ToArgb() && SystemColors.Highlight.ToArgb() == Color.FromArgb(178, 180, 191).ToArgb())
                {
                    stock = eExplorerBarStockStyle.Silver;
                    special = eExplorerBarStockStyle.SilverSpecial;
                }
                else if (SystemColors.Control.ToArgb() == Color.FromArgb(236, 233, 216).ToArgb() && SystemColors.Highlight.ToArgb() == Color.FromArgb(147, 160, 112).ToArgb())
                {
                    stock = eExplorerBarStockStyle.OliveGreen;
                    special = eExplorerBarStockStyle.OliveGreenSpecial;
                }

                if (group.XPSpecialGroup)
                    stockStyle = special;
                else
                    stockStyle = stock;
            }

            if (stockStyle != eExplorerBarStockStyle.Custom)
            {
                group.TitleStyle.Reset();
                group.TitleHotStyle.Reset();
                group.BackStyle.Reset();

                group.TitleStyle.CornerTypeTopLeft = eCornerType.Rounded;
                group.TitleStyle.CornerTypeTopRight = eCornerType.Rounded;
                group.TitleStyle.CornerDiameter = 3;
                group.TitleHotStyle.CornerTypeTopLeft = eCornerType.Rounded;
                group.TitleHotStyle.CornerTypeTopRight = eCornerType.Rounded;
                group.TitleHotStyle.CornerDiameter = 3;
            }

            switch (stockStyle)
            {
                case eExplorerBarStockStyle.Blue:
                    {
                        group.TitleStyle.BackColor = Color.White;
                        group.TitleStyle.BackColor2 = Color.FromArgb(199, 211, 247);
                        group.TitleStyle.TextColor = Color.FromArgb(33, 93, 198);
                        group.TitleHotStyle.TextColor = Color.FromArgb(66, 142, 255);
                        group.TitleHotStyle.BackColor = Color.White;
                        group.TitleHotStyle.BackColor2 = Color.FromArgb(199, 211, 247); ;
                        group.BackStyle.BackColor = Color.FromArgb(214, 223, 247);
                        group.BackStyle.BorderBottom = eStyleBorderType.Solid;
                        group.BackStyle.BorderTop = eStyleBorderType.None;
                        group.BackStyle.BorderLeft = eStyleBorderType.Solid;
                        group.BackStyle.BorderRight = eStyleBorderType.Solid;
                        group.BackStyle.BorderBottomWidth = 1;
                        group.BackStyle.BorderTopWidth = 0;
                        group.BackStyle.BorderLeftWidth = 1;
                        group.BackStyle.BorderRightWidth = 1;
                        group.BackStyle.BorderColor = Color.White;
                        group.ExpandBackColor = Color.White;
                        group.ExpandBorderColor = Color.FromArgb(174, 182, 216);
                        group.ExpandForeColor = Color.FromArgb(0, 60, 165);
                        group.ExpandHotBackColor = Color.White;
                        group.ExpandHotBorderColor = Color.FromArgb(174, 182, 216);
                        group.ExpandHotForeColor = Color.FromArgb(66, 142, 255);

                        break;
                    }
                case eExplorerBarStockStyle.BlueSpecial:
                    {
                        group.TitleStyle.BackColor = Color.FromArgb(0, 73, 181);
                        group.TitleStyle.BackColor2 = Color.FromArgb(41, 93, 206);
                        group.TitleStyle.TextColor = Color.White;
                        group.TitleHotStyle.TextColor = Color.FromArgb(66, 142, 255);
                        group.TitleHotStyle.BackColor = Color.FromArgb(0, 73, 181);
                        group.TitleHotStyle.BackColor2 = Color.FromArgb(41, 93, 206);
                        group.BackStyle.Reset();
                        group.BackStyle.BackColor = Color.FromArgb(239, 243, 255);
                        group.BackStyle.BorderBottom = eStyleBorderType.Solid;
                        group.BackStyle.BorderTop = eStyleBorderType.None;
                        group.BackStyle.BorderLeft = eStyleBorderType.Solid;
                        group.BackStyle.BorderRight = eStyleBorderType.Solid;
                        group.BackStyle.BorderBottomWidth = 1;
                        group.BackStyle.BorderTopWidth = 0;
                        group.BackStyle.BorderLeftWidth = 1;
                        group.BackStyle.BorderRightWidth = 1;
                        group.BackStyle.BorderColor = Color.White;

                        group.ExpandBackColor = Color.FromArgb(48, 97, 196);
                        group.ExpandBorderColor = Color.FromArgb(123, 168, 229);
                        group.ExpandForeColor = Color.White;
                        group.ExpandHotBackColor = Color.FromArgb(48, 97, 196);
                        group.ExpandHotBorderColor = Color.FromArgb(123, 168, 229);
                        group.ExpandHotForeColor = Color.FromArgb(172, 205, 255);

                        break;
                    }
                case eExplorerBarStockStyle.OliveGreen:
                    {
                        group.TitleStyle.BackColor = Color.FromArgb(255, 252, 236);
                        group.TitleStyle.BackColor2 = Color.FromArgb(224, 231, 184);
                        group.TitleStyle.TextColor = Color.FromArgb(86, 102, 45);
                        group.TitleHotStyle.TextColor = Color.FromArgb(114, 146, 29);
                        group.TitleHotStyle.BackColor = Color.FromArgb(255, 252, 236);
                        group.TitleHotStyle.BackColor2 = Color.FromArgb(224, 231, 184);
                        group.BackStyle.Reset();
                        group.BackStyle.BackColor = Color.FromArgb(246, 246, 236);
                        group.BackStyle.BorderBottom = eStyleBorderType.Solid;
                        group.BackStyle.BorderTop = eStyleBorderType.None;
                        group.BackStyle.BorderLeft = eStyleBorderType.Solid;
                        group.BackStyle.BorderRight = eStyleBorderType.Solid;
                        group.BackStyle.BorderBottomWidth = 1;
                        group.BackStyle.BorderTopWidth = 0;
                        group.BackStyle.BorderLeftWidth = 1;
                        group.BackStyle.BorderRightWidth = 1;
                        group.BackStyle.BorderColor = Color.White;
                        group.ExpandBackColor = Color.FromArgb(254, 254, 253);
                        group.ExpandBorderColor = Color.FromArgb(194, 206, 185);
                        group.ExpandForeColor = Color.FromArgb(75, 103, 28);
                        group.ExpandHotBackColor = Color.FromArgb(254, 254, 253);
                        group.ExpandHotBorderColor = Color.FromArgb(194, 206, 185);
                        group.ExpandHotForeColor = Color.FromArgb(114, 146, 29);

                        break;
                    }
                case eExplorerBarStockStyle.OliveGreenSpecial:
                    {
                        group.TitleStyle.BackColor = Color.FromArgb(119, 140, 64);
                        group.TitleStyle.BackColor2 = Color.FromArgb(150, 168, 103);
                        group.TitleStyle.TextColor = Color.White;
                        group.TitleHotStyle.TextColor = Color.FromArgb(224, 231, 184);
                        group.TitleHotStyle.BackColor = Color.FromArgb(119, 140, 64);
                        group.TitleHotStyle.BackColor2 = Color.FromArgb(150, 168, 103);
                        group.BackStyle.Reset();
                        group.BackStyle.BackColor = Color.FromArgb(246, 246, 236);
                        group.BackStyle.BorderBottom = eStyleBorderType.Solid;
                        group.BackStyle.BorderTop = eStyleBorderType.None;
                        group.BackStyle.BorderLeft = eStyleBorderType.Solid;
                        group.BackStyle.BorderRight = eStyleBorderType.Solid;
                        group.BackStyle.BorderBottomWidth = 1;
                        group.BackStyle.BorderTopWidth = 0;
                        group.BackStyle.BorderLeftWidth = 1;
                        group.BackStyle.BorderRightWidth = 1;
                        group.BackStyle.BorderColor = Color.White;

                        group.ExpandBackColor = Color.FromArgb(129, 163, 79);
                        group.ExpandBorderColor = Color.FromArgb(191, 205, 156);
                        group.ExpandForeColor = Color.White;
                        group.ExpandHotBackColor = Color.FromArgb(130, 164, 80);
                        group.ExpandHotBorderColor = Color.FromArgb(182, 202, 139);
                        group.ExpandHotForeColor = Color.FromArgb(221, 237, 190);

                        break;
                    }
                case eExplorerBarStockStyle.Silver:
                    {
                        group.TitleStyle.BackColor = Color.White;
                        group.TitleStyle.BackColor2 = Color.FromArgb(214, 215, 224);
                        group.TitleStyle.TextColor = Color.FromArgb(63, 61, 61);
                        group.TitleHotStyle.TextColor = Color.FromArgb(126, 124, 124);
                        group.TitleHotStyle.BackColor = Color.White;
                        group.TitleHotStyle.BackColor2 = Color.FromArgb(214, 215, 224);
                        group.BackStyle.Reset();
                        group.BackStyle.BackColor = Color.FromArgb(240, 241, 245);
                        group.BackStyle.BorderBottom = eStyleBorderType.Solid;
                        group.BackStyle.BorderTop = eStyleBorderType.None;
                        group.BackStyle.BorderLeft = eStyleBorderType.Solid;
                        group.BackStyle.BorderRight = eStyleBorderType.Solid;
                        group.BackStyle.BorderBottomWidth = 1;
                        group.BackStyle.BorderTopWidth = 0;
                        group.BackStyle.BorderLeftWidth = 1;
                        group.BackStyle.BorderRightWidth = 1;
                        group.BackStyle.BorderColor = Color.White;

                        group.ExpandBackColor = Color.White;
                        group.ExpandBorderColor = Color.FromArgb(188, 189, 203);
                        group.ExpandForeColor = Color.FromArgb(49, 68, 115);
                        group.ExpandHotBackColor = Color.White;
                        group.ExpandHotBorderColor = Color.FromArgb(194, 195, 208);
                        group.ExpandHotForeColor = Color.FromArgb(126, 124, 124);

                        break;
                    }
                case eExplorerBarStockStyle.SilverSpecial:
                    {
                        group.TitleStyle.BackColor = Color.FromArgb(119, 119, 146);
                        group.TitleStyle.BackColor2 = Color.FromArgb(180, 182, 199);
                        group.TitleStyle.TextColor = Color.White;
                        group.TitleHotStyle.BackColor = Color.FromArgb(119, 119, 146);
                        group.TitleHotStyle.BackColor2 = Color.FromArgb(180, 182, 199);
                        group.TitleHotStyle.TextColor = Color.FromArgb(230, 230, 230);
                        group.BackStyle.Reset();
                        group.BackStyle.BackColor = Color.FromArgb(240, 241, 245);
                        group.BackStyle.BorderBottom = eStyleBorderType.Solid;
                        group.BackStyle.BorderTop = eStyleBorderType.None;
                        group.BackStyle.BorderLeft = eStyleBorderType.Solid;
                        group.BackStyle.BorderRight = eStyleBorderType.Solid;
                        group.BackStyle.BorderBottomWidth = 1;
                        group.BackStyle.BorderTopWidth = 0;
                        group.BackStyle.BorderLeftWidth = 1;
                        group.BackStyle.BorderRightWidth = 1;
                        group.BackStyle.BorderColor = Color.White;

                        group.ExpandBackColor = Color.FromArgb(111, 117, 151);
                        group.ExpandBorderColor = Color.FromArgb(196, 203, 224);
                        group.ExpandForeColor = Color.White;
                        group.ExpandHotBackColor = Color.FromArgb(111, 117, 151);
                        group.ExpandHotBorderColor = Color.FromArgb(196, 203, 224);
                        group.ExpandHotForeColor = Color.White;

                        break;
                    }
            }
        }
        public static void SetExplorerBarStyle(ButtonItem item, eExplorerBarStockStyle stockStyle)
        {
            if (stockStyle == eExplorerBarStockStyle.SystemColors)
            {
                eExplorerBarStockStyle stock = eExplorerBarStockStyle.Blue;

                if (SystemColors.Control.ToArgb() == Color.FromArgb(224, 223, 227).ToArgb() && SystemColors.Highlight.ToArgb() == Color.FromArgb(178, 180, 191).ToArgb())
                {
                    stock = eExplorerBarStockStyle.Silver;
                }
                else if (SystemColors.Control.ToArgb() == Color.FromArgb(236, 233, 216).ToArgb() && SystemColors.Highlight.ToArgb() == Color.FromArgb(147, 160, 112).ToArgb())
                {
                    stock = eExplorerBarStockStyle.OliveGreen;
                }

                stockStyle = stock;
            }

            switch (stockStyle)
            {
                case eExplorerBarStockStyle.Blue:
                case eExplorerBarStockStyle.BlueSpecial:
                    {
                        item.ForeColor = Color.FromArgb(33, 93, 198);
                        item.HotForeColor = Color.FromArgb(66, 142, 255);
                        break;
                    }
                case eExplorerBarStockStyle.OliveGreen:
                case eExplorerBarStockStyle.OliveGreenSpecial:
                    {
                        item.ForeColor = Color.FromArgb(86, 102, 45);
                        item.HotForeColor = Color.FromArgb(114, 146, 29);
                        break;
                    }
                case eExplorerBarStockStyle.Silver:
                case eExplorerBarStockStyle.SilverSpecial:
                    {
                        item.ForeColor = Color.FromArgb(63, 61, 61);
                        item.HotForeColor = Color.FromArgb(126, 124, 124);
                        break;
                    }
                default:
                    {
                        item.ForeColor = SystemColors.ControlText;
                        item.HotForeColor = SystemColors.ControlDark;
                        break;
                    }
            }
        }

        public static System.Windows.Forms.MdiClient GetMdiClient(System.Windows.Forms.Form MdiForm)
        {
            if (!MdiForm.IsMdiContainer)
                return null;
            foreach (System.Windows.Forms.Control ctrl in MdiForm.Controls)
            {
                if (ctrl is System.Windows.Forms.MdiClient)
                    return (ctrl as System.Windows.Forms.MdiClient);
            }
            return null;
        }

        //internal static Bitmap CreateDisabledBitmap(Bitmap bmp)
        //{
        //    if(bmp==null)
        //        return null;

        //    Bitmap bmpTarget=null;
        //    try
        //    {
        //        int nWidth = bmp.Width;
        //        int nHeight = bmp.Height;
        //        bmpTarget = new Bitmap(bmp);
        //        for (int iX = 0; iX < nWidth; iX++)
        //        {
        //            for (int iY = 0; iY < nHeight; iY++)
        //            {
        //                Color cr = bmp.GetPixel(iX, iY);
        //                if(cr.IsEmpty || cr==Color.Transparent)
        //                {
        //                    bmpTarget.SetPixel(iX, iY, Color.Transparent);
        //                }
        //                else
        //                {
        //                    byte nA = cr.A;
        //                    byte nB = (byte) ((cr.B + 255) / 2);
        //                    byte nG = (byte) ((cr.G + 255) / 2);
        //                    byte nR = (byte) ((cr.R + 255) / 2);
        //                    nR = nG = nB = (byte) (nR * 0.299 + nG * 0.587 + nB * 0.114);
        //                    bmpTarget.SetPixel(iX, iY, Color.FromArgb(nA, nR, nG, nB));
        //                }
        //            }
        //        }
        //    }
        //    catch
        //    {
        //        return null;
        //    }

        //    return bmpTarget;
        //}
        internal static Icon CreateDisabledIcon(Icon ico)
        {
            try
            {
                System.IO.MemoryStream memStream = new System.IO.MemoryStream();
                ico.Save(memStream);
                byte[] array = memStream.ToArray();
                int nIconCount = array[4];
                for (int iIcon = 0; iIcon < nIconCount; iIcon++)
                {
                    int nBaseOffset = 6 + iIcon * 16;
                    int nWidth = array[nBaseOffset + 0];
                    int nHeight = array[nBaseOffset + 1];
                    int nOffset = GetDWORD(ref array, nBaseOffset + 12);
                    //data at position
                    int nStructSize = GetDWORD(ref array, nOffset);
                    int nPlanes = GetWORD(ref array, nOffset + 12);
                    int nBitCount = GetWORD(ref array, nOffset + 14);
                    //process 32-bit Icons (bitcount=32)

                    //process 24-bit icons (bitcount=24)

                    //process 8-bit (256 color) icons (bitcount=8)
                    if (nPlanes != 1) continue;

                    int nSize = GetDWORD(ref array, nOffset + 20);
                    int nStep = 0;

                    switch (nBitCount)
                    {
                        case 32: nStep = 4; nSize = nWidth * nHeight * 4; break;
                        case 24: nStep = 3; nSize = nWidth * nHeight * 3; break;
                        case 8: nStep = 4; nSize = 1024; break;           //256 colors x 4 bytes
                        default: continue;
                    }

                    int iDataStart = nOffset + nStructSize;
                    for (int iPtr = iDataStart; iPtr < iDataStart + nSize; iPtr += nStep)
                    {
                        byte nB = (byte)((array[iPtr] + 255) / 2);
                        byte nG = (byte)((array[iPtr + 1] + 255) / 2);
                        byte nR = (byte)((array[iPtr + 2] + 255) / 2);
                        if (!(nB == 127 && nG == 127 && nR == 127))
                        {
                            byte nNewRGB = (byte)(nR * 0.299 + nG * 0.587 + nB * 0.114);
                            array[iPtr] = nNewRGB;
                            array[iPtr + 1] = nNewRGB;
                            array[iPtr + 2] = nNewRGB;
                        }
                    }
                }
                return new Icon(new System.IO.MemoryStream(array));
            }
            catch (Exception)
            {
                return null;
            }
        }
        private static int GetDWORD(ref byte[] array, int offset)
        {
            return array[offset] + (array[offset + 1] << 8) + (array[offset + 2] << 16) + (array[offset + 3] << 24);
        }
        private static int GetWORD(ref byte[] array, int offset)
        {
            return array[offset] + (array[offset + 1] << 8);
        }

        internal static void PaintBackgroundImage(Graphics g, Rectangle targetRect, Image backgroundImage, eStyleBackgroundImage backgroundImagePosition, int backgroundImageAlpha)
        {
            PaintBackgroundImage(g, targetRect, backgroundImage, (eBackgroundImagePosition)backgroundImagePosition, backgroundImageAlpha);
        }

        internal static void PaintBackgroundImage(Graphics g, Rectangle targetRect, Image backgroundImage, eBackgroundImagePosition backgroundImagePosition, int backgroundImageAlpha)
        {
            if (backgroundImage == null)
                return;

            Rectangle r = targetRect;
            System.Drawing.Imaging.ImageAttributes imageAtt = null;

            if (backgroundImageAlpha != 255)
            {
                float[][] matrixItems ={ 
                   new float[] {1, 0, 0, 0, 0},
                   new float[] {0, 1, 0, 0, 0},
                   new float[] {0, 0, 1, 0, 0},
                   new float[] {0, 0, 0, (float)backgroundImageAlpha/255, 0}, 
                   new float[] {0, 0, 0, 0, 1}};
                System.Drawing.Imaging.ColorMatrix colorMatrix = new System.Drawing.Imaging.ColorMatrix(matrixItems);

                //System.Drawing.Imaging.ColorMatrix colorMatrix = new System.Drawing.Imaging.ColorMatrix();
                //colorMatrix.Matrix33 = 255 - backgroundImageAlpha;
                imageAtt = new System.Drawing.Imaging.ImageAttributes();
                imageAtt.SetColorMatrix(colorMatrix, System.Drawing.Imaging.ColorMatrixFlag.Default, System.Drawing.Imaging.ColorAdjustType.Bitmap);
            }

            switch (backgroundImagePosition)
            {
                case eBackgroundImagePosition.Stretch:
                    {
                        if (imageAtt != null)
                            g.DrawImage(backgroundImage, r, 0, 0, backgroundImage.Width, backgroundImage.Height, GraphicsUnit.Pixel, imageAtt);
                        else
                            g.DrawImage(backgroundImage, r, 0, 0, backgroundImage.Width, backgroundImage.Height, GraphicsUnit.Pixel);
                        break;
                    }
                case eBackgroundImagePosition.CenterLeft:
                case eBackgroundImagePosition.CenterRight:
                    {
                        Rectangle destRect = new Rectangle(r.X, r.Y, backgroundImage.Width, backgroundImage.Height);
                        if (r.Width > backgroundImage.Width && backgroundImagePosition == eBackgroundImagePosition.CenterRight)
                            destRect.X += (r.Width - backgroundImage.Width);
                        destRect.Y += (r.Height - backgroundImage.Height) / 2;
                        if (imageAtt != null)
                            g.DrawImage(backgroundImage, destRect, 0, 0, backgroundImage.Width, backgroundImage.Height, GraphicsUnit.Pixel, imageAtt);
                        else
                            g.DrawImage(backgroundImage, destRect, 0, 0, backgroundImage.Width, backgroundImage.Height, GraphicsUnit.Pixel);

                        break;
                    }
                case eBackgroundImagePosition.Center:
                    {
                        Rectangle destRect = new Rectangle(r.X, r.Y, backgroundImage.Width, backgroundImage.Height);
                        if (r.Width > backgroundImage.Width)
                            destRect.X += (r.Width - backgroundImage.Width) / 2;
                        if (r.Height > backgroundImage.Height)
                            destRect.Y += (r.Height - backgroundImage.Height) / 2;
                        if (imageAtt != null)
                            g.DrawImage(backgroundImage, destRect, 0, 0, backgroundImage.Width, backgroundImage.Height, GraphicsUnit.Pixel, imageAtt);
                        else
                            g.DrawImage(backgroundImage, destRect, 0, 0, backgroundImage.Width, backgroundImage.Height, GraphicsUnit.Pixel);
                        break;
                    }
                case eBackgroundImagePosition.TopLeft:
                case eBackgroundImagePosition.TopRight:
                case eBackgroundImagePosition.BottomLeft:
                case eBackgroundImagePosition.BottomRight:
                    {
                        Rectangle destRect = new Rectangle(r.X, r.Y, backgroundImage.Width, backgroundImage.Height);
                        if (backgroundImagePosition == eBackgroundImagePosition.TopRight)
                            destRect.X = r.Right - backgroundImage.Width;
                        else if (backgroundImagePosition == eBackgroundImagePosition.BottomLeft)
                            destRect.Y = r.Bottom - backgroundImage.Height;
                        else if (backgroundImagePosition == eBackgroundImagePosition.BottomRight)
                        {
                            destRect.Y = r.Bottom - backgroundImage.Height;
                            destRect.X = r.Right - backgroundImage.Width;
                        }

                        if (imageAtt != null)
                            g.DrawImage(backgroundImage, destRect, 0, 0, backgroundImage.Width, backgroundImage.Height, GraphicsUnit.Pixel, imageAtt);
                        else
                            g.DrawImage(backgroundImage, destRect, 0, 0, backgroundImage.Width, backgroundImage.Height, GraphicsUnit.Pixel);
                        break;
                    }
                case eBackgroundImagePosition.Tile:
                    {
                        if (imageAtt != null)
                        {
                            if (r.Width > backgroundImage.Width || r.Height > backgroundImage.Height)
                            {
                                int x = r.X, y = r.Y;
                                while (y < r.Bottom)
                                {
                                    while (x < r.Right)
                                    {
                                        Rectangle destRect = new Rectangle(x, y, backgroundImage.Width, backgroundImage.Height);
                                        if (destRect.Right > r.Right)
                                            destRect.Width = destRect.Width - (destRect.Right - r.Right);
                                        if (destRect.Bottom > r.Bottom)
                                            destRect.Height = destRect.Height - (destRect.Bottom - r.Bottom);
                                        g.DrawImage(backgroundImage, destRect, 0, 0, destRect.Width, destRect.Height, GraphicsUnit.Pixel, imageAtt);
                                        x += backgroundImage.Width;
                                    }
                                    x = r.X;
                                    y += backgroundImage.Height;
                                }
                            }
                            else
                            {
                                g.DrawImage(backgroundImage, new Rectangle(0, 0, backgroundImage.Width, backgroundImage.Height), 0, 0, backgroundImage.Width, backgroundImage.Height, GraphicsUnit.Pixel, imageAtt);
                            }
                        }
                        else
                        {
                            SmoothingMode sm = g.SmoothingMode;
                            g.SmoothingMode = SmoothingMode.None;
                            using (TextureBrush brush = new TextureBrush(backgroundImage))
                            {
                                brush.WrapMode = System.Drawing.Drawing2D.WrapMode.Tile;
                                g.FillRectangle(brush, r);
                            }
                            g.SmoothingMode = sm;
                        }
                        break;
                    }
            }
        }

        public static bool IsSystemKey(System.Windows.Forms.Keys key)
        {
            if (key == Keys.Add || key == Keys.Alt || key == Keys.Apps || key == Keys.Attn ||
                key == Keys.Back || key == Keys.Escape || key == Keys.Enter ||
                (key >= Keys.F1 && key <= Keys.F19) || key == Keys.Tab)
                return true;
            return false;
        }

        public static bool IsFormActive(Form f)
        {
            if (f == null) return false;

            if (Form.ActiveForm == f)
            {
                if (f.IsMdiChild)
                {
                    if (f.MdiParent != null)
                    {
                        if (f.MdiParent.ActiveMdiChild == f)
                            return true;
                        else
                            return false;
                    }
                }
                return true;
            }

            return false;
        }

        public static void AnimateControl(System.Windows.Forms.Control control, bool show, int animationTime, Rectangle rectStart, Rectangle rectEnd)
        {
            control.Bounds = rectStart;
            if (!control.Visible)
                control.Visible = true;

            bool directSet = false;
            TimeSpan time = new TimeSpan(0, 0, 0, 0, animationTime);
            int dxLoc, dyLoc;
            int dWidth, dHeight;
            dxLoc = dyLoc = dWidth = dHeight = 0;
            if (rectStart.Left == rectEnd.Left &&
                rectStart.Top == rectEnd.Top &&
                rectStart.Right == rectEnd.Right)
            {
                dHeight = (rectEnd.Height > rectStart.Height ? 1 : -1);
            }
            else if (rectStart.Left == rectEnd.Left &&
                rectStart.Top == rectEnd.Top &&
                rectStart.Bottom == rectEnd.Bottom)
            {
                dWidth = (rectEnd.Width > rectStart.Width ? 1 : -1);
            }
            else if (rectStart.Right == rectEnd.Right &&
                rectStart.Top == rectEnd.Top &&
                rectStart.Bottom == rectEnd.Bottom)
            {
                dxLoc = (rectEnd.Width > rectStart.Width ? -1 : 1);
                dWidth = (rectEnd.Width > rectStart.Width ? 1 : -1);
            }
            else if (rectStart.Right == rectEnd.Right &&
                rectStart.Left == rectEnd.Left &&
                rectStart.Bottom == rectEnd.Bottom)
            {
                dyLoc = (rectEnd.Height > rectStart.Height ? -1 : 1);
                dHeight = (rectEnd.Height > rectStart.Height ? 1 : -1);
            }
            else
                directSet = true;

            if (directSet)
            {
                control.Bounds = rectEnd;
            }
            else
            {
                int speedFactor = 1;
                int totalPixels = (rectStart.Width != rectEnd.Width) ?
                    Math.Abs(rectStart.Width - rectEnd.Width) :
                    Math.Abs(rectStart.Height - rectEnd.Height);
                int remainPixels = totalPixels;
                DateTime startingTime = DateTime.Now;
                Rectangle rectAnimation = rectStart;
                while (rectAnimation != rectEnd)
                {
                    DateTime startPerMove = DateTime.Now;

                    rectAnimation.X += dxLoc * speedFactor;
                    rectAnimation.Y += dyLoc * speedFactor;
                    rectAnimation.Width += dWidth * speedFactor;
                    rectAnimation.Height += dHeight * speedFactor;
                    if (Math.Sign(rectEnd.X - rectAnimation.X) != Math.Sign(dxLoc))
                        rectAnimation.X = rectEnd.X;
                    if (Math.Sign(rectEnd.Y - rectAnimation.Y) != Math.Sign(dyLoc))
                        rectAnimation.Y = rectEnd.Y;
                    if (Math.Sign(rectEnd.Width - rectAnimation.Width) != Math.Sign(dWidth))
                        rectAnimation.Width = rectEnd.Width;
                    if (Math.Sign(rectEnd.Height - rectAnimation.Height) != Math.Sign(dHeight))
                        rectAnimation.Height = rectEnd.Height;
                    control.Bounds = rectAnimation;
                    if (control.Parent != null)
                        control.Parent.Update();
                    else
                        control.Update();

                    remainPixels -= speedFactor;

                    while (true)
                    {
                        TimeSpan elapsedPerMove = DateTime.Now - startPerMove;
                        TimeSpan elapsedTime = DateTime.Now - startingTime;
                        if ((time - elapsedTime).TotalMilliseconds <= 0)
                        {
                            speedFactor = remainPixels;
                            break;
                        }
                        else
                        {
                            if ((int)(time - elapsedTime).TotalMilliseconds == 0)
                                speedFactor = 1;
                            else
                            {
                                try
                                {

                                    speedFactor = remainPixels * (int)elapsedPerMove.TotalMilliseconds / (int)((time - elapsedTime).TotalMilliseconds);
                                }
                                catch { }
                            }
                        }
                        if (speedFactor >= 1)
                            break;
                    }
                }
            }

            if (!show)
            {
                control.Visible = false;
                control.Bounds = rectStart;
            }
        }

        //		internal static Keys GetPressedKey()
        //		{
        //			NativeFunctions.GetKeyboardState 
        //
        //			return Keys.None;
        //		}

        internal static eWinXPColorScheme WinXPColorScheme
        {
            get
            {
                eWinXPColorScheme c = eWinXPColorScheme.Undetermined;
                if (BarFunctions.ThemedOS && NativeFunctions.ColorDepth >= 16)
                {
                    if (m_IsVista)
                        c = eWinXPColorScheme.Blue;
                    else if (SystemColors.Control.ToArgb() == Color.FromArgb(236, 233, 216).ToArgb() && SystemColors.Highlight.ToArgb() == Color.FromArgb(49, 106, 197).ToArgb())
                        c = eWinXPColorScheme.Blue;
                    else if (SystemColors.Control.ToArgb() == Color.FromArgb(224, 223, 227).ToArgb() && SystemColors.Highlight.ToArgb() == Color.FromArgb(178, 180, 191).ToArgb())
                        c = eWinXPColorScheme.Silver;
                    else if (SystemColors.Control.ToArgb() == Color.FromArgb(236, 233, 216).ToArgb() && SystemColors.Highlight.ToArgb() == Color.FromArgb(147, 160, 112).ToArgb())
                        c = eWinXPColorScheme.OliveGreen;
                }

                return c;
            }
        }

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

        public static Graphics CreateGraphics(Control objCtrl)
        {
            if (objCtrl is ItemControl)
                return ((ItemControl)objCtrl).CreateGraphics();
            else if (objCtrl is Bar)
                return ((Bar)objCtrl).CreateGraphics();
            else if (objCtrl is ExplorerBar)
                return ((ExplorerBar)objCtrl).CreateGraphics();
            else if (objCtrl is BaseItemControl)
                return ((BaseItemControl)objCtrl).CreateGraphics();
            else if (objCtrl is BarBaseControl)
                return ((BarBaseControl)objCtrl).CreateGraphics();
            else if (objCtrl is PanelControl)
                return ((PanelControl)objCtrl).CreateGraphics();
            else if (objCtrl is PopupItemControl)
                return ((PopupItemControl)objCtrl).CreateGraphics();

            return objCtrl.CreateGraphics();
        }

        public static bool IsOffice2007Style(eDotNetBarStyle style)
        {
            if (style == eDotNetBarStyle.StyleManagerControlled)
                style = StyleManager.GetEffectiveStyle();
            return (style == eDotNetBarStyle.Office2007 || style == eDotNetBarStyle.Office2010 || style == eDotNetBarStyle.Windows7);
        }

        public static bool IsOffice2010Style(eDotNetBarStyle style)
        {
            if (style == eDotNetBarStyle.StyleManagerControlled)
                style = StyleManager.GetEffectiveStyle();
            return (style == eDotNetBarStyle.Office2010 || style == eDotNetBarStyle.Windows7);
        }

        public static bool IsOffice2007StyleOnly(eDotNetBarStyle style)
        {
            if (style == eDotNetBarStyle.StyleManagerControlled)
                style = StyleManager.GetEffectiveStyle();
            return (style == eDotNetBarStyle.Office2007);
        }
    }

    internal class ScreenInformation
    {
        public Rectangle Bounds = Rectangle.Empty;
        public Rectangle WorkingArea = Rectangle.Empty;
        public ScreenInformation(Rectangle bounds, Rectangle workingarea)
        {
            this.Bounds = bounds;
            this.WorkingArea = workingarea;
        }
    }

    internal enum eWinXPColorScheme
    {
        Undetermined,
        Blue,
        OliveGreen,
        Silver
    }

    internal class LocalizationManager : IDisposable
    {
        private ResourceManager m_ResourceManager = null;
        private IOwnerLocalize m_Manager = null;

        public LocalizationManager(IOwnerLocalize manager)
        {
            m_Manager = manager;
        }
        public void Dispose()
        {
            m_ResourceManager = null;
            m_Manager = null;
        }

        public string GetDefaultLocalizedString(string key)
        {
            string s = GetLocalizedString(key);
            if (s == "" || s == null)
            {
                ResourceManager res = BarFunctions.GetResourceManager(true);
                s = res.GetString(key);
            }
            if (s == null)
                s = "";
            return s;
        }

        public static string GetLocalizedString(string key, string defaultValue)
        {
            LocalizeEventArgs e = new LocalizeEventArgs();
            e.Key = key;
            e.LocalizedValue = defaultValue;
            LocalizationKeys.InvokeLocalizeString(e);
            if (e.Handled)
                return e.LocalizedValue;

            return defaultValue;
        }

        public string GetLocalizedString(string key)
        {
            string s = "";
            if (m_ResourceManager == null)
                m_ResourceManager = BarFunctions.GetResourceManager();
            if (m_ResourceManager != null)
            {
                s = m_ResourceManager.GetString(key);
                if (s == null)
                    s = "";
            }

            // Fire static event first
            LocalizeEventArgs e = new LocalizeEventArgs();
            e.Key = key;
            e.LocalizedValue = s;
            LocalizationKeys.InvokeLocalizeString(e);
            if (e.Handled)
                return e.LocalizedValue;


            if (m_Manager != null)
            {
                m_Manager.InvokeLocalizeString(e);
                if (e.Handled)
                    return e.LocalizedValue;
            }

            return s;
        }
    }
}
