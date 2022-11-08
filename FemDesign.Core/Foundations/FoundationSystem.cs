using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FemDesign.Foundations
{
    public enum FoundationSystem
    {

        [System.Xml.Serialization.XmlEnum(Name = "simple")]
        Simple,

        [System.Xml.Serialization.XmlEnum(Name = "surface_support_group")]
        SurfaceSupportGroup,

        [System.Xml.Serialization.XmlEnum(Name = "from_soil")]
        FromSoil,
    }
}
