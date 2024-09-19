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
        [Parseable("beam", "Beam", "BEAM")]
        [XmlEnum("beam")]
        Beam,

        [Parseable("column", "Column", "COLUMN")]
        [XmlEnum("column")]
        Column,

        [Parseable("truss", "Truss", "TRUSS")]
        [XmlEnum("truss")]
        Truss
    };
}
