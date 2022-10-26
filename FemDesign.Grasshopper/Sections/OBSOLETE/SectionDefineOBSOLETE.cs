// https://strusoft.com/
using System;
using System.Linq;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Special;

using FemDesign.Grasshopper.Extension.ComponentExtension;

namespace FemDesign.Grasshopper
{
    public class SectionDefine_OBSOLETE : GH_Component
    {
        public SectionDefine_OBSOLETE() : base("Section.Define", "Define", "Define a new custom section.", CategoryName.Name(), SubCategoryName.Cat4b())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddSurfaceParameter("Surfaces", "Srfs", "Item or list of surfaces of section. Surfaces must lie in the XY-plane at z=0.", GH_ParamAccess.list);
            pManager.AddTextParameter("Name", "Name", "Name of section", GH_ParamAccess.item);
            pManager.AddTextParameter("MaterialType", "MatType", "Connect 'ValueList' to get the options.\nSteelRolled\nSteelColdWorked\nSteelWelded\nConcrete\nTimber\nUnknown\nUndefined", GH_ParamAccess.item);
            pManager.AddTextParameter("GroupName", "GroupName", "Name of section group", GH_ParamAccess.item);
            pManager.AddTextParameter("TypeName", "TypeName", "Name of section type", GH_ParamAccess.item);
            pManager.AddTextParameter("SizeName", "SizeName", "Name of section size", GH_ParamAccess.item);
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Custom Section", "Custom Section", "CustomSection.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<Rhino.Geometry.Brep> breps = new List<Rhino.Geometry.Brep>();
            if (!DA.GetDataList(0, breps))
            {
                return;
            }

            string name = null;
            if (!DA.GetData(1, ref name))
            {
                return;
            }

            string matType = null;
            if (!DA.GetData(2, ref matType))
            {
                return;
            }

            string groupName = null;
            if (!DA.GetData(3, ref groupName))
            {
                return;
            }

            string typeName = null;
            if (!DA.GetData(4, ref typeName))
            {
                return;
            }

            string sizeName = null;
            if (!DA.GetData(5, ref sizeName))
            {
                return;
            }

            if (name == null || matType == null || groupName == null || typeName == null || sizeName == null)
            {
                return;
            }

            // convert geometry
            List<FemDesign.Geometry.Region> regions = new List<FemDesign.Geometry.Region>();
            foreach (Rhino.Geometry.Brep brep in breps)
            {
                regions.Add(brep.FromRhino());
            }

            // create region group
            FemDesign.Geometry.RegionGroup regionGroup = new FemDesign.Geometry.RegionGroup(regions);

            // get mat type
            FemDesign.Materials.MaterialTypeEnum matTypeEnum = (FemDesign.Materials.MaterialTypeEnum)Enum.Parse(typeof(FemDesign.Materials.MaterialTypeEnum), matType);

            // create section
            FemDesign.Sections.Section section = new FemDesign.Sections.Section(regionGroup, name, "custom", matTypeEnum, groupName, typeName, sizeName);

            // return
            DA.SetData(0, section);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.SectionDefine;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("637c784c-7832-4a23-8cd7-a8a942dbb272"); }
        }

        protected override void BeforeSolveInstance()
        {
            ValueListUtils.updateValueLists(this, 2, Enum.GetNames(typeof( FemDesign.Materials.MaterialTypeEnum )).ToList()
            , null, GH_ValueListMode.DropDown);
        }

        public override GH_Exposure Exposure => GH_Exposure.hidden;


    }
}