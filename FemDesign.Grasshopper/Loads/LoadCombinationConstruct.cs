// https://strusoft.com/
using System;
using System.Linq;
using System.Collections.Generic;

using Rhino.Geometry;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Special;
using Grasshopper.Kernel.Types;
using FemDesign.Grasshopper.Extension.ComponentExtension;

using FemDesign.Loads;

namespace FemDesign.Grasshopper
{
    public class LoadCombinationConstruct : GH_Component
    {
        public LoadCombinationConstruct() : base("LoadCombination.Construct", "Construct", "Construct a LoadCombination from a LoadCase or a list of LoadCases.", CategoryName.Name(), SubCategoryName.Cat3())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Name", "Name", "Name of LoadCombination.", GH_ParamAccess.item);
            pManager.AddTextParameter("Type", "Type", "Connect 'ValueList' to get the options.\nultimate_ordinary\nultimate_accidental\nultimate_seismic\nserviceability_quasi_permanent\nserviceability_frequent\nserviceability_characteristic.", GH_ParamAccess.item, "ultimate_ordinary");
            pManager[1].Optional = true;
            pManager.AddGenericParameter("LoadCase", "LoadCase", "Load cases to include in LoadCombination.\n\nThis may also include a single construction stage and/or any special load case strings. Connect a ValueList to get the options for the special load cases.", GH_ParamAccess.list);
            pManager.AddNumberParameter("Gamma", "Gamma", "Gamma value for respective LoadCase. Must be equal to the number of load cases.", GH_ParamAccess.list);
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("LoadCombination", "LoadCombination.", "LoadCombination.", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string name = null, type = "ultimate_ordinary";
            List<object> loadCasesOrOthers = new List<object>();
            List<double> gammas = new List<double>();
            if (!DA.GetData(0, ref name)) { return; }
            if (!DA.GetDataList(2, loadCasesOrOthers)) { return; }
            if (!DA.GetDataList(3, gammas)) { return; }

            DA.GetData(1, ref type);

            if (loadCasesOrOthers.Count != gammas.Count)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, $"The number of load cases ({loadCasesOrOthers.Count}) and gamma values ({gammas.Count}) must be equal.");
                return;
            }

            var _type = FemDesign.GenericClasses.EnumParser.Parse<LoadCombType>(type);

            if (name == null || type == null || loadCasesOrOthers == null || gammas == null) { return; }

            LoadCombination loadCombination = new LoadCombination(name, _type);

            LoadCase loadCase = null;
            Stage stage = null;
            string text = null;
            for (int i = 0; i < loadCasesOrOthers.Count; i++)
            {
                object loadCaseLike = loadCasesOrOthers[i];
                double gamma = gammas[i];

                if (loadCaseLike.GetType() == typeof(GH_ObjectWrapper))
                {
                    GH_ObjectWrapper wrapper = (GH_ObjectWrapper)loadCaseLike;

                    if (wrapper.Value.TryCast(ref loadCase))
                        loadCombination.AddLoadCase(loadCase, gamma);

                    //if (wrapper.CastTo(out stage))
                    else if (wrapper.Value.TryCast(ref stage))
                        loadCombination.SetStageLoadCase(stage, gamma);
                }

                else if (loadCaseLike.GetType() == typeof(GH_String))
                {
                    GH_String wrapper = (GH_String)loadCaseLike;

                    if (wrapper.Value.TryCast(ref text))
                    {
                        string sanitized = text.ToLower()
                            .Replace("_", " ")
                            .Replace(".", "")
                            .Replace(",", "")
                            .Trim();
                        switch (sanitized)
                        {
                            case "seis max":
                            case "seismic max":
                                loadCombination.SeismicMax = new LoadCombinationCaseBase(gamma);
                                break;

                            case "seis fx-mx":
                            case "seis res fx-mx":
                            case "seismic fx-mx":
                            case "seismic res fx-mx":
                                loadCombination.SeismicResFxMinusMx = new LoadCombinationCaseBase(gamma);
                                break;

                            case "seis fx+mx":
                            case "seis res fx+mx":
                            case "seismic fx+mx":
                            case "seismic res fx+mx":
                                loadCombination.SeismicResFxPlusMx = new LoadCombinationCaseBase(gamma);
                                break;

                            case "seis fy-my":
                            case "seis res fy-my":
                            case "seismic fy-my":
                            case "seismic res fy-my":
                                loadCombination.SeismicResFyMinusMy = new LoadCombinationCaseBase(gamma);
                                break;

                            case "seis fy+my":
                            case "seis res fy+my":
                            case "seismic fy+my":
                            case "seismic res fy+my":
                                loadCombination.SeismicResFyPlusMy = new LoadCombinationCaseBase(gamma);
                                break;

                            case "seis fz":
                            case "seis res fz":
                            case "seismic fz":
                            case "seismic res fz":
                                loadCombination.SeismicResFz = new LoadCombinationCaseBase(gamma);
                                break;

                            case "t0":
                            case "ptc t0":
                                loadCombination.PtcT0 = new LoadCombinationCaseBase(gamma);
                                break;

                            case "t8":
                            case "ptc t8":
                                loadCombination.PtcT8 = new LoadCombinationCaseBase(gamma);
                                break;

                            case "pile":
                            case "pile loadcase":
                            case "pile load case":
                            case "ldcase pile":
                            case "loadcase pile":
                            case "load case pile":
                            case "neg shaft friction":
                                loadCombination.PileLoadCase = new LoadCombinationCaseBase(gamma);
                                break;

                            case "final cs":
                            case "final stage":
                            case "final construction stage":
                                loadCombination.SetFinalStageLoadCase(gamma);
                                break;

                            default:
                                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, $"'{wrapper.Value}' is not a valid special load case string.");
                                break;
                        }
                    }
                }

                else
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Error, $"'{loadCaseLike}' of type {loadCaseLike.GetType().FullName} is not a valid LoadCase-like object.");
            }

            DA.SetData(0, loadCombination);
        }

        protected override void BeforeSolveInstance()
        {
            ValueListUtils.updateValueLists(this, 1, new List<string>
            { "ultimate_ordinary",
                "ultimate_accidental",
                "ultimate_seismic",
                "serviceability_quasi_permanent",
                "serviceability_frequent",
                "serviceability_characteristic"
            }, null, GH_ValueListMode.DropDown);

            ValueListUtils.updateValueLists(this, 2, new List<string>
            {
                "Seismic max.",
                "Seis res, Fx+Mx",
                "Seis res, Fx-Mx",
                "Seis res, Fy+My",
                "Seis res, Fy-My",
                "Seis res, Fz",
                "Final construction stage",
                "PTC T0",
                "PTC T8",
                "Neg. Shaft friction"
            }, null, GH_ValueListMode.DropDown);
        }

        protected override System.Drawing.Bitmap Icon => FemDesign.Properties.Resources.LoadCombinationDefine;
        public override Guid ComponentGuid => new Guid("4dc97ef5-44a8-4117-8abd-c94fdadd1f11");
        public override GH_Exposure Exposure => GH_Exposure.quarternary;
    }

    public static class CastingExtensions
    {
        public static bool TryCast<T>(this object obj, ref T result)
        {
            if (obj is T)
            {
                result = (T)obj;
                return true;
            }

            result = default(T);
            return false;
        }
    }
}