using System.Xml.Serialization;

#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign
{
    [IsVisibleInDynamoLibrary(false)]
    public partial class LocationValue: Geometry.Point3d
    {
        
    }
}