using System;
using System.Xml.Serialization;
#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Calculate
{
    [IsVisibleInDynamoLibrary(false)]
    public partial class Footfall
    {
        #region dynamo

        /// <summary>
        /// Define calculation parameters for a footfall calculation.
        /// </summary>
        /// <param name="top">Top of substructure. Masses on this level and below are not considered in Footfall calculation.</param>
        [IsVisibleInDynamoLibrary(true)]
        public static Footfall Define(double top = -0.01)
        {
            return new Footfall(top);
        }

        #endregion
    }
}