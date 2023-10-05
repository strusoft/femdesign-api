// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using System.Linq;
using FemDesign.Grasshopper.Extension.ComponentExtension;
using Grasshopper.Kernel.Special;
using System.ComponentModel;
using FemDesign.Sections;

namespace FemDesign.Grasshopper
{
    public class SectionsByFamily : FEM_Design_API_Component
    {
        public SectionsByFamily() : base("Section.ByShape", ".ByShape", "Get a Section from a SectionDatabase by shape.", CategoryName.Name(), SubCategoryName.Cat4b())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Sections", "Sections", "", GH_ParamAccess.list);
            pManager.AddTextParameter("Family", "Family", "Family of sections to retreive. Connect 'ValueList' to get the options.", GH_ParamAccess.item);
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Sections", "Sections", "Sections.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var sections = new List<Sections.Section>();
            DA.GetDataList(0, sections);

            string family = null;
            DA.GetData(1, ref family);


            var fam = (Sections.Family)Enum.Parse(typeof(Sections.Family), family);
             
            var sectionsByShape = sections.FilterSectionsByFamily(fam);

            DA.SetDataList(0, sectionsByShape);
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
            get { return new Guid("{816DB12F-9B29-4FBE-A5E9-375DC8AF3658}"); }
        }

        protected override void BeforeSolveInstance()
        {
            ValueListUtils.updateValueLists(this, 1, Enum.GetNames( typeof(FemDesign.Sections.Family)).ToList(), null, GH_ValueListMode.DropDown);
        }


        public override GH_Exposure Exposure => GH_Exposure.primary;

    }
}