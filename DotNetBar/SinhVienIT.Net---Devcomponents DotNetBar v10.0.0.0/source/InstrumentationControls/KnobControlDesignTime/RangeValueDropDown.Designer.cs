namespace DevComponents.Instrumentation.Design
{
    partial class RangeValueDropDown
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
            this._TrackBar = new System.Windows.Forms.TrackBar();
            this._LabelValue = new System.Windows.Forms.Label();
            this._LabelMin = new System.Windows.Forms.Label();
            this._LabelMax = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this._TrackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // _TrackBar
            // 
            this._TrackBar.AutoSize = false;
            this._TrackBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this._TrackBar.Location = new System.Drawing.Point(0, 0);
            this._TrackBar.Name = "_TrackBar";
            this._TrackBar.Size = new System.Drawing.Size(186, 40);
            this._TrackBar.TabIndex = 0;
            this._TrackBar.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this._TrackBar_PreviewKeyDown);
            this._TrackBar.ValueChanged += new System.EventHandler(this.TrackBar_ValueChanged);
            // 
            // _LabelValue
            // 
            this._LabelValue.AutoEllipsis = true;
            this._LabelValue.Location = new System.Drawing.Point(59, 21);
            this._LabelValue.Name = "_LabelValue";
            this._LabelValue.Size = new System.Drawing.Size(69, 16);
            this._LabelValue.TabIndex = 1;
            this._LabelValue.Text = "Value";
            this._LabelValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // _LabelMin
            // 
            this._LabelMin.AutoEllipsis = true;
            this._LabelMin.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this._LabelMin.Location = new System.Drawing.Point(3, 21);
            this._LabelMin.Name = "_LabelMin";
            this._LabelMin.Size = new System.Drawing.Size(50, 16);
            this._LabelMin.TabIndex = 2;
            this._LabelMin.Text = "Min";
            this._LabelMin.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // _LabelMax
            // 
            this._LabelMax.AutoEllipsis = true;
            this._LabelMax.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this._LabelMax.Location = new System.Drawing.Point(134, 21);
            this._LabelMax.Name = "_LabelMax";
            this._LabelMax.Size = new System.Drawing.Size(52, 16);
            this._LabelMax.TabIndex = 3;
            this._LabelMax.Text = "Max";
            this._LabelMax.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // RangeValueDropDown
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._LabelMax);
            this.Controls.Add(this._LabelMin);
            this.Controls.Add(this._LabelValue);
            this.Controls.Add(this._TrackBar);
            this.Name = "RangeValueDropDown";
            this.Size = new System.Drawing.Size(186, 40);
            ((System.ComponentModel.ISupportInitialize)(this._TrackBar)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TrackBar _TrackBar;
        private System.Windows.Forms.Label _LabelValue;
        private System.Windows.Forms.Label _LabelMin;
        private System.Windows.Forms.Label _LabelMax;


    }
}
