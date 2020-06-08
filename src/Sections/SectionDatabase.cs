// https://strusoft.com/

using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;

namespace FemDesign.Sections
{

    /// <summary>
    /// Section database.
    /// </summary>
    [XmlRoot("database", Namespace="urn:strusoft")]
    public class SectionDatabase
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
        [XmlElement("sections")]
        public DatabaseSections sections { get; set; }
        [XmlElement("end")]
        public string end { get; set;}
        private SectionDatabase()
        {
            // parameterless constructor for serialization
        }
        /// <summary>
        /// List the names of all Sections in SectionDatabase.
        /// </summary>
        /// <remarks>Action</remarks>
        /// <returns></returns>
        public List<string> ListSectionNames()
        {
            List<string> list = new List<string>();
            foreach (Section section in this.sections.section)
            {
                list.Add(section.name);
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
        public static SectionDatabase FromStruxml(string filePath)
        {
            SectionDatabase sectionDatabase = SectionDatabase.DeserializeFromFilePath(filePath);
            sectionDatabase.end = "";
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
                throw new System.ArgumentException("Section library resource not in assembly! Was solution compiled without embedded resource?");
            }
            return null;
        }
        /// <summary>
        /// Load the default SectionDatabase.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <returns></returns>
        public static SectionDatabase Default()
        {
            SectionDatabase sectionDatabase = SectionDatabase.DeserializeFromResource();
            sectionDatabase.end = "";
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