using System.Collections.Generic;

namespace DevComponents.Schedule.Model.Serialization
{
    public class CalendarEntry
    {
        #region Private variables

        private string _Id = "";
        private string _Value = "";

        private List<AttributeData> _Attributes;

        private int _LineStart;
        private int _LineEnd;

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="lineStart">Text line start</param>
        /// <param name="lineEnd">Text line end</param>
        public CalendarEntry(int lineStart, int lineEnd)
        {
            _LineStart = lineStart;
            _LineEnd = lineEnd;

            _Attributes = new List<AttributeData>();
        }

        #region Public properties

        #region Attributes

        /// <summary>
        /// Attributes
        /// </summary>
        public List<AttributeData> Attributes
        {
            get { return (_Attributes); }
            set { _Attributes = value; }
        }

        #endregion

        #region Id

        /// <summary>
        /// Id
        /// </summary>
        public string Id
        {
            get { return (_Id); }
            set { _Id = value; }
        }

        #endregion

        #region LineEnd

        /// <summary>
        /// LineEnd
        /// </summary>
        public int LineEnd
        {
            get { return (_LineEnd); }
            set { _LineEnd = value; }
        }

        #endregion

        #region LineStart

        /// <summary>
        /// LineStart
        /// </summary>
        public int LineStart
        {
            get { return (_LineStart); }
            set { _LineStart = value; }
        }

        #endregion

        #region Value

        /// <summary>
        /// Value
        /// </summary>
        public string Value
        {
            get { return (_Value); }
            set { _Value = value; }
        }

        #endregion

        #endregion
    }

    #region AttributeData

    public class AttributeData
    {
        #region Private variables

        private string _Id;
        private string _Value;

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="value">Value</param>
        public AttributeData(string id, string value)
        {
            _Id = id;
            _Value = value;
        }

        #region Public properties

        #region Id

        /// <summary>
        /// Gets or sets the attribute Id
        /// </summary>
        public string Id
        {
            get { return (_Id); }
            set { _Id = value; }
        }

        #endregion

        #region Value

        /// <summary>
        /// Gets or sets the attribute value
        /// </summary>
        public string Value
        {
            get { return (_Value); }
            set { _Value = value; }
        }

        #endregion

        #endregion
    }

    #endregion
}
