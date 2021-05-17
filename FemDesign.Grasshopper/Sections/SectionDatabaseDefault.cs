// https://strusoft.com/
using System;
using Grasshopper.Kernel;


namespace FemDesign.Grasshopper
{
    public class SectionDatabaseDefault: GH_Component
    {
       public SectionDatabaseDefault(): base("SectionDatabase.Default", "Default", "Load the default SectionDatabase.", "FemDesign", "Sections")
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
                return null;
           }
       }
       public override Guid ComponentGuid
       {
           get { return new Guid("48778d80-3201-4f2d-8414-7b6a2db1f055"); }
       }

    }
}