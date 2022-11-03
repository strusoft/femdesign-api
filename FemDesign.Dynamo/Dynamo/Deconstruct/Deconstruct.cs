using System.Collections.Generic;
using System.Linq;

#region dynamo
using Autodesk.DesignScript.Runtime;

namespace FemDesign
{
    /// <summary>
    /// Static methods from other classes are put under this class Dynamo library heirarchy reasons 
    /// so that all deconstruc methods are organized under Deconstruct.
    /// </summary>
    [IsVisibleInDynamoLibrary(false)]
    public partial class Deconstruct
    {
        /// <summary>
        /// Deconstruct an axis element
        /// </summary>
        /// <param name="axis">Axis.</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        [MultiReturn(new[]{"Guid", "Line", "Prefix", "Id", "IdIsLetter"})]
        public static Dictionary<string, object> AxisDeconstruct(FemDesign.StructureGrid.Axis axis)
        {
            return new Dictionary<string, object>
            {
                {"Guid", axis.Guid},
                {"Line", Autodesk.DesignScript.Geometry.Line.ByStartPointEndPoint(axis.StartPoint.ToDynamo(), axis.EndPoint.ToDynamo())},
                {"Prefix", axis.Prefix},
                {"Id", axis.Id},
                {"IdIsLetter", axis.IdIsLetter}
            };
        }
        /// <summary>
        /// Deconstruct a bar element.
        /// </summary>
        /// <param name="bar">Bar.</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        [MultiReturn(new[]{"Guid", "Curve", "Type", "Material", "Section", "Connectivity", "Eccentricity", "LocalY", "Stirrups", "LongitudinalBars", "PTC", "Identifier"})]
        public static Dictionary<string, object> BarDeconstruct(FemDesign.Bars.Bar bar)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();

            result.Add("Guid", bar.Guid);
            result.Add("Curve", bar.GetDynamoCurve());
            result.Add("Type", bar.Type);
            result.Add("Material", bar.BarPart.ComplexMaterialObj);

            if (bar.BarPart.ComplexSectionObj != null)
            {
                result.Add("Section", bar.BarPart.ComplexSectionObj.Sections);
            }
            else if (bar.BarPart.Type == Bars.BarType.Truss)
            {
                result.Add("Section", new List<Sections.Section> { bar.BarPart.TrussUniformSectionObj });
            }
            else if (bar.BarPart.HasComplexCompositeRef || bar.BarPart.HasDeltaBeamComplexSectionRef)
            {
                result.Add("Section", null);
                throw new System.Exception("Composite Section in the model. The object has not been implemented yet. Please, get in touch if needed.");
            }

            result.Add("Connectivity", bar.BarPart.Connectivity);

            var eccentricity = (bar.BarPart.ComplexSectionObj != null) ? bar.BarPart.ComplexSectionObj.Eccentricities : null;
            result.Add("Eccentricity", eccentricity);

            result.Add("LocalY", bar.BarPart.LocalY.ToDynamo());
            result.Add("Stirrups", bar.Stirrups);
            result.Add("LongitudinalBars", bar.LongitudinalBars);
            result.Add("PTC", bar.Ptc);
            result.Add("Identifier", bar.Identifier);


            return result;
        }

        /// <summary>
        /// Deconstruct a fictitious bar element.
        /// </summary>
        /// <param name="fictitiousBar">FictitiousBar.</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        [MultiReturn(new[]{"Guid", "AnalyticalID", "Curve", "AE", "ItG", "I1E", "I2E", "Connectivity", "LocalY"})]
        public static Dictionary<string, object> FictitiousBarDeconstruct(FemDesign.ModellingTools.FictitiousBar fictitiousBar)
        {
            return new Dictionary<string, object>
            {
                {"Guid", fictitiousBar.Guid},
                {"AnalyticalID", fictitiousBar.Name},
                {"Curve", fictitiousBar.Edge.ToDynamo()},
                {"AE", fictitiousBar.AE},
                {"ItG", fictitiousBar.ItG},
                {"I1E", fictitiousBar.I1E},
                {"I2E", fictitiousBar.I2E},
                {"Connectivity", fictitiousBar._connectivity},
                {"LocalY", fictitiousBar.LocalY.ToDynamo()}
            };
        }

        /// <summary>
        /// Deconstruct a cover.
        /// </summary>
        /// <param name="cover">Cover.</param>
        /// <returns></returns>
        [MultiReturn(new[]{"Guid", "Id", "Surface", "Contours"})]
        [IsVisibleInDynamoLibrary(true)]

        public static Dictionary<string, object> CoverDeconstruct(FemDesign.Cover cover)
        {
            return new Dictionary<string, object>
            {
                {"Guid", cover.Guid},
                {"Id", cover.Name},
                {"Surface", cover.GetDynamoSurface()},
                {"Contours", cover.GetDynamoCurves()}
            };
        }

        /// <summary>
        /// Deconstruct a PointLoad.
        /// </summary>
        /// <param name="pointLoad">PointLoad.</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        [MultiReturn(new[]{"Guid", "Type", "Point", "Direction", "q", "LoadCaseGuid", "Comment"})]
        public static Dictionary<string, object> PointLoadDeconstruct(FemDesign.Loads.PointLoad pointLoad)
        {
            return new Dictionary<string, object>
            {
                {"Guid", pointLoad.LoadCaseGuid},
                {"Type", pointLoad.LoadType},
                {"Point", pointLoad.GetDynamoGeometry()},
                {"Direction", pointLoad.Direction.ToDynamo()},
                {"q", pointLoad.Load.Value},
                {"LoadCaseGuid", pointLoad.LoadCaseGuid},
                {"Comment", pointLoad.Comment}
            };
        }

