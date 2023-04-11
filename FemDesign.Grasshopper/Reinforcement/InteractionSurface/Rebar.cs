// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;
using FemDesign.GenericClasses;
using FemDesign.Reinforcement;

namespace FemDesign.Grasshopper
{
    public partial class RebarComp : GH_Component
    {
        public RebarComp() : base("Rebar", "Rebar", "", "FEM-Design", "Reinforcement")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("Point", "Pos", "", GH_ParamAccess.item);
            pManager.AddNumberParameter("Diameter", "Diameter", "Diameter of reinforcement bar.", GH_ParamAccess.item);
            pManager.AddGenericParameter("Material", "Material", "Material of reinforcement bar.", GH_ParamAccess.item);
            pManager.AddTextParameter("Profile", "Profile", "Profile of reinforcement bar. Allowed values: smooth/ribbed", GH_ParamAccess.item, "ribbed");
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.Register_GenericParam("Rebar", "Rebar", "");
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Rhino.Geometry.Point3d point = new Rhino.Geometry.Point3d(0,0,0);
            if (!DA.GetData(0, ref point)) return;

            double diameter = 0;
            if (!DA.GetData("Diameter", ref diameter)) return;
            
            FemDesign.Materials.Material material = null;
            if (!DA.GetData("Material", ref material)) return;

            string profile = "ribbed";
            DA.GetData("Profile", ref profile);

            WireProfileType _profile = EnumParser.Parse<WireProfileType>(profile);

            var obj = new FemDesign.Grasshopper.Rebar(point, diameter, material, _profile);

            DA.SetData(0, obj);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return null;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("{04A59B44-5EE7-4030-A363-E4249390FC3B}"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.senary;
    }
}