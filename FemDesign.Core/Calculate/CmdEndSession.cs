// https://strusoft.com/
using System.Xml.Serialization;
using System.Xml.Linq;


namespace FemDesign.Calculate
{
    [XmlRoot("cmdendsession")]
    [System.Serializable]
    public partial class CmdEndSession : CmdCommand
    {
        public override XElement ToXElement()
        {
            return Extension.ToXElement<CmdEndSession>(this);
        }
    }

}