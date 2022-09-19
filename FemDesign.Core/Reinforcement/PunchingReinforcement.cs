using System.Xml.Serialization;


namespace FemDesign.Reinforcement
{
    [System.Serializable]
    public partial class PunchingReinforcement: EntityBase
    {
        [XmlElement("base_shell", Order = 1)]
        public GuidListType BaseShell { get; set; }

        [XmlElement("punching_area", Order = 2)]
        public GuidListType PunchingArea { get; set; }

        // choice bended_bar
        [XmlElement("bended_bar", Order = 3)]
        public BendedBar BendedBar { get; set; }

        // choice open_stirrups
        [XmlElement("open_stirrups", Order = 4)]
        public OpenStirrups OpenStirrups { get; set; }

        // choice reinforcing_ring
        [XmlElement("reinforcing_ring", Order = 5)]
        public ReinforcingRing ReinforcingRing { get; set; }

        // choice stud_rails
        [XmlElement("stud_rails", Order = 6)]
        public StudRails StudRails { get; set; }
    }

    [System.Serializable]
    public partial class BendedBar
    {
        [XmlElement("local_center", Order = 1)]
        public Geometry.Point3d LocalCenter { get; set; }

        [XmlElement("wire", Order = 2)]
        public Wire Wire { get; set; }

        [XmlAttribute("tip_sections_length")]
        public double TipSectionsLength { get; set; }

        [XmlAttribute("middle_sections_length")]
        public double MiddleSectionsLength { get; set; }

        [XmlAttribute("height")]
        public double Height { get; set; }

        [XmlAttribute("angle")]
        public double Angle { get; set; }

        [XmlAttribute("direction")]
        public string Direction { get; set; }
    }

    [System.Serializable]
    public partial class OpenStirrups
    {
        [XmlElement("wire", Order = 1)]
        public Wire Wire { get; set; }

        [XmlElement("region", Order = 2)]
        public Geometry.Region Region { get; set; }

        [XmlAttribute("width")]
        public double Width { get; set; }

        [XmlAttribute("length")]
        public double Length { get; set; }

        [XmlAttribute("height")]
        public double Height { get; set; }

        [XmlAttribute("distance_x")]
        public double DistanceX { get; set; }

        [XmlAttribute("distance_y")]
        public double DistanceY { get; set; }
    }

    [System.Serializable]
    public partial class ReinforcingRing
    {
        [XmlElement("auxiliary_reinforcement", Order = 1)]
        public AuxiliaryReinforcement AuxiliaryReinforcement { get; set; }

        [XmlElement("stirrups", Order = 2)]
        public ReinforcingRingStirrups Stirrups { get; set; }
    }

    [System.Serializable]
    public partial class AuxiliaryReinforcement
    {
        [XmlElement("wire", Order = 1)]
        public Wire Wire { get; set; }

        [XmlAttribute("inner_radius")]
        public double InnerRadius { get; set; }

        [XmlAttribute("overlap")]
        public double Overlap { get; set; }
    }

    [System.Serializable]
    public partial class ReinforcingRingStirrups
    {
        [XmlElement("wire", Order = 1)]
        public Wire Wire { get; set; }

        [XmlAttribute("width")]
        public double Width { get; set; }

        [XmlAttribute("height")]
        public double Height { get; set; }

        [XmlAttribute("max_distance")]
        public double MaxDistance { get; set; }
    }

    [System.Serializable]
    public partial class StudRails
    {
        // choice general_product
        [XmlElement("general_product", Order = 1)]
        public GeneralProduct GeneralProduct { get; set; }

        // choice peikko_psb_product
        [XmlElement("peikko_psb_product", Order = 2)]
        public PeikkoPsbProduct PeikkoPsbProduct { get; set; }

        [XmlAttribute("pattern")]
        public string Patter { get; set; }

        [XmlAttribute("s0")]
        public double S0 { get; set; }

        [XmlAttribute("s1")]
        public double S1 { get; set; }

        [XmlAttribute("s2")]
        public double S2 { get; set; }

        [XmlAttribute("rails_on_circle")]
        public string _railsOnCircle;

        [XmlIgnore]
        public int RailsOnCircle
        {
            get
            {
                return System.Int32.Parse(this._railsOnCircle);
            }
            set
            {
                this._railsOnCircle = value.ToString();
            }
        }

        [XmlAttribute("studs_on_rail")]
        public string _studsOnRail;

        [XmlIgnore]
        public int StudsOnRail
        {
            get
            {
                return System.Int32.Parse(this._studsOnRail);
            }
            set
            {
                this._studsOnRail = value.ToString();
            }
        }

        [XmlAttribute("height")]
        public string _height;

        [XmlIgnore]
        public double Height
        {
            get
            {
                return double.Parse(this._height);
            }
            set
            {
                this._height = value.ToString();
            }
        }
        
        [XmlAttribute("use_minimal_elements")]
        public bool UseMinimalElements { get; set; }
    }

    [System.Serializable]
    public partial class GeneralProduct
    {
        [XmlElement("wire", Order = 1)]
        public Wire Wire { get; set; }
    }

    [System.Serializable]
    public partial class PeikkoPsbProduct
    {
        [XmlElement("psh", Order = 1)]
        public PshData Psh { get; set; }

        [XmlAttribute("wire_diameter")]
        public double WireDiameter { get; set; }
    }

    [System.Serializable]
    public partial class PshData
    {
        [XmlAttribute("diameter")]
        public double Diameter { get; set; }
        [XmlAttribute("cd")]
        public double Cd { get; set; }
        [XmlAttribute("n_x_dir")]
        public int NumXDirection { get; set; }
        [XmlAttribute("n_y_dir")]
        public int NumYDireciton { get; set; }
    }
}