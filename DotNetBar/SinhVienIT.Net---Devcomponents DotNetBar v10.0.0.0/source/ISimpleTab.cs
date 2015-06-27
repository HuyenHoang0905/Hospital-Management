using System.ComponentModel;
using System.Drawing;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Represents interface for simple text only tab.
	/// </summary>
	public interface ISimpleTab
	{
		/// <summary>
		/// Gets or sets the text displayed on the tab.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),Category("Appearance"),Description("Indicates the text displayed on the tab.")]
		string Text{get;set;}

		/// <summary>
		/// Gets or sets whether tab is visible.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),DefaultValue(true),Category("Behavior"),Description("Indicates whether the tab is visible.")]
		bool Visible{get;set;}

		/// <summary>
		/// Gets the display bounds of the tab.
		/// </summary>
		[Browsable(false)]
		Rectangle DisplayRectangle{get;}

		/// <summary>
		/// Gets or sets the background color of the tab when inactive.
		/// </summary>
		[Browsable(true),Description("Indicates the inactive tab background color."),Category("Style")]
		Color BackColor{get;set;}

		/// <summary>
		/// Gets or sets the target gradient background color of the tab when inactive.
		/// </summary>
		[Browsable(true),Description("Indicates the inactive tab target gradient background color."),Category("Style")]
		Color BackColor2{get;set;}

		/// <summary>
		/// Gets or sets the gradient angle.
		/// </summary>
		[Browsable(true),Description("Indicates the gradient angle."),Category("Style"),DefaultValue(90)]
		int BackColorGradientAngle{get;set;}

		/// <summary>
		/// Gets or sets the light border color when tab is inactive.
		/// </summary>
		[Browsable(true),Description("Indicates the inactive tab light border color."),Category("Style")]
		Color LightBorderColor{get;set;}

		/// <summary>
		/// Gets or sets the dark border color when tab is inactive.
		/// </summary>
		[Browsable(true),Description("Indicates the inactive tab dark border color."),Category("Style")]
		Color DarkBorderColor{get;set;}

		/// <summary>
		/// Gets or sets the border color when tab is inactive.
		/// </summary>
		[Browsable(true),Description("Indicates the inactive tab border color."),Category("Style")]
		Color BorderColor{get;set;}

		/// <summary>
		/// Gets or sets the text color when tab is inactive.
		/// </summary>
		[Browsable(true),Description("Indicates the inactive tab text color."),Category("Style")]
		Color TextColor{get;set;}

		/// <summary>
		/// Gets or sets name of the tab item that can be used to identify item from the code.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),Category("Design"),Description("Indicates the name used to identify item.")]
		string Name{get;set;}

		/// <summary>
		/// Gets or sets the predefined tab color.
		/// </summary>
		[Browsable(true),DefaultValue(eTabItemColor.Default),Category("Style"),Description("Applies predefined color to tab.")]
		eTabItemColor PredefinedColor{get;set;}

		/// <summary>
		/// Returns the font used for tab text.
		/// </summary>
		/// <returns>Reference to font object.</returns>
		Font GetTabFont();

		/// <summary>
		/// Returns true if tab is selected tab.
		/// </summary>
		bool IsSelected{get;}

		/// <summary>
		/// Returns true if mouse is over the tab.
		/// </summary>
		bool IsMouseOver{get;}

		/// <summary>
		/// Gets the tab alignment.
		/// </summary>
		eTabStripAlignment TabAlignment{get;}
	}
}