using System;
using System.Collections.Generic;
using System.Linq;

using Grasshopper.Kernel;

using FemDesign;

namespace FemDesign.Grasshopper
{
    public class CompositeSectionBeamB : GH_Component
    {
        public CompositeSectionBeamB() : base("CompositeSection.BeamB", "CompositeSection", "Composite section. For more information check FEM-Design GUI.", CategoryName.Name(), SubCategoryName.Cat2a())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("SectionName", "Name", "Composite section name.", GH_ParamAccess.item);
            pManager.AddGenericParameter("Steel", "Steel", "Steel part material.", GH_ParamAccess.item);
            pManager.AddGenericParameter("Concrete", "Concrete", "Concrete part material.", GH_ParamAccess.item);
            pManager.AddNumberParameter("b", "b", "Intermediate width of the bottom flange [mm]. Optional, default value if undefined.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("bt", "bt", "Top flange width [mm]. Optional, default value if undefined.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("o1", "o1", "Left overhang [mm]. Optional, default value if undefined.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("o2", "o2", "Right overhang [mm]. Optional, default value if undefined.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("h", "h", "Web hight [mm]. Optional, default value if undefined.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("tw", "tw", "Web thickness [mm]. Optional, default value if undefined.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("tfb", "tfb", "Bottom flange thickness [mm]. Optional, default value if undefined.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("tft", "tft", "Top flange thickness [mm]. Optional, default value if undefined.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.Register_GenericParam("CompositeSection", "Section", "Steel-concrete composite section.");
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

            double b = 200;
            DA.GetData(3, ref b);

            double bt = 240;
            DA.GetData(4, ref bt);

            double o1 = 150;
            DA.GetData(5, ref o1);

            double o2 = 150;
            DA.GetData(6, ref o2);

            double h = 360;
            DA.GetData(7, ref h);

            double tw = 10;
            DA.GetData(8, ref tw);

            double tfb = 20;
            DA.GetData(9, ref tfb);

            double tft = 20;
            DA.GetData(10, ref tft);

            // check input data
            if (name == null || steel == null || concrete == null || b == 0 || bt == 0 || h == 0 || tw == 0 || tfb == 0 || tft == 0) { return; }
            if (steel.Family != Materials.Family.Steel)
            {
                throw new ArgumentException($"Steel input must be steel material but it is {steel.Family}");
            }
            if (concrete.Family != Materials.Family.Concrete)
            {
                throw new ArgumentException($"Concrete input must be concrete material but it is {concrete.Family}");
            }

            // create composite section
            Composites.CompositeSection compositeSection = Composites.CompositeSection.BeamB(steel, concrete, name, b, bt, o1, o2, h, tw, tfb, tft);

            // get output
            DA.SetData(0, null);
            DA.SetData(0, compositeSection);
        }
        //protected override System.Drawing.Bitmap Icon
        //{
        //    get
        //    {
        //        return FemDesign.Properties.Resources
        //    }
        //}
        protected override System.Drawing.Bitmap Icon { get { return null; } }

        public override Guid ComponentGuid
        {
            get { return new Guid("{D20F98A4-38DC-46C4-9E15-D6F4D0DF8AA5}"); }
        }
        public override GH_Exposure Exposure => GH_Exposure.primary;
    }
}
