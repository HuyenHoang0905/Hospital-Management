using System;
using System.Text;
using System.ComponentModel;
using DevComponents.DotNetBar.Ribbon;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Represents the item that provides Quick Access Toolbar customization.
    /// </summary>
    public class QatCustomizeItem : CustomizeItem
    {
        #region Private Variables
        //private bool m_SetupComplete = false;
        private const string DefaultTooltipText = "Customize Quick Access Toolbar";
        private const int FIXED_SIZE = 14;
        #endregion

        #region Internal Implementation
        /// <summary>
		/// Creates new instance of CustomizeItem object.
		/// </summary>
        public QatCustomizeItem():base()
        {
            this.Tooltip = DefaultTooltipText;
        }

        /// <summary>
        /// Returns copy of CustomizeItem item
        /// </summary>
        public override BaseItem Copy()
        {
            QatCustomizeItem objCopy = new QatCustomizeItem();
            this.CopyToItem(objCopy);
            return objCopy;
        }

        public override void Paint(ItemPaintArgs p)
        {
            Rendering.BaseRenderer renderer = p.Renderer;
            if (renderer != null)
            {
                renderer.DrawQatCustomizeItem(new QatCustomizeItemRendererEventArgs(this, p.Graphics));
                return;
            }
            else
            {
                Rendering.QatCustomizeItemPainter painter = PainterFactory.CreateQatCustomizeItemPainter(this);
                if (painter != null)
                {
                    painter.Paint(new QatCustomizeItemRendererEventArgs(this, p.Graphics));
                    return;
                }
            }

            base.Paint(p);
        }

        private bool QatContainsItem(RibbonControl rc, string itemName)
        {
            if (rc.QuickToolbarItems.Contains(itemName))
                return true;

            if(this.Parent is QatOverflowItem && this.Parent.SubItems.Contains(itemName))
                return true;
            
            return false;
        }

        protected override void SetupCustomizeItem()
        {
            this.SubItems.Clear();

            // Add customize items...
            string qatCustomize="&Customize Quick Access Toolbar...";

            RibbonControl rc = GetRibbonControl();
            if (rc!=null)
                qatCustomize = rc.SystemText.QatCustomizeText;

            string s = "<b>Customize Quick Access Toolbar</b>";
            if (rc != null)
                s = rc.SystemText.QatCustomizeMenuLabel;
            LabelItem label = new LabelItem(RibbonControl.SysQatCustomizeLabelName, s);
            label.PaddingBottom = 2;
            label.PaddingTop = 2;
            label.PaddingLeft = 12;
            label.BorderSide = eBorderSide.Bottom;
            label.BorderType = eBorderType.SingleLine;
            label.CanCustomize = false;
            if (GlobalManager.Renderer is Office2007Renderer)
            {
                label.BackColor = ((Office2007Renderer)GlobalManager.Renderer).ColorTable.QuickAccessToolbar.QatCustomizeMenuLabelBackground;
                label.ForeColor = ((Office2007Renderer)GlobalManager.Renderer).ColorTable.QuickAccessToolbar.QatCustomizeMenuLabelText;
            }
            this.SubItems.Add(label);

            bool beginGroup = false;
            if (rc.QatFrequentCommands.Count > 0)
            {
                beginGroup = true;
                foreach (BaseItem qatFC in rc.QatFrequentCommands)
                {
                    if (qatFC.Text.Length > 0)
                    {
                        ButtonItem bf = new ButtonItem(RibbonControl.SysFrequentlyQatNamePart + qatFC.Name, qatFC.Text);
                        if (QatContainsItem(rc, qatFC.Name))
                            bf.Checked = true;
                        bf.CanCustomize = false;
                        this.SubItems.Add(bf);
                        bf.Click += new EventHandler(AddFrequentCommandToQat);
                        bf.Tag = qatFC.Name;
                    }
                }
            }

            ButtonItem item = new ButtonItem(RibbonControl.SysQatCustomizeItemName, qatCustomize);
            item.BeginGroup = beginGroup;
            item.CanCustomize = false;
            this.SubItems.Add(item);
            item.Click += new EventHandler(QatCustomizeItemClick);

            if (rc != null && rc.EnableQatPlacement)
            {
                ButtonItem b = new ButtonItem(RibbonControl.SysQatPlaceItemName);
                b.CanCustomize = false;
                if (rc.QatPositionedBelowRibbon)
                    b.Text = rc.SystemText.QatPlaceAboveRibbonText;
                else
                    b.Text = rc.SystemText.QatPlaceBelowRibbonText;
                b.Click += new EventHandler(QuickAccessToolbarChangePlacement);
                this.SubItems.Add(b);
            }

            if (rc != null)
            {
                ButtonItem b = null;
                if (rc.Expanded)
                    b = new ButtonItem(RibbonControl.SysMinimizeRibbon, rc.SystemText.MinimizeRibbonText);
                else
                    b = new ButtonItem(RibbonControl.SysMaximizeRibbon, rc.SystemText.MaximizeRibbonText);
                b.CanCustomize = false;
                b.BeginGroup = true;
                b.Click += new EventHandler(ToggleRibbonExpand);
                this.SubItems.Add(b);
            }
        }

        private void AddFrequentCommandToQat(object sender, EventArgs e)
        {
            RibbonControl rc = GetRibbonControl();
            if (rc==null) return;
            CollapseAll(this);
            rc.RibbonStrip.ClosePopups();
            ButtonItem b = sender as ButtonItem;
            if (b != null && b.Tag is string && b.Tag.ToString().Length > 0)
            {
                if (b.Checked)
                {
                    // Remove from QAT
                    rc.RemoveItemFromQuickAccessToolbar(rc.QuickToolbarItems[b.Tag.ToString()]);
                }
                else
                {
                    // Add to QAT
                    rc.AddItemToQuickAccessToolbar(rc.QatFrequentCommands[b.Tag.ToString()]);
                }
            }
        }

        private void ToggleRibbonExpand(object sender, EventArgs e)
        {
            CollapseAll(this);
            ButtonItem b = sender as ButtonItem;
            if(b==null) return;
            RibbonControl rc = GetRibbonControl();
            if (rc == null) return;
            rc.RibbonStrip.ClosePopups();
            if (b.Name == RibbonControl.SysMinimizeRibbon)
                rc.Expanded = false;
            else if (b.Name == RibbonControl.SysMaximizeRibbon)
                rc.Expanded = true;
        }

        private void QuickAccessToolbarChangePlacement(object sender, EventArgs e)
        {
            CollapseAll(this);
            RibbonControl rc = GetRibbonControl();
            if (rc != null)
            {
                rc.RibbonStrip.ClosePopups();
                rc.QuickAccessToolbarChangePlacement();
            }
        }

        private void QatCustomizeItemClick(object sender, EventArgs e)
        {
            RibbonControl rc = GetRibbonControl();
            CollapseAll(this);
            if (rc != null)
            {
                rc.RibbonStrip.ClosePopups();
                rc.ShowQatCustomizeDialog();
            }
        }

        private RibbonControl GetRibbonControl()
        {
            RibbonStrip strip = this.ContainerControl as RibbonStrip;
            if (strip == null)
            {
                BaseItem parent = this;
                while (parent != null && strip == null)
                {
                    parent = parent.Parent;
                    if (parent != null)
                    {
                        if (parent.ContainerControl is QatToolbar)
                        {
                            QatToolbar qat = parent.ContainerControl as QatToolbar;
                            if (qat.Parent is RibbonControl)
                            {
                                strip = ((RibbonControl)qat.Parent).RibbonStrip;
                                break;
                            }
                        }
                        else
                            strip = parent.ContainerControl as RibbonStrip;
                    }
                }
            }
            RibbonControl rc = null;
            if (strip != null) rc = strip.Parent as RibbonControl;

            Ribbon.QatToolbar qatToolbar = this.ContainerControl as Ribbon.QatToolbar;
            if (qatToolbar != null && qatToolbar.Parent is RibbonControl)
                return qatToolbar.Parent as RibbonControl;

            return rc;
        }

        protected override void ClearCustomizeItem()
        {
            // Nothing to do leave all items as they are
        }

        protected override void LoadResources() {}

        //protected override void SetCustomTooltip(string text) {}

        /// <summary>
        /// Gets or sets whether Customize menu item is visible.
        /// </summary>
        [Browsable(false), DevCoBrowsable(false), Category("Behavior"), Description("Indicates whether Customize menu item is visible."), DefaultValue(true), EditorBrowsable(EditorBrowsableState.Never)]
        public override bool CustomizeItemVisible
        {
            get { return base.CustomizeItemVisible; }
            set { base.CustomizeItemVisible = value; }
        }

        /// <summary>
        /// Gets/Sets informational text (tooltip) for the item.
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), DefaultValue(QatCustomizeItem.DefaultTooltipText), System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("Indicates the text that is displayed when mouse hovers over the item."), Localizable(true)]
        public override string Tooltip
        {
            get { return base.Tooltip; }
            set { base.Tooltip = value; }
        }

        protected internal override void OnExpandChange()
        {
            this.PopupSide = ePopupSide.Bottom;
            base.OnExpandChange();
        }

        /// <summary>
		/// Overridden. Recalculates the size of the item.
		/// </summary>
        public override void RecalcSize()
        {
            if (this.SuspendLayout)
                return;

            if (m_Orientation == eOrientation.Vertical)
            {
                // Take suggested width
                m_Rect.Height = FIXED_SIZE;
                m_Rect.Width = 22;
            }
            else
            {
                // Take suggested height
                m_Rect.Width = FIXED_SIZE;
                m_Rect.Height = 22;
            }

            SetCustomTooltip(GetTooltipText());
        }

        /// <summary>
        /// Gets localized tooltip text for this instance of the item.
        /// </summary>
        /// <returns>Tooltip text.</returns>
        protected override string GetTooltipText()
        {
            string s = "";
            using (LocalizationManager lm = new LocalizationManager(this.GetOwner() as IOwnerLocalize))
                s = lm.GetLocalizedString(LocalizationKeys.QatCustomizeTooltip);
            if (s == "") s = DefaultTooltipText;
            return s;
        }

        /// <summary>
        /// Called when mouse hovers over the customize item.
        /// </summary>
        protected override void MouseHoverCustomize()
        {
        }
        #endregion
    }
}
