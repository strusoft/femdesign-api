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
            var lib = sections.Where(s => s.MaterialFamily == "Steel").ToList().GetRange(0,20);

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
            var concreteConfig = new Calculate.ConcreteConfig(ConcreteConfig.CalculationMethod.NominalCurvature, true, false, true);
            concreteConfig.ReopeningCracks = true;

            //var cmdConfig = new CmdConfig(configPath, ecst, steelDesign, steelConfig, concreteConfig);
            var cmdConfig = new CmdConfig(configPath, concreteConfig);

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