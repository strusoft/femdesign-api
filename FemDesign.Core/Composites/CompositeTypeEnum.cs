// https://strusoft.com/

using System.Xml.Serialization;

namespace FemDesign.Composites
{
    [System.Serializable]
    public enum CompositeSectionType
    {
        [XmlEnum("beam_a")]
        IProfileWithEffectiveConcreteSlab,

        [XmlEnum("beam_b")]
        FilledHSQProfile,
        
        [XmlEnum("beam_p")]
        FilledDeltaBeamProfile,
        
        [XmlEnum("column_a")]
        FilledIProfile,
        
        [XmlEnum("column_c")]
        FilledCruciformProfile,
        
        [XmlEnum("column_d")]
        FilledRHSProfile,
        
        [XmlEnum("column_e")]
        FilledSteelTube,
        
        [XmlEnum("column_f")]
        FilledSteelTubeWithIProfile,
        
        [XmlEnum("column_g")]
        FilledSteelTubeWithSteelCore,
    }
}
