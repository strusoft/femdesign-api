// https://strusoft.com/

using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Linq;

using FemDesign.Sections;
using FemDesign.Materials;
using FemDesign.Geometry;
using System.CodeDom;

namespace FemDesign.Composites
{
    [System.Serializable]
    public partial class CompositeSection : EntityBase
    {
        [XmlAttribute("type")]
        public CompositeType _type;

        [XmlIgnore]
        public CompositeType Type
        {
            get 
            { 
                return this._type; 
            }
            set 
            {
                this._type = value;
            }
        }
                
        [XmlElement("part", Order = 1)]
        public List<CompositeSectionPart> Parts { get; set; }

        [XmlElement("property", Order = 2)]
        public List<CompositeSectionParameter> ParameterList { get; set; }

        [XmlIgnore]
        internal Dictionary<CompositeParameterType, string> ParameterDictionary
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
        internal CompositeSection(CompositeType type, List<Material> materials, List<Section> sections, double[] offsetY, double[] offsetZ, List<CompositeSectionParameter> parameters)
        {
            // check incoming data
            if ((materials.Count != sections.Count) || (materials.Count != offsetY.Length) || (materials.Count != offsetZ.Length))
            {
                throw new ArgumentException("Input variables of materials, sections, offsetY and offsetZ must have the same length.");
            }

            this.EntityCreated();
            this.Type = type;
            this.ParameterList = parameters;
            this.Parts = new List<CompositeSectionPart>();
            
            for (int i = 0; i < materials.Count; i++)
            {
                this.Parts.Add(new CompositeSectionPart(materials[i], sections[i], offsetY[i], offsetZ[i]));
            }
            this.CheckCompositeSectionPartList(type, this.Parts);            
        }

        /// <summary>
        /// Create a composite section without offset. Use this constructor for column types.
        /// </summary>
        /// <param name="type">CompositeType enum member.</param>
        /// <param name="materials">The list of materials corresponding to each composite section part.</param>
        /// <param name="sections">The list of sections corresponding to each composite section part.</param>
        /// <param name="parameters">List of composite section parameters describing the composite section (e.g. name, geometry, etc.). Values of geometry parameters must be expressed in milimeters.</param>
        /// <exception cref="ArgumentException"></exception>
        internal CompositeSection(CompositeType type, List<Material> materials, List<Section> sections, List<CompositeSectionParameter> parameters)
        {
            // check incoming data
            if (IsOffsetNeeded(type))
            {
                throw new ArgumentException("Offset is required. For this type of composite section, the distance of concrete section part from the steel part must be defined.");
            }
            if (materials.Count != sections.Count)
            {
                throw new ArgumentException("Input variables of materials and sections must have the same length.");
            }

            this.EntityCreated();
            this.Type = type;
            this.ParameterList = parameters;
            this.Parts = new List<CompositeSectionPart>();
            
            for (int i = 0; i < materials.Count; i++)
            {
                this.Parts.Add(new CompositeSectionPart(materials[i], sections[i]));
            }
            this.CheckCompositeSectionPartList(type, this.Parts);
        }

