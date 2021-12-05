// https://strusoft.com/

using System.Xml.Serialization;
using System;
using System.Collections.Generic;

namespace FemDesign.Loads
{
    /// <summary>
    /// load_case (child of load_group_table)
    /// </summary>
    [System.Serializable]
    public partial class ModelGeneralLoadGroup: GenericClasses.IFemDesignEntity
    {
        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlAttribute("guid")]
        public System.Guid Guid { get; set; }
        [XmlAttribute("consider_in_gmax")]
        public bool ConsiderInGmax { get; set; } = true;
        [XmlElement("permanent")]
        public ModelLoadGroupPermanent ModelLoadGroupPermanent { get; set; }
        [XmlElement("temporary")]
        public ModelLoadGroupTemporary ModelLoadGroupTemporary { get; set; }  //Ändra när temporary group finns!!!

        /// <summary>
        /// parameterless constructor for serialization
        /// </summary>
        public ModelGeneralLoadGroup()
        {
            // parameterless constructor for serialization
        }

        /// <summary>
        /// Public constructor.
        /// </summary>

        public ModelGeneralLoadGroup(LoadGroup LoadGroup, string name)
        {
            EntityCreated();
            AddSpecificLoadGroup(LoadGroup);
            Name = name;
        }

        /// <summary>
        /// Invoke when an instance is created.
        /// 
        public void EntityCreated()
        {
            Guid = Guid.NewGuid();
        }

        /// <summary>
        /// Creates an instance of <see cref="ModelSpecificLoadGroup">ModelSpecificLoadGroup</see> and assigne it to the corresponding field
        /// </summary>
        /// <param name="loadGroup"><see cref="LoadGroup">Instance of LoadGroup</see></param>
        public void AddSpecificLoadGroup(LoadGroup loadGroup)
        {
            if (loadGroup.Type == ELoadGroupType.Permanent)
            {
                
                this.ModelLoadGroupPermanent = new ModelLoadGroupPermanent(loadGroup.SafetyFactorFavourable, loadGroup.SafetyFactorUnfavourable,
                                                                   loadGroup.SafetyFactorAccidentalFavourable, loadGroup.SafetyFactorAccidentalUnfavourable,
                                                                   loadGroup.LoadCases, loadGroup.LoadCaseRelation, loadGroup.Xi);
            }
            else if (loadGroup.Type == ELoadGroupType.Temporary)
            {
                this.ModelLoadGroupTemporary = new ModelLoadGroupTemporary(loadGroup.SafetyFactorUnfavourable, loadGroup.LoadCategory.Psi0,
                                                                   loadGroup.LoadCategory.Psi1, loadGroup.LoadCategory.Psi2, loadGroup.PotentiallyLeadingAction,
                                                                   loadGroup.LoadCases, loadGroup.LoadCaseRelation);
            }
            else
                throw new System.ArgumentException("Load group type not yet implemented");
        }
    }
}