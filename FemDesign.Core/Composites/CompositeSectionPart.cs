// https://strusoft.com/

using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Linq;

namespace FemDesign.Composites
{
    [System.Serializable]
    public partial class CompositeSectionPart
    {
        [XmlAttribute("material")]
        public Guid _materialRef;

        [XmlIgnore]
        public Guid MaterialRef
        {
            get { return this._materialRef; }
            set { this._materialRef = value; }
        }

        [XmlIgnore]
        public Materials.Material _material;

        [XmlIgnore]
        public Materials.Material Material
        {
            get
            {
                return this._material;
            }
            set
            {
                this._material = value;
                this._materialRef = value.Guid;
            }
        }

        [XmlAttribute("section")]
        public Guid _sectionRef;

        [XmlIgnore]
        public Guid SectionRef
        {
            get { return this._sectionRef; }
            set { this._sectionRef = value; }
        }

        [XmlIgnore]
        public Sections.Section _section;

        [XmlIgnore]
        public Sections.Section Section
        {
            get 
            { 
                return this._section; 
            }
            set
            {
                this._section = value;
                this._sectionRef = value.Guid;
            }
        }

        [XmlAttribute("cog_offset_x")]
        public double _offsetY;

        [XmlIgnore]
        public double OffsetY
        {
            get { return _offsetY; }
            set { _offsetY = value; }
        }

        [XmlAttribute("cog_offset_y")]
        public double _offsetZ;

        [XmlIgnore]
        public double OffsetZ
        {
            get { return _offsetZ; }
            set { _offsetZ = value; }
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private CompositeSectionPart()
        {

        }

        /// <summary>
        /// Construct a new composite section part with offset. This is for composite section types where the centre of the concrete and steel sections are not at the same point (e.g. beam composite types)
        /// </summary>
        /// <param name="material">Material of composite section part. Can be steel or concrete.</param>
        /// <param name="section">Section part.</param>
        /// <param name="offsetY">Offset of concrete section's centre from steel section's center in Y direction. It must be expressed in meter.</param>
        /// <param name="offsetZ">Offset of concrete section's centre from steel section's center in Z direction. It must be expressed in meter.</param>
        public CompositeSectionPart(Materials.Material material, Sections.Section section, double offsetY, double offsetZ)
        {
            this.Material = material;
            this.Section = section;
            this.OffsetY = offsetY;
            this.OffsetZ = offsetZ;
        }

        /// <summary>
        /// Construct a new composite section part without offset. This is for composite section types where the centre of the concrete and steel sections are at the same point (e.g. column composite types).
        /// </summary>
        /// <param name="type">CompositeType enum member.</param>
        /// <param name="material">Material of composite section part. Can be steel or concrete.</param>
        /// <param name="section">Section part.</param>
        /// <exception cref="ArgumentException"></exception>
        public CompositeSectionPart(CompositeType type, Materials.Material material, Sections.Section section)
        {


            // Check material >> it must be concrete or steel
            // Check section shape >> e.g. for ColumnD, only "VKR" or "KKR" section types can be used as a steel part
            // Check section by material 

            if (IsOffsetNeeded(type))
            {
                throw new ArgumentException("Offset is required. For this type of composite section, the distance of concrete section part from the steel part must be defined.");
            }
            this.Material = material;
            this.Section = section;
        }

        public bool IsOffsetNeeded(CompositeType type)
        {
            switch(type)
            {
                case CompositeType.BeamA: 
                    return true;
                case CompositeType.BeamB: 
                    return true;
                case CompositeType.BeamP:
                    return true;
                case CompositeType.ColumnA:
                    return false;
                case CompositeType.ColumnC:
                    return false;
                case CompositeType.ColumnD:
                    return false;
                case CompositeType.ColumnE:
                    return false;
                case CompositeType.ColumnF:
                    return false;
                case CompositeType.ColumnG:
                    return false;
                default:
                    throw new ArgumentException("Incorrect or unknown type.");
            }
        }
    }
}
