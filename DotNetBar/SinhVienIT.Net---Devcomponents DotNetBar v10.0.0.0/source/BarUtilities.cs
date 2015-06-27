using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Drawing.Text;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Represents class with static functions that provide commonly used utility functions when working with
	/// Bar objects and items hosted by Bar object.
	/// </summary>
	public class BarUtilities
    {
        #region Docking
        /// <summary>
		/// Sets the visible property of DockContainerItem and hides the bar if the given item is the last visible item on the bar.
		/// It will also automatically display the bar if bar is not visible.
		/// </summary>
		/// <param name="item">DockContainerItem to set visibility for.</param>
		/// <param name="visible">Indicates the visibility of the item</param>
		public static void SetDockContainerVisible(DevComponents.DotNetBar.DockContainerItem item, bool visible)
		{
			if(item==null || item.Visible==visible)
				return;

			DevComponents.DotNetBar.Bar containerBar=item.ContainerControl as DevComponents.DotNetBar.Bar;

			if(containerBar==null)
			{
				// If bar has not been assigned yet just set the visible property and exit
				item.Visible=visible;
				return;
			}

			DotNetBarManager manager=containerBar.Owner as DotNetBarManager;
			if(manager!=null)
				manager.SuspendLayout=true;

			try
			{
				int visibleCount=containerBar.VisibleItemCount;

				if(visible)
				{
					item.Visible=true;
					if(!containerBar.AutoHide && !containerBar.Visible && visibleCount<=1)
					{
						containerBar.Visible=true;
						if(containerBar.PropertyBag.ContainsKey(BarPropertyBagKeys.AutoHideSetting))
						{
							containerBar.PropertyBag.Remove(BarPropertyBagKeys.AutoHideSetting);
							containerBar.AutoHide=true;
						}
					}
				}
				else
				{
                    if (visibleCount <= 1)
                    {
                        if (containerBar.PropertyBag.ContainsKey(BarPropertyBagKeys.AutoHideSetting))
                            containerBar.PropertyBag.Remove(BarPropertyBagKeys.AutoHideSetting);
                        // Remember auto-hide setting
                        if (containerBar.AutoHide)
                            containerBar.PropertyBag.Add(BarPropertyBagKeys.AutoHideSetting, true);
                        containerBar.CloseBar();
                    }
					item.Visible=false;
				}
			}
			finally
			{
				if(manager!=null)
					manager.SuspendLayout=false;
				containerBar.RecalcLayout();
			}
		}

		/// <summary>
		/// Creates new instance of the bar and sets its properties so bar can be used as Document bar.
		/// </summary>
		/// <returns>Returns new instance of the bar.</returns>
		public static Bar CreateDocumentBar()
		{
			Bar bar=new Bar();
			BarUtilities.InitializeDocumentBar(bar);
			return bar;
		}

		/// <summary>
		/// Sets the properties on a bar so it can be used as Document bar.
		/// </summary>
		/// <param name="bar">Bar to set properties of.</param>
		public static void InitializeDocumentBar(Bar bar)
		{
			TypeDescriptor.GetProperties(bar)["LayoutType"].SetValue(bar,eLayoutType.DockContainer);
			TypeDescriptor.GetProperties(bar)["DockTabAlignment"].SetValue(bar,eTabStripAlignment.Top);
			TypeDescriptor.GetProperties(bar)["AlwaysDisplayDockTab"].SetValue(bar,true);
			TypeDescriptor.GetProperties(bar)["Stretch"].SetValue(bar,true);
			TypeDescriptor.GetProperties(bar)["GrabHandleStyle"].SetValue(bar,eGrabHandleStyle.None);
			TypeDescriptor.GetProperties(bar)["CanDockBottom"].SetValue(bar,false);
			TypeDescriptor.GetProperties(bar)["CanDockTop"].SetValue(bar,false);
			TypeDescriptor.GetProperties(bar)["CanDockLeft"].SetValue(bar,false);
			TypeDescriptor.GetProperties(bar)["CanDockRight"].SetValue(bar,false);
			TypeDescriptor.GetProperties(bar)["CanDockDocument"].SetValue(bar,true);
			TypeDescriptor.GetProperties(bar)["CanUndock"].SetValue(bar,false);
			TypeDescriptor.GetProperties(bar)["CanHide"].SetValue(bar,true);
			TypeDescriptor.GetProperties(bar)["CanCustomize"].SetValue(bar,false);
			TypeDescriptor.GetProperties(bar)["TabNavigation"].SetValue(bar,true);
        }

        #region Win API
        [DllImport("user32")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        [DllImport("user32")]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        private const int GWL_EXSTYLE = (-20);
        private const int WS_EX_CLIENTEDGE = 0x00000200;
        [DllImport("user32")]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, int uFlags);
        const int SWP_FRAMECHANGED = 0x0020;
        const int SWP_NOSIZE = 0x0001;
        const int SWP_NOMOVE = 0x0002;
        const int SWP_NOZORDER = 0x0004;
        #endregion

        /// <summary>
        /// Changes the MDI Client border edge to remove 3D border or to add it.
        /// </summary>
        /// <param name="c">Reference to MDI Client object.</param>
        /// <param name="removeBorder">Indicates whether to remove border.</param>
        public static void ChangeMDIClientBorder(System.Windows.Forms.MdiClient c, bool removeBorder)
        {
            if (c != null)
            {
                int exStyle = GetWindowLong(c.Handle, GWL_EXSTYLE);
                
                if(removeBorder)
                    exStyle ^= WS_EX_CLIENTEDGE;
                else
                    exStyle |= WS_EX_CLIENTEDGE;

                SetWindowLong(c.Handle, GWL_EXSTYLE, exStyle);
                SetWindowPos(c.Handle, IntPtr.Zero, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_NOZORDER | SWP_FRAMECHANGED);
            }
        }

        /// <summary>
        /// Changes the MDI Client border edge to remove 3D border or to add it.
        /// </summary>
        /// <param name="c">Reference to MDI parent form.</param>
        /// <param name="removeBorder">Indicates whether to remove border.</param>
        public static void ChangeMDIClientBorder(System.Windows.Forms.Form c, bool removeBorder)
        {
            if (c.IsMdiContainer && c.IsHandleCreated)
            {
                foreach (System.Windows.Forms.Control control in c.Controls)
                {
                    if (control is System.Windows.Forms.MdiClient)
                    {
                        ChangeMDIClientBorder(control as System.Windows.Forms.MdiClient, removeBorder);
                        break;
                    }
                }
            }
        }
        #endregion

        #region Item Invalidate
        internal static void InvalidateFontChange(SubItemsCollection col)
        {
            foreach (BaseItem item in col)
            {
                InvalidateFontChange(item);
            }
        }

        internal static void InvalidateFontChange(BaseItem item)
        {
            if (item.TextMarkupBody != null) item.TextMarkupBody.InvalidateElementsSize();
            if (item.SubItems.Count > 0) InvalidateFontChange(item.SubItems);
        }
        #endregion

        internal static void InvokeRecalcLayout(System.Windows.Forms.Control control)
        {
            if (control is Bar)
                ((Bar)control).RecalcLayout();
            else if (control is ItemControl)
                ((ItemControl)control).RecalcLayout();
            else if (control is BaseItemControl)
                ((BaseItemControl)control).RecalcLayout();
            else if (control is ExplorerBar)
                ((ExplorerBar)control).RecalcLayout();
            else if (control is SideBar)
                ((SideBar)control).RecalcLayout();
        }

        /// <summary>
        /// Gets or sets whether StringFormat internally used by all DotNetBar controls to render text is GenericDefault. Default value is false
        /// which indicates that GenericTypographic is used.
        /// </summary>
        public static bool UseGenericDefaultStringFormat
        {
            get { return TextDrawing.UseGenericDefault; }
            set { TextDrawing.UseGenericDefault = value; }
        }

        /// <summary>
        /// Gets or sets the anti-alias text rendering hint that will be used to render text on controls that have AntiAlias property set to true.
        /// </summary>
        public static TextRenderingHint AntiAliasTextRenderingHint
        {
            get
            {
                return DisplayHelp.AntiAliasTextRenderingHint;
            }
            set
            {
            	DisplayHelp.AntiAliasTextRenderingHint = value;
            }
        }

#if FRAMEWORK20
        /// <summary>
        /// Gets or sets whether .NET Framework TextRenderer class is used for text rendering instead of Graphics.DrawString. 
        /// Default value is false. 
        /// Using TextRenderer will disable the Fade and Animation effects on controls because of issues in TextRenderer when drawing text on transparent
        /// surfaces.
        /// </summary>
        public static bool UseTextRenderer
        {
            get { return TextDrawing.UseTextRenderer; }
            set { TextDrawing.UseTextRenderer = value; }
        }
#endif


        private static bool _AlwaysGenerateAccessibilityFocusEvent = false;
        /// <summary>
        /// Gets or sets whether items always generate the Focus accessibility event when mouse enters the item. Default value is false which indicates
        /// that focus event will be raised only when item is on menu bar.
        /// </summary>
        public static bool AlwaysGenerateAccessibilityFocusEvent
        {
            get { return _AlwaysGenerateAccessibilityFocusEvent; }
            set
            {
                _AlwaysGenerateAccessibilityFocusEvent = value;
            }
        }

        internal static bool IsModalFormOpen
        {
            get
            {
#if (FRAMEWORK20)
                for (int i = 0; i < System.Windows.Forms.Application.OpenForms.Count; i++)
                {
                    System.Windows.Forms.Form form = System.Windows.Forms.Application.OpenForms[i];
                    if (form.Modal) return true;
                }
#endif
                return false;
            }
        }

        private static bool _AutoRemoveMessageFilter = false;
        /// <summary>
        /// Gets or sets whether Application Message Filter that is registered by popup controls
        /// is automatically unregistered when last control is disposed. Default value is false and
        /// in most cases should not be changed.
        /// </summary>
        public static bool AutoRemoveMessageFilter
        {
            get { return _AutoRemoveMessageFilter; }
            set { _AutoRemoveMessageFilter = value; }
        }

        private static int _TextMarkupCultureSpecific = 3;
        /// <summary>
        /// Get or sets the text-markup padding for text measurement when running on Japanese version of Windows.
        /// </summary>
        public static int TextMarkupCultureSpecificPadding
        {
            get { return _TextMarkupCultureSpecific; }
            set
            {
                _TextMarkupCultureSpecific = value;
            }
        }

        private static bool _DisposeItemImages = false;
        /// <summary>
        /// Gets or sets whether Image and Icon resources assigned to items and controls are automatically disposed when
        /// control or item is disposed. Default value is false.
        /// </summary>
        public static bool DisposeItemImages
        {
            get
            {
                return _DisposeItemImages;
            }
            set
            {
                _DisposeItemImages = value;
            }
        }

        /// <summary>
        /// Disposes image reference and sets it to null.
        /// </summary>
        /// <param name="image">Reference to image to dispose.</param>
        internal static void DisposeImage(ref System.Drawing.Image image)
        {
            if (image == null) return;
            image.Dispose();
            image = null;
        }
        /// <summary>
        /// Disposes image reference and sets it to null.
        /// </summary>
        /// <param name="image">Reference to image to dispose.</param>
        internal static void DisposeImage(ref System.Drawing.Icon icon)
        {
            if (icon == null) return;
            icon.Dispose();
            icon = null;
        }
    }

	internal class BarPropertyBagKeys
	{
		public static string AutoHideSetting="autohide";
	}
}