        /// <summary>
        /// Not finished yet!
        /// </summary>
        /// <param name="steel">Steel part material.</param>
        /// <param name="concrete">Concrete part material.</param>
        /// <param name="steelSection">Steel section from database. Can be a KKR, VKR or I-shaped section type.</param>
        /// <param name="name">Composite section name.</param>
        /// <param name="t">Slab thickness.</param>
        /// <param name="bEff">Concrete slab effective width.</param>
        /// <param name="th">Hunch thickness.</param>
        /// <param name="bt">Hunch width at the top.</param>
        /// <param name="bb">Hunch width at the bottom.</param>
        /// <param name="filled">True if steel section is filled with concrete, false if not.</param>
        /// <returns></returns>
        internal static CompositeSection BeamA(Material steel, Material concrete, Section steelSection, string name, double t, double bEff, double th, double bt, double bb, bool filled = false)
        {
            NotImplemented();

            // check incoming data
            CheckSteelSectionCompatibility(CompositeType.ColumnE, steelSection);

            List<Material> materials = new List<Material>() { steel, concrete };     // !the sequence of steel and concrete materials must match the sequence of steel and concrete sections
            List<Section> sections = new List<Section>();
            //List<Section> sections = CreateSectionBeamA(steelSection, t, bEff, th, bt, bb, filled);     // !CreateSectionBeamA() method is not impemented yet
            //(double[], double[]) offsetYZ = CalculateOffsetBeamA(steelSection, t, bEff, th, bt, bb, filled);    // !CalculateOffsetBeamA() method is not impemented yet

            List<CompositeSectionParameter> parameters = new List<CompositeSectionParameter>();
            parameters.Add(new CompositeSectionParameter(CompositeParameterType.Name, name));
            parameters.Add(new CompositeSectionParameter(CompositeParameterType.t, t.ToString()));
            parameters.Add(new CompositeSectionParameter(CompositeParameterType.bEff, bEff.ToString()));
            parameters.Add(new CompositeSectionParameter(CompositeParameterType.th, th.ToString()));
            parameters.Add(new CompositeSectionParameter(CompositeParameterType.bt, bt.ToString()));
            parameters.Add(new CompositeSectionParameter(CompositeParameterType.bb, bb.ToString()));
            parameters.Add(new CompositeSectionParameter(CompositeParameterType.FillBeam, filled.ToString()));

            return new CompositeSection(CompositeType.BeamA, materials, sections,/* offsetYZ.Item1, offsetYZ.Item2,*/ parameters);
        }

        /// <summary>
        /// Create a BeamB type CompositeSection object.
        /// </summary>
        /// <param name="steel">Steel part material.</param>
        /// <param name="concrete">Concrete part material.</param>
        /// <param name="name">Composite section name.</param>
        /// <param name="b">Intermediate width of the bottom flange.</param>
        /// <param name="bt">Top flange width.</param>
        /// <param name="o1">Left overhang.</param>
        /// <param name="o2">Right overhang.</param>
        /// <param name="h">Web hight.</param>
        /// <param name="tw">Web thickness.</param>
        /// <param name="tfb">Bottom flange thickness.</param>
        /// <param name="tft">Top flange thickness.</param>
        /// <returns></returns>
        public static CompositeSection BeamB(Material steel, Material concrete, string name, double b, double bt, double o1, double o2, double h, double tw, double tfb, double tft)
        {
            List<Material> materials = new List<Material>() { steel, concrete };     // !the sequence of steel and concrete materials must match the sequence of steel and concrete sections
            List<Section> sections = CreateSectionBeamB(b, bt, o1, o2, h, tw, tfb, tft);
            (double[], double[]) offsetYZ = CalculateOffsetBeamB(b, bt, o1, o2, h, tw, tfb, tft);

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

            return new CompositeSection(CompositeType.BeamB, materials, sections, offsetYZ.Item1, offsetYZ.Item2, parameters);
        }

