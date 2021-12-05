// https://strusoft.com/

using System.Xml.Serialization;
using System.Collections.Generic;

namespace FemDesign.Loads
{
    /// <summary>
    /// permanent_load_group (child of general_load_group)
    /// </summary>
    [System.Serializable]
    public partial class ModelLoadGroupPermanent: GenericClasses.IFemDesignEntity
    {
        [XmlAttribute("standard_favourable")]
        public double StandardFavourable { get; set; }
        [XmlAttribute("standard_unfavourable")]
        public double StandardUnfavourable { get; set; }
        [XmlAttribute("xi")]
        public double _xi;
        [XmlIgnore]
        public double Xi
        {
            get
            {
                return this._xi;
            }
            set
            {
                this._xi = RestrictedDouble.NonNegMax_10(value);
            }
        }
        [XmlAttribute("accidental_favourable")]
        public double AccidentalFavourable { get; set; }
        [XmlAttribute("accidental_unfavourable")]
        public double AccidentalUnfavourable { get; set; }
        [XmlElement("load_case")]
        public List<ModelLoadCaseInGroup> ModelLoadCase { get; set;} // sequence: ModelLoadCaseInGroup
        [XmlAttribute("relationship")]
        public ELoadGroupRelationship Relationship { get; set; }

        /// <summary>
        /// parameterless constructor for serialization
        /// </summary>
        public ModelLoadGroupPermanent() { }

        /// <summary>
        /// Public constructor.
        /// </summary>
        /// <param name="standardFavourable">Safety factor for favourable load.</param>
        /// <param name="standardUnfavourable">Safety factor for unfavourable load.</param>
        /// <param name="accidentalFavourable">Safety factor for favourable load for accidental load combinations.</param>
        /// <param name="accidentalUnfavourable">Safety factor for unfavourable load for accidental load combinations.</param>
        /// <param name="loadCases">List of load cases in the load group</param>
        /// <param name="relationsship">Specifies how to condider the load cases in combinations</param>
        /// <param name="xi">Xi-factor used in the combinations, see EN 1990.</param>
        public ModelLoadGroupPermanent(double standardFavourable, 
                                       double standardUnfavourable, double accidentalFavourable,
                                       double accidentalUnfavourable, List<LoadCase> loadCases, 
                                       ELoadGroupRelationship relationsship, double xi)
        {
            this.StandardFavourable = standardFavourable;
            this.StandardUnfavourable = standardUnfavourable;
            this.AccidentalFavourable = accidentalFavourable;
            this.AccidentalUnfavourable = accidentalUnfavourable;
            this.Relationship = relationsship;
            this.Xi = xi;

            ModelLoadCase = new List<ModelLoadCaseInGroup>();
            for (int i = 0; i < loadCases.Count; i++)
            {
                AddLoadCase(loadCases[i]);
            }
        }

        /// <summary>
        /// Add LoadCase to group.
        /// </summary>
        private void AddLoadCase(LoadCase loadCase)
        {
            if (LoadCaseInLoadGroup(loadCase))
            {
                // pass
            }
            else
            {
                ModelLoadCase.Add(new ModelLoadCaseInGroup(loadCase.Guid));
            }
        }

        /// <summary>
        /// Check if LoadCase in LoadGroup.
        /// </summary>
        private bool LoadCaseInLoadGroup(LoadCase loadCase)
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