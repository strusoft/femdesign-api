// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;

using System.Linq;

namespace FemDesign.Grasshopper
{
    public class SectionDatabase : GH_Component
    {
        public SectionDatabase() : base(" SectionDatabase", "SectionDatabase", "SectionDatabase Default or FromStruxml", CategoryName.Name(), SubCategoryName.Cat4b())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("FilePath", "filePath", "File path to .struxml file.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
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
            DA.GetData(0, ref filePath);

            FemDesign.Sections.SectionDatabase sectionDatabase;
            if (filePath == null)
            {
                sectionDatabase = FemDesign.Sections.SectionDatabase.GetDefault();
            }
            else
            {
                sectionDatabase = FemDesign.Sections.SectionDatabase.DeserializeStruxml(filePath);
            }

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
            get { return new Guid("{8CF53197-C368-460C-B0AE-4CB0FAB260F7}"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;
    }
}