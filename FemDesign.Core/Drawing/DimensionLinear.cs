using System.Collections.Generic;
using System.Linq;
using FemDesign.Geometry;
using FemDesign.GenericClasses;
using Struxml = StruSoft.Interop.StruXml.Data;

namespace FemDesign.Drawing
{
    public class DimensionLinear: EntityBase, IStructureElement
    {
        /// <value>
        /// Points to measure. Will measure between the points projection in the plane of the dimension line.
        /// </value>
        public List<Point3d> ReferencePoints;

        /// <value>
        /// Plane of dimension. Dimension line will be placed at origin of plane.
        /// </value>
        public Plane Plane;

        /// <summary>
        /// Construct a new linear dimension from reference points and the plane of the dimension.
        /// </summary>   
        public DimensionLinear(List<Point3d> referencePoints, Plane dimPlane)
        {
            ReferencePoints = referencePoints;
            Plane = dimPlane;
        }

        public List<double> Dimensions
        {
            get
            {
                throw new System.Exception("Not implemented");
            }
        }
        //public static implicit operator Struxml.Dimline_type(DimensionLinear d) => new Struxml.Dimline_type{
        //    Point = d.ReferencePoints.Select(x => (Struxml.Point_type_3d)x).ToList(),
        //    Position = d.Plane.Origin,
        //    Plane_x = d.Plane.LocalX,
        //    Plane_y = d.Plane.LocalY,
        //    Dimension_line = d.DimensionLine,
        //    Extension_line = d.ExtensionLine,
        //    Arrow = d.Arrow,
        //    Font = d.Font,
        //    Text = new List<Struxml.Dimtext_type>{d.Text}
        //};
    }
}
