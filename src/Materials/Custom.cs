// https://strusoft.com/

#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Materials
{
    /// <summary>
    /// material_type --> custom
    /// </summary>
    [System.Serializable]
    [IsVisibleInDynamoLibrary(false)]
    public class Custom: MaterialBase
    {
    }
}