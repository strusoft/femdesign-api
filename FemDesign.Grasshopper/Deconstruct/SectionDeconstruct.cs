// https://strusoft.com/
using System;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class SectionDeconstruct: GH_Component
    {
       public SectionDeconstruct(): base("Section.Deconstruct", "Deconstruct", "Deconstruct a Section.", "FEM-Design", "Deconstruct")
       {

       }
       protected override void RegisterInputParams(GH_InputParamManager pManager)
       {
           pManager.AddGenericParameter("Section", "Section", "Section.", GH_ParamAccess.item);           
       } 
       protected override void RegisterOutputParams(GH_OutputParamManager pManager)
       {   
           pManager.AddTextParameter("Guid", "Guid", "Guid.", GH_ParamAccess.item);
           pManager.AddTextParameter("Name", "Name", "Name.", GH_ParamAccess.item);
           pManager.AddBrepParameter("Surfaces", "Surfaces", "Surfaces.", GH_ParamAccess.list);
           pManager.AddTextParameter("SectionType", "SectionType", "SectionType.", GH_ParamAccess.item);
           pManager.AddTextParameter("MaterialType", "MaterialType", "MaterialType.", GH_ParamAccess.item);
           pManager.AddTextParameter("GroupName", "GroupName", "GroupName.", GH_ParamAccess.item);
           pManager.AddTextParameter("TypeName", "TypeName", "TypeName.", GH_ParamAccess.item);
           pManager.AddTextParameter("SizeName", "SizeName", "SizeName.", GH_ParamAccess.item);
       }
       protected override void SolveInstance(IGH_DataAccess DA)
       {
            FemDesign.Sections.Section obj = null;
            if (!DA.GetData(0, ref obj))
            {
                return;
            }

            DA.SetData(0, obj.Guid);
            DA.SetData(1, obj.Name);
            DA.SetDataList(2, obj.RegionGroup.ToRhino());
            DA.SetData(3, obj.Type);
            DA.SetData(4, obj.MaterialType);
            DA.SetData(5, obj.GroupName);
            DA.SetData(6, obj.TypeName);
            DA.SetData(7, obj.SizeName);
       }
       protected override System.Drawing.Bitmap Icon
       {
           get
           {
                return FemDesign.Properties.Resources.SectionDeconstruct;
           }
       }
       public override Guid ComponentGuid
       {
           get { return new Guid("79ff17b1-387a-44e1-9596-981b8a96e847"); }
       }

        public override GH_Exposure Exposure => GH_Exposure.tertiary;

    }
}