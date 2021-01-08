// https://strusoft.com/
using System;
using Grasshopper.Kernel;


namespace FemDesign.GH
{
    public class SectionDatabaseSave: GH_Component
    {
       public SectionDatabaseSave(): base("SectionDatabase.Save", "Save", "Save this SectionDatabase to .struxml.", "FemDesign", "Sections")
       {

       }
       protected override void RegisterInputParams(GH_InputParamManager pManager)
       {
            pManager.AddGenericParameter("SectionDatabase", "SectionDatabase", "SectionDatabase.", GH_ParamAccess.item);
            pManager.AddTextParameter("FilePathStruxml", "FilePath", "File path where to save the section database as .struxml", GH_ParamAccess.item);
       } 
       protected override void RegisterOutputParams(GH_OutputParamManager pManager)
       {

       }
       protected override void SolveInstance(IGH_DataAccess DA)
       {
            Sections.SectionDatabase db = null;
            if (!DA.GetData(0, ref db))
            {
                return;
            }

            string filePath = null;
            if (!DA.GetData(1, ref filePath))
            {
                return;
            }

            // save
            db.SerializeSectionDatabase(filePath);
       }
       protected override System.Drawing.Bitmap Icon
       {
           get
           {
                return FemDesign.Properties.Resources.SectionDatabaseSave;
           }
       }
       public override Guid ComponentGuid
       {
           get { return new Guid("8ca63a70-13dc-40a0-b04f-e633db878a47"); }
       }

    }
}