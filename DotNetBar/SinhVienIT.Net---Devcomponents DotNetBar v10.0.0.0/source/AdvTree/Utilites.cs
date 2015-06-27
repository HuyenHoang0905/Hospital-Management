using System;
using System.ComponentModel.Design;
using System.Drawing;
using System.ComponentModel;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Rendering;
using System.Windows.Forms;

namespace DevComponents.AdvTree
{
	/// <summary>
	/// Represents class for static tree utilities.
	/// </summary>
	public class Utilities
	{
		/// <summary>
		/// Initializes control with default settings for connectors and nodes.
		/// </summary>
		/// <param name="tree">Control to initialize.</param>
		public static void InitializeTree(AdvTree tree)
		{
			InitializeTree(tree,new ComponentFactory());
		}
		
		/// <summary>
		/// Initializes control with default settings for connectors and nodes.
		/// </summary>
		/// <param name="tree">Control to initialize.</param>
		/// <param name="factory">Factory to use to create new instances of objects.</param>
		public static void InitializeTree(AdvTree tree, ComponentFactory factory)
		{
            //tree.RootConnector=factory.CreateComponent(typeof(NodeConnector)) as NodeConnector;
            //tree.RootConnector.LineWidth=1;
            //tree.RootConnector.LineColor = SystemColors.ControlText;
			tree.NodesConnector=factory.CreateComponent(typeof(NodeConnector)) as NodeConnector;
			tree.NodesConnector.LineWidth=1;
			tree.NodesConnector.LineColor=SystemColors.ControlText;
            tree.BackColor = SystemColors.Window;
			//eStyleBorderType border=eStyleBorderType.Solid;
			ElementStyle style=factory.CreateComponent(typeof(ElementStyle)) as ElementStyle;
			
			//style.BackColorSchemePart=eColorSchemePart.BarBackground;
			//style.BackColor2SchemePart=eColorSchemePart.BarBackground2;
			//style.BackColorGradientAngle=90;
			//style.CornerDiameter=4;
			//style.CornerType=eCornerType.Rounded;
			//style.BorderLeft=border;
			//style.BorderLeftWidth=1;
			//style.BorderTop=border;
			//style.BorderTopWidth=1;
			//style.BorderBottom=border;
			//style.BorderBottomWidth=1;
			//style.BorderRight=border;
			//style.BorderRightWidth=1;
			//style.BorderColorSchemePart=eColorSchemePart.BarDockedBorder;
			//style.PaddingBottom=3;
			//style.PaddingLeft=3;
			//style.PaddingRight=3;
			//style.PaddingTop=3;
            style.TextColor = SystemColors.ControlText;
			tree.Styles.Add(style);

			tree.NodeStyle=style;

            tree.BackgroundStyle.Class = ElementStyleClassKeys.TreeBorderKey;
            tree.AccessibleRole = AccessibleRole.Outline;
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
			style.CornerType=eCornerType.Square;
			style.BorderLeft=border;
			style.BorderLeftWidth=1;
			style.BorderTop=border;
			style.BorderTopWidth=1;
			style.BorderBottom=border;
			style.BorderBottomWidth=1;
			style.BorderRight=border;
			style.BorderRightWidth=1;
			style.BorderColor=borderColor;
			style.PaddingBottom=1;
			style.PaddingLeft=1;
			style.PaddingRight=1;
			style.PaddingTop=1;
			style.TextColor=textColor;
			
			return style;
		}

		/// <summary>
		/// Returns reference to a node that is hosting given control.
		/// </summary>
		/// <param name="tree">Reference to the AdvTree control instance</param>
		/// <param name="c">Control instance to look for</param>
		/// <returns>Reference to a node hosting control or null if node could not be found</returns>
		public static Node FindNodeForControl(AdvTree tree, System.Windows.Forms.Control c)
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


        internal static bool StartsWithNumber(string s)
        {
            if (s.Length > 0 && char.IsDigit(s[0]))
                return true;
            return false;
        }

        internal static int CompareAlphaNumeric(string t, string t2)
        {
            if (Utilities.StartsWithNumber(t) && Utilities.StartsWithNumber(t2))
            {
                long l = GetNumber(t), l2 = GetNumber(t2);
                int i = l.CompareTo(l2);
                if (i != 0)
                    return i;
            }
#if FRAMEWORK20
            return string.Compare(t, t2, StringComparison.CurrentCulture);
#else
            return string.Compare(t, t2);
#endif
        }

