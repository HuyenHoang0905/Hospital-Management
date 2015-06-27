using System;
using System.Text;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Defines the provider for the element style classes support.
    /// </summary>
    public interface IElementStyleClassProvider
    {
        /// <summary>
        /// Returns the instance of the ElementStyle with given class name or null if there is no class with that name defined.
        /// </summary>
        /// <param name="className">Class name. See static members of ElementStyleClassKeys class for the list of available keys.</param>
        /// <returns>Instance of ElementStyle for given class name or null if class cannot be found.</returns>
        ElementStyle GetClass(string className);
    }
}
