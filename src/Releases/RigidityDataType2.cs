// https://strusoft.com/

using System.Xml.Serialization;

#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Releases
{
    /// <summary>
    /// rigidity_data_type2
    /// </summary>
    [System.Serializable]
    [IsVisibleInDynamoLibrary(false)]
    public class RigidityDataType2: RigidityDataType1
    {
        [XmlElement("rotations", Order=3)]
        public Releases.Rotations rotations { get; set; }
        [XmlElement("plastic_limit_moments", Order=4)]
        public Releases.PlasticityType3d plasticLimitMoments { get; set; }
    }
}