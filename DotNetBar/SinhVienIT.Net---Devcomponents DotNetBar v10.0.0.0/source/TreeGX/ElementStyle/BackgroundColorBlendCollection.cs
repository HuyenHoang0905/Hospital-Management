using System;
using System.Collections;
using System.Text;
using System.Drawing.Drawing2D;
using System.Drawing;

#if TREEGX
namespace DevComponents.Tree
#elif DOTNETBAR
namespace DevComponents.DotNetBar
#endif
{
    /// <summary>
    /// Represents Collection for the BackgroundColorBlend objects.
    /// </summary>
    public class BackgroundColorBlendCollection : CollectionBase
    {
        #region Private Variables
        private ElementStyle m_Parent = null; 
        #endregion

        #region Internal Implementation
        /// <summary>Creates new instance of the class.</summary>
        public BackgroundColorBlendCollection() {}

        internal ElementStyle Parent
        {
            get { return m_Parent; }
            set { m_Parent = value; }
        }

        /// <summary>
		/// Adds new object to the collection.
		/// </summary>
        /// <param name="item">Object to add.</param>
		/// <returns>Index of newly added object.</returns>
		public int Add(BackgroundColorBlend item)
		{
			return List.Add(item);
		}

        /// <summary>
        /// Adds array of new objects to the collection.
        /// </summary>
        /// <param name="items">Array of object to add.</param>
        public void AddRange(BackgroundColorBlend[] items)
        {
            foreach (BackgroundColorBlend item in items)
                this.Add(item);
        }

		/// <summary>
		/// Returns reference to the object in collection based on it's index.
		/// </summary>
		public BackgroundColorBlend this[int index]
		{
			get {return (BackgroundColorBlend)(List[index]);}
			set {List[index] = value;}
		}

		/// <summary>
		/// Inserts new object into the collection.
		/// </summary>
		/// <param name="index">Position of the object.</param>
		/// <param name="value">Object to insert.</param>
		public void Insert(int index, BackgroundColorBlend value) 
		{
			List.Insert(index, value);
		}

		/// <summary>
		/// Returns index of the object inside of the collection.
		/// </summary>
		/// <param name="value">Reference to the object.</param>
		/// <returns>Index of the object.</returns>
		public int IndexOf(BackgroundColorBlend value) 
		{
			return List.IndexOf(value);
		}

		/// <summary>
		/// Returns whether collection contains specified object.
		/// </summary>
		/// <param name="value">Object to look for.</param>
		/// <returns>true if object is part of the collection, otherwise false.</returns>
		public bool Contains(BackgroundColorBlend value) 
		{
			return List.Contains(value);
		}

		/// <summary>
		/// Removes specified object from the collection.
		/// </summary>
		/// <param name="value"></param>
		public void Remove(BackgroundColorBlend value) 
		{
			List.Remove(value);
		}

        //protected override void OnRemoveComplete(int index,object value)
        //{
        //    base.OnRemoveComplete(index,value);
        //    BackgroundColorBlend me=value as BackgroundColorBlend;
        //}
        //protected override void OnInsertComplete(int index,object value)
        //{
        //    base.OnInsertComplete(index,value);
        //    BackgroundColorBlend me=value as BackgroundColorBlend;
        //}

		/// <summary>
		/// Copies collection into the specified array.
		/// </summary>
		/// <param name="array">Array to copy collection to.</param>
		/// <param name="index">Starting index.</param>
		public void CopyTo(BackgroundColorBlend[] array, int index) 
		{
			List.CopyTo(array, index);
		}

		/// <summary>
		/// Copies contained items to the BackgroundColorBlend array.
		/// </summary>
		/// <param name="array">Array to copy to.</param>
		internal void CopyTo(BackgroundColorBlend[] array)
		{
			List.CopyTo(array,0);
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

            for(int i=0; i<this.Count; i++)
            {
                BackgroundColorBlend b = this[i];
                colors[i] = b.Color;
                positions[i] = b.Position;
            }

            blend.Colors = colors;
            blend.Positions = positions;
            
            return blend;
        }

        /// <summary>
        /// Adds the BackgroundColorBlend objects from the collection.
        /// </summary>
        /// <param name="col">Collection to copy objects from</param>
        public void CopyFrom(BackgroundColorBlendCollection col)
        {
            foreach (BackgroundColorBlend b in col)
                this.Add(b);
        }

        internal eBackgroundColorBlendType GetBlendType()
        {
            BackgroundColorBlendCollection c = this;
            if (c.Count <= 1)
                return eBackgroundColorBlendType.Invalid;

            eBackgroundColorBlendType t = eBackgroundColorBlendType.Invalid;

            foreach (BackgroundColorBlend b in c)
            {
                if (b.Position == 0 || b.Position == 1f)
                    continue;
                if (b.Position <= 1f)
                {
                    if (t == eBackgroundColorBlendType.Invalid)
                        t = eBackgroundColorBlendType.Relative;
                    else if (t == eBackgroundColorBlendType.Absolute)
                    {
                        t = eBackgroundColorBlendType.Invalid;
                        break;
                    }
                }
                else
                {
                    if (t == eBackgroundColorBlendType.Invalid)
                        t = eBackgroundColorBlendType.Absolute;
                    else if (t == eBackgroundColorBlendType.Relative)
                    {
                        t = eBackgroundColorBlendType.Invalid;
                        break;
                    }
                }
            }

            if (c.Count == 2 && c[0].Position == 0f && c[1].Position == 1f)
                return eBackgroundColorBlendType.Relative;

            if (t == eBackgroundColorBlendType.Invalid)
                return t;

            if (t == eBackgroundColorBlendType.Relative && c[0].Position != 0f && c[c.Count - 1].Position != 1f)
                return eBackgroundColorBlendType.Invalid;
            else if (t == eBackgroundColorBlendType.Absolute && ((c.Count / 2) * 2 != c.Count))
                return eBackgroundColorBlendType.Invalid;

            return t;
        }

        /// <summary>
        /// Initializes the collection with the two color blend.
        /// </summary>
        /// <param name="collection">Collection to initialize.</param>
        /// <param name="backColor1">Start color.</param>
        /// <param name="backColor2">End color.</param>
        public static void InitializeCollection(BackgroundColorBlendCollection collection, int backColor1, int backColor2)
        {
            InitializeCollection(collection, ColorScheme.GetColor(backColor1), ColorScheme.GetColor(backColor2));
        }

        /// <summary>
        /// Initializes the collection with the two color blend.
        /// </summary>
        /// <param name="collection">Collection to initialize.</param>
        /// <param name="backColor1">Start color.</param>
        /// <param name="backColor2">End color.</param>
        public static void InitializeCollection(BackgroundColorBlendCollection collection, Color backColor1, Color backColor2)
        {
            collection.Clear();
            collection.Add(new BackgroundColorBlend(backColor1, 0f));
            collection.Add(new BackgroundColorBlend(backColor2, 1f));
        }
		#endregion
    }

    internal enum eBackgroundColorBlendType
    {
        Invalid,
        Relative,
        Absolute
    }
}
