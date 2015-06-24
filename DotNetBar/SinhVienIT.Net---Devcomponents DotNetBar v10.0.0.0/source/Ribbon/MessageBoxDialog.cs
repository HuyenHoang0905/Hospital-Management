using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Globalization;

namespace DevComponents.DotNetBar
{
    internal class MessageBoxDialog : Office2007Form
    {
        private ButtonX Button1;
        private ButtonX Button2;
        private ButtonX Button3;
        private PictureBox PictureBox1;
        private PanelEx TextPanel;
        private PanelEx ButtonBackgroundPanel;
        private eDotNetBarStyle m_Style = eDotNetBarStyle.Office2007;
        private bool m_Button1Visible = true;
        private bool m_Button2Visible = true;
        private bool m_Button3Visible = true;
        private MessageBoxButtons m_Buttons = MessageBoxButtons.OK;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public MessageBoxDialog()
        {
            InitializeComponent();
            TextPanel.DisableSelection();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.Button1.Click -= new System.EventHandler(this.Button1_Click);
                this.Button2.Click -= new System.EventHandler(this.Button2_Click);
                this.Button3.Click -= new System.EventHandler(this.Button3_Click);
                this.TextPanel.MarkupLinkClick -= new MarkupLinkClickEventHandler(TextPanelMarkupLinkClick);
            }

            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Button1 = new DevComponents.DotNetBar.ButtonX();
            this.Button2 = new DevComponents.DotNetBar.ButtonX();
            this.Button3 = new DevComponents.DotNetBar.ButtonX();
            this.PictureBox1 = new System.Windows.Forms.PictureBox();
            this.TextPanel = new DevComponents.DotNetBar.PanelEx();
            this.ButtonBackgroundPanel = new DevComponents.DotNetBar.PanelEx();
            this.SuspendLayout();
            // 
            // Button1
            // 
            this.Button1.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.Button1.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.Button1.Location = new System.Drawing.Point(26, 85);
            this.Button1.Name = "Button1";
            this.Button1.Size = new System.Drawing.Size(77, 24);
            this.Button1.TabIndex = 0;
            this.Button1.Text = "&OK";
            this.Button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // Button2
            // 
            this.Button2.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.Button2.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.Button2.Location = new System.Drawing.Point(109, 85);
            this.Button2.Name = "Button2";
            this.Button2.Size = new System.Drawing.Size(77, 24);
            this.Button2.TabIndex = 1;
            this.Button2.Text = "&Cancel";
            this.Button2.Click += new System.EventHandler(this.Button2_Click);
            // 
            // Button3
            // 
            this.Button3.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.Button3.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.Button3.Location = new System.Drawing.Point(192, 85);
            this.Button3.Name = "Button3";
            this.Button3.Size = new System.Drawing.Size(77, 24);
            this.Button3.TabIndex = 2;
            this.Button3.Text = "&Ignore";
            this.Button3.Click += new System.EventHandler(this.Button3_Click);
            // 
            // PictureBox1
            // 
            this.PictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.PictureBox1.Location = new System.Drawing.Point(10, 10);
            this.PictureBox1.Name = "PictureBox1";
            this.PictureBox1.Size = new System.Drawing.Size(34, 34);
            this.PictureBox1.TabIndex = 3;
            this.PictureBox1.TabStop = false;
            // 
            // TextPanel
            // 
            this.TextPanel.AntiAlias = false;
            this.TextPanel.CanvasColor = System.Drawing.SystemColors.Control;
            this.TextPanel.Location = new System.Drawing.Point(53, 10);
            this.TextPanel.Name = "TextPanel";
            this.TextPanel.Size = new System.Drawing.Size(225, 53);
            this.TextPanel.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.TextPanel.Style.BorderWidth = 0;
            this.TextPanel.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.TextPanel.Style.GradientAngle = 90;
            this.TextPanel.Style.LineAlignment = System.Drawing.StringAlignment.Near;
            this.TextPanel.TabIndex = 4;
            this.TextPanel.TabStop = false;
            this.TextPanel.Style.WordWrap = true;
            this.TextPanel.MarkupLinkClick += new MarkupLinkClickEventHandler(TextPanelMarkupLinkClick);

