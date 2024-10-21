// https://strusoft.com/
using System;
using System.Linq;
using System.Collections.Generic;
using Grasshopper.Kernel;
using System.Threading.Tasks;
using FemDesign.Loads;
using FemDesign.Grasshopper.Extension.ComponentExtension;
using Grasshopper.Kernel.Special;
using FemDesign.GenericClasses;

namespace FemDesign.Grasshopper
{
    public class LoadGroupToLoadComb : FEM_Design_API_Component
    {
        public LoadGroupToLoadComb() : base("LoadGroupToLoadComb", "LoadGroupToLoadComb", "", CategoryName.Name(), SubCategoryName.Cat3())
        {

        }

        public List<string> _log = new List<string>();

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("CountryCode", "CountryCode", "National annex of calculation code.\nConnect 'ValueList' to get the options.\nB/COMMON/D/DK/E/EST/FIN/GB/H/LT/N/NL/PL/RO/S/TR\n\nNote: the TR (Turkish) national annex is no longer supported by FEM-Design.", GH_ParamAccess.item, "S");
            pManager.AddGenericParameter("LoadGroups", "LoadGroups", "Load groups to convert in load combinations", GH_ParamAccess.list);
            pManager.AddTextParameter("CombinationMethod", "CombinationMethod", "Connect 'ValueList' to get the options.\nCombination Method type:\nEN 1990 6.4.3(6.10)\nEN 1990 6.4.3(6.10.a, b)", GH_ParamAccess.item, "EN 1990 6.4.3(6.10)");
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("fU", "fU", "", GH_ParamAccess.item, true);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("fUa", "fUa", "", GH_ParamAccess.item, true);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("fUs", "fUs", "", GH_ParamAccess.item, true);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("fSq", "fSq", "", GH_ParamAccess.item, true);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("fSf", "fSf", "", GH_ParamAccess.item, true);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("fSc", "fSc", "", GH_ParamAccess.item, true);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("fSeisSigned", "fSeisSigned", "", GH_ParamAccess.item, true);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("fSeisTorsion", "fSeisTorsion", "", GH_ParamAccess.item, true);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("fSeisZdir", "fSeisZdir", "", GH_ParamAccess.item, false);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("fSkipMinDL", "fSkipMinDL", "", GH_ParamAccess.item, true);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("fForceTemp", "fForceTemp", "", GH_ParamAccess.item, true);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("fShortName", "fShortName", "", GH_ParamAccess.item, true);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.Register_GenericParam("LoadCombination", "LoadCombination", "");
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var loadGroups = new List<Loads.ModelGeneralLoadGroup>();
            DA.GetDataList("LoadGroups", loadGroups);

            if (loadGroups.Count == 0)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Input parameter 'LoadGroups' failed to collect data");
                return;
            }

            var _loadCases = new List<FemDesign.Loads.LoadCase>();

            foreach (var loadGroup in loadGroups)
            {
                if (loadGroup.ModelLoadGroupPermanent != null)
                {
                    _loadCases.AddRange(loadGroup.ModelLoadGroupPermanent.LoadCase);
                }
                else if (loadGroup.ModelLoadGroupTemporary != null)
                {
                    _loadCases.AddRange(loadGroup.ModelLoadGroupTemporary.LoadCase);
                }
            }

            _loadCases = _loadCases.Distinct().ToList();

            string combinationMethod = "EN 1990 6.4.3(6.10)";
            DA.GetData("CombinationMethod", ref combinationMethod);

            LoadCombinationMethod _combinationMethod = FemDesign.GenericClasses.EnumParser.Parse<LoadCombinationMethod>(combinationMethod);

            string _countryCode = "S";
            DA.GetData("CountryCode", ref _countryCode);

            Country countryCode = EnumParser.Parse<Country>(_countryCode);

            var fU = true;
            DA.GetData("fU", ref fU);

            var fUa = true;
            DA.GetData("fUa", ref fUa);

            var fUs = true;
            DA.GetData("fUs", ref fUs);

            var fSq = true;
            DA.GetData("fSq", ref fSq);

            var fSf = true;
            DA.GetData("fSf", ref fSf);

            var fSc = true;
            DA.GetData("fSc", ref fSc);

            var fSeisSigned = true;
            DA.GetData("fSeisSigned", ref fSeisSigned);

            var fSeisTorsion = true;
            DA.GetData("fSeisTorsion", ref fSeisTorsion);

            var fSeisZdir = false;
            DA.GetData("fSeisZdir", ref fSeisZdir);

            var fSkipMinDL = true;
            DA.GetData("fSkipMinDL", ref fSkipMinDL);

            var fForceTemp = true;
            DA.GetData("fForceTemp", ref fForceTemp);

            var fShortName = true;
            DA.GetData("fShortName", ref fShortName);

            var loadCombinations = new List<FemDesign.Loads.LoadCombination>();


            #region Set Current Directory
            bool fileExist = OnPingDocument().IsFilePathDefined;
            if (!fileExist)
            {
                // hops issue
                //var folderPath = System.IO.Directory.GetCurrentDirectory();
                string tempPath = System.IO.Path.GetTempPath();
                System.IO.Directory.SetCurrentDirectory(tempPath);
            }
            else
            {
                var filePath = OnPingDocument().FilePath;
                var currentDir = System.IO.Path.GetDirectoryName(filePath);
                System.IO.Directory.SetCurrentDirectory(currentDir);
            }
            #endregion


            // Create Task
            var t = Task.Run(() =>
            {
                var connection = new FemDesignConnection(minimized: true);

                var model = new Model(countryCode, loadCases: _loadCases, loadGroups: loadGroups);
                model.Entities.Loads.LoadGroupTable.SimpleCombinationMethod = _combinationMethod;

                connection.Open(model);

                connection.LoadGroupToLoadComb(fU, fUa, fUs, fSq, fSf, fSc, fSeisSigned, fSeisTorsion, fSeisZdir, fSkipMinDL, fForceTemp, fShortName);

                loadCombinations = connection.GetLoadCombinations().Values.ToList();

                // Close FEM-Design
                connection.Dispose();
            });

            t.ConfigureAwait(false);
            try
            {
                t.Wait();
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }

            DA.SetDataList("LoadCombination", loadCombinations);
        }

        protected override void BeforeSolveInstance()
        {
            ValueListUtils.UpdateValueLists(this, 0, Enum.GetNames(typeof(Country)).ToList(), null, GH_ValueListMode.DropDown);

            ValueListUtils.UpdateValueLists(this, 2, new List<string> { "EN 1990 6.4.3(6.10)", "EN 1990 6.4.3(6.10.a, b)" }, null, GH_ValueListMode.DropDown);
        }


        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.LoadGroupToComb;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("{A5D427E1-B928-4684-A8B4-9E1353FF46CB}"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.senary;
    }
}