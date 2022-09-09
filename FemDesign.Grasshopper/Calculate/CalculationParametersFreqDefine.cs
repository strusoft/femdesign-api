// https://strusoft.com/
using System;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class CalculationParametersFreqDefine: GH_Component
    {
        public CalculationParametersFreqDefine(): base("Freq.Define", "Define", "Define calculation parameters for an eigenfrequency calculation.", CategoryName.Name(), SubCategoryName.Cat7a())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddIntegerParameter("NumShapes", "NumShapes", "Number of shapes.", GH_ParamAccess.item, 2);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddIntegerParameter("MaxSturm", "MaxSturm", "Max number of Sturm check steps (checking missing eigenvalues).", GH_ParamAccess.item, 0);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("X", "X", "Consider masses in global x-direction.", GH_ParamAccess.item, true);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("Y", "Y", "Consider masses in global y-direction.", GH_ParamAccess.item, true);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("Z", "Z", "Consider masses in global z-direction.", GH_ParamAccess.item, true);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("Top", "Top", "Top of substructure. Masses on this level and below are not considered in Eigenfrequency calculation.", GH_ParamAccess.item, -0.01);
            pManager[pManager.ParamCount - 1].Optional = true;

        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Freq", "Freq", "Freq.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            int numShapes = 2;
            if (!DA.GetData(0, ref numShapes))
            {
                // pass
            }

            int maxSturm = 0;
            if (!DA.GetData(1, ref maxSturm))
            {
                // pass
            }

            bool x = true;
            if (!DA.GetData(2, ref x))
            {
                // pass
            }
            
            bool y = true;
            if (!DA.GetData(3, ref y))
            {
                // pass
            }

            bool z = true;
            if (!DA.GetData(4, ref z))
            {
                // pass
            }

            double top = -0.01;
            if (!DA.GetData(5, ref top))
            {
                // pass
            }
            
            FemDesign.Calculate.Freq obj = new Calculate.Freq(numShapes, maxSturm, x, y, z, top);

            // return
            DA.SetData(0, obj);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.FreqDefine;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("f29e56a7-6112-402c-af80-ad0fe07f12b2"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.tertiary;

    }
}