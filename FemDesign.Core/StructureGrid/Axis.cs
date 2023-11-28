using System.Reflection;
using System.Xml.Serialization;
using FemDesign.GenericClasses;


namespace FemDesign.StructureGrid
{
    [System.Serializable]
    public partial class Axis: EntityBase, IStructureElement
    {
        [XmlElement("start_point", Order = 1)]
        public Geometry.Point2d _startPoint; // point_type_2d
        [XmlIgnore]
        public Geometry.Point3d StartPoint
        {
            get { return this._startPoint.To3d(); }
            set { this._startPoint = value.To2d(); }
        }
        [XmlElement("end_point", Order = 2)]
        public Geometry.Point2d _endPoint; // point_type_2d
        [XmlIgnore]
        public Geometry.Point3d EndPoint
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
                this._id = RestrictedInteger.ValueInRange(value, 1, 321272406);
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
        /// Convert alphabet to index. A, AA, BA, ZZA
        /// </summary>
        /// <param name="letters"></param>
        /// <returns></returns>
        private int alphabetNumbering(string letters)
        {
            int index = 0;

            foreach (char c in letters)
            {
                if (char.IsLetter(c))
                {
                    index = index * 26 + (char.ToUpper(c) - 'A' + 1);
                }
            }
            return index;

        }

        /// <summary>
        /// Internal constructor.
        /// </summary>
        /// <param name="startPoint">Start point of axis.</param>
        /// <param name="endPoint">End point of axis.</param>
        /// <param name="prefix">Prefix of axis identifier.</param>
        /// <param name="id">Number of axis identifier.</param>
        /// <param name="idIsLetter">Is identifier numbering a letter?</param>
        public Axis(Geometry.Point3d startPoint, Geometry.Point3d endPoint, string prefix, int id, bool idIsLetter)
        {
            this.EntityCreated();
            this.StartPoint = startPoint;
            this.EndPoint = endPoint;
            this.Id = id;
            this.IdIsLetter = idIsLetter;
            this.Prefix = prefix;
        }

        public Axis(Geometry.Point3d startPoint, Geometry.Point3d endPoint, int number, string prefix = "")
        {
            this.EntityCreated();
            this.StartPoint = startPoint;
            this.EndPoint = endPoint;
            this.Id = number;
            this.IdIsLetter = false;
            this.Prefix = prefix;
        }

        public Axis(Geometry.Point3d startPoint, Geometry.Point3d endPoint, string letter, string prefix = "")
        {
            this.EntityCreated();
            this.StartPoint = startPoint;
            this.EndPoint = endPoint;
            this.Id = alphabetNumbering(letter);
            this.IdIsLetter = true;
            this.Prefix = prefix;
        }

    }
}