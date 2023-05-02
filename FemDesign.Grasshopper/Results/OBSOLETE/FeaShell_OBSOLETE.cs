using System;
using System.Collections.Generic;
using Grasshopper;
using Grasshopper.Kernel;
using System.Linq;
using Rhino.Geometry;
using FemDesign.Results;

namespace FemDesign.Grasshopper
{
    public class FeaShell_OBSOLETE : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the FeaShell class.
        /// </summary>
        public FeaShell_OBSOLETE()
          : base("FiniteElement.FeaShell", "FemShell",
              "Deconstruct an Fea Shell in his Part",
              CategoryName.Name(), SubCategoryName.Cat7b())
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("FiniteElement", "FiniteElement", "Result to be Parse", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.Register_StringParam("Identifier", "Id", "Face Name");
            pManager.Register_IntegerParam("ElementId", "ElementId", "Element Id");
            pManager.Register_MeshFaceParam("FaceIndex", "FaceIndex", "Face Indexes as per FEM Design Model. FEM Design start counting from 1!");
            pManager.Register_MeshParam("Mesh", "Mesh Geometry", "Mesh as per Fem Design");
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            FemDesign.Results.FiniteElement FiniteElement = null;
            DA.GetData("FiniteElement", ref FiniteElement);
            if (FiniteElement == null)
                return;

            // Read the Shell Result
            var result = FemDesign.Results.FemShell.DeconstructFeaShell(FiniteElement.FemShell);

            // Extract the results from the Dictionary
            var id = (List<string>)result["Identifier"];
            var elementId = (List<int>)result["ElementId"];
            var feaShellFaces = (List<FemDesign.Geometry.Face>)result["Face"];

            // Convert the FDface to Rhino
            var oFeaShellFaces = feaShellFaces.Select(x => x.ToRhino());

            // Read the Node Results
            var nodeData = FemDesign.Results.FemNode.DeconstructFeaNode(FiniteElement.FemNode);

            // Extract the results from the Dictionary
            var nodeId = (List<int>)nodeData["NodeId"];
            var feaNodePoint = (List<FemDesign.Geometry.Point3d>)nodeData["Position"];

            // Create Rhino Mesh
            var rhinoPoint = feaNodePoint.Select(x => x.ToRhino());
            var oMesh = new Rhino.Geometry.Mesh();
            oMesh.Vertices.AddVertices(rhinoPoint);

            foreach (var obj in feaShellFaces)
            {
                if (obj.IsTriangle())
                {
                    oMesh.Faces.AddFace((int)obj.Node1 - 1, (int)obj.Node2 - 1, (int)obj.Node3 - 1);
                }
                else
                {
                    oMesh.Faces.AddFace((int)obj.Node1 - 1, (int)obj.Node2 - 1, (int)obj.Node3 - 1, (int)obj.Node4 - 1);
                }
            }

            var meshExploded = FemDesign.Grasshopper.GeomUtility.ExplodeMesh(oMesh);

            // Set output
            DA.SetDataList("Identifier", id);
            DA.SetDataList("ElementId", elementId);
            DA.SetDataList("FaceIndex", oFeaShellFaces);
            DA.SetDataList("Mesh", meshExploded);
        }

        public override GH_Exposure Exposure => GH_Exposure.hidden;

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return FemDesign.Properties.Resources.FeShell;
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