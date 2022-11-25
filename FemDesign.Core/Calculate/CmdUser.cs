// https://strusoft.com/
using System.Xml.Serialization;
using System.Xml.Linq;

namespace FemDesign.Calculate
{
    [XmlRoot("cmduser")]
    [System.Serializable]
    public partial class CmdUser : CmdCommand
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
        internal CmdUser()
        {

        }
        public CmdUser(CmdUserModule module)
        {
            this.Command = module;
        }

        public override XElement ToXElement()
        {
            return Extension.ToXElement<CmdUser>(this);
        }

    }







}