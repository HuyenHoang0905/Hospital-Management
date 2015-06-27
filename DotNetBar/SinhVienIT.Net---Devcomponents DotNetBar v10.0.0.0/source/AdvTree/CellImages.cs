using System;
using System.Drawing;
using System.ComponentModel;
using DevComponents.DotNetBar;

namespace DevComponents.AdvTree
{
	/// <summary>
	/// Represents class that holds images for a cell.
	/// </summary>
	/// <remarks>
	/// If you plan to use alpha-blended images we recommend using PNG-24 format which
	/// supports alpha-blending. As of this writing .NET Framework 1.0 and 1.1 do not support
	/// alpha-blending when used through Image class.
	/// </remarks>
	[ToolboxItem(false), TypeConverter(typeof(System.ComponentModel.ExpandableObjectConverter))]
	public class CellImages
	{
		// Image variables
		private System.Drawing.Image m_Image=null;
        private System.Drawing.Image m_DisabledImageGenerated = null;
		private int m_ImageIndex=-1; // Image index if image from ImageList is used
		private System.Drawing.Image m_ImageMouseOver=null;
		private int m_ImageMouseOverIndex=-1; // Image index if image from ImageList is used
		private System.Drawing.Image m_ImageDisabled=null;
//		private bool m_DisabledImageCustom=false;
		private int m_ImageDisabledIndex=-1; // Image index if image from ImageList is used
		private System.Drawing.Image m_ImageExpanded=null;
		private int m_ImageExpandedIndex=-1;
		private Cell m_ParentCell=null;
		private Size m_LargestImageSize=Size.Empty;

		/// <summary>
		/// Initializes new instance of CellImages class.
		/// </summary>
		/// <param name="parentCell">Reference to parent cell.</param>
		public CellImages(Cell parentCell)
		{
			m_ParentCell=parentCell;
		}

		#region Properties

        internal System.Drawing.Image DisabledImageGenerated
        {
            get { return m_DisabledImageGenerated; }
            set
            {
                m_DisabledImageGenerated = value;
            }
        }

		/// <summary>
		/// Gets or sets default cell image. Setting this property to valid image will
		/// override any setting of ImageIndex property.
		/// </summary>
		/// <remarks>
		/// 	<para>The image set through this property will be serialized with the cell. If you
		///     plan to use ImageList then use <see cref="ImageIndex">ImageIndex</see>
		///     property.</para>
		/// 	<para>
		/// 		<para>If you plan to use alpha-blended images we recommend using PNG-24 format
		///         which supports alpha-blending. As of this writing .NET Framework 1.0 and 1.1
		///         do not support alpha-blending when used through Image class.</para>
		/// 	</para>
		/// </remarks>
		/// <value>Image object or <strong>null (Nothing)</strong> if no image is assigned.</value>
		[Browsable(true),DefaultValue(null),Category("Images"),Description("Indicates default cell image"), DevCoSerialize()]
		public System.Drawing.Image Image
		{
			get {return m_Image;}
			set
			{
				ChangeImage(ref m_Image, value);
			}
		}

		/// <summary>
		/// Resets Image property to it's default value (null, VB nothing).
		/// </summary>
		public void ResetImage()
		{
			TypeDescriptor.GetProperties(this)["Image"].SetValue(this, null);
		}

		/// <summary>
		/// Gets or sets the image that is displayed when mouse is over the cell. Setting
		/// this property to valid image will override any setting of ImageMouseOverIndex
		/// property.
		/// </summary>
		/// <remarks>
		/// If you plan to use alpha-blended images we recommend using PNG-24 format which
		/// supports alpha-blending. As of this writting .NET Framework 1.0 and 1.1 do not support
		/// alpha-blending when used through Image class.
		/// </remarks>
		[Browsable(true),DefaultValue(null),Category("Images"),Description("Indicates cell image when mouse is over the cell"), DevCoSerialize()]
		public System.Drawing.Image ImageMouseOver
		{
			get {return m_ImageMouseOver;}
			set
			{
                ChangeImage(ref m_ImageMouseOver, value);
			}
		}

		/// <summary>
		/// Resets ImageMouseOver to it's default value (null, VB nothing).
		/// </summary>
		public void ResetImageMouseOver()
		{
			TypeDescriptor.GetProperties(this)["ImageMouseOver"].SetValue(this, null);
		}

        /// <summary>
        /// Gets or sets the image that is displayed when cell is disabled. If not assigned
        /// disabled image is created from default cell image. Setting this property to valid image
        /// will override any setting of ImageDisabledIndex property.
        /// </summary>
        /// <remarks>
        /// If you plan to use alpha-blended images we recommend using PNG-24 format which
        /// supports alpha-blending. As of this writing .NET Framework 1.0 and 1.1 do not support
        /// alpha-blending when used through Image class.
        /// </remarks>
        [Browsable(true), DefaultValue(null), Category("Images"), Description("Indicates disabled cell image")]
        public System.Drawing.Image ImageDisabled
        {
            get { return m_ImageDisabled; }
            set
            {
                ChangeImage(ref m_ImageDisabled, value);
            }
        }

