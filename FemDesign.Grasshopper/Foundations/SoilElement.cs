using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.Grasshopper
{
    public class SoilElement : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public SoilElement()
          : base("SoilElement", "SoilElement", "Create a soil element.",
            CategoryName.Name(),
            SubCategoryName.Cat0())
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Strata", "Strata", "", GH_ParamAccess.item);
            pManager.AddGenericParameter("Boreholes", "Boreholes", "", GH_ParamAccess.list);
            pManager.AddGenericParameter("Filling", "Filling", "NOT implemented", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("Excavation", "Excavation", "NOT implemented", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("Piles", "Piles", "NOT implemented", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.Register_GenericParam("Soil", "Soil", "");
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            FemDesign.Soil.Strata strata = null;
            DA.GetData(0, ref strata);

            var boreholes = new List<FemDesign.Soil.BoreHole>();
            DA.GetDataList(1, boreholes);

            var soilElement = new FemDesign.Soil.SoilElements(strata, boreholes);

            DA.SetData(0, soilElement);
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
                return FemDesign.Properties.Resources.soil;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{2100DFB8-422A-4945-9C30-4BF69D068B2B}"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.tertiary;
    }
}