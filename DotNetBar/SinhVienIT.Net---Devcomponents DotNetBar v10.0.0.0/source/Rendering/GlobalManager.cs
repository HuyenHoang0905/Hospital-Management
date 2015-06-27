using System;
using System.Text;

namespace DevComponents.DotNetBar.Rendering
{
    /// <summary>
    /// Represents a static class that maintains the global rendering properties for all controls when eRenderMode is set to global.
    /// </summary>
    public class GlobalManager
    {
        private static BaseRenderer m_Renderer = null;

        /// <summary>
        /// Gets or sets the global renderer used by all controls that have RenderMode set to eRenderMode.Global.
        /// </summary>
        public static BaseRenderer Renderer
        {
            get
            {
                if (m_Renderer == null)
                    m_Renderer = new Office2007Renderer();

                return m_Renderer;
            }
            set { m_Renderer = value; }
        }
        
    }
}
