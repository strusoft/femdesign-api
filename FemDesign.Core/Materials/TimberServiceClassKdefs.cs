using System.Xml.Serialization;


namespace FemDesign.Materials
{
    /// <summary>
    /// service_class_kdefs
    /// </summary>
    [System.Serializable]
    public partial class TimberServiceClassKdfes
    {
        [XmlAttribute("service_class_0")]
        public double ServiceClass0 {get; set;}

        [XmlAttribute("service_class_1")]
        public double ServiceClass1 {get; set;}

        [XmlAttribute("service_class_2")]
        public double ServiceClass2 {get; set;}
                       
    }
}

