// https://strusoft.com/

using System.Xml.Serialization;
#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Sections
{
    /// <summary>
    /// Section.
    /// </summary>
    [System.Serializable]
    [IsVisibleInDynamoLibrary(false)]
    public class Section: EntityBase
    {
        [IsVisibleInDynamoLibrary(true)]
        [XmlAttribute("name")]
        public string Name { get; set; } // string
        [XmlAttribute("type")]
        public string Type { get; set; } // sectiontype
        [XmlAttribute("fd-mat")]
        public string FdMat { get; set; } // fd_mat_type
        [XmlAttribute("fd_name_code")]
        public string FdNameCode { get; set; } // string. Optional
        [XmlAttribute("fd_name_type")]
        public string FdNameType { get; set; } // string. Optional
        [XmlAttribute("fd_name_size")]
        public string FdNameSize { get; set; } // string. Optional
        [XmlElement("region_group")]
        public Geometry.RegionGroup RegionGroup { get; set; } // region_group_type
        [XmlElement("end")]
        public string End { get; set; } // enpty_type

        /// <summary>
        /// Get a Section from a SectionDatabase by name.
        /// </summary>
        /// <remarks>Action</remarks>
        /// <param name="sectionDatabase">SectionDatabase.</param>
        /// <param name="sectionName">Name of Section.</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        public static Section GetSectionByName(SectionDatabase sectionDatabase, string sectionName)
        {
            foreach (Section section in sectionDatabase.Sections.Section)
            {
                if (section.Name == sectionName)
                {
                    return section;
                }
            }
            throw new System.ArgumentException("Section was not found. Incorrect section name or empty section database.");
        }
    }
}