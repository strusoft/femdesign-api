﻿// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using System.Linq;
using FemDesign.GenericClasses;
using Grasshopper.Kernel.Special;
using FemDesign.Grasshopper.Extension.ComponentExtension;
using FemDesign.Bars;
using FemDesign.Geometry;

namespace FemDesign.Grasshopper
{
    public class ModelConstruct : FEM_Design_API_Component
    {
        public ModelConstruct() : base("Model.Construct", "Construct", "Construct new model. Add entities to model. Nested lists are not supported.", CategoryName.Name(), SubCategoryName.Cat6())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("CountryCode", "CountryCode", "National annex of calculation code.\nConnect 'ValueList' to get the options.\nD,DK,EST,FIN,GB,H,N,PL,RO,S,TR,NL", GH_ParamAccess.item, "S");
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("Structure Elements", "Elements", "Single structure element or list of structure elements to add. Nested lists are not supported.", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("Loads", "Loads", "Single PointLoad, LineLoad, SurfaceLoad or PressureLoad element or list of PointLoad, LineLoad, SurfaceLoad or PressureLoad to add. Nested lists are not supported.", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("LoadCases", "LoadCases", "Single LoadCase element or list of LoadCase elements to add. Nested lists are not supported.", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("LoadCombinations", "LoadCombinations", "Single LoadCombination element or list of LoadCombination elements to add. Nested lists are not supported.", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("LoadGroups", "LoadGroups", "Single LoadGroup element or list of LoadGroup elements to add. Nested lists are not supported.", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("Soil", "Soil", "Single Soil element. FEM-Design can only have one soil element in a model.", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("Stages", "Stages", "List of Stages to add. Minimum number of stages is two. Nested lists are not supported.", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Model", "Model", "Model.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // Get indata
            string countryCode = "S";
            DA.GetData("CountryCode", ref countryCode);

            List<FemDesign.GenericClasses.IStructureElement> elements = new List<FemDesign.GenericClasses.IStructureElement>();
            DA.GetDataList("Structure Elements", elements);

            List<FemDesign.GenericClasses.ILoadElement> loads = new List<FemDesign.GenericClasses.ILoadElement>();
            DA.GetDataList("Loads", loads);

            List<FemDesign.Loads.LoadCase> loadCases = new List<FemDesign.Loads.LoadCase>();
            DA.GetDataList("LoadCases", loadCases);

            List<FemDesign.Loads.LoadCombination> loadCombinations = new List<FemDesign.Loads.LoadCombination>();
            DA.GetDataList("LoadCombinations", loadCombinations);

            List<FemDesign.Loads.ModelGeneralLoadGroup> loadGroups = new List<FemDesign.Loads.ModelGeneralLoadGroup>();
            DA.GetDataList("LoadGroups", loadGroups);

            var stages = new List<FemDesign.Stage>();
            DA.GetDataList("Stages", stages);

            ConstructionStages constructionStage = null;

            if (stages.Count != 0)
            {
                constructionStage = new ConstructionStages(
                    stages,
                    assignModifedElement: false,
                    assignNewElement: false);
            }


            List<FemDesign.Soil.SoilElements> _soil = new List<FemDesign.Soil.SoilElements>();
            DA.GetDataList("Soil", _soil);

            if(_soil.Count > 1)
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "FEM-Design can only have one soil element in a model.");

            Soil.SoilElements soil = null;

            if (_soil.Count != 0)
                soil = _soil[0];

            if(loadCases == null || loadCases.Count == 0)
            {
                string message = "LoadCase must be connected to get loads, load combinations or load groups in FEM-Design.";
                if(loads.Count != 0)
                {
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, message);
                    return;
                }
                else if(loadCombinations.Count != 0)
                {
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, message);
                    return;
                }
                else if (loadGroups.Count != 0)
                {
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, message);
                    return;
                }
            }

            // Create model
            Model model = new Model(EnumParser.Parse<Country>(countryCode), elements, loads, loadCases, loadCombinations, loadGroups, constructionStage, soil);

            // check bounding box
            if (GeomUtility.GetBoundingBox(elements).Diagonal.Length >= 500)
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Bounding Box model is larger than 500 meters. Ensure your model is set up in meters.");

            DA.SetData("Model", model);
        }

        protected override void BeforeSolveInstance()
        {
            ValueListUtils.UpdateValueLists(this, 0, Enum.GetNames(typeof(Country)).ToList() , null, GH_ValueListMode.DropDown);
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.ModelCreate;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("{A0E6EA1A-1A75-4C16-B5D8-831E2ECA4E35}"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;

    }
}