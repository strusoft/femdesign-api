// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;


namespace FemDesign.Grasshopper
{
    public class SectionGetSectionByName : GH_Component
    {
        public SectionGetSectionByName() : base("Section.GetSectionByName|Index", "GetSectionByName|Index", "Get a Section from a SectionDatabase by ByName or Index.", CategoryName.Name(), SubCategoryName.Cat4())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Sections", "Sections", "", GH_ParamAccess.list);
            pManager.AddTextParameter("SectionName|Index", "SectionName|Index", "Name of section to retreive or positional index in the list..", GH_ParamAccess.item);
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Section", "Section", "Section.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var sections = new List<Sections.Section>();
            DA.GetDataList(0, sections);

            dynamic sectionInput = null;
            DA.GetData(1, ref sectionInput);

            sectionInput = sectionInput.Value;

            var section = Sections.Section.GetSectionByNameOrIndex(sections, sectionInput);

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
            get { return new Guid("{0561A068-DBBA-451C-B65B-299D90250144}"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary;

    }
}