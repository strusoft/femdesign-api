// https://strusoft.com/

using System;
using System.Collections.Generic;
using System.Xml.Serialization;
#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Shells
{
    /// <summary>
    /// slab_type
    /// </summary>
    [System.Serializable]
    [IsVisibleInDynamoLibrary(false)]
    public class Slab: EntityBase
    {
        private static int _plateInstance = 0;
        private static int _wallInstance = 0;
        [XmlIgnore]
        public Materials.Material Material { get; set; }
        [XmlIgnore]
        public Reinforcement.SurfaceReinforcementParameters SurfaceReinforcementParameters { get; set; }
        [XmlIgnore]
        public List<Reinforcement.SurfaceReinforcement> SurfaceReinforcement = new List<Reinforcement.SurfaceReinforcement>();
        [XmlAttribute("name")]
        public string Name {get; set;} // identifier
        [XmlAttribute("type")]
        public string _type; // slabtype
        [XmlIgnore]
        public string Type
        {
            get {return this._type;}
            set {this._type = RestrictedString.SlabType(value);}
        }
        [XmlElement("slab_part", Order=1)]
        public SlabPart SlabPart {get; set;}
        [XmlElement("end", Order=2)]
        public string End {get; set;} // empty_type

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private Slab()
        {

        }

        /// <summary>
        /// Construct Slab.
        /// </summary>
        private Slab(string type, string name, SlabPart slabPart, Materials.Material material)
        {
            this.EntityCreated();
            this.Name = name;
            this.Type = type;
            this.SlabPart = slabPart;
            this.Material = material;
            this.End = "";
        }

        internal static Slab Plate(string identifier, Materials.Material material, Geometry.Region region, ShellEdgeConnection shellEdgeConnection, ShellEccentricity eccentricity, ShellOrthotropy orthotropy, List<Thickness> thickness)
        {
            Slab._plateInstance++;
            string type = "plate";
            string name = identifier + "." + Slab._plateInstance.ToString() + ".1";
            SlabPart slabPart = SlabPart.Define(name, region, thickness, material, shellEdgeConnection, eccentricity, orthotropy);
            Slab shell = new Slab(type, name, slabPart, material);
            return shell;
        }
        internal static Slab Wall(string identifier, Materials.Material material, Geometry.Region region, ShellEdgeConnection shellEdgeConnection, ShellEccentricity eccentricity, ShellOrthotropy orthotropy, List<Thickness> thickness)
        {
            // check if surface is vertical
            if (Math.Abs(region.CoordinateSystem.LocalZ.Z) > FemDesign.Tolerance.Point3d)
            {
                throw new System.ArgumentException("Wall is not vertical! Create plate instead.");
            }
            
            Slab._wallInstance++;
            string type = "wall";
            string name = identifier + "." + Slab._wallInstance.ToString() + ".1";
            SlabPart slabPart = SlabPart.Define(name, region, thickness, material, shellEdgeConnection, eccentricity, orthotropy);
            Slab shell = new Slab(type, name, slabPart, material);
            return shell;
        }

        /// <summary>
        /// Set ShellEdgeConnections by indices.
        /// </summary>
        /// <param name="slab">Slab.</param>
        /// <param name="shellEdgeConnection">ShellEdgeConnection.</param>
        /// <param name="indices">Index. List of items. Use SlabDeconstruct to extract index for each respective edge.</param>
        [IsVisibleInDynamoLibrary(true)]
        public static Slab SetShellEdgeConnection(Slab slab, ShellEdgeConnection shellEdgeConnection, List<int> indices)
        {
            // deep clone. downstreams objs will contain changes made in this method, upstream objs will not.
            // downstream and uppstream objs will share guid.
            Slab slabClone = slab.DeepClone();

            foreach (int index in indices)
            {
                if (index >= 0 & index < slab.SlabPart.GetEdgeConnections().Count)
                {
                    // pass
                }
                else
                {
                    throw new System.ArgumentException("Index is out of bounds.");
                }
                
                //
                slabClone.SlabPart.Region.SetEdgeConnection(shellEdgeConnection, index);  

            }

            //
            return slabClone;          
        }
        /// <summary>
        /// Set average mesh size to slab.
        /// </summary>
        /// <remarks>Action</remarks>
        /// <param name="slab">Slab.</param>
        /// <param name="avgMeshSize">Average mesh size.</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        public static Slab SetAverageSurfaceElementSize(Slab slab, double avgMeshSize)
        {
            // deep clone. downstreams objs will contain changes made in this method, upstream objs will not.
            // downstream and uppstream objs will share guid.
            Slab slabClone = slab.DeepClone();

            //
            slabClone.SlabPart.MeshSize = avgMeshSize;

            // return
            return slabClone;
        }
        #region dynamo
        /// <summary>
        /// Create a plate element.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="surface">Surface. Surface must be flat.</param>
        /// <param name="thickness">Thickness.</param>
        /// <param name="material">Material.</param>
        /// <param name="shellEccentricity">ShellEccentricity. Optional, if undefined default value will be used.</param>
        /// <param name="shellOrthotropy">ShellOrthotropy. Optional, if undefined default value will be used.</param>
        /// <param name="shellEdgeConnection">ShellEdgeConnection. Optional, if undefined rigid.</param>
        /// <param name="localX">Set local x-axis. Vector must be perpendicular to surface local z-axis. Local y-axis will be adjusted accordingly. Optional, local x-axis from surface coordinate system used if undefined.</param>
        /// <param name="localZ">Set local z-axis. Vector must be perpendicular to surface local x-axis. Local y-axis will be adjusted accordingly. Optional, local z-axis from surface coordinate system used if undefined."</param>
        /// <param name="identifier">Identifier of plate element. Optional.</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        public static Slab Plate(Autodesk.DesignScript.Geometry.Surface surface, double thickness, Materials.Material material, [DefaultArgument("ShellEccentricity.Default()")] ShellEccentricity shellEccentricity, [DefaultArgument("ShellOrthotropy.Default()")] ShellOrthotropy shellOrthotropy, [DefaultArgument("ShellEdgeConnection.Default()")] ShellEdgeConnection shellEdgeConnection, [DefaultArgument("Autodesk.DesignScript.Geometry.Vector.ByCoordinates(0,0,0)")] Autodesk.DesignScript.Geometry.Vector localX,  [DefaultArgument("Autodesk.DesignScript.Geometry.Vector.ByCoordinates(0,0,0)")] Autodesk.DesignScript.Geometry.Vector localZ, string identifier = "P")
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
                slab.SlabPart.LocalX = FemDesign.Geometry.FdVector3d.FromDynamo(localX);
            }

            // set local z-axis
            if (!localZ.Equals(Autodesk.DesignScript.Geometry.Vector.ByCoordinates(0,0,0)))
            {
                slab.SlabPart.LocalZ = FemDesign.Geometry.FdVector3d.FromDynamo(localZ);
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
        /// <param name="shellEdgeConnection">ShellEdgeConnection. Optional, if undefined rigid.</param>
        /// <param name="localX">Set local x-axis. Vector must be perpendicular to surface local z-axis. Local y-axis will be adjusted accordingly. Optional, local x-axis from surface coordinate system used if undefined.</param>
        /// <param name="localZ">Set local z-axis. Vector must be perpendicular to surface local x-axis. Local y-axis will be adjusted accordingly. Optional, local z-axis from surface coordinate system used if undefined."</param>
        /// <param name="identifier">Identifier of plate element. Optional.</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        public static Slab PlateVariableThickness(Autodesk.DesignScript.Geometry.Surface surface, List<Thickness> thickness, Materials.Material material, [DefaultArgument("ShellEccentricity.Default()")] ShellEccentricity shellEccentricity, [DefaultArgument("ShellOrthotropy.Default()")] ShellOrthotropy shellOrthotropy, [DefaultArgument("ShellEdgeConnection.Default()")] ShellEdgeConnection shellEdgeConnection, [DefaultArgument("Autodesk.DesignScript.Geometry.Vector.ByCoordinates(0,0,0)")] Autodesk.DesignScript.Geometry.Vector localX,  [DefaultArgument("Autodesk.DesignScript.Geometry.Vector.ByCoordinates(0,0,0)")] Autodesk.DesignScript.Geometry.Vector localZ, string identifier = "P")
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
                slab.SlabPart.LocalX = FemDesign.Geometry.FdVector3d.FromDynamo(localX);
            }

            // set local z-axis
            if (!localZ.Equals(Autodesk.DesignScript.Geometry.Vector.ByCoordinates(0,0,0)))
            {
                slab.SlabPart.LocalZ = FemDesign.Geometry.FdVector3d.FromDynamo(localZ);
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
        /// <param name="shellEdgeConnection">ShellEdgeConnection. Optional, if undefined rigid.</param>
        /// <param name="localX">Set local x-axis. Vector must be perpendicular to surface local z-axis. Local y-axis will be adjusted accordingly. Optional, local x-axis from surface coordinate system used if undefined.</param>
        /// <param name="localZ">Set local z-axis. Vector must be perpendicular to surface local x-axis. Local y-axis will be adjusted accordingly. Optional, local z-axis from surface coordinate system used if undefined."</param>
        /// <param name="identifier">Identifier of wall element. Optional.</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        public static Slab Wall(Autodesk.DesignScript.Geometry.Surface surface, double thickness, Materials.Material material,  [DefaultArgument("ShellEccentricity.Default()")] ShellEccentricity shellEccentricity, [DefaultArgument("ShellOrthotropy.Default()")] ShellOrthotropy shellOrthotropy, [DefaultArgument("ShellEdgeConnection.Default()")] ShellEdgeConnection shellEdgeConnection, [DefaultArgument("Autodesk.DesignScript.Geometry.Vector.ByCoordinates(0,0,0)")] Autodesk.DesignScript.Geometry.Vector localX, [DefaultArgument("Autodesk.DesignScript.Geometry.Vector.ByCoordinates(0,0,0)")] Autodesk.DesignScript.Geometry.Vector localZ, string identifier = "W")
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
                slab.SlabPart.LocalX = FemDesign.Geometry.FdVector3d.FromDynamo(localX);
            }

            // set local z-axis
            if (!localZ.Equals(Autodesk.DesignScript.Geometry.Vector.ByCoordinates(0,0,0)))
            {
                slab.SlabPart.LocalZ = FemDesign.Geometry.FdVector3d.FromDynamo(localZ);
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
        /// <param name="shellEdgeConnection">ShellEdgeConnection. Optional, if undefined rigid.</param>
        /// <param name="localX">Set local x-axis. Vector must be perpendicular to surface local z-axis. Local y-axis will be adjusted accordingly. Optional, local x-axis from surface coordinate system used if undefined.</param>
        /// <param name="localZ">Set local z-axis. Vector must be perpendicular to surface local x-axis. Local y-axis will be adjusted accordingly. Optional, local z-axis from surface coordinate system used if undefined."</param>
        /// <param name="identifier">Identifier of wall element. Optional.</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        public static Slab WallVariableThickness(Autodesk.DesignScript.Geometry.Surface surface,  List<Thickness> thickness, Materials.Material material, [DefaultArgument("ShellEccentricity.Default()")] ShellEccentricity shellEccentricity, [DefaultArgument("ShellOrthotropy.Default()")] ShellOrthotropy shellOrthotropy, [DefaultArgument("ShellEdgeConnection.Default()")] ShellEdgeConnection shellEdgeConnection, [DefaultArgument("Autodesk.DesignScript.Geometry.Vector.ByCoordinates(0,0,0)")] Autodesk.DesignScript.Geometry.Vector localX, [DefaultArgument("Autodesk.DesignScript.Geometry.Vector.ByCoordinates(0,0,0)")] Autodesk.DesignScript.Geometry.Vector localZ, string identifier = "W")
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
                slab.SlabPart.LocalX = FemDesign.Geometry.FdVector3d.FromDynamo(localX);
            }

            // set local z-axis
            if (!localZ.Equals(Autodesk.DesignScript.Geometry.Vector.ByCoordinates(0,0,0)))
            {
                slab.SlabPart.LocalZ = FemDesign.Geometry.FdVector3d.FromDynamo(localZ);
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
            slab.SlabPart.LocalX = Geometry.FdVector3d.FromDynamo(localX);
            slab.SlabPart.LocalZ = Geometry.FdVector3d.FromDynamo(localZ);

            //
            return slab;
        }
        #endregion
    }
}