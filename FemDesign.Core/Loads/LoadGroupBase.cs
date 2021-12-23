﻿// https://strusoft.com/
using System.Xml.Serialization;
using FemDesign.GenericClasses;
using System.Collections.Generic;

namespace FemDesign.Loads
{
    /// <summary>
    /// load_base_attribs
    /// </summary>
    [System.Serializable]
    public partial class LoadGroupBase : EntityBase
    {
        [XmlElement("load_case")]
        public List<ModelLoadCaseInGroup> ModelLoadCase = new List<ModelLoadCaseInGroup>();// sequence: ModelLoadCaseInGroup
        [XmlAttribute("relationship")]
        public ELoadGroupRelationship Relationship { get; set; }
        [XmlIgnore]
        public List<LoadCase> LoadCase = new List<LoadCase>(); // List of complete load cases

        /// <summary>
        /// Add LoadCase to group.
        /// </summary>
        public void AddLoadCase(LoadCase loadCase)
        {
            if (LoadCaseInLoadGroup(loadCase))
            {
                // pass
            }
            else
            {
                ModelLoadCase.Add(new ModelLoadCaseInGroup(loadCase.Guid));
                LoadCase.Add(loadCase);
            }
        }

        /// <summary>
        /// Check if LoadCase in LoadGroup.
        /// </summary>
        public bool LoadCaseInLoadGroup(LoadCase loadCase)
        {
            foreach (ModelLoadCaseInGroup elem in this.ModelLoadCase)
            {
                if (elem.Guid == loadCase.Guid)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Adds attributes for serialization
        /// </summary>
        public void EntityCreated()
        {
            //pass
        }
    }
}
