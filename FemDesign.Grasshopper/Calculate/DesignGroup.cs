// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using FemDesign.Calculate;
using System.Linq;

namespace FemDesign.Grasshopper
{
    public class DesignGroup : GH_Component
    {
        public DesignGroup() : base("DesignGroup", "DesignGroup", "", CategoryName.Name(), SubCategoryName.Cat7a())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("GroupName", "GroupName", "", GH_ParamAccess.item);
            pManager.AddGenericParameter("StructuralElements", "StructuralElements", "", GH_ParamAccess.list);
            pManager.AddColourParameter("Color", "Color", "", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;

        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.Register_GenericParam("DesignGroup", "DesignGroup", "");
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string name = null;
            if (!DA.GetData(0, ref name)) { return; }

            List<FemDesign.Bars.Bar> bars = new List<Bars.Bar>();
            if (!DA.GetDataList(1, bars)) { return; }

            System.Drawing.Color? color = null;
            DA.GetData(2, ref color);

            DesignGroupType groupType = (DesignGroupType)Enum.Parse(typeof(DesignGroupType), "SteelBars");

            var designGroup = new Calculate.CmdDesignGroup(name, bars, groupType, color);

            // Set output
            DA.SetData("DesignGroup", designGroup);

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
            get { return new Guid("{7A1D411E-3999-4D28-B681-437C9F70DF9E}"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary;
    }
}