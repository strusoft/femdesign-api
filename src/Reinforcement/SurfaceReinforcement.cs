// https://strusoft.com/

using System.Xml.Serialization;
using System.Collections.Generic;

#region dynamo
using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Reinforcement
{
    /// <summary>
    /// surface_rf_type
    /// 
    /// Surface reinforcement
    /// </summary>
    [System.Serializable]
    [IsVisibleInDynamoLibrary(false)]
    public class SurfaceReinforcement: EntityBase
    {
        [XmlElement("base_shell", Order=1)]
        public GuidListType baseShell { get; set; } // guid_list_type // reference to slabPart of slab
        [XmlElement("surface_reinforcement_parameters", Order=2)]
        public FemDesign.GuidListType surfaceReinforcementParametersGuid { get; set; } // guid_list_type
        [XmlElement("straight", Order=3)]
        public Straight straight { get; set; } // choice
        [XmlElement("centric", Order=4)]
        public Centric centric { get; set; } // next choice
        [XmlElement("wire", Order=5)] 
        public Wire wire { get; set; } // rf_wire_type
        [XmlElement("region", Order=6)]
        public Geometry.Region region { get; set; } // region_type

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
            this.straight = _straight;
            this.centric = _centric;
            this.wire = _wire;
            this.region = _region;
        }

        /// <summary>
        /// Create straight lay-out surface reinforcement.
        /// Internal static method used by GH components and Dynamo nodes.
        /// </summary>
        internal static SurfaceReinforcement Straight(Geometry.Region region, Straight straight, Wire wire)
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
            if (slabClone.material.concrete == null)
            {
                throw new System.ArgumentException("material of slab must be concrete");
            }

            //
            GuidListType baseShell = new GuidListType(slabClone.slabPart.guid);

            // check if surfaceReinforcementParameters are set to slab
            SurfaceReinforcementParameters _surfaceReinforcementParameters;
            if (slabClone.surfaceReinforcementParameters == null)
            {
                _surfaceReinforcementParameters = SurfaceReinforcementParameters.Straight(slabClone);
                slabClone.surfaceReinforcementParameters = _surfaceReinforcementParameters;
            }

            // any surfaceReinforcementParameter set to slab will be overwritten
            // any surfaceReinforcement with option "centric" will be removed
            else if (slabClone.surfaceReinforcementParameters.center.polarSystem == true)
            {
                _surfaceReinforcementParameters = SurfaceReinforcementParameters.Straight(slabClone);
                slabClone.surfaceReinforcementParameters = _surfaceReinforcementParameters;

                foreach (SurfaceReinforcement item in slabClone.surfaceReinforcement)
                {
                    if (item.centric != null)
                    {
                        slabClone.surfaceReinforcement.Remove(item);
                    }
                }
            }

            // use surface parameters already set to slab
            else
            { 
                _surfaceReinforcementParameters = slabClone.surfaceReinforcementParameters;
            }

            // add surface reinforcement
            FemDesign.GuidListType surfaceReinforcementParametersGuidReference = new FemDesign.GuidListType(slabClone.surfaceReinforcementParameters.guid);
            foreach (SurfaceReinforcement item in _surfaceReinforcement)
            {
                // add references to item
                item.baseShell = baseShell;
                item.surfaceReinforcementParametersGuid = surfaceReinforcementParametersGuidReference;

                // check if region item exists
                if (item.region == null)
                {
                    item.region = Geometry.Region.FromSlab(slabClone);
                }

                // add item to slab  
                slabClone.surfaceReinforcement.Add(item);
            }

            // return
            return slabClone;
        }

        #region dynamo
        /// <summary>
        /// Create straight surface reinforcement for a portion of a slab. This surface reinforcement will cover the passed surface of any slab it is applied to. Note that the surface must be contained within the target slab.
        /// </summary>
        /// <param name="surface">Surface.</param>
        /// <param name="straight">Straight reinforcement layout.</param>
        /// <param name="wire">Wire.</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        public static SurfaceReinforcement BySurface(Surface surface, Straight straight, Wire wire)
        {
            // convert geometry
            Geometry.Region region = Geometry.Region.FromDynamo(surface);

            // return
            return SurfaceReinforcement.Straight(region, straight, wire);
        }
        
        /// <summary>
        /// Add surface reinforcement to slab.
        /// </summary>
        /// <remarks>Action</remarks>
        /// <param name="slab">Slab.</param>
        /// <param name="surfaceReinforcement">SurfaceReinforcment to add to slab. Item or list. Nested lists are not supported.</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        public static Shells.Slab AddToSlab(Shells.Slab slab, List<SurfaceReinforcement> surfaceReinforcement)
        {
            // return
            return SurfaceReinforcement.AddStraightReinforcementToSlab(slab, surfaceReinforcement);
        }
        #endregion
    }
}