        /// <summary>
        /// Deconstruct a LineLoad.
        /// </summary>
        /// <param name="lineLoad">LineLoad.</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        [MultiReturn(new[]{"Guid", "Type", "Curve", "Direction", "q1", "q2", "LoadCaseGuid", "Comment"})]
        public static Dictionary<string, object> LineLoadDeconstruct(FemDesign.Loads.LineLoad lineLoad)
        {
            return new Dictionary<string, object>
            {
                {"Guid", lineLoad.LoadCaseGuid},
                {"Type", lineLoad.LoadType},
                {"Curve", lineLoad.GetDynamoGeometry()},
                {"Direction", lineLoad.Direction.ToDynamo()},
                {"q1", lineLoad.Load[0].Value},
                {"q2", lineLoad.Load[1].Value},
                {"LoadCaseGuid", lineLoad.LoadCaseGuid},
                {"Comment", lineLoad.Comment}
            };
        }

        /// <summary>
        /// Deconstruct a LineLoad.
        /// </summary>
        /// <param name="lineTemperatureLoad">LineLoad.</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        [MultiReturn(new[]{"Guid", "Curve", "Direction", "TopBotLocVal1", "TopBotLocVal2", "LoadCaseGuid", "Comment"})]
        public static Dictionary<string, object> LineTemperatureLoadDeconstruct(FemDesign.Loads.LineTemperatureLoad lineTemperatureLoad)
        {
            return new Dictionary<string, object>
            {
                {"Guid", lineTemperatureLoad.LoadCaseGuid},
                {"Curve", lineTemperatureLoad.Edge.ToDynamo()},
                {"Direction", lineTemperatureLoad.Direction.ToDynamo()},
                {"TopBotLocVal1", lineTemperatureLoad.TopBotLocVal[0]},
                {"TopBotLocVal2", lineTemperatureLoad.TopBotLocVal[1]},
                {"LoadCaseGuid", lineTemperatureLoad.LoadCaseGuid},
                {"Comment", lineTemperatureLoad.Comment}
            };
        }

        /// <summary>
        /// Deconstruct a LongitudinalBar
        /// </summary>
        /// <param name="longBar">LongitudinalBar of a bar element..</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        [MultiReturn(new[]{"Guid", "BaseBar", "Wire", "YPos", "ZPos", "StartAnchorage", "EndAnchorage", "StartMeasurement", "EndMeasurement", "AuxBar"})]
        public static Dictionary<string, object> LongitudinalBarDeconstruct(FemDesign.Reinforcement.BarReinforcement longBar)
        {
            if (longBar.IsStirrups)
            {
                throw new System.ArgumentException($"Passed object {longBar.Guid} is not a longitudinal bar reinforcement object. Did you pass a stirrups bar?");
            }
            else
            {
                return new Dictionary<string, object>
                {
                    {"Guid", longBar.Guid},
                    {"BaseBar", longBar.BaseBar.Guid},
                    {"Wire", longBar.Wire},
                    {"YPos", longBar.LongitudinalBar.Position2d.X},
                    {"ZPos", longBar.LongitudinalBar.Position2d.Y},
                    {"StartAnchorage", longBar.LongitudinalBar.Anchorage.Start},
                    {"EndAnchorage", longBar.LongitudinalBar.Anchorage.End},
                    {"StartMeasurement", longBar.LongitudinalBar.Start},
                    {"EndMeasurement", longBar.LongitudinalBar.End},
                    {"AuxBar", longBar.LongitudinalBar.Auxiliary}
                };

            }
        }

        /// <summary>
        /// Deconstruct a thickness item (location value)
        /// </summary>
        [IsVisibleInDynamoLibrary(true)]
        [MultiReturn(new[]{"Point", "Value"})]
        public static Dictionary<string, object> ThicknessDeconstruct(FemDesign.Shells.Thickness thicknessLocationValue)
        {
            return new Dictionary<string, object>
            {
                {"Point", thicknessLocationValue.ToDynamo()},
                {"Value", thicknessLocationValue.Value}
            };
        }

        /// <summary>
        /// Deconstruct a TopBottomLocationValue element.
        /// </summary>
        /// <param name="topBotLocVal">TopBottomLocationValue</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        [MultiReturn(new[]{"Point", "TopValue", "BottomValue"})]
        public static Dictionary<string, object> TopBottomLocationValueDeconstruct(FemDesign.Loads.TopBotLocationValue topBotLocVal)
        {
            return new Dictionary<string, object>
            {
                {"Point", topBotLocVal.GetFdPoint().ToDynamo()},
                {"TopValue", topBotLocVal.TopVal},
                {"BottomValue", topBotLocVal.BottomVal}
            };
        }

