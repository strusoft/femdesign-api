using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Linq;
using System.Collections.Generic;
using FemDesign.Reinforcement;

namespace FemDesign.Grasshopper
{
    public class PtcStrand : FEM_Design_API_Component
    {
        /// <summary>
        /// Initializes a new instance of the PtcStrand class.
        /// </summary>
        public PtcStrand(): base("PTC.Strand", "Strand", "Post-tensioning strands.", "FEM-Design", "Reinforcement")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Name", "Name", "Starnd type name.", GH_ParamAccess.item);
            pManager.AddNumberParameter("f pk", "f pk", "Characteristic value of tensile strength [N/mm2].", GH_ParamAccess.item);
            pManager.AddNumberParameter("A p", "A p", "Cross sectional area (nominal value).", GH_ParamAccess.item);
            pManager.AddNumberParameter("E p", "E p", "Modulus of elasticity [N/mm2].", GH_ParamAccess.item);
            pManager.AddNumberParameter("Rho", "Rho", "Density [t/m3]", GH_ParamAccess.item);
            pManager.AddIntegerParameter("RelaxationClass", "RelaxationClass", "Relaxation class. Enter a value between 1 and 3.", GH_ParamAccess.item);
            pManager.AddNumberParameter("Rho 1000", "Rho 1000", "Relaxation at 1000 hour [%].", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("PTC.Strand", "Strand", "Post-tensioning strands.", GH_ParamAccess.item);
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

            if (!DA.GetData(0, ref name)) { return; }
            if (!DA.GetData(1, ref f_pk)) { return; }
            if (!DA.GetData(2, ref a_p)) { return; }
            if (!DA.GetData(3, ref e_p)) { return; }
            if (!DA.GetData(4, ref density)) { return; }
            if (!DA.GetData(5, ref relaxationClass)) { return; }
            if (!DA.GetData(6, ref rho_1000)) { return; }

            if (name == null || f_pk == 0 || a_p == 0 || e_p == 0 || density == 0 || relaxationClass == 0 || rho_1000 == 0) { return; }

            var strand = new PtcStrandLibType(name, f_pk, a_p, e_p, density, relaxationClass, rho_1000);

            DA.SetData(0, strand);
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
                return FemDesign.Properties.Resources.PtcStrand;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("E1E8E8A5-B7BB-428A-BD4D-E642BB760005"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.quarternary;
    }
}