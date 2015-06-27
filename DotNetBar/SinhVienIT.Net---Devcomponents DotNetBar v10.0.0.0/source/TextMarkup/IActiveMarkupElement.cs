using System;
using System.Text;
using System.Windows.Forms;

#if AdvTree
namespace DevComponents.Tree.TextMarkup
#elif DOTNETBAR
namespace DevComponents.DotNetBar.TextMarkup
#elif SUPERGRID
namespace DevComponents.SuperGrid.TextMarkup
#endif
{
    internal interface IActiveMarkupElement
    {
        bool HitTest(int x, int y);
        void MouseEnter(Control parent);
        void MouseLeave(Control parent);
        void MouseDown(Control parent, MouseEventArgs e);
        void MouseUp(Control parent, MouseEventArgs e);
        void Click(Control parent);
    }
}
