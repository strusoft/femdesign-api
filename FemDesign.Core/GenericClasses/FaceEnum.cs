using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FemDesign.GenericClasses
{
    public enum Face
    {
        [Parseable("top", "Top", "TOP", "up", "Up", "UP")]
        [XmlEnum("top")]
        Top,
        
        [Parseable("bottom", "Bottom", "BOTTOM", "down", "Down", "DOWN")]
        [XmlEnum("bottom")]
        Bottom,
    }
}
