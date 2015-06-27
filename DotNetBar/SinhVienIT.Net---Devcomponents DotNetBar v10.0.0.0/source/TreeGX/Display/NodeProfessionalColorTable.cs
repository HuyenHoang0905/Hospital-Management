using System.Drawing;

namespace DevComponents.Tree.Display
{
	/// <summary>
	/// Describes colors used by Professional Renderer.
	/// </summary>
	public class NodeProfessionalColorTable
	{
		/// <summary>
		/// Gets or sets the gradient begin color for top part of background.
		/// </summary>
		public Color NodeTopGradientBegin = ColorScheme.GetColor("B0CCF9");
		/// <summary>
		/// Gets or sets the middle gradient color for top part of background.
		/// </summary>
		public Color NodeTopGradientMiddle = ColorScheme.GetColor("E2EDFF"); // Color.FromArgb(242, 245, 250);
		/// <summary>
		/// Gets or sets the end gradient color for top part of background.
		/// </summary>
		public Color NodeTopGradientEnd = Color.Empty;
		/// <summary>
		/// Gets or sets the end gradient type for top part of background.
		/// </summary>
		public eGradientType NodeTopGradientType = eGradientType.Linear;
		/// <summary>
		/// Gets or sets the end gradient angle for top part of background.
		/// </summary>
		public int NodeTopGradientAngle = 90;

		/// <summary>
		/// Gets or sets the starting gradient color for bottom part of background.
		/// </summary>
		public Color NodeBottomGradientBegin = ColorScheme.GetColor("75ABFF");
		/// <summary>
		/// Gets or sets the middle gradient color for bottom part of background.
		/// </summary>
		public Color NodeBottomGradientMiddle = Color.FromArgb(208, 242, 248);
		/// <summary>
		/// Gets or sets the end gradient color for bottom part of background.
		/// </summary>
		public Color NodeBottomGradientEnd = Color.Empty;
		/// <summary>
		/// Gets or sets the type of the gradient for bottom part of background.
		/// </summary>
		public eGradientType NodeBottomGradientType = eGradientType.Linear;
		/// <summary>
		/// Gets or sets the gradient angle for bottom part of background.
		/// </summary>
		public int NodeBottomGradientAngle = 90;

		/// <summary>
		/// Gets or sets the starting gradient color for top part of background when mouse is over the node.
		/// </summary>
		public Color NodeMouseOverTopGradientBegin = ColorScheme.GetColor("B0CCF9");
		/// <summary>
		/// Gets or sets the middle gradient color for top part of background when mouse is over the node.
		/// </summary>
		public Color NodeMouseOverTopGradientMiddle = Color.White;
		/// <summary>
		/// Gets or sets the end gradient color for top part of background when mouse is over the node.
		/// </summary>
		public Color NodeMouseOverTopGradientEnd = Color.Empty;
		/// <summary>
		/// Gets or sets the gradient type for top part of background when mouse is over the node.
		/// </summary>
		public eGradientType NodeMouseOverTopGradientType = eGradientType.Linear;
		/// <summary>
		/// Gets or sets the gradient angle for top part of background when mouse is over the node.
		/// </summary>
		public int NodeMouseOverTopGradientAngle = 90;

		/// <summary>
		/// Gets or sets the starting gradient color for bottom part of background when mouse is over the node.
		/// </summary>
		public Color NodeMouseOverBottomGradientBegin = ColorScheme.GetColor("6599EA");
		/// <summary>
		/// Gets or sets the middle gradient color for bottom part of background when mouse is over the node.
		/// </summary>
		public Color NodeMouseOverBottomGradientMiddle = Color.Cyan;
		/// <summary>
		/// Gets or sets the end gradient color for bottom part of background when mouse is over the node.
		/// </summary>
		public Color NodeMouseOverBottomGradientEnd = Color.Empty;
		/// <summary>
		/// Gets or sets the gradient type for bottom part of background when mouse is over the node.
		/// </summary>
		public eGradientType NodeMouseOverBottomGradientType = eGradientType.Linear;
		/// <summary>
		/// Gets or sets the gradient angle for bottom part of background when mouse is over the node.
		/// </summary>
		public int NodeMouseOverBottomGradientAngle = 90;

//		public Color NodeTopGradientBegin=ColorScheme.GetColor("BDCCD9");
//		public Color NodeTopGradientMiddle=Color.White;
//		public Color NodeTopGradientEnd=ColorScheme.GetColor("406986");
//		
//		public Color NodeBottomGradientBegin=ColorScheme.GetColor("03436D");
//		public Color NodeBottomGradientMiddle=ColorScheme.GetColor("0BC0E9");
//		public Color NodeBottomGradientEnd=ColorScheme.GetColor("0C84C1");
	}
}