        /// <summary>
        /// Deconstruct a SurfaceLoad.
        /// </summary>
        /// <param name="surfaceLoad">SurfaceLoad.</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        [MultiReturn(new[]{"Guid", "Type", "Surface", "Direction", "q1", "q2", "q3", "LoadCaseGuid", "Comment"})]
        public static Dictionary<string, object> SurfaceLoadDeconstruct(FemDesign.Loads.SurfaceLoad surfaceLoad)
        {
            if (surfaceLoad.Loads.Count == 1)
            {
                return new Dictionary<string, object>
                {
                    {"Guid", surfaceLoad.LoadCaseGuid},
                    {"Type", surfaceLoad.LoadType},
                    {"Surface", surfaceLoad.Region.ToDynamoSurface()},
                    {"Direction", surfaceLoad.Direction.ToDynamo()},
                    {"q1", surfaceLoad.Loads[0].Value},
                    {"q2", surfaceLoad.Loads[0].Value},
                    {"q3", surfaceLoad.Loads[0].Value},
                    {"LoadCaseGuid", surfaceLoad.LoadCaseGuid},
                    {"Comment", surfaceLoad.Comment}
                };
            }
            else if (surfaceLoad.Loads.Count == 3)
            {
                return new Dictionary<string, object>
                {
                    {"Guid", surfaceLoad.LoadCaseGuid},
                    {"Type", surfaceLoad.LoadType},
                    {"Surface", surfaceLoad.Region.ToDynamoSurface()},
                    {"Direction", surfaceLoad.Direction.ToDynamo()},
                    {"q1", surfaceLoad.Loads[0].Value},
                    {"q2", surfaceLoad.Loads[1].Value},
                    {"q3", surfaceLoad.Loads[2].Value},
                    {"LoadCaseGuid", surfaceLoad.LoadCaseGuid},
                    {"Comment", surfaceLoad.Comment}
                };
            }
            else
            {
                throw new System.ArgumentException("Length of load must be 1 or 3.");
            }  
        }

        /// <summary>
        /// Deconstruct a SurfaceTemperatureLoad.
        /// </summary>
        /// <param name="srfTmpLoad">SurfaceTemperatureLoad.</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        [MultiReturn(new[]{"Guid", "Surface", "Direction", "TopBotLocVal1", "TopBotLocVal2", "TopBotLocVal3", "LoadCaseGuid", "Comment"})]
        public static Dictionary<string, object> SurfaceTemperatureLoadDeconstruct(FemDesign.Loads.SurfaceTemperatureLoad srfTmpLoad)
        {
            if (srfTmpLoad.TopBotLocVal.Count == 1)
            {
                return new Dictionary<string, object>
                {
                    {"Guid", srfTmpLoad.LoadCaseGuid},
                    {"Surface", srfTmpLoad.Region.ToDynamoSurface()},
                    {"Direction", srfTmpLoad.LocalZ.ToDynamo()},
                    {"TopBotLocVal1", srfTmpLoad.TopBotLocVal[0]},
                    {"TopBotLocVal2", srfTmpLoad.TopBotLocVal[0]},
                    {"TopBotLocVal3", srfTmpLoad.TopBotLocVal[0]},
                    {"LoadCaseGuid", srfTmpLoad.LoadCaseGuid},
                    {"Comment", srfTmpLoad.Comment}
                };
            }
            else if (srfTmpLoad.TopBotLocVal.Count == 3)
            {
                return new Dictionary<string, object>
                {
                    {"Guid", srfTmpLoad.LoadCaseGuid},
                    {"Surface", srfTmpLoad.Region.ToDynamoSurface()},
                    {"Direction", srfTmpLoad.LocalZ.ToDynamo()},
                    {"TopBotLocVal1", srfTmpLoad.TopBotLocVal[0]},
                    {"TopBotLocVal2", srfTmpLoad.TopBotLocVal[1]},
                    {"TopBotLocVal3", srfTmpLoad.TopBotLocVal[2]},
                    {"LoadCaseGuid", srfTmpLoad.LoadCaseGuid},
                    {"Comment", srfTmpLoad.Comment}
                };
            }
            else
            {
                throw new System.ArgumentException("Length of load must be 1 or 3.");
            }
            
        }

        /// <summary>
        /// Deconstruct a PressureLoad.
        /// </summary>
        /// <param name="pressureLoad">PressureLoad.</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        [MultiReturn(new[]{"Guid", "Type", "Surface", "Direction", "z0", "q0", "qh", "LoadCaseGuid", "Comment"})]
        public static Dictionary<string, object> PressureLoadDeconstruct(FemDesign.Loads.PressureLoad pressureLoad)
        {
            return new Dictionary<string, object>
            {
                {"Guid", pressureLoad.LoadCaseGuid},
                {"Type", pressureLoad.LoadType},
                {"Surface", pressureLoad.Region.ToDynamoSurface()},
                {"Direction", pressureLoad.Direction.ToDynamo()},
                {"z0", pressureLoad.Z0},
                {"q0", pressureLoad.Q0},
                {"qh", pressureLoad.Qh},
                {"LoadCaseGuid", pressureLoad.LoadCaseGuid},
                {"Comment", pressureLoad.Comment}
            };
        }

        /// <summary>
        /// Deconstruct a LoadCase.
        /// </summary>
        /// <param name="loadCase">LoadCase.</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        [MultiReturn(new[]{"Guid", "Name", "Type", "DurationClass"})]
        public static Dictionary<string, object> LoadCaseDeconstruct(FemDesign.Loads.LoadCase loadCase)
        {
            return new Dictionary<string, object>
            {
                {"Guid", loadCase.Guid.ToString()},
                {"Name", loadCase.Name},
                {"Type", loadCase.Type.ToString()},
                {"DurationClass", loadCase.DurationClass.ToString()}
            };
        }
        
