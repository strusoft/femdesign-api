using FemDesign.Calculate;
using FemDesign.Loads;
using FemDesign.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FemDesign.Examples
{
    internal class Program
    {
        static void Main()
        {
            // EXAMPLE 7: Iterative analysis
            // This example will show you how to iterate using only one instance of FEM-Design.

            // This example was last updated using the ver. 22.11.0 FEM-Design API.

            var filepath = "iterative-analysis.struxml";
            var model = Model.DeserializeFromFilePath(filepath);

            var analysis = Analysis.StaticAnalysis();
            var unit = new UnitResults(Length.mm, Angle.rad, SectionalData.mm, Force.kN, Mass.kg, Displacement.mm, Stress.MPa);

            using(var femDesign = new FemDesign.FemDesignConnection("C:\\Program Files\\StruSoft\\FEM-Design 23 Night Install"))
            {                             
                for(double i = 0; i < 1; i += 0.1)
                {
                    // Reference the model
                    var newModel = model.DeepClone();

                    // Create additional support and add it to the model
                    var point = model.Entities.Bars[0].Edge.GetIntermediatePoint(i);
                    var support = Supports.PointSupport.Hinged(point);
                    newModel.AddElements(support);

                    // FEM-Design analysis
                    femDesign.Open(newModel);
                    femDesign.RunAnalysis(analysis);

                    // Results
                    var displacements = femDesign.GetLoadCaseResults<NodalDisplacement>(loadCase: null, unit);
                    var maxDisp = displacements.Select(x => Math.Abs(x.Ez)).Max();
                    // Print results
                    Console.WriteLine();
                    Console.WriteLine($"MaxDisp: {maxDisp}, length location: {i * 100} [%]");

                }
            }
        }
    }
}