// https://strusoft.com/
using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.Grasshopper
{
    public class LineConnectionDeconstruct_OBSOLETE : FEM_Design_API_Component
    {
        public LineConnectionDeconstruct_OBSOLETE() : base("LineConnection.Deconstruct", "Deconstruct", "Deconstruct a LineConnection.", CategoryName.Name(), "Deconstruct")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("LineConnection", "LnConnect", "LineConnection from ModellingTools.", GH_ParamAccess.item);
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Guid", "Guid", "Guid.", GH_ParamAccess.item);
            pManager.AddGenericParameter("ConnectedElementsReference", "Ref", "GUIDs of connected structural elements (e.g. slabs, surface supports, fictious shells, etc).", GH_ParamAccess.list);
            pManager.AddLineParameter("Lines", "Lns", "Master line and slave line.", GH_ParamAccess.list);
            pManager.AddGenericParameter("Motion", "Mot", "Motion release.", GH_ParamAccess.item);
            pManager.AddGenericParameter("MotionsPlasticLimits", "PlaLimM", "Plastic limits forces for motion springs.", GH_ParamAccess.item);
            pManager.AddGenericParameter("Rotation", "Rot", "Rotation release.", GH_ParamAccess.item);
            pManager.AddGenericParameter("RotationsPlasticLimits", "PlaLimR", "Plastic limits moments for rotation springs.", GH_ParamAccess.item);
            pManager.AddVectorParameter("LocalX", "X", "Local x-axis.", GH_ParamAccess.item);
            pManager.AddVectorParameter("LocalY", "Y", "Local y-axis.", GH_ParamAccess.item);
            pManager.AddTextParameter("Identifier", "ID", "Identifier.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get input
            FemDesign.ModellingTools.ConnectedLines obj = null;
            if (!DA.GetData(0, ref obj)) { return; }
            if (obj == null) { return; }

            var rhinoLines = obj.Edges.Select(l => l.ToRhino()).ToList();

            // get output
            DA.SetData(0, obj.Guid);
            DA.SetDataList(1, obj.References);
            DA.SetDataList(2, rhinoLines);
            DA.SetData(3, obj.Rigidity.Motions);
            DA.SetData(4, obj.Rigidity.PlasticLimitForces);
            DA.SetData(5, obj.Rigidity.Rotations);
            DA.SetData(6, obj.Rigidity.PlasticLimitMoments);
            DA.SetData(7, obj.LocalX.ToRhino());
            DA.SetData(8, obj.LocalY.ToRhino());
            DA.SetData(9, obj.Identifier);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.LineConnectionDeconstruct;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("{B4480216-0C2E-4B63-AA52-3764992D3886}"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.hidden;

    }
}