using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Linq;
using System.Collections.Generic;

namespace FemDesign.Reinforcement
{
    public class ReinforcementPtcManufacturingType : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the PtcManufacturingType class.
        /// </summary>
        public ReinforcementPtcManufacturingType() : base("PtcManufacturing", "Manufacturing", "Description", "FEM-Design", "Reinforcement")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Positions", "Positions", "[m]", GH_ParamAccess.list);
            pManager.AddNumberParameter("ShiftX", "ShiftX", "[m]", GH_ParamAccess.item);
            pManager.AddNumberParameter("ShiftZ", "ShiftZ", "[m]", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Manufacturing", "Manufacturing", "FemDesign.Reinforcement.PtcManufacturingType", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<double> positions = new List<double>();
            double shiftX = 0.0;
            double shiftZ = 0.0;
            DA.GetDataList("Positions", positions);
            DA.GetData("ShiftX", ref shiftX);
            DA.GetData("ShiftZ", ref shiftZ);

            var manufacturing = new PtcManufacturingType(positions, shiftX, shiftZ);

            DA.SetData("Manufacturing", manufacturing);
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
                return FemDesign.Properties.Resources.PtcManufacturing;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("6323fbff-e53a-40dd-b368-8d60f04fec3c"); }
        }
        public override GH_Exposure Exposure => GH_Exposure.quarternary;

    }
}