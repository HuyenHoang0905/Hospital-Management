#if FRAMEWORK20
using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Represents Panel for the SuperTabControl
    /// </summary>
    ///
    [ToolboxItem(false)]
    [Designer("DevComponents.DotNetBar.Design.SuperTabControlPanelDesigner, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
    public class SuperTabControlPanel : PanelControl
    {
        #region Events

        /// <summary>
        /// Occurs when the tab colors have changed
        /// </summary>
        [Description("Occurs when the panel colors have changed.")]
        public event EventHandler<EventArgs> PanelColorChanged;

        #endregion

        #region Constants

        private const string InfoText =
            "Drop controls on this panel to associate them with currently selected tab.";

        #endregion

        #region Private Variables

        private SuperTabItem _Tab;
        private ElementStyle _PanelStyle;
        private SuperTabPanelColorTable _PanelColor;

        #endregion

        public SuperTabControlPanel()
        {
            _PanelStyle = new ElementStyle();
            _PanelColor = new SuperTabPanelColorTable();

            _PanelColor.ColorTableChanged += PanelColor_ColorTableChanged;
        }

        #region Public property hiding

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override ElementStyle Style
        {
            get { return base.Style; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override ElementStyle StyleMouseDown
        {
            get { return base.Style; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override ElementStyle StyleMouseOver
        {
            get { return base.Style; }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
        public override ImageLayout BackgroundImageLayout
        {
            get
            {
                return base.BackgroundImageLayout;
            }
            set
            {
                base.BackgroundImageLayout = value;
            }
        }
        #endregion

        #region Public properties

        #region DockStyle

        /// <summary>
        /// Gets or sets which edge of the parent container a control is docked to.
        /// </summary>
        [Browsable(false), DefaultValue(DockStyle.None)]
        public override DockStyle Dock
        {
            get { return (base.Dock); }
            set { base.Dock = value; }
        }

        #endregion

        #region PanelColor

        /// <summary>
        /// Gets or sets user specified tab panel display colors
        /// </summary>
        [Browsable(true), Category("Style")]
        [Description("Contains user specified tab panel display colors.")]
        public SuperTabPanelColorTable PanelColor
        {
            get { return (_PanelColor); }

            set
            {
                if (_PanelColor.Equals(value) == false)
                {
                    if (_PanelColor != null)
                        _PanelColor.ColorTableChanged -= PanelColor_ColorTableChanged;

                    _PanelColor = value;

                    if (value != null)
                        _PanelColor.ColorTableChanged += PanelColor_ColorTableChanged;

                    OnPanelColorChanged();

                    Refresh();
                }
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializePanelColor()
        {
            return (_PanelColor.IsEmpty == false);
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetPanelColor()
        {
            PanelColor = new SuperTabPanelColorTable();
        }

        #endregion

        #region PanelStyle

        /// <summary>
        /// Gets or sets the Panel ElementStyle
        /// </summary>
        [Browsable(false)]
        public ElementStyle PanelStyle
        {
            get { return (_PanelStyle); }

            set
            {
                _PanelStyle = value;

                Refresh();
            }
        }

        #endregion

        #region SuperTabItem

        /// <summary>
        /// Gets or sets TabItem that this panel is attached to.
        /// </summary>
        [Browsable(false), DefaultValue(null)]
        public SuperTabItem TabItem
        {
            get { return (_Tab); }
            set { _Tab = value; }
        }

        #endregion

        #region BackgroundImage
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public override Image BackgroundImage
        {
            get { return base.BackgroundImage; }
            set { base.BackgroundImage = value; }
        }
        protected override void OnBackgroundImageChanged(EventArgs e)
        {
            OnPanelColorChanged();
            base.OnBackgroundImageChanged(e);
        }
        private eStyleBackgroundImage _BackgroundImagePosition = eStyleBackgroundImage.Stretch;
        /// <summary>
        /// Specifies background image position when container is larger than image.
        /// </summary>
        [Browsable(true), Category("Appearance"), DefaultValue(eStyleBackgroundImage.Stretch), Description("Specifies background image position when container is larger than image.")]
        public eStyleBackgroundImage BackgroundImagePosition
        {
            get { return _BackgroundImagePosition; }
            set
            {
                if (_BackgroundImagePosition != value)
                {
                    _BackgroundImagePosition = value;
                    OnPanelColorChanged();
                }
            }
        }

        #endregion

        #endregion

        #region PanelColor_ColorTableChanged

        /// <summary>
        /// _PanelColor_ColorTableChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PanelColor_ColorTableChanged(object sender, EventArgs e)
        {
            OnPanelColorChanged();
        }

        #endregion

        #region OnPanelColorChanged

        /// <summary>
        /// Processes ColorTable changes
        /// </summary>
        protected void OnPanelColorChanged()
        {
            if (PanelColorChanged != null)
                PanelColorChanged(this, EventArgs.Empty);

            Refresh();
        }

        #endregion

        #region GetStyle

        /// <summary>
        /// GetStyle
        /// </summary>
        /// <returns></returns>
        protected override ElementStyle GetStyle()
        {
            if (_PanelStyle != null)
                return (_PanelStyle);

            return (Style);
        }

        #endregion

        #region OnPaint

        /// <summary>
        /// Provides OnPaint support
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (ClientRectangle.Width > 0 && ClientRectangle.Height > 0)
            {
                SuperTabControl tab = Parent as SuperTabControl;

                if (tab != null && tab.SelectedPanel == this)
                {
                    base.OnPaint(e);

                    if (this.DesignMode && this.Controls.Count == 0 && this.Text == "")
                    {
                        Rectangle r = this.ClientRectangle;
                        r.Inflate(-2, -2);

                        const eTextFormat sf = eTextFormat.Default | eTextFormat.VerticalCenter |
                                               eTextFormat.HorizontalCenter | eTextFormat.EndEllipsis |
                                               eTextFormat.WordBreak;

                        using (Font font = new Font(this.Font, FontStyle.Bold))
                        {
                            TextDrawing.DrawString(e.Graphics, InfoText,
                                                   font, ControlPaint.Dark(this.Style.BackColor), r, sf);

                        }
                    }
                }
            }
        }

        #endregion
    }
}
#endif