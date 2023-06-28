using FemDesign.Bars.Buckling;
using FemDesign.Geometry;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FemDesign.Loads
{
    [System.Serializable]
    public partial class MovingLoad : EntityBase
    {

        [XmlElement("division_points")]
        private int _divisionPoint;
        public int DivisionPoint
        {
            get { return _divisionPoint; }
            set { _divisionPoint = RestrictedInteger.ValueInRange(value, 1, 1000); }
        }

        [XmlElement("division_distance")]
        private double _divisionDistance;
        public double DivisionDistance
        {
            get { return _divisionDistance; }
            set { _divisionDistance = RestrictedDouble.ValueInClosedInterval(value, 0.1, 1000); }
        }

        [XmlElement("path_position")]
        private List<Point3d> _pathPosition;
        public List<Point3d> PathPosition
        {
            get
            {
                return _pathPosition;
            }
            set
            {
                if (value.Count < 2)
                    throw new ArgumentException("List must have at least two elements.");
                _pathPosition = value;
            }
        }

        [XmlElement("vehicle_positions")]
        public VehiclePosition Vehicle { get; set; }

        [DefaultValue(false)]
        [XmlAttribute("return")]
        public bool Return { get; set; } = false;

        [DefaultValue(false)]
        [XmlAttribute("lock_direction")]
        public bool LockDirection { get; set; } = false;

        [DefaultValue(false)]
        [XmlAttribute("cut_to_path")]
        public bool CutToPath { get; set; } = false;


        private MovingLoad() { }
    }

    public partial class VehiclePosition
    {
        private VehiclePosition()
        {

        }

        [XmlElement("position")]
        public List<Point3d> Position { get; set; }
    }
}