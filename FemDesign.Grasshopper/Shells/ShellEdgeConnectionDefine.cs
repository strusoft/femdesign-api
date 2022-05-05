// https://strusoft.com/
using System;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class ShellEdgeConnectionDefine: GH_Component
    {
        public ShellEdgeConnectionDefine(): base("ShellEdgeConnection.Define", "Define", "Define a new ShellEdgeConnection", "FEM-Design", "Shells")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Motions", "Motions", "Motions.", GH_ParamAccess.item);
            pManager.AddGenericParameter("Rotations", "Rotations", "Rotations.", GH_ParamAccess.item);
            pManager.AddGenericParameter("Plastic Limits Forces Motions", "PlaLimM", "Plastic limits forces for motion springs. Optional.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("Plastic Limits Moments Rotations", "PlaLimR", "Plastic limits moments for rotation springs. Optional.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("ShellEdgeConnection", "ShellEdgeConnection", "ShellEdgeConnection.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Releases.Motions motions = null;
            Releases.Rotations rotations = null;
            Releases.MotionsPlasticLimits motionsPlasticLimit = null;
            Releases.RotationsPlasticLimits rotationsPlasticLimit = null;
            if (!DA.GetData("Motions", ref motions)) return;
            if (!DA.GetData("Rotations", ref rotations)) return;
            if (motions == null || rotations == null) return;
            DA.GetData("Plastic Limits Forces Motions", ref motionsPlasticLimit);
            DA.GetData("Plastic Limits Moments Rotations", ref rotationsPlasticLimit);

            Shells.ShellEdgeConnection edgeConnection = new Shells.ShellEdgeConnection(motions, motionsPlasticLimit, rotations, rotationsPlasticLimit);

            DA.SetData("ShellEdgeConnection", edgeConnection);
        }
        protected override System.Drawing.Bitmap Icon => FemDesign.Properties.Resources.ShellEdgeConnectionDefine;
        public override Guid ComponentGuid => new Guid("c6088f65-a1ca-4c37-9bca-7f5ef3d41e70");
    }
}