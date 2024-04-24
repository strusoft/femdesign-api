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


    /// <summary>
    /// Specifies if the motion vector describes a motion or a rotation
    /// </summary>
    public enum SupportMotionType
    {
        /// <summary>
        /// Moment
        /// </summary>
        [XmlEnum("motion")]
        [Parseable("Motion", "motion")]
        Motion,

        /// <summary>
        /// Moment
        /// </summary>
        [XmlEnum("rotation")]
        [Parseable("Rotation", "rotation")]
        Rotation,
    }


}
