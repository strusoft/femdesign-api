// https://strusoft.com/

using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;
#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Sections
{

    /// <summary>
    /// Section database.
    /// </summary>
    [IsVisibleInDynamoLibrary(false)]
    [XmlRoot("database", Namespace="urn:strusoft")]
    public class SectionDatabase
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
        [XmlAttribute("xmlns")]
        public string Xmlns { get; set; }
        [XmlElement("sections")]
        public DatabaseSections Sections { get; set; }
        [XmlElement("end")]
        public string End { get; set;}
        private SectionDatabase()
        {
            // parameterless constructor for serialization
        }
        /// <summary>
        /// List the names of all Sections in SectionDatabase.
        /// </summary>
        /// <remarks>Action</remarks>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        public List<string> ListSectionNames()
        {
            List<string> list = new List<string>();
            foreach (Section section in this.Sections.Section)
            {
                list.Add(section.Name);
            }
            return list;
        }
        private static SectionDatabase DeserializeFromFilePath(string filePath)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(SectionDatabase));
            TextReader reader = new StreamReader(filePath);
            object obj = deserializer.Deserialize(reader);
            SectionDatabase sectionDatabase = (SectionDatabase)obj;
            reader.Close();
            return sectionDatabase;
        }
        /// <summary>
        /// Load a custom SectionDatabase from a .struxml file.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="filePath">File path to .struxml file.</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        public static SectionDatabase FromStruxml(string filePath)
        {
            SectionDatabase sectionDatabase = SectionDatabase.DeserializeFromFilePath(filePath);
            sectionDatabase.End = "";
            return sectionDatabase;
        }

        /// <summary>
        /// Deserialize section database from embedded resource.
        /// </summary>
        private static SectionDatabase DeserializeFromResource()
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(SectionDatabase));
            
            Assembly assembly = Assembly.GetExecutingAssembly();
            foreach (string resourceName in assembly.GetManifestResourceNames())
            {
                if (resourceName.EndsWith("sections.struxml"))
                {
                    using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                    {
                        TextReader reader = new StreamReader(stream);
                        object obj = deserializer.Deserialize(reader);
                        SectionDatabase sectionDatabase = (SectionDatabase)obj;
                        reader.Close();
                        return sectionDatabase;
                    }
                }
            }
            throw new System.ArgumentException("Section library resource not in assembly! Was solution compiled without embedded resource?");
        }
        /// <summary>
        /// Load the default SectionDatabase.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        public static SectionDatabase Default()
        {
            SectionDatabase sectionDatabase = SectionDatabase.DeserializeFromResource();
            sectionDatabase.End = "";
            return sectionDatabase;
        }
        private void SerializeSectionDatabase(string filepath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(SectionDatabase));
            using (TextWriter writer = new StreamWriter(filepath))
            {
                serializer.Serialize(writer, this);
            }
        }
    }
}