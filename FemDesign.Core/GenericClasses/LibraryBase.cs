// https://strusoft.com/
using System.Xml.Serialization;
using FemDesign.GenericClasses;


namespace FemDesign
{
    /// <summary>
    /// entity_attribs
    /// </summary>
    [System.Serializable]
    public partial class LibraryBase : EntityBase, ILibraryBase
    {
        [XmlIgnore]
        private string _identifier;
        
        [XmlAttribute("name")]
        public string Identifier
        {
            get
            {
                return this._identifier;
            }
            set
            {
                this._identifier = RestrictedString.Length(value, 40);
            }
        }

        public LibraryBase()
        {

        }
        
    }
}