using System.Xml.Serialization;

namespace FemDesign.Loads
{
    /// <summary>
    /// Used for keeping track of the relationsship of the load cases in a group
    /// </summary>
    [System.Serializable]
    public enum ELoadGroupRelationship
    {
        /// <summary> If all cases are to be applied together </summary>
        [XmlEnum("entire")]
        Entire,
        /// <summary> If all cases are to be applied mutually exclusive </summary>
        [XmlEnum("alternative")]
        Alternative,
        /// <summary> If all cases are to be applied simultaneously</summary>
        [XmlEnum("simultaneous")]
        Simultaneous,
        /// <summary> Custom combination pattern</summary>
        [XmlEnum("custom")]
        Custom,
    }
}
