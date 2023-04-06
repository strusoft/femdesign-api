using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FemDesign.Results
{
    public partial class InteractionSurface
    {
        public Dictionary<int, FemDesign.Geometry.Point3d> Vertices { get; set; }
        public Dictionary<int, FemDesign.Geometry.Face> Faces { get; set; }
        internal static InteractionSurface ReadFromFile(string filepath)
        {

            return new InteractionSurface();
        }
    }

}