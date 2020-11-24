// https://strusoft.com/
using System.Collections.Generic;
using System.Xml.Serialization;

#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Bars
{
    /// <summary>
    /// eccentricity_type
    /// 
    /// This class is called ModelEccentricity as Eccentricity (ecc_value_type) is the Dynamo facing class and thus need to be named accordingly.
    /// </summary>
    [System.Serializable]
    [IsVisibleInDynamoLibrary(false)]
    public class ModelEccentricity
    {
        // attributes
        [XmlAttribute("use_default_physical_alignment")]
        public bool UseDefaultPhysicalAlignment { get; set; } // bool

        // elements
        [XmlElement("analytical", Order = 1)]
        public Eccentricity[] _analytical = new Eccentricity[2]; // ecc_value_type

        [XmlIgnore]
        public Eccentricity StartAnalytical
        {
            get
            {
                return this._analytical[0];
            }
            set
            {
                this._analytical[0] = value;
            }
        }

        [XmlIgnore]
        public Eccentricity EndAnalytical
        {
            get
            {
                return this._analytical[1];
            }
            set
            {
                this._analytical[1] = value;
            }
        }

        [XmlElement("physical", Order = 2)]
        public Eccentricity[] _physical = new Eccentricity[2]; // ecc_value_type

        [XmlIgnore]
        public Eccentricity StartPhysical
        {
            get
            {
                return this._physical[0];
            }
            set
            {
                this._physical[0] = value;
            }
        }

        [XmlIgnore]
        public Eccentricity EndPhysical
        {
            get
            {
                return this._physical[1];
            }
            set
            {
                this._physical[1] = value;
            }
        }
        
        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private ModelEccentricity()
        {
            
        }

        /// <summary>
        /// Construct uniform ModelEccentricity with presets from Eccentricity.
        /// </summary>
        public ModelEccentricity(Eccentricity eccentricity)
        {
            this.UseDefaultPhysicalAlignment = true;
            Eccentricity[] eccentricities = new Eccentricity[2]{eccentricity, eccentricity};
            this._analytical = eccentricities;
            this._physical = eccentricities;
        }

        /// <summary>
        /// Construct non-uniform ModelEccentricity with presets from Eccentricity.
        /// </summary>
        public ModelEccentricity(Eccentricity startEccentricity, Eccentricity endEccentricity, bool useDefaultPhysicalAlignment)
        {
            this.UseDefaultPhysicalAlignment = useDefaultPhysicalAlignment;
            this._analytical[0] = startEccentricity;
            this._analytical[1] = endEccentricity;
            this._physical[0] = startEccentricity;
            this._physical[1] = endEccentricity;
        }
    }
}