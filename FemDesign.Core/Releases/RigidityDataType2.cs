// https://strusoft.com/

using System.Xml.Serialization;


namespace FemDesign.Releases
{
    /// <summary>
    /// rigidity_data_type2
    /// </summary>
    [System.Serializable]
    public partial class RigidityDataType2: RigidityDataType1
    {
        [XmlElement("rotations", Order=3)]
        public Releases.Rotations Rotations { get; set; }
        [XmlElement("plastic_limit_moments", Order=4)]
        public Releases.RotationsPlasticLimits PlasticLimitMoments { get; set; }

        public RigidityDataType2()
        {
            
        }

        public RigidityDataType2(Motions motions, Rotations rotations) : base(motions)
        {
            Rotations = rotations;
        }

        public RigidityDataType2(Motions motions, MotionsPlasticLimits motionsPlasticLimits, Rotations rotations, RotationsPlasticLimits rotationsPlasticLimits) : base(motions, motionsPlasticLimits)
        {
            Rotations = rotations;
            PlasticLimitMoments = rotationsPlasticLimits;
        }
    }
}