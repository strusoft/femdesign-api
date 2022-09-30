using System;
using System.Collections.Generic;
using Grasshopper;
using Grasshopper.Kernel;
using System.Linq;
using Rhino.Geometry;
using FemDesign.Results;

namespace FemDesign.Grasshopper
{
    public class PointSupportReactionMinMax : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the PointSupportReaction class.
        /// </summary>
        public PointSupportReactionMinMax()
          : base("PointSupportReactionMinMax",
                "PointSupportReactionMinMax",
                "Read the Max of Load Combination reaction forces",
                CategoryName.Name(), SubCategoryName.Cat7b())
        {

        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Result", "Result", "Result to be Parse", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.Register_StringParam("MinMax", "MinMax", "MinMax.");
            pManager.Register_StringParam("CaseIdentifier", "CaseIdentifier", "CaseIdentifier.");
            pManager.Register_StringParam("Identifier", "Identifier", "Node Identifier.");
            pManager.Register_IntegerParam("NodeId", "NodeId", "Node Index");
            pManager.Register_PointParam("SupportPosition", "SupportPosition", "Position Point for the returned reaction forces");
            pManager.Register_VectorParam("ReactionForce", "ReactionForce", "Reaction Forces in global x, y, z for all nodes.");
            pManager.Register_VectorParam("ReactionMoment", "ReactionMoment", "Reaction Moments in global x, y, z for all nodes.");
            pManager.Register_DoubleParam("ForceResultant", "ForceResultant", "Force Resultant");
            pManager.Register_DoubleParam("MomentResultant", "MomentResultant", "Moment Resultant");
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get indata
            List<FemDesign.Results.PointSupportReactionMinMax> iResult = new List<FemDesign.Results.PointSupportReactionMinMax>();
            DA.GetDataList(0, iResult);

            var max = new List<object>();
            var loadCases = new List<object>();
            var identifier = new List<object>();
            var nodeId = new List<object>();
            var supportPosition = new List<object>();
            var reactionForce = new List<object>();
            var reactionMoment = new List<object>();
            var forceResultant = new List<object>();
            var momentResultant = new List<object>();

            foreach (var result in iResult)
			{
                max.Add(result.Max);
                loadCases.Add(result.CaseIdentifier);
                identifier.Add(result.Id);
                nodeId.Add(result.NodeId);

                var x = result.X;
                var y = result.Y;
                var z = result.Z;
                supportPosition.Add(new Rhino.Geometry.Point3d(x, y, z));

                var fx = result.Fx;
                var fy = result.Fy;
                var fz = result.Fz;
                reactionForce.Add(new Rhino.Geometry.Vector3d(fx, fy, fz));

                var mx = result.Mx;
                var my = result.My;
                var mz = result.Mz;
                reactionMoment.Add(new Rhino.Geometry.Vector3d(mx, my, mz));

                forceResultant.Add(result.Fr);
                momentResultant.Add(result.Mr);
            }

            // Set output
            DA.SetDataList(0, max);
            DA.SetDataList(1, loadCases);
            DA.SetDataList(2, identifier);
            DA.SetDataList(3, nodeId);
            DA.SetDataList(4, supportPosition);
            DA.SetDataList(5, reactionForce);
            DA.SetDataList(6, reactionMoment);
            DA.SetDataList(7, forceResultant);
            DA.SetDataList(8, momentResultant);
        }

        public override GH_Exposure Exposure => GH_Exposure.tertiary;

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return FemDesign.Properties.Resources.results;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{2367BF43-23C4-41A3-8EA5-F8DC5253EB34}"); }
        }
    }
}