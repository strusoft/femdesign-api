using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using FemDesign.GenericClasses;

namespace FemDesign.Loads
{
    /// <summary>
    /// FEM-Design Load Case Duration.
    /// </summary>
    public enum LoadCaseDuration
    {

        /// <summary>
        /// "Permanent" load case duration
        /// </summary>
        [Parseable("permanent", "Permanent", "PERMANENT")]
        [XmlEnum("permanent")]
        Permanent,

        /// <summary>
        /// "Long-term" load case duration
        /// </summary>
        [Parseable("long-term", "Long-term", "LongTerm", "LONG_TERM")]
        [XmlEnum("long-term")]
        LongTerm,

        /// <summary>
        /// "Medium-term" load case duration
        /// </summary>
        [Parseable("medium-term", "Medium-term", "MediumTerm", "MEDIUM_TERM")]
        [XmlEnum("medium-term")]
        MediumTerm,

        /// <summary>
        /// "Short-term" load case duration
        /// </summary>
        [Parseable("short-term", "Short-term", "ShortTerm", "SHORT_TERM")]
        [XmlEnum("short-term")]
        ShortTerm,

        /// <summary>
        /// "Instantaneous" load case duration
        /// </summary>
        [Parseable("instantaneous", "Instantaneous", "INSTANTANEOUS")]
        [XmlEnum("instantaneous")]
        Instantaneous,
    }
}
