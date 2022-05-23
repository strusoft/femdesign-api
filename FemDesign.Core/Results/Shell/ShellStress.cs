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
        public string NodeId { get; }

        /// <summary>
        /// Normal Stress in the local X direction
        /// </summary>
        public double SigmaX { get; }

        /// <summary>
        /// Normal Stress in in the local Y direction
        /// </summary>
        public double SigmaY { get; }

        /// <summary>
        /// Tangential Stress in XY plane
        /// </summary>
        public double TauXY { get; }

        /// <summary>
        /// Tangential Stress in XZ plane
        /// </summary>
        public double TauXZ { get; }

        /// <summary>
        /// Tangential Stress in YZ plane
        /// </summary>
        public double TauYZ { get; }

        /// <summary>
        /// VonMises Stress [N/mm2]
        /// </summary>
        public double SigmaVM { get; }

        /// <summary>
        /// Principal Stress Value - First direction
        /// </summary>
        public double Sigma1 { get; }

        /// <summary>
        /// Principal Stress Value - Second direction
        /// </summary>
        public double Sigma2 { get; }

        /// <summary>
        /// Angle between the local X axis and the direction
        /// of the principle stress Sigma1 [rad]
        /// </summary>
        public double Alpha { get; }

        /// <summary>
        /// Load case or combination name
        /// </summary>
        public string CaseIdentifier { get; }

        internal ShellStress(string id, int elementId, string nodeId, double sigmaX, double sigmaY, double tauXY, double tauXZ, double tauYZ, double sigmaVM, double sigma1, double sigma2, double alpha, string caseIdentifier)
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
                return new Regex(@"(?'type'Shells, Stresses), (?'side'top|bottom|membrane) ?(?'extract'\(Extract\))?, .* Load (?'casecomb'case|comb.+): (?'casename'[ -#%'-;=?A-\ufffd]{1,79})");
            }
        }

        internal static Regex HeaderExpression
        {
            get
            {
                return new Regex(@"(?'type'Shells, Stresses), (?'side'top|bottom|membrane) ?(?'extract'\(Extract\))?, (?'loadcasetype'[\w\.\-]+).* Load (?'casecomb'case|comb.+): (?'casename'[ -#%'-;=?A-\ufffd]{1,79})$|Shell\t(Elem|Max\.).*|\[.*\]");
            }
        }

        internal static ShellStress Parse(string[] row, CsvParser reader, Dictionary<string, string> HeaderData)
        {
            if (row.Count() == 14) // Extract
            {
                string id = row[0];
                int elementId = Int32.Parse(row[2], CultureInfo.InvariantCulture);
                // Depending on the output option, some values are not specified as integer but as [-].
                string nodeId = row[3];
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
                //to be implemented
                string side = HeaderData["side"];
                return new ShellStress(id, elementId, nodeId, sigmaX, sigmaY, tauXY, tauXZ, tauYZ, sigmaVM, sigma1, sigma2, alpha, caseIdentifier);
            }
            else
            {
                string id = row[0];
                int elementId = Int32.Parse(row[1], CultureInfo.InvariantCulture);
                // Depending on the output option, some values are not specified as integer but as [-].
                string nodeId = row[2];
                double sigmaX = Double.Parse(row[3], CultureInfo.InvariantCulture);
                double sigmaY = Double.Parse(row[4], CultureInfo.InvariantCulture);
                double tauXY = Double.Parse(row[5], CultureInfo.InvariantCulture);
                double tauXZ = Double.Parse(row[6], CultureInfo.InvariantCulture);
                double tauYZ = Double.Parse(row[7], CultureInfo.InvariantCulture);
                double sigmaVM = Double.Parse(row[8], CultureInfo.InvariantCulture);
                double sigma1 = Double.Parse(row[9], CultureInfo.InvariantCulture);
                double sigma2 = Double.Parse(row[10], CultureInfo.InvariantCulture);
                double alpha = Double.Parse(row[11], CultureInfo.InvariantCulture);
                string caseIdentifier = row[12];
                //to be implemented
                string side = HeaderData["side"];
                return new ShellStress(id, elementId, nodeId, sigmaX, sigmaY, tauXY, tauXZ, tauYZ, sigmaVM, sigma1, sigma2, alpha, caseIdentifier);
            }
        }


        /// <summary>
        /// The method has been created for returning the value for Grasshopper and Dynamo.
        /// The method can still be use for C# users.
        /// </summary>
        public static Dictionary<string, object> DeconstructShellStress(List<FemDesign.Results.ShellStress> Result, string LoadCase)
        {
            var shellStress = Result.Cast<FemDesign.Results.ShellStress>();

            // Return the unique load case - load combination
            var uniqueLoadCases = shellStress.Select(n => n.CaseIdentifier).Distinct().ToList();

            // Select a Default load case if the user does not provide an input
            LoadCase = LoadCase == null ? uniqueLoadCases.First() : LoadCase;

            // Select the Nodal Displacement for the selected Load Case - Load Combination
            if (uniqueLoadCases.Contains(LoadCase, StringComparer.OrdinalIgnoreCase))
            {
                shellStress = shellStress.Where(n => String.Equals(n.CaseIdentifier, LoadCase, StringComparison.OrdinalIgnoreCase));
            }
            else
            {
                var warning = $"Load Case '{LoadCase}' does not exist";
                throw new ArgumentException(warning);
            }

            // Parse Results from the object
            var identifier = new List<string>();
            var elementId = new List<int>();
            var nodeId = new List<string>();

            var sigmaX = new List<double>();
            var sigmaY = new List<double>();
            var tauXY = new List<double>();
            var tauXZ = new List<double>();
            var tauYZ = new List<double>();
            var sigmaVM = new List<double>();
            var sigma1 = new List<double>();
            var sigma2 = new List<double>();
            var alpha = new List<double>();


            var uniqueLoadCase = shellStress.Select(n => n.CaseIdentifier).Distinct().ToList();

            foreach (var shellResult in shellStress)
            {
                // FemDesign Return also a value in the center of the shell
                // The output is not necessary in this case as the user can compute the value
                // doing an average.
                if (shellResult.NodeId != "-")
                {
                    identifier.Add(shellResult.Id);
                    elementId.Add(shellResult.ElementId);
                    nodeId.Add(shellResult.NodeId);

                    sigmaX.Add(shellResult.SigmaX);
                    sigmaY.Add(shellResult.SigmaY);
                    tauXY.Add(shellResult.TauXY);
                    tauXZ.Add(shellResult.TauXZ);
                    tauYZ.Add(shellResult.TauYZ);
                    sigmaVM.Add(shellResult.SigmaVM);
                    sigma1.Add(shellResult.Sigma1);
                    sigma2.Add(shellResult.Sigma2);
                    alpha.Add(shellResult.Alpha);
                }
            }


            return new Dictionary<string, dynamic>
            {
                {"CaseIdentifier", uniqueLoadCase},
                {"Identifier", identifier},
                {"ElementId", elementId},
                {"NodeId", nodeId},
                {"SigmaX",sigmaX},
                {"SigmaY",sigmaY},
                {"TauXY",tauXY},
                {"TauXZ",tauXZ},
                {"TauYZ",tauYZ},
                {"SigmaVM",sigmaVM},
                {"Sigma1",sigma1},
                {"Sigma2",sigma2},
                {"Alpha",alpha},
            };
        }
    }
}
