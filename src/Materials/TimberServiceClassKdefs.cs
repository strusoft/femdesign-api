using System.Xml.Serialization;

#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Materials
{
    /// <summary>
    /// service_class_kdefs
    /// </summary>
    [System.Serializable]
    [IsVisibleInDynamoLibrary(false)]
    public class TimberServiceClassKdfes
    {
        [XmlAttribute("service_class_0")]
        public double ServiceClass0 {get; set;}

        [XmlAttribute("service_class_1")]
        public double ServiceClass1 {get; set;}

        [XmlAttribute("service_class_2")]
        public double ServiceClass2 {get; set;}
                       
    }
}

