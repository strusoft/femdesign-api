// https://strusoft.com/
using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;
using Rhino.Geometry;
using FemDesign.Grasshopper.Extension.ComponentExtension;

namespace FemDesign.Grasshopper
{
    public class StiffnessModifierSlab : GH_Component
    {
        public StiffnessModifierSlab() : base("StiffnessModifierSlab", "StiffnessModifierSlab", "StiffnessModifier factor on Slab.", CategoryName.Name(),
             SubCategoryName.Cat2b())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("AnalysisType", "AnalysisType", "Connect 'ValueList' to get the options.\nFirstOrderU\nFirstOrderSq\nFirstOrderSf\nFirstOrderSc\nSecondOrderU\nSecondOrderSq\nSecondOrderSf\nSecondOrderSc\nStability\nDynamic", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("Bending11", "Bending11", "StiffnessModifierFactor", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("Bending22", "Bending22", "StiffnessModifierFactor", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("Bending12", "Bending12", "StiffnessModifierFactor", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("Membrane11", "Membrane11", "StiffnessModifierFactor", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("Membrane22", "Membrane22", "StiffnessModifierFactor", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("Membrane12", "Membrane12", "StiffnessModifierFactor", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("Shear13", "Shear13", "StiffnessModifierFactor", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("Shear23", "Shear23", "StiffnessModifierFactor", GH_ParamAccess.list);
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
            if (analysisType.Count >= 2 && isSameForAll)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, $"'SameForAllCalculation' must be use without the other analysis type");
                return;
            }

            List<double> bending11 = new List<double>();
            if (!DA.GetDataList(1, bending11))
            {
                bending11 = new List<double> { 1.0 };
            };

            List<double> bending22 = new List<double>();
            if (!DA.GetDataList(2, bending22))
            {
                bending22 = new List<double> { 1.0 };
            };

            List<double> bending12 = new List<double>();
            if (!DA.GetDataList(3, bending12))
            {
                bending12 = new List<double> { 1.0 };
            };

            List<double> membrane11 = new List<double>();
            if (!DA.GetDataList(4, membrane11))
            {
                membrane11 = new List<double> { 1.0 };
            };

            List<double> membrane22 = new List<double>();
            if (!DA.GetDataList(5, membrane22))
            {
                membrane22 = new List<double> { 1.0 };
            };

            List<double> membrane12 = new List<double>();
            if (!DA.GetDataList(6, membrane12))
            {
                membrane12 = new List<double> { 1.0 };
            };

            List<double> shear13 = new List<double>();
            if (!DA.GetDataList(7, shear13))
            {
                shear13 = new List<double> { 1.0 };
            };

            List<double> shear23 = new List<double>();
            if (!DA.GetDataList(8, shear23))
            {
                shear23 = new List<double> { 1.0 };
            };
            #endregion


            if (analysisType.Count != bending11.Count && (bending11.Count != 1 || bending11[0] != 1))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, $"The number of analysis type ({analysisType.Count}) and Area Factors must be equal.");
                return;
            }


            if (analysisType.Count != bending22.Count && (bending22.Count != 1 || bending22[0] != 1))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, $"The number of analysis type ({analysisType.Count}) and shearArea1 must be equal.");
                return;
            }

            if (analysisType.Count != bending12.Count && (bending12.Count != 1 || bending12[0] != 1))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, $"The number of analysis type ({analysisType.Count}) and shearArea2 must be equal.");
                return;
            }

