using FemDesign.Bars;
using FemDesign.GenericClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static FemDesign.Calculate.SteelBarCalculationParameters;

namespace FemDesign.Calculate
{
    /// <summary>
    /// Calculation parameters for steel bars
    /// </summary>
    [System.Serializable]
    public class SteelBarCalculationParameters : CONFIG
    {
        public enum BucklingCurve
        {
            [XmlEnum("-1")]
            [Parseable("Auto", "auto", "AUTO")]
            Auto = -1,
            [XmlEnum("0")]
            [Parseable("a0", "A0")]
            a0,
            [XmlEnum("1")]
            [Parseable("a", "A")]
            a,
            [XmlEnum("2")]
            [Parseable("b", "B")]
            b,
            [XmlEnum("3")]
            [Parseable("c", "C")]
            c,
            [XmlEnum("4")]
            [Parseable("d", "D")]
            d,
        }

        public enum BucklingCurveLt
        {
            [XmlEnum("-1")]
            [Parseable("Auto", "auto", "AUTO")]
            Auto = -1,
            [XmlEnum("0")]
            [Parseable("a", "A")]
            a,
            [XmlEnum("1")]
            [Parseable("b", "B")]
            b,
            [XmlEnum("2")]
            [Parseable("c", "C")]
            c,
            [XmlEnum("3")]
            [Parseable("d", "D")]
            d,
        }

        public enum SecondOrder
        {
            [XmlEnum("0")]
            [Parseable("Ignore", "ignore", "IGNORE")]
            Ignore,
            [XmlEnum("1")]
            [Parseable("ConsiderIfAvailable", "consider_if_available", "Consider_if_available", "CONSIDER_IF_AVAILABLE")]
            ConsiderIfAvailable,
            [XmlEnum("2")]
            [Parseable("ConsiderAndFirstOrderDesign", "consider_and_first_order_design", "Consider_and_first_order_design", "CONSIDER_AND_FIRST_ORDER_DESIGN")]
            ConsiderAndFirstOrderDesign
        }

        [XmlAttribute("type")]
        public string Type = "ECCALCPARAMBARST";

        /// <summary>
        /// Flexural buckling stiff direction
        /// </summary>
        [XmlAttribute("aBucklingCurve_fx1")]
        public BucklingCurve BucklingCurveFx1 { get; set; } = BucklingCurve.Auto;

        /// <summary>
        /// Flexural buckling weak direction
        /// </summary>
        [XmlAttribute("aBucklingCurve_fx2")]
        public BucklingCurve BucklingCurveFx2 { get; set; } = BucklingCurve.Auto;

        /// <summary>
        /// Torsional-flexural buckling
        /// </summary>
        [XmlAttribute("aBucklingCurve_tf")]
        public BucklingCurve BucklingCurveTf { get; set; } = BucklingCurve.Auto;

        /// <summary>
        /// Lateral-torsional buckling bottom flange
        /// </summary>
        [XmlAttribute("aBucklingCurve_ltb")]
        public BucklingCurveLt BucklingCurveLtb { get; set; } = BucklingCurveLt.Auto;

        /// <summary>
        /// Lateral-torsional buckling top flange
        /// </summary>
        [XmlAttribute("aBucklingCurve_ltt")]
        public BucklingCurveLt BucklingCurveLtt { get; set; } = BucklingCurveLt.Auto;

        [XmlAttribute("CheckResistanceOnly")]
        public int _checkResistanceOnly = 0;

        [XmlIgnore]
        public bool CheckResistanceOnly
        {
            get
            {
                return System.Convert.ToBoolean(this._checkResistanceOnly);
            }
            set
            {
                this._checkResistanceOnly = System.Convert.ToInt32(value);
            }
        }


        [XmlAttribute("class4Ignored")]
        public int _class4Ignored = 0;

        [XmlIgnore]
        public bool Class4Ignored
        {
            get
            {
                return System.Convert.ToBoolean(this._class4Ignored);
            }
            set
            {
                this._class4Ignored = System.Convert.ToInt32(value);
            }
        }

