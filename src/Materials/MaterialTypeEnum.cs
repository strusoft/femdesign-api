using System.Xml.Serialization;

namespace FemDesign.Materials
{
    public enum MaterialTypeEnum
    {
        [XmlEnum(Name = "0")]
        SteelRolled,
        [XmlEnum(Name = "1")]
        SteelColdWorked,
        [XmlEnum(Name = "2")]
        SteelWelded,
        [XmlEnum(Name = "3")]
        Concrete,
        [XmlEnum(Name = "4")]
        Timber,
        [XmlEnum(Name = "65535")]
        Unknown,
        [XmlEnum(Name = "-1")]
        Undefined
    }
}