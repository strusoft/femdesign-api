using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using FemDesign.GenericClasses;

namespace FemDesign.Calculate
{
    /// <summary>
    /// Options
    /// </summary>
    [System.Serializable]
    public partial class Options
    {
        [XmlElement("bar")]
        public int Bar { get; set; }

        [XmlElement("step")]
        public double _step { get; set; }

        [XmlIgnore]
        public double Step
        {
            get { return this._step; }
            set { this._step = value; }
        }

        [XmlElement("surface")]
        public int SrfValues { get; set; }


        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        public Options()
        {

        }

        private Options(int bar, double step)
        {
            this.Bar = bar;
            this.Step = step;
        }

        private Options(int srf, string type)
        {
            this.SrfValues = srf;
        }

        /// <summary>
        /// Specify the result output locations.
        /// </summary>
        /// <param name="barResult"></param>
        /// <param name="shellResult"></param>
        /// <param name="step">Distance between nodal output results for bar element</param>
        public Options(BarResultPosition barResult, ShellResultPosition shellResult, double step = 0.50)
        {
            this.Bar = (int)barResult;
            if(barResult == BarResultPosition.ByStep)
                this.Step = step;
            this.SrfValues = (int)shellResult;
        }

        public static Options Default()
        {
            return new Options(BarResultPosition.ByStep, ShellResultPosition.Vertices, 0.50);
        }

        // Assumption
        // the method is returning the results every 50 cm for bar elements.
        // Future Development...Get the user to decide the step ?
        // It is RUBBISH code developed to Deconstruct the Results

        public static Options GetOptions(ListProc resultType)
        {
            string r = resultType.ToString();
            if (r.StartsWith("BarsInternalForces") || r.StartsWith("BarsStresses") || r.StartsWith("BarsDisplacements") || r.StartsWith("LabelledSection"))
            {
                return new Options(1, 0.5);
            }
            else if (r.StartsWith("ShellDisplacement") || r.StartsWith("ShellStress") || r.StartsWith("ShellInternalForce") || r.StartsWith("ShellDerivedForce") || r.StartsWith("SurfaceSupportReaction"))
            {
                return new Options(1, "srf");
            }
            else
            {
                return new Options();
            }
        }
    }

    public enum BarResultPosition
    {
        [Parseable("OnlyNodes", "0")]
        OnlyNodes = 0,

        [Parseable("ByStep", "1")]
        ByStep = 1,

        [Parseable("ResultPoints", "2")]
        ResultPoints = 2,
    }

    public enum ShellResultPosition
    {
        [Parseable("Center", "0")]
        Center = 0,

        [Parseable("Vertices", "1")]
        Vertices = 1,

        [Parseable("ResultPoints", "2")]
        ResultPoints = 2,
    }
}
