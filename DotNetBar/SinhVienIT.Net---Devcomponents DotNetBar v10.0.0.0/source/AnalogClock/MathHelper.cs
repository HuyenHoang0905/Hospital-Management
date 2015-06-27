using System;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Math helper class
    /// </summary>
    internal static class MathHelper
    {
        /// <summary>
        /// Converts radians to degrees.
        /// </summary>
        /// <param name="radians">Value to be converted in radians.</param>
        /// <returns>Converted value in degrees.</returns>
        public static double GetDegrees(double radians)
        {
            return radians * (180.0 / Math.PI);
        }

        /// <summary>
        /// Converts degrees to radians.
        /// </summary>
        /// <param name="degrees">Value to be converted in degrees.</param>
        /// <returns>Converted value in radians.</returns>
        public static double GetRadians(double degrees)
        {
            return degrees * (Math.PI / 180.0);
        }
    }
}
