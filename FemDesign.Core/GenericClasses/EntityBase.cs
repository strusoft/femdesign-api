// https://strusoft.com/
using System.Xml.Serialization;


namespace FemDesign
{
    /// <summary>
    /// entity_attribs
    /// </summary>
    [System.Serializable]
    public partial class EntityBase
    {
        [XmlAttribute("guid")]
        public System.Guid Guid { get; set; }
        [XmlAttribute("last_change")]
        public string _lastChange;
        [XmlIgnore]
        internal System.DateTime LastChange
        {
            get
            {
                return System.DateTime.Parse(this._lastChange);
            }
            set
            {
                this._lastChange = value.ToString("yyyy-MM-ddTHH:mm:ss.fff");
            }
        }
        [XmlAttribute("action")]
        public string Action { get; set; }

        /// <summary>
        /// Invoke when an instance is created.
        /// 
        /// Creates a new guid, adds timestamp and changes action.
        /// </summary>
        public void EntityCreated()
        {
            this.Guid = System.Guid.NewGuid();
            this.LastChange = System.DateTime.UtcNow;
            this.Action = "added";
        }

        /// <summary>
        /// Invoke when an instance is modified.
        /// 
        /// Changes timestamp and action.
        /// </summary>
        public void EntityModified()
        {
            this.LastChange = System.DateTime.UtcNow;
            this.Action = "modified";
        }
    }
}