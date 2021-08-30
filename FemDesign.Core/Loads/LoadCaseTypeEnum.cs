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
        // TODO:
        //"seis_max",
        //"deviation",
        //"notional"

        /// <summary>
        /// "Ordinary" load type
        /// </summary>
        [Parseable("static", "Static", "STATIC")]
        [XmlEnum("static")]
        Static,

        /// <summary>
        /// "Struc. dead load" load type
        /// </summary>
        [Parseable("dead_load", "Dead_load", "DEAD_LOAD")]
        [XmlEnum("dead_load")]
        DeadLoad,

        /// <summary>
        /// "Soil dead load" load type
        /// </summary>
        [Parseable("soil", "soil_dead_load", "Soil_dead_load", "SOIL_DEAD_LOAD")]
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
    }
}
