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
        PERMANENT,

        /// <summary>
        /// "Long-term" load case duration
        /// </summary>
        [Parseable("long-term", "Long-term", "LONG_TERM")]
        [XmlEnum("long-term")]
        LONG_TERM,

        /// <summary>
        /// "Medium-term" load case duration
        /// </summary>
        [Parseable("medium-term", "Medium-term", "MEDIUM_TERM")]
        [XmlEnum("medium-term")]
        MEDIUM_TERM,

        /// <summary>
        /// "Short-term" load case duration
        /// </summary>
        [Parseable("short-term", "Short-term", "SHORT_TERM")]
        [XmlEnum("short-term")]
        SHORT_TERM,

        /// <summary>
        /// "Instantaneous" load case duration
        /// </summary>
        [Parseable("instantaneous", "Instantaneous", "INSTANTANEOUS")]
        [XmlEnum("instantaneous")]
        INSTANTANEOUS,
    }
}
