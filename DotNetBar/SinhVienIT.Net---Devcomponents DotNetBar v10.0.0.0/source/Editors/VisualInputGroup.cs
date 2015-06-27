#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;

namespace DevComponents.Editors
{
    /// <summary>
    /// Represents the group of the input items with automatic and manual item focus change.
    /// </summary>
    public class VisualInputGroup : VisualGroup
    {
        #region Private Variables
        #endregion

        #region Events
        #endregion

        #region Constructor
        #endregion

        #region Internal Implementation
        /// <summary>
        /// Resets the input position so the new input overwrites current value.
        /// </summary>
        public void ResetInputPosition()
        {
            foreach (VisualItem item in this.Items)
            {
                VisualInputBase input = item as VisualInputBase;
                if (input != null)
                    input.ResetInputPosition();
            }
        }

        protected override bool OnCmdKey(ref System.Windows.Forms.Message msg, System.Windows.Forms.Keys keyData)
        {
            if (keyData == Keys.Tab && (Control.ModifierKeys & Keys.Shift) != Keys.Shift && this.TabNavigationEnabled ||
                keyData == Keys.Enter && this.EnterNavigationEnabled ||
                keyData == Keys.Right && this.ArrowNavigationEnabled)
            {
                ValidateInput(this.FocusedItem);
                if (SelectNextInput())
                    return true;
            }
            else if (keyData == Keys.Left && this.ArrowNavigationEnabled ||
                (keyData & Keys.Tab) == Keys.Tab && (keyData & Keys.Shift) == Keys.Shift && this.TabNavigationEnabled)
            {
                ValidateInput(this.FocusedItem);
                if (SelectPreviousInput())
                    return true;
            }

            return base.OnCmdKey(ref msg, keyData);
        }

        private bool _TabNavigationEnabled = true;
        /// <summary>
        /// Gets or sets whether Tab key is used to navigate between the fields. Default value is true.
        /// </summary>
        [DefaultValue(true)]
        public bool TabNavigationEnabled
        {
            get
            {
                return _TabNavigationEnabled;
            }
            set
            {
                _TabNavigationEnabled = value;
            }
        }

        private bool _EnterNavigationEnabled = true;
        /// <summary>
        /// Gets or sets whether Enter key is used to navigate between input fields. Default value is true.
        /// </summary>
        [DefaultValue(true)]
        public bool EnterNavigationEnabled
        {
            get
            {
                return _EnterNavigationEnabled;
            }
            set
            {
                _EnterNavigationEnabled = value;
            }
        }

        private bool _ArrowNavigationEnabled = true;
        /// <summary>
        /// Gets or sets whether Arrow keys are used to navigate between input fields. Default value is true.
        /// </summary>
        [DefaultValue(true)]
        public bool ArrowNavigationEnabled
        {
            get
            {
                return _ArrowNavigationEnabled;
            }
            set
            {
                _ArrowNavigationEnabled = value;
            }
        }

        private bool _AutoAdvance = false;
        /// <summary>
        /// Gets or sets whether input focus is automatically advanced to next input field when input is complete in current one.
        /// </summary>
        [DefaultValue(false)]
        public bool AutoAdvance
        {
            get { return _AutoAdvance; }
            set { _AutoAdvance = value; }
        }

        protected override void OnInputComplete()
        {
            if (ValidateInput(this.FocusedItem))
            {
                if (_AutoAdvance)
                {
                    if (!this.SelectNextInput())
                    {
                        OnGroupInputComplete();
                    }
                }
            }

            base.OnInputComplete();
        }

        protected virtual bool ValidateInput(VisualItem inputItem)
        {
            return true;
        }

        protected virtual void OnGroupInputComplete()
        {
            if (this.Parent != null)
            {
                this.Parent.ProcessInputComplete();
            }
        }

        internal bool SelectNextInput()
        {
            return SelectInputItem(true);
        }

        internal bool SelectPreviousInput()
        {
            return SelectInputItem(false);
        }

        internal bool SelectInputItem(bool moveForward)
        {
            if (this.FocusedItem is VisualInputGroup)
            {
                VisualInputGroup group = this.FocusedItem as VisualInputGroup;
                if (group.SelectInputItem(moveForward))
                    return true;
            }

            VisualCollectionEnumerator enumerator = new VisualCollectionEnumerator(this.Items, !moveForward || this.IsRightToLeft);
            enumerator.CurrentIndex = this.Items.IndexOf(this.FocusedItem);

            while (enumerator.MoveNext())
            {
                VisualInputBase vi = enumerator.Current as VisualInputBase;
                if (vi != null && CanFocus(vi))
                {
                    this.FocusedItem = vi;
                    return true;
                }
                else if (enumerator.Current is VisualInputGroup)
                {
                    VisualInputGroup group = enumerator.Current as VisualInputGroup;
                    if (group.SelectInputItem(moveForward))
                    {
                        this.FocusedItem = group;
                        return true;
                    }
                }
            }

            return false;
        }

        protected override void OnGroupFocused()
        {
            if (this.FocusedItem == null)
                SelectInputItem(true);
        }

        private bool _IsReadOnly = false;
        /// <summary>
        /// Gets or sets whether input items are read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return _IsReadOnly; }
            set
            {
                if (_IsReadOnly != value)
                {
                    _IsReadOnly = value;
                    OnIsReadOnlyChanged();
                }
            }
        }