        /// <summary>
        /// Create a parametric section for BeamB type CompositeSection object.
        /// </summary>
        /// <param name="b">Intermediate width of the bottom flange.</param>
        /// <param name="bt">Top flange width.</param>
        /// <param name="o1">Left overhang.</param>
        /// <param name="o2">Right overhang.</param>
        /// <param name="h">Web hight.</param>
        /// <param name="tw">Web thickness.</param>
        /// <param name="tfb">Bottom flange thickness.</param>
        /// <param name="tft">Top flange thickness.</param>
        /// <returns></returns>
        internal static List<Sections.Section> CreateSectionBeamB(double b, double bt, double o1, double o2, double h, double tw, double tfb, double tft)
        {
            // conversion of geometric parameters from millimeters to meters
            b = b / 1000;
            bt = bt / 1000;
            o1 = o1 / 1000;
            o2 = o2 / 1000;
            h = h / 1000;
            tw = tw / 1000;
            tfb = tfb / 1000;
            tft = tft / 1000;

            // definition of corner points
            List<Point3d> points = new List<Point3d>();
            points.Add(new Point3d(-b / 2 + tw, -h / 2, 0));
            points.Add(new Point3d(-points[0].X, points[0].Y, 0));
            points.Add(new Point3d(-points[0].X, -points[0].Y, 0));
            points.Add(new Point3d(points[0].X, -points[0].Y, 0));
            var intPoints = points.Take(4).ToList();
                        
            points.Add(new Point3d(-b / 2, h / 2, 0));
            points.Add(new Point3d(points[4].X, -points[4].Y, 0));
            points.Add(new Point3d(points[5].X - o1, points[5].Y, 0));
            points.Add(new Point3d(points[6].X, points[6].Y - tfb, 0));
            points.Add(new Point3d(b / 2 + o2, points[7].Y, 0));
            points.Add(new Point3d(points[8].X, points[5].Y, 0));
            points.Add(new Point3d(-points[5].X, points[5].Y, 0));
            points.Add(new Point3d(-points[4].X, points[4].Y, 0));
            points.Add(new Point3d(bt / 2, points[4].Y, 0));
            points.Add(new Point3d(points[12].X, points[12].Y + tft, 0));
            points.Add(new Point3d(-points[12].X, points[13].Y, 0));
            points.Add(new Point3d(-points[12].X, points[12].Y, 0));
            var extPoints = points.Skip(4).Take(points.Count - 4).ToList();                 

            // create contours
            var intContour = new Contour(intPoints);
            var extContour = new Contour(extPoints);

            // create regions
            var steelRegion = new Region(new List<Contour> { extContour, intContour });
            var steelRegionGroup = new RegionGroup(steelRegion);
            var concreteRegion = new Region(new List<Contour> { intContour });
            var concreteRegionGroup = new RegionGroup(concreteRegion);

            // create sections
            var concreteSection = new Section(concreteRegionGroup, "custom", MaterialTypeEnum.Concrete, "Concrete section", "Rectangle", $"{(b-2*tw)*1000}x{h*1000}");
            var steelSection = new Section(steelRegionGroup, "custom", MaterialTypeEnum.SteelWelded, "Steel section", "Welded", " ");
            List<Section> sections = new List<Section>() { steelSection, concreteSection };   // !the sequence of steel and concrete sections must match the sequence of steel and concrete materials

            return sections;
        }

        /// <summary>
        /// Calculates the offset of the steel part from the concrete part in Y and Z.
        /// </summary>
        /// <param name="b">Intermediate width of the bottom flange.</param>
        /// <param name="bt">Top flange width.</param>
        /// <param name="o1">Left overhang.</param>
        /// <param name="o2">Right overhang.</param>
        /// <param name="h">Web hight.</param>
        /// <param name="tw">Web thickness.</param>
        /// <param name="tfb">Bottom flange thickness.</param>
        /// <param name="tft">Top flange thickness.</param>
        /// <returns></returns>
        internal static (double[], double[]) CalculateOffsetBeamB(double b, double bt, double o1, double o2, double h, double tw, double tfb, double tft)
        {
            // conversion of geometric parameters from millimeters to meters
            b = b / 1000;
            bt = bt / 1000;
            o1 = o1 / 1000;
            o2 = o2 / 1000;
            h = h / 1000;
            tw = tw / 1000;
            tfb = tfb / 1000;
            tft = tft / 1000;

            var steelArea = (bt * tft) + (2 * tw * h) + ((b + o1 + o2) * tfb);
            var ezSteel = (((bt * tft) * (tft + h)) - (((b + o1 + o2) * tfb) * (tfb + h))) / 2 / steelArea;
            var eySteel = (((b + o1 + o2) * tfb) * (o2 - o1)) / 2 / steelArea;

            double[] offsetY = { eySteel, 0 };   // !the sequence of steel and concrete offsets must match the sequence of steel and concrete sections
            double[] offsetZ = { ezSteel, 0 };

            return (offsetY, offsetZ);
        }

        /// <summary>
        /// Not finished yet!
        /// </summary>
        /// <param name="steel">Steel part material.</param>
        /// <param name="concrete">Concrete part material.</param>
        /// <param name="steelSection">Delta beam cross-section from database. Can be a D or DR section type.</param>
        /// <param name="name">Composite section name.</param>
        /// <returns></returns>
        internal static CompositeSection BeamP(Material steel, Material concrete, Section steelSection, string name)
        {
            NotImplemented();

            // check incoming data
            CheckSteelSectionCompatibility(CompositeType.ColumnE, steelSection);

            List<Material> materials = new List<Material>() { steel, concrete };     // !the sequence of steel and concrete materials must match the sequence of steel and concrete 
            List<Section> sections = new List<Section>();
            //List<Section> sections = CreateSectionBeamP(steelSection);      // !CreateSectionBeamP() method is not impemented yet
            //(double[], double[]) offsetYZ = CalculateOffsetBeamP(steelSection);     // !CalculateOffsetBeamP() method is not impemented yet

            List<CompositeSectionParameter> parameters = new List<CompositeSectionParameter>();
            parameters.Add(new CompositeSectionParameter(CompositeParameterType.Name, name));

            return new CompositeSection(CompositeType.BeamP, materials, sections, /*offsetYZ.Item1, offsetYZ.Item2,*/ parameters);
        }

