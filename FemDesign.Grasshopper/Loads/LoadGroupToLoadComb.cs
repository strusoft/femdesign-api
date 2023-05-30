// https://strusoft.com/
using System;
using System.Linq;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;
using FemDesign;
using System.Threading.Tasks;
using FemDesign.Calculate;
using System.Reflection;
using Grasshopper;

namespace FemDesign.Grasshopper
{
    public class LoadGroupToLoadComb : GH_Component
    {
        public LoadGroupToLoadComb() : base("LoadGroupToLoadComb", "LoadGroupToLoadComb", "", CategoryName.Name(), SubCategoryName.Cat3())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("LoadCase", "LoadCase", "", GH_ParamAccess.list);
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
            pManager.Register_MeshParam("LoadCombination", "LoadCombination", "");
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var loadGroups = new List<Loads.LoadGroupCombineGeneral>();

            var loadCases = new List<Loads.LoadCase>();
            if( !DA.GetDataList(0, loadCases) ) return;

            var fU = true;
            DA.GetData(1, ref fU);

            var fUa = true;
            DA.GetData(2, ref fUa);

            var fUs = true;
            DA.GetData(3, ref fUs);

            var fSq = true;
            DA.GetData(4, ref fSq);

            var fSf = true;
            DA.GetData(5, ref fSf);

            var fSc = true;
            DA.GetData(6, ref fSc);

            var fSeisSigned = true;
            DA.GetData(7, ref fSeisSigned);

            var fSeisTorsion = true;
            DA.GetData(8, ref fSeisTorsion);

            var fSeisZdir = false;
            DA.GetData(9, ref fSeisZdir);

            var fSkipMinDL = true;
            DA.GetData(10, ref fSkipMinDL);

            var fForceTemp = true;
            DA.GetData(11, ref fForceTemp);

            var fShortName = true;
            DA.GetData(12, ref fShortName);

            // Create Task
            var t = Task.Run(() =>
            {
                var connection = new FemDesignConnection(minimized: true, tempOutputDir: false);

                connection.LoadGroupToLoadComb(fU, fUa, fUs, fSq, fSf, fSc, fSeisSigned, fSeisTorsion, fSeisZdir, fSkipMinDL, fForceTemp, fShortName, loadCases);

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

            // todo
            var loadCombination = new List<Loads.LoadCombination>();

            DA.SetData("LoadCombination", loadCombination);
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
            get { return new Guid("{915AFE1C-BA17-456C-A65F-0AB200F4DC6A}"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.senary;
    }
}