using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Represents class for displaying BubbleButton objects on canvas.
	/// </summary>
	internal class BubbleButtonDisplay
	{
		static int BUTTON_TOOLTIP_SPACING=3;
		/// <summary>
		/// Paints button on given canvas.
		/// </summary>
		/// <param name="info">Painting information.</param>
		public static void Paint(BubbleButtonDisplayInfo info)
		{
			if(!info.Button.Visible)
				return;

			CompositeImage image=null;
			Rectangle buttonRectangle=Rectangle.Empty;
			if(info.Magnified)
			{
				buttonRectangle=info.Button.MagnifiedDisplayRectangle;
				image=GetButtonImage(info.Button,buttonRectangle.Size);
			}
			else
			{
				buttonRectangle=info.Button.DisplayRectangle;
				image=GetButtonImage(info.Button,buttonRectangle.Size);
			}

            if (buttonRectangle.Width < 2 || buttonRectangle.Height < 2) return;

			if(image!=null)
			{
				if(!info.Button.Enabled)
					image.DrawImage(info.Graphics,buttonRectangle,0,0,image.RealWidth,image.RealHeight,GraphicsUnit.Pixel,GetDisabledAttributes());
				else if(info.Button.MouseDown)
					image.DrawImage(info.Graphics,buttonRectangle,0,0,image.RealWidth,image.RealHeight,GraphicsUnit.Pixel,GetDarkAttributes());
				else
					image.DrawImage(info.Graphics,buttonRectangle);
			}
			else
				info.Graphics.DrawRectangle(SystemPens.Highlight,buttonRectangle);
            
			if(info.Button.Focus)
			{
				buttonRectangle.Inflate(1,1);
				DesignTime.DrawDesignTimeSelection(info.Graphics,buttonRectangle,Color.Navy);
			}

			DrawTooltip(info);
		}

		public static CompositeImage GetButtonImage(BubbleButton button, Size size)
		{
			CompositeImage image=null;
			if(button.Image!=null)
			{
				if(button.Image.Size==size || button.ImageLarge==null)
					image=new CompositeImage(button.Image,false,size);
				else if(button.ImageLarge!=null && (button.ImageLarge.Size==size || button.ImageLarge.Size.Height/size.Height<2))
					image=new CompositeImage(button.ImageLarge,false,size);
				else
					image=new CompositeImage(button.Image,false,size);
			}
			else if(button.ImageCached!=null)
			{
				if(button.ImageCached.Size==size || button.ImageLargeCached==null)
					image=new CompositeImage(button.ImageCached,false,size);
				else if(button.ImageLargeCached!=null && button.ImageLargeCached.Size.Height/size.Height<=2)
					image=new CompositeImage(button.ImageLargeCached,false,size);
				else
					image=new CompositeImage(button.ImageCached,false,size);
			}
//			else if(button.Icon!=null)
//			{
//				
//			}
			return image;
		}

		private static void DrawTooltip(BubbleButtonDisplayInfo info)
		{
			//float emMulti=1.3285f;

			if(info.Button.MouseOver && info.BubbleBar.ShowTooltips && info.Button.TooltipText!="")
			{
                Color textColor = info.BubbleBar.TooltipTextColor;
                Color outlineColor = info.BubbleBar.TooltipOutlineColor;

                StringFormat format = TextDrawing.GetStringFormat(eTextFormat.Default); // BarFunctions.CreateStringFormat();
				System.Drawing.Drawing2D.CompositingMode cs=info.Graphics.CompositingMode=System.Drawing.Drawing2D.CompositingMode.SourceOver;
				Font font=info.BubbleBar.TooltipFont;
				if(font==null)
					font=info.BubbleBar.Font;
				Rectangle rText=info.Button.DisplayRectangle;
                
                Size size = TextDrawing.MeasureString(info.Graphics, info.Button.TooltipText, font);

                if(info.Magnified)
					rText=info.Button.MagnifiedDisplayRectangle;
				if(info.Alignment==eBubbleButtonAlignment.Bottom)
					rText.Y-=(Math.Max(font.Height, size.Height)+BUTTON_TOOLTIP_SPACING);
				else
					rText.Y=rText.Bottom+BUTTON_TOOLTIP_SPACING;
				
				rText.Offset(-(size.Width-rText.Width)/2,0);
				Point pOutline=rText.Location;
				pOutline.Offset(-1,0);
				GraphicsPath path=new GraphicsPath();
				path.AddString(info.Button.TooltipText,font.FontFamily,(int)font.Style,font.SizeInPoints/*font.SizeInPoints*emMulti*/,new PointF((pOutline.X+1)*72/info.Graphics.DpiX,(pOutline.Y-1)*72/info.Graphics.DpiY),format);
				using(Pen pen=new Pen(outlineColor,(font.SizeInPoints>=10?1:1)))
					path.Widen(pen);
				using(SolidBrush brush=new SolidBrush(Color.FromArgb(200,outlineColor)))
				{
					GraphicsUnit pageUnit=info.Graphics.PageUnit;
					info.Graphics.PageUnit=GraphicsUnit.Point;
					info.Graphics.FillPath(brush,path);
					info.Graphics.PageUnit=pageUnit;
				}
				
				path.Dispose();
				path=new GraphicsPath();
				path.AddString(info.Button.TooltipText,font.FontFamily,(int)font.Style,font.SizeInPoints/*font.SizeInPoints*emMulti*/,new PointF(pOutline.X*72/info.Graphics.DpiX,pOutline.Y*72/info.Graphics.DpiY),format);
				path.Widen(SystemPens.ControlText);
				using(SolidBrush brush=new SolidBrush(Color.FromArgb(200,outlineColor)))
				{
					GraphicsUnit pageUnit=info.Graphics.PageUnit;
					info.Graphics.PageUnit=GraphicsUnit.Point;
					info.Graphics.FillPath(brush,path);
					info.Graphics.PageUnit=pageUnit;
				}
				path.Dispose();

				TextDrawing.DrawString(info.Graphics,info.Button.TooltipText,font,textColor,rText.X,rText.Y,eTextFormat.Default);

				info.Graphics.CompositingMode=cs;
				format.Dispose();
			}
		}

		private static ImageAttributes GetDarkAttributes()
		{
			float[][] array = new float[5][];
//			array[0] = new float[5] {1, 0, 0, 0, 0};
//			array[1] = new float[5] {0, 1, 0, 0, 0};
//			array[2] = new float[5] {0, 0, 1, 0, 0};
//			array[3] = new float[5] {0, 0, 1, 0, 0};
//			array[4] = new float[5] {.5f, .5f, .5f, 0, 1};
			array[0] = new float[5] {.65f, 0, 0, 0, 0};
			array[1] = new float[5] {0, .65f, 0, 0, 0};
			array[2] = new float[5] {0, 0, .65f, 0, 0};
			array[3] = new float[5] {0, 0, 0, 1, 0};
			array[4] = new float[5] {0, 0, 0, 0, .65f};
			ColorMatrix grayMatrix = new ColorMatrix(array);
			ImageAttributes darkImageAttr = new ImageAttributes();
			darkImageAttr.ClearColorKey();
			darkImageAttr.SetColorMatrix(grayMatrix);
			return darkImageAttr;
		}

		private static ImageAttributes GetDisabledAttributes()
		{
			float[][] array = new float[5][];
			array[0] = new float[5] {0.5f,  0.5f,  0.5f,  0.0f,  0.0f};
			array[1] = new float[5] {0.5f,  0.5f,  0.5f,  0.0f,  0.0f};
			array[2] = new float[5] {0.5f,  0.5f,  0.5f,  0.0f,  0.0f};
			array[3] = new float[5] {0.0f,  0.0f,  0.0f,  1.0f,  0.0f};
			array[4] = new float[5] {0.0f,  0.0f,  0.0f,  0.0f,  1.0f};
			//ColorMatrix grayMatrix = new ColorMatrix(array);
			ColorMatrix grayMatrix = new ColorMatrix();
			grayMatrix.Matrix00 = 1/3f; 
			grayMatrix.Matrix01 = 1/3f; 
			grayMatrix.Matrix02 = 1/3f; 
			grayMatrix.Matrix10 = 1/3f; 
			grayMatrix.Matrix11 = 1/3f; 
			grayMatrix.Matrix12 = 1/3f; 
			grayMatrix.Matrix20 = 1/3f; 
			grayMatrix.Matrix21 = 1/3f; 
			grayMatrix.Matrix22 = 1/3f;
			grayMatrix.Matrix33 = .5f; // Alpha-channel

			ImageAttributes darkImageAttr = new ImageAttributes();
			darkImageAttr.ClearColorKey();
			darkImageAttr.SetColorMatrix(grayMatrix);
			return darkImageAttr;
		}
	}

	#region BubbleButtonDisplayInfo
	/// <summary>
	/// Represents class that holds information for BubbleButton painting.
	/// </summary>
	public class BubbleButtonDisplayInfo
	{
		/// <summary>
		/// Graphics object.
		/// </summary>
		public Graphics Graphics=null;
		/// <summary>
		/// Button to paint.
		/// </summary>
		public BubbleButton Button=null;
		/// <summary>
		/// Reference to BubbleBar control.
		/// </summary>
		public BubbleBar BubbleBar=null;
		/// <summary>
		/// Gets or sets whether magnified version of the button is painted.
		/// </summary>
		public bool Magnified=false;
		/// <summary>
		/// Gets or sets the button alignment inside of the bar.
		/// </summary>
		public eBubbleButtonAlignment Alignment=eBubbleButtonAlignment.Bottom;
	}
	#endregion
}
