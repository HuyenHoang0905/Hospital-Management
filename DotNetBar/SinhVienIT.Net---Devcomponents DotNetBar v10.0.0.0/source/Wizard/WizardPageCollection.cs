using System;
using System.Collections;
using System.ComponentModel;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Represents the collection of WizardPage objects which determines the flow of the wizard.
    /// </summary>
    public class WizardPageCollection : CollectionBase
    {
        #region Private Variables
		private Wizard m_Parent=null;
        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IgnoreEvents = false;
		#endregion

		#region Internal Implementation
        public WizardPageCollection()
		{
		}
		/// <summary>
		/// Gets the parent this collection is associated with.
		/// </summary>
		[Browsable(false),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Wizard Parent
		{
			get {return m_Parent;}
		}

		internal Wizard ParentWizard
		{
            get { return m_Parent; }
            set { m_Parent = value; }
		}

		/// <summary>
		/// Adds new object to the collection.
		/// </summary>
		/// <param name="WizardPage">Object to add.</param>
		/// <returns>Index of newly added object.</returns>
		public int Add(WizardPage WizardPage)
		{
			return List.Add(WizardPage);
		}
		
		/// <summary>
		/// Adds an array of objects to the collection. 
		/// </summary>
		/// <param name="WizardPages">Array of WizardPage objects.</param>
		public void AddRange(WizardPage[] WizardPages)
		{
			foreach(WizardPage WizardPage in WizardPages)
				List.Add(WizardPage);
		}
		
		/// <summary>
		/// Returns reference to the object in collection based on it's index.
		/// </summary>
		public WizardPage this[int index]
		{
			get {return (WizardPage)(List[index]);}
			set {List[index] = value;}
		}

        /// <summary>
        /// Returns reference to the object in collection based on it's name.
        /// </summary>
        public WizardPage this[string name]
        {
            get
            {
                foreach(WizardPage page in this.List)
                {
                    if (page.Name == name)
                        return page;
                }
                return null;
            }
        }

		/// <summary>
		/// Inserts new object into the collection.
		/// </summary>
		/// <param name="index">Position of the object.</param>
		/// <param name="value">Object to insert.</param>
		public void Insert(int index, WizardPage value) 
		{
			List.Insert(index, value);
		}
		
		/// <summary>
		/// Returns index of the object inside of the collection.
		/// </summary>
		/// <param name="value">Reference to the object.</param>
		/// <returns>Index of the object.</returns>
		public int IndexOf(WizardPage value) 
		{
			return List.IndexOf(value);
		}

		/// <summary>
		/// Returns whether collection contains specified object.
		/// </summary>
		/// <param name="value">Object to look for.</param>
		/// <returns>true if object is part of the collection, otherwise false.</returns>
		public bool Contains(WizardPage value) 
		{
			return List.Contains(value);
		}

		/// <summary>
		/// Removes specified object from the collection.
		/// </summary>
		/// <param name="value"></param>
		public void Remove(WizardPage value) 
		{
			List.Remove(value);
		}
		
		protected override void OnRemove(int index, object value)
		{
			
			base.OnRemove (index, value);
		}


		protected override void OnRemoveComplete(int index,object value)
		{
			base.OnRemoveComplete(index,value);
            if (IgnoreEvents)
                return;

            if (m_Parent != null)
                m_Parent.OnWizardPageRemoved(value as WizardPage);
		}
		
		protected override void OnInsertComplete(int index,object value)
		{
			base.OnInsertComplete(index,value);

            if (IgnoreEvents)
                return;

            if (m_Parent != null)
                m_Parent.OnWizardPageAdded(value as WizardPage);
		}

		/// <summary>
		/// Copies collection into the specified array.
		/// </summary>
		/// <param name="array">Array to copy collection to.</param>
		/// <param name="index">Starting index.</param>
		public void CopyTo(WizardPage[] array, int index) 
		{
			List.CopyTo(array, index);
		}

		/// <summary>
		/// Copies contained items to the WizardPage array.
		/// </summary>
		/// <param name="array">Array to copy to.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
		public void CopyTo(WizardPage[] array)
		{
			List.CopyTo(array,0);
		}

        /// <summary>
        /// Copies contained items to the WizardPage array.
        /// </summary>
        /// <param name="array">Array to copy to.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void CopyTo(WizardPageCollection col)
        {
            foreach (WizardPage page in this.List)
                col.Add(page);
        }

		protected override void OnClear()
		{
            if (!IgnoreEvents)
            {
                if (m_Parent != null)
                {
                    foreach (WizardPage page in this.List)
                    {
                        m_Parent.OnWizardPageRemoved(page);
                    }
                }
            }
			base.OnClear();
		}
		
		#endregion
    }
}