        /// <summary>
        /// Resets ImageDisabled to it's default value (null, VB nothing).
        /// </summary>
        public void ResetImageDisabled()
        {
            this.ImageDisabled = null;
        }

		/// <summary>
		/// Gets or sets image that is displayed when Node that this cell belongs to is
		/// expanded. Setting this property to valid image will override any setting of
		/// ImageExpandedIndex property.
		/// </summary>
		/// <remarks>
		/// If you plan to use alpha-blended images we recommend using PNG-24 format which
		/// supports alpha-blending. As of this writing .NET Framework 1.0 and 1.1 do not support
		/// alpha-blending when used through Image class.
		/// </remarks>
        [Browsable(true),DefaultValue(null),Category("Images"),Description("Indicates cell image when node associtaed with this cell is expanded"), DevCoSerialize()]
		public System.Drawing.Image ImageExpanded
		{
			get {return m_ImageExpanded;}
			set
			{
                ChangeImage(ref m_ImageExpanded, value);
			}
		}

		/// <summary>
		/// Resets ImageExpanded to it's default value (null, VB nothing).
		/// </summary>
		public void ResetImageExpanded()
		{
			TypeDescriptor.GetProperties(this)["ImageExpanded"].SetValue(this, null);
		}

		/// <summary>
		/// Gets or sets the Index of default cell image from ImageList specified on AdvTree
		/// control.
		/// </summary>
		/// <remarks>
		/// If you plan to use alpha-blended images we recommend using PNG-24 format which
		/// supports alpha-blending. As of this writing .NET Framework 1.0 and 1.1 do not support
		/// alpha-blending when used through Image class.
		/// </remarks>
		[Browsable(true),DefaultValue(-1),Category("ImageList Images"),Description("Indicates default cell image"), DevCoSerialize()]
		public int ImageIndex
		{
			get {return m_ImageIndex;}
			set
			{
				m_ImageIndex=value;
				this.OnImageChanged();
			}
		}

        private string _ImageKey = "";
        /// <summary>
        /// Gets or sets the key of the default cell image from ImageList specified on AdvTree control.
        /// </summary>
        [DefaultValue(""), Category("ImageList Images"), Description("Indicates the default cell image key"), DevCoSerialize()]
        public string ImageKey
        {
            get { return _ImageKey; }
            set
            {
                _ImageKey = value;
                this.OnImageChanged();
            }
        }
        

