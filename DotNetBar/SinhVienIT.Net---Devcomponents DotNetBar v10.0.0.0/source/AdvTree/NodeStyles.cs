using System;
using System.ComponentModel;
using System.Drawing;
using DevComponents.DotNetBar;

namespace DevComponents.AdvTree
{
	/// <summary>
	/// Class that provides predefined styles for the nodes. Styles are defined as static memeber of the class
	/// </summary>
	public class NodeStyles
	{
		private static ElementStyle s_AppleStyle=null;
		private static ElementStyle s_BlueStyle=null;
		private static ElementStyle s_BlueLightStyle=null;
		private static ElementStyle s_BlueNightStyle=null;
		private static ElementStyle s_BlueMistStyle=null;
		private static ElementStyle s_CyanStyle=null;
		private static ElementStyle s_GreenStyle=null;
		private static ElementStyle s_LemonStyle=null;
		private static ElementStyle s_MagentaStyle=null;
		private static ElementStyle s_OrangeStyle=null;
		private static ElementStyle s_OrangeLightStyle=null;
		private static ElementStyle s_PurpleStyle=null;
		private static ElementStyle s_PurpleMistStyle=null;
		private static ElementStyle s_RedStyle=null;
		private static ElementStyle s_SilverStyle=null;
		private static ElementStyle s_SilverMistStyle=null;
		private static ElementStyle s_TanStyle=null;
		private static ElementStyle s_TealStyle=null;
		private static ElementStyle s_YellowStyle=null;
		private static ElementStyle s_GrayStyle=null;
		
		/// <summary>
		/// Returns Apple element style
		/// </summary>
		public static ElementStyle Apple
		{
			get
			{
				if(s_AppleStyle==null)
					s_AppleStyle = GetStyle(ePredefinedElementStyle.Apple);
				return s_AppleStyle;
			}	
		}
		
		/// <summary>
		/// Returns Blue element style
		/// </summary>
		public static ElementStyle Blue
		{
			get
			{
				if(s_BlueStyle==null)
					s_BlueStyle= GetStyle(ePredefinedElementStyle.Blue);
				return s_BlueStyle;
			}	
		}
		
		/// <summary>
		/// Returns BlueLight element style
		/// </summary>
		public static ElementStyle BlueLight
		{
			get
			{
				if(s_BlueLightStyle==null)
					s_BlueLightStyle= GetStyle(ePredefinedElementStyle.BlueLight);
				return s_BlueLightStyle;
			}	
		}
		
		/// <summary>
		/// Returns BlueNight element style
		/// </summary>
		public static ElementStyle BlueNight
		{
			get
			{
				if(s_BlueNightStyle==null)
					s_BlueNightStyle= GetStyle(ePredefinedElementStyle.BlueNight);
				return s_BlueNightStyle;
			}	
		}
		
		/// <summary>
		/// Returns BlueMist element style
		/// </summary>
		public static ElementStyle BlueMist
		{
			get
			{
				if(s_BlueMistStyle==null)
					s_BlueMistStyle= GetStyle(ePredefinedElementStyle.BlueMist);
				return s_BlueMistStyle;
			}	
		}
		
		/// <summary>
		/// Returns Cyan element style
		/// </summary>
		public static ElementStyle Cyan
		{
			get
			{
				if(s_CyanStyle==null)
					s_CyanStyle= GetStyle(ePredefinedElementStyle.Cyan);
				return s_CyanStyle;
			}	
		}
		
		/// <summary>
		/// Returns Green element style
		/// </summary>
		public static ElementStyle Green
		{
			get
			{
				if(s_GreenStyle==null)
					s_GreenStyle= GetStyle(ePredefinedElementStyle.Green);
				return s_GreenStyle;
			}	
		}
		
		/// <summary>
		/// Returns Lemon element style
		/// </summary>
		public static ElementStyle Lemon
		{
			get
			{
				if(s_LemonStyle==null)
					s_LemonStyle= GetStyle(ePredefinedElementStyle.Lemon);
				return s_LemonStyle;
			}	
		}
		
		/// <summary>
		/// Returns Magenta element style
		/// </summary>
		public static ElementStyle Magenta
		{
			get
			{
				if(s_MagentaStyle==null)
					s_MagentaStyle= GetStyle(ePredefinedElementStyle.Magenta);
				return s_MagentaStyle;
			}	
		}
		
