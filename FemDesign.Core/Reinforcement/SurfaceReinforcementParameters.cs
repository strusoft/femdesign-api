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
    public partial class SurfaceReinforcementParameters: EntityBase
    {
        [XmlAttribute("single_layer_reinforcement")]
        public bool SingleLayerReinforcement { get; set; } // bool. Default = false

        [XmlElement("base_shell", Order=1)]
        public GuidListType BaseShell { get; set; } // guid_list_type // reference to slabPart of slab
        [XmlElement("center", Order=2)]
        public Center Center { get; set; }
        [XmlElement("x_direction", Order=3)]
        public Geometry.Vector3d XDirection { get; set; } // point_type_3d
        [XmlElement("y_direction", Order = 4)]
        public Geometry.Vector3d YDirection { get; set; } // point_type_3d

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private SurfaceReinforcementParameters()
        {

        }

        /// <summary>
        /// Private constructor accessed by static methods.
        /// </summary>
        private SurfaceReinforcementParameters(bool singleLayerReinforcement, GuidListType baseShell, Center center, Geometry.Vector3d xDirection, Geometry.Vector3d yDirection)
        {
            // object information
            this.EntityCreated();

            // single layer reinforcement?
            if (singleLayerReinforcement)
            {
                this.SingleLayerReinforcement = singleLayerReinforcement;
            }

            // other properties
            this.BaseShell = baseShell;
            this.Center = center;
            this.XDirection = xDirection;
            this.YDirection = yDirection;
        }

        /// <summary>
        /// Straight reinforcement layout on slab.
        /// </summary>
        public static SurfaceReinforcementParameters Straight(Shells.Slab slab, bool singleLayerReinforcement = false)
        {
            GuidListType baseShell = new GuidListType(slab.SlabPart.Guid);
            Center center = Center.Straight();
            Geometry.Vector3d xDirection = slab.SlabPart.LocalX;
            Geometry.Vector3d yDirection = slab.SlabPart.LocalY;
            return new SurfaceReinforcementParameters(singleLayerReinforcement, baseShell, center, xDirection, yDirection);
        }
    }
}