        /// <summary>
        /// Deconstruct a LoadCombination.
        /// </summary>
        /// <param name="loadCombination">LoadCombination.</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        [MultiReturn(new[]{"Guid", "Name", "Type", "LoadCases", "Gammas"})]
        public static Dictionary<string, object> LoadCombinationDeconstruct(FemDesign.Loads.LoadCombination loadCombination)
        {
            return new Dictionary<string, object>
            {
                {"Guid", loadCombination.Guid},
                {"Name", loadCombination.Name},
                {"Type", loadCombination.Type.ToString()},
                {"LoadCases", loadCombination.GetLoadCaseGuidsAsString()},
                {"Gammas", loadCombination.GetGammas()}
            };
        }

        /// <summary>
        /// Deconstruct basic material information
        /// </summary>
        /// <param name="material">FemDesign.Materials.Material or a timber panel type timberPanelLibraryData or cltDataLibraryType.</param>
        [IsVisibleInDynamoLibrary(true)]
        [MultiReturn(new[]{"Guid", "Standard", "Country", "Name"})]
        public static Dictionary<string, object> MaterialDeconstruct(FemDesign.Materials.IMaterial material)
        {
            string standard = null;
            string country = null;
            if (material is Materials.Material mat)
            {
                standard = mat.Standard;
                country = mat.Country;
            }

            return new Dictionary<string, object>
            {
                {"Guid", material.Guid},
                {"Standard", standard},
                {"Country", country},
                {"Name", material.Name}
            };
        }

        /// <summary>
        /// Deconstruct model.
        /// </summary>
        /// <param name="model">Model.</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        [MultiReturn(new[]{"Guid", "CountryCode", "Bars", "FictitiousBars", "Shells", "FictitiousShells", "Diaphragms", "Panels", "Covers", "Loads", "LoadCases", "LoadCombinations", "Supports", "Axes", "Storeys"})]
        public static Dictionary<string, object> ModelDeconstruct(FemDesign.Model model)
        {
            List<StructureGrid.Axis> axes;
            if (model.Entities.Axes != null)
            {
                axes = model.Entities.Axes.Axis;
            }
            else
            {
                axes = null;
            }

            List<StructureGrid.Storey> storeys;
            if (model.Entities.Storeys != null)
            {
                storeys = model.Entities.Storeys.Storey;
            }
            else
            {
                storeys = null;
            }
        
            // return
            return new Dictionary<string, object>
            {
                {"Guid", model.Guid},
                {"CountryCode", model.Country.ToString()},
                {"Bars", model.Entities.Bars},
                {"FictitiousBars", model.Entities.AdvancedFem.FictitiousBars},
                {"Shells", model.Entities.Slabs},
                {"FictitiousShells", model.Entities.AdvancedFem.FictitiousShells},
                {"Diaphragms", model.Entities.AdvancedFem.Diaphragms},
                {"Panels", model.Entities.Panels},
                {"Covers", model.Entities.AdvancedFem.Covers},
                {"Loads", model.Entities.Loads.GetLoads()},
                {"LoadCases", model.Entities.Loads.LoadCases},
                {"LoadCombinations", model.Entities.Loads.LoadCombinations},
                {"Supports", model.Entities.Supports.GetSupports()},
                {"Axes", axes},
                {"Storeys", storeys}
            };
        }

        /// <summary>
        /// Deconstruct a SurfaceReinforcement.
        /// </summary>
        /// <param name="surfaceReinforcement">SurfaceReinforcement</param>
        /// <returns></returns>
        [MultiReturn(new[]{"Guid", "Straight", "Wire", "Surface"})]
        [IsVisibleInDynamoLibrary(true)]
        public static Dictionary<string, object> SurfaceReinforcementDeconstruct(FemDesign.Reinforcement.SurfaceReinforcement surfaceReinforcement)
        {
            return new Dictionary<string, object>
            {
                {"Guid", surfaceReinforcement.Guid},
                {"Straight", surfaceReinforcement.Straight},
                {"Wire", surfaceReinforcement.Wire},
                {"Surface", surfaceReinforcement.Region.ToDynamoSurface()}
            };
        }

        /// <summary>
        /// Deconstruct a SurfaceReinforcement Parameters
        /// </summary>
        /// <param name="surfaceReinforcement">SurfaceReinforcement</param>
        /// <returns></returns>
        [MultiReturn(new[]{"Guid", "SingleLayerReinforcement", "XDirection", "YDirection"})]
        [IsVisibleInDynamoLibrary(true)]
        public static Dictionary<string, object> SurfaceReinforcementParametersDeconstruct(FemDesign.Reinforcement.SurfaceReinforcementParameters surfaceReinforcementParameters)
        {
            return new Dictionary<string, object>
            {
                {"Guid", surfaceReinforcementParameters.Guid},
                {"SingleLayerReinforcement", surfaceReinforcementParameters.SingleLayerReinforcement},
                {"XDirection", surfaceReinforcementParameters.XDirection.ToDynamo()},
                {"YDirection", surfaceReinforcementParameters.YDirection.ToDynamo()}
            };
        }

