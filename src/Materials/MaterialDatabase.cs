// https://strusoft.com/

using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;
#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Materials
{
    /// <summary>
    /// Material database.
    /// </summary>
    [IsVisibleInDynamoLibrary(false)]
    [XmlRoot("database", Namespace="urn:strusoft")]
    public class MaterialDatabase
    {
        [XmlIgnore]
        public string filePath {get; set; }
        [XmlAttribute("struxml_version")]
        public string struxml_version { get; set; }
        [XmlAttribute("source_software")]
        public string source_software { get; set; }
        [XmlAttribute("start_time")]
        public string start_time { get; set; }
        [XmlAttribute("end_time")]
        public string end_time { get; set; }
        [XmlAttribute("guid")]
        public string guid { get; set; }
        [XmlAttribute("convertid")]
        public string convertid { get; set; }
        [XmlAttribute("standard")]
        public string standard { get; set; }
        [XmlAttribute("country")]
        public string country { get; set; }
        [XmlAttribute("xmlns")]
        public string xmlns { get; set; }
        [XmlElement("materials")]
        public Materials materials { get; set; } // materials
        [XmlElement("reinforcing_materials")]
        public Materials reinforcingMaterials { get; set; } // reinforcing_materials
        [XmlElement("end")]
        public string end { get; set;}

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private MaterialDatabase()
        {

        }

        /// <summary>
        /// Lists the names of all Materials in MaterialDatabase.
        /// </summary>
        /// <returns>List of material names.</returns>
        [IsVisibleInDynamoLibrary(true)]
        public List<string> ListMaterialNames()
        {
            // empty list
            List<string> list = new List<string>();

            // list material names
            if (this.materials != null)
            {
                foreach (Material material in this.materials.material)
                {
                    list.Add(material.name);
                }
            }
            if (this.reinforcingMaterials != null)
            {
                foreach (Material material in this.reinforcingMaterials.material)
                {
                    list.Add(material.name);
                } 
            }

            // return
            return list;
        }
        private static MaterialDatabase DeserializeFromFilePath(string filePath)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(MaterialDatabase));
            TextReader reader = new StreamReader(filePath);
            object obj = deserializer.Deserialize(reader);
            MaterialDatabase materialDatabase = (MaterialDatabase)obj;
            reader.Close();
            return materialDatabase;
        }
        /// <summary>
        /// Load a custom MaterialDatabase from a .struxml file.
        /// </summary>
        /// <param name="filePath">File path to .struxml file.</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        public static MaterialDatabase FromStruxml(string filePath)
        {
            MaterialDatabase materialDatabase = MaterialDatabase.DeserializeFromFilePath(filePath);
            materialDatabase.end = "";
            return materialDatabase;
        }

        /// <summary>
        /// Deserialize MaterialDatabase from embedded resource.
        /// </summary>
        private static MaterialDatabase DeserializeFromResource(string countryCode)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(MaterialDatabase));

            Assembly assembly = Assembly.GetExecutingAssembly();
            foreach (string resourceName in assembly.GetManifestResourceNames())
            {
                if (resourceName.EndsWith("materials_" + countryCode + ".struxml"))
                {
                    using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                    {
                        TextReader reader = new StreamReader(stream);
                        object obj = deserializer.Deserialize(reader);
                        MaterialDatabase materialDatabase = (MaterialDatabase)obj;
                        reader.Close();
                        return materialDatabase;
                    }
                }
            }
            throw new System.ArgumentException("Material library resource not in assembly! Was solution compiled without embedded resources?");
        }
        /// <summary>
        /// Load the default MaterialDatabase for each respective country.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="countryCode">National annex of calculation code ("D"/"DK"/"EST"/"FIN"/"GB"/"H"/"N"/"PL"/"RO"/"S"/"TR")</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        public static MaterialDatabase Default(string countryCode = "S")
        {
            string code = RestrictedString.EurocodeType(countryCode);
            MaterialDatabase materialDatabase = MaterialDatabase.DeserializeFromResource(code);
            materialDatabase.end = "";
            return materialDatabase;
        }

        /// <summary>
        /// Serialize MaterialDatabase to file (.struxml).
        /// </summary>
        private void SerializeMaterialDatabase(string filepath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(MaterialDatabase));
            using (TextWriter writer = new StreamWriter(filepath))
            {
                serializer.Serialize(writer, this);
            }
        }
    }
}