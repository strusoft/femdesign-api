using System.Collections.Generic;
using System.Xml.Serialization;


namespace FemDesign.StructureGrid
{
    /// <summary>
    /// Class to contain list in entities. For serialization purposes only.
    /// </summary>
    [System.Serializable]
    public partial class Axes
    {
        [XmlElement("axis", Order = 1)]
        public List<Axis> Axis = new List<Axis>();
    }
}