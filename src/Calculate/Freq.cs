// https://strusoft.com/
using System;
using System.Xml.Serialization;


namespace FemDesign.Calculate
{
    /// <summary>
    /// fdscript.xsd
    /// ANALFREQ
    /// </summary>
    public class Freq
    {
        [XmlAttribute("Numshapes")]
        public int Numshapes { get; set; } // int
        [XmlAttribute("MaxSturm")]
        public int MaxSturm { get; set; } // int
        [XmlAttribute("X")]
        public int X { get; set; } // bool // int(?)
        [XmlAttribute("Y")]
        public int Y { get; set; } // bool // int(?)
        [XmlAttribute("Z")]
        public int Z { get; set; } // bool // int(?)
        [XmlAttribute("top")]
        public double top { get; set; } // double

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private Freq()
        {
            
        }

        public Freq(int Numshapes, int MaxSturm = 0, bool X = true, bool Y = true, bool Z = true, double top = 0)
        {
            this.Numshapes = Numshapes;
            this.MaxSturm = MaxSturm;
            this.X = Convert.ToInt32(X);
            this.Y = Convert.ToInt32(Y);
            this.Z = Convert.ToInt32(Z);
            this.top = top;
        }
    }
}