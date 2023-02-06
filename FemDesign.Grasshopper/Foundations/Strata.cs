using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Linq;

namespace FemDesign.Grasshopper
{
    public class Strata : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public Strata()
          : base("Strata", "Strata", "Create a Strata element.",
            CategoryName.Name(),
            SubCategoryName.Cat0())
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("Contour", "Contour", "Define the boundary region of the soil element. The boundary edges must be straight.", GH_ParamAccess.item);
            pManager.AddGenericParameter("Stratum", "Stratum", "", GH_ParamAccess.list);
            pManager.AddGenericParameter("GroundWater", "GroundWater", "", GH_ParamAccess.list);
            pManager.AddNumberParameter("LevelLimit", "LevelLimit", "Limit Depth Level [m]", GH_ParamAccess.item);
            pManager.AddTextParameter("Identifier", "Identifier", "", GH_ParamAccess.item, "SOIL");
            pManager[pManager.ParamCount - 1].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.Register_GenericParam("Strata", "Strata", "");
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Rhino.Geometry.Curve _contourCurve = null;
            DA.GetData(0, ref _contourCurve);

            var stratum = new List<Soil.Stratum>();
            DA.GetDataList(1, stratum);

            var waterLevel = new List<Soil.GroundWater>();
            DA.GetDataList(2, waterLevel);

            double levelLimit = 0.0;
            DA.GetData(3, ref levelLimit);

            string identifier = "SOIL";
            DA.GetData(4, ref identifier);

            var listCurves = FemDesign.Grasshopper.GeomUtility.Explode(_contourCurve);

            var _contour = new List<Rhino.Geometry.Point3d>();
            foreach (var curve in listCurves)
            {
                if (!curve.IsLinear())
                {
                    this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Curve must have straight edges");
                    return;
                }
                else
                {
                    _contour.Add(curve.PointAtStart);
                    _contour.Add(curve.PointAtEnd);
                }
            }

            _contour = Rhino.Geometry.Point3d.CullDuplicates(_contour, FemDesign.Tolerance.Point3d).ToList();

            var contour = new List<Geometry.Point2d>();
            foreach(var point in _contour)
            {
                var point2d = new FemDesign.Geometry.Point2d(point.X, point.Y);
                contour.Add(point2d);
            }

            var strata = new FemDesign.Soil.Strata(stratum, waterLevel, contour, levelLimit, identifier);

            DA.SetData(0, strata);
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return FemDesign.Properties.Resources.Strata;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("3AF58EDB-A0FC-4D45-923B-9454A1E7D89B"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.quarternary;
    }
}