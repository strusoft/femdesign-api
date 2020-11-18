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
        [XmlElement("region_group", Order = 1)]
        public Geometry.RegionGroup RegionGroup { get; set; } // region_group_type

        [XmlElement("end", Order = 2)]
        public string _end { get; set; } // enpty_type

        [XmlAttribute("name")]
        public string Name { get; set; } // string

        [XmlAttribute("type")]
        public string Type { get; set; } // sectiontype

        [XmlAttribute("fd-mat")]
        public string MaterialType { get; set; }
        // public Materials.MaterialTypeEnum MaterialType { get; set; } // fd_mat_type

        [XmlAttribute("fd_name_code")]
        public string GroupName { get; set; } // string. Optional

        [XmlAttribute("fd_name_type")]
        public string TypeName { get; set; } // string. Optional

        [XmlAttribute("fd_name_size")]
        public string SizeName { get; set; } // string. Optional

        /// <summary>
        /// Parameterless constructor for serialization
        /// <summary>
        private Section()
        {

        }

        // /// <summary>
        // /// Construct a new section
        // /// <summary>
        // public Section(Geometry.RegionGroup regionGroup, string name, string type, Materials.MaterialTypeEnum materialTypeEnum, string groupName, string typeName, string sizeName)
        // {
        //     this.EntityCreated();
        //     this.RegionGroup = regionGroup;
        //     this.Name = name;
        //     this.Type = type;
        //     this.MaterialType = materialTypeEnum; 
        //     this.GroupName = groupName;
        //     this.TypeName = typeName;
        //     this.SizeName = sizeName;
        //     this._end = "";
        // }

        /// <summary>
        /// Get a Section from a SectionDatabase by name.
        /// </summary>
        /// <remarks>Action</remarks>
        /// <param name="sectionDatabase">SectionDatabase.</param>
        /// <param name="sectionName">Name of Section.</param>
        /// <returns></returns>
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