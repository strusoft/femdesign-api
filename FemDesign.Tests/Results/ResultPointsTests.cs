using Microsoft.VisualStudio.TestTools.UnitTesting;
using FemDesign.Results;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FemDesign.Calculate;

namespace FemDesign.Results
{
    [TestClass()]
    public class ResultPointsTests
    {
        [TestCategory("FEM-Design required")]
        [TestMethod]
        public void ResPoints()
        {
            string filepath = @"Assets/Slab.str";


            var connection = new FemDesign.FemDesignConnection();
            connection.Open(filepath);
            var newModel = connection.GetModel();
            var analysis = Analysis.StaticAnalysis();
            connection.RunAnalysis(analysis);

            var slab = newModel.Entities.Slabs[0];

            var pos = new FemDesign.Geometry.Point3d(2.5, 2.5, 0);
            var resultPoint = new List<CmdResultPoint> { new CmdResultPoint(pos, slab.Guid, "middlePoint_0") };
            connection.CreateResultPoint(resultPoint);

            pos = new FemDesign.Geometry.Point3d(5, 5, 0);
            resultPoint = new List<CmdResultPoint> { new CmdResultPoint(pos, slab.Guid, "middlePoint_1") };
            connection.CreateResultPoint(resultPoint);

            pos = new FemDesign.Geometry.Point3d(10, 10, 0);
            resultPoint = new List<CmdResultPoint> { new CmdResultPoint(pos, slab.Guid, "middlePoint_2") };
            connection.CreateResultPoint(resultPoint);


            var results = connection.GetLoadCaseResults<Results.ShellDisplacement>(options: new Options(BarResultPosition.ResultPoints, ShellResultPosition.ResultPoints) );

            Assert.IsNotNull(results);
            Assert.IsTrue(results.Count == 3);
        }
    }
}