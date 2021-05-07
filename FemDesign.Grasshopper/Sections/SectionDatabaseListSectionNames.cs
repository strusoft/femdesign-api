// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;


namespace FemDesign.GH
{
    public class SectionDatabaseListSectionNames: GH_Component
    {
       public SectionDatabaseListSectionNames(): base("SectionDatabase.ListSectionNames", "ListSectionNames", "List the names of all Sections in SectionDatabase.", "FemDesign", "Sections")
       {
       }
       protected override void RegisterInputParams(GH_InputParamManager pManager)
       {
           pManager.AddGenericParameter("SectionDatabase", "SectionDatabase", "SectionDatabase.", GH_ParamAccess.item);
       } 
       protected override void RegisterOutputParams(GH_OutputParamManager pManager)
       {
           pManager.AddTextParameter("SectionNames", "SectionNames", "List of section names.", GH_ParamAccess.list);
       }
       protected override void SolveInstance(IGH_DataAccess DA)
       {
           // get input
            FemDesign.Sections.SectionDatabase sectionDatabase = null;
            if (!DA.GetData(0, ref sectionDatabase)) { return; }
            if (sectionDatabase == null) { return; }

            //
            List<string> sectionNames = sectionDatabase.ListSectionNames();

            // output
            DA.SetDataList(0, sectionNames);

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
           get { return new Guid("d06a89c3-eb5b-437e-837e-1427af3a3dc4"); }
       }
    }
}