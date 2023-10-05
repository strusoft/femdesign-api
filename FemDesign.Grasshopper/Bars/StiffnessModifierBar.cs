// https://strusoft.com/
using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;
using Rhino.Geometry;
using FemDesign.Grasshopper.Extension.ComponentExtension;

namespace FemDesign.Grasshopper
{
    public class StiffnessModifierBar : FEM_Design_API_Component
    {
        public StiffnessModifierBar() : base("StiffnessModifierBar", "StiffnessModifierBar", "StiffnessModifier factor on Beam.", CategoryName.Name(),
             SubCategoryName.Cat2a())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("AnalysisType", "AnalysisType", "Connect 'ValueList' to get the options.\nFirstOrderU\nFirstOrderSq\nFirstOrderSf\nFirstOrderSc\nSecondOrderU\nSecondOrderSq\nSecondOrderSf\nSecondOrderSc\nStability\nDynamic", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;
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
            pManager.AddGenericParameter("StiffnessModifier", "StiffnessModifier", "", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            #region INPUT
            // get input
            var analysisType = new List<string>();
            if (!DA.GetDataList(0, analysisType))
            {
                analysisType = new List<string> { "SameForAllCalculation" };
            };

            var isSameForAll = analysisType.Contains(FemDesign.GenericClasses.StiffnessAnalysisType.SameForAllCalculation.ToString());
            if(analysisType.Count >= 2 && isSameForAll)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, $"'SameForAllCalculation' must be use without the other analysis type");
                return;
            }

            List<double> areaFactors = new List<double>();
            if (!DA.GetDataList(1, areaFactors))
            {
                areaFactors = new List<double> { 1.0 };
            };

            List<double> shearArea1 = new List<double>();
            if (!DA.GetDataList(2, shearArea1))
            {
                shearArea1 = new List<double> { 1.0 };
            };

            List<double> shearArea2 = new List<double>();
            if (!DA.GetDataList(3, shearArea2))
            {
                shearArea2 = new List<double> { 1.0 };
            };

            List<double> torsional = new List<double>();
            if (!DA.GetDataList(4, torsional))
            {
                torsional = new List<double> { 1.0 };
            };

            List<double> bendingAxis1 = new List<double>();
            if (!DA.GetDataList(5, bendingAxis1))
            {
                bendingAxis1 = new List<double> { 1.0 };
            };

            List<double> bendingAxis2 = new List<double>();
            if (!DA.GetDataList(6, bendingAxis2))
            {
                bendingAxis2 = new List<double> { 1.0 };
            };
            #endregion


            if ( analysisType.Count != areaFactors.Count && (areaFactors.Count!= 1 || areaFactors[0] != 1) )
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, $"The number of analysis type ({analysisType.Count}) and Area Factors must be equal.");
                return;
            }


            if (analysisType.Count != shearArea1.Count && (shearArea1.Count != 1 || shearArea1[0] != 1))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, $"The number of analysis type ({analysisType.Count}) and shearArea1 must be equal.");
                return;
            }

            if (analysisType.Count != shearArea2.Count && (shearArea2.Count != 1 || shearArea2[0] != 1))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, $"The number of analysis type ({analysisType.Count}) and shearArea2 must be equal.");
                return;
            }

            if (analysisType.Count != torsional.Count && (torsional.Count != 1 || torsional[0] != 1))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, $"The number of analysis type ({analysisType.Count}) and torsional must be equal.");
                return;
            }

            if (analysisType.Count != bendingAxis1.Count && (bendingAxis1.Count != 1 || bendingAxis1[0] != 1))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, $"The number of analysis type ({analysisType.Count}) and bendingAxis1 must be equal.");
                return;
            }

            if (analysisType.Count != bendingAxis2.Count && (bendingAxis2.Count != 1 || bendingAxis2[0] != 1))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, $"The number of analysis type ({analysisType.Count}) and bendingAxis2 must be equal.");
                return;
            }

            var maxLength = new List<int> { areaFactors.Count, shearArea1.Count, shearArea2.Count, torsional.Count, bendingAxis1.Count, bendingAxis2.Count }.Max();

            var stiffFactors = new Bars.BarStiffnessFactors();


            if(isSameForAll)
            {
                var barStiffRecord = new StruSoft.Interop.StruXml.Data.Bar_stiffness_factor_record(areaFactors[0], shearArea1[0], shearArea2[0], torsional[0], bendingAxis1[0], bendingAxis2[0]);
                stiffFactors._keyPairAnalysysFactors = Bars.BarStiffnessFactors.SameAllCalculation(barStiffRecord);
            }
            else
            {
                // initiate the object
                stiffFactors._keyPairAnalysysFactors = Bars.BarStiffnessFactors.Default();

                for (int i = 0; i < maxLength; i++)
                {
                    var value = (FemDesign.GenericClasses.StiffnessAnalysisType)Enum.Parse(typeof(FemDesign.GenericClasses.StiffnessAnalysisType), analysisType[i]);
                    var barStiffRecord = new StruSoft.Interop.StruXml.Data.Bar_stiffness_factor_record();

                    if (i < areaFactors.Count)
                    {
                        barStiffRecord.Crosssectional_area = areaFactors[i];
                    }
                    else
                    {
                        barStiffRecord.Crosssectional_area = areaFactors[areaFactors.Count - 1];
                    }

                    if (i < shearArea1.Count)
                    {
                        barStiffRecord.Shear_area_direction_1 = shearArea1[i];
                    }
                    else
                    {
                        barStiffRecord.Shear_area_direction_1 = shearArea1[shearArea1.Count - 1];
                    }

                    if (i < shearArea2.Count)
                    {
                        barStiffRecord.Shear_area_direction_2 = shearArea2[i];
                    }
                    else
                    {
                        barStiffRecord.Shear_area_direction_2 = shearArea2[shearArea2.Count - 1];
                    }

                    if (i < torsional.Count)
                    {
                        barStiffRecord.Torsional_constant = torsional[i];
                    }
                    else
                    {
                        barStiffRecord.Torsional_constant = torsional[torsional.Count - 1];
                    }

                    if (i < bendingAxis1.Count)
                    {
                        barStiffRecord.Inertia_about_axis_1 = bendingAxis1[i];
                    }
                    else
                    {
                        barStiffRecord.Inertia_about_axis_1 = bendingAxis1[bendingAxis1.Count - 1];
                    }

                    if (i < bendingAxis2.Count)
                    {
                        barStiffRecord.Inertia_about_axis_2 = bendingAxis2[i];
                    }
                    else
                    {
                        barStiffRecord.Inertia_about_axis_2 = bendingAxis2[bendingAxis2.Count - 1];
                    }

                    stiffFactors._keyPairAnalysysFactors[value] = barStiffRecord;

                }

            }

            stiffFactors._factors = stiffFactors.Factors;
            // output
            DA.SetData(0, stiffFactors);
        }


        protected override void BeforeSolveInstance()
        {
            ValueListUtils.updateValueLists(this, 0, Enum.GetNames( typeof(FemDesign.GenericClasses.StiffnessAnalysisType) ).ToList(), null, 0);
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.StiffnessModifier;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("{EDCE3350-C23B-4A4A-9F58-7500E0F1A7DB}"); }
        }
        public override GH_Exposure Exposure => GH_Exposure.secondary;


    }
}