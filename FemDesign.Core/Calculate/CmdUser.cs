// https://strusoft.com/
using System.Xml.Serialization;


namespace FemDesign.Calculate
{
    public partial class CmdUser
    {
        [XmlAttribute("command")]
        public string _command; // token
        [XmlIgnore]
        public CmdUserModule Command
        {
            get { return (CmdUserModule)System.Enum.Parse(typeof(CmdUserModule), this._command.Split(new string[] { "; CXL $MODULE " }, System.StringSplitOptions.None)[0]); }
            set { this._command = "; CXL $MODULE " + value.ToString(); }
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>                          
        private CmdUser()
        {
            
        }
        public CmdUser(CmdUserModule module)
        {
            this.Command = module;
        }
    }
}