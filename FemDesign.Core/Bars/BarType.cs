// https://strusoft.com/
using FemDesign.GenericClasses;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace FemDesign.Bars
{
    /// <summary>
    /// BarType enum
    /// </summary>
    [System.Serializable]
    public enum BarType
    {
        [Parseable("Beam", "B", "beam", "BEAM")]
        [XmlEnum("beam")]
        Beam,

        [Parseable("Column", "C", "column", "COLUMN")]
        [XmlEnum("column")]
        Column,

        [Parseable("Truss", "T", "truss", "TRUSS")]
        [XmlEnum("truss")]
        Truss
    };
}
