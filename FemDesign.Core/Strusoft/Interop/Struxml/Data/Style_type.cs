using System;
using System.Xml.Serialization;

namespace StruSoft.Interop.StruXml.Data
{
    public partial class Style_type
    {
        /// <value>
        /// Used to store custom layers
        /// </value>
        [XmlIgnore]
        public Layer_type LayerObj;
    }
}
