using System.Collections.Generic;
using System.Xml.Serialization;


namespace FemDesign.Loads
{
    [System.Serializable]
    public partial class LineTemperatureLoad: LoadBase
    {
        /// <summary>
        /// Edge defining the geometry of the load
        /// </summary>
        [XmlElement("edge", Order=1)]
        public Geometry.Edge Edge { get; set; }

        /// <summary>
        /// Direction of load.
        /// </summary>
        [XmlElement("direction", Order=2)]
        public Geometry.Vector3d Direction { get; set; }

        /// <summary>
        /// Optional. Ambiguous what this does.
        /// </summary>
        /// <value></value>

        [XmlElement("normal", Order=3)]
        public Geometry.Vector3d Normal { get; set; }

        /// <summary>
        /// Field
        /// </summary>
        [XmlElement("temperature", Order=4)]
        public List<TopBotLocationValue> _topBotLocVal;

        
        /// <summary>
        /// Top bottom value can be a list of 1 or 2 items. 1 item defines a uniform line load, 2 items defines a variable line load.
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
        private LineTemperatureLoad()
        {

        }

        /// <summary>
        /// Construct a uniform or variable line temperature load
        /// </summary>
        /// <param name="edge">Underlying edge of line load. Line or Arc.</param>
        /// <param name="direction">Directio of load.</param>
        /// <param name="topBotLocVals">1 or 2 top bottom location values</param>
        /// <param name="loadCase"></param>
        /// <param name="comment"></param>
        public LineTemperatureLoad(Geometry.Edge edge, Geometry.Vector3d direction, List<TopBotLocationValue> topBotLocVals, LoadCase loadCase, string comment)
        {
            this.EntityCreated();
            this.Edge = edge;
            this.Direction = direction;
            this.TopBotLocVal = topBotLocVals;
            this.LoadCaseGuid = loadCase.Guid;
            this.Comment = comment;
        }

    }
}