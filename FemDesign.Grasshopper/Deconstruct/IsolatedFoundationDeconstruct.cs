// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Rhino.Geometry;

namespace FemDesign.Grasshopper
{
    public class IsolatedFoundationDeconstruct : GH_Component
    {
        public IsolatedFoundationDeconstruct() : base("IsolatedFoundation.Deconstruct", "Deconstruct", "Deconstruct an Isolated Foundation element.", "FEM-Design", "Deconstruct")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("IsolatedFoundation", "IsolatedFoundation", "IsolatedFoundation.", GH_ParamAccess.item);
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Guid", "Guid", "Guid.", GH_ParamAccess.item);
            pManager.AddPointParameter("ConnectionPoint", "ConnectionPoint", "", GH_ParamAccess.item);
            pManager.AddSurfaceParameter("Surface", "Surface", "", GH_ParamAccess.item);
            pManager.AddNumberParameter("Thickness", "Thickness", "", GH_ParamAccess.item);
            pManager.AddNumberParameter("Bedding", "Bedding", "", GH_ParamAccess.item);
            pManager.AddGenericParameter("Material", "Material", "Material", GH_ParamAccess.item);
            pManager.AddTextParameter("Analytic", "Analytic", "", GH_ParamAccess.item);
            pManager.AddTextParameter("Name", "Name", "Structural element ID.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get input
            FemDesign.Foundations.IsolatedFoundation foundation = null;
            DA.GetData(0, ref foundation);


            var beddingModulus = foundation.BeddingModulus;
            var material = foundation.ComplexMaterialObj;

            var point = foundation.ConnectionPoint.ToRhino();
            var direction = foundation.Direction.ToRhino();
            var position = new Rhino.Geometry.Plane(point, direction);

            var surface = foundation.ExtrudedSolid.Region.ToRhinoBrep();
            var thickness = foundation.ExtrudedSolid.Thickness;

            var analytic = foundation.FoundationSystem;
            var guid = foundation.Guid;
            var name = foundation.Name;

            DA.SetData("Guid", guid);
            DA.SetData("ConnectionPoint", position);
            DA.SetData("Surface", surface);
            DA.SetData("Thickness", thickness);
            DA.SetData("Bedding", beddingModulus);
            DA.SetData("Material", material);
            DA.SetData("Analytic", analytic);
            DA.SetData("Name", name);

        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.IsolatedFoundationDeconstruct;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("{09872CAB-B4BD-4FFA-B490-237FE42227FC}"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;

    }
}