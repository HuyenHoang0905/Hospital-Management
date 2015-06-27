using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar
{
    internal partial class TaskDialogForm : DevComponents.DotNetBar.Office2007Form
    {
        public TaskDialogForm()
        {
            InitializeComponent();

            Office2007ColorTable table = ((Office2007Renderer)GlobalManager.Renderer).ColorTable;
            bottomPanel.Style.BackColor1.Color = table.Form.BackColor;

            headerLabel.MarkupLinkClick += new MarkupLinkClickEventHandler(MarkupLinkClick);
            contentLabel.MarkupLinkClick += new MarkupLinkClickEventHandler(MarkupLinkClick);
            footerLabel.MarkupLinkClick += new MarkupLinkClickEventHandler(MarkupLinkClick);
            this.ShowInTaskbar = false;
        }

        private void MarkupLinkClick(object sender, MarkupLinkClickEventArgs e)
        {
            TaskDialog.InvokeMarkupLinkClick(sender, e);
        }

        private void TaskDialogForm_Load(object sender, EventArgs e)
        {

        }

        private eTaskDialogBackgroundColor _TaskBackgroundColor = eTaskDialogBackgroundColor.Default;
        /// <summary>
        /// Gets or sets the task-background color.
        /// </summary>
        [DefaultValue(eTaskDialogBackgroundColor.Default)]
        public eTaskDialogBackgroundColor TaskBackgroundColor
        {
            get { return _TaskBackgroundColor; }
            set { _TaskBackgroundColor = value; }
        }
        private static readonly int MinimumWidth = 420;
        private static readonly int MinimumHeight = 200;
        public eTaskDialogResult ShowTaskDialog(IWin32Window owner, TaskDialogInfo dialogInfo)
        {
            if (owner != null)
                this.StartPosition = FormStartPosition.CenterParent;
            else
                this.StartPosition = FormStartPosition.CenterScreen;

            UpdateDialogColor(dialogInfo);

            int dialogButtonsWidth = 20;
            int footerWidth = 0;
            int contentHeight = 32;

            this.Text = dialogInfo.Title;
            headerLabel.Text = dialogInfo.Header;
            contentLabel.Text = dialogInfo.Text;

            if (dialogInfo.TaskDialogIcon == eTaskDialogIcon.None)
            {
                headerLabel.Left = headerImage.Left;
                contentLabel.Left = headerImage.Left;
                buttonsPanel.Left = headerImage.Left;
                headerImage.Visible = false;
            }
            else
                headerImage.Image = TaskDialog.GetImage(dialogInfo.TaskDialogIcon);

            if (dialogInfo.CheckBoxCommand != null)
            {
                taskCheckBox.Command = dialogInfo.CheckBoxCommand;
                //taskCheckBox.Checked = dialogInfo.CheckBoxCommand.Checked;
                dialogButtonsWidth += taskCheckBox.Width + 12;
            }
            else
                taskCheckBox.Visible = false;
            
            if (!string.IsNullOrEmpty(dialogInfo.FooterText))
            {
                footerLabel.Text = dialogInfo.FooterText;
                footerWidth += footerLabel.Width + 12;
            }
            else
                footerLabel.Visible = false;

            if (dialogInfo.FooterImage != null)
            {
                footerImage.Image = dialogInfo.FooterImage;
                footerWidth += footerImage.Width;
            }
            else
            {
                footerImage.Visible = false;
                footerLabel.Left = footerImage.Left;
            }

            if (dialogInfo.RadioButtons != null && dialogInfo.RadioButtons.Length > 0)
            {
                foreach (Command command in dialogInfo.RadioButtons)
                {
                    CheckBoxItem item = new CheckBoxItem();
                    item.CheckBoxStyle = eCheckBoxStyle.RadioButton;
                    //item.Checked = command.Checked;
                    item.Command = command;
                    buttonsPanel.Items.Add(item);
                } 
            }

            if (dialogInfo.Buttons != null && dialogInfo.Buttons.Length > 0)
            {
                foreach (Command command in dialogInfo.Buttons)
                {
                    ButtonItem item = new ButtonItem();
                    if (command.Image != null)
                    {
                        item.ButtonStyle = eButtonStyle.ImageAndText;
                        item.ImagePosition = eImagePosition.Left;
                    }
                    
                    item.Command = command;

                    buttonsPanel.Items.Add(item);
                }
            }

            if ((dialogInfo.DialogButtons & eTaskDialogButton.Ok) == 0)
            {
                buttonOk.Visible = false;
            }
            else
                dialogButtonsWidth += buttonOk.Width + 3;
            if ((dialogInfo.DialogButtons & eTaskDialogButton.Cancel) == 0)
            {
                buttonCancel.Visible = false;
            }
            else
            {
                dialogButtonsWidth += buttonCancel.Width + 3;
                this.CancelButton = buttonCancel;
            }
            if ((dialogInfo.DialogButtons & eTaskDialogButton.Yes) == 0)
            {
                buttonYes.Visible = false;
            }
            else
                dialogButtonsWidth += buttonYes.Width + 3;
            if ((dialogInfo.DialogButtons & eTaskDialogButton.No) == 0)
            {
                buttonNo.Visible = false;
            }
            else
            {
                dialogButtonsWidth += buttonNo.Width + 3;
                if (this.CancelButton == null)
                    this.CancelButton = buttonNo;
            }
            if ((dialogInfo.DialogButtons & eTaskDialogButton.Close) == 0)
            {
                buttonClose.Visible = false;
            }
            else
            {
                dialogButtonsWidth += buttonClose.Width + 3;
                if (this.CancelButton == null)
                    this.CancelButton = buttonClose;
            }
            if ((dialogInfo.DialogButtons & eTaskDialogButton.Retry) == 0)
            {
                buttonRetry.Visible = false;
            }
            else
                dialogButtonsWidth += buttonRetry.Width + 3;

            // If only OK button is visible it is cancel button as well
            if (dialogInfo.DialogButtons == eTaskDialogButton.Ok)
                this.CancelButton = buttonOk;

            if (string.IsNullOrEmpty(dialogInfo.FooterText) && dialogInfo.FooterImage == null)
            {
                footerPanel.Visible = false;
                bottomPanel.Height = flowLayoutPanel1.Height + 4;
            }
            else
                contentHeight += footerImage.Height;

            this.Width = Math.Max(MinimumWidth, Math.Max(footerWidth, dialogButtonsWidth));

            using (Graphics g = headerLabel.CreateGraphics()) { }
            headerLabel.MaximumSize=new Size(headerLabel.Width, 1000);
            headerLabel.Height=headerLabel.GetPreferredSize(Size.Empty).Height;
            contentHeight += headerLabel.Height + 3;

            using (Graphics g = contentLabel.CreateGraphics()) { }
            contentLabel.Top = headerLabel.Bounds.Bottom + 3;
            contentLabel.MaximumSize = new Size(contentLabel.Width, 1000);
            contentLabel.Height=contentLabel.GetPreferredSize(Size.Empty).Height;
            contentHeight += contentLabel.Height + 3;

            buttonsPanel.Top = contentLabel.Bottom + 3;
            if (buttonsPanel.Items.Count == 0)
            {
                buttonsPanel.Visible = false;
            }
            else
            {
                using (Graphics g = buttonsPanel.CreateGraphics()) { }
                buttonsPanel.Height = buttonsPanel.GetAutoSizeHeight() + 2;
                contentHeight += buttonsPanel.Height + 6;
                if (string.IsNullOrEmpty(dialogInfo.FooterText) && dialogInfo.FooterImage == null)
                    contentHeight += 8;
            }

            contentHeight += bottomPanel.Height;

            this.Height = Math.Max(MinimumHeight, contentHeight);

            if (dialogInfo.TaskDialogIcon == eTaskDialogIcon.Help)
                System.Media.SystemSounds.Question.Play();
            else if (dialogInfo.TaskDialogIcon == eTaskDialogIcon.Information)
                System.Media.SystemSounds.Asterisk.Play();
            else
                System.Media.SystemSounds.Exclamation.Play();

            LocalizeText();

            ShowDialog(owner);

            return _Result;
        }

        private void LocalizeText()
        {
            buttonCancel.Text = LocalizationManager.GetLocalizedString(LocalizationKeys.MessageBoxCancelButton, buttonCancel.Text);
            buttonClose.Text = LocalizationManager.GetLocalizedString(LocalizationKeys.MessageBoxCloseButton, buttonClose.Text);
            buttonNo.Text = LocalizationManager.GetLocalizedString(LocalizationKeys.MessageBoxNoButton, buttonNo.Text);
            buttonOk.Text = LocalizationManager.GetLocalizedString(LocalizationKeys.MessageBoxOkButton, buttonOk.Text);
            buttonRetry.Text = LocalizationManager.GetLocalizedString(LocalizationKeys.MessageBoxRetryButton, buttonRetry.Text);
            buttonYes.Text = LocalizationManager.GetLocalizedString(LocalizationKeys.MessageBoxYesButton, buttonYes.Text);
        }

        private void UpdateDialogColor(TaskDialogInfo dialogInfo)
        {
            eTaskDialogBackgroundColor color = dialogInfo.DialogColor;

            if (color == eTaskDialogBackgroundColor.Aqua)
            {
                //this.BackColor = ColorScheme.GetColor(0xDBEEF3);
                bottomPanel.Style.BackColor1.Color = ColorScheme.GetColor(0xDBEEF3);
                headerLabel.ForeColor = ColorScheme.GetColor(0x205867);
                contentLabel.ForeColor = ColorScheme.GetColor(0x205867);
            }
            else if (color == eTaskDialogBackgroundColor.Blue)
            {
                bottomPanel.Style.BackColor1.Color = ColorScheme.GetColor(0xDBE5F1);
                headerLabel.ForeColor = ColorScheme.GetColor(0x244061);
                contentLabel.ForeColor = ColorScheme.GetColor(0x244061);
            }
            else if (color == eTaskDialogBackgroundColor.DarkBlue)
            {
                bottomPanel.Style.BackColor1.Color = ColorScheme.GetColor(0xC6D9F0);
                headerLabel.ForeColor = ColorScheme.GetColor(0x0F243E);
                contentLabel.ForeColor = ColorScheme.GetColor(0x0F243E);
            }
            else if (color == eTaskDialogBackgroundColor.OliveGreen)
            {
                bottomPanel.Style.BackColor1.Color = ColorScheme.GetColor(0xEBF1DD);
                headerLabel.ForeColor = ColorScheme.GetColor(0x4F6128);
                contentLabel.ForeColor = ColorScheme.GetColor(0x4F6128);
            }
            else if (color == eTaskDialogBackgroundColor.Orange)
            {
                bottomPanel.Style.BackColor1.Color = ColorScheme.GetColor(0xFDEADA);
                headerLabel.ForeColor = ColorScheme.GetColor(0x974806);
                contentLabel.ForeColor = ColorScheme.GetColor(0x974806);
            }
            else if (color == eTaskDialogBackgroundColor.Purple)
            {
                bottomPanel.Style.BackColor1.Color = ColorScheme.GetColor(0xE5E0EC);
                headerLabel.ForeColor = ColorScheme.GetColor(0x3F3151);
                contentLabel.ForeColor = ColorScheme.GetColor(0x3F3151);
            }
            else if (color == eTaskDialogBackgroundColor.Red)
            {
                bottomPanel.Style.BackColor1.Color = ColorScheme.GetColor(0xF2DCDB);
                headerLabel.ForeColor = ColorScheme.GetColor(0x632423);
                contentLabel.ForeColor = ColorScheme.GetColor(0x632423);
            }
            else if (color == eTaskDialogBackgroundColor.Silver)
            {
                bottomPanel.Style.BackColor1.Color = ColorScheme.GetColor(0xF2F2F2);
                headerLabel.ForeColor = ColorScheme.GetColor(0x0C0C0C);
                contentLabel.ForeColor = ColorScheme.GetColor(0x0C0C0C);
            }
            else if (color == eTaskDialogBackgroundColor.Tan)
            {
                bottomPanel.Style.BackColor1.Color = ColorScheme.GetColor(0xDDD9C3);
                headerLabel.ForeColor = ColorScheme.GetColor(0x1D1B10);
                contentLabel.ForeColor = ColorScheme.GetColor(0x1D1B10);
            }

            if (color != eTaskDialogBackgroundColor.Default)
                bottomPanel.Style.BorderColor.Color = headerLabel.ForeColor;
        }

        private eTaskDialogResult _Result = eTaskDialogResult.None;
        /// <summary>
        /// Gets the task-dialog result
        /// </summary>
        public eTaskDialogResult Result
        {
            get { return _Result; }
        }

        private bool _AntiAlias = true;
        /// <summary>
        /// Gets or sets the anti-alias text-rendering setting for the controls.
        /// </summary>
        public bool AntiAlias
        {
            get { return _AntiAlias; }
            set
            {
                _AntiAlias = value;
                headerLabel.AntiAlias = value;
                contentLabel.AntiAlias = value;
                buttonsPanel.AntiAlias = value;
                buttonOk.AntiAlias = value;
                buttonCancel.AntiAlias = value;
                buttonYes.AntiAlias = value;
                buttonNo.AntiAlias = value;
                buttonRetry.AntiAlias = value;
                footerLabel.AntiAlias = value;
                taskCheckBox.AntiAlias = value;
            }
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            CloseDialog(eTaskDialogResult.Ok);
        }

        private void buttonYes_Click(object sender, EventArgs e)
        {
            CloseDialog(eTaskDialogResult.Yes);
        }

        private void buttonNo_Click(object sender, EventArgs e)
        {
            CloseDialog(eTaskDialogResult.No);
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            CloseDialog(eTaskDialogResult.Cancel);
        }

        private void buttonRetry_Click(object sender, EventArgs e)
        {
            CloseDialog(eTaskDialogResult.Retry);
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            CloseDialog(eTaskDialogResult.Close);
        }

        internal void CloseDialog(eTaskDialogResult result)
        {
            _Result = result;
            this.Close();
        }
    }

    /// <summary>
    /// Defines TaskDialog colors.
    /// </summary>
    public enum eTaskDialogBackgroundColor
    {
        /// <summary>
        /// Task dialog will use default background as specified by current theme.
        /// </summary>
        Default,
        /// <summary>
        /// Task dialog will use silver background color.
        /// </summary>
        Silver,
        /// <summary>
        /// Task dialog will use tan background color.
        /// </summary>
        Tan,
        /// <summary>
        /// Task dialog will use dark-blue background color.
        /// </summary>
        DarkBlue,
        /// <summary>
        /// Task dialog will use blue background color.
        /// </summary>
        Blue,
        /// <summary>
        /// Task dialog will use red background color.
        /// </summary>
        Red,
        /// <summary>
        /// Task dialog will use olive-green background color.
        /// </summary>
        OliveGreen,
        /// <summary>
        /// Task dialog will use purple background color.
        /// </summary>
        Purple,
        /// <summary>
        /// Task dialog will use aqua background color.
        /// </summary>
        Aqua,
        /// <summary>
        /// Task dialog will use orange background color.
        /// </summary>
        Orange
    }
}