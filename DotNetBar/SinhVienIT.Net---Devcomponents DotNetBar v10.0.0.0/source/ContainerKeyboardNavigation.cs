using System;
using System.Text;
using System.Windows.Forms;

namespace DevComponents.DotNetBar
{
    internal class ContainerKeyboardNavigation
    {
        internal static void ProcessKeyDown(BaseItem container, System.Windows.Forms.KeyEventArgs objArg)
        {
            if (container.SubItems.Count == 0 || objArg.Handled)
                return;

            BaseItem objExpanded = container.ExpandedItem();

            if (objExpanded != null)
            {
                objExpanded.InternalKeyDown(objArg);
                if (objArg.Handled)
                    return;
            }

            eOrientation containerOrientation = container.Orientation;
            if (container is ItemContainer)
                containerOrientation = ((ItemContainer)container).LayoutOrientation;

            if (containerOrientation == eOrientation.Horizontal && (objArg.KeyCode == Keys.Left || objArg.KeyCode == Keys.Right || container.HotSubItem == null && (objArg.KeyCode == Keys.Down || objArg.KeyCode == Keys.Up)) ||
               (containerOrientation == eOrientation.Vertical && (objArg.KeyCode == Keys.Up || objArg.KeyCode == Keys.Down || container.HotSubItem == null && (objArg.KeyCode == Keys.Right || objArg.KeyCode == Keys.Left))))
            {
                // Select next object
                if (container.HotSubItem != null)
                {
                    container.HotSubItem.InternalMouseLeave();
                    if (container.AutoExpand && container.HotSubItem.Expanded)
                    {
                        container.HotSubItem.Expanded = false;
                    }
                }
                if (objArg.KeyCode == System.Windows.Forms.Keys.Left || objArg.KeyCode == Keys.Up)
                {
                    int iIndex = 0;
                    if (container.HotSubItem != null)
                        iIndex = container.SubItems.IndexOf(container.HotSubItem) - 1;
                    if (iIndex < 0)
                        iIndex = container.SubItems.Count - 1;
                    BaseItem objNew = null;
                    bool bRepeat = false;
                    do
                    {
                        for (int i = iIndex; i >= 0; i--)
                        {
                            objNew = container.SubItems[i];
                            if (CanFocus(objNew))
                            {
                                iIndex = i;
                                break;
                            }
                        }
                        if (!CanFocus(container.SubItems[iIndex]))
                        {
                            if (!bRepeat)
                            {
                                iIndex = container.SubItems.Count - 1;
                                bRepeat = true;
                            }
                            else
                                bRepeat = false;
                        }
                        else
                            bRepeat = false;
                    } while (bRepeat);
                    container.HotSubItem = container.SubItems[iIndex];
                }
                else
                {
                    int iIndex = 0;
                    if (container.HotSubItem != null)
                        iIndex = container.SubItems.IndexOf(container.HotSubItem) + 1;

                    while (iIndex < container.SubItems.Count && !CanFocus(container.SubItems[iIndex]))
                        iIndex++;
                    if (iIndex >= container.SubItems.Count)
                        iIndex = 0;
                    BaseItem objNew = null;
                    for (int i = iIndex; i < container.SubItems.Count; i++)
                    {
                        objNew = container.SubItems[i];
                        if (CanFocus(objNew))
                        {
                            iIndex = i;
                            break;
                        }
                    }
                    container.HotSubItem = container.SubItems[iIndex];
                }
                
                if (container.HotSubItem != null)
                {
                    if (container.HotSubItem is ItemContainer)
                    {
                        ((ItemContainer)container.HotSubItem).SelectFirstItem();
                    }
                    else
                    {
                        container.HotSubItem.InternalMouseEnter();
                        container.HotSubItem.InternalMouseMove(new MouseEventArgs(MouseButtons.None, 0, container.HotSubItem.LeftInternal + 1, container.HotSubItem.TopInternal + 1, 0));
                        IScrollableItemControl isc = container.ContainerControl as IScrollableItemControl;
                        if (isc != null)
                            isc.KeyboardItemSelected(container.HotSubItem);
                    }
                }

                objArg.Handled = true;
            }
            else if (objArg.KeyCode == System.Windows.Forms.Keys.Escape)
            {
                if (objExpanded != null)
                {
                    objExpanded.Expanded = false;
                    objArg.Handled = true;
                }
                else
                {
                    Control cc = container.ContainerControl as Control;
                    if (cc is Bar)
                    {
                        Bar bar = cc as Bar;
                        if (bar.BarState == eBarState.Popup)
                        {
                            bar.ParentItem.Expanded = false;
                        }
                        else
                        {
                            if (container.AutoExpand)
                                container.AutoExpand = false;
                            else if (bar.Focused || bar.MenuFocus)
                            {
                                bar.MenuFocus = false;
                                bar.ReleaseFocus();
                            }
                        }
                        objArg.Handled = true;
                    }
                    else if (cc is ItemControl)
                    {
                        ItemControl ic = cc as ItemControl;
                        if (container.AutoExpand)
                            container.AutoExpand = false;
                        else if (ic.Focused || ic.MenuFocus)
                        {
                            ic.MenuFocus = false;
                            ic.ReleaseFocus();
                        }
                    }
                }
            }
            else
            {
                BaseItem objItem = container.ExpandedItem();
                if (objItem != null)
                    objItem.InternalKeyDown(objArg);
                else
                {
                    int key = 0;
                    if (objArg.Shift)
                    {
                        try
                        {
                            byte[] keyState = new byte[256];
                            if (NativeFunctions.GetKeyboardState(keyState))
                            {
                                byte[] chars = new byte[2];
                                if (NativeFunctions.ToAscii((uint)objArg.KeyValue, 0, keyState, chars, 0) != 0)
                                {
                                    key = chars[0];
                                }
                            }
                        }
                        catch (Exception)
                        {
                            key = 0;
                        }
                    }

                    if (key == 0)
                        key = (int)NativeFunctions.MapVirtualKey((uint)objArg.KeyValue, 2);

                    if (container.HotSubItem != null)
                        container.HotSubItem.InternalKeyDown(objArg);
                }
            }
        }

        private static bool CanFocus(BaseItem objNew)
        {
            if (objNew == null) return false;
            if (!objNew.Visible || !objNew.GetEnabled())
                return false;

            if (objNew is LabelItem)
                return false;

            return true;
        }
    }
}
