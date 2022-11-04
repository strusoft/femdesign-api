
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Shells
{
    [IsVisibleInDynamoLibrary(false)]
    public partial class Slab
    {
        #region dynamo

        /// <summary>
        /// Set EdgeConnections by indices.
        /// </summary>
        /// <param name="slab">Slab.</param>
        /// <param name="shellEdgeConnection">EdgeConnection.</param>
        /// <param name="indices">Index. List of items. Use SlabDeconstruct to extract index for each respective edge.</param>
        public static Slab SetEdgeConnection(Slab slab, EdgeConnection shellEdgeConnection, List<int> indices)
        {
            return EdgeConnection(slab, shellEdgeConnection, indices);
        }

        /// <summary>
        /// Set average mesh size to slab.
        /// </summary>
        /// <remarks>Action</remarks>
        /// <param name="slab">Slab.</param>
        /// <param name="avgMeshSize">Average mesh size.</param>
        /// <returns></returns>
        public static Slab SetAverageSurfaceElementSize(Slab slab, double avgMeshSize)
        {
            return AverageSurfaceElementSize(slab, avgMeshSize);
        }

        /// <summary>
        /// Create a plate element.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="surface">Surface. Surface must be flat.</param>
        /// <param name="thickness">Thickness.</param>
        /// <param name="material">Material.</param>
        /// <param name="shellEccentricity">ShellEccentricity. Optional, if undefined default value will be used.</param>
        /// <param name="shellOrthotropy">ShellOrthotropy. Optional, if undefined default value will be used.</param>
        /// <param name="shellEdgeConnection">EdgeConnection. Optional, if undefined rigid.</param>
        /// <param name="localX">Set local x-axis. Vector must be perpendicular to surface local z-axis. Local y-axis will be adjusted accordingly. Optional, local x-axis from surface coordinate system used if undefined.</param>
        /// <param name="localZ">Set local z-axis. Vector must be perpendicular to surface local x-axis. Local y-axis will be adjusted accordingly. Optional, local z-axis from surface coordinate system used if undefined."</param>
        /// <param name="identifier">Identifier of plate element. Optional.</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        public static Slab Plate(Autodesk.DesignScript.Geometry.Surface surface, double thickness, Materials.Material material, [DefaultArgument("ShellEccentricity.Default()")] ShellEccentricity shellEccentricity, [DefaultArgument("ShellOrthotropy.Default()")] ShellOrthotropy shellOrthotropy, [DefaultArgument("EdgeConnection.Default()")] EdgeConnection shellEdgeConnection, [DefaultArgument("Autodesk.DesignScript.Geometry.Vector.ByCoordinates(0,0,0)")] Autodesk.DesignScript.Geometry.Vector localX,  [DefaultArgument("Autodesk.DesignScript.Geometry.Vector.ByCoordinates(0,0,0)")] Autodesk.DesignScript.Geometry.Vector localZ, string identifier = "P")
        {
            // create FlatSurface
            Geometry.Region region = Geometry.Region.FromDynamo(surface);

            // create Thickness object
            List<Thickness> _thickness = new List<Thickness>();
            _thickness.Add(new Thickness(region.CoordinateSystem.Origin, thickness));

            // create shell
            Slab slab = Slab.Plate(identifier, material, region, shellEdgeConnection, shellEccentricity, shellOrthotropy, _thickness);

            // set local x-axis
            if (!localX.Equals(Autodesk.DesignScript.Geometry.Vector.ByCoordinates(0,0,0)))
            {
                slab.SlabPart.LocalX = FemDesign.Geometry.Vector3d.FromDynamo(localX);
            }

            // set local z-axis
            if (!localZ.Equals(Autodesk.DesignScript.Geometry.Vector.ByCoordinates(0,0,0)))
            {
                slab.SlabPart.LocalZ = FemDesign.Geometry.Vector3d.FromDynamo(localZ);
            }

            return slab;
        }
        /// <summary>
        /// Create a plate element with variable thickness.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="surface">Surface. Surface must be flat.</param>
        /// <param name="thickness">Thickness. List of 3 items [t1, t2, t3].</param>
        /// <param name="material">Material.</param>
        /// <param name="shellEccentricity">ShellEccentricity. Optional, if undefined default value will be used.</param>
        /// <param name="shellOrthotropy">ShellOrthotropy. Optional, if undefined default value will be used.</param>
        /// <param name="shellEdgeConnection">EdgeConnection. Optional, if undefined rigid.</param>
        /// <param name="localX">Set local x-axis. Vector must be perpendicular to surface local z-axis. Local y-axis will be adjusted accordingly. Optional, local x-axis from surface coordinate system used if undefined.</param>
        /// <param name="localZ">Set local z-axis. Vector must be perpendicular to surface local x-axis. Local y-axis will be adjusted accordingly. Optional, local z-axis from surface coordinate system used if undefined."</param>
        /// <param name="identifier">Identifier of plate element. Optional.</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        public static Slab PlateVariableThickness(Autodesk.DesignScript.Geometry.Surface surface, List<Thickness> thickness, Materials.Material material, [DefaultArgument("ShellEccentricity.Default()")] ShellEccentricity shellEccentricity, [DefaultArgument("ShellOrthotropy.Default()")] ShellOrthotropy shellOrthotropy, [DefaultArgument("EdgeConnection.Default()")] EdgeConnection shellEdgeConnection, [DefaultArgument("Autodesk.DesignScript.Geometry.Vector.ByCoordinates(0,0,0)")] Autodesk.DesignScript.Geometry.Vector localX,  [DefaultArgument("Autodesk.DesignScript.Geometry.Vector.ByCoordinates(0,0,0)")] Autodesk.DesignScript.Geometry.Vector localZ, string identifier = "P")
        {
            // create FlatSurface
            Geometry.Region region = Geometry.Region.FromDynamo(surface);

            // check length of thickness
            if (thickness.Count != 3)
            {
                throw new System.ArgumentException("Thickness must contain exactly 3 items.");
            }

            // create shell
            Slab slab = Slab.Plate(identifier, material, region, shellEdgeConnection, shellEccentricity, shellOrthotropy, thickness);

            // set local x-axis
            if (!localX.Equals(Autodesk.DesignScript.Geometry.Vector.ByCoordinates(0,0,0)))
            {
                slab.SlabPart.LocalX = FemDesign.Geometry.Vector3d.FromDynamo(localX);
            }

            // set local z-axis
            if (!localZ.Equals(Autodesk.DesignScript.Geometry.Vector.ByCoordinates(0,0,0)))
            {
                slab.SlabPart.LocalZ = FemDesign.Geometry.Vector3d.FromDynamo(localZ);
            }

            return slab;
        }
        /// <summary>
        /// Create a wall element.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="surface">Surface. Surface must be flat and vertical.</param>
        /// <param name="thickness">Thickness.</param>
        /// <param name="material">Material.</param>
        /// <param name="shellEccentricity">ShellEccentricity. Optional, if undefined default value will be used.</param>
        /// <param name="shellOrthotropy">ShellOrthotropy. Optional, if undefined default value will be used.</param>
        /// <param name="shellEdgeConnection">EdgeConnection. Optional, if undefined rigid.</param>
        /// <param name="localX">Set local x-axis. Vector must be perpendicular to surface local z-axis. Local y-axis will be adjusted accordingly. Optional, local x-axis from surface coordinate system used if undefined.</param>
        /// <param name="localZ">Set local z-axis. Vector must be perpendicular to surface local x-axis. Local y-axis will be adjusted accordingly. Optional, local z-axis from surface coordinate system used if undefined."</param>
        /// <param name="identifier">Identifier of wall element. Optional.</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        public static Slab Wall(Autodesk.DesignScript.Geometry.Surface surface, double thickness, Materials.Material material,  [DefaultArgument("ShellEccentricity.Default()")] ShellEccentricity shellEccentricity, [DefaultArgument("ShellOrthotropy.Default()")] ShellOrthotropy shellOrthotropy, [DefaultArgument("EdgeConnection.Default()")] EdgeConnection shellEdgeConnection, [DefaultArgument("Autodesk.DesignScript.Geometry.Vector.ByCoordinates(0,0,0)")] Autodesk.DesignScript.Geometry.Vector localX, [DefaultArgument("Autodesk.DesignScript.Geometry.Vector.ByCoordinates(0,0,0)")] Autodesk.DesignScript.Geometry.Vector localZ, string identifier = "W")
        {
            // create FlatSurface
            Geometry.Region region = Geometry.Region.FromDynamo(surface);

            // create Thickness object
            List<Thickness> _thickness = new List<Thickness>();
            _thickness.Add(new Thickness(region.CoordinateSystem.Origin, thickness));

            // check if surface is vertical
            if (Math.Abs(region.CoordinateSystem.LocalZ.Z) > FemDesign.Tolerance.Point3d)
            {
                throw new System.ArgumentException("Wall is not vertical! Create plate instead.");
            }

            // create shell
            Slab slab = Slab.Wall(identifier, material, region, shellEdgeConnection, shellEccentricity, shellOrthotropy, _thickness);

            // set local x-axis
            if (!localX.Equals(Autodesk.DesignScript.Geometry.Vector.ByCoordinates(0,0,0)))
            {
                slab.SlabPart.LocalX = FemDesign.Geometry.Vector3d.FromDynamo(localX);
            }

            // set local z-axis
            if (!localZ.Equals(Autodesk.DesignScript.Geometry.Vector.ByCoordinates(0,0,0)))
            {
                slab.SlabPart.LocalZ = FemDesign.Geometry.Vector3d.FromDynamo(localZ);
            }

            return slab;
        }
        /// <summary>
        /// Create a wall element with variable thickness.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="surface">Surface. Surface must be flat.</param>
        /// <param name="thickness">Thickness. List of 2 items [t1, t2].</param>
        /// <param name="material">Material.</param>
        /// <param name="shellEccentricity">ShellEccentricity. Optional, if undefined default value will be used.</param>
        /// <param name="shellOrthotropy">ShellOrthotropy. Optional, if undefined default value will be used.</param>
        /// <param name="shellEdgeConnection">EdgeConnection. Optional, if undefined rigid.</param>
        /// <param name="localX">Set local x-axis. Vector must be perpendicular to surface local z-axis. Local y-axis will be adjusted accordingly. Optional, local x-axis from surface coordinate system used if undefined.</param>
        /// <param name="localZ">Set local z-axis. Vector must be perpendicular to surface local x-axis. Local y-axis will be adjusted accordingly. Optional, local z-axis from surface coordinate system used if undefined."</param>
        /// <param name="identifier">Identifier of wall element. Optional.</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        public static Slab WallVariableThickness(Autodesk.DesignScript.Geometry.Surface surface,  List<Thickness> thickness, Materials.Material material, [DefaultArgument("ShellEccentricity.Default()")] ShellEccentricity shellEccentricity, [DefaultArgument("ShellOrthotropy.Default()")] ShellOrthotropy shellOrthotropy, [DefaultArgument("EdgeConnection.Default()")] EdgeConnection shellEdgeConnection, [DefaultArgument("Autodesk.DesignScript.Geometry.Vector.ByCoordinates(0,0,0)")] Autodesk.DesignScript.Geometry.Vector localX, [DefaultArgument("Autodesk.DesignScript.Geometry.Vector.ByCoordinates(0,0,0)")] Autodesk.DesignScript.Geometry.Vector localZ, string identifier = "W")
        {
            // create FlatSurface
            Geometry.Region region = Geometry.Region.FromDynamo(surface);

            // check if surface is vertical
            if (Math.Abs(region.CoordinateSystem.LocalZ.Z) > FemDesign.Tolerance.Point3d)
            {
                throw new System.ArgumentException("Wall is not vertical! Create plate instead.");
            }

            // check length of thickness
            if (thickness.Count != 2)
            {
                throw new System.ArgumentException("Thickness must contain exactly 2 items.");
            }

            // create shell
            Slab slab = Slab.Wall(identifier, material, region, shellEdgeConnection, shellEccentricity, shellOrthotropy, thickness);

            // set local x-axis
            if (!localX.Equals(Autodesk.DesignScript.Geometry.Vector.ByCoordinates(0,0,0)))
            {
                slab.SlabPart.LocalX = FemDesign.Geometry.Vector3d.FromDynamo(localX);
            }

            // set local z-axis
            if (!localZ.Equals(Autodesk.DesignScript.Geometry.Vector.ByCoordinates(0,0,0)))
            {
                slab.SlabPart.LocalZ = FemDesign.Geometry.Vector3d.FromDynamo(localZ);
            }

            return slab;
        }
        /// <summary>
        /// Set local x- and z-axes. Local y-axis will be defined according to a right handed coordinate system.
        /// </summary>
        /// <param name="localX">Vector. Must be perpendicular to slab surface normal.</param>
        /// <param name="localZ">Vector. Must be parallell to slab surface normal.</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(false)]
        public Slab SetLocalAxes(Autodesk.DesignScript.Geometry.Vector localX, Autodesk.DesignScript.Geometry.Vector localZ)
        {
            // deep clone. downstreams objs will contain changes made in this method, upstream objs will not.
            // downstream and uppstream objs will share guid.
            Slab slab = this.DeepClone();

            // set local x and local z
            slab.SlabPart.LocalX = Geometry.Vector3d.FromDynamo(localX);
            slab.SlabPart.LocalZ = Geometry.Vector3d.FromDynamo(localZ);

            //
            return slab;
        }
        
        #endregion
    }
}