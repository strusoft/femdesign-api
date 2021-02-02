using System.Xml.Serialization;

#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Materials
{
    [System.Serializable]
    [IsVisibleInDynamoLibrary(false)]
    public enum MaterialTypeEnum
    {
        SteelRolled  = 0,
        SteelColdWorked = 1,
        SteelWelded = 2,
        Concrete = 3,
        Timber = 4,
        Unknown = 65535,
        Undefined = -1
    }
}