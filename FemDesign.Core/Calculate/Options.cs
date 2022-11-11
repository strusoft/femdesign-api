using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FemDesign.Calculate
{
    /// <summary>
    /// doctable
    /// </summary>
    [System.Serializable]
    public partial class Options
    {
        [XmlElement("bar")]
        public int Bar { get; set; }

        [XmlElement("step")]
        public double Step { get; set; }

        [XmlElement("surface")]
        public int SrfValues { get; set; }

        [XmlIgnore]
        public ResPosition ResPosition { get; set; }

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

        public Options(ListProc listProc, double step)
        {
            
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

    public enum ResPosition
    {
        OnlyNodes,
        ByStep,
        ResultPoint
    }

}
