// https://strusoft.com/
using System;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class ShellOrthotropyConstruct: GH_Component
    {
        public ShellOrthotropyConstruct(): base("ShellOrthotropy.Construct", "Construct", "Construct a definition for ShellOrthotropy.", CategoryName.Name(), SubCategoryName.Cat2b())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("orthoAlfa", "orthoAlfa", "Alpha in degrees.", GH_ParamAccess.item, 0);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("orthoRatio", "orthoRatio", "E2/E1", GH_ParamAccess.item, 1);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("ShellOrthotropy", "ShellOrthotropy", "ShellOrthotropy", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get input
            double orthoAlpha = 0;
            double orthoRatio = 1;
            if (!DA.GetData(0, ref orthoAlpha))
            {
                // pass
            }
            if (!DA.GetData(1, ref orthoRatio))
            {
                // pass
            }

            //
            FemDesign.Shells.ShellOrthotropy obj = new FemDesign.Shells.ShellOrthotropy(orthoAlpha, orthoRatio);

            // return
            DA.SetData(0, obj);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.ShellOrthotropyDefine;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("fac54948-cf89-406d-87a0-05aa62791aad"); }
        }
        public override GH_Exposure Exposure => GH_Exposure.tertiary;

    }
}