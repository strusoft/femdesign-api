// https://strusoft.com/

using FemDesign.Geometry;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace FemDesign.Loads
{
    /// <summary>
    /// line_stress_load_type
    /// </summary>
    [System.Serializable]
    public partial class LineStressLoad : LoadBase
    {
        /// <summary>
        /// Edge defining the geometry of the load
        /// </summary>
        [XmlElement("edge", Order = 1)]
        public Geometry.Edge Edge { get; set; }

        /// <summary>
        /// Direction of load.
        /// </summary>
        [XmlElement("direction", Order = 2)]
        public Geometry.Vector3d Direction { get; set; }

        /// <summary>
        /// Optional. Ambiguous what this does.
        /// </summary>
        /// <value></value>
        [XmlElement("normal", Order = 3)]
        public Geometry.Vector3d Normal { get; set; }

        /// <summary>
        /// Top bottom field. Top value is normal force [kN], bottom value is moment [kNm], according to FD GUI.
        /// </summary>
        [XmlElement("stress", Order = 4)]
        public List<TopBotLocationValue> _topBotLocVal;

        /// <summary>
        /// Top bottom property. Top bottom value can be a list of 1 or 2 items. 1 item defines a uniform line load, 2 items defines a variable line load.
        /// Top bottom field. Top value is normal force [kN], bottom value is moment [kNm], according to FD GUI.
        /// </summary>
        [XmlIgnore]
        public List<TopBotLocationValue> TopBotLocVal
        {
            get
            {
                return this._topBotLocVal;
            }
            set
            {
                if (value.Count == 2)
                {
                    this._topBotLocVal = value;
                }
                else
                {
                    throw new System.ArgumentException($"Length of list is: {value.Count}, expected 2");
                }
            }
        }

        /// <summary>
        /// Parameterless constructor for serialization
        /// </summary>
        private LineStressLoad()
        {

        }

        /// <summary>
        /// Construct a line stress load
        /// </summary>
        /// <param name="edge"></param>
        /// <param name="force">Force (n1 and n2)</param>
        public LineStressLoad(Edge edge, double force, LoadCase loadCase, string comment = "")
                : this(edge, edge.CoordinateSystem.LocalY, force, force, 0.0, 0.0, loadCase, comment)
        { }

        /// <summary>
        /// Construct a line stress load
        /// </summary>
        /// <param name="edge">Underlying edge of line load. Line or Arc.</param>
        /// <param name="force">Force (n1 and n2)</param>
        /// <param name="moment">Moment (m1 and m2)</param>
        public LineStressLoad(Edge edge, double force, double moment, LoadCase loadCase, string comment = "")
                : this(edge, edge.CoordinateSystem.LocalY, force, force, moment, moment, loadCase, comment)
        { }

        /// <summary>
        /// Construct a uniform or variable line stress load
        /// </summary>
        /// <param name="edge">Underlying edge of line load. Line or Arc.</param>
        /// <param name="n1">Force at start.</param>
        /// <param name="n2">Force at end.</param>
        /// <param name="m1">Moment at start.</param>
        /// <param name="m2">Moment at end.</param>
        public LineStressLoad(Edge edge, double n1, double n2, double m1, double m2, LoadCase loadCase, string comment = "")
                : this(edge, edge.CoordinateSystem.LocalY, n1, n2, m1, m2, loadCase, comment)
        { }

        /// <summary>
        /// Construct a uniform or variable line stress load
        /// </summary>
        /// <param name="edge">Underlying edge of line load. Line or Arc.</param>
        /// <param name="direction">Direction of load.</param>
        /// <param name="n1">Force at start.</param>
        /// <param name="n2">Force at end.</param>
        /// <param name="m1">Moment at start.</param>
        /// <param name="m2">Moment at end.</param>
        public LineStressLoad(Edge edge, Vector3d direction, double n1, double n2, double m1, double m2, LoadCase loadCase, string comment = "")
        {
            var topBotLocVals = new List<TopBotLocationValue> {
                new TopBotLocationValue(edge.Points[0], n1, m1),
                new TopBotLocationValue(edge.Points[0], n2, m2)
            };
            Initialize(edge, direction, topBotLocVals, loadCase, comment);
        }

        /// <summary>
        /// Construct a uniform or variable line stress load
        /// </summary>
        /// <param name="edge">Underlying edge of line load. Line or Arc.</param>
        /// <param name="direction">Direction of load.</param>
        /// <param name="topBotLocVals">List of 2 top bottom location values</param>
        public LineStressLoad(Edge edge, Vector3d direction, List<TopBotLocationValue> topBotLocVals, LoadCase loadCase, string comment = "")
        {
            Initialize(edge, direction, topBotLocVals, loadCase, comment);
        }

        private void Initialize(Edge edge, Vector3d direction, List<TopBotLocationValue> topBotLocVals, LoadCase loadCase, string comment)
        {
            this.EntityCreated();
            this.Edge = edge;
            this.Direction = direction;
            this.TopBotLocVal = topBotLocVals;
            this.LoadCaseGuid = loadCase.Guid;
            this.LoadCaseName = loadCase.Name;
            this.Comment = comment;
        }

        public override string ToString()
        {
            return $"{this.GetType().Name}, Force: {TopBotLocVal[0].TopVal:0.0}/{TopBotLocVal[1].TopVal:0.0}kN, Moment: {TopBotLocVal[0].BottomVal:0.0}/{TopBotLocVal[1].BottomVal:0.0}kNm, LoadCase: {this.LoadCaseName}";
        }
    }
}