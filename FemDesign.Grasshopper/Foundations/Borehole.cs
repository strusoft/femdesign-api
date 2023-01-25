using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.Grasshopper
{
    public class Borehole : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public Borehole()
          : base("Borehole", "Borehole", "Create a Borehole element.",
            CategoryName.Name(),
            SubCategoryName.Cat0())
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Position", "Position", "", GH_ParamAccess.item);
            pManager.AddPointParameter("FinalGroundLevel", "FinalGroundLevel", "Final ground level [m]", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("StrataLevels", "StrataLevels", "", GH_ParamAccess.list);
            pManager.AddNumberParameter("WaterLevels", "WaterLevels", "", GH_ParamAccess.list);
            pManager.AddTextParameter("Identifier", "Identifier", "", GH_ParamAccess.item, "BH");
            pManager[pManager.ParamCount - 1].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.Register_GenericParam("Borehole", "Borehole", "");
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var pos = Rhino.Geometry.Point3d.Origin;
            DA.GetData(0, ref pos);

            var finalGroundLevel = 0.0;
            DA.GetData(1, ref finalGroundLevel);

            var strataLevels = new List<double>();
            DA.GetDataList(2, strataLevels);

            var waterLevels = new List<double>();
            DA.GetDataList(3, waterLevels);

            string identifier = "BH";
            DA.GetData(4, ref identifier);

            var allLevels = new FemDesign.Soil.AllLevels(strataLevels, waterLevels);

            var borehole = new FemDesign.Soil.BoreHole(pos.X, pos.Y, finalGroundLevel, allLevels, identifier);

            DA.SetData(0, borehole);
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
                return FemDesign.Properties.Resources.boreholes;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{C9CADB17-A5EA-4F17-B87C-8C6EA149429D}"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.tertiary;
    }
}