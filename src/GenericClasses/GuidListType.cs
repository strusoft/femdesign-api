// https://strusoft.com/
using System.Xml.Serialization;

namespace FemDesign
{
    /// <summary>
    /// guid_list_type
    /// </summary>
    [System.Serializable]
    public class GuidListType
    {
        [XmlAttribute("guid")]
        public System.Guid Guid { get; set; }

        /// <summary>
        /// Parameterless constructor.
        /// </summary>
        private GuidListType()
        {
            
        }
        public GuidListType(System.Guid guid)
        {
            this.Guid = guid;
        }
    }
}