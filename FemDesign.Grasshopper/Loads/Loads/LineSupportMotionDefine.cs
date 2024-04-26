// https://strusoft.com/
using System;
using GH = Grasshopper;
using Grasshopper.Kernel;
using Rhino.Geometry;
using FemDesign.Grasshopper.Extension.ComponentExtension;
using Grasshopper.Kernel.Special;
using System.Collections.Generic;
using FemDesign.Loads;
using FemDesign.Results;
using System.Linq;

namespace FemDesign.Grasshopper
{
    public class LineSupportMotionDefine : FEM_Design_API_Component
    {
        public LineSupportMotionDefine() : base("LineSupportMotion.Define", "LineSupportMotion.Define", "Creates a line support motion load.", CategoryName.Name(), SubCategoryName.Cat3())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("Curve", "Curve", "Curve defining the line load.", GH_ParamAccess.item);
            pManager.AddVectorParameter("StartDisplacement", "StartDisplacement", "StartDisplacement. The start displacement will define the direction of the line load. [m]", GH_ParamAccess.item);
            pManager.AddVectorParameter("EndDisplacement", "EndDisplacement", "EndDisplacement. Optional. If undefined LineLoad will be uniform with a displacement of StartDisplacement. [m]", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("LoadCase", "LoadCase", "LoadCase.", GH_ParamAccess.item);
            pManager.AddBooleanParameter("ConstLoadDir", "ConstLoadDir", "Constant load direction? If true direction of load will be constant along action line. If false direction of load will vary along action line - characteristic direction is in the middle point of line. Optional.", GH_ParamAccess.item, true);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddTextParameter("Type", "Type", "Connect 'ValueList' to get the options.\nLine load type:\nMotion\nRotation", GH_ParamAccess.item, "Motion");
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddTextParameter("Comment", "Comment", "Comment.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("LineMotionLoad", "LineLoad", "LineLoad.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Curve curve = null;
            if (!DA.GetData("Curve", ref curve)) return;

            Vector3d startForce = Vector3d.Zero;
            if (!DA.GetData("StartDisplacement", ref startForce)) return;

            Vector3d endForce = Vector3d.Zero;
            if (!DA.GetData("EndDisplacement", ref endForce))
            {
                // if no data set endForce to startForce to create a uniform line load.
                endForce = startForce;
            }

            Loads.LoadCase loadCase = null;
            if (!DA.GetData("LoadCase", ref loadCase)) return;

            bool constLoadDir = true;
            DA.GetData("ConstLoadDir", ref constLoadDir);

            string type = "Motion";
            DA.GetData("Type", ref type);

            string comment = null;
            DA.GetData("Comment", ref comment);

            if (curve == null || startForce == null || endForce == null || loadCase == null) return;

            Geometry.Edge edge = Convert.FromRhinoLineOrArc1(curve);
            Geometry.Vector3d _startForce = startForce.FromRhino();
            Geometry.Vector3d _endForce = endForce.FromRhino();

            SupportMotionType _type = FemDesign.GenericClasses.EnumParser.Parse<SupportMotionType>(type);

            var obj = new Loads.LineSupportMotion(edge, _startForce, _endForce, loadCase, _type, comment, constLoadDir);
            

            DA.SetData(0, obj);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.LineMotion;
            }
        }

        protected override void BeforeSolveInstance()
        {
            ValueListUtils.UpdateValueLists(this, 5, Enum.GetNames(typeof(SupportMotionType)).ToList(), null, GH_ValueListMode.DropDown);
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("{8B465471-57B0-4E74-899A-392F94BB8515}"); }
        }
        public override GH_Exposure Exposure => GH_Exposure.secondary;

    }
}