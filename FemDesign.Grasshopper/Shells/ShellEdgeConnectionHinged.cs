// https://strusoft.com/
using System;
using Grasshopper.Kernel;

namespace FemDesign.GH
{
    public class ShellEdgeConnectionHinged: GH_Component
    {
        public ShellEdgeConnectionHinged(): base("ShellEdgeConnection.Hinged", "Hinged", "Create a Hinged ShellEdgeConnection.", "FemDesign", "Shells")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("ShellEdgeConnection", "ShellEdgeConnection", "ShellEdgeConnection.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            //
            FemDesign.Shells.ShellEdgeConnection obj = FemDesign.Shells.ShellEdgeConnection.GetHinged();

            // return
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
            get { return new Guid("cedaca89-caa4-40a0-916d-533dc3bf7f8b"); }
        }
    }
}