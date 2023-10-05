using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.Grasshopper
{
    public class Stratum : FEM_Design_API_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public Stratum()
          : base("Stratum", "Stratum", "Create a Stratum element.",
            CategoryName.Name(),
            SubCategoryName.Cat0())
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Material", "Material", "", GH_ParamAccess.item);
            pManager.AddColourParameter("Color", "Color", "", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.Register_GenericParam("Stratum", "Stratum", "");
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            FemDesign.Materials.Material material = null;
            DA.GetData(0, ref material);

            System.Drawing.Color? color = null;
            DA.GetData(1, ref color);

            var stratum = new FemDesign.Soil.Stratum(material, color);

            DA.SetData(0, stratum);
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.Stratum;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{F8CE51AD-CEBA-4C16-BE16-19468EFEE1D9}"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.quarternary;
    }
}