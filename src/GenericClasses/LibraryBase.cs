// https://strusoft.com/
using System.Xml.Serialization;

#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign
{
    /// <summary>
    /// entity_attribs
    /// </summary>
    [System.Serializable]
    [IsVisibleInDynamoLibrary(false)]
    public class LibraryBase: EntityBase
    {
        [XmlIgnore]
        private string _name;
        
        [XmlAttribute("name")]
        public string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = RestrictedString.Length(value, 40);
            }
        }

        public LibraryBase()
        {

        }
        
    }
}