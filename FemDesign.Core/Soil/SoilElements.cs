using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FemDesign.Soil
{
    public partial class SoilElements
    {
        [XmlElement("strata", Order = 1)]
        public Strata Strata { get; set; }

        [XmlElement("borehole", Order = 2)]
        public List<BoreHole> BoreHoles { get; set; }

        // Filling
        // Excavation
        // Pipe

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private SoilElements()
        {
        }

        public SoilElements(Strata strata, List<BoreHole> boreholes)
        {
            this.Strata = strata;
            this.BoreHoles = boreholes;
        }
    }
}