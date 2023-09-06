// https://strusoft.com/

using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Linq;

using FemDesign.Geometry;

namespace FemDesign.Composites
{
    [System.Serializable]
    public partial class CompositeSection : EntityBase
    {
        [XmlAttribute("type")]
        public CompositeType _compositeType;

        [XmlIgnore]
        public CompositeType CompositeType
        {
            get 
            { 
                return this._compositeType; 
            }
            set 
            {
                this._compositeType = value;
            }
        }
                
        [XmlElement("part", Order = 1)]
        public List<CompositeSectionPart> Parts { get; set; }

        [XmlElement("property", Order = 2)]
        public List<CompositeSectionParameter> ParameterList { get; set; }

        [XmlIgnore]
        public Dictionary<CompositeParameterType, string> ParameterDictionary 
        { 
            get
            {
                return ParameterList.ToDictionary(p => p.Name, p => p.Value);
            }
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private CompositeSection()
        {

        }

        /// <summary>
        /// Create a composite section with offset. Use this constructor for beam types. For column types offset must be 0.
        /// </summary>
        /// <param name="type">CompositeType enum member.</param>
        /// <param name="materials">The list of materials corresponding to each composite section part.</param>
        /// <param name="sections">The list of sections corresponding to each composite section part.</param>
        /// <param name="offsetY">Offset of center of each section from center of steel section in Y direction. It must be expressed in meter.</param>
        /// <param name="offsetZ">Offset of center of each section from center of steel section in Z direction.  It must be expressed in meter.</param>
        /// <param name="parameters">List of composite section parameters describing the composite section (e.g. name, geometry, etc.). Values of geometry parameters must be expressed in milimeters.</param>
        /// <exception cref="ArgumentException"></exception>
        public CompositeSection(CompositeType type, List<Materials.Material> materials, List<Sections.Section> sections, List<double> offsetY, List<double> offsetZ, List<CompositeSectionParameter> parameters)
        {
            // check incoming data
            if ((materials.Count != sections.Count) || (materials.Count != offsetY.Count) || (materials.Count != offsetZ.Count))
            {
                throw new ArgumentException("Input variables of materials, sections, offsetY and offsetZ must have the same length.");
            }

            this.EntityCreated();
            this.CompositeType = type;
            this.ParameterList = parameters;
            this.Parts = new List<CompositeSectionPart>();
            
            for (int i = 0; i < materials.Count; i++)
            {
                this.Parts.Add(new CompositeSectionPart(materials[i], sections[i], offsetY[i], offsetZ[i]));
            }
            this.CheckCompositeSectionPartList(this.Parts);            
        }

        /// <summary>
        /// Create a composite section without offset. Use this constructor for column types.
        /// </summary>
        /// <param name="type">CompositeType enum member.</param>
        /// <param name="materials">The list of materials corresponding to each composite section part.</param>
        /// <param name="sections">The list of sections corresponding to each composite section part.</param>
        /// <param name="parameters">List of composite section parameters describing the composite section (e.g. name, geometry, etc.). Values of geometry parameters must be expressed in milimeters.</param>
        /// <exception cref="ArgumentException"></exception>
        public CompositeSection(CompositeType type, List<Materials.Material> materials, List<Sections.Section> sections, List<CompositeSectionParameter> parameters)
        {
            // check incoming data
            if (materials.Count != sections.Count)
            {
                throw new ArgumentException("Input variables of materials and sections must have the same length.");
            }

            this.EntityCreated();
            this.CompositeType = type;
            this.ParameterList = parameters;
            this.Parts = new List<CompositeSectionPart>();
            
            for (int i = 0; i < materials.Count; i++)
            {
                this.Parts.Add(new CompositeSectionPart(type, materials[i], sections[i]));
            }
            this.CheckCompositeSectionPartList(this.Parts);
        }

        public void CheckCompositeSectionPartList(List<CompositeSectionPart> parts)
        {
            // Check list length. Occurance number is defined in the struxml schema
            int listLength = parts.Count;
            if (listLength < 2)
            {
                throw new ArgumentException($"Composite section must have at least 2 section parts, but it has {listLength}.");
            }
            else if (listLength > 8)
            {
                throw new ArgumentException($"Composite section may have up to 8 section parts, but it has {listLength}.");
            }

            // Check materials
            List<Materials.Family> families = parts.Select(p => p.Material.Family).ToList();
            if ((!families.Contains(Materials.Family.Concrete)) || (!families.Contains(Materials.Family.Steel)))
                throw new ArgumentException("Composite section must contain at least one steel and one concrete section part.");
        }


        public static CompositeSection BeamA(List<Materials.Material> materials, List<Sections.Section> sections, List<double> offsetY, List<double> offsetZ, string name, double t, double bEff, double th, double bt, double bb, bool filled = false)
        {
            List<CompositeSectionParameter> parameters = new List<CompositeSectionParameter>();
            parameters.Add(new CompositeSectionParameter(CompositeParameterType.Name, name));
            parameters.Add(new CompositeSectionParameter(CompositeParameterType.t, t.ToString()));
            parameters.Add(new CompositeSectionParameter(CompositeParameterType.bEff, bEff.ToString()));
            parameters.Add(new CompositeSectionParameter(CompositeParameterType.th, th.ToString()));
            parameters.Add(new CompositeSectionParameter(CompositeParameterType.bt, bt.ToString()));
            parameters.Add(new CompositeSectionParameter(CompositeParameterType.bb, bb.ToString()));
            parameters.Add(new CompositeSectionParameter(CompositeParameterType.FillBeam, filled.ToString()));

            return new CompositeSection(CompositeType.BeamA, materials, sections, offsetY, offsetZ, parameters);
        }

        // + method description



        public static CompositeSection BeamB(List<Materials.Material> materials, List<double> offsetY, List<double> offsetZ, string name, double b, double bt, double o1, double o2, double h, double tw, double tfb, double tft)
        {
            List<CompositeSectionParameter> parameters = new List<CompositeSectionParameter>();
            parameters.Add(new CompositeSectionParameter(CompositeParameterType.Name, name));
            parameters.Add(new CompositeSectionParameter(CompositeParameterType.b, b.ToString()));
            parameters.Add(new CompositeSectionParameter(CompositeParameterType.bt, bt.ToString()));
            parameters.Add(new CompositeSectionParameter(CompositeParameterType.o1, o1.ToString()));
            parameters.Add(new CompositeSectionParameter(CompositeParameterType.o2, o2.ToString()));
            parameters.Add(new CompositeSectionParameter(CompositeParameterType.h, h.ToString()));
            parameters.Add(new CompositeSectionParameter(CompositeParameterType.tw, tw.ToString()));
            parameters.Add(new CompositeSectionParameter(CompositeParameterType.tfb, tfb.ToString()));
            parameters.Add(new CompositeSectionParameter(CompositeParameterType.tft, tft.ToString()));

            var compositeSection = new CompositeSection(CompositeType.BeamB, materials, sections, offsetY, offsetZ, parameters);

            return compositeSection;
        }

        internal Sections.Section CreateBeamBSection(double b, double bt, double o1, double o2, double h, double tw, double tfb, double tft)
        {
            Point3d pointA = new Point3d(-b / 2 + tw, -h / 2, 0);
            Point3d pointB = new Point3d(-pointA.X, pointA.Y, 0);
            Point3d pointC = new Point3d(-pointA.X, -pointA.Y, 0);
            Point3d pointD = new Point3d(pointA.X, -pointA.Y, 0);

            Point3d pointE = new Point3d(-bt / 2, h / 2, 0);
            Point3d pointF = new Point3d(-pointE.X, pointE.Y, 0);
            Point3d pointG = new Point3d(-pointE.X, pointE.Y + tft, 0);
            Point3d pointH = new Point3d(pointE.X, pointE.Y + tft, 0);


        }



        public static CompositeSection BeamP(List<Materials.Material> materials, List<Sections.Section> sections, List<double> offsetY, List<double> offsetZ, string name)
        {
            List<CompositeSectionParameter> parameters = new List<CompositeSectionParameter>();
            parameters.Add(new CompositeSectionParameter(CompositeParameterType.Name, name));

            return new CompositeSection(CompositeType.BeamP, materials, sections, offsetY, offsetZ, parameters);
        }
        public static CompositeSection ColumnA(List<Materials.Material> materials, List<Sections.Section> sections, string name, double cy, double cz)
        {
            List<CompositeSectionParameter> parameters = new List<CompositeSectionParameter>();
            parameters.Add(new CompositeSectionParameter(CompositeParameterType.Name, name));
            parameters.Add(new CompositeSectionParameter(CompositeParameterType.cy, cy.ToString()));
            parameters.Add(new CompositeSectionParameter(CompositeParameterType.cz, cz.ToString()));

            return new CompositeSection(CompositeType.ColumnA, materials, sections, parameters);
        }
        public static CompositeSection ColumnC(List<Materials.Material> materials, List<Sections.Section> sections, string name, double bc, double hc, double bf, double tw, double tf)
        {
            List<CompositeSectionParameter> parameters = new List<CompositeSectionParameter>();
            parameters.Add(new CompositeSectionParameter(CompositeParameterType.Name, name));
            parameters.Add(new CompositeSectionParameter(CompositeParameterType.bc, bc.ToString()));
            parameters.Add(new CompositeSectionParameter(CompositeParameterType.hc, hc.ToString()));
            parameters.Add(new CompositeSectionParameter(CompositeParameterType.bf, bf.ToString()));
            parameters.Add(new CompositeSectionParameter(CompositeParameterType.tw, tw.ToString()));
            parameters.Add(new CompositeSectionParameter(CompositeParameterType.tf, tf.ToString()));

            return new CompositeSection(CompositeType.ColumnC, materials, sections, parameters);
        }
        public static CompositeSection ColumnD(List<Materials.Material> materials, List<Sections.Section> sections, string name)
        {
            List<CompositeSectionParameter> parameters = new List<CompositeSectionParameter>();
            parameters.Add(new CompositeSectionParameter(CompositeParameterType.Name, name));

            return new CompositeSection(CompositeType.ColumnD, materials, sections, parameters);
        }
        public static CompositeSection ColumnE(List<Materials.Material> materials, List<Sections.Section> sections, string name, double d, double t)
        {
            List<CompositeSectionParameter> parameters = new List<CompositeSectionParameter>();
            parameters.Add(new CompositeSectionParameter(CompositeParameterType.Name, name));
            parameters.Add(new CompositeSectionParameter(CompositeParameterType.d, d.ToString()));
            parameters.Add(new CompositeSectionParameter(CompositeParameterType.t, t.ToString()));

            return new CompositeSection(CompositeType.ColumnE, materials, sections, parameters);
        }
        public static CompositeSection ColumnF(List<Materials.Material> materials, List<Sections.Section> sections, string name, double d, double t)
        {
            List<CompositeSectionParameter> parameters = new List<CompositeSectionParameter>();
            parameters.Add(new CompositeSectionParameter(CompositeParameterType.Name, name));
            parameters.Add(new CompositeSectionParameter(CompositeParameterType.d, d.ToString()));
            parameters.Add(new CompositeSectionParameter(CompositeParameterType.t, t.ToString()));

            return new CompositeSection(CompositeType.ColumnE, materials, sections, parameters);
        }
        public static CompositeSection ColumnG(List<Materials.Material> materials, List<Sections.Section> sections, string name, double d1, double d2, double t)
        {
            List<CompositeSectionParameter> parameters = new List<CompositeSectionParameter>();
            parameters.Add(new CompositeSectionParameter(CompositeParameterType.Name, name));
            parameters.Add(new CompositeSectionParameter(CompositeParameterType.d1, d1.ToString()));
            parameters.Add(new CompositeSectionParameter(CompositeParameterType.d2, d2.ToString()));
            parameters.Add(new CompositeSectionParameter(CompositeParameterType.t, t.ToString()));

            return new CompositeSection(CompositeType.ColumnG, materials, sections, parameters);
        }
        
        //public list<compositeparametertype> getcompositesectionparameters(compositesection compositesection)
        //{
        //    return compositesection.parameterlist.select(p => p.name).tolist();
            
        //}
    }
}
