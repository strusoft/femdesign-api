// https://strusoft.com/
using System;
using System.Linq;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Rhino.Geometry;
using Grasshopper;

namespace FemDesign.Grasshopper
{
    public class ComplexCompositeDeconstruct : GH_Component
    {
        public ComplexCompositeDeconstruct() : base("ComplexComposite.Deconstruct", "Deconstruct", "Deconstruct a complex  composite.", "FemDesign", "Deconstruct")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("ComplexComposite", "ComplexComposite", "ComplexComposite.", GH_ParamAccess.item);
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Material", "Material", "Material", GH_ParamAccess.tree);
            pManager.AddGenericParameter("Section", "Section", "Section", GH_ParamAccess.tree);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get input
            StruSoft.Interop.StruXml.Data.Complex_composite_type ComplexComposite = null;
            if (!DA.GetData(0, ref ComplexComposite)) { return; }
            if (ComplexComposite == null) { return; }

            // return


            DataTree<FemDesign.Materials.Material> material = new DataTree<FemDesign.Materials.Material>();
            DataTree<FemDesign.Sections.Section> section = new DataTree<FemDesign.Sections.Section>();

            for(int i = 0; i < ComplexComposite.Composite_section.Count; i++)
            {
                var item = ComplexComposite.Composite_section[i];
                foreach (var obj in item.CompositeSectionDataObj.Part)
                {
                    material.Add(obj.MaterialObj, new GH_Path(i));
                    section.Add(obj.SectionObj, new GH_Path(i));
                }
            }

            DA.SetDataTree(0, material);
            DA.SetDataTree(1, section);

        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.BarDeconstruct;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("B35D2E20-A7DB-4530-8F97-D99251DB634E"); }
        }

    }
}