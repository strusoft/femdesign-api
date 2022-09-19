// https://strusoft.com/
using System;
using System.Xml.Serialization;
using FemDesign.GenericClasses;
using System.Collections.Generic;

namespace FemDesign.Loads
{
    /// <summary>
    /// load_base_attribs
    /// </summary>
    [System.Serializable]
    public partial class LoadGroupBase
    {
        [XmlIgnore]
        public string Name { get; set; }
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
                ModelLoadCase.Add(new ModelLoadCaseInGroup(loadCase.Guid, this));
                LoadCase.Add(loadCase);
            }
        }

        /// <summary>
        /// Find the corresponding LoadCase instance stored in the load group based on the guid of the modelLoadCaseInGroup instance
        /// </summary>
        /// <param name="modelLoadCaseInGroup">Model load case to find corresponding complete LoadCase instance of</param>
        /// <returns>The LoadCase that has the same guid</returns>
        public LoadCase GetCorrespondingCompleteLoadCase(ModelLoadCaseInGroup modelLoadCaseInGroup)
        {
            LoadCase correspodningLoadCase = LoadCase.Find(i => i.Guid == modelLoadCaseInGroup.Guid);
            return correspodningLoadCase;
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
    }
}
