using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using FemDesign.Loads;
using System.Text;
using System.Threading.Tasks;

namespace FemDesign.Loads
{
    [TestClass()]
    public class LoadGroupsCombineTests
    {
        [TestMethod()]
        public void GetGammasLC1Test()
        {
            // Arrange
            List<double> gammasExpected = new List<double>() { 1.35, 1.35 };

            // Act
            List<LoadCombination> loadCombinations = CreateLoadCombinations();

            // Assert
            List<double> actual_gammas = GetGammasLC1(loadCombinations);
            CollectionAssert.AreEqual(gammasExpected, actual_gammas, "LC1 coefficients are inncorrect");
        }

        [TestMethod()]
        public void GetGammasLC2Test()
        {
            // Arrange
            List<double> gammasExpected = new List<double>() { 1.2015, 1.2015, 1.5, 0.44999999999999996 };

            // Act
            List<LoadCombination> loadCombinations = CreateLoadCombinations();

            // Assert
            List<double> actual_gammas = GetGammasLC2(loadCombinations);
            CollectionAssert.AreEqual(gammasExpected, actual_gammas, "LC2 coefficients are inncorrect");
        }

        [TestMethod()]
        public void GetGammasLC5Test()
        {
            // Arrange
            List<double> gammasExpected = new List<double>() { 1.2015, 1.2015, 1.5, 1.0499999999999998 };

            // Act
            List<LoadCombination> loadCombinations = CreateLoadCombinations();

            // Assert
            List<double> actual_gammas = GetGammasLC5(loadCombinations);
            CollectionAssert.AreEqual(gammasExpected, actual_gammas, "LC5 coefficients are inncorrect");
        }

        [TestMethod()]
        public void GetGammasLC9Test()
        {
            // Arrange
            List<double> gammasExpected = new List<double>() { 1, 1, 1, 0.3 };

            // Act
            List<LoadCombination> loadCombinations = CreateLoadCombinations();

            // Assert
            List<double> actual_gammas = GetGammasLC9(loadCombinations);
            CollectionAssert.AreEqual(gammasExpected, actual_gammas, "LC9 coefficients are inncorrect");
        }

        [TestMethod()]
        public void GetGammasLC13Test()
        {
            // Arrange
            List<double> gammasExpected = new List<double>() { 1, 1, 1, 0.7 };

            // Act
            List<LoadCombination> loadCombinations = CreateLoadCombinations();

            // Assert
            List<double> actual_gammas = GetGammasLC13(loadCombinations);
            CollectionAssert.AreEqual(gammasExpected, actual_gammas, "LC13 coefficients are inncorrect");
        }

        [TestMethod()]
        public void GetGammasLC17Test()
        {
            // Arrange
            List<double> gammasExpected = new List<double>() { 1, 1, 0.5 };

            // Act
            List<LoadCombination> loadCombinations = CreateLoadCombinations();

            // Assert
            List<double> actual_gammas = GetGammasLC17(loadCombinations);
            CollectionAssert.AreEqual(gammasExpected, actual_gammas, "LC17 coefficients are inncorrect");
        }

        [TestMethod()]
        public void GetGammasLC19Test()
        {
            // Arrange
            List<double> gammasExpected = new List<double>() { 1, 1, 0.2, 0.3 };

            // Act
            List<LoadCombination> loadCombinations = CreateLoadCombinations();

            // Assert
            List<double> actual_gammas = GetGammasLC19(loadCombinations);
            CollectionAssert.AreEqual(gammasExpected, actual_gammas, "LC19 coefficients are inncorrect");
        }

        [TestMethod()]
        public void GetGammasLC23Test()
        {
            // Arrange
            List<double> gammasExpected = new List<double>() { 1, 1, 0.3 };

            // Act
            List<LoadCombination> loadCombinations = CreateLoadCombinations();

            // Assert
            List<double> actual_gammas = GetGammasLC23(loadCombinations);
            CollectionAssert.AreEqual(gammasExpected, actual_gammas, "LC23 coefficients are inncorrect");
        }

        private static List<double> GetGammasLC1(List<LoadCombination> loadCombinations)
        {
            List<double> gammasLC1 = new List<double>();
            gammasLC1 = loadCombinations[0].ModelLoadCase.Select(o => o.Gamma).ToList();
            return gammasLC1;
        }

        private static List<double> GetGammasLC2(List<LoadCombination> loadCombinations)
        {
            List<double> gammasLC2 = new List<double>();
            gammasLC2 = loadCombinations[1].ModelLoadCase.Select(o => o.Gamma).ToList();
            return gammasLC2;

        }

