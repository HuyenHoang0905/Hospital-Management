using System.Drawing;
using System.Drawing.Drawing2D;

#if TREEGX
namespace DevComponents.Tree
#elif DOTNETBAR
namespace DevComponents.DotNetBar
#endif
{
	/// <summary>
	/// Summary description for Display.
	/// </summary>
	internal class Display
	{
		private Display()
		{
		}

		public static LinearGradientBrush CreateLinearGradientBrush(Rectangle r,Color color1, Color color2,float gradientAngle)
		{
			if(r.Width<=0)
				r.Width=1;
			if(r.Height<=0)
				r.Height=1;
			return new LinearGradientBrush(new Rectangle(r.X,r.Y-1,r.Width,r.Height+1),color1,color2,gradientAngle);
		}

		public static LinearGradientBrush CreateLinearGradientBrush(RectangleF r,Color color1, Color color2,float gradientAngle)
		{
			if(r.Width<=0)
				r.Width=1;
			if(r.Height<=0)
				r.Height=1;
			return new LinearGradientBrush(new RectangleF(r.X,r.Y-1,r.Width,r.Height+1),color1,color2,gradientAngle);
		}

		public static LinearGradientBrush CreateLinearGradientBrush(Rectangle r,Color color1, Color color2,float gradientAngle, bool isAngleScalable)
		{
			if(r.Width<=0)
				r.Width=1;
			if(r.Height<=0)
				r.Height=1;
			return new LinearGradientBrush(new Rectangle(r.X,r.Y-1,r.Width,r.Height+1),color1,color2,gradientAngle,isAngleScalable);
		}

		public static Rectangle GetDrawRectangle(Rectangle r)
		{
			r.Width--;
			r.Height--;
			return r;
		}

		public static Rectangle GetPathRectangle(Rectangle r)
		{
			//r.Width++;
			//r.Height++;
			return r;
		}
	}
}