            // 
            // ButtonBackgroundPanel
            // 
            this.ButtonBackgroundPanel.AntiAlias = false;
            this.ButtonBackgroundPanel.CanvasColor = System.Drawing.SystemColors.Control;
            this.ButtonBackgroundPanel.Location = new System.Drawing.Point(53, 10);
            this.ButtonBackgroundPanel.Name = "ButtonBackgroundPanel";
            this.ButtonBackgroundPanel.Size = new System.Drawing.Size(225, 42);
            this.ButtonBackgroundPanel.Dock = DockStyle.Bottom;
            this.ButtonBackgroundPanel.ColorSchemeStyle = eDotNetBarStyle.StyleManagerControlled;
            this.ButtonBackgroundPanel.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.ButtonBackgroundPanel.Style.BorderWidth = 1;
            this.ButtonBackgroundPanel.Style.BorderSide = eBorderSide.Top;
            this.ButtonBackgroundPanel.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.ButtonBackgroundPanel.Style.BackColor1.ColorSchemePart = eColorSchemePart.BarBackground;
            this.ButtonBackgroundPanel.Style.BorderColor.ColorSchemePart = eColorSchemePart.BarDockedBorder;
            this.ButtonBackgroundPanel.Style.GradientAngle = 90;
            this.ButtonBackgroundPanel.Style.LineAlignment = System.Drawing.StringAlignment.Near;
            this.ButtonBackgroundPanel.TabIndex = 4;
            this.ButtonBackgroundPanel.TabStop = false;
            // 
            // MessageBoxDialog
            // 
#if FRAMEWORK20
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
#endif
            this.ClientSize = new System.Drawing.Size(290, 121);
            this.ShowInTaskbar = false;
            this.Controls.Add(this.TextPanel);
            this.Controls.Add(this.PictureBox1);
            this.Controls.Add(this.Button3);
            this.Controls.Add(this.Button2);
            this.Controls.Add(this.Button1);
            this.Controls.Add(this.ButtonBackgroundPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MessageBoxDialog";
            this.ResumeLayout(false);

        }

        #endregion
        private void TextPanelMarkupLinkClick(object sender, MarkupLinkClickEventArgs e)
        {
            MessageBoxEx.InvokeMarkupLinkClick(sender, e);
        }

