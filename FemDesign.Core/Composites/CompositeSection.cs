// https://strusoft.com/

using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Linq;

using FemDesign.Sections;
using FemDesign.Materials;
using FemDesign.Geometry;

namespace FemDesign.Composites
{
    [System.Serializable]
    public partial class CompositeSection : EntityBase
    {
        [XmlAttribute("type")]
        public CompositeSectionType _type;

        [XmlIgnore]
        public CompositeSectionType Type
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

        [XmlIgnore]
        public List<Material> Materials
        {
            get
            {
                List<Material> list = new List<Material>();
                foreach (var part in this.Parts)
                {
                    list.Add(part.Material);
                }

                return list;
            }
        }

        [XmlIgnore]
        public List<Section> Sections
        {
            get
            {
                List<Section> list = new List<Section>();
                foreach (var part in this.Parts)
                {
                    list.Add(part.Section);
                }

                return list;
            }
        }

        [XmlElement("property", Order = 2)]
        public List<CompositeSectionParameter> ParameterList { get; set; }

        [XmlIgnore]
        public Dictionary<CompositeSectionParameterType, string> ParameterDictionary
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
        internal CompositeSection(CompositeSectionType type, List<Material> materials, List<Section> sections, double[] offsetY, double[] offsetZ, List<CompositeSectionParameter> parameters)
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
        internal CompositeSection(CompositeSectionType type, List<Material> materials, List<Section> sections, List<CompositeSectionParameter> parameters)
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
        /// Not implemented yet! WIP!
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
        private static CompositeSection IProfileWithEffectiveConcreteSlab(string name, Material steel, Material concrete, Section steelSection, double t, double bEff, double th, double bt, double bb, bool filled = false)
        {
            NotImplemented();
            CheckMaterialFamily(new List<Material> { steel }, concrete);

            // check input data
            CheckSteelSectionCompatibility(CompositeSectionType.FilledSteelTube, steelSection);

            List<Material> materials = new List<Material>() { steel, concrete };     // !the sequence of steel and concrete materials must match the sequence of steel and concrete sections
            List<Section> sections = new List<Section>();
            //List<Section> sections = CreateSectionBeamA(steelSection, t, bEff, th, bt, bb, filled);     // !CreateSectionBeamA() method is not impemented yet
            //(double[], double[]) offsetYZ = CalculateOffsetBeamA(steelSection, t, bEff, th, bt, bb, filled);    // !CalculateOffsetBeamA() method is not impemented yet

            List<CompositeSectionParameter> parameters = new List<CompositeSectionParameter>
            {
                new CompositeSectionParameter(CompositeSectionParameterType.Name, name),
                new CompositeSectionParameter(CompositeSectionParameterType.t, t.ToString()),
                new CompositeSectionParameter(CompositeSectionParameterType.bEff, bEff.ToString()),
                new CompositeSectionParameter(CompositeSectionParameterType.th, th.ToString()),
                new CompositeSectionParameter(CompositeSectionParameterType.bt, bt.ToString()),
                new CompositeSectionParameter(CompositeSectionParameterType.bb, bb.ToString()),
                new CompositeSectionParameter(CompositeSectionParameterType.FillBeam, filled.ToString())
            };

            return new CompositeSection(CompositeSectionType.IProfileWithEffectiveConcreteSlab, materials, sections,/* offsetYZ.Item1, offsetYZ.Item2,*/ parameters);
        }

