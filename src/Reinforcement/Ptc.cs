using System;
using System.Xml.Serialization;

#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Reinforcement
{
    [System.Serializable]
    [IsVisibleInDynamoLibrary(false)]
    public class PtcLosses
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
    }

    [System.Serializable]
    [IsVisibleInDynamoLibrary(false)]
    public class PtcShapeType
    {
        [XmlElement("start_point", Order = 1)]
        public PtcShapeStart StartPoint { get; set; }

        [XmlElement("intermediate_point", Order = 2)]
        public PtcShapeInner IntermediatePoint { get; set; }

        [XmlElement("end_point", Order = 3)]
        public PtcShapeEnd EndPoint { get; set; }

        [XmlAttribute("top")]
        public double Top { get; set; }
        [XmlAttribute("bottom")]
        public double Bottom { get; set; }
    }

    [System.Serializable]
    [IsVisibleInDynamoLibrary(false)]
    public class PtcShapeStart
    {
        [XmlAttribute("z")]
        public double Z { get; set; }

        [XmlAttribute("tangent")]
        public double Tangent { get; set; }
    }

    [System.Serializable]
    [IsVisibleInDynamoLibrary(false)]
    public class PtcShapeInner
    {
        [XmlAttribute("pos")]
        public double Position { get; set; }
        [XmlAttribute("z")]
        public double Z { get; set; }
        [XmlAttribute("tangent")]
        public double Tangent { get; set; }
        [XmlAttribute("prior_inflection_pos")]
        public double PriorInflectionPosition { get; set; }
    }

    [System.Serializable]
    [IsVisibleInDynamoLibrary(false)]
    public class PtcShapeEnd
    {
        [XmlAttribute("z")]
        public double Z { get; set; }
        [XmlAttribute("tangent")]
        public double Tangent { get; set; }
        [XmlAttribute("prior_inflection_pos")]
        public double PriorInflectionPosition { get; set; }
    }

    [System.Serializable]
    [IsVisibleInDynamoLibrary(false)]
    public class PtcManufacturingType
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
    }

    [System.Serializable]
    [IsVisibleInDynamoLibrary(false)]
    public class Ptc: EntityBase
    {
        [XmlElement("start_point", Order = 1)]
        public Geometry.FdPoint3d StartPoint { get; set; }

        [XmlElement("end_point", Order = 2)]
        public Geometry.FdPoint3d EndPoint { get; set; }

        [XmlElement("local_z", Order = 3)]
        public Geometry.FdVector3d LocalZ { get; set; }
        
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
        public Guid StrandType { get; set; }

        [XmlAttribute("number_of_strands")]
        public int NumberOfStrands { get; set; }

        [XmlAttribute("jacking_side")]
        public string JackingSide { get; set; }

        [XmlAttribute("jacking_stress")]
        public double JackingStress { get; set; }
    }

    [System.Serializable]
    [IsVisibleInDynamoLibrary(false)]
    public class PtcStrandType
    {
        [XmlElement("predefined_type")]
        public PtcStrandLibType[] PtcStrandLibTypes { get; set; }
    }

    [System.Serializable]
    [IsVisibleInDynamoLibrary(false)]
    public class PtcStrandLibType: LibraryBase
    {
        [XmlElement("ptc_strand_data", Order = 1)]
        public PtcStrandData PtcStrandData { get; set; }
    }

    [System.Serializable]
    [IsVisibleInDynamoLibrary(false)]
    public class PtcStrandData
    {
        [XmlAttribute("f_pk")]
        public double f_pk { get; set; }

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
    }
}