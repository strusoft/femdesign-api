// https://strusoft.com/
using System;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class LoadGroupDeconstruct: GH_Component
    {
       public LoadGroupDeconstruct(): base("LoadGroup.Deconstruct", "Deconstruct", "Deconstruct a LoadGroup.", "FEM-Design", "Deconstruct")
       {

       }
       protected override void RegisterInputParams(GH_InputParamManager pManager)
       {
           pManager.AddGenericParameter("LoadGroup", "LoadGroup", "LoadGroup.", GH_ParamAccess.item);           
       } 
       protected override void RegisterOutputParams(GH_OutputParamManager pManager)
       {
            pManager.AddTextParameter("Name", "Name", "Name.", GH_ParamAccess.item);
            pManager.AddTextParameter("Type", "Type", "Type.", GH_ParamAccess.item);
            pManager.AddTextParameter("LoadCaseRelationship", "LoadCaseRelationship", "LoadCaseRelationship.", GH_ParamAccess.item);
            pManager.AddTextParameter("LoadCases", "LoadCases", "The guids of the LoadCases", GH_ParamAccess.list);
            pManager.AddBooleanParameter("ConsiderInMaxOfLoadGroups", "ConsiderInMaxOfLoadGroups",
                                         "True if groups is considered in max f load groups.", GH_ParamAccess.item);
            pManager.AddTextParameter("Guid", "Guid", "Guid.", GH_ParamAccess.item);
            pManager.AddNumberParameter("StandardFavourable", "StandardFavourable", "StandardFavourable", GH_ParamAccess.item);
            pManager.AddNumberParameter("StandardUnfavourable", "StandardUnfavourable", "StandardUnfavourable", GH_ParamAccess.item);
            pManager.AddNumberParameter("AccidentalFavourable", "AccidentalFavourable", "AccidentalFavourable", GH_ParamAccess.item);
            pManager.AddNumberParameter("AccidentalUnfavourable", "AccidentalUnfavourable", "AccidentalUnfavourable", GH_ParamAccess.item);
            pManager.AddNumberParameter("Xi", "Xi", "Xi", GH_ParamAccess.item);
            pManager.AddNumberParameter("SafetyFactor", "SafetyFactor", "SafetyFactor", GH_ParamAccess.item);
            pManager.AddNumberParameter("Psi0", "Psi0", "Psi0", GH_ParamAccess.item);
            pManager.AddNumberParameter("Psi1", "Psi1", "Psi1", GH_ParamAccess.item);
            pManager.AddNumberParameter("Psi2", "Psi2", "Psi2", GH_ParamAccess.item);
            pManager.AddBooleanParameter("PotentiallyLeadingCases", "PotentiallyLeadingCases", "PotentiallyLeadingCases", GH_ParamAccess.item);
            pManager.AddBooleanParameter("IgnoreInSLSCombinations", "IgnoreInSLSCombinations", "IgnoreInSLSCombinations", GH_ParamAccess.item);
        }
       protected override void SolveInstance(IGH_DataAccess DA)
       {
            // get input
            FemDesign.Loads.ModelGeneralLoadGroup obj = null;
            if (!DA.GetData(0, ref obj))
            {
                return;
            }
            if (obj == null)
            {
                return;
            }

            // Get the properties associated to the specific load group
            double? standardFavourable = null;
            double? standardUnfavourable = null;
            double? accidentalFavourable = null;
            double? accidentalUnfavourable = null;
            double? xi = null;
            double? safetyFactor = null;
            double? psi0 = null;
            double? psi1 = null;
            double? psi2 = null;
            bool? potentiallyLeadingCases = null;
            bool? ignoreInSLS = null;

            if (obj.GetLoadGroupType() == Loads.ELoadGroupType.Permanent)
            {
                standardFavourable = ((Loads.LoadGroupPermanent)obj.GetSpecificLoadGroup()).StandardFavourable;
                standardUnfavourable = ((Loads.LoadGroupPermanent)obj.GetSpecificLoadGroup()).StandardUnfavourable;
                accidentalFavourable = ((Loads.LoadGroupPermanent)obj.GetSpecificLoadGroup()).AccidentalFavourable;
                accidentalUnfavourable = ((Loads.LoadGroupPermanent)obj.GetSpecificLoadGroup()).AccidentalUnfavourable;
                xi = ((Loads.LoadGroupPermanent)obj.GetSpecificLoadGroup()).Xi;
            }
            else if(obj.GetLoadGroupType() == Loads.ELoadGroupType.Temporary)
            {
                safetyFactor = ((Loads.LoadGroupTemporary)obj.GetSpecificLoadGroup()).SafetyFactor;
                psi0 = ((Loads.LoadGroupTemporary)obj.GetSpecificLoadGroup()).Psi0;
                psi1 = ((Loads.LoadGroupTemporary)obj.GetSpecificLoadGroup()).Psi1;
                psi2 = ((Loads.LoadGroupTemporary)obj.GetSpecificLoadGroup()).Psi2;
                potentiallyLeadingCases = ((Loads.LoadGroupTemporary)obj.GetSpecificLoadGroup()).LeadingCases;
                ignoreInSLS = ((Loads.LoadGroupTemporary)obj.GetSpecificLoadGroup()).IgnoreSls;
            }
            
            // return
            DA.SetData(0, obj.Name);
            DA.SetData(1, obj.GetLoadGroupType().ToString());
            DA.SetData(2, obj.GetSpecificLoadGroup().Relationship.ToString());
            DA.SetDataList(3, obj.GetLoadCaseGuidsAsString());
            DA.SetData(4, obj.ConsiderInGmax);
            DA.SetData(5, obj.Guid);
            DA.SetData(6, standardFavourable);
            DA.SetData(7, standardUnfavourable);
            DA.SetData(8, accidentalFavourable);
            DA.SetData(9, accidentalUnfavourable);
            DA.SetData(10, xi);
            DA.SetData(11, safetyFactor);
            DA.SetData(12, psi0);
            DA.SetData(13, psi1);
            DA.SetData(14, psi2);
            DA.SetData(15, potentiallyLeadingCases);
            DA.SetData(16, ignoreInSLS);
        }
       protected override System.Drawing.Bitmap Icon
       {
           get
           {
                return FemDesign.Properties.Resources.LoadGroupDeconstruct;
           }
       }
       public override Guid ComponentGuid
       {
           get { return new Guid("e3bdb4d8-6b64-47bc-a3ce-37aa75af4673"); }
       }

        public override GH_Exposure Exposure => GH_Exposure.quarternary;

    }
}