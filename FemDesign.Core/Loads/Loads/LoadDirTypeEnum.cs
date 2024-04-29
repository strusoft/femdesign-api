using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FemDesign.Loads
{
    public enum LoadDirType
    {
        [XmlEnum("constant")]
        Constant, 
        [XmlEnum("changing")]
        Changing
    }
}
