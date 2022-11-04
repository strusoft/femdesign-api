// https://strusoft.com/
using System;
using System.Linq;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Special;

using FemDesign.Grasshopper.Extension.ComponentExtension;

namespace FemDesign.Grasshopper
{
    public class SectionDefine : GH_Component
    {
        public SectionDefine() : base("Section.Define", "Define", "Define a new custom section.", CategoryName.Name(), SubCategoryName.Cat4b())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddSurfaceParameter("Surfaces", "Srfs", "Item or list of surfaces of section. Surfaces must lie in the XY-plane at z=0.", GH_ParamAccess.list);
            pManager.AddTextParameter("MaterialType", "MatType", "Connect 'ValueList' to get the options.\nSteelRolled\nSteelColdWorked\nSteelWelded\nConcrete\nTimber\nUnknown\nUndefined", GH_ParamAccess.item);
            pManager.AddTextParameter("GroupName", "GroupName", "Name of section group", GH_ParamAccess.item);
            pManager.AddTextParameter("TypeName", "TypeName", "Name of section type", GH_ParamAccess.item);
            pManager.AddTextParameter("SizeName", "SizeName", "Name of section size. The name must be unique", GH_ParamAccess.item);
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

            string matType = null;
            if (!DA.GetData(1, ref matType))
            {
                return;
            }

            string groupName = null;
            if (!DA.GetData(2, ref groupName))
            {
                return;
            }

            string typeName = null;
            if (!DA.GetData(3, ref typeName))
            {
                return;
            }

            string sizeName = null;
            if (!DA.GetData(4, ref sizeName))
            {
                return;
            }

            if (matType == null || groupName == null || typeName == null || sizeName == null)
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
            FemDesign.Sections.Section section = new FemDesign.Sections.Section(regionGroup, "custom", matTypeEnum, groupName, typeName, sizeName);

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
            get { return new Guid("{FD3B284F-1199-4870-8902-5937A1BADE71}"); }
        }

        protected override void BeforeSolveInstance()
        {
            ValueListUtils.updateValueLists(this, 1, Enum.GetNames(typeof(FemDesign.Materials.MaterialTypeEnum)).ToList()
            , null, GH_ValueListMode.DropDown);
        }

        public override GH_Exposure Exposure => GH_Exposure.tertiary;


    }
}