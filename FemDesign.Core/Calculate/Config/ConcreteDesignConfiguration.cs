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
    public partial class ConcreteConfig : CONFIG
    {
        [XmlAttribute("type")]
        public string Type = "ECRCCONFIG";

        public enum CalculationMethod
        {
            [XmlEnum("0")]
            NominalStiffness,
            [XmlEnum("1")]
            NominalCurvature
        }

        [XmlAttribute("s2ndOrder")]
        public CalculationMethod SecondOrderCalculationMethod { get; set; } = ConcreteConfig.CalculationMethod.NominalStiffness;

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


        private ConcreteConfig()
        {

        }

        public ConcreteConfig(CalculationMethod secondOrder, bool crackQuasiPermanent = true, bool crackFrequent = false, bool crackCharacteristic = false)
        {
            SecondOrderCalculationMethod = secondOrder;
            CrackWidthQuasiPermanent = crackQuasiPermanent;
            CrackWidthFrequent = crackFrequent;
            CrackWidthCharacteristic = crackCharacteristic;
        }

        public static ConcreteConfig NominalStiffness(bool crackQuasiPermanent = true, bool crackFrequent = false, bool crackCharacteristic = false)
        {
            return new ConcreteConfig(CalculationMethod.NominalStiffness, crackQuasiPermanent, crackFrequent, crackCharacteristic);
        }

        public static ConcreteConfig NominalCurvature(bool crackQuasiPermanent = true, bool crackFrequent = false, bool crackCharacteristic = false)
        {
            return new ConcreteConfig(CalculationMethod.NominalCurvature, crackQuasiPermanent, crackFrequent, crackCharacteristic);
        }

    }
}