        /// <summary>
        /// Create a FilledHSQProfile type CompositeSection object.
        /// </summary>
        /// <param name="steel">Steel part material.</param>
        /// <param name="concrete">Concrete part material.</param>
        /// <param name="name">Composite section name.</param>
        /// <param name="b">Intermediate width of the bottom flange [mm].</param>
        /// <param name="bt">Top flange width [mm].</param>
        /// <param name="o1">Left overhang [mm].</param>
        /// <param name="o2">Right overhang [mm].</param>
        /// <param name="h">Web hight [mm].</param>
        /// <param name="tw">Web thickness [mm].</param>
        /// <param name="tfb">Bottom flange thickness [mm].</param>
        /// <param name="tft">Top flange thickness [mm].</param>
        /// <returns></returns>
        public static CompositeSection FilledHSQProfile(string name, Material steel, Material concrete, double b, double bt, double o1, double o2, double h, double tw, double tfb, double tft)
        {
            CheckMaterialFamily(new List<Material> { steel }, concrete);

            List<Material> materials = new List<Material>() { steel, concrete };     // !the sequence of steel and concrete materials must match the sequence of steel and concrete sections
            List<Section> sections = CreateFilledHSQSection(b, bt, o1, o2, h, tw, tfb, tft);
            (double[], double[]) offsetYZ = CalculateFilledHSQSectionOffset(b, bt, o1, o2, h, tw, tfb, tft);

            List<CompositeSectionParameter> parameters = new List<CompositeSectionParameter>
            {
                new CompositeSectionParameter(CompositeSectionParameterType.Name, name),
                new CompositeSectionParameter(CompositeSectionParameterType.b, b.ToString()),
                new CompositeSectionParameter(CompositeSectionParameterType.bt, bt.ToString()),
                new CompositeSectionParameter(CompositeSectionParameterType.o1, o1.ToString()),
                new CompositeSectionParameter(CompositeSectionParameterType.o2, o2.ToString()),
                new CompositeSectionParameter(CompositeSectionParameterType.h, h.ToString()),
                new CompositeSectionParameter(CompositeSectionParameterType.tw, tw.ToString()),
                new CompositeSectionParameter(CompositeSectionParameterType.tfb, tfb.ToString()),
                new CompositeSectionParameter(CompositeSectionParameterType.tft, tft.ToString())
            };

            return new CompositeSection(CompositeSectionType.FilledHSQProfile, materials, sections, offsetYZ.Item1, offsetYZ.Item2, parameters);
        }

        /// <summary>
        /// Create a parametric section for FilledHSQProfile type CompositeSection object.
        /// </summary>
        /// <param name="b">Intermediate width of the bottom flange [mm].</param>
        /// <param name="bt">Top flange width [mm].</param>
        /// <param name="o1">Left overhang [mm].</param>
        /// <param name="o2">Right overhang [mm].</param>
        /// <param name="h">Web hight [mm].</param>
        /// <param name="tw">Web thickness [mm].</param>
        /// <param name="tfb">Bottom flange thickness [mm].</param>
        /// <param name="tft">Top flange thickness [mm].</param>
        /// <returns></returns>
        internal static List<Sections.Section> CreateFilledHSQSection(double b, double bt, double o1, double o2, double h, double tw, double tfb, double tft)
        {
            // round inputs
            b = Math.Round(b, 2, MidpointRounding.AwayFromZero);
            bt = Math.Round(bt, 2, MidpointRounding.AwayFromZero);
            o1 = Math.Round(o1, 2, MidpointRounding.AwayFromZero);
            o2 = Math.Round(o2, 2, MidpointRounding.AwayFromZero);
            h = Math.Round(h, 2, MidpointRounding.AwayFromZero);
            tw = Math.Round(tw, 2, MidpointRounding.AwayFromZero);
            tfb = Math.Round(tfb, 2, MidpointRounding.AwayFromZero);
            tft = Math.Round(tft, 2, MidpointRounding.AwayFromZero);

            // check inputs
            if (b <= 0 || bt <= 0 || o1 <= 0 || o2 <= 0 || h <= 0 || tw <= 0 || tfb <= 0 || tft <= 0)
                throw new ArgumentException(" Composite section parameters must be positive, non-zero numbers!");
            if (bt < b)
                throw new ArgumentException(" Top flange width must be greater than or equal to the bottom flange intermediate width!");
            if (tw >= b / 2)
                throw new ArgumentException($" Web thickness must be smaller than b/2 = {b / 2}!");

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
            if (b / 2 != bt / 2)
            {
                points.Add(new Point3d(bt / 2, points[4].Y, 0));
            }
            points.Add(new Point3d(points[points.Count - 1].X, points[4].Y + tft, 0));
            points.Add(new Point3d(-points[points.Count - 1].X, points[points.Count - 1].Y, 0));
            if (b / 2 != bt / 2)
            {
                points.Add(new Point3d(points[points.Count - 1].X, points[4].Y, 0));
            }
            var extPoints = points.Skip(4).Take(points.Count - 4).ToList();

            // create contours
            var intContour = new Contour(intPoints);
            var extContour = new Contour(extPoints);
            List<Contour> contours = new List<Contour> { extContour, intContour };

            // define section data
            List<MaterialTypeEnum> materialTypes = new List<MaterialTypeEnum> { MaterialTypeEnum.SteelWelded, MaterialTypeEnum.Concrete };
            List<string> groupNames = new List<string> { "Steel section", "Concrete section" };
            List<string> typeNames = materialTypes.Select(x => x.ToString()).ToList();

            string steelSizeName = $"{bt * 1000}x{tft * 1000}-{h * 1000}x{tw * 1000}-{(b + o1 + o2) * 1000}x{tfb * 1000}";
            string concreteSizeName = $"{(b - 2 * tw) * 1000}x{h * 1000}";
            List<string> sizeNames = new List<string> { steelSizeName, concreteSizeName };

            
            return SectionsFromContours(contours, materialTypes, groupNames, typeNames, sizeNames);
        }

