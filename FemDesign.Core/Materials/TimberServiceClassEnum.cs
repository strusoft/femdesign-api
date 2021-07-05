using System.Xml.Serialization;


namespace FemDesign.Materials
{
    [System.Serializable]
    public enum TimberServiceClassEnum
    {
        /// <summary>
        /// Service class 1, enumeration 0
        /// </summary>
        [XmlEnum("0")]
        ServiceClass1 = 0,

        
        [XmlEnum("1")]
        /// <summary>
        /// Service class 2, enumeration 1
        /// </summary>
        ServiceClass2 = 1,

        
        [XmlEnum("2")]
        /// <summary>
        /// Service class 3, enumeration 2
        /// </summary>
        ServiceClass3 = 2                    
    }
}