        /// <summary>
        /// Deconstruct a Wire.
        /// </summary>
        /// <param name="wire">Wire.</param>
        /// <returns></returns>
        [MultiReturn(new[]{"Diameter", "ReinforcingMaterial", "Profile"})]
        [IsVisibleInDynamoLibrary(true)]
        public static Dictionary<string, object> WireDeconstruct(FemDesign.Reinforcement.Wire wire)
        {
            return new Dictionary<string, object>
            {
                {"Diameter", wire.Diameter},
                {"ReinforcingMaterial", wire.ReinforcingMaterialGuid},
                {"Profile", wire.Profile}
            };
        }

        /// <summary>
        /// Deconstruct a storey element.
        /// </summary>
        /// <param name="storey">Storey.</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        [MultiReturn(new[]{"Guid", "Origo", "Direction", "DimensionX", "DimensionY", "Name"})]
        public static Dictionary<string, object> StoreyDeconstruct(FemDesign.StructureGrid.Storey storey)
        {
            return new Dictionary<string, object>
            {
                {"Guid", storey.Guid},
                {"Origo", storey.Origo.ToDynamo()},
                {"Direction", storey.Direction.ToDynamo()},
                {"DimensionX", storey.DimensionX},
                {"DimensionY", storey.DimensionY},
                {"Name", storey.Name}
            };
        }

        /// <summary>
        /// Deconstruct a Straight.
        /// </summary>
        /// <param name="straight">Straight.</param>
        /// <returns></returns>
        [MultiReturn(new[]{"Direction", "Space", "Face", "Cover"})]
        [IsVisibleInDynamoLibrary(true)]
        public static Dictionary<string, object> StraightDeconstruct(FemDesign.Reinforcement.Straight straight)
        {
            return new Dictionary<string, object>
            {
                {"Direction", straight.Direction},
                {"Space", straight.Space},
                {"Face", straight.Face},
                {"Cover", straight.Cover}
            };
        }

        /// <summary>
        /// Deconstruct a slab element.
        /// </summary>
        /// <param name="slab">Slab.</param>
        /// <returns></returns>
        [MultiReturn(new[]{"Guid", "Surface", "ThicknessItems", "Material", "ShellEccentricity", "ShellOrthotropy", "EdgeCurves", "EdgeConnections", "LocalX", "LocalY", "SurfaceReinforcementParameters", "SurfaceReinforcement", "Identifier"})]
        [IsVisibleInDynamoLibrary(true)]
        public static Dictionary<string, object> SlabDeconstruct(FemDesign.Shells.Slab slab)
        {
            return new Dictionary<string, object>
            {
                {"Guid", slab.Guid},
                {"Surface", slab.SlabPart.Region.ToDynamoSurface()},
                {"ThicknessItems", slab.SlabPart.Thickness},
                {"Material", slab.Material},
                {"ShellEccentricity", slab.SlabPart.ShellEccentricity},
                {"ShellOrthotropy", slab.SlabPart.ShellOrthotropy},
                {"EdgeCurves", slab.SlabPart.Region.ToDynamoCurves()},
                {"EdgeConnections", slab.SlabPart.GetEdgeConnections()},
                {"LocalX", slab.SlabPart.LocalX.ToDynamo()},
                {"LocalY", slab.SlabPart.LocalY.ToDynamo()},
                {"SurfaceReinforcementParameters", slab.SurfaceReinforcementParameters},
                {"SurfaceReinforcement", slab.SurfaceReinforcement},
                {"Identifier", slab.Identifier}
            };
        }

        /// <summary>
        /// Deconstruct a fictitious shell element.
        /// </summary>
        /// <param name="fictitiousShell">FictitiousShell.</param>
        /// <returns></returns>
        [MultiReturn(new[]{"Guid", "AnalyticalId", "Surface", "MembraneStiffness", "FlexuralStiffness", "ShearStiffness", "Density", "T1", "T2", "Alpha1", "Alpha2", "IgnoreInStImpCalc", "EdgeCurves", "EdgeConnections", "LocalX", "LocalY"})]
        [IsVisibleInDynamoLibrary(true)]
        public static Dictionary<string, object> FictitiousShellDeconstruct(FemDesign.ModellingTools.FictitiousShell fictitiousShell)
        {
            return new Dictionary<string, object>
            {
                {"Guid", fictitiousShell.Guid},
                {"AnalyticalId", fictitiousShell.Name},
                {"Surface", fictitiousShell.Region.ToDynamoSurface()},
                {"MembraneStiffness", fictitiousShell.MembraneStiffness},
                {"FlexuralStiffness", fictitiousShell.FlexuralStiffness},
                {"ShearStiffness", fictitiousShell.ShearStiffness},
                {"Density", fictitiousShell.Density},
                {"T1", fictitiousShell.T1},
                {"T2", fictitiousShell.T2},
                {"Alpha1", fictitiousShell.Alpha1},
                {"Alpha2", fictitiousShell.Alpha2},
                {"IgnoreInStImpCalc", fictitiousShell.IgnoreInStImpCalculation},
                {"EdgeCurves", fictitiousShell.Region.ToDynamoCurves()},
                {"EdgeConnections", fictitiousShell.Region.GetEdgeConnections()},
                {"LocalX", fictitiousShell.LocalX.ToDynamo()},
                {"LocalY", fictitiousShell.LocalY.ToDynamo()}
            };
        }

