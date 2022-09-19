// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.Grasshopper
{
    public class PanelContinuousAnalyticalModelDeconstruct : GH_Component
    {
        public PanelContinuousAnalyticalModelDeconstruct() : base("Panel.Deconstruct", "Deconstruct", "Deconstruct a panel of continuous analytical model element.", "FEM-Design", "Deconstruct")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Panel", "Panel", "Panel", GH_ParamAccess.item);
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Guid", "Guid", "Guid.", GH_ParamAccess.item);
            pManager.AddBrepParameter("ExtSurface", "ExtSurface", "ExtSurface", GH_ParamAccess.item);
            pManager.AddGenericParameter("Material", "Material", "Material", GH_ParamAccess.item);
            pManager.AddGenericParameter("Section", "Section", "Section", GH_ParamAccess.item);
            pManager.AddCurveParameter("ExtEdgeCurves", "ExtEdgeCurves", "ExtEdgeCurves", GH_ParamAccess.list);
            pManager.AddGenericParameter("ExtEdgeConnections", "ExtEdgeConnections", "ExtEdgeConnections", GH_ParamAccess.list);
            pManager.AddVectorParameter("LocalX", "LocalX", "LocalX", GH_ParamAccess.item);
            pManager.AddVectorParameter("LocalY", "LocalY", "LocalY", GH_ParamAccess.item);
            pManager.AddTextParameter("Identifier", "Identifier", "Structural element ID.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get input
            FemDesign.Shells.Panel panel = null;
            if (!DA.GetData(0, ref panel))
            {
                return;
            }
            if (panel == null)
            {
                return;
            }

            if (panel.InternalPanels.IntPanels.Count != 1)
            {
                throw new System.ArgumentException("Panel has more than 1 internal panel. Panel analytical model is not of type continuous.");
            }

            // Get Material (concrete/steel material) or TimberPanelData (timber material)
            Materials.IMaterial material = null;
            if (panel.Material != null)
                material = panel.Material;
            else if (panel.TimberPanelData != null)
                material = panel.TimberPanelData;

            DA.SetData("Guid", panel.Guid);
            DA.SetData("ExtSurface", panel.InternalPanels.IntPanels[0].Region.ToRhinoBrep());
            DA.SetData("Material", material);
            DA.SetData("Section", panel.Section);
            DA.SetDataList("ExtEdgeCurves", panel.InternalPanels.IntPanels[0].Region.ToRhinoCurves());
            DA.SetDataList("ExtEdgeConnections", panel.InternalPanels.IntPanels[0].Region.GetEdgeConnections());
            DA.SetData("LocalX", panel.LocalX.ToRhino());
            DA.SetData("LocalY", panel.LocalY.ToRhino());
            DA.SetData("Identifier", panel.Name);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.PanelDeconstruct;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("2aef7b29-a024-4591-98c9-5bbce2acb954"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;

    }
}