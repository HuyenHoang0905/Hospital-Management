#if FRAMEWORK20
using System;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using DevComponents.DotNetBar;

namespace DevComponents.Editors
{
    public class VisualInputBase : VisualItem
    {
        #region Private Variables
        private int _InputPosition = 0;
        private string _InputStack = "";
        private bool _InsertMode = true;
        private string _UndoInputStack = "";
        #endregion

        #region Events
        /// <summary>
        /// Occurs when input on the control has changed.
        /// </summary>
        public event EventHandler InputChanged;
        /// <summary>
        /// Occurs when validation of the input is performed and it allows you to deny the input.
        /// </summary>
        public event InputValidationEventHandler ValidateInput;
        /// <summary>
        /// Occurs when IsEmpty property has changed.
        /// </summary>
        public event EventHandler IsEmptyChanged;
        #endregion

        #region Constructor
        public VisualInputBase()
        {
            this.Focusable = true;
        }
        #endregion

        #region Internal Implementation
        internal override void ProcessKeyDown(KeyEventArgs e)
        {
            if (!_IsReadOnly)
                base.ProcessKeyDown(e);
        }
        internal override void ProcessKeyPress(KeyPressEventArgs e)
        {
            if (!_IsReadOnly)
                base.ProcessKeyPress(e);
        }
        internal override void ProcessKeyUp(KeyEventArgs e)
        {
            if (!_IsReadOnly)
                base.ProcessKeyUp(e);
        }
        internal override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (!_IsReadOnly)
                return base.ProcessCmdKey(ref msg, keyData);
            return false;
        }

        protected override void OnKeyDown(System.Windows.Forms.KeyEventArgs e)
        {
            OnInputKeyDown(e);
            base.OnKeyDown(e);
        }

