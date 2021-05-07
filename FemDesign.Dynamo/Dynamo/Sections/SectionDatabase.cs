
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
        // [IsVisibleInDynamoLibrary(true)]
     
        // [IsVisibleInDynamoLibrary(true)]

        // [IsVisibleInDynamoLibrary(true)]

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