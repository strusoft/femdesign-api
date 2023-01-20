using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Xml.Serialization;
using FemDesign.GenericClasses;
using StruSoft.Interop.StruXml.Data;

namespace FemDesign.Foundations
{
    [System.Serializable]
    [XmlType(TypeName = "borehole")]
    public partial class BoreHole : NamedEntityBase
    {
        [XmlIgnore]
        private static int _boreHoleInstances = 0;
        public static void ResetInstanceCount() => _boreHoleInstances = 0;
        protected override int GetUniqueInstanceCount() => ++_boreHoleInstances;

        [XmlElement]
        public List<double> StrataLevels { get; set; }

        [XmlAttribute("x")]
        public double X { get; set; }

        [XmlAttribute("y")]
        public double Y { get; set; }

        [XmlAttribute("final_ground_level")]
        public double FinalGroundLevel { get; set; }

        [XmlElement("whole_level_data")]
        public AllLevels WholeLevelData { get; set; }

        private BoreHole() { }

        public BoreHole(double x, double y, double finalGroundLevel = 0.00, AllLevels allLevels = null, string identifier = "BH")
        {
            X = x;
            Y = y;
            FinalGroundLevel = finalGroundLevel;
            WholeLevelData = allLevels;
            Identifier = identifier;
            this.EntityCreated();
        }
    }

    public partial class AllLevels
    {
        [XmlElement("strata_top_levels")]
        public string _strataTopLevels;

        [XmlIgnore]
        public List<double> StrataTopLevels
        {
            get
            {
                if (_strataTopLevels != null)
                {
                    return _strataTopLevels.Split(' ').Select(x => double.Parse(x)).ToList();
                }
                else return null;
            }

            set
            {
                if (value != null)
                {
                    _strataTopLevels = String.Join(" ", value);
                }
            }
        }

        [XmlElement("water_levels")]
        public string _waterLevels;

        [XmlIgnore]
        public List<double> WaterLevels
        {
            get
            {
                if (_waterLevels != null)
                {
                    return _waterLevels.Split(' ').Select(x => double.Parse(x)).ToList();
                }
                else return null;
            }
            set
            {
                if(value != null)
                {
                    _waterLevels = String.Join(" ", value);
                }
            }
        }

        public AllLevels() { }

        public AllLevels(List<double> strataTopLevels, List<double> waterLevels)
        {
            StrataTopLevels = strataTopLevels;
            WaterLevels = waterLevels;
        }
    }
}