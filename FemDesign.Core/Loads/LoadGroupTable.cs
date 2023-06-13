using System;
using System.Globalization;
using System.Collections.Generic;
using System.Xml.Serialization;
using FemDesign.GenericClasses;

namespace FemDesign.Loads
{
    /// <summary>
    /// load_group_table
    /// </summary>
    [System.Serializable]
    public partial class LoadGroupTable
    {
        [XmlAttribute("last_change")]
        public string _lastChange;
        [XmlIgnore]
        internal DateTime LastChange
        {
            get
            {
                return DateTime.Parse(this._lastChange);
            }
            set
            {
                this._lastChange = value.ToString("yyyy-MM-ddTHH:mm:ss.fff", CultureInfo.InvariantCulture);
            }
        }
        [XmlAttribute("action")]
        public string Action { get; set; }
        [XmlAttribute("simple_combination_method")]
        public LoadCombinationMethod SimpleCombinationMethod { get; set; } = LoadCombinationMethod.False;
        [XmlElement("group")]
        public List<ModelGeneralLoadGroup> GeneralLoadGroups { get; set; } = new List<ModelGeneralLoadGroup>(); // sequence: ModelGeneralLoadGroup

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private LoadGroupTable() { }

        /// <summary>
        /// Internal constructor. Used for GH components and Dynamo nodes.
        /// </summary>

        public LoadGroupTable(List<ModelGeneralLoadGroup> loadGroups, LoadCombinationMethod combinationMethod = LoadCombinationMethod.False)
        {
            EntityCreated();
            foreach (ModelGeneralLoadGroup loadGroup in loadGroups)
                AddGeneralLoadGroup(loadGroup);
            this.SimpleCombinationMethod = combinationMethod;
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
        /// </summary>
        public void EntityCreated()
        {
            LastChange = DateTime.UtcNow;
            Action = "added";
        }

        /// <summary>
        /// Invoke when an instance is modified.
        /// 
        /// Changes timestamp and action.
        /// </summary>
        public void EntityModified()
        {
            this.LastChange = DateTime.UtcNow;
            this.Action = "modified";
        }
    }

    [System.Serializable]
    public enum LoadCombinationMethod
    {
        [XmlEnum("true")]
        [Parseable("true")]
        True,
        [XmlEnum("false")]
        [Parseable("false")]
        False,
        [XmlEnum("EN 1990 6.4.3(6.10.a, b)")]
        [Parseable("EN 1990 6.4.3(6.10.a, b)")]
        EN_1990_643_610_ab,
        [XmlEnum("EN 1990 6.4.3(6.10)")]
        [Parseable("EN 1990 6.4.3(6.10)")]
        EN_1990_643_610,
        [XmlEnum("custom")]
        [Parseable("custom")]
        Custom
    }
}
