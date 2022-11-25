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
    public partial class CmdDesignDesignChangesPipe : CmdCommand
    {
        [XmlAttribute("command")]
        public string Command; // token

        public static string Cmd = "; CXL FEM $CODE(DESCHANGESAPPLY)";

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        public CmdDesignDesignChangesPipe()
        {
            this.Command = CmdDesignDesignChangesPipe.Cmd;
        }

        public override XElement ToXElement()
        {
            return Extension.ToXElement<CmdDesignDesignChangesPipe>(this);
        }
    }

}