        /// <summary>
        /// Deconstruct a diaphragm element.
        /// </summary>
        /// <param name="diaphragm">Diaphragm</param>
        /// <returns></returns>
        [MultiReturn(new[]{"Guid", "Surface", "Identifier"})]
        [IsVisibleInDynamoLibrary(true)]
        public static Dictionary<string, object> DiaphragmDeconstruct(FemDesign.ModellingTools.Diaphragm diaphragm)
        {
            return new Dictionary<string, object>
            {
                {"Guid", diaphragm.Guid},
                {"Surface", diaphragm.Region.ToDynamoSurface()},
                {"Identifier", diaphragm.Name}
            };
        }

        /// <summary>
        /// Deconstruct a panel of continuous analytical model.
        /// </summary>
        /// <param name="panel">Panel.</param>
        [MultiReturn(new[]{"Guid", "ExtSurface", "Material", "Section", "ExtEdgeCurves", "ExtEdgeConnections", "LocalX", "LocalY", "Identifier"})]
        [IsVisibleInDynamoLibrary(true)]
        public static Dictionary<string, object> PanelContinuousAnalyticalModelDeconstruct(FemDesign.Shells.Panel panel)
        {
            if (panel.InternalPanels.IntPanels.Count != 1)
            {
                throw new System.ArgumentException("Panel has more than 1 internal panel. Panel analytical model is not of type continuous.");
            }

            return new Dictionary<string, object>
            {
                {"Guid", panel.Guid},
                {"ExtSurface", panel.InternalPanels.IntPanels[0].Region.ToDynamoSurface()},
                {"Material", panel.Material},
                {"Section", panel.Section},
                {"ExtEdgeCurves", panel.InternalPanels.IntPanels[0].Region.ToDynamoCurves()},
                {"ExtEdgeConnections", panel.InternalPanels.IntPanels[0].Region.GetEdgeConnections()},
                {"LocalX", panel.LocalX.ToDynamo()},
                {"LocalY", panel.LocalY.ToDynamo()},
                {"Identifier", panel.Identifier}
            };
        }

        /// <summary>
        /// Deconstruct a section
        /// </summary>
        [IsVisibleInDynamoLibrary(true)]
        [MultiReturn(new[]{"Guid", "Name", "Surfaces", "SectionType", "MaterialType", "GroupName", "TypeName", "SizeName"})]
        public static Dictionary<string, object> SectionDeconstruct(FemDesign.Sections.Section section)
        {
            return new Dictionary<string, object>
            {
                {"Guid", section.Guid},
                {"Name", section.Name},
                {"Surfaces", section.RegionGroup.ToDynamo()},
                {"SectionType", section.Type},
                {"MaterialType", section.MaterialType},
                {"GroupName", section.GroupName},
                {"TypeName", section.TypeName},
                {"SizeName", section.SizeName}
            };
        }

        /// <summary>
        /// Deconstruct a EdgeConnection.
        /// </summary>
        /// <param name="shellEdgeConnection">EdgeConnection.</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        [MultiReturn(new[] {"Guid", "AnalyticalID", "PredefinedName", "PredefinedGuid", "Friction", "Motions", "Rotations"})]
        public static Dictionary<string, object> EdgeConnectionDeconstruct(FemDesign.Shells.EdgeConnection shellEdgeConnection)
        {
            if (shellEdgeConnection == null)
            {
                return new Dictionary<string, object>
                {
                    {"Guid", null},
                    {"AnalyticalID", null},
                    {"PredefinedName", null},
                    {"PredefinedGuid", null},
                    {"Friction", null},
                    {"Motions", null},
                    {"Rotations", null},
                };
            }
            else
            {
                if (shellEdgeConnection.Rigidity != null && shellEdgeConnection.Rigidity._friction == null)
                {
                    return new Dictionary<string, object>
                    {
                        {"Guid", shellEdgeConnection.Guid},
                        {"AnalyticalID", shellEdgeConnection.Name},
                        {"PredefinedName", null},
                        {"PredefinedGuid", null},
                        {"Friction", null},
                        {"Motions", shellEdgeConnection.Rigidity.Motions},
                        {"Rotations", shellEdgeConnection.Rigidity.Rotations},
                    };
                }
                else if (shellEdgeConnection.Rigidity != null && shellEdgeConnection.Rigidity._friction != null)
                {
                    return new Dictionary<string, object>
                    {
                        {"Guid", shellEdgeConnection.Guid},
                        {"AnalyticalID", shellEdgeConnection.Name},
                        {"PredefinedName", null},
                        {"PredefinedGuid", null},
                        {"Friction", shellEdgeConnection.Rigidity.Friction},
                        {"Motions", shellEdgeConnection.Rigidity.Motions},
                        {"Rotations", shellEdgeConnection.Rigidity.Rotations},
                    };
                }
                else if (shellEdgeConnection.PredefRigidity != null && shellEdgeConnection.PredefRigidity.Rigidity._friction == null)
                {
                    return new Dictionary<string, object>
                    {
                        {"Guid", shellEdgeConnection.Guid},
                        {"AnalyticalID", shellEdgeConnection.Name},
                        {"PredefinedName", shellEdgeConnection.PredefRigidity.Name},
                        {"PredefinedGuid", shellEdgeConnection.PredefRigidity.Guid},
                        {"Friction", null},
                        {"Motions", shellEdgeConnection.PredefRigidity.Rigidity.Motions},
                        {"Rotations", shellEdgeConnection.PredefRigidity.Rigidity.Rotations},
                    };   
                }
                else if (shellEdgeConnection.PredefRigidity != null && shellEdgeConnection.PredefRigidity.Rigidity._friction != null)
                {
                    return new Dictionary<string, object>
                    {
                        {"Guid", shellEdgeConnection.Guid},
                        {"AnalyticalID", shellEdgeConnection.Name},
                        {"PredefinedName", shellEdgeConnection.PredefRigidity.Name},
                        {"PredefinedGuid", shellEdgeConnection.PredefRigidity.Guid},
                        {"Friction", shellEdgeConnection.PredefRigidity.Rigidity.Friction},
                        {"Motions", shellEdgeConnection.PredefRigidity.Rigidity.Motions},
                        {"Rotations", shellEdgeConnection.PredefRigidity.Rigidity.Rotations},
                    };   
                }
                else
                {
                    throw new System.ArgumentException("Unexpected shell edge connection");
                }
            }       
        }

