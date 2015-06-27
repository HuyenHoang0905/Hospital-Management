using System;
using System.ComponentModel.Design;
using System.Drawing;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;
using DevComponents.DotNetBarKnobControl.Design.Properties;

namespace DevComponents.Instrumentation.Design
{
    public class GaugeItemCollectionEditor : CollectionEditor
    {
        #region Private variables

        private ListBox _ListBox;
        private GaugeItem _CopyItem;
        private GaugeItem _GaugeItem;
        private GaugeItem _LastGaugeItem;
        private PropertyGrid _PropertyGrid;

        private ToolStripItem _AddTextItem;
        private ToolStripItem _AddImageItem;
        private ToolStripItem _AddDigitalIndicatorItem;
        private ToolStripItem _AddMechanicalIndicatorItem;
        private ToolStripItem _AddStateIndicatorItem;

        private Button _CopyButton;
        private ToolTip _ToolTip;

        #endregion

        public GaugeItemCollectionEditor(Type type)
            : base(type)
        {
            _CopyItem = null;
        }

        #region CreateCollectionForm

        protected override CollectionForm CreateCollectionForm()
        {
            CollectionForm collectionForm = base.CreateCollectionForm();

            if (collectionForm.Controls[0] is TableLayoutPanel)
            {
                TableLayoutPanel tlpf = collectionForm.Controls["overArchingTableLayoutPanel"] as TableLayoutPanel;

                if (tlpf != null)
                {
                    TableLayoutPanel tlp2 = tlpf.Controls["addRemoveTableLayoutPanel"] as TableLayoutPanel;

                    if (tlp2 != null)
                    {
                        Button btn = tlp2.Controls["removeButton"] as Button;

                        if (btn != null)
                            btn.Click += GaugeCollectionEditor_RemoveClick;

                        btn = tlp2.Controls["addButton"] as Button;

                        if (btn != null)
                        {
                            _AddTextItem = btn.ContextMenuStrip.Items[0];
                            _AddImageItem = btn.ContextMenuStrip.Items[1];
                            _AddDigitalIndicatorItem = btn.ContextMenuStrip.Items[2];
                            _AddMechanicalIndicatorItem = btn.ContextMenuStrip.Items[3];
                            _AddStateIndicatorItem = btn.ContextMenuStrip.Items[4];

                            AddCopyButton(collectionForm);
                        }
                    }

                    _ListBox = tlpf.Controls["listbox"] as ListBox;

                    if (_ListBox != null)
                        _ListBox.SelectedIndexChanged += ListBox_SelectedIndexChanged;

                    _PropertyGrid = tlpf.Controls["propertyBrowser"] as PropertyGrid;

                    if (_PropertyGrid != null)
                        _PropertyGrid.HelpVisible = true;
                }
            }

            return (collectionForm);
        }

        #endregion

        #region CreateInstance

        protected override object CreateInstance(Type itemType)
        {
            if (itemType == typeof(DigitalIndicator))
            {
                NumericIndicator ind = (NumericIndicator)
                    base.CreateInstance(typeof(NumericIndicator));

                ind.Style = NumericIndicatorStyle.Digital16Segment;

                ind.BackColor.BorderWidth = 3;
                ind.BackColor.BorderColor = Color.Gray;
                ind.BackColor.Color1 = Color.Black;

                ind.DigitColor = Color.Red;
                ind.DigitDimColor = Color.FromArgb(94, 0, 0);
                ind.DecimalColor = Color.Lime;
                ind.DecimalDimColor = Color.FromArgb(0, 94, 0);

                ind.Size = new SizeF(.4f, .08f);

                return (ind);
            }

            if (itemType == typeof(MechanicalIndicator))
            {
                NumericIndicator ind = (NumericIndicator)
                    base.CreateInstance(typeof(NumericIndicator));

                ind.BackColor.BorderWidth = 1;
                ind.BackColor.BorderColor = Color.Black;
                ind.BackColor.Color1 = Color.Gray;
                ind.BackColor.Color2 = Color.White;
                ind.BackColor.GradientFillType = GradientFillType.HorizontalCenter;

                ind.SeparatorColor.BorderWidth = 1;
                ind.SeparatorColor.BorderColor = Color.Gray;
                ind.SeparatorColor.Color1 = Color.LightGray;
                ind.SeparatorColor.Color2 = Color.DimGray;
                ind.SeparatorColor.GradientFillType = GradientFillType.HorizontalCenter;

                ind.Size = new SizeF(.4f, .08f);

                return (ind);
            }

            if (itemType == typeof(StateIndicator))
            {
                StateIndicator ind = (StateIndicator)
                    base.CreateInstance(typeof(StateIndicator));

                ind.EmptyString = "";

                return (ind);
            }

            return (base.CreateInstance(itemType));
        }

