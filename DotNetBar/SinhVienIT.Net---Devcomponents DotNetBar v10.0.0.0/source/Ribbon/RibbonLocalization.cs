using System;
using System.Text;
using System.ComponentModel;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Represents the class that stores text used by ribbon control only for localization purposes.
    /// </summary>
    [ToolboxItem(false), TypeConverter(typeof(System.ComponentModel.ExpandableObjectConverter))]
    public class RibbonLocalization
    {
        #region Private Variables
        private string m_QatRemoveItemText = "&Remove from Quick Access Toolbar";
        private string m_QatAddItemText = "&Add to Quick Access Toolbar";
        private string m_QatCustomizeText = "&Customize Quick Access Toolbar...";
        private string m_QatPlaceBelowRibbonText = "&Place Quick Access Toolbar below the Ribbon";
        private string m_QatPlaceAboveRibbonText = "&Place Quick Access Toolbar above the Ribbon";

        private string m_QatDialogOkButton = "OK";
        private string m_QatDialogCancelButton = "Cancel";
        private string m_QatDialogAddButton = "&Add >>";
        private string m_QatDialogRemoveButton = "&Remove";
        private string m_QatDialogCategoriesLabel = "&Choose commands from:";
        private string m_QatDialogPlacementCheckbox = "&Place Quick Access Toolbar below the Ribbon";
        private string m_QatDialogCaption = "Customize Quick Access Toolbar";
        private string m_MinimizeRibbonText = "Mi&nimize the Ribbon";
        private string m_MaximizeRibbonText = "&Maximize the Ribbon";
        private string m_QatCustomizeMenuLabel = "<b>Customize Quick Access Toolbar</b>";
        #endregion

        #region Internal Implementation
        /// <summary>
        /// Gets or sets the title text of the Quick Access Toolbar Customize dialog form.
        /// </summary>
        [Localizable(true), Description("Indicates the the title text of the Quick Access Toolbar Customize dialog form."), Category("QAT Customize Dialog")]
        public string QatDialogCaption
        {
            get { return m_QatDialogCaption; }
            set { m_QatDialogCaption = value; }
        }

        /// <summary>
        /// Gets or sets the text of the "Place Quick Access Toolbar below the Ribbon" check-box on the Quick Access Toolbar Customize dialog form.
        /// </summary>
        [Localizable(true), Description("Indicates the text of the 'Place Quick Access Toolbar below the Ribbon' check-box on the Quick Access Toolbar Customize dialog form."), Category("QAT Customize Dialog")]
        public string QatDialogPlacementCheckbox
        {
            get { return m_QatDialogPlacementCheckbox; }
            set { m_QatDialogPlacementCheckbox = value; }
        }

        /// <summary>
        /// Gets or sets the text of the Choose commands from label on the Quick Access Toolbar Customize dialog form.
        /// </summary>
        [Localizable(true), Description("Indicates the text of the Choose commands from label on the Quick Access Toolbar Customize dialog form."), Category("QAT Customize Dialog")]
        public string QatDialogCategoriesLabel
        {
            get { return m_QatDialogCategoriesLabel; }
            set { m_QatDialogCategoriesLabel = value; }
        }

        /// <summary>
        /// Gets or sets the text of the Remove button on the Quick Access Toolbar Customize dialog form.
        /// </summary>
        [Localizable(true), Description("Indicates the text of the Remove button on the Quick Access Toolbar Customize dialog form."), Category("QAT Customize Dialog")]
        public string QatDialogRemoveButton
        {
            get { return m_QatDialogRemoveButton; }
            set { m_QatDialogRemoveButton = value; }
        }

        /// <summary>
        /// Gets or sets the text of the Add button on the Quick Access Toolbar Customize dialog form.
        /// </summary>
        [Localizable(true), Description("Indicates the text of the Add button on the Quick Access Toolbar Customize dialog form."), Category("QAT Customize Dialog")]
        public string QatDialogAddButton
        {
            get { return m_QatDialogAddButton; }
            set { m_QatDialogAddButton = value; }
        }

        /// <summary>
        /// Gets or sets the text of the OK button on the Quick Access Toolbar Customize dialog form.
        /// </summary>
        [Localizable(true), Description("Indicates the text of the OK button on the Quick Access Toolbar Customize dialog form."), Category("QAT Customize Dialog")]
        public string QatDialogOkButton
        {
            get { return m_QatDialogOkButton; }
            set { m_QatDialogOkButton = value; }
        }

        /// <summary>
        /// Gets or sets the text of the Cancel button on the Quick Access Toolbar Customize dialog form.
        /// </summary>
        [Localizable(true), Description("Indicates the text of the OK button on the Quick Access Toolbar Customize dialog form."), Category("QAT Customize Dialog")]
        public string QatDialogCancelButton
        {
            get { return m_QatDialogCancelButton; }
            set { m_QatDialogCancelButton = value; }
        }

        /// <summary>
        /// Gets or sets the text that is used on context menu used to customize Quick Access Toolbar.
        /// </summary>
        [Localizable(true), Description("Indicates the text that is used on context menu used to customize Quick Access Toolbar."), Category("Quick Access Toolbar")]
        public string QatRemoveItemText
        {
            get { return m_QatRemoveItemText; }
            set { m_QatRemoveItemText = value; }
        }

        /// <summary>
        /// Gets or sets the text that is used on context menu used to customize Quick Access Toolbar.
        /// </summary>
        [Localizable(true), Description("Indicates the text that is used on context menu used to customize Quick Access Toolbar."), Category("Quick Access Toolbar")]
        public string QatAddItemText
        {
            get { return m_QatAddItemText; }
            set { m_QatAddItemText = value; }
        }

        /// <summary>
        /// Gets or sets the text that is used on context menu used to customize Quick Access Toolbar.
        /// </summary>
        [Localizable(true), Description("Indicates the text that is used on context menu used to customize Quick Access Toolbar."), Category("Quick Access Toolbar")]
        public string QatCustomizeText
        {
            get { return m_QatCustomizeText; }
            set { m_QatCustomizeText = value; }
        }

        /// <summary>
        /// Gets or sets the text that is used on Quick Access Toolbar customize menu label.
        /// </summary>
        [Localizable(true), Description("Indicates text that is used on Quick Access Toolbar customize menu label."), Category("Quick Access Toolbar")]
        public string QatCustomizeMenuLabel
        {
            get { return m_QatCustomizeMenuLabel; }
            set { m_QatCustomizeMenuLabel = value; }
        }

        /// <summary>
        /// Gets or sets the text that is used on context menu used to change placement of the Quick Access Toolbar.
        /// </summary>
        [Localizable(true), Description("Indicates the text that is used on context menu used to change placement of the Quick Access Toolbar."), Category("Quick Access Toolbar")]
        public string QatPlaceBelowRibbonText
        {
            get { return m_QatPlaceBelowRibbonText; }
            set { m_QatPlaceBelowRibbonText = value; }
        }

        /// <summary>
        /// Gets or sets the text that is used on context menu used to change placement of the Quick Access Toolbar.
        /// </summary>
        [Localizable(true), Description("Indicates the text that is used on context menu used to change placement of the Quick Access Toolbar."), Category("Quick Access Toolbar")]
        public string QatPlaceAboveRibbonText
        {
            get { return m_QatPlaceAboveRibbonText; }
            set { m_QatPlaceAboveRibbonText = value; }
        }

        /// <summary>
        /// Gets or sets the text that is used on context menu item used to minimize the Ribbon.
        /// </summary>
        [Localizable(true), Description("Indicates text that is used on context menu item used to minimize the Ribbon."), Category("Quick Access Toolbar")]
        public string MinimizeRibbonText
        {
            get { return m_MinimizeRibbonText; }
            set { m_MinimizeRibbonText = value; }
        }

        /// <summary>
        /// Gets or sets the text that is used on context menu item used to maximize the Ribbon.
        /// </summary>
        [Localizable(true), Description("Indicates text that is used on context menu item used to maximize the Ribbon."), Category("Quick Access Toolbar")]
        public string MaximizeRibbonText
        {
            get { return m_MaximizeRibbonText; }
            set { m_MaximizeRibbonText = value; }
        }
        #endregion
    }
}
