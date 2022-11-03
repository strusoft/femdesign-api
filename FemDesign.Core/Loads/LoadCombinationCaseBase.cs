// https://strusoft.com/

using System.Xml.Serialization;

namespace FemDesign.Loads
{
    public class LoadCombinationCaseBase // spec_load_case_item
    {
        [XmlAttribute("gamma")]
        public double Gamma;

        /// <summary>
        /// Parameterless constructor for serialization
        /// </summary>
        protected LoadCombinationCaseBase() { }
        public LoadCombinationCaseBase(double gamma)
        {
            Gamma = gamma;
        }
    }
}