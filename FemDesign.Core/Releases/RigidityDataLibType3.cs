// https://strusoft.com/

using System.Xml.Serialization;


namespace FemDesign.Releases
{
    [System.Serializable]
    public partial class RigidityDataLibType3: LibraryBase
    {
        // choice rigidity_data_type1
        [XmlElement("rigidity", Order = 1)]
        public Releases.RigidityDataType3 Rigidity { get; set; }

        // choice rigidity_group_typ1
        // [XmlElement("rigidity_group", Order = 2)]  
    }
}