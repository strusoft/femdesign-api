// https://strusoft.com/
using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.Grasshopper
{
    public class BarDeconstructModify : FEM_Design_API_Component
    {
       public BarDeconstructModify(): base("Bar.Deconstruct.Modify", "Deconstruct.Modify", "Deconstruct and modify properties of an exiting bar element of any type.", CategoryName.Name(),
            SubCategoryName.Cat2a())
        {

       }
       protected override void RegisterInputParams(GH_InputParamManager pManager)
       {
           pManager.AddGenericParameter("Bar", "Bar", "Bar element", GH_ParamAccess.item);
           pManager.AddBooleanParameter("NewGuid", "NewGuid", "Generate a new guid for this bar?", GH_ParamAccess.item);
           pManager[pManager.ParamCount - 1].Optional = true;
           pManager.AddCurveParameter("Curve", "Curve", "LineCurve or ArcCurve", GH_ParamAccess.item);
           pManager[pManager.ParamCount - 1].Optional = true;
           pManager.AddGenericParameter("Material", "Material", "Material.", GH_ParamAccess.item);
           pManager[pManager.ParamCount - 1].Optional = true;
           pManager.AddGenericParameter("Section", "Section", "Section. If 1 item this item defines both start and end. If two items the first item defines the start and the last item defines the end.", GH_ParamAccess.list);
           pManager[pManager.ParamCount - 1].Optional = true;
           pManager.AddGenericParameter("Connectivity", "Connectivity", "Connectivity. If 1 item this item defines both start and end. If two items the first item defines the start and the last item defines the end. Optional, default value if undefined.", GH_ParamAccess.list);
           pManager[pManager.ParamCount - 1].Optional = true;
           pManager.AddGenericParameter("Eccentricity", "Eccentricity", "Eccentricity. If 1 item this item defines both start and end. If two items the first item defines the start and the last item defines the end. Optional, default value if undefined.", GH_ParamAccess.list);
           pManager[pManager.ParamCount - 1].Optional = true;
           pManager.AddVectorParameter("LocalY", "LocalY", "Set local y-axis. Vector must be perpendicular to Curve mid-point local x-axis. This parameter overrides OrientLCS", GH_ParamAccess.item);
           pManager[pManager.ParamCount - 1].Optional = true;
           pManager.AddGenericParameter("Stirrups", "Stirrups", "Stirrups to add to bar. Item or list. New reinforcement will overwrite the original.", GH_ParamAccess.list);
           pManager[pManager.ParamCount - 1].Optional = true;
           pManager.AddGenericParameter("LongitudinalBars", "LongBars", "Longitudinal reinforcement to add to bar. Item or list. New reinforcement will overwrite the original.", GH_ParamAccess.list);
           pManager[pManager.ParamCount - 1].Optional = true;
           pManager.AddGenericParameter("PTC", "PTC", "Post-tensioning cables. New PTC will overwrite the original.", GH_ParamAccess.list);
           pManager[pManager.ParamCount - 1].Optional = true;
           pManager.AddGenericParameter("StiffnessModifier", "StiffnessModifier", "", GH_ParamAccess.item);
           pManager[pManager.ParamCount - 1].Optional = true;
           pManager.AddTextParameter("Identifier", "Identifier", "Identifier. Optional, default value if undefined.", GH_ParamAccess.item);
           pManager[pManager.ParamCount - 1].Optional = true;

       } 
       protected override void RegisterOutputParams(GH_OutputParamManager pManager)
       {
           pManager.AddGenericParameter("Bar", "Bar", "Bar.", GH_ParamAccess.item);
           pManager.AddTextParameter("Guid", "Guid", "Guid.", GH_ParamAccess.item);
           pManager.AddCurveParameter("Curve", "Curve", "LineCurve or ArcCurve [m]", GH_ParamAccess.item);
           pManager.AddGenericParameter("Material", "Material", "Material", GH_ParamAccess.item);
           pManager.AddGenericParameter("Section", "Section", "Section", GH_ParamAccess.list);
           pManager.AddGenericParameter("Connectivity", "Connectivity", "Connectivity", GH_ParamAccess.list);
           pManager.AddGenericParameter("Eccentricity", "Eccentricity", "Eccentricity", GH_ParamAccess.list);
           pManager.AddGenericParameter("LocalY", "LocalY", "LocalY", GH_ParamAccess.item);
           pManager.AddGenericParameter("Stirrups", "Stirrups", "Stirrup bar reinforcement.", GH_ParamAccess.list);
           pManager.AddGenericParameter("LongitudinalBars", "LongBars", "Longitudinal reinforcement for bar.", GH_ParamAccess.list);
           pManager.AddGenericParameter("PTC", "PTC", "Post-tensioning cables.", GH_ParamAccess.list);
           pManager.AddGenericParameter("StiffnessModifier", "StiffnessModifier", "", GH_ParamAccess.item);
           pManager.AddTextParameter("Identifier", "Identifier", "Structural element ID.", GH_ParamAccess.item);
        }
       protected override void SolveInstance(IGH_DataAccess DA)
       {
            // get input
            Bars.Bar bar = null;
            if (!DA.GetData(0, ref bar))
            {
                return;
            }
            else if (bar == null)
            {
                return;
            }
            else
            {
                if (bar.BarPart.HasComplexCompositeRef || bar.BarPart.HasDeltaBeamComplexSectionRef)
                {
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "The bar has a Composite Section. The object has not been implemented yet. Please, get in touch if needed.");
                    return;
                }

                bar = bar.DeepClone();
            }

            bool newGuid = false;
            if (DA.GetData(1, ref newGuid))
            {
                if (newGuid)
                {
                    bar.EntityCreated();
                    bar.BarPart.EntityCreated();

                    for (int i = 0; i < bar.Reinforcement.Count; i++)
                    {
                        bar.Reinforcement[i].BaseBar.Guid = bar.BarPart.Guid;
                    }
                    for (int i = 0; i < bar.Ptc.Count; i++)
                    {
                        bar.Ptc[i].BaseObject = bar.BarPart.Guid;
                    }
                }
            }

            Curve curve = null;
            if (DA.GetData(2, ref curve))
            {
                // convert geometry 
                FemDesign.Geometry.Edge edge = Convert.FromRhinoLineOrArc2(curve);

                // update edge
                bar.BarPart.Edge = edge;
            }
            
            FemDesign.Materials.Material material = null;
            if (DA.GetData(3, ref material))
            {
                if (material.Family != bar.BarPart.ComplexMaterialObj.Family)
                {
                    if (bar.BarPart.BucklingData != null && bar.BarPart.BucklingData.BucklingLength != null)
                    {
                        
                        AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "The bar's buckling data was created for another material and might not correspond to the new material. If you change the material you need to change the buckling length's properties.");
                    }
                    if (bar.Reinforcement.Any() || bar.Ptc.Any())
                    {
                        AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "The bar already has reinforcement or PTC. Material has changed. The reinforcement and PTC have been removed.");
                        bar.Reinforcement.Clear();
                        bar.Ptc.Clear();
                    }
                }

                bar.BarPart.ComplexMaterialObj = material;
            }

            List<FemDesign.Sections.Section> sections = new List<Sections.Section>();
            if (DA.GetDataList(4, sections))
            {
                if (bar.Type != Bars.BarType.Truss)
                {
                    bar.BarPart.ComplexSectionObj.Sections = sections.ToArray();
                }
                else
                {
                    bar.BarPart.TrussUniformSectionObj = sections[0];
                    if(sections.Count > 1)
                    {
                        AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "In FEM-Design, it is not possible to set a variable cross section for truss. The first value will be selected.");
                    }
                }
            }

            List<FemDesign.Bars.Connectivity> connectivities = new List<Bars.Connectivity>();
            if (DA.GetDataList(5, connectivities))
            {
                bar.BarPart.Connectivity = connectivities.ToArray();
            }

            List<FemDesign.Bars.Eccentricity> eccentricities = new List<Bars.Eccentricity>();
            if (DA.GetDataList(6, eccentricities))
            {
                if(bar.Type != Bars.BarType.Truss)
                {
                    bar.BarPart.ComplexSectionObj.Eccentricities = eccentricities.ToArray();
                }
                else
                {
                    throw new System.Exception("Truss has no eccentricity.");
                }
            }
            
            Vector3d v = Vector3d.Zero;
            if (DA.GetData(7, ref v))
            {
                bar.BarPart.LocalY = v.FromRhino();
            }

            List<FemDesign.Reinforcement.StirrupReinforcement> stirrups = new List<Reinforcement.StirrupReinforcement>();
            List<FemDesign.Reinforcement.LongitudinalBarReinforcement> longBars = new List<Reinforcement.LongitudinalBarReinforcement>();
            DA.GetDataList(8, stirrups);
            DA.GetDataList(9, longBars);
            List<FemDesign.Reinforcement.BarReinforcement> barReinf = new List<Reinforcement.BarReinforcement>();
            if (stirrups.Count != 0)
            {
                var clonedStirrups = stirrups.Select(x => x.DeepClone()).ToList();
                clonedStirrups.ForEach(s => barReinf.Add(s));
                bar.Reinforcement.RemoveAll(x => x.IsStirrups);
            }
            if (longBars.Count != 0)
            {
                var clonedLongBars = longBars.Select(x => x.DeepClone()).ToList();
                clonedLongBars.ForEach(l => barReinf.Add(l));
                bar.Reinforcement.RemoveAll(x => !x.IsStirrups);
            }
            if (barReinf.Any())
            {
                bar = FemDesign.Reinforcement.BarReinforcement.AddReinforcementToBar(bar, barReinf, true);
            }

            List<FemDesign.Reinforcement.Ptc> ptc = new List<FemDesign.Reinforcement.Ptc>();
            if(DA.GetDataList(10, ptc))
            {
                var clonedPtc = ptc.Select(x => x.DeepClone()).ToList();
                bar.Ptc.Clear();
                bar = FemDesign.Reinforcement.Ptc.AddPtcToBar(bar, clonedPtc, true);
            }

            Bars.BarStiffnessFactors stiffnessFactors = null;
            if (DA.GetData(11, ref stiffnessFactors)) 
            {
                bar.BarPart.StiffnessModifiers = new List<Bars.BarStiffnessFactors>() { stiffnessFactors };
            }

            string identifier = null;
            if (DA.GetData(12, ref identifier))
            {
                bar.Identifier = identifier;
            }


            // output

            // The following code is to convert 'item' to 'list object'
            // It is required to construct the bar without graftening the data
            var materialList = new List<object>() { bar.BarPart.ComplexMaterialObj };

            DA.SetData(0, bar);
            DA.SetData(1, bar.Guid);
            DA.SetData(2, bar.GetRhinoCurve());
            DA.SetDataList(3, materialList);

            if (bar.BarPart.ComplexSectionObj != null)
            {
                DA.SetDataList(4, bar.BarPart.ComplexSectionObj.Sections);
            }
            else if (bar.BarPart.HasComplexCompositeRef || bar.BarPart.HasDeltaBeamComplexSectionRef)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "The bar has a Composite Section. The object has not been implemented yet. Please, get in touch if needed.");
                DA.SetDataList(4, null);
            }
            else if (bar.BarPart.Type == Bars.BarType.Truss)
            {
                var truss = new List<Sections.Section> { bar.BarPart.TrussUniformSectionObj };
                DA.SetDataList(4, truss);
            }
            else
            {
                DA.SetDataList(4, null);
            }

            DA.SetDataList(5, bar.BarPart.Connectivity);

            var result = (bar.BarPart.ComplexSectionObj != null) ? bar.BarPart.ComplexSectionObj.Eccentricities : null;
            DA.SetDataList(6, result);

            DA.SetData(7, bar.BarPart.LocalY.ToRhino());

            List<FemDesign.Reinforcement.StirrupReinforcement> stirrupsOut = new List<FemDesign.Reinforcement.StirrupReinforcement>();
            List<FemDesign.Reinforcement.LongitudinalBarReinforcement> longBarOut = new List<FemDesign.Reinforcement.LongitudinalBarReinforcement>();
            foreach (Reinforcement.BarReinforcement reinf in bar.Reinforcement)
            {
                if (reinf.IsStirrups)
                {
                    stirrupsOut.Add(new Reinforcement.StirrupReinforcement(reinf));
                }
                else
                {
                    longBarOut.Add(new Reinforcement.LongitudinalBarReinforcement(reinf));
                }
            }
            DA.SetDataList(8, stirrupsOut);
            DA.SetDataList(9, longBarOut);
            

            if ((bar.Type != FemDesign.Bars.BarType.Truss) && (bar.BarPart.ComplexSectionObj.Sections[0] != bar.BarPart.ComplexSectionObj.Sections[1]) && bar.Reinforcement.Any())
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "In FEM-Design, it is not possible to create reinforcement for bars with variable cross sections.");
            }

            DA.SetDataList(10, bar.Ptc);
            DA.SetData(11, bar.BarPart.StiffnessModifiers);
            DA.SetData(12, bar.Name);
        }
        protected override System.Drawing.Bitmap Icon
       {
           get
           {
                return FemDesign.Properties.Resources.BarModify;
           }
       }
       public override Guid ComponentGuid
       {
           get { return new Guid("3A6FD9BA-6EBB-4822-B1A8-B3E3E297ED99"); }
       }
        public override GH_Exposure Exposure => GH_Exposure.secondary;

    }
}