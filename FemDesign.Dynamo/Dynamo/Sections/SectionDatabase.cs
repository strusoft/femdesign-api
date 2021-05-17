
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;
#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Sections
{

    [IsVisibleInDynamoLibrary(false)]
    public partial class SectionDatabase
    {
        /// <summary>
        /// List the names of all Sections in SectionDatabase.
        /// </summary>
        /// <remarks>Action</remarks>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        public List<string> ListSectionNames()
        {
            return SectionNames();
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
            return DeserializeStruxml(filePath);

        }

        /// <summary>
        /// Load the default SectionDatabase.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        public static SectionDatabase Default()
        {
            return GetDefault();
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