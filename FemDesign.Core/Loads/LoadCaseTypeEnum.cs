using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using FemDesign.GenericClasses;

namespace FemDesign.Loads
{
    /// <summary>
    /// FEM-Design Load Case Type.
    /// </summary>
    public enum LoadCaseType
    {
        /// <summary>
        /// "Ordinary" load type
        /// </summary>
        [Parseable("static", "Static", "STATIC")]
        [XmlEnum("static")]
        Static,

        /// <summary>
        /// "Struc. dead load" load type
        /// </summary>
        [Parseable("dead_load", "DeadLoad", "Dead_load", "DEAD_LOAD")]
        [XmlEnum("dead_load")]
        DeadLoad,

        /// <summary>
        /// "Soil dead load" load type
        /// </summary>
        [Parseable("soil", "SoilDeadLoad", "soil_dead_load", "Soil_dead_load", "SOIL_DEAD_LOAD")]
        [XmlEnum("soil_dead_load")]
        SoilDeadLoad,

        /// <summary>
        /// "Shrinkage" load type
        /// </summary>
        [Parseable("shrinkage", "Shrinkage", "SHRINKAGE")]
        [XmlEnum("shrinkage")]
        Shrinkage,

        /// <summary>
        /// "Camber sim." load type
        /// </summary>
        [Parseable("prestressing", "Prestressing", "PRESTRESSING", "camber_sim", "Camber_sim", "CAMBER_SIM")]
        [XmlEnum("prestressing")]
        Prestressing,

        /// <summary>
        /// "Fire" load type
        /// </summary>
        [Parseable("fire", "Fire", "FIRE")]
        [XmlEnum("fire")]
        Fire,

        /// <summary>
        /// Seismic "Seis load, Fx+Mx" load type
        /// </summary>
        [Parseable("seis_sxp", "Seis_sxp", "SEIS_SXP")]
        [XmlEnum("seis_sxp")]
        Seis_sxp,

        /// <summary>
        /// Seismic "Seis load, Fx-Mx" load type
        /// </summary>
        [Parseable("seis_sxm", "Seis_sxm", "SEIS_SXM")]
        [XmlEnum("seis_sxm")]
        Seis_sxm,

        /// <summary>
        /// Seismic "Seis load, Fx+My" load type
        /// </summary>
        [Parseable("seis_syp", "Seis_syp", "SEIS_SYP")]
        [XmlEnum("seis_syp")]
        Seis_syp,

        /// <summary>
        /// Seismic "Seis load, Fx-My" load type
        /// </summary>
        [Parseable("seis_sym", "Seis_sym", "SEIS_SYM")]
        [XmlEnum("seis_sym")]
        Seis_sym,

        /// <summary>
        /// (Macro generated) "Deviation" load type
        /// </summary>
        [Parseable("deviation", "Deviation", "DEVIATION")]
        [XmlEnum("deviation")]
        Deviation,

        /// <summary>
        /// (Macro generated) "Notional" load type
        /// </summary>
        [Parseable("notional", "Notional", "NOTIONAL")]
        [XmlEnum("notional")]
        Notional,

        /// <summary>
        /// 
        /// </summary>
        [Parseable("seis_max", "Seis_max", "SeisMax", "SEIS_MAX")]
        [XmlEnum("seis_max")]
        Seis_max,
    }
}
