// https://strusoft.com/

using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace FemDesign.Loads
{
    /// <summary>
    /// line_stress_load_type
    /// </summary>
    [System.Serializable]
    public partial class LineStressLoad: LoadBase
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
        public Geometry.FdVector3d Direction { get; set; }

        /// <summary>
        /// Optional. Ambiguous what this does.
        /// </summary>
        /// <value></value>
        [XmlElement("normal", Order = 3)]
        public Geometry.FdVector3d Normal { get; set; }

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
        /// Construct a uniform or variable line stress load
        /// </summary>
        /// <param name="edge">Underlying edge of line load. Line or Arc.</param>
        /// <param name="direction">Direction of load.</param>
        /// <param name="topBotLocVal">List of 2 top bottom location values</param>
        public LineStressLoad(Geometry.Edge edge, Geometry.FdVector3d direction, List<TopBotLocationValue> topBotLocVals, LoadCase loadCase, string comment)
        {
            this.EntityCreated();
            this.Edge = edge;
            this.Direction = direction;
            this.TopBotLocVal = topBotLocVals;
            this.LoadCase = (Guid)loadCase.IndexedGuid;
            this.Comment = comment;
        }

    }
}