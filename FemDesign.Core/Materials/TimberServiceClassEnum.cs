using System.Xml.Serialization;


namespace FemDesign.Materials
{
    [System.Serializable]
    public enum TimberServiceClassEnum
    {
        /// <summary>
        /// Service class 1
        /// </summary>
        [XmlEnum("0")]
        ServiceClass1 = 0,

        /// <summary>
        /// Service class 2
        /// </summary>
        [XmlEnum("1")]
        ServiceClass2 = 1,
        
        /// <summary>
        /// Service class 3
        /// </summary>
        [XmlEnum("2")]
        ServiceClass3 = 2                    
    }
}

