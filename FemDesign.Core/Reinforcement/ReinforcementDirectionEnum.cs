using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using FemDesign.GenericClasses;

namespace FemDesign.Reinforcement
{
    public enum ReinforcementDirection
    {
        [Parseable("x", "X")]
        [XmlEnum("x")]
        X,

        [Parseable("y", "Y")]
        [XmlEnum("y")]
        Y,
    }
}
