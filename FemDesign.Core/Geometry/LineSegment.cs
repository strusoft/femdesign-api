// https://strusoft.com/

using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Linq;


namespace FemDesign.Geometry
{
    [System.Serializable]
    public partial class LineSegment
    {
        [XmlElement("start_point")]
        public FdPoint3d StartPoint;
        [XmlElement("base_point")]
        public FdPoint3d BasePoint;
        [XmlElement("end_point")]
        public FdPoint3d EndPoint;

        [XmlIgnore]
        public List<FdPoint3d> Verticies { 
            get {
                return new List<FdPoint3d>() { StartPoint, EndPoint };
            } 
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        public LineSegment()
        {

        }

        /// <summary>
        /// Construct LineSegment from start and endpoint.
        /// </summary>
        /// <param name="verticies"></param>
        public LineSegment(FdPoint3d start, FdPoint3d end)
        {
            this.StartPoint = start;
            this.BasePoint = new FdPoint3d((start.X + end.X) / 2.0, (start.Y + end.Y) / 2.0, (start.Z + end.Z) / 2.0);
            this.EndPoint = end;
        }
    }
}