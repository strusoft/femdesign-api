using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using FemDesign.GenericClasses;

namespace FemDesign.Bars.Buckling
{
    /// <summary>
    /// Buckling types
    /// </summary>
    [System.Serializable]
    public enum BucklingType
    {
        /// <summary>
        /// 
        /// </summary>
        [Parseable("flexural_weak")]
        [XmlEnum("flexural_weak")]
        FlexuralWeak,
        
        /// <summary>
        /// 
        /// </summary>
        [Parseable("flexural_stiff")]
        [XmlEnum("flexural_stiff")]
        FlexuralStiff,

        /// <summary>
        /// Only for steel bars. It is the top flange from FEM-Design 18
        /// </summary>
        [Parseable("pressured_flange")]
        [XmlEnum("pressured_flange")]
        PressuredTopFlange,

        /// <summary>
        /// Only for timber bars
        /// </summary>
        [Parseable("lateral_torsional")]
        [XmlEnum("lateral_torsional")]
        LateralTorsional,
        
        /// <summary>
        /// From FD18; only for steel bars
        /// </summary>
        [Parseable("pressured_bottom_flange")]
        [XmlEnum("pressured_bottom_flange")]
        PressuredBottomFlange,
    }
}