        [XmlAttribute("convergencyratio")]
        public double ConvergencyRatio { get; set; } = 1.0;

        [XmlAttribute("fLatTorBuckGen")]
        public int _fLatTorBuckGen = 1;

        [XmlIgnore]
        public bool LatTorBuckGen
        {
            get
            {
                return System.Convert.ToBoolean(this._fLatTorBuckGen);
            }
            set
            {
                this._fLatTorBuckGen = System.Convert.ToInt32(value);
            }
        }


        [XmlAttribute("fLatTorBuckGenSpecForI")]
        public int _fLatTorBuckGenSpecForI = 0;

        [XmlIgnore]
        public bool LatTorBuckGenSpecForI
        {
            get
            {
                return System.Convert.ToBoolean(this._fLatTorBuckGenSpecForI);
            }
            set
            {
                this._fLatTorBuckGenSpecForI = System.Convert.ToInt32(value);
            }
        }

        [XmlAttribute("maxIterStep")]
        public int MaxIterStep { get; set; } = 50;

        [XmlAttribute("plasticIgnored")]
        public int _plasticIgnored = 0;

        [XmlIgnore]
        public bool PlasticIgnored
        {
            get
            {
                return System.Convert.ToBoolean(this._plasticIgnored);
            }
            set
            {
                this._plasticIgnored = System.Convert.ToInt32(value);
            }
        }

        /// <summary>
        /// Max. distance between calculated sections [m]
        /// </summary>
        [XmlAttribute("rStep")]
        public double DistanceCalculatedSection { get; set; } = 0.50;

        [XmlAttribute("s2ndOrder")]
        public SecondOrder S2ndOrder { get; set; } = SecondOrder.ConsiderIfAvailable;

        [XmlAttribute("UseEqation6_41")]
        public int _useEquation6_41 = 1;

        [XmlIgnore]
        public bool UseEquation6_41
        {
            get
            {
                return System.Convert.ToBoolean(this._useEquation6_41);
            }
            set
            {
                this._useEquation6_41 = System.Convert.ToInt32(value);
            }
        }

        /// <summary>
        /// List of BarPart GUIDs to apply the parameters to
        /// </summary>
        [XmlElement("GUID")]
        public List<Guid> Guids { get; set; }

        public void SetParametersOnBars(List<Bar> bars)
        {

            this.Guids = bars.Select(x => x.BarPart.Guid).ToList();
        }

        public void SetParametersOnBars(Bar bars)
        {
            this.Guids = new List<Guid> { bars.BarPart.Guid };
        }
        public SteelBarCalculationParameters()
        {
        }

        public SteelBarCalculationParameters(double sectionsDistance, SecondOrder secondOrder, bool plasticCalculation, bool equation641, bool class4, bool ignore, double convergence, int iteration, BucklingCurve stiffDirection, BucklingCurve weakDirection, BucklingCurve torsionalDirection, bool en1993_1_1_6_3_2_2, bool en1993_1_1_6_3_2_3, BucklingCurveLt topFlange, BucklingCurveLt bottomFlange)
        {
            this.DistanceCalculatedSection = sectionsDistance;
            this.S2ndOrder = secondOrder;
            this.PlasticIgnored = plasticCalculation;
            this.UseEquation6_41 = equation641;
            this.Class4Ignored = class4;
            this.CheckResistanceOnly = ignore;
            this.ConvergencyRatio = convergence;
            this.MaxIterStep = iteration;
            this.BucklingCurveFx1 = stiffDirection;
            this.BucklingCurveFx2 = weakDirection;
            this.BucklingCurveTf = torsionalDirection;
            this.LatTorBuckGen = en1993_1_1_6_3_2_2;
            this.LatTorBuckGenSpecForI = en1993_1_1_6_3_2_3;
            this.BucklingCurveLtb = topFlange;
            this.BucklingCurveLtt = bottomFlange;
        }
    }
}