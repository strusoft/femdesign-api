// https://strusoft.com/

using System.Xml.Serialization;
using System.Collections.Generic;

namespace FemDesign.Materials
{
    /// <summary>
    /// material_type --> steel
    /// </summary>
    [System.Serializable]
    public partial class Steel: MaterialBase
    {
        [XmlElement("tda_creep")]
        public StruSoft.Interop.StruXml.Data.Tda_creep1 CreepTimeDependant { get; set; }

        [XmlElement("plastic_analysis_data")]
        public StruSoft.Interop.StruXml.Data.Steel_pl_data Plasticity { get; set; }

        [XmlAttribute("Fyk16")]
        public string Fyk16 { get; set; } // material_base_value
        [XmlAttribute("Fyk40")]
        public string Fyk40 { get; set; } // material_base_value
        [XmlAttribute("Fyk63")]
        public string Fyk63 { get; set; } // material_base_value
        [XmlAttribute("Fyk80")]
        public string Fyk80 { get; set; } // material_base_value
        [XmlAttribute("Fyk100")]
        public string Fyk100 { get; set; } // material_base_value
        [XmlAttribute("Fyk150")]
        public string Fyk150 { get; set; } // material_base_value
        [XmlAttribute("Fyk200")]
        public string Fyk200 { get; set; } // material_base_value
        [XmlAttribute("Fyk250")]
        public string Fyk250 { get; set; } // material_base_value
        [XmlAttribute("Fyk400")]
        public string Fyk400 { get; set; } // material_base_value
        [XmlAttribute("Fuk3")]
        public string Fuk3 { get; set; } // material_base_value
        [XmlAttribute("Fuk40")]
        public string Fuk40 { get; set; } // material_base_value
        [XmlAttribute("Fuk100")]
        public string Fuk100 { get; set; } // material_base_value
        [XmlAttribute("Fuk150")]
        public string Fuk150 { get; set; } // material_base_value
        [XmlAttribute("Fuk250")]
        public string Fuk250 { get; set; } // material_base_value
        [XmlAttribute("Fuk400")]
        public string Fuk400 { get; set; } // material_base_value
        [XmlAttribute("gammaM0_0")]
        public string gammaM0_0 { get; set; } // material_base_value
        [XmlAttribute("gammaM0_1")]
        public string gammaM0_1 { get; set; } // material_base_value
        [XmlAttribute("gammaM1_0")]
        public string gammaM1_0 { get; set; } // material_base_value
        [XmlAttribute("gammaM1_1")]
        public string gammaM1_1 { get; set; } // material_base_value
        [XmlAttribute("gammaM2_0")]
        public string gammaM2_0 { get; set; } // material_base_value
        [XmlAttribute("gammaM2_1")]
        public string gammaM2_1 { get; set; } // material_base_value
        [XmlAttribute("gammaM5_0")]
        public string gammaM5_0 { get; set; } // material_base_value
        [XmlAttribute("gammaM5_1")]
        public string gammaM5_1 { get; set; } // material_base_value
        [XmlAttribute("Ek")]
        public string Ek { get; set; } // material_base_value
        [XmlAttribute("Ed_0")]
        public string Ed_0 { get; set; } // double
        [XmlAttribute("Ed_1")]
        public string Ed_1 { get; set; } // double
        [XmlAttribute("nu")]
        public string nu { get; set; } // material_nu_value
        [XmlAttribute("G")]
        public string G { get; set; } // material_base_value
        [XmlAttribute("alfa")]
        public string alfa { get; set; } // material_base_value

        /// <summary>
        /// Set the plasticity parameters for the steel material.
        /// </summary>
        /// <param name="plastic"></param>
        /// <param name="strainLimit"></param>
        /// <exception cref="System.ArgumentException"></exception>
        public void SetPlasticity(bool plastic = true, double strainLimit = 2.5)
        {
            // create a list with 4 times plastic value
            var plasticList = new List<bool> { plastic, plastic, plastic, plastic };
            var strainLimitList = new List<double> { strainLimit, strainLimit, strainLimit, strainLimit };

            SetPlasticity(plasticList, strainLimitList);
        }


        /// <summary>
        /// The method SetPlasticity is used to set the plasticity parameters for the steel material.
        /// The list must contain 1 or 4 values. The first value is used to set the plasticity for U, the second for Sq, the third for Sf and the fourth for Sc.
        /// </summary>
        /// <param name="plastic"></param>
        /// <param name="strainLimit"></param>
        /// <exception cref="System.ArgumentException"></exception>
        public void SetPlasticity(List<bool> plastic, List<double> strainLimit)
        {
            if(plastic.Count != 4 && strainLimit.Count != 4)
                throw new System.ArgumentException("Both list must contain 4 values.");

            this.Plasticity.Elasto_plastic_behaviour_U = plastic[0];
            this.Plasticity.Elasto_plastic_behaviour_Sq = plastic[1];
            this.Plasticity.Elasto_plastic_behaviour_Sf = plastic[2];
            this.Plasticity.Elasto_plastic_behaviour_Sc = plastic[3];


            // set U plastic data
            if (strainLimit[0] == 0)
            {
                this.Plasticity.Elasto_plastic_strain_limit_U = false;
                this.Plasticity.Elasto_plastic_strain_limit_option_U = 2.5; // this value will not be used
            }
            else if (strainLimit[0] > 0 && strainLimit[0] < 50)
            {
                this.Plasticity.Elasto_plastic_strain_limit_U = true;
                this.Plasticity.Elasto_plastic_strain_limit_option_U = strainLimit[0];
            }
            else
                throw new System.ArgumentException("Strain limit must be in range 0.00 - 50.00");

            // set Sq plastic data
            if (strainLimit[1] == 0)
            {
                this.Plasticity.Elasto_plastic_strain_limit_Sq = false;
                this.Plasticity.Elasto_plastic_strain_limit_option_Sq = 2.5; // this value will not be used
            }
            else if (strainLimit[1] > 0 && strainLimit[1] < 50)
            {
                this.Plasticity.Elasto_plastic_strain_limit_Sq = true;
                this.Plasticity.Elasto_plastic_strain_limit_option_Sq = strainLimit[1];
            }
            else
                throw new System.ArgumentException("Strain limit must be in range 0.00 - 50.00");

            // Set Sf plastic data
            if (strainLimit[2] == 0)
            {
                this.Plasticity.Elasto_plastic_strain_limit_Sf = false;
                this.Plasticity.Elasto_plastic_strain_limit_option_Sf = 2.5; // this value will not be used
            }
            else if (strainLimit[2] > 0 && strainLimit[2] < 50)
            {
                this.Plasticity.Elasto_plastic_strain_limit_Sf = true;
                this.Plasticity.Elasto_plastic_strain_limit_option_Sf = strainLimit[2];
            }
            else
                throw new System.ArgumentException("Strain limit must be in range 0.00 - 50.00");

            // Set Sc plastic data
            if (strainLimit[3] == 0)
            {
                this.Plasticity.Elasto_plastic_strain_limit_Sc = false;
                this.Plasticity.Elasto_plastic_strain_limit_option_Sc = 2.5; // this value will not be used
            }
            else if (strainLimit[3] > 0 && strainLimit[3] < 50)
            {
                this.Plasticity.Elasto_plastic_strain_limit_Sc = true;
                this.Plasticity.Elasto_plastic_strain_limit_option_Sc = strainLimit[3];
            }
            else
                throw new System.ArgumentException("Strain limit must be in range 0.00 - 50.00");


        }

    }
}