		/// <summary>
		/// Returns Orange element style
		/// </summary>
		public static ElementStyle Orange
		{
			get
			{
				if(s_OrangeStyle==null)
					s_OrangeStyle= GetStyle(ePredefinedElementStyle.Orange);
				return s_OrangeStyle;
			}	
		}
		
		/// <summary>
		/// Returns OrangeLight element style
		/// </summary>
		public static ElementStyle OrangeLight
		{
			get
			{
				if(s_OrangeLightStyle==null)
					s_OrangeLightStyle= GetStyle(ePredefinedElementStyle.OrangeLight);
				return s_OrangeLightStyle;
			}	
		}
		
		/// <summary>
		/// Returns Purple element style
		/// </summary>
		public static ElementStyle Purple
		{
			get
			{
				if(s_PurpleStyle==null)
					s_PurpleStyle= GetStyle(ePredefinedElementStyle.Purple);
				return s_PurpleStyle;
			}	
		}
		
		/// <summary>
		/// Returns PurpleMist element style
		/// </summary>
		public static ElementStyle PurpleMist
		{
			get
			{
				if(s_PurpleMistStyle==null)
					s_PurpleMistStyle= GetStyle(ePredefinedElementStyle.PurpleMist);
				return s_PurpleMistStyle;
			}	
		}
		
		/// <summary>
		/// Returns Red element style
		/// </summary>
		public static ElementStyle Red
		{
			get
			{
				if(s_RedStyle==null)
					s_RedStyle= GetStyle(ePredefinedElementStyle.Red);
				return s_RedStyle;
			}	
		}
		
		/// <summary>
		/// Returns Silver element style
		/// </summary>
		public static ElementStyle Silver
		{
			get
			{
				if(s_SilverStyle==null)
					s_SilverStyle= GetStyle(ePredefinedElementStyle.Silver);
				return s_SilverStyle;
			}	
		}
		
		/// <summary>
		/// Returns SilverMist element style
		/// </summary>
		public static ElementStyle SilverMist
		{
			get
			{
				if(s_SilverMistStyle==null)
					s_SilverMistStyle= GetStyle(ePredefinedElementStyle.SilverMist);
				return s_SilverMistStyle;
			}	
		}
		
		/// <summary>
		/// Returns Tan element style
		/// </summary>
		public static ElementStyle Tan
		{
			get
			{
				if(s_TanStyle==null)
					s_TanStyle= GetStyle(ePredefinedElementStyle.Tan);
				return s_TanStyle;
			}	
		}
		
		/// <summary>
		/// Returns Teal element style
		/// </summary>
		public static ElementStyle Teal
		{
			get
			{
				if(s_TealStyle==null)
					s_TealStyle= GetStyle(ePredefinedElementStyle.Teal);
				return s_TealStyle;
			}	
		}
		
		/// <summary>
		/// Returns Yellow element style
		/// </summary>
		public static ElementStyle Yellow
		{
			get
			{
				if(s_YellowStyle==null)
					s_YellowStyle= GetStyle(ePredefinedElementStyle.Yellow);
				return s_YellowStyle;
			}	
		}
		
		/// <summary>
		/// Returns Gray element style
		/// </summary>
		public static ElementStyle Gray
		{
			get
			{
				if(s_GrayStyle==null)
					s_GrayStyle= GetStyle(ePredefinedElementStyle.Gray);
				return s_GrayStyle;
			}	
		}
		
