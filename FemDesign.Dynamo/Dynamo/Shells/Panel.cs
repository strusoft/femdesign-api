
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Serialization;

#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Shells
{
    [IsVisibleInDynamoLibrary(false)]
    public partial class Panel
    {
        #region dynamo

        /// <summary>
        /// Create a profiled plate.
        /// </summary>
        /// <param name="surface">Surface.</param>
        /// <param name="material">Material.</param>
        /// <param name="section">Section.</param>
        /// <param name="eccentricity">ShellEccentricity. Optional.</param>
        /// <param name="orthoRatio">Transverse flexural stiffness factor.</param>
        /// <param name="borderEdgeConnection">EdgeConnection of external border of the Panel. Optional. If not defined hinged will be used.</param>
        /// <param name="LocalX">"Set local x-axis. Vector must be perpendicular to surface local z-axis. Local y-axis will be adjusted accordingly. Optional, local x-axis from surface coordinate system used if undefined."</param>
        /// <param name="LocalZ">Set local z-axis. Vector must be perpendicular to surface local x-axis. Local y-axis will be adjusted accordingly. Optional, local z-axis from surface coordinate system used if undefined.</param>
        /// <param name="avgMeshSize">Average mesh size. If zero an automatic value will be used by FEM-Design. Optional.</param>
        /// <param name="identifier">Identifier.</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        public static Panel ProfiledPlate(Autodesk.DesignScript.Geometry.Surface surface, Materials.Material material, Sections.Section section, [DefaultArgument("ShellEccentricity.Default()")] ShellEccentricity eccentricity, [DefaultArgument("1")] double orthoRatio, [DefaultArgument("EdgeConnection.Hinged()")] EdgeConnection edgeConnection, [DefaultArgument("Autodesk.DesignScript.Geometry.Vector.ByCoordinates(0,0,0)")] Autodesk.DesignScript.Geometry.Vector localX, [DefaultArgument("Autodesk.DesignScript.Geometry.Vector.ByCoordinates(0,0,0)")] Autodesk.DesignScript.Geometry.Vector localZ, [DefaultArgument("0")] double avgMeshSize, string identifier = "PP")
        {
            // convert geometry
            Geometry.Region region = Geometry.Region.FromDynamo(surface);

            // create panel
            Panel obj = Panel.DefaultContreteContinuous(region, edgeConnection, material, section, identifier, orthoRatio, eccentricity);

            // set local x-axis
            if (!localX.Equals(Autodesk.DesignScript.Geometry.Vector.ByCoordinates(0,0,0)))
            {
                obj.LocalX = FemDesign.Geometry.Vector3d.FromDynamo(localX);
            }

            // set local z-axis
            if (!localZ.Equals(Autodesk.DesignScript.Geometry.Vector.ByCoordinates(0,0,0)))
            {
                obj.LocalZ = FemDesign.Geometry.Vector3d.FromDynamo(localZ);
            }

            // set mesh
            obj.UniformAvgMeshSize = avgMeshSize;

            // return
            return obj;
        }

        ///<summary>
        /// Set camber simulation (by prestress) defining the prestress force and the related eccentricity
        ///</summary>
        ///<param name="panel">Panel.</param>
        ///<param name="force">Total prestress force in kN</param>
        ///<param name="eccentricity">Eccentricity of prestress force</param>
        [IsVisibleInDynamoLibrary(true)]
        public static Panel SetCamberSimByPreStress(Panel panel, double force, double eccentricity)
        {
            // deep clone to create a new instance
            var obj = panel.DeepClone();

            // set camber of new instance
            obj.Camber = new Camber(force, eccentricity);

            // return new instance
            return obj;
        }

        /// <summary>
        /// Set external EdgeConnections by indices on a panel with continuous analytical model.
        /// </summary>
        /// <param name="panel">Panel.</param>
        /// <param name="shellEdgeConnection">EdgeConnection.</param>
        /// <param name="indices">Index. List of items. Use SlabDeconstruct to extract index for each respective edge.</param>
        [IsVisibleInDynamoLibrary(true)]
        public static Panel SetExternalEdgeConnectionForContinuousAnalyticalModel(Panel panel, EdgeConnection shellEdgeConnection, List<int> indices)
        {
            // deep clone. downstreams objs will contain changes made in this method, upstream objs will not.
            // downstream and uppstream objs will share guid.
            Panel panelClone = panel.DeepClone();

            // set external edge connections
            panelClone.SetExternalEdgeConnectionsForContinuousAnalyticalModel(shellEdgeConnection, indices);

            // return
            return panelClone;  
        }
        
        #endregion
    }
}