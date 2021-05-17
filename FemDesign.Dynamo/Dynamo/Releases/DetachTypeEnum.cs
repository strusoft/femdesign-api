// https://strusoft.com/

using System.Xml.Serialization;
#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Releases
{
    [System.Serializable]
    [IsVisibleInDynamoLibrary(false)]
    public enum DetachType
    {
        [XmlEnum("x_tens")]
        x_tens,
        [XmlEnum("x_comp")]
        x_comp,
        [XmlEnum("y_tens")]
        y_tens,
        [XmlEnum("y_comp")]
        y_comp,
        [XmlEnum("z_tens")]
        z_tens,
        [XmlEnum("z_comp")]
        z_comp
    }
}
