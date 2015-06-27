using System;
using System.Text;
using System.Collections;

namespace DevComponents.DotNetBar.Presentation
{
    internal class ShapeCollection : CollectionBase
    {
        #region Internal Implementation
        /// <summary>Creates new instance of the class.</summary>
        public ShapeCollection()
		{
		}

        /// <summary>
		/// Adds new object to the collection.
		/// </summary>
		/// <param name="Shape">Object to add.</param>
		/// <returns>Index of newly added object.</returns>
		public int Add(Shape shape)
		{
			return List.Add(shape);
		}
		/// <summary>
		/// Returns reference to the object in collection based on it's index.
		/// </summary>
		public Shape this[int index]
		{
			get {return (Shape)(List[index]);}
			set {List[index] = value;}
		}

		/// <summary>
		/// Inserts new object into the collection.
		/// </summary>
		/// <param name="index">Position of the object.</param>
		/// <param name="value">Object to insert.</param>
		public void Insert(int index, Shape value) 
		{
			List.Insert(index, value);
		}

		/// <summary>
		/// Returns index of the object inside of the collection.
		/// </summary>
		/// <param name="value">Reference to the object.</param>
		/// <returns>Index of the object.</returns>
		public int IndexOf(Shape value) 
		{
			return List.IndexOf(value);
		}

		/// <summary>
		/// Returns whether collection contains specified object.
		/// </summary>
		/// <param name="value">Object to look for.</param>
		/// <returns>true if object is part of the collection, otherwise false.</returns>
		public bool Contains(Shape value) 
		{
			return List.Contains(value);
		}

		/// <summary>
		/// Removes specified object from the collection.
		/// </summary>
		/// <param name="value"></param>
		public void Remove(Shape value) 
		{
			List.Remove(value);
		}

        //protected override void OnRemoveComplete(int index,object value)
        //{
        //    base.OnRemoveComplete(index,value);
        //    Shape me=value as Shape;
        //}
        //protected override void OnInsertComplete(int index,object value)
        //{
        //    base.OnInsertComplete(index,value);
        //    Shape me=value as Shape;
        //}

		/// <summary>
		/// Copies collection into the specified array.
		/// </summary>
		/// <param name="array">Array to copy collection to.</param>
		/// <param name="index">Starting index.</param>
		public void CopyTo(Shape[] array, int index) 
		{
			List.CopyTo(array, index);
		}

		/// <summary>
		/// Copies contained items to the Shape array.
		/// </summary>
		/// <param name="array">Array to copy to.</param>
		internal void CopyTo(Shape[] array)
		{
			List.CopyTo(array,0);
		}
		#endregion
    }
}
