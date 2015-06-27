using System;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Represents the class that provides MessageBox like functionality with the styled Office 2007 dialog and text markup support.
    /// </summary>
    public class MessageBoxEx
    {
        /// <summary>
        /// Occurs when text markup link on Message Box is clicked. Markup links can be created using "a" tag, for example:
        /// <a name="MyLink">Markup link</a>
        /// </summary>
        public static event MarkupLinkClickEventHandler MarkupLinkClick;

        internal static void InvokeMarkupLinkClick(object sender, MarkupLinkClickEventArgs e)
        {
            MarkupLinkClickEventHandler h = MarkupLinkClick;
            if (h != null)
                h(sender, e);
        }

        /// <summary>
        /// Displays a message box with specified text.
        /// </summary>
        /// <param name="text">The text to display in the message box.</param>
        /// <returns>One of the DialogResult values.</returns>
        public static DialogResult Show(string text)
        {
            return ShowInternal(null, text, "", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, false);
        }

        /// <summary>
        /// Displays a message box in front of the specified object and with the specified text.
        /// </summary>
        /// <param name="owner">The IWin32Window the message box will display in front of. </param>
        /// <param name="text">The text to display in the message box. </param>
        /// <returns>One of the DialogResult values.</returns>
        public static DialogResult Show(IWin32Window owner, string text)
        {
            return ShowInternal(owner, text, "", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, GetTopMost(owner));
        }

        /// <summary>
        /// Displays a message box with specified text and caption.
        /// </summary>
        /// <param name="text">The text to display in the message box.</param>
        /// <param name="caption">The text to display in the title bar of the message box.</param>
        /// <returns>One of the DialogResult values.</returns>
        public static DialogResult Show(string text, string caption)
        {
            return ShowInternal(null, text, caption, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, false);
        }

        /// <summary>
        /// Displays a message box with specified text and caption.
        /// </summary>
        /// <param name="owner">The IWin32Window the message box will display in front of.</param>
        /// <param name="text">The text to display in the message box.</param>
        /// <param name="caption">The text to display in the title bar of the message box.</param>
        /// <returns>One of the DialogResult values.</returns>
        public static DialogResult Show(IWin32Window owner, string text, string caption)
        {
            return ShowInternal(owner, text, caption, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, GetTopMost(owner));
        }

        /// <summary>
        /// Displays a message box with specified text, caption, and buttons.
        /// </summary>
        /// <param name="text">The text to display in the message box. </param>
        /// <param name="caption">The text to display in the title bar of the message box.</param>
        /// <param name="buttons">One of the MessageBoxButtons values that specifies which buttons to display in the message box. </param>
        /// <returns>One of the DialogResult values.</returns>
        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons)
        {
            return ShowInternal(null, text, caption, buttons, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, false);
        }

        /// <summary>
        /// Displays a message box with specified text, caption, and buttons.
        /// </summary>
        /// <param name="owner">The IWin32Window the message box will display in front of.</param>
        /// <param name="text">The text to display in the message box. </param>
        /// <param name="caption">The text to display in the title bar of the message box.</param>
        /// <param name="buttons">One of the MessageBoxButtons values that specifies which buttons to display in the message box. </param>
        /// <returns>One of the DialogResult values.</returns>
        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons)
        {
            return ShowInternal(owner, text, caption, buttons, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, GetTopMost(owner));
        }

        /// <summary>
        /// Displays a message box with specified text, caption, buttons, and icon.
        /// </summary>
        /// <param name="text">The text to display in the message box. </param>
        /// <param name="caption">The text to display in the title bar of the message box.</param>
        /// <param name="buttons">One of the MessageBoxButtons values that specifies which buttons to display in the message box.</param>
        /// <param name="icon">One of the MessageBoxIcon values that specifies which icon to display in the message box.</param>
        /// <returns>One of the DialogResult values.</returns>
        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            return ShowInternal(null, text, caption, buttons, icon, MessageBoxDefaultButton.Button1, false);
        }

        /// <summary>
        /// Displays a message box with specified text, caption, buttons, and icon.
        /// </summary>
        /// <param name="owner">The IWin32Window the message box will display in front of.</param>
        /// <param name="text">The text to display in the message box. </param>
        /// <param name="caption">The text to display in the title bar of the message box.</param>
        /// <param name="buttons">One of the MessageBoxButtons values that specifies which buttons to display in the message box.</param>
        /// <param name="icon">One of the MessageBoxIcon values that specifies which icon to display in the message box.</param>
        /// <returns>One of the DialogResult values.</returns>
        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            return ShowInternal(owner, text, caption, buttons, icon, MessageBoxDefaultButton.Button1, GetTopMost(owner));
        }

        /// <summary>
        /// Displays a message box with the specified text, caption, buttons, icon, and default button.
        /// </summary>
        /// <param name="text">The text to display in the message box. </param>
        /// <param name="caption">The text to display in the title bar of the message box.</param>
        /// <param name="buttons">One of the MessageBoxButtons values that specifies which buttons to display in the message box.</param>
        /// <param name="icon">One of the MessageBoxIcon values that specifies which icon to display in the message box.</param>
        /// <param name="defaultButton">One of the MessageBoxDefaultButton values that specifies the default button for the message box. </param>
        /// <returns>One of the DialogResult values.</returns>
        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton)
        {
            return ShowInternal(null, text, caption, buttons, icon, defaultButton, false);
        }

        /// <summary>
        /// Displays a message box with the specified text, caption, buttons, icon, and default button.
        /// </summary>
        /// <param name="owner">The IWin32Window the message box will display in front of.</param>
        /// <param name="text">The text to display in the message box. </param>
        /// <param name="caption">The text to display in the title bar of the message box.</param>
        /// <param name="buttons">One of the MessageBoxButtons values that specifies which buttons to display in the message box.</param>
        /// <param name="icon">One of the MessageBoxIcon values that specifies which icon to display in the message box.</param>
        /// <param name="defaultButton">One of the MessageBoxDefaultButton values that specifies the default button for the message box. </param>
        /// <returns>One of the DialogResult values.</returns>
        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton)
        {
            return ShowInternal(owner, text, caption, buttons, icon, defaultButton, GetTopMost(owner));
        }

        /// <summary>
        /// Displays a message box with the specified text, caption, buttons, icon, and default button.
        /// </summary>
        /// <param name="owner">The IWin32Window the message box will display in front of.</param>
        /// <param name="text">The text to display in the message box. </param>
        /// <param name="caption">The text to display in the title bar of the message box.</param>
        /// <param name="buttons">One of the MessageBoxButtons values that specifies which buttons to display in the message box.</param>
        /// <param name="icon">One of the MessageBoxIcon values that specifies which icon to display in the message box.</param>
        /// <param name="defaultButton">One of the MessageBoxDefaultButton values that specifies the default button for the message box. </param>
        /// <param name="topMost">Indicates value for Message Box dialog TopMost property. </param>
        /// <returns>One of the DialogResult values.</returns>
        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, bool topMost)
        {
            return ShowInternal(owner, text, caption, buttons, icon, defaultButton, topMost);
        }

        private static DialogResult ShowInternal(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, bool topMost)
        {
            MessageBoxDialog d = new MessageBoxDialog();
            d.EnableGlass = _EnableGlass;
            d.AntiAlias = _AntiAlias;
            if (owner != null)
                d.StartPosition = _OwnerStartPosition; //FormStartPosition.CenterParent;
            else
                d.StartPosition = _DefaultStartPosition; // FormStartPosition.CenterScreen;
            d.MessageTextColor = _MessageBoxTextColor;
            d.ButtonsDividerVisible = _ButtonsDividerVisible;
            DialogResult r = d.Show(owner, text, caption, buttons, icon, defaultButton, topMost);
            d.Dispose();
            return r;
        }

        private static bool GetTopMost(IWin32Window owner)
        {
            if (owner is Form)
                return ((Form)owner).TopMost;
            return false;
        }

        private static bool _UseSystemLocalizedString = false;
        /// <summary>
        /// Gets or sets whether MessageBoxEx is using Windows System API function to retrieve the localized strings used by MessageBoxEx. Set this to false
        /// if you experience issues when using MessageBoxEx under certain conditions.
        /// </summary>
        public static bool UseSystemLocalizedString
        {
            get
            {
                return _UseSystemLocalizedString;
            }
            set
            {
                _UseSystemLocalizedString = value;
            }
        }

        private static bool _EnableGlass = true;
        /// <summary>
        /// Gets or sets whether MessageBoxEx form has Windows Vista Glass enabled if running on 
        /// Windows Vista with Glass enabled. Default value is true.
        /// </summary>
        public static bool EnableGlass
        {
            get { return _EnableGlass; }
            set
            {
                _EnableGlass = value;
            }
        }

        private static bool _AntiAlias = false;
        /// <summary>
        /// Gets or sets the anti-alias setting for message box text.
        /// </summary>
        public static bool AntiAlias
        {
            get { return _AntiAlias; }
            set
            {
                _AntiAlias = value;
            }
        }

        private static Color _MessageBoxTextColor = Color.Empty;
        /// <summary>
        /// Gets or sets the text color for the message box text. Default value is Color.Empty which indicates that system colors are used.
        /// </summary>
        public static Color MessageBoxTextColor
        {
            get { return _MessageBoxTextColor; }
            set
            {
                _MessageBoxTextColor = value;
            }
        }

        private static bool _ButtonsDividerVisible = true;
        /// <summary>
        /// Gets or sets whether divider panel that divides message box buttons and text content is visible. Default value is true.
        /// </summary>
        public static bool ButtonsDividerVisible
        {
            get { return _ButtonsDividerVisible; }
            set
            {
                _ButtonsDividerVisible = value;
            }
        }

        private static FormStartPosition _DefaultStartPosition = FormStartPosition.CenterScreen;
        /// <summary>
        /// Gets or sets the message box start position when Owner is not specified. Default value is CenterScreen.
        /// </summary>
        public static FormStartPosition DefaultStartPosition
        {
            get { return _DefaultStartPosition; }
            set { _DefaultStartPosition = value; }
        }

        private static FormStartPosition _OwnerStartPosition = FormStartPosition.CenterParent;
        /// <summary>
        /// Gets or sets the message box start position when Owner is specified. Default value is CenterParent.
        /// </summary>
        public static FormStartPosition OwnerStartPosition
        {
            get { return _OwnerStartPosition; }
            set { _OwnerStartPosition = value; }
        }
        
    }
}
