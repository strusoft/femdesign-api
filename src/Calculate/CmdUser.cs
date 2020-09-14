// https://strusoft.com/
using System.Xml.Serialization;


namespace FemDesign.Calculate
{
    public class CmdUser
    {
        [XmlAttribute("command")]
        public string _command; // token
        [XmlIgnore]
        public string Command
        {
            get {return this._command;}
            set {this._command = "; CXL $MODULE " + RestrictedString.CmdUserModule(value);}
        }

        
        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>                          
        private CmdUser()
        {
            
        }
        public CmdUser(string module)
        {
            this.Command = module;
        }
    }
}