		/// <summary>
		/// Property Editor support for ImageIndex selection
		/// </summary>
		[System.ComponentModel.Browsable(false),System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public System.Windows.Forms.ImageList ImageList
		{
			get
			{
				if(this.Parent!=null)
				{
					AdvTree tree=this.Parent.TreeControl;
					if(tree!=null)
					{
						return tree.ImageList;
					}
				}

				return null;
			}
		}

		/// <remarks>
		/// If you plan to use alpha-blended images we recommend using PNG-24 format which
		/// supports alpha-blending. As of this writing .NET Framework 1.0 and 1.1 do not support
		/// alpha-blending when used through Image class.
		/// </remarks>
		/// <summary>
		/// Gets or sets the Index of cell image when mouse is over the cell from ImageList
		/// specified on AdvTree control.
		/// </summary>
		[Browsable(true),DefaultValue(-1),Category("ImageList Images"),Description("Indicates cell image when mouse is over the cell"), DevCoSerialize()]
		public int ImageMouseOverIndex
		{
			get {return m_ImageMouseOverIndex;}
			set
			{
				m_ImageMouseOverIndex=value;
				this.OnImageChanged();
			}
		}

        private string _ImageMouseOverKey = "";
        /// <remarks>
        /// If you plan to use alpha-blended images we recommend using PNG-24 format which
        /// supports alpha-blending. As of this writing .NET Framework 1.0 and 1.1 do not support
        /// alpha-blending when used through Image class.
        /// </remarks>
        /// <summary>
        /// Gets or sets the key of cell image when mouse is over the cell from ImageList
        /// specified on AdvTree control.
        /// </summary>
        [Browsable(true), DefaultValue(""), Category("ImageList Images"), Description("Indicates cell image when mouse is over the cell"), DevCoSerialize()]
        public string ImageMouseOverKey
        {
            get { return _ImageMouseOverKey; }
            set
            {
                _ImageMouseOverKey = value;
                this.OnImageChanged();
            }
        }

        /// <remarks>
        /// If you plan to use alpha-blended images we recommend using PNG-24 format which
        /// supports alpha-blending. As of this writing .NET Framework 1.0 and 1.1 do not support
        /// alpha-blending when used through Image class.
        /// </remarks>
        /// <summary>
        /// Gets or sets the Index of disabled cell image from ImageList specified on AdvTree
        /// control.
        /// </summary>
        [Browsable(true), DefaultValue(-1), Category("ImageList Images"), Description("Indicates disabled cell image")]
        public int ImageDisabledIndex
        {
            get { return m_ImageDisabledIndex; }
            set
            {
                m_ImageDisabledIndex = value;
                this.OnImageChanged();
            }
        }

        private string _ImageDisabledKey = "";
        /// <remarks>
        /// If you plan to use alpha-blended images we recommend using PNG-24 format which
        /// supports alpha-blending. As of this writing .NET Framework 1.0 and 1.1 do not support
        /// alpha-blending when used through Image class.
        /// </remarks>
        /// <summary>
        /// Gets or sets the key of disabled cell image from ImageList specified on AdvTree
        /// control.
        /// </summary>
        [Browsable(true), DefaultValue(""), Category("ImageList Images"), Description("Indicates disabled cell image")]
        public string ImageDisabledKey
        {
            get { return _ImageDisabledKey; }
            set
            {
                _ImageDisabledKey = value;
                this.OnImageChanged();
            }
        }

		/// <remarks>
		/// If you plan to use alpha-blended images we recommend using PNG-24 format which
		/// supports alpha-blending. As of this writing .NET Framework 1.0 and 1.1 do not support
		/// alpha-blending when used through Image class.
		/// </remarks>
		/// <summary>
		/// Gets or sets the Index of cell image from ImageList specified on AdvTree control
		/// that is used when Node associated with this cell is expanded
		/// </summary>
		[Browsable(true),DefaultValue(-1),Category("ImageList Images"),Description("Indicates expanded cell image"), DevCoSerialize()]
		public int ImageExpandedIndex
		{
			get {return m_ImageExpandedIndex;}
			set
			{
				m_ImageExpandedIndex=value;
				this.OnImageChanged();
			}
		}

        private string _ImageExpandedKey = "";
        /// <remarks>
        /// If you plan to use alpha-blended images we recommend using PNG-24 format which
        /// supports alpha-blending. As of this writing .NET Framework 1.0 and 1.1 do not support
        /// alpha-blending when used through Image class.
        /// </remarks>
        /// <summary>
        /// Gets or sets the key of cell image from ImageList specified on AdvTree control
        /// that is used when Node associated with this cell is expanded
        /// </summary>
        [Browsable(true), DefaultValue(""), Category("ImageList Images"), Description("Indicates expanded cell image"), DevCoSerialize()]
        public string ImageExpandedKey
        {
            get { return _ImageExpandedKey; }
            set
            {
                _ImageExpandedKey = value;
                this.OnImageChanged();
            }
        }

		/// <summary>
		/// Gets or sets the parent node of the cell.
		/// </summary>
		[Browsable(false),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Cell Parent
		{
			get {return m_ParentCell;}
			set {m_ParentCell=value;}
		}

		/// <summary>
		/// Gets whether CellImages object should be serialized or not. If object has all
		/// default values then this property will return <strong>false</strong>.
		/// </summary>
		internal bool ShouldSerialize
		{
			get
			{
				if(m_Image==null && m_ImageDisabled==null && m_ImageDisabledIndex==-1 &&
					m_ImageExpanded==null && m_ImageExpandedIndex==-1 && m_ImageIndex==-1 &&
					m_ImageMouseOver==null && m_ImageMouseOverIndex==-1)
					return false;
				return true;
			}
		}

		/// <summary>
		/// Returns largest image size in this set of images.
		/// </summary>
		internal Size LargestImageSize
		{
			get
			{
				return m_LargestImageSize;
			}
		}

		#endregion

		#region Methods

		/// <summary>Makes a copy of a CellImages object.</summary>
		public virtual CellImages Copy()
		{
			CellImages ci=new CellImages(null);
			ci.Image=this.Image;
//			ci.ImageDisabled=this.ImageDisabled;
//			ci.ImageDisabledIndex=this.ImageDisabledIndex;
		    ci.ImageExpanded = this.ImageExpanded == null ? null : (Image) this.ImageExpanded.Clone();
			ci.ImageExpandedIndex=this.ImageExpandedIndex;
			ci.ImageIndex=this.ImageIndex;
			ci.ImageMouseOver=this.ImageMouseOver == null? null : (Image)this.ImageMouseOver.Clone();
			ci.ImageMouseOverIndex=this.ImageMouseOverIndex;
            ci.ImageDisabledKey = this.ImageDisabledKey;
            ci.ImageExpandedKey = this.ImageExpandedKey;
            ci.ImageKey = this.ImageKey;
            ci.ImageMouseOverKey = this.ImageMouseOverKey;
			return ci;
		}

		#endregion

		#region Internals
        /// <summary>
        /// Changes the image and invokes largest image size calculation if the
        /// image size truly changed.
        /// </summary>
        /// <param name="currentImage"></param>
        /// <param name="newImage"></param>
        private void ChangeImage(ref System.Drawing.Image currentImage, System.Drawing.Image newImage)
        {
            // Early out if no real change
            if (currentImage == newImage)
                return;

            // Hold onto previous image
            System.Drawing.Image previousImage = currentImage;

            // Assign new image
            currentImage = newImage;

            // If either current or previous is null, or the sizes don't match,
            // we need to resize ourselves and the parent.
            if (previousImage == null || currentImage == null || GetImageSize(previousImage) != GetImageSize(currentImage))
            {
                RefreshLargestImageSize();

                if (this.Parent != null)
                    this.Parent.OnImageChanged();
            }

            // Dispose the generated disabled image, if applicable.
            DisposeGeneratedDisabledImage();
        }
        private Size GetImageSize(Image image)
        {
            try
            {
                return image.Size;
            }
            catch
            {
                return Size.Empty;
            }
        }

        private void OnImageChanged()
        {
            RefreshLargestImageSize();
            if (this.Parent != null)
                this.Parent.OnImageChanged();
            DisposeGeneratedDisabledImage();
        }

        public void Dispose()
        {
            DisposeGeneratedDisabledImage();
            if (BarUtilities.DisposeItemImages)
            {
                BarUtilities.DisposeImage(ref m_Image);
                BarUtilities.DisposeImage(ref m_ImageDisabled);
                BarUtilities.DisposeImage(ref m_ImageExpanded);
                BarUtilities.DisposeImage(ref m_ImageMouseOver);
            }
        }

        internal void DisposeGeneratedDisabledImage()
        {
            if (m_DisabledImageGenerated != null)
            {
                m_DisabledImageGenerated.Dispose();
                m_DisabledImageGenerated = null;
            }
        }

		internal void RefreshLargestImageSize()
		{
			m_LargestImageSize=Size.Empty;
			AdjustSize(m_Image,ref m_LargestImageSize);
			AdjustSize(m_ImageDisabled,ref m_LargestImageSize);
			AdjustSize(m_ImageExpanded,ref m_LargestImageSize);
			AdjustSize(m_ImageMouseOver,ref m_LargestImageSize);
            
			AdjustSize(GetImageByIndex(m_ImageIndex),ref m_LargestImageSize);
			AdjustSize(GetImageByIndex(m_ImageDisabledIndex),ref m_LargestImageSize);
			AdjustSize(GetImageByIndex(m_ImageExpandedIndex),ref m_LargestImageSize);
			AdjustSize(GetImageByIndex(m_ImageMouseOverIndex),ref m_LargestImageSize);

            AdjustSize(GetImageByKey(_ImageKey), ref m_LargestImageSize);
            AdjustSize(GetImageByKey(_ImageDisabledKey), ref m_LargestImageSize);
            AdjustSize(GetImageByKey(_ImageExpandedKey), ref m_LargestImageSize);
            AdjustSize(GetImageByKey(_ImageMouseOverKey), ref m_LargestImageSize);
		}
		private void AdjustSize(System.Drawing.Image image, ref Size size)
		{
			if(image!=null)
			{
				if(image.Width>size.Width)
					size.Width=image.Width;
				if(image.Height>size.Height)
					size.Height=image.Height;
			}
		}
		/// <summary>
		/// Returns image from image list based on the image index.
		/// </summary>
		/// <param name="imageIndex">Index of the image to return.</param>
		/// <returns>Image object from image list.</returns>
		internal System.Drawing.Image GetImageByIndex(int imageIndex)
		{
            if (imageIndex >= 0 && this.Parent != null && this.Parent.TreeControl != null && this.Parent.TreeControl.ImageList != null && this.Parent.TreeControl.ImageList.Images.Count > 0)
            {
                try
                {
                    return this.Parent.TreeControl.ImageList.Images[imageIndex];
                }
                catch
                {
                    return null;
                }
                
            }
            else
                return null;
		}

        /// <summary>
        /// Returns image from image list based on the image key.
        /// </summary>
        /// <param name="key">Key of the image to return.</param>
        /// <returns>Image object from image list.</returns>
        internal System.Drawing.Image GetImageByKey(string key)
        {
            if (string.IsNullOrEmpty(key)) return null;

            Cell parent = this.Parent;
            if (parent == null) return null;

            AdvTree tree = parent.TreeControl;
            if (tree == null || tree.ImageList == null || !tree.ImageList.Images.ContainsKey(key)) return null;

            return tree.ImageList.Images[key];
        }
		#endregion
	}
}
