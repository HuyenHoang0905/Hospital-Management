using System;
using System.Text;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DevComponents.DotNetBar
{
    [ToolboxItem(false), EditorBrowsable(EditorBrowsableState.Never)]
    public class PanelExTitle:PanelEx
    {
        #region Private Variables, Events & Constructor
        private ItemContainer m_ItemContainer = null;
        private ButtonItem m_ExpandChangeButton = null;
        private int m_ButtonMargin = 3;
        private eTitleButtonAlignment m_ButtonAlignment = eTitleButtonAlignment.Right;
        private bool m_DrawText = true;
        /// <summary>
        /// Occurs when Expanded/Collapse button is clicked.
        /// </summary>
        public event EventHandler ExpandedClick;

        public PanelExTitle()
        {
            m_ExpandChangeButton = new ButtonItem("close");
            m_ExpandChangeButton.Style = eDotNetBarStyle.Office2003;
            m_ExpandChangeButton.Image = BarFunctions.LoadBitmap("SystemImages.CollapseTitle.png");
            m_ExpandChangeButton.Displayed = true;
            m_ExpandChangeButton.Click += new EventHandler(m_ExpandChangeButton_Click);

            m_ItemContainer = new ItemContainer();
            m_ItemContainer.ContainerControl = this;
            m_ItemContainer.Style = eDotNetBarStyle.Office2003;
            m_ItemContainer.SubItems.Add(m_ExpandChangeButton);
            base.MarkupUsesStyleAlignment = true;
        }
        #endregion

        #region ButtonItem integration
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            m_ItemContainer.InternalMouseEnter();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            m_ItemContainer.InternalMouseLeave();
        }

        protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseDown(e);
            m_ItemContainer.InternalMouseDown(e);
        }

        protected override void OnMouseUp(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseUp(e);
            m_ItemContainer.InternalMouseUp(e);
        }

        protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseMove(e);
            m_ItemContainer.InternalMouseMove(e);
        }

        protected override void OnClick(EventArgs e)
        {
            Point p = this.PointToClient(Control.MousePosition);
            m_ItemContainer.InternalClick(Control.MouseButtons, new Point(p.X, p.Y));
            base.OnClick(e);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            UpdateButtonPosition();
        }

        /// <summary>
        /// Updates position of the title bar button to reflect any changes to the button that influence size of the button.
        /// </summary>
        public void UpdateButtonPosition()
        {
            m_ExpandChangeButton.FixedSize = Size.Empty;

            m_ItemContainer.NeedRecalcSize = true;
            m_ItemContainer.RecalcSize();

            if (m_ItemContainer.HeightInternal >= this.Height && this.Height>8)
            {
                m_ExpandChangeButton.FixedSize = new Size(this.Height-2, this.Height-2);
                m_ItemContainer.RecalcSize();
            }

            if (m_ButtonAlignment == eTitleButtonAlignment.Right)
            {
                m_ItemContainer.LeftInternal = this.Width - m_ItemContainer.WidthInternal - m_ButtonMargin;
                m_ItemContainer.TopInternal = (this.Height - m_ItemContainer.HeightInternal) / 2;
            }
            else
            {
                m_ItemContainer.LeftInternal = m_ButtonMargin;
                m_ItemContainer.TopInternal = (this.Height - m_ItemContainer.HeightInternal) / 2;
            }
            m_ItemContainer.RecalcSize();

            RefreshTextClientRectangle();
        }

        protected override void RefreshTextClientRectangle()
        {
            Rectangle r = this.DisplayRectangle;
            r.Width -= (m_ItemContainer.WidthInternal + m_ButtonMargin);
            if (m_ButtonAlignment == eTitleButtonAlignment.Left)
            {
                r.X += (m_ItemContainer.WidthInternal + m_ButtonMargin);
            }
            if (!m_DrawText)
                r.Width = 0;
            ClientTextRectangle = r;
            ResizeMarkup();
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);
            ItemPaintArgs pa = new ItemPaintArgs(null, this, e.Graphics, this.ColorScheme);
            m_ItemContainer.Paint(pa);
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            UpdateButtonPosition();
        }
        #endregion

        #region Internal Implementation
        void m_ExpandChangeButton_Click(object sender, EventArgs e)
        {
            InvokeExpandedClick(e);
        }

        protected virtual void InvokeExpandedClick(EventArgs e)
        {
            if (ExpandedClick != null)
                ExpandedClick(this, e);
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ButtonItem ExpandChangeButton
        {
            get { return m_ExpandChangeButton; }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool DrawText
        {
            get { return m_DrawText; }
            set { m_DrawText = value; }
        }

        /// <summary>
        /// Gets or sets whether expand button is visible or not. Default value is true.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Category("Expand Button"), Description("Indicates whether expand button is visible or not.")]
        public bool ExpandButtonVisible
        {
            get { return m_ExpandChangeButton.Visible; }
            set
            { 
                m_ExpandChangeButton.Visible = value;
                this.UpdateButtonPosition();
                this.Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets alignment of the button.
        /// </summary>
        [Browsable(false), DefaultValue(eTitleButtonAlignment.Right), Category("Expand Button"), Description("Indicates the alignment of button."), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public eTitleButtonAlignment ButtonAlignment
        {
            get { return m_ButtonAlignment; }
            set
            {
                m_ButtonAlignment = value;
                this.UpdateButtonPosition();
                this.Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets whether text markup if it occupies less space than control provides uses the Style Alignment and LineAlignment properties to align the markup inside of the control. Default value is true.
        /// </summary>
        [Browsable(true), Category("Appearance"), DefaultValue(true), Description("Indicates whether text markup if it occupies less space than control provides uses the Style Alignment and LineAlignment properties to align the markup inside of the control.")]
        public override bool MarkupUsesStyleAlignment
        {
            get
            {
                return base.MarkupUsesStyleAlignment;
            }
            set
            {
                base.MarkupUsesStyleAlignment = value;
            }
        }
        #endregion
    }
}