        /// <summary>
        /// Calculates the offset of the steel part from the concrete part in Y and Z for a FilledHSQProfile.
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
        internal static (double[], double[]) CalculateFilledHSQSectionOffset(double b, double bt, double o1, double o2, double h, double tw, double tfb, double tft)
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
        /// Not implemented yet! WIP!
        /// </summary>
        /// <param name="steel">Steel part material.</param>
        /// <param name="concrete">Concrete part material.</param>
        /// <param name="deltaBeamProfile">Delta beam cross-section from database. Can be a D or DR section type.</param>
        /// <param name="name">Composite section name.</param>
        /// <returns></returns>
        private static CompositeSection FilledDeltaBeamProfile(string name, Material steel, Material concrete, Section deltaBeamProfile)
        {
            NotImplemented();
            CheckMaterialFamily(new List<Material> { steel }, concrete);

            // check input data
            CheckSteelSectionCompatibility(CompositeSectionType.FilledSteelTube, deltaBeamProfile);

            List<Material> materials = new List<Material>() { steel, concrete };     // !the sequence of steel and concrete materials must match the sequence of steel and concrete sections
            List<Section> sections = new List<Section>();
            //List<Section> sections = CreateSectionBeamP(steelSection);      // !CreateSectionBeamP() method is not impemented yet
            //(double[], double[]) offsetYZ = CalculateOffsetBeamP(steelSection);     // !CalculateOffsetBeamP() method is not impemented yet

            List<CompositeSectionParameter> parameters = new List<CompositeSectionParameter>
            {
                new CompositeSectionParameter(CompositeSectionParameterType.Name, name)
            };

            return new CompositeSection(CompositeSectionType.FilledDeltaBeamProfile, materials, sections, /*offsetYZ.Item1, offsetYZ.Item2,*/ parameters);
        }

        /// <summary>
        /// Not implemented yet! WIP!
        /// </summary>
        /// <param name="steel">Steel part material.</param>
        /// <param name="concrete">Concrete part material.</param>
        /// <param name="steelIProfile">Steel section from database. Must be an I-shaped section type.</param>
        /// <param name="name">Composite section name.</param>
        /// <param name="cy">Concrete cover in Y direction.</param>
        /// <param name="cz">Concrete cover in Z direction.</param>
        /// <returns></returns>
        private static CompositeSection FilledIProfile(string name, Material steel, Material concrete, Section steelIProfile, double cy, double cz)
        {
            NotImplemented();
            CheckMaterialFamily(new List<Material> { steel }, concrete);

            // check input data
            CheckSteelSectionCompatibility(CompositeSectionType.FilledSteelTube, steelIProfile);

            List<Material> materials = new List<Material>() { steel, concrete };     // !the sequence of steel and concrete materials must match the sequence of steel and concrete sections
            List<Section> sections = new List<Section>();
            //List<Section> sections = CreateSectionColumnA(steelIProfile, double cy, double cz);      // !CreateSectionColumnA() method is not impemented yet

            List<CompositeSectionParameter> parameters = new List<CompositeSectionParameter>
            {
                new CompositeSectionParameter(CompositeSectionParameterType.Name, name),
                new CompositeSectionParameter(CompositeSectionParameterType.cy, cy.ToString()),
                new CompositeSectionParameter(CompositeSectionParameterType.cz, cz.ToString())
            };

            return new CompositeSection(CompositeSectionType.FilledIProfile, materials, sections, parameters);
        }

