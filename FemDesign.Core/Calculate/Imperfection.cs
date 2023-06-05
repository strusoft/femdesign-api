using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FemDesign.Calculate
{
    public partial class Imperfection : Stability
    {
        [XmlIgnore]
        public bool InitialiseAmplitude { get; set; } = true;
        private Imperfection() { }
        public Imperfection(List<string> loadCombinations, List<int> numShapes, bool positiveOnly = false, int numberIteration = 5, bool initialiseAmplitude = true) : base(loadCombinations, numShapes, positiveOnly, numberIteration)
        {
            this.InitialiseAmplitude = initialiseAmplitude;
        }
    }
}
