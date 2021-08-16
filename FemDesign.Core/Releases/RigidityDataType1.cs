// https://strusoft.com/

using System.Xml.Serialization;


namespace FemDesign.Releases
{
    /// <summary>
    /// rigidity_data_type1
    /// </summary>
    [System.Serializable]
    public partial class RigidityDataType1
    {
        [XmlElement("motions", Order=1)]
        public Releases.Motions Motions { get; set; }
        [XmlElement("plastic_limit_forces", Order=2)]
        public Releases.MotionsPlasticLimits PlasticLimitForces { get; set; }
        [XmlAttribute("detach")]
        public DetachType Detach { get; set; }

        /// <summary>
        /// Parameterless constructor for serialization
        /// </summary>
        public RigidityDataType1()
        {
            
        }

        /// <summary>
        /// Construct RigidityDataType1 with motions only
        /// </summary>
        public RigidityDataType1(Motions motions)
        {
            this.Motions = motions;
        }

        /// <summary>
        /// Construct RigidityDataType1 with motions and plastic limits forces only
        /// </summary>
        public RigidityDataType1(Motions motions, MotionsPlasticLimits motionsPlasticLimits)
        {
            this.Motions = motions;
            this.PlasticLimitForces = motionsPlasticLimits;
        }
    }
}