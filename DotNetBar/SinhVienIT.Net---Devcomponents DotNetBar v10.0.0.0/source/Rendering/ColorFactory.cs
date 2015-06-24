using System;
using System.Drawing;
using System.Text;

namespace DevComponents.DotNetBar.Rendering
{
    /// <summary>
    /// Represents the class used to create Color objects.
    /// </summary>
    public class ColorFactory
    {
        #region Private Variables
        #endregion

        #region Constructor
        /// <summary>
        /// Creates new instance of the object.
        /// </summary>
        public ColorFactory() { }
        #endregion

        #region Internal Implementation
        public virtual Color GetColor(int color)
        {
            return ColorScheme.GetColor(color);
        }

        public virtual Color GetColor(int alpha, int color)
        {
            return ColorScheme.GetColor(alpha, color);
        }

        public virtual Color GetColor(Color color)
        {
            return color;
        }

        public virtual Color GetColor(string argb)
        {
            return ColorScheme.GetColor(argb);
        }

        public static readonly ColorFactory Empty = new ColorFactory();
        #endregion

    }

    /// <summary>
    /// Represents the class used to create Color objects blended based on base color.
    /// </summary>
    public class ColorBlendFactory : ColorFactory
    {
        #region Private Variables
        private Color m_BlendColor = Color.Empty;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates new instance of the object.
        /// </summary>
        public ColorBlendFactory(Color blendColor)
        {
            m_BlendColor = blendColor;
        }
        #endregion

        #region Internal Implementation
        /// <summary>
        /// Converts integer color representation to Color type.
        /// </summary>
        /// <param name="rgb">Color representation as 32-bit RGB value.</param>
        /// <returns>Reference to Color object.</returns>
        public override Color GetColor(int rgb)
        {
            if (rgb == -1) return Color.Empty;
            return Color.FromArgb(SoftLight((rgb & 0xFF0000) >> 16, m_BlendColor.R), SoftLight((rgb & 0xFF00) >> 8, m_BlendColor.G), SoftLight(rgb & 0xFF, m_BlendColor.B));
        }

        /// <summary>
        /// Converts integer color representation to Color type.
        /// </summary>
        /// <param name="rgb">Color representation as 32-bit RGB value.</param>
        /// <returns>Reference to Color object.</returns>
        public override Color GetColor(int alpha, int rgb)
        {
            if (rgb == -1) return Color.Empty;
            return Color.FromArgb(alpha, SoftLight((rgb & 0xFF0000) >> 16, m_BlendColor.R), SoftLight((rgb & 0xFF00) >> 8, m_BlendColor.G), SoftLight(rgb & 0xFF, m_BlendColor.B));
        }

        /// <summary>
        /// Converts integer color representation to Color type.
        /// </summary>
        /// <param name="c">Color value.</param>
        /// <returns>Reference to Color object.</returns>
        public override Color GetColor(Color c)
        {
            if (c.IsEmpty) return Color.Empty;
            return Color.FromArgb(c.A, SoftLight(c.R, m_BlendColor.R), SoftLight(c.G, m_BlendColor.G), SoftLight(c.B, m_BlendColor.B));
        }

        /// <summary>
        /// Converts integer color representation to Color type.
        /// </summary>
        /// <param name="c">Color value.</param>
        /// <returns>Reference to Color object.</returns>
        public override Color GetColor(string argb)
        {
            return GetColor(ColorScheme.GetColor(argb));
        }

        internal static int SoftLight(int a, int b)
        {
            int c = 0;
            c = a * b / 255;
            return c + a * (255 - ((255 - a) * (255 - b) / 255) - c) / 255;
        }

        internal static Color SoftLight(Color c, Color light)
        {
            return Color.FromArgb(SoftLight(c.R, light.R), SoftLight(c.G, light.G), SoftLight(c.B, light.B));
        }
        #endregion

    }
}
