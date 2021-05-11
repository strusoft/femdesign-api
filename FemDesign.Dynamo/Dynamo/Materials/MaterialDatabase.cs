
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;
#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Materials
{
    [IsVisibleInDynamoLibrary(false)]
    public partial class MaterialDatabase
    {
        /// <summary>
        /// Lists the names of all Materials in MaterialDatabase.
        /// </summary>
        /// <returns>List of material names.</returns>
        [IsVisibleInDynamoLibrary(true)]
        public List<string> ListMaterialNames()
        {
            return MaterialNames();
        }

        /// <summary>
        /// Load a custom MaterialDatabase from a .struxml file.
        /// </summary>
        /// <param name="filePath">File path to .struxml file.</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        public static MaterialDatabase FromStruxml(string filePath)
        {
            return DeserializeStruxml(filePath);
        }

        /// <summary>
        /// Deserialize MaterialDatabase from embedded resource.
        /// </summary>
        private static MaterialDatabase DeserializeFromResource(string countryCode)
        {
            return DeserializeResource(countryCode);
        }

        /// <summary>
        /// Load the default MaterialDatabase for each respective country.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="countryCode">National annex of calculation code (D/DK/EST/FIN/GB/H/N/PL/RO/S/TR)</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        public static MaterialDatabase Default(string countryCode = "S")
        {
            return GetDefault(countryCode);
        }

        /// <summary>
        /// Serialize MaterialDatabase to file (.struxml).
        /// </summary>
        private void SerializeMaterialDatabase(string filepath)
        {
            Serialize(filepath);
        }
    }
}