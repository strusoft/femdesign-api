using System;
using System.Globalization;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace FemDesign.Loads
{
    /// <summary>
    /// load_group_table
    /// </summary>
    [System.Serializable]
    public partial class LoadGroupTable: EntityBase
    {
        [XmlAttribute("simple_combination_method")]
        public bool SimpleCombinationMethod { get; set; } = false;
        [XmlElement("group")]
        public List<ModelGeneralLoadGroup> GeneralLoadGroups = new List<ModelGeneralLoadGroup>(); // sequence: ModelGeneralLoadGroup

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private LoadGroupTable() { }

        /// <summary>
        /// Internal constructor. Used for GH components and Dynamo nodes.
        /// </summary>
        
        public LoadGroupTable(List<ModelGeneralLoadGroup> loadGroups)
        {
            EntityCreated();
            foreach (ModelGeneralLoadGroup loadGroup in loadGroups)
                AddGeneralLoadGroup(loadGroup);
        }

        /// <summary>
        /// Add GeneralLoadGroup to LoadGroupTable.
        /// </summary>
        private void AddGeneralLoadGroup(ModelGeneralLoadGroup generalLoadGroup)
        {
            if (this.LoadGroupInLoadGroupTable(generalLoadGroup))
            {
                // pass
            }
            else
            {
                this.GeneralLoadGroups.Add(generalLoadGroup);
            }
        }

        /// <summary>
        /// Check if GeneralLoadGroup is in LoadGroupTable.
        /// </summary>
        private bool LoadGroupInLoadGroupTable(ModelGeneralLoadGroup generalLoadGroup)
        {
            foreach (ModelGeneralLoadGroup elem in this.GeneralLoadGroups)
            {
                if (elem.Guid == generalLoadGroup.Guid)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Invoke when an instance is created.
        /// 
        public void EntityCreated()
        {
            LastChange = DateTime.UtcNow;
            Action = "added";
        }
    }
}
