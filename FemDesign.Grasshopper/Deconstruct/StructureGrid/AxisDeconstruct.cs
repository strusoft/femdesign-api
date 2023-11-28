// https://strusoft.com/
using System;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class AxisDeconstruct : FEM_Design_API_Component
    {
        public AxisDeconstruct() : base("Axis.Deconstruct", "Deconstruct", "Deconstruct an axis element.", "FEM-Design", "Deconstruct")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Axis", "Axis", "Axis.", GH_ParamAccess.item);
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Guid", "Guid", "Guid.", GH_ParamAccess.item);
            pManager.AddCurveParameter("Line", "Line", "Line. [m]", GH_ParamAccess.item);
            pManager.AddGenericParameter("Number|Letter", "Number|Letter", "Number|Letter", GH_ParamAccess.item);
            pManager.AddTextParameter("Prefix", "Prefix", "Prefix.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get input
            FemDesign.StructureGrid.Axis obj = null;
            if (!DA.GetData(0, ref obj))
            {
                return;
            }
            if (obj == null)
            {
                return;
            }

            // return
            DA.SetData(0, obj.Guid);
            DA.SetData(1, new Rhino.Geometry.LineCurve(obj.StartPoint.ToRhino(), obj.EndPoint.ToRhino()));

            if(obj.Letter == null)
                DA.SetData(2, obj.Number);
            else
                DA.SetData(2, obj.Letter);

            DA.SetData(3, obj.Prefix);


        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.AxisDeconstruct;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("{424167BA-C94F-4240-8431-F28584AB49BE}"); }
        }
        public override GH_Exposure Exposure => GH_Exposure.senary;

    }
}