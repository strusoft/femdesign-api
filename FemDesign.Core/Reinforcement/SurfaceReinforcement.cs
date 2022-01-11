// https://strusoft.com/

using System.Linq;
using System.Xml.Serialization;
using System.Collections.Generic;


namespace FemDesign.Reinforcement
{
    /// <summary>
    /// surface_rf_type
    /// 
    /// Surface reinforcement
    /// </summary>
    [System.Serializable]
    public partial class SurfaceReinforcement: EntityBase
    {
        [XmlElement("base_shell", Order=1)]
        public GuidListType BaseShell { get; set; } // guid_list_type // reference to slabPart of slab
        [XmlElement("surface_reinforcement_parameters", Order=2)]
        public FemDesign.GuidListType SurfaceReinforcementParametersGuid { get; set; } // guid_list_type
        [XmlElement("straight", Order=3)]
        public Straight Straight { get; set; } // choice
        [XmlElement("centric", Order=4)]
        public Centric Centric { get; set; } // next choice
        [XmlElement("wire", Order=5)] 
        public Wire Wire { get; set; } // rf_wire_type
        [XmlElement("region", Order=6)]
        public Geometry.Region Region { get; set; } // region_type

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private SurfaceReinforcement()
        {

        }

        /// <summary>
        /// Private constructor accessed by static methods.
        /// </summary>        
        private SurfaceReinforcement(Geometry.Region region, Straight straight, Centric centric, Wire wire)
        {
            // object information
            this.EntityCreated();

            // other properties
            this.Straight = straight;
            this.Centric = centric;
            this.Wire = wire;
            this.Region = region;
        }

        /// <summary>
        /// Create straight lay-out surface reinforcement.
        /// Internal static method used by GH components and Dynamo nodes.
        /// </summary>
        public static SurfaceReinforcement DefineStraightSurfaceReinforcement(Geometry.Region region, Straight straight, Wire wire)
        {
            // set straight (e.g. centric == null)
            Centric centric = null;

            // new surfaceReinforcement
            SurfaceReinforcement obj = new SurfaceReinforcement(region, straight, centric, wire);

            // return
            return obj;
        }

        /// <summary>
        /// Add SurfaceReinforcement to slab.
        /// Internal method use by GH components and Dynamo nodes.
        /// </summary>
        public static Shells.Slab AddReinforcementToSlab(Shells.Slab slab, List<SurfaceReinforcement> srfReinfs)
        {
            // deep clone. downstreams objs will contain changes made in this method, upstream objs will not.
            // downstream and uppstream objs will share guid.
            Shells.Slab clone = slab.DeepClone();

            // check if slab material is concrete
            if (clone.Material.Concrete == null)
            {
                throw new System.ArgumentException("Material of slab must be concrete");
            }

            // check if mixed layers
            if (SurfaceReinforcement.MixedLayers(srfReinfs))
            {
                throw new System.ArgumentException("Can't add mixed layers to the same slab");
            }

            if (SurfaceReinforcement.AllStraight(srfReinfs))
            {
                return SurfaceReinforcement.AddStraightReinfToSlab(clone, srfReinfs);
            }
            else if (SurfaceReinforcement.AllCentric(srfReinfs))
            {
                return SurfaceReinforcement.AddCentricReinfToSlab(clone, srfReinfs);
            }
            else
            {
                throw new System.ArgumentException("Can't add mixed surface reinforcement layouts to the same slab.");
            }
        } 
        
