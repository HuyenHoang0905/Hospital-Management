using System;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using System.Collections;
using System.Reflection;
using System.Drawing;
using DevComponents.DotNetBar.Rendering;
using System.Threading;

namespace DevComponents.DotNetBar
{
    [ToolboxBitmap(typeof(StyleManager), "StyleManager.ico"), ToolboxItem(true)]
    public class StyleManager : Component
    {
        #region Constructor
        static StyleManager()
        {
            StyleManager._ReadWriteClientsListLock = new ReaderWriterLock();
        }
        /// <summary>
        /// Initializes a new instance of the StyleManager class.
        /// </summary>
        public StyleManager()
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the StyleManager class with the specified container.
        /// </summary>
        /// <param name="container">An IContainer that represents the container for the command.</param>
        public StyleManager(IContainer container)
            : this()
        {
            container.Add(this);
        }
        #endregion

        #region Implementation
        /// <summary>
        /// Gets or sets the global style for the controls that have Style=ManagerControlled.
        /// </summary>
        [DefaultValue(eDotNetBarStyle.Office2007), Category("Appearance"), Description("Indicates global style for the controls that have Style=ManagerControlled.")]
        public eStyle ManagerStyle
        {
            get { return StyleManager.Style; }
            set
            {
                StyleManager.Style = value;
            }
        }
        /// <summary>
        /// Gets or sets the color current style is tinted with.
        /// </summary>
        [Category("Appearance"), Description("Indicates color current style is tinted with.")]
        public Color ManagerColorTint
        {
            get { return StyleManager.ColorTint; }
            set { StyleManager.ColorTint = value; }
        }
        /// <summary>
        /// Gets whether property should be serialized by WinForms designer.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeManagerColorTint()
        {
            return !ManagerColorTint.IsEmpty;
        }
        /// <summary>
        /// Resets property to its default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetManagerColorTint()
        {
            ManagerColorTint = Color.Empty;
        }

        private static eStyle _Style = eStyle.Office2007Blue;
        public static eStyle Style
        {
            get
            {
                return _Style;
            }
            set
            {
                eStyle oldValue = _Style;
            	_Style = value;
                _EffectiveStyle = DotNetBarStyleFromStyle(value);
                OnStyleChanged(oldValue, value);
            }
        }

        private static eDotNetBarStyle DotNetBarStyleFromStyle(eStyle value)
        {
            if (value == eStyle.Windows7Blue)
                return eDotNetBarStyle.Windows7;
            else if (value == eStyle.Office2010Silver || value == eStyle.Office2010Blue || value == eStyle.Office2010Black || value == eStyle.VisualStudio2010Blue)
                return eDotNetBarStyle.Office2010;

            return eDotNetBarStyle.Office2007;
        }
        private static eDotNetBarStyle _EffectiveStyle = eDotNetBarStyle.Office2007;
        public static eDotNetBarStyle GetEffectiveStyle()
        {
            return _EffectiveStyle;
        }

        private static Color _ColorTint = Color.Empty;
        /// <summary>
        /// Gets or sets the color tint that is applied to current Office 2007, Office 2010 or Windows 7 color table.
        /// Default value is Color.Empty which indicates that no color blending is performed.
        /// </summary>
        public static Color ColorTint
        {
            get
            {
                return _ColorTint;
            }
            set
            {
                Color oldValue = _ColorTint;
            	_ColorTint = value;
                OnColorTintChanged(oldValue, value);
            }
        }

        /// <summary>
        /// Changes the StyleManager style and color tint in one step. Use this method if you need to change style and color tint simultaneously in single step for better performance.
        /// </summary>
        /// <param name="newStyle">New style.</param>
        /// <param name="colorTint">Color tint for the style.</param>
        public static void ChangeStyle(eStyle newStyle, Color colorTint)
        {
            _ColorTint = colorTint;
            Style = newStyle;
        }

        private static void OnColorTintChanged(Color oldValue, Color newValue)
        {
            ChangeColorTable(_Style, true);
            NotifyOnStyleChange();
#if FRAMEWORK20
            foreach (Form form in Application.OpenForms)
            {
                if (form.InvokeRequired)
                {
                    if (form is Office2007Form)
                        form.BeginInvoke(new
                                MethodInvoker(delegate() { ((Office2007Form)form).InvalidateNonClient(true); }));

                    form.BeginInvoke(new
                                MethodInvoker(delegate() { form.Invalidate(true); }));
                }
                else
                {
                    if (form is Office2007Form)
                        ((Office2007Form)form).InvalidateNonClient(true);
                    form.Invalidate(true);
                }
            }
#else
            WeakReference[] references = new WeakReference[_RegisteredControls.Count];
            _RegisteredControls.CopyTo(references);
            foreach (WeakReference reference in references)
            {
                Control target = reference.Target as Control;
                if (reference.IsAlive)
                {
                    if (target != null)
                    {
                        target.Invalidate(true);
                    }
                }
            }
#endif
        }

        private static void ChangeColorTable(eStyle style)
        {
            ChangeColorTable(style, false);
        }

