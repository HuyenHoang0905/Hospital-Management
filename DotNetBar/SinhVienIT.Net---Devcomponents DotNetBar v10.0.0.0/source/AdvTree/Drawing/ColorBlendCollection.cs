using System;
using System.Collections;
using System.Text;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace DevComponents.WinForms.Drawing
{
    /// <summary>
    /// Represents Collection for the ColorStop objects.
    /// </summary>
    public class ColorBlendCollection : CollectionBase
    {
        #region Private Variables
        
        #endregion

        #region Internal Implementation
        /// <summary>Creates new instance of the class.</summary>
        public ColorBlendCollection() { }

        /// <summary>
        /// Adds new object to the collection.
        /// </summary>
        /// <param name="item">Object to add.</param>
        /// <returns>Index of newly added object.</returns>
        public int Add(ColorStop item)
        {
            return List.Add(item);
        }

        /// <summary>
        /// Adds array of new objects to the collection.
        /// </summary>
        /// <param name="items">Array of object to add.</param>
        public void AddRange(ColorStop[] items)
        {
            foreach (ColorStop item in items)
                this.Add(item);
        }

        /// <summary>
        /// Returns reference to the object in collection based on it's index.
        /// </summary>
        public ColorStop this[int index]
        {
            get { return (ColorStop)(List[index]); }
            set { List[index] = value; }
        }

        /// <summary>
        /// Inserts new object into the collection.
        /// </summary>
        /// <param name="index">Position of the object.</param>
        /// <param name="value">Object to insert.</param>
        public void Insert(int index, ColorStop value)
        {
            List.Insert(index, value);
        }

        /// <summary>
        /// Returns index of the object inside of the collection.
        /// </summary>
        /// <param name="value">Reference to the object.</param>
        /// <returns>Index of the object.</returns>
        public int IndexOf(ColorStop value)
        {
            return List.IndexOf(value);
        }

        /// <summary>
        /// Returns whether collection contains specified object.
        /// </summary>
        /// <param name="value">Object to look for.</param>
        /// <returns>true if object is part of the collection, otherwise false.</returns>
        public bool Contains(ColorStop value)
        {
            return List.Contains(value);
        }

        /// <summary>
        /// Removes specified object from the collection.
        /// </summary>
        /// <param name="value"></param>
        public void Remove(ColorStop value)
        {
            List.Remove(value);
        }

        //protected override void OnRemoveComplete(int index,object value)
        //{
        //    base.OnRemoveComplete(index,value);
        //    ColorStop me=value as ColorStop;
        //}
        //protected override void OnInsertComplete(int index,object value)
        //{
        //    base.OnInsertComplete(index,value);
        //    ColorStop me=value as ColorStop;
        //}

        /// <summary>
        /// Copies collection into the specified array.
        /// </summary>
        /// <param name="array">Array to copy collection to.</param>
        /// <param name="index">Starting index.</param>
        public void CopyTo(ColorStop[] array, int index)
        {
            List.CopyTo(array, index);
        }

        /// <summary>
        /// Copies contained items to the ColorStop array.
        /// </summary>
        /// <param name="array">Array to copy to.</param>
        internal void CopyTo(ColorStop[] array)
        {
            List.CopyTo(array, 0);
        }

        /// <summary>
        /// Creates ColorBlend object based on the members of the collection. ColorBlend object will be valid only if all members of the collection
        /// represents relative/percentage based color blends.
        /// </summary>
        /// <returns></returns>
        public ColorBlend GetColorBlend()
        {
            ColorBlend blend = new ColorBlend();
            Color[] colors = new Color[this.Count];
            float[] positions = new float[this.Count];

            for (int i = 0; i < this.Count; i++)
            {
                ColorStop b = this[i];
                colors[i] = b.Color;
                positions[i] = b.Position;
            }

            blend.Colors = colors;
            blend.Positions = positions;

            return blend;
        }

        /// <summary>
        /// Adds the ColorStop objects from the collection.
        /// </summary>
        /// <param name="col">Collection to copy objects from</param>
        public void CopyFrom(ColorBlendCollection col)
        {
            foreach (ColorStop b in col)
                this.Add(b);
        }

        internal eColorStopType GetBlendType()
        {
            ColorBlendCollection c = this;
            if (c.Count <= 1)
                return eColorStopType.Invalid;

            eColorStopType t = eColorStopType.Invalid;

            foreach (ColorStop b in c)
            {
                if (b.Position == 0 || b.Position == 1f)
                    continue;
                if (b.Position <= 1f)
                {
                    if (t == eColorStopType.Invalid)
                        t = eColorStopType.Relative;
                    else if (t == eColorStopType.Absolute)
                    {
                        t = eColorStopType.Invalid;
                        break;
                    }
                }
                else
                {
                    if (t == eColorStopType.Invalid)
                        t = eColorStopType.Absolute;
                    else if (t == eColorStopType.Relative)
                    {
                        t = eColorStopType.Invalid;
                        break;
                    }
                }
            }

            if (c.Count == 2 && c[0].Position == 0f && c[1].Position == 1f)
                return eColorStopType.Relative;

            if (t == eColorStopType.Invalid)
                return t;

            if (t == eColorStopType.Relative && c[0].Position != 0f && c[c.Count - 1].Position != 1f)
                return eColorStopType.Invalid;
            else if (t == eColorStopType.Absolute && ((c.Count / 2) * 2 != c.Count))
                return eColorStopType.Invalid;

            return t;
        }

        ///// <summary>
        ///// Initializes the collection with the two color blend.
        ///// </summary>
        ///// <param name="collection">Collection to initialize.</param>
        ///// <param name="backColor1">Start color.</param>
        ///// <param name="backColor2">End color.</param>
        //public static void InitializeCollection(ColorBlendCollection collection, int backColor1, int backColor2)
        //{
        //    InitializeCollection(collection, ColorScheme.GetColor(backColor1), ColorScheme.GetColor(backColor2));
        //}

        /// <summary>
        /// Initializes the collection with the two color blend.
        /// </summary>
        /// <param name="collection">Collection to initialize.</param>
        /// <param name="backColor1">Start color.</param>
        /// <param name="backColor2">End color.</param>
        public static void InitializeCollection(ColorBlendCollection collection, Color backColor1, Color backColor2)
        {
            collection.Clear();
            collection.Add(new ColorStop(backColor1, 0f));
            collection.Add(new ColorStop(backColor2, 1f));
        }
        #endregion
    }

    internal enum eColorStopType
    {
        Invalid,
        Relative,
        Absolute
    }
}
