#if FRAMEWORK20
using System;
using System.Text;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Collections.ObjectModel;

namespace DevComponents.DotNetBar.Validator
{
    [ToolboxBitmap(typeof(SuperTooltip), "Validator.SuperValidator.ico"), ToolboxItem(true), ProvideProperty("Validator1", typeof(Control)),
    ProvideProperty("Validator2", typeof(Control)), ProvideProperty("Validator3", typeof(Control)),
   System.Runtime.InteropServices.ComVisible(false), Designer("DevComponents.DotNetBar.Design.SuperValidatorDesigner, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
    public class SuperValidator : Component, IExtenderProvider
    {
        #region Private Variables
        private Dictionary<Control, ValidatorBase> _Validators1 = new Dictionary<Control, ValidatorBase>();
        private Dictionary<Control, ValidatorBase> _Validators2 = new Dictionary<Control, ValidatorBase>();
        private Dictionary<Control, ValidatorBase> _Validators3 = new Dictionary<Control, ValidatorBase>();
        #endregion

        #region Constructor

        #endregion

        #region Events
        /// <summary>
        /// Occurs when validator retrieves the value for the control. It allows you to return value for the controls validator does not recognize.
        /// </summary>
        public event ValidatorGetValueEventHandler GetValue;
        /// <summary>
        /// Occurs when CustomValidator needs to validate the control value.
        /// </summary>
        public event ValidateValueEventHandler CustomValidatorValidateValue;
        #endregion

        #region Implementation
        protected override void Dispose(bool disposing)
        {
            ClearAllValidators();
            ContainerControl = null;
            base.Dispose(disposing);
        }

        private void ClearAllValidators()
        {
            ClearValidator(_Validators1);
            ClearValidator(_Validators2);
            ClearValidator(_Validators3);
        }

        private void ClearValidator(Dictionary<Control, ValidatorBase> validators)
        {
            if (validators == null || validators.Count == 0) return;

            foreach (KeyValuePair<Control, ValidatorBase> item in validators)
            {
                item.Key.Validating -= ControlValidating;
            }

            validators.Clear();
        }


        internal void InvokeCustomValidatorValidateValue(CustomValidator validator, ValidateValueEventArgs e)
        {
            OnCustomValidatorValidateValue(validator, e);
        }
        /// <summary>
        /// Raises the CustomValidatorValidateValue event.
        /// </summary>
        /// <param name="validator">Validator that needs validation.</param>
        /// <param name="e">Control to validate.</param>
        protected virtual void OnCustomValidatorValidateValue(CustomValidator validator, ValidateValueEventArgs e)
        {
            ValidateValueEventHandler h = CustomValidatorValidateValue;
            if (h != null) h(validator, e);
        }

        /// <summary>
        /// Retrieves first level Validator for given control or return null if control does not have validator associated with it.
        /// </summary>
        [Editor("DevComponents.DotNetBar.Design.ValidatorEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(System.Drawing.Design.UITypeEditor)), DefaultValue(null), Localizable(true)]
        public ValidatorBase GetValidator1(Control c)
        {
            if (_Validators1.ContainsKey(c))
            {
                ValidatorBase info = _Validators1[c];
                return info;
            }
            return null;
        }
        /// <summary>
        /// Associates first level Validator with given control.
        /// </summary>
        /// <param name="c">Reference to supported control.</param>
        /// <param name="info">Instance of validator class. If null is passed existing Validator is detached from the given control.</param>
        public void SetValidator1(Control c, ValidatorBase info)
        {
            if (_Validators1.ContainsKey(c))
            {
                if (info == null)
                {
                    ValidatorBase validator = _Validators1[c];
                    this.RemoveValidator(_Validators1, c);

                    DestroyValidator(validator);
                }
                else
                {
                    ValidatorBase validator = _Validators1[c];
                    if (validator != info)
                        DestroyValidator(validator);
                    _Validators1[c] = info;
                }
            }
            else if (info != null)
            {
                this.AddValidator(_Validators1, c, info);
            }
        }

        /// <summary>
        /// Retrieves second level Validator for given control or return null if control does not have validator associated with it.
        /// </summary>
        [Editor("DevComponents.DotNetBar.Design.ValidatorEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(System.Drawing.Design.UITypeEditor)), DefaultValue(null), Localizable(true)]
        public ValidatorBase GetValidator2(Control c)
        {
            if (_Validators2.ContainsKey(c))
            {
                ValidatorBase info = _Validators2[c];
                return info;
            }
            return null;
        }
        /// <summary>
        /// Associates second level Validator with given control.
        /// </summary>
        /// <param name="c">Reference to supported control.</param>
        /// <param name="info">Instance of validator class. If null is passed existing Validator is detached from the given control.</param>
        public void SetValidator2(Control c, ValidatorBase info)
        {
            if (_Validators2.ContainsKey(c))
            {
                if (info == null)
                {
                    ValidatorBase validator = _Validators2[c];
                    this.RemoveValidator(_Validators2, c);

                    DestroyValidator(validator);
                }
                else
                {
                    ValidatorBase validator = _Validators2[c];
                    if (validator != info)
                        DestroyValidator(validator);
                    _Validators2[c] = info;
                }
            }
            else if (info != null)
            {
                this.AddValidator(_Validators2, c, info);
            }
        }

        /// <summary>
        /// Retrieves third level Validator for given control or return null if control does not have validator associated with it.
        /// </summary>
        [Editor("DevComponents.DotNetBar.Design.ValidatorEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(System.Drawing.Design.UITypeEditor)), DefaultValue(null), Localizable(true)]
        public ValidatorBase GetValidator3(Control c)
        {
            if (_Validators3.ContainsKey(c))
            {
                ValidatorBase info = _Validators3[c];
                return info;
            }
            return null;
        }
        /// <summary>
        /// Associates third level Validator with given control.
        /// </summary>
        /// <param name="c">Reference to supported control.</param>
        /// <param name="info">Instance of validator class. If null is passed existing Validator is detached from the given control.</param>
        public void SetValidator3(Control c, ValidatorBase info)
        {
            if (_Validators3.ContainsKey(c))
            {
                if (info == null)
                {
                    ValidatorBase validator = _Validators3[c];
                    this.RemoveValidator(_Validators3, c);

                    DestroyValidator(validator);
                }
                else
                {
                    ValidatorBase validator = _Validators3[c];
                    if (validator != info)
                        DestroyValidator(validator);
                    _Validators3[c] = info;
                }
            }
            else if (info != null)
            {
                this.AddValidator(_Validators3, c, info);
            }
        }

        private void DestroyValidator(ValidatorBase validator)
        {
            if (this.Site != null && this.DesignMode)
            {
                IDesignerHost dh = this.Site.GetService(typeof(IDesignerHost)) as IDesignerHost;
                if (dh != null)
                {
                    dh.DestroyComponent(validator);
                }
            }
        }
        private void RemoveValidator(Dictionary<Control, ValidatorBase> validatorCollection, Control control)
        {
            ValidatorBase info = validatorCollection[control];
            info.SuperValidator = null;
            validatorCollection.Remove(control);
            control.Validating -= ControlValidating;
        }
        private void AddValidator(Dictionary<Control, ValidatorBase> validatorCollection, Control control, ValidatorBase info)
        {
            info.SuperValidator = this;
            validatorCollection.Add(control, info);
            control.Validating += ControlValidating;
        }

        private void ControlValidating(object sender, CancelEventArgs e)
        {
            if (!this.Enabled)
                return;
            if (_ValidationType == eValidationType.Manual)
                return;
            if ((_ValidationType & eValidationType.ValidatingEventPerControl) != eValidationType.ValidatingEventPerControl)
                return;

            Control control = sender as Control;
            if (control == null) return;

            ValidatorBase validator = null;
            bool isValid = true;
            if (_Validators1.TryGetValue(control, out validator))
            {
                bool cancelValidating = false;
                isValid = ValidateSingleControl(validator, control, out cancelValidating);
                if (!isValid) AddToLastFailedValidations(validator, control);
                e.Cancel = cancelValidating;
            }

            if (isValid && _Validators2.TryGetValue(control, out validator))
            {
                bool cancelValidating = false;
                isValid = ValidateSingleControl(validator, control, out cancelValidating);
                if (!isValid) AddToLastFailedValidations(validator, control);
                e.Cancel = cancelValidating;
            }

            if (isValid && _Validators3.TryGetValue(control, out validator))
            {
                bool cancelValidating = false;
                isValid = ValidateSingleControl(validator, control, out cancelValidating);
                if (!isValid) AddToLastFailedValidations(validator, control);
                e.Cancel = cancelValidating;
            }
        }
        private void AddToLastFailedValidations(ValidatorBase validator, Control control)
        {
            foreach (ValidatorControlPair item in _LastFailedValidations)
            {
                if (item.Control == control && item.Validator == validator) return;
            }
            _LastFailedValidations.Add(new ValidatorControlPair(validator, control));
        }

        private bool ValidateSingleControl(ValidatorBase validator, Control control, out bool cancelValidating)
        {
            cancelValidating = false;
            if (!validator.Enabled) return true;

            bool result = true;
            bool validateControl = true;
            if (!string.IsNullOrEmpty(validator.OptionalValidationGroup))
            {
                List<ValidatorControlPair> optionalValidators = GetOptionalValidators(validator.OptionalValidationGroup);
                if (optionalValidators.Count > 1)
                {
                    result = false;
                    foreach (ValidatorControlPair pair in optionalValidators)
                    {
                        result |= pair.Validator.Validate(pair.Control);
                    }

#if !TRIAL
                    if (NativeFunctions.keyValidated2 != 266)
                        result = false;
#endif

                    if (!result)
                    {
                        foreach (ValidatorControlPair pair in optionalValidators)
                        {
                            if (pair.Validator.DisplayError)
                                SetError(pair.Validator, pair.Control);
                        }
                    }
                    else
                    {
                        foreach (ValidatorControlPair pair in optionalValidators)
                        {
                            ClearError(pair.Validator, pair.Control);
                        }
                    }
                    validateControl = false;
                }
            }

            if (validateControl)
            {
                result = validator.Validate(control);

#if !TRIAL
                if (NativeFunctions.keyValidated2 != 266)
                    result = false;
#endif

                if (!result)
                {
                    if (validator.DisplayError)
                        SetError(validator, control);
                    cancelValidating = _CancelValidatingOnControl;
                }
                else
                {
                    ClearError(validator, control);
                }
            }

            return result;
        }

        private List<ValidatorControlPair> GetOptionalValidators(string optionalValidationGroup)
        {
            List<ValidatorControlPair> list = new List<ValidatorControlPair>();

            FindOptionalValidators(_Validators1, optionalValidationGroup, list);
            FindOptionalValidators(_Validators2, optionalValidationGroup, list);
            FindOptionalValidators(_Validators3, optionalValidationGroup, list);

            return list;
        }

        private void FindOptionalValidators(Dictionary<Control, ValidatorBase> validators, string optionalValidationGroup, List<ValidatorControlPair> list)
        {
            foreach (KeyValuePair<Control, ValidatorBase> item in validators)
            {
                if (item.Value.OptionalValidationGroup.Equals(optionalValidationGroup, StringComparison.CurrentCultureIgnoreCase))
                {
                    list.Add(new ValidatorControlPair(item.Value, item.Key));
                }
            }
        }
        /// <summary>
        /// Raises the GetValue event.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnGetValue(ValidatorGetValueEventArgs e)
        {
            ValidatorGetValueEventHandler handler = GetValue;
            if (handler != null) handler(this, e);
        }

        internal void InvokeGetValue(ValidatorGetValueEventArgs e)
        {
            OnGetValue(e);
        }

        private bool _SteppedValidation = false;
        /// <summary>
        /// Gets or sets whether validation is performed in steps meaning that if first level validation fails, second level is not validated
        /// until first level validation succeeds. Default value is false which means that all validation levels are validated even if first level fails.
        /// </summary>
        [DefaultValue(false), Category("Behavior"), Description("Indicates whether validation is performed in steps meaning that if first level validation fails, second level is not validated until first level validation succeeds")]
        public bool SteppedValidation
        {
            get { return _SteppedValidation; }
            set
            {
                _SteppedValidation = value;
            }
        }

        /// <summary>
        /// Performs validation on all validators. It also uses error provider to display failed validations if validator has that enabled.
        /// </summary>
        /// <returns>Returns true if all validations succeeded or false if at least one validation has failed.</returns>
        public bool Validate()
        {
            bool validated = true;

            Dictionary<string, bool> table = new Dictionary<string, bool>();
            _LastFailedValidations.Clear();

            validated &= ValidateSingleTable(_Validators1, table);
            if (!validated && _SteppedValidation) return validated;
            validated &= ValidateSingleTable(_Validators2, table);
            if (!validated && _SteppedValidation) return validated;
            validated &= ValidateSingleTable(_Validators3, table);

            return validated;
        }

        /// <summary>
        /// Validate single control. Note that control must have validator assigned to it. This method will change LastFailedValidationResults collection.
        /// </summary>
        /// <param name="controlToValidate">Control to validate.</param>
        /// <returns>returns true if validation succeeds or false if it fails</returns>
        public bool Validate(Control controlToValidate)
        {
            bool result = true;
            bool cancelValidating = false;
            _LastFailedValidations.Clear();

            ValidatorBase validator = null;
            if (_Validators1.ContainsKey(controlToValidate))
            {
                validator = _Validators1[controlToValidate];
                bool b = ValidateSingleControl(validator, controlToValidate, out cancelValidating);
                if (!b) _LastFailedValidations.Add(new ValidatorControlPair(validator, controlToValidate));
                result &= b;
            }
            if (!result && _SteppedValidation) return result;

            if (_Validators2.ContainsKey(controlToValidate))
            {
                validator = _Validators2[controlToValidate];
                bool b = ValidateSingleControl(validator, controlToValidate, out cancelValidating);
                if (!b) _LastFailedValidations.Add(new ValidatorControlPair(validator, controlToValidate));
                result &= b;
            }
            if (!result && _SteppedValidation) return result;

            if (_Validators3.ContainsKey(controlToValidate))
            {
                validator = _Validators3[controlToValidate];
                bool b = ValidateSingleControl(validator, controlToValidate, out cancelValidating);
                if (!b) _LastFailedValidations.Add(new ValidatorControlPair(validator, controlToValidate));
                result &= b;
            }
            if (!result && _SteppedValidation) return result;

            return result;
        }
        private List<ValidatorControlPair> _LastFailedValidations = new List<ValidatorControlPair>();
        /// <summary>
        /// Gets the readonly collection that returns failed validations that were result of last Validate method call.
        /// </summary>
        [Browsable(false)]
        public ReadOnlyCollection<ValidatorControlPair> LastFailedValidationResults
        {
            get
            {
                return new ReadOnlyCollection<ValidatorControlPair>(_LastFailedValidations);
            }
        }

        private bool ValidateSingleTable(Dictionary<Control, ValidatorBase> validators, Dictionary<string, bool> table)
        {
            bool validated = true;
            foreach (KeyValuePair<Control, ValidatorBase> item in validators)
            {
                // If control has already failed validation do not validate with second/third level validator again
                if (ControlFailedValidation(item.Key)) continue;
                string optionalValidationGroup = item.Value.OptionalValidationGroup;
                if (!string.IsNullOrEmpty(optionalValidationGroup))
                {
                    if (table.ContainsKey(optionalValidationGroup))
                        continue;
                    else
                        table.Add(optionalValidationGroup, true);
                }
                
                bool cancelValidatingEvent = false;
                bool b = ValidateSingleControl(item.Value, item.Key, out cancelValidatingEvent);
                if (!b) _LastFailedValidations.Add(new ValidatorControlPair(item.Value, item.Key));
                validated &= b;
            }

            return validated;
        }

        private bool ControlFailedValidation(Control control)
        {
            foreach (ValidatorControlPair item in _LastFailedValidations)
            {
                if (item.Control == control) return true;
            }
            return false;
        }
        private void SetError(ValidatorBase validator, Control controlToValidate)
        {
            List<IErrorProvider> errorProviders = GetErrorProviders();
            if (errorProviders == null || errorProviders.Count == 0) return;

            foreach (IErrorProvider errorProvider in errorProviders)
            {
#if !TRIAL
                if (NativeFunctions.keyValidated2 != 266)
                {
                    errorProvider.SetError(controlToValidate, "Invalid license key for SuperValidator control. Control is disabled.");
                    continue;
                }
#endif
                if (errorProvider is Highlighter)
                    ((Highlighter)errorProvider).SetHighlightColor(controlToValidate, validator.HighlightColor);
                else
                    errorProvider.SetError(controlToValidate, validator.ErrorMessage);
            }
        }

        /// <summary>
        /// Removes all visual markers from failed validations that were placed on the controls.
        /// </summary>
        public void ClearFailedValidations()
        {
            foreach (ValidatorControlPair item in _LastFailedValidations)
            {
                ClearError(item.Validator, item.Control);
            }
            _LastFailedValidations.Clear();
        }

        private void ClearError(ValidatorBase validator, Control controlToValidate)
        {
            List<IErrorProvider> errorProviders = GetErrorProviders();
            if (errorProviders == null || errorProviders.Count == 0) return;

            foreach (IErrorProvider errorProvider in errorProviders)
            {
                errorProvider.ClearError(controlToValidate);
            }
        }


        private List<IErrorProvider> GetErrorProviders()
        {
            List<IErrorProvider> providers = new List<IErrorProvider>();
            if (_ErrorProvider != null)
                providers.Add(new ErrorProviderWrapper(_ErrorProvider, (_Highlighter != null ? 2 : 0)));
            if (_CustomErrorProvider != null)
                providers.Add(_CustomErrorProvider);
            if (_Highlighter != null)
                providers.Add(_Highlighter);

            return providers;
        }

        private ErrorProvider _ErrorProvider;
        /// <summary>
        /// Gets or sets the error provider that is used by the validator to report validation errors.
        /// </summary>
        [DefaultValue(null), Category("Behavior"), Description("Indicates error provider that is used by the validator to report validation errors.")]
        public ErrorProvider ErrorProvider
        {
            get { return _ErrorProvider; }
            set { _ErrorProvider = value; }
        }

        private IErrorProvider _CustomErrorProvider = null;
        /// <summary>
        /// Gets or sets the custom error provider that is used by validator to report errors. You can provide your own error validators by implementing IErrorProvider interface.
        /// </summary>
        [DefaultValue(null), Browsable(false), Category("Behavior"), Description("Indicates custom error provider that is used by validator to report errors. You can provide your own error validators by implementing IErrorProvider interface.")]
        public IErrorProvider CustomErrorProvider
        {
            get { return _CustomErrorProvider; }
            set { _CustomErrorProvider = value; }
        }

        private Highlighter _Highlighter;
        /// <summary>
        /// Gets or sets the Highlighter component that is used to highlight validation errors.
        /// </summary>
        [DefaultValue(null), Category("Appearance"), Description("Highlighter component that is used to highlight validation errors.")]
        public Highlighter Highlighter
        {
            get { return _Highlighter; }
            set { _Highlighter = value; }
        }

        private eValidationType _ValidationType = eValidationType.Manual;
        /// <summary>
        /// Gets or sets the validation type performed by the control. Default value is Manual.
        /// </summary>
        [DefaultValue(eValidationType.Manual), Category("Behavior"), Description("Indicates validation type performed by the control.")]
        public eValidationType ValidationType
        {
            get { return _ValidationType; }
            set { _ValidationType = value; }
        }

        private Control _ContainerControl = null;
        /// <summary>
        /// Gets or sets the container control validator is bound to. The container control must be set for the ValidationType ValidatingEventOnContainer.
        /// When ContainerControl is Form the validator handles the Closing event of the form to perform the validation and cancel the form closing.
        /// You can disable that by setting either Enabled=false or Form.CausesValidation=false.
        /// </summary>
        [DefaultValue(null), Description("Indicates container control validator is bound to. Should be set to parent form."), Category("Behavior")]
        public Control ContainerControl
        {
            get
            {
                return _ContainerControl;
            }
            set
            {
                if (_ContainerControl != value)
                {
                    if (_ContainerControl != null)
                    {
                        if (_ContainerControl is Form)
                            ((Form)_ContainerControl).FormClosing -= ContainerFormClosing;
                        else
                            _ContainerControl.Validating -= ContainerValidating;
                    }

                    _ContainerControl = value;

                    if (_ContainerControl != null)
                    {
                        if (_ContainerControl is Form)
                            ((Form)_ContainerControl).FormClosing += ContainerFormClosing;
                        else
                            _ContainerControl.Validating += ContainerValidating;
                    }
                }
            }
        }
        private void ContainerFormClosing(object sender, FormClosingEventArgs e)
        {
            if (!this.Enabled || (_ValidationType & eValidationType.ValidatingEventOnContainer) != eValidationType.ValidatingEventOnContainer || !((Form)sender).CausesValidation)
                return;

            e.Cancel = !Validate();
        }
        private void ContainerValidating(object sender, CancelEventArgs e)
        {
            if (!this.Enabled) return;

            e.Cancel = !Validate();
        }

        private bool _Enabled = true;
        /// <summary>
        /// Gets or sets whether validation is performed. Default value is true.
        /// </summary>
        [DefaultValue(true), Category("Behavior"), Description("Indicates whether validation is performed.")]
        public bool Enabled
        {
            get { return _Enabled; }
            set
            {
                _Enabled = value;
            }
        }

        private bool _CancelValidatingOnControl = true;
        /// <summary>
        /// Gets or sets whether Cancel argument in Validating event for validation type ValidatingEventPerControl is set to true when validation fails.
        /// </summary>
        [DefaultValue(true), Category("Behavior"), Description("Indicates whether Cancel argument in Validating event for validation type ValidatingEventPerControl is set to true when validation fails.")]
        public bool CancelValidatingOnControl
        {
            get { return _CancelValidatingOnControl; }
            set
            {
                _CancelValidatingOnControl = value;
            }
        }

        #endregion

        #region IExtenderProvider Members

        public bool CanExtend(object extendee)
        {
            return extendee is Control;
        }

        #endregion

        #region Licensing
#if !TRIAL
        private string _LicenseKey = "";
        [Browsable(false), DefaultValue("")]
        public string LicenseKey
        {
            get { return _LicenseKey; }
            set
            {
                if (NativeFunctions.ValidateLicenseKey(value))
                    return;
                _LicenseKey = (!NativeFunctions.CheckLicenseKey(value) ? "9dsjkhds7" : value);
            }
        }
#endif
        #endregion
    }

    /// <summary>
    /// Specifies the validation type for SuperValidator control.
    /// </summary>
    [Flags()]
    public enum eValidationType
    {
        /// <summary>
        /// SuperValidator uses manual validation, i.e. you will call Validate method to perform validation.
        /// </summary>
        Manual = 1,
        /// <summary>
        /// Validation is performed per control from each controls Validating event. The Cancel is set to true on event arguments if validation fails.
        /// </summary>
        ValidatingEventPerControl = 2,
        /// <summary>
        /// Validation is performed for all controls from Validating event on container control. By default container control is Form that SuperValidator is on.
        /// </summary>
        ValidatingEventOnContainer = 4
    }
    /// <summary>
    /// Defines validator control pair used by SuperValidator control.
    /// </summary>
    public struct ValidatorControlPair
    {
        /// <summary>
        /// Gets the validator associated with the control.
        /// </summary>
        public readonly ValidatorBase Validator;
        /// <summary>
        /// Gets control reference.
        /// </summary>
        public readonly Control Control;

        /// <summary>
        /// Initializes a new instance of the ValidatorControlPair structure.
        /// </summary>
        /// <param name="validator">Validator associated with the control</param>
        /// <param name="control">Control reference</param>
        public ValidatorControlPair(ValidatorBase validator, Control control)
        {
            Validator = validator;
            Control = control;
        }
    }
}
#endif