// https://strusoft.com/

using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Linq;

namespace FemDesign.Composites
{
    [System.Serializable]
    public partial class CompositeSectionParameter
    {
        [XmlAttribute("name")]
        public CompositeParameterType Name { get; set; }

        [XmlAttribute("value")]
        public string Value { get; set; }

        //[XmlIgnore]
        //public Dictionary<CompositeParameterType, string> Parameter { get; set; }

        /// <summary>
        /// Parameterless constructor for serialization
        /// </summary>
        private CompositeSectionParameter()
        {

        }

        /// <summary>
        /// Construct a new CompositeSectionParameter. 
        /// </summary>
        /// <param name="paramName">Parameter name.</param>
        /// <param name="paramValue">Parameter value. Number values must be expressed in milimeter.</param>
        internal CompositeSectionParameter(CompositeParameterType paramName, string paramValue)
        {
            this.Name = paramName;
            this.Value = paramValue;
        }
               
    }
}
