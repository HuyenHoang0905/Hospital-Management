using System;
using System.Drawing;

namespace DevComponents.Instrumentation.Primitives
{
    public class ColorFactory
    {
        /// <summary>
		/// Converts hex string to Color type.
		/// </summary>
		/// <param name="rgbHex">Hexadecimal color representation.</param>
		/// <returns>Reference to Color object.</returns>
		public static Color GetColor(string rgbHex)
        {
            if (string.IsNullOrEmpty(rgbHex) == false)
            {
                if (rgbHex.Length == 8)
                {
                    return (Color.FromArgb(Convert.ToInt32(rgbHex.Substring(0, 2), 16),
                                           Convert.ToInt32(rgbHex.Substring(2, 2), 16),
                                           Convert.ToInt32(rgbHex.Substring(4, 2), 16),
                                           Convert.ToInt32(rgbHex.Substring(6, 2), 16)));
                }

                return (Color.FromArgb(Convert.ToInt32(rgbHex.Substring(0, 2), 16),
                                       Convert.ToInt32(rgbHex.Substring(2, 2), 16),
                                       Convert.ToInt32(rgbHex.Substring(4, 2), 16)));
            }

            return Color.Empty;
        }

        /// <summary>
        /// Converts hex string to Color type.
        /// </summary>
        /// <param name="rgb">Color representation as 32-bit RGB value.</param>
        /// <returns>Reference to Color object.</returns>
        public static Color GetColor(int rgb)
        {
            return ((rgb == -1) ? Color.Empty :
                Color.FromArgb((rgb & 0xFF0000) >> 16, (rgb & 0xFF00) >> 8, rgb & 0xFF));
        }

        /// <summary>
        /// Converts hex string to Color type.
        /// </summary>
        /// <param name="alpha"></param>
        /// <param name="rgb">Color representation as 32-bit RGB value.</param>
        /// <returns>Reference to Color object.</returns>
        public static Color GetColor(int alpha, int rgb)
        {
            return ((rgb == -1) ? Color.Empty :
                Color.FromArgb(alpha, (rgb & 0xFF0000) >> 16, (rgb & 0xFF00) >> 8, rgb & 0xFF));
        }
    }
}
