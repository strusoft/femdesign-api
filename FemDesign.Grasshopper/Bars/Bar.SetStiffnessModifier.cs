// https://strusoft.com/
using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.Grasshopper
{
    public class Bars_SetStiffnessModifier : GH_Component
    {
        public Bars_SetStiffnessModifier() : base("Bars.SetStiffnessModifier", "StiffnessModifier", "Set StiffnessModifier factor on Beam.", CategoryName.Name(),
             SubCategoryName.Cat2a())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Bar", "Bar", "Bar", GH_ParamAccess.item);
            pManager.AddNumberParameter("CrossSectionArea", "CrossSectionArea", "StiffnessModifierFactor", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("ShearAreaDirection1", "ShearAreaDirection1", "StiffnessModifierFactor", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("ShearAreaDirection2", "ShearAreaDirection2", "StiffnessModifierFactor", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("TorsionalConstant", "TorsionalConstant", "StiffnessModifierFactor", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("InertiaAboutAxis1", "InertiaAboutAxis1", "StiffnessModifierFactor", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("InertiaAboutAxis2", "InertiaAboutAxis2", "StiffnessModifierFactor", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Bar", "Bar", "Bar.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get input
            Bars.Bar bar = null;
            if (!DA.GetData(0, ref bar)) { return; }

            List<double> areaFactors = new List<double>();
            if(!DA.GetDataList(1, areaFactors))
            {
                areaFactors = new List<double> { 1.0 };
            };

            List<double> shearArea1 = new List<double> { 1.0 };
            DA.GetDataList(2, shearArea1);

            List<double> shearArea2 = new List<double> { 1.0 };
            DA.GetDataList(3, shearArea2);

            List<double> torsional = new List<double> { 1.0 };
            DA.GetDataList(4, torsional);

            List<double> bendingAxis1 = new List<double> { 1.0 };
            DA.GetDataList(5, bendingAxis1);

            List<double> bendingAxis2 = new List<double> { 1.0 };
            DA.GetDataList(6, bendingAxis2);


            var stiffFactors = new Bars.BarStiffnessFactors();
            stiffFactors.StiffnessModifiers = new List<StruSoft.Interop.StruXml.Data.Bar_stiffness_factor_record>();

            var maxListLength = new List<double>{ areaFactors.Count, shearArea1.Count, shearArea2.Count, torsional.Count, bendingAxis1.Count, bendingAxis2.Count }.Max();

            for(int i = 0; i < maxListLength; i++)
            {
                double area;
                try {area = areaFactors[i];}
                catch { area = 1.0;}

                double shear1;
                try { shear1 = shearArea1[i]; }
                catch { shear1 = 1.0; }

                double shear2;
                try { shear2 = shearArea2[i]; }
                catch { shear2 = 1.0; }

                double torsion;
                try { torsion = torsional[i]; }
                catch { torsion = 1.0; }

                double bending1;
                try { bending1 = bendingAxis1[i]; }
                catch { bending1 = 1.0; }

                double bending2;
                try { bending2 = bendingAxis2[i]; }
                catch { bending2 = 1.0; }

                var stiffModifier = new StruSoft.Interop.StruXml.Data.Bar_stiffness_factor_record(area, shear1, shear2, torsion, bending1, bending2);

                stiffFactors.StiffnessModifiers.Add(stiffModifier);
            }

            // output
            bar.BarPart.BarStiffnessFactors = stiffFactors;
            DA.SetData(0, bar);

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
            get { return new Guid("{759CCE08-5EE6-4ECA-B2B8-4BF14B3694A0}"); }
        }
        public override GH_Exposure Exposure => GH_Exposure.tertiary;


    }
}