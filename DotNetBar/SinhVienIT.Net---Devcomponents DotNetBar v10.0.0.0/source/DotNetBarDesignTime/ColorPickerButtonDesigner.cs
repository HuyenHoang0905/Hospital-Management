using System;
using System.Text;
using System.ComponentModel;
using System.Drawing;
using System.Collections;

namespace DevComponents.DotNetBar.Design
{
    public class ColorPickerButtonDesigner : ButtonXDesigner
    {
#if FRAMEWORK20
        public override void InitializeNewComponent(IDictionary defaultValues)
        {
            base.InitializeNewComponent(defaultValues);
            SetDesignTimeDefaults();
        }
#else
		public override void OnSetComponentDefaults()
		{
			base.OnSetComponentDefaults();
            SetDesignTimeDefaults();
		}
#endif

        private void SetDesignTimeDefaults()
        {
            ColorPickerButton b = this.Control as ColorPickerButton;
            TypeDescriptor.GetProperties(b)["Image"].SetValue(b, Helpers.LoadBitmap("SystemImages.ColorPickerButtonImage.png"));
            TypeDescriptor.GetProperties(b)["SelectedColorImageRectangle"].SetValue(b, new Rectangle(2,2,12,12));
            TypeDescriptor.GetProperties(b)["Text"].SetValue(b, "");
            TypeDescriptor.GetProperties(b)["Size"].SetValue(b, new Size(37,23));
            TypeDescriptor.GetProperties(b)["Style"].SetValue(b, eDotNetBarStyle.StyleManagerControlled);
        }
    }
}
