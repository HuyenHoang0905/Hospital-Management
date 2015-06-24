using System;
using System.ComponentModel.Design;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Reprensents Windows Forms Designer for ItemContainer object.
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
						new DesignerVerb("Add Text Box", new EventHandler(CreateTextBox)),
						new DesignerVerb("Add Combo Box", new EventHandler(CreateComboBox)),
						new DesignerVerb("Add Label", new EventHandler(CreateLabel)),
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
			DesignerSupport.CreateItemContainer(this,(BaseItem)this.Component,orientation);
			this.RecalcLayout();
		}

	}
}
