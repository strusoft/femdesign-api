// https://strusoft.com/

using System.Globalization;
using System.Xml.Serialization;

namespace FemDesign.Materials
{
    /// <summary>
    /// material_type
    /// </summary>
    [System.Serializable]
    public class Material: EntityBase
    {
        [XmlAttribute("standard")]
        public string standard { get; set; } // standardtype
        [XmlAttribute("country")]
        public string country { get; set; } // eurocodetype
        /// <summary>
        /// Name of Material.
        /// </summary>
        /// <value></value>
        [XmlAttribute("name")]
        public string name { get; set; } // name256
        [XmlElement("timber")]
        public Timber timber { get; set; }
        [XmlElement("concrete")]
        public Concrete concrete { get; set; }
        [XmlElement("custom")]
        public Custom custom { get; set; }
        [XmlElement("steel")]
        public Steel steel { get; set; }
        [XmlElement("reinforcing_steel")]
        public ReinforcingSteel reinforcingSteel { get; set; }

        /// <summary>
        /// Get Material from MaterialDatabase by name.
        /// </summary>
        /// <remarks>Action</remarks>
        /// <param name="materialDatabase">MaterialDatabase</param>
        /// <param name="materialName">Name of Material</param>
        /// <returns></returns>
        public static Material GetMaterialByName(MaterialDatabase materialDatabase, string materialName)
        {
            if (materialDatabase.materials != null)
            {
                foreach (Material material in materialDatabase.materials.material)
                {
                    if (material.name == materialName)
                    {
                        // update object information
                        material.guid = System.Guid.NewGuid();
                        material.EntityModified();

                        // return
                        return material;
                    }
                }
            }
            if (materialDatabase.reinforcingMaterials != null)
            {
                foreach (Material material in materialDatabase.reinforcingMaterials.material)
                {
                    if (material.name == materialName)
                    {
                        // update object information
                        material.guid = System.Guid.NewGuid();
                        material.EntityModified();

                        // return
                        return material;
                    }
                }
            }
            return null;
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
        public static Material SetConcreteMaterialProperties(Material material, double creepUls = 0, double creepSlq = 0, double creepSlf = 0, double creepSlc = 0, double shrinkage = 0)
        {
            if (material.concrete != null)
            {
                material.concrete.SetMaterialParameters(creepUls, creepSlq, creepSlf, creepSlc, shrinkage);
                material.EntityModified();
            }
            else
            {
                throw new System.ArgumentException("Material must be concrete!");
            }
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