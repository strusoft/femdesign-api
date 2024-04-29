using FemDesign.GenericClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;


namespace FemDesign.Loads
{
    /// <summary>
    /// Specifies if the load vector describes a force or a moment
    /// </summary>
    public enum ForceLoadType
    {
        /// <summary>
        /// Force
        /// </summary>
        [XmlEnum("force")]
        [Parseable("Force", "force")]
        Force,

        /// <summary>
        /// Moment
        /// </summary>
        [XmlEnum("moment")]
        [Parseable("Moment", "moment")]
        Moment,
    }
}
