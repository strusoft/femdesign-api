// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.GH
{
    public class ShellEdgeConnectionRigid: GH_Component
    {
        public ShellEdgeConnectionRigid(): base("ShellEdgeConnection.Rigid", "Rigid", "Create a Rigid ShellEdgeConnection.", "FemDesign", "Shells")
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
            FemDesign.Shells.ShellEdgeConnection obj = FemDesign.Shells.ShellEdgeConnection.GetRigid();

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
            get { return new Guid("dc62abe8-a594-4a8d-9a54-8c319e1e1d78"); }
        }
    }
}