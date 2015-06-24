#if FRAMEWORK20
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar
{
    public class SuperTabCloseBox : BaseItem
    {
        #region Private variables

        private SuperTabControlBox _ControlBox;
        private Size _ItemSize = new Size(16, 16);

        private bool _IsMouseOver;
        private bool _IsMouseDown;

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="controlBox">Associated SuperTabControlBox</param>
        public SuperTabCloseBox(SuperTabControlBox controlBox)
        {
            _ControlBox = controlBox;

            Visible = false;
        }

        #region Public override designer hiding

        [Browsable(false)]
        public override bool BeginGroup
        {
            get { return base.BeginGroup; }
            set { base.BeginGroup = value; }
        }

        [Browsable(false)]
        public override bool AutoCollapseOnClick
        {
            get { return base.AutoCollapseOnClick; }
            set { base.AutoCollapseOnClick = value; }
        }

        [Browsable(false)]
        public override bool CanCustomize
        {
            get { return base.CanCustomize; }
            set { base.CanCustomize = value; }
        }

        [Browsable(false)]
        public override string Category
        {
            get { return base.Category; }
            set { base.Category = value; }
        }

        [Browsable(false)]
        public override bool ClickAutoRepeat
        {
            get { return base.ClickAutoRepeat; }
            set { base.ClickAutoRepeat = value; }
        }

        [Browsable(false)]
        public override int ClickRepeatInterval
        {
            get { return base.ClickRepeatInterval; }
            set { base.ClickRepeatInterval = value; }
        }

        [Browsable(false)]
        public override Cursor Cursor
        {
            get { return base.Cursor; }
            set { base.Cursor = value; }
        }

        [Browsable(false)]
        public override bool Enabled
        {
            get { return base.Enabled; }
            set { base.Enabled = value; }
        }

        [Browsable(false)]
        public override bool Stretch
        {
            get { return base.Stretch; }
            set { base.Stretch = value; }
        }

        [Browsable(false)]
        public override string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        [Browsable(false)]
        public override bool ThemeAware
        {
            get { return base.ThemeAware; }
            set { base.ThemeAware = value; }
        }

        [Browsable(false)]
        public override string Tooltip
        {
            get { return base.Tooltip; }
            set { base.Tooltip = value; }
        }

        [Browsable(false)]
        public override eItemAlignment ItemAlignment
        {
            get { return base.ItemAlignment; }
            set { base.ItemAlignment = value; }
        }

        [Browsable(false)]
        public override string KeyTips
        {
            get { return base.KeyTips; }
            set { base.KeyTips = value; }
        }

        [Browsable(false)]
        public override ShortcutsCollection Shortcuts
        {
            get { return base.Shortcuts; }
            set { base.Shortcuts = value; }
        }

        [Browsable(false)]
        public override string Description
        {
            get { return base.Description; }
            set { base.Description = value; }
        }

        [Browsable(false)]
        public override bool GlobalItem
        {
            get { return base.GlobalItem; }
            set { base.GlobalItem = value; }
        }

        [Browsable(false)]
        public override bool ShowSubItems
        {
            get { return base.ShowSubItems; }
            set { base.ShowSubItems = value; }
        }

        [Browsable(false)]
        public override string GlobalName
        {
            get { return base.GlobalName; }
            set { base.GlobalName = value; }
        }

        [Browsable(false)]
        public override Command Command
        {
            get { return base.Command; }
            set { base.Command = value; }
        }

        [Browsable(false)]
        public override object CommandParameter
        {
            get { return base.CommandParameter; }
            set { base.CommandParameter = value; }
        }

        #endregion

        #region Public properties

        #region Visible

        /// <summary>
        /// Gets or sets the CloseBox Visible state
        /// </summary>
        [Browsable(true), DefaultValue(false), Category("Behavior")]
        [Description("Indicates CloseBox Visible state.")]
        public override bool Visible
        {
            get { return base.Visible; }

            set
            {
                if (base.Visible != value)
                {
                    base.Visible = value;

                    MyRefresh();
                }
            }
        }

        #endregion

        #endregion

        #region Internal properties

        #region IsMouseDown

        /// <summary>
        /// Gets whether the mouse is down
        /// </summary>
        internal bool IsMouseDown
        {
            get { return (_IsMouseDown); }
        }

        #endregion

        #region IsMouseOver

        /// <summary>
        /// Gets whether the mouse is over the CloseBox
        /// </summary>
        internal bool IsMouseOver
        {
            get { return (_IsMouseOver); }
        }

        #endregion

        #endregion

        #region RecalcSize

        /// <summary>
        /// Performs RecalcSize processing
        /// </summary>
        public override void RecalcSize()
        {
            base.RecalcSize();

            WidthInternal = _ItemSize.Width;
            HeightInternal = _ItemSize.Height;
        }

        #endregion

        #region MyRefresh

        /// <summary>
        /// Refreshes the CloseBox
        /// </summary>
        private void MyRefresh()
        {
            if (_ControlBox.TabDisplay != null)
            {
                SuperTabStripItem tsi = _ControlBox.TabDisplay.TabStripItem;

                if (tsi != null)
                {
                    _ControlBox.NeedRecalcSize = true;
                    _ControlBox.RecalcSize();

                    tsi.NeedRecalcSize = true;
                    tsi.Refresh();
                }
            }
        }

        #endregion

        #region Paint

        /// <summary>
        /// Performs control Paint processing
        /// </summary>
        /// <param name="p"></param>
        public override void Paint(ItemPaintArgs p)
        {
            Graphics g = p.Graphics;

            SuperTabColorTable sct = _ControlBox.TabDisplay.GetColorTable();

            Color imageColor = sct.ControlBoxDefault.Image;

            if (_IsMouseOver == true)
            {
                Rectangle r = Bounds;

                r.Width--;
                r.Height--;

                if (_IsMouseDown == true && _IsMouseOver == true)
                {
                    imageColor = sct.ControlBoxPressed.Image;

                    using (Brush br = new SolidBrush(sct.ControlBoxPressed.Background))
                        g.FillRectangle(br, r);

                    using (Pen pen = new Pen(sct.ControlBoxPressed.Border))
                        g.DrawRectangle(pen, r);
                }
                else
                {
                    imageColor = sct.ControlBoxMouseOver.Image;

                    using (Brush br = new SolidBrush(sct.ControlBoxMouseOver.Background))
                        g.FillRectangle(br, r);

                    using (Pen pen = new Pen(sct.ControlBoxMouseOver.Border))
                        g.DrawRectangle(pen, r);
                }
            }

            g.DrawImageUnscaled(
                _ControlBox.TabDisplay.GetCloseButton(g, imageColor), Bounds);
        }

        #endregion

        #region Mouse support

        #region InternalMouseEnter

        /// <summary>
        /// InternalMouseEnter
        /// </summary>
        public override void InternalMouseEnter()
        {
            base.InternalMouseEnter();

            _IsMouseOver = true;

            Refresh();
        }

        #endregion

        #region InternalMouseLeave

        /// <summary>
        /// InternalMouseLeave
        /// </summary>
        public override void InternalMouseLeave()
        {
            base.InternalMouseLeave();

            _IsMouseOver = false;

            Refresh();
        }

        #endregion

        #region InternalMouseDown

        /// <summary>
        /// InternalMouseDown
        /// </summary>
        /// <param name="objArg"></param>
        public override void InternalMouseDown(MouseEventArgs objArg)
        {
            base.InternalMouseDown(objArg);

            if (DesignMode == false && objArg.Button == MouseButtons.Left)
                _IsMouseDown = true;

            Refresh();
        }

        #endregion

        #region InternalMouseUp

        /// <summary>
        /// InternalMouseUp
        /// </summary>
        /// <param name="objArg"></param>
        public override void InternalMouseUp(MouseEventArgs objArg)
        {
            base.InternalMouseUp(objArg);

            _IsMouseDown = false;

            if (_IsMouseOver == true)
            {
                SuperTabItem tab = _ControlBox.TabDisplay.SelectedTab;

                if (tab != null)
                    tab.Close();
            }

            Refresh();
        }

        #endregion

        #endregion

        #region Copy object support

        /// <summary>
        /// Returns copy of the item
        /// </summary>
        public override BaseItem Copy()
        {
            SuperTabCloseBox objCopy = new SuperTabCloseBox(_ControlBox);
            CopyToItem(objCopy);

            return (objCopy);
        }

        protected override void CopyToItem(BaseItem copy)
        {
            SuperTabCloseBox objCopy = copy as SuperTabCloseBox;
            base.CopyToItem(objCopy);
        }

        #endregion
    }
}
#endif