        private static Shells.Slab AddStraightReinfToSlab(Shells.Slab slab, List<SurfaceReinforcement> srfReinfs)
        {
            // assert layout
            if (SurfaceReinforcement.AllStraight(srfReinfs))
            {
                
            }
            else
            {
                throw new System.ArgumentException("Not all passed surface reinforcement objects are of layout type straight");
            }

            // assert layers
            if (SurfaceReinforcement.MixedLayers(srfReinfs))
            {
                throw new System.ArgumentException("Can't add mixed layers to the same slab");
            }

            // single layer?
            var singleLayer = SurfaceReinforcement.AllSingleLayer(srfReinfs);

            // check if surface reinf parameters are set to slab
            SurfaceReinforcementParameters srfReinfParams;
            if (slab.SurfaceReinforcementParameters == null)
            {
                srfReinfParams = SurfaceReinforcementParameters.Straight(slab, singleLayer);
                slab.SurfaceReinforcementParameters = srfReinfParams;
            }

            // any surfaceReinforcementParameter set to slab will be overwritten
            // any surfaceReinforcement with option "centric" will be removed
            else if (slab.SurfaceReinforcementParameters.Center.PolarSystem == true)
            {
                srfReinfParams = SurfaceReinforcementParameters.Straight(slab);
                slab.SurfaceReinforcementParameters = srfReinfParams;

                foreach (SurfaceReinforcement item in slab.SurfaceReinforcement)
                {
                    if (item.Centric != null)
                    {
                        slab.SurfaceReinforcement.Remove(item);
                    }
                }
            }

            // use surface parameters already set to slab
            else
            {
                srfReinfParams = slab.SurfaceReinforcementParameters;
            }

            // add surface reinforcement
            GuidListType baseShell = new GuidListType(slab.SlabPart.Guid);
            FemDesign.GuidListType surfaceReinforcementParametersGuidReference = new FemDesign.GuidListType(slab.SurfaceReinforcementParameters.Guid);
            foreach (SurfaceReinforcement item in srfReinfs)
            {
                // add references to item
                item.BaseShell = baseShell;
                item.SurfaceReinforcementParametersGuid = surfaceReinforcementParametersGuidReference;

                // check if region item exists
                if (item.Region == null)
                {
                    item.Region = Geometry.Region.FromSlab(slab);
                }

                // add item to slab  
                slab.SurfaceReinforcement.Add(item);
            }

            // return
            return slab;
        }

        private static Shells.Slab AddCentricReinfToSlab(Shells.Slab slab, List<SurfaceReinforcement> srfReinfs)
        {
            if (SurfaceReinforcement.AllCentric(srfReinfs))
            {
                throw new System.ArgumentException("Method to add centric surface reinforcement is not implemented yet.");
            }
            else
            {
                throw new System.ArgumentException("Not all passed surface reinforcement objects are of layout type centric");
            }
        }

        // check if surface reinforcement objects in list are mixed.
        private static bool MixedLayers(List<SurfaceReinforcement> srfReinfs)
        {
            return SurfaceReinforcement.AllMultiLayer(srfReinfs) == false && SurfaceReinforcement.AllSingleLayer(srfReinfs) == false;
        }
    
        // check if all surface reinforcement objects in list are multilayer
        private static bool AllMultiLayer(List<SurfaceReinforcement> srfReinfs)
        {
            var items = srfReinfs.Where(x => x.Straight != null).Where(x => x.Straight.MultiLayer == true).ToList();
            items.AddRange(srfReinfs.Where(x => x.Centric != null).Where(x => x.Centric.MultiLayer == true));
            return (items.Count() == srfReinfs.Count());
        }

        // check if all surface reinforcement objects in list are singlelayer
        private static bool AllSingleLayer(List<SurfaceReinforcement> srfReinfs)
        {
            var items = srfReinfs.Where(x => x.Straight != null).Where(x => x.Straight.SingleLayer == true).ToList();
            items.AddRange(srfReinfs.Where(x => x.Centric != null).Where(x => x.Centric.SingleLayer == true));
            return (items.Count() == srfReinfs.Count());
        }

        private static bool MixedLayout(List<SurfaceReinforcement> srfReinfs)
        {
            return srfReinfs.Any(x => x.Straight != null) && srfReinfs.Any(x => x.Centric != null);
        }

        // check if all surface reinforcement objects in list are of layout type straight
        private static bool AllStraight(List<SurfaceReinforcement> srfReinfs)
        {
            var items = srfReinfs.Where(x => x.Straight != null);
            return (items.Count() == srfReinfs.Count());
        }
        
        // check if all surface reinforcement objects in list are of layout type centric
        private static bool AllCentric(List<SurfaceReinforcement> srfReinfs)
        {
            var items = srfReinfs.Where(x => x.Centric != null);
            return (items.Count() == srfReinfs.Count());
        }
    }
}