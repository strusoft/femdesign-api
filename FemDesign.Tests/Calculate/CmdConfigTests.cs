using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FemDesign;
using FemDesign.Calculate;
using System.Xml.Serialization;
using System.IO;
using System.Net.Http.Headers;

namespace FemDesign.Calculate
{
    [TestClass()]
    public partial class CmdConfigTest
    {
        [TestMethod("CreateConfig")]
        public void CreateConfigSteel()
        {
            string fdScriptPath = "Calculate//script.fdscript";
            string configPath = "Calculate//config.xml";
            var logPath = "Calculate//logfile.log";

            var sections = Sections.SectionDatabase.GetDefault().Sections.Section;
            var lib = new List<Guid>
            {
                new Guid("16d7557c-87e7-4649-a302-df58a9810985"),
                new Guid("b8dcc4b1-48ab-42b1-b5d4-a3080f045b41"),
                new Guid("e26954f6-ee0c-481d-9bbb-94b69f102ab6"),
            };

            var ecst = new Calculate.SteelDesignConfiguration(SteelDesignConfiguration.Method.Method1);
            var steelDesign = new Calculate.SteelBarDesignParameters(0.7, lib);

            // Steel bar calculation parameters
            var steelConfig = new Calculate.SteelBarCalculationParameters();
            steelConfig.BucklingCurveFx1 = SteelBarCalculationParameters.BucklingCurve.Auto;
            steelConfig.BucklingCurveFx2 = SteelBarCalculationParameters.BucklingCurve.Auto;
            steelConfig.BucklingCurveTf = SteelBarCalculationParameters.BucklingCurve.Auto;
            steelConfig.BucklingCurveLtt = SteelBarCalculationParameters.BucklingCurveLt.Auto;
            steelConfig.BucklingCurveLtb = SteelBarCalculationParameters.BucklingCurveLt.Auto;
            steelConfig.DistanceCalculatedSection = 0.123;
            steelConfig.S2ndOrder = SteelBarCalculationParameters.SecondOrder.ConsiderIfAvailable;
            steelConfig.PlasticIgnored = true;
            steelConfig.UseEquation6_41 = true;
            steelConfig.Class4Ignored = true;
            steelConfig.CheckResistanceOnly = false;
            steelConfig.MaxIterStep = 90;
            steelConfig.ConvergencyRatio = 0.15;
            steelConfig.LatTorBuckGenSpecForI = true;
            steelConfig.LatTorBuckGen = true;

            // Concrete calculation parameters
            var concreteConfig = new Calculate.ConcreteDesignConfig(ConcreteDesignConfig.CalculationMethod.NominalCurvature, true, false, true);
            concreteConfig.ReopeningCracks = true;

            var cmdConfig = new CmdConfig(configPath, ecst, steelDesign, steelConfig, concreteConfig);

            var fdscript = new FdScript(logPath, cmdConfig);
            fdscript.Serialize(fdScriptPath);

            // read text from file path
            string fdscriptText = System.IO.File.ReadAllText(fdScriptPath);
            string configText = System.IO.File.ReadAllText(configPath);

            Console.WriteLine(fdscriptText);
            Console.WriteLine();
            Console.WriteLine(configText);
        }
    }
}