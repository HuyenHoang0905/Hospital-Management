using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms.Design;
using DevComponents.Instrumentation;

namespace DevComponents.Instrumentation.Design
{
    /// <summary>
    /// KnobControlDesigner
    /// </summary>
    public class KnobControlDesigner : ControlDesigner
    {
        #region Private variables

        private KnobControl _KnobControl;
        private DesignerActionListCollection _ActionLists;

        #endregion

        #region Initialize

        /// <summary>
        /// Initializes our designer
        /// </summary>
        /// <param name="component"></param>
        public override void Initialize(IComponent component)
        {
            base.Initialize(component);

            if (component.Site.DesignMode == true)
                _KnobControl = component as KnobControl;
        }

        #endregion

        #region Verbs

        /// <summary>
        /// Creates our verb collection
        /// </summary>
        public override DesignerVerbCollection Verbs
        {
            get
            {
                DesignerVerb[] verbs = new DesignerVerb[]
                {
                    new DesignerVerb("KnobStyle 1", SetStyle1),
                    new DesignerVerb("KnobStyle 2", SetStyle2),
                    new DesignerVerb("KnobStyle 3", SetStyle3),
                    new DesignerVerb("KnobStyle 4", SetStyle4),
                };

                return (new DesignerVerbCollection(verbs));
            }
        }

        #endregion

        #region SetStyle

        /// <summary>
        /// Sets the control to Style1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void SetStyle1(object sender, EventArgs e)
        {
            _KnobControl.KnobStyle = eKnobStyle.Style1;
        }

        /// <summary>
        /// Sets the control to Style2
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void SetStyle2(object sender, EventArgs e)
        {
            _KnobControl.KnobStyle = eKnobStyle.Style2;
        }

        /// <summary>
        /// Sets the control to Style3
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void SetStyle3(object sender, EventArgs e)
        {
            _KnobControl.KnobStyle = eKnobStyle.Style3;
        }

        /// <summary>
        /// Sets the control to Style4
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void SetStyle4(object sender, EventArgs e)
        {
            _KnobControl.KnobStyle = eKnobStyle.Style4;
        }

        #endregion

        #region ActionLists

        /// <summary>
        /// Gets our DesignerActionListCollection list
        /// </summary>
        public override DesignerActionListCollection ActionLists
        {
            get
            {
                if (_ActionLists == null)
                {
                    _ActionLists = new DesignerActionListCollection();

                    _ActionLists.Add(new KnobControlActionList(_KnobControl));
                }

                return (_ActionLists);
            }
        }

        #endregion
    }
}
