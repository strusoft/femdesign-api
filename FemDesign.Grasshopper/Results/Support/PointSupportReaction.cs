﻿using System;
using System.Collections.Generic;
using Grasshopper;
using Grasshopper.Kernel;
using System.Linq;
using Rhino.Geometry;
using FemDesign.Results;

namespace FemDesign.Grasshopper
{
    public class PointSupportReaction : FEM_Design_API_Component
    {
        /// <summary>
        /// Initializes a new instance of the PointSupportReaction class.
        /// </summary>
        public PointSupportReaction()
          : base("PointSupportReaction",
                "PointSupportReaction",
                "Read the nodal reaction forces",
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
            pManager.AddTextParameter("CaseIdentifier", "CaseIdentifier", "CaseIdentifier.", GH_ParamAccess.list);
            pManager.AddTextParameter("Identifier", "Identifier", "Node Identifier.", GH_ParamAccess.list);
            pManager.AddIntegerParameter("NodeId", "NodeId", "Node Index", GH_ParamAccess.list);
            pManager.AddPointParameter("SupportPosition", "SupportPosition", "Position Point for the returned reaction forces", GH_ParamAccess.list);
            pManager.AddVectorParameter("ReactionForce", "ReactionForce", "Reaction Forces in global x, y, z for all nodes.", GH_ParamAccess.list);
            pManager.AddVectorParameter("ReactionMoment", "ReactionMoment", "Reaction Moments in global x, y, z for all nodes.", GH_ParamAccess.list);
            pManager.AddNumberParameter("ForceResultant", "ForceResultant", "Force Resultant", GH_ParamAccess.list);
            pManager.AddNumberParameter("MomentResultant", "MomentResultant", "Moment Resultant", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get indata

            List<FemDesign.Results.PointSupportReaction> iResult = new List<FemDesign.Results.PointSupportReaction>();
            DA.GetDataList("Result", iResult);

            string iLoadCase = null;
            DA.GetData(1, ref iLoadCase);

            // Read Result from Abstract Method
            Dictionary<string, object> result;

            try
            {
                result = FemDesign.Results.PointSupportReaction.DeconstructPointSupportReaction(iResult, iLoadCase);
            }
            catch (ArgumentException ex)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, ex.Message);
                return;
            }

            var loadCases = (List<string>)result["CaseIdentifier"];
            var identifier = (List<string>)result["Identifier"];
            var nodeId = (List<int>)result["NodeId"];
            var iPos = (List<FemDesign.Geometry.Point3d>)result["Position"];
            var iReactionForce = (List<FemDesign.Geometry.Vector3d>)result["ReactionForce"];
            var iReactionMoment = (List<FemDesign.Geometry.Vector3d>)result["ReactionMoment"];
            var iForceResultant = (List<double>)result["ForceResultant"];
            var iMomentResultant = (List<double>)result["MomentResultant"];

            // Convert the FdVector to Grasshopper
            var oPos = iPos.Select(x => x.ToRhino());
            var oReactionForce = iReactionForce.Select(x => x.ToRhino());
            var oReactionMoment = iReactionMoment.Select(x => x.ToRhino());


            // Set output
            DA.SetDataList("CaseIdentifier", loadCases);
            DA.SetDataList("Identifier", identifier);
            DA.SetDataList("NodeId", nodeId);
            DA.SetDataList("SupportPosition", oPos);
            DA.SetDataList("ReactionForce", oReactionForce);
            DA.SetDataList("ReactionMoment", oReactionMoment);
            DA.SetDataList("ForceResultant", iForceResultant);
            DA.SetDataList("MomentResultant", iMomentResultant);
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
                return FemDesign.Properties.Resources.Results;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("FABF23CD-30DB-4F95-9EBD-9D42FFD57B37"); }
        }
    }
}