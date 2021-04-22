// https://strusoft.com/

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
    public class SurfaceReinforcement: EntityBase
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
        private SurfaceReinforcement(Geometry.Region _region, Straight _straight, Centric _centric, Wire _wire)
        {
            // object information
            this.EntityCreated();

            // other properties
            this.Straight = _straight;
            this.Centric = _centric;
            this.Wire = _wire;
            this.Region = _region;
        }

        /// <summary>
        /// Create straight lay-out surface reinforcement.
        /// Internal static method used by GH components and Dynamo nodes.
        /// </summary>
        internal static SurfaceReinforcement DefineStraightSurfaceReinforcement(Geometry.Region region, Straight straight, Wire wire)
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
        internal static Shells.Slab AddStraightReinforcementToSlab(Shells.Slab slab, List<SurfaceReinforcement> _surfaceReinforcement)
        {
            // deep clone. downstreams objs will contain changes made in this method, upstream objs will not.
            // downstream and uppstream objs will share guid.
            Shells.Slab slabClone = slab.DeepClone();

            // check if slab material is concrete
            if (slabClone.Material.Concrete == null)
            {
                throw new System.ArgumentException("material of slab must be concrete");
            }

            //
            GuidListType baseShell = new GuidListType(slabClone.SlabPart.Guid);

            // check if surfaceReinforcementParameters are set to slab
            SurfaceReinforcementParameters _surfaceReinforcementParameters;
            if (slabClone.SurfaceReinforcementParameters == null)
            {
                _surfaceReinforcementParameters = SurfaceReinforcementParameters.Straight(slabClone);
                slabClone.SurfaceReinforcementParameters = _surfaceReinforcementParameters;
            }

            // any surfaceReinforcementParameter set to slab will be overwritten
            // any surfaceReinforcement with option "centric" will be removed
            else if (slabClone.SurfaceReinforcementParameters.Center.PolarSystem == true)
            {
                _surfaceReinforcementParameters = SurfaceReinforcementParameters.Straight(slabClone);
                slabClone.SurfaceReinforcementParameters = _surfaceReinforcementParameters;

                foreach (SurfaceReinforcement item in slabClone.SurfaceReinforcement)
                {
                    if (item.Centric != null)
                    {
                        slabClone.SurfaceReinforcement.Remove(item);
                    }
                }
            }

            // use surface parameters already set to slab
            else
            { 
                _surfaceReinforcementParameters = slabClone.SurfaceReinforcementParameters;
            }

            // add surface reinforcement
            FemDesign.GuidListType surfaceReinforcementParametersGuidReference = new FemDesign.GuidListType(slabClone.SurfaceReinforcementParameters.Guid);
            foreach (SurfaceReinforcement item in _surfaceReinforcement)
            {
                // add references to item
                item.BaseShell = baseShell;
                item.SurfaceReinforcementParametersGuid = surfaceReinforcementParametersGuidReference;

                // check if region item exists
                if (item.Region == null)
                {
                    item.Region = Geometry.Region.FromSlab(slabClone);
                }

                // add item to slab  
                slabClone.SurfaceReinforcement.Add(item);
            }

            // return
            return slabClone;
        }

    }
}