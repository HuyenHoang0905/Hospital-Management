using System;
using System.Text;
using System.ComponentModel;
using System.Collections;
using System.Windows.Forms;
using System.Drawing;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Represents the context menu bar that provides the context menus for the System.Windows.Forms.Control inherited controls on the form.
    /// </summary>
    [ToolboxItem(true), Designer("DevComponents.DotNetBar.Design.ContextMenuBarDesigner, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf"), ProvideProperty("ContextMenuEx", typeof(Control)), System.Runtime.InteropServices.ComVisible(false), DefaultEvent("ItemClick")]
    public class ContextMenuBar : Bar, System.ComponentModel.IExtenderProvider
    {
        #region Private Variables
        private Hashtable m_ContextExMenus = new Hashtable();
        private Hashtable m_ContextExHandlers = new Hashtable();
        private bool m_ContextMenuSubclass = true;
        #endregion

        #region Internal Implementation
        public ContextMenuBar() : base()
        {
            this.Visible = false;
            this.WrapItemsDock = true;
            this.WrapItemsFloat = true;
        }

        protected override bool IsContextPopup(BaseItem popup)
        {
            if (this.Items.Contains(popup)) return true;
            return base.IsContextPopup(popup);
        }
        #endregion

        #region Property Hiding
        /// <summary>
        /// Gets/Sets whether Bar is visible or not.
        /// </summary>
        [DevCoBrowsable(false), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new bool Visible
        {
            get { return base.Visible; }
            set { base.Visible = value; }
        }
        #endregion

        #region Extender Implementation
        // *********************************************************************
        //
        // Extended Property ContextMenuEx implementation code
        //
        // *********************************************************************
        bool IExtenderProvider.CanExtend(object target)
        {
            if (target is Control)
                return true;
            return false;
        }

        private delegate void WmContextEventHandler(object sender, WmContextEventArgs e);
        /// <summary>
        /// Returns the instance of the BaseItem that is assigned as context menu to the control.
        /// </summary>
        /// <param name="control">Control to return context menu for.</param>
        /// <returns>Instance of the BaseItem used as context menu for the control.</returns>
        [DefaultValue(null), Editor(typeof(ContextExMenuTypeEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public BaseItem GetContextMenuEx(Control control)
        {
            BaseItem item = (BaseItem)m_ContextExMenus[control];
            return item;
        }
        /// <summary>
        /// Assigns the context menu to a control.
        /// </summary>
        /// <param name="control">Control to assign the context menu to.</param>
        /// <param name="value">Instance of PopupItem derived class usually ButtonItem to act as context menu for a control. The SubItems collection of the item specified here actually defines the visible context menu items.</param>
        public void SetContextMenuEx(Control control, BaseItem value)
        {
            if (value == null)
            {
                if (m_ContextExMenus.Contains(control))
                {
                    if (m_ContextExHandlers.Contains(control))
                    {
                        ContextMessageHandler h = m_ContextExHandlers[control] as ContextMessageHandler;
                        if (h != null)
                        {
                            h.ContextMenu -= new WmContextEventHandler(this.OnContextMenu);
                            h.ReleaseHandle();
                            h = null;
                        }
                        m_ContextExHandlers.Remove(control);
                    }

                    m_ContextExMenus.Remove(control);
                    control.MouseUp -= new MouseEventHandler(this.ContextExMouseUp);
                    try
                    {
                        control.HandleDestroyed -= new EventHandler(this.ContextExHandleDestroy);
                    }
                    catch { }
                    try
                    {
                        control.HandleCreated -= new EventHandler(this.ContextExHandleCreate);
                    }
                    catch { }
                    try { control.MouseUp -= new MouseEventHandler(this.ContextExMouseUp); }
                    catch { }
                }
            }
            else
            {
                if (m_ContextExMenus.Contains(control))
                {
                    m_ContextExMenus[control] = value;
                }
                else
                {
                    m_ContextExMenus[control] = value;
                    if (!m_ContextExHandlers.Contains(control) && !this.DesignMode)
                    {
                        if (!(control is System.Windows.Forms.TreeView) && !(control is System.Windows.Forms.Form) && 
                            !(control is System.Windows.Forms.Panel
#if FRAMEWORK20
                            || control is System.Windows.Forms.DataGridView
#endif
                            ) && 
                            m_ContextMenuSubclass)
                        {
                            if (control.IsHandleCreated)
                            {
                                ContextMessageHandler h = new ContextMessageHandler();
                                h.ContextMenu += new WmContextEventHandler(this.OnContextMenu);
                                h.ParentControl = control;
                                h.AssignHandle(control.Handle);
                                m_ContextExHandlers[control] = h;
                            }
                            control.HandleDestroyed += new EventHandler(this.ContextExHandleDestroy);
                            control.HandleCreated += new EventHandler(this.ContextExHandleCreate);
                        }
                        if (control is ComboBox)
                        {
                            ComboBox cbo = control as ComboBox;
                            cbo.ContextMenu = new ContextMenu();
                        }
                    }
                    try { control.MouseUp += new MouseEventHandler(this.ContextExMouseUp); }
                    catch { }
                }
            }
        }

        internal bool HasContextMenu(Control ctrl)
        {
            return m_ContextExMenus.Contains(ctrl);
        }

        private void ContextExHandleDestroy(object sender, EventArgs e)
        {
            Control control = sender as Control;
            if (control == null)
                return;
            ContextMessageHandler h = m_ContextExHandlers[control] as ContextMessageHandler;
            if (h != null)
            {
                h.ContextMenu -= new WmContextEventHandler(this.OnContextMenu);
                h.ReleaseHandle();
                h = null;
            }
            m_ContextExHandlers.Remove(control);
        }

        private void ContextExHandleCreate(object sender, EventArgs e)
        {
            Control control = sender as Control;
            if (control == null)
                return;
            if (m_ContextExHandlers.Contains(control))
                return;

            ContextMessageHandler h = new ContextMessageHandler();
            h.ContextMenu += new WmContextEventHandler(this.OnContextMenu);
            h.ParentControl = control;
            h.AssignHandle(control.Handle);
            m_ContextExHandlers[control] = h;
        }

        private void ContextExMouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;
            Control ctrl = sender as Control;
            if (ctrl == null)
                return;
            // Find it in pop-ups
            PopupItem popup = GetContextExItem(ctrl) as PopupItem;
            if (popup == null) return;

            if (!popup.Expanded)
            {
                popup.Style = this.Style;
                popup.SetSourceControl(ctrl);
                popup.Popup(ctrl.PointToScreen(new Point(e.X, e.Y)));
            }
        }

        private Control FindControl(Control parent, IntPtr handle)
        {
            foreach (Control ctrl in parent.Controls)
            {
                if (ctrl.Handle == handle)
                    return ctrl;
                if (ctrl.Controls.Count > 0)
                {
                    Control ret = FindControl(ctrl, handle);
                    if (ret != null)
                        return ret;
                }
            }
            return parent;
        }

        private BaseItem GetContextExItem(Control ctrl)
        {
            BaseItem item = (BaseItem)m_ContextExMenus[ctrl];
            return item;
        }
        private void OnContextMenu(object sender, WmContextEventArgs e)
        {
            ContextMessageHandler h = sender as ContextMessageHandler;
            if (h == null)
                return;

            BaseItem contextItem = GetContextExItem(h.ParentControl);
            if (contextItem == null)
                return;

            // Find it in pop-ups
            PopupItem popup = contextItem as PopupItem;
            popup.Style = this.Style;

            if (e.Button == MouseButtons.None)
            {
                // Get the control with focus
                Control ctrl = Control.FromChildHandle(e.Handle);
                if (ctrl != null && ctrl.Handle != e.Handle)
                {
                    ctrl = FindControl(ctrl, e.Handle);
                }
                popup.SetSourceControl(h.ParentControl);
                if (ctrl != null)
                    popup.Popup(ctrl.PointToScreen(Point.Empty));
                else
                    popup.Popup(Control.MousePosition);

                // We need to eat the message in OnSysKeyUp for Shift+F10 case
                if (this.IgnoreSysKeyUp)
                {
                    this.IgnoreSysKeyUp = false;
                    this.EatSysKeyUp = true;
                }
            }
            else
            {
                // This is handled by the WM_RBUTTONUP just eat it
                if (!e.WmContext)
                {
                    popup.SetSourceControl(h.ParentControl);
                    popup.Popup(e.X, e.Y);
                }
            }
            e.Handled = true;
        }
        private class ContextMessageHandler : NativeWindow
        {
            public event WmContextEventHandler ContextMenu;
            private const int WM_CONTEXTMENU = 0x007B;
            private const int WM_RBUTTONUP = 0x0205;
            private const int WM_NCRBUTTONUP = 0x00A5;
            private const int WM_RBUTTONDOWN = 0x0204;
            public Control ParentControl = null;
            protected override void WndProc(ref Message m)
            {
                if (m.Msg == WM_CONTEXTMENU)
                {
                    if (ContextMenu != null)
                    {
                        int ilParam = m.LParam.ToInt32();
                        int y = ilParam >> 16;
                        int x = ilParam & 0xFFFF;
                        IntPtr hWnd = m.WParam;
                        if (hWnd == IntPtr.Zero)
                            hWnd = m.HWnd;
                        bool context = true;
                        if (m.HWnd != m.WParam)
                            context = false;
                        WmContextEventArgs e = new WmContextEventArgs(hWnd, x, y, ((x == 65535 && y == -1) ? MouseButtons.None : MouseButtons.Right), context);
                        ContextMenu(this, e);
                        if (e.Handled)
                            return;
                    }
                }
                // This case was taken out becouse the message was not generated for the listview control and possibly for
                // treview control so this code was moved to the MouseUp event of the Control see ContextExMouseUp
                //				else if(m.Msg==WM_RBUTTONUP || m.Msg==WM_NCRBUTTONUP)
                //				{
                //					if(ContextMenu!=null)
                //					{
                //						int ilParam=m.LParam.ToInt32();
                //						int y=ilParam>>16;
                //						int x=ilParam & 0xFFFF;
                //						Point p=ParentControl.PointToScreen(new Point(x,y));
                //						WmContextEventArgs e=new WmContextEventArgs(m.HWnd,p.X,p.Y,MouseButtons.Right,false);
                //						ContextMenu(this,e);
                //					}
                //				}
                base.WndProc(ref m);
            }
        }
        private class WmContextEventArgs : EventArgs
        {
            private readonly int x;
            private readonly int y;
            private readonly MouseButtons button = 0;
            private readonly IntPtr hwnd;
            private readonly bool wmcontext;
            public bool Handled = false;
            public WmContextEventArgs(IntPtr phwnd, int ix, int iy, MouseButtons eButton, bool WmContextMessage)
            {
                this.x = ix;
                this.y = iy;
                this.button = eButton;
                this.hwnd = phwnd;
                this.wmcontext = WmContextMessage;
            }
            public int X
            {
                get { return this.x; }
            }
            public int Y
            {
                get { return this.y; }
            }
            public MouseButtons Button
            {
                get { return this.button; }
            }
            public IntPtr Handle
            {
                get { return this.hwnd; }
            }
            public bool WmContext
            {
                get { return this.wmcontext; }
            }
        }
        #endregion
    }
}
