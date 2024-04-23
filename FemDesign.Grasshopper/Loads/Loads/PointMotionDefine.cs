// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;
using FemDesign.Loads;
using FemDesign.Grasshopper.Extension.ComponentExtension;
using Grasshopper.Kernel.Special;
using System.Linq;

namespace FemDesign.Grasshopper
{
    public class PointMotionDefine : FEM_Design_API_Component
    {
        public PointMotionDefine() : base("PointMotion.Define", "PointMotion.Define", "Create point support motion.", CategoryName.Name(), SubCategoryName.Cat3())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("Point", "Point", "Point. [m]", GH_ParamAccess.item);
            pManager.AddVectorParameter("Displacement", "Displacement", "Displacement. [m]", GH_ParamAccess.item);
            pManager.AddGenericParameter("LoadCase", "LoadCase", "LoadCase.", GH_ParamAccess.item);
            pManager.AddTextParameter("Type", "Type", "Connect 'ValueList' to get the options.\nPoint load type:\nMotion\nRotation", GH_ParamAccess.item, "Motion");
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddTextParameter("Comment", "Comment", "Comment.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("PointMotion", "PointMotion", "PointMotion.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get data
            Point3d point = Point3d.Origin;
            if (!DA.GetData(0, ref point)) { return; }

            Vector3d displacement = Vector3d.Zero;
            if (!DA.GetData(1, ref displacement)) { return; }

            Loads.LoadCase loadCase = null;
            if (!DA.GetData(2, ref loadCase)) { return; }

            string type = "Motion";
            DA.GetData(3, ref type);

            string comment = null;
            DA.GetData(4, ref comment);



            SupportMotionType _type = FemDesign.GenericClasses.EnumParser.Parse<SupportMotionType>(type);

            // Convert geometry
            FemDesign.Geometry.Point3d fdPoint = point.FromRhino();
            FemDesign.Geometry.Vector3d _force = displacement.FromRhino();

            var obj = new FemDesign.Loads.PointMotion(fdPoint, _force, loadCase, comment, _type);

            DA.SetData(0, obj);
        }

        protected override void BeforeSolveInstance()
        {
            ValueListUtils.UpdateValueLists(this, 3, Enum.GetNames(typeof(SupportMotionType)).ToList());
        }


        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.PointMotion;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("{B896C087-CBC9-42BC-9AE5-62C3B1BF6E7F}"); }
        }
        public override GH_Exposure Exposure => GH_Exposure.primary;

    }
}