        protected virtual void OnInputKeyDown(System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.Back)
                ProcessBackSpace(e);
            else if (e.KeyCode == System.Windows.Forms.Keys.Delete)
                ProcessDelete(e);
            else if (e.KeyCode == Keys.C && e.Modifiers == Keys.Control)
            {
                ProcessClipboardCopy();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.X && e.Modifiers == Keys.Control)
            {
                ProcessClipboardCut();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.V && e.Modifiers == Keys.Control)
            {
                ProcessClipboardPaste();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Z && e.Modifiers == Keys.Control)
            {
                ProcessUndo();
                e.Handled = true;
            }
        }

        internal virtual void ProcessClear()
        {
            OnClear();
        }

        internal virtual void ProcessClipboardCopy()
        {
            OnClipboardCopy();
        }

        internal virtual void ProcessClipboardCut()
        {
            OnClipboardCut();
        }

        internal virtual void ProcessClipboardPaste()
        {
            OnClipboardPaste();
        }

        internal virtual void ProcessUndo()
        {
            OnUndo();
        }

        protected virtual void OnUndo()
        {
            if (SetInputStack(_UndoInputStack))
            {
                _InputPosition = _InputStack.Length;
                OnInputKeyAccepted();
            }
        }

        /// <summary>
        /// Reverts the input to the last stored value.
        /// </summary>
        public void UndoInput()
        {
            ProcessUndo();
        }

        protected virtual void OnClear()
        {
            if (SetInputStack(""))
            {
                _InputPosition = 0;
            }
        }

        protected virtual void OnClipboardCut()
        {
            OnClipboardCopy();
            OnClear();
        }

        protected virtual void OnClipboardPaste()
        {
            if (Clipboard.ContainsText())
            {
                if (SetInputStack(Clipboard.GetText()))
                {
                    _InputPosition = _InputStack.Length;
                    OnInputKeyAccepted();
                }
            }
        }

        protected virtual void OnClipboardCopy()
        {
            Clipboard.SetText(GetInputStringValue());
        }

        protected virtual string GetInputStringValue()
        {
            return _InputStack;
        }

        protected virtual void ProcessDelete(System.Windows.Forms.KeyEventArgs e)
        {
            OnClear();
            e.Handled = true;
        }

        protected virtual void ProcessBackSpace(System.Windows.Forms.KeyEventArgs e)
        {
            if (_InputStack.Length > 0 && _InputPosition > 0)
            {
                string s = _InputStack.Substring(0, _InputPosition - 1) + _InputStack.Substring(_InputPosition);
                if (SetInputStack(s))
                {
                    _InputPosition = Math.Max(0, _InputPosition - 1);
                }
            }
            else
                OnClear();
            e.Handled = true;
        }

        protected virtual void ResetValue() { }

        protected override void OnKeyPress(System.Windows.Forms.KeyPressEventArgs e)
        {
            OnInputKeyPress(e);
            base.OnKeyPress(e);
        }

        protected virtual void OnInputKeyPress(System.Windows.Forms.KeyPressEventArgs e)
        {
            if (AcceptKeyPress(e))
                UpdateInputStack(e.KeyChar);
        }

        protected virtual void UpdateInputStack(char c)
        {
            string s = _InputStack;
            if (_InputPosition >= _InputStack.Length)
            {
                if (_InputPosition > _InputStack.Length)
                    _InputPosition = _InputStack.Length;
                s += c.ToString();

            }
            else if (_InsertMode)
                s.Insert(_InputPosition, c.ToString());
            else
                s = _InputStack.Substring(0, _InputPosition - 1) + c.ToString() + _InputStack.Substring(_InputPosition + 1);

            if (SetInputStack(s))
            {
                _InputPosition++;
                OnInputKeyAccepted();
            }
        }

        protected virtual void OnInputKeyAccepted()
        {
        }

        protected virtual bool SetInputStack(string s)
        {
            if (ValidateNewInputStack(s))
            {
                _InputStack = s;

                OnInputStackChanged();

                return true;
            }

            return false;
        }

        protected virtual bool ValidateNewInputStack(string s)
        {
            if (ValidateInput != null)
            {
                InputValidationEventArgs e = new InputValidationEventArgs(s);
                ValidateInput(this, e);
                return e.AcceptInput;
            }

            return true;
        }

        protected virtual void OnInputStackChanged()
        {
            OnInputChanged();
        }

        protected virtual void OnInputChanged()
        {
            if (this.Parent != null)
                this.Parent.ProcessInputChanged(this);

            if (InputChanged != null)
                InputChanged(this, new EventArgs());
        }

        protected virtual bool AcceptKeyPress(System.Windows.Forms.KeyPressEventArgs e)
        {
            e.Handled = true;
            return true;
        }

        protected override void OnGotFocus()
        {
            OnInputGotFocus();
            base.OnGotFocus();
        }

        protected override void OnLostFocus()
        {
            OnInputLostFocus();
            base.OnLostFocus();
        }

        protected virtual void OnInputGotFocus()
        {
            ResetInputStack();
            _UndoInputStack = GetInputStringValue();
            this.InvalidateArrange();
        }

        protected virtual void OnInputLostFocus()
        {
            this.InvalidateArrange();
        }

        protected string InputStack
        {
            get
            {
                return _InputStack;
            }
        }

        protected void SetInputPosition(int position)
        {
            _InputPosition = position;
        }

        public int InputPosition
        {
            get { return _InputPosition; }
        }

        //private bool _IsInputStackEmpty = true;
        protected bool IsInputStackEmpty
        {
            get
            {
                return _InputStack.Length == 0;
            }
        }
        /// <summary>
        /// Called when input field is full, i.e. it has an complete entry. If auto-overwrite is enabled the continued typing after input is complete
        /// will erase the existing entry and start new one.
        /// </summary>
        protected virtual void InputComplete(bool sendNotification)
        {
            if (_AutoOverwrite)
            {
                ResetInputStack();
            }
            if (sendNotification && this.Parent != null)
            {
                this.Parent.ProcessInputComplete();
            }
        }

        /// <summary>
        /// Resets the input position so the new input overwrites current value.
        /// </summary>
        public void ResetInputPosition()
        {
            ResetInputStack();
        }

        protected virtual void ResetInputStack()
        {
            _InputStack = "";
            _InputPosition = 0;
        }

        public override void PerformLayout(PaintInfo p)
        {
            if (p.WatermarkEnabled && this.DrawWatermark)
            {
                Font font = p.DefaultFont;
                if (p.WatermarkFont != null) font = p.WatermarkFont;
                System.Drawing.Size size = TextDrawing.MeasureString(p.Graphics, this.WatermarkText, font, 0, eTextFormat.Default | eTextFormat.NoPadding);
                this.Size = size;
            }
            base.PerformLayout(p);
        }

        protected override void OnPaint(PaintInfo p)
        {
            if (p.WatermarkEnabled && this.DrawWatermark)
            {
                Font font = p.DefaultFont;
                if (p.WatermarkFont != null) font = p.WatermarkFont;
                TextDrawing.DrawString(p.Graphics, this.WatermarkText, font, p.WatermarkColor, this.RenderBounds, eTextFormat.Default);
            }

            base.OnPaint(p);
        }

        /// <summary>
        /// Gets whether watermark will be drawn in current control state.
        /// </summary>
        protected virtual bool DrawWatermark
        {
            get
            {
                return this.WatermarkEnabled && this.IsEmpty && this.WatermarkText.Length > 0 && !this.IsFocused;
            }
        }

        private bool _IsEmpty = true;
        /// <summary>
        /// Gets or sets whether control is empty.
        /// </summary>
        public virtual bool IsEmpty
        {
            get { return _IsEmpty; }
            set
            {
                if (value != _IsEmpty)
                {
                    if (value)
                        ResetValue();
                    _IsEmpty = value;
                    InvalidateArrange();
                    if (this.Parent is VisualInputGroup)
                        ((VisualInputGroup)this.Parent).UpdateIsEmpty();
                    OnIsEmptyChanged();
                }
            }
        }

        protected virtual void OnIsEmptyChanged() 
        {
            if (IsEmptyChanged != null)
                IsEmptyChanged(this, new EventArgs());
        }

        private bool _AutoOverwrite = true;
        /// <summary>
        /// Gets or sets whether auto-overwrite functionality for input is enabled. When in auto-overwrite mode input field will erase existing entry
        /// and start new one if typing is continued after InputComplete method is called.
        /// </summary>
        public bool AutoOverwrite
        {
            get { return _AutoOverwrite; }
            set { _AutoOverwrite = value; }
        }

        private string _WatermarkText = "";
        /// <summary>
        /// Gets or sets the watermark text displayed on the input control when control is empty.
        /// </summary>
        [Localizable(true)]
        public string WatermarkText
        {
            get { return _WatermarkText; }
            set
            {
                if (value != null)
                    _WatermarkText = value;
            }
        }

        private bool _WatermarkEnabled = true;
        /// <summary>
        /// Gets or sets whether watermark text is displayed if set. Default value is true.
        /// </summary>
        [DefaultValue(true), Description("Indicates whether watermark text is displayed if set.")]
        public virtual bool WatermarkEnabled
        {
            get { return _WatermarkEnabled; }
            set { _WatermarkEnabled = value; }
        }

        private bool _IsReadOnly = false;
        /// <summary>
        /// Gets or sets whether input item is read-only.
        /// </summary>
        public virtual bool IsReadOnly
        {
            get { return _IsReadOnly; }
            set
            {
                _IsReadOnly = value;
            }
        }
        #endregion

    }
}
#endif

