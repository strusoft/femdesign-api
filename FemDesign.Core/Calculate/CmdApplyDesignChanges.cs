// https://strusoft.com/
using System.Xml.Serialization;


namespace FemDesign.Calculate
{
    /// <summary>
    /// fdscript.xsd
    /// CMDCALCULATION
    /// </summary>
    public partial class CmdDesignDesignChanges
    {
        [XmlAttribute("command")]
        public string Command; // token

        public static string Cmd = "; CXL FEM $CODE(DESCHANGESAPPLY)";

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        public CmdDesignDesignChanges()
        {
            this.Command = CmdDesignDesignChanges.Cmd;
        }
    }
}