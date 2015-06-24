#if FRAMEWORK20
using System;
using System.Text;
using System.Windows.Forms.Design;
using System.Collections;
using System.Windows.Forms.Design.Behavior;

namespace DevComponents.DotNetBar.Design
{
    public class LabelXDesigner : ControlDesigner
    {
        public override System.Collections.IList SnapLines
        {
            get
            {
                LabelX l = this.Control as LabelX;
                IList snapLines = base.SnapLines;
                
                int fontBaseOffset = (int)Math.Floor(l.Font.Size * l.Font.FontFamily.GetCellDescent(l.Font.Style) / l.Font.FontFamily.GetEmHeight(l.Font.Style));
                if (l.TextLineAlignment == System.Drawing.StringAlignment.Center)
                {
                    fontBaseOffset = (int)Math.Floor((double)(l.Height - l.Font.Height) / 2) + l.Font.Height-fontBaseOffset;
                }
                else if (l.TextLineAlignment == System.Drawing.StringAlignment.Far)
                {
                    fontBaseOffset = l.Height - fontBaseOffset;
                }
                else
                {
                    fontBaseOffset = l.Font.Height - fontBaseOffset;
                }

                snapLines.Add(new SnapLine(SnapLineType.Baseline, fontBaseOffset, SnapLinePriority.Medium));
                return snapLines;
            }
        }
    }
}
#endif