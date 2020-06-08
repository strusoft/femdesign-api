// https://strusoft.com/

#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Releases
{
    /// <summary>
    /// plasticity_type_3d
    /// </summary>
    [System.Serializable]
    [IsVisibleInDynamoLibrary(false)]
    public class PlasticityType3d: StiffnessType
    {
        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private PlasticityType3d()
        {

        }
    }
}