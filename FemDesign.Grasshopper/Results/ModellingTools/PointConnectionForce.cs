using System;
using System.Collections.Generic;
using Grasshopper;
using Grasshopper.Kernel;
using System.Linq;
using Rhino.Geometry;
using FemDesign.Results;

namespace FemDesign.Grasshopper
{
    public class PointConnectionForce : FEM_Design_API_Component
    {
        public PointConnectionForce()
          : base("PointConnectionForce",
                "PointConnectionForce",
                "Read the point connection forces",
                CategoryName.Name(), SubCategoryName.Cat7b())
        {

        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Result", "Result", "Result to be Parse", GH_ParamAccess.list);
            pManager.AddTextParameter("Case/Combination Name", "Case/Comb Name", "Name of Load Case/Load Combination for which to return the results.", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.Register_StringParam("CaseIdentifier", "CaseIdentifier", "CaseIdentifier.");
            pManager.Register_StringParam("Identifier", "Identifier", "Node Identifier.");
            pManager.Register_IntegerParam("NodeId", "NodeId", "Node Index");
            pManager.Register_PointParam("Position", "Position", "Position Point for the returned forces");
            pManager.Register_VectorParam("ReactionForce", "ReactionForce", "Reaction Forces in global x, y, z for all nodes.", GH_ParamAccess.list);
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

            var result = new List<FemDesign.Results.PointConnectionForce>();
            DA.GetDataList("Result", result);

            string iLoadCase = null;
            DA.GetData(1, ref iLoadCase);

            var iResults = result.Where(x => x.CaseIdentifier == iLoadCase);

            var loadCases = iResults.Select(x => x.CaseIdentifier);
            var identifier = iResults.Select(x => x.Id);
            var nodeId = iResults.Select(x => x.NodeId);
            var iPos = iResults.Select(x => x.Pos.ToRhino());
            var iReactionForce = iResults.Select(x => x.Force.ToRhino());
            var iReactionMoment = iResults.Select(x => x.Moment.ToRhino());
            var iForceResultant = iResults.Select(x => x.Fr);
            var iMomentResultant = iResults.Select(x => x.Mr);


            // Set output
            DA.SetDataList("CaseIdentifier", loadCases);
            DA.SetDataList("Identifier", identifier);
            DA.SetDataList("NodeId", nodeId);
            DA.SetDataList("Position", iPos);
            DA.SetDataList("ReactionForce", iReactionForce);
            DA.SetDataList("ReactionMoment", iReactionMoment);
            DA.SetDataList("ForceResultant", iForceResultant);
            DA.SetDataList("MomentResultant", iMomentResultant);
        }

        public override GH_Exposure Exposure => GH_Exposure.tertiary;


        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.Results;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("{F3A1E684-662F-4834-B294-C04FA737C59C}"); }
        }
    }
}