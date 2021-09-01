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
    public partial class Slab: EntityBase, IStructureElement
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
        public SlabType Type { get; set; }
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
        private Slab(SlabType type, string name, SlabPart slabPart, Materials.Material material)
        {
            this.EntityCreated();
            this.Name = name;
            this.Type = type;
            this.SlabPart = slabPart;
            this.Material = material;
            this.End = "";
        }

        public static Slab Plate(string identifier, Materials.Material material, Geometry.Region region, ShellEdgeConnection shellEdgeConnection, ShellEccentricity eccentricity, ShellOrthotropy orthotropy, List<Thickness> thickness)
        {
            Slab._plateInstance++;
            SlabType type = SlabType.Plate;
            string name = identifier + "." + Slab._plateInstance.ToString() + ".1";
            SlabPart slabPart = SlabPart.Define(name, region, thickness, material, shellEdgeConnection, eccentricity, orthotropy);
            Slab shell = new Slab(type, name, slabPart, material);
            return shell;
        }
        public static Slab Wall(string identifier, Materials.Material material, Geometry.Region region, ShellEdgeConnection shellEdgeConnection, ShellEccentricity eccentricity, ShellOrthotropy orthotropy, List<Thickness> thickness)
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
        /// Set ShellEdgeConnections by indices.
        /// </summary>
        /// <param name="slab">Slab.</param>
        /// <param name="shellEdgeConnection">ShellEdgeConnection.</param>
        /// <param name="indices">Index. List of items. Use SlabDeconstruct to extract index for each respective edge.</param>
        public static Slab ShellEdgeConnection(Slab slab, ShellEdgeConnection shellEdgeConnection, List<int> indices)
        {
            // deep clone. downstreams objs will contain changes made in this method, upstream objs will not.
            // downstream and uppstream objs will share guid.
            Slab slabClone = slab.DeepClone();

            foreach (int index in indices)
            {
                if (index >= 0 & index < slabClone.SlabPart.GetEdgeConnections().Count)
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