        public DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, bool topMost)
        {
            m_Buttons = buttons;
            this.Text = caption;
            TextPanel.Style.UseMnemonic = false;
            TextPanel.Text = text;
            if (icon != MessageBoxIcon.None)
            {
                Image iconImage= GetSystemImage(icon);
                if (iconImage != null && iconImage.Width > PictureBox1.Width) PictureBox1.Width = iconImage.Width;
                if (iconImage!=null && iconImage.Height > PictureBox1.Height) PictureBox1.Height = iconImage.Height;
                PictureBox1.Image = iconImage;
            }
            else
            {
                PictureBox1.Image = null;
                PictureBox1.Visible = false;
            }

            if (!BarFunctions.IsOffice2007Style(m_Style))
                this.EnableCustomStyle = false;

            if (buttons == MessageBoxButtons.OKCancel || buttons == MessageBoxButtons.RetryCancel || buttons == MessageBoxButtons.YesNo)
            {
                Button3.Visible = false;
                m_Button3Visible = false;
            }
            else if (buttons == MessageBoxButtons.OK)
            {
                Button2.Visible = false;
                Button3.Visible = false;
                m_Button2Visible = false;
                m_Button3Visible = false;
            }

            // Set Cancel and Accept buttons
            if (buttons == MessageBoxButtons.OK)
            {
                this.AcceptButton = Button1;
                this.CancelButton = Button1;
            }
            else if (buttons == MessageBoxButtons.OKCancel || buttons == MessageBoxButtons.RetryCancel || buttons == MessageBoxButtons.YesNo)
            {
                this.AcceptButton = Button1;
                this.CancelButton = Button2;
            }
            else if (buttons == MessageBoxButtons.YesNoCancel)
            {
                this.AcceptButton = Button1;
                this.CancelButton = Button3;
            }

            SetButtonText(buttons);

            if (defaultButton == MessageBoxDefaultButton.Button1 && m_Button1Visible)
            {
                Button1.Select();
                this.AcceptButton = Button1;
            }
            else if (defaultButton == MessageBoxDefaultButton.Button2 && m_Button2Visible)
            {
                this.AcceptButton = Button2;
                Button2.Select();
            }
            else if (defaultButton == MessageBoxDefaultButton.Button3 && m_Button3Visible)
            {
                this.AcceptButton = Button3;
                Button3.Select();
            }

            ResizeDialog();

            SetupColors();

#if FRAMEWORK20
            if (icon == MessageBoxIcon.Question)
                System.Media.SystemSounds.Question.Play(); // NativeFunctions.sndPlaySound("SystemQuestion", NativeFunctions.SND_ASYNC | NativeFunctions.SND_NODEFAULT);
            else if (icon == MessageBoxIcon.Asterisk)
                System.Media.SystemSounds.Asterisk.Play(); // NativeFunctions.sndPlaySound("SystemAsterisk", NativeFunctions.SND_ASYNC | NativeFunctions.SND_NODEFAULT);
            else
                System.Media.SystemSounds.Exclamation.Play(); // NativeFunctions.sndPlaySound("SystemExclamation", NativeFunctions.SND_ASYNC | NativeFunctions.SND_NODEFAULT);
#else
            if(icon == MessageBoxIcon.Question)
                NativeFunctions.sndPlaySound("SystemQuestion", NativeFunctions.SND_ASYNC | NativeFunctions.SND_NODEFAULT);
            else if(icon == MessageBoxIcon.Asterisk)
                NativeFunctions.sndPlaySound("SystemAsterisk", NativeFunctions.SND_ASYNC | NativeFunctions.SND_NODEFAULT);
            else
                NativeFunctions.sndPlaySound("SystemExclamation", NativeFunctions.SND_ASYNC | NativeFunctions.SND_NODEFAULT);
#endif
            if (buttons != MessageBoxButtons.OK)
            {
                this.CloseEnabled = false;
            }

            if(this.TopMost!=topMost)
                this.TopMost = topMost;
            return this.ShowDialog(owner);
        }

        private void SetupColors()
        {
            if (!_MessageTextColor.IsEmpty)
            {
                this.TextPanel.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.Custom;
                this.TextPanel.Style.ForeColor.Color = _MessageTextColor;
            }
            else if (BarFunctions.IsOffice2007Style(m_Style) && Rendering.GlobalManager.Renderer is Rendering.Office2007Renderer)
            {
                if (WinApi.IsGlassEnabled)
                {
                    this.TextPanel.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.Custom;
                    this.TextPanel.Style.ForeColor.Color = SystemColors.ControlText;
                }
                else
                {
                    Rendering.Office2007ColorTable ct = ((Rendering.Office2007Renderer)Rendering.GlobalManager.Renderer).ColorTable;
                    this.TextPanel.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.Custom;
                    this.TextPanel.Style.ForeColor.Color = ct.Form.TextColor;
                }
            }
            
            if (this.EnableGlass && WinApi.IsGlassEnabled)
            {
                Button1.ThemeAware = true;
                Button2.ThemeAware = true;
                Button3.ThemeAware = true;
                this.ButtonBackgroundPanel.Style.BackColor1.Color = SystemColors.ControlLight;
                this.ButtonBackgroundPanel.Style.BorderColor.Color = SystemColors.ControlDark;
            }
        }

