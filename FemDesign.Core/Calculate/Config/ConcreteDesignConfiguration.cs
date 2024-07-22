using FemDesign.GenericClasses;
using FemDesign.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FemDesign.Calculate
{
    [System.Serializable]
    public partial class ConcreteDesignConfig : CONFIG
    {
        [XmlAttribute("type")]
        public string Type = "ECRCCONFIG";

        public enum CalculationMethod
        {
            [XmlEnum("0")]
            [Parseable("NominalStiffness", "nominal_stiffness", "Nominal_stiffness", "NOMINAL_STIFFNESS")]
            NominalStiffness,
            [XmlEnum("1")]
            [Parseable("NominalCurvature", "nominal_curvature", "Nominal_curvature", "NOMINAL_CURVATURE")]
            NominalCurvature
        }

        [XmlAttribute("s2ndOrder")]
        public CalculationMethod SecondOrderCalculationMethod { get; set; } = ConcreteDesignConfig.CalculationMethod.NominalStiffness;

        /// <summary>
        /// Crack with load combinations
        /// </summary>
        [XmlAttribute("crackw_q")]
        public bool CrackWidthQuasiPermanent { get; set; } = true;

        /// <summary>
        /// Crack with load combinations
        /// </summary>
        [XmlAttribute("crackw_f")]
        public bool CrackWidthFrequent { get; set; } = false;

        /// <summary>
        /// Crack with load combinations
        /// </summary>
        [XmlAttribute("crackw_c")]
        public bool CrackWidthCharacteristic { get; set; } = false;

        /// <summary>
        /// Cracking calculation
        /// </summary>
        [XmlAttribute("fReopeningCracks")]
        public bool ReopeningCracks { get; set; }  = false;


        private ConcreteDesignConfig()
        {

        }

        public ConcreteDesignConfig(CalculationMethod secondOrder, bool crackQuasiPermanent = true, bool crackFrequent = false, bool crackCharacteristic = false, bool reopeningCracks = false)
        {
            SecondOrderCalculationMethod = secondOrder;
            CrackWidthQuasiPermanent = crackQuasiPermanent;
            CrackWidthFrequent = crackFrequent;
            CrackWidthCharacteristic = crackCharacteristic;
            ReopeningCracks = reopeningCracks;
        }

        public static ConcreteDesignConfig NominalStiffness(bool crackQuasiPermanent = true, bool crackFrequent = false, bool crackCharacteristic = false)
        {
            return new ConcreteDesignConfig(CalculationMethod.NominalStiffness, crackQuasiPermanent, crackFrequent, crackCharacteristic);
        }

        public static ConcreteDesignConfig NominalCurvature(bool crackQuasiPermanent = true, bool crackFrequent = false, bool crackCharacteristic = false)
        {
            return new ConcreteDesignConfig(CalculationMethod.NominalCurvature, crackQuasiPermanent, crackFrequent, crackCharacteristic);
        }

    }
}
