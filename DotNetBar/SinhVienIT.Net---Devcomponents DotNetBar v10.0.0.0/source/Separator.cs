using System;
using System.Text;
using System.Drawing;
using System.ComponentModel;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Represents the visual separator line that is displayed between items.
    /// </summary>
    [ToolboxItem(false)]
    public class Separator : BaseItem
    {
        #region Private Variables

        #endregion

        #region Constructors
        /// <summary>
        /// Creates new instance of Separator.
		/// </summary>
		public Separator():this("") {}
		/// <summary>
        /// Creates new instance of Separator and assigns the name to it.
		/// </summary>
		/// <param name="sItemName">Item name.</param>
		public Separator(string sItemName):base(sItemName) 
        {
        }
        #endregion

        #region Internal Implementation
        public override void Paint(ItemPaintArgs p)
        {
            Rectangle bounds = this.Bounds;
            if (bounds.Width < 1 || bounds.Height < 1) return;

            Color colorLine = p.Colors.ItemSeparator;
            Color colorShade = p.Colors.ItemSeparatorShade;
            Graphics g=p.Graphics;
            Rectangle r;
            if (this.Orientation == eOrientation.Vertical)
            {
                r=new Rectangle(bounds.X + _Padding.Left, bounds.Y + _Padding.Top + bounds.Height / 2, bounds.Width - _Padding.Right, 1);
                if (!colorLine.IsEmpty)
                    DisplayHelp.DrawLine(g, r.X, r.Y, r.Right, r.Y, colorLine, 1);
            }
            else
            {
                r = new Rectangle(bounds.X + _Padding.Left + bounds.Width / 2, 
                    bounds.Y + _Padding.Top + (bounds.Height - _FixedSize.Height)/2, 1, _FixedSize.Height);
                if (!colorLine.IsEmpty)
                    DisplayHelp.DrawLine(g, r.X, r.Y, r.X, r.Bottom, colorLine, 1);
            }

            if (!colorShade.IsEmpty)
            {
                r.Inflate(1, 1);
                DisplayHelp.DrawRectangle(g, colorShade, r);
            }
        
        }

        private Padding _Padding = new Padding(2, 2, 2, 2);
        /// <summary>
        /// Gets or sets separator padding.
        /// </summary>
        [Browsable(true), Category("Appearance"), Description("Gets or sets separator padding."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Padding Padding
        {
            get { return _Padding; }
        }
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializePadding()
        {
            return _Padding.Bottom != 2 || _Padding.Top != 2 || _Padding.Left != 2 || _Padding.Right != 2;
        }
        [EditorBrowsable(EditorBrowsableState.Never)]
        private void ResetPadding()
        {
            _Padding = new Padding(2, 2, 2, 2);
        }

        private Size _FixedSize = new Size(3, 16);
        /// <summary>
        /// Gets or sets the size of separator. Size specified is for separator in Vertical orientation. If orientation changes then the size will be internally switched to respect proper orientation.
        /// </summary>
        public Size FixedSize
        {
            get { return _FixedSize; }
            set
            {
                _FixedSize = value;
                NeedRecalcSize = true;
                OnAppearanceChanged();
            }
        }

        public override void RecalcSize()
        {
            if (this.Orientation == eOrientation.Horizontal)
                m_Rect.Size = new Size(_FixedSize.Width + _Padding.Horizontal, _FixedSize.Height + _Padding.Vertical);
            else
                m_Rect.Size = new Size(_FixedSize.Height + Padding.Horizontal, _FixedSize.Width + _Padding.Vertical);
            base.RecalcSize();
        }

        /// <summary>
		/// Returns copy of the item.
		/// </summary>
		public override BaseItem Copy()
		{
            Separator objCopy = new Separator(m_Name);
			this.CopyToItem(objCopy);
			return objCopy;
		}
		/// <summary>
		/// Copies the ButtonItem specific properties to new instance of the item.
		/// </summary>
		/// <param name="copy">New ButtonItem instance.</param>
        protected override void CopyToItem(BaseItem copy)
        {
            Separator objCopy = copy as Separator;
            base.CopyToItem(objCopy);
            objCopy.FixedSize = _FixedSize;
            objCopy.Padding.Left = _Padding.Left;
            objCopy.Padding.Right = _Padding.Right;
            objCopy.Padding.Top = _Padding.Top;
            objCopy.Padding.Bottom = _Padding.Bottom;
        }
        #endregion


    }
}
