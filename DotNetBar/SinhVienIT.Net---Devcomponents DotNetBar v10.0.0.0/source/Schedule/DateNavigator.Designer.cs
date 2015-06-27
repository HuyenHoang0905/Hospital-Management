#if FRAMEWORK20
namespace DevComponents.DotNetBar.Schedule
{
    partial class DateNavigator
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
                CalendarView = null;
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.CurrentDateLabel = new LabelX();
            this.NavigateForward = new DevComponents.DotNetBar.ButtonX();
            this.NavigateBack = new DevComponents.DotNetBar.ButtonX();
            this.SuspendLayout();
            // 
            // CurrentDateLabel
            // 
            this.CurrentDateLabel.AutoSize = true;
            this.CurrentDateLabel.AutoSize = true;
            this.CurrentDateLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CurrentDateLabel.Location = new System.Drawing.Point(54, 4);
            this.CurrentDateLabel.Name = "CurrentDateLabel";
            this.CurrentDateLabel.Size = new System.Drawing.Size(132, 20);
            this.CurrentDateLabel.TabIndex = 1;
            this.CurrentDateLabel.Text = "October 20, 2009";
            // 
            // NavigateForward
            // 
            this.NavigateForward.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.NavigateForward.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.NavigateForward.Location = new System.Drawing.Point(31, 4);
            this.NavigateForward.Name = "NavigateForward";
            this.NavigateForward.Shape = new DevComponents.DotNetBar.EllipticalShapeDescriptor();
            this.NavigateForward.Size = new System.Drawing.Size(20, 20);
            this.NavigateForward.TabIndex = 0;
            this.NavigateForward.Click += new System.EventHandler(this.NavigateForward_Click);
            this.NavigateForward.FocusCuesEnabled = false;
            // 
            // NavigateBack
            // 
            this.NavigateBack.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.NavigateBack.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.NavigateBack.Location = new System.Drawing.Point(5, 4);
            this.NavigateBack.Name = "NavigateBack";
            this.NavigateBack.Shape = new DevComponents.DotNetBar.EllipticalShapeDescriptor();
            this.NavigateBack.Size = new System.Drawing.Size(20, 20);
            this.NavigateBack.TabIndex = 0;
            this.NavigateBack.Click += new System.EventHandler(this.NavigateBack_Click);
            this.NavigateBack.FocusCuesEnabled = false;
            // 
            // DateNavigator
            // 
            this.Controls.Add(this.CurrentDateLabel);
            this.Controls.Add(this.NavigateForward);
            this.Controls.Add(this.NavigateBack);
            this.Name = "DateNavigator";
            this.Size = new System.Drawing.Size(223, 30);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ButtonX NavigateBack;
        private ButtonX NavigateForward;
        private LabelX CurrentDateLabel;
    }
}
#endif

