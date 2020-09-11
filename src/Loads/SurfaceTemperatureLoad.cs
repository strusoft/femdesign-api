using System.Xml.Serialization;
using System.Collections.Generic;


namespace FemDesign.Loads
{
    [System.Serializable]
    public class SurfaceTemperatureLoad: LoadBase
    {
        [XmlElement("region", Order=1)]
        public Geometry.Region Region { get; set; }
        [XmlElement("temperature", Order=2)]
        public List<TopBotLocationValue> _tempLocationValue;
        [XmlIgnore]
        public List<TopBotLocationValue> TempLocationValue
        {
            get
            {
                return this._tempLocationValue;
            }
            set
            {
                if (value.Count == 1 || value.Count == 3)
                {
                    this._tempLocationValue = value;
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
        /// <param name="tempLocValue">List of top bottom location value. List should have 1 or 3 elements.></param>
        /// <param name="_loadCase">LoadCase.</param>
        /// <param name="_comment">Comment.</param>
        public SurfaceTemperatureLoad(Geometry.Region region, List<TopBotLocationValue> tempLocValue, LoadCase _loadCase, string _comment)
        {
            this.EntityCreated();
            this.Region = region;
            this.TempLocationValue = tempLocValue;
            this.loadCase = _loadCase.guid;
            this.comment = _comment;
        }
        
        /// <summary>
        /// Construct surface temperature load by region, top value and bottom value.
        /// </summary>
        /// <param name="region">Region</param>
        /// <param name="topVal">Top value, temperature in celsius</param>
        /// <param name="bottomVal">Bottom value, temperature in celsius</param>
        /// <param name="_loadCase">LoadCase.</param>
        /// <param name="_comment">Comment.</param>
        public SurfaceTemperatureLoad(Geometry.Region region, double topVal, double bottomVal, LoadCase _loadCase, string _comment)
        {
            this.EntityCreated();
            this.Region = region;
            this.TempLocationValue = new List<TopBotLocationValue>{new TopBotLocationValue(region.coordinateSystem.origin, topVal, bottomVal)};
            this.loadCase = _loadCase.guid;
            this.comment = _comment;
        }

    }
}