        private void OnIsReadOnlyChanged()
        {
            for (int i = 0; i < this.Items.Count; i++)
            {
                VisualInputBase v = this.Items[i] as VisualInputBase;
                if (v != null) v.IsReadOnly = this.IsReadOnly;
            }
        }

        protected override void OnItemsCollectionChanged(CollectionChangedInfo collectionChangedInfo)
        {
            if (collectionChangedInfo.ChangeType == eCollectionChangeType.Adding || collectionChangedInfo.ChangeType == eCollectionChangeType.Removing ||
                collectionChangedInfo.ChangeType == eCollectionChangeType.Clearing)
            {
                if (collectionChangedInfo.Added != null)
                {
                    foreach (VisualItem item in collectionChangedInfo.Added)
                    {
                        VisualInputBase v = item as VisualInputBase;
                        if (v != null)
                        {
                            v.IsReadOnly = this.IsReadOnly;
                            v.AutoOverwrite = _AutoOverwrite;
                        }
                    }
                }
            }

            base.OnItemsCollectionChanged(collectionChangedInfo);
        }

        private bool _AllowEmptyState = true;
        [DefaultValue(true)]
        public bool AllowEmptyState
        {
            get { return _AllowEmptyState; }
            set
            {
                if (_AllowEmptyState != value)
                {
                    _AllowEmptyState = value;
                    OnAllowEmptyStateChanged();
                    InvalidateArrange();
                }
            }
        }

        protected virtual void OnAllowEmptyStateChanged()
        {
            foreach (VisualItem item in this.Items)
            {
                if (item is VisualNumericInput) ((VisualNumericInput)item).AllowEmptyState = this.AllowEmptyState;
            }
        }

        private bool _IsEmpty = true;
        /// <summary>
        /// Gets or sets whether input group is empty i.e. it does not hold any value.
        /// </summary>
        [DefaultValue(true), Browsable(false)]
        public bool IsEmpty
        {
            get { return _IsEmpty; }
            set
            {
                if (_IsEmpty != value)
                {
                    _IsEmpty = value;
                    if (_IsEmpty)
                    {
                        ResetValue();
                    }
                    if (this.Parent is VisualInputGroup)
                        ((VisualInputGroup)this.Parent).UpdateIsEmpty();
                }
            }
        }

        protected override void OnInputChanged(VisualInputBase input)
        {
            UpdateIsEmpty();
            base.OnInputChanged(input);
        }

        /// <summary>
        /// Updates the IsEmpty property value based on the contained input controls.
        /// </summary>
        public virtual void UpdateIsEmpty()
        {
            bool empty = true;
            foreach (VisualItem item in this.Items)
            {
                if (item is VisualInputBase && !((VisualInputBase)item).IsEmpty)
                {
                    empty = false;
                    break;
                }
                else if (item is VisualInputGroup && !((VisualInputGroup)item).IsEmpty)
                {
                    empty = false;
                    break;
                }

            }
            this.IsEmpty = empty;
        }

        private bool _ResettingValue = false;
        protected virtual void ResetValue()
        {
            _ResettingValue = true;

            try
            {
                foreach (VisualItem item in this.Items)
                {
                    if (item is VisualInputGroup)
                        ((VisualInputGroup)item).IsEmpty = true;
                    else if (item is VisualInputBase)
                        ((VisualInputBase)item).IsEmpty = true;
                }
            }
            finally
            {
                _ResettingValue = false;
            }
        }

        private bool _AutoOverwrite = true;
        /// <summary>
        /// Gets or sets whether auto-overwrite functionality for input is enabled. When in auto-overwrite mode input field will erase existing entry
        /// and start new one if typing is continued after InputComplete method is called.
        /// </summary>
        public bool AutoOverwrite
        {
            get { return _AutoOverwrite; }
            set 
            {
                if (_AutoOverwrite != value)
                {
                    _AutoOverwrite = value;
                    OnAutoOverwriteChanged();
                }
            }
        }

        private void OnAutoOverwriteChanged()
        {
            foreach (VisualItem item in this.Items)
            {
                VisualInputBase input = item as VisualInputBase;
                if (input != null && input.AutoOverwrite != _AutoOverwrite)
                    input.AutoOverwrite = _AutoOverwrite;
            }
        }

        private bool _IsUserInput = false;
        /// <summary>
        /// Gets or sets whether current input is the user input.
        /// </summary>
        public bool IsUserInput
        {
            get { return _IsUserInput; }
            set
            {
                _IsUserInput = value;
            }
        }

        private string _SelectNextInputCharacters = "";
        /// <summary>
        /// List of characters that when pressed would select next input field.
        /// </summary>
        public string SelectNextInputCharacters
        {
            get { return _SelectNextInputCharacters; }
            set
            {
                if (value == null) value = "";
                if (_SelectNextInputCharacters != value)
                {
                    _SelectNextInputCharacters = value;
                }
            }
        }

        protected override void OnKeyPress(System.Windows.Forms.KeyPressEventArgs e)
        {
            if (_SelectNextInputCharacters.Length > 0 && _SelectNextInputCharacters.Contains(e.KeyChar.ToString()))
            {
                if (SelectNextInput())
                {
                    e.Handled = true;
                    return;
                }
            }
            base.OnKeyPress(e);
        }
        #endregion

    }
}
#endif

