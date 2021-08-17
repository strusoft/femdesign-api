using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using FemDesign.GenericClasses;

namespace FemDesign.Reinforcement
{
    /// <summary>
    /// Wire profile type
    /// </summary>
    public enum WireProfileType
    {
        /// <summary>
        /// Smooth wire profile
        /// </summary>
        [Parseable("smooth", "Smooth", "SMOOTH")]
        [XmlEnum("smooth")]
        Smooth,

        /// <summary>
        /// Ribbed wire profile
        /// </summary>
        [Parseable("ribbed", "Ribbed", "RIBBED")]
        [XmlEnum("ribbed")]
        Ribbed,
    }
}