        /// <summary>
        /// Not implemented yet! WIP!
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
        private static CompositeSection FilledCruciformProfile(string name, Material steel, Material concrete, double bc, double hc, double bf, double tw, double tf)
        {
            NotImplemented();
            CheckMaterialFamily(new List<Material> { steel }, concrete);

            List<Material> materials = new List<Material>() { steel, concrete };     // !the sequence of steel and concrete materials must match the sequence of steel and concrete sections
            List<Section> sections = new List<Section>();
            //List<Section> sections = CreateSectionColumnC(bc, hc, bf, tw, tf);      // !CreateSectionColumnA() method is not impemented yet

            List<CompositeSectionParameter> parameters = new List<CompositeSectionParameter>
            {
                new CompositeSectionParameter(CompositeSectionParameterType.Name, name),
                new CompositeSectionParameter(CompositeSectionParameterType.bc, bc.ToString()),
                new CompositeSectionParameter(CompositeSectionParameterType.hc, hc.ToString()),
                new CompositeSectionParameter(CompositeSectionParameterType.bf, bf.ToString()),
                new CompositeSectionParameter(CompositeSectionParameterType.tw, tw.ToString()),
                new CompositeSectionParameter(CompositeSectionParameterType.tf, tf.ToString())
            };

            return new CompositeSection(CompositeSectionType.FilledCruciformProfile, materials, sections, parameters);
        }

        /// <summary>
        /// Create a FilledRHSProfile type CompositeSection object.
        /// </summary>
        /// <param name="steel">Steel part material.</param>
        /// <param name="concrete">Concrete part material.</param>
        /// <param name="steelRHSProfile">Steel section from database. Can be a KKR or VKR section type.</param>
        /// <param name="name">Composite section name.</param>
        /// <returns></returns>
        public static CompositeSection FilledRHSProfile(string name, Material steel, Material concrete, Section steelRHSProfile)
        {
            CheckMaterialFamily(new List<Material> { steel }, concrete);

            // check input data
            CheckSteelSectionCompatibility(CompositeSectionType.FilledSteelTube, steelRHSProfile);

            List<Material> materials = new List<Material>() { steel, concrete };     // !the sequence of steel and concrete materials must match the sequence of steel and concrete sections
            List<Section> sections = CreateFilledRHSSection(steelRHSProfile);

            List<CompositeSectionParameter> parameters = new List<CompositeSectionParameter>
            {
                new CompositeSectionParameter(CompositeSectionParameterType.Name, name)
            };

            return new CompositeSection(CompositeSectionType.FilledRHSProfile, materials, sections, parameters);
        }

        /// <summary>
        /// Create a parametric section for FilledRHSProfile type CompositeSection object.
        /// </summary>
        /// <param name="steelRHSProfile">Steel section from database. Can be a KKR or VKR section type.</param>
        /// <returns></returns>
        internal static List<Sections.Section> CreateFilledRHSSection(Section steelRHSProfile)
        {
            // get contours
            var region = steelRHSProfile.RegionGroup.Regions[0];
            List<Contour> contours = new List<Contour> { region.Contours[0], region.Contours[1] };

            // define section data
            MaterialTypeEnum matTypeRHSProfile = (MaterialTypeEnum)Enum.Parse(typeof(MaterialTypeEnum), steelRHSProfile.MaterialType);
            List<MaterialTypeEnum> materialTypes = new List<MaterialTypeEnum> { matTypeRHSProfile, MaterialTypeEnum.Concrete };
            List<string> groupNames = new List<string> { "Steel section", "Concrete section" };     // !the sequence of steel and concrete materials must match the sequence of steel and concrete sections
            List<string> typeNames = materialTypes.Select(x => x.ToString()).ToList();
            List<string> sizeNames = new List<string> { steelRHSProfile.SizeName, "--" };

            return SectionsFromContours(contours, materialTypes, groupNames, typeNames, sizeNames);
        }

        /// <summary>
        /// Create a FilledSteelTube type CompositeSection object.
        /// </summary>
        /// <param name="steel">Steel tube material.</param>
        /// <param name="concrete">Concrete part material.</param>
        /// <param name="name">Composite section name.</param>
        /// <param name="d">Steel tube exterior diameter [mm].</param>
        /// <param name="t">Steel tube thickness [mm].</param>
        /// <returns></returns>
        public static CompositeSection FilledSteelTube(string name, Material steel, Material concrete, double d, double t)
        {
            CheckMaterialFamily(new List<Material> { steel }, concrete);

            List<Material> materials = new List<Material>() { steel, concrete };     // !the sequence of steel and concrete materials must match the sequence of steel and concrete sections
            List<Section> sections = CreateFilledSteelTubeSection(d, t);

            List<CompositeSectionParameter> parameters = new List<CompositeSectionParameter>
            {
                new CompositeSectionParameter(CompositeSectionParameterType.Name, name),
                new CompositeSectionParameter(CompositeSectionParameterType.d, d.ToString()),
                new CompositeSectionParameter(CompositeSectionParameterType.t, t.ToString())
            };

            return new CompositeSection(CompositeSectionType.FilledSteelTube, materials, sections, parameters);
        }

