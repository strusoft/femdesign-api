using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using FemDesign;


namespace FemDesign.Samples
{
    public partial class SampleProgram
    {
        /// <summary>
        /// NOTE: CHANGE THIS TO YOUR LOCAL MATERIALS
        /// </summary>
        static readonly string MaterialsPath = @"C:\Users\SamuelNyberg\OneDrive - StruSoft AB\Samuels arbetshörna\materialochsektioner\materials.struxml";

        /// <summary>
        /// NOTE: CHANGE THIS TO YOUR LOCAL SECTIONS
        /// </summary>
        static readonly string SectionsPath = @"C:\Users\SamuelNyberg\OneDrive - StruSoft AB\Samuels arbetshörna\materialochsektioner\sections.struxml";

        private static void Main(string[] args)
        {
            string path = @"C:\Users\AlexanderRadne\OneDrive - StruSoft AB\Desktop\No cracking.str";
            var (model, results) = Model.ReadStr(path, new List<Results.ResultType> { Results.ResultType.RCDesignShellCracking });

            foreach (var result in results)
            {
                Console.WriteLine(result);
            }
        }
    }
}
