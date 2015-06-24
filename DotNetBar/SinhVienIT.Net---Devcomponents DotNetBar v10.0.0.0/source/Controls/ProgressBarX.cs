using System;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Controls
{
    /// <summary>
    /// Represents the stand-alone progress bar control.
    /// </summary>
    [ToolboxBitmap(typeof(ProgressBarX), "Controls.ProgressBarX.ico"), ToolboxItem(true), DefaultEvent("Click"), System.Runtime.InteropServices.ComVisible(false)]
    public class ProgressBarX : BaseItemControl
    {
        #region Private Variables
        private ProgressBarItem m_ProgressBar = null;
        #endregion

        #region Constructor, Dispose
        public ProgressBarX()
        {
            m_ProgressBar = new ProgressBarItem();
            m_ProgressBar.Style = eDotNetBarStyle.Office2007;
            m_ProgressBar.Minimum = 0;
            m_ProgressBar.Maximum = 100;
            m_ProgressBar.Value = 0;
            m_ProgressBar.BackStyle = this.BackgroundStyle;

            this.HostItem = m_ProgressBar;
        }
        #endregion

        #region Internal Implementation
        protected override void PaintBackground(PaintEventArgs e)
        {
            // ProgressBarItem shares same background style as control so it will paint itself. This avoids double painting...
            Graphics g = e.Graphics;
            Rectangle r = this.ClientRectangle;
            ElementStyle style = this.GetBackgroundStyle();

            if ((!style.Custom || style.BackColor.IsEmpty)&& !this.BackColor.IsEmpty && this.BackColor!=Color.Transparent)
            {
                DisplayHelp.FillRectangle(g, r, this.BackColor);
            }
        }

        /// <summary>
        /// Gets or sets the maximum value of the range of the control.
        /// </summary>
        [Browsable(true), Description("Gets or sets the maximum value of the range of the control."), Category("Behavior"), DefaultValue(100)]
        public int Maximum
        {
            get
            {
                return m_ProgressBar.Maximum;
            }
            set
            {
                m_ProgressBar.Maximum = value;
            }
        }

        /// <summary>
        /// Gets or sets the minimum value of the range of the control.
        /// </summary>
        [DevCoBrowsable(true), Browsable(true), Description("Gets or sets the minimum value of the range of the control."), Category("Behavior"), DefaultValue(0)]
        public int Minimum
        {
            get
            {
                return m_ProgressBar.Minimum;
            }
            set
            {
                m_ProgressBar.Minimum = value;
            }
        }

        /// <summary>
        /// Gets or sets the current position of the progress bar.
        /// </summary>
        [DevCoBrowsable(true), Browsable(true), Description("Gets or sets the current position of the progress bar."), Category("Behavior"), DefaultValue(0)]
        public int Value
        {
            get { return m_ProgressBar.Value; }
            set
            {
                m_ProgressBar.Value = value;
                //this.Invalidate();
                //this.Update();
            }
        }

        /// <summary>
        /// Gets or sets the amount by which a call to the PerformStep method increases the current position of the progress bar.
        /// </summary>
        [DevCoBrowsable(true), Browsable(true), Description("Gets or sets the amount by which a call to the PerformStep method increases the current position of the progress bar."), Category("Behavior"), DefaultValue(1)]
        public int Step
        {
            get
            {
                return m_ProgressBar.Step;
            }
            set
            {
                m_ProgressBar.Step = value;
            }
        }

        /// <summary>
        /// Advances the current position of the progress bar by the amount of the Step property.
        /// </summary>
        public void PerformStep()
        {
            this.Value += m_ProgressBar.Step;
        }

        /// <summary>
        /// Advances the current position of the progress bar by the specified amount.
        /// </summary>
        /// <param name="value">The amount by which to increment the progress bar's current position. </param>
        public void Increment(int value)
        {
            this.Value += value;
        }

        /// <summary>
        /// Gets or sets the color of the progress chunk.
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), Category("Appearance"), Description("Gets or sets the color of the progress chunk.")]
        public Color ChunkColor
        {
            get
            {
                return m_ProgressBar.ChunkColor;
            }
            set
            {
                m_ProgressBar.ChunkColor = value;
                this.Invalidate();
            }
        }
        /// <summary>
        /// Gets whether ChunkColor property should be serialized.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeChunkColor()
        {
            return (!m_ProgressBar.ChunkColor.IsEmpty);
        }
        /// <summary>
        /// Resets the ChunkColor property to its default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetChunkColor()
        {
            m_ProgressBar.ChunkColor = Color.Empty;
        }

        /// <summary>
        /// Gets or sets the target gradient color of the progress chunk.
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), Category("Appearance"), Description("Gets or sets the target gradient color of the progress chunk.")]
        public Color ChunkColor2
        {
            get
            {
                return m_ProgressBar.ChunkColor2;
            }
            set
            {
                m_ProgressBar.ChunkColor2 = value;
                this.Invalidate();
            }
        }
        /// <summary>
        /// Gets whether ChunkColor property should be serialized.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeChunkColor2()
        {
            return (!m_ProgressBar.ChunkColor2.IsEmpty);
        }
        /// <summary>
        /// Resets the ChunkColor property to its default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetChunkColor2()
        {
            m_ProgressBar.ChunkColor2 = Color.Empty;
        }

        /// <summary>
        /// Gets or sets the gradient angle of the progress chunk.
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), Category("Appearance"), Description("Gets or sets the gradient angle of the progress chunk."), DefaultValue(0)]
        public int ChunkGradientAngle
        {
            get
            {
                return (int)m_ProgressBar.ChunkGradientAngle;
            }
            set
            {
                m_ProgressBar.ChunkGradientAngle = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets whether the text inside the progress bar is displayed.
        /// </summary>
        [Browsable(true), Description("Gets or sets whether the text inside the progress bar is displayed."), Category("Behavior"), DefaultValue(false)]
        public bool TextVisible
        {
            get
            {
                return m_ProgressBar.TextVisible;
            }
            set
            {
                m_ProgressBar.TextVisible = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the type of progress bar used to indicate progress. The Standard style displays the progress based on Minimum, Maximum and current Value.
        /// The Marquee type is automatically moving progress bar that is used to indicate an ongoing operation for which the actual duration cannot be estimated.
        /// </summary>
        [Browsable(true), Category("Behavior"), DefaultValue(eProgressItemType.Standard), Description("Indicates type of progress bar used to indicate progress.")]
        public eProgressItemType ProgressType
        {
            get { return m_ProgressBar.ProgressType; }
            set { m_ProgressBar.ProgressType = value; }
        }

        /// <summary>
        /// Gets or sets the marquee animation speed in milliseconds.
        /// </summary>
        [Browsable(true), DefaultValue(100), Category("Behavior"), Description("Indicates marquee animation speed in milliseconds.")]
        public int MarqueeAnimationSpeed
        {
            get { return m_ProgressBar.MarqueeAnimationSpeed; }
            set { m_ProgressBar.MarqueeAnimationSpeed = value; }
        }

        /// <summary>
        /// Gets or sets the predefined color state table for progress bar. Color specified applies to items with Office 2007 style only. It does not have
        /// any effect on other styles. You can use ColorTable to indicate the state of the operation that Progress Bar is tracking. Default value is eProgressBarItemColor.Normal.
        /// </summary>
        [Browsable(true), DevCoBrowsable(false), DefaultValue(eProgressBarItemColor.Normal), Category("Appearance"), Description("Indicates predefined color of item when Office 2007 style is used.")]
        public eProgressBarItemColor ColorTable
        {
            get { return m_ProgressBar.ColorTable; }
            set
            {
                m_ProgressBar.ColorTable = value;
            }
        }

        [DefaultValue(eOrientation.Horizontal)]
        public eOrientation Orientation
        {
            get { return m_ProgressBar.Orientation; }
            set
            {
                if (m_ProgressBar.Orientation != value)
                {
                    m_ProgressBar.Orientation = value;
                    this.Invalidate();
                }
            }
        }

        /// <summary>
        /// Gets the underlying ProgressBarItem
        /// </summary>
        internal ProgressBarItem ProgressBarItem
        {
            get { return (m_ProgressBar); }
        }

        #endregion
    }
}