        private bool _ButtonsDividerVisible = true;
        /// <summary>
        /// Gets or sets whether divider panel that divides message box buttons and text content is visible. Default value is true.
        /// </summary>
        public bool ButtonsDividerVisible
        {
            get { return _ButtonsDividerVisible; }
            set
            {
                if (_ButtonsDividerVisible != value)
                {
                    _ButtonsDividerVisible = value;
                    this.ButtonBackgroundPanel.Visible = _ButtonsDividerVisible;
                }
            }
        }
        

        private Color _MessageTextColor = Color.Empty;
        public Color MessageTextColor
        {
            get { return _MessageTextColor; }
            set { _MessageTextColor = value; }
        }

        private void SetButtonText(MessageBoxButtons buttons)
        {
            if (buttons == MessageBoxButtons.AbortRetryIgnore)
            {
                Button1.Text = GetString(SystemStrings.Abort);
                Button2.Text = GetString(SystemStrings.Retry);
                Button3.Text = GetString(SystemStrings.Ignore);
            }
            else if (buttons == MessageBoxButtons.OK)
            {
                Button1.Text = GetString(SystemStrings.OK);
            }
            else if (buttons == MessageBoxButtons.OKCancel)
            {
                Button1.Text = GetString(SystemStrings.OK);
                Button2.Text = GetString(SystemStrings.Cancel);
            }
            else if (buttons == MessageBoxButtons.RetryCancel)
            {
                Button1.Text = GetString(SystemStrings.Retry);
                Button2.Text = GetString(SystemStrings.Cancel);
            }
            else if (buttons == MessageBoxButtons.YesNo)
            {
                Button1.Text = GetString(SystemStrings.Yes);
                Button2.Text = GetString(SystemStrings.No);
            }
            else if (buttons == MessageBoxButtons.YesNoCancel)
            {
                Button1.Text = GetString(SystemStrings.Yes);
                Button2.Text = GetString(SystemStrings.No);
                Button3.Text = GetString(SystemStrings.Cancel);
            }
        }

        internal static Image GetSystemImage(MessageBoxIcon icon)
        {
            Icon ico = null;
            if (icon == MessageBoxIcon.Asterisk)
                ico = SystemIcons.Asterisk;
            else if (icon == MessageBoxIcon.Error || icon == MessageBoxIcon.Stop)
                ico = SystemIcons.Error;
            else if (icon == MessageBoxIcon.Exclamation)
                ico = SystemIcons.Exclamation;
            else if (icon == MessageBoxIcon.Hand)
                ico = SystemIcons.Hand;
            else if (icon == MessageBoxIcon.Information)
                ico = SystemIcons.Information;
            else if (icon == MessageBoxIcon.Question)
                ico = SystemIcons.Question;
            else if (icon == MessageBoxIcon.Warning)
                ico = SystemIcons.Warning;

            Bitmap bmp = new Bitmap(ico.Width, ico.Height);
            bmp.MakeTransparent();
            using (Graphics g = Graphics.FromImage(bmp))
            {
                if (System.Environment.Version.Build <= 3705 && System.Environment.Version.Revision == 288 && System.Environment.Version.Major == 1 && System.Environment.Version.Minor == 0)
                {
                    IntPtr hdc = g.GetHdc();
                    try
                    {
                        NativeFunctions.DrawIconEx(hdc, 0, 0, ico.Handle, ico.Width, ico.Height, 0, IntPtr.Zero, 3);
                    }
                    finally
                    {
                        g.ReleaseHdc(hdc);
                    }
                }
                else if (ico.Handle != IntPtr.Zero)
                {
                    try
                    {
                        g.DrawIcon(ico, 0,0);
                    }
                    catch { }
                }
            }
            return bmp;
        }

