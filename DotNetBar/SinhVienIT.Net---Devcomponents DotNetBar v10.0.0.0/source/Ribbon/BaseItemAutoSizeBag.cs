using System;
using System.Text;
using System.Drawing;

namespace DevComponents.DotNetBar
{
    #region BaseItemAutoSizeBag
    internal class BaseItemAutoSizeBag
    {
        public BaseItem Item = null;
        private bool m_SettingsRecorded;

        protected bool SettingsRecorded
        {
            get
            {
                return m_SettingsRecorded;
            }
            set
            {
                m_SettingsRecorded = value;
            }
        }

        public virtual void RecordSetting(BaseItem item)
        {
            this.Item = item;
            m_SettingsRecorded = true;
        }

        public virtual void RestoreSettings()
        {
            m_SettingsRecorded = false;
        }
    }
    #endregion

    #region ItemContainerAutoSizeBag
    internal class ItemContainerAutoSizeBag : BaseItemAutoSizeBag
    {
        private bool m_MultiLine = false;
        private eOrientation m_LayoutOrientation = eOrientation.Horizontal;

        public override void RecordSetting(BaseItem item)
        {
            if (this.SettingsRecorded)
                return;
            
            ItemContainer cont = item as ItemContainer;
            m_MultiLine = cont.MultiLine;
            m_LayoutOrientation = cont.LayoutOrientation;

            base.RecordSetting(item);
        }

        public override void RestoreSettings()
        {
            if (!this.SettingsRecorded) return;

            ItemContainer cont = this.Item as ItemContainer;
            cont.MultiLine = m_MultiLine;
            cont.LayoutOrientation = m_LayoutOrientation;

            base.RestoreSettings();
        }
    }
    #endregion

    #region ButtonItemAutoSizeBag
    internal class ButtonItemAutoSizeBag : BaseItemAutoSizeBag
    {
        private eButtonStyle m_ButtonStyle = eButtonStyle.Default;
        private eImagePosition m_ImagePosition = eImagePosition.Left;
        private Size m_ImageFixedSize = Size.Empty;
        private string m_Text = null;

        public override void RecordSetting(BaseItem item)
        {
            if (this.SettingsRecorded)
                return;
            
            ButtonItem button = item as ButtonItem;
            m_ButtonStyle = button.ButtonStyle;
            m_ImagePosition = button.ImagePosition;
            m_ImageFixedSize = button.ImageFixedSize;
            if (button.TextMarkupBody != null && button.TextMarkupBody.HasExpandElement)
                m_Text = button.Text;

            base.RecordSetting(item);
        }

        public override void RestoreSettings()
        {
            if (!this.SettingsRecorded)
                return;

            ButtonItem button = this.Item as ButtonItem;
            bool gi = button.GlobalItem;
            button.GlobalItem = false;
            button.ButtonStyle = m_ButtonStyle;
            button.ImagePosition = m_ImagePosition;
            button.ImageFixedSize = m_ImageFixedSize;
            if (m_Text != null) button.Text = m_Text;

            button.GlobalItem = gi;
            base.RestoreSettings();
        }
    }
    #endregion

    #region AutoSizeBagFactory
    internal class AutoSizeBagFactory
    {
        public static BaseItemAutoSizeBag CreateAutoSizeBag(ButtonItem item)
        {
            ButtonItemAutoSizeBag b = new ButtonItemAutoSizeBag();
            b.Item = item;
            return b;
        }

        public static ItemContainerAutoSizeBag CreateAutoSizeBag(ItemContainer item)
        {
            ItemContainerAutoSizeBag c = new ItemContainerAutoSizeBag();
            c.Item = item;
            return c;
        }
    }
    #endregion
}
