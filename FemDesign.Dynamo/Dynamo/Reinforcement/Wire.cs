
using System.Xml.Serialization;
#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Reinforcement
{
    [IsVisibleInDynamoLibrary(false)]
    public partial class Wire
    {
        #region dynamo

        /// <summary>
        /// Define a reinforcement bar (wire) for a normal reinforcement layout.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="diameter">Diameter of reinforcement bar.</param>
        /// <param name="reinforcingMaterial">Material. Material of reinforcement bar.</param>
        /// <param name="profile">"smooth"/"ribbed"</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        public static Wire Define(double diameter, Materials.Material reinforcingMaterial, WireProfileType profile)
        {
            return new Wire(diameter, reinforcingMaterial, profile);
        }

        #endregion
    }
}