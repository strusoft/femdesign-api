using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.ComponentModel;

using FemDesign.GenericClasses;
using FemDesign.Geometry;


namespace FemDesign.Foundations
{
    public partial class IsolatedFoundation : NamedEntityBase, IStructureElement, IStageElement
    {
        [XmlIgnore]
        internal static int _instance = 0;
        protected override int GetUniqueInstanceCount() => ++_instance;

        [XmlAttribute("bedding_modulus")]
        public double BeddingModulus { get; set; }

        [XmlAttribute("stage")]
        [DefaultValue("1")]
        public int StageId { get; set; }

        [XmlElement("connection_point", Order = 1)]
        public Point3d ConnectionPoint { get; set; }

        [XmlElement("direction", Order = 2)]
        public Vector3d Direction { get; set; }

        [XmlElement("extruded_solid", Order = 3)]
        public ExtrudedSolid ExtrudedSolid { get; set; }

        [XmlElement("referable_parts", Order = 4)]
        public RefParts Parts { get; set; }
        
        [XmlElement("insulation", Order = 5)]
        public Insulation Insulation { get; set; }

        [XmlAttribute("analythical_system")]
        [DefaultValue(FoundationSystem.Simple)]
        public FoundationSystem FoundationSystem => FoundationSystem.Simple;

        #region MATERIAL

        [XmlAttribute("complex_material")]
        public string _complexMaterialRef;

        [XmlIgnore]
        public System.Guid ComplexMaterialRef
        {
            get
            {
                return System.Guid.Parse(this._complexMaterialRef);
            }
            set
            {
                this._complexMaterialRef = value.ToString();
            }
        }

        [XmlIgnore]
        public Materials.Material _complexMaterialObj;

        [XmlIgnore]
        public Materials.Material ComplexMaterialObj
        {
            get
            {
                return this._complexMaterialObj;
            }
            set
            {
                this._complexMaterialObj = value;
                this.ComplexMaterialRef = this._complexMaterialObj.Guid;
            }
        }

        #endregion


        private IsolatedFoundation()
        {
        }

        public IsolatedFoundation(Point3d point, ExtrudedSolid solid, double bedding, string identifier = "F")
        {
            this.EntityCreated();
            this.ConnectionPoint = point;
            this.Direction = Vector3d.UnitY;
            this.ExtrudedSolid = solid;
            this.BeddingModulus = bedding;
            this.Insulation = null;
            this.Identifier = identifier;
        }

        public IsolatedFoundation(Point3d point, ExtrudedSolid solid, Vector3d direction, double bedding, Insulation insulation, string identifier = "F") : this(point, solid, bedding, identifier)
        {
            this.EntityCreated();
            this.ConnectionPoint = point;
            this.Direction = direction;
            this.ExtrudedSolid = solid;
            this.BeddingModulus = bedding;
            this.Insulation = insulation;
            this.Identifier = identifier;
        }

    }
}
