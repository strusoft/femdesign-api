using System.Collections.Generic;
using System.Xml.Serialization;


namespace FemDesign.StructureGrid
{
    /// <summary>
    /// Class to contain list in entities. For serialization purposes only.
    /// </summary>
    [System.Serializable]
    public class Axes
    {
        [XmlElement("axis", Order = 1)]
        public List<Axis> axis = new List<Axis>();
    }
}