using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Autodesk.DesignScript.Runtime;


namespace FemDesign.Loads
{
    /// <summary>
    /// Specifies if the load vector describes a force or a moment
    /// </summary>
    [IsVisibleInDynamoLibrary(false)]
    public enum ForceLoadType
    {
        /// <summary>
        /// Force
        /// </summary>
        [XmlEnum("force")]
        Force,

        /// <summary>
        /// Moment
        /// </summary>
        [XmlEnum("moment")]
        Moment,
    }
}
