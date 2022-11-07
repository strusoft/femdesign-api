// https://strusoft.com/
using System;
using System.Globalization;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Linq;

namespace FemDesign.Materials
{
    /// <summary>
    /// material_type
    /// </summary>
    [System.Serializable]
    public partial class Material: EntityBase, IMaterial
    {
        [XmlAttribute("standard")]
        public string Standard { get; set; } // standardtype
        [XmlAttribute("country")]
        public string Country { get; set; } // eurocodetype
        /// <summary>
        /// Name of Material.
        /// </summary>
        /// <value></value>
        [XmlAttribute("name")]
        public string Name { get; set; } // name256
        [XmlElement("timber")]
        public Timber Timber { get; set; }
        [XmlElement("concrete")]
        public Concrete Concrete { get; set; }
        [XmlElement("custom")]
        public Custom Custom { get; set; }
        [XmlElement("steel")]
        public Steel Steel { get; set; }
        [XmlElement("reinforcing_steel")]
        public ReinforcingSteel ReinforcingSteel { get; set; }
        [XmlElement("stratum")]
        public StruSoft.Interop.StruXml.Data.Material_typeStratum Stratum { get; set; }
        
        [XmlIgnore]
        public string Family
        {
            get
            {
                if (this.Steel != null)
                    return "Steel";
                else if (this.Concrete != null)
                    return "Concrete";
                else if (this.Timber != null)
                    return "Timber";
                else if (this.Stratum != null)
                    return "Stratum";
                else if (this.ReinforcingSteel != null)
                    return "ReinforcingSteel";
                else
                    return "Custom";
            }
        }

        public override string ToString()
        {
            return $"{this.Name}";
        }

        /// <summary>
        /// Set creep and shrinkage parameters to a concrete Material.
        /// </summary>
        /// <param name="material">Material.</param>
        /// <param name="creepUls">Creep ULS.</param>
        /// <param name="creepSlq">Creep SLS Quasi-Permanent</param>
        /// <param name="creepSlf">Creep SLS Frequent</param>
        /// <param name="creepSlc">Creep SLS Characteristic</param>
        /// <param name="shrinkage">Shrinkage.</param>
        /// <returns></returns>
        public static Material ConcreteMaterialProperties(Material material, double creepUls = 0, double creepSlq = 0, double creepSlf = 0, double creepSlc = 0, double shrinkage = 0)
        {
            if (material.Concrete != null)
            {
                // deep clone. downstreams objs will have contain changes made in this method, upstream objs will not.
                Material newMaterial = material.DeepClone();
                
                // downstream and uppstream objs will NOT share guid.
                newMaterial.EntityCreated();

                // set parameters
                newMaterial.Concrete.SetMaterialParameters(creepUls, creepSlq, creepSlf, creepSlc, shrinkage);
                newMaterial.EntityModified();

                // return
                return newMaterial;
            }
            else
            {
                throw new System.ArgumentException("Material must be concrete!");
            }
        }
        
        public static Material CustomUniaxialMaterial(string name, double mass, double e_0, double nu_0, double alfa_0)
        {
            var material = new Material();
            material.Name = name;
            material.Country = "n/a";
            material.Standard = "general";
            material.EntityCreated();

            material.Custom = new Custom(mass, e_0, nu_0, alfa_0);
            return material;
        }

        // /// <summary>
        // /// Set material properties for timber material.
        // /// </summary>
        // /// <param name="material">Material.</param>
        // /// <param name="ksys">System strength factor.</param>
        // /// <param name="k_cr">k_cr. Must be between 0 and 1.</param>
        // /// <param name="service_class">Service class. 1,2 or 3.</param>
        // /// <param name="kdefU">kdef U/Ua/Us.</param>
        // /// <param name="kdefSq">kdef Sq.</param>
        // /// <param name="kdefSf">kdef Sf.</param>
        // /// <param name="kdefSc">kdef Sc.</param>
        // /// <returns></returns>
        // public static Material SetTimberMaterialProperties(Material material, double ksys, double k_cr, int service_class, double kdefU, double kdefSq, double kdefSf, double kdefSc)
        // {
        //     if (material.timber != null)
        //     {
        //         material.timber.SetMaterialParameters(ksys, k_cr, service_class, kdefU, kdefSq, kdefSf, kdefSc);
        //     }
        //     else
        //     {
        //         throw new System.ArgumentException("Material must be timber!");
        //     }
        //     return material;
        // }
    }
}