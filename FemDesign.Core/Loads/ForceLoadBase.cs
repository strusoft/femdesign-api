// https://strusoft.com/
using System.Xml.Serialization;


namespace FemDesign
{
    [System.Serializable]
    public partial class ForceLoadBase: LoadBase
    {
        [XmlAttribute("load_type")]
        public string _loadType; // force_load_type
        [XmlIgnore]
        public string LoadType
        {
            get {return this._loadType;}
            set {this._loadType = RestrictedString.ForceLoadType(value);}
        }
    }
}