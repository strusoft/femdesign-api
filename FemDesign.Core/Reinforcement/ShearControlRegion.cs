using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FemDesign.Reinforcement
{

    [System.Serializable]
    public partial class ShearControlAutoType
    {
        [XmlAttribute("connected_structure")]
        public Guid ConnectedStructureGuid { get; set; }
    }

    [System.Serializable]
    public partial class ShearControlRegionType : EntityBase
    {
        [XmlElement("automatic", Order = 1)]
        public ShearControlAutoType Automatic { get; set; }

        [XmlElement("contour", Order = 2)]
        public Geometry.Contour Contour { get; set; }

        [XmlAttribute("base_plate")]
        public Guid BasePlate { get; set; }

        [XmlAttribute("ignore_shear_check")]
        public bool IgnoreShearCheck { get; set; }

        [XmlAttribute("x")]
        public double _x { get; set; }

        [XmlIgnore]
        public double X
        {
            get { return this._x; }
            set { this._x = RestrictedDouble.NonNegMax_20(value); }
        }

        [XmlAttribute("physical_extension")]
        public double _physicalExtension { get; set; }

        [XmlIgnore]
        public double PhysicalExtension
        {
            get { return this._physicalExtension; }
            set { this._physicalExtension = RestrictedDouble.ValueInRange(0.01, 100, value); }
        }

        [XmlAttribute("reduce_shear_forces")]
        public bool ReduceShearForces { get; set; }
    }
}