        private void ResizeDialog()
        {
            Size size = Size.Empty;
            int buttonSpacing = 6;
            int buttonMargin = 40;
            int textMargin = 10;
            int minTextSize = 110;

            if (PictureBox1.Image!=null)
            {
                TextPanel.Left = PictureBox1.Bounds.Right + 16;
            }
            else
                TextPanel.Left = PictureBox1.Left;

            Rectangle workingArea = Screen.PrimaryScreen.WorkingArea;
            TextPanel.Size = TextPanel.GetAutoSize(workingArea.Width);
            
            if (TextPanel.Size.Width > (double)workingArea.Width * .75d)
                TextPanel.Size = TextPanel.GetAutoSize((int)((double)workingArea.Width * .75d));
            else if (TextPanel.Size.Width < minTextSize)
                TextPanel.Width = minTextSize;
            
            // Measure the caption size
            if (this.Text.Length > 0)
            {
                Size captionSize = Size.Empty;
                Font font = this.Font;
                using (Graphics g = BarFunctions.CreateGraphics(this))
                {
                    size = TextDrawing.MeasureString(g, this.Text, font);
                }
                size.Width += 2;
                size.Height += 2;
                if (size.Width > TextPanel.Width)
                    TextPanel.Width = size.Width;
            }

            int y = Math.Max(TextPanel.Bounds.Bottom, PictureBox1.Bounds.Bottom);
            y += 19;

            Button1.Top = y;
            Button2.Top = y;
            Button3.Top = y;

            int buttonWidth = Button1.Width +
                (m_Button2Visible ? Button2.Width + buttonSpacing : 0) +
                (m_Button3Visible ? Button3.Width + buttonSpacing : 0);

            int buttonArea = buttonWidth + buttonMargin * 2;
            if (buttonWidth < TextPanel.Bounds.Right + textMargin)
                buttonArea = TextPanel.Bounds.Right + textMargin;
            else
            {
                TextPanel.Width += buttonArea - TextPanel.Bounds.Right - textMargin;
            }
           
            // Arrange buttons inside of the available area
            int x = (buttonArea - buttonWidth) / 2;
            Button1.Left = x;
            x += Button1.Width + buttonSpacing;

            if (m_Button2Visible)
            {
                Button2.Left = x;
                x += Button2.Width + buttonSpacing;
            }

            if (m_Button3Visible)
            {
                Button3.Left = x;
                x += Button3.Width + buttonSpacing;
            }

            size = new Size(TextPanel.Bounds.Right + textMargin + SystemInformation.FixedFrameBorderSize.Width * 2,
                Button1.Bounds.Bottom + textMargin + +SystemInformation.FixedFrameBorderSize.Height * 2 + SystemInformation.CaptionHeight);

            this.Size = size;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            DialogResult r = DialogResult.OK;
            if (m_Buttons == MessageBoxButtons.OK || m_Buttons == MessageBoxButtons.OKCancel)
                r = DialogResult.OK;
            else if (m_Buttons == MessageBoxButtons.YesNo || m_Buttons == MessageBoxButtons.YesNoCancel)
                r = DialogResult.Yes;
            else if (m_Buttons == MessageBoxButtons.AbortRetryIgnore)
                r = DialogResult.Abort;
            else if (m_Buttons == MessageBoxButtons.RetryCancel)
                r = DialogResult.Retry;

            this.DialogResult = r;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            DialogResult r = DialogResult.Cancel;
            if (m_Buttons == MessageBoxButtons.OKCancel)
                r = DialogResult.Cancel;
            else if (m_Buttons == MessageBoxButtons.YesNo || m_Buttons == MessageBoxButtons.YesNoCancel)
                r = DialogResult.No;
            else if (m_Buttons == MessageBoxButtons.AbortRetryIgnore)
                r = DialogResult.Retry;
            else if (m_Buttons == MessageBoxButtons.RetryCancel)
                r = DialogResult.Cancel;

            this.DialogResult = r;
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            DialogResult r = DialogResult.Cancel;
            if (m_Buttons == MessageBoxButtons.AbortRetryIgnore)
                r = DialogResult.Ignore;
            else if (m_Buttons == MessageBoxButtons.YesNoCancel)
                r = DialogResult.Cancel;

            this.DialogResult = r;
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == NativeFunctions.WM_SYSCOMMAND && (m.WParam.ToInt32() == NativeFunctions.SC_MAXIMIZE || m.WParam.ToInt32() == NativeFunctions.SC_MINIMIZE))
                return;
            base.WndProc(ref m);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if ((keyData & Keys.C) == Keys.C && (keyData & Keys.Control) == Keys.Control)
            {
                string s = "------------------------------" + "\r\n" + this.Text + "\r\n" + "------------------------------" +
                    "\r\n" + TextPanel.Text +
                    "\r\n" + "------------------------------" + "\r\n";
                if (Button1.Visible)
                    s += "[" + Button1.Text.Replace("&", "") + "]  ";
                if (Button2.Visible)
                    s += "[" + Button2.Text.Replace("&", "") + "]  ";
                if (Button3.Visible)
                    s += "[" + Button3.Text.Replace("&", "") + "]";
                s += "\r\n------------------------------";
#if (FRAMEWORK20)
                Clipboard.SetText(s);
#else
                Clipboard.SetDataObject(s);
#endif
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        /// <summary>
        /// Gets or sets the anti-alias setting for text-pane.
        /// </summary>
        public bool AntiAlias
        {
            get { return TextPanel.AntiAlias; }
            set
            {
                TextPanel.AntiAlias = value;
            }
        }
        

        #region System Strings
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern string MB_GetString(int i);

        private static string GetLocalizedText(SystemStrings sysString)
        {
            string key = "";
            string result = null;
            if (sysString == SystemStrings.Abort)
            {
                result = "&Abort";
                key = LocalizationKeys.MessageBoxAbortButton;
            }
            else if (sysString == SystemStrings.Cancel)
            {
                result = "&Cancel";
                key = LocalizationKeys.MessageBoxCancelButton;
            }
            else if (sysString == SystemStrings.Close)
            {
                result = "C&lose";
                key = LocalizationKeys.MessageBoxCloseButton;
            }
            else if (sysString == SystemStrings.Continue)
            {
                result = "Co&ntinue";
                key = LocalizationKeys.MessageBoxContinueButton;
            }
            else if (sysString == SystemStrings.Help)
            {
                result = "&Help";
                key = LocalizationKeys.MessageBoxHelpButton;
            }
            else if (sysString == SystemStrings.Ignore)
            {
                result = "&Ignore";
                key = LocalizationKeys.MessageBoxIgnoreButton;
            }
            else if (sysString == SystemStrings.No)
            {
                result = "&No";
                key = LocalizationKeys.MessageBoxNoButton;
            }
            else if (sysString == SystemStrings.OK)
            {
                result = "&OK";
                key = LocalizationKeys.MessageBoxOkButton;
            }
            else if (sysString == SystemStrings.Retry)
            {
                result = "&Retry";
                key = LocalizationKeys.MessageBoxRetryButton;
            }
            else if (sysString == SystemStrings.TryAgain)
            {
                result = "&Try Again";
                key = LocalizationKeys.MessageBoxTryAgainButton;
            }
            else if (sysString == SystemStrings.Yes)
            {
                result = "&Yes";
                key = LocalizationKeys.MessageBoxYesButton;
            }

            if (key != null)
            {
                result = LocalizationManager.GetLocalizedString(key, result);
            }

            return result;
        }
        private static string GetString(SystemStrings sysString)
        {
            string result = "";

            if (CultureInfo.CurrentUICulture.TwoLetterISOLanguageName.ToLower() != "en" && MessageBoxEx.UseSystemLocalizedString)
            {
                try
                {
                    result = MB_GetString((int)sysString);
                }
                catch
                {
                    result = "";
                }
            }

            if (result == "")
            {
                result = GetLocalizedText(sysString);
            }

            return result;
        }

        /// <summary>
        /// Enumeration of available common system strings.
        /// </summary>
        private enum SystemStrings
        {
            OK = 0,
            Cancel = 1,
            Abort = 2,
            Retry = 3,
            Ignore = 4,
            Yes = 5,
            No = 6,
            Close = 7,
            Help = 8,
            TryAgain = 9,
            Continue = 10
        }
        #endregion
    }
}