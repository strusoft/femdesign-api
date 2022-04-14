using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Globalization;

namespace FemDesign.Results
{
    /// <summary>
    /// FemDesign "EigenFrequencies" result
    /// </summary>
    public class EigenFrequencies : IResult
    {
        /// <summary>
        /// Shape Number Identified
        /// </summary>
        public int ShapeId  { get; }

        /// <summary>
        /// Frequency [Hz]
        /// </summary>
        public double Frequency { get; }

        /// <summary>
        /// Period [s]
        /// </summary>
        public double Period    { get; }

        /// <summary>
        /// ModalMass [ton]
        /// </summary>
        public double ModalMass { get; }

        /// <summary>
        /// MassParticipant in X direction [%]
        /// </summary>
        public double MassParticipantXi { get; }

        /// <summary>
        /// MassParticipant in X direction [%]
        /// </summary>
        public double MassParticipantYi { get; }

        /// <summary>
        /// MassParticipant in X direction [%]
        /// </summary>
        public double MassParticipantZi { get; }
    

        internal EigenFrequencies(int shapeId, double frequency, double period, double modalMass, double massPartXi, double massPartYi, double massPartZi)
        {
            this.ShapeId = shapeId;
            this.Frequency = frequency;
            this.Period = period;
            this.ModalMass = modalMass;
            this.MassParticipantXi = massPartXi;
            this.MassParticipantYi = massPartYi;
            this.MassParticipantZi = massPartZi;
        }

        public override string ToString()
        {
            return $"{base.ToString()}, {ShapeId}, {Frequency}";
        }
        internal static Regex IdentificationExpression
        {
            get
            {
                return new Regex(@"(?'type'Eigenfrequencies)");
            }
        }

        internal static Regex HeaderExpression
        {
            get
            {
                return new Regex(@"(?'type'Eigenfrequencies)|Shape\tFrequency|\[.*\]");
            }
        }

        internal static EigenFrequencies Parse(string[] row, CsvParser reader, Dictionary<string, string> HeaderData)
        {
            int shapeId = Int16.Parse(row[0], CultureInfo.InvariantCulture);
            double frequency = Double.Parse(row[1], CultureInfo.InvariantCulture);
            double period = Double.Parse(row[2], CultureInfo.InvariantCulture);
            double modalMass = Double.Parse(row[3], CultureInfo.InvariantCulture);
            double massPartXi = Double.Parse(row[4], CultureInfo.InvariantCulture);
            double massPartYi = Double.Parse(row[5], CultureInfo.InvariantCulture);
            double massPartZi = Double.Parse(row[6], CultureInfo.InvariantCulture);
            return new EigenFrequencies(shapeId, frequency, period, modalMass, massPartXi, massPartYi, massPartZi);
        }
    }
}