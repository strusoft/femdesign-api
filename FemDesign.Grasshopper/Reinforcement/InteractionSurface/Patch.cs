// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;
using FemDesign.GenericClasses;
using FemDesign.Reinforcement;

namespace FemDesign.Grasshopper
{
    public partial class PatchComp : GH_Component
    {
        public PatchComp() : base("Patch", "Patch", "Define the boundary of the structural section", "FEM-Design", "Reinforcement")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddBrepParameter("Surface", "Srf", "", GH_ParamAccess.item);
            pManager.AddGenericParameter("Material", "Material", "Section material: Only concrete material can be specified", GH_ParamAccess.item);
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.Register_GenericParam("Patch", "Patch", "");
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Rhino.Geometry.Brep srf = null;
            if (!DA.GetData(0, ref srf)) return;

            foreach (var _srf in srf.Surfaces)
            {
                if (_srf.IsPlanar() == false)
                    throw new Exception("Surface must be planar!");
            }

            FemDesign.Materials.Material material = null;
            if (!DA.GetData("Material", ref material)) return;

            var obj = new FemDesign.Grasshopper.Patch(srf, material);

            DA.SetData(0, obj);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.Patch;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("{2264144C-145D-4F2D-8677-F9CFF323C81C}"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.senary;
    }
}