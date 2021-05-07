using System.Xml.Serialization;

#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.ModellingTools
{
    [IsVisibleInDynamoLibrary(false)]
    public partial class StiffnessMatrix4Type
    {
        #region dynamo
        
        /// <summary>
        /// Define a membrane (D) or flexural (K) stiffness matrix.
        /// </summary>
        /// <param name="xx">xx component in kN/m or kNm</param>
        /// <param name="xy">xy component in kN/m or kNm</param>
        /// <param name="yy">yy component in kN/m or kNm</param>
        /// <param name="gxy">gxy component in kN/m or kNm</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        public static StiffnessMatrix4Type Define(double xx = 10000, double xy = 5000, double yy = 10000, double gxy = 10000)
        {
            return new StiffnessMatrix4Type(xx, xy, yy, gxy);
        }

        #endregion
    }
}