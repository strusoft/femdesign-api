using System;
using System.Collections.Generic;
using Grasshopper;
using Grasshopper.Kernel;
using System.Linq;
using Rhino.Geometry;
using FemDesign.Results;

namespace FemDesign.Grasshopper
{
    public class NodalDisplacement : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public NodalDisplacement()
          : base("Results.NodalDisplacement",
                "NodalDisplacement",
                "Read the nodal displacement for the entire model",
                "FEM-Design",
                "Results")
        {

        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Result", "Result", "Result to be Parse", GH_ParamAccess.list);
            pManager.AddTextParameter("LoadCase", "LoadCase", "Name of Load Case for which to return the results. Default value returns the displacement for the first load case", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("CaseIdentifier", "CaseIdentifier", "CaseIdentifier.", GH_ParamAccess.list);
            pManager.AddIntegerParameter("NodeId", "NodeId", "Node Index", GH_ParamAccess.list);
            pManager.AddVectorParameter("Translation", "Translation", "Nodal translations in global x, y, z for all nodes. [m]", GH_ParamAccess.list);
            pManager.AddVectorParameter("Rotation", "Rotation", "Nodal rotations in global x, y, z for all nodes. [rad]", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get indata

            List<Results.IResult> iResult = new List<Results.IResult>();
            DA.GetDataList("Result", iResult);

            string iLoadCase = null;
            DA.GetData("LoadCase", ref iLoadCase);

            var translation = new List<Rhino.Geometry.Vector3d>();
            var rotation = new List<Rhino.Geometry.Vector3d>();


            // IResult is a List of Interfaces. It needs to be cast to use the object
            var nodalDisplacement = iResult.Cast<FemDesign.Results.NodalDisplacement>();


            // Return the unique load case - load combination
            var uniqueLoadCases = nodalDisplacement.Select(n => n.CaseIdentifier).Distinct().ToList();


            // Select a Default load case if the user does not provide an input
            iLoadCase = iLoadCase == null ? uniqueLoadCases.First() : iLoadCase;


            // Select the Nodal Displacement for the selected Load Case - Load Combination
            if (uniqueLoadCases.Contains(iLoadCase, StringComparer.OrdinalIgnoreCase))
            {
                nodalDisplacement = nodalDisplacement.Where(n => String.Equals(n.CaseIdentifier, iLoadCase, StringComparison.OrdinalIgnoreCase));
            }
            else
            {
                string msg = string.Format("Load Case '{0}' does not exist", iLoadCase);
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, msg);
                return;
            }


            // Parse Results from the object
            var nodeId = nodalDisplacement.Select(n => n.NodeId);
            var loadCases = nodalDisplacement.Select(n => n.CaseIdentifier).Distinct().ToList();
            var transX = nodalDisplacement.Select(n => n.Ex);
            var transY = nodalDisplacement.Select(n => n.Ey);
            var transZ = nodalDisplacement.Select(n => n.Ez);
            var rotationX = nodalDisplacement.Select(n => n.Fix);
            var rotationY = nodalDisplacement.Select(n => n.Fiy);
            var rotationZ = nodalDisplacement.Select(n => n.Fiz);


            // Create a Rhino Vector for Displacement and Rotation
            for (int i = 0; i < nodalDisplacement.Count(); i++)
            {
                translation.Add(new Rhino.Geometry.Vector3d(transX.ElementAt(i), transY.ElementAt(i), transZ.ElementAt(i)));
                rotation.Add(new Rhino.Geometry.Vector3d(rotationX.ElementAt(i), rotationY.ElementAt(i), rotationZ.ElementAt(i)));
            }


            var CaseIdentifier = loadCases;
            var NodeId = nodeId;
            var Translation = translation;
            var Rotation = rotation;


            // Set output
            DA.SetDataList("CaseIdentifier", CaseIdentifier);
            DA.SetDataList("NodeId", nodeId);
            DA.SetDataList("Translation", Translation);
            DA.SetDataList("Rotation", Rotation);
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
            get { return new Guid("4A4FD737-4510-4C00-893E-3DB6814A8F68"); }
        }
    }
}