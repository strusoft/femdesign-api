// https://strusoft.com/

using System.Xml.Serialization;
#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Materials
{
    /// <summary>
    /// material_type --> concrete
    /// </summary>
    [System.Serializable]
    [IsVisibleInDynamoLibrary(false)]
    public class Concrete: MaterialBase
    {
        [XmlAttribute("Fck")]
        public string Fck { get; set; } // material_base_value
        [XmlAttribute("Fck_cube")]
        public string Fck_cube { get; set; } // material_base_value
        [XmlAttribute("Fctk")]
        public string Fctk { get; set; } // material_base_value
        [XmlAttribute("Fctm")]
        public string Fctm { get; set; } // material_base_value
        [XmlAttribute("Ecm")]
        public string Ecm { get; set; } // material_base_value
        [XmlAttribute("gammaC_0")]
        public string gammaC_0 { get; set; } // material_base_value
        [XmlAttribute("gammaC_1")]
        public string gammaC_1 { get; set; } // material_base_value
        [XmlAttribute("gammaCE")]
        public string gammaCE { get; set; } // material_base_value
        [XmlAttribute("gammaS_0")]
        public string gammaS_0 { get; set; } // material_base_value
        [XmlAttribute("gammaS_1")]
        public string gammaS_1 { get; set; } // material_base_value
        [XmlAttribute("alfaCc")]
        public string alfaCc { get; set; } // material_base_value
        [XmlAttribute("alfaCt")]
        public string alfaCt { get; set; } // material_base_value
        [XmlAttribute("Fcd_0")]
        public string Fcd_0 { get; set; } // double
        [XmlAttribute("Fcd_1")]
        public string Fcd_1 { get; set; } // double
        [XmlAttribute("Fctd_0")]
        public string Fctd_0 { get; set; } // double
        [XmlAttribute("Fctd_1")]
        public string Fctd_1 { get; set; } // double
        [XmlAttribute("Ecd_0")]
        public string Ecd_0 { get; set; } // double
        [XmlAttribute("Ecd_1")]
        public string Ecd_1 { get; set; } // double
        [XmlAttribute("Epsc2")]
        public string Epsc2 { get; set; } // material_base_value
        [XmlAttribute("Epscu2")]
        public string Epscu2 { get; set; } // material_base_value

        [XmlAttribute("Epsc3")]
        public string Epsc3 { get; set; } // material_base_value
        [XmlAttribute("Epscu3")]
        public string Epscu3 { get; set; } // material_base_value
        [XmlAttribute("environment")]
        public string environment { get; set; } // integer
        [XmlAttribute("creep")]
        public double creep { get; set; } // material_0base_value
        [XmlAttribute("creep_sls")]
        public double creepSlq { get; set;} // material_0base_value
        [XmlAttribute("creep_slf")]
        public double creepSlf { get; set;} // material_0base_value
        [XmlAttribute("creep_slc")]
        public double creepSlc { get; set;} // material_0base_value
        [XmlAttribute("shrinkage")]
        public double shrinkage { get; set; } // non_neg_max_1000
        [XmlAttribute("nu")]
        public string nu { get; set; } // material_nu_value
        [XmlAttribute("alfa")]
        public string alfa { get; set; } // material_base_value
        // [XmlAttribute("stability")]
        // public string stability { get; set; } // reduction_factor_type

        /// <summary>
        /// Set Material parameters.
        /// </summary>
        internal void SetMaterialParameters(double _creepUls, double _creepSlq, double _creepSlf, double _creepSlc, double _shrinkage)
        {
            this.creep = _creepUls;
            this.creepSlq = _creepSlq;
            this.creepSlf = _creepSlf;
            this.creepSlc = _creepSlc;
            this.shrinkage = _shrinkage;
        }
    }
}