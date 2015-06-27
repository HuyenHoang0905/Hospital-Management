using System;
using System.Text;
using System.Drawing;
using System.ComponentModel;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Represents color item used for color picker control. Color item can only be used as part of the color picker DotNetBar feature.
    /// </summary>
    public class ColorItem : BaseItem
    {
        #region Private Variables
        private Color m_Color = Color.Black;
        private Size m_DesiredSize = new Size(13, 13);
        private bool m_MouseOver = false;
        private eColorItemBorder m_Border = eColorItemBorder.All;
        #endregion

        #region Internal Implementation
        public ColorItem():this("","") {}
		public ColorItem(string sName):this(sName,""){}
        public ColorItem(string sName, string ItemText)
            : base(sName, ItemText)
		{
			this.IsAccessible=false;
            this.CanCustomize = false;
		}

        public ColorItem(string sName, string ItemText, Color color)
            : base(sName, ItemText)
        {
            this.IsAccessible = false;
            this.CanCustomize = false;
            m_Color = color;
        }

		public override BaseItem Copy()
		{
            ColorItem copy = new ColorItem(m_Name);
			this.CopyToItem(copy);

			return copy;
		}
		protected override void CopyToItem(BaseItem copy)
		{
            ColorItem ci = copy as ColorItem;
			base.CopyToItem(ci);
            ci.DesiredSize = this.DesiredSize;
            ci.Color = this.Color;
            ci.Border = this.Border;
		}

        public override void Paint(ItemPaintArgs p)
        {
            Rendering.BaseRenderer renderer = p.Renderer;
            if (renderer != null)
            {
                ColorItemRendererEventArgs e = new ColorItemRendererEventArgs(p.Graphics, this);
                renderer.DrawColorItem(e);
            }
            else
            {
                Rendering.ColorItemPainter painter = PainterFactory.CreateColorItemPainter(this);
                if (painter != null)
                {
                    ColorItemRendererEventArgs e = new ColorItemRendererEventArgs(p.Graphics, this);
                    painter.PaintColorItem(e);
                }
            }
        }

        public override void RecalcSize()
        {
            m_Rect.Size = m_DesiredSize;
            base.RecalcSize();
        }

        /// <summary>
        /// Gets or sets the color represented by this item. Default value is Color.Black.
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), DevCoSerialize(), Category("Appearance"), Description("Indicates the color represented by this item.")]
        public System.Drawing.Color Color
        {
            get { return m_Color; }
            set
            {
                m_Color = value;
                OnAppearanceChanged();
            }
        }
        private bool ShouldSerializeColor()
        {
            return (m_Color != Color.Black);
        }

        /// <summary>
        /// Gets or sets the size of the item when displayed. Default value is 13x13 pixels.
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), DevCoSerialize(), Category("Appearance"), Description("Indicates the size of the item when displayed.")]
        public System.Drawing.Size DesiredSize
        {
            get { return m_DesiredSize; }
            set
            {
                if (value.Width > 0 && value.Height > 0)
                {
                    m_DesiredSize = value;
                    NeedRecalcSize = true;
                    OnAppearanceChanged();
                }
            }
        }
        private bool ShouldSerializeDesiredSize()
        {
            return (m_DesiredSize.Width!=13 || m_DesiredSize.Height!=13);
        }

        /// <summary>
        /// Gets or sets border drawn around the item. Default value is eColorItemBorder.All which indicates that border is drawn
        /// on all four sides.
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), DefaultValue(eColorItemBorder.All), Description("Indicate border drawn around the item"), DevCoSerialize()]
        public eColorItemBorder Border
        {
            get { return m_Border; }
            set
            {
                m_Border = value;
                OnAppearanceChanged();
            }
        }

        /// <summary>
        /// Gets whether mouse is over the item.
        /// </summary>
        public bool IsMouseOver
        {
            get { return m_MouseOver; }
        }

        [System.ComponentModel.Browsable(false), System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public override void InternalMouseEnter()
        {
            base.InternalMouseEnter();
            SetMouseOver(true);
            BaseItem parent = this.Parent;
            while (parent != null && !(parent is ColorPickerDropDown))
                parent = parent.Parent;
            if (parent != null && parent is ColorPickerDropDown)
                ((ColorPickerDropDown)parent).InvokeColorPreview(new ColorPreviewEventArgs(this.Color, this));
        }


        [System.ComponentModel.Browsable(false), System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public override void InternalMouseLeave()
        {
            base.InternalMouseLeave();
            SetMouseOver(false);
        }

        private void SetMouseOver(bool value)
        {
            if (value != m_MouseOver)
            {
                m_MouseOver = value;
                this.Refresh();
            }
        }
        #endregion

        #region Serialization
		/// <summary>
		/// Overloaded. Serializes the item and all sub-items into the XmlElement.
		/// </summary>
		/// <param name="ThisItem">XmlElement to serialize the item to.</param>
        protected internal override void Serialize(ItemSerializationContext context)
        {
            base.Serialize(context);
            System.Xml.XmlElement xml = context.ItemXmlElement;
            ElementSerializer.Serialize(this, xml);
        }

        // <summary>
		/// Overloaded. Deserializes the Item from the XmlElement.
		/// </summary>
		/// <param name="ItemXmlSource">Source XmlElement.</param>
        public override void Deserialize(ItemSerializationContext context)
        {
            base.Deserialize(context);
            System.Xml.XmlElement xml = context.ItemXmlElement;
            ElementSerializer.Deserialize(this, xml);
        }
        #endregion
    }
}
