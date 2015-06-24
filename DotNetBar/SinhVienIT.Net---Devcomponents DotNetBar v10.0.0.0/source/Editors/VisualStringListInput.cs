#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Text;
using DevComponents.Editors.Primitives;
using System.ComponentModel;

namespace DevComponents.Editors
{
    public class VisualStringListInput : VisualListInput
    {
        #region Private Variables
        private List<string> _Items = new List<string>();
        private string _Text = "";
        private string _LastValidatedInputStack = "", _LastMatch = "";
        private bool _LastMatchComplete = false;
        #endregion

        #region Events
        /// <summary>
        /// Occurs when SelectedIndex property has changed.
        /// </summary>
        public event EventHandler SelectedIndexChanged;
        #endregion

        #region Constructor
        public VisualStringListInput()
        {

        }
        #endregion

        #region Internal Implementation
        public List<String> Items
        {
            get
            {
                return _Items;
            }
        }

        protected virtual List<String> GetItems()
        {
            return _Items;
        }

        protected override bool ValidateNewInputStack(string s)
        {
            _LastValidatedInputStack = "";
            _LastMatch = "";
            _LastMatchComplete = false;

            if (s.Length > 0)
            {
                List<String> items = GetItems();
                StartsWithPredicate p = new StartsWithPredicate(s.ToLower(), 2);
                List<string> match = items.FindAll(p.MatchTop);
                if (match == null || match.Count == 0)
                    return false;

                _LastMatch = match[0];
                _LastValidatedInputStack = s;
                _LastMatchComplete = match.Count == 1;
                return true;
            }

            return base.ValidateNewInputStack(s);
        }

        protected string LastValidatedInputStack
        {
            get
            {
                return _LastValidatedInputStack;
            }
            set
            {
                _LastValidatedInputStack = value;
            }
        }

        protected string LastMatch
        {
            get
            {
                return _LastMatch;
            }
            set
            {
                _LastMatch = value;
            }
        }

        protected bool LastMatchComplete
        {
            get
            {
                return _LastMatchComplete;
            }
            set
            {
                _LastMatchComplete = value;
            }
        }

        protected override void OnInputStackChanged()
        {
            bool changed = false;
            if (_LastValidatedInputStack.Length > 0 && this.InputStack == _LastValidatedInputStack)
            {
                changed = _Text != _LastMatch;
                _Text = _LastMatch;
            }
            else if (this.InputStack.Length == 0)
            {
                changed = _Text != "";
                _Text = "";
            }

            InvalidateArrange();

            if (changed)
                OnSelectedIndexChanged(new EventArgs());

            base.OnInputStackChanged();
        }

        protected override void OnInputKeyAccepted()
        {
            if (_LastMatchComplete)
            {
                InputComplete(true);
                _LastMatchComplete = false;
            }
            base.OnInputKeyAccepted();
        }

        /// <summary>
        /// Gets or sets the text that is selected by the control.
        /// </summary>
        [DefaultValue("")]
        public string Text
        {
            get
            {
                return GetInputStringValue();
            }
            set
            {
                if (_Text != value)
                {
                    List<String> items = GetItems();
                    if (value == "" && !AllowEmptyState && items.Count > 0)
                        value = items[0];
                    else if (value != null && value.Length > 0)
                    {
                        if (!items.Contains(value))
                        {
                            if (!AllowEmptyState && items.Count > 0)
                                value = items[0];
                            else
                                value = "";
                        }
                    }

                    _Text = value;
                    ResetInputStack();
                    OnInputChanged();
                    OnSelectedIndexChanged(new EventArgs());

                    InvalidateArrange();
                    if (this.IsEmpty) this.IsEmpty = false;
                }
            }
        }

        /// <summary>
        /// Raises SelectedIndexChanged event
        /// </summary>
        protected virtual void OnSelectedIndexChanged(EventArgs e)
        {
            if (SelectedIndexChanged != null)
                SelectedIndexChanged(this, e);
        }

        /// <summary>
        /// Gets or sets the currently selected index. -1 is returned if nothing is selected.
        /// </summary>
        [DefaultValue(-1), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int SelectedIndex
        {
            get
            {
                if (this.IsEmpty || _Text == "") return -1;
                List<string> items = GetItems();
                return items.IndexOf(this.Text);
            }
            set
            {
                if (value >= 0)
                {
                    List<string> items = GetItems();
                    this.Text = items[value];
                }
                else
                    this.Text = "";
            }
        }

        protected override string GetInputStringValue()
        {
            if (_Text == null) return "";
            return _Text;
        }

        public override void SelectNext()
        {
            int index = this.SelectedIndex + 1;
            List<string> items = GetItems();
            if (index >= items.Count)
                index = 0;
            this.SelectedIndex = index;
            base.SelectNext();
        }

        public override void SelectPrevious()
        {
            int index = this.SelectedIndex - 1;
            if (index < 0)
            {
                List<string> items = GetItems();
                index = items.Count - 1;
            }
            this.SelectedIndex = index;
            base.SelectPrevious();
        }
        #endregion

    }
}
#endif

