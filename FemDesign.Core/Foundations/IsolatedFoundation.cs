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
    [System.Serializable]
    public partial class IsolatedFoundation : NamedEntityBase, IStructureElement, IFoundationElement, IStageElement
    {
        [XmlIgnore]
        internal static int _instance = 0;

        protected override int GetUniqueInstanceCount() => ++_instance;

        [XmlAttribute("bedding_modulus")]
        public double BeddingModulus { get; set; }

        [XmlAttribute("stage")]
        [DefaultValue(1)]
        public int StageId { get; set; } = 1;

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
        public FoundationSystem FoundationSystem { get; set; }

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
                if(value.Concrete == null) { throw new System.ArgumentException("Material type must be concrete"); }
                this._complexMaterialObj = value;
                this.ComplexMaterialRef = this._complexMaterialObj.Guid;
            }
        }

        #endregion


        private IsolatedFoundation()
        {
        }

#if !ISDYNAMO
        public IsolatedFoundation(ExtrudedSolid solid, double bedding, Materials.Material material, CoordinateSystem coordinateSystem, Insulation insulation = null, FoundationSystem foundationSystem = FoundationSystem.Simple, string identifier = "F")
        {
            this.Initialise(coordinateSystem, solid, bedding, material, identifier);
            this.BeddingModulus = bedding;
            this.Insulation = insulation;

            if (foundationSystem == FoundationSystem.FromSoil)
            {
                throw new InvalidEnumArgumentException("FromSoil is not a valid input for Isolated Foundation!");
            }
            this.FoundationSystem = foundationSystem;
            this.Parts = foundationSystem == FoundationSystem.SurfaceSupportGroup ? new RefParts(true) : new RefParts(false);
        }
#endif



        private void Initialise(CoordinateSystem coordinateSystem, ExtrudedSolid solid, double bedding, Materials.Material material, string identifier = "F")
        {
            this.EntityCreated();
            this.ConnectionPoint = coordinateSystem.Origin;
            this.Direction = coordinateSystem.LocalX;
            this.ExtrudedSolid = solid;
            this.ComplexMaterialObj = material;
            this.BeddingModulus = bedding;
            this.Identifier = identifier;
        }


    }
}
