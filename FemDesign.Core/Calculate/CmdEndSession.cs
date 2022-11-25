// https://strusoft.com/
using System.Xml.Serialization;
using System.Xml.Linq;


namespace FemDesign.Calculate
{
    [System.Serializable]
    public partial class CmdEndSession
    {
    }

    [XmlRoot("cmdendsession")]
    [System.Serializable]
    public partial class CmdEndSessionPipe : CmdCommand
    {
        public override XElement ToXElement()
        {
            return Extension.ToXElement<CmdEndSessionPipe>(this);
        }
    }

}