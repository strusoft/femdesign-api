// https://strusoft.com/
using System;
using Grasshopper.Kernel;
using FemDesign;
using FemDesign.Calculate;
using FemDesign.Grasshopper.Extension.ComponentExtension;
using Grasshopper.Kernel.Special;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using FemDesign.Loads;

namespace FemDesign.Grasshopper
{
    public class CalculationParametersFreqDefine : FEM_Design_API_Component
    {
        public CalculationParametersFreqDefine() : base("Freq.Define", "Freq", "Define calculation parameters for an eigenfrequency calculation.", CategoryName.Name(), SubCategoryName.Cat7a())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddIntegerParameter("NumShapes", "NumShapes", "Number of shapes.", GH_ParamAccess.item, 2);
            pManager[pManager.ParamCount - 1].Optional = true;

            pManager.AddIntegerParameter("AutoIter", "AutoIter", "Iteration to try to reach 90% of horizontal effective mass", GH_ParamAccess.item, 0);
            pManager[pManager.ParamCount - 1].Optional = true;

            pManager.AddTextParameter("ShapeNormalisation", "ShapeNormalisation", "Connect 'ValueList' to get the options.\nShapeNormalisation type:\nUnit\nMassMatrix.", GH_ParamAccess.item, "Unit");
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

            int autoIter = 0;
            if (!DA.GetData(1, ref autoIter))
            {
                // pass
            }

            string shapeNormalisation = "";
            if (!DA.GetData(2, ref shapeNormalisation))
            {
                // pass
            }

            int maxSturm = 0;
            if (!DA.GetData(3, ref maxSturm))
            {
                // pass
            }

            bool x = true;
            if (!DA.GetData(4, ref x))
            {
                // pass
            }

            bool y = true;
            if (!DA.GetData(5, ref y))
            {
                // pass
            }

            bool z = true;
            if (!DA.GetData(6, ref z))
            {
                // pass
            }

            double top = -0.01;
            if (!DA.GetData(7, ref top))
            {
                // pass
            }

            ShapeNormalisation _shapeNormalisation = FemDesign.GenericClasses.EnumParser.Parse<ShapeNormalisation>(shapeNormalisation);

            Freq obj = new Calculate.Freq(numShapes, autoIter, _shapeNormalisation, x, y, z, maxSturm, top);

            // return
            DA.SetData(0, obj);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Properties.Resources.FreqDefine;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("{4A587916-E783-4B3E-9F63-75EA92117C8C}"); }
        }

        protected override void BeforeSolveInstance()
        {
            ValueListUtils.UpdateValueLists(this, 2, Enum.GetNames(typeof(ShapeNormalisation)).ToList(), null, GH_ValueListMode.DropDown);
        }


        public override GH_Exposure Exposure => GH_Exposure.tertiary;

    }
}