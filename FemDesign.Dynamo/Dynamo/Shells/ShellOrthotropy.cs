
#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Shells
{
    [IsVisibleInDynamoLibrary(false)]
    public partial class ShellOrthotropy
    {
        /// <summary>
        /// Create a definition for ShellOrthotropy.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="orthoAlfa">Alpha in degrees.</param>
        /// <param name="orthoRatio">E2/E1</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        public static ShellOrthotropy Create(double orthoAlfa = 0, double orthoRatio = 1)
        {
            return new ShellOrthotropy(orthoAlfa, orthoRatio);
        }
    }
}