        #endregion

        #region CopyButton support

        #region AddCopyButton

        private void AddCopyButton(CollectionForm collectionForm)
        {
            _CopyButton = new Button();

            _CopyButton.Size = new Size(23, 24);
            _CopyButton.Enabled = false;
            _CopyButton.Click += CopyButton_Click;

            ResourceManager rm = Resources.ResourceManager;
            _CopyButton.Image = (Image)rm.GetObject("Copy");

            collectionForm.Controls.Add(_CopyButton);

            _CopyButton.Location = new Point(208, 85);
            _CopyButton.BringToFront();

            _ToolTip = new ToolTip();
            _ToolTip.SetToolTip(_CopyButton, "Clone the selected item");
        }

        #endregion

        #region CopyButton_Click

        void CopyButton_Click(object sender, EventArgs e)
        {
            if (_GaugeItem != null)
            {
                _CopyItem = _GaugeItem;

                if (_GaugeItem is GaugeText)
                    _AddTextItem.PerformClick();

                else if (_GaugeItem is GaugeImage)
                    _AddImageItem.PerformClick();

                else if (_GaugeItem is NumericIndicator)
                {
                    if (((NumericIndicator) _GaugeItem).Style == NumericIndicatorStyle.Mechanical)
                        _AddMechanicalIndicatorItem.PerformClick();
                    else
                        _AddDigitalIndicatorItem.PerformClick();
                }
                else if (_GaugeItem is StateIndicator)
                    _AddStateIndicatorItem.PerformClick();
            }
        }

        #endregion

        #endregion

        #region ListBox_SelectedIndexChanged

        void ListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_ListBox.SelectedItem != null)
            {
                PropertyInfo p = _ListBox.SelectedItem.GetType().GetProperty("Value");

                _LastGaugeItem = _GaugeItem;
                _GaugeItem = (GaugeItem)p.GetValue(_ListBox.SelectedItem, null);

                if (_CopyItem != null)
                {
                    if (_GaugeItem is GaugeText)
                        CopyGaugeText(_LastGaugeItem as GaugeText, _GaugeItem as GaugeText);

                    else if (_GaugeItem is GaugeImage)
                        CopyGaugeImage(_LastGaugeItem as GaugeImage, _GaugeItem as GaugeImage);

                    else if (_GaugeItem is NumericIndicator)
                        CopyGaugeNumericIndicator(_LastGaugeItem as NumericIndicator, _GaugeItem as NumericIndicator);

                    else if (_GaugeItem is StateIndicator)
                        CopyGaugeStateIndicator(_LastGaugeItem as StateIndicator, _GaugeItem as StateIndicator);

                    _CopyItem = null;
                }

                _CopyButton.Enabled = true;
            }
            else
            {
                _CopyButton.Enabled = false;
            }
        }

        #endregion

        #region CopyGaugeText

        private void CopyGaugeText(GaugeText gt, GaugeText clone)
        {
            if (gt != null && clone != null)
                gt.CopyToItem(clone);
        }

        #endregion

        #region CopyGaugeImage

        private void CopyGaugeImage(GaugeImage gi, GaugeImage clone)
        {
            if (gi != null && clone != null)
                gi.CopyToItem(clone);
        }

        #endregion

        #region CopyGaugeNumericIndicator

        private void CopyGaugeNumericIndicator(NumericIndicator gi, NumericIndicator clone)
        {
            if (gi != null && clone != null)
                gi.CopyToItem(clone);
        }

        #endregion

        #region CopyGaugeStateIndicator

        private void CopyGaugeStateIndicator(StateIndicator gi, StateIndicator clone)
        {
            if (gi != null && clone != null)
                gi.CopyToItem(clone);
        }

        #endregion

        #region CreateCollectionItemType

        protected override Type CreateCollectionItemType()
        {
            return typeof(GaugeText);
        }

        #endregion

        #region CreateNewItemTypes

        protected override Type[] CreateNewItemTypes()
        {
            return new Type[]
            {
                typeof(GaugeText),
                typeof(GaugeImage),
                typeof(DigitalIndicator),
                typeof(MechanicalIndicator),
                typeof(StateIndicator),
            };
        }

        private class DigitalIndicator : NumericIndicator
        {
        }

        private class MechanicalIndicator : NumericIndicator
        {
        }

        #endregion

        #region GaugeCollectionEditor_RemoveClick

        void GaugeCollectionEditor_RemoveClick(object sender, EventArgs e)
        {
            if (_LastGaugeItem != null)
            {
                _LastGaugeItem.Visible = false;
                _LastGaugeItem = null;
            }
            else if (_GaugeItem != null)
            {
                _GaugeItem.Visible = false;
                _GaugeItem = null;
            }
        }

        #endregion
    }
}
