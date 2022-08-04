// https://strusoft.com/

using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using FemDesign.GenericClasses;

namespace FemDesign.Shells
{
    /// <summary>
    /// slab_type
    /// </summary>
    [System.Serializable]
    public partial class Slab : EntityBase, IStructureElement, IStageElement
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
        public string Identifier { get; set; } // identifier
        [XmlAttribute("type")]
        public SlabType Type { get; set; }

        [XmlAttribute("stage")]
        public int StageId { get; set; } = 1;

        [XmlElement("slab_part", Order = 1)]
        public SlabPart SlabPart { get; set; }
        [XmlElement("end", Order = 2)]
        public string End { get; set; } // empty_type

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private Slab()
        {

        }

        /// <summary>
        /// Construct Slab.
        /// </summary>
        private Slab(SlabType type, string name, SlabPart slabPart, Materials.Material material)
        {
            this.EntityCreated();
            this.Identifier = name;
            this.Type = type;
            this.SlabPart = slabPart;
            this.Material = material;
            this.End = "";
        }

        public static Slab Plate(string identifier, Materials.Material material, Geometry.Region region, EdgeConnection shellEdgeConnection, ShellEccentricity eccentricity, ShellOrthotropy orthotropy, List<Thickness> thickness)
        {
            Slab._plateInstance++;
            SlabType type = SlabType.Plate;
            string name = identifier + "." + Slab._plateInstance.ToString() + ".1";
            SlabPart slabPart = SlabPart.Define(name, region, thickness, material, shellEdgeConnection, eccentricity, orthotropy);
            Slab shell = new Slab(type, name, slabPart, material);
            return shell;
        }

        public static Slab Plate(double width, double height, double thickness, Materials.Material material, EdgeConnection shellEdgeConnection = null, ShellEccentricity eccentricity = null, ShellOrthotropy orthotropy = null, string identifier = "Plate")
        {
            Slab._plateInstance++;
            SlabType type = SlabType.Plate;
            string name = identifier + "." + Slab._wallInstance.ToString() + ".1";
            var region = Geometry.Region.RectangleXY(width, height);

            List<FemDesign.Shells.Thickness> thicknessObj = new List<FemDesign.Shells.Thickness>();
            thicknessObj.Add(new FemDesign.Shells.Thickness(region.CoordinateSystem.Origin, thickness));

            SlabPart slabPart = SlabPart.Define(name, region, thicknessObj, material, shellEdgeConnection, eccentricity, orthotropy);
            Slab shell = new Slab(type, name, slabPart, material);
            return shell;
        }

        public static Slab Wall(string identifier, Materials.Material material, Geometry.Region region, EdgeConnection shellEdgeConnection, ShellEccentricity eccentricity, ShellOrthotropy orthotropy, List<Thickness> thickness)
        {
            // check if surface is vertical
            if (Math.Abs(region.CoordinateSystem.LocalZ.Z) > FemDesign.Tolerance.Point3d)
            {
                throw new System.ArgumentException("Wall is not vertical! Create plate instead.");
            }

            Slab._wallInstance++;
            SlabType type = SlabType.Wall;
            string name = identifier + "." + Slab._wallInstance.ToString() + ".1";
            SlabPart slabPart = SlabPart.Define(name, region, thickness, material, shellEdgeConnection, eccentricity, orthotropy);
            Slab shell = new Slab(type, name, slabPart, material);
            return shell;
        }
        /// <summary>
        /// Construct a Wall Element in XZ plane
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="thickness"></param>
        /// <param name="material"></param>
        /// <param name="shellEdgeConnection"></param>
        /// <param name="eccentricity"></param>
        /// <param name="orthotropy"></param>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public static Slab Wall(double width, double height, double thickness, Materials.Material material, EdgeConnection shellEdgeConnection = null, ShellEccentricity eccentricity = null, ShellOrthotropy orthotropy = null, string identifier = "Wall")
        {
            Slab._wallInstance++;
            SlabType type = SlabType.Wall;
            string name = identifier + "." + Slab._wallInstance.ToString() + ".1";
            var region = Geometry.Region.RectangleXZ(width, height);

            List<FemDesign.Shells.Thickness> thicknessObj = new List<FemDesign.Shells.Thickness>();
            thicknessObj.Add(new FemDesign.Shells.Thickness(region.CoordinateSystem.Origin, thickness));

            SlabPart slabPart = SlabPart.Define(name, region, thicknessObj, material, shellEdgeConnection, eccentricity, orthotropy);
            Slab shell = new Slab(type, name, slabPart, material);
            return shell;
        }
        /// <summary>
        /// Set EdgeConnections by indices.
        /// </summary>
        /// <param name="slab">Slab.</param>
        /// <param name="shellEdgeConnection">EdgeConnection.</param>
        /// <param name="indices">Index. List of items. Use SlabDeconstruct to extract index for each respective edge.</param>
        public static Slab EdgeConnection(Slab slab, EdgeConnection shellEdgeConnection, List<int> indices)
        {
            // deep clone. downstreams objs will contain changes made in this method, upstream objs will not.
            // downstream and uppstream objs will share guid.
            Slab slabClone = slab.DeepClone();

            foreach (int index in indices)
            {
                if (index < 0 & index >= slabClone.SlabPart.GetEdgeConnections().Count)
                    throw new System.ArgumentException("Index is out of bounds.");

                slabClone.SlabPart.Region.SetEdgeConnection(shellEdgeConnection, index);
            }

            return slabClone;
        }

        /// <summary>
        /// Set EdgeConnections by indices.
        /// </summary>
        /// <param name="slab">Slab.</param>
        /// <param name="shellEdgeConnection">EdgeConnection.</param>
        /// <param name="index">Index of edge to set.</param>
        public static Slab EdgeConnection(Slab slab, EdgeConnection shellEdgeConnection, int index)
        {
            // deep clone. downstreams objs will contain changes made in this method, upstream objs will not.
            // downstream and uppstream objs will share guid.
            Slab slabClone = slab.DeepClone();

            if (index < 0 & index >= slabClone.SlabPart.GetEdgeConnections().Count)
                throw new System.ArgumentException("Index is out of bounds.");

            slabClone.SlabPart.Region.SetEdgeConnection(shellEdgeConnection, index);

            return slabClone;
        }

        /// <summary>
        /// Set average mesh size to slab.
        /// </summary>
        /// <remarks>Action</remarks>
        /// <param name="slab">Slab.</param>
        /// <param name="avgMeshSize">Average mesh size.</param>
        /// <returns></returns>
        public static Slab AverageSurfaceElementSize(Slab slab, double avgMeshSize)
        {
            // deep clone. downstreams objs will contain changes made in this method, upstream objs will not.
            // downstream and uppstream objs will share guid.
            Slab slabClone = slab.DeepClone();

            //
            slabClone.SlabPart.MeshSize = avgMeshSize;

            // return
            return slabClone;
        }
    }
}