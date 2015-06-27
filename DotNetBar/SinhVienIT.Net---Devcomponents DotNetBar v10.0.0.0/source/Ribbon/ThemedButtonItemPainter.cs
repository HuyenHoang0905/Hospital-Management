using System;
using System.Drawing;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Summary description for ThemedButtonItemPainter.
	/// </summary>
	public class ThemedButtonItemPainter
	{
		public static void PaintButton(ButtonItem button, ItemPaintArgs pa)
		{
			System.Drawing.Graphics g=pa.Graphics;
			ThemeToolbar theme=pa.ThemeToolbar;
			ThemeToolbarParts part=ThemeToolbarParts.Button;
			ThemeToolbarStates state=ThemeToolbarStates.Normal;
			Color textColor=ButtonItemPainterHelper.GetTextColor(button,pa);

			Rectangle rectImage=Rectangle.Empty;
			Rectangle itemRect=button.DisplayRectangle;
			
			Font font=null;
			CompositeImage image=button.GetImage();

			font=button.GetFont(pa, false);

			eTextFormat format= GetStringFormat(button, pa, image);

			bool bSplitButton=(button.SubItems.Count>0 || button.PopupType==ePopupType.Container) && button.ShowSubItems && !button.SubItemsRect.IsEmpty;

			if(bSplitButton)
				part=ThemeToolbarParts.SplitButton;

			// Calculate image position
			if(image!=null)
			{
				if(button.ImagePosition==eImagePosition.Top || button.ImagePosition==eImagePosition.Bottom)
					rectImage=new Rectangle(button.ImageDrawRect.X,button.ImageDrawRect.Y,itemRect.Width,button.ImageDrawRect.Height);
				else
					rectImage=new Rectangle(button.ImageDrawRect.X,button.ImageDrawRect.Y,button.ImageDrawRect.Width,button.ImageDrawRect.Height);

				rectImage.Offset(itemRect.Left,itemRect.Top);
				rectImage.Offset((rectImage.Width-button.ImageSize.Width)/2,(rectImage.Height-button.ImageSize.Height)/2);
				rectImage.Width=button.ImageSize.Width;
				rectImage.Height=button.ImageSize.Height;
			}

			// Set the state and text brush
			if(!ButtonItemPainter.IsItemEnabled(button, pa))
			{
				state=ThemeToolbarStates.Disabled;
			}
			else if(button.IsMouseDown)
			{
				state=ThemeToolbarStates.Pressed;
			}
			else if(button.IsMouseOver && button.Checked)
			{
				state=ThemeToolbarStates.HotChecked;
			}
			else if(button.IsMouseOver || button.Expanded)
			{
				state=ThemeToolbarStates.Hot;
			}
			else if(button.Checked)
			{
				state=ThemeToolbarStates.Checked;
			}
			
			Rectangle backRect=button.DisplayRectangle;
			if(button.HotTrackingStyle==eHotTrackingStyle.Image && image!=null)
			{
				backRect=rectImage;
				backRect.Inflate(3,3);
			}
			else if(bSplitButton)
			{
				backRect.Width=backRect.Width-button.SubItemsRect.Width;
			}

			// Draw Button Background
			if(button.HotTrackingStyle!=eHotTrackingStyle.None)
			{
				theme.DrawBackground(g,part,state,backRect);
			}

			// Draw Image
			if(image!=null && button.ButtonStyle!=eButtonStyle.TextOnlyAlways)
			{
				if(state==ThemeToolbarStates.Normal && button.HotTrackingStyle==eHotTrackingStyle.Color)
				{
					// Draw gray-scale image for this hover style...
					float[][] array = new float[5][];
					array[0] = new float[5] {0.2125f, 0.2125f, 0.2125f, 0, 0};
					array[1] = new float[5] {0.5f, 0.5f, 0.5f, 0, 0};
					array[2] = new float[5] {0.0361f, 0.0361f, 0.0361f, 0, 0};
					array[3] = new float[5] {0,       0,       0,       1, 0};
					array[4] = new float[5] {0.2f,    0.2f,    0.2f,    0, 1};
					System.Drawing.Imaging.ColorMatrix grayMatrix = new System.Drawing.Imaging.ColorMatrix(array);
					System.Drawing.Imaging.ImageAttributes att = new System.Drawing.Imaging.ImageAttributes();
					att.SetColorMatrix(grayMatrix);
					//g.DrawImage(image,rectImage,0,0,image.Width,image.Height,GraphicsUnit.Pixel,att);
					image.DrawImage(g,rectImage,0,0,image.Width,image.Height,GraphicsUnit.Pixel,att);
				}
				else if(state==ThemeToolbarStates.Normal && !image.IsIcon)
				{
					// Draw image little bit lighter, I decied to use gamma it is easy
					System.Drawing.Imaging.ImageAttributes lightImageAttr = new System.Drawing.Imaging.ImageAttributes();
					lightImageAttr.SetGamma(.7f,System.Drawing.Imaging.ColorAdjustType.Bitmap);
					//g.DrawImage(image,rectImage,0,0,image.Width,image.Height,GraphicsUnit.Pixel,lightImageAttr);
					image.DrawImage(g,rectImage,0,0,image.Width,image.Height,GraphicsUnit.Pixel,lightImageAttr);
				}
				else
				{
					image.DrawImage(g,rectImage);
				}
			}

			// Draw Text
			if(button.ButtonStyle==eButtonStyle.ImageAndText || button.ButtonStyle==eButtonStyle.TextOnlyAlways || image==null)
			{
				Rectangle rectText=button.TextDrawRect;
				if(button.ImagePosition==eImagePosition.Top || button.ImagePosition==eImagePosition.Bottom)
				{
					if(button.Orientation==eOrientation.Vertical)
					{
						rectText=new Rectangle(button.TextDrawRect.X,button.TextDrawRect.Y,button.TextDrawRect.Width,button.TextDrawRect.Height);
					}
					else
					{
						rectText=new Rectangle(button.TextDrawRect.X,button.TextDrawRect.Y,itemRect.Width,button.TextDrawRect.Height);
						if((button.SubItems.Count>0 || button.PopupType==ePopupType.Container) && button.ShowSubItems)
							rectText.Width-=10;
					}
					format|=eTextFormat.HorizontalCenter;
				}

				rectText.Offset(itemRect.Left,itemRect.Top);

				if(button.Orientation==eOrientation.Vertical)
				{
					g.RotateTransform(90);
					TextDrawing.DrawStringLegacy(g,ButtonItemPainter.GetDrawText(button.Text),font,textColor,new Rectangle(rectText.Top,-rectText.Right,rectText.Height,rectText.Width),format);
					g.ResetTransform();
				}
				else
				{
					if(rectText.Right>button.DisplayRectangle.Right)
						rectText.Width=button.DisplayRectangle.Right-rectText.Left;
					TextDrawing.DrawString(g,ButtonItemPainter.GetDrawText(button.Text),font,textColor,rectText,format);
					if(!button.DesignMode && button.Focused && !pa.IsOnMenu && !pa.IsOnMenuBar)
					{
						//SizeF szf=g.MeasureString(m_Text,font,rectText.Width,format);
						Rectangle r=rectText;
						//r.Width=(int)Math.Ceiling(szf.Width);
						//r.Height=(int)Math.Ceiling(szf.Height);
						//r.Inflate(1,1);
						System.Windows.Forms.ControlPaint.DrawFocusRectangle(g,r);
					}
				}				
			}

			// If it has subitems draw the triangle to indicate that
			if(bSplitButton)
			{
				part=ThemeToolbarParts.SplitButtonDropDown;
				
				if(!ButtonItemPainter.IsItemEnabled(button, pa))
					state=ThemeToolbarStates.Disabled;
				else
					state=ThemeToolbarStates.Normal;

				if(button.HotTrackingStyle!=eHotTrackingStyle.None && button.HotTrackingStyle!=eHotTrackingStyle.Image && ButtonItemPainter.IsItemEnabled(button, pa))
				{
					if(button.Expanded || button.IsMouseDown)
						state=ThemeToolbarStates.Pressed;
					else if(button.IsMouseOver && button.Checked)
						state=ThemeToolbarStates.HotChecked;
					else if(button.Checked)
						state=ThemeToolbarStates.Checked;
					else if(button.IsMouseOver)
						state=ThemeToolbarStates.Hot;
				}

                if (!button.AutoExpandOnClick)
                {
                    if (button.Orientation == eOrientation.Horizontal)
                    {
                        Rectangle r = button.SubItemsRect;
                        r.Offset(itemRect.X, itemRect.Y);
                        theme.DrawBackground(g, part, state, r);
                    }
                    else
                    {
                        Rectangle r = button.SubItemsRect;
                        r.Offset(itemRect.X, itemRect.Y);
                        theme.DrawBackground(g, part, state, r);
                    }
                }
			}

			if(button.Focused && button.DesignMode)
			{
				Rectangle r=itemRect;
				r.Inflate(-1,-1);
				DesignTime.DrawDesignTimeSelection(g,r,pa.Colors.ItemDesignTimeBorder);
			}

			if(image!=null)
				image.Dispose();
		}

		private static eTextFormat GetStringFormat(ButtonItem button, ItemPaintArgs pa, CompositeImage image)
		{
			eTextFormat stringFormat=pa.ButtonStringFormat;
			bool isOnMenu = IsOnMenu(button, pa);
			if(!isOnMenu)
			{
				if(button.ContainerControl is RibbonStrip && (image==null || button.ImagePosition==eImagePosition.Top || button.ImagePosition==eImagePosition.Bottom))
					stringFormat |= eTextFormat.HorizontalCenter;
				else if(pa.IsOnMenuBar || pa.ContainerControl is Bar && image==null || button.ImagePosition==eImagePosition.Top || button.ImagePosition==eImagePosition.Bottom)
					stringFormat |= eTextFormat.HorizontalCenter;
			}
			return stringFormat;
		}

		private static bool IsOnMenu(ButtonItem button, ItemPaintArgs pa)
		{
			bool isOnMenu = pa.IsOnMenu;
			if (isOnMenu && button.Parent is ItemContainer)
				isOnMenu = false;
			return isOnMenu;
		}
	}
}
