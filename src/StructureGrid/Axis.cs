using System.Xml.Serialization;

#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.StructureGrid
{
    [System.Serializable]
    [IsVisibleInDynamoLibrary(false)]
    public class Axis: EntityBase
    {
        [XmlElement("start_point", Order = 1)]
        public Geometry.FdPoint2d _startPoint; // point_type_2d
        [XmlIgnore]
        public Geometry.FdPoint3d StartPoint
        {
            get { return this._startPoint.To3d(); }
            set { this._startPoint = value.To2d(); }
        }
        [XmlElement("end_point", Order = 2)]
        public Geometry.FdPoint2d _endPoint; // point_type_2d
        [XmlIgnore]
        public Geometry.FdPoint3d EndPoint
        {
            get { return this._endPoint.To3d(); }
            set { this._endPoint = value.To2d(); }
        }
        [XmlAttribute("prefix")]
        public string _prefix; // string15
        [XmlIgnore]
        public string Prefix
        {
             get
             {
                 return this._prefix;
             }
             set
             {
                 this._prefix = RestrictedString.Length(value, 15);
             }
        }
        [XmlAttribute("id")]
        public int _id; // int_1_to_1024
        [XmlIgnore]
        public int Id
        {
            get
            {
                return this._id;
            }
            set
            {
                this._id = RestrictedInteger.ValueInRange(value, 1, 1024);
            }
        }
        [XmlAttribute("id_is_letter")]
        public bool IdIsLetter { get; set; } // bool


        /// <summary>
        /// Private constructor for serialization
        /// </summary>
        private Axis()
        {

        }

        /// <summary>
        /// Internal constructor.
        /// </summary>
        /// <param name="startPoint">Start point of axis.</param>
        /// <param name="endPoint">End point of axis.</param>
        /// <param name="prefix">Prefix of axis identifier.</param>
        /// <param name="id">Number of axis identifier.</param>
        /// <param name="idIsLetter">Is identifier numbering a letter?</param>
        public Axis(Geometry.FdPoint3d startPoint, Geometry.FdPoint3d endPoint, string prefix, int id, bool idIsLetter)
        {
            this.EntityCreated();
            this.StartPoint = startPoint;
            this.EndPoint = endPoint;
            this.Prefix = prefix;
            this.Id = id;
            this.IdIsLetter = idIsLetter;
        }

        #region dynamo
        /// <summary>
        /// Create an axis.
        /// </summary>
        /// <param name="line">Line of axis. Line will be projected onto XY-plane.</param>
        /// <param name="prefix">Prefix of axis identifier.</param>
        /// <param name="id">Number of axis identifier. Number can be converted to letter.</param>
        /// <param name="idIsLetter">Is identifier a letter?</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        public static Axis Define(Autodesk.DesignScript.Geometry.Line line, [DefaultArgument("")] string prefix, int id, [DefaultArgument("false")] bool idIsLetter)
        {
            // convert geometry
            Geometry.FdPoint3d p0 = Geometry.FdPoint3d.FromDynamo(line.StartPoint);
            Geometry.FdPoint3d p1 = Geometry.FdPoint3d.FromDynamo(line.EndPoint);

            //
            return new Axis(p0, p1, prefix, id, idIsLetter);

        }
        #endregion
    }
}