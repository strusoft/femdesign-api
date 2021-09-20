using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using FemDesign.GenericClasses;

namespace FemDesign.Shells
{
    public enum PanelType
    {
        [XmlEnum("concrete")]
        Concrete,
        
        [XmlEnum("timber")]
        Timber,
    }
}
