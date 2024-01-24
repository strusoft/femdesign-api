// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;
using FemDesign.Loads;
using FemDesign.Grasshopper.Extension.ComponentExtension;
using Grasshopper.Kernel.Special;

namespace FemDesign.Grasshopper
{
    public class PointLoadDefine_OBSOLETE : FEM_Design_API_Component
    {
        public PointLoadDefine_OBSOLETE() : base("PointLoad.Define", "PointLoad.Define", "Create point load.", CategoryName.Name(), SubCategoryName.Cat3())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("Point", "Point", "Point. [m]", GH_ParamAccess.item);
            pManager.AddVectorParameter("Force", "Force", "Force. [kN]", GH_ParamAccess.item);
            pManager.AddGenericParameter("LoadCase", "LoadCase", "LoadCase.", GH_ParamAccess.item);
            pManager.AddTextParameter("Type", "Type", "Connect 'ValueList' to get the options.\nPoint load type:\nForce\nMoment", GH_ParamAccess.item, "Force");
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddTextParameter("Comment", "Comment", "Comment.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("PointLoad", "PointLoad", "PointLoad.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get data
            Point3d point = Point3d.Origin;
            Vector3d force = Vector3d.Zero;
            string type = "Force";
            FemDesign.Loads.LoadCase loadCase = null;
            string comment = null;
            if (!DA.GetData(0, ref point)) { return; }
            if (!DA.GetData(1, ref force)) { return; }
            if (!DA.GetData(2, ref loadCase)) { return; }
            DA.GetData(3, ref type);
            DA.GetData(4, ref comment);
            if (force == null || loadCase == null) { return; };

            ForceLoadType _type = FemDesign.GenericClasses.EnumParser.Parse<ForceLoadType>(type);

            // Convert geometry
            FemDesign.Geometry.Point3d fdPoint = point.FromRhino();
            FemDesign.Geometry.Vector3d _force = force.FromRhino();



            PointLoad obj = new FemDesign.Loads.PointLoad(fdPoint, _force, loadCase, comment, _type);

            DA.SetData(0, obj);
        }

        protected override void BeforeSolveInstance()
        {
            ValueListUtils.updateValueLists(this, 3, new List<string>
            { "Force", "Moment" }, null, GH_ValueListMode.DropDown);
        }


        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.PointLoadForce;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("{2BDC495D-0C7C-4F26-8C0B-DCFCCFC8CD4D}"); }
        }
        public override GH_Exposure Exposure => GH_Exposure.hidden;

    }
}