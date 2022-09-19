// https://strusoft.com/
using System;
using Grasshopper.Kernel;


namespace FemDesign.Grasshopper
{
    public class SectionGetSectionByNameOBSOLETE: GH_Component
    {
        public SectionGetSectionByNameOBSOLETE(): base("Section.GetSectionByName", "GetSectionByName", "Get a Section from a SectionDatabase by name.", CategoryName.Name(), SubCategoryName.Cat4b())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("SectionDatabase", "SectionDatabase", "SectionDatabase.", GH_ParamAccess.item);
            pManager.AddTextParameter("SectionName", "SectionName", "Name of section to retreive.", GH_ParamAccess.item);
        } 
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Section", "Section", "Section.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get input
            FemDesign.Sections.SectionDatabase sectionDatabase = null;
            string sectionName = null;
            if (!DA.GetData(0, ref sectionDatabase)) { return; }
            if (!DA.GetData(1, ref sectionName)) { return; }
            if (sectionDatabase == null || sectionName == null) { return; }

            //
            FemDesign.Sections.Section section = sectionDatabase.SectionByName(sectionName);

            // set output
            DA.SetData(0, section);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.SectionGetSectionByName;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("579e1f74-ff16-4a1d-bf23-1a7b178a0421"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.hidden;

    }
}