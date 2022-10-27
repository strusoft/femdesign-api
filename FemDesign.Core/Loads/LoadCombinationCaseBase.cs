// https://strusoft.com/

using System.Xml.Serialization;

namespace FemDesign.Loads
{
    public class LoadCombinationCaseBase // spec_load_case_item
    {
        [XmlAttribute("gamma")]
        public double Gamma;

        public LoadCombinationCaseBase(double gamma)
        {
            Gamma = gamma;
        }
    }
}