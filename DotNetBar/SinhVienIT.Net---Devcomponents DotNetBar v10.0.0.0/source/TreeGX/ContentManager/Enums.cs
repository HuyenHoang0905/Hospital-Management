using System;

#if TREEGX
namespace DevComponents.Tree.TextMarkup
#elif DOTNETBAR
namespace DevComponents.UI.ContentManager
#endif

{
	/// <summary>
	/// Specifies orientation of content.
	/// </summary>
	public enum eContentOrientation
	{
		/// <summary>
		/// Indicates Horizontal orientation of the content.
		/// </summary>
		Horizontal,
		/// <summary>
		/// Indicates Vertical orientation of the content.
		/// </summary>
		Vertical
	}

	/// <summary>
	/// Specifies content horizontal alignment.
	/// </summary>
	public enum eContentAlignment
	{
		/// <summary>
		/// Content is left aligned.UI
		/// </summary>
		Left,
		/// <summary>
		/// Content is right aligned.
		/// </summary>
		Right,
		/// <summary>
		/// Content is centered.
		/// </summary>
		Center
	}

	/// <summary>
	/// Specifies content vertical alignment.
	/// </summary>
	public enum eContentVerticalAlignment
	{
		/// <summary>
		/// Content is top aligned.
		/// </summary>
		Top,
		/// <summary>
		/// Content is bottom aligned.
		/// </summary>
		Bottom,
		/// <summary>
		/// Content is in the middle.
		/// </summary>
		Middle
	}
}
