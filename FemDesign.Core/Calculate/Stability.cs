// https://strusoft.com/
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Linq;

namespace FemDesign.Calculate
{
    public partial class Stability
    {

        [XmlIgnore]
        public List<string> CombNames { get; set; }

        [XmlIgnore]
        public List<int> NumShapes { get; set; }

        [XmlIgnore]
        public bool PositiveOnly { get; set; }

        [XmlIgnore]
        public int numberIteration { get; set; }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        public Stability()
        {

        }

        public Stability(List<string> loadCombinations, List<int> numShapes, bool positiveOnly = false, int numberIteration = 5)
        {
            if(loadCombinations.Count != numShapes.Count)
            {
                throw new System.Exception("Load combinations and number of shapes must have the same number of items!");
            }

            this.CombNames = loadCombinations;
            this.NumShapes = numShapes;
            this.PositiveOnly = positiveOnly;
            this.numberIteration = numberIteration;
        }

        public Stability(string loadCombination, int numShape, bool positiveOnly = false, int numberIteration = 5)
        {
            this.CombNames = new List<string> { loadCombination };
            this.NumShapes = new List<int> { numShape };
            this.PositiveOnly = positiveOnly;
            this.numberIteration = numberIteration;
        }

    }
}