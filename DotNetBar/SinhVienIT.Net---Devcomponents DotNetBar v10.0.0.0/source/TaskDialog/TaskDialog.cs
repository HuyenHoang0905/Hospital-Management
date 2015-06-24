using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Represent a task-dialog message box window.
    /// </summary>
    public static class TaskDialog
    {
        /// <summary>
        /// Displays TaskDialog message.
        /// </summary>
        /// <param name="dialogTitle">Title of the window.</param>
        /// <param name="dialogHeader">Task dialog header.</param>
        /// <param name="dialogText">Task dialog text.</param>
        /// <param name="dialogButtons">Displayed buttons.</param>
        /// <returns>Result from task-dialog.</returns>
        public static eTaskDialogResult Show(string dialogTitle, string dialogHeader, string dialogText, eTaskDialogButton dialogButtons)
        {
            TaskDialogInfo info = new TaskDialogInfo(dialogTitle, eTaskDialogIcon.Information, dialogHeader, dialogText, dialogButtons);
            return Show(info);
        }

        /// <summary>
        /// Displays TaskDialog message.
        /// </summary>
        /// <param name="dialogTitle">Title of the window.</param>
        /// <param name="dialogHeader">Task dialog header.</param>
        /// <param name="dialogText">Task dialog text.</param>
        /// <param name="dialogButtons">Displayed buttons.</param>
        /// <param name="dialogColor">Specifies the predefined color for the dialog.</param>
        /// <returns>Result from task-dialog.</returns>
        public static eTaskDialogResult Show(string dialogTitle, string dialogHeader, string dialogText, eTaskDialogButton dialogButtons, eTaskDialogBackgroundColor dialogColor)
        {
            TaskDialogInfo info = new TaskDialogInfo(dialogTitle, eTaskDialogIcon.Information, dialogHeader, dialogText, dialogButtons, dialogColor);
            return Show(info);
        }

        /// <summary>
        /// Displays TaskDialog message.
        /// </summary>
        /// <param name="dialogTitle">Title of the window.</param>
        /// <param name="dialogIcon">Icon displayed on dialog.</param>
        /// <param name="dialogHeader">Task dialog header.</param>
        /// <param name="dialogText">Task dialog text.</param>
        /// <param name="dialogButtons">Displayed buttons.</param>
        /// <returns>Result from task-dialog.</returns>
        public static eTaskDialogResult Show(string dialogTitle, eTaskDialogIcon dialogIcon, string dialogHeader, string dialogText, eTaskDialogButton dialogButtons)
        {
            TaskDialogInfo info = new TaskDialogInfo(dialogTitle, dialogIcon, dialogHeader, dialogText, dialogButtons);
            return Show(info);
        }

        /// <summary>
        /// Displays TaskDialog message.
        /// </summary>
        /// <param name="dialogTitle">Title of the window.</param>
        /// <param name="dialogIcon">Icon displayed on dialog.</param>
        /// <param name="dialogHeader">Task dialog header.</param>
        /// <param name="dialogText">Task dialog text.</param>
        /// <param name="dialogButtons">Displayed buttons.</param>
        /// <param name="dialogColor">Specifies the predefined color for the dialog.</param>
        /// <returns>Result from task-dialog.</returns>
        public static eTaskDialogResult Show(string dialogTitle, eTaskDialogIcon dialogIcon, string dialogHeader, string dialogText, eTaskDialogButton dialogButtons, eTaskDialogBackgroundColor dialogColor)
        {
            TaskDialogInfo info = new TaskDialogInfo(dialogTitle, dialogIcon, dialogHeader, dialogText, dialogButtons, dialogColor);
            return Show(info);
        }

        /// <summary>
        /// Displays TaskDialog message.
        /// </summary>
        /// <param name="info">Specifies the content of the task dialog.</param>
        /// <returns>Result from task-dialog.</returns>
        public static eTaskDialogResult Show(TaskDialogInfo info)
        {
            return Show(null, info);
        }

        private static TaskDialogForm _TaskDialogForm = null;
        /// <summary>
        /// Displays TaskDialog message.
        /// </summary>
        /// <param name="owner">Window owner of the task dialog.</param>
        /// <param name="info">Specifies the content of the task dialog.</param>
        /// <returns>Result from task-dialog.</returns>
        public static eTaskDialogResult Show(IWin32Window owner, TaskDialogInfo info)
        {
            eTaskDialogResult result = eTaskDialogResult.None;
            TaskDialogForm taskDialog = new TaskDialogForm();
            try
            {
                _TaskDialogForm = taskDialog;
                if (!_AntiAlias)
                    taskDialog.AntiAlias = _AntiAlias;
                taskDialog.ShowTaskDialog(owner, info);
                result = taskDialog.Result;
            }
            finally
            {
                taskDialog.Dispose();
                _TaskDialogForm = null;
            }
            return result;
        }

        /// <summary>
        /// Closes the task dialog if it is open with eTaskDialogResult.None result.
        /// </summary>
        public static void Close()
        {
            Close(eTaskDialogResult.None);
        }
        /// <summary>
        /// Closes the task dialog if it is open with specified result value.
        /// </summary>
        /// <param name="result">Value that will be used as return value from Show method.</param>
        public static void Close(eTaskDialogResult result)
        {
            if (_TaskDialogForm == null)
                throw new NullReferenceException("Task Dialog Form is not shown.");
            _TaskDialogForm.CloseDialog(result);
        }

        private static bool _EnableGlass = true;
        /// <summary>
        /// Gets or sets whether TaskDialog form has Windows Vista Glass enabled if running on 
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

        private static bool _AntiAlias = true;
        /// <summary>
        /// Gets or sets the anti-alias text-rendering setting for the controls on task-dialog. Default value is true.
        /// </summary>
        public static bool AntiAlias
        {
            get { return _AntiAlias; }
            set
            {
                _AntiAlias = value;
            }
        }
        

        internal static Image GetImage(eTaskDialogIcon icon)
        {
            if (icon == eTaskDialogIcon.None) return null;
            return BarFunctions.LoadBitmap("SystemImages.Task" + icon.ToString() + ".png");
        }

        /// <summary>
        /// Occurs when any text markup link on Task-Dialog Box is clicked. Markup links can be created using "a" tag, for example:
        /// <a name="MyLink">Markup link</a>
        /// </summary>
        public static event MarkupLinkClickEventHandler MarkupLinkClick;

        internal static void InvokeMarkupLinkClick(object sender, MarkupLinkClickEventArgs e)
        {
            MarkupLinkClickEventHandler h = MarkupLinkClick;
            if (h != null)
                h(sender, e);
        }
    }

    /// <summary>
    /// Specifies the information displayed on task-dialog.
    /// </summary>
    public struct TaskDialogInfo
    {
        /// <summary>
        /// Initializes a new instance of the TaskDialogInfo structure.
        /// </summary>
        /// <param name="title">Title of dialog.</param>
        /// <param name="taskDialogIcon">Task-dialog icon</param>
        /// <param name="header">Header text.</param>
        /// <param name="text">Dialog main/content text.</param>
        /// <param name="dialogButtons">Dialog buttons displayed.</param>
        /// <param name="dialogColor">Dialog background color.</param>
        /// <param name="radioButtons">Radio Button Commands</param>
        /// <param name="buttons">Button commands.</param>
        /// <param name="checkBoxCommand">Check-box command.</param>
        /// <param name="footerText">Footer text</param>
        /// <param name="footerImage">Footer image.</param>
        public TaskDialogInfo(string title, eTaskDialogIcon taskDialogIcon, string header, string text, eTaskDialogButton dialogButtons, eTaskDialogBackgroundColor dialogColor, Command[] radioButtons, Command[] buttons, Command checkBoxCommand, string footerText, Image footerImage)
        {
            _Title = title;
            _Header = header;
            _Text = text;
            _DialogButtons = dialogButtons;
            _DialogColor = dialogColor;
            _RadioButtons = radioButtons;
            _Buttons = buttons;
            _FooterText = footerText;
            _CheckBoxCommand = checkBoxCommand;
            _TaskDialogIcon = taskDialogIcon;
            _FooterImage = footerImage;
        }

        /// <summary>
        /// Initializes a new instance of the TaskDialogInfo structure.
        /// </summary>
        /// <param name="title">Title of dialog.</param>
        /// <param name="taskDialogIcon">Task-dialog icon</param>
        /// <param name="header">Header text.</param>
        /// <param name="text">Dialog main/content text.</param>
        /// <param name="dialogButtons">Dialog buttons displayed.</param>
        /// <param name="dialogColor">Dialog background color.</param>
        public TaskDialogInfo(string title, eTaskDialogIcon taskDialogIcon, string header, string text, eTaskDialogButton dialogButtons, eTaskDialogBackgroundColor dialogColor)
        {
            _Title = title;
            _Header = header;
            _Text = text;
            _DialogButtons = dialogButtons;
            _DialogColor = dialogColor;
            _RadioButtons = null;
            _Buttons = null;
            _FooterText = null;
            _CheckBoxCommand = null;
            _TaskDialogIcon = taskDialogIcon;
            _FooterImage = null;
        }


        /// <summary>
        /// Initializes a new instance of the TaskDialogInfo structure.
        /// </summary>
        /// <param name="title">Title of dialog.</param>
        /// <param name="taskDialogIcon">Task-dialog icon</param>
        /// <param name="header">Header text.</param>
        /// <param name="text">Dialog main/content text.</param>
        /// <param name="dialogButtons">Dialog buttons displayed.</param>
        public TaskDialogInfo(string title, eTaskDialogIcon taskDialogIcon, string header, string text, eTaskDialogButton dialogButtons)
        {
            _Title = title;
            _Header = header;
            _Text = text;
            _DialogButtons = dialogButtons;
            _DialogColor = eTaskDialogBackgroundColor.Default;
            _RadioButtons = null;
            _Buttons = null;
            _FooterText = null;
            _CheckBoxCommand = null;
            _TaskDialogIcon = taskDialogIcon;
            _FooterImage = null;
        }

        private string _Title;
        /// <summary>
        /// Gets or sets the task-dialog window title.
        /// </summary>
        public string Title
        {
            get { return _Title; }
            set
            {
                _Title = value;
            }
        }

        private string _Header;
        /// <summary>
        /// Gets or sets the task-dialog header.
        /// </summary>
        public string Header
        {
            get { return _Header; }
            set
            {
                _Header = value;
            }
        }

        private string _Text;
        /// <summary>
        /// Gets or sets the task-dialog text.
        /// </summary>
        public string Text
        {
            get { return _Text; }
            set
            {
                _Text = value;
            }
        }

        private eTaskDialogButton _DialogButtons;
        /// <summary>
        /// Gets or sets the task-dialog buttons displayed.
        /// </summary>
        public eTaskDialogButton DialogButtons
        {
            get { return _DialogButtons; }
            set { _DialogButtons = value; }
        }

        private eTaskDialogBackgroundColor _DialogColor;
        /// <summary>
        /// Gets or sets the task-dialog background color.
        /// </summary>
        public eTaskDialogBackgroundColor DialogColor
        {
            get { return _DialogColor; }
            set { _DialogColor = value; }
        }

        private Command[] _RadioButtons;
        /// <summary>
        /// Gets or sets the array of commands that will be used to create the radio-buttons displayed on task-dialog. Each command will be executed as radio-buttons are checked by user.
        /// </summary>
        public Command[] RadioButtons
        {
            get { return _RadioButtons; }
            set { _RadioButtons = value; }
        }

        private Command[] _Buttons;
        /// <summary>
        /// Gets or sets the array of commands that will be used to create the buttons displayed on task-dialog. Each command will be executed as buttons are clicked by user.
        /// </summary>
        public Command[] Buttons
        {
            get { return _Buttons; }
            set { _Buttons = value; }
        }

        private string _FooterText;
        /// <summary>
        /// Gets or sets the footer text displayed on task-dialog.
        /// </summary>
        public string FooterText
        {
            get { return _FooterText; }
            set
            {
                _FooterText = value;
            }
        }

        private Command _CheckBoxCommand;
        /// <summary>
        /// Gets or sets the command that is used to initialize the footer check-box. Command will be executed when check-box state changes by end user.
        /// </summary>
        public Command CheckBoxCommand
        {
            get { return _CheckBoxCommand; }
            set { _CheckBoxCommand = value; }
        }

        private eTaskDialogIcon _TaskDialogIcon;
        /// <summary>
        /// Gets or sets the icon that is displayed on task dialog.
        /// </summary>
        public eTaskDialogIcon TaskDialogIcon
        {
            get { return _TaskDialogIcon; }
            set { _TaskDialogIcon = value; }
        }

        private Image _FooterImage;
        /// <summary>
        /// Gets or sets the image that is displayed in the task-dialog footer. Expected image size is 16x16 pixels.
        /// </summary>
        public Image FooterImage
        {
            get { return _FooterImage; }
            set { _FooterImage = value; }
        }
    }

    /// <summary>
    /// Specifies the task dialog buttons.
    /// </summary>
    [Flags]
    public enum eTaskDialogButton
    {
        /// <summary>
        /// OK button will be displayed.
        /// </summary>
        Ok = 1,
        /// <summary>
        /// Yes button will be displayed.
        /// </summary>
        Yes = 2,
        /// <summary>
        /// No button will be displayed.
        /// </summary>
        No = 4,
        /// <summary>
        /// Cancel button will be displayed.
        /// </summary>
        Cancel = 8,
        /// <summary>
        /// Retry button will be displayed.
        /// </summary>
        Retry = 16,
        /// <summary>
        /// Close button will be displayed.
        /// </summary>
        Close = 32
    }

    /// <summary>
    /// Specifies the task dialog return values.
    /// </summary>
    public enum eTaskDialogResult
    {
        /// <summary>
        /// No button was clicked because dialog was closed using TaskDialog.Close method.
        /// </summary>
        None,
        /// <summary>
        /// OK button was clicked.
        /// </summary>
        Ok,
        /// <summary>
        /// Yes button was clicked.
        /// </summary>
        Yes,
        /// <summary>
        /// No button was clicked.
        /// </summary>
        No,
        /// <summary>
        /// Cancel button was clicked.
        /// </summary>
        Cancel,
        /// <summary>
        /// Retry button was clicked.
        /// </summary>
        Retry,
        /// <summary>
        /// Close button was clicked.
        /// </summary>
        Close,
        /// <summary>
        /// Specifies the custom result. Custom result can be specified if TaskDialog.Close method is called to close dialog.
        /// </summary>
        Custom1,
        /// <summary>
        /// Specifies the custom result. Custom result can be specified if TaskDialog.Close method is called to close dialog.
        /// </summary>
        Custom2,
        /// <summary>
        /// Specifies the custom result. Custom result can be specified if TaskDialog.Close method is called to close dialog.
        /// </summary>
        Custom3
    }

    /// <summary>
    /// Define icons available on TaskDialog.
    /// </summary>
    public enum eTaskDialogIcon
    {
        /// <summary>
        /// No icon.
        /// </summary>
        None,
        /// <summary>
        /// Blue flag icon.
        /// </summary>
        BlueFlag,
        /// <summary>
        /// Blue stop icon.
        /// </summary>
        BlueStop,
        /// <summary>
        /// Light bulb, idea icon.
        /// </summary>
        Bulb,
        /// <summary>
        /// Check-mark icon.
        /// </summary>
        CheckMark,
        /// <summary>
        /// Check-mark icon.
        /// </summary>
        CheckMark2,
        /// <summary>
        /// Trash-can delete icon.
        /// </summary>
        Delete,
        /// <summary>
        /// Exclamation icon.
        /// </summary>
        Exclamation,
        /// <summary>
        /// Flag icon.
        /// </summary>
        Flag,
        /// <summary>
        /// Hand-stop icon.
        /// </summary>
        Hand,
        /// <summary>
        /// Help icon.
        /// </summary>
        Help,
        /// <summary>
        /// Informational icon.
        /// </summary>
        Information,
        /// <summary>
        /// Informational icon.
        /// </summary>
        Information2,
        /// <summary>
        /// No entry icon.
        /// </summary>
        NoEntry,
        /// <summary>
        /// Shield icon.
        /// </summary>
        Shield,
        /// <summary>
        /// Shield help icon.
        /// </summary>
        ShieldHelp,
        /// <summary>
        /// Shield OK icon.
        /// </summary>
        ShieldOk,
        /// <summary>
        /// Shield stop icon.
        /// </summary>
        ShieldStop,
        /// <summary>
        /// Stop icon.
        /// </summary>
        Stop,
        /// <summary>
        /// Stop icon.
        /// </summary>
        Stop2,
        /// <summary>
        /// Users icons.
        /// </summary>
        Users
    }
}
