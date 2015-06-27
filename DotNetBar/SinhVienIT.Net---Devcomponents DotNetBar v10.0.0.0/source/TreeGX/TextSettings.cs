using System;
using System.Collections.Generic;
using System.Text;

namespace DevComponents.Tree
{
    /// <summary>
    /// Defines settings for text rendering.
    /// </summary>
    internal static class TextSettings
    {
        private static int _TextMarkupCultureSpecific = 3;
        /// <summary>
        /// Get or sets the text-markup padding for text measurement when running on Japanese version of Windows.
        /// </summary>
        public static int TextMarkupCultureSpecificPadding
        {
            get { return _TextMarkupCultureSpecific; }
            set
            {
                _TextMarkupCultureSpecific = value;
            }
        }
        
    }
}
