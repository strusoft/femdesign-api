// https://strusoft.com/
using System;
using Grasshopper.Kernel;


namespace FemDesign.Grasshopper
{
    public class SectionDatabaseFromStruxml_OBSOLETE : GH_Component
    {
        public SectionDatabaseFromStruxml_OBSOLETE() : base("SectionDatabase.FromStruxml", "FromStruxml", "Load a custom SectionDatabase from a .struxml file.", CategoryName.Name(), SubCategoryName.Cat4b())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("filePath", "filePath", "File path to .struxml file.", GH_ParamAccess.item);
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("SectionDatabase", "SectionDatabase", "SectionDatabase representation of .struxml file.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get input
            string filePath = null;
            if (!DA.GetData(0, ref filePath)) { return; }
            if (filePath == null) { return; }

            //
            FemDesign.Sections.SectionDatabase sectionDatabase = FemDesign.Sections.SectionDatabase.DeserializeStruxml(filePath);

            // set output
            DA.SetData(0, sectionDatabase);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.SectionDatabaseFromStruxml;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("d1d23586-1a46-4e80-9528-0fcc59416daf"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.hidden;

    }
}