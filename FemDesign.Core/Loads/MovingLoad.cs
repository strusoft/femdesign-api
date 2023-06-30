using FemDesign.Bars.Buckling;
using FemDesign.Geometry;
using FemDesign.LibraryItems;
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
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("vehicle")]
        public Guid vehicleGuid;

        [DefaultValue(0.0)]
        [XmlAttribute("vehicle_shift_x")]
        public double _vehicleShiftX = 0.0;
        [XmlIgnore]
        public double VehicleShiftX
        {
            get
            {
                return _vehicleShiftX;
            }
            set
            {
                _vehicleShiftX = RestrictedDouble.ValueInClosedInterval(value, -1000, 1000);
            }
        }

        [DefaultValue(0.0)]
        [XmlAttribute("vehicle_shift_y")]
        public double _vehicleShiftY = 0.0;
        [XmlIgnore]
        public double VehicleShiftY
        {
            get
            {
                return _vehicleShiftY;
            }
            set
            {
                _vehicleShiftY = RestrictedDouble.ValueInClosedInterval(value, -1000, 1000);
            }
        }

        [XmlElement("division_points")]
        public int _divisionPoint;

        [XmlIgnore]
        public int DivisionPoint
        {
            get { return _divisionPoint; }
            set { _divisionPoint = RestrictedInteger.ValueInRange(value, 1, 1000); }
        }

        [XmlElement("division_distance")]
        private double _divisionDistance;

        [XmlIgnore]
        public double DivisionDistance
        {
            get { return _divisionDistance; }
            set { _divisionDistance = RestrictedDouble.ValueInClosedInterval(value, 0.1, 1000); }
        }

        [XmlElement("path_position")]
        private List<Point3d> _pathPosition;

        [XmlIgnore]
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

        public MovingLoad(string name, Guid vehicleGuid, List<Point3d> pathPosition, List<Point3d> vehiclePosition, bool returnPath = false, bool lockDirection = false, bool cutLoadsToPathExtent = false)
        {
            this.EntityCreated();
            this.Name = name;
            this.vehicleGuid = vehicleGuid;
            this.PathPosition = pathPosition;

            this.Vehicle = new VehiclePosition(vehiclePosition);
            this.DivisionDistance = vehiclePosition.Count;
            this.Return = returnPath;
            this.LockDirection = lockDirection;
            this.CutToPath = cutLoadsToPathExtent;
        }
    }

    public partial class VehiclePosition
    {
        private VehiclePosition()
        {

        }
        public VehiclePosition(List<Point3d> Position)
        {
            this.Position = Position;
        }

        [XmlElement("position")]
        public List<Point3d> Position { get; set; }
    }
}