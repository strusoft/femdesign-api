// https://strusoft.com/
using System.Xml.Serialization;

#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign
{
    [System.Serializable]
    [IsVisibleInDynamoLibrary(false)]
    public class ForceLoadBase: LoadBase
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