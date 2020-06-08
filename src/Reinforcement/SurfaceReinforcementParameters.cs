// https://strusoft.com/

using System.Xml.Serialization;


namespace FemDesign.Reinforcement
{
    /// <summary>
    /// shell_rf_params_type
    /// 
    /// Shell reinforcement parameters
    /// </summary>
    [System.Serializable]
    public class SurfaceReinforcementParameters: EntityBase
    {
        [XmlAttribute("single_layer_reinforcement")]
        public bool singleLayerReinforcement { get; set; } // bool. Default = false

        [XmlElement("base_shell", Order=1)]
        public GuidListType baseShell { get; set; } // guid_list_type // reference to slabPart of slab
        [XmlElement("center", Order=2)]
        public Center center { get; set; }
        [XmlElement("x_direction", Order=3)]
        public Geometry.FdVector3d xDirection { get; set; } // point_type_3d
        [XmlElement("y_direction", Order = 4)]
        public Geometry.FdVector3d yDirection { get; set; } // point_type_3d

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private SurfaceReinforcementParameters()
        {

        }

        /// <summary>
        /// Private constructor accessed by static methods.
        /// </summary>
        private SurfaceReinforcementParameters(bool singleLayerReinforcement, GuidListType baseShell, Center center, Geometry.FdVector3d xDirection, Geometry.FdVector3d yDirection)
        {
            // object information
            this.EntityCreated();

            // single layer reinforcement?
            if (singleLayerReinforcement)
            {
                this.singleLayerReinforcement = singleLayerReinforcement;
            }

            // other properties
            this.baseShell = baseShell;
            this.center = center;
            this.xDirection = xDirection;
            this.yDirection = yDirection;
        }

        /// <summary>
        /// Straight reinforcement layout on slab.
        /// </summary>
        public static SurfaceReinforcementParameters Straight(Shells.Slab slab, bool singleLayerReinforcement = false)
        {
            GuidListType baseShell = new GuidListType(slab.slabPart.guid);
            Center center = Center.Straight();
            Geometry.FdVector3d xDirection = slab.slabPart.localX;
            Geometry.FdVector3d yDirection = slab.slabPart.localY;
            return new SurfaceReinforcementParameters(singleLayerReinforcement, baseShell, center, xDirection, yDirection);
        }
    }
}