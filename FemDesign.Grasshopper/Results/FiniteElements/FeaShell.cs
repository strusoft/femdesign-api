using System;
using System.Collections.Generic;
using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;

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
          : base("FdFeaModel.FeaShell", "FeaShell",
              "Deconstruct an Fea Shell in his Part",
              CategoryName.Name(), SubCategoryName.Cat7b())
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("FdFeaModel", "FdFeaModel", "Result to be Parse", GH_ParamAccess.item);
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
            FemDesign.Results.FDfea fdFeaModel = null;
            DA.GetData("FdFeaModel", ref fdFeaModel);
            if (fdFeaModel == null)
                return;

            // Read the Shell Result
            var result = FemDesign.Results.FeaShell.DeconstructFeaShell(fdFeaModel.FeaShell);

            // Extract the results from the Dictionary
            var id = (List<string>)result["Identifier"];
            var elementId = (List<int>)result["ElementId"];
            var feaShellFaces = (List<FemDesign.Geometry.Face>)result["Face"];


            // Read the Node Results
            var nodeData = FemDesign.Results.FeaNode.DeconstructFeaNode(fdFeaModel.FeaNode);
            var nodeId = (List<int>)nodeData["NodeId"];
            var feaNodePoint = (List<FemDesign.Geometry.Point3d>)nodeData["Position"];

            var feaNodedict = new Dictionary<int, Rhino.Geometry.Point3d>();
            for(int i = 0; i < nodeId.Count; i++)
            {
                feaNodedict.Add(nodeId[i], feaNodePoint[i].ToRhino());
            }

            // Convert the FDface to Rhino
            var oFeaShellFaces = feaShellFaces.Select(x => x.ToRhino());

            // Create Rhino Mesh
            var oMesh = new List<Rhino.Geometry.Mesh>();

            foreach (var obj in feaShellFaces)
            {
                var tempMesh = new Rhino.Geometry.Mesh();

                if (obj.IsTriangle())
                {
                    int index1 = (int)obj.Node1;
                    int index2 = (int)obj.Node2;
                    int index3 = (int)obj.Node3;

                    tempMesh.Vertices.Add(feaNodedict[index1]);
                    tempMesh.Vertices.Add(feaNodedict[index2]);
                    tempMesh.Vertices.Add(feaNodedict[index3]);


                    tempMesh.Faces.AddFace(0, 1, 2);
                }
                else
                {
                    int index1 = (int)obj.Node1;
                    int index2 = (int)obj.Node2;
                    int index3 = (int)obj.Node3;
                    int index4 = (int)obj.Node4;

                    tempMesh.Vertices.Add(feaNodedict[index1]);
                    tempMesh.Vertices.Add(feaNodedict[index2]);
                    tempMesh.Vertices.Add(feaNodedict[index3]);
                    tempMesh.Vertices.Add(feaNodedict[index4]);

                    tempMesh.Faces.AddFace(0, 1, 2, 3);
                }

                tempMesh.Normals.ComputeNormals();
                oMesh.Add(tempMesh);
            }


            var indexedNumbers = id.Select((number, index) => new { Value = number, Index = index });
            var groups = indexedNumbers.GroupBy(x => x.Value);

            var idTree = new DataTree<object>();
            var elementIdTree = new DataTree<object>();
            var feaShellFacesTree = new DataTree<object>();
            var meshTree = new DataTree<object>();

            int j = 0;
            foreach (var group in groups)
            {
                idTree.Add(group.Key, new GH_Path(j));
                foreach (var indexedNumber in group)
                {
                    elementIdTree.Add(elementId.ElementAt(indexedNumber.Index), new GH_Path(j));
                    feaShellFacesTree.Add(oFeaShellFaces.ElementAt(indexedNumber.Index), new GH_Path(j));
                    meshTree.Add(oMesh.ElementAt(indexedNumber.Index), new GH_Path(j));
                }
                j++;
            }


            // Set output
            DA.SetDataTree(0, idTree);
            DA.SetDataTree(1, elementIdTree);
            DA.SetDataTree(2, feaShellFacesTree);
            DA.SetDataTree(3, meshTree);
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
                return FemDesign.Properties.Resources.FeShell;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("CC8AAE80-6BAD-423F-A74F-9227F8760521"); }
        }
    }
}