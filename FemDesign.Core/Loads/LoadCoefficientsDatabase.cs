// https://strusoft.com/

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;

namespace FemDesign.Loads
{
    /// <summary>
    /// Material database.
    /// </summary>
    [XmlRoot("database", Namespace="urn:strusoft")]
    public partial class LoadCoefficientsDatabase
    {
        [XmlIgnore]
        public string FilePath {get; set; }
        [XmlAttribute("struxml_version")]
        public string StruxmlVersion { get; set; }
        [XmlAttribute("source_software")]
        public string SourceSoftware { get; set; }
        [XmlAttribute("start_time")]
        public string StartTime { get; set; }
        [XmlAttribute("end_time")]
        public string EndTime { get; set; }
        [XmlAttribute("guid")]
        public string Guid { get; set; }
        [XmlAttribute("convertid")]
        public string ConvertId { get; set; }
        [XmlAttribute("standard")]
        public string Standard { get; set; }
        [XmlAttribute("country")]
        public string Country { get; set; }
        [XmlElement("load_types")]
        public LoadTypes LoadTypes { get; set;}
        [XmlElement("end")]
        public string End { get; set; }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private LoadCoefficientsDatabase()
        {

        }

        /// <summary>
        /// Lists the names of all Load Types in LoadCoefficientsDatabase.
        /// </summary>
        /// <returns>List of load type names.</returns>
        public List<string> LoadTypeNames()
        {
            // empty list
            List<string> list = new List<string>();

            // list material names
            if (this.LoadTypes != null)
            {
                foreach (LoadType loadType in this.LoadTypes.LoadType)
                {
                    list.Add(loadType.Name);
                }
            }
            // return
            return list;
        }

        /// <summary>
        /// Get load type from LoadCoefficientDatabase by name.
        /// </summary>
        /// <param name="loadTypeName">Name of load type</param>
        /// <returns></returns>
        public LoadType LoadTypeByName(string loadTypeName)
        {
            if (this.LoadTypes != null)
            {
                foreach (LoadType loadType in this.LoadTypes.LoadType)
                {
                    if (loadType.Name == loadTypeName)
                    {
                        // update object information
                        loadType.Guid = System.Guid.NewGuid();
                        loadType.EntityModified();

                        // return
                        return loadType;
                    }
                }
            }
            throw new System.ArgumentException($"Load type was not found. Incorrect material name ({loadTypeName}) or empty load coefficient database.");
        }

        /*
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
        public static MaterialDatabase DeserializeStruxml(string filePath)
        {
            MaterialDatabase materialDatabase = MaterialDatabase.DeserializeFromFilePath(filePath);
            materialDatabase.End = "";
            return materialDatabase;
        }
        */

        /// <summary>
        /// Deserialize LoadcoefficientDatabase from embedded resource.
        /// </summary>
        private static LoadCoefficientsDatabase DeserializeResource(string countryCode)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(LoadCoefficientsDatabase));

            Assembly assembly = Assembly.GetExecutingAssembly();
            foreach (string resourceName in assembly.GetManifestResourceNames())
            {
                if (resourceName.EndsWith("loadCoefficients_" + countryCode + ".struxml"))
                {
                    using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                    {
                        TextReader reader = new StreamReader(stream);
                        object obj = deserializer.Deserialize(reader);
                        LoadCoefficientsDatabase loadCoefficientDatabase = (LoadCoefficientsDatabase)obj;
                        reader.Close();
                        return loadCoefficientDatabase;
                    }
                }
            }
            throw new System.ArgumentException("Load coefficient library resource not in assembly! Was solution compiled without embedded resources?");
        }
        /// <summary>
        /// Load the default LoadCoefficientDatabase for each respective country.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="countryCode">National annex of calculation code (D/DK/EST/FIN/GB/H/N/PL/RO/S/TR)</param>
        /// <returns></returns>
        public static LoadCoefficientsDatabase GetDefault(string countryCode = "S")
        {
            string code = RestrictedString.EurocodeType(countryCode);
            LoadCoefficientsDatabase loadCoefficientDatabase = LoadCoefficientsDatabase.DeserializeResource(code);
            loadCoefficientDatabase.End = "";
            return loadCoefficientDatabase;
        }

        /// <summary>
        /// Serialize LoadCoefficientsDatabase to file (.struxml).
        /// </summary>
        private void Serialize(string filepath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(LoadCoefficientsDatabase));
            using (TextWriter writer = new StreamWriter(filepath))
            {
                serializer.Serialize(writer, this);
            }
        }
    }
}