using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FemDesign.GenericClasses
{
    public enum VerticalAlignment
    {
        [Parseable("top", "Top", "TOP")]
        [XmlEnum("top")]
        Top,
        
        [Parseable("center", "Center", "CENTER", "centre", "Centre", "CENTRE")]
        [XmlEnum("center")]
        Center,

        [Parseable("bottom", "Bottom", "BOTTOM")]
        [XmlEnum("bottom")]
        Bottom,
    }
}
