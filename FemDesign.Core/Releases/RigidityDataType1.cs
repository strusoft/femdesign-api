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
        public DetachType _deachType = DetachType.None;

        [XmlIgnore]
        public DetachType DetachType
        {
            get
            {
                return this._deachType;
            }
            set
            {
                this._deachType = value;
                if (value == DetachType.X_Compression)
                {
                    this.Motions.XNeg = 0.00;
                }
                else if (value == DetachType.X_Tension)
                {
                    this.Motions.XPos = 0.00;
                }
                else if (value == DetachType.Y_Compression)
                {
                    this.Motions.YNeg = 0.00;
                }
                else if (value == DetachType.Y_Tension)
                {
                    this.Motions.YPos = 0.00;
                }
                else if (value == DetachType.Z_Compression)
                {
                    this.Motions.ZNeg = 0.00;
                }
                else if (value == DetachType.Z_Tension)
                {
                    this.Motions.ZPos = 0.00;
                }

            }
        }

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
        public RigidityDataType1(Motions motions, MotionsPlasticLimits motionsPlasticLimits, DetachType detachType = DetachType.None)
        {
            this.Motions = motions;
            this.PlasticLimitForces = motionsPlasticLimits;
            this.DetachType = detachType;
        }
    }
}