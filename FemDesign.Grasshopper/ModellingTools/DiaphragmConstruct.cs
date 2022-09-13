// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.Grasshopper
{
    public class DiaphragmConstruct: GH_Component
    {
        public DiaphragmConstruct(): base("Diaphragm.Construct", "Construct", "Construct a fictitious shell", "FEM-Design", "ModellingTools")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddSurfaceParameter("Surface", "Srf", "Surface.", GH_ParamAccess.item);
            pManager.AddTextParameter("Identifier", "Identifier", "Identifier.", GH_ParamAccess.item, "D");
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Diaphragm", "D", "Diaphragm", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Rhino.Geometry.Brep brep = null;
            if (!DA.GetData("Surface", ref brep))
            {
                return;
            }

            string identifier = "D";
            if (!DA.GetData("Identifier", ref identifier))
            {
                // pass
            }

            // Convert geometry
            Geometry.Region region = brep.FromRhino();

            ModellingTools.Diaphragm obj = new ModellingTools.Diaphragm(region, identifier);
            DA.SetData("Diaphragm", obj);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.Diaphragm;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("d2ea3660-9e30-4a6a-b9de-d4dcd9b2fcaa"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;
    }  
}