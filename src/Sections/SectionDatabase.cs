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
    [System.Serializable]
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

        /// <summary>
        /// Add a section to this section database
        /// </summary>
        internal void AddNewSection(Section obj)
        {
            if (this.SectionInDatabase(obj))
            {
                throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.Guid} has already been added to SectionDatabase. Are you adding the same element twice?");
            }
            else
            {
                this.Sections.Section.Add(obj);
            }
        }

        /// <summary>
        /// Check if a section is in this section database
        /// </summary>
        internal bool SectionInDatabase(Section newSection)
        {
            foreach (Section section in this.Sections.Section)
            {
                if (section.Guid == newSection.Guid)
                {
                    return true;
                }
            }
            return false;
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

        /// <summary>
        /// Serialize section database to file
        /// </summary>
        internal void SerializeSectionDatabase(string filePath)
        {
            // check file extension
            if (Path.GetExtension(filePath) != ".struxml")
            {
                throw new System.ArgumentException("File extension must be .struxml! SectionDatabase.SerializeDatabase failed.");
            }

            XmlSerializer serializer = new XmlSerializer(typeof(SectionDatabase));
            using (TextWriter writer = new StreamWriter(filePath))
            {
                serializer.Serialize(writer, this);
            }
        }

        #region dynamo
        /// <summary>
        /// Add a section to a SectionDatabase.
        /// </summary>
        /// <param name="section">Add a section to the section database</param>
        [IsVisibleInDynamoLibrary(true)]
        public SectionDatabase AddSection(Section section)
        {
            // check if section is null
            if (section == null)
            {
                throw new System.ArgumentException("Section is null.");
            }

            // clone section db
            SectionDatabase obj = this.DeepClone();

            // add section
            obj.AddNewSection(section);

            // return
            return obj;
        }


        /// <summary>
        /// Save this SectionDatabase to .struxml.
        /// </summary>
        /// <param name="filePathStruxml">File path where to save the section database as .struxml</param>
        [IsVisibleInDynamoLibrary(true)]
        public void Save(string filePathStruxml)
        {
            // serialize to file
            this.SerializeSectionDatabase(filePathStruxml);
        }
        #endregion
    }
}