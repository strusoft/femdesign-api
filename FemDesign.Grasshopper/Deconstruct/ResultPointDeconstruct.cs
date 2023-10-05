// https://strusoft.com/
using System;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class ResultPointDeconstruct : FEM_Design_API_Component
    {
        public ResultPointDeconstruct() : base("ResultPoint.Deconstruct", "Deconstruct", "Deconstruct a ResultPoint.", "FEM-Design", "Deconstruct")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("ResultPoint", "ResultPoint", "", GH_ParamAccess.item);
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddPointParameter("Position", "Position", "", GH_ParamAccess.item);
            pManager.AddTextParameter("BaseEntity", "BaseEntity", "", GH_ParamAccess.item);
            pManager.AddTextParameter("Name", "Name", "Name", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get input
            FemDesign.AuxiliaryResults.ResultPoint obj = null;
            if (!DA.GetData(0, ref obj))
            {
                return;
            }

            // return
            DA.SetData(0, obj.Position.ToRhino());
            DA.SetData(1, obj.BaseEntity);
            DA.SetData(2, obj.Name);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.ResultPointDeconstruct;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("{76E100EF-E6A6-4C46-938D-C70506BADE88}"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary;

    }
}