        /// <summary>
        /// Not finished yet!
        /// </summary>
        /// <param name="steel">Steel part material.</param>
        /// <param name="concrete">Concrete part material.</param>
        /// <param name="steelSection">Steel section from database. Must be an I-shaped section type.</param>
        /// <param name="name">Composite section name.</param>
        /// <param name="cy">Concrete cover in Y direction.</param>
        /// <param name="cz">Concrete cover in Z direction.</param>
        /// <returns></returns>
        internal static CompositeSection ColumnA(Material steel, Material concrete, Section steelSection, string name, double cy, double cz)
        {
            NotImplemented();

            // check incoming data
            CheckSteelSectionCompatibility(CompositeType.ColumnE, steelSection);

            List<Material> materials = new List<Material>() { steel, concrete };     // !the sequence of steel and concrete materials must match the sequence of steel and concrete sections
            List<Section> sections = new List<Section>();
            //List<Section> sections = CreateSectionColumnA(steelSection, double cy, double cz);      // !CreateSectionColumnA() method is not impemented yet

            List<CompositeSectionParameter> parameters = new List<CompositeSectionParameter>();
            parameters.Add(new CompositeSectionParameter(CompositeParameterType.Name, name));
            parameters.Add(new CompositeSectionParameter(CompositeParameterType.cy, cy.ToString()));
            parameters.Add(new CompositeSectionParameter(CompositeParameterType.cz, cz.ToString()));

            return new CompositeSection(CompositeType.ColumnA, materials, sections, parameters);
        }

        /// <summary>
        /// Not finished yet!
        /// </summary>
        /// <param name="steel">Steel part material.</param>
        /// <param name="concrete">Concrete part material.</param>
        /// <param name="name">Composite section name.</param>
        /// <param name="bc">Cross-section width.</param>
        /// <param name="hc">Cross-section height.</param>
        /// <param name="bf">Flange width.</param>
        /// <param name="tw">Web thickness.</param>
        /// <param name="tf">Flange thickness.</param>
        /// <returns></returns>
        internal static CompositeSection ColumnC(Material steel, Material concrete, string name, double bc, double hc, double bf, double tw, double tf)
        {
            NotImplemented();

            List<Material> materials = new List<Material>() { steel, concrete };     // !the sequence of steel and concrete materials must match the sequence of steel and concrete sections
            List<Section> sections = new List<Section>();
            //List<Section> sections = CreateSectionColumnC(bc, hc, bf, tw, tf);      // !CreateSectionColumnA() method is not impemented yet

            List<CompositeSectionParameter> parameters = new List<CompositeSectionParameter>();
            parameters.Add(new CompositeSectionParameter(CompositeParameterType.Name, name));
            parameters.Add(new CompositeSectionParameter(CompositeParameterType.bc, bc.ToString()));
            parameters.Add(new CompositeSectionParameter(CompositeParameterType.hc, hc.ToString()));
            parameters.Add(new CompositeSectionParameter(CompositeParameterType.bf, bf.ToString()));
            parameters.Add(new CompositeSectionParameter(CompositeParameterType.tw, tw.ToString()));
            parameters.Add(new CompositeSectionParameter(CompositeParameterType.tf, tf.ToString()));

            return new CompositeSection(CompositeType.ColumnC, materials, sections, parameters);
        }

