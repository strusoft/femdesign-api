// https://strusoft.com/
using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Special;
using Rhino.Geometry;
using FemDesign.Foundations;
using FemDesign.Grasshopper.Extension.ComponentExtension;

namespace FemDesign.Grasshopper
{
    public class IsolatedFoundation : GH_Component
    {
        public IsolatedFoundation() : base("IsolatedFoundation", "IsolatedFoundation", "Create an IsolatedFoundation element.",
            CategoryName.Name(),
            SubCategoryName.Cat0())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("ExtrudedSolid", "ExtrudedSolid", "ExtrudedSolid.", GH_ParamAccess.item);
            pManager.AddPlaneParameter("ConnectionPoint", "ConnectionPoint", "Reference point which will connect the foundation with other elements. The point must lie on the Surface Geometry. Default is centroid point of surface.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("Material", "Material", "Material must be of type concrete.", GH_ParamAccess.item);
            pManager.AddNumberParameter("Bedding", "Bedding", "Bedding [kN/m2/m]", GH_ParamAccess.item);
            pManager.AddTextParameter("Analytical", "Analytical", "Connect 'ValueList' to get the options.\nSimple\nSurfaceSupportGroup.", GH_ParamAccess.item, "SurfaceSupportGroup");
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("Insulation", "Insulation", "Insulation.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddTextParameter("Identifier", "Identifier", "Identifier. Optional, default value if undefined.", GH_ParamAccess.item, "F");
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Foundation", "Foundation", "Foundation.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Foundations.ExtrudedSolid extrudedSolid = null;
            if (!DA.GetData("ExtrudedSolid", ref extrudedSolid)) { return; }

            Rhino.Geometry.Plane connectionPoint = Rhino.Geometry.Plane.WorldXY;
            if(!DA.GetData("ConnectionPoint", ref connectionPoint) )
            {
                var centroid = Rhino.Geometry.AreaMassProperties.Compute(extrudedSolid.Region.ToRhinoBrep()).Centroid;
                connectionPoint = new Rhino.Geometry.Plane(centroid, Rhino.Geometry.Vector3d.ZAxis);
            };

            Materials.Material material = null;
            if (!DA.GetData("Material", ref material)) { return; }

            double bedding = 0.0;
            if (!DA.GetData("Bedding", ref bedding)) { return; }

            string analytical = "SurfaceSupportGroup";
            DA.GetData("Analytical", ref analytical);

            FoundationSystem _analytical = FemDesign.GenericClasses.EnumParser.Parse<FoundationSystem>(analytical);

            FemDesign.Foundations.Insulation insulation = null;
            DA.GetData("Insulation", ref insulation);

            string identifier = "F";
            DA.GetData("Identifier", ref identifier);


            var foundation = new FemDesign.Foundations.IsolatedFoundation(extrudedSolid, bedding, material, connectionPoint.FromRhinoPlane(), insulation, _analytical, identifier);

            // output
            DA.SetData(0, foundation);

        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.IsolatedFoundation;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("{67E196E5-264D-47EE-B3F9-398639A049F2}"); }
        }
        protected override void BeforeSolveInstance()
        {
            ValueListUtils.updateValueLists(this, 4, new List<string>
            { "Simple", "SurfaceSupportGroup"}, null, GH_ValueListMode.DropDown);

        }

        public override GH_Exposure Exposure => GH_Exposure.primary;

    }
}