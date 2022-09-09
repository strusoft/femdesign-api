// https://strusoft.com/
using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.Grasshopper
{
    public class Units : GH_Component
    {
        public Units() : base("Units", "Units", "Define output Units", CategoryName.Name(), SubCategoryName.Cat7b())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Length", "Length", "Accepted input are: \n " +
                "mm, cm, dm, m, inch, feet, yd", GH_ParamAccess.item, "m");
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddTextParameter("Angle", "Angle", "Accepted input are: \n" +
                "rad, deg", GH_ParamAccess.item, "rad");
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddTextParameter("SectionalData", "SectionalData", "Accepted input are: \n " + 
                "mm, cm, dm, m, inch, feet, yd", GH_ParamAccess.item, "m");
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddTextParameter("Force", "Force", "Accepted input are: \n " +
                "N, daN, kN, MN, lbf, kips", GH_ParamAccess.item, "kN");
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddTextParameter("Mass", "Mass", "Accepted input are: \n " +
                "t, kg, lb, tonUK, tonUS", GH_ParamAccess.item, "kg");
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddTextParameter("Displacement", "Displacement", "Accepted input are: \n " +
                "mm, cm, dm, m, inch, feet, yd", GH_ParamAccess.item, "m");
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddTextParameter("Stress", "Stress", "Accepted input are: \n " +
                "Pa, kPa, MPa, GPa", GH_ParamAccess.item, "Pa");
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Units", "Units", "", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get input
            string length = null;
            DA.GetData(0, ref length);

            string angle = null;
            DA.GetData(1, ref angle);

            string sectionalData = null;
            DA.GetData(2, ref sectionalData);

            string force = null;
            DA.GetData(3, ref force);

            string mass = null;
            DA.GetData(4, ref mass);

            string disp = null;
            DA.GetData(5, ref disp);

            string stress = null;
            DA.GetData(6, ref stress);

            var lengthEnum = (Results.Length) Enum.Parse(typeof(Results.Length), length);
            var angleEnum = (Results.Angle) Enum.Parse(typeof(Results.Angle), angle);
            var secDataEnum = (Results.SectionalData) Enum.Parse(typeof(Results.SectionalData), sectionalData);
            var forceEnum = (Results.Force) Enum.Parse(typeof(Results.Force), force);
            var masshEnum = (Results.Mass) Enum.Parse(typeof(Results.Mass), mass);
            var dispEnum = (Results.Displacement) Enum.Parse(typeof(Results.Displacement), disp);
            var stressEnum = (Results.Stress) Enum.Parse(typeof(Results.Stress), stress);


            var units = new FemDesign.Results.UnitResults(lengthEnum, angleEnum, secDataEnum, forceEnum, masshEnum, dispEnum, stressEnum);
            // output
            DA.SetData(0, units);

        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.Units;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("508DA44B-3D3C-4806-A784-558C4A78E3D7"); }
        }
    }
}