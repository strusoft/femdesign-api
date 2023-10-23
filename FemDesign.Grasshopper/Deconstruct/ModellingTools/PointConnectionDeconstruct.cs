// https://strusoft.com/
using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.Grasshopper
{
    public class PointConnectionDeconstruct : FEM_Design_API_Component
    {
        public PointConnectionDeconstruct() : base("PointConnection.Deconstruct", "Deconstruct", "Deconstruct a PointConnection.", CategoryName.Name(), "Deconstruct")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("PointConnection", "PtConnect", "PointConnection from ModellingTools.", GH_ParamAccess.item);
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Guid", "Guid", "Guid.", GH_ParamAccess.item);
            pManager.AddGenericParameter("ConnectedElementsReference", "Ref", "GUIDs of connected structural elements (e.g. slabs, surface supports, fictious shells, etc).", GH_ParamAccess.list);
            pManager.AddPointParameter("Points", "Pts", "Master point and slave point.", GH_ParamAccess.list);
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
            FemDesign.ModellingTools.ConnectedPoints obj = null;
            if(!DA.GetData(0, ref obj)) { return; }
            if(obj == null) { return; }

            var rhinoPoints = obj.Points.Select(p => p.ToRhino()).ToList();

            // get output
            DA.SetData(0, obj.Guid);
            DA.SetDataList(1, obj.References);
            DA.SetDataList(2, rhinoPoints);
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
                return FemDesign.Properties.Resources.PointConnectionDeconstruct;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("{49D35435-806E-4E21-90CF-B13729388D0C}"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.senary;

    }
}