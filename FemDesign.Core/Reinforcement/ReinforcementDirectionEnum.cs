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
    /// Reinforcement direction of a reinforcement layout.
    /// </summary>
    public enum ReinforcementDirection
    {
        /// <summary>
        /// X-direction.
        /// </summary>
        [Parseable("x", "X")]
        [XmlEnum("x")]
        X,

        /// <summary>
        /// Y-direction
        /// </summary>
        [Parseable("y", "Y")]
        [XmlEnum("y")]
        Y,
    }
}
