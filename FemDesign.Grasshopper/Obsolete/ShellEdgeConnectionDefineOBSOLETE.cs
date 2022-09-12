// https://strusoft.com/
using System;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class ShellEdgeConnectionDefineOBSOLETE : GH_Component
    {
        public ShellEdgeConnectionDefineOBSOLETE() : base("ShellEdgeConnection.Define", "Define", "Define a new ShellEdgeConnection", CategoryName.Name(), SubCategoryName.Cat2b())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Motions", "Motions", "Motions.", GH_ParamAccess.item);
            pManager.AddGenericParameter("Rotations", "Rotations", "Rotations.", GH_ParamAccess.item);

        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("ShellEdgeConnection", "ShellEdgeConnection", "ShellEdgeConnection.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get data
            FemDesign.Releases.Motions motions = null;
            FemDesign.Releases.Rotations rotations = null;
            if (!DA.GetData(0, ref motions)) { return; }
            if (!DA.GetData(1, ref rotations)) { return; }
            if (motions == null || rotations == null) { return; }

            //
            FemDesign.Shells.EdgeConnection obj = new FemDesign.Shells.EdgeConnection(motions, rotations);

            // return
            DA.SetData(0, obj);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.EdgeConnectionDefine;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("c6088f65-a1ca-4c37-9bca-7f5ef3d41e70"); }
        }
        public override GH_Exposure Exposure => GH_Exposure.hidden;

    }
}