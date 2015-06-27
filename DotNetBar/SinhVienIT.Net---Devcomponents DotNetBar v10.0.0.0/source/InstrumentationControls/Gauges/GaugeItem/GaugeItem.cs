using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DevComponents.Instrumentation.Primitives;

namespace DevComponents.Instrumentation
{
    public class GaugeItemCollection : GenericCollection<GaugeItem>
    {
    }

    public class GaugeItem : IDisposable, ICloneable
    {
        #region Events

        public event EventHandler<EventArgs> GaugeItemChanged;

        #endregion

        #region Private variables

        private string _Name;
        private string _Tooltip;

        private object _Tag;

        private bool _NeedRecalcLayout;
        private bool _Visible;

        #endregion

        public GaugeItem()
        {
            _NeedRecalcLayout = true;
            _Visible = true;
        }

        #region Public properties

        #region Name

        /// <summary>
        /// Gets or sets the Name associated with the item
        /// </summary>
        [Browsable(true), Category("Misc."), DefaultValue(null)]
        [Description("Indicates the Name associated with the item.")]
        [ParenthesizePropertyName(true)]
        public string Name
        {
            get { return (_Name); }
            set { _Name = value; }
        }

        #endregion

        #region Tag

        /// <summary>
        /// Gets or sets the user defined Tag associated with the item
        /// </summary>
        [Browsable(false), DefaultValue(0)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object Tag
        {
            get { return (_Tag); }
            set { _Tag = value; }
        }

        #endregion

        #region Tooltip

        /// <summary>
        /// Gets or sets the Tooltip associated with the item
        /// </summary>
        [Browsable(true), Category("Appearance"), DefaultValue(null)]
        [Description("Indicates the Tooltip associated with the item.")]
        public string Tooltip
        {
            get { return (_Tooltip); }
            set { _Tooltip = value; }
        }

        #endregion

        #region Visible

        /// <summary>
        /// Gets or sets the item Visibility state.
        /// </summary>
        [Browsable(true), Category("Appearance"), DefaultValue(true)]
        [Description("Indicates the item Visibility state.")]
        [ParenthesizePropertyName(true)]
        public virtual bool Visible
        {
            get { return (_Visible); }

            set
            {
                if (_Visible != value)
                {
                    _Visible = value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        #endregion

        #endregion

        #region Internal properties

        #region NeedRecalcLayout

        internal virtual bool NeedRecalcLayout
        {
            get { return (_NeedRecalcLayout); }
            set { _NeedRecalcLayout = value; }
        }

        #endregion

        #endregion

        #region RecalcLayout

        public virtual void RecalcLayout()
        {
            _NeedRecalcLayout = false;
        }

        #endregion

        #region PerformLayout

        /// <summary>
        /// Causes the item to recalculate its layout
        /// </summary>
        public virtual void PerformLayout()
        {
            _NeedRecalcLayout = true;

            RecalcLayout();
        }

        #endregion

        #region OnPaint

        public virtual void OnPaint(PaintEventArgs e)
        {
            if (_NeedRecalcLayout == true)
                RecalcLayout();
        }

        #endregion

        #region OnGaugeItemChanged

        protected virtual void OnGaugeItemChanged()
        {
            if (GaugeItemChanged != null)
                GaugeItemChanged(this, EventArgs.Empty);
        }

        protected virtual void OnGaugeItemChanged(bool recalc)
        {
            if (recalc == true)
                NeedRecalcLayout = true;

            if (GaugeItemChanged != null)
                GaugeItemChanged(this, EventArgs.Empty);
        }

        #endregion

        #region FindItem

        internal virtual GaugeItem FindItem(Point pt)
        {
            return (null);
        }

        #endregion

        #region OnMouseMove

        internal virtual void OnMouseMove(MouseEventArgs e, bool mouseDown)
        {
        }

        #endregion

        #region OnMouseEnter

        internal virtual void OnMouseEnter()
        {
        }

        #endregion

        #region OnMouseLeave

        internal virtual void OnMouseLeave()
        {
        }

        #endregion

        #region OnMouseDown

        internal virtual void OnMouseDown(MouseEventArgs e)
        {
        }

        #endregion

        #region OnMouseUp

        internal virtual void OnMouseUp(MouseEventArgs e)
        {
        }

        #endregion

        #region GetDisplayTemplateText

        internal string GetitemTemplateText(GaugeControl gauge)
        {
            string s = _Tooltip;

            if (String.IsNullOrEmpty(s) == true)
                return (s);

            Regex r = new Regex(
                @"\[(?<key>[^\{\]]+)" +
                @"(\{(?<data>[^\}\]]+)\})*" +
                @"\]");

            MatchCollection mc = r.Matches(s);

            if (mc.Count <= 0)
                return (s);

            int index = 0;
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < mc.Count; i++)
            {
                Match ma = mc[i];

                if (ma.Index > index)
                    sb.Append(s.Substring(index, ma.Index - index));

                string t1 = mc[i].Groups["key"].Value;
                string t2 = mc[i].Groups["data"].Value;

                ProcessTemplateText(gauge, sb, t1, t2);

                index = ma.Index + ma.Length;
            }

            if (s.Length > index)
                sb.Append(s.Substring(index));

            return (sb.ToString());
        }

        #endregion

        #region AppendTemplateText

        protected virtual void ProcessTemplateText(
            GaugeControl gauge, StringBuilder sb, string key, string data)
        {
            switch (key)
            {
                case "Name":
                    sb.Append(string.IsNullOrEmpty(data)
                        ? _Name
                        : String.Format("{0:" + data + "}", _Name));
                    break;

                case "Tag":
                    sb.Append(string.IsNullOrEmpty(data)
                        ? Tag.ToString()
                        : String.Format("{0:" + data + "}", Tag));
                    break;

                default:
                    sb.Append(gauge.OnGetDisplayTemplateText(this, key, data));
                    break;
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            OnDispose();
        }

        protected virtual void OnDispose()
        {
        }

        #endregion

        #region ICloneable Members

        public virtual object Clone()
        {
            GaugeItem copy = new GaugeItem();

            CopyToItem(copy);

            return (copy);
        }

        #endregion

        #region CopyToItem

        public virtual void CopyToItem(GaugeItem copy)
        {
            copy.Name = _Name;
            copy.Tag = _Tag;
            copy.Tooltip = _Tooltip;
            copy.Visible = _Visible;
        }

        #endregion
    }

    #region Enums

    public enum DisplayPlacement
    {
        Near,
        Center,
        Far
    }

    public enum DisplayLevel
    {
        Top,
        Bottom
    }

    public enum ColorSourceFillEntry
    {
        MajorTickMark,
        MinorTickMark,
        Pointer,
        Cap
    }

    #endregion
}
