// https://strusoft.com/

using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace FemDesign.Shells
{
    /// <summary>
    /// slab_type
    /// </summary>
    [System.Serializable]
    public class Slab: EntityBase
    {
        private static int plateInstance = 0;
        private static int wallInstance = 0;
        [XmlIgnore]
        public Materials.Material material { get; set; }
        [XmlIgnore]
        public Reinforcement.SurfaceReinforcementParameters surfaceReinforcementParameters { get; set; }
        [XmlIgnore]
        public List<Reinforcement.SurfaceReinforcement> surfaceReinforcement = new List<Reinforcement.SurfaceReinforcement>();
        [XmlAttribute("name")]
        public string name {get; set;} // identifier
        [XmlAttribute("type")]
        public string _type; // slabtype
        [XmlIgnore]
        public string type
        {
            get {return this._type;}
            set {this._type = RestrictedString.SlabType(value);}
        }
        [XmlElement("slab_part", Order=1)]
        public SlabPart slabPart {get; set;}
        [XmlElement("end", Order=2)]
        public string end {get; set;} // empty_type

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
            this.name = name;
            this.type = type;
            this.slabPart = slabPart;
            this.material = material;
            this.end = "";
        }

        internal static Slab Plate(string identifier, Materials.Material material, Geometry.Region region, ShellEdgeConnection shellEdgeConnection, ShellEccentricity eccentricity, ShellOrthotropy orthotropy, List<Thickness> thickness)
        {
            plateInstance++;
            string type = "plate";
            string name = identifier + "." + plateInstance.ToString() + ".1";
            SlabPart slabPart = SlabPart.Define(name, region, thickness, material, shellEdgeConnection, eccentricity, orthotropy);
            Slab shell = new Slab(type, name, slabPart, material);
            return shell;
        }
        internal static Slab Wall(string identifier, Materials.Material material, Geometry.Region region, ShellEdgeConnection shellEdgeConnection, ShellEccentricity eccentricity, ShellOrthotropy orthotropy, List<Thickness> thickness)
        {
            // check if surface is vertical
            if (Math.Abs(region.coordinateSystem.localZ.z) > FemDesign.Tolerance.point3d)
            {
                throw new System.ArgumentException("Wall is not vertical! Create plate instead.");
            }
            
            wallInstance++;
            string type = "wall";
            string name = identifier + "." + wallInstance.ToString() + ".1";
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
        public static Slab SetShellEdgeConnection(Slab slab, ShellEdgeConnection shellEdgeConnection, List<int> indices)
        {
            // deep clone. downstreams objs will contain changes made in this method, upstream objs will not.
            // downstream and uppstream objs will share guid.
            Slab slabClone = slab.DeepClone();

            foreach (int index in indices)
            {
                if (index >= 0 & index < slab.slabPart.GetEdgeConnections().Count)
                {
                    // pass
                }
                else
                {
                    throw new System.ArgumentException("Index is out of bounds.");
                }
                
                //
                slabClone.slabPart.region.SetEdgeConnection(shellEdgeConnection, index);  

            }

            //
            return slabClone;          
        }
        /// <summary>
        /// Set average mesh size to slab. Note: Does not work for walls in FEM-Design 19.00.001.
        /// </summary>
        /// <remarks>Action</remarks>
        /// <param name="slab">Slab.</param>
        /// <param name="avgMeshSize">Average mesh size.</param>
        /// <returns></returns>
        public static Slab SetAverageSurfaceElementSize(Slab slab, double avgMeshSize)
        {
            // deep clone. downstreams objs will contain changes made in this method, upstream objs will not.
            // downstream and uppstream objs will share guid.
            Slab slabClone = slab.DeepClone();

            //
            slabClone.slabPart.meshSize = avgMeshSize;

            // return
            return slabClone;
        }
    }
}