        /// <summary>
        /// Deconstruct a shear stiffness matrix, stiffness matrix 2 type
        /// </summary>
        [IsVisibleInDynamoLibrary(true)]
        [MultiReturn(new[]{"XZ", "YZ"})]
        public static Dictionary<string, object> StiffnessMatrix2Type(ModellingTools.StiffnessMatrix2Type stiffnessMatrix)
        {
            return new Dictionary<string, object>
            {
                {"XZ", stiffnessMatrix.XZ},
                {"YZ", stiffnessMatrix.YZ}
            };
        }

        /// <summary>
        /// Deconstruct a membrane or flexural stiffness matrix, stiffness matrix 4 type
        /// </summary>
        [IsVisibleInDynamoLibrary(true)]
        [MultiReturn(new[]{"XX", "XY", "YY", "GXY"})]
        public static Dictionary<string, object> StiffnessMatrix4Type(ModellingTools.StiffnessMatrix4Type stiffnessMatrix)
        {
            return new Dictionary<string, object>
            {
                {"XX", stiffnessMatrix.XX},
                {"XY", stiffnessMatrix.XY},
                {"YY", stiffnessMatrix.YY},
                {"GXY", stiffnessMatrix.GXY}
            };
        }

        /// <summary>
        /// Deconstruct a distribution of stirrups
        /// </summary>
        /// <param name="stirrups">Stirrups along a distribution of a bar element.</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        [MultiReturn(new[]{"Guid", "BaseBar", "Wire", "Profiles", "StartMeasurement", "EndMeasurement", "Spacing"})]
        public static Dictionary<string, object> StirrupDeconstruct(FemDesign.Reinforcement.BarReinforcement stirrups)
        {
            if (!stirrups.IsStirrups)
            {
                throw new System.ArgumentException($"Passed object {stirrups.Guid} is not a stirrup bar reinforcement object. Did you pass a longitudinal bar?");
            }
            else
            {
                return new Dictionary<string, object>
                {
                    {"Guid", stirrups.Guid},
                    {"BaseBar", stirrups.BaseBar.Guid},
                    {"Wire", stirrups.Wire},
                    {"Profiles", stirrups.Stirrups.Regions.Select(x => x.ToDynamoSurface())},
                    {"StartMeasurement", stirrups.Stirrups.Start},
                    {"EndMeasurement", stirrups.Stirrups.End},
                    {"Spacing", stirrups.Stirrups.Distance}
                };

            }
        }

        /// <summary>
        /// Deconstruct a Motions or Rotations element.
        /// </summary>
        /// <param name="release">Motions or Rotations.</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        [MultiReturn(new[]{"x_neg", "x_pos", "y_neg", "y_pos", "z_neg", "z_pos"})]
        public static Dictionary<string, object> ReleaseDeconstruct(object release)
        {
            //
            if (release == null)
            {
                return null;
            }

            //
            if (release.GetType() == typeof(FemDesign.Releases.Motions))
            {
                var obj = (FemDesign.Releases.Motions)release;
                return new Dictionary<string, object>
                {
                    {"x_neg", obj.XNeg},
                    {"x_pos", obj.XPos},
                    {"y_neg", obj.YNeg},
                    {"y_pos", obj.YPos},
                    {"z_neg", obj.ZNeg},
                    {"z_pos", obj.ZPos}
                };
            }
            else if (release.GetType() == typeof(FemDesign.Releases.Rotations))
            {
                var obj = (FemDesign.Releases.Rotations)release;
                return new Dictionary<string, object>
                {
                    {"x_neg", obj.XNeg},
                    {"x_pos", obj.XPos},
                    {"y_neg", obj.YNeg},
                    {"y_pos", obj.YPos},
                    {"z_neg", obj.ZNeg},
                    {"z_pos", obj.ZPos}
                };
            }
            else
            {
                throw new System.ArgumentException("Type is not supported. ReleaseDeconstruct failed.");
            }
        }

