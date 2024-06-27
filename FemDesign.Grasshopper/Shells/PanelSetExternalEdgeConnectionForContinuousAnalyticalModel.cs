// https://strusoft.com/
using System;
using System.Collections.Generic;
using FemDesign.Shells;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class PanelSetExternalEdgeConnectionForContinuousAnalycitalModel: FEM_Design_API_Component
    {
        public PanelSetExternalEdgeConnectionForContinuousAnalycitalModel(): base("Panel.SetExtEdgeConnectionForContAnalModel", "SetExtEdgeConnectionContAnalModel", "Set EdgeConnection by index on a panel with a continuous analytical model. Index for each respective edge can be extracted using PanelDeconstruct.", CategoryName.Name(), SubCategoryName.Cat2b())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Panel", "Panel", "Panel.", GH_ParamAccess.item);
            pManager.AddGenericParameter("EdgeConnection", "EdgeConnection", "EdgeConnection.", GH_ParamAccess.list);
            pManager.AddIntegerParameter("Index", "Index", "Index for edge.", GH_ParamAccess.list);
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Panel", "Panel", "Passed panel.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get inputs
            FemDesign.Shells.Panel panel = null;
            if (!DA.GetData(0, ref panel)) { return; }

            List<FemDesign.Shells.EdgeConnection> edgeConnections = new List<EdgeConnection>();
            if (!DA.GetDataList(1, edgeConnections)) { return; }

            List<int> indices = new List<int>();
            if (!DA.GetDataList(2, indices)) { return; }

            // check inputs
            if (panel == null || edgeConnections == null || indices == null) { return; }
            if (edgeConnections.Count == 0)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, $"No edge connection added to panel {panel.Name}");
                return;
            }

            // clone
            FemDesign.Shells.Panel panelClone = panel.DeepClone();

            // set edge connections
            if(edgeConnections.Count == 1)
            {
                panelClone.SetExternalEdgeConnectionsForContinuousAnalyticalModel(edgeConnections[0], indices);
            }
            else if (edgeConnections.Count == indices.Count)
            {
                for (int i = 0; i < edgeConnections.Count; i++)
                    panelClone.SetExternalEdgeConnectionAtIndexForContinousAnalyticalModel(edgeConnections[i], indices[i]);
            }
            else
            {
                throw new ArgumentException($"The number of EdgeConnections must be 1 or equal to the number of indices provided. Recieved {edgeConnections.Count} and {indices.Count}");
            }

            // set output
            DA.SetData(0, panelClone);
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
            get { return new Guid("{BAB2DB6B-818D-40ED-9D62-E716C97A20C2}"); }
        }
        public override GH_Exposure Exposure => GH_Exposure.secondary;

    }
}