        /// <summary>
        /// Not finished yet!
        /// </summary>
        /// <param name="steel">Steel part material.</param>
        /// <param name="concrete">Concrete part material.</param>
        /// <param name="steelSection">Steel section from database. Can be a KKR or VKR section type.</param>
        /// <param name="name">Composite section name.</param>
        /// <returns></returns>
        internal static CompositeSection ColumnD(Material steel, Material concrete, Section steelSection, string name)
        {
            NotImplemented();

            // check incoming data
            CheckSteelSectionCompatibility(CompositeType.ColumnE, steelSection);

            List<Material> materials = new List<Material>() { steel, concrete };     // !the sequence of steel and concrete materials must match the sequence of steel and concrete 
            List<Section> sections = new List<Section>();
            //List<Section> sections = CreateSectionColumnD(steelSection);      // !CreateSectionColumnD() method is not impemented yet. Only KKR or VKR type steel sections can be used.

            List<CompositeSectionParameter> parameters = new List<CompositeSectionParameter>();
            parameters.Add(new CompositeSectionParameter(CompositeParameterType.Name, name));

            return new CompositeSection(CompositeType.ColumnD, materials, sections, parameters);
        }

        /// <summary>
        /// Not finished yet!
        /// </summary>
        /// <param name="steel">Steel part material.</param>
        /// <param name="concrete">Concrete part material.</param>
        /// <param name="name">Composite section name.</param>
        /// <param name="d">Exterior diameter of the steel tube.</param>
        /// <param name="t">Thickness of the steel tube.</param>
        /// <returns></returns>
        internal static CompositeSection ColumnE(Material steel, Material concrete, string name, double d, double t)
        {
            NotImplemented();

            List<Material> materials = new List<Material>() { steel, concrete };     // !the sequence of steel and concrete materials must match the sequence of steel and concrete 
            List<Section> sections = new List<Section>();
            //List<Section> sections = CreateSectionColumnE(d, t);      // !CreateSectionColumnE() method is not impemented yet

            List<CompositeSectionParameter> parameters = new List<CompositeSectionParameter>();
            parameters.Add(new CompositeSectionParameter(CompositeParameterType.Name, name));
            parameters.Add(new CompositeSectionParameter(CompositeParameterType.d, d.ToString()));
            parameters.Add(new CompositeSectionParameter(CompositeParameterType.t, t.ToString()));

            return new CompositeSection(CompositeType.ColumnE, materials, sections, parameters);
        }

        /// <summary>
        /// Not implemented yet!
        /// </summary>
        /// <param name="steelTube">Steel tube's material.</param>
        /// <param name="steelI">I-shaped section's material.</param>
        /// <param name="concrete">Concrete part material.</param>
        /// <param name="steelSectionI">Steel section from database. Must be an I-shaped section type.</param>
        /// <param name="name">Composite section name.</param>
        /// <param name="d">Steel tube exterior diameter.</param>
        /// <param name="t">Steel tube thickness.</param>
        /// <returns></returns>
        internal static CompositeSection ColumnF(Material steelTube, Material steelI, Material concrete, Section steelSectionI, string name, double d, double t)
        {
            NotImplemented();

            // check incoming data
            CheckSteelSectionCompatibility(CompositeType.ColumnE, steelSectionI);

            List<Material> materials = new List<Material>() { steelTube, steelI, concrete };     // !the sequence of steel and concrete materials must match the sequence of steel and concrete 
            List<Section> sections = new List<Section>();
            //List<Section> sections = CreateSectionColumnF(steelSection, d, t);      // !CreateSectionColumnF() method is not impemented yet

            List<CompositeSectionParameter> parameters = new List<CompositeSectionParameter>();
            parameters.Add(new CompositeSectionParameter(CompositeParameterType.Name, name));
            parameters.Add(new CompositeSectionParameter(CompositeParameterType.d, d.ToString()));
            parameters.Add(new CompositeSectionParameter(CompositeParameterType.t, t.ToString()));

            return new CompositeSection(CompositeType.ColumnE, materials, sections, parameters);
        }

