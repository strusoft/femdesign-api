using System;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Serialization;
using FemDesign.GenericClasses;
using FemDesign.Bars;

namespace FemDesign.Reinforcement
{
    public enum JackingSide
    {
        [Parseable("start", "Start", "START")]
        [XmlEnum("start")]
        Start,
        [Parseable("end", "End", "END")]
        [XmlEnum("end")]
        End,
        [Parseable("start_then_end", "start then end", "Start then end", "StartThenEnd", "START_THEN_END")]
        [XmlEnum("start_then_end")]
        StartThenEnd,
        [Parseable("end_then_start", "end then start", "End then start", "EndThenStart", "END_THEN_START")]
        [XmlEnum("end_then_start")]
        EndThenStart,
    }

    [System.Serializable]
    public partial class PtcLosses
    {
        [XmlAttribute("curvature_coefficient")]
        public double CurvatureCoefficient { get; set; }

        [XmlAttribute("wobble_coefficient")]
        public double WobbleCoefficient { get; set; }

        [XmlAttribute("anchorage_set_slip")]
        public double AnchorageSetSlip { get; set; }
        [XmlAttribute("elastic_shortening")]
        public double ElasticShortening { get; set; }

        [XmlAttribute("creep_stress")]
        public double CreepStress { get; set; }

        [XmlAttribute("shrinkage_stress")]
        public double ShrinkageStress { get; set; }

        [XmlAttribute("relaxation_stress")]
        public double RelaxationStress { get; set; }

        /// <summary>
        /// Private constructor for serialization
        /// </summary>
        private PtcLosses()
        {

        }

        public PtcLosses(double curvatureCoefficient, double wobbleCoefficient, double anchorageSetSlip, double elasticShortening, double creepStress, double shrinkageStress, double relaxationStress)
        {
            CurvatureCoefficient = curvatureCoefficient;
            WobbleCoefficient = wobbleCoefficient;
            AnchorageSetSlip = anchorageSetSlip;
            ElasticShortening = elasticShortening;
            CreepStress = creepStress;
            ShrinkageStress = shrinkageStress;
            RelaxationStress = relaxationStress;
        }
    }

    /// <summary>
    /// Defines the shape of a post-tensioned cable (PTC) element.
    /// </summary>
    [System.Serializable]
    public partial class PtcShapeType
    {
        [XmlElement("start_point", Order = 1)]
        public PtcShapeStart StartPoint { get; set; }

        [XmlElement("intermediate_point", Order = 2)]
        public PtcShapeInner[] IntermediatePoint { get; set; }

        [XmlElement("end_point", Order = 3)]
        public PtcShapeEnd EndPoint { get; set; }

        [XmlAttribute("top")]
        public double Top { get; set; }
        [XmlAttribute("bottom")]
        public double Bottom { get; set; }

        /// <summary>
        /// Private constructor for serialization
        /// </summary>
        private PtcShapeType()
        {

        }

        /// <summary>
        /// Defines the shape of a Post Tensioned Cable.
        /// </summary>
        public PtcShapeType(PtcShapeStart start, PtcShapeEnd end, IEnumerable<PtcShapeInner> intermediates)
        {
            DenormalizePriorInflectionParameters(ref end, ref intermediates);

            StartPoint = start;
            EndPoint = end;
            IntermediatePoint = intermediates.ToArray();
        }

        /// <summary>
        /// Change normalization on PriorInflectionPosition from normalized along parent element to be normalized between ptc shape base points.
        /// </summary>
        /// <param name="end"></param>
        /// <param name="intermediates"></param>
        private void DenormalizePriorInflectionParameters(ref PtcShapeEnd end, ref IEnumerable<PtcShapeInner> intermediates)
        {
            var sortedInner = intermediates.OrderBy(i => i.Position).ToList();
            List<PtcShapeInner> newIntermediates = new List<PtcShapeInner>();

            double Denormalize(double t, double x1, double x2)
            {
                if (t < x1 || t > x2)
                    throw new ArgumentException($"PriorInflectionPosition must be a parameter between prior end next basepoint but got (position: {t}, prior: {x1}, next: {x2})");
                return (t - x1) / (x2 - x1);
            }

            // Normalize inner
            for (int i = 0; i < sortedInner.Count; i++)
            {
                var next = sortedInner[i];
                if (next.PriorInflectionPosition.HasValue)
                {
                    double x1 = 0.0;
                    if (i > 0)
                        x1 = sortedInner[i - 1].Position;
                    double x2 = next.Position;
                    double t = next.PriorInflectionPosition.Value;

                    newIntermediates.Add(new PtcShapeInner(next.Position, next.Z, next.Tangent, Denormalize(t, x1, x2)));
                }
                else
                {
                    newIntermediates.Add(next);
                }
            }
            intermediates = newIntermediates;

            // Normalize end
            if (sortedInner.Any() && end.PriorInflectionPosition.HasValue && end.NormalizedInflectionPosition)
            {
                double x1 = sortedInner.Last().Position;
                double x2 = 1.0;
                double t = end.PriorInflectionPosition.Value;

                end = new PtcShapeEnd(end.Z, end.Tangent, Denormalize(t, x1, x2)) { NormalizedInflectionPosition = false };
            }
        }
    }

