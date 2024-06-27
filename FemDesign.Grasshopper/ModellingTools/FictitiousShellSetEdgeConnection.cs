// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class FictitiousShellSetEdgeConnection: FEM_Design_API_Component
    {
        public FictitiousShellSetEdgeConnection(): base("FictitiousShell.SetEdgeConnection", "SetEdgeConnection", "Set EdgeConnection by index. Index for each respective edge can be extracted using FictitiousShellDeconstruct.", CategoryName.Name(), "ModellingTools")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("FictitiousShell", "FictitiousShell", "FictitiousShell.", GH_ParamAccess.item);
            pManager.AddGenericParameter("EdgeConnection", "EdgeConnection", "EdgeConnection.", GH_ParamAccess.list);
            pManager.AddIntegerParameter("Index", "Index", "Index for edge.", GH_ParamAccess.list);
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("FictitiousShell", "FictitiousShell", "Passed FictitiousShell.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get inputs
            FemDesign.ModellingTools.FictitiousShell fictShell = null;
            if (!DA.GetData(0, ref fictShell)){ return; }
            if (fictShell == null) { return; }

            List<FemDesign.Shells.EdgeConnection> edgeConnections = new List<FemDesign.Shells.EdgeConnection>();
            if (!DA.GetDataList(1, edgeConnections)) { return; }

            List<int> indices = new List<int>();
            if (!DA.GetDataList(2, indices)) { return; }

            // set edge connections on object
            ModellingTools.FictitiousShell obj;
            if (edgeConnections.Count == 0)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, $"No edge connection added to fictious shell {fictShell.Name}");
                return;
            }
            else if (edgeConnections.Count == 1)
            {
                obj = ModellingTools.FictitiousShell.UpdateEdgeConnection(fictShell, edgeConnections[0], indices);
            }
            else if (edgeConnections.Count == indices.Count)
            {
                obj = ModellingTools.FictitiousShell.UpdateEdgeConnection(fictShell, edgeConnections[0], indices[0]);
                for (int i = 0; i < edgeConnections.Count; i++)
                    obj = ModellingTools.FictitiousShell.UpdateEdgeConnection(obj, edgeConnections[i], indices[i]);
            }
            else
            {
                throw new ArgumentException($"The number of EdgeConnections must be 1 or equal to the number of indices provided. Recieved {edgeConnections.Count} and {indices.Count}");
            }

            // get output
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
            get { return new Guid("{0D7CA5E0-B3C4-4EA4-8856-FE4B21E6143C}"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary;

    }
}