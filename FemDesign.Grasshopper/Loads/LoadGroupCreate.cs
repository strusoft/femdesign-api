
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Rhino.Geometry;

namespace FemDesign.Grasshopper
{
    public class LoadGroupCreate : GH_Component
    {
        public LoadGroupCreate() : base("CreateLoadGroup", "CreateLoadGroup", "Creates a load group.", "FemDesign", "Loads")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Name", "Name", "Name of LoadGroup.", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Type", "Type", "LoadGroup type.", GH_ParamAccess.item, 0);
            Param_Integer type = pManager[1] as Param_Integer;
            type.AddNamedValue("permanent", 0);
            type.AddNamedValue("temporary", 1);
            pManager[1].Optional = true;
            pManager.AddGenericParameter("LoadCase", "LoadCase", "LoadCase to include in LoadGroup. Single LoadCase or list of LoadCases.", GH_ParamAccess.list);
            pManager.AddNumberParameter("Psi", "Psi", "Psi values to apply when combining loads (not needed for permanent group).", GH_ParamAccess.list);
            pManager[3].Optional = true;
            pManager.AddNumberParameter("Gamma_d", "Gamma_d", "Partialkoefficient för säkerhetsklass.", GH_ParamAccess.item);
            pManager.AddNumberParameter("SafetyFactor", "SafetyFactor", "Safety factor to multiply load with.", GH_ParamAccess.item);
            pManager.AddIntegerParameter("LoadCaseRelation", "LoadCaseRelation", "Specifies how the load cases are related", GH_ParamAccess.item, 0);
            Param_Integer loadCaseRelation = pManager[6] as Param_Integer;
            loadCaseRelation.AddNamedValue("entire", 0);
            loadCaseRelation.AddNamedValue("alternative", 1);
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
            string name = null;
            double gamma_d = 0, safetyFactor = 0, xi = 0;
            int loadCaseRelation = 0, type = 0;
            List<FemDesign.Loads.LoadCase> loadCases = new List<FemDesign.Loads.LoadCase>();
            List<double> psi = new List<double>();

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
                if(type == 0)
                    throw new System.ArgumentException("Must provide Xi value for permanent load group");                   
            }
            if (!DA.GetDataList(2, loadCases)) { return; }
            if (!DA.GetDataList(3, psi))
            {
                // pass
            }
            if (name == null)
                if(psi == null || type != 0){ return; }

            // Convert the load case relation to an enum
            Loads.ELoadGroupRelation loadCaseRelationEnum = Loads.ELoadGroupRelation.Entire;
            if(loadCaseRelation == 0)
                loadCaseRelationEnum = Loads.ELoadGroupRelation.Entire;
            else if(loadCaseRelation == 1)
                loadCaseRelationEnum = Loads.ELoadGroupRelation.Alternative;
                

            // Create load group object
            FemDesign.Loads.LoadGroup obj = new Loads.LoadGroup();
            if (type == 0)
                obj = new FemDesign.Loads.LoadGroup(name, Loads.ELoadGroupType.Permanent, loadCases, gamma_d, safetyFactor, loadCaseRelationEnum, xi);
            else if (type == 1)
                obj = new FemDesign.Loads.LoadGroup(name, Loads.ELoadGroupType.Variable, loadCases, psi, gamma_d, safetyFactor, loadCaseRelationEnum);
            else
                throw new System.ArgumentException("Load group type not yet implemented");

            // return
            DA.SetData(0, obj);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.LoadGroupDefine;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("ebf804c1-91a6-40bb-adee-5a02a9e42f81"); }
        }
    }
}