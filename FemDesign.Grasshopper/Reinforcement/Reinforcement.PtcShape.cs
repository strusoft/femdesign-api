using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Linq;
using System.Collections.Generic;

namespace FemDesign.Reinforcement
{
    public class PtcShape : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public PtcShape(): base("PtcShape", "Shape", "Description", "FemDesign", "Reinforcement")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("startZ", "startZ", "", GH_ParamAccess.item);
            pManager.AddNumberParameter("intermediatePos", "intermediatePos", "Positions of intermediate points. Normalized along cable length.", GH_ParamAccess.list);
            pManager.AddNumberParameter("intermediateZ", "intermediateZ", "Z (heights) values of intermediate points", GH_ParamAccess.list);
            pManager.AddNumberParameter("endZ", "endZ", "", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("shape", "shape", "FemDesign.Reinforcement.PtcShapeType", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            double startZ = 0.0;
            if (!DA.GetData("startZ", ref startZ)) return;

            List<double> intermediatePos = new List<double>();
            DA.GetDataList("intermediatePos", intermediatePos);

            List<double> intermediateZ = new List<double>();
            DA.GetDataList("intermediateZ", intermediateZ);

            double endZ = 0.0;
            if (!DA.GetData("endZ", ref endZ)) return;

            if (intermediatePos.Count != intermediateZ.Count)
                throw new ArgumentException("The number of intermediatePos and intermediateZ must be equal!");

            PtcShapeType shape = new PtcShapeType(
                start: new PtcShapeStart() { Z = startZ, Tangent = 0.0 },
                intermediates: intermediatePos.Zip(intermediateZ, (p, z) => 
                    new PtcShapeInner() { 
                        Position = p, Z = z, Tangent = 0.0, 
                    }),
                end: new PtcShapeEnd() { Z = endZ, Tangent = 0.0 }
                );

            DA.SetData("shape", shape);
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
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("6323fbff-e53a-40dd-b368-8d60f04fec3a"); }
        }
    }
}