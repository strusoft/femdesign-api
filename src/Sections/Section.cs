// https://strusoft.com/

using System.Xml.Serialization;

namespace FemDesign.Sections
{
    /// <summary>
    /// Section.
    /// </summary>
    [System.Serializable]
    public class Section: EntityBase
    {
        [XmlAttribute("name")]
        public string name { get; set; } // string
        [XmlAttribute("type")]
        public string type { get; set; } // sectiontype
        [XmlAttribute("fd-mat")]
        public string fdMat { get; set; } // fd_mat_type
        [XmlAttribute("fd_name_code")]
        public string fdNameCode { get; set; } // string. Optional
        [XmlAttribute("fd_name_type")]
        public string fdNameType { get; set; } // string. Optional
        [XmlAttribute("fd_name_size")]
        public string fdNameSize { get; set; } // string. Optional
        [XmlElement("region_group")]
        public Geometry.RegionGroup regionGroup { get; set; } // region_group_type
        [XmlElement("end")]
        public string end { get; set; } // enpty_type

        /// <summary>
        /// Get a Section from a SectionDatabase by name.
        /// </summary>
        /// <remarks>Action</remarks>
        /// <param name="sectionDatabase">SectionDatabase.</param>
        /// <param name="sectionName">Name of Section.</param>
        /// <returns></returns>
        public static Section GetSectionByName(SectionDatabase sectionDatabase, string sectionName)
        {
            foreach (Section section in sectionDatabase.sections.section)
            {
                if (section.name == sectionName)
                {
                    return section;
                }
            }
            return null;
        }
    }
}