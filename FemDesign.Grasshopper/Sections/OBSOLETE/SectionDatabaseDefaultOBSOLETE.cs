// https://strusoft.com/
using System;
using Grasshopper.Kernel;


namespace FemDesign.Grasshopper
{
    public class SectionDatabaseDefault_OBSOLETE : GH_Component
    {
        public SectionDatabaseDefault_OBSOLETE() : base("SectionDatabase.Default", "Default", "Load the default SectionDatabase.", CategoryName.Name(), SubCategoryName.Cat4b())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("SectionDatabase", "SectionDatabase", "SectionDatabase.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            //
            FemDesign.Sections.SectionDatabase sectionDatabase = FemDesign.Sections.SectionDatabase.GetDefault();

            // set output
            DA.SetData(0, sectionDatabase);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.SectionDatabaseDefault;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("48778d80-3201-4f2d-8414-7b6a2db1f055"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.hidden;

    }
}