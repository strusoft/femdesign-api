// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;

using System.Linq;

namespace FemDesign.Grasshopper
{
    public class SectionDatabaseDefault : GH_Component
    {
        public SectionDatabaseDefault() : base("SectionDatabase.Default", "Default", "Load the default SectionDatabase", CategoryName.Name(), SubCategoryName.Cat4b())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
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

            FemDesign.Sections.SectionDatabase sectionDatabase = FemDesign.Sections.SectionDatabase.GetDefault();
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
                return FemDesign.Properties.Resources.SectionDatabaseDefault;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("{6DD34A10-06B3-40ED-8A11-FEFABA2C8B70}"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;
    }
}