        /// <summary>
        /// Create a parametric section for FilledSteelTube type CompositeSection object.
        /// </summary>
        /// <param name="d">Steel tube exterior diameter [mm].</param>
        /// <param name="t">Steel tube thickness [mm].</param>
        /// <returns></returns>
        internal static List<Sections.Section> CreateFilledSteelTubeSection(double d, double t)
        {
            // round inputs
            d = Math.Round(d, 2, MidpointRounding.AwayFromZero);
            t = Math.Round(t, 2, MidpointRounding.AwayFromZero);

            // check inputs
            if (d <= 0 || t <= 0)
                throw new ArgumentException("Composite section parameters must be positive, non zero numbers!");
            if (d <= 2 * t)
                throw new ArgumentException($"Diameter must be greater than 2t = {2*t}!");

            // conversion of geometric parameters from millimeters to meters
            d = d / 1000;
            t = t / 1000;

            var center = new Point3d(0, 0, 0);
            double extRadius = d / 2;
            double intRadius = (d - 2 * t) / 2;
            var plane = new Plane(center, Vector3d.UnitX, Vector3d.UnitY);

            var extCircle = new CircleEdge(extRadius, center, plane);
            var intCircle = new CircleEdge(intRadius, center, plane);

            // create contours
            var extContour = new Contour(new List<Edge>() { extCircle });
            var intContour = new Contour(new List<Edge>() { intCircle });
            List<Contour> contours = new List<Contour> { extContour, intContour };

            // define section data
            List<MaterialTypeEnum> materialTypes = new List<MaterialTypeEnum> { MaterialTypeEnum.SteelColdWorked, MaterialTypeEnum.Concrete };
            List<string> groupNames = new List<string> { "Steel section", "Concrete section" };
            List<string> typeNames = materialTypes.Select(x => x.ToString()).ToList();

            string steelSizeName = $"{d * 1000}-{t * 1000}";
            string concreteSizeName = $"{(d - 2 * t) * 1000}";
            List<string> sizeNames = new List<string> { steelSizeName, concreteSizeName };


            return SectionsFromContours(contours, materialTypes, groupNames, typeNames, sizeNames);
        }

        /// <summary>
        /// Create a FilledSteelTubeWithIProfile type CompositeSection object.
        /// </summary>
        /// <param name="steelTube">Steel tube's material.</param>
        /// <param name="steelI">I-shaped section's material.</param>
        /// <param name="concrete">Concrete part material.</param>
        /// <param name="steelIProfile">Steel section from database. Must be an I-shaped section type.</param>
        /// <param name="name">Composite section name.</param>
        /// <param name="d">Steel tube exterior diameter [mm].</param>
        /// <param name="t">Steel tube thickness [mm].</param>
        /// <returns></returns>
        public static CompositeSection FilledSteelTubeWithIProfile(string name, Material steelTube, Material steelI, Material concrete, Section steelIProfile, double d, double t)
        {
            CheckMaterialFamily(new List<Material> { steelTube, steelI }, concrete);

            // check input data
            CheckSteelSectionCompatibility(CompositeSectionType.FilledSteelTubeWithIProfile, steelIProfile);

            List<Material> materials = new List<Material>() { steelTube, concrete, steelI };     // !the sequence of steel and concrete materials must match the sequence of steel and concrete sections
            List<Section> sections = CreateFilledSteelTubeWithISection(steelIProfile, d, t);

            List<CompositeSectionParameter> parameters = new List<CompositeSectionParameter>
            {
                new CompositeSectionParameter(CompositeSectionParameterType.Name, name),
                new CompositeSectionParameter(CompositeSectionParameterType.d, d.ToString()),
                new CompositeSectionParameter(CompositeSectionParameterType.t, t.ToString())
            };

            return new CompositeSection(CompositeSectionType.FilledSteelTubeWithIProfile, materials, sections, parameters);
        }

