using Microsoft.VisualStudio.TestTools.UnitTesting;
using FemDesign.GenericClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FemDesign.GenericClasses
{
    [TestClass()]
    public class EnumHelpersTests
    {
        [TestMethod("Parse Country")]
        public void ParseCountryTest()
        {
            Assert.IsTrue(EnumParser.Parse<Country>("S") == Country.S, "Should be able to parse country successfully.");
            Assert.IsTrue(EnumParser.Parse<Country>("s") == Country.S, "Should be able to parse country for lower case letters.");
            Assert.IsTrue(EnumParser.Parse<Country>("Sweden") == Country.S, "Should be able to parse country from full country name.");
            Assert.IsTrue(EnumParser.Parse<Country>("sweden") == Country.S, "Should be able to parse country from full country name.");
            Assert.ThrowsException<ArgumentException>(() => EnumParser.Parse<Country>("X"), "Should throw exception on invalid country code.");

            foreach (string countryCode in new string[] { "D", "DK", "EST", "FIN", "GB", "H", "LT", "N", "NL", "PL", "RO", "S", "TR"})
                try {
                    EnumParser.Parse<Country>(countryCode);
                } catch (Exception) {
                    Assert.Fail($"Should be able to parse country \"{countryCode}\" successfully");
                }
        }

        [TestMethod("Parse Activation Type")]
        public void ParseLoadActivation()
		{
            Assert.IsTrue(EnumParser.Parse<ActivationType>("0") == ActivationType.OnlyInThisStage);
            Assert.IsTrue(EnumParser.Parse<ActivationType>("1") == ActivationType.FromThisStageOn);
            Assert.IsTrue(EnumParser.Parse<ActivationType>("2") == ActivationType.ShiftedFromFirstStage);
            Assert.IsTrue(EnumParser.Parse<ActivationType>("3") == ActivationType.OnlyStageActivatedElements);

            Assert.IsTrue(EnumParser.Parse<ActivationType>("only_in_this_stage") == ActivationType.OnlyInThisStage);
            Assert.IsTrue(EnumParser.Parse<ActivationType>("only_stage_activated_elem") == ActivationType.OnlyStageActivatedElements);
        }
    }
}