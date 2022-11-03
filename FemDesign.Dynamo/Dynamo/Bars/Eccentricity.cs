
using System.Xml.Serialization;

#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Bars
{
    [IsVisibleInDynamoLibrary(false)]
    public partial class Eccentricity
    {
        /// <summary>
        /// Define the eccentricity of bar-element along its local axes.
        /// Sign convention of values as defined in FEM-Design GUI. Note that the value defined here will be negated in the generated .struxml file based on the data-protocol.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="y">Eccentricity local-y.</param>
        /// <param name="z">Eccentricity local-z.</param>
        [IsVisibleInDynamoLibrary(true)]
        public static Eccentricity Define(double y = 0, double z = 0)
        {
            return new Eccentricity(y, z);
        }
    }
}