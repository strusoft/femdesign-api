// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;
using FemDesign.GenericClasses;
using FemDesign.Reinforcement;

namespace FemDesign.Grasshopper
{
    public partial class RebarComp : FEM_Design_API_Component
    {
        public RebarComp() : base("Rebar", "Rebar", "Define a single rebar.", "FEM-Design", "Reinforcement")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("Point", "Pos", "Rebar position.", GH_ParamAccess.item);
            pManager.AddGenericParameter("Wire", "Wire", "", GH_ParamAccess.item);
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.Register_GenericParam("Rebar", "Rebar", "");
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Rhino.Geometry.Point3d point = new Rhino.Geometry.Point3d(0,0,0);
            if (!DA.GetData(0, ref point)) return;

            FemDesign.Reinforcement.Wire wire = null;
            DA.GetData("Wire", ref wire);

            var obj = new FemDesign.Grasshopper.Rebar(point.FromRhino(), wire.Diameter, wire.ReinforcingMaterial, wire.Profile);

            DA.SetData(0, obj);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.Rebar;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("{04A59B44-5EE7-4030-A363-E4249390FC3B}"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.senary;
    }
}