// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.Grasshopper
{
    public class EdgeConnectionRigid: GH_Component
    {
        public EdgeConnectionRigid(): base("EdgeConnection.Rigid", "Rigid", "Create a Rigid EdgeConnection.", CategoryName.Name(), SubCategoryName.Cat2b())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("EdgeConnection", "EdgeConnection", "EdgeConnection.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            //
            FemDesign.Shells.EdgeConnection obj = FemDesign.Shells.EdgeConnection.Rigid;

            // return
            DA.SetData(0, obj);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.EdgeConnectionRigid;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("dc62abe8-a594-4a8d-9a54-8c319e1e1d78"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary;

    }
}