
using System.Globalization;
using System.Xml.Serialization;
#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Materials
{
    [IsVisibleInDynamoLibrary(false)]
    public partial class Material: EntityBase
    {
        /// <summary>
        /// Get Material from MaterialDatabase by name.
        /// </summary>
        /// <remarks>Action</remarks>
        /// <param name="materialDatabase">MaterialDatabase</param>
        /// <param name="materialName">Name of Material</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        public static Material GetMaterialByName(MaterialDatabase materialDatabase, string materialName)
        {
            return materialDatabase.MaterialByName(materialName);
        }

        /// <summary>
        /// Set creep and shrinkage parameters to a concrete Material.
        /// </summary>
        /// <param name="material">Material.</param>
        /// <param name="creepUls">Creep ULS.</param>
        /// <param name="creepSlq">Creep SLS Quasi-Permanent</param>
        /// <param name="creepSlf">Creep SLS Frequent</param>
        /// <param name="creepSlc">Creep SLS Characteristic</param>
        /// <param name="shrinkage">Shrinkage.</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        public static Material SetConcreteMaterialProperties(Material material, double creepUls = 0, double creepSlq = 0, double creepSlf = 0, double creepSlc = 0, double shrinkage = 0)
        {
            return ConcreteMaterialProperties(material, creepUls, creepSlq, creepSlf, creepSlc, shrinkage);
        }
    }
}