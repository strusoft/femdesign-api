using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Linq;
using System.Collections.Generic;

namespace FemDesign.Reinforcement
{
    public class PtcStrand : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the PtcStrand class.
        /// </summary>
        public PtcStrand(): base("PtcStrand", "Strand", "Description", "FemDesign", "Reinforcement")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("name", "name", "", GH_ParamAccess.item);
            pManager.AddNumberParameter("f_pk", "f_pk", "", GH_ParamAccess.item);
            pManager.AddNumberParameter("a_p", "a_p", "", GH_ParamAccess.item);
            pManager.AddNumberParameter("e_p", "e_p", "", GH_ParamAccess.item);
            pManager.AddNumberParameter("density", "density", "", GH_ParamAccess.item);
            pManager.AddIntegerParameter("relaxationClass", "relaxationClass", "", GH_ParamAccess.item);
            pManager.AddNumberParameter("rho_1000", "rho_1000", "", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("PtcStrand", "Strand", "FemDesign.Reinforcement.PtcStrandLibType", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string name = null;
            double f_pk = 0.0;
            double a_p = 0.0;
            double e_p = 0.0;
            double density = 0.0;
            int relaxationClass = 2;
            double rho_1000 = 0.0;
            DA.GetData("name", ref name);
            DA.GetData("f_pk", ref f_pk);
            DA.GetData("a_p", ref a_p);
            DA.GetData("e_p", ref e_p);
            DA.GetData("density", ref density);
            DA.GetData("relaxationClass", ref relaxationClass);
            DA.GetData("rho_1000", ref rho_1000);

            var strand = new PtcStrandLibType(name, f_pk, a_p, e_p, density, relaxationClass, rho_1000);

            DA.SetData("PtcStrand", strand);
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
            get { return new Guid("6323fbff-e53a-40dd-b368-8d60f04fec3d"); }
        }
    }
}