using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FemDesign.Calculate
{
    public enum Length
    {
        mm,
        cm,
        dm,
        m,
        inch,
        feet,
        yd
    }

    public enum Angle
    {
        rad,
        deg
    }

    public enum SectionalData
    {
        mm,
        cm,
        dm,
        m,
        inch,
        feet,
        yd
    }

    public enum Force
    {
        N,
        daN,
        kN,
        MN,
        lbf,
        kips
    }

    public enum Mass
    {
        t,
        kg,
        lb,
        tonUK,
        tonUS
    }

    public enum Displacement
    {
        mm,
        cm,
        dm,
        m,
        inch,
        feet,
        yd
    }

    public enum Stress
    {
        Pa,
        kPa,
        MPa,
        GPa
    }

    /// <summary>
    /// doctable
    /// </summary>
    [System.Serializable]
    public partial class Units
    {
        [XmlElement("num")]
        public int Num { get; set; }

        [XmlElement("unit")]
        public int Unit { get; set; }

        [XmlIgnore]
        public UnitResults UnitResults { get; set; }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        public Units()
        {

        }

        public Units(int num, int unit)
        {
            this.Num = num;
            this.Unit = unit;
        }

        public static List<Units> GetUnits(UnitResults unitResult)
        {
            // Define the Units for some output
            // the schema has been discussed in the following issue
            // https://github.com/strusoft/femdesign-api/issues/375

            var unitsObj = new List<Units>()
            {
                new Units(0, 0),
                new Units(1, (int) unitResult.Angle),
                new Units(2, (int) unitResult.Length),
                new Units(3, (int) unitResult.Force),
                new Units(4, (int) unitResult.Mass),
                new Units(5, (int) unitResult.SectionalData),
                new Units(6, (int) unitResult.Displacement),
                new Units(7, (int) unitResult.Stress),
            };

            // the object between 8 and 63 are not implemented yet

            for (int i = 8; i <= 63; i++)
            {
                unitsObj.Add(new Units(i, 0));
            }

            return unitsObj;
        }
    }


    public partial class UnitResults
    {
        public Length Length { get; set; }
        public Angle Angle { get; set; }
        public SectionalData SectionalData { get; set; }
        public Force Force { get; set; }
        public Mass Mass { get; set; }
        public Displacement Displacement { get; set; }
        public Stress Stress { get; set; }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        public UnitResults()
        {

        }

        public UnitResults(Length length, Angle angle, SectionalData sectionalData, Force force, Mass mass, Displacement displacement, Stress stress)
        {
            this.Length = length;
            this.Angle = angle;
            this.SectionalData = sectionalData;
            this.Force = force;
            this.Mass = mass;
            this.Displacement = displacement;
            this.Stress = stress;
        }
    }
}
