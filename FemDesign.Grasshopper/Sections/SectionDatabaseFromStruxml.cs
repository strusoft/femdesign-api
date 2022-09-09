// https://strusoft.com/
using System;
using Grasshopper.Kernel;


namespace FemDesign.Grasshopper
{
    public class SectionDatabaseFromStruxml : GH_Component
    {
        public SectionDatabaseFromStruxml() : base("SectionDatabase.FromStruxml", "FromStruxml", "Load a custom SectionDatabase from a .struxml file.", CategoryName.Name(), SubCategoryName.Cat4b())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("filePath", "filePath", "File path to .struxml file.", GH_ParamAccess.item);
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.Register_GenericParam("Steel Section", "Steel Section", "");
            pManager.Register_GenericParam("Concrete Section", "Concrete Section", "");
            pManager.Register_GenericParam("Timber Section", "Timber Section", "");
            pManager.Register_GenericParam("Hollow CoreSection", "HollowCore Section", "");
            pManager.Register_GenericParam("Custom Section", "Custom Section", "");
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get input
            string filePath = null;
            if (!DA.GetData(0, ref filePath)) { return; }
            if (filePath == null) { return; }

            //
            FemDesign.Sections.SectionDatabase sectionDatabase = FemDesign.Sections.SectionDatabase.DeserializeStruxml(filePath);
            (var steel, var concrete, var timber, var hollowCore, var custom) = sectionDatabase.ByType();

            // set output
            DA.SetDataList(0, steel);
            DA.SetDataList(1, concrete);
            DA.SetDataList(2, timber);
            DA.SetDataList(3, hollowCore);
            DA.SetDataList(4, custom);
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
            get { return new Guid("{17C1C6E9-E68F-4AF1-93BD-133CD5DA4275}"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;

    }
}