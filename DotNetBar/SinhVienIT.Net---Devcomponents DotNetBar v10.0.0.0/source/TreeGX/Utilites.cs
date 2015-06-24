using System;
using System.ComponentModel.Design;
using System.Drawing;

namespace DevComponents.Tree
{
	/// <summary>
	/// Represents class for static tree utilities.
	/// </summary>
	public class Utilites
	{
		/// <summary>
		/// Initializes control with default settings for connectors and nodes.
		/// </summary>
		/// <param name="tree">Control to initialize.</param>
		public static void InitializeTree(TreeGX tree)
		{
			InitializeTree(tree,new ComponentFactory());
		}
		
		/// <summary>
		/// Initializes control with default settings for connectors and nodes.
		/// </summary>
		/// <param name="tree">Control to initialize.</param>
		/// <param name="factory">Factory to use to create new instances of objects.</param>
		internal static void InitializeTree(TreeGX tree, ComponentFactory factory)
		{
			tree.RootConnector=factory.CreateComponent(typeof(NodeConnector)) as NodeConnector;
			tree.RootConnector.LineWidth=5;
			tree.RootConnector.LineColor=SystemColors.Highlight;
			tree.NodesConnector=factory.CreateComponent(typeof(NodeConnector)) as NodeConnector;
			tree.NodesConnector.LineWidth=5;
			tree.NodesConnector.LineColor=SystemColors.Highlight;

			eStyleBorderType border=eStyleBorderType.Solid;
			ElementStyle style=factory.CreateComponent(typeof(ElementStyle)) as ElementStyle;
			
			style.BackColorSchemePart=eColorSchemePart.BarBackground;
			style.BackColor2SchemePart=eColorSchemePart.BarBackground2;
			style.BackColorGradientAngle=90;
			style.CornerDiameter=4;
			style.CornerType=eCornerType.Rounded;
			style.BorderLeft=border;
			style.BorderLeftWidth=1;
			style.BorderTop=border;
			style.BorderTopWidth=1;
			style.BorderBottom=border;
			style.BorderBottomWidth=1;
			style.BorderRight=border;
			style.BorderRightWidth=1;
			style.BorderColorSchemePart=eColorSchemePart.BarDockedBorder;
			style.PaddingBottom=3;
			style.PaddingLeft=3;
			style.PaddingRight=3;
			style.PaddingTop=3;
			style.TextColor=Color.FromArgb(0,0,128);
			style.Font=tree.Font;
			tree.Styles.Add(style);

			tree.NodeStyle=style;
		}
		
		/// <summary>
		/// Creates new style and adds it to styles collection
		/// </summary>
		/// <param name="tree">Tree to assign style to</param>
		/// <param name="factory">Style factory</param>
		/// <param name="backColor"></param>
		/// <param name="backColor2"></param>
		/// <param name="gradientAngle"></param>
		/// <param name="textColor"></param>
		internal static ElementStyle CreateStyle(ComponentFactory factory, string description, Color borderColor, Color backColor, Color backColor2, int gradientAngle, Color textColor)
		{
			eStyleBorderType border=eStyleBorderType.Solid;
			ElementStyle style=factory.CreateComponent(typeof(ElementStyle)) as ElementStyle;
			
			style.Description = description;
			style.BackColor = backColor;
			style.BackColor2=backColor2;
			style.BackColorGradientAngle=gradientAngle;
			style.CornerDiameter=4;
			style.CornerType=eCornerType.Rounded;
			style.BorderLeft=border;
			style.BorderLeftWidth=1;
			style.BorderTop=border;
			style.BorderTopWidth=1;
			style.BorderBottom=border;
			style.BorderBottomWidth=1;
			style.BorderRight=border;
			style.BorderRightWidth=1;
			style.BorderColor=borderColor;
			style.PaddingBottom=3;
			style.PaddingLeft=3;
			style.PaddingRight=3;
			style.PaddingTop=3;
			style.TextColor=textColor;
			
			return style;
		}

		/// <summary>
		/// Returns reference to a node that is hosting given control.
		/// </summary>
		/// <param name="tree">Reference to the TreeGX control instance</param>
		/// <param name="c">Control instance to look for</param>
		/// <returns>Reference to a node hosting control or null if node could not be found</returns>
		public static Node FindNodeForControl(TreeGX tree, System.Windows.Forms.Control c)
		{
			if(tree==null || c==null || tree.Nodes.Count==0)
				return null;
			
			Node node = tree.Nodes[0];
			while(node!=null)
			{
				foreach(Cell cell in node.Cells)
				{
					if(cell.HostedControl==c)
						return node;
				}
				node = node.NextVisibleNode;
			}
			
			return null;
		}
	}
	
	#region ComponentFactory
	/// <summary>
	/// Represents internal component factory with design-time support.
	/// </summary>
	internal class ComponentFactory
	{
		private IDesignerHost m_Designer=null; 
		/// <summary>
		/// Creates new instace of the class.
		/// </summary>
		/// <param name="designer">Reference to DesignerHost to use for creation of new components.</param>
		public ComponentFactory(IDesignerHost designer)
		{
			m_Designer=designer;
		}

		/// <summary>
		/// Creates new instace of the class.
		/// </summary>
		public ComponentFactory() {}

		/// <summary>
		/// Creates component and returns reference to the new instace.
		/// </summary>
		/// <param name="type">Type that identifies component to create.</param>
		/// <returns>New instace of the component.</returns>
		public object CreateComponent(Type type)
		{
			object o=null;
			if(m_Designer!=null)
				o=m_Designer.CreateComponent(type);
			else
				o=type.Assembly.CreateInstance(type.FullName);
			return o;
		}
	}
	#endregion
}