        private static List<double> GetGammasLC5(List<LoadCombination> loadCombinations)
        {
            List<double> gammas = new List<double>();
            gammas = loadCombinations[5].ModelLoadCase.Select(o => o.Gamma).ToList();
            return gammas;

        }

        private static List<double> GetGammasLC9(List<LoadCombination> loadCombinations)
        {
            List<double> gammas = new List<double>();
            gammas = loadCombinations[9].ModelLoadCase.Select(o => o.Gamma).ToList();
            return gammas;

        }

        private static List<double> GetGammasLC13(List<LoadCombination> loadCombinations)
        {
            List<double> gammas = new List<double>();
            gammas = loadCombinations[13].ModelLoadCase.Select(o => o.Gamma).ToList();
            return gammas;

        }

        private static List<double> GetGammasLC17(List<LoadCombination> loadCombinations)
        {
            List<double> gammas = new List<double>();
            gammas = loadCombinations[17].ModelLoadCase.Select(o => o.Gamma).ToList();
            return gammas;

        }

        private static List<double> GetGammasLC19(List<LoadCombination> loadCombinations)
        {
            List<double> gammas = new List<double>();
            gammas = loadCombinations[19].ModelLoadCase.Select(o => o.Gamma).ToList();
            return gammas;

        }

        private static List<double> GetGammasLC23(List<LoadCombination> loadCombinations)
        {
            List<double> gammas = new List<double>();
            gammas = loadCombinations[23].ModelLoadCase.Select(o => o.Gamma).ToList();
            return gammas;

        }

        private List<LoadCombination> CreateLoadCombinations()
        {
            // Create load cases
            LoadCase deadLoad1 = new LoadCase("Deadload1", LoadCaseType.DeadLoad, LoadCaseDuration.Permanent);
            LoadCase deadLoad2 = new LoadCase("Deadload2", LoadCaseType.DeadLoad, LoadCaseDuration.Permanent);
            LoadCase liveLoad1 = new LoadCase("Liveload1", LoadCaseType.Static, LoadCaseDuration.Permanent);
            LoadCase liveLoad2 = new LoadCase("Liveload2", LoadCaseType.Static, LoadCaseDuration.Permanent);
            LoadCase windLoad1 = new LoadCase("Windload1", LoadCaseType.Static, LoadCaseDuration.Permanent);
            LoadCase windLoad2 = new LoadCase("Windload2", LoadCaseType.Static, LoadCaseDuration.Permanent);
            List<LoadCase> loadCasesDeadLoads = new List<LoadCase>() { deadLoad1, deadLoad2 };
            List<LoadCase> loadCaseCategoryA = new List<LoadCase>() { liveLoad1, liveLoad2 };
            List<LoadCase> loadCaseCategoryWind = new List<LoadCase>() { windLoad1, windLoad2 };
            List<LoadCase> loadCases = loadCasesDeadLoads.Concat(loadCaseCategoryA).Concat(loadCaseCategoryWind).ToList();

            // Get the load categories that hold the coefficients
            var loadCategoryDatabase = LoadCategoryDatabase.GetDefault();
            LoadCategory loadCategoryA = loadCategoryDatabase.LoadCategoryByName("A");
            LoadCategory loadCategoryWind = loadCategoryDatabase.LoadCategoryByName("Wind");

            // Create load groups
            var LGPermanent = new LoadGroupPermanent(1, 1.35, 1, 1, loadCasesDeadLoads, ELoadGroupRelationship.Entire, 0.89, "LGPermanent");
            var LGA = new LoadGroupTemporary(1.5, loadCategoryA.Psi0, loadCategoryA.Psi1, loadCategoryA.Psi2, true, loadCaseCategoryA, ELoadGroupRelationship.Alternative, "LGCategoryA");
            var LGWind = new LoadGroupTemporary(1.5, loadCategoryWind.Psi0, loadCategoryWind.Psi1, loadCategoryWind.Psi2, true, loadCaseCategoryWind, ELoadGroupRelationship.Alternative, "LGCategoryWind");

            var loadGroups = new List<LoadGroupBase>() { LGPermanent, LGA, LGWind };

            // Generate ULS and SLS Combinations
            LoadCombinationTable loadCombinationTable = new LoadCombinationTable();
            FemDesign.Examples.Program.CombineULS(loadGroups, loadCombinationTable);
            FemDesign.Examples.Program.CombineSLS(loadGroups, loadCombinationTable);
            return loadCombinationTable.LoadCombinations;
        }
    }
}