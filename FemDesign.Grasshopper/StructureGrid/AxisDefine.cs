// https://strusoft.com/
using System;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class AxisDefine: GH_Component
    {
        public AxisDefine(): base("Axis.Define", "Define", "Define an Axis.", "FEM-Design", "ModellingTools")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("Line", "Line", "Line of axis. Line will be projected onto XY-plane.", GH_ParamAccess.item);
            pManager.AddTextParameter("Prefix", "Prefix", "Prefix of axis identifier.", GH_ParamAccess.item, "");
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddIntegerParameter("Id", "Id", "Number of axis identifier. Number can be converted to letter.", GH_ParamAccess.item);
            pManager.AddBooleanParameter("IdIsLetter", "IdIsLetter", "Is identifier a letter?", GH_ParamAccess.item, false);
            pManager[pManager.ParamCount - 1].Optional = false;

        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Axis", "Axis", "Axis.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get data
            Rhino.Geometry.Curve line = null;
            if (!DA.GetData(0, ref line))
            {
                return;
            }
            string prefix = "";
            if (!DA.GetData(1, ref prefix))
            {
                // pass
            }
            int id = 0;
            if (!DA.GetData(2, ref id))
            {
                return;
            }
            bool idIsLetter = false;
            if (!DA.GetData(3, ref idIsLetter))
            {
                // pass
            }
            
            // test nullable input
            if (prefix == null)
            {
                return;
            }

            // convert geometry
            if (line.GetType() != typeof(Rhino.Geometry.LineCurve))
            {
                throw new System.ArgumentException("Curve must be a LineCurve");
            }
            FemDesign.Geometry.Point3d p0 = Convert.FromRhino(line.PointAtStart);
            FemDesign.Geometry.Point3d p1 = Convert.FromRhino(line.PointAtEnd);

            //
            FemDesign.StructureGrid.Axis obj = new StructureGrid.Axis(p0, p1, prefix, id, idIsLetter);
            DA.SetData(0, obj); 
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
               return FemDesign.Properties.Resources.AxisDefine;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("09dd7852-f4c4-43b6-b5e6-242bc2da5794"); }
        }
        public override GH_Exposure Exposure => GH_Exposure.quarternary;

    }
}