        /// <summary>
        /// Create a parametric section for FilledSteelTubeWithIProfile type CompositeSection object.
        /// </summary>
        /// <param name="steelIProfile">Steel section from database. Must be an I-shaped section type.</param>
        /// <param name="d">Steel tube exterior diameter [mm].</param>
        /// <param name="t">Steel tube thickness [mm].</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        internal static List<Sections.Section> CreateFilledSteelTubeWithISection(Section steelIProfile, double d, double t)
        {
            // round inputs
            d = Math.Round(d, 2, MidpointRounding.AwayFromZero);
            t = Math.Round(t, 2, MidpointRounding.AwayFromZero);

            // check inputs
            if (d <= 0 || t <= 0)
                throw new ArgumentException("Composite section parameters must be positive, non-zero numbers!");
            if (d <= 2 * t)
                throw new ArgumentException($"Diameter must be greater than 2t = {2 * t}!");
            // !Missing error message! -> I profile cannot overlap the CHS section

            // conversion of geometric parameters from millimeters to meters
            d = d / 1000;
            t = t / 1000;

            var center = new Point3d(0, 0, 0);
            double tubeExtRadius = d / 2;
            double tubeIntRadius = (d - 2 * t) / 2;
            var plane = new Plane(center, Vector3d.UnitX, Vector3d.UnitY);

            var tubeExtEdge = new CircleEdge(tubeExtRadius, center, plane);
            var tubeIntEdge = new CircleEdge(tubeIntRadius, center, plane);

            // create contours
            var tubeExtContour = new Contour(new List<Edge>() { tubeExtEdge });
            var tubeIntContour = new Contour(new List<Edge>() { tubeIntEdge });
            var contourIProfile = steelIProfile.RegionGroup.Regions[0].Contours[0];
            List<Contour> contours = new List<Contour> { tubeExtContour, tubeIntContour, contourIProfile };

            // define section data
            MaterialTypeEnum matTypeIProfile = (MaterialTypeEnum)Enum.Parse(typeof(MaterialTypeEnum), steelIProfile.MaterialType);
            List<MaterialTypeEnum> materialTypes = new List<MaterialTypeEnum> { MaterialTypeEnum.SteelWelded, MaterialTypeEnum.Concrete, matTypeIProfile };
            List<string> groupNames = new List<string> { "Steel section", "Concrete section", "Steel section" };     // !the sequence of steel and concrete materials must match the sequence of steel and concrete sections
            List<string> typeNames = materialTypes.Select(x => x.ToString()).ToList();

            string steelTubeSizeName = $"{d * 1000}-{t * 1000}";
            string concreteSizeName = $"{(d - 2 * t) * 1000}";
            List<string> sizeNames = new List<string> { steelTubeSizeName, concreteSizeName, steelIProfile.SizeName };


            return SectionsFromContours(contours, materialTypes, groupNames, typeNames, sizeNames);
        }

        /// <summary>
        /// Create a FilledSteelTubeWithSteelCore type CompositeSection object.
        /// </summary>
        /// <param name="steelTube">Steel tube material.</param>
        /// <param name="steelCore">Steel core material.</param>
        /// <param name="concrete">Concrete part material.</param>
        /// <param name="name">Composite section name.</param>
        /// <param name="d1">Steel tube exterior diameter [mm].</param>
        /// <param name="d2">Steel core diameter [mm].</param>
        /// <param name="t">Steel tube thickness [mm].</param>
        /// <returns></returns>
        public static CompositeSection FilledSteelTubeWithSteelCore(string name, Material steelTube, Material steelCore, Material concrete, double d1, double d2, double t)
        {
            CheckMaterialFamily(new List<Material> { steelTube, steelCore }, concrete);

            List<Material> materials = new List<Material>() { steelTube, concrete, steelCore };     // !the sequence of steel and concrete materials must match the sequence of steel and concrete sections
            List<Section> sections = CreateFilledSteelTubeWithSteelCoreSection(d1, d2, t);

            List<CompositeSectionParameter> parameters = new List<CompositeSectionParameter>
            {
                new CompositeSectionParameter(CompositeSectionParameterType.Name, name),
                new CompositeSectionParameter(CompositeSectionParameterType.d1, d1.ToString()),
                new CompositeSectionParameter(CompositeSectionParameterType.d2, d2.ToString()),
                new CompositeSectionParameter(CompositeSectionParameterType.t, t.ToString())
            };

            return new CompositeSection(CompositeSectionType.FilledSteelTubeWithSteelCore, materials, sections, parameters);
        }

