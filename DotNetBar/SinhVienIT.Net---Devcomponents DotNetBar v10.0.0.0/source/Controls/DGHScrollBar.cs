#if FRAMEWORK20
using System;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using DevComponents.DotNetBar.ScrollBar;
using System.ComponentModel;
using DevComponents.DotNetBar.Rendering;
using System.Runtime.InteropServices;
using System.Reflection;

namespace DevComponents.DotNetBar.Controls
{
    internal class DGHScrollBar : HScrollBar, ScrollBarReplacement.IScrollBarExtender
    {
        #region Private Variables
        private ScrollBarReplacement m_ScrollBarImpl = null;
        private bool m_EnableStyling = true;
        #endregion

        #region Constructor
        public DGHScrollBar()
        {
            m_ScrollBarImpl = new ScrollBarReplacement(this);

            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.Opaque, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(DisplayHelp.DoubleBufferFlag, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, false);
        }
        #endregion

        #region Internal Implementation
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams p = base.CreateParams;
                if (m_EnableStyling)
                {
                    p.ClassName = null;
                    p.Style = 0x2000000;
                    p.Style |= 0x44000000;
                    if (this.Visible)
                        p.Style |= 0x10000000;
                    if (!this.Enabled)
                        p.Style |= 0x8000000;
                    if (this.RightToLeft == RightToLeft.Yes)
                    {
                        p.ExStyle |= 0x2000;
                        p.ExStyle |= 0x1000;
                        p.ExStyle |= 0x4000;
                    }
                }
                return p;
            }
        }
        protected override void Dispose(bool disposing)
        {
            m_ScrollBarImpl.Dispose();
            base.Dispose(disposing);
        }

        /// <summary>
        /// Gets or sets whether custom styling (Office 2007 style) is enabled. Default value is true.
        /// </summary>
        [Browsable(true), DefaultValue(true), Category("Appearance"), Description("Indicates whether custom styling (Office 2007 style) is enabled.")]
        public bool EnableStyling
        {
            get { return m_EnableStyling; }
            set
            {
                if (m_EnableStyling != value)
                {
                    m_EnableStyling = value;
                    if (this.IsHandleCreated)
                        this.RecreateHandle();
                }
            }
        }

        ///// <summary>
        ///// Gets or sets whether scroll bar appears as application style scroll bar which usually uses darker colors. Default value is true.
        ///// </summary>
        //[Browsable(true), DefaultValue(true), Category("Appearance"), Description("Indicates whether scroll bar appears as application style scroll bar which usually uses darker colors.")]
        //public bool IsAppScrollBarStyle
        //{
        //    get { return m_ScrollBarImpl.IsAppScrollBarStyle; }
        //    set { m_ScrollBarImpl.IsAppScrollBarStyle = value; }
        //}

        protected override void OnHandleCreated(EventArgs e)
        {
            m_ScrollBarImpl.OnHandleCreated();
            base.OnHandleCreated(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (m_EnableStyling)
                m_ScrollBarImpl.OnMouseDown(e);
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (m_EnableStyling)
                m_ScrollBarImpl.OnMouseUp(e);
            base.OnMouseUp(e);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            if (m_EnableStyling)
                m_ScrollBarImpl.OnMouseEnter(e);
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            if (m_EnableStyling)
                m_ScrollBarImpl.OnMouseLeave(e);
            base.OnMouseLeave(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (m_EnableStyling)
                m_ScrollBarImpl.OnMouseMove(e);
            base.OnMouseMove(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if(m_EnableStyling)
                m_ScrollBarImpl.OnPaint(e);
            base.OnPaint(e);
        }

        protected override void NotifyInvalidate(Rectangle invalidatedArea)
        {
            if (m_EnableStyling)
                m_ScrollBarImpl.NotifyInvalidate(invalidatedArea);
            base.NotifyInvalidate(invalidatedArea);
        }

        public void UpdateScrollValues()
        {
            m_ScrollBarImpl.UpdateScrollValues();
        }

        protected override void OnParentChanged(EventArgs e)
        {
            if (this.Parent is DataGridView)
            {
                m_ScrollBarImpl.ScrollBarCore.SideBorderOnly = true;
                m_ScrollBarImpl.ScrollBarCore.HasBorder = false;
            }
            else
            {
                m_ScrollBarImpl.ScrollBarCore.SideBorderOnly = false;
                m_ScrollBarImpl.ScrollBarCore.HasBorder = true;
            }
            base.OnParentChanged(e);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            UpdateScrollValues();
        }

        public bool AppStyleScrollBar
        {
            get { return m_ScrollBarImpl.AppStyleScrollBar; }
            set { m_ScrollBarImpl.AppStyleScrollBar = value; }
        }
        #endregion

        #region IScrollBarExtender Members
        void ScrollBarReplacement.IScrollBarExtender.SetValue(int newValue, ScrollEventType type)
        {
            if (newValue != this.Value)
            {
                int v = this.Value;
                this.Value = newValue;
                ScrollEventArgs se = new ScrollEventArgs(type, v, newValue, ScrollOrientation.HorizontalScroll);
                OnScroll(se);
            }
        }
        #endregion
    }
}
#endif