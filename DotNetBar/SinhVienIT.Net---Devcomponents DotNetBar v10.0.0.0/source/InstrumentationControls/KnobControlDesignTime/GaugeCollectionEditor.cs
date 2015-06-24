using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Drawing;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;
using DevComponents.DotNetBarKnobControl.Design.Properties;

namespace DevComponents.Instrumentation.Design
{
    public class GaugeCollectionEditor : CollectionEditor
    {
        #region Private variables

        private ListBox _ListBox;
        private GaugeItem _GaugeItem;
        private GaugeItem _LastGaugeItem;
        private PropertyGrid _PropertyGrid;

        private Button _AddButton;
        private Button _RemButton;
        private Button _CopyButton;
        private ToolTip _ToolTip;

        #endregion

        public GaugeCollectionEditor(Type type)
            : base(type)
        {
        }

        #region CreateCollectionForm

        protected override CollectionForm CreateCollectionForm()
        {
            CollectionForm collectionForm = base.CreateCollectionForm();

            _GaugeItem = null;

            if (collectionForm.Controls[0] is TableLayoutPanel)
            {
                TableLayoutPanel tlpf = collectionForm.Controls["overArchingTableLayoutPanel"] as TableLayoutPanel;

                if (tlpf != null)
                {
                    TableLayoutPanel tlp2 = tlpf.Controls["addRemoveTableLayoutPanel"] as TableLayoutPanel;

                    if (tlp2 != null)
                    {
                        _RemButton = tlp2.Controls["removeButton"] as Button;

                        if (_RemButton != null)
                            _RemButton.Click += GaugeCollectionEditor_RemoveClick;

                        _AddButton = tlp2.Controls["addButton"] as Button;

                        if (_AddButton != null)
                        {
                            _AddButton.Click += GaugeCollectionEditor_AddClick;

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

        #region CopyButton_Click

        void CopyButton_Click(object sender, EventArgs e)
        {
            if (_GaugeItem != null)
            {
                if (_GaugeItem is GaugeCircularScale)
                    CopyCircularScale(_GaugeItem as GaugeCircularScale);

                else if (_GaugeItem is GaugeLinearScale)
                    CopyLinearScale(_GaugeItem as GaugeLinearScale);

                else if (_GaugeItem is GaugeSection)
                    CopySection(_GaugeItem as GaugeSection);

                else if (_GaugeItem is GaugeRange)
                    CopyRange(_GaugeItem as GaugeRange);

                else if (_GaugeItem is GaugeCustomLabel)
                    CopyLabel(_GaugeItem as GaugeCustomLabel);

                else if (_GaugeItem is GaugePointer)
                    CopyPointer(_GaugeItem as GaugePointer);

                else if (_GaugeItem is NumericRange)
                    CopyNumericRange(_GaugeItem as NumericRange);

                else if (_GaugeItem is StateRange)
                    CopyStateRange(_GaugeItem as StateRange);
            }
        }

        #endregion

        #region CopyCircularScale

        private void CopyCircularScale(GaugeCircularScale scale)
        {
            if (_AddButton != null)
            {
                _AddButton.PerformClick();

                GaugeCircularScaleCollection css = scale.GaugeControl.CircularScales;
                GaugeCircularScale clone = css[css.Count - 1];

                scale.CopyToItem(clone);
            }
        }

        #endregion

        #region CopyLinearScale

        private void CopyLinearScale(GaugeLinearScale scale)
        {
            if (_AddButton != null)
            {
                _AddButton.PerformClick();

                GaugeLinearScaleCollection lss = scale.GaugeControl.LinearScales;
                GaugeLinearScale clone = lss[lss.Count - 1];

                scale.CopyToItem(clone);
            }
        }

        #endregion

        #region CopySection

        private void CopySection(GaugeSection section)
        {
            if (_AddButton != null)
            {
                _AddButton.PerformClick();

                GaugeSectionCollection sc = section.Scale.Sections;
                GaugeSection clone = sc[sc.Count - 1];

                section.CopyToItem(clone);
            }
        }

        #endregion

        #region CopyRange

        private void CopyRange(GaugeRange range)
        {
            if (_AddButton != null)
            {
                _AddButton.PerformClick();

                GaugeRangeCollection rc = range.Scale.Ranges;
                GaugeRange clone = rc[rc.Count - 1];

                range.CopyToItem(clone);
            }
        }

        #endregion

        #region CopyLabel

        private void CopyLabel(GaugeCustomLabel label)
        {
            if (_AddButton != null)
            {
                _AddButton.PerformClick();

                GaugeCustomLabelCollection lc = label.Scale.CustomLabels;
                GaugeCustomLabel clone = lc[lc.Count - 1];

                label.CopyToItem(clone);
            }
        }

        #endregion

        #region CopyPointer

        private void CopyPointer(GaugePointer pointer)
        {
            if (_AddButton != null)
            {
                _AddButton.PerformClick();

                GaugePointerCollection pc = pointer.Scale.Pointers;
                GaugePointer clone = pc[pc.Count - 1];

                pointer.CopyToItem(clone);
            }
        }

        #endregion

        #region CopyNumericRange

        private void CopyNumericRange(NumericRange range)
        {
            if (_AddButton != null)
            {
                _AddButton.PerformClick();

                NumericRangeCollection pc = range.NumericIndicator.Ranges;
                NumericRange clone = pc[pc.Count - 1];

                range.CopyToItem(clone);
            }
        }

        #endregion

        #region CopyStateRange

        private void CopyStateRange(StateRange range)
        {
            if (_AddButton != null)
            {
                _AddButton.PerformClick();

                StateRangeCollection pc = range.StateIndicator.Ranges;
                StateRange clone = pc[pc.Count - 1];

                range.CopyToItem(clone);
            }
        }

        #endregion

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

                _CopyButton.Enabled = true;
            }
            else
            {
                _CopyButton.Enabled = false;
            }
        }

        #endregion

        #region GaugeCollectionEditor_AddClick

        void GaugeCollectionEditor_AddClick(object sender, EventArgs e)
        {
            if (_GaugeItem != null)
            {
                if (_GaugeItem is GaugeCircularScale)
                    InitializeNewCircularScale(_GaugeItem as GaugeCircularScale);

                else if (_GaugeItem is GaugeLinearScale)
                    InitializeNewLinearScale(_GaugeItem as GaugeLinearScale);

                else if (_GaugeItem is GaugeSection)
                    InitializeNewSection(_GaugeItem as GaugeSection);

                else if (_GaugeItem is GaugeRange)
                    InitializeNewRange(_GaugeItem as GaugeRange);

                else if (_GaugeItem is GaugeCustomLabel)
                    InitializeNewCustomLabel(_GaugeItem as GaugeCustomLabel);

                else if (_GaugeItem is GaugePointer)
                    InitializeNewPointer(_GaugeItem as GaugePointer);

                else if (_GaugeItem is NumericRange)
                    InitializeNewNumericRange(_GaugeItem as NumericRange);

                else if (_GaugeItem is StateRange)
                    InitializeNewStateRange(_GaugeItem as StateRange);
            }
        }

        #region InitializeNewScale

        #region InitializeNewCircularScale

        private void InitializeNewCircularScale(GaugeCircularScale scale)
        {
            scale.Name = GetScaleName(scale);

            GaugeSection section = new GaugeSection(scale);
            section.Name = GetSectionName(scale);

            section.FillColor = new GradientFillColor(Color.CornflowerBlue);

            scale.Sections.Add(section);
        }

        #region GetScaleName

        private string GetScaleName(GaugeCircularScale scale)
        {
            for (int i = 1; i < 100; i++)
            {
                string s = "Scale" + i.ToString();

                if (scale.GaugeControl.CircularScales[s] == null)
                    return (s);
            }

            return (null);
        }

        #endregion

        #endregion

        #region InitializeNewLinearScale

        private void InitializeNewLinearScale(GaugeLinearScale scale)
        {
            scale.Orientation = GetDefaultOrientation(scale);

            scale.Name = GetScaleName(scale);

            GaugeSection section = new GaugeSection(scale);
            section.Name = GetSectionName(scale);

            section.FillColor = new GradientFillColor(Color.CornflowerBlue);

            scale.Sections.Add(section);
        }

        #region GetDefaultOrientation

        private Orientation GetDefaultOrientation(GaugeLinearScale scale)
        {
            foreach (GaugeLinearScale gls in scale.GaugeControl.LinearScales)
            {
                if (gls != scale)
                    return (gls.Orientation);
            }

            return (scale.GaugeControl.Width > scale.GaugeControl.Height
                ? Orientation.Horizontal : Orientation.Vertical);
        }

        #endregion

        #region GetScaleName

        private string GetScaleName(GaugeLinearScale scale)
        {
            for (int i = 1; i < 100; i++)
            {
                string s = "Scale" + i.ToString();

                if (scale.GaugeControl.LinearScales[s] == null)
                    return (s);
            }

            return (null);
        }

        #endregion

        #endregion

        #endregion

        #region InitializeNewSection

        #region InitializeNewSection

        private void InitializeNewSection(GaugeSection section)
        {
            section.StartValue = section.Scale.MinValue;
            section.EndValue = section.Scale.MinValue + (section.Scale.MajorTickMarks.Interval * 2);

            section.Name = GetSectionName(section.Scale);

            section.FillColor = new GradientFillColor(Color.Lime);
        }

        #endregion

        #region GetSectionName

        private string GetSectionName(GaugeScale scale)
        {
            for (int i = 1; i < 100; i++)
            {
                string s = "Section" + i.ToString();

                if (scale.Sections[s] == null)
                    return (s);
            }

            return (null);
        }

        #endregion

        #endregion

        #region InitializeNewRange

        #region InitializeNewRange

        private void InitializeNewRange(GaugeRange range)
        {
            range.EndValue = range.Scale.MaxValue;
            range.StartValue = range.EndValue - range.Scale.MajorTickMarks.Interval * 3;

            range.Name = GetRangeName(range.Scale);

            range.FillColor = new GradientFillColor(Color.Lime, Color.Red);
            range.FillColor.BorderColor = Color.Black;
            range.FillColor.BorderWidth = 1;
        }

        #endregion

        #region GetRangeName

        private string GetRangeName(GaugeScale scale)
        {
            for (int i = 1; i < 100; i++)
            {
                string s = "Range" + i.ToString();

                if (scale.Ranges[s] == null)
                    return (s);
            }

            return (null);
        }

        #endregion

        #endregion

        #region InitializeNewCustomLabel

        #region InitializeNewCustomLabel

        private void InitializeNewCustomLabel(GaugeCustomLabel label)
        {
            label.Value = label.Scale.MinValue;

            label.Name = GetLabelName(label.Scale);
        }

        #endregion

        #region GetLabelName

        private string GetLabelName(GaugeScale scale)
        {
            for (int i = 1; i < 100; i++)
            {
                string s = "Label" + i.ToString();

                if (scale.CustomLabels[s] == null)
                    return (s);
            }

            return (null);
        }

        #endregion

        #endregion

        #region InitializeNewPointer

        #region InitializeNewPointer

        private void InitializeNewPointer(GaugePointer pointer)
        {
            pointer.Style = (pointer.Scale is GaugeCircularScale)
                ? PointerStyle.Needle : PointerStyle.Marker;

            pointer.Name = GetPointerName(pointer.Scale);

            pointer.FillColor = new GradientFillColor(Color.WhiteSmoke, Color.Red);
            pointer.FillColor.BorderColor = Color.DimGray;
            pointer.FillColor.BorderWidth = 1;

            Color color1 = Color.FromArgb(100, 60, 60, 60);

            pointer.ThermoBackColor = new GradientFillColor(color1);
            pointer.ThermoBackColor.BorderColor = Color.Black;
            pointer.ThermoBackColor.BorderWidth = 1;
        }

        #endregion

        #region GetPointerName

        private string GetPointerName(GaugeScale scale)
        {
            for (int i = 1; i < 100; i++)
            {
                string s = "Pointer" + i.ToString();

                if (scale.Pointers[s] == null)
                    return (s);
            }

            return (null);
        }

        #endregion

        #endregion

        #endregion

        #region InitializeNewNumericRange

        #region InitializeNewNumericRange

        private void InitializeNewNumericRange(NumericRange range)
        {
            range.StartValue = 500;
            range.EndValue = 1000;

            range.Name = GetNumericRangeName(range.NumericIndicator);

            range.DigitColor = Color.Red;
            range.DigitDimColor = Color.FromArgb(94, 0, 0);
            range.DecimalColor = range.DigitColor;
            range.DecimalDimColor = range.DigitDimColor;
        }

        #endregion

        #region GetNumericRangeName

        private string GetNumericRangeName(NumericIndicator indicator)
        {
            for (int i = 1; i < 100; i++)
            {
                string s = "Range" + i.ToString();

                if (indicator.Ranges[s] == null)
                    return (s);
            }

            return (null);
        }

        #endregion

        #endregion

        #region InitializeNewStateRange

        #region InitializeNewStateRange

        private void InitializeNewStateRange(StateRange range)
        {
            range.StartValue = 500;
            range.EndValue = 1000;

            range.Name = GetStateRangeName(range.StateIndicator);
        }

        #endregion

        #region GetStateRangeName

        private string GetStateRangeName(StateIndicator indicator)
        {
            for (int i = 1; i < 100; i++)
            {
                string s = "Range" + i.ToString();

                if (indicator.Ranges[s] == null)
                    return (s);
            }

            return (null);
        }

        #endregion

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

    #region CollectionChangedEventArgs

    /// <summary>
    /// CollectionChangedEventArgs
    /// </summary>
    public class CollectionChangedEventArgs : EventArgs
    {
        #region Private variables

        private List<object> _NewList;

        #endregion

        public CollectionChangedEventArgs(List<object> newList)
        {
            _NewList = newList;
        }

        #region Public properties

        /// <summary>
        /// Gets the selected NewList
        /// </summary>
        public List<object> NewList
        {
            get { return (_NewList); }
        }

        #endregion
    }

    #endregion

}
