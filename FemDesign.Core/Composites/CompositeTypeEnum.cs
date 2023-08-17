// https://strusoft.com/

using System.Xml.Serialization;

namespace FemDesign.Composites
{
    [System.Serializable]
    public enum CompositeType
    {
        [XmlEnum("beam_a")]
        BeamA,

        [XmlEnum("beam_b")]
        BeamB,
        
        [XmlEnum("beam_p")]
        BeamP,
        
        [XmlEnum("column_a")]
        ColumnA,
        
        [XmlEnum("column_c")]
        ColumnC,
        
        [XmlEnum("column_d")]
        ColumnD,
        
        [XmlEnum("column_e")]
        ColumnE,
        
        [XmlEnum("column_f")]
        ColumnF,
        
        [XmlEnum("column_g")]
        ColumnG,
    }
}