        private static void ChangeColorTable(eStyle style, bool colorBlendChanged)
        {
            if (style == eStyle.Office2010Silver)
            {
                // Ensure that proper color table is selected.
                if (GlobalManager.Renderer is Office2007Renderer)
                {
                    Office2007Renderer renderer = (Office2007Renderer)GlobalManager.Renderer;
                    if (colorBlendChanged || !(renderer.ColorTable is Office2010ColorTable) ||
                        renderer.ColorTable is Office2010ColorTable && ((Office2010ColorTable)renderer.ColorTable).ColorScheme != eOffice2010ColorScheme.Silver)
                    {
                        ((Office2007Renderer)GlobalManager.Renderer).ColorTable = new Office2010ColorTable(eOffice2010ColorScheme.Silver, _ColorTint);
                    }
                }
            }
            else if (style == eStyle.Office2010Blue)
            {
                // Ensure that proper color table is selected.
                if (GlobalManager.Renderer is Office2007Renderer)
                {
                    Office2007Renderer renderer = (Office2007Renderer)GlobalManager.Renderer;
                    if (colorBlendChanged || !(renderer.ColorTable is Office2010ColorTable) || 
                        renderer.ColorTable is Office2010ColorTable && ((Office2010ColorTable)renderer.ColorTable).ColorScheme != eOffice2010ColorScheme.Blue)
                    {
                        ((Office2007Renderer)GlobalManager.Renderer).ColorTable = new Office2010ColorTable(eOffice2010ColorScheme.Blue, _ColorTint);
                    }
                }
            }
            else if (style == eStyle.Office2010Black)
            {
                // Ensure that proper color table is selected.
                if (GlobalManager.Renderer is Office2007Renderer)
                {
                    Office2007Renderer renderer = (Office2007Renderer)GlobalManager.Renderer;
                    if (colorBlendChanged || !(renderer.ColorTable is Office2010ColorTable) ||
                        renderer.ColorTable is Office2010ColorTable && ((Office2010ColorTable)renderer.ColorTable).ColorScheme != eOffice2010ColorScheme.Black)
                    {
                        ((Office2007Renderer)GlobalManager.Renderer).ColorTable = new Office2010ColorTable(eOffice2010ColorScheme.Black, _ColorTint);
                    }
                }
            }
            else if (style == eStyle.VisualStudio2010Blue)
            {
                // Ensure that proper color table is selected.
                if (GlobalManager.Renderer is Office2007Renderer)
                {
                    Office2007Renderer renderer = (Office2007Renderer)GlobalManager.Renderer;
                    if (colorBlendChanged || !(renderer.ColorTable is Office2010ColorTable) ||
                        renderer.ColorTable is Office2010ColorTable && ((Office2010ColorTable)renderer.ColorTable).ColorScheme != eOffice2010ColorScheme.VS2010)
                    {
                        ((Office2007Renderer)GlobalManager.Renderer).ColorTable = new Office2010ColorTable(eOffice2010ColorScheme.VS2010, _ColorTint);
                    }
                }
            }
            else if (style == eStyle.Windows7Blue)
            {
                // Ensure that proper color table is selected.
                if (GlobalManager.Renderer is Office2007Renderer)
                {
                    if (colorBlendChanged || !(((Office2007Renderer)GlobalManager.Renderer).ColorTable is Windows7ColorTable))
                    {
                        ((Office2007Renderer)GlobalManager.Renderer).ColorTable = new Windows7ColorTable(eWindows7ColorScheme.Blue, _ColorTint);
                    }

                }
            }
            else if (style == eStyle.Office2007Blue || style == eStyle.Office2007Silver || style == eStyle.Office2007Black || style == eStyle.Office2007VistaGlass)
            {
                // Ensure that proper color table is selected.
                if (GlobalManager.Renderer is Office2007Renderer)
                {
                    if (colorBlendChanged || (((Office2007Renderer)GlobalManager.Renderer).ColorTable is Office2010ColorTable) ||
                        (((Office2007Renderer)GlobalManager.Renderer).ColorTable is Windows7ColorTable) ||
                        (((Office2007Renderer)GlobalManager.Renderer).ColorTable.InitialColorScheme != ColorSchemeFromStyle(style)))
                    {
                        ((Office2007Renderer)GlobalManager.Renderer).ColorTable = new Office2007ColorTable(ColorSchemeFromStyle(style), _ColorTint);
                    }

                }
            }
        }

        private static eOffice2007ColorScheme ColorSchemeFromStyle(eStyle style)
        {
            eOffice2007ColorScheme colorScheme = eOffice2007ColorScheme.Blue;
            if (style == eStyle.Office2007Silver)
                colorScheme = eOffice2007ColorScheme.Silver;
            else if (style == eStyle.Office2007Black)
                colorScheme = eOffice2007ColorScheme.Black;
            else if (style == eStyle.Office2007VistaGlass)
                colorScheme = eOffice2007ColorScheme.VistaGlass;
            return colorScheme;
        }

