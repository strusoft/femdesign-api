// https://strusoft.com/
using System.Collections.Generic;

#region dynamo
using Autodesk.DesignScript.Runtime;

namespace FemDesign
{
    /// <summary>
    /// Static methods from other classes are put under this class Dynamo library heirarchy reasons 
    /// so that all deconstruc methods are organized under Deconstruct.
    /// </summary>
    [IsVisibleInDynamoLibrary(false)]
    public class Deconstruct
    {

        /// <summary>
        /// Deconstruct a bar element.
        /// </summary>
        /// <param name="bar">Bar.</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        [MultiReturn(new[]{"Guid", "StructuralID", "AnalyticalID", "Type", "Curve", "Material", "Section"})]
        public static Dictionary<string, object> BarDeconstruct(FemDesign.Bars.Bar bar)
        {
            return new Dictionary<string, object>
            {
                {"Guid", bar.guid},
                {"AnalyticalID", bar.name},
                {"StructuralID", bar.barPart.name},
                {"Type", bar.type},
                {"Curve", bar.GetDynamoCurve()},
                {"Material", bar.material},
                {"Section", bar.section}
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
                {"Guid", cover.guid},
                {"Id", cover.name},
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
                {"Guid", pointLoad.guid},
                {"Type", pointLoad.loadType},
                {"Point", pointLoad.GetDynamoGeometry()},
                {"Direction", pointLoad.direction.ToDynamo()},
                {"q", pointLoad.load.val},
                {"LoadCaseGuid", pointLoad.loadCase},
                {"Comment", pointLoad.comment}
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
                {"Guid", lineLoad.guid},
                {"Type", lineLoad.loadType},
                {"Curve", lineLoad.GetDynamoGeometry()},
                {"Direction", lineLoad.direction.ToDynamo()},
                {"q1", lineLoad.load[0].val},
                {"q2", lineLoad.load[1].val},
                {"LoadCaseGuid", lineLoad.loadCase},
                {"Comment", lineLoad.comment}
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
            if (surfaceLoad.load.Count == 1)
            {
                return new Dictionary<string, object>
                {
                    {"Guid", surfaceLoad.guid},
                    {"Type", surfaceLoad.loadType},
                    {"Surface", surfaceLoad.region.ToDynamoSurface()},
                    {"Direction", surfaceLoad.direction.ToDynamo()},
                    {"q1", surfaceLoad.load[0].val},
                    {"q2", surfaceLoad.load[0].val},
                    {"q3", surfaceLoad.load[0].val},
                    {"LoadCaseGuid", surfaceLoad.loadCase},
                    {"Comment", surfaceLoad.comment}
                };
            }
            else if (surfaceLoad.load.Count == 3)
            {
                return new Dictionary<string, object>
                {
                    {"Guid", surfaceLoad.guid},
                    {"Type", surfaceLoad.loadType},
                    {"Surface", surfaceLoad.region.ToDynamoSurface()},
                    {"Direction", surfaceLoad.direction.ToDynamo()},
                    {"q1", surfaceLoad.load[0].val},
                    {"q2", surfaceLoad.load[1].val},
                    {"q3", surfaceLoad.load[2].val},
                    {"LoadCaseGuid", surfaceLoad.loadCase},
                    {"Comment", surfaceLoad.comment}
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
                {"Guid", pressureLoad.guid},
                {"Type", pressureLoad.loadType},
                {"Surface", pressureLoad.region.ToDynamoSurface()},
                {"Direction", pressureLoad.direction.ToDynamo()},
                {"z0", pressureLoad.z0},
                {"q0", pressureLoad.q0},
                {"qh", pressureLoad.qh},
                {"LoadCaseGuid", pressureLoad.loadCase},
                {"Comment", pressureLoad.comment}
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
                {"Guid", loadCase.guid},
                {"Name", loadCase.name},
                {"Type", loadCase.type},
                {"DurationClass", loadCase.durationClass}
            };
        }
        
        /// <summary>
        /// Deconstruct a LoadCombination.
        /// </summary>
        /// <param name="loadCombination">LoadCombination.</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        [MultiReturn(new[]{"Guid", "Name", "LoadCases", "Gammas"})]
        public static Dictionary<string, object> LoadCombinationDeconstruct(FemDesign.Loads.LoadCombination loadCombination)
        {
            return new Dictionary<string, object>
            {
                {"Guid", loadCombination.guid},
                {"Name", loadCombination.name},
                {"LoadCases", loadCombination.GetLoadCaseGuidsAsString()},
                {"Gammas", loadCombination.GetGammas()}
            };
        }

        /// <summary>
        /// Deconstruct model.
        /// </summary>
        /// <param name="model">Model.</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        [MultiReturn(new[]{"Guid", "CountryCode", "Bars", "Shells", "Covers", "Loads", "LoadCases", "LoadCombinations", "Supports"})]
        public static Dictionary<string, object> ModelDeconstruct(FemDesign.Model model)
        {
            // return
            return new Dictionary<string, object>
            {
                {"Guid", model.guid},
                {"CountryCode", model.country},
                {"Bars", model.GetBars()},
                {"Shells", model.GetSlabs()},
                {"Covers", model.entities.advancedFem.cover},
                {"Loads", model.entities.loads.GetLoads()},
                {"LoadCases", model.entities.loads.loadCase},
                {"LoadCombinations", model.entities.loads.loadCombination},
                {"Supports", model.entities.supports.ListSupports()}
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
                {"Guid", surfaceReinforcement.guid},
                {"Straight", surfaceReinforcement.straight},
                {"Wire", surfaceReinforcement.wire},
                {"Surface", surfaceReinforcement.region.ToDynamoSurface()}
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
                {"Diameter", wire.diameter},
                {"ReinforcingMaterial", wire.reinforcingMaterialGuid},
                {"Profile", wire.profile}
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
                {"Direction", straight.direction},
                {"Space", straight.space},
                {"Face", straight.face},
                {"Cover", straight.cover}
            };
        }

        /// <summary>
        /// Deconstruct a slab element.
        /// </summary>
        /// <param name="slab">Slab.</param>
        /// <returns></returns>
        [MultiReturn(new[]{"Guid", "StructuralId", "AnalyticalId", "Material", "Surface", "EdgeCurves", "ShellEdgeConnections", "SurfaceReinforcement"})]
        [IsVisibleInDynamoLibrary(true)]
        public static Dictionary<string, object> SlabDeconstruct(FemDesign.Shells.Slab slab)
        {
            return new Dictionary<string, object>
            {
                {"Guid", slab.guid},
                {"StructuralId", slab.name},
                {"AnalyticalId", slab.slabPart.name},
                {"Material", slab.material},
                {"Surface", slab.slabPart.GetDynamoSurface()},
                {"EdgeCurves", slab.slabPart.GetDynamoCurves()},
                {"ShellEdgeConnections", slab.slabPart.GetEdgeConnections()},
                {"SurfaceReinforcement", slab.surfaceReinforcement}
            };
        }

        /// <summary>
        /// Deconstruct a ShellEdgeConnection.
        /// </summary>
        /// <param name="shellEdgeConnection">ShellEdgeConnection.</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        [MultiReturn(new[] {"Guid", "AnalyticalID", "Motions", "Rotations"})]
        public static Dictionary<string, object> ShellEdgeConnectionDeconstruct(FemDesign.Shells.ShellEdgeConnection shellEdgeConnection)
        {
            if (shellEdgeConnection == null)
            {
                return new Dictionary<string, object>
                {
                    {"Guid", null},
                    {"AnalyticalID", null},
                    {"Motions", null},
                    {"Rotations", null},
                };
            }
            else
            {
                try
                {
                    return new Dictionary<string, object>
                    {
                        {"Guid", shellEdgeConnection.guid},
                        {"AnalyticalID", shellEdgeConnection.name},
                        {"Motions", shellEdgeConnection.rigidity.motions},
                        {"Rotations", shellEdgeConnection.rigidity.rotations},
                    };
                }
                catch
                {
                    return new Dictionary<string, object>
                    {
                        {"Guid", shellEdgeConnection.guid},
                        {"AnalyticalID", shellEdgeConnection.name},
                        {"Motions", "Pre-defined edge connection type was serialized."},
                        {"Rotations", "Pre-defined edge connection type was serialized."},
                    };   
                }
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
                    {"x_neg", obj.x_neg},
                    {"x_pos", obj.x_pos},
                    {"y_neg", obj.y_neg},
                    {"y_pos", obj.y_pos},
                    {"z_neg", obj.z_neg},
                    {"z_pos", obj.z_pos}
                };
            }
            else if (release.GetType() == typeof(FemDesign.Releases.Rotations))
            {
                var obj = (FemDesign.Releases.Rotations)release;
                return new Dictionary<string, object>
                {
                    {"x_neg", obj.x_neg},
                    {"x_pos", obj.x_pos},
                    {"y_neg", obj.y_neg},
                    {"y_pos", obj.y_pos},
                    {"z_neg", obj.z_neg},
                    {"z_pos", obj.z_pos}
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
        [MultiReturn(new[]{"Guid", "AnalyticalID", "Geometry", "MovingLocal", "LocalX", "LocalY", "Motions", "Rotations"})]
        public static Dictionary<string, object> SupportDeconstruct(object support)
        {
            if (support.GetType() == typeof(FemDesign.Supports.PointSupport))
            {
                var obj = (FemDesign.Supports.PointSupport)support;
                return new Dictionary<string, object>
                {
                    {"Guid", obj.guid},
                    {"AnalyticalID", obj.name},
                    {"Geometry", obj.GetDynamoGeometry()},
                    {"MovingLocal", "PointLoad has no moving local property."},
                    {"LocalX", obj.group.localX.ToDynamo()},
                    {"LocalY", obj.group.localY.ToDynamo()},
                    {"Motions", obj.group.rigidity.motions},
                    {"Rotations", obj.group.rigidity.rotations}
                };
            }
            else if (support.GetType() == typeof(FemDesign.Supports.LineSupport))
            {
                var obj = (FemDesign.Supports.LineSupport)support;
                return new Dictionary<string, object>
                {
                    {"Guid", obj.guid},
                    {"AnalyticalID", obj.name},
                    {"Geometry", obj.GetDynamoGeometry()},
                    {"MovingLocal", obj.movingLocal},
                    {"LocalX", obj.group.localX.ToDynamo()},
                    {"LocalY", obj.group.localY.ToDynamo()},
                    {"Motions", obj.group.rigidity.motions},
                    {"Rotations", obj.group.rigidity.rotations}
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