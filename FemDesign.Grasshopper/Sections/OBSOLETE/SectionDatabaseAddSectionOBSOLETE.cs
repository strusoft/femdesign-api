// https://strusoft.com/
using System;
using Grasshopper.Kernel;


namespace FemDesign.Grasshopper
{
    public class SectionDatabaseAddSectionOBSOLETE: GH_Component
    {
       public SectionDatabaseAddSectionOBSOLETE(): base("SectionDatabase.AddSection", "AddSection", "Add a section to the SectionDatabase.", CategoryName.Name(), SubCategoryName.Cat4b())
       {

       }
       protected override void RegisterInputParams(GH_InputParamManager pManager)
       {
           pManager.AddGenericParameter("SectionDatabase", "SectionDatabase", "SectionDatabase.", GH_ParamAccess.item);
           pManager.AddGenericParameter("Section", "Section", "Section to add.", GH_ParamAccess.item);
       } 
       protected override void RegisterOutputParams(GH_OutputParamManager pManager)
       {
           pManager.AddGenericParameter("SectionDatabase", "SectionDatabase", "SectionDatabase.", GH_ParamAccess.item);
       }
       protected override void SolveInstance(IGH_DataAccess DA)
        {
            Sections.SectionDatabase db = null;
            if (!DA.GetData(0, ref db))
            {
                return;
            }
            
            Sections.Section section = null;
            if (!DA.GetData(1, ref section))
            {
                return;
            }

            // clone section db
            Sections.SectionDatabase obj = db.DeepClone();

            // add section
            obj.AddNewSection(section);

            // return
            DA.SetData(0, obj);
       }
       protected override System.Drawing.Bitmap Icon
       {
           get
           {
                return FemDesign.Properties.Resources.SectionDatabaseAddSection;
           }
       }
       public override Guid ComponentGuid
       {
           get { return new Guid("e85c4f7d-87c8-4021-88dd-4843e4a2ea6b"); }
       }
        public override GH_Exposure Exposure => GH_Exposure.hidden;


    }
}