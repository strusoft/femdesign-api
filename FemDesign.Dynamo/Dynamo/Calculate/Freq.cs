using System;
using System.Xml.Serialization;
#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Calculate
{
    [IsVisibleInDynamoLibrary(false)]
    public partial class Freq
    {
        #region dynamo
        /// <summary>
        /// Define calculation parameters for an eigenfrequency calculation.
        /// </summary>
        /// <param name="numShapes">Number of shapes.</param>
        /// <param name="maxSturm">Max number of Sturm check steps (checking missing eigenvalues).</param>
        /// <param name="x">Consider masses in global x-direction.</param>
        /// <param name="y">Consider masses in global y-direction.</param>
        /// <param name="z">Consider masses in global z-direction.</param>
        /// <param name="top">Top of substructure. Masses on this level and below are not considered in Eigenfrequency calculation.</param>
        [IsVisibleInDynamoLibrary(true)]
        public static Freq Define(int numShapes = 2, int maxSturm = 0, bool x = true, bool y = true, bool z = true, double top = -0.01)
        {
            return new Freq(numShapes, maxSturm, x, y, z, top);
        }
        #endregion
    }
}