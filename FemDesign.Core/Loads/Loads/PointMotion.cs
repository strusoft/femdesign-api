// https://strusoft.com/

using System.Xml.Serialization;
using FemDesign.Geometry;
using FemDesign.Utils;
using System;

namespace FemDesign.Loads
{
    /// <summary>
    /// point_load_type
    /// </summary>
    [System.Serializable]
    public partial class PointMotion : SupportMotionBase
    {
        [XmlElement("direction")]
        public Geometry.Vector3d Direction { get; set; } // point_type_3d
        [XmlElement("displacement")]
        public LoadLocationValue Displacement { get; set; } // location_value

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private PointMotion()
        {

        }

        public PointMotion(Geometry.Point3d point, Geometry.Vector3d disp, LoadCase loadCase, string comment, SupportMotionType type)
        {
            this.EntityCreated();
            this.LoadCase = loadCase;
            this.Comment = comment;
            this.SupportMotionType = type;
            this.Direction = disp.Normalize();
            this.Displacement = new LoadLocationValue(point, disp.Length());
        }


        public static PointMotion Motion(Geometry.Point3d point, Geometry.Vector3d disp, LoadCase loadCase, string comment = "")
        {
            return new PointMotion(point, disp, loadCase, comment, SupportMotionType.Motion);
        }

        public static PointMotion Rotation(Geometry.Point3d point, Geometry.Vector3d disp, LoadCase loadCase, string comment = "")
        {
            return new PointMotion(point, disp, loadCase, comment, SupportMotionType.Rotation);
        }

        

        public override string ToString()
        {
            var units = this.SupportMotionType == SupportMotionType.Motion ? "m" : "rad";
            var text = $"{this.GetType().Name} Pos: ({this.Displacement.X.ToString("0.00")}, {this.Displacement.Y.ToString("0.00")}, {this.Displacement.Z.ToString("0.00")}), {this.SupportMotionType}: {this.Direction * this.Displacement.Value} {units}";
            if (LoadCase != null)
                return text + $", LoadCase: {this.LoadCase.Name}";
            else
                return text;
        }
    }
}