// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;


namespace FemDesign.Grasshopper
{
    public class SectionDatabaseSave : GH_Component
    {
        public SectionDatabaseSave() : base("SectionDatabase.Save", "Save", "Save these Sections to .struxml.", CategoryName.Name(), SubCategoryName.Cat4b())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Sections", "Sections", "", GH_ParamAccess.list);
            pManager.AddTextParameter("FilePathStruxml", "FilePath", "File path where to save the section database as .struxml", GH_ParamAccess.item);
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {

        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<Sections.Section> sections = new List<Sections.Section>();
            if (!DA.GetDataList(0, sections))
            {
                return;
            }

            string filePath = null;
            if (!DA.GetData(1, ref filePath))
            {
                return;
            }

            // save
            var database = FemDesign.Sections.SectionDatabase.Empty();

            // clone section db
            Sections.SectionDatabase obj = database.DeepClone();

            // add section
            foreach(var section in sections)
                obj.AddNewSection(section);

            obj.SerializeSectionDatabase(filePath);
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
            get { return new Guid("{824CE056-143D-423A-865C-0EDB8B51CDD9}"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.quarternary;

    }
}