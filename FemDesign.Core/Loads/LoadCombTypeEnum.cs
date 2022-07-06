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
    /// FEM-Design Load Combination Type.
    /// </summary>
    public enum LoadCombType
    {
        /// <summary>
        /// Ultimate Ordinary, "U", load combination type.
        /// </summary>
        [Parseable("u", "U", "ultimate_ordinary", "Ultimate_ordinary", "ULTIMATE_ORDINARY", "UltimateOrdinary")]
        [XmlEnum("ultimate_ordinary")]
        UltimateOrdinary,

        /// <summary>
        /// Ultimate accidental, "Ua", load combination type.
        /// </summary>
        [Parseable("ua", "Ua", "ultimate_accidental", "Ultimate_accidental", "ULTIMATE_ACCIDENTAL", "UltimateAccidental")]
        [XmlEnum("ultimate_accidental")]
        UltimateAccidental,

        /// <summary>
        /// Ultimate seismic, "Us", load combination type.
        /// </summary>
        [Parseable("us", "Us", "ultimate_seismic", "Ultimate_seismic", "ULTIMATE_SEISMIC", "UltimateSeismic")]
        [XmlEnum("ultimate_seismic")]
        UltimateSeismic,

        /// <summary>
        /// Serviceability quasi permanent, "Sq", load combination type.
        /// </summary>
        [Parseable("sq", "Sq", "serviceability_quasi_permanent", "Serviceability_quasi_permanent", "SERVICEABILITY_QUASI_PERMANENT", "ServicabilityQuasiPermanent")]
        [XmlEnum("serviceability_quasi_permanent")]
        ServiceabilityQuasiPermanent,

        /// <summary>
        /// Serviceability frequent, "Sf", load combination type.
        /// </summary>
        [Parseable("sf", "Sf", "serviceability_frequent", "Serviceability_frequent", "SERVICEABILITY_FREQUENT", "ServicabilityFrequent")]
        [XmlEnum("serviceability_frequent")]
        ServiceabilityFrequent,

        /// <summary>
        /// Serviceability characteristic, "Sc", load combination type.
        /// </summary>
        [Parseable("sc", "Sc", "serviceability_characteristic", "Serviceability_characteristic", "SERVICEABILITY_CHARACTERISTIC", "ServiceabilityCharacteristic")]
        [XmlEnum("serviceability_characteristic")]
        ServiceabilityCharacteristic,
    }
}
