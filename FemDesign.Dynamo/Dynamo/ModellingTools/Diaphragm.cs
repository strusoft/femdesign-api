using System.Xml.Serialization;

#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.ModellingTools
{
    [IsVisibleInDynamoLibrary(false)]
    public partial class Diaphragm
    {
        [IsVisibleInDynamoLibrary(true)]
        public static Diaphragm Define(Autodesk.DesignScript.Geometry.Surface surface, string identifier = "D")
        {
            return new Diaphragm(Geometry.Region.FromDynamo(surface), identifier);
        }
    }
}
