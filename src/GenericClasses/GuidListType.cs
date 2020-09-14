// https://strusoft.com/
using System.Xml.Serialization;
#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign
{
    /// <summary>
    /// guid_list_type
    /// </summary>
    [System.Serializable]
    [IsVisibleInDynamoLibrary(false)]
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