// https://strusoft.com/
using System;
using System.Xml.Serialization;
using System.Linq;


namespace FemDesign
{
    /// <summary>
    /// guid_list_type
    /// </summary>
    [Serializable]
    public partial class GuidListType
    {
        [XmlAttribute("guid")]
        public Guid Guid { get; set; }

        /// <summary>
        /// Parameterless constructor.
        /// </summary>
        private GuidListType()
        {
            
        }
        public GuidListType(Guid guid)
        {
            this.Guid = guid;
        }


        /// <summary>
        /// Implicit conversion of a Entity to its Global Unique Identifier.
        /// </summary>
        /// <param name="entity"></param>
        public static implicit operator GuidListType(EntityBase entity) => new GuidListType(entity.Guid);
    }

    /// <summary>
    /// two_guid_list_type
    /// </summary>
    [Serializable]
    public partial class TwoGuidListType
    {
        [XmlAttribute("first")]
        public Guid First { get; set; }

        [XmlAttribute("second")]
        public Guid Second { get; set; }

        /// <summary>
        /// Parameterless constructor.
        /// </summary>
        private TwoGuidListType()
        {
            
        }
        public TwoGuidListType(Guid first, Guid second)
        {
            this.First = first;
            this.Second = second;
        }
    }

    /// <summary>
    /// three_guid_list_type
    /// </summary>
    [Serializable]
    public partial class ThreeGuidListType
    {
        [XmlAttribute("first")]
        public Guid First { get; set; }

        [XmlAttribute("second")]
        public Guid Second { get; set; }

        [XmlAttribute("third")]
        public Guid Third { get; set; }

        /// <summary>
        /// Parameterless constructor.
        /// </summary>
        private ThreeGuidListType()
        {
            
        }
        public ThreeGuidListType(Guid first, Guid second, Guid third)
        {
            this.First = first;
            this.Second = second;
            this.Third = third;
        }
    }
}