        private static void NotifyOnStyleChange()
        {
            WeakReference[] references;
            StyleManager._ReadWriteClientsListLock.AcquireReaderLock(-1);
            try
            {
                references = new WeakReference[_RegisteredControls.Count];
                _RegisteredControls.CopyTo(references);
            }
            finally
            {
                StyleManager._ReadWriteClientsListLock.ReleaseReaderLock();
            }

            foreach (WeakReference reference in references)
            {
                object target = reference.Target;
                if (reference.IsAlive)
                {
                    if (target != null)
                    {
                        Control control = target as Control;
                        #if FRAMEWORK20
                        if (control != null && control.InvokeRequired)
                        {
                            control.BeginInvoke(new
                                MethodInvoker(delegate() { InvokeStyleManagerStyleChanged(target, _EffectiveStyle); }));
                            if(BarFunctions.IsHandleValid(control))
                                control.BeginInvoke(new
                                    MethodInvoker(delegate() { control.Invalidate(true); }));
                        }
                        else
                        #endif
                        {
                            InvokeStyleManagerStyleChanged(target, _EffectiveStyle);
                            if (control != null && BarFunctions.IsHandleValid(control))
                                control.Invalidate(true);
                        }
                    }
                }
            }
        }
        private static void OnStyleChanged(eStyle oldValue, eStyle newValue)
        {
            ChangeColorTable(newValue, !_ColorTint.IsEmpty);
            NotifyOnStyleChange();
        }

        private static void InvokeStyleManagerStyleChanged(object target, eDotNetBarStyle newStyle)
        {
            if (target == null) return;
            MethodInfo mi = target.GetType().GetMethod("StyleManagerStyleChanged");
            if (mi != null)
            {
                mi.Invoke(target, new object[] { newStyle });
            }
        }

        private static ReaderWriterLock _ReadWriteClientsListLock;
        private static ArrayList _RegisteredControls = new ArrayList();
        /// <summary>
        /// Registers control with the StyleManager so control can be notified of global style changes.
        /// </summary>
        /// <param name="control">Control to register with the StyleManager.</param>
        public static void Register(Control control)
        {
            if (GetControlReference(control) == null)
            {
                WeakReference reference = new WeakReference(control);
                reference.Target = control;

                LockCookie cookie1 = new LockCookie();
                bool readerLockHeld = StyleManager._ReadWriteClientsListLock.IsReaderLockHeld;
                if (readerLockHeld)
                {
                    cookie1 = StyleManager._ReadWriteClientsListLock.UpgradeToWriterLock(-1);
                }
                else
                {
                    StyleManager._ReadWriteClientsListLock.AcquireWriterLock(-1);
                }
                try
                {
                    _RegisteredControls.Add(reference);
                }
                finally
                {
                    if (readerLockHeld)
                    {
                        StyleManager._ReadWriteClientsListLock.DowngradeFromWriterLock(ref cookie1);
                    }
                    else
                    {
                        StyleManager._ReadWriteClientsListLock.ReleaseWriterLock();
                    }
                }

                control = null;
            }
        }

        private static WeakReference GetControlReference(Control control)
        {
            ArrayList registeredControls = _RegisteredControls;
            if (registeredControls == null) return null;

            StyleManager._ReadWriteClientsListLock.AcquireReaderLock(-1);
            try
            {
                foreach (WeakReference item in registeredControls)
                {
                    object target = item.Target;
                    if (target != null && target.Equals(control))
                        return item;
                }
            }
            finally
            {
                StyleManager._ReadWriteClientsListLock.ReleaseReaderLock();
            }
            return null;
        }
        /// <summary>
        /// Unregister the control from StyleManager notifications.
        /// </summary>
        /// <param name="control">Control that was registered through Register method.</param>
        public static void Unregister(Control control)
        {
            LockCookie cookie1 = new LockCookie();
            bool readerLockHeld = StyleManager._ReadWriteClientsListLock.IsReaderLockHeld;
            if (readerLockHeld)
            {
                cookie1 = StyleManager._ReadWriteClientsListLock.UpgradeToWriterLock(-1);
            }
            else
            {
                StyleManager._ReadWriteClientsListLock.AcquireWriterLock(-1);
            }
            try
            {

                foreach (WeakReference item in _RegisteredControls)
                {
                    object target = item.Target;
                    if (target != null && target.Equals(control))
                    {
                        _RegisteredControls.Remove(item);
                        break;
                    }
                }
            }
            finally
            {
                if (readerLockHeld)
                {
                    StyleManager._ReadWriteClientsListLock.DowngradeFromWriterLock(ref cookie1);
                }
                else
                {
                    StyleManager._ReadWriteClientsListLock.ReleaseWriterLock();
                }
            }
        }
        #endregion
    }
    /// <summary>
    /// Defines the StyleManager styles.
    /// </summary>
    public enum eStyle
    {
        Office2007Blue,
        Office2007Silver,
        Office2007Black,
        Office2007VistaGlass,
        Office2010Silver,
        Office2010Blue,
        Office2010Black,
        Windows7Blue,
        VisualStudio2010Blue
    }
}
