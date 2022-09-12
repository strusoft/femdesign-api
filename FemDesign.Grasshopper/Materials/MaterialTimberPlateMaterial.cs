// https://strusoft.com/
using System;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class MaterialTimberPlateMaterial : GH_Component
    {
        public MaterialTimberPlateMaterial() : base("TimberPlateMaterial", "Define", "Define timber factor parameters for a timber panel type.", CategoryName.Name(), SubCategoryName.Cat4a())
        {
            
        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("CltPanelLibraryType", "CltType", "Clt Panel LibraryType.", GH_ParamAccess.item);
            pManager.AddGenericParameter("TimberFactors", "Factors", "Timber Factors", GH_ParamAccess.item);
            pManager.AddBooleanParameter("ShearCoupling", "ShearCoupling", "Consider shear coupling between layers", GH_ParamAccess.item, true);
            pManager.AddBooleanParameter("GluedNarrowSides", "Glued", "Glue at narrow sides", GH_ParamAccess.item, true);
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("TimberPlateMaterial", "TimberPlateMaterial", "Timber Plate Material.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Materials.CltPanelLibraryType cltPanelLibraryType = null;
            if (!DA.GetData("CltPanelLibraryType", ref cltPanelLibraryType)) return;

            Materials.TimberFactors factors = null;
            if (!DA.GetData("TimberFactors", ref factors)) return;

            bool shearCoupling = true, gluedNarrowSides = true;
            DA.GetData("ShearCoupling", ref shearCoupling);
            DA.GetData("GluedNarrowSides", ref gluedNarrowSides);

            FemDesign.Materials.TimberPanelType obj = new Materials.TimberPanelType(cltPanelLibraryType, factors, shearCoupling, gluedNarrowSides);

            DA.SetData(0, obj);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.MaterialTimberPlateMaterial;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("098e6fdd-c621-487d-9246-61ad5586be45"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.quarternary;

    }
}