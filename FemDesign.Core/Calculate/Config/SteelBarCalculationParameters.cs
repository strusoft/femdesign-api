using FemDesign.Bars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

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
            Auto = -1,
            [XmlEnum("0")]
            a0,
            [XmlEnum("1")]
            a,
            [XmlEnum("2")]
            b,
            [XmlEnum("3")]
            c,
            [XmlEnum("4")]
            d,
        }

        public enum BucklingCurveLt
        {
            [XmlEnum("-1")]
            Auto = -1,
            [XmlEnum("0")]
            a,
            [XmlEnum("1")]
            b,
            [XmlEnum("2")]
            c,
            [XmlEnum("3")]
            d,
        }

        public enum SecondOrder
        {
            [XmlEnum("0")]
            Ignore,
            [XmlEnum("1")]
            ConsiderIfAvailable,
            [XmlEnum("2")]
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
    }
}
