// https://strusoft.com/

using System;
using System.Xml.Serialization;
using System.Collections.Generic;


namespace FemDesign.Composites
{
    [System.Serializable]
    public partial class CompositeSection : EntityBase
    {
        [XmlIgnore]
        public CompositeType CompositeType
        {
            get { return this._compositeType; }
            set { this._compositeType = value; }
        }

        [XmlAttribute("type")]
        public CompositeType _compositeType;

        //[XmlAttribute("composite_prop_type")]
        //public Dictionary<CompositeSectionParameter, object> Parameters;
    }
}
