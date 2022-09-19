// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class FictitiousShellSetEdgeConnection: GH_Component
    {
        public FictitiousShellSetEdgeConnection(): base("FictitiousShell.SetEdgeConnection", "SetEdgeConnection", "Set EdgeConnection by index. Index for each respective edge can be extracted using FictitiousShellDeconstruct.", "FEM-Design", "ModellingTools")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("FictitiousShell", "FictitiousShell", "FictitiousShell.", GH_ParamAccess.item);
            pManager.AddGenericParameter("EdgeConnection", "EdgeConnection", "EdgeConnection.", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Index", "Index", "Index for edge.", GH_ParamAccess.list);
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("FictitiousShell", "FictitiousShell", "Passed FictitiousShell.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get input
            FemDesign.ModellingTools.FictitiousShell fictShell = null;
            FemDesign.Shells.EdgeConnection shellEdgeConnection = null;
            List<int> indices = new List<int>();
            if (!DA.GetData(0, ref fictShell))
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
            if (fictShell == null)
            {
                return;
            }

            //
            FemDesign.ModellingTools.FictitiousShell obj = FemDesign.ModellingTools.FictitiousShell.UpdateEdgeConnection(fictShell, shellEdgeConnection, indices);

            //
            DA.SetData(0, obj);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.SlabSetEdgeConnection;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("41ca29fd-c058-4272-8409-f201ff1496eb"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary;

    }
}