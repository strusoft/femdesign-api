
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Rhino.Geometry;

namespace FemDesign.Grasshopper
{
    public class LoadGroupConstruct : GH_Component
    {
        public LoadGroupConstruct() : base("LoadGroup.Construct", "Construct", "Construct a load group.", CategoryName.Name(), SubCategoryName.Cat3())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Name", "Name", "Name of LoadGroup.", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Type", "Type", "LoadGroup type.", GH_ParamAccess.item, 0);
            pManager.AddGenericParameter("LoadCase", "LoadCase", "LoadCase to include in LoadGroup. Single LoadCase or list of LoadCases.", GH_ParamAccess.list);
            pManager.AddGenericParameter("LoadCategory", "LoadCategory", "Psi (\u03A8) values to apply when combining loads (not needed for permanent group).", GH_ParamAccess.item);
            pManager.AddNumberParameter("StandardUnfavourableSafetyFactor", "StandardUnfavourableSafetyFactor", "Unfavourable safety factor to multiply load with. [-]", GH_ParamAccess.item);
            pManager.AddNumberParameter("StandardFavourableSafetyFactor", "StandardFavourableSafetyFactor", "Favourable safety factor to multiply load with  (only needed for permanent loads). [-]", GH_ParamAccess.item, 1);
            pManager.AddNumberParameter("AccidentalUnfavourableSafetyFactor", "AccidentalUnfavourableSafetyFactor", "Unfavourable safety factor to multiply load with (for accidental combinations). [-]", GH_ParamAccess.item, 1);
            pManager.AddNumberParameter("AccidentalFavourableSafetyFactor", "AccidentalFavourableSafetyFactor", "Favourable safety factor to multiply load with (for accidental combinations). [-]", GH_ParamAccess.item, 1);
            pManager.AddIntegerParameter("LoadCaseRelationship", "LoadCaseRelationship", "Specifies how the load cases are related.", GH_ParamAccess.item, 0);
            pManager.AddNumberParameter("Xi", "Xi", "Factor to multiply permanent load with (only needed for permanent loads). [-]", GH_ParamAccess.item);
            pManager.AddIntegerParameter("PotentiallyLeadingAction", "PotentiallyLeadingAction", "True if the load cases in the group can be leading actions.", GH_ParamAccess.item, 1);

            // Named values instead of integers
            Param_Integer type = pManager[1] as Param_Integer;
            type.AddNamedValue("permanent", 0);
            type.AddNamedValue("temporary", 1);

            Param_Integer loadCaseRelation = pManager[8] as Param_Integer;
            loadCaseRelation.AddNamedValue("entire", 0);
            loadCaseRelation.AddNamedValue("alternative", 1);

            Param_Integer potentiallyLeadingAction = pManager[10] as Param_Integer;
            potentiallyLeadingAction.AddNamedValue("False", 0);
            potentiallyLeadingAction.AddNamedValue("True", 1);

            // Set optional parameters
            List<int> optionalParameters = new List<int>() { 1, 3, 5, 6, 7, 9, 10 };
            foreach (int parameter in optionalParameters)
                pManager[parameter].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("LoadGroup", "LoadGroup", "LoadGroup.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get data
            string name = null;
            double unfavourableSafetyFactor = 0, favourableSafetyFactor = 0, unfavourableSafetyFactorAccidental = 0, favourableSafetyFactorAccidental = 0, xi = 0.89;
            int loadCaseRelation = 0, type = 0, potentiallyLeadingAction = 1;
            List<FemDesign.Loads.LoadCase> loadCases = new List<FemDesign.Loads.LoadCase>();
            FemDesign.Loads.LoadCategory psi = null;

            if (!DA.GetData(0, ref name)) { return; }
            DA.GetData(1, ref type);
            if (!DA.GetDataList(2, loadCases)) { return; }
            DA.GetData(3, ref psi);
            if (!DA.GetData(4, ref unfavourableSafetyFactor)) { return; }
            DA.GetData(5, ref favourableSafetyFactor);
            DA.GetData(6, ref unfavourableSafetyFactorAccidental);
            DA.GetData(7, ref favourableSafetyFactorAccidental);
            if (!DA.GetData(8, ref loadCaseRelation)) { return; }
            if (!DA.GetData(9, ref xi))
            {
                if(type == 0)
                    throw new System.ArgumentException("Must provide Xi value for permanent load group");                   
            }
            if(!DA.GetData(10, ref potentiallyLeadingAction))
            {
                if (type == 1)
                    throw new System.ArgumentException("Must specify if group is potentially leading action for temporary load group");
            }

            if (name == null)
                if(psi == null || type != 0){ return; }

            if (loadCaseRelation == 1 && type == 0)
                throw new System.ArgumentException("Alternative relationsship not yet implemented for permanent groups!");

            // Convert the load case relation to an enum
            Loads.ELoadGroupRelationship loadCaseRelationEnum = Loads.ELoadGroupRelationship.Entire;
            if(loadCaseRelation == 0)
                loadCaseRelationEnum = Loads.ELoadGroupRelationship.Entire;
            else if(loadCaseRelation == 1)
                loadCaseRelationEnum = Loads.ELoadGroupRelationship.Alternative;

            // Convert 0 and 1 to booleans
            Boolean potentiallyLeadingActionBool = true;
            if (potentiallyLeadingAction == 0)
                potentiallyLeadingActionBool = false;
                

            // Create load group object
            Loads.LoadGroupBase obj = null;
            if (type == 0)
                obj = new Loads.LoadGroupPermanent(favourableSafetyFactor, unfavourableSafetyFactor, favourableSafetyFactorAccidental, 
                                                   unfavourableSafetyFactorAccidental, loadCases, loadCaseRelationEnum, xi, name);
            else if (type == 1)
                obj = new Loads.LoadGroupTemporary(unfavourableSafetyFactor, psi.Psi0, psi.Psi1, psi.Psi2, potentiallyLeadingActionBool, 
                                                   loadCases, loadCaseRelationEnum, name);
            else
                throw new System.ArgumentException("Load group type not yet implemented");

            Loads.ModelGeneralLoadGroup loadGroup = new Loads.ModelGeneralLoadGroup(obj);
            // return
            DA.SetData(0, loadGroup);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.LoadGroup;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("ebf804c1-91a6-40bb-adee-5a02a9e42f81"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.quarternary;

    }
}