            if (analysisType.Count != membrane11.Count && (membrane11.Count != 1 || membrane11[0] != 1))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, $"The number of analysis type ({analysisType.Count}) and torsional must be equal.");
                return;
            }

            if (analysisType.Count != membrane22.Count && (membrane22.Count != 1 || membrane22[0] != 1))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, $"The number of analysis type ({analysisType.Count}) and bendingAxis1 must be equal.");
                return;
            }

            if (analysisType.Count != membrane12.Count && (membrane12.Count != 1 || membrane12[0] != 1))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, $"The number of analysis type ({analysisType.Count}) and bendingAxis2 must be equal.");
                return;
            }

            if (analysisType.Count != shear13.Count && (shear13.Count != 1 || shear13[0] != 1))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, $"The number of analysis type ({analysisType.Count}) and bendingAxis2 must be equal.");
                return;
            }

            if (analysisType.Count != shear23.Count && (shear23.Count != 1 || shear23[0] != 1))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, $"The number of analysis type ({analysisType.Count}) and bendingAxis2 must be equal.");
                return;
            }

            var maxLength = new List<int> { bending11.Count, bending22.Count, bending12.Count, membrane11.Count, membrane22.Count, membrane12.Count, shear13.Count, shear23.Count }.Max();

            var stiffFactors = new Shells.SlabStiffnessFactors();


            if (isSameForAll)
            {
                var slabStiffRecord = new StruSoft.Interop.StruXml.Data.Slab_stiffness_record(bending11[0], bending22[0], bending12[0], membrane11[0], membrane22[0], membrane12[0], shear13[0], shear23[0]);
                stiffFactors._stiffnessModifiers = Shells.SlabStiffnessFactors.SameAllCalculation(slabStiffRecord);
            }
            else
            {
                // initiate the object
                stiffFactors._stiffnessModifiers = Shells.SlabStiffnessFactors.Default();

                for (int i = 0; i < maxLength; i++)
                {
                    var value = (FemDesign.GenericClasses.StiffnessAnalysisType)Enum.Parse(typeof(FemDesign.GenericClasses.StiffnessAnalysisType), analysisType[i]);
                    var slabStiffRecord = new StruSoft.Interop.StruXml.Data.Slab_stiffness_record();

                    if (i < bending11.Count)
                    {
                        slabStiffRecord.Bending_1_1 = bending11[i];
                    }
                    else
                    {
                        slabStiffRecord.Bending_1_1 = bending11[bending11.Count - 1];
                    }

                    if (i < bending22.Count)
                    {
                        slabStiffRecord.Bending_2_2 = bending22[i];
                    }
                    else
                    {
                        slabStiffRecord.Bending_2_2 = bending22[bending22.Count - 1];
                    }

                    if (i < bending12.Count)
                    {
                        slabStiffRecord.Bending_1_2 = bending12[i];
                    }
                    else
                    {
                        slabStiffRecord.Bending_1_2 = bending12[bending12.Count - 1];
                    }

                    if (i < membrane11.Count)
                    {
                        slabStiffRecord.Membran_1_1 = membrane11[i];
                    }
                    else
                    {
                        slabStiffRecord.Membran_1_1 = membrane11[membrane11.Count - 1];
                    }

                    if (i < membrane22.Count)
                    {
                        slabStiffRecord.Membran_2_2 = membrane22[i];
                    }
                    else
                    {
                        slabStiffRecord.Membran_2_2 = membrane22[membrane22.Count - 1];
                    }

                    if (i < membrane12.Count)
                    {
                        slabStiffRecord.Membran_1_2 = membrane12[i];
                    }
                    else
                    {
                        slabStiffRecord.Membran_1_2 = membrane12[membrane12.Count - 1];
                    }

                    if (i < shear13.Count)
                    {
                        slabStiffRecord.Shear_1_3 = shear13[i];
                    }
                    else
                    {
                        slabStiffRecord.Shear_1_3 = shear13[shear13.Count - 1];
                    }

                    if (i < shear23.Count)
                    {
                        slabStiffRecord.Shear_2_3 = shear23[i];
                    }
                    else
                    {
                        slabStiffRecord.Shear_2_3 = shear23[shear23.Count - 1];
                    }

                    stiffFactors._stiffnessModifiers[value] = slabStiffRecord;

                }
            }
            // output
            DA.SetData(0, stiffFactors);
        }


        protected override void BeforeSolveInstance()
        {
            ValueListUtils.updateValueLists(this, 0, Enum.GetNames(typeof(FemDesign.GenericClasses.StiffnessAnalysisType)).ToList(), null, 0);
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.StiffnessModifier; ;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("{BD936782-5D0C-4FBC-AA56-CA5C90389272}"); }
        }
        public override GH_Exposure Exposure => GH_Exposure.quinary;
    }
}