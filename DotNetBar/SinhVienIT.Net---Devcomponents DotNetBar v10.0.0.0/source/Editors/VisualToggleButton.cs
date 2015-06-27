#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing;

namespace DevComponents.Editors
{
    public class VisualToggleButton : VisualItem
    {
        #region Private Variables

        #endregion

        #region Events
        /// <summary>
        /// Occurs when Checked property has changed.
        /// </summary>
        public event EventHandler CheckedChanged;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the VisualToggleButton class.
        /// </summary>
        public VisualToggleButton()
        {
            this.Focusable = true;
        }

        #endregion

        #region Internal Implementation
        protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left && GetIsEnabled())
            {
                ToggleChecked();
                IsMouseDown = true;
            }
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
                IsMouseDown = false;
            base.OnMouseUp(e);
        }

        protected override void OnMouseEnter()
        {
            this.IsMouseOver = true;
            base.OnMouseEnter();
        }

        protected override void OnMouseLeave()
        {
            this.IsMouseOver = false;
            base.OnMouseLeave();
        }

        protected override void OnKeyDown(System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.Space && GetIsEnabled())
            {
                ToggleChecked();
                e.Handled = true;
            }
            base.OnKeyDown(e);
        }

        private void ToggleChecked()
        {
            this.Checked = !this.Checked;
        }

        /// <summary>
        /// Raises the CheckedChanged event.
        /// </summary>
        protected virtual void OnCheckedChanged(EventArgs e)
        {
            if (CheckedChanged != null)
                CheckedChanged(this, e);
        }

        private bool _Checked = false;
        /// <summary>
        /// Gets or sets whether item is checked.
        /// </summary>
        [DefaultValue(false)]
        public virtual bool Checked
        {
            get { return _Checked; }
            set
            {
                if (_Checked != value)
                {
                    _Checked = value;
                    InvalidateRender();
                    OnCheckedChanged(new EventArgs());
                }
            }
        }

        private bool _IsMouseDown;
        /// <summary>
        /// Gets whether left mouse button is pressed over the item.
        /// </summary>
        [Browsable(false)]
        public bool IsMouseDown
        {
            get { return _IsMouseDown; }
            internal set
            {
                if (value != _IsMouseDown)
                {
                    _IsMouseDown = value;
                    this.InvalidateRender();
                }
            }
        }

        private bool _IsMouseOver = false;
        /// <summary>
        /// Gets whether mouse is over the item.
        /// </summary>
        [Browsable(false)]
        public bool IsMouseOver
        {
            get { return _IsMouseOver; }
            internal set
            {
                if (value != _IsMouseOver)
                {
                    _IsMouseOver = value;
                    this.InvalidateRender();
                }
            }
        }
        #endregion

    }
}
#endif

