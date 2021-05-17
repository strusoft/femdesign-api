// https://strusoft.com/

using System.Xml.Serialization;

namespace FemDesign.Materials
{
    /// <summary>
    /// material_type --> timber
    /// </summary>
    [System.Serializable]
    public partial class Timber: MaterialBase
    {
        [XmlAttribute("type")]
        public string Type { get; set; } // integer
        [XmlAttribute("quality")]
        public string Quality { get; set; } // integer
        [XmlAttribute("Fmk0")]
        public string Fmk0 { get; set; } // material_base_value
        [XmlAttribute("Fmk90")]
        public string Fmk90 { get; set; } // material_base_value
        [XmlAttribute("Ft0k")]
        public string Ft0k { get; set; } // material_base_value
        [XmlAttribute("Ft90k")]
        public string Ft90k { get; set; } // material_base_value
        [XmlAttribute("Fc0k")]
        public string Fc0k { get; set; } // material_base_value
        [XmlAttribute("Fc90k")]
        public string Fc90k { get; set; } // material_base_value
        [XmlAttribute("Fvk")]
        public string Fvk { get; set; } // material_base_value
        [XmlAttribute("E0mean")]
        public string E0mean { get; set; } // material_base_value
        [XmlAttribute("E90mean")]
        public string E90mean { get; set; } // material_base_value
        [XmlAttribute("E0comp")]
        public string E0comp { get; set; } // double
        [XmlAttribute("E90comp")]
        public string E90comp { get; set; } // double
        [XmlAttribute("Gmean")]
        public string Gmean { get; set; } // material_base_value
        [XmlAttribute("E005")]
        public string E005 { get; set; } // material_base_value
        [XmlAttribute("G005")]
        public string G005 { get; set; } // material_base_value
        [XmlAttribute("Rhok")]
        public string Rhok { get; set; } // material_base_value
        [XmlAttribute("Rhomean")]
        public string Rhomean { get; set; } // material_base_value
        [XmlAttribute("gammaM_0")]
        public string GammaM_0 { get; set; } // material_base_value
        [XmlAttribute("gammaM_1")]
        public string GammaM_1 { get; set; } // material_base_value
        [XmlAttribute("ksys")]
        public double ksys { get; set; } // double
        [XmlAttribute("k_cr")]
        public double k_cr { get; set; } // reduction_factor_type. Optional.
        [XmlAttribute("service_class")]
        public int ServiceClass { get; set; } // timber_service_class_type
        [XmlAttribute("kdefU")]
        public double kdefU { get; set; } // material_0base_value. Optional.
        [XmlAttribute("kdefSq")]
        public double kdefSq { get; set; } // material_0base_value. Optional.
        [XmlAttribute("kdefSf")]
        public double kdefSf { get; set; } // material_0base_value. Optional.
        [XmlAttribute("kdefSc")]
        public double kdefSc { get; set; } // material_0base_value. Optional.
        [XmlAttribute("gammaMfi")]
        public string GammaMFi { get; set; } // material_base_value. Optional.

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private Timber()
        {

        }

        // /// <summary>
        // /// Set material parameters to timber material.
        // /// </summary>
        // /// <param name="_ksys">System strength factor.</param>
        // /// <param name="_k_cr">-</param>
        // /// <param name="_service_class">Service class [1, 2, 3]</param>
        // /// <param name="_kdefU">kdef U/Ua/Us</param>
        // /// <param name="_kdefSq">kdef Sq</param>
        // /// <param name="_kdefSf">kdef Sf</param>
        // /// <param name="_kdefSc">kdef Sc</param>
        // internal void SetMaterialParameters(double _ksys, double _k_cr, int _service_class, double _kdefU, double _kdefSq, double _kdefSf, double _kdefSc)
        // {
        //     this.ksys = _ksys;
        //     if (_k_cr >= 0 & _k_cr <= 1)
        //     {                
        //         this.k_cr = _k_cr;
        //     }
        //     else
        //     {
        //         throw new System.ArgumentException("0 <= k_cr <= 1");
        //     }
        //     if (_service_class == 1 | _service_class == 2 | _service_class == 3)
        //     {
        //         this.service_class = _service_class - 1; // struxml service class is [0, 1 ,2] = [1, 2, 3]
        //     }
        //     else
        //     {
        //         throw new System.ArgumentException("service_class must be 1, 2 or 3");
        //     }
        //     this.kdefU = _kdefU;
        //     this.kdefSq = _kdefSq;
        //     this.kdefSf = _kdefSf;
        //     this.kdefSc = _kdefSc;
        // }
    }
}