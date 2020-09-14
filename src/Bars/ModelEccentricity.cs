// https://strusoft.com/
using System.Collections.Generic;
using System.Xml.Serialization;


namespace FemDesign.Bars
{
    /// <summary>
    /// eccentricity_type
    /// 
    /// This class is called ModelEccentricity as Eccentricity (ecc_value_type) is the Dynamo facing class and thus need to be named accordingly.
    /// </summary>
    [System.Serializable]
    public class ModelEccentricity
    {
        // attributes
        [XmlAttribute("use_default_physical_alignment")]
        public bool UseDefaultPhysicalAlignment { get; set; } // bool

        // elements
        [XmlElement("analytical")]
        public List<Eccentricity> Analytical = new List<Eccentricity>(); // ecc_value_type
        [XmlElement("physical")]
        public List<Eccentricity> Physical = new List<Eccentricity>(); // ecc_value_type
        
        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private ModelEccentricity()
        {
            
        }

        /// <summary>
        /// Construct ModelEccentricity with presets from Eccentricity.
        /// </summary>
        public ModelEccentricity(Eccentricity eccentricity)
        {
            this.UseDefaultPhysicalAlignment = true;
            List<Eccentricity> eccentricities = new List<Eccentricity>{eccentricity, eccentricity};
            this.Analytical = eccentricities;
            this.Physical = eccentricities;
        }
    }
}