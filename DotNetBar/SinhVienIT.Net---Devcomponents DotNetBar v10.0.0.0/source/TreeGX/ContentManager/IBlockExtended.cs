using System;
using System.Text;

#if TREEGX
namespace DevComponents.Tree.TextMarkup
#elif DOTNETBAR
namespace DevComponents.UI.ContentManager
#endif

{
    /// <summary>
    /// Represents a extended content block interface for advanced layout information.
    /// </summary>
    public interface IBlockExtended : IBlock
    {
        bool IsBlockElement { get;}
        bool IsNewLineAfterElement { get;}
    }
}
