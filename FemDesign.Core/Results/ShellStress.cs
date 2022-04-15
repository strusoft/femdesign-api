using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FemDesign.Results
{
    /// <summary>
    /// FemDesign "Shells, Stresses" result
    /// </summary>
    public class ShellStress : IResult
    {
        /// <summary>
        /// Shell name identifier
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Finite element id
        /// </summary>
        public int ElementId { get; }

        /// <summary>
        /// Finite element node id
        /// </summary>
        public int NodeId { get; }

        /// <summary>
        /// Normal Stress in the local X direction [N/mm2]
        /// </summary>
        public double SigmaX { get; }

        /// <summary>
        /// Normal Stress in in the local Y direction [N/mm2]
        /// </summary>
        public double SigmaY { get; }

        /// <summary>
        /// Tangential Stress in XY plane [N/mm2]
        /// </summary>
        public double TauXY { get; }

        /// <summary>
        /// Tangential Stress in XZ plane [N/mm2]
        /// </summary>
        public double TauXZ { get; }

        /// <summary>
        /// Tangential Stress in YZ plane [N/mm2]
        /// </summary>
        public double TauYZ { get; }

        /// <summary>
        /// VonMises Stress [N/mm2]
        /// </summary>
        public double SigmaVM { get; }

        /// <summary>
        /// Principal Stress Value - First direction [N/mm2]
        /// </summary>
        public double Sigma1 { get; }

        /// <summary>
        /// Principal Stress Value - Second direction [N/mm2]
        /// </summary>
        public double Sigma2 { get; }

        /// <summary>
        /// Angle between the local X axis and the direction
        /// of the principla stress Sigma1 [rad]
        /// </summary>
        public double Alpha { get; }

        /// <summary>
        /// Load case or combination name
        /// </summary>
        public string CaseIdentifier { get; }

        internal ShellStress(string id, int elementId, int nodeId, double sigmaX, double sigmaY, double tauXY, double tauXZ, double tauYZ, double sigmaVM, double sigma1, double sigma2, double alpha, string caseIdentifier)
        {
            this.Id = id;
            this.ElementId = elementId;
            this.NodeId = nodeId;
            this.SigmaX = sigmaX;
            this.SigmaY = sigmaY;
            this.TauXY = tauXY;
            this.TauXZ = tauXZ;
            this.TauYZ = tauYZ;
            this.SigmaVM = sigmaVM;
            this.Sigma1 = sigma1;
            this.Sigma2 = sigma2;
            this.Alpha = alpha;
            this.CaseIdentifier = caseIdentifier;
        }

        public override string ToString()
        {
            return $"{base.ToString()}, {Id}, {CaseIdentifier}";
        }

        internal static Regex IdentificationExpression
        {
            get
            {
                return new Regex(@"(?'type'Shells, Stresses), (?'side'(top \(Extract\)|membrane \(Extract\)|bottom \(Extract\)|top|membrane|bottom)), ((?'loadcasetype'[\w\ ]+)? - )?Load (?'casecomb'case|comb\.+): (?'casename'[ -#%'-;=?A-\ufffd]{1,79})");
            }
        }

        internal static Regex HeaderExpression
        {
            get
            {
                return new Regex(@"(?'type'Shells, Stresses), (?'side'(top \(Extract\)|membrane \(Extract\)|bottom \(Extract\)|top|membrane|bottom)), ((?'loadcasetype'[\w\ ]+)? - )?Load (?'casecomb'case|comb\.+): (?'casename'[ -#%'-;=?A-\ufffd]{1,79})$|Shell(\tElem|\tMax)|\[.*\]");
            }
        }

        internal static ShellStress Parse(string[] row, CsvParser reader, Dictionary<string, string> HeaderData)
        {
            if (row.Count() == 14) // Extract
            {
                string id = row[0];
                int elementId = Int32.Parse(row[2], CultureInfo.InvariantCulture);
                int nodeId = Int32.Parse(row[3], CultureInfo.InvariantCulture);
                double sigmaX = Double.Parse(row[4], CultureInfo.InvariantCulture);
                double sigmaY = Double.Parse(row[5], CultureInfo.InvariantCulture);
                double tauXY = Double.Parse(row[6], CultureInfo.InvariantCulture);
                double tauXZ = Double.Parse(row[7], CultureInfo.InvariantCulture);
                double tauYZ = Double.Parse(row[8], CultureInfo.InvariantCulture);
                double sigmaVM = Double.Parse(row[9], CultureInfo.InvariantCulture);
                double sigma1 = Double.Parse(row[10], CultureInfo.InvariantCulture);
                double sigma2 = Double.Parse(row[11], CultureInfo.InvariantCulture);
                double alpha = Double.Parse(row[12], CultureInfo.InvariantCulture);
                string caseIdentifier = row[13];
                return new ShellStress(id, elementId, nodeId, sigmaX, sigmaY, tauXY, tauXZ, tauYZ, sigmaVM, sigma1, sigma2, alpha, caseIdentifier);
            }
            else
            {
                string id = row[0];
                int elementId = Int32.Parse(row[1], CultureInfo.InvariantCulture);
                int nodeId = Int32.Parse(row[2], CultureInfo.InvariantCulture);
                double sigmaX = Double.Parse(row[3], CultureInfo.InvariantCulture);
                double sigmaY = Double.Parse(row[4], CultureInfo.InvariantCulture);
                double tauXY = Double.Parse(row[5], CultureInfo.InvariantCulture);
                double tauXZ = Double.Parse(row[6], CultureInfo.InvariantCulture);
                double tauYZ = Double.Parse(row[7], CultureInfo.InvariantCulture);
                double sigmaVM = Double.Parse(row[8], CultureInfo.InvariantCulture);
                double sigma1 = Double.Parse(row[9], CultureInfo.InvariantCulture);
                double sigma2 = Double.Parse(row[10], CultureInfo.InvariantCulture);
                double alpha = Double.Parse(row[11], CultureInfo.InvariantCulture);
                string caseIdentifier = HeaderData["casename"];
                return new ShellStress(id, elementId, nodeId, sigmaX, sigmaY, tauXY, tauXZ, tauYZ, sigmaVM, sigma1, sigma2, alpha, caseIdentifier);
            }
        }
    }
}