    [System.Serializable]
    public partial class PtcShapeStart
    {
        [XmlAttribute("z")]
        public double Z { get; set; }

        [XmlAttribute("tangent")]
        public double Tangent { get; set; }

        /// <summary>
        /// Parameterless constructor for serialization
        /// </summary>
        private PtcShapeStart() { }

        public PtcShapeStart(double z = 0.0, double tangent = 0.0)
        {
            Z = z;
            Tangent = tangent;
        }
    }

    [System.Serializable]
    public partial class PtcShapeInner
    {
        /// <summary>
        /// x'
        /// </summary>
        [XmlAttribute("pos")]
        public double Position { get; set; }
        /// <summary>
        /// z'
        /// </summary>
        [XmlAttribute("z")]
        public double Z { get; set; }
        [XmlAttribute("tangent")]
        public double Tangent { get; set; }
        [XmlAttribute("prior_inflection_pos")]
        public string _priorInflectionPosition;
        [XmlIgnore]
        public double? PriorInflectionPosition
        {
            get
            {
                return _priorInflectionPosition == null ? (double?)null : (double?)double.Parse(_priorInflectionPosition, System.Globalization.CultureInfo.InvariantCulture);
            }
            set
            {
                _priorInflectionPosition = value.HasValue ? ((double)value).ToString(System.Globalization.CultureInfo.InvariantCulture) : null;
            }
        }

        /// <summary>
        /// Internal property to reflect whether or not the PriorInflectionPosition is been normalized along the parent element or not. <br/><br/>
        /// 
        /// true: <see cref="PriorInflectionPosition"/> is normalized (0-1) along parant element. <br/>
        /// false: <see cref="PriorInflectionPosition"/> is normalized (0-1) between prior and next basepoints. <br/><br/>
        /// 
        /// When serializing to struxml this shold have been set to false by <see cref="PtcShapeType.DenormalizePriorInflectionParameters(ref PtcShapeEnd, ref IEnumerable{PtcShapeInner})"/>.
        /// </summary>
        internal bool NormalizedInflectionPosition = true;

        /// <summary>
        /// Parameterless constructor for serialization
        /// </summary>
        private PtcShapeInner() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x">Parameter along axis of element.</param>
        /// <param name="z">Height</param>
        /// <param name="tangent">Tangent of cable at the inner point</param>
        /// <param name="priorInflectionPosition">Parameter of prior inflection point, if any. Optional</param>
        public PtcShapeInner(double x, double z = 0.0, double tangent = 0.0, double? priorInflectionPosition = null)
        {
            Position = x;
            Z = z;
            Tangent = tangent;
            PriorInflectionPosition = priorInflectionPosition;
        }
    }

    [System.Serializable]
    public partial class PtcShapeEnd
    {
        [XmlAttribute("z")]
        public double Z { get; set; }
        [XmlAttribute("tangent")]
        public double Tangent { get; set; }
        [XmlAttribute("prior_inflection_pos")]
        public string _priorInflectionPosition;
        [XmlIgnore]
        public double? PriorInflectionPosition
        {
            get
            {
                return _priorInflectionPosition == null ? (double?)null : (double?)double.Parse(_priorInflectionPosition, System.Globalization.CultureInfo.InvariantCulture);
            }
            set
            {
                _priorInflectionPosition = value.HasValue ? ((double)value).ToString(System.Globalization.CultureInfo.InvariantCulture) : null;
            }
        }

        /// <summary>
        /// Internal property to reflect whether or not the PriorInflectionPosition is been normalized along the parent element or not. <br/><br/>
        /// 
        /// true: <see cref="PriorInflectionPosition"/> is normalized (0-1) along parant element. <br/>
        /// false: <see cref="PriorInflectionPosition"/> is normalized (0-1) between prior and next basepoints. <br/><br/>
        /// 
        /// When serializing to struxml this shold have been set to false by <see cref="PtcShapeType.DenormalizePriorInflectionParameters(ref PtcShapeEnd, ref IEnumerable{PtcShapeInner})"/>.
        /// </summary>
        internal bool NormalizedInflectionPosition = true;

