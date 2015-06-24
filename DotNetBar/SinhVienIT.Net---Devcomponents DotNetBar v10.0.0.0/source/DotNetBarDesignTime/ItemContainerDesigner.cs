using System;
using System.ComponentModel.Design;

namespace DevComponents.DotNetBar.Design
{
	/// <summary>
	/// Represents Windows Forms Designer for ItemContainer object.
	/// </summary>
	public class ItemContainerDesigner:BaseItemDesigner
	{
		public override DesignerVerbCollection Verbs 
		{
			get 
			{
				DesignerVerb[] verbs = new DesignerVerb[]
					{
						new DesignerVerb("Add Button", new EventHandler(CreateButton)),
						new DesignerVerb("Add Horizontal Container", new EventHandler(CreateHorizontalContainer)),
						new DesignerVerb("Add Vertical Container", new EventHandler(CreateVerticalContainer)),
                        new DesignerVerb("Add Scrollable Container", new EventHandler(CreateGallery)),
						new DesignerVerb("Add Text Box", new EventHandler(CreateTextBox)),
						new DesignerVerb("Add Combo Box", new EventHandler(CreateComboBox)),
						new DesignerVerb("Add Label", new EventHandler(CreateLabel)),
                        new DesignerVerb("Add Check Box", new EventHandler(CreateCheckBox)),
                        new DesignerVerb("Add Micro-Chart", new EventHandler(CreateMicroChart)),
                        new DesignerVerb("Add Switch button", new EventHandler(CreateSwitch)),
                        new DesignerVerb("Add Control Container", new EventHandler(CreateControlContainer)),
						new DesignerVerb("Add Color Picker", new EventHandler(CreateColorPicker)),
                        new DesignerVerb("Add Progress-Bar", new EventHandler(CreateProgressBar)),
                        new DesignerVerb("Add Circular Progress", new EventHandler(CreateCircularProgressItem)),
                        new DesignerVerb("Add Rating Item", new EventHandler(CreateRatingItem)),
                        new DesignerVerb("Add Slider", new EventHandler(CreateSlider))
#if FRAMEWORK20
                        ,new DesignerVerb("Add Month Calendar", new EventHandler(CreateMonthCalendar))
#endif
				};
				return new DesignerVerbCollection(verbs);
			}
		}

		private void CreateVerticalContainer(object sender, EventArgs e)
		{
			CreateContainer(eOrientation.Vertical);
		}

		private void CreateHorizontalContainer(object sender, EventArgs e)
		{
			CreateContainer(eOrientation.Horizontal);
		}

		private void CreateContainer(eOrientation orientation)
		{
            try
            {
                m_CreatingItem = true;
                DesignerSupport.CreateItemContainer(this, (BaseItem)this.Component, orientation);
                this.RecalcLayout();
            }
            finally
            {
                m_CreatingItem = false;
            }
		}

	}
}
