// https://strusoft.com/

using System.Collections.Generic;
using System.Xml.Serialization;

namespace FemDesign.Supports
{
    /// <summary>
    /// supports
    /// </summary>
    [System.Serializable]
    public partial class Supports
    {
        [XmlElement("point_support", Order = 1)]
        public List<PointSupport> PointSupport = new List<PointSupport>(); // point_support_type
        [XmlElement("line_support", Order = 2)]
        public List<LineSupport> LineSupport = new List<LineSupport>(); // line_support_type
        [XmlElement("surface_support", Order = 3)] 
        public List<SurfaceSupport> SurfaceSupport = new List<SurfaceSupport>(); // surface_support
        [XmlElement("stiffness_point", Order = 4)]
        public List<StiffnessPoint> StiffnessPoint = new List<StiffnessPoint>(); // surface_support

        public List<GenericClasses.ISupportElement> GetSupports()
        {
            var objs = new List<GenericClasses.ISupportElement>();
            objs.AddRange(this.PointSupport);
            objs.AddRange(this.LineSupport);
            objs.AddRange(this.SurfaceSupport);
            return objs;
        }

    }
}