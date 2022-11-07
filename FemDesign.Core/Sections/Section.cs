// https://strusoft.com/

using System;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace FemDesign.Sections
{
    /// <summary>
    /// Section.
    /// </summary>
    [System.Serializable]
    public partial class Section: EntityBase
    {
        [XmlElement("region_group", Order = 1)]
        public Geometry.RegionGroup _regionGroup;

        [XmlIgnore]
        public Geometry.RegionGroup RegionGroup
        {
            set
            {
                
                Geometry.Vector3d unitZ = Geometry.Vector3d.UnitZ;
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
        public string Name { get; set; } // string i.e. GroupName, TypeName, SizeName --> "Steel sections, CHS, 20-2.0"

        [XmlAttribute("type")]
        public string Type { get; set; } // sectiontype

        [XmlAttribute("fd-mat")]
        public string MaterialType { get; set; } // int i.e. 1, 2, 3

        [XmlAttribute("fd_name_code")]
        public string GroupName { get; set; } // string. Optional i.e. Steel section, Concrete section

        [XmlAttribute("fd_name_type")]
        public string TypeName { get; set; } // string. Optional i.e. CHS, HE-A

        [XmlAttribute("fd_name_size")]
        public string SizeName { get; set; } // string. Optional

        [XmlIgnore]
        internal string _sectionName
        {
            get
            {
                return string.Join("-", new List<string> { this.TypeName + this.SizeName });
            }
        }

        /// <summary>
        /// Parameterless constructor for serialization
        /// </summary>
        private Section()
        {

        }

        /// <summary>
        /// Construct a new section
        /// </summary>
        [Obsolete("Consider use the other constructor. 'Name' shouldn't be set.")]
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
        /// Construct a new section
        /// </summary>
        public Section(Geometry.RegionGroup regionGroup, string type, Materials.MaterialTypeEnum materialTypeEnum, string groupName, string typeName, string sizeName)
        {
            this.EntityCreated();
            this.RegionGroup = regionGroup;
            this.Name = $"{groupName}, {typeName}, {sizeName}";
            this.Type = type;
            this.MaterialType = ((int)materialTypeEnum).ToString();
            this.GroupName = groupName;
            this.TypeName = typeName;
            this.SizeName = sizeName;
            this._end = "";
        }


        [XmlIgnore]
        public string MaterialFamily
        {
            get
            {
                string materialFamily = this.GroupName.Split(' ')[0];
                if (materialFamily == "Steel")
                    return "Steel";
                else if (materialFamily == "Concrete")
                    return "Concrete";
                else if (materialFamily == "Timber")
                    return "Timber";
                else if (materialFamily == "Hollow")
                    return "Hollow";
                else
                    return "Custom";
            }
        }

        public override string ToString()
        {
            return $"{this.Name}";
        }

    }
}