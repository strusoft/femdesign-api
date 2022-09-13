using System.Xml.Serialization;
using System.Collections.Generic;


namespace FemDesign.Loads
{
    [System.Serializable]
    public partial class SurfaceTemperatureLoad: LoadBase
    {
        [XmlElement("region", Order=1)]
        public Geometry.Region Region { get; set; }

        [XmlIgnore]
        public Geometry.Vector3d LocalZ
        {
            get
            {
                return this.Region.LocalZ;
            }
            set
            {
                this.Region.LocalZ = value;
            }
        }

        [XmlElement("temperature", Order=2)]
        public List<TopBotLocationValue> _topBotLocVal;
        [XmlIgnore]
        public List<TopBotLocationValue> TopBotLocVal
        {
            get
            {
                return this._topBotLocVal;
            }
            set
            {
                if (value.Count == 1 || value.Count == 3)
                {
                    this._topBotLocVal = value;
                }
                else
                {
                    throw new System.ArgumentException($"Length of list is: {value.Count}, expected 1 or 3");
                }
            }
        }

        /// <summary>
        /// Parameterless constructor for serialization
        /// </summary>
        private SurfaceTemperatureLoad()
        {

        }

        /// <summary>
        /// Construct a surface temperature load by region and temperature location values (top/bottom)
        /// </summary>
        /// <param name="region">Region</param>
        /// <param name="direction">Direction of load</param>
        /// <param name="tempLocValue">List of top bottom location value. List should have 1 or 3 elements.></param>
        /// <param name="loadCase">LoadCase.</param>
        /// <param name="comment">Comment.</param>
        public SurfaceTemperatureLoad(Geometry.Region region, Geometry.Vector3d direction, List<TopBotLocationValue> tempLocValue, LoadCase loadCase, string comment)
        {
            this.EntityCreated();
            this.Region = region;
            this.LocalZ = direction;
            this.TopBotLocVal = tempLocValue;
            this.LoadCaseGuid = loadCase.Guid;
            this.Comment = comment;
        }
        
        /// <summary>
        /// Construct surface temperature load by region, top value and bottom value.
        /// </summary>
        /// <param name="region">Region</param>
        /// <param name="direction">Direction of load</param>
        /// <param name="topVal">Top value, temperature in celsius</param>
        /// <param name="bottomVal">Bottom value, temperature in celsius</param>
        /// <param name="loadCase">LoadCase.</param>
        /// <param name="comment">Comment.</param>
        public SurfaceTemperatureLoad(Geometry.Region region, Geometry.Vector3d direction, double topVal, double bottomVal, LoadCase loadCase, string comment)
        {
            this.EntityCreated();
            this.Region = region;
            this.LocalZ = direction;
            this.TopBotLocVal = new List<TopBotLocationValue>{new TopBotLocationValue(region.CoordinateSystem.Origin, topVal, bottomVal)};
            this.LoadCaseGuid = loadCase.Guid;
            this.Comment = comment;
        }

    }
}