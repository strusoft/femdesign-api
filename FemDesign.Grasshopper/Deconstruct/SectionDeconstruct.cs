// https://strusoft.com/
using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;

namespace FemDesign.Grasshopper
{
    public class SectionDeconstruct: FEM_Design_API_Component
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
           pManager.AddTextParameter("MaterialType", "MaterialType", "MaterialType.", GH_ParamAccess.list);
           pManager.AddTextParameter("GroupName", "GroupName", "GroupName.", GH_ParamAccess.item);
           pManager.AddTextParameter("TypeName", "TypeName", "TypeName.", GH_ParamAccess.item);
           pManager.AddTextParameter("SizeName", "SizeName", "SizeName.", GH_ParamAccess.item);
       }
       protected override void SolveInstance(IGH_DataAccess DA)
       {
            object _obj = null;
            if (!DA.GetData(0, ref _obj))
            {
                return;
            }

            if (_obj is GH_ObjectWrapper dataWrapper)
            {
                var objVal = dataWrapper.Value;

                if (objVal is Sections.Section)
                {
                    Sections.Section obj = (Sections.Section)objVal;
                    DA.SetData(0, obj.Guid);
                    DA.SetData(1, obj.Name);
                    DA.SetDataList(2, obj.RegionGroup.ToRhino());
                    DA.SetData(3, obj.Type);
                    DA.SetDataList(4, new List<string> { obj.MaterialType });
                    DA.SetData(5, obj.GroupName);
                    DA.SetData(6, obj.TypeName);
                    DA.SetData(7, obj.SizeName);
                }
                else if (objVal is Composites.CompositeSection)
                {
                    Composites.CompositeSection obj = (Composites.CompositeSection)objVal;
                    DA.SetData(0, obj.Guid);
                    DA.SetData(1, obj.ParameterDictionary[Composites.CompositeSectionParameterType.Name]);

                    List<Rhino.Geometry.Brep> regions = new List<Rhino.Geometry.Brep>();
                    foreach (var item in obj.Sections)
                    {
                        var _region = item.RegionGroup.ToRhino();
                        regions.AddRange(_region);
                    }
                    DA.SetDataList(2, regions);

                    DA.SetData(3, obj.Type);

                    List<string> matType = obj.Materials.Select(m => m.Family.ToString()).ToList();
                    DA.SetDataList(4, matType);

                    DA.SetData(5, null);
                    DA.SetData(6, null);
                    DA.SetData(7, null);
                }
                else
                {
                    throw new ArgumentException($"Section input parameter type must be Section or CompositeSection, but it is {_obj.GetType()}");
                }
            }
            else
            {
                return;
            }

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
           get { return new Guid("{51E24345-C191-4FDA-BC7D-F99E15687405}"); }
       }

        public override GH_Exposure Exposure => GH_Exposure.tertiary;

    }
}