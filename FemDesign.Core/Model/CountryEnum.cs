using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using FemDesign.GenericClasses;

namespace FemDesign
{
    /// <summary>
    /// FEM-Design National Annex.
    /// </summary>
    public enum Country
    {
        /// <summary>
        /// Unspecified/Common annex
        /// </summary>
        [Parseable("", "common", "Common", "COMMON", "none", "None", "NONE")]
        [XmlEnum("common")]
        COMMON,

        /// <summary>
        /// German annex
        /// </summary>
        [Parseable("D", "d", "germany", "Germany")]
        [XmlEnum("D")]
        D,

        /// <summary>
        /// Danish annex
        /// </summary>
        [Parseable("DK", "dk", "denmark", "Denmark")]
        [XmlEnum("DK")]
        DK,

        /// <summary>
        /// Estonian annex
        /// </summary>
        [Parseable("EST", "est", "estonia", "Estonia")]
        [XmlEnum("EST")]
        EST, 
        /// <summary>
        /// Finnish annex
        /// </summary>
        [Parseable("FIN", "fin", "finland", "Finland")]
        [XmlEnum("FIN")]
        FIN,

        /// <summary>
        /// Brittish annex
        /// </summary>
        [Parseable("GB", "gb", "brittish", "Brittish", "great britain", "Great Britain")]
        [XmlEnum("GB")]
        GB,

        /// <summary>
        /// Hungarian annex
        /// </summary>
        [Parseable("H", "h", "hungary", "Hungary")]
        [XmlEnum("H")]
        H,

        /// <summary>
        /// Latvian annex
        /// </summary>
        [Parseable("LT", "lt", "lithuania", "Lithuania")]
        [XmlEnum("LT")]
        LT,

        /// <summary>
        /// Norwegian annex
        /// </summary>
        [Parseable("N", "n", "norway", "Norway")]
        [XmlEnum("N")]
        N,

        /// <summary>
        /// Dutch annex
        /// </summary>
        [Parseable("NL", "nl", "netherlands", "Netherlands", "dutch", "Dutch")]
        [XmlEnum("NL")]
        NL,

        /// <summary>
        /// Polish annex
        /// </summary>
        [Parseable("PL", "pl", "poland", "Poland")]
        [XmlEnum("PL")]
        PL,

        /// <summary>
        /// Romanian annex
        /// </summary>
        [Parseable("RO", "ro", "romania", "Romania")]
        [XmlEnum("RO")]
        RO, 
        /// <summary>
        /// Swedish annex
        /// </summary>
        [Parseable("S", "s", "SE", "sweden", "Sweden", "😎")]
        [XmlEnum("S")]
        S,

        /// <summary>
        /// Turkish annex
        /// </summary>
        [Parseable("TR", "tr", "turkey", "Turkey")]
        [XmlEnum("TR")]
        TR
    }
}