        /// <summary>
        /// Not implemented yet!
        /// </summary>
        /// <param name="steelTube">Steel tube material.</param>
        /// <param name="steelCore">Steel core material.</param>
        /// <param name="concrete">Concrete part material.</param>
        /// <param name="name">Composite section name.</param>
        /// <param name="d1">Steel tube exterior diameter.</param>
        /// <param name="d2">Steel core diameter.</param>
        /// <param name="t">Steel tube thickness.</param>
        /// <returns></returns>
        internal static CompositeSection ColumnG(Material steelTube, Material steelCore, Material concrete, string name, double d1, double d2, double t)
        {
            NotImplemented();

            List<Material> materials = new List<Material>() { steelTube, steelCore, concrete };     // !the sequence of steel and concrete materials must match the sequence of steel and concrete 
            List<Section> sections = new List<Section>();
            //List<Section> sections = CreateSectionColumnG(d1, d2, t);      // !CreateSectionColumnG() method is not impemented yet

            List<CompositeSectionParameter> parameters = new List<CompositeSectionParameter>();
            parameters.Add(new CompositeSectionParameter(CompositeParameterType.Name, name));
            parameters.Add(new CompositeSectionParameter(CompositeParameterType.d1, d1.ToString()));
            parameters.Add(new CompositeSectionParameter(CompositeParameterType.d2, d2.ToString()));
            parameters.Add(new CompositeSectionParameter(CompositeParameterType.t, t.ToString()));

            return new CompositeSection(CompositeType.ColumnG, materials, sections, parameters);
        }      

        internal static void NotImplemented()
        {
            throw new ArgumentException($"This composite section type is not implemented yet. If needed please contact us. Implemented composite section types: {CompositeType.BeamA}");
        }

        internal bool IsOffsetNeeded(CompositeType type)
        {
            switch (type)
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

        internal void CheckCompositeSectionPartList(CompositeType type, List<CompositeSectionPart> parts)
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
            List<Materials.Family> materialTypes = parts.Select(p => p.Material.Family).ToList();
            int steelNum = materialTypes.Select(m => m == Materials.Family.Steel).Count();

            if ((!materialTypes.Contains(Materials.Family.Concrete)) || (!materialTypes.Contains(Materials.Family.Steel)))
                throw new ArgumentException("Check the material types! Composite section must contain at least one steel and one concrete section part.");
            if(type == CompositeType.ColumnF || type == CompositeType.ColumnG)
            {
                if (steelNum != 2)
                    throw new ArgumentException($"{type} must have 2 steel composite section parts, but it has {steelNum}.");
            }
        }

        public static List<Sections.Family> GetCompatibleSteelSectionType(CompositeType type)
        {
            switch (type)
            {
                case CompositeType.BeamA:
                    return new List<Sections.Family>() 
                    { 
                        Sections.Family.HE_A, 
                        Sections.Family.HE_B, 
                        Sections.Family.HE_M, 
                        Sections.Family.I, 
                        Sections.Family.IPE, 
                        Sections.Family.UKB, 
                        Sections.Family.UKC,
                        Sections.Family.KKR,
                        Sections.Family.VKR 
                    };
                case CompositeType.BeamP:
                    return new List<Sections.Family>()
                    {
                        Sections.Family.D,
                        Sections.Family.DR
                    };
                case CompositeType.ColumnA:
                    return new List<Sections.Family>()
                    {
                        Sections.Family.HE_A,
                        Sections.Family.HE_B,
                        Sections.Family.HE_M,
                        Sections.Family.I,
                        Sections.Family.IPE,
                        Sections.Family.UKB,
                        Sections.Family.UKC
                    };
                case CompositeType.ColumnD:
                    return new List<Sections.Family>()
                    {
                        Sections.Family.KKR,
                        Sections.Family.VKR
                    };
                case CompositeType.ColumnF:
                    return new List<Sections.Family>()
                    {
                        Sections.Family.HE_A,
                        Sections.Family.HE_B,
                        Sections.Family.HE_M,
                        Sections.Family.I,
                        Sections.Family.IPE,
                        Sections.Family.UKB,
                        Sections.Family.UKC
                    };
                default:
                    throw new ArgumentException("For this CompositeType you cannot specify a steel section from database.");
            }
        }
                
        public static void CheckSteelSectionCompatibility(CompositeType type, Section steelSection)
        {
            // check section material
            if (steelSection.MaterialFamily != "Steel")
                throw new ArgumentException("Section group name must be Steel!");                    

            // check steel section TypeName
            string typeName = steelSection.TypeName;
            List<string> compatibleSectionTypes = GetCompatibleSteelSectionType(type).Select(f => f.ToString()).ToList();

            if (!compatibleSectionTypes.Contains(typeName, StringComparer.OrdinalIgnoreCase))
                throw new ArgumentException($"Invalid steel section type. Compatible section type for {type}: {string.Join(" ", compatibleSectionTypes)}.");
        }
        
    }
}