        internal static long GetNumber(string s)
        {
            long l = 0;
            int start = 0, end = 0;
            for (int i = 0; i < s.Length; i++)
            {
                if (char.IsDigit(s[i]))
                {
                    end = i;
                }
                else
                    break;
            }
#if FRAMEWORK20
            long.TryParse(s.Substring(start, end - start + 1), out l);
#else
            try
            {
                l = long.Parse(s.Substring(start, end - start + 1));
            }
            catch { }
#endif
            return l;
        }

        internal static string StripNonNumeric(string p)
        {
            string s = "";
            string decimalSeparator = NumberDecimalSeparator;
            string groupSeparator = NumberGroupSeparator;
            for (int i = 0; i < p.Length; i++)
            {
                if (p[i].ToString() == decimalSeparator || p[i].ToString() == groupSeparator || p[i] >= '0' && p[i] <= '9' || i == 0 && p[i] == '-')
                    s += p[i];
                else if (s.Length > 0) break;
            }
            return s;
        }

        private static string NumberDecimalSeparator
        {
            get 
			{
#if FRAMEWORK20
				return DevComponents.Editors.DateTimeAdv.DateTimeInput.GetActiveCulture().NumberFormat.NumberDecimalSeparator; 
#else
				return System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
#endif
			}
        }
        private static string NumberGroupSeparator
        {
            get
            {
#if FRAMEWORK20
                return DevComponents.Editors.DateTimeAdv.DateTimeInput.GetActiveCulture().NumberFormat.NumberGroupSeparator;
#else
				return System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator;
#endif
            }
        }
	}
	
	#region ComponentFactory
	/// <summary>
	/// Represents internal component factory with design-time support.
	/// </summary>
	public class ComponentFactory
	{
		private IDesignerHost m_Designer=null; 
		/// <summary>
		/// Creates new instance of the class.
		/// </summary>
		/// <param name="designer">Reference to DesignerHost to use for creation of new components.</param>
		public ComponentFactory(IDesignerHost designer)
		{
			m_Designer=designer;
		}

		/// <summary>
		/// Creates new instance of the class.
		/// </summary>
		public ComponentFactory() {}

		/// <summary>
		/// Creates component and returns reference to the new instance.
		/// </summary>
		/// <param name="type">Type that identifies component to create.</param>
		/// <returns>New instance of the component.</returns>
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

    #region Padding Class
    /// <summary>
    /// Represents class that holds padding information for user interface elements.
    /// </summary>
    public class Padding
    {
        /// <summary>
        /// Gets or sets padding on left side. Default value is 0
        /// </summary>
        public int Left = 0;
        /// <summary>
        /// Gets or sets padding on right side. Default value is 0
        /// </summary>
        public int Right = 0;
        /// <summary>
        /// Gets or sets padding on top side. Default value is 0
        /// </summary>
        public int Top = 0;
        /// <summary>
        /// Gets or sets padding on bottom side. Default value is 0
        /// </summary>
        public int Bottom = 0;

        /// <summary>
        /// Creates new instance of the class and initializes it.
        /// </summary>
        /// <param name="left">Left padding</param>
        /// <param name="right">Right padding</param>
        /// <param name="top">Top padding</param>
        /// <param name="bottom">Bottom padding</param>
        public Padding(int left, int right, int top, int bottom)
        {
            this.Left = left;
            this.Right = right;
            this.Top = top;
            this.Bottom = bottom;
        }

        /// <summary>
        /// Gets amount of horizontal padding (Left+Right)
        /// </summary>
        [Browsable(false)]
        public int Horizontal
        {
            get { return this.Left + this.Right; }
        }

        /// <summary>
        /// Gets amount of vertical padding (Top+Bottom)
        /// </summary>
        [Browsable(false)]
        public int Vertical
        {
            get { return this.Top + this.Bottom; }
        }

        /// <summary>
        /// Gets whether Padding is empty.
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return this.Left == 0 && this.Right == 0 && this.Top == 0 && this.Bottom == 0;
            }
        }
    }
    #endregion
}
