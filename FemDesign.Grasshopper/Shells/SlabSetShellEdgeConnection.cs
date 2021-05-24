// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class SlabSetShellEdgeConnection: GH_Component
    {
        public SlabSetShellEdgeConnection(): base("Slab.SetShellEdgeConnection", "SetEdgeConnection", "Set ShellEdgeConnection by index. Index for each respective edge can be extracted using SlabDeconstruct.", "FemDesign", "Shells")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Slab", "Slab", "Slab.", GH_ParamAccess.item);
            pManager.AddGenericParameter("ShellEdgeConnection", "ShellEdgeConnection", "ShellEdgeConnection.", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Index", "Index", "Index for edge.", GH_ParamAccess.list);
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Slab", "Slab", "Passed slab.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get input
            FemDesign.Shells.Slab slab = null;
            FemDesign.Shells.ShellEdgeConnection shellEdgeConnection = null;
            List<int> indices = new List<int>();
            if (!DA.GetData(0, ref slab))
            {
                return;
            }
            if (!DA.GetData(1, ref shellEdgeConnection))
            {
                return;
            }
            if (!DA.GetDataList(2, indices))
            {
                return;
            }
            if (slab == null)
            {
                return;
            }

            //
            FemDesign.Shells.Slab obj = FemDesign.Shells.Slab.ShellEdgeConnection(slab, shellEdgeConnection, indices);

            //
            DA.SetData(0, obj);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.SlabSetShellEdgeConnection;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("9108b02c-4aa8-4352-84c6-366f7c42f9bf"); }
        }
    }
}