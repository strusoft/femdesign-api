using System;
using System.Collections.Generic;
using System.Linq;
using FemDesign.Geometry;
using FemDesign.GenericClasses;
using Struxml = StruSoft.Interop.StruXml.Data;

namespace FemDesign.Drawing
{
    public class DimensionLinear : EntityBase, IStructureElement
    {
        /// <value>
        /// Points to measure. Will measure between the points projection in the plane of the dimension line.
        /// </value>
        public List<Point3d> ReferencePoints;

        /// <value>
        /// Plane of dimension. Dimension line will be placed at origin of plane.
        /// </value>
        public Plane Plane;

        /// <value>
        /// Dimension text font
        /// </value>
        public Struxml.Dimtext_font_type Font;


        /// <value>
        /// Dimension line arrow
        /// </value>
        public Struxml.Arrow_type Arrow;

        /// <value>
        /// Number of decimals of measurement
        /// </value>
        public int Decimals = new Struxml.Dimtext_type().Decimals;

        /// <value>
        /// Measurement unit
        /// </value>
        public Struxml.Lengthunit_type LengthUnit = Struxml.Lengthunit_type.M;

        /// <value>
        /// Show unit after measurement
        /// </value>
        public bool ShowUnit = false;

        /// <value>
        /// Returns the distances between the reference points measured along the plane x-axis.
        /// </value>
        public List<double> Distances
        {
            get
            {
                var dims = new List<double>();
                for (int idx = 0; idx < ReferencePoints.Count; idx++)
                {
                    if (idx != 0)
                    {
                        Point3d p1 = ReferencePoints[idx - 1];
                        Point3d p2 = ReferencePoints[idx];
                        Vector3d v = p2 - p1;
                        dims.Add(v.Dot(Plane.XDir));
                    }
                }
                return dims;
            }
        }

        /// <value>
        /// Returns the positions used to place the dimension text on dimension line.
        /// y' value needs 21% extra padding
        /// </value>
        public List<Point3d> TextPositions
        {
            get
            {
                var textPositions = new List<Point3d>();
                for (int idx = 0; idx < ReferencePoints.Count; idx++)
                {
                    if (idx != 0)
                    {
                        // previous reference point
                        Point3d p1 = ReferencePoints[idx - 1];

                        // current reference point
                        Point3d p2 = ReferencePoints[idx];

                        // vector from plane origin to previous reference point
                        Vector3d v1 = p1 - Plane.Origin;

                        // vector from previous reference point to current reference point
                        Vector3d v2 = p2 - p1;

                        // project vector along plane x-axis. multiply with 0.5 to get mid.
                        Vector3d t = v1.Dot(Plane.XDir) * Plane.XDir + v2.Dot(Plane.XDir) * 0.5 * Plane.XDir;

                        // position is plane origin translated with t
                        textPositions.Add(Plane.Origin + t);
                    }
                }
                return textPositions;
            }
        }

        public List<Struxml.Dimtext_type> DimtextTypes
        {
            get
            {
                var distances = Distances;
                var textPositions = TextPositions;
                var dimTextTypes = new List<Struxml.Dimtext_type>();
                for (int idx = 0; idx < distances.Count; idx++)
                {
                    var dimTextType = new Struxml.Dimtext_type
                    {
                        Value = distances[idx],
                        Decimals = this.Decimals,
                        Length_unit = this.LengthUnit,
                        Measurement_unit = this.ShowUnit,
                        Position = textPositions[idx], // schema is incorrect?
                        Plane_x = Plane.XDir,
                        Plane_y = Plane.YDir,
                    };
                    dimTextTypes.Add(dimTextType);
                }

                return dimTextTypes;
            }
        }

        /// <value>
        /// Get action as Modification_type
        /// </value>
        public Struxml.Modification_type StruxmlAction
        {
            get
            {
                Struxml.Modification_type res;
                System.Enum.TryParse<Struxml.Modification_type>(Action, out res);
                return res;
            }
        }

        public override string ToString()
        {
            return $"DimensionLinear: O: {Plane.Origin}, X: {Plane.XDir}, Measures: {string.Join(" ", Distances.Select(x => Math.Round(x, Decimals)))}";
        }

        public void Initialise()
        {
            EntityCreated();
            Font = new Struxml.Dimtext_font_type();
            Arrow = new Struxml.Arrow_type();
        }

        /// <summary>
        /// Construct a new linear dimension from reference points and the plane of the dimension.
        /// </summary>   
        public DimensionLinear(List<Point3d> referencePoints, Plane dimPlane)
        {
            Initialise();
            ReferencePoints = referencePoints;
            Plane = dimPlane;
        }

        public static implicit operator Struxml.Dimline_type(DimensionLinear d) => new Struxml.Dimline_type
        {
            Guid = d.Guid.ToString(),
            Action = d.StruxmlAction,
            Last_change = d.LastChange,
            Point = d.ReferencePoints.Select(x => (Struxml.Point_type_3d)x).ToList(),
            Position = d.Plane.Origin,
            Plane_x = d.Plane.XDir,
            Plane_y = d.Plane.YDir,
            Dimension_line = new Struxml.Dimdimline_type { },
            Extension_line = new Struxml.Extline_type
            {
                Extension_a = 0.005,
                Extension_b = 0.005,
                Offset_c = 0.005
            },
            Arrow = new Struxml.Arrow_type
            {
                Type = Struxml.Arrowtype_type.Tick,
                Size = 0.005,
                Penwidth = 0.00018
            },
            Font = d.Font,
            Text = d.DimtextTypes
        };
    }
}
