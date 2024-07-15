using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FemDesign.Calculate
{
    /// <summary>
    /// Eurocode steel design configuration
    /// </summary>
    [System.Serializable]
    public partial class SteelDesignConfiguration : CONFIG
    {
        [XmlAttribute("type")]
        public string Type = "ECSTCONFIG";

        /// <summary>
        /// Method to calculate interaction factors k_ij
        /// </summary>
        public enum Method
        {
            /// <summary>
            /// Method 1: Interaction factors k_ij for interaction formula in 6.3.3(4).
            /// </summary>
            [XmlEnum("0")]
            Method1,
            /// <summary>
            /// Method 2: Interaction factors k_ij for interaction formula in 6.3.3(4).
            /// </summary>
            [XmlEnum("1")]
            Method2
        }


        /// <summary>
        /// Interation factors k_ij formula in 6.3.3(4).
        /// </summary>
        [XmlAttribute("sInteraction")]
        public Method InteractionFactors { get; set; } = Method.Method1;

        private SteelDesignConfiguration()
        {

        }

        public SteelDesignConfiguration(Method interaction = Method.Method1)
        {
            InteractionFactors = interaction;
        }

        /// <summary>
        /// Steel design configuration use method 1 for interaction factors k_ij formula in 6.3.3(4)
        /// </summary>
        /// <returns></returns>
        public static SteelDesignConfiguration Method1()
        {
            return new SteelDesignConfiguration(Method.Method1);
        }

        /// <summary>
        /// Steel design configuration use method 2 for interaction factors k_ij formula in 6.3.3(4)
        /// </summary>
        /// <returns></returns>
        public static SteelDesignConfiguration Method2()
        {
            return new SteelDesignConfiguration(Method.Method2);
        }

    }
}
