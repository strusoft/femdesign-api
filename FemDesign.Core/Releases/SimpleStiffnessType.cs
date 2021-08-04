// https://strusoft.com/

using System.Xml.Serialization;

namespace FemDesign.Releases
{
    /// <summary>
    /// stiffness_type
    /// </summary>
    [System.Serializable]
    public partial class SimpleStiffnessType
    {
        [XmlElement("mov_x", Order = 1)]
        public StiffBaseType MovX { get; set; }

        [XmlElement("rot_x", Order = 2)]
        public StiffBaseType RotX { get; set; }

        [XmlElement("mov_y", Order = 3)]
        public StiffBaseType MovY { get; set; }

        [XmlElement("rot_y", Order = 4)]
        public StiffBaseType RotY { get; set; }

        [XmlElement("mov_z", Order = 5)]
        
        public StiffBaseType MovZ { get; set; }

        [XmlElement("rot_z", Order = 6)]
        public StiffBaseType RotZ { get; set; }

        [XmlAttribute("detach")]
        public DetachType Detach { get; set; }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        public SimpleStiffnessType()
        {

        }
    }
}