        /// <summary>
        /// Deconstruct a Support element.
        /// </summary>
        /// <param name="support">PointSupport or LineSupport</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        [MultiReturn(new[]{"Guid", "AnalyticalID", "Geometry", "MovingLocal", "LocalX", "LocalY", "Motions", "Rotations", "MotionsPlasticLimits", "RotationsPlasticLimits"})]
        public static Dictionary<string, object> SupportDeconstruct(object support)
        {
            if (support.GetType() == typeof(FemDesign.Supports.PointSupport))
            {
                var obj = (FemDesign.Supports.PointSupport)support;

                // catch pre-defined rigidity
                Releases.Motions motions;
                Releases.Rotations rotations;
                Releases.MotionsPlasticLimits motionsPlasticLimits;
                Releases.RotationsPlasticLimits rotationsPlasticLimits;
                if (obj.Group.Rigidity != null)
                {
                    motions = obj.Group.Rigidity.Motions;
                    rotations = obj.Group.Rigidity.Rotations;
                    motionsPlasticLimits = obj.Group.Rigidity.PlasticLimitForces;
                    rotationsPlasticLimits = obj.Group.Rigidity.PlasticLimitMoments;
                }
                else
                {
                    motions = obj.Group.PredefRigidity.Rigidity.Motions;
                    rotations = obj.Group.PredefRigidity.Rigidity.Rotations;
                    motionsPlasticLimits = obj.Group.PredefRigidity.Rigidity.PlasticLimitForces;
                    rotationsPlasticLimits = obj.Group.PredefRigidity.Rigidity.PlasticLimitMoments;
                }

                return new Dictionary<string, object>
                {
                    {"Guid", obj.Guid},
                    {"AnalyticalID", obj.Identifier},
                    {"Geometry", obj.GetDynamoGeometry()},
                    {"MovingLocal", "PointLoad has no moving local property."},
                    {"LocalX", obj.Group.LocalX.ToDynamo()},
                    {"LocalY", obj.Group.LocalY.ToDynamo()},
                    {"Motions", motions},
                    {"Rotations", rotations},
                    {"MotionsPlasticLimits", motionsPlasticLimits},
                    {"RotationsPlasticLimits", rotationsPlasticLimits}
                };
            }
            else if (support.GetType() == typeof(FemDesign.Supports.LineSupport))
            {
                var obj = (FemDesign.Supports.LineSupport)support;

                // catch pre-defined rigidity
                Releases.Motions motions;
                Releases.Rotations rotations;
                Releases.MotionsPlasticLimits motionsPlasticLimits;
                Releases.RotationsPlasticLimits rotationsPlasticLimits;
                if (obj.Group.Rigidity != null)
                {
                    motions = obj.Group.Rigidity.Motions;
                    rotations = obj.Group.Rigidity.Rotations;
                    motionsPlasticLimits = obj.Group.Rigidity.PlasticLimitForces;
                    rotationsPlasticLimits = obj.Group.Rigidity.PlasticLimitMoments;
                }
                else
                {
                    motions = obj.Group.PredefRigidity.Rigidity.Motions;
                    rotations = obj.Group.PredefRigidity.Rigidity.Rotations;
                    motionsPlasticLimits = obj.Group.PredefRigidity.Rigidity.PlasticLimitForces;
                    rotationsPlasticLimits = obj.Group.PredefRigidity.Rigidity.PlasticLimitMoments;
                }

                return new Dictionary<string, object>
                {
                    {"Guid", obj.Guid},
                    {"AnalyticalID", obj.Identifier},
                    {"Geometry", obj.GetDynamoGeometry()},
                    {"MovingLocal", obj.MovingLocal},
                    {"LocalX", obj.Group.LocalX.ToDynamo()},
                    {"LocalY", obj.Group.LocalY.ToDynamo()},
                    {"Motions", motions},
                    {"Rotations", rotations},
                    {"MotionsPlasticLimits", motionsPlasticLimits},
                    {"RotationsPlasticLimits", rotationsPlasticLimits}
                };
            }
            else if (support.GetType() == typeof(FemDesign.Supports.SurfaceSupport))
            {
                var obj = (FemDesign.Supports.SurfaceSupport)support;

                // catch pre-defined rigidity
                Releases.Motions motions;
                Releases.MotionsPlasticLimits motionsPlasticLimits;
                if (obj.Rigidity != null)
                {
                    motions = obj.Rigidity.Motions;
                    motionsPlasticLimits = obj.Rigidity.PlasticLimitForces;
                }
                else
                {
                    motions = obj.PredefRigidity.Rigidity.Motions;
                    motionsPlasticLimits = obj.PredefRigidity.Rigidity.PlasticLimitForces;
                }

                return new Dictionary<string, object>
                {
                    {"Guid", obj.Guid},
                    {"AnalyticalID", obj.Identifier},
                    {"Geometry", obj.Region.ToDynamoSurface()},
                    {"MovingLocal", "SurfaceSupport has no moving local property."},
                    {"LocalX", obj.CoordinateSystem.LocalX.ToDynamo()},
                    {"LocalY", obj.CoordinateSystem.LocalY.ToDynamo()},
                    {"Motions", motions},
                    {"Rotations", "SurfaceSupport has no rotations property."},
                    {"MotionsPlasticLimits", motionsPlasticLimits},
                    {"RotationsPlasticLimits", "SurfaceSupport has no rotations plastic limits property."}
                };
            }
            else
            {
                throw new System.ArgumentException("Type is not supported. SupportDeconstruct failed.");
            }
        }
    }
}
#endregion