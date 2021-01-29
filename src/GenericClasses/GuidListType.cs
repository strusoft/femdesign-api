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

    /// <summary>
    /// two_guid_list_type
    /// </summary>
    [System.Serializable]
    public class TwoGuidListType
    {
        [XmlAttribute("first")]
        public System.Guid First { get; set; }

        [XmlAttribute("second")]
        public System.Guid Second { get; set; }

        /// <summary>
        /// Parameterless constructor.
        /// </summary>
        private TwoGuidListType()
        {
            
        }
        public TwoGuidListType(System.Guid first, System.Guid second)
        {
            this.First = first;
            this.Second = second;
        }
    }

    /// <summary>
    /// three_guid_list_type
    /// </summary>
    [System.Serializable]
    public class ThreeGuidListType
    {
        [XmlAttribute("first")]
        public System.Guid First { get; set; }

        [XmlAttribute("second")]
        public System.Guid Second { get; set; }

        [XmlAttribute("third")]
        public System.Guid Third { get; set; }

        /// <summary>
        /// Parameterless constructor.
        /// </summary>
        private ThreeGuidListType()
        {
            
        }
        public ThreeGuidListType(System.Guid first, System.Guid second, System.Guid third)
        {
            this.First = first;
            this.Second = second;
            this.Third = third;
        }
    }
}