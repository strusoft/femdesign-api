using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

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
    }
}