		private static ElementStyle GetStyle(ePredefinedElementStyle c)
		{
			Color color1=Color.Empty;
			Color color2=Color.Empty;
			int gradientAngle = 90;
			Color textColor=Color.Black;
			Color borderColor = Color.DarkGray;
			
			switch (c)
			
			{
				case ePredefinedElementStyle.Apple:
				{
					color1 = Color.FromArgb(232, 248, 224);
					color2 = Color.FromArgb(173, 231, 146);
					break;
				}
				case ePredefinedElementStyle.Blue:
				{
					color1 = Color.FromArgb(221, 230, 247);
					color2 = Color.FromArgb(138, 168, 228);
					break;
				}
				case ePredefinedElementStyle.BlueLight:
				{
					color1=Color.FromArgb(255,255,255);
					color2=Color.FromArgb(210,224,252);
					textColor=Color.FromArgb(69,84,115);
					break;
				}
				case ePredefinedElementStyle.BlueMist:
				{
					color1 = Color.FromArgb(227, 236, 243);
					color2 = Color.FromArgb(155, 187, 210);
					break;
				}
				case ePredefinedElementStyle.BlueNight:
				{
					color1=Color.FromArgb(77,108,152);
					color2=Color.Navy;
					textColor=Color.White;
					borderColor=Color.Navy;
					break;
				}
				case ePredefinedElementStyle.Cyan:
				{
					color1 = Color.FromArgb(227, 236, 243);
					color2 = Color.FromArgb(155, 187, 210);
					break;
				}
				case ePredefinedElementStyle.Green:
				{
					color1 = Color.FromArgb(234, 240, 226);
					color2 = Color.FromArgb(183, 201, 151);
					break;
				}
				case ePredefinedElementStyle.Lemon:
				{
					color1 = Color.FromArgb(252, 253, 215);
					color2 = Color.FromArgb(245, 249, 111);
					break;
				}
				case ePredefinedElementStyle.Magenta:
				{
					color1 = Color.FromArgb(243, 229, 236);
					color2 = Color.FromArgb(213, 164, 187);
					break;
				}
				case ePredefinedElementStyle.Orange:
				{
					color1 = Color.FromArgb(252, 233, 217);
					color2 = Color.FromArgb(246, 176, 120);
					break;
				}
				case ePredefinedElementStyle.OrangeLight:
				{
					color1=Color.FromArgb(255,239,201);
					color2=Color.FromArgb(242,210,132);
					textColor=Color.FromArgb(117,83,2);
					break;
				}
				case ePredefinedElementStyle.Purple:
				{
					color1 = Color.FromArgb(234, 227, 245);
					color2 = Color.FromArgb(180, 158, 222);
					break;
				}
				case ePredefinedElementStyle.PurpleMist:
				{
					color1 = Color.FromArgb(232, 227, 234);
					color2 = Color.FromArgb(171, 156, 183);
					break;
				}
				case ePredefinedElementStyle.Red:
				{
					color1 = Color.FromArgb(249, 225, 226);
					color2 = Color.FromArgb(238, 149, 151);
					break;
				}
				case ePredefinedElementStyle.Silver:
				{
					color1 = Color.FromArgb(225, 225, 232);
					color2 = Color.FromArgb(149, 149, 170);
					break;
				}
				case ePredefinedElementStyle.SilverMist:
				{
					color1 = Color.FromArgb(243,244,250);
					color2=Color.FromArgb(155,153,183);
					textColor=Color.FromArgb(87,86,113);
					break;
				}
				case ePredefinedElementStyle.Tan:
				{
					color1 = Color.FromArgb(248, 242, 226);
					color2 = Color.FromArgb(232, 209, 153);
					break;
				}
				case ePredefinedElementStyle.Teal:
				{
					color1 = Color.FromArgb(205, 236, 240);
					color2 = Color.FromArgb(78, 188, 202);
					break;
				}
				case ePredefinedElementStyle.Yellow:
				{
					color1 = Color.FromArgb(255, 244, 213);
					color2 = Color.FromArgb(255, 216, 105);
					break;
				}
				case ePredefinedElementStyle.Gray:
				{
					color1 = Color.White;
					color2 = ColorScheme.GetColor("E4E4F0");
					break;
				}
			}
			
			ElementStyle style=Utilities.CreateStyle(new ComponentFactory(),Enum.GetName(typeof(ePredefinedElementStyle),c),borderColor,color1, color2, gradientAngle,textColor);
			return style;
		}
	}
	
	/// <summary>
	/// Indicates predefined element style.
	/// </summary>
	public enum ePredefinedElementStyle
	{
		Blue,
		BlueLight,
		BlueNight,
		Yellow,
		Green,
		Red,
		Purple,
		Cyan,
		Orange,
		OrangeLight,
		Magenta,
		BlueMist,
		PurpleMist,
		Tan,
		Lemon,
		Apple,
		Teal,
		Silver,
		SilverMist,
		Gray
	}
}
