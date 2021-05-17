using System.Xml.Serialization;

#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.ModellingTools
{
    [IsVisibleInDynamoLibrary(false)]
    public partial class StiffnessMatrix2Type
    {
        #region dynamo
        
        /// <summary>
        /// Define a shear stiffness matrix H
        /// </summary>
        /// <param name="xz">XZ component in kN/m</param>
        /// <param name="yz">YZ component in kN/m</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        public static StiffnessMatrix2Type Define(double xz = 10000, double yz = 10000)
        {
            return new StiffnessMatrix2Type(xz, yz);
        }

        #endregion
    }
}