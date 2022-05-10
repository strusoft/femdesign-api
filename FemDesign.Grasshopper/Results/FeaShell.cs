using System;
using System.Collections.Generic;
using Grasshopper;
using Grasshopper.Kernel;
using System.Linq;
using Rhino.Geometry;
using FemDesign.Results;

namespace FemDesign.Grasshopper
{
    public class FeaShell : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the FeaNode class.
        /// </summary>
        public FeaShell()
          : base("Results.FeaShell", "FeaShell",
              "Deconstruct an Fea Shell in his Part",
              "FEM-Design", "Results")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("FeaShell", "FeaShell", "Result to be Parse", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.Register_StringParam("Identifier", "Id", "Face Name");
            pManager.Register_IntegerParam("ElementId", "ElementId", "Element Id");
            pManager.Register_MeshFaceParam("FaceIndex", "FaceIndex", "Face Indexes as per FEM Design Model. FEM Design start counting from 1!");
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var FeaShell = new List<FemDesign.Results.FeaShell>();
            DA.GetDataList("FeaShell", FeaShell);

            // Read Result from Abstract Method
            var result = FemDesign.Results.FeaShell.DeconstructFeaShell(FeaShell);


            var id = (List<string>)result["Identifier"];
            var elementId = (List<int>)result["ElementId"];
            var feaShellFaces = (List<FemDesign.Geometry.Face>)result["Face"];


            // Convert the FDface to Rhino
            var oFeaShellFaces = feaShellFaces.Select(x => x.ToRhino());

            // Set output
            DA.SetDataList("Identifier", id);
            DA.SetDataList("ElementId", elementId);
            DA.SetDataList("FaceIndex", oFeaShellFaces);
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary;

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("0DA5C6E4-DA66-46CF-8058-4C83F2B27FDE"); }
        }
    }
}