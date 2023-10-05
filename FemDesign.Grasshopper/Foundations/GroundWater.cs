using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.Grasshopper
{
    public class GroundWater : FEM_Design_API_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public GroundWater()
          : base("GroundWater", "GroundWater", "Create a Ground Water element.",
            CategoryName.Name(),
            SubCategoryName.Cat0())
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Name", "Name", "", GH_ParamAccess.item);
            pManager.AddColourParameter("Color", "Color", "", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.Register_GenericParam("GroundWater", "GroundWater", "");
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string name = null;
            DA.GetData(0, ref name);

            System.Drawing.Color? color = null;
            DA.GetData(1, ref color);

            var groundWater = new FemDesign.Soil.GroundWater(name, color);

            DA.SetData(0, groundWater);
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
                return FemDesign.Properties.Resources.GroundWater;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{C78E71B0-BD2C-4619-82B5-2C38E30FF6E3}"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.quarternary;
    }
}