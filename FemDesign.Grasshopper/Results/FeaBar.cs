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
    public class FeaBar : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the FeaBar class.
        /// </summary>
        public FeaBar()
          : base("FdFeaModel.FeaBar", "FeaBar",
              "Deconstruct an Fea Bar in his Part",
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
            pManager.Register_IntegerParam("Node-i", "Node-i", "Node Indexes as per FEM Design Model. FEM Design start counting from 1!");
            pManager.Register_IntegerParam("Node-j", "Node-j", "Node Indexes as per FEM Design Model. FEM Design start counting from 1!");

            pManager.Register_LineParam("Line", "Line Geometry", "Line Geometry as per Fem Design Model");
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
            var result = FemDesign.Results.FeaBar.DeconstructFeaBar(fdFeaModel.FeaBar);

            // Extract the results from the Dictionary
            var id = (List<string>)result["Identifier"];
            var elementId = (List<int>)result["ElementId"];
            var nodei = (List<int>)result["Nodei"];
            var nodej = (List<int>)result["Nodej"];

            // clean Identifier List
            // some of the elements are null
            int i = 0;
            int j = 0;
            string currentId = null;
            var uniqueId = id.Where(x => x != "").ToList();
            var identifier = new List<string>();

            for (i = 0; i < id.Count(); i++)
            {
                if (id.ElementAt(i) != "")
                {
                    identifier.Add(uniqueId.ElementAt(j));
                    currentId = uniqueId.ElementAt(j);
                    j++;
                }
                else
                {
                    identifier.Add(currentId);
                }
            }


            var lines = new List<Rhino.Geometry.LineCurve>();

            for (i = 0; i < nodei.Count(); i++)
            {
                var posStartX = fdFeaModel.FeaNode.ElementAt(nodei.ElementAt(i) - 1 ).X;
                var posStartY = fdFeaModel.FeaNode.ElementAt(nodei.ElementAt(i) - 1).Y;
                var posStartZ = fdFeaModel.FeaNode.ElementAt(nodei.ElementAt(i) - 1).Z;
                var pointi = new Rhino.Geometry.Point3d(posStartX, posStartY, posStartZ);

                var posEndX = fdFeaModel.FeaNode.ElementAt(nodej.ElementAt(i) - 1).X;
                var posEndY = fdFeaModel.FeaNode.ElementAt(nodej.ElementAt(i) - 1).Y;
                var posEndZ = fdFeaModel.FeaNode.ElementAt(nodej.ElementAt(i) - 1).Z;
                var pointj = new Rhino.Geometry.Point3d(posEndX, posEndY, posEndZ);

                var line = new Rhino.Geometry.LineCurve(pointi, pointj);
                lines.Add(line);
            }

            var identifierTree = new DataTree<object>();
            var nodeiTree = new DataTree<object>();
            var nodejTree = new DataTree<object>();
            var elementIdTree = new DataTree<object>();
            var lineTree = new DataTree<object>();

            i = 0;
            foreach (var ids in uniqueId)
            {
                identifierTree.Add(ids);

                var indexes = identifier.Select((value, index) => new { value, index })
                  .Where(a => string.Equals(a.value, ids))
                  .Select(a => a.index);
                foreach (int index in indexes)
                {
                    //loadCasesTree.Add(loadCases.ElementAt(index), new GH_Path(i));
                    nodeiTree.Add(nodei.ElementAt(index), new GH_Path(i));
                    nodejTree.Add(nodej.ElementAt(index), new GH_Path(i));
                    elementIdTree.Add(elementId.ElementAt(index), new GH_Path(i));
                    lineTree.Add(lines.ElementAt(index), new GH_Path(i));
                }
                i++;
            }
        

            // Set output
            DA.SetDataTree(0, identifierTree);
            DA.SetDataTree(1, elementIdTree);
            DA.SetDataTree(2, nodeiTree);
            DA.SetDataTree(3, nodejTree);

            DA.SetDataTree(4, lineTree);
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
                return FemDesign.Properties.Resources.feabar;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("65572272-DFA3-4A8D-9237-C87CB94629A7"); }
        }
    }
}