        /// <summary>
        /// Parameterless constructor for serialization
        /// </summary>
        private PtcShapeEnd() { }

        public PtcShapeEnd(double z = 0.0, double tangent = 0.0, double? priorInflectionPosition = null)
        {
            Z = z;
            Tangent = tangent;
            PriorInflectionPosition = priorInflectionPosition;
        }
    }

    [System.Serializable]
    public partial class PtcManufacturingType
    {
        private double[] _positions;

        [XmlElement("positions", Order = 1)]
        public string Positions
        {
            get
            {
                string str = "";

                for (int idx = 0; idx < this._positions.Length; idx++)
                {
                    if (idx == 0)
                    {

                    }
                    else
                    {
                        str = str + " ";
                    }

                    str = str + _positions[idx].ToString(System.Globalization.CultureInfo.InvariantCulture);
                }

                return str;
            }
            set
            {

                string[] vals = value.Split(' ');
                double[] dbls = new double[vals.Length];

                for (int idx = 0; idx < vals.Length; idx++)
                {
                    dbls[idx] = double.Parse(vals[idx], System.Globalization.CultureInfo.InvariantCulture);
                }

                this._positions = dbls;
            }
        }

        [XmlAttribute("shift_x")]
        public double ShiftX { get; set; }

        [XmlAttribute("shift_z")]
        public double ShiftZ { get; set; }

        /// <summary>
        /// Private constructor for serialization
        /// </summary>
        private PtcManufacturingType()
        {

        }

        public PtcManufacturingType(List<double> positions, double shiftX, double shiftZ)
        {
            _positions = positions.ToArray();
            ShiftX = shiftX;
            ShiftZ = shiftZ;
        }
    }

    [System.Serializable]
    public partial class Ptc : EntityBase, IStructureElement
    {
        [XmlElement("start_point", Order = 1)]
        public Geometry.Point3d StartPoint { get; set; }

        [XmlElement("end_point", Order = 2)]
        public Geometry.Point3d EndPoint { get; set; }

        [XmlElement("local_z", Order = 3)]
        public Geometry.Vector3d LocalZ { get; set; }

        [XmlElement("losses", Order = 4)]
        public PtcLosses Losses { get; set; }

        [XmlElement("shape_base_points", Order = 5)]
        public PtcShapeType ShapeBasePoints { get; set; }

        [XmlElement("manufacturing", Order = 6)]
        public PtcManufacturingType Manufacturing { get; set; }

        [XmlAttribute("base_object")]
        public Guid BaseObject { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }


        [XmlAttribute("strand_type")]
        public Guid StrandTypeGuid { get; set; }
        
        [XmlIgnore]
        public PtcStrandLibType StrandType { get; set; }

        [XmlAttribute("number_of_strands")]
        public int NumberOfStrands { get; set; }

        [XmlAttribute("jacking_side")]
        public JackingSide JackingSide { get; set; }

        [XmlAttribute("jacking_stress")]
        public double JackingStress { get; set; }

        private static int instances = 0;

        /// <summary>
        /// Private constructor for serialization
        /// </summary>
        private Ptc()
        {

        }

        /// <summary>
        /// Construct post-tension cable
        /// </summary>
        /// <param name="bar">Reference bar element</param>
        /// <param name="shape"></param>
        /// <param name="losses"></param>
        /// <param name="manufacturing"></param>
        /// <param name="strand"></param>
        /// <param name="jackingSide"></param>
        /// <param name="jackingStress"></param>
        /// <param name="numberOfStrands"></param>
        /// <param name="identifier"></param>
        public Ptc(Bars.Bar bar, PtcShapeType shape, PtcLosses losses, PtcManufacturingType manufacturing, PtcStrandLibType strand, JackingSide jackingSide, double jackingStress, int numberOfStrands = 3, string identifier = "PTC")
        {
            if (bar.BarPart.Edge.Type == "line")
            {
                var start = bar.BarPart.Edge.Points[0];
                var end = bar.BarPart.Edge.Points[1];
                Initialize(start, end, bar.BarPart.Guid, shape, losses, manufacturing, strand, jackingSide, jackingStress, numberOfStrands, identifier);
            }
            else
                throw new ArgumentException($"Bar must be of type line but got '{bar.BarPart.Edge.Type}'", "bar");
        }
        /// <summary>
        /// Construct post-tension cable
        /// </summary>
        /// <param name="slab">Reference slab element</param>
        /// <param name="line">Cable line</param>
        /// <param name="shape"></param>
        /// <param name="losses"></param>
        /// <param name="manufacturing"></param>
        /// <param name="strand"></param>
        /// <param name="jackingSide"></param>
        /// <param name="jackingStress"></param>
        /// <param name="numberOfStrands"></param>
        /// <param name="identifier"></param>
        public Ptc(Shells.Slab slab, Geometry.LineSegment line, PtcShapeType shape, PtcLosses losses, PtcManufacturingType manufacturing, PtcStrandLibType strand, JackingSide jackingSide, double jackingStress, int numberOfStrands = 3, string identifier = "PTC")
        {
            Initialize(line.StartPoint, line.EndPoint, slab.SlabPart.Guid, shape, losses, manufacturing, strand, jackingSide, jackingStress, numberOfStrands, identifier);
        }

