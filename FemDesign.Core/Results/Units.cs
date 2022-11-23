using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

#if ISDYNAMO
using Autodesk.DesignScript.Runtime;
#endif

namespace FemDesign.Results
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
    #if ISDYNAMO
    [IsVisibleInDynamoLibrary(false)]
    #endif
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
        private Units()
        {

        }

        private Units(int num, int unit)
        {
            this.Num = num;
            this.Unit = unit;
        }

        public static List<Units> GetUnits(UnitResults unitResult = null)
        {
            // Define the Units for some output
            // the schema has been discussed in the following issue
            // https://github.com/strusoft/femdesign-api/issues/375

            if(unitResult == null)
            {
                unitResult = UnitResults.Default();
            }

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

    #if ISDYNAMO
    [IsVisibleInDynamoLibrary(false)]
    #endif
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

        #if ISDYNAMO
        [IsVisibleInDynamoLibrary(true)]
        #endif
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

        /// <summary>
        /// Returns the Default UnitResults
        /// </summary>
        #if ISDYNAMO
        [IsVisibleInDynamoLibrary(true)]
        #endif
        public static UnitResults Default()
        {
            return new UnitResults(Results.Length.m, Results.Angle.deg, Results.SectionalData.m, Results.Force.kN, Results.Mass.kg, Results.Displacement.m, Results.Stress.Pa);
        }
    }
}