        /// <summary>
        /// Create a parametric section for FilledSteelTubeWithSteelCore type CompositeSection object.
        /// </summary>
        /// <param name="d1">Steel tube exterior diameter [mm].</param>
        /// <param name="d2">Steel core diameter [mm].</param>
        /// <param name="t">Steel tube thickness [mm].</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        internal static List<Sections.Section> CreateFilledSteelTubeWithSteelCoreSection(double d1, double d2, double t)
        {
            // round inputs
            d1 = Math.Round(d1, 2, MidpointRounding.AwayFromZero);
            d2 = Math.Round(d2, 2, MidpointRounding.AwayFromZero);
            t = Math.Round(t, 2, MidpointRounding.AwayFromZero);

            // check inputs
            if (d1 <= 0 || d2 <= 0 || t <= 0)
                throw new ArgumentException("Composite section parameters must be positive, non zero numbers!");
            if (d1 <= 2 * t)
                throw new ArgumentException($"Diameter of steel tube must be greater than 2t = {2 * t}!");
            if (d2 >= (d1 - 2 * t))
                throw new ArgumentException($"Diameter of steel core must be smaller than d1 - 2t = {d1 - 2 * t}!");

            // conversion of geometric parameters from millimeters to meters
            d1 = d1 / 1000;
            d2 = d2 / 1000;
            t = t / 1000;

            var center = new Point3d(0, 0, 0);
            double tubeExtRadius = d1 / 2;
            double tubeIntRadius = (d1 - 2 * t) / 2;
            double coreRadius = d2 / 2;
            var plane = new Plane(center, Vector3d.UnitX, Vector3d.UnitY);

            var tubeExtEdge = new CircleEdge(tubeExtRadius, center, plane);
            var tubeIntEdge = new CircleEdge(tubeIntRadius, center, plane);
            var coreEdge = new CircleEdge(coreRadius, center, plane);

            // create contours
            var tubeExtContour = new Contour(new List<Edge>() { tubeExtEdge });
            var tubeIntContour = new Contour(new List<Edge>() { tubeIntEdge });
            var coreContour = new Contour(new List<Edge>() { coreEdge });
            List<Contour> contours = new List<Contour> { tubeExtContour, tubeIntContour, coreContour };

            // define section data
            List<MaterialTypeEnum> materialTypes = new List<MaterialTypeEnum> { MaterialTypeEnum.SteelWelded, MaterialTypeEnum.Concrete, MaterialTypeEnum.SteelRolled };
            List<string> groupNames = new List<string> { "Steel section", "Concrete section", "Steel section" };     // !the sequence of steel and concrete materials must match the sequence of steel and concrete sections
            List<string> typeNames = materialTypes.Select(x => x.ToString()).ToList();

            string steelTubeSizeName = $"{d1 * 1000}-{t * 1000}";
            string concreteSizeName = $"{(d1 - 2 * t) * 1000}";
            string steelCoreSizeName = $"{(d2) * 1000}";
            List<string> sizeNames = new List<string> { steelTubeSizeName, concreteSizeName, steelCoreSizeName };


            return SectionsFromContours(contours, materialTypes, groupNames, typeNames, sizeNames);
        }


        public static void CheckSteelSectionCompatibility(CompositeSectionType type, Section steelSection)
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

        public static List<Sections.Family> GetCompatibleSteelSectionType(CompositeSectionType type)
        {
            switch (type)
            {
                case CompositeSectionType.IProfileWithEffectiveConcreteSlab:
                    return new List<Sections.Family>()
                    {
                        FemDesign.Sections.Family.HE_A,
                        FemDesign.Sections.Family.HE_B,
                        FemDesign.Sections.Family.HE_M,
                        FemDesign.Sections.Family.I,
                        FemDesign.Sections.Family.IPE,
                        FemDesign.Sections.Family.UKB,
                        FemDesign.Sections.Family.UKC,
                        FemDesign.Sections.Family.KKR,
                        FemDesign.Sections.Family.VKR
                    };
                case CompositeSectionType.FilledDeltaBeamProfile:
                    return new List<Sections.Family>()
                    {
                        FemDesign.Sections.Family.D,
                        FemDesign.Sections.Family.DR
                    };
                case CompositeSectionType.FilledIProfile:
                    return new List<Sections.Family>()
                    {
                        FemDesign.Sections.Family.HE_A,
                        FemDesign.Sections.Family.HE_B,
                        FemDesign.Sections.Family.HE_M,
                        FemDesign.Sections.Family.I,
                        FemDesign.Sections.Family.IPE,
                        FemDesign.Sections.Family.UKB,
                        FemDesign.Sections.Family.UKC
                    };
                case CompositeSectionType.FilledRHSProfile:
                    return new List<Sections.Family>()
                    {
                        FemDesign.Sections.Family.KKR,
                        FemDesign.Sections.Family.VKR
                    };
                case CompositeSectionType.FilledSteelTubeWithIProfile:
                    return new List<Sections.Family>()
                    {
                        FemDesign.Sections.Family.HE_A,
                        FemDesign.Sections.Family.HE_B,
                        FemDesign.Sections.Family.HE_M,
                        FemDesign.Sections.Family.I,
                        FemDesign.Sections.Family.IPE,
                        FemDesign.Sections.Family.UKB,
                        FemDesign.Sections.Family.UKC
                    };
                default:
                    throw new ArgumentException("For this CompositeSectionType you cannot specify a steel section from database.");
            }
        }

