
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.Grasshopper
{
    public class LoadGroupCreate : GH_Component
    {
        public LoadGroupCreate() : base("LoadGroup.Create", "Create", "Creates a load group.", "FemDesign", "Loads")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Name", "Name", "Name of LoadGroup.", GH_ParamAccess.item);
            pManager.AddTextParameter("Type", "Type", "LoadGroup type. permanent/variable.", GH_ParamAccess.item, "permanent");
            pManager[1].Optional = true;
            pManager.AddGenericParameter("LoadCase", "LoadCase", "LoadCase to include in LoadGroup. Single LoadCase or list of LoadCases.", GH_ParamAccess.list);
            pManager.AddNumberParameter("Psi", "Psi", "Psi values to apply when combining loads.", GH_ParamAccess.list);
            pManager.AddNumberParameter("Gamma_d", "Gamma_d", "Partialkoefficient för säkerhetsklass.", GH_ParamAccess.item);
            pManager.AddNumberParameter("SafetyFactor", "SafetyFactor", "Safety factor to multiply load with.", GH_ParamAccess.item);
            pManager.AddNumberParameter("LoadCaseRelation", "LoadCaseRelation", "Specifies how the load cases are related", GH_ParamAccess.item);
            pManager.AddNumberParameter("Xi", "Xi", "Factor to multiply permanent load with (only needed for permanent loads).", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("LoadGroup", "LoadGroup", "LoadGroup.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get data
            string name = null, type = "permanent";
            double gamma_d = 0, safetyFactor = 0, xi = 0;
            List<FemDesign.Loads.LoadCase> loadCases = new List<FemDesign.Loads.LoadCase>();
            List<double> psi = new List<double>();
            Loads.ELoadGroupRelation loadCaseRelation = Loads.ELoadGroupRelation.Correlated;

            if (!DA.GetData(0, ref name)) { return; }
            if (!DA.GetData(1, ref type))
            {
                // pass
            }
            if (!DA.GetData(4, ref gamma_d)) { return; }
            if (!DA.GetData(5, ref safetyFactor)) { return; }
            if (!DA.GetData(6, ref loadCaseRelation)) { return; }
            if (!DA.GetData(7, ref xi))
            {
                // pass
            }

            if (!DA.GetDataList(2, loadCases)) { return; }
            if (!DA.GetDataList(3, psi)) { return; }
            if (name == null || type == null || psi == null) { return; }

            FemDesign.Loads.LoadGroup obj = new FemDesign.Loads.LoadGroup();

            //
            if (type == "permanent")
                obj = new FemDesign.Loads.LoadGroup(name, Loads.ELoadGroupType.Permanent, loadCases, psi, gamma_d, safetyFactor, loadCaseRelation, xi);
            else
                obj = new FemDesign.Loads.LoadGroup(name, Loads.ELoadGroupType.Variable, loadCases, psi, gamma_d, safetyFactor, loadCaseRelation);

            // return
            DA.SetData(0, obj);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.LoadCombinationDefine;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("ebf804c1-91a6-40bb-adee-5a02a9e42f81"); }
        }
    }
}