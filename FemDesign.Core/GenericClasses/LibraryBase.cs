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