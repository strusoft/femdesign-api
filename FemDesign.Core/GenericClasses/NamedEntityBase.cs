// https://strusoft.com/
using System;
using System.Xml.Serialization;
using System.Text.RegularExpressions;
using FemDesign.GenericClasses;

namespace FemDesign
{
    public abstract partial class NamedEntityBase : EntityBase, IFemDesignEntity, INamedEntity
    {
        [XmlAttribute("name")]
        public string _name; // identifier

        [XmlIgnore]
        public string Name => this._name.Replace("@", "");

        [XmlIgnore]
        public int Instance
        {
            get
            {
                var found = this._name.IndexOf(".");
                return int.Parse(this._name.Substring(found + 1));
            }
        }

        [XmlIgnore]
        public string Identifier
        {
            get { return this._name.Replace("@", "").Split('.')[0]; }
            set
            {
                if (string.IsNullOrEmpty(value) || _namePattern.IsMatch(value) == false)
                    throw new ArgumentException($"'{value}' is not a valid Identifier");

                if (value == this.Name) return;

                this._name = $"{value}.{GetUniqueInstanceCount()}";
            }
        }

        [XmlIgnore]
        public bool LockedIdentifier
        {
            get
            {
                return _name.StartsWith("@");
            }
            set
            {
                if (value && !LockedIdentifier)
                    _name = "@" + _name;
                else if (!value && LockedIdentifier)
                    _name = _name.Substring(1);
            }
        }

        private static readonly Regex _namePattern = new Regex(@"@{0,1}[ -#%'-;=?A-\ufffd;]{0,50}");

        protected NamedEntityBase()
        {
            this._name = "";
        }

        /// <summary>
        /// This value will be used to set number (like ".1") part of the entity name (like "B.1"). Typically this is a counter starting at 1 and incrementing for each new instance of this class that has been created.
        /// </summary>
        /// <returns>A unique number.</returns>
        protected abstract int GetUniqueInstanceCount();
    }
}