        internal bool IsOffsetNeeded(CompositeSectionType type)
        {
            switch (type)
            {
                case CompositeSectionType.IProfileWithEffectiveConcreteSlab:
                    return true;
                case CompositeSectionType.FilledHSQProfile:
                    return true;
                case CompositeSectionType.FilledDeltaBeamProfile:
                    return true;
                case CompositeSectionType.FilledIProfile:
                    return false;
                case CompositeSectionType.FilledCruciformProfile:
                    return false;
                case CompositeSectionType.FilledRHSProfile:
                    return false;
                case CompositeSectionType.FilledSteelTube:
                    return false;
                case CompositeSectionType.FilledSteelTubeWithIProfile:
                    return false;
                case CompositeSectionType.FilledSteelTubeWithSteelCore:
                    return false;
                default:
                    throw new ArgumentException("Incorrect or unknown type.");
            }
        }

        internal static void NotImplemented()
        {
            throw new ArgumentException($"This composite section type is not implemented yet. If needed please contact us. Implemented composite section types: " +
                $"{CompositeSectionType.FilledHSQProfile}, " +
                $"{CompositeSectionType.FilledSteelTube}, " +
                $"{CompositeSectionType.FilledSteelTubeWithSteelCore}, " +
                $"{CompositeSectionType.FilledSteelTubeWithIProfile}, " +
                $"{CompositeSectionType.FilledRHSProfile}.");
        }

        internal static void CheckMaterialFamily(List<Material> steelMaterials, Material concrete)
        {
            foreach(var steel in steelMaterials)
            {
                if(steel.Family != FemDesign.Materials.Family.Steel)
                    throw new ArgumentException($"Each steel material input parameter must be FemDesign.Materials.Family.Steel, but one of them is: {steel.Family}");
            }
            if (concrete.Family != FemDesign.Materials.Family.Concrete)
                throw new ArgumentException($"Concrete material input parameter must be FemDesign.Materials.Family.Concrete, but it is: {concrete.Family}");
        }

        internal void CheckCompositeSectionPartList(CompositeSectionType type, List<CompositeSectionPart> parts)
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
            int steelNum = materialTypes.Select(m => m == FemDesign.Materials.Family.Steel).Count();

            if ((!materialTypes.Contains(FemDesign.Materials.Family.Concrete)) || (!materialTypes.Contains(FemDesign.Materials.Family.Steel)))
                throw new ArgumentException("Check the material types! Composite section must contain at least one steel and one concrete section part.");
            if (type == CompositeSectionType.FilledSteelTubeWithIProfile || type == CompositeSectionType.FilledSteelTubeWithSteelCore)
            {
                if (steelNum != 2)
                    throw new ArgumentException($"{type} must have 2 steel composite section parts, but it has {steelNum}.");
            }
        }

        internal static List<Sections.Section> SectionsFromContours(List<Contour> contours, List<MaterialTypeEnum> materialTypes, List<string> groupNames, List<string> typeNames, List<string> sizeNames)
        {
            // check input lists
            if (contours.Count != materialTypes.Count || contours.Count != groupNames.Count || contours.Count != typeNames.Count || contours.Count != sizeNames.Count)
                throw new ArgumentException("Length of input lists must be the same!");

            List<Section> sections = new List<Section>();
            for(int i = 0; i < contours.Count; i++)
            {
                var region = new Region();
                if(i < contours.Count - 1)
                {
                    region = new Region(new List<Contour> { contours[i], contours[i + 1] });                    
                }
                else
                {
                    region = new Region(new List<Contour> { contours[i] });
                }

                var regionGroup = new RegionGroup(region);

                sections.Add(new Section(regionGroup, "custom", materialTypes[i], groupNames[i], typeNames[i], sizeNames[i]));
            }

            return sections;
        }
    }
}