        private void Initialize(Geometry.Point3d start, Geometry.Point3d end, Guid baseObject, PtcShapeType shape, PtcLosses losses, PtcManufacturingType manufacturing, PtcStrandLibType strand, JackingSide jackingSide, double jackingStress, int numberOfStrands, string identifier)
        {
            StartPoint = start;
            EndPoint = end;

            BaseObject = baseObject;
            StrandType = strand;
            StrandTypeGuid = strand.Guid;
            Name = $"{identifier}.{++instances}";

            Losses = losses;
            NumberOfStrands = numberOfStrands;
            JackingSide = jackingSide;
            JackingStress = jackingStress;
            ShapeBasePoints = shape;
            Manufacturing = manufacturing;

            EntityCreated();
        }
    }

    [System.Serializable]
    public partial class PtcStrandType
    {
        [XmlElement("predefined_type")]
        public List<PtcStrandLibType> PtcStrandLibTypes { get; set; } = new List<PtcStrandLibType>();
    }

    [System.Serializable]
    public partial class PtcStrandLibType : LibraryBase
    {
        [XmlElement("ptc_strand_data", Order = 1)]
        public PtcStrandData PtcStrandData { get; set; }

        /// <summary>
        /// Parameterless constructor for serialization
        /// </summary>
        private PtcStrandLibType()
        {

        }

        /// <summary>
        /// Create a custom PTC-strand.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="f_pk">f pk [N/mm2]</param>
        /// <param name="a_p">A p [mm2]</param>
        /// <param name="e_p">E p [N/mm2]</param>
        /// <param name="rho">Rho [t/mm3]</param>
        /// <param name="relaxationClass">Relaxation class [1, 2, 3] </param>
        /// <param name="rho_1000">Rho 1000 [%]</param>
        public PtcStrandLibType(string name, double f_pk, double a_p, double e_p, double rho, int relaxationClass, double rho_1000)
        {
            Name = name;
            PtcStrandData = new PtcStrandData(f_pk, a_p, e_p, rho, relaxationClass, rho_1000);
            EntityCreated();
        }
    }

    /// <summary>
    /// Strand material/type data
    /// </summary>
    [System.Serializable]
    public partial class PtcStrandData : EntityBase
    {
        /// <summary>
        /// f pk
        /// </summary>
        [XmlAttribute("f_pk")]
        public double F_pk { get; set; }

        [XmlAttribute("A_p")]
        public double A_p { get; set; }

        [XmlAttribute("E_p")]
        public double E_p { get; set; }

        [XmlAttribute("density")]
        public double Density { get; set; }

        [XmlAttribute("relaxation_class")]
        public int RelaxationClass { get; set; }

        [XmlAttribute("Rho_1000")]
        public double Rho_1000 { get; set; }

        /// <summary>
        /// Parameterless constructor for serialization
        /// </summary>
        private PtcStrandData()
        {

        }

        /// <summary>
        /// Create a custom PTC-strand.
        /// </summary>
        /// <param name="f_pk">f pk [N/mm2]</param>
        /// <param name="a_p">A p [mm2]</param>
        /// <param name="e_p">E p [N/mm2]</param>
        /// <param name="density">Rho [t/mm3]</param>
        /// <param name="relaxationClass">Relaxation class [1, 2, 3] </param>
        /// <param name="rho_1000">Rho 1000 [%]</param>
        public PtcStrandData(double f_pk, double a_p, double e_p, double density, int relaxationClass, double rho_1000)
        {
            F_pk = f_pk;
            A_p = a_p;
            E_p = e_p;
            Density = density;
            RelaxationClass = relaxationClass;
            Rho_1000 = rho_1000;
        }
    }
}