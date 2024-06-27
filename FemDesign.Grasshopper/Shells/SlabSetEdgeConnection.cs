// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class SlabSetEdgeConnection: FEM_Design_API_Component
    {
        public SlabSetEdgeConnection(): base("Slab.SetEdgeConnection", "SetEdgeConnection", "Set EdgeConnection by index. Index for each respective edge can be extracted using SlabDeconstruct.", CategoryName.Name(), SubCategoryName.Cat2b())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Slab", "Slab", "Slab.", GH_ParamAccess.item);
            pManager.AddGenericParameter("EdgeConnection", "EdgeConnection", "EdgeConnection or multiple EdgeConnections.", GH_ParamAccess.list);
            pManager.AddIntegerParameter("Index", "Index", "Index of the edge or multiple indices for the edges.", GH_ParamAccess.list);
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Slab", "Slab", "Passed slab.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get inputs
            Shells.Slab slab = null;
            if (!DA.GetData(0, ref slab)) { return; }
            
            
            List<Shells.EdgeConnection> shellEdgeConnections = new List<Shells.EdgeConnection>();
            if (!DA.GetDataList(1, shellEdgeConnections)) { return; }

            List<int> indices = new List<int>();
            if (!DA.GetDataList(2, indices)) { return; }

            // check inputs
            if (slab == null || shellEdgeConnections == null || indices == null) { return; }
            if (shellEdgeConnections.Count == 0)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, $"No edge connection added to shell {slab.Name}");
                return;
            }

            // set edge connections on object
            Shells.Slab obj;
            if (shellEdgeConnections.Count == 1)
            {
                obj = Shells.Slab.SetEdgeConnection(slab, shellEdgeConnections[0], indices);
            }
            else if (shellEdgeConnections.Count == indices.Count)
            {
                obj = Shells.Slab.SetEdgeConnection(slab, shellEdgeConnections[0], indices[0]);
                for (int i = 0; i < shellEdgeConnections.Count; i++)
                    obj = Shells.Slab.SetEdgeConnection(obj, shellEdgeConnections[i], indices[i]);
            }
            else
            {
                throw new ArgumentException($"The number of EdgeConnections must be 1 or equal to the number of indices provided. Recieved {shellEdgeConnections.Count} and {indices.Count}");
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
            get { return new Guid("9108b02c-4aa8-4352-84c6-366f7c42f9bf"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary;

    }
}
