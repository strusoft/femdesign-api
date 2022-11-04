// https://strusoft.com/

using System.Xml.Serialization;
using FemDesign.GenericClasses;
using FemDesign.Releases;


namespace FemDesign.Supports
{
    [System.Serializable]
    public partial class SurfaceSupport: NamedEntityBase, IStructureElement, ISupportElement, IStageElement
    {
        protected override int GetUniqueInstanceCount() => ++PointSupport._instance; // PointSupport and SurfaceSupport share the same instance counter.

        [XmlAttribute("stage")]
        public int StageId { get; set; } = 1;

        [XmlElement("region", Order= 1)]
        public Geometry.Region Region { get; set; }
        
        [XmlElement("rigidity", Order= 2)]
        public RigidityDataType1 Rigidity { get; set; }

        [XmlElement("predefined_rigidity", Order = 3)]
        public GuidListType _predefRigidityRef; // reference_type

        [XmlIgnore]
        public RigidityDataLibType1 _predefRigidity;

        [XmlIgnore]
        public RigidityDataLibType1 PredefRigidity
        {
            get
            {
                return this._predefRigidity;
            }
            set
            {
                this._predefRigidity = value;
                this._predefRigidityRef = new GuidListType(value.Guid);
            }
        }

        [XmlElement("local_system", Order= 4)]
        public Geometry.CoordinateSystem CoordinateSystem { get; set; }
        public Motions Motions { get { return Rigidity?.Motions; } }
        public MotionsPlasticLimits MotionsPlasticityLimits { get { return Rigidity?.PlasticLimitForces; } }

        /// <summary>
        /// Parameterless constructor for serialization
        /// </summary>
        private SurfaceSupport()
        {

        }

        /// <summary>
        /// Create surface support
        /// </summary>
        public SurfaceSupport(Geometry.Region region, RigidityDataType1 rigidity, string identifier = "S")
        {
            Initialize(region, rigidity, identifier);
        }

        /// <summary>
        /// Create surface support with only translation rigidity defined. 
        /// </summary>
        public SurfaceSupport(Geometry.Region region, Motions motions, string identifier = "S")
        {
            var rigidity = new RigidityDataType1(motions);
            Initialize(region, rigidity, identifier);
        }

        /// <summary>
        /// Create surface support with only translation rigidity and force plastic limits defined. 
        /// </summary>
        public SurfaceSupport(Geometry.Region region, Motions motions, MotionsPlasticLimits motionsPlasticLimits, string identifier = "S")
        {
            var rigidity = new RigidityDataType1(motions, motionsPlasticLimits);
            Initialize(region, rigidity, identifier);
        }

        private void Initialize(Geometry.Region region, RigidityDataType1 rigidity, string identifier)
        {
            this.EntityCreated();
            this.Identifier = identifier;
            this.Region = region;
            this.Rigidity = rigidity;
            this.CoordinateSystem = region.CoordinateSystem;
        }

        public override string ToString()
        {
            bool hasPlasticLimit = false;
            if (this.Rigidity != null)
			{
                if(this.Rigidity.PlasticLimitForces != null)
                    hasPlasticLimit = true;
                return $"{this.GetType().Name} {this.Rigidity.Motions}, PlasticLimit: {hasPlasticLimit}";
			}
            else
                return $"{this.GetType().Name} {this.PredefRigidity.Rigidity.Motions}, PlasticLimit: {hasPlasticLimit}";
        }
    }
}