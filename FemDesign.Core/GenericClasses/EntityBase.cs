// https://strusoft.com/
using System;
using System.Globalization;
using System.Xml.Serialization;
using FemDesign.GenericClasses;

namespace FemDesign
{
    /// <summary>
    /// entity_attribs
    /// </summary>
    [Serializable]
    public abstract partial class EntityBase : IFemDesignEntity
    {
        [XmlAttribute("guid")]
        public Guid Guid { get; set; }
        [XmlAttribute("last_change")]
        public string _lastChange;
        [XmlIgnore]
        public DateTime LastChange
        {
            get
            {
                return DateTime.Parse(this._lastChange);
            }
            private set
            {
                this._lastChange = value.ToString("yyyy-MM-ddTHH:mm:ss.fff", CultureInfo.InvariantCulture);
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
            this.Guid = Guid.NewGuid();
            this.LastChange = DateTime.UtcNow;
            this.Action = "added";
        }

        /// <summary>
        /// Invoke when an instance is modified.
        /// 
        /// Changes timestamp and action.
        /// </summary>
        public void EntityModified()
        {
            this.LastChange = DateTime.UtcNow;
            this.Action = "modified";
        }

        public static implicit operator Guid(EntityBase entity) => entity.Guid;
    }
}