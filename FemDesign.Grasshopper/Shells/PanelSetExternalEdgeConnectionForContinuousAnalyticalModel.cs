// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class PanelSetExternalEdgeConnectionForContinuousAnalycitalModel: GH_Component
    {
        public PanelSetExternalEdgeConnectionForContinuousAnalycitalModel(): base("Panel.SetExtEdgeConnectionForContAnalModel", "SetExtEdgeConnectionContAnalModel", "Set EdgeConnection by index on a panel with a continuous analytical model. Index for each respective edge can be extracted using PanelDeconstruct.", CategoryName.Name(), SubCategoryName.Cat2b())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Panel", "Panel", "Panel.", GH_ParamAccess.item);
            pManager.AddGenericParameter("EdgeConnection", "EdgeConnection", "EdgeConnection.", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Index", "Index", "Index for edge.", GH_ParamAccess.list);
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Panel", "Panel", "Passed panel.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            FemDesign.Shells.Panel panel = null;
            if (!DA.GetData(0, ref panel))
            {
                return;
            }

            FemDesign.Shells.EdgeConnection shellEdgeConnection = null;
            if (!DA.GetData(1, ref shellEdgeConnection))
            {
                return;
            }

            List<int> indices = new List<int>();
            if (!DA.GetDataList(2, indices))
            {
                return;
            }
            if (panel == null)
            {
                return;
            }

            // clone
            FemDesign.Shells.Panel panelClone = panel.DeepClone();

            // set edge connections
            panelClone.SetExternalEdgeConnectionsForContinuousAnalyticalModel(shellEdgeConnection, indices);

            //
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
            get { return new Guid("a78149dd-9104-4174-a8a9-5197fed30988"); }
        }
        public override GH_Exposure Exposure => GH_Exposure.secondary;

    }
}