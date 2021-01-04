// https://strusoft.com/

using System;
using System.Collections.Generic;
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
        [XmlElement("region_group", Order = 1)]
        public Geometry.RegionGroup _regionGroup;

        [XmlIgnore]
        public Geometry.RegionGroup RegionGroup
        {
            set
            {
                
                Geometry.FdVector3d unitZ = Geometry.FdVector3d.UnitZ();
                foreach(Geometry.Region region in value.Regions)
                {
                    // check normal
                    int par = region.LocalZ.Parallel(unitZ);
                    if (par == 1)
                    {
                        // pass
                    }
                    else if (par == -1)
                    {
                        region.LocalZ = unitZ;
                    }
                    else
                    {
                        throw new System.ArgumentException("Normal of region must be parallell with z-axis");
                    }

                    // check if z of any point is not 0
                    if (region.Contours[0].Edges[0].Points[0].Z != 0)
                    {
                        throw new System.ArgumentException("Region must lie in the XY-plane with z=0");
                    }
                }

                this._regionGroup = value;
            }
            get
            {
                return this._regionGroup;
            }
        }

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

        /// <summary>
        /// Construct a new section
        /// <summary>
        public Section(Geometry.RegionGroup regionGroup, string name, string type, Materials.MaterialTypeEnum materialTypeEnum, string groupName, string typeName, string sizeName)
        {
            this.EntityCreated();
            this.RegionGroup = regionGroup;
            this.Name = name;
            this.Type = type;
            this.MaterialType = ((int)materialTypeEnum).ToString();
            this.GroupName = groupName;
            this.TypeName = typeName;
            this.SizeName = sizeName;
            this._end = "";
        }

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

        #region dynamo
        /// <summary>
        /// Define a new custom section.
        /// </summary>
        /// <param name="surfaces">Item or list of surfaces of section. Surfaces must lie in the XY-plane at z=0.</param>
        /// <param name="name">Name of section</param>
        /// <param name="materialType">Material type. Choice: SteelRolled/SteelColdWorked/SteelWelded/Concrete/Timber</param>
        /// <param name="groupName">Name of section group</param>
        /// <param name="typeName">Name of section type</param>
        /// <param name="sizeName">Nem of section size</param>
        [IsVisibleInDynamoLibrary(true)]
        public static Section Define(List<Autodesk.DesignScript.Geometry.Surface> surfaces, string name, string materialType, string groupName, string typeName, string sizeName)
        {
            // check if string input is null
            if (name == null || materialType == null || groupName == null || typeName == null || sizeName == null)
            {
                throw new System.ArgumentException($"Some input is null. name: {name}, materialType: {materialType}, groupName: {groupName}, typeName: {typeName}, sizeName: {sizeName}");
            }

            // convert geometry
            List<Geometry.Region> regions = new List<Geometry.Region>();
            foreach (Autodesk.DesignScript.Geometry.Surface surface in surfaces)
            {
                regions.Add(Geometry.Region.FromDynamo(surface));
            }

            // create regions group
            Geometry.RegionGroup regionGroup = new Geometry.RegionGroup(regions);

            // get mat type
            FemDesign.Materials.MaterialTypeEnum matTypeEnum = (FemDesign.Materials.MaterialTypeEnum)Enum.Parse(typeof(FemDesign.Materials.MaterialTypeEnum), materialType);

            // create section
            Sections.Section section = new Sections.Section(regionGroup, name, "custom", matTypeEnum, groupName, typeName, sizeName);

            // return
            return section;
        }
        #endregion
    }
}