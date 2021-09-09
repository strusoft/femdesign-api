// https://strusoft.com/

using System;
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
    }
}