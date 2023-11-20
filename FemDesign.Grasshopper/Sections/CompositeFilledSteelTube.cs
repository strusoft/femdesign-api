// https://strusoft.com/
using System;
using System.Collections.Generic;
using System.Linq;

using Grasshopper.Kernel;

using FemDesign;

namespace FemDesign.Grasshopper
{
    public class CompositeFilledSteelTube : GH_Component
    {
        public CompositeFilledSteelTube() : base("Composite.FilledSteelTube", "Composite", "Create a composite section for filled CHS cross-sections. For more information, see FEM-Design GUI.", CategoryName.Name(), SubCategoryName.Cat4b())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("SectionName", "Name", "Composite section name.", GH_ParamAccess.item);
            pManager.AddGenericParameter("Steel", "Steel", "Steel material.", GH_ParamAccess.item);
            pManager.AddGenericParameter("Concrete", "Concrete", "Concrete material.", GH_ParamAccess.item);
            pManager.AddNumberParameter("d", "d", "Diameter of steel tube [mm].", GH_ParamAccess.item, 159);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("t", "t", "Thickness of tube [mm].", GH_ParamAccess.item, 10);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("CompositeSection", "Section", "Steel-concrete composite section.",GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get input
            string name = null;
            if (!DA.GetData(0, ref name)) { return; }

            Materials.Material steel = new Materials.Material();
            if (!DA.GetData(1, ref steel)) { return; }

            Materials.Material concrete = new Materials.Material();
            if (!DA.GetData(2, ref concrete)) { return; }

            double d = 159;
            DA.GetData(3, ref d);

            double t = 10;
            DA.GetData(4, ref t);

            // check input data
            if (steel.Family != Materials.Family.Steel)
                throw new ArgumentException($"Steel input must be steel material but it is {steel.Family}");
            if (concrete.Family != Materials.Family.Concrete)
                throw new ArgumentException($"Concrete input must be concrete material but it is {concrete.Family}");

            // create composite section
            Composites.CompositeSection compositeSection = Composites.CompositeSection.FilledSteelTube(name, steel, concrete, d, t);

            // get output
            DA.SetData(0, compositeSection);
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
            get { return new Guid("8306C4B5-1F8F-4436-8CA5-831C7B7BDC79"); }
        }
        public override GH_Exposure Exposure => GH_Exposure.quarternary;
    }
}
