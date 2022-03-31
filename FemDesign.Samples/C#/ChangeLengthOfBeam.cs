using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FemDesign.Samples
{
    public partial class SampleProgram
    
    {
        private static void ChangeLengthOfBeam()
        {
            string struxmlPath = @"C:\Users\JohannaRiad\OneDrive - StruSoft AB\Desktop\Webinar - intro FEM design API\exbeam.struxml";
            string struxmlPathOut = @"C:\Users\JohannaRiad\OneDrive - StruSoft AB\Desktop\Webinar - intro FEM design API\exbeam_out.struxml";
            Model model = Model.DeserializeFromFilePath(struxmlPath);

            Bars.Bar exbeam = model.Entities.Bars[0];
            exbeam.BarPart.Edge.Points[1].X = 35;
            

            Supports.PointSupport support = model.Entities.Supports.PointSupport[1];
            support.Position.X = 35;

            model.SerializeModel(struxmlPathOut);
        }

    }
}
