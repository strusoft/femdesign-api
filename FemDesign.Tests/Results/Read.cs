using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FemDesign.Results
{
    [TestClass]
    public class Read
    {
        [TestCategory("FEM-Design required")]
        [TestMethod("ReadResults")]
        public void ReadResults()
        {
            dynamic results;
            var filePath = @"Results/Assets/Model.str";

            using(var femdesign = new FemDesign.FemDesignConnection())
            {
                femdesign.Open(filePath);


                // Finite element
                results = femdesign.GetResults<FemNode>();
                results = femdesign.GetResults<FemBar>();
                results = femdesign.GetResults<FemShell>();

                results = femdesign.GetResults<NodalDisplacement>();


                // RC
                results = femdesign.GetResults<RCBarUtilization>();
                results = femdesign.GetResults<RCShellCrackWidth>();
                results = femdesign.GetResults<RCShellDesignForces>();
                results = femdesign.GetResults<RCShellReinforcementRequired>();
                results = femdesign.GetResults<RCShellShearCapacity>();
                results = femdesign.GetResults<RCShellShearUtilization>();
                results = femdesign.GetResults<RCShellUtilization>();

                // Shell
                results = femdesign.GetResults<ShellDerivedForce>();
                results = femdesign.GetResults<ShellDisplacement>();
                results = femdesign.GetResults<ShellInternalForce>();
                results = femdesign.GetResults<ShellStress>();

                // Stability
                results = femdesign.GetResults<CriticalParameter>();
                results = femdesign.GetResults<ImperfectionFactor>();
                results = femdesign.GetResults<NodalBucklingShape>();

                // Dynamic
                results = femdesign.GetResults<NodalVibration>();
                results = femdesign.GetResults<EigenFrequencies>();

                // Timber
                results = femdesign.GetResults<BarTimberUtilization>();
                results = femdesign.GetResults<CLTShellUtilization>();
                results = femdesign.GetResults<CLTFireUtilization>();

                // Bar
                results = femdesign.GetResults<BarDisplacement>();
                results = femdesign.GetResults<BarEndForce>();
                results = femdesign.GetResults<BarInternalForce>();
                results = femdesign.GetResults<BarStress>();

                results = femdesign.GetResults<LabelledSectionInternalForce>();
                results = femdesign.GetResults<LabelledSectionResultant>();

                // Support
                results = femdesign.GetResults<PointSupportReaction>();
                results = femdesign.GetResults<PointSupportReactionMinMax>();

                results = femdesign.GetResults<LineSupportReaction>();
                results = femdesign.GetResults<LineSupportResultant>();

                results = femdesign.GetResults<SurfaceSupportReaction>();
                results = femdesign.GetResults<SurfaceSupportResultant>();

                // Connection
                results = femdesign.GetResults<PointConnectionForce>();

                results = femdesign.GetResults<LineConnectionForce>();
                results = femdesign.GetResults<LineConnectionResultant>();


                // Quantity
                results = femdesign.GetResults<QuantityEstimationConcrete>();
                results = femdesign.GetResults<QuantityEstimationReinforcement>();
                results = femdesign.GetResults<QuantityEstimationSteel>();
                results = femdesign.GetResults<QuantityEstimationTimber>();
                results = femdesign.GetResults<QuantityEstimationGeneral>();
                results = femdesign.GetResults<QuantityEstimationMasonry>();
                // crashes FEM-Design
                //results = femdesign.GetResults<QuantityEstimationTimberPanel>();
                results = femdesign.GetResults<QuantityEstimationProfiledPlate>();

            }
        }

        [TestMethod]
        [TestCategory("FEM-Design required")]
        public void GetResultMethodTests()
        {
            string filepath = "Results\\Assets\\Model.str";

            using (var connection = new FemDesignConnection())
            {
                connection.Open(filepath);

                var model = connection.GetModel();
                var bars = model.Entities.Bars;
                var elements = new List<FemDesign.Bars.Bar> { bars[1], bars[2] };
                var elemIds = elements.Select(e => e.BarPart.Name).ToList();

                var loads = connection.GetLoads();
                var loadCases = loads.LoadCases.Select(c => c.Name).ToList();
                var loadCombs = loads.LoadCombinations.Select(c => c.Name).ToList();

                //------------------------------------------------------------------
                // Test GetResults() method
                //------------------------------------------------------------------

                // Get all of the BarDisplacement results
                var allRes = connection.GetResults<BarDisplacement>().OrderBy(r => r.Id).ToList();
                Assert.IsTrue(allRes.Count != 0);

                List<BarDisplacement> filteredAllRes = new List<BarDisplacement>();
                foreach (var id in elemIds)
                {
                    var filteredRes = allRes.Where(r => r.Id == id).ToList();
                    filteredAllRes.AddRange(filteredRes);
                }
                filteredAllRes = filteredAllRes.OrderBy(r => r.Pos).OrderBy(r => r.CaseIdentifier).OrderBy(r => r.Id).ToList();


                // Get all of the BarDisplacement results by structural elements
                var structElements = elements.Select(e => (FemDesign.GenericClasses.IStructureElement)e).ToList();
                var allResByElements = connection.GetResults<BarDisplacement>(elements: structElements).OrderBy(r => r.Pos).OrderBy(r => r.CaseIdentifier).OrderBy(r => r.Id).ToList();
                Assert.IsTrue(allRes.Count != 0);


                // Check
                Assert.AreEqual(allResByElements.Count, filteredAllRes.Count);
                for (int i = 0; i < allResByElements.Count; i++)
                {
                    PropertyInfo[] properties = typeof(BarDisplacement).GetProperties();
                    properties = properties.Where(p => p.Name != nameof(BarDisplacement.CaseIdentifier)).ToArray();

                    foreach (var prop in properties)
                    {
                        var item1 = prop.GetValue(allResByElements[i]);
                        var item2 = prop.GetValue(filteredAllRes[i]);
                        Assert.AreEqual(item1, item2);
                    }
                    string caseId1 = allResByElements[i].CaseIdentifier.Replace(" - selected objects", "");
                    string caseId2 = filteredAllRes[i].CaseIdentifier;
                    Assert.AreEqual(caseId1, caseId2);
                }




            }
        }
    }
}
