using System;
using System.Drawing;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Summary description for ISimpleElement.
	/// </summary>
	public interface ISimpleElement
	{
		Rectangle Bounds {get;set;}
		int FixedWidth {get;set;}
		bool ImageVisible {get;}
		Size ImageLayoutSize {get;}
		eSimplePartAlignment ImageAlignment {get;set;}
		Rectangle ImageBounds {get;set;}
		int ImageTextSpacing {get;}

		bool TextVisible {get;}
		string Text {get;set;}
		Rectangle TextBounds {get;set;}
		System.Drawing.Image Image {get;set;}
	}
}

