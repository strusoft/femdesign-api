// https://strusoft.com/
using System.Xml.Serialization;
using System.Xml.Linq;

namespace FemDesign.Calculate
{
    /// <summary>
    /// fdscript.xsd
    /// CMDCALCULATION
    /// </summary>
    [System.Serializable]
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

    /// <summary>
    /// fdscript.xsd
    /// CMDCALCULATION
    /// </summary>
    [XmlRoot("cmduser")]
    [System.Serializable]
    public partial class CmdDesignDesignChanges2 : CmdCommand
    {
        [XmlAttribute("command")]
        public string Command; // token

        public static string Cmd = "; CXL FEM $CODE(DESCHANGESAPPLY)";

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        public CmdDesignDesignChanges2()
        {
            this.Command = CmdDesignDesignChanges2.Cmd;
        }

        public override XElement ToXElement()
        {
            return Extension.ToXElement<CmdDesignDesignChanges2>(this);
        }
    }

}