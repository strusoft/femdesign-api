// https://strusoft.com/

using System.Xml.Serialization;

#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Releases
{
    [System.Serializable]
    [IsVisibleInDynamoLibrary(false)]
    public class RigidityDataLibType3: LibraryBase
    {
        // choice rigidity_data_type1
        [XmlElement("rigidity", Order = 1)]
        public Releases.RigidityDataType3 Rigidity { get; set; }

        // choice rigidity_group_typ1
        // [XmlElement("rigidity_group", Order = 2)]  
    }
}