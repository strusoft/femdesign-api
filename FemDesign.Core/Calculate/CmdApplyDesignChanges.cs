// https://strusoft.com/
using System.Xml.Serialization;
using System.Xml.Linq;

namespace FemDesign.Calculate
{
    /// <summary>
    /// fdscript.xsd
    /// CMDCALCULATION
    /// </summary>
    [XmlRoot("cmduser")]
    [System.Serializable]
    public partial class CmdApplyDesignChanges : CmdCommand
    {
        [XmlAttribute("command")]
        public string Command; // token

        public static string Cmd = "; CXL FEM $CODE(DESCHANGESAPPLY)";

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        public CmdApplyDesignChanges()
        {
            this.Command = CmdApplyDesignChanges.Cmd;
        }

        public override XElement ToXElement()
        {
            return Extension.ToXElement<CmdApplyDesignChanges>(this);
        }
    }

}