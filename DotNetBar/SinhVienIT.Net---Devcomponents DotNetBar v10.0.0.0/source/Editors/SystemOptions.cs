#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Text;

namespace DevComponents.Editors
{
    public class SystemOptions
    {
        internal static System.Windows.Forms.ControlStyles DoubleBufferFlag
        {
            get
            {
#if FRAMEWORK20
                return System.Windows.Forms.ControlStyles.OptimizedDoubleBuffer;
#else
                    return System.Windows.Forms.ControlStyles.DoubleBuffer;
#endif
            }
        }
    }
}
#endif

