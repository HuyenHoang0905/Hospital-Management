#if FRAMEWORK20
using System;
using System.Text;
using System.Windows.Forms.Design;
using DevComponents.DotNetBar.Controls;
using System.ComponentModel;
using System.Drawing;
using System.ComponentModel.Design;

namespace DevComponents.DotNetBar.Design
{
    /// <summary>
    /// Represents the designer for WarningBox control.
    /// </summary>
    public class WarningBoxDesigner : ControlDesigner
    {
        #region Internal Implementation
        public override void InitializeNewComponent(System.Collections.IDictionary defaultValues)
        {
            base.InitializeNewComponent(defaultValues);
            SetDefaults();
        }

        private void SetDefaults()
        {
            WarningBox box = this.Control as WarningBox;
            if (box == null) return;

            TypeDescriptor.GetProperties(box)["Text"].SetValue(box, "<b>Warning Box</b> control with <i>text-markup</i> support.");
            Image image = GetDefaultImage();
            if (image != null)
                TypeDescriptor.GetProperties(box)["Image"].SetValue(box, image);
        }

        private Image GetDefaultImage()
        {
            string imageName = "images\\Warning.png";
            try
            {
                Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine;
                string path = "";
                try
                {
                    if (key != null)
                        key = key.OpenSubKey("Software\\DevComponents\\DotNetBar");
                    if (key != null)
                        path = key.GetValue("InstallationFolder", "").ToString();
                }
                finally { if (key != null) key.Close(); }

                if (path != "")
                {
                    if (path.Substring(path.Length - 1, 1) != "\\")
                        path += "\\";

                    if (System.IO.File.Exists(path + imageName))
                        path += imageName;
                    else
                        path = "";
                }

                if (path != "")
                {
                    return new Bitmap(path);
                }
            }
            catch (Exception)
            {
            }
            return null;
        }

        private DesignerActionListCollection _ActionLists = null;
        public override DesignerActionListCollection ActionLists
        {
            get
            {
                if (_ActionLists == null)
                {
                    _ActionLists = new DesignerActionListCollection();
                    _ActionLists.Add(new WarningBoxActionList(this));
                }
                return _ActionLists;
            }
        }
        #endregion
    }

    #region WarningBoxActionList
    public class WarningBoxActionList : DesignerActionList
    {
        private WarningBoxDesigner _Designer = null;

        /// <summary>
        /// Initializes a new instance of the AdvTreeActionList class.
        /// </summary>
        /// <param name="designer"></param>
        public WarningBoxActionList(WarningBoxDesigner designer)
            : base(designer.Component)
        {
            _Designer = designer;
        }

        public override DesignerActionItemCollection GetSortedActionItems()
        {
            DesignerActionItemCollection items = new DesignerActionItemCollection();
            items.Add(new DesignerActionHeaderItem("Appearance"));
            items.Add(new DesignerActionHeaderItem("Information"));

            items.Add(new DesignerActionPropertyItem("OptionsButtonVisible", "Show Options button?", "Appearance", "Controls visibility of Options button in WarningBox"));
            items.Add(new DesignerActionPropertyItem("CloseButtonVisible", "Show Close button?", "Appearance", "Controls visibility of Close button in WarningBox"));
            items.Add(new DesignerActionPropertyItem("Image", "Image", "Appearance", "Image that is displayed on Warning Box"));

            items.Add(new DesignerActionTextItem("See Office2007ColorTable.WarningBox members to customize colors.", "Information"));
            items.Add(new DesignerActionMethodItem(this, "OpenKB", "How to customize color table?", "Information", "Opens online Knowledge Base article which shows how to customize Office2007ColorTable", true));
            items.Add(new DesignerActionMethodItem(this, "OpenTextMarkupRef", "Text Markup Reference...", "Information", "Opens online Text Markup reference. Markup can be used in Text property.", true));
            
            return items;
        }

        public void OpenKB()
        {
            System.Diagnostics.Process.Start("http://www.devcomponents.com/kb/questions.php?questionid=37&vs");
        }
        public void OpenTextMarkupRef()
        {
            System.Diagnostics.Process.Start("http://www.devcomponents.com/kb/questions.php?questionid=5&vs");
        }

        public bool OptionsButtonVisible
        {
            get
            {
                return ((WarningBox)base.Component).OptionsButtonVisible;
            }
            set
            {
                TypeDescriptor.GetProperties(base.Component)["OptionsButtonVisible"].SetValue(base.Component, value);
            }
        }

        public bool CloseButtonVisible
        {
            get
            {
                return ((WarningBox)base.Component).CloseButtonVisible;
            }
            set
            {
                TypeDescriptor.GetProperties(base.Component)["CloseButtonVisible"].SetValue(base.Component, value);
            }
        }

        public Image Image
        {
            get
            {
                return ((WarningBox)base.Component).Image;
            }
            set
            {
                TypeDescriptor.GetProperties(base.Component)["Image"].SetValue(base.Component, value);
            }
        }
    }
    #endregion
}
#endif