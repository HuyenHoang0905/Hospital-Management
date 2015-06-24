using System;
using System.ComponentModel.Design;

namespace DevComponents.DotNetBar.Design
{
    /// <summary>
    /// Support for SuperTabStrip tabs design-time editor
    /// </summary>
    public class SuperTabStripTabsEditor : CollectionEditor
    {
        public SuperTabStripTabsEditor(Type type)
            : base(type)
        {
        }

        #region CreateCollectionItemType

        protected override Type CreateCollectionItemType()
        {
            return typeof(SuperTabItem);
        }

        #endregion

        #region CreateNewItemTypes

        protected override Type[] CreateNewItemTypes()
        {
            return new Type[]
            {
                typeof(SuperTabItem),
                typeof(ButtonItem),
                typeof(TextBoxItem),
                typeof(ComboBoxItem),
                typeof(LabelItem),
                typeof(ColorPickerDropDown),
                typeof(ProgressBarItem),
                typeof(CheckBoxItem),
            };
        }

        #endregion

        #region CreateInstance

        protected override object CreateInstance(Type itemType)
        {
            object item = base.CreateInstance(itemType);

            if (item is SuperTabItem)
            {
                SuperTabItem tabItem = item as SuperTabItem;

                tabItem.Text = String.IsNullOrEmpty(tabItem.Name) ? "My Tab" : tabItem.Name;
            }
            else if (item is ButtonItem)
            {
                ButtonItem bi = item as ButtonItem;

                bi.Text = String.IsNullOrEmpty(bi.Name) ? "My Button" : bi.Name;
            }
            else if (item is TextBoxItem)
            {
                TextBoxItem tbi = item as TextBoxItem;

                tbi.Text = String.IsNullOrEmpty(tbi.Name) ? "My TextBox" : tbi.Name;
            }
            else if (item is ComboBoxItem)
            {
                ComboBoxItem cbi = item as ComboBoxItem;

                cbi.Text = String.IsNullOrEmpty(cbi.Name) ? "My ComboBox" : cbi.Name;
            }
            else if (item is LabelItem)
            {
                LabelItem lbi = item as LabelItem;

                lbi.Text = String.IsNullOrEmpty(lbi.Name) ? "My Label" : lbi.Name;
            }
            else if (item is ColorItem)
            {
                ColorItem ci = item as ColorItem;

                ci.Text = String.IsNullOrEmpty(ci.Name) ? "My Color" : ci.Name;
            }
            else if (item is ProgressBarItem)
            {
                ProgressBarItem pbi = item as ProgressBarItem;

                pbi.Text = String.IsNullOrEmpty(pbi.Name) ? "My ProgressBar" : pbi.Name;
            }
            else if (item is CheckBoxItem)
            {
                CheckBoxItem cbi = item as CheckBoxItem;

                cbi.Text = String.IsNullOrEmpty(cbi.Name) ? "My CheckBox" : cbi.Name;
            }
            
            return (item);
        }

        #endregion
    }
}
